using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class DirectPurchaseReturn : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string filterExpressionSupplier = "";
        protected string filterExpressionItemProduct = "";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE_RETURN;
        }

        protected override void InitializeDataControl()
        {
            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            hdnVATPercentageFromSetvar.Value = hdnVATPercentage.Value;
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED,
                                                                                    Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA,
                                                                                    Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE
                                                                                ));
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            hdnIsCalculateHNA.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;
            hdnIsPPNAppliedToAveragePrice.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).FirstOrDefault().ParameterValue;

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionSupplier = string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM DirectPurchaseHd)");

            SetControlProperties();

            decimal tempTransactionAmount = -1, tempReturnCostAmount = 0;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempReturnCostAmount);

            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboReason, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        protected string GetTransactionStatusVoid()
        {
            return Constant.TransactionStatus.VOID;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PURCHASE_RETURN_TYPE, Constant.StandardCode.PURCHASE_RETURN_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReturnType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_RETURN_TYPE && p.StandardCodeID != Constant.PurchaseReturnType.CREDIT_NOTE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReason, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_RETURN_REASON).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPRID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDirectPurchaseReturnNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseReturnDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(false, false, false, ""));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(lblDirectPurchaseNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(hdnDirectPurchaseID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDirectPurchaseNo, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(txtTotalRetur, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(chkIsReturnCostInPct, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtReturnCostPct, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtReturnCostAmount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtGrandTotal, new ControlEntrySetting(false, false, false, "0"));
        }

        #region Load Entity

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnGCTransactionStatus.Value = "";
            chkPPN.Checked = false;
            chkPPN.Enabled = true;
            txtVATPercentageDefault.ReadOnly = true;
            txtNotes.ReadOnly = false;
            hdnVATPercentage.Value = txtVATPercentageDefault.Text;
            chkIsReturnCostInPct.Enabled = true;
            txtReturnCostPct.Enabled = true;
            txtReturnCostAmount.Enabled = false;

            divCreatedBy.InnerHtml = string.Empty;
            divCreatedDate.InnerHtml = string.Empty;
            divApprovedBy.InnerHtml = string.Empty;
            divApprovedDate.InnerHtml = string.Empty;
            trApprovedBy.Style.Add("display", "none");
            trApprovedDate.Style.Add("display", "none");
            divVoidBy.InnerHtml = string.Empty;
            divVoidDate.InnerHtml = string.Empty;
            trVoidBy.Style.Add("display", "none");
            trVoidDate.Style.Add("display", "none");
            divLastUpdatedBy.InnerHtml = string.Empty;
            divLastUpdatedDate.InnerHtml = string.Empty;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override int OnGetRowCount()
        {
            return BusinessLayer.GetvDirectPurchaseReturnHdRowCount("");
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            vDirectPurchaseReturnHd entity = BusinessLayer.GetvDirectPurchaseReturnHd("", PageIndex, "DirectPurchaseReturnID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = "";
            if (keyValue != "" && keyValue != null)
            {
                filterExpression += string.Format("DirectPurchaseReturnNo = '{0}'", keyValue);
            }
            PageIndex = BusinessLayer.GetvDirectPurchaseReturnHdRowIndex(filterExpression, keyValue, "DirectPurchaseReturnID DESC");
            vDirectPurchaseReturnHd entity = BusinessLayer.GetvDirectPurchaseReturnHd(filterExpression, PageIndex, "DirectPurchaseReturnID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vDirectPurchaseReturnHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";

                txtNotes.Enabled = false;
                chkPPN.Enabled = false;
                txtVATPercentageDefault.Enabled = false;
                chkIsReturnCostInPct.Enabled = false;
                txtReturnCostPct.Enabled = false;
                txtReturnCostAmount.Enabled = false;
            }
            else
            {
                hdnIsEditable.Value = "1";

                txtNotes.Enabled = true;
                chkPPN.Enabled = true;
                txtVATPercentageDefault.Enabled = true;
                chkIsReturnCostInPct.Enabled = true;
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnPRID.Value = entity.DirectPurchaseReturnID.ToString();
            txtDirectPurchaseReturnNo.Text = entity.DirectPurchaseReturnNo;
            hdnDirectPurchaseID.Value = entity.DirectPurchaseID.ToString();
            txtDirectPurchaseNo.Text = entity.DirectPurchaseNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            else
                txtReferenceDate.Text = "";
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            cboReturnType.Value = entity.GCDirectPurchaseReturnType.ToString();
            txtNotes.Text = entity.Remarks;
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            if (entity.IsIncludeVAT)
            {
                hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");
                txtVATPercentageDefault.Text = entity.VATPercentage.ToString();

                if (hdnIsPpnAllowChanged.Value == "1")
                {
                    SetControlEntrySetting(txtVATPercentageDefault, new ControlEntrySetting(true, true, false));
                    txtVATPercentageDefault.Attributes.Remove("readonly");
                }
                else
                {
                    SetControlEntrySetting(txtVATPercentageDefault, new ControlEntrySetting(false, false, false));
                    txtVATPercentageDefault.Attributes.Add("readonly", "readonly");
                }
            }
            else
            {
                hdnVATPercentage.Value = hdnVATPercentageFromSetvar.Value;
                txtVATPercentageDefault.Text = Convert.ToDecimal(hdnVATPercentageFromSetvar.Value).ToString("N2");
            }

            chkIsReturnCostInPct.Checked = entity.IsReturnCostInPercentage;
            txtReturnCostPct.Text = entity.ReturnCostPercentage.ToString();
            txtReturnCostAmount.Text = entity.ReturnCostAmount.ToString(Constant.FormatString.NUMERIC_2);

            txtTotalRetur.Text = entity.TransactionAmount.ToString();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedByName != null && entity.ApprovedByName != "")
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            else
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");
            }

            if (entity.VoidByName != null && entity.VoidByName != "")
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");

                divVoidBy.InnerHtml = entity.VoidByName;
                divVoidDate.InnerHtml = entity.cfVoidDateInFullString;
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
            }

            if (entity.LastUpdatedByName != null && entity.LastUpdatedByName != "")
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            decimal tempTransactionAmount = -1, tempReturnCostAmount = 0;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempReturnCostAmount);

            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount, ref decimal returnCostAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPRID.Value != "0")
            {
                filterExpression = string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvDirectPurchaseReturnDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (transactionAmount > -1)
            {
                DirectPurchaseReturnHd dphd = BusinessLayer.GetDirectPurchaseReturnHd(Convert.ToInt32(hdnPRID.Value));
                transactionAmount = dphd.TransactionAmount;
                returnCostAmount = dphd.ReturnCostAmount;
            }

            List<vDirectPurchaseReturnDt> lstEntity = BusinessLayer.GetvDirectPurchaseReturnDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntity(DirectPurchaseReturnHd entityHd)
        {
            entityHd.ReturnDate = Helper.GetDatePickerValue(txtPurchaseReturnDate.Text);
            entityHd.DirectPurchaseID = Convert.ToInt32(hdnDirectPurchaseID.Value);
            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityHd.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entityHd.GCDirectPurchaseReturnType = cboReturnType.Value.ToString();
            entityHd.ReferenceNo = Request.Form[txtReferenceNo.UniqueID];
            if (Request.Form[txtReferenceDate.UniqueID] != "")
            {
                entityHd.ReferenceDate = Helper.GetDatePickerValue(Request.Form[txtReferenceDate.UniqueID]);
            }
            else
            {
                entityHd.ReferenceDate = Helper.InitializeDateTimeNull();
            }

            entityHd.FinalDiscount = Convert.ToDecimal(txtFinalDiscountInPercentage.Text);
            entityHd.IsIncludeVAT = chkPPN.Checked;

            if (entityHd.IsIncludeVAT)
            {
                entityHd.VATPercentage = Convert.ToDecimal(Request.Form[txtVATPercentageDefault.UniqueID]);
            }
            else
            {
                entityHd.VATPercentage = 0;
            }

            entityHd.IsReturnCostInPercentage = chkIsReturnCostInPct.Checked;
            entityHd.ReturnCostPercentage = Convert.ToDecimal(txtReturnCostPct.Text);
            entityHd.ReturnCostAmount = Convert.ToDecimal(txtReturnCostAmount.Text);

            entityHd.Remarks = txtNotes.Text;
        }

        public void SavePurchaseReturnHd(IDbContext ctx, ref int PRID, ref string PRNo)
        {
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            if (hdnPRID.Value == "0")
            {
                DirectPurchaseReturnHd entityHd = new DirectPurchaseReturnHd();
                ControlToEntity(entityHd);
                entityHd.DirectPurchaseReturnNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.DIRECT_PURCHASE_RETURN, entityHd.ReturnDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                PRID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                PRNo = entityHd.DirectPurchaseReturnNo;
            }
            else
            {
                PRID = Convert.ToInt32(hdnPRID.Value);
                PRNo = txtDirectPurchaseReturnNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            try
            {
                int PRID = 0;
                string purchaseReturnNo = "";
                SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);

                DirectPurchaseHd entity = entityHdDao.Get(Convert.ToInt32(hdnDirectPurchaseID.Value));
                entity.IsHasPurchaseReturn = true;
                entity.DirectPurchaseReturnID = Convert.ToInt32(hdnPRID.Value);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Update(entity);

                retval = entity.DirectPurchaseNo;
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            try
            {
                DirectPurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    retval = entity.DirectPurchaseReturnNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat diubah karena sudah diproses.";
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            DirectPurchaseHdDao purchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao purchaseDtDao = new DirectPurchaseDtDao(ctx);

            try
            {
                DirectPurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;

                    List<DirectPurchaseReturnDt> lstEntity = BusinessLayer.GetDirectPurchaseReturnDtList(string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus = '{1}'", hdnPRID.Value, Constant.TransactionStatus.OPEN), ctx);
                    foreach (DirectPurchaseReturnDt entityDt in lstEntity)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat diubah karena sudah diproses.";
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

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                DirectPurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    int count = 0;
                    List<DirectPurchaseReturnDt> lstEntity = BusinessLayer.GetDirectPurchaseReturnDtList(string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus = '{1}'", hdnPRID.Value, Constant.TransactionStatus.OPEN), ctx);
                    foreach (DirectPurchaseReturnDt entityDt in lstEntity)
                    {
                        decimal returnQty = entityDt.Quantity * entityDt.ConversionFactor;
                        string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", entity.LocationID, entityDt.ItemID);
                        string filterExpressionItemProduct = String.Format("ItemID = '{0}'", entityDt.ItemID);

                        ItemBalance lstItemBalance = BusinessLayer.GetItemBalanceList(filterExpression, ctx).FirstOrDefault();
                        ItemProduct lstItemProduct = BusinessLayer.GetItemProductList(filterExpressionItemProduct, ctx).FirstOrDefault();

                        decimal currentStock = lstItemBalance.QuantityEND;
                        if (currentStock < returnQty)
                        {
                            if (lstItemProduct.IsInventoryItem == true)
                            {
                                count = count + 1;
                                result = false;
                                break;
                            }
                            else
                            {
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityDtDao.Update(entityDt);
                            }
                        }
                        else
                        {
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);
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

                            decimal qtyReturn = entityDt.Quantity * entityDt.ConversionFactor * -1;
                            decimal amountReturn = qtyReturn * entityDt.UnitPrice;

                            if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                            {
                                if (entity.IsIncludeVAT)
                                {
                                    decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                    amountReturn = amountReturn + ppnAmount;
                                }
                            }

                            if (qtyNow + qtyReturn != 0)
                            {
                                decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;
                                decimal avg = (amountReturn + amountNow) / (qtyReturn + qtyNow);
                                entityItemPlanning.AveragePrice = Math.Round(avg, 2);
                            }
                            else
                            {
                                entityItemPlanning.AveragePrice = 0;
                            }

                            entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                            entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemPlanningDao.Update(entityItemPlanning);
                            BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DIRECTPURCHASE RETURN APPROVED", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            #endregion
                        }
                    }

                    if (count == 0)
                    {
                        ControlToEntity(entity);
                        entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        entity.ApprovedBy = AppSession.UserLogin.UserID;
                        entity.ApprovedDate = DateTime.Now;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat di-approve karena stok tidak mencukupi di lokasi ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat diubah karena sudah diproses.";
                    Exception ex = new Exception(errMessage);
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            DirectPurchaseHdDao purchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao purchaseDtDao = new DirectPurchaseDtDao(ctx);

            try
            {
                DirectPurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.VoidBy = AppSession.UserLogin.UserID;
                    entity.VoidDate = DateTime.Now;

                    List<DirectPurchaseReturnDt> lstEntity = BusinessLayer.GetDirectPurchaseReturnDtList(string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID), ctx);
                    foreach (DirectPurchaseReturnDt entityDt in lstEntity)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    List<DirectPurchaseDt> entityDtList = BusinessLayer.GetDirectPurchaseDtList(string.Format("ReturnedInformation LIKE '%|{0}%'", hdnPRID.Value), ctx);
                    foreach (DirectPurchaseDt prDt in entityDtList)
                    {
                        string returnedInformation = prDt.ReturnedInformation.Replace(hdnPRID.Value, "");
                        if (returnedInformation != "|" && returnedInformation != string.Empty)
                            prDt.ReturnedInformation = returnedInformation;
                        else
                            prDt.ReturnedInformation = null;

                        DirectPurchaseDt tempReceiveDt = BusinessLayer.GetDirectPurchaseDtList(string.Format("DirectPurchaseID = {0} AND ItemID = {1}", hdnPRID.Value, prDt.ItemID), ctx).FirstOrDefault();
                        if (tempReceiveDt != null)
                        {
                            if ((prDt.ReturnedQuantity - tempReceiveDt.Quantity) >= 0)
                            {
                                prDt.ReturnedQuantity -= tempReceiveDt.Quantity;
                                purchaseDtDao.Update(prDt);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat diubah karena sudah diproses.";
                    Exception ex = new Exception(errMessage);
                    result = false;
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            if (type == "reopen")
            {
                try
                {
                    DirectPurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                    {
                        ControlToEntity(entity);
                        List<DirectPurchaseReturnDt> lstEntity = BusinessLayer.GetDirectPurchaseReturnDtList(string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID), ctx);
                        foreach (DirectPurchaseReturnDt entityDt in lstEntity)
                        {
                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(entityDt);

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

                                decimal qtyReturn = entityDt.Quantity * entityDt.ConversionFactor;
                                decimal amountReturn = qtyReturn * entityDt.UnitPrice;
                                decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;

                                if (entity.IsIncludeVAT)
                                {
                                    decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                    amountReturn = amountReturn + ppnAmount;
                                }

                                decimal avg = (amountReturn + amountNow) / (qtyReturn + qtyNow);

                                entityItemPlanning.AveragePrice = Math.Round(avg, 2);

                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DIRECTPURCHASE RETURN VOID", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                #endregion
                            }
                        }
                        entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entity.ApprovedBy = null;
                        entity.ApprovedDate = Helper.GetDatePickerValue("01-01-1900");
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityHdDao.Update(entity);
                        retval = entity.DirectPurchaseReturnNo;
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                        errMessage = "Retur pembelian tunai dgn nomor <b>" + entity.DirectPurchaseReturnNo + "</b> tidak dapat diubah karena sudah diproses.";
                        Exception ex = new Exception(errMessage);
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }
        #endregion

        #region Callback
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = 0;

            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "edit")
                {
                    result = "edit";
                }
            }
            cboItemUnit.JSProperties["cpResult"] = result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            decimal transactionAmount = 0;
            decimal returnCostAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    transactionAmount = -1;
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref transactionAmount, ref returnCostAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref transactionAmount, ref returnCostAmount);
                    result = string.Format("refresh|{0}|{1}|{2}", pageCount, transactionAmount, returnCostAmount);
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Process Detail
        protected void cbpProcess_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            int PRID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    PRID = Convert.ToInt32(hdnPRID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref PRID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                PRID = Convert.ToInt32(hdnEntryID.Value);
                if (OnDeleteEntityDt(ref errMessage, PRID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPurchaseReturnID"] = PRID.ToString();
        }

        private void ControlToEntity(DirectPurchaseReturnDt entityDt)
        {
            entityDt.DirectPurchaseDtID = Convert.ToInt32(hdnDirectPurchaseDtID.Value);
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.UnitPrice = Convert.ToDecimal(Request.Form[txtPrice.UniqueID]);
            entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
            entityDt.IsDiscountInPercentage1 = chkIsDiscountInPercentage1.Checked;
            entityDt.DiscountPercentage1 = Convert.ToDecimal(Request.Form[txtDiscount.UniqueID]);
            entityDt.DiscountAmount1 = Convert.ToDecimal(Request.Form[txtDiscountAmount.UniqueID]);
            entityDt.IsDiscountInPercentage2 = chkIsDiscountInPercentage2.Checked;
            entityDt.DiscountPercentage2 = Convert.ToDecimal(Request.Form[txtDiscount2.UniqueID]);
            entityDt.DiscountAmount2 = Convert.ToDecimal(Request.Form[txtDiscountAmount2.UniqueID]);
            entityDt.GCPurchaseReturnReason = cboReason.Value.ToString();
            if (entityDt.GCPurchaseReturnReason == "X162^999")
            {
                entityDt.PurchaseReturnReason = txtReason.Text;
            }
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int PRID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao entityDtDao = new DirectPurchaseDtDao(ctx);
            DirectPurchaseReturnHdDao entityHdReturnDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtReturnDao = new DirectPurchaseReturnDtDao(ctx);
            try
            {
                string purchaseReturnNo = "";
                SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);
                DirectPurchaseReturnHd entityHd = entityHdReturnDao.Get(PRID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    DirectPurchaseReturnDt entityDt = new DirectPurchaseReturnDt();
                    ControlToEntity(entityDt);
                    entityDt.DirectPurchaseReturnID = PRID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtReturnDao.Insert(entityDt);

                    #region validate return qty
                    string filterPurchaseDt = string.Format("DirectPurchaseID = '{0}' AND ItemID = '{1}' AND GCItemDetailStatus = '{2}'", entityHd.DirectPurchaseID, entityDt.ItemID, Constant.TransactionStatus.APPROVED);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    DirectPurchaseDt purchaseDt = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDt, ctx).FirstOrDefault();
                    Decimal qtyPurchaseInBaseUnit = purchaseDt.Quantity * purchaseDt.ConversionFactor;

                    string filterPurchaseReturnHd = string.Format("DirectPurchaseID = '{0}' AND GCTransactionStatus != '{1}'", entityHd.DirectPurchaseID, Constant.TransactionStatus.VOID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<DirectPurchaseReturnHd> lstPurhcaseReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(filterPurchaseReturnHd, ctx);

                    Decimal qtyReturnInBaseUnit = 0;
                    foreach (DirectPurchaseReturnHd h in lstPurhcaseReturnHd)
                    {
                        string filterReturnDt = string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}' AND ItemID = '{2}'", h.DirectPurchaseReturnID, Constant.TransactionStatus.VOID, entityDt.ItemID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        DirectPurchaseReturnDt returnDt = BusinessLayer.GetDirectPurchaseReturnDtList(filterReturnDt, ctx).FirstOrDefault();
                        if (returnDt != null)
                        {
                            qtyReturnInBaseUnit += (returnDt.Quantity * returnDt.ConversionFactor);
                        }
                    }

                    if (qtyReturnInBaseUnit > qtyPurchaseInBaseUnit)
                    {
                        result = false;
                        errMessage = "Jumlah Retur Sudah Melebihi Jumah Pembelian.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        ctx.CommitTransaction();
                    }
                    #endregion

                }
                else
                {
                    result = false;
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnSaveEditRecordEntityDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdReturnDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            try
            {
                DirectPurchaseReturnDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                DirectPurchaseReturnHd entityHd = entityHdReturnDao.Get(entityDt.DirectPurchaseReturnID);
                if (entityHd.GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    #region validate return qty
                    string filterPurchaseDt = string.Format("DirectPurchaseID = '{0}' AND ItemID = '{1}' AND GCItemDetailStatus = '{2}'", entityHd.DirectPurchaseID, entityDt.ItemID, Constant.TransactionStatus.APPROVED);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    DirectPurchaseDt purchaseDt = BusinessLayer.GetDirectPurchaseDtList(filterPurchaseDt, ctx).FirstOrDefault();
                    Decimal qtyPurchaseInBaseUnit = purchaseDt.Quantity * purchaseDt.ConversionFactor;

                    string filterPurchaseReturnHd = string.Format("DirectPurchaseID = '{0}' AND GCTransactionStatus != '{1}' AND DirectPurchaseReturnID != '{2}'", entityHd.DirectPurchaseID, Constant.TransactionStatus.VOID, entityDt.DirectPurchaseReturnID);
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<DirectPurchaseReturnHd> lstPurhcaseReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(filterPurchaseReturnHd, ctx);

                    Decimal qtyReturnInBaseUnit = 0;
                    foreach (DirectPurchaseReturnHd h in lstPurhcaseReturnHd)
                    {
                        string filterReturnDt = string.Format("DirectPurchaseReturnID = {0} AND GCItemDetailStatus != '{1}' AND ItemID = '{2}'", h.DirectPurchaseReturnID, Constant.TransactionStatus.VOID, entityDt.ItemID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        DirectPurchaseReturnDt returnDt = BusinessLayer.GetDirectPurchaseReturnDtList(filterReturnDt, ctx).FirstOrDefault();
                        if (returnDt != null)
                        {
                            qtyReturnInBaseUnit += (returnDt.Quantity * returnDt.ConversionFactor);
                        }
                    }

                    decimal qtyFinal = entityDt.Quantity * entityDt.ConversionFactor;
                    qtyReturnInBaseUnit += qtyFinal;

                    if (qtyReturnInBaseUnit > qtyPurchaseInBaseUnit)
                    {
                        result = false;
                        errMessage = "Jumlah Retur Sudah Melebihi Jumah Pembelian.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                    else
                    {
                        ctx.CommitTransaction();
                    }
                    #endregion
                }
                else
                {
                    result = false;
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
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

        private bool OnDeleteEntityDt(ref string errMessage, int ID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseReturnHdDao entityHdDao = new DirectPurchaseReturnHdDao(ctx);
            DirectPurchaseReturnDtDao entityDtDao = new DirectPurchaseReturnDtDao(ctx);
            try
            {
                DirectPurchaseReturnDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.DirectPurchaseReturnID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
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
        #endregion
    }
}