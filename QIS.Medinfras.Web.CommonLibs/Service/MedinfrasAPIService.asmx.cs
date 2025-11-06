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
using System.Security.Authentication;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for MedinfrasAPIService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MedinfrasAPIService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";
		private string url = string.Empty;
        private string url_MedinfrasAPI = string.Empty;
        private string consID = string.Empty;
        private string consPassword = string.Empty;
        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        private void GetSettingParameter(bool isApi)
        {
            List<SettingParameterDt> lstDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.BPJS_SEP_WS_URL,
                Constant.SettingParameter.BPJS_CONSUMER_ID,
                Constant.SettingParameter.BPJS_CONSUMER_PASSWORD,
                Constant.SettingParameter.SA0130,
                Constant.SettingParameter.SA0131,
                Constant.SettingParameter.SA0132));

            if (isApi)
            {
                url_MedinfrasAPI = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0132).FirstOrDefault().ParameterValue;
                consID = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0130).FirstOrDefault().ParameterValue;
                consPassword = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0131).FirstOrDefault().ParameterValue;
            }
            else
            {
                url = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.BPJS_SEP_WS_URL).FirstOrDefault().ParameterValue;
                consID = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.BPJS_CONSUMER_ID).FirstOrDefault().ParameterValue;
                consPassword = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.BPJS_CONSUMER_PASSWORD).FirstOrDefault().ParameterValue;
            }
        }

        #region MEDINFRAS-API : Centerback
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetLaboratoryResultReportFromLIS(string healthcareID, string transactionNo, string userName)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                string url = AppSession.SA0132;

                if (!string.IsNullOrEmpty(healthcareID) && !string.IsNullOrEmpty(transactionNo) && !string.IsNullOrEmpty(userName))
                {
                    string param = string.Format("{0}|{1}|{2}", healthcareID, transactionNo.Replace("/", "_"), userName);
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/laboratory/result/{1}", url, param));
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0058, AppSession.SA0059);

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
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Informasi NIK tidak boleh kosong");
                }
                return result;
            }
            catch (WebException ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Message.ToString(), string.Empty));
                return result;
            }
        }
		#region Print Template DocumentPrint
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetTemplateDocumentPrint(String ReportCode, String ParamValue)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/utility/created_document", this.url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                ReportDocumentParam param = new ReportDocumentParam();
                param.ReportCode = ReportCode;
                param.ParamValue = ParamValue;
                entityAPILog.MessageText = JsonConvert.SerializeObject(param);
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
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", "PROTOCOL_ERROR");
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", "CONNECTION_CLOSED");
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", "TIMEOUT");
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        #endregion

        #endregion

        #region GenerateReferralLetter
        /// <summary>
        /// MCU Pivot Get Batch No
        /// </summary>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GenerateReferralLetter(int id)
        {
            string result = "";
            var json = string.Empty;

            try
            {

                string filterexpression = string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}', '{2}', '{3}')",
                    "001",
                    Constant.SettingParameter.SA0132,
                    Constant.SettingParameter.SA0130,
                    Constant.SettingParameter.SA0131
                    );
                List<SettingParameterDt> lstData = BusinessLayer.GetSettingParameterDtList(filterexpression).ToList();
                string SA0132 = lstData.Where(p => p.ParameterCode == Constant.SettingParameter.SA0132).FirstOrDefault().ParameterValue;
                string CONSUMER_CONS_ID = lstData.Where(p => p.ParameterCode == Constant.SettingParameter.SA0130).FirstOrDefault().ParameterValue;
                string CONSUMER_PASS_ID = lstData.Where(p => p.ParameterCode == Constant.SettingParameter.SA0131).FirstOrDefault().ParameterValue;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/utility/generatereferralletterDoc/{1}", SA0132, id));
                request.Method = "POST";
                request.ContentType = "application/json";
                Methods.SetRequestHeaderWithHealthcareID(request, "001", CONSUMER_CONS_ID, CONSUMER_PASS_ID);

                string data = JsonConvert.SerializeObject(Convert.ToInt32(id));

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();

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
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                }


                return result;
            }
            catch (WebException ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Message.ToString(), string.Empty));
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
		private void SetRequestHeaderMedinfrasAPI(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}{1}", unixTimestamp, consID), consPassword));
        }
    }
}
