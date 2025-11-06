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
    public partial class BLabelPasienRSSMPSection5 : BaseRpt
    {
        public BLabelPasienRSSMPSection5()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vLabelPatientRegistration> lst)
        {
            this.DataSource = lst;
        }
    }
}
