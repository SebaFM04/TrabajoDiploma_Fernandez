using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Punto de una serie de gráfico: etiqueta del eje (fecha, mes, tipo o insumo) y valor
    public class ANALISIS_PUNTO
    {
        private string etiqueta;
        public string Etiqueta
        {
            get { return etiqueta; }
            set { etiqueta = value; }
        }

        private decimal valor;
        public decimal Valor
        {
            get { return valor; }
            set { valor = value; }
        }
    }
}
