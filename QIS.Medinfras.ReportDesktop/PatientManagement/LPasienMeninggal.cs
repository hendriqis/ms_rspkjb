using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPasienMeninggal : BaseCustomDailyLandscapeRpt
    {
        public LPasienMeninggal()
        {
            InitializeComponent();
        }

        private void xrTableCell16_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String Summary = (String) xrTableCell16.Summary.GetResult();
            
            if (String.IsNullOrEmpty(Summary)) 
            { 
                xrTableCell16.Text = "-";
            }
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblperiod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }
    }
}