using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInvoiceSupplierVerificationDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseInvoiceID.Value = param;

            PurchaseInvoiceHd entity = BusinessLayer.GetPurchaseInvoiceHd(Convert.ToInt32(hdnPurchaseInvoiceID.Value));
            txtPurchaseInvoiceNo.Text = entity.PurchaseInvoiceNo;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseInvoiceID = {0} AND IsDeleted=0", hdnPurchaseInvoiceID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vPurchaseInvoiceDt> lstDistributionDt = BusinessLayer.GetvPurchaseInvoiceDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ID ASC");
            lvwView.DataSource = lstDistributionDt;
            lvwView.DataBind();
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