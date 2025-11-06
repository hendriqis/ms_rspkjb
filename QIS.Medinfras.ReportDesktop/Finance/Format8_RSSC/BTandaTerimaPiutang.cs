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
    public partial class BTandaTerimaPiutang :BaseCustomDailyPotraitRpt
    {
        public BTandaTerimaPiutang()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vARInvoiceHd entity = BusinessLayer.GetvARInvoiceHdList(param[0]).FirstOrDefault();
            List<vARInvoiceDt> ARInvoiceDtlist = BusinessLayer.GetvARInvoiceDtList(param[0]);
            var patientResult = ARInvoiceDtlist.FirstOrDefault();

            if (entity.CustomerBillToName == "PERSONAL")
            {
                cBusinessPartnerName.Text = patientResult.PatientName;
                cBusinessPartnerName2.Text = patientResult.PatientName;
            }
            else
            {
                cBusinessPartnerName.Text = entity.CustomerBillToName;
                cBusinessPartnerName2.Text = entity.CustomerBillToName;
            }

            cBusinessPartnerAddressLine1.Text = entity.CustomerBillToStreetName;
            cBusinessPartnerAddressLine2.Text = entity.CustomerBillToCity + " " + entity.CustomerBillToState;

            var results = ARInvoiceDtlist.GroupBy(test => test.MedicalNo).Select(grp => grp.First()).ToList().OrderBy(x => x.MedicalNo);


            string temp = "";
            foreach (vARInvoiceDt result in results)
            {
                temp = temp + ", " + result.MedicalNo;
            }

            if (patientResult.DepartmentID == "INPATIENT")
            {
                cMain1.Text = string.Format("Surat pengantar Klaim No. Voucher : {0}, Tanggal : {1}, Rp.{2}", entity.ARInvoiceNo, entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
                cMain2.Text = string.Format("Kwitansi Klaim Asli No. Voucher : {0}, Tanggal : {1}, Rp.{2}", entity.ARInvoiceNo, entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
                cMain3.Text = string.Format("Rekap Total Biaya Asli / Tembusan No. RM{0}, Tanggal : {1}, Rp.{2}", temp, entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
            }
            else {
                cMain1.Text = string.Format("Surat pengantar Klaim No. Voucher : {0}, Tanggal : {1}, Rp.{2}", entity.ARInvoiceNo, entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
                cMain2.Text = string.Format("Kwitansi Klaim Asli No. Voucher : {0}, Tanggal : {1}, Rp.{2}", entity.ARInvoiceNo, entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
                cMain3.Text = string.Format("Rekap Total Biaya Asli / Tembusan Tanggal : {0}, Rp.{1}", entity.ARInvoiceDateInString, entity.TotalClaimedAmountInString);
            
            }

            //cDueDate.Text = entity.DueDateInString;
            //cBankName.Text = entity.BankName;
            //cBankAccountNo.Text = entity.BankAccountNo;
            //cBankAccountName.Text = entity.BankAccountName;

            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = {0}", appSession.HealthcareID)).FirstOrDefault();
            lblTanggalTTD.Text = string.Format("{0}, {1}", h.City, entity.ARInvoiceDateInString);
            //lblNamaTTD.Text = appSession.UserFullName;

            base.InitializeReport(param);
        }

    }
}
