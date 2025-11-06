using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using System.Linq;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class DetailSummaryVitalSignRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public DetailSummaryVitalSignRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID,int isShowInDetail)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            string filterExpression2 = string.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID IN (88,3,5,1)", VisitID);
            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression);
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(filterExpression2);
            List<vVitalSignHd> lstValid;

            if (isShowInDetail == 1)
            {
                lstValid = lstVitalSignHd.Where(t => t != lstVitalSignHd.LastOrDefault()).ToList();
            }
            else if (isShowInDetail == 0)
            {
                lstValid = lstVitalSignHd.Where(t => t == lstVitalSignHd.LastOrDefault()).ToList();
            }
            else {
                lstValid = lstVitalSignHd;
            }

            var lst = (from p in lstValid
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ParamedicName = p.ParamedicName,
                           Remarks = p.Remarks,
                           VitalSignDts = lstVitalSignDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "VitalSignDts";
        }

        private void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (DetailReport.RowCount == 0)
            {
                Detail.Visible = false;
            }
        }

    }
}
