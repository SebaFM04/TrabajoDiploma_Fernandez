using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03: promoción de productos y tamaños con precio promocional y vigencia (decisiones 74 a 82)
    public class PROMOCION
    {
        private int idPromocion;
        public int IdPromocion
        {
            get { return idPromocion; }
            set { idPromocion = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Criterio del análisis que la originó: Vencimiento, Estacionalidad, Ventas o Rotación (decisión 75)
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

        // En proceso, Vigente, Finalizada o Renovada (decisión 78)
        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private DateTime fechaCreacion;
        public DateTime FechaCreacion
        {
            get { return fechaCreacion; }
            set { fechaCreacion = value; }
        }

        // CU035: promoción de la que es renovación (null = no es renovación)
        private int? idPromocionOrigen;
        public int? IdPromocionOrigen
        {
            get { return idPromocionOrigen; }
            set { idPromocionOrigen = value; }
        }

        // CU034: Efectiva o No efectiva (null = sin evaluar)
        private string resultado;
        public string Resultado
        {
            get { return resultado; }
            set { resultado = value; }
        }

        private DateTime? fechaEvaluacion;
        public DateTime? FechaEvaluacion
        {
            get { return fechaEvaluacion; }
            set { fechaEvaluacion = value; }
        }

        private string observaciones;
        public string Observaciones
        {
            get { return observaciones; }
            set { observaciones = value; }
        }

        // Promoción de ejemplo del simulador de ventas (se borra con la simulación)
        private bool simulada;
        public bool Simulada
        {
            get { return simulada; }
            set { simulada = value; }
        }

        private List<PROMOCION_DETALLE> detalles = new List<PROMOCION_DETALLE>();
        public List<PROMOCION_DETALLE> Detalles
        {
            get { return detalles; }
            set { detalles = value; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
