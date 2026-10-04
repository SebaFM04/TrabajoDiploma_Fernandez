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

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Valores posibles: Bebida, Piqueo
        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        // Baja lógica: true = activo, false = dado de baja
        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
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
