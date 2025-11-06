using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LVerifikasiPembayaranHutang : BaseCustomDailyPotraitRpt 
    {
        public LVerifikasiPembayaranHutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriode.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }
    }
}
