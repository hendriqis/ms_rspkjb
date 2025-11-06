using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BInvoiceMCUOriginalPackageDetailRpt : DevExpress.XtraReports.UI.XtraReport
    {
        public BInvoiceMCUOriginalPackageDetailRpt()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetMCUDetailOriginalPackageTariff> lst)
        {
            this.DataSource = lst;
        }
    }
}
