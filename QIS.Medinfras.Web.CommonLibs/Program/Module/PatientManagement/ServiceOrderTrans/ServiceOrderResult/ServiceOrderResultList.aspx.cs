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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ServiceOrderResultList : BasePageRegisteredPatient
    {
        private GetUserMenuAccess menu;
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private string refreshGridInterval = "";

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case "eo": return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_RESULT_TRANS;
                case "op": return Constant.MenuCode.Outpatient.SERVICE_ORDER_RESULT_TRANS;
                default: return Constant.MenuCode.EmergencyCare.SERVICE_ORDER_RESULT_TRANS;
            }
        }

        public override bool IsShowRightPanel()
        {
            return false;
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
            {
                return GetLabel(menu.MenuCaption);
            }
            else
            {
                return "";
            }
        }

        protected string GetRefreshGridInterval()
        {
            return refreshGridInterval;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {

                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;

                string id = Page.Request.QueryString["id"];
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

                #region Cbo To ServiceUnit
                if (id == "eo")
                {
                    trServiceUnit.Style.Add("display", "none");
                }
                else
                {
                    List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.OUTPATIENT, "IsUsingRegistration = 1");

                    Methods.SetComboBoxField<GetServiceUnitUserList>(cboToServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                    cboToServiceUnit.SelectedIndex = 1;
                }
                #endregion

                string laboratoryID = BusinessLayer.GetSettingParameter(Constant.SettingParameter.LABORATORY_SERVICE_UNIT_ID).ParameterValue;
                hdnID.Value = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND ServiceUnitID = {1}", AppSession.UserLogin.HealthcareID, laboratoryID))[0].HealthcareServiceUnitID.ToString();

                string filterExpression = string.Format("IsUsingRegistration = 1 AND HealthcareID = '{0}' AND DepartmentID = '{1}'", AppSession.UserLogin.HealthcareID, Constant.Facility.DIAGNOSTIC);

                if (filterExpression != "") trPenunjangMedis.Style.Add("Display", "None");

                List<vHealthcareServiceUnit> lstHServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstHServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 1;
                
                List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsHasRegistration = 1 AND IsActive = 1");
                lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
                Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
                cboDepartment.SelectedIndex = 0;

                txtDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, false), "mpPatientList");
                Helper.SetControlEntrySetting(cboDepartment, new ControlEntrySetting(true, true, false), "mpPatientList");

                //////protected global::QIS.Medinfras.Web.CommonLibs.Controls.GridPatientServiceResultCtl grdPatientServiceResult;
                grdPatientServiceResult.InitializeControl();
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            filterExpression += string.Format("TransactionDate = '{0}' AND GCTransactionStatus NOT IN ('{1}','{2}')",
                    Helper.GetDatePickerValue(txtDate).ToString(Constant.FormatString.DATE_FORMAT_112),
                    Constant.TransactionStatus.OPEN, Constant.TransactionStatus.VOID);
            
            if (cboToServiceUnit.Value != null && cboToServiceUnit.Value.ToString() != "")
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboToServiceUnit.Value);
            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (hdnServiceUnitID.Value != "0" && hdnServiceUnitID.Value != "")
                filterExpression += string.Format(" AND VisitHealthcareServiceUnitID = '{0}'", hdnServiceUnitID.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            return filterExpression;
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override void OnGrdRowClick(string transactionNo)
        {
            vPatientChargesHd entityPCHD = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", transactionNo)).FirstOrDefault();
            RegisteredPatient pt = new RegisteredPatient();
            pt.RegistrationID = entityPCHD.RegistrationID;
            pt.VisitID = entityPCHD.VisitID;
            AppSession.RegisteredPatient = pt;

            string url = "";
            string id = Page.Request.QueryString["id"];
            if (id == "eo")
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/ServiceOrderTrans/ServiceOrderResult/ServiceOrderResultDetail.aspx?id=eo|{0}", transactionNo);
            }
            else
            {
                url = string.Format("~/Libs/Program/Module/PatientManagement/ServiceOrderTrans/ServiceOrderResult/ServiceOrderResultDetail.aspx?id=op|{0}", transactionNo);
            }

            Response.Redirect(url);
        }
    }
}