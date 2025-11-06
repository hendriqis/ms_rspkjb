using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPersediaanBarangSemuaGudangPerKelompokItem : BaseDailyPortraitRpt
    {
        private int _lineNumber = 0;
        public LPersediaanBarangSemuaGudangPerKelompokItem()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            base.InitializeReport(param);
        }

        private void GroupHeader3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblNo.Text = (++_lineNumber).ToString();
        }
    }
}
