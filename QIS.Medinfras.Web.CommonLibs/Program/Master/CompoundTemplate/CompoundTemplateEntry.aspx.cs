using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class CompoundTemplateEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String moduleID = hdnModuleID.Value;
            switch (moduleID)
            {
                case Constant.Module.PHARMACY: return Constant.MenuCode.Pharmacy.COMPOUND_TEMPLATE;
                case Constant.Module.EMR: return Constant.MenuCode.EMR.COMPOUND_TEMPLATE;
                default: return Constant.MenuCode.EMR.COMPOUND_TEMPLATE;
            }
        }

        protected String GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String[] param = Request.QueryString["id"].Split('|');
            hdnModuleID.Value = param[0];
            if (param.Length > 1)
            {
                IsAdd = false;
                hdnID.Value = param[1];
                SetControlProperties();
                CompoundTemplateHd entity = BusinessLayer.GetCompoundTemplateHd(Convert.ToInt32(hdnID.Value));
                EntityToControl(entity);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }
            txtTemplateCode.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<EmbalaceHd> lstEmbalace = BusinessLayer.GetEmbalaceHdList("IsDeleted = 0");
            lstEmbalace.Insert(0, new EmbalaceHd() { EmbalaceID = 0, EmbalaceName = "" });
            Methods.SetComboBoxField<EmbalaceHd>(cboEmbalace, lstEmbalace, "EmbalaceName", "EmbalaceID");
            cboEmbalace.SelectedIndex = 0;

            cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;
            cboMedicationRoute.SelectedIndex = 0;

            txtDispenseQty.Text = "1";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtTemplateCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTemplateName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY));
            SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAsRequired, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboEmbalace, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtEmbalaceQty, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMedicationPurpose, new ControlEntrySetting(true, true, false));        
        }

        private void EntityToControl(CompoundTemplateHd entity)
        {
            txtTemplateCode.Text = entity.CompoundTemplateCode;
            txtTemplateName.Text = entity.CompoundTemplateName;
            txtFrequencyNumber.Text = entity.Frequency.ToString();
            cboFrequencyTimeline.Value = entity.GCDosingFrequency;
            txtDosingDose.Text = entity.NumberOfDosage.ToString();
            cboDosingUnit.Value = entity.GCDosingUnit;
            txtDosingDuration.Text = entity.DosingDuration.ToString();
            txtDispenseQty.Text = entity.DispenseQuantity.ToString();
            if (entity.EmbalaceID != null)
	        {
		       cboEmbalace.Value = entity.EmbalaceID.ToString();
               txtEmbalaceQty.Text = entity.EmbalaceQty.ToString();
	        }
            cboMedicationRoute.Value = entity.GCMedicationRoute;
            if (entity.GCCoenamRule != null)
                cboCoenamRule.Value = entity.GCCoenamRule;
            txtMedicationAdministration.Text = entity.MedicationAdministration;
            txtMedicationPurpose.Text = entity.MedicationPurpose;
            chkIsUsingSweetener.Checked = entity.IsUseSweetener;
            chkIsAsRequired.Checked = entity.IsAsRequired;
        }

        private void ControlToEntity(CompoundTemplateHd entity)
        {
            entity.CompoundTemplateCode = txtTemplateCode.Text;
            entity.CompoundTemplateName = txtTemplateName.Text;
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
            entity.DispenseQuantity = Convert.ToDecimal(txtDispenseQty.Text);
            if (!String.IsNullOrEmpty(cboEmbalace.Text))
            {
                entity.EmbalaceID = Convert.ToInt16(cboEmbalace.Value);
                entity.EmbalaceQty = Convert.ToDecimal(txtEmbalaceQty.Text);
            }
            else
            {
                entity.EmbalaceID = null;
                entity.EmbalaceQty = 0;
            }

            entity.GCMedicationRoute = cboMedicationRoute.Value.ToString();
            if (!String.IsNullOrEmpty(cboCoenamRule.Text))
                entity.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRule.Value.ToString()) ? cboCoenamRule.Value.ToString() : null;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.MedicationPurpose = txtMedicationAdministration.Text;
            entity.IsUseSweetener = chkIsUsingSweetener.Checked;
            entity.IsAsRequired = chkIsAsRequired.Checked;
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("CompoundTemplateCode = '{0}'", txtTemplateCode.Text);
            List<CompoundTemplateHd> lst = BusinessLayer.GetCompoundTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Compound Prescription Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("CompoundTemplateCode = '{0}' AND CompoundTemplateID != {1}", txtTemplateCode.Text, hdnID.Value);
            List<CompoundTemplateHd> lst = BusinessLayer.GetCompoundTemplateHdList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Compound Prescription Template With Code " + txtTemplateCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            CompoundTemplateHdDao entityDao = new CompoundTemplateHdDao(ctx);
            bool result = false;
            try
            {
                CompoundTemplateHd entity = new CompoundTemplateHd();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityDao.Insert(entity);
                retval = BusinessLayer.GetCompoundTemplateHdMaxID(ctx).ToString();
                ctx.CommitTransaction();
                result = true;
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = ex.Message;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                CompoundTemplateHd entity = BusinessLayer.GetCompoundTemplateHd(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCompoundTemplateHd(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
    }
}