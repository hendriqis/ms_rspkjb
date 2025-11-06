using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LPenerimaanHarianSemuaKasirByCreatedDateAndVoidDate : BaseCustomDailyLandscapeA3Rpt
    {
        List<GetPatientPaymentReportByDate> lstMain = null;

        private decimal totalCashBackSesuaiTanggalValid = 0;
        private decimal totalCashSesuaiTanggalValid = 0;
        private decimal totalCreditSesuaiTanggalValid = 0;
        private decimal totalDebitSesuaiTanggalValid = 0;
        private decimal totalTransferSesuaiTanggalValid = 0;
        private decimal totalPiutangSesuaiTanggalValid = 0;
        private decimal totalUangMukaKeluarSesuaiTanggalValid = 0;
        private decimal totalPengembalianPembayaranSesuaiTanggalValid = 0;
        private decimal totalVoucherSesuaiTanggalValid = 0;
        private decimal totalAkhirSesuaiTanggalValid = 0;

        private decimal totalCashBackSesuaiTanggalNotValid = 0;
        private decimal totalCashSesuaiTanggalNotValid = 0;
        private decimal totalCreditSesuaiTanggalNotValid = 0;
        private decimal totalDebitSesuaiTanggalNotValid = 0;
        private decimal totalTransferSesuaiTanggalNotValid = 0;
        private decimal totalPiutangSesuaiTanggalNotValid = 0;
        private decimal totalUangMukaKeluarSesuaiTanggalNotValid = 0;
        private decimal totalPengembalianPembayaranSesuaiTanggalNotValid = 0;
        private decimal totalVoucherSesuaiTanggalNotValid = 0;
        private decimal totalAkhirSesuaiTanggalNotValid = 0;

        private decimal totalCashBackTidakSesuaiTanggalValid = 0;
        private decimal totalCashTidakSesuaiTanggalValid = 0;
        private decimal totalCreditTidakSesuaiTanggalValid = 0;
        private decimal totalDebitTidakSesuaiTanggalValid = 0;
        private decimal totalTransferTidakSesuaiTanggalValid = 0;
        private decimal totalPiutangTidakSesuaiTanggalValid = 0;
        private decimal totalUangMukaKeluarTidakSesuaiTanggalValid = 0;
        private decimal totalPengembalianPembayaranTidakSesuaiTanggalValid = 0;
        private decimal totalVoucherTidakSesuaiTanggalValid = 0;
        private decimal totalAkhirTidakSesuaiTanggalValid = 0;

        private decimal totalCashBackTidakSesuaiTanggalNotValid = 0;
        private decimal totalCashTidakSesuaiTanggalNotValid = 0;
        private decimal totalCreditTidakSesuaiTanggalNotValid = 0;
        private decimal totalDebitTidakSesuaiTanggalNotValid = 0;
        private decimal totalTransferTidakSesuaiTanggalNotValid = 0;
        private decimal totalPiutangTidakSesuaiTanggalNotValid = 0;
        private decimal totalUangMukaKeluarTidakSesuaiTanggalNotValid = 0;
        private decimal totalPengembalianPembayaranTidakSesuaiTanggalNotValid = 0;
        private decimal totalVoucherTidakSesuaiTanggalNotValid = 0;
        private decimal totalAkhirTidakSesuaiTanggalNotValid = 0;

        public LPenerimaanHarianSemuaKasirByCreatedDateAndVoidDate()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string[] temp2 = param[1].Split(';');
            lblPeriode.Text = string.Format("Tanggal : {0}", Helper.YYYYMMDDToDate(param[0]).ToString(Constant.FormatString.DATE_FORMAT));
            lblPeriodeTime.Text = string.Format("Jam : {0} s/d {1}", (temp2[0]).ToString(), (temp2[1]).ToString());

            #region load data
            lstMain = BusinessLayer.GetPatientPaymentReportByDateList(param[0],param[1]);

            foreach (GetPatientPaymentReportByDate e in lstMain) {
                if (e.Notes == "SESUAI TANGGAL")
                {
                    if (e.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    {
                        totalCashBackSesuaiTanggalValid = totalCashBackSesuaiTanggalValid + e.CashBackAmount;
                        totalCashSesuaiTanggalValid = totalCashSesuaiTanggalValid + e.CashAmount;
                        totalCreditSesuaiTanggalValid = totalCreditSesuaiTanggalValid + e.CreditCardAmount;
                        totalDebitSesuaiTanggalValid = totalDebitSesuaiTanggalValid + e.DebitCardAmount;
                        totalTransferSesuaiTanggalValid = totalTransferSesuaiTanggalValid + e.BankTransferAmount;
                        totalPiutangSesuaiTanggalValid = totalPiutangSesuaiTanggalValid + e.PiutangAmount;
                        totalUangMukaKeluarSesuaiTanggalValid = totalUangMukaKeluarSesuaiTanggalValid + e.UangMukaKeluarAmount;
                        totalPengembalianPembayaranSesuaiTanggalValid = totalPengembalianPembayaranSesuaiTanggalValid + e.PengembalianPembayaranAmount;
                        totalVoucherSesuaiTanggalValid = totalVoucherSesuaiTanggalValid + e.VoucherAmount;
                        totalAkhirSesuaiTanggalValid = totalAkhirSesuaiTanggalValid + e.cfTotal;
                    }
                    else
                    {
                        totalCashBackSesuaiTanggalNotValid = totalCashBackSesuaiTanggalNotValid + e.CashBackAmount;
                        totalCashSesuaiTanggalNotValid = totalCashSesuaiTanggalNotValid + e.CashAmount;
                        totalCreditSesuaiTanggalNotValid = totalCreditSesuaiTanggalNotValid + e.CreditCardAmount;
                        totalDebitSesuaiTanggalNotValid = totalDebitSesuaiTanggalNotValid + e.DebitCardAmount;
                        totalTransferSesuaiTanggalNotValid = totalTransferSesuaiTanggalNotValid + e.BankTransferAmount;
                        totalPiutangSesuaiTanggalNotValid = totalPiutangSesuaiTanggalNotValid + e.PiutangAmount;
                        totalUangMukaKeluarSesuaiTanggalNotValid = totalUangMukaKeluarSesuaiTanggalNotValid + e.UangMukaKeluarAmount;
                        totalPengembalianPembayaranSesuaiTanggalNotValid = totalPengembalianPembayaranSesuaiTanggalNotValid + e.PengembalianPembayaranAmount;
                        totalVoucherSesuaiTanggalNotValid = totalVoucherSesuaiTanggalNotValid + e.VoucherAmount;
                        totalAkhirSesuaiTanggalNotValid = totalAkhirSesuaiTanggalNotValid + e.cfTotal;
                    }
                }
                else
                {
                    if (e.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    {
                        totalCashBackTidakSesuaiTanggalValid = totalCashBackTidakSesuaiTanggalValid + e.CashBackAmount;
                        totalCashTidakSesuaiTanggalValid = totalCashTidakSesuaiTanggalValid + e.CashAmount;
                        totalCreditTidakSesuaiTanggalValid = totalCreditTidakSesuaiTanggalValid + e.CreditCardAmount;
                        totalDebitTidakSesuaiTanggalValid = totalDebitTidakSesuaiTanggalValid + e.DebitCardAmount;
                        totalTransferTidakSesuaiTanggalValid = totalTransferTidakSesuaiTanggalValid + e.BankTransferAmount;
                        totalPiutangTidakSesuaiTanggalValid = totalPiutangTidakSesuaiTanggalValid + e.PiutangAmount;
                        totalUangMukaKeluarTidakSesuaiTanggalValid = totalUangMukaKeluarTidakSesuaiTanggalValid + e.UangMukaKeluarAmount;
                        totalPengembalianPembayaranTidakSesuaiTanggalValid = totalPengembalianPembayaranTidakSesuaiTanggalValid + e.PengembalianPembayaranAmount;
                        totalVoucherTidakSesuaiTanggalValid = totalVoucherTidakSesuaiTanggalValid + e.VoucherAmount;
                        totalAkhirTidakSesuaiTanggalValid = totalAkhirTidakSesuaiTanggalValid + e.cfTotal;
                    }
                    else 
                    {
                        totalCashBackTidakSesuaiTanggalNotValid = totalCashBackTidakSesuaiTanggalNotValid + e.CashBackAmount;
                        totalCashTidakSesuaiTanggalNotValid = totalCashTidakSesuaiTanggalNotValid + e.CashAmount;
                        totalCreditTidakSesuaiTanggalNotValid = totalCreditTidakSesuaiTanggalNotValid + e.CreditCardAmount;
                        totalDebitTidakSesuaiTanggalNotValid = totalDebitTidakSesuaiTanggalNotValid + e.DebitCardAmount;
                        totalTransferTidakSesuaiTanggalNotValid = totalTransferTidakSesuaiTanggalNotValid + e.BankTransferAmount;
                        totalPiutangTidakSesuaiTanggalNotValid = totalPiutangTidakSesuaiTanggalNotValid + e.PiutangAmount;
                        totalUangMukaKeluarTidakSesuaiTanggalNotValid = totalUangMukaKeluarTidakSesuaiTanggalNotValid + e.UangMukaKeluarAmount;
                        totalPengembalianPembayaranTidakSesuaiTanggalNotValid = totalPengembalianPembayaranTidakSesuaiTanggalNotValid + e.PengembalianPembayaranAmount;
                        totalVoucherTidakSesuaiTanggalNotValid = totalVoucherTidakSesuaiTanggalNotValid + e.VoucherAmount;
                        totalAkhirTidakSesuaiTanggalNotValid = totalAkhirTidakSesuaiTanggalNotValid + e.cfTotal;                    
                    }
                }
            }
            #endregion

            #region sesuai tanggal

            #region total sesuai tanggal
            cSesuaiTglCashBackTotal.Text = (totalCashBackSesuaiTanggalValid + totalCashBackSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglCashTotal.Text = (totalCashSesuaiTanggalValid + totalCashSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglCreditTotal.Text = (totalCreditSesuaiTanggalValid + totalCreditSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglDebitTotal.Text = (totalDebitSesuaiTanggalValid + totalDebitSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglTransferTotal.Text = (totalTransferSesuaiTanggalValid + totalTransferSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglPiutangTotal.Text = (totalPiutangSesuaiTanggalValid + totalPiutangSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglUangMukaKeluarTotal.Text = (totalUangMukaKeluarSesuaiTanggalValid + totalUangMukaKeluarSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglPengembalianPembayaranTotal.Text = (totalPengembalianPembayaranSesuaiTanggalValid + totalPengembalianPembayaranSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglVoucherTotal.Text = (totalVoucherSesuaiTanggalValid + totalVoucherSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglTotal.Text = (totalAkhirSesuaiTanggalValid + totalAkhirSesuaiTanggalNotValid).ToString("N2");
            #endregion

            #region valid sesuai tanggal
            cSesuaiTglCashBackTotalValid.Text = (totalCashBackSesuaiTanggalValid).ToString("N2");
            cSesuaiTglCashTotalValid.Text = (totalCashSesuaiTanggalValid).ToString("N2");
            cSesuaiTglCreditTotalValid.Text = (totalCreditSesuaiTanggalValid).ToString("N2");
            cSesuaiTglDebitTotalValid.Text = (totalDebitSesuaiTanggalValid).ToString("N2");
            cSesuaiTglTransferTotalValid.Text = (totalTransferSesuaiTanggalValid).ToString("N2");
            cSesuaiTglPiutangTotalValid.Text = (totalPiutangSesuaiTanggalValid).ToString("N2");
            cSesuaiTglUangMukaKeluarTotalValid.Text = (totalUangMukaKeluarSesuaiTanggalValid).ToString("N2");
            cSesuaiTglPengembalianPembayaranTotalValid.Text = (totalPengembalianPembayaranSesuaiTanggalValid).ToString("N2");
            cSesuaiTglVoucherTotalValid.Text = (totalVoucherSesuaiTanggalValid).ToString("N2");
            cSesuaiTglTotalValid.Text = (totalAkhirSesuaiTanggalValid).ToString("N2");
            #endregion

            #region not valid sesuai tanggal
            cSesuaiTglCashBackTotalVoid.Text = (totalCashBackSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglCashTotalVoid.Text = (totalCashSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglCreditTotalVoid.Text = (totalCreditSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglDebitTotalVoid.Text = (totalDebitSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglTransferTotalVoid.Text = (totalTransferSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglPiutangTotalVoid.Text = (totalPiutangSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglUangMukaKeluarTotalVoid.Text = (totalUangMukaKeluarSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglPengembalianPembayaranTotalVoid.Text = (totalPengembalianPembayaranSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglVoucherTotalVoid.Text = (totalVoucherSesuaiTanggalNotValid).ToString("N2");
            cSesuaiTglTotalVoid.Text = (totalAkhirSesuaiTanggalNotValid).ToString("N2");
            #endregion

            #endregion

            #region tidak sesuai tanggal

            #region total tidak sesuai tanggal
            cTdkSesuaiTglCashBackTotal.Text = (totalCashBackTidakSesuaiTanggalValid + totalCashBackTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglCashTotal.Text = (totalCashTidakSesuaiTanggalValid + totalCashTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglCreditTotal.Text = (totalCreditTidakSesuaiTanggalValid + totalCreditTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglDebitTotal.Text = (totalDebitTidakSesuaiTanggalValid + totalDebitTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglTransferTotal.Text = (totalTransferTidakSesuaiTanggalValid + totalTransferTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglPiutangTotal.Text = (totalPiutangTidakSesuaiTanggalValid + totalPiutangTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglUangMukaKeluarTotal.Text = (totalUangMukaKeluarTidakSesuaiTanggalValid + totalUangMukaKeluarTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglPengembalianPembayaranTotal.Text = (totalPengembalianPembayaranTidakSesuaiTanggalValid + totalPengembalianPembayaranTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglVoucherTotal.Text = (totalVoucherTidakSesuaiTanggalValid + totalVoucherTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglTotal.Text = (totalAkhirTidakSesuaiTanggalValid + totalAkhirTidakSesuaiTanggalNotValid).ToString("N2");
            #endregion

            #region valid tidak sesuai tanggal
            cTdkSesuaiTglCashBackTotalValid.Text = (totalCashBackTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglCashTotalValid.Text = (totalCashTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglCreditTotalValid.Text = (totalCreditTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglDebitTotalValid.Text = (totalDebitTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglTransferTotalValid.Text = (totalTransferTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglPiutangTotalValid.Text = (totalPiutangTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglUangMukaKeluarTotalValid.Text = (totalUangMukaKeluarTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglPengembalianPembayaranTotalValid.Text = (totalPengembalianPembayaranTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglVoucherTotalValid.Text = (totalVoucherTidakSesuaiTanggalValid).ToString("N2");
            cTdkSesuaiTglTotalValid.Text = (totalAkhirTidakSesuaiTanggalValid).ToString("N2");
            #endregion

            #region not valid tidak sesuai tanggal
            cTdkSesuaiTglCashBackTotalVoid.Text = (totalCashBackTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglCashTotalVoid.Text = (totalCashTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglCreditTotalVoid.Text = (totalCreditTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglDebitTotalVoid.Text = (totalDebitTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglTransferTotalVoid.Text = (totalTransferTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglPiutangTotalVoid.Text = (totalPiutangTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglUangMukaKeluarTotalVoid.Text = (totalUangMukaKeluarTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglPengembalianPembayaranTotalVoid.Text = (totalPengembalianPembayaranTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglVoucherTotalVoid.Text = (totalVoucherTidakSesuaiTanggalNotValid).ToString("N2");
            cTdkSesuaiTglTotalVoid.Text = (totalAkhirTidakSesuaiTanggalNotValid).ToString("N2");
            #endregion

            #endregion


            base.InitializeReport(param);
        }

        private void xrTable2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            if ((GetCurrentColumnValue("GCTransactionStatus") != null))
            {
                String GCRegistrationStatus = GetCurrentColumnValue("GCTransactionStatus").ToString();
                if (GCRegistrationStatus == Constant.TransactionStatus.VOID)
                {
                    xrTable2.ForeColor = Color.Red;
                }
                else
                {
                    xrTable2.ForeColor = Color.Black;
                }
            }
        }
    }
}