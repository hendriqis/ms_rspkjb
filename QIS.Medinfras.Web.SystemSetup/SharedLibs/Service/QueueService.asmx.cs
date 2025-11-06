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

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for QueueService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class QueueService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendVisitInformation(Registration oRegistration,VisitInfo visitData, PatientData patientData)
        {
            string result = "";
            try
            {
                string url = AppSession.QUEUE_WEB_API_URL;
                #region Convert into DTO Objects
                RegistrationDTO oData = new RegistrationDTO();
                oData.RegistrationID = oRegistration.RegistrationID;
                oData.RegistrationNo = oRegistration.RegistrationNo;
                oData.PatientID = (int)oRegistration.MRN;
                oData.PatientInfo = patientData;

                List<VisitInfo> lstVisit = new List<VisitInfo>();
                VisitInfo oVisit = visitData;
                lstVisit.Add(oVisit);
                oData.VisitInformation = lstVisit;

                #endregion
             
                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ADT_A01", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";

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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data,json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark,json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties",string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "Method not found",string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(),string.Empty));
                        break;
                }
                return result;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendDischargeInformation(int registrationID, string registrationNo, int mrn, VisitInfo visitData)
        {
            string result = "";
            try
            {
                string url = AppSession.QUEUE_WEB_API_URL;
                #region Convert into DTO Objects
                RegistrationDTO oData = new RegistrationDTO();
                oData.RegistrationID = registrationID;
                oData.RegistrationNo = registrationNo;
                oData.PatientID = mrn;
                oData.PatientInfo = null;

                List<VisitInfo> lstVisit = new List<VisitInfo>();
                VisitInfo oVisit = visitData;
                lstVisit.Add(oVisit);
                oData.VisitInformation = lstVisit;

                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ADT_A03", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";

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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (respInfo.Status == MSG_SUCCESS)
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data,json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark,json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties",string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "Method not found",string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(),string.Empty));
                        break;
                }
                return result;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendPatientTransferInformation(int registrationID, string registrationNo, int mrn, VisitInfo visitData, PatientTranferInfo transferInfo)
        {
            string result = "";
            try
            {
                string url = AppSession.QUEUE_WEB_API_URL;
                #region Convert into DTO Objects
                PatientTransferDTO oData = new PatientTransferDTO();
                oData.RegistrationID = registrationID;
                oData.RegistrationNo = registrationNo;
                oData.PatientID = mrn;
                VisitInfo oVisit = visitData;
                oData.VisitInformation = oVisit;
                oData.TransferInfo = transferInfo;

                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ADT_A02", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";

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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (respInfo.Status == MSG_SUCCESS)
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data,json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark,json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties",string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "Method not found",string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(),string.Empty));
                        break;
                }
                return result;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendPrescriptionOrder(PrescriptionOrderDTO oData)
        {
            string result = "";
            try
            {
                string url = AppSession.QUEUE_WEB_API_URL;

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ORM_O01", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";

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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (respInfo.Status == MSG_SUCCESS)
                    {
                        result = string.Format("{0}|{1}", "1", respInfo.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", respInfo.Remark);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}", "0", "There is no information to be sent to 3rd Parties");
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}", "0", "Method not found");
                        break;
                    default:
                        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                        break;
                }
                return result;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendOrderInformation(OrderDTO oData)
        {
            string result = "";
            try
            {
                string url = AppSession.QUEUE_WEB_API_URL;

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ORM_O01", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";

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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (respInfo.Status == MSG_SUCCESS)
                    {
                        result = string.Format("{0}|{1}", "1", respInfo.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", respInfo.Remark);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}", "0", "There is no information to be sent to 3rd Parties");
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}", "0", "Method not found");
                        break;
                    default:
                        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                        break;
                }
                return result;
            }
        }

    }
}
