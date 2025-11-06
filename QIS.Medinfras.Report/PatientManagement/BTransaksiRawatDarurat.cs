using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Report
{
    public partial class BTransaksiRawatDarurat : BaseDailyPortraitRpt
    {
        public BTransaksiRawatDarurat()
        {
            InitializeComponent();
        }

        private void xrLabel26_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}",AppSession.UserLogin.HealthcareID))[0];
            ((XRLabel)sender).Text = entity.City+", "+DateTime.Now.ToString("dd-MMM-yyyy");
        }

    }
}
