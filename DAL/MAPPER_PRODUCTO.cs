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
            parametros.Add(acceso.CrearParametro("@Nombre", Producto.Nombre));
            parametros.Add(acceso.CrearParametro("@Tipo", Producto.Tipo));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));

            DataTable tabla = acceso.Leer("AltaProducto", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count > 0)
            {
                return Convert.ToInt32(tabla.Rows[0]["IdProducto"]);
            }
            return 0;
        }

        // Baja lógica: el SP pone Activo = 0 y guarda el DVH recalculado
        public int BajaProducto(BE.PRODUCTO Producto)
        {
            string NombreSp = "BajaProducto";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdProducto", Producto.IdProducto));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));
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
            parametros.Add(acceso.CrearParametro("@Nombre", Producto.Nombre));
            parametros.Add(acceso.CrearParametro("@Tipo", Producto.Tipo));
            parametros.Add(acceso.CrearParametro("@DVH", Producto.DVH));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        // Todos los productos, activos y dados de baja (lo usa el dígito verificador)
        public List<BE.PRODUCTO> ListarProductos()
        {
            return Listar("ListarProducto");
        }

        // Solo los productos activos, para las pantallas de operación
        public List<BE.PRODUCTO> ListarProductosActivos()
        {
            return Listar("ListarProductoActivo");
        }

        private List<BE.PRODUCTO> Listar(string NombreSp)
        {
            List<BE.PRODUCTO> listaProductos = new List<BE.PRODUCTO>();
            acceso.Abrir();

            DataTable tabla = new DataTable();
            tabla = acceso.Leer(NombreSp);
            acceso.Cerrar();
            foreach (DataRow u in tabla.Rows)
            {
                listaProductos.Add(MapearProducto(u));
            }
            return listaProductos;
        }

        private BE.PRODUCTO MapearProducto(DataRow u)
        {
            return new BE.PRODUCTO
            {
                IdProducto = Convert.ToInt32(u["IdProducto"]),
                Nombre = u["Nombre"].ToString(),
                Tipo = u["Tipo"].ToString(),
                Activo = Convert.ToBoolean(u["Activo"]),
                DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString()
            };
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

            return MapearProducto(tabla.Rows[0]);
        }

    }
}
