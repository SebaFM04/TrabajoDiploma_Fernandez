using BE;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIO
{
    public class SessionManager
    {
        private static SessionManager _instancia;
        private Sesion _sesion;

        private SessionManager()
        {
            _sesion = new Sesion();
        }

        public static SessionManager Instancia
        {
            get
            {
                if (_instancia == null)
                    _instancia = new SessionManager();
                return _instancia;
            }
        }

        public void Login(USUARIO usuario)
        {
            _sesion.Login(usuario);
        }

        public void Logout()
        {
            _sesion.Logout();
        }

        public bool IsLogged()
        {
            return _sesion.IsLogged();
        }
        public USUARIO UsuarioActual => _sesion.UsuarioLogueado;
    }
}
