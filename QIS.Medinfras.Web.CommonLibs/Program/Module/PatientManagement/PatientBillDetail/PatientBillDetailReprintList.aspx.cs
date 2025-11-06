using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillDetailReprintList : BasePageCheckRegisteredPatient
    {
        private GetUserMenuAccess menu;
        private string refreshGridInterval = "10";

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_BILL_DETAIL_REPRINT;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_BILL_DETAIL_REPRINT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PATIENT_BILL_DETAIL_REPRINT;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PATIENT_BILL_DETAIL_REPRINT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_BILL_DETAIL_REPRINT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_BILL_DETAIL_REPRINT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.PATIENT_BILL_DETAIL_REPRINT;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_BILL_DETAIL_REPRINT;
                default: return Constant.MenuCode.EmergencyCare.PATIENT_BILL_DETAIL_REPRINT;
            }
        }

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected string GetRefreshGridInterval()
        {
            string paramFilter = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL);
            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(paramFilter);
            if (lstParam.Count > 0)
            {
                refreshGridInterval = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.FM_PATIENT_GRID_REFRESH_INTERVAL).FirstOrDefault().ParameterValue;
            }
            return refreshGridInterval;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            MPTrx master = (MPTrx)Master;
            if (!Page.IsPostBack)
            {
                menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            }
        }

        protected override void InitializeDataControl()
        {
            string departmentID = Page.Request.QueryString["id"];
            grdRegisteredPatient.InitializeControl();
        }

        public override void LoadAllWords()
        {
            LoadWords();
        }

        public override string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];

            string filterExpression = "";

            if (hdnFilterExpressionQuickSearch.Value == "Search")
            {
                hdnFilterExpressionQuickSearch.Value = "";
            }

            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                filterExpression = "1=0";
            }

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += string.Format("GCRegistrationStatus NOT IN ('{0}','{1}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED);

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != null)
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                if (filterExpression != "")
                {
                    filterExpression += " AND ";
                }
                filterExpression += string.Format("ActualVisitDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            return filterExpression;
        }
    }
}