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
    public partial class EpisodeSummaryWithIntegratedRpt : BaseRpt
    {

        public EpisodeSummaryWithIntegratedRpt()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion

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
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(String.Format(param[0].ToString())).FirstOrDefault();
          //  vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            //Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID))[0];
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
           // vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();

            pictPatient.ImageUrl = entityCV.PatientImageUrl;
            cPatientName.Text = entityCV.PatientName;
            cMedicalNo.Text = entityCV.MedicalNo;
            cDOB.Text = entityCV.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
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

            if (entityCV.GCTriage != null && entityCV.GCTriage != "")
            {
                StandardCode entitiSC_Triage = BusinessLayer.GetStandardCode(entityCV.GCTriage);
                cTriage.Text = string.Format("{0} - {1}", entitiSC_Triage.StandardCodeName, entitiSC_Triage.TagProperty);
            }
            else
            {
                cTriage.Text = "";
            }

            xrTableCell66.Visible = false;
            xrTableCell67.Visible = false;
            cTriage.Visible = false;

            //Buat Triagle Kalau Emergency Muncul Kalau bukan di hide
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

                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("{0} AND IsDeleted = 0", param[0]));
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

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                entityCV.MedicalNo, entityCV.RegistrationNo, entityCV.FirstName, entityCV.MiddleName, entityCV.LastName, entityCV.cfPatientLocation);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 0;
            qRCodeEncoder.QRCodeErrorCorrect = QRCodeEncoder.ERROR_CORRECTION.L;
            MemoryStream memoryStream = new MemoryStream();
            //System.Web.UI.WebControls.Image imgBarCode = new System.Web.UI.WebControls.Image();
            //imgBarCode.Height = 400;
            //imgBarCode.Width = 400;

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

            #endregion

            //#region Header 3 : Per Page

            //cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
            //    entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            //cHeaderRegistration.Text = entityReg.RegistrationNo;
            //cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            //#endregion

            //#region Episode Information

            //lblVisitType.Text = string.Format("{0} - {1}", entityCV.VisitTypeCode, entityCV.VisitTypeName);
            //if (entityCV.GCVisitReason != null && entityCV.GCVisitReason != "")
            //{
            //    StandardCode entitiSC_VisitReason = BusinessLayer.GetStandardCode(entityCV.GCVisitReason);
            //    lblVisitReason.Text = entitiSC_VisitReason.StandardCodeName;
            //}
            //else
            //{
            //    lblVisitReason.Text = "";
            //}
            //lblLOS.Text = entityCV.LOS;

            //subChiefComplaint.CanGrow = true;
            //episodeSummaryChiefComplaintRpt.InitializeReport(entityCV.VisitID);

            //subPatientAllergies.CanGrow = true;
            //episodeSummaryAllergyRpt.InitializeReport(entityCV.VisitID);

            //#endregion

            //#region Vital Sign
            //subVitalSign.CanGrow = true;
            //episodeSummaryVitalSignRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            //#region Review of System
            //subReviewOfSystem.CanGrow = true;
            //episodeSummaryReviewOfSystemRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            //#region Body Diagram
            //subBodyDiagram.CanGrow = true;
            //episodeSummaryBodyDiagramRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            //#region Test Order
            //subTestOrder.CanGrow = true;
            //episodeSummaryTestOrderRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            //#region Prescription
            //subPrescription.CanGrow = true;
            //episodeSummaryPrescriptionRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            #region Integrated Notes
            subIntegratedNotes.CanGrow = true;
            episodeSummaryIntegratedNotesRpt.InitializeReport(entityCV.VisitID);
            #endregion

        }
    }
}
