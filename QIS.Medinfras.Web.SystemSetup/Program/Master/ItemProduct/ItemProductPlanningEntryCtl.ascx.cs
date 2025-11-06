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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class ItemProductPlanningEntryCtl : BaseEntryPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnItemPlanningID.Value = param;

            vItemPlanning entity = BusinessLayer.GetvItemPlanningList(string.Format("ID = {0}", hdnItemPlanningID.Value))[0];
            txtItemName.Text = entity.ItemName1;
            txtHealthcareName.Text = entity.HealthcareName;

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
            txtBasePrice.Text = entity.UnitPrice.ToString();
            txtMinOrderQty.Text = entity.MinOrderQty.ToString();
            txtMaxOrderQty.Text = entity.MaxOrderQty.ToString();
        }

        private void ControlToEntity(ItemPlanning entity)
        {
            if (hdnSupplierID.Value != "" && hdnSupplierID.Value != "0")
                entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            else
                entity.BusinessPartnerID = null;
            entity.LeadTime = Convert.ToByte(txtLeadTime.Text);
            //entity.ToleranceQty = Convert.ToDecimal(txtTolerance.Text);
            entity.SafetyTime = Convert.ToByte(txtSafetyTime.Text);
            entity.TimeFence = Convert.ToByte(txtTimeFence.Text);
            entity.SafetyStock = Convert.ToDecimal(txtSafetyStock.Text);
            entity.UnitPrice = Convert.ToDecimal(txtBasePrice.Text);
            entity.MinOrderQty = Convert.ToDecimal(txtMinOrderQty.Text);
            entity.MaxOrderQty = Convert.ToDecimal(txtMaxOrderQty.Text);
            entity.IsPriceLastUpdatedBySystem = false;
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