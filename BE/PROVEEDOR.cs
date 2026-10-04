using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    // N02: proveedor de insumos (CU022, CU026, CU027). Baja lógica con Activo.
    public class PROVEEDOR
    {
        private int idProveedor;
        public int IdProveedor
        {
            get { return idProveedor; }
            set { idProveedor = value; }
        }

        private string razonSocial;
        public string RazonSocial
        {
            get { return razonSocial; }
            set { razonSocial = value; }
        }

        // Formato XX-XXXXXXXX-X
        private string cuit;
        public string CUIT
        {
            get { return cuit; }
            set { cuit = value; }
        }

        private string telefono;
        public string Telefono
        {
            get { return telefono; }
            set { telefono = value; }
        }

        private string correo;
        public string Correo
        {
            get { return correo; }
            set { correo = value; }
        }

        private bool activo;
        public bool Activo
        {
            get { return activo; }
            set { activo = value; }
        }

        // INSUMO_PROVEEDOR: insumos que ofrece
        private List<INSUMO> insumos = new List<INSUMO>();
        public List<INSUMO> Insumos
        {
            get { return insumos; }
            set { insumos = value; }
        }

        public override string ToString()
        {
            return RazonSocial;
        }
    }
}
