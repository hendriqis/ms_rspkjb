using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BRekonsiliasiDanPemantauanTerapiObatPasienRSDO : BaseRpt
    {

        public BRekonsiliasiDanPemantauanTerapiObatPasienRSDO()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }
            #endregion

            #region Header 2 : Patient

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            //info kanan atas
            cPatientName.Text = entityPatient.FullName + " (" + entityCV.cfGenderInitial + ")";
            cDOB.Text = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            cMedicalNo.Text = entityPatient.MedicalNo;

            List<vParamedicTeam> entityPTeam = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole NOT IN ('{1}', '{2}') AND IsDeleted = 0", entityCV.RegistrationID, Constant.ParamedicRole.DOKTER_JAGA, Constant.ParamedicRole.LAIN_LAIN));
            if (entityPTeam == null)
            {
                cPatientPJ.Text = "(1) " + entityCV.ParamedicName;
            }
            else
            {
                List<vParamedicTeam> lstPTeam = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole NOT IN ('{1}', '{2}') AND IsDeleted = 0", entityCV.RegistrationID, Constant.ParamedicRole.DOKTER_JAGA, Constant.ParamedicRole.LAIN_LAIN));
                StringBuilder PTeam = new StringBuilder();
                Int32 i = 0;
                foreach (vParamedicTeam paramedicTeam in lstPTeam)
                {
                    i = i + 1;
                    PTeam.Append("(" + i + ") ");
                    PTeam.Append(String.Format("{0} ", paramedicTeam.ParamedicName, Environment.NewLine));
                    PTeam.Append(Environment.NewLine);
                }
                cPatientPJ.Text = PTeam.ToString();
            }

            vVitalSignHd vitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).LastOrDefault();
            if (vitalSignHd == null)
            {
                cTTV.Text = String.Format("    Kg /     Cm");
            }
            else
            {
                //Weight
                vVitalSignDt vitalSignDtWeight = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID = 8", entityCV.VisitID)).LastOrDefault();
                String valueWeight;

                if (vitalSignDtWeight.VitalSignID == null)
                    valueWeight = "";
                else
                    valueWeight = vitalSignDtWeight.VitalSignValue;

                vVitalSignDt vitalSignDtHeight = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND IsDeleted = 0 AND VitalSignID = 9", entityCV.VisitID)).LastOrDefault();
                String valueHeight;

                //Height
                if (vitalSignDtHeight.VitalSignID == null)
                    valueHeight = "";
                else
                    valueHeight = vitalSignDtHeight.VitalSignValue;

                //Weight & Height
                cTTV.Text = String.Format("{0} Kg / {1} Cm", valueWeight, valueHeight);
            }

            //cRegistrationDate.Text = entityCV.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            //cRegistrationNo.Text = entityCV.RegistrationNo;

            #endregion

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Allergy
            List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", entityCV.MRN));
                subPatientAllergies.CanGrow = true;
                episodeSummaryAllergyRpt.InitializeReport(entityCV.MRN);
            #endregion

            #region Footer
            //lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Jakarta, 29-Mar-2019
            //lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}