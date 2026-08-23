using System.IO;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class RESTORE_BLL
    {
        MAPPER_RESTORE mp_rst = new MAPPER_RESTORE();
        public void RestaurarBackup(string rutaArchivo)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivo))
            {
                throw new System.Exception("La ruta no puede estar vacía.");
            }
            if (!File.Exists(rutaArchivo))
            {
                throw new System.Exception($"No se encontró el archivo en la ruta: {rutaArchivo}");
            }
            mp_rst.RestaurarBackup(rutaArchivo);
        }
    }
}
