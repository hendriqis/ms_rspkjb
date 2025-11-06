using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanPerKelompokPelayanan : BaseCustomDailyPotraitRpt
    {
        //String val = "";
        public LPenerimaanPerKelompokPelayanan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void xrTableCell40_SummaryGetResult(object sender, SummaryGetResultEventArgs e)
        {
            //Decimal sumOfValues = 0;
            //string formatString = String.Empty;
            //foreach (var value in e.CalculatedValues)
            //{
            //    sumOfValues = sumOfValues + Convert.ToDecimal(value);   //calculate the sum of values
            //}

            //Decimal Total = sumOfValues;
            //Decimal rounding = Convert.ToDecimal(xrTableCell32.Text.ToString());
            //val = (Total + rounding).ToString();
        }

    }
}
