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
    public class MAPPER_COMANDA
    {
        ACCESO acceso;

        public MAPPER_COMANDA()
        {
            acceso = new ACCESO();
        }

        // CU010: se usa dentro de la transacción de la venta (decisión 27)
        public MAPPER_COMANDA(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU010 mensajes 47-50
        public BE.COMANDA GuardarComanda(BE.COMANDA comanda)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVenta", comanda.IdVenta),
                    acceso.CrearParametro("@FechaHoraEmision", comanda.FechaHoraEmision),
                    acceso.CrearParametro("@Estado", comanda.Estado)
                };
                DataTable tabla = acceso.Leer("GuardarComanda", parametros);
                comanda.IdComanda = Convert.ToInt32(tabla.Rows[0]["IdComanda"]);
                return comanda;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU011 mensajes 3-6 y 11-14: comanda con sus líneas de piqueos. null si no existe.
        public BE.COMANDA BuscarComanda(int idComanda)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdComanda", idComanda)
                };
                tabla = acceso.Leer("BuscarComanda", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
            if (tabla.Rows.Count == 0) return null;

            var comanda = MapearComanda(tabla.Rows[0]);
            foreach (DataRow u in tabla.Rows)
            {
                // LEFT JOIN: sin piqueos el detalle viene en NULL
                if (u["IdVentaDetalle"] == DBNull.Value) continue;
                comanda.Detalles.Add(new BE.VENTA_DETALLE
                {
                    IdVentaDetalle = Convert.ToInt32(u["IdVentaDetalle"]),
                    IdVenta = comanda.IdVenta,
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    Cantidad = Convert.ToInt32(u["Cantidad"]),
                    ProductoTamanio = new BE.PRODUCTO_TAMANIO
                    {
                        IdProducto = Convert.ToInt32(u["IdProducto"]),
                        IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                        Producto = new BE.PRODUCTO { IdProducto = Convert.ToInt32(u["IdProducto"]), Nombre = u["NombreProducto"].ToString(), Tipo = "Piqueo" },
                        Tamanio = new BE.TAMANIO { IdTamanio = Convert.ToInt32(u["IdTamaño"]), Nombre = u["NombreTamanio"].ToString() }
                    }
                });
            }
            return comanda;
        }

        // CU011 mensajes 17-20
        public void ActualizarEstado(int idComanda, string estado)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdComanda", idComanda),
                    acceso.CrearParametro("@Estado", estado)
                };
                acceso.Escribir("ActualizarEstadoComanda", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU011 paso 1: comandas en un estado (Pendiente), de la más vieja a la más nueva
        public List<BE.COMANDA> ListarPorEstado(string estado)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Estado", estado)
                };
                tabla = acceso.Leer("ListarComandaPorEstado", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
            return tabla.Rows.Cast<DataRow>().Select(MapearComanda).ToList();
        }

        private BE.COMANDA MapearComanda(DataRow u)
        {
            return new BE.COMANDA
            {
                IdComanda = Convert.ToInt32(u["IdComanda"]),
                IdVenta = Convert.ToInt32(u["IdVenta"]),
                FechaHoraEmision = Convert.ToDateTime(u["FechaHoraEmision"]),
                Estado = u["Estado"].ToString()
            };
        }
    }
}
