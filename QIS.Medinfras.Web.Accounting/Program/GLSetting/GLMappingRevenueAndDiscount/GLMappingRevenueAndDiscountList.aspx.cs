using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLMappingRevenueAndDiscountList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_MAPPING_REVENUE_AND_DISCOUNT;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;

            filterExpression = GetFilterExpression();

            if (keyValue != "")
            {
                int row = BusinessLayer.GetvChartOfAccountRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "GLAccount No", "GLAccount Name" };
            fieldListValue = new string[] { "GLAccountNo", "GLAccountName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("HealthcareID = '{0}' AND IsDeleted = 0 AND IsHeader = 0 AND GCGLAccountType IN ('{1}','{2}','{3}','{4}','{5}')", AppSession.UserLogin.HealthcareID, Constant.GLAccountType.PENDAPATAN, Constant.GLAccountType.HPP, Constant.GLAccountType.BEBAN_USAHA, Constant.GLAccountType.BIAYA_LANGSUNG, Constant.GLAccountType.PENDAPATAN_BEBAN_LAIN);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvChartOfAccountRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vChartOfAccount> lstEntity = BusinessLayer.GetvChartOfAccountList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vConsultVisit9 entity = e.Item.DataItem as vConsultVisit9;
                //HtmlImage imgPatientSatisfactionLevelImageUri = (HtmlImage)e.Item.FindControl("imgPatientSatisfactionLevelImageUri");
                //HtmlInputImage imgOrderStatus = e.Item.FindControl("imgOrderStatus") as HtmlInputImage;
                //HtmlGenericControl divOrderStatus = e.Item.FindControl("divOrderStatus") as HtmlGenericControl;

                //if (entity.DepartmentID == Constant.Facility.EMERGENCY)
                //{
                //    HtmlTableCell tdIndicator = e.Item.FindControl("tdIndicator") as HtmlTableCell;
                //    if (!String.IsNullOrEmpty(entity.TriageColor))
                //    {
                //        tdIndicator.Style.Add("background-color", entity.TriageColor);
                //    }
                //}
            }
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

    }
}