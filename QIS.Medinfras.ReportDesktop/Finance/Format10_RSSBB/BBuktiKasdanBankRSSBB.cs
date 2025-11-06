using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BBuktiKasdanBankRSSBB : BaseCustomDailyPotraitA5Rpt
    {
        public BBuktiKasdanBankRSSBB()
        {
            InitializeComponent();
        }

        private void lblJudul_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String TransactionCode = GetCurrentColumnValue("TransactionCode").ToString();
            String GCTreasuryType = GetCurrentColumnValue("GCTreasuryType").ToString();

            if (TransactionCode == "7282" || TransactionCode == "7283")
            {
                if (GCTreasuryType == Constant.TreasuryType.PENERIMAAN)
                {
                    lblJudul.Text = "KAS PENERIMAAN";
                }
                else if (GCTreasuryType == Constant.TreasuryType.PENGELUARAN)
                {
                    lblJudul.Text = "KAS PENGELUARAN";
                }
                else if (GCTreasuryType == Constant.TreasuryType.PINDAH_BUKU)
                {
                    lblJudul.Text = "KAS PINDAH BUKU";
                }
                else
                {
                    lblJudul.Text = "";
                }
            }
            else if (TransactionCode == "7284" || TransactionCode == "7285")
            {
                if (GCTreasuryType == Constant.TreasuryType.PENERIMAAN)
                {
                    lblJudul.Text = "BANK PENERIMAAN";
                }
                else if (GCTreasuryType == Constant.TreasuryType.PENGELUARAN)
                {
                    lblJudul.Text = "BANK PENGELUARAN";
                }
                else if (GCTreasuryType == Constant.TreasuryType.PINDAH_BUKU)
                {
                    lblJudul.Text = "BANK PINDAH BUKU";
                }
                else
                {
                    lblJudul.Text = "";
                }
            }
            else
            {
                lblJudul.Text = "";
            }
        }

    }
}
