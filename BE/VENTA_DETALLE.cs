using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Línea de la venta: producto, tamaño, cantidad e importe (decisión 6: no tiene IdVale)
    public class VENTA_DETALLE
    {
        private int idVentaDetalle;
        public int IdVentaDetalle
        {
            get { return idVentaDetalle; }
            set { idVentaDetalle = value; }
        }

        private int idVenta;
        public int IdVenta
        {
            get { return idVenta; }
            set { idVenta = value; }
        }

        private int idProducto;
        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private int idTamanio;
        public int IdTamanio
        {
            get { return idTamanio; }
            set { idTamanio = value; }
        }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private decimal montoLinea;
        public decimal MontoLinea
        {
            get { return montoLinea; }
            set { montoLinea = value; }
        }

        // Decisión 72: costo real de los lotes consumidos (FEFO) al vender
        private decimal costoLinea;
        public decimal CostoLinea
        {
            get { return costoLinea; }
            set { costoLinea = value; }
        }

        // Decisión 80: promoción aplicada a la línea (null = precio de lista)
        private int? idPromocion;
        public int? IdPromocion
        {
            get { return idPromocion; }
            set { idPromocion = value; }
        }

        // Producto, tamaño y precio de la línea (para calcular, mostrar y escalar la receta)
        private PRODUCTO_TAMANIO productoTamanio;
        public PRODUCTO_TAMANIO ProductoTamanio
        {
            get { return productoTamanio; }
            set { productoTamanio = value; }
        }
    }
}
