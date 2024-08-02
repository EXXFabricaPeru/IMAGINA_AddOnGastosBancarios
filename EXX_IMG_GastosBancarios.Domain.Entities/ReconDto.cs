using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class ReconDto
    {
        public int TransID { get; set; }

        public int LineIDPago { get; set; }

        public string Cuenta { get; set; }
        public int Secuencia { get; set; }

        public int LineID { get; set; }

        public string Banco { get; set; }

        public string Estado { get; set; }

        public string Sucursal { get; set; }
    }
}
