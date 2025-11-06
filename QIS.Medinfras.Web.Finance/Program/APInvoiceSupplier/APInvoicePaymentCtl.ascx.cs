using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoicePaymentCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseInvoiceID.Value = param.ToString();
            PurchaseInvoiceHd piHD = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));
            txtPurchaseInvoiceNo.Text = piHD.PurchaseInvoiceNo;
            txtInvoiceNetAmount.Text = piHD.TotalNetTransactionAmount.ToString(Constant.FormatString.NUMERIC_2);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseInvoiceID = {0}", hdnPurchaseInvoiceID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceHdPaymentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vPurchaseInvoiceHdPayment> lstEntity = BusinessLayer.GetvPurchaseInvoiceHdPaymentList(filterExpression, 8, pageIndex, "SupplierPaymentNo ASC");
            lvwDetail.DataSource = lstEntity;
            lvwDetail.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            int pageCount = 1;
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (param[0] == "changepage")
            {
                BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                result = "changepage";
            }
            else if (param[0] == "refresh")
            {
                BindGridView(1, true, ref pageCount);
                result = string.Format("refresh|{0}", pageCount);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}