using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_TAMANIO
    {
        ACCESO acceso = new ACCESO();

        // CU006 paso 2: tamaños activos para ofrecer en el alta del producto
        public List<BE.TAMANIO> ListarTamaniosActivos()
        {
            List<BE.TAMANIO> tamanios = new List<BE.TAMANIO>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarTamanioActivo");
            acceso.Cerrar();

            foreach (DataRow u in tabla.Rows)
            {
                tamanios.Add(new BE.TAMANIO
                {
                    IdTamanio = Convert.ToInt32(u["IdTamaño"]),
                    Nombre = u["Nombre"].ToString(),
                    CantidadMagnitud = Convert.ToDecimal(u["CantidadMagnitud"]),
                    UnidadMagnitud = u["UnidadMagnitud"].ToString(),
                    Activo = Convert.ToBoolean(u["Activo"])
                });
            }
            return tamanios;
        }
    }
}
