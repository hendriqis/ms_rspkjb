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

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class GeneratePlanningNotesCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnID.Value = "";
            IsAdd = true;
            SetControlProperties();
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtNoteText, new ControlEntrySetting(true, false, true));
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
            entity.GCPatientNoteType = Constant.PatientVisitNotes.PLANNING_NOTES;
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.NoteText = txtNoteText.Text;
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
            string filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND TestOrderDate = '{2}' AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            StringBuilder sbNotes = new StringBuilder();
            List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
            if (lstOrder.Count>0)
            {
                sbNotes.AppendLine("PDx :");
                foreach (vTestOrderDt item in lstOrder)
                {
                    sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                }
            }

            filterExpression = string.Format("RegistrationID = {0} AND ParamedicID = {1} AND PrescriptionDate = '{2}' AND IsDeleted=0", AppSession.RegisteredPatient.RegistrationID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vPrescriptionOrderDt> lstPrescription = BusinessLayer.GetvPrescriptionOrderDtList(filterExpression);
            if (lstPrescription.Count > 0)
            {
                sbNotes.AppendLine("PTx :");
                foreach (vPrescriptionOrderDt item in lstPrescription)
                {
                    if (item.IsRFlag)                    
                        sbNotes.AppendLine(string.Format("- {0} {1}x{2}", item.ItemName1,item.Frequency,item.NumberOfDosage.ToString("G29")));                        
                    else
                        sbNotes.AppendLine(string.Format("  {0}", item.ItemName1));                        
                }
            }
            txtNoteText.Text = sbNotes.ToString();
            #endregion
        }
    }
}