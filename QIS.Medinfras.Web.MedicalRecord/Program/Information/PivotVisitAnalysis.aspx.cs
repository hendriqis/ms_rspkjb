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
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Globalization;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;
using System.Text;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class PivotVisitAnalysis : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_VISIT_ANALYSIS;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            cboMonth.DataSource = Enumerable.Range(1, 12).Select(a => new
            {
                MonthName = DateTimeFormatInfo.CurrentInfo.GetMonthName(a),
                MonthNumber = a
            });
            cboMonth.TextField = "MonthName";
            cboMonth.ValueField = "MonthNumber";
            cboMonth.EnableCallbackMode = false;
            cboMonth.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboMonth.DropDownStyle = DropDownStyle.DropDownList;
            cboMonth.DataBind();
            cboMonth.Value = DateTime.Now.Month.ToString();

            cboYear.DataSource = Enumerable.Range(DateTime.Now.Year - 99, 100).Reverse();
            cboYear.EnableCallbackMode = false;
            cboYear.IncrementalFilteringMode = IncrementalFilteringMode.Contains;
            cboYear.DropDownStyle = DropDownStyle.DropDownList;
            cboYear.DataBind();
            cboYear.SelectedIndex = 0;   
            hdnFilterExpression1.Value = string.Format("DepartmentID = '{0}' OR DepartmentID = '{1}'", Constant.Facility.OUTPATIENT,Constant.Facility.INPATIENT);
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

            pvView.Fields["departmentName"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["departmentName"].AreaIndex = 0;
            pvView.Fields["gender"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["gender"].AreaIndex = 1;
            pvView.Fields["statusNewPatient"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["statusNewPatient"].AreaIndex = 2;            
            pvView.Fields["registrationID"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["ageGroup"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["ageGroup"].AreaIndex = 0;
            pvView.Fields["ageGroup"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;
            hdnFilterExpression1.Value = string.Format("DepartmentID = '{0}' OR DepartmentID = '{1}' AND YEAR(RegistrationDate) = {2} AND MONTH(RegistrationDate) = {3} AND GCRegistrationStatus != '{4}'", Constant.Facility.OUTPATIENT, Constant.Facility.INPATIENT, cboYear.Value, cboMonth.Value, Constant.VisitStatus.CANCELLED);
            UpdatePivotGridFieldLayout();
        }
       
    }
}