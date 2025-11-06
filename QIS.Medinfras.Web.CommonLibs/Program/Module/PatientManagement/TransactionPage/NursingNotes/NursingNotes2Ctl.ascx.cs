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
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NursingNotes2Ctl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "0")
            {
                IsAdd = false;
                hdnID.Value = paramInfo[0];
                OnControlEntrySettingLocal();
                ReInitControl();
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                hdnInstructionID.Value = paramInfo[1];
                hdnInstructionText.Value = paramInfo[2];
                OnControlEntrySettingLocal();
                ReInitControl();
                hdnID.Value = "";
                IsAdd = true;
            }
        }

        private void SetControlProperties()
        {
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}','{1}','{2}')",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, Constant.ParamedicType.Nutritionist, AppSession.RegisteredPatient.HealthcareServiceUnitID));
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
                filterExpression = string.Format("HealthcareServiceUnitID = {0}",AppSession.RegisteredPatient.HealthcareServiceUnitID);
                List<vServiceUnitParamedic> lstPhysician2 = BusinessLayer.GetvServiceUnitParamedicList(filterExpression);
                Methods.SetComboBoxField(cboPhysician, lstPhysician2, "ParamedicName", "ParamedicID");
            }

            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
        }

        //ini terpaksa buat function baru karena tidak mau mengubah master page
        //tunggu ada waktu baru di rapiin
        private void OnControlEntrySettingLocal()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlProperties();
            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
                || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic.ToString()));
            }
            else SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true));

            string[] instructionInfo = hdnInstructionText.Value.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string item in instructionInfo)
            {
                sb.AppendLine(item);
            }
            txtInstruction.Text = sb.ToString();
        }

        private void EntityToControl(PatientVisitNote entity)
        {
            txtNoteDate.Text = entity.NoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entity.NoteTime;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtNoteText.Text = entity.NoteText;
            txtInstruction.Text = entity.InstructionText;
         
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
            chkIsNeedConfirmation.Checked = entity.IsNeedConfirmation;
            chkIsNeedNotification.Checked = entity.IsNeedNotification;
            if (entity.NotificationUnitID != null)
            {
                cboServiceUnit.Value = entity.NotificationUnitID.ToString();
            }

        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            string[] instructionInfo = hdnInstructionText.Value.Split(',');
            StringBuilder sb = new StringBuilder();
            foreach (string item in instructionInfo)
            {
                sb.AppendLine(item);
            }
            string instructionText = sb.ToString();

            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            entity.InstructionText = Request.Form[txtInstruction.UniqueID];
            entity.NoteText = txtNoteText.Text;
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.GCPatientNoteType = Constant.PatientVisitNotes.NURSING_NOTES;

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

                if (chkIsCompleted.Checked)
                {
                    UpdateInstructionToCompleted(hdnInstructionID.Value);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }

        private string UpdateInstructionToCompleted(string lstRecordID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientInstructionDao entityDao = new PatientInstructionDao(ctx);
            string filterExpression = string.Format("PatientInstructionID IN ({0})", lstRecordID);

            try
            {
                if (AppSession.UserLogin.ParamedicID != null)
                {
                    //Confirm
                    List<PatientInstruction> oList = BusinessLayer.GetPatientInstructionList(filterExpression, ctx);
                    foreach (PatientInstruction instruction in oList)
                    {
                        if (!instruction.IsCompleted && AppSession.UserLogin.ParamedicID != null)
                        {
                            instruction.IsCompleted = true;
                            instruction.ExecutedDateTime = DateTime.Now;
                            instruction.ExecutedBy = AppSession.UserLogin.ParamedicID;
                            entityDao.Update(instruction);
                        }
                    }
                    ctx.CommitTransaction();
                    result = string.Format("process|1|{0}", string.Empty);
                }
                else
                {
                    result = string.Format("process|0|{0}", "Invalid Paramedic / Nurse ID");
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                result = string.Format("process|0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
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