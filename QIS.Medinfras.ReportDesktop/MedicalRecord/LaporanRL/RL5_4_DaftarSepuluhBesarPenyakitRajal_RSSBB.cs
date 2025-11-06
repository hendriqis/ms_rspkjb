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
    public partial class RL5_4_DaftarSepuluhBesarPenyakitRajal_RSSBB : BaseCustomDailyPotraitRpt
    {
        public RL5_4_DaftarSepuluhBesarPenyakitRajal_RSSBB()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            string healthcareServiceUnit = param[1];

            if (healthcareServiceUnit != "")
            {
                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = '{0}'", healthcareServiceUnit))[0];
                lblServiceUnit.Text = hsu.ServiceUnitName;
            }
            else
            {
                lblServiceUnit.Text = "";
            }
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }
    }
}
