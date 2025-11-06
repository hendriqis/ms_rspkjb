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
    public partial class PivotRegistrationAnalysis : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PIVOT_REGISTRATION_ANALYSIS;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            ctlPeriode.InitializeControl();

            List<PivotLayout> lstLayout = BusinessLayer.GetPivotLayoutList(string.Format("PivotID = {0}", Constant.Pivot.REGISTRATION_ANALYSIS));
            lstLayout.Insert(0, new PivotLayout { ID = 0, TemplateName = "" });
            Methods.SetComboBoxField<PivotLayout>(cboLayoutTemplate, lstLayout, "TemplateName", "ID");
            cboLayoutTemplate.SelectedIndex = 0;

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
            pvView.Fields["registrationWeek"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["registrationWeek"].AreaIndex = 2;
            pvView.Fields["departmentName"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["departmentName"].AreaIndex = 0;
            pvView.Fields["visitID"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["visitID"].AreaIndex = 0;
            pvView.Fields["isCancelVisit"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["isCancelVisit"].AreaIndex = 1;
            pvView.Fields["isNewPatient"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["isNewPatient"].AreaIndex = 2;
            pvView.Fields["isOldPatient"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["isOldPatient"].AreaIndex = 3;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();

            if (cboLayoutTemplate.Value != null && cboLayoutTemplate.Value.ToString() != "0")
            {
                PivotLayout layout = BusinessLayer.GetPivotLayout(Convert.ToInt32(cboLayoutTemplate.Value));
                pvView.LoadLayoutFromString(layout.TemplateText);
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ctlPeriode.GetPeriodDate(ref startDate, ref endDate);
           
            hdnFilterExpression1.Value = string.Format("(VisitDate BETWEEN '{0}' AND '{1}')", startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            UpdatePivotGridFieldLayout();
        }
    }
}