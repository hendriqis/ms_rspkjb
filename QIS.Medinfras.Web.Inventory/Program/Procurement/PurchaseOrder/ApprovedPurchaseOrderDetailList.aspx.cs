using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ApprovedPurchaseOrderDetailList : BasePageTrx
    {
        protected int PageCount = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_ORDER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        //private GetUserMenuAccess menu;

        //protected String GetMenuCaption()
        //{
        //    if (menu != null)
        //        return GetLabel(menu.MenuCaption);
        //    return "";
        //}

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = false;
        }

        protected override void InitializeDataControl()
        {
            //MPTrx master = (MPTrx)Master;
            //menu = ((MPMain)master.Master).ListMenu.FirstOrDefault(p => p.MenuCode == OnGetMenuCode());

            string[] param = Page.Request.QueryString["id"].Split('|');

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.IM_ALLOW_PRINT_ORDER_RECEIPT_AFTER_PROPOSDED
                                                                                ));
            hdnIsAllowPrintOrderReceiptAfterProposed.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_ALLOW_PRINT_ORDER_RECEIPT_AFTER_PROPOSDED).FirstOrDefault().ParameterValue;

            hdnGCPurchasingType.Value = Constant.PurchasingType.RUTIN;

            hdnOrderID.Value = param[0];
            if (param.Length > 1)
                hdnParam.Value = param[1];
            else
                hdnParam.Value = "1";

            vPurchaseOrderHd entityItemRequest = BusinessLayer.GetvPurchaseOrderHdList(String.Format("PurchaseOrderID = '{0}'", Convert.ToInt32(hdnOrderID.Value)))[0];
            hdnVATPercentage.Value = entityItemRequest.VATPercentage.ToString("0.##");
            //EntityToControl(entityItemRequest);
            EntityToControl(entityItemRequest, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (hdnIsAllowPrintOrderReceiptAfterProposed.Value == "0")
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                {
                    if (entity.GCTransactionStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                    {
                        hdnPrintStatus.Value = "true";
                    }
                    else
                    {
                        hdnPrintStatus.Value = "false";
                    }
                }
                else
                {
                    hdnPrintStatus.Value = "false";
                }
            }
            else
            {
                if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                    hdnPrintStatus.Value = "true";
                else
                    hdnPrintStatus.Value = "false";
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnOrderID.Value = entity.PurchaseOrderID.ToString();
            hdnPurchasingType.Value = entity.GCPurchasingType.ToString();
            hdnGCPurchasingType.Value = entity.GCPurchasingType.ToString();
            hdnPurchaseOrderType.Value = entity.GCPurchaseOrderType.ToString();
            hdnGCPurchaseOrderType.Value = entity.GCPurchaseOrderType.ToString();
            txtOrderNo.Text = entity.PurchaseOrderNo;
            txtItemOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = Convert.ToString(entity.BusinessPartnerID);
            txtSupplierName.Text = entity.BusinessPartnerName;
            txtNotes.Text = entity.Remarks;
            txtExpiredDate.Text = entity.POExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseOrderType.Text = entity.PurchaseOrderType;
            txtTermCondition.Text = entity.TermName;
            txtFrancoRegion.Text = entity.FrancoRegion;
            txtCurrencyCode.Text = entity.CurrencyCode;
            txtCurrencyRate.Text = entity.CurrencyRate.ToString();
            txtDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTotalOrder.Text = entity.TransactionAmount.ToString("N");
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString("N");
            txtFinalDiscount.Text = (entity.TransactionAmount * entity.FinalDiscount / 100).ToString("N");
            txtPPN.Text = ((entity.VATPercentage / 100) * (entity.TransactionAmount - Convert.ToDecimal(txtFinalDiscount.Text))).ToString("N");
            txtDP.Text = entity.DownPaymentAmount.ToString("N");
            txtPaymentRemarks.Text = entity.PaymentRemarks;
            txtTotalOrderSaldo.Text = (entity.TransactionAmount - Convert.ToDecimal(txtFinalDiscount.Text) + Convert.ToDecimal(txtPPN.Text) - entity.DownPaymentAmount).ToString("N");
            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "")
                filterExpression = string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
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

        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int OrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "updatePrintNo")
            {
                if (OnUpdatePrintNumber(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = OrderID.ToString();
        }

        private bool OnUpdatePrintNumber(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao entityDao = new PurchaseOrderHdDao(ctx);
            try
            {
                PurchaseOrderHd entity = entityDao.Get(Convert.ToInt32(hdnOrderID.Value));
                entity.PrintNumber += 1;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);
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