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
    public partial class PurchaseOrderCloseNewCtl : BaseViewPopupCtl
    {
        protected string filterExpressionSupplier = "";

        public override void InitializeDataControl(string param)
        {
            hdnPurchaseOrderIDCtlClosedNew.Value = param;

            string filterPOHD = string.Format("PurchaseOrderID = {0}", param);

            vPurchaseOrderHd orderHd = BusinessLayer.GetvPurchaseOrderHdList(filterPOHD).FirstOrDefault();
            txtPurchaseOrderNo.Text = orderHd.PurchaseOrderNo;
            hdnSupplierID.Value = orderHd.BusinessPartnerID.ToString();
            txtSupplierCode.Text = orderHd.BusinessPartnerCode;
            txtSupplierName.Text = orderHd.BusinessPartnerName;

            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsBlackList = 0 AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);

            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CLOSED_PO_REASON);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboClosedPOReason, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.CLOSED_PO_REASON).ToList(), "StandardCodeName", "StandardCodeID");
            cboClosedPOReason.SelectedIndex = 0;
        }

        protected void cbpCloseNewPO_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (OnProcessRecord(ref errMessage))
                result = "success";
            else
                result = "fail|" + errMessage;

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void CopyToEntityHd(PurchaseOrderHd newEntity, PurchaseOrderHd oldEntity)
        {
            newEntity.LocationID = oldEntity.LocationID;
            newEntity.ProductLineID = oldEntity.ProductLineID;
            newEntity.RevenueCostCenterID = oldEntity.RevenueCostCenterID;
            newEntity.TransactionCode = Constant.TransactionCode.PURCHASE_ORDER;
            newEntity.DeliveryDate = oldEntity.DeliveryDate;
            newEntity.POExpiredDate = oldEntity.POExpiredDate;
            newEntity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            newEntity.PaymentRemarks = oldEntity.PaymentRemarks;
            newEntity.Remarks = oldEntity.Remarks;
            newEntity.OtherReferenceNo = newEntity.OtherReferenceNo;
            newEntity.OtherRequestReferenceNo = newEntity.OtherRequestReferenceNo;
            newEntity.IsUrgent = oldEntity.IsUrgent;
            newEntity.IsCampaign = oldEntity.IsCampaign;
            newEntity.PaymentRemarks = oldEntity.PaymentRemarks;
            newEntity.DownPaymentReferenceNo = oldEntity.DownPaymentReferenceNo;
            newEntity.DownPaymentAmount = oldEntity.DownPaymentAmount;

            newEntity.IsIncludeVAT = oldEntity.IsIncludeVAT;
            if (newEntity.IsIncludeVAT)
                newEntity.VATPercentage = oldEntity.VATPercentage;
            else
                newEntity.VATPercentage = 0;

            newEntity.GCPPHType = oldEntity.GCPPHType;
            newEntity.PPHMode = oldEntity.PPHMode;
            newEntity.IsPPHInPercentage = oldEntity.IsPPHInPercentage;
            newEntity.PPHPercentage = oldEntity.PPHPercentage;
            newEntity.PPHAmount = oldEntity.PPHAmount;
            newEntity.IsIncludePPh = oldEntity.IsIncludePPh;

            newEntity.IsFinalDiscountInPercentage = oldEntity.IsFinalDiscountInPercentage;
            newEntity.FinalDiscount = oldEntity.FinalDiscount;
            newEntity.FinalDiscountAmount = oldEntity.FinalDiscountAmount;

            newEntity.OrderDate = Helper.GetDatePickerValue(DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT));
            newEntity.GCPurchaseOrderType = oldEntity.GCPurchaseOrderType;
            newEntity.TermID = oldEntity.TermID;
            newEntity.GCFrancoRegion = oldEntity.GCFrancoRegion;
            newEntity.GCCurrencyCode = oldEntity.GCCurrencyCode;
            newEntity.CurrencyRate = oldEntity.CurrencyRate;
            newEntity.GCChargesType = oldEntity.GCChargesType;
        }

        private void CopyToEntityDt(PurchaseOrderDt newEntityDt, PurchaseOrderDt oldEntityDt)
        {
            newEntityDt.ItemID = oldEntityDt.ItemID;
            newEntityDt.Quantity = oldEntityDt.Quantity;
            newEntityDt.DraftQuantity = oldEntityDt.DraftQuantity;
            newEntityDt.GCPurchaseUnit = oldEntityDt.GCPurchaseUnit;
            newEntityDt.GCBaseUnit = oldEntityDt.GCBaseUnit;
            newEntityDt.ConversionFactor = oldEntityDt.ConversionFactor;
            newEntityDt.PurchaseRequestID = oldEntityDt.PurchaseRequestID;

            if (chkIsUsingNewPrice.Checked)
            {
                List<GetItemMasterPurchase> itemMasterPurchaseLst = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, oldEntityDt.ItemID, Convert.ToInt32(hdnSupplierID.Value));
                if (itemMasterPurchaseLst.Count() > 0)
                {
                    List<GetItemMasterPurchase> itemMasterPurchaseLst2 = itemMasterPurchaseLst.Where(a => a.PurchaseUnit == oldEntityDt.GCPurchaseUnit).ToList();
                    if (itemMasterPurchaseLst2.Count() > 0)
                    {
                        GetItemMasterPurchase itemMasterPurchase = itemMasterPurchaseLst2.FirstOrDefault();
                        newEntityDt.UnitPrice = itemMasterPurchase.UnitPrice;
                        newEntityDt.IsDiscountInPercentage1 = true;
                        newEntityDt.DiscountPercentage1 = itemMasterPurchase.Discount;
                        newEntityDt.IsDiscountInPercentage2 = true;
                        newEntityDt.DiscountPercentage2 = itemMasterPurchase.Discount2;

                        decimal total = oldEntityDt.Quantity * itemMasterPurchase.UnitPrice;
                        decimal discAmount1 = total * itemMasterPurchase.Discount / 100;
                        decimal totaltemp = total - discAmount1;
                        decimal discAmount2 = totaltemp * itemMasterPurchase.Discount2 / 100;
                        decimal totalend = total - discAmount1 - discAmount2;

                        newEntityDt.DiscountAmount1 = discAmount1;
                        newEntityDt.DiscountAmount2 = discAmount2;

                        newEntityDt.LineAmount = totalend;

                        newEntityDt.DraftUnitPrice = itemMasterPurchase.UnitPrice;
                        newEntityDt.IsDraftDiscountInPercentage1 = true;
                        newEntityDt.DraftDiscountPercentage1 = itemMasterPurchase.Discount;
                        newEntityDt.IsDraftDiscountInPercentage2 = true;
                        newEntityDt.DraftDiscountPercentage2 = itemMasterPurchase.Discount2;
                    }
                    else
                    {
                        List<GetItemMasterPurchase> itemMasterPurchaseLst3 = itemMasterPurchaseLst.Where(a => a.PurchaseUnit == oldEntityDt.GCBaseUnit).ToList();
                        if (itemMasterPurchaseLst3.Count() > 0)
                        {
                            GetItemMasterPurchase itemMasterPurchase = itemMasterPurchaseLst3.FirstOrDefault();
                            newEntityDt.UnitPrice = itemMasterPurchase.UnitPrice * oldEntityDt.ConversionFactor;
                            newEntityDt.IsDiscountInPercentage1 = true;
                            newEntityDt.DiscountPercentage1 = itemMasterPurchase.Discount;
                            newEntityDt.IsDiscountInPercentage2 = true;
                            newEntityDt.DiscountPercentage2 = itemMasterPurchase.Discount2;

                            decimal total = oldEntityDt.Quantity * newEntityDt.UnitPrice;
                            decimal discAmount1 = total * itemMasterPurchase.Discount / 100;
                            decimal totaltemp = total - discAmount1;
                            decimal discAmount2 = totaltemp * itemMasterPurchase.Discount2 / 100;
                            decimal totalend = total - discAmount1 - discAmount2;

                            newEntityDt.DiscountAmount1 = discAmount1;
                            newEntityDt.DiscountAmount2 = discAmount2;

                            newEntityDt.LineAmount = totalend;

                            newEntityDt.DraftUnitPrice = itemMasterPurchase.UnitPrice;
                            newEntityDt.IsDraftDiscountInPercentage1 = true;
                            newEntityDt.DraftDiscountPercentage1 = itemMasterPurchase.Discount;
                            newEntityDt.IsDraftDiscountInPercentage2 = true;
                            newEntityDt.DraftDiscountPercentage2 = itemMasterPurchase.Discount2;
                        }
                        else
                        {
                            GetItemMasterPurchase itemMasterPurchase = itemMasterPurchaseLst.FirstOrDefault();
                            newEntityDt.UnitPrice = itemMasterPurchase.Price * oldEntityDt.ConversionFactor;
                            newEntityDt.IsDiscountInPercentage1 = true;
                            newEntityDt.DiscountPercentage1 = itemMasterPurchase.Discount;
                            newEntityDt.IsDiscountInPercentage2 = true;
                            newEntityDt.DiscountPercentage2 = itemMasterPurchase.Discount2;

                            decimal total = oldEntityDt.Quantity * newEntityDt.UnitPrice;
                            decimal discAmount1 = total * itemMasterPurchase.Discount / 100;
                            decimal totaltemp = total - discAmount1;
                            decimal discAmount2 = totaltemp * itemMasterPurchase.Discount2 / 100;
                            decimal totalend = total - discAmount1 - discAmount2;

                            newEntityDt.DiscountAmount1 = discAmount1;
                            newEntityDt.DiscountAmount2 = discAmount2;

                            newEntityDt.LineAmount = totalend;

                            newEntityDt.DraftUnitPrice = newEntityDt.UnitPrice;
                            newEntityDt.IsDraftDiscountInPercentage1 = newEntityDt.IsDiscountInPercentage1;
                            newEntityDt.DraftDiscountPercentage1 = newEntityDt.DiscountPercentage1;
                            newEntityDt.IsDraftDiscountInPercentage2 = newEntityDt.IsDiscountInPercentage2;
                            newEntityDt.DraftDiscountPercentage2 = newEntityDt.DiscountPercentage2;
                        }
                    }
                }
                else
                {
                    newEntityDt.UnitPrice = 0;
                    newEntityDt.IsDiscountInPercentage1 = true;
                    newEntityDt.DiscountPercentage1 = 0;
                    newEntityDt.DiscountAmount1 = 0;
                    newEntityDt.IsDiscountInPercentage2 = true;
                    newEntityDt.DiscountPercentage2 = 0;
                    newEntityDt.DiscountAmount2 = 0;
                    newEntityDt.LineAmount = 0;

                    newEntityDt.DraftUnitPrice = 0;
                    newEntityDt.IsDraftDiscountInPercentage1 = true;
                    newEntityDt.DraftDiscountPercentage1 = 0;
                    newEntityDt.DraftDiscountAmount1 = 0;
                    newEntityDt.IsDraftDiscountInPercentage2 = true;
                    newEntityDt.DraftDiscountPercentage2 = 0;
                    newEntityDt.DraftDiscountAmount2 = 0;
                }
            }
            else
            {
                newEntityDt.UnitPrice = oldEntityDt.UnitPrice;
                newEntityDt.IsDiscountInPercentage1 = oldEntityDt.IsDiscountInPercentage1;
                newEntityDt.DiscountPercentage1 = oldEntityDt.DiscountPercentage1;
                newEntityDt.DiscountAmount1 = oldEntityDt.DiscountAmount1;
                newEntityDt.IsDiscountInPercentage2 = oldEntityDt.IsDiscountInPercentage2;
                newEntityDt.DiscountPercentage2 = oldEntityDt.DiscountPercentage2;
                newEntityDt.DiscountAmount2 = oldEntityDt.DiscountAmount2;
                newEntityDt.LineAmount = oldEntityDt.LineAmount;

                newEntityDt.DraftUnitPrice = oldEntityDt.DraftUnitPrice;
                newEntityDt.IsDraftDiscountInPercentage1 = oldEntityDt.IsDraftDiscountInPercentage1;
                newEntityDt.DraftDiscountPercentage1 = oldEntityDt.DraftDiscountPercentage1;
                newEntityDt.DraftDiscountAmount1 = oldEntityDt.DraftDiscountAmount1;
                newEntityDt.IsDraftDiscountInPercentage2 = oldEntityDt.IsDraftDiscountInPercentage2;
                newEntityDt.DraftDiscountPercentage2 = oldEntityDt.DraftDiscountPercentage2;
                newEntityDt.DraftDiscountAmount2 = oldEntityDt.DraftDiscountAmount2;
            }

            newEntityDt.IsBonusItem = oldEntityDt.IsBonusItem;
            newEntityDt.Remarks = oldEntityDt.Remarks;
            newEntityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

        }

        private bool OnProcessRecord(ref string errMessage)
        {
            bool result = true;
            int OrderID = 0;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao POHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao PODtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestPODao PRPODao = new PurchaseRequestPODao(ctx);

            try
            {
                string filterExpressionPODtNew = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0 AND ReceivedInformation IS NULL", hdnPurchaseOrderIDCtlClosedNew.Value);
                List<PurchaseOrderDt> lstPurchaseOrderDtnew = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPODtNew, ctx);
                if (lstPurchaseOrderDtnew.Count > 0)
                {
                    PurchaseOrderHd POHd = POHdDao.Get(Convert.ToInt32(hdnPurchaseOrderIDCtlClosedNew.Value));
                    POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                    POHd.GCClosedReason = cboClosedPOReason.Value.ToString();
                    if (cboClosedPOReason.Value.ToString() == Constant.POClosedReason.OTHER)
                    {
                        POHd.ClosedReason = txtReason.Text;
                    }
                    POHd.ClosedBy = AppSession.UserLogin.UserID;
                    POHd.ClosedDate = DateTime.Now;
                    POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    POHdDao.Update(POHd);

                    string filterExpressionPODt = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnPurchaseOrderIDCtlClosedNew.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPODt, ctx);
                    foreach (PurchaseOrderDt PODt in lstPurchaseOrderDt)
                    {
                        PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                        PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PODtDao.Update(PODt);
                    }

                    PurchaseOrderHd entityHd = new PurchaseOrderHd();
                    CopyToEntityHd(entityHd, POHd);
                    entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_ORDER, entityHd.OrderDate, ctx);
                    entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                    entityHd.ReferenceNo = POHd.PurchaseOrderNo;
                    entityHd.CreatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    OrderID = POHdDao.InsertReturnPrimaryKeyID(entityHd);


                    foreach (PurchaseOrderDt entity in lstPurchaseOrderDtnew)
                    {
                        PurchaseOrderDt entityDt = new PurchaseOrderDt();
                        CopyToEntityDt(entityDt, entity);
                        entityDt.PurchaseOrderID = OrderID;
                        entityDt.IsBySystem = true;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PODtDao.Insert(entityDt);

                        if (entityDt.PurchaseRequestID != null && entityDt.PurchaseRequestID != 0)
                        {
                            PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                        "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                        entityDt.PurchaseRequestID, entityDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                            string[] poInfArr = entityPRDt.OrderInformation.Split('|');
                            string poInfNew = "";

                            for (int i = 0; i < poInfArr.Count(); i++)
                            {
                                if (poInfArr[i] != "")
                                {
                                    if (poInfArr[i] != POHd.PurchaseOrderID.ToString())
                                    {
                                        poInfNew += "|" + poInfArr[i];
                                    }
                                }
                            }
                            entityPRDt.OrderInformation = poInfNew + "|" + entityDt.PurchaseOrderID;

                            entityPRDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPRDTDao.Update(entityPRDt);
                        }
                    }

                    List<PurchaseRequestPO> lstPRPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", POHd.PurchaseOrderID), ctx);
                    if (lstPRPO != null)
                    {
                        foreach (PurchaseRequestPO prpo in lstPRPO)
                        {
                            prpo.PurchaseOrderID = OrderID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            PRPODao.Update(prpo);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Tidak dapat dilakukan proses close & new karena semua detail pemesanan sudah ada penerimaan");
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