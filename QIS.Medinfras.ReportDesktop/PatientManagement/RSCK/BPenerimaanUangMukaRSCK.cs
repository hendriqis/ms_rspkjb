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
    public partial class BPenerimaanUangMukaRSCK : BaseCustomPharmacyA5Rpt
    {
        public BPenerimaanUangMukaRSCK()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vHealthcare entityHealthcare = BusinessLayer.GetvHealthcareList(String.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];

            vPatientPaymentHd entityPayment = BusinessLayer.GetvPatientPaymentHdList(string.Format("{0}", param[0]))[0];
            lblLastUpdatedDate.Text = entityHealthcare.City + ", " + entityPayment.PaymentDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblLastUpdatedBy.Text = appSession.UserFullName;
            lblTotalAmountString.Text = "# " + entityPayment.TotalPaymentAmountInString + " #";

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityPayment.RegistrationID))[0];
            lblUnitPelayanan.Text = entityReg.ServiceUnitName;
            lblParamedicName.Text = entityReg.ParamedicName;

            base.InitializeReport(param);

                #region Header
                //cMedicalNo.Text = entity.MedicalNo;
                //cPatientName.Text = entity.PatientName;
                //cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entity.DateOfBirthInString, entity.AgeInYear, entity.AgeInMonth, entity.AgeInDay);
                //cCorporate.Text = entity.BusinessPartnerName;
                //cPrescriptionNo.Text = entity.TransactionNo;
                //cOrderNo.Text = entity.PrescriptionOrderNo;
                //cRegistrationNo.Text = entity.RegistrationNo;
                //cRegisteredPhysician.Text = entity.ParamedicName;
                #endregion

                #region Header : Per Page
                //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                //    entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
                //cHeaderRegistration.Text = entityCV.RegistrationNo;
                //cHeaderMedicalNo.Text = entityCV.MedicalNo;
                #endregion

                #region Footer
                //lblPrescriptionOrderNotes.Text = entity.Remarks;
                //lblOrderNotes.Text = entity.Remarks;
                //cTTDTakenBy.Text = entityCV.PatientName;
                //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
                //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
                //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
                #endregion
        }

    }
}