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
using QIS.Medinfras.Web.CommonLibs.Program;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APSupplierVerification : BasePageTrx
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private string[] lstNilai = null;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_SUPPLIER_VERIFICATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            GetUserMenuAccess menu = BusinessLayer.GetUserMenuAccess(ModuleID, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            string CRUDMode = menu.CRUDMode;
            hdnIsAllowVoidByReason.Value = CRUDMode.Contains("X") ? "1" : "0";

            hdnIsAdd.Value = "1";

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.FN_STAMP_AMOUNT_IN_SUPPLIER_PAYMENT_HD,
                                                        Constant.SettingParameter.FN_SUPPLIERPAYMENTNO_COUNTER_BY_DATE,
                                                        Constant.SettingParameter.FN_IS_USING_APPROVAL_VERIFICATION_SUPPLIER
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnStampAmount.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_STAMP_AMOUNT_IN_SUPPLIER_PAYMENT_HD).ToList().FirstOrDefault().ParameterValue;

            hdnCounterNoPakaiTanggal.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_SUPPLIERPAYMENTNO_COUNTER_BY_DATE).ToList().FirstOrDefault().ParameterValue;

            hdnUsingApprovalVerification.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_IS_USING_APPROVAL_VERIFICATION_SUPPLIER).ToList().FirstOrDefault().ParameterValue;

            cboPaymentMethod.SelectedIndex = 0;
            Helper.SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtDueDateFrom, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtDueDateTo, new ControlEntrySetting(true, false, false), "mpEntry");

            txtDueDateFrom.Text = txtVerificationDate.Text = txtReferenceDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDueDateTo.Text = DateTime.Today.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();
        }

        protected String IsAdd()
        {
            return hdnIsAdd.Value;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override void OnAddRecord()
        {
            hdnIsAdd.Value = "1";
            IsAdd();
            BindGridView();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                                    "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                                    Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.SUPPLIER_PAYMENT_METHOD));
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.SUPPLIER_PAYMENT_METHOD).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.SelectedIndex = 1;

            List<Bank> lstBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0 AND (GCBankType = '{0}' OR GCBankType IS NULL)", Constant.BankType.BANK_HUTANG));
            Methods.SetComboBoxField<Bank>(cboBank, lstBank, "BankName", "BankID");
            cboBank.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnFilterExpressionQuickSearch, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedMember, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedPayment, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnSelectedSupplier, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnSupplierPaymentID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "1"));
            SetControlEntrySetting(hdnIsEditable, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(true, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            if (hdnCounterNoPakaiTanggal.Value == "2") // pakai tanggal rencana bayar
            {
                SetControlEntrySetting(txtPlanningPaymentDate, new ControlEntrySetting(true, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }
            else
            {
                SetControlEntrySetting(txtPlanningPaymentDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }

            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankReferenceNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDueDateFrom, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtDueDateTo, new ControlEntrySetting(true, true, false, DateTime.Now.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtStampAmount, new ControlEntrySetting(true, true, false, hdnStampAmount.Value));
            SetControlEntrySetting(txtVerificationAmount, new ControlEntrySetting(false, false, false, "0"));
        }

        public override int OnGetRowCount()
        {
            string filterExpression = "";
            return BusinessLayer.GetvSupplierPaymentHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = "";
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHd(filterExpression, PageIndex, "SupplierPaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = "";
            if (keyValue != "0" && keyValue != "" && keyValue != null)
            {
                filterExpression = string.Format("SupplierPaymentNo = '{0}'", keyValue);
                PageIndex = BusinessLayer.GetvSupplierPaymentHdRowIndex(filterExpression, keyValue, "SupplierPaymentID DESC");
                vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHd(filterExpression, PageIndex, "SupplierPaymentID DESC");
                EntityToControl(entity, ref isShowWatermark, ref watermarkText);
            }
            else
            {
                BindGridView();
            }
        }

        private void EntityToControl(vSupplierPaymentHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED || entity.GCTransactionStatus == Constant.TransactionStatus.VOID)
                {
                    SetControlEntrySetting(txtVerificationDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(txtPlanningPaymentDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(false, false, true));
                    SetControlEntrySetting(cboBank, new ControlEntrySetting(false, false, true));
                    SetControlEntrySetting(txtBankReferenceNo, new ControlEntrySetting(false, false, true));
                }
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                hdnIsEditable.Value = "1";
                cboPaymentMethod.Enabled = true;
                cboBank.Enabled = true;
                txtStampAmount.Enabled = true;
            }
            else
            {
                hdnIsEditable.Value = "0";
                cboPaymentMethod.Enabled = false;
                cboBank.Enabled = false;
                txtStampAmount.Enabled = false;
            }

            hdnSupplierPaymentID.Value = entity.SupplierPaymentID.ToString();
            txtPaymentNo.Text = entity.SupplierPaymentNo;
            txtVerificationDate.Text = entity.VerificationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPlanningPaymentDate.Text = entity.PlanningPaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;
            cboPaymentMethod.Value = entity.GCSupplierPaymentMethod;
            txtVoucherNo.Text = entity.JournalNo;
            if (entity.JournalDate != null && entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtVoucherDate.Text = entity.JournalDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            txtReferenceNo.Text = entity.ReferenceNo;
            txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER || entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO || entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
            {
                trBank.Attributes.Remove("style");
                trBankRef.Attributes.Remove("style");
                cboBank.Value = entity.BankID.ToString();
                txtBankReferenceNo.Text = entity.BankReferenceNo;
            }
            else
            {
                trBank.Attributes.Add("style", "display:none");
                trBankRef.Attributes.Add("style", "display:none");
            }

            txtStampAmount.Text = entity.StampAmount.ToString(Constant.FormatString.NUMERIC_2);

            ////List<SupplierPaymentDt> dt = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", entity.SupplierPaymentID));
            ////decimal total = entity.StampAmount;
            ////foreach (SupplierPaymentDt lst in dt)
            ////{
            ////    total += lst.PaymentAmount;
            ////}
            txtVerificationAmount.Text = entity.TotalPaymentAmount.ToString(Constant.FormatString.NUMERIC_2);
            hdnTransactionStatus.Value = entity.GCTransactionStatus;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.VerifiedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trProposedBy.Style.Add("display", "none");
                trProposedDate.Style.Add("display", "none");

                divProposedBy.InnerHtml = "";
                divProposedDate.InnerHtml = "";
            }
            else
            {
                trProposedBy.Style.Remove("display");
                trProposedDate.Style.Remove("display");

                divProposedBy.InnerHtml = entity.VerifiedByName;
                divProposedDate.InnerHtml = entity.VerifiedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.VoidDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");

                divVoidBy.InnerHtml = "";
                divVoidDate.InnerHtml = "";
            }
            else
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.VoidDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter == "delete")
            {
                if (OnDeleteEntityDt(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private bool OnDeleteEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierPaymentHdDao entityHdDao = new SupplierPaymentHdDao(ctx);
            SupplierPaymentDtDao entityDtDao = new SupplierPaymentDtDao(ctx);
            PurchaseInvoiceHdDao entityInvoiceHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseInvoiceDtPaymentDao entityInvoiceDtPaymentDao = new PurchaseInvoiceDtPaymentDao(ctx);
            PurchaseInvoiceHdPaymentDao entityInvoiceHdPaymentDao = new PurchaseInvoiceHdPaymentDao(ctx);
            try
            {
                int supplierPaymentID = Convert.ToInt32(hdnSupplierPaymentID.Value);
                int purchaseInvoiceID = Convert.ToInt32(hdnEntryID.Value);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filter = string.Format("SupplierPaymentID = '{0}' AND PurchaseInvoiceID = '{1}'", supplierPaymentID, purchaseInvoiceID);
                SupplierPaymentDt entityDt = BusinessLayer.GetSupplierPaymentDtList(filter, ctx).FirstOrDefault();

                entityDtDao.Delete(supplierPaymentID, purchaseInvoiceID);

                PurchaseInvoiceHd entityInvoice = entityInvoiceHdDao.Get(purchaseInvoiceID);
                entityInvoice.NumberOfPayment -= 1;
                entityInvoice.PaymentAmount -= entityDt.PaymentAmount;
                entityInvoice.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                entityInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityInvoiceHdDao.Update(entityInvoice);

                decimal paymentAmount = entityDt.PaymentAmount;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<PurchaseInvoiceDt> lstPurchaseInvoiceDt1 = BusinessLayer.GetPurchaseInvoiceDtList(string.Format("PurchaseInvoiceID IN ({0}) AND IsDeleted = 0", purchaseInvoiceID), ctx);
                foreach (PurchaseInvoiceDt purchaseInvoiceDt in lstPurchaseInvoiceDt1)
                {
                    if (paymentAmount > 0)
                    {
                        decimal tempPaymentAmount = paymentAmount;
                        decimal remainingAmount = purchaseInvoiceDt.LineAmount - purchaseInvoiceDt.PaymentAmount;
                        if (tempPaymentAmount > remainingAmount)
                            tempPaymentAmount = remainingAmount;
                        purchaseInvoiceDt.PaymentAmount -= tempPaymentAmount;
                        entityInvoiceDtDao.Update(purchaseInvoiceDt);

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        string filterPIDT = string.Format("PurchaseInvoiceDtID = '{0}' AND SupplierPaymentID = '{1}' AND PaymentDate = '{2}'", purchaseInvoiceDt.ID, supplierPaymentID, Helper.GetDatePickerValue(txtVerificationDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                        PurchaseInvoiceDtPayment entityInvoiceDtPayment = BusinessLayer.GetPurchaseInvoiceDtPaymentList(filterPIDT, ctx).FirstOrDefault();
                        if (entityInvoiceDtPayment != null)
                        {
                            entityInvoiceDtPaymentDao.Delete(entityInvoiceDtPayment.ID);
                        }

                        paymentAmount -= tempPaymentAmount;
                    }
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                string filterPIHD = string.Format("PurchaseInvoiceID = '{0}' AND SupplierPaymentID = '{1}' AND PaymentDate = '{2}'", purchaseInvoiceID, supplierPaymentID, Helper.GetDatePickerValue(txtVerificationDate.Text).ToString(Constant.FormatString.DATE_PICKER_FORMAT2));
                PurchaseInvoiceHdPayment entityInvoiceHdPayment = BusinessLayer.GetPurchaseInvoiceHdPaymentList(filterPIHD, ctx).FirstOrDefault();
                if (entityInvoiceHdPayment != null)
                {
                    entityInvoiceHdPaymentDao.Delete(entityInvoiceHdPayment.ID);
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

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnIsAdd.Value == "0")
            {
                if (hdnSupplierPaymentID.Value != "" && hdnSupplierPaymentID.Value != "0")
                {
                    filterExpression = string.Format("SupplierPaymentID = {0} ORDER BY PurchaseInvoiceID", hdnSupplierPaymentID.Value);
                    List<vSupplierPaymentDt> lstEntity = BusinessLayer.GetvSupplierPaymentDtList(filterExpression);
                    grdView.DataSource = lstEntity;
                    grdView.DataBind();
                }
            }
            else
            {
                if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
                {
                    filterExpression = hdnFilterExpressionQuickSearch.Value;
                    filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}','{1}') AND (TotalNetTransactionAmount != PaymentAmount OR TotalNetTransactionAmount = 0)", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
                }
                else
                {
                    filterExpression = string.Format("GCTransactionStatus IN ('{0}','{1}') AND (TotalNetTransactionAmount != PaymentAmount OR TotalNetTransactionAmount = 0)", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
                }

                if (!chkIsAll.Checked)
                {
                    if (!string.IsNullOrEmpty(txtDueDateFrom.Text) && !string.IsNullOrEmpty(txtDueDateTo.Text))
                    {
                        filterExpression += string.Format(" AND DueDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtDueDateFrom.Text), Helper.GetDatePickerValue(txtDueDateTo.Text));
                    }
                    else
                    {
                        filterExpression += string.Format("DueDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtDueDateFrom.Text), Helper.GetDatePickerValue(txtDueDateTo.Text));
                    }
                }

                filterExpression += string.Format(" ORDER BY PurchaseInvoiceID");

                lstSelectedMember = hdnSelectedMember.Value.Split(',');
                lstNilai = hdnSelectedPayment.Value.Split(',');

                List<vPurchaseInvoiceHd> lst = BusinessLayer.GetvPurchaseInvoiceHdList(filterExpression);
                lvwView.DataSource = lst;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseInvoiceHd entity = e.Item.DataItem as vPurchaseInvoiceHd;
                CheckBox chkIsSelected = (CheckBox)e.Item.FindControl("chkIsSelected");
                TextBox txtBayar = (TextBox)e.Item.FindControl("txtPembayaran");
                txtBayar.Text = entity.CustomSisaHutang.ToString();

                if (lstSelectedMember.Contains(entity.PurchaseInvoiceID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedMember, entity.PurchaseInvoiceID.ToString());
                    chkIsSelected.Checked = true;
                    txtBayar.ReadOnly = false;
                    txtBayar.Attributes.Add("hiddenVal", lstNilai[idx].ToString());
                    txtBayar.Text = lstNilai[idx].ToString();
                }
            }
        }

        #region Get Constant
        protected string GetSupplierPaymentMethodTransfer()
        {
            return Constant.SupplierPaymentMethod.TRANSFER;
        }
        protected string GetSupplierPaymentMethodGiro()
        {
            return Constant.SupplierPaymentMethod.GIRO;
        }
        protected string GetSupplierPaymentMethodCheque()
        {
            return Constant.SupplierPaymentMethod.CHEQUE;
        }
        #endregion

        #region Save Edit
        public void SaveSupplierPaymentHd(IDbContext ctx, ref int SupplierPaymentID, ref string errorMessage)
        {
            SupplierPaymentHdDao entityHdDao = new SupplierPaymentHdDao(ctx);
            if (hdnSupplierPaymentID.Value == "0")
            {
                SupplierPaymentHd entityHd = new SupplierPaymentHd();
                entityHd.VerificationDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                entityHd.PlanningPaymentDate = Helper.GetDatePickerValue(txtPlanningPaymentDate.Text);
                entityHd.GCCurrencyCode = "X147^IDR";
                entityHd.Remarks = txtRemarks.Text;
                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                if (entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER ||
                    entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO ||
                    entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
                {
                    entityHd.BankID = Convert.ToInt32(cboBank.Value.ToString());
                    entityHd.BankReferenceNo = txtBankReferenceNo.Text;
                }
                else
                {
                    entityHd.BankID = null;
                }
                entityHd.StampAmount = Convert.ToDecimal(txtStampAmount.Text);

                if (hdnCounterNoPakaiTanggal.Value == "1") // pakai tanggal verifikasi
                {
                    entityHd.SupplierPaymentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION, entityHd.VerificationDate, ctx);
                }
                else if (hdnCounterNoPakaiTanggal.Value == "2") // pakai tanggal rencana bayar
                {
                    entityHd.SupplierPaymentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION, entityHd.PlanningPaymentDate, ctx);
                }
                else
                {
                    entityHd.SupplierPaymentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION, entityHd.VerificationDate, ctx);
                }

                entityHd.VerifiedBy = AppSession.UserLogin.UserID;
                entityHd.VerifiedDate = DateTime.Now;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                SupplierPaymentID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                SupplierPaymentID = Convert.ToInt32(hdnSupplierPaymentID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierPaymentHdDao entityHdDao = new SupplierPaymentHdDao(ctx);
            SupplierPaymentDtDao entityDtDao = new SupplierPaymentDtDao(ctx);
            PurchaseInvoiceHdDao entityInvoiceHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseInvoiceDtPaymentDao entityInvoiceDtPaymentDao = new PurchaseInvoiceDtPaymentDao(ctx);
            PurchaseInvoiceHdPaymentDao entityInvoiceHdPaymentDao = new PurchaseInvoiceHdPaymentDao(ctx);
            try
            {
                int SupplierPaymentID = 0;
                string errorMessage = "";
                SaveSupplierPaymentHd(ctx, ref SupplierPaymentID, ref errorMessage);

                if (String.IsNullOrEmpty(errorMessage))
                {
                    DateTime planPaymentDate = Helper.GetDatePickerValue(txtPlanningPaymentDate.Text);

                    string[] lstSelectedPurchaseInvoiceID = hdnSelectedMember.Value.Substring(1).Split(',');
                    string[] lstSelectedPayment = hdnSelectedPayment.Value.Substring(1).Split(',');
                    string[] lstSelectedSupplier = hdnSelectedSupplier.Value.Substring(1).Split(',');

                    List<PurchaseInvoiceHd> lstPurchaseInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                    List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(string.Format("PurchaseInvoiceID IN ({0}) AND LineAmount > PaymentAmount AND IsDeleted = 0", hdnSelectedMember.Value.Substring(1)), ctx);
                    for (int i = 0; i < lstSelectedPurchaseInvoiceID.Length; ++i)
                    {
                        SupplierPaymentDt entityDt = new SupplierPaymentDt();
                        entityDt.SupplierPaymentID = SupplierPaymentID;
                        entityDt.PurchaseInvoiceID = Convert.ToInt32(lstSelectedPurchaseInvoiceID[i]);
                        entityDt.BusinessPartnerID = Convert.ToInt32(lstSelectedSupplier[i]);
                        entityDt.PaymentAmount = Convert.ToDecimal(lstSelectedPayment[i]);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);

                        PurchaseInvoiceHd entityInvoice = lstPurchaseInvoiceHd.FirstOrDefault(p => p.PurchaseInvoiceID == entityDt.PurchaseInvoiceID);
                        entityInvoice.NumberOfPayment += 1;
                        entityInvoice.PaymentAmount += entityDt.PaymentAmount;
                        entityInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                        if (entityInvoice.TotalNetTransactionAmount == entityInvoice.PaymentAmount)
                        {
                            entityInvoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                        }
                        entityInvoiceHdDao.Update(entityInvoice);

                        if (planPaymentDate != DateTime.Now)
                        {
                            if (entityInvoice.DueDate < planPaymentDate)
                            {
                                planPaymentDate = entityInvoice.DueDate;
                            }
                        }

                        decimal paymentAmount = entityDt.PaymentAmount;
                        List<PurchaseInvoiceDt> lstPurchaseInvoiceDt1 = lstPurchaseInvoiceDt.Where(p => p.PurchaseInvoiceID == entityDt.PurchaseInvoiceID).ToList();
                        foreach (PurchaseInvoiceDt purchaseInvoiceDt in lstPurchaseInvoiceDt1)
                        {
                            if (paymentAmount > 0)
                            {
                                decimal tempPaymentAmount = paymentAmount;
                                decimal remainingAmount = purchaseInvoiceDt.LineAmount - purchaseInvoiceDt.PaymentAmount;
                                if (tempPaymentAmount > remainingAmount)
                                    tempPaymentAmount = remainingAmount;
                                purchaseInvoiceDt.PaymentAmount += tempPaymentAmount;
                                entityInvoiceDtDao.Update(purchaseInvoiceDt);

                                PurchaseInvoiceDtPayment entityInvoiceDtPayment = new PurchaseInvoiceDtPayment();
                                entityInvoiceDtPayment.PurchaseInvoiceDtID = purchaseInvoiceDt.ID;
                                entityInvoiceDtPayment.SupplierPaymentID = SupplierPaymentID;
                                entityInvoiceDtPayment.PaymentAmount = tempPaymentAmount;
                                entityInvoiceDtPayment.PaymentDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                                entityInvoiceDtPaymentDao.Insert(entityInvoiceDtPayment);

                                paymentAmount -= tempPaymentAmount;
                            }
                        }

                        if (paymentAmount > 0)
                        {
                            decimal tempPaymentAmount = paymentAmount;
                            decimal remainingAmount = entityInvoice.TotalNetTransactionAmount - entityInvoice.TotalTransactionAmount - entityInvoice.PaymentAmount;
                            if (tempPaymentAmount > remainingAmount)
                                tempPaymentAmount = remainingAmount;
                            PurchaseInvoiceHdPayment entityInvoiceHdPayment = new PurchaseInvoiceHdPayment();
                            entityInvoiceHdPayment.PurchaseInvoiceID = entityInvoice.PurchaseInvoiceID;
                            entityInvoiceHdPayment.SupplierPaymentID = SupplierPaymentID;
                            entityInvoiceHdPayment.PaymentAmount = tempPaymentAmount;
                            entityInvoiceHdPayment.PaymentDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                            entityInvoiceHdPaymentDao.Insert(entityInvoiceHdPayment);
                        }
                        else if (entityInvoice.TotalNetTransactionAmount < entityInvoice.TotalTransactionAmount)
                        {
                            if (entityInvoice.PaymentAmount == entityInvoice.TotalNetTransactionAmount)
                            {
                                PurchaseInvoiceHdPayment entityInvoiceHdPayment = new PurchaseInvoiceHdPayment();
                                entityInvoiceHdPayment.PurchaseInvoiceID = entityInvoice.PurchaseInvoiceID;
                                entityInvoiceHdPayment.SupplierPaymentID = SupplierPaymentID;
                                entityInvoiceHdPayment.PaymentAmount = entityInvoice.TotalNetTransactionAmount - entityInvoice.TotalTransactionAmount;
                                entityInvoiceHdPayment.PaymentDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                                entityInvoiceHdPaymentDao.Insert(entityInvoiceHdPayment);
                            }
                        }
                    }

                    SupplierPaymentHd entityHd = entityHdDao.Get(SupplierPaymentID);
                    entityHd.PlanningPaymentDate = planPaymentDate;
                    entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entityHd);

                    retval = SupplierPaymentID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = errorMessage;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                SupplierPaymentHd entity = BusinessLayer.GetSupplierPaymentHd(Convert.ToInt32(hdnSupplierPaymentID.Value));

                bool isTransactionLocked = false;
                TransactionTypeLock entityLock = BusinessLayer.GetTransactionTypeLock(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.VerificationDate;
                    if (DateNow > DateLock)
                    {
                        isTransactionLocked = false;
                    }
                    else
                    {
                        isTransactionLocked = true;
                    }
                }
                else
                {
                    isTransactionLocked = false;
                }

                if (!isTransactionLocked)
                {
                    entity.VerificationDate = Helper.GetDatePickerValue(txtVerificationDate.Text);
                    entity.PlanningPaymentDate = Helper.GetDatePickerValue(txtPlanningPaymentDate.Text);
                    entity.GCCurrencyCode = "X147^IDR";
                    //entity.BusinessPartnerID = AppSession.BusinessPartnerID;
                    entity.Remarks = txtRemarks.Text;
                    entity.ReferenceNo = txtReferenceNo.Text;
                    entity.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                    if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER ||
                        entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO ||
                        entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
                    {
                        entity.BankID = Convert.ToInt32(cboBank.Value.ToString());
                        entity.BankReferenceNo = txtBankReferenceNo.Text;
                    }
                    else
                    {
                        entity.BankID = null;
                    }
                    entity.StampAmount = Convert.ToDecimal(txtStampAmount.Text);
                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    BusinessLayer.UpdateSupplierPaymentHd(entity);
                    return true;
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierPaymentHdDao entityDao = new SupplierPaymentHdDao(ctx);
            PurchaseInvoiceHdDao invoiceHdDao = new PurchaseInvoiceHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                SupplierPaymentHd entity = BusinessLayer.GetSupplierPaymentHd(Convert.ToInt32(hdnSupplierPaymentID.Value));

                bool isTransactionLocked = false;
                TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION);
                if (entityLock.LockedUntilDate != null)
                {
                    DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                    DateTime DateNow = entity.VerificationDate;
                    if (DateNow > DateLock)
                    {
                        isTransactionLocked = false;
                    }
                    else
                    {
                        isTransactionLocked = true;
                    }
                }
                else
                {
                    isTransactionLocked = false;
                }

                if (!isTransactionLocked)
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        entity.PlanningPaymentDate = Helper.GetDatePickerValue(txtPlanningPaymentDate.Text);
                        entity.GCCurrencyCode = "X147^IDR";
                        //entity.BusinessPartnerID = AppSession.BusinessPartnerID;
                        entity.Remarks = txtRemarks.Text;
                        entity.ReferenceNo = txtReferenceNo.Text;
                        entity.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
                        entity.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                        if (entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER ||
                            entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO ||
                            entity.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
                        {
                            entity.BankID = Convert.ToInt32(cboBank.Value.ToString());
                            entity.BankReferenceNo = txtBankReferenceNo.Text;
                        }
                        else
                        {
                            entity.BankID = null;
                        }
                        entity.VerifiedBy = AppSession.UserLogin.UserID;
                        entity.VerifiedDate = DateTime.Now;
                        entity.StampAmount = Convert.ToDecimal(txtStampAmount.Text);
                        entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        if (hdnUsingApprovalVerification.Value == "1")
                        {
                            entity.GCApprovalTransactionStatus = Constant.TransactionStatus.OPEN;
                        }
                        else
                        {
                            entity.GCApprovalTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entity.ApprovalApprovedBy = AppSession.UserLogin.UserID;
                            entity.ApprovalApprovedDate = DateTime.Now;
                        }
                        entityDao.Update(entity);

                        List<SupplierPaymentDt> lstEntityDt = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", hdnSupplierPaymentID.Value), ctx);
                        foreach (SupplierPaymentDt paymentDt in lstEntityDt)
                        {
                            PurchaseInvoiceHd invoiceHd = invoiceHdDao.Get(paymentDt.PurchaseInvoiceID);
                            if (paymentDt.PaymentAmount == invoiceHd.TotalNetTransactionAmount)
                            {
                                invoiceHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            }
                            invoiceHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            invoiceHdDao.Update(invoiceHd);
                        }

                        //List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format(
                        //    "PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", hdnSupplierPaymentID.Value));
                        //foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                        //{
                        //    if (invoice.TotalNetTransactionAmount == 0)
                        //    {
                        //        invoice.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        //    }
                        //    invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //    invoiceHdDao.Update(invoice);
                        //}
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        errMessage = "Hutang supplier tidak dapat diubah. Harap refresh halaman ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        result = false;
                        ctx.RollBackTransaction();
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

        #region Custom Button Click
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao purchaseInvoiceHDDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao purchaseInvoiceDTDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseOrderHdDao orderHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao orderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseOrderTermDao poTermDao = new PurchaseOrderTermDao(ctx);
            PurchaseReceiveHdDao purchaseReceiveHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao purchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            SupplierCreditNoteDao entityCNDao = new SupplierCreditNoteDao(ctx);
            TestPartnerTransactionHdDao entityTPTHdDao = new TestPartnerTransactionHdDao(ctx);
            SupplierPaymentHdDao supplierPaymentHdDao = new SupplierPaymentHdDao(ctx);
            TransactionTypeLockDao entityLockDao = new TransactionTypeLockDao(ctx);

            try
            {
                if (type.Contains("justvoid") || type.Contains("reopenpayment"))
                {
                    #region Supplier Payment

                    SupplierPaymentHd entity = supplierPaymentHdDao.Get(Convert.ToInt32(hdnSupplierPaymentID.Value));

                    bool isTransactionLocked = false;
                    TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION);
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entity.VerificationDate;
                        if (DateNow > DateLock)
                        {
                            isTransactionLocked = false;
                        }
                        else
                        {
                            isTransactionLocked = true;
                        }
                    }
                    else
                    {
                        isTransactionLocked = false;
                    }

                    if (!isTransactionLocked)
                    {
                        if (type.Contains("justvoid"))
                        {
                            #region Void by Reason

                            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                            {
                                string[] param = type.Split(';');
                                string gcDeleteReason = param[1];
                                string reason = param[2];

                                List<SupplierPaymentDt> lstPaymentDt = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", entity.SupplierPaymentID), ctx);
                                foreach (SupplierPaymentDt paymentDt in lstPaymentDt)
                                {
                                    PurchaseInvoiceHd pInvoice = purchaseInvoiceHDDao.Get(paymentDt.PurchaseInvoiceID);
                                    if (pInvoice.GCTransactionStatus == Constant.TransactionStatus.APPROVED || pInvoice.GCTransactionStatus == Constant.TransactionStatus.PROCESSED && result == true)
                                    {
                                        pInvoice.NumberOfPayment -= 1;
                                        pInvoice.PaymentAmount -= paymentDt.PaymentAmount;

                                        if (pInvoice.PaymentAmount == 0)
                                        {
                                            pInvoice.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                        }
                                        else
                                        {
                                            pInvoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        }

                                        pInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseInvoiceHDDao.Update(pInvoice);


                                        List<PurchaseInvoiceDtPayment> lstPurchaseInvoiceDtPayment = BusinessLayer.GetPurchaseInvoiceDtPaymentList(string.Format("SupplierPaymentID = {0}", paymentDt.SupplierPaymentID), ctx);
                                        foreach (PurchaseInvoiceDtPayment purchaseInvoiceDtPayment in lstPurchaseInvoiceDtPayment)
                                        {
                                            PurchaseInvoiceDt pInvoiceDt = purchaseInvoiceDTDao.Get(purchaseInvoiceDtPayment.PurchaseInvoiceDtID);
                                            pInvoiceDt.PaymentAmount -= paymentDt.PaymentAmount;
                                            pInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            purchaseInvoiceDTDao.Update(pInvoiceDt);
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        break;
                                    }
                                }

                                if (result)
                                {

                                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    entity.GCVoidReason = gcDeleteReason;
                                    entity.VoidReason = reason;
                                    entity.VoidBy = AppSession.UserLogin.UserID;
                                    entity.VoidDate = DateTime.Now;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    supplierPaymentHdDao.Update(entity);
                                    retval = entity.SupplierPaymentNo;

                                    ctx.CommitTransaction();
                                }
                                else
                                {
                                    errMessage = "Hutang supplier tidak dapat diubah. Harap refresh halaman ini.";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = string.Format("Hutang supplier tidak dapat diubah. Harap refresh halaman ini.");
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }

                            #endregion
                        }
                        else if (type.Contains("reopenpayment"))
                        {
                            #region Reopen Payment

                            if (hdnUsingApprovalVerification.Value == "1")
                            {
                                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL && entity.GCApprovalTransactionStatus == Constant.TransactionStatus.OPEN)
                                {

                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.GCApprovalTransactionStatus = null;
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    supplierPaymentHdDao.Update(entity);

                                    List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", entity.SupplierPaymentID), ctx);
                                    foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                                    {
                                        invoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                        invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        purchaseInvoiceHDDao.Update(invoice);
                                    }

                                    retval = entity.SupplierPaymentNo;

                                    ctx.CommitTransaction();
                                }
                                else
                                {
                                    result = false;
                                    errMessage = string.Format("Hutang supplier tidak dapat diubah. Harap refresh halaman ini.");
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                    ctx.RollBackTransaction();
                                }
                            }
                            else
                            {
                                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entity.GCApprovalTransactionStatus = null;
                                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                supplierPaymentHdDao.Update(entity);

                                List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", entity.SupplierPaymentID), ctx);
                                foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                                {
                                    invoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                                    invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseInvoiceHDDao.Update(invoice);
                                }

                                retval = entity.SupplierPaymentNo;

                                ctx.CommitTransaction();
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
                    #endregion
                }
                else if (type.Contains("unpaid"))
                {
                    #region Supplier Payment
                    SupplierPaymentHd entity = supplierPaymentHdDao.Get(Convert.ToInt32(hdnSupplierPaymentID.Value));

                    bool isTransactionLocked = false;
                    TransactionTypeLock entityLock = entityLockDao.Get(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION);
                    if (entityLock.LockedUntilDate != null)
                    {
                        DateTime DateLock = Convert.ToDateTime(entityLock.LockedUntilDate);
                        DateTime DateNow = entity.VerificationDate;
                        if (DateNow > DateLock)
                        {
                            isTransactionLocked = false;
                        }
                        else
                        {
                            isTransactionLocked = true;
                        }
                    }
                    else
                    {
                        isTransactionLocked = false;
                    }

                    if (!isTransactionLocked)
                    {
                        #region Un Paid

                        if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            supplierPaymentHdDao.Update(entity);

                            //retval = entity.SupplierPaymentNo;

                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Hutang supplier tidak dapat diubah. Harap refresh halaman ini.");
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }

                        #endregion
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("This Transaction Type has been Locked until {0}. Please contact an authorized personnel.", Convert.ToDateTime(entityLock.LockedUntilDate).ToString(Constant.FormatString.DATE_FORMAT));
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    #endregion
                }
                else
                {
                    #region Purchase Invoice

                    string filterPymnt = string.Format("PurchaseInvoiceID IN ({0}) AND SupplierPaymentID IN (SELECT SupplierPaymentID FROM SupplierPaymentHd WHERE GCTransactionStatus != '{1}')",
                                                            hdnSelectedMember.Value.Substring(1), Constant.TransactionStatus.VOID);
                    List<SupplierPaymentDt> lstPaymentDt = BusinessLayer.GetSupplierPaymentDtList(filterPymnt, ctx);
                    if (lstPaymentDt.Count() == 0)
                    {
                        if (type.Contains("closenew"))
                        {
                            #region Close & New

                            List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                            foreach (PurchaseInvoiceHd invoiceHD in lstInvoiceHd)
                            {
                                if (invoiceHD.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                                {
                                    //VOID HD OLD
                                    invoiceHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    invoiceHD.GCVoidReason = Constant.DeleteReason.OTHER;
                                    invoiceHD.VoidReason = "CLOSE & NEW";
                                    invoiceHD.VoidBy = AppSession.UserLogin.UserID;
                                    invoiceHD.VoidDate = DateTime.Now;
                                    invoiceHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseInvoiceHDDao.Update(invoiceHD);

                                    //RE-INSERT
                                    PurchaseInvoiceHd newInvoiceHd = new PurchaseInvoiceHd();
                                    newInvoiceHd.PurchaseInvoiceDate = invoiceHD.PurchaseInvoiceDate;
                                    newInvoiceHd.DueDate = invoiceHD.DueDate;
                                    newInvoiceHd.DocumentDate = invoiceHD.DocumentDate;
                                    newInvoiceHd.GCCurrencyCode = invoiceHD.GCCurrencyCode;
                                    newInvoiceHd.GCChargesType = invoiceHD.GCChargesType;
                                    newInvoiceHd.BusinessPartnerID = invoiceHD.BusinessPartnerID;
                                    newInvoiceHd.ProductLineID = invoiceHD.ProductLineID;
                                    newInvoiceHd.Remarks = invoiceHD.Remarks;
                                    newInvoiceHd.VATPercentage = invoiceHD.VATPercentage;
                                    newInvoiceHd.IsFinalDiscountInPercentage = invoiceHD.IsFinalDiscountInPercentage;
                                    newInvoiceHd.FinalDiscount = invoiceHD.FinalDiscount;
                                    newInvoiceHd.FinalDiscountAmount = invoiceHD.FinalDiscountAmount;
                                    newInvoiceHd.IsPPHInPercentage = invoiceHD.IsPPHInPercentage;
                                    newInvoiceHd.PPHMode = invoiceHD.PPHMode;
                                    newInvoiceHd.GCPPHType = invoiceHD.GCPPHType;
                                    newInvoiceHd.PPHPercentage = invoiceHD.PPHPercentage;
                                    newInvoiceHd.PPHAmount = invoiceHD.PPHAmount;
                                    newInvoiceHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    newInvoiceHd.StampAmount = invoiceHD.StampAmount;
                                    newInvoiceHd.ChargesAmount = invoiceHD.ChargesAmount;
                                    //newInvoiceHd.TotalNetTransactionAmount = invoiceHD.TotalNetTransactionAmount;
                                    newInvoiceHd.ReferenceNo = invoiceHD.PurchaseInvoiceNo;
                                    newInvoiceHd.PurchaseInvoiceNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_INVOICE, invoiceHD.PurchaseInvoiceDate, ctx);
                                    newInvoiceHd.CreatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    newInvoiceHd.PurchaseInvoiceID = purchaseInvoiceHDDao.InsertReturnPrimaryKeyID(newInvoiceHd);

                                    string filterDt = string.Format("PurchaseInvoiceID = {0} AND IsDeleted = 0", invoiceHD.PurchaseInvoiceID);
                                    List<PurchaseInvoiceDt> lstDt = BusinessLayer.GetPurchaseInvoiceDtList(filterDt, ctx);
                                    foreach (PurchaseInvoiceDt invoiceDT in lstDt)
                                    {
                                        PurchaseInvoiceDt newInvoiceDt = new PurchaseInvoiceDt();
                                        newInvoiceDt.PurchaseInvoiceID = newInvoiceHd.PurchaseInvoiceID;
                                        if (invoiceDT.PurchaseReceiveID != null && invoiceDT.PurchaseReceiveID != 0)
                                        {
                                            newInvoiceDt.PurchaseReceiveID = invoiceDT.PurchaseReceiveID;
                                        }
                                        if (invoiceDT.PurchaseOrderID != null && invoiceDT.PurchaseOrderID != 0)
                                        {
                                            newInvoiceDt.PurchaseOrderID = invoiceDT.PurchaseOrderID;
                                        }
                                        if (invoiceDT.PurchaseOrderTermID != null && invoiceDT.PurchaseOrderTermID != 0)
                                        {
                                            newInvoiceDt.PurchaseOrderTermID = invoiceDT.PurchaseOrderTermID;
                                        }
                                        if (invoiceDT.CreditNoteID != null && invoiceDT.CreditNoteID != 0)
                                        {
                                            newInvoiceDt.CreditNoteID = invoiceDT.CreditNoteID;
                                        }
                                        if (invoiceDT.TestPartnerTransactionID != null && invoiceDT.TestPartnerTransactionID != 0)
                                        {
                                            newInvoiceDt.TestPartnerTransactionID = invoiceDT.TestPartnerTransactionID;
                                        }
                                        newInvoiceDt.TransactionAmount = invoiceDT.TransactionAmount;
                                        newInvoiceDt.FinalDiscountAmount = invoiceDT.FinalDiscountAmount;
                                        newInvoiceDt.VATAmount = invoiceDT.VATAmount;
                                        newInvoiceDt.ChargesAmount = invoiceDT.ChargesAmount;
                                        newInvoiceDt.StampAmount = invoiceDT.StampAmount;
                                        newInvoiceDt.DownPaymentAmount = invoiceDT.DownPaymentAmount;
                                        newInvoiceDt.CreditNoteAmount = invoiceDT.CreditNoteAmount;
                                        newInvoiceDt.TaxInvoiceNo = invoiceDT.TaxInvoiceNo;
                                        newInvoiceDt.TaxInvoiceDate = invoiceDT.TaxInvoiceDate;
                                        newInvoiceDt.ProductLineID = invoiceDT.ProductLineID;
                                        newInvoiceDt.ReferenceNo = invoiceDT.ReferenceNo;
                                        newInvoiceDt.ReferenceDate = invoiceDT.ReferenceDate;
                                        newInvoiceDt.LineAmount = invoiceDT.LineAmount;
                                        newInvoiceDt.PaymentAmount = invoiceDT.PaymentAmount;
                                        newInvoiceDt.RoundingAmount = invoiceDT.RoundingAmount;
                                        newInvoiceDt.CreatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseInvoiceDTDao.Insert(newInvoiceDt);

                                        //VOID DT OLD
                                        invoiceDT.IsDeleted = true;
                                        invoiceDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseInvoiceDTDao.Update(invoiceDT);

                                        if (invoiceDT.PurchaseOrderID != 0 && invoiceDT.PurchaseOrderID != null)
                                        {
                                            PurchaseOrderHd purchaseOrderHd = orderHdDao.Get(Convert.ToInt32(invoiceDT.PurchaseOrderID));
                                            if (purchaseOrderHd.TransactionCode == Constant.TransactionCode.CONSIGNMENT_ORDER)
                                            {
                                                purchaseOrderHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                purchaseOrderHd.ApprovedBy = AppSession.UserLogin.UserID;
                                                purchaseOrderHd.ApprovedDate = DateTime.Now;
                                                purchaseOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                orderHdDao.Update(purchaseOrderHd);

                                                List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", purchaseOrderHd.PurchaseOrderID, Constant.TransactionStatus.VOID), ctx);
                                                foreach (PurchaseOrderDt orderDt in lstPurchaseOrderDt)
                                                {
                                                    orderDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                    orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    orderDtDao.Update(orderDt);
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result = false;
                                    break;
                                }
                            }

                            if (result)
                            {
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = "Proses hutang tidak dapat diubah. Harap refresh halaman ini.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }

                            #endregion
                        }
                        else if (type.Contains("closevoid"))
                        {
                            #region Close & Void

                            List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                            foreach (PurchaseInvoiceHd invoiceHD in lstInvoiceHd)
                            {
                                string[] param = type.Split(';');
                                string gcDeleteReason = param[1];
                                string reason = param[2];

                                if (invoiceHD.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                                {
                                    invoiceHD.GCTransactionStatus = Constant.TransactionStatus.VOID;
                                    invoiceHD.GCVoidReason = gcDeleteReason;
                                    invoiceHD.VoidReason = reason;
                                    invoiceHD.VoidBy = AppSession.UserLogin.UserID;
                                    invoiceHD.VoidDate = DateTime.Now;
                                    invoiceHD.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseInvoiceHDDao.Update(invoiceHD);

                                    string filterDt = string.Format("PurchaseInvoiceID = {0} AND IsDeleted = 0", invoiceHD.PurchaseInvoiceID);
                                    List<PurchaseInvoiceDt> lstDt = BusinessLayer.GetPurchaseInvoiceDtList(filterDt, ctx);
                                    foreach (PurchaseInvoiceDt invoiceDT in lstDt)
                                    {
                                        invoiceDT.IsDeleted = true;
                                        invoiceDT.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseInvoiceDTDao.Update(invoiceDT);

                                        if (invoiceDT.PurchaseOrderTermID != 0 && invoiceDT.PurchaseOrderTermID != null)
                                        {
                                            PurchaseOrderTerm poTerm = poTermDao.Get(Convert.ToInt32(invoiceDT.PurchaseOrderTermID));
                                            if (poTerm.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                            {
                                                poTerm.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                poTerm.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                poTermDao.Update(poTerm);
                                            }
                                        }
                                        else if (invoiceDT.TestPartnerTransactionID != 0 && invoiceDT.TestPartnerTransactionID != null)
                                        {
                                            TestPartnerTransactionHd tptHd = entityTPTHdDao.Get(Convert.ToInt32(invoiceDT.TestPartnerTransactionID));
                                            if (tptHd.GCTransactionStatus != Constant.TransactionStatus.VOID)
                                            {
                                                tptHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                tptHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                entityTPTHdDao.Update(tptHd);
                                            }
                                        }
                                        else
                                        {
                                            if (invoiceDT.PurchaseOrderID != 0 && invoiceDT.PurchaseOrderID != null)
                                            {
                                                PurchaseOrderHd purchaseOrderHd = orderHdDao.Get(Convert.ToInt32(invoiceDT.PurchaseOrderID));
                                                if (purchaseOrderHd.TransactionCode == Constant.TransactionCode.CONSIGNMENT_ORDER)
                                                {
                                                    purchaseOrderHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                    purchaseOrderHd.ApprovedBy = AppSession.UserLogin.UserID;
                                                    purchaseOrderHd.ApprovedDate = DateTime.Now;
                                                    purchaseOrderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    orderHdDao.Update(purchaseOrderHd);

                                                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", purchaseOrderHd.PurchaseOrderID, Constant.TransactionStatus.VOID), ctx);
                                                    foreach (PurchaseOrderDt orderDt in lstPurchaseOrderDt)
                                                    {
                                                        orderDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                        orderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                        ctx.CommandType = CommandType.Text;
                                                        ctx.Command.Parameters.Clear();
                                                        orderDtDao.Update(orderDt);
                                                    }
                                                }
                                            }

                                            if (invoiceDT.PurchaseReceiveID != 0 && invoiceDT.PurchaseReceiveID != null)
                                            {
                                                PurchaseReceiveHd entityPR = purchaseReceiveHdDao.Get(Convert.ToInt32(invoiceDT.PurchaseReceiveID));
                                                entityPR.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                entityPR.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                purchaseReceiveHdDao.Update(entityPR);

                                                List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format(
                                                                                                                "PurchaseReceiveID = ({0}) AND GCItemDetailStatus != '{1}'",
                                                                                                                Convert.ToInt32(entityPR.PurchaseReceiveID), Constant.TransactionStatus.VOID), ctx);
                                                foreach (PurchaseReceiveDt purchaseReceiveDt in lstPurchaseReceiveDt)
                                                {
                                                    purchaseReceiveDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                    purchaseReceiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    purchaseReceiveDtDao.Update(purchaseReceiveDt);
                                                }
                                            }

                                            if (invoiceDT.CreditNoteID != 0 && invoiceDT.CreditNoteID != null)
                                            {
                                                SupplierCreditNote entityCN = entityCNDao.Get(Convert.ToInt32(invoiceDT.CreditNoteID));
                                                entityCN.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityCNDao.Update(entityCN);
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    result = false;
                                    break;
                                }
                            }

                            if (result)
                            {
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = "Proses hutang tidak dapat diubah. Harap refresh halaman ini.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                            }

                            #endregion
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Proses hutang tidak dapat diubah karena sudah ada pembayaran ke supplier";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }

                    #endregion
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