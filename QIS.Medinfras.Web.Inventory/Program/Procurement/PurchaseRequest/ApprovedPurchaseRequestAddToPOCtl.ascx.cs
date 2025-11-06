using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ApprovedPurchaseRequestAddToPOCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        public override void InitializeDataControl(string param)
        {
            hdnPurchaseRequestID.Value = param;
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}'", Constant.TransactionStatus.OPEN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            List<vPurchaseOrderHd> lstEntity = BusinessLayer.GetvPurchaseOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "PurchaseOrderNo DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    result = string.Format("refresh|{0}", pageCount);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        class CPurchaseRequest
        {
            public String ID { get; set; }
            public String Discount1 { get; set; }
            public String Discount2 { get; set; }
            public String Price { get; set; }
            public String QtyPO { get; set; }
            public String SupplierID { get; set; }
            public String GCPurchaseUnit { get; set; }
            public String ConversionFactor { get; set; }
            public String TermID { get; set; }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            ApprovedPurchaseRequestDetailList myPage = (ApprovedPurchaseRequestDetailList)Page;
            String[] paramID = myPage.GetSelectedMember().Substring(1).Split('|');
            String[] paramQuantityPO = myPage.GetPurchaseOrderQty().Substring(1).Split('|');
            String[] paramPrice = myPage.GetPrice().Substring(1).Split('|');
            String[] paramDiscount1 = myPage.GetDiscount1().Substring(1).Split('|');
            String[] paramDiscount2 = myPage.GetDiscount2().Substring(1).Split('|');
            String[] paramSupplierID = myPage.GetListSupplierID().Substring(1).Split('|');
            String[] paramGCPurchaseUnit = myPage.GetListGCPurchaseUnit().Substring(1).Split('|');
            String[] paramConversionFactor = myPage.GetListConversionFactor().Substring(1).Split('|');
            String[] paramTermID = myPage.GetListTermID().Substring(1).Split('|');

            List<CPurchaseRequest> listEntityTempPR = new List<CPurchaseRequest>();

            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao entityPurchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPurchaseRequestDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                List<PurchaseRequestDt> lstEntityPurchaseReqDt = BusinessLayer.GetPurchaseRequestDtList(string.Format("ID IN ({0})", myPage.GetSelectedMember().Substring(1).Replace('|', ',')));
                for (int i = 0; i < paramID.Length; i++)
                {
                    CPurchaseRequest entityTempPR = new CPurchaseRequest();
                    entityTempPR.ID = paramID[i];
                    entityTempPR.QtyPO = paramQuantityPO[i];
                    entityTempPR.Discount1 = paramDiscount1[i];
                    entityTempPR.Discount2 = paramDiscount2[i];
                    entityTempPR.SupplierID = paramSupplierID[i];
                    entityTempPR.Price = paramPrice[i];
                    entityTempPR.GCPurchaseUnit = paramGCPurchaseUnit[i];
                    entityTempPR.ConversionFactor = paramConversionFactor[i];
                    entityTempPR.TermID = paramTermID[i];
                    listEntityTempPR.Add(entityTempPR);
                }

                List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND ItemID IN ({1}) AND GCItemDetailStatus != '{2}'", hdnID.Value, String.Join(",", lstEntityPurchaseReqDt.Select(p => p.ItemID).ToList()), Constant.TransactionStatus.VOID), ctx);
                foreach (CPurchaseRequest entityCPurchaseReqDt in listEntityTempPR)
                {
                    PurchaseRequestDt entityPurchaseReqDt = lstEntityPurchaseReqDt.Where(p => p.ID.ToString() == entityCPurchaseReqDt.ID).ToList()[0];
                    entityPurchaseReqDt.GCItemDetailStatus = Constant.TransactionStatus.PROCESSED;
                    PurchaseOrderDt entityPurchaseOrderDt = lstPurchaseOrderDt.FirstOrDefault(p => p.ItemID == entityPurchaseReqDt.ItemID);
                    
                    decimal orderQty = Convert.ToDecimal(entityCPurchaseReqDt.QtyPO);
                    if (entityPurchaseOrderDt == null)
                    {
                        entityPurchaseOrderDt = new PurchaseOrderDt();
                        //entityPurchaseOrderDt.PurchaseRequestID = entityPurchaseReqDt.PurchaseRequestID;
                        entityPurchaseOrderDt.ItemID = entityPurchaseReqDt.ItemID;
                        entityPurchaseOrderDt.Quantity = Convert.ToDecimal(entityCPurchaseReqDt.QtyPO);
                        entityPurchaseOrderDt.GCPurchaseUnit = entityCPurchaseReqDt.GCPurchaseUnit;
                        entityPurchaseOrderDt.GCBaseUnit = entityPurchaseReqDt.GCBaseUnit;
                        entityPurchaseOrderDt.ConversionFactor = Convert.ToDecimal(entityCPurchaseReqDt.ConversionFactor);
                        entityPurchaseOrderDt.UnitPrice = Convert.ToDecimal(entityCPurchaseReqDt.Price);
                        entityPurchaseOrderDt.DiscountPercentage1 = Convert.ToDecimal(entityCPurchaseReqDt.Discount1);
                        entityPurchaseOrderDt.DiscountPercentage2 = Convert.ToDecimal(entityCPurchaseReqDt.Discount2);
                        entityPurchaseOrderDt.IsBonusItem = false;

                        decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                        lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage1) / 100;
                        lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage2) / 100;
                        entityPurchaseOrderDt.LineAmount = lineAmount;
                        entityPurchaseOrderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityPurchaseOrderDt.CreatedBy = AppSession.UserLogin.UserID;

                        entityPurchaseOrderDt.PurchaseOrderID = Convert.ToInt32(hdnID.Value);
                        entityPurchaseOrderDtDao.Insert(entityPurchaseOrderDt);
                    }
                    else
                    {
                        decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                        lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage1) / 100;
                        lineAmount = lineAmount * (100 - entityPurchaseOrderDt.DiscountPercentage2) / 100;

                        entityPurchaseOrderDt.LineAmount = lineAmount;
                        entityPurchaseOrderDt.Quantity += orderQty;
                        entityPurchaseOrderDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPurchaseOrderDtDao.Update(entityPurchaseOrderDt);
                    }

                    entityPurchaseReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPurchaseRequestDtDao.Update(entityPurchaseReqDt);                    

                    PurchaseRequestPO entityPRPO = new PurchaseRequestPO();
                    entityPRPO.PurchaseOrderID = entityPurchaseOrderDt.PurchaseOrderID;
                    entityPRPO.ItemID = entityPurchaseOrderDt.ItemID;
                    entityPRPO.PurchaseRequestID = Convert.ToInt32(hdnPurchaseRequestID.Value);
                    entityPRPO.OrderQuantity = orderQty;
                    entityPRPODao.Insert(entityPRPO);
                }

                int count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED), ctx);
                retval = count.ToString();
                if (count == 0)
                {
                    PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(hdnPurchaseRequestID.Value));
                    entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
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
            return result;
        }
    }
}