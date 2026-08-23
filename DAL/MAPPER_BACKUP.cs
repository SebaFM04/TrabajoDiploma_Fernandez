using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_BACKUP
    {
        ACCESO acceso = new ACCESO();
        public void GenerarBackup(string rutaDestino)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@RutaDestinoCompleta", rutaDestino)
            };
            acceso.Escribir("RealizarBackup", parametros);
            acceso.Cerrar();
        }
    }
}
