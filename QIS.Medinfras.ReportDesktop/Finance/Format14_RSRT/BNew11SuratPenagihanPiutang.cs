using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Globalization;


namespace QIS.Medinfras.ReportDesktop
{
    public partial class BNew11SuratPenagihanPiutang : BaseCustomDailyPotraitRptRSRT
    {
        public BNew11SuratPenagihanPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            SettingParameter setvar = BusinessLayer.GetSettingParameterList(string.Format(
                    "ParameterCode = '{0}'", Constant.SettingParameter.MANAGER_KEUANGAN)).FirstOrDefault();
            SettingParameterDt setvardt = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, setvar.ParameterCode);
            List<SettingParameterDt> lstSetparDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID ='{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}', '{5}', '{6}')", 
                appSession.HealthcareID,
                Constant.SettingParameter.FN_FAX_NO_AR,
                Constant.SettingParameter.FN_EMAIL_AR,
                Constant.SettingParameter.FN_BAGIAN_PENAGIHAN,
                Constant.SettingParameter.DIREKTUR_YANMED,
                Constant.SettingParameter.FN_NAMA_PETUGAS_PENAGIHAN_NONOPERASIONAL,
                Constant.SettingParameter.FN_ALAMAT_EMAIL_PENAGIHAN_NONOPERASIONAL
                )).ToList();

            SettingParameterDt setvarFax = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.FN_FAX_NO_AR).FirstOrDefault();
            SettingParameterDt setvarEmail = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.FN_EMAIL_AR).FirstOrDefault();
            SettingParameterDt setvarPenagih = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.FN_BAGIAN_PENAGIHAN).FirstOrDefault();
            SettingParameterDt setvarDir = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.DIREKTUR_YANMED).FirstOrDefault();
            SettingParameterDt setvarPenagihNonOp = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.FN_NAMA_PETUGAS_PENAGIHAN_NONOPERASIONAL).FirstOrDefault();
            SettingParameterDt setvarEmailPenagihNonOp = lstSetparDt.Where(p => p.ParameterCode == Constant.SettingParameter.FN_ALAMAT_EMAIL_PENAGIHAN_NONOPERASIONAL).FirstOrDefault();
            ///BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_FAX_NO_AR);
            //SettingParameterDt setvarEmail = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_EMAIL_AR);
            //SettingParameterDt setvarPenagih = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.FN_BAGIAN_PENAGIHAN);
            //SettingParameterDt setvarDir = BusinessLayer.GetSettingParameterDt(appSession.HealthcareID, Constant.SettingParameter.DIREKTUR_YANMED);

            lblTTDir.Text = setvarDir.ParameterValue;
           
            lblTTD1.Text = setvardt.ParameterValue;
            lblTTD2.Text = "Manager Keuangan dan Akunting";
            ARInvoiceHd invoiceHd = BusinessLayer.GetARInvoiceHdList(param[0]).FirstOrDefault();
            List<vARInvoiceDt3> lstinvoicedt3 = BusinessLayer.GetvARInvoiceDt3List(string.Format("ARInvoiceID = {0}", invoiceHd.ARInvoiceID));
            List<vARInvoiceDt3> lstRanap = lstinvoicedt3.Where(t => t.DepartmentID == Constant.Facility.INPATIENT).ToList();

            //Paling lambat 14 hari setelah tagihan diterima
            Term oTerm = BusinessLayer.GetTermList(string.Format("TermID={0}", invoiceHd.TermID)).FirstOrDefault();
            string TermDay = "";
            if (oTerm != null) {
                TermDay = oTerm.TermName;
            }
            lblTempo.Text = string.Format("Paling lambat {0} setelah tagihan diterima", TermDay);

            string remarks = "";
            string remarks2 = "";
            if (invoiceHd.TransactionCode == Constant.TransactionCode.AR_INVOICE_NON_OPERATIONAL)
            {
                lblup.Text = "Dengan Hormat";
                lblTerlampir.Visible = false;
                lblTitleJatuhTempo1.Visible = false;
                lblTitleJatuhTempo2.Visible = false;
                lblTempo.Visible = false;
                lblHal.Text = invoiceHd.Remarks;
                remarks = "Dengan ini kami menyatakan bahwa tagihan dengan perincian terlampir adalah sebesar :";
                remarks2 = "";
                lblRemarks3.Text = string.Format("Dan bukti transfer di fax ke {0}, U.P {1} (Div Keuangan dan Akunting) atau email ke : {2}", setvarFax.ParameterValue, setvarPenagihNonOp.ParameterValue, setvarEmailPenagihNonOp.ParameterValue);

            }
            else
            {
                if (lstinvoicedt3.Count == lstRanap.Count)
                {
                    lblHal.Text = "Tagihan Rawat Inap";
                    remarks = "Sesuai dengan surat kerjasama yang telah disepakati mengenai pertanggungan biaya Rawat Inap di Rumah Sakit Royal Taruma, dengan ini kami menyatakan bahwa,";
                    remarks2 = "Biaya Rawat Inap dengan nama terlampir adalah senilai : ";
                }
                else
                {
                    lblHal.Text = "Tagihan Rawat Jalan";
                    remarks = "Sesuai dengan surat kerjasama yang telah disepakati mengenai pertanggungan biaya Rawat Jalan di Rumah Sakit Royal Taruma, dengan ini kami menyatakan bahwa,";
                    remarks2 = "Biaya Rawat Jalan dengan nama terlampir adalah senilai : ";
                }
                lblRemarks3.Text = string.Format("Saat pembayaran harap mencantumkan nomor invoice dan bukti transfer di e-mail : {0}U.p.: Devisi Keuangan dan Akunting ({1}, line 8173).", setvarEmail.ParameterValue, setvarPenagih.ParameterValue);
           
            }
            lblRemarks1.Text = remarks;
            lblRemarks2.Text = remarks2;
            DateTime get = invoiceHd.ARInvoiceDate;
            int tgl = get.Day;
            int month = get.Month;
            int year = get.Year;

            string result = "";
            if (month == 1)
            {
                result = "Januari";
            }
            else if (month == 2)
            {
                result = "Februari";
            }
            else if (month == 3)
            {
                result = "Maret";
            }
            else if (month == 4)
            {
                result = "April";
            }
            else if (month == 5)
            {
                result = "Mei";
            }
            else if (month == 6)
            {
                result = "Juni";
            }
            else if (month == 7)
            {
                result = "Juli";
            }
            else if (month == 8)
            {
                result = "Agustus";
            }
            else if (month == 9)
            {
                result = "September";
            }
            else if (month == 10)
            {
                result = "Oktober";
            }
            else if (month == 11)
            {
                result = "November";
            }
            else if (month == 12)
            {
                result = "Desember";
            }
            lblTanggal.Text = string.Format("Jakarta, {0} {1} {2}", tgl, result, year);
            vBusinessPartnerVirtualAccount oBusinessPartnerVirtualAccount = BusinessLayer.GetvBusinessPartnerVirtualAccountList(string.Format("ID='{0}'", invoiceHd.BusinessPartnerVirtualAccountID)).FirstOrDefault();
            if (oBusinessPartnerVirtualAccount != null) {
                Bank oBank = BusinessLayer.GetBankList(string.Format("BankID = {0}", oBusinessPartnerVirtualAccount.BankID)).FirstOrDefault();
                lblBankAN.Text = oBank.BankAccountName;
                lblBankName.Text = oBank.BankNameDisplay;
                lblBankVANo.Text = oBusinessPartnerVirtualAccount.AccountNo;

                if (oBusinessPartnerVirtualAccount.IsVirtualAccount == true)
                {
                    lblNoRekType.Text = "No. Virtual Account";
                }
                else {
                    lblNoRekType.Text = "No. Rekening"; 
                }
                
            }

            string cfTotalClaimedAmountInStringInd = invoiceHd.cfTotalClaimedAmountInStringInd.ToLower();
            TextInfo cultInfo = new CultureInfo("en-US", false).TextInfo;
            string TotalClaimedAmountInStringInd = cultInfo.ToTitleCase(cfTotalClaimedAmountInStringInd);
            lblTerbilang.Text = TotalClaimedAmountInStringInd;
           

            base.InitializeReport(param);
        }

        
        
    }
}
