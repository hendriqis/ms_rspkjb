using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentRescheduleV2Ctl : BaseEntryPopupCtl
    {
        protected string GetErrorMsgAppointmentSlot()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_APPOINTMENT_SLOT_VALIDATION);
        }

        protected int PageCount = 1;
        protected int PageCount1 = 1;
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnDepartmentIDCtlReschedule.Value = queryString;
            SetControlProperties();
            BindGridPhysicianFrom(1, true, ref PageCount);
            BindGridPhysicianTo(1, true, ref PageCount1);
        }

        protected void SetControlProperties()
        {
            int serviceUnitUserCount = 0;
            string filterExpression = "";
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.OP0035));
            string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnChangeAppointmentCreateNewAppointment.Value = setvar.Where(w => w.ParameterCode == Constant.SettingParameter.OP0035).FirstOrDefault().ParameterValue;

            if (hdnDepartmentIDCtlReschedule.Value == "OP")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "IS")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "LB")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "MD")
            {
                serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND HealthcareServiceUnitID NOT IN ('{3}','{4}')", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory));
            }

            if (hdnDepartmentIDCtlReschedule.Value == "OP")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "IS")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, setvarImaging);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", setvarImaging, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "LB")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND HealthcareServiceUnitID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, setvarLaboratory);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE HealthcareServiceUnitID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", setvarLaboratory, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
            }
            else if (hdnDepartmentIDCtlReschedule.Value == "MD")
            {
                filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0 AND HealthcareServiceUnitID NOT IN ('{2}','{3}')", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, setvarImaging, setvarLaboratory);
                if (serviceUnitUserCount > 0)
                    filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0 AND HealthcareServiceUnitID NOT IN ('{3}','{4}'))", Constant.Facility.DIAGNOSTIC, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarImaging, setvarLaboratory);
            }

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression, int.MaxValue, 1, "ServiceUnitName");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitRescheduleFrom, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitRescheduleTo, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitRescheduleFrom.SelectedIndex = 0;
            cboServiceUnitRescheduleTo.SelectedIndex = 0;

            //List<StandardCode> lstSession = new List<StandardCode>();
            //lstSession.Insert(0, new StandardCode { StandardCodeName = "Session 1", StandardCodeID = "0" });
            //lstSession.Insert(1, new StandardCode { StandardCodeName = "Session 2", StandardCodeID = "1" });
            //lstSession.Insert(2, new StandardCode { StandardCodeName = "Session 3", StandardCodeID = "2" });
            //lstSession.Insert(3, new StandardCode { StandardCodeName = "Session 4", StandardCodeID = "3" });
            //lstSession.Insert(4, new StandardCode { StandardCodeName = "Session 5", StandardCodeID = "4" });
            //Methods.SetComboBoxField<StandardCode>(cboSessionRescheduleCtlFrom, lstSession, "StandardCodeName", "StandardCodeID");
            //cboSessionRescheduleCtlFrom.SelectedIndex = 0;

            hdnDefaultServiceUnitInterval.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL).ParameterValue;
            hdnCalAppointmentSelectedDateRescheduleCtlFrom.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnCalAppointmentSelectedDateRescheduleCtlTo.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNewAppointmentDate, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtNewAppointmentDateTo, new ControlEntrySetting(false, false, true, Constant.DefaultValueEntry.DATE_NOW));
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicIDCtlFrom.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlFrom.Value);
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
                            if (objStart1.TimeOfDay <= endTimeDefault.TimeOfDay)
                            {
                                obj.StartTime1 = endTime.ToString("HH:mm");
                            }
                        }
                    }
                }
                #endregion

                #region set time slot Paramedic Schedule Date
                if (objSchDate != null)
                {
                    if (objSchDate.ScheduleDate == objLeave.FirstOrDefault().Date)
                    {
                        DateTime startTimeDefault = DateTime.Parse(String.Format("{0} {1}:00", objSchDate.cfScheduleDateInString, objLeave.FirstOrDefault().StartTime));

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
                        DateTime objSchStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                        DateTime objSchStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                        DateTime objSchStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                        DateTime objSchStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                        DateTime objSchStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);

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
                            if (objSchStart1.TimeOfDay <= endTimeDefault.TimeOfDay)
                            {
                                objSchDate.StartTime1 = endTime.ToString("HH:mm");
                            }
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

        protected void cboSessionRescheduleCtlFrom_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            Int32 ParamedicIDFrom = Convert.ToInt32(hdnParamedicIDCtlFrom.Value);
            Int32 HealthcareParamedicFrom = Convert.ToInt32(cboServiceUnitRescheduleFrom.Value);
            DateTime selectedDateFrom = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlFrom.Value);

            List<StandardCode> lstSession = new List<StandardCode>();

            #region Take Master Schedule FROM
            int dayNumberFrom = (int)selectedDateFrom.DayOfWeek;
            if (dayNumberFrom == 0)
            {
                dayNumberFrom = 7;
            }

            vParamedicSchedule objFrom = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                        HealthcareParamedicFrom, ParamedicIDFrom, dayNumberFrom)).FirstOrDefault();

            vParamedicScheduleDate objSchDateFrom = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                            HealthcareParamedicFrom, ParamedicIDFrom, selectedDateFrom)).FirstOrDefault();

            ValidateParamedicScSchedule(objFrom, objSchDateFrom);

            if (objSchDateFrom != null)
            {
                if (objSchDateFrom.StartTime1 == "" && objSchDateFrom.StartTime2 == "" && objSchDateFrom.StartTime3 == "" && objSchDateFrom.StartTime4 == "" && objSchDateFrom.StartTime5 == "")
                {
                    objSchDateFrom = null;
                }
            }

            if (objFrom != null)
            {
                if (objFrom.StartTime1 == "" && objFrom.StartTime2 == "" && objFrom.StartTime3 == "" && objFrom.StartTime4 == "" && objFrom.StartTime5 == "")
                {
                    objFrom = null;
                }
            }

            if (objSchDateFrom != null)
            {
                if (objSchDateFrom.StartTime1 != "")
                {
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDateFrom.StartTime1, objSchDateFrom.EndTime1), StandardCodeID = "0" });
                }

                if (objSchDateFrom.StartTime2 != "")
                {
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDateFrom.StartTime2, objSchDateFrom.EndTime2), StandardCodeID = "1" });
                }

                if (objSchDateFrom.StartTime3 != "")
                {
                    lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDateFrom.StartTime3, objSchDateFrom.EndTime3), StandardCodeID = "2" });
                }

                if (objSchDateFrom.StartTime4 != "")
                {
                    lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objSchDateFrom.StartTime4, objSchDateFrom.EndTime4), StandardCodeID = "3" });
                }

                if (objSchDateFrom.StartTime5 != "")
                {
                    lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", objSchDateFrom.StartTime5, objSchDateFrom.EndTime5), StandardCodeID = "4" });
                }
            }
            else
            {
                if (objFrom.StartTime1 != "")
                {
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objFrom.StartTime1, objFrom.EndTime1), StandardCodeID = "0" });
                }

                if (objFrom.StartTime2 != "")
                {
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objFrom.StartTime2, objFrom.EndTime2), StandardCodeID = "1" });
                }

                if (objFrom.StartTime3 != "")
                {
                    lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objFrom.StartTime3, objFrom.EndTime3), StandardCodeID = "2" });
                }

                if (objFrom.StartTime4 != "")
                {
                    lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objFrom.StartTime4, objFrom.EndTime4), StandardCodeID = "3" });
                }

                if (objFrom.StartTime5 != "")
                {
                    lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", objFrom.StartTime5, objFrom.EndTime5), StandardCodeID = "4" });
                }
            }
            #endregion

            Methods.SetComboBoxField<StandardCode>(cboSessionRescheduleCtlFrom, lstSession, "StandardCodeName", "StandardCodeID");
            cboSessionRescheduleCtlFrom.SelectedIndex = 0;
        }

        private void BindGridPhysicianFrom(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlFrom.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = string.Format(
                    "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1} UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = {0} AND ScheduleDate = '{2}') AND IsDeleted = 0",
                    cboServiceUnitRescheduleFrom.Value, daynumber, selectedDate.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ParamedicName");

            #region validate paramedic List (Exclude Paramedic Leave)
            foreach (vParamedicMaster e in lstEntity.ToList())
            {
                List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                        cboServiceUnitRescheduleFrom.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


                vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                    cboServiceUnitRescheduleFrom.Value, e.ParamedicID, daynumber)).FirstOrDefault();

                //cek untuk yang jadwal yang tidak ada tanggal apakah ada yang sama dengan jadwal cuti atau tidak (Validasi ParamedicSchedule)
                if ((et.Where(t => t.DayNumber == daynumber).Count()) > 0)
                {
                    if (ParamedicSchedule != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicSchedule.DayNumber == et.FirstOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                    else
                                    {
                                        ParamedicSchedule = null;
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicSchedule.DayNumber == et.LastOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }

                //cek apakah ada jadwal cuti atau tidak di tanggal yang dipilih (Validasi ParamedicScheduleDate)
                if (et.FirstOrDefault().DayNumber != 0)
                {
                    if (ParamedicScheduleDate != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicScheduleDate.ScheduleDate == et.FirstOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {

                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicScheduleDate.ScheduleDate == et.LastOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }

                                if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                {
                                    if (ParamedicSchedule == null)
                                    {
                                        lstEntity.Remove(e);
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }
            }
            #endregion

            grdPhysician.DataSource = lstEntity;
            grdPhysician.DataBind();
        }

        private void BindGridPhysicianTo(int pageIndex, bool isCountPageCount, ref int pageCount1)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlTo.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = string.Format(
                    "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1} UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = {0} AND ScheduleDate = '{2}') AND IsDeleted = 0",
                    cboServiceUnitRescheduleTo.Value, daynumber, selectedDate.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount1 = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ParamedicName");

            #region validate paramedic List (Exclude Paramedic Leave)
            foreach (vParamedicMaster e in lstEntity.ToList())
            {
                List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                        cboServiceUnitRescheduleTo.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


                vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                    cboServiceUnitRescheduleTo.Value, e.ParamedicID, daynumber)).FirstOrDefault();

                //cek untuk yang jadwal yang tidak ada tanggal apakah ada yang sama dengan jadwal cuti atau tidak (Validasi ParamedicSchedule)
                if ((et.Where(t => t.DayNumber == daynumber).Count()) > 0)
                {
                    if (ParamedicSchedule != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicSchedule.DayNumber == et.FirstOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                    else
                                    {
                                        ParamedicSchedule = null;
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicSchedule.DayNumber == et.LastOrDefault().DayNumber)
                        {
                            if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                            else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicSchedule.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicScheduleDate == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                        else
                                        {
                                            ParamedicSchedule = null;
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }

                //cek apakah ada jadwal cuti atau tidak di tanggal yang dipilih (Validasi ParamedicScheduleDate)
                if (et.FirstOrDefault().DayNumber != 0)
                {
                    if (ParamedicScheduleDate != null)
                    {
                        //cek apakah jadwal praktek adalah hari pertama cuti atau tidak
                        if (ParamedicScheduleDate.ScheduleDate == et.FirstOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime StartScheduleDateInString5 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString5.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime StartScheduleDateInString4 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString4.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime StartScheduleDateInString3 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {

                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString3.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString1 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime StartScheduleDateInString2 = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString2.TimeOfDay && LeaveDateInString.TimeOfDay <= StartScheduleDateInString1.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime StartScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.StartTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.FirstOrDefault().StartTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (LeaveDateInString.TimeOfDay <= StartScheduleDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //cek apakah jadwal praktek ini adalah hari terakhir cuti atau bukan
                        else if (ParamedicScheduleDate.ScheduleDate == et.LastOrDefault().Date)
                        {
                            if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime5);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime5);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }

                                if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                {
                                    if (ParamedicSchedule == null)
                                    {
                                        lstEntity.Remove(e);
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime4);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime4);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime3);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime3);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime2);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime2);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                            else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                            {
                                DateTime EndScheduleDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + ParamedicScheduleDate.EndTime1);
                                DateTime LeaveDateInString = DateTime.Parse(ParamedicScheduleDate.cfScheduleDateInString + ' ' + et.LastOrDefault().EndTime);

                                if (et.FirstOrDefault().Date == et.LastOrDefault().Date)
                                {
                                    DateTime Start = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.StartTime1);
                                    DateTime End = DateTime.Parse("08-01-2012" + ' ' + ParamedicScheduleDate.EndTime1);

                                    DateTime StartLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().StartTime);
                                    DateTime EndLeaveDateInString = DateTime.Parse("08-01-2012" + ' ' + et.LastOrDefault().EndTime);

                                    if (StartLeaveDateInString.TimeOfDay <= Start.TimeOfDay && EndLeaveDateInString.TimeOfDay >= End.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                                else
                                {
                                    if (EndScheduleDateInString.TimeOfDay <= LeaveDateInString.TimeOfDay)
                                    {
                                        if (ParamedicSchedule == null)
                                        {
                                            lstEntity.Remove(e);
                                        }
                                    }
                                }
                            }
                        }
                        //semua jadwal praktek yang bukan hari pertama dan hari terakhir cuti di buang
                        else
                        {
                            lstEntity.Remove(e);
                        }
                    }
                }
            }
            #endregion

            grdPhysicianTo.DataSource = lstEntity;
            grdPhysicianTo.DataBind();
        }

        protected void cbpPhysicianRescheduleAppointmentFrom_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysicianFrom(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridPhysicianFrom(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpPhysicianRescheduleAppointmentTo_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysicianTo(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridPhysicianTo(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = false;
            IDbContext ctx = DbFactory.Configure(true);
            AppointmentDao appointmentDao = new AppointmentDao(ctx);

            try
            {
                bool isTimeSlot = ChkIsByTimeSlotFrom.Checked;
                String timeSlot = cboSessionRescheduleCtlFrom.Value.ToString();

                Int32 ParamedicIDFrom = Convert.ToInt32(hdnParamedicIDCtlFrom.Value);
                Int32 HealthcareParamedicFrom = Convert.ToInt32(cboServiceUnitRescheduleFrom.Value);
                DateTime selectedDateFrom = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlFrom.Value);
                List<Appointment> lstAppoFrom = new List<Appointment>();
                bool isValidFrom = false;
                string filterFrom = "";

                Int32 ParamedicIDTo = Convert.ToInt32(hdnParamedicIDCtlTo.Value);
                DateTime selectedDateTo = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlTo.Value);
                Int32 HealthcareParamedicTo = Convert.ToInt32(cboServiceUnitRescheduleTo.Value);
                List<Appointment> lstAppoTo = new List<Appointment>();
                DateTime startValid = DateTime.Now;
                DateTime endValid = DateTime.Now;
                bool isValidTo = false;
                string filterTo = "";
                string startTime = "";
                string endTime = "";

                if (ParamedicIDFrom != 0 && ParamedicIDTo != 0)
                {
                    if (ParamedicIDFrom != ParamedicIDTo || HealthcareParamedicFrom != HealthcareParamedicTo || selectedDateFrom != selectedDateTo)
                    {
                        #region Take Master Schedule FROM
                        int dayNumberFrom = (int)selectedDateFrom.DayOfWeek;
                        if (dayNumberFrom == 0)
                        {
                            dayNumberFrom = 7;
                        }

                        vParamedicSchedule objFrom = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                    HealthcareParamedicFrom, ParamedicIDFrom, dayNumberFrom)).FirstOrDefault();

                        vParamedicScheduleDate objSchDateFrom = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                        HealthcareParamedicFrom, ParamedicIDFrom, selectedDateFrom)).FirstOrDefault();

                        ParamedicMaster pmFrom = BusinessLayer.GetParamedicMaster(ParamedicIDFrom);

                        ValidateParamedicScSchedule(objFrom, objSchDateFrom);

                        if (objSchDateFrom != null)
                        {
                            if (objSchDateFrom.StartTime1 == "" && objSchDateFrom.StartTime2 == "" && objSchDateFrom.StartTime3 == "" && objSchDateFrom.StartTime4 == "" && objSchDateFrom.StartTime5 == "")
                            {
                                objSchDateFrom = null;
                            }
                        }

                        if (objFrom != null)
                        {
                            if (objFrom.StartTime1 == "" && objFrom.StartTime2 == "" && objFrom.StartTime3 == "" && objFrom.StartTime4 == "" && objFrom.StartTime5 == "")
                            {
                                objFrom = null;
                            }
                        }
                        #endregion

                        #region check Paramedic Schedule FROM
                        if (isTimeSlot)
                        {
                            if (timeSlot == "0")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (objSchDateFrom.IsAppointmentByTimeSlot1 && objSchDateFrom.StartTime1 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime1, objSchDateFrom.EndTime1);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (objFrom.IsAppointmentByTimeSlot1 && objFrom.StartTime1 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime1, objFrom.EndTime1);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "1")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (objSchDateFrom.IsAppointmentByTimeSlot2 && objSchDateFrom.StartTime2 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime2, objSchDateFrom.EndTime2);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (objFrom.IsAppointmentByTimeSlot2 && objFrom.StartTime2 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime2, objFrom.EndTime2);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "2")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (objSchDateFrom.IsAppointmentByTimeSlot3 && objSchDateFrom.StartTime3 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime3, objSchDateFrom.EndTime3);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (objFrom.IsAppointmentByTimeSlot3 && objFrom.StartTime3 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime3, objFrom.EndTime3);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "3")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (objSchDateFrom.IsAppointmentByTimeSlot4 && objSchDateFrom.StartTime4 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime4, objSchDateFrom.EndTime4);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (objFrom.IsAppointmentByTimeSlot4 && objFrom.StartTime4 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime4, objFrom.EndTime4);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "4")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (objSchDateFrom.IsAppointmentByTimeSlot5 && objSchDateFrom.StartTime5 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime5, objSchDateFrom.EndTime5);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (objFrom.IsAppointmentByTimeSlot5 && objFrom.StartTime5 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime5, objFrom.EndTime5);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (timeSlot == "0")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (!objSchDateFrom.IsAppointmentByTimeSlot1 && objSchDateFrom.StartTime1 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime1, objSchDateFrom.EndTime1);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (!objFrom.IsAppointmentByTimeSlot1 && objFrom.StartTime1 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime1, objFrom.EndTime1);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "1")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (!objSchDateFrom.IsAppointmentByTimeSlot2 && objSchDateFrom.StartTime2 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime2, objSchDateFrom.EndTime2);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (!objFrom.IsAppointmentByTimeSlot2 && objFrom.StartTime2 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime2, objFrom.EndTime2);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "2")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (!objSchDateFrom.IsAppointmentByTimeSlot3 && objSchDateFrom.StartTime3 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime3, objSchDateFrom.EndTime3);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (!objFrom.IsAppointmentByTimeSlot3 && objFrom.StartTime3 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime3, objFrom.EndTime3);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "3")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (!objSchDateFrom.IsAppointmentByTimeSlot4 && objSchDateFrom.StartTime4 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime4, objSchDateFrom.EndTime4);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (!objFrom.IsAppointmentByTimeSlot4 && objFrom.StartTime4 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime4, objFrom.EndTime4);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            else if (timeSlot == "4")
                            {
                                if (objSchDateFrom != null)
                                {
                                    if (!objSchDateFrom.IsAppointmentByTimeSlot5 && objSchDateFrom.StartTime5 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objSchDateFrom.StartTime5, objSchDateFrom.EndTime5);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (objFrom != null)
                                {
                                    if (!objFrom.IsAppointmentByTimeSlot5 && objFrom.StartTime5 != "")
                                    {
                                        filterFrom = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDFrom, Convert.ToInt32(cboServiceUnitRescheduleFrom.Value), selectedDateFrom.ToString("yyyy-MM-dd"), objFrom.StartTime5, objFrom.EndTime5);
                                        lstAppoFrom = BusinessLayer.GetAppointmentList(filterFrom);

                                        if (lstAppoFrom.Count() > 0)
                                        {
                                            isValidFrom = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Tidak Ada Appointment Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>')", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), "NO Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                        }
                        #endregion

                        if (isValidFrom)
                        {
                            #region Take Master Schedule To
                            int dayNumberTo = (int)selectedDateTo.DayOfWeek;
                            if (dayNumberTo == 0)
                            {
                                dayNumberTo = 7;
                            }

                            vParamedicSchedule objTo = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                                        HealthcareParamedicTo, ParamedicIDTo, dayNumberTo)).FirstOrDefault();

                            vParamedicScheduleDate objSchDateTo = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                            HealthcareParamedicTo, ParamedicIDFrom, selectedDateTo)).FirstOrDefault();

                            ParamedicMaster pmTo = BusinessLayer.GetParamedicMaster(ParamedicIDTo);

                            ValidateParamedicScSchedule(objTo, objSchDateTo);

                            if (objSchDateTo != null)
                            {
                                if (objSchDateTo.StartTime1 == "" && objSchDateTo.StartTime2 == "" && objSchDateTo.StartTime3 == "" && objSchDateTo.StartTime4 == "" && objSchDateTo.StartTime5 == "")
                                {
                                    objSchDateTo = null;
                                }
                            }

                            if (objTo != null)
                            {
                                if (objTo.StartTime1 == "" && objTo.StartTime2 == "" && objTo.StartTime3 == "" && objTo.StartTime4 == "" && objTo.StartTime5 == "")
                                {
                                    objTo = null;
                                }
                            }
                            #endregion

                            #region check ParamedicSchedule To
                            if (isTimeSlot)
                            {
                                if (timeSlot == "0")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (objSchDateTo.IsAppointmentByTimeSlot1 && objSchDateTo.StartTime1 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime1, objSchDateTo.EndTime1);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime1;
                                            endTime = objSchDateTo.EndTime1;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objSchDateTo.StartTime1.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objSchDateTo.StartTime1.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objSchDateTo.EndTime1.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objSchDateTo.EndTime1.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 1");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (objTo.IsAppointmentByTimeSlot1 && objTo.StartTime1 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime1, objTo.EndTime1);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime1;
                                            endTime = objTo.EndTime1;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objTo.StartTime1.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objTo.StartTime1.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objTo.EndTime1.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objTo.EndTime1.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 1");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                }
                                else if (timeSlot == "1")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (objSchDateTo.IsAppointmentByTimeSlot2 && objSchDateTo.StartTime2 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime2, objSchDateTo.EndTime2);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime2;
                                            endTime = objSchDateTo.EndTime2;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objSchDateTo.StartTime2.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objSchDateTo.StartTime2.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objSchDateTo.EndTime2.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objSchDateTo.EndTime2.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 2");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (objTo.IsAppointmentByTimeSlot2 && objTo.StartTime2 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime2, objTo.EndTime2);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime2;
                                            endTime = objTo.EndTime2;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objTo.StartTime2.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objTo.StartTime2.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objTo.EndTime2.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objTo.EndTime2.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 2");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                }
                                else if (timeSlot == "2")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (objSchDateTo.IsAppointmentByTimeSlot3 && objSchDateTo.StartTime3 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime3, objSchDateTo.EndTime3);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime3;
                                            endTime = objSchDateTo.EndTime3;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objSchDateTo.StartTime3.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objSchDateTo.StartTime3.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objSchDateTo.EndTime3.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objSchDateTo.EndTime3.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 3");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (objTo.IsAppointmentByTimeSlot3 && objTo.StartTime3 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime3, objTo.EndTime3);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime3;
                                            endTime = objTo.EndTime3;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objTo.StartTime3.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objTo.StartTime3.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objTo.EndTime3.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objTo.EndTime3.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 3");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                }
                                else if (timeSlot == "3")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (objSchDateTo.IsAppointmentByTimeSlot4 && objSchDateTo.StartTime4 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime4, objSchDateTo.EndTime4);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime4;
                                            endTime = objSchDateTo.EndTime4;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objSchDateTo.StartTime4.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objSchDateTo.StartTime4.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objSchDateTo.EndTime4.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objSchDateTo.EndTime4.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 4");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (objTo.IsAppointmentByTimeSlot4 && objTo.StartTime4 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime4, objTo.EndTime4);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime4;
                                            endTime = objTo.EndTime4;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objTo.StartTime4.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objTo.StartTime4.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objTo.EndTime4.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objTo.EndTime4.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 4");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                }
                                else if (timeSlot == "4")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (objSchDateTo.IsAppointmentByTimeSlot5 && objSchDateTo.StartTime5 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime5, objSchDateTo.EndTime5);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime5;
                                            endTime = objSchDateTo.EndTime5;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objSchDateTo.StartTime5.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objSchDateTo.StartTime5.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objSchDateTo.EndTime5.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objSchDateTo.EndTime5.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 5");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (objTo.IsAppointmentByTimeSlot5 && objTo.StartTime5 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime5, objTo.EndTime5);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime5;
                                            endTime = objTo.EndTime5;

                                            if (lstAppoTo.Count() == 0)
                                            {
                                                int hStart = Convert.ToInt32(objTo.StartTime5.Substring(0, 2));
                                                int mStart = Convert.ToInt32(objTo.StartTime5.Substring(3));
                                                startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                                int hEnd = Convert.ToInt32(objTo.EndTime5.Substring(0, 2));
                                                int mEnd = Convert.ToInt32(objTo.EndTime5.Substring(3));
                                                endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);
                                                isValidTo = true;
                                            }
                                            else
                                            {
                                                errMessage = String.Format("Maaf Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Sudah Memiliki Appointment", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "Time Slot", "Sesi 5");
                                                ctx.RollBackTransaction();
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (timeSlot == "0")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (!objSchDateTo.IsAppointmentByTimeSlot1 && objSchDateTo.StartTime1 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime1, objSchDateTo.EndTime1);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime1;
                                            endTime = objSchDateTo.EndTime1;

                                            int hStart = Convert.ToInt32(objSchDateTo.StartTime1.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objSchDateTo.StartTime1.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objSchDateTo.EndTime1.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objSchDateTo.EndTime1.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (!objTo.IsAppointmentByTimeSlot1 && objTo.StartTime1 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime1, objTo.EndTime1);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime1;
                                            endTime = objTo.EndTime1;

                                            int hStart = Convert.ToInt32(objTo.StartTime1.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objTo.StartTime1.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objTo.EndTime1.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objTo.EndTime1.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 1");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (timeSlot == "1")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (!objSchDateTo.IsAppointmentByTimeSlot2 && objSchDateTo.StartTime2 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime2, objSchDateTo.EndTime2);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime2;
                                            endTime = objSchDateTo.EndTime2;

                                            int hStart = Convert.ToInt32(objSchDateTo.StartTime2.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objSchDateTo.StartTime2.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objSchDateTo.EndTime2.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objSchDateTo.EndTime2.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (!objTo.IsAppointmentByTimeSlot2 && objTo.StartTime2 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime2, objTo.EndTime2);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime2;
                                            endTime = objTo.EndTime2;

                                            int hStart = Convert.ToInt32(objTo.StartTime2.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objTo.StartTime2.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objTo.EndTime2.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objTo.EndTime2.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 2");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (timeSlot == "2")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (!objSchDateTo.IsAppointmentByTimeSlot3 && objSchDateTo.StartTime3 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime3, objSchDateTo.EndTime3);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime3;
                                            endTime = objSchDateTo.EndTime3;

                                            int hStart = Convert.ToInt32(objSchDateTo.StartTime3.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objSchDateTo.StartTime3.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objSchDateTo.EndTime3.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objSchDateTo.EndTime3.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (!objTo.IsAppointmentByTimeSlot3 && objTo.StartTime3 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime3, objTo.EndTime3);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime3;
                                            endTime = objTo.EndTime3;

                                            int hStart = Convert.ToInt32(objTo.StartTime3.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objTo.StartTime3.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objTo.EndTime3.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objTo.EndTime3.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 3");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (timeSlot == "3")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (!objSchDateTo.IsAppointmentByTimeSlot4 && objSchDateTo.StartTime4 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime4, objSchDateTo.EndTime4);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime4;
                                            endTime = objSchDateTo.EndTime4;

                                            int hStart = Convert.ToInt32(objSchDateTo.StartTime4.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objSchDateTo.StartTime4.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objSchDateTo.EndTime4.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objSchDateTo.EndTime4.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (!objTo.IsAppointmentByTimeSlot4 && objTo.StartTime4 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime4, objTo.EndTime4);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime4;
                                            endTime = objTo.EndTime4;

                                            int hStart = Convert.ToInt32(objTo.StartTime4.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objTo.StartTime4.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objTo.EndTime4.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objTo.EndTime4.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 4");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                                else if (timeSlot == "4")
                                {
                                    if (objSchDateTo != null)
                                    {
                                        if (!objSchDateTo.IsAppointmentByTimeSlot5 && objSchDateTo.StartTime5 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objSchDateTo.StartTime5, objSchDateTo.EndTime5);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objSchDateTo.StartTime5;
                                            endTime = objSchDateTo.EndTime5;

                                            int hStart = Convert.ToInt32(objSchDateTo.StartTime5.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objSchDateTo.StartTime5.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objSchDateTo.EndTime5.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objSchDateTo.EndTime5.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                    else if (objTo != null)
                                    {
                                        if (!objTo.IsAppointmentByTimeSlot5 && objTo.StartTime5 != "")
                                        {
                                            filterTo = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicIDTo, Convert.ToInt32(cboServiceUnitRescheduleTo.Value), selectedDateTo.ToString("yyyy-MM-dd"), objTo.StartTime5, objTo.EndTime5);
                                            lstAppoTo = BusinessLayer.GetAppointmentList(filterTo);
                                            startTime = objTo.StartTime5;
                                            endTime = objTo.EndTime5;

                                            int hStart = Convert.ToInt32(objTo.StartTime5.Substring(0, 2));
                                            int mStart = Convert.ToInt32(objTo.StartTime5.Substring(3));
                                            startValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hStart, mStart, 0);

                                            int hEnd = Convert.ToInt32(objTo.EndTime5.Substring(0, 2));
                                            int mEnd = Convert.ToInt32(objTo.EndTime5.Substring(3));
                                            endValid = new DateTime(selectedDateTo.Year, selectedDateTo.Month, selectedDateTo.Day, hEnd, mEnd, 0);

                                            isValidTo = true;
                                        }
                                        else
                                        {
                                            errMessage = String.Format("Maaf Jadwal Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk Jadwal '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ditemukan", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), "No Time Slot", "Sesi 5");
                                            ctx.RollBackTransaction();
                                        }
                                    }
                                }
                            }
                            #endregion

                            if (isValidTo)
                            {
                                DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateRescheduleCtlTo.Value);

                                int hourStart = Convert.ToInt32(startTime.Substring(0, 2));
                                int minuteStart = Convert.ToInt32(startTime.Substring(3));

                                int hourEnd = Convert.ToInt32(endTime.Substring(0, 2));
                                int minuteEnd = Convert.ToInt32(endTime.Substring(3));

                                DateTime startNew = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hourStart, minuteStart, 0);
                                DateTime endNew = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, hourEnd, minuteEnd, 0);

                                #region validate VisitType
                                bool isValidFromVisitTpe = false;
                                List<Appointment> visitTypeFrom = lstAppoFrom.GroupBy(x => x.VisitTypeID).Select(a => a.First()).OrderBy(z => z.VisitTypeID).ToList();
                                List<ParamedicVisitType> ps = new List<ParamedicVisitType>();
                                List<ServiceUnitVisitType> sv = new List<ServiceUnitVisitType>();

                                foreach (Appointment x in visitTypeFrom)
                                {
                                    #region take Paramedic Visit Type
                                    ParamedicVisitType tempVisit = BusinessLayer.GetParamedicVisitTypeList(String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND VisitTypeID = {2}", ParamedicIDTo, HealthcareParamedicTo, x.VisitTypeID)).FirstOrDefault();
                                    if (tempVisit != null)
                                    {
                                        ps.Add(tempVisit);
                                    }
                                    #endregion
                                    #region service unit visit Type
                                    ServiceUnitVisitType tempServiceVisit = BusinessLayer.GetServiceUnitVisitTypeList(String.Format("HealthcareServiceUnitID = {0} AND VisitTypeID = {1}", HealthcareParamedicTo, x.VisitTypeID)).FirstOrDefault();
                                    if (tempServiceVisit != null)
                                    {
                                        sv.Add(tempServiceVisit);
                                    }
                                    #endregion
                                }

                                if (ps.Count() > 0)
                                {
                                    if (ps.Count() == visitTypeFrom.Count())
                                    {
                                        isValidFromVisitTpe = true;
                                    }
                                }
                                else
                                {
                                    if (sv.Count() == visitTypeFrom.Count())
                                    {
                                        isValidFromVisitTpe = true;
                                    }
                                }
                                #endregion

                                if (isValidFromVisitTpe)
                                {
                                    Appointment appoEND = null;
                                    if (lstAppoTo.Count() > 0)
                                    {
                                        DateTime AppoToEnd = lstAppoTo.LastOrDefault().StartDate;
                                        int AppohourEnd = Convert.ToInt32(lstAppoTo.LastOrDefault().EndTime.Substring(0, 2));
                                        int AppominuteEnd = Convert.ToInt32(lstAppoTo.LastOrDefault().EndTime.Substring(3));
                                        DateTime AppoToEndTemp = new DateTime(AppoToEnd.Year, AppoToEnd.Month, AppoToEnd.Day, AppohourEnd, AppominuteEnd, 0);

                                        int QueueTemp = 1;
                                        List<int> lstAppointmentID = new List<int>();
                                        foreach (Appointment appoTo in lstAppoTo)
                                        {
                                            appoTo.QueueNo = Convert.ToInt16(QueueTemp);
                                            appoTo.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            appointmentDao.Update(appoTo);
                                            QueueTemp = QueueTemp + 1;
                                        }

                                        DateTime start = AppoToEndTemp;
                                        foreach (Appointment a in lstAppoFrom)
                                        {
                                            if (hdnChangeAppointmentCreateNewAppointment.Value == "0")
                                            {
                                                a.ParamedicID = ParamedicIDTo;
                                                a.StartDate = a.EndDate = selectedDateTo;
                                                a.StartTime = start.ToString("HH:mm");
                                                start = start.AddMinutes(a.VisitDuration);
                                                a.EndTime = start.ToString("HH:mm");
                                                a.QueueNo = Convert.ToInt16(QueueTemp);
                                                a.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                QueueTemp = QueueTemp + 1;
                                                a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                appointmentDao.Update(a);

                                                if (a == lstAppoFrom.LastOrDefault())
                                                {
                                                    appoEND = a;
                                                }
                                            }
                                            else
                                            {
                                                Appointment newAppoTo = new Appointment();
                                                newAppoTo.TransactionCode = a.TransactionCode;
                                                newAppoTo.FromVisitID = a.FromVisitID;
                                                newAppoTo.IsNewPatient = a.IsNewPatient;
                                                newAppoTo.MRN = a.MRN;
                                                newAppoTo.GuestID = a.GuestID;
                                                newAppoTo.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                newAppoTo.RoomID = a.RoomID;
                                                newAppoTo.ParamedicID = ParamedicIDTo;
                                                newAppoTo.StartDate = selectedDateTo;
                                                newAppoTo.EndDate = selectedDateTo;
                                                newAppoTo.StartTime = start.ToString("HH:mm");
                                                start = start.AddMinutes(a.VisitDuration);
                                                newAppoTo.EndTime = start.ToString("HH:mm");
                                                newAppoTo.VisitTypeID = a.VisitTypeID;
                                                newAppoTo.VisitDuration = a.VisitDuration;
                                                newAppoTo.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                                newAppoTo.GCSalutation = a.GCSalutation;
                                                newAppoTo.FirstName = a.FirstName;
                                                newAppoTo.MiddleName = a.MiddleName;
                                                newAppoTo.LastName = a.LastName;
                                                newAppoTo.Name = a.Name;
                                                newAppoTo.StreetName = a.StreetName;
                                                newAppoTo.PhoneNo = a.PhoneNo;
                                                newAppoTo.MobilePhoneNo = a.MobilePhoneNo;
                                                newAppoTo.EmailAddress = a.EmailAddress;
                                                newAppoTo.GCMedicalFileStatus = a.GCMedicalFileStatus;
                                                newAppoTo.Notes = a.Notes;
                                                newAppoTo.IsRecurring = a.IsRecurring;
                                                newAppoTo.IsWaitingList = a.IsWaitingList;
                                                newAppoTo.Session = a.Session;
                                                newAppoTo.GCCustomerType = a.GCCustomerType;
                                                //newAppoTo.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(newAppoTo.HealthcareServiceUnitID, Convert.ToInt32(newAppoTo.ParamedicID), newAppoTo.StartDate, newAppoTo.StartTime, newAppoTo.EndTime, newAppoTo.IsWaitingList ? 1 : 0, ctx));
                                                //newAppoTo.QueueNo = Convert.ToInt16(QueueTemp);


                                                bool isBPJS = false;
                                                if (newAppoTo.GCCustomerType == Constant.CustomerType.BPJS)
                                                {
                                                    isBPJS = true;
                                                }

                                                newAppoTo.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(newAppoTo.HealthcareServiceUnitID, Convert.ToInt32(newAppoTo.ParamedicID), newAppoTo.StartDate, Convert.ToInt32(newAppoTo.Session), false, isBPJS, 1, 0, ctx));

                                                QueueTemp = QueueTemp + 1;
                                                newAppoTo.CreatedBy = AppSession.UserLogin.UserID;
                                                newAppoTo.CreatedDate = DateTime.Now;
                                                newAppoTo.GCAppointmentMethod = a.GCAppointmentMethod;
                                                newAppoTo.IsAppointmentRescheduled = false;
                                                newAppoTo.RescheduledFromAppointmentID = a.AppointmentID;
                                                newAppoTo.AppointmentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.OP_APPOINTMENT, newAppoTo.StartDate, ctx);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Insert(newAppoTo);

                                                //a.ParamedicID = ParamedicIDTo;
                                                //a.StartDate = a.EndDate = selectedDateTo;
                                                //a.StartTime = start.ToString("HH:mm");
                                                //start = start.AddMinutes(a.VisitDuration);
                                                //a.EndTime = start.ToString("HH:mm");
                                                //a.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                a.LastUpdatedDate = DateTime.Now;
                                                a.IsAppointmentRescheduled = true;
                                                a.RescheduledBy = AppSession.UserLogin.UserID;
                                                a.RescheduledDate = DateTime.Now;
                                                a.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                                string rescheduleMessage = string.Format("Reschedule ke nomor appointment {0} (Dokter: {1}, Poli: {2}, Tanggal: {3} {4}", newAppoTo.AppointmentNo, Request.Form[txtNewPhysicianTo.UniqueID], cboServiceUnitRescheduleTo.Text, selectedDateTo.ToString(Constant.FormatString.DATE_PICKER_FORMAT), start.ToString("HH:mm"));
                                                a.Notes = !string.IsNullOrEmpty(a.Notes) ? string.Format("||{0}", rescheduleMessage) : rescheduleMessage;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Update(a);

                                                //if (a == lstAppoFrom.LastOrDefault())
                                                //if (newAppoTo == lstAppoFrom.LastOrDefault())
                                                //{
                                                //    appoEND = a;
                                                //}
                                                appoEND = newAppoTo;
                                            }
                                        }

                                        int h = Convert.ToInt32(appoEND.EndTime.Substring(0, 2));
                                        int m = Convert.ToInt32(appoEND.EndTime.Substring(3));
                                        DateTime endAppointment = new DateTime(appoEND.StartDate.Year, appoEND.StartDate.Month, appoEND.StartDate.Day, h, m, 0);

                                        if (endAppointment.TimeOfDay > endValid.TimeOfDay)
                                        {
                                            errMessage = String.Format("Waktu Praktek Untuk Dokter Yang Dituju Tidak Cukup Untuk Menampung Semua Appointment Yang Ada");
                                            ctx.RollBackTransaction();
                                        }
                                        else
                                        {
                                            result = true;
                                            ctx.CommitTransaction();

                                            #region Is Bridging To Gateway
                                            if (hdnIsBridgingToGateway.Value == "1")
                                            {
                                                List<vAppointment> lstFrom = new List<vAppointment>();
                                                foreach (Appointment e in lstAppoFrom)
                                                {
                                                    vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID)).FirstOrDefault();
                                                    lstFrom.Add(entity);
                                                }

                                                GatewayService oService = new GatewayService();
                                                APIMessageLog entityAPILog = new APIMessageLog();
                                                string apiResult = oService.OnChangedAppointmentInformation("APPOFROM", lstFrom);
                                                string[] apiResultInfo = apiResult.Split('|');
                                                if (apiResultInfo[0] == "0")
                                                {
                                                    entityAPILog.IsSuccess = false;
                                                    entityAPILog.MessageText = apiResultInfo[1];
                                                    entityAPILog.Response = apiResultInfo[1];
                                                    Exception ex = new Exception(apiResultInfo[1]);
                                                    Helper.InsertErrorLog(ex);
                                                }
                                                else
                                                {
                                                    entityAPILog.MessageText = apiResultInfo[0];
                                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                    else
                                    {
                                        DateTime start = startValid;
                                        foreach (Appointment a in lstAppoFrom)
                                        {
                                            if (hdnChangeAppointmentCreateNewAppointment.Value == "0")
                                            {
                                                a.ParamedicID = ParamedicIDTo;
                                                a.StartDate = a.EndDate = selectedDateTo;
                                                a.StartTime = start.ToString("HH:mm");
                                                start = start.AddMinutes(a.VisitDuration);
                                                a.EndTime = start.ToString("HH:mm");
                                                a.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                appointmentDao.Update(a);

                                                if (a == lstAppoFrom.LastOrDefault())
                                                {
                                                    appoEND = a;
                                                }
                                            }
                                            else
                                            {
                                                Appointment newAppoTo = new Appointment();
                                                newAppoTo.TransactionCode = a.TransactionCode;
                                                newAppoTo.FromVisitID = a.FromVisitID;
                                                newAppoTo.IsNewPatient = a.IsNewPatient;
                                                newAppoTo.MRN = a.MRN;
                                                newAppoTo.GuestID = a.GuestID;
                                                newAppoTo.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                newAppoTo.RoomID = a.RoomID;
                                                newAppoTo.ParamedicID = ParamedicIDTo;
                                                newAppoTo.StartDate = selectedDateTo;
                                                newAppoTo.EndDate = selectedDateTo;
                                                newAppoTo.StartTime = start.ToString("HH:mm");
                                                start = start.AddMinutes(a.VisitDuration);
                                                newAppoTo.EndTime = start.ToString("HH:mm");
                                                newAppoTo.VisitTypeID = a.VisitTypeID;
                                                newAppoTo.VisitDuration = a.VisitDuration;
                                                newAppoTo.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                                newAppoTo.GCSalutation = a.GCSalutation;
                                                newAppoTo.FirstName = a.FirstName;
                                                newAppoTo.MiddleName = a.MiddleName;
                                                newAppoTo.LastName = a.LastName;
                                                newAppoTo.Name = a.Name;
                                                newAppoTo.StreetName = a.StreetName;
                                                newAppoTo.PhoneNo = a.PhoneNo;
                                                newAppoTo.MobilePhoneNo = a.MobilePhoneNo;
                                                newAppoTo.EmailAddress = a.EmailAddress;
                                                newAppoTo.GCMedicalFileStatus = a.GCMedicalFileStatus;
                                                newAppoTo.Notes = a.Notes;
                                                newAppoTo.IsRecurring = a.IsRecurring;
                                                newAppoTo.IsWaitingList = a.IsWaitingList;
                                                newAppoTo.Session = a.Session;
                                                newAppoTo.GCCustomerType = a.GCCustomerType;
                                                //newAppoTo.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(newAppoTo.HealthcareServiceUnitID, Convert.ToInt32(newAppoTo.ParamedicID), newAppoTo.StartDate, newAppoTo.StartTime, newAppoTo.EndTime, newAppoTo.IsWaitingList ? 1 : 0, ctx));

                                                bool isBPJS = false;
                                                if (newAppoTo.GCCustomerType == Constant.CustomerType.BPJS)
                                                {
                                                    isBPJS = true;
                                                }

                                                newAppoTo.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(newAppoTo.HealthcareServiceUnitID, Convert.ToInt32(newAppoTo.ParamedicID), newAppoTo.StartDate, Convert.ToInt32(newAppoTo.Session), false, isBPJS, 1, 0, ctx));

                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();

                                                newAppoTo.CreatedBy = AppSession.UserLogin.UserID;
                                                newAppoTo.CreatedDate = DateTime.Now;
                                                newAppoTo.GCAppointmentMethod = a.GCAppointmentMethod;
                                                newAppoTo.IsAppointmentRescheduled = false;
                                                newAppoTo.RescheduledFromAppointmentID = a.AppointmentID;
                                                newAppoTo.AppointmentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.OP_APPOINTMENT, newAppoTo.StartDate, ctx);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Insert(newAppoTo);

                                                //a.ParamedicID = ParamedicIDTo;
                                                //a.StartDate = a.EndDate = selectedDateTo;
                                                //a.StartTime = start.ToString("HH:mm");
                                                //start = start.AddMinutes(a.VisitDuration);
                                                //a.EndTime = start.ToString("HH:mm");
                                                //a.HealthcareServiceUnitID = HealthcareParamedicTo;
                                                a.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                a.LastUpdatedDate = DateTime.Now;
                                                a.IsAppointmentRescheduled = true;
                                                a.RescheduledBy = AppSession.UserLogin.UserID;
                                                a.RescheduledDate = DateTime.Now;
                                                a.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                                                string rescheduleMessage = string.Format("Reschedule ke nomor appointment {0} (Dokter: {1}, Poli: {2}, Tanggal: {3} {4}", newAppoTo.AppointmentNo, Request.Form[txtNewPhysicianTo.UniqueID], cboServiceUnitRescheduleTo.Text, selectedDateTo.ToString(Constant.FormatString.DATE_PICKER_FORMAT), start.ToString("HH:mm"));
                                                a.Notes = !string.IsNullOrEmpty(a.Notes) ? string.Format("||{0}", rescheduleMessage) : rescheduleMessage;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                appointmentDao.Update(a);

                                                //if (a == lstAppoFrom.LastOrDefault())
                                                //if (newAppoTo == lstAppoFrom.LastOrDefault())
                                                //{
                                                //    appoEND = a;
                                                //}
                                                appoEND = newAppoTo;
                                            }
                                        }

                                        int h = Convert.ToInt32(appoEND.EndTime.Substring(0, 2));
                                        int m = Convert.ToInt32(appoEND.EndTime.Substring(3));
                                        DateTime endAppointment = new DateTime(appoEND.StartDate.Year, appoEND.StartDate.Month, appoEND.StartDate.Day, h, m, 0);

                                        if (endAppointment.TimeOfDay > endValid.TimeOfDay)
                                        {
                                            errMessage = String.Format("Waktu Praktek Untuk Dokter '<b>'{0}'</b>' Tidak Cukup Untuk Menampung Semua Appointment Yang Ada", pmTo.FullName);
                                            ctx.RollBackTransaction();
                                        }
                                        else
                                        {
                                            result = true;
                                            ctx.CommitTransaction();

                                            #region Is Bridging To Gateway
                                            if (hdnIsBridgingToGateway.Value == "1")
                                            {
                                                List<vAppointment> lstFrom = new List<vAppointment>();
                                                foreach (Appointment e in lstAppoFrom)
                                                {
                                                    vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID)).FirstOrDefault();
                                                    lstFrom.Add(entity);
                                                }

                                                GatewayService oService = new GatewayService();
                                                APIMessageLog entityAPILog = new APIMessageLog();
                                                string apiResult = oService.OnChangedAppointmentInformation("APPOFROM", lstFrom);
                                                string[] apiResultInfo = apiResult.Split('|');
                                                if (apiResultInfo[0] == "0")
                                                {
                                                    entityAPILog.IsSuccess = false;
                                                    entityAPILog.MessageText = apiResultInfo[1];
                                                    entityAPILog.Response = apiResultInfo[1];
                                                    Exception ex = new Exception(apiResultInfo[1]);
                                                    Helper.InsertErrorLog(ex);
                                                }
                                                else
                                                {
                                                    entityAPILog.MessageText = apiResultInfo[0];
                                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                                }
                                            }
                                            #endregion
                                        }
                                    }
                                }
                                else
                                {
                                    errMessage = String.Format("Maaf, Jenis Kunjungan Untuk Dokter '<b>'{0}'</b>' Tidak Mencakup Semua Jenis Kunjungan Dari Dokter '<b>'{1}'</b>'", pmTo.FullName, pmFrom.FullName);
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                string timeslotMessage = "";
                                string slotMessage = "";

                                if (isTimeSlot)
                                {
                                    timeslotMessage = "Time Slot";
                                }
                                else
                                {
                                    timeslotMessage = "NO Time Slot";
                                }

                                if (timeSlot == "0")
                                {
                                    slotMessage = "Sesi 1";
                                }
                                else if (timeSlot == "1")
                                {
                                    slotMessage = "Sesi 2";
                                }
                                else if (timeSlot == "2")
                                {
                                    slotMessage = "Sesi 3";
                                }
                                else if (timeSlot == "3")
                                {
                                    slotMessage = "Sesi 4";
                                }
                                else
                                {
                                    slotMessage = "Sesi 5";
                                }

                                if (String.IsNullOrEmpty(errMessage))
                                {
                                    errMessage = String.Format("Maaf Jadwal Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ada", pmTo.FullName, selectedDateTo.ToString(Constant.FormatString.DATE_FORMAT), timeslotMessage, slotMessage);
                                }

                                ctx.RollBackTransaction();
                            }
                        }
                        else if (!isValidTo && !isValidFrom)
                        {
                            string timeslotMessage = "";
                            string slotMessage = "";

                            if (isTimeSlot)
                            {
                                timeslotMessage = "Time Slot";
                            }
                            else
                            {
                                timeslotMessage = "NO Time Slot";
                            }

                            if (timeSlot == "0")
                            {
                                slotMessage = "Sesi 1";
                            }
                            else if (timeSlot == "1")
                            {
                                slotMessage = "Sesi 2";
                            }
                            else if (timeSlot == "2")
                            {
                                slotMessage = "Sesi 3";
                            }
                            else if (timeSlot == "3")
                            {
                                slotMessage = "Sesi 4";
                            }
                            else
                            {
                                slotMessage = "Sesi 5";
                            }

                            if (String.IsNullOrEmpty(errMessage))
                            {
                                errMessage = String.Format("Maaf Jadwal Untuk Dokter '<b>'{0}'</b>' Di Tanggal '<b>'{1}'</b>' Untuk '<b>'{2}'</b>' ('<b>'{3}'</b>') Tidak Ada", pmFrom.FullName, selectedDateFrom.ToString(Constant.FormatString.DATE_FORMAT), timeslotMessage, slotMessage);
                            }

                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = String.Format("Maaf Dokter Asal Dan Tujuan Tidak Boleh Sama");
                        ctx.RollBackTransaction();
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                return false;
            }
        }
    }
}