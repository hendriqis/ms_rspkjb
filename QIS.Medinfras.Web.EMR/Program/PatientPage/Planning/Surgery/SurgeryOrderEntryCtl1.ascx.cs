using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class SurgeryOrderEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnVisitDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE));
            hdnHealthcareServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.MD_SERVICE_UNIT_OPERATING_THEATRE).ParameterValue;

            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                _orderID = hdnID.Value;
                hdnTestOrderID.Value = param;
                OnControlEntrySettingLocal();
                ReInitControl();
                string filterExpression = string.Format("TestOrderID = {0}", hdnID.Value);
                vSurgeryTestOrderHd1 entity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "0";
                _orderID = hdnID.Value;
                IsAdd = true;
                BindGridViewProcedureGroup(1, true, ref gridProcedureGroupPageCount);
                BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.SURGERY_TEAM_ROLE,
                    Constant.StandardCode.TIPE_PENYAKIT_INFEKSI));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_PENYAKIT_INFEKSI).ToList();
            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCInfectiousDisease, lstCode2, "StandardCodeName", "StandardCodeID");
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduleTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtEstimatedDuration, new ControlEntrySetting(true, true, true));

            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                chkIsNextVisit.Enabled = false;
                chkIsNextVisit.Checked = false;
            }
            else
            {
                chkIsNextVisit.Enabled = true;
                chkIsNextVisit.Checked = false;
            }
        }

        private void EntityToControl(vSurgeryTestOrderHd1 entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            chkIsCITO.Checked = entity.IsCITO;
            chkIsUsedRequestTime.Checked = entity.IsUsedRequestTime;
            chkIsUsingSpecificItem.Checked = entity.IsUsingSpecificItem;
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.IsUsedRequestTime ? string.Empty : entity.ScheduledTime;
            if (entity.IsUsedRequestTime)
                divScheduleInfo.Style.Add("display", "block");
            else
                divScheduleInfo.Style.Add("display", "none");

            if (entity.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
            {
                chkIsNextVisit.Checked = true;
                rblNextVisitType.SelectedValue = entity.IsODSVisit ? "1" : "2";
                trNextVisit.Style.Add("display", "table-row");
            }
            else
            {
                chkIsNextVisit.Checked = false;
                rblNextVisitType.SelectedValue = null;
                trNextVisit.Style.Add("display", "none");
            }

            rblIsEmergency.SelectedValue = entity.IsEmergency ? "1" : "0";

            chkIsUsingSpecificItem.Checked = entity.IsUsingSpecificItem;
            txtEstimatedDuration.Text = entity.EstimatedDuration.ToString();
            hdnRoomID.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            txtRemarks.Text = entity.Remarks;

            rblIsHasInfectiousDisease.SelectedValue = entity.IsHasInfectiousDisease ? "1" : "0";
            if (entity.IsHasInfectiousDisease)
            {
                cboGCInfectiousDisease.Value = entity.GCInfectiousDisease;
                txtOtherInfectiousDisease.Text = entity.OtherInfectiousDisease;
                trInfectiousInfo.Style.Add("display", "table-row");
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (cboGCInfectiousDisease.Value.ToString() == Constant.StandardCode.InfectiousDisease.OTHERS)
                        txtOtherInfectiousDisease.ReadOnly = false;
                }
            }
            else
            {
                trInfectiousInfo.Style.Add("display", "none");
            }

            BindGridViewProcedureGroup(1, true, ref gridProcedureGroupPageCount);
            BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityHd.FromHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.VisitHealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.VisitID = AppSession.RegisteredPatient.VisitID;
            entityHd.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entityHd.TestOrderTime = txtOrderTime.Text;
            entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
            entityHd.EstimatedDuration = Convert.ToInt32(txtEstimatedDuration.Text);
            entityHd.ScheduledDate = Helper.GetDatePickerValue(txtScheduleDate);
            entityHd.ScheduledTime = txtScheduleTime.Text;
            entityHd.IsUsedRequestTime = chkIsUsedRequestTime.Checked;
            entityHd.IsUsingSpecificItem = chkIsUsingSpecificItem.Checked;
            entityHd.IsOperatingRoomOrder = true;
            entityHd.IsEmergency = rblIsEmergency.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(hdnRoomID.Value) && hdnRoomID.Value != "0")
            {
                entityHd.RoomID = Convert.ToInt32(hdnRoomID.Value);
            }

            if (chkIsNextVisit.Checked)
            {
                entityHd.GCToBePerformed = Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT;
                entityHd.IsODSVisit = rblNextVisitType.SelectedValue == "1" ? true : false;
            }
            else
            {
                entityHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE;
                entityHd.IsODSVisit = false;
            }

            entityHd.IsCITO = chkIsCITO.Checked;
            entityHd.Remarks = txtRemarks.Text;

            entityHd.IsHasInfectiousDisease = rblIsHasInfectiousDisease.SelectedValue == "1" ? true : false;
            if (entityHd.IsHasInfectiousDisease)
            {
                if (cboGCInfectiousDisease.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCInfectiousDisease.Value.ToString()))
                    {
                        entityHd.GCInfectiousDisease = cboGCInfectiousDisease.Value.ToString();
                        if (cboGCInfectiousDisease.Value.ToString() == "X522^999")
                            entityHd.OtherInfectiousDisease = Page.Request.Form[txtOtherInfectiousDisease.UniqueID].ToString();
                        else
                            entityHd.OtherInfectiousDisease = null;
                    }
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);
            TestOrderDtParamedicTeamDao paramedicTeamDao = new TestOrderDtParamedicTeamDao(ctx);
            TestOrderDtProcedureGroupDao procedureGroupDao = new TestOrderDtProcedureGroupDao(ctx);
            RoomScheduleDao oScheduleDao = new RoomScheduleDao(ctx);
            int orderID = 0;
            int appointmentID = 0;

            try
            {
                if (IsValidated(ref errMessage))
                {
                    if (_orderID == "0")
                    {
                        TestOrderHd entity = new TestOrderHd();
                        ControlToEntity(entity);

                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.GCOrderStatus = Constant.OrderStatus.OPEN;
                        entity.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                        entity.TestOrderNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.TestOrderDate, ctx);
                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        orderID = entityHdDao.InsertReturnPrimaryKeyID(entity);
                        _orderID = orderID.ToString();

                        #region Create Appointment
                        if (chkIsNextVisit.Checked)
                        {
                            //Proses to Appointment Directly
                            vConsultVisit9 oVisit = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", entity.VisitID), ctx).FirstOrDefault();
                            if (oVisit != null)
                            {
                                AppointmentDao oAppointmentDao = new AppointmentDao(ctx);
                                Appointment oAppointment = new Appointment();
                                oAppointment.FromVisitID = entity.VisitID;
                                oAppointment.HealthcareServiceUnitID = Convert.ToInt32(AppSession.MD0016);
                                oAppointment.ParamedicID = entity.ParamedicID;
                                oAppointment.VisitTypeID = Convert.ToInt32(AppSession.MD0017);
                                oAppointment.StartDate = entity.ScheduledDate;
                                oAppointment.StartTime = entity.ScheduledTime;
                                oAppointment.EndDate = entity.ScheduledDate;
                                oAppointment.IsNewPatient = false;
                                oAppointment.MRN = oVisit.MRN;
                                oAppointment.Name = oVisit.PatientName;
                                oAppointment.GCGender = oVisit.GCGender;
                                oAppointment.StreetName = oVisit.StreetName;
                                oAppointment.PhoneNo = oVisit.PhoneNo1;
                                oAppointment.MobilePhoneNo = oVisit.MobilePhoneNo1;
                                oAppointment.GCSalutation = oVisit.GCSalutation;
                                oAppointment.GCCustomerType = oVisit.GCCustomerType;
                                oAppointment.BusinessPartnerID = oVisit.BusinessPartnerID;
                                oAppointment.GCAppointmentStatus = Constant.AppointmentStatus.STARTED;
                                oAppointment.TransactionCode = Constant.TransactionCode.OP_APPOINTMENT;
                                oAppointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(oAppointment.TransactionCode, oAppointment.StartDate);
                                oAppointment.Session = 1;
                                oAppointment.QueueNo = 1; //Abaikan nomor antrian, karena diatur berdasarkan jam
                                oAppointment.GCAppointmentMethod = Constant.AppointmentMethod.CALLCENTER;
                                oAppointment.Notes = string.Format("Nomor Order Tindakan Kamar Operasi dari Kunjungan Asal ", Request.Form["txtOrderNo"]);
                                oAppointment.CreatedBy = AppSession.UserLogin.UserID;
                                oAppointment.CreatedDate = DateTime.Now;
                                appointmentID = oAppointmentDao.InsertReturnPrimaryKeyID(oAppointment);

                                DraftTestOrderHd draftTestOrderHd = new DraftTestOrderHd();
                                DraftTestOrderHdDao draftTestOrderHdDao = new DraftTestOrderHdDao(ctx);

                                draftTestOrderHd.TransactionCode = Constant.TransactionCode.DRAFT_OTHER_TEST_ORDER;
                                draftTestOrderHd.AppointmentID = appointmentID;
                                draftTestOrderHd.FromHealthcareServiceUnitID = entity.FromHealthcareServiceUnitID;
                                draftTestOrderHd.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                                draftTestOrderHd.ParamedicID = entity.ParamedicID;
                                draftTestOrderHd.DraftTestOrderDate = entity.TestOrderDate;
                                draftTestOrderHd.DraftTestOrderTime = entity.TestOrderTime;
                                draftTestOrderHd.IsCITO = entity.IsCITO;
                                draftTestOrderHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE; //Episode sekarang jika di appointment
                                draftTestOrderHd.ScheduledDate = entity.ScheduledDate;
                                draftTestOrderHd.ScheduledTime = entity.ScheduledTime;
                                draftTestOrderHd.RoomID = entity.RoomID;
                                draftTestOrderHd.IsOperatingRoomOrder = entity.IsOperatingRoomOrder;
                                draftTestOrderHd.EstimatedDuration = entity.EstimatedDuration;
                                draftTestOrderHd.IsUsedRequestTime = entity.IsUsedRequestTime;
                                draftTestOrderHd.IsEmergency = entity.IsEmergency;
                                draftTestOrderHd.IsUsingSpecificItem = entity.IsUsingSpecificItem;
                                draftTestOrderHd.Remarks = entity.Remarks;
                                draftTestOrderHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                draftTestOrderHd.DraftTestOrderNo = BusinessLayer.GenerateTransactionNo(draftTestOrderHd.TransactionCode, draftTestOrderHd.DraftTestOrderDate);
                                draftTestOrderHd.CreatedBy = AppSession.UserLogin.UserID;
                                draftTestOrderHd.CreatedDate = DateTime.Now;
                                draftTestOrderHdDao.Insert(draftTestOrderHd);

                            }
                            else
                            {
                                errMessage = "Terjadi kesalahan pada saat pembuatan appointment untuk order ini (Invalid Visit Information)";
                                result = false;
                            }
                        }
                        #endregion

                    }

                    retVal = _orderID;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            try
            {
                TestOrderHd entityUpdate = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateTestOrderHd(entityUpdate);

                retVal = entityUpdate.TestOrderID.ToString();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(txtOrderTime.Text))
            {
                message.AppendLine("Jam order harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtOrderTime.Text))
                    message.AppendLine("Format Jam Order Operasi tidak sesuai format (HH:MM)");
            }

            if (string.IsNullOrEmpty(txtEstimatedDuration.Text))
                message.AppendLine("Estimasi Lama Operasi tidak boleh kosong atau 0");

            if (string.IsNullOrEmpty(txtScheduleDate.Text))
            {
                message.AppendLine("Tanggal Rencana Operasi tidak boleh kosong");
            }

            errMessage = message.ToString();

            return string.IsNullOrEmpty(errMessage);
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Procedure Group
        private void BindGridViewProcedureGroup(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnID.Value = _orderID;
            }

            List<vTestOrderDtProcedureGroup> lstEntity = new List<vTestOrderDtProcedureGroup>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _orderID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtProcedureGroupRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderDtProcedureGroupList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProcedureGroupCode");
            }

            grdProcedureGroupView.DataSource = lstEntity;
            grdProcedureGroupView.DataBind();
        }
        protected void cbpProcedureGroupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedureGroup(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedureGroup(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        }
        protected void cbpProcedureGroup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtProcedureGroupDao procedureGroupDao = new TestOrderDtProcedureGroupDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);

            try
            {
                string errMessage = string.Empty;
                if (!IsValidated(ref errMessage))
                {
                    result = string.Format("0|process|{0}", errMessage);
                    ctx.RollBackTransaction();
                }
                else
                {
                    if (e.Parameter != null && e.Parameter != "")
                    {
                        string[] param = e.Parameter.Split('|');
                        int orderID = 0;

                        if (param[0] == "add")
                        {
                            if (_orderID == "0")
                            {
                                TestOrderHd entityHd = new TestOrderHd();
                                ControlToEntity(entityHd);
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                                entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
                                entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                orderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                                _orderID = orderID.ToString();
                                hdnID.Value = orderID.ToString();
                            }
                            else
                            {
                                hdnID.Value = _orderID;
                                orderID = Convert.ToInt32(hdnID.Value);
                            }

                            TestOrderDtProcedureGroup obj = new TestOrderDtProcedureGroup();

                            obj.TestOrderID = orderID;
                            obj.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                            obj.CreatedBy = AppSession.UserLogin.UserID;
                            procedureGroupDao.Insert(obj);

                            result = "1|add|" + _orderID;
                        }
                        else if (param[0] == "edit")
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtProcedureGroupID.Value);
                            TestOrderDtProcedureGroup entity = BusinessLayer.GetTestOrderDtProcedureGroup(recordID);

                            if (entity != null)
                            {
                                entity.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateTestOrderDtProcedureGroup(entity);
                                result = "1|edit|" + _orderID;
                            }
                            else
                            {
                                result = string.Format("0|delete|{0}", "Jenis Tindakan Operasi tidak valid");
                            }
                        }
                        else
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtProcedureGroupID.Value);
                            TestOrderDtProcedureGroup entity = BusinessLayer.GetTestOrderDtProcedureGroup(recordID);

                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateTestOrderDtProcedureGroup(entity);
                                result = "1|delete|";
                            }
                            else
                            {
                                result = string.Format("0|edit|{0}", "Jenis Tindakan Operasi tidak valid");
                            }
                            result = "1|delete|";
                        }
                        ctx.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Paramedic Team
        private void BindGridViewParamedicTeam(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (Page.IsCallback && _orderID != "0")
            {
                hdnID.Value = _orderID;
            }

            List<vTestOrderDtParamedicTeam> lstEntity = new List<vTestOrderDtParamedicTeam>();
            if (hdnID.Value != "0")
            {
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, _orderID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvTestOrderDtParamedicTeamRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                lstEntity = BusinessLayer.GetvTestOrderDtParamedicTeamList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
                
            } 

            grdParamedicTeamView.DataSource = lstEntity;
            grdParamedicTeamView.DataBind();
        }
        protected void cbpParamedicTeamView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewParamedicTeam(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewParamedicTeam(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        protected void cbpParamedicTeam_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtParamedicTeamDao paramedicTeamDao = new TestOrderDtParamedicTeamDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);

            try
            {
                string errMessage = string.Empty;
                if (!IsValidated(ref errMessage))
                {
                    result = string.Format("0|process|{0}", errMessage);
                    ctx.RollBackTransaction();
                }
                else
                {
                    if (e.Parameter != null && e.Parameter != "")
                    {
                        string[] param = e.Parameter.Split('|');
                        int orderID = 0;
                        if (param[0] == "add")
                        {
                            if (_orderID == "0")
                            {
                                TestOrderHd entityHd = new TestOrderHd();
                                ControlToEntity(entityHd);
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entityHd.GCOrderStatus = Constant.OrderStatus.OPEN;
                                entityHd.TestOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.TestOrderDate);
                                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                                orderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                                _orderID = orderID.ToString();
                                hdnID.Value = orderID.ToString();
                            }
                            else
                            {
                                hdnID.Value = _orderID;
                                orderID = Convert.ToInt32(hdnID.Value);
                            }

                            TestOrderDtParamedicTeam obj = new TestOrderDtParamedicTeam();

                            obj.TestOrderID = orderID;
                            obj.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                            obj.GCParamedicRole = cboParamedicType.Value.ToString();
                            obj.CreatedBy = AppSession.UserLogin.UserID;
                            paramedicTeamDao.Insert(obj);

                            result = "1|add|" + _orderID;
                        }
                        else if (param[0] == "edit")
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                            TestOrderDtParamedicTeam entity = BusinessLayer.GetTestOrderDtParamedicTeam(recordID);

                            if (entity != null)
                            {
                                entity.ParamedicID = Convert.ToInt32(hdnEntryParamedicID.Value);
                                entity.GCParamedicRole = cboParamedicType.Value.ToString();
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateTestOrderDtParamedicTeam(entity);
                                result = "1|edit|" + _orderID;
                            }
                            else
                            {
                                result = string.Format("0|delete|{0}", "Informasi Dokter/Tenaga Medis tidak valid");
                            }
                        }
                        else
                        {
                            int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                            TestOrderDtParamedicTeam entity = BusinessLayer.GetTestOrderDtParamedicTeam(recordID);

                            if (entity != null)
                            {
                                entity.IsDeleted = true;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                BusinessLayer.UpdateTestOrderDtParamedicTeam(entity);
                                result = "1|delete|";
                            }
                            else
                            {
                                result = string.Format("0|edit|{0}", "Jenis Dokter/Tenaga Medis tidak valid");
                            }
                            result = "1|delete|";
                        }
                        ctx.CommitTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}