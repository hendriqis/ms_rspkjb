using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using Newtonsoft.Json;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePatientAdjustmentEntry : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PATIENT_ADJUSTMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowVoid = IsAllowProposed = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnBusinessPartnerID.Value = AppSession.BusinessPartnerID.ToString();

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_BACKDATE, //1
                                                        Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_FUTUREDATE //2
                                                    );
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnIsAllowBackDate.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_BACKDATE).FirstOrDefault().ParameterValue;
            hdnIsAllowFutureDate.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_ARINVOICEDATE_ALLOW_FUTUREDATE).FirstOrDefault().ParameterValue;
            hdnDateToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_PIUTANG));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            txtInvoiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnARInvoiceID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtARInvoiceNo, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(txtInvoiceDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtDueDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtDocumentDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnBusinessPartnerVirtualAccountID, new ControlEntrySetting(false, true, true));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtAccountNo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRecipientName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrintAsName, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(hdnTransactionAdjustmentID, new ControlEntrySetting(false, true, true));
            SetControlEntrySetting(txtClaimAmount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(hdnRevenueCostCenterID, new ControlEntrySetting(false, true, true));
            SetControlEntrySetting(txtRevenueCostCenterCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRevenueCostCenterName, new ControlEntrySetting(false, false, true));
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Load Entity
        protected string onGetARInvoiceFilterExpression()
        {
            return string.Format("BusinessPartnerID = {0} AND TransactionCode = {1}",
                                    AppSession.BusinessPartnerID,
                                    Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT);
        }

        protected string GetFilterExpression()
        {
            return onGetARInvoiceFilterExpression();
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvARInvoiceHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHd(filterExpression, PageIndex, "ARInvoiceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvARInvoiceHdRowIndex(filterExpression, keyValue, "ARInvoiceID DESC");
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHd(filterExpression, PageIndex, "ARInvoiceID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vARInvoiceHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnARInvoiceID.Value = entity.ARInvoiceID.ToString();
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
            }
            else
            {
                hdnIsEditable.Value = "1";

            }
            if (hdnIsAllowPrintInvoiceAfterApprove.Value == "0")
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                {
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        hdnPrintStatus.Value = "true";
                    }
                    else
                    {
                        hdnPrintStatus.Value = "false";
                    }
                }
                else
                {
                    hdnPrintStatus.Value = "false";
                }
            }
            else
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    hdnPrintStatus.Value = "false";
                else
                    hdnPrintStatus.Value = "true";
            }

            txtARInvoiceNo.Text = entity.ARInvoiceNo;
            txtInvoiceDate.Text = entity.ARInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDueDate.Text = entity.DueDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboBank.Value = entity.BankID.ToString();
            hdnBusinessPartnerVirtualAccountID.Value = entity.BusinessPartnerVirtualAccountID.ToString();
            txtBankName.Text = entity.BankName;
            txtAccountNo.Text = entity.BankAccountNo;
            txtRemarks.Text = entity.Remarks;
            txtReferenceNo.Text = entity.ARReferenceNo;
            txtRecipientName.Text = entity.RecipientName;
            txtPrintAsName.Text = entity.PrintAsName;

            txtTotalClaimed.Text = entity.TotalClaimedAmount.ToString("N2");
            txtTotalDiscount.Text = entity.TotalDiscountAmount.ToString("N2");
            txtTotalPayment.Text = entity.TotalPaymentAmount.ToString("N2");

            BindGridView();
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView()
        {
            String filterExpression = "1 = 0";
            if (hdnARInvoiceID.Value != "")
            {
                filterExpression = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hdnARInvoiceID.Value, Constant.TransactionStatus.VOID);
            }

            List<vARInvoiceDtAdjustment> lstInvoiceDt = BusinessLayer.GetvARInvoiceDtAdjustmentList(filterExpression);
            lvwView.DataSource = lstInvoiceDt;
            lvwView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntityHd(ARInvoiceHd entityHd)
        {
            entityHd.ARInvoiceDate = Helper.GetDatePickerValue(txtInvoiceDate);
            entityHd.BusinessPartnerID = AppSession.BusinessPartnerID;
            entityHd.MRN = null;
            entityHd.RecipientName = txtRecipientName.Text;
            entityHd.PrintAsName = txtPrintAsName.Text;
            entityHd.DueDate = Helper.GetDatePickerValue(txtDueDate);
            entityHd.TermID = 1;
            entityHd.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
            entityHd.BusinessPartnerVirtualAccountID = Convert.ToInt32(hdnBusinessPartnerVirtualAccountID.Value);
            entityHd.Remarks = txtRemarks.Text;
        }

        public void SaveARInvoiceHd(IDbContext ctx, ref int ARInvoiceID)
        {
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            if (hdnARInvoiceID.Value == "0" || hdnARInvoiceID.Value == "")
            {
                ARInvoiceHd entityHd = new ARInvoiceHd();
                ControlToEntityHd(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT;
                entityHd.ARInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT, entityHd.ARInvoiceDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ARInvoiceID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int ARInvoiceID = 0;
                string errorMessage = "";
                SaveARInvoiceHd(ctx, ref ARInvoiceID);
                if (String.IsNullOrEmpty(errorMessage))
                {
                    retval = ARInvoiceID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = errorMessage;
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ARInvoiceHd entity = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));

                bool isLocked = false;
                TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.ARInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isLocked = false;
                    }
                    else
                    {
                        isLocked = true;
                    }
                }
                else
                {
                    isLocked = false;
                }

                if (!isLocked)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        ControlToEntityHd(entity);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateARInvoiceHd(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = string.Format("Penyesuaian piutang dengan nomor invoice {0} tidak dapat diubah. Harap refresh halaman ini.", entity.ARInvoiceNo);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }
                else
                {
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
        #endregion

        #region callBack Trigger
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int InvoiceID = 0;
            int ID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref InvoiceID))
                        result += string.Format("success|{0}", InvoiceID);
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                ID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, ID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ARInvoiceDt entityDt)
        {
            entityDt.TransactionNonOperationalTypeID = Convert.ToInt32(hdnTransactionAdjustmentID.Value);
            entityDt.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
            entityDt.GrouperAmountClaim = 0;
            entityDt.GrouperAmountFinal = 0;
            entityDt.ClaimedAmountBefore = 0;
            entityDt.TransactionAmount = entityDt.ClaimedAmount = Convert.ToDecimal(txtClaimAmount.Text);
            entityDt.VarianceAmount = 0;
            entityDt.PaymentAmount = 0;
            entityDt.ReferenceNo = "";
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int ARInvoiceID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            ARInvoiceDtAmortizationTempDao entityAmortizationTempDao = new ARInvoiceDtAmortizationTempDao(ctx);

            try
            {
                SaveARInvoiceHd(ctx, ref ARInvoiceID);
                if (entityHdDao.Get(ARInvoiceID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ARInvoiceDt entityDt = new ARInvoiceDt();
                    ControlToEntity(entityDt);
                    entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                    entityDt.ARInvoiceID = ARInvoiceID;
                    hdnARInvoiceID.Value = Convert.ToString(ARInvoiceID);
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    int entityDtID = entityDtDao.InsertReturnPrimaryKeyID(entityDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Transaksi tidak bisa diubah. Harap untuk refresh halaman ini";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            ARInvoiceDtAmortizationTempDao entityAmortizationTempDao = new ARInvoiceDtAmortizationTempDao(ctx);

            try
            {
                ARInvoiceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entityDt);
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);

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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            ARInvoiceDtAmortizationTempDao entityAmortizationTempDao = new ARInvoiceDtAmortizationTempDao(ctx);

            try
            {
                ARInvoiceDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entityDt);

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
        #endregion

        #region Process Header
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            ARInvoiceDtAmortizationDao entityAmortizationDao = new ARInvoiceDtAmortizationDao(ctx);
            ARInvoiceDtAmortizationTempDao entityAmortizationTempDao = new ARInvoiceDtAmortizationTempDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                ARInvoiceHd entity = entityHdDao.Get(Convert.ToInt32(hdnARInvoiceID.Value));

                bool isTransactionLock = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.ARInvoiceDate;
                    if (DateNow > DateLock)
                    {
                        isTransactionLock = false;
                    }
                    else
                    {
                        isTransactionLock = true;
                    }
                }
                else
                {
                    isTransactionLock = false;
                }

                if (!isTransactionLock)
                {
                    if (type.Contains("justvoid"))
                    {
                        #region justvoid

                        string[] param = type.Split(';');
                        string gcDeleteReason = param[1];
                        string reason = param[2];

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.GCVoidReason = gcDeleteReason;
                            entity.VoidReason = reason;
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

                            List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", entity.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                            foreach (ARInvoiceDt entityDt in lstARInvoiceDt)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }

                            retval = entity.ARInvoiceNo;

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else if (type.Contains("approve"))
                    {
                        #region approve
                        string[] param = type.Split(';');

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

                            List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", entity.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                            foreach (ARInvoiceDt entityDt in lstARInvoiceDt)
                            {
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.APPROVED;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);

                                string filterDtTemp = string.Format("ARInvoiceDtID = {0} AND IsDeleted = 0", entityDt.ID);
                                List<ARInvoiceDtAmortizationTemp> lstAmortizationTemp = BusinessLayer.GetARInvoiceDtAmortizationTempList(filterDtTemp, ctx);
                                foreach (ARInvoiceDtAmortizationTemp dtAmortizationTemp in lstAmortizationTemp)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    dtAmortizationTemp.IsDeleted = true;
                                    dtAmortizationTemp.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityAmortizationTempDao.Update(dtAmortizationTemp);

                                    ARInvoiceDtAmortization dtAmortization = new ARInvoiceDtAmortization();
                                    dtAmortization.ARInvoiceDtID = dtAmortizationTemp.ARInvoiceDtID;
                                    dtAmortization.AmortizationDate = dtAmortizationTemp.AmortizationDate;
                                    dtAmortization.AmortizationAmount = dtAmortizationTemp.AmortizationAmount;
                                    dtAmortization.CreatedBy = AppSession.UserLogin.UserID;
                                    entityAmortizationDao.Insert(dtAmortization);
                                }

                            }

                            retval = entity.ARInvoiceNo;

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else if (type.Contains("closenew"))
                    {
                        #region closenew

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            //VOID HD OLD
                            entity.Remarks = txtRemarks.Text;
                            entity.RecipientName = txtRecipientName.Text;
                            entity.PrintAsName = txtPrintAsName.Text;
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.GCVoidReason = Constant.DeleteReason.OTHER;
                            entity.VoidReason = "CLOSE & NEW";
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

                            //RE-INSERT
                            ARInvoiceHd entityHd = new ARInvoiceHd();
                            entityHd.ARReferenceNo = entity.ARInvoiceNo;
                            entityHd.ARInvoiceDate = entity.ARInvoiceDate;
                            //entityHd.BankID = entity.BankID; // ditutup oleh RN 20191118 karna dialihkan ke BPVA
                            entityHd.BusinessPartnerVirtualAccountID = entity.BusinessPartnerVirtualAccountID;
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entityHd.MRN = entity.MRN;
                            entityHd.BusinessPartnerID = entity.BusinessPartnerID;
                            entityHd.Remarks = entity.Remarks;
                            entityHd.RecipientName = entity.RecipientName;
                            entityHd.PrintAsName = entity.PrintAsName;
                            entityHd.TermID = entity.TermID;
                            entityHd.DocumentDate = entity.DocumentDate;
                            entityHd.ARDocumentReceiveDate = entity.ARDocumentReceiveDate;
                            entityHd.ARDocumentReceiveByName = entity.ARDocumentReceiveByName;
                            entityHd.DueDate = entity.DueDate;
                            entityHd.ARInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.AR_INVOICE_PATIENT_ADJUSTMENT, entityHd.ARInvoiceDate, ctx);
                            entityHd.CreatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityHd.ARInvoiceID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                            string filterDt = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", entity.ARInvoiceID, Constant.TransactionStatus.VOID);
                            List<ARInvoiceDt> lstDt = BusinessLayer.GetARInvoiceDtList(filterDt);
                            foreach (ARInvoiceDt dt in lstDt)
                            {
                                ARInvoiceDt entityDt = new ARInvoiceDt();
                                entityDt.ARInvoiceID = entityHd.ARInvoiceID;
                                entityDt.PaymentDetailID = dt.PaymentDetailID;
                                entityDt.RegistrationID = dt.RegistrationID;
                                entityDt.PaymentID = dt.PaymentID;
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.OPEN;
                                entityDt.ReferenceNo = dt.ReferenceNo;
                                entityDt.TransactionAmount = dt.TransactionAmount;
                                entityDt.ClaimedAmount = dt.ClaimedAmount;
                                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Insert(entityDt);
                            }

                            //VOID DT OLD
                            List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                                                "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'",
                                                                                entity.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                            foreach (ARInvoiceDt entityDt in lstARInvoiceDt)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }

                            retval = entityHd.ARInvoiceNo;

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else if (type.Contains("closevoid"))
                    {
                        #region closevoid

                        string[] param = type.Split(';');
                        string gcDeleteReason = param[1];
                        string reason = param[2];

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.GCVoidReason = gcDeleteReason;
                            entity.VoidReason = reason;
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityHdDao.Update(entity);

                            List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(string.Format(
                                                "ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", entity.ARInvoiceID, Constant.TransactionStatus.VOID), ctx);
                            foreach (ARInvoiceDt entityDt in lstARInvoiceDt)
                            {
                                entityDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }

                            retval = entity.ARInvoiceNo;

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Piutang tidak dapat diubah. Harap refresh halaman ini.";
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
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

    }
}