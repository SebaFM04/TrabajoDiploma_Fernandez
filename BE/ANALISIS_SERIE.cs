using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03 CU031: serie de un gráfico del análisis (decisión 83). Se guarda con el análisis para volver a dibujarlo.
    public class ANALISIS_SERIE
    {
        // Gráfico al que pertenece: Evolucion, Participacion, Riesgo o Estacionalidad
        private string grafico;
        public string Grafico
        {
            get { return grafico; }
            set { grafico = value; }
        }

        // Nombre de la serie (clave de idioma o nombre de producto)
        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Linea, Torta o Barras
        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        private List<ANALISIS_PUNTO> puntos = new List<ANALISIS_PUNTO>();
        public List<ANALISIS_PUNTO> Puntos
        {
            get { return puntos; }
            set { puntos = value; }
        }
    }
}
