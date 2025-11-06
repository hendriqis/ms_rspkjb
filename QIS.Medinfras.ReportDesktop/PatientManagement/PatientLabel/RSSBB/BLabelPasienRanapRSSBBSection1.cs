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
    public partial class BLabelPasienRanapRSSBBSection1 : BaseRpt
    {
        public BLabelPasienRanapRSSBBSection1()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vLabelInpatientRegistration> lst)
        {
            this.DataSource = lst;
        }
    }
}
