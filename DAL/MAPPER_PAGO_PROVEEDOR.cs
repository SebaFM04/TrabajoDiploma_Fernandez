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
    public class MAPPER_PAGO_PROVEEDOR
    {
        ACCESO acceso;

        public MAPPER_PAGO_PROVEEDOR()
        {
            acceso = new ACCESO();
        }

        // CU021: se usa dentro de la transacción del pago (decisión 27)
        public MAPPER_PAGO_PROVEEDOR(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU021 mensajes 27-30
        public BE.PAGO_PROVEEDOR GuardarPago(BE.PAGO_PROVEEDOR pago)
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("GuardarPagoProveedor", new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdOrdenCompra", pago.IdOrdenCompra),
                    acceso.CrearParametro("@FechaPago", pago.FechaPago),
                    acceso.CrearParametro("@MedioPago", pago.MedioPago),
                    acceso.CrearParametro("@Monto", pago.Monto),
                    acceso.CrearParametro("@NumeroComprobante", pago.NumeroComprobante)
                });
                pago.IdPago = Convert.ToInt32(tabla.Rows[0]["IdPago"]);
                return pago;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
