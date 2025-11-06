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
                NutritionNotes entity = BusinessLayer.GetNutritionNotes(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
        }

        private void SetControlProperties()
        {
            //List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})",
            //    Constant.ParamedicType.Nutritionist, AppSession.RegisteredPatient.HealthcareServiceUnitID));
            //Methods.SetComboBoxField<vParamedicMaster>(cboParamedicID, lstParamedic, "ParamedicName", "ParamedicID");
            //cboParamedicID.SelectedIndex = 0;

            //if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Nutritionist)
            //{
                //int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                //cboParamedicID.ClientEnabled = false;
                //cboParamedicID.Value = userLoginParamedic.ToString();
            //}

            //List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND SpecialtyID = {1} AND ParamedicID = {2}",
            //    Constant.ParamedicType.Nurse, Constant.Specialty.Nutritionist, AppSession.UserLogin.ParamedicID));

            List<ParamedicMaster> lstParamedic = BusinessLayer.GetParamedicMasterList(string.Format("GCParamedicMasterType IN ('{0}','{1}') AND ParamedicID = {2}",
                Constant.ParamedicType.Nurse, Constant.ParamedicType.Nutritionist, AppSession.UserLogin.ParamedicID));

            Methods.SetComboBoxField<ParamedicMaster>(cboParamedicID, lstParamedic, "FullName", "ParamedicID");
            cboParamedicID.SelectedIndex = 0;

            List<StandardCode> lstDietType = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DIET_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboDietType, lstDietType, "StandardCodeName", "StandardCodeID");
            cboDietType.SelectedIndex = 0;

            List<StandardCode> lstMealForm = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.MEAL_FORM));
            Methods.SetComboBoxField<StandardCode>(cboMealForm, lstMealForm, "StandardCodeName", "StandardCodeID");
            cboMealForm.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtAssessmentDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtAssessmentTime, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
            SetControlEntrySetting(cboParamedicID, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(cboDietType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboMealForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtNoOfCalories, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkAllowMilk, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkAllowTea, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkAllowCoffee, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkVegetarian, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProhibitionRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOtherRemarks, new ControlEntrySetting(true, true, false));
        }

        private void EntityToControl(NutritionNotes entity)
        {
            hdnParamedicID.Value = entity.ParamedicID.ToString();
            cboParamedicID.Value = entity.ParamedicID.ToString();
            cboDietType.Value = entity.GCDietType;
            cboMealForm.Value = entity.GCMealForm;
            txtNoOfCalories.Text = entity.NoOfCalories.ToString();
            chkAllowMilk.Checked = entity.IsAllowMilk;
            chkAllowCoffee.Checked = entity.IsAllowCoffe;
            chkAllowTea.Checked = entity.IsAllowTea;
            chkVegetarian.Checked = entity.IsVegetarian;
            txtProhibitionRemarks.Text = entity.ProhibitionRemarks;
            txtOtherRemarks.Text = entity.OtherRemarks;
        }

        private void ControlToEntity(NutritionNotes entity)
        {
            entity.VisitID = Convert.ToInt32(hdnVisitID.Value);
            entity.ParamedicID = Convert.ToInt32(cboParamedicID.Value.ToString());
            entity.GCDietType = cboDietType.Value.ToString();
            entity.GCMealForm = cboMealForm.Value.ToString();
            entity.NoOfCalories = txtNoOfCalories.Text;
            entity.IsAllowMilk = chkAllowMilk.Checked;
            entity.IsAllowCoffe = chkAllowCoffee.Checked;
            entity.IsAllowTea = chkAllowTea.Checked;
            entity.IsVegetarian = chkVegetarian.Checked;
            entity.ProhibitionRemarks = txtProhibitionRemarks.Text;
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