using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class FAPartsDetailCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            hdnParentFixedAssetID.Value = param;

            FAItem faItem = BusinessLayer.GetFAItem(Convert.ToInt32(hdnParentFixedAssetID.Value));
            txtParentFixedAssetCode.Text = faItem.FixedAssetCode;
            txtParentFixedAssetName.Text = faItem.FixedAssetName;

            BindGridView(CurrPage, true, ref PageCount);
        }

        protected void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = String.Format("GCItemStatus = '{0}' AND IsDeleted = 0 AND ParentID = {1}", Constant.ItemStatus.ACTIVE, hdnParentFixedAssetID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFAItemRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_CTL);
            }

            List<vFAItem> entity = BusinessLayer.GetvFAItemList(filterExpression, Constant.GridViewPageSize.GRID_CTL, pageIndex, "DisplayOrder, FixedAssetCode");
            grdPopupView.DataSource = entity;
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