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

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class NutritionCareNotesEntryCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] parameter = param.Split('|');
            if (parameter[0] == "add")
            {
                IsAdd = true;
                hdnVisitID.Value = parameter[1];
                hdnID.Value = "";
                SetControlProperties();
            }
            else if (parameter[0] == "edit")
            {
                IsAdd = false;
                hdnVisitID.Value = parameter[1];
                hdnID.Value = parameter[2];
                SetControlProperties();
                NutritionCareNotes entity = BusinessLayer.GetNutritionCareNotes(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            txtAssessmentDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'",
                Constant.ParamedicType.Nutritionist, AppSession.UserLogin.ParamedicID));
            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
            if (AppSession.UserLogin.ParamedicID != null)
                cboParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            else
                cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.DIET_TYPE, Constant.StandardCode.MEAL_FORM, Constant.StandardCode.DIET_ROUTE));
            Methods.SetComboBoxField<StandardCode>(cboGCDietType, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DIET_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCMealForm, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.MEAL_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCRoute, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DIET_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboGCDietType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCMealForm, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCRoute, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NutritionCareNotes entity)
        {
            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();

            txtAntropometri.Text = entity.Antropometri;
            txtReviewOfSystem.Text = entity.ReviewOfSystem;
            txtBioChemical.Text = entity.BioChemical;
            txtDietPrincipal.Text = entity.DietPrincipal;
            txtInterventionPurposes.Text = entity.InterventionPurposes;
            txtEtiologi.Text = entity.Etiologi;
            txtSymptoms.Text = entity.Symptoms;
            txtNutritionHistory.Text = entity.NutritionHistory;
            txtPersonalHistory.Text = entity.PersonalHistory;
            txtCouncelingMaterial.Text = entity.CouncelingMaterial;
            txtCouncelingMedia.Text = entity.CouncelingMedia;
            txtCouncelingGoal.Text = entity.CouncelingGoal;
            txtEvaluationPlanning.Text = entity.EvaluationPlanning;
            txtDiagnoseID.Text = entity.DiagnoseID;
            Diagnose entityDiagnose = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID = '{0}'", entity.DiagnoseID)).FirstOrDefault();
            if (entityDiagnose != null)
            {
                txtDiagnoseName.Text = entityDiagnose.DiagnoseName;
            }

            if (entity.GCDietType != null)
                cboGCDietType.Value = entity.GCDietType;
            if (entity.GCMealForm != null)
                cboGCMealForm.Value = entity.GCMealForm;
            if (entity.GCRoute != null)
                cboGCRoute.Value = entity.GCRoute;
        }

        private void ControlToEntity(NutritionCareNotes entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;

            entity.Antropometri = txtAntropometri.Text;
            entity.ReviewOfSystem = txtReviewOfSystem.Text;
            entity.BioChemical = txtBioChemical.Text;
            entity.DietPrincipal = txtDietPrincipal.Text;
            entity.InterventionPurposes = txtInterventionPurposes.Text;
            entity.Etiologi = txtEtiologi.Text;
            entity.Symptoms = txtSymptoms.Text;
            entity.NutritionHistory = txtNutritionHistory.Text;
            entity.PersonalHistory = txtPersonalHistory.Text;
            entity.CouncelingMaterial = txtCouncelingMaterial.Text;
            entity.CouncelingMedia = txtCouncelingMedia.Text;
            entity.CouncelingGoal = txtCouncelingGoal.Text;
            entity.EvaluationPlanning = txtEvaluationPlanning.Text;
            entity.DiagnoseID = txtDiagnoseID.Text;

            if (cboGCDietType.Value != null)
                entity.GCDietType = cboGCDietType.Value.ToString();
            if (cboGCMealForm.Value != null)
                entity.GCMealForm = cboGCMealForm.Value.ToString();
            if (cboGCRoute.Value != null)
                entity.GCRoute = cboGCRoute.Value.ToString();

        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionCareNotes entity = new NutritionCareNotes();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionCareNotes(entity);
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
            bool result = true;
            try
            {
                NutritionCareNotes entityUpdate = BusinessLayer.GetNutritionCareNotes(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionCareNotes(entityUpdate);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result = false;
            }
            return result;
        }
    }
}