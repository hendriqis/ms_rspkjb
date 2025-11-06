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

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class NutritionNotesCtl : BasePagePatientPageEntryCtl
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
                vNutritionistNotes entity = BusinessLayer.GetvNutritionistNotesList(string.Format("ID = {0}",Convert.ToInt32(hdnID.Value))).FirstOrDefault();
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
            SetControlEntrySetting(txtPersonalHistoryRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBioChemistryRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNutritionAndFoodHistory, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDiagnoseID, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(vNutritionistNotes entity)
        {
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtPersonalHistoryRemarks.Text = entity.PersonalHistoryRemarks;
            txtBioChemistryRemarks.Text = entity.BioChemistryRemarks;
            txtNutritionAndFoodHistory.Text = entity.NutritionAndFoodHistory;
            txtDiagnoseID.Text = entity.DiagnosisID;
            txtDiagnoseName.Text = entity.DiagnoseName;
        }

        private void ControlToEntity(NutritionistNotes entity)
        {
            entity.AssessmentDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.AssessmentTime = txtAssessmentTime.Text;
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.PersonalHistoryRemarks = txtPersonalHistoryRemarks.Text;
            entity.BioChemistryRemarks = txtBioChemistryRemarks.Text;
            entity.NutritionAndFoodHistory = txtNutritionAndFoodHistory.Text;
            entity.DiagnosisID = txtDiagnoseID.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionistNotes entity = new NutritionistNotes();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionistNotes(entity);
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
                NutritionistNotes entityUpdate = BusinessLayer.GetNutritionistNotes(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionistNotes(entityUpdate);
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