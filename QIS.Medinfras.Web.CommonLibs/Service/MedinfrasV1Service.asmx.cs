using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;


namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for GatewayService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MedinfrasV1Service : System.Web.Services.WebService
    {
        private const string MSG_SUCCESS = "SUCCESS";

        #region POST METHOD

        #region Send Order Medical Diagnostic
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendOrderMedicalDiagnosticServices(int ProcessType, TestOrderHd oOrder, PrescriptionOrderHd oPrescription, List<vPrescriptionOrderDt1> lstPrescriptionDt)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/OBR_R01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                SendOrderInfo obj = new SendOrderInfo();
                obj.ProcessType = ProcessType;
                obj.TestOrderHd = oOrder;
                obj.PrescriptionOrderHd = oPrescription;
                obj.ListPrescriptionOrderDt = lstPrescriptionDt;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Send Patient Diagnose Information
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendPatientDiagnoseInformation(int ProcessType, PatientDiagnosis diagnose)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/DG1_D01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                SendPatientClinicalInformation obj = new SendPatientClinicalInformation();
                obj.ProcessType = ProcessType;
                obj.Diagnosis = diagnose;
                obj.Procedures = null;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Send Patient Procedures Information
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendPatientProceduresInformation(int ProcessType, PatientProcedure procedures)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/DG1_P01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                SendPatientClinicalInformation obj = new SendPatientClinicalInformation();
                obj.ProcessType = ProcessType;
                obj.Diagnosis = null;
                obj.Procedures = procedures;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Send Notification Patient's Visit Discharge
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendPatientVisitDischarge(string registrationNo)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/PV1_D01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                PV1_BodyRequest obj = new PV1_BodyRequest();
                obj.RegistrationNo = registrationNo;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Send Notification Patient's Vital Sign Information
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendPatientVitalSignInfo(string registrationNo)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/DG1_V01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                SendPatientClinicalInformation obj = new SendPatientClinicalInformation();
                obj.RegistrationNo = registrationNo;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Send Notification Patient's Allergies Information
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendPatientAllergiesInformation(int processType, string registrationNo)
        {
            string result = string.Empty;
            string processStage = string.Empty;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/PID_A01", AppSession.SA0132));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeader(updateRequest);

                SendPatientClinicalInformation obj = new SendPatientClinicalInformation();
                obj.ProcessType = processType;
                obj.RegistrationNo = registrationNo;

                string data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    MedinfrasAPIResponse apiResponse = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(sr.ReadToEnd());
                    if (apiResponse.Status == "SUCCESS")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", data, apiResponse.Data);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", data, apiResponse.Remarks);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion
        
        #endregion

        #region Utility Function
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
        #endregion
    }
}
