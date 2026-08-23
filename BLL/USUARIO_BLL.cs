using DAL;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class USUARIO_BLL
    {
        MAPPER_USUARIO GestorUsuario = new MAPPER_USUARIO();
        
        public BE.USUARIO LoginUsuario(string Correo, string contraseña)
        {
            if (SessionManager.Instancia.IsLogged()) throw new Exception("Ya hay una sesión iniciada.");
            var user  = GestorUsuario.BuscarUsuario( Correo, contraseña );
            if (user == null) throw new Exception("Usuario no encontrado.");
            
            // Cargar permisos del usuario antes de guardarlo en sesión
            user.PermisosAsignados = new PERMISO_BLL().ListarPermisosJerarquicosPorUsuarioId(user.IdUsuario);
            
            SessionManager.Instancia.Login(user);
            // aca inicializamos el idioma del usuario!
            new IDIOMA_BLL().InicializarIdiomaUsuario(user.IdIdioma); //si todavia no tiene idioma, se le asigna el default q es español por ahora.

            new BITACORA_BLL().RegistrarEvento(user.IdUsuario, "Inicio de sesion", $"Usuario: {user.CorreoElectronico}");
            return user;
        }

        public void LogoutUsuario(){
            var user = SessionManager.Instancia.UsuarioActual;
            SessionManager.Instancia.Logout();
            new BITACORA_BLL().RegistrarEvento(user.IdUsuario, "Cierre de sesion", $"El Usuario: {user.CorreoElectronico} Cerró la sesion.");
        }

        public void RegistrarUsuario(BE.USUARIO usuario)
        {
            GestorUsuario.AltaUsuario(usuario);
            new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Alta de usuario", $"Se agrego el usuario: {usuario.CorreoElectronico}");
        }

        public BE.USUARIO BuscarUsuarioEnBD(string Correo, string Contraseña)
        {
            return new MAPPER_USUARIO().BuscarUsuario(Correo, Contraseña);
        }

        public int EliminarUsuario(BE.USUARIO usuario)
        {
            BE.USUARIO usuarioBorrado = usuario;
            int filas = GestorUsuario.BajaUsuario(usuario);
            try
            {
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Baja de usuario", $"Se eliminó el usuario: {usuarioBorrado.CorreoElectronico}");
                }
            }
            catch
            {
                // No interrumpir por fallos en bitácora
            }
            return filas;
        }

        public int ModificarUsuario(BE.USUARIO usuario)
        {
            int filas = GestorUsuario.EditarUsuario(usuario);
            try
            {
                if (SessionManager.Instancia != null && SessionManager.Instancia.IsLogged())
                {
                    new BITACORA_BLL().RegistrarEvento(SessionManager.Instancia.UsuarioActual.IdUsuario, "Edición de usuario", $"Se modificó el usuario: {usuario.CorreoElectronico}");
                }
            }
            catch
            {
                // No interrumpir por fallos en bitácora
            }
            return filas;
        }

        public List<BE.USUARIO> ListarUsuarios()
        {
            return GestorUsuario.ListarUsuarios();
        }
    }
}
