using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class OutstandingItemRequest : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_DISTRIBUTION_OUTSTANDING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_DISTRIBUTION);
            filterExpressionLocationTo = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_REQUEST);
            
            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected string GetFilterExpression()
        {
            string filterExpression = "";
            
            if (txtFromLocationCode.Text != "" || txtToLocationCode.Text != "")
            {
                if (txtFromLocationCode.Text != "")
                {
                    filterExpression += string.Format("FromLocationID = {0} AND DistributionQty < RequestQty", hdnFromLocationID.Value);
                }

                if (txtToLocationCode.Text != "")
                {
                    if (filterExpression != "")
                        filterExpression += " AND ";
                    filterExpression += String.Format("ToLocationID = {0}", hdnToLocationID.Value);
                }

                if (!chkIsClose.Checked)
                {
                    filterExpression += String.Format(" AND GCTransactionStatus NOT IN ('{0}')", Constant.TransactionStatus.CLOSED);
                }
            }
            else
                filterExpression = "1 = 0";
            return filterExpression;
        }
        
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (!String.IsNullOrEmpty(txtItemName.Text))
            {
                filterExpression += string.Format(" AND ItemName1 LIKE '%{0}%'", txtItemName.Text);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvOutstandingItemRequestRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vOutstandingItemRequest> lstEntity = BusinessLayer.GetvOutstandingItemRequestList(filterExpression, 10, pageIndex, "ItemName1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}