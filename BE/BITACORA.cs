using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class BITACORA
    {
        private int idBitacora;
        public int IdBitacora
        {
            get { return idBitacora; }
            set { idBitacora = value; }
        }

        private DateTime fechaHora;
        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        private int idUsuario;
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        private string actividad;
        public string Actividad
        {
            get { return actividad; }
            set { actividad = value; }
        }

        private string informacionAsociada;
        public string InformacionAsociada
        {
            get { return informacionAsociada; }
            set { informacionAsociada = value; }
        }
    }
}
