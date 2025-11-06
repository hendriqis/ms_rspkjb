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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PivotPatientChargesDt : BasePageList
    {
        public override string OnGetMenuCode()
        {
            String GCItemType = Request.QueryString["id"];
            if (GCItemType == Constant.ItemGroupMaster.RADIOLOGY)
                return Constant.MenuCode.Imaging.PIVOT_PATIENT_CHARGES;
            else
                return Constant.MenuCode.Laboratory.PIVOT_PATIENT_CHARGES;
        }
        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnGCItemType.Value = Request.QueryString["id"];

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
            lstDepartment.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;


            ((PeriodeCtl)ctlPeriode).InitializeControl();
            hdnFilterExpression1.Value = string.Format("GCItemType = '{0}'", hdnGCItemType.Value);
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

            pvView.Fields["DepartmentName"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["DepartmentName"].AreaIndex = 0;
            pvView.Fields["ClassName"].Area = DevExpress.XtraPivotGrid.PivotArea.ColumnArea;
            pvView.Fields["ClassName"].AreaIndex = 1;
            pvView.Fields["itemCode"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["tariff"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["lineAmount"].Area = DevExpress.XtraPivotGrid.PivotArea.DataArea;
            pvView.Fields["itemName"].Area = DevExpress.XtraPivotGrid.PivotArea.RowArea;
            pvView.Fields["itemName"].AreaIndex = 0;
            pvView.Fields["itemName"].TotalsVisibility = DevExpress.XtraPivotGrid.PivotTotalsVisibility.AutomaticTotals;

            pvView.Width = Unit.Percentage(100);
            pvView.EndUpdate();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            DateTime startDate = DateTime.Today;
            DateTime endDate = DateTime.Today;

            ((PeriodeCtl)ctlPeriode).GetPeriodDate(ref startDate, ref endDate);

            string filterExpression = string.Format("GCItemType = '{0}' AND TransactionDate BETWEEN '{1}' AND '{2}'", hdnGCItemType.Value, startDate.ToString("yyyyMMdd"), endDate.ToString("yyyyMMdd"));
            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID = {0}", hdnItemGroupID.Value);
            hdnFilterExpression1.Value = filterExpression;
            UpdatePivotGridFieldLayout();
        }
    }
}