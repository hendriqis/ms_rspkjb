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
using System.IO;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class GenerateMedicalResume1Ctl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            MedicalResume obj = BusinessLayer.GetMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                hdnID.Value = obj.ID.ToString();
            }
            else
            {
                hdnID.Value = "0";
            }

            IsAdd = hdnID.Value == "0";
            SetControlProperties(obj);

            //PopulateFormContent();
        }

        private void PopulateFormContent()
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            StringBuilder innerHtml = new StringBuilder();

            #region DRG
            string fileName = string.Format(@"{0}\medicalForm\MedicalResume\{1}", filePath, "diagnosisRelatedGroup01.html");
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            divFormContent1.InnerHtml = innerHtml.ToString();
            hdnDRGLayout.Value = innerHtml.ToString();
            #endregion
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtMedicalResumeDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtMedicalResumeTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSubjectiveResumeText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveResumeText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAssessmentResumeText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPlanningResumeText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationResumeText, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInstructionResumeText, new ControlEntrySetting(true, true, false));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                MedicalResume entity = new MedicalResume();
                MedicalResumeDao entityDao = new MedicalResumeDao();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int id = entityDao.InsertReturnPrimaryKeyID(entity);
                retval = id.ToString();
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = true;
            if (rblIsHasSickLetter.SelectedValue == "1")
            {
                if (string.IsNullOrEmpty(txtNoOfDays.Text))
                {
                    errMessage = "Jumlah hari untuk surat keterangan sakit harus diisi dan lebih besar dari 0";
                }
                else
                {
                    int value;
                    if (!int.TryParse(txtNoOfDays.Text, out value))
                    {
                        errMessage = "Jumlah hari untuk surat keterangan sakit harus berupa angka dan lebih besar dari 0";
                    }
                }
            }
            return isValid;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                MedicalResume entity = BusinessLayer.GetMedicalResume(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateMedicalResume(entity);
                retval = hdnID.Value;
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private void ControlToEntity(MedicalResume entity)
        {
            entity.MedicalResumeDate = Helper.GetDatePickerValue(txtMedicalResumeDate);
            entity.MedicalResumeTime = txtMedicalResumeTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.IsHasSickLetter = rblIsHasSickLetter.SelectedValue == "1";
            if (entity.IsHasSickLetter)
            {
                entity.NoOfAbsenceDays = Convert.ToInt16(txtNoOfDays.Text);
            }
            entity.SubjectiveResumeText = txtSubjectiveResumeText.Text;
            entity.ObjectiveResumeText = txtObjectiveResumeText.Text;
            entity.AssessmentResumeText = txtAssessmentResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
            entity.MedicationResumeText = txtMedicationResumeText.Text;
            entity.SurgeryResumeText = txtMedicalResumeText.Text;
            entity.RelatedDiagnosisGroupInfoLayout = hdnDRGLayout.Value;
            entity.RelatedDiagnosisGroupInfoValues = hdnDRGValue.Value;
            entity.InstructionResumeText = txtInstructionResumeText.Text;
        }

        private void SetControlProperties(MedicalResume obj)
        {
            string filterExpression = string.Empty;
            string planningNotes = string.Empty;

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

            StringBuilder sbNotes = new StringBuilder();
            if (IsAdd)
            {
                #region Subjective Content
                if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
                {
                    vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                    if (oChiefComplaint != null)
                    {
                        sbNotes.AppendLine(string.Format("{0}{1}{2}", oChiefComplaint.ChiefComplaintText, Environment.NewLine, oChiefComplaint.HPISummary));

                        planningNotes = oChiefComplaint.PlanningSummary;
                    }
                }

                txtSubjectiveResumeText.Text = sbNotes.ToString();
                #endregion

                #region Objective Content
                sbNotes = new StringBuilder();
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
                if (lstVitalSignDt.Count > 0)
                {
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
                sbNotes.AppendLine(" ");

                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0}) AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID));
                if (lstROS.Count > 0)
                {
                    foreach (vReviewOfSystemDt item in lstROS)
                    {
                        sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                    }
                }

                txtObjectiveResumeText.Text = sbNotes.ToString();
                #endregion

                #region Assessment Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    foreach (vPatientDiagnosis item in lstDiagnosis)
                    {
                        if (string.IsNullOrEmpty(item.DiagnoseID))
                            sbNotes.AppendLine(string.Format("- {0}", item.cfDiagnosisText));
                        else
                            sbNotes.AppendLine(string.Format("- {0} ({1})", item.cfDiagnosisText, item.DiagnoseID));
                    }
                }
                txtAssessmentResumeText.Text = sbNotes.ToString();
                #endregion

                #region Planning Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
                List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
                if (lstOrder.Count > 0)
                {
                    foreach (vTestOrderDt item in lstOrder)
                    {
                        sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                    }
                }

                txtPlanningResumeText.Text = sbNotes.ToString();
                #endregion

                #region Medication Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted=0", AppSession.RegisteredPatient.RegistrationID, AppSession.UserLogin.ParamedicID);
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
                txtMedicationResumeText.Text = sbNotes.ToString();
                #endregion

                #region Tindakan yang dilakukan
                sbNotes = new StringBuilder();
                sbNotes.AppendLine("Tindakan :");
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
                List<vPatientProcedure> lstProcedure = BusinessLayer.GetvPatientProcedureList(filterExpression);
                if (lstProcedure.Count > 0)
                {
                    foreach (vPatientProcedure item in lstProcedure)
                    {
                        sbNotes.AppendLine(string.Format("- {0} ({1})", item.ProcedureName, item.ProcedureID));
                    }
                }

                sbNotes.AppendLine("");
                sbNotes.AppendLine("Catatan Tindakan :");
                sbNotes.AppendLine(string.Format("{0}", planningNotes));

                txtMedicalResumeText.Text = sbNotes.ToString();
                #endregion
            }
            else
            {
                rblIsHasSickLetter.SelectedValue = obj.IsHasSickLetter ? "1" : "0";
                txtNoOfDays.Text = obj.NoOfAbsenceDays.ToString();
                txtSubjectiveResumeText.Text = obj.SubjectiveResumeText;
                txtObjectiveResumeText.Text = obj.ObjectiveResumeText;
                txtAssessmentResumeText.Text = obj.AssessmentResumeText;
                txtPlanningResumeText.Text = obj.PlanningResumeText;
                txtMedicationResumeText.Text = obj.MedicationResumeText;
                txtMedicalResumeText.Text = obj.SurgeryResumeText;
                txtInstructionResumeText.Text = obj.InstructionResumeText;
                hdnDRGLayout.Value = obj.RelatedDiagnosisGroupInfoLayout;
                hdnDRGValue.Value = obj.RelatedDiagnosisGroupInfoValues;
            }
        }
    }
}