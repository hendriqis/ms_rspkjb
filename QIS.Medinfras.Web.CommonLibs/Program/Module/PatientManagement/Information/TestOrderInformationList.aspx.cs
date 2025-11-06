using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestOrderInformationList : BasePageTrx
    {
        string menuType = string.Empty;
        string deptType = string.Empty;
        protected int PageCount = 1;
        private string deptID = "";
        private string suID = "";
        private string filterDept = "IsActive = 1 AND IsHasRegistration = 1";
        private string filterSer = "IsDeleted = 0 AND IsUsingRegistration = 1";

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.INPATIENT:
                    return Constant.MenuCode.Inpatient.FOLLOWUP_PAGE_TESTORDER_INFORMATION;
                case Constant.Facility.OUTPATIENT:
                    return Constant.MenuCode.Outpatient.FOLLOWUP_PAGE_TESTORDER_INFORMATION;
                case Constant.Facility.EMERGENCY:
                    return Constant.MenuCode.EmergencyCare.FOLLOWUP_PAGE_TESTORDER_INFORMATION;
                default:
                    return Constant.MenuCode.Inpatient.FOLLOWUP_PAGE_TESTORDER_INFORMATION;

            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnHealthcareServiceUnitID.Value = Convert.ToString(AppSession.RegisteredPatient.HealthcareServiceUnitID);
            
            filterDept = string.Format("DepartmentID IN ('{0}')", Constant.Facility.DIAGNOSTIC);
            List<Department> lstDepartment = BusinessLayer.GetDepartmentList(filterDept);
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            hdnFilterServiceUnitID.Value = filterSer;
            cboDepartment.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";

            filterExpression = string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);

            if (cboDepartment.Value.ToString() != "" && cboDepartment.Value != null)
            {
                filterExpression += String.Format(" AND DepartmentID = '{0}'", cboDepartment.Value.ToString());
            }

            if (!String.IsNullOrEmpty(hdnServiceUnitOrderID.Value.ToString()))
            {
                filterExpression += String.Format(" AND HealthcareServiceUnitID = {0}", Convert.ToInt32(hdnServiceUnitOrderID.Value.ToString()));
            }

            filterExpression += String.Format(" Order By OrderID");

            List<vPatientOrderAll> lstEntity = BusinessLayer.GetvPatientOrderAllList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

    }
}