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
    public class QueueService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";
        string apiKey = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.QUMATIC_API_KEY).ParameterValue;

        #region MEDINFRAS-API : Centerback
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ADT_A01(string healthcareID, Registration oRegistration, VisitInfo visitData, PatientData patientData)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                string url = AppSession.SA0132;

                #region Convert into DTO Objects
                RegistrationDTO oData = new RegistrationDTO();
                oData.HealthcareID = healthcareID;
                oData.RegistrationID = oRegistration.RegistrationID;
                oData.RegistrationNo = oRegistration.RegistrationNo;
                oData.AppointmentID = Convert.ToInt32(oRegistration.AppointmentID);
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
                    SetMedinfrasAPIRequestHeader(request, AppSession.SA0130, AppSession.SA0131);

                    json = JsonConvert.SerializeObject(oData);
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

                    MedinfrasAPIResponse2 respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse2>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remarks.ToString(), json);
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

        public string ADT_A05_Cancel(string healthcareID, Appointment oAppointment)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                string url = AppSession.SA0132;

                #region Convert into DTO Objects
                Appointment2DTO oData = new Appointment2DTO();
                oData.HealthcareID = healthcareID;
                oData.AppointmentID = oAppointment.AppointmentID;
                oData.PatientID = Convert.ToInt32(oAppointment.MRN);
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ADT_A05_Cancel", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    SetMedinfrasAPIRequestHeader(request, AppSession.SA0130, AppSession.SA0131);

                    json = JsonConvert.SerializeObject(oData);
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

                    MedinfrasAPIResponse2 respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse2>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remarks.ToString(), json);
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
                result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Message.ToString(), string.Empty), "");
                return result;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string BAR_P01(string healthcareID, Registration oRegistration)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;
                #region Convert into DTO Objects
                RegistrationDTO oData = new RegistrationDTO();
                oData.HealthcareID = healthcareID;
                oData.RegistrationID = oRegistration.RegistrationID;
                oData.RegistrationNo = oRegistration.RegistrationNo;
                oData.AppointmentID = Convert.ToInt32(oRegistration.AppointmentID);
                oData.PatientID = (int)oRegistration.MRN;
                oData.PatientInfo = null;
                oData.VisitInformation = null;
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/BAR_P01", url));
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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark, json);
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

        /// <summary>
        /// Type : 00 = Data Kunjungan, 01 = Transaksi Pasien, 02 = Pembuatan Tagihan, 03 = Pembayaran
        /// TransactionReferenceID = Nomor Transaksi / Nomor Tagihan / Nomor Payment tergantung type
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string BAR_P05(string healthcareID, Registration oRegistration, string type, int transactionReferenceID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;
                #region Convert into DTO Objects
                Registration2DTO oData = new Registration2DTO();
                oData.HealthcareID = healthcareID;
                oData.RegistrationID = oRegistration.RegistrationID;
                oData.RegistrationNo = oRegistration.RegistrationNo;
                oData.AppointmentID = Convert.ToInt32(oRegistration.AppointmentID);
                oData.PatientID = (int)oRegistration.MRN;
                oData.PatientInfo = null;
                oData.VisitInformation = null;
                oData.PayerType = oRegistration.GCCustomerType;
                oData.MessageType = type;
                oData.TransactionReferenceID = transactionReferenceID;
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/BAR_P05", url));
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

        public string PV1_R01_Cancel(string healthcareID, Registration oRegistration)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                string url = AppSession.SA0132;

                #region Convert into DTO Objects
                Appointment2DTO oData = new Appointment2DTO();
                oData.HealthcareID = healthcareID;
                if (oRegistration.AppointmentID > 0)
                {
                    oData.AppointmentID = Convert.ToInt32(oRegistration.AppointmentID);
                }
                oData.RegistrationID = oRegistration.RegistrationID;
                oData.PatientID = Convert.ToInt32(oRegistration.MRN);
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/PV1_R01_Cancel", url));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    SetMedinfrasAPIRequestHeader(request, AppSession.SA0130, AppSession.SA0131);

                    json = JsonConvert.SerializeObject(oData);
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

                    MedinfrasAPIResponse2 respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse2>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remarks.ToString(), json);
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
                result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Message.ToString(), string.Empty), "");
                return result;
            }
        }
        #endregion


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
                        result = string.Format("{0}|{1}|{2}", "0", String.Format(" Catch : {0}", ex.ToString()), string.Empty);
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
                        result = string.Format("{0}|{1}|{2}", "0", String.Format(" Catch : {0}", ex.ToString()), string.Empty);
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
                        result = string.Format("{0}|{1}|{2}", "0", String.Format(" Catch : {0}", ex.ToString()), string.Empty);
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
                        result = string.Format("{0}|{1}", "0", String.Format(" Catch : {0}", ex.ToString()));
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
                        result = string.Format("{0}|{1}", "0", String.Format(" Catch : {0}", ex.ToString()));
                        break;
                }
                return result;
            }
        }

        #region MobileJKN Services
        /// <summary>
        /// MFN : Master File Notification
        /// X01 : Physician-Service Unit Schedule
        /// </summary>
        /// <param name="paramedicID"></param>
        /// <param name="healthcareServiceUnitID"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string MFN_X01(string healthcareID,int paramedicID, int healthcareServiceUnitID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;
                #region Convert into DTO Objects
                MFNParameter1DTO oData = new MFNParameter1DTO();
                oData.HealthcareID = healthcareID;
                oData.ParamedicID = paramedicID;
                oData.HealthcareServiceUnitID = healthcareServiceUnitID;
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/MFN_X01", url));
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

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark, json);
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

        #region Qumatic's Method Services
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string QumaticNewApmInformation(vAppointment oAppointment, QumaticPatientInfo oPatient, QumaticPhysicianInfo oPhysician, QumaticScheduleInfo oSchedule, QumaticBodyRequestNewApm oRequest)
        {
            string result = "";
            try
            {
                SettingParameterDt url = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.WEB_URL_QUMATIC);
                //string url = "";
                #region Convert into DTO Objects
                QumaticBodyRequest oData = new QumaticBodyRequest();
                oData.apiKey = apiKey;
                oData.payload = oRequest;
                var jsona = JsonConvert.SerializeObject(oData);
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/v3.appointment.new", url.ParameterValue));
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
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark, json);
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string QumaticConfirmation(Registration oRegistration, QumaticBodyRequestApmConfirmation oBody)
        {
            string result = "";
            try
            {
                SettingParameterDt url = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.WEB_URL_QUMATIC);
                //string url = "";
                #region Convert into DTO Objects
                QumaticBodyRequestConfirmation oData = new QumaticBodyRequestConfirmation();
                oData.apiKey = apiKey;
                oData.payload = oBody;
                var jsona = JsonConvert.SerializeObject(oData);
                #endregion

                if (oData != null)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/v3.appointment.confirm", url.ParameterValue));
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
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark, json);
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

        #region Medinlink's (SIMIDUN) Method Services
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string MedinlinkPatientCallNotification(vConsultVisit9 entity, String roomCode)
        {
            string result = "";
            try
            {
                SettingParameterDt url = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA_ALAMAT_WEBAPI_SISTEM_ANTRIAN);
                Room entityRoom = BusinessLayer.GetRoomList(string.Format("RoomID = {0}", entity.RoomID)).FirstOrDefault();
                Healthcare entityHealthcare = BusinessLayer.GetHealthcare("001");

                if (entity != null)
                {
                    WebRequest request = WebRequest.Create(url.ParameterValue);
                    request.Method = "POST";
                    request.ContentType = "application/x-www-form-urlencoded";

                    String obj = string.Empty;
                    if (entityHealthcare.Initial == "BROS")
                    {
                        obj += string.Format("idService=Brospoli&", Environment.NewLine);
                        obj += string.Format("method=umumkan&", Environment.NewLine);
                        obj += string.Format("ticket={0}&", entity.QueueNo.ToString(), Environment.NewLine);
                        obj += string.Format("ruangan={0}&", entityRoom.RoomQueuePrefix, Environment.NewLine);
                        obj += string.Format("dokter={0}", entity.ParamedicName);
                    }
                    else if (entityHealthcare.Initial == "RSSM")
                    {
                        string initial = string.Empty;
                        string QueueNo = string.Empty;
                        string InitialPoli = string.Empty;
                        ParamedicMaster oParamedicMaster = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = '{0}'", entity.ParamedicID)).FirstOrDefault();
                        if (oParamedicMaster != null) {
                            initial = oParamedicMaster.Initial; 
                        }
                        ServiceUnitMaster osm = BusinessLayer.GetServiceUnitMasterList(string.Format("ServiceUnitID='{0}'", entity.ServiceUnitID)).FirstOrDefault();
                        if (osm != null) {
                            InitialPoli = osm.ShortName; 
                        }

                        QueueNo = string.Format("{0}{1}{2}", initial, InitialPoli, entity.QueueNo);

                        obj += string.Format("method=umumkan&");
                        obj += string.Format("idService=Poli_sa&");
                        obj += string.Format("poli={0}&", entity.ServiceUnitName);
                        obj += string.Format("nomor={0}&", QueueNo);
                        obj += string.Format("dokter={0}&", entity.ParamedicName);
                        obj += string.Format("kdpoli={0}&", entity.ServiceUnitCode);
                        obj += string.Format("kdruang={0}&", entity.RoomCode);
                        obj += string.Format("namaruang={0}", entity.RoomName);

                    }

                    byte[] bytes = Encoding.UTF8.GetBytes(obj);
                    request.ContentLength = bytes.Length;
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)request.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = sr.ReadToEnd();
                        //result = string.Format("{0}|{1}|{2}", "1", requestUrl.Replace('|', '^'), string.Empty);
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

        #region Medinlink's (SIMIDUN)  VIA MEDIN API
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string MedinlinkPatientCallNotificationViaAPIMedin(vConsultVisit9 entity, String roomCode)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;

                List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_USING_AUTHENTICATION));
                string isUsingAuth = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.IS_USING_AUTHENTICATION).FirstOrDefault().ParameterValue;

                HttpWebRequest GetRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/medinlink/patientcall/{1}/{2}", url, entity.RegistrationID, entity.VisitID));
                GetRequest.Method = "GET";

                if (isUsingAuth == "1")
                {
                    SetRequestHeader(GetRequest);
                }

                HttpWebResponse response = (HttpWebResponse)GetRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);

                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    Paramedic respInfo = JsonConvert.DeserializeObject<Paramedic>(result);
                    if (respInfo.metadata.code == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "0", result, respInfo.metadata.message);
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

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string MedinlinkGetQueuePharmacyViaAPIMedin(string TransactionNo)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/medinlink/GetQueueNoPharmacy", url));
                request.Method = "POST";
                request.ContentType = "application/json";

                #region Convert into DTO Objects
                PatientTransactionDto oData = new PatientTransactionDto();
                oData.TransactionNo = TransactionNo;
                var jsona = JsonConvert.SerializeObject(oData);
                #endregion
                //if (Constant.SettingParameter.IS_USING_AUTHENTICATION == "1")
                //{
                //    SetRequestHeader(GetRequest);
                //}
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

                //APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                //if (!string.IsNullOrEmpty(respInfo.Data))
                //{
                //    result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                //}
                //else
                //{
                //    result = string.Format("{0}|{1}|{2}", "0", respInfo.Remark, json);
                //}

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

    public class MedinlinkNotification
    {
        public string idService { get; set; }
        public string method { get; set; }
        public string ticket { get; set; }
        public string ruangan { get; set; }
        public string dokter { get; set; }
    }
}
