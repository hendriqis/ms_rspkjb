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
    public partial class MRBodyDiagramRSSK : DevExpress.XtraReports.UI.XtraReport
    {
        public MRBodyDiagramRSSK()
        {
            InitializeComponent();
        }

        class CPatientBodyDiagram
        {
            public int ID { get; set; }
            public string DiagramName { get; set; }
            public string FileImageUrl { get; set; }
            public string ParamedicName { get; set; }
            public string DisplayObservationDateTime { get; set; }
            public List<vPatientBodyDiagramDt> PatientBodyDiagramDts { get; set; }
        }

        List<vPatientBodyDiagramDt> lstDt = null;
        public bool InitializeReport(int VisitID)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", VisitID);
            List<vPatientBodyDiagramHd> lstHd = BusinessLayer.GetvPatientBodyDiagramHdList(filterExpression);
            lstDt = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);

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
            if (GetCurrentColumnValue("FileImageUrl") != null)
            {
                // Obtain the current label.
                XRPictureBox pictureBox = (XRPictureBox)sender;

                // Get the total value.
                string imgUrl = GetCurrentColumnValue("FileImageUrl").ToString();                
                pictureBox.ImageUrl = imgUrl;
            }
        }

        private void sbrBodyDiagramDt_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if (GetCurrentColumnValue("ID") != null)
            {
                // Obtain the current label.
                XRSubreport subreport = (XRSubreport)sender;

                // Get the total value.
                int ID = Convert.ToInt32(GetCurrentColumnValue("ID"));
                ((MRBodyDiagramDtRSSK)subreport.ReportSource).InitializeReport(lstDt.Where(p => p.ID == ID).ToList());
                //subreport.In
            }
        }

    }
}
