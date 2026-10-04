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
    public class MAPPER_FACTURA
    {
        ACCESO acceso;

        public MAPPER_FACTURA()
        {
            acceso = new ACCESO();
        }

        // CU015: número y alta de la factura en la misma transacción (el SP bloquea FACTURA hasta el commit)
        public MAPPER_FACTURA(ACCESO accesoCompartido)
        {
            acceso = accesoCompartido;
        }

        // CU015 mensajes 8-11: próximo número de comprobante (siempre creciente, decisión 11)
        public int ObtenerProximoNumero()
        {
            acceso.Abrir();
            try
            {
                DataTable tabla = acceso.Leer("ObtenerProximoNumeroFactura");
                return Convert.ToInt32(tabla.Rows[0]["NumeroComprobante"]);
            }
            finally
            {
                acceso.Cerrar();
            }
        }

        // CU015 mensajes 12-19
        public BE.FACTURA GuardarFactura(BE.FACTURA factura)
        {
            acceso.Abrir();
            try
            {
                List<SqlParameter> parametros = new List<SqlParameter>
                {
                    acceso.CrearParametro("@IdVenta", factura.IdVenta),
                    acceso.CrearParametro("@NumeroComprobante", factura.NumeroComprobante),
                    acceso.CrearParametro("@FechaHoraEmision", factura.FechaHoraEmision),
                    acceso.CrearParametro("@NombreCliente", factura.NombreCliente ?? string.Empty),
                    acceso.CrearParametro("@TelefonoCliente", factura.TelefonoCliente ?? string.Empty),
                    acceso.CrearParametro("@CorreoCliente", factura.CorreoCliente ?? string.Empty),
                    acceso.CrearParametro("@Total", factura.Total)
                };
                DataTable tabla = acceso.Leer("GuardarFactura", parametros);
                factura.IdFactura = Convert.ToInt32(tabla.Rows[0]["IdFactura"]);
                return factura;
            }
            finally
            {
                acceso.Cerrar();
            }
        }
    }
}
