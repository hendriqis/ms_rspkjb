using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceive : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string filterAllowPORqtybiggerthenPOqty = "";
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected string filterExpressionItemProduct = "";
        string menuType = string.Empty;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            if (menuType == "v2")
                return Constant.MenuCode.Inventory.PURCHASE_RECEIVE_V2;
            else
                return Constant.MenuCode.Inventory.PURCHASE_RECEIVE;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
                hdnMenuType.Value = menuType;
            }

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}', '{24}')",
                                                AppSession.UserLogin.HealthcareID, //0
                                                Constant.SettingParameter.IS_CONFIRM_PURCHASE_RECEIVE, //1
                                                Constant.SettingParameter.VAT_PERCENTAGE, //2
                                                Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //3
                                                Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //4
                                                Constant.SettingParameter.IM_SALIN_PEMESANAN_BARANG_TANPA_FILTER, //5
                                                Constant.SettingParameter.IM_IS_NO_FACTUR_DONT_ALLOW_DUPLICATE, //6
                                                Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE, //7
                                                Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE, //8
                                                Constant.SettingParameter.ALLOW_POR_QTY_BIGGER_THEN_PO, //9
                                                Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM, //10
                                                Constant.SettingParameter.IM_IS_POR_DATE_ALLOW_BACKDATE, //11
                                                Constant.SettingParameter.IM_CHANGE_QTY_POR, //12
                                                Constant.SettingParameter.IM_UBAH_PPJDETAIL_POR, //13
                                                Constant.SettingParameter.IM_PENGADAAN_TAMPIL_DISCOUNT_FINAL, //14
                                                Constant.SettingParameter.IM_PENGADAAN_TAMPIL_ONGKOS_KIRIM, //15
                                                Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED, //16
                                                Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE, //17
                                                Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED, //18
                                                Constant.SettingParameter.IM0121, //19
                                                Constant.SettingParameter.IM0123, //20
                                                Constant.SettingParameter.IM0127, //21 
                                                Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT, //22
                                                Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN //23
                                                , Constant.SettingParameter.IM0135
                                            ));
            hdnIsShowDefaultLocation.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0135).FirstOrDefault().ParameterValue;
            hdnAllowPORQtyBiggerThanPO.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.ALLOW_POR_QTY_BIGGER_THEN_PO).ParameterValue;
            hdnIsAutoUpdateToSupplierItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM).ParameterValue;
            hdnIsAllowBackdatePOR.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_IS_POR_DATE_ALLOW_BACKDATE).ParameterValue;
            hdnChangeQtyPOR.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_CHANGE_QTY_POR).ParameterValue;
            hdnIsPPhDetailEdit.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_UBAH_PPJDETAIL_POR).ParameterValue;
            hdnIsPpnAllowChanged.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            hdnIsValidateVAT.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0127).FirstOrDefault().ParameterValue;
            hdnIsUsingDownPayment.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0123).FirstOrDefault().ParameterValue;
            hdnIsReceiveUsingBaseUnit.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).FirstOrDefault().ParameterValue;
            hdnKapanPerubahanNilaiHargaKeItemPlanning.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN).FirstOrDefault().ParameterValue;

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

            string tampilDiscountFinal = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PENGADAAN_TAMPIL_DISCOUNT_FINAL).FirstOrDefault().ParameterValue;
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

            string tampilOngkosKirim = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PENGADAAN_TAMPIL_ONGKOS_KIRIM).FirstOrDefault().ParameterValue;
            if (tampilOngkosKirim == "1")
            {
                trCharges.Attributes.Remove("style");
            }
            else
            {
                trCharges.Attributes.Add("style", "display:none");
            }

            hdnIsPORWithPriceInformation.Value = AppSession.IsPORWithPriceInformation;

            if (menuType == "v2" && hdnIsPORWithPriceInformation.Value == "0")
            {
                trTotalOrder.Style.Add("display", "none");
                txtPPN.Style.Add("display", "none");
                trDPReferrenceNo.Style.Add("display", "none");
                trDP.Style.Add("display", "none");
                trCharges.Style.Add("display", "none");
                trStamp.Style.Add("display", "none");
                trTotalOrderSaldo.Style.Add("display", "none");

                trPrice.Style.Add("display", "none");
                trDiscount.Style.Add("display", "none");
                trDiscount2.Style.Add("display", "none");
                trSubTotalPrice.Style.Add("display", "none");
            }

            hdnIsUsedProductLine.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).ParameterValue;

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

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsBlackList = 0 AND IsDeleted = 0 AND IsActive = 1", Constant.BusinessObjectType.SUPPLIER);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_RECEIVE);

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


            string isConfirm = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_CONFIRM_PURCHASE_RECEIVE).ParameterValue;
            if (isConfirm != "" && isConfirm != null)
            {
                hdnNeedConfirmation.Value = isConfirm;
            }
            string vatPercent = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            if (vatPercent != "" && vatPercent != null)
            {
                hdnVATPercentage.Value = vatPercent;
                hdnVATPercentageFromSetvar.Value = vatPercent;
            }
            else
            {
                hdnVATPercentage.Value = "0";
                hdnVATPercentageFromSetvar.Value = "0";
            }
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            string isDiscountToAveragePrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isDiscountToAveragePrice != "" && isDiscountToAveragePrice != null)
            {
                hdnIsDiscountAppliedToAveragePrice.Value = isDiscountToAveragePrice;
            }
            string isDiscountToUnitPrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue;
            if (isDiscountToUnitPrice != "" && isDiscountToUnitPrice != null)
            {
                hdnIsDiscountAppliedToUnitPrice.Value = isDiscountToUnitPrice;
            }
            string salinTanpaFilter = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_SALIN_PEMESANAN_BARANG_TANPA_FILTER).ParameterValue;
            if (salinTanpaFilter != "" && salinTanpaFilter != null)
            {
                hdnIsFilterPurchaseOrderNo.Value = salinTanpaFilter;
            }
            string isDontAllowDuplicateFactur = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_IS_NO_FACTUR_DONT_ALLOW_DUPLICATE).ParameterValue;
            if (isDontAllowDuplicateFactur != "" && isDontAllowDuplicateFactur != null)
            {
                hndIsDontAllowDuplicateFacturNo.Value = isDontAllowDuplicateFactur;
            }
            string isPPNToAveragePrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            if (isPPNToAveragePrice != "" && isPPNToAveragePrice != null)
            {
                hdnIsPPNAppliedToAveragePrice.Value = isPPNToAveragePrice;
            }
            string isPPNToUnitPrice = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE).ParameterValue;
            if (isPPNToUnitPrice != "" && isPPNToUnitPrice != null)
            {
                hdnIsPPNAppliedToUnitPrice.Value = isPPNToUnitPrice;
            }

            hdnIsUsingPurchaseDiscountShared.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED).FirstOrDefault().ParameterValue;
            hdnDefaultValueForNoFakturPajak.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM0121).FirstOrDefault().ParameterValue;
            var test = hdnPRID.Value;

            cboPPHOptions.Items.Add("Plus");
            cboPPHOptions.Items.Add("Minus");
            cboPPHOptions.SelectedIndex = 0;

            cboPPHOptionsDetail.Items.Add("Plus");
            cboPPHOptionsDetail.Items.Add("Minus");
            cboPPHOptionsDetail.SelectedIndex = 0;

            SetControlProperties();
            decimal tempTransactionAmount = -1;
            decimal tempPPHAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempPPHAmount);
            Helper.SetControlEntrySetting(txtQuantity, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected string GetVATPercentage()
        {
            return hdnVATPercentage.Value;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.CHARGES_TYPE, Constant.StandardCode.PPH_TYPE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"), 50, 1, "TermDay");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            Methods.SetComboBoxField<StandardCode>(cboPPhType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PPH_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPPhTypeDetail, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.PPH_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");

            cboCurrency.SelectedIndex = 0;
            cboTerm.SelectedIndex = 1;
            chkPPN.Checked = true;
            if (hdnIsPpnAllowChanged.Value == "1")
            {
                txtVATPercentageDefault.ReadOnly = false;
            }
            else
            {
                txtVATPercentageDefault.ReadOnly = true;
            }
            cboPPhType.SelectedIndex = 0;
            cboPPhTypeDetail.SelectedIndex = 0;

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
            }

            if (hdnIsShowDefaultLocation.Value == "1")
            {
                hdnLocationID.Value = locationID;
                txtLocationCode.Text = locationCode;
                txtLocationName.Text = locationName;
            }
            else
            {
                hdnLocationID.Value = "";
                txtLocationCode.Text = "";
                txtLocationName.Text = "";
            }
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPRID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPurchaseReceiveNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(true, true, false, ""));
            SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            if (hdnIsAllowBackdatePOR.Value != "1")
            {
                SetControlEntrySetting(txtPurchaseReceiveDate, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }
            else
            {
                SetControlEntrySetting(txtPurchaseReceiveDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            }

            SetControlEntrySetting(txtPurchaseReceiveTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtFacturNo, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDateReferrence, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, true, true)); // per patch 202006-02 cboTerm ini boleh diedit, karna kebutuhan RSDOSOBA ada copas Term dari PO, tapi di POR nya masih boleh diubah (by RN)
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, true, true, "1.00"));
            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(cboPPhType, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(cboPPHOptions, new ControlEntrySetting(false, true, false));
            SetControlEntrySetting(chkPPHPercent, new ControlEntrySetting(false, true, false, false));
            SetControlEntrySetting(txtPPH, new ControlEntrySetting(false, true, true, "0.00"));
            SetControlEntrySetting(txtPPHPI, new ControlEntrySetting(false, false, false, "0.00"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtDP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtDPReferrenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCharges, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtStamp, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtTotalOrderSaldo, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtRemarksDetail, new ControlEntrySetting(true, true, false));

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

            SetControlEntrySetting(txtPaymentDueDate, new ControlEntrySetting(true, true, false));

            #region Detail
            if (hdnIsPPhDetailEdit.Value == "0")
            {
                SetControlEntrySetting(cboPPhTypeDetail, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(cboPPHOptionsDetail, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPHPercentDetail, new ControlEntrySetting(false, false, false, false));
                SetControlEntrySetting(txtPPHPercentageDetail, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPPHAmountDetail, new ControlEntrySetting(false, false, false));
            }
            #endregion
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReceiveDt entity = e.Row.DataItem as vPurchaseReceiveDt;

                CheckBox chkIsBonus = e.Row.FindControl("chkIsBonus") as CheckBox;
                CheckBox chkIsDiscInPct1 = e.Row.FindControl("chkIsDiscInPct1") as CheckBox;
                CheckBox chkIsDiscInPct2 = e.Row.FindControl("chkIsDiscInPct2") as CheckBox;

                chkIsBonus.Checked = entity.IsBonusItem;
                chkIsDiscInPct1.Checked = entity.IsDiscountInPercentage1;
                chkIsDiscInPct2.Checked = entity.IsDiscountInPercentage2;

                if (hdnMenuType.Value == "v2" && hdnIsPORWithPriceInformation.Value == "0")
                {
                    HtmlGenericControl lblCustomUnitPrice = e.Row.FindControl("lblCustomUnitPrice") as HtmlGenericControl;
                    HtmlGenericControl lblDiscountPercentage1 = e.Row.FindControl("lblDiscountPercentage1") as HtmlGenericControl;
                    HtmlGenericControl lblDiscountAmount1 = e.Row.FindControl("lblDiscountAmount1") as HtmlGenericControl;
                    HtmlGenericControl lblDiscountPercentage2 = e.Row.FindControl("lblDiscountPercentage2") as HtmlGenericControl;
                    HtmlGenericControl lblDiscountAmount2 = e.Row.FindControl("lblDiscountAmount2") as HtmlGenericControl;
                    HtmlGenericControl lblCustomSubTotal = e.Row.FindControl("lblCustomSubTotal") as HtmlGenericControl;
                    HtmlGenericControl lblPPHAmount = e.Row.FindControl("lblPPHAmount") as HtmlGenericControl;

                    lblCustomUnitPrice.Visible = false;
                    lblDiscountPercentage1.Visible = false;
                    lblDiscountAmount1.Visible = false;
                    lblDiscountPercentage2.Visible = false;
                    lblDiscountAmount2.Visible = false;
                    lblCustomSubTotal.Visible = false;
                    lblPPHAmount.Visible = false;
                }
            }
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnVATPercentage.Value = hdnVATPercentageFromSetvar.Value;
            txtVATPercentageDefault.Text = hdnVATPercentageFromSetvar.Value;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        #region Load Entity
        protected string GetFilterExpression()
        {
            string filterExpression = hdnRecordFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionCode = '{0}'", Constant.TransactionCode.PURCHASE_RECEIVE);
            return filterExpression;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvPurchaseReceiveHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vPurchaseReceiveHd entity = BusinessLayer.GetvPurchaseReceiveHd(filterExpression, PageIndex, "PurchaseReceiveID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvPurchaseReceiveHdRowIndex(filterExpression, keyValue, "PurchaseReceiveID DESC");
            vPurchaseReceiveHd entity = BusinessLayer.GetvPurchaseReceiveHd(filterExpression, PageIndex, "PurchaseReceiveID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vPurchaseReceiveHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
                SetControlEntrySetting(txtFacturNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtDateReferrence, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(cboTerm, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtPaymentDueDate, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtRemarksDetail, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtDPReferrenceNo, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtDP, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtCharges, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtStamp, new ControlEntrySetting(true, false, false));

                cboTerm.Enabled = false;

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

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;

            hdnPRID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReceiveDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseReceiveTime.Text = entity.ReceivedTime;
            txtDateReferrence.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnReferenceDate.Value = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.PaymentDueDate != null)
            {
                trPaymentDueDate.Attributes.CssStyle.Remove("display");
                txtPaymentDueDate.Text = entity.PaymentDueDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            hdnSupplierID.Value = entity.SupplierID.ToString();
            txtSupplierCode.Text = entity.SupplierCode;
            txtSupplierName.Text = entity.SupplierName;
            txtFacturNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtCharges.Text = entity.ChargesAmount.ToString();
            txtStamp.Text = entity.StampAmount.ToString();
            txtDPReferrenceNo.Text = entity.DownPaymentReferenceNo;
            txtDP.Text = entity.DownPaymentAmount.ToString();
            cboTerm.Value = entity.TermID.ToString();
            txtNotes.Text = entity.Remarks;
            cboCurrency.Value = entity.GCCurrencyCode.ToString();
            txtKurs.Text = entity.CurrencyRate.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            if (chkPPN.Checked)
            {
                trTaxInvoice.Attributes.CssStyle.Remove("display");

                hdnVATPercentage.Value = entity.VATPercentage.ToString();
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

            txtTaxInvoiceNo.Text = entity.TaxInvoiceNo;
            txtTaxInvoiceDate.Text = entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTotalOrder.Text = (entity.TransactionAmount - entity.DiscountAmount).ToString();
            txtFinalDiscount.Text = entity.FinalDiscount.ToString();

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            if (entity.ApprovedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");

                divApprovedBy.InnerHtml = "";
                divApprovedDate.InnerHtml = "";
            }
            else
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");

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

            chkPPHPercent.Checked = entity.IsPPHInPercentage;
            cboPPhType.Value = entity.GCPPhType;
            if (entity.PPHMode == true)
            {
                cboPPHOptions.Text = "Plus";
            }
            else
            {
                cboPPHOptions.Text = "Minus";
            }

            txtPPH.Text = entity.PPHPercentage.ToString();
            txtPPHPI.Text = entity.PPHAmount.ToString();

            decimal tempTransactionAmount = -1, tempPPHAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount, ref tempPPHAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount, ref decimal pphAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPRID.Value != "")
            {
                filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (transactionAmount > -1)
            {
                PurchaseReceiveHd entityHd = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPRID.Value));
                transactionAmount = entityHd.TransactionAmount - entityHd.DiscountAmount;
                pphAmount = entityHd.PPHAmount;
            }

            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");

            String POID = "";
            foreach (vPurchaseReceiveDt p in lstEntity)
            {
                if (string.IsNullOrEmpty(POID))
                {
                    POID = p.PurchaseOrderID.ToString();
                }
                else
                {
                    POID += ", " + p.PurchaseOrderID.ToString();
                }
            }

            if (POID != "")
            {
                string filterExpressionPO = string.Format("PurchaseOrderID IN ({0})", POID);
                List<PurchaseOrderHd> lstEntityPO = BusinessLayer.GetPurchaseOrderHdList(filterExpressionPO);

                Int32 includeVAT = 0;
                foreach (PurchaseOrderHd po in lstEntityPO)
                {
                    if (po.IsIncludeVAT == true)
                    {
                        includeVAT += includeVAT + 1;
                    }
                }

                if (includeVAT > 0)
                {
                    hdnIsVATMandatory.Value = "1";
                    if (!chkPPN.Checked)
                    {
                        trPPN.Style.Remove("display");
                    }
                }
            }

            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntityHd(PurchaseReceiveHd entityHd)
        {
            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityHd.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entityHd.TermID = Convert.ToInt32(cboTerm.Value.ToString());
            entityHd.ReferenceNo = txtFacturNo.Text;
            entityHd.ReferenceDate = Helper.GetDatePickerValue(txtDateReferrence.Text);

            string setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE).ParameterValue;
            if (setvar == "2")
            {
                if (txtDateReferrence.Text != hdnReferenceDate.Value)
                {
                    int termDay = BusinessLayer.GetTerm(entityHd.TermID).TermDay;
                    entityHd.PaymentDueDate = Helper.GetDatePickerValue(txtDateReferrence.Text).AddDays(termDay);
                }
                else
                {
                    entityHd.PaymentDueDate = Helper.GetDatePickerValue(txtPaymentDueDate.Text);
                }
            }
            else
            {
                entityHd.PaymentDueDate = Helper.GetDatePickerValue(txtPaymentDueDate.Text);
            }

            entityHd.GCCurrencyCode = cboCurrency.Value.ToString();
            entityHd.CurrencyRate = Convert.ToDecimal(txtKurs.Text);
            entityHd.IsIncludeVAT = chkPPN.Checked;
            if (chkPPN.Checked)
            {
                entityHd.TaxInvoiceDate = Helper.GetDatePickerValue(txtPurchaseReceiveDate.Text);
                entityHd.TaxInvoiceNo = txtTaxInvoiceNo.Text;
            }
            else
            {
                entityHd.TaxInvoiceDate = null;
                entityHd.TaxInvoiceNo = string.Empty;
            }
            if (entityHd.IsIncludeVAT)
            {
                entityHd.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
            }
            else
            {
                entityHd.VATPercentage = 0;
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }

            //entityHd.GCPPhType = cboPPhType.Value.ToString();
            //if (chkPPHPercent.Checked)
            //{
            //    entityHd.IsPPHInPercentage = true;
            //}
            //else
            //{
            //    entityHd.IsPPHInPercentage = false;
            //}

            //entityHd.PPHPercentage = Convert.ToDecimal(txtPPH.Text);
            //entityHd.PPHAmount = Convert.ToDecimal(txtPPHPI.Text);

            //if (cboPPHOptions.Text.Equals("Plus"))
            //{
            //    entityHd.PPHMode = true;
            //}
            //else
            //{
            //    entityHd.PPHMode = false;
            //}

            entityHd.Remarks = txtNotes.Text;
            entityHd.ChargesAmount = Convert.ToDecimal(txtCharges.Text);
            entityHd.StampAmount = Convert.ToDecimal(txtStamp.Text);
            if (Convert.ToDecimal(txtCharges.Text) != 0 && Convert.ToDecimal(txtStamp.Text) == 0)
            {
                entityHd.GCChargesType = Constant.ChargesType.ONGKOS_KIRIM;
            }
            else if (Convert.ToDecimal(txtCharges.Text) == 0 && Convert.ToDecimal(txtStamp.Text) != 0)
            {
                entityHd.GCChargesType = Constant.ChargesType.MATERAI;
            }
            else
            {
                entityHd.GCChargesType = Constant.ChargesType.LAIN_LAIN;
            }

            entityHd.DownPaymentReferenceNo = txtDPReferrenceNo.Text;
            //entityHd.FinalDiscount = Convert.ToDecimal(Request.Form[txtFinalDiscount.UniqueID]);
            entityHd.FinalDiscount = Convert.ToDecimal(txtFinalDiscount.Text);
            entityHd.DownPaymentAmount = Convert.ToDecimal(txtDP.Text);
            entityHd.NetTransactionAmount = ((entityHd.TransactionAmount - entityHd.DiscountAmount - entityHd.FinalDiscount) * (100 + entityHd.VATPercentage) / 100) + entityHd.StampAmount + entityHd.ChargesAmount - entityHd.DownPaymentAmount + entityHd.PPHAmount;
        }

        public void SetTotalDPText(string TotalDP)
        {
            txtDP.Text = TotalDP;
        }

        public void GetPurchaseReceiveHdID(ref int PRID)
        {
            if (hdnPRID.Value == "0")
            {
                PRID = 0;
            }
            else
            {
                PRID = Convert.ToInt32(hdnPRID.Value);
            }
        }

        public void SavePurchaseReceiveHd(IDbContext ctx, ref int PRID, ref string PRNo, int oTermID, string hdnTotalDP = "", string hdnTotalCharges = "")
        {
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            TermDao termDao = new TermDao(ctx);

            if (hdnPRID.Value == "0")
            {
                PurchaseReceiveHd entityHd = new PurchaseReceiveHd();
                ControlToEntityHd(entityHd);

                if (oTermID != 0)
                {
                    entityHd.TermID = oTermID;
                }

                if (hdnTotalDP != "")
                {
                    entityHd.DownPaymentAmount = Convert.ToDecimal(hdnTotalDP);
                }

                if (hdnTotalCharges != "")
                {
                    entityHd.ChargesAmount = Convert.ToDecimal(hdnTotalCharges);
                }

                entityHd.IsIncludeVAT = chkPPN.Checked;

                if (entityHd.IsIncludeVAT)
                {
                    entityHd.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
                }
                else
                {
                    entityHd.VATPercentage = 0;
                }

                entityHd.DiscountAmount = 0;

                entityHd.ReceivedDate = Helper.GetDatePickerValue(txtPurchaseReceiveDate.Text);
                entityHd.ReceivedTime = txtPurchaseReceiveTime.Text;

                int termDay = termDao.Get(entityHd.TermID).TermDay;
                string setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE).ParameterValue;
                if (setvar == "2")
                {
                    entityHd.PaymentDueDate = entityHd.ReferenceDate.AddDays(termDay);
                }
                else
                {
                    entityHd.PaymentDueDate = entityHd.ReceivedDate.AddDays(termDay);
                }

                entityHd.TransactionCode = Constant.TransactionCode.PURCHASE_RECEIVE;
                entityHd.PurchaseReceiveNo = BusinessLayer.GenerateTransactionNo(entityHd.TransactionCode, entityHd.ReceivedDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                PRID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);

                PRNo = entityHd.PurchaseReceiveNo;
            }
            else
            {
                PRID = Convert.ToInt32(hdnPRID.Value);
                PRNo = Request.Params[txtPurchaseReceiveNo.UniqueID];
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int PRID = 0;
                string purchaseReceiveNo = "";
                int oTermID = 0;

                SavePurchaseReceiveHd(ctx, ref PRID, ref purchaseReceiveNo, oTermID);
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
                PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entity);

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseReceiveHd(entity);
                }
                else
                {
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseOrderHdDao purchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestPODao purchaseRequestPODao = new PurchaseRequestPODao(ctx);
            PurchaseReceivePODao purchaseReceivePODao = new PurchaseReceivePODao(ctx);
            PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);
            TermDao termDao = new TermDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            ItemMasterDao iMasterDao = new ItemMasterDao(ctx);
            ItemGroupMasterDao iGroupMasterDao = new ItemGroupMasterDao(ctx);

            try
            {
                String filterExpression = "";
                if (hdnNeedConfirmation.Value == "1")
                {
                    filterExpression = string.Format("PurchaseReceiveID = {0} AND IsNeedConfirmation =  1 AND GCItemDetailStatus NOT IN ('{1}','{2}')", hdnPRID.Value, Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED);
                    List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, ctx);
                    if (lstEntity.Count > 0)
                    {
                        errMessage = "Approval Penerimaan Pembelian tidak bisa dilakukan karena masih ada item yang harus dikonfirmasi!";
                        return false;
                    }
                }

                PurchaseReceiveHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    decimal termAmount = 0;

                    ControlToEntityHd(entity);

                    List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", entity.PurchaseReceiveID, Constant.TransactionStatus.VOID), ctx);

                    string lstItemID = "";
                    string lstID = "";

                    foreach (PurchaseReceiveDt obj in lstPurchaseReceiveDt)
                    {
                        if (lstID != "")
                        {
                            lstID += ",";
                        }
                        lstID += obj.ID.ToString();

                        if (lstItemID != "")
                        {
                            lstItemID += ",";
                        }
                        lstItemID += obj.ItemID.ToString();
                    }

                    filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                    List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterExpression, ctx);
                    filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                    List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, ctx);
                    filterExpression = String.Format("BusinessPartnerID = {0} AND ItemID IN ({1}) AND IsDeleted = 0", entity.BusinessPartnerID, lstItemID);
                    List<SupplierItem> lstSupplierItem = BusinessLayer.GetSupplierItemList(filterExpression, ctx);
                    filterExpression = String.Format("ID IN ({0})", lstID);
                    List<PurchaseReceiveDtExpired> lstExpiredDate = BusinessLayer.GetPurchaseReceiveDtExpiredList(filterExpression, ctx);

                    List<int> tempLstItemID = new List<int>();
                    List<classAveragePerItem> classAverageList = new List<classAveragePerItem>();
                    foreach (PurchaseReceiveDt purchaseDt in lstPurchaseReceiveDt)
                    {
                        purchaseDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;

                        #region Update Item Planning
                        ItemPlanning entityItemPlanning = lstItemPlanning.Where(x => x.ItemID == purchaseDt.ItemID).FirstOrDefault();

                        decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                        decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                        decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                        bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                        bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                        PurchaseReceiveDtExpired entityExpiredDate = lstExpiredDate.Where(x => x.ID == purchaseDt.ID).FirstOrDefault();

                        decimal unitPrice = purchaseDt.UnitPrice;
                        decimal unitPriceTemp = purchaseDt.UnitPrice;
                        decimal discountAmount1 = purchaseDt.DiscountAmount1;
                        decimal discountAmount2 = purchaseDt.DiscountAmount2;

                        ItemProduct entityItemProduct = iProductDao.Get(purchaseDt.ItemID);
                        ItemMaster entityItem = iMasterDao.Get(purchaseDt.ItemID);
                        ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                        if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                        {
                            if (entityItemGroup.IsDiscountCalculateHNA == true)
                            {
                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                {
                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / purchaseDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                }
                                else
                                {
                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
                                }
                            }
                        }
                        else
                        {
                            if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                            {
                                if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                {
                                    unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / purchaseDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                }
                                else
                                {
                                    unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
                                }
                            }
                        }

                        if (entityItemProduct.IsPPNCalculateHNAFromItemGroupMaster == true)
                        {
                            if (entityItemGroup.IsPPNCalculateHNA == true)
                            {
                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                unitPrice = unitPrice + ppnAmountUnitPrice;
                            }
                        }
                        else
                        {
                            if (hdnIsPPNAppliedToUnitPrice.Value == "1")
                            {
                                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                unitPrice = unitPrice + ppnAmountUnitPrice;
                            }
                        }

                        decimal tempAmount = unitPriceTemp;
                        if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                        {
                            tempAmount = ((tempAmount * purchaseDt.Quantity) - (discountAmount1 + discountAmount2));
                        }
                        else
                        {
                            tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity);
                        }

                        if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                        {
                            decimal ppnAmount = tempAmount * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                            tempAmount = tempAmount + ppnAmount;
                        }
                        else
                        {
                            if (hdnIsDiscountAppliedToAveragePrice.Value != "1")
                            {
                                tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity);
                            }
                        }

                        decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == purchaseDt.ItemID).Sum(p => p.QuantityEND);

                        if (!purchaseDt.IsBonusItem) // [QISMS2-595] utk yg item bonus tidak boleh update AveragePrice di ItemPlanning
                        {
                            List<PurchaseReceiveDt> lstCheckItemOthersID = lstPurchaseReceiveDt.Where(a => a.ItemID == purchaseDt.ItemID && a.ID != purchaseDt.ID).ToList();
                            if (lstCheckItemOthersID.Count() > 0)
                            {
                                decimal tempQtyCek = 0, tempAmountCek = 0, tempAmountSum = 0;
                                foreach (PurchaseReceiveDt porDt in lstPurchaseReceiveDt.Where(a => a.ItemID == purchaseDt.ItemID).ToList())
                                {
                                    decimal unitPriceCek = porDt.UnitPrice;
                                    decimal discountAmount1Cek = porDt.DiscountAmount1;
                                    decimal discountAmount2Cek = porDt.DiscountAmount2;

                                    tempAmountCek = porDt.UnitPrice;

                                    if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                    {
                                        tempAmountCek = ((tempAmountCek * porDt.Quantity) - (discountAmount1Cek + discountAmount2Cek));
                                    }
                                    else
                                    {
                                        tempAmountCek = (porDt.UnitPrice * porDt.Quantity);
                                    }

                                    if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                    {
                                        decimal ppnAmountCek = tempAmountCek * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                        tempAmountCek = tempAmountCek + ppnAmountCek;
                                    }
                                    else
                                    {
                                        if (hdnIsDiscountAppliedToAveragePrice.Value != "1")
                                        {
                                            tempAmountCek = (porDt.UnitPrice * porDt.Quantity);
                                        }
                                    }

                                    tempAmountSum += tempAmountCek;
                                    tempQtyCek += (porDt.Quantity * porDt.ConversionFactor);
                                }

                                if (classAverageList.Where(a => a.ItemID == purchaseDt.ItemID).ToList().Count() == 0)
                                {
                                    decimal tempQty = (qtyEnd + tempQtyCek);
                                    if (tempQty > 0)
                                    {
                                        decimal averagePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmountSum)) / tempQty;
                                        entityItemPlanning.AveragePrice = Math.Round(averagePrice, 2);

                                        classAveragePerItem classAverage = new classAveragePerItem();
                                        classAverage.ItemID = purchaseDt.ItemID;
                                        classAverage.NewAveragePrice = Math.Round(averagePrice, 2);
                                        classAverageList.Add(classAverage);
                                    }
                                }
                                else
                                {
                                    entityItemPlanning.AveragePrice = classAverageList.Where(a => a.ItemID == purchaseDt.ItemID).FirstOrDefault().NewAveragePrice;
                                }

                            }
                            else
                            {
                                decimal tempQty = (qtyEnd + (purchaseDt.Quantity * purchaseDt.ConversionFactor));
                                if (tempQty > 0)
                                {
                                    decimal averagePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;
                                    entityItemPlanning.AveragePrice = Math.Round(averagePrice, 2);
                                }
                            }
                        }

                        decimal unitPriceItemUnit = unitPrice / purchaseDt.ConversionFactor;

                        entityItemPlanning.BusinessPartnerID = entity.BusinessPartnerID;
                        entityItemPlanning.LastBusinessPartnerID = entity.BusinessPartnerID;
                        entityItemPlanning.LastPurchasePrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                        entityItemPlanning.LastConversionFactor = purchaseDt.ConversionFactor;
                        entityItemPlanning.LastPurchaseDiscount = purchaseDt.DiscountPercentage1;
                        entityItemPlanning.LastPurchaseDiscount2 = purchaseDt.DiscountPercentage2;

                        if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                        {
                            if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                            {
                                entityItemPlanning.UnitPrice = unitPriceItemUnit;
                            }
                            else
                            {
                                purchaseDt.TempUnitPrice = unitPriceItemUnit;
                            }

                            if (hdnIsReceiveUsingBaseUnit.Value == "1")
                            {
                                entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                {
                                    entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.UnitPrice;
                                }
                                else
                                {
                                    purchaseDt.TempPurchaseUnitPrice = purchaseDt.TempUnitPrice;
                                }
                                entityItemPlanning.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                            }
                            else
                            {
                                entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                {
                                    entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.UnitPrice * purchaseDt.ConversionFactor;
                                }
                                else
                                {
                                    purchaseDt.TempPurchaseUnitPrice = purchaseDt.TempUnitPrice * purchaseDt.ConversionFactor;
                                }
                                entityItemPlanning.GCPurchaseUnit = purchaseDt.GCItemUnit;
                            }
                        }
                        else
                        {
                            if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                            {
                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                {
                                    entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                }
                                else
                                {
                                    purchaseDt.TempUnitPrice = unitPriceItemUnit;
                                }

                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                    {
                                        entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.UnitPrice;
                                    }
                                    else
                                    {
                                        purchaseDt.TempPurchaseUnitPrice = purchaseDt.TempUnitPrice;
                                    }
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                    {
                                        entityItemPlanning.PurchaseUnitPrice = entityItemPlanning.UnitPrice * purchaseDt.ConversionFactor;
                                    }
                                    else
                                    {
                                        purchaseDt.TempPurchaseUnitPrice = purchaseDt.TempUnitPrice * purchaseDt.ConversionFactor;
                                    }
                                    entityItemPlanning.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                }
                            }
                            else
                            {
                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                }
                            }
                        }

                        if (entityExpiredDate != null)
                        {
                            entityItemPlanning.LastPurchaseBatchNo = entityExpiredDate.BatchNumber;
                            entityItemPlanning.LastPurchaseExpiredDate = entityExpiredDate.ExpiredDate;
                        }
                        entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                        entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                        itemPlanningDao.Update(entityItemPlanning);
                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("POR", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                        #endregion

                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);

                        if (hdnIsAutoUpdateToSupplierItem.Value == "1")
                        {
                            #region Update Supplier Item
                            if (!(tempLstItemID.IndexOf(purchaseDt.ItemID) != -1))
                            {
                                SupplierItem entitySuppItem = null;

                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                {
                                    entitySuppItem = lstSupplierItem.Where(x => x.ItemID == purchaseDt.ItemID && x.GCPurchaseUnit == purchaseDt.GCBaseUnit && x.BusinessPartnerID == entity.BusinessPartnerID).FirstOrDefault();
                                }
                                else
                                {
                                    entitySuppItem = lstSupplierItem.Where(x => x.ItemID == purchaseDt.ItemID && x.GCPurchaseUnit == purchaseDt.GCItemUnit && x.BusinessPartnerID == entity.BusinessPartnerID).FirstOrDefault();
                                }

                                if (entitySuppItem != null)
                                {
                                    entitySuppItem.ConversionFactor = purchaseDt.ConversionFactor;
                                    entitySuppItem.Price = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                    entitySuppItem.DiscountPercentage = purchaseDt.DiscountPercentage1;
                                    entitySuppItem.DiscountPercentage2 = purchaseDt.DiscountPercentage2;

                                    if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                    {
                                        entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                        entitySuppItem.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                    }
                                    else
                                    {
                                        entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                        entitySuppItem.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                    }

                                    entitySuppItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    supplierItemDao.Update(entitySuppItem);
                                }
                                else
                                {
                                    if (!purchaseDt.IsBonusItem)
                                    {
                                        entitySuppItem = new SupplierItem();
                                        entitySuppItem.BusinessPartnerID = entity.BusinessPartnerID;
                                        entitySuppItem.ItemID = purchaseDt.ItemID;
                                        entitySuppItem.Price = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                        entitySuppItem.DiscountPercentage = purchaseDt.DiscountPercentage1;
                                        entitySuppItem.DiscountPercentage2 = purchaseDt.DiscountPercentage2;
                                        entitySuppItem.ConversionFactor = purchaseDt.ConversionFactor;

                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                            entitySuppItem.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entitySuppItem.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                            entitySuppItem.GCPurchaseUnit = purchaseDt.GCItemUnit;
                                        }

                                        entitySuppItem.CreatedBy = AppSession.UserLogin.UserID;
                                        entitySuppItem.CreatedDate = DateTime.Now;
                                        supplierItemDao.Insert(entitySuppItem);
                                    }
                                }
                                tempLstItemID.Add(purchaseDt.ItemID);
                            }
                            #endregion
                        }

                        if (purchaseDt.PurchaseOrderID != null)
                        {
                            PurchaseReceivePO entityPRPO = new PurchaseReceivePO();
                            entityPRPO.PurchaseOrderID = (int)purchaseDt.PurchaseOrderID;
                            entityPRPO.PurchaseReceiveID = entity.PurchaseReceiveID;
                            entityPRPO.ItemID = purchaseDt.ItemID;
                            entityPRPO.ReceivedQuantity = purchaseDt.Quantity;
                            purchaseReceivePODao.Insert(entityPRPO);

                            PurchaseOrderDt poDt = BusinessLayer.GetPurchaseOrderDtList(string.Format("PurchaseOrderID = {0} AND ItemID = {1} AND GCItemDetailStatus != '{2}'", purchaseDt.PurchaseOrderID, purchaseDt.ItemID, Constant.TransactionStatus.VOID), ctx).FirstOrDefault();
                            if (poDt != null)
                            {
                                decimal receivedQty = purchaseDt.Quantity;
                                List<PurchaseRequestPO> lstPurchaseRequestPO = BusinessLayer.GetPurchaseRequestPOList(string.Format("PurchaseOrderID = {0} AND ItemID = {1}", purchaseDt.PurchaseOrderID, purchaseDt.ItemID), ctx);
                                foreach (PurchaseRequestPO purchaseRequestPO in lstPurchaseRequestPO)
                                {
                                    decimal tempReceivedQuantity = receivedQty;
                                    decimal completeQuantity = purchaseRequestPO.OrderQuantity - purchaseRequestPO.ReceivedQuantity;
                                    if (tempReceivedQuantity > completeQuantity)
                                        tempReceivedQuantity = completeQuantity;
                                    purchaseRequestPO.ReceivedQuantity += tempReceivedQuantity;
                                    purchaseRequestPODao.Update(purchaseRequestPO);
                                    receivedQty -= tempReceivedQuantity;
                                }

                                if (poDt.Quantity == poDt.ReceivedQuantity)
                                {
                                    poDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                    poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    purchaseOrderDtDao.Update(poDt);
                                }
                            }

                            int count = BusinessLayer.GetPurchaseOrderDtRowCount(string.Format("PurchaseOrderID = {0} AND Quantity > ReceivedQuantity AND IsDeleted = 0", purchaseDt.PurchaseOrderID), ctx);
                            if (count < 1)
                            {
                                PurchaseOrderHd entityPOHd = purchaseOrderHdDao.Get((int)purchaseDt.PurchaseOrderID);
                                entityPOHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                entityPOHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                purchaseOrderHdDao.Update(entityPOHd);

                                string filterPOTerm = string.Format("PurchaseOrderID = {0} AND GCTransactionStatus = '{1}'", entityPOHd.PurchaseOrderID, Constant.TransactionStatus.APPROVED);
                                List<PurchaseOrderTerm> lstPOTerm = BusinessLayer.GetPurchaseOrderTermList(filterPOTerm, ctx);
                                foreach (PurchaseOrderTerm poTerm in lstPOTerm)
                                {
                                    termAmount += poTerm.TermAmount;
                                }
                            }
                        }
                    }

                    entity.TotalTerminAmount = termAmount;
                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.ApprovedBy = AppSession.UserLogin.UserID;
                    entity.ApprovedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entity.LastUpdatedDate = DateTime.Now;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
                if (hdnNeedConfirmation.Value == "1")
                {
                    bool flag = true;
                    String filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
                    List<vPurchaseOrderDtOutStanding> lstEntity = BusinessLayer.GetvPurchaseOrderDtOutStandingList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (vPurchaseOrderDtOutStanding temp in lstEntity)
                        {
                            if (temp.GCItemDetailStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                flag = false;
                                break;
                            }
                        }
                    }

                    if (!flag)
                    {
                        errMessage = "Anda Tidak Bisa Melakukan Propose Karena Butuh Konfirmasi Item yang Tidak Sesuai";
                        return false;
                    }
                }

                PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<PurchaseReceiveDt> lstEntity = BusinessLayer.GetPurchaseReceiveDtList(string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", entity.PurchaseReceiveID, Constant.TransactionStatus.VOID));
                    foreach (PurchaseReceiveDt receiveDt in lstEntity)
                    {
                        receiveDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        receiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdatePurchaseReceiveDt(receiveDt);
                    }

                    ControlToEntityHd(entity);

                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseReceiveHd(entity);
                }
                else
                {
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderHdDao entityPOHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);

            try
            {
                PurchaseReceiveHd entity = entityHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    List<PurchaseReceiveDt> lstEntity = BusinessLayer.GetPurchaseReceiveDtList(string.Format("PurchaseReceiveID = {0}", hdnPRID.Value));
                    foreach (PurchaseReceiveDt receiveDt in lstEntity)
                    {
                        if (receiveDt.PurchaseOrderDtID != null)
                        {
                            if (receiveDt.GCItemDetailStatus != Constant.TransactionStatus.VOID)
                            {
                                PurchaseOrderDt poDt = BusinessLayer.GetPurchaseOrderDtList(string.Format("ID = {0}", receiveDt.PurchaseOrderDtID), ctx).FirstOrDefault();

                                if (poDt.GCPurchaseUnit == receiveDt.GCItemUnit)
                                {
                                    string receivedInfo = poDt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                    {
                                        poDt.ReceivedInformation = receivedInfo;
                                    }
                                    else
                                    {
                                        poDt.ReceivedInformation = null;
                                    }

                                    //if ((poDt.ReceivedQuantity - poDt.Quantity) >= 0)
                                    //{
                                    //    poDt.ReceivedQuantity -= receiveDt.Quantity;
                                    //}

                                    poDt.ReceivedQuantity -= receiveDt.Quantity;
                                    poDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                    poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPODtDao.Update(poDt);
                                }
                                else
                                {
                                    decimal receiveQtyCust = 0;
                                    if (poDt.ConversionFactor < receiveDt.ConversionFactor)
                                    {
                                        receiveQtyCust = receiveDt.Quantity * receiveDt.ConversionFactor;
                                    }
                                    else
                                    {
                                        receiveQtyCust = receiveDt.Quantity / poDt.ConversionFactor;
                                    }

                                    if (receiveQtyCust == receiveDt.Quantity)
                                    {
                                        string receivedInfo = poDt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                        if (receivedInfo != "|" && receivedInfo != string.Empty)
                                        {
                                            poDt.ReceivedInformation = receivedInfo;
                                        }
                                        else
                                        {
                                            poDt.ReceivedInformation = null;
                                        }

                                        poDt.ReceivedQuantity -= receiveDt.Quantity;
                                        poDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPODtDao.Update(poDt);
                                    }
                                    else
                                    {
                                        string receivedInfo = poDt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                        if (receivedInfo != "|" && receivedInfo != string.Empty)
                                        {
                                            poDt.ReceivedInformation = receivedInfo;
                                        }
                                        else
                                        {
                                            poDt.ReceivedInformation = null;
                                        }

                                        poDt.ReceivedQuantity -= receiveQtyCust;
                                        poDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                                        poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        entityPODtDao.Update(poDt);
                                    }
                                }

                                PurchaseOrderHd poHd = BusinessLayer.GetPurchaseOrderHdList(string.Format("PurchaseOrderID = {0}", receiveDt.PurchaseOrderID), ctx).FirstOrDefault();

                                poHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                                poHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPOHdDao.Update(poHd);
                            }

                            receiveDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                            receiveDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityDtDao.Update(receiveDt);
                        }
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);

                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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

        #region Trigger Callback
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
            decimal pphAmount = 0;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    transactionAmount = -1;
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount, ref transactionAmount, ref pphAmount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref pageCount, ref transactionAmount, ref pphAmount);
                    result = string.Format("refresh|{0}|{1}|{2}", pageCount, transactionAmount, pphAmount);
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
                //Validate : Received Quantity should be less than or equal with Order Quantity
                if (true)
                {

                }

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
                PRID = Convert.ToInt32(hdnPRID.Value);
                if (OnDeleteEntityDt(ref errMessage, PRID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpOrderID"] = PRID.ToString();
        }

        private void ControlToEntity(PurchaseReceiveDt entityDt)
        {
            decimal oldQuantity = entityDt.Quantity;

            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
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

            entityDt.GCPPHType = cboPPhTypeDetail.Value.ToString();
            if (cboPPHOptionsDetail.Text.Equals("Plus"))
            {
                entityDt.PPHMode = true;
            }
            else
            {
                entityDt.PPHMode = false;
            }

            if (chkPPHPercentDetail.Checked)
            {
                entityDt.IsPPHInPercentage = true;
            }
            else
            {
                entityDt.IsPPHInPercentage = false;
            }
            entityDt.PPHPercentage = Convert.ToDecimal(Request.Form[txtPPHPercentageDetail.UniqueID]);
            entityDt.PPHAmount = Convert.ToDecimal(Request.Form[txtPPHAmountDetail.UniqueID]);
            entityDt.RemarksDetail = txtRemarksDetail.Text;

            entityDt.LineAmount = entityDt.CustomSubTotal2;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int PRID)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            try
            {
                string purchaseReceiveNo = "";
                int oTermID = 0;

                SavePurchaseReceiveHd(ctx, ref PRID, ref purchaseReceiveNo, oTermID);

                if (entityHdDao.Get(PRID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PurchaseReceiveDt entityDt = new PurchaseReceiveDt();
                    ControlToEntity(entityDt);
                    entityDt.IsBonusItem = true;
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                    entityDt.PurchaseReceiveID = PRID;
                    entityDt.PurchaseOrderID = null;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);
            try
            {
                PurchaseReceiveDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    decimal oldQuantity = entityDt.Quantity;

                    ControlToEntity(entityDt);

                    if (entityDt.PurchaseOrderDtID != null && entityDt.PurchaseOrderDtID != 0)
                    {
                        PurchaseOrderDt entityPODt = entityPODtDao.Get(Convert.ToInt32(entityDt.PurchaseOrderDtID));
                        if (entityPODt != null)
                        {
                            decimal newReceivedQuantity = (entityPODt.ReceivedQuantity - oldQuantity) + entityDt.Quantity;
                            decimal remainingQuantity = entityPODt.Quantity - newReceivedQuantity;

                            if ((entityPODt.UnitPrice != entityDt.UnitPrice)
                                    || (entityPODt.DiscountPercentage1 != entityDt.DiscountPercentage1)
                                    || (entityPODt.DiscountPercentage2 != entityDt.DiscountPercentage2)
                                    || (remainingQuantity < 0))
                            {
                                entityDt.IsNeedConfirmation = true;
                            }
                            else
                            {
                                entityDt.IsNeedConfirmation = false;
                            }
                        }
                    }

                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);
            PurchaseOrderHdDao entityPOHdDao = new PurchaseOrderHdDao(ctx);
            try
            {
                PurchaseReceiveDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseReceiveID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);

                    if (entityDt.PurchaseOrderDtID != null)
                    {
                        PurchaseOrderDt entityPODt = entityPODtDao.Get(Convert.ToInt32(entityDt.PurchaseOrderDtID));
                        if (entityPODt != null)
                        {
                            entityPODt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;
                            if (entityPODt.GCPurchaseUnit == entityDt.GCItemUnit)
                            {
                                if ((entityPODt.ReceivedQuantity - entityDt.Quantity) >= 0)
                                {
                                    string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                        entityPODt.ReceivedInformation = receivedInfo;
                                    else
                                        entityPODt.ReceivedInformation = null;

                                    entityPODt.ReceivedQuantity -= entityDt.Quantity;
                                    entityPODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPODtDao.Update(entityPODt);
                                }
                            }
                            else
                            {
                                decimal receiveQtyCust = entityDt.Quantity * entityDt.ConversionFactor / entityPODt.ConversionFactor;

                                string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                if (receivedInfo != "|" && receivedInfo != string.Empty)
                                    entityPODt.ReceivedInformation = receivedInfo;
                                else
                                    entityPODt.ReceivedInformation = null;

                                entityPODt.ReceivedQuantity -= receiveQtyCust;
                                entityPODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityPODtDao.Update(entityPODt);

                                //if (entityPODt.ConversionFactor < entityDt.ConversionFactor)
                                //{
                                //    receiveQtyCust = entityDt.Quantity * entityDt.ConversionFactor;
                                //}
                                //else
                                //{
                                //    receiveQtyCust = entityDt.Quantity / entityPODt.ConversionFactor;
                                //}

                                //if (receiveQtyCust == entityDt.Quantity)
                                //{
                                //    string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                //    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                //        entityPODt.ReceivedInformation = receivedInfo;
                                //    else
                                //        entityPODt.ReceivedInformation = null;

                                //    entityPODt.ReceivedQuantity -= entityDt.Quantity;
                                //    entityPODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                //    entityPODtDao.Update(entityPODt);
                                //}
                                //else
                                //{
                                //    string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                //    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                //        entityPODt.ReceivedInformation = receivedInfo;
                                //    else
                                //        entityPODt.ReceivedInformation = null;

                                //    entityPODt.ReceivedQuantity -= receiveQtyCust;
                                //    entityPODt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                //    entityPODtDao.Update(entityPODt);
                                //}
                            }
                        }

                        int orderDtCount = BusinessLayer.GetPurchaseOrderDtRowCount(string.Format("PurchaseOrderID = {0} AND (Quantity - ReceivedQuantity) > 0 AND IsDeleted = 0", entityDt.PurchaseOrderID), ctx);
                        if (orderDtCount > 0)
                        {
                            PurchaseOrderHd orderHd = entityPOHdDao.Get(Convert.ToInt32(entityDt.PurchaseOrderID));
                            orderHd.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                            orderHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityPOHdDao.Update(orderHd);
                        }
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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

        public class classAveragePerItem
        {
            private Int32 _ItemID;
            private Decimal _NewAveragePrice;

            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            public Decimal NewAveragePrice
            {
                get { return _NewAveragePrice; }
                set { _NewAveragePrice = value; }
            }
        }

    }
}