using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryContent7Ctl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int _visitID = 0;
        protected int _registrationID = 0;

        public override void InitializeDataControl(string queryString)
        {
            if (queryString != "")
                _visitID = Convert.ToInt32(queryString);
            else
                _visitID = AppSession.RegisteredPatient.VisitID;

            LoadContentInformation(Convert.ToInt32(queryString));
        }

        private void LoadContentInformation(int visitID)
        {
            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", visitID))[0];
            _registrationID = registeredPatient.RegistrationID;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("RegistrationID = {0} AND IsDeleted = 0", _registrationID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientReferralRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstView = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID));
            grdContent7View.DataSource = lstView;
            grdContent7View.DataBind();
        }

        protected void grdContent7View_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected void cbpContent7View_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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