using BE;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_PRODUCTO
    {
        ACCESO acceso = new ACCESO();

        public int AltaProducto(BE.PRODUCTO Producto)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@NombreProducto", Producto.NombreProducto));
            parametros.Add(acceso.CrearParametro("@PrecioProducto", Producto.PrecioProducto));
            parametros.Add(acceso.CrearParametro("@TipoProducto", Producto.TipoProducto));
            parametros.Add(acceso.CrearParametro("@Cantidad", Producto.Cantidad));
            parametros.Add(acceso.CrearParametro("@Descripcion", Producto.Descripcion));
            parametros.Add(acceso.CrearParametro("@CodigoProducto", Producto.CodigoProducto));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));

            DataTable tabla = acceso.Leer("AltaProducto", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count > 0)
            {
                return Convert.ToInt32(tabla.Rows[0]["IdProducto"]);
            }
            return 0;
        }

        public int BajaProducto(BE.PRODUCTO Producto)
        {
            string NombreSp = "BajaProducto";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdProducto", Producto.IdProducto));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        public int EditarProducto(BE.PRODUCTO Producto)
        {
            string NombreSp = "ModificarProducto";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdProducto", Producto.IdProducto));
            parametros.Add(acceso.CrearParametro("@NombreProducto", Producto.NombreProducto));
            parametros.Add(acceso.CrearParametro("@PrecioProducto", Producto.PrecioProducto));
            parametros.Add(acceso.CrearParametro("@TipoProducto", Producto.TipoProducto));
            parametros.Add(acceso.CrearParametro("@Cantidad", Producto.Cantidad.ToString()));
            parametros.Add(acceso.CrearParametro("@Descripcion", Producto.Descripcion));
            parametros.Add(acceso.CrearParametro("@CodigoProducto", Producto.CodigoProducto));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        public List<BE.PRODUCTO> ListarProductos()
        {
            List<BE.PRODUCTO> listaProductos = new List<BE.PRODUCTO>();
            string NombreSp = "ListarProducto";
            acceso.Abrir();

            DataTable tabla = new DataTable();
            tabla = acceso.Leer(NombreSp);
            acceso.Cerrar();
            foreach (DataRow u in tabla.Rows)
            {
                BE.PRODUCTO producto = new BE.PRODUCTO();

                producto.IdProducto = Convert.ToInt32(u["IdProducto"].ToString());
                producto.NombreProducto = u["NombreProducto"].ToString();
                producto.PrecioProducto = Convert.ToDecimal(u["PrecioProducto"].ToString());
                producto.TipoProducto = u["TipoProducto"].ToString();               
                producto.Descripcion = u["Descripcion"].ToString();
                producto.Cantidad = Convert.ToInt32(u["Cantidad"].ToString());
                producto.CodigoProducto = Convert.ToInt32(u["CodigoProducto"].ToString());
                producto.DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString();
                listaProductos.Add(producto);
            }
            return listaProductos;
        }

        public void ActualizarDVH(int idProducto, string dvh)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto),
                acceso.CrearParametro("@DVH", dvh)
            };
            acceso.Escribir("ActualizarDVHProducto", parametros);
            acceso.Cerrar();
        }

        public BE.PRODUCTO ObtenerPorId(int idProducto)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto)
            };
            DataTable tabla = acceso.Leer("ObtenerProductoPorId", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count == 0) return null;

            DataRow u = tabla.Rows[0];
            return new BE.PRODUCTO
            {
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                NombreProducto = u["NombreProducto"].ToString(),
                PrecioProducto = Convert.ToDecimal(u["PrecioProducto"]),
                TipoProducto = u["TipoProducto"].ToString(),
                Descripcion = u["Descripcion"].ToString(),
                Cantidad = Convert.ToInt32(u["Cantidad"]),
                CodigoProducto = Convert.ToInt32(u["CodigoProducto"]),
                DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString()
            };
        }

    }
}
