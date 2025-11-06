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
    public partial class APInvoiceSupplierProcessDtOrderCtl : BaseViewPopupCtl
    {
        protected string GetVATPercentage()
        {
            return hdnVATPercentage.Value;
        }

        public override void InitializeDataControl(string param)
        {
            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;

            string[] temp = param.Split('|');
            hdnIDCtl.Value = temp[0];
            hdnPurchaseReceiveIDCtl.Value = temp[1];
            hdnPurchaseOrderIDCtl.Value = temp[2];

            PurchaseOrderHd entity = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(hdnPurchaseOrderIDCtl.Value));
            txtPurchaseOrderNo.Text = entity.PurchaseOrderNo;
            txtPurchaseOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseOrderTime.Text = entity.CreatedDate.ToString(Constant.FormatString.TIME_FORMAT);
            cboTerm.Value = entity.TermID.ToString();
            txtNotes.Text = entity.Remarks;
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            txtFinalDiscount.Text = entity.FinalDiscountAmount.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");

            decimal oFinalDiscountAmount = 0, oPPNAmount = 0, oTotalEndTransactionAmount = 0;

            oFinalDiscountAmount = !entity.IsFinalDiscountInPercentage ? (entity.TransactionAmount * entity.FinalDiscount / 100) : (entity.FinalDiscountAmount);
            oPPNAmount = (entity.TransactionAmount - oFinalDiscountAmount) * entity.VATPercentage / 100;
            oTotalEndTransactionAmount = entity.TransactionAmount - oFinalDiscountAmount;

            txtPPN.Text = oPPNAmount.ToString();
            txtTotalOrder.Text = entity.TransactionAmount.ToString();            

            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnPurchaseOrderIDCtl.Value, Constant.TransactionStatus.VOID);
            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}