using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridInformationPatientVisitCtlNew : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        const string openedVisitCode = Constant.VisitStatus.OPEN;
        const string checkedInVisitCode = Constant.VisitStatus.CHECKED_IN;
        const string receivingTreatmentVisitCode = Constant.VisitStatus.RECEIVING_TREATMENT;
        const string physicianDischargeVisitCode = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
        const string dischargedVisitCode = Constant.VisitStatus.DISCHARGED;
        const string cancelledVisitCode = Constant.VisitStatus.CANCELLED;
        const string closedVisitCode = Constant.VisitStatus.CLOSED;
        const string transferredVisitCode = Constant.VisitStatus.TRANSFERRED;

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
            int openedCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + openedVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            openVisitCounter.InnerText = "Opened " + openedCount.ToString();

            int checkInCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + checkedInVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            checkInVisitCounter.InnerText = "Checked-In " + checkInCount.ToString();

            int receivingTreatmentCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + receivingTreatmentVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            receivingTreatmentVisitCounter.InnerText = "Receiving Treatment " + receivingTreatmentCount.ToString();

            int physicanDischargeCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + physicianDischargeVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            physicanDischargeVisitCounter.InnerText = "Physician Discharge " + physicanDischargeCount.ToString();

            int dischargedCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + dischargedVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            dischargedVisitCounter.InnerText = "Discharged " + dischargedCount.ToString();

            int cancelledCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + cancelledVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            cancelledVisitCounter.InnerText = "Cancelled " + cancelledCount.ToString();

            int closedCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + closedVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            closedVisitCounter.InnerText = "Closed " + closedCount.ToString();

            int transferredCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND GCVisitStatus = '" + transferredVisitCode + "'" + " AND LinkedToRegistrationID IS NULL");
            if (transferredCount != 0)
            {
                transferredVisitCounter.InnerText = "Transferred " + transferredCount.ToString();
            }
            else
            {
                transferredPatientColor.Attributes.Add("style", "display:none");
                transferredVisitCounter.Attributes.Add("style", "display:none");
            }

            int transferredInpatientCount = BusinessLayer.GetvConsultVisitRowCount(filterExpression + " AND LinkedToRegistrationID IS NOT NULL");
            transferredInpatientCounter.InnerText = "Transferred Inpatient " + transferredInpatientCount.ToString();

//            filterExpression += String.Format("Order By RegistrationID ASC");            
            List<vConsultVisitInformation> lstEntity = BusinessLayer.GetvConsultVisitInformationList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "RegistrationID ASC");
            lstEntity.ForEach(e =>
            {
                if (e.GCVisitStatusInformation != null)
                {
                    switch (e.GCVisitStatusInformation)
                    {
                        case openedVisitCode: e.GCVisitStatus = "background-color:#EEEEEE;"; break;
                        case checkedInVisitCode : e.GCVisitStatus = "background-color:#ffffcc;"; break;
                        case receivingTreatmentVisitCode : e.GCVisitStatus = "background-color:#ff66cc;"; break;
                        case physicianDischargeVisitCode : e.GCVisitStatus = "background-color:#66a4ff;"; break;
                        case dischargedVisitCode: e.GCVisitStatus = "background-color:#FFFF69;"; break;
                        case cancelledVisitCode: e.GCVisitStatus = "background-color:#FF4545;"; break;
                        case closedVisitCode: e.GCVisitStatus = "background-color:#55FF55;"; break;
                        case transferredVisitCode: e.GCVisitStatus = "background-color:#AAAAAA;"; break;
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