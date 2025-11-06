using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.XtraReports.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.ReportDesktop
{
    public partial class BFormulirRawatJalan : BaseCustomDailyPotraitRpt
    {
        public BFormulirRawatJalan()
        {
            InitializeComponent();
        }

        public override void InitializeReport(string[] param)
        {
            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(param[0]));

            string filterCf = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
            ChiefComplaint cf = BusinessLayer.GetChiefComplaintList(filterCf).FirstOrDefault();
            if (cf != null)
            {
                lblAnamnesa.Text = cf.ChiefComplaintText;
            }

            string filterDiagnose = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
            List<PatientDiagnosis> lstDiagnose = BusinessLayer.GetPatientDiagnosisList(filterDiagnose);

            PatientDiagnosis mainDiagnose = lstDiagnose.Where(t => t.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).FirstOrDefault();
            if (mainDiagnose != null)
            {
                lblMainDiagnose.Text = string.Format("{0} ({1})", mainDiagnose.DiagnosisText, mainDiagnose.DiagnoseID);
            }

            PatientDiagnosis secondDiagnose = lstDiagnose.Where(t => t.GCDiagnoseType == Constant.DiagnoseType.COMPLICATION).FirstOrDefault();
            if (secondDiagnose != null)
            {
                lblSecondaryDiagnose.Text = string.Format("{0} ({1})", secondDiagnose.DiagnosisText, secondDiagnose.DiagnoseID);
            }

            string filterProcedures = string.Format("VisitID = {0} AND IsDeleted = 0", visit.VisitID);
            PatientProcedure procedures = BusinessLayer.GetPatientProcedureList(filterProcedures).FirstOrDefault();
            if (procedures != null)
            {
                lblProcedures.Text = string.Format("{0} ({1})", procedures.ProcedureText, procedures.ProcedureID);
            }

            string filterPsv = string.Format("VisitID = {0} AND IsDeleted = 0 AND GCPatientNoteType = '{1}'", visit.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES);
            PatientVisitNote psv = BusinessLayer.GetPatientVisitNoteList(filterPsv).FirstOrDefault();
            if (psv != null)
            {
                lblEvaluation.Text = psv.InstructionText;
            }

            base.InitializeReport(param);
        }
    }
}
