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
    public partial class DonationApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DONATION_APPROVAL;
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

            SettingParameterDt setParSupItem = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM);
            hdnIsAutoUpdateToSupplierItem.Value = setParSupItem.ParameterValue;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.DONATION_RECEIVE); 
            BindGridView(1, true, ref PageCount);
        }

        #region Item Request HD
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'", Constant.TransactionStatus.WAIT_FOR_APPROVAL, Constant.TransactionCode.DONATION_RECEIVE);

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));

            if (count > 0)
                filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    filterExpression += string.Format(" AND LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
            }

            if (hdnLocationID.Value != "")
            {
                filterExpression += string.Format(" AND LocationID = {0}", hdnLocationID.Value);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReceiveHd> lstEntity = BusinessLayer.GetvPurchaseReceiveHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseReceiveNo DESC");
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
        #endregion

        #region CustomButtonClick
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
                SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);
                TermDao termDao = new TermDao(ctx);

                try
                {
                    string filterExpressionNotProposed = String.Format("PurchaseReceiveID IN ({0}) AND GCTransactionStatus != '{1}'", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                    List<PurchaseReceiveHd> lstNotProposed = BusinessLayer.GetPurchaseReceiveHdList(filterExpressionNotProposed, ctx);
                    if (lstNotProposed.Count() == 0)
                    {
                        string filterExpressionPurchaseReceiveHd = String.Format("PurchaseReceiveID IN ({0})", hdnParam.Value);
                        List<PurchaseReceiveHd> lstPurchaseReceiveHd = BusinessLayer.GetPurchaseReceiveHdList(filterExpressionPurchaseReceiveHd, ctx);
                        foreach (PurchaseReceiveHd purchaseHd in lstPurchaseReceiveHd)
                        {
                            string lstItemID = "";
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            string filterExpression = String.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", purchaseHd.PurchaseReceiveID, Constant.TransactionStatus.VOID);
                            List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterExpression, ctx);
                            foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                            {
                                if (lstItemID != "")
                                    lstItemID += ",";
                                lstItemID += purchaseDt.ItemID.ToString();
                            }

                            int termDay = termDao.Get(purchaseHd.TermID).TermDay;
                            //int termDay = termDao.Get(entityHd.TermID).TermDay;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                    "HealthcareID = {0} AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                    AppSession.UserLogin.HealthcareID,
                                                                    Constant.SettingParameter.IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE,
                                                                    Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT,
                                                                    Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE,
                                                                    Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE
                                                                ), ctx);

                            string setvar = lstParam.Where(t => t.ParameterCode == Constant.SettingParameter.IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE).FirstOrDefault().ParameterValue;
                            if (setvar == "2")
                            {
                                purchaseHd.PaymentDueDate = purchaseHd.ReferenceDate.AddDays(termDay);
                            }
                            else
                            {
                                purchaseHd.PaymentDueDate = purchaseHd.ReceivedDate.AddDays(termDay);
                            }

                            string spd = lstParam.Where(t => t.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).FirstOrDefault().ParameterValue;
                            string setvarValue = "0";
                            if (spd != null)
                            {
                                setvarValue = spd;
                            }

                            bool isDiscountAppliedToAveragePrice = false;
                            bool isDiscountAppliedToUnitPrice = false;
                            if (lstParam != null)
                            {
                                isDiscountAppliedToAveragePrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue == "1";
                                isDiscountAppliedToUnitPrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue == "1";
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                            List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterExpression, ctx);
                            filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                            List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, ctx);
                            filterExpression = String.Format("BusinessPartnerID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", purchaseHd.BusinessPartnerID, lstItemID);
                            List<SupplierItem> lstSupplierItem = BusinessLayer.GetSupplierItemList(filterExpression, ctx);

                            List<int> tempLstItemID = new List<int>();
                            foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                            {
                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                purchaseDtDao.Update(purchaseDt);

                                #region Update Item Planning
                                ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                                decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                decimal unitPrice = purchaseDt.UnitPrice;
                                decimal unitPriceTemp = purchaseDt.UnitPrice;
                                decimal discountAmount1 = (unitPrice * purchaseDt.DiscountPercentage1) / 100;
                                decimal discountAmount2 = ((unitPrice - discountAmount1) * purchaseDt.DiscountPercentage2) / 100;

                                if (isDiscountAppliedToUnitPrice)
                                {
                                    unitPrice = unitPrice - (discountAmount1 + discountAmount2);
                                }

                                decimal tempAmount = 0;
                                if (isDiscountAppliedToAveragePrice)
                                {
                                    unitPriceTemp = unitPriceTemp - (discountAmount1 + discountAmount2);
                                    tempAmount = (unitPriceTemp * purchaseDt.Quantity);
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
                                //if (isDiscountAppliedToUnitPrice)
                                //{
                                //    discountAmount1 = (unitPriceItemUnit * purchaseDt.DiscountPercentage1) / 100;
                                //    discountAmount2 = ((unitPriceItemUnit - discountAmount1) * purchaseDt.DiscountPercentage2) / 100;
                                //    unitPriceItemUnit = unitPriceItemUnit - (discountAmount1 + discountAmount2);
                                //}

                                //Di Comment Karena Jika Beda Satuan Pembelian Maka Hasil nya salah
                                //Contoh : Sebelumnya Beli 1 Tablet @2000
                                //Terus Beli Lagi 1 Box @150000, 1 Box = 100 Tablet
                                //Maka Akan Masuk If Padahal Seharus nya Tidak
                                //                        if (entityItemPlanning.PurchaseUnitPrice < unitPrice || entityItemPlanning.UnitPrice < unitPriceItemUnit)

                                if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                {
                                    entityItemPlanning.PurchaseUnitPrice = unitPrice;
                                    entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                }

                                entityItemPlanning.BusinessPartnerID = purchaseHd.BusinessPartnerID;
                                entityItemPlanning.LastBusinessPartnerID = purchaseHd.BusinessPartnerID;
                                entityItemPlanning.LastPurchasePrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                entityItemPlanning.LastConversionFactor = purchaseDt.ConversionFactor;
                                entityItemPlanning.LastPurchaseDiscount = purchaseDt.DiscountPercentage1;
                                entityItemPlanning.LastPurchaseDiscount2 = purchaseDt.DiscountPercentage2;

                                if (setvarValue == "1")
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                    entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.UnitPrice;
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                    entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.PurchaseUnitPrice;
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                }

                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DN", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                #endregion

                                if (hdnIsAutoUpdateToSupplierItem.Value == "1")
                                {
                                    #region Update Supplier Item
                                    if (!(tempLstItemID.IndexOf(purchaseDt.ItemID) != -1))
                                    {
                                        SupplierItem entitySuppItem = lstSupplierItem.Where(x => x.ItemID == purchaseDt.ItemID && x.GCPurchaseUnit == purchaseDt.GCItemUnit && x.BusinessPartnerID == purchaseHd.BusinessPartnerID && x.IsDeleted == false).FirstOrDefault();

                                        if (entitySuppItem != null)
                                        {
                                            entitySuppItem.ConversionFactor = purchaseDt.ConversionFactor;
                                            entitySuppItem.Price = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                            entitySuppItem.DiscountPercentage = purchaseDt.DiscountPercentage1;
                                            entitySuppItem.DiscountPercentage2 = purchaseDt.DiscountPercentage2;

                                            if (setvarValue == "1")
                                            {
                                                entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                                entitySuppItem.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                            }
                                            else
                                            {
                                                entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                                entitySuppItem.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                            }

                                            entitySuppItem.LastUpdatedBy = AppSession.UserLogin.UserID;

                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            supplierItemDao.Update(entitySuppItem);
                                        }
                                        else
                                        {
                                            if (!purchaseDt.IsBonusItem)
                                            {
                                                entitySuppItem = new SupplierItem();
                                                entitySuppItem.BusinessPartnerID = purchaseHd.BusinessPartnerID;
                                                entitySuppItem.ItemID = purchaseDt.ItemID;
                                                entitySuppItem.Price = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                                entitySuppItem.DiscountPercentage = purchaseDt.DiscountPercentage1;
                                                entitySuppItem.DiscountPercentage2 = purchaseDt.DiscountPercentage2;
                                                entitySuppItem.ConversionFactor = purchaseDt.ConversionFactor;

                                                if (setvarValue == "1")
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                                    entitySuppItem.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                                }
                                                else
                                                {
                                                    entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                                    entitySuppItem.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                                }

                                                entitySuppItem.CreatedBy = AppSession.UserLogin.UserID;
                                                entitySuppItem.CreatedDate = DateTime.Now;

                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                supplierItemDao.Insert(entitySuppItem);
                                            }
                                        }
                                        tempLstItemID.Add(purchaseDt.ItemID);
                                    }
                                    #endregion
                                }

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
                    else
                    {
                        errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
                        result = false;
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

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(entityDt);
                            }

                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
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
        #endregion

        #region Item Request Dt
        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnID.Value != "")
            {
                filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnID.Value, Constant.TransactionStatus.VOID);

                if (isCountPageCount)
                {
                    int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                    pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
                }
            }
            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }
        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion
    }
}