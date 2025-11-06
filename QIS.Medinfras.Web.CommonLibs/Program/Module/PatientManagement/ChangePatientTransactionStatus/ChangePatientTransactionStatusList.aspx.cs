using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ChangePatientTransactionStatusList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CHANGE_PATIENT_TRANSACTION_STATUS;
                case Constant.Module.RADIOTHERAPHY: return Constant.MenuCode.Radiotheraphy.CHANGE_PATIENT_TRANSACTION_STATUS;
                default: return Constant.MenuCode.Imaging.CHANGE_PATIENT_TRANSACTION_STATUS;
            }
        }

        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }
        private string refreshGridInterval = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                #region Region Registrasi
                txtRealisationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                //Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(false, false, false), "mpServiceUnit");

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1"));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;
                #endregion

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string departmentID = cboDepartment.Value.ToString();
            string filterExpression = string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}'", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, departmentID);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (departmentID != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));   
            }
            
            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);

            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            ConsultVisit oVisit = BusinessLayer.GetConsultVisit(Convert.ToInt32(transactionNo));
            RegisteredPatient pt = new RegisteredPatient();
            pt.RegistrationID = oVisit.RegistrationID;
            pt.VisitID = oVisit.VisitID;
            AppSession.RegisteredPatient = pt;

            string healthcareServiceUnitID = "";
            if (hdnModuleID.Value == Constant.Module.LABORATORY || hdnModuleID.Value == Constant.Module.IMAGING)
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                healthcareServiceUnitID = hdnServiceUnitID.Value.ToString();
            if (String.IsNullOrEmpty(healthcareServiceUnitID))
            {
                if (oVisit != null)
                {
                    healthcareServiceUnitID = oVisit.HealthcareServiceUnitID.ToString();
                }
            }
            if (healthcareServiceUnitID != "" || healthcareServiceUnitID != "0")
            {
                string url = "";
                url = string.Format("~/Libs/Program/Module/PatientManagement/ChangePatientTransactionStatus/ChangePatientTransactionStatusEntry.aspx?id={0}|{1}", healthcareServiceUnitID, transactionNo);
                Response.Redirect(url); 
            }
        }
    }
}