using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanBarangPerPemasok : BaseDailyLandscapeRpt
    {
        private int _lineNumber = 0;
        private Decimal totItemPrice = 0;

        public LPenerimaanBarangPerPemasok()
        {
            InitializeComponent();            
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            _lineNumber = 0;
            base.InitializeReport(param);
        }

        void lblSubTotal_SummaryCalculated(object sender, TextFormatEventArgs e)
        {
            if (e.Value != null)
                totItemPrice = Convert.ToDecimal(e.Value);
        }

        private void LPenerimaanBarangPerPemasok_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
        }

        private void lblRecordNumber_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblLineNumber.Text = (++_lineNumber).ToString();
        }

        private void lblSubTotalLineAmount_AfterPrint(object sender, EventArgs e)
        {
            _lineNumber = 0;
        }        
    }
}
