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
    public partial class BKajianIGDRSSBB : BaseRpt
    {
        public BKajianIGDRSSBB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare

            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();
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

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            #region Header 2 : Patient

            cPatientName.Text = entityCV.PatientName;
            cMedicalNo.Text = entityCV.MedicalNo;
            cDOB.Text = entityCV.DateOfBirthInString;
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
            if (entityPatient.GCOccupation != null && entityPatient.GCOccupation != "")
            {
                StandardCode entitiSC_Occ = BusinessLayer.GetStandardCode(entityPatient.GCOccupation);
                if (entityPatient.BloodRhesus != null && entityPatient.BloodRhesus != "")
                {
                    cOccupation.Text = string.Format("{0}", entitiSC_Occ.StandardCodeName);
                }
                else
                {
                    cOccupation.Text = string.Format("{0}", entitiSC_Occ.StandardCodeName);
                }
            }
            else
            {
                cOccupation.Text = "";
            }
            cAddress.Text = entityCV.StreetName + " " + entityCV.City + " " + entityCV.ZipCode + " ";


            cParamedicName.Text = entityCV.ParamedicName;
            cRegistrationNo.Text = entityCV.RegistrationNo;
            cRegistrationDate.Text = entityCV.ActualVisitDateInString;
            cRegistrationTime.Text = entityCV.ActualVisitTime;
            cServiceUnit.Text = entityCV.ServiceUnitName + " " + entityCV.RoomName;
            cBed.Text = entityCV.BedCode;
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
            cPayer.Text = entityCV.BusinessPartnerName;

            #endregion

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                                                    entityPatient.FullName,
                                                    entityCV.Gender,
                                                    entityCV.AgeInYear,
                                                    entityCV.AgeInMonth,
                                                    entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion
        }

    }
}
