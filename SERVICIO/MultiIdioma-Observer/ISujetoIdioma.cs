using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIO.MultiIdioma_Observer
{
    public interface ISujetoIdioma
    {
        void Suscribir(IObservadorIdioma obs);
        void Desuscribir(IObservadorIdioma obs);

        void Notificar();
    }
}
