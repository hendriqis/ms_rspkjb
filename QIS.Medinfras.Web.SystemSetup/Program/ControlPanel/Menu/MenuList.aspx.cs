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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class MenuList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.MENU;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvMenuRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Code", "Caption" };
            fieldListValue = new string[] { "MenuCode", "MenuCaption" };
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvMenuRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vMenu> lstEntity = BusinessLayer.GetvMenuList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "MenuCode");
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Checking the RowType of the Row  
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vMenu obj = (vMenu)e.Row.DataItem;
                Label lnkReport = (Label)e.Row.FindControl("lnkReport");
                if (obj.IsHeader)
                {
                    lnkReport.Style.Add("display", "none");
                }

            }
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl("~/Program/ControlPanel/Menu/MenuEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/ControlPanel/Menu/MenuEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                BusinessLayer.DeleteMenuMaster(Convert.ToInt32(hdnID.Value));
                return true;
            }
            return false;
        }
    }
}