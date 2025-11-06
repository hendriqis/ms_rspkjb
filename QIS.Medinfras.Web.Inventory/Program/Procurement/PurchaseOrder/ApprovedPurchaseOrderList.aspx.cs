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
    public partial class ApprovedPurchaseOrderList : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.APPROVED_PURCHASE_ORDER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);

            SettingParameterDt setvarDTR = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC);

            if (setvarDTR.ParameterValue != "" && setvarDTR.ParameterValue != "0" && setvarDTR.ParameterValue != null)
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

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //string filterExpression = hdnFilterExpression.Value;
            //if (filterExpression != "")
            //    filterExpression += " AND ";

            string locationID = hdnLocationIDFrom.Value.ToString();
            string filterExpression;
            if (locationID != "" && locationID != null)
            {
                filterExpression = String.Format("GCTransactionStatus IN ('{0}','{1}') AND TransactionCode = '{2}' AND LocationID = {3}",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED, Constant.TransactionCode.PURCHASE_ORDER, locationID);
            }
            else
            {
                filterExpression = String.Format("GCTransactionStatus IN ('{0}','{1}') AND TransactionCode = '{2}'",
                    Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.PROCESSED, Constant.TransactionCode.PURCHASE_ORDER);
            }

            if (hdnFilterExpressionQuickSearch.Value != "" && hdnFilterExpressionQuickSearch.Value != null)
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

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
            newEntity.TransactionCode = Constant.TransactionCode.PURCHASE_ORDER;
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
            newEntityDt.DraftQuantity = oldEntityDt.DraftQuantity;
            newEntityDt.DraftUnitPrice = oldEntityDt.DraftUnitPrice;
            newEntityDt.IsDraftDiscountInPercentage1 = oldEntityDt.IsDraftDiscountInPercentage1;
            newEntityDt.DraftDiscountPercentage1 = oldEntityDt.DraftDiscountPercentage1;
            newEntityDt.DraftDiscountAmount1 = oldEntityDt.DraftDiscountAmount1;
            newEntityDt.IsDraftDiscountInPercentage2 = oldEntityDt.IsDraftDiscountInPercentage2;
            newEntityDt.DraftDiscountPercentage2 = oldEntityDt.DraftDiscountPercentage2;
            newEntityDt.DraftDiscountAmount2 = oldEntityDt.DraftDiscountAmount2;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            int OrderID;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao POHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao PODtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestPODao PRPODao = new PurchaseRequestPODao(ctx);

            try
            {
                string filterExpressionPOHd = String.Format("PurchaseOrderID = {0}", hdnID.Value);

                //List<PurchaseOrderHd> lstPurchaseOrderHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPOHd);
                //foreach (PurchaseOrderHd POHd in lstPurchaseOrderHd)
                //{
                //    POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                //    POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                //    POHdDao.Update(POHd);
                //}

                PurchaseOrderHd POHd = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPOHd).FirstOrDefault();

                POHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                POHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                POHdDao.Update(POHd);

                List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPOHd, ctx);
                foreach (PurchaseOrderDt PODt in lstPurchaseOrderDt)
                {
                    PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                    PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    PODtDao.Update(PODt);
                }

                //PurchaseOrderDt PODt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPOHd, ctx).FirstOrDefault();
                //PODt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                //PODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                //PODtDao.Update(PODt);

                PurchaseOrderHd entityHd = new PurchaseOrderHd();
                CopyToEntityHd(entityHd, POHd);
                entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.PURCHASE_ORDER, entityHd.OrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entityHd.ReferenceNo = POHd.PurchaseOrderNo;
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                OrderID = POHdDao.InsertReturnPrimaryKeyID(entityHd);

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

                        //string orderInformation = !string.IsNullOrEmpty(entityPRDt.OrderInformation) ? entityPRDt.OrderInformation + "|" : string.Empty;
                        //entityPRDt.OrderInformation = string.Format("{0}{1}", orderInformation, entityDt.PurchaseOrderID);
                        entityPRDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPRDTDao.Update(entityPRDt);
                    }
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                List<PurchaseRequestPO> lstPRPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0}", POHd.PurchaseOrderID), ctx);
                if (lstPRPO != null)
                {
                    foreach (PurchaseRequestPO prpo in lstPRPO)
                    {
                        prpo.PurchaseOrderID = OrderID;
                        PRPODao.Update(prpo);
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
}