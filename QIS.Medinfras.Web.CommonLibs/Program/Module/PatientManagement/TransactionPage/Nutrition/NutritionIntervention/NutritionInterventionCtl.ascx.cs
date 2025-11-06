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
    public partial class NutritionInterventionCtl : BasePagePatientPageEntryCtl
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
                NutritionNotes entity = BusinessLayer.GetNutritionNotes(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID = {1}",
                Constant.ParamedicType.Nutritionist, AppSession.UserLogin.ParamedicID));
            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtOtherRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NutritionNotes entity)
        {
            entity.NotesDate = entity.NotesDate;
            entity.NotesTime = entity.NotesTime;
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            cboParamedicID.Value = entity.ParamedicID.ToString();
            txtOtherRemarks.Text = entity.OtherRemarks;
        }

        private void ControlToEntity(NutritionNotes entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.NotesDate = Helper.GetDatePickerValue(txtAssessmentDate);
            entity.NotesTime = txtAssessmentTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.OtherRemarks = txtOtherRemarks.Text;
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                NutritionNotes entity = new NutritionNotes();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertNutritionNotes(entity);
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
                NutritionNotes entityUpdate = BusinessLayer.GetNutritionNotes(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entityUpdate);
                entityUpdate.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateNutritionNotes(entityUpdate);
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