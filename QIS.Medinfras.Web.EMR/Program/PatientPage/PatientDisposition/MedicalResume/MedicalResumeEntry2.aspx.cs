﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class MedicalResumeEntry2 : BasePagePatientPageList
    {
        protected static string _medicalResumeID = "0";
        protected static string _linkedVisitID;

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetMedicalResumeID()
        {
            return _medicalResumeID;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.MEDICAL_RESUME;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnMedicalResumeID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }


            Helper.SetControlEntrySetting(txtAssessmentResumeText, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            txtResumeDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResumeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnLinkedVisitID.Value = _linkedVisitID;

            SetEntityToControl();

            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                        AppSession.UserLogin.HealthcareID, //1
                                                        Constant.SettingParameter.EM_CASEMIX_ALLOW_CHANGE_MEDICAL_RESUME //2
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsCasemixAllowChange.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_CASEMIX_ALLOW_CHANGE_MEDICAL_RESUME).FirstOrDefault().ParameterValue;

        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void SetEntityToControl()
        {
            StringBuilder sbNotes;
            string filterExpression;
            string planningNotes;

            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            hdnDepartmentID.Value = entityVisit.DepartmentID;

            _linkedVisitID = hdnLinkedVisitID.Value;

            if (string.IsNullOrEmpty(hdnMedicalResumeID.Value) || hdnMedicalResumeID.Value == "0")
            {
                hdnMedicalResumeID.Value = "0";
                _medicalResumeID = "0";
                hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

                GenerateDefaultMedicalResumeData();

                hdnIsCasemixRevision.Value = "0";
            }
            else
            {
                vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(string.Format("ID = {0} AND IsDeleted = 0", hdnMedicalResumeID.Value)).FirstOrDefault();
                if (obj != null)
                {
                    _medicalResumeID = obj.ID.ToString();
                    hdnMedicalResumeID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtResumeDate.Text = obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtResumeTime.Text = obj.MedicalResumeTime;
                    txtSubjectiveResumeText.Text = obj.SubjectiveResumeText;
                    txtObjectiveResumeText.Text = obj.ObjectiveResumeText;
                    txtAssessmentResumeText.Text = obj.AssessmentResumeText;
                    txtMedicalResumeText.Text = obj.MedicalResumeText;
                    txtMedicationResumeText.Text = obj.MedicationResumeText;
                    txtDischargeMedicationResumeText.Text = obj.DischargeMedicationResumeText;
                    txtPlanningResumeText.Text = obj.PlanningResumeText;
                    txtInstructionResumeText.Text = obj.InstructionResumeText;

                    rblIsHasSickLetter.SelectedValue = obj.IsHasSickLetter ? "1" : "0";
                    txtNoOfDays.Text = obj.NoOfAbsenceDays.ToString();

                    hdnIsCasemixRevision.Value = obj.IsCasemixRevision ? "1" : "0";
                    hdnRevisionParamedicID.Value = obj.RevisedByParamedicID.ToString();

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
        }

        private void GenerateDefaultMedicalResumeData()
        {
            StringBuilder sbNotes;
            string filterExpression;
            string planningNotes;

            //New Medical Resume
            vChiefComplaint sourceCC = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", hdnVisitID.Value)).FirstOrDefault();
            if (sourceCC != null)
            {
                txtResumeDate.Text = sourceCC.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResumeTime.Text = sourceCC.ObservationTime;

                txtSubjectiveResumeText.Text = sourceCC.ChiefComplaintText;

                sbNotes = new StringBuilder();

                sbNotes.AppendLine(sourceCC.ChiefComplaintText);
                if (!string.IsNullOrEmpty(sourceCC.HPISummary))
                {
                    sbNotes.AppendLine("");
                    sbNotes.AppendLine(sourceCC.HPISummary);
                }

                hdnSubjectiveText.Value = sourceCC.ChiefComplaintText;

                planningNotes = sourceCC.PlanningSummary;

                #region Objective Content
                sbNotes = new StringBuilder();
                vVitalSignHd vitalSignHd = BusinessLayer.GetvVitalSignHdList(string.Format("VisitID = {0} ORDER BY ID DESC", hdnVisitID.Value)).FirstOrDefault();
                if (vitalSignHd != null)
                {
                    List<vVitalSignDt> lstVitalSign = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} ORDER BY DisplayOrder", vitalSignHd.ID));

                    if (lstVitalSign.Count > 0)
                    {
                        foreach (vVitalSignDt vitalSign in lstVitalSign)
                        {
                            sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                        }
                    }
                }

                sbNotes.AppendLine(" ");

                vReviewOfSystemHd reviewOfSystemHd = BusinessLayer.GetvReviewOfSystemHdList(string.Format("VisitID = {0} AND GCParamedicMasterType = '{1}' ORDER BY ID DESC", hdnVisitID.Value, Constant.ParamedicType.Physician)).FirstOrDefault();
                if (reviewOfSystemHd != null)
                {
                    List<vReviewOfSystemDt> lstReviewOfSystem = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID = {0} AND IsDeleted = 0 ORDER BY TagProperty", reviewOfSystemHd.ID));

                    if (lstReviewOfSystem.Count > 0)
                    {
                        foreach (vReviewOfSystemDt item in lstReviewOfSystem)
                        {
                            sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                        }
                    }
                }

                txtObjectiveResumeText.Text = sbNotes.ToString();
                hdnObjectiveText.Value = sbNotes.ToString();
                #endregion

                #region Assessment Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", hdnVisitID.Value);
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
                hdnAssessmentText.Value = sbNotes.ToString();
                #endregion

                #region Planning Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND IsDeleted=0", hdnVisitID.Value, AppSession.UserLogin.ParamedicID);
                List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
                if (lstOrder.Count > 0)
                {
                    foreach (vTestOrderDt item in lstOrder)
                    {
                        sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                    }
                }

                txtPlanningResumeText.Text = sbNotes.ToString();
                hdnPlanningText.Value = sbNotes.ToString();
                #endregion

                #region Medication Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("RegistrationID = {0} AND ParamedicID = {1} AND GCTransactionStatus != '{2}' AND GCPrescriptionType IN ('{3}','{4}')", AppSession.RegisteredPatient.RegistrationID, AppSession.UserLogin.ParamedicID, Constant.TransactionStatus.VOID, Constant.PrescriptionType.MEDICATION_ORDER, Constant.PrescriptionType.CITO);
                vPrescriptionOrderHd entityHd = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression).FirstOrDefault();
                if (entityHd != null)
                {
                    List<vPrescriptionOrderDt> lstPrescription = BusinessLayer.GetvPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entityHd.PrescriptionOrderID));
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
                }
                txtMedicationResumeText.Text = sbNotes.ToString();
                hdnMedicationResumeText.Value = sbNotes.ToString();
                #endregion

                #region Discharge Medication Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("RegistrationID = {0} AND ParamedicID = {1} AND GCTransactionStatus != '{2}' AND GCPrescriptionType = '{3}'", AppSession.RegisteredPatient.RegistrationID, AppSession.UserLogin.ParamedicID, Constant.TransactionStatus.VOID, Constant.PrescriptionType.DISCHARGE_PRESCRIPTION);
                vPrescriptionOrderHd entityHdDischarge = BusinessLayer.GetvPrescriptionOrderHdList(filterExpression).FirstOrDefault();
                if (entityHdDischarge != null)
                {
                    List<vPrescriptionOrderDt> lstPrescriptionDischarge = BusinessLayer.GetvPrescriptionOrderDtList(string.Format("PrescriptionOrderID = {0} AND IsDeleted = 0", entityHdDischarge.PrescriptionOrderID));
                    if (lstPrescriptionDischarge.Count > 0)
                    {
                        foreach (vPrescriptionOrderDt item in lstPrescriptionDischarge)
                        {
                            if (item.IsRFlag)
                                sbNotes.AppendLine(string.Format("- {0} {1}x{2}", item.ItemName1, item.Frequency, item.NumberOfDosage.ToString("G29")));
                            else
                                sbNotes.AppendLine(string.Format("  {0}", item.ItemName1));
                        }
                    }
                }
                txtDischargeMedicationResumeText.Text = sbNotes.ToString();
                hdnDischargeMedicationResumeText.Value = sbNotes.ToString();
                #endregion

                #region Tindakan yang dilakukan
                sbNotes = new StringBuilder();
                sbNotes.AppendLine("Tindakan :");
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value);
                List<vPatientProcedure> lstProcedure = BusinessLayer.GetvPatientProcedureList(filterExpression);
                if (lstProcedure.Count > 0)
                {
                    foreach (vPatientProcedure item in lstProcedure)
                    {
                        sbNotes.AppendLine(string.Format("- {0}", item.ProcedureText));
                    }
                }

                sbNotes.AppendLine("");
                sbNotes.AppendLine("Catatan Tindakan :");
                sbNotes.AppendLine(string.Format("{0}", planningNotes));

                txtMedicalResumeText.Text = sbNotes.ToString();
                hdnMedicalResumeText.Value = sbNotes.ToString();
                #endregion

                #region Instruksi
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID IN ({0}) AND PhysicianID = {1} AND IsDeleted = 0 ORDER BY PatientInstructionID", hdnVisitID.Value, AppSession.UserLogin.ParamedicID);
                List<PatientInstruction> lstEntity = BusinessLayer.GetPatientInstructionList(filterExpression);

                StringBuilder instructionText = new StringBuilder();
                foreach (PatientInstruction obj in lstEntity)
                {
                    instructionText.AppendLine(string.Format("{0}", obj.Description));
                }

                hdnInstructionText.Value = instructionText.ToString();
                txtInstructionResumeText.Text = hdnInstructionText.Value;
                #endregion

                hdnIsChanged.Value = "1";
                hdnIsSaved.Value = "0";
            }
        }

        protected override void SetControlProperties()
        {
       }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            MedicalResumeDao medicalResumeDao = new MedicalResumeDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);

            MedicalResume entity = null;
            bool isNewMedicalResume = true;
            string objectiveText = string.Empty;

            objectiveText = txtObjectiveResumeText.Text;

            if (hdnMedicalResumeID.Value != "" && hdnMedicalResumeID.Value != "0")
            {
                entity = medicalResumeDao.Get(Convert.ToInt32(hdnMedicalResumeID.Value));
                isNewMedicalResume = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new MedicalResume();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.MedicalResumeDate = Helper.GetDatePickerValue(txtResumeDate);
            entity.MedicalResumeTime = txtResumeTime.Text;
            entity.SubjectiveResumeText = txtSubjectiveResumeText.Text;
            entity.ObjectiveResumeText = objectiveText;
            entity.AssessmentResumeText = txtAssessmentResumeText.Text;
            entity.MedicationResumeText = txtMedicationResumeText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
            entity.DischargeMedicationResumeText = txtDischargeMedicationResumeText.Text;
            entity.InstructionResumeText = txtInstructionResumeText.Text;

            entity.IsHasSickLetter = rblIsHasSickLetter.SelectedValue == "1";
            if (!string.IsNullOrEmpty(txtNoOfDays.Text))
            {
                entity.NoOfAbsenceDays = Convert.ToInt32(txtNoOfDays.Text);
            }

            if (isNewMedicalResume)
            {
                entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                hdnParamedicID.Value = entity.ParamedicID.ToString();
                _medicalResumeID = medicalResumeDao.InsertReturnPrimaryKeyID(entity).ToString();
                hdnMedicalResumeID.Value = _medicalResumeID.ToString();
            }
            else
            {
                medicalResumeDao.Update(entity);
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                bool result = true;

                if ((hdnMedicalResumeID.Value != "" && hdnMedicalResumeID.Value != "0") && !IsValidToSave(ref message))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    //MedicalResume obj = BusinessLayer.GetMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                    //if (obj != null)
                    //    hdnMedicalResumeID.Value = obj.ID.ToString();
                    //else
                    //    hdnMedicalResumeID.Value = "0";

                    UpdateConsultVisitRegistration(ctx);
                    ctx.CommitTransaction();

                    if (hdnMedicalResumeID.Value != "0")
                    {
                        hdnIsSaved.Value = "1";
                        hdnIsChanged.Value = "0";
                    }
                    else
                    {
                        hdnIsSaved.Value = "0";
                        hdnIsChanged.Value = "1";
                    }

                    message = hdnMedicalResumeID.Value;
                }
                catch (Exception ex)
                {
                    result = false;
                    message = ex.Message;
                    hdnIsSaved.Value = "0";
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }

                return result;
            }
            return true;
        }

        private bool IsValidToSave(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if ((hdnParamedicID.Value != "" && hdnParamedicID.Value != "0") || (hdnRevisionParamedicID.Value != "" && hdnRevisionParamedicID.Value != "0"))
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                int revisionParamedicID = 0;

                if (!String.IsNullOrEmpty(hdnRevisionParamedicID.Value) && hdnRevisionParamedicID.Value != "0")
                {
                    revisionParamedicID = Convert.ToInt32(hdnRevisionParamedicID.Value);
                }

                if (hdnIsCasemixAllowChange.Value == "0")
                {
                    if ((AppSession.UserLogin.ParamedicID != paramedicID))
                    {
                        errMsg.AppendLine("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian atau Revisi Pengkajian");
                    }
                    else if (revisionParamedicID != 0)
                    {
                        if (AppSession.UserLogin.ParamedicID != revisionParamedicID)
                        {
                            errMsg.AppendLine("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian atau Revisi Pengkajian");
                        }
                    }
                }
            }
            errMessage = errMsg.ToString();

            return (errMessage == string.Empty);
        }

        protected void cbpGenerate_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            try
            {
                if (param[0] == "generate")
                {
                    GenerateDefaultMedicalResumeData();
                    result = param[0] + string.Format("|1|{0}|{1}|{2}|{3}|{4}|{5}|{6}", hdnSubjectiveText.Value,hdnObjectiveText.Value, hdnAssessmentText.Value, hdnPlanningText.Value,hdnMedicationResumeText.Value, hdnMedicalResumeText.Value, hdnInstructionText.Value);
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}
