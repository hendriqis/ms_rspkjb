using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class MedicalAssessmentFormList1 : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (!Page.IsPostBack)
            {
                if (entity != null)
                {
                    hdnPageMedicalNo.Value = entity.MedicalNo;
                    hdnPagePatientDOB.Value = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                    hdnPagePatientName.Value = entity.PatientName;
                    hdnPageRegistrationNo.Value = entity.RegistrationNo;
                }

                BindGridView(1, true, ref PageCount);
                BindGridViewDt(1, true, ref PageCount);

                BindGridView1(1, true, ref PageCount);
                BindGridViewDt1(1, true, ref PageCount);
            }
        }

        #region Header
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";

            if (rblItemType.SelectedValue == "1")
            {
                filterExpression += string.Format("StandardCodeID IN (SELECT DISTINCT GCAssessmentType FROM PatientAssessment vp WITH (NOLOCK) WHERE vp.VisitID = {0}  AND IsDeleted = 0)", AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                filterExpression += string.Format("StandardCodeID IN (SELECT DISTINCT GCAssessmentType FROM vPatientAssessment vp WITH(NOLOCK) INNER JOIN Registration r WITH(NOLOCK) ON r.RegistrationID = vp.RegistrationID WHERE r.MRN = {0}  AND vp.IsDeleted = 0)", AppSession.RegisteredPatient.MRN);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_DEFAULT, pageIndex);
            grdView.DataSource = lstEntity;
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


        private void BindGridView1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "";
            if (filterExpression != "")
                filterExpression += " AND ";

            if (rblItemType1.SelectedValue == "1")
            {
                filterExpression += string.Format("StandardCodeID IN (SELECT DISTINCT GCAssessmentType FROM PopulationAssessment vp WITH (NOLOCK) WHERE vp.VisitID = {0}  AND IsDeleted = 0)", AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                filterExpression += string.Format("StandardCodeID IN (SELECT DISTINCT GCAssessmentType FROM vPopulationAssessment vp WITH(NOLOCK) INNER JOIN Registration r WITH(NOLOCK) ON r.RegistrationID = vp.RegistrationID WHERE r.MRN = {0}  AND vp.IsDeleted = 0)", AppSession.RegisteredPatient.MRN);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetStandardCodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression, Constant.GridViewPageSize.GRID_DEFAULT, pageIndex);
            grdView1.DataSource = lstEntity;
            grdView1.DataBind();
        }

        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView1(1, true, ref pageCount);
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

            if (rblItemType.SelectedValue == "1")
            {
                filterExpression += string.Format("GCAssessmentType = '{0}' AND VisitID IN (SELECT DISTINCT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.VisitID = {1}) AND IsDeleted = 0", hdnGCAssessmentTypeCBCtl.Value, AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                filterExpression += string.Format("GCAssessmentType = '{0}' AND RegistrationID IN (SELECT DISTINCT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.MRN = {1}) AND IsDeleted = 0", hdnGCAssessmentTypeCBCtl.Value, AppSession.RegisteredPatient.MRN);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
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

        private void BindGridViewDt1(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            if (rblItemType1.SelectedValue == "1")
            {
                filterExpression += string.Format("GCAssessmentType = '{0}' AND VisitID IN (SELECT DISTINCT cv.VisitID FROM ConsultVisit cv WITH(NOLOCK) WHERE cv.VisitID = {1}) AND IsDeleted = 0", hdnGCAssessmentTypeCBCtl1.Value, AppSession.RegisteredPatient.VisitID);
            }
            else
            {
                filterExpression += string.Format("GCAssessmentType = '{0}' AND RegistrationID IN (SELECT DISTINCT r.RegistrationID FROM Registration r WITH(NOLOCK) WHERE r.MRN = {1}) AND IsDeleted = 0", hdnGCAssessmentTypeCBCtl1.Value, AppSession.RegisteredPatient.MRN);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPopulationAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPopulationAssessment> lstEntity = BusinessLayer.GetvPopulationAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdViewDt1.DataSource = lstEntity;
            grdViewDt1.DataBind();
        }
        protected void cbpViewDt1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt1(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt1(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}