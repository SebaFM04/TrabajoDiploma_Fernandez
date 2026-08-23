using BE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace DAL
{
    public class MAPPER_PERMISO
    {
        ACCESO acceso = new ACCESO();

        public List<PERMISOCOMPONENT> ObtenerTodosLosPermisos()
        {
            var lista = new List<PERMISOCOMPONENT>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarPermisos");
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
            {
                bool esFamilia = (bool)row["EsFamilia"];
                PERMISOCOMPONENT p = esFamilia ? (PERMISOCOMPONENT)new PERMISOCOMPOSITE() : new PERMISOATOMICO();

                p.Id = (int)row["Id"];          
                p.NombrePermiso = row["Nombre"].ToString();
                lista.Add(p);
            }
            return lista;
        }
        public List<PERMISOCOMPONENT> ObtenerTodosLosRoles()
        {
            return ObtenerTodosLosPermisos()
                   .Where(p => p.EsFamilia)
                   .ToList();
        }

        public PERMISOCOMPONENT ObtenerPermisoConJerarquiaPorId(int idRaiz)
        {
            var todos = ObtenerTodosLosPermisos();
            var relaciones = ObtenerRelaciones();
            return ConstruirArbol(idRaiz, todos, relaciones);
        }

        private PERMISOCOMPONENT ConstruirArbol(int idNodo, List<PERMISOCOMPONENT> todos, List<(int padre, int hijo)> relaciones)
        {
            var nodoOriginal = todos.Find(p => p.Id == idNodo);
            if (nodoOriginal == null) return null;

            if (nodoOriginal.EsFamilia)
            {
                var composite = new PERMISOCOMPOSITE
                {
                    Id = nodoOriginal.Id,
                    NombrePermiso = nodoOriginal.NombrePermiso
                };
                foreach (var rel in relaciones)
                {
                    if (rel.padre == idNodo)
                    {
                        var hijo = ConstruirArbol(rel.hijo, todos, relaciones);
                        if (hijo != null) composite.AgregarPermiso(hijo);
                    }
                }
                return composite;
            }

            return new PERMISOATOMICO
            {
                Id = nodoOriginal.Id,
                NombrePermiso = nodoOriginal.NombrePermiso
            };
        }

        private List<(int padre, int hijo)> ObtenerRelaciones()
        {
            var lista = new List<(int, int)>();
            acceso.Abrir();
            DataTable tabla = acceso.Leer("ListarPermisosRelaciones");
            acceso.Cerrar();
            foreach (DataRow row in tabla.Rows)
            {
                lista.Add(((int)row["IdPadre"], (int)row["IdHijo"]));
            }              
            return lista;
        }

        public void CrearPermiso(string nombre, bool esFamilia)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@Nombre", nombre),
                new SqlParameter("@EsFamilia", esFamilia)
            };
            acceso.Escribir("AltaPermiso", parametros);
            acceso.Cerrar();
        }

        public void ModificarPermiso(int id, string nuevoNombre, bool esFamilia)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@Id", id),
                acceso.CrearParametro("@Nombre", nuevoNombre),
                new SqlParameter("@EsFamilia", esFamilia)
            };
            acceso.Escribir("ModificarPermiso", parametros);
            acceso.Cerrar();
        }

        public void EliminarPermiso(int id)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@Id", id)
            };
            acceso.Escribir("EliminarPermiso", parametros);
            acceso.Cerrar();
        }

        public void AgregarRelacion(int idPadre, int idHijo)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdPadre", idPadre),
                acceso.CrearParametro("@IdHijo", idHijo)
            };
            acceso.Escribir("AgregarPermisoHijo", parametros);
            acceso.Cerrar();
        }

        public void QuitarRelacion(int idPadre, int idHijo)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdPadre", idPadre),
                acceso.CrearParametro("@IdHijo", idHijo)
            };
            acceso.Escribir("QuitarPermisoHijo", parametros);
            acceso.Cerrar();
        }

        public bool EsHijoDeAlguien(int idPermiso)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdHijo", idPermiso)
            };
            DataTable tabla = acceso.Leer("EsHijoDeAlguien", parametros);
            acceso.Cerrar();
            return tabla.Rows.Count > 0;
        }

        public void AsignarPermisoAUsuario(int idUsuario, int idPermiso)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdUsuario", idUsuario),
                acceso.CrearParametro("@IdPermiso", idPermiso)
            };
            acceso.Escribir("AsignarPermisoAUsuario", parametros);
            acceso.Cerrar();
        }

        public void DesasignarPermisoDeUsuario(int idUsuario, int idPermiso)
        {
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
             acceso.CrearParametro("@IdUsuario", idUsuario),
             acceso.CrearParametro("@IdPermiso", idPermiso)
            };
            acceso.Escribir("DesasignarPermisoDeUsuario", parametros);
            acceso.Cerrar();
        }

        public List<PERMISOCOMPONENT> ListarPermisosJerarquicosPorUsuarioId(int idUsuario)
        {
            var idsPermiso = new List<int>();
            acceso.Abrir();
            var parametros = new List<SqlParameter>
            {
                acceso.CrearParametro("@IdUsuario", idUsuario)
            };
            DataTable tabla = acceso.Leer("ListarPermisosPorUsuario", parametros);
            acceso.Cerrar();

            foreach (DataRow row in tabla.Rows)
                idsPermiso.Add((int)row["IdPermiso"]);

            var todos = ObtenerTodosLosPermisos();
            var relaciones = ObtenerRelaciones();
            var lista = new List<PERMISOCOMPONENT>();

            foreach (int id in idsPermiso)
            {
                var arbol = ConstruirArbol(id, todos, relaciones);
                if (arbol != null) lista.Add(arbol);
            }
            return lista;
        }

        public List<PERMISOCOMPONENT> ObtenerPermisosCompuestosRaiz()
        {
            var todos = ObtenerTodosLosPermisos();
            var relaciones = ObtenerRelaciones();
            var idsHijos = new HashSet<int>();

            foreach (var rel in relaciones)
            {
                idsHijos.Add(rel.hijo);
            }
                
            var resultado = new List<PERMISOCOMPONENT>();
            foreach (var p in todos)
            {
                if (p is PERMISOCOMPOSITE && !idsHijos.Contains(p.Id))
                {
                    resultado.Add(ConstruirArbol(p.Id, todos, relaciones));
                }                
            }
            return resultado;
        }
    }
}