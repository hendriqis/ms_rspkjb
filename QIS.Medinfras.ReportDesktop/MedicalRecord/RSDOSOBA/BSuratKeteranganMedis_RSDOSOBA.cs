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
    public partial class BSuratKeteranganMedis_RSDOSOBA : BaseRpt
    {

        public BSuratKeteranganMedis_RSDOSOBA()
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

            #region Header 2 : Patient Information

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            ChiefComplaint entityComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            vPastMedical entityMedical = BusinessLayer.GetvPastMedicalList(string.Format("RegistrationID = {0} AND IsDeleted = 0", entityCV.RegistrationID)).FirstOrDefault();
            
            List<PatientTransferHistory> lstTransferHistory = BusinessLayer.GetPatientTransferHistoryList(string.Format("VisitID = {0}", param[0]));

            cPatientName.Text = entityPatient.FullName;
            cMedicalNo.Text = entityPatient.MedicalNo;
            cGender.Text = entityCV.Gender;

            if (entityComplaint == null)
            {
                cAnamnesis.Text = "";
            }
            else
            {
                cAnamnesis.Text = entityComplaint.ChiefComplaintText;
            }

            cActualVisitDate.Text = entityCV.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);

            if (entityCV.PhysicianDischargedBy != 0)
            {
                UserAttribute entityUA = BusinessLayer.GetUserAttributeList(string.Format("UserID = {0}", entityCV.PhysicianDischargedBy)).FirstOrDefault();
                ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityUA.ParamedicID)).FirstOrDefault();
                if (entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900" || entityCV.PhysicianDischargedDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                {
                    if (entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) != "01-Jan-1900" || entityCV.DischargeDate.ToString("dd-MM-yyyy") != Constant.ConstantDate.DEFAULT_NULL)
                        cDischargeInfo.Text = String.Format("{0} | {1} - {2} | {3}", entityCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT), entityCV.RoomName, entityCV.BedCode, entityPM.FullName);
                    else
                        cDischargeInfo.Text = String.Format("{0} | {1} - {2} | {3}", entityCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT), entityCV.RoomName, entityCV.BedCode, entityPM.FullName);
                }
            }

            //if (entityCV.GCDischargeMethod == null)
            //{
            //    cFollowUpTherapy.Text = "";
            //}
            //else
            //{
            //    StandardCode entitySD = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = {0}", entityCV.GCDischargeMethod)).FirstOrDefault();
            //    cFollowUpTherapy.Text = entitySD.StandardCodeName;
            //}

            //if (entityCV.ReferralPhysicianID == null)
            //{
            //    xrTableCell56.Text = "";
            //}
            //else
            //{
            //    ParamedicMaster entityPM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ReferralPhysicianID)).FirstOrDefault();
            //    xrTableCell56.Text = entityPM.FullName;
            //}

            #endregion

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Pernyataan pemberian kuasa
            xrTableCell83.Text = string.Format("Dengan ini saya memberikan kuasa kepada dokter {0} yang telah memeriksa, melakukan tindakan/ operasi atau merawat saya karena sebab apapun, untuk memberikan keterangan-keterangan lengkap termasuk riwayat medis saya sebelumnya kepada perusahaan/ asuransi tersebut di atas. Dalam hal ini saya akan mengganti kepada perusahaan/ asuransi tersebut biaya yang tidak dipertanggungkan dalam polis. Penggantian biaya tersebut akan saya lakukan berdasarkan prosedur yang berlaku pada perusahaan/ asuransi tersebut. Saya menyatakan semua yang tertulis ini benar adanya dan dibuat tanpa paksaan dari pihak manapun. Dilarang memperbanyak, mengkopy, merubah atau menyebarkan isi dari informasi medis ini.", oHealthcare.HealthcareName);
            xrTableCell20.Text = string.Format("I hereby authorize any physician of {0}, who has been giving me health service due to what ever the causes are, to give my medical record /information including my past medical history to company/insurance as written above. And I will pay the exceeded a mount in accordance with the company procedure as written in my policy insurance. I declare that all this statement was written truthfully without force from any one. Disciosure, copying, altering or communication of this message if you are not the addresses is prohibit.", oHealthcare.HealthcareName);
            #endregion

            #region Subjektif
            List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", entityCV.MRN));
            if (lstAllergy.Count() > 0)
            {
                subPatientAllergies.CanGrow = true;
                episodeSummaryAllergyRptRSSBB.InitializeReport(entityCV.MRN);
            }
            else
            {
                subPatientAllergies.Visible = false;
            }
            #endregion

            #region Obyektif
            //vVitalSignHd lstVHD = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            //vVitalSignDt lstVDT = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            
            //vVitalSignDt lstKes = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 88 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblKesadaran.Text = lstKes.VitalSignValue + " " + lstKes.ValueUnit;
            //vVitalSignDt lstGSC = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 97 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblGCS.Text = lstGSC.VitalSignValue + " " + lstGSC.ValueUnit;
            //vVitalSignDt lstGSCE = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 13 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblGCSE.Text = lstGSCE.VitalSignValue + " " + lstGSCE.ValueUnit;
            //vVitalSignDt lstGSCM = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 14 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblGCSM.Text = lstGSCM.VitalSignValue + " " + lstGSCM.ValueUnit;
            //vVitalSignDt lstGCSV = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 15 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblGCSV.Text = lstGCSV.VitalSignValue + " " + lstGCSV.ValueUnit;

            //vVitalSignDt lstRR = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 5 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblRR.Text = lstRR.VitalSignValue + " " + lstRR.ValueUnit;
            //vVitalSignDt lstSuhu = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 1 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblSuhu.Text = lstSuhu.VitalSignValue + " " + lstSuhu.ValueUnit;
            //vVitalSignDt lstSPO2 = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 30 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblSPO2.Text = lstSPO2.VitalSignValue + " " + lstSPO2.ValueUnit;

            //vVitalSignDt lstBB = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 8 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblBB.Text = lstBB.VitalSignValue + " " + lstBB.ValueUnit;
            //vVitalSignDt lstTB = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 9 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
            //lblTB.Text = lstTB.VitalSignValue + " " + lstTB.ValueUnit;
            #endregion

            #region Body Diagram
            List<vPatientBodyDiagramHd> lstHdBodyDiagram = BusinessLayer.GetvPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstHdBodyDiagram.Count() > 0)
            {
                subBodyDiagram.CanGrow = true;
                episodeSummaryBodyDiagramRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                episodeSummaryBodyDiagramRpt.Visible = false;
            }
            #endregion

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            string laboratoryUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            string imagingUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;

            #region Laboratory Order
            List<vTestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", entityCV.VisitID, laboratoryUnitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdTestOrder.Count() > 0)
            {
                subLabOrder.CanGrow = true;
                episodeSummaryLabOrderRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subLabOrder.Visible = false;
            }
            #endregion

            #region Imaging Order
            List<vTestOrderHd> lstImagingTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", entityCV.VisitID, imagingUnitID, Constant.TransactionStatus.VOID));

            if (lstImagingTestOrder.Count() > 0)
            {
                subImagingOrder.CanGrow = true;
                episodeSummaryImagingOrderRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subImagingOrder.Visible = false;
            }
            #endregion

            #region Diagnosis
            List<PatientDiagnosis> lstentityDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstentityDiagnose.Count() > 0)
            {
                subDiagnosis.CanGrow = true;
                episodeSummaryDiagnosisRptRSAJ1.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subDiagnosis.Visible = false;
            }
            #endregion

            #region Resep yang diberikan
            List<vGetPrescriptionOrderDtRM> oPrescOrderDt = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType = '{1}'", entityCV.VisitID, Constant.ItemType.OBAT_OBATAN));
            subPrescription.CanGrow = true;
            mR2PrescriptionRSSBB.InitializeReport(oPrescOrderDt);
            #endregion
            
            //end

            #region Vital Sign
            List<vVitalSignHd> lstVitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstVitalSignHd.Count() > 0)
            {
                subVitalSign.CanGrow = true;
                episodeSummaryVitalSignRpt.InitializeReport(entityCV.VisitID, 1);
            }
            else
            {
                xrLabel13.Visible = false;
                subVitalSign.Visible = false;
            }
            #endregion

            #region Review of System
            List<vReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstHdReviewOfSystem.Count() > 0)
            {
                subReviewOfSystem.CanGrow = true;
                episodeSummaryReviewOfSystemRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel14.Visible = false;
                subReviewOfSystem.Visible = false;
            }
            #endregion

            #region Prescription
            List<vPrescriptionOrderHd> lstEntityHdPrescription = BusinessLayer.GetvPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdPrescription.Count() > 0)
            {
                subPrescription.CanGrow = true;
                episodeSummaryPrescriptionRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel17.Visible = false;
                subPrescription.Visible = false;
            }
            #endregion

            #region Integrated Notes
            vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format(
                    "VisitID = {0}", entityCV.VisitID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration.LinkedRegistrationID != null)
            {
                cvLinkedID = entityLinkedRegistration.LinkedRegistrationID;
            }
            string filterExpressionIN = string.Format(
                "VisitID IN ({0},{1}) AND GCPatientNoteType IN ('{2}', '{3}', '{4}', '{5}', '{6}','{7}') ORDER BY NoteDate DESC, NoteTime DESC",
               entityCV.VisitID, cvLinkedID, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.FOLLOWUP_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT);
            List<vPatientVisitNote> lstEntityVisit = BusinessLayer.GetvPatientVisitNoteList(filterExpressionIN);

            if (lstEntityVisit.Count() > 0)
            {
                subIntegratedNotes.CanGrow = true;
                episodeSummaryIntegratedNotesRptRSAJ1.InitializeReport(entityCV.VisitID);
            }
            else
            {
                subIntegratedNotes.Visible = false;
            }
            #endregion

            #region Discharge
            vConsultVisit lstEntity = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0} AND IsMainVisit = 1", entityCV.VisitID)).FirstOrDefault();

            if (String.IsNullOrEmpty(lstEntity.DischargeCondition))
            {
                xrLabel18.Visible = false;
                subDischarge.Visible = false;
            }
            else
            {
                subDischarge.CanGrow = true;
                episodeSummaryDischargeRpt1.InitializeReport(entityCV.VisitID);
            }
            #endregion

            #region Vital Sign
            List<vVitalSignHd> lstVitalSignHdLast = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstVitalSignHdLast.Count() > 0)
            {
                subVitalSignLast.CanGrow = true;
                episodeSummaryVitalSignRpt1.InitializeReport(entityCV.VisitID, 0);
            }
            else
            {
                xrLabel21.Visible = false;
                subVitalSignLast.Visible = false;
            }
            #endregion

            #region Footer
            cCityDate.Text = String.Format("{0}, {1}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT)); // Surabaya, 13-Nov-2017
            lblPhysicianName.Text = entityCV.ParamedicName;
            ParamedicMaster entityPMM = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", entityCV.ParamedicID)).FirstOrDefault();
            lblSIP.Text = entityPMM.LicenseNo;
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}