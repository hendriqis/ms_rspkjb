using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using System.Globalization;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class BPJSPatientVisitList : BasePageRegisteredPatient
    {
        private string refreshGridInterval = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.BPJS_CLAIM_PROCESS;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                List<Variable> lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Belum Diproses" });
                lstVariable.Add(new Variable { Code = "2", Value = "Sudah Diproses" });
                Methods.SetComboBoxField<Variable>(cboResultType, lstVariable, "Value", "Code");

                lstVariable = new List<Variable>();
                lstVariable.Add(new Variable { Code = "0", Value = "Semua" });
                lstVariable.Add(new Variable { Code = "1", Value = "Pulang" });
                lstVariable.Add(new Variable { Code = "2", Value = "Dirawat" });
                Methods.SetComboBoxField<Variable>(cboRegistrationStatus, lstVariable, "Value", "Code");

                List<Department> lstDept = BusinessLayer.GetDepartmentList(string.Format("DepartmentID != '{0}' AND IsActive = 1 AND IsHasRegistration=1", Constant.Facility.PHARMACY));
                Methods.SetComboBoxField<Department>(cboPatientFrom, lstDept, "DepartmentName", "DepartmentID");

                InitializeFilterParameter();
                if (AppSession.LastContentVisitListMR != null)
                {
                    LastContentVisitListMR lc = AppSession.LastContentVisitListMR;

                    cboPatientFrom.Value = lc.DepartmentID;
                    txtFromRegistrationDate.Text = DateTime.ParseExact(lc.FromDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtToRegistrationDate.Text = DateTime.ParseExact(lc.ToDate,
                              Constant.FormatString.DATE_FORMAT_112,
                               CultureInfo.InvariantCulture).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    hdnServiceUnitID.Value = lc.HealthcareServiceUnitID.ToString();
                    txtServiceUnitCode.Text = lc.ServiceUnitCode;
                    hdnQuickText.Value = lc.QuickText.Equals("Search") ? "" : lc.QuickText;
                    hdnFilterExpressionQuickSearch.Value = lc.QuickFilterExpression;
                    cboRegistrationStatus.Value = lc.RegistrationStatus;
                    cboResultType.Value = lc.ProcessStatus;
                }

                Helper.SetControlEntrySetting(txtFromRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");
                Helper.SetControlEntrySetting(txtToRegistrationDate, new ControlEntrySetting(false, false, false), "mpPatientList");

                grdRegisteredPatient.InitializeControl();

                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
            }
        }

        private void InitializeFilterParameter()
        {
            cboPatientFrom.Value = Constant.Facility.OUTPATIENT;
            hdnServiceUnitID.Value = string.Empty;
            txtServiceUnitCode.Text = string.Empty;
            txtFromRegistrationDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboRegistrationStatus.Value = "0";
            cboResultType.Value = "0";
            txtSearchView.Text = "Search";
            hdnFilterExpressionQuickSearch.Value = string.Empty;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("(VisitDate BETWEEN '{0}' AND '{1}') AND GCVisitStatus != '{2}'", Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.VisitStatus.CANCELLED);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", hdnServiceUnitID.Value);
            else if (cboPatientFrom.Value != null)
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboPatientFrom.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            string registrationStatus = cboRegistrationStatus.Value.ToString();
            if (registrationStatus == "1")
                filterExpression += string.Format(" AND GCVisitStatus = '{0}'", Constant.VisitStatus.CLOSED);
            else if (registrationStatus == "2")
                filterExpression += string.Format(" AND GCVisitStatus != '{0}'", Constant.VisitStatus.CLOSED);

            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            string id = Page.Request.QueryString["id"];
            string url = "";

            LastContentVisitListMR lc = new LastContentVisitListMR()
            {
                DepartmentID = cboPatientFrom.Value.ToString(),
                FromDate = Helper.GetDatePickerValue(txtFromRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                ToDate = Helper.GetDatePickerValue(txtToRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                HealthcareServiceUnitID = string.IsNullOrEmpty(hdnServiceUnitID.Value) ? 0 : Convert.ToInt32(hdnServiceUnitID.Value),
                ServiceUnitCode = txtServiceUnitCode.Text,
                QuickText = txtSearchView.Text,
                QuickFilterExpression = hdnFilterExpressionQuickSearch.Value,
                RegistrationStatus = cboRegistrationStatus.Value.ToString(),
                ProcessStatus = cboResultType.Value.ToString(),
            };
            AppSession.LastContentVisitListMR = lc;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", transactionNo))[0];
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

            string parentCode = Constant.MenuCode.MedicalRecord.BPJS_CLAIM_PROCESS;
            string filterExpression = string.Format("ParentCode = '{0}'", parentCode);
            List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            int parentID = (int)lstMenu.Where(p => p.MenuIndex > 0).OrderBy(p => p.MenuIndex).FirstOrDefault().MenuID;

            filterExpression = string.Format("ParentID = {0}", parentID);
            lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.MEDICAL_RECORD, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, filterExpression);
            GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
            url = Page.ResolveUrl(menu.MenuUrl);

            Response.Redirect(url);
        }

    }
}