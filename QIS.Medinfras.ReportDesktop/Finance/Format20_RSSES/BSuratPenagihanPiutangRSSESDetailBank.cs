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
    public partial class BSuratPenagihanPiutangRSSESDetailBank : DevExpress.XtraReports.UI.XtraReport
    {
        public BSuratPenagihanPiutangRSSESDetailBank()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<vBusinessPartnerVirtualAccount> lst)
        {
            this.DataSource = lst;
        }
    }
}
