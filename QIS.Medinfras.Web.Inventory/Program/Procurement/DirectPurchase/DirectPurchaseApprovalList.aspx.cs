using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DirectPurchaseApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE_APPROVAL;
        }

        private GetUserMenuAccess menu;

        protected String GetMenuCaption()
        {
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}')",
                            AppSession.UserLogin.HealthcareID, //0
                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //1
                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //2
                            Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE, //3
                            Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE, //4
                            Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM, //5
                            Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC, //6
                            Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC, //7
                            Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY, //8
                            Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED, //9
                            Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT, //10
                            Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN //11
                        ));
            hdnIsReceiveUsingBaseUnit.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).FirstOrDefault().ParameterValue;
            hdnKapanPerubahanNilaiHargaKeItemPlanning.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN).FirstOrDefault().ParameterValue;

            string isDiscountToAveragePrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isDiscountToAveragePrice != "" && isDiscountToAveragePrice != null)
            {
                hdnIsDiscountAppliedToAveragePrice.Value = isDiscountToAveragePrice;
            }

            string isDiscountToUnitPrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue;
            if (isDiscountToUnitPrice != "" && isDiscountToUnitPrice != null)
            {
                hdnIsDiscountAppliedToUnitPrice.Value = isDiscountToUnitPrice;
            }

            string isPPNToAveragePrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isPPNToAveragePrice != "" && isPPNToAveragePrice != null)
            {
                hdnIsPPNAppliedToAveragePrice.Value = isPPNToAveragePrice;
            }

            string isPPNToUnitPrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE).ParameterValue;
            if (isPPNToUnitPrice != "" && isPPNToUnitPrice != null)
            {
                hdnIsPPNAppliedToUnitPrice.Value = isPPNToUnitPrice;
            }

            hdnIsAutoUpdateToSupplierItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM).ParameterValue;

            hdnIsUsingPurchaseDiscountShared.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED).FirstOrDefault().ParameterValue;

            string setvarDTR = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC).ParameterValue;
            string setvarDTL = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC).ParameterValue;
            string setvarDTP = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY).ParameterValue;
            if (setvarDTR != null && setvarDTR != "" && setvarDTR != "0")
            {
                List<UserInRole> uir = BusinessLayer.GetUserInRoleList(String.Format(
                    "HealthcareID = {0} AND UserID = {1} AND RoleID = {2}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarDTR));
                if (uir.Count() > 0)
                {
                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTL)).FirstOrDefault();
                    hdnLocationIDFrom.Value = loc.LocationID.ToString();
                    txtLocationCode.Text = loc.LocationCode;
                    txtLocationName.Text = loc.LocationName;
                }
                else
                {
                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTP)).FirstOrDefault();
                    hdnLocationIDFrom.Value = loc.LocationID.ToString();
                    txtLocationCode.Text = loc.LocationCode;
                    txtLocationName.Text = loc.LocationName;
                }
            }

            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            if (hdnLocationIDFrom.Value.ToString() != "" && hdnLocationIDFrom.Value.ToString() != "0" && hdnLocationIDFrom.Value.ToString() != null)
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}' AND LocationID = {1}",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, hdnLocationIDFrom.Value);
            }
            else
            {
                filterExpression += String.Format("GCTransactionStatus = '{0}'",
                    Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.DIRECT_PURCHASE);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvDirectPurchaseHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vDirectPurchaseHd> lstEntity = BusinessLayer.GetvDirectPurchaseHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DirectPurchaseNo DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            ItemMasterDao iMasterDao = new ItemMasterDao(ctx);
            ItemGroupMasterDao iGroupMasterDao = new ItemGroupMasterDao(ctx);
            SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);

            if (type == "approve")
            {
                try
                {
                    string filterPurchaseDtOpen = String.Format("DirectPurchaseID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.OPEN);
                    List<DirectPurchaseDt> lstPurchaseDtOpen = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDtOpen, ctx);
                    if (lstPurchaseDtOpen.Count == 0)
                    {
                        string filterPurchaseHd = String.Format("DirectPurchaseID IN ({0}) AND GCTransactionStatus = '{1}'", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterPurchaseHd, ctx);
                        foreach (DirectPurchaseHd entityHd in lstDirectPurchaseHd)
                        {
                            entityHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entityHd.ApprovedBy = AppSession.UserLogin.UserID;
                            entityHd.ApprovedDate = DateTime.Now;
                            entityHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            directPurchaseHdDao.Update(entityHd);

                            string filterPurchaseDt = String.Format("DirectPurchaseID IN ({0}) AND GCItemDetailStatus IN ('{1}')", entityHd.DirectPurchaseID, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                            List<DirectPurchaseDt> lstPurchaseDt = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDt, ctx);
                            foreach (DirectPurchaseDt entityDt in lstPurchaseDt)
                            {
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;

                                string filterItemMovement = string.Format("ItemID = {0}", entityDt.ItemID);
                                List<ItemMovement> lstItemMovement = BusinessLayer.GetItemMovementList(filterItemMovement, ctx);

                                string filterPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID);
                                ItemPlanning entityItemPlanning = BusinessLayer.GetItemPlanningList(filterPlanning, ctx).FirstOrDefault();

                                decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                string filterBalance = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID);
                                List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterBalance, ctx);

                                string filterSupplierItem = String.Format("BusinessPartnerID = {0} AND ItemID IN ({1}) AND GCPurchaseUnit = '{2}' AND IsDeleted = 0", entityHd.BusinessPartnerID, entityDt.ItemID, entityDt.GCItemUnit);
                                List<SupplierItem> lstSupplierItem = BusinessLayer.GetSupplierItemList(filterSupplierItem, ctx);

                                if (lstItemMovement.Count == 0)
                                {
                                    #region Belum Ada Movement

                                    if (entityItemPlanning.UnitPrice == 0)
                                    {
                                        #region Belum Ada Movement & Belum Definisi Harga

                                        #region Update Item Planning
                                        decimal unitPrice = entityDt.UnitPrice;
                                        decimal unitPriceTemp = entityDt.UnitPrice;
                                        decimal discountAmount1 = entityDt.DiscountAmount;
                                        decimal discountAmount2 = entityDt.DiscountAmount2;

                                        ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                                        ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                                        ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                                        if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                                        {
                                            if (entityItemGroup.IsDiscountCalculateHNA == true)
                                            {
                                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                                {
                                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                                }
                                                else
                                                {
                                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                            {
                                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                                {
                                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                                }
                                                else
                                                {
                                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                                }
                                            }
                                        }

                                        if (entityItemProduct.IsPPNCalculateHNAFromItemGroupMaster == true)
                                        {
                                            if (entityItemGroup.IsPPNCalculateHNA == true)
                                            {
                                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                                unitPrice = unitPrice + ppnAmountUnitPrice;
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsPPNAppliedToUnitPrice.Value == "1")
                                            {
                                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                                unitPrice = unitPrice + ppnAmountUnitPrice;
                                            }
                                        }

                                        decimal tempAmount = 0;
                                        tempAmount = unitPriceTemp;
                                        if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                        {
                                            tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                                        }
                                        else
                                        {
                                            tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                        }

                                        if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                        {
                                            decimal ppnAmount = tempAmount * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                            tempAmount = tempAmount + ppnAmount;
                                        }
                                        else
                                        {
                                            tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                        }

                                        decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                                        decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                                        if (tempQty > 0)
                                            entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                                        decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                                        entityItemPlanning.BusinessPartnerID = entityHd.BusinessPartnerID;
                                        entityItemPlanning.LastBusinessPartnerID = entityHd.BusinessPartnerID;
                                        entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                                        entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                                        entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                                        if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                        {
                                            if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                            {
                                                entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                            }
                                            else
                                            {
                                                entityDt.TempUnitPrice = unitPriceItemUnit;
                                            }

                                            if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                                {
                                                    entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                }
                                                else
                                                {
                                                    entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                }
                                                entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                            }
                                            else
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                                {
                                                    entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                                }
                                                else
                                                {
                                                    entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice;
                                                }
                                                entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                            {
                                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                                {
                                                    entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                                }
                                                else
                                                {
                                                    entityDt.TempUnitPrice = unitPriceItemUnit;
                                                }

                                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                                {
                                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                                    {
                                                        entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    }
                                                    else
                                                    {
                                                        entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    }
                                                    entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                                }
                                                else
                                                {
                                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                                    {
                                                        entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                                    }
                                                    else
                                                    {
                                                        entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice;
                                                    }
                                                    entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                                }
                                            }
                                            else
                                            {
                                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                                {
                                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                                }
                                                else
                                                {
                                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                                }
                                            }
                                        }

                                        entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                        entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        itemPlanningDao.Update(entityItemPlanning);
                                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                        #endregion

                                        if (hdnIsAutoUpdateToSupplierItem.Value == "1")
                                        {
                                            #region Update Supplier Item

                                            SupplierItem entitySuppItem = lstSupplierItem.Where(x => x.ItemID == entityDt.ItemID && x.GCPurchaseUnit == entityDt.GCItemUnit && x.BusinessPartnerID == entityHd.BusinessPartnerID).FirstOrDefault();

                                            if (entitySuppItem != null)
                                            {
                                                entitySuppItem.ConversionFactor = entityDt.ConversionFactor;
                                                entitySuppItem.Price = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                entitySuppItem.DiscountPercentage = entityDt.DiscountPercentage;
                                                entitySuppItem.DiscountPercentage2 = 0;

                                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    entitySuppItem.GCPurchaseUnit = entityDt.GCBaseUnit;
                                                }
                                                else
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice;
                                                    entitySuppItem.GCPurchaseUnit = entityDt.GCItemUnit;
                                                }

                                                entitySuppItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                supplierItemDao.Update(entitySuppItem);
                                            }
                                            else
                                            {
                                                entitySuppItem = new SupplierItem();
                                                entitySuppItem.BusinessPartnerID = entityHd.BusinessPartnerID;
                                                entitySuppItem.ItemID = entityDt.ItemID;
                                                entitySuppItem.Price = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                entitySuppItem.DiscountPercentage = entityDt.DiscountPercentage;
                                                entitySuppItem.DiscountPercentage2 = 0;
                                                entitySuppItem.ConversionFactor = entityDt.ConversionFactor;

                                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                    entitySuppItem.GCPurchaseUnit = entityDt.GCBaseUnit;
                                                }
                                                else
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice;
                                                    entitySuppItem.GCPurchaseUnit = entityDt.GCItemUnit;
                                                }

                                                entitySuppItem.CreatedBy = AppSession.UserLogin.UserID;
                                                entitySuppItem.CreatedDate = DateTime.Now;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                supplierItemDao.Insert(entitySuppItem);
                                            }
                                            #endregion
                                        }

                                        #endregion

                                    }
                                    else
                                    {
                                        #region Belum Ada Movement & Ada Definisi Harga

                                        #region Update Item Planning (AveragePrice)
                                        decimal unitPrice = entityDt.UnitPrice;
                                        decimal unitPriceTemp = entityDt.UnitPrice;
                                        decimal discountAmount1 = entityDt.DiscountAmount;
                                        decimal discountAmount2 = entityDt.DiscountAmount2;

                                        ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                                        ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                                        ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                                        if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                                        {
                                            if (entityItemGroup.IsDiscountCalculateHNA == true)
                                            {
                                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                                {
                                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                                }
                                                else
                                                {
                                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                            {
                                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                                {
                                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                                }
                                                else
                                                {
                                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                                }
                                            }
                                        }

                                        if (entityItemProduct.IsPPNCalculateHNAFromItemGroupMaster == true)
                                        {
                                            if (entityItemGroup.IsPPNCalculateHNA == true)
                                            {
                                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                                unitPrice = unitPrice + ppnAmountUnitPrice;
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsPPNAppliedToUnitPrice.Value == "1")
                                            {
                                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                                unitPrice = unitPrice + ppnAmountUnitPrice;
                                            }
                                        }

                                        decimal tempAmount = 0;
                                        tempAmount = unitPriceTemp;
                                        if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                        {
                                            tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                                        }
                                        else
                                        {
                                            tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                        }

                                        if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                        {
                                            decimal ppnAmount = tempAmount * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                            tempAmount = tempAmount + ppnAmount;
                                        }
                                        else
                                        {
                                            tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                        }

                                        decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                                        decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                                        if (tempQty > 0)
                                            entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                                        decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                                        entityItemPlanning.BusinessPartnerID = entityHd.BusinessPartnerID;
                                        entityItemPlanning.LastBusinessPartnerID = entityHd.BusinessPartnerID;
                                        entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                                        entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                                        entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                                        if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                        {
                                            //entityItemPlanning.UnitPrice = unitPriceItemUnit;

                                            if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                                //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                //entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                            }
                                            else
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                                //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                                //entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                            }
                                        }
                                        else
                                        {
                                            if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                            }
                                            else
                                            {
                                                entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                                entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                            }
                                        }

                                        entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                        entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        itemPlanningDao.Update(entityItemPlanning);
                                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                        #endregion

                                        #endregion
                                    }

                                    #endregion
                                }
                                else
                                {
                                    #region Ada Movement

                                    #region Update Item Planning (AveragePrice)
                                    decimal unitPrice = entityDt.UnitPrice;
                                    decimal unitPriceTemp = entityDt.UnitPrice;
                                    decimal discountAmount1 = entityDt.DiscountAmount;
                                    decimal discountAmount2 = entityDt.DiscountAmount2;

                                    ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                                    ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                                    ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                                    if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                                    {
                                        if (entityItemGroup.IsDiscountCalculateHNA == true)
                                        {
                                            if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                            {
                                                unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                            }
                                            else
                                            {
                                                unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                        {
                                            if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                            {
                                                unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                            }
                                            else
                                            {
                                                unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                            }
                                        }
                                    }

                                    decimal tempAmount = 0;
                                    tempAmount = unitPriceTemp;
                                    if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                    {
                                        tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                                    }
                                    else
                                    {
                                        tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                    }

                                    if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                    {
                                        decimal ppnAmount = tempAmount * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                                        tempAmount = tempAmount + ppnAmount;
                                    }
                                    else
                                    {
                                        tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                    }

                                    decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                                    decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                                    if (tempQty > 0)
                                        entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                                    decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                                    entityItemPlanning.BusinessPartnerID = entityHd.BusinessPartnerID;
                                    entityItemPlanning.LastBusinessPartnerID = entityHd.BusinessPartnerID;
                                    entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                    entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                                    entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                                    if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                    {
                                        //entityItemPlanning.UnitPrice = unitPriceItemUnit;

                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                            //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            //entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                            //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                            //entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                        }
                                    }
                                    else
                                    {
                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                        }
                                    }

                                    entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                    entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    itemPlanningDao.Update(entityItemPlanning);
                                    BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                    #endregion

                                    #endregion
                                }

                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                directPurchaseDtDao.Update(entityDt);
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, masih ada Item yang statusnya masih OPEN";
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
            }
            else if (type == "decline")
            {
                try
                {
                    string filterPurchaseDtOpen = String.Format("DirectPurchaseID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.OPEN);
                    List<DirectPurchaseDt> lstPurchaseDtOpen = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDtOpen, ctx);
                    if (lstPurchaseDtOpen.Count == 0)
                    {
                        string filterPurchaseHd = String.Format("DirectPurchaseID IN ({0}) AND GCTransactionStatus = '{1}'", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseHd> lstDirectPurchaseHd = BusinessLayer.GetDirectPurchaseHdList(filterPurchaseHd, ctx);
                        foreach (DirectPurchaseHd purchaseHd in lstDirectPurchaseHd)
                        {
                            purchaseHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            directPurchaseHdDao.Update(purchaseHd);
                        }

                        string filterPurchaseDt = String.Format("DirectPurchaseID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseDt> lstPurchaseDt = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDt, ctx);
                        foreach (DirectPurchaseDt purchaseDt in lstPurchaseDt)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            directPurchaseDtDao.Update(purchaseDt);
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, masih ada Item yang statusnya masih OPEN";
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
            }

            return result;
        }
    }
}