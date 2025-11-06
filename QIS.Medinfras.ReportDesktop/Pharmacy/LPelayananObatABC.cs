using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPelayananObatABC :BaseCustomDailyLandscapeRpt
    {
        public LPelayananObatABC()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));
            vHealthcareServiceUnit vsu = BusinessLayer.GetvHealthcareServiceUnitList(String.Format("HealthcareServiceUnitID = {0}", param[1])).FirstOrDefault();
            lblFarmasi.Text = String.Format("Farmasi : {0}", vsu.ServiceUnitName);
            base.InitializeReport(param);
        }
    }
}
