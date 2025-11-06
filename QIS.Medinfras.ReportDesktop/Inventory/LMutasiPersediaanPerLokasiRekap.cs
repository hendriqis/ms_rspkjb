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
    public partial class LMutasiPersediaanPerLokasiRekap : BaseCustomDailyLandscapeA3Rpt
    {
        public LMutasiPersediaanPerLokasiRekap()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp = param[0].Split(';');
            lblPeriod.Text = string.Format("Periode : {0} s/d {1}", Helper.YYYYMMDDToDate(temp[0]).ToString(Constant.FormatString.DATE_FORMAT), Helper.YYYYMMDDToDate(temp[1]).ToString(Constant.FormatString.DATE_FORMAT));

            Location loc = BusinessLayer.GetLocation(Convert.ToInt32(param[1]));
            lblLocation.Text = string.Format("Lokasi : {0}", loc.LocationName);

            base.InitializeReport(param);
        }

    }
}
