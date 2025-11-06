using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionalAssessmentEntry1 : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int gridVitalSignPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridROSPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        protected static string _visitNoteID;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Nutrition.NUTRITION_ASSESMENT;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetVisitNoteID()
        {
            return _visitNoteID;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (Page.Request.QueryString["id"] != null)
            {
                hdnID.Value = string.IsNullOrEmpty(Page.Request.QueryString["id"]) ? "0" : Page.Request.QueryString["id"];
            }
            else
            {
                hdnID.Value = "0";
            }

            txtServiceDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeNow1.Value = txtServiceTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
            hdnTimeNow2.Value = txtServiceTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                Constant.StandardCode.FAMILY_RELATION);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(p => p.ParentID == Constant.StandardCode.FAMILY_RELATION || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            vNutritionAssessment entity = BusinessLayer.GetvNutritionAssessmentList(string.Format("VisitID = {0} AND ID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value)).FirstOrDefault();
            PatientVisitNote oPatientVisitNote = new PatientVisitNote();

            if (entity != null)
            {
                oPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID IN ({0}) AND NutritionAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value)).FirstOrDefault();
                if (oPatientVisitNote != null)
                    hdnPatientVisitNoteID.Value = oPatientVisitNote.ID.ToString();
                else
                    hdnPatientVisitNoteID.Value = "0";

                _visitNoteID = hdnPatientVisitNoteID.Value;

                EntityToControl(entity, oPatientVisitNote);
            }
            else
            {
                _visitNoteID = "0";
                hdnPatientVisitNoteID.Value = "0";
            }

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            filterExpression = string.Empty;
            if (paramedicID != 0)
                filterExpression = string.Format("ParamedicID = {0}", paramedicID);
            else
                filterExpression = string.Format("GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(filterExpression);
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            bool isEnabled = true;
            if (entity != null)
            {
                isEnabled = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
            }

            Helper.SetControlEntrySetting(txtNutritionHistory, new ControlEntrySetting(false, false, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtAntropometricNotes, new ControlEntrySetting(true, true, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAutoAnamnesis, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(chkAlloAnamnesis, new ControlEntrySetting(true, true, false, false), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceDate, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime1, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(txtServiceTime2, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                {
                    if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                        cboParamedicID.Value = Convert.ToInt32(hdnAssessmentParamedicID.Value);
                    else
                        cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                else
                {
                    cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                }
                cboParamedicID.Enabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nurse
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Bidan
                    || AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                if (userLoginParamedic != 0 && userLoginParamedic != null)
                {
                    Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(false, false, true, userLoginParamedic), "mpPatientStatus");
                    hdnIsNotAllowNurseFillChiefComplaint.Value = "1";
                }
                else
                {
                    Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                    hdnIsNotAllowNurseFillChiefComplaint.Value = "0";
                }
            }
            else
            {
                Helper.SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(isEnabled, isEnabled, true), "mpPatientStatus");
                hdnIsNotAllowNurseFillChiefComplaint.Value = "0";
            }

            filterExpression = string.Format("RegistrationID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID);
            List<vParamedicTeam> lstPhysician = BusinessLayer.GetvParamedicTeamList(filterExpression);
            ParamedicMaster entityPM = BusinessLayer.GetParamedicMaster(AppSession.RegisteredPatient.ParamedicID);

            if (lstPhysician.Count() > 0)
            {
                if (hdnPatientVisitNoteID.Value != "0")
                {
                    if (oPatientVisitNote.ConfirmationPhysicianID != null)
                    {
                        lstPhysician.Add(new vParamedicTeam() { ParamedicID = entityPM.ParamedicID, ParamedicCode = entityPM.ParamedicCode, ParamedicName = entityPM.FullName });
                        Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
                        cboPhysician.Value = oPatientVisitNote.ConfirmationPhysicianID.ToString();
                    }
                    else
                    {
                        lstPhysician.Add(new vParamedicTeam() { ParamedicID = entityPM.ParamedicID, ParamedicCode = entityPM.ParamedicCode, ParamedicName = entityPM.FullName });
                        Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
                        cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
                    }
                }
                else
                {
                    lstPhysician.Add(new vParamedicTeam() { ParamedicID = entityPM.ParamedicID, ParamedicCode = entityPM.ParamedicCode, ParamedicName = entityPM.FullName });
                    Methods.SetComboBoxField(cboPhysician, lstPhysician, "ParamedicName", "ParamedicID");
                    cboPhysician.Value = AppSession.RegisteredPatient.ParamedicID.ToString();
                }
            }
            else
            {
                int physician = AppSession.RegisteredPatient.ParamedicID != null ? Convert.ToInt32(AppSession.RegisteredPatient.ParamedicID) : 0;
                List<vParamedicMaster> lstPhysician2 = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                        "ParamedicID = {0}", physician));
                Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstPhysician2, "ParamedicName", "ParamedicID");
            }
        }

        private void EntityToControl(vNutritionAssessment entity, PatientVisitNote oPatientVisitNote)
        {
            hdnAssessmentParamedicID.Value = entity.ParamedicID.ToString();

            if (entity.ID == 0)
            {
                hdnID.Value = "";
            }
            else
            {
                hdnID.Value = entity.ID.ToString();
            }

            if (entity.AssessmentDate != null && !string.IsNullOrEmpty(entity.AssessmentTime))
            {
                txtServiceDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime1.Text = entity.AssessmentTime.Substring(0, 2);
                txtServiceTime2.Text = entity.AssessmentTime.Substring(3, 2);
            }

            #region table sebelah kiri
            #region Nutrition Assessment
            txtNutritionHistory.Text = entity.NutritionHistory;
            txtAntropometricNotes.Text = entity.AntropometricNotes;
            txtBiochemistryNotes.Text = entity.BiochemistryNotes;
            txtPhysicalNotes.Text = entity.PhysicalNotes;
            txtMedicalHistory.Text = entity.MedicalHistory;
            txtProblem.Text = entity.Problem;
            txtEtiology.Text = entity.Etiology;
            txtSymptom.Text = entity.Symptoms;
            txtInterventionPurpose.Text = entity.InterventionGoal;
            txtInterventionDiet.Text = entity.NutritionDelivery;
            txtInterventionEducation.Text = entity.NutritionEducation;
            txtInterventionCollaboration.Text = entity.NutritionCounseling;
            txtMonitoring.Text = entity.Monitoring;
            txtEvaluation.Text = entity.Evaluation;
            cboParamedicID.Value = entity.ParamedicID.ToString();
            chkAutoAnamnesis.Checked = entity.IsAutoAnamnesis;
            chkAlloAnamnesis.Checked = entity.IsAlloAnamnesis;

            if (!string.IsNullOrEmpty(entity.GCFamilyRelation))
            {
                cboFamilyRelation.Value = entity.GCFamilyRelation;
            }
            chkIsPatientAllergyExists.Checked = !entity.IsPatientAllergyExists;

            #endregion
            #endregion

            if (oPatientVisitNote != null)
            {
                if (string.IsNullOrEmpty(txtInterventionCollaboration.Text))
                {
                    txtInterventionCollaboration.Text = oPatientVisitNote.PlanningText;
                    txtEvaluation.Text = oPatientVisitNote.InstructionText;
                }
                chkIsNeedConfirmation.Checked = oPatientVisitNote.IsNeedConfirmation;

                cboPhysician.Value = oPatientVisitNote.ConfirmationPhysicianID.ToString();
            }

            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
        }

        private string GetFormLayout(string typeName)
        {
            string filePath = HttpContext.Current.Server.MapPath("~/Libs/App_Data");
            string fileLocation = string.Empty;
            string formName = string.Empty;

            switch (typeName)
            {
                case "PsychoSocial":
                    fileLocation = @"medicalForm\Psychosocial";
                    formName = AppSession.OP0029;
                    break;
                case "Education":
                    fileLocation = @"medicalForm\Education";
                    formName = AppSession.OP0030;
                    break;
                case "Additional":
                    fileLocation = @"medicalForm\Population";
                    formName = "populationAssessment.html";
                    break;
                default:
                    fileLocation = @"medicalForm\PhysicalExam";
                    formName = string.Empty;
                    break;
            }

            string fileName = string.Format(@"{0}\{1}\{2}", filePath, fileLocation, formName);
            IEnumerable<string> lstText = File.ReadAllLines(fileName);
            StringBuilder innerHtml = new StringBuilder();

            foreach (string text in lstText)
            {
                innerHtml.AppendLine(text);
            }

            return innerHtml.ToString();
        }

        private void ControlToEntity(NutritionAssessment entity)
        {
            #region NutritionAssessment
            entity.NutritionHistory = txtNutritionHistory.Text;
            entity.AntropometricNotes = txtAntropometricNotes.Text;
            entity.BiochemistryNotes = txtBiochemistryNotes.Text;
            entity.PhysicalNotes = txtPhysicalNotes.Text;
            entity.MedicalHistory = txtMedicalHistory.Text;
            entity.Problem = txtProblem.Text;
            entity.Etiology = txtEtiology.Text;
            entity.Symptoms = txtSymptom.Text;
            entity.InterventionGoal = txtInterventionPurpose.Text;
            entity.NutritionEducation = txtInterventionEducation.Text;
            entity.NutritionDelivery = txtInterventionDiet.Text;
            entity.NutritionCounseling = txtInterventionCollaboration.Text;
            entity.Monitoring = txtMonitoring.Text;
            entity.Evaluation = txtEvaluation.Text;
            if (cboParamedicID.Value != null)
            {
                entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value);
            }
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            if (cboFamilyRelation.Value != null && entity.IsAlloAnamnesis)
            {
                entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
            }
            entity.IsPatientAllergyExists = Convert.ToBoolean(!chkIsPatientAllergyExists.Checked);

            if (hdnPatientVisitNoteID.Value != "0")
            {
                entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value); 
            }

            entity.AssessmentDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.AssessmentTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);


            entity.IsDeleted = false;

            #endregion
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            string nutritionAssessmentID = string.Empty;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                bool isAllowSave = true;
                try
                {
                    if (!string.IsNullOrEmpty(hdnAssessmentParamedicID.Value) && hdnAssessmentParamedicID.Value != "0")
                    {
                        if (Convert.ToInt32(hdnAssessmentParamedicID.Value) != AppSession.UserLogin.ParamedicID)
                            isAllowSave = false;
                    }

                    if (isAllowSave)
                    {
                        OnSaveAddEditPatientStatus(ctx, ref errMessage, ref nutritionAssessmentID);

                        PatientVisitNote entityNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, hdnPatientVisitNoteID.Value), ctx).FirstOrDefault();
                        bool isEntityNoteNull = false;
                        if (entityNote == null)
                        {
                            isEntityNoteNull = true;
                            entityNote = new PatientVisitNote();
                        }

                        entityNote.NutritionAssessmentID = Convert.ToInt32(nutritionAssessmentID);

                        NutritionAssessment entity = BusinessLayer.GetNutritionAssessmentList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, nutritionAssessmentID), ctx).FirstOrDefault();

                        ControlToEntity(entityNote);
                        entityNote.IsDeleted = false;

                        if (isEntityNoteNull)
                        {
                            entityNote.VisitID = AppSession.RegisteredPatient.VisitID;
                            entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                            entityNote.GCPatientNoteType = Constant.PatientVisitNotes.NUTRITION_INITIAL_ASSESSMENT;
                            entityNote.CreatedBy = AppSession.UserLogin.UserID;
                            entityNote.IsNeedConfirmation = Convert.ToBoolean(chkIsNeedConfirmation.Checked);
                            ///entityNote.ConfirmationPhysicianID = Convert.ToInt32(cboPhysician.Value);
                            if (Convert.ToBoolean(chkIsNeedConfirmation.Checked))
                            {
                                entityNote.ConfirmationPhysicianID = Convert.ToInt32(cboPhysician.Value);
                            }
                            else
                            {
                                entityNote.ConfirmationPhysicianID = null;
                            }
                            hdnPatientVisitNoteID.Value = patientVisitNoteDao.InsertReturnPrimaryKeyID(entityNote).ToString();
                        }
                        else
                        {
                            entityNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                            entityNote.IsNeedConfirmation = Convert.ToBoolean(chkIsNeedConfirmation.Checked);
                            ///entityNote.ConfirmationPhysicianID = Convert.ToInt32(cboPhysician.Value);
                            if (Convert.ToBoolean(chkIsNeedConfirmation.Checked))
                            {
                                entityNote.ConfirmationPhysicianID = Convert.ToInt32(cboPhysician.Value);
                            }
                            else
                            {
                                entityNote.ConfirmationPhysicianID = null;
                            }
                            entityNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                            patientVisitNoteDao.Update(entityNote);

                            hdnPatientVisitNoteID.Value = entityNote.ID.ToString();
                        }

                        errMessage = hdnPatientVisitNoteID.Value + ";" + nutritionAssessmentID;

                        ctx.CommitTransaction();

                        hdnIsSaved.Value = "1";
                        hdnIsChanged.Value = "0";
                    }
                    else
                    {
                        errMessage = "Maaf, Perubahan Pengkajian Pasien hanya bisa dilakukan oleh Ahli Gizi yang melakukan pengkajian pertama kali";
                        hdnIsChanged.Value = "0";

                        ctx.RollBackTransaction();
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    hdnIsSaved.Value = "0";
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }

        private void ControlToEntity(PatientVisitNote entityNote)
        {
            string adimeText = GenerateADIMEText();

            entityNote.NoteText = adimeText;
            entityNote.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entityNote.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
            entityNote.NoteTime = string.Format("{0}:{1}", txtServiceTime1.Text, txtServiceTime2.Text);
        }

        private string GenerateADIMEText()
        {
            StringBuilder sbNotes = new StringBuilder();
            StringBuilder sbSubjective = new StringBuilder();
            sbNotes.AppendLine("A:");
            sbNotes.AppendLine("-".PadRight(20, '-'));

            sbNotes.AppendLine(string.Format("Riwayat Gizi  : "));
            sbNotes.AppendLine(string.Format(" {0}   ", txtNutritionHistory.Text));

            StringBuilder sbVitalSign = new StringBuilder();            
            VitalSignHd lstVitalSignHd = BusinessLayer.GetVitalSignHdList(string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsDeleted = 0 ORDER BY ID", AppSession.RegisteredPatient.VisitID, hdnPatientVisitNoteID.Value)).FirstOrDefault();
            if (lstVitalSignHd == null)
            {
                sbNotes.AppendLine("");
            }
            else
            {
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("ID = {0} ORDER BY DisplayOrder", lstVitalSignHd.ID));
                if (lstVitalSignDt.Count > 0)
                {
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format("- {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }
            }

            if (!string.IsNullOrEmpty(txtAntropometricNotes.Text))
            {
                sbNotes.AppendLine(string.Format("Antropometri  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtAntropometricNotes.Text));

                sbNotes.AppendLine(string.Format(" {0}   ", sbVitalSign.ToString()));
            }
            if (!string.IsNullOrEmpty(txtBiochemistryNotes.Text))
            {
                sbNotes.AppendLine(string.Format("Biokimia  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtBiochemistryNotes.Text));
            }
            if (!string.IsNullOrEmpty(txtPhysicalNotes.Text))
            {
                sbNotes.AppendLine(string.Format("Klinik/Fisik  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtPhysicalNotes.Text));
            }
            if (!string.IsNullOrEmpty(txtMedicalHistory.Text))
            {
                sbNotes.AppendLine(string.Format("Riwayat Personal : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtMedicalHistory.Text));
            }

            sbNotes.AppendLine("D:");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            if (!string.IsNullOrEmpty(txtProblem.Text))
            {
                sbNotes.AppendLine(string.Format("Masalah  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtProblem.Text));
            }
            if (!string.IsNullOrEmpty(txtEtiology.Text))
            {
                sbNotes.AppendLine(string.Format("Etiologi  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtEtiology.Text));
            }
            if (!string.IsNullOrEmpty(txtSymptom.Text))
            {
                sbNotes.AppendLine(string.Format("Symtom  : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtSymptom.Text));
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("I :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            if (!string.IsNullOrEmpty(txtInterventionPurpose.Text))
            {
                sbNotes.AppendLine(string.Format("Tujuan : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtInterventionPurpose.Text));
            }
            if (!string.IsNullOrEmpty(txtInterventionDiet.Text))
            {
                sbNotes.AppendLine(string.Format("Pemberian/Penentuan Diet : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtInterventionDiet.Text));
            }
            if (!string.IsNullOrEmpty(txtInterventionEducation.Text))
            {
                sbNotes.AppendLine(string.Format("Edukasi/Konseling : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtInterventionEducation.Text));
            }
            if (!string.IsNullOrEmpty(txtInterventionCollaboration.Text))
            {
                sbNotes.AppendLine(string.Format("Kolaborasi/Rujukan Gizi : "));
                sbNotes.AppendLine(string.Format(" {0}   ", txtInterventionCollaboration.Text));
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("M :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(string.Format(" {0}", txtMonitoring.Text));

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("E :");
            sbNotes.AppendLine("-".PadRight(20, '-'));
            sbNotes.AppendLine(string.Format(" {0}", txtEvaluation.Text));

            return sbNotes.ToString();
        }

        private void OnSaveAddEditPatientStatus(IDbContext ctx, ref string errMessage, ref string nutritionAssessmentID)
        {
            NutritionAssessmentDao objDao = new NutritionAssessmentDao(ctx);

            bool isAssessmentNull = false;

            NutritionAssessment entity = null;
            if (hdnID.Value == "0")
            {
                isAssessmentNull = true;
                entity = new NutritionAssessment();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = objDao.Get(Convert.ToInt32(hdnID.Value));
            }

            ControlToEntity(entity);
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

 
            if (isAssessmentNull)
            {
                nutritionAssessmentID = objDao.InsertReturnPrimaryKeyID(entity).ToString();
            }
            else
            {
                objDao.Update(entity);
                nutritionAssessmentID = entity.ID.ToString();
            }
        }

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            string visitNoteID = !string.IsNullOrEmpty(_visitNoteID) ? _visitNoteID : "0";
            List<NutritionAssessment> lstNCC = BusinessLayer.GetNutritionAssessmentList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID));
            if (lstNCC.Count() > 0)
            {
                hdnID.Value = lstNCC.FirstOrDefault().ID.ToString();
            }
            else
            {
                hdnID.Value = "0";
            }

            filterExpression += string.Format("VisitID IN ({0}) AND NutritionAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND NutritionAssessmentID = {1} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, hdnID.Value));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}