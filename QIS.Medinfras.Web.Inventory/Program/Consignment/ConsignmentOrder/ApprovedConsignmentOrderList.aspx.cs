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
    public partial class ApprovedConsignmentOrderList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.APPROVED_CONSIGNMENT_ORDER;
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

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        protected override void InitializeDataControl()
        {
            MPTrx master = (MPTrx)Master;
            menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            bool IsAllowAdd = false;
            bool IsAllowSave = false;
            bool IsAllowVoid = false;
            bool IsAllowNextPrev = false;
            SetToolbarVisibility(ref IsAllowAdd, ref IsAllowSave, ref IsAllowVoid, ref IsAllowNextPrev);

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format("GCTransactionStatus = '{0}' AND TransactionCode = '{1}'", Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderHd> lstEntity = BusinessLayer.GetvPurchaseOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PurchaseOrderNo DESC");
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


        private void CopyToEntityHd(PurchaseOrderHd newEntity, PurchaseOrderHd oldEntity)
        {
            newEntity.TransactionCode = Constant.TransactionCode.CONSIGNMENT_ORDER;
            newEntity.DeliveryDate = oldEntity.DeliveryDate;
            newEntity.POExpiredDate = oldEntity.POExpiredDate;
            newEntity.BusinessPartnerID = oldEntity.BusinessPartnerID;
            newEntity.PaymentRemarks = oldEntity.PaymentRemarks;
            newEntity.Remarks = oldEntity.Remarks;
            newEntity.OrderDate = Helper.GetDatePickerValue(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            newEntity.GCPurchaseOrderType = oldEntity.GCPurchaseOrderType;
            newEntity.TermID = oldEntity.TermID;
            newEntity.GCFrancoRegion = oldEntity.GCFrancoRegion;
            newEntity.GCCurrencyCode = oldEntity.GCCurrencyCode;
            newEntity.CurrencyRate = oldEntity.CurrencyRate;
            newEntity.DownPaymentAmount = oldEntity.DownPaymentAmount;
            newEntity.LocationID = oldEntity.LocationID;
            newEntity.ProductLineID = oldEntity.ProductLineID;
            newEntity.RevenueCostCenterID = oldEntity.RevenueCostCenterID;
            newEntity.GCChargesType = oldEntity.GCChargesType;
            newEntity.ChargesAmount = oldEntity.ChargesAmount;
            newEntity.GCCurrencyCode = oldEntity.GCCurrencyCode;
            newEntity.CurrencyRate = oldEntity.CurrencyRate;
            newEntity.IsIncludeVAT = oldEntity.IsIncludeVAT;
            newEntity.FinalDiscount = oldEntity.FinalDiscount;
            if (newEntity.IsIncludeVAT)
                newEntity.VATPercentage = oldEntity.VATPercentage;
            else
                newEntity.VATPercentage = 0;
            newEntity.IsCampaign = oldEntity.IsCampaign;
            newEntity.IsUrgent = oldEntity.IsUrgent;
            newEntity.IsUsingTermPO = oldEntity.IsUsingTermPO;
            newEntity.IsIncludePPh = oldEntity.IsIncludePPh;
            newEntity.GCPPHType = oldEntity.GCPPHType;
            newEntity.PPHMode = oldEntity.PPHMode;
            newEntity.IsPPHInPercentage = oldEntity.IsPPHInPercentage;
            newEntity.PPHPercentage = oldEntity.PPHPercentage;
        }

        private void CopyToEntityDt(PurchaseOrderDt newEntityDt, PurchaseOrderDt oldEntityDt)
        {
            newEntityDt.ItemID = oldEntityDt.ItemID;
            newEntityDt.Quantity = oldEntityDt.Quantity;
            newEntityDt.GCPurchaseUnit = oldEntityDt.GCPurchaseUnit;
            newEntityDt.GCBaseUnit = oldEntityDt.GCBaseUnit;
            newEntityDt.ConversionFactor = oldEntityDt.ConversionFactor;
            newEntityDt.PurchaseRequestID = oldEntityDt.PurchaseRequestID;
            newEntityDt.UnitPrice = oldEntityDt.UnitPrice;
            newEntityDt.IsDiscountInPercentage1 = oldEntityDt.IsDiscountInPercentage1;
            newEntityDt.DiscountPercentage1 = oldEntityDt.DiscountPercentage1;
            newEntityDt.DiscountAmount1 = oldEntityDt.DiscountAmount1;
            newEntityDt.IsDiscountInPercentage2 = oldEntityDt.IsDiscountInPercentage2;
            newEntityDt.DiscountPercentage2 = oldEntityDt.DiscountPercentage2;
            newEntityDt.DiscountAmount2 = oldEntityDt.DiscountAmount2;
            newEntityDt.GCBudgetCategory = oldEntityDt.GCBudgetCategory;
            newEntityDt.BudgetPlanNo = oldEntityDt.BudgetPlanNo;
            newEntityDt.IsBonusItem = oldEntityDt.IsBonusItem;
            newEntityDt.Remarks = oldEntityDt.Remarks;
            newEntityDt.LineAmount = oldEntityDt.CustomSubTotal;
            newEntityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

            //Simpan data draft
            newEntityDt.DraftQuantity = oldEntityDt.Quantity;
            newEntityDt.DraftUnitPrice = oldEntityDt.UnitPrice;
            newEntityDt.IsDraftDiscountInPercentage1 = oldEntityDt.IsDraftDiscountInPercentage1;
            newEntityDt.DraftDiscountPercentage1 = oldEntityDt.DiscountPercentage1;
            newEntityDt.DraftDiscountAmount1 = oldEntityDt.DraftDiscountAmount1;
            newEntityDt.IsDraftDiscountInPercentage2 = oldEntityDt.IsDraftDiscountInPercentage2;
            newEntityDt.DraftDiscountPercentage2 = oldEntityDt.DiscountPercentage2;
            newEntityDt.DraftDiscountAmount2 = oldEntityDt.DraftDiscountAmount2;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            int OrderID;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao POHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao PODtDao = new PurchaseOrderDtDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            AuditLog entityAuditLog = new AuditLog();

            if (type == "close")
            {
                try
                {
                    string filterExpressionPOHd = String.Format("PurchaseOrderID = {0}", hdnID.Value);

                    PurchaseOrderHd POHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPOHd).FirstOrDefault();
                    entityAuditLog.OldValues = JsonConvert.SerializeObject(POHd);
                    POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    POHd.ClosedBy = AppSession.UserLogin.UserID;
                    POHd.ClosedDate = DateTime.Now;
                    POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    POHdDao.Update(POHd);

                    entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                    entityAuditLog.NewValues = JsonConvert.SerializeObject(POHd);
                    entityAuditLog.UserID = AppSession.UserLogin.UserID;
                    entityAuditLog.LogDate = DateTime.Now;
                    entityAuditLog.TransactionID = POHd.PurchaseOrderID;

                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPOHd, ctx);
                    foreach (PurchaseOrderDt PODt in lstPurchaseOrderDt)
                    {
                        PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                        PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        PODtDao.Update(PODt);
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
            else
            {
                try
                {
                    string filterExpressionPOHd = String.Format("PurchaseOrderID = {0}", hdnID.Value);

                    PurchaseOrderHd POHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPOHd).FirstOrDefault();
                    entityAuditLog.OldValues = JsonConvert.SerializeObject(POHd);
                    POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    POHdDao.Update(POHd);
                    entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                    entityAuditLog.NewValues = JsonConvert.SerializeObject(POHd);
                    entityAuditLog.UserID = AppSession.UserLogin.UserID;
                    entityAuditLog.LogDate = DateTime.Now;
                    entityAuditLog.TransactionID = POHd.PurchaseOrderID;

                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPOHd, ctx);
                    foreach (PurchaseOrderDt PODt in lstPurchaseOrderDt)
                    {
                        PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                        PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        PODtDao.Update(PODt);
                    }

                    PurchaseOrderHd entityHd = new PurchaseOrderHd();
                    CopyToEntityHd(entityHd, POHd);
                    entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.CONSIGNMENT_ORDER, entityHd.OrderDate, ctx);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.ReferenceNo = POHd.PurchaseOrderNo;                    
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    OrderID = POHdDao.InsertReturnPrimaryKeyID(entityHd);

                    ctx.Command.Parameters.Clear();

                    string filterExpressionPODt = String.Format("PurchaseOrderID = {0} AND ReceivedInformation IS NULL", hdnID.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDtnew = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPODt, ctx);
                    foreach (PurchaseOrderDt entity in lstPurchaseOrderDtnew)
                    {
                        PurchaseOrderDt entityDt = new PurchaseOrderDt();
                        CopyToEntityDt(entityDt, entity);
                        entityDt.PurchaseOrderID = OrderID;
                        entityDt.IsBySystem = true;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        PODtDao.Insert(entityDt);
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    result = false;
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