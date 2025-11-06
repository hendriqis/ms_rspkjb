using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Outpatient.Program
{
    public partial class AppointmentEntry : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Outpatient.APPOINTMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        #region Error Message
        protected string GetErrMessageCompletedAppointment()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_COMPLETED_APPOINTMENT_VALIDATION);
        }
        protected string GetErrMessageSelectAppointmentFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_APPOINTMENT_FIRST_VALIDATION);
        }
        #endregion

        protected int PageCount = 1;
        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            InitializeControlProperties();
            BindGridPhysician(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtVisitTypeCode, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtMRN, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(cboSalutation, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtFirstName, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtMiddleName, new ControlEntrySetting(true, false, false), "mpAppointment");
            Helper.SetControlEntrySetting(txtFamilyName, new ControlEntrySetting(true, false, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtAddress, new ControlEntrySetting(true, true, true), "mpAppointment");
            Helper.SetControlEntrySetting(txtPhoneNo, new ControlEntrySetting(true, true, true), "mpAppointment");
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void InitializeControlProperties()
        {
            int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
            if (serviceUnitUserCount > 0)
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", Constant.Facility.OUTPATIENT, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnit.SelectedIndex = 0;

            txtServiceUnit.Text = cboServiceUnit.Text;
            txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnCalAppointmentSelectedDate.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.TITLE));
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboSalutation, lstStandardCode, "StandardCodeName", "StandardCodeID");

            hdnDefaultServiceUnitInterval.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL).ParameterValue;

            txtMaximumAppointment.Attributes.Add("readonly", "readonly");
            txtMaximumWaitingList.Attributes.Add("readonly", "readonly");
        }

        private void BindGridPhysician(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = string.Format(
                    "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1} UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = {0} AND ScheduleDate = '{2}') AND IsDeleted = 0",
                    cboServiceUnit.Value, daynumber, selectedDate.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);

            #region validate paramedic List (Exclude Paramedic Leave)
            foreach (vParamedicMaster e in lstEntity.ToList())
            {
                List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                        cboServiceUnit.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


                vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                    cboServiceUnit.Value, e.ParamedicID, daynumber)).FirstOrDefault();

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

        protected void cbpPhysician_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysician(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridPhysician(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void SettingScheduleTable(List<ParamedicScheduleCustom> ListDisplayAppTime, String start, String end, Int32 interval, ref int id, ref int selectedRowIndex, bool isAppointmentByTimeSlot, ref bool addStartTime)
        {
            DateTime startTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", start));
            DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", end));
            List<ParamedicScheduleCustom> ListDisplayAppTimeTemp = new List<ParamedicScheduleCustom>();

            bool result = true;
            //foreach (ParamedicScheduleCustom e in ListDisplayAppTime)
            //{
            //    //if (e.Time == start || e.Time == end)
            //    //{
            //    //    result = false;
            //    //}

            //    if (e.Time == start)
            //    {
            //        ListDisplayAppTimeTemp.Add(e);
            //    }
            //    else if (e.Time == end)
            //    {
            //        ListDisplayAppTimeTemp.Add(e);
            //    }

            //}

            //foreach (ParamedicScheduleCustom e in ListDisplayAppTimeTemp)
            //{
            //    ListDisplayAppTime.Remove(e);
            //}

            if (result)
            {
                if (isAppointmentByTimeSlot)
                {
                    int minuteStartTime = startTime.Minute;
                    int parentID = 0;
                    while (startTime < endTime)
                    {
                        ParamedicScheduleCustom entity = new ParamedicScheduleCustom();
                        entity.ID = ++id;

                        if (selectedRowIndex < 1)
                        {
                            vAppointment appointment = lstAppointment.FirstOrDefault(p => p.StartTime == startTime.ToString("HH:mm"));
                            if (appointment == null)
                                selectedRowIndex = id;
                        }

                        entity.Time = startTime.ToString("HH:mm");
                        if (startTime.Minute == minuteStartTime)
                        {
                            parentID = entity.ID;
                            entity.ParentID = -1;
                        }
                        else
                            entity.ParentID = parentID;
                        startTime = startTime.AddMinutes(interval);

                        ListDisplayAppTime.Add(entity);
                    }
                    ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                    temp.ID = ++id;
                    ListDisplayAppTime.Add(temp);
                }
                else
                {
                    int ct = 0;
                    int idtemp = 0;
                    DateTime lastTime = startTime;
                    selectedRowIndex = id;
                    List<vAppointment> lstAppointmentTemp = lstAppointment.Where(t => !t.IsWaitingList).GroupBy(x => x.StartTime).Select(a => a.First()).OrderBy(z => z.StartTime).ToList();
                    bool flagIsIdDefine = false;
                    if (addStartTime)
                    {
                        if (!(lstAppointmentTemp.Where(t => t.StartTime == lastTime.ToString("HH:mm")).Count() > 0))
                        {
                            ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                            temp.ID = ++id;
                            temp.Time = lastTime.ToString("HH:mm");
                            idtemp = id;
                            temp.ParentID = idtemp;
                            flagIsIdDefine = true;
                            ListDisplayAppTime.Add(temp);
                        }
                    }
                    addStartTime = false;
                    foreach (vAppointment entityAppointment in lstAppointmentTemp)
                    {
                        if (DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)) >= startTime &&
                            DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)) < endTime)
                        {
                            ParamedicScheduleCustom entity = new ParamedicScheduleCustom();
                            entity.ID = ++id;
                            entity.Time = entityAppointment.StartTime;
                            if (ct == 0 && !flagIsIdDefine) idtemp = id;
                            entity.ParentID = idtemp;
                            ct++;
                            lastTime = DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)).AddMinutes(Convert.ToInt32(entityAppointment.VisitDuration));
                            ListDisplayAppTime.Add(entity);
                        }
                    }

                    if (ct != 0)
                    {
                        ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                        temp.ID = ++id;
                        temp.Time = lastTime.ToString("HH:mm");
                        temp.ParentID = idtemp;
                        if (lastTime < endTime && lstAppointment.Count < Convert.ToInt32(txtMaximumAppointment.Text)) ListDisplayAppTime.Add(temp);
                        else addStartTime = true;
                    }
                }
            }

            List<ParamedicScheduleCustom> ListDisplayAppTimeFinalCheck = new List<ParamedicScheduleCustom>();
            
            foreach (ParamedicScheduleCustom e in ListDisplayAppTime) {
                if (ListDisplayAppTime.Where(t => t.Time == e.Time).Count() > 1)
                {
                    if (ListDisplayAppTimeFinalCheck.Where(t => t.Time == e.Time).Count() == 0)
                    {
                        ListDisplayAppTimeFinalCheck.Add(e);
                    }
                }
            }

            foreach (ParamedicScheduleCustom e in ListDisplayAppTimeFinalCheck)
            {
                ListDisplayAppTime.Remove(e);
            }
        }

        List<vAppointment> lstAppointment = null;
        protected void cbpAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            Int32 HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);

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
                            obj = null;
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
                            obj = null;
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
                            obj = null;
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
                            obj = null;
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
                            obj = null;
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
                        else if(startTimeDefault.TimeOfDay >= objEnd2.TimeOfDay && startTimeDefault.TimeOfDay <= objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd3.TimeOfDay) {
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
                            obj = null;
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
                            obj = null;
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
                        else if (startTimeDefault.TimeOfDay == objEnd2.TimeOfDay && endTimeDefault.TimeOfDay > objEnd3.TimeOfDay && endTimeDefault.TimeOfDay < objStart4.TimeOfDay) {
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
                            obj = null;
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
                        else if(startTimeDefault.TimeOfDay > objEnd2.TimeOfDay && startTimeDefault.TimeOfDay < objStart3.TimeOfDay && endTimeDefault.TimeOfDay >= objEnd4.TimeOfDay && endTimeDefault.TimeOfDay < objStart5.TimeOfDay)
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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
                            objSchDate = null;
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

            List<ParamedicScheduleCustom> ListDisplayAppTime = new List<ParamedicScheduleCustom>();
            List<ParamedicScheduleCustom> ListDisplayWaitingList = new List<ParamedicScheduleCustom>();

            int selectedRowIndex = 0;
            if (obj != null)
            {
                if (!obj.IsAppointmentByTimeSlot)
                {
                    txtMaximumAppointment.Text = obj.MaximumAppointment.ToString();
                    trMaxAppointment.Style.Remove("display");
                }
                else
                {
                    trMaxAppointment.Style.Add("display", "none");
                }

                if (!obj.IsAllowWaitingList)
                {
                    txtMaximumWaitingList.Text = "0";
                    trMaxWaitingList.Style.Add("display", "none");
                }
                else
                {
                    txtMaximumWaitingList.Text = obj.MaximumWaitingList.ToString();
                    trMaxWaitingList.Style.Remove("display");
                }

                lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED));
                txtAlreadyRegister.Text = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count().ToString();
                txtPending.Text = (lstAppointment.Count - Convert.ToInt32(txtAlreadyRegister.Text)).ToString();
                txtTotalAppointment.Text = lstAppointment.Count.ToString();
                int serviceInterval = BusinessLayer.GetHealthcareServiceUnit(HealthcareServiceUnitID).ServiceInterval;
                if (serviceInterval < 1)
                    serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                int id = 0;
                bool isAddStartTime = true;

                if (obj.StartTime1 != "") SettingScheduleTable(ListDisplayAppTime, obj.StartTime1, obj.EndTime1, serviceInterval, ref id, ref selectedRowIndex, obj.IsAppointmentByTimeSlot, ref isAddStartTime);
                if (obj.StartTime2 != "") SettingScheduleTable(ListDisplayAppTime, obj.StartTime2, obj.EndTime2, serviceInterval, ref id, ref selectedRowIndex, obj.IsAppointmentByTimeSlot, ref isAddStartTime);
                if (obj.StartTime3 != "") SettingScheduleTable(ListDisplayAppTime, obj.StartTime3, obj.EndTime3, serviceInterval, ref id, ref selectedRowIndex, obj.IsAppointmentByTimeSlot, ref isAddStartTime);
                if (obj.StartTime4 != "") SettingScheduleTable(ListDisplayAppTime, obj.StartTime4, obj.EndTime4, serviceInterval, ref id, ref selectedRowIndex, obj.IsAppointmentByTimeSlot, ref isAddStartTime);
                if (obj.StartTime5 != "") SettingScheduleTable(ListDisplayAppTime, obj.StartTime5, obj.EndTime5, serviceInterval, ref id, ref selectedRowIndex, obj.IsAppointmentByTimeSlot, ref isAddStartTime);

                if (objSchDate != null)
                {

                    DateTime objLeaveStart = DateTime.Parse("2012-01-28" + ' ' + objLeave.FirstOrDefault().StartTime);

                    if (objSchDate.StartTime1 != "")
                    {
                        if (objSchDate.StartTime1 == obj.StartTime1)
                        {
                            if (objSchDate.EndTime1 != obj.EndTime1)
                            {
                                string tempStartTime = obj.EndTime1;
                                SettingScheduleTable(ListDisplayAppTime, tempStartTime, objSchDate.EndTime1, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                            }
                        }
                        else
                        {
                            DateTime objStart1 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime1);
                            DateTime objEnd1 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime1);
                            DateTime objschStart1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime1);
                            DateTime objschEnd1 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime1);

                            if (objschStart1.TimeOfDay > objStart1.TimeOfDay && objschStart1.TimeOfDay <= objEnd1.TimeOfDay && objschEnd1.TimeOfDay > objEnd1.TimeOfDay)
                            {
                                objSchDate.StartTime1 = obj.EndTime1;
                            }
                            SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime1, objSchDate.EndTime1, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                        }
                    }

                    if (objSchDate.StartTime2 != "")
                    {
                        if (objSchDate.StartTime2 == obj.StartTime2)
                        {
                            if (objSchDate.EndTime2 != obj.EndTime2)
                            {
                                string tempStartTime = obj.EndTime2;
                                SettingScheduleTable(ListDisplayAppTime, tempStartTime, objSchDate.EndTime2, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                            }
                        }
                        else
                        {
                            DateTime objStart2 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime2);
                            DateTime objEnd2 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime2);
                            DateTime objschStart2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime2);
                            DateTime objschEnd2 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime2);

                            if (objschStart2.TimeOfDay > objStart2.TimeOfDay && objschStart2.TimeOfDay <= objEnd2.TimeOfDay && objschEnd2.TimeOfDay > objEnd2.TimeOfDay)
                            {
                                objSchDate.StartTime2 = obj.EndTime2;
                            }
                            SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime2, objSchDate.EndTime2, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                        }
                    }

                    if (objSchDate.StartTime3 != "")
                    {
                        if (objSchDate.StartTime3 == obj.StartTime3)
                        {
                            if (objSchDate.EndTime3 != obj.EndTime3)
                            {
                                string tempStartTime = obj.EndTime3;
                                SettingScheduleTable(ListDisplayAppTime, tempStartTime, objSchDate.EndTime3, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                            }
                        }
                        else
                        {
                            DateTime objStart3 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime3);
                            DateTime objEnd3 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime3);
                            DateTime objschStart3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime3);
                            DateTime objschEnd3 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime3);

                            if (objschStart3.TimeOfDay > objStart3.TimeOfDay && objschStart3.TimeOfDay <= objEnd3.TimeOfDay && objschEnd3.TimeOfDay > objEnd3.TimeOfDay)
                            {
                                objSchDate.StartTime3 = obj.EndTime3;
                            }
                            SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime3, objSchDate.EndTime3, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                        }
                    }

                    if (objSchDate.StartTime4 != "")
                    {
                        if (objSchDate.StartTime4 == obj.StartTime4)
                        {
                            if (objSchDate.EndTime4 != obj.EndTime4)
                            {
                                string tempStartTime = obj.EndTime4;
                                SettingScheduleTable(ListDisplayAppTime, tempStartTime, objSchDate.EndTime4, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                            }
                        }
                        else
                        {
                            DateTime objStart4 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime4);
                            DateTime objEnd4 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime4);
                            DateTime objschStart4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime4);
                            DateTime objschEnd4 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime4);

                            if (objschStart4.TimeOfDay > objStart4.TimeOfDay && objschStart4.TimeOfDay <= objEnd4.TimeOfDay && objschEnd4.TimeOfDay > objEnd4.TimeOfDay)
                            {
                                objSchDate.StartTime4 = obj.EndTime4;
                            }
                            SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime4, objSchDate.EndTime4, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                        }
                    }

                    if (objSchDate.StartTime5 != "")
                    {
                        if (objSchDate.StartTime5 == obj.StartTime5)
                        {
                            if (objSchDate.EndTime5 != obj.EndTime5)
                            {
                                string tempStartTime = obj.EndTime5;
                                SettingScheduleTable(ListDisplayAppTime, tempStartTime, objSchDate.EndTime5, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                            }
                        }
                        else
                        {
                            DateTime objStart5 = DateTime.Parse("2012-01-28" + ' ' + obj.StartTime5);
                            DateTime objEnd5 = DateTime.Parse("2012-01-28" + ' ' + obj.EndTime5);
                            DateTime objschStart5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.StartTime5);
                            DateTime objschEnd5 = DateTime.Parse("2012-01-28" + ' ' + objSchDate.EndTime5);

                            if (objschStart5.TimeOfDay > objStart5.TimeOfDay && objschStart5.TimeOfDay <= objEnd5.TimeOfDay && objschEnd5.TimeOfDay > objEnd5.TimeOfDay)
                            {
                                objSchDate.StartTime5 = obj.EndTime5;
                            }
                            SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime5, objSchDate.EndTime5, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                        }
                    }
                }
                SettingSheduleWaitingList(ListDisplayWaitingList);
            }
            // untuk dokter yang hanya punya paramedicscheduleDate Saja
            else
            {
                if (objSchDate != null)
                {
                    if (!objSchDate.IsAppointmentByTimeSlot)
                    {
                        txtMaximumAppointment.Text = objSchDate.MaximumAppointment.ToString();
                        trMaxAppointment.Style.Remove("display");
                    }
                    else
                    {
                        trMaxAppointment.Style.Add("display", "none");
                    }

                    if (!objSchDate.IsAllowWaitingList)
                    {
                        txtMaximumWaitingList.Text = "0";
                        trMaxWaitingList.Style.Add("display", "none");
                    }
                    else
                    {
                        txtMaximumWaitingList.Text = objSchDate.MaximumWaitingList.ToString();
                        trMaxWaitingList.Style.Remove("display");
                    }

                    lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED));
                    txtAlreadyRegister.Text = lstAppointment.Where(t => t.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count().ToString();
                    txtPending.Text = (lstAppointment.Count - Convert.ToInt32(txtAlreadyRegister.Text)).ToString();
                    txtTotalAppointment.Text = lstAppointment.Count.ToString();
                    int serviceInterval = BusinessLayer.GetHealthcareServiceUnit(HealthcareServiceUnitID).ServiceInterval;
                    if (serviceInterval < 1)
                        serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                    int id = 0;
                    bool isAddStartTime = true;

                    if (objSchDate.StartTime1 != "") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime1, objSchDate.EndTime1, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                    if (objSchDate.StartTime2 != "") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime2, objSchDate.EndTime2, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                    if (objSchDate.StartTime3 != "") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime3, objSchDate.EndTime3, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                    if (objSchDate.StartTime4 != "") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime4, objSchDate.EndTime4, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);
                    if (objSchDate.StartTime5 != "") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime5, objSchDate.EndTime5, serviceInterval, ref id, ref selectedRowIndex, objSchDate.IsAppointmentByTimeSlot, ref isAddStartTime);

                    SettingSheduleWaitingList(ListDisplayWaitingList);
                }
            }

            grdWaitingList.DataSource = ListDisplayWaitingList;
            grdWaitingList.DataBind();


            grdAppointment.DataSource = ListDisplayAppTime.OrderBy(o => o.Time).ToList();
            grdAppointment.DataBind();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = selectedRowIndex;
        }

        private void SettingSheduleWaitingList(List<ParamedicScheduleCustom> lstParamedicScheduleCustom)
        {
            List<vAppointment> lstTempAppointment = lstAppointment.Where(t => t.IsWaitingList).OrderBy(a => a.CreatedDate).ToList();
            int ct = 1;
            foreach (vAppointment entity in lstTempAppointment)
            {
                ParamedicScheduleCustom newEntity = new ParamedicScheduleCustom();
                newEntity.Queue = ct;
                entity.cfQueue = ct;
                lstParamedicScheduleCustom.Add(newEntity);
                ct++;
            }
            int maximumWaitingList = Convert.ToInt32(txtMaximumWaitingList.Text);
            if (ct <= maximumWaitingList)
            {
                lstParamedicScheduleCustom.Add(new ParamedicScheduleCustom()
                {
                    Queue = ct
                });
            }
        }

        protected void grdAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obj = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstBindAppointment = (from p in lstAppointment.Where(p => p.StartTime == obj.Time && !p.IsWaitingList).ToList()
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName }).ToList();
                AppointmentCustomClass app = new AppointmentCustomClass();
                if (obj.Time == null)
                    app.AppointmentID = -2;
                else
                    app.AppointmentID = -1;
                app.AppointmentNo = "";
                app.GCAppointmentStatus = "";
                app.VisitTypeName = "";
                app.PatientName = "";
                lstBindAppointment.Add(app);

                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstBindAppointment;
                rpt.DataBind();
            }
        }


        protected void grdWaitingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obje = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstBindWaitingList = (from p in lstAppointment.Where(p => p.cfQueue == obje.Queue)
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName }).ToList();
                if (lstBindWaitingList.Count == 0)
                {
                    AppointmentCustomClass app = new AppointmentCustomClass();
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstBindWaitingList.Add(app);
                }

                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstBindWaitingList;
                rpt.DataBind();
            }
        }
        private void ControlToEntity(Appointment entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            if (hdnRoomID.Value != "" && hdnRoomID.Value != null)
            {
                entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.StartDate = entity.EndDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value);
            entity.StartTime = Request.Form[txtAppointmentHour.UniqueID];
            if (!chkIsNewPatient.Checked)
            {
                entity.MRN = Convert.ToInt32(hdnMRN.Value);
                entity.IsNewPatient = false;
            }
            else
            {
                entity.IsNewPatient = true;
                if (cboSalutation.Value != null)
                    entity.GCSalutation = (string)cboSalutation.Value;
                else
                    entity.GCSalutation = null;
                entity.FirstName = txtFirstName.Text;
                entity.MiddleName = txtMiddleName.Text;
                entity.LastName = txtFamilyName.Text;
                entity.StreetName = txtAddress.Text;
                entity.PhoneNo = txtPhoneNo.Text;
                entity.MobilePhoneNo = txtMobilePhone.Text;
                entity.EmailAddress = txtEmail.Text;
                entity.Name = Helper.GenerateName(entity.LastName, entity.MiddleName, entity.FirstName);
            }
            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
            entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
            entity.Notes = txtRemarks.Text;

            int minute = Convert.ToInt32(entity.StartTime.Substring(3));
            int hour = Convert.ToInt32(entity.StartTime.Substring(0, 2));
            DateTime dtStartTime = new DateTime(2000, 1, 1, hour, minute, 0);
            entity.EndTime = dtStartTime.AddMinutes(entity.VisitDuration).ToString("HH:mm");
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

            entityPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
            entityAddress.LastUpdatedBy = AppSession.UserLogin.UserID;

            entityPatientDao.Update(entityPatient);
            entityAddressDao.Update(entityAddress);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;

                IDbContext ctx = DbFactory.Configure(true);
                AppointmentDao entityDao = new AppointmentDao(ctx);
                ServiceUnitParamedicDao entityServiceUnitParamedicDao = new ServiceUnitParamedicDao(ctx);

                try
                {
                    string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}' AND StartTime = '{4}' AND VisitTypeID = {5}", cboServiceUnit.Value, hdnParamedicID.Value, Helper.GetDatePickerValue(hdnCalAppointmentSelectedDate.Value).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED, Request.Form[txtAppointmentHour.UniqueID], hdnVisitTypeID.Value);
                    if (hdnAppointmentID.Value != "")
                        filterExpression += string.Format(" AND AppointmentID != {0}", hdnAppointmentID.Value);
                    int count = BusinessLayer.GetAppointmentRowCount(filterExpression, ctx);
                    if (count > 0 && !(hdnIsWaitingList.Value == "1"))
                    {
                        errMessage = string.Format("Visit Type {0} Has Already Been Used In This Slot. Cannot Be Used", Request.Form[txtVisitTypeName.UniqueID]);
                        result = false;
                    }
                    else
                    {
                        Appointment entity = null;

                        if (hdnAppointmentID.Value != "")
                        {
                            entity = entityDao.Get(Convert.ToInt32(hdnAppointmentID.Value));
                            entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeID.Value);
                            entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDuration.UniqueID]);
                            if (hdnRoomID.Value != "" && hdnRoomID.Value != null)
                            {
                                entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
                            }
                            entity.Notes = txtRemarks.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.IsWaitingList = (hdnIsWaitingList.Value == "1");
                            if (!chkIsNewPatient.Checked)
                                UpdatePatientData(ctx, (int)entity.MRN);

                            entityDao.Update(entity);
                        }
                        else
                        {
                            entity = new Appointment();
                            ControlToEntity(entity);
                            entity.IsWaitingList = (hdnIsWaitingList.Value == "1");
                            if (!chkIsNewPatient.Checked)
                                UpdatePatientData(ctx, (int)entity.MRN);
                            entity.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                            entity.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                            entity.AppointmentNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.StartDate);
                            //short currentNo = BusinessLayer.GetAppointmentMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", entity.HealthcareServiceUnitID, entity.ParamedicID, entity.StartDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                            //ServiceUnitParamedic entityServiceUnitParamedic = entityServiceUnitParamedicDao.Get(entity.HealthcareServiceUnitID, entity.ParamedicID);
                            //entityServiceUnitParamedic.LastQueueNo += 1;
                            //entity.QueueNo = Convert.ToInt16(entityServiceUnitParamedic.LastQueueNo); 
                            //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate) + 1);
                            entity.CreatedBy = AppSession.UserLogin.UserID;
                            //entityServiceUnitParamedicDao.Update(entityServiceUnitParamedic);
                            entityDao.Insert(entity);
                        }

                        ctx.CommitTransaction();
                    }
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
                return result;
            }
            return false;
        }

        public class ParamedicScheduleCustom
        {
            public Int32 ID { get; set; }
            public Int32 Queue { get; set; }
            public String Time { get; set; }
            public Int32 ParentID { get; set; }
        }

        public class AppointmentCustomClass
        {
            public Int32 AppointmentID { get; set; }
            public String PatientName { get; set; }
            public String PatientImageUrl { get; set; }
            public String VisitTypeName { get; set; }
            public String GCAppointmentStatus { get; set; }
            public String AppointmentNo { get; set; }
            public Boolean IsAppointmentCompleted
            {
                get { return GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE; }
            }
        }
    }
}