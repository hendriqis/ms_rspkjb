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
    public partial class LSaldoUangMuka : BaseDailyLandscapeRpt
    {
        public LSaldoUangMuka()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {


            lblPeriod.Text = string.Format("Tanggal : {0} ", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            
            base.InitializeReport(param);
        }
       
    }
}
