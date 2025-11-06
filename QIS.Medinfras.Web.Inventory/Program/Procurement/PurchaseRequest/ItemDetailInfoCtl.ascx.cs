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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDetailInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnLocationID.Value = lstParam[0];
            hdnItemID.Value = lstParam[1];

            txtDateFrom.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            ItemMaster im = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnItemID.Value));
            txtItemName.Text = string.Format("{0} - {1}", im.ItemCode, im.ItemName1);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("LocationID = {0} AND MovementDate BETWEEN '{1}' AND '{2}'", hdnLocationID.Value, Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyyMMdd"), Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyyMMdd"));
            filterExpression += string.Format(" AND ItemID = {0}", hdnItemID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemMovementRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vItemMovement> lstDistributionDt = BusinessLayer.GetvItemMovementList(filterExpression, 10, pageIndex);
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