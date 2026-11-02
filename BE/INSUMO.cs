using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class INSUMO
    {
        private int idInsumo;
        public int IdInsumo
        {
            get { return idInsumo; }
            set { idInsumo = value; }
        }

        private string nombre;
        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        // Unidad en que se lleva VolumenPesoDisponible: ml, g o un (decisión 4). No cambia después del alta.
        private string unidadMedida;
        public string UnidadMedida
        {
            get { return unidadMedida; }
            set { unidadMedida = value; }
        }

        private string unidadCompra;
        public string UnidadCompra
        {
            get { return unidadCompra; }
            set { unidadCompra = value; }
        }

        // Cantidad de UnidadMedida que trae una unidad de compra
        private decimal equivalenciaMagnitud;
        public decimal EquivalenciaMagnitud
        {
            get { return equivalenciaMagnitud; }
            set { equivalenciaMagnitud = value; }
        }

        // Stock: solo lo mueven el alta (stock inicial), las ventas y las recepciones de N02
        private decimal volumenPesoDisponible;
        public decimal VolumenPesoDisponible
        {
            get { return volumenPesoDisponible; }
            set { volumenPesoDisponible = value; }
        }

        private decimal umbralReposicion;
        public decimal UmbralReposicion
        {
            get { return umbralReposicion; }
            set { umbralReposicion = value; }
        }

        private decimal costoUnidadCompra;
        public decimal CostoUnidadCompra
        {
            get { return costoUnidadCompra; }
            set { costoUnidadCompra = value; }
        }

        // true = hay un aviso de stock bajo pendiente (decisión 8). Lo maneja el sistema.
        private bool avisoStockBajo;
        public bool AvisoStockBajo
        {
            get { return avisoStockBajo; }
            set { avisoStockBajo = value; }
        }

        // Baja lógica: true = activo, false = dado de baja
        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        // Decisión 58: en qué se usa (bebidas, comidas o ambos). Al menos uno de los dos.
        private bool usoBebidas;
        public bool UsoBebidas
        {
            get { return usoBebidas; }
            set { usoBebidas = value; }
        }

        private bool usoComidas;
        public bool UsoComidas
        {
            get { return usoComidas; }
            set { usoComidas = value; }
        }

        // Decisión 66: perecedero = la fecha de vencimiento de sus lotes es obligatoria
        private bool perecedero;
        public bool Perecedero
        {
            get { return perecedero; }
            set { perecedero = value; }
        }

        // Decisión 68 (calculado, no es columna): lotes Disponible sin fecha o no vencidos.
        // VolumenPesoDisponible es el stock físico (incluye los vencidos hasta que se dan de baja).
        private decimal stockVendible;
        public decimal StockVendible
        {
            get { return stockVendible; }
            set { stockVendible = value; }
        }

        // Calculado: fecha de vencimiento más cercana entre los lotes Disponible (null = ninguno vence)
        private DateTime? proximoVencimiento;
        public DateTime? ProximoVencimiento
        {
            get { return proximoVencimiento; }
            set { proximoVencimiento = value; }
        }

        public override string ToString()
        {
            return Nombre;
        }
    }
}
