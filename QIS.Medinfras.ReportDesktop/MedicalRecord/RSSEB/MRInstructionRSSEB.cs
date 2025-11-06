using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRInstructionRSSEB : DevExpress.XtraReports.UI.XtraReport
    {
        public MRInstructionRSSEB()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID)
        {
            vPatientInstruction entityPI = BusinessLayer.GetvPatientInstructionList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();
            lblInstruction.Text = string.Format("{0}", entityPI.Description);
        }
    }
}
