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
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for DrugAlertService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DrugAlertService : System.Web.Services.WebService
    {
        private string url_MedinfrasAPI = AppSession.SA0132;
        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDrugAlertValidation(String prescriptionOrderNo, String checkType)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string result = "";
            var json = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/drugalert/ValidateDrugs", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);

                DrugAlertRequestData param = new DrugAlertRequestData();
                param.PrescriptionOrderNo = prescriptionOrderNo;
                param.CheckType = checkType;

                string jsonReq = JsonConvert.SerializeObject(param);

                byte[] bytes = Encoding.UTF8.GetBytes(jsonReq);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    MedinfrasAPIResponse2 resp = JsonConvert.DeserializeObject<MedinfrasAPIResponse2>(result);
                    if (resp.Status == "SUCCESS")
                    {
                        result = string.Format("1|{0}|{1}", resp.Data, resp.Remarks);
                        entityAPILog.Response = result;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", null, resp.Remarks);
                        entityAPILog.Response = result;
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, json);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, json);
                        break;
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Message.ToString(), string.Empty));
                return result;
            }
        }


        private void SetMedinfrasAPIRequestHeader(HttpWebRequest Request, string consID, string passkey)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string data = unixTimestamp.ToString() + consID;
            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(data, passkey));
        }

        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            SettingParameterDt entityConsID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA0130);
            SettingParameterDt entityPassID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA0131);

            string consID = entityConsID.ParameterValue;
            string pass = entityPassID.ParameterValue;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        private void SetRequestHeaderMedinfrasAPI(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = AppSession.SA0130;
            string pass = AppSession.SA0131;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}{1}", unixTimestamp, consID), pass));
        }

        private string GenerateSignature(string data, string secretKey)
        {
            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);
            return encodedSignature;
        }

        public class DrugAlertRequestData
        {
            public string PrescriptionOrderNo { get; set; }
            public string CheckType { get; set; }
        }
    }
}
