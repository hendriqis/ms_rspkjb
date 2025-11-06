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
    public partial class LPendaftaranPasienPerCaraDaftar : BaseCustomDailyLandscapeA3Rpt
    {
        public LPendaftaranPasienPerCaraDaftar()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string registrationMethod = param[1].ToString();
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            if (!String.IsNullOrEmpty(registrationMethod))
            {
                StandardCode sc = BusinessLayer.GetStandardCode(registrationMethod);
                if (sc != null)
                {
                    lblinfo.Text = string.Format("Cara Daftar : {0}", sc.StandardCodeName);
                }
            }
            else
            {
                lblinfo.Text = "Cara Daftar : Semua";
            }

            base.InitializeReport(param);
        }
        private void xrTable3_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCRegistrationStatus") != null))
            {
                String GCRegistrationStatus = GetCurrentColumnValue("GCRegistrationStatus").ToString();
                if (GCRegistrationStatus == Constant.VisitStatus.CANCELLED)
                {
                    xrTable3.ForeColor = Color.Red;
                }
                else
                {
                    xrTable3.ForeColor = Color.Black;
                }
            }
        }

    }
}
