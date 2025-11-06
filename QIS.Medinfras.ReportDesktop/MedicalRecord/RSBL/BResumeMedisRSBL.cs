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
    public partial class BResumeMedisRSBL : BaseRpt
    {

        public BResumeMedisRSBL()
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
                xrLabel23.Text = string.Format("RINGKASAN PASIEN PULANG\n(DISCHARGE SUMMARY)");
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

            string contents2 = string.Format(@"{0}\r\n{1}\r\n{2}",
                entityPM.FullName, entityPM.LicenseNo, entity.cfMedicalResumeDateTime);

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

            using (Bitmap bitMap = qRCodeEncoder.Encode(contents2, System.Text.Encoding.UTF8))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    bitMap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    //byte[] byteImage = ms.ToArray();
                    //imgBarCode.ImageUrl = "data:image/png;base64," + Convert.ToBase64String(byteImage);
                    ttdDokter.Image = System.Drawing.Image.FromStream(ms, true, true);
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
                    subSummary.CanGrow = true;
                    mrSummaryNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSummary.Visible = false;
                }
            }
            else
            {
                subSummary.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik
            #region Pemeriksaan Fisik Saat Masuk Perawatan
            if (entity != null)
            {
                subReviewOfSystemIn.CanGrow = true;
                mrReviewOfSystemNewIn.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subReviewOfSystemIn.Visible = false;
            }
            #endregion

            #region Pemeriksaan Fisik Saat Pulang
            if (entity != null)
            {
                List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdReviewOfSystem.Count() > 0)
                {
                    subReviewOfSystemOut.CanGrow = true;
                    mrReviewOfSystemNewOut.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReviewOfSystemOut.Visible = false;
                }
            }
            else
            {
                subReviewOfSystemOut.Visible = false;
            }
            #endregion
            #endregion

            #region Tanda Vital dan Indikator Lainnya
            #region Tanda Vital Saat Masuk Perawatan
            if (entity != null)
            {
                subVitalSignIn.CanGrow = true;
                mrVitalSignNewMRIn.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subVitalSignIn.Visible = false;
            }
            #endregion

            #region Tanda Vital Saat Pulang
            if (entity != null)
            {
                List<vVitalSignHd> lstHdVitalSign = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstHdVitalSign.Count() > 0)
                {
                    subVitalSignOut.CanGrow = true;
                    mrVitalSignNewMROut.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subVitalSignOut.Visible = false;
                }
            }
            else
            {
                subVitalSignOut.Visible = false;
            }
            #endregion
            #endregion

            #region Pemeriksaan Penunjang
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume1 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume1.Count() > 0)
                {
                    subDiagnosticExam.CanGrow = true;
                    mrDiagnosticExamNewMR.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDiagnosticExam.Visible = false;
                }
            }
            else
            {
                subDiagnosticExam.Visible = false;
            }
            #endregion

            #region Diagnosa
            #region Diagnosa Saat Masuk Perawatan
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis.Count() > 0)
                {
                    subDiagnoseIn.CanGrow = true;
                    mrDiagnoseInformationNewIn.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseIn.Visible = false;
                }
            }
            else
            {
                subDiagnoseIn.Visible = false;
            }
            #endregion

            #region Diagnosa Saat Pulang
            if (entity != null)
            {
                List<vPatientDiagnosis> lstPatientDiagnosis2 = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientDiagnosis2.Count() > 0)
                {
                    subDiagnoseOut.CanGrow = true;
                    mrDiagnoseInformationNewOut.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subDiagnoseOut.Visible = false;
                }
            }
            else
            {
                subDiagnoseOut.Visible = false;
            }
            #endregion
            #endregion

            #region Consult
            if (entity != null)
            {
                List<vPatientReferral> lstPatientReferral = BusinessLayer.GetvPatientReferralList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstPatientReferral.Count() > 0)
                {
                    subReferral.CanGrow = true;
                    mrReferralRSBL.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subReferral.Visible = false;
                }
            }
            else
            {
                subReferral.Visible = false;
            }
            #endregion

            #region Body Diagram
            if (entity != null)
            {
                List<vPatientBodyDiagramHd> lstBd = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstBd.Count() > 0)
                {
                    subBodyDiagram.CanGrow = true;
                    mrBodyDiagram.InitializeReport(entityCV.VisitID);
                }
                else
                {
                    subBodyDiagram.Visible = false;
                }
            }
            else
            {
                subBodyDiagram.Visible = false;
            }
            #endregion

            #region Ringkasan Terapi Pengobatan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume2 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume2.Count() > 0)
                {
                    subTerapiPengobatan.CanGrow = true;
                    mrTherapyPengobatanNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subTerapiPengobatan.Visible = false;
                }
            }
            else
            {
                subTerapiPengobatan.Visible = false;
            }
            #endregion

            //#region Perkembangan Selama Perawatan
            //if (entity != null)
            //{
            //    List<vMedicalResume> lstMedicalResume3 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            //    if (lstMedicalResume3.Count() > 0)
            //    {
            //        subTreatment.CanGrow = true;
            //        mrTreatmentNew.InitializeReport(entityCV.VisitID, entity.ID);
            //    }
            //    else
            //    {
            //        subTreatment.Visible = false;
            //    }
            //}
            //else
            //{
            //    subTreatment.Visible = false;
            //}
            //#endregion

            #region Prosedur Terapi dan Tindakan
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume4 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume4.Count() > 0)
                {
                    subSurgery.CanGrow = true;
                    mrSurgeryNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subSurgery.Visible = false;
                }
            }
            else
            {
                subSurgery.Visible = false;
            }
            #endregion

            #region Kondisi dan Cara Pulang
            if (entity != null)
            {
                List<vConsultVisit1> lstCV1 = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", entityCV.VisitID));

                if (lstCV1.Count() > 0)
                {
                    subDischarge.CanGrow = true;
                    mrDischarge.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subDischarge.Visible = false;
                }
            }
            else
            {
                subDischarge.Visible = false;
            }
            #endregion

            #region Instruksi dan Rencana Tindak Lanjut
            if (entity != null)
            {
                List<vMedicalResume> lstMedicalResume5 = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

                if (lstMedicalResume5.Count() > 0)
                {
                    subInstruction.CanGrow = true;
                    mrInstructionNew.InitializeReport(entityCV.VisitID, entity.ID);
                }
                else
                {
                    subInstruction.Visible = false;
                }
            }
            else
            {
                subInstruction.Visible = false;
            }
            #endregion

            #region Footer
            string filterExpressionResumeMedis = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RESUME_MEDIS_TELEPHONE);
            string filterExpressionIGD = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IGD_TELEPHONE_ON_RESUME_MEDIS);
            SettingParameterDt ResumeMedisPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionResumeMedis).FirstOrDefault();
            SettingParameterDt IGDPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionIGD).FirstOrDefault();

            cResumeMedisPhone.Text = string.Format("√ Bila ada perubahan nomor telepon dan/handphone. Mohon segera menginformasikan ke nomor {0}", ResumeMedisPhone.ParameterValue);
            cIGDPhone.Text = string.Format("√ Dalam keadaan darurat, segera ke IGD {0}", oHealthcare.HealthcareName);

            if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblTTDTanggal.Text = string.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT_4));
            }
            else
            {
                lblTTDTanggal.Text = string.Format("{0}, {1} {2}", oHealthcare.City, entityCV.DischargeDateInString, entityCV.DischargeTime);
            }

            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);

            PageFooter.Visible = reportMaster.IsShowFooter;
            #endregion
        }
    }
}
