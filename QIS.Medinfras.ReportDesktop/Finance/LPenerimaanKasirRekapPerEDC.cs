using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanKasirRekapPerEDC: BaseCustomDailyPotraitRpt
    {
        public LPenerimaanKasirRekapPerEDC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp1 = param[0].Split(';');
            string[] temp = param[1].Split(';');

            //lblParamTanggal.Text = "Tanggal Bayar : " + Convert.ToDateTime(param[0]).ToString(Constant.FormatString.DATE_FORMAT);
            lblParamTanggal.Text = string.Format("Tanggal Bayar : {0}", Helper.YYYYMMDDToDate(temp1[0]).ToString(Constant.FormatString.DATE_FORMAT));
            lblParamJam.Text = "Jam Bayar : " + temp[0] + " - " + temp[1];

            base.InitializeReport(param);
        }

    }
}
