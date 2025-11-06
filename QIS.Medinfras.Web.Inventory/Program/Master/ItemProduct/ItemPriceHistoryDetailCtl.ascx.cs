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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemPriceHistoryDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            hdnFilterExpressionCtl.Value = param.ToString();
            ItemMaster item = BusinessLayer.GetItemMaster(Convert.ToInt32(param));
            txtItemName.Text = String.Format("{0} | {1}", item.ItemCode, item.ItemName1);
            BindGridView(1, true, ref PageCount);

        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ItemID = {0}", hdnFilterExpressionCtl.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemPriceHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vItemPriceHistory> lstEntity = BusinessLayer.GetvItemPriceHistoryList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "HistoryDate ASC");
            lvwDetail.DataSource = lstEntity;
            lvwDetail.DataBind();
        }

        protected void cbpViewItemPriceHistoryDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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