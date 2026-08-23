using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PERMISOCOMPOSITE:PERMISOCOMPONENT
    {
        public List<PERMISOCOMPONENT> PermisosIncluidos { get; set; } = new List<PERMISOCOMPONENT>();

        public void AgregarPermiso(PERMISOCOMPONENT permiso)
        {
            PermisosIncluidos.Add(permiso);
        }

        public void QuitarPermiso(PERMISOCOMPONENT permiso)
        {
            PermisosIncluidos.Remove(permiso);
        }

        public override List<PERMISOCOMPONENT> ListarPermisosHijos()
        {
            return PermisosIncluidos;
        }
        public override bool EsFamilia => true;

    }
}
