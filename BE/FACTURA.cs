using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // CU015: factura simulada a consumidor final (decisión 11). Datos del cliente opcionales (decisión 46).
    public class FACTURA
    {
        private int idFactura;
        public int IdFactura
        {
            get { return idFactura; }
            set { idFactura = value; }
        }

        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        private int numeroComprobante;
        public int NumeroComprobante
        {
            get { return numeroComprobante; }
            set { numeroComprobante = value; }
        }

        private DateTime fechaHoraEmision;
        public DateTime FechaHoraEmision
        {
            get { return fechaHoraEmision; }
            set { fechaHoraEmision = value; }
        }

        private string nombreCliente;
        public string NombreCliente
        {
            get { return nombreCliente; }
            set { nombreCliente = value; }
        }

        private string telefonoCliente;
        public string TelefonoCliente
        {
            get { return telefonoCliente; }
            set { telefonoCliente = value; }
        }

        private string correoCliente;
        public string CorreoCliente
        {
            get { return correoCliente; }
            set { correoCliente = value; }
        }

        private decimal total;
        public decimal Total
        {
            get { return total; }
            set { total = value; }
        }
    }
}
