using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIO.MultiIdioma_Observer
{
    public class GestorIdioma : ISujetoIdioma
    {
        // Singleton 
        private static GestorIdioma _instancia;
        public static GestorIdioma Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new GestorIdioma();
                return _instancia;
            }
        }

        private GestorIdioma()
        {
            _idIdiomaActual = 2; // Inglés por defecto
            traducciones = new Dictionary<string, string>();
            observadores = new List<IObservadorIdioma>();
        }

        //  Estado 
        private int _idIdiomaActual;
        private Dictionary<string, string> traducciones;
        private readonly List<IObservadorIdioma> observadores;
        public int IdIdiomaActual => _idIdiomaActual;

        //Traducción
        public string Traducir(string nombreControl)
        {
            try { return traducciones[nombreControl]; }
            catch { return nombreControl; }
        }

        //  Gestión de observadores 
        public void Suscribir(IObservadorIdioma observador)
        {
            if (observador == null) return;
            if (!observadores.Contains(observador))
            {
                observadores.Add(observador);
            }

            // Notificación inmediata si ya hay traducciones
            if (traducciones.Count > 0)
            {
                observador.ActualizarIdioma();
            }
        }

        public void Desuscribir(IObservadorIdioma observador)
        {
            if (observador != null)
            {
                observadores.Remove(observador);
            }
        }
        

        //  Cambio de idioma 
        public void CambiarIdioma(int idIdioma, Dictionary<string, string> traducciones)
        {
            _idIdiomaActual = idIdioma;
            this.traducciones = traducciones ?? new Dictionary<string, string>();
            Notificar();
        }

        private void Notificar()
        {
            foreach (var obs in new List<IObservadorIdioma>(observadores))
            {
                try { obs.ActualizarIdioma(); }
                catch { /* No interrumpir al resto si un form ya fue cerrado */ }
            }
        }

        void ISujetoIdioma.Notificar() => Notificar();
    }
}