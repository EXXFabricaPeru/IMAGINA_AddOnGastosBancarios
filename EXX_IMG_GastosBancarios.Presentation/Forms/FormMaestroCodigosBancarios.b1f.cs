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
        private SAPbouiCOM.StaticText StaticText1;
        private SAPbouiCOM.ComboBox cmbBancos;
        private SAPbouiCOM.Matrix mtxCodBancs;
        private SAPbouiCOM.Button Button0;
        private SAPbouiCOM.Button Button1;
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
            //   this.cmbEmpresas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbEmpresas_ComboSelectAfter);
            this.StaticText1 = ((SAPbouiCOM.StaticText)(this.GetItem("Item_2").Specific));
            this.cmbBancos = ((SAPbouiCOM.ComboBox)(this.GetItem("Item_3").Specific));
            this.cmbBancos.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbBancos_ComboSelectAfter);
            //   this.cmbCuentas.ComboSelectAfter += new SAPbouiCOM._IComboBoxEvents_ComboSelectAfterEventHandler(this.cmbCuentas_ComboSelectAfter);
            this.mtxCodBancs = ((SAPbouiCOM.Matrix)(this.GetItem("Item_9").Specific));
            this.mtxCodBancs.ValidateAfter += new SAPbouiCOM._IMatrixEvents_ValidateAfterEventHandler(this.mtxCodBancs_ValidateAfter);
            this.mtxCodBancs.ComboSelectAfter += new SAPbouiCOM._IMatrixEvents_ComboSelectAfterEventHandler(this.mtxCodBancs_ComboSelectAfter);
            this.mtxCodBancs.ClickBefore += new SAPbouiCOM._IMatrixEvents_ClickBeforeEventHandler(this.mtxCodBancs_ClickBefore);
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
            this.DataLoadAfter += new SAPbouiCOM.Framework.FormBase.DataLoadAfterHandler(this.Form_DataLoadAfter);
            this.LoadAfter += new LoadAfterHandler(this.Form_LoadAfter);

        }

        internal void FormDataLoadAdd()
        {
            //cmbEmpresas.Item.Enabled = true;
            cmbBancos.Item.Enabled = true;
            //cmbCuentas.Item.Enabled = true;
            //edtMoneda.Item.Enabled = false;
        }

        private void OnCustomInitialize()
        {
            var sqlQry = " select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0 ";// "select \"BPLId\",\"BPLName\" from OBPL";
            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            recSet.DoQuery(sqlQry);
            cmbBancos.LoadValidValues(recSet);
            try
            {
                mtxCodBancs.Columns.Item("Col_4").Visible = false;
                mtxCodBancs.Columns.Item("Col_0").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_1").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_2").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_3").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_4").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_5").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_6").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_7").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_8").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_9").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_10").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_11").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_12").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_13").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_14").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_15").TitleObject.Sortable = true;
                mtxCodBancs.Columns.Item("Col_16").TitleObject.Sortable = true;
            }
            catch (Exception)
            {

            }
         

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

            sqlQry = "select * from ODIM";
            recSet.DoQuery(sqlQry);
            while (!recSet.EoF)
            {
                var dimCode = recSet.Fields.Item(0).Value.ToString();
                var dimDesc = recSet.Fields.Item(3).Value.ToString();
                var dimActv = recSet.Fields.Item(2).Value.ToString();
                var clfDim = this.UIAPIRawForm.ChooseFromLists.Item($"CFL_DIM{dimCode}");
                var dimCnds = clfDim.GetConditions();
                var dimCnd = dimCnds.Add();
                dimCnd.Alias = "DimCode";
                dimCnd.Operation = SAPbouiCOM.BoConditionOperation.co_EQUAL;
                dimCnd.CondVal = dimCode;
                clfDim.SetConditions(null);
                clfDim.SetConditions(dimCnds);
                mtxCodBancs.Columns.Item($"Col_{Convert.ToInt32(dimCode) + 4}").Visible = (dimActv == "Y");
                mtxCodBancs.Columns.Item($"Col_{Convert.ToInt32(dimCode) + 4}").TitleObject.Caption = dimDesc;
                recSet.MoveNext();
            }
        }

       

        private void cmbBancos_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            //var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
            //var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
            //var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            //var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //recSet.DoQuery(sqlQry);
            //cmbCuentas.LoadValidValues(recSet);
            //dbsEXD_OMCB.SetValueExt("U_COD_CUENTA", null);
            //dbsEXD_OMCB.SetValueExt("U_COD_MONEDA", null);
            for (int i = 1; i <= mtxCodBancs.RowCount; i++)
            {
                var cuenta = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("").Cells.Item(i).Specific;
                while (cuenta.ValidValues.Count > 0)
                {
                    cuenta.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                }
            }
            
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
                switch (pVal.ColUID)
                {
                    case "Col_2":
                        dbsEXD_MCB1.SetValue("U_NRO_CUENTA", pVal.Row - 1, dtbl.GetValue("FormatCode", 0).ToString());
                        break;
                    case "Col_5":
                        dbsEXD_MCB1.SetValue("U_COD_DIM1", pVal.Row - 1, dtbl.GetValue("PrcCode", 0).ToString());
                        break;
                    case "Col_6":
                        dbsEXD_MCB1.SetValue("U_COD_DIM2", pVal.Row - 1, dtbl.GetValue("PrcCode", 0).ToString());
                        break;
                    case "Col_7":
                        dbsEXD_MCB1.SetValue("U_COD_DIM3", pVal.Row - 1, dtbl.GetValue("PrcCode", 0).ToString());
                        break;
                    case "Col_8":
                        dbsEXD_MCB1.SetValue("U_COD_DIM4", pVal.Row - 1, dtbl.GetValue("PrcCode", 0).ToString());
                        break;
                    case "Col_9":
                        dbsEXD_MCB1.SetValue("U_COD_DIM5", pVal.Row - 1, dtbl.GetValue("PrcCode", 0).ToString());
                        break;
                    case "Col_12":
                        dbsEXD_MCB1.SetValue("U_COD_PROYECTO", pVal.Row - 1, dtbl.GetValue("PrjCode", 0).ToString());
                        break;
                    default:
                        break;
                }
            }
            mtxCodBancs.LoadFromDataSourceEx(false);
            if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_ADD_MODE && this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_VIEW_MODE)
            {
                if (this.UIAPIRawForm.Mode != SAPbouiCOM.BoFormMode.fm_UPDATE_MODE) UIAPIRawForm.Mode = SAPbouiCOM.BoFormMode.fm_UPDATE_MODE;
            }
        }

        private void Button0_PressedBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_FIND_MODE) return;

                var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                //if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA")))
                //{
                //    throw new InvalidOperationException("Seleccione una empresa");
                //}

                if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_BANCO")))
                {
                    throw new InvalidOperationException("Seleccione un banco");
                }

                //if (string.IsNullOrWhiteSpace(dbsEXD_OMCB.GetValueExt("U_COD_CUENTA")))
                //{
                //    throw new InvalidOperationException("Seleccione una cuenta");
                //}

                //var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
                var codBanco = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
                //var codCuenta = dbsEXD_OMCB.GetValueExt("U_COD_CUENTA");

                var sqlQry = $"select COUNT('A') from \"@EXD_OMCB\" where U_COD_BANCO = '{codBanco}' ";
                recSet.DoQuery(sqlQry);

                if (this.UIAPIRawForm.Mode == SAPbouiCOM.BoFormMode.fm_ADD_MODE && Convert.ToInt32(recSet.Fields.Item(0).Value) > 0)
                {
                    throw new InvalidOperationException("Ya existe un registro para este banco ");
                }

                if (mtxCodBancs.RowCount == 0)
                {
                    throw new InvalidOperationException("Registre al menos un código de operación");
                }

                mtxCodBancs.FlushToDataSource();




                for (int i = 0; i < mtxCodBancs.VisualRowCount; i++)
                {
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.ComboBox)mtxCodBancs.GetCellSpecific("Col_14", i + 1)).Value))
                    {
                        ((SAPbouiCOM.ComboBox)mtxCodBancs.GetCellSpecific("Col_14", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre una sucursal");
                    }

                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_15", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_15", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre una cuenta de banco");
                    }

                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", i + 1)).Value))
                    {
                        //((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", i + 1)).Active = true;
                        //throw new InvalidOperationException("Ingrese un código de operación");

                        if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_1", i + 1)).Value))
                        {
                            ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", i + 1)).Active = true;
                            throw new InvalidOperationException("Ingrese una glosa");
                        }
                    }

                  
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_12", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_12", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un proyecto");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_5", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_5", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre una etapa");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_6", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_6", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre una sub etapa");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_10", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_10", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un código de partida");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_11", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_11", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un nombre de partida");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_2", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_2", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un número de cuenta");
                    }                 
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_7", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_7", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un centro de costo");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_8", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_8", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un centro de gestión");
                    }
                    if (string.IsNullOrWhiteSpace(((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_13", i + 1)).Value))
                    {
                        ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_13", i + 1)).Active = true;
                        throw new InvalidOperationException("Registre un medio de pago de transferencia");
                    }



                }

                //VALIDAR cuenta
                int rowCount = mtxCodBancs.RowCount;
                //Application.SBO_Application.SetStatusWarningMessage("inicio");
                // Iterar sobre las filas de la matriz
                for (int i = 1; i <= rowCount; i++)
                {
                    // Obtener el valor de la celda que quieres validar (supongamos que es la primera columna)
                    string currentValue1 = ((SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(i).Specific).Selected.Value;
                    string currentValue2 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_15").Cells.Item(i).Specific).Value;
                    string currentValue3 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_0").Cells.Item(i).Specific).Value;
                    string currentValue4 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_1").Cells.Item(i).Specific).Value;

                    // Realizar la validación
                    for (int j = 1; j <= rowCount; j++)
                    {
                        if (i != j) // Evitar comparar la misma fila
                        {
                            //Application.SBO_Application.SetStatusWarningMessage("j: " + j);
                            string compareValue1 = ((SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(j).Specific).Selected.Value;
                            string compareValue2 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_15").Cells.Item(j).Specific).Value;
                            string compareValue3 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_0").Cells.Item(j).Specific).Value;
                            string compareValue4 = ((SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_1").Cells.Item(j).Specific).Value;

                            if (!string.IsNullOrEmpty(compareValue3))
                            {
                                if (currentValue1 == compareValue1 && compareValue2 == currentValue2 && currentValue3 == compareValue3)
                                {
                                    // Si hay un duplicado, muestra un mensaje de error y devuelve false para evitar que se valide la matriz
                                    Application.SBO_Application.SetStatusErrorMessage("Sucursal " + currentValue1 + ", cuenta " + compareValue2 + " y código " + compareValue3 + " duplicada");
                                    throw new InvalidOperationException("No se permiten duplicados en la matriz.(código) - Revisar línea " + i + " y la línea " + j);
                                }
                            }
                          

                            if (currentValue1 == compareValue1 && compareValue2 == currentValue2 && currentValue4 == compareValue4)
                            {
                                // Si hay un duplicado, muestra un mensaje de error y devuelve false para evitar que se valide la matriz
                                Application.SBO_Application.SetStatusErrorMessage("Sucursal " + currentValue1 + ", cuenta " + compareValue2 + " y código " + compareValue4 + " duplicada");
                                throw new InvalidOperationException("No se permiten duplicados en la matriz.(glosa) - Revisar línea " + i + " y la línea " + j);
                            }
                        }
                    }
                }

                //var ultimoCodigoAñadido = ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", mtxCodBancs.VisualRowCount)).Value;
                //var dsrEXDMCB1 = XmlDSSerializer.DeserializeDBDataSource(dbsEXD_MCB1.GetAsXML());
                //var rsltExisteCodOpe = dsrEXDMCB1.Rows.Count(r => r.Cells.FirstOrDefault(c => c.Uid == "U_COD_OPE_BANC").Value == ultimoCodigoAñadido);

                //if (rsltExisteCodOpe > 1)
                //{
                //    ((SAPbouiCOM.EditText)mtxCodBancs.GetCellSpecific("Col_0", mtxCodBancs.VisualRowCount)).Active = true;
                //    throw new InvalidOperationException("Ya se ha registrado este codigo de operación");
                //}

               

                //throw new InvalidOperationException("Error pde pruebas");

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
                //Application.SBO_Application.SetStatusErrorMessage(ex.StackTrace);
                BubbleEvent = false;
            }
        }

        private void Form_DataLoadAfter(ref SAPbouiCOM.BusinessObjectInfo pVal)
        {
            //cmbEmpresas.Item.Enabled = false;
            cmbBancos.Item.Enabled = false;
            //cmbCuentas.Item.Enabled = false;
            //edtMoneda.Item.Enabled = false;
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("Form_DataLoadAfter");
                //Cargo bancos
                //var codEmp = dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
                var sqlQry = $"select distinct T0.\"BankCode\",T0.\"BankName\" from ODSC T0";
                var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                //recSet.DoQuery(sqlQry);
                //cmbBancos.LoadValidValues(recSet);

                //mtxCodBancs.Columns.Item("Col_14").DisplayDesc = true;
                
                //Application.SBO_Application.SetStatusWarningMessage("inicio");
                mtxCodBancs.Columns.Item("Col_14").DisplayDesc = true;
                if (mtxCodBancs.RowCount > 0)
                {
                    try
                    {
                        for (int i = 1; i <= mtxCodBancs.RowCount; i++)
                        {
                            //Application.SBO_Application.SetStatusWarningMessage(mtxCodBancs.RowCount.ToString());
                            SAPbouiCOM.ComboBox comboBox = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(i).Specific;

                            if (comboBox.ValidValues.Count == 0)
                            {
                                sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
                                recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                                recSet.DoQuery(sqlQry);
                                //cmbEmpresas.LoadValidValues(recSet);
                                //Application.SBO_Application.SetStatusWarningMessage(sqlQry);
                                while (!recSet.EoF)
                                {
                                    string code = recSet.Fields.Item("BPLId").Value.ToString();
                                    string name = recSet.Fields.Item("BPLName").Value.ToString();
                                    //SAPbouiCOM.ComboBox comboBox = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;
                                    comboBox.ValidValues.Add(code, name);
                                    recSet.MoveNext();
                                }
                            }
                        }

                        mtxCodBancs.Columns.Item("Col_14").DisplayDesc = true;

                        for (int i = 1; i <= mtxCodBancs.RowCount; i++)
                        {
                            //Application.SBO_Application.SetStatusWarningMessage(mtxCodBancs.RowCount.ToString());
                            SAPbouiCOM.ComboBox codEmp = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(i).Specific;

                            var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");

                            //sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp.Selected.Value}'";
                            //recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            //recSet.DoQuery(sqlQry);

                            //SAPbouiCOM.EditText comboBoxCuenta = (SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_15").Cells.Item(i).Specific;

                            //while (comboBoxCuenta.ValidValues.Count > 0)
                            //{
                            //    comboBoxCuenta.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                            //}

                            //while (!recSet.EoF)
                            //{
                            //    string code = recSet.Fields.Item("GLAccount").Value.ToString();
                            //    string name = recSet.Fields.Item("Account").Value.ToString();
                            //    comboBoxCuenta.ValidValues.Add(code, name);
                            //    recSet.MoveNext();
                            //}

                        }
                        //mtxCodBancs.Columns.Item("Col_15").DisplayDesc = true;

                    }
                    catch (Exception ex)
                    {
                        Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message+" - "+ex.InnerException);
            }
            

            // cargo cuentas
            //var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
            //sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
            //recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
            //recSet.DoQuery(sqlQry);
            //cmbCuentas.LoadValidValues(recSet);
        }

        public void EnModoBusqueda()
        {
            //cmbEmpresas.Item.Enabled = true;
            cmbBancos.Item.Enabled = true;
            //cmbCuentas.Item.Enabled = true;
            //edtMoneda.Item.Enabled = true;
        }

        private void mtxCodBancs_ClickBefore(object sboObject, SAPbouiCOM.SBOItemEventArg pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            if (pVal.Row > 0)
            {
                if (mtxCodBancs.RowCount > 0)
                {
                    try
                    {
                        SAPbouiCOM.ComboBox comboBox = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;

                        if (comboBox.ValidValues.Count == 0)
                        {
                            var sqlQry = "select \"BPLId\",\"BPLName\" from OBPL";
                            var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                            recSet.DoQuery(sqlQry);
                            //cmbEmpresas.LoadValidValues(recSet);

                            while (!recSet.EoF)
                            {
                                string code = recSet.Fields.Item("BPLId").Value.ToString();
                                string name = recSet.Fields.Item("BPLName").Value.ToString();
                                //SAPbouiCOM.ComboBox comboBox = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;
                                comboBox.ValidValues.Add(code, name);
                                recSet.MoveNext();
                            }
                        }
                        mtxCodBancs.Columns.Item("Col_14").DisplayDesc = true;

                    }
                    catch (Exception ex)
                    {
                        Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                    }
                }
            }

        }

        private void mtxCodBancs_ComboSelectAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage(pVal.ColUID);

                if (pVal.ColUID == "Col_14")
                {
                    SAPbouiCOM.ComboBox comboBoxEmpresa = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;



                    var codEmp = comboBoxEmpresa.Selected.Value;// dbsEXD_OMCB.GetValueExt("U_COD_EMPRESA");
                    var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");

                    var sqlQry = $"select \"GLAccount\",\"Account\" from DSC1 where \"BankCode\" = '{codBnc}' and \"Branch\" = '{codEmp}'";
                    //var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    //recSet.DoQuery(sqlQry);

                    //SAPbouiCOM.ComboBox comboBoxCuenta = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific;

                    //while (comboBoxCuenta.ValidValues.Count > 0)
                    //{
                    //    comboBoxCuenta.ValidValues.Remove(0, SAPbouiCOM.BoSearchKey.psk_Index);
                    //}

                    //try
                    //{

                    //}
                    //catch (Exception)
                    //{

                    //}
                    ////comboBoxCuenta.ValidValues.Add(" ", "");
               

                    //while (!recSet.EoF)
                    //{
                    //    string code = recSet.Fields.Item("GLAccount").Value.ToString();
                    //    string name = recSet.Fields.Item("Account").Value.ToString();
                    //    comboBoxCuenta.ValidValues.Add(code, name);
                    //    recSet.MoveNext();
                    //}

                    

                    

                }
                if (pVal.ColUID == "Col_15")
                {
                    //SAPbouiCOM.ComboBox comboBoxCuenta = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific;
                    //SAPbouiCOM.ComboBox comboBoxEmpresa = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;
                    //SAPbouiCOM.EditText moneda = (SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_16").Cells.Item(pVal.Row).Specific;
                

                    //var codEmp = comboBoxEmpresa.Selected.Value;
                    //var codCta = comboBoxCuenta.Selected.Value;
                    //var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
                    //var sqlQry = $"select TX1.\"ActCurr\" from DSC1 TX0 inner join OACT  TX1 on TX0.\"GLAccount\" = TX1.\"AcctCode\"  where TX0.\"BankCode\" = '{codBnc}' and TX0.\"Branch\" = '{codEmp}' and TX0.\"GLAccount\" = '{codCta}'";
                    //var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                    //recSet.DoQuery(sqlQry);
                    //if (!recSet.EoF)
                    //{
                    //    moneda.Value = recSet.Fields.Item(0).Value.ToString();
                    //}
                    //dbsEXD_OMCB.SetValueExt("U_COD_MONEDA", recSet.Fields.Item(0).Value.ToString());

                }

                for (int i = 1; i <= mtxCodBancs.RowCount; i++)
                {
                    //SAPbouiCOM.ComboBox comboBoxCuenta = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_15").Cells.Item(i).Specific;
                    //comboBoxCuenta.Item.DisplayDesc = true;
                }


            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            }

        }

        private void Form_LoadAfter(SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage("Form_LoadAfter");
                //mtxCodBancs.Columns.Item("Col_14").DisplayDesc = true;
            }
            catch (Exception ex )
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
            }

        }

        private void mtxCodBancs_ValidateAfter(object sboObject, SAPbouiCOM.SBOItemEventArg pVal)
        {
            try
            {
                //Application.SBO_Application.SetStatusWarningMessage(pVal.ColUID);


                if (pVal.ColUID == "Col_15")
                {
                    SAPbouiCOM.EditText comboBoxCuenta = (SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_15").Cells.Item(pVal.Row).Specific;
                    SAPbouiCOM.ComboBox comboBoxEmpresa = (SAPbouiCOM.ComboBox)mtxCodBancs.Columns.Item("Col_14").Cells.Item(pVal.Row).Specific;
                    SAPbouiCOM.EditText moneda = (SAPbouiCOM.EditText)mtxCodBancs.Columns.Item("Col_16").Cells.Item(pVal.Row).Specific;

                    if (!string.IsNullOrEmpty(comboBoxEmpresa.Value))
                    {
                        var codEmp = comboBoxEmpresa.Selected.Value;
                        var codCta = comboBoxCuenta.Value;
                        var codBnc = dbsEXD_OMCB.GetValueExt("U_COD_BANCO");
                        var sqlQry = $"select TX1.\"ActCurr\" from DSC1 TX0 inner join OACT  TX1 on TX0.\"GLAccount\" = TX1.\"AcctCode\"  where TX0.\"BankCode\" = '{codBnc}' and TX0.\"Branch\" = '{codEmp}' and TX0.\"GLAccount\" = '{codCta}'";
                        var recSet = (SAPbobsCOM.Recordset)this.GetCompany().GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);
                        recSet.DoQuery(sqlQry);
                        if (!recSet.EoF)
                        {
                            moneda.Value = recSet.Fields.Item(0).Value.ToString();
                            mtxCodBancs.FlushToDataSource();
                            //dbsEXD_MCB1.SetValueExt("U_COD_MONEDA", recSet.Fields.Item(0).Value.ToString());

                        }
                    }
                   

                }

              


            }
            catch (Exception ex)
            {
                Application.SBO_Application.SetStatusErrorMessage(ex.Message);
                
            }

        }
    }
}