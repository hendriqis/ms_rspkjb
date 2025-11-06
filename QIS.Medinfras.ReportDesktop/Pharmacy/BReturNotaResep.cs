using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BReturNotaResep : BaseCustomPharmacyA5Rpt
    {
        public BReturNotaResep()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vPrescriptionReturnOrderHd entityHd = BusinessLayer.GetvPrescriptionReturnOrderHdList(param[0])[0];
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entityHd.VisitID))[0];

            #region Header : Patient Detail
            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.PatientName;
            cDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entityCV.DateOfBirthInString, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cCorporate.Text = entityCV.BusinessPartnerName;
            cPrescriptionReturNo.Text = entityHd.PrescriptionReturnOrderNo;
            cTransactionNo.Text = entityHd.TransactionNo;
            cRegistrationNo.Text = entityCV.RegistrationNo;
            cRegisteredPhysician.Text = entityCV.ParamedicName;
            lblPembeli.Text = entityCV.PatientName;
            lblPetugas.Text = entityHd.CreatedByName;
            #endregion

            #region Header : Per Page
            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            //cHeaderRegistration.Text = entityCV.RegistrationNo;
            //cHeaderMedicalNo.Text = entityCV.MedicalNo;
            #endregion

            #region Footer
            //lblPrescriptionReturnOrderNotes.Text = entityHd.Remarks;
            //cTTDTakenBy.Text = entityCV.PatientName;
            //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
            //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
            //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
            #endregion
            base.InitializeReport(param);
        }

        //private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        //{
        //    e.Cancel = String.IsNullOrEmpty(lblPrescriptionReturnOrderNotes.Text);
        //}
    }
}
