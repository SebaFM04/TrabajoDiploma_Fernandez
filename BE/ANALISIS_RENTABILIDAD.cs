using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03 CU031: análisis de rentabilidad de un período según un criterio (decisiones 74 a 76).
    // No se guarda en la base: se serializa en XML (decisión 85), por eso tiene constructor sin parámetros
    // y propiedades públicas de lectura y escritura.
    public class ANALISIS_RENTABILIDAD
    {
        // Vencimiento, Estacionalidad, Ventas o Rotación (decisión 75)
        private string criterio;
        public string Criterio
        {
            get { return criterio; }
            set { criterio = value; }
        }

        private DateTime fechaDesde;
        public DateTime FechaDesde
        {
            get { return fechaDesde; }
            set { fechaDesde = value; }
        }

        private DateTime fechaHasta;
        public DateTime FechaHasta
        {
            get { return fechaHasta; }
            set { fechaHasta = value; }
        }

        // Criterio Vencimiento: días que cuentan como "por vencer" (7, 15 o 30, decisión 76)
        private int diasPorVencer;
        public int DiasPorVencer
        {
            get { return diasPorVencer; }
            set { diasPorVencer = value; }
        }

        private DateTime fechaGeneracion;
        public DateTime FechaGeneracion
        {
            get { return fechaGeneracion; }
            set { fechaGeneracion = value; }
        }

        // Usuario que lo generó (nombre y correo)
        private string usuario;
        public string Usuario
        {
            get { return usuario; }
            set { usuario = value; }
        }

        // Criterio Estacionalidad: meses con ventas en los 12 meses que terminan en FechaHasta
        private int mesesHistoria;
        public int MesesHistoria
        {
            get { return mesesHistoria; }
            set { mesesHistoria = value; }
        }

        private int totalUnidades;
        public int TotalUnidades
        {
            get { return totalUnidades; }
            set { totalUnidades = value; }
        }

        private decimal totalIngresos;
        public decimal TotalIngresos
        {
            get { return totalIngresos; }
            set { totalIngresos = value; }
        }

        private decimal totalCosto;
        public decimal TotalCosto
        {
            get { return totalCosto; }
            set { totalCosto = value; }
        }

        private decimal totalMargen;
        public decimal TotalMargen
        {
            get { return totalMargen; }
            set { totalMargen = value; }
        }

        // Un ítem por producto y tamaño a la venta
        private List<ANALISIS_ITEM> items = new List<ANALISIS_ITEM>();
        public List<ANALISIS_ITEM> Items
        {
            get { return items; }
            set { items = value; }
        }

        // Avisos del análisis (claves de idioma), por ejemplo historial insuficiente o sin lotes por vencer
        private List<string> avisos = new List<string>();
        public List<string> Avisos
        {
            get { return avisos; }
            set { avisos = value; }
        }

        // Series de los gráficos (decisión 83): se guardan para volver a dibujarlos sin recalcular
        private List<ANALISIS_SERIE> series = new List<ANALISIS_SERIE>();
        public List<ANALISIS_SERIE> Series
        {
            get { return series; }
            set { series = value; }
        }

        // Productos sugeridos para promocionar, en el orden del criterio (método y no propiedad, para que el XML no lo duplique)
        public List<ANALISIS_ITEM> ObtenerSugeridos()
        {
            return Items.Where(i => i.Sugerido).OrderBy(i => i.Orden).ToList();
        }
    }
}
