using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BPengkajianInstalasiGawatDaruratRSPW : BaseRpt
    {
        public BPengkajianInstalasiGawatDaruratRSPW()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            //Binding
            vHealthcare oHealthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID))[0];
            vConsultVisit oCV = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", param[0]))[0];
            ConsultVisit oVisit = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", param[0]))[0];
            vRegistration oReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", oCV.RegistrationID))[0];
            Patient oPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oCV.MRN))[0];
            ParamedicMaster oPhysician = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", oCV.ParamedicID))[0];
            ChiefComplaint oComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            NurseChiefComplaint oNurseComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", param[0])).FirstOrDefault();
            vHealthcareServiceUnit oServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", oCV.HealthcareServiceUnitID))[0];

            #region Header 1 : Healthcare
            if (oHealthcare != null)
            {
                xrLogo.ImageUrl = ResolveUrl("~/SystemSetup/Libs/Images/logo.png");
                cHealthcareName.Text = oHealthcare.HealthcareName;
                cHealthcareAddress.Text = oHealthcare.StreetName;
                cHealthcareCityZipCodes.Text = string.Format("{0} {1}", oHealthcare.City, oHealthcare.ZipCode);
                cHealthcarePhone.Text = oHealthcare.PhoneNo1;
                cHealthcareFax.Text = oHealthcare.FaxNo1;
            }


            lblReportTitle.Text = reportMaster.ReportTitle1;
            #endregion

            #region Header 2 : Patient Information
            lblNoRM.Text = oPatient.MedicalNo;
            lblNamaPasien.Text = oCV.PatientName;
            lblTglLahir.Text = oCV.DateOfBirthInString;
            lblDPJPUtama.Text = oCV.ParamedicName;
            cHeaderRegistration.Text = oPatient.MedicalNo + " | " + oCV.PatientName;
            #endregion

            #region Detail
            #region Tanggal Masuk - Keluar
            lblTglJamDaftar.Text = oReg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT) + " / " + oReg.RegistrationTime;
            if (oCV.PhysicianDischargedDate == null || oCV.PhysicianDischargedDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
            {
                lblTglJamKeluar.Text = "-";
            }
            else
            {
                if (oCV.DischargeDate == null || oCV.DischargeDate.ToString("dd-MM-yyyy") == Constant.ConstantDate.DEFAULT_NULL)
                {
                    lblTglJamKeluar.Text = oCV.PhysicianDischargedDate.ToString(Constant.FormatString.DATE_FORMAT) + " / " + oCV.PhysicianDischargedDate.ToString(Constant.FormatString.TIME_FORMAT);
                }
                else
                {
                    lblTglJamKeluar.Text = oCV.DischargeDate.ToString(Constant.FormatString.DATE_FORMAT) + " / " + oCV.DischargeTime;
                }
            }
            if (oCV.BedCode == null || oCV.BedCode == "")
                lblKamarBed.Text = "-";
            else
                lblKamarBed.Text = oCV.RoomName + " / " + oCV.BedCode;
            #endregion

            #region Triage
            lblTriage.Text = oReg.Triage;
            #endregion

            #region Keluhan
            if (oComplaint == null)
                lblKeluhan.Text = "";
            else
                lblKeluhan.Text = oComplaint.ChiefComplaintText;
            #endregion

            #region Riwayat Penyakit Dahulu
            if (oComplaint == null)
            {
                if (oNurseComplaint == null)
                {
                    lblRiwayatPenyakit.Text = "";
                }
                else
                {
                    lblRiwayatPenyakit.Text = oNurseComplaint.MedicalHistory;
                }
            }
            else
            {
                lblRiwayatPenyakit.Text = oComplaint.PastMedicalHistory;
            }
            #endregion

            #region Dokter konsul
            List<vParamedicTeamRM> oParamedicTeam = BusinessLayer.GetvParamedicTeamRMList(string.Format("RegistrationID = {0} AND GCParamedicRole = '{1}' AND IsDeleted = 0", oCV.RegistrationID, Constant.ParamedicRole.KONSULEN));
            if (oParamedicTeam.Count != 0)
            {
                subParamedicTeam.CanGrow = true;
                mR2ParamedicTeamRSSBB.InitializeReport(oParamedicTeam);
            }
            #endregion

            #region Diagnosa keluar
            List<vPatientDiagnosis> oPatientDiagUtama = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0 ORDER BY ID DESC", oCV.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS));
            if (oPatientDiagUtama.Count != 0)
            {
                cDiagnosaUtama.Text = oPatientDiagUtama.FirstOrDefault().cfDiagnosisText;
            }

            List<vPatientDiagnosis> oPatientDiagSekunder = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", oCV.VisitID, Constant.DiagnoseType.COMPLICATION));
            if (oPatientDiagSekunder.Count == 0)
                cDiagnosaSekunder.Text = "";
            else
            {
                StringBuilder otherDiagNotes = new StringBuilder();
                foreach (vPatientDiagnosis patientDiagnosis in oPatientDiagSekunder)
                {
                    if (otherDiagNotes.ToString() != "")
                        otherDiagNotes.Append(", ");
                    otherDiagNotes.Append(patientDiagnosis.cfDiagnosisText);
                }
                cDiagnosaSekunder.Text = otherDiagNotes.ToString();
            }

            #endregion

            #region Pemeriksaan Fisik
            List<ReviewOfSystemHd> oROSHd = BusinessLayer.GetReviewOfSystemHdList(string.Format("VisitID = {0} AND IsDeleted = 0", oCV.VisitID));
            if (oROSHd.Count != 0)
            {
                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsNormal = 0 AND IsNotExamined = 0 AND IsDeleted = 0", oROSHd.LastOrDefault().ID));
                if (lstROS.Count != 0)
                {
                    subReviewOfSystem.CanGrow = true;
                    mR2ReviewOfSystemRSSBB.InitializeReport(lstROS);
                }
            }
            #endregion

            #region Tanda - Tanda Vital
            List<VitalSignHd> oVTHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND IsDeleted = 0", oCV.VisitID));
            if (oVTHd.Count != 0)
            {
                List<vVitalSignDt> lstVT = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} AND IsDeleted = 0", oVTHd.LastOrDefault().ID));
                if (lstVT.Count != 0)
                {
                    subVitalSign.CanGrow = true;
                    mR2VitalSignRSSBB.InitializeReport(lstVT);
                }
            }
            #endregion

            #region Terapi

            #region Terapi selama dirawat
            //List<vGetPrescriptionOrderDtRM> oPrescOrderDt = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType = '{1}' AND GCPrescriptionType IN ('{2}', '{3}')", oVisit.VisitID, Constant.ItemType.OBAT_OBATAN, Constant.PrescriptionType.MEDICATION_ORDER, Constant.PrescriptionType.CITO));
            //List<vGetPrescriptionOrderDtRM> oPrescOrderDt = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND ItemID IN (SELECT dt.ItemID FROM vPatientChargesDt10 dt WITH(NOLOCK) WHERE dt.VisitID = VisitID OR dt.ItemID IN (SELECT vis.ItemID FROM vItemService vis WITH(NOLOCK) WHERE vis.GCItemType IN ('{1}', '{2}', '{3}', '{4}') AND vis.IsUsingProcedureCoding = 1))", oVisit.VisitID, Constant.ItemType.PELAYANAN, Constant.ItemType.LABORATORIUM, Constant.ItemType.RADIOLOGI, Constant.ItemType.MEDICAL_CHECKUP));
            List<vGetPrescriptionOrderDtRM> oPrescOrderDt = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType IN ('{1}', '{2}', '{3}', '{4}', '{5}') AND GCPrescriptionType != '{6}'", oVisit.VisitID, Constant.ItemType.PELAYANAN, Constant.ItemType.RADIOLOGI, Constant.ItemType.PENUNJANG_MEDIS, Constant.ItemType.LABORATORIUM, Constant.ItemType.OBAT_OBATAN, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION));
            subPrescription.CanGrow = true;
            mR2PrescriptionRSSBB.InitializeReport(oPrescOrderDt);
            #endregion

            #region Terapi di rumah
            List<vGetPrescriptionOrderDtRM> oPrescOrderDtHome = BusinessLayer.GetvGetPrescriptionOrderDtRMList(string.Format("VisitID = {0} AND GCItemType = '{1}' AND GCPrescriptionType = '{2}'", oVisit.VisitID, Constant.ItemType.OBAT_OBATAN, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION));
            subPrescriptionDischarge.CanGrow = true;
            mR2PrescriptionDischargeRSSBB.InitializeReport(oPrescOrderDtHome);
            #endregion

            #endregion

            #region Kondisi Keluar
            lblDischargeCondition.Text = oCV.DischargeCondition;
            //if (oCV.GCDischargeCondition == Constant.PatientOutcome.SEHAT_ATAU_NORMAL)
            //{
            //    cbSehat.Checked = true;
            //}
            //else if (oCV.GCDischargeCondition == Constant.PatientOutcome.MEMBAIK)
            //{
            //    cbMembaik.Checked = true;
            //}
            //else if (oCV.GCDischargeCondition == Constant.PatientOutcome.BELUM_SEMBUH)
            //{
            //    cbBelumSembuh.Checked = true;
            //}
            //else if (oCV.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48)
            //{
            //    cbMeninggalSebelum48.Checked = true;
            //}
            //else if (oCV.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            //{
            //    cbMeninggalSesudah48.Checked = true;
            //}
            #endregion

            #region Cara Keluar
            lblDischargeMethod.Text = oCV.DischargeMethod;
            //if (oCV.GCDischargeMethod == Constant.DischargeMethod.ATAS_PERSETUJUAN)
            //{
            //    cbAtasPersetujuan.Checked = true;
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
            //{
            //    cbTfRajal.Checked = true;
            //    if (oVisit.ReferralUnitID != null && oVisit.ReferralPhysicianID != null)
            //    {
            //        List<vHealthcareServiceUnit> oServiceUnitReferral = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", oVisit.ReferralUnitID));
            //        List<ParamedicMaster> oParamedicReferral = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", oVisit.ReferralPhysicianID));
            //        cbTfRajal.Checked = true;
            //        if (oServiceUnitReferral.Count != 0)
            //        {
            //            lblTfRajal.Text = string.Format("Transfer Rawat Jalan : {0} - {1} | Kontrol kembali tanggal : {2}", oServiceUnitReferral.FirstOrDefault().ServiceUnitName, oParamedicReferral.FirstOrDefault().FullName, oVisit.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            //        }
            //    }
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.TRANSFERED_TO_UPH)
            //{
            //    cbTransferUPH.Checked = true;
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.TRANSFERED_TO_ODS)
            //{
            //    cbTransferODS.Checked = true;
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD)
            //{
            //    cbTransferRanap.Checked = true;
            //    if (oVisit.ReferralPhysicianID != null)
            //    {
            //        List<ParamedicMaster> oParamedicReferral = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", oVisit.ReferralPhysicianID));
            //        lblTransferRanap.Text = string.Format("Transfer Rawat Inap : {0}", oParamedicReferral.FirstOrDefault().FullName);
            //    }
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.GO_HOME_BY_OWN_REQUEST)
            //{
            //    cbPulangAPS.Checked = true;
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.RUNAWAY)
            //{
            //    cbPulangTanpaIjin.Checked = true;
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
            //{
            //    if (oVisit.ReferralTo != null)
            //    {
            //        BusinessPartners oReferralTo = BusinessLayer.GetBusinessPartnersList(string.Format("BusinessPartnerID = {0}", oVisit.ReferralTo))[0];
            //        cbPindahRS.Checked = true;
            //        lblPindahRS.Text = string.Format("Dirujuk : {0}", oReferralTo.BusinessPartnerName);
            //    }
            //}
            //else if (oCV.GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_MORTUARY)
            //{
            //    cbDikirimKeKamarJenazah.Checked = true;
            //}
            #endregion

            #region Footer
            lblTanggalTTD.Text = string.Format("{0}, {1} Jam {2}", oHealthcare.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT));
            lblDPJP.Text = "( " + oPhysician.FullName + " )";

            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, oPhysician.ParamedicCode);
            ttdDokter.Visible = true;
            #endregion

            #endregion

            #region Footer
            string filterExpressionResumeMedis = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RESUME_MEDIS_TELEPHONE);
            string filterExpressionIGD = string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IGD_TELEPHONE_ON_RESUME_MEDIS);
            SettingParameterDt ResumeMedisPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionResumeMedis).FirstOrDefault();
            SettingParameterDt IGDPhone = BusinessLayer.GetSettingParameterDtList(filterExpressionIGD).FirstOrDefault();

            cResumeMedisPhone.Text = string.Format("Bila ada perubahan nomor telepon dan/handphone. Mohon segera menginformasikan ke nomor {0}", ResumeMedisPhone.ParameterValue);
            cIGDPhone.Text = string.Format("Dalam keadaan darurat, hubungi IGD {0} ke nomor {1}", oHealthcare.HealthcareName, IGDPhone.ParameterValue);
            #endregion

            base.InitializeReport(param);
        }

    }
}
