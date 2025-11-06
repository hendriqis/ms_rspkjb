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
    public partial class PurchaseReturn : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string filterExpressionSupplier = "";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string GetPurchaseReturnCreditNote()
        {
            return Constant.PurchaseReturnType.CREDIT_NOTE;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_RETURN;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            string filterSetVarDt = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}', '{7}')",
                                                            AppSession.UserLogin.HealthcareID, //0
                                                            Constant.SettingParameter.VAT_PERCENTAGE, //1
                                                            Constant.SettingParameter.IM_PURCHASE_RETURN_TAX_INFORMATION_MANDATORY, //2
                                                            Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED, //3
                                                            Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA, //4
                                                            Constant.SettingParameter.IM0128, //5
                                                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //6
                                                            Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE //7
                                                        );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVarDt);

            hdnVATPercentage.Value = hdnVATPercentageFromSetvar.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).FirstOrDefault().ParameterValue;
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;
            hdnIsTaxInvoiceMandatory.Value = lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RETURN_TAX_INFORMATION_MANDATORY).FirstOrDefault().ParameterValue;
            hdnIsPpnAllowChanged.Value = lstSetVarDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            hdnIsCalculateHNA.Value = lstSetVarDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;
            hdnIM0128.Value = lstSetVarDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0128).FirstOrDefault().ParameterValue;
            filterExpressionSupplier = string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM PurchaseReceiveHd)");

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            string isDiscountToAveragePrice = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isDiscountToAveragePrice != "" && isDiscountToAveragePrice != null)
            {
                hdnIsDiscountAppliedToAveragePrice.Value = isDiscountToAveragePrice;
            }
            string isPPNToAveragePrice = lstSetVarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isPPNToAveragePrice != "" && isPPNToAveragePrice != null)
            {
                hdnIsPPNAppliedToAveragePrice.Value = isPPNToAveragePrice;
            }

            SetControlProperties();
            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);

            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboReason, new ControlEntrySetting(true, true, true), "mpTrxPopup");

        }

        protected string GetTransactionStatusVoid()
        {
            return Constant.TransactionStatus.VOID;
        }

        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.PURCHASE_RETURN_TYPE, Constant.StandardCode.PURCHASE_RETURN_REASON));
            Methods.SetComboBoxField<StandardCode>(cboReturnType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_RETURN_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReason, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_RETURN_REASON).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPRID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPurchaseReturnDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(hdnPurchaseReceiveID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnPurchaseReceiveDtID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblPurchaseReceiveNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtPurchaseReceiveNo, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(chkIsAutoUpdateStock, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtTransactionAmount, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(chkIsReturnCostInPct, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtReturnCostPct, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtReturnCostAmount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtGrandTotal, new ControlEntrySetting(false, false, true, "0"));

        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            chkPPN.Checked = false;
            txtTransactionAmount.Text = "0";
            txtPPN.Text = "0";
            txtGrandTotal.Text = "0";
            chkIsAutoUpdateStock.Enabled = true;
            hdnIsEditable.Value = "1";
            txtVATPercentageDefault.ReadOnly = true;
            hdnVATPercentage.Value = txtVATPercentageDefault.Text;
            chkIsReturnCostInPct.Enabled = true;
            //txtReturnCostPct.Enabled = true;
            //txtReturnCostAmount.Enabled = false;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return string.Format("TransactionCode = '{0}'", Constant.TransactionCode.PURCHASE_RETURN);
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseReturnHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseReturnHd entity = BusinessLayer.GetvPurchaseReturnHd(filterExpression, PageIndex, "PurchaseReturnID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseReturnHdRowIndex(filterExpression, keyValue, "PurchaseReturnID DESC");
            vPurchaseReturnHd entity = BusinessLayer.GetvPurchaseReturnHd(filterExpression, PageIndex, "PurchaseReturnID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseReturnHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(true, false, false));

                chkIsAutoUpdateStock.Enabled = false;
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

                if (entity.PurchaseReturnType == Constant.PurchaseReturnType.REPLACEMENT)
                    chkIsAutoUpdateStock.Enabled = false;
                else
                    chkIsAutoUpdateStock.Enabled = true;

                txtNotes.Enabled = true;
                chkPPN.Enabled = true;
                txtVATPercentageDefault.Enabled = true;
                chkIsReturnCostInPct.Enabled = true;
            }

            chkIsAutoUpdateStock.Checked = entity.IsAutoUpdateStock;

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnPRID.Value = entity.PurchaseReturnID.ToString();
            txtReturnNo.Text = entity.PurchaseReturnNo;
            hdnPurchaseReceiveID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferenceNo.Text = entity.ReferenceNo;
            if (entity.ReferenceDate != null && entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                txtReferenceDate.Text = "";
            }
            txtTaxInvoiceNo.Text = entity.TaxInvoiceNo;
            if (entity.TaxInvoiceDate != null && entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtTaxInvoiceDate.Text = entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                txtTaxInvoiceDate.Text = "";
            }
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            cboReturnType.Value = entity.GCPurchaseReturnType.ToString();
            txtNotes.Text = entity.Remarks;

            txtTransactionAmount.Text = entity.TransactionAmount.ToString();

            chkPPN.Checked = entity.IsIncludeVAT;
            if (entity.IsIncludeVAT)
            {
                hdnVATPercentage.Value = entity.VATPercentage.ToString();
                txtVATPercentageDefault.Text = entity.VATPercentage.ToString();
                txtPPN.Text = entity.VATAmount.ToString();
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

            if (entity.LastUpdatedByName != null && entity.LastUpdatedByName != "")
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPRID.Value != "0")
                filterExpression = string.Format("PurchaseReturnID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReturnDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            if (transactionAmount > -1)
                transactionAmount = BusinessLayer.GetPurchaseReturnHd(Convert.ToInt32(hdnPRID.Value)).TransactionAmount;

            List<vPurchaseReturnDt> lstEntity = BusinessLayer.GetvPurchaseReturnDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntity(PurchaseReturnHd entityHd)
        {
            entityHd.ReturnDate = Helper.GetDatePickerValue(txtPurchaseReturnDate.Text);
            entityHd.PurchaseReceiveID = Convert.ToInt32(hdnPurchaseReceiveID.Value);
            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityHd.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entityHd.GCPurchaseReturnType = cboReturnType.Value.ToString();
            entityHd.ReferenceNo = Request.Form[txtReferenceNo.UniqueID];
            entityHd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
            entityHd.TaxInvoiceNo = txtTaxInvoiceNo.Text;
            entityHd.TaxInvoiceDate = Helper.GetDatePickerValue(txtTaxInvoiceDate.Text);
            entityHd.IsIncludeVAT = chkPPN.Checked;
            entityHd.Remarks = txtNotes.Text;

            if (entityHd.IsIncludeVAT)
            {
                entityHd.VATPercentage = Convert.ToDecimal(txtVATPercentageDefault.Text);
            }
            else
            {
                entityHd.VATPercentage = 0;
            }

            entityHd.IsReturnCostInPercentage = chkIsReturnCostInPct.Checked;
            entityHd.ReturnCostPercentage = Convert.ToDecimal(txtReturnCostPct.Text);
            entityHd.ReturnCostAmount = Convert.ToDecimal(txtReturnCostAmount.Text);

            entityHd.IsAutoUpdateStock = chkIsAutoUpdateStock.Checked;
        }

        public void SavePurchaseReturnHd(IDbContext ctx, ref int PRID, ref string PRNo)
        {
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            if (hdnPRID.Value == "0")
            {
                PurchaseReturnHd entityHd = new PurchaseReturnHd();
                ControlToEntity(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.PURCHASE_RETURN;
                entityHd.PurchaseReturnNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ReturnDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                PRID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                PRNo = entityHd.PurchaseReturnNo;
            }
            else
            {
                PRID = Convert.ToInt32(hdnPRID.Value);
                PRNo = txtReturnNo.Text;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PRID = 0;
                string purchaseReturnNo = "";
                SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);
                retval = PRID.ToString();
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
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);

            try
            {
                PurchaseReturnHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur " + entity.PurchaseReturnNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseReturnHdDao purchaseReturnHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao purchaseReturnDtDao = new PurchaseReturnDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                PurchaseReturnHd purchaseReturnHd = purchaseReturnHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (purchaseReturnHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    int count = 0;
                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        decimal returnQty = purchaseDt.Quantity * purchaseDt.ConversionFactor;
                        string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", purchaseReturnHd.LocationID, purchaseDt.ItemID);
                        string filterExpressionItemProduct = String.Format("ItemID = '{0}'", purchaseDt.ItemID);
                        ItemProduct lstItemProduct = BusinessLayer.GetItemProductList(filterExpressionItemProduct, ctx).FirstOrDefault();

                        if (hdnIsCalculateHNA.Value == "1")
                        {
                            #region new
                            string filterIPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, purchaseDt.ItemID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterIPlanning, ctx);

                            ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                            string filterHistory = string.Format("ItemID = {0} ORDER BY HistoryID DESC", purchaseDt.ItemID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            ItemPriceHistory itemPriceHistory = BusinessLayer.GetItemPriceHistoryList(filterHistory, ctx).FirstOrDefault();

                            string filterBalance = string.Format("ItemID = {0} AND IsDeleted = 0", purchaseDt.ItemID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<ItemBalance> ib = BusinessLayer.GetItemBalanceList(filterBalance, ctx);
                            decimal qtyNow = ib.Sum(p => p.QuantityEND);

                            decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                            decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                            decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                            bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                            bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                            decimal qtyReturn = purchaseDt.Quantity * -1; //purchaseDt.Quantity * purchaseDt.ConversionFactor * -1;
                            decimal amountReturn = qtyReturn * purchaseDt.UnitPrice;
                            decimal tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity) * -1;
                            decimal discountAmount1 = 0;
                            decimal discountAmount2 = 0;

                            if (purchaseDt.DiscountAmount1 > 0)
                            {
                                discountAmount1 = purchaseDt.DiscountAmount1;
                            }
                            else
                            {
                                discountAmount1 = ((purchaseDt.Quantity * purchaseDt.UnitPrice * -1) * purchaseDt.DiscountPercentage1 / 100);
                            }

                            if (purchaseDt.DiscountAmount2 > 0)
                            {
                                discountAmount2 = purchaseDt.DiscountAmount2;
                            }
                            else
                            {
                                discountAmount2 = ((purchaseDt.Quantity * purchaseDt.UnitPrice * -1) * purchaseDt.DiscountPercentage2 / 100);
                            }

                            if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                            {
                                tempAmount = tempAmount - (discountAmount1 + discountAmount2);
                            }


                            if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                            {
                                decimal ppnAmount = tempAmount * (Convert.ToDecimal(purchaseReturnHd.VATPercentage) / Convert.ToDecimal(100));
                                tempAmount = tempAmount + ppnAmount;
                            }
                            else
                            {
                                if (hdnIsDiscountAppliedToAveragePrice.Value != "1")
                                {
                                    tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity) * -1;
                                }
                            }
                            amountReturn = tempAmount;
                            //if (purchaseReturnHd.IsIncludeVAT)
                            //{
                            //    decimal ppnAmount = ((purchaseReturnHd.VATPercentage / 100) * tempAmount);
                            //    tempAmount = tempAmount + ppnAmount;
                            //}
                            decimal TotalQTYReturn = qtyNow + (qtyReturn * purchaseDt.ConversionFactor);
                            if (TotalQTYReturn > 0)
                            {
                                decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;
                                decimal avg = (amountReturn + amountNow) / TotalQTYReturn;
                                entityItemPlanning.AveragePrice = Math.Round(avg, 2);
                            }


                            entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                            entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                            itemPlanningDao.Update(entityItemPlanning);
                            BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("POR RETURN APPROVED", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            #endregion
                        }

                        if (lstItemProduct.IsInventoryItem == true)
                        {
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
                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseReturnDtDao.Update(purchaseDt);
                            }
                        }
                        else
                        {
                            purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            purchaseReturnDtDao.Update(purchaseDt);
                        }
                    }

                    if (count == 0)
                    {
                        ControlToEntity(purchaseReturnHd);
                        purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseReturnHd.ApprovedBy = AppSession.UserLogin.UserID;
                        purchaseReturnHd.ApprovedDate = DateTime.Now;
                        purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnHd.LastUpdatedDate = DateTime.Now;
                        purchaseReturnHdDao.Update(purchaseReturnHd);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Retur dgn nomor <b>" + purchaseReturnHd.PurchaseReturnNo + "</b> tidak dapat di-approve karena stok tidak mencukupi di lokasi ini.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Retur dgn nomor <b>" + purchaseReturnHd.PurchaseReturnNo + "</b> tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseReturnHdDao purchaseReturnHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao purchaseReturnDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                PurchaseReturnHd purchaseReturnHd = purchaseReturnHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (purchaseReturnHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
                    }

                    ControlToEntity(purchaseReturnHd);
                    purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseReturnHdDao.Update(purchaseReturnHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur " + purchaseReturnHd.PurchaseReturnNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReturnHdDao purchaseReturnHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao purchaseReturnDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                PurchaseReturnHd purchaseReturnHd = purchaseReturnHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (purchaseReturnHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
                    }

                    ControlToEntity(purchaseReturnHd);
                    purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseReturnHdDao.Update(purchaseReturnHd);

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Retur " + purchaseReturnHd.PurchaseReturnNo + " tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Callback Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = 0;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            decimal transactionAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    transactionAmount = -1;
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref transactionAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref transactionAmount);
                    result = string.Format("refresh|{0}|{1}", pageCount, transactionAmount);
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
            panel.JSProperties["cpOrderID"] = PRID.ToString();
        }

        private void ControlToEntity(PurchaseReturnDt entityDt)
        {
            entityDt.PurchaseReceiveDtID = Convert.ToInt32(hdnPurchaseReceiveDtID.Value);
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

        private bool IsHaveStockReturn()
        {
            bool valid = true;
            ItemProduct itemProduct = BusinessLayer.GetItemProduct(Convert.ToInt32(hdnItemID.Value));

            if (itemProduct.IsInventoryItem == true)
            {
                decimal returnQty = Convert.ToDecimal(hdnConversionFactor.Value) * Convert.ToDecimal(txtQuantity.Text);
                string filterExpression = String.Format("HealthcareID = '{0}' AND LocationID = {1} AND ItemID = {2} AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, hdnLocationID.Value, hdnItemID.Value);
                vItemBalance lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression).FirstOrDefault();

                decimal currentStock = lstItemBalance.QuantityEND;

                if (currentStock < returnQty)
                {
                    valid = false;
                }
            }
            return valid;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int PRID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                string purchaseReturnNo = "";
                SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);
                if (entityHdDao.Get(PRID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (chkIsAutoUpdateStock.Checked)
                    {
                        if (IsHaveStockReturn())
                        {
                            PurchaseReturnDt entityDt = new PurchaseReturnDt();
                            ControlToEntity(entityDt);
                            entityDt.PurchaseReturnID = PRID;
                            entityDt.CreatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Insert(entityDt);
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = string.Format("Quantity item {0} tidak mencapai {1} {2}", Request.Form[txtItemName.UniqueID], Convert.ToDecimal(txtQuantity.Text), cboItemUnit.Text);
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        PurchaseReturnDt entityDt = new PurchaseReturnDt();
                        ControlToEntity(entityDt);
                        entityDt.PurchaseReturnID = PRID;
                        entityDt.CreatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Insert(entityDt);
                        ctx.CommitTransaction();
                    }
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
            PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                PurchaseReturnDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    if (IsHaveStockReturn())
                    {
                        ControlToEntity(entityDt);
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDtDao.Update(entityDt);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = string.Format("Quantity item {0} tidak mencapai {1} {2}", Request.Form[txtItemName.UniqueID], Convert.ToDecimal(txtQuantity.Text), cboItemUnit.Text);
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
                    }
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
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                PurchaseReturnDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseReturnID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
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