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
    public partial class LNeracaBentukTDetailAktivaRSMD : DevExpress.XtraReports.UI.XtraReport
    {
        List<GetGLBalancePerPeriodPerLevel> lstTemp = new List<GetGLBalancePerPeriodPerLevel>();

        public LNeracaBentukTDetailAktivaRSMD()
        {
            InitializeComponent();
        }

        public void InitializeReport(List<GetGLBalancePerPeriodPerLevel> lst)
        {
            this.DataSource = lst;
            lstTemp = lst;
        }

        private void xrTableCell2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String GCGLAccountType = GetCurrentColumnValue("GCGLAccountType").ToString();
            xrTableCell2.Text = lstTemp.Where(t => t.GCGLAccountType == GCGLAccountType).FirstOrDefault().BalanceEND.ToString("N2");
        }
    }
}
