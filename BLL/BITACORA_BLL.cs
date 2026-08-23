using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class BITACORA_BLL
    {
        DAL.MAPPER_BITACORA mpBitacora = new DAL.MAPPER_BITACORA();
        public void RegistrarEvento(int idUsuario, string actividad, string infoAsociada)
        {
            var registro = new BITACORA
            {
                FechaHora = DateTime.Now,
                IdUsuario = idUsuario,
                Actividad = actividad,
                InformacionAsociada = infoAsociada
            };
            mpBitacora.GuardarRegistro(registro);
        }

        public List<BITACORA> BuscarRegistros(int? idUsuario, string actividad, DateTime? fechaDesde, DateTime? fechaHasta)
        {
            return mpBitacora.ObtenerRegistros(idUsuario, actividad, fechaDesde, fechaHasta);
        }
    }
}
