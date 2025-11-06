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
    public partial class MRBodyDiagram : DevExpress.XtraReports.UI.XtraReport
    {
        public MRBodyDiagram()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID);
            List<vPatientBodyDiagramHd> lstHd = BusinessLayer.GetvPatientBodyDiagramHdList(filterExpression);
            List<vPatientBodyDiagramDt> lstDt = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);

            var lst = (from p in lstHd
                       select new
                       {
                           ObservationDateInString = p.ObservationDateInString,
                           ObservationTime = p.ObservationTime,
                           ParamedicName = p.ParamedicName,
                           BodyDiagramDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                       }).ToList();

            this.DataSource = lst;
            DetailReport.DataMember = "BodyDiagramDts";
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
