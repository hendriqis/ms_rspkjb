using System;
using System.Drawing;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BSuratKeteranganFeedback : BaseCustomDailyPotrait2Rpt
    {
        private string city = "";
        public BSuratKeteranganFeedback()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            city = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault().City;

            base.InitializeReport(param);
        }

        private void lblTanggal_BeforePrint_1(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            DateTime dateNow = DateTime.Now;
            String date = dateNow.ToString(Constant.FormatString.DATE_FORMAT);
            lblTanggal.Text = string.Format("{0}, {1}", city, date);
        }
    }
}
