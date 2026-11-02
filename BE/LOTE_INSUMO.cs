using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // Lote de un insumo (decisiones 65 a 71): una recepción de cada insumo o el stock inicial del alta.
    // Cantidades en la unidad de medida del insumo; el costo, por unidad de compra.
    public class LOTE_INSUMO
    {
        private int idLote;
        public int IdLote
        {
            get { return idLote; }
            set { idLote = value; }
        }

        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        // null = stock inicial del alta del insumo (CU023)
        private int? idRecepcion;
        public int? IdRecepcion
        {
            get { return idRecepcion; }
            set { idRecepcion = value; }
        }

        private DateTime fechaIngreso;
        public DateTime FechaIngreso
        {
            get { return fechaIngreso; }
            set { fechaIngreso = value; }
        }

        // null = no vence (decisión 66)
        private DateTime? fechaVencimiento;
        public DateTime? FechaVencimiento
        {
            get { return fechaVencimiento; }
            set { fechaVencimiento = value; }
        }

        private decimal cantidadInicial;
        public decimal CantidadInicial
        {
            get { return cantidadInicial; }
            set { cantidadInicial = value; }
        }

        private decimal cantidadDisponible;
        public decimal CantidadDisponible
        {
            get { return cantidadDisponible; }
            set { cantidadDisponible = value; }
        }

        private decimal costoUnidadCompra;
        public decimal CostoUnidadCompra
        {
            get { return costoUnidadCompra; }
            set { costoUnidadCompra = value; }
        }

        // Disponible, Agotado o Vencido (dado de baja en CU036)
        private string estado;
        public string Estado
        {
            get { return estado; }
            set { estado = value; }
        }

        private DateTime? fechaBaja;
        public DateTime? FechaBaja
        {
            get { return fechaBaja; }
            set { fechaBaja = value; }
        }

        private decimal? cantidadBaja;
        public decimal? CantidadBaja
        {
            get { return cantidadBaja; }
            set { cantidadBaja = value; }
        }

        private INSUMO insumo;
        public INSUMO Insumo
        {
            get { return insumo; }
            set { insumo = value; }
        }

        // Decisión 73: costo por unidad de medida = costo por unidad de compra / equivalencia
        public decimal CostoUnidadMedida
        {
            get { return Insumo != null && Insumo.EquivalenciaMagnitud > 0 ? CostoUnidadCompra / Insumo.EquivalenciaMagnitud : 0; }
        }

        // Decisión 68: vencido = tiene fecha anterior a hoy
        public bool Vencido
        {
            get { return FechaVencimiento.HasValue && FechaVencimiento.Value.Date < DateTime.Today; }
        }
    }
}
