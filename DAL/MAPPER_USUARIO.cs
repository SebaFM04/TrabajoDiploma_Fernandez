using BE;
using SERVICIO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_USUARIO
    {
        ACCESO acceso = new ACCESO();

        public int AltaUsuario(BE.USUARIO Usuario)
        {
            string NombreSp = "AltaUsuario";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@CorreoElectronico", Usuario.CorreoElectronico));
            // Guardar la contraseña hasheada
            parametros.Add(acceso.CrearParametro("@ContraseñaUsuario", ENCRIPTADOR.Hash(Usuario.ContraseñaUsuario)));
            parametros.Add(acceso.CrearParametro("@NombreUsuario", Usuario.NombreUsuario));
            parametros.Add(acceso.CrearParametro("@ApellidoUsuario", Usuario.ApellidoUsuario));
            parametros.Add(acceso.CrearParametro("@Dni", Usuario.Dni));

            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }
  
        public int BajaUsuario(BE.USUARIO Usuario)
        {
            string NombreSp = "BajaUsuario";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdUsuario", Usuario.IdUsuario));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }

        public int EditarUsuario(BE.USUARIO Usuario)
        {
            string NombreSp = "ModificarUsuario";
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@IdUsuario", Usuario.IdUsuario));
            parametros.Add(acceso.CrearParametro("@CorreoElectronico", Usuario.CorreoElectronico));
            // Guardar la contraseña hasheada
            parametros.Add(acceso.CrearParametro("@ContraseñaUsuario", ENCRIPTADOR.Hash(Usuario.ContraseñaUsuario)));
            parametros.Add(acceso.CrearParametro("@NombreUsuario", Usuario.NombreUsuario));
            parametros.Add(acceso.CrearParametro("@ApellidoUsuario", Usuario.ApellidoUsuario));
            parametros.Add(acceso.CrearParametro("@Dni", Usuario.Dni));
            int filas = acceso.Escribir(NombreSp, parametros);
            acceso.Cerrar();
            return filas;
        }
       
        public BE.USUARIO BuscarUsuario(string CorreoElectronico, string ContraseñaUsuario)
        {
            acceso.Abrir();
            List<SqlParameter> parametros = new List<SqlParameter>();
            parametros.Add(acceso.CrearParametro("@CorreoElectronico", CorreoElectronico));
            parametros.Add(acceso.CrearParametro("@ContraseñaUsuario", ENCRIPTADOR.Hash(ContraseñaUsuario)));

            DataTable tabla = acceso.Leer("LeerUsuario", parametros);
            acceso.Cerrar();
            if (tabla.Rows.Count > 0)
            {
                DataRow u = tabla.Rows[0];
                BE.USUARIO C = new BE.USUARIO();
                C.CorreoElectronico = u["CorreoElectronico"].ToString();
                C.ContraseñaUsuario = u["ContraseñaUsuario"].ToString();
                C.NombreUsuario = u["NombreUsuario"].ToString();
                C.ApellidoUsuario = u["ApellidoUsuario"].ToString();
                C.Dni = int.Parse(u["Dni"].ToString());
                C.IdIdioma = u["IdIdioma"] != DBNull.Value ? (int?)int.Parse(u["IdIdioma"].ToString()) : null;
                C.IdUsuario = int.Parse(u["IdUsuario"].ToString());
                return C;
            }
            else
            {
                return null;
            }
        }

        public List<BE.USUARIO> ListarUsuarios()
        {
            List<BE.USUARIO> listaUsuarios = new List<BE.USUARIO>();
            string NombreSp = "ListarUsuario";
            acceso.Abrir();

            DataTable tabla = new DataTable();
            tabla = acceso.Leer(NombreSp);
            acceso.Cerrar();
            foreach (DataRow u in tabla.Rows)
            {
                BE.USUARIO usuario = new BE.USUARIO();

                usuario.IdUsuario = Convert.ToInt32(u["IdUsuario"].ToString());
                usuario.CorreoElectronico = (u["CorreoElectronico"].ToString());
                usuario.ContraseñaUsuario = (u["ContraseñaUsuario"].ToString());
                usuario.NombreUsuario = u["NombreUsuario"].ToString();
                usuario.ApellidoUsuario = u["ApellidoUsuario"].ToString();
                usuario.Dni = int.Parse(u["Dni"].ToString());
                listaUsuarios.Add(usuario);
            }
            return  listaUsuarios;
        }
    }
}
