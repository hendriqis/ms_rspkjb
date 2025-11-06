using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LRekapDaftarHonorDokterPasien : BaseCustom2DailyPotraitRpt
    {
        private int _lineNumber = 0;

        public LRekapDaftarHonorDokterPasien()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            _lineNumber = 0;

            string[] temp = param[1].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            
            base.InitializeReport(param);
        }

        private void cNumberOfRow_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            cNumberOfRow.Text = (++_lineNumber).ToString();
        }
        
    }
}
