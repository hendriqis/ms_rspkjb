using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class Hemodialysis1 : BasePage
    {
        protected int gridView1PageCount = 1;
        protected int gridView2PageCount = 1;
        protected int gridViewFormListPageCount = 1;       
        protected List<vVitalSignDt> lstVitalSignDt = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                hdnPageMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
                hdnPagePatientDOB.Value = AppSession.RegisteredPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnPagePatientName.Value = AppSession.RegisteredPatient.PatientName;
                if (hdnPagePatientName.Value == "" || hdnPagePatientName.Value == null)
                {
                    Patient entityP = BusinessLayer.GetPatientList(string.Format("MedicalNo = '{0}'", hdnPageMedicalNo.Value)).FirstOrDefault();
                    hdnPagePatientName.Value = entityP.FullName;
                }
                hdnPageRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;

                BindGridView(1, true, ref gridView1PageCount);
                BindGridViewFormList(1, true, ref gridView2PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPreHDAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPreHDAssessment> lstEntity = BusinessLayer.GetvPreHDAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            grdHistoryView.DataSource = lstEntity;
            grdHistoryView.DataBind();
        }

        private void BindGridViewFormList(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.FORM_PENGKAJIAN_HEMODIALISA);

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        private void BindGridViewFormListDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            string filterExpression = string.Format("MRN = {0} AND GCAssessmentType = '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.MRN,  hdnGCAssessmentType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdViewFormDt.DataSource = lstEntity;
            grdViewFormDt.DataBind();
        }

        private void BindGridViewDt3(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (!string.IsNullOrEmpty(hdnAssessmentID.Value) && hdnAssessmentID.Value != "0")
            {
                string filterExpression = string.Format("PreHDAssessmentID = {0} AND IsDeleted = 0", hdnAssessmentID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
                lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format(" MRN = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.MRN));
                grdViewDt3.DataSource = lstEntity;
                grdViewDt3.DataBind(); 
            }
        }

        private void BindGridViewDt4(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            if (!string.IsNullOrEmpty(hdnAssessmentID.Value) && hdnAssessmentID.Value != "0")
            {
                string filterExpression = string.Format("PreHDAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvHSUFluidBalanceRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }

                List<vHSUFluidBalance> lstEntity = BusinessLayer.GetvHSUFluidBalanceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "LogTime DESC");

                grdViewDt4.DataSource = lstEntity;
                grdViewDt4.DataBind(); 
            }
        }

        protected void cbpHistoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref gridView1PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref gridView1PageCount);
                    result = "refresh|" + gridView1PageCount;
                }
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
                    BindGridViewFormList(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewFormList(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewFormDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewFormListDt(Convert.ToInt32(param[1]), false, ref gridViewFormListPageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewFormListDt(1, true, ref gridViewFormListPageCount);
                    result = "refresh|" + gridViewFormListPageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt3_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt3(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt3(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewDt4_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt4(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt4(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdViewDt3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }
    }
}