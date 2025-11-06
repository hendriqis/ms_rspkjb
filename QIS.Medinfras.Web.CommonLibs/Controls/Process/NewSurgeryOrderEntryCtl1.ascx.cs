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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NewSurgeryOrderEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _visitID = "0";
        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            IsAdd = true;

            hdnVisitID.Value = "0";
            hdnHealthcareServiceUnitID.Value = AppSession.MD0006;

            OnControlEntrySettingLocal();
            ReInitControl();
            hdnVisitID.Value = "0";
            hdnID.Value = "0";
            _orderID = hdnID.Value;
            _visitID = hdnVisitID.Value;
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}') AND IsDeleted = 0", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        {
            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                        "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.TIPE_PENYAKIT_INFEKSI,
                                                        Constant.StandardCode.TIPE_KOMORBID));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_PENYAKIT_INFEKSI).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_KOMORBID).ToList();
            Methods.SetComboBoxField<StandardCode>(cboGCInfectiousDisease, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCComorbidities, lstCode2, "StandardCodeName", "StandardCodeID");

            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduleTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtEstimatedDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRegistrationNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProcedureGroupCode, new ControlEntrySetting(true, true, true));

            SetControlProperties();

            if (IsAdd)
            {
                txtOrderDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtScheduleDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboParamedicID.Value = userLoginParamedic.ToString();
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
            txtScheduleTime.Text = entity.IsUsedRequestTime ? entity.ScheduledTime : string.Empty;
            if (entity.IsUsedRequestTime)
                divScheduleInfo.Style.Add("display", "block");
            else
                divScheduleInfo.Style.Add("display", "none");

            if (entity.GCToBePerformed == Constant.ToBePerformed.PRIOR_TO_NEXT_VISIT)
            {
                chkIsNextVisit.Checked = true;
                rblNextVisitType.SelectedValue = entity.IsODSVisit ? "1" : "2";

                if (entity.IsUsedRequestTime)
                    trNextVisit.Style.Add("display", "table-row");
                else
                    trNextVisit.Style.Add("display", "none");
            }
            else
            {
                chkIsNextVisit.Checked = false;
                rblNextVisitType.SelectedValue = "2";
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

            rblIsHasComorbidities.SelectedValue = entity.IsHasComorbidities ? "1" : "0";
            if (entity.IsHasComorbidities)
            {
                cboGCComorbidities.Value = entity.GCComorbidities;
                txtOtherComorbidities.Text = entity.OtherComorbidities;
                trComorbiditiesInfo.Style.Add("display", "table-row");
                if (cboGCComorbidities.Value != null)
                {
                    if (cboGCComorbidities.Value.ToString() == Constant.StandardCode.Comorbidities.OTHERS)
                        txtOtherComorbidities.ReadOnly = false;
                }
            }
            else
            {
                trComorbiditiesInfo.Style.Add("display", "none");
            }
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityHd.FromHealthcareServiceUnitID = Convert.ToInt32(hdnFromHealthcareServiceUnitID.Value);
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
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

            entityHd.IsHasComorbidities = rblIsHasComorbidities.SelectedValue == "1" ? true : false;
            if (entityHd.IsHasComorbidities)
            {
                if (cboGCComorbidities.Value != null)
                {
                    if (!string.IsNullOrEmpty(cboGCComorbidities.Value.ToString()))
                    {
                        entityHd.GCComorbidities = cboGCComorbidities.Value.ToString();
                        if (cboGCComorbidities.Value.ToString() == "X523^999")
                            entityHd.OtherComorbidities = Page.Request.Form[txtOtherComorbidities.UniqueID];
                        else
                            entityHd.OtherComorbidities = null;
                    }
                }
            }

            entityHd.IsCITO = chkIsCITO.Checked;
            entityHd.Remarks = txtRemarks.Text;

            entityHd.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
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
            bool isError = false;

            try
            {
                if (IsValid(ref errMessage))
                {
                    if (_orderID == "0")
                    {
                        TestOrderHd entity = new TestOrderHd();
                        ControlToEntity(entity);
                        if (!chkIsNextVisit.Checked)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        }
                        else
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                        }

                        entity.TestOrderNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.TestOrderDate, ctx);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        entity.CreatedBy = AppSession.UserLogin.UserID;
                        orderID = entityHdDao.InsertReturnPrimaryKeyID(entity);

                        TestOrderDtParamedicTeam obj1 = new TestOrderDtParamedicTeam();
                        obj1.TestOrderID = orderID;
                        obj1.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
                        obj1.GCParamedicRole = Constant.SurgeryTeamRole.OPERATOR;
                        obj1.CreatedBy = AppSession.UserLogin.UserID;
                        paramedicTeamDao.Insert(obj1);

                        TestOrderDtProcedureGroup obj2 = new TestOrderDtProcedureGroup();
                        obj2.TestOrderID = orderID;
                        obj2.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                        obj2.CreatedBy = AppSession.UserLogin.UserID;
                        procedureGroupDao.Insert(obj2);

                        RoomSchedule oSchedule = new RoomSchedule();
                        oSchedule.CreatedBy = AppSession.UserLogin.UserID;

                        oSchedule.GCScheduleType = Constant.ScheduleType.OPERATING_ROOM;
                        oSchedule.ScheduleDate = entity.ScheduledDate;
                        oSchedule.ScheduleTime = entity.ScheduledTime;
                        oSchedule.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                        oSchedule.RoomID = entity.RoomID;
                        oSchedule.VisitID = entity.VisitID;
                        oSchedule.TestOrderID = orderID;
                        oSchedule.GCScheduleStatus = Constant.ScheduleStatus.OPEN;

                        #region Create Appointment : Jika Proses pertama kali dan order untuk kunjungan berikutnya
                        if (chkIsNextVisit.Checked)
                        {
                            //Proses to Appointment Directly
                            vConsultVisit9 oVisit = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", Convert.ToInt32(hdnVisitID.Value)), ctx).FirstOrDefault();
                            if (oVisit != null)
                            {
                                AppointmentDao oAppointmentDao = new AppointmentDao(ctx);
                                Appointment oAppointment = new Appointment();
                                oAppointment.FromVisitID = Convert.ToInt32(hdnVisitID.Value);
                                oAppointment.HealthcareServiceUnitID = Convert.ToInt32(AppSession.MD0016);
                                oAppointment.ParamedicID = entity.ParamedicID;
                                oAppointment.VisitTypeID = Convert.ToInt32(AppSession.MD0017);
                                oAppointment.StartDate = entity.ScheduledDate;
                                oAppointment.StartTime = entity.ScheduledTime;
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

                                oSchedule.AppointmentID = appointmentID;
                            }
                            else
                            {
                                errMessage = "Terjadi kesalahan pada saat pembuatan appointment untuk order ini (Invalid Visit Information)";
                                isError = true;
                                result = false;
                            }
                        }
                        #endregion

                        if (!isError)
                        {
                            if (appointmentID != 0)
                            {
                                oSchedule.AppointmentID = appointmentID;
                            }
                            oScheduleDao.Insert(oSchedule);
                        }

                        ctx.CommitTransaction();
                    }
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

        private bool IsValid(ref string errMessage)
        {
            bool result = true;
            StringBuilder errMsg = new StringBuilder();

            #region Order Date
            string date = txtOrderDate.Text;
            if (string.IsNullOrEmpty(date))
            {
                errMsg.AppendLine("Tanggal Order harus diisi");
            }
            else
            {
                DateTime startDate;
                string format = Constant.FormatString.DATE_PICKER_FORMAT;
                try
                {
                    startDate = DateTime.ParseExact(date, format, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    errMsg.AppendLine(string.Format("Format Tanggal Order {0} tidak benar atau invalid", date));
                }

                DateTime sdate = Helper.GetDatePickerValue(date);
                if (DateTime.Compare(sdate, DateTime.Now.Date) > 0)
                {
                    errMsg.AppendLine("Tanggal order harus lebih kecil atau sama dengan tanggal saat ini.");
                }
            }
            #endregion

            #region Order Time
            if (string.IsNullOrEmpty(txtOrderTime.Text))
            {
                errMsg.AppendLine("Jam order harus diisi");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtOrderTime.Text))
                    errMsg.AppendLine("Format Jam Order Operasi tidak sesuai format (HH:MM)");
            }
            #endregion

            #region Schedule Date and Time
            if (string.IsNullOrEmpty(txtScheduleDate.Text))
            {
                errMsg.AppendLine("Tanggal Rencana Operasi tidak boleh kosong");
            }
            else
            {
                DateTime tmpDate;
                string scheduleDate = txtScheduleDate.Text;
                string format = Constant.FormatString.DATE_PICKER_FORMAT;
                try
                {
                    tmpDate = DateTime.ParseExact(scheduleDate, format, CultureInfo.InvariantCulture);
                }
                catch (FormatException)
                {
                    errMsg.AppendLine(string.Format("Format Tanggal Rencana Operasi {0} tidak benar atau invalid", date));
                }

                //DateTime sdate = Helper.GetDatePickerValue(scheduleDate);
                //if (DateTime.Compare(sdate, DateTime.Now.Date) < 0)
                //{
                //    errMsg.AppendLine("Tanggal Rencana Operasi harus lebih besar atau sama dengan tanggal saat ini.");
                //}
            }

            if (string.IsNullOrEmpty(txtScheduleTime.Text))
            {
                errMsg.AppendLine("Jam Rencana Operasi tidak boleh kosong");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtScheduleTime.Text))
                    errMsg.AppendLine("Format Jam Rencana Operasi tidak sesuai format (HH:MM)");
            }
            #endregion

            #region Dokter
            if (cboParamedicID.Value != null)
            {
                if (string.IsNullOrEmpty(cboParamedicID.Value.ToString()))
                {
                    errMsg.AppendLine("Dokter yang order harus diisi");
                }
            }
            else
            {
                errMsg.AppendLine("Dokter yang order harus diisi");
            }
            #endregion

            errMessage = errMsg.ToString();

            result = string.IsNullOrEmpty(errMessage.ToString());

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    TestOrderHd entityUpdate = BusinessLayer.GetTestOrderHd(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entityUpdate);
                    entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entityUpdate);

                    retVal = entityUpdate.TestOrderID.ToString();
                }
                else
                {
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        #region Paramedic Team
        protected void cbpParamedicTeam_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderDtParamedicTeamDao paramedicTeamDao = new TestOrderDtParamedicTeamDao(ctx);
            TestOrderHdDao entityHdDao = new TestOrderHdDao(ctx);

            try
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
                            if (!entityHd.IsODSVisit)
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            }
                            else
                            {
                                entityHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                entityHd.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                            }
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
                        obj.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
                        obj.GCParamedicRole = Constant.SurgeryTeamRole.OPERATOR;
                        obj.CreatedBy = AppSession.UserLogin.UserID;
                        paramedicTeamDao.Insert(obj);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnOrderDtParamedicTeamID.Value);
                        TestOrderDtParamedicTeam entity = BusinessLayer.GetTestOrderDtParamedicTeam(recordID);

                        if (entity != null)
                        {
                            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
                            entity.GCParamedicRole = Constant.SurgeryTeamRole.OPERATOR;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdateTestOrderDtParamedicTeam(entity);
                            result = "1|edit|";
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

        #region Registration
        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = "";
            if (rblSourcePatientType.SelectedValue == "1")
            {
                filterExpression = string.Format("DepartmentID = '{0}' AND GCRegistrationStatus IN ('{1}','{2}')", Constant.Facility.INPATIENT, Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.RECEIVING_TREATMENT);
            }
            else
            {
                filterExpression = string.Format("DepartmentID = '{0}' AND GCRegistrationStatus NOT IN ('{1}')", Constant.Facility.OUTPATIENT, Constant.VisitStatus.CANCELLED);
            }
            return filterExpression;
        }
        #endregion
    }
}