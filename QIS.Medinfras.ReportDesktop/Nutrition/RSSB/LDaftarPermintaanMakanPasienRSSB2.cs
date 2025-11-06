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
    public partial class LDaftarPermintaanMakanPasienRSSB2 : BaseCustomDailyPotraitRpt
    {
        public LDaftarPermintaanMakanPasienRSSB2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            base.InitializeReport(param);

            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID))[0];

            lblPrintDate.Text = string.Format("{0},", entityHealthcare.City);
        }
    }
}
