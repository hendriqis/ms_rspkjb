using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRProceduresRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRProceduresRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int visitID)
        {
            List<vPatientProcedure> lstProcedures = BusinessLayer.GetvPatientProcedureList(string.Format("VisitID = {0} AND IsDeleted = 0", visitID));
            this.DataSource = lstProcedures;
        }

    }
}
