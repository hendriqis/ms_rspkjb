using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
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
    public class HL7Service : System.Web.Services.WebService
    {
        private string url = AppSession.RIS_WEB_API_URL;

        #region Get Request Token FujiFilm
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRequestTokenFujifilm(string accessionNumber, string urlViewer, ImagingResultDt dt)
        {
            try
            {
                string result = "";
                GetTokenFujifilm oData = new GetTokenFujifilm();
                if (!string.IsNullOrEmpty(accessionNumber))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}{1}", url, accessionNumber));
                    request.Method = "GET";

                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);

                    using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                    {
                        result = sr.ReadToEnd();
                        List<GetTokenFujifilm> respInfo = JsonConvert.DeserializeObject<List<GetTokenFujifilm>>(result);
                        if (respInfo.FirstOrDefault().noro == accessionNumber)
                        {
                            IDbContext ctx = DbFactory.Configure(true);
                            ImagingResultDtDao entityResultDtDao = new ImagingResultDtDao(ctx);
                            //ImagingResultDt entityResultDt = BusinessLayer.GetImagingResultDtList(string.Format("ReferenceNo = {0}", accessionNumber)).FirstOrDefault();
                            try
                            {
                                if (dt != null)
                                {
                                    dt.ImageViewerLinkUrl = string.Format("{0}{1}", urlViewer, respInfo.FirstOrDefault().token);
                                    dt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    dt.LastUpdatedDate = DateTime.Now;
                                    entityResultDtDao.Update(dt);
                                    ctx.CommitTransaction();
                                    result = string.Format("{0}|{1}|{2}", "1", result, respInfo.FirstOrDefault().token);
                                }
                                else
                                {
                                    result = string.Format("{0}|{1}|Accession Number Tidak Ditemukan", "0", result);
                                    ctx.RollBackTransaction();
                                }
                            }
                            catch
                            {
                                ctx.RollBackTransaction();
                            }
                            finally
                            {
                                ctx.Close();
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}|{2}", "1", "null", respInfo);
                        }
                    }
                    return result;

                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Centerback
       
        #region Send HL7 
        #region Send Vital Sign
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendVitalSignMessage(string JsonRequest)
        {
            try
            {
                string result = "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/hl7/VitalSign/send/message", AppSession.SA0132));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);



                string data = JsonRequest; // JsonConvert.SerializeObject(JsonRequest);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    //MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(result);

                    //if (apiResponse.Status == "SUCCESS")
                    //{
                    //    result = string.Format("{0}|{1}|{2}", "1", apiResponse.Data, data);
                    //}
                    //else
                    //{
                    //    result = string.Format("{0}|{1}|{2}", "0", apiResponse.Remarks, data);
                    //}
                }
                return result;
            }
            catch (Exception ex)
            {
                return string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
            }
        }
        #endregion 
        #endregion
        #endregion

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

        private string GenerateSignature(string data, string secretKey)
        {
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            var encodedSignature = Convert.ToBase64String(signature);

            return encodedSignature;
        }
    }

    

    public class SendVitalSignData
    {
        public string RegistrationNo { get; set; }
        public string HL7MessageType { get; set; }
    }
}
