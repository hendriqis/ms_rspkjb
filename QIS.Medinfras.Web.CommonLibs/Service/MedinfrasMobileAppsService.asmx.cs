using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;


namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for MedinfrasMobileAppsService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class MedinfrasMobileAppsService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";
        string url = string.Empty;
        string healthcareGroup = string.Empty;

        private void GetSettingParameter()
        {
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, Constant.SettingParameter.WEB_URL_MEDINFRAS_MOBILE_APPS, Constant.SettingParameter.HEALTHCARE_GROUP_MEDINFRAS_MOBILE_APPS));

            url = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.WEB_URL_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            healthcareGroup = lstSettingParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.HEALTHCARE_GROUP_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        #region POST METHOD

        #region On Changed Service Unit Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnServiceUnitMasterChanged(string ServiceUnitID, string ServiceUnitName, string ServiceUnitName2, string ShortName, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/serviceunit_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/serviceunit/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/serviceunit/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreServiceUnitChanged entity = new StoreServiceUnitChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.ServiceUnitID = ServiceUnitID;
                entity.ServiceUnitName = ServiceUnitName;
                entity.ServiceUnitName2 = ServiceUnitName2;
                entity.ShortName = ShortName;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Healthcare Service Unit Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnHealthcareServiceUnitChanged(string HealthcareServiceUnitID, string ServiceUnitID, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/healthcareserviceunit_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/healthcareserviceunit/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/healthcareserviceunit/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreHealthcareServiceUnitChanged entity = new StoreHealthcareServiceUnitChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.HealthcareServiceUnitID = HealthcareServiceUnitID;
                entity.ServiceUnitID = ServiceUnitID;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Operational Time Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnOperationalTimeMasterChanged(OperationalTime oOperationalTime, int operationalTimeID, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/operationaltime_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/operationaltime/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/operationaltime/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreOperationalTimeChanged entity = new StoreOperationalTimeChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.OperationalTimeID = operationalTimeID.ToString();
                entity.StartTime1 = oOperationalTime.StartTime1;
                entity.StartTime2 = oOperationalTime.StartTime2;
                entity.StartTime3 = oOperationalTime.StartTime3;
                entity.StartTime4 = oOperationalTime.StartTime4;
                entity.StartTime5 = oOperationalTime.StartTime5;
                entity.EndTime1 = oOperationalTime.EndTime1;
                entity.EndTime2 = oOperationalTime.EndTime2;
                entity.EndTime3 = oOperationalTime.EndTime3;
                entity.EndTime4 = oOperationalTime.EndTime4;
                entity.EndTime5 = oOperationalTime.EndTime5;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Specialty Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSpecialtyMasterChanged(string SpecialtyID, string SpecialtyName, string SpecialtyName2, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/specialty_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/specialty/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/specialty/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreSpecialtyChanged entity = new StoreSpecialtyChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.SpecialtyID = SpecialtyID;
                entity.SpecialtyName = SpecialtyName;
                entity.SpecialtyName2 = SpecialtyName2;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Paramedic Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnParamedicMasterChanged(ParamedicMaster oParamedicMaster, int paramedicID, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/paramedicmaster_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/paramedicmaster/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);

                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/paramedicmaster/store", url));

                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreParamedicChanged entity = new StoreParamedicChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.SpecialtyID = oParamedicMaster.SpecialtyID;
                entity.ParamedicID = paramedicID.ToString();
                entity.FullName = oParamedicMaster.FullName;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Visit Type Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnVisitTypeMasterChanged(VisitType oVisitType, int visitTypeID, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/visittype_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/visittype/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/visittype/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreVisitTypeChanged entity = new StoreVisitTypeChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.VisitTypeID = visitTypeID.ToString();
                entity.VisitTypeName = oVisitType.VisitTypeName;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Patient Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPatientMasterChanged(Patient oPatient, int mrn, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/patient_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/patient/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/patient/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StorePatientChanged entity = new StorePatientChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.MedicalNo = oPatient.MedicalNo;
                entity.MobilePhoneNo1 = oPatient.MobilePhoneNo1;
                entity.MRN = mrn.ToString();
                entity.EmailAddress = oPatient.EmailAddress;
                entity.FullName = oPatient.FullName;
                entity.DateOfBirth = oPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Patient Family Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPatientFamilyMasterChanged(PatientFamily oPatientFamily, string email, string eventType)
        {
            string result = "";
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/patient_family_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/patient/family/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/patient/family/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                Patient entityPatient = null;
                if (oPatientFamily.FamilyMRN > 0)
                {
                    entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oPatientFamily.FamilyMRN)).FirstOrDefault();
                }

                StorePatientFamilyChanged entity = new StorePatientFamilyChanged();
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.FamilyID = oPatientFamily.FamilyID;
                entity.MRN = oPatientFamily.MRN;
                entity.FamilyMRN = Convert.ToInt32(oPatientFamily.FamilyMRN);
                entity.GCFamilyRelation = oPatientFamily.GCFamilyRelation;
                entity.IsDeleted = oPatientFamily.IsDeleted ? 1 : 0;
                entity.MedicalNo = entityPatient != null ? entityPatient.MedicalNo : string.Empty;
                entity.FullName = entityPatient != null ? entityPatient.FullName : string.Empty;
                entity.MobilePhone1 = entityPatient != null ? !string.IsNullOrEmpty(entityPatient.MobilePhoneNo2) ? entityPatient.MobilePhoneNo2 : !string.IsNullOrEmpty(entityPatient.MobilePhoneNo1) ? entityPatient.MobilePhoneNo1 : string.Empty : string.Empty;
                entity.EmailAddress = !string.IsNullOrEmpty(email) ? email : string.Empty;
                entity.DateOfBirth = entityPatient != null ? !string.IsNullOrEmpty(entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT)) ? entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT) : string.Empty : string.Empty;

                string data = JsonConvert.SerializeObject(entity);

                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    updateRequest.ContentLength = bytes.Length;
                    Stream putStream = updateRequest.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)updateRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                    }
                }
                catch (Exception ex)
                {
                    result = string.Format("0|{0}|{1}", data, ex.Message);
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Changed Paramedic Schedule Routine Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnParamedicScheduleChanged(ParamedicSchedule oSchedule, int healthcareServiceUnitID, int paramedicID, int operationalTimeID, int dayNumber, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/paramedicschedule_routine_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/paramedicschedule/routine/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/paramedicschedule/routine/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreParamedicScheduleChanged entity = new StoreParamedicScheduleChanged();
                if (eventType == "001")
                {
                    entity.HealthcareServiceUnitID = oSchedule.HealthcareServiceUnitID;
                    entity.ParamedicID = oSchedule.ParamedicID;
                    entity.DayNumber = oSchedule.DayNumber;
                    entity.OperationalTimeID = oSchedule.OperationalTimeID;
                }
                else
                {
                    entity.HealthcareServiceUnitID = healthcareServiceUnitID;
                    entity.ParamedicID = paramedicID;
                    entity.DayNumber = dayNumber;
                    entity.OperationalTimeID = operationalTimeID;
                }
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.IsBPJS1 = oSchedule.IsBPJS1;
                entity.IsBPJS2 = oSchedule.IsBPJS2;
                entity.IsBPJS3 = oSchedule.IsBPJS3;
                entity.IsBPJS4 = oSchedule.IsBPJS4;
                entity.IsBPJS5 = oSchedule.IsBPJS5;
                entity.IsNonBPJS1 = oSchedule.IsNonBPJS1;
                entity.IsNonBPJS2 = oSchedule.IsNonBPJS2;
                entity.IsNonBPJS3 = oSchedule.IsNonBPJS3;
                entity.IsNonBPJS4 = oSchedule.IsNonBPJS4;
                entity.IsNonBPJS5 = oSchedule.IsNonBPJS5;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Paramedic Schedule By Date Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnParamedicScheduleDateChanged(vParamedicScheduleDate1 oSchedule, int healthcareServiceUnitID, int paramedicID, int operationalTimeID, string scheduleDate, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/paramedicschedule_date_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/paramedicschedule/date/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/paramedicschedule/date/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreParamedicScheduleDateChanged entity = new StoreParamedicScheduleDateChanged();
                if (eventType == "001")
                {
                    entity.HealthcareServiceUnitID = oSchedule.HealthcareServiceUnitID;
                    entity.ParamedicID = oSchedule.ParamedicID;
                    entity.ScheduleDate = oSchedule.ScheduleDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    entity.OperationalTimeID = oSchedule.OperationalTimeID;
                }
                else
                {
                    entity.HealthcareServiceUnitID = healthcareServiceUnitID;
                    entity.ParamedicID = paramedicID;
                    entity.ScheduleDate = Helper.GetDatePickerValue(scheduleDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    entity.OperationalTimeID = operationalTimeID;
                }
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.IsBPJS1 = oSchedule.IsBPJS1;
                entity.IsBPJS2 = oSchedule.IsBPJS2;
                entity.IsBPJS3 = oSchedule.IsBPJS3;
                entity.IsBPJS4 = oSchedule.IsBPJS4;
                entity.IsBPJS5 = oSchedule.IsBPJS5;
                entity.IsNonBPJS1 = oSchedule.IsNonBPJS1;
                entity.IsNonBPJS2 = oSchedule.IsNonBPJS2;
                entity.IsNonBPJS3 = oSchedule.IsNonBPJS3;
                entity.IsNonBPJS4 = oSchedule.IsNonBPJS4;
                entity.IsNonBPJS5 = oSchedule.IsNonBPJS5;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Changed Paramedic Leave Schedule Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnParamedicLeaveScheduleChanged(ParamedicLeaveSchedule oSchedule, int paramedicID, string startDate, string endDate, string startTime, string endTime, string eventType)
        {
            GetSettingParameter();
            try
            {
                string endpointpath = string.Format("{0}/paramedicschedule_leave_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/paramedicschedule/leave/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/paramedicschedule/leave/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                StoreParamedicLeaveScheduleChanged entity = new StoreParamedicLeaveScheduleChanged();
                entity.ID = oSchedule.ID;
                entity.ParamedicID = oSchedule.ParamedicID;
                entity.EventType = eventType;
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.StartDate = Helper.GetDatePickerValue(startDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.StartTime = startTime;
                entity.EndDate = Helper.GetDatePickerValue(endDate).ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.EndTime = endTime;
                entity.GCParamedicLeaveReason = oSchedule.GCParamedicLeaveReason;
                entity.LeaveOtherReason = oSchedule.LeaveOtherReason;
                entity.Remarks = oSchedule.Remarks;

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
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
        #endregion

        #region On Get Appointment Request
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnGetAppointmentRequest(string startDate, string endDate)
        {
            GetSettingParameter();
            string result = "";
            try
            {
                string endpointpath = string.Format("{0}/appointment_get.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/appointment/get", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/get", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                string apmFromDateYear = startDate.Substring(6, 4);
                string apmFromDateMonth = startDate.Substring(3, 2);
                string apmFromDateDay = startDate.Substring(0, 2);
                string apmFromDateFormat = string.Format("{0}-{1}-{2}", apmFromDateYear, apmFromDateMonth, apmFromDateDay);
                DateTime apmFromDate = DateTime.ParseExact(apmFromDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

                string apmToDateYear = endDate.Substring(6, 4);
                string apmToDateMonth = endDate.Substring(3, 2);
                string apmToDateDay = endDate.Substring(0, 2);
                string apmToDateFormat = string.Format("{0}-{1}-{2}", apmToDateYear, apmToDateMonth, apmToDateDay);
                DateTime apmToDate = DateTime.ParseExact(apmToDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

                MobileAppointmentRequestBody entity = new MobileAppointmentRequestBody();
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.StartDate = apmFromDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.EndDate = apmToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    MobileAppointmentRequestResponse responseApm = JsonConvert.DeserializeObject<MobileAppointmentRequestResponse>(result);
                    if (responseApm != null)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        AppointmentRequestDao entityDao = new AppointmentRequestDao(ctx);
                        AppointmentRequest entityApm = new AppointmentRequest();

                        try
                        {
                            if (responseApm.Code == "200")
                            {
                                List<MobileAppointmentRequestResponseData> lstDataApm = responseApm.Data;
                                if (lstDataApm.Count > 0)
                                {
                                    foreach (MobileAppointmentRequestResponseData a in lstDataApm)
                                    {
                                        //List<AppointmentRequest> lstChkData = BusinessLayer.GetAppointmentRequestList(string.Format("Remarks LIKE '%{0}%'", a.KeyID), ctx);
                                        //if (lstChkData.Count == 0)

                                        List<AppointmentRequest> chkDataLst = BusinessLayer.GetAppointmentRequestList(string.Format("Remarks LIKE '%{0}%'", a.KeyID), ctx);
                                        if (chkDataLst.Count() == 0)
                                        {
                                            entityApm.MRN = a.MRN;
                                            entityApm.HealthcareServiceUnitID = a.HealthcareServiceUnitID;
                                            entityApm.ParamedicID = a.ParamedicID;
                                            entityApm.VisitTypeID = a.VisitTypeID;
                                            entityApm.AppointmentDate = Convert.ToDateTime(a.StartDate);
                                            entityApm.AppointmentTime = a.StartTime;
                                            entityApm.GCAppointmentRequestMethod = Constant.AppointmentRequestMethod.MOBILE_APPS;
                                            if (!string.IsNullOrEmpty(a.GCCustomerType))
                                            {
                                                entityApm.GCCustomerType = a.GCCustomerType;
                                            }
                                            entityApm.Remarks = string.Format("KeyID^{0}", a.KeyID);
                                            entityApm.SessionRequest = a.Session;
                                            entityApm.CreatedBy = 0;
                                            entityApm.CreatedDate = Convert.ToDateTime(a.CreatedDate);

                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityApm.AppointmentRequestID = entityDao.InsertReturnPrimaryKeyID(entityApm);
                                        }
                                    }
                                }
                                ctx.CommitTransaction();

                                result = string.Format("1|{0}|{1}", data, JsonConvert.SerializeObject(responseApm));
                            }
                            else
                            {
                                result = string.Format("0|{0}|{1}", data, JsonConvert.SerializeObject(responseApm));
                            }
                        }
                        catch (Exception ex)
                        {
                            result = string.Format("0|{0}|{1}", data, ex.Message);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Get Cancel Appointment Request
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnGetCancelAppointmentRequest(string startDate, string endDate)
        {
            GetSettingParameter();
            string result = "";
            try
            {
                string endpointpath = string.Format("{0}/appointment_get_cancel.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/appointment/get/cancel", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/get/cancel", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                string apmFromDateYear = startDate.Substring(6, 4);
                string apmFromDateMonth = startDate.Substring(3, 2);
                string apmFromDateDay = startDate.Substring(0, 2);
                string apmFromDateFormat = string.Format("{0}-{1}-{2}", apmFromDateYear, apmFromDateMonth, apmFromDateDay);
                DateTime apmFromDate = DateTime.ParseExact(apmFromDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

                string apmToDateYear = endDate.Substring(6, 4);
                string apmToDateMonth = endDate.Substring(3, 2);
                string apmToDateDay = endDate.Substring(0, 2);
                string apmToDateFormat = string.Format("{0}-{1}-{2}", apmToDateYear, apmToDateMonth, apmToDateDay);
                DateTime apmToDate = DateTime.ParseExact(apmToDateFormat, Constant.FormatString.DATE_PICKER_FORMAT2, null);

                MobileAppointmentRequestBody entity = new MobileAppointmentRequestBody();
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.StartDate = apmFromDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.EndDate = apmToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

                string data = JsonConvert.SerializeObject(entity);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    MobileAppointmentRequestResponse responseApm = JsonConvert.DeserializeObject<MobileAppointmentRequestResponse>(result);
                    if (responseApm != null)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        AppointmentRequestDao entityDao = new AppointmentRequestDao(ctx);
                        AppointmentRequest entityApm = new AppointmentRequest();

                        try
                        {
                            if (responseApm.Code == "200")
                            {
                                List<MobileAppointmentRequestResponseData> lstDataApm = responseApm.Data;
                                if (lstDataApm.Count > 0)
                                {
                                    foreach (MobileAppointmentRequestResponseData a in lstDataApm)
                                    {
                                        //List<AppointmentRequest> lstChkData = BusinessLayer.GetAppointmentRequestList(string.Format("Remarks LIKE '%{0}%'", a.KeyID), ctx);
                                        //if (lstChkData.Count == 0)

                                        List<AppointmentRequest> chkDataLst = BusinessLayer.GetAppointmentRequestList(string.Format("Remarks LIKE '%{0}%'", a.KeyID), ctx);
                                        if (chkDataLst.Count() > 0)
                                        {
                                            foreach (AppointmentRequest apm in chkDataLst)
                                            {
                                                apm.IsRequestDeleted = true;
                                                apm.IsRequestDeletedByPatient = true;
                                                apm.RequestDeletedBy = a.MRN;
                                                apm.RequestDeletedDate = DateTime.Now;
                                                apm.GCRequestDeletedReason = Constant.DeleteReason.OTHER;
                                                apm.RequestDeletedReason = a.RequestDeleteReason;
                                                apm.LastUpdatedBy = 0;
                                                apm.LastUpdatedDate = DateTime.Now;

                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                entityDao.Update(apm);
                                            }
                                        }
                                    }
                                }
                                ctx.CommitTransaction();

                                result = string.Format("1|{0}|{1}", data, JsonConvert.SerializeObject(responseApm));
                            }
                            else
                            {
                                result = string.Format("0|{0}|{1}", data, JsonConvert.SerializeObject(responseApm));
                            }
                        }
                        catch (Exception ex)
                        {
                            result = string.Format("0|{0}|{1}", data, ex.Message);
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Save Appointment Request
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSaveAppointmentRequest(int appointmentRequestID)
        {
            GetSettingParameter();
            string result = "";

            string endpointpath = string.Format("{0}/appointment_store.php", url);
            if (!RemoteFileExists(endpointpath))
            {
                endpointpath = string.Format("{0}/appointment/store", url);
            }
            HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
            //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/store", url));
            updateRequest.Method = "POST";
            updateRequest.ContentType = "application/json";

            SetRequestHeader(updateRequest);

            AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequest(appointmentRequestID);
            Appointment entityApm = BusinessLayer.GetAppointment(Convert.ToInt32(entityApmReq.AppointmentID));
            if (entityApmReq.Remarks.Contains('^'))
            {
                string keyID = entityApmReq.Remarks.Split('^')[1];

                MobileUpdateAppointmentRequest entity = new MobileUpdateAppointmentRequest();
                entity.EventType = "002";
                entity.KeyID = keyID;
                if (entityApmReq.MRN != 0 && entityApmReq.MRN != null)
                {
                    entity.MRN = entityApmReq.MRN.ToString();
                }
                else
                {
                    entity.GuestID = entityApm.GuestID.ToString();
                }
                entity.StartDate = entityApm.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.ParamedicID = entityApm.ParamedicID.ToString();
                entity.HealthcareServiceUnitID = entityApmReq.HealthcareServiceUnitID.ToString();
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.AppointmentID = entityApmReq.AppointmentID.ToString();
                entity.AppointmentQueueNo = entityApm.QueueNo.ToString();
                entity.AppointmentSession = entityApm.Session.ToString();
                entity.AppointmentNo = entityApm.AppointmentNo;
                if (entityApmReq.RegistrationID != null)
                {
                    vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entityApmReq.RegistrationID)).FirstOrDefault();
                    entity.RegistrationNo = entityCV.RegistrationNo;
                    entity.RegistrationQueueNo = entityCV.QueueNo.ToString();
                    entity.RegistrationSession = entityCV.Session.ToString();
                }
                entity.IsRejected = "0";
                entity.QueueNo = entityApm.QueueNo.ToString();

                string data = JsonConvert.SerializeObject(entity);

                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    updateRequest.ContentLength = bytes.Length;
                    Stream putStream = updateRequest.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)updateRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    result = string.Format("0|{0}|{1}", data, ex.Message);
                    return result;
                }
            }
            else
            {
                result = string.Format("0|{0}|{1}", "", "Tidak bridging dengan mobile apps");
                return result;
            }
        }
        #endregion

        #region On Save Appointment Without Request
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSaveAppointmentWithoutRequest(string appointmentID)
        {
            GetSettingParameter();
            string result = "";

            string endpointpath = string.Format("{0}/appointment_storewithoutrequest.php", url);
            if (!RemoteFileExists(endpointpath))
            {
                endpointpath = string.Format("{0}/appointment/storewithoutrequest", url);
            }
            HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
            //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/storewithoutrequest", url));
            updateRequest.Method = "POST";
            updateRequest.ContentType = "application/json";

            SetRequestHeader(updateRequest);

            //AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequest(appointmentID);
            //Appointment entityApm = BusinessLayer.GetAppointment(Convert.ToInt32(appointmentID));
            string filterExpression = string.Format(" AppointmentNo = '{0}' ", appointmentID);

            vAppointment entityApm = BusinessLayer.GetvAppointmentList(filterExpression).FirstOrDefault();

                //string keyID = entityApmReq.Remarks.Split('^')[1];
                
                MobileAppointmentInfo entity = new MobileAppointmentInfo();
                entity.EventType = "002";
                //entity.KeyID = keyID;
                if (entityApm.MRN != 0 && entityApm.MRN != null)
                {
                    entity.MRN = entityApm.MRN.ToString();
                }
                else
                {
                    entity.GuestID = entityApm.GuestID.ToString();
                }
                entity.MedicalNo = entityApm.MedicalNo;
                entity.PatientName = entityApm.PatientName;
                entity.MobilePhoneNo1 = entityApm.MobilePhoneNo;
                entity.StartDate = entityApm.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                entity.StartTime = entityApm.StartTime;
                entity.EndTime = entityApm.EndTime;
                entity.ParamedicID = entityApm.ParamedicID.ToString();
                entity.ParamedicName = entityApm.ParamedicName;
                entity.HealthcareServiceUnitID = entityApm.HealthcareServiceUnitID.ToString();
                entity.HealthcareGroup = healthcareGroup;
                entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.ServiceUnitName = entityApm.ServiceUnitName;
                entity.AppointmentID = entityApm.AppointmentID.ToString();
                entity.AppointmentQueueNo = entityApm.QueueNo.ToString();
                entity.AppointmentSession = entityApm.Session.ToString();
                entity.AppointmentNo = entityApm.AppointmentNo;
                entity.GCAppointmentMethod = entityApm.GCAppointmentMethod;
                //if (entityApmReq.RegistrationID != null)
                //{
                //    vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", entityApmReq.RegistrationID)).FirstOrDefault();
                //    entity.RegistrationNo = entityCV.RegistrationNo;
                //    entity.RegistrationQueueNo = entityCV.QueueNo.ToString();
                //    entity.RegistrationSession = entityCV.Session.ToString();
                //}
                entity.IsRejected = "0";
                entity.QueueNo = entityApm.QueueNo.ToString();

                string data = JsonConvert.SerializeObject(entity);

                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    updateRequest.ContentLength = bytes.Length;
                    Stream putStream = updateRequest.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)updateRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                    }
                    return result;
                }
                catch (Exception ex)
                {
                    result = string.Format("0|{0}|{1}", data, ex.Message);
                    return result;
                }

        }
        #endregion


        #region On Reject Appointment Request
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnRejectAppointmentRequest(int appointmentRequestID)
        {
            GetSettingParameter();
            string result = "";

            string endpointpath = string.Format("{0}/appointment_store.php", url);
            if (!RemoteFileExists(endpointpath))
            {
                endpointpath = string.Format("{0}/appointment/store", url);
            }
            HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
            //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/store", url));
            updateRequest.Method = "POST";
            updateRequest.ContentType = "application/json";

            SetRequestHeader(updateRequest);

            vAppointmentRequest entityApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = {0}", appointmentRequestID)).FirstOrDefault();
            Appointment entityApm = BusinessLayer.GetAppointment(Convert.ToInt32(entityApmReq.AppointmentID));
            string keyID = entityApmReq.Remarks.Split('^')[1];

            MobileUpdateAppointmentRequest entity = new MobileUpdateAppointmentRequest();
            entity.EventType = "003";
            entity.KeyID = keyID;
            entity.MRN = entityApmReq.MRN.ToString();
            entity.StartDate = entityApmReq.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            entity.ParamedicID = entityApmReq.ParamedicID.ToString();
            entity.HealthcareServiceUnitID = entityApmReq.HealthcareServiceUnitID.ToString();
            entity.HealthcareGroup = healthcareGroup;
            entity.HealthcareID = AppSession.UserLogin.HealthcareID;
            if (entityApmReq.AppointmentID != 0)
            {
                entity.AppointmentID = entityApmReq.AppointmentID.ToString();
                entity.AppointmentQueueNo = entityApm.QueueNo.ToString();
                entity.AppointmentSession = entityApm.Session.ToString();
                entity.AppointmentNo = entityApm.AppointmentNo;
                entity.QueueNo = entityApm.QueueNo.ToString();
            }
            entity.RegistrationNo = string.Empty;
            entity.IsRejected = "1";
            entity.RejectedReason = string.Format("{0}|{1}", entityApmReq.GCDeleteReason, entityApmReq.DeleteReason);
            entity.LastUpdatedBy = AppSession.UserLogin.UserID.ToString();

            string data = JsonConvert.SerializeObject(entity);

            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", data, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Void Appointment
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnVoidAppointment(int appointmentID)
        {
            GetSettingParameter();
            string result = "";
            try
            {
                string endpointpath = string.Format("{0}/appointment_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/appointment/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                vAppointmentRequest entityApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentID = {0}", appointmentID)).FirstOrDefault();
                if (entityApmReq != null)
                {
                    Appointment entityApm = BusinessLayer.GetAppointment(Convert.ToInt32(entityApmReq.AppointmentID));
                    string keyID = entityApmReq.Remarks.Split('^')[1];

                    MobileUpdateAppointmentRequest entity = new MobileUpdateAppointmentRequest();
                    entity.EventType = "003";
                    entity.KeyID = keyID;
                    entity.MRN = entityApmReq.MRN.ToString();
                    entity.StartDate = entityApmReq.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    entity.ParamedicID = entityApmReq.ParamedicID.ToString();
                    entity.HealthcareServiceUnitID = entityApmReq.HealthcareServiceUnitID.ToString();
                    entity.HealthcareGroup = healthcareGroup;
                    entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                    entity.AppointmentID = entityApm.AppointmentID.ToString();
                    entity.AppointmentQueueNo = entityApm.QueueNo.ToString();
                    entity.AppointmentSession = entityApm.Session.ToString();
                    entity.AppointmentNo = entityApm.AppointmentNo;
                    entity.QueueNo = entityApm.QueueNo.ToString();
                    entity.RegistrationNo = string.Empty;
                    entity.IsRejected = "1";
                    entity.RejectedReason = string.Format("{0}|{1}", entityApm.GCDeleteReason, entityApm.DeleteReason);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID.ToString();

                    string data = JsonConvert.SerializeObject(entity);

                    try
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        updateRequest.ContentLength = bytes.Length;
                        Stream putStream = updateRequest.GetRequestStream();
                        putStream.Write(bytes, 0, bytes.Length);
                        putStream.Close();
                        WebResponse response = (WebResponse)updateRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result = string.Format("0|{0}|{1}", data, ex.Message);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Registration From Appointment
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnRegistrationFromAppointment(int appointmentRequestID)
        {
            GetSettingParameter();
            string result = "";
            try
            {
                string endpointpath = string.Format("{0}/appointment_store.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/appointment/store", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                vAppointmentRequest entityApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = {0}", appointmentRequestID)).FirstOrDefault();
                if (entityApmReq != null)
                {
                    Appointment entityApm = BusinessLayer.GetAppointment(Convert.ToInt32(entityApmReq.AppointmentID));
                    string keyID = entityApmReq.Remarks.Split('^')[1];
                    vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("AppointmentID = {0}", entityApmReq.AppointmentID)).FirstOrDefault();

                    MobileUpdateAppointmentRequest entity = new MobileUpdateAppointmentRequest();
                    entity.EventType = "002";
                    entity.KeyID = keyID;
                    entity.MRN = entityCV.MRN.ToString();
                    //entity.MRN = entityApmReq.MRN.ToString();
                    entity.StartDate = entityApmReq.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
                    entity.ParamedicID = entityApmReq.ParamedicID.ToString();
                    entity.HealthcareServiceUnitID = entityApmReq.HealthcareServiceUnitID.ToString();
                    entity.HealthcareGroup = healthcareGroup;
                    entity.HealthcareID = AppSession.UserLogin.HealthcareID;
                    entity.AppointmentID = entityApm.AppointmentID.ToString();
                    entity.AppointmentQueueNo = entityApm.QueueNo.ToString();
                    entity.AppointmentSession = entityApm.Session.ToString();
                    entity.AppointmentNo = entityApm.AppointmentNo;
                    entity.QueueNo = entityApm.QueueNo.ToString();
                    entity.RegistrationNo = entityCV.RegistrationNo;
                    entity.RegistrationQueueNo = entityCV.QueueNo.ToString();
                    entity.RegistrationSession = entityCV.Session.ToString();
                    entity.RoomName = entityCV.RoomName;
                    entity.IsRejected = "0";
                    entity.RejectedReason = string.Empty;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID.ToString();

                    string data = JsonConvert.SerializeObject(entity);

                    try
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        updateRequest.ContentLength = bytes.Length;
                        Stream putStream = updateRequest.GetRequestStream();
                        putStream.Write(bytes, 0, bytes.Length);
                        putStream.Close();
                        WebResponse response = (WebResponse)updateRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result = string.Format("0|{0}|{1}", data, ex.Message);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region On Registration Without Appointment
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnRegistrationWithoutAppointment(int registrationID)
        {
            GetSettingParameter();
            string result = "";
            try
            {
                string endpointpath = string.Format("{0}/registration_infotowhatsapp.php", url);
                if (!RemoteFileExists(endpointpath))
                {
                    endpointpath = string.Format("{0}/registration/infotowhatsapp", url);
                }
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(endpointpath);
                //HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/appointment/store", url));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                SetRequestHeader(updateRequest);

                string filterExpression = string.Format(" RegistrationID = '{0}'", registrationID);
                vRegistration entityReg = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();

                if (entityReg != null)
                {
                    MobileRegistrationInfo entity = new MobileRegistrationInfo();
                    //entity.EventType = "002";
                    entity.RegistrationNo = entityReg.RegistrationNo;
                    entity.AppointmentID = entityReg.AppointmentID;
                    entity.AppointmentNo = entityReg.AppointmentNo;
                    entity.IsNewPatient = Convert.ToInt16(entityReg.IsNewPatient);
                    entity.MRN = entityReg.MRN;
                    entity.MedicalNo = entityReg.MedicalNo;
                    entity.OldMedicalNo = entityReg.OldMedicalNo;
                    entity.GuestID = entityReg.GuestID;
                    entity.PatientName = entityReg.PatientName;
                    entity.MobilePhoneNo1 = entityReg.MobilePhoneNo1;
                    entity.EmailAddress = entityReg.EmailAddress;
                    entity.HealthcareServiceUnitID = entityReg.HealthcareServiceUnitID;
                    entity.ServiceUnitCode = entityReg.ServiceUnitCode;
                    entity.ServiceUnitName = entityReg.ServiceUnitName;
                    entity.ParamedicID = entityReg.ParamedicID;
                    entity.ParamedicCode = entityReg.ParamedicCode;
                    entity.ParamedicName = entityReg.ParamedicName;
                    entity.SpecialtyID = entityReg.SpecialtyID;
                    entity.SpecialtyName = entityReg.SpecialtyName;
                    entity.Session = entityReg.Session;
                    entity.QueueNo = entityReg.QueueNo;
                    entity.RegistrationTicketNo = entityReg.RegistrationTicketNo;
                    entity.RoomID = entityReg.RoomID;
                    entity.RoomCode = entityReg.RoomCode;
                    entity.RoomName = entityReg.RoomName;
                    entity.VisitTypeID = entityReg.VisitTypeID;
                    entity.VisitTypeCode = entityReg.VisitTypeCode;
                    entity.VisitTypeName = entityReg.VisitTypeName;
                    entity.GCCustomerType = entityReg.GCCustomerType;
                    entity.CustomerType = entityReg.CustomerType;
                    entity.BusinessPartnerName = entityReg.BusinessPartnerName;
                    entity.GCRegistrationStatus = entityReg.GCRegistrationStatus;
                    entity.RegistrationStatus = entityReg.RegistrationStatus;
                    entity.GCVoidReason = entityReg.GCVoidReason;
                    entity.VoidReason = entityReg.VoidReason;

                    string data = JsonConvert.SerializeObject(entity);

                    try
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(data);
                        updateRequest.ContentLength = bytes.Length;
                        Stream putStream = updateRequest.GetRequestStream();
                        putStream.Write(bytes, 0, bytes.Length);
                        putStream.Close();
                        WebResponse response = (WebResponse)updateRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                        {
                            result = string.Format("1|{0}|{1}", data, sr.ReadToEnd());
                        }
                    }
                    catch (Exception ex)
                    {
                        result = string.Format("0|{0}|{1}", data, ex.Message);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", string.Empty, ex.Message);
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

            SettingParameterDt entityConsID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_CONS_ID);
            SettingParameterDt entityPassID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_PASS_ID);

            string consID = entityConsID.ParameterValue;
            string pass = entityPassID.ParameterValue;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-cons-id", consID);
            Request.Headers.Add("X-timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        private string GenerateSignature(string data, string secretKey)
        {
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            var encodedSignature = Convert.ToBase64String(signature);

            return encodedSignature;
        }

        private bool RemoteFileExists(string url)
        {
            try
            {
                //Creating the HttpWebRequest
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //Setting the Request method HEAD, you can also use GET too.
                request.Method = "HEAD";
                //Getting the Web Response.
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //Returns TRUE if the Status code == 200
                response.Close();
                return (response.StatusCode == HttpStatusCode.OK);
            }
            catch
            {
                //Any exception will returns false.
                return false;
            }
        }
        #endregion
    }
}
