using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_RESTORE
    {
        ACCESO acceso = new ACCESO();

        public void RestaurarBackup(string rutaArchivo)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@RutaDestinoCompleta", rutaArchivo)
            };
            acceso.Escribir("RealizarRestore", parametros);
            acceso.Cerrar();
        }

        public DateTime? ObtenerFechaUltimoBackup()
        {
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ObtenerFechaUltimoBackup");
            acceso.Cerrar();

            if (tabla.Rows.Count > 0 && tabla.Rows[0]["FechaUltimoBackup"] != DBNull.Value)
            {
                return Convert.ToDateTime(tabla.Rows[0]["FechaUltimoBackup"]);
            }
            return null;
        }
    }
}
