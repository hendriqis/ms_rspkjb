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
    public partial class LPenerimaanPerHariRekap : BaseCustomDailyPotraitRpt
    {
        public LPenerimaanPerHariRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string[] temp2 = param[1].Split(';');
            lblPeriode.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            xrLabel3.Text = string.Format("Jam : {0} s/d {1}", (temp2[0]).ToString(), (temp2[1]).ToString());

            if (param[3].ToString() == "1")
            {
                xrLabel4.Text = "Status Transaksi : NON VOID";
            }
            else
            {
                xrLabel4.Text = "Status Transaksi : VOID";
            }

            base.InitializeReport(param);
        }
    }
}
