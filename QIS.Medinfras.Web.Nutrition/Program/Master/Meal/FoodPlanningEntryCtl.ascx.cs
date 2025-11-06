using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Nutrition.Program
{
    public partial class FoodPlanningEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        protected string OnGetSupplierFilterExpression()
        {
            return string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnItemPlanningID.Value = param;
            hdnConversionFactor.Value = "1";

            vItemPlanning entity = BusinessLayer.GetvItemPlanningList(string.Format("ID = {0}", hdnItemPlanningID.Value))[0];
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemName.Text = entity.ItemName1;
            txtHealthcareName.Text = entity.HealthcareName;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, entity.ItemID));
            lstSc.Insert(0,new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPurchaseUnit, lstSc, "StandardCodeName", "StandardCodeID");
            cboPurchaseUnit.SelectedIndex = 0;
            
            EntityToControl(entity);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtLeadTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTolerance, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSafetyTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeFence, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSafetyStock, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBasePrice, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMinOrderQty, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtMaxOrderQty, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vItemPlanning entity)
        {
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtLeadTime.Text = entity.LeadTime.ToString();
            txtTolerance.Text = entity.ToleranceQty.ToString();
            txtSafetyTime.Text = entity.SafetyTime.ToString();
            txtTimeFence.Text = entity.TimeFence.ToString();
            txtSafetyStock.Text = entity.SafetyStock.ToString();
            txtMinOrderQty.Text = entity.MinOrderQty.ToString();
            txtMaxOrderQty.Text = entity.MaxOrderQty.ToString();
            cboPurchaseUnit.Value = entity.GCPurchaseUnit;
            txtAveragePrice.Text = entity.AveragePrice.ToString();
            txtBasePrice.Text = entity.UnitPrice.ToString();
            txtPurchasePrice.Text = entity.PurchaseUnitPrice.ToString();
            if (entity.GCPurchaseUnit != "")
            {
                vItemAlternateUnit alternateUnit = BusinessLayer.GetvItemAlternateUnitList(string.Format("ItemID = {0} AND GCAlternateUnit = '{1}'", entity.ItemID, entity.GCPurchaseUnit)).FirstOrDefault();
                hdnConversionFactor.Value = alternateUnit.ConversionFactor.ToString();
            }
        }

        private void ControlToEntity(ItemPlanning entity)
        {
            if (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0")
                entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            else
                entity.BusinessPartnerID = null;
            entity.LeadTime = Convert.ToByte(txtLeadTime.Text);
            //entity.TolerancePercentage = Convert.ToDecimal(txtTolerance.Text);
            entity.SafetyTime = Convert.ToByte(txtSafetyTime.Text);
            entity.TimeFence = Convert.ToByte(txtTimeFence.Text);
            entity.SafetyStock = Convert.ToDecimal(txtSafetyStock.Text);
            entity.UnitPrice = Convert.ToDecimal(txtBasePrice.Text);
            entity.PurchaseUnitPrice = Convert.ToDecimal(txtPurchasePrice.Text);
            entity.AveragePrice = Convert.ToDecimal(txtAveragePrice.Text);
            entity.MinOrderQty = Convert.ToDecimal(txtMinOrderQty.Text);
            entity.MaxOrderQty = Convert.ToDecimal(txtMaxOrderQty.Text);
            entity.GCPurchaseUnit = cboPurchaseUnit.Value == null ? null : cboPurchaseUnit.Value.ToString();
            entity.IsPriceLastUpdatedBySystem = true;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            try
            {
                ItemPlanning entity = BusinessLayer.GetItemPlanning(Convert.ToInt32(hdnItemPlanningID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemPlanning(entity);
                return true;
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                errMessage = ex.Message;
                return false;
            }
        }
    }
}