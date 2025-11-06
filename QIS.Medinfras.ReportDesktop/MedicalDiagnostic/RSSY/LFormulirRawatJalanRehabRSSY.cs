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
    public partial class LFormulirRawatJalanRehabRSSY : BaseDailyPortraitRpt
    {
        public LFormulirRawatJalanRehabRSSY()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            string registrationId = param[0];
            StringBuilder sbKesimpulan = new StringBuilder();
            #region Registration and Patient Information
            vRegistration reg = BusinessLayer.GetvRegistrationList(string.Format("{0}", registrationId)).FirstOrDefault();
            ConsultVisit cv = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0}", reg.RegistrationID)).FirstOrDefault();

            lblMedicalNo.Text = reg.MedicalNo;
            lblPatientName.Text = reg.PatientName;
            lblDOBAndAge.Text = string.Format("{0}", reg.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT), reg.PatientAgeInYear);
            lblAddress.Text = reg.HomeAddress;
            lblPhoneNo.Text = reg.MobilePhoneNo1;
            lblRegistrationDate.Text = reg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);
            lblParamedicName.Text = string.Format("({0})", reg.ParamedicName);
            lblRegistrationDate.Text = reg.RegistrationDate.ToString(Constant.FormatString.DATE_FORMAT);
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
                lblDiagnose.Text = diagnose;

                sbKesimpulan.AppendLine(diagnose);
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
                lblMedicalProblem.Text = procedure;
            }
            #endregion

            #region Chief Complaint
            vChiefComplaint cc = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", cv.VisitID)).FirstOrDefault();
            if (cc != null)
            {
                lblChiefComplaint.Text = cc.ChiefComplaintText;
                //lblProcedure.Text = cc.PlanningSummary;

                PatientVisitNote entityVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", cv.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES)).FirstOrDefault();
                if (entityVisitNote != null)
                {
                    lblEvaluasi.Text = entityVisitNote.InstructionText;
                }
            }
            #endregion

            #region Test Order
            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.MD_SERVICE_UNIT_FISIOTHERAPY)).FirstOrDefault();
            List<TestOrderHd> orderHd = BusinessLayer.GetTestOrderHdList(string.Format("IsMultiVisitScheduleOrder = 1 AND HealthcareServiceUnitID IN ('{0}') AND GCTransactionStatus NOT IN ('{1}') AND VisitID = {2}", setvarDt.ParameterValue, Constant.TransactionStatus.VOID, cv.VisitID));
            if (orderHd.Count > 0)
            {
                string lstID = string.Empty;
                foreach (TestOrderHd hd in orderHd) 
                {
                    lstID += string.Format("{0},", hd.TestOrderID);
                }
                List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("IsDeleted = 0 AND TestOrderID IN ({0})", lstID.Remove(lstID.Length - 1, 1))).GroupBy(g => g.ItemID).Select(s => s.FirstOrDefault()).ToList();
                StringBuilder lstItem = new StringBuilder();
                int i = 0;
                foreach (vTestOrderDt dt in lstOrderDt)
                {
                    if (i == 0)
                    {
                        //lstItem.AppendLine(string.Format("Tata Laksana KFR                         : {0} ", dt.ItemName1));
                        lstItem.AppendLine(String.Format("{0}{1}", "Tata Laksana KFR".PadRight(40), " : " + dt.ItemName1));
                    }
                    else
                    {
                        //lstItem.AppendLine(string.Format("                                                    {0} ", dt.ItemName1));
                        lstItem.AppendLine(String.Format("{0}{1}", string.Empty.PadRight(52), dt.ItemName1));
                    }
                    i += 1;
                }
                lblProcedure.Text = lstItem.ToString();
            }
            #endregion

            #region Footer
            ttdDokter.ImageUrl = string.Format("{0}{1}/Signature/{2}.png", AppConfigManager.QISPhysicalDirectory, AppConfigManager.QISParamedicImagePath, reg.ParamedicCode);
            ttdDokter.Visible = true;
            #endregion

            #region Healthcare
            vHealthcare h = BusinessLayer.GetvHealthcareList(string.Format("HealthcareID = '{0}'", AppSession.UserLogin.HealthcareID)).FirstOrDefault();
            if (h != null)
            {
                lblHealthcareCityAndDate.Text = string.Format("{0}, {1}", h.City, DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT));
            }
            #endregion

            base.InitializeReport(param);
        }

    }
}
