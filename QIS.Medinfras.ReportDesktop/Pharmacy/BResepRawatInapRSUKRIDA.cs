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
    public partial class BResepRawatInapRSUKRIDA : BaseCustomDailyPotrait2Rpt
    {
        public BResepRawatInapRSUKRIDA()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            GetPrescriptionOrderDtCustom7 entityHd = BusinessLayer.GetPrescriptionOrderDtCustom7List(Convert.ToInt32(param[0])).FirstOrDefault();
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0}", entityHd.VisitID))[0];
            vRegistration entityR = BusinessLayer.GetvRegistrationList(String.Format("RegistrationID = {0}" , entityCV.RegistrationID))[0];

            if (entityR.GCCustomerType != Constant.CustomerType.PERSONAL)
            {
                vCustomerContract entityCC = BusinessLayer.GetvCustomerContractList(String.Format("BusinessPartnerID = {0}", entityR.BusinessPartnerID))[0];
                lblSkemaNo.Text = entityCC.ContractNo;
            }
            else
            {
                lblSkemaNo.Text = "-";
            }

            #region Header : Patient Detail
            lblMedicalNo.Text = entityCV.MedicalNo;
            lblPatientName.Text = entityCV.PatientName;
            lblDOB.Text = string.Format("{0} ( {1} yr {2} mth {3} day )", entityCV.DateOfBirthInString, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            lblCorporate.Text = entityCV.BusinessPartnerName;
            lblRegistrationDate.Text = entityR.RegistrationDateInString;
            lblParamedicName.Text = entityCV.ParamedicName;
            if (entityCV.DischargeDateInString != null && entityCV.DischargeDateInString != "")
            {
                lblDischargeDate.Text = entityCV.DischargeDateInString;
            }
            else
            {
                lblDischargeDate.Text = "-";
            }
            lblRegistrationNo.Text = entityCV.RegistrationNo;
            lblDiagnose.Text = entityR.DiagnosisText;
            #endregion

            #region Header : Per Page
            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entityCV.PatientName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            //cHeaderRegistration.Text = entityCV.RegistrationNo;
            //cHeaderMedicalNo.Text = entityCV.MedicalNo;
            #endregion

            #region Footer
            //cTTDTakenBy.Text = entityCV.PatientName;
            //cTTDEtiketBy.Text = entityPresHD.ParamedicName;
            //cTTDCompoundBy.Text = entityPresHD.ParamedicName;
            //cTTDVerificationBy.Text = entityPresHD.ParamedicName;
            #endregion
            base.InitializeReport(param);
        }

        private void GroupFooter2_BeforePrint(object sender, System.Drawing.Printing.PrintEventArgs e)
        {
            //e.Cancel = String.IsNullOrEmpty(lblPrescriptionReturnOrderNotes.Text);
        }
    }
}
