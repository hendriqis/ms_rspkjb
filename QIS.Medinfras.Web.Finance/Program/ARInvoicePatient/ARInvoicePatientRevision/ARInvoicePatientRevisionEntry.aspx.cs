using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePatientRevisionEntry : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PATIENT_APPROVAL;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = false;
        }

        protected string OnGetCustomerFilterExpression()
        {
            return string.Format("IsDeleted = 0");
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_PIUTANG));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;

            txtDocumentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtARDocumentReceiveDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_AR_LEAD_TIME,
                                                    Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnSetvarLeadTime.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_LEAD_TIME).FirstOrDefault().ParameterValue;
            hdnSetvarHitungJatuhTempoDari.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM).FirstOrDefault().ParameterValue;

            decimal tempTransactionAmount = -1, tempClaimedAmount = -1, tempVarianceAmount = -1, tempDiscountAmount = -1;

            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempClaimedAmount, ref tempVarianceAmount, ref tempDiscountAmount);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtARInvoiceNo, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnBusinessPartnerVirtualAccountID, new ControlEntrySetting(false, true, true));
            SetControlEntrySetting(txtBankName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtAccountNo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtRemarksRevision, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPrintAsName, new ControlEntrySetting(true, true, false));
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnIsEditable.Value = "1";
        }

        protected string OnGetARInvoiceHdFilterExpression()
        {
            return string.Format("MRN = {0} AND GCTransactionStatus IN ('{1}','{2}') AND TotalPaymentAmount = 0",
                                    AppSession.PatientDetail.MRN,
                                    Constant.TransactionStatus.APPROVED,
                                    Constant.TransactionStatus.PROCESSED);
        }

        public override int OnGetRowCount()
        {
            return BusinessLayer.GetvARInvoiceHdRowCount(OnGetARInvoiceHdFilterExpression());
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(OnGetARInvoiceHdFilterExpression(), PageIndex, "ARInvoiceID DESC").FirstOrDefault();
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = OnGetARInvoiceHdFilterExpression();
            PageIndex = BusinessLayer.GetvARInvoiceHdRowIndex(filterExpression, keyValue, "ARInvoiceID DESC");
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(filterExpression, PageIndex, "ARInvoiceID DESC").FirstOrDefault();
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vARInvoiceHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            txtARInvoiceNo.Text = entity.ARInvoiceNo;
            txtInvoiceDate.Text = entity.ARInvoiceDateInString;
            txtDueDate.Text = entity.DueDateInString;
            txtDocumentDate.Text = entity.DocumentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtARDocumentReceiveDate.Text = entity.ARDocumentReceiveDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtARDocumentReceiveByName.Text = entity.ARDocumentReceiveByName;
            cboBank.Value = entity.BankID.ToString();
            hdnBusinessPartnerVirtualAccountID.Value = entity.BusinessPartnerVirtualAccountID.ToString();
            txtBankName.Text = entity.BankName;
            txtAccountNo.Text = entity.BankAccountNo;
            txtRemarksRevision.Text = entity.Remarks;
            txtReferenceNo.Text = entity.ARReferenceNo;
            txtPrintAsName.Text = entity.PrintAsName;
            hdnARInvoiceID.Value = entity.ARInvoiceID.ToString();
            txtTotalTransaction.Text = entity.TotalTransactionAmount.ToString();
            txtTotalClaimed.Text = entity.TotalClaimedAmount.ToString();
            txtTotalVariance.Text = entity.TotalVarianceAmount.ToString();
            if (entity.TotalVarianceAmount < 0)
                txtTotalVariance.Attributes.Add("ForeColor", "Red");
            else
                txtTotalVariance.Attributes.Remove("ForeColor");
            txtTotalDiscount.Text = entity.TotalDiscountAmount.ToString();
            txtTotalPayment.Text = entity.TotalPaymentAmount.ToString();

            if (entity.TotalPaymentAmount != 0)
            {
                hdnIsAllowAddRevision.Value = "0";
            }
            else
            {
                hdnIsAllowAddRevision.Value = "1";
            }

            decimal tempTransactionAmount = -1;
            decimal tempClaimedAmount = -1;
            decimal tempVarianceAmount = -1;
            decimal tempDiscountAmount = -1;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            divRevisionCount.InnerHtml = entity.RevisionCount.ToString();
            divLastRevisionBy.InnerHtml = entity.LastRevisionByName;
            if (entity.LastRevisionDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastRevisionDate.InnerHtml = "";
            }
            else
            {
                divLastRevisionDate.InnerHtml = entity.LastRevisionDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempClaimedAmount, ref tempVarianceAmount, ref tempDiscountAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount, ref decimal claimedAmount, ref decimal varianceAmount, ref decimal discountAmount)
        {
            String filterExpression = "1 = 0";
            if (hdnARInvoiceID.Value != "")
            {
                filterExpression = string.Format("ARInvoiceID = {0} AND ISNULL(GCTransactionDetailStatus,'') != '{1}'", hdnARInvoiceID.Value, Constant.TransactionStatus.VOID);
                if (transactionAmount > -1)
                {
                    ARInvoiceHd entity = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                    transactionAmount = entity.TotalTransactionAmount;
                    claimedAmount = entity.TotalClaimedAmount;
                    varianceAmount = entity.TotalVarianceAmount;
                    discountAmount = entity.TotalDiscountAmount;
                    hdnTransactionStatus.Value = entity.GCTransactionStatus;
                }
            }

            int rowCount = BusinessLayer.GetvARInvoiceDtRowCount(filterExpression);

            List<vARInvoiceDt> lstInvoiceDt = BusinessLayer.GetvARInvoiceDtList(filterExpression, rowCount, pageIndex, "ARInvoiceID, RegistrationID, PaymentDetailID ASC");
            lvwView.DataSource = lstInvoiceDt;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vARInvoiceDt entity = e.Item.DataItem as vARInvoiceDt;
                TextBox txtClaimedAmount = e.Item.FindControl("txtClaimedAmount") as TextBox;
                TextBox txtVarianceAmount = e.Item.FindControl("txtVarianceAmount") as TextBox;
                TextBox txtDiscountAmount = e.Item.FindControl("txtDiscountAmount") as TextBox;

                txtClaimedAmount.Text = entity.ClaimedAmount.ToString();
                txtVarianceAmount.Text = entity.VarianceAmount.ToString();
                txtDiscountAmount.Text = entity.DiscountAmount.ToString();

                if (entity.PaymentAmount != 0)
                {
                    txtClaimedAmount.ReadOnly = true;
                    txtVarianceAmount.ReadOnly = true;
                    txtDiscountAmount.ReadOnly = true;
                }
            }
        }
        #endregion

        #region Header
        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceHdDao entityHdDao = new ARInvoiceHdDao(ctx);
            TermDao termDao = new TermDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                ARInvoiceHd entity = entityHdDao.Get(Convert.ToInt32(hdnARInvoiceID.Value));
                bool isTransactionLock = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.AR_INVOICE_PATIENT);
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
                    //entity.BankID = Convert.ToInt32(cboBank.Value); // ditutup oleh RN 20200908 karna dialihkan ke BPVA
                    entity.BusinessPartnerVirtualAccountID = Convert.ToInt32(hdnBusinessPartnerVirtualAccountID.Value);
                    entity.DocumentDate = Helper.GetDatePickerValue(txtDocumentDate);
                    entity.ARDocumentReceiveDate = Helper.GetDatePickerValue(txtARDocumentReceiveDate);
                    entity.ARDocumentReceiveByName = txtARDocumentReceiveByName.Text;

                    Term term = termDao.Get(entity.TermID);

                    DateTime tempdue1; // 1 = TglInvoice(ARInvoiceDate) || 2 = TglDokumen(DocumentDate) || 3 = TglTerimaDokumen(ARDocumentReceiveDate)
                    if (hdnSetvarHitungJatuhTempoDari.Value == "1")
                    {
                        tempdue1 = entity.ARInvoiceDate.AddDays(term.TermDay);
                    }
                    else if (hdnSetvarHitungJatuhTempoDari.Value == "2")
                    {
                        tempdue1 = entity.DocumentDate.AddDays(term.TermDay);
                    }
                    else if (hdnSetvarHitungJatuhTempoDari.Value == "3")
                    {
                        tempdue1 = entity.ARDocumentReceiveDate.AddDays(term.TermDay);
                    }
                    else
                    {
                        tempdue1 = entity.DocumentDate.AddDays(term.TermDay);
                    }

                    DateTime tempdue2 = tempdue1.AddDays(Convert.ToInt32(hdnSetvarLeadTime.Value));
                    entity.DueDate = tempdue2;

                    entity.Remarks = txtRemarksRevision.Text;
                    entity.PrintAsName = txtPrintAsName.Text;
                    entity.RevisionCount += 1;
                    entity.LastRevisionBy = AppSession.UserLogin.UserID;
                    entity.LastRevisionDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
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

        #region Callback
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            decimal transactionAmount = 0, claimedAmount = 0, varianceAmount = 0, discountAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    transactionAmount = -1;
                    claimedAmount = -1;
                    varianceAmount = -1;
                    discountAmount = -1;
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref transactionAmount, ref claimedAmount, ref varianceAmount, ref discountAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref transactionAmount, ref claimedAmount, ref varianceAmount, ref discountAmount);
                    result = string.Format("refresh|{0}|{1}|{2}|{3}", pageCount, transactionAmount, claimedAmount, varianceAmount);
                }
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
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                decimal tempTransactionAmount = -1;
                decimal tempClaimedAmount = -1;
                decimal tempVarianceAmount = -1;
                decimal tempDiscountAmount = -1;
                if (OnProcessRecord(ref errMessage, Convert.ToDecimal(param[1]), Convert.ToDecimal(param[2]), Convert.ToDecimal(param[3]), ref tempTransactionAmount, ref tempClaimedAmount, ref tempVarianceAmount, ref tempDiscountAmount))
                    result += string.Format("success|{0}|{1}|{2}|{3}", tempTransactionAmount, tempClaimedAmount, tempVarianceAmount, tempDiscountAmount);
                else
                    result += "fail|" + errMessage;
            }
            else if (param[0] == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += "fail|" + errMessage;
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                ARInvoiceHd entity = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                bool isTransactionLock = false;
                TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(Constant.TransactionCode.AR_INVOICE_PATIENT);
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
                    int ARInvoiceDtID = Convert.ToInt32(hdnARInvoiceDtID.Value);
                    int ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
                    int RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    int PaymentID = Convert.ToInt32(hdnPaymentID.Value);
                    int PaymentDetailID = Convert.ToInt32(hdnPaymentDetailID.Value);

                    ARInvoiceDt entityARDt = BusinessLayer.GetARInvoiceDt(ARInvoiceDtID);
                    entityARDt.GCTransactionDetailStatus = Constant.TransactionStatus.VOID;
                    entityARDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateARInvoiceDt(entityARDt);

                    entity.RevisionCount += 1;
                    entity.LastRevisionBy = AppSession.UserLogin.UserID;
                    entity.LastRevisionDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateARInvoiceHd(entity);
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }

            return result;
        }

        private bool OnProcessRecord(ref string errMessage, Decimal claimedAmountSave, Decimal varianceAmountSave, Decimal discountAmountSave, ref decimal transactionAmount, ref decimal claimedAmount, ref decimal varianceAmount, ref decimal discountAmount)
        {
            bool result = true;
            try
            {
                ARInvoiceHd entity = BusinessLayer.GetARInvoiceHd(Convert.ToInt32(hdnARInvoiceID.Value));
                bool isTransactionLock = false;
                TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(Constant.TransactionCode.AR_INVOICE_PATIENT);
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
                    int ARInvoiceDtID = Convert.ToInt32(hdnARInvoiceDtID.Value);
                    int ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
                    int RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
                    int PaymentID = Convert.ToInt32(hdnPaymentID.Value);
                    int PaymentDetailID = Convert.ToInt32(hdnPaymentDetailID.Value);

                    ARInvoiceDt entityDt = BusinessLayer.GetARInvoiceDt(ARInvoiceDtID);
                    entityDt.ClaimedAmount = claimedAmountSave;
                    entityDt.VarianceAmount = varianceAmountSave;
                    entityDt.DiscountAmount = discountAmountSave;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateARInvoiceDt(entityDt);

                    entity.RevisionCount += 1;
                    entity.LastRevisionBy = AppSession.UserLogin.UserID;
                    entity.LastRevisionDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateARInvoiceHd(entity);

                    transactionAmount = entity.TotalTransactionAmount;
                    claimedAmount = entity.TotalClaimedAmount;
                    varianceAmount = entity.TotalVarianceAmount;
                    discountAmount = entity.TotalDiscountAmount;
                }
                else
                {
                    result = false;
                    errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }

            return result;
        }
        #endregion

        #region Discount to All Dt
        protected void cbpProcessDiscount_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";

            if (ApplyDiscountToAllDt(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private Boolean ApplyDiscountToAllDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);

            try
            {
                string filterExpression = string.Format("ARInvoiceID = {0} AND GCTransactionDetailStatus != '{1}'", hdnARInvoiceID.Value, Constant.TransactionStatus.VOID);
                List<ARInvoiceDt> lstARInvoiceDt = BusinessLayer.GetARInvoiceDtList(filterExpression, ctx);
                foreach (ARInvoiceDt entityDt in lstARInvoiceDt)
                {
                    decimal entryDiscPercent = Convert.ToDecimal(Request.Form[txtEntryDiscountPercentage.UniqueID]);
                    decimal entryDiscAmount = Convert.ToDecimal(txtEntryDiscountAmount.Text);

                    decimal discountAmount = 0;

                    if (!chkIsDiscountPercent.Checked)
                    {
                        discountAmount = entryDiscAmount;
                    }
                    else
                    {
                        discountAmount = entityDt.TransactionAmount * (entryDiscPercent / 100);
                    }

                    discountAmount = Math.Round(discountAmount, 2);

                    entityDt.DiscountAmount = discountAmount;
                    entityDt.ClaimedAmount = entityDt.TransactionAmount + entityDt.VarianceAmount - entityDt.DiscountAmount;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
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
        #endregion
    }
}