using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Inventory
{
    public partial class DirectPurchase : BasePageTrx
    {
        protected string filterExpressionItemProduct = "";
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected int PageCount = 1;
        protected int total = 0;

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DIRECT_PURCHASE;
        }

        protected override void InitializeDataControl()
        {
            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            hdnVATPercentageFromSetvar.Value = hdnVATPercentage.Value;
            txtVATPercentageDefault.Text = hdnVATPercentage.Value;

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.DIRECT_PURCHASE);

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

            hdnGCTransactionStatus.Value = "";


            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                        AppSession.UserLogin.HealthcareID, //0
                        Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //1
                        Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //2
                        Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE, //3
                        Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE, //4
                        Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM, //5
                        Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED, //6
                        Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA, //7
                        Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT, //8
                        Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN //9
            ));
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

            hdnIsAutoUpdateToSupplierItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IM_POR_AUTO_UPDATE_SUPPLIER_ITEM).ParameterValue;

            hdnIsUsingPurchaseDiscountShared.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_USING_PURCHASE_DISCOUNT_SHARED).FirstOrDefault().ParameterValue;

            hdnIsCalculateHNA.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA).FirstOrDefault().ParameterValue;

            hdnIsReceiveUsingBaseUnit.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).FirstOrDefault().ParameterValue;
            hdnKapanPerubahanNilaiHargaKeItemPlanning.Value = lstSettingParameter.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN).FirstOrDefault().ParameterValue;

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}','{3}') AND IsDeleted = 0", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.PENUNJANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsBlackList = 0 AND IsDeleted = 0 AND IsActive = 1", Constant.BusinessObjectType.SUPPLIER);
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
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}' AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.DIRECT_PURCHASE_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboDirectPurchaseType, listStandardCode.ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboDirectPurchaseType.SelectedIndex = 0;
        }

        protected override void OnControlEntrySetting()
        {
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                                    "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                                                    AppSession.UserLogin.HealthcareID, //0
                                                                                    Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC, //1
                                                                                    Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED //2
                                                                                ));

            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            SettingParameterDt setvarDTR = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC).FirstOrDefault();

            string locationCode = string.Empty;
            string locationName = string.Empty;
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
                }
                else
                {
                    SettingParameterDt setvarDTP = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY);

                    Location loc = BusinessLayer.GetLocationList(string.Format("LocationID = '{0}'", setvarDTP.ParameterValue)).FirstOrDefault();
                    locationID = loc.LocationID.ToString();
                    locationItemGroupID = loc.ItemGroupID.ToString();
                    locationCode = loc.LocationCode;
                    locationName = loc.LocationName;
                }
            }

            SetControlEntrySetting(hdnPurchaseID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtDirectPurchaseNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDirectPurchaseDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true));

            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true, locationCode));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true, locationName));
            SetControlEntrySetting(hdnLocationID, new ControlEntrySetting(true, false, true, locationID));
            SetControlEntrySetting(hdnLocationItemGroupID, new ControlEntrySetting(false, false, false, locationItemGroupID));
            SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(true, true, false, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            SetControlEntrySetting(txtTotalPurchase, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtTotalDirectPurchase, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(cboDirectPurchaseType, new ControlEntrySetting(true, false, true));

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
        }

        #region Load Entity
        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            hdnGCTransactionStatus.Value = "";
            chkPPN.Checked = false;
            txtVATPercentageDefault.ReadOnly = true;
            hdnVATPercentage.Value = txtVATPercentageDefault.Text;

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
        protected string GetFilterExpression()
        {
            return hdnRecordFilterExpression.Value;
        }

        protected string IsEditable()
        {
            return hdnIsEditable.Value;
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvDirectPurchaseHdRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHd(filterExpression, PageIndex, "DirectPurchaseID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            if (keyValue != "" && keyValue != null)
            {
                if (filterExpression != "" && filterExpression != null)
                {
                    filterExpression += string.Format(" AND DirectPurchaseNo = '{0}'", keyValue);
                }
                else
                {
                    filterExpression += string.Format("DirectPurchaseNo = '{0}'", keyValue);
                }
            }
            PageIndex = BusinessLayer.GetvDirectPurchaseHdRowIndex(filterExpression, keyValue, "DirectPurchaseID DESC");
            vDirectPurchaseHd entity = BusinessLayer.GetvDirectPurchaseHd(filterExpression, PageIndex, "DirectPurchaseID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vDirectPurchaseHd entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
                hdnIsEditable.Value = "0";
                SetControlEntrySetting(chkIsUrgent, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtReferenceNo, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtReferenceDate, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(txtNotes, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
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

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                rblDiscountType.Enabled = true;
            }
            else
            {
                rblDiscountType.Enabled = false;
            }

            hdnGCTransactionStatus.Value = entity.GCTransactionStatus;
            hdnPurchaseID.Value = entity.DirectPurchaseID.ToString();
            txtDirectPurchaseNo.Text = entity.DirectPurchaseNo;
            txtDirectPurchaseDate.Text = entity.PurchaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != "01-01-1900")
                txtReferenceDate.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            else
                txtReferenceDate.Text = "";
            txtReferenceNo.Text = entity.ReferenceNo;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            hdnLocationItemGroupID.Value = entity.LocationItemGroupID.ToString();
            chkIsUrgent.Checked = entity.IsUrgent;
            cboDirectPurchaseType.Value = entity.GCDirectPurchaseType;
            txtNotes.Text = entity.Remarks;
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

            if (entity.FinalDiscount > 0)
            {
                rblDiscountType.SelectedIndex = 0;
                txtFinalDiscountInPercentage.Text = entity.FinalDiscount.ToString();
            }
            else if (entity.FinalDiscountAmount > 0)
            {
                rblDiscountType.SelectedIndex = 1;
                txtFinalDiscount.Text = entity.FinalDiscountAmount.ToString();
            }
            else
            {
                rblDiscountType.SelectedIndex = 0;
                txtFinalDiscount.Text = "";
                txtFinalDiscountInPercentage.Text = "";
            }

            txtTotalPurchase.Text = entity.TransactionAmount.ToString();

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);

            divApprovedBy.InnerHtml = entity.ApprovedByName;
            divApprovedDate.InnerHtml = entity.cfApprovedDateInFullString;

            if (entity.ApprovedByName != null && entity.ApprovedByName != "")
            {
                trApprovedBy.Style.Remove("display");
                trApprovedDate.Style.Remove("display");
            }
            else
            {
                trApprovedBy.Style.Add("display", "none");
                trApprovedDate.Style.Add("display", "none");
            }

            divVoidBy.InnerHtml = entity.VoidByName;
            divVoidDate.InnerHtml = entity.cfVoidDateInFullString;

            if (entity.VoidByName != null && entity.VoidByName != "")
            {
                trVoidBy.Style.Remove("display");
                trVoidDate.Style.Remove("display");
            }
            else
            {
                trVoidBy.Style.Add("display", "none");
                trVoidDate.Style.Add("display", "none");
            }

            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            divLastUpdatedDate.InnerHtml = entity.cfLastUpdatedDateInFullString;

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPurchaseID.Value != "" && hdnPurchaseID.Value != "0")
            {
                filterExpression = string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.VOID);
                if (transactionAmount > -1)
                    transactionAmount = BusinessLayer.GetDirectPurchaseHd(Convert.ToInt32(hdnPurchaseID.Value)).TransactionAmount;
            }
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvDirectPurchaseDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            List<vDirectPurchaseDt> lstEntity = BusinessLayer.GetvDirectPurchaseDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vDirectPurchaseDt entity = e.Row.DataItem as vDirectPurchaseDt;
                CheckBox chkIsDiscInPct1 = e.Row.FindControl("chkIsDiscInPct1") as CheckBox;
                CheckBox chkIsDiscInPct2 = e.Row.FindControl("chkIsDiscInPct2") as CheckBox;

                chkIsDiscInPct1.Checked = entity.IsDiscountInPercentage;
                chkIsDiscInPct2.Checked = entity.IsDiscountInPercentage2;

                if (entity.CustomSubTotal2 <= 0)
                {
                    e.Row.BackColor = System.Drawing.Color.LightPink;
                }
            }
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntity(DirectPurchaseHd entityHd)
        {
            entityHd.PurchaseDate = Helper.GetDatePickerValue(txtDirectPurchaseDate.Text);
            if (txtReferenceDate.Text != "" && txtReferenceDate.Text != null)
                entityHd.ReferenceDate = Helper.GetDatePickerValue(txtReferenceDate.Text);
            else
                entityHd.ReferenceDate = Helper.InitializeDateTimeNull();
            entityHd.ReferenceNo = txtReferenceNo.Text;
            entityHd.GCDirectPurchaseType = cboDirectPurchaseType.Value.ToString();
            entityHd.IsUrgent = chkIsUrgent.Checked;
            entityHd.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entityHd.Remarks = txtNotes.Text;
            entityHd.IsIncludeVAT = chkPPN.Checked;

            int discountType = rblDiscountType.SelectedIndex;

            if (discountType == 0)
            {
                entityHd.FinalDiscount = Convert.ToDecimal(Request.Form[txtFinalDiscountInPercentage.UniqueID]);
                entityHd.FinalDiscountAmount = 0;
            }
            else
            {
                entityHd.FinalDiscount = 0;
                entityHd.FinalDiscountAmount = Convert.ToDecimal(Request.Form[txtFinalDiscount.UniqueID]);
            }

            if (entityHd.IsIncludeVAT)
            {
                entityHd.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
            }
            else
            {
                entityHd.VATPercentage = 0;
            }

            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityHd.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
        }

        public void SaveDirectPurchaseHd(IDbContext ctx, ref int DirectPurchaseID, ref string DirectPurchaseNo)
        {
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            if (hdnPurchaseID.Value == "0")
            {
                DirectPurchaseHd entityHd = new DirectPurchaseHd();
                ControlToEntity(entityHd);
                entityHd.DirectPurchaseNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.DIRECT_PURCHASE, entityHd.PurchaseDate, ctx);
                entityHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entityHd.CreatedBy = AppSession.UserLogin.UserID;
                DirectPurchaseID = entityHdDao.InsertReturnPrimaryKeyID(entityHd);
                DirectPurchaseNo = entityHd.DirectPurchaseNo;
            }
            else
            {
                DirectPurchaseID = Convert.ToInt32(hdnPurchaseID.Value);
                DirectPurchaseNo = txtDirectPurchaseNo.Text;
            }
        }


        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            try
            {
                int DirectPurchaseID = 0;
                string DirectPurchaseNo = "";
                SaveDirectPurchaseHd(ctx, ref DirectPurchaseID, ref DirectPurchaseNo);
                retval = DirectPurchaseNo;
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
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            try
            {
                DirectPurchaseHd entity = entityHdDao.Get(Convert.ToInt32(hdnPurchaseID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    retval = entity.DirectPurchaseNo;
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses.";
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

        protected override bool OnProposeRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);

            try
            {
                DirectPurchaseHd entity = directPurchaseHdDao.Get(Convert.ToInt32(hdnPurchaseID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;

                    List<DirectPurchaseDt> lstEntity = BusinessLayer.GetDirectPurchaseDtList(string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus = '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.OPEN), ctx);
                    foreach (DirectPurchaseDt entityDt in lstEntity)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        directPurchaseDtDao.Update(entityDt);
                    }

                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    directPurchaseHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses.";
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
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            ItemMasterDao iMasterDao = new ItemMasterDao(ctx);
            ItemGroupMasterDao iGroupMasterDao = new ItemGroupMasterDao(ctx);
            SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);

            try
            {
                DirectPurchaseHd entity = directPurchaseHdDao.Get(Convert.ToInt32(hdnPurchaseID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.ApprovedBy = AppSession.UserLogin.UserID;
                    entity.ApprovedDate = DateTime.Now;

                    List<DirectPurchaseDt> lstEntity = BusinessLayer.GetDirectPurchaseDtList(string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.VOID), ctx);
                    foreach (DirectPurchaseDt entityDt in lstEntity)
                    {
                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.APPROVED;

                        string filterItemMovement = string.Format("ItemID = {0}", entityDt.ItemID);
                        List<ItemMovement> lstItemMovement = BusinessLayer.GetItemMovementList(filterItemMovement, ctx);

                        string filterPlanning = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID);
                        ItemPlanning entityItemPlanning = BusinessLayer.GetItemPlanningList(filterPlanning, ctx).FirstOrDefault();

                        decimal oOldAveragePrice = entityItemPlanning.AveragePrice;
                        decimal oOldUnitPrice = entityItemPlanning.UnitPrice;
                        decimal oOldPurchasePrice = entityItemPlanning.PurchaseUnitPrice;
                        bool oOldIsPriceLastUpdatedBySystem = entityItemPlanning.IsPriceLastUpdatedBySystem;
                        bool oOldIsDeleted = entityItemPlanning.IsDeleted;

                        string filterBalance = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, entityDt.ItemID);
                        List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterBalance, ctx);

                        string filterSupplierItem = String.Format("BusinessPartnerID = {0} AND ItemID IN ({1}) AND GCPurchaseUnit = '{2}' AND IsDeleted = 0", entity.BusinessPartnerID, entityDt.ItemID, entityDt.GCItemUnit);
                        List<SupplierItem> lstSupplierItem = BusinessLayer.GetSupplierItemList(filterSupplierItem, ctx);

                        if (lstItemMovement.Count == 0)
                        {
                            #region Belum Ada Movement

                            if (entityItemPlanning.UnitPrice == 0)
                            {
                                #region Belum Ada Movement & Belum Definisi Harga

                                #region Update Item Planning

                                decimal unitPrice = entityDt.UnitPrice;
                                decimal unitPriceTemp = entityDt.UnitPrice;
                                decimal discountAmount1 = entityDt.DiscountAmount;
                                decimal discountAmount2 = entityDt.DiscountAmount2;

                                ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                                ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                                ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                                if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                                {
                                    if (entityItemGroup.IsDiscountCalculateHNA == true)
                                    {
                                        if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                        {
                                            unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                        }
                                        else
                                        {
                                            unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                        }
                                    }
                                }
                                else
                                {
                                    if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                    {
                                        if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                        {
                                            unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                        }
                                        else
                                        {
                                            unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
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

                                decimal tempAmount = 0;
                                tempAmount = unitPriceTemp;
                                if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                {
                                    tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                                }
                                else
                                {
                                    tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                }

                                if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                {
                                    decimal ppnAmount = tempAmount * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                    tempAmount = tempAmount + ppnAmount;
                                }
                                else
                                {
                                    tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                }

                                decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                                decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                                if (tempQty > 0)
                                    entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                                decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                                entityItemPlanning.BusinessPartnerID = entity.BusinessPartnerID;
                                entityItemPlanning.LastBusinessPartnerID = entity.BusinessPartnerID;
                                entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                                entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                                entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                                if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                {
                                    if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                    {
                                        entityItemPlanning.UnitPrice = unitPriceItemUnit;
                                    }
                                    else
                                    {
                                        entityDt.TempUnitPrice = unitPriceItemUnit;
                                    }

                                    if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                        if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                        {
                                            entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        }
                                        else
                                        {
                                            entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        }
                                        entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                    }
                                    else
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                        if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                        {
                                            entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                        }
                                        else
                                        {
                                            entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice;
                                        }
                                        entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
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
                                            entityDt.TempUnitPrice = unitPriceItemUnit;
                                        }

                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                            if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                            {
                                                entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            }
                                            else
                                            {
                                                entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            }
                                            entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                            if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                            {
                                                entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                            }
                                            else
                                            {
                                                entityDt.TempPurchaseUnitPrice = entityDt.UnitPrice;
                                            }
                                            entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                        }
                                    }
                                    else
                                    {
                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                            entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                        }
                                    }
                                }

                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                #endregion

                                if (hdnIsAutoUpdateToSupplierItem.Value == "1")
                                {
                                    #region Update Supplier Item

                                    SupplierItem entitySuppItem = lstSupplierItem.Where(x => x.ItemID == entityDt.ItemID && x.GCPurchaseUnit == entityDt.GCItemUnit && x.BusinessPartnerID == entity.BusinessPartnerID).FirstOrDefault();

                                    if (entitySuppItem != null)
                                    {
                                        entitySuppItem.ConversionFactor = entityDt.ConversionFactor;
                                        entitySuppItem.Price = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entitySuppItem.DiscountPercentage = entityDt.DiscountPercentage;
                                        entitySuppItem.DiscountPercentage2 = 0;

                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entitySuppItem.GCPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice;
                                            entitySuppItem.GCPurchaseUnit = entityDt.GCItemUnit;
                                        }

                                        entitySuppItem.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        supplierItemDao.Update(entitySuppItem);
                                    }
                                    else
                                    {
                                        entitySuppItem = new SupplierItem();
                                        entitySuppItem.BusinessPartnerID = entity.BusinessPartnerID;
                                        entitySuppItem.ItemID = entityDt.ItemID;
                                        entitySuppItem.Price = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entitySuppItem.DiscountPercentage = entityDt.DiscountPercentage;
                                        entitySuppItem.DiscountPercentage2 = 0;
                                        entitySuppItem.ConversionFactor = entityDt.ConversionFactor;

                                        if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                        {
                                            entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                            entitySuppItem.GCPurchaseUnit = entityDt.GCBaseUnit;
                                        }
                                        else
                                        {
                                            entitySuppItem.PurchaseUnitPrice = entityDt.UnitPrice;
                                            entitySuppItem.GCPurchaseUnit = entityDt.GCItemUnit;
                                        }

                                        entitySuppItem.CreatedBy = AppSession.UserLogin.UserID;
                                        entitySuppItem.CreatedDate = DateTime.Now;
                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        supplierItemDao.Insert(entitySuppItem);
                                    }
                                    #endregion
                                }

                                #endregion

                            }
                            else
                            {
                                #region Belum Ada Movement & Ada Definisi Harga

                                #region Update Item Planning (AveragePrice)
                                decimal unitPrice = entityDt.UnitPrice;
                                decimal unitPriceTemp = entityDt.UnitPrice;
                                decimal discountAmount1 = entityDt.DiscountAmount;
                                decimal discountAmount2 = entityDt.DiscountAmount2;

                                ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                                ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                                ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                                if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                                {
                                    if (entityItemGroup.IsDiscountCalculateHNA == true)
                                    {
                                        if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                        {
                                            unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                        }
                                        else
                                        {
                                            unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                        }
                                    }
                                }
                                else
                                {
                                    if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                    {
                                        if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                        {
                                            unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                        }
                                        else
                                        {
                                            unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
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

                                decimal tempAmount = 0;
                                tempAmount = unitPriceTemp;
                                if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                                {
                                    tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                                }
                                else
                                {
                                    tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                }

                                if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                                {
                                    decimal ppnAmount = tempAmount * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                    tempAmount = tempAmount + ppnAmount;
                                }
                                else
                                {
                                    tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                                }

                                decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                                decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                                if (tempQty > 0)
                                    entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                                decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                                entityItemPlanning.BusinessPartnerID = entity.BusinessPartnerID;
                                entityItemPlanning.LastBusinessPartnerID = entity.BusinessPartnerID;
                                entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                                entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                                entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                                if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                                {
                                    //entityItemPlanning.UnitPrice = unitPriceItemUnit;

                                    if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                        //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        //entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                    }
                                    else
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                        //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                        //entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                    }
                                }
                                else
                                {
                                    if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                    }
                                    else
                                    {
                                        entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                        entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                    }
                                }

                                entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                                entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                itemPlanningDao.Update(entityItemPlanning);
                                BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                                #endregion

                                #endregion
                            }

                            #endregion
                        }
                        else
                        {
                            #region Ada Movement

                            #region Update Item Planning (AveragePrice)
                            decimal unitPrice = entityDt.UnitPrice;
                            decimal unitPriceTemp = entityDt.UnitPrice;
                            decimal discountAmount1 = entityDt.DiscountAmount;
                            decimal discountAmount2 = entityDt.DiscountAmount2;

                            ItemProduct entityItemProduct = iProductDao.Get(entityDt.ItemID);
                            ItemMaster entityItem = iMasterDao.Get(entityDt.ItemID);
                            ItemGroupMaster entityItemGroup = iGroupMasterDao.Get(entityItem.ItemGroupID);

                            if (entityItemProduct.IsDiscountCalculateHNAFromItemGroupMaster == true)
                            {
                                if (entityItemGroup.IsDiscountCalculateHNA == true)
                                {
                                    if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                    {
                                        unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                    }
                                    else
                                    {
                                        unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                    }
                                }
                            }
                            else
                            {
                                if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                                {
                                    if (hdnIsUsingPurchaseDiscountShared.Value == "1")
                                    {
                                        unitPrice = unitPrice - (((discountAmount1 + discountAmount2) / entityDt.Quantity) * entityItemPlanning.PurchaseDiscountSharedInPercentage / 100);
                                    }
                                    else
                                    {
                                        unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / entityDt.Quantity);
                                    }
                                }
                            }

                            decimal tempAmount = 0;
                            tempAmount = unitPriceTemp;
                            if (hdnIsDiscountAppliedToAveragePrice.Value == "1")
                            {
                                tempAmount = ((tempAmount * entityDt.Quantity) - (discountAmount1 + discountAmount2));
                            }
                            else
                            {
                                tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                            }

                            if (hdnIsPPNAppliedToAveragePrice.Value == "1")
                            {
                                decimal ppnAmount = tempAmount * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                                tempAmount = tempAmount + ppnAmount;
                            }
                            else
                            {
                                tempAmount = (entityDt.UnitPrice * entityDt.Quantity);
                            }

                            decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == entityDt.ItemID).Sum(p => p.QuantityEND);
                            decimal tempQty = (qtyEnd + (entityDt.Quantity * entityDt.ConversionFactor));
                            if (tempQty > 0)
                                entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

                            decimal unitPriceItemUnit = unitPrice / entityDt.ConversionFactor;

                            entityItemPlanning.BusinessPartnerID = entity.BusinessPartnerID;
                            entityItemPlanning.LastBusinessPartnerID = entity.BusinessPartnerID;
                            entityItemPlanning.LastPurchasePrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                            entityItemPlanning.LastConversionFactor = entityDt.ConversionFactor;
                            entityItemPlanning.LastPurchaseDiscount = entityDt.DiscountPercentage;
                            entityItemPlanning.LastPurchaseDiscount2 = entityDt.DiscountPercentage2;

                            if (entityItemPlanning.UnitPrice < unitPriceItemUnit)
                            {
                                //entityItemPlanning.UnitPrice = unitPriceItemUnit;

                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                    //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                    //entityItemPlanning.GCPurchaseUnit = entityDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                    //entityItemPlanning.PurchaseUnitPrice = entityDt.UnitPrice;
                                    //entityItemPlanning.GCPurchaseUnit = entityDt.GCItemUnit;
                                }
                            }
                            else
                            {
                                if (hdnIsReceiveUsingBaseUnit.Value == "1")
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice / entityDt.ConversionFactor;
                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCBaseUnit;
                                }
                                else
                                {
                                    entityItemPlanning.LastPurchaseUnitPrice = entityDt.UnitPrice;
                                    entityItemPlanning.LastPurchaseUnit = entityDt.GCItemUnit;
                                }
                            }

                            entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                            entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            itemPlanningDao.Update(entityItemPlanning);
                            BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DP", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                            #endregion

                            #endregion
                        }

                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        directPurchaseDtDao.Update(entityDt);

                    }
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    directPurchaseHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses.";
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
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);

            try
            {
                DirectPurchaseHd entity = directPurchaseHdDao.Get(Convert.ToInt32(hdnPurchaseID.Value));

                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    if ((entity.GLTransactionIDRequest != null && entity.GLTransactionIDRequest != 0) || (entity.GLTransactionDtIDRequest != null && entity.GLTransactionDtIDRequest != 0))
                    {
                        result = false;
                        errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses permintaan kas bon.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        List<DirectPurchaseReturnHd> entityReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(string.Format("DirectPurchaseID = {0} AND GCTransactionStatus != '{1}'", entity.DirectPurchaseID, Constant.TransactionStatus.VOID), ctx);
                        if (entityReturnHd.Count == 0)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                            entity.VoidBy = AppSession.UserLogin.UserID;
                            entity.VoidDate = DateTime.Now;

                            List<DirectPurchaseDt> lstEntity = BusinessLayer.GetDirectPurchaseDtList(string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.VOID), ctx);
                            foreach (DirectPurchaseDt entityDt in lstEntity)
                            {
                                entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                directPurchaseDtDao.Update(entityDt);
                            }
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            directPurchaseHdDao.Update(entity);
                        }
                        else
                        {
                            result = false;
                            errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses retur.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                        }
                    }
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                return false;
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
            DirectPurchaseHdDao directPurchaseHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao directPurchaseDtDao = new DirectPurchaseDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            SupplierItemDao supplierItemDao = new SupplierItemDao(ctx);

            if (type == "reopen")
            {
                #region Re-Open

                try
                {
                    DirectPurchaseHd entity = directPurchaseHdDao.Get(Convert.ToInt32(hdnPurchaseID.Value));
                    if (entity.GCTransactionStatus == Constant.TransactionStatus.APPROVED)
                    {
                        if ((entity.GLTransactionID != null && entity.GLTransactionID != 0) || (entity.GLTransactionDtID != null && entity.GLTransactionDtID != 0))
                        {
                            result = false;
                            errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses realisasi kas bon.";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                        }
                        else
                        {
                            List<DirectPurchaseReturnHd> entityReturnHd = BusinessLayer.GetDirectPurchaseReturnHdList(string.Format("DirectPurchaseID = {0} AND GCTransactionStatus != '{1}'", entity.DirectPurchaseID, Constant.TransactionStatus.VOID), ctx);
                            if (entityReturnHd.Count == 0)
                            {
                                int count = 0;
                                List<DirectPurchaseDt> lstEntity = BusinessLayer.GetDirectPurchaseDtList(string.Format("DirectPurchaseID = {0} AND GCItemDetailStatus != '{1}'", hdnPurchaseID.Value, Constant.TransactionStatus.VOID), ctx);
                                foreach (DirectPurchaseDt entityDt in lstEntity)
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
                                            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                            entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            directPurchaseDtDao.Update(entityDt);
                                        }
                                    }
                                    else
                                    {
                                        entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
                                        entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        directPurchaseDtDao.Update(entityDt);
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
                                        decimal amountNow = qtyNow * itemPriceHistory.NewAveragePrice;

                                        if (entity.IsIncludeVAT)
                                        {
                                            decimal ppnAmount = ((entity.VATPercentage / 100) * amountReturn);
                                            amountReturn = amountReturn + ppnAmount;
                                        }

                                        if (qtyNow + qtyReturn != 0)
                                        {
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
                                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("DIRECTPURHCASE VOID", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        #endregion
                                    }
                                }

                                if (count == 0)
                                {
                                    entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                                    entity.ApprovedBy = null;
                                    entity.ApprovedDate = Helper.GetDatePickerValue("01-01-1900");
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    directPurchaseHdDao.Update(entity);
                                    retval = entity.DirectPurchaseNo;
                                }
                                else
                                {
                                    result = false;
                                    errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat di-reopen karena stok tidak mencukupi di lokasi ini.";
                                    Exception ex = new Exception(errMessage);
                                    Helper.InsertErrorLog(ex);
                                }
                            }
                            else
                            {
                                result = false;
                                errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses retur.";
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                            }
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Pembelian tunai dgn nomor <b>" + entity.DirectPurchaseNo + "</b> tidak dapat diubah karena sudah diproses.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }

                    if (result)
                    {
                        ctx.CommitTransaction();
                    }
                    else
                    {
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

                #endregion
            }
            return result;
        }

        #endregion

        #region callBack Trigger
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
            int PurchaseID = 0;
            string PurchaseNo = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "save")
            {
                if (hdnEntryID.Value.ToString() != "")
                {
                    PurchaseID = Convert.ToInt32(hdnPurchaseID.Value);
                    if (OnSaveEditRecordEntityDt(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecordEntityDt(ref errMessage, ref PurchaseID, ref PurchaseNo))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param[0] == "delete")
            {
                PurchaseID = Convert.ToInt32(hdnPurchaseID.Value);
                if (OnDeleteEntityDt(ref errMessage, PurchaseID))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpPurchaseID"] = PurchaseID.ToString();
        }

        private void ControlToEntity(DirectPurchaseDt entityDt)
        {
            entityDt.ItemID = Convert.ToInt32(hdnItemID.Value);
            entityDt.Quantity = Convert.ToDecimal(txtQuantity.Text);
            entityDt.GCItemUnit = cboItemUnit.Value.ToString();
            entityDt.GCBaseUnit = hdnGCBaseUnit.Value;
            entityDt.ConversionFactor = Convert.ToDecimal(hdnConversionFactor.Value);
            entityDt.UnitPrice = Convert.ToDecimal(txtPrice.Text);
            
            if (chkIsDiscountInPercentage1.Checked)
            {
                entityDt.IsDiscountInPercentage = true;
                entityDt.DiscountPercentage = Convert.ToDecimal(txtDiscount.Text);
                entityDt.DiscountAmount = Convert.ToDecimal(hdnDiscountAmount1.Value);
            }
            else
            {
                entityDt.IsDiscountInPercentage = false;
                entityDt.DiscountPercentage = Convert.ToDecimal(txtDiscount.Text);
                entityDt.DiscountAmount = Convert.ToDecimal(hdnDiscountAmount1.Value);
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

            entityDt.IsControlExpired = false; //butuh ditanyakan
            entityDt.GCItemDetailStatus = Constant.TransactionStatus.OPEN;
        }

        private bool OnSaveAddRecordEntityDt(ref string errMessage, ref int DirectPurchaseID, ref string DirectPurchaseNo)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao entityDtDao = new DirectPurchaseDtDao(ctx);
            try
            {
                SaveDirectPurchaseHd(ctx, ref DirectPurchaseID, ref DirectPurchaseNo);
                if (entityHdDao.Get(DirectPurchaseID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    DirectPurchaseDt entityDt = new DirectPurchaseDt();
                    ControlToEntity(entityDt);
                    entityDt.DirectPurchaseID = DirectPurchaseID;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao entityDtDao = new DirectPurchaseDtDao(ctx);
            try
            {
                DirectPurchaseDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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
            DirectPurchaseHdDao entityHdDao = new DirectPurchaseHdDao(ctx);
            DirectPurchaseDtDao entityDtDao = new DirectPurchaseDtDao(ctx);
            try
            {
                DirectPurchaseDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    entityDt.GCItemDetailStatus = Constant.TransactionStatus.VOID;
                    //entityDt.IsDeleted = true;
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    errMessage = "Pembelian tidak dapat diubah. Harap refresh halaman ini.";
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
    }
}