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
    public partial class ConsignmentReturn : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string filterExpressionSupplier = "";
        protected string filterExpressionItemProduct = "";

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
            return Constant.MenuCode.Inventory.CONSIGNMENT_RETURN;
        }

        protected override void InitializeDataControl()
        {
            hdnIsAPConsignmentFromOrder.Value = AppSession.IsAPConsignmentFromOrder;

            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            hdnVATPercentageFromSetvar.Value = hdnVATPercentage.Value;
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED
                                                                                ));
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;


            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            filterExpressionSupplier = string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM PurchaseReceiveHd)");

            SetControlProperties();
            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboReason, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
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
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true, ""));
            SetControlEntrySetting(hdnPurchaseReceiveID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(hdnPurchaseReceiveDtID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(lblPurchaseReceiveNo, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtPurchaseReceiveNo, new ControlEntrySetting(true, false, true, ""));
            SetControlEntrySetting(chkIsAutoUpdateStock, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(cboReturnType, new ControlEntrySetting(true, false, true));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtTotalOrder, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtTotalOrderSaldo, new ControlEntrySetting(false, false, true, "0"));
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            chkPPN.Checked = false;
            txtTotalOrder.Text = "0";
            txtPPN.Text = "0";
            txtTotalOrderSaldo.Text = "0";
            chkIsAutoUpdateStock.Enabled = true;
            hdnIsEditable.Value = "1";
            txtVATPercentageDefault.ReadOnly = true;
            hdnVATPercentage.Value = txtVATPercentageDefault.Text;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            return string.Format("TransactionCode = '{0}'", Constant.TransactionCode.CONSIGNMENT_RETURN);
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
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsAutoUpdateStock, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, false, false));
                txtVATPercentageDefault.Enabled = false;
            }
            else
            {
                hdnIsEditable.Value = "1";
                txtVATPercentageDefault.Enabled = true;
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnPRID.Value = entity.PurchaseReturnID.ToString();
            txtReturnNo.Text = entity.PurchaseReturnNo;
            hdnPurchaseReceiveID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReturnDate.Text = entity.ReturnDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.SupplierName;
            txtReferenceNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            cboReturnType.Value = entity.GCPurchaseReturnType.ToString();
            txtNotes.Text = entity.Remarks;
            chkIsAutoUpdateStock.Checked = entity.IsAutoUpdateStock;
            chkPPN.Checked = entity.IsIncludeVAT;
            if (entity.IsIncludeVAT)
            {
                hdnVATPercentage.Value = entity.VATPercentage.ToString("0.##");
                txtVATPercentageDefault.Text = entity.VATPercentage.ToString();

                if (hdnIsPpnAllowChanged.Value == "1")
                {
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                    {
                        SetControlEntrySetting(txtVATPercentageDefault, new ControlEntrySetting(true, true, false));
                        txtVATPercentageDefault.Attributes.Remove("readonly");
                        txtVATPercentageDefault.Enabled = true;
                    }
                    else
                    {
                        SetControlEntrySetting(txtVATPercentageDefault, new ControlEntrySetting(false, false, false));
                        txtVATPercentageDefault.Attributes.Add("readonly", "readonly");
                        txtVATPercentageDefault.Enabled = false;
                    }

                }
                else
                {
                    SetControlEntrySetting(txtVATPercentageDefault, new ControlEntrySetting(false, false, false));
                    txtVATPercentageDefault.Attributes.Add("readonly", "readonly");
                    txtVATPercentageDefault.Enabled = false;
                }
            }
            else
            {
                hdnVATPercentage.Value = hdnVATPercentageFromSetvar.Value;
                txtVATPercentageDefault.Text = Convert.ToDecimal(hdnVATPercentageFromSetvar.Value).ToString("N2");
            }

            txtTotalOrder.Text = entity.TransactionAmount.ToString();
            if (entity.PurchaseReturnType == Constant.PurchaseReturnType.REPLACEMENT)
                chkIsAutoUpdateStock.Enabled = false;
            else
                chkIsAutoUpdateStock.Enabled = true;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_FORMAT);
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
            entityHd.IsIncludeVAT = chkPPN.Checked;
            entityHd.Remarks = txtNotes.Text;
            if (entityHd.IsIncludeVAT)
                entityHd.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
            else
                entityHd.VATPercentage = 0;
            entityHd.IsAutoUpdateStock = chkIsAutoUpdateStock.Checked;
        }

        public void SavePurchaseReturnHd(IDbContext ctx, ref int PRID, ref string PRNo)
        {
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            if (hdnPRID.Value == "0")
            {
                PurchaseReturnHd entityHd = new PurchaseReturnHd();
                ControlToEntity(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.CONSIGNMENT_RETURN;
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
            try
            {
                PurchaseReturnHd entity = BusinessLayer.GetPurchaseReturnHd(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseReturnHd(entity);
                }
                else
                {
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
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
                    int count = 0;
                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        decimal returnQty = purchaseDt.Quantity * purchaseDt.ConversionFactor;
                        string filterExpression = String.Format("LocationID = {0} AND ItemID = {1} AND IsDeleted = 0", purchaseReturnHd.LocationID, purchaseDt.ItemID);
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

                    ControlToEntity(purchaseReturnHd);
                    purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseReturnHdDao.Update(purchaseReturnHd);

                    if (count == 0)
                    {
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
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
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
                    ControlToEntity(purchaseReturnHd);
                    purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseReturnHdDao.Update(purchaseReturnHd);

                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
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
            PurchaseReceiveDtDao purchaseReceiveDtDao = new PurchaseReceiveDtDao(ctx);
            try
            {
                PurchaseReturnHd purchaseReturnHd = purchaseReturnHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (purchaseReturnHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(purchaseReturnHd);
                    purchaseReturnHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    purchaseReturnHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseReturnHdDao.Update(purchaseReturnHd);

                    string filterExpressionPurchaseReturnHd = String.Format("PurchaseReturnID IN ({0}) AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<PurchaseReturnDt> lstPurchaseReturnDt = BusinessLayer.GetPurchaseReturnDtList(filterExpressionPurchaseReturnHd);
                    foreach (PurchaseReturnDt purchaseDt in lstPurchaseReturnDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseReturnDtDao.Update(purchaseDt);
                    }

                    List<PurchaseReceiveDt> entityPRDtList = BusinessLayer.GetPurchaseReceiveDtList(string.Format("ReturnedInformation LIKE '%|{0}%'", hdnPRID.Value), ctx);
                    foreach (PurchaseReceiveDt prDt in entityPRDtList)
                    {
                        string returnedInformation = prDt.ReturnedInformation.Replace(hdnPRID.Value, "");
                        if (returnedInformation != "|" && returnedInformation != string.Empty)
                            prDt.ReturnedInformation = returnedInformation;
                        else
                            prDt.ReturnedInformation = null;

                        PurchaseReceiveDt tempReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(string.Format(
                                "PurchaseReceiveID = {0} AND ItemID = {1}", hdnPRID.Value, prDt.ItemID), ctx)[0];
                        if (tempReceiveDt != null)
                        {
                            if ((prDt.ReturnedQuantity - tempReceiveDt.Quantity) >= 0)
                            {
                                prDt.ReturnedQuantity -= tempReceiveDt.Quantity;
                                purchaseReceiveDtDao.Update(prDt);
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
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
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND IsActive = 1 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            cboItemUnit.SelectedIndex = -1;
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
            entityDt.DiscountPercentage1 = Convert.ToDecimal(Request.Form[txtDiscount.UniqueID]);
            entityDt.DiscountPercentage2 = Convert.ToDecimal(Request.Form[txtDiscount2.UniqueID]);
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
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                string purchaseReturnNo = "";
                SavePurchaseReturnHd(ctx, ref PRID, ref purchaseReturnNo);
                if (entityHdDao.Get(PRID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
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
            PurchaseReturnHdDao entityHdDao = new PurchaseReturnHdDao(ctx);
            PurchaseReturnDtDao entityDtDao = new PurchaseReturnDtDao(ctx);
            try
            {
                PurchaseReturnDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseReturnID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
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
                    errMessage = "Retur tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                    ctx.RollBackTransaction();
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
            return result;
        }
        #endregion
    }
}