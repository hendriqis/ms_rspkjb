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
    public partial class BQCPemeriksaanGRANOSTICMCU : BaseCustomDailyPotraitA5Rpt
    {

        public BQCPemeriksaanGRANOSTICMCU()
        {
            InitializeComponent();
        }
        public override void InitializeReport(string[] param)
        {
            String registrationID = param[0];
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            List<vRegistration> lstRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID));
            PaymentReceipt entityPayment = BusinessLayer.GetPaymentReceiptList(string.Format("RegistrationID = {0} AND IsDeleted = 0", lstRegistration.FirstOrDefault().RegistrationID)).FirstOrDefault();
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = '{0}'", registrationID)).FirstOrDefault();
            lblTanggal.Text = DateTime.Now.ToString("dd-MMM-yyyy HH:MM:ss");
            lblTanggalLahir.Text = string.Format("{0} ({1}T) ({2})", entity.DateOfBirthInString, entity.AgeInYear, entity.cfGenderInitial);
            lblJenisP.Text = string.Format("{0} ({1})",entity.VisitTypeName, entity.ParamedicName);
            if (entityPayment != null)
            {
                lblPembayar.Text = entityPayment.PrintAsName;
            }
            else
            {
                lblPembayar.Text = "";
            }
            lblPrint.Text = string.Format("Print Date : {0}, User ID : {1}",DateTime.Now.ToString("dd-MMM-yyyy hh:mm"),appSession.UserFullName);
            if (entityPayment != null)
            {
                lblName.Text = entityPayment.PaymentReceiptNo;
            }
            List<vPatientChargesDt> lstResultLab = BusinessLayer.GetvPatientChargesDtList(string.Format("RegistrationID IN ({0})", entity.RegistrationID));

            #region Lab Detail
            subDetail.CanGrow = true;
            bqcPemeriksaanGRANOSTICDetail.InitializeReport(lstResultLab.FirstOrDefault().RegistrationID);
            #endregion

            base.InitializeReport(param);
        }
        
    }
}
