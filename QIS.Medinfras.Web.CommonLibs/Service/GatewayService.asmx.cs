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
    public class GatewayService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";
        SettingParameterDt url = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.URL_WEB_API);
        SettingParameterDt auth = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_USING_AUTHENTICATION);

        #region GET METHOD

        #region SendParamedicInformation
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendParamedicInformation(ParamedicMaster oPhysician)
        {
            string result = "";
            try
            {
                HttpWebRequest GetRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}", url, oPhysician.ParamedicCode));
                GetRequest.Method = "GET";
                if (Constant.SettingParameter.IS_USING_AUTHENTICATION == "1")
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
                        result = string.Format("{0}|{1}|{2}", "1", result, respInfo.metadata.message);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "1", "null", respInfo.metadata.message);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #endregion

        #region POST METHOD

        #region Jadwal Dokter Rutin

        #region Push Physician Schedule Changed Event
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianScheduleChanged(string mode, vParamedicSchedule1 oPhysician)
        {
            try
            {
                #region Convert into DTO Objects
                ParamedicScheduleDTO oData = new ParamedicScheduleDTO();
                oData.DayNumber = oPhysician.DayNumber;
                oData.ParamedicID = oPhysician.ParamedicID;
                oData.ParamedicName = oPhysician.ParamedicName;
                oData.SpecialtyID = oPhysician.SpecialtyID;
                oData.SpecialtyName = oPhysician.SpecialtyName;
                oData.ServiceUnitID = oPhysician.ServiceUnitID;
                oData.ServiceUnitName = oPhysician.ServiceUnitName;
                oData.RoomID = oPhysician.RoomID;
                oData.RoomName = oPhysician.RoomName;
                oData.OperationalTimeID = oPhysician.OperationalTimeID;
                oData.OperationalTimeCode = oPhysician.OperationalTimeCode;
                oData.OperationalTimeName = oPhysician.OperationalTimeName;
                oData.StartTime1 = oPhysician.StartTime1;
                oData.StartTime2 = oPhysician.StartTime2;
                oData.StartTime3 = oPhysician.StartTime3;
                oData.StartTime4 = oPhysician.StartTime4;
                oData.StartTime5 = oPhysician.StartTime5;
                oData.EndTime1 = oPhysician.EndTime1;
                oData.EndTime2 = oPhysician.EndTime2;
                oData.EndTime3 = oPhysician.EndTime3;
                oData.EndTime4 = oPhysician.EndTime4;
                oData.EndTime5 = oPhysician.EndTime5;
                oData.IsAppointmentByTimeSlot1 = oPhysician.IsAppointmentByTimeSlot1;
                oData.IsAppointmentByTimeSlot2 = oPhysician.IsAppointmentByTimeSlot2;
                oData.IsAppointmentByTimeSlot3 = oPhysician.IsAppointmentByTimeSlot3;
                oData.IsAppointmentByTimeSlot4 = oPhysician.IsAppointmentByTimeSlot4;
                oData.IsAppointmentByTimeSlot5 = oPhysician.IsAppointmentByTimeSlot5;
                oData.MaximumAppointment1 = oPhysician.MaximumAppointment1;
                oData.MaximumAppointment2 = oPhysician.MaximumAppointment2;
                oData.MaximumAppointment3 = oPhysician.MaximumAppointment3;
                oData.MaximumAppointment4 = oPhysician.MaximumAppointment4;
                oData.MaximumAppointment5 = oPhysician.MaximumAppointment5;
                oData.IsAllowWaitingList1 = oPhysician.IsAllowWaitingList1;
                oData.IsAllowWaitingList2 = oPhysician.IsAllowWaitingList2;
                oData.IsAllowWaitingList3 = oPhysician.IsAllowWaitingList3;
                oData.IsAllowWaitingList4 = oPhysician.IsAllowWaitingList4;
                oData.IsAllowWaitingList5 = oPhysician.IsAllowWaitingList5;
                oData.MaximumWaitingList1 = oPhysician.MaximumWaitingList1;
                oData.MaximumWaitingList2 = oPhysician.MaximumWaitingList2;
                oData.MaximumWaitingList3 = oPhysician.MaximumWaitingList3;
                oData.MaximumWaitingList4 = oPhysician.MaximumWaitingList4;
                oData.MaximumWaitingList5 = oPhysician.MaximumWaitingList5;

                List<vParamedicSchedule1> lstParamedicSch = new List<vParamedicSchedule1>();
                vParamedicSchedule1 oParamedicSch = oPhysician;
                lstParamedicSch.Add(oParamedicSch);
                #endregion

                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                EventMessage eventInfo = new EventMessage();
                string eventCode = EventCode.SCH_001;
                string remarks = "";
                switch (mode)
                {
                    case "CREATE" :
                        eventCode = EventCode.SCH_001;
                        remarks = string.Format("New Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    case "UPDATE" :
                        eventCode = EventCode.SCH_002;
                        remarks = string.Format("Update Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    case "DELETE" :
                        eventCode = EventCode.SCH_003;
                        remarks = string.Format("Delete Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    default:
                        break;
                }

                eventInfo.EventCode = eventCode;
                eventInfo.Remarks = remarks;
                eventInfo.Data = JsonConvert.SerializeObject(oData);
                
                string data = JsonConvert.SerializeObject(eventInfo);

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

        #endregion

        #region Jadwal Dokter Per Tanggal

        #region Push Physician Schedule with Date Changed Event
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianScheduleWithDateChanged(string mode, vParamedicScheduleDate1 oPhysician)
        {
            try
            {
                #region Convert into DTO Objects
                ParamedicScheduleWithDateDTO oData = new ParamedicScheduleWithDateDTO();
                oData.ScheduleDate = oPhysician.ScheduleDate;
                oData.ParamedicID = oPhysician.ParamedicID;
                oData.ParamedicName = oPhysician.ParamedicName;
                oData.SpecialtyID = oPhysician.SpecialtyID;
                oData.SpecialtyName = oPhysician.SpecialtyName;
                oData.ServiceUnitID = oPhysician.ServiceUnitID;
                oData.ServiceUnitName = oPhysician.ServiceUnitName;
                oData.RoomID = oPhysician.RoomID;
                oData.RoomName = oPhysician.RoomName;
                oData.OperationalTimeID = oPhysician.OperationalTimeID;
                oData.OperationalTimeCode = oPhysician.OperationalTimeCode;
                oData.OperationalTimeName = oPhysician.OperationalTimeName;
                oData.StartTime1 = oPhysician.StartTime1;
                oData.StartTime2 = oPhysician.StartTime2;
                oData.StartTime3 = oPhysician.StartTime3;
                oData.StartTime4 = oPhysician.StartTime4;
                oData.StartTime5 = oPhysician.StartTime5;
                oData.EndTime1 = oPhysician.EndTime1;
                oData.EndTime2 = oPhysician.EndTime2;
                oData.EndTime3 = oPhysician.EndTime3;
                oData.EndTime4 = oPhysician.EndTime4;
                oData.EndTime5 = oPhysician.EndTime5;
                oData.IsAppointmentByTimeSlot1 = oPhysician.IsAppointmentByTimeSlot1;
                oData.IsAppointmentByTimeSlot2 = oPhysician.IsAppointmentByTimeSlot2;
                oData.IsAppointmentByTimeSlot3 = oPhysician.IsAppointmentByTimeSlot3;
                oData.IsAppointmentByTimeSlot4 = oPhysician.IsAppointmentByTimeSlot4;
                oData.IsAppointmentByTimeSlot5 = oPhysician.IsAppointmentByTimeSlot5;
                oData.MaximumAppointment1 = oPhysician.MaximumAppointment1;
                oData.MaximumAppointment2 = oPhysician.MaximumAppointment2;
                oData.MaximumAppointment3 = oPhysician.MaximumAppointment3;
                oData.MaximumAppointment4 = oPhysician.MaximumAppointment4;
                oData.MaximumAppointment5 = oPhysician.MaximumAppointment5;
                oData.IsAllowWaitingList1 = oPhysician.IsAllowWaitingList1;
                oData.IsAllowWaitingList2 = oPhysician.IsAllowWaitingList2;
                oData.IsAllowWaitingList3 = oPhysician.IsAllowWaitingList3;
                oData.IsAllowWaitingList4 = oPhysician.IsAllowWaitingList4;
                oData.IsAllowWaitingList5 = oPhysician.IsAllowWaitingList5;
                oData.MaximumWaitingList1 = oPhysician.MaximumWaitingList1;
                oData.MaximumWaitingList2 = oPhysician.MaximumWaitingList2;
                oData.MaximumWaitingList3 = oPhysician.MaximumWaitingList3;
                oData.MaximumWaitingList4 = oPhysician.MaximumWaitingList4;
                oData.MaximumWaitingList5 = oPhysician.MaximumWaitingList5;

                List<vParamedicScheduleDate1> lstParamedicSch = new List<vParamedicScheduleDate1>();
                vParamedicScheduleDate1 oParamedicSch = oPhysician;
                lstParamedicSch.Add(oParamedicSch);
                #endregion

                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                EventMessage eventInfo = new EventMessage();
                string eventCode = EventCode.SCH_013;
                string remarks = "";
                switch (mode)
                {
                    case "CREATE":
                        eventCode = EventCode.SCH_013;
                        remarks = string.Format("New Physician {0} Schedule With Date : {1}", oPhysician.ParamedicName, oPhysician.ScheduleDate);
                        break;
                    case "UPDATE":
                        eventCode = EventCode.SCH_014;
                        remarks = string.Format("Update Physician {0} Schedule With Date : {1}", oPhysician.ParamedicName, oPhysician.ScheduleDate);
                        break;
                    case "DELETE":
                        eventCode = EventCode.SCH_015;
                        remarks = string.Format("Delete Physician {0} Schedule With Date : {1}", oPhysician.ParamedicName, oPhysician.ScheduleDate);
                        break;
                    default:
                        break;
                }

                eventInfo.EventCode = eventCode;
                eventInfo.Remarks = remarks;
                eventInfo.Data = JsonConvert.SerializeObject(oData);

                string data = JsonConvert.SerializeObject(eventInfo);

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

        #endregion

        #region Cuti Dokter

        #region Push Physician On-Leave Schedule Changed Event
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnLeavePhysicianScheduleChanged(string mode, vParamedicLeaveSchedule1 oPhysician)
        {
            try
            {
                #region Convert into DTO Objects
                ParamedicLeaveScheduleDTO oData = new ParamedicLeaveScheduleDTO();
                oData.ID = oPhysician.ID;
                oData.ParamedicID = oPhysician.ParamedicID;
                oData.ParamedicName = oPhysician.ParamedicName;
                oData.SpecialtyID = oPhysician.SpecialtyID;
                oData.SpecialtyName = oPhysician.SpecialtyName;
                oData.StartDate = oPhysician.StartDate;
                oData.StartTime = oPhysician.StartTime;
                oData.EndDate = oPhysician.EndDate;
                oData.EndTime = oPhysician.EndTime;
                oData.IsFullDay = oPhysician.IsFullDay;
                oData.GCParamedicLeaveReason = oPhysician.GCParamedicLeaveReason;
                oData.Remarks = oPhysician.Remarks;

                List<vParamedicLeaveSchedule1> lstParamedicSch = new List<vParamedicLeaveSchedule1>();
                vParamedicLeaveSchedule1 oParamedicSch = oPhysician;
                lstParamedicSch.Add(oParamedicSch);
                #endregion

                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                EventMessage eventInfo = new EventMessage();
                string eventCode = EventCode.SCH_004;
                string remarks = "";
                switch (mode)
                {
                    case "CREATE":
                        eventCode = EventCode.SCH_004;
                        remarks = string.Format("New On-Leave Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    case "UPDATE":
                        eventCode = EventCode.SCH_005;
                        remarks = string.Format("Update On-Leave Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    case "DELETE":
                        eventCode = EventCode.SCH_006;
                        remarks = string.Format("Delete On-Leave Physician {0} Schedule Information", oPhysician.ParamedicName);
                        break;
                    default:
                        break;
                }

                eventInfo.EventCode = eventCode;
                eventInfo.Remarks = remarks;
                eventInfo.Data = JsonConvert.SerializeObject(oData);

                string data = JsonConvert.SerializeObject(eventInfo);

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

        #endregion

        #region Appointment

        #region On Changed Appointment Event
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnChangedAppointmentInformation(string mode, List<vAppointment> lstAppo)
        {
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                EventMessage eventInfo = new EventMessage();
                string eventCode = EventCode.SCH_007;
                string remarks = "";
                switch (mode)
                {
                    case "NEW":
                        eventCode = EventCode.SCH_007;
                        remarks = string.Format("New Appointment for {0} at {1} ({2})", lstAppo.FirstOrDefault().PatientName, lstAppo.FirstOrDefault().StartDate, lstAppo.FirstOrDefault().StartTime);
                        break;
                    case "UPDATE":
                        eventCode = EventCode.SCH_008;
                        remarks = string.Format("Update Appointment for {0} from {1} ({2}) to {3} ({4})", lstAppo.FirstOrDefault().PatientName, lstAppo.FirstOrDefault().StartDate, lstAppo.FirstOrDefault().StartTime, lstAppo.FirstOrDefault().EndDate, lstAppo.FirstOrDefault().EndTime);
                        break;
                    case "VOID":
                        eventCode = EventCode.SCH_009;
                        remarks = string.Format("Void Appointment for {0} with void reason = {1}", lstAppo.FirstOrDefault().PatientName, lstAppo.FirstOrDefault().DeleteReason);
                        break;
                    case "REOPEN":
                        eventCode = EventCode.SCH_010;
                        remarks = string.Format("Re-open appointment for {0}", lstAppo.FirstOrDefault().PatientName);
                        break;
                    case "ALLVOID":
                        eventCode = EventCode.SCH_011;
                        remarks = string.Format("Void all patient appointment for {0}", lstAppo.FirstOrDefault().ParamedicName);
                        break;
                    case "APPOFROM":
                        eventCode = EventCode.SCH_012;
                        remarks = string.Format("Reschedule Appointment from {0}", lstAppo.FirstOrDefault().ParamedicName);
                        break;
                    default:
                        break;
                }

                eventInfo.EventCode = eventCode;
                eventInfo.Remarks = remarks;

                List<AppointmentDTO> oDataList = new List<AppointmentDTO>();

                foreach (vAppointment oAppointment in lstAppo)
                {
                    #region Convert into DTO Objects
                    AppointmentDTO oData = new AppointmentDTO();
                    oData.AppointmentID = oAppointment.AppointmentID;
                    oData.AppointmentNo = oAppointment.AppointmentNo;
                    oData.GCAppointmentStatus = oAppointment.GCAppointmentStatus;
                    oData.AppointmentStatus = oAppointment.AppointmentStatus;
                    oData.QueueNo = oAppointment.QueueNo;
                    oData.ParamedicID = oAppointment.ParamedicID;
                    oData.ParamedicCode = oAppointment.ParamedicCode;
                    oData.ParamedicName = oAppointment.ParamedicName;
                    oData.ServiceUnitID = oAppointment.ServiceUnitID;
                    oData.ServiceUnitCode = oAppointment.ServiceUnitCode;
                    oData.ServiceUnitName = oAppointment.ServiceUnitName;
                    oData.PatientName = oAppointment.PatientName;
                    oData.MedicalNo = oAppointment.MedicalNo;
                    oData.StartDate = oAppointment.StartDate;
                    oData.StartTime = oAppointment.StartTime;
                    oData.EndDate = oAppointment.EndDate;
                    oData.EndTime = oAppointment.EndTime;
                    oData.VisitTypeCode = oAppointment.VisitTypeCode;
                    oData.VisitTypeName = oAppointment.VisitTypeName;
                    oData.DeleteReason = oAppointment.DeleteReason;

                    //List<vAppointment> lstAppointment = new List<vAppointment>();
                    //vAppointment oChangedAppointment = oAppointment;
                    //lstAppointment.Add(oChangedAppointment);
                    #endregion

                    if (auth.ParameterValue == "1")
                    {
                        SetRequestHeader(updateRequest);
                    }

                    oDataList.Add(oData);
                }

                eventInfo.Data = JsonConvert.SerializeObject(oDataList);
                string data = JsonConvert.SerializeObject(eventInfo);

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

        #endregion

        #region RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA

        #region Update Jadwal
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianRoutineSchedule(string jenisJadwal, vParamedicSchedule1 oPhysician)
        {
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/jadwal/update", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                PhysicianScheduleUpdate physicianScheduleUpdate = new PhysicianScheduleUpdate();
                physicianScheduleUpdate.jenisJadwal = jenisJadwal;
                physicianScheduleUpdate.serviceUnitID = oPhysician.ServiceUnitID.ToString();
                physicianScheduleUpdate.physicianID = oPhysician.ParamedicID.ToString();
                physicianScheduleUpdate.user = AppSession.UserLogin.UserName;

                string data = JsonConvert.SerializeObject(physicianScheduleUpdate);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = string.Format("1|{0}|{1}", sr.ReadToEnd(), data);
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianScheduleWithDate(string jenisJadwal, vParamedicScheduleDate1 oPhysician)
        {
            try
            {

                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/jadwal/update", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                PhysicianScheduleUpdate physicianScheduleUpdate = new PhysicianScheduleUpdate();
                physicianScheduleUpdate.jenisJadwal = jenisJadwal;
                physicianScheduleUpdate.serviceUnitID = oPhysician.ServiceUnitID.ToString();
                physicianScheduleUpdate.physicianID = oPhysician.ParamedicID.ToString();
                physicianScheduleUpdate.user = AppSession.UserLogin.UserName;

                string data = JsonConvert.SerializeObject(physicianScheduleUpdate);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = string.Format("1|{0}|{1}", sr.ReadToEnd(), data);
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnLeavePhysicianSchedule(string jenisJadwal, string healthcareServiceUnitID, vParamedicLeaveSchedule1 oPhysician)
        {
            try
            {
                #region Convert into DTO Objects
                ParamedicLeaveScheduleDTO oData = new ParamedicLeaveScheduleDTO();
                oData.ID = oPhysician.ID;
                oData.ParamedicID = oPhysician.ParamedicID;
                oData.ParamedicName = oPhysician.ParamedicName;
                oData.SpecialtyID = oPhysician.SpecialtyID;
                oData.SpecialtyName = oPhysician.SpecialtyName;
                oData.StartDate = oPhysician.StartDate;
                oData.StartTime = oPhysician.StartTime;
                oData.EndDate = oPhysician.EndDate;
                oData.EndTime = oPhysician.EndTime;
                oData.IsFullDay = oPhysician.IsFullDay;
                oData.GCParamedicLeaveReason = oPhysician.GCParamedicLeaveReason;
                oData.Remarks = oPhysician.Remarks;

                List<vParamedicLeaveSchedule1> lstParamedicSch = new List<vParamedicLeaveSchedule1>();
                vParamedicLeaveSchedule1 oParamedicSch = oPhysician;
                lstParamedicSch.Add(oParamedicSch);
                #endregion

                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/jadwal/update", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                PhysicianScheduleUpdate physicianScheduleUpdate = new PhysicianScheduleUpdate();
                physicianScheduleUpdate.jenisJadwal = jenisJadwal;
                physicianScheduleUpdate.serviceUnitID = healthcareServiceUnitID;
                physicianScheduleUpdate.physicianID = oPhysician.ParamedicID.ToString();
                physicianScheduleUpdate.user = AppSession.UserLogin.UserName;

                string data = JsonConvert.SerializeObject(physicianScheduleUpdate);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = string.Format("1|{0}|{1}",sr.ReadToEnd(), data);
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Notifikasi Buka Tutup Klinik
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnChangeClinicStatus(string jenisNotif, string queueNo, string hsuID, string paramedicID, string pauseReason)
        {
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/notifikasi/send", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                ClinicStatusNotificationChanged clinicChangeStatus = new ClinicStatusNotificationChanged();
                clinicChangeStatus.jenisNotif = jenisNotif;
                clinicChangeStatus.noAntrian = queueNo;
                clinicChangeStatus.serviceUnitID = hsuID;
                clinicChangeStatus.physicianID = paramedicID;
                clinicChangeStatus.user = AppSession.UserLogin.UserName;

                string data = JsonConvert.SerializeObject(clinicChangeStatus);

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

        #region Kirim Stream PDF
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendStreamPDF(string pdf, string filename)
        {
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/stream/pdf", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                SendPdfStream sendPdf = new SendPdfStream();
                sendPdf.stream = pdf;
                sendPdf.filename = filename;

                string data = JsonConvert.SerializeObject(sendPdf);

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

        #region Kirim Notifikasi Registrasi
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendNotificationRegistration(int RegistrationID)
        {
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/registrasi/save", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeader(updateRequest);
                }

                string result = "";
                vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", RegistrationID)).FirstOrDefault();
                if (entityReg != null)
                { 
                    SendNotificationRegistration sendNotif = new SendNotificationRegistration();
                    sendNotif.appointmentID = entityReg.AppointmentID;
                    sendNotif.appointmentNo = entityReg.AppointmentNo;
                    sendNotif.registrationNo = entityReg.RegistrationNo;
                    sendNotif.user = AppSession.UserLogin.UserName;

                    string data = JsonConvert.SerializeObject(sendNotif);

                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    updateRequest.ContentLength = bytes.Length;
                    Stream putStream = updateRequest.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)updateRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        //result = sr.ReadToEnd();
                        result = string.Format("{0}|{1}", "1", sr.ReadToEnd());
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}", "0", "Tidak ada yang dikirim");
                }
                return result;
            }
            catch (Exception ex)
            {
                return string.Format("0|{0}", ex.Message);
            }
        }
        #endregion

        #endregion

        #region RUMAH SAKIT DR. OEN SOLO BARU

        #region Get Nomor Antrian dari Engine Antrian RSDOSOBA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetQueueNo(string medicalNo, string paramedicCode, string customerType, DateTime date, string hour, string via, string healthcareServiceUnitID, int session)
        {
            string queue = string.Empty;
            string result = string.Empty;
            string data = string.Empty;
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/antrian", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeaderRSDOSOBA(updateRequest);
                }

                #region Convert To DTO
                GetQueueBodyRequest body = new GetQueueBodyRequest();
                body.rm = entityPatient.MedicalNo;
                body.nama = entityPatient.FullName;
                body.noktp = !string.IsNullOrEmpty(entityPatient.SSN) ? entityPatient.SSN : string.Empty;
                body.nobpjs = !string.IsNullOrEmpty(entityPatient.NHSRegistrationNo) ? entityPatient.NHSRegistrationNo : string.Empty;
                body.tgllahir = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
                if (entityPatient.GCGender == Constant.Gender.FEMALE)
                {
                    body.jnskelamin = "F";
                }
                else
                {
                    body.jnskelamin = "L";
                }
                body.str = paramedicCode;
                if (customerType == Constant.CustomerType.PERSONAL)
                {
                    body.tipe = "3";
                }
                else if (customerType == Constant.CustomerType.BPJS)
                {
                    body.tipe = "9";
                }
                else
                {
                    body.tipe = "2";
                }
                body.tanggal = date.ToString(Constant.FormatString.DATE_FORMAT_112);

                #region Validation Schedule
                vParamedicScheduleDate entityScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", paramedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                if (entityScheduleDate != null)
                {
                    body.kodejam = entityScheduleDate.OperationalTimeCode;
                }
                else
                {
                    vParamedicSchedule entitySchedule = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND DayNumber = (DATEPART(dw, '{2}') + 5) % 7 + 1", paramedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                    if (entitySchedule != null)
                    {
                        body.kodejam = entitySchedule.OperationalTimeCode;
                    }
                }
                #endregion
                body.sesi = session;
                body.via = via;
                #endregion

                data = new JavaScriptSerializer().Serialize(body);

                //string data = JsonConvert.SerializeObject(body);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    GetQueueResponse queueResponse = JsonConvert.DeserializeObject<GetQueueResponse>(result);
                    if (queueResponse.status == "100")
                    {
                        result = string.Format("1|{0}|{1}", queueResponse.antrian, data);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", queueResponse.message, data);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, data);
            }

            return result;
        }
        #endregion

        #region Delete Antrian dari Engine Antrian RSDOSOBA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetQueueNoByVoidRegistration(string medicalNo, DateTime date, string paramedicCode, string serviceUnitCode, string hour, string queueNo, string healtcareServiceUnitID)
        {
            string queue = string.Empty;
            string result = string.Empty;
            string data = string.Empty;
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/delete_antrian", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeaderRSDOSOBA(updateRequest);
                }

                #region Convert To DTO
                //GetQueueBodyRequest body = new GetQueueBodyRequest();
                ResetQueueBodyRequest body = new ResetQueueBodyRequest();
                body.rm = entityPatient.MedicalNo;
                body.str = paramedicCode;
                body.tanggal = date.ToString(Constant.FormatString.DATE_FORMAT_112);
                body.serviceunit = serviceUnitCode;

                #region Validation Schedule
                vParamedicScheduleDate entityScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", paramedicCode, healtcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                if (entityScheduleDate != null)
                {
                    body.kodejam = entityScheduleDate.OperationalTimeCode;
                }
                else
                {
                    vParamedicSchedule entitySchedule = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND DayNumber = (DATEPART(dw, '{2}') + 5) % 7 + 1", paramedicCode, healtcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                    if (entitySchedule != null)
                    {
                        body.kodejam = entitySchedule.OperationalTimeCode;
                    }
                }
                #endregion

                body.noantrian = queueNo;
                #endregion

                //string data = new JavaScriptSerializer().Serialize(body);

                data = JsonConvert.SerializeObject(body);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    GetQueueResponse queueResponse = JsonConvert.DeserializeObject<GetQueueResponse>(result);
                    if (queueResponse.status == "100")
                    {
                        result = string.Format("1|{0}|{1}", queueResponse.antrian, data);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", queueResponse.message, data);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, data);
            }

            return result;
        }
        #endregion

        #region Ubah Dokter RSDOSOBA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnRegistrationChangePhysician(string medicalNo, DateTime date, string oldParamedicCode, string newParamedicCode, string hour, string queueNo, string businessPartnerCode, string via, string healthcareServiceUnitID)
        {
            string queue = string.Empty;
            string result = string.Empty;
            string data = string.Empty;
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ubah_dokter", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeaderRSDOSOBA(updateRequest);
                }

                #region Convert To DTO
                //ResetQueueBodyRequest body = new ResetQueueBodyRequest();
                ChangePhysicianBodyRequest body = new ChangePhysicianBodyRequest();
                body.rm = entityPatient.MedicalNo;
                body.tanggallama = body.tanggalbaru = date.ToString(Constant.FormatString.DATE_FORMAT_112);
                body.kddokterlama = oldParamedicCode;
                body.kddokterbaru = newParamedicCode;

                #region Validation Schedule
                vParamedicScheduleDate entityScheduleDateOld = BusinessLayer.GetvParamedicScheduleDateList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", oldParamedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                if (entityScheduleDateOld != null)
                {
                    body.kodejamlama = entityScheduleDateOld.OperationalTimeCode;
                }
                else
                {
                    vParamedicSchedule entityScheduleOld = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND DayNumber = (DATEPART(dw, '{2}') + 5) % 7 + 1", oldParamedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                    if (entityScheduleOld != null)
                    {
                        body.kodejamlama = entityScheduleOld.OperationalTimeCode;
                    }
                }

                vParamedicScheduleDate entityScheduleDateNew = BusinessLayer.GetvParamedicScheduleDateList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", newParamedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                if (entityScheduleDateNew != null) {
                    body.kodejambaru = entityScheduleDateNew.OperationalTimeCode;
                }
                else 
                {
                    vParamedicSchedule entityScheduleNew = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicCode = '{0}' AND HealthcareServiceUnitID = {1} AND DayNumber = (DATEPART(dw, '{2}') + 5) % 7 + 1", newParamedicCode, healthcareServiceUnitID, date.ToString(Constant.FormatString.DATE_FORMAT_112))).FirstOrDefault();
                    if (entityScheduleNew != null)
                    {
                        body.kodejambaru = entityScheduleNew.OperationalTimeCode;
                    }
                }
                #endregion

                body.antrianlama = queueNo;
                body.penjaminbaru = businessPartnerCode;
                body.via = via;
                #endregion

                //string data = new JavaScriptSerializer().Serialize(body);

                data = JsonConvert.SerializeObject(body);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ChangePhysicianResponse queueResponse = JsonConvert.DeserializeObject<ChangePhysicianResponse>(result);
                    if (queueResponse.status == "100")
                    {
                        result = string.Format("1|{0}|{1}", queueResponse.antrian, data);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", queueResponse.message, data);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, data);
            }

            return result;
        }
        #endregion

        #region Ubah Jadwal Dokter Rutin RSDOSOBA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianScheduleRoutineChanged(string eventcode, int paramedicID, int healthcareServiceUnitID, int dayNumber, ParamedicSchedule oPS)
        {
            string queue = string.Empty;
            string result = string.Empty;
            string data = string.Empty;
            //Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
            vParamedicSchedule entity = new vParamedicSchedule();
            vHealthcareServiceUnit entityHSU = new vHealthcareServiceUnit();
            OperationalTime entityOT = new OperationalTime();
            ParamedicMaster entityPM = new ParamedicMaster();
            if (eventcode != "003")
            {
                entity = BusinessLayer.GetvParamedicScheduleList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", paramedicID, healthcareServiceUnitID, dayNumber)).FirstOrDefault();
                entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
            }
            else
            {
                entityPM = BusinessLayer.GetParamedicMaster(oPS.ParamedicID);
                entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", oPS.HealthcareServiceUnitID)).FirstOrDefault();
                entityOT = BusinessLayer.GetOperationalTime(oPS.OperationalTimeID);
            }

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/perubahan_jadwal_dokter", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeaderRSDOSOBA(updateRequest);
                }

                #region Convert To DTO
                ChangeMasterPhysicianScheduleRoutineBodyRequest obj = new ChangeMasterPhysicianScheduleRoutineBodyRequest();
                obj.eventcode = eventcode;
                obj.hari = dayNumber.ToString();
                if (entityHSU.ServiceUnitCode.Contains("NR"))
                {
                    obj.tipepoli = "NON_REGULER";
                }
                else
                {
                    obj.tipepoli = "REGULER";
                }
                obj.serviceunit = entityHSU.ServiceUnitCode;
                obj.petugas = AppSession.UserLogin.UserName;

                if (eventcode != "003")
                {
                    obj.kddokter = entity.ParamedicCode;
                    obj.jamstart = entity.StartTime1;
                    obj.jamend = entity.EndTime1;
                    obj.qmax = entity.MaximumAppointment1.ToString();
                    obj.specialty = entity.SpecialtyID;
                    obj.kodejam = entity.OperationalTimeCode;
                    if (!string.IsNullOrEmpty(entity.MobileAppointment1.ToString()))
                    {
                        obj.qapp = entity.MobileAppointment1.ToString();
                    }
                    else
                    {
                        obj.qapp = "0";
                    }
                }
                else
                {
                    obj.kddokter = entityPM.ParamedicCode;
                    obj.jamstart = entityOT.StartTime1;
                    obj.jamend = entityOT.EndTime1;
                    obj.qmax = oPS.MaximumAppointment1.ToString();
                    obj.specialty = entityPM.SpecialtyID;
                    obj.kodejam = entityOT.OperationalTimeCode;
                    if (!string.IsNullOrEmpty(oPS.MaximumAppointment1.ToString()))
                    {
                        obj.qapp = oPS.MaximumAppointment1.ToString();
                    }
                    else
                    {
                        obj.qapp = "0";
                    }
                }
                #endregion

                //string data = new JavaScriptSerializer().Serialize(body);

                data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ChangeMasterPhysicianScheduleRoutineResponse queueResponse = JsonConvert.DeserializeObject<ChangeMasterPhysicianScheduleRoutineResponse>(result);
                    if (queueResponse.status == "100" || queueResponse.status == "200" || queueResponse.status == "300")
                    {
                        result = string.Format("1|{0}|{1}", queueResponse.message, data);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", queueResponse.message, data);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, data);
            }

            return result;
        }
        #endregion

        #region Ubah Jadwal Dokter Per Tanggal RSDOSOBA
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPhysicianScheduleByDateChanged(string eventcode, int paramedicID, int healthcareServiceUnitID, string date, vParamedicScheduleDate1 entityOld)
        {
            string queue = string.Empty;
            string result = string.Empty;
            string data = string.Empty;
            //Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
            vParamedicScheduleDate1 entity = BusinessLayer.GetvParamedicScheduleDate1List(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", paramedicID, healthcareServiceUnitID, date)).FirstOrDefault();
            ParamedicScheduleDate entitySchedule = BusinessLayer.GetParamedicScheduleDateList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", paramedicID, healthcareServiceUnitID, date)).FirstOrDefault();
            vHealthcareServiceUnit entityHSU = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/perubahan_dokter_tanggal", url.ParameterValue));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";

                if (auth.ParameterValue == "1")
                {
                    SetRequestHeaderRSDOSOBA(updateRequest);
                }

                #region Convert To DTO
                ChangeMasterPhysicianScheduleByDateBodyRequest obj = new ChangeMasterPhysicianScheduleByDateBodyRequest();
                if (entityOld != null)
                {
                    obj.kddokter = entityOld.ParamedicCode;
                    obj.specialty = entityOld.SpecialtyID;
                    obj.kodejam = entityOld.OperationalTimeCode;
                    obj.jamstart = entityOld.StartTime1;
                    obj.jamend = entityOld.EndTime1;
                    obj.qmax = entityOld.MaximumAppointment1.ToString();
                    obj.tanggal = entityOld.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                }
                else
                {
                    obj.kddokter = entity.ParamedicCode;
                    obj.specialty = entity.SpecialtyID;
                    obj.kodejam = entity.OperationalTimeCode;
                    obj.jamstart = entity.StartTime1;
                    obj.jamend = entity.EndTime1;
                    obj.qmax = entity.MaximumAppointment1.ToString();
                    obj.tanggal = entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                }
                obj.eventcode = eventcode;
                obj.serviceunit = entityHSU.ServiceUnitCode;
                //obj.tanggal = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                if (entityHSU.ServiceUnitCode.Contains("NR"))
                {
                    obj.tipepoli = "NON_REGULER";
                }
                else
                {
                    obj.tipepoli = "REGULER";
                }
                if (entitySchedule != null)
                {
                    if (!string.IsNullOrEmpty(entitySchedule.MobileAppointment1.ToString()))
                    {
                        obj.qapp = entitySchedule.MobileAppointment1.ToString();
                    }
                    else
                    {
                        obj.qapp = "0";
                    }
                }
                else
                {
                    obj.qapp = "0";
                }
                #endregion

                //string data = new JavaScriptSerializer().Serialize(body);

                data = JsonConvert.SerializeObject(obj);

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ChangeMasterPhysicianScheduleByDateResponse queueResponse = JsonConvert.DeserializeObject<ChangeMasterPhysicianScheduleByDateResponse>(result);
                    if (queueResponse.status == "100" || queueResponse.status == "200" || queueResponse.status == "300")
                    {
                        result = string.Format("1|{0}|{1}", queueResponse.message, data);
                    }
                    else
                    {
                        result = string.Format("0|{0}|{1}", queueResponse.message, data);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}|{1}", ex.Message, data);
            }

            return result;
        }
        #endregion

        //#region Ubah Jadwal Cuti Dokter RSDOSOBA
        //[WebMethod]
        //[ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        //public string OnPhysicianLeaveScheduleChanged(string eventcode, int id)
        //{
        //    string queue = string.Empty;
        //    string result = string.Empty;
        //    //Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}' AND IsDeleted = 0", medicalNo)).FirstOrDefault();
        //    vParamedicLeaveSchedule entity = BusinessLayer.GetvParamedicLeaveScheduleList(string.Format("ID = {0}", id)).FirstOrDefault();
        //    try
        //    {
        //        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/perubahan_cuti_dokter", url.ParameterValue));
        //        updateRequest.Method = "POST";
        //        updateRequest.ContentType = "application/json";

        //        if (auth.ParameterValue == "1")
        //        {
        //            SetRequestHeaderRSDOSOBA(updateRequest);
        //        }

        //        #region Convert To DTO
        //        ChangeMasterPhysicianLeaveBodyRequest obj = new ChangeMasterPhysicianLeaveBodyRequest();
        //        obj.eventcode = eventcode;
        //        obj.kddokter = entity.ParamedicCode;
        //        obj.tanggalmulai = entity.StartDate.ToString(Constant.FormatString.DATE_FORMAT_112);
        //        obj.tanggalakhir = entity.EndDate.ToString(Constant.FormatString.DATE_FORMAT_112);
        //        obj.alasancuti = entity.ParamedicLeaveReason;
        //        ParamedicSchedule entityNR = BusinessLayer.GetParamedicScheduleList(string.Format("ParamedicID = {0} AND DayNumber")).FirstOrDefault();
        //        if (entity != null)
        //        {
        //            entity.
        //        }
        //        else
        //        {
        //            obj.qmaxnonreguler = "0";
        //        }
        //        #endregion

        //        //string data = new JavaScriptSerializer().Serialize(body);

        //        string data = JsonConvert.SerializeObject(obj);

        //        byte[] bytes = Encoding.UTF8.GetBytes(data);
        //        updateRequest.ContentLength = bytes.Length;
        //        Stream putStream = updateRequest.GetRequestStream();
        //        putStream.Write(bytes, 0, bytes.Length);
        //        putStream.Close();
        //        WebResponse response = (WebResponse)updateRequest.GetResponse();
        //        using (StreamReader sr = new StreamReader(response.GetResponseStream()))
        //        {
        //            result = sr.ReadToEnd();
        //            ChangeMasterPhysicianScheduleByDateResponse queueResponse = JsonConvert.DeserializeObject<ChangeMasterPhysicianScheduleByDateResponse>(result);
        //            if (queueResponse.status == "100")
        //            {
        //                result = string.Format("1|{0}", queueResponse.message);
        //            }
        //            else
        //            {
        //                result = string.Format("0|{0}", queueResponse.message);
        //            }
        //        }
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result = string.Format("0|{0}", ex.Message);
        //    }

        //    return result;
        //}
        //#endregion

        #endregion

        #region IPTV
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string IPTV_BedTransfer(List<CenterbackBedTransferDTO> lstObj)
        {
            string result = string.Empty;

            List<SettingParameterDt> lstDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')", "001", Constant.SettingParameter.SA0130, Constant.SettingParameter.SA0131, Constant.SettingParameter.SA0132));
            string consID = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0130).FirstOrDefault().ParameterValue;
            string consPassword = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0131).FirstOrDefault().ParameterValue;
            string urlAPI = lstDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0132).FirstOrDefault().ParameterValue;

            try
            {
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/PV1_T01", urlAPI));
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/json";
                SetRequestHeaderCenterback(updateRequest, consID, consPassword);

                var json = JsonConvert.SerializeObject(lstObj);
                using (var streamWriter = new StreamWriter(updateRequest.GetRequestStream()))
                {
                    streamWriter.Write(json);
                }

                WebResponse response = (WebResponse)updateRequest.GetResponse();
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

                //string data = JsonConvert.SerializeObject(lstObj);

                //byte[] bytes = Encoding.UTF8.GetBytes(data);
                //updateRequest.ContentLength = bytes.Length;
                //Stream putStream = updateRequest.GetRequestStream();
                //putStream.Write(bytes, 0, bytes.Length);
                //putStream.Close();

                //WebResponse response = (WebResponse)updateRequest.GetResponse();
                //string result = "";
                //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //{
                //    result = sr.ReadToEnd();
                //}
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
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

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        private void SetRequestHeaderCenterback(HttpWebRequest Request, string consID, string password)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), password));
        }

        private string GenerateSignature(string data, string secretKey)
        {
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            var encodedSignature = Convert.ToBase64String(signature);

            return encodedSignature;
        }

        private void SetRequestHeaderRSDOSOBA(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            SettingParameterDt entityConsID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_CONS_ID);
            SettingParameterDt entityPassID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_PASS_ID);

            string consID = entityConsID.ParameterValue;
            string pass = entityPassID.ParameterValue;
            //string data = unixTimestamp.ToString() + consID;
            string data = string.Format("{0}&{1}", consID, unixTimestamp.ToString());

            Request.Headers.Add("id", consID);
            Request.Headers.Add("time", unixTimestamp.ToString());
            Request.Headers.Add("token", GenerateSignature(string.Format("{0}", data), pass));
        }
        #endregion

    }
}
