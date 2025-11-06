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
    public partial class BudgetingRealizationInformationPerMonthList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.BUDGETING_REALIZATION_INFORMATION_PER_MONTH;
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

            List<StandardCode> lstSession = new List<StandardCode>();
            lstSession.Insert(0, new StandardCode { StandardCodeName = "Januari", StandardCodeID = "01" });
            lstSession.Insert(1, new StandardCode { StandardCodeName = "Februari", StandardCodeID = "02" });
            lstSession.Insert(2, new StandardCode { StandardCodeName = "Maret", StandardCodeID = "03" });
            lstSession.Insert(3, new StandardCode { StandardCodeName = "April", StandardCodeID = "04" });
            lstSession.Insert(4, new StandardCode { StandardCodeName = "Mei", StandardCodeID = "05" });
            lstSession.Insert(5, new StandardCode { StandardCodeName = "Juni", StandardCodeID = "06" });
            lstSession.Insert(6, new StandardCode { StandardCodeName = "Juli", StandardCodeID = "07" });
            lstSession.Insert(7, new StandardCode { StandardCodeName = "Agustus", StandardCodeID = "08" });
            lstSession.Insert(8, new StandardCode { StandardCodeName = "September", StandardCodeID = "09" });
            lstSession.Insert(9, new StandardCode { StandardCodeName = "Oktober", StandardCodeID = "10" });
            lstSession.Insert(10, new StandardCode { StandardCodeName = "November", StandardCodeID = "11" });
            lstSession.Insert(11, new StandardCode { StandardCodeName = "Desember", StandardCodeID = "12" });
            Methods.SetComboBoxField<StandardCode>(cboMonth, lstSession, "StandardCodeName", "StandardCodeID");
            cboMonth.SelectedIndex = 0;

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
            String BudgetYear = txtBudgetYear.Text;
            Int32 BudgetMonth = Convert.ToInt32(cboMonth.Value);
            String DisplayFilter = cboDisplayFilter.Value.ToString();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetBudgetingRealizationPerMonthInformationRowCount(BudgetYear, BudgetMonth, DisplayFilter);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<GetBudgetingRealizationPerMonthInformation> lstEntity = BusinessLayer.GetBudgetingRealizationPerMonthInformationList(BudgetYear, BudgetMonth, DisplayFilter, pageIndex, Constant.GridViewPageSize.GRID_ITEM);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}