using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N02: orden de compra a un proveedor. Estados (decisión 49): Pendiente de aprobación, Aprobada, Observada,
    // Recibida parcialmente, Cerrada, Pagada.
    public class ORDEN_COMPRA
    {
        private int idOrdenCompra;
        public int IdOrdenCompra
        {
            get { return idOrdenCompra; }
            set { idOrdenCompra = value; }
        }

        private int idProveedor;
        public int IdProveedor
        {
            get { return idProveedor; }
            set { idProveedor = value; }
        }

        private DateTime fechaGeneracion;
        public DateTime FechaGeneracion
        {
            get { return fechaGeneracion; }
            set { fechaGeneracion = value; }
        }

        private DateTime? fechaAprobacion;
        public DateTime? FechaAprobacion
        {
            get { return fechaAprobacion; }
            set { fechaAprobacion = value; }
        }

        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        // CU017 FA2: observaciones del Dueño al devolver la orden
        private string observaciones;
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        private PROVEEDOR proveedor;
        public PROVEEDOR Proveedor
        {
            get { return proveedor; }
            set { proveedor = value; }
        }

        private List<ORDEN_COMPRA_DETALLE> detalles = new List<ORDEN_COMPRA_DETALLE>();
        public List<ORDEN_COMPRA_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }

        // Consulta de compras: pago de la orden (null si no está pagada)
        private PAGO_PROVEEDOR pago;
        public PAGO_PROVEEDOR Pago
        {
            get { return pago; }
            set { pago = value; }
        }

        // Consulta de compras: Σ cantidad recibida × costo de cada recepción (dato calculado)
        private decimal totalRecibido;
        public decimal TotalRecibido
        {
            get { return totalRecibido; }
            set { totalRecibido = value; }
        }

        public override string ToString()
        {
            return $"{IdOrdenCompra} - {Proveedor?.RazonSocial}";
        }
    }
}
