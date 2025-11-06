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
    public partial class PurchaseOrder : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_ORDER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            List<SettingParameter> lstParam = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode IN ('{0}','{1}')", Constant.SettingParameter.VAT_PERCENTAGE, Constant.SettingParameter.PPH_PERCENTAGE));
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}', '{10}')",
                                                                                    AppSession.UserLogin.HealthcareID,
                                                                                    Constant.SettingParameter.IM_SEARCH_DIALOG_TYPE,
                                                                                    Constant.SettingParameter.IM_ALLOW_PRINT_ORDER_RECEIPT_AFTER_PROPOSDED,
                                                                                    Constant.SettingParameter.IM_PENGADAAN_TAMPIL_DISCOUNT_FINAL,
                                                                                    Constant.SettingParameter.IM_PENGADAAN_TAMPIL_ONGKOS_KIRIM,
                                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED,
                                                                                    Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE,
                                                                                    Constant.SettingParameter.IM0123,
                                                                                    Constant.SettingParameter.IM0131,
                                                                                    Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY,
                                                                                    Constant.SettingParameter.IM0135
                                                                                ));
            hdnIsShowDefaultLocation.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0135).FirstOrDefault().ParameterValue;
            hdnSearchDialogType.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_SEARCH_DIALOG_TYPE).FirstOrDefault().ParameterValue;
            hdnIsAllowPrintOrderReceiptAfterProposed.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_ALLOW_PRINT_ORDER_RECEIPT_AFTER_PROPOSDED).FirstOrDefault().ParameterValue;
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            hdnIM0131.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0131).FirstOrDefault().ParameterValue;
            hdnIsUsingDownPayment.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0123).FirstOrDefault().ParameterValue;
            hdnIsPOQtyCannotOverPRQty.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_PO_QTY_CANNOT_OVER_PR_QTY).FirstOrDefault().ParameterValue;
            if (hdnIsUsingDownPayment.Value == "1")
            {
                trDPReferrenceNo.Attributes.Remove("style");
                trDP.Attributes.Remove("style");
            }
            else
            {
                trDPReferrenceNo.Attributes.Add("style", "display:none");
                trDP.Attributes.Add("style", "display:none");
            }

            string tampilDiscountFinal = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PENGADAAN_TAMPIL_DISCOUNT_FINAL).FirstOrDefault().ParameterValue;
            if (tampilDiscountFinal == "1")
            {
                trDiscountFinalPercent.Attributes.Remove("style");
                trDiscountFinal.Attributes.Remove("style");
            }
            else
            {
                trDiscountFinalPercent.Attributes.Add("style", "display:none");
                trDiscountFinal.Attributes.Add("style", "display:none");
            }

            string tampilOngkosKirim = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PENGADAAN_TAMPIL_ONGKOS_KIRIM).FirstOrDefault().ParameterValue;
            if (tampilOngkosKirim == "1")
            {
                trCharges.Attributes.Remove("style");
            }
            else
            {
                trCharges.Attributes.Add("style", "display:none");
            }

            if (lstParam != null)
            {
                hdnVATPercentage.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).FirstOrDefault().ParameterValue;
                hdnVATPercentageFromSetvar.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).FirstOrDefault().ParameterValue;
                hdnPPHPercentage.Value = lstParam.Where(lst => lst.ParameterCode == Constant.SettingParameter.PPH_PERCENTAGE).FirstOrDefault().ParameterValue;
            }
            else
            {
                hdnVATPercentage.Value = "0";
                hdnVATPercentageFromSetvar.Value = "0";
                hdnPPHPercentage.Value = "0";
            }
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_ORDER);
            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0 AND IsBlackList = 0 AND IsActive = 1", Constant.BusinessObjectType.SUPPLIER);

            hdnIsUsedRevenueCostCenter.Value = AppSession.IsUsedRevenueCostCenter;

            if (hdnIsUsedRevenueCostCenter.Value == "1")
            {
                trRevenueCostCenter.Style.Remove("display");
                lblRevenueCostCenter.Attributes.Add("class", "lblLink");
            }
            else
            {
                trRevenueCostCenter.Style.Add("display", "none");
                lblRevenueCostCenter.Attributes.Remove("class");
            }

            hdnIsUsedProductLine.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).ParameterValue;

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

            hdnGCPurchasingType.Value = Constant.PurchasingType.RUTIN;

            cboPPHOptions.Items.Add("Plus");
            cboPPHOptions.Items.Add("Minus");
            cboPPHOptions.SelectedIndex = 0;

            SetControlProperties();

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscount, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscountAmount, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscount2, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiscountAmount2, new ControlEntrySetting(true, true, false), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string GetVATPercentageDefault()
        {
            return hdnVATPercentage.Value;
        }

        protected string GetPPhPercentageDefault()
        {
            return hdnPPHPercentage.Value;
        }

        protected string GetFinalDiscountDefault()
        {
            return hdnFinalDiscount.Value;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PURCHASING_TYPE, Constant.StandardCode.PURCHASE_ORDER_TYPE, Constant.StandardCode.FRANCO_REGION, Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.PURCHASING_BUDGET_CATEGORY));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"), 50, 1, "TermDay");
            Methods.SetComboBoxField<StandardCode>(cboPurchasingType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASING_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboFrancoRegion, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.FRANCO_REGION).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            Methods.SetComboBoxField<StandardCode>(cboBudgetCategory, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PURCHASING_BUDGET_CATEGORY).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboPurchasingType.SelectedIndex = 0;
            cboFrancoRegion.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;
            cboTerm.SelectedIndex = 1;
            cboBudgetCategory.SelectedIndex = 0;
            //chkPPN.Checked = true;

            List<StandardCode> listStandardCodePO = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.PURCHASE_ORDER_TYPE));
            listStandardCodePO.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPurchaseOrderType, listStandardCodePO, "StandardCodeName", "StandardCodeID");
            cboPurchaseOrderType.SelectedIndex = 0;

            List<StandardCode> listStandardCodePPh = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.PPH_TYPE));
            listStandardCodePPh.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPPHType, listStandardCodePPh, "StandardCodeName", "StandardCodeID");
            cboPPHType.SelectedIndex = 0;

            //hdnRecordFilterExpression.Value = string.Format("LocationID = {0}", hdnLocationID.Value);
        }

        protected override void OnControlEntrySetting()
        {
            SettingParameterDt setvarDTR = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC);

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
                SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, true, true, gcPurchaseOrderType));
            }
            else
            {
                SetControlEntrySetting(cboPurchasingType, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(cboPurchaseOrderType, new ControlEntrySetting(true, true, true));
            }

            SetControlEntrySetting(hdnOrderID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtOrderNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboPurchasingType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOtherReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOtherRequestReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsCampaign, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemOrderDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderDeliveryDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtItemOrderExpiredDate, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboFrancoRegion, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, true, true, "1"));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsPPHInPercentage, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtPaymentRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTotalOrder, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(cboPPHType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPPHPercentage, new ControlEntrySetting(true, true, true, GetPPhPercentageDefault()));
            SetControlEntrySetting(txtPPHAmount, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtDPReferrenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtCharges, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtTotalOrderSaldo, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            if (hdnIsShowDefaultLocation.Value == "1")
            {
                SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true, locationCode));
                SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true, locationName));
                SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(true, false, true, locationID));
            }
            else
            {
                SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
                SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(true, false, true));
            }
            SetControlEntrySetting(hdnLocationItemGroupID, new ControlEntrySetting(true, false, true, locationItemGroupID));
            SetControlEntrySetting(chkIsUsingTermPO, new ControlEntrySetting(true, true, false));

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

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string locationID = hdnLocationID.Value.ToString();
            string filterExpression;
            if (locationID != "" && locationID != null)
            {
                filterExpression = string.Format("LocationID = {0} AND TransactionCode = {1}", locationID, Constant.TransactionCode.PURCHASE_ORDER);
            }
            else
            {
                filterExpression = string.Format("TransactionCode = {0}", Constant.TransactionCode.PURCHASE_ORDER);
            }
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseOrderHd1RowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseOrderHd1 entity = BusinessLayer.GetvPurchaseOrderHd1(filterExpression, PageIndex, "PurchaseOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseOrderHd1RowIndex(filterExpression, keyValue, "PurchaseOrderID DESC");
            vPurchaseOrderHd1 entity = BusinessLayer.GetvPurchaseOrderHd1(filterExpression, PageIndex, "PurchaseOrderID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseOrderHd1 entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
                SetControlEntrySetting(txtItemOrderDeliveryDate, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtItemOrderExpiredDate, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtRevenueCostCenterCode, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtOtherReferenceNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtOtherRequestReferenceNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsCampaign, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPaymentRemarks, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtTotalOrder, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsFinalDiscountInPercentage, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsPPHInPercentage, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPPHPercentage, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPPHAmount, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtDPReferrenceNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtDP, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtCharges, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(lblSupplier, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsUsingTermPO, new ControlEntrySetting(false, false, false));

                cboPurchasingType.Enabled = false;
                cboPurchaseOrderType.Enabled = false;
                cboTerm.Enabled = false;
                cboFrancoRegion.Enabled = false;
                cboPPHType.Enabled = false;
                cboPPHOptions.Enabled = false;
                txtFinalDiscountInPercentage.Enabled = false;
                txtVATPercentageDefault.Enabled = false;
                txtPPHPercentage.Enabled = false;
            }
            else
            {
                hdnIsEditable.Value = "1";

                cboPurchasingType.Enabled = true;
                cboPurchaseOrderType.Enabled = true;
                cboTerm.Enabled = true;
                cboFrancoRegion.Enabled = true;
                cboPPHType.Enabled = true;
                cboPPHOptions.Enabled = true;
                txtFinalDiscountInPercentage.Enabled = true;
                txtVATPercentageDefault.Enabled = true;
                txtPPHPercentage.Enabled = true;
            }

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

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            string filterReceive = String.Format("PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveDt WHERE PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}') AND GCTransactionStatus != '{1}'", entity.PurchaseOrderID, Constant.TransactionStatus.VOID);
            List<PurchaseReceiveHd> lsteceive = BusinessLayer.GetPurchaseReceiveHdList(filterReceive);
            if (lsteceive.Count > 0)
            {
                SetControlEntrySetting(lblSupplier, new ControlEntrySetting(false, false, false));
            }
            hdnOrderID.Value = entity.PurchaseOrderID.ToString();
            hdnPurchasingType.Value = entity.GCPurchasingType.ToString();
            hdnGCPurchasingType.Value = entity.GCPurchasingType.ToString();
            hdnPurchaseOrderType.Value = entity.GCPurchaseOrderType.ToString();
            hdnGCPurchaseOrderType.Value = entity.GCPurchaseOrderType.ToString();
            txtOrderNo.Text = entity.PurchaseOrderNo;
            txtReferenceNo.Text = entity.ReferenceNo;
            txtOtherReferenceNo.Text = entity.OtherReferenceNo;
            txtOtherRequestReferenceNo.Text = entity.OtherRequestReferenceNo;
            chkIsUrgent.Checked = entity.IsUrgent;
            chkIsCampaign.Checked = entity.IsCampaign;
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
            cboPurchasingType.Value = entity.GCPurchasingType;
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

            cboPPHType.Value = entity.GCPPhType;
            if (entity.PPHMode == true)
            {
                cboPPHOptions.Value = "Plus";
            }
            else
            {
                cboPPHOptions.Value = "Minus";
            }
            txtTotalOrder.Text = entity.TransactionAmount.ToString();
            chkIsPPHInPercentage.Checked = entity.IsPPHInPercentage;
            txtPPHPercentage.Text = entity.PPHPercentage.ToString();
            txtPPHAmount.Text = entity.PPHAmount.ToString();
            //txtPPHAmount.Attributes.Add("hiddenVal", entity.PPHAmount.ToString());
            chkIsFinalDiscountInPercentage.Checked = entity.IsFinalDiscountInPercentage;
            txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            txtFinalDiscount.Text = entity.FinalDiscountAmount.ToString();
            txtDPReferrenceNo.Text = entity.DownPaymentReferenceNo;
            txtDP.Text = entity.DownPaymentAmount.ToString("N");
            txtDP.Attributes.Add("hiddenVal", entity.DownPaymentAmount.ToString());
            txtCharges.Text = entity.ChargesAmount.ToString();
            chkIsUsingTermPO.Checked = entity.IsUsingTermPO;
            hdnIsUsingTermPO.Value = entity.IsUsingTermPO ? "1" : "0";

            decimal totalOrderSaldo = 0;
            decimal ppn = 0;
            decimal pph = 0;

            if (entity.IsIncludeVAT)
            {
                ppn = (entity.TransactionAmount - entity.FinalDiscountAmount) * entity.VATPercentage / 100;
            }

            if (entity.IsPPHInPercentage)
            {
                pph = entity.TransactionAmount * entity.PPHPercentage / 100;
            }
            else
            {
                pph = entity.PPHAmount;
            }
            if (entity.PPHMode == false)
            {
                pph = pph * -1;
            }

            totalOrderSaldo = entity.TransactionAmount + ppn + pph - entity.FinalDiscountAmount - entity.DownPaymentAmount + entity.ChargesAmount;

            txtTotalOrderSaldo.Text = totalOrderSaldo.ToString();

            hdnRevenueCostCenterID.Value = entity.RevenueCostCenterID.ToString();
            txtRevenueCostCenterCode.Text = entity.RevenueCostCenterCode;
            txtRevenueCostCenterName.Text = entity.RevenueCostCenterName;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            divPrintNumber.InnerHtml = entity.PrintNumber.ToString("G29");

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
            if (entity.ClosedReason != null && entity.ClosedReason != "")
            {
                divClosedReason.InnerHtml = entity.ClosedReasonName + " | " + entity.ClosedReason;
            }
            else
            {
                divClosedReason.InnerHtml = entity.ClosedReasonName;
            }

            List<GetDateDiffPOPORPerSupplier> ETA = BusinessLayer.GetDateDiffPOPORPerSupplier(entity.BusinessPartnerID);
            txtETA.Text = String.Format("{0} Hari", ETA.FirstOrDefault().Hasil.ToString());

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
                transactionAmount = BusinessLayer.GetvPurchaseOrderHd1List(string.Format("PurchaseOrderID = {0}", hdnOrderID.Value)).FirstOrDefault().TransactionAmount;

            List<vPurchaseOrderDt> lstEntity = BusinessLayer.GetvPurchaseOrderDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseOrderDt entity = e.Row.DataItem as vPurchaseOrderDt;
                CheckBox chkIsBonus = e.Row.FindControl("chkIsBonus") as CheckBox;
                CheckBox chkIsDiscInPct1 = e.Row.FindControl("chkIsDiscInPct1") as CheckBox;
                CheckBox chkIsDiscInPct2 = e.Row.FindControl("chkIsDiscInPct2") as CheckBox;

                chkIsBonus.Checked = entity.IsBonusItem;
                chkIsDiscInPct1.Checked = entity.IsDiscountInPercentage1;
                chkIsDiscInPct2.Checked = entity.IsDiscountInPercentage2;

                if (entity.LineAmount <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntityHd(PurchaseOrderHd entity)
        {
            entity.DeliveryDate = Helper.GetDatePickerValue(txtItemOrderDeliveryDate.Text);
            entity.POExpiredDate = Helper.GetDatePickerValue(txtItemOrderExpiredDate.Text);
            entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entity.IsUrgent = chkIsUrgent.Checked;
            entity.IsCampaign = chkIsCampaign.Checked;
            entity.PaymentRemarks = txtPaymentRemarks.Text;
            entity.DownPaymentReferenceNo = txtDPReferrenceNo.Text;
            entity.DownPaymentAmount = Convert.ToDecimal(txtDP.Text);
            entity.Remarks = txtNotes.Text;
            entity.OtherReferenceNo = txtOtherReferenceNo.Text;
            entity.OtherRequestReferenceNo = txtOtherRequestReferenceNo.Text;
            entity.IsIncludeVAT = chkPPN.Checked;
            if (cboPPHType.Value == null)
            {
                entity.GCPPHType = null;
            }
            else
            {
                entity.GCPPHType = cboPPHType.Value.ToString();
            }
            if (cboPPHOptions.Text.Equals("Plus"))
            {
                entity.PPHMode = true;
            }
            else
            {
                entity.PPHMode = false;
            }
            entity.IsPPHInPercentage = chkIsPPHInPercentage.Checked;
            entity.PPHPercentage = Convert.ToDecimal(Request.Form[txtPPHPercentage.UniqueID]);
            entity.PPHAmount = Convert.ToDecimal(Request.Form[txtPPHAmount.UniqueID]);

            if (entity.PPHPercentage != 0 || entity.PPHAmount != 0)
            {
                entity.IsIncludePPh = true;
            }
            else
            {
                entity.IsIncludePPh = false;
            }

            if (chkIsFinalDiscountInPercentage.Checked)
            {
                entity.IsFinalDiscountInPercentage = true;
            }
            else
            {
                entity.IsFinalDiscountInPercentage = false;
            }
            entity.FinalDiscount = Convert.ToDecimal(Request.Form[txtFinalDiscountInPercentage.UniqueID]);
            entity.FinalDiscountAmount = Convert.ToDecimal(Request.Form[txtFinalDiscount.UniqueID]);

            if (entity.IsIncludeVAT)
            {
                entity.VATPercentage = Convert.ToDecimal(Request.Form[txtVATPercentageDefault.UniqueID]);
            }
            else
            {
                entity.VATPercentage = 0;
            }

            if (hdnRevenueCostCenterID.Value == "0" || hdnRevenueCostCenterID.Value == "")
            {
                entity.RevenueCostCenterID = null;
            }
            else
            {
                entity.RevenueCostCenterID = Convert.ToInt32(hdnRevenueCostCenterID.Value);
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }

            if (hdnOrderID.Value.ToString() == "" || hdnOrderID.Value.ToString() == "0")
            {
                entity.OrderDate = Helper.GetDatePickerValue(txtItemOrderDate.Text);
            }

            entity.GCPurchasingType = cboPurchasingType.Value.ToString();
            entity.GCPurchaseOrderType = cboPurchaseOrderType.Value.ToString();
            entity.TermID = Convert.ToInt32(cboTerm.Value.ToString());
            entity.GCFrancoRegion = cboFrancoRegion.Value.ToString();
            entity.GCCurrencyCode = cboCurrency.Value.ToString();
            entity.CurrencyRate = Convert.ToDecimal(txtKurs.Text);

            entity.ChargesAmount = Convert.ToDecimal(txtCharges.Text);

            if (Convert.ToDecimal(txtCharges.Text) != 0)
            {
                entity.GCChargesType = Constant.ChargesType.ONGKOS_KIRIM;
            }
            else
            {
                entity.GCChargesType = null;
            }

            entity.DownPaymentAmount = Convert.ToDecimal(txtDP.Text);

            entity.IsUsingTermPO = chkIsUsingTermPO.Checked;
        }

        public void SavePurchaseOrderHd(IDbContext ctx, ref int OrderID)
        {
            hdnOrderID.Value = OrderID.ToString();
            PurchaseOrderHdDao entityHdDao = new PurchaseOrderHdDao(ctx);
            if (hdnOrderID.Value == "0")
            {
                PurchaseOrderHd entityHd = new PurchaseOrderHd();
                ControlToEntityHd(entityHd);
                entityHd.TransactionCode = Constant.TransactionCode.PURCHASE_ORDER;
                entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
                entityHd.PurchaseOrderNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.OrderDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;

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

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            if (cboPurchaseOrderType.Value.ToString() != "" && cboPurchaseOrderType.Value != null)
            {
                return true;
            }
            else
            {
                errMessage = "Mohon isi Jenis Permintaan terlebih dahulu.";
                return false;
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int OrderID = 0;

                Boolean isAllowAdd = true;
                if (cboPurchasingType.Value.ToString() == Constant.PurchasingType.NON_RUTIN)
                {
                    if (hdnRevenueCostCenterID.Value == "0" || hdnRevenueCostCenterID.Value == "")
                    {
                        isAllowAdd = false;
                    }
                }

                if (isAllowAdd)
                {
                    SavePurchaseOrderHd(ctx, ref OrderID);
                    retval = OrderID.ToString();
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Mohon isi Revenue Cost Center terlebih dahulu.";
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

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            try
            {
                PurchaseOrderHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    Boolean isAllowEdit = true;
                    if (cboPurchasingType.Value.ToString() == Constant.PurchasingType.NON_RUTIN)
                    {
                        if (hdnRevenueCostCenterID.Value == "0" || hdnRevenueCostCenterID.Value == "")
                        {
                            isAllowEdit = false;
                        }
                    }

                    if (isAllowEdit)
                    {
                        int countTermPO = 0;
                        bool isBlockedValidateTermPO = false;
                        bool oldIsUsingTermPO = entity.IsUsingTermPO;
                        bool newIsUsingTermPO = chkIsUsingTermPO.Checked;
                        bool isBlockedPPHType = false;
                        decimal pphAmount = Convert.ToDecimal(Request.Form[txtPPHAmount.UniqueID]);
                        string pphType = string.Empty;
                        if (cboPPHType.Value != null)
                        {
                            pphType = cboPPHType.Value.ToString();
                        }

                        if (pphAmount != 0 && pphType == "")
                        {
                            isBlockedPPHType = true;
                        }

                        if (!isBlockedPPHType)
                        {
                            if (oldIsUsingTermPO && !newIsUsingTermPO)
                            {
                                string filterTermPO = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus <> '{1}'", entity.PurchaseOrderID, Constant.TransactionStatus.VOID);
                                List<PurchaseOrderTerm> listPOTerm = BusinessLayer.GetPurchaseOrderTermList(filterTermPO, ctx);
                                countTermPO = listPOTerm.Count();
                                if (countTermPO > 0)
                                {
                                    isBlockedValidateTermPO = true;
                                }
                            }

                            if (!isBlockedValidateTermPO)
                            {
                                ControlToEntityHd(entity);
                                purchaseHdDao.Update(entity);
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                result = false;
                                errMessage = "Tidak bisa membatalkan centangan Menggunakan Termin karena masih ada " + countTermPO.ToString() + " data termin PO yg masih aktif.";
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Mohon isi Jenis PPH terlebih dahulu.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Mohon isi Revenue Cost Center terlebih dahulu.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
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
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestHdDao entityPRHDDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);

            try
            {
                PurchaseOrderHd purchaseHd = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                if (purchaseHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    bool isAllowApproved = true;

                    if (cboPurchasingType.Value.ToString() == Constant.PurchasingType.NON_RUTIN)
                    {
                        if (hdnRevenueCostCenterID.Value == "0" || hdnRevenueCostCenterID.Value == "")
                        {
                            isAllowApproved = false;
                        }
                    }

                    int countTermPO = 0;
                    bool isBlockedValidateTermPO = false;
                    bool oldIsUsingTermPO = purchaseHd.IsUsingTermPO;
                    bool newIsUsingTermPO = chkIsUsingTermPO.Checked;
                    bool isBlockedPPHType = false;
                    decimal pphAmount = Convert.ToDecimal(Request.Form[txtPPHAmount.UniqueID]);
                    string pphType = string.Empty;
                    if (cboPPHType.Value != null)
                    {
                        pphType = cboPPHType.Value.ToString();
                    }

                    if (pphAmount != 0 && pphType == "")
                    {
                        isBlockedPPHType = true;
                    }

                    if (oldIsUsingTermPO && !newIsUsingTermPO)
                    {
                        string filterTermPO = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus <> '{1}'", purchaseHd.PurchaseOrderID, Constant.TransactionStatus.VOID);
                        List<PurchaseOrderTerm> listPOTerm = BusinessLayer.GetPurchaseOrderTermList(filterTermPO, ctx);
                        countTermPO = listPOTerm.Count();
                        if (countTermPO > 0)
                        {
                            isBlockedValidateTermPO = true;
                        }
                    }

                    if (!isBlockedPPHType)
                    {
                        if (!isBlockedValidateTermPO)
                        {
                            if (isAllowApproved)
                            {
                                ControlToEntityHd(purchaseHd);
                                purchaseHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                purchaseHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseHd.ApprovedDate = DateTime.Now;
                                purchaseHd.ApprovedBy = AppSession.UserLogin.UserID;
                                purchaseHdDao.Update(purchaseHd);

                                string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                                List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd, ctx);
                                foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                                {
                                    purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseDtDao.Update(purchaseDt);

                                    List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("ItemID = {0} AND PurchaseOrderID = {1}", purchaseDt.ItemID, hdnOrderID.Value), ctx);
                                    if (lstPurchaseRequestPO.Count > 0)
                                    {
                                        foreach (PurchaseRequestPO entityPRPO in lstPurchaseRequestPO)
                                        {
                                            List<PurchaseRequestDt> entityPRDtList = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                                        "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                                        entityPRPO.PurchaseRequestID, purchaseDt.ItemID, Constant.TransactionStatus.VOID), ctx);
                                            if (entityPRDtList.Count() > 0)
                                            {
                                                PurchaseRequestDt entityPRDt = entityPRDtList.LastOrDefault();
                                                if (entityPRDt.OrderedQuantity >= entityPRDt.Quantity)
                                                {
                                                    entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityPRDTDao.Update(entityPRDt);
                                                }

                                                int Count = BusinessLayer.GetPurchaseRequestDtRowCount(string.Format(
                                                                                    "PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus != '{1}'",
                                                                                    entityPRDt.PurchaseRequestID, Constant.TransactionStatus.CLOSED), ctx);
                                                if (Count == 0)
                                                {
                                                    PurchaseRequestHd entityPRHd = BusinessLayer.GetPurchaseRequestHdList(string.Format(
                                                                                                        "PurchaseRequestID = {0} AND GCTransactionStatus != '{1}'",
                                                                                                        entityPRDt.PurchaseRequestID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                                                    entityPRHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                                    entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                                    ctx.CommandType = CommandType.Text;
                                                    ctx.Command.Parameters.Clear();
                                                    entityPRHDDao.Update(entityPRHd);
                                                }
                                            }
                                        }
                                    }
                                }
                                ctx.CommitTransaction();
                            }
                            else
                            {
                                errMessage = "Harap isi Revenue Cost Center terlebih dahulu.";
                                result = false;
                                ctx.RollBackTransaction();
                            }
                        }
                        else
                        {
                            result = false;
                            errMessage = "Tidak bisa membatalkan centangan Menggunakan Termin karena masih ada " + countTermPO.ToString() + " data termin PO yg masih aktif.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Harap isi Jenis PPH terlebih dahulu.";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
                    result = false;
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
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);

            try
            {
                PurchaseOrderHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    int countTermPO = 0;
                    bool isBlockedValidateTermPO = false;
                    bool oldIsUsingTermPO = entity.IsUsingTermPO;
                    bool newIsUsingTermPO = chkIsUsingTermPO.Checked;

                    bool isBlockedPPHType = false;
                    decimal pphAmount = Convert.ToDecimal(Request.Form[txtPPHAmount.UniqueID]);
                    string pphType = string.Empty;
                    if (cboPPHType.Value != null)
                    {
                        pphType = cboPPHType.Value.ToString();
                    }

                    if (pphAmount != 0 && pphType == "")
                    {
                        isBlockedPPHType = true;
                    }

                    if (oldIsUsingTermPO && !newIsUsingTermPO)
                    {
                        string filterTermPO = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus <> '{1}'", entity.PurchaseOrderID, Constant.TransactionStatus.VOID);
                        List<PurchaseOrderTerm> listPOTerm = BusinessLayer.GetPurchaseOrderTermList(filterTermPO, ctx);
                        countTermPO = listPOTerm.Count();
                        if (countTermPO > 0)
                        {
                            isBlockedValidateTermPO = true;
                        }
                    }

                    if (!isBlockedPPHType)
                    {
                        if (!isBlockedValidateTermPO)
                        {
                            ControlToEntityHd(entity);
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entity.ProposedDate = DateTime.Now;
                            entity.ProposedBy = AppSession.UserLogin.UserID;
                            purchaseHdDao.Update(entity);

                            string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                            List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd, ctx);
                            foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                            {
                                purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                                purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                purchaseDtDao.Update(purchaseDt);
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Tidak bisa membatalkan centangan Menggunakan Termin karena masih ada " + countTermPO.ToString() + " data termin PO yg masih aktif.";
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        errMessage = "Harap isi Jenis PPH terlebih dahulu.";
                        result = false;
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                result = false;
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
            PurchaseOrderHdDao purchaseHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestHdDao entityPRHDDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                PurchaseOrderHd entityPOHd = purchaseHdDao.Get(Convert.ToInt32(hdnOrderID.Value));

                if (entityPOHd.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityPOHd.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entityPOHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                    purchaseHdDao.Update(entityPOHd);

                    string filterExpressionPurchaseOrderHd = String.Format("PurchaseOrderID = {0} AND IsDeleted = 0", hdnOrderID.Value);
                    List<PurchaseOrderDt> lstPurchaseOrderDt = BusinessLayer.GetPurchaseOrderDtList(filterExpressionPurchaseOrderHd, ctx);
                    foreach (PurchaseOrderDt purchaseDt in lstPurchaseOrderDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);

                        List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("ItemID = {0} AND PurchaseOrderID = {1}", purchaseDt.ItemID, hdnOrderID.Value), ctx);
                        foreach (PurchaseRequestPO entityPRPO in lstPurchaseRequestPO)
                        {
                            PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                                            "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                                            entityPRPO.PurchaseRequestID, purchaseDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                            //decimal orderQty = entityPRDt.OrderedQuantity - entityPRPO.RequestQuantity;

                            //if (entityPRDt.OrderedQuantity < entityPRPO.RequestQuantity)
                            //{
                            //    orderQty = entityPRDt.OrderedQuantity - purchaseDt.Quantity;
                            //}
                            //else if (entityPRDt.OrderedQuantity > entityPRPO.RequestQuantity)
                            //{
                            //    orderQty = 0;
                            //}

                            entityPRDt.OrderedQuantity -= entityPRPO.OrderQuantity;

                            string[] poInfArr = entityPRDt.OrderInformation.Split('|');
                            string poInfNew = "";

                            for (int i = 0; i < poInfArr.Count(); i++)
                            {
                                if (poInfArr[i] != "")
                                {
                                    if (poInfArr[i] != purchaseDt.PurchaseOrderID.ToString())
                                    {
                                        poInfNew += "|" + poInfArr[i];
                                    }
                                }
                            }
                            entityPRDt.OrderInformation = poInfNew;

                            //String PRInformation = entityPRDt.OrderInformation.Replace("|" + purchaseDt.PurchaseOrderID.ToString(), "");
                            //if (PRInformation != "|" && PRInformation != String.Empty)
                            //{
                            //    entityPRDt.OrderInformation = PRInformation;
                            //}
                            //else
                            //{
                            //    entityPRDt.OrderInformation = null;
                            //}
                            entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            entityPRDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            entityPRDTDao.Update(entityPRDt);

                            PurchaseRequestHd entityPRHd = entityPRHDDao.Get(entityPRDt.PurchaseRequestID);
                            if (entityPRHd.GCTransactionStatus == Constant.TransactionStatus.CLOSED)
                            {
                                entityPRHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityPRHDDao.Update(entityPRHd);
                            }

                            entityPRPO.OrderQuantity = entityPRDt.OrderedQuantity;
                            entityPRPODao.Update(entityPRPO);
                        }
                    }
                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
                    ctx.RollBackTransaction();
                    result = false;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        #endregion

        #region callBack Trigger
        protected void cboItemUnit_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            bool isUsingCatalogSupplier = false;

            string filterIP = string.Format(string.Format("ItemID = {0} AND IsDeleted = 0 AND HealthcareID = '{1}'", hdnItemID.Value, AppSession.UserLogin.HealthcareID));
            ItemPlanning ip = BusinessLayer.GetItemPlanningList(filterIP).FirstOrDefault();
            if (ip != null)
            {
                isUsingCatalogSupplier = ip.IsUsingSupplierCatalog;
            }

            if (isUsingCatalogSupplier)
            {
                string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT GCPurchaseUnit FROM SupplierItem WITH(NOLOCK) WHERE IsDeleted = 0 AND ItemID = {1} AND BusinessPartnerID = {2}) OR StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                    Constant.StandardCode.ITEM_UNIT, hdnItemID.Value, hdnSupplierID.Value);
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
                Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                cboItemUnit.SelectedIndex = -1;
            }
            else
            {
                string filterSC = string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1 AND (StandardCodeID IN (SELECT vaiu.GCAlternateUnit FROM vItemAlternateItemUnit vaiu WITH(NOLOCK) WHERE vaiu.ItemID = {1} AND vaiu.IsDeleted = 0 AND vaiu.IsActive = 1))",
                                                    Constant.StandardCode.ITEM_UNIT, hdnItemID.Value);
                List<StandardCode> lst = BusinessLayer.GetStandardCodeList(filterSC);
                Methods.SetComboBoxField<StandardCode>(cboItemUnit, lst, "StandardCodeName", "StandardCodeID");
                cboItemUnit.SelectedIndex = 0;
            }

            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "edit")
                {
                    result = "edit";
                }
                else if (param[0] == "addItem")
                {
                    result = "addItem";
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
                OrderID = Convert.ToInt32(hdnOrderID.Value);
                if (hdnEntryID.Value.ToString() != "")
                {
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
            else if (param[0] == "updatePrintNo")
            {
                if (OnUpdatePrintNumber(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            else if (param[0] == "recalculate")
            {
                if (OnRecalculate(ref errMessage))
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
            entityDt.IsBonusItem = chkIsBonus.Checked;
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);

            if (hdnQtyEndLocation.Value != "0" && hdnQtyEndLocation.Value != "" && hdnQtyEndLocation.Value != null)
            {
                entityDt.QtyENDLocation = Convert.ToDecimal(hdnQtyEndLocation.Value);
                entityDt.GCItemUnitQtyENDLocation = hdnGCItemUnitQtyEndLocation.Value;
            }
            else
            {
                entityDt.QtyENDLocation = 0;
                entityDt.GCItemUnitQtyENDLocation = null;
            }

            entityDt.GCPurchaseUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
            entityDt.GCBudgetCategory = cboBudgetCategory.Value.ToString();
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
                entityDt.DiscountPercentage1 = Convert.ToDecimal(txtDiscount.Text);
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
                entityDt.DiscountPercentage2 = Convert.ToDecimal(txtDiscount2.Text);
                entityDt.DiscountAmount2 = Convert.ToDecimal(hdnDiscountAmount2.Value);
            }

            entityDt.Remarks = txtNotesDt.Text;

            //ditutup AG 20210216 patch 202102-03
            //entityDt.LineAmount = entityDt.CustomSubTotal;
            //entityDt.LineAmount = (Convert.ToDecimal(txtPrice.Text) - (Convert.ToDecimal(hdnDiscountAmount1.Value) + Convert.ToDecimal(hdnDiscountAmount2.Value)));

            entityDt.LineAmount = entityDt.CustomSubTotal2;

            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;

            entityDt.DraftQuantity = entityDt.Quantity;
            entityDt.DraftUnitPrice = entityDt.UnitPrice;
            entityDt.IsDraftDiscountInPercentage1 = entityDt.IsDiscountInPercentage1;
            entityDt.DraftDiscountPercentage1 = entityDt.DiscountPercentage1;
            entityDt.DraftDiscountAmount1 = entityDt.DiscountAmount1;
            entityDt.IsDraftDiscountInPercentage2 = entityDt.IsDiscountInPercentage2;
            entityDt.DraftDiscountPercentage2 = entityDt.DiscountPercentage2;
            entityDt.DraftDiscountAmount2 = entityDt.DiscountAmount2;
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
                    result = false;
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
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
            PurchaseRequestHdDao entityPRHDDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestDtDao entityPRDTDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                PurchaseOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseOrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.IsDeleted == false && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    //Ambil Qty PO Sebelum Di Edit
                    decimal QtyOld = 0;
                    QtyOld = entityDt.Quantity * entityDt.ConversionFactor;

                    ControlToEntity(entityDt);

                    decimal QtyNew = entityDt.Quantity * entityDt.ConversionFactor;
                    decimal selisih = QtyOld - QtyNew;

                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;

                    decimal tempSelisih;
                    List<PurchaseRequestPO> lstEntityPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0} AND ItemID = {1} ORDER BY ID DESC", entityDt.PurchaseOrderID, entityDt.ItemID, ctx));
                    foreach (PurchaseRequestPO entityPurchaseRequestPO in lstEntityPurchaseRequestPO)
                    {
                        tempSelisih = selisih;
                        PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                    "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                    entityPurchaseRequestPO.PurchaseRequestID, entityDt.ItemID, Constant.TransactionStatus.VOID)).FirstOrDefault();
                        if (tempSelisih > entityPurchaseRequestPO.RequestQuantity * entityPRDt.ConversionFactor)
                        {
                            tempSelisih = entityPurchaseRequestPO.RequestQuantity * entityPRDt.ConversionFactor;
                        }
                        entityPRDt.OrderedQuantity = entityPRDt.OrderedQuantity - (tempSelisih / entityPRDt.ConversionFactor);
                        entityPRDTDao.Update(entityPRDt);

                        entityPurchaseRequestPO.OrderQuantity = entityPRDt.OrderedQuantity;
                        entityPRPODao.Update(entityPurchaseRequestPO);

                        selisih -= tempSelisih;
                        if (selisih == 0) break;
                    }
                    entityDtDao.Update(entityDt);

                    if (result)
                    {
                        //WR : terkait perbaikan issue RSAJ-203
                        bool resultOverQty = true;
                        if (hdnIsPOQtyCannotOverPRQty.Value == "1")
                        {
                            if (entityDt.PurchaseRequestID != 0 && entityDt.PurchaseRequestID != null)
                            {
                                Decimal jmlhQtyPR_Check = 0, jmlhQtyPO_Check = 0;
                                foreach (PurchaseRequestPO entityPurchaseRequestPO in lstEntityPurchaseRequestPO)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                        "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                        entityPurchaseRequestPO.PurchaseRequestID, entityDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                                    jmlhQtyPR_Check += entityPRDt.Quantity * entityPRDt.ConversionFactor;
                                    jmlhQtyPO_Check += entityPRDt.OrderedQuantity * entityPRDt.ConversionFactor;
                                }

                                if (jmlhQtyPO_Check > jmlhQtyPR_Check)
                                {
                                    resultOverQty = false;
                                }
                            }
                        }

                        if (resultOverQty)
                        {
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Pemesanan barang tidak bisa diubah. Quantity melebihi dari jumlah permintaan. Harap untuk merefresh halaman ini";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
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
            PurchaseRequestDtDao entityPRDtDao = new PurchaseRequestDtDao(ctx);
            PurchaseRequestHdDao entityPRHdDao = new PurchaseRequestHdDao(ctx);
            PurchaseRequestPODao entityPRPODao = new PurchaseRequestPODao(ctx);
            try
            {
                PurchaseOrderDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseOrderID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("ItemID = {0} AND PurchaseOrderID = {1}", entityDt.ItemID, hdnOrderID.Value), ctx);
                    decimal qtyPengurang = entityDt.Quantity;
                    foreach (PurchaseRequestPO entityPRPO in lstPurchaseRequestPO)
                    {
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        PurchaseRequestDt entityPRDt = BusinessLayer.GetPurchaseRequestDtList(string.Format(
                                                    "PurchaseRequestID = {0} AND ItemID = {1} AND IsDeleted = 0 AND GCItemDetailStatus != '{2}'",
                                                    entityPRPO.PurchaseRequestID, entityDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();

                        decimal orderQty = entityPRDt.OrderedQuantity - entityPRPO.OrderQuantity;

                        if (entityPRDt.OrderedQuantity < entityPRPO.RequestQuantity)
                        {
                            decimal qtyPengurangDetail = qtyPengurang;
                            if (qtyPengurangDetail > entityPRDt.OrderedQuantity)
                                qtyPengurangDetail = entityPRDt.OrderedQuantity;

                            orderQty = (entityPRDt.OrderedQuantity - qtyPengurangDetail);

                            qtyPengurang -= qtyPengurangDetail;
                        }
                        else if (entityPRDt.OrderedQuantity > entityPRPO.RequestQuantity)
                        {
                            orderQty = 0;
                        }

                        entityPRDt.OrderedQuantity = orderQty;

                        string[] poInfArr = entityPRDt.OrderInformation.Split('|');
                        string poInfNew = "";

                        for (int i = 0; i < poInfArr.Count(); i++)
                        {
                            if (poInfArr[i] != "")
                            {
                                if (poInfArr[i] != entityDt.PurchaseOrderID.ToString())
                                {
                                    poInfNew += "|" + poInfArr[i];
                                }
                            }
                        }
                        entityPRDt.OrderInformation = poInfNew;

                        //String PRInformation = entityPRDt.OrderInformation.Replace("|" + entityDt.PurchaseOrderID.ToString(), "");
                        //if (PRInformation != "|" && PRInformation != String.Empty)
                        //{
                        //    entityPRDt.OrderInformation = PRInformation;
                        //}
                        //else
                        //{
                        //    entityPRDt.OrderInformation = null;
                        //}

                        entityPRDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                        entityPRDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPRDtDao.Update(entityPRDt);

                        PurchaseRequestHd entityPRHd = entityPRHdDao.Get(entityPRDt.PurchaseRequestID);
                        if (entityPRHd.GCTransactionStatus == Constant.TransactionStatus.CLOSED)
                        {
                            entityPRHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            entityPRHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPRHdDao.Update(entityPRHd);
                        }

                        entityPRPO.OrderQuantity = entityPRDt.OrderedQuantity;
                        entityPRPODao.Update(entityPRPO);
                    }
                    entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
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
        #endregion

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

        private bool OnRecalculate(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseOrderHdDao entityDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityDtDao = new PurchaseOrderDtDao(ctx);
            try
            {
                Int32 supplierID = Convert.ToInt32(hdnSupplierID.Value);
                PurchaseOrderHd entity = entityDao.Get(Convert.ToInt32(hdnOrderID.Value));

                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    string filterReceive = String.Format("PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveDt WHERE PurchaseOrderID = {0} AND GCItemDetailStatus != '{1}') AND GCTransactionStatus != '{1}'", Convert.ToInt32(hdnOrderID.Value), Constant.TransactionStatus.VOID);
                    List<PurchaseReceiveHd> lstReceive = BusinessLayer.GetPurchaseReceiveHdList(filterReceive, ctx);
                    if (lstReceive.Count() == 0)
                    {
                        List<PurchaseOrderDt> lstItem = BusinessLayer.GetPurchaseOrderDtList(String.Format("PurchaseOrderID = {0} AND GCItemDetailStatus = '{1}' AND IsDeleted = 0", hdnOrderID.Value, Constant.TransactionStatus.OPEN), ctx);

                        foreach (PurchaseOrderDt x in lstItem)
                        {
                            if (hdnIM0131.Value == "0")
                            {
                                #region take price
                                GetItemMasterPurchase itemMasterPurchase = BusinessLayer.GetItemMasterPurchaseList(AppSession.UserLogin.HealthcareID, x.ItemID, supplierID, ctx).FirstOrDefault();
                                String cboItemUnitValue = x.GCPurchaseUnit;
                                String GCBaseUnit = itemMasterPurchase.ItemUnit;
                                #endregion

                                #region validate convertion factor
                                Decimal factor = 1;
                                if (GCBaseUnit != cboItemUnitValue)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    string filterExpression = String.Format("ItemID = {0} AND GCAlternateUnit = '{1}'", x.ItemID, cboItemUnitValue);
                                    vItemAlternateUnit alternate = BusinessLayer.GetvItemAlternateUnitList(filterExpression, ctx).FirstOrDefault();
                                    factor = alternate.ConversionFactor;
                                    itemMasterPurchase.Price = itemMasterPurchase.Price * factor;
                                }
                                #endregion

                                x.UnitPrice = itemMasterPurchase.Price;
                                x.DraftUnitPrice = itemMasterPurchase.Price;

                                x.IsDiscountInPercentage1 = true;
                                x.DiscountPercentage1 = itemMasterPurchase.Discount;
                                x.DiscountAmount1 = Convert.ToDecimal(x.Quantity * x.UnitPrice * x.DiscountPercentage1 / 100);

                                x.IsDraftDiscountInPercentage1 = true;
                                x.DraftDiscountPercentage1 = itemMasterPurchase.Discount;
                                x.DraftDiscountAmount1 = Convert.ToDecimal(x.Quantity * x.UnitPrice * x.DiscountPercentage1 / 100);

                                x.IsDiscountInPercentage2 = true;
                                x.DiscountPercentage2 = itemMasterPurchase.Discount2;
                                x.DiscountAmount2 = Convert.ToDecimal(((x.Quantity * x.UnitPrice) - x.DiscountAmount1) * x.DiscountPercentage2 / 100);

                                x.IsDraftDiscountInPercentage2 = true;
                                x.DraftDiscountPercentage2 = itemMasterPurchase.Discount2;
                                x.DraftDiscountAmount2 = Convert.ToDecimal(((x.Quantity * x.UnitPrice) - x.DiscountAmount1) * x.DiscountPercentage2 / 100);

                                x.LineAmount = x.CustomSubTotal;

                                x.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(x);
                            }
                            else
                            {
                                #region take price
                                GetItemMasterPurchaseWithDate itemMasterPurchase = BusinessLayer.GetItemMasterPurchaseWithDateList(AppSession.UserLogin.HealthcareID, x.ItemID, supplierID, entity.OrderDate.ToString(Constant.FormatString.DATE_FORMAT_112), ctx).FirstOrDefault();
                                String cboItemUnitValue = x.GCPurchaseUnit;
                                String GCBaseUnit = itemMasterPurchase.ItemUnit;
                                #endregion

                                #region validate convertion factor
                                Decimal factor = 1;
                                if (GCBaseUnit != cboItemUnitValue)
                                {
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    string filterExpression = String.Format("ItemID = {0} AND GCAlternateUnit = '{1}'", x.ItemID, cboItemUnitValue);
                                    vItemAlternateUnit alternate = BusinessLayer.GetvItemAlternateUnitList(filterExpression, ctx).FirstOrDefault();
                                    factor = alternate.ConversionFactor;
                                    itemMasterPurchase.Price = itemMasterPurchase.Price * factor;
                                }
                                #endregion

                                x.UnitPrice = itemMasterPurchase.Price;
                                x.DraftUnitPrice = itemMasterPurchase.Price;

                                x.IsDiscountInPercentage1 = true;
                                x.DiscountPercentage1 = itemMasterPurchase.Discount;
                                x.DiscountAmount1 = Convert.ToDecimal(x.Quantity * x.UnitPrice * x.DiscountPercentage1 / 100);

                                x.IsDraftDiscountInPercentage1 = true;
                                x.DraftDiscountPercentage1 = itemMasterPurchase.Discount;
                                x.DraftDiscountAmount1 = Convert.ToDecimal(x.Quantity * x.UnitPrice * x.DiscountPercentage1 / 100);

                                x.IsDiscountInPercentage2 = true;
                                x.DiscountPercentage2 = itemMasterPurchase.Discount2;
                                x.DiscountAmount2 = Convert.ToDecimal(((x.Quantity * x.UnitPrice) - x.DiscountAmount1) * x.DiscountPercentage2 / 100);

                                x.IsDraftDiscountInPercentage2 = true;
                                x.DraftDiscountPercentage2 = itemMasterPurchase.Discount2;
                                x.DraftDiscountAmount2 = Convert.ToDecimal(((x.Quantity * x.UnitPrice) - x.DiscountAmount1) * x.DiscountPercentage2 / 100);

                                x.LineAmount = x.CustomSubTotal;

                                x.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                entityDtDao.Update(x);
                            }
                        }

                        entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDao.Update(entity);
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf Pemesanan Barang Ini Sudah Memiliki Penerimaan.";
                        ctx.RollBackTransaction();
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pemesanan barang tidak bisa diubah. Harap untuk merefresh halaman ini";
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