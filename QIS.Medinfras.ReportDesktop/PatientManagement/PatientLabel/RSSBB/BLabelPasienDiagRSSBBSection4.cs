using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLabelPasienDiagRSSBBSection4 : BaseRpt
    {
        public BLabelPasienDiagRSSBBSection4()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vLabelPatientRegistrationMedicalDiagnostic> lst)
        {
            this.DataSource = lst;
        }
    }
}
