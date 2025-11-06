using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BDaftarPasienRI : BaseCustomDailyPotraitRpt
    {
        public BDaftarPasienRI()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblDate.Text = string.Format("{0}", DateTime.Now.ToString("dd-MMM-yyyy"));
            base.InitializeReport(param);
        }

    }

}
