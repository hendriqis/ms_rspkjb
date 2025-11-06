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
using QIS.Medinfras.Web.Finance.Program;
using System.Drawing;
using System.Globalization;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformasiPembayaranSupplierCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private InformasiPembayaranSupplier DetailPage
        {
            get { return (InformasiPembayaranSupplier)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnInvoiceID.Value = param;

            PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnInvoiceID.Value));
            txtInvoiceNo.Text = string.Format("{0}", entity.PurchaseInvoiceNo);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("PurchaseInvoiceID = {0}", hdnInvoiceID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInformationSupplierPaymentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_FIVE);
            }

            List<vInformationSupplierPayment> lstEntity = BusinessLayer.GetvInformationSupplierPaymentList(filterExpression, Constant.GridViewPageSize.GRID_FIVE, pageIndex, "PurchaseInvoiceID, SupplierPaymentID");
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }
        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
    }
}