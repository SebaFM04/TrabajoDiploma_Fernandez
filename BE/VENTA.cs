using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // CU010: venta cobrada. Se registra junto con sus detalles, el vale y la comanda (si hay piqueos).
    public class VENTA
    {
        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        private DateTime fecha;
        public DateTime Fecha
        {
            get { return fecha; }
            set { fecha = value; }
        }

        private decimal monto;
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        // Efectivo, Débito, Crédito, Transferencia o QR (CHECK CK_VENTA_MedioPago)
        private string medioPago;
        public string MedioPago
        {
            get { return medioPago; }
            set { medioPago = value; }
        }

        private List<VENTA_DETALLE> detalles = new List<VENTA_DETALLE>();
        public List<VENTA_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }

        // Generados en la misma transacción (CU010 paso 5). Comanda es null si no hay piqueos.
        private VALE vale;
        public VALE Vale
        {
            get { return vale; }
            set { vale = value; }
        }

        private COMANDA comanda;
        public COMANDA Comanda
        {
            get { return comanda; }
            set { comanda = value; }
        }

        // Factura B de la venta (CU015); null si todavía no se emitió. Se carga en la consulta de ventas.
        private FACTURA factura;
        public FACTURA Factura
        {
            get { return factura; }
            set { factura = value; }
        }
    }
}
