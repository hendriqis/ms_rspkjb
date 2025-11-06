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
    public partial class DirectPurchaseReturnApprovalList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE_RETURN_APPROVAL;
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

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC,
                                                                                    Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA,
                                                                                    Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE
                                                                                ));


            SettingParameterDt setvarDTR = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC).FirstOrDefault();
            hdnIsCalculateHNA.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;
            hdnIsPPNAppliedToAveragePrice.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).FirstOrDefault().ParameterValue;

            if (setvarDTR.ParameterValue != null && setvarDTR.ParameterValue != "" && setvarDTR.ParameterValue != "0")
            {
                List<UserInRole> uir = BusinessLayer.GetUserInRoleList(String.Format(
                    "HealthcareID = {0} AND UserID = {1} AND RoleID = {2}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarDTR.ParameterValue));
                if (uir.Count() > 0)
                {
                    SettingParameterDt setvarDTL = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTL.ParameterValue)).FirstOrDefault();
                    hdnLocationIDFrom.Value = loc.LocationID.ToString();
                    txtLocationCode.Text = loc.LocationCode;
                    txtLocationName.Text = loc.LocationName;
                }
                else
                {
                    SettingParameterDt setvarDTP = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTP.ParameterValue)).FirstOrDefault();
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
                int rowCount = BusinessLayer.GetvDirectPurchaseReturnHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vDirectPurchaseReturnHd> lstEntity = BusinessLayer.GetvDirectPurchaseReturnHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "DirectPurchaseNo DESC");
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
            DirectPurchaseReturnHdDao DirectPurchaseReturnHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao DirectPurchaseReturnDtDao = new DirectPurchaseReturnDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            if (type == "approve")
            {
                try
                {
                    string filterPurchaseDtOpen = String.Format("DirectPurchaseReturnID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.OPEN);
                    List<DirectPurchaseReturnDt> lstPurchaseDtOpen = BusinessLayer.GetDirectPurchaseReturnDtList(filterPurchaseDtOpen, ctx);
                    if (lstPurchaseDtOpen.Count == 0)
                    {
                        int count = 0;
                        string filterPurchaseHd = String.Format("DirectPurchaseReturnID IN ({0}) AND GCTransactionStatus = '{1}'", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseReturnHd> lstDirectPurchaseReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(filterPurchaseHd, ctx);
                        foreach (DirectPurchaseReturnHd purchaseHd in lstDirectPurchaseReturnHd)
                        {
                            string filterPurchaseDt = String.Format("DirectPurchaseReturnID IN ({0}) AND GCItemDetailStatus IN ('{1}')", purchaseHd.DirectPurchaseReturnID, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                            List<DirectPurchaseReturnDt> lstPurchaseDt = BusinessLayer.GetDirectPurchaseReturnDtList(filterPurchaseDt, ctx);
                            foreach (DirectPurchaseReturnDt purchaseDt in lstPurchaseDt)
                            {
                                decimal returnQty = purchaseDt.Quantity * purchaseDt.ConversionFactor;
                                string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", purchaseHd.LocationID, purchaseDt.ItemID);
                                string filterExpressionItemProduct = String.Format("ItemID = '{0}'", purchaseDt.ItemID);

                                ItemBalance lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpression, ctx).FirstOrDefault();
                                ItemProduct lstItemProduct = BusinessLayer.GetItemProductList(filterExpressionItemProduct, ctx).FirstOrDefault();
                                decimal currentStock = lstItemBalance.QuantityEND;
                                if (currentStock < returnQty)
                                {
                                    if (lstItemProduct.IsInventoryItem == true)
                                    {
                                        count = count + 1;
                                        result = false;
                                        break;
                                    }
                                    else
                                    {
                                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        DirectPurchaseReturnDtDao.Update(purchaseDt);
                                    }
                                }
                                else
                                {
                                    purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    DirectPurchaseReturnDtDao.Update(purchaseDt);
                                }

                                if (hdnIsCalculateHNA.Value == "1")
                                {
                                    #region new
                                    string filterIPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, purchaseDt.ItemID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterIPlanning, ctx);

                                    ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                                    string filterHistory = string.Format("ItemID = {0} ORDER BY HistoryID DESC", purchaseDt.ItemID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    ItemPriceHistory itemPriceHistory = BusinessLayer.GetItemPriceHistoryList(filterHistory, ctx).FirstOrDefault();

                                    string filterBalance = string.Format("ItemID = {0} AND IsDeleted = 0", purchaseDt.ItemID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<ItemBalance> ib = BusinessLayer.GetItemBalanceList(filterBalance, ctx);
                                    decimal qtyNow = ib.Sum(p => p.QuantityEND);

                                    decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                    decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                    decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                    bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                    bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                    decimal qtyReturn = purchaseDt.Quantity * purchaseDt.ConversionFactor * -1;
                                    decimal amountReturn = qtyReturn * purchaseDt.UnitPrice;

                                    if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                    {
                                        if (purchaseHd.IsIncludeVAT)
                                        {
                                            decimal ppnAmount = ((purchaseHd.VATPercentage / 100) * amountReturn);
                                            amountReturn = amountReturn + ppnAmount;
                                        }
                                    }

                                    if (qtyNow + qtyReturn != 0)
                                    {
                                        decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;
                                        decimal avg = (amountReturn + amountNow) / (qtyReturn + qtyNow);
                                        entityItemPlanning.AveragePrice = Math.Round(avg, 2);
                                    }
                                    else
                                    {
                                        entityItemPlanning.AveragePrice = 0;
                                    }

                                    entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                    entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    itemPlanningDao.Update(entityItemPlanning);
                                    BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DIRECTPURCHASE RETURN APPROVED", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    #endregion
                                }
                            }
                            
                            if (count == 0)
                            {
                                purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                                purchaseHd.ApprovedDate = DateTime.Now;
                                purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                DirectPurchaseReturnHdDao.Update(purchaseHd);
                            }
                            else
                            {
                                result = false;
                                errMessage = "Retur pembelian tunai dgn nomor <b>" + purchaseHd.DirectPurchaseReturnNo + "</b> tidak dapat di-approve karena stok tidak mencukupi di lokasi ini.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                ctx.RollBackTransaction();
                                break;
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
                    string filterPurchaseDtOpen = String.Format("DirectPurchaseReturnID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.OPEN);
                    List<DirectPurchaseReturnDt> lstPurchaseDtOpen = BusinessLayer.GetDirectPurchaseReturnDtList(filterPurchaseDtOpen, ctx);
                    if (lstPurchaseDtOpen.Count == 0)
                    {
                        string filterPurchaseHd = String.Format("DirectPurchaseReturnID IN ({0}) AND GCTransactionStatus = '{1}'", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseReturnHd> lstDirectPurchaseReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(filterPurchaseHd, ctx);
                        foreach (DirectPurchaseReturnHd purchaseHd in lstDirectPurchaseReturnHd)
                        {
                            purchaseHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                            purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            DirectPurchaseReturnHdDao.Update(purchaseHd);
                        }

                        string filterPurchaseDt = String.Format("DirectPurchaseReturnID IN ({0}) AND GCItemDetailStatus IN ('{1}')", hdnParam.Value, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
                        List<DirectPurchaseReturnDt> lstPurchaseDt = BusinessLayer.GetDirectPurchaseReturnDtList(filterPurchaseDt, ctx);
                        foreach (DirectPurchaseReturnDt purchaseDt in lstPurchaseDt)
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            DirectPurchaseReturnDtDao.Update(purchaseDt);

                            if (hdnIsCalculateHNA.Value == "1")
                            {
                                DirectPurchaseReturnHd entity = lstDirectPurchaseReturnHd.Where(t => t.DirectPurchaseReturnID == purchaseDt.DirectPurchaseReturnID).FirstOrDefault();

                                #region new
                                string filterIPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, purchaseDt.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterIPlanning, ctx);

                                ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                                string filterHistory = string.Format("ItemID = {0} ORDER BY HistoryID DESC", purchaseDt.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                ItemPriceHistory itemPriceHistory = BusinessLayer.GetItemPriceHistoryList(filterHistory, ctx).FirstOrDefault();

                                string filterBalance = string.Format("ItemID = {0} AND IsDeleted = 0", purchaseDt.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ItemBalance> ib = BusinessLayer.GetItemBalanceList(filterBalance, ctx);
                                decimal qtyNow = ib.Sum(p => p.QuantityEND);

                                decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                decimal qtyReturn = purchaseDt.Quantity * purchaseDt.ConversionFactor;
                                decimal amountReturn = qtyReturn * purchaseDt.UnitPrice;
                                decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;

                                if (entity.IsIncludeVAT)
                                {
                                    decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                    amountReturn = amountReturn + ppnAmount;
                                }

                                decimal avg = (amountReturn + amountNow) / (qtyReturn + qtyNow);

                                entityItemPlanning.AveragePrice = Math.Round(avg, 2);

                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DIRECTPURCHASE RETURN VOID", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                #endregion
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

            return result;
        }
    }
}