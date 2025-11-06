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


namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProductLineList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PRODUCT_LINE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvProductLineRowIndex(filterExpression, keyValue, " ProductLineCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Product Line", "Kode Product Line" };
            fieldListValue = new string[] { "ProductLineName", "ProductLineCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvProductLineRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vProductLine> lstEntity = BusinessLayer.GetvProductLineList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ProductLineCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Libs/Program/Master/ProductLine/ProductLineEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ProductLine/ProductLineEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ProductLine entity = BusinessLayer.GetProductLine(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateProductLine(entity);
                return true;
            }
            return false;
        }
    }
}