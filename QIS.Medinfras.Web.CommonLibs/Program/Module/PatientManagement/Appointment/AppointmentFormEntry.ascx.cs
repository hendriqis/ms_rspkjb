using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Text;
using System.IO;
using DevExpress.Web.ASPxCallbackPanel;
using System.Net;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentFormEntry : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                string[] paramInfo = param.Split('|');
                hdnDepartmentID.Value = paramInfo[0];
                hdnAppointmentMode.Value = paramInfo[1];
                IsAdd = paramInfo[2] == "1";
                SetControlProperties(paramInfo);
            }
        }

        protected string GetCustomerTypePersonal()
        {
            return Constant.CustomerType.PERSONAL;
        }

        protected string GetCustomerTypeHealthcare()
        {
            return Constant.CustomerType.HEALTHCARE;
        }

        protected string GetCustomerFilterExpression()
        {
            return string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM CustomerContract WHERE '{0}' BETWEEN StartDate AND EndDate AND IsDeleted = 0)", DateTime.Now.ToString("yyyyMMdd"));
        }

        private void SetControlProperties(string[] paramInfo)
        {
            //hdnAppointmentMode.Value = paramInfo[2];
            hdnQueueNo.Value = paramInfo[3];
            divQueueNo.InnerText = paramInfo[3];
            hdnHealthcareServiceUnitID.Value = paramInfo[5];
            txtServiceUnit.Text = paramInfo[6];
            hdnParamedicID.Value = paramInfo[7];
            txtPhysician.Text = paramInfo[8];
            txtAppointmentTime.Text = paramInfo[9];
            hdnSessionID.Value = paramInfo[10];

            txtAppointmentDate.Text = Helper.GetDatePickerValue(paramInfo[4]).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnAppointmentDate.Value = Helper.GetDatePickerValue(paramInfo[4]).ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (hdnAppointmentMode.Value == "2")
            {
                txtAppointmentTime.Enabled = false;
            }

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(String.Format(
                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}')"
                            , AppSession.UserLogin.HealthcareID //0
                            , Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC // 1
                    ));

            hdnIsMobilePhoneNumeric.Value = lstSetvarDt.Where(p => p.ParameterCode == Constant.SettingParameter.RM_IS_MOBILE_PHONE_NUMERIC).FirstOrDefault().ParameterValue; 

        }

        protected override void OnControlEntrySetting()
        {
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')"
                , AppSession.UserLogin.HealthcareID
                , Constant.SettingParameter.OP_ALLOW_RESCHEDULE_BACK_DATE
                , Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI               
                , Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM
                , Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY
                , Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL
                , Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE
                , Constant.SettingParameter.SA_MAX_APPOINTMENT_VALIDATION
                ));

            string setvarImaging = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

            hdnIsBridgingToGateway.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnTipeCustomerBPJS.Value = AppSession.TipeCustomerBPJS;
            hdnIsUsingValidationMaxAppointment.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.SA_MAX_APPOINTMENT_VALIDATION).FirstOrDefault().ParameterValue;
            hdnDefaultServiceUnitInterval.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL).FirstOrDefault().ParameterValue;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0 AND IsActive = 1",
                                                                                                    Constant.StandardCode.SALUTATION,
                                                                                                    Constant.StandardCode.CUSTOMER_TYPE,
                                                                                                    Constant.StandardCode.APPOINTMENT_METHOD,
                                                                                                    Constant.StandardCode.GENDER,
                                                                                                    Constant.StandardCode.REFERRAL
                                                                                                ));
            Methods.SetComboBoxField<StandardCode>(cboAppointmentPayer, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.CUSTOMER_TYPE).OrderByDescending(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
            cboAppointmentPayer.SelectedIndex = 0;

            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutationAppo, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.SALUTATION).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboAppointmentMethod, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.APPOINTMENT_METHOD).OrderBy(t => t.StandardCodeID).ToList(), "StandardCodeName", "StandardCodeID");
            cboAppointmentMethod.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboGenderAppointment, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.GENDER).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField<ClassCare>(cboControlClassCare, lstClassCare, "ClassName", "ClassID");
            cboControlClassCare.SelectedIndex = 0;

            hdnGCTariffSchemePersonal.Value = BusinessLayer.GetCustomer(1).GCTariffScheme;
        }

        private void ControlToEntity(Appointment entity)
        {
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);

            bool isValidMRN = true;
            bool newQueueValid = true;
            int newQueue = 0;
            AppointmentDao entityDao = new AppointmentDao(ctx);
            GuestDao entityGuestDao = new GuestDao(ctx);
            ServiceUnitParamedicDao entityServiceUnitParamedicDao = new ServiceUnitParamedicDao(ctx);
            Appointment entity = null;
            Guest entityGuest = null;

            try
            {
                Int32 HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(hdnAppointmentDate.Value);
                String appointmentID = "";

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

                #region Get / Validate Start and End Time
                int hour = 0;
                int minute = 0;
                string startTimeValidInString = "";
                DateTime stAppo = DateTime.Now;
                DateTime stAppoValid = DateTime.Now;
                DateTime enAppo = DateTime.Now;
                string timeSlot = hdnSessionID.Value;
                string startTimeCheck = "";
                string endTimeCheck = "";

                if (hdnAppointmentMode.Value == "1") //Time-Slot
                {
                    hour = Convert.ToInt32(Request.Form[txtAppointmentTime.UniqueID].Substring(0, 2));
                    minute = Convert.ToInt32(Request.Form[txtAppointmentTime.UniqueID].Substring(3));
                    startTimeValidInString = Request.Form[txtAppointmentTime.UniqueID];

                    if (timeSlot == "0")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime1 != "")
                            {
                                startTimeCheck = objSchDate.StartTime1;
                                endTimeCheck = objSchDate.EndTime1;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime1 != "")
                            {
                                startTimeCheck = obj.StartTime1;
                                endTimeCheck = obj.EndTime1;
                            }
                        }
                    }
                    else if (timeSlot == "1")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime2 != "")
                            {
                                startTimeCheck = objSchDate.StartTime2;
                                endTimeCheck = objSchDate.EndTime2;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime2 != "")
                            {
                                startTimeCheck = obj.StartTime2;
                                endTimeCheck = obj.EndTime2;
                            }
                        }
                    }
                    else if (timeSlot == "2")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime3 != "")
                            {
                                startTimeCheck = objSchDate.StartTime3;
                                endTimeCheck = objSchDate.EndTime3;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime3 != "")
                            {
                                startTimeCheck = obj.StartTime3;
                                endTimeCheck = obj.EndTime3;
                            }
                        }
                    }
                    else if (timeSlot == "3")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime4 != "")
                            {
                                startTimeCheck = objSchDate.StartTime4;
                                endTimeCheck = objSchDate.EndTime4;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime4 != "")
                            {
                                startTimeCheck = obj.StartTime4;
                                endTimeCheck = obj.EndTime4;
                            }
                        }
                    }
                    else if (timeSlot == "4")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime5 != "")
                            {
                                startTimeCheck = objSchDate.StartTime5;
                                endTimeCheck = objSchDate.EndTime5;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime5 != "")
                            {
                                startTimeCheck = obj.StartTime5;
                                endTimeCheck = obj.EndTime5;
                            }
                        }
                    }
                }
                else if (hdnAppointmentMode.Value == "2") //No-Time Slot Mode
                {
                    if (timeSlot == "0")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime1 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime1, objSchDate.EndTime1);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime1;
                                endTimeCheck = objSchDate.EndTime1;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime1 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value),selectedDate.ToString("yyyy-MM-dd"), obj.StartTime1, obj.EndTime1);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime1;
                                endTimeCheck = obj.EndTime1;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                    }
                    else if (timeSlot == "1")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime2 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime2, objSchDate.EndTime2);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime2;
                                endTimeCheck = objSchDate.EndTime2;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime2 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), obj.StartTime2, obj.EndTime2);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime2;
                                endTimeCheck = obj.EndTime2;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                    }
                    else if (timeSlot == "2")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime3 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime3, objSchDate.EndTime3);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime3;
                                endTimeCheck = objSchDate.EndTime3;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime3 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), obj.StartTime3, obj.EndTime3);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime3;
                                endTimeCheck = obj.EndTime3;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                    }
                    else if (timeSlot == "3")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime4 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime4, objSchDate.EndTime4);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime4;
                                endTimeCheck = objSchDate.EndTime4;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime4 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), obj.StartTime4, obj.EndTime4);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime4;
                                endTimeCheck = obj.EndTime4;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                    }
                    else if (timeSlot == "4")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime5 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), objSchDate.StartTime5, objSchDate.EndTime5);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime5;
                                endTimeCheck = objSchDate.EndTime5;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                    minute = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime5 != "")
                            {
                                String filter = String.Format("isWaitingList = 0 AND ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{3}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{2}') + ' ' + '{4}')", ParamedicID, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), selectedDate.ToString("yyyy-MM-dd"), obj.StartTime5, obj.EndTime5);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime5;
                                endTimeCheck = obj.EndTime5;

                                if (!String.IsNullOrEmpty(hdnMRN.Value))
                                {
                                    if (String.IsNullOrEmpty(hdnAppointmentID.Value))
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                    else
                                    {
                                        if (lstAppo.Where(t => t.MRN == Convert.ToInt32(hdnMRN.Value) && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.AppointmentID != Convert.ToInt32(hdnAppointmentID.Value)).Count() > 0)
                                        {
                                            isValidMRN = false;
                                        }
                                    }
                                }

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = selectedDate;
                                    hour = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                    minute = Convert.ToInt32(obj.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hour, minute, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]));
                                }
                            }
                        }
                    }
                }

                entity = new Appointment();
                entityGuest = new Guest();

                if (cboReferral.Value != null)
                {
                    entity.GCReferrerGroup = cboReferral.Value.ToString();
                }

                if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                {
                    entity.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                    entity.ReferrerParamedicID = null;
                }
                else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                {
                    entity.ReferrerID = null;
                    entity.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                }
                else
                {
                    entity.ReferrerID = null;
                    entity.ReferrerParamedicID = null;
                }

                string endTimeValidInString = "";

                if (objSchDate != null)
                {
                    if (objSchDate.StartTime1 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot1 == false)
                    {
                        //startTimeValidInString = objSchDate.StartTime1;
                        endTimeValidInString = objSchDate.EndTime1;
                    }
                    else if (objSchDate.StartTime2 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot2 == false)
                    {
                        //startTimeValidInString = objSchDate.StartTime2;
                        endTimeValidInString = objSchDate.EndTime2;
                    }
                    else if (objSchDate.StartTime3 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot3 == false)
                    {
                        //startTimeValidInString = objSchDate.StartTime3;
                        endTimeValidInString = objSchDate.EndTime3;
                    }
                    else if (objSchDate.StartTime4 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot4 == false)
                    {
                        //startTimeValidInString = objSchDate.StartTime4;
                        endTimeValidInString = objSchDate.EndTime4;
                    }
                    else if (objSchDate.StartTime5 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot5 == false)
                    {
                        //startTimeValidInString = objSchDate.StartTime5;
                        endTimeValidInString = objSchDate.EndTime5;
                    }
                    else
                    {
                        DateTime dtEndTime = new DateTime(2000, 1, 1, hour, minute, 0);
                        DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(hdnDefaultServiceUnitInterval.Value));
                        endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                    }
                }
                else if (obj != null && objSchDate == null)
                {
                    if (obj.StartTime1 == startTimeValidInString && obj.IsAppointmentByTimeSlot1 == false)
                    {
                        //startTimeValidInString = obj.StartTime1;
                        endTimeValidInString = obj.EndTime1;
                    }
                    else if (obj.StartTime2 == startTimeValidInString && obj.IsAppointmentByTimeSlot2 == false)
                    {
                        //startTimeValidInString = obj.StartTime2;
                        endTimeValidInString = obj.EndTime2;
                    }
                    else if (obj.StartTime3 == startTimeValidInString && obj.IsAppointmentByTimeSlot3 == false)
                    {
                        startTimeValidInString = obj.StartTime3;
                        endTimeValidInString = obj.EndTime3;
                    }
                    else if (obj.StartTime4 == startTimeValidInString && obj.IsAppointmentByTimeSlot4 == false)
                    {
                        //startTimeValidInString = obj.StartTime4;
                        endTimeValidInString = obj.EndTime4;
                    }
                    else if (obj.StartTime5 == startTimeValidInString && obj.IsAppointmentByTimeSlot5 == false)
                    {
                        //startTimeValidInString = obj.StartTime5;
                        endTimeValidInString = obj.EndTime5;
                    }
                    else
                    {
                        DateTime dtEndTime = new DateTime(2000, 1, 1, hour, minute, 0);
                        DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(hdnDefaultServiceUnitInterval.Value));
                        endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                    }
                }

                DateTime end = new DateTime(2000, 1, 1, hour, minute, 0);
                entity.StartDate = selectedDate;
                entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
              
                if (hdnAppointmentMode.Value == "2")
                {
                    entity.StartTime = stAppoValid.ToString("HH:mm");
                    entity.EndTime = enAppo.ToString("HH:mm");
                }
                else
                {
                    entity.StartTime = Request.Form[txtAppointmentTime.UniqueID];
                    entity.EndTime = end.AddMinutes(entity.VisitDuration).ToString("HH:mm");
                }
                #endregion

                if (!chkIsNewPatient.Checked)
                {
                    entity.MRN = Convert.ToInt32(hdnMRN.Value);
                    entity.IsNewPatient = false;
                }
                else
                {
                    entity.IsNewPatient = true;
                    if (cboSalutationAppo.Value != null)
                    {
                        entity.GCSalutation = (string)cboSalutationAppo.Value;
                        entityGuest.GCSalutation = (string)cboSalutationAppo.Value;
                    }
                    else
                    {
                        entity.GCSalutation = null;
                        entityGuest.GCSalutation = null;
                    }
                    entity.FirstName = txtFirstName.Text;
                    entity.MiddleName = txtMiddleName.Text;
                    entity.LastName = txtFamilyName.Text;
                    entity.StreetName = txtAddress.Text;
                    entity.PhoneNo = txtPhoneNo.Text;
                    entity.MobilePhoneNo = txtMobilePhone.Text;
                    entity.EmailAddress = txtEmail.Text;
                    entity.CorporateAccountNo = txtCorporateAccountNo.Text;
                    entity.CorporateAccountName = txtCorporateAccountName.Text;
                    entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
                    entity.GCGender = (string)cboGenderAppointment.Value;
                    entity.MRN = null;

                    if (entity.GuestID != null)
                    {
                        entityGuest = entityGuestDao.Get(Convert.ToInt32(entity.GuestID));
                    }
                    else
                    {
                        entityGuest = new Guest();
                    }

                    entityGuest.FirstName = txtFirstName.Text;
                    entityGuest.MiddleName = txtMiddleName.Text;
                    entityGuest.LastName = txtFamilyName.Text;
                    entityGuest.FullName = Helper.GenerateFullName(entityGuest.FirstName, entityGuest.MiddleName, entityGuest.LastName);
                    entityGuest.StreetName = txtAddress.Text;
                    entityGuest.PhoneNo = txtPhoneNo.Text;
                    entityGuest.MobilePhoneNo = txtMobilePhone.Text;
                    entityGuest.EmailAddress = txtEmail.Text;
                    entityGuest.CorporateAccountNo = txtCorporateAccountNo.Text;
                    entityGuest.CorporateAccountName = txtCorporateAccountName.Text;
                    entityGuest.GCGender = (string)cboGenderAppointment.Value;

                    if (Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]).ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                    {
                        entityGuest.DateOfBirth = Helper.GetDatePickerValue(Request.Form[txtDOBMainAppt.UniqueID]);
                    }
                    entityGuest.IsDeleted = false;
                }

                if (!chkIsNewPatient.Checked)
                {
                    UpdatePatientData(ctx, (int)entity.MRN);
                }
                else
                {
                    if (entity.GuestID != null)
                    {
                        entityGuest.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityGuestDao.Update(entityGuest);
                    }
                    else
                    {
                        entityGuest.GuestNo = BusinessLayer.GenerateGuestNo(DateTime.Now);
                        entityGuest.CreatedBy = AppSession.UserLogin.UserID;
                        entityGuestDao.Insert(entityGuest);
                    }
                }

                entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (hdnRoomID.Value != "0" && hdnRoomID != null && hdnRoomID.Value != "")
                    entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
                else
                    entity.RoomID = null;
                entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
                if (cboReferral.Value != null)
                {
                    entity.GCReferrerGroup = cboReferral.Value.ToString();
                }
                if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                {
                    entity.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                    entity.ReferrerParamedicID = null;
                }
                else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                {
                    entity.ReferrerID = null;
                    entity.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                }
                else
                {
                    entity.ReferrerID = null;
                    entity.ReferrerParamedicID = null;
                }

                entity.GCCustomerType = cboAppointmentPayer.Value.ToString();
                entity.IsUsingCOB = chkIsUsingCOB.Checked;

                if (entity.GCCustomerType == Constant.CustomerType.PERSONAL)
                {
                    entity.BusinessPartnerID = 1;
                    entity.ContractID = null;
                    entity.CoverageTypeID = null;
                    entity.CoverageLimitAmount = 0;
                    entity.IsCoverageLimitPerDay = false;
                    entity.GCTariffScheme = hdnGCTariffSchemePersonal.Value;
                    entity.IsControlClassCare = false;
                    entity.ControlClassID = null;
                    entity.EmployeeID = null;
                }
                else
                {
                    entity.BusinessPartnerID = Convert.ToInt32(hdnPayerID.Value);
                    entity.ContractID = Convert.ToInt32(hdnContractID.Value);
                    entity.CoverageTypeID = Convert.ToInt32(hdnCoverageTypeID.Value);
                    entity.CorporateAccountNo = txtParticipantNo.Text;

                    if (!String.IsNullOrEmpty(txtCoverageLimit.Text))
                    {
                        entity.CoverageLimitAmount = Convert.ToDecimal(txtCoverageLimit.Text);
                    }
                    else
                    {
                        entity.CoverageLimitAmount = 0;
                    }

                    entity.IsCoverageLimitPerDay = chkIsCoverageLimitPerDay.Checked;
                    entity.GCTariffScheme = hdnGCTariffScheme.Value;
                    entity.IsControlClassCare = (hdnIsControlClassCare.Value == "1");
                    if (entity.IsControlClassCare)
                        entity.ControlClassID = Convert.ToInt32(cboControlClassCare.Value);
                    else
                        entity.ControlClassID = null;
                    if (hdnEmployeeID.Value == "" || hdnEmployeeID.Value == "0")
                        entity.EmployeeID = null;
                    else
                        entity.EmployeeID = Convert.ToInt32(hdnEmployeeID.Value);
                }

                bool isBPJS = hdnIsBPJS.Value == "1";
                entity.Session = Convert.ToInt32(hdnSessionID.Value) + 1;
                if (hdnAppointmentMode.Value == "2")
                {
                    //No-Time Slot Mode
                    entity.QueueNo = Convert.ToInt16(hdnQueueNo.Value);
                }
                else
                {
                    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, entity.ParamedicID, selectedDate, Convert.ToInt32(entity.Session), false, isBPJS, 1, ctx));
                }

                entity.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                switch (hdnDepartmentID.Value)
                {
                    case "OP":
                        entity.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                        break;
                    default:
                        entity.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                        break;
                }
                appointmentID = entity.AppointmentNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.StartDate);
                entity.GCAppointmentMethod = (string)cboAppointmentMethod.Value;
              
                entity.Notes = txtRemarks.Text;
                entity.IsWaitingList = false;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityDao.Insert(entity);
                ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientAssessmentDao entityDao = new PatientAssessmentDao(ctx);
            //RegistrationDao regDao = new RegistrationDao(ctx);
            //try
            //{
            //    PatientAssessment entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
            //    ControlToEntity(entity);
            //    entity.AssessmentFormLayout = hdnDivHTML.Value;
            //    entity.AssessmentFormValue = hdnFormValues.Value;
            //    entity.CreatedBy = AppSession.UserLogin.UserID;
            //    entityDao.Update(entity);

            //    ctx.CommitTransaction();
            //}
            //catch (Exception ex)
            //{
            //    result = false;
            //    errMessage = ex.Message;
            //    Helper.InsertErrorLog(ex);
            //    ctx.RollBackTransaction();
            //}
            //finally
            //{
            //    ctx.Close();
            //}
            return result;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        private void UpdatePatientData(IDbContext ctx, int MRN)
        {
            PatientDao entityPatientDao = new PatientDao(ctx);
            AddressDao entityAddressDao = new AddressDao(ctx);
            Patient entityPatient = entityPatientDao.Get(MRN);
            entityPatient.MobilePhoneNo1 = txtMobilePhone.Text;
            entityPatient.EmailAddress = txtEmail.Text;

            Address entityAddress = entityAddressDao.Get((int)entityPatient.HomeAddressID);
            entityAddress.PhoneNo1 = txtPhoneNo.Text;
            entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;

            entityPatientDao.Update(entityPatient);
            entityAddressDao.Update(entityAddress);
        }
    }
}