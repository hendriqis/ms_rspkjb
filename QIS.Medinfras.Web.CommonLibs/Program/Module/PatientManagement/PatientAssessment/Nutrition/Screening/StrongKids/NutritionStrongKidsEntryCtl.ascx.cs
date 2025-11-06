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
    public partial class NutritionStrongKidsEntryCtl : BasePagePatientPageEntryCtl
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
                NutritionalStrongKidsAssessment entity = BusinessLayer.GetNutritionalStrongKidsAssessment(Convert.ToInt32(hdnID.Value));
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
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));

            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
        }

        private void EntityToControl(NutritionalStrongKidsAssessment entity)
        {
            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();
            rblIsSkinny.SelectedValue = entity.IsSkinny ? "1" : "0";
            if (entity.IsSkinny)
            {
                txtIsSkinny.Text = "1";
            }
            else
            {
                txtIsSkinny.Text = "0";
            }
            rblIsWeightChanged.SelectedValue = entity.IsWeightChanged ? "1" : "0";
            if (entity.IsWeightChanged)
            {
                txtIsWeightChanged.Text = "1";
            }
            else
            {
                txtIsWeightChanged.Text = "0";
            }
            rblIsSpecificCondition.SelectedValue = entity.IsSpecificCondition ? "1" : "0";
            if (entity.IsSpecificCondition)
            {
                txtIsSpecificCondition.Text = "1";
            }
            else
            {
                txtIsSpecificCondition.Text = "0";
            }
            rblIsMalnutrition.SelectedValue = entity.IsMalnutrition ? "1" : "0";
            if (entity.IsMalnutrition)
            {
                txtIsMalnutrition.Text = "2";
            }
            else
            {
                txtIsMalnutrition.Text = "0";
            }
            txtTotalScore.Text = Convert.ToString(entity.TotalScore);
        }

        private void ControlToEntity(NutritionalStrongKidsAssessment entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.IsSkinny = rblIsSkinny.SelectedValue == "1";
            entity.IsWeightChanged = rblIsWeightChanged.SelectedValue == "1";
            entity.IsSpecificCondition = rblIsSpecificCondition.SelectedValue == "1";
            entity.IsMalnutrition = rblIsMalnutrition.SelectedValue == "2";
            entity.TotalScore = Convert.ToInt16(Request.Form[txtTotalScore.UniqueID]);
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionalStrongKidsAssessment entity = new NutritionalStrongKidsAssessment();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionalStrongKidsAssessment(entity);
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
                NutritionalStrongKidsAssessment entityUpdate = BusinessLayer.GetNutritionalStrongKidsAssessment(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionalStrongKidsAssessment(entityUpdate);
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