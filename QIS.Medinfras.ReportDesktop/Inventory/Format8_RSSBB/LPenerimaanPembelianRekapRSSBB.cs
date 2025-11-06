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
    public partial class LPenerimaanPembelianRekapRSSBB : BaseCustomDailyPotraitRpt
    {
        public LPenerimaanPembelianRekapRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            lblReceivedTime.Text = string.Format("Jam : {0} - {1}", param[1].Substring(0, 5), param[1].Substring(6));
            base.InitializeReport(param);

            lblPrintDate.Text = string.Format("{0} / {1}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
            lblPrintUser.Text = AppSession.UserLogin.UserFullName;
        }
    }
}
