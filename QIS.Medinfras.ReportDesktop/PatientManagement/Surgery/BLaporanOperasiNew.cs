using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.IO;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using ThoughtWorks.QRCode.Codec;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BLaporanOperasiNew : BaseRpt
    {
        public BLaporanOperasiNew()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            #region Header 1 : Healthcare
            vConsultVisit entityCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            Registration entityReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", entityCV.RegistrationID)).FirstOrDefault();
            vPatientSurgery entity = BusinessLayer.GetvPatientSurgeryList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0", param)).FirstOrDefault();
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
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeInString);
                }
                else
                {
                    cDischargeDateTime.Text = string.Format("Tgl & Jam Keluar* : {0}", entityCV.cfPhysicianDischargedDateTimeInString);
                }
            }

            vPatientTransferResumeMedis entityPTR = BusinessLayer.GetvPatientTransferResumeMedisList(string.Format("RegistrationID = {0}", entityReg.RegistrationID)).FirstOrDefault();
            if (entityPTR != null)
            {
                cUnitVisit.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.FromHSU);
                cUnitDischarge.Text = string.Format("Kamar/Bagian     : {0}", entityPTR.ToHSU);
            }
            else
            {
                cUnitVisit.Text = "Kamar/Bagian   :";
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

            #region Detail
            if (entity != null)
            {
                vPatientSurgery entityPs = BusinessLayer.GetvPatientSurgeryList(string.Format("VisitID = {0} AND PatientSurgeryID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, param[0])).FirstOrDefault();
                string procedureGroupRemarks = string.Empty;

                if (entityPs.StartTime != null)
                {
                    cStartTime.Text = entityPs.StartTime;
                }
                else
                {
                    cStartTime.Text = "";
                }
                if (entityPs.EndTime != null)
                {
                    cEndTime.Text = entityPs.EndTime;
                }
                else
                {
                    cEndTime.Text = "";
                }
                if (entityPs.PreOperativeDiagnosisText != null)
                {
                    cPreDiagnosis.Text = string.Format("{0}", entityPs.PreOperativeDiagnosisText);
                }
                else
                {
                    cPreDiagnosis.Text = "";
                }
                if (entityPs.PostOperativeDiagnosisText != null)
                {
                    cPostDiagnosis.Text = string.Format("{0}", entityPs.PostOperativeDiagnosisText);
                }
                else
                {
                    cPostDiagnosis.Text = "";
                }

                if (!string.IsNullOrEmpty(entityPs.ProcedureGroupRemarks))
                {
                    procedureGroupRemarks = entityPs.ProcedureGroupRemarks;
                }

                vPatientSurgeryTeam entityPst = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.OPERATOR)).FirstOrDefault();
                vPatientSurgeryTeam entityPst2 = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.ANESTESI)).FirstOrDefault();
                vPatientSurgeryTeam entityPst3 = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.ASISTEN_DOKTER)).FirstOrDefault();
                vPatientSurgeryTeam entityPst4 = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.ASISTEN_PERAWAT)).FirstOrDefault();
                vPatientSurgeryTeam entityPst5 = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.PERAWAT_INSTRUMEN)).FirstOrDefault();
                vPatientSurgeryTeam entityPst6 = BusinessLayer.GetvPatientSurgeryTeamList(string.Format("TestOrderID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", entityPs.TestOrderID, Constant.SurgeryTeamRole.PERAWAT_ON_LOOP)).FirstOrDefault();

                if (entityPst != null)
                {
                    cDokterOperator.Text = entityPst.ParamedicName;
                }
                else
                {
                    cDokterOperator.Text = "";
                }

                if (entityPst2 != null)
                {
                    cDokterAnestesi.Text = entityPst2.ParamedicName;
                }
                else
                {
                    cDokterAnestesi.Text = "";
                }

                if (entityPst3 != null)
                {
                    cAsistenDokter.Text = entityPst3.ParamedicName;
                }
                else
                {
                    cAsistenDokter.Text = "";
                }

                if (entityPst4 != null)
                {
                    cAsistenPerawat.Text = entityPst4.ParamedicName;
                }
                else
                {
                    cAsistenPerawat.Text = "";
                }

                if (entityPst5 != null)
                {
                    cPerawatInstrumen.Text = entityPst5.ParamedicName;
                }
                else
                {
                    cPerawatInstrumen.Text = "";
                }

                if (entityPst6 != null)
                {
                    cCirculatingNurse.Text = entityPst6.ParamedicName;
                }
                else
                {
                    cCirculatingNurse.Text = "";
                }

                //vPatientSurgeryProcedureGroup entityPspg = BusinessLayer.GetvPatientSurgeryProcedureGroupList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0 ORDER BY ID DESC", entity.PatientSurgeryID)).FirstOrDefault();
                //if (entityPspg != null)
                //{
                //    cOperasi.Text = string.Format("{0} - {1}", entityPspg.ProcedureGroupName, entityPspg.SurgeryClassification);
                //}
                //else
                //{
                //    cOperasi.Text = "";
                //}

                List<vPatientSurgeryProcedureGroup> lstEntityPspg = BusinessLayer.GetvPatientSurgeryProcedureGroupList(string.Format("PatientSurgeryID = {0} AND IsDeleted = 0 ORDER BY ID DESC", entity.PatientSurgeryID));
                if (lstEntityPspg.Count > 0)
                {
                    StringBuilder sbProcedure = new StringBuilder();
                    foreach (vPatientSurgeryProcedureGroup patientprocedure in lstEntityPspg)
                    {
                        if (string.IsNullOrEmpty(patientprocedure.GCSurgeryClassification))
                        {
                            sbProcedure.AppendLine(string.Format("{0}", patientprocedure.ProcedureGroupName));
                        }
                        else
                        {
                            sbProcedure.AppendLine(string.Format("{0} - {1}", patientprocedure.ProcedureGroupName, patientprocedure.SurgeryClassification));
                        }
                    }

                    if (!string.IsNullOrEmpty(procedureGroupRemarks))
                    {
                        sbProcedure.AppendLine("Catatan Tindakan Operasi :");
                        sbProcedure.AppendLine(procedureGroupRemarks);
                    }

                    cOperasi.Text = sbProcedure.ToString();
                }

                if (entityPs.IsHasHemorrhage)
                {
                    chk1.Checked = true;
                    cPerdarahan.Text = string.Format("{0} ml", entityPs.Hemorrhage);
                }
                else
                {
                    chk1.Checked = false;
                    cPerdarahan.Text = "";
                }

                if (entityPs.IsBloodDrain)
                {
                    chk2.Checked = true;
                    cDrain.Text = entityPs.OtherBloodDrainType;
                }
                else
                {
                    chk2.Checked = false;
                    cDrain.Text = "";
                }

                if (entityPs.IsUsingTampon)
                {
                    chk3.Checked = true;
                    cTampon.Text = entityPs.TamponType;
                }
                else
                {
                    chk3.Checked = false;
                    cTampon.Text = "";
                }

                if (entityPs.IsTestPA)
                {
                    chk4.Checked = true;
                }
                else
                {
                    chk4.Checked = false;
                }

                if (entityPs.IsTestKultur)
                {
                    chk5.Checked = true;
                }
                else
                {
                    chk5.Checked = false;
                }

                if (entityPs.IsTestCytology)
                {
                    chk6.Checked = true;
                }
                else
                {
                    chk6.Checked = false;
                }

                if (entityPs.PostOperativeDiagnosisRemarks != null)
                {
                    cUraianPembedahan.Text = string.Format("{0}", entityPs.PostOperativeDiagnosisRemarks);
                }
                else
                {
                    cUraianPembedahan.Text = "";
                }

                #region Footer
                if (entityPs != null)
                {
                    lblTTDTanggal.Text = entityPs.cfReportDate;
                }
                else
                {
                    lblTTDTanggal.Text = "";
                }

                lblSignParamedicName.Text = entityPs.ParamedicName;
                ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, entityPs.ParamedicCode);
                ttdDokter.Visible = true;
            }
            #endregion
            lblReportProperties.Text = string.Format("MEDINFRAS - {0}, Print Date/Time:{1}, User ID:{2}", reportMaster.ReportCode, DateTime.Now.ToString("dd-MMM-yyyy/HH:mm:ss"), appSession.UserName);
            #endregion
        }
    }
}
