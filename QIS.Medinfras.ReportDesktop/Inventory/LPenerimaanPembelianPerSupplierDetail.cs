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
    public partial class LPenerimaanPembelianPerSupplierDetail : BaseCustomDailyLandscapeRpt
    {
        private decimal cSTSupplierAmount = 0;
        private decimal cSTLocationAmount = 0;
        private decimal cGTAmount = 0;

        public LPenerimaanPembelianPerSupplierDetail()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            cSTSupplierAmount = 0;
            cSTLocationAmount = 0;
            cGTAmount = 0;

            base.InitializeReport(param);
        }

        private void cGH3LineAmount_AfterPrint(object sender, EventArgs e)
        {
            if (cGH3LineAmount.Text != "")
            {
                cSTSupplierAmount += Convert.ToDecimal(cGH3LineAmount.Text);
            }
            else
            {
                cSTSupplierAmount += 0;
            }
        }

        private void cSTSupplier_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cSTSupplier.Text = cSTSupplierAmount.ToString(Constant.FormatString.NUMERIC_2);
        }

        private void cSTSupplier_AfterPrint(object sender, EventArgs e)
        {
            cSTLocationAmount += cSTSupplierAmount;
            cSTSupplierAmount = 0;
        }

        private void cSTLocation_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cSTLocation.Text = cSTLocationAmount.ToString(Constant.FormatString.NUMERIC_2);
        }

        private void cSTLocation_AfterPrint(object sender, EventArgs e)
        {
            cGTAmount += cSTLocationAmount;
            cSTLocationAmount = 0;
        }

        private void cGT_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cGT.Text = cGTAmount.ToString(Constant.FormatString.NUMERIC_2);
        }

        private void cGT_AfterPrint(object sender, EventArgs e)
        {
        }

    }
}
