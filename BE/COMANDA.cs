using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Comanda para Cocina (VENTA 1 → 0..1 COMANDA). Estado: Pendiente o Entregada (decisión 45).
    public class COMANDA
    {
        private int idComanda;
        public int IdComanda
        {
            get { return idComanda; }
            set { idComanda = value; }
        }

        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        private DateTime fechaHoraEmision;
        public DateTime FechaHoraEmision
        {
            get { return fechaHoraEmision; }
            set { fechaHoraEmision = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        // Líneas de piqueos de la venta (decisión 32: la comanda no tiene detalle propio)
        private List<VENTA_DETALLE> detalles = new List<VENTA_DETALLE>();
        public List<VENTA_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }
    }
}
