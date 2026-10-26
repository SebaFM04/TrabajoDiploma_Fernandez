using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N03 CU034: un producto y tamaño de la promoción (o el total) antes y durante la vigencia
    public class EVALUACION_ITEM
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

        private int unidadesAntes;
        public int UnidadesAntes
        {
            get { return unidadesAntes; }
            set { unidadesAntes = value; }
        }

        private int unidadesDurante;
        public int UnidadesDurante
        {
            get { return unidadesDurante; }
            set { unidadesDurante = value; }
        }

        private decimal ingresosAntes;
        public decimal IngresosAntes
        {
            get { return ingresosAntes; }
            set { ingresosAntes = value; }
        }

        private decimal ingresosDurante;
        public decimal IngresosDurante
        {
            get { return ingresosDurante; }
            set { ingresosDurante = value; }
        }

        // Margen total = ingresos - costo real de lo vendido (CostoLinea)
        private decimal margenAntes;
        public decimal MargenAntes
        {
            get { return margenAntes; }
            set { margenAntes = value; }
        }

        private decimal margenDurante;
        public decimal MargenDurante
        {
            get { return margenDurante; }
            set { margenDurante = value; }
        }

        // Variaciones % (null = sin base: el período anterior fue 0, propuesta 8 de la Fase 6)
        private decimal? variacionUnidades;
        public decimal? VariacionUnidades
        {
            get { return variacionUnidades; }
            set { variacionUnidades = value; }
        }

        private decimal? variacionIngresos;
        public decimal? VariacionIngresos
        {
            get { return variacionIngresos; }
            set { variacionIngresos = value; }
        }

        private decimal? variacionMargen;
        public decimal? VariacionMargen
        {
            get { return variacionMargen; }
            set { variacionMargen = value; }
        }
    }
}
