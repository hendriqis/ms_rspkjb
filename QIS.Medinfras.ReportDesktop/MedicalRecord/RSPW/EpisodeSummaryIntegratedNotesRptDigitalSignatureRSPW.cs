using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class EpisodeSummaryIntegratedNotesRptDigitalSignatureRSPW : DevExpress.XtraReports.UI.XtraReport
    {
        public EpisodeSummaryIntegratedNotesRptDigitalSignatureRSPW()
        {
            InitializeComponent();
        }

        public bool InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vPatientVisitNote> lstHd = BusinessLayer.GetvPatientVisitNoteList(filterExpression);

            if (lstHd.Count > 0)
            {
                this.DataSource = lstHd;

                return true;
            }
            else
            {
                this.Visible = false;
                return false;
            }
        }

        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            var img = (Image)GetCurrentColumnValue("cfSignature1");
            if (img != null)
            {
                xrPictureBox1.Image = img;
            }           
        }

    }
}
