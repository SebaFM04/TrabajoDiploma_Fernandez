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

        public override string ToString()
        {
            return Nombre;
        }
    }
}
