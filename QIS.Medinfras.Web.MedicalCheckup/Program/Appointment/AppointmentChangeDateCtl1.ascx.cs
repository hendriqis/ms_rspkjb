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
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class AppointmentChangeDateCtl1 : BaseViewPopupCtl
    {
        protected string GetErrorMsgAppointmentSlot()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_APPOINTMENT_SLOT_VALIDATION);
        }

        protected int PageCount = 1;
        public override void InitializeDataControl(string queryString)
        {
            //            IsAdd = true;
            String[] param = queryString.Split('|');
            hdnID.Value = param[0];
            hdnIsVoidAndNoTimeSlot.Value = param[1];
            hdnDepartmentIDCtl.Value = param[2];
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", hdnID.Value))[0];
            SetControlProperties();
            EntityToControl(entity);
            BindGridPhysician(1, true, ref PageCount);

            Helper.SetControlEntrySetting(hdnVisitTypeIDTo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(hdnVisitDurationTo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtVisitTypeCodeTo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtVisitTypeNameTo, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboAppointmentMethodTo, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        protected void SetControlProperties()
        {
            int serviceUnitUserCount = BusinessLayer.GetvServiceUnitUserRowCount(string.Format("DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2}", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID));
            string filterExpression = "";
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY));
            string setvarImaging = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            string setvarLaboratory = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;

            filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.MEDICAL_CHECKUP);
            if (serviceUnitUserCount > 0)
                filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vServiceUnitUser WHERE DepartmentID = '{0}' AND HealthcareID = '{1}' AND UserID = {2} AND IsDeleted = 0)", Constant.Facility.MEDICAL_CHECKUP, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);

            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnitChangeAppointment, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitChangeAppointment.SelectedIndex = 0;

            List<StandardCode> lstSession = new List<StandardCode>();
            lstSession.Insert(0, new StandardCode { StandardCodeName = "Sesi 1", StandardCodeID = "0" });
            lstSession.Insert(1, new StandardCode { StandardCodeName = "Sesi 2", StandardCodeID = "1" });
            lstSession.Insert(2, new StandardCode { StandardCodeName = "Sesi 3", StandardCodeID = "2" });
            lstSession.Insert(3, new StandardCode { StandardCodeName = "Sesi 4", StandardCodeID = "3" });
            lstSession.Insert(4, new StandardCode { StandardCodeName = "Sesi 5", StandardCodeID = "4" });
            Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
            cboSessionCtl.SelectedIndex = 0;

            //string exp = String.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DELETE_REASON);
            //List<StandardCode> lstReason = BusinessLayer.GetStandardCodeList(exp);
            //Methods.SetComboBoxField<StandardCode>(cboChangeReason, lstReason, "StandardCodeName", "StandardCodeID");
            //cboChangeReason.SelectedIndex = 0;

            hdnDefaultServiceUnitInterval.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_SERVICE_UNIT_INTERVAL).FirstOrDefault().ParameterValue;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.APPOINTMENT_METHOD));
            Methods.SetComboBoxField<StandardCode>(cboAppointmentMethodTo, lstStandardCode, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAppointmentMethodFrom, lstStandardCode, "StandardCodeName", "StandardCodeID");
            cboAppointmentMethodTo.SelectedIndex = 0;
        }

        //protected override void OnControlEntrySetting()
        //{
        //    // SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));

        //    //SetControlEntrySetting(hdnVisitTypeIDTo, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(hdnVisitDurationTo, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(txtVisitTypeCodeTo, new ControlEntrySetting(true, true, true));
        //    //SetControlEntrySetting(txtVisitTypeNameTo, new ControlEntrySetting(false, false, true));
        //}

        private void EntityToControl(vAppointment entity)
        {
            txtAppointmentNo.Text = entity.AppointmentNo;
            txtAppointmentStatus.Text = entity.AppointmentStatus;
            txtPatientName.Text = entity.cfPatientName;
            txtServiceUnit.Text = entity.ServiceUnitName;
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtPhysician.Text = entity.ParamedicName;
            txtAppointmentDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAppointmentHour.Text = entity.StartTime;
            txtVisitType.Text = entity.VisitTypeName;
            txtVisitDuration.Text = Convert.ToString(entity.VisitDuration);

            hdnVisitDurationTo.Value = entity.VisitDuration.ToString();
            hdnVisitTypeIDTo.Value = entity.VisitTypeID.ToString();
            txtVisitTypeCodeTo.Text = entity.VisitTypeCode;
            txtVisitTypeNameTo.Text = entity.VisitTypeName;
            txtVisitDurationTo.Text = Convert.ToString(entity.VisitDuration);

            cboServiceUnitChangeAppointment.Value = entity.HealthcareServiceUnitID.ToString();

            hdnOldParamedicID.Value = entity.ParamedicID.ToString();
            txtNewServiceUnit.Text = cboServiceUnitChangeAppointment.Text;
            hdnCalAppointmentSelectedDateCtl.Value = txtNewAppointmentDate.Text = entity.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (!String.IsNullOrEmpty(entity.GCAppointmentMethod))
            {
                cboAppointmentMethodFrom.Value = entity.GCAppointmentMethod;
                cboAppointmentMethodTo.Value = entity.GCAppointmentMethod;
            }
            else
            {
                cboAppointmentMethodFrom.Value = "";
                cboAppointmentMethodTo.Value = "";
            }
        }

        private void ReplcaceAppointment(Appointment NewAppo, Appointment oldAppo)
        {
            NewAppo.TransactionCode = oldAppo.TransactionCode;
            NewAppo.AppointmentNo = BusinessLayer.GenerateTransactionNo(oldAppo.TransactionCode, oldAppo.StartDate);
            NewAppo.FromVisitID = oldAppo.FromVisitID;
            NewAppo.IsNewPatient = oldAppo.IsNewPatient;
            NewAppo.MRN = oldAppo.MRN;
            NewAppo.HealthcareServiceUnitID = oldAppo.HealthcareServiceUnitID;
            NewAppo.RoomID = oldAppo.RoomID;
            NewAppo.ParamedicID = oldAppo.ParamedicID;
            NewAppo.StartDate = oldAppo.StartDate;
            NewAppo.EndDate = oldAppo.EndDate;
            NewAppo.StartTime = oldAppo.StartTime;
            NewAppo.EndTime = oldAppo.EndTime;
            NewAppo.VisitTypeID = oldAppo.VisitTypeID;
            NewAppo.VisitDuration = oldAppo.VisitDuration;
            NewAppo.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
            NewAppo.GCSalutation = oldAppo.GCSalutation;
            NewAppo.FirstName = oldAppo.FirstName;
            NewAppo.MiddleName = oldAppo.MiddleName;
            NewAppo.LastName = oldAppo.LastName;
            NewAppo.Name = oldAppo.Name;
            NewAppo.StreetName = oldAppo.StreetName;
            NewAppo.PhoneNo = oldAppo.PhoneNo;
            NewAppo.MobilePhoneNo = oldAppo.MobilePhoneNo;
            NewAppo.EmailAddress = oldAppo.EmailAddress;
            NewAppo.GCMedicalFileStatus = oldAppo.GCMedicalFileStatus;
            NewAppo.Notes = oldAppo.Notes;
            NewAppo.IsRecurring = oldAppo.IsRecurring;
            NewAppo.IsWaitingList = oldAppo.IsWaitingList;
            NewAppo.QueueNo = oldAppo.QueueNo;
            NewAppo.CreatedBy = AppSession.UserLogin.UserID;
            NewAppo.GCAppointmentMethod = (string)cboAppointmentMethodTo.Value;
            NewAppo.Session = (Int32)cboSessionCtl.Value;
        }

        private void ValidateParamedicScSchedule(vParamedicSchedule obj, vParamedicScheduleDate objSchDate)
        {
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
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

        private void BindGridPhysician(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            string filterExpression = string.Format(
                    "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND ParamedicID IN (SELECT ParamedicID FROM vParamedicSchedule WHERE HealthcareServiceUnitID = {0} AND DayNumber = {1} UNION ALL SELECT ParamedicID FROM vParamedicScheduleDate WHERE HealthcareServiceUnitID = {0} AND ScheduleDate = '{2}') AND IsDeleted = 0",
                    cboServiceUnitChangeAppointment.Value, daynumber, selectedDate.ToString("yyyyMMdd"));

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vParamedicMaster> lstEntity = BusinessLayer.GetvParamedicMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ParamedicName");

            cboSessionCtl.SelectedIndex = 0;
            grdPhysician.DataSource = lstEntity;
            grdPhysician.DataBind();
        }

        private void validateParamedic(List<vParamedicMaster> lstEntity)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            #region validate paramedic List (Exclude Paramedic Leave)
            foreach (vParamedicMaster e in lstEntity.ToList())
            {
                List<GetParamedicLeaveScheduleCompare> et = BusinessLayer.GetParamedicLeaveScheduleCompareList(selectedDate.ToString("yyyyMMdd"), e.ParamedicID);

                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                        "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                        cboServiceUnitChangeAppointment.Value, e.ParamedicID, selectedDate)).FirstOrDefault();


                vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                    "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                    cboServiceUnitChangeAppointment.Value, e.ParamedicID, daynumber)).FirstOrDefault();

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
        }

        protected void cbpPhysicianChangeAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridPhysician(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else if (param[0] == "save")
                {
                    if (OnSaveAddRecord(ref errMessage))
                    {
                        result = "save";
                    }
                    else
                    {
                        result = string.Format("fail|{0}", errMessage);
                    }
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

        private void SettingScheduleTable(List<ParamedicScheduleCustom> ListDisplayAppTime, String start, String end, Int32 interval, ref int id, ref int selectedRowIndex, ref bool addStartTime)
        {
            DateTime startTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", start));
            DateTime endTime = DateTime.Parse(String.Format("2012-01-28 {0}:00", end));
            List<ParamedicScheduleCustom> ListDisplayAppTimeTemp = new List<ParamedicScheduleCustom>();

            bool result = true;

            if (result)
            {
                //if (isAppointmentByTimeSlot)
                //{
                int minuteStartTime = startTime.Minute;
                int parentID = 0;
                while (startTime < endTime)
                {
                    ParamedicScheduleCustom entity = new ParamedicScheduleCustom();
                    entity.ID = ++id;

                    if (selectedRowIndex < 1)
                    {
                        vAppointment appointment = lstAppointment.FirstOrDefault(p => p.StartTime == startTime.ToString("HH:mm") && p.GCAppointmentStatus != Constant.AppointmentStatus.DELETED);
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
                    entity.EndTime = end;
                    ListDisplayAppTime.Add(entity);
                }
                ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                temp.ID = ++id;
                ListDisplayAppTime.Add(temp);
                //}
                //else
                //{
                //    int ct = 0;
                //    int idtemp = 0;
                //    DateTime lastTime = startTime;
                //    selectedRowIndex = id;
                //    List<vAppointment> lstAppointmentTemp = lstAppointment.Where(t => !t.IsWaitingList).GroupBy(x => x.StartTime).Select(a => a.First()).OrderBy(z => z.StartTime).ToList();
                //    bool flagIsIdDefine = false;
                //    if (addStartTime)
                //    {
                //        if (!(lstAppointmentTemp.Where(t => t.StartTime == lastTime.ToString("HH:mm")).Count() > 0))
                //        {
                //            ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                //            //                            temp.ID = ++id;
                //            temp.ID = 1;
                //            temp.Time = lastTime.ToString("HH:mm");
                //            idtemp = id;
                //            temp.ParentID = idtemp;
                //            flagIsIdDefine = true;
                //            temp.EndTime = end;
                //            ListDisplayAppTime.Add(temp);
                //        }
                //    }
                //    //                    addStartTime = false;
                //    foreach (vAppointment entityAppointment in lstAppointmentTemp)
                //    {
                //        if (DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)) >= startTime &&
                //            DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)) <= endTime)
                //        {
                //            ParamedicScheduleCustom entity = new ParamedicScheduleCustom();
                //            //                            entity.ID = ++id;
                //            entity.ID = 1;
                //            entity.Time = entityAppointment.StartTime;
                //            if (ct == 0 && !flagIsIdDefine) idtemp = id;
                //            entity.ParentID = idtemp;
                //            ct++;
                //            lastTime = DateTime.Parse(String.Format("2012-01-28 {0}", entityAppointment.StartTime)).AddMinutes(Convert.ToInt32(entityAppointment.VisitDuration));

                //            if (entity.Time == start)
                //            {
                //                entity.EndTime = end;
                //                ListDisplayAppTime.Add(entity);
                //            }
                //        }
                //    }

                //    if (ct != 0)
                //    {
                //        ParamedicScheduleCustom temp = new ParamedicScheduleCustom();
                //        temp.ID = ++id;
                //        temp.Time = lastTime.ToString("HH:mm");
                //        temp.ParentID = idtemp;
                //        addStartTime = true;
                //    }
                //}
            }

            List<ParamedicScheduleCustom> ListDisplayAppTimeFinalCheck = new List<ParamedicScheduleCustom>();

            foreach (ParamedicScheduleCustom e in ListDisplayAppTime)
            {
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

        private void SettingShedulegrdAppointmentNoTimeSlotList(List<ParamedicScheduleCustom> lstParamedicScheduleCustom, string start, string end)
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
            DateTime StartMaster = DateTime.Parse(selectedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + ' ' + start);
            DateTime EndMaster = DateTime.Parse(selectedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + ' ' + end);

            List<vAppointment> lstTempAppointment = lstAppointment.Where(t => !t.IsWaitingList).OrderBy(a => a.CreatedDate).ToList();
            List<vAppointment> lstValid = new List<vAppointment>();

            foreach (vAppointment a in lstTempAppointment)
            {
                DateTime startAppo = DateTime.Parse(a.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + ' ' + a.StartTime);
                DateTime endAppo = DateTime.Parse(a.StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2) + ' ' + a.StartTime);

                if (startAppo.TimeOfDay >= StartMaster.TimeOfDay && endAppo.TimeOfDay <= EndMaster.TimeOfDay)
                {
                    lstValid.Add(a);
                }
            }

            int ct = 1;
            foreach (vAppointment entity in lstValid.OrderBy(t => t.QueueNo))
            {
                ParamedicScheduleCustom newEntity = new ParamedicScheduleCustom();
                newEntity.Queue = ct;
                entity.cfQueue = ct;
                lstParamedicScheduleCustom.Add(newEntity);
                ct++;
            }

            lstParamedicScheduleCustom.Add(new ParamedicScheduleCustom()
            {
                Queue = ct
            });
        }

        List<vAppointment> lstAppointment = null;
        protected void cbpAppointmentChangeAppointment_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string param = e.Parameter;

            Int32 HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnitChangeAppointment.Value);
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

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

            List<ParamedicScheduleCustom> ListDisplayAppTime = new List<ParamedicScheduleCustom>();
            List<ParamedicScheduleCustom> ListDisplayAppTimeWithoutTimeSlot = new List<ParamedicScheduleCustom>();
            List<ParamedicScheduleCustom> ListDisplayWaitingList = new List<ParamedicScheduleCustom>();

            int selectedRowIndex = 0;
            if (obj != null && objSchDate == null)
            {
                hdnRoomIDDefaultCtl.Value = Convert.ToString(obj.RoomID);
                hdnRoomCodeDefaultCtl.Value = obj.RoomCode;
                hdnRoomNameDefaultCtl.Value = obj.RoomName;

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
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                    lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                    lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "3" });
                    lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", obj.StartTime5, obj.EndTime5), StandardCodeID = "4" });
                    Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                }
                else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 == "")
                {
                    List<StandardCode> lstSession = new List<StandardCode>();
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                    lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                    lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", obj.StartTime4, obj.EndTime4), StandardCodeID = "3" });
                    Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                }
                else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                {
                    List<StandardCode> lstSession = new List<StandardCode>();
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                    lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", obj.StartTime3, obj.EndTime3), StandardCodeID = "2" });
                    Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                }
                else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                {
                    List<StandardCode> lstSession = new List<StandardCode>();
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                    lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", obj.StartTime2, obj.EndTime2), StandardCodeID = "1" });
                    Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                }
                else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                {
                    List<StandardCode> lstSession = new List<StandardCode>();
                    lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", obj.StartTime1, obj.EndTime1), StandardCodeID = "0" });
                    Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                }

                if (param == "changeParamedic")
                {
                    cboSessionCtl.SelectedIndex = 0;
                }
                if (cboSessionCtl.Value != null)
                {
                    if (cboSessionCtl.Value.ToString() == "0" || cboSessionCtl.Value.ToString() == "1" || cboSessionCtl.Value.ToString() == "2" || cboSessionCtl.Value.ToString() == "3" || cboSessionCtl.Value.ToString() == "4")
                    {
                        if (cboSessionCtl.Value.ToString() == "0")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lst1.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lst1.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(obj.MaximumAppointment1);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(obj.MaximumWaitingList1);

                            if (lst1.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment1)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lst1.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList1)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (obj.IsAppointmentByTimeSlot1 && obj.StartTime1 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot1 && obj.StartTime1 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList1)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "1")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lst2.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lst2.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(obj.MaximumAppointment2);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(obj.MaximumWaitingList2);

                            if (lst2.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment2)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lst2.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList2)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (obj.IsAppointmentByTimeSlot2 && obj.StartTime2 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot2 && obj.StartTime2 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList2)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "2")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lst3.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lst3.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(obj.MaximumAppointment3);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(obj.MaximumWaitingList3);

                            if (lst3.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment3)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lst3.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList3)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (obj.IsAppointmentByTimeSlot3 && obj.StartTime3 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot3 && obj.StartTime3 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList3)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "3")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lst4.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lst4.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(obj.MaximumAppointment4);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(obj.MaximumWaitingList4);

                            if (lst4.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment4)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lst4.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList4)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (obj.IsAppointmentByTimeSlot4 && obj.StartTime4 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot4 && obj.StartTime4 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList4)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        if (cboSessionCtl.Value.ToString() == "4")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lst5.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lst5.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(obj.MaximumAppointment5);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(obj.MaximumWaitingList5);

                            if (lst5.Where(a => !a.IsWaitingList).Count() >= obj.MaximumAppointment5)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lst5.Where(a => a.IsWaitingList).Count() >= obj.MaximumWaitingList5)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (obj.IsAppointmentByTimeSlot5 && obj.StartTime5 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!obj.IsAppointmentByTimeSlot5 && obj.StartTime5 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!obj.IsAllowWaitingList5)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                    }
                }

                //test 1
                //                lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED));
                lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                List<ParamedicVisitType> lstIntervalParamedic = BusinessLayer.GetParamedicVisitTypeList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} ORDER BY VisitDuration DESC", HealthcareServiceUnitID, ParamedicID));
                List<ServiceUnitVisitType> lstIntervalHealthcare = BusinessLayer.GetServiceUnitVisitTypeList(String.Format("HealthcareServiceUnitID = {0} ORDER BY VisitDuration DESC", HealthcareServiceUnitID));
                int serviceIntervalParamedic = lstIntervalParamedic.Sum(t => t.VisitDuration);
                int serviceIntervalHealthcare = lstIntervalHealthcare.Sum(t => t.VisitDuration);

                int serviceInterval = 0;
                serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                if (serviceInterval < 1)
                    serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                int id = 0;
                bool isAddStartTime = true;

                //untuk dengan slot Time
                if (obj.StartTime1 != "" && obj.IsAppointmentByTimeSlot1 && cboSessionCtl.Value.ToString() == "0") SettingScheduleTable(ListDisplayAppTime, obj.StartTime1, obj.EndTime1, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                if (obj.StartTime2 != "" && obj.IsAppointmentByTimeSlot2 && cboSessionCtl.Value.ToString() == "1") SettingScheduleTable(ListDisplayAppTime, obj.StartTime2, obj.EndTime2, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                if (obj.StartTime3 != "" && obj.IsAppointmentByTimeSlot3 && cboSessionCtl.Value.ToString() == "2") SettingScheduleTable(ListDisplayAppTime, obj.StartTime3, obj.EndTime3, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                if (obj.StartTime4 != "" && obj.IsAppointmentByTimeSlot4 && cboSessionCtl.Value.ToString() == "3") SettingScheduleTable(ListDisplayAppTime, obj.StartTime4, obj.EndTime4, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                if (obj.StartTime5 != "" && obj.IsAppointmentByTimeSlot5 && cboSessionCtl.Value.ToString() == "4") SettingScheduleTable(ListDisplayAppTime, obj.StartTime5, obj.EndTime5, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);

                //untuk tanpa slot Time
                if (obj.StartTime1 != "" && !obj.IsAppointmentByTimeSlot1 && cboSessionCtl.Value.ToString() == "0") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime1, obj.EndTime1);
                if (obj.StartTime2 != "" && !obj.IsAppointmentByTimeSlot2 && cboSessionCtl.Value.ToString() == "1") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime2, obj.EndTime2);
                if (obj.StartTime3 != "" && !obj.IsAppointmentByTimeSlot3 && cboSessionCtl.Value.ToString() == "2") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime3, obj.EndTime3);
                if (obj.StartTime4 != "" && !obj.IsAppointmentByTimeSlot4 && cboSessionCtl.Value.ToString() == "3") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime4, obj.EndTime4);
                if (obj.StartTime5 != "" && !obj.IsAppointmentByTimeSlot5 && cboSessionCtl.Value.ToString() == "4") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, obj.StartTime5, obj.EndTime5);

                if (obj.StartTime1 != "" && obj.IsAllowWaitingList1 && cboSessionCtl.Value.ToString() == "0") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime1);
                if (obj.StartTime2 != "" && obj.IsAllowWaitingList2 && cboSessionCtl.Value.ToString() == "1") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime2);
                if (obj.StartTime3 != "" && obj.IsAllowWaitingList3 && cboSessionCtl.Value.ToString() == "2") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime3);
                if (obj.StartTime4 != "" && obj.IsAllowWaitingList4 && cboSessionCtl.Value.ToString() == "3") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime4);
                if (obj.StartTime5 != "" && obj.IsAllowWaitingList5 && cboSessionCtl.Value.ToString() == "4") SettingSheduleWaitingList(ListDisplayWaitingList, obj.StartTime5);
            }
            // untuk dokter yang hanya punya paramedicscheduleDate Saja
            else
            {
                if (objSchDate != null)
                {
                    hdnRoomIDDefaultCtl.Value = Convert.ToString(objSchDate.RoomID);
                    hdnRoomCodeDefaultCtl.Value = objSchDate.RoomCode;
                    hdnRoomNameDefaultCtl.Value = objSchDate.RoomName;

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
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "2" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objSchDate.StartTime4, objSchDate.EndTime4), StandardCodeID = "3" });
                        lstSession.Insert(4, new StandardCode { StandardCodeName = string.Format("Sesi 5 ({0} - {1})", objSchDate.StartTime5, objSchDate.EndTime5), StandardCodeID = "4" });
                        Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "2" });
                        lstSession.Insert(3, new StandardCode { StandardCodeName = string.Format("Sesi 4 ({0} - {1})", objSchDate.StartTime4, objSchDate.EndTime4), StandardCodeID = "3" });
                        Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                        lstSession.Insert(2, new StandardCode { StandardCodeName = string.Format("Sesi 3 ({0} - {1})", objSchDate.StartTime3, objSchDate.EndTime3), StandardCodeID = "2" });
                        Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                        lstSession.Insert(1, new StandardCode { StandardCodeName = string.Format("Sesi 2 ({0} - {1})", objSchDate.StartTime2, objSchDate.EndTime2), StandardCodeID = "1" });
                        Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                    }
                    else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                    {
                        List<StandardCode> lstSession = new List<StandardCode>();
                        lstSession.Insert(0, new StandardCode { StandardCodeName = string.Format("Sesi 1 ({0} - {1})", objSchDate.StartTime1, objSchDate.EndTime1), StandardCodeID = "0" });
                        Methods.SetComboBoxField<StandardCode>(cboSessionCtl, lstSession, "StandardCodeName", "StandardCodeID");
                    }

                    if (param == "changeParamedic")
                    {
                        cboSessionCtl.SelectedIndex = 0;
                    }

                    if (cboSessionCtl.Value.ToString() == "0" || cboSessionCtl.Value.ToString() == "1" || cboSessionCtl.Value.ToString() == "2" || cboSessionCtl.Value.ToString() == "3" || cboSessionCtl.Value.ToString() == "4")
                    {
                        if (cboSessionCtl.Value.ToString() == "0")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lstSch1.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lstSch1.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(objSchDate.MaximumAppointment1);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(objSchDate.MaximumWaitingList1);

                            if (lstSch1.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment1)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lstSch1.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList1)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (objSchDate.IsAppointmentByTimeSlot1 && objSchDate.StartTime1 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!objSchDate.IsAppointmentByTimeSlot1 && objSchDate.StartTime1 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!objSchDate.IsAllowWaitingList1)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "1")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lstSch2.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lstSch2.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(objSchDate.MaximumAppointment2);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(objSchDate.MaximumWaitingList2);

                            if (lstSch2.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment2)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lstSch2.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList2)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (objSchDate.IsAppointmentByTimeSlot2 && objSchDate.StartTime2 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!objSchDate.IsAppointmentByTimeSlot2 && objSchDate.StartTime2 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!objSchDate.IsAllowWaitingList2)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "2")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lstSch3.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lstSch3.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(objSchDate.MaximumAppointment3);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(objSchDate.MaximumWaitingList3);

                            if (lstSch3.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment3)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lstSch3.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList3)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (objSchDate.IsAppointmentByTimeSlot3 && objSchDate.StartTime3 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!objSchDate.IsAppointmentByTimeSlot3 && objSchDate.StartTime3 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!objSchDate.IsAllowWaitingList3)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        else if (cboSessionCtl.Value.ToString() == "3")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lstSch4.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lstSch4.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(objSchDate.MaximumAppointment4);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(objSchDate.MaximumWaitingList4);

                            if (lstSch4.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment4)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lstSch4.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList4)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (objSchDate.IsAppointmentByTimeSlot4 && objSchDate.StartTime4 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!objSchDate.IsAppointmentByTimeSlot4 && objSchDate.StartTime4 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!objSchDate.IsAllowWaitingList4)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                        if (cboSessionCtl.Value.ToString() == "4")
                        {
                            #region take validation info
                            hdnTotalAppoMessageCtl.Value = Convert.ToString(lstSch5.Where(a => !a.IsWaitingList).Count());
                            hdnTotalWaitingMessageCtl.Value = Convert.ToString(lstSch5.Where(a => a.IsWaitingList).Count());
                            hdnMaxAppoMessageCtl.Value = Convert.ToString(objSchDate.MaximumAppointment5);
                            hdnMaxWaitingMessageCtl.Value = Convert.ToString(objSchDate.MaximumWaitingList5);

                            if (lstSch5.Where(a => !a.IsWaitingList).Count() >= objSchDate.MaximumAppointment5)
                            {
                                hdnIsValidAppoMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidAppoMessageCtl.Value = "1";
                            }

                            if (lstSch5.Where(a => a.IsWaitingList).Count() >= objSchDate.MaximumWaitingList5)
                            {
                                hdnIsValidWaitingMessageCtl.Value = "0";
                            }
                            else
                            {
                                hdnIsValidWaitingMessageCtl.Value = "1";
                            }
                            #endregion

                            #region validate ulTab
                            if (objSchDate.IsAppointmentByTimeSlot5 && objSchDate.StartTime5 != "")
                            {
                                containerAppointmentCtl.Style.Remove("display");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "1";
                            }
                            else if (!objSchDate.IsAppointmentByTimeSlot5 && objSchDate.StartTime5 != "")
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Remove("display");

                                hdnIsShowTimeSlotContainerEntryCtl.Value = "0";
                            }
                            else
                            {
                                containerAppointmentCtl.Style.Add("display", "none");
                                containerAppointmentNoTimeSlotCtl.Style.Add("display", "none");
                            }

                            if (!objSchDate.IsAllowWaitingList5)
                            {
                                containerWaitingListCtl.Style.Add("display", "none");
                            }
                            else
                            {
                                containerWaitingListCtl.Style.Remove("display");
                            }
                            #endregion
                        }
                    }

                    //test 2
                    //                    lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}' AND GCAppointmentStatus != '{3}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112), Constant.AppointmentStatus.DELETED));
                    lstAppointment = BusinessLayer.GetvAppointmentList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND StartDate = '{2}'", HealthcareServiceUnitID, ParamedicID, selectedDate.ToString(Constant.FormatString.DATE_FORMAT_112)));
                    List<ParamedicVisitType> lstIntervalParamedic = BusinessLayer.GetParamedicVisitTypeList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} ORDER BY VisitDuration DESC", HealthcareServiceUnitID, ParamedicID));
                    List<ServiceUnitVisitType> lstIntervalHealthcare = BusinessLayer.GetServiceUnitVisitTypeList(String.Format("HealthcareServiceUnitID = {0} ORDER BY VisitDuration DESC", HealthcareServiceUnitID));
                    int serviceIntervalParamedic = lstIntervalParamedic.Sum(t => t.VisitDuration);
                    int serviceIntervalHealthcare = lstIntervalHealthcare.Sum(t => t.VisitDuration);

                    int serviceInterval = 0;
                    serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                    if (serviceInterval < 1)
                        serviceInterval = Convert.ToInt32(hdnDefaultServiceUnitInterval.Value);
                    int id = 0;
                    bool isAddStartTime = true;

                    //untuk dengan slot Time
                    if (objSchDate.StartTime1 != "" && objSchDate.IsAppointmentByTimeSlot1 && cboSessionCtl.Value.ToString() == "0") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime1, objSchDate.EndTime1, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (objSchDate.StartTime2 != "" && objSchDate.IsAppointmentByTimeSlot2 && cboSessionCtl.Value.ToString() == "1") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime2, objSchDate.EndTime2, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (objSchDate.StartTime3 != "" && objSchDate.IsAppointmentByTimeSlot3 && cboSessionCtl.Value.ToString() == "2") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime3, objSchDate.EndTime3, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (objSchDate.StartTime4 != "" && objSchDate.IsAppointmentByTimeSlot4 && cboSessionCtl.Value.ToString() == "3") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime4, objSchDate.EndTime4, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);
                    if (objSchDate.StartTime5 != "" && objSchDate.IsAppointmentByTimeSlot5 && cboSessionCtl.Value.ToString() == "4") SettingScheduleTable(ListDisplayAppTime, objSchDate.StartTime5, objSchDate.EndTime5, serviceInterval, ref id, ref selectedRowIndex, ref isAddStartTime);

                    //untuk tanpa slot Time
                    if (objSchDate.StartTime1 != "" && !objSchDate.IsAppointmentByTimeSlot1 && cboSessionCtl.Value.ToString() == "0") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime1, objSchDate.EndTime1);
                    if (objSchDate.StartTime2 != "" && !objSchDate.IsAppointmentByTimeSlot2 && cboSessionCtl.Value.ToString() == "1") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime2, objSchDate.EndTime2);
                    if (objSchDate.StartTime3 != "" && !objSchDate.IsAppointmentByTimeSlot3 && cboSessionCtl.Value.ToString() == "2") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime3, objSchDate.EndTime3);
                    if (objSchDate.StartTime4 != "" && !objSchDate.IsAppointmentByTimeSlot4 && cboSessionCtl.Value.ToString() == "3") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime4, objSchDate.EndTime4);
                    if (objSchDate.StartTime5 != "" && !objSchDate.IsAppointmentByTimeSlot5 && cboSessionCtl.Value.ToString() == "4") SettingShedulegrdAppointmentNoTimeSlotList(ListDisplayAppTimeWithoutTimeSlot, objSchDate.StartTime5, objSchDate.EndTime5);

                    if (objSchDate.StartTime1 != "" && objSchDate.IsAllowWaitingList1 && cboSessionCtl.Value.ToString() == "0") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime1);
                    if (objSchDate.StartTime2 != "" && objSchDate.IsAllowWaitingList2 && cboSessionCtl.Value.ToString() == "1") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime2);
                    if (objSchDate.StartTime3 != "" && objSchDate.IsAllowWaitingList3 && cboSessionCtl.Value.ToString() == "2") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime3);
                    if (objSchDate.StartTime4 != "" && objSchDate.IsAllowWaitingList4 && cboSessionCtl.Value.ToString() == "3") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime4);
                    if (objSchDate.StartTime5 != "" && objSchDate.IsAllowWaitingList5 && cboSessionCtl.Value.ToString() == "4") SettingSheduleWaitingList(ListDisplayWaitingList, objSchDate.StartTime5);
                }
            }

            grdWaitingList.DataSource = ListDisplayWaitingList;
            grdWaitingList.DataBind();

            grdAppointmentNoTimeSlot.DataSource = ListDisplayAppTimeWithoutTimeSlot;
            grdAppointmentNoTimeSlot.DataBind();

            grdAppointment.DataSource = ListDisplayAppTime.OrderBy(o => o.Time).ToList();
            grdAppointment.DataBind();

            string result = Convert.ToString(selectedRowIndex);

            if (ListDisplayAppTime.Count() == 0 && ListDisplayAppTimeWithoutTimeSlot.Count() == 0)
            {
                String filterExpresion = String.Format("ParamedicID = {0} AND IsDeleted = 0 AND ('{1}' BETWEEN StartDate AND EndDate)", ParamedicID, selectedDate.ToString("yyyyMMdd"));
                vParamedicLeaveSchedule leaveEntity = BusinessLayer.GetvParamedicLeaveScheduleList(filterExpresion).FirstOrDefault();

                if (leaveEntity != null)
                {
                    //trTimeSlotCtl.Style.Add("display", "none");

                    //trStartLeaveCtl.Style.Remove("display");
                    //trEndLeaveCtl.Style.Remove("display");
                    //trLeaveReasonCtl.Style.Remove("display");

                    hdnStartLeaveCtlValidMessage.Value = leaveEntity.StartDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + leaveEntity.StartTime;
                    hdnEndLeaveCtlValidMessage.Value = leaveEntity.EndDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + leaveEntity.EndTime;
                    hdnLeaveReasonCtlValidMessage.Value = leaveEntity.ParamedicLeaveReason + " (" + leaveEntity.LeaveOtherReason + ")";

                    result += "|leave";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void SettingSheduleWaitingList(List<ParamedicScheduleCustom> lstParamedicScheduleCustom, string start)
        {
            List<vAppointment> lstTempAppointment = lstAppointment.Where(t => t.IsWaitingList && t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED && t.StartTime == start).OrderBy(a => a.QueueNo).ToList();
            int ct = 1;
            foreach (vAppointment entity in lstTempAppointment)
            {
                ParamedicScheduleCustom newEntity = new ParamedicScheduleCustom();
                newEntity.Queue = ct;
                entity.cfQueue = ct;
                lstParamedicScheduleCustom.Add(newEntity);
                ct++;
            }

            lstParamedicScheduleCustom.Add(new ParamedicScheduleCustom()
            {
                Queue = ct
            });
        }

        protected void grdAppointment_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ParamedicScheduleCustom obj = e.Row.DataItem as ParamedicScheduleCustom;
            if (e.Row.RowType == DataControlRowType.DataRow && obj.Time != null)
            {
                List<vAppointment> lstTemp = new List<vAppointment>();
                List<vAppointment> lstMaster = lstAppointment.Where(t => t.GCAppointmentStatus != Constant.AppointmentStatus.DELETED).ToList();
                for (int i = 0; i < lstMaster.Count; i++)
                {
                    if (obj.Time != null)
                    {
                        vAppointment a = lstMaster[i];

                        int minuteObj = Convert.ToInt32(obj.Time.Substring(3));
                        int hourObj = Convert.ToInt32(obj.Time.Substring(0, 2));

                        int minuteAppoStart = Convert.ToInt32(a.StartTime.Substring(3));
                        int hourAppoStart = Convert.ToInt32(a.StartTime.Substring(0, 2));
                        int minuteAppoEnd = Convert.ToInt32(a.EndTime.Substring(3));
                        int hourAppoEnd = Convert.ToInt32(a.EndTime.Substring(0, 2));

                        DateTime stAppo = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourAppoStart, minuteAppoStart, 0);
                        DateTime enAppo = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourAppoEnd, minuteAppoEnd, 0);
                        DateTime stObj = new DateTime(a.StartDate.Year, a.StartDate.Month, a.StartDate.Day, hourObj, minuteObj, 0);

                        ParamedicMaster pm = BusinessLayer.GetParamedicMaster(Convert.ToInt32(hdnParamedicIDCtl.Value));
                        int serviceInterval = 0;
                        serviceInterval = Convert.ToInt16(pm.VisitDurationDefault);

                        DateTime enObj = stObj.AddMinutes(serviceInterval);

                        if (stAppo.TimeOfDay >= stObj.TimeOfDay && enAppo.TimeOfDay <= enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay == stObj.TimeOfDay && enAppo.TimeOfDay > enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay <= stObj.TimeOfDay && enAppo.TimeOfDay <= enObj.TimeOfDay && enAppo.TimeOfDay > stObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        else if (stAppo.TimeOfDay < stObj.TimeOfDay && enAppo.TimeOfDay > enObj.TimeOfDay)
                        {
                            if (!a.IsWaitingList)
                            {
                                lstTemp.Add(a);
                            }
                        }
                        //else if (stAppo.TimeOfDay < stObj.TimeOfDay && enAppo.TimeOfDay == stObj.TimeOfDay)
                        //{
                        //    if (!a.IsWaitingList)
                        //    {
                        //        lstTemp.Add(a);
                        //    }
                        //}
                    }
                }

                List<AppointmentCustomClass> lstBindAppointment = (from p in lstTemp.ToList()
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime }).ToList();
                AppointmentCustomClass app = new AppointmentCustomClass();

                if (obj.Time == null)
                {
                    app.AppointmentID = -2;
                    lstBindAppointment.Add(app);
                }
                else if (lstBindAppointment.Count() == 0)
                {
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstBindAppointment.Add(app);
                }
                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstBindAppointment;
                rpt.DataBind();
            }
        }

        protected void grdAppointmentNoTimeSlot_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obje = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstNoTimeSlot = (from p in lstAppointment.Where(p => p.cfQueue == obje.Queue && !p.IsWaitingList)
                                                              select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime }).ToList();
                if (lstNoTimeSlot.Count == 0)
                {
                    AppointmentCustomClass app = new AppointmentCustomClass();
                    app.AppointmentID = -1;
                    app.AppointmentNo = "";
                    app.GCAppointmentStatus = "";
                    app.VisitTypeName = "";
                    app.PatientName = "";
                    lstNoTimeSlot.Add(app);
                }

                Repeater rpt = e.Row.FindControl("rptAppointmentInformation") as Repeater;
                rpt.DataSource = lstNoTimeSlot;
                rpt.DataBind();
            }
        }

        protected void grdWaitingList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ParamedicScheduleCustom obje = e.Row.DataItem as ParamedicScheduleCustom;
                List<AppointmentCustomClass> lstBindWaitingList = (from p in lstAppointment.Where(p => p.cfQueue == obje.Queue && p.IsWaitingList && p.GCAppointmentStatus != Constant.AppointmentStatus.DELETED)
                                                                   select new AppointmentCustomClass { AppointmentID = p.AppointmentID, AppointmentNo = p.AppointmentNo, GCAppointmentStatus = p.GCAppointmentStatus, PatientImageUrl = p.PatientImageUrl, PatientName = p.cfPatientName, VisitTypeName = p.VisitTypeName, StartTime = p.StartTime, EndTime = p.EndTime }).ToList();
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

        public class ParamedicScheduleCustom
        {
            public Int32 ID { get; set; }
            public Int32 Queue { get; set; }
            public String Time { get; set; }
            public String EndTime { get; set; }
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
            public String StartTime { get; set; }
            public String EndTime { get; set; }
            public Boolean IsAppointmentCompleted
            {
                get { return GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE; }
            }
        }

        protected bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = false;
            try
            {
                #region Take Master Schedule
                Int32 ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);
                DateTime selectedDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);

                int dayNumber = (int)selectedDate.DayOfWeek;
                if (dayNumber == 0)
                {
                    dayNumber = 7;
                }

                vParamedicSchedule obj = BusinessLayer.GetvParamedicScheduleList(string.Format(
                                                            "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND DayNumber = {2}",
                                                            Convert.ToInt32(cboServiceUnitChangeAppointment.Value), ParamedicID, dayNumber)).FirstOrDefault();

                vParamedicScheduleDate objSchDate = BusinessLayer.GetvParamedicScheduleDateList(string.Format(
                                                                                "HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'",
                                                                                Convert.ToInt32(cboServiceUnitChangeAppointment.Value), ParamedicID, selectedDate)).FirstOrDefault();

                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(ParamedicID);

                ValidateParamedicScSchedule(obj, objSchDate);

                #endregion

                Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(hdnID.Value));
                entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeIDTo.Value);
                entity.VisitDuration = Convert.ToInt16(Request.Form[txtVisitDurationTo.UniqueID]);
                entity.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnitChangeAppointment.Value);
                entity.GCAppointmentMethod = (string)cboAppointmentMethodTo.Value;
                entity.Session = Convert.ToInt32(cboSessionCtl.Value) + 1;

                string filterExp = String.Format("AppointmentID = {0}", entity.AppointmentID);
                List<vAppointment> entityAppointment = BusinessLayer.GetvAppointmentList(filterExp);
                int AppIDTemp = 0;

                #region validate start and end time Appointment
                int h = 0;
                int m = 0;
                String startTimeValidInString = "";
                String endTimeValidInString = "";
                DateTime stAppo = DateTime.Now;
                DateTime stAppoValid = DateTime.Now;
                DateTime enAppo = DateTime.Now;
                string timeSlot = cboSessionCtl.Value.ToString();
                string startTimeCheck = "";
                string endTimeCheck = "";

                if (hdnIsByTimeSlotCtl.Value == "1")
                {
                    h = Convert.ToInt32(Request.Form[txtNewAppointmentTime.UniqueID].Trim().Substring(0, 2));
                    m = Convert.ToInt32(Request.Form[txtNewAppointmentTime.UniqueID].Trim().Substring(3));
                    startTimeValidInString = Request.Form[txtNewAppointmentTime.UniqueID].Trim();

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
                else if (hdnIsByNoTimeSlotCtl.Value == "1")
                {
                    if (timeSlot == "0")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime1 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime1, objSchDate.EndTime1);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime1;
                                endTimeCheck = objSchDate.EndTime1;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime1.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = objSchDate.StartTime1;
                                endTimeValidInString = objSchDate.EndTime1;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime1 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), obj.StartTime1, obj.EndTime1);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime1;
                                endTimeCheck = obj.EndTime1;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime1.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime1.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = obj.StartTime1;
                                endTimeValidInString = obj.EndTime1;
                            }
                        }
                    }
                    else if (timeSlot == "1")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime2 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime2, objSchDate.EndTime2);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime2;
                                endTimeCheck = objSchDate.EndTime2;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime2.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = objSchDate.StartTime2;
                                endTimeValidInString = objSchDate.EndTime2;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime2 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), obj.StartTime2, obj.EndTime2);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime2;
                                endTimeCheck = obj.EndTime2;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime2.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime2.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = obj.StartTime2;
                                endTimeValidInString = obj.EndTime2;
                            }
                        }
                    }
                    else if (timeSlot == "2")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime3 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime3, objSchDate.EndTime3);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime3;
                                endTimeCheck = objSchDate.EndTime3;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime3.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = objSchDate.StartTime3;
                                endTimeValidInString = objSchDate.EndTime3;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime3 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), obj.StartTime3, obj.EndTime3);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime3;
                                endTimeCheck = obj.EndTime3;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime3.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime3.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = obj.StartTime3;
                                endTimeValidInString = obj.EndTime3;
                            }
                        }
                    }
                    else if (timeSlot == "3")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime4 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime4, objSchDate.EndTime4);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime4;
                                endTimeCheck = objSchDate.EndTime4;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime4.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = objSchDate.StartTime4;
                                endTimeValidInString = objSchDate.EndTime4;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime4 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), obj.StartTime4, obj.EndTime4);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime4;
                                endTimeCheck = obj.EndTime4;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime4.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime4.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = obj.StartTime4;
                                endTimeValidInString = obj.EndTime4;
                            }
                        }
                    }
                    else if (timeSlot == "4")
                    {
                        if (objSchDate != null)
                        {
                            if (objSchDate.StartTime5 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), objSchDate.StartTime5, objSchDate.EndTime5);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = objSchDate.StartTime5;
                                endTimeCheck = objSchDate.EndTime5;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(objSchDate.StartTime5.Substring(0, 2));
                                    m = Convert.ToInt32(objSchDate.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = objSchDate.StartTime5;
                                endTimeValidInString = objSchDate.EndTime5;
                            }
                        }
                        else if (obj != null)
                        {
                            if (obj.StartTime5 != "")
                            {
                                String filter = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND isWaitingList = 0 AND ParamedicID = {2} AND HealthcareServiceUnitID = {3} AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}')", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, Convert.ToInt32(cboServiceUnitChangeAppointment.Value), Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), obj.StartTime5, obj.EndTime5);
                                List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filter);

                                startTimeCheck = obj.StartTime5;
                                endTimeCheck = obj.EndTime5;

                                if (lstAppo.Count > 0)
                                {
                                    int duration = 0;
                                    foreach (Appointment a in lstAppo)
                                    {
                                        duration = duration + a.VisitDuration;
                                    }

                                    //set jam mulai dan jam selesai Appointment
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo.AddMinutes(Convert.ToInt16(duration));
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                    //end
                                }
                                else
                                {
                                    DateTime defaultDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                                    h = Convert.ToInt32(obj.StartTime5.Substring(0, 2));
                                    m = Convert.ToInt32(obj.StartTime5.Substring(3));

                                    stAppo = new DateTime(defaultDate.Year, defaultDate.Month, defaultDate.Day, h, m, 0);
                                    stAppoValid = stAppo;
                                    enAppo = stAppoValid.AddMinutes(entity.VisitDuration);
                                }

                                startTimeValidInString = obj.StartTime5;
                                endTimeValidInString = obj.EndTime5;
                            }
                        }
                    }
                }

                if (hdnIsByTimeSlotCtl.Value == "1")
                {
                    if (objSchDate != null)
                    {
                        if (objSchDate.StartTime1 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot1 == false)
                        {
                            endTimeValidInString = objSchDate.EndTime1;
                        }
                        else if (objSchDate.StartTime2 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot2 == false)
                        {
                            endTimeValidInString = objSchDate.EndTime2;
                        }
                        else if (objSchDate.StartTime3 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot3 == false)
                        {
                            endTimeValidInString = objSchDate.EndTime3;
                        }
                        else if (objSchDate.StartTime4 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot4 == false)
                        {
                            endTimeValidInString = objSchDate.EndTime4;
                        }
                        else if (objSchDate.StartTime5 == startTimeValidInString && objSchDate.IsAppointmentByTimeSlot5 == false)
                        {
                            endTimeValidInString = objSchDate.EndTime5;
                        }
                        else
                        {
                            DateTime dtEndTime = new DateTime(2000, 1, 1, h, m, 0);
                            DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(pm.VisitDurationDefault));
                            endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                        }
                    }
                    else if (obj != null && objSchDate == null)
                    {
                        if (obj.StartTime1 == startTimeValidInString && obj.IsAppointmentByTimeSlot1 == false)
                        {
                            endTimeValidInString = obj.EndTime1;
                        }
                        else if (obj.StartTime2 == startTimeValidInString && obj.IsAppointmentByTimeSlot2 == false)
                        {
                            endTimeValidInString = obj.EndTime2;
                        }
                        else if (obj.StartTime3 == startTimeValidInString && obj.IsAppointmentByTimeSlot3 == false)
                        {
                            endTimeValidInString = obj.EndTime3;
                        }
                        else if (obj.StartTime4 == startTimeValidInString && obj.IsAppointmentByTimeSlot4 == false)
                        {
                            endTimeValidInString = obj.EndTime4;
                        }
                        else if (obj.StartTime5 == startTimeValidInString && obj.IsAppointmentByTimeSlot5 == false)
                        {
                            endTimeValidInString = obj.EndTime5;
                        }
                        else
                        {
                            DateTime dtEndTime = new DateTime(2000, 1, 1, h, m, 0);
                            DateTime dtEndTimeValid = dtEndTime.AddMinutes(Convert.ToDouble(pm.VisitDurationDefault));
                            endTimeValidInString = Convert.ToString(dtEndTimeValid.Hour) + ":" + Convert.ToString(dtEndTimeValid.Minute);
                        }
                    }

                    DateTime end = new DateTime(2000, 1, 1, h, m, 0);
                    entity.StartTime = Request.Form[txtNewAppointmentTime.UniqueID].Trim();
                    entity.EndTime = end.AddMinutes(entity.VisitDuration).ToString("HH:mm");
                }
                else if (hdnIsByNoTimeSlotCtl.Value == "1")
                {
                    entity.StartTime = stAppoValid.ToString("HH:mm");
                    entity.EndTime = enAppo.ToString("HH:mm");
                }
                else
                {
                    if (objSchDate != null)
                    {
                        if (timeSlot == "0")
                        {
                            entity.StartTime = startTimeCheck = objSchDate.StartTime1;
                            entity.EndTime = endTimeCheck = objSchDate.StartTime1;
                        }
                        else if (timeSlot == "1")
                        {
                            entity.StartTime = startTimeCheck = objSchDate.StartTime2;
                            entity.EndTime = endTimeCheck = objSchDate.StartTime2;
                        }
                        else if (timeSlot == "2")
                        {
                            entity.StartTime = startTimeCheck = objSchDate.StartTime3;
                            entity.EndTime = endTimeCheck = objSchDate.StartTime3;
                        }
                        else if (timeSlot == "3")
                        {
                            entity.StartTime = startTimeCheck = objSchDate.StartTime4;
                            entity.EndTime = endTimeCheck = objSchDate.StartTime4;
                        }
                        else if (timeSlot == "4")
                        {
                            entity.StartTime = startTimeCheck = objSchDate.StartTime5;
                            entity.EndTime = endTimeCheck = objSchDate.StartTime5;
                        }
                    }
                    else if (obj != null && objSchDate == null)
                    {
                        if (timeSlot == "0")
                        {
                            entity.StartTime = startTimeCheck = obj.StartTime1;
                            entity.EndTime = endTimeCheck = obj.StartTime1;
                        }
                        else if (timeSlot == "1")
                        {
                            entity.StartTime = startTimeCheck = obj.StartTime2;
                            entity.EndTime = endTimeCheck = obj.StartTime2;
                        }
                        else if (timeSlot == "2")
                        {
                            entity.StartTime = startTimeCheck = obj.StartTime3;
                            entity.EndTime = endTimeCheck = obj.StartTime3;
                        }
                        else if (timeSlot == "3")
                        {
                            entity.StartTime = startTimeCheck = obj.StartTime4;
                            entity.EndTime = endTimeCheck = obj.StartTime4;
                        }
                        else if (timeSlot == "4")
                        {
                            entity.StartTime = startTimeCheck = obj.StartTime5;
                            entity.EndTime = endTimeCheck = obj.StartTime5;
                        }
                    }
                }
                #endregion

                string filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND GCAppointmentStatus != '{2}' AND VisitTypeID = {3} AND isWaitingList = 0 AND CONVERT(DATETIME,CONVERT(VARCHAR,StartDate) + ' ' + StartTime) >= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{5}') AND CONVERT(DATETIME,CONVERT(VARCHAR,EndDate) + ' ' + EndTime) <= CONVERT(DATETIME,CONVERT(VARCHAR,'{4}') + ' ' + '{6}') AND AppointmentID != {7}", cboServiceUnitChangeAppointment.Value, hdnParamedicIDCtl.Value, Constant.AppointmentStatus.DELETED, hdnVisitTypeIDTo.Value, Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value).ToString("yyyy-MM-dd"), startTimeValidInString, endTimeValidInString, hdnID.Value);
                int count = BusinessLayer.GetAppointmentRowCount(filterExpression);
                if (count > 0 && !(hdnIsWaitingListCtl.Value == "1") && !(hdnIsByNoTimeSlotCtl.Value == "1"))
                {
                    errMessage = string.Format("Jenis Kunjungan {0} Sudah Digunakan Di Slot Ini. Perjanjian Tidak Dapat Diproses", Request.Form[txtVisitTypeNameTo.UniqueID]);
                    result = false;
                }
                else
                {
                    if (hdnRoomIDTo.Value != "0" && hdnRoomIDTo != null && hdnRoomIDTo.Value != "")
                    {
                        entity.RoomID = Convert.ToInt32(hdnRoomIDTo.Value);
                    }
                    else
                    {
                        entity.RoomID = null;
                    }

                    entity.StartDate = entity.EndDate = Helper.GetDatePickerValue(hdnCalAppointmentSelectedDateCtl.Value);
                    entity.ParamedicID = Convert.ToInt32(hdnParamedicIDCtl.Value);

                    //if (hdnIsWaitingListCtl.Value == "0")
                    //{
                    //    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, startTimeCheck, endTimeCheck, 0) + 1);
                    //}
                    //else
                    //{
                    //    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNoAppointment(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, startTimeCheck, endTimeCheck, 1) + 1);
                    //}
                    //                    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate) + 1);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    #region validate maximum Appoitment & waiting List
                    if (hdnIsWaitingListCtl.Value == "0")
                    {
                        DateTime AppointmentStart = DateTime.Parse("2012-01-28" + ' ' + entity.StartTime);
                        DateTime AppointmentEnd = DateTime.Parse("2012-01-28" + ' ' + entity.EndTime);

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

                            if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 != "")
                            {
                                if (AppointmentStart.TimeOfDay >= objSchStart5.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd5.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd4.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 != "" && objSchDate.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objSchStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd4.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 != "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objSchStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 != "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objSchStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (objSchDate.StartTime1 != "" && objSchDate.StartTime2 == "" && objSchDate.StartTime3 == "" && objSchDate.StartTime4 == "" && objSchDate.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objSchStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objSchEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false; 
                                    bool isBPJS = false;
                                    if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                    {
                                        isBPJS = true;
                                    }
                                    entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                    //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                    BusinessLayer.UpdateAppointment(entity);
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                        }
                        else
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

                            if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "" && obj.StartTime5 != "")
                            {
                                if (AppointmentStart.TimeOfDay >= objStart5.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd5.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd4.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 != "")
                            {
                                if (AppointmentStart.TimeOfDay >= objStart4.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd4.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 != "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objStart3.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd3.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (obj.StartTime1 != "" && obj.StartTime2 != "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objStart2.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd2.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                            else if (obj.StartTime1 != "" && obj.StartTime2 == "" && obj.StartTime3 == "" && obj.StartTime4 == "" && obj.StartTime5 == "")
                            {
                                if (AppointmentStart.TimeOfDay >= objStart1.TimeOfDay && AppointmentEnd.TimeOfDay <= objEnd1.TimeOfDay)
                                {
                                    entity.IsWaitingList = false;
                                    if (hdnIsVoidAndNoTimeSlot.Value == "0")
                                    {
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                                        BusinessLayer.UpdateAppointment(entity);
                                    }
                                    else
                                    {
                                        Appointment a = new Appointment();
                                        bool isBPJS = false;
                                        if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                                        ReplcaceAppointment(a, entity);
                                        BusinessLayer.InsertAppointment(a);
                                        AppIDTemp = a.AppointmentID;
                                    }
                                    result = true;
                                }
                                else
                                {
                                    errMessage = "Maaf Durasi Kunjungan Tidak Valid";
                                    result = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        entity.IsWaitingList = true;
                        if (hdnIsVoidAndNoTimeSlot.Value == "0")
                        {
                            bool isBPJS = false;
                            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                            {
                                isBPJS = true;
                            }
                            entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(entity.Session) + 1), false, isBPJS, 0));
                            //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), 1));
                            BusinessLayer.UpdateAppointment(entity);
                        }
                        else
                        {
                            Appointment a = new Appointment();
                            bool isBPJS = false;
                            if (entity.GCCustomerType == Constant.CustomerType.BPJS)
                            {
                                isBPJS = true;
                            }
                            entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.StartDate, (Convert.ToInt32(cboSessionCtl.Value) + 1), false, isBPJS, 0));
                            ReplcaceAppointment(a, entity);
                            a.IsWaitingList = true;
                            BusinessLayer.InsertAppointment(a);
                            AppIDTemp = a.AppointmentID;
                        }
                        result = true;
                    }
                    #endregion
                }
                if (AppIDTemp != 0)
                {
                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        string filterexp = String.Format("AppointmentID = {0}", AppIDTemp);
                        List<vAppointment> entityAppo = BusinessLayer.GetvAppointmentList(filterexp);

                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnChangedAppointmentInformation("UPDATE", entityAppo);
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
                    //#endregion
                }
                else
                {
                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnChangedAppointmentInformation("UPDATE", entityAppointment);
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
                    //#endregion
                }

                return result;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}