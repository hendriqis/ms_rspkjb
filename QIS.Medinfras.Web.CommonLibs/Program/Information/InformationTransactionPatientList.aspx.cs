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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class InformationTransactionPatientList : BasePageContent
    {
        private GetUserMenuAccess menu;
        private string refreshGridInterval = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param[0] == "MD")
            //if (Page.Request.QueryString["id"] == "MD")
                return Constant.MenuCode.MedicalDiagnostic.INFORMATION_TRANSACTION_PATIENT_MD080230;
            else
                return Constant.MenuCode.SystemSetup.INFORMATION_TRANSACTION_PATIENT;
        }

        //public override string OnGetMenuCode()
        //{
        //    return Constant.MenuCode.SystemSetup.INFORMATION_TRANSACTION_PATIENT;
        //}

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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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

            filterExpression += string.Format(" AND GCVisitStatus != '{0}' AND GCRegistrationStatus != '{0}'", Constant.VisitStatus.CANCELLED);

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
            string url = string.Format("InformationTransactionPatientDetail.aspx?id={0}|{1}",
                    id, entity.VisitID);
            return url;
        }
    }
}