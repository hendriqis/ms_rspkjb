using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierProcessReturnDtCtl : BaseViewPopupCtl
    {
        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        public override void InitializeDataControl(string param)
        {
            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;

            vPurchaseReturnHd entity = BusinessLayer.GetvPurchaseReturnHdList(string.Format("PurchaseReturnID = {0}", param))[0];
            hdnPurchaseReturnID.Value = entity.PurchaseReturnID.ToString();
            txtReturnNo.Text = entity.PurchaseReturnNo;
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtReturnType.Text = entity.PurchaseReturnType;
            txtNotes.Text = entity.Remarks;
            chkIsAutoUpdateStock.Checked = entity.IsAutoUpdateStock;
            chkPPN.Checked = entity.IsIncludeVAT;
            txtTotalOrder.Text = entity.TransactionAmount.ToString();

            decimal vatAmount = (entity.TransactionAmount * entity.VATPercentage) / 100;
            txtPPN.Text = vatAmount.ToString();
            txtTotalOrderSaldo.Text = (entity.TransactionAmount + vatAmount).ToString();

            hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");

            if (entity.PurchaseReturnType == Constant.PurchaseReturnType.REPLACEMENT)
                chkIsAutoUpdateStock.Enabled = false;
            else
                chkIsAutoUpdateStock.Enabled = true;
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("PurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseReturnID.Value, Constant.TransactionStatus.VOID);
            List<vPurchaseReturnDt> lstEntity = BusinessLayer.GetvPurchaseReturnDtList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }
    }
}