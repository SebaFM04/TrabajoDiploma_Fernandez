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
    public class MAPPER_VALE
    {
        ACCESO acceso;

        public MAPPER_VALE()
        {
            acceso = new ACCESO();
        }

        // CU010: se usa dentro de la transacción de la venta (decisión 27)
        public MAPPER_VALE(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU010 mensajes 41-44
        public BE.VALE GuardarVale(BE.VALE vale)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVenta", vale.IdVenta),
                    acceso.CrearParametro("@FechaHoraEmision", vale.FechaHoraEmision),
                    acceso.CrearParametro("@MontoCertificado", vale.MontoCertificado)
                };
                DataTable tabla = acceso.Leer("GuardarVale", parametros);
                vale.IdVale = Convert.ToInt32(tabla.Rows[0]["IdVale"]);
                return vale;
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU012 mensajes 3-6: vale con las líneas de bebidas de su venta. null si no existe.
        // Si la venta es solo de piqueos, Detalles queda vacío (LEFT JOIN con NULL, FA2).
        public BE.VALE BuscarVale(int idVale)
        {
            acceso.Abrir();
            DataTable tabla;
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVale", idVale)
                };
                tabla = acceso.Leer("BuscarVale", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
            if (tabla.Rows.Count == 0) return null;

            DataRow r = tabla.Rows[0];
            var vale = new BE.VALE
            {
                IdVale = Convert.ToInt32(r["IdVale"]),
                IdVenta = Convert.ToInt32(r["IdVenta"]),
                FechaHoraEmision = Convert.ToDateTime(r["FechaHoraEmision"]),
                MontoCertificado = Convert.ToDecimal(r["MontoCertificado"]),
                Utilizado = Convert.ToBoolean(r["Utilizado"])
            };
            foreach (DataRow u in tabla.Rows)
            {
                if (u["IdVentaDetalle"] == DBNull.Value) continue;
                vale.Detalles.Add(new BE.VENTA_DETALLE
                {
                    IdVentaDetalle = Convert.ToInt32(u["IdVentaDetalle"]),
                    IdVenta = vale.IdVenta,
                    IdProducto = Convert.ToInt32(u["IdProducto"]),
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    Cantidad = Convert.ToInt32(u["Cantidad"]),
                    ProductoTamanio = new BE.PRODUCTO_TAMANIO
                    {
                        IdProducto = Convert.ToInt32(u["IdProducto"]),
                        IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                        Producto = new BE.PRODUCTO { IdProducto = Convert.ToInt32(u["IdProducto"]), Nombre = u["NombreProducto"].ToString(), Tipo = "Bebida" },
                        Tamanio = new BE.TAMANIO
                        {
                            IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                            Nombre = u["NombreTamanio"].ToString(),
                            CantidadMagnitud = Convert.ToDecimal(u["CantidadMagnitud"])
                        }
                    }
                });
            }
            return vale;
        }

        // CU012 mensajes 19-22. Devuelve las filas afectadas: 0 si el vale ya estaba utilizado.
        public int ActualizarUtilizado(int idVale)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVale", idVale)
                };
                return acceso.Escribir("ActualizarUtilizadoVale", parametros);
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
