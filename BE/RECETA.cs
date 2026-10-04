using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Línea de receta: cuánto insumo lleva un producto por unidad de magnitud del tamaño (decisión 4)
    public class RECETA
    {
        private int idProducto;
        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        // Cantidad de insumo (en su unidad de medida) por unidad de magnitud del tamaño
        private decimal proporcion;
        public decimal Proporcion
        {
            get { return proporcion; }
            set { proporcion = value; }
        }

        // Asociación con el insumo de la línea (nombre, unidad y si está activo), para mostrar y validar
        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }
    }
}
