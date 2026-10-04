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
    }
}
