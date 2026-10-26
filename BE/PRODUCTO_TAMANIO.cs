using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Tabla [PRODUCTO_TAMAÑO]: tamaño en que se vende un producto y su precio (decisión 1)
    public class PRODUCTO_TAMANIO
    {
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

        private decimal precio;
        public decimal Precio
        {
            get { return precio; }
            set { precio = value; }
        }

        // Decisión 80 (calculados, no son columnas): en VENTA_BLL.ListarPreciosVenta, Precio es el que se cobra
        // (el promocional si hay una promoción vigente), PrecioLista el de PRODUCTO_TAMAÑO e IdPromocion la promoción aplicada
        private decimal precioLista;
        public decimal PrecioLista
        {
            get { return precioLista; }
            set { precioLista = value; }
        }

        private int? idPromocion;
        public int? IdPromocion
        {
            get { return idPromocion; }
            set { idPromocion = value; }
        }

        // Datos del tamaño (nombre y magnitud) para mostrar y escalar la receta
        private TAMANIO tamanio;
        public TAMANIO Tamanio
        {
            get { return tamanio; }
            set { tamanio = value; }
        }

        // Nombre del producto, cuando la lista viene de ListarProductoTamanioActivo (venta)
        private PRODUCTO producto;
        public PRODUCTO Producto
        {
            get { return producto; }
            set { producto = value; }
        }

        public override string ToString()
        {
            return (Producto != null ? Producto.Nombre + " - " : "") + (Tamanio != null ? Tamanio.Nombre : IdTamanio.ToString());
        }
    }
}
