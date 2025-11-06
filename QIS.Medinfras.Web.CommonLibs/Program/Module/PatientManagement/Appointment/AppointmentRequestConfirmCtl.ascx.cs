using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentRequestConfirmCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] data = param.Split('|');

                vAppointmentRequest oApmReq = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = '{0}'", data[0])).FirstOrDefault();
                EntityToControl(oApmReq, data);

                IsAdd = true;
                SetControlProperties();
                GetSettingParameter();
            }
        }

        protected override void SetControlProperties()
        {
            bindCboSession();
        }
        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnHealthcareServiceUnitImagingID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnHealthcareServiceUnitLaboratoryID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }
        private void EntityToControl(vAppointmentRequest entity, string[] data)
        {
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            hdnServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnAppointmentRequestID.Value = entity.AppointmentRequestID.ToString();

            txtDateApm.Text = data[1];
            hdnParamedicID.Value = data[2];

            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID='{0}'", hdnParamedicID.Value)).FirstOrDefault();
            txtParmedicName.Text = oParamedic.FullName;

        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);
            ParamedicScheduleDao paramedicScheduleDao = new ParamedicScheduleDao(ctx);
            ParamedicScheduleDateDao paramedicScheduleDateDao = new ParamedicScheduleDateDao(ctx);
            ParamedicLeaveScheduleDao paramedicLeaveScheduleDao = new ParamedicLeaveScheduleDao(ctx);
            RegistrationBPJSDao registrationBPJSDao = new RegistrationBPJSDao(ctx);
            AppointmentRequestDao appointmentRequestDao = new AppointmentRequestDao(ctx);
            int appointmentRegID = 0;
            int retvalref = 0;
            try
            {
                if (!String.IsNullOrEmpty(hdnAppointmentRequestID.Value))
                {
                    AppointmentRequest appRequest = appointmentRequestDao.Get(Convert.ToInt32(hdnAppointmentRequestID.Value));
                    if (appRequest != null)
                    {
                        bool isvalid = true;

                        if (cboSession.Value == null)
                        {
                            isvalid = false;
                            result = false;
                            errMessage = "Silahkan di pilih untuk sesi tersebut";
                            ctx.RollBackTransaction();
                        }

                        if (isvalid)
                        {
                            if (appRequest.AppointmentID == null)
                            {
                                Appointment appointment = new Appointment();
                                DateTime stAppo = DateTime.Now;
                                DateTime stAppoValid = DateTime.Now;
                                int hour = 0;
                                int minute = 0;
                                string startTimeCheck = "";
                                string endTimeCheck = "";

                                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                                int healthcareServiceUnitID = appRequest.HealthcareServiceUnitID;
                                DateTime scheduleDate = Helper.GetDatePickerValue(Request.Form[txtDateApm.UniqueID]);

                                bool isValid = true;

                                appointment.StartDate = appointment.EndDate = scheduleDate;
                                Int16 daynumber = (Int16)scheduleDate.DayOfWeek;
                                if (daynumber == 0)
                                {
                                    daynumber = 7;
                                }

                                #region validate paramedic Schedule
                                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                            healthcareServiceUnitID, paramedicID, daynumber), ctx).FirstOrDefault();

                                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                                healthcareServiceUnitID, paramedicID, scheduleDate), ctx).FirstOrDefault();

                                //ValidateParamedicScSchedule(obj, objSchDate);
                                List<ParamedicLeaveSchedule> objLeave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND ('{1}' BETWEEN CONVERT(VARCHAR,StartDate,112) AND CONVERT(VARCHAR,EndDate,112)) AND IsDeleted = 0", paramedicID, scheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                                #endregion

                                if (objLeave.Count == 0)
                                {

                                    #region validate Visit Type
                                    int visitDuration = 0;

                                    List<ParamedicVisitType> lstVisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitTypeID = {2}", healthcareServiceUnitID, paramedicID, appRequest.VisitTypeID));
                                    vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID)).FirstOrDefault();
                                    if (lstVisitTypeParamedic.Count > 0)
                                    {
                                        visitDuration = lstVisitTypeParamedic.FirstOrDefault().VisitDuration;
                                        appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                        appointment.VisitTypeID = lstVisitTypeParamedic.FirstOrDefault().VisitTypeID;
                                    }
                                    else
                                    {
                                        if (VisitTypeHealthcare.IsHasVisitType)
                                        {
                                            List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND VisitTypeID = {1}", healthcareServiceUnitID, appRequest.VisitTypeID));
                                            if (lstServiceUnitVisitType.Count > 0)
                                            {
                                                visitDuration = lstServiceUnitVisitType.FirstOrDefault().VisitDuration;
                                                appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                                appointment.VisitTypeID = lstServiceUnitVisitType.FirstOrDefault().VisitTypeID;
                                            }
                                            else
                                            {
                                                List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                                                visitDuration = 15;
                                                appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                                appointment.VisitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                            }
                                        }
                                        else
                                        {
                                            List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"));
                                            visitDuration = 15;
                                            appointment.VisitDuration = Convert.ToInt16(visitDuration);
                                            appointment.VisitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                        }
                                    }
                                    #endregion

                                    #region Save Appointment
                                    int session = Convert.ToInt32(cboSession.Value); /// Convert.ToInt32(appRequest.SessionRequest) != 0 && Convert.ToInt32(appRequest.SessionRequest) != null ? Convert.ToInt32(appRequest.SessionRequest) : 1;
                                    if (objSchDate != null)
                                    {
                                        DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                                        DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                                        DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                                        DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                                        DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                                        DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                                        DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                                        DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                                        DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                                        DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                                        string filterExpression;
                                        List<Appointment> lstAppointment;
                                        DateTime startAppo, endAppo;

                                        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "") //5 sesi
                                        {
                                            if (session == 5)
                                            {
                                                #region check slot time 5
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime5, objSchDate.EndTime5, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 5;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime5;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 5;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot5)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart5.TimeOfDay || endAppo.TimeOfDay > objSchEnd5.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList5)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime5;
                                                            appointment.EndTime = objSchDate.StartTime5;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime5;
                                                            endTimeCheck = objSchDate.EndTime5;

                                                            session = 5;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime5;
                                                        endTimeCheck = objSchDate.EndTime5;

                                                        session = 5;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 4)
                                            {
                                                #region check slot time 4
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 4;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime4;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 4;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot4)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList4)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime4;
                                                            appointment.EndTime = objSchDate.StartTime4;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime4;
                                                            endTimeCheck = objSchDate.EndTime4;

                                                            session = 4;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime3;
                                                            appointment.EndTime = objSchDate.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime3;
                                                            endTimeCheck = objSchDate.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime2;
                                                            appointment.EndTime = objSchDate.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime2;
                                                            endTimeCheck = objSchDate.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime1;
                                                            appointment.EndTime = objSchDate.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime1;
                                                            endTimeCheck = objSchDate.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime1;
                                                        endTimeCheck = objSchDate.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }

                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "") //4 sesi
                                        {
                                            if (session == 4)
                                            {
                                                #region check slot time 4
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime4, objSchDate.EndTime4, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 4;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime4;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 4;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot4)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart4.TimeOfDay || endAppo.TimeOfDay > objSchEnd4.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList4)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime4;
                                                            appointment.EndTime = objSchDate.StartTime4;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime4;
                                                            endTimeCheck = objSchDate.EndTime4;

                                                            session = 4;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime4;
                                                        endTimeCheck = objSchDate.EndTime4;

                                                        session = 4;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime3;
                                                            appointment.EndTime = objSchDate.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime3;
                                                            endTimeCheck = objSchDate.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime2;
                                                            appointment.EndTime = objSchDate.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime2;
                                                            endTimeCheck = objSchDate.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime1;
                                                            appointment.EndTime = objSchDate.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime1;
                                                            endTimeCheck = objSchDate.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime1;
                                                        endTimeCheck = objSchDate.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "") //3 sesi
                                        {
                                            if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime3, objSchDate.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart3.TimeOfDay || endAppo.TimeOfDay > objSchEnd3.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime3;
                                                            appointment.EndTime = objSchDate.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime3;
                                                            endTimeCheck = objSchDate.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime3;
                                                        endTimeCheck = objSchDate.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime2;
                                                            appointment.EndTime = objSchDate.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime2;
                                                            endTimeCheck = objSchDate.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime1;
                                                            appointment.EndTime = objSchDate.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime1;
                                                            endTimeCheck = objSchDate.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime1;
                                                        endTimeCheck = objSchDate.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "") //2 sesi
                                        {
                                            if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime2, objSchDate.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart2.TimeOfDay || endAppo.TimeOfDay > objSchEnd2.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime2;
                                                            appointment.EndTime = objSchDate.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime2;
                                                            endTimeCheck = objSchDate.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime2;
                                                        endTimeCheck = objSchDate.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = objSchDate.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!objSchDate.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                                    {
                                                        if (objSchDate.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = objSchDate.StartTime1;
                                                            appointment.EndTime = objSchDate.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = objSchDate.StartTime1;
                                                            endTimeCheck = objSchDate.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = objSchDate.StartTime1;
                                                        endTimeCheck = objSchDate.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "") //1 sesi
                                        {
                                            #region check slot time 1
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, objSchDate.StartTime1, objSchDate.EndTime1, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 1;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = objSchDate.StartTime1;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 1;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!objSchDate.IsAppointmentByTimeSlot1)
                                            {
                                                if (startAppo.TimeOfDay < objSchStart1.TimeOfDay || endAppo.TimeOfDay > objSchEnd1.TimeOfDay)
                                                {
                                                    if (objSchDate.IsAllowWaitingList1)
                                                    {
                                                        appointment.StartTime = objSchDate.StartTime1;
                                                        appointment.EndTime = objSchDate.StartTime1;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = objSchDate.StartTime1;
                                                        endTimeCheck = objSchDate.EndTime1;

                                                        session = 1;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = objSchDate.StartTime1;
                                                    endTimeCheck = objSchDate.EndTime1;

                                                    session = 1;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                            #endregion
                                        }
                                    }
                                    else if (obj != null && objSchDate == null)
                                    {
                                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                                        string filterExpression;
                                        List<Appointment> lstAppointment;
                                        DateTime startAppo, endAppo;

                                        if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                                        {
                                            if (session == 5)
                                            {
                                                #region check slot time 5
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime5, obj.EndTime5, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 5;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime5.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime5;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 5;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot5)
                                                {
                                                    if (startAppo.TimeOfDay < objStart5.TimeOfDay || endAppo.TimeOfDay > objEnd5.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList5)
                                                        {
                                                            appointment.StartTime = obj.StartTime5;
                                                            appointment.EndTime = obj.StartTime5;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime5;
                                                            endTimeCheck = obj.EndTime5;

                                                            session = 5;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime5;
                                                        endTimeCheck = obj.EndTime5;

                                                        session = 5;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 4)
                                            {
                                                #region check slot time 4
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 4;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime4;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 4;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot4)
                                                {
                                                    if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList4)
                                                        {
                                                            appointment.StartTime = obj.StartTime4;
                                                            appointment.EndTime = obj.StartTime4;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime4;
                                                            endTimeCheck = obj.EndTime4;

                                                            session = 4;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = obj.StartTime3;
                                                            appointment.EndTime = obj.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime3;
                                                            endTimeCheck = obj.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = obj.StartTime2;
                                                            appointment.EndTime = obj.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime2;
                                                            endTimeCheck = obj.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = obj.StartTime1;
                                                            appointment.EndTime = obj.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime1;
                                                            endTimeCheck = obj.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime1;
                                                        endTimeCheck = obj.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                                        {
                                            if (session == 4)
                                            {
                                                #region check slot time 4
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime4, obj.EndTime4, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 4;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime4.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime4;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 4;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot4)
                                                {
                                                    if (startAppo.TimeOfDay < objStart4.TimeOfDay || endAppo.TimeOfDay > objEnd4.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList4)
                                                        {
                                                            appointment.StartTime = obj.StartTime4;
                                                            appointment.EndTime = obj.StartTime4;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime4;
                                                            endTimeCheck = obj.EndTime4;

                                                            session = 4;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime4;
                                                        endTimeCheck = obj.EndTime4;

                                                        session = 4;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = obj.StartTime3;
                                                            appointment.EndTime = obj.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime3;
                                                            endTimeCheck = obj.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = obj.StartTime2;
                                                            appointment.EndTime = obj.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime2;
                                                            endTimeCheck = obj.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = obj.StartTime1;
                                                            appointment.EndTime = obj.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime1;
                                                            endTimeCheck = obj.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime1;
                                                        endTimeCheck = obj.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            if (session == 3)
                                            {
                                                #region check slot time 3
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime3, obj.EndTime3, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 3;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime3.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime3;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 3;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot3)
                                                {
                                                    if (startAppo.TimeOfDay < objStart3.TimeOfDay || endAppo.TimeOfDay > objEnd3.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList3)
                                                        {
                                                            appointment.StartTime = obj.StartTime3;
                                                            appointment.EndTime = obj.StartTime3;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime3;
                                                            endTimeCheck = obj.EndTime3;

                                                            session = 3;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime3;
                                                        endTimeCheck = obj.EndTime3;

                                                        session = 3;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = obj.StartTime2;
                                                            appointment.EndTime = obj.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime2;
                                                            endTimeCheck = obj.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = obj.StartTime1;
                                                            appointment.EndTime = obj.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime1;
                                                            endTimeCheck = obj.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime1;
                                                        endTimeCheck = obj.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            if (session == 2)
                                            {
                                                #region check slot time 2
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime2, obj.EndTime2, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 2;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime2.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime2;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 2;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot2)
                                                {
                                                    if (startAppo.TimeOfDay < objStart2.TimeOfDay || endAppo.TimeOfDay > objEnd2.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList2)
                                                        {
                                                            appointment.StartTime = obj.StartTime2;
                                                            appointment.EndTime = obj.StartTime2;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime2;
                                                            endTimeCheck = obj.EndTime2;

                                                            session = 2;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime2;
                                                        endTimeCheck = obj.EndTime2;

                                                        session = 2;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                            else if (session == 1)
                                            {
                                                #region check slot time 1
                                                filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                                lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                                if (lstAppointment.Count > 0)
                                                {
                                                    //set jam mulai dan jam selesai Appointment
                                                    hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                    minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                    stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                    stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                    //end

                                                    appointment.StartTime = stAppo.ToString("HH:mm");
                                                    appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                    session = 1;
                                                }
                                                else
                                                {
                                                    int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                                    int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                                    DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                    appointment.StartTime = obj.StartTime1;
                                                    appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                    session = 1;
                                                }

                                                startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                                endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                                if (!obj.IsAppointmentByTimeSlot1)
                                                {
                                                    if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                                    {
                                                        if (obj.IsAllowWaitingList1)
                                                        {
                                                            appointment.StartTime = obj.StartTime1;
                                                            appointment.EndTime = obj.StartTime1;
                                                            appointment.IsWaitingList = true;
                                                            startTimeCheck = obj.StartTime1;
                                                            endTimeCheck = obj.EndTime1;

                                                            session = 1;
                                                        }
                                                        else
                                                        {
                                                            isValid = false;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        appointment.IsWaitingList = false;
                                                        startTimeCheck = obj.StartTime1;
                                                        endTimeCheck = obj.EndTime1;

                                                        session = 1;
                                                    }
                                                }
                                                else
                                                {
                                                    isValid = false;
                                                }
                                                #endregion
                                            }
                                        }
                                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                                        {
                                            #region check slot time 1
                                            filterExpression = string.Format("StartDate = '{0}' AND (StartTime >= '{1}' AND EndTime <= '{2}') AND ParamedicID = {3} ORDER BY AppointmentID ASC", scheduleDate, obj.StartTime1, obj.EndTime1, paramedicID);
                                            lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);

                                            if (lstAppointment.Count > 0)
                                            {
                                                //set jam mulai dan jam selesai Appointment
                                                hour = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(0, 2));
                                                minute = Convert.ToInt32(lstAppointment.LastOrDefault().EndTime.Substring(3));

                                                stAppo = new DateTime(scheduleDate.Year, scheduleDate.Month, scheduleDate.Day, hour, minute, 0);
                                                stAppoValid = stAppo.AddMinutes(appointment.VisitDuration);
                                                //end

                                                appointment.StartTime = stAppo.ToString("HH:mm");
                                                appointment.EndTime = stAppoValid.ToString("HH:mm");

                                                session = 1;
                                            }
                                            else
                                            {
                                                int hourTemp = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                                int minuteTemp = Convert.ToInt32(obj.StartTime1.Substring(3));

                                                DateTime end = new DateTime(2000, 1, 1, hourTemp, minuteTemp, 0);
                                                appointment.StartTime = obj.StartTime1;
                                                appointment.EndTime = end.AddMinutes(appointment.VisitDuration).ToString("HH:mm");

                                                session = 1;
                                            }

                                            startAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.StartTime);
                                            endAppo = DateTime.Parse("2012-01-28" + ' ' + appointment.EndTime);

                                            if (!obj.IsAppointmentByTimeSlot1)
                                            {
                                                if (startAppo.TimeOfDay < objStart1.TimeOfDay || endAppo.TimeOfDay > objEnd1.TimeOfDay)
                                                {
                                                    if (obj.IsAllowWaitingList1)
                                                    {
                                                        appointment.StartTime = obj.StartTime1;
                                                        appointment.EndTime = obj.StartTime1;
                                                        appointment.IsWaitingList = true;
                                                        startTimeCheck = obj.StartTime1;
                                                        endTimeCheck = obj.EndTime1;

                                                        session = 1;
                                                    }
                                                    else
                                                    {
                                                        isValid = false;
                                                    }
                                                }
                                                else
                                                {
                                                    appointment.IsWaitingList = false;
                                                    startTimeCheck = obj.StartTime1;
                                                    endTimeCheck = obj.EndTime1;

                                                    session = 1;
                                                }
                                            }
                                            else
                                            {
                                                isValid = false;
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        isValid = false;
                                    }

                                    #region finalisasi appointment
                                    if (isValid)
                                    {
                                        List<Appointment> lstAppointment;
                                        string filterExpression;

                                        filterExpression = string.Format("StartDate = '{0}' AND ParamedicID = {1} AND HealthcareServiceUnitID = {2} AND GCAppointmentStatus NOT IN ('{3}','{4}')", appRequest.AppointmentDate.ToString(Constant.FormatString.DATE_FORMAT_112), hdnParamedicID.Value, appRequest.HealthcareServiceUnitID, Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED);
                                        if (appRequest.MRN != null && appRequest.MRN != 0)
                                        {
                                            filterExpression += string.Format(" AND MRN = {0}", appRequest.MRN);
                                        }
                                        else if (appRequest.GuestID != null && appRequest.GuestID != 0)
                                        {
                                            filterExpression += string.Format(" AND GuestID = {0}", appRequest.GuestID);
                                        }
                                        lstAppointment = BusinessLayer.GetAppointmentList(filterExpression);
                                        vRegistration7 reg = null;
                                        vPatient entityPatient = null;
                                        Guest entityGuest = null;
                                        if (lstAppointment.Count > 0)
                                        {

                                            result = false;
                                            errMessage = string.Format("Sudah ada perjanjian untuk pasien ini dengan nomor perjanjian {0}", lstAppointment.FirstOrDefault().AppointmentNo);
                                            ctx.RollBackTransaction();

                                        }
                                        else
                                        {
                                            if (appRequest.RegistrationID != null)
                                            {
                                                reg = BusinessLayer.GetvRegistration7List(string.Format("RegistrationID = {0}", appRequest.RegistrationID)).FirstOrDefault();
                                            }
                                            else
                                            {
                                                if (appRequest.MRN != null && appRequest.MRN != 0)
                                                {
                                                    entityPatient = BusinessLayer.GetvPatientList(string.Format("MRN = {0}", appRequest.MRN)).FirstOrDefault();
                                                }
                                                else if (appRequest.GuestID != null && appRequest.GuestID != 0)
                                                {
                                                    entityGuest = BusinessLayer.GetGuestList(string.Format("GuestID = {0}", appRequest.GuestID), ctx).FirstOrDefault();
                                                }
                                            }

                                            if (reg != null)
                                            {
                                                if (reg.MRN == 0 && reg.GuestID != 0)
                                                {
                                                    appointment.IsNewPatient = true;
                                                    appointment.Name = reg.PatientName;
                                                    appointment.GCGender = reg.GCGender;
                                                    appointment.StreetName = reg.StreetName;
                                                    appointment.PhoneNo = reg.PhoneNo1;
                                                    appointment.MobilePhoneNo = reg.MobilePhoneNo1;
                                                    appointment.GCSalutation = reg.GCSalutation;
                                                }
                                                else if (reg.MRN != 0 && reg.GuestID == 0)
                                                {
                                                    appointment.IsNewPatient = false;
                                                    appointment.MRN = reg.MRN;
                                                }

                                                appointment.Notes = string.Format("Permintaan Perjanjian | No Registration: {0}", reg.RegistrationNo);
                                            }
                                            else
                                            {
                                                if (entityPatient != null)
                                                {
                                                    appointment.IsNewPatient = false;
                                                    appointment.MRN = entityPatient.MRN;
                                                    appointment.Name = entityPatient.PatientName;
                                                    appointment.GCGender = entityPatient.GCGender;
                                                    appointment.StreetName = entityPatient.StreetName;
                                                    appointment.PhoneNo = entityPatient.PhoneNo1;
                                                    appointment.MobilePhoneNo = entityPatient.MobilePhoneNo1;
                                                    appointment.GCSalutation = entityPatient.GCSalutation;
                                                }
                                                else if (entityGuest != null)
                                                {
                                                    appointment.IsNewPatient = true;
                                                    appointment.GuestID = entityGuest.GuestID;
                                                    appointment.FirstName = entityGuest.FirstName;
                                                    appointment.MiddleName = entityGuest.MiddleName;
                                                    appointment.LastName = entityGuest.LastName;
                                                    appointment.Name = entityGuest.FullName;
                                                    appointment.GCGender = entityGuest.GCGender;
                                                    appointment.StreetName = entityGuest.StreetName;
                                                    appointment.PhoneNo = entityGuest.PhoneNo;
                                                    appointment.MobilePhoneNo = entityGuest.MobilePhoneNo;
                                                    appointment.GCSalutation = entityGuest.GCSalutation;
                                                    appointment.EmailAddress = entityGuest.EmailAddress;
                                                }
                                            }

                                            appointment.HealthcareServiceUnitID = healthcareServiceUnitID;
                                            appointment.ParamedicID = paramedicID;
                                            appointment.FromVisitID = appRequest.VisitID;

                                            appointment.GCReferrerGroup = appRequest.GCReferrerGroup;
                                            appointment.ReferrerParamedicID = appRequest.ReferrerParamedicID;
                                            appointment.ReferrerID = appRequest.ReferrerID;
                                            appointment.ReferenceNo = appRequest.ReferenceNo;
                                            appointment.IsReferralVisit = appRequest.IsReferralVisit;

                                            appointment.GCCustomerType = appRequest.GCCustomerType;
                                            if (string.IsNullOrEmpty(appointment.GCCustomerType))
                                            {
                                                appointment.BusinessPartnerID = 1;
                                                appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                                appointment.ContractID = null;
                                                appointment.CoverageTypeID = null;
                                                appointment.CoverageLimitAmount = 0;
                                                appointment.IsCoverageLimitPerDay = false;
                                                appointment.GCTariffScheme = null;
                                                appointment.IsControlClassCare = false;
                                                appointment.ControlClassID = null;
                                                appointment.EmployeeID = null;
                                            }
                                            else
                                            {
                                                if (appointment.GCCustomerType == Constant.CustomerType.PERSONAL)
                                                {
                                                    appointment.BusinessPartnerID = 1;
                                                    appointment.GCCustomerType = Constant.CustomerType.PERSONAL;
                                                    appointment.ContractID = null;
                                                    appointment.CoverageTypeID = null;
                                                    appointment.CoverageLimitAmount = 0;
                                                    appointment.IsCoverageLimitPerDay = false;
                                                    appointment.GCTariffScheme = null;
                                                    appointment.IsControlClassCare = false;
                                                    appointment.ControlClassID = null;
                                                    appointment.EmployeeID = null;
                                                }
                                                else
                                                {
                                                    appointment.BusinessPartnerID = appRequest.BusinessPartnerID;
                                                    appointment.ContractID = appRequest.ContractID;
                                                    appointment.CoverageTypeID = appRequest.CoverageTypeID;
                                                    appointment.CoverageLimitAmount = appRequest.CoverageLimitAmount;
                                                    appointment.IsCoverageLimitPerDay = appRequest.IsCoverageLimitPerDay;
                                                    appointment.GCTariffScheme = appRequest.GCTariffScheme;
                                                    appointment.IsControlClassCare = appRequest.IsControlClassCare;
                                                    appointment.ControlClassID = appRequest.ControlClassID;
                                                    appointment.EmployeeID = appRequest.EmployeeID;
                                                }

                                            }


                                            appointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                            appointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                            appointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(appointment.TransactionCode, appointment.StartDate, ctx);
                                            appointment.Session = session;
                                            //appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, session, 1));
                                            bool isBPJS = false;
                                            if (appointment.GCCustomerType == Constant.CustomerType.BPJS)
                                            {
                                                isBPJS = true;
                                            }
                                            appointment.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(appointment.HealthcareServiceUnitID, Convert.ToInt32(appointment.ParamedicID), appointment.StartDate, session, false, isBPJS, 1, 0, ctx));
                                            if (appRequest.GCAppointmentRequestMethod == Constant.AppointmentRequestMethod.MOBILE_APPS)
                                            {
                                                appointment.GCAppointmentMethod = Constant.AppointmentMethod.MOBILE;
                                            }
                                            else
                                            {
                                                appointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                                            }
                                            appointment.CreatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            retvalref = appointmentDao.InsertReturnPrimaryKeyID(appointment);

                                            appRequest.AppointmentID = retvalref;
                                            appRequest.AppointmentDate = appointment.StartDate;
                                            appRequest.AppointmentTime = appointment.StartTime;
                                            appRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            appointmentRequestDao.Update(appRequest);

                                            if (appRequest.RegistrationID != null && appRequest.RegistrationID != 0)
                                            {
                                                RegistrationBPJS regBPJS = registrationBPJSDao.Get(Convert.ToInt32(appRequest.RegistrationID));
                                                if (regBPJS != null)
                                                {
                                                    regBPJS.AppointmentID = retvalref;
                                                    regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    registrationBPJSDao.Update(regBPJS);
                                                }
                                            }

                                            retval = appRequest.AppointmentRequestID.ToString();

                                            appointmentRegID = appRequest.AppointmentRequestID;
                                            ctx.CommitTransaction();
                                            BridgingToMedinfrasMobileApps("save", appointment.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), appointment.EndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2), Convert.ToInt32(hdnAppointmentRequestID.Value));

                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = "Maaf Tidak Ada Jadwal Praktek Dokter Untuk Tanggal Yang Dipilih";
                                        ctx.RollBackTransaction();
                                    }
                                    #endregion
                                    #endregion
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Dokter sedang cuti";
                                    ctx.RollBackTransaction();
                                }
                            }

                            else
                            {
                                result = false;
                                errMessage = "Permintaan perjanjian ini sudah diproses menjadi appointment";
                                ctx.RollBackTransaction();
                            }
                        }


                    }
                    else
                    {
                        result = false;
                        errMessage = "Permintaan perjanjian Tidak ditemukan";
                        ctx.RollBackTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void bindCboSession()
        {
            if (!String.IsNullOrEmpty(hdnParamedicID.Value))
            {
                Int32 HealthcareServiceUnitID = Convert.ToInt32(hdnServiceUnitID.Value);
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(txtDateApm);

                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }

                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                            HealthcareServiceUnitID, ParamedicID, dayNumber)).FirstOrDefault();

                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                HealthcareServiceUnitID, ParamedicID, selectedDate)).FirstOrDefault();

                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(ParamedicID);

                ValidateParamedicScSchedule(obj, objSchDate);

                List<ParamedicLeaveSchedule> objLeave = BusinessLayer.GetParamedicLeaveScheduleList(string.Format("ParamedicID = {0} AND ('{1}' BETWEEN CONVERT(VARCHAR,StartDate,112) AND CONVERT(VARCHAR,EndDate,112)) AND IsDeleted = 0", ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                if (objLeave.Count > 0)
                {

                    trRemarks.Style.Remove("display");
                    txtRemarks.Text = string.Format("dokter sedang cuti");

                }



                List<ParamedicScheduleCustom> ListDisplayAppTime = new List<ParamedicScheduleCustom>();
                List<ParamedicScheduleCustom> ListDisplayAppTimeWithoutTimeSlot = new List<ParamedicScheduleCustom>();
                List<ParamedicScheduleCustom> ListDisplayWaitingList = new List<ParamedicScheduleCustom>();

                int selectedRowIndex = 0;

                if (obj != null && objSchDate == null)
                {

                    String exp1 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime1, obj.EndTime1);
                    String exp2 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime2, obj.EndTime2);
                    String exp3 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime3, obj.EndTime3);
                    String exp4 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime4, obj.EndTime4);
                    String exp5 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), obj.StartTime5, obj.EndTime5);

                    List<Appointment> lst1 = BusinessLayer.GetAppointmentList(exp1);
                    List<Appointment> lst2 = BusinessLayer.GetAppointmentList(exp2);
                    List<Appointment> lst3 = BusinessLayer.GetAppointmentList(exp3);
                    List<Appointment> lst4 = BusinessLayer.GetAppointmentList(exp4);
                    List<Appointment> lst5 = BusinessLayer.GetAppointmentList(exp5);

                    if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "2" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "3" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "4" });
                        lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", obj.StartTime5, obj.EndTime5), StandardCodeID = "5" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "2" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "3" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "4" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "2" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "3" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "2" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                        Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                    }


                }
                // untuk dokter yang hanya punya paramedicscheduleDate Saja
                else
                {
                    if (objSchDate != null)
                    {

                        String expSch1 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime1, objSchDate.EndTime1);
                        String expSch2 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime2, objSchDate.EndTime2);
                        String expSch3 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime3, objSchDate.EndTime3);
                        String expSch4 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime4, objSchDate.EndTime4);
                        String expSch5 = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, HealthcareServiceUnitID, selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime5, objSchDate.EndTime5);

                        List<Appointment> lstSch1 = BusinessLayer.GetAppointmentList(expSch1);
                        List<Appointment> lstSch2 = BusinessLayer.GetAppointmentList(expSch2);
                        List<Appointment> lstSch3 = BusinessLayer.GetAppointmentList(expSch3);
                        List<Appointment> lstSch4 = BusinessLayer.GetAppointmentList(expSch4);
                        List<Appointment> lstSch5 = BusinessLayer.GetAppointmentList(expSch5);

                        if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "1" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "2" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "3" });
                            lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "4" });
                            lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", obj.StartTime5, obj.EndTime5), StandardCodeID = "5" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "1" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "2" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "3" });
                            lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objSchDate.StartTime4, objSchDate.EndTime4), StandardCodeID = "4" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "1" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "2" });
                            lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "3" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "1" });
                            lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "2" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            List<StandardCode> lstSession = new List<StandardCode>();
                            lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "1" });
                            Methods.SetComboBoxField<StandardCode>(cboSession, lstSession, "StandardCodeName", "StandardCodeID");
                        }
                    }
                    else
                    {
                        trRemarks.Style.Remove("display");
                        txtRemarks.Text = string.Format("tidak ada jadwal untuk tanggal {0}", txtDateApm.Text);
                    }
                }

            }
        }

        public class ParamedicScheduleCustom
        {
            public Int32 ID { get; set; }
            public Int32 Queue { get; set; }
            public String Time { get; set; }
            public String EndTime { get; set; }
            public Int32 ParentID { get; set; }
        }
        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(txtDateApm.Text);
            List<GetParamedicLeaveScheduleCompare> objLeave = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), ParamedicID);

            #region validate time slot
            #region if leave in period
            if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() > 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    if (obj.DayNumber == objLeave.FirstOrDefault().DayNumber && objLeave.FirstOrDefault().Date == selectedDate)
                    {
                        DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime5 = "";
                                obj.EndTime5 = "";
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                            {
                                obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay)
                            {
                                obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay)
                            {
                                obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime2 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                            }
                        }
                    }
                    else if (obj.DayNumber == objLeave.LastOrDefault().DayNumber && objLeave.LastOrDefault().Date == selectedDate)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:15", objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);
                        DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                        DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                        DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                        DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                        DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                        DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                        DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                        DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                        DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                        DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                        if (obj.StartTime5 != "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart5.TimeOfDay)
                            {
                                obj.StartTime5 = endTime.ToString("HH:mm");
                                obj.StartTime4 = "";
                                obj.EndTime4 = "";
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime4 != "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart4.TimeOfDay)
                            {
                                obj.StartTime4 = endTime.ToString("HH:mm");
                                obj.StartTime3 = "";
                                obj.EndTime3 = "";
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart3.TimeOfDay)
                            {
                                obj.StartTime3 = endTime.ToString("HH:mm");
                                obj.StartTime2 = "";
                                obj.EndTime2 = "";
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                        else if (obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objStart2.TimeOfDay)
                            {
                                obj.StartTime2 = endTime.ToString("HH:mm");
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objStart1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                        {
                            if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay)
                            {
                                obj.StartTime1 = "";
                                obj.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            obj.StartTime1 = "";
                            obj.StartTime2 = "";
                            obj.StartTime3 = "";
                            obj.StartTime4 = "";
                            obj.StartTime5 = "";

                            obj.EndTime1 = "";
                            obj.EndTime2 = "";
                            obj.EndTime3 = "";
                            obj.EndTime4 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
                    {
                        DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));
                        if (objSchDate.StartTime5 != "")
                        {

                            if (startTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.EndTime5 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime5 = "";
                                objSchDate.EndTime5 = "";
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                            {
                                objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay)
                            {
                                objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay)
                            {
                                objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                            }
                            else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                            }
                        }
                    }
                    else if (objSchDate.ScheduleDate == objLeave.LastOrDefault().Date)
                    {
                        DateTime endTime = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.LastOrDefault().EndTime));
                        endTime = endTime.AddMinutes(15);

                        DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.LastOrDefault().EndTime);

                        if (objSchDate.StartTime5 != "")
                        {

                            if (endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = endTime.ToString("HH:mm");
                                objSchDate.StartTime4 = "";
                                objSchDate.EndTime4 = "";
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = endTime.ToString("HH:mm");
                                objSchDate.StartTime3 = "";
                                objSchDate.EndTime3 = "";
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = endTime.ToString("HH:mm");
                                objSchDate.StartTime2 = "";
                                objSchDate.EndTime2 = "";
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = endTime.ToString("HH:mm");
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                        else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                        {
                            if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
                            else if (endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = "";
                                objSchDate.EndTime1 = "";
                            }
                        }
                    }
                    else
                    {
                        List<GetParamedicLeaveScheduleCompare> c = objLeave.Where(t => t.Date == selectedDate).ToList();
                        if (c.Count > 0)
                        {
                            objSchDate.StartTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.StartTime5 = "";

                            objSchDate.EndTime1 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #region if leave only in one day
            else if (objLeave.FirstOrDefault().DayNumber != 0 && objLeave.Count() == 1)
            {
                #region set time slot Paramedic Schedule
                if (obj != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                    DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                    DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                    DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                    DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);

                    DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                    DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                    DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                    DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                    DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);

                    if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //1/2
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList2 = obj.IsAllowWaitingList1;
                            obj.MaximumWaitingList2 = obj.MaximumWaitingList1;

                            obj.IsAppointmentByTimeSlot2 = obj.IsAppointmentByTimeSlot1;
                            obj.MaximumAppointment2 = obj.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //2 modif
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //2 modif
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //9
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList3 = obj.IsAllowWaitingList2;
                            obj.MaximumWaitingList3 = obj.MaximumWaitingList2;

                            obj.IsAppointmentByTimeSlot3 = obj.IsAppointmentByTimeSlot2;
                            obj.MaximumAppointment3 = obj.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList4 = obj.IsAllowWaitingList3;
                            obj.MaximumWaitingList4 = obj.MaximumWaitingList3;

                            obj.IsAppointmentByTimeSlot4 = obj.IsAppointmentByTimeSlot3;
                            obj.MaximumAppointment4 = obj.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) //22
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");

                            obj.IsAllowWaitingList5 = obj.IsAllowWaitingList4;
                            obj.MaximumWaitingList5 = obj.MaximumWaitingList4;

                            obj.IsAppointmentByTimeSlot5 = obj.IsAppointmentByTimeSlot4;
                            obj.MaximumAppointment5 = obj.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                    }
                    else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1
                        {
                            obj.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //1/2
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objStart1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd1.TimeOfDay) //2
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = obj.StartTime3;
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            //obj = null;
                            obj.StartTime1 = "";
                            obj.EndTime1 = "";
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime5;
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay == objEnd3.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime1 = obj.StartTime4;
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime4;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay) //3
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime2;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = obj.EndTime1;
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //4
                        {
                            obj.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //5
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay)  //6
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objStart2.AddMinutes(15);
                            obj.StartTime1 = start2.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //8
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //9
                        {
                            obj.StartTime1 = obj.StartTime3;
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //10
                        {
                            obj.StartTime1 = obj.StartTime2;
                            obj.EndTime1 = obj.EndTime2;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objStart2.TimeOfDay) //11
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //12
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //13
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay == objStart2.TimeOfDay) //14
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //15
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //16
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //17
                        {
                            obj.StartTime4 = obj.StartTime3;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = obj.EndTime2;
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay < objEnd2.TimeOfDay) //18
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //19
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objStart3.TimeOfDay) //20
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //21
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime3;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objStart1.TimeOfDay && endTimeDefault.TimeOfDay < objEnd1.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //23
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //24
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay) //25
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //26
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //27
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime3;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //28
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay) //29
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //30
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart3.TimeOfDay) //31
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart3.TimeOfDay) //32
                        {
                            obj.StartTime5 = obj.StartTime4;
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = obj.EndTime3;
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay) //33
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) //34
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objStart4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart4.TimeOfDay && endTimeDefault.TimeOfDay < objEnd4.TimeOfDay)
                        {
                            obj.EndTime5 = obj.EndTime4;
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime4;
                            obj.EndTime2 = obj.EndTime4;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime4;
                            obj.EndTime3 = obj.EndTime4;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objStart5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objStart1.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime1 = endTime.ToString("HH:mm");
                            obj.EndTime1 = obj.EndTime5;
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart1.TimeOfDay && startTimeDefault.TimeOfDay < objEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime1 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime2 = obj.StartTime5;
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = endTime.ToString("HH:mm");
                            obj.EndTime2 = obj.EndTime5;
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime2 = "";
                            obj.EndTime2 = "";
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objStart2.TimeOfDay && startTimeDefault.TimeOfDay < objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.EndTime2 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime3 = "";
                            obj.EndTime3 = "";
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime3 = endTime.ToString("HH:mm");
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime3 = obj.StartTime5;
                            obj.EndTime3 = obj.EndTime5;
                            obj.StartTime4 = "";
                            obj.EndTime4 = "";
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart3.TimeOfDay && startTimeDefault.TimeOfDay < objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime3 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.StartTime4 = obj.StartTime5;
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime4 = endTime.ToString("HH:mm");
                            obj.EndTime4 = obj.EndTime5;
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objStart4.TimeOfDay && startTimeDefault.TimeOfDay < objEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.EndTime4 = startTimeDefault.ToString("HH:mm");
                            obj.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = endTime.ToString("HH:mm");
                            obj.EndTime5 = obj.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay > objStart5.TimeOfDay && endTimeDefault.TimeOfDay < objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = startTime.ToString("HH:mm");
                            obj.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd5.TimeOfDay)
                        {
                            obj.StartTime5 = "";
                            obj.EndTime5 = "";
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    DateTime startTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);
                    DateTime endTimeDefault = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().EndTime);

                    DateTime startTime = startTimeDefault.AddMinutes(15);
                    DateTime endTime = endTimeDefault.AddMinutes(15);

                    DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                    DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                    DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                    DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                    DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

                    DateTime objSchEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);
                    DateTime objSchEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);
                    DateTime objSchEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);
                    DateTime objSchEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);
                    DateTime objSchEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                    if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //1/2
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList2 = objSchDate.IsAllowWaitingList1;
                            objSchDate.MaximumWaitingList2 = objSchDate.MaximumWaitingList1;

                            objSchDate.IsAppointmentByTimeSlot2 = objSchDate.IsAppointmentByTimeSlot1;
                            objSchDate.MaximumAppointment2 = objSchDate.MaximumAppointment1;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //2 modif
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //2 modif
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //9
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList3 = objSchDate.IsAllowWaitingList2;
                            objSchDate.MaximumWaitingList3 = objSchDate.MaximumWaitingList2;

                            objSchDate.IsAppointmentByTimeSlot3 = objSchDate.IsAppointmentByTimeSlot2;
                            objSchDate.MaximumAppointment3 = objSchDate.MaximumAppointment2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList4 = objSchDate.IsAllowWaitingList3;
                            objSchDate.MaximumWaitingList4 = objSchDate.MaximumWaitingList3;

                            objSchDate.IsAppointmentByTimeSlot4 = objSchDate.IsAppointmentByTimeSlot3;
                            objSchDate.MaximumAppointment4 = objSchDate.MaximumAppointment3;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay) //22
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");

                            objSchDate.IsAllowWaitingList5 = objSchDate.IsAllowWaitingList4;
                            objSchDate.MaximumWaitingList5 = objSchDate.MaximumWaitingList4;

                            objSchDate.IsAppointmentByTimeSlot5 = objSchDate.IsAppointmentByTimeSlot4;
                            objSchDate.MaximumAppointment5 = objSchDate.MaximumAppointment4;
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                    {
                        if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1
                        {
                            objSchDate.StartTime1 = startTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //1/2
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay < objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay == objSchEnd1.TimeOfDay) //2
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = objSchDate.StartTime3;
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            //objSchDate = null;
                            objSchDate.StartTime1 = "";
                            objSchDate.EndTime1 = "";
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime5;
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime4;
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime4;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay) //3
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime2;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = objSchDate.EndTime1;
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //4
                        {
                            objSchDate.EndTime1 = objLeave.FirstOrDefault().StartTime;
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //5
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay)  //6
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //7
                        {
                            DateTime start2 = objSchStart2.AddMinutes(15);
                            objSchDate.StartTime1 = start2.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //8
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //9
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime3;
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //10
                        {
                            objSchDate.StartTime1 = objSchDate.StartTime2;
                            objSchDate.EndTime1 = objSchDate.EndTime2;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart2.TimeOfDay) //11
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //12
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //13
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart2.TimeOfDay) //14
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //15
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime2;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //16
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //17
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime3;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = objSchDate.EndTime2;
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay) //18
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //19
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart3.TimeOfDay) //20
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //21
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime3;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //23
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //24
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay) //25
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay) //26
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //27
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime3;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //28
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay) //29
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //30
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay <= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay) //31
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay >= objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart3.TimeOfDay) //32
                        {
                            objSchDate.StartTime5 = objSchDate.StartTime4;
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = objSchDate.EndTime3;
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay) //33
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay) //34
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay)
                        {
                            objSchDate.EndTime5 = objSchDate.EndTime4;
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime4;
                            objSchDate.EndTime2 = objSchDate.EndTime4;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart4.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime4;
                            objSchDate.EndTime3 = objSchDate.EndTime4;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay == objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay <= objSchStart1.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            objSchDate.EndTime1 = objSchDate.EndTime5;
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart1.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd1.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime1 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = objSchDate.StartTime5;
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = endTime.ToString("HH:mm");
                            objSchDate.EndTime2 = objSchDate.EndTime5;
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime2 = "";
                            objSchDate.EndTime2 = "";
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd1.TimeOfDay && startTimeDefault.TimeOfDay > objSchStart2.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime2 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime3 = "";
                            objSchDate.EndTime3 = "";
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = endTime.ToString("HH:mm");
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime3 = objSchDate.StartTime5;
                            objSchDate.EndTime3 = objSchDate.EndTime5;
                            objSchDate.StartTime4 = "";
                            objSchDate.EndTime4 = "";
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart3.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime3 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchEnd3.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchEnd3.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = objSchDate.StartTime5;
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime4 = endTime.ToString("HH:mm");
                            objSchDate.EndTime4 = objSchDate.EndTime5;
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objSchStart5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay > objSchStart4.TimeOfDay && startTimeDefault.TimeOfDay < objSchEnd4.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.EndTime4 = startTimeDefault.ToString("HH:mm");
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = endTime.ToString("HH:mm");
                            objSchDate.EndTime5 = objSchDate.EndTime5;
                        }
                        else if (startTimeDefault.TimeOfDay >= objSchEnd4.TimeOfDay && startTimeDefault.TimeOfDay < objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay > objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay < objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = startTime.ToString("HH:mm");
                            objSchDate.EndTime5 = endTimeDefault.ToString("HH:mm");
                        }
                        else if (startTimeDefault.TimeOfDay == objSchStart5.TimeOfDay && endTimeDefault.TimeOfDay >= objSchEnd5.TimeOfDay)
                        {
                            objSchDate.StartTime5 = "";
                            objSchDate.EndTime5 = "";
                        }
                    }
                }
                #endregion
            }
            #endregion
            #endregion
        }

        private void BridgingToMedinfrasMobileApps(string eventType, string fromAppointmentDate, string toAppointmentDate, int appointmentRequestID)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.IsSuccess = true;
                entityAPILog.MessageDateTime = DateTime.Now;
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "MOBILE APPS";
                if (eventType == "import")
                {
                    string apiResult = oService.OnGetAppointmentRequest(fromAppointmentDate, toAppointmentDate);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.ErrorMessage = apiResultInfo[2];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                else if (eventType == "save")
                {
                    string apiResult = oService.OnSaveAppointmentRequest(appointmentRequestID);
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}