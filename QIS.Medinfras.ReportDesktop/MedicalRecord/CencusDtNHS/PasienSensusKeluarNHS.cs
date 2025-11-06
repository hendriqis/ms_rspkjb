using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class PasienSensusKeluarNHS : DevExpress.XtraReports.UI.XtraReport
    {
        public PasienSensusKeluarNHS()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<NursingPatientNameDischarged> lstPatientNameDischarged)
        {


            this.DataSource = lstPatientNameDischarged;
        }
    }
}
