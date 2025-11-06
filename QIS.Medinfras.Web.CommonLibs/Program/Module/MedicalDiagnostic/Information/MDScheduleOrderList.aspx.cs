using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MDScheduleOrderList : BasePagePatientOrder
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Imaging.WORK_LIST;
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareServiceUnitID NOT IN ({0},{1})", AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID);
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.DIAGNOSTIC, filterExpression);
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboMedicalDiagnosticOrder, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboMedicalDiagnosticOrder.SelectedIndex = 0;

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));

                #region Region Order
                txtOrderDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                lstDept.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboPatientFromOrder, lstDept, "DepartmentName", "DepartmentID");

                //lstHealthcareServiceUnit.Insert(0, new vHealthcareServiceUnit { HealthcareServiceUnitID = 0, ServiceUnitName = string.Format(" - {0} - ", GetLabel("All")) });
                cboPatientFromOrder.SelectedIndex = 0;
                #endregion

                ((GridPatientOrderCtl)grdOrderedPatient).InitializeControl();

                Helper.SetControlEntrySetting(cboPatientFromOrder, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtOrderDate, new ControlEntrySetting(true, true, true), "mpPatientList");
            }
        }
        public override bool IsShowRightPanel()
        {
            return false;
        }


        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string TestOrderID)
        {
            string url = string.Format("~/Libs/Program/Module/PatientManagement/Transaction/PatientManagementTransactionDetail.aspx?id=to|{0}|{1}", TestOrderID, transactionNo);
            Response.Redirect(url);
        }

        public override string GetFilterExpression()
        {
            return string.Empty;
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = "";
            string healthcareServiceUnitID = "";
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                healthcareServiceUnitID = cboMedicalDiagnosticOrder.Value.ToString();
            else
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();

            filterExpression += string.Format("ScheduledDate = '{0}' AND HealthcareServiceUnitID = {1} AND GCVisitStatus NOT IN ('{2}','{3}','{4}') AND ",
                    Helper.GetDatePickerValue(txtOrderDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    healthcareServiceUnitID, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            filterExpression += string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL);

            if (cboPatientFromOrder.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFromOrder.Value);
            if (hdnServiceUnitOrderID.Value != "0" && hdnServiceUnitOrderID.Value != "")
                filterExpression += string.Format(" AND VisitHSUID = {0}", hdnServiceUnitOrderID.Value);
            if (hdnFilterExpressionQuickSearchOrder.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchOrder.Value);
            if (hdnBedID.Value != "")
                filterExpression += string.Format(" AND BedID = {0}", hdnBedID.Value);
            return filterExpression;
        }

        public override string GetSortingTestOrder()
        {
            string sortBy = "";

            return sortBy;
        }
    }
}