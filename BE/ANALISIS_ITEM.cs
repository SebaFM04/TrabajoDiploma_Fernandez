using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03 CU031: rentabilidad de un producto y tamaño en el período del análisis
    public class ANALISIS_ITEM
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

        private string nombreProducto;
        public string NombreProducto
        {
            get { return nombreProducto; }
            set { nombreProducto = value; }
        }

        private string nombreTamanio;
        public string NombreTamanio
        {
            get { return nombreTamanio; }
            set { nombreTamanio = value; }
        }

        // Bebida o Piqueo
        private string tipoProducto;
        public string TipoProducto
        {
            get { return tipoProducto; }
            set { tipoProducto = value; }
        }

        // ── Ventas del período (simuladas incluidas) ──
        private int unidades;
        public int Unidades
        {
            get { return unidades; }
            set { unidades = value; }
        }

        // Σ MontoLinea
        private decimal ingresos;
        public decimal Ingresos
        {
            get { return ingresos; }
            set { ingresos = value; }
        }

        // Σ CostoLinea (costo real de los lotes consumidos, decisión 72)
        private decimal costo;
        public decimal Costo
        {
            get { return costo; }
            set { costo = value; }
        }

        private decimal margenTotal;
        public decimal MargenTotal
        {
            get { return margenTotal; }
            set { margenTotal = value; }
        }

        private decimal margenUnitario;
        public decimal MargenUnitario
        {
            get { return margenUnitario; }
            set { margenUnitario = value; }
        }

        // Margen total / ingresos × 100
        private decimal margenPorcentaje;
        public decimal MargenPorcentaje
        {
            get { return margenPorcentaje; }
            set { margenPorcentaje = value; }
        }

        // Ingresos del ítem / ingresos totales del período × 100
        private decimal participacion;
        public decimal Participacion
        {
            get { return participacion; }
            set { participacion = value; }
        }

        // ── Situación actual (para armar promociones) ──
        private decimal precioActual;
        public decimal PrecioActual
        {
            get { return precioActual; }
            set { precioActual = value; }
        }

        // Receta × costo de los lotes que se pueden vender
        private decimal costoUnitarioActual;
        public decimal CostoUnitarioActual
        {
            get { return costoUnitarioActual; }
            set { costoUnitarioActual = value; }
        }

        private decimal margenUnitarioActual;
        public decimal MargenUnitarioActual
        {
            get { return margenUnitarioActual; }
            set { margenUnitarioActual = value; }
        }

        // Clasificación por margen y ventas contra el promedio (clave de idioma)
        private string clasificacion;
        public string Clasificacion
        {
            get { return clasificacion; }
            set { clasificacion = value; }
        }

        // ── Resultado del criterio ──
        private bool sugerido;
        public bool Sugerido
        {
            get { return sugerido; }
            set { sugerido = value; }
        }

        // Posición entre los sugeridos (1 = el más conveniente)
        private int orden;
        public int Orden
        {
            get { return orden; }
            set { orden = value; }
        }

        // Número del criterio: índice estacional, días de cobertura o valor en riesgo (0 si no aplica)
        private decimal indicador;
        public decimal Indicador
        {
            get { return indicador; }
            set { indicador = value; }
        }

        // Motivo de la sugerencia: clave de idioma con sus valores ({0} = MotivoInsumo, {1} = MotivoValor1, {2} = MotivoValor2)
        private string motivo;
        public string Motivo
        {
            get { return motivo; }
            set { motivo = value; }
        }

        private string motivoInsumo;
        public string MotivoInsumo
        {
            get { return motivoInsumo; }
            set { motivoInsumo = value; }
        }

        private decimal motivoValor1;
        public decimal MotivoValor1
        {
            get { return motivoValor1; }
            set { motivoValor1 = value; }
        }

        private decimal motivoValor2;
        public decimal MotivoValor2
        {
            get { return motivoValor2; }
            set { motivoValor2 = value; }
        }
    }
}
