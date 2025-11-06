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
    public partial class SurgeryOrderEntryCtl1 : BaseEntryPopupCtl3
    {
        protected int gridProcedureGroupPageCount = 1;
        protected int gridParamedicTeamPageCount = 1;

        protected static string _orderID = "0";

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnHealthcareServiceUnitID.Value = AppSession.MD0006;

            hdnVisitID.Value = paramInfo[0];
            hdnID.Value = paramInfo[1];

            IsAdd = false;
            _orderID = hdnID.Value;
            string filterExpression = string.Format("TestOrderID = {0}", hdnID.Value);
            vSurgeryTestOrderHd1 entity = BusinessLayer.GetvSurgeryTestOrderHd1List(filterExpression).FirstOrDefault();
            hdnVisitDepartmentID.Value = entity.DepartmentID;

            if (hdnVisitDepartmentID.Value == Constant.Facility.INPATIENT)
            {
                chkIsNextVisit.Enabled = false;
            }
            else
            {
                chkIsNextVisit.Enabled = true;
            }

            OnControlEntrySettingLocal(entity);
            ReInitControl();
            EntityToControl(entity);

            if (entity.IsUsedRequestTime)
                txtScheduleTime.Enabled = true;
        }

        private void OnControlEntrySettingLocal(vSurgeryTestOrderHd1 entity)
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}') AND IsDeleted = 0",
                                                    Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
                    Constant.StandardCode.SURGERY_TEAM_ROLE,
                    Constant.StandardCode.TIPE_PENYAKIT_INFEKSI,
                    Constant.StandardCode.TIPE_KOMORBID));

            List<StandardCode> lstCode1 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.SURGERY_TEAM_ROLE).ToList();
            List<StandardCode> lstCode2 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_PENYAKIT_INFEKSI).ToList();
            List<StandardCode> lstCode3 = lstCode.Where(lst => lst.ParentID == Constant.StandardCode.TIPE_KOMORBID).ToList();

            Methods.SetComboBoxField<StandardCode>(cboParamedicType, lstCode1, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCInfectiousDisease, lstCode2, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCComorbidities, lstCode3, "StandardCodeName", "StandardCodeID");

            SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtOrderTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtScheduleDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtScheduleTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtEstimatedDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, true, true));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));

            SetControlProperties();

            if (IsAdd)
            {
                txtOrderDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtScheduleDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
        }

        private void EntityToControl(vSurgeryTestOrderHd1 entity)
        {
            txtOrderDate.Text = entity.TestOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtOrderTime.Text = entity.TestOrderTime;
            txtMedicalNo.Text = entity.MedicalNo;
            txtPatientName.Text = entity.PatientName;
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtOrderNo.Text = entity.TestOrderNo;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            chkIsCITO.Checked = entity.IsCITO;
            chkIsUsedRequestTime.Checked = entity.IsUsedRequestTime;
            chkIsUsingSpecificItem.Checked = entity.IsUsingSpecificItem;
            txtScheduleDate.Text = entity.ScheduledDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtScheduleTime.Text = entity.ScheduledTime;
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

            BindGridViewProcedureGroup(1, true, ref gridProcedureGroupPageCount);
            BindGridViewParamedicTeam(1, true, ref gridParamedicTeamPageCount);
        }

        private void ControlToEntity(TestOrderHd entityHd)
        {
            entityHd.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
            entityHd.FromHealthcareServiceUnitID = entityHd.FromHealthcareServiceUnitID;
            entityHd.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entityHd.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entityHd.TestOrderDate = Helper.GetDatePickerValue(txtOrderDate);
            entityHd.TestOrderTime = txtOrderTime.Text.Replace('.', ':');
            entityHd.GCToBePerformed = Constant.ToBePerformed.SCHEDULLED;
            entityHd.EstimatedDuration = Convert.ToInt32(txtEstimatedDuration.Text);
            entityHd.ScheduledDate = Helper.GetDatePickerValue(txtScheduleDate);
            entityHd.ScheduledTime = txtScheduleTime.Text;
            entityHd.IsUsedRequestTime = chkIsUsedRequestTime.Checked;
            entityHd.IsUsingSpecificItem = chkIsUsingSpecificItem.Checked;
            entityHd.IsOperatingRoomOrder = true;
            entityHd.IsEmergency = rblIsEmergency.SelectedValue == "1" ? true : false;

            if (!string.IsNullOrEmpty(hdnRoomID.Value))
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
            entityHd.IsOperatingRoomOrder = true;
        }

        //protected override bool OnSaveAddRecord(ref string errMessage, ref string retVal)
        //{
        //    bool result = true;

        //    if (!IsValidated(ref errMessage))
        //    {
        //        result = false;
        //        return result;
        //    }

        //    IDbContext ctx = DbFactory.Configure(true);
        //    TestOrderHdDao oOrderHdDao = new TestOrderHdDao(ctx);
        //    RoomScheduleDao oScheduleDao = new RoomScheduleDao(ctx);
        //    TestOrderHd entity;

        //    try
        //    {
        //        if (_orderID == "0")
        //        {
        //            entity = new TestOrderHd();
        //            ControlToEntity(entity);
        //            entity.TransactionCode = Constant.TransactionCode.OTHER_TEST_ORDER;
        //            entity.TestOrderNo = BusinessLayer.GenerateTransactionNo(entity.TransactionCode, entity.TestOrderDate, ctx);
        //            if (!entity.IsODSVisit)
        //            {
        //                entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
        //                entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
        //            }
        //            else
        //            {
        //                entity.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
        //                entity.GCOrderStatus = Constant.OrderStatus.RECEIVED;
        //            }
        //            entity.CreatedBy = AppSession.UserLogin.UserID;
        //            _orderID = oOrderHdDao.InsertReturnPrimaryKeyID(entity).ToString();
        //        }
        //        else
        //        {
        //            entity = oOrderHdDao.Get(Convert.ToInt32(_orderID));
        //            ControlToEntity(entity);
        //            entity.GCOrderStatus = Constant.OrderStatus.RECEIVED; //Schedulled
        //            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //            oOrderHdDao.Update(entity);
        //        }

        //        string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);
        //        RoomSchedule oSchedule = BusinessLayer.GetRoomScheduleList(filterExpression, ctx).FirstOrDefault();
        //        bool isNewRecord = false;

        //        if (oSchedule == null)
        //        {
        //            isNewRecord = true;
        //            oSchedule = new RoomSchedule();
        //            oSchedule.CreatedBy = AppSession.UserLogin.UserID;
        //        }
        //        else
        //        {
        //            oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        }

        //        oSchedule.GCScheduleType = Constant.ScheduleType.OPERATING_ROOM;
        //        oSchedule.ScheduleDate = entity.ScheduledDate;
        //        oSchedule.ScheduleTime = entity.ScheduledTime;
        //        oSchedule.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
        //        oSchedule.RoomID = entity.RoomID;
        //        oSchedule.VisitID = entity.VisitID;
        //        oSchedule.TestOrderID = entity.TestOrderID;
        //        oSchedule.GCScheduleStatus = Constant.ScheduleStatus.OPEN;

        //        if (isNewRecord)
        //        {
        //            oScheduleDao.Insert(oSchedule);
        //        }
        //        else
        //        {
        //            oScheduleDao.Update(oSchedule);
        //        }

        //        ctx.CommitTransaction();
        //    }
        //    catch (Exception ex)
        //    {
        //        result = false;
        //        errMessage = ex.Message;
        //        Helper.InsertErrorLog(ex);
        //        ctx.RollBackTransaction();
        //    }
        //    finally
        //    {
        //        ctx.Close();
        //    }
        //    return result;
        //}

        private bool IsValidated(ref string errMessage)
        {
            StringBuilder message = new StringBuilder();

            if (string.IsNullOrEmpty(txtOrderTime.Text))
            {
                message.AppendLine("Jam Order Operasi tidak boleh kosong");
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

            if (string.IsNullOrEmpty(txtScheduleTime.Text))
            {
                message.AppendLine("Jam Rencana Operasi tidak boleh kosong");
            }
            else
            {
                if (!Methods.ValidateTimeFormat(txtScheduleTime.Text))
                    message.AppendLine("Format Jam Rencana Operasi tidak sesuai format (HH:MM)");
            }

            errMessage = message.ToString();

            return string.IsNullOrEmpty(errMessage);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retVal)
        {
            bool result = true;

            if (!IsValidated(ref errMessage))
            {
                result = false;
                return result;
            }

            IDbContext ctx = DbFactory.Configure(true);
            TestOrderHdDao oOrderHdDao = new TestOrderHdDao(ctx);
            RoomScheduleDao oScheduleDao = new RoomScheduleDao(ctx);
            bool isError = false;

            try
            {
                TestOrderHd entityUpdate = oOrderHdDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                if (chkIsNextVisit.Checked)
                {
                    if (entityUpdate.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        entityUpdate.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entityUpdate.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                }
                else
                {
                    if (entityUpdate.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        entityUpdate.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityUpdate.GCOrderStatus = Constant.OrderStatus.RECEIVED;
                }
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                oOrderHdDao.Update(entityUpdate);

                string filterExpression = string.Format("TestOrderID = {0} AND IsDeleted = 0", hdnID.Value);
                RoomSchedule oSchedule = BusinessLayer.GetRoomScheduleList(filterExpression, ctx).FirstOrDefault();
                bool isNewRecord = false;
                int appointmentID = 0;

                if (oSchedule == null)
                {
                    isNewRecord = true;
                    oSchedule = new RoomSchedule();
                    oSchedule.CreatedBy = AppSession.UserLogin.UserID;
                }
                else
                {
                    oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                }

                oSchedule.GCScheduleType = Constant.ScheduleType.OPERATING_ROOM;
                oSchedule.ScheduleDate = entityUpdate.ScheduledDate;
                oSchedule.ScheduleTime = entityUpdate.ScheduledTime;
                oSchedule.HealthcareServiceUnitID = entityUpdate.HealthcareServiceUnitID;
                oSchedule.RoomID = entityUpdate.RoomID;
                oSchedule.VisitID = entityUpdate.VisitID;
                oSchedule.TestOrderID = entityUpdate.TestOrderID;
                oSchedule.GCScheduleStatus = Constant.ScheduleStatus.OPEN;

                if (isNewRecord)
                {
                    #region Create Appointment : Jika Proses pertama kali dan order untuk kunjungan berikutnya
                    if (entityUpdate.IsODSVisit)
                    {
                        //Proses to Appointment Directly
                        vConsultVisit9 oVisit = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", Convert.ToInt32(hdnVisitID.Value)), ctx).FirstOrDefault();
                        if (oVisit != null)
                        {
                            AppointmentDao oAppointmentDao = new AppointmentDao(ctx);
                            Appointment oAppointment = new Appointment();
                            oAppointment.FromVisitID = Convert.ToInt32(hdnVisitID.Value); ;
                            oAppointment.HealthcareServiceUnitID = Convert.ToInt32(AppSession.MD0016);
                            oAppointment.ParamedicID = entityUpdate.ParamedicID;
                            oAppointment.VisitTypeID = Convert.ToInt32(AppSession.MD0017);
                            oAppointment.StartDate = entityUpdate.ScheduledDate;
                            oAppointment.StartTime = entityUpdate.ScheduledTime;
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
                            draftTestOrderHd.FromHealthcareServiceUnitID = entityUpdate.FromHealthcareServiceUnitID;
                            draftTestOrderHd.HealthcareServiceUnitID = entityUpdate.HealthcareServiceUnitID;
                            draftTestOrderHd.ParamedicID = entityUpdate.ParamedicID;
                            draftTestOrderHd.DraftTestOrderDate = entityUpdate.TestOrderDate;
                            draftTestOrderHd.DraftTestOrderTime = entityUpdate.TestOrderTime;
                            draftTestOrderHd.IsCITO = entityUpdate.IsCITO;
                            draftTestOrderHd.GCToBePerformed = Constant.ToBePerformed.CURRENT_EPISODE; //Episode sekarang jika di appointment
                            draftTestOrderHd.ScheduledDate = entityUpdate.ScheduledDate;
                            draftTestOrderHd.ScheduledTime = entityUpdate.ScheduledTime;
                            draftTestOrderHd.RoomID = entityUpdate.RoomID;
                            draftTestOrderHd.IsOperatingRoomOrder = entityUpdate.IsOperatingRoomOrder;
                            draftTestOrderHd.EstimatedDuration = entityUpdate.EstimatedDuration;
                            draftTestOrderHd.IsUsedRequestTime = entityUpdate.IsUsedRequestTime;
                            draftTestOrderHd.IsEmergency = entityUpdate.IsEmergency;
                            draftTestOrderHd.IsUsingSpecificItem = entityUpdate.IsUsingSpecificItem;
                            draftTestOrderHd.Remarks = entityUpdate.Remarks;
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
                        oScheduleDao.Insert(oSchedule);
                    }
                }
                else
                {
                    if (oSchedule.AppointmentID != null)
                    {
                        AppointmentDao oAppointmentDao = new AppointmentDao(ctx);
                        Appointment oAppointment = oAppointmentDao.Get(Convert.ToInt32(oSchedule.AppointmentID));
                        if (oAppointment != null)
                        {
                            DateTime oldAppointmentDate = oAppointment.StartDate;
                            oAppointment.StartDate = entityUpdate.ScheduledDate;
                            oAppointment.StartTime = entityUpdate.ScheduledTime;
                            if (oAppointment.StartDate !=  oldAppointmentDate)
                            {
                                oAppointment.AppointmentNo = BusinessLayer.GenerateTransactionNo(oAppointment.TransactionCode, oAppointment.StartDate);
                            }
                            oAppointment.ParamedicID = entityUpdate.ParamedicID;
                            oAppointment.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oAppointment.LastUpdatedDate = DateTime.Now;
                            oAppointmentDao.Update(oAppointment);

                            DraftTestOrderHdDao draftTestOrderHdDao = new DraftTestOrderHdDao(ctx);
                            string filterExp = string.Format("TransactionCode = '{0}' AND AppointmentID = '{1}' AND GCTransactionStatus != '{2}'",Constant.TransactionCode.DRAFT_OTHER_TEST_ORDER, oAppointment.AppointmentID, Constant.TransactionStatus.VOID);
                            DraftTestOrderHd draftTestOrderHd = BusinessLayer.GetDraftTestOrderHdList(filterExp, ctx).FirstOrDefault();
                            if (draftTestOrderHd != null)
                            {
                                draftTestOrderHd.ScheduledDate = entityUpdate.ScheduledDate;
                                draftTestOrderHd.ScheduledTime = entityUpdate.ScheduledTime;
                                draftTestOrderHd.ParamedicID = entityUpdate.ParamedicID;
                                draftTestOrderHd.RoomID = entityUpdate.RoomID;
                                draftTestOrderHd.IsOperatingRoomOrder = entityUpdate.IsOperatingRoomOrder;
                                draftTestOrderHd.EstimatedDuration = entityUpdate.EstimatedDuration;
                                draftTestOrderHd.IsUsedRequestTime = entityUpdate.IsUsedRequestTime;
                                draftTestOrderHd.IsEmergency = entityUpdate.IsEmergency;
                                draftTestOrderHd.IsUsingSpecificItem = entityUpdate.IsUsingSpecificItem;
                                draftTestOrderHd.Remarks = entityUpdate.Remarks;
                                draftTestOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                draftTestOrderHd.LastUpdatedDate = DateTime.Now;
                                draftTestOrderHdDao.Update(draftTestOrderHd);
                            }
                        }
                    }

                    if (!isError)
                    {
                        oScheduleDao.Update(oSchedule);
                    }
                }

                if (!isError)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }

                retVal = hdnID.Value;
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
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);

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
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    int orderID = 0;

                    if (param[0] == "add")
                    {
                        if (hdnID.Value == "0")
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

                        TestOrderDtProcedureGroup obj = new TestOrderDtProcedureGroup();

                        obj.TestOrderID = orderID;
                        obj.ProcedureGroupID = Convert.ToInt32(hdnEntryProcedureGroupID.Value);
                        obj.CreatedBy = AppSession.UserLogin.UserID;
                        procedureGroupDao.Insert(obj);

                        result = "1|add|";
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
                            result = "1|edit|";
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
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
                Helper.InsertErrorLog(ex);
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
                string filterExpression = string.Format("VisitID = {0} AND TestOrderID = {1} AND IsDeleted = 0", hdnVisitID.Value, _orderID);

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
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    int orderID = 0;
                    if (param[0] == "add")
                    {
                        if (hdnID.Value == "0")
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

                        result = "1|add|";
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
                Helper.InsertErrorLog(ex);
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