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
    public partial class PasienSensusKeluarMeninggal_RSCK : DevExpress.XtraReports.UI.XtraReport
    {
        public PasienSensusKeluarMeninggal_RSCK()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientATDLogRSCK> lstPatientDISCHARGEDDIED)
        {

            this.DataSource = lstPatientDISCHARGEDDIED;
        }
    }
}
