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
    public partial class PivotDrugsAnalysis : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PIVOT_DRUGS_ANALYSIS;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnHealthcareID.Value = AppSession.UserLogin.HealthcareID;
            ctlPeriode.InitializeControl();
            UpdatePivotGridFieldLayout();
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected string OnGetFilterExpressionItemProduct()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }

        void UpdatePivotGridFieldLayout()
        {
            ChangePivotGridFieldLayout();
        }

        void ChangePivotGridFieldLayout()
        {
            pvView.BeginUpdate();
            pvView.OptionsPager.RowsPerPage = 10;
            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ctlPeriode.GetPeriodDate(ref startDate, ref endDate);

            hdnFromDate.Value = startDate.ToString("yyyyMMdd");
            hdnToDate.Value = endDate.ToString("yyyyMMdd");
            if (hdnItemGroupID.Value.ToString() != "")
            {
                hdnAdditionalFilterExpression.Value = String.Format("ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value); 
            } 
            else hdnAdditionalFilterExpression.Value = "";
            //hdnFilterExpression1.Value = string.Format("(VisitDate BETWEEN '{0}' AND '{1}')", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            UpdatePivotGridFieldLayout();
        }
    }
}