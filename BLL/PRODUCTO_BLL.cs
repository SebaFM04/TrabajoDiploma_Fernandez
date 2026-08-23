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

        public void InsertarProducto(BE.PRODUCTO producto)
        {
            // Obtener el ID real generado por la BD
            producto.DVH = "PENDIENTE";
            int idGenerado = GestorProducto.AltaProducto(producto);
            producto.IdProducto = idGenerado;
            producto.DVH = dvBLL.CalcularDVH(producto);
            GestorProducto.ActualizarDVH(producto.IdProducto, producto.DVH); // actualizar en BD

            dvBLL.RecalcularDV();

            cambiosBLL.RegistrarCambio(SessionManager.Instancia.UsuarioActual.IdUsuario, producto.IdProducto,  "ALTA", "", producto.NombreProducto, "Alta");

            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario,"Alta de producto",$"Se agrego el producto: {producto.NombreProducto}");
        }

        public int EliminarProducto(BE.PRODUCTO producto)
        {
            cambiosBLL.RegistrarCambio(SessionManager.Instancia.UsuarioActual.IdUsuario, producto.IdProducto,"BAJA", producto.NombreProducto, "", "Baja");

            int filas = GestorProducto.BajaProducto(producto);
            try
            {
                dvBLL.RecalcularDV();
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de producto", $"Se eliminó el producto: {producto.NombreProducto}");
                }
            }
            catch { }
            return filas;
        }

        public int ModificarProducto(BE.PRODUCTO producto)
        {
            var productoAnterior = GestorProducto.ObtenerPorId(producto.IdProducto);
            int idUsuario = SessionManager.Instancia.UsuarioActual.IdUsuario;

            if (productoAnterior.NombreProducto != producto.NombreProducto)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "NombreProducto", productoAnterior.NombreProducto, producto.NombreProducto, "Modificación");
            }
            if (productoAnterior.PrecioProducto != producto.PrecioProducto)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "PrecioProducto", productoAnterior.PrecioProducto.ToString(), producto.PrecioProducto.ToString(), "Modificación");
            }
            if (productoAnterior.TipoProducto != producto.TipoProducto)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "TipoProducto", productoAnterior.TipoProducto, producto.TipoProducto, "Modificación");
            }         
            if (productoAnterior.Descripcion != producto.Descripcion)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "Descripcion", productoAnterior.Descripcion, producto.Descripcion, "Modificación");
            }
            if (productoAnterior.Cantidad != producto.Cantidad)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "Cantidad", productoAnterior.Cantidad.ToString(), producto.Cantidad.ToString(), "Modificación");
            }
            if (productoAnterior.CodigoProducto != producto.CodigoProducto)
            {
                cambiosBLL.RegistrarCambio(idUsuario, producto.IdProducto, "CodigoProducto", productoAnterior.CodigoProducto.ToString(), producto.CodigoProducto.ToString(), "Modificación");
            }

            producto.DVH = dvBLL.CalcularDVH(producto);
            int filas = GestorProducto.EditarProducto(producto);
            try
            {
                dvBLL.RecalcularDV();
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(idUsuario, "Modificación de producto", $"Se modificó el producto: {producto.NombreProducto}");
                }
            }
            catch { }
            return filas;
        }

        public List<BE.PRODUCTO> ListarProductos()
        {
            return GestorProducto.ListarProductos();
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
