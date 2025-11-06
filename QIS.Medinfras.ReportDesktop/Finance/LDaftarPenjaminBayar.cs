using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LDaftarPenjaminBayar :BaseCustomDailyLandscapeRpt
    {
        public LDaftarPenjaminBayar()
        {
            InitializeComponent();
        }

        private void xrTable2_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("IsActive") != null))
            {
                String IsActive = GetCurrentColumnValue("IsActive").ToString();
                if (IsActive == "False")
                    xrTable2.ForeColor = Color.Red;
                else
                    xrTable2.ForeColor = Color.Black;
            }
        }
    }
}
