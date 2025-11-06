using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;
using System.Data;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReturnVoidList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_RETURN_VOID;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "No Retur" };
            fieldListValue = new string[] { "PurchaseReturnNo" };
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            string filterSetVar = string.Format("ParameterCode IN ('{0}','{1}', '{2}')",
                Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA,
                 Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE,
                Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE

                );
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsCalculateHNA.Value = lstSetVar.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;
            string isDiscountToAveragePrice = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).FirstOrDefault().ParameterValue;
            if (isDiscountToAveragePrice != "" && isDiscountToAveragePrice != null)
            {
                hdnIsDiscountAppliedToAveragePrice.Value = isDiscountToAveragePrice;
            }
            string isPPNToAveragePrice = lstSetVar.Where(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).FirstOrDefault().ParameterValue;
            if (isPPNToAveragePrice != "" && isPPNToAveragePrice != null)
            {
                hdnIsPPNAppliedToAveragePrice.Value = isPPNToAveragePrice;
            }
            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression() 
        {
            string filterExpression = String.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.APPROVED);
            filterExpression += String.Format(" AND PurchaseReturnID NOT IN (SELECT PurchaseReturnID FROM SupplierCreditNote WHERE GCTransactionStatus != '{0}')", 
                            Constant.TransactionStatus.VOID);
            filterExpression += String.Format(" AND PurchaseReturnID NOT IN (SELECT PurchaseReturnID FROM PurchaseReplacementHd WHERE GCTransactionStatus != '{0}')", 
                            Constant.TransactionStatus.VOID);
            //filterExpression += String.Format(" AND PurchaseReceiveID NOT IN (SELECT PurchaseReceiveID FROM PurchaseInvoiceDt WHERE IsDeleted = 0 AND PurchaseReceiveID IS NOT NULL)");
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReturnHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReturnHd> lstEntity = BusinessLayer.GetvPurchaseReturnHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseReturnNo DESC");
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
            bool result = true;
            if (type == "decline")
            {
                IDbContext ctx = DbFactory.Configure(true);
                PurchaseReturnHdDao entityDao = new PurchaseReturnHdDao(ctx);
                PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
                ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
                try
                {
                    string[] listParam = hdnParam.Value.Split(',');
                    foreach (string param in listParam)
                    {
                        int ItemRequestID = Convert.ToInt32(param);
                        PurchaseReturnHd entity = entityDao.Get(ItemRequestID);
                        List<PurchaseReturnDt> entityDt = BusinessLayer.GetPurchaseReturnDtList(String.Format("PurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", entity.PurchaseReturnID, Constant.TransactionStatus.VOID), ctx);
                        foreach (PurchaseReturnDt obj in entityDt) 
                        {
                            obj.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            obj.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(obj);

                            if (hdnIsCalculateHNA.Value == "1")
                            {
                                #region new
                                string filterIPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, obj.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterIPlanning, ctx);

                                ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == obj.ItemID).FirstOrDefault();

                                string filterHistory = string.Format("ItemID = {0} ORDER BY HistoryID DESC", obj.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                ItemPriceHistory itemPriceHistory = BusinessLayer.GetItemPriceHistoryList(filterHistory, ctx).FirstOrDefault();

                                string filterBalance = string.Format("ItemID = {0} AND IsDeleted = 0", obj.ItemID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<ItemBalance> ib = BusinessLayer.GetItemBalanceList(filterBalance, ctx);
                                decimal qtyNow = ib.Sum(p => p.QuantityEND);

                                decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                decimal qtyReturn = obj.Quantity * obj.ConversionFactor;
                                decimal amountReturn = qtyReturn * obj.UnitPrice;

                                decimal tempAmount = (obj.UnitPrice * obj.Quantity);
                                decimal discountAmount1 = 0;
                                decimal discountAmount2 = 0;

                                if (obj.DiscountAmount1 > 0)
                                {
                                    discountAmount1 = obj.DiscountAmount1;
                                }
                                else
                                {
                                    discountAmount1 = ((obj.Quantity * obj.UnitPrice) * obj.DiscountPercentage1 / 100);
                                }

                                if (obj.DiscountAmount2 > 0)
                                {
                                    discountAmount2 = obj.DiscountAmount2;
                                }
                                else
                                {
                                    discountAmount2 = ((obj.Quantity * obj.UnitPrice) * obj.DiscountPercentage2 / 100);
                                }

                                if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                {
                                    tempAmount = tempAmount - (discountAmount1 + discountAmount2);
                                }


                                if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                {
                                    decimal ppnAmount = tempAmount * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                    tempAmount = tempAmount + ppnAmount;
                                }
                                else
                                {
                                    if (hdnIsDiscountAppliedToAveragePrice.Value != "1")
                                    {
                                        tempAmount = (obj.UnitPrice * obj.Quantity);
                                    }
                                }
                                amountReturn = tempAmount;

                                decimal TotalQTYReturn = qtyNow + (qtyReturn * obj.ConversionFactor);
                                if (TotalQTYReturn > 0)
                                {
                                    decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;
                                    decimal avg = (amountNow - amountReturn) / TotalQTYReturn;
                                    entityItemPlanning.AveragePrice = Math.Round(avg, 2);
                                }


                                //decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;

                                //if (entity.IsIncludeVAT)
                                //{
                                //    decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                //    amountReturn = amountReturn + ppnAmount;
                                //}

                                //decimal avg = (amountReturn + amountNow) / (qtyReturn + qtyNow);

                                //  entityItemPlanning.AveragePrice = Math.Round(avg, 2);

                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("POR RETURN VOID", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                #endregion
                            }
                        }
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);
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
            }
            return result;
        }
    }
}