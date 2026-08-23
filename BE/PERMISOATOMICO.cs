using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class PERMISOATOMICO: PERMISOCOMPONENT
    {
        public override List<PERMISOCOMPONENT> ListarPermisosHijos()
        {
            return new List<PERMISOCOMPONENT>();
        }
        public override bool EsFamilia => false;

    }
}
