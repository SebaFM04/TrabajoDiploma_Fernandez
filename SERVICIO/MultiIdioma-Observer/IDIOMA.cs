using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIO.MultiIdioma_Observer
{
    public class IDIOMA
    {
        public int IdIdioma { get; set; }
        public string Nombre { get; set; }
        public bool IsDisponible { get; set; }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
