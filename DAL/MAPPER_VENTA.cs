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
    public class MAPPER_VENTA
    {
        ACCESO acceso;

        public MAPPER_VENTA()
        {
            acceso = new ACCESO();
        }

        // CU010: se usa dentro de la transacción de la venta (decisión 27)
        public MAPPER_VENTA(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU010 mensajes 36-39: guarda la venta y sus detalles (decisión 53). Devuelve la venta con sus Id.
        public BE.VENTA GuardarVenta(BE.VENTA venta)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@Fecha", venta.Fecha),
                    acceso.CrearParametro("@Monto", venta.Monto),
                    acceso.CrearParametro("@MedioPago", venta.MedioPago)
                };
                DataTable tabla = acceso.Leer("GuardarVenta", parametros);
                venta.IdVenta = Convert.ToInt32(tabla.Rows[0]["IdVenta"]);

                foreach (var detalle in venta.Detalles)
                {
                    detalle.IdVenta = venta.IdVenta;
                    List<SqlParameter> pDetalle = new List<SqlParameter>
                    {
                        acceso.CrearParametro("@IdVenta", venta.IdVenta),
                        acceso.CrearParametro("@IdProducto", detalle.IdProducto),
                        acceso.CrearParametro("@IdTamanio", detalle.IdTamanio),
                        acceso.CrearParametro("@Cantidad", detalle.Cantidad),
                        acceso.CrearParametro("@MontoLinea", detalle.MontoLinea)
                    };
                    DataTable tDetalle = acceso.Leer("GuardarVentaDetalle", pDetalle);
                    detalle.IdVentaDetalle = Convert.ToInt32(tDetalle.Rows[0]["IdVentaDetalle"]);
                }
                return venta;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
