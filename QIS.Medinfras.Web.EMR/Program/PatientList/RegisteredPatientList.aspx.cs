using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class RegisteredPatientList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.REGISTERED_PATIENT;
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

                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboVisitStatus, lstVariable, "Value", "Code");
                cboVisitStatus.Value = "1";

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1", Constant.Facility.PHARMACY));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");

                if (AppSession.UserLogin.DepartmentID != null)
                    cboPatientFrom.Value = AppSession.UserLogin.DepartmentID;
                else
                    cboPatientFrom.SelectedIndex = 0;

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "");
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");
                Helper.SetControlEntrySetting(cboServiceUnit, new ControlEntrySetting(true, true, true), "mpPatientList");
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("VisitDate = '{0}' AND HealthcareServiceUnitID = {1} AND RegistrationNo LIKE '%{2}%' ", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), cboServiceUnit.Value, txtRegistrationNo.Text);
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);
            //, Constant.RegistrationStatus.CANCELLED, Constant.RegistrationStatus.PHYSICIAN_DISCHARGE, Constant.RegistrationStatus.CLOSED, Constant.RegistrationStatus.OPEN
            if (cboVisitStatus.Value.ToString() == "0")
                filterExpression += string.Format(" AND GCVisitStatus IN ('{0}', '{1}') ", Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.PHYSICIAN_DISCHARGE);
            else if (cboVisitStatus.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCVisitStatus IN ('{0}') ", Constant.VisitStatus.CHECKED_IN);
            else 
                filterExpression += string.Format(" AND GCVisitStatus IN ('{0}') ", Constant.VisitStatus.PHYSICIAN_DISCHARGE);

            return filterExpression;
        }
        

        public override void OnGrdRowClick(string transactionNo)
        {
            vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", transactionNo))[0];
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.MedicalNo = entity.MedicalNo;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            pt.VisitDate = entity.VisitDate;
            pt.VisitTime = entity.VisitTime;
            pt.ParamedicID = entity.ParamedicID;
            pt.SpecialtyID = entity.SpecialtyID;
            pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
            pt.DepartmentID = entity.DepartmentID;
            pt.ClassID = entity.ClassID;
            AppSession.RegisteredPatient = pt;

            Response.Redirect("~/Program/PatientPage/PatientDataView.aspx");
        }
    }
}