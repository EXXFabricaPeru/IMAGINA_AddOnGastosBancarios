using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Presentation.Helper
{
    public class Globals
    {
        public static SAPbobsCOM.Company oCompany;
        public static SAPbouiCOM.Application SBO_Application;
        public static string ServiceLayerUrl = "https://172.19.163.3:50000/b1s/v1";
        public static string ExternalReconciliationService = "/ExternalReconciliationsService_Reconcile";
    }
}
