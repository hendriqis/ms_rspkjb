using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PSScheduleDayEntryCtl : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        //public class CDaySchedule
        //{
        //    public int DayNumber { get; set; }
        //    public int OperationalTimeID { get; set; }
        //    public  MyProperty { get; set; }
        //    public CheckBox chk { get; set; }
        //}

        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;

            List<ParamedicSchedule> listParamedicSchedule = BusinessLayer.GetParamedicScheduleList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value));

            //SetControlProperties();

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtOperationalTimeCodeCtl, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, false), "mpEntryPopup");

            List<Variable> lstVar = new List<Variable>(){ 
                new Variable{ Code = "Senin",Value = "1"},
                new Variable{ Code = "Selasa",Value = "2"},
                new Variable{ Code = "Rabu",Value = "3"},
                new Variable{ Code = "Kamis",Value = "4"},
                new Variable{ Code = "Jumat",Value = "5"},
                new Variable{ Code = "Sabtu",Value = "6"},
                new Variable{ Code = "Minggu",Value = "7"}};
            Methods.SetComboBoxField<Variable>(cboDay, lstVar, "Code", "Value");
            cboDay.SelectedIndex = 0;
            GetSettingParameter();

            if (hdnIsAllowDifferentQueueNo.Value == "0")
            {
                trIsAllowDifferentQueueNo.Attributes.Remove("style");
            }
            else
            {
                trIsAllowDifferentQueueNo.Attributes.Add("style", "display:none");
            }
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}', '{4}', '{5}')", 
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, 
                Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, 
                Constant.SettingParameter.OP_CHECK_APPOINTMENT_BEFORE_CHANGE_PHYSICIAN_SCHEDULE, 
                Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS,
                Constant.SettingParameter.OP_IS_QUEUE_NO_USING_APPOINTMENT));

            hdnCheckAppointmentBeforeChangeSchedule.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.OP_CHECK_APPOINTMENT_BEFORE_CHANGE_PHYSICIAN_SCHEDULE).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMedinfrasMobileApps.Value = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsAllowDifferentQueueNo.Value = lstSettingParameter.Where(t => t.ParameterCode == Constant.SettingParameter.OP_IS_QUEUE_NO_USING_APPOINTMENT).FirstOrDefault().ParameterValue;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value);
            List<vParamedicSchedule> lstSchedule = BusinessLayer.GetvParamedicScheduleList(filterExpression);
            grdView.DataSource = lstSchedule;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            int pageCount = 1;

            string[] param = e.Parameter.Split('|');

            string result = param[0] + "|";
            string errMessage = "";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = "refresh|" + pageCount;
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                {
                    result += "success";
                    BindGridView(1, true, ref pageCount);
                }
                else
                {
                    result += string.Format("fail|{0}", errMessage);
                }
            }
            else
            {
                if (param[0] == "save")
                {
                    if (hdnHealthcareServiceUnitID.Value != "" && hdnParamedicID.Value != "" && hdnDayNumber.Value != "")
                    {
                        if (OnSaveEditRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecord(ref errMessage))
                        {
                            result += "success";
                            BindGridView(1, true, ref pageCount);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region ControlToEntity
        private void ControlToEntity(ParamedicSchedule entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.DayNumber = Convert.ToInt16(cboDay.Value);
            if (hdnRoomID.Value != "")
            {
                entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }
            else
            {
                entity.RoomID = null;
            }
            entity.IsAllowDifferentQueueNo = chkIsAllowDifferentQueueNo.Checked;
            entity.Remarks = txtRemarks.Text;
            entity.OperationalTimeID = Convert.ToInt32(hdnOperationalTimeID.Value);
            String FilterExpression = string.Format("OperationalTimeID = {0}", hdnOperationalTimeID.Value);
            OperationalTime operationalTime = BusinessLayer.GetOperationalTimeList(FilterExpression).FirstOrDefault();

            #region IsAppointmentByTimeSlot

            String Start1 = operationalTime.StartTime1;
            String Start2 = operationalTime.StartTime2;
            String Start3 = operationalTime.StartTime3;
            String Start4 = operationalTime.StartTime4;
            String Start5 = operationalTime.StartTime5;

            if (Start1 != "" && Start2 != "" && Start3 != "" && Start4 != "" && Start5 != "")
            {
                entity.IsAppointmentByTimeSlot1 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot2 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot3 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot4 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot5 = chkIsAppointmentByTimeSlot.Checked;

            }
            else if (Start1 != "" && Start2 != "" && Start3 != "" && Start4 != "" && Start5 == "")
            {
                entity.IsAppointmentByTimeSlot1 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot2 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot3 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot4 = chkIsAppointmentByTimeSlot.Checked;
            }
            else if (Start1 != "" && Start2 != "" && Start3 != "" && Start4 == "" && Start5 == "")
            {
                entity.IsAppointmentByTimeSlot1 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot2 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot3 = chkIsAppointmentByTimeSlot.Checked;
            }
            else if (Start1 != "" && Start2 != "" && Start3 == "" && Start4 == "" && Start5 == "")
            {
                entity.IsAppointmentByTimeSlot1 = chkIsAppointmentByTimeSlot.Checked;
                entity.IsAppointmentByTimeSlot2 = chkIsAppointmentByTimeSlot.Checked;
            }
            else if (Start1 != "" && Start2 == "" && Start3 == "" && Start4 == "" && Start5 == "")
            {
                entity.IsAppointmentByTimeSlot1 = chkIsAppointmentByTimeSlot.Checked;
            }
            #endregion

            #region Maximum Appointment
            if (!string.IsNullOrEmpty(txtMaximumAppointment1.Text))
                entity.MaximumAppointment1 = Convert.ToInt16(txtMaximumAppointment1.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment2.Text))
                entity.MaximumAppointment2 = Convert.ToInt16(txtMaximumAppointment2.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment3.Text))
                entity.MaximumAppointment3 = Convert.ToInt16(txtMaximumAppointment3.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment4.Text))
                entity.MaximumAppointment4 = Convert.ToInt16(txtMaximumAppointment4.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment5.Text))
                entity.MaximumAppointment5 = Convert.ToInt16(txtMaximumAppointment5.Text);

            if (!string.IsNullOrEmpty(txtMaximumAppointment1BPJS.Text))
                entity.MaximumAppointmentBPJS1 = Convert.ToInt16(txtMaximumAppointment1BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment2BPJS.Text))
                entity.MaximumAppointmentBPJS2 = Convert.ToInt16(txtMaximumAppointment2BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment3BPJS.Text))
                entity.MaximumAppointmentBPJS3 = Convert.ToInt16(txtMaximumAppointment3BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment4BPJS.Text))
                entity.MaximumAppointmentBPJS4 = Convert.ToInt16(txtMaximumAppointment4BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment5BPJS.Text))
                entity.MaximumAppointmentBPJS5 = Convert.ToInt16(txtMaximumAppointment5BPJS.Text); 

            #endregion

            #region Kuota Online/Mobile
            if (!string.IsNullOrEmpty(txtMobileAppointment1.Text))
                entity.MobileAppointment1 = Convert.ToByte(txtMobileAppointment1.Text);
            if (!string.IsNullOrEmpty(txtMobileAppointment1BPJS.Text))
                entity.MobileAppointmentBPJS1 = Convert.ToByte(txtMobileAppointment1BPJS.Text);

            if (!string.IsNullOrEmpty(txtMobileAppointment2.Text))
                entity.MobileAppointment2 = Convert.ToByte(txtMobileAppointment2.Text);
            if (!string.IsNullOrEmpty(txtMobileAppointment2BPJS.Text))
                entity.MobileAppointmentBPJS2 = Convert.ToByte(txtMobileAppointment2BPJS.Text);

            if (!string.IsNullOrEmpty(txtMobileAppointment3.Text))
                entity.MobileAppointment3 = Convert.ToByte(txtMobileAppointment3.Text);
            if (!string.IsNullOrEmpty(txtMobileAppointment3BPJS.Text))
                entity.MobileAppointmentBPJS3 = Convert.ToByte(txtMobileAppointment3BPJS.Text);

            if (!string.IsNullOrEmpty(txtMobileAppointment4.Text))
                entity.MobileAppointment4 = Convert.ToByte(txtMobileAppointment4.Text);
            if (!string.IsNullOrEmpty(txtMobileAppointment4BPJS.Text))
                entity.MobileAppointmentBPJS4 = Convert.ToByte(txtMobileAppointment4BPJS.Text);

            if (!string.IsNullOrEmpty(txtMobileAppointment5.Text))
                entity.MobileAppointment5 = Convert.ToByte(txtMobileAppointment5.Text);
            if (!string.IsNullOrEmpty(txtMobileAppointment5BPJS.Text))
                entity.MobileAppointmentBPJS5 = Convert.ToByte(txtMobileAppointment5BPJS.Text);
            #endregion

            #region VisitDuration 
            if (!string.IsNullOrEmpty(txtVisitDuration1.Text))
            {
                entity.VisitDuration1 = Convert.ToInt32(txtVisitDuration1.Text);
            }
            if (!string.IsNullOrEmpty(txtVisitDuration2.Text))
            {
                entity.VisitDuration2 = Convert.ToInt32(txtVisitDuration2.Text);
            }
            if (!string.IsNullOrEmpty(txtVisitDuration3.Text))
            {
                entity.VisitDuration3 = Convert.ToInt32(txtVisitDuration3.Text);
            }
            if (!string.IsNullOrEmpty(txtVisitDuration4.Text))
            {
                entity.VisitDuration4 = Convert.ToInt32(txtVisitDuration4.Text);
            }
            if (!string.IsNullOrEmpty(txtVisitDuration5.Text))
            {
                entity.VisitDuration5 = Convert.ToInt32(txtVisitDuration5.Text);
            }
            #endregion


            #region Reserved Queue Start No
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo1.Text))
                entity.ReservedQueueStartNo1 = Convert.ToByte(txtReservedQueueStartNo1.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo1BPJS.Text))
                entity.ReservedQueueStartNoBPJS1 = Convert.ToByte(txtReservedQueueStartNo1BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueStartNo2.Text))
                entity.ReservedQueueStartNo2 = Convert.ToByte(txtReservedQueueStartNo2.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo2BPJS.Text))
                entity.ReservedQueueStartNoBPJS2 = Convert.ToByte(txtReservedQueueStartNo2BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueStartNo3.Text))
                entity.ReservedQueueStartNo3 = Convert.ToByte(txtReservedQueueStartNo3.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo3BPJS.Text))
                entity.ReservedQueueStartNoBPJS3 = Convert.ToByte(txtReservedQueueStartNo3BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueStartNo4.Text))
                entity.ReservedQueueStartNo4 = Convert.ToByte(txtReservedQueueStartNo4.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo4BPJS.Text))
                entity.ReservedQueueStartNoBPJS4 = Convert.ToByte(txtReservedQueueStartNo4BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueStartNo5.Text))
                entity.ReservedQueueStartNo5 = Convert.ToByte(txtReservedQueueStartNo5.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo5BPJS.Text))
                entity.ReservedQueueStartNoBPJS5 = Convert.ToByte(txtReservedQueueStartNo5BPJS.Text);

            #endregion

            #region Reserved Queue End No
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo1.Text))
                entity.ReservedQueueEndNo1 = Convert.ToByte(txtReservedQueueEndNo1.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo1BPJS.Text))
                entity.ReservedQueueEndNoBPJS1 = Convert.ToByte(txtReservedQueueEndNo1BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueEndNo2.Text))
                entity.ReservedQueueEndNo2 = Convert.ToByte(txtReservedQueueEndNo2.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo2BPJS.Text))
                entity.ReservedQueueEndNoBPJS2 = Convert.ToByte(txtReservedQueueEndNo2BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueEndNo3.Text))
                entity.ReservedQueueEndNo3 = Convert.ToByte(txtReservedQueueEndNo3.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo3BPJS.Text))
                entity.ReservedQueueEndNoBPJS3 = Convert.ToByte(txtReservedQueueEndNo3BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueEndNo4.Text))
                entity.ReservedQueueEndNo4 = Convert.ToByte(txtReservedQueueEndNo4.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo4BPJS.Text))
                entity.ReservedQueueEndNoBPJS4 = Convert.ToByte(txtReservedQueueEndNo4BPJS.Text);

            if (!string.IsNullOrEmpty(txtReservedQueueEndNo5.Text))
                entity.ReservedQueueEndNo5 = Convert.ToByte(txtReservedQueueEndNo5.Text);
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo5BPJS.Text))
                entity.ReservedQueueEndNoBPJS5 = Convert.ToByte(txtReservedQueueEndNo5BPJS.Text);
            #endregion

            #region AllowWaitingList
            entity.IsAllowWaitingList1 = chkIsAllowWaitingList1.Checked;
            entity.IsAllowWaitingList2 = chkIsAllowWaitingList2.Checked;
            entity.IsAllowWaitingList3 = chkIsAllowWaitingList3.Checked;
            entity.IsAllowWaitingList4 = chkIsAllowWaitingList4.Checked;
            entity.IsAllowWaitingList5 = chkIsAllowWaitingList5.Checked;
            #endregion

            #region Maximum Waiting List
            if (!string.IsNullOrEmpty(txtMaximumWaitingList1.Text))
                entity.MaximumWaitingList1 = Convert.ToInt16(txtMaximumWaitingList1.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList1BPJS.Text))
                entity.MaximumWaitingListBPJS1 = Convert.ToInt16(txtMaximumWaitingList1BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList2.Text))
                entity.MaximumWaitingList2 = Convert.ToInt16(txtMaximumWaitingList2.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList2BPJS.Text))
                entity.MaximumWaitingListBPJS2 = Convert.ToInt16(txtMaximumWaitingList2BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList3.Text))
                entity.MaximumWaitingList3 = Convert.ToInt16(txtMaximumWaitingList3.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList3BPJS.Text))
                entity.MaximumWaitingListBPJS3 = Convert.ToInt16(txtMaximumWaitingList3BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList4.Text))
                entity.MaximumWaitingList4 = Convert.ToInt16(txtMaximumWaitingList4.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList4BPJS.Text))
                entity.MaximumWaitingListBPJS4 = Convert.ToInt16(txtMaximumWaitingList4BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList5.Text))
                entity.MaximumWaitingList5 = Convert.ToInt16(txtMaximumWaitingList5.Text);
            if (!string.IsNullOrEmpty(txtMaximumWaitingList5BPJS.Text))
                entity.MaximumWaitingListBPJS5 = Convert.ToInt16(txtMaximumWaitingList5BPJS.Text);
            #endregion

            #region Maximum Online Waiting List
            if (!string.IsNullOrEmpty(txtWaiting1.Text))
                entity.MobileWaitingList1 = Convert.ToByte(txtWaiting1.Text);
            if (!string.IsNullOrEmpty(txtWaiting1BPJS.Text))
                entity.MobileWaitingListBPJS1 = Convert.ToByte(txtWaiting1BPJS.Text);
            if (!string.IsNullOrEmpty(txtWaiting2.Text))
                entity.MobileWaitingList2 = Convert.ToByte(txtWaiting2.Text);
            if (!string.IsNullOrEmpty(txtWaiting2BPJS.Text))
                entity.MobileWaitingListBPJS2 = Convert.ToByte(txtWaiting2BPJS.Text);
            if (!string.IsNullOrEmpty(txtWaiting3.Text))
                entity.MobileWaitingList3 = Convert.ToByte(txtWaiting3.Text);
            if (!string.IsNullOrEmpty(txtWaiting3BPJS.Text))
                entity.MobileWaitingListBPJS3 = Convert.ToByte(txtWaiting3BPJS.Text);
            if (!string.IsNullOrEmpty(txtWaiting4.Text))
                entity.MobileWaitingList4 = Convert.ToByte(txtWaiting4.Text);
            if (!string.IsNullOrEmpty(txtWaiting4BPJS.Text))
                entity.MobileWaitingListBPJS4 = Convert.ToByte(txtWaiting4BPJS.Text);
            if (!string.IsNullOrEmpty(txtWaiting5.Text))
                entity.MobileWaitingList5 = Convert.ToByte(txtWaiting5.Text);
            if (!string.IsNullOrEmpty(txtWaiting5BPJS.Text))
                entity.MobileWaitingListBPJS5 = Convert.ToByte(txtWaiting5BPJS.Text);
            #endregion

            #region Is BPJS/Non BPJS Appointment
            entity.IsBPJS1 = chkIsBPJS1.Checked;
            entity.IsBPJS2 = chkIsBPJS2.Checked;
            entity.IsBPJS3 = chkIsBPJS3.Checked;
            entity.IsBPJS4 = chkIsBPJS4.Checked;
            entity.IsBPJS5 = chkIsBPJS5.Checked;

            entity.IsNonBPJS1 = chkIsNonBPJS1.Checked;
            entity.IsNonBPJS2 = chkIsNonBPJS2.Checked;
            entity.IsNonBPJS3 = chkIsNonBPJS3.Checked;
            entity.IsNonBPJS4 = chkIsNonBPJS4.Checked;
            entity.IsNonBPJS5 = chkIsNonBPJS5.Checked;
            #endregion
        }
        #endregion

        #region EntityToControl
        private void EntityToControl(vParamedicSchedule entity)
        {
            txtOperationalTimeCodeCtl.Text = entity.OperationalTimeCode;
            txtOperationalTimeNameCtl.Text = entity.OperationalTimeName;
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtRemarks.Text = entity.Remarks;

            #region StartTime
            txtStartTime1.Text = entity.StartTime1;
            txtStartTime2.Text = entity.StartTime2;
            txtStartTime3.Text = entity.StartTime3;
            txtStartTime4.Text = entity.StartTime4;
            txtStartTime5.Text = entity.StartTime5;
            #endregion

            #region StartEnd
            txtEndTime1.Text = entity.EndTime1;
            txtEndTime2.Text = entity.EndTime2;
            txtEndTime3.Text = entity.EndTime3;
            txtEndTime4.Text = entity.EndTime4;
            txtEndTime5.Text = entity.EndTime5;
            #endregion

            #region IsAppointmentByTimeSlot
            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot1) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;

            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot1) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;

            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot2) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;

            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot3) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;

            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot4) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;

            if (Convert.ToInt16(entity.IsAppointmentByTimeSlot4) != 0)
                chkIsAppointmentByTimeSlot.Checked = true;
            else
                chkIsAppointmentByTimeSlot.Checked = false;
            #endregion

            #region MyRegion
            chkIsBPJS1.Checked = entity.IsBPJS1;
            chkIsBPJS2.Checked = entity.IsBPJS2;
            chkIsBPJS3.Checked = entity.IsBPJS3;
            chkIsBPJS4.Checked = entity.IsBPJS4;
            chkIsBPJS5.Checked = entity.IsBPJS5;
            chkIsNonBPJS1.Checked = entity.IsNonBPJS1;
            chkIsNonBPJS2.Checked = entity.IsNonBPJS2;
            chkIsNonBPJS3.Checked = entity.IsNonBPJS3;
            chkIsNonBPJS4.Checked = entity.IsNonBPJS4;
            chkIsNonBPJS5.Checked = entity.IsNonBPJS5;
            #endregion

            #region Maximum Appointment
            txtMaximumAppointment1.Text = Convert.ToString(entity.MaximumAppointment1);
            txtMaximumAppointment2.Text = Convert.ToString(entity.MaximumAppointment2);
            txtMaximumAppointment3.Text = Convert.ToString(entity.MaximumAppointment3);
            txtMaximumAppointment4.Text = Convert.ToString(entity.MaximumAppointment4);
            txtMaximumAppointment5.Text = Convert.ToString(entity.MaximumAppointment5);
            #endregion

            #region Kuota Online / Mobile
            txtMobileAppointment1.Text = Convert.ToString(entity.MobileAppointment1);
            txtMobileAppointment1BPJS.Text = Convert.ToString(entity.MobileAppointmentBPJS1);
            txtMobileAppointment2.Text = Convert.ToString(entity.MobileAppointment2);
            txtMobileAppointment2BPJS.Text = Convert.ToString(entity.MobileAppointmentBPJS2);
            txtMobileAppointment3.Text = Convert.ToString(entity.MobileAppointment3);
            txtMobileAppointment3BPJS.Text = Convert.ToString(entity.MobileAppointmentBPJS3);
            txtMobileAppointment4.Text = Convert.ToString(entity.MobileAppointment4);
            txtMobileAppointment4BPJS.Text = Convert.ToString(entity.MobileAppointmentBPJS4);
            txtMobileAppointment5.Text = Convert.ToString(entity.MobileAppointment5);
            txtMobileAppointment5BPJS.Text = Convert.ToString(entity.MobileAppointmentBPJS5);
            #endregion

            #region IsAllowWaitingList

            if (Convert.ToInt16(entity.IsAllowWaitingList1) != 0)
                chkIsAllowWaitingList1.Checked = true;
            else
                chkIsAllowWaitingList1.Checked = false;

            if (Convert.ToInt16(entity.IsAllowWaitingList2) != 0)
                chkIsAllowWaitingList2.Checked = true;
            else
                chkIsAllowWaitingList2.Checked = false;

            if (Convert.ToInt16(entity.IsAllowWaitingList3) != 0)
                chkIsAllowWaitingList3.Checked = true;
            else
                chkIsAllowWaitingList3.Checked = false;

            if (Convert.ToInt16(entity.IsAllowWaitingList4) != 0)
                chkIsAllowWaitingList4.Checked = true;
            else
                chkIsAllowWaitingList4.Checked = false;

            if (Convert.ToInt16(entity.IsAllowWaitingList5) != 0)
                chkIsAllowWaitingList5.Checked = true;
            else
                chkIsAllowWaitingList5.Checked = false;
            #endregion

            #region Maximum Waiting List
            txtMaximumWaitingList1.Text = Convert.ToString(entity.MaximumWaitingList1);
            txtMaximumWaitingList1BPJS.Text = Convert.ToString(entity.MaximumWaitingListBPJS1);
            txtMaximumWaitingList2.Text = Convert.ToString(entity.MaximumWaitingList2);
            txtMaximumWaitingList2BPJS.Text = Convert.ToString(entity.MaximumWaitingListBPJS2);
            txtMaximumWaitingList3.Text = Convert.ToString(entity.MaximumWaitingList3);
            txtMaximumWaitingList3BPJS.Text = Convert.ToString(entity.MaximumWaitingListBPJS3);
            txtMaximumWaitingList4.Text = Convert.ToString(entity.MaximumWaitingList4);
            txtMaximumWaitingList4BPJS.Text = Convert.ToString(entity.MaximumWaitingListBPJS4);
            txtMaximumWaitingList5.Text = Convert.ToString(entity.MaximumWaitingList5);
            txtMaximumWaitingList5BPJS.Text = Convert.ToString(entity.MaximumWaitingListBPJS5);
            #endregion

            #region Mobile App Percentage Waiting List
            txtWaiting1.Text = Convert.ToString(entity.MobileWaitingList1);
            txtWaiting1BPJS.Text = Convert.ToString(entity.MobileWaitingListBPJS1);
            txtWaiting2.Text = Convert.ToString(entity.MobileWaitingList2);
            txtWaiting2BPJS.Text = Convert.ToString(entity.MobileWaitingListBPJS2);
            txtWaiting3.Text = Convert.ToString(entity.MobileWaitingList3);
            txtWaiting3BPJS.Text = Convert.ToString(entity.MobileWaitingListBPJS3);
            txtWaiting4.Text = Convert.ToString(entity.MobileWaitingList4);
            txtWaiting4BPJS.Text = Convert.ToString(entity.MobileWaitingListBPJS4);
            txtWaiting5.Text = Convert.ToString(entity.MobileWaitingList5);
            txtWaiting5BPJS.Text = Convert.ToString(entity.MobileWaitingListBPJS5);
            #endregion

            #region Reserved Queue Start No
            txtReservedQueueStartNo1.Text = Convert.ToString(entity.ReservedQueueStartNo1);
            txtReservedQueueStartNo1BPJS.Text = Convert.ToString(entity.ReservedQueueStartNoBPJS1);
            txtReservedQueueStartNo2.Text = Convert.ToString(entity.ReservedQueueStartNo2);
            txtReservedQueueStartNo2BPJS.Text = Convert.ToString(entity.ReservedQueueStartNoBPJS2);
            txtReservedQueueStartNo3.Text = Convert.ToString(entity.ReservedQueueStartNo3);
            txtReservedQueueStartNo3BPJS.Text = Convert.ToString(entity.ReservedQueueStartNoBPJS3);
            txtReservedQueueStartNo4.Text = Convert.ToString(entity.ReservedQueueStartNo4);
            txtReservedQueueStartNo4BPJS.Text = Convert.ToString(entity.ReservedQueueStartNoBPJS4);
            txtReservedQueueStartNo5.Text = Convert.ToString(entity.ReservedQueueStartNo5);
            txtReservedQueueStartNo5BPJS.Text = Convert.ToString(entity.ReservedQueueStartNoBPJS5);
            #endregion

            #region Reserved Queue End No
            txtReservedQueueEndNo1.Text = Convert.ToString(entity.ReservedQueueEndNo1);
            txtReservedQueueEndNo1BPJS.Text = Convert.ToString(entity.ReservedQueueEndNoBPJS1);
            txtReservedQueueEndNo2.Text = Convert.ToString(entity.ReservedQueueEndNo2);
            txtReservedQueueEndNo2BPJS.Text = Convert.ToString(entity.ReservedQueueEndNoBPJS2);
            txtReservedQueueEndNo3.Text = Convert.ToString(entity.ReservedQueueEndNo3);
            txtReservedQueueEndNo3BPJS.Text = Convert.ToString(entity.ReservedQueueEndNoBPJS3);
            txtReservedQueueEndNo4.Text = Convert.ToString(entity.ReservedQueueEndNo4);
            txtReservedQueueEndNo4BPJS.Text = Convert.ToString(entity.ReservedQueueEndNoBPJS4);
            txtReservedQueueEndNo5.Text = Convert.ToString(entity.ReservedQueueEndNo5);
            txtReservedQueueEndNo5BPJS.Text = Convert.ToString(entity.ReservedQueueEndNoBPJS5);
            #endregion

        }
        #endregion

        #region OnSaveAddRecord
        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDao entityDao = new ParamedicScheduleDao(ctx);
            try
            {
                ParamedicSchedule entity = new ParamedicSchedule();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;

                entityDao.Insert(entity);

                ctx.CommitTransaction();

                string filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExp).FirstOrDefault();

                #region Is Bridging To Gateway
                if (hdnIsBridgingToGateway.Value == "1")
                {
                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnPhysicianScheduleChanged("CREATE", entityParamedic);
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
                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                            entityAPILog.MessageText = apiResultInfo[2];
                            entityAPILog.Response = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                    }
                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        entityAPILog.Sender = "MEDINFRAS";
                        entityAPILog.Recipient = "QUEUE ENGINE";
                        string apiResult = oService.OnPhysicianScheduleRoutineChanged("001", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                        string[] apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.MessageDateTime = DateTime.Now;
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[2];
                            entityAPILog.Response = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);
                        }
                        else
                        {
                            entityAPILog.MessageDateTime = DateTime.Now;
                            entityAPILog.MessageDateTime = DateTime.Now;
                            entityAPILog.MessageText = apiResultInfo[2];
                            entityAPILog.Response = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                    }
                }

                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                {

                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                    APIMessageLog entityAPILog = new APIMessageLog();
                    string apiResult = oService.OnParamedicScheduleChanged(entity, 0, 0, 0, 0, "001");
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
        #endregion

        #region OnSaveEditRecord
        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDao entityDao = new ParamedicScheduleDao(ctx);
            AppointmentDao entityAppDao = new AppointmentDao(ctx);            
            try
            {
                bool isHasScheduleDate = false;
                vParamedicScheduleDate entityScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'", hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx).FirstOrDefault();
                if (entityScheduleDate != null)
                {
                    isHasScheduleDate = true;
                }

                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
                    #region Check
                    string filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = '{2}'", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), hdnDayNumber.Value);
                    vParamedicSchedule vParamedic = BusinessLayer.GetvParamedicScheduleList(filterExp).FirstOrDefault();

                    String objStart1 = "";
                    String objStart2 = "";
                    String objStart3 = "";
                    String objStart4 = "";
                    String objStart5 = "";

                    String objEnd1 = "";
                    String objEnd2 = "";
                    String objEnd3 = "";
                    String objEnd4 = "";
                    String objEnd5 = "";


                    string filterAppointment = string.Format("StartDate >= '{0}' AND ", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
                    int session = 0;

                    #region 5 sessions
                    if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 != "" && vParamedic.StartTime5 != "")
                    {
                        session = 5;
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;
                        objStart3 = vParamedic.StartTime3;
                        objEnd3 = vParamedic.EndTime3;
                        objStart4 = vParamedic.StartTime4;
                        objEnd4 = vParamedic.EndTime4;
                        objStart5 = vParamedic.StartTime5;
                        objEnd5 = vParamedic.EndTime5;

                        filterAppointment += String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime),'HH:mm') BETWEEN '{4}' AND '{5}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{6}' AND '{7}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{8}' AND '{9}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{10}' AND '{11}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);
                    }
                    #endregion
                    #region 4 sessions
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 != "" && vParamedic.StartTime5 == "")
                    {
                        session = 4;
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;
                        objStart3 = vParamedic.StartTime3;
                        objEnd3 = vParamedic.EndTime3;
                        objStart4 = vParamedic.StartTime4;
                        objEnd4 = vParamedic.EndTime4;

                        filterAppointment += String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime),'HH:mm') BETWEEN '{4}' AND '{5}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{6}' AND '{7}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{8}' AND '{9}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);

                    }
                    #endregion
                    #region 3 sessions
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        session = 3;
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;
                        objStart3 = vParamedic.StartTime3;
                        objEnd3 = vParamedic.EndTime3;

                        filterAppointment += String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime),'HH:mm') BETWEEN '{4}' AND '{5}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{6}' AND '{7}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);

                    }
                    #endregion
                    #region 2 sessions
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 == "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        session = 2;
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;


                        filterAppointment += String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime),'HH:mm') BETWEEN '{4}' AND '{5}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);
                    }
                    #endregion
                    #region 1 session
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 == "" && vParamedic.StartTime3 == "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        session = 1;
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;

                        filterAppointment += String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime),'HH:mm') BETWEEN '{4}' AND '{5}') OR (FORMAT(CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime),'HH:mm') BETWEEN '{4}' AND '{5}')) AND StartDate >= '{6}'", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                    }
                    #endregion
                    #endregion

                    filterAppointment += String.Format(" AND DayNumber = {0}", Convert.ToInt16(hdnDayNumber.Value));
                    if (hdnRoomIDNew.Value != hdnRoomIDOld.Value && hdnOperationalTime.Value == "0")
                    {
                        List<vAppointment> lstvAppointment = BusinessLayer.GetvAppointmentList(filterAppointment, ctx);

                        foreach (vAppointment appointment in lstvAppointment)
                        {
                            Appointment entityAppointment = entityAppDao.Get(appointment.AppointmentID);
                            entityAppointment.RoomID = Convert.ToInt32(hdnRoomIDNew.Value);
                            entityAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityAppDao.Update(entityAppointment);
                        }

                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                        ParamedicSchedule entity = new ParamedicSchedule();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);

                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                        vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {

                            MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                    else if (hdnOperationalTime.Value == "0" && hdnIsChkBPJSChanged.Value == "1")
                    {
                        #region valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                        ParamedicSchedule entity = new ParamedicSchedule();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);

                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                        vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {

                            MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                        #endregion
                    }
                    else if (hdnOperationalTime.Value == "0" && hdnIsRemarksChanged.Value == "1")
                    {
                        #region valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                        ParamedicSchedule entity = new ParamedicSchedule();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);

                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                        vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {

                            MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                        #endregion
                    }
                    else if (hdnOperationalTime.Value == "0" && hdnIsChangeDifferentQueueNo.Value == "1")
                    {
                        #region valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                        ParamedicSchedule entity = new ParamedicSchedule();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);

                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                        vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {

                            MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                        #endregion
                    }
                    else
                    {
                        List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(filterAppointment);

                        if (hdnMaxAppointmentChanged.Value == "1" && hdnOperationalTime.Value == "0")
                        {
                            if (onCheckMaximumAppointment(ctx, lstAppointment, session, ref errMessage, isHasScheduleDate))
                            {
                                #region valid
                                entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                                ParamedicSchedule entity = new ParamedicSchedule();
                                ControlToEntity(entity);
                                entity.CreatedBy = AppSession.UserLogin.UserID;

                                entityDao.Insert(entity);

                                ctx.CommitTransaction();

                                string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                                vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                                #region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        entityAPILog.Sender = "MEDINFRAS";
                                        entityAPILog.Recipient = "QUEUE ENGINE";
                                        string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.MessageDateTime = DateTime.Now;
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                        {
                                            entityAPILog.MessageDateTime = DateTime.Now;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                }

                                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                                {

                                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                                    APIMessageLog entityAPILog = new APIMessageLog();
                                    string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                                #endregion
                            }
                            else
                            {
                                //errMessage = "Maaf, kuota tidak bisa kurang dari appointment yang sudah ada.";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            if (lstAppointment.Count <= 0)
                            {
                                #region valid
                                entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                                ParamedicSchedule entity = new ParamedicSchedule();
                                ControlToEntity(entity);
                                entity.CreatedBy = AppSession.UserLogin.UserID;

                                entityDao.Insert(entity);

                                ctx.CommitTransaction();

                                string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                                vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                                #region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        entityAPILog.Sender = "MEDINFRAS";
                                        entityAPILog.Recipient = "QUEUE ENGINE";
                                        string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.MessageDateTime = DateTime.Now;
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                        {
                                            entityAPILog.MessageDateTime = DateTime.Now;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                        }
                                    }
                                }

                                if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                                {

                                    MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                                    APIMessageLog entityAPILog = new APIMessageLog();
                                    string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
                                #endregion
                            }
                            else
                            {
                                errMessage = "Maaf, di jadwal ini sudah ada perjanjian pasien. Jadwal tidak dapat diubah";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                    }
                }
                else
                {
                    entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                    ParamedicSchedule entity = new ParamedicSchedule();
                    ControlToEntity(entity);
                    //entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                    //entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                    //entity.OperationalTimeID = Convert.ToInt32(hdnOperationalTimeID.Value);
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    entityDao.Insert(entity);

                    ctx.CommitTransaction();

                    string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                    vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpEdit).FirstOrDefault();

                    #region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleChanged("UPDATE", entityParamedic);
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            entityAPILog.Sender = "MEDINFRAS";
                            entityAPILog.Recipient = "QUEUE ENGINE";
                            string apiResult = oService.OnPhysicianScheduleRoutineChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.MessageDateTime = DateTime.Now;
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);
                            }
                            else
                            {
                                entityAPILog.MessageDateTime = DateTime.Now;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                    }

                    if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                    {

                        MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "002");
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
        #endregion

        #region OnDeleteRecord
        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDao entityDao = new ParamedicScheduleDao(ctx);
            try
            {
                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
                    //string filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                    //vParamedicSchedule vParamedic = BusinessLayer.GetvParamedicScheduleList(filterExp).FirstOrDefault();

                    string filterAppointment = String.Format("DATEPART(dw,StartDate)-1 = {0} AND StartDate >= (SELECT FORMAT(GETDATE(),'yyyy-MM-dd')) AND ParamedicID = {1} AND HealthcareServiceUnitId = {2} AND GCAppointmentStatus NOT IN ('{3}','{4}')", Convert.ToInt16(hdnDayNumber.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED);

                    List<vAppointment> lstAppointment = BusinessLayer.GetvAppointmentList(filterAppointment, ctx);

                    if (lstAppointment.Count <= 0)
                    {
                        ParamedicSchedule entity = BusinessLayer.GetParamedicSchedule(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                        string filterExpDelete = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                        vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpDelete, ctx).FirstOrDefault();

                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));

                        ctx.CommitTransaction();

                        //#region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleChanged("DELETE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleRoutineChanged("003", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageDateTime = DateTime.Now;
                                    entityAPILog.MessageText = apiResultInfo[2];
                                    entityAPILog.Response = apiResultInfo[1];
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }

                        if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                        {

                            MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "003");
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

                        //ParamedicSchedule entity = new ParamedicSchedule();
                        //ControlToEntity(entity);
                        //entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        //entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
                        //entity.OperationalTimeID = Convert.ToInt32(hdnOperationalTimeID.Value);
                        //entity.CreatedBy = AppSession.UserLogin.UserID;

                        //entityDao.Insert(entity);
                    }
                    else
                    {
                        errMessage = "Maaf, di jadwal ini sudah ada perjanjian pasien. Jadwal tidak dapat diubah";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    ParamedicSchedule entity = BusinessLayer.GetParamedicSchedule(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));
                    string filterExpDelete = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND DayNumber = {2}", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber);
                    vParamedicSchedule1 entityParamedic = BusinessLayer.GetvParamedicSchedule1List(filterExpDelete).FirstOrDefault();

                    entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt16(hdnDayNumber.Value));

                    ctx.CommitTransaction();

                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD) // RUMAH SAKIT MEDISTRA 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleChanged("DELETE", entityParamedic);
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA) // RUMAH SAKIT DR. OEN KANDANG SAPI SURAKARTA 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianRoutineSchedule("1", entityParamedic);
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
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA) // RUMAH SAKIT DR. OEN SOLO BARU 
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            entityAPILog.Sender = "MEDINFRAS";
                            entityAPILog.Recipient = "QUEUE ENGINE";
                            string apiResult = oService.OnPhysicianScheduleRoutineChanged("003", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.DayNumber, entity);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.MessageDateTime = DateTime.Now;
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);
                            }
                            else
                            {
                                entityAPILog.MessageDateTime = DateTime.Now;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }

                    }

                    if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                    {

                        MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnParamedicScheduleChanged(entity, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), Convert.ToInt32(hdnDayNumber.Value), "003");
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
        #endregion

        private bool onCheckMaximumAppointment(IDbContext ctx, List<vAppointment> lstAppointment, int session, ref string errMessage, bool isHasScheduleDate)
        {
            bool result = true;

            int maxAppointment1 = 0;
            int maxAppointment2 = 0;
            int maxAppointment3 = 0;
            int maxAppointment4 = 0;
            int maxAppointment5 = 0;
            int maxAppointmentBPJS1 = 0;
            int maxAppointmentBPJS2 = 0;
            int maxAppointmentBPJS3 = 0;
            int maxAppointmentBPJS4 = 0;
            int maxAppointmentBPJS5 = 0;

            if (!string.IsNullOrEmpty(txtMaximumAppointment1.Text))
                maxAppointment1 = Convert.ToInt32(txtMaximumAppointment1.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment2.Text))
                maxAppointment2 = Convert.ToInt32(txtMaximumAppointment2.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment3.Text))
                maxAppointment3 = Convert.ToInt32(txtMaximumAppointment3.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment4.Text))
                maxAppointment4 = Convert.ToInt32(txtMaximumAppointment4.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment5.Text))
                maxAppointment5 = Convert.ToInt32(txtMaximumAppointment5.Text);

            if (!string.IsNullOrEmpty(txtMaximumAppointment1BPJS.Text))
                maxAppointmentBPJS1 = Convert.ToInt32(txtMaximumAppointment1BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment2BPJS.Text))
                maxAppointmentBPJS2 = Convert.ToInt32(txtMaximumAppointment2BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment3BPJS.Text))
                maxAppointmentBPJS3 = Convert.ToInt32(txtMaximumAppointment3BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment4BPJS.Text))
                maxAppointmentBPJS4 = Convert.ToInt32(txtMaximumAppointment4BPJS.Text);
            if (!string.IsNullOrEmpty(txtMaximumAppointment5BPJS.Text))
                maxAppointmentBPJS5 = Convert.ToInt32(txtMaximumAppointment5BPJS.Text);

            int[] lstMaxApm = new int[6];
            lstMaxApm[0] = 0;
            lstMaxApm[1] = maxAppointment1 + maxAppointmentBPJS1;
            lstMaxApm[2] = maxAppointment2 + maxAppointmentBPJS2;
            lstMaxApm[3] = maxAppointment3 + maxAppointmentBPJS3;
            lstMaxApm[4] = maxAppointment4 + maxAppointmentBPJS4;
            lstMaxApm[5] = maxAppointment5 + maxAppointmentBPJS5; 

            string[] lstDay = new string[8];
            lstDay[0] = "";
            lstDay[1] = "Senin";
            lstDay[2] = "Selasa";
            lstDay[3] = "Rabu";
            lstDay[4] = "Kamis";
            lstDay[5] = "Jumat";
            lstDay[6] = "Sabtu";
            lstDay[7] = "Minggu";

            List<vAppointment> filterApm = null;
            if (session > 0)
            {
                if (lstAppointment.Count != 0)
                {
                    for (int i = 1; i <= session; i++)
                    {
                        filterApm = new List<vAppointment>();
                        if (!isHasScheduleDate)
                        {
                            filterApm = lstAppointment.Where(w => w.Session == i && w.StartDate.ToString(Constant.FormatString.DATE_FORMAT_112) == DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112)).ToList();
                            if (lstMaxApm[i] > 0)
                            {
                                if (lstMaxApm[i] < filterApm.Count)
                                {
                                    result = false;
                                    errMessage = string.Format("Kuota di sesi <b>{0}</b> tidak bisa kurang dari jumlah perjanjian pasien (Sudah ada <b>{1}</b> perjanjian yang valid)", i, filterApm.Count);
                                }
                            }
                        }
                        else
                        {
                            filterApm = lstAppointment.Where(w => w.Session == i && w.StartDate > DateTime.Now && w.DayNumber == Convert.ToInt32(hdnDayNumber.Value)).ToList();
                            if (lstMaxApm[i] > 0)
                            {
                                if (lstMaxApm[i] < filterApm.Count)
                                {
                                    result = false;
                                    errMessage = string.Format("Kuota di sesi <b>{0}</b> tidak bisa kurang dari jumlah perjanjian pasien (Sudah ada <b>{1}</b> perjanjian yang valid di hari <b>{2}</b>)", i, filterApm.Count, lstDay[Convert.ToUInt32(hdnDayNumber.Value)]);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                result = false;
            }
            return result;
        }
    }
}