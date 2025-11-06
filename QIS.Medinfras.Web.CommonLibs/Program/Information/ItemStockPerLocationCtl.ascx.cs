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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemStockPerLocationCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstEntitySC = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.STOCK_ITEM_STATUS));
            Methods.SetComboBoxField(cboStockStatus, lstEntitySC, "StandardCodeName", "StandardCodeID");
            cboStockStatus.SelectedIndex = 0;
            BindGridView(1, true, ref PageCount);
        }

        private void SetControlProperties()
        {
            
        }

        protected void cbpViewISPL_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
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
                filterExpression += String.Format("AND {0}", hdnFilterExpressionQuickSearch.Value);
                if (cboStockStatus.Value.ToString() == Constant.StockStatus.READY_STOCK)
                {
                    filterExpression += "AND QuantityEND > 0 ";
                }
                else if (cboStockStatus.Value.ToString() == Constant.StockStatus.NO_STOCK)
                {
                    filterExpression += "AND QuantityEND <= 0 ";
                }
            }
            else
                filterExpression += "AND 1 = 0";

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceAlternateUnitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 8);
            }

            List<vItemBalanceAlternateUnit> lstEntity = BusinessLayer.GetvItemBalanceAlternateUnitList(filterExpression, 8, pageIndex, "ItemName1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }
    }
}