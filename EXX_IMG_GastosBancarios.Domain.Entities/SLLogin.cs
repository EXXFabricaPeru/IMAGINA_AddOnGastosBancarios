using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class SLLogin
    {
        public string B1SESSION { get; set; }
        public string path { get; set; }
        public int HttpOnly { get; set; }
        public string SameSite { get; set; }
        public string ROUTEID { get; set; }
    }
}
