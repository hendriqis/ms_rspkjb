using System;
using System.Linq;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Data;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturPenerimaanPembelianTanpaNilaiKKDI : BaseDailyPortraitRpt
    {
        public BReturPenerimaanPembelianTanpaNilaiKKDI()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vPurchaseReturnHd entityHd = BusinessLayer.GetvPurchaseReturnHdList(param[0]).FirstOrDefault();

            lblRemarks.Text = entityHd.Remarks;

            lblCreatedByName.Text = entityHd.CreatedByName;

            lblApprovedByName.Text = "Dr. Susi Anggraini, MM";

            base.InitializeReport(param);
        }

    }
}
