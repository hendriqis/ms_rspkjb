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
    public partial class APInvoiceSupplierPayment : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_INVOICE_SUPPLIER_VERIFICATION_TO_PAYMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected String IsAdd()
        {
            return hdnIsAdd.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnIsAdd.Value = "1";

            cboPaymentMethod.SelectedIndex = 0;
            Helper.SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, false), "mpEntry");
            Helper.SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(true, false, false), "mpEntry");

            txtPaymentDate.Text = txtReferenceDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView();
        }

        public override void OnAddRecord()
        {
            hdnIsAdd.Value = "1";
            IsAdd();
            BindGridView();
            trBank.Attributes.Add("style", "display:none");
            trBankRef.Attributes.Add("style", "display:none");
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.SUPPLIER_PAYMENT_METHOD));
            List<Bank> listBank = BusinessLayer.GetBankList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<StandardCode>(cboPaymentMethod, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.SUPPLIER_PAYMENT_METHOD).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboPaymentMethod.SelectedIndex = 0;
            Methods.SetComboBoxField<Bank>(cboBank, listBank, "BankName", "BankID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnTransactionStatus, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnSupplierPaymentID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnIsAdd, new ControlEntrySetting(false, false, false, "1"));
            SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(true, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBank, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBankReferenceNo, new ControlEntrySetting(true, true, true));
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvSupplierPaymentHdRowCount(filterExpression);
        }

        public string GetFilterExpression()
        {
            string filterExpression = String.Format("BusinessPartnerID = {0}", AppSession.BusinessPartnerID);
            return filterExpression;
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = GetFilterExpression();
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHd(filterExpression, PageIndex, "SupplierPaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            hdnIsAdd.Value = "0";
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvSupplierPaymentHdRowIndex(filterExpression, keyValue, "SupplierPaymentID DESC");
            vSupplierPaymentHd entity = BusinessLayer.GetvSupplierPaymentHd(filterExpression, PageIndex, "SupplierPaymentID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vSupplierPaymentHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL || entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                {
                    SetControlEntrySetting(txtPaymentDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
                    SetControlEntrySetting(cboPaymentMethod, new ControlEntrySetting(false, false, true));
                    SetControlEntrySetting(cboBank, new ControlEntrySetting(false, false, true));
                    SetControlEntrySetting(txtBankReferenceNo, new ControlEntrySetting(false, false, true));
                }
            }
            hdnSupplierPaymentID.Value = entity.SupplierPaymentID.ToString();
            txtPaymentNo.Text = entity.SupplierPaymentNo;
            txtPaymentDate.Text = entity.VerificationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtRemarks.Text = entity.Remarks;
            cboPaymentMethod.Value = entity.GCSupplierPaymentMethod;
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

            List<SupplierPaymentDt> dt = BusinessLayer.GetSupplierPaymentDtList(string.Format("SupplierPaymentID = {0}", entity.SupplierPaymentID));
            decimal total = 0;
            foreach (SupplierPaymentDt lst in dt)
            {
                total += lst.PaymentAmount;
            }
            txtVerificationAmount.Text = total.ToString("N2");
            hdnTransactionStatus.Value = entity.GCTransactionStatus;
            BindGridView();
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnIsAdd.Value == "0")
            {
                if (hdnSupplierPaymentID.Value != "" && hdnSupplierPaymentID.Value != "0")
                {
                    filterExpression = string.Format("SupplierPaymentID = {0} AND BusinessPartnerID = {1}", hdnSupplierPaymentID.Value, AppSession.BusinessPartnerID);
                    List<vPurchaseInvoiceHdPayment> lstEntity = BusinessLayer.GetvPurchaseInvoiceHdPaymentList(filterExpression);
                    grdView.DataSource = lstEntity;
                    grdView.DataBind();
                }
            }
            else
            {
                filterExpression = string.Format("BusinessPartnerID = {0} AND GCTransactionStatus = '{1}'", AppSession.BusinessPartnerID, Constant.TransactionStatus.APPROVED);
                List<vPurchaseInvoiceHd> lst = BusinessLayer.GetvPurchaseInvoiceHdList(filterExpression);
                //lvwView.DataSource = lst.Where(p => p.CustomSisaHutang > 0);
                lvwView.DataSource = lst;
                lvwView.DataBind();
            }
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseInvoiceHd entity = e.Item.DataItem as vPurchaseInvoiceHd;
                TextBox txtBayar = (TextBox)e.Item.FindControl("txtPembayaran");
                txtBayar.Text = Convert.ToDecimal(entity.CustomSisaHutang).ToString();
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
        public void SaveSupplierPaymentHd(IDbContext ctx, ref int SupplierPaymentID)
        {
            SupplierPaymentHdDao entityHdDao = new SupplierPaymentHdDao(ctx);
            if (hdnSupplierPaymentID.Value == "0")
            {
                SupplierPaymentHd entityHd = new SupplierPaymentHd();
                entityHd.VerificationDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
                entityHd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);

                entityHd.GCCurrencyCode = "X147^IDR";
                entityHd.BusinessPartnerID = AppSession.BusinessPartnerID;
                entityHd.Remarks = txtRemarks.Text;

                entityHd.ReferenceNo = txtReferenceNo.Text;
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
                entityHd.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                if (entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.TRANSFER ||
                    entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.GIRO ||
                    entityHd.GCSupplierPaymentMethod == Constant.SupplierPaymentMethod.CHEQUE)
                {
                    entityHd.BankID = Convert.ToInt32(cboBank.Value.ToString());
                    entityHd.BankReferenceNo = txtBankReferenceNo.Text;
                }
                entityHd.SupplierPaymentNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SUPPLIER_PAYMENT_VERIFICATION, entityHd.VerificationDate, ctx);

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.VerifiedBy = AppSession.UserLogin.UserID;
                entityHd.VerifiedDate = DateTime.Now;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entityHd);
                SupplierPaymentID = BusinessLayer.GetSupplierPaymentHdMaxID(ctx);
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
            SupplierPaymentDtDao entityDtDao = new SupplierPaymentDtDao(ctx);
            PurchaseInvoiceHdDao entityInvoiceHdDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao entityInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            PurchaseInvoiceDtPaymentDao entityInvoiceDtPaymentDao = new PurchaseInvoiceDtPaymentDao(ctx);
            PurchaseInvoiceHdPaymentDao entityInvoiceHdPaymentDao = new PurchaseInvoiceHdPaymentDao(ctx);
            try
            {
                int SupplierPaymentID = 0;
                SaveSupplierPaymentHd(ctx, ref SupplierPaymentID);

                string[] lstSelectedPurchaseInvoiceID = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedPayment = hdnSelectedPayment.Value.Split(',');

                List<PurchaseInvoiceHd> lstPurchaseInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value));
                List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(string.Format("PurchaseInvoiceID IN ({0}) AND LineAmount > PaymentAmount AND IsDeleted = 0", hdnSelectedMember.Value));
                for (int i = 0; i < lstSelectedPurchaseInvoiceID.Length; ++i)
                {
                    SupplierPaymentDt entityDt = new SupplierPaymentDt();
                    entityDt.SupplierPaymentID = SupplierPaymentID;
                    entityDt.PurchaseInvoiceID = Convert.ToInt32(lstSelectedPurchaseInvoiceID[i]);
                    entityDt.PaymentAmount = Convert.ToDecimal(lstSelectedPayment[i]);
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    PurchaseInvoiceHd entityInvoice = lstPurchaseInvoiceHd.FirstOrDefault(p => p.PurchaseInvoiceID == entityDt.PurchaseInvoiceID);
                    entityInvoice.NumberOfPayment += 1;
                    entityInvoice.PaymentAmount += entityDt.PaymentAmount;
                    entityInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                    if (entityInvoice.TotalNetTransactionAmount == entityInvoice.PaymentAmount)
                        entityInvoice.GCTransactionStatus = Constant.TransactionStatus.PROCESSED;
                    entityInvoiceHdDao.Update(entityInvoice);

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
                            entityInvoiceDtPayment.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
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
                        entityInvoiceHdPayment.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
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
                            entityInvoiceHdPayment.PaymentDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
                            entityInvoiceHdPaymentDao.Insert(entityInvoiceHdPayment);
                        }
                    }
                }

                retval = SupplierPaymentID.ToString();
                ctx.CommitTransaction();
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                SupplierPaymentHd entity = BusinessLayer.GetSupplierPaymentHd(Convert.ToInt32(hdnSupplierPaymentID.Value));
                entity.VerificationDate = Helper.GetDatePickerValue(txtPaymentDate.Text);
                entity.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);

                entity.GCCurrencyCode = "X147^IDR";
                entity.BusinessPartnerID = AppSession.BusinessPartnerID;
                entity.Remarks = txtRemarks.Text;

                entity.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                entity.BankID = Convert.ToInt32(cboBank.Value.ToString());
                entity.ReferenceNo = txtReferenceNo.Text;
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
                if (cboPaymentMethod.Value.ToString() == Constant.SupplierPaymentMethod.TRANSFER ||
                    cboPaymentMethod.Value.ToString() == Constant.SupplierPaymentMethod.GIRO ||
                    cboPaymentMethod.Value.ToString() == Constant.SupplierPaymentMethod.CHEQUE)
                {
                    entity.BankID = Convert.ToInt32(cboBank.Value.ToString());
                    entity.BankReferenceNo = txtBankReferenceNo.Text;
                }
                entity.GCSupplierPaymentMethod = cboPaymentMethod.Value.ToString();
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                BusinessLayer.UpdateSupplierPaymentHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Custom Button Click
        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            if (type == "proposed")
            {

                IDbContext ctx = DbFactory.Configure(true);
                SupplierPaymentHdDao entityDao = new SupplierPaymentHdDao(ctx);
                PurchaseInvoiceHdDao invoiceHdDao = new PurchaseInvoiceHdDao(ctx);
                try
                {
                    SupplierPaymentHd entity = BusinessLayer.GetSupplierPaymentHd(Convert.ToInt32(hdnSupplierPaymentID.Value));
                    entity.VerifiedBy = AppSession.UserLogin.UserID;
                    entity.VerifiedDate = DateTime.Now;
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);

                    List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format(
                        "PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", hdnSupplierPaymentID.Value));
                    foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                    {
                        invoice.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                        invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                        invoiceHdDao.Update(invoice);
                    }
                    retval = entity.SupplierPaymentNo;
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            else if (type == "decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseInvoiceHdDao purchaseInvoiceHDDao = new PurchaseInvoiceHdDao(ctx);
                PurchaseReceiveHdDao purchaseReceiveHdDao = new PurchaseReceiveHdDao(ctx);
                SupplierCreditNoteDao entityCNDao = new SupplierCreditNoteDao(ctx);
                try
                {
                    List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format(
                        "PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value));
                    foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                    {
                        invoice.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        invoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseInvoiceHDDao.Update(invoice);

                        List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(String.Format(
                            "PurchaseInvoiceID = {0} AND IsDeleted = 0", invoice.PurchaseInvoiceID), ctx);
                        foreach (PurchaseInvoiceDt purchaseInvoiceDt in lstPurchaseInvoiceDt)
                        {
                            if (purchaseInvoiceDt.PurchaseReceiveID != 0 && purchaseInvoiceDt.PurchaseReceiveID != null)
                            {
                                PurchaseReceiveHd entityPR = purchaseReceiveHdDao.Get(Convert.ToInt32(purchaseInvoiceDt.PurchaseReceiveID));
                                entityPR.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                entityPR.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseReceiveHdDao.Update(entityPR);
                            }

                            if (purchaseInvoiceDt.CreditNoteID != 0 && purchaseInvoiceDt.CreditNoteID != null)
                            {
                                SupplierCreditNote entityCN = entityCNDao.Get(Convert.ToInt32(purchaseInvoiceDt.CreditNoteID));
                                entityCN.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                entityCN.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityCNDao.Update(entityCN);
                            }
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }
        #endregion

        #region Void Entity
        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceHdDao pinvoiceDao = new PurchaseInvoiceHdDao(ctx);
            PurchaseInvoiceDtDao pinvoiceDtDao = new PurchaseInvoiceDtDao(ctx);
            SupplierPaymentHdDao supplierPaymentHdDao = new SupplierPaymentHdDao(ctx);
            try
            {
                //string filterExpression = string.Format("SupplierPaymentID = {0}", hdnSupplierPaymentID.Value);
                //List<vPurchaseInvoiceHdPayment> lstEntity = BusinessLayer.GetvPurchaseInvoiceHdPaymentList(filterExpression, ctx);
                //foreach (vPurchaseInvoiceHdPayment purchaseInvoiceHdPayment in lstEntity)
                //{
                List<PurchaseInvoiceHd> lstInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(string.Format(
                    "PurchaseInvoiceID IN (SELECT PurchaseInvoiceID FROM SupplierPaymentDt WHERE SupplierPaymentID = {0})", hdnSupplierPaymentID.Value));
                foreach (PurchaseInvoiceHd invoice in lstInvoiceHd)
                {
                    PurchaseInvoiceHd pInvoice = BusinessLayer.GetPurchaseInvoiceHd(invoice.PurchaseInvoiceID);
                    pInvoice.NumberOfPayment -= 1;
                    pInvoice.PaymentAmount -= invoice.PaymentAmount;
                    pInvoice.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    pInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                    pinvoiceDao.Update(pInvoice);
                }

                List<PurchaseInvoiceDtPayment> lstPurchaseInvoiceDtPayment = BusinessLayer.GetPurchaseInvoiceDtPaymentList(string.Format("SupplierPaymentID = {0}", hdnSupplierPaymentID.Value), ctx);
                foreach (PurchaseInvoiceDtPayment purchaseInvoiceDtPayment in lstPurchaseInvoiceDtPayment)
                {
                    PurchaseInvoiceDt pInvoice = BusinessLayer.GetPurchaseInvoiceDt(purchaseInvoiceDtPayment.PurchaseInvoiceDtID);
                    pInvoice.PaymentAmount -= purchaseInvoiceDtPayment.PaymentAmount;
                    pInvoice.LastUpdatedBy = AppSession.UserLogin.UserID;
                    pinvoiceDtDao.Update(pInvoice);
                }

                SupplierPaymentHd entity = supplierPaymentHdDao.Get(Convert.ToInt32(hdnSupplierPaymentID.Value));
                entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                supplierPaymentHdDao.Update(entity);
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
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