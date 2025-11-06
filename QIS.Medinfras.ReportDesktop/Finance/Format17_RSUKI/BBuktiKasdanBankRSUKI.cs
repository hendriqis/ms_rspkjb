using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BBuktiKasdanBankRSUKI : BaseCustomDailyPotraitRpt
    {
        public BBuktiKasdanBankRSUKI()
        {
            InitializeComponent();
        }

        private void lblJudul_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String GLTransactionID = GetCurrentColumnValue("GLTransactionID").ToString();
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
                else if (GCTreasuryType == Constant.TreasuryType.REALISASI_KAS_BON)
                {
                    lblJudul.Text = "KAS BON";
                }
                else if (GCTreasuryType == Constant.TreasuryType.PERMINTAAN_KAS_BON)
                {
                    lblJudul.Text = "KAS BON";
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

            List<vTreasuryTransaction> entity = BusinessLayer.GetvTreasuryTransactionList(string.Format("GLTransactionID = {0} AND DisplayOrder = 1", GLTransactionID));
            lblAkunTreasury.Text = string.Format("{0} {1}", entity.FirstOrDefault().GLAccountNo, entity.FirstOrDefault().GLAccountName);

            //string filterNamattd1 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IM_NAMA_JABATAN_DIREKTUR); // Direktur
            //string filterNamattd2 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_WAKIL_DIREKTUR_KEUANGAN); // Direktur Keuangan
            //string filterNamattd3 = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.FN_KEPALABAGIAN_KEUANGAN); // Kabag Keuangan
            //SettingParameterDt namaTTD1 = BusinessLayer.GetSettingParameterDtList(filterNamattd1).FirstOrDefault();
            //SettingParameterDt namaTTD2 = BusinessLayer.GetSettingParameterDtList(filterNamattd2).FirstOrDefault();
            //SettingParameterDt namaTTD3 = BusinessLayer.GetSettingParameterDtList(filterNamattd3).FirstOrDefault();

            //cTTDDisetujui.Text = namaTTD3.ParameterValue;
            //cTTDDireki1.Text = namaTTD1.ParameterValue;
            //cTTDDireksi2.Text = namaTTD2.ParameterValue;

            lblttd1.Text = "Kabag. Keuangan";
            lblttd2.Text = "Direksi";
            lblttd3.Text = "Direksi";
        }
    }
}
