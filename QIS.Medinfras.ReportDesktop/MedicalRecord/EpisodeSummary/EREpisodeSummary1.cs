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
    public partial class EREpisodeSummary1 : BaseRpt
    {

        public EREpisodeSummary1()
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

            List<vConsultVisit> lstVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0]));
            vConsultVisit entityCV = lstVisit.FirstOrDefault();
            //Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID))[0];
            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            ChiefComplaint entityComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            PastMedical entityMedical = BusinessLayer.GetPastMedicalList(string.Format("RegistrationID = {0} AND IsDeleted = 0", entityCV.RegistrationID)).FirstOrDefault();

            List<PatientTransferHistory> lstTransferHistory = BusinessLayer.GetPatientTransferHistoryList(string.Format("VisitID = {0}", param[0]));

            if (entityComplaint != null)
            {
                if (entityComplaint.IsPatientAllergyExists != null || entityComplaint.IsPatientAllergyExists != false)
                {
                    cAllergy.Text = entityComplaint.IsPatientAllergyExists ? "Ya / YES" : "Tidak Ada / NO";
                }
                else
                {
                    cAllergy.Text = "";
                }
                cRPS.Text = entityComplaint.HPISummary;
                cRPD.Text = entityMedical != null ? entityMedical.MedicalSummary : string.Empty;
            }
            else
            {
                cAllergy.Text = "";
                cRPS.Text = "";
                cRPD.Text = "";
            }

            cOtherHistory.Text = entityMedical != null ? entityMedical.MedicationSummary : string.Empty;
            pictPatient.ImageUrl = entityCV.PatientImageUrl;
            cPatientName.Text = entityPatient.FullName;
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

            if (lstTransferHistory.Count > 0)
            {
                PatientTransferHistory oHistory = lstTransferHistory.FirstOrDefault();
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(oHistory.FromParamedicID);
                cParamedicName.Text = oParamedic.FullName;
            }
            else
            {
                cParamedicName.Text = entityCV.ParamedicName;
            }

            cRegistrationNo.Text = entityCV.RegistrationNo;
            cRegistrationDate.Text = entityCV.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT);
            cRegistrationTime.Text = entityCV.ActualVisitTime;
            cServiceUnit.Text = entityCV.ServiceUnitName;

            if (entityCV.GCTriage != null && entityCV.GCTriage != "")
            {
                StandardCode entitiSC_Triage = BusinessLayer.GetStandardCode(entityCV.GCTriage);
                cTriage.Text = string.Format("{0}", entitiSC_Triage.StandardCodeName);
            }
            else
            {
                cTriage.Text = "";
            }

            //xrTableCell66.Visible = false;
            //xrTableCell67.Visible = false;
            //cTriage.Visible = false;

            //Buat Triagle Kalau Emergency Muncul Kalau bukan di hide
            if (entityCV.DepartmentID == Constant.Facility.EMERGENCY)
            {
                xrTableCell66.Visible = true;
                xrTableCell67.Visible = true;
                cTriage.Visible = true;
            }
            else
            {
                xrTableCell66.Visible = false;
                xrTableCell67.Visible = false;
                cTriage.Visible = false;
            }

            //Tambahan baca diagnosa, jika ada diagnosa masuk tetapi tidak ada diagnosa utama
            Healthcare hsu = BusinessLayer.GetHealthcareList(string.Format(""))[0];
            if (hsu.Initial == "RSAJ")
            {
                if (entityDiagnose != null)
                {
                    vPatientDiagnosis diagmain = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entityCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
                    vPatientDiagnosis diagearly = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entityCV.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();

                    if (diagmain == null)
                    {
                        if (diagearly != null)
                        {
                            cMainDiagnose.Text = string.Format("{0} - {1} (Diagnosa Masuk)", diagearly.DiagnoseID, diagearly.DiagnoseName);
                        }
                        else
                        {
                            cMainDiagnose.Text = "";
                        }
                    }
                    else
                    {
                        cMainDiagnose.Text = string.Format("{0} - {1}", diagmain.DiagnoseID, diagmain.DiagnoseName);
                    }

                    List<PatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0]));
                    StringBuilder diagNotes = new StringBuilder();
                    foreach (PatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
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
            }
            else
            {
                //Perbaikan baca diagnosa utama agar muncul di cetakan
                if (entityDiagnose != null)
                {
                    //if (entityDiagnose.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    //{
                    //    cMainDiagnose.Text = string.Format("{0} - {1}", entityDiagnose.DiagnoseID, entityDiagnose.DiagnoseName);
                    //}
                    //else
                    //{
                    //    cMainDiagnose.Text = "";
                    //}

                    vPatientDiagnosis diagmain = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", entityCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
                    if (diagmain != null)
                    {
                        cMainDiagnose.Text = string.Format("{0} - {1}", diagmain.DiagnoseID, diagmain.DiagnoseName);
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
            }

            #region QR Codes Image
            string contents = string.Format(@"{0}\r\n{1}\r\n{2}\r\n{3}\r\n{4}\r\n{5}",
                entityCV.MedicalNo, entityCV.RegistrationNo, entityCV.FirstName, entityCV.MiddleName, entityCV.LastName, entityCV.cfPatientLocation);

            QRCodeEncoder qRCodeEncoder = new QRCodeEncoder();
            qRCodeEncoder.QRCodeEncodeMode = QRCodeEncoder.ENCODE_MODE.BYTE;
            qRCodeEncoder.QRCodeScale = 4;
            qRCodeEncoder.QRCodeVersion = 7;
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

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Episode Information

            lblVisitType.Text = string.Format("{0} - {1}", entityCV.VisitTypeCode, entityCV.VisitTypeName);
            if (entityCV.GCVisitReason != null && entityCV.GCVisitReason != "")
            {
                StandardCode entitiSC_VisitReason = BusinessLayer.GetStandardCode(entityCV.GCVisitReason);
                lblVisitReason.Text = entitiSC_VisitReason.StandardCodeName;
            }
            else
            {
                lblVisitReason.Text = "";
            }
            lblLOS.Text = entityCV.LOS;

            List<ChiefComplaint> lstChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstChiefComplaint.Count() > 0)
            {
                subChiefComplaint.CanGrow = true;
                episodeSummaryChiefComplaintRptRSAJ1.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel3.Visible = false;
                xrLabel4.Visible = false;
                subChiefComplaint.Visible = false;
            }

            //List<vPatientAllergy> lstAllergy = BusinessLayer.GetvPatientAllergyList(string.Format("MRN = {0} AND IsDeleted = 0", entityCV.MRN));
            //if (lstAllergy.Count() > 0)
            //{
            //    subPatientAllergies.CanGrow = true;
            //    episodeSummaryAllergyRpt.InitializeReport(entityCV.MRN);
            //}
            //else 
            //{
            //    xrLabel5.Visible = false;
            //    xrLabel6.Visible = false;
            //    subPatientAllergies.Visible = false;
            //}
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
                xrLabel27.Visible = false;
                subDiagnose.Visible = false;
            }
            #endregion

            #region Vital Sign
            List<VitalSignHd> lstVitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
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
            List<ReviewOfSystemHd> lstHdReviewOfSystem = BusinessLayer.GetReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

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

            #region Body Diagram
            List<PatientBodyDiagramHd> lstHdBodyDiagram = BusinessLayer.GetPatientBodyDiagramHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstHdBodyDiagram.Count() > 0)
            {
                subBodyDiagram.CanGrow = true;
                episodeSummaryBodyDiagramRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel15.Visible = false;
                episodeSummaryBodyDiagramRpt.Visible = false;
            }
            #endregion

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            string laboratoryUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            string imagingUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;


            #region Laboratory Order
            List<TestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetTestOrderHdList(string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", entityCV.VisitID, laboratoryUnitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdTestOrder.Count() > 0)
            {
                subLabOrder.CanGrow = true;
                episodeSummaryLabOrderRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel16.Visible = false;
                subLabOrder.Visible = false;
            }
            #endregion

            #region Imaging Order
            List<TestOrderHd> lstImagingTestOrder = BusinessLayer.GetTestOrderHdList(string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", entityCV.VisitID, imagingUnitID, Constant.TransactionStatus.VOID));

            if (lstImagingTestOrder.Count() > 0)
            {
                subImagingOrder.CanGrow = true;
                episodeSummaryImagingOrderRpt.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel26.Visible = false;
                subImagingOrder.Visible = false;
            }
            #endregion

            #region Prescription
            List<PrescriptionOrderHd> lstEntityHdPrescription = BusinessLayer.GetPrescriptionOrderHdList(string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", entityCV.VisitID, Constant.TransactionStatus.VOID));

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

            #region Diagnosis
            List<PatientDiagnosis> lstentityDiagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstentityDiagnose.Count() > 0)
            {
                subDiagnosis.CanGrow = true;
                episodeSummaryDiagnosisRptRSAJ1.InitializeReport(entityCV.VisitID);
            }
            else
            {
                xrLabel19.Visible = false;
                subDiagnosis.Visible = false;
            }
            #endregion

            #region Integrated Notes
            //vConsultVisit entityLinkedRegistration = BusinessLayer.GetvConsultVisitList(string.Format(
            //        "VisitID = {0}", entityCV.VisitID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityCV.LinkedRegistrationID != null)
            {
                cvLinkedID = entityCV.LinkedRegistrationID;
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
                xrLabel20.Visible = false;
                subIntegratedNotes.Visible = false;
            }
            #endregion

            #region Discharge
            vConsultVisit lstEntity = BusinessLayer.GetvConsultVisitList(String.Format("VisitID = {0} AND IsMainVisit = 1", entityCV.VisitID)).FirstOrDefault();

            if (lstEntity == null)
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
            //List<vVitalSignHd> lstVitalSignHdLast = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));

            if (lstVitalSignHd.Count() > 0)
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

            //#region Integrated Notes
            //subIntegratedNotes.CanGrow = true;
            //episodeSummaryIntegratedNotesRpt.InitializeReport(entityCV.VisitID);
            //#endregion

            #region Footer
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}