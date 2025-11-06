using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicNotesCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnDepartmentID.Value = paramInfo[0];
            hdnServiceUnitID.Value = paramInfo[1];
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();

            if (paramInfo[2] != "")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[2];
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
            string filterExp = string.Format("GCParamedicMasterType IN ('{0}','{1}','{2}')",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, AppSession.RegisteredPatient.HealthcareServiceUnitID);
            if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                filterExp = string.Format("GCParamedicMasterType IN ('{0}')", Constant.ParamedicType.Pharmacist);    
            else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                filterExp = string.Format("GCParamedicMasterType NOT IN ('{0}','{1}')", Constant.ParamedicType.Pharmacist, Constant.ParamedicType.Physician);    
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExp);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            string filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            filterExpression = string.Format("DepartmentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.Facility.DIAGNOSTIC, Constant.Facility.PHARMACY);
            List<vHealthcareServiceUnit> lstUnit = BusinessLayer.GetvHealthcareServiceUnitList(filterExpression);
            Methods.SetComboBoxField(cboServiceUnit, lstUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);

            if (lstPhysician.Count() > 0)
            {
                Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
            }
            else
            {
                filterExpression = string.Format("HealthcareServiceUnitID = {0}", AppSession.RegisteredPatient.HealthcareServiceUnitID);
                List<vServiceUnitParamedic> lstPhysician2 = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                Methods.SetComboBoxField(cboPhysician, lstPhysician2, "ParamedicName", "ParamedicID");
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
            if (AppSession.UserLogin.ParamedicID != 0)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));
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
            chkIsNeedNotification.Checked = entity.IsNeedNotification;
            if (entity.NotificationUnitID != null)
            {
                cboServiceUnit.Value = entity.NotificationUnitID.ToString();
            }
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
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

            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            string noteType = Constant.PatientVisitNotes.NURSING_NOTES;
            if (hdnDepartmentID.Value == Constant.Facility.PHARMACY || hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
            {
                if (hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                    noteType = Constant.PatientVisitNotes.PHARMACY_NOTES;
                else
                    noteType = Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES;
            }

            entity.GCPatientNoteType = noteType;

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
            //IDbContext ctx = DbFactory.Configure(true);
            //PatientVisitNoteDao entityPatientVisitNoteDao = new PatientVisitNoteDao(ctx);
            bool result = true;
            try
            {
                PatientVisitNote entityUpdate = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entityUpdate);
                //ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
               // ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}