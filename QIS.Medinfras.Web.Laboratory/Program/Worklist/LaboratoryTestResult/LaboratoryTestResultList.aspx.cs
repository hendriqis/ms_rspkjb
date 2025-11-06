using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class LaboratoryTestResultList : BasePageRegisteredPatient
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.LAB_RESULT;
        }
        private GetUserMenuAccess menu;
        public override bool IsShowRightPanel()
        {
            return false;
        }
        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }
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
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                
                string filterExpression = string.Format("IsUsingRegistration = 1 AND IsLaboratoryUnit = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                hdnID.Value = cboServiceUnit.Value.ToString();

                hdnHealthcareServiceUnitID.Value = cboServiceUnit.Value.ToString();

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Ada Hasil" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Ada Hasil" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");
                cboResultType.Value = "1";

                List<Variable> lstVariableCoverage = new List<Variable>();
                lstVariableCoverage.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariableCoverage.Add(new Variable { Code = "1", Value = "Belum Dibayar" });
                lstVariableCoverage.Add(new Variable { Code = "2", Value = "Sudah Dibayar" });
                Methods.SetComboBoxField<Variable>(cboCoverage, lstVariableCoverage, "Value", "Code");
                cboCoverage.Value = "0";

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
                lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                txtDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                
                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                grdPatientResult.InitializeControl();
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            filterExpression += string.Format("TransactionDate = '{0}' AND HealthcareServiceUnitID = '{1}' AND GCTransactionStatus NOT IN ('{2}','{3}')",
                    Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112), 
                    //cboServiceUnit.Value, 
                    hdnID.Value,
                    Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            
            if (cboResultType.Value.ToString() == "1")
                filterExpression += " AND TransactionID NOT IN (SELECT ChargeTransactionID FROM LaboratoryResultHd WHERE IsDeleted = 0)";
            else if (cboResultType.Value.ToString() == "2")
                filterExpression += " AND TransactionID IN (SELECT ChargeTransactionID FROM LaboratoryResultHd WHERE IsDeleted = 0)";

            if (cboCoverage.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCTransactionStatus NOT IN ('{0}')", Constant.TransactionStatus.CLOSED);
            else if (cboCoverage.Value.ToString() == "2")
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", Constant.TransactionStatus.CLOSED);

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND VisitDepartmentID = '{0}'", cboDepartment.Value); 
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND VisitHealthcareServiceUnitID = '{0}'", hdnServiceUnitID.Value);
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            if (chkIsPathologicalAnatomyTest.Checked)
                filterExpression += string.Format(" AND IsPathologicalAnatomyTest = 1");
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string oTransactionID)
        {
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", oTransactionID)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.PatientName = entity.PatientName;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.RegistrationNo = entity.RegistrationNo;
            pt.BusinessPartnerID = entity.BusinessPartnerID;
            pt.LinkedRegistrationID = entity.LinkedRegistrationID;
            pt.LinkedToRegistrationID = entity.LinkedToRegistrationID;
            pt.HealthcareServiceUnitID = entity.VisitHealthcareServiceUnitID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            url = string.Format("~/Program/Worklist/LaboratoryTestResult/LaboratoryTestResultDetail.aspx?id=to|{0}", oTransactionID);
            Response.Redirect(url);
        }
    }
}