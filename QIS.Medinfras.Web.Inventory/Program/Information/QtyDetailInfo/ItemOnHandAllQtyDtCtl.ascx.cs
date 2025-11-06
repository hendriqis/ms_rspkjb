using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemOnHandAllQtyDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnItemID.Value = param;

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemName.Text = im.ItemName1;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ItemID = {0}", hdnItemID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vItemBalance> lstEntity = BusinessLayer.GetvItemBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "LocationName ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    result = string.Format("refresh|{0}", pageCount);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}