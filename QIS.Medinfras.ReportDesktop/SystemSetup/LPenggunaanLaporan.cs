using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenggunaanLaporan : BaseCustomDailyLandscapeRpt
    {
        public LPenggunaanLaporan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (!String.IsNullOrEmpty(param[0])) 
            {
                txtModule.Text = string.Format("Modul : Semua");
            }
            else
            {
                Module m = BusinessLayer.GetModule(param[1]);
                txtModule.Text = string.Format("Modul : {0}", m.ModuleName);
            }

            base.InitializeReport(param);
        }
    }
}
