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
    public partial class ROSAntarRuangDt : DevExpress.XtraReports.UI.XtraReport
    {
        public ROSAntarRuangDt()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0 ORDER BY ID DESC", VisitID);
            vReviewOfSystemHd entityHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
            vReviewOfSystemDt entityDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format(filterExpression)).FirstOrDefault();

            if (entityHd != null)
            {
                List<vReviewOfSystemHd> lstHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
                List<vReviewOfSystemDt> lstDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", entityHd.ID));
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
                xrLabel3.Visible = false;
                xrLabel6.Visible = false;
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
