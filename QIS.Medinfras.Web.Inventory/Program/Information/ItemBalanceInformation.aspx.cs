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
    public partial class ItemBalanceInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_BALANCE_INFORMATION_PER_ITEM;
        }

        protected override void InitializeDataControl()
        {
            List<StandardCode> lstStatusStock = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.STOCK_ITEM_STATUS));
            Methods.SetComboBoxField(cboStockStatus, lstStatusStock, "StandardCodeName", "StandardCodeID");
            cboStockStatus.SelectedIndex = 0;
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "IsDeleted = 0 ";

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                if (cboStockStatus.Value.ToString() == Constant.StockStatus.READY_STOCK)
                {
                    filterExpression += "AND QuantityEND > 0 ";
                }
                else if (cboStockStatus.Value.ToString() == Constant.StockStatus.NO_STOCK)
                {
                    filterExpression += "AND QuantityEND <= 0 ";
                }
                filterExpression += String.Format(" AND {0} ", hdnFilterExpressionQuickSearch.Value);
            }
            else
                filterExpression += "AND 1 = 0";
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceAlternateUnitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemBalanceAlternateUnit> lstEntity = BusinessLayer.GetvItemBalanceAlternateUnitList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();

        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}