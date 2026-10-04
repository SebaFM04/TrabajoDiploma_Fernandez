using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Tabla [TAMAÑO]: tamaño de venta (vaso chico, vaso grande, Único para piqueos, decisión 3)
    public class TAMANIO
    {
        private int idTamanio;
        public int IdTamanio
        {
            get { return idTamanio; }
            set { idTamanio = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Cantidad de la unidad de magnitud que representa el tamaño (350 ml, 1 porción). Escala la receta (decisión 4).
        private decimal cantidadMagnitud;
        public decimal CantidadMagnitud
        {
            get { return cantidadMagnitud; }
            set { cantidadMagnitud = value; }
        }

        private string unidadMagnitud;
        public string UnidadMagnitud
        {
            get { return unidadMagnitud; }
            set { unidadMagnitud = value; }
        }

        // Baja lógica: true = activo, false = dado de baja
        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
