using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Globalization;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LTransaksiPemeriksaanRadiologiPerItemPerDepartment : BaseCustomDailyPotraitRpt
    {
        public LTransaksiPemeriksaanRadiologiPerItemPerDepartment()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Periode {0} {1}", CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(param[1])), param[0]);
            base.InitializeReport(param);
        }
    }
}
