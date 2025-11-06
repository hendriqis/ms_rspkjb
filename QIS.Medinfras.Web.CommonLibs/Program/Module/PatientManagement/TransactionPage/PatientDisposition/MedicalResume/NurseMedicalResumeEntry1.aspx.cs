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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NurseMedicalResumeEntry1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected static string _medicalResumeID = "0";
        protected static string _linkedVisitID;

        public override string OnGetMenuCode()
        {
            if (hdnMenuType.Value == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (hdnDeptType.Value)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSE_MEDICAL_RESUME;
                    default:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_NURSE_MEDICAL_RESUME;
                }
                #endregion
            }
            else
            {
                switch (hdnDeptType.Value)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.NURSE_MEDICAL_RESUME;
                    default:
                        return Constant.MenuCode.Inpatient.NURSE_MEDICAL_RESUME;
                }
            }
        }

        protected string GetUserID()
        {
            return AppSession.UserLogin.UserID.ToString();
        }

        protected string GetMedicalResumeID()
        {
            return _medicalResumeID;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                hdnDeptType.Value = param[0];
                hdnMenuType.Value = param[1];
                hdnVisitID.Value = param[2];
                hdnMedicalResumeID.Value = param[3];
            }
            else
            {
                hdnDeptType.Value = param[0];
            }

            txtResumeDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtResumeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            SetEntityToControl();

            hdnMedicalResumeID.Value = _medicalResumeID;

        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        private void SetEntityToControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnRegistrationID.Value = entityVisit.RegistrationID.ToString();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);
            hdnDepartmentID.Value = entityVisit.DepartmentID;

            //cboPatientOutcome.Value = entityVisit.GCDischargeCondition;
            //cboDischargeRoutine.Value = entityVisit.GCDischargeMethod;

            if (string.IsNullOrEmpty(hdnMedicalResumeID.Value) || hdnMedicalResumeID.Value == "0")
            {
                hdnMedicalResumeID.Value = "0";
                hdnParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

                //New Medical Resume
                txtResumeDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResumeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }
            else
            {
                vNurseMedicalResume obj = BusinessLayer.GetvNurseMedicalResumeList(string.Format("ID = {0} AND IsDeleted = 0", hdnMedicalResumeID.Value)).FirstOrDefault();
                if (obj != null)
                {
                    _medicalResumeID = obj.ID.ToString();
                    hdnMedicalResumeID.Value = obj.ID.ToString();
                    hdnParamedicID.Value = obj.ParamedicID.ToString();
                    txtResumeDate.Text = obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtResumeTime.Text = obj.MedicalResumeTime;
                    txtSubjectiveResumeText.Text = obj.SubjectiveResumeText;
                    txtEvaluationResumeText.Text = obj.EvaluationResumeText;
                    txtPlanningResumeText.Text = obj.PlanningResumeText;

                    txtDischargeMedicationResumeText.Text = obj.DischargeMedicationResumeText;
                    txtNutritionistResumeText.Text = obj.NutritionistResumeText;

                    if (obj.IsPatientFamily)
                        trFamilyInfo.Style.Add("display", "table-row");
                    else
                        trFamilyInfo.Style.Add("display", "none");

                    rblIsGeriatric.SelectedValue = obj.AgeInYear > 60 ? "1" : "0";
                    rblIsImmobility.SelectedValue = obj.IsImmobility ? "1" : "0";
                    rblIsNeedFollowupCare.SelectedValue = obj.IsNeedFollowupCare ? "1" : "0";
                    rblIsHasDependency.SelectedValue = obj.IsHasDependency ? "1" : "0";
                    cboDischargeTransportation.Value = obj.GCDischargeTransportation;
                    txtDischargeMedicalSummary.Text = obj.DischargeMedicalResumeText;
                    txtHomecarePIC.Text = obj.HomecarePIC;

                    string[] instructionInfo = obj.InstructionResumeText.Split('|');
                    txtInstructionResumeText1.Text = instructionInfo[0];
                    txtInstructionResumeText2.Text = instructionInfo[1];
                    txtEducationSummaryText.Text = obj.EducationSummaryText;
                    txtFamilyName.Text = obj.FamilyName;
                    rblIsPatientFamily.SelectedValue = obj.IsPatientFamily ? "1" : "0";
                    cboFamilyRelation.Value = obj.GCFamilyRelation;

                    hdnIsChanged.Value = "0";
                    hdnIsSaved.Value = "0";
                }
            }
        }

        protected override void SetControlProperties()
        {
            string Department = hdnDepartmentID.Value;
            string filterExpression = string.Format("ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
            Constant.StandardCode.DISCHARGE_TRANSPORTATION, Constant.StandardCode.FAMILY_RELATION);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstSc2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboDischargeTransportation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_TRANSPORTATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFamilyRelation, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.FAMILY_RELATION).ToList(), "StandardCodeName", "StandardCodeID");

            int paramedicID = AppSession.UserLogin.ParamedicID != null ? Convert.ToInt32(AppSession.UserLogin.ParamedicID) : 0;
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format(
                                                    "GCParamedicMasterType IN ('{0}','{1}') AND ParamedicID = {2}",
                                                    Constant.ParamedicType.Nurse, Constant.ParamedicType.Bidan, paramedicID));
            Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");

            if (AppSession.UserLogin.ParamedicID != 0 && AppSession.UserLogin.ParamedicID != null)
            {
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
                cboParamedicID.ClientEnabled = false;
            }
            else
            {
                cboParamedicID.SelectedIndex = 0;
            }
        }


        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            NurseMedicalResumeDao medicalResumeDao = new NurseMedicalResumeDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            NurseMedicalResume entity = null;
            bool isNewMedicalResume = true;
            string objectiveText = string.Empty;

            if (hdnMedicalResumeID.Value != "" && hdnMedicalResumeID.Value != "0")
            {
                entity = medicalResumeDao.Get(Convert.ToInt32(hdnMedicalResumeID.Value));
                isNewMedicalResume = false;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
            }
            else
            {
                entity = new NurseMedicalResume();
                entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }

            entity.MedicalResumeDate = Helper.GetDatePickerValue(txtResumeDate);
            entity.MedicalResumeTime = txtResumeTime.Text;
            entity.SubjectiveResumeText = txtSubjectiveResumeText.Text;
            entity.ObjectiveResumeText = objectiveText;
            entity.EvaluationResumeText = txtEvaluationResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
            entity.DischargeMedicationResumeText = txtDischargeMedicationResumeText.Text;
            entity.DischargeMedicalResumeText = txtDischargeMedicalSummary.Text;
            entity.NutritionistResumeText = txtNutritionistResumeText.Text;
            entity.InstructionResumeText = string.Format("{0}|{1}",txtInstructionResumeText1.Text, txtInstructionResumeText2.Text);
            entity.EducationSummaryText = txtEducationSummaryText.Text;
            entity.IsPatientFamily = rblIsPatientFamily.SelectedValue == "1" ? true : false;

            if (entity.IsPatientFamily)
            {
                entity.FamilyName = txtFamilyName.Text;
                if (cboFamilyRelation.Value != null)
                    if (!string.IsNullOrEmpty(cboFamilyRelation.Value.ToString()))
                        entity.GCFamilyRelation = cboFamilyRelation.Value.ToString();
            }
            entity.HomecarePIC = txtHomecarePIC.Text;

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

            if (hdnMedicalResumeID.Value != "0" && hdnMedicalResumeID.Value != "")
            {
                Registration oRegistration = registrationDao.Get(Convert.ToInt32(hdnRegistrationID.Value));
                if (oRegistration != null)
	            {
                    oRegistration.IsGeriatric = rblIsGeriatric.SelectedValue == "1" ? true : false;
                    oRegistration.IsImmobility = rblIsImmobility.SelectedValue == "1" ? true : false;
                    oRegistration.IsNeedFollowupCare = rblIsNeedFollowupCare.SelectedValue == "1" ? true : false;
                    oRegistration.IsHasDependency = rblIsHasDependency.SelectedValue == "1" ? true : false;
                    if (cboDischargeTransportation.Value != null)
                        if (!string.IsNullOrEmpty(cboDischargeTransportation.Value.ToString()))
                            oRegistration.GCDischargeTransportation = cboDischargeTransportation.Value.ToString();

                    registrationDao.Update(oRegistration);
	            }
            }
        }

        protected override bool OnCustomButtonClick(string type, ref string message)
        {
            if (type == "save")
            {
                bool result = true;

                if (!IsValid(ref message))
                {
                    result = false;
                    hdnIsSaved.Value = "0";
                    return result;                  
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    NurseMedicalResume obj = BusinessLayer.GetNurseMedicalResumeList(string.Format("VisitID = {0} AND IsDeleted = 0", hdnVisitID.Value), ctx).FirstOrDefault();
                    if (obj != null)
                        hdnMedicalResumeID.Value = obj.ID.ToString();
                    else
                        hdnMedicalResumeID.Value = "0";

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

        private bool IsValid(ref string errMessage)
        {
            StringBuilder errMsg = new StringBuilder();

            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
            {
                int paramedicID = Convert.ToInt32(hdnParamedicID.Value);
                if (AppSession.UserLogin.ParamedicID != paramedicID)
                {
                    errMsg.Append("Perubahan Kajian Awal Pasien hanya dapat dilakukan oleh Dokter yang melakukan Pengkajian|");
                }
            }

            if (string.IsNullOrEmpty(txtSubjectiveResumeText.Text))
            {
                errMsg.Append("Masalah Keperawatan tidak boleh kosong|");
            }
            if (string.IsNullOrEmpty(txtPlanningResumeText.Text))
            {
                errMsg.Append("Tindakan perawat selama masa keperawatan tidak boleh kosong|");
            }
            if (string.IsNullOrEmpty(txtEvaluationResumeText.Text))
            {
                errMsg.AppendLine("Evaluasi keperawatan tidak boleh kosong|");
            }

            errMessage = errMsg.ToString().Replace(@"|","<br />");

            return (errMessage == string.Empty);
        }
    }
}
