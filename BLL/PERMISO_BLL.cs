using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class PERMISO_BLL
    {
        MAPPER_PERMISO GestorPermiso = new MAPPER_PERMISO();

        public void AsignarPermisoAUsuario(int idUsuario, int idPermiso)
        {
            GestorPermiso.AsignarPermisoAUsuario(idUsuario, idPermiso);
        }

        public void DesasignarPermisoDeUsuario(int idUsuario, int idPermiso)
        {
            GestorPermiso.DesasignarPermisoDeUsuario(idUsuario, idPermiso);
        }

        public void CrearPermiso(string nombre, bool esFamilia)
        {
            if (!esFamilia)
            {
                throw new Exception("Solo se pueden crear roles. Los permisos están precargados en el sistema.");
            }

            if (string.IsNullOrWhiteSpace(nombre))
            { 
                throw new Exception("El nombre del rol no puede estar vacío."); 
            }

            GestorPermiso.CrearPermiso(nombre, esFamilia);
        }

        public void EliminarPermiso(int id)
        {
            GestorPermiso.EliminarPermiso(id);
        }

        public void ModificarPermiso(int id, string nuevoNombre, bool esFamilia)
        {
            GestorPermiso.ModificarPermiso(id, nuevoNombre, esFamilia);
        }
        /*Como estaba antes (Entrega 2)
        public void AgregarRelacion(int idPadre, int idHijo)
        {
            PERMISOCOMPONENT padre = GestorPermiso.ObtenerPermisoConJerarquiaPorId(idPadre);
            PERMISOCOMPONENT hijo = GestorPermiso.ObtenerPermisoConJerarquiaPorId(idHijo);

            if (padre == null)
            {
                throw new Exception($"El permiso padre con ID {idPadre} no existe.");
            }
            if (hijo == null)
            {
                throw new Exception($"El permiso hijo con ID {idHijo} no existe.");
            }
            if (idPadre == idHijo)
            {
                throw new Exception("Un permiso no puede ser hijo de sí mismo.");
            }
            if (!(padre is PERMISOCOMPOSITE))
            {
                throw new Exception($"El permiso '{padre.NombrePermiso}' (ID: {idPadre}) no es un rol compuesto y no puede tener hijos.");
            }

            if (hijo.ContienePermiso(padre.NombrePermiso))
            {
                throw new Exception($"No se admite Referencia circular! No se puede asignar '{hijo.NombrePermiso}' a '{padre.NombrePermiso}' porque '{hijo.NombrePermiso}' ya contiene a '{padre.NombrePermiso}' en su jerarquía.");
            }

            if (padre.ContienePermiso(hijo.NombrePermiso))
            {
                Console.WriteLine($"Advertencia: El rol '{padre.NombrePermiso}' (ID: {idPadre}) ya contiene a '{hijo.NombrePermiso}' (ID: {idHijo}). No se realizará la asignación duplicada.");
                return;
            }
            GestorPermiso.AgregarRelacion(idPadre, idHijo);
        }
        */
        public string MensajeDuplicados { get; private set; }

        public void AgregarRelacion(int idPadre, int idHijo)
        {
            PERMISOCOMPONENT padre = GestorPermiso.ObtenerPermisoConJerarquiaPorId(idPadre);
            PERMISOCOMPONENT hijo = GestorPermiso.ObtenerPermisoConJerarquiaPorId(idHijo);

            if (padre == null)
                throw new Exception($"El permiso padre con ID {idPadre} no existe.");
            if (hijo == null)
                throw new Exception($"El permiso hijo con ID {idHijo} no existe.");
            if (idPadre == idHijo)
                throw new Exception("Un permiso no puede ser hijo de sí mismo.");
            if (!padre.EsFamilia)
                throw new Exception($"El permiso '{padre.NombrePermiso}' (ID: {idPadre}) no es un rol compuesto y no puede tener hijos.");
            if (hijo.ContienePermiso(padre.NombrePermiso))
                throw new Exception($"No se admite Referencia circular! No se puede asignar '{hijo.NombrePermiso}' a '{padre.NombrePermiso}' porque '{hijo.NombrePermiso}' ya contiene a '{padre.NombrePermiso}' en su jerarquía.");
            if (padre.ContienePermiso(hijo.NombrePermiso))
            {
                Console.WriteLine($"Advertencia: El rol '{padre.NombrePermiso}' ya contiene a '{hijo.NombrePermiso}'. No se realizará la asignación duplicada.");
                return;
            }

            // Detectar permisos duplicados
            var permisosPadre = ObtenerPermisosAtomicos(padre);
            var permisosHijo = ObtenerPermisosAtomicos(hijo);
            var duplicados = permisosHijo
                .Where(h => permisosPadre.Any(p => p.NombrePermiso == h.NombrePermiso))
                .Select(p => p.NombrePermiso)
                .ToList();

            MensajeDuplicados = duplicados.Count > 0 ? $"'{hijo.NombrePermiso}' tiene {duplicados.Count} permiso(s) que '{padre.NombrePermiso}' ya posee: {string.Join(", ", duplicados)}." : null;
            GestorPermiso.AgregarRelacion(idPadre, idHijo);
        }

        private List<PERMISOCOMPONENT> ObtenerPermisosAtomicos(PERMISOCOMPONENT nodo)
        {
            var lista = new List<PERMISOCOMPONENT>();
            foreach (var hijo in nodo.ListarPermisosHijos())
            {
                if (!hijo.EsFamilia)
                    lista.Add(hijo);
                else
                    lista.AddRange(ObtenerPermisosAtomicos(hijo));
            }
            return lista;
        }
        public void QuitarRelacion(int idPadre, int idHijo)
        {
            GestorPermiso.QuitarRelacion(idPadre, idHijo);
        }

        public List<PERMISOCOMPONENT> ObtenerTodosLosPermisos()
        {
            return GestorPermiso.ObtenerTodosLosPermisos();
        }

        public bool EsHijoDeAlguien(int idPermiso)
        {
            return GestorPermiso.EsHijoDeAlguien(idPermiso);
        }

        public List<PERMISOCOMPONENT> ObtenerTodosLosRoles()
        {
            return GestorPermiso.ObtenerTodosLosRoles();
        }

        public int ObtenerIdPermisoPorRol(string rol)
        {
            switch (rol.ToLower().Trim())
            {
                case "administrador":
                    return 10;
                case "vendedor":
                    return 11;
                case "encargado de stock":
                    return 12;
                default:
                    return -1;
            }
        }

        public PERMISOCOMPONENT ObtenerPermisoConJerarquiaPorId(int idPermiso)
        {
            return GestorPermiso.ObtenerPermisoConJerarquiaPorId(idPermiso);
        }

        public List<PERMISOCOMPONENT> ListarPermisosJerarquicosPorUsuarioId(int idUsuario)
        {
            return GestorPermiso.ListarPermisosJerarquicosPorUsuarioId(idUsuario);
        }

        public List<PERMISOCOMPONENT> ObtenerPermisosCompuestosRaiz()
        {
            return  GestorPermiso.ObtenerPermisosCompuestosRaiz();
        }
    }
}
