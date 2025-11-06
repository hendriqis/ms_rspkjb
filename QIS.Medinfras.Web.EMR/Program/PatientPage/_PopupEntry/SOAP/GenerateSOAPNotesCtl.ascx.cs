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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class GenerateSOAPNotesCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = "";
            IsAdd = true;
            SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtPlanningText, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtInstructionText, new ControlEntrySetting(true, false, false));
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {

                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientVisitNote(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
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

        private void SetControlProperties()
        {
            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false; 
            #endregion

            #region Generate Planning Notes Content
            string filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND TestOrderDate = '{2}' AND IsDeleted = 0 AND HealthcareServiceUnitID NOT IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WHERE IsNutritionUnit = 1)",
                    AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            StringBuilder sbNotes = new StringBuilder();
            List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
            if (lstOrder.Count>0)
            {
                sbNotes.AppendLine("Pemeriksaan Penunjang :");
                foreach (vTestOrderDt item in lstOrder)
                {
                    sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                }
            }

            if (!string.IsNullOrEmpty(sbNotes.ToString()))
            {
                sbNotes.AppendLine("");
            }

            filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND PrescriptionDate = '{2}'", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vPrescriptionOrderDt1> lstPrescription = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
            if (lstPrescription.Count > 0)
            {
                sbNotes.AppendLine("Order Farmasi :");
                foreach (vPrescriptionOrderDt1 item in lstPrescription)
                {
                    if (item.IsRFlag)                    
                        sbNotes.AppendLine(string.Format("- {0} {1}x{2}", item.DrugName,item.Frequency,item.NumberOfDosage.ToString("G29")));                        
                    else
                        sbNotes.AppendLine(string.Format("  {0}", item.DrugName));                        
                }
            }
            txtPlanningText.Text = sbNotes.ToString();
            #endregion

            #region Generate Instruction Notes Content
            filterExpression = string.Format("VisitID = {0} AND PhysicianID = {1} AND InstructionDate = '{2}' AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            sbNotes = new StringBuilder();
            List<vPatientInstruction> lstInstruction = BusinessLayer.GetvPatientInstructionList(filterExpression);
            if (lstInstruction.Count > 0)
            {
                foreach (vPatientInstruction item in lstInstruction)
                {
                    sbNotes.AppendLine(string.Format("- {0}", item.Description));
                }
            }
            txtInstructionText.Text = sbNotes.ToString();
            #endregion
        }
    }
}