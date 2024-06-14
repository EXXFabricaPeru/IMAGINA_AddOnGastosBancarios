using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities.BF
{
    public class FormattedSearchDto
    {
        public string sqlQuery { get; set; }
        public string Name { get; set; }

        public string FormID { get; set; }
        public string ItemID { get; set; }
        public string ColumnID { get; set; }
        public int userQueryId { get; set; }
    }
}
