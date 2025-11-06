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

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PivotRLClass : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_RLCLASS;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            ctlPeriode.InitializeControl();
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

            pvView.Fields["Tahun"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["Tahun"].AreaIndex = 0;
            pvView.Fields["Bulan"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["Bulan"].AreaIndex = 1;
            pvView.Fields["ClassRL"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["ClassRL"].AreaIndex = 2;
            
            pvView.Fields["JumlahHariRawat"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["JumlahHariRawat"].AreaIndex = 0;
            pvView.Fields["visitID"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["visitID"].AreaIndex = 1;

            pvView.Fields["DepartmentID"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["DepartmentID"].AreaIndex = 0;
            pvView.Fields["DepartmentID"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;
            pvView.Fields["DepartmentID"].FilterValues.Add("INPATIENT");
            pvView.Fields["DepartmentID"].FilterValues.FilterType = DevExpress.XtraPivotGrid.PivotFilterType.Included;

            pvView.Fields["SpecialtyName"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["SpecialtyName"].AreaIndex = 1;
            pvView.Fields["SpecialtyName"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ctlPeriode.GetPeriodDate(ref startDate, ref endDate);
           
            hdnFilterExpression1.Value = string.Format("(VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"), Constant.VisitStatus.CANCELLED);
            UpdatePivotGridFieldLayout();
        }
    }
}