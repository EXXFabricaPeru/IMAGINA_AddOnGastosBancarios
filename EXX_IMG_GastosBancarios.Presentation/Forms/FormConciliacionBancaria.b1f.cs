using EXX_IMG_GastosBancarios.Domain.Entities;
using EXX_IMG_GastosBancarios.Presentation.Helper;
using JF_SBOAddon.Utiles.Extensions;
using JF_SBOAddon.Utiles.Utilities;
using Newtonsoft.Json;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            isOpen = true;
        }
        public static bool isOpen;

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
            this.edtFechaContabilizacion = ((SAPbouiCOM.EditText)(this.GetItem("Item_21").Specific));
            this.StaticText8 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_22").Specific));
            this.Button0 = ((SAPbouiCOM.Button)(this.GetItem("Item_23").Specific));
            this.Button0.PressedAfter += new SAPbouiCOM._IButtonEvents_PressedAfterEventHandler(this.Button0_PressedAfter);
            this.OnCustomInitialize();

        }

        /// <summary>
        /// Initialize form event. Called by framework before form creation.
        /// </summary>
        public override void OnInitializeFormEvents()
        {
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.DataAddAfter += new SAPbouiCOM.Framework.FormBase.DataAddAfterHandler(this.Form_DataAddAfter);
            this.ResizeAfter += new SAPbouiCOM.Framework.FormBase.ResizeAfterHandler(this.Form_ResizeAfter);
            this.ClickBefore += new SAPbouiCOM.Framework.FormBase.ClickBeforeHandler(this.Form_ClickBefore);
            this.CloseAfter += new CloseAfterHandler(this.Form_CloseAfter);

        }

        private SAPbouiCOM.StaticText StaticText0;

        private void OnCustomInitialize()
        {
            btcOpciones.Item.Visible = false;
            ComboBox5.Item.Visible = false;
            StaticText7.Item.Visible = false;

            var sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);


            while (cmbEmpresas.ValidValues.Count > 0)
            {
                cmbEmpresas.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbEmpresas.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("BPLId").Value.ToString();
                string name = recSet.Fields.Item("BPLName").Value.ToString();
                cmbEmpresas.ValidValues.Add(code, name);
                recSet.MoveNext();
            }


            //cmbEmpresas.LoadValidValues(recSet);

            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 ";
            recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            //cmbBancos.LoadValidValues(recSet);

            cmbBancos.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("BankCode").Value.ToString();
                string name = recSet.Fields.Item("BankName").Value.ToString();
                cmbBancos.ValidValues.Add(code, name);
                recSet.MoveNext();
            }

            cmbCuentas.ValidValues.Add("-1", "Todos");

            btcOpciones.ValidValues.Add("E", "Ejecutar");
            btcOpciones.ValidValues.Add("R", "Reconciliar");

            mtxOperaciones.Columns.Item("Col_7").Visible = false;
            mtxOperaciones.Columns.Item("Col_9").Visible = false;
            mtxOperaciones.Columns.Item("Col_10").Visible = false;

            sqlQry = "select * from ODIM";
            recSet.DoQuery(sqlQry);
            while (!recSet.EoF)
            {
                var dimCode = recSet.Fields.Item(0).Value.ToString();
                var dimDesc = recSet.Fields.Item(3).Value.ToString();
                var dimActv = recSet.Fields.Item(2).Value.ToString();
                mtxOperaciones.Columns.Item($"Col_{Convert.ToInt32(dimCode) + 12}").Visible = (dimActv == "Y");
                mtxOperaciones.Columns.Item($"Col_{Convert.ToInt32(dimCode) + 12}").TitleObject.Caption = dimDesc;
                recSet.MoveNext();
            }
            this.FormDataLoadAdd();

            cmbBancos.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            cmbCuentas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            cmbEmpresas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);

            try
            {

                mtxOperaciones.Columns.Item("Col_1").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_2").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_3").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_4").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_5").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_6").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_7").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_8").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_9").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_10").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_11").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_12").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_13").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_14").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_15").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_16").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_17").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_18").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_19").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_20").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_21").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_22").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_23").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_24").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_25").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_26").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_27").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_28").TitleObject.Sortable = true;
            }
            catch (Exception)
            {

            }
        }

        private void cmbEmpresas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            //var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 inner join DSC1 T1 on T0.\"BankCode\" = T1.\"BankCode\" where T1.\"Branch\" = '{codEmp}'";
            //var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //recSet.DoQuery(sqlQry);
            //cmbBancos.LoadValidValues(recSet);

            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);

            while (cmbCuentas.ValidValues.Count > 0)
            {
                cmbCuentas.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbCuentas.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("GLAccount").Value.ToString();
                string name = recSet.Fields.Item("Account").Value.ToString();
                cmbCuentas.ValidValues.Add(code, name);
                recSet.MoveNext();
            }
            cmbCuentas.Item.DisplayDesc = true;
            cmbCuentas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
            //cmbCuentas.LoadValidValues(recSet);
        }

        private void cmbBancos_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            //var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            //var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            //var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //recSet.DoQuery(sqlQry);
            //cmbCuentas.LoadValidValues(recSet);

            while (cmbCuentas.ValidValues.Count > 0)
            {
                cmbCuentas.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbCuentas.ValidValues.Add("-1", "Todos");
            try
            {
                fillData();
                cmbCuentas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                cmbCuentas.Item.DisplayDesc = true;
            }
            catch (Exception)
            {
            }


        }

        private void cmbCuentas_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {

            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            var codCta = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");

            var sqlQry = $"select TX1.\"ActCurr\" from DSC1 TX0 inner join OACT  TX1 on TX0.\"GLAccount\" = TX1.\"AcctCode\"  where TX0.\"BankCode\" = '{codBnc}' and TX0.\"Branch\" = '{codEmp}' and TX0.\"GLAccount\" = '{codCta}'";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            if (!recSet.EoF)
                dbsEXD_OCBN.SetValueExt("U_COD_MONEDA", recSet.Fields.Item(0).Value.ToString());
            else
                dbsEXD_OCBN.SetValueExt("U_COD_MONEDA", "");
        }

        private void btnBuscar_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                //ServiceLayerHelper.Connect();

                
                lstOperaciones = new List<OperacionDto>();
                var nroLinea = 0;
                var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");
                var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
                var codCta = dbsEXD_OCBN.GetValueExt("U_COD_CUENTA");
                var fchDsd = dbsEXD_OCBN.GetValueExt("U_FECHA_DESDE");
                var fchHst = dbsEXD_OCBN.GetValueExt("U_FECHA_HASTA");
                var sqlQry = string.Empty;

                codEmp = string.IsNullOrWhiteSpace(codEmp) ? "-1" : codEmp;


                if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                {
                    if (codBnc == "-1" && codEmp == "-1" && codCta == "-1")
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES(null,null,null,'{fchDsd}','{fchHst}')";
                    else if (codBnc != "-1" && codEmp == "-1" && codCta == "-1")
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES(null,'{codBnc}',null,'{fchDsd}','{fchHst}')";
                    else if (codBnc == "-1" && codEmp != "-1" && codCta == "-1")
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}',null,null,'{fchDsd}','{fchHst}')";
                    else if (codBnc != "-1" && codEmp != "-1" && codCta == "-1")
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}','{codBnc}',null,'{fchDsd}','{fchHst}')";
                    else if (codBnc != "-1" && codEmp != "-1" && codCta != "-1")
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}','{codBnc}','{codCta}','{fchDsd}','{fchHst}')";

                    //if (codCta == "-1")
                    //    sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}','{codBnc}',null,'{fchDsd}','{fchHst}')";
                    //else
                    //    sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES('{codEmp}','{codBnc}','{codCta}','{fchDsd}','{fchHst}')";
                }
                else
                    sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_OPERACIONES_PENDIENTES '{codEmp}','{codBnc}','{codCta}','{fchDsd}','{fchHst}'";

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

                    dbsEXD_CBN1.SetValue("U_COD_PROYECTO", nroLinea, recSet.Fields.Item(10).Value.ToString());
                    operacionDto.CodProyecto = recSet.Fields.Item(10).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_DIM1", nroLinea, recSet.Fields.Item(11).Value.ToString());
                    operacionDto.CodDim1 = recSet.Fields.Item(11).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_DIM2", nroLinea, recSet.Fields.Item(12).Value.ToString());
                    operacionDto.CodDim2 = recSet.Fields.Item(12).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_DIM3", nroLinea, recSet.Fields.Item(13).Value.ToString());
                    operacionDto.CodDim3 = recSet.Fields.Item(13).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_DIM4", nroLinea, recSet.Fields.Item(14).Value.ToString());
                    operacionDto.CodDim4 = recSet.Fields.Item(14).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_DIM5", nroLinea, recSet.Fields.Item(15).Value.ToString());
                    operacionDto.CodDim5 = recSet.Fields.Item(15).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_PARPRE", nroLinea, recSet.Fields.Item(16).Value.ToString());
                    operacionDto.CodParPre = recSet.Fields.Item(16).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_NOM_PARPRE", nroLinea, recSet.Fields.Item(17).Value.ToString());
                    operacionDto.NomParPre = recSet.Fields.Item(17).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_MPTRABAN", nroLinea, recSet.Fields.Item(18).Value.ToString());
                    operacionDto.CodMPTraBan = recSet.Fields.Item(18).Value.ToString();

                    //NUEVOS CAMPOS
                    dbsEXD_CBN1.SetValue("U_COD_BANCO", nroLinea, recSet.Fields.Item(19).Value.ToString());
                    operacionDto.IdBanco = recSet.Fields.Item(19).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_DES_BANCO", nroLinea, recSet.Fields.Item(20).Value.ToString());
                    //operacionDto.CodMPTraBan = recSet.Fields.Item(20).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_EMPRESA", nroLinea, recSet.Fields.Item(21).Value.ToString());
                    operacionDto.Idsucursal = recSet.Fields.Item(21).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_DES_EMPRESA", nroLinea, recSet.Fields.Item(22).Value.ToString());
                    //operacionDto.CodMPTraBan = recSet.Fields.Item(22).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_CUENTA", nroLinea, recSet.Fields.Item(23).Value.ToString());
                    operacionDto.Idcuenta = recSet.Fields.Item(23).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_DES_CUENTA", nroLinea, recSet.Fields.Item(24).Value.ToString());
                    //operacionDto.CodMPTraBan = recSet.Fields.Item(24).Value.ToString();

                    dbsEXD_CBN1.SetValue("U_COD_MONEDA", nroLinea, recSet.Fields.Item(25).Value.ToString());
                    operacionDto.Idmoneda = recSet.Fields.Item(25).Value.ToString();


                    lstOperaciones.Add(operacionDto);
                    nroLinea++;
                    recSet.MoveNext();
                }

                mtxOperaciones.LoadFromDataSource();
                mtxOperaciones.AutoResizeColumns();
                aplicarOrden();

            }
            catch (Exception)
            {

            }

        }

        private void aplicarOrden()
        {
            try
            {

                mtxOperaciones.Columns.Item("Col_1").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_2").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_3").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_4").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_5").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_6").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_7").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_8").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_9").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_10").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_11").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_12").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_13").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_14").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_15").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_16").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_17").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_18").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_19").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_20").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_21").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_22").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_23").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_24").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_25").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_26").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_27").TitleObject.Sortable = true;
                mtxOperaciones.Columns.Item("Col_28").TitleObject.Sortable = true;
            }
            catch (Exception)
            {

            }
        }
        internal void FormDataLoadAdd()
        {
            try
            {
                dbsEXD_OCBN.SetValueExt("U_FECHA_DESDE", DateTime.Today.ToString("yyyyMMdd"));
                dbsEXD_OCBN.SetValueExt("U_FECHA_HASTA", DateTime.Today.ToString("yyyyMMdd"));
                dbsEXD_OCBN.SetValueExt("U_FECHA_CONT", DateTime.Today.ToString("yyyyMMdd"));


                dbsEXD_OCBN.SetValueExt("U_ESTADO", "A");

                cmbSeries.ValidValues.LoadSeries(this.UIAPIRawForm.BusinessObject.Type, SAPbouiCOM.BoSeriesMode.sf_Add);
                if (cmbSeries.ValidValues.Count > 0) cmbSeries.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                dbsEXD_OCBN.SetValueExt("DocNum", this.UIAPIRawForm.BusinessObject.GetNextSerialNumber(dbsEXD_OCBN.GetValueExt("Series")
                    , this.UIAPIRawForm.BusinessObject.Type).ToString());
                ManejarControlesPorEstado("A");
            }
            catch (Exception)
            {

            }


        }


        private void ManejarControlesPorEstado(string Estado)
        {
            //((SAPbouiCOM.EditText)this.UIAPIRawForm.Items.Item("Item_16").Specific).Active = true;
            //Application.SBO_Application.SetStatusWarningMessage("ManejarControlesPorEstado");
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
                    edtFechaContabilizacion.Item.Enabled = true;

                    edtFechaHasta.Value = DateTime.Today.ToString("yyyyMMdd");
                    edtFechaDesde.Value = DateTime.Today.ToString("yyyyMMdd");
                    edtFechaContabilizacion.Value = DateTime.Today.ToString("yyyyMMdd");

                    dbsEXD_OCBN.SetValueExt("U_FECHA_DESDE", DateTime.Today.ToString("yyyyMMdd"));
                    dbsEXD_OCBN.SetValueExt("U_FECHA_HASTA", DateTime.Today.ToString("yyyyMMdd"));
                    dbsEXD_OCBN.SetValueExt("U_FECHA_CONT", DateTime.Today.ToString("yyyyMMdd"));
                    //Application.SBO_Application.SetStatusWarningMessage("ManejarControlesPorEstado  estado");
                    cmbBancos.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    cmbEmpresas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    cmbCuentas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    cmbSeries.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);

                    mtxOperaciones.Columns.Item("Col_0").Editable = true;
                    //cmbBancos.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    //cmbEmpresas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    //cmbCuentas.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
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
                    //edtFechaContabilizacion.Item.Enabled = false;
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
                    try
                    {
                        edtFechaContabilizacion.Item.Enabled = false;
                    }
                    catch (Exception)
                    {

                    }

                    //edtFechaContabilizacion.Item.Enabled = false;
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
                Application.SBO_Application.SetStatusWarningMessage("Procesando...");
                var filaBorrada = true;

                if (lstOperaciones.Count == 0)
                {
                    throw new InvalidOperationException("Matriz de operaciones vacia");
                }
                if (mtxOperaciones.RowCount == 0)
                {
                    throw new InvalidOperationException("Matriz de operaciones vacia");
                }
                if (dbsEXD_CBN1.Size == 0)
                {
                    throw new InvalidOperationException("Matriz de operaciones vacia");
                }

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    {
                        var check = (SAPbouiCOM.CheckBox)mtxOperaciones.Columns.Item("Col_0").Cells.Item(i).Specific;
                        var seleccionado = check.Checked ? "Y" : "N";

                        mtxOperaciones.FlushToDataSource();

                        var id = dbsEXD_CBN1.GetValue("LineId", i - 1);
                        //var seleccionado = dbsEXD_CBN1.GetValue("U_SELECCIONADO", i - 1);
                        var operacion = lstOperaciones.FirstOrDefault(o => o.Id.ToString() == id);
                        dbsEXD_CBN1.SetValue("U_SELECCIONADO", i - 1, seleccionado);
                        operacion.Seleccionado = seleccionado;
                        //Application.SBO_Application.SetStatusWarningMessage(seleccionado + " - linea : "+ id);

                    }
                    if (lstOperaciones.Count > 0 && !lstOperaciones.Any(o => o.Seleccionado == "Y"))
                    {
                        throw new InvalidOperationException("Debe seleccionar al menos una linea");
                    }

                }

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    dbsEXD_OCBN.SetValueExt("U_ESTADO", "R");

                if (string.IsNullOrEmpty(edtFechaContabilizacion.Value))
                {
                    throw new InvalidOperationException("Debe seleccionar la fecha de contabilización");
                }

                var cntOK = 0;
                var cntErr = 0;

                edtFechaContabilizacion.Active = true;
                //ManejarControlesPorEstado("R");

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    ////Generar pago 
                    //Application.SBO_Application.SetStatusWarningMessage("Generando Pago...");
                    //var lstOperacionesAux = lstOperaciones.Where(o => string.IsNullOrWhiteSpace(o.CodPagoSAP) && o.Seleccionado == "Y").ToList();
                    //foreach (var ope in lstOperacionesAux)
                    //{
                    //    try
                    //    {
                    //        var rslt = GenerarPago(ope);
                    //        //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1 + "-"+ rslt.Item2);

                    //        for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    //        {
                    //            //Application.SBO_Application.SetStatusWarningMessage("Generando Pago... " + i);
                    //            var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                    //            var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                    //            var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                    //            var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                    //            if (ope.NroSecuencia.ToString() == numSecuencia.Value && ope.IdBanco == codbanco.Value && ope.Idsucursal== codSucursal.Value && ope.Idcuenta== codCuenta.Value)
                    //            {
                    //                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_8").Cells.Item(i).Specific).Value = rslt.Item1.ToString();
                    //                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_10").Cells.Item(i).Specific).Value = rslt.Item2.ToString();
                    //                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific).Value = "Procesado";
                    //                mtxOperaciones.FlushToDataSource();
                    //                //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1.ToString() +" - "+ rslt.Item2.ToString());
                    //                //dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                    //                //dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                    //                //dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                    //            }
                    //        }
                    //        //for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                    //        //{
                    //        //    if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                    //        //    {
                    //        //        dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                    //        //        dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                    //        //        dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                    //        //    }
                    //        //}
                    //        ope.CodPagoSAP = rslt.Item1.ToString();
                    //        cntOK++;
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        cntErr++;

                    //        for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    //        {
                    //            //Application.SBO_Application.SetStatusWarningMessage("Generando Pago... " + i);
                    //            var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                    //            var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                    //            var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                    //            var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                    //            //if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                    //            if (ope.NroSecuencia.ToString() == numSecuencia.Value && ope.IdBanco == codbanco.Value && ope.Idsucursal == codSucursal.Value && ope.Idcuenta == codCuenta.Value)
                    //            {
                    //                ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = ex.Message;
                    //                //dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message);
                    //                mtxOperaciones.FlushToDataSource();
                    //            }
                    //        }
                    //        //for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                    //        //{
                    //        //    if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                    //        //    //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                    //        //    {
                    //        //        ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific)).Value = ex.Message;
                    //        //        dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message);
                    //        //        mtxOperaciones.FlushToDataSource();
                    //        //    }
                    //        //}
                    //        //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1.ToString() + " - " + rslt.Item2.ToString());
                    //        mtxOperaciones.AutoResizeColumns();
                    //        //throw new InvalidOperationException("Error de Pago");
                    //        //Application.SBO_Application.SetStatusErrorMessage("GenerarPago:" + ex.Message);
                    //    }
                    //}
                    if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    {
                        //if (lstOperaciones.Count > 0 && !lstOperaciones.Any(o => o.Seleccionado == "Y"))
                        //{
                        //    throw new InvalidOperationException("Debe seleccionar al menos una linea");
                        //}
                        while (filaBorrada)
                        {
                            filaBorrada = false;
                            for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                            {
                                var selec = (SAPbouiCOM.CheckBox)mtxOperaciones.Columns.Item("Col_0").Cells.Item(i).Specific;
                                if (!selec.Checked)
                                {
                                    mtxOperaciones.DeleteRow(i);
                                    filaBorrada = true;
                                    break;
                                }
                            }
                        }
                        mtxOperaciones.FlushToDataSource();
                        //while (filaBorrada)
                        //{
                        //    filaBorrada = false;
                        //    for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                        //    {
                        //        if (dbsEXD_CBN1.GetValue("U_SELECCIONADO", i) == "N")
                        //        {
                        //            dbsEXD_CBN1.RemoveRecord(i);
                        //            filaBorrada = true;
                        //            break;
                        //        }
                        //    }
                        //}
                        //mtxOperaciones.LoadFromDataSource();
                    }
                    ////mtxOperaciones.LoadFromDataSource();

                    //if (cntErr == 0 && cntOK > 0) dbsEXD_OCBN.SetValueExt("U_ESTADO", "E");
                    ////if (cntOK > 0)
                    ////{
                    ////    if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    ////        this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                    ////    this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                    ////}
                    //ManejarControlesPorEstado("E");
                    //Application.SBO_Application.SetStatusWarningMessage("Pago Generado...");
                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                BubbleEvent = false;
            }
        }

        private void ProcesoDePagosBack()
        {
            try
            {
                var filaBorrada = true;
                var cntOK = 0;
                var cntErr = 0;
                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    //Generar pago 
                    Application.SBO_Application.SetStatusWarningMessage("Generando Pago...");
                    var lstOperacionesAux = lstOperaciones.Where(o => string.IsNullOrWhiteSpace(o.CodPagoSAP) && o.Seleccionado == "Y").ToList();
                    foreach (var ope in lstOperacionesAux)
                    {
                        try
                        {
                            var rslt = GenerarPago(ope, "");
                            //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1 + "-"+ rslt.Item2);

                            for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                            {
                                //Application.SBO_Application.SetStatusWarningMessage("Generando Pago... " + i);
                                var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                                var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                                var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                                var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                                if (ope.NroSecuencia.ToString() == numSecuencia.Value && ope.IdBanco == codbanco.Value && ope.Idsucursal == codSucursal.Value && ope.Idcuenta == codCuenta.Value)
                                {
                                    ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_8").Cells.Item(i).Specific).Value = rslt.Item1.ToString();
                                    ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_10").Cells.Item(i).Specific).Value = rslt.Item2.ToString();
                                    ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific).Value = "Procesado";
                                    mtxOperaciones.FlushToDataSource();
                                    //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1.ToString() +" - "+ rslt.Item2.ToString());
                                    //dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                                    //dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                                    //dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                                }
                            }
                            //for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                            //{
                            //    if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                            //    {
                            //        dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                            //        dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                            //        dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                            //    }
                            //}
                            ope.CodPagoSAP = rslt.Item1.ToString();
                            cntOK++;
                        }
                        catch (Exception ex)
                        {
                            cntErr++;

                            for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                            {
                                //Application.SBO_Application.SetStatusWarningMessage("Generando Pago... " + i);
                                var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                                var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                                var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                                var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                                //if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                                if (ope.NroSecuencia.ToString() == numSecuencia.Value && ope.IdBanco == codbanco.Value && ope.Idsucursal == codSucursal.Value && ope.Idcuenta == codCuenta.Value)
                                {
                                    ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = ex.Message;
                                    //dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message);
                                    mtxOperaciones.FlushToDataSource();
                                }
                            }
                            //for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                            //{
                            //    if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                            //    //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                            //    {
                            //        ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific)).Value = ex.Message;
                            //        dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message);
                            //        mtxOperaciones.FlushToDataSource();
                            //    }
                            //}
                            //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1.ToString() + " - " + rslt.Item2.ToString());
                            mtxOperaciones.AutoResizeColumns();
                            //throw new InvalidOperationException("Error de Pago");
                            //Application.SBO_Application.SetStatusErrorMessage("GenerarPago:" + ex.Message);
                        }
                    }
                    if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                    {
                        //if (lstOperaciones.Count > 0 && !lstOperaciones.Any(o => o.Seleccionado == "Y"))
                        //{
                        //    throw new InvalidOperationException("Debe seleccionar al menos una linea");
                        //}
                        while (filaBorrada)
                        {
                            filaBorrada = false;
                            for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                            {
                                var selec = (SAPbouiCOM.CheckBox)mtxOperaciones.Columns.Item("Col_0").Cells.Item(i).Specific;
                                if (!selec.Checked)
                                {
                                    mtxOperaciones.DeleteRow(i);
                                    filaBorrada = true;
                                    break;
                                }
                            }
                        }
                        mtxOperaciones.FlushToDataSource();
                        //while (filaBorrada)
                        //{
                        //    filaBorrada = false;
                        //    for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                        //    {
                        //        if (dbsEXD_CBN1.GetValue("U_SELECCIONADO", i) == "N")
                        //        {
                        //            dbsEXD_CBN1.RemoveRecord(i);
                        //            filaBorrada = true;
                        //            break;
                        //        }
                        //    }
                        //}
                        //mtxOperaciones.LoadFromDataSource();
                    }
                    //mtxOperaciones.LoadFromDataSource();

                    if (cntErr == 0 && cntOK > 0) dbsEXD_OCBN.SetValueExt("U_ESTADO", "E");
                    //if (cntOK > 0)
                    //{
                    //    if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                    //        this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                    //    this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                    //}
                    ManejarControlesPorEstado("E");
                    Application.SBO_Application.SetStatusWarningMessage("Pago Generado...");
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);

            }
        }

        private void GeneracióndePagos(int docEntry)
        {
            try
            {
                Application.SBO_Application.SetStatusWarningMessage("Iniciando Generación de Pagos :" + docEntry);
                var listPagos = ObtenerPagos(docEntry);

                foreach (var ope in listPagos)
                {
                    try
                    {
                        var pago = GenerarPago(ope, ope.FechaCont);
                        for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                        {
                            if (ope.Idcuenta == dbsEXD_CBN1.GetValue("U_COD_CUENTA", i) && ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                            {
                                dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, pago.Item1.ToString());
                                dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, pago.Item2.ToString());
                                dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_8").Cells.Item(i + 1).Specific).Value = pago.Item1.ToString();
                                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_10").Cells.Item(i + 1).Specific).Value = pago.Item2.ToString();
                                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific).Value = "Procesado";
                                mtxOperaciones.FlushToDataSource();
                                //contCOrrectoReprocesar++;
                            }
                        }
                        ActualizarPago(docEntry, ope, pago.Item1, pago.Item2, "Procesado");


                    }
                    catch (Exception ex)
                    {
                        Application.SBO_Application.SetStatusWarningMessage("Error Generación de Pagos :" + ex.Message);
                        for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                        {
                            if (ope.Idcuenta == dbsEXD_CBN1.GetValue("U_COD_CUENTA", i) && ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                            {

                                dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message.Length > 200 ? ex.Message.Substring(0, 199) : ex.Message);

                                ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific).Value = ex.Message.Length > 200 ? ex.Message.Substring(0, 199) : ex.Message;
                                //contCOrrectoReprocesar++;
                                mtxOperaciones.FlushToDataSource();
                            }
                        }
                        ActualizarPago(docEntry, ope, 0, 0, ex.Message.Length > 200 ? ex.Message.Substring(0, 199) : ex.Message);
                    }

                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);

            }
        }

        private void ActualizarPago(int docEntry, OperacionDto ope, int CodPago, int tipoPago, string estado)
        {
            try
            {
                var sqlQry = string.Empty;
                var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                if (CodPago > 0)
                {
                    if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                        sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS({docEntry},{ope.Id},{CodPago},{tipoPago}, '{estado}')";
                    else
                        sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_UPDATE_PAGOS '{docEntry},{ope.Id},{CodPago},{tipoPago}, '{estado}''";
                }
                else
                {
                    if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
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
        private List<OperacionDto> ObtenerPagos(int docEntry)
        {
            var sqlQry = string.Empty;

            //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
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

        private void mtxOperaciones_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            if (pVal.ColUID == "Col_0")
            {
                mtxOperaciones.FlushToDataSource();
                var id = dbsEXD_CBN1.GetValue("LineId", pVal.Row - 1);
                var seleccionado = dbsEXD_CBN1.GetValue("U_SELECCIONADO", pVal.Row - 1);
                var operacion = lstOperaciones.FirstOrDefault(o => o.Id.ToString() == id);
                operacion.Seleccionado = seleccionado;
                //Application.SBO_Application.SetStatusWarningMessage(seleccionado + " - linea : "+ id);
            }

        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            //Application.SBO_Application.SetStatusWarningMessage("sadsdasdasdasdasdas");
            //Application.SBO_Application.SetStatusWarningMessage("Form_DataLoadAfter");
            var estado = dbsEXD_OCBN.GetValueExt("U_ESTADO");
            var dsrEXDCBN1 = XmlDSSerializer.DeserializeDBDataSource(dbsEXD_CBN1.GetAsXML());

            var codEmp = dbsEXD_OCBN.GetValueExt("U_COD_EMPRESA");


            var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 ";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            //cmbBancos.LoadValidValues(recSet);

            while (cmbBancos.ValidValues.Count > 0)
            {
                cmbBancos.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbBancos.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("BankCode").Value.ToString();
                string name = recSet.Fields.Item("BankName").Value.ToString();
                cmbBancos.ValidValues.Add(code, name);
                recSet.MoveNext();
            }
            cmbBancos.Item.DisplayDesc = true;


            sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
            recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);


            while (cmbEmpresas.ValidValues.Count > 0)
            {
                cmbEmpresas.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbEmpresas.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("BPLId").Value.ToString();
                string name = recSet.Fields.Item("BPLName").Value.ToString();
                cmbEmpresas.ValidValues.Add(code, name);
                recSet.MoveNext();
            }
            cmbEmpresas.Item.DisplayDesc = true;



            var codBnc = dbsEXD_OCBN.GetValueExt("U_COD_BANCO");
            sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);

            while (cmbCuentas.ValidValues.Count > 0)
            {
                cmbCuentas.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
            }

            cmbCuentas.ValidValues.Add("-1", "Todos");
            while (!recSet.EoF)
            {
                string code = recSet.Fields.Item("GLAccount").Value.ToString();
                string name = recSet.Fields.Item("Account").Value.ToString();
                cmbCuentas.ValidValues.Add(code, name);
                recSet.MoveNext();
            }

            cmbCuentas.Item.DisplayDesc = true;



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
                NroReferencia = r.Cells.FirstOrDefault(c => c.Uid == "U_NUM_OPERACION")?.Value,
                CodProyecto = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_PROYECTO")?.Value,
                CodDim1 = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_DIM1")?.Value,
                CodDim2 = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_DIM2")?.Value,
                CodDim3 = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_DIM3")?.Value,
                CodDim4 = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_DIM4")?.Value,
                CodDim5 = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_DIM5")?.Value,
                CodParPre = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_PARPRE")?.Value,
                NomParPre = r.Cells.FirstOrDefault(c => c.Uid == "U_NOM_PARPRE")?.Value,
                CodMPTraBan = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_MPTRABAN")?.Value,
                IdBanco = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_BANCO")?.Value,
                Idsucursal = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_EMPRESA")?.Value,
                Idcuenta = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_CUENTA")?.Value,
                Idmoneda = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_MONEDA")?.Value,
                Estado = r.Cells.FirstOrDefault(c => c.Uid == "U_COD_ESTADO")?.Value,
            }).ToList();

            ManejarControlesPorEstado(estado);
            mtxOperaciones.AutoResizeColumns();
            aplicarOrden();
        }

        private void btcOpciones_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_OK_MODE;
        }

        private void btcOpciones_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //switch (btcOpciones.Selected?.Value)
            //{
            //    case "E":
            //        var cntOK = 0;
            //        var cntErr = 0;
            //        //Generar pago 
            //        var lstOperacionesAux = lstOperaciones.Where(o => string.IsNullOrWhiteSpace(o.CodPagoSAP) && o.Seleccionado == "Y").ToList();
            //        Application.SBO_Application.SetStatusWarningMessage("lista: " + lstOperacionesAux.Count);
            //        foreach (var ope in lstOperacionesAux)
            //        {
            //            try
            //            {
            //                var rslt = GenerarPago(ope);
            //                for (int i = 0; i < dbsEXD_CBN1.Size; i++)
            //                {
            //                    if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
            //                    {
            //                        dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
            //                        dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
            //                    }
            //                }
            //                ope.CodPagoSAP = rslt.Item1.ToString();
            //                cntOK++;
            //            }
            //            catch (Exception ex)
            //            {
            //                cntErr++;
            //                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            //            }
            //        }
            //        mtxOperaciones.LoadFromDataSource();
            //        if (cntErr == 0 && cntOK > 0) dbsEXD_OCBN.SetValueExt("U_ESTADO", "E");
            //        if (cntOK > 0)
            //        {
            //            if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            //                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
            //            this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
            //        }
            //        ManejarControlesPorEstado("E");
            //        break;
            //    case "R":
            //        try
            //        {
            //            GenerarReconciliacion(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));
            //            dbsEXD_OCBN.SetValueExt("U_ESTADO", "R");
            //            if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
            //                this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
            //            this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
            //            Application.SBO_Application.SetStatusSuccessMessage("Reconciliación realizada con éxito");
            //            ManejarControlesPorEstado("R");
            //        }
            //        catch (Exception ex)
            //        {
            //            Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            //        }
            //        break;
            //    default:
            //        Application.SBO_Application.MessageBox("Seleccione una opción");
            //        break;
            //}
        }

        private Tuple<int, int> GenerarPago(OperacionDto ope, string fechaCont)
        {
            var payment = (SAPbobsCOM.Payments)this.GetCompany().GetBusinessObject(ope.TipoImporte == "D"
                ? SAPbobsCOM.BoObjectTypes.oVendorPayments : SAPbobsCOM.BoObjectTypes.oIncomingPayments);
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
            payment.DocType = SAPbobsCOM.BoRcptTypes.rAccount;

            //Application.SBO_Application.SetStatusWarningMessage("657");

            payment.TransferAccount = ope.Idcuenta; //payment.TransferAccount = ctaPrincipal;
            payment.TransferDate = DateTime.ParseExact(fechaCont, "yyyyMMdd", CultureInfo.InvariantCulture);
            payment.TransferReference = ope.NroReferencia;
            payment.TransferSum = ope.Importe;

            payment.UserFields.Fields.Item("U_EXX_MPTRABAN").Value = ope.CodMPTraBan;

            payment.AccountPayments.AccountCode = ObtenerCodigoDeCuenta(ope.NroCuenta);
            payment.AccountPayments.AccountName = ope.NroCuenta;
            payment.AccountPayments.GrossAmount = ope.Importe;
            payment.AccountPayments.SumPaid = ope.Importe;
            //Application.SBO_Application.SetStatusWarningMessage("670");
            payment.AccountPayments.ProjectCode = ope.CodProyecto;
            payment.AccountPayments.ProfitCenter = ope.CodDim1;
            payment.AccountPayments.ProfitCenter2 = ope.CodDim2;
            payment.AccountPayments.ProfitCenter3 = ope.CodDim3;
            payment.AccountPayments.ProfitCenter4 = ope.CodDim4;
            payment.AccountPayments.ProfitCenter5 = ope.CodDim5;
            payment.JournalRemarks = ope.Glosa;


            payment.AccountPayments.UserFields.Fields.Item("U_EXC_PARTPRES").Value = ope.CodParPre;
            payment.AccountPayments.UserFields.Fields.Item("U_EXC_NOMPPR").Value = ope.NomParPre;
            payment.UserFields.Fields.Item("U_EXX_CODGB").Value = ope.Idcuenta + "-" + ope.NroSecuencia;

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
            //var oCompanyService = this.GetCompany().GetCompanyService();
            //var ExtReconSvc = (SAPbobsCOM.ExternalReconciliationsService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ExternalReconciliationsService);
            //var ExtReconciliation = (SAPbobsCOM.ExternalReconciliation)ExtReconSvc.GetDataInterface(SAPbobsCOM.ExternalReconciliationsServiceDataInterfaces.ersExternalReconciliation);
            var sqlQry = string.Empty;

            //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION('{docEntry}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION '{docEntry}'";

            recSet.DoQuery(sqlQry);

            Application.SBO_Application.SetStatusWarningMessage("Generar Reconciliacion " + mtxOperaciones.RowCount);

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


                //var oCompanyService = this.GetCompany().GetCompanyService();
                //var ExtReconSvc = (SAPbobsCOM.ExternalReconciliationsService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ExternalReconciliationsService);
                //var ExtReconciliation = (SAPbobsCOM.ExternalReconciliation)ExtReconSvc.GetDataInterface(SAPbobsCOM.ExternalReconciliationsServiceDataInterfaces.ersExternalReconciliation);
                //Application.SBO_Application.SetStatusWarningMessage("Procesando " + recSet.Fields.Item(0).Value.ToString());

                //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;

                //SAPbobsCOM.ReconciliationJournalEntryLine jeLine1 = (SAPbobsCOM.ReconciliationJournalEntryLine)ExtReconciliation.ReconciliationJournalEntryLines.Add();
                //jeLine1.TransactionNumber = Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                //jeLine1.LineNumber = Convert.ToInt32(recSet.Fields.Item(1).Value.ToString());

                //SAPbobsCOM.ReconciliationBankStatementLine bstLine1 = (SAPbobsCOM.ReconciliationBankStatementLine)ExtReconciliation.ReconciliationBankStatementLines.Add();
                //bstLine1.BankStatementAccountCode = recSet.Fields.Item(2).Value.ToString();
                //bstLine1.Sequence = Convert.ToInt32(recSet.Fields.Item(3).Value.ToString());
                //ExtReconSvc.Reconcile(ExtReconciliation);


                //for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                //{
                //    var linea = recSet.Fields.Item(4).Value.ToString();
                //    var banco = recSet.Fields.Item(5).Value.ToString();
                //    var estado = recSet.Fields.Item(6).Value.ToString();
                //    var secuencia = recSet.Fields.Item(3).Value.ToString();
                //    var sucursal = recSet.Fields.Item(7).Value.ToString();
                //    var cuenta = recSet.Fields.Item(8).Value.ToString();

                //    var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                //    var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                //    var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                //    var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                //    if (numSecuencia.Value == secuencia && codbanco.Value == banco && sucursal == codSucursal.Value && cuenta == codCuenta.Value)
                //    //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                //    {
                //        ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = "Reconciliado";
                //        dbsEXD_CBN1.SetValue("U_COD_ESTADO", i - 1, "Reconciliado");
                //        mtxOperaciones.FlushToDataSource();

                //    }
                //}
                //contCOrrectoReprocesar++;

                recSet.MoveNext();
            }
            //if (recSet != null)
            //    recSet = null;


            foreach (var item in reconDtos)
            {
                try
                {
                    var oCompanyService = this.GetCompany().GetCompanyService();
                    var ExtReconSvc = (SAPbobsCOM.ExternalReconciliationsService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ExternalReconciliationsService);
                    var ExtReconciliation = (SAPbobsCOM.ExternalReconciliation)ExtReconSvc.GetDataInterface(SAPbobsCOM.ExternalReconciliationsServiceDataInterfaces.ersExternalReconciliation);
                    Application.SBO_Application.SetStatusWarningMessage("Procesando " +item.LineIDPago);

                    ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;

                    SAPbobsCOM.ReconciliationJournalEntryLine jeLine1 = (SAPbobsCOM.ReconciliationJournalEntryLine)ExtReconciliation.ReconciliationJournalEntryLines.Add();
                    jeLine1.TransactionNumber = item.TransID;//Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                    jeLine1.LineNumber = item.LineIDPago;// Convert.ToInt32(recSet.Fields.Item(1).Value.ToString());

                    SAPbobsCOM.ReconciliationBankStatementLine bstLine1 = (SAPbobsCOM.ReconciliationBankStatementLine)ExtReconciliation.ReconciliationBankStatementLines.Add();
                    bstLine1.BankStatementAccountCode = item.Cuenta;// recSet.Fields.Item(2).Value.ToString();
                    bstLine1.Sequence = item.Secuencia;//  Convert.ToInt32(recSet.Fields.Item(3).Value.ToString());
                    ExtReconSvc.Reconcile(ExtReconciliation);


                    for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    {
                        //var linea = recSet.Fields.Item(4).Value.ToString();
                        //var banco = recSet.Fields.Item(5).Value.ToString();
                        //var estado = recSet.Fields.Item(6).Value.ToString();
                        //var secuencia = recSet.Fields.Item(3).Value.ToString();
                        //var sucursal = recSet.Fields.Item(7).Value.ToString();
                        //var cuenta = recSet.Fields.Item(8).Value.ToString();

                        var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                        var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                        var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                        var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                        if (numSecuencia.Value == item.Secuencia.ToString() && codbanco.Value == item.Banco && item.Sucursal == codSucursal.Value && item.Cuenta == codCuenta.Value)
                        //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                        {
                            ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = "Reconciliado";
                            dbsEXD_CBN1.SetValue("U_COD_ESTADO", i - 1, "Reconciliado");
                            mtxOperaciones.FlushToDataSource();
                            ActualizarRecon(docEntry, item.LineID, "Reconciliado");

                        }
                    }
                    contCOrrectoReprocesar++;

                }
                catch (Exception ex)
                {
                    for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    {
                        //var linea = recSet.Fields.Item(4).Value.ToString();
                        //var banco = recSet.Fields.Item(5).Value.ToString();
                        //var estado = recSet.Fields.Item(6).Value.ToString();
                        //var secuencia = recSet.Fields.Item(3).Value.ToString();
                        //var sucursal = recSet.Fields.Item(7).Value.ToString();
                        //var cuenta = recSet.Fields.Item(8).Value.ToString();
                        var error = ex.Message.Length > 200 ? ex.Message.Substring(0, 199) : ex.Message;
                        var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                        var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                        var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                        var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                        if (numSecuencia.Value == item.Secuencia.ToString() && codbanco.Value == item.Banco && item.Sucursal == codSucursal.Value && item.Cuenta == codCuenta.Value)
                        //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                        {
                            ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = error;
                            dbsEXD_CBN1.SetValue("U_COD_ESTADO", i - 1, error);
                            mtxOperaciones.FlushToDataSource();
                            ActualizarRecon(docEntry, item.LineID, error);

                        }
                    }
                }
            }

            Application.SBO_Application.SetStatusWarningMessage("Reconciliación procesada");
        }


        private void GenerarReconciliacionSL(int docEntry)
        {
            //var oCompanyService = this.GetCompany().GetCompanyService();
            //var ExtReconSvc = (SAPbobsCOM.ExternalReconciliationsService)oCompanyService.GetBusinessService(SAPbobsCOM.ServiceTypes.ExternalReconciliationsService);
            //var ExtReconciliation = (SAPbobsCOM.ExternalReconciliation)ExtReconSvc.GetDataInterface(SAPbobsCOM.ExternalReconciliationsServiceDataInterfaces.ersExternalReconciliation);
            var sqlQry = string.Empty;

            //ExtReconciliation.ReconciliationAccountType = SAPbobsCOM.ReconciliationAccountTypeEnum.rat_GLAccount;
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
                sqlQry = $"CALL EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION('{docEntry}')";
            else
                sqlQry = $"EXEC EXD_IMG_CNBN_LISTAR_DATOS_RECONCILIACION '{docEntry}'";

            recSet.DoQuery(sqlQry);

            Application.SBO_Application.SetStatusWarningMessage("Generar Reconciliacion " + mtxOperaciones.RowCount);

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
            //if (recSet != null)
            //    recSet = null;

            ExternalReconciliationSL reconciliationSL =  new ExternalReconciliationSL();
            reconciliationSL.ExternalReconciliation = new ExternalReconciliationSL.ExternalReconciliationModel();
            reconciliationSL.ExternalReconciliation.ReconciliationAccountType = "rat_GLAccount";
            reconciliationSL.ExternalReconciliation.ReconciliationJournalEntryLines = new List<ExternalReconciliationSL.ReconciliationJournalEntryLine>();
            reconciliationSL.ExternalReconciliation.ReconciliationBankStatementLines = new List<ExternalReconciliationSL.ReconciliationBankStatementLine>();

            foreach (var item in reconDtos)
            {
                try
                {

                    var url = Globals.ServiceLayerUrl + Globals.ExternalReconciliationService;

                    var journalLine = new ExternalReconciliationSL.ReconciliationJournalEntryLine();
                    journalLine.TransactionNumber = item.TransID;//Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                    journalLine.LineNumber = item.LineIDPago;//

                    reconciliationSL.ExternalReconciliation.ReconciliationJournalEntryLines.Add(journalLine);

                    var entryLine = new ExternalReconciliationSL.ReconciliationBankStatementLine();
                    entryLine.BankStatementAccountCode = item.Cuenta;//Convert.ToInt32(recSet.Fields.Item(0).Value.ToString());
                    entryLine.Sequence = item.Secuencia;// Convert.ToInt32(recSet.Fields.Item(1).Value.ToString());

                    reconciliationSL.ExternalReconciliation.ReconciliationBankStatementLines.Add(entryLine);
                    
                    var jsonBody = JsonConvert.SerializeObject(reconciliationSL, new JsonSerializerSettings
                    {
                        DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate,
                    });
                    ServiceLayerHelper.PostSL(url, jsonBody);


                    for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    {
                        var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                        var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                        var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                        var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                        if (numSecuencia.Value == item.Secuencia.ToString() && codbanco.Value == item.Banco && item.Sucursal == codSucursal.Value && item.Cuenta == codCuenta.Value)
                        //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                        {
                            ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = "Reconciliado";
                            dbsEXD_CBN1.SetValue("U_COD_ESTADO", i - 1, "Reconciliado");
                            mtxOperaciones.FlushToDataSource();
                            ActualizarRecon(docEntry, item.LineID, "Reconciliado");

                        }
                    }
                    contCOrrectoReprocesar++;

                }
                catch (Exception ex)
                {
                    for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                    {

                        var error = ex.Message.Length > 200 ? ex.Message.Substring(0, 199) : ex.Message;
                        var numSecuencia = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_9").Cells.Item(i).Specific;
                        var codbanco = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_21").Cells.Item(i).Specific;
                        var codSucursal = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_23").Cells.Item(i).Specific;
                        var codCuenta = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_25").Cells.Item(i).Specific;

                        if (numSecuencia.Value == item.Secuencia.ToString() && codbanco.Value == item.Banco && item.Sucursal == codSucursal.Value && item.Cuenta == codCuenta.Value)
                        //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                        {
                            ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific)).Value = error;
                            dbsEXD_CBN1.SetValue("U_COD_ESTADO", i - 1, error);
                            mtxOperaciones.FlushToDataSource();
                            ActualizarRecon(docEntry, item.LineID, error);

                        }
                    }
                }
            }

            Application.SBO_Application.SetStatusWarningMessage("Reconciliación procesada");
        }
        private void ActualizarRecon(int docEntry, int lineID, string estado)
        {
            try
            {
                var sqlQry = string.Empty;
                var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                if (this.GetCompany().DbServerType == SAPbobsCOM.BoDataServerTypes.dst_HANADB)
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

        private SAPbouiCOM.EditText edtFechaContabilizacion;
        private SAPbouiCOM.StaticText StaticText8;

        private void Form_DataAddAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("Form_DataAddAfter: Procesando reconciliación");
                //Application.SBO_Application.SetStatusWarningMessage(dbsEXD_OCBN.GetValueExt("DocEntry"));

                GeneracióndePagos(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));
                //GenerarReconciliacion(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));
                GenerarReconciliacionSL(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));


                Application.SBO_Application.SetStatusSuccessMessage(" Reconciliación realizada con éxito");
                ManejarControlesPorEstado("A");
                UIAPIRawForm.Refresh();


                edtFechaHasta.Value = DateTime.Today.ToString("yyyyMMdd");
                edtFechaDesde.Value = DateTime.Today.ToString("yyyyMMdd");
                edtFechaContabilizacion.Value = DateTime.Today.ToString("yyyyMMdd");

                dbsEXD_OCBN.SetValueExt("U_FECHA_DESDE", DateTime.Today.ToString("yyyyMMdd"));
                dbsEXD_OCBN.SetValueExt("U_FECHA_HASTA", DateTime.Today.ToString("yyyyMMdd"));
                dbsEXD_OCBN.SetValueExt("U_FECHA_CONT", DateTime.Today.ToString("yyyyMMdd"));

            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            }

        }

        private SAPbouiCOM.Button Button0;

        public static void ShowDialogMessageBox(string message, Action isSuccess = null, Action isCancel = null)
        {
            var result = Application.SBO_Application.MessageBox(message, Btn1Caption: "OK", Btn2Caption: "Cancelar");
            if (result == 1)
                isSuccess?.Invoke();
            else
                isCancel?.Invoke();
        }

        int contCOrrectoReprocesar = 0;
        private void Button0_PressedAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                contCOrrectoReprocesar = 0;

                var cntOK = 0;
                if (UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE)
                {
                    ShowDialogMessageBox("Se procederá a procesar las líneas pendientes de reconciliacion. ¿Desea continuar? ",
                  () =>
                  {
                      //var pendientes = lstOperaciones.Where(t => t.Estado != "Reconciliado").Count();
                      var pendientesPagados = lstOperaciones.Where(t => t.Estado == "Procesados").Count();
                      var total = lstOperaciones.Count();
                      var Reconciliados = lstOperaciones.Where(t => t.Estado == "Reconciliado").Count();
                      var porProcesar = total - Reconciliados;
                      //var error = total - Reconciliados- pendientesPagados;
                      int pendientes = 0;
                      for (int i = 1; i <= mtxOperaciones.RowCount; i++)
                      {
                          var estado = (SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i).Specific;
                          if (estado.Value != "Reconciliado")
                          {
                              pendientes++;
                          }
                      }

                      if (pendientes > 0)
                      {
                          Application.SBO_Application.SetStatusWarningMessage("Inicio Reproceso de pagos");
                          foreach (var ope in lstOperaciones.Where(t => string.IsNullOrEmpty(t.CodPagoSAP)))
                          {
                              try
                              {

                                  if (string.IsNullOrEmpty(ope.CodPagoSAP))
                                  {
                                      Application.SBO_Application.SetStatusWarningMessage("Generando Pago: " + ope.CodPagoSAP);
                                      var fechaCont = edtFechaContabilizacion.Value;
                                      var rslt = GenerarPago(ope, fechaCont);
                                      //Application.SBO_Application.SetStatusWarningMessage(rslt.Item1 + "-"+ rslt.Item2);
                                      for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                                      {
                                          if (ope.Idcuenta == dbsEXD_CBN1.GetValue("U_COD_CUENTA", i) && ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                                          {
                                              dbsEXD_CBN1.SetValue("U_COD_PAGO_SAP", i, rslt.Item1.ToString());
                                              dbsEXD_CBN1.SetValue("U_TIPO_PAGO", i, rslt.Item2.ToString());
                                              dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, "Procesado");
                                              ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_8").Cells.Item(i + 1).Specific).Value = rslt.Item1.ToString();
                                              ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_10").Cells.Item(i + 1).Specific).Value = rslt.Item2.ToString();
                                              ((SAPbouiCOM.EditText)mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific).Value = "Procesado";
                                              //contCOrrectoReprocesar++;
                                          }
                                      }
                                      ope.CodPagoSAP = rslt.Item1.ToString();
                                  }

                                  cntOK++;
                              }
                              catch (Exception ex)
                              {
                                  //cntErr++;
                                  for (int i = 0; i < dbsEXD_CBN1.Size; i++)
                                  {
                                      if (ope.NroSecuencia.ToString() == dbsEXD_CBN1.GetValue("U_NUM_SECUENCIA", i) && ope.IdBanco == dbsEXD_CBN1.GetValue("U_COD_BANCO", i))
                                      //if (ope.IdBanco.ToString() == dbsEXD_CBN1.GetValue("U_COD_BANCO", i) && ope.Importe == double.Parse(dbsEXD_CBN1.GetValue("U_IMPORTE", i).ToString()))
                                      {
                                          ((SAPbouiCOM.EditText)(mtxOperaciones.Columns.Item("Col_28").Cells.Item(i + 1).Specific)).Value = ex.Message;
                                          dbsEXD_CBN1.SetValue("U_COD_ESTADO", i, ex.Message);
                                      }
                                  }

                                  mtxOperaciones.AutoResizeColumns();
                                  //throw new InvalidOperationException("Error de Pago");
                                  Application.SBO_Application.SetStatusErrorMessage("GenerarPago:" + ex.Message);
                              }

                              if (cntOK > 0)
                              {
                                  if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                                      this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                                  this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);
                              }
                          }

                          Application.SBO_Application.SetStatusWarningMessage("Inicio Reproceso de reconciliación");

                          GenerarReconciliacionSL(Convert.ToInt32(dbsEXD_OCBN.GetValueExt("DocEntry")));

                          dbsEXD_OCBN.SetValueExt("U_ESTADO", "R");

                          if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE)
                              this.UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
                          this.UIAPIRawForm.Items.Item("1").Click(SAPbouiCOM.BoCellClickType.ct_Regular);

                          Application.SBO_Application.MessageBox("Procesados correctamente " + contCOrrectoReprocesar + "/" + porProcesar);
                          Application.SBO_Application.SetStatusSuccessMessage("Reconciliación realizada con éxito");
                      }
                      else
                      {
                          Application.SBO_Application.MessageBox("No se encontraron registros con errores por procesar");
                      }


                  },
                  null);




                }
                else
                {
                    Application.SBO_Application.SetStatusErrorMessage("Solo se puede reprocesar una vez creada la conciliación bancaria");
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            }
            finally
            {
                UIAPIRawForm.Refresh();
            }

        }

        private void Form_ResizeAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            mtxOperaciones.AutoResizeColumns();

        }

        private void fillData()
        {
            try
            {
                if (string.IsNullOrEmpty(edtFechaHasta.Value))
                {
                    edtFechaHasta.Value = DateTime.Today.ToString("yyyyMMdd");
                }
                if (string.IsNullOrEmpty(edtFechaDesde.Value))
                {
                    edtFechaDesde.Value = DateTime.Today.ToString("yyyyMMdd");
                }
                if (string.IsNullOrEmpty(edtFechaContabilizacion.Value))
                {
                    edtFechaContabilizacion.Value = DateTime.Today.ToString("yyyyMMdd");
                }

                if (string.IsNullOrEmpty(cmbSeries.Selected?.Value))
                {
                    cmbSeries.Select(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }



            }
            catch (Exception)
            {

            }
        }
        private void Form_ClickBefore(SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            fillData();
        }

        private void Form_CloseAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                isOpen = false;
            }
            catch (Exception)
            {

            }

        }
    }
}
