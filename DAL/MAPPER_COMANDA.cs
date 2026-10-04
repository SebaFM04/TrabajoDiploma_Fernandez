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
    }
}
