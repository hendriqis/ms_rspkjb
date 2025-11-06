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
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.Service;


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PSScheduleWithDateEntryCtl : BaseViewPopupCtl
    {

        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnParamedicID.Value = par[0];
            hdnHealthcareServiceUnitID.Value = par[1];

            vServiceUnitParamedic entity = BusinessLayer.GetvServiceUnitParamedicList(string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value))[0];
            txtParamedicName.Text = entity.ParamedicName;
            txtHealthcareName.Text = entity.HealthcareName;
            txtServiceUnit.Text = entity.ServiceUnitName;

            //SetControlProperties();

            txtScheduleDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            Helper.SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtOperationalTimeCodeCtl, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, false), "mpEntryPopup");
            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}', '{3}', '{4}','{5}')", 
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
            String filterExpression = string.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvParamedicScheduleDateRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vParamedicScheduleDate> lstEntity = BusinessLayer.GetvParamedicScheduleDateList(filterExpression, 8, pageIndex, "ScheduleDate ASC");
            grdView.DataSource = lstEntity;
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
                    //                    if (hdnHealthcareServiceUnitID.Value != "" && hdnParamedicID.Value != "" && hdnScheduleDate.Value != "")
                    if (hdnIsAdd.Value == "0")
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
        private void ControlToEntity(ParamedicScheduleDate entity)
        {
            entity.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entity.ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            entity.ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);

            if (hdnRoomID.Value != "")
            {
                entity.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }
            else
            {
                entity.RoomID = null;
            }

            //entity.IsAllowDifferentQueueNo = 

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

            #region Mobile App Percentage
            if (!string.IsNullOrEmpty(txtMobileAppointment1.Text))
            {
                entity.MobileAppointment1 = Convert.ToByte(txtMobileAppointment1.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment2.Text))
            {
                entity.MobileAppointment2 = Convert.ToByte(txtMobileAppointment2.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment3.Text))
            {
                entity.MobileAppointment3 = Convert.ToByte(txtMobileAppointment3.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment4.Text))
            {
                entity.MobileAppointment4 = Convert.ToByte(txtMobileAppointment4.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment5.Text))
            {
                entity.MobileAppointment5 = Convert.ToByte(txtMobileAppointment5.Text);
            }

            if (!string.IsNullOrEmpty(txtMobileAppointment1BPJS.Text))
            {
                entity.MobileAppointmentBPJS1 = Convert.ToByte(txtMobileAppointment1BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment2BPJS.Text))
            {
                entity.MobileAppointmentBPJS2 = Convert.ToByte(txtMobileAppointment2BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment3BPJS.Text))
            {
                entity.MobileAppointmentBPJS3 = Convert.ToByte(txtMobileAppointment3BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment4BPJS.Text))
            {
                entity.MobileAppointmentBPJS4 = Convert.ToByte(txtMobileAppointment4BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileAppointment5BPJS.Text))
            {
                entity.MobileAppointmentBPJS5 = Convert.ToByte(txtMobileAppointment5BPJS.Text);
            }
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

            #region Reserved Queue Start No
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo1.Text))
            {
                entity.ReservedQueueStartNo1 = Convert.ToByte(txtReservedQueueStartNo1.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo2.Text))
            {
                entity.ReservedQueueStartNo2 = Convert.ToByte(txtReservedQueueStartNo2.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo3.Text))
            {
                entity.ReservedQueueStartNo3 = Convert.ToByte(txtReservedQueueStartNo3.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo4.Text))
            {
                entity.ReservedQueueStartNo4 = Convert.ToByte(txtReservedQueueStartNo4.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo5.Text))
            {
                entity.ReservedQueueStartNo5 = Convert.ToByte(txtReservedQueueStartNo5.Text);
            }

            if (!string.IsNullOrEmpty(txtReservedQueueStartNo1BPJS.Text))
            {
                entity.ReservedQueueStartNoBPJS1 = Convert.ToByte(txtReservedQueueStartNo1BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo2BPJS.Text))
            {
                entity.ReservedQueueStartNoBPJS2 = Convert.ToByte(txtReservedQueueStartNo2BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo3BPJS.Text))
            {
                entity.ReservedQueueStartNoBPJS3 = Convert.ToByte(txtReservedQueueStartNo3BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo4BPJS.Text))
            {
                entity.ReservedQueueStartNoBPJS4 = Convert.ToByte(txtReservedQueueStartNo4BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueStartNo5BPJS.Text))
            {
                entity.ReservedQueueStartNoBPJS5 = Convert.ToByte(txtReservedQueueStartNo5BPJS.Text);
            }
            #endregion

            #region Reserved Queue End No
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo1.Text))
            {
                entity.ReservedQueueEndNo1 = Convert.ToByte(txtReservedQueueEndNo1.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo2.Text))
            {
                entity.ReservedQueueEndNo2 = Convert.ToByte(txtReservedQueueEndNo2.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo3.Text))
            {
                entity.ReservedQueueEndNo3 = Convert.ToByte(txtReservedQueueEndNo3.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo4.Text))
            {
                entity.ReservedQueueEndNo4 = Convert.ToByte(txtReservedQueueEndNo4.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo5.Text))
            {
                entity.ReservedQueueEndNo5 = Convert.ToByte(txtReservedQueueEndNo5.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo1BPJS.Text))
            {
                entity.ReservedQueueEndNoBPJS1 = Convert.ToByte(txtReservedQueueEndNo1BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo2BPJS.Text))
            {
                entity.ReservedQueueEndNoBPJS2 = Convert.ToByte(txtReservedQueueEndNo2BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo3BPJS.Text))
            {
                entity.ReservedQueueEndNoBPJS3 = Convert.ToByte(txtReservedQueueEndNo3BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo4BPJS.Text))
            {
                entity.ReservedQueueEndNoBPJS4 = Convert.ToByte(txtReservedQueueEndNo4BPJS.Text);
            }
            if (!string.IsNullOrEmpty(txtReservedQueueEndNo5BPJS.Text))
            {
                entity.ReservedQueueEndNoBPJS5 = Convert.ToByte(txtReservedQueueEndNo5BPJS.Text);
            }
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
        private void EntityToControl(vParamedicScheduleDate entity)
        {
            txtOperationalTimeCodeCtl.Text = entity.OperationalTimeCode;
            txtOperationalTimeNameCtl.Text = entity.OperationalTimeName;

            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtRemarks.Text = entity.Remarks;

            #region StartTime
            txtStart1.Text = entity.StartTime1;
            txtStart2.Text = entity.StartTime2;
            txtStart3.Text = entity.StartTime3;
            txtStart4.Text = entity.StartTime4;
            txtStart5.Text = entity.StartTime5;
            #endregion

            #region StartEnd
            txtEnd1.Text = entity.EndTime1;
            txtEnd2.Text = entity.EndTime2;
            txtEnd3.Text = entity.EndTime3;
            txtEnd4.Text = entity.EndTime4;
            txtEnd5.Text = entity.EndTime5;
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

            #region MaximumAppointment
            txtMaximumAppointment1.Text = Convert.ToString(entity.MaximumAppointment1);
            txtMaximumAppointment1BPJS.Text = Convert.ToString(entity.MaximumAppointmentBPJS1);
            txtMaximumAppointment2.Text = Convert.ToString(entity.MaximumAppointment2);
            txtMaximumAppointment2BPJS.Text = Convert.ToString(entity.MaximumAppointmentBPJS2);
            txtMaximumAppointment3.Text = Convert.ToString(entity.MaximumAppointment3);
            txtMaximumAppointment3BPJS.Text = Convert.ToString(entity.MaximumAppointmentBPJS3);
            txtMaximumAppointment4.Text = Convert.ToString(entity.MaximumAppointment4);
            txtMaximumAppointment4BPJS.Text = Convert.ToString(entity.MaximumAppointmentBPJS4);
            txtMaximumAppointment5.Text = Convert.ToString(entity.MaximumAppointment5);
            txtMaximumAppointment5BPJS.Text = Convert.ToString(entity.MaximumAppointmentBPJS5);
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

            #region Is BPJS Appointment
            chkIsBPJS1.Checked = entity.IsBPJS1;
            chkIsBPJS2.Checked = entity.IsBPJS2;
            chkIsBPJS3.Checked = entity.IsBPJS3;
            chkIsBPJS4.Checked = entity.IsBPJS4;
            chkIsBPJS5.Checked = entity.IsBPJS5;
            #endregion

            #region Is Non BPJS Appointment
            chkIsNonBPJS1.Checked = entity.IsNonBPJS1;
            chkIsNonBPJS2.Checked = entity.IsNonBPJS2;
            chkIsNonBPJS3.Checked = entity.IsNonBPJS3;
            chkIsNonBPJS4.Checked = entity.IsNonBPJS4;
            chkIsNonBPJS5.Checked = entity.IsNonBPJS5;
            #endregion

        }
        #endregion

        #region Save Add Record
        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDateDao entityDao = new ParamedicScheduleDateDao(ctx);
            try
            {
                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
                    DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
                    int daynumber = (int)ScheduleDate.DayOfWeek;

                    if (daynumber == 0)
                    {
                        daynumber = 7;
                    }

                    String filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1}", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value));
                    vParamedicSchedule ParamedicSchedule = BusinessLayer.GetvParamedicScheduleList(filterExp, ctx).FirstOrDefault();

                    string filterAppointment = "";

                    DateTime objStart1 = new DateTime();
                    DateTime objStart2 = new DateTime();
                    DateTime objStart3 = new DateTime();
                    DateTime objStart4 = new DateTime();
                    DateTime objStart5 = new DateTime();

                    DateTime objEnd1 = new DateTime();
                    DateTime objEnd2 = new DateTime();
                    DateTime objEnd3 = new DateTime();
                    DateTime objEnd4 = new DateTime();
                    DateTime objEnd5 = new DateTime();

                    DateTime objtxtStart1 = new DateTime();
                    DateTime objtxtStart2 = new DateTime();
                    DateTime objtxtStart3 = new DateTime();
                    DateTime objtxtStart4 = new DateTime();
                    DateTime objtxtStart5 = new DateTime();

                    DateTime objtxtEnd1 = new DateTime();
                    DateTime objtxtEnd2 = new DateTime();
                    DateTime objtxtEnd3 = new DateTime();
                    DateTime objtxtEnd4 = new DateTime();
                    DateTime objtxtEnd5 = new DateTime();

                    //Boolean IsWaitingList1 = false;
                    //Boolean IsWaitingList2 = false;
                    //Boolean IsWaitingList3 = false;
                    //Boolean IsWaitingList4 = false;
                    //Boolean IsWaitingList5 = false;

                    List<Appointment> lstMaster = new List<Appointment>();
                    List<Appointment> lstAppointmentNow = new List<Appointment>();

                    #region ParamedicSchedule
                    if (ParamedicSchedule != null)
                    {
                        if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 != "")
                        {
                            #region set StartDateTime And EndDateTime Master ParamedicSchedule
                            objStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime1);
                            objEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime1);
                            objStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime2);
                            objEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime2);
                            objStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime3);
                            objEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime3);
                            objStart4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime4);
                            objEnd4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime4);
                            objStart5 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime5);
                            objEnd5 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime5);

                            //IsWaitingList1 = ParamedicSchedule.IsAllowWaitingList1;
                            //IsWaitingList2 = ParamedicSchedule.IsAllowWaitingList2;
                            //IsWaitingList3 = ParamedicSchedule.IsAllowWaitingList3;
                            //IsWaitingList4 = ParamedicSchedule.IsAllowWaitingList4;
                            //IsWaitingList5 = ParamedicSchedule.IsAllowWaitingList5;
                            #endregion

                            #region Take Data
                            //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 != false && IsWaitingList5 != false)
                            //{
                            //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{12}' AND '{13}' AND IsWaitingList = 1))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);
                            //}
                            //else
                            //{
                            filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);
                            //}

                            List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                            if (lstAppointment.Count > 0)
                            {
                                lstMaster.AddRange(lstAppointment);
                            }

                            #endregion
                        }
                        else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 != "" && ParamedicSchedule.StartTime5 == "")
                        {
                            #region set StartDateTime And EndDateTime Master ParamedicSchedule
                            objStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime1);
                            objEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime1);
                            objStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime2);
                            objEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime2);
                            objStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime3);
                            objEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime3);
                            objStart4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime4);
                            objEnd4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime4);

                            //IsWaitingList1 = ParamedicSchedule.IsAllowWaitingList1;
                            //IsWaitingList2 = ParamedicSchedule.IsAllowWaitingList2;
                            //IsWaitingList3 = ParamedicSchedule.IsAllowWaitingList3;
                            //IsWaitingList4 = ParamedicSchedule.IsAllowWaitingList4;
                            #endregion

                            #region Take Data
                            //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 != false && IsWaitingList5 == false)
                            //{
                            //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);
                            //}
                            //else
                            //{
                            filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);
                            //}

                            List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                            if (lstAppointment.Count > 0)
                            {
                                lstMaster.AddRange(lstAppointment);
                            }

                            #endregion
                        }
                        else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 != "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                        {
                            #region set StartDateTime And EndDateTime Master ParamedicSchedule
                            objStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime1);
                            objEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime1);
                            objStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime2);
                            objEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime2);
                            objStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime3);
                            objEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime3);

                            //IsWaitingList1 = ParamedicSchedule.IsAllowWaitingList1;
                            //IsWaitingList2 = ParamedicSchedule.IsAllowWaitingList2;
                            //IsWaitingList3 = ParamedicSchedule.IsAllowWaitingList3;
                            #endregion

                            #region Take Data
                            //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 == false && IsWaitingList5 == false)
                            //{
                            //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);
                            //}
                            //else
                            //{
                            filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);
                            //}

                            List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                            if (lstAppointment.Count > 0)
                            {
                                lstMaster.AddRange(lstAppointment);
                            }
                            #endregion
                        }
                        else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 != "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                        {
                            #region set StartDateTime And EndDateTime Master ParamedicSchedule
                            objStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime1);
                            objEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime1);
                            objStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime2);
                            objEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime2);

                            //IsWaitingList1 = ParamedicSchedule.IsAllowWaitingList1;
                            //IsWaitingList2 = ParamedicSchedule.IsAllowWaitingList2;
                            #endregion

                            #region Take Data
                            //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 == false && IsWaitingList4 == false && IsWaitingList5 == false)
                            //{
                            //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}'))  AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);
                            //}
                            //else
                            //{
                            filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);
                            //}

                            List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                            if (lstAppointment.Count > 0)
                            {
                                lstMaster.AddRange(lstAppointment);
                            }
                            #endregion
                        }
                        else if (ParamedicSchedule.StartTime1 != "" && ParamedicSchedule.StartTime2 == "" && ParamedicSchedule.StartTime3 == "" && ParamedicSchedule.StartTime4 == "" && ParamedicSchedule.StartTime5 == "")
                        {
                            #region set StartDateTime And EndDateTime Master ParamedicSchedule
                            objStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.StartTime1);
                            objEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicSchedule.EndTime1);

                            //IsWaitingList1 = ParamedicSchedule.IsAllowWaitingList1;
                            #endregion

                            #region Take Data
                            //if (IsWaitingList1 != false && IsWaitingList2 == false && IsWaitingList3 == false && IsWaitingList4 == false && IsWaitingList5 == false)
                            //{
                            //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{4}' AND '{5}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);
                            //}
                            //else
                            //{
                            filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{4}' AND '{5}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);
                            //}

                            List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                            if (lstAppointment.Count > 0)
                            {
                                lstMaster.AddRange(lstAppointment);
                            }
                            #endregion
                        }
                    }

                    #endregion

                    #region ParamedicScheduleNew

                    if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text != "" && txtStart5.Text != "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objtxtStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart1.Text);
                        objtxtEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd1.Text);
                        objtxtStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart2.Text);
                        objtxtEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd2.Text);
                        objtxtStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart3.Text);
                        objtxtEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd3.Text);
                        objtxtStart4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart4.Text);
                        objtxtEnd4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd4.Text);
                        objtxtStart5 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart5.Text);
                        objtxtEnd5 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd5.Text);

                        //IsWaitingList1 = chkIsAllowWaitingList1.Checked;
                        //IsWaitingList2 = chkIsAllowWaitingList2.Checked;
                        //IsWaitingList3 = chkIsAllowWaitingList3.Checked;
                        //IsWaitingList4 = chkIsAllowWaitingList4.Checked;
                        //IsWaitingList5 = chkIsAllowWaitingList5.Checked;
                        #endregion

                        #region Take Data
                        //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 != false && IsWaitingList5 != false)
                        //{
                        //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{12}' AND '{13}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3, objtxtStart4, objtxtEnd4, objtxtStart5, objtxtEnd5);
                        //}
                        //else
                        //{
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3, objtxtStart4, objtxtEnd4, objtxtStart5, objtxtEnd5);
                        //}

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstAppointmentNow.AddRange(lstAppointment);
                        }

                        #endregion
                    }
                    else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text != "" && txtStart5.Text == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objtxtStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart1.Text);
                        objtxtEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd1.Text);
                        objtxtStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart2.Text);
                        objtxtEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd2.Text);
                        objtxtStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart3.Text);
                        objtxtEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd3.Text);
                        objtxtStart4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart4.Text);
                        objtxtEnd4 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd4.Text);

                        //IsWaitingList1 = chkIsAllowWaitingList1.Checked;
                        //IsWaitingList2 = chkIsAllowWaitingList2.Checked;
                        //IsWaitingList3 = chkIsAllowWaitingList3.Checked;
                        //IsWaitingList4 = chkIsAllowWaitingList4.Checked;
                        #endregion

                        #region Take Data
                        //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 != false && IsWaitingList5 == false)
                        //{
                        //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3, objtxtStart4, objtxtEnd4);
                        //}
                        //else
                        //{
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3, objtxtStart4, objtxtEnd4);
                        //}

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstAppointmentNow.AddRange(lstAppointment);
                        }

                        #endregion
                    }
                    else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text != "" && txtStart4.Text == "" && txtStart5.Text == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objtxtStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart1.Text);
                        objtxtEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd1.Text);
                        objtxtStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart2.Text);
                        objtxtEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd2.Text);
                        objtxtStart3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart3.Text);
                        objtxtEnd3 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd3.Text);

                        //IsWaitingList1 = chkIsAllowWaitingList1.Checked;
                        //IsWaitingList2 = chkIsAllowWaitingList2.Checked;
                        //IsWaitingList3 = chkIsAllowWaitingList3.Checked;
                        #endregion

                        #region Take Data
                        //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 != false && IsWaitingList4 == false && IsWaitingList5 == false)
                        //{
                        //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3);
                        //}
                        //else
                        //{
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2, objtxtStart3, objtxtEnd3);
                        //}

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstAppointmentNow.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (txtStart1.Text != "" && txtStart2.Text != "" && txtStart3.Text == "" && txtStart4.Text == "" && txtStart5.Text == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objtxtStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart1.Text);
                        objtxtEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd1.Text);
                        objtxtStart2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart2.Text);
                        objtxtEnd2 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd2.Text);

                        //IsWaitingList1 = chkIsAllowWaitingList1.Checked;
                        //IsWaitingList2 = chkIsAllowWaitingList2.Checked;
                        #endregion

                        #region Take Data
                        //if (IsWaitingList1 != false && IsWaitingList2 != false && IsWaitingList3 == false && IsWaitingList4 == false && IsWaitingList5 == false)
                        //{
                        //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2);
                        //}
                        //else
                        //{
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1, objtxtStart2, objtxtEnd2);
                        //}

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        if (lstAppointment.Count > 0)
                        {
                            lstAppointmentNow.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (txtStart1.Text != "" && txtStart2.Text == "" && txtStart3.Text == "" && txtStart4.Text == "" && txtStart5.Text == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objtxtStart1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtStart1.Text);
                        objtxtEnd1 = DateTime.Parse(ScheduleDate.ToString("yyyy-MM-dd") + " " + txtEnd1.Text);

                        //IsWaitingList1 = chkIsAllowWaitingList1.Checked;
                        #endregion

                        #region Take Data
                        //if (IsWaitingList1 != false && IsWaitingList2 == false && IsWaitingList3 == false && IsWaitingList4 == false && IsWaitingList5 == false)
                        //{
                        //    filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{4}' AND '{5}'))AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1);
                        //}
                        //else
                        //{
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{4}' AND '{5}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objtxtStart1, objtxtEnd1);
                        //}

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        if (lstAppointment.Count > 0)
                        {
                            lstAppointmentNow.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    #endregion

                    if (lstMaster.Count == lstAppointmentNow.Count)
                    {
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);

                        ctx.CommitTransaction();

                        string filterExpCreate = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} ORDER BY ScheduleDate DESC", entity.ParamedicID, entity.HealthcareServiceUnitID);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpCreate).FirstOrDefault();

                        //#region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("CREATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("2", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("001", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, 0, 0, 0, txtScheduleDate.Text, "001");
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
                        errMessage = "Maaf, di jadwal ini sudah ada perjanjian pasien. Jadwal tidak dapat diubah";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    ParamedicScheduleDate entity = new ParamedicScheduleDate();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;

                    entityDao.Insert(entity);

                    ctx.CommitTransaction();

                    string filterExpCreate = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} ORDER BY ScheduleDate DESC", entity.ParamedicID, entity.HealthcareServiceUnitID);
                    vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpCreate).FirstOrDefault();

                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleWithDateChanged("CREATE", entityParamedic);
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleWithDate("2", entityParamedic);
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            entityAPILog.Sender = "MEDINFRAS";
                            entityAPILog.Recipient = "QUEUE ENGINE";
                            string apiResult = oService.OnPhysicianScheduleByDateChanged("001", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                        string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, 0, 0, 0, txtScheduleDate.Text, "001");
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

        #region Save Edit Record
        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDateDao entityDao = new ParamedicScheduleDateDao(ctx);

            bool isHasScheduleDate = false;
            vParamedicScheduleDate entityScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(String.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND ScheduleDate = '{2}'", hdnHealthcareServiceUnitID.Value, hdnParamedicID.Value, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx).FirstOrDefault();
            if (entityScheduleDate != null)
            {
                isHasScheduleDate = true;
            }

            try
            {
                List<Appointment> lstMaster = new List<Appointment>();
                List<Appointment> lstMasterWaiting = new List<Appointment>();
                List<Appointment> lstAppointmentNow = new List<Appointment>();

                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
                    DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
                    int daynumber = (int)ScheduleDate.DayOfWeek;

                    if (daynumber == 0)
                    {
                        daynumber = 7;
                    }

                    string filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                    vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(filterExp, ctx).FirstOrDefault();

                    DateTime objStart1 = new DateTime();
                    DateTime objStart2 = new DateTime();
                    DateTime objStart3 = new DateTime();
                    DateTime objStart4 = new DateTime();
                    DateTime objStart5 = new DateTime();

                    DateTime objEnd1 = new DateTime();
                    DateTime objEnd2 = new DateTime();
                    DateTime objEnd3 = new DateTime();
                    DateTime objEnd4 = new DateTime();
                    DateTime objEnd5 = new DateTime();

                    string filterAppointment = "";
                    int session = 0;

                    #region ParamedicScheduleDate
                    if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                    {
                        session = 5;
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1);
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1);
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2);
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2);
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3);
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3);
                        objStart4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime4);
                        objEnd4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime4);
                        objStart5 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime5);
                        objEnd5 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime5);
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);

                        //filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{12}' AND '{13}')) AND IsWaitingList = 0", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);
                        //filterAppointmentWaiting = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{12}' AND '{13}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        //List<Appointment> lstAppointmentWaiting = BusinessLayer.GetAppointmentList(filterAppointmentWaiting);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }

                        //if (lstAppointmentWaiting.Count > 0)
                        //{
                        //    lstMasterWaiting.AddRange(lstAppointmentWaiting);
                        //}
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        session = 4;
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1);
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1);
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2);
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2);
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3);
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3);
                        objStart4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime4);
                        objEnd4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime4);
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);

                        //filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}')) AND IsWaitingList = 0", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);
                        //filterAppointmentWaiting = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        //List<Appointment> lstAppointmentWaiting = BusinessLayer.GetAppointmentList(filterAppointmentWaiting);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }

                        //if (lstAppointmentWaiting.Count > 0)
                        //{
                        //    lstMasterWaiting.AddRange(lstAppointmentWaiting);
                        //}
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        session = 3;
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1);
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1);
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2);
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2);
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3);
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3);
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);

                        //filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}')) AND IsWaitingList = 0", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);
                        //filterAppointmentWaiting = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        //List<Appointment> lstAppointmentWaiting = BusinessLayer.GetAppointmentList(filterAppointmentWaiting);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }

                        //if (lstAppointmentWaiting.Count > 0)
                        //{
                        //    lstMasterWaiting.AddRange(lstAppointmentWaiting);
                        //}
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        session = 2;
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1);
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1);
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2);
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2);
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);

                        //filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}')) AND IsWaitingList = 0", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);
                        //filterAppointmentWaiting = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        //List<Appointment> lstAppointmentWaiting = BusinessLayer.GetAppointmentList(filterAppointmentWaiting);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }

                        //if (lstAppointmentWaiting.Count > 0)
                        //{
                        //    lstMasterWaiting.AddRange(lstAppointmentWaiting);
                        //}
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        session = 1;
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1);
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1);
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{4}' AND '{5}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);

                        //filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{4}' AND '{5}')) AND IsWaitingList = 0", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);
                        //filterAppointmentWaiting = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{4}' AND '{5}')) AND IsWaitingList = 1", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);
                        //List<Appointment> lstAppointmentWaiting = BusinessLayer.GetAppointmentList(filterAppointmentWaiting);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }

                        //if (lstAppointmentWaiting.Count > 0)
                        //{
                        //    lstMasterWaiting.AddRange(lstAppointmentWaiting);
                        //}
                        #endregion
                    }
                    #endregion

                    if (hdnRoomIDNew.Value != hdnRoomIDOld.Value && hdnOperationalTime.Value == "0")
                    {
                        #region Valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                    else if (hdnOperationalTime.Value == "0" && hdnIsChkBPJSChanged.Value == "1")
                    {
                        #region Valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                        #region Valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                        #region Valid
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                                #region Valid
                                entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                                ParamedicScheduleDate entity = new ParamedicScheduleDate();
                                ControlToEntity(entity);
                                entity.CreatedBy = AppSession.UserLogin.UserID;

                                entityDao.Insert(entity);
                                ctx.CommitTransaction();

                                string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                                vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                                #region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        entityAPILog.Sender = "MEDINFRAS";
                                        entityAPILog.Recipient = "QUEUE ENGINE";
                                        string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                                    string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                                #region Valid
                                entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                                ParamedicScheduleDate entity = new ParamedicScheduleDate();
                                ControlToEntity(entity);
                                entity.CreatedBy = AppSession.UserLogin.UserID;

                                entityDao.Insert(entity);
                                ctx.CommitTransaction();

                                string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                                vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                                #region Is Brigding To Gateway
                                if (hdnIsBridgingToGateway.Value == "1")
                                {
                                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                                    else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                                    {
                                        GatewayService oService = new GatewayService();
                                        APIMessageLog entityAPILog = new APIMessageLog();
                                        entityAPILog.Sender = "MEDINFRAS";
                                        entityAPILog.Recipient = "QUEUE ENGINE";
                                        string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                                    string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                    int Maximum1 = Convert.ToInt16(txtMaximumAppointment1.Text);
                    int Maximum2 = Convert.ToInt16(txtMaximumAppointment2.Text);
                    int Maximum3 = Convert.ToInt16(txtMaximumAppointment3.Text);
                    int Maximum4 = Convert.ToInt16(txtMaximumAppointment4.Text);
                    int Maximum5 = Convert.ToInt16(txtMaximumAppointment5.Text);

                    int Waiting1 = Convert.ToInt16(txtMaximumWaitingList1.Text);
                    int Waiting2 = Convert.ToInt16(txtMaximumWaitingList2.Text);
                    int Waiting3 = Convert.ToInt16(txtMaximumWaitingList3.Text);
                    int Waiting4 = Convert.ToInt16(txtMaximumWaitingList4.Text);
                    int Waiting5 = Convert.ToInt16(txtMaximumWaitingList5.Text);

                    int SumMaximum = (Maximum1 + Maximum2 + Maximum3 + Maximum4 + Maximum5);
                    int SumWaiting = (Waiting1 + Waiting2 + Waiting3 + Waiting4 + Waiting5);

                    if (SumMaximum >= lstMaster.Count && SumWaiting >= lstMasterWaiting.Count)
                    {
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Helper.GetDatePickerValue(hdnBeforeDatePicker.Value));
                        ParamedicScheduleDate entity = new ParamedicScheduleDate();
                        ControlToEntity(entity);
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        entityDao.Insert(entity);
                        ctx.CommitTransaction();

                        string filterExpEdit = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate);
                        vParamedicScheduleDate1 entityParamedic = BusinessLayer.GetvParamedicScheduleDate1List(filterExpEdit).FirstOrDefault();

                        #region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("UPDATE", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("1", entityParamedic);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("002", entity.ParamedicID, entity.HealthcareServiceUnitID, entity.ScheduleDate.ToString(Constant.FormatString.DATE_FORMAT_112), null);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(entityParamedic, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "002");
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
                    else
                    {
                        errMessage = "Maaf, Jumlah Maximum Appointment dan Waiting List kurang dari jumlah perjanjian yang sudah ada";
                        result = false;
                        ctx.RollBackTransaction();
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

        #region OnDeleteRecord
        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ParamedicScheduleDateDao entityDao = new ParamedicScheduleDateDao(ctx);
            try
            {
                string filterExp = String.Format("ParamedicID = {0} AND HealthcareServiceUnitID = {1} AND ScheduleDate = '{2}'", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
                vParamedicScheduleDate ParamedicScheduleDate = BusinessLayer.GetvParamedicScheduleDateList(filterExp, ctx).FirstOrDefault();
                //utk Bridging
                vParamedicScheduleDate1 ParamedicScheduleDate1 = BusinessLayer.GetvParamedicScheduleDate1List(filterExp, ctx).FirstOrDefault();
                vParamedicScheduleDate1 entityOldSchedule = new vParamedicScheduleDate1();
                if (ParamedicScheduleDate != null)
                {
                    entityOldSchedule = ParamedicScheduleDate1;
                }

                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
                    DateTime ScheduleDate = Helper.GetDatePickerValue(txtScheduleDate.Text);
                    int daynumber = (int)ScheduleDate.DayOfWeek;

                    if (daynumber == 0)
                    {
                        daynumber = 7;
                    }

                    DateTime objStart1 = new DateTime();
                    DateTime objStart2 = new DateTime();
                    DateTime objStart3 = new DateTime();
                    DateTime objStart4 = new DateTime();
                    DateTime objStart5 = new DateTime();

                    DateTime objEnd1 = new DateTime();
                    DateTime objEnd2 = new DateTime();
                    DateTime objEnd3 = new DateTime();
                    DateTime objEnd4 = new DateTime();
                    DateTime objEnd5 = new DateTime();


                    List<Appointment> lstMaster = new List<Appointment>();
                    List<Appointment> lstAppointmentNow = new List<Appointment>();

                    string filterAppointment = "";

                    #region ParamedicScheduleDate
                    if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 != "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1 + ":00.000");
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1 + ":00.000");
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2 + ":00.000");
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2 + ":00.000");
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3 + ":00.000");
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3 + ":00.000");
                        objStart4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime4 + ":00.000");
                        objEnd4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime4 + ":00.000");
                        objStart5 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime5 + ":00.000");
                        objEnd5 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime5 + ":00.000");
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 != "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1 + ":00.000");
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1 + ":00.000");
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2 + ":00.000");
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2 + ":00.000");
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3 + ":00.000");
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3 + ":00.000");
                        objStart4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime4 + ":00.000");
                        objEnd4 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime4 + ":00.000");
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 != "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1 + ":00.000");
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1 + ":00.000");
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2 + ":00.000");
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2 + ":00.000");
                        objStart3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime3 + ":00.000");
                        objEnd3 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime3 + ":00.000");
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 != "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1 + ":00.000");
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1 + ":00.000");
                        objStart2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime2 + ":00.000");
                        objEnd2 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime2 + ":00.000");
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    else if (ParamedicScheduleDate.StartTime1 != "" && ParamedicScheduleDate.StartTime2 == "" && ParamedicScheduleDate.StartTime3 == "" && ParamedicScheduleDate.StartTime4 == "" && ParamedicScheduleDate.StartTime5 == "")
                    {
                        #region set StartDateTime And EndDateTime Master ParamedicSchedule
                        objStart1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.StartTime1 + ":00.000");
                        objEnd1 = DateTime.Parse(ParamedicScheduleDate.ScheduleDate.ToString("yyyy-MM-dd") + " " + ParamedicScheduleDate.EndTime1 + ":00.000");
                        #endregion

                        #region Take Data
                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime + ':00.000') BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime + ':00.000') BETWEEN '{4}' AND '{5}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);

                        List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment, ctx);

                        if (lstAppointment.Count > 0)
                        {
                            lstMaster.AddRange(lstAppointment);
                        }
                        #endregion
                    }
                    #endregion

                    if (lstMaster.Count == 0)
                    {
                        entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
                        ctx.CommitTransaction();

                        //#region Is Brigding To Gateway
                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDateChanged("CREATE", ParamedicScheduleDate1);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnPhysicianScheduleWithDate("2", ParamedicScheduleDate1);
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
                            else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                string apiResult = oService.OnPhysicianScheduleByDateChanged("003", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToDateTime(hdnScheduleDate.Value).ToString(Constant.FormatString.DATE_FORMAT_112), entityOldSchedule);
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
                            string apiResult = oService.OnParamedicScheduleDateChanged(ParamedicScheduleDate1, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "003");
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

                        //ParamedicScheduleDate entity = BusinessLayer.GetParamedicScheduleDate(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
                        //entity.IsDeleted = true;
                        //entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //BusinessLayer.DeleteParamedicScheduleDate(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
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
                    entityDao.Delete(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
                    ctx.CommitTransaction();

                    //#region Is Brigding To Gateway
                    if (hdnIsBridgingToGateway.Value == "1")
                    {
                        if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSMD)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleWithDateChanged("CREATE", ParamedicScheduleDate1);
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSKA)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            string apiResult = oService.OnPhysicianScheduleWithDate("2", ParamedicScheduleDate1);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.MessageDateTime = DateTime.Now;
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[1];
                                entityAPILog.Response = apiResultInfo[1];
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
                        else if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                        {
                            GatewayService oService = new GatewayService();
                            APIMessageLog entityAPILog = new APIMessageLog();
                            entityAPILog.Sender = "MEDINFRAS";
                            entityAPILog.Recipient = "QUEUE ENGINE";
                            string apiResult = oService.OnPhysicianScheduleByDateChanged("003", Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToDateTime(hdnScheduleDate.Value).ToString(Constant.FormatString.DATE_FORMAT_112), entityOldSchedule);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
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
                    }

                    if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
                    {

                        MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        string apiResult = oService.OnParamedicScheduleDateChanged(ParamedicScheduleDate1, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToInt32(hdnOperationalTimeID.Value), txtScheduleDate.Text, "003");
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

                    //ParamedicScheduleDate entity = BusinessLayer.GetParamedicScheduleDate(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
                    //entity.IsDeleted = true;
                    //entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //BusinessLayer.DeleteParamedicScheduleDate(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnParamedicID.Value), Convert.ToDateTime(hdnScheduleDate.Value));
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

        private bool onCheckMaximumAppointment(int appointmentCount, int session)
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

            int[] lstMaxApm = new int[5];
            lstMaxApm[0] = maxAppointment1 + maxAppointmentBPJS1;
            lstMaxApm[1] = maxAppointment2 + maxAppointmentBPJS2;
            lstMaxApm[2] = maxAppointment3 + maxAppointmentBPJS3;
            lstMaxApm[3] = maxAppointment4 + maxAppointmentBPJS4;
            lstMaxApm[4] = maxAppointment5 + maxAppointmentBPJS5; 

            if (session > 0)
            {
                if (lstMaxApm[session - 1] < appointmentCount)
                {
                    result = false;
                }
            }
            else
            {
                result = false;
            }
            return result;
        }

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
            lstMaxApm[1] = maxAppointment1+maxAppointmentBPJS1;
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
