using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRDTReviewOfSystemRSDI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDTReviewOfSystemRSDI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).LastOrDefault();
            vReviewOfSystemRSSES entityDt = BusinessLayer.GetvReviewOfSystemRSSESList(filterExpression).LastOrDefault();

            if (entityHd != null)
            {

                List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                List<vReviewOfSystemRSSES> lstDt = BusinessLayer.GetvReviewOfSystemRSSESList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));

                var lst = (from p in lstHd
                           select new
                           {
                               ObservationDateInString = p.ObservationDateInString,
                               ObservationTime = p.ObservationTime,
                               ParamedicName = p.ParamedicName,
                               ReviewOfSystemDts = lstDt.Where(dt => dt.ID == p.ID).ToList()
                           }).ToList();

                this.DataSource = lst;
                DetailReport.DataMember = "ReviewOfSystemDts";
            }
            else
            {
                xrLabel1.Visible = false;
                xrLabel6.Visible = false;
                xrLabel7.Visible = false;
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
