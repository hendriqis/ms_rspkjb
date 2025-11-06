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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationRegistrationPatient : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "OP": return Constant.MenuCode.Outpatient.INFORMATION;
                case "ER": return Constant.MenuCode.EmergencyCare.REGISTERED_PATIENT_LIST;
                case "MD":
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.INFORMATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.INFORMATION;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.INFORMATION_REGISTERED_PATIENT;
                    return Constant.MenuCode.MedicalDiagnostic.INFORMATION;
                case "PH": return Constant.MenuCode.Pharmacy.INFORMATION;
                default: return Constant.MenuCode.Outpatient.INFORMATION;
            }
        }

        protected string OnGetServiceUnitLabel()
        {
            string id = Page.Request.QueryString["id"];
            if (id == Constant.Facility.DIAGNOSTIC)
                return GetLabel("Penunjang Medis");
            return GetLabel("Klinik");
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

                string departmentID = Page.Request.QueryString["id"];
                cboServiceUnit.SelectedIndex = 0;
                hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.VISIT_STATUS));
                lstStandardCode.Insert(0, new StandardCode() { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboVisitStatus, lstStandardCode, "StandardCodeName", "StandardCodeID");
                cboVisitStatus.SelectedIndex = 0;

                List<StandardCode> lstStandardCodeCustomer = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsActive = 1", Constant.StandardCode.CUSTOMER_TYPE));
                lstStandardCodeCustomer.Insert(0, new StandardCode() { StandardCodeID = "", StandardCodeName = "" });
                Methods.SetComboBoxField<StandardCode>(cboCustomerType, lstStandardCodeCustomer, "StandardCodeName", "StandardCodeID");
                cboCustomerType.SelectedIndex = 0;

                if (departmentID == "MD" && (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy))
                {
                    trServiceUnit.Style.Add("display", "none");
                    hdnHealthcareServiceUnitID.Value = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                }
                else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.OtherMedicalDiagnostic)
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND HealthcareServiceUnitID NOT IN ('{2}','{3}','{4}') AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, AppSession.RT0001);
                    List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                    Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }
                else if (departmentID == "OP")
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.OUTPATIENT);
                    List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                    lstHSU.Insert(0, new vHealthcareServiceUnitCustom() { ServiceUnitName = "", ServiceUnitID = 0 });
                    Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }
                else if (departmentID == "ER")
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.EMERGENCY);
                    List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                    lstHSU.Insert(0, new vHealthcareServiceUnitCustom() { ServiceUnitName = "", ServiceUnitID = 0 });
                    Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }
                else if (departmentID == "PH")
                {
                    string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);
                    List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterExpression);
                    Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnit, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboServiceUnit.SelectedIndex = 0;
                }

                int count = BusinessLayer.GetServiceUnitParamedicRowCount(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value));
                if (count > 0)
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "1";
                else
                    hdnIsHealthcareServiceUnitHasParamedic.Value = "0";

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string departmentID = Page.Request.QueryString["id"];
            string medicSupport = "";
            string deptID = "";
            string filterExpression = "";

            if (departmentID == "OP")
            {
                deptID = "OUTPATIENT";
            }
            else if (departmentID == "ER")
            {
                deptID = "EMERGENCY";
            }
            else if (departmentID == "PH")
            {
                deptID = "PHARMACY";
            }
            else
            {
                //                deptID = AppSession.MedicalDiagnostic.MedicalDiagnosticType.ToString();
                deptID = "DIAGNOSTIC";
            }

            if (departmentID == "MD" && (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory || AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy))
            {
                medicSupport = AppSession.MedicalDiagnostic.HealthcareServiceUnitID.ToString();
                filterExpression = string.Format("VisitDate = '{0}' ", Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else if (departmentID == "MD" && (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.Imaging && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.Laboratory && AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.Radiotheraphy))
            {
                medicSupport = cboServiceUnit.Value.ToString();
                filterExpression = string.Format("DepartmentID = 'DIAGNOSTIC' AND VisitDate = '{0}' AND HealthcareServiceUnitID NOT IN ({1},{2},{3})",
                    Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID,
                    AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID,
                    AppSession.RT0001);
            }
            else
            {
                medicSupport = cboServiceUnit.Value.ToString();
                filterExpression = string.Format("DepartmentID = '{0}' AND VisitDate = '{1}' ", deptID, Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (medicSupport != "0" && medicSupport != "")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", medicSupport);
            }

            if (cboVisitStatus.Value != null)
            {
                if (cboVisitStatus.Value.ToString() != "")
                {
                    if (filterExpression != "")
                    {
                        filterExpression += string.Format(" AND GCVisitStatus = '{0}'", cboVisitStatus.Value.ToString());
                    }
                }
            }

            if (cboCustomerType.Value != null)
            {
                if (cboCustomerType.Value.ToString() != "")
                {
                    if (filterExpression != "")
                    {
                        filterExpression += string.Format(" AND GCCustomerType = '{0}'", cboCustomerType.Value.ToString());
                    }
                }
            }

            //Cek Quick Search
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }
    }
}