using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentOrderAddFromReceiveCtl : BaseEntryPopupCtl
    {
        protected string filterExpressionPurchaseOrder = "";
        public override void InitializeDataControl(string param)
        {
            hdnIsUsedProductLineReceiveCtl.Value = AppSession.IsUsedProductLine;

            //hdnSupplierID.Value = param;
            //filterExpressionPurchaseOrder = string.Format("BusinessPartnerID = '{0}' AND GCTransactionStatus = '{1}' AND TransactionCode = '{2}'", hdnSupplierID.Value, Constant.TransactionStatus.APPROVED, Constant.TransactionCode.CONSIGNMENT_ORDER);
            
            BindGridView();
        }

        public String GetPurchaseReceiveExpression()
        {
            //Int32 SupplierID = ((ConsignmentOrder)Page).GetSupplierID();
            //return String.Format("PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM vPurchaseReceivePOCustom WHERE (ReceivedQuantity - OrderQuantity) > 0 AND SupplierID = {0}) AND SupplierID = {0} AND GCTransactionStatus != '{1}'", SupplierID, Constant.TransactionStatus.VOID);

            Int32 oSupplierID = ((ConsignmentOrder)Page).GetSupplierID();
            Int32 oLocationID = ((ConsignmentOrder)Page).GetLocationID();
            Int32 oProductLineID = ((ConsignmentOrder)Page).GetProductLineID();

            if (hdnIsUsedProductLineReceiveCtl.Value == "1")
            {
                return string.Format("SupplierID = {0} AND LocationID = {1} AND ProductLineID = {2}", oSupplierID, oLocationID, oProductLineID);
            }
            else
            {
                return string.Format("SupplierID = {0} AND LocationID = {1}", oSupplierID, oLocationID);
            }
        }

        private void BindGridView()
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseReceiveID.Value != "") 
            {
                int orderID = ((ConsignmentOrder)Page).GetOrderID();
                filterExpression = string.Format("PurchaseReceiveID = {0} AND ISNULL(OrderQuantity,0) < (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0)) AND (ISNULL(ReceivedQuantity,0) - ISNULL(ReturnQuantity,0) - ISNULL(OrderQuantity,0)) > 0 AND GCItemDetailStatus != '{2}' ORDER BY ItemName1 ASC",
                                        hdnPurchaseReceiveID.Value, orderID, Constant.TransactionStatus.VOID);

                ////filterExpression = string.Format("PurchaseReceiveID = {0} AND OrderQuantity < ReceivedQuantity AND (ReceivedQuantity - OrderQuantity) > 0 AND ItemID NOT IN (SELECT ItemID FROM PurchaseOrderDt WHERE PurchaseOrderID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}') AND GCItemDetailStatus != '{2}' ORDER BY ItemName1 ASC",
                ////                        hdnPurchaseReceiveID.Value, orderID, Constant.TransactionStatus.VOID);
            }
            
            List<vPurchaseReceivePOCustom> lstEntity = BusinessLayer.GetvPurchaseReceivePOCustomList(filterExpression);
            grdPopupView.DataSource = lstEntity;
            grdPopupView.DataBind();
        }

        protected void grdPopupView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReceivePOCustom entity = e.Row.DataItem as vPurchaseReceivePOCustom;
                TextBox txtReceivedQty = e.Row.FindControl("txtReceivedQty") as TextBox;
                TextBox txtReturnQty = e.Row.FindControl("txtReturnQty") as TextBox;
                TextBox txtPOQty = e.Row.FindControl("txtPOQty") as TextBox;
                TextBox txtQuantity = e.Row.FindControl("txtQuantity") as TextBox;

                txtReceivedQty.Text = entity.ReceivedQuantity.ToString();
                txtReturnQty.Text = entity.ReturnQuantity.ToString();
                txtPOQty.Text = entity.OrderQuantity.ToString();
                txtQuantity.Text = (entity.RequiredOrderQuantity - entity.OrderQuantity).ToString();                
            }
        }

        protected void cbpViewPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntityDt(PurchaseOrderDt entityDt, String[] data) 
        {
            entityDt.ItemID = Convert.ToInt32(data[0]);
            entityDt.Quantity = Convert.ToDecimal(data[1]);
            entityDt.GCPurchaseUnit = data[2];
            entityDt.GCBaseUnit = data[3];
            entityDt.ConversionFactor = Convert.ToDecimal(data[4]);
            entityDt.UnitPrice = Convert.ToDecimal(data[5]);

            entityDt.IsDiscountInPercentage1 = Convert.ToBoolean(data[6]);
            entityDt.IsDiscountInPercentage2 = Convert.ToBoolean(data[7]);
            entityDt.DiscountPercentage1 = Convert.ToDecimal(data[8]);
            entityDt.DiscountPercentage2 = Convert.ToDecimal(data[9]);
            entityDt.DiscountAmount1 = Convert.ToDecimal(data[10]);
            entityDt.DiscountAmount2 = Convert.ToDecimal(data[11]);

            entityDt.LineAmount = entityDt.CustomSubTotal;
            //entityDt.ReceivedQuantity = Convert.ToDecimal(data[9]);
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

            //Simpan data draft
            entityDt.DraftQuantity = Convert.ToDecimal(data[1]);
            entityDt.DraftUnitPrice = Convert.ToDecimal(data[5]);
            entityDt.DraftDiscountPercentage1 = Convert.ToDecimal(data[8]);
            entityDt.DraftDiscountPercentage2 = Convert.ToDecimal(data[9]);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceivePODao purchaseReceivePODao = new PurchaseReceivePODao(ctx);
            PurchaseOrderDtDao purchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseReceiveDtDao purchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderHdDao purchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            
            try
            {
                Int32 OrderID = ((ConsignmentOrder)Page).GetOrderID();
                Int32 PurchaseReceiveID = Convert.ToInt32(hdnPurchaseReceiveID.Value);
                Int32 OrderDtID = 0;

                ((ConsignmentOrder)Page).SavePurchaseOrderHd(ctx, ref OrderID);

                if (purchaseOrderHdDao.Get(OrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<String> items = hdnLstItem.Value.Split('|').ToList();
                    items.Remove("");
                    PurchaseOrderDt entityDt = null;
                    PurchaseReceivePO entityPO = null;
                    PurchaseReceiveDt receiveDt = null;

                    foreach (String item in items)
                    {
                        String[] data = item.Split(';');
                        Int32 itemID = Convert.ToInt32(data[0]);

                        entityDt = new PurchaseOrderDt();
                        entityDt.PurchaseOrderID = OrderID;
                        ControlToEntityDt(entityDt, data);
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        OrderDtID = purchaseOrderDtDao.InsertReturnPrimaryKeyID(entityDt);

                        entityPO = new PurchaseReceivePO();
                        entityPO.ItemID = itemID;
                        entityPO.PurchaseOrderID = OrderID;
                        entityPO.PurchaseReceiveID = PurchaseReceiveID;
                        entityPO.ReceivedQuantity = Convert.ToDecimal(data[13]);
                        entityPO.OrderedQuantity = entityDt.Quantity * entityDt.ConversionFactor;
                        purchaseReceivePODao.Insert(entityPO);

                        receiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format("PurchaseReceiveID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'", PurchaseReceiveID, itemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                        receiveDt.PurchaseOrderID = OrderID;
                        receiveDt.PurchaseOrderDtID = OrderDtID;
                        receiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReceiveDtDao.Update(receiveDt);
                    }

                    retval = purchaseOrderHdDao.Get(OrderID).PurchaseOrderNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", purchaseOrderHdDao.Get(OrderID).PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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