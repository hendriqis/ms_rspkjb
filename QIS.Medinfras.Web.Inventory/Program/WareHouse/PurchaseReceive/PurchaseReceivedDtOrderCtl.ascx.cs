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
    public partial class PurchaseReceivedDtOrderCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnPORID.Value = param;
                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount);
            }
        }

        #region Purchase Receive Dt
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnPORID.Value, Constant.TransactionStatus.VOID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            PurchaseReceiveHd prhd = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPORID.Value));
            txtPurchaseReceiveNo.Text = prhd.PurchaseReceiveNo;

            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
            grdViewPRDT.DataSource = lstEntity;
            grdViewPRDT.DataBind();
        }

        protected void cbpViewPRDT_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion

        #region Purchase Order Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnItemID.Value != "")
            {
                filterExpression = string.Format("PurchaseReceiveID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'",
                    hdnPORID.Value, hdnItemID.Value, Constant.TransactionStatus.VOID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPurchaseReceivePODtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
                }
            }

            List<vPurchaseReceivePODt> lstEntity = BusinessLayer.GetvPurchaseReceivePODtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "PurchaseOrderID ASC, ItemName1 ASC");
            grdViewPODT.DataSource = lstEntity;
            grdViewPODT.DataBind();
        }

        protected void cbpViewPODT_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}