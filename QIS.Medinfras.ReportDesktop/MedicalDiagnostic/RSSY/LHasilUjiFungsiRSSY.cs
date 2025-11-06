using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Text;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class LHasilUjiFungsiRSSY : BaseDailyPortraitRpt
    {
        public LHasilUjiFungsiRSSY()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string registrationId = param[0];
            StringBuilder sbKesimpulan = new StringBuilder();
            sbKesimpulan.AppendLine("Kesimpulan : ");
            #region Registration and Patient Information
            vRegistration reg = BusinessLayer.GetvRegistrationList(string.Format("{0}", registrationId)).FirstOrDefault();
            ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", reg.RegistrationID)).FirstOrDefault();

            lblMedicalNo.Text = reg.MedicalNo;
            lblPatientName.Text = reg.PatientName;
            lblDOBAndAge.Text = string.Format("{0} / {1}", reg.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT), reg.PatientAgeInYear);
            lblAddress.Text = reg.HomeAddress;
            lblGenderLP.Text = reg.GCGender == Constant.Gender.MALE ? "L" : "P";
            lblRegistrationDate.Text = reg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);

            lblParamedicName.Text = reg.ParamedicName;
            #endregion

            #region Patient Diagnose
            PatientDiagnosis pd = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", cv.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            if (pd != null)
            {
                string diagnose = string.Empty;
                if (!string.IsNullOrEmpty(pd.DiagnoseID))
                {
                    Diagnose diag = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID = '{0}' AND IsDeleted = 0", pd.DiagnoseID)).FirstOrDefault();
                    diagnose = string.Format("{0} - {1}", diag.DiagnoseID, diag.DiagnoseName);
                }
                else if (!string.IsNullOrEmpty(pd.DiagnosisText))
                {
                    diagnose = pd.DiagnosisText;
                }
                lblMedicalDiagnose.Text = diagnose;

                sbKesimpulan.AppendLine("Diagnosa Medis : " + diagnose);
            }
            #endregion

            #region Patient Procedure
            PatientProcedure pp = BusinessLayer.GetPatientProcedureList(string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted = 0", cv.VisitID, cv.ParamedicID)).LastOrDefault();
            string procedure = string.Empty;
            if (pp != null)
            {
                if (!string.IsNullOrEmpty(pp.ProcedureID))
                {
                    Procedures proc = BusinessLayer.GetProceduresList(string.Format("ProcedureID = '{0}' AND IsDeleted = 0", pp.ProcedureID)).FirstOrDefault();
                    procedure = string.Format("{0} - {1}", proc.ProcedureID, proc.ProcedureName);
                }
                else if (!string.IsNullOrEmpty(pp.ProcedureText))
                {
                    procedure = pp.ProcedureText;
                }
                sbKesimpulan.AppendLine("Diagnosa Fungsional : " + procedure);
            }
            #endregion

            #region Review Of System
            string filterExpression = string.Format("VisitID IN ({0}) AND IsDeleted = 0", cv.VisitID);
            StringBuilder sbROS = new StringBuilder();
            vReviewOfSystemHd rosHd = BusinessLayer.GetvReviewOfSystemHdList(filterExpression).LastOrDefault();
            if (rosHd != null)
            {
                sbROS.AppendLine("Hasil Yang Didapat :");
                List<vReviewOfSystemDt> lstRosDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("{0} AND ID = {1} AND IsNormal = 0", filterExpression, rosHd.ID));
                foreach (vReviewOfSystemDt ros in lstRosDt)
                {
                    sbROS.AppendLine(string.Format("{0}: {1}", ros.ROSystem, ros.cfRemarks));
                }
                lblHasilYangDidapat.Text = sbROS.ToString();
            }
            #endregion

            #region Chief Complaint
            vChiefComplaint cc = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", cv.VisitID)).FirstOrDefault();
            if (cc != null)
            {
                lblFunctionalDiagnose.Text = cc.MedicalProblem;
                if (!string.IsNullOrEmpty(cc.MedicalProblem))
                {
                    lblLembarHasilTindakan.Text = string.Format("Lembar Hasil Tindakan Uji Fungsi/Prosedur KFR {0} (Koding ..........)", cc.MedicalProblem);
                }
                else
                {
                    lblLembarHasilTindakan.Text = string.Format("Lembar Hasil Tindakan Uji Fungsi/Prosedur KFR ........... (Koding ..........)");
                }
                StringBuilder sbInstrumenUjiFungsi = new StringBuilder();
                sbInstrumenUjiFungsi.AppendLine("Instrumen Uji Fungsi/Prosedur KFR : ");
                sbInstrumenUjiFungsi.AppendLine(string.Format("{0}", procedure));
                lblInstrumenUjiFungsi.Text = sbInstrumenUjiFungsi.ToString();
                StringBuilder sbRekomendasi = new StringBuilder();
                sbRekomendasi.AppendLine("Rekomendasi : ");
                sbRekomendasi.AppendLine(cc.PlanningSummary);
                lblRekomendasi.Text = sbRekomendasi.ToString();
            }
            lblKesimpulan.Text = sbKesimpulan.ToString();
            #endregion

            #region Footer
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, reg.ParamedicCode);
            ttdDokter.Visible = true;
            #endregion

            base.InitializeReport(param);
        }

    }
}
