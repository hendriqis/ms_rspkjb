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
    public partial class PasienSensusMeninggalSetelah48 : DevExpress.XtraReports.UI.XtraReport
    {
        public PasienSensusMeninggalSetelah48()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vPatientATDLog> lstPatientDeadBefore48)
        {


            this.DataSource = lstPatientDeadBefore48;
        }
    }
}
