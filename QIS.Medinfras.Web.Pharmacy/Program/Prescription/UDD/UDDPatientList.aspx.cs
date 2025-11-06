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

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class UDDPatientList : BasePagePatientOrder
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.UDD_PRESCRIPTION_ENTRY;
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
        private string refreshGridInterval = "10";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT);

                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
                if (lstParam.Count > 0)
                {
                    hdnIsUsingUDD.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).FirstOrDefault().ParameterValue;
                    refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
                }

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsInpatientDispensary = 1 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                string deptFilter = string.Format("IsActive = 1 AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.Value = Constant.Facility.INPATIENT.ToString();

                SettingControlProperties();
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentPrescriptionEntry != null)
            {
                LastContentPrescriptionEntry lc = AppSession.LastContentPrescriptionEntry;
                hdnLastContentID.Value = "containerDaftar";
                cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                cboDepartment.Value = lc.FromDepartmentID;
                hdnServiceUnitID.Value = lc.FromHealthcareServiceUnitID.ToString();
                vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value)).FirstOrDefault();
                txtServiceUnitCode.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                txtServiceUnitName.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                hdnQuickTextReg.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                hdnFilterExpressionQuickSearchReg.Value = lc.QuickFilterExpression;
            }
            else
            {
                cboDepartment.Value = Constant.Facility.INPATIENT;
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("DepartmentID = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}','{4}')", cboDepartment.Value, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);

            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
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
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo)).FirstOrDefault();
            if (entity != null)
            {
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
                pt.ChargeClassID = entity.ChargeClassID;
                AppSession.RegisteredPatient = pt;
                LastContentPrescriptionEntry lc = new LastContentPrescriptionEntry()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value.ToString(),
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0"
                };
                AppSession.LastContentPrescriptionEntry = lc;
                string url = "";
                url = string.Format("~/Program/Prescription/UDD/MedicationOrder/UDDMedicationOrderEntry.aspx");
                Response.Redirect(url);
            }
        }

        public override void OnGrdRowClickTestOrder(string transactionNo, string prescriptionOrderID)
        {
        }

        public override string GetFilterExpressionTestOrder()
        {
            string filterExpression = hdnFilterExpression.Value;
            return filterExpression;
        }
    }
}