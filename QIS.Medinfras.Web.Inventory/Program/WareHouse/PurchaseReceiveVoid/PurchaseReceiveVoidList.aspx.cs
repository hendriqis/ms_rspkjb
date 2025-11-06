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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceiveVoidList : BasePageList
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_RECEIVE_VOID;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "No. Penerimaan", "Tgl. Penerimaan [YYYY-MM-DD]", "Product Line", "Nama Supplier", "Lokasi" };
            fieldListValue = new string[] { "PurchaseReceiveNo", "ReceivedDate", "ProductLineName", "SupplierName", "LocationName" };
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            string filterSetVar = string.Format("ParameterCode IN ('{0}','{1}','{2}', '{3}')",
                                                        Constant.SettingParameter.FN_REVISI_HUTANG_PENERIMAAN_MENGGUNAKAN_PEMBULATAN_TUKAR_FAKTUR, 
                                                        Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA,
                                                        Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE,
                                                        Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE
                                                        );
            List<SettingParameterDt> lstSetVar = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            hdnIsChangedAPAmountFromRoundingInvoice.Value = lstSetVar.Where(a => a.ParameterCode == Constant.SettingParameter.FN_REVISI_HUTANG_PENERIMAAN_MENGGUNAKAN_PEMBULATAN_TUKAR_FAKTUR).FirstOrDefault().ParameterValue;
            hdnIsCalculateHNA.Value = lstSetVar.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;
            
            string isDiscountToAveragePrice = lstSetVar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isDiscountToAveragePrice != "" && isDiscountToAveragePrice != null)
            {
                hdnIsDiscountAppliedToAveragePrice.Value = isDiscountToAveragePrice;
            }
            string isPPNToAveragePrice = lstSetVar.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isPPNToAveragePrice != "" && isPPNToAveragePrice != null)
            {
                hdnIsPPNAppliedToAveragePrice.Value = isPPNToAveragePrice;
            }

            BindGridView(1, true, ref PageCount);
        }

        private string GetFilterExpression()
        {
            string filterExpression = String.Format("GCTransactionStatus IN ('{0}') AND ProductLineID IS NOT NULL AND IsInventoryItem = 1", Constant.TransactionStatus.APPROVED);
            filterExpression += String.Format(" AND PurchaseReceiveID NOT IN (SELECT PurchaseReceiveID FROM PurchaseInvoiceDt WHERE IsDeleted = 0 AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseInvoiceHd WHERE GCTransactionStatus != '{0}'))", Constant.TransactionStatus.VOID);
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
                int rowCount = BusinessLayer.GetvPurchaseReceiveHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseReceiveHd> lstEntity = BusinessLayer.GetvPurchaseReceiveHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseReceiveID DESC");
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
                PurchaseReceiveHdDao entityDao = new PurchaseReceiveHdDao(ctx);
                PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
                PurchaseOrderHdDao entityOrderDao = new PurchaseOrderHdDao(ctx);
                PurchaseOrderDtDao entityOrderDtDao = new PurchaseOrderDtDao(ctx);
                ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
                ItemProductDao itemProductDao = new ItemProductDao(ctx);
                try
                {
                    if (hdnIsChangedAPAmountFromRoundingInvoice.Value == "0")
                    {
                        string PORHasReturn = "";
                        string filterPReturn = string.Format("PurchaseReceiveID IN ({0}) AND GCTransactionStatus != '{1}'", hdnParam.Value, Constant.TransactionStatus.VOID);
                        List<vPurchaseReturnHd> lstPReturn = BusinessLayer.GetvPurchaseReturnHdList(filterPReturn, ctx);
                        foreach (vPurchaseReturnHd preturn in lstPReturn)
                        {
                            if (PORHasReturn != "")
                            {
                                PORHasReturn += ", ";
                            }
                            PORHasReturn += preturn.PurchaseReceiveNo;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                        }

                        if (lstPReturn.Count() == 0)
                        {
                            int count = 0;
                            int countGL = 0;
                            string[] listParam = hdnParam.Value.Split(',');
                            foreach (string param in listParam)
                            {
                                int PurchaseReceiveID = Convert.ToInt32(param);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                //PurchaseReceiveHd entity = entityDao.Get(PurchaseReceiveID);
                                PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHdList(string.Format("PurchaseReceiveID = {0}", PurchaseReceiveID), ctx).FirstOrDefault();
                                if ((entity.GLTransactionID != null && entity.GLTransactionID != 0) || (entity.GLTransactionDtID != null && entity.GLTransactionDtID != 0))
                                {
                                    result = false;
                                    errMessage = "Penerimaan pembelian dgn nomor <b>" + entity.PurchaseReceiveNo + "</b> tidak dapat diubah karena sudah diproses realisasi kas bon.";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                                    {
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        List<PurchaseReceiveDt> lstDt = BusinessLayer.GetPurchaseReceiveDtList(string.Format(
                                                        "PurchaseReceiveID = {0} AND GCItemDetailStatus = '{1}'", entity.PurchaseReceiveID, Constant.TransactionStatus.APPROVED), ctx);
                                        foreach (PurchaseReceiveDt entityDt in lstDt)
                                        {
                                            #region PurchaseReceiveDt

                                            ItemProduct itemProduct = itemProductDao.Get(entityDt.ItemID);
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            if (itemProduct.IsInventoryItem)
                                            {
                                                decimal returnQty = entityDt.Quantity * entityDt.ConversionFactor;
                                                string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", entity.LocationID, entityDt.ItemID);
                                                ItemBalance lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpression, ctx).FirstOrDefault();
                                                decimal currentStock = lstItemBalance.QuantityEND;
                                                if (currentStock < returnQty)
                                                {
                                                    count = count + 1;
                                                    result = false;
                                                    break;
                                                }
                                                else
                                                {
                                                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityDtDao.Update(entityDt);
                                                }
                                            }
                                            else
                                            {
                                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityDtDao.Update(entityDt);
                                            }

                                            #endregion

                                            if (entityDt.PurchaseOrderDtID != null && entityDt.PurchaseOrderDtID != 0)
                                            {
                                                #region PurchaseOrder

                                                PurchaseOrderDt poDt = entityOrderDtDao.Get(Convert.ToInt32(entityDt.PurchaseOrderDtID));
                                                poDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                                poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityOrderDtDao.Update(poDt);

                                                PurchaseOrderHd poHd = entityOrderDao.Get(Convert.ToInt32(entityDt.PurchaseOrderID));
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                poHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                                poHd.LastUpdatedBy = AppSession.UserLogin.UserID;

                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                entityOrderDao.Update(poHd);
                                                #endregion
                                            }

                                            if (hdnIsCalculateHNA.Value == "1")
                                            {
                                                #region new
                                                string filterIPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterIPlanning, ctx);

                                                ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == entityDt.ItemID).FirstOrDefault();

                                                string filterHistory = string.Format("ItemID = {0} ORDER BY HistoryID DESC", entityDt.ItemID);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                ItemPriceHistory itemPriceHistory = BusinessLayer.GetItemPriceHistoryList(filterHistory, ctx).FirstOrDefault();

                                                string filterBalance = string.Format("ItemID = {0} AND IsDeleted = 0", entityDt.ItemID);
                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                List<ItemBalance> ib = BusinessLayer.GetItemBalanceList(filterBalance, ctx);
                                                decimal qtyNow = ib.Sum(p => p.QuantityEND);

                                                decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                                                decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                                                decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                                                bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                                                bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                                                decimal qtyReturn = entityDt.Quantity * -1; //purchaseDt.Quantity * purchaseDt.ConversionFactor * -1;
                                                decimal amountReturn = qtyReturn * entityDt.UnitPrice;
                                                decimal tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                                decimal discountAmount1 = 0;
                                                decimal discountAmount2 = 0;

                                                //decimal qtyReturn = entityDt.Quantity * entityDt.ConversionFactor * -1;
                                                //decimal amountReturn = qtyReturn * entityDt.UnitPrice;
                                                decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;

                                                //if (entity.IsIncludeVAT)
                                                //{
                                                //    decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                                //    amountReturn = amountReturn + ppnAmount;
                                                //}
                                                if (entityDt.DiscountAmount1 > 0)
                                                {
                                                    discountAmount1 = entityDt.DiscountAmount1;
                                                }
                                                else
                                                {
                                                    discountAmount1 = ((entityDt.Quantity * entityDt.UnitPrice) * entityDt.DiscountPercentage1 / 100);
                                                }

                                                if (entityDt.DiscountAmount2 > 0)
                                                {
                                                    discountAmount2 = entityDt.DiscountAmount2;
                                                }
                                                else
                                                {
                                                    discountAmount2 = ((entityDt.Quantity * entityDt.UnitPrice) * entityDt.DiscountPercentage2 / 100);
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
                                                        tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                                    }
                                                }
                                                amountReturn = tempAmount;
                                                decimal totalQty = qtyNow + (qtyReturn * entityDt.ConversionFactor);
                                                if (totalQty > 0)
                                                {
                                                    decimal avg = (amountReturn + amountNow) / totalQty;
                                                    entityItemPlanning.AveragePrice = Math.Round(avg, 2);
                                                }

                                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                itemPlanningDao.Update(entityItemPlanning);
                                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("POR VOID", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                                ctx.CommandType = CommandType.Text;
                                                ctx.Command.Parameters.Clear();
                                                #endregion
                                            }
                                        }

                                        if (count == 0)
                                        {
                                            entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();
                                            entityDao.Update(entity);
                                        }
                                        else
                                        {
                                            result = false;
                                            errMessage = "Penerimaan barang " + entity.PurchaseReceiveNo + " tidak dapat dibatalkan karena stok tidak mencukupi di lokasi ini.";
                                            Exception ex = new Exception(errMessage);
                                            Helper.InsertErrorLog(ex);
                                        }
                                    }
                                    else
                                    {
                                        result = false;
                                        errMessage = "Penerimaan barang " + entity.PurchaseReceiveNo + " tidak dapat diubah. Harap refresh halaman ini.";
                                        Exception ex = new Exception(errMessage);
                                        Helper.InsertErrorLog(ex);
                                    }
                                }
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Pembatalan penerimaan tidak dapat dilakukan, karena nomor penerimaan " + PORHasReturn + " sudah memiliki retur penerimaan pembelian.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Pembatalan penerimaan tidak dapat dilakukan, karena revisi penerimaan hanya bisa dilakukan melalui 'pembulatan' detail tukar faktur.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
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