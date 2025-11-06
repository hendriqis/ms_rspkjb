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
    public partial class LRekapKartuStockInfo2 : BaseDailyPortraitRpt
    {
        public LRekapKartuStockInfo2()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            lblPeriod.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));

            Location loc = BusinessLayer.GetLocation(Convert.ToInt32(param[1]));
            lblLocation.Text = string.Format("{0} / {1}", loc.LocationCode, loc.LocationName);

            if(String.IsNullOrEmpty(param[3].ToString())) {
                lblStatusTampilan.Text = "Semua (Ada Dan Tidak Ada Transaksi Pasien)";
            }
            else {
                lblStatusTampilan.Text = "Ada Transaksi Pasien";
            }
            base.InitializeReport(param);
        }

    }
}
