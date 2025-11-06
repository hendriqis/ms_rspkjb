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

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BResumeMedisRSPM : BaseRpt
    {
        public BResumeMedisRSPM()
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
            vMedicalResume entity = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
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
                //xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG\n(DISCHARGE SUMMARY)");
                if (entityCV.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT JALAN\n(OUTPATIENT DISCHARGE SUMMARY)");
                }
                else if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
                {
                    xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG EMERGENCY\n(EMERGENCY DISCHARGE SUMMARY)");
                }
                else
                {
                    xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG\n(DISCHARGE SUMMARY)");
                }
            }
            else
            {
                xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG RAWAT INAP\n(INPATIENT DISCHARGE SUMMARY)");
            }

            if (entityReg.ReferrerParamedicID != null)
            {
                ParamedicMaster entityPMR = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityReg.ReferrerParamedicID)).FirstOrDefault();
                cDokterPengirim.Text = entityPMR.FullName;
            }
            else
            {
                cDokterPengirim.Text = "";
            }
            cBusinessPartners.Text = entityBP.BusinessPartnerName;

            cVisitDateTime.Text = string.Format("Tgl & Jam Masuk : {0}", entityCV.cfVisitDateTimeInString);

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar** : {0}", entityCV.cfDischargeDateTimeInString);
                }
                else
                {
                    cDischargeDateTime.Text = "Tgl & Jam Keluar :";
                }
            }
            else
            {
                if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeOrderInString);
                }
                else
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar** : {0}", entityCV.cfDischargeDateTimeInString);
                }
            }

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.FromHSU);
            }
            else
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityCV.ServiceUnitName);
            }

            if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL || entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                cUnitDischarge.Text = string.Format("Kamar/Bagian     : {0}", entityCV.ServiceUnitName);
            }
            else
            {
                cUnitDischarge.Text = "Kamar/Bagian   :";
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

            #region Riwayat Kesehatan / Riwayat Penyakit
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume.Count() > 0)
                {
                    subSummaryRSSES.CanGrow = true;
                    mrSummaryRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSummaryRSSES.Visible = false;
                }
            }
            else
            {
                subSummaryRSSES.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            #region Pemeriksaan Fisik Saat Masuk Perawatan
            if (entity != null)
            {
                subReviewOfSystemInRSSES.CanGrow = true;
                mrReviewOfSystemNewInRSSES.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subReviewOfSystemInRSSES.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik Saat Pulang
            if (entity != null)
            {
                List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdReviewOfSystem.Count() > 0)
                {
                    subReviewOfSystemOutRSSES.CanGrow = true;
                    mrReviewOfSystemNewOutRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReviewOfSystemOutRSSES.Visible = false;
                }
            }
            else
            {
                subReviewOfSystemOutRSSES.Visible = false;
            }
            #endregion
            #endregion

            #region Tanda Vital dan Indikator Lainnya
            #region Tanda Vital Saat Masuk Perawatan
            if (entity != null)
            {
                subVitalSignInRSSES.CanGrow = true;
                mrVitalSignNewMRInRSSES.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subVitalSignInRSSES.Visible = false;
            }
            #endregion

            #region Tanda Vital Saat Pulang
            if (entity != null)
            {
                List<vVitalSignHd> lstHdVitalSign = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdVitalSign.Count() > 0)
                {
                    subVitalSignOutRSSES.CanGrow = true;
                    mrVitalSignNewMROutRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subVitalSignOutRSSES.Visible = false;
                }
            }
            else
            {
                subVitalSignOutRSSES.Visible = false;
            }
            #endregion
            #endregion

            #region Pemeriksaan Penunjang
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume1 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume1.Count() > 0)
                {
                    subDiagnosticExamRSSES.CanGrow = true;
                    mrDiagnosticExamNewMRRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDiagnosticExamRSSES.Visible = false;
                }
            }
            else
            {
                subDiagnosticExamRSSES.Visible = false;
            }
            #endregion

            #region Diagnosa
            #region Diagnosa Saat Masuk Perawatan
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis.Count() > 0)
                {
                    subDiagnoseInRSSES.CanGrow = true;
                    mrDiagnoseInformationNewInRSSES.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseInRSSES.Visible = false;
                }
            }
            else
            {
                subDiagnoseInRSSES.Visible = false;
            }
            #endregion

            #region Diagnosa Saat Pulang
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis2 = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis2.Count() > 0)
                {
                    subDiagnoseOutRSSES.CanGrow = true;
                    mrDiagnoseInformationNewOutRSSES.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseOutRSSES.Visible = false;
                }
            }
            else
            {
                subDiagnoseOutRSSES.Visible = false;
            }
            #endregion
            #endregion

            #region Consult
            if (entity != null)
            {
                List<vPatientReferral> lstPatientReferral = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientReferral.Count() > 0)
                {
                    subReferralRSSES.CanGrow = true;
                    mrReferralRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReferralRSSES.Visible = false;
                }
            }
            else
            {
                subReferralRSSES.Visible = false;
            }
            #endregion

            #region Body Diagram
            if (entity != null)
            {
                List<vPatientBodyDiagramHd> lstBd = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstBd.Count() > 0)
                {
                    subBodyDiagramRSSES.CanGrow = true;
                    mrBodyDiagramRSSES.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subBodyDiagramRSSES.Visible = false;
                }
            }
            else
            {
                subBodyDiagramRSSES.Visible = false;
            }
            #endregion

            #region Ringkasan Terapi Pengobatan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume2 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume2.Count() > 0)
                {
                    subTerapiPengobatanRSSES.CanGrow = true;
                    mrTherapyPengobatanNewRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subTerapiPengobatanRSSES.Visible = false;
                }
            }
            else
            {
                subTerapiPengobatanRSSES.Visible = false;
            }
            #endregion

            #region Prosedur Terapi dan Tindakan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume4 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume4.Count() > 0)
                {
                    subSurgeryRSSES.CanGrow = true;
                    mrSurgeryNewRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSurgeryRSSES.Visible = false;
                }
            }
            else
            {
                subSurgeryRSSES.Visible = false;
            }
            #endregion

            #region Kondisi dan Cara Pulang
            if (entity != null)
            {
                List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entityCV.VisitID));

                if (lstCV1.Count() > 0)
                {
                    subDischargeRSSES.CanGrow = true;
                    mrDischargeRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDischargeRSSES.Visible = false;
                }
            }
            else
            {
                subDischargeRSSES.Visible = false;
            }
            #endregion

            #region Instruksi dan Rencana Tindak Lanjut
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume5 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume5.Count() > 0)
                {
                    subInstructionRSSES.CanGrow = true;
                    mrInstructionNewRSSES.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subInstructionRSSES.Visible = false;
                }
            }
            else
            {
                subInstructionRSSES.Visible = false;
            }
            #endregion

            #region Footer
            string filterExpressionResumeMedis = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RESUME_MEDIS_TELEPHONE);
            string filterExpressionIGD = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IGD_TELEPHONE_ON_RESUME_MEDIS);
            SettingParameterDt ResumeMedisPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionResumeMedis).FirstOrDefault();
            SettingParameterDt IGDPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionIGD).FirstOrDefault();

            cResumeMedisPhone.Text = string.Format("√ Bila ada perubahan nomor telepon dan/handphone. Mohon segera menginformasikan ke nomor {0}", ResumeMedisPhone.ParameterValue);
            cIGDPhone.Text = string.Format("√ Dalam keadaan darurat, hubungi IGD {0} ke nomor {1}", oHealthcare.HealthcareName, IGDPhone.ParameterValue);

            if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_4));
            }
            else
            {
                lblTTDTanggal.Text = string.Format("{0}, {1} {2}", oHealthcare.City, entityCV.DischargeDateInString, entityCV.DischargeTime);
            }

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityCV.ParamedicCode);
            ttdDokter.Visible = true;

            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS-{0}, Print Date/Time:{1}, User:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserFullName);
            #endregion

        }
    }
}
