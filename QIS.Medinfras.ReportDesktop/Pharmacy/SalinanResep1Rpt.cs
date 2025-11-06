using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class SalinanResep1Rpt : BaseReceipt2Rpt
    {
        public SalinanResep1Rpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(param[0])[0];
            lblReportSubTitle.Text = string.Format("No. : {0}", entity.ChargesTransactionNo);

            vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entity.VisitID))[0];
            lblPatientName.Text = entityConsultVisit.PatientName;
            lblPatientAge.Text = entityConsultVisit.PatientAge;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, {1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), "admin");
        }

        protected void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }
    }
}
