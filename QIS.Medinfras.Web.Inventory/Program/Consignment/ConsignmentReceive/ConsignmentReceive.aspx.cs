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
    public partial class ConsignmentReceive : BasePageTrx
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        protected string filterExpressionSupplier = "";
        protected string filterExpressionLocation = "";
        protected string filterExpressionItemProduct = "";

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CONSIGNMENT_RECEIVE;
        }

        protected override void InitializeDataControl()
        {
            hdnIsAPConsignmentFromOrder.Value = AppSession.IsAPConsignmentFromOrder;

            filterExpressionItemProduct = string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
            filterExpressionSupplier = string.Format("GCBusinessPartnerType = '{0}' AND IsActive = 1 AND IsBlackList = 0 AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.CONSIGNMENT_RECEIVE);

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

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                            "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                                                                            AppSession.UserLogin.HealthcareID, //0
                                                                            Constant.SettingParameter.IS_CONFIRM_PURCHASE_RECEIVE, //1
                                                                            Constant.SettingParameter.VAT_PERCENTAGE, //2
                                                                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE, //3
                                                                            Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE, //4
                                                                            Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE, //5
                                                                            Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE, //6
                                                                            Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED, //7
                                                                            Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC, //8
                                                                            Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT, //9
                                                                            Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN //10
                                                                        ));

            hdnNeedConfirmation.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_CONFIRM_PURCHASE_RECEIVE).ParameterValue;
            hdnVATPercentage.Value = hdnVATPercentageFromSetvar.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            hdnIsDiscountAppliedToAveragePrice.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            hdnIsDiscountAppliedToUnitPrice.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_UNIT_PRICE).ParameterValue;
            hdnIsPPNAppliedToAveragePrice.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_AVERAGE_PRICE).ParameterValue;
            hdnIsPPNAppliedToUnitPrice.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_UNIT_PRICE).ParameterValue;
            hdnIsPpnAllowChanged.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_PPN_ALLOW_CHANGED).FirstOrDefault().ParameterValue;
            hdnRoleOfficerLogistic.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_DEFAULT_ROLE_OFFICER_LOGISTIC).FirstOrDefault().ParameterValue;
            hdnIsReceiveUsingBaseUnit.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_PURCHASE_RECEIVE_USE_BASE_UNIT).FirstOrDefault().ParameterValue;
            hdnKapanPerubahanNilaiHargaKeItemPlanning.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN).FirstOrDefault().ParameterValue;

            txtVATPercentageDefault.Text = hdnVATPercentage.Value;
            
            SetControlProperties();
            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
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
            List<StandardCode> listStandardCode = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}') AND IsDeleted = 0", Constant.StandardCode.CURRENCY_CODE, Constant.StandardCode.CHARGES_TYPE));
            List<Term> listTerm = BusinessLayer.GetTermList(string.Format("IsDeleted = 0"));
            Methods.SetComboBoxField<StandardCode>(cboChargesType, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CHARGES_TYPE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCurrency, listStandardCode.Where(p => p.ParentID == Constant.StandardCode.CURRENCY_CODE).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<Term>(cboTerm, listTerm, "TermName", "TermID");
            cboChargesType.SelectedIndex = 0;
            cboCurrency.SelectedIndex = 0;

            string locationCode = string.Empty;
            string locationName = string.Empty;
            string gcPurchaseOrderType = string.Empty;
            string locationID = string.Empty;
            string locationItemGroupID = string.Empty;

            if (hdnRoleOfficerLogistic.Value != "" && hdnRoleOfficerLogistic.Value != "0" && hdnRoleOfficerLogistic.Value != null)
            {
                List<UserInRole> uir = BusinessLayer.GetUserInRoleList(String.Format(
                    "HealthcareID = {0} AND UserID = {1} AND RoleID = {2}", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, hdnRoleOfficerLogistic.Value));

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

            hdnLocationID.Value = locationID;
            txtLocationCode.Text = locationCode;
            txtLocationName.Text = locationName;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(hdnPRID, new ControlEntrySetting(false, false, false, "0"));
            SetControlEntrySetting(txtPurchaseReceiveNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseReceiveDate, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtPurchaseReceiveTime, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtFacturNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtDateReferrence, new ControlEntrySetting(true, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));

            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(cboTerm, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(lblLocation, new ControlEntrySetting(true, false));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboCurrency, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, true, true, "1.00"));

            SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtPPN, new ControlEntrySetting(false, false, true, "0"));
            SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtDP, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtCharges, new ControlEntrySetting(true, true, false, "0"));
            SetControlEntrySetting(txtTotalOrderSaldo, new ControlEntrySetting(false, false, true, "0"));

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

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseReceiveDt entity = e.Row.DataItem as vPurchaseReceiveDt;
                CheckBox chkIsBonus = e.Row.FindControl("chkIsBonus") as CheckBox;
                chkIsBonus.Checked = entity.IsBonusItem;
            }
        }

        public override void OnAddRecord()
        {
            hdnPageCount.Value = "0";
            hdnIsEditable.Value = "1";
            chkPPN.Checked = false;
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
            string filterExpression = hdnRecordFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("TransactionCode = '{0}'", Constant.TransactionCode.CONSIGNMENT_RECEIVE);
            
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

                SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtKurs, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtFinalDiscount, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtFinalDiscountInPercentage, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtDP, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtCharges, new ControlEntrySetting(true, false, false));

                txtDPReferrenceNo.Enabled = false;
                cboChargesType.Enabled = false;
                chkPPN.Enabled = false;
                txtVATPercentageDefault.Enabled = false;
            }
            else
            {
                hdnIsEditable.Value = "1";
                txtDPReferrenceNo.Enabled = true;
                cboChargesType.Enabled = true;
                chkPPN.Enabled = true;
                txtVATPercentageDefault.Enabled = true;
            }

            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN && entity.GCTransactionStatus != Constant.TransactionStatus.VOID)
                hdnPrintStatus.Value = "true";
            else
                hdnPrintStatus.Value = "false";

            hdnPRID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseReceiveNo.Text = entity.PurchaseReceiveNo;
            txtPurchaseReceiveDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPurchaseReceiveTime.Text = entity.ReceivedTime;
            txtDateReferrence.Text = entity.ReferenceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.SupplierID.ToString();
            txtSupplierCode.Text = entity.SupplierCode;
            txtSupplierName.Text = entity.SupplierName;
            txtFacturNo.Text = entity.ReferenceNo;
            hdnLocationID.Value = entity.LocationID.ToString();
            txtLocationCode.Text = entity.LocationCode;
            txtLocationName.Text = entity.LocationName;
            txtCharges.Text = entity.ChargesAmount.ToString();
            txtDPReferrenceNo.Text = entity.DownPaymentReferenceNo;
            txtDP.Text = entity.DownPaymentAmount.ToString();
            cboChargesType.Value = entity.GCChargesType.ToString();
            cboTerm.Value = entity.TermID.ToString();
            txtNotes.Text = entity.Remarks;
            cboCurrency.Value = entity.GCCurrencyCode.ToString();
            txtKurs.Text = entity.CurrencyRate.ToString();
            chkPPN.Checked = entity.IsIncludeVAT;
            if (entity.IsIncludeVAT)
            {
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

            txtTotalOrder.Text = entity.TransactionAmount.ToString();
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

            decimal tempTransactionAmount = -1;
            BindGridView(1, true, ref PageCount, ref tempTransactionAmount);
            hdnPageCount.Value = PageCount.ToString();
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount, ref decimal transactionAmount)
        {
            string filterExpression = "1 = 0";
            if (hdnPRID.Value != "")
                filterExpression = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseReceiveDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            if (transactionAmount > -1)
            {
                PurchaseReceiveHd entityHd = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPRID.Value));
                transactionAmount = entityHd.TransactionAmount - entityHd.DiscountAmount;
            }

            List<vPurchaseReceiveDt> lstEntity = BusinessLayer.GetvPurchaseReceiveDtList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
        #endregion

        #region Save Edit Header
        private void ControlToEntityHd(PurchaseReceiveHd entityHd)
        {
            entityHd.ReceivedDate = Helper.GetDatePickerValue(txtPurchaseReceiveDate.Text);
            entityHd.ReceivedTime = txtPurchaseReceiveTime.Text;
            entityHd.LocationID = Convert.ToInt32(hdnLocationID.Value);
            entityHd.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entityHd.TermID = Convert.ToInt32(cboTerm.Value.ToString());
            entityHd.ReferenceNo = txtFacturNo.Text;
            entityHd.ReferenceDate = Helper.GetDatePickerValue(txtDateReferrence.Text);

            entityHd.GCCurrencyCode = cboCurrency.Value.ToString();
            entityHd.CurrencyRate = Convert.ToDecimal(txtKurs.Text);
            entityHd.IsIncludeVAT = chkPPN.Checked;

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

            entityHd.Remarks = txtNotes.Text;
            entityHd.ChargesAmount = Convert.ToDecimal(txtCharges.Text);
            entityHd.DownPaymentReferenceNo = txtDPReferrenceNo.Text;
            entityHd.GCChargesType = cboChargesType.Value.ToString();
            entityHd.FinalDiscount = Convert.ToDecimal(Request.Form[txtFinalDiscount.UniqueID]);
            entityHd.FinalDiscount = Convert.ToDecimal(txtFinalDiscount.Text);
            entityHd.DownPaymentAmount = Convert.ToDecimal(txtDP.Text);
            entityHd.NetTransactionAmount = ((entityHd.TransactionAmount - entityHd.DiscountAmount - entityHd.FinalDiscount) * (100 + entityHd.VATPercentage) / 100) + entityHd.StampAmount + entityHd.ChargesAmount - entityHd.DownPaymentAmount;
        }

        public void SavePurchaseReceiveHd(IDbContext ctx, ref int PRID, ref string PRNo)
        {
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            TermDao termDao = new TermDao(ctx);
            if (hdnPRID.Value == "0")
            {
                PurchaseReceiveHd entityHd = new PurchaseReceiveHd();
                ControlToEntityHd(entityHd);
                int termDay = termDao.Get(entityHd.TermID).TermDay;
                entityHd.IsIncludeVAT = chkPPN.Checked;

                if (entityHd.IsIncludeVAT)
                    entityHd.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
                else
                    entityHd.VATPercentage = 0;
                entityHd.DiscountAmount = 0;
                entityHd.PaymentDueDate = entityHd.ReferenceDate.AddDays(termDay);
                entityHd.TransactionCode = Constant.TransactionCode.CONSIGNMENT_RECEIVE;
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
                SavePurchaseReceiveHd(ctx, ref PRID, ref purchaseReceiveNo);
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
                    errMessage = string.Format("Penerimaan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseReceiveNo);
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
            PurchaseOrderHdDao purchaseOrderHdDao = new PurchaseOrderHdDao(ctx);
            PurchaseOrderDtDao purchaseOrderDtDao = new PurchaseOrderDtDao(ctx);
            PurchaseRequestPODao purchaseRequestPODao = new PurchaseRequestPODao(ctx);
            PurchaseReceivePODao purchaseReceivePODao = new PurchaseReceivePODao(ctx);
            PurchaseReceiveHdDao purchaseHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao purchaseDtDao = new PurchaseReceiveDtDao(ctx);
            ItemPlanningDao itemPlanningDao = new ItemPlanningDao(ctx);
            ItemProductDao iProductDao = new ItemProductDao(ctx);
            ItemMasterDao iMasterDao = new ItemMasterDao(ctx);
            ItemGroupMasterDao iGroupMasterDao = new ItemGroupMasterDao(ctx);

            try
            {
                String filterExpression = "";
                if (hdnNeedConfirmation.Value == "1")
                {
                    filterExpression = string.Format("PurchaseReceiveID = {0}", hdnPRID.Value);
                    List<vPurchaseOrderDtOutStanding> lstEntity = BusinessLayer.GetvPurchaseOrderDtOutStandingList(filterExpression);
                    if (lstEntity.Count > 0)
                    {
                        foreach (vPurchaseOrderDtOutStanding temp in lstEntity)
                        {
                            if (temp.GCItemDetailStatus != Constant.TransactionStatus.WAIT_FOR_APPROVAL)
                            {
                                errMessage = string.Format("APPROVE GAGAL. Butuh konfirmasi item yang tidak sesuai.");
                                Exception ex = new Exception(errMessage);
                                Helper.InsertErrorLog(ex);
                                return false;
                            }
                        }
                    }
                }
                PurchaseReceiveHd entity = purchaseHdDao.Get(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entity);
                    List<PurchaseReceiveDt> lstPurchaseReceiveDt = BusinessLayer.GetPurchaseReceiveDtList(String.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", entity.PurchaseReceiveID, Constant.TransactionStatus.VOID), ctx);
                    String lstItemID = "";
                    foreach (PurchaseReceiveDt obj in lstPurchaseReceiveDt)
                    {
                        if (lstItemID != "")
                            lstItemID += ",";
                        lstItemID += obj.ItemID.ToString();
                    }

                    filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                    List<ItemPlanning> lstItemPlanning = BusinessLayer.GetItemPlanningList(filterExpression, ctx);
                    filterExpression = String.Format("HealthcareID = '{0}' AND ItemID IN ({1}) AND LocationIsDeleted = 0 AND IsDeleted = 0", AppSession.UserLogin.HealthcareID, lstItemID);
                    List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, ctx);

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
                                unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
                            }
                        }
                        else
                        {
                            if (hdnIsDiscountAppliedToUnitPrice.Value == "1")
                            {
                                unitPrice = unitPrice - ((discountAmount1 + discountAmount2) / purchaseDt.Quantity);
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
                            tempAmount = (purchaseDt.UnitPrice * purchaseDt.Quantity);
                        }

                        decimal qtyEnd = lstItemBalance.Where(p => p.ItemID == purchaseDt.ItemID).Sum(p => p.QuantityEND);
                        decimal tempQty = (qtyEnd + (purchaseDt.Quantity * purchaseDt.ConversionFactor));
                        if (tempQty > 0)
                            entityItemPlanning.AveragePrice = ((entityItemPlanning.AveragePrice * qtyEnd) + (tempAmount)) / tempQty;

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
                                    entityItemPlanning.PurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                }
                                else
                                {
                                    purchaseDt.TempPurchaseUnitPrice = purchaseDt.UnitPrice / purchaseDt.ConversionFactor;
                                }
                                entityItemPlanning.GCPurchaseUnit = purchaseDt.GCBaseUnit;
                            }
                            else
                            {
                                entityItemPlanning.LastPurchaseUnitPrice = purchaseDt.UnitPrice;
                                entityItemPlanning.LastPurchaseUnit = purchaseDt.GCItemUnit;
                                if (hdnKapanPerubahanNilaiHargaKeItemPlanning.Value != "2")
                                {
                                    entityItemPlanning.PurchaseUnitPrice = purchaseDt.UnitPrice;
                                }
                                else
                                {
                                    purchaseDt.TempPurchaseUnitPrice = purchaseDt.UnitPrice;
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

                        entityItemPlanning.IsPriceLastUpdatedBySystem = true;
                        entityItemPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        itemPlanningDao.Update(entityItemPlanning);
                        BusinessLayer.ProcessItemPlanningChangedInsertItemPriceHistory("CR", entityItemPlanning.ID, oOldAveragePrice, oOldUnitPrice, oOldPurchasePrice, oOldIsPriceLastUpdatedBySystem, oOldIsDeleted, ctx);
                        #endregion

                        purchaseDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        purchaseDtDao.Update(purchaseDt);

                        if (purchaseDt.PurchaseOrderID != null)
                        {
                            PurchaseReceivePO entityPRPO = new PurchaseReceivePO();
                            entityPRPO.PurchaseOrderID = (int)purchaseDt.PurchaseOrderID;
                            entityPRPO.PurchaseReceiveID = entity.PurchaseReceiveID;
                            entityPRPO.ItemID = purchaseDt.ItemID;
                            entityPRPO.ReceivedQuantity = purchaseDt.Quantity;
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
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
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseRequestPODao.Update(purchaseRequestPO);
                                    receivedQty -= tempReceivedQuantity;
                                }

                                if (poDt.Quantity == poDt.ReceivedQuantity)
                                {
                                    poDt.GCItemDetailStatus = Constant.TransactionStatus.CLOSED;
                                    poDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    purchaseOrderDtDao.Update(poDt);
                                }
                            }

                            int count = BusinessLayer.GetPurchaseOrderDtRowCount(string.Format("PurchaseOrderID = {0} AND Quantity > ReceivedQuantity AND IsDeleted = 0", purchaseDt.PurchaseOrderID), ctx);
                            if (count < 1)
                            {
                                PurchaseOrderHd entityPOHd = purchaseOrderHdDao.Get((int)purchaseDt.PurchaseOrderID);
                                entityPOHd.GCTransactionStatus = Constant.TransactionStatus.CLOSED;
                                entityPOHd.LastUpdatedBy = AppSession.UserLogin.UserID;
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                purchaseOrderHdDao.Update(entityPOHd);
                            }
                        }
                    }

                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.ApprovedBy = AppSession.UserLogin.UserID;
                    entity.ApprovedDate = DateTime.Now;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    purchaseHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else 
                {
                    errMessage = string.Format("Penerimaan konsinyasi {0} tidak dapat diubah. Harap refresh halaman ini.", entity.PurchaseReceiveNo);
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
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
                        errMessage = "PROPOSED GAGAL. Butuh konfirmasi item yang tidak sesuai.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        return false;
                    }
                }

                PurchaseReceiveHd entity = BusinessLayer.GetPurchaseReceiveHd(Convert.ToInt32(hdnPRID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntityHd(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePurchaseReceiveHd(entity);
                    return true;
                }
                else 
                {
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    return false;                                  
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
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
                        else
                        {

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
                SavePurchaseReceiveHd(ctx, ref PRID, ref purchaseReceiveNo);
                if (entityHdDao.Get(PRID).GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    PurchaseReceiveDt entityDt = new PurchaseReceiveDt();
                    ControlToEntity(entityDt);
                    entityDt.IsBonusItem = chkIsBonus.Checked;
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
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            try
            {
                PurchaseReceiveDt entityDt = entityDtDao.Get(Convert.ToInt32(hdnEntryID.Value));
                if (entityHdDao.Get(entityDt.PurchaseReceiveID).GCTransactionStatus == Constant.TransactionStatus.OPEN && entityDt.GCItemDetailStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entityDt);
                    entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Update(entityDt);
                    ctx.CommitTransaction();
                }
                else 
                {
                    result = false;
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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
            PurchaseReceiveHdDao entityHdDao = new PurchaseReceiveHdDao(ctx);
            PurchaseReceiveDtDao entityDtDao = new PurchaseReceiveDtDao(ctx);
            PurchaseOrderDtDao entityPODtDao = new PurchaseOrderDtDao(ctx);
            PurchaseOrderHdDao entityPOHdDao = new PurchaseOrderHdDao(ctx);
            AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
            AuditLog entityAuditLog = new AuditLog();
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
                                    entityPODtDao.Update(entityPODt);
                                }
                            }
                            else
                            {
                                decimal receiveQtyCust = 0;
                                if (entityPODt.ConversionFactor < entityDt.ConversionFactor)
                                {
                                    receiveQtyCust = entityDt.Quantity * entityDt.ConversionFactor;
                                }
                                else
                                {
                                    receiveQtyCust = entityDt.Quantity / entityPODt.ConversionFactor;
                                }

                                if (receiveQtyCust == entityDt.Quantity)
                                {
                                    string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                        entityPODt.ReceivedInformation = receivedInfo;
                                    else
                                        entityPODt.ReceivedInformation = null;

                                    entityPODt.ReceivedQuantity -= entityDt.Quantity;
                                    entityPODtDao.Update(entityPODt);
                                }
                                else
                                {
                                    string receivedInfo = entityPODt.ReceivedInformation.Replace("|" + hdnPRID.Value, "");
                                    if (receivedInfo != "|" && receivedInfo != string.Empty)
                                        entityPODt.ReceivedInformation = receivedInfo;
                                    else
                                        entityPODt.ReceivedInformation = null;

                                    entityPODt.ReceivedQuantity -= receiveQtyCust;
                                    entityPODtDao.Update(entityPODt);
                                }
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
                    result = false;
                    errMessage = "Penerimaan barang tidak dapat diubah. Harap refresh halaman ini.";
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