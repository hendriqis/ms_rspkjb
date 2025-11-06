using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraReports.Parameters;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRReferralNewRSSA : DevExpress.XtraReports.UI.XtraReport
    {
        public MRReferralNewRSSA()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID);
            vPatientReferral entityPR = BusinessLayer.GetvPatientReferralList(filterExpression).FirstOrDefault();
            if (entityPR != null)
            {
                xrLabel7.Text = string.Format("Dikonsulkan Kepada");
                xrLabel8.Text = string.Format(":");
                List<vPatientReferral> lstPD = BusinessLayer.GetvPatientReferralList(filterExpression);
                List<vPatientReferral> lstPDD = BusinessLayer.GetvPatientReferralList(filterExpression);
                var lst = (from p in lstPD
                           select new
                           {
                               FullName = p.ToPhysicianName,
                               ParamedicDts = lstPDD.Where(pdd => pdd.ID == p.ID).ToList()
                           }).ToList();
                this.DataSource = lst;
                DetailReport.DataMember = "ParamedicDts";
            }
            else
            {
                xrTable1.Visible = false;
                //xrLabel1.Visible = false;
                //xrLabel2.Visible = false;
                //xrLabel3.Visible = false;
                //xrLabel4.Visible = false;
                //xrLabel6.Visible = false;
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
