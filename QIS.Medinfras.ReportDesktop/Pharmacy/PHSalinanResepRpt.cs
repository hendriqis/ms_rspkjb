using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class PHSalinanResepRpt : BaseCustomPaperRpt
    {
        public PHSalinanResepRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);
            vPrescriptionOrderHd entity = BusinessLayer.GetvPrescriptionOrderHdList(param[0])[0];
            lblReportSubtitle.Text = entity.ChargesTransactionNo;

            vConsultVisit entityConsultVisit = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}",entity.VisitID))[0];
            lblPatientName.Text = entityConsultVisit.PatientName;
            lblPatientAge.Text = entityConsultVisit.PatientAge;
        }

        protected void Detail_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            Boolean IsCompound = Convert.ToBoolean(GetCurrentColumnValue("IsCompound"));
            if (!IsCompound) e.Cancel = true;
        }
    }
}
