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
    public partial class ItemRequestPurchaseRequestCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnIRID.Value = param;
                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount);
            }
        }

        #region Item Request Dt
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ItemRequestID = {0} AND IsDeleted = 0 AND PurchaseRequestQty != 0", hdnIRID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            ItemRequestHd irhd = BusinessLayer.GetItemRequestHd(Convert.ToInt32(hdnIRID.Value));
            txtItemRequestNo.Text = irhd.ItemRequestNo;

            List<vItemRequestDt> lstEntity = BusinessLayer.GetvItemRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "ItemName1 ASC");
            grdViewPRDT.DataSource = lstEntity;
            grdViewPRDT.DataBind();
        }

        protected void cbpViewIRDT_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        #region PurchaseRequest Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnItemID.Value != "")
            {
                filterExpression = string.Format("ItemRequestID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'", 
                    hdnIRID.Value, hdnItemID.Value, Constant.TransactionStatus.VOID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPurchaseRequestDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
                }
            }
            List<vPurchaseRequestDt> lstEntity = BusinessLayer.GetvPurchaseRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "ItemName1 ASC");
            grdViewPurchaseRequestDt.DataSource = lstEntity;
            grdViewPurchaseRequestDt.DataBind();
        }

        protected void cbpViewPurchaseRequestDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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