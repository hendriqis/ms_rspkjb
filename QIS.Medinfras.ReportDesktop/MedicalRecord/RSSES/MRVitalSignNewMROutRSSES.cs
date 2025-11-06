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
    public partial class MRVitalSignNewMROutRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRVitalSignNewMROutRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0 ORDER BY ID DESC", VisitID, MedicalResumeID);
            vVitalSignHd entityHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
            vVitalSignRSSES entityDt = BusinessLayer.GetvVitalSignRSSESList(string.Format(filterExpression)).FirstOrDefault();

            if (entityHd != null)
            {
                List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                List<vVitalSignRSSES> lstDt = BusinessLayer.GetvVitalSignRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                var lst = (from p in lstHd
                           select new
                           {
                               ObservationDateInString = p.ObservationDateInString,
                               ObservationTime = p.cfObservationTimeRM,
                               Remarks = p.Remarks,
                               VitalSignDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                           }).ToList();

                this.DataSource = lst;
                DetailReport.DataMember = "VitalSignDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel2.Visible = false;
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                xrLabel5.Visible = false;
                xrLabel8.Visible = false;
            }
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