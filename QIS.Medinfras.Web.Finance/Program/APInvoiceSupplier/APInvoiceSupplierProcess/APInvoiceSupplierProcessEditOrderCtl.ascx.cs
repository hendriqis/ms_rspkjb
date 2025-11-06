using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessEditOrderCtl : BaseEntryPopupCtl
    {
        protected string GetVATPercentage()
        {
            return hdnVATPercentage.Value;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            string filterSetVar = string.Format("ParameterCode IN ('{0}','{1}','{2}')",
                                                        Constant.SettingParameter.VAT_PERCENTAGE,
                                                        Constant.SettingParameter.IS_PROCESS_INVOICE_CAN_CHANGE_AVERAGE_PRICE,
                                                        Constant.SettingParameter.FN_REVISI_HUTANG_PENERIMAAN_MENGGUNAKAN_PEMBULATAN_TUKAR_FAKTUR);
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnVATPercentage.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).FirstOrDefault().ParameterValue;
            hdnIsAllowChangePriceInAP.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.IS_PROCESS_INVOICE_CAN_CHANGE_AVERAGE_PRICE).FirstOrDefault().ParameterValue;
            hdnIsChangedAPAmountFromRoundingInvoice.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_REVISI_HUTANG_PENERIMAAN_MENGGUNAKAN_PEMBULATAN_TUKAR_FAKTUR).FirstOrDefault().ParameterValue;

            if (hdnIsChangedAPAmountFromRoundingInvoice.Value != "1")
            {
                txtRoundingInvoiceDtAmount.ReadOnly = true;
            }

            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.CHARGES_TYPE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            cboCurrency.SelectedIndex = 0;

            string[] temp = param.Split('|');
            hdnID.Value = temp[0];
            hdnPurchaseOrderID.Value = temp[1];
            hdnPurchaseInvoiceID.Value = temp[2];

            PurchaseInvoiceDt pid = BusinessLayer.GetPurchaseInvoiceDt(Convert.ToInt32(hdnID.Value));
            txtRoundingInvoiceDtAmount.Text = pid.RoundingAmount.ToString();

            PurchaseOrderHd entity = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(hdnPurchaseOrderID.Value));
            txtPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            txtPurchaseOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDP.Text = entity.DownPaymentAmount.ToString();
            cboTerm.Value = entity.TermID.ToString();
            txtNotes.Text = entity.Remarks;
            cboCurrency.Value = entity.GCCurrencyCode.ToString();
            txtKurs.Text = entity.CurrencyRate.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            decimal transactionAmount = entity.TransactionAmount - entity.FinalDiscount;
            txtTotalOrder.Text = transactionAmount.ToString();
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            txtFinalDiscount.Text = entity.FinalDiscountAmount.ToString();
            hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnPurchaseOrderID.Value, Constant.TransactionStatus.VOID);
            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                vPurchaseOrderDt entity = (vPurchaseOrderDt)e.Item.DataItem;
                TextBox txtUnitPrice = (TextBox)e.Item.FindControl("txtUnitPrice");
                TextBox txtDiscountPercentage1 = (TextBox)e.Item.FindControl("txtDiscountPercentage1");
                TextBox txtDiscountAmount1 = (TextBox)e.Item.FindControl("txtDiscountAmount1");
                TextBox txtDiscountPercentage2 = (TextBox)e.Item.FindControl("txtDiscountPercentage2");
                TextBox txtDiscountAmount2 = (TextBox)e.Item.FindControl("txtDiscountAmount2");
                TextBox txtLineAmount = (TextBox)e.Item.FindControl("txtLineAmount");
                txtUnitPrice.Text = entity.UnitPrice.ToString();
                txtDiscountPercentage1.Text = entity.DiscountPercentage1.ToString();
                txtDiscountAmount1.Text = entity.DiscountAmount1.ToString();
                txtDiscountPercentage2.Text = entity.DiscountPercentage2.ToString();
                txtDiscountAmount2.Text = entity.DiscountAmount2.ToString();
                txtLineAmount.Text = entity.CustomSubTotal.ToString();
            }
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseInvoiceDtDao purchaseInvoiceDtDao = new PurchaseInvoiceDtDao(ctx);

            try
            {
                PurchaseInvoiceDt purchaseInvoiceDt = purchaseInvoiceDtDao.Get(Convert.ToInt32(hdnID.Value));
                if (!purchaseInvoiceDt.IsDeleted)
                {
                    purchaseInvoiceDt.RoundingAmount = Convert.ToDecimal(txtRoundingInvoiceDtAmount.Text);
                    purchaseInvoiceDt.LineAmount = purchaseInvoiceDt.TransactionAmount
                                                    - purchaseInvoiceDt.DownPaymentAmount
                                                    - purchaseInvoiceDt.DiscountAmount
                                                    - purchaseInvoiceDt.FinalDiscountAmount
                                                    + purchaseInvoiceDt.VATAmount
                                                    + purchaseInvoiceDt.PPH23Amount
                                                    + purchaseInvoiceDt.PPH25Amount
                                                    + purchaseInvoiceDt.StampAmount
                                                    + purchaseInvoiceDt.ChargesAmount
                                                    - purchaseInvoiceDt.CreditNoteAmount
                                                    + purchaseInvoiceDt.RoundingAmount;
                    purchaseInvoiceDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseInvoiceDtDao.Update(purchaseInvoiceDt);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Hutang supplier tidak dapat diubah. Harap refresh halaman ini.";
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
    }
}