using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public abstract class PERMISOCOMPONENT
    {
        private int id;
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        private string nombrePermiso;
        public string NombrePermiso
        {
            get { return nombrePermiso; }
            set { nombrePermiso = value; }
        }

        public abstract List<PERMISOCOMPONENT> ListarPermisosHijos();

        public override string ToString()
        {
            return NombrePermiso;
        }

        public bool ContienePermiso(string nombrePermisoABuscar)
        {
            if (NombrePermiso == nombrePermisoABuscar)
                return true;

            foreach (var hijo in ListarPermisosHijos())
            {

                if (hijo != null && hijo.ContienePermiso(nombrePermisoABuscar))
                    return true;
            }

            return false;
        }
        public abstract bool EsFamilia { get; }

    }
}
