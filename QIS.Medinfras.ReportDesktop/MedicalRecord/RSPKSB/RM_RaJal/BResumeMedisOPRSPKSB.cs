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
using ThoughtWorks.QRCode.Codec;
using QISEncryption;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BResumeMedisOPRSPKSB : BaseRpt
    {
        public BResumeMedisOPRSPKSB()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            //vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(param[0])[0];
            //vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", entity.VisitID))[0];

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", entityCV.VisitID)).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
            BusinessPartners entityBP = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", entityCV.BusinessPartnerID)).FirstOrDefault();
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();
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

            #region Header 2 : Patient Page
            cMedicalNo.Text = entityCV.MedicalNo;
            cPatientName.Text = entityCV.cfPatientNameSalutation;
            cDOB.Text = string.Format("{0} / {1}", entityCV.DateOfBirthInString, entityCV.cfGenderInitial2);
            cNIK.Text = entityPatient.SSN;
            cDPJP.Text = entityPM.FullName;
            if (entityCV.DepartmentID != Constant.Facility.INPATIENT)
            {
                xrLabel23.Text = string.Format("RESUME MEDIS RAWAT JALAN");
            }
            else
            {
                xrLabel23.Text = string.Format("RESUME MEDIS RAWAT INAP");
            }

            cBusinessPartners.Text = entityBP.BusinessPartnerName;

            cVisitDateTime.Text = string.Format("Tgl & Jam Masuk : {0}", entityCV.cfVisitDateTimeInString);

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.FromHSU);
            }
            else
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityCV.ServiceUnitName);
            }

            #region QR Codes Image
            string filterSetvar = string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.RM0072, Constant.SettingParameter.RM0073);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                entityCV.MedicalNo, entityCV.RegistrationNo, entityCV.FirstName, entityCV.MiddleName, entityCV.LastName, entityCV.cfPatientLocation);

            if (lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0072).FirstOrDefault().ParameterValue == "1")
            {
                string url = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM0073).FirstOrDefault().ParameterValue;
                string contenPlain = string.Format(@"{0}|{1}|{2}", reportMaster.ReportCode, entityCV.VisitID, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"));
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
                    //pictMRNQR.Image = System.Drawing.Image.FromStream(ms, true, true);
                }
            }
            #endregion

            #endregion

            #region Riwayat Kesehatan / Riwayat Penyakit
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", entityCV.VisitID));
                if (lstMedicalResume.Count() > 0)
                {
                    if (lstMedicalResume.FirstOrDefault().SubjectiveResumeText != null)
                    {
                        subSummaryRSPKSB.CanGrow = true;
                        mrSummaryOPRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                    }
                    else
                    {
                        subSummaryRSPKSB.Visible = false;
                        xrLabel11.Visible = false;
                        xrLabel29.Visible = false;
                    }
                }
                else
                {
                    subSummaryRSPKSB.Visible = false;
                    xrLabel11.Visible = false;
                    xrLabel29.Visible = false;
                }
            }
            else
            {
                subSummaryRSPKSB.Visible = false;
                xrLabel29.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            #region Pemeriksaan Fisik Saat Masuk Perawatan
            if (entity != null)
            {
                subReviewOfSystemInRSPKSB.CanGrow = true;
                mrReviewOfSystemNewInOPRSPKSB.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subReviewOfSystemInRSPKSB.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik Saat Pulang
            if (entity != null)
            {
                List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdReviewOfSystem.Count() > 0)
                {
                    subReviewOfSystemOutRSPKSB.CanGrow = true;
                    mrReviewOfSystemNewOutRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReviewOfSystemOutRSPKSB.Visible = false;
                }
            }
            else
            {
                subReviewOfSystemOutRSPKSB.Visible = false;
            }
            #endregion
            #endregion

            #region Tanda Vital dan Indikator Lainnya
            #region Tanda Vital Saat Masuk Perawatan
            if (entity != null)
            {
                subVitalSignInRSPKSB.CanGrow = true;
                mrVitalSignNewMRInOPRSPKSB.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subVitalSignInRSPKSB.Visible = false;
                xrLabel20.Visible = false;
            }
            #endregion

            #region Tanda Vital Saat Pulang
            if (entity != null)
            {
                List<vVitalSignHd> lstHdVitalSign = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdVitalSign.Count() > 0)
                {
                    subVitalSignOutRSPKSB.CanGrow = true;
                    mrVitalSignNewMROutRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subVitalSignOutRSPKSB.Visible = false;
                    xrLabel21.Visible = false;
                    xrLabel25.Visible = false;
                }
            }
            else
            {
                subVitalSignOutRSPKSB.Visible = false;
                xrLabel21.Visible = false;
                xrLabel25.Visible = false;
            }
            #endregion
            #endregion

            #region Pemeriksaan Penunjang
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume1 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0 ORDER BY ID DESC", entityCV.VisitID));

                if (lstMedicalResume1.Count() > 0)
                {
                    subDiagnosticExamRSPKSB.CanGrow = true;
                    mrDiagnosticExamNewMRRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDiagnosticExamRSPKSB.Visible = false;
                    xrLabel5.Visible = false;
                    xrLabel8.Visible = false;
                }
            }
            else
            {
                subDiagnosticExamRSPKSB.Visible = false;
                xrLabel5.Visible = false;
                xrLabel8.Visible = false;
            }
            #endregion

            #region Diagnosa
            #region Diagnosa Saat Masuk Perawatan
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis.Count() > 0)
                {
                    subDiagnoseInRSPKSB.CanGrow = true;
                    mrDiagnoseInformationNewInRSPKSB.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseInRSPKSB.Visible = false;
                    xrLabel6.Visible = false;
                }
            }
            else
            {
                subDiagnoseInRSPKSB.Visible = false;
                xrLabel6.Visible = false;
            }
            #endregion

            #region Diagnosa Saat Pulang
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis2 = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis2.Count() > 0)
                {
                    subDiagnoseOutRSPKSB.CanGrow = true;
                    mrDiagnoseInformationNewOutRSPKSB.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseOutRSPKSB.Visible = false;
                    xrLabel14.Visible = false;
                    xrLabel13.Visible = false;
                }
            }
            else
            {
                subDiagnoseOutRSPKSB.Visible = false;
                xrLabel14.Visible = false;
                xrLabel13.Visible = false;
            }
            #endregion
            #endregion

            #region Consult
            if (entity != null)
            {
                List<vPatientReferral> lstPatientReferral = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientReferral.Count() > 0)
                {
                    subReferralRSPKSB.CanGrow = true;
                    mrReferralRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReferralRSPKSB.Visible = false;
                    xrLabel24.Visible = false;
                    xrLabel26.Visible = false;
                }
            }
            else
            {
                subReferralRSPKSB.Visible = false;
                xrLabel24.Visible = false;
                xrLabel26.Visible = false;
            }
            #endregion

            #region Body Diagram
            if (entity != null)
            {
                List<vPatientBodyDiagramHd> lstBd = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstBd.Count() > 0)
                {
                    subBodyDiagramRSPKSB.CanGrow = true;
                    mrBodyDiagramRSPKSB.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subBodyDiagramRSPKSB.Visible = false;
                    xrLabel16.Visible = false;
                    xrLabel17.Visible = false;
                }
            }
            else
            {
                subBodyDiagramRSPKSB.Visible = false;
                xrLabel16.Visible = false;
                xrLabel17.Visible = false;
            }
            #endregion

            #region Ringkasan Terapi Pengobatan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume2 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", entityCV.VisitID));

                if (lstMedicalResume2.Count() > 0)
                {
                    subTerapiPengobatanRSPKSB.CanGrow = true;
                    mrTherapyPengobatanNewRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subTerapiPengobatanRSPKSB.Visible = false;
                    xrLabel7.Visible = false;
                    xrLabel9.Visible = false;
                }
            }
            else
            {
                subTerapiPengobatanRSPKSB.Visible = false;
                xrLabel7.Visible = false;
                xrLabel9.Visible = false;
            }
            #endregion

            #region Prosedur Terapi dan Tindakan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume4 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", entityCV.VisitID));

                if (lstMedicalResume4.Count() > 0)
                {
                    subSurgery.CanGrow = true;
                    mrSurgeryNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSurgery.Visible = false;
                    xrLabel15.Visible = false;
                    xrLabel18.Visible = false;
                }
            }
            else
            {
                subSurgery.Visible = false;
                xrLabel15.Visible = false;
                xrLabel18.Visible = false;
            }
            #endregion

            #region Kondisi dan Cara Pulang
            if (entity != null)
            {
                List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entityCV.VisitID));

                if (lstCV1.Count() > 0)
                {
                    subDischargeRSPKSB.CanGrow = true;
                    mrDischargeOPRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDischargeRSPKSB.Visible = false;
                    xrLabel10.Visible = false;
                    xrLabel12.Visible = false;
                }
            }
            else
            {
                subDischargeRSPKSB.Visible = false;
                xrLabel10.Visible = false;
                xrLabel12.Visible = false;
            }
            #endregion

            #region Instruksi dan Rencana Tindak Lanjut
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume5 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", entityCV.VisitID));

                if (lstMedicalResume5.Count() > 0)
                {
                    subInstructionRSPKSB.CanGrow = true;
                    mrInstructionNewRSPKSB.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subInstructionRSPKSB.Visible = false;
                    xrLabel19.Visible = false;
                    xrLabel35.Visible = false;
                }
            }
            else
            {
                subInstructionRSPKSB.Visible = false;
                xrLabel19.Visible = false;
                xrLabel35.Visible = false;
            }
            #endregion

            #region Footer
            string filterExpressionResumeMedis = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RESUME_MEDIS_TELEPHONE);
            string filterExpressionIGD = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IGD_TELEPHONE_ON_RESUME_MEDIS);
            SettingParameterDt ResumeMedisPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionResumeMedis).FirstOrDefault();
            SettingParameterDt IGDPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionIGD).FirstOrDefault();

            cResumeMedisPhone.Text = string.Format("√ Bila ada perubahan nomor telepon dan/handphone. Mohon segera menginformasikan ke nomor {0}", ResumeMedisPhone.ParameterValue);
            cIGDPhone.Text = string.Format("√ Dalam keadaan darurat, hubungi IGD {0} ke nomor {1}", oHealthcare.HealthcareName, IGDPhone.ParameterValue);

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, entityCV.DischargeDateInString);
                }
                else
                {
                    lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
                }
            }
            else
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, entityCV.cfPhysicianDischargedDateOrderInString);
                }
                else
                {
                    lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, entityCV.DischargeDateInString);
                }
            }
            
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            ttdDokter.Visible = true;

            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS-{0}, Print Date/Time:{1}, User:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserFullName);

            PageFooter.Visible = reportMaster.IsShowFooter;
            #endregion

        }
    }
}
