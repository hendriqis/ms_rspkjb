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
    public partial class LPenerimaanPerRegistrasi : BaseCustomDailyLandscapeRpt
    {
        public LPenerimaanPerRegistrasi()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            string[] temp1 = param[1].Split(';');
            xrLabel4.Text = string.Format("Jam : {0} s/d {1}", temp1[0].ToString(), temp1[1].ToString());

            if (!String.IsNullOrEmpty(param[3]))
            {
                UserAttribute ua = BusinessLayer.GetUserAttribute(Convert.ToInt32(param[3]));
                if (ua != null)
                {
                    xrLabel3.Text = string.Format("Kasir : {0}", ua.FullName);
                }
            }
            else
            {
                xrLabel3.Text = string.Format("Kasir : Semua");
            }

            base.InitializeReport(param);
        }

    }
}
