using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientBillDetailReprintList2 : BasePageContent
    {
        private GetUserMenuAccess menu;
        private string refreshGridInterval = "10";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PATIENT_BILL_DETAIL_REPRINT_2;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PATIENT_BILL_DETAIL_REPRINT_2;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PATIENT_BILL_DETAIL_REPRINT_2;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PATIENT_BILL_DETAIL_REPRINT_2;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PATIENT_BILL_DETAIL_REPRINT_2;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PATIENT_BILL_DETAIL_REPRINT_2;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.PATIENT_BILL_DETAIL_REPRINT_2;
                    return Constant.MenuCode.MedicalDiagnostic.PATIENT_BILL_DETAIL_REPRINT_2;
                default: return Constant.MenuCode.EmergencyCare.PATIENT_BILL_DETAIL_REPRINT_2;
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
            if (!Page.IsPostBack)
            {
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                BindGridView(1, true, ref PageCount);
            }
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "RegistrationDate, RegistrationID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = "";

            //if (id == Constant.Facility.OUTPATIENT)
            //{
            //    filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.OUTPATIENT);
            //}
            //else if (id == Constant.Facility.EMERGENCY)
            //{
            //    filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.EMERGENCY);
            //}
            //else if (id == Constant.Facility.INPATIENT)
            //{
            //    filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.INPATIENT);
            //}
            //else if (id == Constant.Facility.PHARMACY)
            //{
            //    filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.PHARMACY);
            //}
            //else if (id == Constant.Facility.DIAGNOSTIC)
            //{
            //    filterExpression += string.Format("DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC);
            //}
            //else
            //{
            //    filterExpression += string.Format("DepartmentID IS NOT NULL");
            //}

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression = string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                filterExpression = string.Format("ActualVisitDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            filterExpression += string.Format(" AND GCVisitStatus NOT IN ('{0}','{1}') AND GCRegistrationStatus NOT IN ('{0}','{1}')", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED);

            return filterExpression;
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value != "")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", hdnID.Value))[0];
                Response.Redirect(GetResponseRedirectUrl(entity));
            }
        }

        private string GetResponseRedirectUrl(vConsultVisit entity)
        {
            RegisteredPatient pt = new RegisteredPatient();
            pt.MRN = entity.MRN;
            pt.RegistrationID = entity.RegistrationID;
            pt.VisitID = entity.VisitID;
            AppSession.RegisteredPatient = pt;

            string id = Page.Request.QueryString["id"];
            string url = string.Format("~/Libs/Program/Module/PatientManagement/PatientBillDetail/PatientBillDetailReprintList2Detail.aspx?id={0}|{1}",
                    id, entity.VisitID);
            return url;
        }
    }
}