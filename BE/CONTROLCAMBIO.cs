using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class CONTROLCAMBIO
    {
        private int idCambio;
        public int IdCambio
        {
            get { return idCambio; }
            set { idCambio = value; }
        }

        private int idUsuario;
        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

        private int idProducto;
        public int IdProducto
        {
            get { return idProducto; }
            set { idProducto = value; }
        }

        private string campoModificado;
        public string CampoModificado
        {
            get { return campoModificado; }
            set { campoModificado = value; }
        }

        private string valorAnterior;
        public string ValorAnterior
        {
            get { return valorAnterior; }
            set { valorAnterior = value; }
        }

        private string valorActual;
        public string ValorActual
        {
            get { return valorActual; }
            set { valorActual = value; }
        }

        private DateTime fechaModificacion;
        public DateTime FechaModificacion
        {
            get { return fechaModificacion; }
            set { fechaModificacion = value; }
        }

        private string tipoOperacion;
        public string TipoOperacion
        {
            get { return tipoOperacion; }
            set { tipoOperacion = value; }
        }
    }
}
