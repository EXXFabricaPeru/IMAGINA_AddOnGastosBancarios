using EXX_IMG_GastosBancarios.Domain.Entities;
using JF_SBOAddon.Utiles.Extensions;
using Newtonsoft.Json.Linq;
using RestSharp;
using SAPbouiCOM.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace EXX_IMG_GastosBancarios.Presentation.Helper
{
    public class ServiceLayerHelper
    {
        public static string sConnectionContext = null;
        public static string serviceLayerAddress = null;
        public static SLLogin SLLoginResponse;

        public static void Connect()
        {
            try
            {
                //string server = DIExtensions.Company.Server.Replace("NDB@", "").Substring(0, Globals.oCompany.Server.Replace("NDB@", "").IndexOf(":")).Trim();
                string serviceLayerAddressAux = "https://172.19.163.3:50000/b1s/v1";
                string sConnectionContextAux = null;

                try
                {
                    sConnectionContextAux = Application.SBO_Application.Company.GetServiceLayerConnectionContext(serviceLayerAddressAux);
                }
                catch (System.Exception ex)
                {
                }

                if (sConnectionContextAux == null)
                    throw new Exception("No se logró establecer conexión con Service Layer");

                sConnectionContext = sConnectionContextAux;
                serviceLayerAddress = serviceLayerAddressAux;
                SLLoginResponse = new SLLogin();
                SLLoginResponse.B1SESSION = sConnectionContext.Split(';')[0].Replace("B1SESSION=", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static IRestResponse PostSL(string url, string body)
        {
        band:
            try
            {
                if (serviceLayerAddress == null)
                    Connect();


                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                var client = new RestClient(url);


                client.Timeout = -1;
                client.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");

                if (!string.IsNullOrEmpty(body))
                    request.AddParameter("application/json", body, ParameterType.RequestBody);
                request.AddCookie("B1SESSION", SLLoginResponse.B1SESSION);
                IRestResponse response = client.Execute(request);


                return response;
            }
            catch (Exception ex)
            {
                if (ex.Message == "No se logró establecer conexión con Service Layer")
                    throw ex;

                if (ex.Message.Contains("Invalid session"))
                {
                    serviceLayerAddress = null;
                    goto band;
                }

                dynamic errorMsj = JObject.Parse(ex.Message.Replace("'", ""));
                throw new Exception(errorMsj.error.message.value);
            }

        }


        public static IRestResponse GetSL(string url)
        {
            if (serviceLayerAddress == null)
                Connect();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient(url);
            client.Timeout = -1;
            client.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Prefer", "odata.maxpagesize=500");
            request.AddCookie("B1SESSION", SLLoginResponse.B1SESSION);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

            return response;
        }




        public static IRestResponse PatchSL(string url, string body)
        {

            if (serviceLayerAddress == null) Connect();

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            var client = new RestClient(url);


            client.Timeout = -1;
            client.RemoteCertificateValidationCallback += (sender, certificate, chain, errors) => true;
            var request = new RestRequest(Method.PATCH);
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            request.AddCookie("B1SESSION", SLLoginResponse.B1SESSION);

            IRestResponse response = client.Execute(request);

            Console.WriteLine(response.Content);
            return response;
        }
    }
}
