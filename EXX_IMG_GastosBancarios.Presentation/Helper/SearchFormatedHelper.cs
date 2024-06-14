using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAPbobsCOM;
using SAPbouiCOM.Framework;
using EXX_Metadata.BL;
using JF_SBOAddon.Utiles.Extensions;
using System.IO;
using System.Resources;
using System.Collections;
using EXX_IMG_GastosBancarios.Domain.Entities.BF;

namespace EXX_IMG_GastosBancarios.Presentation.Helper
{
    public class SearchFormatedHelper
    {

        public static string CategoriesName = "9980. Búsquedas Formateadas Exxis - Gtos Bancarios";


        public static void Initialize(Company oCompany, Version version, string companyPrefix, string addonID)
        {

            Application.SBO_Application.SetStatusWarningMessage("Iniciando Creación de BF");

            //ValidVersion(oCompany, version, companyPrefix, addonID);

            var IdCategory = CreateCategoryFormattedSearch(CategoriesName, oCompany);
            Application.SBO_Application.SetStatusWarningMessage("IdCategory: " + IdCategory);
            CreateFormattedSearchByList(IdCategory, oCompany);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private static bool ValidVersion(Company oCompany, Version version, string companyPrefix, string addonID)
        {
            try
            {
                string sQuery;
                sQuery = $@"select * from ""@EXX_SETUP"" where ""U_EXX_ADDN"" = '{ addonID }'";
                string str = "";
                var oRec = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(sQuery);

                while (!oRec.EoF)
                {
                    str = oRec.Fields.Item("U_EXX_VERS").Value.ToString();
                }

                if (version.ToString() != str)
                {
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static void CreateFormattedSearchByList(int idCategory, Company oCompany)
        {
            var list = SetListFormattedSearch();

            foreach (var item in list)
            {
                DesassingFomattedSearch(oCompany, item.FormID, item.ItemID, item.ColumnID);
                DropIfExistFormattedSearch(oCompany, item.sqlQuery, item.name, idCategory);

                var idFs = CreateFormattedSearch(oCompany, item.sqlQuery, item.name, idCategory);

                AssignCreateFormattedSearch(oCompany, item.FormID, item.ItemID, item.ColumnID, idFs);

            }
        }

        private static void DesassingFomattedSearch(Company company, string FormID, string ItemID, string ColumnID)
        {
            try
            {
                Application.SBO_Application.SetStatusWarningMessage("DesassingFomattedSearch");
                FormattedSearches formattedSearch = (FormattedSearches)company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
                var idAPPFS = indexFMS(company, FormID, ItemID, ColumnID);
                if (idAPPFS != -1)
                {
                    formattedSearch.GetByKey(idAPPFS);
                    int ret=formattedSearch.Remove();
                    if (ret == 0)
                    {
                        Application.SBO_Application.SetStatusWarningMessage("DesassingFomattedSearch  BF OK ");
                    }
                    else
                    {
                        Application.SBO_Application.SetStatusWarningMessage("DesassingFomattedSearch Error a BF: " + company.GetLastErrorDescription());
                    }
                    if (formattedSearch != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(formattedSearch);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void DropIfExistFormattedSearch(Company oCompany, string sqlQuery, string name, int idCategory)
        {
            try
            {
                var id = GetInternalKeyBF(oCompany, name, idCategory);
                if (id > 0)
                {
                    UserQueries userQuery = (UserQueries)oCompany.GetBusinessObject(BoObjectTypes.oUserQueries);
                    userQuery.GetByKey(id, idCategory);

                    int ret=userQuery.Remove();

                    if (ret == 0)
                    {
                        Application.SBO_Application.SetStatusWarningMessage("DropIfExistFormattedSearch  BF: " + name);
                    }
                    else
                    {
                        Application.SBO_Application.SetStatusWarningMessage("DropIfExistFormattedSearch Error a BF: " + oCompany.GetLastErrorDescription());
                    }
                    if (userQuery != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(userQuery);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                Application.SBO_Application.SetStatusErrorMessage(ex.StackTrace);
                Application.SBO_Application.SetStatusErrorMessage(ex.InnerException.Message);
            }
            finally
            {
                GC.Collect();
            }
        }

        public static int CreateCategoryFormattedSearch(string NomeCategoria, Company company)
        {
            int iRet = 0;
            try
            {
                //verifica se existe a categoria
                string sQuery;
                sQuery = $@"select ""CategoryId"" from OQCN where ""CatName"" = '{NomeCategoria}'";

                var oRec = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(sQuery);

                while (!oRec.EoF)
                {
                    return Convert.ToInt32(oRec.Fields.Item(0).Value.ToString());
                }



                SAPbobsCOM.QueryCategories oQCat;

                oQCat = (SAPbobsCOM.QueryCategories)company.GetBusinessObject(BoObjectTypes.oQueryCategories);
                //fim teste
                oQCat.Name = NomeCategoria;
                iRet = oQCat.Add();

                string errMessage;
                company.GetLastError(out iRet, out errMessage);
                if (oQCat != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(oQCat);
                }

                string strCode = "";
                company.GetNewObjectCode(out strCode);

                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
                Application.SBO_Application.SetStatusWarningMessage("Se creó la categoría de BF: " + strCode);
                return Convert.ToInt32(strCode);
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                Application.SBO_Application.SetStatusErrorMessage(ex.StackTrace);
                Application.SBO_Application.SetStatusErrorMessage(ex.InnerException.Message);
            }

            return (iRet);
        }


        public static int CreateFormattedSearch(Company company, string sqlQuery, string name, int idCategories)
        {
            try
            {
                UserQueries userQuery = (UserQueries)company.GetBusinessObject(BoObjectTypes.oUserQueries);
                userQuery.Query = sqlQuery;
                userQuery.QueryCategory = idCategories; // Categoría de la consulta, -1 para "Mis Consultas"
                userQuery.QueryDescription = name;

                int queryId = userQuery.Add();
                if (queryId == 0)
                {
                    Console.WriteLine("User query added successfully.");
                    string strCode = "";
                    //company.GetNewObjectCode(out strCode);



                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Application.SBO_Application.SetStatusWarningMessage("Se creó la BF: " + name);
                    if (userQuery != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(userQuery);
                    }
                    return GetInternalKeyBF(company, name, idCategories);
                }
                else
                {
                    Application.SBO_Application.SetStatusWarningMessage("Error al crear la BF: " + name);
                    Console.WriteLine($"Failed to add user query. Error code: {company.GetLastErrorCode()} - {company.GetLastErrorDescription()}");
                    return 0;
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                Application.SBO_Application.SetStatusErrorMessage(ex.StackTrace);
                Application.SBO_Application.SetStatusErrorMessage(ex.InnerException.Message);
            }
            finally
            {
                GC.Collect();
            }

            return 0;
        }

        public static int GetInternalKeyBF(Company company, string name, int idCategories)
        {
            string sQuery;
            sQuery = $@"select ""IntrnalKey"" from ""OUQR"" where ""QCategory""={idCategories} and ""QName""='{name}'";

            var oRec = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRec.DoQuery(sQuery);

            while (!oRec.EoF)
            {
                return Convert.ToInt32(oRec.Fields.Item(0).Value.ToString());
            }
            return 0;

        }

        public static void AssignCreateFormattedSearch(Company company, string FormID, string ItemID, string ColumnID, int userQueryId)
        {
            try
            {
                // Crear la búsqueda formateada
                Application.SBO_Application.SetStatusWarningMessage("AssignCreateFormattedSearch");
                FormattedSearches formattedSearch = (FormattedSearches)company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
                var idAPPFS = indexFMS(company, FormID, ItemID, ColumnID);
                if (idAPPFS != -1)
                {
                    formattedSearch.GetByKey(idAPPFS);
                }
                else
                {

                    formattedSearch.FormID = FormID;// "UDO_FT_EXX_ANXN_CONF";// Entities.EXX_ANXN_COND.ID; // Form ID del UDO
                    formattedSearch.ItemID = ItemID;// "0_U_G"; // ID del campo en el formulario
                    formattedSearch.ColumnID = ColumnID;// "C_0_8"; // Nombre del campo en la tabla

                }

                formattedSearch.Action = BoFormattedSearchActionEnum.bofsaQuery;
                formattedSearch.QueryID = userQueryId;
                formattedSearch.ByField = BoYesNoEnum.tYES; // Nombre del campo en la tabla

                if (idAPPFS == -1)
                {
                    int ret = formattedSearch.Add();
                    if (ret != 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Application.SBO_Application.SetStatusWarningMessage("Falló la asignación Add: " + company.GetLastErrorDescription());
                    }
                    else
                    {
                        Console.WriteLine("Formatted search added successfully.");
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Application.SBO_Application.SetStatusWarningMessage("Se asignó la BF: " + ItemID);
                    }
                }
                else
                {
                    int ret = formattedSearch.Update();
                    if (ret != 0)
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Application.SBO_Application.SetStatusWarningMessage("Falló la asignación Update: " + company.GetLastErrorDescription());
                    }
                    else
                    {
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        Application.SBO_Application.SetStatusWarningMessage("Se asignó la BF: " + ItemID);
                    }
                }

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                Application.SBO_Application.SetStatusErrorMessage(ex.StackTrace);
                Application.SBO_Application.SetStatusErrorMessage(ex.InnerException.Message);
            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }

        }

        private static int indexFMS(Company company, string FormID, string ItemID, string ColumnId)
        {
            int fmsId = -1;


            string sQuery;
            sQuery = $@"select ""IndexID"" from ""CSHS"" where ""FormID"" = '{FormID}' and ""ItemID"" = '{ ItemID }' and ""ColID"" = '{ColumnId}'";

            var oRec = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            oRec.DoQuery(sQuery);

            while (!oRec.EoF)
            {
                return Convert.ToInt32(oRec.Fields.Item(0).Value.ToString());
            }
            return fmsId;
        }

        public static List<FormattedSearchDto> SetListFormattedSearch()
        {

            try
            {
                List<FormattedSearchDto> dtos = new List<FormattedSearchDto>();

                dtos.Add(new FormattedSearchDto
                {
                    ColumnID = "Col_17",
                    FormID = "FormMaestroCodigosBancarios",
                    ItemID = "Item_9",
                    name = "EXC_AddOnGastosBancarios_MaestroCodigosBancarios_Detalle_CuentaBancoGet",
                    sqlQuery = @"select ""Account"" , * from DSC1 where ""BankCode"" = $[""@EXD_OMCB"".""U_COD_BANCO""] and ""Branch"" = $[""@EXD_MCB1"".""U_COD_EMPRESA""]",
                    userQueryId = 0
                });

                dtos.Add(new FormattedSearchDto
                {
                    ColumnID = "Col_15",
                    FormID = "FormMaestroCodigosBancarios",
                    ItemID = "Item_9",
                    name = "EXC_AddOnGastosBancarios_MaestroCodigosBancarios_Detalle_CodCuentaBancoSet",
                    sqlQuery = @"select ""GLAccount"" from DSC1 where ""BankCode"" = $[""@EXD_OMCB"".""U_COD_BANCO""] and ""Branch"" = $[""@EXD_MCB1"".""U_COD_EMPRESA""] and ""Account"" = $[""@EXD_MCB1"".""U_DES_CUENTA""]",
                    userQueryId = 0
                });

                return dtos;

            }
            catch (Exception ex)
            {

            }

            return null;

        }
    }
}
