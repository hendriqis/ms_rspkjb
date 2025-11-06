using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LTempatTidurTerisiPerKelas : BaseCustomDailyPotraitRpt
    {
        public LTempatTidurTerisiPerKelas()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            lblTanggalCetak.Text = string.Format("Tanggal Cetak : {0} | {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
            base.InitializeReport(param);
        }
    }
}
