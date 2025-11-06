using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridInformationPatientVisitCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        const string openedVisitCode = "X020^001";
        const string cancelledVisitCode = "X020^006";
        const string closedVisitCode = "X020^007";
        const string transferredVisitCode = "X020^008";
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePageRegisteredPatient)Page).LoadAllWords();
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
            string filterExpression = ((BasePageRegisteredPatient)Page).GetFilterExpression();
            int rowCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression);
            if (isCountPageCount)
            {
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }
            string openedFilterExp = filterExpression + " AND GCVisitStatus = '" + openedVisitCode + "'";
            int openedCount = BusinessLayer.GetvConsultVisitRowCount(openedFilterExp);
            openedVisitCounter.InnerText = "Opened " + openedCount.ToString();
            int closedCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + closedVisitCode + "'");
            closedVisitCounter.InnerText = "Closed " + closedCount.ToString();
            int transferredCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + transferredVisitCode + "'");
            transferredVisitCounter.InnerText = "Transferred " + transferredCount.ToString();
            int cancelledCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + cancelledVisitCode + "'");
            cancelledVisitCounter.InnerText = "Cancelled " + cancelledCount.ToString();
            int unknownCount = rowCount - (openedCount + closedCount + transferredCount + cancelledCount);
            unknownVisitCounter.InnerText = "Unknown "+unknownCount.ToString();
            List<vConsultVisit> lstEntity = BusinessLayer.GetvConsultVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex);
            lstEntity.ForEach(e =>
            {
                if (e.GCVisitStatus != null)
                {
                    switch (e.GCVisitStatus)
                    {
                        case openedVisitCode: e.GCVisitStatus = "background-color:#FFFF69;"; break;
                        case closedVisitCode: e.GCVisitStatus = "background-color:#55FF55;"; break;
                        case transferredVisitCode: e.GCVisitStatus = "background-color:#AAAAAA;"; break;
                        case cancelledVisitCode: e.GCVisitStatus = "background-color:#FF4545;"; break;
                        default: e.GCVisitStatus = "background-color:#EEEEEE;"; break;
                    }
                }
                else
                {
                    e.GCVisitStatus = "background-color:#EEEEEE;";
                }
            });
            
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected string GetLabel(string code)
        {
            return ((BasePageRegisteredPatient)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            
        }
    }
}