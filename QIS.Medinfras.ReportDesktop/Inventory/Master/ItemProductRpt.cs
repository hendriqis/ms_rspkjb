using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class ItemProductRpt : BaseDailyPortraitRpt
    {
        public ItemProductRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string hid = appSession.HealthcareID.ToString();
            vHealthcare entity = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = {0}", hid))[0];

            xrLabel1.Text = "Nama RS : " + entity.HealthcareName;

            base.InitializeReport(param);
        }

    }
}
