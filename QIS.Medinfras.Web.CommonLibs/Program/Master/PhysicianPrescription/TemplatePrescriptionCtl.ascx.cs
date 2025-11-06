using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TemplatePrescriptionCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            #region Header
            hdnPrescriptionTemplateID.Value = param;
            vPrescriptionTemplateHD entityHd = BusinessLayer.GetvPrescriptionTemplateHDList(string.Format("PrescriptionTemplateID = {0} AND IsDeleted = 0", hdnPrescriptionTemplateID.Value)).FirstOrDefault();
            List<PrescriptionTemplateDt> lst = BusinessLayer.GetPrescriptionTemplateDtList(string.Format("PrescriptionTemplateID = {0} AND IsDeleted = 0", hdnPrescriptionTemplateID.Value));
            foreach (PrescriptionTemplateDt lstDt in lst)
            {
                if (lstDt != null)
                {
                    hdnPrescriptionTemplateDetailID.Value = lstDt.PrescriptionTemplateDetailID.ToString();
                }
            }
            txtTemplateCode.Text = entityHd.PrescriptionTemplateCode;
            txtTemplateName.Text = entityHd.PrescriptionTemplateName;
            #endregion

            #region Detail
            BindGridView();
            #endregion

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtItemName, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtFrequencyNumber, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboFrequencyTimeline, new ControlEntrySetting(true, true, true, Constant.DosingFrequency.DAY), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDosingDose, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDosingDuration, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDispenseQty, new ControlEntrySetting(true, true, true, "1"), "mpEntryPopup");
            Helper.SetControlEntrySetting(chkIsUsingSweetener, new ControlEntrySetting(true, true, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtMedicationAdministration, new ControlEntrySetting(true, true, false), "mpEntryPopup");

            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DOSING_FREQUENCY, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.COENAM_RULE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboFrequencyTimeline, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DOSING_FREQUENCY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && sc.TagProperty == "1" || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            cboFrequencyTimeline.Value = Constant.DosingFrequency.DAY;
            cboMedicationRoute.SelectedIndex = 0;

            txtDispenseQty.Text = "0";
        }

        protected string OnGetItemMasterFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
        }

        protected void cboDosingUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnDrugID.Value != null && hdnDrugID.Value.ToString() != "")
            {
                //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))", Constant.StandardCode.ITEM_UNIT, hdnDrugID.Value, hdnDrugID.Value));
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (TagProperty LIKE '%1%' OR TagProperty LIKE '%PRE%')", Constant.StandardCode.ITEM_UNIT));
                Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lst, "StandardCodeName", "StandardCodeID");
            }
        }

        private void BindGridView()
        {
            string filterExpression = String.Format("PrescriptionTemplateID = {0} AND ItemID != 0 AND OrderIsDeleted = 0", hdnPrescriptionTemplateID.Value);

            List<vPrescriptionTemplateDt> lstEntity = BusinessLayer.GetvPrescriptionTemplateDtList(filterExpression);
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        protected void lvwView_ItemDataBound(object sender, ListViewItemEventArgs e)
        {

        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";
            int prescriptiontemplateDetailID = 0;

            if (param == "save")
            {
                if (hdnIsFlagAdd.Value.ToString() != "1")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                prescriptiontemplateDetailID = Convert.ToInt32(hdnPrescriptionTemplateDetailID.Value);
                if (OnDeleteRecord(ref errMessage, prescriptiontemplateDetailID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(PrescriptionTemplateDt entity)
        {
            entity.IsRFlag = true;
            entity.PrescriptionTemplateID = Int32.Parse(hdnPrescriptionTemplateID.Value);
            entity.ItemID = Int32.Parse(hdnItemID.Value);
            entity.DrugName = Request.Form[txtItemName.UniqueID];
            entity.CompoundQty = 0;
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            if (!string.IsNullOrEmpty(cboMedicationRoute.Value.ToString()))
            {
                entity.GCRoute = cboMedicationRoute.Value.ToString();
            }
            else
            {
                entity.GCRoute = Constant.MedicationRoute.OTHER;
            }
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.DosingDuration = Convert.ToInt16(txtDosingDuration.Text);
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            if (!String.IsNullOrEmpty(cboCoenamRule.Text))
                entity.GCCoenamRule = !String.IsNullOrEmpty(cboCoenamRule.Value.ToString()) ? cboCoenamRule.Value.ToString() : null;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.MedicationPurpose = txtMedicationAdministration.Text;
            entity.IsUseSweetener = chkIsUsingSweetener.Checked;

            if (hdnSignaID.Value != "")
            {
                entity.SignaID = Convert.ToInt32(hdnSignaID.Value);
            }
            else
            {
                entity.SignaID = null;
            }
            string strengthUnit = hdnStrengthUnit.Value;
            string strengthAmount = hdnStrengthAmount.Value;
            if (!String.IsNullOrEmpty(strengthUnit))
            {
                entity.Dose = Convert.ToDecimal(strengthAmount);
                entity.GCDoseUnit = strengthUnit;
            }
            else
            {
                entity.Dose = 0;
                entity.GCDoseUnit = null;
            }
            entity.GCDosingFrequency = cboFrequencyTimeline.Value.ToString();
            entity.Frequency = Convert.ToInt16(txtFrequencyNumber.Text);
            entity.NumberOfDosage = Convert.ToDecimal(txtDosingDose.Text);
            entity.GCDosingUnit = cboDosingUnit.Value.ToString();
            entity.IsMorning = chkIsMorning.Checked;
            entity.IsNoon = chkIsNoon.Checked;
            entity.IsEvening = chkIsEvening.Checked;
            entity.IsNight = chkIsNight.Checked;
            entity.MedicationAdministration = txtMedicationAdministration.Text;
            entity.DosingDuration = Convert.ToDecimal(txtDosingDuration.Text);
            entity.IsAsRequired = chkIsAsRequired.Checked;
            entity.DispenseQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.TakenQty = Convert.ToDecimal(txtDispenseQty.Text);
            entity.ResultQty = entity.DispenseQty;
            entity.ChargeQty = entity.DispenseQty;
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {

            try
            {
                PrescriptionTemplateDt entity = new PrescriptionTemplateDt();
                ControlToEntity(entity);
                BusinessLayer.InsertPrescriptionTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PrescriptionTemplateDt entity = BusinessLayer.GetPrescriptionTemplateDt(Convert.ToInt32(hdnPrescriptionTemplateDetailID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage, int prescriptiontemplateDetailID)
        {
            try
            {
                PrescriptionTemplateDt entity = BusinessLayer.GetPrescriptionTemplateDt(Convert.ToInt32(prescriptiontemplateDetailID));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePrescriptionTemplateDt(entity);
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