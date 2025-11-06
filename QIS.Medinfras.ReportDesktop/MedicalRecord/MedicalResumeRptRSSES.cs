using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QISEncryption;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MedicalResumeRptRSSES : BaseRpt
    {
        public MedicalResumeRptRSSES()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit cv = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", cv.RegistrationID)).FirstOrDefault();
            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            if (healthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = healthcare.HealthcareName;
                cHealthcareAddress.Text = healthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", healthcare.City, healthcare.ZipCode);
                cHealthcarePhone.Text = healthcare.PhoneNo1;
                cHealthcareFax.Text = healthcare.FaxNo1;
            }

            cMedicalNo.Text = cv.MedicalNo;
            cPatientName.Text = cv.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0} / {1}", cv.DateOfBirthInString, cv.cfGenderInitial2);
            cNIK.Text = cv.SSN;
            cDPJP.Text = cv.ParamedicName;
            //if (cv.DepartmentID != Constant.Facility.INPATIENT)
            //{
            //    xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG\n(DISCHARGE SUMMARY)");
            //}
            //else
            //{
            //    xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT INAP\n(INPATIENT DISCHARGE SUMMARY)");
            //}

            if (entityReg.ReferrerParamedicID != null)
            {
                ParamedicMaster entityPMR = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityReg.ReferrerParamedicID)).FirstOrDefault();
                cDokterPengirim.Text = entityPMR.FullName;
            }
            else
            {
                cDokterPengirim.Text = "";
            }
            cBusinessPartners.Text = cv.BusinessPartnerName;

            cVisitDateTime.Text = string.Format("{0}", cv.cfVisitDateTimeInString);

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("{0}", entityPTR.FromHSU);
            }
            else
            {
                cUnitVisit.Text = string.Format("{0}", cv.ServiceUnitName);
            }

            //lblPatientName.Text = cv.PatientName;
            //lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            //lblPatientAge.Text = cv.PatientAge;
            //lblMedicalNo.Text = cv.MedicalNo;

            #region QR Codes Image
            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM0072, Constant.SettingParameter.RM0073);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                cv.MedicalNo, cv.RegistrationNo, cv.FirstName, cv.MiddleName, cv.LastName, cv.cfPatientLocation);

            if (lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0072).FirstOrDefault().ParameterValue == "1")
            {
                string url = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0073).FirstOrDefault().ParameterValue;
                string contenPlain = string.Format(@"{0}|{1}|{2}", reportMaster.ReportCode, cv.VisitID, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"));
                string ecnryptText = Encryption.EncryptString(contenPlain);
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(ecnryptText);
                contents = string.Format("{0}/{1}", url, System.Convert.ToBase64String(plainTextBytes));
            }

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

            if (obj != null)
            {
                lblSubjectiveSummaryText.Text = obj.SubjectiveResumeText;
                lblObjectiveSummaryText.Text = obj.ObjectiveResumeText;
                lblAssessmentSummaryText.Text = obj.AssessmentResumeText;
                lblPlanningSummaryText.Text = obj.PlanningResumeText;
                lblMedicationSummaryText.Text = obj.MedicationResumeText;
                if (obj.MedicalResumeText == null || obj.MedicalResumeText == "")
                {
                    lblMedicalSummaryText.Text = obj.SurgeryResumeText;
                }
                else
                {
                    lblMedicalSummaryText.Text = obj.MedicalResumeText;
                }
                lblInstructionSummaryText.Text = obj.InstructionResumeText;

                lblMedicalResumeDate.Text = string.Format("{0}, {1}", healthcare.City, obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_FORMAT));
                ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", obj.ParamedicID)).FirstOrDefault();
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPM.ParamedicCode);
                ttdDokter.Visible = true;
                lblPhysicianName.Text = obj.ParamedicName;
            }
            else
            {
                lblSubjectiveSummaryText.Text = "";
                lblObjectiveSummaryText.Text = "";
                lblAssessmentSummaryText.Text = "";
                lblPlanningSummaryText.Text = "";
                lblMedicationSummaryText.Text = "";
                lblMedicalSummaryText.Text = "";
                lblInstructionSummaryText.Text = "";

                lblMedicalResumeDate.Text = "";
                lblPhysicianName.Text = "";
            }

            base.InitializeReport(param);
        }

    }
}
