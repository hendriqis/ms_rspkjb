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
    public partial class AdultScreeningEntryCtl : BasePagePatientPageEntryCtl
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
                NutritionalRiskScreening entity = BusinessLayer.GetNutritionalRiskScreening(Convert.ToInt32(hdnID.Value));
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

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.NUTRITION_DISRUPTION_STATUS, Constant.StandardCode.DISEASE_SEVERITY));
            Methods.SetComboBoxField<StandardCode>(cboGCNutritionDisruptionStatus, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.NUTRITION_DISRUPTION_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCDiseaseSeverity, lstStandardCode.Where(lst => lst.ParentID == Constant.StandardCode.DISEASE_SEVERITY).ToList(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboGCNutritionDisruptionStatus, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboGCDiseaseSeverity, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NutritionalRiskScreening entity)
        {
            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();
            rblIsBMIChanged.SelectedValue = entity.IsBMILessThanParameter ? "1" : "0";
            if (entity.IsBMILessThanParameter)
            {
                txtIsBMIChanged.Text = "1";
            }
            else
            {
                txtIsBMIChanged.Text = "0";
            }
            rblIsWeightChanged.SelectedValue = entity.IsLoseWeight ? "1" : "0";
            if (entity.IsLoseWeight)
            {
                txtIsWeightChanged.Text = "1";
            }
            else
            {
                txtIsWeightChanged.Text = "0";
            }
            rblIsIntakeDecreasing.SelectedValue = entity.IsIntakeDecrease ? "1" : "0";
            if (entity.IsIntakeDecrease)
            {
                txtIsIntakeDecreasing.Text = "1";
            }
            else
            {
                txtIsIntakeDecreasing.Text = "0";
            }
            rblIsSeverelySick.SelectedValue = entity.IsSeverelySick ? "1" : "0";
            if (entity.IsSeverelySick)
            {
                txtIsSeverelySick.Text = "1";
            }
            else
            {
                txtIsSeverelySick.Text = "0";
            }
            rblIsAgeMoreThan70.SelectedValue = entity.AgeLessThanParameter ? "1" : "0";
            if (entity.AgeLessThanParameter)
            {
                txtIsAgeMoreThan70.Text = "1";
            }
            else
            {
                txtIsAgeMoreThan70.Text = "0";
            }
            txtTotalScore.Text = entity.TotalScore.ToString();
            cboGCNutritionDisruptionStatus.Value = entity.GCNutritionDisruptionStatus;
            cboGCDiseaseSeverity.Value = entity.GCDiseaseSeverity;
        }

        private void ControlToEntity(NutritionalRiskScreening entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.IsBMILessThanParameter = rblIsBMIChanged.SelectedValue == "1";
            entity.IsLoseWeight = rblIsWeightChanged.SelectedValue == "1";
            entity.IsIntakeDecrease = rblIsIntakeDecreasing.SelectedValue == "1";
            entity.IsSeverelySick = rblIsSeverelySick.SelectedValue == "1";
            entity.AgeLessThanParameter = rblIsAgeMoreThan70.SelectedValue == "1";
            entity.TotalScore = Convert.ToInt16(Request.Form[txtTotalScore.UniqueID]);

            if (cboGCNutritionDisruptionStatus.Value != null)
                entity.GCNutritionDisruptionStatus = cboGCNutritionDisruptionStatus.Value.ToString();

            if (cboGCDiseaseSeverity.Value != null)
                entity.GCDiseaseSeverity = cboGCDiseaseSeverity.Value.ToString();
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionalRiskScreening entity = new NutritionalRiskScreening();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionalRiskScreening(entity);
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
                NutritionalRiskScreening entityUpdate = BusinessLayer.GetNutritionalRiskScreening(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionalRiskScreening(entityUpdate);
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