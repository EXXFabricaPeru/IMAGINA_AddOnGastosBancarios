using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class OperacionDto
    {
        public int Id { get; set; }
        public string Seleccionado { get; set; }
        public string CodOperacion { get; set; }
        public string Glosa { get; set; }
        public double Importe { get; set; }
        public string NroCuenta { get; set; }
        public string TipoImporte { get; set; }
        public int NroSecuencia { get; set; }
        public string CodPagoSAP { get; set; }
        public string NroReferencia { get; set; }
    }
}
