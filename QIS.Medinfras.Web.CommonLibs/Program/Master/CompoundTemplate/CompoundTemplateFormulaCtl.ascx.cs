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
    public partial class CompoundTemplateFormulaCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            #region Header
            hdnCompoundTemplateID.Value = param;
            vCompoundTemplateHd entityHd = BusinessLayer.GetvCompoundTemplateHdList(string.Format("CompoundTemplateID = {0}", hdnCompoundTemplateID.Value)).FirstOrDefault();
            txtTemplateCode.Text = entityHd.CompoundTemplateCode;
            txtTemplateName.Text = entityHd.CompoundTemplateName;
            txtFrequencyNumber.Text = entityHd.Frequency.ToString();
            txtFrequencyTimeline.Text = entityHd.DosingFrequency;
            txtDosingDose.Text = entityHd.NumberOfDosage.ToString();
            txtDosingUnit.Text = entityHd.DosingUnit;
            txtDosingDuration.Text = entityHd.DosingDuration.ToString();
            txtDispenseQty.Text = entityHd.DispenseQuantity.ToString();
            if (entityHd.EmbalaceID != null && entityHd.EmbalaceID != 0)
            {
                hdnEmbalanceID.Value = entityHd.EmbalaceID.ToString();
                EmbalaceHd entityEmbalance = BusinessLayer.GetEmbalaceHd(entityHd.EmbalaceID);
                txtEmbalace.Text = entityEmbalance.EmbalaceName;
                txtEmbalaceQty.Text = entityHd.EmbalaceQty.ToString();
            }
            txtMedicationRoute.Text = entityHd.MedicationRoute;
            if (entityHd.GCCoenamRule != null)
            {
                txtCoenamRule.Text = entityHd.CoenamRule;
            }
            txtMedicationAdministration.Text = entityHd.MedicationAdministration;
            txtMedicationPurpose.Text = entityHd.MedicationPurpose;
            chkIsUsingSweetener.Checked = entityHd.IsUseSweetener;
            #endregion

            #region Detail
            BindGridView();
            #endregion

            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }

        protected string OnGetItemMasterFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES);
        }

        protected void cboCompoundUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            if (hdnItemID.Value != null && hdnItemID.Value.ToString() != "")
            {
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format(
                    "ParentID = '{0}' AND (StandardCodeID IN ( (SELECT GCDoseUnit FROM vDrugInfo WHERE ItemID = {1}),(SELECT GCItemUnit FROM vDrugInfo WHERE ItemID = {2})))",
                    Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, hdnItemID.Value));
                Methods.SetComboBoxField<StandardCode>(cboCompoundUnit, lst, "StandardCodeName", "StandardCodeID");
                cboCompoundUnit.SelectedIndex = 1;
            }
        }

        private void BindGridView()
        {
            string filterExpression = String.Format("CompoundTemplateID = {0} AND ItemID != 0 ORDER BY DisplayOrder ASC",hdnCompoundTemplateID.Value);

            List<vCompoundTemplateDt> lstEntity = BusinessLayer.GetvCompoundTemplateDtList(filterExpression);
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
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(CompoundTemplateDt entity)
        {
            entity.CompoundTemplateID = Int32.Parse(hdnCompoundTemplateID.Value);
            entity.ItemID = Int32.Parse(hdnItemID.Value);
            entity.CompoundQty = 0;
            entity.CompoundQtyInString = txtCompoundQuantity.Text;
            entity.GCCompoundUnit = cboCompoundUnit.Value.ToString();
            entity.ConversionFactor = Decimal.Parse(Request.Form[txtConversionFactor.UniqueID]);
            entity.ResultQuantity = Decimal.Parse(Request.Form[txtResultQty.UniqueID]);
            entity.DisplayOrder = Int16.Parse(txtDisplayOrder.Text);
        }

        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                CompoundTemplateDt entity = new CompoundTemplateDt();
                ControlToEntity(entity);
                BusinessLayer.InsertCompoundTemplateDt(entity);
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
                CompoundTemplateDt entity = BusinessLayer.GetCompoundTemplateDt(Convert.ToInt32(hdnCompoundTemplateID.Value), Convert.ToInt32(hdnItemID.Value));
                ControlToEntity(entity);
                BusinessLayer.UpdateCompoundTemplateDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                CompoundTemplateDt entity = BusinessLayer.GetCompoundTemplateDt(Convert.ToInt32(hdnCompoundTemplateID.Value), Convert.ToInt32(hdnItemID.Value));
                BusinessLayer.DeleteCompoundTemplateDt(entity.CompoundTemplateID, entity.ItemID);
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