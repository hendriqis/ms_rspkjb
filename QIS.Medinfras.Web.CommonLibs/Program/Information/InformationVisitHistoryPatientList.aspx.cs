using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class InformationVisitHistoryPatientList : BasePageRegisteredPatient
    {
        public override string OnGetMenuCode()
        {
            if (Request.QueryString["id"] == "IP")
               return Constant.MenuCode.Inpatient.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "OP")
                return Constant.MenuCode.Outpatient.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "ER")
                return Constant.MenuCode.EmergencyCare.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "IS")
                return Constant.MenuCode.Imaging.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "RT")
                return Constant.MenuCode.Radiotheraphy.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "MD")
                return Constant.MenuCode.MedicalDiagnostic.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "LB")
                return Constant.MenuCode.Laboratory.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            if (Request.QueryString["id"] == "PH")
                return Constant.MenuCode.Pharmacy.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
            else
                return Constant.MenuCode.Inpatient.INFORMATION_VISIT_HISTORY_PATIENT_LIST;
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
                //grdInpatientReg.InitializeControl();
                ((GridPatientVisitHistory)grdInpatientReg).InitializeControl();
            }
        }
        public override string GetFilterExpression()
        {
            string filterExpression = string.Format("GCVisitStatus != '{0}'", Constant.VisitStatus.CANCELLED);

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                filterExpression += string.Format(" AND VisitDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
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