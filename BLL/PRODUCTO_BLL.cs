using BE;
using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;

namespace BLL
{
    public class PRODUCTO_BLL
    {
        MAPPER_PRODUCTO GestorProducto = new MAPPER_PRODUCTO();
        DIGITOVERIFICADOR_BLL dvBLL = new DIGITOVERIFICADOR_BLL();
        CONTROLCAMBIO_BLL cambiosBLL = new CONTROLCAMBIO_BLL();

        // Valores de PRODUCTO.Tipo (CHECK CK_PRODUCTO_Tipo en script.sql)
        public string[] ListarTiposProducto()
        {
            return new[] { "Bebida", "Piqueo" };
        }

        // Reglas de negocio del producto; el nombre duplicado lo rechaza el SP
        private void ValidarProducto(BE.PRODUCTO producto)
        {
            if (string.IsNullOrWhiteSpace(producto.Nombre))
                throw new ArgumentException("El nombre del producto es obligatorio.");
            producto.Nombre = producto.Nombre.Trim();
            if (producto.Nombre.Length > 100)
                throw new ArgumentException("El nombre del producto no puede superar los 100 caracteres.");
            if (Array.IndexOf(ListarTiposProducto(), producto.Tipo) < 0)
                throw new ArgumentException("El tipo del producto debe ser Bebida o Piqueo.");
        }

        public void InsertarProducto(BE.PRODUCTO producto)
        {
            ValidarProducto(producto);
            producto.Activo = true;

            // Obtener el ID real generado por la BD
            producto.DVH = "PENDIENTE";
            int idGenerado = GestorProducto.AltaProducto(producto);
            producto.IdProducto = idGenerado;
            producto.DVH = dvBLL.CalcularDVH(producto);
            GestorProducto.ActualizarDVH(producto.IdProducto, producto.DVH); // actualizar en BD

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

        public int ModificarProducto(BE.PRODUCTO producto)
        {
            ValidarProducto(producto);
            var productoAnterior = GestorProducto.ObtenerPorId(producto.IdProducto);
            int idUsuario = SessionManager.Instancia.UsuarioActual.IdUsuario;

            // La modificación no cambia el estado de baja
            producto.Activo = productoAnterior.Activo;
            producto.DVH = dvBLL.CalcularDVH(producto);
            int filas = GestorProducto.EditarProducto(producto);

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
