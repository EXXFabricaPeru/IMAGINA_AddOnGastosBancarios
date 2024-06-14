using EXX_IMG_GastosBancarios.Presentation.Forms;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace EXX_IMG_GastosBancarios.Presentation
{
    class Menu
    {
        private FormMaestroCodigosBancarios formMstrCodBanc = null;
        private FormConciliacionBancaria formConciBanc = null;

        public void AddMenuItems()
        {
            SAPbouiCOM.Menus oMenus = null;
            SAPbouiCOM.MenuItem oMenuItem = null;

            oMenus = Application.SBO_Application.Menus;

            SAPbouiCOM.MenuCreationParams oCreationPackage = null;
            oCreationPackage = ((SAPbouiCOM.MenuCreationParams)(Application.SBO_Application.CreateObject(SAPbouiCOM.BoCreatableObjectType.cot_MenuCreationParams)));

            Application.SBO_Application.StatusBar.SetText("GB:Cargando opciones de menú", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Warning);
            Application.SBO_Application.Forms.GetForm("169", 1)?.Freeze(true);

            oMenuItem = Application.SBO_Application.Menus.Item("11008"); // moudles'

            try
            {
                // Get the menu collection of the newly added pop-up item
                oMenus = oMenuItem.SubMenus;
                if (!oMenuItem.SubMenus.Exists("GBMenu1"))
                {
                    // Create s sub menu
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "GBMenu1";
                    oCreationPackage.String = "Maestro de códigos bancarios";
                    oCreationPackage.Position = 6;
                    oMenus.AddEx(oCreationPackage);
                }

                if (!oMenuItem.SubMenus.Exists("GBMenu2"))
                {
                    // Create s sub menu
                    oCreationPackage.Type = SAPbouiCOM.BoMenuType.mt_STRING;
                    oCreationPackage.UniqueID = "GBMenu2";
                    oCreationPackage.String = "Conciliación bancaria";
                    oCreationPackage.Position = 7;
                    oMenus.AddEx(oCreationPackage);
                }

                Application.SBO_Application.StatusBar.SetText("Add-on de gastos bancarios cargado correctamente", SAPbouiCOM.BoMessageTime.bmt_Short, SAPbouiCOM.BoStatusBarMessageType.smt_Success);
            }
            catch (Exception er)
            { //  Menu already exists
                Application.SBO_Application.SetStatusBarMessage("Menu Already Exists", SAPbouiCOM.BoMessageTime.bmt_Short, true);
            }
            finally
            {
                Application.SBO_Application.Forms.GetForm("169", 1)?.Freeze(false);
                Application.SBO_Application.Forms.GetForm("169", 1)?.Update();
            }
        }

        public void SBO_Application_MenuEvent(ref SAPbouiCOM.MenuEvent pVal, out bool BubbleEvent)
        {
            BubbleEvent = true;

            try
            {
                if (pVal.BeforeAction && pVal.MenuUID == "GBMenu1")
                {
                    formMstrCodBanc = new FormMaestroCodigosBancarios();
                    formMstrCodBanc.Show();
                }
                else if (!pVal.BeforeAction && pVal.MenuUID == "1292")
                {
                    var formAux = Application.SBO_Application.Forms.ActiveForm;
                    if (formAux.TypeEx == "FormMaestroCodigosBancarios")
                        formMstrCodBanc.AddNewLineMatrix();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "1293")
                {
                    var formAux = Application.SBO_Application.Forms.ActiveForm;
                    if (formAux.TypeEx == "FormMaestroCodigosBancarios")
                    {
                        formMstrCodBanc.DeleLineMatrix();
                        BubbleEvent = false;
                    }
                }
                else if (!pVal.BeforeAction && pVal.MenuUID == "1282")
                {
                    var formAux = Application.SBO_Application.Forms.ActiveForm;
                    if (formAux.TypeEx == "FormConciliacionBancaria")
                        formConciBanc.FormDataLoadAdd();
                    else if (formAux.TypeEx == "FormMaestroCodigosBancarios")
                        formMstrCodBanc.FormDataLoadAdd();
                }
                else if (!pVal.BeforeAction && pVal.MenuUID == "1281")
                {
                    var formAux = Application.SBO_Application.Forms.ActiveForm;
                    if (formAux.TypeEx == "FormMaestroCodigosBancarios")
                        formMstrCodBanc.EnModoBusqueda();
                }
                else if (pVal.BeforeAction && pVal.MenuUID == "GBMenu2")
                {
                    //var formAux = Application.SBO_Application.Forms.ActiveForm;
                    try
                    {
                        if (!FormConciliacionBancaria.isOpen)
                        {
                            formConciBanc = new FormConciliacionBancaria();
                            formConciBanc.Show();
                        }
                    }
                    catch (Exception)
                    {
                        formConciBanc = new FormConciliacionBancaria();
                        formConciBanc.Show();
                    }
                    
                        
                    

                }
            }
            catch (Exception ex)
            {
                Application.SBO_Application.MessageBox(ex.ToString(), 1, "Ok", "", "");
            }
        }
    }
}
