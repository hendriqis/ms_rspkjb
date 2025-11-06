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
    public partial class BPemesananMakanan : BaseDailyPortraitRpt
    {
        public BPemesananMakanan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPrintDate.Text = DateTime.Now.ToString();
            base.InitializeReport(param);
        }
    }
}
