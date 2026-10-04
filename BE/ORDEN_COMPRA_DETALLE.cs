using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Línea de la orden: insumo y cantidades en unidades de compra
    public class ORDEN_COMPRA_DETALLE
    {
        private int idOrdenCompra;
        public int IdOrdenCompra
        {
            get { return idOrdenCompra; }
            set { idOrdenCompra = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        private int cantidadPedida;
        public int CantidadPedida
        {
            get { return cantidadPedida; }
            set { cantidadPedida = value; }
        }

        private int cantidadRecibida;
        public int CantidadRecibida
        {
            get { return cantidadRecibida; }
            set { cantidadRecibida = value; }
        }

        // Lo que falta recibir (CU019)
        public int CantidadPendiente
        {
            get { return CantidadPedida - CantidadRecibida; }
        }

        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }
    }
}
