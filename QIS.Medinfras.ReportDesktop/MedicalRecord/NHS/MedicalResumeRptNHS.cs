using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MedicalResumeRptNHS : BaseCustomDailyPotraitRpt
    {
        public MedicalResumeRptNHS()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            vConsultVisit1 cv = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", param[0])).FirstOrDefault();
            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0 AND IsRevised = 0", param[0])).FirstOrDefault();
            vHealthcare healthcare = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", appSession.HealthcareID)).FirstOrDefault();

            lblPatientName.Text = cv.PatientName;
            lblDOB.Text = cv.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
            lblPatientAge.Text = cv.PatientAge;
            lblMedicalNo.Text = cv.MedicalNo;

            if (obj != null)
            {
                lblSubjectiveSummaryText.Text = obj.SubjectiveResumeText;

                #region Pemeriksaan Fisik Saat Masuk
                StringBuilder sbNotes;
                sbNotes = new StringBuilder();
                Registration entityRegistration = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = {0}", cv.RegistrationID)).FirstOrDefault();
                string filterExpression = "";
                if (entityRegistration.LinkedRegistrationID != null)
                {
                    ConsultVisit entityLinkedentityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.LinkedRegistrationID)).FirstOrDefault();
                    filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0", entityLinkedentityVisit.VisitID);
                }
                else
                {
                    ConsultVisit entityVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", entityRegistration.RegistrationID)).FirstOrDefault();
                    filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID IS NULL AND IsDeleted = 0", entityVisit.VisitID);
                }
                vVitalSignHd vitalSignHd = BusinessLayer.GetvVitalSignHdList(filterExpression).FirstOrDefault();
                if (vitalSignHd != null)
                {
                    List<vVitalSignDt> lstVitalSign = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} ORDER BY DisplayOrder", vitalSignHd.ID));

                    if (lstVitalSign.Count > 0)
                    {
                        foreach (vVitalSignDt vitalSign in lstVitalSign)
                        {
                            sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                        }
                    }
                }

                sbNotes.AppendLine(" ");

                vReviewOfSystemHd reviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).FirstOrDefault();
                if (reviewOfSystemHd != null)
                {
                    List<vReviewOfSystemDt> lstReviewOfSystem = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", reviewOfSystemHd.ID));

                    if (lstReviewOfSystem.Count > 0)
                    {
                        foreach (vReviewOfSystemDt item in lstReviewOfSystem)
                        {
                            sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                        }
                    }
                }

                lblObjectiveResumeText.Text = sbNotes.ToString();
                #endregion

                lblObjectiveSummaryText.Text = obj.ObjectiveResumeText;
                lblAssessmentSummaryText.Text = obj.AssessmentResumeText;
                lblPlanningResumeText.Text = obj.PlanningResumeText;
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

                #region Kondisi dan Cara Pulang
                if (cv.GCDischargeCondition != null && cv.GCDischargeCondition != "")
                {
                    if (cv.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || cv.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
                    {
                        if (cv.GCDischargeMethod != Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar",
                                        Environment.NewLine,
                                        "Tanggal dan Jam Meninggal");
                            lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}",
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":");
                            lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod,
                                        Environment.NewLine,
                                        cv.DateTimeOfDeathInString);
                        }
                        else
                        {
                            if (cv.GCReferralDischargeReason != Constant.ReferralDischargeReason.LAINNYA)
                            {
                                lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar",
                                        Environment.NewLine,
                                        "Tanggal dan Jam Meninggal",
                                        Environment.NewLine,
                                        "Rujuk Ke",
                                        Environment.NewLine,
                                        "Rumah Sakit / Faskes",
                                        Environment.NewLine,
                                        "Alasan Pasien Dirujuk");
                                lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":");
                                lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod,
                                        Environment.NewLine,
                                        cv.DateTimeOfDeathInString,
                                        Environment.NewLine,
                                        cv.ReferrerGroup,
                                        Environment.NewLine,
                                        string.Format("{0} - {1}", cv.ReferrerCode, cv.ReferrerName),
                                        Environment.NewLine,
                                        cv.ReferralDischargeReason);
                            }
                            else
                            {
                                lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar",
                                        Environment.NewLine,
                                        "Tanggal dan Jam Meninggal",
                                        Environment.NewLine,
                                        "Rujuk Ke",
                                        Environment.NewLine,
                                        "Rumah Sakit / Faskes",
                                        Environment.NewLine,
                                        "Alasan Pasien Dirujuk");
                                lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":");
                                lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod,
                                        Environment.NewLine,
                                        cv.DateTimeOfDeathInString,
                                        Environment.NewLine,
                                        cv.ReferrerGroup,
                                        Environment.NewLine,
                                        string.Format("{0} - {1}", cv.ReferrerCode, cv.ReferrerName),
                                        Environment.NewLine,
                                        cv.ReferralDischargeReason,
                                        Environment.NewLine,
                                        cv.ReferralDischargeReasonOther);
                            }
                        }
                    }
                    else
                    {
                        if (cv.GCDischargeMethod != Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar");
                            lblLabel2.Text = string.Format("{0}{1}{2}",
                                        ":",
                                        Environment.NewLine,
                                        ":");
                            lblDischarge.Text = string.Format("{0}{1}{2}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod);
                        }
                        else
                        {
                            if (cv.GCReferralDischargeReason != Constant.ReferralDischargeReason.LAINNYA)
                            {
                                lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar",
                                        Environment.NewLine,
                                        "Rujuk Ke",
                                        Environment.NewLine,
                                        "Rumah Sakit / Faskes",
                                        Environment.NewLine,
                                        "Alasan Pasien Dirujuk");
                                lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":");
                                lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod,
                                        Environment.NewLine,
                                        cv.ReferrerGroup,
                                        Environment.NewLine,
                                        string.Format("{0} - {1}", cv.ReferrerCode, cv.ReferrerName),
                                        Environment.NewLine,
                                        cv.ReferralDischargeReason);
                            }
                            else
                            {
                                lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        "Keadaan Keluar",
                                        Environment.NewLine,
                                        "Cara Keluar",
                                        Environment.NewLine,
                                        "Rujuk Ke",
                                        Environment.NewLine,
                                        "Rumah Sakit / Faskes",
                                        Environment.NewLine,
                                        "Alasan Pasien Dirujuk");
                                lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}",
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":",
                                        Environment.NewLine,
                                        ":");
                                lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                        cv.DischargeCondition,
                                        Environment.NewLine,
                                        cv.DischargeMethod,
                                        Environment.NewLine,
                                        cv.ReferrerGroup,
                                        Environment.NewLine,
                                        string.Format("{0} - {1}", cv.ReferrerCode, cv.ReferrerName),
                                        Environment.NewLine,
                                        cv.ReferralDischargeReason,
                                        Environment.NewLine,
                                        cv.ReferralDischargeReasonOther);
                            }
                        }
                    }
                }
                else
                {
                    lblLabel1.Visible = false;
                    lblLabel2.Visible = false;
                    lblDischarge.Visible = false;
                }
                #endregion

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
                lblPlanningResumeText.Text = "";
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
