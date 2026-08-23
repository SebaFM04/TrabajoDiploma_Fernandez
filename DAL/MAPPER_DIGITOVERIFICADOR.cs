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
    public class MAPPER_DIGITOVERIFICADOR
    {
        ACCESO acceso = new ACCESO();

        public void GuardarDVV(DIGITOVERIFICADOR dv)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@NombreTabla", dv.NombreTabla),
                acceso.CrearParametro("@DVV", dv.DVV)
            };
            acceso.Escribir("GuardarDVV", parametros);
            acceso.Cerrar();
        }

        public DIGITOVERIFICADOR ObtenerDVV(string nombreTabla)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@NombreTabla", nombreTabla)
            };
            DataTable tabla = acceso.Leer("ObtenerDVV", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count > 0)
            {
                return new DIGITOVERIFICADOR
                {
                    NombreTabla = tabla.Rows[0]["NombreTabla"].ToString(),
                    DVV = tabla.Rows[0]["DVV"].ToString()
                };
            }
            return null;
        }
    }
}
