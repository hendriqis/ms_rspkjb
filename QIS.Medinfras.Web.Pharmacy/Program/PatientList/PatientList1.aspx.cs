using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Pharmacy.Program
{
    public partial class PatientList1 : BasePageRegisteredPatient2
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL;
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
                string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL, Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT);

                List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
                if (lstParam.Count > 0)
                {
                    hdnIsUsingUDD.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_USING_UDD_FOR_INPATIENT).FirstOrDefault().ParameterValue;
                    refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
                }

                string filterExpression = string.Format("HealthcareID = '{0}' AND DepartmentID = '{1}' AND IsUsingRegistration = 1 ", AppSession.UserLogin.HealthcareID, Constant.Facility.PHARMACY);

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 1;

                string deptFilter = string.Format("IsHasRegistration = 1 AND IsActive = 1 AND DepartmentID != '{0}'", Constant.Facility.PHARMACY);

                List<Department> lstDepartment = BusinessLayer.GetDepartmentList(deptFilter);
                List<Department> lstDepartmentOrder = BusinessLayer.GetDepartmentList(deptFilter);
                lstDepartmentOrder.Insert(0, new Department { DepartmentName = string.Format("{0}", GetLabel(" ")) });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;


                hdnLastContentID.Value = "containerByOrder";
                txtDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                SettingControlProperties();
                grdRegisteredPatient.InitializeControl();

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        private void SettingControlProperties()
        {
            if (AppSession.LastContentFarmasiKlinis != null)
            {
                LastContentFarmasiKlinis lc = AppSession.LastContentFarmasiKlinis;

                hdnLastContentID.Value = "containerDaftar";
                cboServiceUnit.Value = lc.HealthcareServiceUnitID.ToString();
                cboDepartment.Value = lc.FromDepartmentID;
                hdnServiceUnitID.Value = lc.FromHealthcareServiceUnitID.ToString();
                vHealthcareServiceUnit entityHealthcareServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value)).FirstOrDefault();
                txtServiceUnitCode.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitCode;
                txtServiceUnitName.Text = entityHealthcareServiceUnit == null ? "" : entityHealthcareServiceUnit.ServiceUnitName;
                hdnQuickTextReg.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                txtDate.Text = lc.Date;
                hdnFilterExpressionQuickSearchReg.Value = lc.QuickFilterExpression;
            }
        }

        public override string GetFilterExpression2()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            else
                filterExpression += string.Format("VisitDate = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}')", Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            if (cboDepartment.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearchReg.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearchReg.Value);


            return filterExpression;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            if (cboDepartment.Value != null && cboDepartment.Value.ToString() == "INPATIENT")
            {
                filterExpression += string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}','{3}')",
                                                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.DISCHARGED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            }
            else
            {
                filterExpression += string.Format("VisitDate = '{0}' AND GCVisitStatus NOT IN ('{1}','{2}','{3}')",
                                                    Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                                                    Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.OPEN);
            }

            if (cboDepartment.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
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
                pt.PatientName = entity.PatientName;
                pt.RegistrationID = entity.RegistrationID;
                pt.VisitID = entity.VisitID;
                pt.VisitDate = entity.VisitDate;
                pt.VisitTime = entity.VisitTime;
                pt.ParamedicID = entity.ParamedicID;
                pt.ParamedicCode = entity.ParamedicCode;
                pt.ParamedicName = entity.ParamedicName;
                pt.IsLockDown = entity.IsLockDown;
                pt.SpecialtyID = entity.SpecialtyID;
                pt.HealthcareServiceUnitID = entity.HealthcareServiceUnitID;
                pt.DepartmentID = entity.DepartmentID;
                pt.ClassID = entity.ClassID;
                pt.ChargeClassID = entity.ChargeClassID;
                AppSession.RegisteredPatient = pt;

                AppSession.LastContentFarmasiKlinis = null;

                LastContentFarmasiKlinis lc = new LastContentFarmasiKlinis()
                {
                    ContentID = hdnLastContentID.Value,
                    HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value),
                    FromDepartmentID = cboDepartment.Value != null ? cboDepartment.Value.ToString() : "",
                    FromHealthcareServiceUnitID = hdnServiceUnitID.Value == "" ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                    Date = txtDate.Text,
                    QuickText = txtSearchViewReg.Text,
                    QuickFilterExpression = hdnFilterExpressionQuickSearchReg.Value,
                    VariableDisplay = "0",
                    PrescriptionType = ""
                };
                AppSession.LastContentFarmasiKlinis = lc;

                string parentCode = "";

                parentCode = Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL;
                string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

                filterExpression = string.Format("ParentID = {0}", parentID);
                lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.PHARMACY, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                string url = Page.ResolveUrl(menu.MenuUrl);
                Response.Redirect(url);
            }
        }
    }
}