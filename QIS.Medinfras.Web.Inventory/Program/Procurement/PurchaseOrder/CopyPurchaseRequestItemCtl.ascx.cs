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
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CopyPurchaseRequestItemCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        public List<DataTempPurchaseRequest> lstTemp = new List<DataTempPurchaseRequest>();

        private PurchaseOrder DetailPage
        {
            get { return (PurchaseOrder)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            hdnPurchaseOrderIDCtl.Value = temp[0];
            hdnSupplierIDCtl.Value = temp[1];
            hdnLocationIDCtl.Value = temp[2];
            hdnProductLineIDCtl.Value = temp[3];
            hdnPurchaseOrderTypeCtl.Value = temp[4];
            hdnIM0131Ctl.Value = temp[5];
            hdnOrderDateCtl.Value = temp[6];

            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT, //1
                                                            Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY //2
                                                        );
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterExpression);
            hdnIsUsedPurchaseOrderType.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_IS_PURCHASE_REQUEST_SERVICE_UNIT).FirstOrDefault().ParameterValue;
            hdnIsPOQtyCannotOverPRQty.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY).FirstOrDefault().ParameterValue;

            Helper.SetControlEntrySetting(txtPurhcaseRequestNo, new ControlEntrySetting(false, false, true), "mpDrugsQuickPicks");
            SetControlEntrySetting(txtPurhcaseRequestNo, new ControlEntrySetting(false, false, true));
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseRequestDtOutstanding entity = e.Row.DataItem as vPurchaseRequestDtOutstanding;
                TextBox txtQty = e.Row.FindControl("txtQty") as TextBox;
                TextBox txtUnitPrice = e.Row.FindControl("txtUnitPrice") as TextBox;
                TextBox txtDiscount1 = e.Row.FindControl("txtDiscount1") as TextBox;
                TextBox txtDiscount2 = e.Row.FindControl("txtDiscount2") as TextBox;
                HtmlGenericControl lblPurchaseUnit = e.Row.FindControl("lblPurchaseUnitCtl") as HtmlGenericControl;
                HtmlInputHidden hdnConversionFactor = (HtmlInputHidden)e.Row.FindControl("hdnConversionFactor");
                HtmlInputHidden hdnGCPurchaseUnit = (HtmlInputHidden)e.Row.FindControl("hdnGCPurchaseUnit");

                txtQty.Text = entity.cfRemainingQtyBaseUnit.ToString();
                //txtQty.Attributes.Add("max", entity.cfRemainingQtyBaseUnit.ToString());
                txtUnitPrice.Text = entity.cfUnitPriceBaseUnit.ToString("N2");
                txtDiscount1.Text = entity.DiscountPercentage.ToString("N2");
                txtDiscount2.Text = entity.DiscountPercentage2.ToString("N2");
                lblPurchaseUnit.InnerText = entity.DefaultPurchaseUnit;
                hdnConversionFactor.Value = entity.DefaultConversionFactor.ToString();
                hdnGCPurchaseUnit.Value = entity.DefaultGCPurchaseUnit;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseRequestID IN ({0}) AND GCItemDetailStatus = '{1}' AND IsDeleted = 0  AND (OrderedQuantity < Quantity)", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED);
            //if (!string.IsNullOrEmpty(hdnPurchaseOrderIDCtl.Value) && hdnPurchaseOrderIDCtl.Value != "0")
            //{
            //    filterExpression += string.Format(" AND ItemID NOT IN (SELECT ItemID FROM PurchaseOrderDt WHERE PurchaseOrderID = '{0}' AND IsDeleted = 0)", hdnPurchaseOrderIDCtl.Value);
            //}
            filterExpression += string.Format(" ORDER BY BusinessPartnerName, ItemName1");

            List<vPurchaseRequestDtOutstanding> lstEntity = BusinessLayer.GetvPurchaseRequestDtOutstandingList(filterExpression);
            List<vPurchaseRequestDtOutstanding> lstEntity1 = new List<vPurchaseRequestDtOutstanding>();

            lstEntity1 = (from bs in lstEntity
                          group bs by bs.ItemID into g
                          select new vPurchaseRequestDtOutstanding
                          {
                              ItemID = g.Key,
                              ItemName1 = g.First().ItemName1,
                              TransactionDate = g.First().TransactionDate,
                              TransactionTime = g.First().TransactionTime,
                              ApprovedDate = g.First().ApprovedDate,
                              ApprovedByName = g.First().ApprovedByName,
                              IsUrgent = g.First().IsUrgent,
                              BusinessPartnerID = g.First().BusinessPartnerID,
                              BusinessPartnerName = g.First().BusinessPartnerName,
                              BusinessPartnerShortName = g.First().BusinessPartnerShortName,
                              QuantityMAX = g.First().QuantityMAX,
                              QuantityMIN = g.First().QuantityMIN,
                              QtyOnHandAll = g.First().QtyOnHandAll,
                              Quantity = g.Sum(x => x.cfRemainingQty),
                              QtyOnOrder = g.First().QtyOnOrder,
                              PurchaseUnit = g.First().PurchaseUnit,
                              ConversionFactor = g.First().ConversionFactor,
                              QuantityEND = g.First().QuantityEND,
                              BaseUnit = g.First().BaseUnit,
                              DefaultGCPurchaseUnit = g.First().DefaultGCPurchaseUnit,
                              DefaultPurchaseUnit = g.First().DefaultPurchaseUnit,
                              DefaultConversionFactor = g.First().DefaultConversionFactor,
                              SupplierItemCode = g.First().SupplierItemCode,
                              SupplierItemName = g.First().SupplierItemName,
                              UnitPrice = g.First().UnitPrice,
                              DiscountPercentage = g.First().DiscountPercentage,
                              DiscountPercentage2 = g.First().DiscountPercentage2,
                              GCPurchaseUnit = g.First().GCPurchaseUnit,
                              GCBaseUnit = g.First().GCBaseUnit,
                              TermID = g.First().TermID,
                              Remarks = g.First().Remarks
                          }).ToList();

            grdView.DataSource = lstEntity1;
            grdView.DataBind();
        }

        protected void setDataBeforeSave()
        {
            string[] paramNew = hdnDataSave.Value.Split('$');
            lstTemp = new List<DataTempPurchaseRequest>();

            for (int i = 0; i < paramNew.Length; i++)
            {
                if (!String.IsNullOrEmpty(paramNew[i]))
                {
                    string[] paramNewSplit = paramNew[i].Split('|');
                    int keyNew = Convert.ToInt32(paramNewSplit[1]);
                    Decimal qtyNew = Convert.ToDecimal(paramNewSplit[2]);
                    Decimal amountNew = Convert.ToDecimal(paramNewSplit[3]);
                    String gcPurchaseUnitNew = paramNewSplit[4];
                    Decimal conversionFactorNew = Convert.ToDecimal(paramNewSplit[5]);
                    decimal disc1New = Convert.ToDecimal(paramNewSplit[6]);
                    decimal disc2New = Convert.ToDecimal(paramNewSplit[7]);

                    DataTempPurchaseRequest oData = new DataTempPurchaseRequest();
                    oData.Key = keyNew;
                    oData.Qty = qtyNew;
                    oData.Amount = amountNew;
                    oData.ConversionFactor = conversionFactorNew;
                    oData.GCPurchaseUnit = gcPurchaseUnitNew;
                    oData.Disc1 = disc1New;
                    oData.Disc2 = disc2New;
                    lstTemp.Add(oData);
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderDtDao entityPurchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPurchaseRequestDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPurchaseRequestHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                int TransactionID = Convert.ToInt32(hdnPurchaseOrderIDCtl.Value);
                setDataBeforeSave();
                DetailPage.SavePurchaseOrderHd(ctx, ref TransactionID);

                foreach (DataTempPurchaseRequest e in lstTemp)
                {
                    PurchaseRequestDt entityPurchaseReqDt = BusinessLayer.GetPurchaseRequestDtList(string.Format("PurchaseRequestID IN ({0}) AND ItemID IN ({1}) AND IsDeleted = 0 AND GCItemDetailStatus = '{2}' AND (OrderedQuantity < Quantity)", hdnPurchaseRequestID.Value, e.Key, Constant.TransactionStatus.APPROVED), ctx).FirstOrDefault();
                    if (entityPurchaseReqDt != null)
                    {
                        string filterPO = string.Format("PurchaseOrderID = '{0}' AND ItemID = '{1}' AND IsDeleted = 0", TransactionID, entityPurchaseReqDt.ItemID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PurchaseOrderDt entityPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterPO, ctx).FirstOrDefault();
                        if (entityPurchaseOrderDt == null)
                        {
                            entityPurchaseOrderDt = new PurchaseOrderDt();
                            entityPurchaseOrderDt.PurchaseRequestID = entityPurchaseReqDt.PurchaseRequestID;
                            entityPurchaseOrderDt.ItemID = entityPurchaseReqDt.ItemID;
                            entityPurchaseOrderDt.Quantity = e.Qty;
                            entityPurchaseOrderDt.GCPurchaseUnit = e.GCPurchaseUnit;
                            entityPurchaseOrderDt.GCBaseUnit = entityPurchaseReqDt.GCBaseUnit;
                            entityPurchaseOrderDt.ConversionFactor = e.ConversionFactor;
                            entityPurchaseOrderDt.UnitPrice = e.Amount;

                            entityPurchaseOrderDt.DiscountPercentage1 = e.Disc1;
                            entityPurchaseOrderDt.IsDiscountInPercentage1 = entityPurchaseOrderDt.DiscountPercentage1 > 0 ? true : false;
                            entityPurchaseOrderDt.DiscountAmount1 = Convert.ToDecimal((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) * entityPurchaseOrderDt.DiscountPercentage1 / 100);

                            entityPurchaseOrderDt.DiscountPercentage2 = e.Disc2;
                            entityPurchaseOrderDt.IsDiscountInPercentage2 = entityPurchaseOrderDt.DiscountPercentage2 > 0 ? true : false;
                            entityPurchaseOrderDt.DiscountAmount2 = Convert.ToDecimal(((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) - entityPurchaseOrderDt.DiscountAmount1) * entityPurchaseOrderDt.DiscountPercentage2 / 100);

                            entityPurchaseOrderDt.IsBonusItem = false;

                            decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                            entityPurchaseOrderDt.DiscountAmount1 = e.Disc1 * lineAmount / 100;
                            entityPurchaseOrderDt.DiscountAmount2 = e.Disc2 * (lineAmount - entityPurchaseOrderDt.DiscountAmount1) / 100;
                            lineAmount = lineAmount - entityPurchaseOrderDt.DiscountAmount1 - entityPurchaseOrderDt.DiscountAmount2;

                            entityPurchaseOrderDt.DraftQuantity = entityPurchaseOrderDt.Quantity;
                            entityPurchaseOrderDt.DraftUnitPrice = entityPurchaseOrderDt.UnitPrice;

                            entityPurchaseOrderDt.IsDraftDiscountInPercentage1 = entityPurchaseOrderDt.IsDiscountInPercentage1;
                            entityPurchaseOrderDt.DraftDiscountPercentage1 = entityPurchaseOrderDt.DiscountPercentage1;
                            entityPurchaseOrderDt.DraftDiscountAmount1 = entityPurchaseOrderDt.DiscountAmount1;

                            entityPurchaseOrderDt.IsDraftDiscountInPercentage2 = entityPurchaseOrderDt.IsDiscountInPercentage2;
                            entityPurchaseOrderDt.DraftDiscountPercentage2 = entityPurchaseOrderDt.DiscountPercentage2;
                            entityPurchaseOrderDt.DraftDiscountAmount2 = entityPurchaseOrderDt.DiscountAmount2;

                            entityPurchaseOrderDt.LineAmount = lineAmount;
                            entityPurchaseOrderDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityPurchaseOrderDt.CreatedBy = AppSession.UserLogin.UserID;

                            entityPurchaseOrderDt.PurchaseOrderID = TransactionID;
                            entityPurchaseOrderDtDao.Insert(entityPurchaseOrderDt);
                        }
                        else
                        {
                            entityPurchaseOrderDt.Quantity = entityPurchaseOrderDt.Quantity + e.Qty;

                            entityPurchaseOrderDt.DiscountAmount1 = Convert.ToDecimal((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) * entityPurchaseOrderDt.DiscountPercentage1 / 100);

                            entityPurchaseOrderDt.DiscountPercentage2 = e.Disc2;
                            entityPurchaseOrderDt.DiscountAmount2 = Convert.ToDecimal(((entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.UnitPrice) - entityPurchaseOrderDt.DiscountAmount1) * entityPurchaseOrderDt.DiscountPercentage2 / 100);

                            decimal lineAmount = entityPurchaseOrderDt.UnitPrice * entityPurchaseOrderDt.Quantity;
                            entityPurchaseOrderDt.DiscountAmount1 = e.Disc1 * lineAmount / 100;
                            entityPurchaseOrderDt.DiscountAmount2 = e.Disc2 * (lineAmount - entityPurchaseOrderDt.DiscountAmount1) / 100;
                            lineAmount = lineAmount - entityPurchaseOrderDt.DiscountAmount1 - entityPurchaseOrderDt.DiscountAmount2;

                            entityPurchaseOrderDt.DraftQuantity = entityPurchaseOrderDt.Quantity;

                            entityPurchaseOrderDt.LineAmount = lineAmount;
                            entityPurchaseOrderDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                            entityPurchaseOrderDtDao.Update(entityPurchaseOrderDt);

                            entityPurchaseOrderDt.Quantity = e.Qty;
                        }

                        decimal orderQuantity = entityPurchaseOrderDt.Quantity;
                        decimal orderQuantityInRequestUnit = entityPurchaseOrderDt.Quantity * entityPurchaseOrderDt.ConversionFactor / entityPurchaseReqDt.ConversionFactor;

                        if (orderQuantityInRequestUnit > 0)
                        {
                            if (entityPurchaseReqDt.GCItemDetailStatus == Constant.TransactionStatus.APPROVED)
                            {
                                decimal tempOrderQuantity = orderQuantity;
                                decimal remainingOrderQuantity = (entityPurchaseReqDt.Quantity - entityPurchaseReqDt.OrderedQuantity);

                                if (entityPurchaseOrderDt.ConversionFactor != entityPurchaseReqDt.ConversionFactor)
                                {
                                    tempOrderQuantity = tempOrderQuantity * entityPurchaseOrderDt.ConversionFactor / entityPurchaseReqDt.ConversionFactor;
                                }

                                if (tempOrderQuantity > remainingOrderQuantity)
                                {
                                    if (hdnIsPOQtyCannotOverPRQty.Value == "1")
                                    {
                                        tempOrderQuantity = remainingOrderQuantity;
                                    }
                                    else
                                    {
                                        tempOrderQuantity = entityPurchaseReqDt.Quantity;
                                    }
                                }

                                string filterCheck = string.Format("PurchaseOrderID = '{0}' AND ItemID = '{1}' AND PurchaseRequestID = '{2}'", TransactionID, entityPurchaseReqDt.ItemID, entityPurchaseReqDt.PurchaseRequestID);
                                PurchaseRequestPO entityPRPOCheck = BusinessLayer.GetPurchaseRequestPOList(filterCheck, ctx).FirstOrDefault();

                                PurchaseRequestPO entityPRPO = new PurchaseRequestPO();
                                entityPRPO.PurchaseOrderID = TransactionID;
                                entityPRPO.ItemID = entityPurchaseReqDt.ItemID;
                                entityPRPO.PurchaseRequestID = entityPurchaseReqDt.PurchaseRequestID;
                                entityPRPO.OrderQuantity = tempOrderQuantity;
                                orderQuantity -= tempOrderQuantity;

                                entityPurchaseReqDt.OrderedQuantity = entityPurchaseReqDt.OrderedQuantity + tempOrderQuantity;
                                string orderInformation = !string.IsNullOrEmpty(entityPurchaseReqDt.OrderInformation) ? entityPurchaseReqDt.OrderInformation + "|" : "|";
                                entityPurchaseReqDt.OrderInformation = string.Format("{0}{1}", orderInformation, entityPurchaseOrderDt.PurchaseOrderID);
                                entityPurchaseReqDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPurchaseRequestDtDao.Update(entityPurchaseReqDt);

                                orderQuantityInRequestUnit -= tempOrderQuantity;
                                entityPRPO.RequestQuantity = entityPurchaseReqDt.Quantity;

                                if (entityPRPOCheck == null)
                                {
                                    entityPRPODao.Insert(entityPRPO);
                                }
                                else
                                {
                                    entityPRPOCheck.OrderQuantity = tempOrderQuantity;
                                    entityPRPODao.Update(entityPRPOCheck);
                                }

                            }
                            else
                            {
                                errMessage = "Harap refresh halaman ini.";
                                result = false;
                            }
                        }
                    }
                    else
                    {
                        errMessage = "Harap refresh halaman ini.";
                        result = false;
                    }
                }

                if (result)
                {
                    int countApproved = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnPurchaseRequestID.Value, Constant.TransactionStatus.APPROVED), ctx);
                    int countNonDecline = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format("PurchaseRequestID = {0} AND GCItemDetailStatus != '{1}' AND IsDeleted = 0", hdnPurchaseRequestID.Value, Constant.TransactionStatus.VOID), ctx);
                    if (countNonDecline == 0)
                    {
                        PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(hdnPurchaseRequestID.Value));
                        entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                        entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
                    }
                    else
                    {
                        if (countApproved == 0)
                        {
                            PurchaseRequestHd entityPurchaseRequestHd = entityPurchaseRequestHdDao.Get(Convert.ToInt32(hdnPurchaseRequestID.Value));
                            entityPurchaseRequestHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                            entityPurchaseRequestHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPurchaseRequestHdDao.Update(entityPurchaseRequestHd);
                        }
                    }

                    retval = TransactionID.ToString();
                    ctx.CommitTransaction();
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

    public class DataTempPurchaseRequest
    {
        public int Key { get; set; }
        public Decimal Qty { get; set; }
        public Decimal Amount { get; set; }
        public Decimal ConversionFactor { get; set; }
        public String GCPurchaseUnit { get; set; }
        public Decimal Disc1 { get; set; }
        public Decimal Disc2 { get; set; }
    }
}