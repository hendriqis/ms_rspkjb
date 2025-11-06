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
    public partial class PaymentReceiptReprintList : BasePageContent
    {
        private GetUserMenuAccess menu;
        private string refreshGridInterval = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            switch (id)
            {
                case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.PAYMENT_RECEIPT_REPRINT;
                case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.PAYMENT_RECEIPT_REPRINT;
                case Constant.Facility.PHARMACY: return Constant.MenuCode.Pharmacy.PAYMENT_RECEIPT_REPRINT;
                case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.PAYMENT_RECEIPT_REPRINT;
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                        return Constant.MenuCode.Laboratory.PAYMENT_RECEIPT_REPRINT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                        return Constant.MenuCode.Imaging.PAYMENT_RECEIPT_REPRINT;
                    else if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Radiotheraphy)
                        return Constant.MenuCode.Radiotheraphy.PAYMENT_RECEIPT_REPRINT;
                    return Constant.MenuCode.MedicalDiagnostic.PAYMENT_RECEIPT_REPRINT;
                default: return Constant.MenuCode.EmergencyCare.PAYMENT_RECEIPT_REPRINT;
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
            return refreshGridInterval;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                menu = ((MPMain)Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
                refreshGridInterval = BusinessLayer.GetSettingParameter(Constant.SettingParameter.PATIENT_GRID_REFRESH_INTERVAL).ParameterValue;
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

        private string GetFilterExpression()
        {
            string id = Page.Request.QueryString["id"];
            string filterExpression = "";

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != "Search" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression = string.Format("{0}", hdnFilterExpressionQuickSearch.Value);
            }
            else
            {
                filterExpression = string.Format("ActualVisitDate = '{0}'", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            filterExpression += string.Format(" AND IsMainVisit = 1 AND GCVisitStatus = '{0}' AND GCRegistrationStatus = '{0}'", Constant.VisitStatus.CLOSED);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "RegistrationID");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value != "")
            {
                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnID.Value)).FirstOrDefault();
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
            string url = string.Format("~/Libs/Program/Module/PatientManagement/PatientBillDetail/PaymentReceiptReprintDetail.aspx?id={0}|{1}", id, entity.RegistrationID);
            return url;
        }
    }
}