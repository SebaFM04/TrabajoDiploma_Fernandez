using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class USUARIO
    {

        private int idusuario;
        public int IdUsuario
        {
            get { return idusuario; }
            set { idusuario = value; }
        }

        private string nombreUsuario;
        public string NombreUsuario
        {
            get { return nombreUsuario; }
            set { nombreUsuario = value; }
        }

        private string apellidoUsuario;
        public string ApellidoUsuario
        {
            get { return apellidoUsuario; }
            set { apellidoUsuario = value; }
        }

        private int dni;
        public int Dni
        {
            get { return dni; }
            set { dni = value; }
        }

        private string correoElectronico;
        public string CorreoElectronico
        {
            get { return correoElectronico; }
            set { correoElectronico = value; }
        }

        private string contraseñaUsuario;
        public string ContraseñaUsuario
        {
            get { return contraseñaUsuario; }
            set { contraseñaUsuario = value; }
        }

        private List<PERMISOCOMPONENT> permisosAsignados;
        public List<PERMISOCOMPONENT> PermisosAsignados
        {
            get { return permisosAsignados; }
            set { permisosAsignados = value; }
        }

        private int? idIdioma;
        public int? IdIdioma
        {
            get { return idIdioma; }
            set { idIdioma = value; }
        }

        public bool TienePermiso(string nombrePermiso)
        {
            if (PermisosAsignados == null) return false;

            foreach (var permiso in PermisosAsignados)
            {
                if (permiso.ContienePermiso(nombrePermiso))
                    return true;
            }

            return false;
        }

        public override string ToString()
        {
            return CorreoElectronico.ToString();
        }
    }
}
