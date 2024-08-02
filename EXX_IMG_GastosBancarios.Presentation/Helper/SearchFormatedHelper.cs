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
using System.Xml;
using System.Runtime.InteropServices;

namespace EXX_IMG_GastosBancarios.Presentation.Helper
{
    public class SearchFormatedHelper
    {

        public static string CategoriesName = "9980. Búsquedas Formateadas Exxis - Gtos Bancarios";
        private static string TableID = "EXX_SETUP";
        public static bool versionID = false;
        public static void Initialize(Company oCompany, Version version, string AddonID, bool validVersion)
        {

            Application.SBO_Application.SetStatusWarningMessage("Iniciando Creación de BF");

            //if (ValidVersion(oCompany,version, AddonID))
            if (validVersion)
            {
                var IdCategory = CreateCategoryFormattedSearch(CategoriesName, oCompany);
                //Application.SBO_Application.SetStatusWarningMessage("IdCategory: " + IdCategory);
                CreateFormattedSearchByList(IdCategory, oCompany);

                updateSYSTable(oCompany, AddonID, version, TableID);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();

            }


        }
        private static void updateSYSTable(Company oCompany, string addonID, Version version, string _tableSys)
        {
            UserTable userTable = oCompany.UserTables.Item((object)_tableSys);
            Recordset businessObject = oCompany.GetBusinessObject(BoObjectTypes.BoRecordset) as Recordset;
            string QueryStr1 = "select * from \"@EXX_SETUP\"";
            businessObject.DoQuery(QueryStr1);
            int recordCount = businessObject.RecordCount;
            string QueryStr2 = QueryStr1 + " where \"U_EXX_ADDN\" = '" + addonID + "'";
            businessObject.DoQuery(QueryStr2);
            if (!businessObject.EoF)
            {
                if (!userTable.GetByKey(businessObject.Fields.Item((object)"Code").Value.ToString()))
                    return;
                userTable.UserFields.Fields.Item((object)"U_EXX_ADDN").Value = (object)addonID;
                userTable.UserFields.Fields.Item((object)"U_EXX_RUTA").Value = (object)version.ToString();
                if (userTable.Update() != 0)
                    throw new InvalidOperationException(string.Format("SYS - TBL:{0} - {1}", (object)oCompany.GetLastErrorCode(), (object)oCompany.GetLastErrorDescription()));
            }
            else
            {
                userTable.Code = (recordCount + 1).ToString().PadLeft(2, '0');
                userTable.Name = (recordCount + 1).ToString().PadLeft(2, '0');
                userTable.UserFields.Fields.Item((object)"U_EXX_ADDN").Value = (object)addonID;
                userTable.UserFields.Fields.Item((object)"U_EXX_RUTA").Value = (object)version.ToString();
                if (userTable.Add() != 0)
                    throw new InvalidOperationException(string.Format("SYS - TBL:{0} - {1}", (object)oCompany.GetLastErrorCode(), (object)oCompany.GetLastErrorDescription()));
            }
        }
        public static void ValidVersionV2(Company oCompany, Version version, string addonID)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("ValidVersionV2");
                string sQuery;
                sQuery = $@"select * from ""@EXX_SETUP"" where ""U_EXX_ADDN"" = '{ addonID }'";
                string str = "";
                var oRec = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(sQuery);

                while (!oRec.EoF)
                {
                    str = oRec.Fields.Item("U_EXX_RUTA").Value.ToString();
                }
                Marshal.ReleaseComObject((object)oRec);
                GC.Collect();
                if (version.ToString() != str)
                {
                    versionID = true;
                }

