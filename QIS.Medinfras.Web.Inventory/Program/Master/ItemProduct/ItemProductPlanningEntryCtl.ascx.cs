using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemProductPlanningEntryCtl : BaseEntryPopupCtl
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

            string[] paramInfo = param.Split('|');

            hdnItemPlanningID.Value = paramInfo[0];
            txtFactorXMin.Text = paramInfo[1];
            txtFactorXMax.Text = paramInfo[2];
            hdnConversionFactor.Value = "1";

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                        AppSession.UserLogin.HealthcareID, //0
                        Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED //1
            ));

            hdnIsUsingPurchaseDiscountShared.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED).FirstOrDefault().ParameterValue;

            if (hdnIsUsingPurchaseDiscountShared.Value == "1")
            {
                trPurchaseDiscountShared.Attributes.Remove("style");
            }
            else
            {
                trPurchaseDiscountShared.Attributes.Add("style", "display:none");
            }

            vItemPlanning entity = BusinessLayer.GetvItemPlanningList(string.Format("ID = {0}", hdnItemPlanningID.Value)).FirstOrDefault();
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemName.Text = entity.ItemCode + " | " + entity.ItemName1;
            txtHealthcareName.Text = entity.HealthcareName;

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE ItemID = {1} AND IsDeleted = 0 AND IsActive = 1) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, entity.ItemID));
            Methods.SetComboBoxField<StandardCode>(cboPurchaseUnit, lstSc, "StandardCodeName", "StandardCodeID");
            cboPurchaseUnit.SelectedIndex = 0;

            EntityToControl(entity);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsUsingSupplierCatalog, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtLeadTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTolerance, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSafetyTime, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeFence, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSafetyStock, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAveragePrice, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtBasePrice, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtPurchasePrice, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtInpatientPOPrecentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtOutpatientPOPrecentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPurchaseDiscountSharedInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPatientDiscountSharedInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtMinOrderQty, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtMaxOrderQty, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(rblROP, new ControlEntrySetting(true, true, false, "false"));
            SetControlEntrySetting(txtBackward, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtForward, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vItemPlanning entity)
        {
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            chkIsUsingSupplierCatalog.Checked = entity.IsUsingSupplierCatalog;
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
            txtLastPurchaseKecilInfo.Text = string.Format("{0} per {1}",
                                            entity.LastPurchasePrice.ToString(Constant.FormatString.NUMERIC_2), entity.ItemUnit);
            txtLastPurchaseBesarInfo.Text = string.Format("{0} per {1}",
                                            entity.LastPurchaseUnitPrice.ToString(Constant.FormatString.NUMERIC_2), entity.LastPurchaseUnitDesc);
            txtLastPurchaseDiscount.Text = entity.LastPurchaseDiscount.ToString();
            txtLastPurchaseDiscount2.Text = entity.LastPurchaseDiscount2.ToString();
            txtInpatientPOPrecentage.Text = entity.InpatientPOPercentage.ToString();
            txtOutpatientPOPrecentage.Text = entity.OutpatientPOPercentage.ToString();
            txtPurchaseDiscountSharedInPercentage.Text = entity.PurchaseDiscountSharedInPercentage.ToString();
            txtPatientDiscountSharedInPercentage.Text = entity.PatientDiscountSharedInPercentage.ToString();
            if (entity.IsUsingDynamicROP)
            {
                rblROP.SelectedValue = "true";
            }
            else
            {
                rblROP.SelectedValue = "false";
            }
            txtBackward.Text = entity.BackwardDays.ToString();
            txtForward.Text = entity.ForwardDays.ToString();
            if (entity.GCPurchaseUnit != "" && entity.GCPurchaseUnit != entity.GCItemUnit)
            {
                List<vItemAlternateUnit> altUnitLst = BusinessLayer.GetvItemAlternateUnitList(string.Format("ItemID = {0} AND GCAlternateUnit = '{1}' AND IsDeleted = 0 AND IsActive = 1", entity.ItemID, entity.GCPurchaseUnit));
                if (altUnitLst.Count > 0)
                {
                    hdnConversionFactor.Value = altUnitLst.FirstOrDefault().ConversionFactor.ToString();
                }
                else
                {
                    hdnConversionFactor.Value = "1";
                }
            }
        }

        private void ControlToEntity(ItemPlanning entity)
        {
            if (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0")
                entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            else
                entity.BusinessPartnerID = null;
            entity.IsUsingSupplierCatalog = chkIsUsingSupplierCatalog.Checked;
            entity.LeadTime = Convert.ToByte(txtLeadTime.Text);
            entity.SafetyTime = Convert.ToByte(txtSafetyTime.Text);
            entity.TimeFence = Convert.ToByte(txtTimeFence.Text);
            entity.SafetyStock = Convert.ToDecimal(txtSafetyStock.Text);
            entity.UnitPrice = Convert.ToDecimal(txtBasePrice.Text);
            entity.PurchaseUnitPrice = Convert.ToDecimal(txtPurchasePrice.Text);
            entity.AveragePrice = Convert.ToDecimal(txtAveragePrice.Text);
            entity.MinOrderQty = Convert.ToDecimal(txtMinOrderQty.Text);
            entity.MaxOrderQty = Convert.ToDecimal(txtMaxOrderQty.Text);
            entity.GCPurchaseUnit = cboPurchaseUnit.Value == null ? null : cboPurchaseUnit.Value.ToString();
            entity.IsUsingDynamicROP = Convert.ToBoolean(rblROP.SelectedValue);
            entity.BackwardDays = Convert.ToByte(txtBackward.Text);
            entity.ForwardDays = Convert.ToByte(txtForward.Text);
            if (entity.LastPurchasePrice == 0)
            {
                entity.LastPurchaseUnitPrice = entity.PurchaseUnitPrice;
                entity.LastPurchasePrice = entity.LastPurchaseUnitPrice / Convert.ToDecimal(hdnConversionFactor.Value);
            }
            entity.IsPriceLastUpdatedBySystem = false;
            entity.InpatientPOPercentage = Convert.ToDecimal(txtInpatientPOPrecentage.Text);
            entity.OutpatientPOPercentage = Convert.ToDecimal(txtOutpatientPOPrecentage.Text);
            entity.PurchaseDiscountSharedInPercentage = Convert.ToDecimal(txtPurchaseDiscountSharedInPercentage.Text);
            entity.PatientDiscountSharedInPercentage = Convert.ToDecimal(txtPatientDiscountSharedInPercentage.Text);
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemPlanningDao entityDao = new ItemPlanningDao(ctx);
            try
            {
                ItemPlanning entity = entityDao.Get(Convert.ToInt32(hdnItemPlanningID.Value));

                decimal oOldAveragePrice = entity.AveragePrice;
                decimal oOldUnitPrice = entity.UnitPrice;
                decimal oOldPurchasePrice = entity.PurchaseUnitPrice;
                bool oOldIsPriceLastUpdatedBySystem = entity.IsPriceLastUpdatedBySystem;
                bool oOldIsDeleted = entity.IsDeleted;

                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                //if (entity.AveragePrice != 0 && entity.UnitPrice != 0)
                //{
                entityDao.Update(entity);

                //////ditutup oleh RN (20210603) karna pindah ke trigger onItemPlanningChangedInsertItemPriceHistoryV2
                //////dibuka lagi oleh RN (202112-04) lalu trigger onItemPlanningChangedInsertItemPriceHistoryV2 dibuat disable
                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("IP", entity.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                ctx.CommitTransaction();
                //}
                //else
                //{
                //    result = false;
                //    errMessage = "Maaf, Harga Rata-rata & Harga Satuan Kecil != 0";
                //    ctx.RollBackTransaction();
                //}
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
    }
}