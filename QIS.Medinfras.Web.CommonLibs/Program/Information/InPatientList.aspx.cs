using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Web.CommonLibs.Controls;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InPatientList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Request.QueryString["id"] == "IP")
                return Constant.MenuCode.Inpatient.INPATIENT_LIST;
            else
                return Constant.MenuCode.Nutrition.HOSPITALIZED_PATIENT_LIST;
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

                List<GetServiceUnitUserList> lstServiceUnit = BusinessLayer.GetServiceUnitUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.Facility.INPATIENT, "IsUsingRegistration = 1");
                lstServiceUnit.Insert(0, new GetServiceUnitUserList { HealthcareServiceUnitID = 0, ServiceUnitName = "" });

                List<ClassCare> lstClass = BusinessLayer.GetClassCareList(string.Format("isDeleted = 0"));
                lstClass.Insert(0, new ClassCare { ClassID = 0, ClassName = "" });



                #region REKAP
                ((GridInpatientRegistrationAllCtl)grdInpatientRegAll).InitializeControl();
                #endregion

                #region DETAIL
                Methods.SetComboBoxField<GetServiceUnitUserList>(cboServiceUnitDetail, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnitDetail.SelectedIndex = 0;

                Methods.SetComboBoxField<ClassCare>(cboClass, lstClass, "ClassName", "ClassID");
                cboClass.SelectedIndex = 0;

                ((GridInpatientRegistrationCtl)grdInpatientReg).InitializeControl();
                #endregion
            }
        }

        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCRegistrationStatus IN ('{0}','{1}','{2}')", Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.RECEIVING_TREATMENT, Constant.VisitStatus.PHYSICIAN_DISCHARGE);
            
            if (cboServiceUnitDetail.Value != null && cboServiceUnitDetail.Value.ToString() != "0")
            {
                filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnitDetail.Value);
            }
            else
            {
                filterExpression += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            }
            string classID = Convert.ToString(cboClass.Value);

            if (classID != "0")
            {
                if (chkIsChargeClass.Checked != true)
                {
                    filterExpression += string.Format(" AND ClassID = {0}", cboClass.Value);
                }
                else
                {
                    filterExpression += string.Format(" AND ChargeClassID = {0}", cboClass.Value);
                }
            }
            if (txtPhysicianCode.Text != "")
                filterExpression += string.Format(" AND ParamedicID = {0}", hdnPhysicianID.Value);

            if (hdnBusinessPartnerID.Value != "")
            {
                filterExpression += string.Format(" AND BusinessPartnerID = {0}", hdnBusinessPartnerID.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
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