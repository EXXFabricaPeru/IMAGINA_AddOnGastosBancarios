using EXX_IMG_GastosBancarios.Domain.Entities;
using EXX_IMG_GastosBancarios.Presentation.Helper;
using JF_SBOAddon.Utiles.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SAPbobsCOM;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Presentation.Domain
{
    public class ConciliacionBancariaDomain
    {

        #region Generación de Pagos
        internal static void GeneraciondePagosSL(int docEntry, SAPbobsCOM.Company company)
        {
            List<RespuestaPagoModel> listRespPagos = new List<RespuestaPagoModel>();
            try
            {
                Application.SBO_Application.SetStatusWarningMessage("Iniciando Generación de Pagos :" + docEntry);
                var listPagos = ObtenerPagos(docEntry, company);

                foreach (var ope in listPagos)
                {
                    var pago = GenerarPago(ope, ope.FechaCont, company, docEntry);
                    listRespPagos.Add(pago);
                }

                Application.SBO_Application.SetStatusWarningMessage("Terminó proceso de pagos");
            }
            catch (Exception ex)
            {

                Application.SBO_Application.SetStatusWarningMessage("Error de Generación de Pagos :" + docEntry + " - " + ex.Message);
            }
        }

        private static RespuestaPagoModel GenerarPago(OperacionDto ope, string fechaCont, SAPbobsCOM.Company company, int docEntry)
        {
            var url = Globals.ServiceLayerUrl + (ope.TipoImporte == "D" ? Globals.VendorPayments : Globals.IncomingPayments);

            PaymentSL payment = new PaymentSL();
            RespuestaPagoModel respuesta = new RespuestaPagoModel();

            //var codSucursal = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            //var ctaPrincipal = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");
            //var codMoneda = dbsEXD_OCBN.GetValueExt("U_COD_MONEDA");
            //Application.SBO_Application.SetStatusWarningMessage(edtFechaContabilizacion.Value);
            //var fechaCont = edtFechaContabilizacion.Value;
            //Application.SBO_Application.SetStatusWarningMessage("646");

            payment.DocDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            payment.DueDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            payment.TaxDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            payment.DocCurrency = ope.Idmoneda; //payment.DocCurrency = codMoneda;

            payment.BPLID = Convert.ToInt32(ope.Idsucursal);  //payment.BPLID = Convert.ToInt32(codSucursal);
            payment.DocType = "rAccount";

            //Application.SBO_Application.SetStatusWarningMessage("657");

            payment.TransferAccount = ope.Idcuenta; //payment.TransferAccount = ctaPrincipal;
            payment.TransferDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            payment.TransferReference = ope.NroReferencia;
            payment.TransferSum = ope.Importe;

            payment.U_EXX_MPTRABAN = ope.CodMPTraBan;
            payment.U_EXX_CODGB = ope.Idcuenta + "-" + ope.NroSecuencia;
            payment.JournalRemarks = ope.Glosa;

            payment.PaymentAccounts = new List<PaymentSL.PaymentAccount>();

            var AccountPayments = new PaymentSL.PaymentAccount();

            AccountPayments.AccountCode = ObtenerCodigoDeCuenta(ope.NroCuenta, company);
            AccountPayments.AccountName = ope.NroCuenta;
            AccountPayments.GrossAmount = ope.Importe;
            AccountPayments.SumPaid = ope.Importe;
            //Application.SBO_Application.SetStatusWarningMessage("670");
            AccountPayments.ProjectCode = ope.CodProyecto;
            AccountPayments.ProfitCenter = ope.CodDim1;
            AccountPayments.ProfitCenter2 = ope.CodDim2;
            AccountPayments.ProfitCenter3 = ope.CodDim3;
            AccountPayments.ProfitCenter4 = ope.CodDim4;
            AccountPayments.ProfitCenter5 = ope.CodDim5;

            AccountPayments.U_EXC_PARTPRES = ope.CodParPre;
            AccountPayments.U_EXC_NOMPPR = ope.NomParPre;

            payment.PaymentAccounts.Add(AccountPayments);


            var jsonBody = JsonConvert.SerializeObject(payment, new JsonSerializerSettings
            {
                DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
            });
            var response = ServiceLayerHelper.PostSL(url, jsonBody);

            if (response.StatusCode == System.Net.HttpStatusCode.Created)
            {
                var pago = JsonConvert.DeserializeObject<PaymentSL>(response.Content);
                respuesta.DocEntry = pago.DocEntry;
                respuesta.TipoPago = ope.TipoImporte == "D" ? 46 : 24;
                respuesta.LineNum = ope.Id;
                respuesta.Procesado = true;
                respuesta.Mensaje = "Procesado";
                ActualizarPago(docEntry, ope, respuesta.DocEntry, respuesta.TipoPago, respuesta.Mensaje, company);

            }
            else
            {
                var error = JsonConvert.DeserializeObject<ErrorSLModel>(response.Content);
                respuesta.DocEntry = 0;
                respuesta.TipoPago = ope.TipoImporte == "D" ? 46 : 24;
                respuesta.LineNum = ope.Id;
                respuesta.Procesado = false;
                respuesta.Mensaje = error.error.message.value;
                ActualizarPago(docEntry, ope, respuesta.DocEntry, 0, respuesta.Mensaje, company);
            }
            return respuesta;

        }
        private static string ObtenerCodigoDeCuenta(string formatCode, SAPbobsCOM.Company company)
        {
            var sqlQry = $"select \"AcctCode\" from  OACT where \"FormatCode\" = '{formatCode}'";
            var recSet = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);

            string codigo = recSet.Fields.Item(0).Value.ToString();

            GC.Collect();
            if (recSet != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);

            return codigo;
        }
        private static List<OperacionDto> ObtenerPagos(int docEntry, SAPbobsCOM.Company company)
        {
            var sqlQry = string.Empty;

            //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_DATOS_PAGOS('{docEntry}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_DATOS_PAGOS '{docEntry}'";

            recSet.DoQuery(sqlQry);
            List<OperacionDto> list = new List<OperacionDto>();
            while (!recSet.EoF)
            {
                try
                {
                    OperacionDto ope = new OperacionDto
                    {
                        NroSecuencia = int.Parse(recSet.Fields.Item("U_NUM_SECUENCIA").Value.ToString()),
                        CodDim1 = recSet.Fields.Item("U_COD_DIM1").Value.ToString(),
                        CodDim2 = recSet.Fields.Item("U_COD_DIM2").Value.ToString(),
                        CodDim3 = recSet.Fields.Item("U_COD_DIM3").Value.ToString(),
                        CodDim4 = recSet.Fields.Item("U_COD_DIM4").Value.ToString(),
                        CodDim5 = recSet.Fields.Item("U_COD_DIM5").Value.ToString(),
                        CodMPTraBan = recSet.Fields.Item("U_COD_MPTRABAN").Value.ToString(),
                        Seleccionado = recSet.Fields.Item("U_SELECCIONADO").Value.ToString(),
                        Id = Convert.ToInt32(recSet.Fields.Item("LineId").Value.ToString()),
                        CodOperacion = recSet.Fields.Item("U_COD_OPERACION").Value.ToString(),
                        Glosa = recSet.Fields.Item("U_GLOSA").Value.ToString(),
                        IdBanco = recSet.Fields.Item("U_COD_BANCO").Value.ToString(),
                        CodParPre = recSet.Fields.Item("U_COD_PARPRE").Value.ToString(),
                        CodProyecto = recSet.Fields.Item("U_COD_PROYECTO").Value.ToString(),
                        Estado = recSet.Fields.Item("U_COD_ESTADO").Value.ToString(),
                        Idcuenta = recSet.Fields.Item("U_COD_CUENTA").Value.ToString(),
                        Idmoneda = recSet.Fields.Item("U_COD_MONEDA").Value.ToString(),
                        Idsucursal = recSet.Fields.Item("U_COD_EMPRESA").Value.ToString(),
                        Importe = Convert.ToDouble(recSet.Fields.Item("U_IMPORTE").Value.ToString()),
                        NomParPre = recSet.Fields.Item("U_NOM_PARPRE").Value.ToString(),
                        NroCuenta = recSet.Fields.Item("U_NUM_CUENTA").Value.ToString(),
                        NroReferencia = recSet.Fields.Item("U_NUM_OPERACION").Value.ToString(),
                        TipoImporte = recSet.Fields.Item("U_TIPO_IMPORTE").Value.ToString(),
                        FechaCont = recSet.Fields.Item("U_FECHA_CONT").Value.ToString()
                    };

                    list.Add(ope);
                    recSet.MoveNext();
                }
                catch (Exception)
                {

                }

            }

            GC.Collect();
            if (recSet != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);

            return list;
        }

        private static void ActualizarPago(int docEntry, OperacionDto ope, int CodPago, int tipoPago, string estado, SAPbobsCOM.Company company)
        {
            try
            {
                var sqlQry = string.Empty;
                var recSet = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                if (CodPago > 0)
                {
                    if (company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS({docEntry},{ope.Id},{CodPago},{tipoPago}, '{estado}')";
                    else
                        sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS '{docEntry},{ope.Id},{CodPago},{tipoPago}, '{estado}''";
                }
                else
                {
                    if (company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS({docEntry},{ope.Id},null,null, '{estado}')";
                    else
                        sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS '{docEntry},{ope.Id},null,null, '{estado}''";

                }

                recSet.DoQuery(sqlQry);


                GC.Collect();
                if (recSet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);

            }
            catch (Exception ex)
            {

            }
        }


        #endregion

        #region Generación de Reconciliación
        internal static void GenerarReconciliacionSL(int docEntry, Company company)
        {
            try
            {
                Application.SBO_Application.SetStatusWarningMessage("Iniciando Reconciliación :" + docEntry);
                var listPagos = ObtenerPendientesAReconciliar(docEntry, company);

                procesarReconciliaciones(listPagos, company, docEntry);


                Application.SBO_Application.SetStatusWarningMessage("Reconciliación procesada");
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusWarningMessage("Error de Reconciliación:" + docEntry + " - " + ex.Message);
            }
        }

        private static void procesarReconciliaciones(List<ReconDto> reconDtos, Company company, int docEntry)
        {
            ExternalReconciliationSL reconciliationSL = new ExternalReconciliationSL();
            reconciliationSL.ExternalReconciliation = new ExternalReconciliationSL.ExternalReconciliationModel();
            reconciliationSL.ExternalReconciliation.ReconciliationAccountType = "rat_GLAccount";
            //reconciliationSL.ExternalReconciliation.ReconciliationDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            var cuentasGroup = reconDtos.GroupBy(t => t.Cuenta);
            var url = Globals.ServiceLayerUrl + Globals.ExternalReconciliationService;

            foreach (var grupo in cuentasGroup)
            {
                reconciliationSL.ExternalReconciliation.ReconciliationJournalEntryLines = new List<ExternalReconciliationSL.ReconciliationJournalEntryLine>();
                reconciliationSL.ExternalReconciliation.ReconciliationBankStatementLines = new List<ExternalReconciliationSL.ReconciliationBankStatementLine>();

                foreach (var item in grupo)
                {
                    try
                    {
                        var journalLine = new ExternalReconciliationSL.ReconciliationJournalEntryLine();
                        journalLine.TransactionNumber = item.TransID;//Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                        journalLine.LineNumber = item.LineIDPago;//

                        reconciliationSL.ExternalReconciliation.ReconciliationJournalEntryLines.Add(journalLine);

                        var entryLine = new ExternalReconciliationSL.ReconciliationBankStatementLine();
                        entryLine.BankStatementAccountCode = item.Cuenta;//Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                        entryLine.Sequence = item.Secuencia;// Convert.ToInt32(recSet.Fields.Item(1).Value.ToString());

                        reconciliationSL.ExternalReconciliation.ReconciliationBankStatementLines.Add(entryLine);

                    }
                    catch (Exception)
                    {

                    }
                }

                var jsonBody = JsonConvert.SerializeObject(reconciliationSL, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                });
                var response = ServiceLayerHelper.PostSL(url, jsonBody);

                if (response.StatusCode == System.Net.HttpStatusCode.OK || response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    foreach (var item in grupo)
                    {
                        ActualizarRecon(docEntry, item.LineID, "Reconciliado", company);
                    }

                }

            }

        }

        private static List<ReconDto> ObtenerPendientesAReconciliar(int docEntry, Company company)
        {
            var sqlQry = string.Empty;

            //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION('{docEntry}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION '{docEntry}'";

            recSet.DoQuery(sqlQry);

            //Application.SBO_Application.SetStatusWarningMessage("Generar Reconciliacion " + mtxOperaciones.RowCount);

            List<ReconDto> reconDtos = new List<ReconDto>();
            while (!recSet.EoF)
            {
                reconDtos.Add(new ReconDto
                {
                    TransID = Convert.ToInt32(recSet.Fields.Item(0).Value.ToString()),
                    LineIDPago = Convert.ToInt32(recSet.Fields.Item(1).Value.ToString()),
                    Cuenta = recSet.Fields.Item(2).Value.ToString(),
                    Secuencia = Convert.ToInt32(recSet.Fields.Item(3).Value.ToString()),
                    LineID = Convert.ToInt32(recSet.Fields.Item(4).Value),
                    Banco = recSet.Fields.Item(5).Value.ToString(),
                    Estado = recSet.Fields.Item(6).Value.ToString(),
                    Sucursal = recSet.Fields.Item(7).Value.ToString()
                    //var cuenta = recSet.Fields.Item(8).Value.ToString();

                });



                recSet.MoveNext();
            }

            GC.Collect();
            if (recSet != null)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);

            return reconDtos;
        }

        private static void ActualizarRecon(int docEntry, int lineID, string estado, Company company)
        {
            try
            {
                var sqlQry = string.Empty;
                var recSet = (SAPbobsCOM.Recordset)company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                if (company.DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                    sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_UPDATE_RECON({docEntry},{lineID}, '{estado}')";
                else
                    sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_UPDATE_RECON '{docEntry},{lineID},'{estado}''";


                recSet.DoQuery(sqlQry);


                GC.Collect();
                if (recSet != null)
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(recSet);

            }
            catch (Exception ex)
            {

            }
        }


        #endregion
    }
}
