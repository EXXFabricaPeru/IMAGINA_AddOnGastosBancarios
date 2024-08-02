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
        public string CodProyecto { get; set; }
        public string CodDim1 { get; set; }
        public string CodDim2 { get; set; }
        public string CodDim3 { get; set; }
        public string CodDim4 { get; set; }
        public string CodDim5 { get; set; }
        public string CodParPre { get; set; }
        public string NomParPre { get; set; }
        public string CodMPTraBan { get; set; }

        public string Idsucursal { get; set; }
        public string Idcuenta { get; set; }
        public string IdBanco { get; set; }
        public string Idmoneda { get; set; }
        public string Estado { get; set; }

        public string FechaCont { get; set; }

        
    }
}
