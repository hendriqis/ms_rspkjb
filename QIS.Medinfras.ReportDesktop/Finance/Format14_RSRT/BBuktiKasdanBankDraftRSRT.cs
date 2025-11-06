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
    public partial class BBuktiKasdanBankDraftRSRT : BaseCustomDailyPotraitRpt
    {
        public BBuktiKasdanBankDraftRSRT()
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

            /*List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
            Constant.SettingParameter.SA_VERIFICATION_TREASURY, Constant.SettingParameter.SA_APPROVAL_TREASURY));

            cTTDReviewedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_VERIFICATION_TREASURY).ParameterValue;
            cTTDPrintedBy.Text = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_APPROVAL_TREASURY).ParameterValue;
       
             */ 
       }
    }
}
