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
    public partial class BNew4ReturPenerimaanPembelianTanpaNilai : BaseDailyPortraitRpt
    {
        public BNew4ReturPenerimaanPembelianTanpaNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vPurchaseReturnHd entityHd = BusinessLayer.GetvPurchaseReturnHdList(param[0]).FirstOrDefault();

            lblRemarks.Text = entityHd.Remarks;

            lblSupervisorLogisticPharmacy.Text = "Supervisor Logistik Farmasi";
            lblSupervisorPharmacy.Text = "Supervisor Farmasi";
            lblManagerMedic.Text = "Manajer Medis & Manajer Penunjang Medis";

            base.InitializeReport(param);
        }

    }
}
