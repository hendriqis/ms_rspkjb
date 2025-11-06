using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Collections.Generic;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRTriageRSRTH : DevExpress.XtraReports.UI.XtraReport
    {
        public MRTriageRSRTH()
        {
            InitializeComponent();
        }

        public void InitializeReport(int RegistrationID)
        {
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("LinkedToRegistrationID IN ({0})", RegistrationID)).FirstOrDefault();

            lblTriage.Text = entity.Triage;
        }
    }
}
