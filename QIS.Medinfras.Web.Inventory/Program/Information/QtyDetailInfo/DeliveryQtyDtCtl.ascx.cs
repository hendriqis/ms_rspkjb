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
using QIS.Medinfras.Web.Inventory.Program;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class DeliveryQtyDtCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private ItemDistributionInfoPerItem DetailPage
        {
            get { return (ItemDistributionInfoPerItem)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnItemID.Value = lstParam[0];
            hdnLocationID.Value = lstParam[1];

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemName.Text = string.Format("{0} - {1}", im.ItemCode, im.ItemName1);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("FromLocationID = {0} AND ItemID = {1} AND GCItemDetailStatus = 'X160^003'", hdnLocationID.Value, hdnItemID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemDistributionDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vItemDistributionDt> lstDistributionDt = BusinessLayer.GetvItemDistributionDtList(filterExpression, 10, pageIndex);
            grdPopupView.DataSource = lstDistributionDt;
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