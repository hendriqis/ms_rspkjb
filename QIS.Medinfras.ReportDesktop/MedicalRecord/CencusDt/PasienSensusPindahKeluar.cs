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
    public partial class PasienSensusPindahKeluar : DevExpress.XtraReports.UI.XtraReport
    {
        public PasienSensusPindahKeluar()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientATDLog> lstPatientMoveOUT)
        {
            this.DataSource = lstPatientMoveOUT;
        }

    }
}
