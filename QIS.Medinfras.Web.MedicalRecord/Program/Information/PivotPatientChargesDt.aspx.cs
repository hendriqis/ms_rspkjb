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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PivotPatientChargesDt : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_PATIENT_CHARGES;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            ctlPeriode.InitializeControl();
            hdnFilterExpression1.Value = string.Format("GCItemType = '{0}'", Constant.ItemGroupMaster.SERVICE);
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

            pvView.Fields["Year"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["Year"].AreaIndex = 0;
            pvView.Fields["Month"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["Month"].AreaIndex = 1;
            pvView.Fields["lineAmount"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["itemCode"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["ServiceUnit"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["ServiceUnit"].AreaIndex = 0;
            pvView.Fields["ServiceUnit"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ctlPeriode.GetPeriodDate(ref startDate, ref endDate);

            hdnFilterExpression1.Value = string.Format("TransactionDate BETWEEN '{0}' AND '{1}'", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            UpdatePivotGridFieldLayout();
        }
    }
}