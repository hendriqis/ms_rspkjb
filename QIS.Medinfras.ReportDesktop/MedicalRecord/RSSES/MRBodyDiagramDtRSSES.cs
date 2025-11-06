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
    public partial class MRBodyDiagramDtRSSES : DevExpress.XtraReports.UI.XtraReport
    {
        public MRBodyDiagramDtRSSES()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientBodyDiagramDt> lst)
        {
            this.DataSource = lst;
        }

        private void xrPictureBox1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("SymbolImageUrl") != null)
            {
                // Obtain the current label.
                XRPictureBox pictureBox = (XRPictureBox)sender;

                // Get the total value.
                string imgUrl = GetCurrentColumnValue("SymbolImageUrl").ToString();
                pictureBox.ImageUrl = imgUrl;
            }
        }

    }
}
