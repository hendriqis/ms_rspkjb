using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRDiagnose : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDiagnose()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            List<vPatientDiagnosis1> lstDiagnose = BusinessLayer.GetvPatientDiagnosis1List(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));
            this.DataSource = lstDiagnose;
        }

    }
}
