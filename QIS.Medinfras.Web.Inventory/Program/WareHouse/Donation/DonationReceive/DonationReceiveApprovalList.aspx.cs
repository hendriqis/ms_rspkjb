using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DonationReceiveApprovalList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_RECEIVE_APPROVAL;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
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

        protected override bool OnCustomButtonClick(string type, ref string retval, ref string errMessage)
        {
            if (type == "approve")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
                PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
                ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);

                try
                {
                    string filterExpressionPurchaseReceiveHd = String.Format("PurchaseReceiveID IN ({0})", hdnParam.Value);
                    List<PurchaseReceiveHd> lstPurchaseReceiveHd = BusinessLayer.GetPurchaseReceiveHdList(filterExpressionPurchaseReceiveHd);
                    foreach (PurchaseReceiveHd purchaseHd in lstPurchaseReceiveHd)
                    {
                        purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseHdDao.Update(purchaseHd);
                    }

                    string filterExpression = String.Format("PurchaseReceiveID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnParam.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(filterExpression);
                    string lstItemID = "";
                    foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt) 
                    {
                        if (lstItemID != "")
                            lstItemID += ",";
                        lstItemID += purchaseDt.ItemID.ToString();
                    }

                    List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')",
                                                          Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE,
                                                          Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE));
                    bool isDiscountAppliedToAveragePrice = false;
                    bool isDiscountAppliedToUnitPrice = false;
                    if (lstParam != null)
                    {
                        isDiscountAppliedToAveragePrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue == "1";
                        isDiscountAppliedToUnitPrice = lstParam.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue == "1";
                    }

                    filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                    List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterExpression, ctx);
                    List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, ctx);
                    foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                        decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                        decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                        decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                        bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                        bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                        decimal unitPrice = purchaseDt.UnitPrice;
                        decimal discountAmount1 = (unitPrice * purchaseDt.DiscountPercentage1) / 100;
                        decimal discountAmount2 = ((unitPrice - discountAmount1) * purchaseDt.DiscountPercentage2) / 100;
                        if (isDiscountAppliedToAveragePrice)
                            unitPrice = unitPrice - (discountAmount1 + discountAmount2);

                        decimal unitPriceItemUnit = unitPrice / purchaseDt.ConversionFactor;
                        if (isDiscountAppliedToUnitPrice)
                        {
                            discountAmount1 = (unitPriceItemUnit * purchaseDt.DiscountPercentage1) / 100;
                            discountAmount2 = ((unitPriceItemUnit - discountAmount1) * purchaseDt.DiscountPercentage2) / 100;
                            unitPriceItemUnit = unitPriceItemUnit - (discountAmount1 + discountAmount2);
                        }

                        if (entityItemPlanning.LastPurchasePrice < unitPriceItemUnit)
                        {
                            entityItemPlanning.LastPurchasePrice = unitPriceItemUnit;
                            entityItemPlanning.UnitPrice = unitPriceItemUnit;
                        }

                        entityItemPlanning.LastBusinessPartnerID = lstPurchaseReceiveHd.FirstOrDefault(p => p.PurchaseReceiveID == purchaseDt.PurchaseReceiveID).BusinessPartnerID;
                        entityItemPlanning.LastPurchaseDiscount = purchaseDt.DiscountPercentage1;

                        decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == purchaseDt.ItemID).Sum(p => p.QuantityEND);
                        entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (unitPrice * purchaseDt.Quantity)) / (qtyEnd + (purchaseDt.Quantity * purchaseDt.ConversionFactor));
                        entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                        itemPlanningDao.Update(entityItemPlanning);
                        purchaseDtDao.Update(purchaseDt);
                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DN", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    result = false;
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
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int PurchaseReceiveID = Convert.ToInt32(param);
                        PurchaseReceiveHd entity = entityDao.Get(PurchaseReceiveID);
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    result = false;
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