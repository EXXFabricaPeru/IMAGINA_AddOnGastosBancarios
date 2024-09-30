using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class RespuestaPagoModel
    {
        public int DocEntry { get; set; }
        public int TipoPago { get; set; }
        public bool Procesado { get; set; }
        public string Mensaje { get; set; }
        public int LineNum { get; set; }
    }
}
