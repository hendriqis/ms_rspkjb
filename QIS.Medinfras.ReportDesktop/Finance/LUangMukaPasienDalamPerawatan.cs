using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Globalization;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.XtraReports.UI;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LUangMukaPasienDalamPerawatan : BaseDailyLandscapeRpt
    {
        public LUangMukaPasienDalamPerawatan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            string[] temp = param[0].Split(';');

            lblPeriod.Text = string.Format("Periode Pembayaran : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            //lblPeriod.Text = string.Format("Tanggal : {0} ", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            
            base.InitializeReport(param);
        }
       
    }
}
