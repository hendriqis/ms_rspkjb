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
    public partial class LSensusRIPerTanggalPerRuang : BaseDailyLandscapeRpt
    {
        public LSensusRIPerTanggalPerRuang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ServiceUnitMaster sum = BusinessLayer.GetServiceUnitMaster(Convert.ToInt32(param[1]));
            lblServiceUnit.Text = string.Format("Bangsal : {0}", sum.ServiceUnitName);
            lblPeriod.Text = string.Format("Periode : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));

            base.InitializeReport(param);
        }

    }
}
