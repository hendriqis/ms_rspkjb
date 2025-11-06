using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BStockOpnameDenganSaldo : BaseDailyPortraitRpt
    {
        public BStockOpnameDenganSaldo()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vStockTakingHd entityHd = BusinessLayer.GetvStockTakingHdList(param[0])[0];
            lblStockOpnameNo.Text = entityHd.StockTakingNo;
            lblStockOpnameDate.Text = entityHd.FormDateInString;
            lblLocation.Text = String.Format("{0} - {1}", entityHd.LocationCode, entityHd.LocationName);
            lblCreatedByName.Text = entityHd.CreatedByName;
            base.InitializeReport(param);
        }

    }
}
