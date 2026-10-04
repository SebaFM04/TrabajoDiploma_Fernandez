using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Vale que certifica el pago (VENTA 1 → 1 VALE). Se usa en CU012 para retirar las bebidas.
    public class VALE
    {
        private int idVale;
        public int IdVale
        {
            get { return idVale; }
            set { idVale = value; }
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

        private decimal montoCertificado;
        public decimal MontoCertificado
        {
            get { return montoCertificado; }
            set { montoCertificado = value; }
        }

        private bool utilizado;
        public bool Utilizado
        {
            get { return utilizado; }
            set { utilizado = value; }
        }

        // CU012: líneas de bebidas de la venta del vale
        private List<VENTA_DETALLE> detalles = new List<VENTA_DETALLE>();
        public List<VENTA_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }
    }
}
