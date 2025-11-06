using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class EpisodeSummaryPatientReferralCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int VisitID = 0;
        protected int RegistrationID = 0;
        public override void InitializeDataControl(string queryString)
        {
            if (queryString != "")
            {
                VisitID = Convert.ToInt32(queryString);
                RegistrationID = 0;
            }
            else
            {
                VisitID = AppSession.RegisteredPatient.VisitID;
                RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            if (RegistrationID == 0)
            {
                filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            }
            else
            {
                filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", RegistrationID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientReferralRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstView = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0}", VisitID));
            grdView.DataSource = lstView;
            grdView.DataBind();
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
    }
}