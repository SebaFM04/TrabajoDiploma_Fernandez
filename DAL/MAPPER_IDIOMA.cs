using SERVICIO.MultiIdioma_Observer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MAPPER_IDIOMA
    {
        ACCESO acceso = new ACCESO();
        public List<IDIOMA> ListarIdiomas()
        {
            var lista = new List<IDIOMA>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarIdiomas");
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new IDIOMA
                {
                    IdIdioma = Convert.ToInt32(row["IdIdioma"]),
                    Nombre = row["Nombre"].ToString(),
                    IsDisponible = Convert.ToBoolean(row["IsDisponible"])
                });
            }
            return lista;
        }

        public Dictionary<string, string> ObtenerTraducciones(int idIdioma)
        {
            var traducciones = new Dictionary<string, string>();
            acceso.Abrir();

            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdIdioma", idIdioma)
            };
            DataTable tabla = acceso.Leer("ObtenerTraduccionesPorIdioma", parametros);
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                string clave = row["NombreControl"].ToString();
                traducciones[clave] = row["TextoTraducido"].ToString();
            }
            return traducciones;
        }

        public void ActualizarIdiomaUsuario(int idUsuario, int idIdioma)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdUsuario", idUsuario),
                acceso.CrearParametro("@IdIdioma",  idIdioma)
            };
            acceso.Escribir("ActualizarIdiomaUsuario", parametros);
            acceso.Cerrar();
        }

        public List<TRADUCCION_DETALLE> ObtenerTraduccionesConDetalle(int idIdioma)
        {
            var lista = new List<TRADUCCION_DETALLE>();
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdIdioma", idIdioma)
            };
            DataTable tabla = acceso.Leer("ObtenerTraduccionesConDetalle", parametros);
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(new TRADUCCION_DETALLE
                {
                    IdControl = Convert.ToInt32(row["IdControl"]),
                    NombreControl = row["NombreControl"].ToString(),
                    NombreFormulario = row["NombreFormulario"].ToString(),
                    TextoTraducido = row["TextoTraducido"].ToString()
                });
            }
            return lista;
        }
        public int AgregarIdioma(string nombre)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter> { acceso.CrearParametro("@Nombre", nombre) };
            DataTable tabla = acceso.Leer("AgregarIdioma", parametros);
            acceso.Cerrar();

            if (tabla.Rows.Count == 0)
            {
                throw new Exception("No se pudo crear el idioma. El procedimiento no devolvió el ID.");
            }
            return Convert.ToInt32(tabla.Rows[0]["IdIdioma"]);
        }

        public void ModificarNombreIdioma(int idIdioma, string nombre)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdIdioma", idIdioma),
                acceso.CrearParametro("@Nombre", nombre)
            };
            acceso.Escribir("ModificarNombreIdioma", parametros);
            acceso.Cerrar();
        }

        public bool ToggleDisponibilidad(int idIdioma)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdIdioma", idIdioma)
            };
            DataTable tabla = acceso.Leer("ToggleDisponibilidadIdioma", parametros);
            acceso.Cerrar();
            return Convert.ToBoolean(tabla.Rows[0]["IsDisponible"]);
        }

        public void ModificarTraduccion(int idControl, int idIdioma, string texto)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdControl", idControl),
                acceso.CrearParametro("@IdIdioma", idIdioma),
                acceso.CrearParametro("@TextoTraducido", texto)
            };
            acceso.Escribir("ModificarTraduccion", parametros);
            acceso.Cerrar();
        }
    }
}
