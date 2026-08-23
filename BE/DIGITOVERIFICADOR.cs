using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    public class DIGITOVERIFICADOR
    {
        private string nombreTabla;
        public string NombreTabla
        {
            get { return nombreTabla; }
            set { nombreTabla = value; }
        }

        private string dvv;
        public string DVV
        {
            get { return dvv; }
            set { dvv = value; }
        }
    }
}
