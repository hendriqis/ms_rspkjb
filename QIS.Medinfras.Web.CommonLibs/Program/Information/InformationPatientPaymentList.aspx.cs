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
    public partial class InformationPatientPaymentList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            switch (hdnModuleID.Value)
            {
                case Constant.Module.OUTPATIENT: return Constant.MenuCode.Outpatient.PAYMENT_INFORMATION;
                case Constant.Module.INPATIENT: return Constant.MenuCode.Inpatient.PAYMENT_INFORMATION;
                case Constant.Module.EMERGENCY: return Constant.MenuCode.EmergencyCare.PAYMENT_INFORMATION;
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.PAYMENT_INFORMATION;
                case Constant.Module.MEDICAL_DIAGNOSTIC: return Constant.MenuCode.MedicalDiagnostic.PAYMENT_INFORMATION;
                case Constant.Module.LABORATORY: return Constant.MenuCode.Laboratory.PAYMENT_INFORMATION;
                case Constant.Module.IMAGING: return Constant.MenuCode.Imaging.PAYMENT_INFORMATION;
                default: return Constant.MenuCode.Outpatient.PAYMENT_INFORMATION;
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
                string moduleName = Helper.GetModuleName();
                string ModuleID = Helper.GetModuleID(moduleName);
                hdnModuleID.Value = ModuleID;

                string DeptID = "";

                if (ModuleID == Constant.Module.OUTPATIENT)
                {
                    DeptID = Constant.Facility.OUTPATIENT;
                }
                else if (ModuleID == Constant.Module.INPATIENT)
                {
                    DeptID = Constant.Facility.INPATIENT;
                }
                else if (ModuleID == Constant.Module.EMERGENCY)
                {
                    DeptID = Constant.Facility.EMERGENCY;
                }
                else if (ModuleID == Constant.Module.MEDICAL_CHECKUP)
                {
                    DeptID = Constant.Facility.MEDICAL_CHECKUP;
                }
                else if (ModuleID == Constant.Module.PHARMACY)
                {
                    DeptID = Constant.Facility.PHARMACY;
                }
                else
                {
                    DeptID = Constant.Facility.DIAGNOSTIC;
                }

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("IsActive = 1 AND IsHasRegistration = 1 AND DepartmentID = '{0}'", DeptID));
                lstDept = lstDept.OrderBy(lst => lst.TabOrder).ToList();
                Methods.SetComboBoxField<Department>(cboDepartment, lstDept, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                if (ModuleID == Constant.Module.INPATIENT)
                {
                    List<Variable> lstVariable = new List<Variable>();
                    lstVariable.Add(new Variable { Code = "0", Value = "Masih Dirawat" });
                    lstVariable.Add(new Variable { Code = "1", Value = "Rencana Pulang" });
                    lstVariable.Add(new Variable { Code = "2", Value = "Sudah Pulang" });
                    lstVariable.Add(new Variable { Code = "3", Value = "Belum Dikonfirmasi" });
                    lstVariable.Add(new Variable { Code = "4", Value = "Tutup" });
                    Methods.SetComboBoxField<Variable>(cboCheckinStatus, lstVariable, "Value", "Code");
                    cboCheckinStatus.SelectedIndex = 0;
                }
                else
                {
                    List<Variable> lstVariable = new List<Variable>();
                    lstVariable.Add(new Variable { Code = "0", Value = "Check-In" });
                    lstVariable.Add(new Variable { Code = "1", Value = "Open" });
                    lstVariable.Add(new Variable { Code = "2", Value = "Receiving Treatment" });
                    lstVariable.Add(new Variable { Code = "3", Value = "Physician Discharge" });
                    lstVariable.Add(new Variable { Code = "3", Value = "Discharge" });
                    lstVariable.Add(new Variable { Code = "4", Value = "Closed" });

                    Methods.SetComboBoxField<Variable>(cboCheckinStatus, lstVariable, "Value", "Code");
                    cboCheckinStatus.SelectedIndex = 0;
                }

                #endregion

                List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(string.Format(
                    "ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM));

                hdnIS.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
                hdnLB.Value = lstSetvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;

                if (hdnModuleID.Value == "IS") {
                    hdnServiceUnitID.Value = hdnIS.Value;
                    vHealthcareServiceUnit vhsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareServiceUnitID = '{0}'", hdnIS.Value)).FirstOrDefault();
                    txtServiceUnitCode.Text = vhsu.ServiceUnitCode;
                    txtServiceUnitName.Text = vhsu.ServiceUnitName;
                }
                else if (hdnModuleID.Value == "LB")
                {
                    hdnServiceUnitID.Value = hdnLB.Value;
                    vHealthcareServiceUnit vhsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format(
                    "HealthcareServiceUnitID = '{0}'", hdnLB.Value)).FirstOrDefault();
                    txtServiceUnitCode.Text = vhsu.ServiceUnitCode;
                    txtServiceUnitName.Text = vhsu.ServiceUnitName;
                }

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
                ((GridPatientRegOrderCtl)grdRegisteredPatient).InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);
            string departmentID = cboDepartment.Value.ToString();
            string filterExpression = string.Format(" DepartmentID = '{0}'", departmentID);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);

            if (departmentID == Constant.Facility.DIAGNOSTIC && (hdnServiceUnitID.Value == "0" || hdnServiceUnitID.Value == ""))
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID NOT IN ({0},{1})", hdnLB.Value, hdnIS.Value);
            }

            if (departmentID != Constant.Facility.INPATIENT)
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", Helper.GetDatePickerValue(txtRealisationDate).ToString(Constant.FormatString.DATE_FORMAT_112));   
            }

            if (hdnFilterExpressionQuickSearchReg.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);
            }

            if (ModuleID == Constant.Module.INPATIENT)
            {
                if (cboCheckinStatus.Value.ToString() == "0")
                {
                    filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "1")
                {
                    filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}') AND IsPlanDischarge = 1",
                        Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "2")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.DISCHARGED);
                }
                else if (cboCheckinStatus.Value.ToString() == "3")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.OPEN);
                }
                else
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                }
            }
            else
            {
                if (cboCheckinStatus.Value.ToString() == "0")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CHECKED_IN);
                }
                else if (cboCheckinStatus.Value.ToString() == "1")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.OPEN);
                }
                else if (cboCheckinStatus.Value.ToString() == "2")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.RECEIVING_TREATMENT);
                }
                else if (cboCheckinStatus.Value.ToString() == "3")
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.PHYSICIAN_DISCHARGE);
                }
                else
                {
                    filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
                };
            }
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
                url = string.Format("InformationPatientPayment.aspx?id={0}|{1}", healthcareServiceUnitID, transactionNo);
                Response.Redirect(url); 
            }
        }
    }
}