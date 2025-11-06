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
    public partial class BPatientBillDetailRSMDRekap : BaseDailyPortraitRpt
    {
        private int _lineNumber = 0;
        public BPatientBillDetailRSMDRekap()
        {
            _lineNumber = 0;
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string filterExpression = param[0];
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(filterExpression).LastOrDefault();

            #region HEADER

            cNoRegNoRM.Text = string.Format("{0} | {1}", entityReg.RegistrationNo, entityReg.MedicalNo);
            cNamaPasien.Text = entityReg.PatientName;
            cTanggalLahir.Text = entityReg.DateOfBirthInString;
            cDokterUtama.Text = entityReg.ParamedicName;
            cTanggalMasuk.Text = entityReg.RegistrationDateInString;

            cRuangPerawatan.Text = entityReg.ServiceUnitName;
            cNoTempatTidur.Text = entityReg.BedCode;
            cKelas.Text = entityReg.ClassName;
            cPenjaminBayar.Text = entityReg.BusinessPartnerName;
            String tanggalPulang = entityReg.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT);
            if (tanggalPulang == "01-Jan-1900")
            {
                cTanggalPulang.Text = "-";
            }
            else
            {
                cTanggalPulang.Text = tanggalPulang;
            }

            #endregion

            #region SUMMARY

            filterExpression += string.Format(" AND IsDeleted = 0 AND GCTransactionStatus != '{0}'", Constant.TransactionStatus.VOID);
            List<vPatientChargesDt> lstPDT = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            List<vPatientBill> lstBill = BusinessLayer.GetvPatientBillList(string.Format("RegistrationID = {0} AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));
            List<vPatientPaymentHd> lstPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format(
                "RegistrationID IN ({0}) AND GCTransactionStatus != '{1}'",
                entityReg.RegistrationID, Constant.TransactionStatus.VOID));

            decimal chargesP = lstPDT.Sum(p => p.PatientAmount);
            decimal chargesC = lstPDT.Sum(p => p.PayerAmount);

            decimal adminP = lstBill.Sum(p => p.PatientAdminFeeAmount);
            decimal adminC = lstBill.Sum(p => p.AdministrationFeeAmount);
            cAdministrasiPasien.Text = adminP.ToString(Constant.FormatString.NUMERIC_2);
            cAdministrasiInstansi.Text = adminC.ToString(Constant.FormatString.NUMERIC_2);

            decimal serviceP = lstBill.Sum(p => p.PatientServiceFeeAmount);
            decimal serviceC = lstBill.Sum(p => p.ServiceFeeAmount);
            cServicePasien.Text = serviceP.ToString(Constant.FormatString.NUMERIC_2);
            cServiceInstansi.Text = serviceC.ToString(Constant.FormatString.NUMERIC_2);

            decimal discP = lstBill.Sum(p => p.PatientDiscountAmount);
            decimal discC = lstBill.Sum(p => p.PayerDiscountAmount);
            cDiskonPasien.Text = discP.ToString(Constant.FormatString.NUMERIC_2);
            cDiskonInstansi.Text = discC.ToString(Constant.FormatString.NUMERIC_2);

            decimal coverC = lstBill.Sum(p => p.CoverageAmount);
            cTanggungan.Text = coverC.ToString(Constant.FormatString.NUMERIC_2);

            decimal billP = lstBill.Sum(p => p.TotalPatientAmount);
            decimal billC = lstBill.Sum(p => p.TotalPayerAmount);
            cTagihanPasien.Text = billP.ToString(Constant.FormatString.NUMERIC_2);
            cTagihanInstansi.Text = billC.ToString(Constant.FormatString.NUMERIC_2);
            
            decimal settlement = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.SETTLEMENT).Sum(p => p.ReceiveAmount);
            cPelunasan.Text = settlement.ToString(Constant.FormatString.NUMERIC_2);

            decimal piutangP = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.AR_PATIENT).Sum(p => p.ReceiveAmount);
            decimal piutangC = lstPayment.Where(p => p.GCPaymentType == Constant.PaymentType.AR_PAYER).Sum(p => p.ReceiveAmount);
            cPiutangPasien.Text = piutangP.ToString(Constant.FormatString.NUMERIC_2);
            cPiutangInstansi.Text = piutangC.ToString(Constant.FormatString.NUMERIC_2);

            decimal sisaTagihanP = chargesP - settlement - piutangP;
            decimal sisaTagihanC = chargesC - piutangC;
            cSisaTagihanPasien.Text = sisaTagihanP.ToString(Constant.FormatString.NUMERIC_2);
            cSisaTagihanInstansi.Text = sisaTagihanC.ToString(Constant.FormatString.NUMERIC_2);

            #endregion

            base.InitializeReport(param);
        }

        private void cDtUnitTransaksi_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            String HealthcareServiceUnitID = GetCurrentColumnValue("HealthcareServiceUnitID").ToString();
            vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", HealthcareServiceUnitID)).FirstOrDefault();

            cDtUnitTransaksi.Text = hsu.ServiceUnitName;
        }

        private void lblNo_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            lblNo.Text = (++_lineNumber).ToString();
        }

    }
}
