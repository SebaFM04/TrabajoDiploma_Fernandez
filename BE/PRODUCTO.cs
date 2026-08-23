using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PRODUCTO
    {
        private int idProducto;
        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private string nombreProducto;

        public string NombreProducto
        {
            get { return nombreProducto; }
            set { nombreProducto = value; }
        }


        private decimal precioProducto;
        public decimal PrecioProducto
        {
            get { return precioProducto; }
            set { precioProducto = value; }
        }

        private string tipoProducto;
        public string TipoProducto
        {
            get { return tipoProducto; }
            set { tipoProducto = value; }
        }

        private string descripcion;
        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        private int cantidad;
        public int Cantidad
        {
            get { return cantidad; }
            set { cantidad = value; }
        }

        private int codigoProducto;
        public int CodigoProducto
        {
            get { return codigoProducto; }
            set { codigoProducto = value; }
        }

        private string dvh;
        public string DVH
        {
            get { return dvh; }
            set { dvh = value; }
        }


        public override string ToString()
        {
            return IdProducto.ToString();
        }
    }
}
