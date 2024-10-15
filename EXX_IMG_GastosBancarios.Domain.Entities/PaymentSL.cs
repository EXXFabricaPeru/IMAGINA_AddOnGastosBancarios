using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXX_IMG_GastosBancarios.Domain.Entities
{
    public class PaymentSL
    {
        // Root myDeserializedClass = JsonConvert.Deserializedynamic<Root>(myJsonResponse);


        public class PaymentAccount
        {
            public int LineNum { get; set; }
            public string AccountCode { get; set; }
            public double SumPaid { get; set; }
            public double SumPaidFC { get; set; }
            public string Decription { get; set; }
            public dynamic VatGroup { get; set; }
            public string AccountName { get; set; }
            public double GrossAmount { get; set; }
            public string ProfitCenter { get; set; }
            public string ProjectCode { get; set; }
            public double VatAmount { get; set; }
            public string ProfitCenter2 { get; set; }
            public string ProfitCenter3 { get; set; }
            public string ProfitCenter4 { get; set; }
            public string ProfitCenter5 { get; set; }
            public dynamic LocationCode { get; set; }
            public double EqualizationVatAmount { get; set; }
            public string U_EXC_PARTPRES { get; set; }
            public string U_EXC_NOMPPR { get; set; }
        }

        //public class PaymentInvoice
        //{
        //    public int LineNum { get; set; }
        //    public int DocEntry { get; set; }
        //    public int DocNum { get; set; }
        //    public double SumApplied { get; set; }
        //    public double AppliedFC { get; set; }
        //    public double AppliedSys { get; set; }
        //    public double DocRate { get; set; }
        //    public int DocLine { get; set; }
        //    public string InvoiceType { get; set; }
        //    public double DiscountPercent { get; set; }
        //    public double PaidSum { get; set; }
        //    public int InstallmentId { get; set; }
        //    public double WitholdingTaxApplied { get; set; }
        //    public double WitholdingTaxAppliedFC { get; set; }
        //    public double WitholdingTaxAppliedSC { get; set; }
        //    public dynamic LinkDate { get; set; }
        //    public dynamic DistributionRule { get; set; }
        //    public dynamic DistributionRule2 { get; set; }
        //    public dynamic DistributionRule3 { get; set; }
        //    public dynamic DistributionRule4 { get; set; }
        //    public dynamic DistributionRule5 { get; set; }
        //    public double TotalDiscount { get; set; }
        //    public double TotalDiscountFC { get; set; }
        //    public double TotalDiscountSC { get; set; }
        //    public dynamic U_EXX_SUJPER { get; set; }
        //    public dynamic U_EXX_INCPER { get; set; }
        //    public double U_EXX_PORPER { get; set; }
        //    public double U_EXX_MONPER { get; set; }
        //    public dynamic U_EXC_PARTPRES { get; set; }
        //    public dynamic U_EXC_NOMPPR { get; set; }
        //}




        public int DocNum { get; set; }
        public string DocType { get; set; }
        public string HandWritten { get; set; }
        public string Printed { get; set; }
        public DateTime DocDate { get; set; }
        public string CardCode { get; set; }
        public string CardName { get; set; }
        public string Address { get; set; }
        public string CashAccount { get; set; }
        public string DocCurrency { get; set; }
        public double CashSum { get; set; }
        public dynamic CheckAccount { get; set; }
        public string TransferAccount { get; set; }
        public double TransferSum { get; set; }
        public DateTime? TransferDate { get; set; }
        public string TransferReference { get; set; }
        public string LocalCurrency { get; set; }
        public double DocRate { get; set; }
        public string Reference1 { get; set; }
        public dynamic Reference2 { get; set; }
        public dynamic CounterReference { get; set; }
        public string Remarks { get; set; }
        public string JournalRemarks { get; set; }
        public string SplitTransaction { get; set; }
        public dynamic ContactPersonCode { get; set; }
        public string ApplyVAT { get; set; }
        public DateTime TaxDate { get; set; }
        public int Series { get; set; }
        public string BankCode { get; set; }
        public string BankAccount { get; set; }
        public double DiscountPercent { get; set; }
        public string ProjectCode { get; set; }
        public string CurrencyIsLocal { get; set; }
        public double DeductionPercent { get; set; }
        public double DeductionSum { get; set; }
        public double CashSumFC { get; set; }
        public double CashSumSys { get; set; }
        public dynamic BoeAccount { get; set; }
        public double BillOfExchangeAmount { get; set; }
        public dynamic BillofExchangeStatus { get; set; }
        public double BillOfExchangeAmountFC { get; set; }
        public double BillOfExchangeAmountSC { get; set; }
        public dynamic BillOfExchangeAgent { get; set; }
        public dynamic WTCode { get; set; }
        public double WTAmount { get; set; }
        public double WTAmountFC { get; set; }
        public double WTAmountSC { get; set; }
        public dynamic WTAccount { get; set; }
        public double WTTaxableAmount { get; set; }
        public string Proforma { get; set; }
        public dynamic PayToBankCode { get; set; }
        public dynamic PayToBankBranch { get; set; }
        public dynamic PayToBankAccountNo { get; set; }
        public dynamic PayToCode { get; set; }
        public dynamic PayToBankCountry { get; set; }
        public string IsPayToBank { get; set; }
        public int DocEntry { get; set; }
        public string PaymentPriority { get; set; }
        public dynamic TaxGroup { get; set; }
        public double BankChargeAmount { get; set; }
        public double BankChargeAmountInFC { get; set; }
        public double BankChargeAmountInSC { get; set; }
        public double UnderOverpaymentdifference { get; set; }
        public double UnderOverpaymentdiffSC { get; set; }
        public double WtBaseSum { get; set; }
        public double WtBaseSumFC { get; set; }
        public double WtBaseSumSC { get; set; }
        public dynamic VatDate { get; set; }
        public dynamic TransactionCode { get; set; }
        public string PaymentType { get; set; }
        public double TransferRealAmount { get; set; }
        public string DocdynamicCode { get; set; }
        public string DocTypte { get; set; }
        public DateTime DueDate { get; set; }
        public dynamic LocationCode { get; set; }
        public string Cancelled { get; set; }
        public string ControlAccount { get; set; }
        public double UnderOverpaymentdiffFC { get; set; }
        public string AuthorizationStatus { get; set; }
        public int BPLID { get; set; }
        public string BPLName { get; set; }
        public dynamic VATRegNum { get; set; }
        public dynamic BlanketAgreement { get; set; }
        public string PaymentByWTCertif { get; set; }
        public dynamic Cig { get; set; }
        public dynamic Cup { get; set; }
        public dynamic AttachmentEntry { get; set; }
        public dynamic SignatureInputMessage { get; set; }
        public dynamic SignatureDigest { get; set; }
        public dynamic CertificationNumber { get; set; }
        public dynamic PrivateKeyVersion { get; set; }
        public dynamic EDocExportFormat { get; set; }
        public dynamic ElecCommStatus { get; set; }
        public dynamic ElecCommMessage { get; set; }
        public string SplitVendorCreditRow { get; set; }
        public dynamic U_EXX_DESCREND { get; set; }
        public dynamic U_EXX_EMPLEADO { get; set; }
        public dynamic U_EXX_SERIEPER { get; set; }
        public dynamic U_EXX_CORREPER { get; set; }
        public dynamic U_EXX_NRODEPDE { get; set; }
        public dynamic U_EXX_FEDEPDET { get; set; }
        public dynamic U_EXX_DETLOT { get; set; }
        public string U_EXX_BORRPER { get; set; }
        public dynamic U_EXX_MPCHEQUE { get; set; }
        public string U_EXX_MPTRABAN { get; set; }
        public dynamic U_EXX_MPTARCRE { get; set; }
        public string U_EXX_MPFONDEF { get; set; }
        public dynamic U_EXX_MPLETRAS { get; set; }
        public dynamic U_EXX_SERIECER { get; set; }
        public dynamic U_EXX_CORRECER { get; set; }
        public dynamic U_EXX_NUMEREND { get; set; }
        public dynamic U_4MEDIO_PAGO { get; set; }
        public dynamic U_4DOCUMENTO_ORIGEN { get; set; }
        public dynamic U_4USUARIO_ORIGEN { get; set; }
        public dynamic U_4FACTURA_ORIGEN { get; set; }
        public dynamic U_EXC_NUMPRO { get; set; }
        public string U_EXC_TIPCOB { get; set; }
        public string U_EXX_DETRACCION { get; set; }
        public dynamic U_EXX_OBJORIDET { get; set; }
        public dynamic U_EXX_ORIDETR { get; set; }
        public string U_EXX_AUTDET { get; set; }
        public dynamic U_EXC_NUMPRP { get; set; }
        public dynamic U_EXC_OcrCode { get; set; }
        public dynamic U_EXC_OcrCode2 { get; set; }
        public dynamic U_EXC_OcrCode3 { get; set; }
        public dynamic U_EXC_OcrCode4 { get; set; }
        public dynamic U_EXC_CODPARPR { get; set; }
        public dynamic U_EXC_DESPARPR { get; set; }
        public dynamic U_CP_VARESC { get; set; }
        public dynamic U_EXX_PRIPAG { get; set; }
        public dynamic U_SMC_SERIE_FE { get; set; }
        public dynamic U_SMC_NUM_FE { get; set; }
        public dynamic U_SMC_ESTADO_FE { get; set; }
        public dynamic U_SMC_MSJ_ESTADO_FE { get; set; }
        public dynamic U_EXC_IDCUOT { get; set; }
        public string U_SMCEECC_ENV { get; set; }
        public dynamic U_SMCEECC_FEC { get; set; }
        public dynamic U_SMC_RETENCION_BAJA { get; set; }
        public dynamic U_EXX_CODPAGDET { get; set; }
        public string U_EXC_MIGR { get; set; }
        //public dynamic U_EXX_CODGB { get; set; }
        public dynamic U_EXX_CODGAB { get; set; }
        public List<dynamic> PaymentChecks { get; set; }
        //public List<PaymentInvoice> PaymentInvoices { get; set; }
        //public List<dynamic> PaymentCreditCards { get; set; }
        public List<PaymentAccount> PaymentAccounts { get; set; }
        //public List<dynamic> PaymentDocumentReferencesCollection { get; set; }
        //public List<dynamic> WithholdingTaxCertificatesCollection { get; set; }
        //public List<dynamic> ElectronicProtocols { get; set; }
        //public List<dynamic> CashFlowAssignments { get; set; }
        //public List<dynamic> Payments_ApprovalRequests { get; set; }
        //public List<dynamic> WithholdingTaxDataWTXCollection { get; set; }



    }
}
