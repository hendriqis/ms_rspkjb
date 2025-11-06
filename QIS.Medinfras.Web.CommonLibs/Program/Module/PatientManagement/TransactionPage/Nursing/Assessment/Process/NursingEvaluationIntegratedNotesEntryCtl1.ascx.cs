using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;
using System.Globalization;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingEvaluationIntegratedNotesEntryCtl1 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = param.Split('|')[1];
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            if (hdnVisitID.Value != "0" && hdnVisitID.Value != "")
            {
                IsAdd = true;
                OnControlEntrySettingLocal();
                ReInitControl();
                List<vNursingTransactionHd> lstEntity = BusinessLayer.GetvNursingTransactionHdList(string.Format("VisitID = {0} AND GCTransactionStatus = '{1}'",Convert.ToInt32(hdnVisitID.Value), Constant.TransactionStatus.OPEN));
                EntityToControl(lstEntity);
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType NOT IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType))
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }
        }

        private void EntityToControl(List<vNursingTransactionHd> lstEntity)
        {
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            string subjectiveText = GetSubjectiveText(lstEntity);
            string objectiveText = GetObjectiveText(lstEntity);
            string assessmentText = GetAssessmentText(lstEntity);
            string planningText = GetPlanning1Text(lstEntity);
            string instructionText = GetInstructionText(lstEntity);

            txtSubjectiveText.Text = !string.IsNullOrEmpty(subjectiveText) ? subjectiveText : string.Empty;
            txtObjectiveText.Text = !string.IsNullOrEmpty(objectiveText) ? objectiveText : string.Empty;
            txtAssessmentText.Text = assessmentText;
            txtPlanningText.Text = planningText;
            txtInstructionText.Text = instructionText;
        }

        private string GetSubjectiveText(List<vNursingTransactionHd> lstEntity)
        {
            StringBuilder text = new StringBuilder();

            foreach (vNursingTransactionHd hdr in lstEntity)
            {
                string filterExpression = string.Format("TransactionID = {0} AND GCNursingEvaluation = '{1}'", hdr.TransactionID, Constant.NursingEvaluation.SUBJECTIVE);
                List<vNursingTransactionDt> lstItem = BusinessLayer.GetvNursingTransactionDtList(filterExpression);
                if (lstItem.Count > 0)
                {
                    foreach (vNursingTransactionDt item in lstItem)
                    {
                        if (!string.IsNullOrEmpty(item.NursingItemText))
                        {
                            text.AppendLine(string.Format("- {0}", item.NursingItemText));
                        }
                    }
                }                
            }


            return text.ToString();
        }

        private string GetObjectiveText(List<vNursingTransactionHd> lstEntity)
        {
            StringBuilder text = new StringBuilder();

            foreach (vNursingTransactionHd hdr in lstEntity)
            {
                string filterExpression = string.Format("TransactionID = {0} AND GCNursingEvaluation = '{1}'", hdr.TransactionID, Constant.NursingEvaluation.OBJECTIVE);
                List<vNursingTransactionDt> lstItem = BusinessLayer.GetvNursingTransactionDtList(filterExpression);
                if (lstItem.Count > 0)
                {
                    foreach (vNursingTransactionDt item in lstItem)
                    {
                        if (!string.IsNullOrEmpty(item.NursingItemText))
                        {
                            text.AppendLine(string.Format("- {0}", item.NursingItemText));
                        }
                    }
                }                
            }

            return text.ToString();
        }

        private string GetAssessmentText(List<vNursingTransactionHd> lstEntity)
        {
            StringBuilder resultText = new StringBuilder();

            foreach (vNursingTransactionHd item in lstEntity)
            {
                resultText.AppendLine(string.Format("{0}", item.ProblemName)); 
            }

            return resultText.ToString();
        }

        private string GetPlanningText(int transactionID)
        {
            StringBuilder planningText = new StringBuilder();

            string filterExpression = string.Format("TransactionID = {0}", transactionID);
            List<vNursingTransactionInterventionHd> lstIntervention = BusinessLayer.GetvNursingTransactionInterventionHdList(filterExpression);
            foreach (vNursingTransactionInterventionHd intervention in lstIntervention)
            {
                filterExpression = string.Format("TransactionID = {0} AND NursingInterventionID = {1} ORDER BY DisplayOrder", transactionID, intervention.NursingInterventionID);
                List<vNursingTransactionInterventionDt> lstEntity = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);

                if (lstEntity.Count>0)
                {
                    planningText.AppendLine(string.Format("{0} ({1})", intervention.NurseInterventionName, intervention.NurseInterventionCode));

                    foreach (vNursingTransactionInterventionDt item in lstEntity)
                    {
                        if (!string.IsNullOrEmpty(item.NursingItemText))
                        {
                            planningText.AppendLine(string.Format("- {0} {1}",item.DisplayOrder, item.NursingItemText)); 
                        }
                    } 
                }
            }

            return planningText.ToString();
        }

        private string GetPlanning1Text(List<vNursingTransactionHd> lstEntity)
        {
            StringBuilder resultText = new StringBuilder();

            foreach (vNursingTransactionHd hdr in lstEntity)
            {
                string period = string.Format("{0} {1}", hdr.NOCInterval, hdr.NOCIntervalPeriod);
                string planningText = string.Format("Setelah dilakukan intervensi selama {0}, Masalah {1} {2} dengan luaran {3}", period,hdr.ProblemName,  hdr.IsProblemSolved ? "teratasi" : "belum teratasi", Environment.NewLine);

                string filterExpression = string.Format("TransactionID = {0}", hdr.TransactionID);
                List<vNursingTransactionOutcomeDt> lstDetail = BusinessLayer.GetvNursingTransactionOutcomeDtList(filterExpression);

                StringBuilder indicatorText = new StringBuilder();

                foreach (vNursingTransactionOutcomeDt item in lstDetail)
                {
                    if (!string.IsNullOrEmpty(item.NursingDiagnoseItemText))
                    {
                        indicatorText.AppendLine(string.Format("- {0} : {1} ({2})", item.NursingDiagnoseItemText.Replace(Environment.NewLine, string.Empty), item.NursingIndicatorText.Replace(Environment.NewLine, string.Empty), item.ScaleScore.ToString()));
                    }
                }
                planningText = string.Format("{0}{1}", planningText, indicatorText.ToString());
                resultText.AppendLine(planningText);
            }

            return resultText.ToString();
        }

        private string GetInstructionText(List<vNursingTransactionHd> lstEntity)
        {
            StringBuilder resultText = new StringBuilder();
            foreach (vNursingTransactionHd hdr in lstEntity)
            {
                string filterExpression = string.Format("TransactionID = {0}", hdr.TransactionID);
                List<vNursingTransactionInterventionHd> lstIntervention = BusinessLayer.GetvNursingTransactionInterventionHdList(filterExpression);
                StringBuilder instructionText = new StringBuilder();
                foreach (vNursingTransactionInterventionHd intervention in lstIntervention)
                {
                    filterExpression = string.Format("TransactionID = {0} AND NursingInterventionID = {1} ORDER BY DisplayOrder", hdr.TransactionID, intervention.NursingInterventionID);
                    List<vNursingTransactionInterventionDt> lstDetail = BusinessLayer.GetvNursingTransactionInterventionDtList(filterExpression);

                    if (lstEntity.Count > 0)
                    {
                        instructionText.AppendLine(string.Format("{0} ({1})", intervention.NurseInterventionName, intervention.NurseInterventionCode));

                        foreach (vNursingTransactionInterventionDt item in lstDetail)
                        {
                            if (!string.IsNullOrEmpty(item.NursingItemText.Replace(Environment.NewLine, string.Empty)))
                            {
                                instructionText.AppendLine(string.Format("- {0} {1}", item.DisplayOrder, item.NursingItemText.Replace(Environment.NewLine, string.Empty)));
                            }
                        }
                    }
                    resultText.AppendLine(instructionText.ToString());
                }                
            }

            return resultText.ToString();
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);

            entity.SubjectiveText = txtSubjectiveText.Text;
            entity.ObjectiveText = txtObjectiveText.Text;
            entity.AssessmentText = txtAssessmentText.Text;
            entity.PlanningText = txtPlanningText.Text;
            entity.InstructionText = txtInstructionText.Text;
            entity.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}",
Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
Environment.NewLine, txtPlanningText.Text, Environment.NewLine);

            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.GCPatientNoteType = Constant.PatientVisitNotes.NURSING_NOTES;

            if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                entity.LinkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientVisitNote(entity);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}