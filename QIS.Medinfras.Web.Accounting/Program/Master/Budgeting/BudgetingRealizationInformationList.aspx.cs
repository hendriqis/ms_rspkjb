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
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class BudgetingRealizationInformationList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BUDGETING_REALIZATION_INFORMATION;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnDefaultYear.Value = DateTime.Now.ToString(Constant.FormatString.YEAR_FORMAT);
            txtBudgetYear.Text = hdnDefaultYear.Value;

            List<Variable> lst = new List<Variable>();
            lst.Insert(0, new Variable { Code = "1", Value = "per COA" });
            lst.Insert(1, new Variable { Code = "2", Value = "per COA per RCC" });
            Methods.SetComboBoxField<Variable>(cboDisplayFilter, lst, "Value", "Code");
            cboDisplayFilter.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
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
            string filterExpression = string.Format("DisplayFilter = '{0}' AND BudgetYear = '{1}'", cboDisplayFilter.Value.ToString(), txtBudgetYear.Text);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBudgetingRealizationInformationRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vBudgetingRealizationInformation> lstEntity = BusinessLayer.GetvBudgetingRealizationInformationList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "GLAccountNo, RevenueCostCenterCode");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}