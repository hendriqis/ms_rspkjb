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
    public partial class CorrectionTransactionInformationList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.CORRECTION_TRANSACTION_INFORMATION;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.CORRECTION_TRANSACTION_INFORMATION;
                default: return Constant.MenuCode.Imaging.CORRECTION_TRANSACTION_INFORMATION;
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

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsHasRegistration = 1 AND IsActive = 1"));
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
            string filterExpression = string.Format("GCVisitStatus <> '{0}' AND VisitID IN (SELECT VisitID FROM PatientChargesHd WHERE IsCorrectionTransaction = 1) AND DepartmentID = '{1}'", Constant.VisitStatus.CANCELLED, departmentID);
            
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
            string healthcareServiceUnitID = "";
            if (hdnModuleID.Value == Constant.Module.LABORATORY || hdnModuleID.Value == Constant.Module.IMAGING)
                healthcareServiceUnitID = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
            else
                healthcareServiceUnitID = hdnServiceUnitID.Value.ToString();

            if (String.IsNullOrEmpty(healthcareServiceUnitID))
            {
                vConsultVisit4 oVisit = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", Convert.ToInt32(transactionNo))).FirstOrDefault();
                if (oVisit != null)
                {
                    healthcareServiceUnitID = oVisit.HealthcareServiceUnitID.ToString();

                    RegisteredPatient pt = new RegisteredPatient();
                    pt.MRN = oVisit.MRN;
                    pt.PatientName = oVisit.PatientName;
                    pt.MedicalNo = oVisit.MedicalNo;
                    pt.RegistrationID = oVisit.RegistrationID;
                    pt.RegistrationNo = oVisit.RegistrationNo;
                    pt.VisitID = oVisit.VisitID;
                    pt.VisitDate = oVisit.VisitDate;
                    pt.VisitTime = oVisit.VisitTime;
                    pt.ParamedicID = oVisit.ParamedicID;
                    pt.ParamedicCode = oVisit.ParamedicCode;
                    pt.ParamedicName = oVisit.ParamedicName;
                    pt.SpecialtyID = oVisit.SpecialtyID;
                    pt.HealthcareServiceUnitID = oVisit.HealthcareServiceUnitID;
                    pt.ServiceUnitName = oVisit.ServiceUnitName;
                    pt.RoomCode = oVisit.RoomCode;
                    pt.BedCode = oVisit.BedCode;
                    pt.DepartmentID = oVisit.DepartmentID;
                    pt.ClassID = oVisit.ClassID;
                    pt.ChargeClassID = oVisit.ChargeClassID;
                    pt.GCRegistrationStatus = oVisit.GCVisitStatus;
                    pt.IsLockDown = oVisit.IsLockDown;
                    pt.LinkedRegistrationID = oVisit.LinkedRegistrationID;
                    pt.GCRegistrationStatus = oVisit.GCRegistrationStatus;
                    pt.DepartmentID = oVisit.DepartmentID;
                    pt.StartServiceDate = oVisit.StartServiceDate;
                    pt.StartServiceTime = oVisit.StartServiceTime;
                    AppSession.RegisteredPatient = pt;
                }
            }

            if (healthcareServiceUnitID != "" || healthcareServiceUnitID != "0")
            {
                string url = "";
                url = string.Format("~/Libs/Program/Information/CorrectionTransactionDetailInformation.aspx?id={0}|{1}", healthcareServiceUnitID, transactionNo);
                Response.Redirect(url); 
            }
        }
    }
}