using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianPerKasirDetailDP : BaseCustomDailyLandscapeA3Rpt
    {
        private int _lineNumber = 0;

        public LPenerimaanHarianPerKasirDetailDP()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            base.InitializeReport(param);
        }

        private void cNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cNo.Text = (++_lineNumber).ToString();
        }

        private void cSubTotalShiftCaption_AfterPrint(object sender, EventArgs e)
        {
            _lineNumber = 0;
        }

    }
}
