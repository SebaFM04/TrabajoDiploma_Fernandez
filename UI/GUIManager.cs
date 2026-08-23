using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UI
{
    public class GUIManager
    {
        private static GUIManager _instancia;
        public static GUIManager Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new GUIManager();
                return _instancia;
            }
        }

        public void AbrirForm(Form form)
        {
            GestorIdioma.Instancia.Suscribir(form as IObservadorIdioma);
            form.ShowDialog();
            GestorIdioma.Instancia.Desuscribir(form as IObservadorIdioma);
        }
    }
}
