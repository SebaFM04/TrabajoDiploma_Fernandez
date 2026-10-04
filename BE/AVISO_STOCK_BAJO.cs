using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Aviso al Encargado cuando un insumo queda bajo el umbral (decisiones 8, 10 y 47). Estado: Pendiente, En compra, Resuelto.
    public class AVISO_STOCK_BAJO
    {
        private int idAviso;
        public int IdAviso
        {
            get { return idAviso; }
            set { idAviso = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        private DateTime fechaHora;
        public DateTime FechaHora
        {
            get { return fechaHora; }
            set { fechaHora = value; }
        }

        private decimal volumenPesoAlMomento;
        public decimal VolumenPesoAlMomento
        {
            get { return volumenPesoAlMomento; }
            set { volumenPesoAlMomento = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        // FK a ORDEN_COMPRA cuando exista N02 (decisión 30)
        private int? idOrdenCompra;
        public int? IdOrdenCompra
        {
            get { return idOrdenCompra; }
            set { idOrdenCompra = value; }
        }
    }
}
