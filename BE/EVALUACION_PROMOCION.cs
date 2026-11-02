using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03 CU034 (decisión 81): comparación de la vigencia de una promoción contra el período anterior de igual
    // duración, solo para sus productos y tamaños. Es un resultado de cálculo: no es una tabla.
    public class EVALUACION_PROMOCION
    {
        private PROMOCION promocion;
        public PROMOCION Promocion
        {
            get { return promocion; }
            set { promocion = value; }
        }

        // Período anterior de igual duración (el "durante" es la vigencia de la promoción)
        private DateTime antesDesde;
        public DateTime AntesDesde
        {
            get { return antesDesde; }
            set { antesDesde = value; }
        }

        private DateTime antesHasta;
        public DateTime AntesHasta
        {
            get { return antesHasta; }
            set { antesHasta = value; }
        }

        // Un ítem por producto y tamaño de la promoción
        private List<EVALUACION_ITEM> items = new List<EVALUACION_ITEM>();
        public List<EVALUACION_ITEM> Items
        {
            get { return items; }
            set { items = value; }
        }

        private EVALUACION_ITEM total;
        public EVALUACION_ITEM Total
        {
            get { return total; }
            set { total = value; }
        }

        // Efectiva si el margen total no bajó y las unidades subieron; si no, No efectiva (decide el Dueño)
        private string resultadoSugerido;
        public string ResultadoSugerido
        {
            get { return resultadoSugerido; }
            set { resultadoSugerido = value; }
        }

        // Avisos (claves de idioma), por ejemplo sin ventas en ninguno de los dos períodos
        private List<string> avisos = new List<string>();
        public List<string> Avisos
        {
            get { return avisos; }
            set { avisos = value; }
        }

        // Unidades por día de los productos de la promoción en los dos períodos (gráfico con la vigencia marcada)
        private ANALISIS_SERIE serie;
        public ANALISIS_SERIE Serie
        {
            get { return serie; }
            set { serie = value; }
        }
    }
}
