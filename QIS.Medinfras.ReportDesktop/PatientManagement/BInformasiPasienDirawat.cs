using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BInformasiPasienDirawat : BaseDailyLandscapeRpt
    {
        public BInformasiPasienDirawat()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            //lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            //string[] temp1 = param[1].Split(';');
            //xrLabel4.Text = string.Format("Jam : {0} s/d {1}", temp1[0].ToString(), temp1[1].ToString());
            base.InitializeReport(param);
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCRegistrationStatus") != null))
            {
                String GCRegistrationStatus = GetCurrentColumnValue("GCRegistrationStatus").ToString();
                if (GCRegistrationStatus == Constant.VisitStatus.CANCELLED)
                {
                    xrTable2.ForeColor = Color.Red;
                }
                else
                {
                    xrTable2.ForeColor = Color.Black;
                }
            }
        }

    }
}
