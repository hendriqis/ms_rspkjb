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


namespace QIS.Medinfras.Web.MedicalCheckup.Program
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
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}', '{1}', '{2}')", Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, Constant.SettingParameter.OP_CHECK_APPOINTMENT_BEFORE_CHANGE_PHYSICIAN_SCHEDULE));

            hdnCheckAppointmentBeforeChangeSchedule.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.OP_CHECK_APPOINTMENT_BEFORE_CHANGE_PHYSICIAN_SCHEDULE).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = lstSettingParameter.Where(p => p.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
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

            #region MaximumAppointment
            entity.MaximumAppointment1 = Convert.ToInt16(txtMaximumAppointment1.Text);
            entity.MaximumAppointment2 = Convert.ToInt16(txtMaximumAppointment2.Text);
            entity.MaximumAppointment3 = Convert.ToInt16(txtMaximumAppointment3.Text);
            entity.MaximumAppointment4 = Convert.ToInt16(txtMaximumAppointment4.Text);
            entity.MaximumAppointment5 = Convert.ToInt16(txtMaximumAppointment5.Text);
            #endregion

            #region AllowWaitingList
            entity.IsAllowWaitingList1 = chkIsAllowWaitingList1.Checked;
            entity.IsAllowWaitingList2 = chkIsAllowWaitingList2.Checked;
            entity.IsAllowWaitingList3 = chkIsAllowWaitingList3.Checked;
            entity.IsAllowWaitingList4 = chkIsAllowWaitingList4.Checked;
            entity.IsAllowWaitingList5 = chkIsAllowWaitingList5.Checked;
            #endregion

            #region MaximumWaitingList
            entity.MaximumWaitingList1 = Convert.ToInt16(txtMaximumWaitingList1.Text);
            entity.MaximumWaitingList2 = Convert.ToInt16(txtMaximumWaitingList2.Text);
            entity.MaximumWaitingList3 = Convert.ToInt16(txtMaximumWaitingList3.Text);
            entity.MaximumWaitingList4 = Convert.ToInt16(txtMaximumWaitingList4.Text);
            entity.MaximumWaitingList5 = Convert.ToInt16(txtMaximumWaitingList5.Text);
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
            #endregion

            #region Mobil Waiting Percentage
            if (!string.IsNullOrEmpty(txtMobileWaitingPercentage1.Text))
            {
                entity.MobileWaitingList1 = Convert.ToByte(txtMobileWaitingPercentage1.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileWaitingPercentage2.Text))
            {
                entity.MobileWaitingList2 = Convert.ToByte(txtMobileWaitingPercentage2.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileWaitingPercentage3.Text))
            {
                entity.MobileWaitingList3 = Convert.ToByte(txtMobileWaitingPercentage3.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileWaitingPercentage4.Text))
            {
                entity.MobileWaitingList4 = Convert.ToByte(txtMobileWaitingPercentage4.Text);
            }
            if (!string.IsNullOrEmpty(txtMobileWaitingPercentage5.Text))
            {
                entity.MobileWaitingList5 = Convert.ToByte(txtMobileWaitingPercentage5.Text);
            }
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
            #endregion

            #region Is BPJS Appointment
            if (chkIsBPJSAppointment1.Checked)
            {
                entity.IsBPJS1 = true;
            }
            else
            {
                entity.IsBPJS1 = false;
            }

            if (chkIsBPJSAppointment2.Checked)
            {
                entity.IsBPJS2 = true;
            }
            else
            {
                entity.IsBPJS2 = false;
            }

            if (chkIsBPJSAppointment3.Checked)
            {
                entity.IsBPJS3 = true;
            }
            else
            {
                entity.IsBPJS3 = false;
            }

            if (chkIsBPJSAppointment4.Checked)
            {
                entity.IsBPJS4 = true;
            }
            else
            {
                entity.IsBPJS4 = false;
            }

            if (chkIsBPJSAppointment5.Checked)
            {
                entity.IsBPJS5 = true;
            }
            else
            {
                entity.IsBPJS5 = false;
            }

            #endregion

            #region Is Non BPJS Appointment
            if (chkIsNonBPJSAppointment1.Checked)
            {
                entity.IsNonBPJS1 = true;
            }
            else
            {
                entity.IsNonBPJS1 = false;
            }

            if (chkIsNonBPJSAppointment2.Checked)
            {
                entity.IsNonBPJS2 = true;
            }
            else
            {
                entity.IsNonBPJS2 = false;
            }

            if (chkIsNonBPJSAppointment3.Checked)
            {
                entity.IsNonBPJS3 = true;
            }
            else
            {
                entity.IsNonBPJS3 = false;
            }

            if (chkIsNonBPJSAppointment4.Checked)
            {
                entity.IsNonBPJS4 = true;
            }
            else
            {
                entity.IsNonBPJS4 = false;
            }

            if (chkIsNonBPJSAppointment5.Checked)
            {
                entity.IsNonBPJS5 = true;
            }
            else
            {
                entity.IsNonBPJS5 = false;
            }
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

            #region MaximumAppointment
            txtMaximumAppointment1.Text = Convert.ToString(entity.MaximumAppointment1);
            txtMaximumAppointment2.Text = Convert.ToString(entity.MaximumAppointment2);
            txtMaximumAppointment3.Text = Convert.ToString(entity.MaximumAppointment3);
            txtMaximumAppointment4.Text = Convert.ToString(entity.MaximumAppointment4);
            txtMaximumAppointment5.Text = Convert.ToString(entity.MaximumAppointment5);
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

            #region MaximumWaitingList
            txtMaximumWaitingList1.Text = Convert.ToString(entity.MaximumWaitingList1);
            txtMaximumWaitingList2.Text = Convert.ToString(entity.MaximumWaitingList2);
            txtMaximumWaitingList3.Text = Convert.ToString(entity.MaximumWaitingList3);
            txtMaximumWaitingList4.Text = Convert.ToString(entity.MaximumWaitingList4);
            txtMaximumWaitingList5.Text = Convert.ToString(entity.MaximumWaitingList5);
            #endregion

            #region Mobile App Percentage
            txtMobileAppointment1.Text = Convert.ToString(entity.MobileAppointment1);
            txtMobileAppointment2.Text = Convert.ToString(entity.MobileAppointment2);
            txtMobileAppointment3.Text = Convert.ToString(entity.MobileAppointment3);
            txtMobileAppointment4.Text = Convert.ToString(entity.MobileAppointment4);
            txtMobileAppointment5.Text = Convert.ToString(entity.MobileAppointment5);
            #endregion

            #region Mobile App Percentage Waiting List
            txtMobileWaitingPercentage1.Text = Convert.ToString(entity.MobileWaitingList1);
            txtMobileWaitingPercentage2.Text = Convert.ToString(entity.MobileWaitingList2);
            txtMobileWaitingPercentage3.Text = Convert.ToString(entity.MobileWaitingList3);
            txtMobileWaitingPercentage4.Text = Convert.ToString(entity.MobileWaitingList4);
            txtMobileWaitingPercentage5.Text = Convert.ToString(entity.MobileWaitingList5);
            #endregion

            #region Reserved Queue Start No
            txtReservedQueueStartNo1.Text = Convert.ToString(entity.ReservedQueueStartNo1);
            txtReservedQueueStartNo2.Text = Convert.ToString(entity.ReservedQueueStartNo2);
            txtReservedQueueStartNo3.Text = Convert.ToString(entity.ReservedQueueStartNo3);
            txtReservedQueueStartNo4.Text = Convert.ToString(entity.ReservedQueueStartNo4);
            txtReservedQueueStartNo5.Text = Convert.ToString(entity.ReservedQueueStartNo5);
            #endregion

            #region Reserved Queue End No
            txtReservedQueueEndNo1.Text = Convert.ToString(entity.ReservedQueueEndNo1);
            txtReservedQueueEndNo2.Text = Convert.ToString(entity.ReservedQueueEndNo2);
            txtReservedQueueEndNo3.Text = Convert.ToString(entity.ReservedQueueEndNo3);
            txtReservedQueueEndNo4.Text = Convert.ToString(entity.ReservedQueueEndNo4);
            txtReservedQueueEndNo5.Text = Convert.ToString(entity.ReservedQueueEndNo5);
            #endregion

            #region Is BPJS Appointment
            chkIsBPJSAppointment1.Checked = entity.IsBPJS1;
            chkIsBPJSAppointment2.Checked = entity.IsBPJS2;
            chkIsBPJSAppointment3.Checked = entity.IsBPJS3;
            chkIsBPJSAppointment4.Checked = entity.IsBPJS4;
            chkIsBPJSAppointment5.Checked = entity.IsBPJS5;
            #endregion

            #region Is Non BPJS Appointment
            chkIsNonBPJSAppointment1.Checked = entity.IsNonBPJS1;
            chkIsNonBPJSAppointment2.Checked = entity.IsNonBPJS2;
            chkIsNonBPJSAppointment3.Checked = entity.IsNonBPJS3;
            chkIsNonBPJSAppointment4.Checked = entity.IsNonBPJS4;
            chkIsNonBPJSAppointment5.Checked = entity.IsNonBPJS5;
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
            try
            {
                if (hdnCheckAppointmentBeforeChangeSchedule.Value == "1")
                {
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


                    string filterAppointment = "";

                    if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 != "" && vParamedic.StartTime5 != "")
                    {
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

                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{12}' AND '{13}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4, objStart5, objEnd5);

                    }
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 != "" && vParamedic.StartTime5 == "")
                    {
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;
                        objStart3 = vParamedic.StartTime3;
                        objEnd3 = vParamedic.EndTime3;
                        objStart4 = vParamedic.StartTime4;
                        objEnd4 = vParamedic.EndTime4;

                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{10}' AND '{11}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3, objStart4, objEnd4);

                    }
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 != "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;
                        objStart3 = vParamedic.StartTime3;
                        objEnd3 = vParamedic.EndTime3;

                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{8}' AND '{9}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2, objStart3, objEnd3);

                    }
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 != "" && vParamedic.StartTime3 == "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;
                        objStart2 = vParamedic.StartTime2;
                        objEnd2 = vParamedic.EndTime2;


                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{6}' AND '{7}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1, objStart2, objEnd2);
                    }
                    else if (vParamedic.StartTime1 != "" && vParamedic.StartTime2 == "" && vParamedic.StartTime3 == "" && vParamedic.StartTime4 == "" && vParamedic.StartTime5 == "")
                    {
                        objStart1 = vParamedic.StartTime1;
                        objEnd1 = vParamedic.EndTime1;

                        filterAppointment = String.Format("ParamedicID = {0} AND GCAppointmentStatus NOT IN ('{1}','{2}') AND HealthcareServiceUnitID = {3} AND ((CONVERT(DATETIME, CONVERT(VARCHAR,StartDate) + ' ' + StartTime) BETWEEN '{4}' AND '{5}') OR (CONVERT(DATETIME, CONVERT(VARCHAR,EndDate) + ' ' + EndTime) BETWEEN '{4}' AND '{5}'))", Convert.ToInt32(hdnParamedicID.Value), Constant.AppointmentStatus.CANCELLED, Constant.AppointmentStatus.DELETED, Convert.ToInt32(hdnHealthcareServiceUnitID.Value), objStart1, objEnd1);
                    }

                    List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment);

                    if (lstAppointment.Count <= 0)
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
                        #endregion
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

                    List<Appointment> lstAppointment = BusinessLayer.GetAppointmentList(filterAppointment);

                    if (lstAppointment.Count <= 0)
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
    }
}