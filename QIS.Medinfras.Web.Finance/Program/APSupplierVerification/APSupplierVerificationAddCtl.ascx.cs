using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APSupplierVerificationAddCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedPurchaseInvoiceID = null;
        private string[] lstNilai = null;

        private APSupplierVerification DetailPage
        {
            get { return (APSupplierVerification)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnTransactionID.Value = temp[0];
            
            txtDueDateFrom.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDueDateTo.Text = DateTime.Today.AddDays(1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private string GetFilterExpression()
        {
            string filterExpression = string.Format("GCTransactionStatus IN ('{0}','{1}') AND (TotalNetTransactionAmount != PaymentAmount OR TotalNetTransactionAmount = 0)", Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED);
            if (hdnFilterExpressionQuickSearchCtl.Value != null && hdnFilterExpressionQuickSearchCtl.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchCtl.Value);
            }

            if (!string.IsNullOrEmpty(txtDueDateFrom.Text) && !string.IsNullOrEmpty(txtDueDateTo.Text))
            {
                filterExpression += string.Format(" AND DueDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtDueDateFrom.Text), Helper.GetDatePickerValue(txtDueDateTo.Text));
            }
            else
            {
                filterExpression += string.Format("DueDate BETWEEN '{0}' AND '{1}'", Helper.GetDatePickerValue(txtDueDateFrom.Text), Helper.GetDatePickerValue(txtDueDateTo.Text));
            }

            filterExpression += string.Format(" ORDER BY PurchaseInvoiceID");
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            lstSelectedPurchaseInvoiceID = hdnSelectedMember.Value.Split(',');
            lstNilai = hdnSelectedMemberAmount.Value.Split(',');

            List<vPurchaseInvoiceHd> lst = BusinessLayer.GetvPurchaseInvoiceHdList(GetFilterExpression());
            grdView.DataSource = lst;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseInvoiceHd entity = e.Row.DataItem as vPurchaseInvoiceHd;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsSelectedCtl");
                TextBox txtBayar = (TextBox)e.Row.FindControl("txtAmount");
                txtBayar.Text = entity.CustomSisaHutang.ToString();

                if (lstSelectedPurchaseInvoiceID.Contains(entity.PurchaseInvoiceID.ToString()))
                {
                    int idx = Array.IndexOf(lstSelectedPurchaseInvoiceID, entity.PurchaseInvoiceID.ToString());
                    chkIsSelected.Checked = true;
                    txtBayar.ReadOnly = false;
                    txtBayar.Attributes.Add("hiddenVal", lstNilai[idx].ToString());
                    txtBayar.Text = lstNilai[idx].ToString();
                }
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
                lstSelectedPurchaseInvoiceID = hdnSelectedMember.Value.Split(',');
                string[] lstSelectedSupplier = hdnSelectedMemberSupplier.Value.Split(',');
                string[] lstSelectedPayment = hdnSelectedMemberAmount.Value.Split(',');

                SupplierPaymentHd entityHd = entityHdDao.Get(Convert.ToInt32(hdnTransactionID.Value));

                string filterHd = string.Format("PurchaseInvoiceID IN ({0})", hdnSelectedMember.Value);
                List<PurchaseInvoiceHd> lstPurchaseInvoiceHd = BusinessLayer.GetPurchaseInvoiceHdList(filterHd, ctx);

                string filterDt = string.Format("PurchaseInvoiceID IN ({0}) AND LineAmount > PaymentAmount AND IsDeleted = 0", hdnSelectedMember.Value);
                List<PurchaseInvoiceDt> lstPurchaseInvoiceDt = BusinessLayer.GetPurchaseInvoiceDtList(filterDt, ctx);

                for (int i = 0; i < lstSelectedPurchaseInvoiceID.Length; ++i)
                {
                    SupplierPaymentDt entityDt = new SupplierPaymentDt();
                    entityDt.SupplierPaymentID = Convert.ToInt32(hdnTransactionID.Value);
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
                            entityInvoiceDtPayment.SupplierPaymentID = Convert.ToInt32(hdnTransactionID.Value);
                            entityInvoiceDtPayment.PaymentAmount = tempPaymentAmount;
                            entityInvoiceDtPayment.PaymentDate = entityHd.VerificationDate;
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
                        entityInvoiceHdPayment.SupplierPaymentID = Convert.ToInt32(hdnTransactionID.Value);
                        entityInvoiceHdPayment.PaymentAmount = tempPaymentAmount;
                        entityInvoiceHdPayment.PaymentDate = entityHd.VerificationDate;
                        entityInvoiceHdPaymentDao.Insert(entityInvoiceHdPayment);
                    }
                    else if (entityInvoice.TotalNetTransactionAmount < entityInvoice.TotalTransactionAmount)
                    {
                        if (entityInvoice.PaymentAmount == entityInvoice.TotalNetTransactionAmount)
                        {
                            PurchaseInvoiceHdPayment entityInvoiceHdPayment = new PurchaseInvoiceHdPayment();
                            entityInvoiceHdPayment.PurchaseInvoiceID = entityInvoice.PurchaseInvoiceID;
                            entityInvoiceHdPayment.SupplierPaymentID = Convert.ToInt32(hdnTransactionID.Value);
                            entityInvoiceHdPayment.PaymentAmount = entityInvoice.TotalNetTransactionAmount - entityInvoice.TotalTransactionAmount;
                            entityInvoiceHdPayment.PaymentDate = entityHd.VerificationDate;
                            entityInvoiceHdPaymentDao.Insert(entityInvoiceHdPayment);
                        }
                    }
                }

                retval = hdnTransactionID.Value;
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
    }
}