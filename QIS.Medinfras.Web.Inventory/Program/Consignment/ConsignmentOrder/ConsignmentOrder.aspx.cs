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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ConsignmentOrder : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected int PageCount = 1;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_ORDER;
        }

        protected override void InitializeDataControl()
        {
            hdnIsAPConsignmentFromOrder.Value = AppSession.IsAPConsignmentFromOrder;

            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            hdnVATPercentageFromSetvar.Value = hdnVATPercentage.Value;
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.CONSIGNMENT_ORDER);
            List<GetLocationUserList> lstLocation = BusinessLayer.GetLocationUserList(AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.CONSIGNMENT_ORDER, "");
            if (lstLocation.Count == 1)
            {
                hdnLocation.Style.Add("display", "none");
                hdnLocationID.Value = lstLocation[0].LocationID.ToString();
                txtLocationCode.Text = lstLocation[0].LocationCode;
                Location loc = BusinessLayer.GetLocation(lstLocation[0].LocationID);
                hdnLocationItemGroupID.Value = loc.ItemGroupID.ToString();
            }

            hdnIsUsedProductLine.Value = AppSession.IsUsedProductLine;

            if (hdnIsUsedProductLine.Value == "1")
            {
                trProductLine.Style.Remove("display");
                lblProductLine.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                trProductLine.Style.Add("display", "none");
                lblProductLine.Attributes.Remove("class");
            }

            int count = BusinessLayer.GetLocationUserRowCount(string.Format("UserID = {0} AND IsDeleted = 0", AppSession.UserLogin.UserID));
            if (count > 0)
                hdnRecordFilterExpression.Value = string.Format("LocationID IN (SELECT LocationID FROM LocationUser WHERE UserID = {0} AND IsDeleted = 0)", AppSession.UserLogin.UserID);
            else
            {
                count = BusinessLayer.GetLocationUserRoleRowCount(string.Format("RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID));
                if (count > 0)
                    hdnRecordFilterExpression.Value = string.Format("LocationID IN (SELECT LocationID FROM LocationUserRole WHERE RoleID IN (SELECT RoleID FROM UserInRole WHERE UserID = {0} AND HealthcareID = '{1}') AND IsDeleted = 0)", AppSession.UserLogin.UserID, AppSession.UserLogin.HealthcareID);
                else
                    hdnRecordFilterExpression.Value = "";
            }

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsActive = 1 AND IsBlackList = 0 AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
            SetControlProperties();

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string GetVATPercentageLabel()
        {
            return hdnVATPercentage.Value;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.StandardCode.PURCHASE_ORDER_TYPE, Constant.StandardCode.FRANCO_REGION, Constant.StandardCode.CURRENCY_CODE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASE_ORDER_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrancoRegion, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.FRANCO_REGION).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            cboPurchaseOrderType.SelectedIndex = 0;
            cboFrancoRegion.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC,
                                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED
                                                                                ));

            SettingParameterDt setvarDTR = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC).FirstOrDefault();
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;

            string locationCode = string.Empty;
            string locationName = string.Empty;
            string gcPurchaseOrderType = string.Empty;
            string locationID = string.Empty;
            string locationItemGroupID = string.Empty;

            if (setvarDTR.ParameterValue != "" && setvarDTR.ParameterValue != "0" && setvarDTR.ParameterValue != null)
            {
                List<UserInRole> uir = BusinessLayer.GetUserInRoleList(String.Format(
                    "HealthcareID = {0} AND UserID = {1} AND RoleID = {2}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, setvarDTR.ParameterValue));

                if (uir.Count() > 0)
                {
                    SettingParameterDt setvarDTL = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTL.ParameterValue)).FirstOrDefault();
                    locationID = loc.LocationID.ToString();
                    locationItemGroupID = loc.ItemGroupID.ToString();
                    locationCode = loc.LocationCode;
                    locationName = loc.LocationName;
                    gcPurchaseOrderType = Constant.PurchaseOrderType.LOGISTIC;
                }
                else
                {
                    SettingParameterDt setvarDTP = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTP.ParameterValue)).FirstOrDefault();
                    locationID = loc.LocationID.ToString();
                    locationItemGroupID = loc.ItemGroupID.ToString();
                    locationCode = loc.LocationCode;
                    locationName = loc.LocationName;
                    gcPurchaseOrderType = Constant.PurchaseOrderType.DRUGMS;
                }
            }

            SetControlEntrySetting(hdnOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseReceiveNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderDeliveryDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderExpiredDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrancoRegion, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtTotalOrder, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtDP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtTotalOrderSaldo, new ControlEntrySetting(false, false, true, "0"));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true, locationCode));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true, locationName));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(true, false, true, locationID));
            SetControlEntrySetting(hdnLocationItemGroupID, new ControlEntrySetting(false, false, false, locationItemGroupID));

            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, false));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(lblProductLine, new ControlEntrySetting(true, true));
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(hdnProductLineItemType, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            }

            trApprovedBy.Attributes.Add("style", "display:none");
            trApprovedDate.Attributes.Add("style", "display:none");
            trClosedBy.Attributes.Add("style", "display:none");
            trClosedDate.Attributes.Add("style", "display:none");
            trClosedReason.Attributes.Add("style", "display:none");
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            txtVATPercentageDefault.ReadOnly = true;
            hdnVATPercentage.Value = txtVATPercentageDefault.Text;
        }

        public Int32 GetOrderID()
        {
            return Convert.ToInt32(hdnOrderID.Value);
        }

        public Int32 GetSupplierID()
        {

            return hdnSupplierID.Value != "" ? Convert.ToInt32(hdnSupplierID.Value) : 0;
        }

        public Int32 GetLocationID()
        {
            return hdnLocationID.Value != "" ? Convert.ToInt32(hdnLocationID.Value) : 0;
        }

        public Int32 GetProductLineID()
        {
            return hdnProductLineID.Value != "" ? Convert.ToInt32(hdnProductLineID.Value) : 0;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        protected string IsAPConsignmentFromOrder()
        {
            return hdnIsAPConsignmentFromOrder.Value;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = hdnRecordFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionCode = '{0}'", Constant.TransactionCode.CONSIGNMENT_ORDER);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseOrderHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHd(filterExpression, PageIndex, "PurchaseOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseOrderHdRowIndex(filterExpression, keyValue, "PurchaseOrderID DESC");
            vPurchaseOrderHd entity = BusinessLayer.GetvPurchaseOrderHd(filterExpression, PageIndex, "PurchaseOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseOrderHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";

                SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(cboFrancoRegion, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, false, false));

                SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtDP, new ControlEntrySetting(true, false, true));

                txtItemOrderDeliveryDate.Enabled = false;
                txtItemOrderExpiredDate.Enabled = false;
                cboPurchaseOrderType.Enabled = false;
                cboTerm.Enabled = false;
                cboFrancoRegion.Enabled = false;
                txtPaymentRemarks.Enabled = false;
                txtNotes.Enabled = false;
                txtVATPercentageDefault.Enabled = false;
            }
            else
            {
                hdnIsEditable.Value = "1";

                txtItemOrderDeliveryDate.Enabled = true;
                txtItemOrderExpiredDate.Enabled = true;
                cboPurchaseOrderType.Enabled = true;
                cboTerm.Enabled = true;
                cboFrancoRegion.Enabled = true;
                txtPaymentRemarks.Enabled = true;
                txtNotes.Enabled = true;
                txtVATPercentageDefault.Enabled = true;
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnOrderID.Value = entity.PurchaseOrderID.ToString();
            txtOrderNo.Text = entity.PurchaseOrderNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtItemOrderDate.Text = entity.OrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderDeliveryDate.Text = entity.DeliveryDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtItemOrderExpiredDate.Text = entity.POExpiredDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            hdnLocationItemGroupID.Value = entity.LocationItemGroupID.ToString();
            cboPurchaseOrderType.Value = entity.GCPurchaseOrderType;
            cboTerm.Value = entity.TermID.ToString();
            txtPaymentRemarks.Text = entity.PaymentRemarks;
            txtNotes.Text = entity.Remarks;
            cboFrancoRegion.Value = entity.GCFrancoRegion.ToString();
            cboCurrency.Value = entity.GCCurrencyCode.ToString();
            txtKurs.Text = entity.CurrencyRate.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            if (entity.IsIncludeVAT)
            {
                hdnVATPercentage.Value = entity.VATPercentage.ToString();
                txtVATPercentageDefault.Text = entity.VATPercentage.ToString();
                txtPPN.Text = ((entity.TransactionAmount - entity.FinalDiscountAmount) * entity.VATPercentage / 100).ToString();

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

            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            txtTotalOrder.Text = entity.TransactionAmount.ToString();

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Attributes.Add("style", "display:none");
                trApprovedDate.Attributes.Add("style", "display:none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Attributes.Remove("style");
                trApprovedDate.Attributes.Remove("style");

                divApprovedBy.InnerHtml = entity.ApprovedByName;
                divApprovedDate.InnerHtml = entity.ApprovedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedBy.InnerHtml = "";
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }

            if (entity.ClosedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trClosedBy.Attributes.Add("style", "display:none");
                trClosedDate.Attributes.Add("style", "display:none");
                trClosedReason.Attributes.Add("style", "display:none");

                divClosedBy.InnerHtml = "";
                divClosedDate.InnerHtml = "";
            }
            else
            {
                trClosedBy.Attributes.Remove("style");
                trClosedDate.Attributes.Remove("style");
                trClosedReason.Attributes.Remove("style");

                divClosedBy.InnerHtml = entity.ClosedByName;
                divClosedDate.InnerHtml = entity.ClosedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
            if (entity.OtherClosedReason != null && entity.OtherClosedReason != "")
            {
                divClosedReason.InnerHtml = entity.ClosedReason + " | " + entity.OtherClosedReason;
            }
            else
            {
                divClosedReason.InnerHtml = entity.ClosedReason;
            }

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnOrderID.Value != "" && hdnOrderID.Value != "0")
                filterExpression = string.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseOrderDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            if (transactionAmount > -1)
                transactionAmount = BusinessLayer.GetvPurchaseOrderHdList(string.Format("PurchaseOrderID = {0}", hdnOrderID.Value)).FirstOrDefault().TransactionAmount;


            if (hdnIsAPConsignmentFromOrder.Value == "1")
            {
                List<vPurchaseOrderDtConsignment> lstEntity = BusinessLayer.GetvPurchaseOrderDtConsignmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
            else
            {
                List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
                grdView.DataSource = lstEntity;
                grdView.DataBind();
            }
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntityHd(PurchaseOrderHd entity)
        {
            entity.DeliveryDate = Helper.GetDatePickerValue(txtItemOrderDeliveryDate.Text);
            entity.POExpiredDate = Helper.GetDatePickerValue(txtItemOrderExpiredDate.Text);
            entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entity.PaymentRemarks = txtPaymentRemarks.Text;
            entity.Remarks = txtNotes.Text;
            entity.IsIncludeVAT = chkPPN.Checked;
            entity.FinalDiscount = Convert.ToDecimal(Request.Form[txtFinalDiscountInPercentage.UniqueID]);
            entity.FinalDiscountAmount = Convert.ToDecimal(Request.Form[txtFinalDiscount.UniqueID]);
            if (entity.IsIncludeVAT)
            {
                entity.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
            }
            else
            {
                entity.VATPercentage = 0;
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            entity.OrderDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            entity.GCPurchaseOrderType = cboPurchaseOrderType.Value.ToString();
            entity.TermID = Convert.ToInt32(cboTerm.Value.ToString());
            entity.GCFrancoRegion = cboFrancoRegion.Value.ToString();
            entity.GCCurrencyCode = cboCurrency.Value.ToString();
            entity.CurrencyRate = Convert.ToDecimal(txtKurs.Text);
        }

        public void SavePurchaseOrderHd(IDbContext ctx, ref int OrderID)
        {
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            if (hdnOrderID.Value == "0")
            {
                PurchaseOrderHd entityHd = new PurchaseOrderHd();
                ControlToEntityHd(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.CONSIGNMENT_ORDER;
                entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.OrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

                entityHd.DownPaymentAmount = Convert.ToDecimal(txtDP.Text);
                entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                OrderID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
            }
            else
            {
                OrderID = Convert.ToInt32(hdnOrderID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;
                SavePurchaseOrderHd(ctx, ref OrderID);
                retval = OrderID.ToString();
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
                PurchaseOrderHd entity = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseOrderHd(entity);
                }
                else
                {
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            AuditLog entityAuditLog = new AuditLog();
            try
            {
                PurchaseOrderHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityAuditLog.OldValues = JsonConvert.SerializeObject(purchaseHd);
                    ControlToEntityHd(purchaseHd);
                    string filterExpressionRecOrd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<PurchaseReceivePO> recOrderList = BusinessLayer.GetPurchaseReceivePOList(filterExpressionRecOrd);
                    if (recOrderList.Count > 0)
                    {
                        decimal orderQty = recOrderList.Sum(a => a.OrderedQuantity);
                        decimal receiveQty = recOrderList.FirstOrDefault().ReceivedQuantity;
                        if (orderQty == receiveQty)
                        {
                            if (hdnIsAPConsignmentFromOrder.Value == "1")
                            {
                                purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                                purchaseHd.ApprovedDate = DateTime.Now;
                            }
                            else
                            {
                                purchaseHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                purchaseHd.ClosedBy = AppSession.UserLogin.UserID;
                                purchaseHd.ClosedDate = DateTime.Now;
                            }
                        }
                        else
                        {
                            purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                            purchaseHd.ApprovedDate = DateTime.Now;
                        }
                    }
                    else
                    {
                        purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                        purchaseHd.ApprovedDate = DateTime.Now;
                    }
                    purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseHdDao.Update(purchaseHd);

                    entityAuditLog.ObjectType = Constant.BusinessObjectType.PURCHASE_ORDER;
                    entityAuditLog.NewValues = JsonConvert.SerializeObject(purchaseHd);
                    entityAuditLog.UserID = AppSession.UserLogin.UserID;
                    entityAuditLog.LogDate = DateTime.Now;
                    entityAuditLog.TransactionID = purchaseHd.PurchaseOrderID;
                    entityAuditLogDao.Insert(entityAuditLog);

                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd);
                    foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                    {
                        purchaseDt.GCItemDetailStatus = purchaseHd.GCTransactionStatus;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        purchaseDtDao.Update(purchaseDt);
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", purchaseHd.PurchaseOrderNo);
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
            try
            {
                PurchaseOrderHd entity = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.ProposedBy = AppSession.UserLogin.UserID;
                    entity.ProposedDate = DateTime.Now;
                    BusinessLayer.UpdatePurchaseOrderHd(entity);

                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd);
                    foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePurchaseOrderDt(purchaseDt);
                    }
                }
                else
                {
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            try
            {
                PurchaseOrderHd entity = BusinessLayer.GetPurchaseOrderHd(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseOrderHd(entity);

                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd);
                    foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePurchaseOrderDt(purchaseDt);

                        string filterRcvDt = String.Format("PurchaseOrderID = {0} AND ItemID = {1} AND PurchaseOrderDtID = {2}", purchaseDt.PurchaseOrderID, purchaseDt.ItemID, purchaseDt.ID);
                        List<PurchaseReceiveDt> rcvDtList = BusinessLayer.GetPurchaseReceiveDtList(filterRcvDt);
                        foreach (PurchaseReceiveDt rcvDt in rcvDtList)
                        {
                            rcvDt.PurchaseOrderID = null;
                            rcvDt.PurchaseOrderDtID = null;
                            rcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePurchaseReceiveDt(rcvDt);

                            string filterRcvPO = String.Format("PurchaseOrderID = {0} AND ItemID = {1} AND PurchaseReceiveID = {2}", purchaseDt.PurchaseOrderID, purchaseDt.ItemID, rcvDt.PurchaseReceiveID);
                            List<PurchaseReceivePO> rcvPOList = BusinessLayer.GetPurchaseReceivePOList(filterRcvPO);
                            foreach (PurchaseReceivePO rcvPO in rcvPOList)
                            {
                                rcvPO.IsDeleted = true;
                                BusinessLayer.UpdatePurchaseReceivePO(rcvPO);
                            }
                        }

                        // ditutup oleh RN per 20210329 (patch 202103-05)
                        //string filterExpressionRecOrd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                        //List<PurchaseReceivePO> recOrderList = BusinessLayer.GetPurchaseReceivePOList(filterExpressionRecOrd);
                        //if (recOrderList != null)
                        //{
                        //    foreach (PurchaseReceivePO recOrder in recOrderList)
                        //    {
                        //        recOrder.IsDeleted = true;
                        //        BusinessLayer.UpdatePurchaseReceivePO(recOrder);

                        //        if(recOrder.ReceivedQuantity == recOrder.OrderedQuantity)
                        //        {
                        //            string filterRCVDt = string.Format(
                        //                    "PurchaseReceiveID = {0} AND ItemID = {1} AND PurchaseOrderID = {2} AND GCItemDetailStatus != '{3}'",
                        //                    recOrder.PurchaseReceiveID, recOrder.ItemID, recOrder.PurchaseOrderID, Constant.TransactionStatus.VOID);
                        //            PurchaseReceiveDt rcvDt = BusinessLayer.GetPurchaseReceiveDtList(filterRCVDt).FirstOrDefault();
                        //            rcvDt.PurchaseOrderID = null;
                        //            rcvDt.PurchaseOrderDtID = null;
                        //            rcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        //            rcvDt.LastUpdatedDate = DateTime.Now;
                        //            BusinessLayer.UpdatePurchaseReceiveDt(rcvDt);
                        //        }
                        //    }
                        //}
                    }
                }
                else
                {
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            return result;
        }

        #endregion

        #region callBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterIP = string.Format(string.Format("ItemID = {0} AND IsDeleted = 0 AND HealthcareID = '{1}'", hdnItemID.Value, AppSession.UserLogin.HealthcareID));
            ItemPlanning ip = BusinessLayer.GetItemPlanningList(filterIP).FirstOrDefault();
            if (ip != null)
            {
                if (ip.IsUsingSupplierCatalog)
                {
                    List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCPurchaseUnit FROM SupplierItem WHERE IsDeleted = 0 AND ItemID = {1} AND BusinessPartnerID = {2}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, hdnSupplierID.Value));
                    Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                    cboItemUnit.SelectedIndex = -1;
                }
                else
                {
                    List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND IsActive = 1 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
                    Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                    cboItemUnit.SelectedIndex = -1;
                }
            }

            //List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND (StandardCodeID IN (SELECT GCAlternateUnit FROM ItemAlternateUnit WHERE IsDeleted = 0 AND IsActive = 1 AND ItemID = {1}) OR StandardCodeID = (SELECT GCItemUnit FROM ItemMaster WHERE ItemID = {1}))", Constant.StandardCode.ITEM_UNIT, hdnItemID.Value));
            //Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
            //cboItemUnit.SelectedIndex = -1;

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
            int OrderID = 0;
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    OrderID = Convert.ToInt32(hdnOrderID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref OrderID))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                OrderID = Convert.ToInt32(hdnOrderID.Value);
                if (OnDeleteEntityDt(ref errMessage, OrderID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = OrderID.ToString();
        }

        private void ControlToEntity(PurchaseOrderDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCPurchaseUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
            entityDt.UnitPrice = Convert.ToDecimal(txtPrice.Text);

            if (chkIsDiscountInPercentage1.Checked)
            {
                entityDt.IsDiscountInPercentage1 = true;
                entityDt.DiscountPercentage1 = Convert.ToDecimal(txtDiscount.Text);
                entityDt.DiscountAmount1 = Convert.ToDecimal(hdnDiscountAmount1.Value);
            }
            else
            {
                entityDt.IsDiscountInPercentage1 = false;
                entityDt.DiscountPercentage1 = 0;
                entityDt.DiscountAmount1 = Convert.ToDecimal(hdnDiscountAmount1.Value);
            }

            if (chkIsDiscountInPercentage2.Checked)
            {
                entityDt.IsDiscountInPercentage2 = true;
                entityDt.DiscountPercentage2 = Convert.ToDecimal(txtDiscount2.Text);
                entityDt.DiscountAmount2 = Convert.ToDecimal(hdnDiscountAmount2.Value);
            }
            else
            {
                entityDt.IsDiscountInPercentage2 = false;
                entityDt.DiscountPercentage2 = 0;
                entityDt.DiscountAmount2 = Convert.ToDecimal(hdnDiscountAmount2.Value);
            }

            //entityDt.IsBonusItem = chkBonus.Checked;
            entityDt.Remarks = txtNotesDt.Text;
            entityDt.LineAmount = entityDt.CustomSubTotal;
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

            //Simpan data draft
            entityDt.DraftQuantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.DraftUnitPrice = Convert.ToDecimal(txtPrice.Text);
            entityDt.DraftDiscountPercentage1 = Convert.ToDecimal(txtDiscount.Text);
            entityDt.DraftDiscountPercentage2 = Convert.ToDecimal(txtDiscount2.Text);
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int OrderID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityDtDao = new PurchaseOrderDtDao(ctx);
            try
            {
                SavePurchaseOrderHd(ctx, ref OrderID);
                if (entityHdDao.Get(OrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PurchaseOrderDt entityDt = new PurchaseOrderDt();
                    ControlToEntity(entityDt);
                    entityDt.PurchaseOrderID = OrderID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entityHdDao.Get(OrderID).PurchaseOrderNo);
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
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseReceivePODao entityRecOrdDao = new PurchaseReceivePODao(ctx);
            try
            {
                PurchaseOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseOrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    string filterExpressionRecOrd = String.Format("PurchaseOrderID = {0} AND ItemID = {1} AND IsDeleted = 0", entityDt.PurchaseOrderID, entityDt.ItemID);
                    List<PurchaseReceivePO> recOrderList = BusinessLayer.GetPurchaseReceivePOList(filterExpressionRecOrd, ctx);
                    if (recOrderList != null)
                    {
                        foreach (PurchaseReceivePO recOrder in recOrderList)
                        {
                            recOrder.OrderedQuantity = entityDt.Quantity * entityDt.ConversionFactor;
                            entityRecOrdDao.Update(recOrder);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entityHdDao.Get(entityDt.PurchaseOrderID).PurchaseOrderNo);
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
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseReceivePODao entityPORPODao = new PurchaseReceivePODao(ctx);
            PurchaseRequestHdDao entityPRHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseReceiveHdDao entityPORHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityPORDtDao = new PurchaseReceiveDtDao(ctx);

            try
            {
                PurchaseOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseOrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if (entityDt.PurchaseRequestID != null)
                    {
                        PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(String.Format("PurchaseRequestID = {0} AND ItemID = {1}", entityDt.PurchaseRequestID, entityDt.ItemID), ctx).FirstOrDefault();
                        entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                        entityPRDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPRDtDao.Update(entityPRDt);

                        PurchaseRequestHd entityPRHd = entityPRHdDao.Get(entityPRDt.PurchaseRequestID);
                        if (entityPRHd.GCTransactionStatus == Constant.TransactionStatus.CLOSED) entityPRHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                        else if (entityPRHd.GCTransactionStatus == Constant.TransactionStatus.APPROVED) entityPRHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPRHdDao.Update(entityPRHd);
                    }

                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    string filterRcvDt = String.Format("PurchaseOrderID = {0} AND ItemID = {1} AND PurchaseOrderDtID = {2}", entityDt.PurchaseOrderID, entityDt.ItemID, entityDt.ID);
                    List<PurchaseReceiveDt> rcvDtList = BusinessLayer.GetPurchaseReceiveDtList(filterRcvDt, ctx);
                    foreach (PurchaseReceiveDt rcvDt in rcvDtList)
                    {
                        rcvDt.PurchaseOrderID = null;
                        rcvDt.PurchaseOrderDtID = null;
                        rcvDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPORDtDao.Update(rcvDt);

                        string filterRcvPO = String.Format("PurchaseOrderID = {0} AND ItemID = {1} AND PurchaseReceiveID = {2}", entityDt.PurchaseOrderID, entityDt.ItemID, rcvDt.PurchaseReceiveID);
                        List<PurchaseReceivePO> rcvPOList = BusinessLayer.GetPurchaseReceivePOList(filterRcvPO, ctx);
                        foreach (PurchaseReceivePO rcvPO in rcvPOList)
                        {
                            rcvPO.IsDeleted = true;
                            entityPORPODao.Update(rcvPO);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = string.Format("Pemesanan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entityHdDao.Get(entityDt.PurchaseOrderID).PurchaseOrderNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
    }
}