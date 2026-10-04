using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N02 CU019: mercadería recibida contra una orden de compra, con remito y factura del proveedor
    public class RECEPCION
    {
        private int idRecepcion;
        public int IdRecepcion
        {
            get { return idRecepcion; }
            set { idRecepcion = value; }
        }

        private int idOrdenCompra;
        public int IdOrdenCompra
        {
            get { return idOrdenCompra; }
            set { idOrdenCompra = value; }
        }

        private DateTime fechaRecepcion;
        public DateTime FechaRecepcion
        {
            get { return fechaRecepcion; }
            set { fechaRecepcion = value; }
        }

        private string numeroRemito;
        public string NumeroRemito
        {
            get { return numeroRemito; }
            set { numeroRemito = value; }
        }

        private string numeroFacturaProveedor;
        public string NumeroFacturaProveedor
        {
            get { return numeroFacturaProveedor; }
            set { numeroFacturaProveedor = value; }
        }

        // Insumos recibidos en condiciones
        private List<RECEPCION_DETALLE> detalles = new List<RECEPCION_DETALLE>();
        public List<RECEPCION_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }
    }
}
