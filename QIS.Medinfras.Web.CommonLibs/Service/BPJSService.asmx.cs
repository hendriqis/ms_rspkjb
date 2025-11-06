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
    /// Summary description for BPJSService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class BPJSService : System.Web.Services.WebService
    {
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

        #region Referensi BPJS
        #region 01. Diagnosa

        #region Diagnosa
        /// <summary>
        /// List Referensi Diagnosa 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSDiagnosaList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
            
            string data = JsonConvert.SerializeObject(string.Format("Referensi Diagnosa = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/diagnosa/{1}", url, keyword));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceDiagnoseResponse respInfo = JsonConvert.DeserializeObject<ReferenceDiagnoseResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceDiagnoseResponse oResult = new ReferenceDiagnoseResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%' OR BPJSName LIKE '%{2}%')", Constant.BPJSObjectType.BPJS_REFERENCE_DIAGNOSA, keyword, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_DIAGNOSA, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.diagnosa.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_DIAGNOSA;
                                entityInsert.BPJSCode = respInfo.response.diagnosa[i].kode;
                                entityInsert.BPJSName = respInfo.response.diagnosa[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region DiagnosaAPI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSDiagnosaList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Diagnosa = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "DIAGNOSA";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 02. Poli

        #region Poli
        /// <summary>
        /// List Referensi Poli
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSPoliList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Poli = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/poli/{1}", url, keyword));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferencePoliResponse respInfo = JsonConvert.DeserializeObject<ReferencePoliResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferencePoliResponse oResult = new ReferencePoliResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%' OR BPJSName LIKE '%{2}%')", Constant.BPJSObjectType.BPJS_REFERENCE_POLI, keyword, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_POLI, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.poli.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_POLI;
                                entityInsert.BPJSCode = respInfo.response.poli[i].kode;
                                entityInsert.BPJSName = respInfo.response.poli[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region PoliAPI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSPoliList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Poli = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "POLI";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 03. FasilitasKesehatan

        #region FasilitasKesehatan
        /// <summary>
        /// List Referensi FasilitasKesehtan
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSFasilitasKesehatanList(String keyword1, String keyword2)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi NamaFaskes = '{0}', JenisFaskes = '{1}'", keyword1, keyword2));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/faskes/{1}/{2}", url, keyword1, keyword2));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceFasilitasKesehatanResponse respInfo = JsonConvert.DeserializeObject<ReferenceFasilitasKesehatanResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceFasilitasKesehatanResponse oResult = new ReferenceFasilitasKesehatanResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '{1}' OR BPJSName LIKE '{2}') OR (BPJSName LIKE '{1}' OR BPJSName LIKE '{2}')", Constant.BPJSObjectType.BPJS_REFERENCE_FASILITAS_KESEHATAN, keyword1, keyword2), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_FASILITAS_KESEHATAN, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.faskes.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_FASILITAS_KESEHATAN;
                                entityInsert.BPJSCode = respInfo.response.faskes[i].kode;
                                entityInsert.BPJSName = respInfo.response.faskes[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region FasilitasKesehatanAPI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSFasilitasKesehatanList_MEDINFRASAPI(String keyword1, String keyword2)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi NamaFaskes = '{0}', JenisFaskes = '{1}'", keyword1, keyword2));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "FASKES";
                param.Parameter = keyword1 + "|" + keyword2 + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 04. Kabupaten

        #region Kabupaten
        /// <summary>
        /// List Referensi Kabupaten
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSKabupatenList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Kabupaten = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/kabupaten/propinsi/{1}", url, keyword));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceKabupatenResponse respInfo = JsonConvert.DeserializeObject<ReferenceKabupatenResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceKabupatenResponse oResult = new ReferenceKabupatenResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%')", Constant.BPJSObjectType.BPJS_REFERENCE_KABUPATEN, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_KABUPATEN, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_KABUPATEN;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Kabupaten API
        /// <summary>
        /// List Referensi Kabupaten API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSKabupatenList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Kabupaten = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "KABUPATEN";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 05. Kecamatan

        #region Kecamatan
        /// <summary>
        /// List Referensi Kecamatan 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSKecamatanList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Kecamatan = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/kecamatan/kabupaten/{1}", url, keyword));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceKecamatanResponse respInfo = JsonConvert.DeserializeObject<ReferenceKecamatanResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceKecamatanResponse oResult = new ReferenceKecamatanResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%' OR BPJSName LIKE '%{2}%')", Constant.BPJSObjectType.BPJS_REFERENCE_KECAMATAN, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_KECAMATAN, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_KECAMATAN;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region KecamatanAPI
        /// <summary>
        /// List Referensi Kecamatan API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSKecamatanList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi kecamatan = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "KECAMATAN";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 06. Propinsi

        #region Propinsi
        /// <summary>
        /// List Referensi Propinsi 
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSPropinsiList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Propinsi = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/propinsi", url));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferencePropinsiResponse respInfo = JsonConvert.DeserializeObject<ReferencePropinsiResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferencePropinsiResponse oResult = new ReferencePropinsiResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_PROPINSI), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_PROPINSI, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_PROPINSI;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region PropinsiAPI
        /// <summary>
        /// List Referensi Propinsi API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSPropinsiList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Propinsi = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "PROPINSI";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 07. DokterDPJP

        #region DokterDPJP
        /// <summary>
        /// List Referensi DokterBPJS
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSDokterDPJPList(String keyword1, String keyword2, String keyword3)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi JenisPelayanan = '{0}', Tanggal='{1}',KodeSpesialis='{2}' ", keyword1, keyword2, keyword3));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/dokter/pelayanan/{1}/tglPelayanan/{2}/Spesialis/{3}", url, keyword1, keyword2, keyword3));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceDokterDPJPResponse respInfo = JsonConvert.DeserializeObject<ReferenceDokterDPJPResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceDokterDPJPResponse oResult = new ReferenceDokterDPJPResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format(
                                                                                    "GCBPJSObjectType = '{0}' AND BPJSCode = '{1}'",
                                                                                    Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER_DPJP,
                                                                                    respInfo.response.list[i].kode), ctx);
                                if (lstReference != null)
                                {
                                    foreach (BPJSReference entity in lstReference)
                                    {
                                        entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER_DPJP, entity.BPJSCode);
                                    }
                                }

                                BPJSReference entityInsert = new BPJSReference();
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER_DPJP;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region DokterDPJPAPI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetReferensiBPJSDokterDPJPList_MEDINFRASAPI(String keyword1, String keyword2, String keyword3)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi JenisPelayanan = '{0}', Tanggal='{1}',KodeSpesialis='{2}' ", keyword1, keyword2, keyword3));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "DOKTER_DPJP";
                param.Parameter = keyword1 + "|" + keyword2 + "|" + keyword3 + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 08. Procedure

        #region Procedure
        /// <summary>
        /// List Referensi Procedure
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProcedureList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Procedure = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/procedure/{1}", url, keyword));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceProcedureResponse respInfo = JsonConvert.DeserializeObject<ReferenceProcedureResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceProcedureResponse oResult = new ReferenceProcedureResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%' OR BPJSName LIKE '%{2}%')", Constant.BPJSObjectType.BPJS_REFERENCE_PROCEDURE, keyword, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_PROCEDURE, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.procedure.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_PROCEDURE;
                                entityInsert.BPJSCode = respInfo.response.procedure[i].kode;
                                entityInsert.BPJSName = respInfo.response.procedure[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region ProcedureAPI
        /// <summary>
        /// List Referensi Procedure API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetProcedureList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Procedure = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "PROCEDURE";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 09. Kelas Rawat

        #region Kelas Rawat
        /// <summary>
        /// List Referensi Procedure
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetClassCareList(String keyword)
        public object GetClassCareList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Kelas Rawat = {0}", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                //HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/kelasrawat", url, keyword));
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/kelasrawat", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceClassCareResponse respInfo = JsonConvert.DeserializeObject<ReferenceClassCareResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceClassCareResponse oResult = new ReferenceClassCareResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_KELASRAWAT), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_KELASRAWAT, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_KELASRAWAT;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Kelas RawatAPI
        /// <summary>
        /// List Referensi Procedure
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetClassCareList(String keyword)
        public object GetClassCareList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Kelas Rawat = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "KELAS_RAWAT";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 10. Dokter

        #region Dokter
        /// <summary>
        /// List Referensi Procedure
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetParamedicList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Dokter = '{0}'", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/dokter/{1}", url, keyword));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceParamedicResponse respInfo = JsonConvert.DeserializeObject<ReferenceParamedicResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceParamedicResponse oResult = new ReferenceParamedicResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}' AND (BPJSCode LIKE '%{1}%' OR BPJSName LIKE '%{2}%')", Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER, keyword, keyword), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_DOKTER;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region DokterAPI
        /// <summary>
        /// List Referensi Dokter
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetParamedicList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Dokter = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "DOKTER";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 11. Spesialistik

        #region Spesialistik
        /// <summary>
        /// List Referensi Spesialistik
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSpecialityList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Spesialistik = {0}", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/spesialistik", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceSpecialityResponse respInfo = JsonConvert.DeserializeObject<ReferenceSpecialityResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceSpecialityResponse oResult = new ReferenceSpecialityResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_SPESIALISTIK), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_SPESIALISTIK, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_SPESIALISTIK;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region SpesialistikAPI
        /// <summary>
        /// List Referensi Spesialistik API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSpecialityList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Spesialistik = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "SPESIALISASI";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 12. Ruang Rawat

        #region Ruang Rawat
        /// <summary>
        /// List Referensi Ruang Rawat
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetClassCareList(String keyword)
        public object GetRuangRawatList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Ruang Rawat = {0}", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/ruangrawat", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceRuangRawatResponse respInfo = JsonConvert.DeserializeObject<ReferenceRuangRawatResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceRuangRawatResponse oResult = new ReferenceRuangRawatResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_RUANGRAWAT), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_RUANGRAWAT, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_RUANGRAWAT;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Ruang RawatAPI
        /// <summary>
        /// List Referensi Ruang Rawat
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetClassCareList(String keyword)
        public object GetRuangRawatList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Ruang Rawat = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RUANG_RAWAT";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 13. Cara Keluar

        #region Cara Keluar
        /// <summary>
        /// List Referensi Cara Keluar
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public object GetClassCareList(String keyword)
        public object GetCaraKeluarList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Cara Keluar = {0}", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/carakeluar", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferenceSpecialityResponse respInfo = JsonConvert.DeserializeObject<ReferenceSpecialityResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferenceSpecialityResponse oResult = new ReferenceSpecialityResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_CARAKELUAR), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_CARAKELUAR, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_CARAKELUAR;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Cara Keluar API
        /// <summary>
        /// List Referensi Cara Keluar API
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetCaraKeluarList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Cara Keluar = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "CARA_KELUAR";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region 14. Pasca Pulang

        #region Pasca Pulang
        /// <summary>
        /// List Referensi Pasca Pulang
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPascaPulangList(String keyword)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Pasca Pulang = {0}", keyword));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/referensi/pascapulang", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    ReferencePascaPulangResponse respInfo = JsonConvert.DeserializeObject<ReferencePascaPulangResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        ReferencePascaPulangResponse oResult = new ReferencePascaPulangResponse();

                        oResult.metadata = new BPJSReferenceModel() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        IDbContext ctx = DbFactory.Configure(true);
                        BPJSReferenceDao entityDao = new BPJSReferenceDao(ctx);

                        try
                        {
                            List<BPJSReference> lstReference = BusinessLayer.GetBPJSReferenceList(string.Format("GCBPJSObjectType = '{0}'", Constant.BPJSObjectType.BPJS_REFERENCE_PASCAPULANG), ctx);
                            if (lstReference != null)
                            {
                                foreach (BPJSReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.BPJSObjectType.BPJS_REFERENCE_PASCAPULANG, entity.BPJSCode);
                                }
                            }

                            BPJSReference entityInsert = new BPJSReference();

                            for (int i = 0; i < Convert.ToInt32(respInfo.response.list.Count()); i++)
                            {
                                entityInsert.GCBPJSObjectType = Constant.BPJSObjectType.BPJS_REFERENCE_PASCAPULANG;
                                entityInsert.BPJSCode = respInfo.response.list[i].kode;
                                entityInsert.BPJSName = respInfo.response.list[i].nama;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }

                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Pasca Pulang API
        /// <summary>
        /// List Referensi Pasca Pulang
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPascaPulangList_MEDINFRASAPI(String keyword)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Pasca Pulang = '{0}'", keyword));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/referensi", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "PASCA_PULANG";
                param.Parameter = keyword + "|" + userID;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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
        #endregion

        #region Data Peserta
        #region Get Peserta
        /// <summary>
        /// Pencarian data peserta BPJS Kesehatan
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPeserta(String NoPeserta, String tglSEP)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Peserta = '{0}'", NoPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Peserta/nokartu/{1}/tglSEP/{2}", url, NoPeserta, tglSEP));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    BPJSPesertaAPI respInfo = JsonConvert.DeserializeObject<BPJSPesertaAPI>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Get Peserta API
        /// <summary>
        /// Pencarian data peserta BPJS Kesehatan menggunakan Medinfras API
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        /// 
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPeserta_MEDINFRASAPI(String NoPeserta, String tglSEP)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Peserta = '{0}'", NoPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/peserta", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "BPJSNO";
                param.Parameter = NoPeserta + "|" + tglSEP;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Get Peserta By NIK
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPesertaByNIK(String NIK, String tglSEP)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Peserta/nik/{1}/tglSEP/{2}", url, NIK, tglSEP));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    BPJSPesertaAPI respInfo = JsonConvert.DeserializeObject<BPJSPesertaAPI>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                int status = (int)response.StatusCode;
                return result;
            }
            catch (WebException ex)
            {
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", ex.Message, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        #endregion

        #region Get Peserta By NIK API
        /// <summary>
        /// Pencarian data peserta BPJS Kesehatan menggunakan Medinfras API
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        /// 
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPesertaByNIK_MEDINFRASAPI(String NIK, String tglSEP)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Peserta = '{0}'", NIK));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/peserta", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "SSN";
                param.Parameter = NIK + "|" + tglSEP;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Pencarian data rujukan berdasarkan nomor dan asal rujukan

        #region Pencarian data rujukan berdasarkan nomor dan asal rujukan
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRujukan(String noRujukan, String asalRujukan)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Rujukan = '{0}'", noRujukan));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest;

                if (asalRujukan == "1")
                    GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Rujukan/{1}", url, noRujukan));
                else
                    GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Rujukan/RS/{1}", url, noRujukan));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    RujukanAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }


                return result;
            }
            catch (WebException ex)
            {
                BPJSDefaultResponse response = new BPJSDefaultResponse();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
                return ex.Message;
            }
        }
        #endregion

        #region Pencarian data rujukan berdasarkan nomor dan asal rujukanAPI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRujukan_MedinfrasAPI(String noRujukan, String asalRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Rujukan = '{0}'", noRujukan));

            string result = "";
            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RUJUKAN";
                param.Parameter = noRujukan + "|" + asalRujukan;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region SEP
        #region Generate No. SEP
        #region Generate No. SEP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GenerateNoSEP(string noKartu, DateTime tglSEP, DateTime tglRujukan, string noRujukan, string ppkRujukan, string jnsPelayanan, string catatan, string diagAwal, string poliTujuan, string klsRawat, string lakaLantas, string lokasiLaka, string noMR, string asalRujukan, string cob, string poliEksekutif, string mobilePhoneNo, string penjamin,
            string katarak, string suplesi, string noSepSuplesi, string kodePropinsi, string kodeKabupaten, string kodeKecamatan, string noSKDP, string kodeDPJP)
        {
            GetSettingParameter(false);

            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannot generate SEP because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            penjamin_sep jaminan = new penjamin_sep()
            {
                penjamin = penjamin,
                tglKejadian = tglSEP.ToString("yyyy-MM-dd HH:mm:ss"),
                keterangan = lokasiLaka,
                suplesi = new suplesi_sep() { suplesi = suplesi, noSepSuplesi = noSepSuplesi, lokasiLaka = new lokasiLaka() { kdPropinsi = kodePropinsi, kdKabupaten = kodeKabupaten, kdKecamatan = kodeKecamatan } }
            };

            string noSuratKontrol = string.Empty;
            if (!string.IsNullOrEmpty(noSKDP) && noSKDP.Contains('.'))
            {
                string[] noSurat = noSKDP.Split('.');
                noSuratKontrol = noSurat[1];
            }
            skdp skdp = new skdp()
            {
                noSurat = noSKDP,
                kodeDPJP = kodeDPJP
            };

            BPJSGenerateSEPAPI generateSEP = new BPJSGenerateSEPAPI()
            {
                request = new BPJSGenerateSEP()
                {
                    t_sep = new t_sep()
                    {
                        noKartu = noKartu,
                        tglSep = tglSEP.ToString("yyyy-MM-dd HH:mm:ss"),
                        ppkPelayanan = ppkPelayanan,
                        jnsPelayanan = jnsPelayanan,
                        klsRawat = klsRawat,
                        noMR = noMR,
                        rujukan = new rujukan_sep() { asalRujukan = asalRujukan, tglRujukan = tglRujukan.ToString("yyyy-MM-dd HH:mm:ss"), noRujukan = noRujukan, ppkRujukan = ppkRujukan },
                        catatan = catatan,
                        diagAwal = diagAwal,
                        poli = new poli_sep() { tujuan = poliTujuan, eksekutif = poliEksekutif },
                        cob = new cob_sep() { cob = cob },
                        katarak = new katarak_sep { katarak = katarak },
                        jaminan = new jaminan() { lakaLantas = lakaLantas, penjamin = jaminan },
                        skdp = skdp,
                        noTelp = mobilePhoneNo,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };

            string data = JsonConvert.SerializeObject(generateSEP);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/1.1/insert", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSSepAPI respInfo = JsonConvert.DeserializeObject<BPJSSepAPI>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response.sep.noSep, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Generate No. SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GenerateNoSEP_MEDINFRASAPI(string noKartu, DateTime tglSEP, string ppkPelayanan, string jnsPelayanan, string klsRawatHak,
            string klsRawatNaik, string pembiayaan, string penanggungJawab, string medicalNo, string asalRujukan, DateTime tglRujukan,
            string noRujukan, string ppkRujukan, string catatan, string diagnosa, string poliTujuan, string poliEksekutif, string cob,
            string katarak, string lakaLantas, string keterangan, string suplesi, string noSepSuplesi, string kodePropinsi, string kodeKabupaten,
            string kodeKecamatan, string tujuanKunj, string flagProcedure, string kdPenunjang, string assesmentPel, string noSurat, string kodeDPJP,
            string dpjpLayan, string noTelp, string nik)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Generate SEP = '{0}'", noKartu));
            entityAPILog.MessageText = data;

            string result = "";
            String tglSEPInString = tglSEP.ToString("yyyy-MM-dd");
            String tglRujukanInString = tglRujukan.ToString("yyyy-MM-dd");
            String tglKejadian = ""; //tglSEP.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "INSERT";
                param.Parameter =
                        noKartu + "|" + tglSEPInString + "|" + ppkPelayanan + "|" + jnsPelayanan + "|"
                        + klsRawatHak + "|" + klsRawatNaik + "|" + pembiayaan + "|" + penanggungJawab + "|" + medicalNo
                        + "|" + asalRujukan + "|" + tglRujukanInString + "|" + noRujukan + "|" + ppkRujukan
                        + "|" + catatan + "|" + diagnosa + "|" + poliTujuan + "|" + poliEksekutif + "|" + cob
                        + "|" + katarak + "|" + lakaLantas + "|" + tglKejadian + "|" + keterangan + "|" + suplesi
                        + "|" + noSepSuplesi + "|" + kodePropinsi + "|" + kodeKabupaten + "|" + kodeKecamatan + "|"
                        + tujuanKunj + "|" + flagProcedure + "|" + kdPenunjang + "|" + assesmentPel + "|" + noSurat + "|"
                        + kodeDPJP + "|" + dpjpLayan + "|" + noTelp + "|" + userName + "|" + nik;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Update No SEP
        #region Update No SEP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateNoSEP(string noSep, string noKartu, DateTime tglSEP, DateTime tglRujukan, string noRujukan, string ppkRujukan, string jnsPelayanan, string catatan, string diagAwal, string poliTujuan, string klsRawat, string lakaLantas, string lokasiLaka, string noMR, string asalRujukan, string cob, string poliEksekutif, string mobilePhoneNo, string penjamin,
            string katarak, string suplesi, string noSepSuplesi, string kodePropinsi, string kodeKabupaten, string kodeKecamatan, string noSKDP, string kodeDPJP)
        {
            GetSettingParameter(false);

            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannot generate SEP because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            penjamin_sep jaminan = new penjamin_sep()
            {
                penjamin = penjamin,
                tglKejadian = tglSEP.ToString("yyyy-MM-dd HH:mm:ss"),
                keterangan = lokasiLaka,
                suplesi = new suplesi_sep() { suplesi = "0", lokasiLaka = new lokasiLaka() { kdPropinsi = kodePropinsi, kdKabupaten = kodeKabupaten, kdKecamatan = kodeKecamatan } }
            };

            skdp skdp = new skdp()
            {
                noSurat = noSKDP,
                kodeDPJP = kodeDPJP
            };

            BPJSUpdateSEPAPI updateSEP = new BPJSUpdateSEPAPI()
            {
                request = new BPJSUpdateSEP()
                {
                    t_sep = new updateSEP()
                    {
                        noSep = noSep,
                        klsRawat = klsRawat,
                        noMR = noMR,
                        rujukan = new rujukan_sep() { asalRujukan = asalRujukan, tglRujukan = tglRujukan.ToString("yyyy-MM-dd"), noRujukan = noRujukan, ppkRujukan = ppkRujukan },
                        catatan = catatan,
                        diagAwal = diagAwal,
                        poli = new poli_sep_2() { eksekutif = poliEksekutif },
                        cob = new cob_sep() { cob = cob },
                        katarak = new katarak_sep { katarak = katarak },
                        skdp = skdp,
                        jaminan = new jaminan() { lakaLantas = lakaLantas, penjamin = jaminan },
                        noTelp = mobilePhoneNo,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(updateSEP);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/1.1/Update", url));
                request.Method = "PUT";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSUpdateSepAPI respInfo = JsonConvert.DeserializeObject<BPJSUpdateSepAPI>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        response.metadata.message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        #region Update No SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateNoSEP_MEDINFRASAPI(DateTime tglSEP, string noSEP, string klsRawatHak, string klsRawatNaik, string pembiayaan, string penanggungJawab,
                            string medicalNo, string catatan, string diagnosa, string poliTujuan, string poliEksekutif, string cob,
                            string katarak, string lakaLantas, string keterangan, string suplesi, string noSepSuplesi, string kodePropinsi,
                            string kodeKabupaten, string kodeKecamatan, string dpjpLayan, string noTelp)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Update SEP = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            String tglKejadian = "";
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "UPDATE";
                param.Parameter = noSEP + "|" + klsRawatHak + "|" + klsRawatNaik + "|" + pembiayaan + "|" + penanggungJawab
                        + "|" + medicalNo + "|" + catatan + "|" + diagnosa + "|" + poliTujuan + "|" + poliEksekutif
                        + "|" + cob + "|" + katarak + "|" + lakaLantas + "|" + tglKejadian + "|" + keterangan + "|" + suplesi + "|" + noSepSuplesi
                        + "|" + kodePropinsi + "|" + kodeKabupaten + "|" + kodeKecamatan + "|" + dpjpLayan + "|" + noTelp + "|" + userName;

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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Delete SEP
        #region Delete SEP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteNoSEP(string noSep)
        {
            GetSettingParameter(false);

            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannote generate SEP because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };
            string ppkPelayanan = AppSession.BPJS_Code;

            BPJSDeleteSEPAPI deleteSEP = new BPJSDeleteSEPAPI()
            {
                request = new BPJSDeleteSEP()
                {
                    t_sep = new deleteSEP()
                    {
                        noSep = noSep,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(deleteSEP);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/delete", url));
                request.Method = "DELETE";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSUpdateSepAPI respInfo = JsonConvert.DeserializeObject<BPJSUpdateSepAPI>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Delete SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteNoSEP_MEDINFRASAPI(string noSep)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Delete SEP = '{0}'", noSep));
            entityAPILog.MessageText = data;

            string result = "";
            String userName = AppSession.UserLogin.UserName;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "DELETE";
                param.Parameter = noSep + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Cari Data SEP
        #region Cari Data SEP
        /// <summary>
        /// Integrasi Pencarian data SEP peserta BPJS Kesehatan
        /// </summary>
        /// <param name="noSEP"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object FindSEPInfo(String noSEP)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Data SEP = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/sep/{1}", url, noSEP));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    BPJSFindSepAPIResponse1 respInfo = JsonConvert.DeserializeObject<BPJSFindSepAPIResponse1>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        FindSepApiResponse oResult = new FindSepApiResponse();

                        oResult.metadata = new MetaData() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        PesertaSEPObj pesertaObj = new PesertaSEPObj()
                        {
                            catatan = respInfo.response.catatan,
                            diagnosa = respInfo.response.diagnosa,
                            jnsPelayanan = respInfo.response.jnsPelayanan,
                            kelasRawat = respInfo.response.kelasRawat,
                            noSep = respInfo.response.noSep,
                            noRujukan = respInfo.response.noRujukan,
                            penjamin = respInfo.response.penjamin,
                            peserta = respInfo.response.peserta,
                            //asuransi = respInfo.response.peserta.asuransi,
                            //hakKelas = respInfo.response.peserta.hakKelas,
                            //jnsPeserta = respInfo.response.peserta.jnsPeserta,
                            //kelamin = respInfo.response.peserta.kelamin,
                            //nama = respInfo.response.peserta.nama,
                            //noKartu = respInfo.response.peserta.noKartu,
                            //noMr = respInfo.response.peserta.noMr,
                            //tglLahir = respInfo.response.peserta.tglLahir,
                            poli = respInfo.response.poli,
                            poliEksekutif = respInfo.response.poliEksekutif,
                            tglSep = respInfo.response.tglSep
                        };

                        oResult.response = pesertaObj;

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Cari Data SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object FindSEPInfo_MEDINFRASAPI(String noSEP)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Search SEP = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "SEARCH";
                param.Parameter = noSEP;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region FindSEPInaCbgInfo
        /// <summary>
        /// Integrasi Pencarian data SEP peserta BPJS Kesehatan
        /// </summary>
        /// <param name="noSEP"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object FindSEPInaCbgInfo(String noSEP)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Data SEP = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/sep/cbg/{1}", url, noSEP));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    BPJSFindSepAPIResponse2 respInfo = JsonConvert.DeserializeObject<BPJSFindSepAPIResponse2>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        FindSepApiResponse oResult = new FindSepApiResponse();

                        oResult.metadata = new MetaData() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        PesertaSEPObj pesertaObj = new PesertaSEPObj()
                        {
                            //noMR = respInfo.response.noMr,
                            //noPeserta = respInfo.response.noKartuBpjs,
                            //tglLahir = respInfo.response.tglLahir,
                            //sex = respInfo.response.kelamin,
                            //jenisPelayanan = respInfo.response.jnsPelayanan,
                            //jenisPeserta = string.Empty,
                            //kodeHakKelas = string.Empty,
                            //namaHakKelas = string.Empty,
                            //hakKelas = string.Empty,
                            //noSEP = noSEP,
                            //tglSEP = respInfo.response.tglPelayanan,
                            //kelasRawat = respInfo.response.kelasRawat,
                            //poliTujuan = respInfo.response.poli,
                            //poliEksekutif = respInfo.response.poliEksekutif,
                            //Catatan = respInfo.response.catatan,
                            //diagnosa = string.Empty,
                        };

                        oResult.response = pesertaObj;

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion
        #endregion

        #region Pengajuan SEP

        #region Pengajuan SEP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PengajuanSEP(string noKartu, DateTime tglSep, string jnsPelayanan, string keterangan)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            SEP_Propose_Request api_request = new SEP_Propose_Request()
            {
                request = new SEP_Propose_Param()
                {
                    t_sep = new t_sep_1()
                    {
                        noKartu = noKartu,
                        tglSep = tglSep.ToString("yyyy-MM-dd"),
                        jnsPelayanan = jnsPelayanan,
                        keterangan = keterangan,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(api_request);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/pengajuanSEP", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSDefaultResponse respInfo = JsonConvert.DeserializeObject<BPJSDefaultResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        response.metadata.message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        #region Pengajuan SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PengajuanSEP_MEDINFRASAPI(string noKartu, DateTime tglSep, string jnsPelayanan, string keterangan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Pengajuan SEP = '{0}'", noKartu));
            entityAPILog.MessageText = data;

            String tglSEPInString = tglSep.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "PENGAJUAN";
                param.Parameter = noKartu + "|" + tglSEPInString + "|" + jnsPelayanan + "|" + keterangan + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Approval SEP
        #region Approval SEP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ApprovalSEP(string noKartu, DateTime tglSep, string jnsPelayanan, string keterangan)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            SEP_Propose_Request api_request = new SEP_Propose_Request()
            {
                request = new SEP_Propose_Param()
                {
                    t_sep = new t_sep_1()
                    {
                        noKartu = noKartu,
                        tglSep = tglSep.ToString("yyyy-MM-dd"),
                        jnsPelayanan = jnsPelayanan,
                        keterangan = keterangan,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(api_request);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/aprovalSEP", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSDefaultResponse respInfo = JsonConvert.DeserializeObject<BPJSDefaultResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Approval SEP API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ApprovalSEP_MEDINFRASAPI(string noKartu, DateTime tglSep, string jnsPelayanan, string keterangan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Approval SEP = '{0}'", noKartu));
            entityAPILog.MessageText = data;

            String tglSEPInString = tglSep.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "APPROVAL";
                param.Parameter = noKartu + "|" + tglSEPInString + "|" + jnsPelayanan + "|" + keterangan + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Update Tanggal Pulang

        #region Update Tanggal Pulang
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateTglPlg(string noSep, DateTime tglPlg)
        {
            GetSettingParameter(false);

            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannot process to BPJS because this is Demo Application.");
            }

            string ppkPelayanan = AppSession.BPJS_Code;

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            BPJSUpdateTglPlgAPI updateTglPlg = new BPJSUpdateTglPlgAPI()
            {
                request = new BPJSUpdateTglPlg()
                {
                    t_sep = new updateTglPlg()
                    {
                        noSep = noSep,
                        tglPulang = tglPlg.ToString("yyyy-MM-dd"),
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(updateTglPlg);
            entityAPILog.MessageText = data;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Sep/updtglplg", url));
                request.Method = "PUT";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSUpdateSepAPI respInfo = JsonConvert.DeserializeObject<BPJSUpdateSepAPI>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        response.metadata.message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        #region Update Tanggal Pulang API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateTglPlg_MEDINFRASAPI(string noSEP, string statusPulang, string noSuratMeninggal, DateTime tglMeninggal, DateTime tglPulang, string noLPManual)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Update Tanggal Pulang = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            String tglMeninggalInString = "";
            String tglPulangInString = tglPulang.ToString("yyyy-MM-dd");
            if (statusPulang == "4")
            {
                tglMeninggalInString = tglMeninggal.ToString("yyyy-MM-dd");
            }

            String userName = AppSession.UserLogin.UserName;
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "UPDATE_TGL_PULANG";
                param.Parameter = noSEP + "|" + statusPulang + "|" + noSuratMeninggal + "|" + tglMeninggalInString + "|" + tglPulangInString + "|" + noLPManual + "|" + userName;
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
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion
        #endregion
        #endregion

        #region Surat Kontrol

        #region Get Rencana Kontrol by No Peserta
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRencanaKontrolByNoPeserta_MEDINFRASAPI(string bulan, string tahun, string noKartu, string filter)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = string.Format("Bulan = {0}, Tahun = {1}, No Kartu = {2}, Filter = {3}", bulan, tahun, noKartu, filter);
            entityAPILog.MessageText = data;

            string result = "";

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "GET_RENCANA_KONTROL_NO_PESERTA";
                param.Parameter = string.Format("{0}|{1}|{2}|{3}", bulan, tahun, noKartu, filter);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Insert SPRI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertNoSPRI_MEDINFRASAPI(string noKartu, string kodeDokter, string poliKontrol, DateTime tglRencanaKontrol)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Insert SPRI = '{0}'", noKartu));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRencanaKontrolInString = tglRencanaKontrol.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "INSERT_SPRI";
                param.Parameter =
                        noKartu + "|" + kodeDokter + "|" + poliKontrol + "|" + tglRencanaKontrolInString + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Update SPRI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateNoSPRI_MEDINFRASAPI(string noSPRI, string kodeDokter, string poliKontrol, DateTime tglRencanaKontrol)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Update SPRI = '{0}'", noSPRI));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRencanaKontrolInString = tglRencanaKontrol.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "UPDATE_SPRI";
                param.Parameter =
                        noSPRI + "|" + kodeDokter + "|" + poliKontrol + "|" + tglRencanaKontrolInString + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Insert Rencana Kontrol
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertRencanaKontrol_MEDINFRASAPI(string noSEP, string kodeDokter, string poliKontrol, DateTime tglRencanaKontrol)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Insert Rencana Kontrol = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRencanaKontrolInString = tglRencanaKontrol.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "INSERT_RENCANA_KONTROL";
                param.Parameter =
                        noSEP + "|" + kodeDokter + "|" + poliKontrol + "|" + tglRencanaKontrolInString + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Update Rencana Kontrol
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateRencanaKontrol_MEDINFRASAPI(string noSuratKontrol, string noSEP, string kodeDokter, string poliKontrol, DateTime tglRencanaKontrol)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Update Rencana Kontrol = '{0}'", noSuratKontrol));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRencanaKontrolInString = tglRencanaKontrol.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "UPDATE_RENCANA_KONTROL";
                param.Parameter =
                        noSuratKontrol + "|" + noSEP + "|" + kodeDokter + "|" + poliKontrol + "|" + tglRencanaKontrolInString + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Delete Rencana Kontrol
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteRencanaKontrol_MEDINFRASAPI(string noSuratKontrol)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Delete Rencana Kontrol = '{0}'", noSuratKontrol));
            entityAPILog.MessageText = data;

            string result = "";
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rencana_kontrol", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "DELETE_RENCANA_KONTROL";
                param.Parameter =
                        noSuratKontrol + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Data Kunjungan
        #region Data Kunjungan
        /// <summary>
        /// Integrasi Pencarian data SEP peserta BPJS Kesehatan
        /// </summary>
        /// <param name="noSEP"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object MonitoringDataKunjungan(String tglSEP, String jenisPelayanan)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Monitoring Data Kunjungan Tanggal = '{0}', Jenis Pelayanan = {1}", tglSEP, jenisPelayanan));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Monitoring/Kunjungan/Tanggal/{1}/JnsPelayanan/{2}", url, tglSEP, jenisPelayanan));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    MonitoringKunjunganApiResponse respInfo = JsonConvert.DeserializeObject<MonitoringKunjunganApiResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        DataKunjunganApiResponse oResult = new DataKunjunganApiResponse();

                        oResult.metadata = new MetaData() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        //foreach (sep1 item in respInfo.response)
                        //{

                        //}
                        //PesertaSEPObj pesertaObj = new PesertaSEPObj()
                        //{
                        //    noMR = respInfo.response.peserta.noMr,
                        //    noPeserta = respInfo.response.peserta.noKartu,
                        //    nama = respInfo.response.peserta.nama,
                        //    tglLahir = respInfo.response.peserta.tglLahir,
                        //    sex = respInfo.response.peserta.kelamin,
                        //    jenisPeserta = respInfo.response.peserta.jnsPeserta,
                        //    kodeHakKelas = string.Empty,
                        //    namaHakKelas = string.Empty,
                        //    hakKelas = respInfo.response.peserta.hakKelas,
                        //    penjamin = respInfo.response.penjamin,
                        //    noSEP = noSEP,
                        //    tglSEP = respInfo.response.tglSep,
                        //    kelasRawat = respInfo.response.kelasRawat,
                        //    poliTujuan = respInfo.response.poli,
                        //    poliEksekutif = respInfo.response.poliEksekutif,
                        //    catatan = respInfo.response.catatan,
                        //    diagnosa = respInfo.response.diagnosa
                        //};

                        //oResult.response = pesertaObj;

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Get History Kunjungan
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetHistoryPelayananPeserta(string noPeserta, string tglAwal, string tglAkhir)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("History Kunjungan Pelayanan = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/monitoring/HistoriPelayanan/NoKartu/{1}/tglAwal/{2}/tglAkhir/{3}", url, noPeserta, tglAwal, tglAkhir));
                GETRequest.Method = "GET";

                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    DataHistoriPelayananApiResponse respInfo = JsonConvert.DeserializeObject<DataHistoriPelayananApiResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (respInfo.metaData.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metaData.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metaData.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetHistoryPelayananPesertaAPI(string noPeserta, string tglAwal, string tglAkhir)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("History Kunjungan Pelayanan = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/monitoring", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "HISTORY";
                param.Parameter = noPeserta + "|" + tglAwal + "|" + tglAkhir;
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
                    if (resp.Data != null)
                    {
                        DataHistoriPelayananApiResponse respInfo = JsonConvert.DeserializeObject<DataHistoriPelayananApiResponse>(resp.Data);
                        entityAPILog.Response = resp.Data;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);

                        if (respInfo.metaData.code == "200")
                        {
                            result = string.Format("{0}|{1}|{2}", "1", resp.Data, respInfo.metaData.message);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metaData.message);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", "No data found");
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Rujukan
        #region GetRujukan
        #region Rujukan Berdasarkan Nomor Rujukan
        /// <summary>
        /// Pencarian data peserta BPJS Kesehatan
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanByNoRujukan(String noRujukan, String asalRujukan)
        {
            GetSettingParameter(false);

            string result = "";
            try
            {
                string apiUrl = string.Format("{0}/Rujukan", url);
                if (asalRujukan == "2")
                {
                    apiUrl += "/RS";
                }
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", apiUrl, noRujukan));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    RujukanAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu (1 Record)

        #region Rujukan Berdasarkan Nomor Kartu (1 Record)
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu (1 Record)
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanByNoPeserta(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(false);

            string result = "";
            try
            {
                string apiUrl = string.Format("{0}", url);

                apiUrl += "/Rujukan/Peserta";

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", apiUrl, noPeserta));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    RujukanAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu (1 Record)API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanByNoPeserta_MEDINFRASAPI(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Rujukan = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RUJUKAN";
                param.Parameter = noPeserta + "|" + asalRujukan;
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
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu (1 Record) RS

        #region Rujukan Berdasarkan Nomor Kartu (1 Record) RS
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu (1 Record) RS
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanByNoPesertaRS(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(false);

            string result = "";
            try
            {
                string apiUrl = string.Format("{0}", url);

                apiUrl += "/Rujukan/RS/Peserta";

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", apiUrl, noPeserta));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    RujukanAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu (1 Record) RS API
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu (1 Record) RS API
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanByNoPesertaRS_MEDINFRASAPI(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Rujukan = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = asalRujukan;
                param.Parameter = noPeserta + "|" + asalRujukan;
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
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu : Multi Record
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanListByNoPeserta(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(false);

            string result = "";
            try
            {
                string apiUrl = string.Format("{0}", url);
                apiUrl += "/Rujukan/List/Peserta";

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", apiUrl, noPeserta));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    RujukanListAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanListAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                //}
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record API
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu : Multi Record API
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanListByNoPeserta_MEDINFRASAPI(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Rujukan = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RUJUKAN_LIST";
                param.Parameter = noPeserta + "|" + asalRujukan;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record RS

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record RS
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu : Multi Record RS
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanListByNoPesertaRS(String noPeserta, String asalRujukan)
        {
            GetSettingParameter(false);

            string result = "";
            try
            {
                string apiUrl = string.Format("{0}", url);
                apiUrl += "/Rujukan/RS/List/Peserta";

                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", apiUrl, noPeserta));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    RujukanListAPIResponse respInfo = JsonConvert.DeserializeObject<RujukanListAPIResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Rujukan Berdasarkan Nomor Kartu : Multi Record RS API
        /// <summary>
        /// Rujukan Berdasarkan Nomor Kartu : Multi Record RS API
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetRujukanListByNoPesertaRS_MEDINFRASAPI(String noPeserta, String asalRujukan)
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
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RUJUKAN_LIST";
                param.Parameter = noPeserta + "|" + asalRujukan;
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
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion
        #endregion
        #endregion

        #region Insert Rujukan API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertRujukan_MEDINFRASAPI(string noSEP, DateTime tglRujukan, DateTime tglRencanaKunjungan, string ppkRujukan,
            string jnsPelayanan, string catatan, string diagRujukan, string tipeRujukan, string poliRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Insert Rujukan = '{0}'", noSEP));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRujukanInString = tglRujukan.ToString("yyyy-MM-dd");
            String tglRencanaKunjunganInString = tglRencanaKunjungan.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "INSERT_RUJUKAN";
                param.Parameter =
                        noSEP + "|" + tglRujukanInString + "|" + tglRencanaKunjunganInString + "|" + ppkRujukan 
                        + "|" + jnsPelayanan + "|" + catatan + "|" + diagRujukan + "|" + tipeRujukan 
                        + "|" + poliRujukan + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Update Rujukan API
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateRujukan_MEDINFRASAPI(string noRujukan, DateTime tglRujukan, DateTime tglRencanaKunjungan, string ppkRujukan,
            string jnsPelayanan, string catatan, string diagRujukan, string tipeRujukan, string poliRujukan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Update Rujukan = '{0}'", noRujukan));
            entityAPILog.MessageText = data;

            string result = "";
            String tglRujukanInString = tglRujukan.ToString("yyyy-MM-dd");
            String tglRencanaKunjunganInString = tglRencanaKunjungan.ToString("yyyy-MM-dd");
            String userName = AppSession.UserLogin.UserName;

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/rujukan", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "UPDATE_RUJUKAN";
                param.Parameter =
                        noRujukan + "|" + tglRujukanInString + "|" + tglRencanaKunjunganInString + "|" + ppkRujukan + "|" 
                        + jnsPelayanan + "|" + catatan + "|" + diagRujukan + "|" + tipeRujukan + "|" 
                        + poliRujukan + "|" + userName;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Obsolete Methods
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetInformasiRujukan(string Keyword)
        {
            GetSettingParameter(false);

            try
            {
                string result = "";
                //string ppkPelayanan = BusinessLayer.GetHealthcareParameter(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.BPJS_CODE).ParameterValue;
                string ppkPelayanan = AppSession.BPJS_Code;
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/provider/ref/provider/query?nama={1}&start=0&limit=100", url, Keyword));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertRujukan(string noSEP, DateTime tglRujukan, string noKartu, DateTime tglSep, string ppkDirujuk, string jnsPelayanan, string catatan, string diagRujukan, string tipeRujukan, string poliRujukan)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            Rujukan_API_Request api_request = new Rujukan_API_Request()
            {
                request = new Rujukan_Default_Param()
                {
                    t_rujukan = new t_rujukan()
                    {
                        noSep = noSEP,
                        tglRujukan = tglRujukan.ToString("yyyy-MM-dd"),
                        ppkDirujuk = ppkDirujuk,
                        jnsPelayanan = jnsPelayanan,
                        catatan = catatan,
                        diagRujukan = diagRujukan,
                        tipeRujukan = tipeRujukan,
                        poliRujukan = poliRujukan,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };
            string data = JsonConvert.SerializeObject(api_request);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Rujukan/insert", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    BPJSDefaultResponse respInfo = JsonConvert.DeserializeObject<BPJSDefaultResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }

        #endregion
        #endregion

        #region Mapping BPJS Transaction
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object MappingBPJS(string noSep, string noTrans)
        {
            GetSettingParameter(false);

            try
            {
                string ppkPelayanan = AppSession.BPJS_Code;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/SEP/map/trans", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                MappingBPJSTransactionAPI mappingBPJS = new MappingBPJSTransactionAPI()
                {
                    request = new MappingBPJSTransaction()
                    {
                        t_map_sep = new mappingTransaction()
                        {
                            noSep = noSep,
                            noTrans = noTrans,
                            ppkPelayanan = ppkPelayanan
                        }
                    }
                };
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.BPJS,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };
                string data = JsonConvert.SerializeObject(mappingBPJS);
                entityAPILog.MessageText = data;
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        response.metadata.message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        response.metadata.message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                string result = JsonConvert.SerializeObject(response);
                return JsonConvert.SerializeObject(response);
            }
        }
        #endregion

        #region Referensi
        #region Dokter

        /// <summary>
        /// List Referensi Dokter 
        /// </summary>
        /// <param name="noSEP"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetDokterDPJPList(String jenisPelayanan, String tglPelayanan, String spesialis)
        {
            GetSettingParameter(false);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Referensi Dokter DPJP : Tanggal = '{0}', Jenis Pelayanan = {1}, Spesialis = {2}", tglPelayanan, jenisPelayanan, spesialis));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Monitoring/Kunjungan/Tanggal/{1}/JnsPelayanan/{2}", url, tglPelayanan, jenisPelayanan));
                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    MonitoringKunjunganApiResponse respInfo = JsonConvert.DeserializeObject<MonitoringKunjunganApiResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    if (respInfo.metadata.code == "200")
                    {
                        DataKunjunganApiResponse oResult = new DataKunjunganApiResponse();

                        oResult.metadata = new MetaData() { code = respInfo.metadata.code, message = respInfo.metadata.message };

                        //foreach (sep1 item in respInfo.response)
                        //{

                        //}
                        //PesertaSEPObj pesertaObj = new PesertaSEPObj()
                        //{
                        //    noMR = respInfo.response.peserta.noMr,
                        //    noPeserta = respInfo.response.peserta.noKartu,
                        //    nama = respInfo.response.peserta.nama,
                        //    tglLahir = respInfo.response.peserta.tglLahir,
                        //    sex = respInfo.response.peserta.kelamin,
                        //    jenisPeserta = respInfo.response.peserta.jnsPeserta,
                        //    kodeHakKelas = string.Empty,
                        //    namaHakKelas = string.Empty,
                        //    hakKelas = respInfo.response.peserta.hakKelas,
                        //    penjamin = respInfo.response.penjamin,
                        //    noSEP = noSEP,
                        //    tglSEP = respInfo.response.tglSep,
                        //    kelasRawat = respInfo.response.kelasRawat,
                        //    poliTujuan = respInfo.response.poli,
                        //    poliEksekutif = respInfo.response.poliEksekutif,
                        //    catatan = respInfo.response.catatan,
                        //    diagnosa = respInfo.response.diagnosa
                        //};

                        //oResult.response = pesertaObj;

                        result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(oResult), respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Insert Faskes
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertFaskes(string kodeFaskes, string namaFaskes, string tipeFaskes)
        {
            string result = string.Empty;

            List<vReferrer> lstFaskes = BusinessLayer.GetvReferrerList(string.Format("CommCode = '{0}' AND GCReferrerGroup = '{1}' AND IsDeleted = 0 AND isActive = 1", kodeFaskes, tipeFaskes));

            if (lstFaskes.Count == 0)
            {
                IDbContext ctx = DbFactory.Configure(true);
                BusinessPartnersDao entityDao = new BusinessPartnersDao(ctx);
                ReferrerDao entityRefDao = new ReferrerDao(ctx);
                AddressDao entityAddressDao = new AddressDao(ctx);
                BusinessPartnerTagFieldDao entityTagFieldDao = new BusinessPartnerTagFieldDao(ctx);
                MetaData entityMetaData = new MetaData();


                try
                {
                    BusinessPartners entity = new BusinessPartners();
                    Referrer entityRef = new Referrer();
                    Address entityAddress = new Address();
                    BusinessPartnerTagField entityTagField = new BusinessPartnerTagField();
                    #region General Information
                    entity.BusinessPartnerName = namaFaskes;
                    entity.ShortName = namaFaskes;
                    entity.CommCode = kodeFaskes;
                    #endregion

                    #region Customer Information
                    entityRef.GCReferrerGroup = tipeFaskes;
                    #endregion

                    #region Address
                    entityAddress.StreetName = "-";
                    #endregion

                    entity.BusinessPartnerCode = Helper.GeneratePartnerCode(ctx, entity.BusinessPartnerName);
                    entity.GCBusinessPartnerType = Constant.BusinessObjectType.RUJUKAN_DARI_PIHAK_KETIGA;
                    entityAddress.CreatedBy = AppSession.UserLogin.UserID;
                    entity.AddressID = entityAddressDao.InsertReturnPrimaryKeyID(entityAddress);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    if (String.IsNullOrEmpty(entity.CommCode))
                    {
                        entity.CommCode = entity.BusinessPartnerCode;
                    }
                    entity.BusinessPartnerID = entityDao.InsertReturnPrimaryKeyID(entity);

                    entityRef.BusinessPartnerID = entity.BusinessPartnerID;
                    entityRefDao.Insert(entityRef);

                    entityTagField.BusinessPartnerID = entity.BusinessPartnerID;
                    entityTagFieldDao.Insert(entityTagField);

                    entityMetaData.code = "200";
                    entityMetaData.message = entity.BusinessPartnerID.ToString();

                    ctx.CommitTransaction();

                    result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(entity), "Record succesfully added to system.");
                }
                catch (Exception ex)
                {
                    result = string.Format("{0}|{1}|{2}", "0", "null", ex.Message);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else
            {
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(lstFaskes.FirstOrDefault().BusinessPartnerID);

                result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(entity), "Record succesfully find in the system.");
            }
            return result;
        }
        #endregion
        #endregion

        #region Finger Print
        #region Finger Print by NoPeserta
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFingerPrint_MEDINFRASAPI(String noPeserta, DateTime tglPelayanan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("Get Finger Print = '{0}'", noPeserta));
            entityAPILog.MessageText = data;

            String tglPelayananInString = tglPelayanan.ToString("yyyy-MM-dd");

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "GET_FINGERPRINT";
                param.Parameter = noPeserta + "|" + tglPelayananInString;
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
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", ex.Message, ex.Status.ToString());
                        break;
                }
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #region Finger Print List
        /// <summary>
        /// List Finger Print
        /// </summary>
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetFingerPrintList_MEDINFRASAPI(DateTime tglPelayanan)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("List Finger Print = '{0}'", tglPelayanan));

            string result = "";
            String tglPelayananInString = tglPelayanan.ToString("yyyy-MM-dd");
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/sep", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "GET_FINGERPRINT_LIST";
                param.Parameter = tglPelayananInString;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region i-Care
        /// <summary>
        /// Pengambilan data i-Care
        /// </summary>
        /// <param name="NoPeserta"></param>
        /// <returns></returns>
        /// 
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetICareData(String NoPeserta, String NIK, String KodeHFIS)
        {
            GetSettingParameter(true);

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = JsonConvert.SerializeObject(string.Format("NoKartu={0}|NIK={1}|HFIS={2}", NoPeserta, NIK, KodeHFIS));
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/i-care", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = "RIWAYAT";
                if (!string.IsNullOrEmpty(NoPeserta))
                {
                    param.Parameter = NoPeserta + "|" + KodeHFIS;
                }
                else
                {
                    param.Parameter = NIK + "|" + KodeHFIS;
                }
                
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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

        #region Utility Function
        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}&{1}", consID, unixTimestamp), consPassword));
        }

        private void SetRequestHeaderMedinfrasAPI(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}{1}", unixTimestamp, consID), consPassword));
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
        #endregion

        #region LPK
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InsertLPK(string noSEP, DateTime tglMasuk, DateTime tglKeluar, string jaminan, string poli, string ruangRawat, string kelasRawat, string spesialistik, string caraKeluar, string kondisiPulang, string mainDiagnosisCode, string diagnosis, string procedure, string tindakLanjut, string kodePPK, DateTime tglKontrol, string poliKontrol, string DPJP)
        {
            GetSettingParameter(false);

            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannote process to BPJS because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            t_lpk_kontrol oFollowUpVisit = null;

            if (!string.IsNullOrEmpty(poliKontrol))
            {
                oFollowUpVisit = new t_lpk_kontrol() { tglKontrol = tglKeluar.ToString("yyyy-MM-dd"), poli = poliKontrol };
            }
            else
            {
                oFollowUpVisit = new t_lpk_kontrol() { tglKontrol = "", poli = "" };
            }

            LPK_SEP_Request_Param lpkRequest = new LPK_SEP_Request_Param()
            {
                request = new LPK_SEP_Request()
                {
                    t_lpk = new t_lpk()
                    {
                        noSep = noSEP,
                        tglMasuk = tglMasuk.ToString("yyyy-MM-dd"),
                        tglKeluar = tglKeluar.ToString("yyyy-MM-dd"),
                        jaminan = jaminan,
                        poli = new t_lpk_poli() { poli = poli },
                        perawatan = new t_lpk_perawatan() { ruangRawat = ruangRawat, kelasRawat = kelasRawat, spesialistik = spesialistik, caraKeluar = caraKeluar, kondisiPulang = kondisiPulang },
                        diagnosa = ConvertToDiagnosisList(mainDiagnosisCode, diagnosis),
                        procedure = ConvertToProcedureList(procedure),
                        rencanaTL = new t_lpk_rencanaTL() { tindakLanjut = tindakLanjut, dirujukKe = new t_lpk_rujukan() { kodePPK = kodePPK }, kontrolKembali = oFollowUpVisit },
                        DPJP = DPJP,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };

            string data = JsonConvert.SerializeObject(lpkRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LPK/insert", url));
                request.Method = "POST";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    LPKResponse respInfo = JsonConvert.DeserializeObject<LPKResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateLPK(string noSEP, DateTime tglMasuk, DateTime tglKeluar, string jaminan, string poli, string ruangRawat, string kelasRawat, string spesialistik, string caraKeluar, string kondisiPulang, string mainDiagnosisCode, string diagnosis, string procedure, string tindakLanjut, string kodePPK, DateTime tglKontrol, string poliKontrol, string DPJP)
        {
            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannote process to BPJS because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            t_lpk_kontrol oFollowUpVisit = null;

            if (!string.IsNullOrEmpty(poliKontrol))
            {
                oFollowUpVisit = new t_lpk_kontrol() { tglKontrol = tglKeluar.ToString("yyyy-MM-dd"), poli = poliKontrol };
            }
            else
            {
                oFollowUpVisit = new t_lpk_kontrol() { tglKontrol = "", poli = "" };
            }

            LPK_SEP_Request_Param lpkRequest = new LPK_SEP_Request_Param()
            {
                request = new LPK_SEP_Request()
                {
                    t_lpk = new t_lpk()
                    {
                        noSep = noSEP,
                        tglMasuk = tglMasuk.ToString("yyyy-MM-dd"),
                        tglKeluar = tglKeluar.ToString("yyyy-MM-dd"),
                        jaminan = jaminan,
                        poli = new t_lpk_poli() { poli = poli },
                        perawatan = new t_lpk_perawatan() { ruangRawat = ruangRawat, kelasRawat = kelasRawat, spesialistik = spesialistik, caraKeluar = caraKeluar, kondisiPulang = kondisiPulang },
                        diagnosa = ConvertToDiagnosisList(mainDiagnosisCode, diagnosis),
                        procedure = ConvertToProcedureList(procedure),
                        rencanaTL = new t_lpk_rencanaTL() { tindakLanjut = tindakLanjut, dirujukKe = new t_lpk_rujukan() { kodePPK = kodePPK }, kontrolKembali = oFollowUpVisit },
                        DPJP = DPJP,
                        user = AppSession.UserLogin.UserName
                    }
                }
            };

            string data = JsonConvert.SerializeObject(lpkRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LPK/update", url));
                request.Method = "PUT";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    LPKResponse respInfo = JsonConvert.DeserializeObject<LPKResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR;
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED;
                        break;
                    case WebExceptionStatus.Timeout:
                        message = Constant.BPJS_WS_EXCEPTION.TIMEOUT;
                        break;
                    default:
                        message = string.Format("{0} - {1}", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status);
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteLPK(string noSEP)
        {
            if (AppConfigManager.BPJSDemoMode == "1")
            {
                return string.Format("{0}|{1}|{2}", "0", "null", "Cannote process to BPJS because this is Demo Application.");
            }

            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string ppkPelayanan = AppSession.BPJS_Code;

            LPK_SEP_Delete_Request_Param lpkRequest = new LPK_SEP_Delete_Request_Param()
            {
                request = new LPK_SEP_Delete_Request()
                {
                    t_lpk = new t_lpk_delete()
                    {
                        noSep = noSEP
                    }
                }
            };

            string data = JsonConvert.SerializeObject(lpkRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LPK/delete", url));
                request.Method = "DELETE";
                request.ContentType = "text/plain";
                SetRequestHeader(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    LPKResponse respInfo = JsonConvert.DeserializeObject<LPKResponse>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.response, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.metadata.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                BPJSSepAPI response = new BPJSSepAPI();
                response.metadata = new MetaData();
                response.metadata.code = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.metadata.message = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }

        private List<t_lpk_procedure> ConvertToProcedureList(string procedure)
        {
            if (!string.IsNullOrEmpty(procedure))
            {
                List<t_lpk_procedure> listPr = new List<t_lpk_procedure>();
                string[] lstProcedure = procedure.Split(';');
                foreach (string pr in lstProcedure)
                {
                    t_lpk_procedure proc = new t_lpk_procedure();
                    proc.kode = pr;
                    listPr.Add(proc);
                }
                return listPr;
            }
            else
            {
                return null;
            }
        }

        private List<t_lpk_diagnosa> ConvertToDiagnosisList(string mainDiagnosisCode, string diagnosis)
        {
            if (!string.IsNullOrEmpty(diagnosis))
            {
                t_lpk_diagnosa diagnosa = new t_lpk_diagnosa();
                if (!String.IsNullOrEmpty(mainDiagnosisCode))
                {
                    diagnosa.level = "1";
                    diagnosa.kode = mainDiagnosisCode;

                    List<t_lpk_diagnosa> listDx = new List<t_lpk_diagnosa>();
                    listDx.Add(diagnosa);

                    string[] lstDiagnosa = diagnosis.Split(';');
                    foreach (string dx in lstDiagnosa)
                    {
                        if (dx != mainDiagnosisCode)
                        {
                            diagnosa = new t_lpk_diagnosa();
                            diagnosa.level = "2";
                            diagnosa.kode = dx;
                            listDx.Add(diagnosa);
                        }
                    }
                    return listDx;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region MOBILE JKN
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendNotificationToJKN(String type, String parameter)
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

            switch (type)
            {
                case "UPDATE_JADWAL_DOKTER"://paramedicCode | serviceUnitCode
                    result = UpdateJadwalDokter(parameter, ref entityAPILog);
                    break;
                case "TAMBAH_ANTRIAN":
                    result = TambahAntrian(parameter, ref entityAPILog);
                    break;
                case "TAMBAH_ANTRIAN_NEW":
                    result = TambahAntrianNew(parameter, ref entityAPILog);
                    break;
                case "BATAL_ANTRIAN":
                    result = BatalAntrian(parameter, ref entityAPILog);
                    break;
                case "UPDATE_WAKTU":
                    result = UpdateAntrian(parameter, ref entityAPILog);
                    break;
            }

            return result;
        }

        private string UpdateJadwalDokter(string parameter, ref APIMessageLog entityAPILog)
        {
            string result = "1|";

            string[] param = parameter.Split('|');
            string paramedicCode = param[0];
            string serviceUnitCode = param[1];

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/base/physician/schedule/update", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                UpdateJadwalDokter obj = new UpdateJadwalDokter();
                obj.ParamedicCode = paramedicCode;
                obj.ServiceUnitCode = serviceUnitCode;
                entityAPILog.MessageText = JsonConvert.SerializeObject(param);

                string jsonReq = JsonConvert.SerializeObject(obj);

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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }

            return result;
        }
        private string TambahAntrian(string parameter, ref APIMessageLog entityAPILog)
        {
            string result = "1|";
            string[] param = parameter.Split('|');
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/base/queue/add", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                TambahAntrianJKN obj = new TambahAntrianJKN();
                obj.AppointmentNo = param[0];
                obj.RegistrationNo = param[1];
                entityAPILog.MessageText = JsonConvert.SerializeObject(obj);

                string jsonReq = JsonConvert.SerializeObject(obj);

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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }

            return result;
        }
        public string TambahAntrianNew(string parameter, ref APIMessageLog entityAPILog)
        {
            string result = "1|";
            string[] param = parameter.Split('|');

            IDbContext ctx = DbFactory.Configure(true);
            APIMessageLogDao logDao = new APIMessageLogDao(ctx);

            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/base/queue/add/new", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                TambahAntrianJKN obj = new TambahAntrianJKN();
                obj.AppointmentNo = param[0];
                entityAPILog.MessageText = JsonConvert.SerializeObject(obj);

                string jsonReq = JsonConvert.SerializeObject(obj);

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
                        //BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        logDao.Insert(entityAPILog);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", null, resp.Remarks);
                        entityAPILog.Response = result;
                        entityAPILog.IsSuccess = false;
                        //BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        logDao.Insert(entityAPILog);
                    }
                }
                ctx.CommitTransaction();
            }
            catch (WebException ex)
            {
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                logDao.Insert(entityAPILog);

                ctx.CommitTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }
        private string BatalAntrian(string parameter, ref APIMessageLog entityAPILog)
        {
            string result = "1|";
            string[] param = parameter.Split('|');
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/base/queue/cancel", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BatalAntrianJKN obj = new BatalAntrianJKN();
                obj.IsFromAppointment = "From Appointment";
                obj.AppointmentNo = param[0];
                obj.DeleteReason = param[1];
                entityAPILog.MessageText = JsonConvert.SerializeObject(obj);

                string jsonReq = JsonConvert.SerializeObject(obj);

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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }

            return result;
        }
        private string UpdateAntrian(string parameter, ref APIMessageLog entityAPILog)
        {
            string result = "1|";
            string[] param = parameter.Split('|');
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/base/queue/update", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                UpdateAntrianWaktuJKN obj = new UpdateAntrianWaktuJKN();
                obj.AppointmentNo = param[0];
                obj.TaskID = param[1];
                entityAPILog.MessageText = JsonConvert.SerializeObject(obj);

                string jsonReq = JsonConvert.SerializeObject(obj);

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
                        result = string.Format("1|{0}|{1}|{2}", resp.Data, resp.Remarks, jsonReq);
                        entityAPILog.Response = result;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}|{2}", null, resp.Remarks, jsonReq);
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", Constant.BPJS_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }

            return result;
        }

        #region Dashboard

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetDashboardAntrianOnline(String dashboardType, String dashboardParameter)
        {
            string result = "1|";
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string userID = AppSession.UserLogin.UserID.ToString();
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/mobilejkn/dashboard", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);

                BPJSReferenceModel_MedinfrasAPI param = new BPJSReferenceModel_MedinfrasAPI();
                param.Type = dashboardType;
                param.Parameter = dashboardParameter;
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
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
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
    }
}
