using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PhysicianNotes1List : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PHYSICIAN_NOTES_1;
        }

        #region List
        protected override void InitializeDataControl()
        {
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            if (AppSession.UserLogin.IsSpecialist)
                tblNotesInstruction.Style.Add("display", "none");           
            else
                tblNotesInstruction.Style.Add("display", "table");

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;

            string code = ddlViewType.SelectedValue;

            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ('{0}','{1}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (code == "0")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}')", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT);
            else if (code == "1")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}')",Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES);
            else if (code == "2")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES);
            else
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}') AND ParamedicID = {3}", Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC,NoteTime DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientVisitNote(entity);
                return true;
            }
            return false;
        }
        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            string filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboSpecialistPhysician, lstParamedic, "ParamedicName", "ParamedicID");

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningText, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.SubjectiveText = txtSubjectiveText.Text;
            entity.ObjectiveText = txtObjectiveText.Text;
            entity.AssessmentText = txtAssessmentText.Text;
            entity.PlanningText = txtPlanningText.Text;
            entity.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}",
                Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
                Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
                Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
                Environment.NewLine, txtPlanningText.Text);

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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion
    }
}