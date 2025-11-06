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
    public partial class BNew4ReturPenerimaanPembelianDenganNilai : BaseDailyPortraitRpt
    {
        public BNew4ReturPenerimaanPembelianDenganNilai()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {

            vPurchaseReturnHd entityHd = BusinessLayer.GetvPurchaseReturnHdList(param[0]).FirstOrDefault();

            lblRemarks.Text = entityHd.Remarks;

            cTransactionAmount.Text = entityHd.TransactionAmount.ToString("N2");
            cPPNAmount.Text = entityHd.VATAmount.ToString("N2");
            cNetAmount.Text = entityHd.GrandTotalTransactionAmount.ToString("N2");

            lblSupervisorLogisticPharmacy.Text = "Supervisor Logistik Farmasi";
            lblSupervisorPharmacy.Text = "Supervisor Farmasi";
            lblManagerMedic.Text = "Manajer Medis & Manajer Penunjang Medis";

            base.InitializeReport(param);
        }

    }

}
