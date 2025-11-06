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
    public partial class RL1_1_DataDasarRumahSakit : BaseCustomDailyPotraitRpt
    {
        public RL1_1_DataDasarRumahSakit()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            DateTime date = DateTime.Now;
            lblYear.Text = date.ToString("yyyy");

            base.InitializeReport(param);
        }
    }
}
