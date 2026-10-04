using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N02 CU021: pago a proveedor de una orden cerrada (se ejecuta fuera del sistema, decisión 19)
    public class PAGO_PROVEEDOR
    {
        private int idPago;
        public int IdPago
        {
            get { return idPago; }
            set { idPago = value; }
        }

        private int idOrdenCompra;
        public int IdOrdenCompra
        {
            get { return idOrdenCompra; }
            set { idOrdenCompra = value; }
        }

        private DateTime fechaPago;
        public DateTime FechaPago
        {
            get { return fechaPago; }
            set { fechaPago = value; }
        }

        private string medioPago;
        public string MedioPago
        {
            get { return medioPago; }
            set { medioPago = value; }
        }

        private decimal monto;
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        private string numeroComprobante;
        public string NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }
    }
}
