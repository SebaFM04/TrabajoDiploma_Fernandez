using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N02 CU020: reclamo al proveedor por faltantes o daños de una recepción. Estado: Pendiente o Resuelto (decisión 60)
    public class RECLAMO
    {
        private int idReclamo;
        public int IdReclamo
        {
            get { return idReclamo; }
            set { idReclamo = value; }
        }

        private int idRecepcion;
        public int IdRecepcion
        {
            get { return idRecepcion; }
            set { idRecepcion = value; }
        }

        private DateTime fechaReclamo;
        public DateTime FechaReclamo
        {
            get { return fechaReclamo; }
            set { fechaReclamo = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private List<RECLAMO_DETALLE> detalles = new List<RECLAMO_DETALLE>();
        public List<RECLAMO_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }
    }
}
