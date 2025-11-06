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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentReceiveApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_RECEIVE_APPROVAL;
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
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());
            
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.CONSIGNMENT_RECEIVE);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            if (count > 0)
                filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReceiveHd> lstEntity = BusinessLayer.GetvPurchaseReceiveHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ReceivedDate DESC");
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
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseOrderHdDao purchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
                PurchaseOrderDtDao purchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
                PurchaseRequestPODao purchaseRequestPODao = new PurchaseRequestPODao(ctx);
                PurchaseReceivePODao purchaseReceivePODao = new PurchaseReceivePODao(ctx);
                PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
                PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
                ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
                ItemProductDao iProductDao = new ItemProductDao(ctx);
                ItemMasterDao iMasterDao = new ItemMasterDao(ctx);
                ItemGroupMasterDao iGroupMasterDao = new ItemGroupMasterDao(ctx);

                try
                {
                    string filterExpressionPurchaseReceiveHd = String.Format("PurchaseReceiveID IN ({0})", hdnParam.Value);
                    List<PurchaseReceiveHd> lstPurchaseReceiveHd = BusinessLayer.GetPurchaseReceiveHdList(filterExpressionPurchaseReceiveHd, ctx);
                    foreach (PurchaseReceiveHd purchaseHd in lstPurchaseReceiveHd)
                    {
                        string filterExpression = String.Format("PurchaseReceiveID IN ({0}) AND GCItemDetailStatus != '{1}'", purchaseHd.PurchaseReceiveID, Constant.TransactionStatus.VOID);
                        List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterExpression, ctx);
                        string lstItemID = "";
                        foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                        {
                            if (lstItemID != "")
                                lstItemID += ",";
                            lstItemID += purchaseDt.ItemID.ToString();
                        }

                        List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                                            AppSession.UserLogin.HealthcareID, //0
                                                                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //1
                                                                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //2
                                                                            Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE, //3
                                                                            Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE, //4
                                                                            Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT, //5
                                                                            Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN //6
                                                                        ));
                        bool isDiscountAppliedToAveragePrice = false;
                        bool isDiscountAppliedToUnitPrice = false;
                        bool isPPNAppliedToAveragePrice = false;
                        bool isPPNAppliedToUnitPrice = false;
                        bool isReceiveUsingBaseUnit = false;
                        string hdnKapanPerubahanNilaiHargaKeItemPlanning = "1";

                        if (lstParam != null)
                        {
                            isDiscountAppliedToAveragePrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue == "1";
                            isDiscountAppliedToUnitPrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue == "1";
                            isPPNAppliedToAveragePrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue == "1";
                            isPPNAppliedToUnitPrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE).ParameterValue == "1";
                            isReceiveUsingBaseUnit = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).ParameterValue == "1";

                            hdnKapanPerubahanNilaiHargaKeItemPlanning = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN).ParameterValue;
                        }

                        filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                        List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterExpression, ctx);
                        List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, ctx);
                        foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;

                            #region Update Item Planning

                            ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                            decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                            decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                            decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                            bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                            bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                            decimal unitPrice = purchaseDt.UnitPrice;
                            decimal unitPriceTemp = purchaseDt.UnitPrice;
                            decimal discountAmount1 = purchaseDt.DiscountAmount1;
                            decimal discountAmount2 = purchaseDt.DiscountAmount2;

                            ItemProduct entityItemProduct = iProductDao.Get(purchaseDt.ItemID);
                            ItemMaster entityItem = iMasterDao.Get(purchaseDt.ItemID);
                            ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                            if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                            {
                                if (entityItemGroup.IsDiscountCalculateHNA == true)
                                {
                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
                                }
                            }
                            else
                            {
                                if (isDiscountAppliedToUnitPrice)
                                {
                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
                                }
                            }

                            if (entityItemProduct.IsPPNCalculateHNAFromItemGroupMaster == true)
                            {
                                if (entityItemGroup.IsPPNCalculateHNA == true)
                                {
                                    decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(purchaseHd.VATPercentage) / Convert.ToDecimal(100));
                                    unitPrice = unitPrice + ppnAmountUnitPrice;
                                }
                            }
                            else
                            {
                                if (isPPNAppliedToUnitPrice)
                                {
                                    decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(purchaseHd.VATPercentage) / Convert.ToDecimal(100));
                                    unitPrice = unitPrice + ppnAmountUnitPrice;
                                }
                            }

                            decimal tempAmount = unitPriceTemp;
                            if (isDiscountAppliedToAveragePrice)
                            {
                                tempAmount = ((tempAmount * purchaseDt.Quantity) - (discountAmount1 + discountAmount2));
                            }
                            else
                            {
                                tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity);
                            }

                            if (isPPNAppliedToAveragePrice)
                            {
                                decimal ppnAmount = tempAmount * (Convert.ToDecimal(purchaseHd.VATPercentage) / Convert.ToDecimal(100));
                                tempAmount = tempAmount + ppnAmount;
                            }
                            else
                            {
                                tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity);
                            }

                            decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == purchaseDt.ItemID).Sum(p => p.QuantityEND);
                            decimal tempQty = (qtyEnd + (purchaseDt.Quantity * purchaseDt.ConversionFactor));
                            if (tempQty > 0)
                                entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                            decimal unitPriceItemUnit = unitPrice / purchaseDt.ConversionFactor;

                            entityItemPlanning.BusinessPartnerID = purchaseHd.BusinessPartnerID;
                            entityItemPlanning.LastBusinessPartnerID = purchaseHd.BusinessPartnerID;
                            entityItemPlanning.LastPurchasePrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                            entityItemPlanning.LastConversionFactor = purchaseDt.ConversionFactor;
                            entityItemPlanning.LastPurchaseDiscount = purchaseDt.DiscountPercentage1;
                            entityItemPlanning.LastPurchaseDiscount2 = purchaseDt.DiscountPercentage2;

                            if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                            {
                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning != "2")
                                {
                                    entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                }
                                else
                                {
                                    purchaseDt.TempUnitPrice = unitPriceItemUnit;
                                }

                                if (isReceiveUsingBaseUnit)
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning != "2")
                                    {
                                        entityItemPlanning.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    }
                                    else
                                    {
                                        purchaseDt.TempPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    }
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning != "2")
                                    {
                                        entityItemPlanning.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                    }
                                    else
                                    {
                                        purchaseDt.TempPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    }
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                }
                            }
                            else
                            {
                                if (isReceiveUsingBaseUnit)
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                }
                            }

                            entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                            entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemPlanningDao.Update(entityItemPlanning);
                            BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("CR", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                            #endregion

                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            purchaseDtDao.Update(purchaseDt);

                            if (purchaseDt.PurchaseOrderID != null)
                            {
                                PurchaseReceivePO entityPRPO = new PurchaseReceivePO();
                                entityPRPO.PurchaseOrderID = (int)purchaseDt.PurchaseOrderID;
                                entityPRPO.PurchaseReceiveID = purchaseHd.PurchaseReceiveID;
                                entityPRPO.ItemID = purchaseDt.ItemID;
                                entityPRPO.ReceivedQuantity = purchaseDt.Quantity;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                purchaseReceivePODao.Insert(entityPRPO);

                                PurchaseOrderDt poDt = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'", purchaseDt.PurchaseOrderID, purchaseDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                                if (poDt != null)
                                {
                                    decimal receivedQty = purchaseDt.Quantity;
                                    List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0} AND ItemID = {1}", purchaseDt.PurchaseOrderID, purchaseDt.ItemID), ctx);
                                    foreach (PurchaseRequestPO purchaseRequestPO in lstPurchaseRequestPO)
                                    {
                                        decimal tempReceivedQuantity = receivedQty;
                                        decimal completeQuantity = purchaseRequestPO.OrderQuantity - purchaseRequestPO.ReceivedQuantity;
                                        if (tempReceivedQuantity > completeQuantity)
                                            tempReceivedQuantity = completeQuantity;
                                        purchaseRequestPO.ReceivedQuantity += tempReceivedQuantity;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseRequestPODao.Update(purchaseRequestPO);
                                        receivedQty -= tempReceivedQuantity;
                                    }

                                    if (poDt.Quantity == poDt.ReceivedQuantity)
                                    {
                                        poDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                        poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        purchaseOrderDtDao.Update(poDt);
                                    }
                                }

                                int count = BusinessLayer.GetPurchaseOrderDtRowCount(string.Format("PurchaseOrderID = {0} AND Quantity > ReceivedQuantity AND IsDeleted = 0", purchaseDt.PurchaseOrderID), ctx);
                                if (count < 1)
                                {
                                    PurchaseOrderHd entityPOHd = purchaseOrderHdDao.Get((int)purchaseDt.PurchaseOrderID);
                                    entityPOHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                    entityPOHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseOrderHdDao.Update(entityPOHd);
                                }
                            }
                        }

                        purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                        purchaseHd.ApprovedDate = DateTime.Now;
                        purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseHdDao.Update(purchaseHd);
                    }

                    ctx.CommitTransaction();
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
            else
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseReceiveHdDao entityDao = new PurchaseReceiveHdDao(ctx);
                PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseReceiveID = Convert.ToInt32(param);
                        PurchaseReceiveHd entity = entityDao.Get(PurchaseReceiveID);
                        if (entity.GCTransactionStatus == Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                        {
                            List<PurchaseReceiveDt> lstDt = BusinessLayer.GetPurchaseReceiveDtList(string.Format(
                                            "PurchaseReceiveID = {0} AND GCItemDetailStatus = '{1}'", entity.PurchaseReceiveID, Constant.TransactionStatus.WAIT_FOR_APPROVAL), ctx);
                            foreach (PurchaseReceiveDt entityDt in lstDt)
                            {
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }

                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDao.Update(entity);
                        }
                        else
                        {
                            errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
                            result = false;
                            ctx.RollBackTransaction();
                        }
                    }
                    ctx.CommitTransaction();
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
}