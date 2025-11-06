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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientObsygnHistoryList : BasePage
    {
        protected int PageCount = 1;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindGridView(1, true, ref PageCount);
                BindGridViewDetail(1, true, ref PageCount);
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvObstetricHistoryRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vObstetricHistory> lstEntity = BusinessLayer.GetvObstetricHistoryList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
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

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDetail(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDetail(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewDetail(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<GetAntenatalHistoryList> lstEntity = BusinessLayer.GetAntenatalHistoryList(AppSession.RegisteredPatient.MRN);
            grdViewDetail.DataSource = lstEntity;
            grdViewDetail.DataBind();
        }

        protected void grdViewDetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                GetAntenatalHistoryList obj = (GetAntenatalHistoryList)e.Row.DataItem;
                Repeater rptTestOrderDt = (Repeater)e.Row.FindControl("rptTestOrderDt");
                Repeater rptPrescription = (Repeater)e.Row.FindControl("rptPrescription");
                Repeater rptDiagnose = (Repeater)e.Row.FindControl("rptDiagnose");

                List<GetListItemForPatientObsygnHistoryList> lstData = BusinessLayer.GetListItemForPatientObsygnHistoryList(obj.VisitID);
                List<GetListItemForPatientObsygnHistoryList> lstTestOrder = lstData.Where(t => t.Jenis == "TestOrder").ToList();
                List<GetListItemForPatientObsygnHistoryList> lstPrescription = lstData.Where(t => t.Jenis == "Prescription").ToList();

                string filterDiagnose = string.Format("VisitID = {0} AND IsDeleted = 0", obj.VisitID);
                List<vPatientDiagnosis> lstDiagnose = BusinessLayer.GetvPatientDiagnosisList(filterDiagnose);

                rptTestOrderDt.DataSource = lstTestOrder;
                rptTestOrderDt.DataBind();

                rptPrescription.DataSource = lstPrescription;
                rptPrescription.DataBind();

                rptDiagnose.DataSource = lstDiagnose;
                rptDiagnose.DataBind();
            }
        }
    }
}