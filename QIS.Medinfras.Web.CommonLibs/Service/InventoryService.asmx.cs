using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for QueueService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class InventoryService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";

        #region MEDINFRAS-API : Centerback
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendInventoryNotification(string healthcareID, string type, int transactionID, string transactionNo)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;
                #region Convert into DTO Objects
                InventoryNotificationDTO oData = new InventoryNotificationDTO();
                oData.HealthcareID = healthcareID;
                oData.MessageType = type;
                oData.TransactionID = transactionID.ToString();
                oData.TransactionReferenceID = transactionID.ToString();
                oData.TransactionNo = transactionNo;
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inventory/purchase/request/send", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    SetMedinfrasAPIRequestHeader(request, AppSession.SA0130, AppSession.SA0131);

                    var json = JsonConvert.SerializeObject(oData);
                    using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                    }

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remarks, json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "Method not found", string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }
        #endregion
     
        private void SetMedinfrasAPIRequestHeader(HttpWebRequest Request, string consID, string passkey)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string data = unixTimestamp.ToString() + consID;
            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(data, passkey));
        }

        private void SetRequestHeader(HttpWebRequest Request, string consID, string passkey)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}&{1}", consID, unixTimestamp), passkey));
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

        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            SettingParameterDt entityConsID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_CONS_ID);
            SettingParameterDt entityPassID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_PASS_ID);

            string consID = entityConsID.ParameterValue;
            string pass = entityPassID.ParameterValue;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }
    }
}
