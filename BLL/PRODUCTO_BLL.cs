using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace BLL
{
    public class PRODUCTO_BLL
    {
        MAPPER_PRODUCTO GestorProducto = new MAPPER_PRODUCTO();
        MAPPER_TAMANIO GestorTamanio = new MAPPER_TAMANIO();
        DIGITOVERIFICADOR_BLL dvBLL = new DIGITOVERIFICADOR_BLL();
        CONTROLCAMBIO_BLL cambiosBLL = new CONTROLCAMBIO_BLL();
        // Error que lanza un SP con RAISERROR (nombre duplicado, tamaño con ventas)
        private const int ErrorDeNegocioSql = 50000;
        // Tamaño de los piqueos (decisión 3)
        private const string TamanioUnico = "Único";

        // Valores de PRODUCTO.Tipo (CHECK CK_PRODUCTO_Tipo en script.sql)
        public string[] ListarTiposProducto()
        {
            return new[] { "Bebida", "Piqueo" };
        }

        // CU006 paso 2 / FA3: tamaños activos que corresponden al tipo. Piqueo: solo "Único" (decisión 3); Bebida: el resto.
        public List<BE.TAMANIO> ListarTamaniosPorTipo(string tipo)
        {
            var tamanios = GestorTamanio.ListarTamaniosActivos();
            if (tipo == "Piqueo")
                return tamanios.Where(t => t.Nombre == TamanioUnico).ToList();
            if (tipo == "Bebida")
                return tamanios.Where(t => t.Nombre != TamanioUnico).ToList();
            return new List<BE.TAMANIO>();
        }

        // CU008: tamaños y precios actuales del producto
        public List<BE.PRODUCTO_TAMANIO> ObtenerTamaniosProducto(int idProducto)
        {
            return GestorProducto.ListarPreciosPorProducto(idProducto);
        }

        // CU006 paso 4 / FA1. Las excepciones llevan la clave de idioma del mensaje.
        // El nombre duplicado (FA2) lo rechaza el SP.
        private void ValidarProducto(BE.PRODUCTO producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("msgProductoNombreObligatorio");
            producto.Nombre = producto.Nombre.Trim();
            if (producto.Nombre.Length > 100)
                throw new ArgumentException("msgProductoNombreLargo");
            if (Array.IndexOf(ListarTiposProducto(), producto.Tipo) < 0)
                throw new ArgumentException("msgProductoTipoObligatorio");

            var tamanios = producto.Tamanios ?? new List<BE.PRODUCTO_TAMANIO>();
            if (tamanios.Count == 0)
                throw new ArgumentException("msgProductoSinTamanios");
            if (tamanios.GroupBy(t => t.IdTamanio).Any(g => g.Count() > 1))
                throw new ArgumentException("msgProductoTamanioRepetido");
            if (tamanios.Any(t => t.Precio < 0))
                throw new ArgumentException("msgProductoPrecioInvalido");
            // FA3: el tamaño tiene que estar activo y corresponder al tipo
            var permitidos = ListarTamaniosPorTipo(producto.Tipo).Select(t => t.IdTamanio).ToList();
            if (tamanios.Any(t => !permitidos.Contains(t.IdTamanio)))
                throw new ArgumentException("msgProductoTamanioInvalido");
        }

        // CU006 Insertar Productos: producto, DVH y precios en una sola transacción (decisión 27)
        public void InsertarProducto(BE.PRODUCTO producto)
        {
            ValidarProducto(producto);
            producto.Activo = true;
            // Obtener el ID real generado por la BD
            producto.DVH = "PENDIENTE";

            var acceso = new ACCESO();
            var mapper = new MAPPER_PRODUCTO(acceso);
            acceso.IniciarTransaccion();
            try
            {
                producto.IdProducto = mapper.AltaProducto(producto);
                producto.DVH = dvBLL.CalcularDVH(producto);
                mapper.ActualizarDVH(producto.IdProducto, producto.DVH); // actualizar en BD
                foreach (var precio in producto.Tamanios)
                {
                    precio.IdProducto = producto.IdProducto;
                    mapper.GuardarPrecio(producto.IdProducto, precio);
                }
                acceso.ConfirmarTransaccion();
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                acceso.DeshacerTransaccion();
                // FA2: ya existe un producto con ese nombre (activo o dado de baja)
                throw new ArgumentException("msgProductoDuplicado", ex);
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }
            dvBLL.RecalcularDV();

            cambiosBLL.RegistrarCambio(SessionManager.Instancia.UsuarioActual.IdUsuario, producto.IdProducto,  "ALTA", "", producto.Nombre, "Alta");

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario,"Alta de producto",$"Se agrego el producto: {producto.Nombre}");
        }

        // CU007 Dar de baja producto: baja lógica (Activo = 0), nunca se borra la fila
        public int EliminarProducto(BE.PRODUCTO producto)
        {
            var productoActual = GestorProducto.ObtenerPorId(producto.IdProducto);
            if (productoActual == null)
                throw new Exception($"No se encontró el producto ID {producto.IdProducto}.");

            productoActual.Activo = false;
            productoActual.DVH = dvBLL.CalcularDVH(productoActual);
            int filas = GestorProducto.BajaProducto(productoActual);

            cambiosBLL.RegistrarCambio(SessionManager.Instancia.UsuarioActual.IdUsuario, producto.IdProducto,"BAJA", productoActual.Nombre, "", "Baja");
            try
            {
                dvBLL.RecalcularDV();
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de producto", $"Se dio de baja el producto: {productoActual.Nombre}");
                }
            }
            catch { }
            return filas;
        }

        // CU008 Modificar producto: nombre, tipo, tamaños y precios (decisión 51) en una sola transacción
        public int ModificarProducto(BE.PRODUCTO producto)
        {
            ValidarProducto(producto);
            var productoAnterior = GestorProducto.ObtenerPorId(producto.IdProducto);
            if (productoAnterior == null || !productoAnterior.Activo)
                throw new ArgumentException("msgProductoInexistente");
            int idUsuario = SessionManager.Instancia.UsuarioActual.IdUsuario;
            // La modificación no cambia el estado de baja
            producto.Activo = productoAnterior.Activo;
            producto.DVH = dvBLL.CalcularDVH(producto);

            var preciosAnteriores = GestorProducto.ListarPreciosPorProducto(producto.IdProducto);
            var acceso = new ACCESO();
            var mapper = new MAPPER_PRODUCTO(acceso);
            int filas;
            acceso.IniciarTransaccion();
            try
            {
                filas = mapper.EditarProducto(producto);
                foreach (var anterior in preciosAnteriores.Where(a => !producto.Tamanios.Any(t => t.IdTamanio == a.IdTamanio)))
                    mapper.QuitarPrecio(producto.IdProducto, anterior.IdTamanio);
                foreach (var precio in producto.Tamanios)
                {
                    precio.IdProducto = producto.IdProducto;
                    mapper.GuardarPrecio(producto.IdProducto, precio);
                }
                acceso.ConfirmarTransaccion();
            }
            catch (SqlException ex) when (ex.Number == ErrorDeNegocioSql)
            {
                acceso.DeshacerTransaccion();
                // FA2 (nombre de otro producto) o un tamaño que ya tiene ventas y no se puede quitar
                throw new ArgumentException(ex.Message.Contains("ventas") ? "msgProductoTamanioConVentas" : "msgProductoDuplicado", ex);
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }

            if (productoAnterior.Nombre != producto.Nombre)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "Nombre", productoAnterior.Nombre, producto.Nombre, "Modificación");
            }
            if (productoAnterior.Tipo != producto.Tipo)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "Tipo", productoAnterior.Tipo, producto.Tipo, "Modificación");
            }

            try
            {
                dvBLL.RecalcularDV();
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(idUsuario, "Modificación de producto", $"Se modificó el producto: {producto.Nombre}");
                    // Los precios no van a CONTROL_CAMBIOS: su reversión solo maneja Nombre y Tipo
                    string precios = string.Join(", ", producto.Tamanios.OrderBy(t => t.IdTamanio).Select(t => $"{t.IdTamanio}={t.Precio:0.00}"));
                    string anteriores = string.Join(", ", preciosAnteriores.OrderBy(t => t.IdTamanio).Select(t => $"{t.IdTamanio}={t.Precio:0.00}"));
                    if (precios != anteriores)
                        new BITACORA_BLL().RegistrarEvento(idUsuario, "Modificación de precios", $"Producto {producto.Nombre}: tamaños y precios {anteriores} -> {precios}");
                }
            }
            catch { }
            return filas;
        }

        public List<BE.PRODUCTO> ListarProductos()
        {
            return GestorProducto.ListarProductos();
        }

        public List<BE.PRODUCTO> ListarProductosActivos()
        {
            return GestorProducto.ListarProductosActivos();
        }

        public BE.PRODUCTO ObtenerPorId(int id)
        {
            return GestorProducto.ObtenerPorId(id);
        }

        public List<string> VerificarIntegridad()
        {
            return dvBLL.VerificarIntegridad();
        }

        public void RecalcularDV()
        {
            dvBLL.RecalcularDV();
        }

        public void HacerBackup(string ruta)
        {
            dvBLL.HacerBackup(ruta);
        }

        public void RestaurarDesdeBackup(string ruta)
        {
            dvBLL.RestaurarDesdeBackup(ruta);
        }

        public DateTime? ObtenerFechaUltimoBackup()
        {
            return dvBLL.ObtenerFechaUltimoBackup();
        }
    }
}
