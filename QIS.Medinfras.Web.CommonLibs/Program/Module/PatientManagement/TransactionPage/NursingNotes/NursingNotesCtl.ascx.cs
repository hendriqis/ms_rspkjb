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
    public partial class NursingNotesCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            if (param != "")
            {
                IsAdd = false;
                hdnID.Value = param;
                OnControlEntrySettingLocal();
                ReInitControl();
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "";
                IsAdd = true;
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
                lstPhysician.Add(new vParamedicTeam() {ParamedicID = entityPM.ParamedicID, ParamedicCode = entityPM.ParamedicCode, ParamedicName = entityPM.FullName });
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
            SetControlProperties();

            if (!string.IsNullOrEmpty(AppSession.UserLogin.GCParamedicMasterType) && AppSession.UserLogin.GCParamedicMasterType != Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else
            {
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
            }

            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionText, new ControlEntrySetting(true, true, true));
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
            cboPhysicianInstructionSource.Value = entity.GCPhysicianInstructionSource;
            if (entity.LinkedNoteID != null)
            {
                hdnPlanningNoteID.Value = entity.LinkedNoteID.ToString();
                PatientVisitNote oLinkNote = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(entity.LinkedNoteID));
                string noteText = string.Empty;
                bool isSOAPText = !string.IsNullOrEmpty(oLinkNote.SubjectiveText);

                if (isSOAPText)
                {
                    if (!string.IsNullOrEmpty(oLinkNote.SubjectiveText))
                    {
                        noteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}",
                            Environment.NewLine, oLinkNote.SubjectiveText, Environment.NewLine,
                            Environment.NewLine, oLinkNote.ObjectiveText, Environment.NewLine,
                            Environment.NewLine, oLinkNote.AssessmentText, Environment.NewLine,
                            Environment.NewLine, oLinkNote.PlanningText, Environment.NewLine);
                    }
                }
                else
                {
                    //noteText = _NoteText;
                    noteText = string.Format(@"{0}{1}{2}{3}{4}",
                        Environment.NewLine, oLinkNote.NoteText, Environment.NewLine,
                        Environment.NewLine, oLinkNote.InstructionText);
                }
                if (oLinkNote != null)
                    txtPatientVisitNoteText.Text = noteText;
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
            chkIsNeedNotification.Checked = entity.IsNeedNotification;
            if (entity.NotificationUnitID != null)
            {
                cboServiceUnit.Value = entity.NotificationUnitID.ToString();
            }
            chkIsPanicRangeReporting.Checked = entity.IsPanicValueReporting;
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            if (string.IsNullOrEmpty(AppSession.HealthcareServiceUnitID) || AppSession.HealthcareServiceUnitID == "0")
            {
                entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            }
            else
            {
                entity.HealthcareServiceUnitID = Convert.ToInt32(AppSession.HealthcareServiceUnitID);
            }
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);

            if (rblNoteMode.SelectedValue == "1")
            {
                entity.NoteText = txtNoteText.Text;
                entity.SubjectiveText = string.Empty;
                entity.ObjectiveText = string.Empty;
                entity.AssessmentText = string.Empty;
                entity.PlanningText = string.Empty;
                entity.InstructionText = string.Empty;
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

            entity.GCPatientNoteType = Constant.PatientVisitNotes.NURSING_NOTES;

            if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                entity.LinkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);
            if (cboPhysicianInstructionSource.Value != null)
                entity.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();

            entity.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
            if (chkIsNeedConfirmation.Checked)
            {
                if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                    entity.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
            }
            entity.IsNeedNotification = chkIsNeedNotification.Checked;
            if (chkIsNeedNotification.Checked)
            {
                if (cboServiceUnit.Value != null)
                {
                    entity.NotificationUnitID = Convert.ToInt32(cboServiceUnit.Value); 
                }
            }

            entity.IsPanicValueReporting = chkIsPanicRangeReporting.Checked;

            string dateTime = string.Format("{0}{1}",Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112),txtNoteTime.Text.Replace(":",""));
            entity.IsWrite = chkIsWritten.Checked;
            if (chkIsWritten.Checked)
            {
                entity.WriteDateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                entity.WriteParamedicID = AppSession.UserLogin.ParamedicID;
            }
            entity.IsReadback = chkIsWritten.Checked;
            if (chkIsReadback.Checked)
            {
                entity.ReadbackDateTime = DateTime.ParseExact(dateTime, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
                entity.ReadbackParamedicID = AppSession.UserLogin.ParamedicID;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientVisitNote entity = new PatientVisitNote();
                    ControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.InsertPatientVisitNote(entity);
                }
                else
                {
                    result = false;
                }
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
            try
            {
                if (IsValid(ref errMessage))
                {
                    PatientVisitNote entityUpdate = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                    ControlToEntity(entityUpdate);
                    entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityUpdate.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdatePatientVisitNote(entityUpdate);
                }
                else
                {
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = true;
            if (rblNoteMode.SelectedValue == "1")
            {
                if (string.IsNullOrEmpty(txtNoteText.Text))
                {
                    isValid = false;
                    errMessage = "Catatan Terintegrasi tidak boleh kosong";
                }
            }
            else
            {
                if (string.IsNullOrEmpty(txtSubjectiveText.Text))
                {
                    isValid = false;
                    errMessage = "Catatan Terintegrasi tidak boleh kosong";
                }
            }
            return isValid;
        }

    }
}