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

        // Decisión 65: vencimiento del lote que genera esta línea. No es columna de RECEPCION_DETALLE:
        // se guarda en LOTE_INSUMO (null = no vence; obligatoria si el insumo es perecedero).
        private DateTime? fechaVencimiento;
        public DateTime? FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }
    }
}
