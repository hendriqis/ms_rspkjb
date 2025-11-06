using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class MRDischargeRSUKI : DevExpress.XtraReports.UI.XtraReport
    {
        public MRDischargeRSUKI()
        {
            InitializeComponent();
        }

        public void InitializeReport(int VisitID, int MedicalResumeID)
        {
            vConsultVisit1 entityCV = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID IN ({0})", VisitID)).FirstOrDefault();
            vMedicalResume entityMR = BusinessLayer.GetvMedicalResumeList(string.Format("VisitID IN ({0}) AND IsDeleted = 0", VisitID)).FirstOrDefault();

            if (entityMR != null)
            {
                if (entityMR.DischargeMedicalResumeText != null && entityMR.DischargeMedicalResumeText != "")
                {
                    xrLabel2.Text = string.Format("Kondisi Klinis / Catatan saat Pulang");
                    xrLabel1.Text = string.Format(":");
                    lblDischargeMedical.Text = string.Format("{0}", entityMR.DischargeMedicalResumeText);
                }
                else
                {
                    xrLabel2.Visible = false;
                    xrLabel1.Visible = false;
                    lblDischargeMedical.Visible = false;
                }
            }
            else
            {
                xrLabel2.Visible = false;
                xrLabel1.Visible = false;
                lblDischargeMedical.Visible = false;
            }

            if (entityCV.GCDischargeCondition != null && entityCV.GCDischargeCondition != "")
            {
                if (entityCV.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entityCV.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
                {
                    if (entityCV.GCDischargeMethod != Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                    {
                        lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition",
                                    Environment.NewLine,
                                    "Tanggal dan Jam Meninggal");
                        lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                    ":",
                                    Environment.NewLine,
                                    ":",
                                    Environment.NewLine,
                                    ":",
                                    Environment.NewLine,
                                    ":");
                        lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}",
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition,
                                    Environment.NewLine,
                                    entityCV.DateTimeOfDeathInString);
                    }
                    else
                    {
                        if (entityCV.GCReferralDischargeReason != Constant.ReferralDischargeReason.LAINNYA)
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition",
                                    Environment.NewLine,
                                    "Tanggal dan Jam Meninggal",
                                    Environment.NewLine,
                                    "Rujuk Ke",
                                    Environment.NewLine,
                                    "Rumah Sakit / Faskes",
                                    Environment.NewLine,
                                    "Alasan Pasien Dirujuk");
                            lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
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
                                    ":",
                                    Environment.NewLine,
                                    ":");
                            lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition,
                                    Environment.NewLine,
                                    entityCV.DateTimeOfDeathInString,
                                    Environment.NewLine,
                                    entityCV.ReferrerGroup,
                                    Environment.NewLine,
                                    string.Format("{0} - {1}", entityCV.ReferrerCode, entityCV.ReferrerName),
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReason);
                        }
                        else
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition",
                                    Environment.NewLine,
                                    "Tanggal dan Jam Meninggal",
                                    Environment.NewLine,
                                    "Rujuk Ke",
                                    Environment.NewLine,
                                    "Rumah Sakit / Faskes",
                                    Environment.NewLine,
                                    "Alasan Pasien Dirujuk");
                            lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}",
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
                                    ":",
                                    Environment.NewLine,
                                    ":");
                            lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}{11}{12}{13}{14}",
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition,
                                    Environment.NewLine,
                                    entityCV.DateTimeOfDeathInString,
                                    Environment.NewLine,
                                    entityCV.ReferrerGroup,
                                    Environment.NewLine,
                                    string.Format("{0} - {1}", entityCV.ReferrerCode, entityCV.ReferrerName),
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReason,
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReasonOther);
                        }
                    }
                }
                else
                {
                    if (entityCV.GCDischargeMethod != Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                    {
                        lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition");
                        lblLabel2.Text = string.Format("{0}{1}{2}{3}{4}",
                                    ":",
                                    Environment.NewLine,
                                    ":",
                                    Environment.NewLine,
                                    ":");
                        lblDischarge.Text = string.Format("{0}{1}{2}{3}{4}",
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition);
                    }
                    else
                    {
                        if (entityCV.GCReferralDischargeReason != Constant.ReferralDischargeReason.LAINNYA)
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition",
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
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition,
                                    Environment.NewLine,
                                    entityCV.ReferrerGroup,
                                    Environment.NewLine,
                                    string.Format("{0} - {1}", entityCV.ReferrerCode, entityCV.ReferrerName),
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReason);
                        }
                        else
                        {
                            lblLabel1.Text = string.Format("{0}{1}{2}{3}{4}{5}{6}{7}{8}{9}{10}",
                                    "Discharge Date",
                                    Environment.NewLine,
                                    "Discharge Method",
                                    Environment.NewLine,
                                    "Discharge Condition",
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
                                    entityCV.cfDischargeDateInString2,
                                    Environment.NewLine,
                                    entityCV.DischargeMethod,
                                    Environment.NewLine,
                                    entityCV.DischargeCondition,
                                    Environment.NewLine,
                                    entityCV.ReferrerGroup,
                                    Environment.NewLine,
                                    string.Format("{0} - {1}", entityCV.ReferrerCode, entityCV.ReferrerName),
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReason,
                                    Environment.NewLine,
                                    entityCV.ReferralDischargeReasonOther);
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
        }
    }
}
