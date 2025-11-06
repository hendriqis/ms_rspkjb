using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class ReferralResponseEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            string recordID = paramInfo[0];

            SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM_SALIN_CATATAN_KONSULTASI)).FirstOrDefault();

            if (oParam != null)
                hdnIsCopyFromSource.Value = oParam.ParameterValue;
            else
                hdnIsCopyFromSource.Value = "0";

            hdnPopupVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnPopupParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            SetControlProperties();

            if (!string.IsNullOrEmpty(recordID))
            {
                IsAdd = false;
                PatientReferral obj = BusinessLayer.GetPatientReferralList(string.Format("ID = {0}", recordID)).FirstOrDefault();
                if (obj != null)
                {
                    hdnID.Value = obj.ID.ToString();
                    EntityToControl(obj);
                }
                else
                {
                    hdnID.Value = "0";
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtReplyDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtReplyTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboPhysician2, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDiagnosisText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMedicalResumeText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningResumeText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReplySubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReplyObjectiveText, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao entityDao = new PatientReferralDao(ctx);
            PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao oInstructionDao = new PatientInstructionDao(ctx);

            try
            {
                PatientReferral entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);

                int replyPatientVisitNoteID = Convert.ToInt32(entity.ReplyPatientVisitNoteID);

                if (replyPatientVisitNoteID == 0)
                {
                    replyPatientVisitNoteID = Convert.ToInt32(hdnPopupVisitNoteID.Value);
                }

                StringBuilder objectiveText = new StringBuilder();
                objectiveText.AppendLine(txtReplyObjectiveText.Text);
                objectiveText.AppendLine(txtMedicalResumeText.Text);


                if (replyPatientVisitNoteID == 0)
                {
                    PatientVisitNote oVisitNote = new PatientVisitNote();
                    oVisitNote.NoteDate = Helper.GetDatePickerValue(txtReplyDate);
                    oVisitNote.NoteTime = txtReplyTime.Text;
                    oVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                    oVisitNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
                    oVisitNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                    oVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                    oVisitNote.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}I:{12}{13}",
                        Environment.NewLine, txtReplySubjectiveText.Text, Environment.NewLine,
                        Environment.NewLine, objectiveText.ToString(), Environment.NewLine,
                        Environment.NewLine, txtDiagnosisText.Text, Environment.NewLine,
                        Environment.NewLine, txtPlanningResumeText.Text, Environment.NewLine,
                        Environment.NewLine, txtInstructionResumeText.Text);
                    oVisitNote.SubjectiveText = txtReplySubjectiveText.Text;
                    oVisitNote.ObjectiveText = objectiveText.ToString();
                    oVisitNote.AssessmentText = txtDiagnosisText.Text;
                    oVisitNote.PlanningText = txtPlanningResumeText.Text;
                    oVisitNote.InstructionText = txtInstructionResumeText.Text;
                    oVisitNote.CreatedBy = AppSession.UserLogin.UserID;
                    replyPatientVisitNoteID = patientVisitNoteDao.InsertReturnPrimaryKeyID(oVisitNote);

                    if (!string.IsNullOrEmpty(txtInstructionResumeText.Text))
                    {
                        PatientInstruction oInstruction = new PatientInstruction();
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = replyPatientVisitNoteID;
                        oInstruction.PhysicianID = Convert.ToInt32(cboPhysician.Value);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionResumeText.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtReplyDate);
                        oInstruction.InstructionTime = txtReplyTime.Text;
                        oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                        oInstructionDao.Insert(oInstruction);
                    }
                }
                else
                {
                    PatientVisitNote oVisitNote = patientVisitNoteDao.Get(replyPatientVisitNoteID);
                    oVisitNote.VisitID = AppSession.RegisteredPatient.VisitID;
                    oVisitNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                    oVisitNote.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}I:{12}{13}",
                        Environment.NewLine, txtReplySubjectiveText.Text, Environment.NewLine,
                        Environment.NewLine, objectiveText.ToString(), Environment.NewLine,
                        Environment.NewLine, txtDiagnosisText.Text, Environment.NewLine,
                        Environment.NewLine, txtPlanningResumeText.Text, Environment.NewLine,
                        Environment.NewLine, txtInstructionResumeText.Text);
                    oVisitNote.SubjectiveText = txtReplySubjectiveText.Text;
                    oVisitNote.ObjectiveText = objectiveText.ToString();
                    oVisitNote.AssessmentText = txtDiagnosisText.Text;
                    oVisitNote.PlanningText = txtPlanningResumeText.Text;
                    oVisitNote.InstructionText = txtInstructionResumeText.Text;
                    oVisitNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                    patientVisitNoteDao.Update(oVisitNote);

                    PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(string.Format("PatientVisitNoteID = {0} AND IsDeleted = 0", replyPatientVisitNoteID), ctx).FirstOrDefault();
                    if (oInstruction != null)
                    {
                        if (oInstruction.AdditionalText != txtInstructionResumeText.Text)
                        {
                            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtReplyDate);
                            oInstruction.InstructionTime = txtReplyTime.Text;
                            oInstruction.Description = txtInstructionResumeText.Text;
                            oInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                            oInstructionDao.Update(oInstruction);
                        }
                    }
                    
                }


                entity.ReplyPatientVisitNoteID = replyPatientVisitNoteID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                ctx.CommitTransaction();

                return true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                return false;
            }
            finally {
                ctx.Close();
            }
        }

        private void ControlToEntity(PatientReferral entity)
        {
            entity.ReferralDate = Helper.GetDatePickerValue(txtReplyDate);
            entity.ReferralTime = txtReplyTime.Text;
            entity.FromPhysicianID = Convert.ToInt32(cboPhysician.Value);
            entity.ToPhysicianID = Convert.ToInt32(cboPhysician2.Value);
            entity.ReplySubjectiveText = txtReplySubjectiveText.Text;
            entity.ReplyObjectiveText = txtReplyObjectiveText.Text;
            entity.ReplyDiagnosisText = txtDiagnosisText.Text;
            entity.ReplyMedicalResumeText = txtMedicalResumeText.Text;
            entity.ReplyPlanningResumeText = txtPlanningResumeText.Text;
            entity.ReplyInstructionResumeText = txtInstructionResumeText.Text;
            if (!entity.IsReply)
            {
                entity.IsReply = true;
                entity.ReplyDate = DateTime.Now.Date;
                entity.ReplyTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT); 
            }
        }

        private void SetControlProperties()
        {
            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician2, lstParamedic, "ParamedicName", "ParamedicID");

            cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false;
            #endregion

            if (IsAdd)
            {
                cboPhysician2.Value = string.Empty;
                txtDiagnosisText.Text = string.Empty;
                txtMedicalResumeText.Text = string.Empty;
                txtPlanningResumeText.Text = string.Empty;
            }
        }

        private void EntityToControl(PatientReferral obj)
        {
            cboPhysician.Value = obj.FromPhysicianID.ToString();
            cboPhysician2.Value = obj.ToPhysicianID.ToString();
            txtSourceDiagnosisText.Text = obj.DiagnosisText;
            txtSourceMedicalResumeText.Text = obj.MedicalResumeText;
            txtSourcePlanningResumeText.Text = obj.PlanningResumeText;

            if (!obj.IsReply)
	        {
                txtReplyDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReplyTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                if (hdnIsCopyFromSource.Value == "1")
                {
                    txtDiagnosisText.Text = obj.DiagnosisText;
                    txtMedicalResumeText.Text = obj.MedicalResumeText;
                    txtPlanningResumeText.Text = obj.PlanningResumeText;
                }
	        }
            else
            {
                txtReplyDate.Text = obj.ReplyDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReplyTime.Text = obj.ReplyTime;

                if (!string.IsNullOrEmpty(obj.ReplySubjectiveText))
                    txtReplySubjectiveText.Text = obj.ReplySubjectiveText;

                if (!string.IsNullOrEmpty(obj.ReplyObjectiveText))
                    txtReplyObjectiveText.Text = obj.ReplyObjectiveText;

                if (!string.IsNullOrEmpty(obj.ReplyDiagnosisText))
                    txtDiagnosisText.Text = obj.ReplyDiagnosisText;

                if (!string.IsNullOrEmpty(obj.ReplyMedicalResumeText))
                    txtMedicalResumeText.Text = obj.ReplyMedicalResumeText;

                if (!string.IsNullOrEmpty(obj.ReplyPlanningResumeText))
                    txtPlanningResumeText.Text = obj.ReplyPlanningResumeText;
                if (!string.IsNullOrEmpty(obj.ReplyInstructionResumeText))
                    txtInstructionResumeText.Text = obj.ReplyInstructionResumeText;
            }
        }
    }
}