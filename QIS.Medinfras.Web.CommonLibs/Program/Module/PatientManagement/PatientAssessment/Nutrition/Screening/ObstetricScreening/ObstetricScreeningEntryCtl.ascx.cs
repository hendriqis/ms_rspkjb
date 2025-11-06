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
    public partial class ObstetricScreeningEntryCtl : BasePagePatientPageEntryCtl
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
                NutritionScreeningObstetric entity = BusinessLayer.GetNutritionScreeningObstetric(Convert.ToInt32(hdnID.Value));
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
        }

        private void EntityToControl(NutritionScreeningObstetric entity)
        {
            txtAssessmentDate.Text = entity.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtAssessmentTime.Text = entity.AssessmentTime;
            hdnParamedicID.Value = entity.NutritionistID.ToString();
            cboParamedicID.Value = entity.NutritionistID.ToString();
            rblIsIntakeDecrease.SelectedValue = entity.IsIntakeDecrease ? "1" : "0";
            rblIsMetabolismProblem.SelectedValue = entity.IsMetabolismProblem ? "1" : "0";
            rblIsWeightIncrease.SelectedValue = entity.IsWeightIncrease ? "1" : "0";
            rblIsHbLessThan10.SelectedValue = entity.IsHbLessThanParameter ? "1" : "0";
        }

        private void ControlToEntity(NutritionScreeningObstetric entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NutritionistID = Convert.ToInt16(cboParamedicID.Value.ToString());
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.IsIntakeDecrease = rblIsIntakeDecrease.SelectedValue == "1";
            entity.IsMetabolismProblem = rblIsMetabolismProblem.SelectedValue == "1";
            entity.IsWeightIncrease = rblIsWeightIncrease.SelectedValue == "1";
            entity.IsHbLessThanParameter = rblIsHbLessThan10.SelectedValue == "1";
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionScreeningObstetric entity = new NutritionScreeningObstetric();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionScreeningObstetric(entity);
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
                NutritionScreeningObstetric entityUpdate = BusinessLayer.GetNutritionScreeningObstetric(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionScreeningObstetric(entityUpdate);
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