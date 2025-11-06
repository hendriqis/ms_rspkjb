using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LKeadaanMorbiditasPasienRawatJalanRSMD : BaseDailyLandscapeRpt
    {
        public LKeadaanMorbiditasPasienRawatJalanRSMD()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            _lineNumber = 0;
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private int _lineNumber = 0;

        //private void xrTableCell129_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    ((XRLabel)sender).Text = recordNo.ToString();
        //}

        

        private void xrTableCell129_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            xrTableCell129.Text = (++_lineNumber).ToString();
        }

        private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            _lineNumber = 0;
        }

        //private void GroupHeader1_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    recordNo++;
        //}

    }
}