using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_RECETA
    {
        ACCESO acceso = new ACCESO();

        // Líneas de la receta del producto, con los datos del insumo de cada una
        public List<BE.RECETA> BuscarRecetaPorProducto(int idProducto)
        {
            List<BE.RECETA> receta = new List<BE.RECETA>();
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdProducto", idProducto)
            };
            DataTable tabla = acceso.Leer("BuscarRecetaPorProducto", parametros);
            acceso.Cerrar();

            foreach (DataRow u in tabla.Rows)
            {
                receta.Add(new BE.RECETA
                {
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                    Proporcion = Convert.ToDecimal(u["Proporcion"]),
                    Insumo = new BE.INSUMO
                    {
                        IdInsumo = Convert.ToInt32(u["IdInsumo"]),
                        Nombre = u["NombreInsumo"].ToString(),
                        UnidadMedida = u["UnidadMedida"].ToString(),
                        Activo = Convert.ToBoolean(u["InsumoActivo"])
                    }
                });
            }
            return receta;
        }

        // CU013: productos activos sin receta
        public List<BE.PRODUCTO> ListarProductosSinReceta()
        {
            return ListarProductos("ListarProductoSinReceta");
        }

        // CU013: guarda todas las líneas en una sola transacción (decisión 27)
        public void GuardarReceta(int idProducto, List<BE.RECETA> receta)
        {
            acceso.IniciarTransaccion();
            try
            {
                GuardarLineas(idProducto, receta);
                acceso.ConfirmarTransaccion();
            }
            catch
            {
                acceso.DeshacerTransaccion();
                throw;
            }
        }

        private void GuardarLineas(int idProducto, List<BE.RECETA> receta)
        {
            foreach (var linea in receta)
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdProducto", idProducto),
                    acceso.CrearParametro("@IdInsumo", linea.IdInsumo),
                    acceso.CrearParametro("@Proporcion", linea.Proporcion)
                };
                acceso.Escribir("GuardarRecetaLinea", parametros);
            }
        }

        private List<BE.PRODUCTO> ListarProductos(string nombreSp)
        {
            List<BE.PRODUCTO> productos = new List<BE.PRODUCTO>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer(nombreSp);
            acceso.Cerrar();

            foreach (DataRow u in tabla.Rows)
            {
                productos.Add(new BE.PRODUCTO
                {
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    Nombre = u["Nombre"].ToString(),
                    Tipo = u["Tipo"].ToString(),
                    Activo = Convert.ToBoolean(u["Activo"]),
                    DVH = u["DVH"] == DBNull.Value ? null : u["DVH"].ToString()
                });
            }
            return productos;
        }
    }
}
