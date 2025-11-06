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
    public partial class EpisodeSummaryInpatientRSSK : BaseRpt
    {

        public EpisodeSummaryInpatientRSSK()
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

            ChiefComplaint entityComplaintFrom = new ChiefComplaint();

            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0])).FirstOrDefault();

            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN))[0];
            vPatientDiagnosis entityDiagnose = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();

            ChiefComplaint entityComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();

            vPastMedical entityMedical = BusinessLayer.GetvPastMedicalList(string.Format("RegistrationID = {0} AND IsDeleted = 0", entityCV.RegistrationID)).FirstOrDefault();

            List<PatientTransferHistory> lstTransferHistory = BusinessLayer.GetPatientTransferHistoryList(string.Format("VisitID = {0}", param[0]));

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            string laboratoryUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
            string imagingUnitID = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;

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
            //Buat Triase Kalau Emergency Muncul Kalau bukan di hide
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

            #region Header 3 : Per Page

            cHeaderPatient.Text = string.Format("{0} | {1} | {2} yr {3} mnth {4} day",
                entityPatient.FullName, entityCV.Gender, entityCV.AgeInYear, entityCV.AgeInMonth, entityCV.AgeInDay);
            cHeaderRegistration.Text = entityCV.RegistrationNo;
            cHeaderMedicalNo.Text = entityPatient.MedicalNo;

            #endregion

            #region Chief Complaint and History of Present Illness
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
            //List<vChiefComplaint> lstChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            //if (lstChiefComplaint.Count() > 0)
            //{
            //    subChiefComplaint.CanGrow = true;
            //    episodeSummaryChiefComplaintRptRSAJ1.InitializeReport(entityCV.VisitID);
            //}
            //else
            //{
            //    xrLabel3.Visible = false;
            //    xrLabel4.Visible = false;
            //    subChiefComplaint.Visible = false;
            //}
            //cRPS.Text = entityComplaint.HPISummary;
            //cRPD.Text = entityMedical != null ? entityMedical.MedicalSummary : string.Empty;
            //cOtherHistory.Text = entityMedical != null ? entityMedical.MedicationSummary : string.Empty;

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

            #region Subjektif
            if (entityComplaint == null)
            {
                cAllergy.Text = "";
                lblChiefComplaintText.Text = "";
                lblHPISummary.Text = "";
            }
            else
            {

                cAllergy.Text = entityComplaint.IsPatientAllergyExists ? "Ya / YES" : "Tidak Ada / NO";
                lblChiefComplaintText.Text = entityComplaint.ChiefComplaintText;
                lblHPISummary.Text = entityComplaint.HPISummary;

                if (entityComplaint.FamilyHistory != null)
                {
                    lblFamilyHistory.Text = entityComplaint.FamilyHistory;
                }

                if (entityComplaint.NursingObjectives != null)
                {
                    lblNursingObjectives.Text = entityComplaint.NursingObjectives;
                }

                if (entityComplaint.EstimatedLOS != null)
                {
                    if (entityComplaint.IsEstimatedLOSInDays)
                    {
                        lblEstimatedLOS.Text = string.Format("Perkiraan lama hari perawatan : {0} Hari", Convert.ToInt32(entityComplaint.EstimatedLOS));
                    }
                    else
                    {
                        lblEstimatedLOS.Text = string.Format("Perkiraan lama hari perawatan : {0} Minggu", Convert.ToInt32(entityComplaint.EstimatedLOS));
                    }
                    checkBox1.Checked = true;
                }

                if (entityComplaint.IsNeedDischargePlan)
                {
                    checkBox2.Checked = true;
                    checkBox3.Checked = true;
                }
                else
                {
                    checkBox2.Checked = true;
                    checkBox4.Checked = true;
                }
            }

            if (entityMedical != null)
            {
                lblMedicalSummary.Text = entityMedical.MedicalSummary;
            }
            else
            {
                if (entityComplaint != null)
                {
                    lblMedicalSummary.Text = entityComplaint.PastMedicalHistory;
                }
            }

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

            #region VitalSignHd
            //vVitalSignHd lstVHD = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", entityCV.VisitID)).FirstOrDefault();
            vVitalSignHd lstVHD = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsInitialAssessment = 1 AND GCParamedicMasterType != '{1}' ORDER BY ID DESC", entityCV.VisitID, Constant.ParamedicType.Nutritionist)).FirstOrDefault();
            if (lstVHD != null)
            {
                vVitalSignDt lstVDT = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();

                vVitalSignDt lstGSC = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 70 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstGSC != null)
                {
                    lblGCS.Text = lstGSC.VitalSignValue + " " + lstGSC.ValueUnit;
                }

                vVitalSignDt lstGSCE = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 18 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstGSCE != null)
                {
                    string gcsE = lstGSCE.VitalSignValue;
                    string E = gcsE.Substring(0,1);
                    lblGCSE.Text = E + " " + lstGSCE.ValueUnit;
                }

                vVitalSignDt lstGSCM = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 19 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstGSCM != null)
                {
                    string gcsM = lstGSCM.VitalSignValue;
                    string M = gcsM.Substring(0, 1);
                    lblGCSM.Text = M + " " + lstGSCM.ValueUnit;
                }

                vVitalSignDt lstGCSV = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 20 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstGCSV != null)
                {
                    string gcsV = lstGCSV.VitalSignValue;
                    string V = gcsV.Substring(0, 1);
                    lblGCSV.Text = V + " " + lstGCSV.ValueUnit;
                }

                vVitalSignDt lstRR = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 13 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstRR != null)
                {
                    lblRR.Text = lstRR.VitalSignValue + " " + lstRR.ValueUnit;
                }

                vVitalSignDt lstSuhu = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 9 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstSuhu != null)
                {
                    lblSuhu.Text = lstSuhu.VitalSignValue + " " + lstSuhu.ValueUnit;
                }

                vVitalSignDt lstSPO2 = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 27 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstSPO2 != null)
                {
                    lblSPO2.Text = lstSPO2.VitalSignValue + " " + lstSPO2.ValueUnit;
                }

                vVitalSignDt lstBB = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 14 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstBB != null)
                {
                    lblBB.Text = lstBB.VitalSignValue + " " + lstBB.ValueUnit;
                }

                vVitalSignDt lstTB = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 15 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstTB != null)
                {
                    lblTB.Text = lstTB.VitalSignValue + " " + lstTB.ValueUnit;
                }

                vVitalSignDt lstTDs = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 11 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstTDs != null)
                {
                    lblTDs.Text = lstTDs.VitalSignValue + " " + lstTDs.ValueUnit;
                }

                vVitalSignDt lstTDd = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VitalSignID = 12 AND IsDeleted = 0", lstVHD.ID)).FirstOrDefault();
                if (lstTDd != null)
                {
                    lblTDd.Text = lstTDd.VitalSignValue + " " + lstTDd.ValueUnit;
                }
            }
            #endregion

            #region ReviewOfSystemHd
            vReviewOfSystemHd lstRHD = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            if (lstRHD != null)
            {
                vReviewOfSystemDt lstRDT = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0", lstRHD.ID)).FirstOrDefault();

                string status = "";

                vReviewOfSystemDt lstKepala = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.KEPALA)).FirstOrDefault();
                if (lstKepala != null)
                {
                    if (lstKepala.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstKepala.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblHead.Text = status + " " + lstKepala.Remarks;
                }

                vReviewOfSystemDt lstMulut = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.MULUT)).FirstOrDefault();
                if (lstMulut != null)
                {
                    if (lstMulut.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstMulut.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblMulut.Text = status + " " + lstMulut.Remarks;
                }

                vReviewOfSystemDt lstLeher = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.LEHER)).FirstOrDefault();
                if (lstLeher != null)
                {
                    if (lstLeher.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstLeher.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblLeher.Text = status + " " + lstLeher.Remarks;
                }

                vReviewOfSystemDt lstDada = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.DADA_PUNGGUNG)).FirstOrDefault();
                if (lstDada != null)
                {
                    if (lstDada.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstDada.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblDada.Text = status + " " + lstDada.Remarks;
                }

                vReviewOfSystemDt lstAbdomen = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.ABDOMEN)).FirstOrDefault();
                if (lstAbdomen != null)
                {
                    if (lstAbdomen.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstAbdomen.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblAbdomen.Text = status + " " + lstAbdomen.Remarks;
                }

                vReviewOfSystemDt lstPelvis = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.PELVIS)).FirstOrDefault();
                if (lstPelvis != null)
                {
                    if (lstPelvis.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstPelvis.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblPelvis.Text = status + " " + lstPelvis.Remarks;
                }

                vReviewOfSystemDt lstEkstremitas = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.EKSTREMITAS)).FirstOrDefault();
                if (lstEkstremitas != null)
                {
                    if (lstEkstremitas.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstEkstremitas.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblEkstremitas.Text = status + " " + lstEkstremitas.Remarks;
                }

                vReviewOfSystemDt lstNeurologis = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.NEUROLOGIS)).FirstOrDefault();
                if (lstNeurologis != null)
                {
                    if (lstNeurologis.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstNeurologis.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblNeurologis.Text = status + " " + lstNeurologis.Remarks;
                }

                vReviewOfSystemDt lstDermatologikus = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.STATUS_DERMATOLOGIKUS)).FirstOrDefault();
                if (lstDermatologikus != null)
                {
                    if (lstDermatologikus.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstDermatologikus.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblDermatologikus.Text = status + " " + lstDermatologikus.Remarks;
                }

                vReviewOfSystemDt lstVenereologikus = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND GCROSystem = '{1}' AND IsDeleted = 0", lstRHD.ID, Constant.ReviewOfSystem.STATUS_VENEREOLOGIKUS)).FirstOrDefault();
                if (lstVenereologikus != null)
                {
                    if (lstVenereologikus.IsNormal)
                    {
                        status = "Normal";
                    }
                    else if (lstVenereologikus.IsNotExamined)
                    {
                        status = "Not Examined";
                    }
                    else
                    {
                        status = "Abnormal";
                    }

                    lblVenereologikus.Text = status + " " + lstVenereologikus.Remarks;
                }
            }
            #endregion

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
                xrTableCell137.Visible = false;
                subBodyDiagram.Visible = false;
                episodeSummaryBodyDiagramRpt.Visible = false;
            }
            #endregion

            #region Laboratory Order
            List<vTestOrderHd> lstEntityHdTestOrder = BusinessLayer.GetvTestOrderHdList(string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", entityCV.VisitID, laboratoryUnitID, Constant.TransactionStatus.VOID));

            if (lstEntityHdTestOrder.Count() > 0)
            {
                subLabOrder.CanGrow = true;
                episodeSummaryLabOrderRSSKRpt.InitializeReport(entityCV.VisitID);
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
                episodeSummaryImagingOrderRSSKRpt.InitializeReport(entityCV.VisitID);
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

            #region MST
            vMSTAssessment lstMST = BusinessLayer.GetvMSTAssessmentList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
            if (lstMST != null)
            {
                if (lstMST.GCWeightChangedStatus != null)
                {
                    if (lstMST.GCWeightChangedStatus == Constant.MSTWeightChanged.TIDAK_ADA_PENURUNAN_BB)
                    {
                        lblMSTA.Text = "0";
                    }

                    if (lstMST.GCWeightChangedStatus == Constant.MSTWeightChanged.TIDAK_YAKIN)
                    {
                        lblMSTB.Text = "2";
                    }
                }

                if (lstMST.GCWeightChangedGroup != null)
                {
                    if (lstMST.GCWeightChangedGroup == Constant.MSTWeightChangedGroup.SATU_SAMPAI_LIMA_KG)
                    {
                        lblMSTC.Text = "1";
                    }
                    else if (lstMST.GCWeightChangedGroup == Constant.MSTWeightChangedGroup.ENAM_SAMPAI_SEPULUH_KG)
                    {
                        lblMSTD.Text = "2";
                    }
                    else if (lstMST.GCWeightChangedGroup == Constant.MSTWeightChangedGroup.SEBELAS_SAMPAI_LIMABELAS_KG)
                    {
                        lblMSTE.Text = "3";
                    }
                    else if (lstMST.GCWeightChangedGroup == Constant.MSTWeightChangedGroup.LEBIH_DARI_LIMABELAS_KG)
                    {
                        lblMSTF.Text = "4";
                    }
                }

                if (lstMST.IsReducedFoodIntake)
                {
                    lblMSTH.Text = "1";
                }
                else
                {
                    lblMSTG.Text = "0";
                }
            }

            int MSTA = 0;
            int MSTB = 0;
            int MSTC = 0;
            int MSTD = 0;
            int MSTE = 0;
            int MSTF = 0;
            int MSTH = 0;
            int MSTG = 0;

            if (!string.IsNullOrEmpty(lblMSTA.Text))
            {
                MSTA = Convert.ToInt32(lblMSTA.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTB.Text))
            {
                MSTB = Convert.ToInt32(lblMSTB.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTC.Text))
            {
                MSTC = Convert.ToInt32(lblMSTC.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTD.Text))
            {
                MSTD = Convert.ToInt32(lblMSTD.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTE.Text))
            {
                MSTE = Convert.ToInt32(lblMSTE.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTF.Text))
            {
                MSTF = Convert.ToInt32(lblMSTF.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTH.Text))
            {
                MSTH = Convert.ToInt32(lblMSTH.Text);
            }
            if (!string.IsNullOrEmpty(lblMSTG.Text))
            {
                MSTG = Convert.ToInt32(lblMSTG.Text);
            }

            lblTotalScore.Text = (MSTA + MSTB + MSTC + MSTD + MSTE + MSTF + MSTH + MSTG).ToString();
            #endregion

            #region Vital Sign

            List<vVitalSignHd> lstHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsInitialAssessment = 1 AND GCParamedicMasterType != '{1}' ORDER BY ID DESC", entityCV.VisitID, Constant.ParamedicType.Nutritionist));
            if (lstHd.Count > 0)
            {
                vVitalSignHd vitalSignHd = lstHd.FirstOrDefault();
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND VisitID = {1} AND IsDeleted = 0", vitalSignHd.ID, entityCV.VisitID));
                subVitalSign.CanGrow = true;
                episodeSummaryVitalSignRptNew1.InitializeReport(lstVitalSignDt);
            }
            else
            {
                xrLabel13.Visible = false;
                subVitalSign.Visible = false;
            }
            #endregion

            #region Review of System
            List<vReviewOfSystemHd> lstROSHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID));
            if (lstROSHd.Count > 0)
            {
                vReviewOfSystemHd reviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", entityCV.VisitID)).FirstOrDefault();
                List<vReviewOfSystemDt1> lstHdReviewOfSystem = BusinessLayer.GetvReviewOfSystemDt1List(string.Format("ID = {0} AND VisitID = {1} AND IsDeleted = 0", reviewOfSystemHd.ID, entityCV.VisitID));
                subReviewOfSystem.CanGrow = true;
                episodeSummaryReviewOfSystemRptNew1.InitializeReport(lstHdReviewOfSystem);
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
            lblSignDate.Text = String.Format("{0}, {1}", oHealthcare.City, entityCV.ActualVisitDateInString);
            lblSignParamedicName.Text = entityCV.ParamedicName;

            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);

            PageFooter.Visible = reportMaster.IsShowFooter;
            #endregion
        }
    }
}