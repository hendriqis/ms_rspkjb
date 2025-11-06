using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TransactionPatientInformationList : BasePageRegisteredPatient
    {

        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value) 
            {
                case "INPATIENT": return Constant.MenuCode.Inpatient.TRANSACTION_PATIENT_INFORMATION;
                case "OUTPATIENT": return Constant.MenuCode.Outpatient.TRANSACTION_PATIENT_INFORMATION;
                case "EMERGENCY": return Constant.MenuCode.EmergencyCare.TRANSACTION_PATIENT_INFORMATION;
            }
            return "";
        }

        public void SetControlVisibility() 
        {
            switch (hdnDepartmentID.Value) 
            {
                case "INPATIENT": trRegistrationDate.Style.Add("display", "none"); break;
                case "EMERGENCY": trServiceUnitName.Style.Add("display", "none"); break;
                case "DIAGNOSTIC": if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                        trServiceUnitName.Style.Add("display", "none"); break;
            }
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

                hdnDepartmentID.Value = Page.Request.QueryString["id"];
                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnDepartmentID.Value, "IsUsingRegistration = 1");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;

                txtRegistrationDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                
                SetControlVisibility();
                BindGridView(CurrPage, true, ref PageCount);
            }
        }

        protected string GetServiceUnitLabel()
        {
            if (hdnDepartmentID.Value == Constant.Facility.INPATIENT)
                return GetLabel("Ruang Perawatan");
            else
                return GetLabel("Klinik");
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvRegistrationOutstandingInfoRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vRegistrationOutstandingInfo> lstEntity = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "RegistrationID ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override string GetFilterExpression()
        {
            string filterExpression = "";

            if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType != MedicalDiagnosticType.OtherMedicalDiagnostic)
                    filterExpression += string.Format("HealthcareServiceUnitID = {0}", AppSession.MedicalDiagnostic.HealthcareServiceUnitID);
                else
                    filterExpression += string.Format("DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1}, {2})", Constant.Facility.DIAGNOSTIC, AppSession.MedicalDiagnostic.LaboratoryHealthcareServiceUnitID, AppSession.MedicalDiagnostic.ImagingHealthcareServiceUnitID);
            }
            else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            else
            {
                int cboServiceUnitVal = Convert.ToInt32(cboServiceUnit.Value);
                if (cboServiceUnitVal != 0)
                    filterExpression += string.Format("HealthcareServiceUnitID = {0}", cboServiceUnitVal);
                else
                    filterExpression = string.Format("DepartmentID = '{0}'", hdnDepartmentID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (hdnDepartmentID.Value != Constant.Facility.INPATIENT)
                filterExpression += string.Format(" AND RegistrationDate = '{0}'", Helper.GetDatePickerValue(txtRegistrationDate.Text).ToString(Constant.FormatString.DATE_FORMAT_112));
            filterExpression += string.Format(" AND GCRegistrationStatus NOT IN ('{0}', '{1}', '{2}')", Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, Constant.VisitStatus.PHYSICIAN_DISCHARGE);
            return filterExpression;
        }

        public override void OnGrdRowClick(string transactionNo)
        {
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }
    }
}