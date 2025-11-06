using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxPivotGrid;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PivotDiagnoseAnalysis : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_DIAGNOSE_ANALSIS;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            ctlPeriode.InitializeControl();
            hdnFilterExpression1.Value = string.Format("GCDiagnoseType = '{0}'", Constant.DiagnoseType.MAIN_DIAGNOSIS);
            UpdatePivotGridFieldLayout();
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        void UpdatePivotGridFieldLayout()
        {
            ChangePivotGridFieldLayout();
        }
        void ChangePivotGridFieldLayout()
        {
            pvView.BeginUpdate();
            foreach (PivotGridField field in pvView.Fields)
            {
                field.Area = DevExpress.XtraPivotGrid.PivotArea.FilterArea;
                field.SortOrder = DevExpress.XtraPivotGrid.PivotSortOrder.Ascending;
            }

            pvView.Fields["registrationYear"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["registrationYear"].AreaIndex = 0;
            pvView.Fields["registrationMonth"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["registrationMonth"].AreaIndex = 1;
            pvView.Fields["icdBlockName"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["icdBlockName"].AreaIndex = 0;
            pvView.Fields["icdBlockName"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;
            pvView.Fields["visitID"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["isNewDiagnose"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["isFollowUpCase"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ctlPeriode.GetPeriodDate(ref startDate, ref endDate);

            hdnFilterExpression1.Value = string.Format("VisitDate BETWEEN '{0}' AND '{1}'", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            UpdatePivotGridFieldLayout();
        }
       
    }
}