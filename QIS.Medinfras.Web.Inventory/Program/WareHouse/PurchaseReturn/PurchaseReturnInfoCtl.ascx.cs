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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReturnInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnParam.Value = param;
                txtPurchaseReceiveNo.Text = BusinessLayer.GetPurchaseReturnHd(Convert.ToInt32(hdnParam.Value)).PurchaseReturnNo;
                vPurchaseInvoiceDt entityInvoice = BusinessLayer.GetvPurchaseInvoiceDtList(string.Format("PurchaseReturnID = {0}", Convert.ToInt32(hdnParam.Value))).FirstOrDefault();
                if (entityInvoice != null)
                {
                    txtNotaKreditNo.Text = entityInvoice.CreditNoteNo;
                    BindGridView(1, true, ref PageCount);
                }
            }
        }

        #region Binding Source
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseReturnID = {0} AND GCTransactionStatus != '{1}' AND PurchaseReturnID IS NOT NULL", hdnParam.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseInvoiceDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }
            List<vPurchaseInvoiceDt> lstEntity = BusinessLayer.GetvPurchaseInvoiceDtList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "PurchaseReturnNo ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpViewCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}