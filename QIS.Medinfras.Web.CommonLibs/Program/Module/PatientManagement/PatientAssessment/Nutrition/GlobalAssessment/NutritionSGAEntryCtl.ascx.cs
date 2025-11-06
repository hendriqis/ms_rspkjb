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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class NutritionSGAEntryCtl : BasePagePatientPageEntryCtl
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
                NutritionSGA entity = BusinessLayer.GetNutritionSGA(Convert.ToInt32(hdnID.Value));
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

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.DIET_TYPE, Constant.StandardCode.DIAGNOSIS_NUTRITION_SEVERITY, Constant.StandardCode.NUTRITION_PHYSICAL_RATING));
            Methods.SetComboBoxField<StandardCode>(cboDietType, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DIET_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDiagnosisSeverity, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DIAGNOSIS_NUTRITION_SEVERITY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCArmMuscle, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_PHYSICAL_RATING).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCSubcutaneousFat, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_PHYSICAL_RATING).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCAnkleEdema, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_PHYSICAL_RATING).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCSacralEdema, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_PHYSICAL_RATING).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCAscites, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_PHYSICAL_RATING).ToList(), "StandardCodeName", "StandardCodeID");

            cboDietType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboDietType, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(NutritionSGA entity)
        {
            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();
            rblIsWeightChanged.SelectedValue = entity.IsWeightChanged ? "1" : "0";
            if (entity.IsWeightChanged)
            {
                txtWeightBefore.Text = entity.PreviousWeight.ToString();
                txtWeightCurrent.Text = entity.CurrentWeight.ToString();
                txtWeightPercentage.Text = entity.WeightChangedPercentage.ToString();
                txtWeightChangedPeriod.Text = entity.WeightChangedPeriod.ToString();
            }
            else
            {
                txtWeightBefore.Text = string.Empty;
                txtWeightCurrent.Text = string.Empty;
                txtWeightPercentage.Text = string.Empty;
                txtWeightChangedPeriod.Text = string.Empty;
            }

            rblIsIntakeChanged.SelectedValue = entity.IsFoodIntakeChanged ? "1" : "0";
            chkIsSubOptimumFood.Checked = entity.IsSubOptimum;
            chkIsHypoCaloric.Checked = entity.IsHypoCaloric;
            chkIsOnlyLiquids.Checked = entity.IsOnlyLiquids;
            chkIsStarvation.Checked = entity.IsStarvation;
            if (entity.IsFoodIntakeChanged)
                txtFoodIntakeChangedPeriod.Text = entity.FoodIntakePeriod.ToString();
            else
                txtFoodIntakeChangedPeriod.Text = string.Empty;

            rblIsGastroChanged.SelectedValue = entity.IsGastroChanged ? "1" : "0";
            chkIsNausea.Checked = entity.IsNausea;
            chkIsVomitting.Checked = entity.IsVomitting;
            chkIsDiarrhea.Checked = entity.IsDiarrhea;
            chkIsAnorexia.Checked = entity.IsAnorexia;

            if (entity.IsGastroChanged)
                txtGastroChangedPeriod.Text = entity.GastroChangedPeriod.ToString();
            else
                txtGastroChangedPeriod.Text = string.Empty;

            rblIsActivityChanged.SelectedValue = entity.IsActivityChanged ? "1" : "0";
            chkIsSubOptimumActivity.Checked = entity.IsSubOptimalActivity;
            chkIsAmbulatory.Checked = entity.IsAmbulatory;
            chkIsBedRest.Checked = entity.IsBedrest;

            if (entity.IsActivityChanged)
                txtActivityChangedPeriod.Text = entity.ActivityChangedPeriod.ToString();
            else
                txtActivityChangedPeriod.Text = string.Empty;

            txtDiagnosisText.Text = entity.DiagnosisText;
            if (entity.GCDiseaseRelatedSeverity != null)
                cboGCDiagnosisSeverity.Value = entity.GCDiseaseRelatedSeverity;

            if (entity.GCSubcutaneousFat != null)
                cboGCSubcutaneousFat.Value = entity.GCSubcutaneousFat;
            if (entity.GCArmMuscle != null)
                cboGCArmMuscle.Value = entity.GCArmMuscle;
            if (entity.GCAnkleEdema != null)
                cboGCAnkleEdema.Value = entity.GCAnkleEdema;
            if (entity.GCSacralEdema != null)
                cboGCSacralEdema.Value = entity.GCSacralEdema;
            if (entity.GCAscites != null)
                cboGCAscites.Value = entity.GCAscites;

            rblIsFoodAllergy.SelectedValue = entity.IsHasFoodAllergy ? "1" : "2";
            txtFoodAllergenName.Text = entity.FoodAllergenName;

            rblSGAScore.SelectedValue = entity.SGARating;

            chkIsUsingDietMenu.Checked = entity.IsUseStandardDietMenu;
            if (entity.GCDietType != null)
                cboDietType.Value = entity.GCDietType;

            chkIsNeedAssessment.Checked = entity.IsNeedNutritionAssessment;
        }

        private void ControlToEntity(NutritionSGA entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;

            entity.IsWeightChanged = rblIsWeightChanged.SelectedValue == "1";

            if (!string.IsNullOrEmpty(txtWeightBefore.Text))
            {
                entity.PreviousWeight = Convert.ToDecimal(txtWeightBefore.Text);
            }

            if (!string.IsNullOrEmpty(txtWeightCurrent.Text))
            {
                entity.CurrentWeight = Convert.ToDecimal(txtWeightCurrent.Text);
            }

            if (!string.IsNullOrEmpty(txtWeightChangedPeriod.Text))
                entity.WeightChangedPeriod = Convert.ToInt16(txtWeightChangedPeriod.Text);
            else
                entity.WeightChangedPeriod = 0;

            if (!string.IsNullOrEmpty(txtWeightPercentage.Text))
                entity.WeightChangedPercentage = Convert.ToDecimal(txtWeightPercentage.Text);

            entity.IsFoodIntakeChanged = rblIsIntakeChanged.SelectedValue == "1";
            entity.IsSubOptimum = chkIsSubOptimumFood.Checked;
            entity.IsHypoCaloric = chkIsHypoCaloric.Checked;
            entity.IsOnlyLiquids = chkIsOnlyLiquids.Checked;
            entity.IsStarvation = chkIsStarvation.Checked;

            if (!string.IsNullOrEmpty(txtFoodIntakeChangedPeriod.Text))
                entity.FoodIntakePeriod = Convert.ToInt16(txtFoodIntakeChangedPeriod.Text);
            else
                entity.FoodIntakePeriod = 0;

            entity.IsGastroChanged = rblIsGastroChanged.SelectedValue == "1";
            entity.IsNausea = chkIsNausea.Checked;
            entity.IsVomitting = chkIsVomitting.Checked;
            entity.IsDiarrhea = chkIsDiarrhea.Checked;
            entity.IsAnorexia = chkIsAnorexia.Checked;
            if (!string.IsNullOrEmpty(txtGastroChangedPeriod.Text))
                entity.GastroChangedPeriod = Convert.ToInt16(txtGastroChangedPeriod.Text);
            else
                entity.GastroChangedPeriod = 0;

            entity.IsActivityChanged = rblIsActivityChanged.SelectedValue == "1";
            entity.IsSubOptimalActivity = chkIsSubOptimumActivity.Checked;
            entity.IsAmbulatory = chkIsAmbulatory.Checked;
            entity.IsBedrest = chkIsBedRest.Checked;
            if (!string.IsNullOrEmpty(txtActivityChangedPeriod.Text))
                entity.ActivityChangedPeriod = Convert.ToInt16(txtActivityChangedPeriod.Text);
            else
                entity.ActivityChangedPeriod = 0;

            entity.DiagnosisText = txtDiagnosisText.Text;
            if (cboGCDiagnosisSeverity.Value != null)
                entity.GCDiseaseRelatedSeverity = cboGCDiagnosisSeverity.Value.ToString();

            if (cboGCSubcutaneousFat.Value != null)
                entity.GCSubcutaneousFat = cboGCSubcutaneousFat.Value.ToString();
            if (cboGCArmMuscle.Value != null)
                entity.GCArmMuscle = cboGCArmMuscle.Value.ToString();
            if (cboGCAnkleEdema.Value != null)
                entity.GCAnkleEdema = cboGCAnkleEdema.Value.ToString();
            if (cboGCSacralEdema.Value != null)
                entity.GCSacralEdema = cboGCSacralEdema.Value.ToString();
            if (cboGCAscites.Value != null)
                entity.GCAscites = cboGCAscites.Value.ToString();

            entity.IsHasFoodAllergy = rblIsFoodAllergy.SelectedValue == "1";
            entity.FoodAllergenName = txtFoodAllergenName.Text;

            entity.SGARating = rblSGAScore.SelectedValue;

            entity.IsUseStandardDietMenu = chkIsUsingDietMenu.Checked;
            if (cboDietType.Value != null)
                entity.GCDietType = cboDietType.Value.ToString();

            entity.IsNeedNutritionAssessment = chkIsNeedAssessment.Checked;

        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionSGA entity = new NutritionSGA();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionSGA(entity);
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
                NutritionSGA entityUpdate = BusinessLayer.GetNutritionSGA(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionSGA(entityUpdate);
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