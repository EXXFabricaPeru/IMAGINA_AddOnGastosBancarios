using EXX_IMG_GastosBancarios.Presentation.Helper;
using EXX_Metadata.BL;
using JF_SBOAddon.Utiles.Extensions;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace EXX_IMG_GastosBancarios.Presentation
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                Application oApp = null;
                if (args.Length < 1)
                {
                    oApp = new Application();
                }
                else
                {
                    //If you want to use an add-on identifier for the development license, you can specify an add-on identifier string as the second parameter.
                    //oApp = new Application(args[0], "XXXXX");
                    oApp = new Application(args[0]);
                }

                DIExtensions.Company = (SAPbobsCOM.Company)Application.SBO_Application.Company.GetDICompany();
                MDResources.Messages = mostrarMensajes;
                if (MDResources.loadMetaData(Assembly.GetExecutingAssembly().GetName().Version, Application.SBO_Application, "EXX", "CONBANC"))
                {
                    SearchFormatedHelper.Initialize(DIExtensions.Company, Assembly.GetExecutingAssembly().GetName().Version,"EXX", "CONBANC");
                    Menu MyMenu = new Menu();
                    MyMenu.AddMenuItems();
                    oApp.RegisterMenuEventHandler(MyMenu.SBO_Application_MenuEvent);
                    Application.SBO_Application.AppEvent += new SAPbouiCOM._IApplicationEvents_AppEventEventHandler(SBO_Application_AppEvent);
                    oApp.Run();
                }
                else
                {

                    System.Windows.Forms.Application.Exit();
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        static void SBO_Application_AppEvent(SAPbouiCOM.BoAppEventTypes EventType)
        {
            switch (EventType)
            {
                case SAPbouiCOM.BoAppEventTypes.aet_ShutDown:
                    //Exit Add-On
                    System.Windows.Forms.Application.Exit();
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_CompanyChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_FontChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_LanguageChanged:
                    break;
                case SAPbouiCOM.BoAppEventTypes.aet_ServerTerminition:
                    break;
                default:
                    break;
            }
        }

        static void mostrarMensajes(string m, MessageType t)
        {
            switch (t)
            {
                case MessageType.Info:
                    Application.SBO_Application.SetStatusWarningMessage(m);
                    break;
                case MessageType.Success:
                    Application.SBO_Application.SetStatusSuccessMessage(m);
                    break;
                case MessageType.Error:
                    Application.SBO_Application.SetStatusErrorMessage(m);
                    break;
                default:
                    break;
            }
        }
    }
}
