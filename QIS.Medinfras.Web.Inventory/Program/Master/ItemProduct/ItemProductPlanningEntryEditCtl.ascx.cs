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
    public partial class ItemProductPlanningEntryEditCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            string[] paramInfo = param.Split('|');

            hdnItemPlanningID.Value = paramInfo[0];
            hdnConversionFactor.Value = "1";

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
            SetControlEntrySetting(txtAveragePrice, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtBasePrice, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPurchasePrice, new ControlEntrySetting(true, true, true, "0"));
        }

        private void EntityToControl(vItemPlanning entity)
        {
            cboPurchaseUnit.Value = entity.GCPurchaseUnit;
            txtAveragePrice.Text = entity.AveragePrice.ToString();
            txtBasePrice.Text = entity.UnitPrice.ToString();
            txtPurchasePrice.Text = entity.PurchaseUnitPrice.ToString();
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
            entity.UnitPrice = Convert.ToDecimal(txtBasePrice.Text);
            entity.PurchaseUnitPrice = Convert.ToDecimal(txtPurchasePrice.Text);
            entity.AveragePrice = Convert.ToDecimal(txtAveragePrice.Text);
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

                if (entity.AveragePrice != 0 && entity.UnitPrice != 0)
                {
                    entityDao.Update(entity);

                    //////ditutup oleh RN (20210603) karna pindah ke trigger onItemPlanningChangedInsertItemPriceHistoryV2
                    //////dibuka lagi oleh RN (202112-04) lalu trigger onItemPlanningChangedInsertItemPriceHistoryV2 dibuat disable
                    BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("IP", entity.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Maaf, Harga Rata-rata & Harga Satuan Kecil != 0";
                    ctx.RollBackTransaction();
                }
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