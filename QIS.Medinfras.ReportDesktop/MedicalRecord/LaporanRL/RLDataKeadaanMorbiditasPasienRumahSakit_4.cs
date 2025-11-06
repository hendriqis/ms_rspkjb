using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class RLDataKeadaanMorbiditasPasienRumahSakit_4 : BaseCustomDailyLandscapeA3Rpt
    {
        public RLDataKeadaanMorbiditasPasienRumahSakit_4()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string Department = param[2].ToString();
            if(Department != "")
            {
                lblDepartment.Text = string.Format("{0}", param[2].ToString());
            }else
            {
                lblDepartment.Text = (" ");
            }
            base.InitializeReport(param);
        }

    }
}
