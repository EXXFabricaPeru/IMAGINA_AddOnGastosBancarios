using JF_SBOAddon.Utiles.Extensions;
using JF_SBOAddon.Utiles.Utilities;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXX_IMG_GastosBancarios.Presentation.Forms
{
    [FormAttribute("FormMaestroCodigosBancarios", "Forms/FormMaestroCodigosBancarios.b1f")]
    class FormMaestroCodigosBancarios : UserFormBase
    {
        private SAPbouiCOM.ComboBox cmbEmpresas;
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox cmbBancos;
        private SAPbouiCOM.StaticText StaticText2;
        private SAPbouiCOM.ComboBox cmbCuentas;
        private SAPbouiCOM.StaticText StaticText3;
        private SAPbouiCOM.EditText edtMoneda;
        private SAPbouiCOM.Matrix mtxCodBancs;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
        private SAPbouiCOM.StaticText StaticText0;
        private SAPbouiCOM.EditText EditText1;

        private SAPbouiCOM.DBDataSource dbsEXD_OMCB = null;
        private SAPbouiCOM.DBDataSource dbsEXD_MCB1 = null;

        private int rightClickRow = -1;
        public FormMaestroCodigosBancarios()
        {
        }

        /// <summary>
        /// Initialize components. Called by framework after form created.
        /// </summary>
        public override void OnInitializeComponent()
        {
            this.StaticText0 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_0").Specific));
            this.cmbEmpresas = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_1").Specific));
            this.cmbEmpresas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbEmpresas_ComboSelectAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.cmbBancos = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_3").Specific));
            this.cmbBancos.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbBancos_ComboSelectAfter);
            this.StaticText2 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_4").Specific));
            this.cmbCuentas = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_5").Specific));
            this.cmbCuentas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbCuentas_ComboSelectAfter);
            this.StaticText3 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_6").Specific));
            this.edtMoneda = ((SAPbouiCOM.EditText)(this.GetItem("Item_8").Specific));
            this.mtxCodBancs = ((SAPbouiCOM.Matrix)(this.GetItem("Item_9").Specific));
            this.mtxCodBancs.ChooseFromListAfter += new SAPbouiCOM._IMatrixEvents_ChooseFromListAfterEventHandler(this.mtxCodBancs_ChooseFromListAfter);
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("1").Specific));
            this.Button0.PressedBefore += new SAPbouiCOM._IButtonEvents_PressedBeforeEventHandler(this.Button0_PressedBefore);
            this.Button1 = ((SAPbouiCOM.Button)(this.GetItem("2").Specific));
            this.dbsEXD_OMCB = this.UIAPIRawForm.GetDBDataSource("@EXD_OMCB");
            this.dbsEXD_MCB1 = this.UIAPIRawForm.GetDBDataSource("@EXD_MCB1");
            this.EditText1 = ((SAPbouiCOM.EditText)(this.GetItem("Item_7").Specific));
            this.OnCustomInitialize();
        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.RightClickBefore += new SAPbouiCOM.Framework.FormBase.RightClickBeforeHandler(this.Form_RightClickBefore);
            this.DataLoadAfter += new DataLoadAfterHandler(this.Form_DataLoadAfter);
        }

        internal void FormDataLoadAdd()
        {
            cmbEmpresas.Item.Enabled = true;
            cmbBancos.Item.Enabled = true;
            cmbCuentas.Item.Enabled = true;
            edtMoneda.Item.Enabled = false;
        }

        private void OnCustomInitialize()
        {
            var sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbEmpresas.LoadValidValues(recSet);

            mtxCodBancs.Columns.Item("Col_4").Visible = false;

            var cflCta = this.UIAPIRawForm.ChooseFromLists.Item("CFL_CTA");
            var cnds = cflCta.GetConditions();
            var cnd = cnds.Add();
            cnd.Alias = "Postable";
            cnd.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
            cnd.CondVal = "Y";
            cnd.Relationship = SAPbouiCOM.BoConditionRelationship.cr_AND;
            cnd = cnds.Add();
            cnd.Alias = "LocManTran";
            cnd.Operation = SAPbouiCOM.BoConditionOperation.co_NOT_EQUAL;
            cnd.CondVal = "Y";
            cflCta.SetConditions(null);
            cflCta.SetConditions(cnds);
        }

        private void cmbEmpresas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
            var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 inner join DSC1 T1 on T0.\"BankCode\" = T1.\"BankCode\" where T1.\"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbBancos.LoadValidValues(recSet);
            dbsEXD_OMCB.SetValueExt("U_COD_BANCO", null);
            dbsEXD_OMCB.SetValueExt("U_COD_CUENTA", null);
            dbsEXD_OMCB.SetValueExt("U_COD_MONEDA", null);
        }

        private void cmbBancos_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
            var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbCuentas.LoadValidValues(recSet);
            dbsEXD_OMCB.SetValueExt("U_COD_CUENTA", null);
            dbsEXD_OMCB.SetValueExt("U_COD_MONEDA", null);
        }

        private void cmbCuentas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
            var codCta = dbsEXD_OMCB.GetValueExt("U_COD_CUENTA");
            var sqlQry = $"select TX1.\"ActCurr\" from DSC1 TX0 inner join OACT  TX1 on TX0.\"GLAccount\" = TX1.\"AcctCode\"  where TX0.\"BankCode\" = '{codBnc}' and TX0.\"Branch\" = '{codEmp}' and TX0.\"GLAccount\" = '{codCta}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            if (!recSet.EoF) dbsEXD_OMCB.SetValueExt("U_COD_MONEDA", recSet.Fields.Item(0).Value.ToString());
        }

        private void Form_RightClickBefore(ref SAPbouiCOM.ContextMenuInfo eventInfo, out bool BubbleEvent)
        {
            BubbleEvent = true;
            this.UIAPIRawForm.EnableMenu("1292", false);
            this.UIAPIRawForm.EnableMenu("1293", false);
            if (eventInfo.ItemUID == mtxCodBancs.Item.UniqueID && eventInfo.Row >= 0)
            {
                rightClickRow = eventInfo.Row;
                this.UIAPIRawForm.EnableMenu("1292", true);
                if (eventInfo.Row >= 1) this.UIAPIRawForm.EnableMenu("1293", true);
            }
        }

        public void AddNewLineMatrix()
        {
            mtxCodBancs.AddRow();
            mtxCodBancs.ClearRowData(mtxCodBancs.RowCount);
            //mtxCodBancs.FlushToDataSource();
        }

        public void DeleLineMatrix()
        {
            mtxCodBancs.DeleteRow(rightClickRow);
            mtxCodBancs.FlushToDataSource();
            if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
        }

        private void mtxCodBancs_ChooseFromListAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            var cflEvent = (dynamic)pVal;
            mtxCodBancs.FlushToDataSource();
            if (cflEvent.SelectedObjects is SAPbouiCOM.DataTable dtbl)
            {
                dbsEXD_MCB1.SetValue("U_NRO_CUENTA", pVal.Row - 1, dtbl.GetValue("FormatCode", 0).ToString());
                mtxCodBancs.LoadFromDataSource();
            }
        }

        private void Button0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE) return;

                var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA")))
                {
                    throw new InvalidOperationException("Seleccione una empresa");
                }

                if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_BANCO")))
                {
                    throw new InvalidOperationException("Seleccione un banco");
                }

                if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_CUENTA")))
                {
                    throw new InvalidOperationException("Seleccione una cuenta");
                }

                var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
                var codBanco = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
                var codCuenta = dbsEXD_OMCB.GetValueExt("U_COD_CUENTA");

                var sqlQry = $"select COUNT('A') from \"@EXD_OMCB\" where U_COD_EMPRESA = '{codEmp}' and U_COD_BANCO = '{codBanco}' and U_COD_CUENTA = '{codCuenta}'";
                recSet.DoQuery(sqlQry);

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE && Convert.ToInt32(recSet.Fields.Item(0).Value) > 0)
                {
                    throw new InvalidOperationException("Ya existe un registro para este empresa, banco y cuenta");
                }

                if (mtxCodBancs.RowCount == 0)
                {
                    throw new InvalidOperationException("Registre al menos un código de operación");
                }

                mtxCodBancs.FlushToDataSource();
                for (int i = 0; i < mtxCodBancs.VisualRowCount; i++)
                {
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", i + 1)).Active = true;
                        throw new InvalidOperationException("Ingrese un código de operación");
                    }

                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_2", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_2", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un número de cuenta");
                    }
                }

                var ultimoCodigoAñadido = ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", mtxCodBancs.VisualRowCount)).Value;
                var dsrEXDMCB1 = XmlDSSerializer.DeserializeDBDataSource(dbsEXD_MCB1.GetAsXML());
                var rsltExisteCodOpe = dsrEXDMCB1.Rows.Count(r => r.Cells.FirstOrDefault(c => c.Uid == "U_COD_OPE_BANC").Value == ultimoCodigoAñadido);

                if (rsltExisteCodOpe > 1)
                {
                    ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", mtxCodBancs.VisualRowCount)).Active = true;
                    throw new InvalidOperationException("Ya se ha registrado este codigo de operación");
                }


                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                        sqlQry = "select 'CB'||right('000000' || ltrim(right(coalesce(max(\"Code\"),'0'),6)+1),6) as \"Codigo\" from \"@EXD_OMCB\"";
                    else
                        sqlQry = "select 'CB'+right('000000' + ltrim(right(coalesce(max(\"Code\"),0),6)+1),6) as \"Codigo\" from \"@EXD_OMCB\"";

                    recSet.DoQuery(sqlQry);
                    if (!recSet.EoF)
                    {
                        dbsEXD_OMCB.SetValueExt("Code", recSet.Fields.Item(0).Value.ToString());
                        dbsEXD_OMCB.SetValueExt("Name", recSet.Fields.Item(0).Value.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                BubbleEvent = false;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            cmbEmpresas.Item.Enabled = false;
            cmbBancos.Item.Enabled = false;
            cmbCuentas.Item.Enabled = false;
            edtMoneda.Item.Enabled = false;

            //Cargo bancos
            var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
            var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 inner join DSC1 T1 on T0.\"BankCode\" = T1.\"BankCode\" where T1.\"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbBancos.LoadValidValues(recSet);

            // cargo cuentas
            var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
            sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbCuentas.LoadValidValues(recSet);
        }

        public void EnModoBusqueda()
        {
            cmbEmpresas.Item.Enabled = true;
            cmbBancos.Item.Enabled = true;
            cmbCuentas.Item.Enabled = true;
            edtMoneda.Item.Enabled = true;
        }
    }
}
