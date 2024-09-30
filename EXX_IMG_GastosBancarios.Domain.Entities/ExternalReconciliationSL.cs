using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class ExternalReconciliationSL
    {
        public class ExternalReconciliationModel
        {
            public string ReconciliationAccountType { get; set; }
            //public DateTime ReconciliationDate { get; set; }
            public List<ReconciliationBankStatementLine> ReconciliationBankStatementLines { get; set; }
            public List<ReconciliationJournalEntryLine> ReconciliationJournalEntryLines { get; set; }
        }

        public class ReconciliationBankStatementLine
        {
            public string BankStatementAccountCode { get; set; }
            public int Sequence { get; set; }
        }

        public class ReconciliationJournalEntryLine
        {
            public int LineNumber { get; set; }
            public int TransactionNumber { get; set; }
        }


        public ExternalReconciliationModel ExternalReconciliation { get; set; }


    }
}
