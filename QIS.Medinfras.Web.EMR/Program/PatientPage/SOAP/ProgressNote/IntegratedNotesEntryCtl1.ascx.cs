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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class IntegratedNotesEntryCtl1 : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnUserLoginParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            bool isFreeText = true;

            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                OnControlEntrySettingLocal();
                ReInitControl();
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);

                if (!String.IsNullOrEmpty(entity.SubjectiveText))
                {
                    isFreeText = false;
                }
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "";
                IsAdd = true;
            }

            if (AppSession.UserLogin.IsRMO)
            {
                if (IsAdd)
                {
                    rblNoteMode.SelectedValue = "1";
                    rblNoteMode.Enabled = true;

                    hdnIsAllowFreeTextMode.Value = "1";

                    trFreeTextNote.Style.Add("display", "table-row");
                    trSOAPINote.Style.Add("display", "none");
                    tdCopyButton.Style.Add("display", "none");
                }
                else
                {
                    rblNoteMode.Enabled = false;

                    if (isFreeText)
                    {
                        rblNoteMode.SelectedValue = "1";

                        hdnIsAllowFreeTextMode.Value = "1";

                        trFreeTextNote.Style.Add("display", "table-row");
                        trSOAPINote.Style.Add("display", "none");
                        tdCopyButton.Style.Add("display", "none");
                    }
                    else
                    {
                        rblNoteMode.SelectedValue = "2";

                        hdnIsAllowFreeTextMode.Value = "0";

                        trFreeTextNote.Style.Add("display", "none");
                        trSOAPINote.Style.Add("display", "table-row");
                        tdCopyButton.Style.Add("display", "block");
                    }
                }

                trRMOPhysician1.Style.Add("display", "table-row");
                trRMOPhysician2.Style.Add("display", "table-row");
                trRMOPhysician3.Style.Add("display", "table-row");
                trRMOPhysician4.Style.Add("display", "table-row");
                trRMOPhysician5.Style.Add("display", "table-row");
            }
            else
            {
                rblNoteMode.SelectedValue = "2";
                rblNoteMode.Enabled = false;

                hdnIsAllowFreeTextMode.Value = "0";

                trFreeTextNote.Style.Add("display", "none");
                trSOAPINote.Style.Add("display", "table-row");
                tdCopyButton.Style.Add("display", "block");

                trRMOPhysician1.Style.Add("display", "none");
                trRMOPhysician2.Style.Add("display", "none");
                trRMOPhysician3.Style.Add("display", "none");
                trRMOPhysician4.Style.Add("display", "none");
                trRMOPhysician5.Style.Add("display", "none");
            }
        }

        protected string GetFilterExpression()
        {
            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM0071));
            hdnFilterParameter.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0071).ParameterValue;

            if (hdnFilterParameter.Value == "1")
            {
                return string.Format("VisitID = {0} AND GCPatientNoteType IN ('X011^002','X011^004','X011^011') AND SubjectiveText IS NOT NULL", hdnVisitID.Value);
            }
            else
            {
                return string.Format("VisitID = {0} AND ParamedicID = {1} AND GCPatientNoteType IN ('X011^002','X011^004','X011^011') AND SubjectiveText IS NOT NULL", hdnVisitID.Value, hdnUserLoginParamedicID.Value);
            }
        }

        private void SetControlProperties()
        {
            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}') AND ParamedicID = {1}",
                                                    Constant.ParamedicType.Physician, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            string filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            filterExpression = string.Format("DepartmentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, Constant.Facility.PHARMACY);
            List<vHealthcareServiceUnit> lstUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField(cboServiceUnit, lstUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            filterExpression = string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);

            if (lstPhysician.Count() > 0)
            {
                lstPhysician.Add(new vParamedicTeam() { ParamedicID = entityPM.ParamedicID, ParamedicCode = entityPM.ParamedicCode, ParamedicName = entityPM.FullName });
                Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            }
            else
            {
                //filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID);
                //List<vServiceUnitParamedic> lstPhysician2 = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                //Methods.SetComboBoxField(cboPhysician, lstPhysician2, "ParamedicName", "ParamedicID");

                int physician = AppSession.RegisteredPatient.ParamedicID != null ? Convert.ToInt32(AppSession.RegisteredPatient.ParamedicID) : 0;
                List<vParamedicMaster> lstPhysician2 = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                        "ParamedicID = {0}", physician));
                Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstPhysician2, "ParamedicName", "ParamedicID");
            }

            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
        }

        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionText, new ControlEntrySetting(true, true, true));
            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }
        }

        private void EntityToControl(PatientVisitNote entity)
        {
            txtNoteDate.Text = entity.NoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entity.NoteTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtNoteText.Text = entity.NoteText;
            txtSubjectiveText.Text = entity.SubjectiveText;
            txtObjectiveText.Text = entity.ObjectiveText;
            txtAssessmentText.Text = entity.AssessmentText;
            txtPlanningText.Text = entity.PlanningText;
            txtInstructionText.Text = entity.InstructionText;
            txtInstructionText2.Text = entity.InstructionText;
            cboPhysicianInstructionSource.Value = entity.GCPhysicianInstructionSource;
            if (entity.LinkedNoteID != null)
            {
                hdnPlanningNoteID.Value = entity.LinkedNoteID.ToString();
                PatientVisitNote oLinkNote = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(entity.LinkedNoteID));
                if (oLinkNote != null)
                    txtPatientVisitNoteText.Text = oLinkNote.NoteText;
                else
                    txtPatientVisitNoteText.Text = "";
            }
            else
            {
                hdnPlanningNoteID.Value = "";
                txtPatientVisitNoteText.Text = "";
            }
            chkIsWritten.Checked = entity.IsWrite;
            chkIsReadback.Checked = entity.IsReadback;
            chkIsNeedConfirmation.Checked = entity.IsNeedConfirmation;
            cboPhysician.Value = entity.ConfirmationPhysicianID.ToString();
            hdnParamedicID.Value = entity.ConfirmationPhysicianID.ToString();
            chkIsNeedNotification.Checked = entity.IsNeedNotification;
            if (entity.NotificationUnitID != null)
            {
                cboServiceUnit.Value = entity.NotificationUnitID.ToString();
            }
            chkIsRMOHandsover.Checked = entity.IsRMOHandsover;
        }

        private void ControlToEntity(PatientVisitNote entity, ref string noteText)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;

            if (rblNoteMode.SelectedValue == "1")
            {
                entity.NoteText = txtNoteText.Text;
                entity.SubjectiveText = string.Empty;
                entity.ObjectiveText = string.Empty;
                entity.AssessmentText = string.Empty;
                entity.PlanningText = string.Empty;
                entity.InstructionText = txtInstructionText2.Text;
            }
            else
            {
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
            }

            noteText = entity.NoteText;

            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;

            if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                entity.LinkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);
            if (cboPhysicianInstructionSource.Value != null)
                entity.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
            if (chkIsNeedConfirmation.Checked)
            {
                entity.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                    entity.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
            }
            if (chkIsNeedNotification.Checked)
            {
                entity.IsNeedNotification = chkIsNeedNotification.Checked;
                if (cboServiceUnit.Value != null)
                {
                    entity.NotificationUnitID = Convert.ToInt32(cboServiceUnit.Value);
                }
            }

            string dateTime = string.Format("{0}{1}", Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtNoteTime.Text.Replace(":", ""));
            if (chkIsWritten.Checked)
            {
                entity.IsWrite = chkIsWritten.Checked;
                entity.WriteDateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                entity.WriteParamedicID = AppSession.UserLogin.ParamedicID;
            }
            if (chkIsReadback.Checked)
            {
                entity.IsReadback = chkIsWritten.Checked;
                entity.ReadbackDateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                entity.ReadbackParamedicID = AppSession.UserLogin.ParamedicID;
            }

            entity.IsRMOHandsover = chkIsRMOHandsover.Checked;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao instructionDao = new PatientInstructionDao(ctx);
            string noteText = string.Empty;

            try
            {
                int visitNoteID = 0;

                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity, ref noteText);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                visitNoteID = visitNoteDao.InsertReturnPrimaryKeyID(entity);

                if (!string.IsNullOrEmpty(txtInstructionText.Text))
                {
                    string filterExpInstruction = string.Format("PatientVisitNoteID = {0}", visitNoteID);
                    PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(filterExpInstruction, ctx).FirstOrDefault();

                    if (oInstruction == null)
                    {
                        oInstruction = new PatientInstruction();
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = Convert.ToInt32(visitNoteID);
                        oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionText.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
                        oInstruction.InstructionTime = txtNoteTime.Text;
                        oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                        instructionDao.Insert(oInstruction);
                    }
                    else
                    {
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = Convert.ToInt32(visitNoteID);
                        oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionText.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
                        oInstruction.InstructionTime = txtNoteTime.Text;
                        oInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                        instructionDao.Update(oInstruction);
                    }
                }

                ctx.CommitTransaction();

            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                string paramedicID = "0";
                string healthcareServiceUnitID = "0";
                if (cboParamedicID.Value != null)
                {
                    paramedicID = cboParamedicID.Value.ToString();
                }
                if (AppSession.RegisteredPatient != null)
                {
                    if (AppSession.RegisteredPatient.HealthcareServiceUnitID != null && AppSession.RegisteredPatient.HealthcareServiceUnitID != 0)
                    {
                        healthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
                    }
                }
                errMessage = string.Format("{0} (HSU = {1}, ParamedicID = {2})", ex.Message, healthcareServiceUnitID, paramedicID);
                Methods.LogIntegrationNotesError(Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtNoteTime.Text, AppSession.UserLogin.UserName, noteText);
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao entityPatientVisitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao instructionDao = new PatientInstructionDao(ctx);
            string noteText = string.Empty;

            bool result = true;
            try
            {
                PatientVisitNote entityUpdate = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate, ref noteText);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityPatientVisitNoteDao.Update(entityUpdate);


                if (!string.IsNullOrEmpty(txtInstructionText.Text))
                {
                    string filterExpInstruction = string.Format("PatientVisitNoteID = {0}", hdnID.Value);
                    PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(filterExpInstruction, ctx).FirstOrDefault();

                    if (oInstruction == null)
                    {
                        oInstruction = new PatientInstruction();
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = Convert.ToInt32(hdnID.Value);
                        oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionText.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
                        oInstruction.InstructionTime = txtNoteTime.Text;
                        oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                        instructionDao.Insert(oInstruction);
                    }
                    else
                    {
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = Convert.ToInt32(hdnID.Value);
                        oInstruction.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionText.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
                        oInstruction.InstructionTime = txtNoteTime.Text;
                        oInstruction.LastUpdatedBy = AppSession.UserLogin.UserID;
                        instructionDao.Update(oInstruction);
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                string paramedicID = "0";
                string healthcareServiceUnitID = "0";
                if (cboParamedicID.Value != null)
                {
                    paramedicID = cboParamedicID.Value.ToString();
                }
                if (AppSession.RegisteredPatient != null)
                {
                    if (AppSession.RegisteredPatient.HealthcareServiceUnitID != null && AppSession.RegisteredPatient.HealthcareServiceUnitID != 0)
                    {
                        healthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID.ToString();
                    }
                }
                errMessage = string.Format("{0} (HSU = {1}, ParamedicID = {2})", ex.Message, healthcareServiceUnitID, paramedicID);
                Methods.LogIntegrationNotesError(Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112), txtNoteTime.Text, AppSession.UserLogin.UserName, noteText);
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }
    }
}