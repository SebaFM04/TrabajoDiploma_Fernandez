using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Insumo recibido: cantidad en unidades de compra y costo por unidad de compra
    public class RECEPCION_DETALLE
    {
        private int idRecepcion;
        public int IdRecepcion
        {
            get { return idRecepcion; }
            set { idRecepcion = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        private int cantidadRecibida;
        public int CantidadRecibida
        {
            get { return cantidadRecibida; }
            set { cantidadRecibida = value; }
        }

        private decimal costoUnidadCompra;
        public decimal CostoUnidadCompra
        {
            get { return costoUnidadCompra; }
            set { costoUnidadCompra = value; }
        }

        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }
    }
}