                versionID = false;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusWarningMessage("ValidVersion Error: " + ex.Message);
                versionID = false;
            }
        }
        public static bool ValidVersion(Company oCompany, Version version, string addonID)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("ValidVersion333");
                string sQuery;
                sQuery = $@"select * from ""@EXX_SETUP"" where ""U_EXX_ADDN"" = '{ addonID }'";
                string str = "";
                var oRec = (SAPbobsCOM.Recordset)oCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                oRec.DoQuery(sQuery);

                while (!oRec.EoF)
                {
                    str = oRec.Fields.Item("U_EXX_VERS").Value.ToString();
                    oRec.MoveNext();
                }

                Marshal.ReleaseComObject((object)oRec);
                GC.Collect();

                if (version.ToString() != str)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusWarningMessage("ValidVersion Error: " + ex.Message);
                return false;
            }
        }

        private static void CreateFormattedSearchByList(int idCategory, Company oCompany)
        {
            var list = GetListFormattedSearch();//SetListFormattedSearch();

            foreach (var item in list)
            {
                DesassingFomattedSearch(oCompany, item.FormID, item.ItemID, item.ColumnID);
                DropIfExistFormattedSearch(oCompany, item.sqlQuery, item.Name, idCategory);

                var idFs = CreateFormattedSearch(oCompany, item.sqlQuery, item.Name, idCategory);

                AssignCreateFormattedSearch(oCompany, item, idFs);

            }
        }

        private static void DesassingFomattedSearch(Company company, string FormID, string ItemID, string ColumnID)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("DesassingFomattedSearch");
                FormattedSearches formattedSearch = (FormattedSearches)company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
                var idAPPFS = indexFMS(company, FormID, ItemID, ColumnID);
                if (idAPPFS != -1)
                {
                    formattedSearch.GetByKey(idAPPFS);
                    int ret = formattedSearch.Remove();
                    if (ret == 0)
                    {
                        //Application.SBO_Application.SetStatusWarningMessage("DesassingFomattedSearch  BF OK ");
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

                    int ret = userQuery.Remove();

                    if (ret == 0)
                    {
                        //Application.SBO_Application.SetStatusWarningMessage("DropIfExistFormattedSearch  BF: " + name);
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
                //Application.SBO_Application.SetStatusWarningMessage("Se creó la categoría de BF: " + strCode);
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
                    //Application.SBO_Application.SetStatusWarningMessage("Se creó la BF: " + name);
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

        public static void AssignCreateFormattedSearch(Company company, FormattedSearchDto formaSearch, int userQueryId)
        {
            try
            {
                // Crear la búsqueda formateada
                //Application.SBO_Application.SetStatusWarningMessage("AssignCreateFormattedSearch");
                FormattedSearches formattedSearch = (FormattedSearches)company.GetBusinessObject(BoObjectTypes.oFormattedSearches);
                var idAPPFS = indexFMS(company, formaSearch.FormID, formaSearch.ItemID, formaSearch.ColumnID);
                if (idAPPFS != -1)
                {
                    formattedSearch.GetByKey(idAPPFS);
                }
                else
                {

                    formattedSearch.FormID = formaSearch.FormID;// "UDO_FT_EXX_ANXN_CONF";// Entities.EXX_ANXN_COND.ID; // Form ID del UDO
                    formattedSearch.ItemID = formaSearch.ItemID;// "0_U_G"; // ID del campo en el formulario
                    formattedSearch.ColumnID = formaSearch.ColumnID;// "C_0_8"; // Nombre del campo en la tabla

                }

                formattedSearch.Action = BoFormattedSearchActionEnum.bofsaQuery;
                formattedSearch.QueryID = userQueryId;
                formattedSearch.ByField = BoYesNoEnum.tNO; // Nombre del campo en la tabla

                formattedSearch.Refresh = formaSearch.Refresh == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;
                formattedSearch.ForceRefresh = formaSearch.ForceRefresh == "Y" ? BoYesNoEnum.tYES : BoYesNoEnum.tNO;

                int cont = 1;

                if (formaSearch.FieldIDs.Count == 1)
                {

                    formattedSearch.FieldID = formaSearch.FieldIDs.FirstOrDefault();
                }
                if (formaSearch.FieldIDs.Count > 1)
                {
                    foreach (var item in formaSearch.FieldIDs)
                    {

                        formattedSearch.FieldIDs.FieldID = item;
                        formattedSearch.FieldIDs.Add();

                    }

                }

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
                        //Application.SBO_Application.SetStatusWarningMessage("Se asignó la BF: " + formaSearch.ColumnID);
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
                        //Application.SBO_Application.SetStatusWarningMessage("Se asignó la BF: " + formaSearch.ItemID);
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
                    Name = "EXC_AddOnGastosBancarios_MaestroCodigosBancarios_Detalle_CuentaBancoGet",
                    sqlQuery = @"select ""Account"" , * from DSC1 where ""BankCode"" = $[""@EXD_OMCB"".""U_COD_BANCO""] and ""Branch"" = $[""@EXD_MCB1"".""U_COD_EMPRESA""]",
                    userQueryId = 0
                });

                dtos.Add(new FormattedSearchDto
                {
                    ColumnID = "Col_15",
                    FormID = "FormMaestroCodigosBancarios",
                    ItemID = "Item_9",
                    Name = "EXC_AddOnGastosBancarios_MaestroCodigosBancarios_Detalle_CodCuentaBancoSet",
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

        public static List<FormattedSearchDto> GetListFormattedSearch()
        {

            // Ruta al archivo XML
            string fileName = "Resources/BF/FS.xml";
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

            string xmlFile = Path.Combine(baseDirectory, fileName);
            List<FormattedSearchDto> fsList = new List<FormattedSearchDto>();

            // Verifica si el archivo existe
            if (File.Exists(xmlFile))
            {
                try
                {
                    // Crear una instancia de XmlDocument
                    XmlDocument xmlDoc = new XmlDocument();

                    // Cargar el archivo XML
                    xmlDoc.Load(xmlFile);

                    // Obtener todos los nodos <FS>
                    XmlNodeList fsNodes = xmlDoc.SelectNodes("//FS");

                    // Recorrer y procesar los nodos <FS>
                    foreach (XmlNode node in fsNodes)
                    {
                        FormattedSearchDto fs = new FormattedSearchDto
                        {
                            sqlQuery = node["sqlQuery"].InnerText,
                            Name = node["Name"].InnerText,
                            FormID = node["FormID"].InnerText,
                            ItemID = node["ItemID"].InnerText,
                            ColumnID = node["ColumnID"].InnerText,
                            Refresh = node["Refresh"].InnerText,
                            ForceRefresh = node["ForceRefresh"].InnerText,
                            userQueryId = 0,
                            FieldIDs = new List<string>()
                        };
                        XmlNodeList fieldIdNodes = node.SelectNodes("FieldIDs/FieldID");
                        foreach (XmlNode fieldIdNode in fieldIdNodes)
                        {
                            fs.FieldIDs.Add(fieldIdNode.InnerText);
                        }

                        //Application.SBO_Application.SetStatusWarningMessage("Cargando..." ) ;
                        fsList.Add(fs);
                    }

                    return fsList;
                }
                catch (Exception ex)
                {
                    return null;
                    Console.WriteLine($"Ocurrió un error al leer el archivo XML: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("El archivo XML no existe.");
                return null;
            }
        }

    }
}
