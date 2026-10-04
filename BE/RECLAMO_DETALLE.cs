using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Insumo reclamado: cantidad pendiente, motivo (Faltante o Dañado) y descripción
    public class RECLAMO_DETALLE
    {
        private int idReclamo;
        public int IdReclamo
        {
            get { return idReclamo; }
            set { idReclamo = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        private int cantidadPendiente;
        public int CantidadPendiente
        {
            get { return cantidadPendiente; }
            set { cantidadPendiente = value; }
        }

        private string motivo;
        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }
    }
}
