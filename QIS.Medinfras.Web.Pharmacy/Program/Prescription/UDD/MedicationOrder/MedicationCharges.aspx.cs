using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class MedicationCharges : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            return Constant.MenuCode.Pharmacy.UDD_MEDICATION_CHARGES;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
            IsAllowSave = false;
        }
        protected override void InitializeDataControl()
        {
            hdnIsAdminCanCancelAllTransaction.Value = AppSession.IsAdminCanCancelAllTransaction ? "1" : "0";

            Registration entityReg = BusinessLayer.GetRegistration(AppSession.RegisteredPatient.RegistrationID);
            hdnGCRegistrationStatus.Value = entityReg.GCRegistrationStatus;

            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnRegistrationPhysicianID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
            hdnBusinessPartnerID.Value = AppSession.RegisteredPatient.BusinessPartnerID.ToString();
            hdnGCCustomerType.Value = AppSession.RegisteredPatient.GCCustomerType;
            hdnClassID.Value = AppSession.RegisteredPatient.ChargeClassID.ToString();
            hdnVisitDepartmentID.Value = hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnLinkedRegistrationID.Value = AppSession.RegisteredPatient.LinkedRegistrationID.ToString();

            hdnHealthcareID.Value = AppSession.UserLogin.HealthcareID;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            List<ClassCare> lstClassCare = BusinessLayer.GetClassCareList("IsDeleted = 0");
            Methods.SetComboBoxField(cboChargeClass, lstClassCare, "ClassName", "ClassID");

            IsLoadFirstRecord = (OnGetRowCount() > 0);

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                    Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT, //1
                                                                    Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100, //2
                                                                    Constant.SettingParameter.IS_DEFAULT_TRANSAKSI_BPJS, //3
                                                                    Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION, //4
                                                                    Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1, //5
                                                                    Constant.SettingParameter.PH_IS_RIGHTPANELPRINT_MUST_PROPOSED_CHARGES //6
                                                                ));

            hdnPrescriptionFeeAmount.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PRESCRIPTION_FEE_AMOUNT).ParameterValue;
            hdnIsEndingAmountRoundingTo100.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_100).FirstOrDefault().ParameterValue;
            hdnIsEndingAmountRoundingTo1.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_END_AMOUNT_ROUNDING_TO_1).FirstOrDefault().ParameterValue;
            hdnDefaultJenisTransaksiBPJS.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.IS_DEFAULT_TRANSAKSI_BPJS).FirstOrDefault().ParameterValue;
            hdnIsReviewPrescriptionMandatoryForProposedTransaction.Value = lstSettingParameterDt.Where(a => a.ParameterCode == Constant.SettingParameter.PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION).FirstOrDefault().ParameterValue;
            hdnIsRightPanelPrintMustProposedCharges.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PH_IS_RIGHTPANELPRINT_MUST_PROPOSED_CHARGES).ParameterValue;

            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TIPE_TRANSAKSI_BPJS);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboBPJSTransType, lstStandardCode, "StandardCodeName", "StandardCodeID");
            
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoid.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override void SetControlProperties()
        {
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnPrescriptionOrderID, new ControlEntrySetting(false, false, false, ""));

            SetControlEntrySetting(txtPrescriptionOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrescriptionDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPrescriptionTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtDispenseryUnitName, new ControlEntrySetting(true, false, false));
            SetControlEntrySetting(hdnPrescriptionReturnOrderID, new ControlEntrySetting(false, false, false, hdnPrescriptionReturnOrderID.Value));
            SetControlEntrySetting(hdnHealthcareServiceUnitID, new ControlEntrySetting(false, false, false, hdnDefaultHealthcareServiceUnitID.Value));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(false, false, false, hdnDefaultLocationID.Value));
            SetControlEntrySetting(cboBPJSTransType, new ControlEntrySetting(false, true, true));

            if (hdnDefaultJenisTransaksiBPJS.Value == "0")
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                }
                else
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                }
            }
            else
            {
                if (hdnBusinessPartnerID.Value == "1")
                {
                    cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                }
                else
                {
                    string filterCustomer = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                    Customer entityCustomer = BusinessLayer.GetCustomerList(filterCustomer).FirstOrDefault();

                    if (entityCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.PAKET;
                    }
                    else
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                    }
                }
            }

        }

        private string ValidateTransaction()
        {
            string result;
            string lstSelectedID = "";
            List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value));
            if (lstEntityDt.Count > 0)
            {
                foreach (PatientChargesDt itm in lstEntityDt)
                    lstSelectedID += "," + itm.ItemID;
            }
            string filterExpression = string.Format(" LocationID = {0} AND ItemID IN({1}) AND IsDeleted = 0", hdnLocationID.Value, lstSelectedID.Substring(1));
            List<vItemBalanceForCheckStock> lstBalance = BusinessLayer.GetvItemBalanceForCheckStockList(filterExpression);
            StringBuilder errMessage = new StringBuilder();
            foreach (PatientChargesDt item in lstEntityDt)
            {
                vItemBalanceForCheckStock oBalance = lstBalance.Where(lst => lst.ItemID == item.ItemID).FirstOrDefault();
                if (oBalance != null)
                {
                    if (item.BaseQuantity > oBalance.QuantityEND)
                        errMessage.AppendLine(string.Format("Quantity Item {0} tidak mencukupi di Lokasi ini.", oBalance.ItemName1));
                }
            }

            if (!string.IsNullOrEmpty(errMessage.ToString()))
                result = string.Format("{0}|{1}", "0", errMessage.ToString());
            else
                result = string.Format("{0}|{1}", "1", "success");

            return result;
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            BindGridView();
            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
        }

        protected string GetFilterExpression()
        {
            String filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);

            filterExpression += string.Format(" AND DepartmentID = '{0}' AND PrescriptionReturnOrderID IS NULL", Constant.Facility.PHARMACY);

            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPatientChargesHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, " TransactionID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPatientChargesHdRowIndex(filterExpression, keyValue, "TransactionID DESC");
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHd(filterExpression, PageIndex, "TransactionID DESC");

            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            //string filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
            //hdnSelectedOrderID.Value = string.Empty;
            //PageIndex = BusinessLayer.GetvPrescriptionOrderHd3RowIndex(filterExpression, keyValue, "PrescriptionOrderID DESC");
            //vPrescriptionOrderHd3 entity = BusinessLayer.GetvPrescriptionOrderHd3(filterExpression, PageIndex, "PrescriptionOrderID DESC");
            //EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected bool IsEditable = true;
        private void EntityToControl(vPatientChargesHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (AppSession.IsAdminCanCancelAllTransaction)
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                {
                    isShowWatermark = true;
                    watermarkText = entity.TransactionStatusWatermark;
                    IsEditable = false;
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                }
            }
            else
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
                {
                    isShowWatermark = true;
                    watermarkText = entity.TransactionStatusWatermark;
                    IsEditable = false;
                }
                else
                {
                    isShowWatermark = false;
                    IsEditable = true;
                }
            }

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnTransactionID.Value = entity.TransactionID.ToString();
            hdnPrescriptionReturnOrderID.Value = entity.PrescriptionReturnOrderID.ToString();

            hdnPrescriptionOrderID.Value = entity.PrescriptionOrderID.ToString();
            hdnReferenceNo.Value = entity.ReferenceNo;
            txtPrescriptionOrderNo.Text = entity.TransactionNo;
            txtPrescriptionDate.Text = entity.TransactionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPrescriptionTime.Text = entity.TransactionTime.ToString();
            txtDispenseryUnitName.Text = entity.ServiceUnitName;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnLocationID.Value = entity.LocationID.ToString();

            if (entity.PrescriptionOrderID != null && entity.PrescriptionOrderID != 0)
            {
                PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(entity.PrescriptionOrderID);
                if (orderHd != null)
                {
                    txtOrderNo.Text = string.Format("{0}, {1}", orderHd.PrescriptionOrderNo, orderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT));
                }
            }
            else
            {
                string filterMS = string.Format("TransactionID = {0} AND IsDeleted = 0", entity.TransactionID);
                List<MedicationSchedule> lstMS = BusinessLayer.GetMedicationScheduleList(filterMS);
                if (lstMS.Count() > 0)
                {
                    PrescriptionOrderHd orderHd = BusinessLayer.GetPrescriptionOrderHd(Convert.ToInt32(lstMS.FirstOrDefault().PrescriptionOrderID));
                    if (orderHd != null)
                    {
                        txtOrderNo.Text = string.Format("{0}, {1}", orderHd.PrescriptionOrderNo, orderHd.PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT));
                    }
                }
            }
            
            if (entity.GCBPJSTransactionType != null && entity.GCBPJSTransactionType != "")
            {
                cboBPJSTransType.Value = entity.GCBPJSTransactionType;
            }
            else
            {
                if (hdnDefaultJenisTransaksiBPJS.Value == "0")
                {
                    if (hdnBusinessPartnerID.Value == "1")
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                    }
                    else
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                    }
                }
                else
                {
                    if (hdnBusinessPartnerID.Value == "1")
                    {
                        cboBPJSTransType.Value = Constant.BPJSTransactionType.DIBAYAR_PASIEN;
                    }
                    else
                    {
                        string filterCustomer = string.Format("BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
                        Customer entityCustomer = BusinessLayer.GetCustomerList(filterCustomer).FirstOrDefault();

                        if (entityCustomer.GCCustomerType == Constant.CustomerType.BPJS)
                        {
                            cboBPJSTransType.Value = Constant.BPJSTransactionType.PAKET;
                        }
                        else
                        {
                            cboBPJSTransType.Value = Constant.BPJSTransactionType.DITAGIHKAN;
                        }
                    }
                }
            }

            BindGridView();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate != null && entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
            {
                divLastUpdatedBy.InnerHtml = string.Empty;
                divLastUpdatedDate.InnerHtml = string.Empty;
            }
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPrescriptionOrderID.Value != "")
                filterExpression = string.Format("TransactionID = {0} AND ID IS NOT NULL AND IsDeleted = 0 ORDER BY ID DESC", hdnTransactionID.Value);
            List<vPatientChargesDt13> lstEntity = BusinessLayer.GetvPatientChargesDt13List(filterExpression);

            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
            if (lstEntity.Count > 0)
            {
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPayer")).InnerHtml = lstEntity.Sum(x => x.PayerAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAllPatient")).InnerHtml = lstEntity.Sum(x => x.PatientAmount).ToString("N");
                ((HtmlGenericControl)lvwView.FindControl("tdTotalAll")).InnerHtml = lstEntity.Sum(x => x.LineAmount).ToString("N");
            }
        }
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            return false;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);

            try
            {
                PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PatientChargesHdInfo entityInfo = entityHdInfoDao.Get(entity.TransactionID);
                    entityInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(entityInfo);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entity.TransactionNo);
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesHdInfoDao entityHdInfoDao = new PatientChargesHdInfoDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);

            try
            {
                PatientChargesHd entity = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {

                    string validationResult = ValidateTransaction();
                    string[] resultInfo = validationResult.Split('|');

                    if (resultInfo[0] == "0")
                    {
                        errMessage = resultInfo[1];
                        result = false;
                    }

                    List<PatientChargesDt> lstEntityDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsApproved = 0 AND IsDeleted = 0", hdnTransactionID.Value), ctx);
                    foreach (PatientChargesDt entityDt in lstEntityDt)
                    {
                        entityDt.IsApproved = true;
                        entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDtDao.Update(entityDt);
                    }

                    PatientChargesHdInfo entityInfo = entityHdInfoDao.Get(entity.TransactionID);
                    entityInfo.GCBPJSTransactionType = cboBPJSTransType.Value.ToString();
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdInfoDao.Update(entityInfo);

                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entity.TransactionNo);
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
        #endregion

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            string[] param = type.Split(';');
            string gcDeleteReason = param[1];
            string reason = param[2];
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PrescriptionOrderHdDao orderHdDao = new PrescriptionOrderHdDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            PatientChargesHdDao chargesDao = new PatientChargesHdDao(ctx);
            PrescriptionReturnOrderHdDao presReturnOrderHdDao = new PrescriptionReturnOrderHdDao(ctx);
            PrescriptionReturnOrderDtDao presReturnOrderDtDao = new PrescriptionReturnOrderDtDao(ctx);
            PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
            MedicationScheduleDao scheduleDao = new MedicationScheduleDao(ctx);

            try
            {
                Int32 TransactionID = Convert.ToInt32(hdnTransactionID.Value);
                PatientChargesHd entity = chargesDao.Get(TransactionID);

                if (param[0] == "void")
                {
                    if (entity.PatientBillingID == null && entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        #region Charges Dt
                        List<PatientChargesDt> lstPatientChargesDt = BusinessLayer.GetPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0 AND GCTransactionDetailStatus != '{1}'", hdnTransactionID.Value, Constant.TransactionStatus.VOID), ctx);
                        foreach (PatientChargesDt patientChargesDt in lstPatientChargesDt)
                        {
                            patientChargesDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                            patientChargesDt.IsApproved = false;
                            patientChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            chargesDtDao.Update(patientChargesDt);

                            if (entity.PrescriptionOrderID == null) // UDD
                            {
                            }
                            else // NON-UDD
                            {
                                PrescriptionOrderHd orderHd = orderHdDao.Get((int)entity.PrescriptionOrderID);
                                if (orderHd != null)
                                {
                                    #region Order Dt
                                    PrescriptionOrderDt orderDt = orderDtDao.Get((int)patientChargesDt.PrescriptionOrderDetailID);
                                    if (!orderDt.IsDeleted)
                                    {
                                        orderDt.GCPrescriptionOrderStatus = Constant.TestOrderStatus.RECEIVED;
                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        orderDtDao.Update(orderDt);
                                    }
                                    #endregion

                                    #region Order Hd
                                    List<PrescriptionOrderDt> lstOrderDtAll = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", orderHd.PrescriptionOrderID), ctx);
                                    List<PrescriptionOrderDt> lstOrderDtAllCancelled = BusinessLayer.GetPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND GCPrescriptionOrderStatus = '{1}' AND IsDeleted = 0", orderHd.PrescriptionOrderID, Constant.TestOrderStatus.CANCELLED), ctx);

                                    if (lstOrderDtAll.Count() == lstOrderDtAllCancelled.Count())
                                    {
                                        orderHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                        orderHd.GCVoidReason = Constant.DeleteReason.OTHER;
                                        orderHd.VoidReason = "Linked transaction was deleted";
                                        orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        orderHdDao.Update(orderHd);
                                    }

                                    #endregion
                                }
                            }
                        }
                        #endregion

                        #region Medication Schedule
                        string filterMS = string.Format("TransactionID = {0} AND GCMedicationStatus = '{1}' AND IsDeleted = 0", entity.TransactionID, Constant.MedicationStatus.DIPROSES_FARMASI);
                        List<MedicationSchedule> lstSchedule = BusinessLayer.GetMedicationScheduleList(filterMS, ctx);
                        foreach (MedicationSchedule oSchedule in lstSchedule)
                        {
                            if (entity.PrescriptionOrderID == null) // UDD
                            {
                                oSchedule.TransactionID = null;
                                oSchedule.ReferenceNo = null;
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                scheduleDao.Update(oSchedule);
                            }
                            else // NON-UDD
                            {
                                oSchedule.TransactionID = null;
                                oSchedule.ReferenceNo = null;
                                oSchedule.GCMedicationStatus = Constant.MedicationStatus.OPEN;
                                oSchedule.IsDeleted = true;
                                oSchedule.GCDeleteReason = Constant.DeleteReason.OTHER;
                                oSchedule.DeleteReason = "Deleted from Void menu MedicationCharges";
                                oSchedule.DeleteBy = AppSession.UserLogin.UserID;
                                oSchedule.DeleteDate = DateTime.Now;
                                oSchedule.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                scheduleDao.Update(oSchedule);
                            }
                        }
                        #endregion

                        #region Charges Hd
                        entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entity.GCVoidReason = gcDeleteReason;
                        if (gcDeleteReason == Constant.DeleteReason.OTHER)
                        {
                            entity.VoidReason = reason;
                        }
                        entity.VoidBy = AppSession.UserLogin.UserID;
                        entity.VoidDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        chargesDao.Update(entity);
                        #endregion

                        #region Prescription Return Order
                        if (entity.PrescriptionReturnOrderID != null)
                        {
                            List<PrescriptionReturnOrderDt> lstReturn = BusinessLayer.GetPrescriptionReturnOrderDtList(string.Format("PrescriptionReturnOrderID = {0})", entity.PrescriptionReturnOrderID), ctx);
                            foreach (PrescriptionReturnOrderDt presOrderReturn in lstReturn)
                            {
                                presOrderReturn.GCPrescriptionReturnOrderStatus = Constant.OrderStatus.CANCELLED;
                                presOrderReturn.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                presReturnOrderDtDao.Update(presOrderReturn);
                            }

                            PrescriptionReturnOrderHd returnHd = presReturnOrderHdDao.Get((int)entity.PrescriptionReturnOrderID);
                            returnHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            returnHd.VoidBy = AppSession.UserLogin.UserID;
                            returnHd.VoidDate = DateTime.Now;
                            returnHd.GCVoidReason = Constant.DeleteReason.OTHER;
                            returnHd.VoidReason = "Deleted from Void menu MedicationCharges";
                            returnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            presReturnOrderHdDao.Update(returnHd);
                        }
                        #endregion
                        
                        ctx.CommitTransaction();
                    }
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

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int TransactionID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (OnSaveEditRecordEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "switch")
            {
                int ID = Convert.ToInt32(param[1]);
                if (OnSwitchDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = TransactionID.ToString();
        }

        private bool OnSwitchDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entity = entityDtDao.Get(ID);
                        if (!entity.IsDeleted)
                        {
                            decimal temp = entity.PayerAmount;
                            entity.PayerAmount = entity.PatientAmount;
                            entity.PatientAmount = temp;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entity);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientChargesHdDao entityHdDao = new PatientChargesHdDao(ctx);
            PatientChargesDtDao entityDtDao = new PatientChargesDtDao(ctx);
            PrescriptionOrderDtDao orderDtDao = new PrescriptionOrderDtDao(ctx);
            try
            {
                PatientChargesHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));
                if (AppSession.IsAdminCanCancelAllTransaction)
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN || entityHd.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityChargesDt.IsDeleted)
                        {
                            if (entityChargesDt.PrescriptionOrderDetailID != null)
                            {
                                PrescriptionOrderDt orderDt = orderDtDao.Get(Convert.ToInt32(entityChargesDt.PrescriptionOrderDetailID));
                                if (hdnEmbalaceID.Value != "" && hdnEmbalaceID.Value != "0")
                                    orderDt.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
                                else
                                    orderDt.EmbalaceID = null;
                                if (Request.Form[txtEmbalaceQty.UniqueID] != "")
                                    orderDt.EmbalaceQty = Convert.ToDecimal(Request.Form[txtEmbalaceQty.UniqueID]);
                                else
                                    orderDt.EmbalaceQty = 0;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDtDao.Update(orderDt);
                            }

                            entityChargesDt.ChargeClassID = Convert.ToInt32(cboChargeClass.Value);
                            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBasePrice.Value);
                            entityChargesDt.TariffComp1 = entityChargesDt.Tariff = Convert.ToDecimal(Request.Form[txtItemUnitPrice.UniqueID]);

                            if (entityChargesDt.ChargedQuantity > 0)
                            {
                                if (!string.IsNullOrEmpty(hdnEmbalaceID.Value) && hdnEmbalaceID.Value != "0")
                                {
                                    entityChargesDt.EmbalaceAmount = Convert.ToDecimal(Request.Form[txtEmbalaceAmount.UniqueID]);
                                }
                                else
                                {
                                    entityChargesDt.EmbalaceAmount = 0;
                                }

                                entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);
                            }
                            else
                            {
                                entityChargesDt.EmbalaceAmount = 0;
                                entityChargesDt.PrescriptionFeeAmount = 0;
                            }

                            decimal oPatientAmount = Convert.ToDecimal(Request.Form[txtPatientAmount.UniqueID]);
                            decimal oPayerAmount = Convert.ToDecimal(Request.Form[txtPayerAmount.UniqueID]);
                            decimal oLineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);

                            if (hdnIsEndingAmountRoundingTo1.Value == "1")
                            {
                                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPatientAmount = Math.Floor(oPatientAmount);
                                }
                                else
                                {
                                    oPatientAmount = Math.Ceiling(oPatientAmount);
                                }

                                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPayerAmount = Math.Floor(oPayerAmount);
                                }
                                else
                                {
                                    oPayerAmount = Math.Ceiling(oPayerAmount);
                                }

                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            if (hdnIsEndingAmountRoundingTo100.Value == "1")
                            {
                                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            entityChargesDt.PatientAmount = oPatientAmount;
                            entityChargesDt.PayerAmount = oPayerAmount;
                            entityChargesDt.LineAmount = oLineAmount;

                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
                else
                {
                    if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        PatientChargesDt entityChargesDt = BusinessLayer.GetPatientChargesDt(Convert.ToInt32(hdnEntryID.Value));
                        if (!entityChargesDt.IsDeleted)
                        {
                            if (entityChargesDt.PrescriptionOrderDetailID != null)
                            {
                                PrescriptionOrderDt orderDt = orderDtDao.Get(Convert.ToInt32(entityChargesDt.PrescriptionOrderDetailID));
                                if (hdnEmbalaceID.Value != "" && hdnEmbalaceID.Value != "0")
                                    orderDt.EmbalaceID = Convert.ToInt32(hdnEmbalaceID.Value);
                                else
                                    orderDt.EmbalaceID = null;
                                if (Request.Form[txtEmbalaceQty.UniqueID] != "")
                                    orderDt.EmbalaceQty = Convert.ToDecimal(Request.Form[txtEmbalaceQty.UniqueID]);
                                else
                                    orderDt.EmbalaceQty = 0;
                                orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                orderDtDao.Update(orderDt);
                            }

                            entityChargesDt.ChargeClassID = Convert.ToInt32(cboChargeClass.Value);
                            entityChargesDt.BaseTariff = Convert.ToDecimal(hdnBasePrice.Value);
                            entityChargesDt.TariffComp1 = entityChargesDt.Tariff = Convert.ToDecimal(Request.Form[txtItemUnitPrice.UniqueID]);

                            if (entityChargesDt.ChargedQuantity > 0)
                            {
                                if (!string.IsNullOrEmpty(hdnEmbalaceID.Value) && hdnEmbalaceID.Value != "0")
                                {
                                    entityChargesDt.EmbalaceAmount = Convert.ToDecimal(Request.Form[txtEmbalaceAmount.UniqueID]);
                                }
                                else
                                {
                                    entityChargesDt.EmbalaceAmount = 0;
                                }

                                entityChargesDt.PrescriptionFeeAmount = Convert.ToDecimal(hdnPrescriptionFeeAmount.Value);
                            }
                            else
                            {
                                entityChargesDt.EmbalaceAmount = 0;
                                entityChargesDt.PrescriptionFeeAmount = 0;
                            }

                            decimal oPatientAmount = Convert.ToDecimal(Request.Form[txtPatientAmount.UniqueID]);
                            decimal oPayerAmount = Convert.ToDecimal(Request.Form[txtPayerAmount.UniqueID]);
                            decimal oLineAmount = Convert.ToDecimal(Request.Form[txtLineAmount.UniqueID]);

                            if (hdnIsEndingAmountRoundingTo1.Value == "1")
                            {
                                decimal upPatientAmount = Math.Ceiling(oPatientAmount);
                                decimal diffUpPatientAmount = upPatientAmount - oPatientAmount;
                                if (diffUpPatientAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPatientAmount = Math.Floor(oPatientAmount);
                                }
                                else
                                {
                                    oPatientAmount = Math.Ceiling(oPatientAmount);
                                }

                                decimal upPayerAmount = Math.Ceiling(oPayerAmount);
                                decimal diffUpPayerAmount = upPayerAmount - oPayerAmount;
                                if (diffUpPayerAmount >= Convert.ToDecimal("0.5"))
                                {
                                    oPayerAmount = Math.Floor(oPayerAmount);
                                }
                                else
                                {
                                    oPayerAmount = Math.Ceiling(oPayerAmount);
                                }

                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            if (hdnIsEndingAmountRoundingTo100.Value == "1")
                            {
                                oPatientAmount = Math.Ceiling(oPatientAmount / 100) * 100;
                                oPayerAmount = Math.Ceiling(oPayerAmount / 100) * 100;
                                oLineAmount = oPatientAmount + oPayerAmount;
                            }

                            entityChargesDt.PatientAmount = oPatientAmount;
                            entityChargesDt.PayerAmount = oPayerAmount;
                            entityChargesDt.LineAmount = oLineAmount;

                            entityChargesDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityChargesDt);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Transaksi dengan nomor <b>{0}</b> statusnya sudah tidak open, sehingga tidak dapat diubah lagi.", entityHd.TransactionNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                }
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

        private bool OnDeleteEntityDt(ref string errMessage)
        {
            bool result = true;
            return result;
        }
        #endregion

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}