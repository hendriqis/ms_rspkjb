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
    public partial class BKajianIGDPHS : BaseRpt
    {

        public BKajianIGDPHS()
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
            //Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID))[0];
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            pictPatient.ImageUrl = entityCV.PatientImageUrl;
            cPatientName.Text = entityPatient.FullName;
            cPayer.Text = entityCV.BusinessPartnerName;
            cBed.Text = entityCV.BedCode;
            cMedicalNo.Text = entityPatient.MedicalNo;
            cDOB.Text = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            cAge.Text = string.Format("{0} yr {1} mnth {2} day", entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cGender.Text = entityCV.Gender;
            if (entityPatient.GCBloodType != null && entityPatient.GCBloodType != "")
            {
                StandardCode entitiSC_BloodType = BusinessLayer.GetStandardCode(entityPatient.GCBloodType);
                if (entityPatient.BloodRhesus != null && entityPatient.BloodRhesus != "")
                {
                    cBloodType.Text = string.Format("{0} Rhesus {1}", entitiSC_BloodType.StandardCodeName, entityPatient.BloodRhesus);
                }
                else
                {
                    cBloodType.Text = string.Format("{0}", entitiSC_BloodType.StandardCodeName);
                }
            }
            else
            {
                cBloodType.Text = "";
            }
            if (entityCV.MobilePhoneNo1 != "")
            {
                cPhone.Text = string.Format("{0} | {1}", entityCV.PhoneNo1, entityCV.MobilePhoneNo1);
            }
            else
            {
                cPhone.Text = entityCV.PhoneNo1;
            }
            cAddress.Text = entityCV.StreetName;

            cParamedicName.Text = entityCV.ParamedicName;
            cRegistrationNo.Text = entityCV.RegistrationNo;
            cRegistrationDate.Text = entityCV.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cRegistrationTime.Text = entityCV.ActualVisitTime;
            cServiceUnit.Text = entityCV.ServiceUnitName;

            //if (entityCV.GCTriage != null && entityCV.GCTriage != "")
            //{
            //    StandardCode entitiSC_Triage = BusinessLayer.GetStandardCode(entityCV.GCTriage);
            //    cBed.Text = string.Format("{0} - {1}", entitiSC_Triage.StandardCodeName, entitiSC_Triage.TagProperty);
            //}
            //else
            //{
            //    cBed.Text = "";
            //}

            xrTableCell66.Visible = false;
            xrTableCell67.Visible = false;
            cBed.Visible = false;

            ////Buat Triagle Kalau Emergency Muncul Kalau bukan di hide
            //if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            //{
            //    xrTableCell66.Visible = true;
            //    xrTableCell67.Visible = true;
            //    cTriage.Visible = true;
            //}
            //else
            //{
            //    xrTableCell66.Visible = false;
            //    xrTableCell67.Visible = false;
            //    cTriage.Visible = false;
            //}

            if (entityDiagnose != null)
            {
                if (entityDiagnose.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                {
                    cMainDiagnose.Text = string.Format("{0} - {1}", entityDiagnose.DiagnoseID, entityDiagnose.DiagnoseName);
                }
                else
                {
                    cMainDiagnose.Text = "";
                }

                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0]));
                StringBuilder diagNotes = new StringBuilder();
                foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
                {
                    if (diagNotes.ToString() != "")
                        diagNotes.Append(", ");
                    diagNotes.Append(patientDiagnosis.DiagnosisText);
                }
                cDiagnosisNotes.Text = diagNotes.ToString();
            }
            else
            {
                cMainDiagnose.Text = "";
                cDiagnosisNotes.Text = "";
            }

            cWaktuAsuhanPerawat.Text = String.Format("{0}", DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));

            #endregion

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Diagnose
            List<vPatientDiagnosis> oDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0}", entityCV.VisitID));
            if (oDiagnose.Count() > 0)
            {
                subDiagnose.CanGrow = true;
                episodeSummaryDiagnoseRpt.InitializeReport(oDiagnose);
            }
            else
            {
                xrLabel19.Visible = false;
                subDiagnose.Visible = false;
            }
            #endregion

            #region Triage
            subTriage.CanGrow = true;
            episodeTriase2.InitializeReport(entityCV.RegistrationID);
            #endregion

            #region Survey Premier
            subSurveyPremier.CanGrow = true;
            episodeSurveyPrimier3.InitializeReport(entityCV.RegistrationID);
            #endregion

            #region Resiko Jatuh
            xResiko.CanGrow = true;
            episodeResiko2.InitializeReport(entityCV.RegistrationID);
            #endregion

            #region Anamnesis Dokter
            xAnamnesisDokter.CanGrow = true;
            episodeAnamnesisDokter1.InitializeReport(entityCV.RegistrationID);
            #endregion

            #region Pemeriksaan Fisik
            cPemeriksaanFisik.CanGrow = true;
            episodePemeriksaanFisik2.InitializeReport(entityCV.RegistrationID);
            #endregion

            #region Footer
            //lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            //lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}