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
    public partial class MedicalSummaryContent9Ctl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        
        protected int _visitID = 0;
        protected int _registrationID = 0;
        protected int _linkedRegistrationID = 0;

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
            _linkedRegistrationID = registeredPatient.LinkedRegistrationID;

            BindGridView(1, true, ref PageCount);
            BindGridViewDt(1, true, ref PageCount);

        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND StandardCodeID IN (SELECT DISTINCT GCAssessmentType FROM vPatientAssessment WITH (NOLOCK) WHERE RegistrationID IN ({1},{2}))", Constant.StandardCode.PATIENT_ASSESSMENT_FORM, AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.LinkedRegistrationID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_DEFAULT, pageIndex);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        protected void cbpFormList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion

        #region Detail
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("RegistrationID IN ({0},{1}) AND GCAssessmentType = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.LinkedRegistrationID, hdnGCAssessmentType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}