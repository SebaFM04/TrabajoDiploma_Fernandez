using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BACKUP_BLL
    {
        MAPPER_BACKUP mp_bkp = new MAPPER_BACKUP();

        public void GenerarBackup(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta))
            {
                throw new System.Exception("La ruta no puede estar vacía.");
            }
            mp_bkp.GenerarBackup(ruta);
        }
    }
}
