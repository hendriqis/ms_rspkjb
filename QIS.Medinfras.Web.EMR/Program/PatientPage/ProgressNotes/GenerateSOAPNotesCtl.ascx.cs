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
            entity.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
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
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("------------");
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {
                    sbNotes.AppendLine(string.Format("C.C  : "));
                    sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.ChiefComplaintText));
                    sbNotes.AppendLine(string.Format("HPI  : "));
                    if (!string.IsNullOrEmpty(oChiefComplaint.Location))
                        sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
                        sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
                        sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
                        sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
                    if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
                        sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
                        sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));
                }
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Objective");
            sbNotes.AppendLine("---------");
            sbNotes.AppendLine("Vital Signs :");
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count>0)
            {
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}",vitalSign.VitalSignLabel,vitalSign.VitalSignValue,vitalSign.ValueUnit));        
                }
            }
            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Review of System :");
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }
            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Planning ");
            sbNotes.AppendLine("---------");
            string filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND TestOrderDate = '{2}' AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID, Helper.GetDatePickerValue(txtNoteDate).ToString(Constant.FormatString.DATE_FORMAT_112));
            List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
            if (lstOrder.Count > 0)
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
                        sbNotes.AppendLine(string.Format("- {0} {1}x{2}", item.ItemName1, item.Frequency, item.NumberOfDosage.ToString("G29")));
                    else
                        sbNotes.AppendLine(string.Format("  {0}", item.ItemName1));
                }
            }
            txtNoteText.Text = sbNotes.ToString();
            #endregion
        }
    }
}