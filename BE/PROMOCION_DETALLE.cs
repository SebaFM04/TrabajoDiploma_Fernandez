using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Producto y tamaño de una promoción, con su precio original, el promocional y el costo al armarla
    public class PROMOCION_DETALLE
    {
        private int idPromocion;
        public int IdPromocion
        {
            get { return idPromocion; }
            set { idPromocion = value; }
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

        private decimal precioOriginal;
        public decimal PrecioOriginal
        {
            get { return precioOriginal; }
            set { precioOriginal = value; }
        }

        private decimal precioPromocional;
        public decimal PrecioPromocional
        {
            get { return precioPromocional; }
            set { precioPromocional = value; }
        }

        // Costo de una unidad al armar la promoción (receta × costo de los insumos)
        private decimal costoUnitario;
        public decimal CostoUnitario
        {
            get { return costoUnitario; }
            set { costoUnitario = value; }
        }

        // Producto y tamaño (nombres, para mostrar)
        private PRODUCTO_TAMANIO productoTamanio;
        public PRODUCTO_TAMANIO ProductoTamanio
        {
            get { return productoTamanio; }
            set { productoTamanio = value; }
        }

        // Decisión 77 (calculados): margen unitario y margen % con el precio promocional; pueden ser negativos
        public decimal MargenUnitario
        {
            get { return PrecioPromocional - CostoUnitario; }
        }

        public decimal MargenPorcentaje
        {
            get { return PrecioPromocional > 0 ? Math.Round(MargenUnitario / PrecioPromocional * 100, 1) : 0; }
        }
    }
}
