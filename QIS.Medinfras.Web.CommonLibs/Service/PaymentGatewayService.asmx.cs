using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for BPJSService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class PaymentGatewayService : System.Web.Services.WebService
    {

        private string Medinfras_Centerback_URL = AppSession.SA0132;
        private string Medinfras_Centerback_consID = AppSession.SA0130;
        private string Medinfras_Centerback_Password = AppSession.SA0131;
        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        #region DOKU VirtualAccount
        #region 1. BCA
        /// <summary>
        /// DOKU Payment Gateway
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DokuBcaVa(DokuHISRequestVa dataRequest)
        {
             
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
            //String invoiceNo = InvoiceNo.Replace("/", "_").Replace(" ","");
           
            //DokuHISVa datadt = new DokuHISVa()
            //{
            //   HealthcareID = AppSession.UserLogin.HealthcareID,
            //   BillingAmount = TotalAmount,
            //   BillingNo = invoiceNo,
            //   PatientEmail = PatienEmail,
            //   PatientName = PatientName

            //};
            //string DataDtRequest = JsonConvert.SerializeObject(datadt);

            //DokuHISRequestVa dataRequest = new DokuHISRequestVa()
            //{
            //    Chasier = GCChasier,
            //    Shift = GCShift,
            //    Channel = Channel,
            //    JsonRequest = DataDtRequest
            //};

            string data = JsonConvert.SerializeObject(dataRequest);
            entityAPILog.MessageText = data;
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/payment_gateway/direct", Medinfras_Centerback_URL));
                request.Method = "POST";
                request.ContentType = "application/json";
                Methods.SetRequestHeader(request, Medinfras_Centerback_consID, Medinfras_Centerback_Password);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
               
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = sr.ReadToEnd();
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    ApiResponse respInfo = JsonConvert.DeserializeObject<ApiResponse>(result);
                    result = string.Format("{0}|{1}|{2}|{3}",'1', respInfo.Status, respInfo.Remarks, respInfo.Data);
                }
                
                return result;
            }
            catch (WebException ex)
            {
                result = string.Format("{0}|{1}|{2}^{3}", "0", "", ex.Message, ex.ToString());
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;

            }
        }
        #endregion

        /// <summary>
        /// DOKU Cek Status Payment Gateway
        /// </summary>
        /// 
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DokuCheckStatusPayment(string data)
        {
            
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            //String invoiceNo = InvoiceNo.Replace("/", "_").Replace(" ", "");
            //DokuHISRequestVa dataRequest = new DokuHISRequestVa()
            //{
            //    BillingNo = invoiceNo,
            //};

         ///   string data = JsonConvert.SerializeObject(dataRequest);
            entityAPILog.MessageText = data;
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/payment_gateway/status", Medinfras_Centerback_URL));
                request.Method = "POST";
                request.ContentType = "application/json";
                Methods.SetRequestHeader(request, Medinfras_Centerback_consID, Medinfras_Centerback_Password);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
              
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    ApiResponse respInfo = JsonConvert.DeserializeObject<ApiResponse>(result);
                    result = string.Format("{0}|{1}|{2}|{3}", '1', respInfo.Status, respInfo.Remarks, respInfo.Data);
                }

                return result;
            }
            catch (WebException ex)
            {
                result =  string.Format("{0}|{1}|{2}^{3}", "0", "", ex.Message, ex.ToString());
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        
         /// <summary>
        /// List Referensi Diagnosa 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DokuCheckPaymentStatus(string BillingNo, string CashierGroup, string CashierShift, string PaymentMethod, string BankCode)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
             
            DokuHISRequestVa dataRequest = new DokuHISRequestVa()
            {
                BillingNo = BillingNo, //invoiceNo,
                CashierGroup = CashierGroup,
                CashierShift = CashierShift,
                PaymentMethod = PaymentMethod,
                BankCode = BankCode,
            };
            string data = JsonConvert.SerializeObject(dataRequest);
            entityAPILog.MessageText = data;
            string result = "";
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/payment_gateway/status", Medinfras_Centerback_URL));
                request.Method = "POST";
                request.ContentType = "application/json";
                Methods.SetRequestHeader(request, Medinfras_Centerback_consID, Medinfras_Centerback_Password);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                 
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    ApiResponse respInfo = JsonConvert.DeserializeObject<ApiResponse>(result);
                    result = string.Format("{0}|{1}|{2}|{3}", '1', respInfo.Status, respInfo.Remarks, respInfo.Data);
                }

                return result;
            }
            catch (WebException ex)
            {
                result = string.Format("{0}|{1}|{2}^{3}", "0", "", ex.Message, ex.ToString());
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        
        #endregion

    }
}
