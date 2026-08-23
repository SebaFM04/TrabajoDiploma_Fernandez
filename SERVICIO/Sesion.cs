using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SERVICIO
{
    public class Sesion
    {
        public USUARIO UsuarioLogueado { get; private set; }
        public void Login(USUARIO usuario)
        {
            UsuarioLogueado = usuario;
        }
        public void Logout()
        {
            UsuarioLogueado = null;
        }

        public bool IsLogged()
        {
            return UsuarioLogueado != null;
        }
    }
}
