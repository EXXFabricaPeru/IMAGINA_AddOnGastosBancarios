using EXX_IMG_GastosBancarios.Domain.Entities;
using JF_SBOAddon.Utiles.Extensions;
using JF_SBOAddon.Utiles.Utilities;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXX_IMG_GastosBancarios.Presentation.Forms
{
    [FormAttribute("FormConciliacionBancaria", "Forms/FormConciliacionBancaria.b1f")]
    class FormConciliacionBancaria : UserFormBase
    {
        private SAPbouiCOM.ComboBox cmbEmpresas;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox cmbBancos;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox cmbCuentas;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText EditText2;
        private SAPbouiCOM.StaticText StaticText4;
        private SAPbouiCOM.StaticText StaticText5;
        private SAPbouiCOM.EditText edtFechaDesde;
        private SAPbouiCOM.EditText edtFechaHasta;
        private SAPbouiCOM.Button btnBuscar;
        private SAPbouiCOM.Matrix mtxOperaciones;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.Button Button2;
        private SAPbouiCOM.ButtonCombo btcOpciones;
        private SAPbouiCOM.StaticText StaticText6;
        private SAPbouiCOM.EditText EditText0;
        private SAPbouiCOM.ComboBox cmbSeries;
        private SAPbouiCOM.StaticText StaticText7;
        private SAPbouiCOM.ComboBox ComboBox5;
        private SAPbouiCOM.EditText EditText1;

        private SAPbouiCOM.DBDataSource dbsEXD_OCBN = null;
        private SAPbouiCOM.DBDataSource dbsEXD_CBN1 = null;

        private List<OperacionDto> lstOperaciones = null;

        public FormConciliacionBancaria()
        {
            lstOperaciones = new List<OperacionDto>();
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.cmbEmpresas = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_2").Specific));
            this.cmbEmpresas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbEmpresas_ComboSelectAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_3").Specific));
            this.cmbBancos = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_4").Specific));
            this.cmbBancos.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbBancos_ComboSelectAfter);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.cmbCuentas = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_7").Specific));
            this.cmbCuentas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbCuentas_ComboSelectAfter);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_8").Specific));
            this.EditText2 = ((SAPbouiCOM.EditText)(this.GetItem("Item_9").Specific));
            this.StaticText4 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_10").Specific));
            this.StaticText5 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_11").Specific));
            this.edtFechaDesde = ((SAPbouiCOM.EditText)(this.GetItem("Item_12").Specific));
            this.edtFechaHasta = ((SAPbouiCOM.EditText)(this.GetItem("Item_13").Specific));
            this.btnBuscar = ((SAPbouiCOM.Button)(this.GetItem("Item_14").Specific));
            this.btnBuscar.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.btnBuscar_PressedAfter);
            this.mtxOperaciones = ((SAPbouiCOM.Matrix)(this.GetItem("Item_15").Specific));
            this.mtxOperaciones.LinkPressedBefore += new SAPbouiCOM._IMatrixEvents_LinkPressedBeforeEventHandler(this.mtxOperaciones_LinkPressedBefore);
            this.mtxOperaciones.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxOperaciones_ClickBefore);
            this.mtxOperaciones.PressedAfter += new SAPbouiCOM._IMatrixEvents_PressedAfterEventHandler(this.mtxOperaciones_PressedAfter);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button1.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button1_PressedBefore);
            this.Button2 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.btcOpciones = ((SAPbouiCOM.ButtonCombo)(this.GetItem("Item_1").Specific));
            this.btcOpciones.PressedAfter += new SAPbouiCOM._IButtonComboEvents_PressedAfterEventHandler(this.btcOpciones_PressedAfter);
            this.btcOpciones.ComboSelectAfter += new SAPbouiCOM._IButtonComboEvents_ComboSelectAfterEventHandler(this.btcOpciones_ComboSelectAfter);
            this.StaticText6 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_5").Specific));
            this.EditText0 = ((SAPbouiCOM.EditText)(this.GetItem("Item_17").Specific));
            this.cmbSeries = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_18").Specific));
            this.StaticText7 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_19").Specific));
            this.ComboBox5 = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_20").Specific));
            this.dbsEXD_OCBN = this.UIAPIRawForm.GetDBDataSource("@EXD_OCBN");
            this.dbsEXD_CBN1 = this.UIAPIRawForm.GetDBDataSource("@EXD_CBN1");
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_16").Specific));
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            var sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbEmpresas.LoadValidValues(recSet);

            btcOpciones.ValidValues.Add("E", "Ejecutar");
            btcOpciones.ValidValues.Add("R", "Reconciliar");

            mtxOperaciones.Columns.Item("Col_7").Visible = false;
            mtxOperaciones.Columns.Item("Col_9").Visible = false;
            mtxOperaciones.Columns.Item("Col_10").Visible = false;

            this.FormDataLoadAdd();
        }

        private void cmbEmpresas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 inner join DSC1 T1 on T0.\"BankCode\" = T1.\"BankCode\" where T1.\"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbBancos.LoadValidValues(recSet);
        }

        private void cmbBancos_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbCuentas.LoadValidValues(recSet);
        }

        private void cmbCuentas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            var codCta = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");
            var sqlQry = $"select TX1.\"ActCurr\" from DSC1 TX0 inner join OACT  TX1 on TX0.\"GLAccount\" = TX1.\"AcctCode\"  where TX0.\"BankCode\" = '{codBnc}' and TX0.\"Branch\" = '{codEmp}' and TX0.\"GLAccount\" = '{codCta}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            if (!recSet.EoF) dbsEXD_OCBN.SetValueExt("U_COD_MONEDA", recSet.Fields.Item(0).Value.ToString());
        }

        private void btnBuscar_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var nroLinea = 0;
            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var codCta = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");
            var fchDsd = dbsEXD_OCBN.GetValueExt("U_FECHA_DESDE");
            var fchHst = dbsEXD_OCBN.GetValueExt("U_FECHA_HASTA");
            var sqlQry = string.Empty;

            codEmp = string.IsNullOrWhiteSpace(codEmp) ? "-1" : codEmp;
            if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}','{codCta}','{fchDsd}','{fchHst}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES '{codEmp}','{codCta}','{fchDsd}','{fchHst}'";

            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            dbsEXD_CBN1.Clear();
            while (!recSet.EoF)
            {
                var operacionDto = new OperacionDto();
                dbsEXD_CBN1.InsertRecord(nroLinea);
                dbsEXD_CBN1.Offset = nroLinea;

                dbsEXD_CBN1.SetValue("LineId", nroLinea, recSet.Fields.Item(0).Value.ToString());
                operacionDto.Id = Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());

                dbsEXD_CBN1.SetValue("U_SELECCIONADO", nroLinea, "N");
                operacionDto.Seleccionado = "N";

                dbsEXD_CBN1.SetValue("U_COD_OPERACION", nroLinea, recSet.Fields.Item(1).Value.ToString());
                operacionDto.CodOperacion = recSet.Fields.Item(1).Value.ToString();

                dbsEXD_CBN1.SetValue("U_GLOSA", nroLinea, recSet.Fields.Item(2).Value.ToString());
                operacionDto.Glosa = recSet.Fields.Item(2).Value.ToString();

                dbsEXD_CBN1.SetValue("U_IMPORTE", nroLinea, recSet.Fields.Item(3).Value.ToString());
                operacionDto.Importe = Convert.ToDouble(recSet.Fields.Item(3).Value.ToString());

                dbsEXD_CBN1.SetValue("U_NUM_CUENTA", nroLinea, recSet.Fields.Item(4).Value.ToString());
                operacionDto.NroCuenta = recSet.Fields.Item(4).Value.ToString();

                dbsEXD_CBN1.SetValue("U_FECHA_OPERACION", nroLinea, recSet.Fields.Item(5).Value.ToString());

                dbsEXD_CBN1.SetValue("U_NUM_OPERACION", nroLinea, recSet.Fields.Item(6).Value.ToString());
                operacionDto.NroReferencia = recSet.Fields.Item(6).Value.ToString();

                dbsEXD_CBN1.SetValue("U_TIPO_IMPORTE", nroLinea, recSet.Fields.Item(7).Value.ToString());
                operacionDto.TipoImporte = recSet.Fields.Item(7).Value.ToString();

                dbsEXD_CBN1.SetValue("U_NUM_SECUENCIA", nroLinea, recSet.Fields.Item(8).Value.ToString());
                operacionDto.NroSecuencia = Convert.ToInt32(recSet.Fields.Item(8).Value.ToString());

                dbsEXD_CBN1.SetValue("U_INFO_DETALLADA", nroLinea, recSet.Fields.Item(9).Value.ToString());

                lstOperaciones.Add(operacionDto);
                nroLinea++;
                recSet.MoveNext();
            }

            mtxOperaciones.LoadFromDataSource();
        }

        internal void FormDataLoadAdd()
        {
            dbsEXD_OCBN.SetValueExt("U_FECHA_DESDE", DateTime.Today.ToString("yyyyMMdd"));
            dbsEXD_OCBN.SetValueExt("U_FECHA_HASTA", DateTime.Today.ToString("yyyyMMdd"));

            dbsEXD_OCBN.SetValueExt("U_ESTADO", "A");

            cmbSeries.ValidValues.LoadSeries(this.UIAPIRawForm.BusinessObject.Type, SAPbouiCOM.BoSeriesMode.sf_Add);
            if (cmbSeries.ValidValues.Count > 0) cmbSeries.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            dbsEXD_OCBN.SetValueExt("DocNum", this.UIAPIRawForm.BusinessObject.GetNextSerialNumber(dbsEXD_OCBN.GetValueExt("Series")
                , this.UIAPIRawForm.BusinessObject.Type).ToString());
            ManejarControlesPorEstado("A");
        }


        private void ManejarControlesPorEstado(string Estado)
        {
            //((SAPbouiCOM.EditText)this.UIAPIRawForm.Items.Item("Item_16").Specific).Active = true;
            switch (Estado)
            {
                case "A":
                    cmbEmpresas.Item.Enabled = true;
                    cmbBancos.Item.Enabled = true;
                    cmbCuentas.Item.Enabled = true;
                    btcOpciones.Item.Enabled = false;
                    edtFechaDesde.Item.Enabled = true;
                    edtFechaHasta.Item.Enabled = true;
                    btnBuscar.Item.Enabled = true;
                    cmbSeries.Item.Enabled = true;
                    mtxOperaciones.Columns.Item("Col_0").Editable = true;
                    break;
                case "P":
                case "E":
                    cmbEmpresas.Item.Enabled = false;
                    cmbBancos.Item.Enabled = false;
                    cmbCuentas.Item.Enabled = false;
                    btcOpciones.Item.Enabled = true;
                    edtFechaDesde.Item.Enabled = false;
                    edtFechaHasta.Item.Enabled = false;
                    btnBuscar.Item.Enabled = false;
                    cmbSeries.Item.Enabled = false;
                    mtxOperaciones.Columns.Item("Col_0").Editable = true;
                    break;
                case "R":
                    cmbEmpresas.Item.Enabled = false;
                    cmbBancos.Item.Enabled = false;
                    cmbCuentas.Item.Enabled = false;
                    btcOpciones.Item.Enabled = false;
                    edtFechaDesde.Item.Enabled = false;
                    edtFechaHasta.Item.Enabled = false;
                    btnBuscar.Item.Enabled = false;
                    cmbSeries.Item.Enabled = false;
                    mtxOperaciones.Columns.Item("Col_0").Editable = false;
                    break;
                default:
                    break;
            }

        }

        private void Button1_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                var filaBorrada = true;

                if (lstOperaciones.Count == 0)
                {
                    throw new InvalidOperationException("Matriz de operaciones vacia");
                }
                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    if (lstOperaciones.Count > 0 && !lstOperaciones.Any(o => o.Seleccionado == "Y"))
                    {
                        throw new InvalidOperationException("Debe seleccionar al menos una linea");
                    }

                    while (filaBorrada)
                    {
                        filaBorrada = false;
                        for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                        {
                            if (dbsEXD_CBN1.GetValue("U_SELECCIONADO", i) == "N")
                            {
                                dbsEXD_CBN1.RemoveRecord(i);
                                filaBorrada = true;
                                break;
                            }
                        }
                    }
                    mtxOperaciones.LoadFromDataSource();
                }

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    dbsEXD_OCBN.SetValueExt("U_ESTADO", "P");
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                BubbleEvent = false;
            }
        }

        private void mtxOperaciones_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Col_0")
            {
                mtxOperaciones.FlushToDataSource();
                var id = dbsEXD_CBN1.GetValue("LineId", pVal.Row - 1);
                var seleccionado = dbsEXD_CBN1.GetValue("U_SELECCIONADO", pVal.Row - 1);
                var operacion = lstOperaciones.FirstOrDefault(o => o.Id.ToString() == id);
                operacion.Seleccionado = seleccionado;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            var estado = dbsEXD_OCBN.GetValueExt("U_ESTADO");
            var dsrEXDCBN1 = XmlDSSerializer.DeserializeDBDataSource(dbsEXD_CBN1.GetAsXML());

            //Cargo bancos
            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 inner join DSC1 T1 on T0.\"BankCode\" = T1.\"BankCode\" where T1.\"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbBancos.LoadValidValues(recSet);

            // cargo cuentas
            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbCuentas.LoadValidValues(recSet);

            lstOperaciones = dsrEXDCBN1.Rows.Select(r => new OperacionDto
            {
                Id = Convert.ToInt32(r.Cells.FirstOrDefault(c => c.Uid == "LineId").Value),
                Seleccionado = r.Cells.FirstOrDefault(c => c.Uid == "U_SELECCIONADO").Value,
                CodOperacion = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_OPERACION").Value,
                Glosa = r.Cells.FirstOrDefault(c => c.Uid == "U_GLOSA").Value,
                Importe = Convert.ToDouble(r.Cells.FirstOrDefault(c => c.Uid == "U_IMPORTE").Value),
                NroCuenta = r.Cells.FirstOrDefault(c => c.Uid == "U_NUM_CUENTA").Value,
                TipoImporte = r.Cells.FirstOrDefault(c => c.Uid == "U_TIPO_IMPORTE").Value,
                NroSecuencia = Convert.ToInt32(r.Cells.FirstOrDefault(c => c.Uid == "U_NUM_SECUENCIA").Value),
                CodPagoSAP = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_PAGO_SAP")?.Value,
                NroReferencia = r.Cells.FirstOrDefault(c => c.Uid == "U_NUM_OPERACION")?.Value
            }).ToList();

            ManejarControlesPorEstado(estado);
        }

        private void btcOpciones_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
        }

        private void btcOpciones_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            switch (btcOpciones.Selected?.Value)
            {
                case "E":
                    var cntOK = 0;
                    var cntErr = 0;
                    //Generar pago 
                    var lstOperacionesAux = lstOperaciones.Where(o => string.IsNullOrWhiteSpace(o.CodPagoSAP) && o.Seleccionado == "Y").ToList();
                    foreach (var ope in lstOperacionesAux)
                    {
                        try
                        {
                            var rslt = GenerarPago(ope);
                            for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                            {
                                if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i))
                                {
                                    dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                                    dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                                }
                            }
                            ope.CodPagoSAP = rslt.Item1.ToString();
                            cntOK++;
                        }
                        catch (Exception ex)
                        {
                            cntErr++;
                            Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                        }
                    }
                    mtxOperaciones.LoadFromDataSource();
                    if (cntErr == 0 && cntOK > 0) dbsEXD_OCBN.SetValueExt("U_ESTADO", "E");
                    if (cntOK > 0)
                    {
                        if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                    }
                    ManejarControlesPorEstado("E");
                    break;
                case "R":
                    try
                    {
                        GenerarReconciliacion(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));
                        dbsEXD_OCBN.SetValueExt("U_ESTADO", "R");
                        if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                        this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                        Application.SBO_Application.SetStatusSuccessMessage("Reconciliación realizada con éxito");
                        ManejarControlesPorEstado("R");
                    }
                    catch (Exception ex)
                    {
                        Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                    }
                    break;
                default:
                    Application.SBO_Application.MessageBox("Seleccione una opción");
                    break;
            }
        }

        private Tuple<int, int> GenerarPago(OperacionDto ope)
        {
            var payment = (SAPbobsCOM.Payments)this.GetCompany().GetBusinessObject(ope.TipoImporte == "D"
                ? SAPbobsCOM.BoObjectTypes.oVendorPayments : SAPbobsCOM.BoObjectTypes.oIncomingPayments);
            var codSucursal = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var ctaPrincipal = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");
            var codMoneda = dbsEXD_OCBN.GetValueExt("U_COD_MONEDA");

            payment.DocDate = DateTime.Today;
            payment.DueDate = DateTime.Today;
            payment.TaxDate = DateTime.Today;
            payment.DocCurrency = codMoneda;
            payment.BPLID = Convert.ToInt32(codSucursal);
            payment.DocType = SAPbobsCOM.BoRcptTypes.rAccount;

            payment.TransferAccount = ctaPrincipal;
            payment.TransferDate = DateTime.Today;
            payment.TransferReference = ope.NroReferencia;
            payment.TransferSum = ope.Importe;

            payment.AccountPayments.AccountCode = ObtenerCodigoDeCuenta(ope.NroCuenta);
            payment.AccountPayments.AccountName = ope.NroCuenta;
            payment.AccountPayments.GrossAmount = ope.Importe;
            payment.AccountPayments.SumPaid = ope.Importe;

            var rslt = payment.Add();

            if (rslt == 0)
                return new Tuple<int, int>(Convert.ToInt32(this.GetCompany().GetNewObjectKey()), ope.TipoImporte == "D" ? 46 : 24);
            else
                throw new InvalidOperationException(this.GetCompany().GetLastErrorDescription());
        }

        private string ObtenerCodigoDeCuenta(string formatCode)
        {
            var sqlQry = $"select \"AcctCode\" from  OACT where \"FormatCode\" = '{formatCode}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            return recSet.Fields.Item(0).Value.ToString();
        }

        private void GenerarReconciliacion(int docEntry)
        {
            var oCompanyService = this.GetCompany().GetCompanyService();
            var ExtReconSvc = (SAPbobsCOM.ExternalReconciliationsService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ExternalReconciliationsService);
            var ExtReconciliation = (SAPbobsCOM.ExternalReconciliation)ExtReconSvc.GetDataInterface(SAPbobsCOM.ExternalReconciliationsServiceDataInterfaces.ersExternalReconciliation);
            var sqlQry = string.Empty;

            ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION('{docEntry}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION '{docEntry}'";

            recSet.DoQuery(sqlQry);
            while (!recSet.EoF)
            {
                SAPbobsCOM.ReconciliationJournalEntryLine jeLine1 = (SAPbobsCOM.ReconciliationJournalEntryLine)ExtReconciliation.ReconciliationJournalEntryLines.Add();
                jeLine1.TransactionNumber = Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                jeLine1.LineNumber = Convert.ToInt32(recSet.Fields.Item(1).Value.ToString());

                SAPbobsCOM.ReconciliationBankStatementLine bstLine1 = (SAPbobsCOM.ReconciliationBankStatementLine)ExtReconciliation.ReconciliationBankStatementLines.Add();
                bstLine1.BankStatementAccountCode = recSet.Fields.Item(2).Value.ToString();
                bstLine1.Sequence = Convert.ToInt32(recSet.Fields.Item(3).Value.ToString());
                recSet.MoveNext();
            }
            ExtReconSvc.Reconcile(ExtReconciliation);
        }

        private void mtxOperaciones_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;
            if (pVal.ColUID == "Col_0")
            {
                var nroCuenta = ((SAPbouiCOM.EditText)mtxOperaciones.GetCellSpecific("Col_4", pVal.Row)).Value;
                if (string.IsNullOrWhiteSpace(nroCuenta))
                {
                    Application.SBO_Application.MessageBox("No se ha asignado una cuenta para este código bancario");
                    BubbleEvent = false;
                }
            }
        }

        private void mtxOperaciones_LinkPressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.ColUID == "Col_8")
            {
                ((SAPbouiCOM.LinkedButton)mtxOperaciones.Columns.Item(pVal.ColUID).ExtendedObject).LinkedObjectType =
                     ((SAPbouiCOM.EditText)mtxOperaciones.GetCellSpecific("Col_10", pVal.Row)).Value;
            }
        }
    }
}
