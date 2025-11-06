using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class FAItemEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            if (hdnPurchaseReceiveDtID.Value == "")
                return Constant.MenuCode.Accounting.FA_ITEM;
            return Constant.MenuCode.Accounting.FA_ITEM_FROM_PURCHASE_RECEIVE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetFilterExpressionParentFixedAsset()
        {
            string filter = "ParentID IS NULL AND IsDeleted = 0";

            if (txtFixedAssetCode.Text != "" && txtFixedAssetCode.Text != null)
            {
                filter += string.Format(" AND FixedAssetCode != '{0}'", txtFixedAssetCode.Text);
            }

            return filter;
        }

        protected string OnGetFilterExpressionItem()
        {
            return string.Format("GCItemType = '{0}' AND IsFixedAsset = 1 AND IsDeleted = 0", Constant.ItemGroupMaster.LOGISTIC);
        }

        protected string OnGetFilterExpressionSupplier()
        {
            return string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            hdnMenuCode.Value = OnGetMenuCode();

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                AppSession.UserLogin.HealthcareID, //0
                                                Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_FA_ITEM, //1
                                                Constant.SettingParameter.IS_PPN_APPLIED_TO_FA_ITEM, //2
                                                Constant.SettingParameter.AC0021 //3
                                            ));

            hdnIsDiscountAppliedToFAItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_DISCOUNT_APPLIED_TO_FA_ITEM).ParameterValue;
            hdnIsPPNAppliedToFAItem.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_PPN_APPLIED_TO_FA_ITEM).ParameterValue;
            hdnAC0021.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.AC0021).ParameterValue;
            hdnFAAcceptanceID.Value = "0";
            if (Request.QueryString.Count > 0)
            {
                string[] param = Request.QueryString["id"].Split('|');
                hdnTransactionCode.Value = param[0];
                if (hdnTransactionCode.Value == Constant.TransactionCode.DIRECT_PURCHASE)
                {
                    hdnDirectPurchaseID.Value = param[1];
                    SetControlProperties();
                    IsAdd = true;
                    List<vDirectPurchaseDt> listDp = BusinessLayer.GetvDirectPurchaseDtList(string.Format("ID = {0}", hdnDirectPurchaseID.Value));
                    if (listDp.Count > 0)
                    {
                        vDirectPurchaseDt entity = listDp.FirstOrDefault();
                        EntityToControlDirect(entity);
                    }
                    trBusinessPartnerNonMaster.Attributes.Add("style", "display:none");
                }
                else if (hdnTransactionCode.Value == Constant.TransactionCode.PURCHASE_RECEIVE || hdnTransactionCode.Value == Constant.TransactionCode.DONATION_RECEIVE)
                {
                    hdnPurchaseReceiveDtID.Value = param[1];
                    SetControlProperties();
                    IsAdd = true;
                    List<vPurchaseReceiveDt> list = BusinessLayer.GetvPurchaseReceiveDtList(string.Format("ID = {0}", hdnPurchaseReceiveDtID.Value));
                    if (list.Count > 0)
                    {
                        vPurchaseReceiveDt entity = list.FirstOrDefault();
                        EntityToControl(entity);
                        hdnIsBonusItemFromPOR.Value = entity.IsBonusItem ? "1" : "0";
                    }
                    trBusinessPartnerNonMaster.Attributes.Add("style", "display:none");
                }
                else
                {
                    IsAdd = false;
                    String ID = param[0];
                    hdnID.Value = ID;
                    SetControlProperties();

                    string filterFAAcceptance = string.Format("FixedAssetID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", ID, Constant.TransactionStatus.VOID);
                    List<vFAAcceptanceDt> faAcceptanceLst = BusinessLayer.GetvFAAcceptanceDtList(filterFAAcceptance);
                    if (faAcceptanceLst.Count() > 0)
                    {
                        vFAAcceptanceDt faAcceptance = faAcceptanceLst.FirstOrDefault();
                        hdnFAAcceptanceID.Value = faAcceptance.FAAcceptanceID.ToString();
                        txtFAAcceptanceNo.Text = faAcceptance.FAAcceptanceNo;
                        txtFAAcceptanceDate.Text = faAcceptance.AcceptanceDate.ToString(Constant.FormatString.DATE_FORMAT);
                        txtFAAcceptanceStatus.Text = faAcceptance.TransactionStatus;
                    }

                    List<vFAItem> listFA = BusinessLayer.GetvFAItemList(string.Format("FixedAssetID = {0}", ID));
                    if (listFA.Count > 0)
                    {
                        vFAItem entity = BusinessLayer.GetvFAItemList(string.Format("FixedAssetID = {0}", ID))[0];
                        List<vFAItemCOA> listCOA = BusinessLayer.GetvFAItemCOAList(string.Format("FixedAssetID = {0} AND HealthcareID = '{1}'", ID, AppSession.UserLogin.HealthcareID));
                        vFAItemCOA entityCOA;
                        if (listCOA.Count > 0)
                            entityCOA = listCOA[0];
                        else
                            entityCOA = null;
                        EntityToControl(entity, entityCOA);
                    }

                    List<vFAWriteOff> listFW = BusinessLayer.GetvFAWriteOffList(string.Format("FixedAssetID = {0}", ID));
                    if (listFW.Count > 0)
                    {
                        vFAWriteOff entity = BusinessLayer.GetvFAWriteOffList(string.Format("FixedAssetID = {0}", ID))[0];
                        EntityToControlFAWriteOff(entity);
                    }
                }
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                trBusinessPartnerNonMaster.Attributes.Add("style", "display:none");
            }

            txtFixedAssetName.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                        "ParentID IN ('{0}','{1}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.ITEM_UNIT,
                                                        Constant.StandardCode.PURCHASING_BUDGET_CATEGORY
                                                 ));

            Methods.SetComboBoxField<StandardCode>(cboBudgetCategory, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.PURCHASING_BUDGET_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
            cboBudgetCategory.SelectedIndex = 0;

            Methods.SetComboBoxField<StandardCode>(cboProcurementUnit, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            cboProcurementUnit.Value = Constant.ItemUnit.X;
        }

        protected override void OnControlEntrySetting()
        {
            #region Data Aktiva Tetap
            SetControlEntrySetting(txtFixedAssetCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFixedAssetName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnItemID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSerialNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnFAGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtFAGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFAGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnFALocationID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtFALocationCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFALocationName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnParentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtParentFixedAssetCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtParentFixedAssetName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSequenceNo, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(txtDisplayOrder, new ControlEntrySetting(true, true, false, "1"));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBusinessPartnerFromMaster, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(txtBusinessPartnerNameNonMaster, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnBusinessPartnerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBusinessPartnerCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBusinessPartnerName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtContractNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBudgetCategory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtBudgetPlanNo, new ControlEntrySetting(true, true, false));
            #endregion

            #region Data Perolehan Aktiva Tetap
            SetControlEntrySetting(hdnPurchaseReceiveID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseRequestNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProcurementNumber, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProcurementReferenceNumber, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReceivedDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtProcurementDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtProcurementAmount, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtProcurementQuantity, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(cboProcurementUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProductLineFromPOR, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGuaranteeStartDate, new ControlEntrySetting(true, true, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtGuaranteeEndDate, new ControlEntrySetting(true, true, false, Constant.DefaultValueEntry.DATE_NOW));
            #endregion

            #region Data Perhitungan Penyusutan Aktiva Tetap
            SetControlEntrySetting(chkIsUsedDepreciation, new ControlEntrySetting(true, true, false, "true"));
            SetControlEntrySetting(hdnFADepreciationMethodID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtFADepreciationMethodCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtFADepreciationMethodName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtDepreciationStartDate, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(txtDepreciationStartLength, new ControlEntrySetting(true, true, true, "0"));
            SetControlEntrySetting(txtAssetFinalValue, new ControlEntrySetting(true, true, true, "0"));
            #endregion

            #region Lain - lain
            SetControlEntrySetting(txtNoSuratKerusakanAset, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtOldFixedAssetCode, new ControlEntrySetting(true, true, false));
            #endregion

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccount1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount1Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount1Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt1, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt1ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt1Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt1Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount2ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount2Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount2Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt2, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt2ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt2Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt2Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount3ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount3Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount3Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt3, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt3ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt3Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt3Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount4ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName4, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID4, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount4Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount4Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt4, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt4ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt4Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt4Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount5ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName5, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID5, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount5Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount5Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt5, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt5ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt5Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt5Name, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccount6ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName6, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID6, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccount6Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGLAccount6Name, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt6, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSubLedgerDt6ID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDt6Code, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDt6Name, new ControlEntrySetting(false, false, false));
            #endregion

            #region Informasi Pemusnahan
            SetControlEntrySetting(hdnFAWriteOffID, new ControlEntrySetting(false, false));
            SetControlEntrySetting(txtFAWriteOffNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFAWriteOffDate, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtFAWriteOffTransactionStatus, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(vPurchaseReceiveDt entity)
        {
            PurchaseReceiveHd entityHd = BusinessLayer.GetPurchaseReceiveHd(entity.PurchaseReceiveID);

            hdnPurchaseReceiveID.Value = entity.PurchaseReceiveID.ToString();
            if (entity.OtherRequestReferenceNo != null && entity.OtherRequestReferenceNo != "")
            {
                txtPurchaseRequestNumber.Text = entity.OtherRequestReferenceNo;
            }
            else
            {
                txtPurchaseRequestNumber.Text = entity.PurchaseOrderNo;
            }
            txtProcurementNumber.Text = entity.PurchaseReceiveNo;
            txtReceivedDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementDate.Text = entity.ReceivedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementReferenceNumber.Text = entity.ReferenceNo;
            txtProductLineFromPOR.Text = entity.ProductLineName;

            if (hdnAC0021.Value == "1")
            {
                //Decimal totalAfterDisc1 = entity.UnitPrice - (entity.UnitPrice * entity.DiscountPercentage1 / 100);
                //Decimal totalAfterDisc2 = totalAfterDisc1 - (entity.DiscountPercentage2 / 100 * totalAfterDisc1);
                //Decimal total = totalAfterDisc2 * (100 + entity.VATPercentage) / 100;

                //if (entityHd.FinalDiscount != 0)
                //{
                //    Decimal discountFinalPerQty = entityHd.FinalDiscount / entity.Quantity;
                //    total = total - discountFinalPerQty;
                //}

                Decimal totalMain = entity.LineAmount;
                Decimal totalFinalDiscount = entityHd.FinalDiscount;
                Decimal totalAfterDisc = (totalMain - totalFinalDiscount);
                Decimal totalWithVAT = totalAfterDisc * (100 + entity.VATPercentage) / 100;
                Decimal total = totalWithVAT / entity.Quantity;

                txtProcurementAmount.Text = Math.Round(total, 2).ToString();
            }
            else
            {
                decimal unitPrice = entity.UnitPrice;
                decimal discountAmount1, discountAmount2;

                if (entity.IsDiscountInPercentage1)
                {
                    discountAmount1 = (unitPrice * entity.DiscountPercentage1) / 100;
                }
                else
                {
                    discountAmount1 = entity.DiscountAmount1 / entity.Quantity;
                }

                if (entity.IsDiscountInPercentage2)
                {
                    discountAmount2 = ((unitPrice - discountAmount1) * entity.DiscountPercentage2) / 100;
                }
                else
                {
                    discountAmount2 = entity.DiscountAmount2 / entity.Quantity;
                }

                if (hdnIsDiscountAppliedToFAItem.Value == "1")
                {
                    unitPrice = unitPrice - (discountAmount1 + discountAmount2);
                }

                unitPrice = unitPrice * entity.Quantity;

                if (hdnIsPPNAppliedToFAItem.Value == "1")
                {
                    decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entityHd.VATPercentage) / Convert.ToDecimal(100));
                    unitPrice = unitPrice + ppnAmountUnitPrice;
                }

                Decimal PPHPercent = entityHd.IsPPHInPercentage ? entityHd.PPHPercentage : 0;

                Decimal total = (unitPrice * (100 + PPHPercent)) / 100;

                Decimal totalPerolehan = total / entity.Quantity;

                string filterDt = string.Format("PurchaseReceiveID = {0} AND GCItemDetailStatus != '{1}'", entityHd.PurchaseReceiveID, Constant.TransactionStatus.VOID);
                List<PurchaseReceiveDt> lstPORDT = BusinessLayer.GetPurchaseReceiveDtList(filterDt);
                Decimal countQtyDt = lstPORDT.Sum(a => a.Quantity);
                Decimal ongkir = (entityHd.ChargesAmount / countQtyDt);

                total = totalPerolehan + ongkir;

                txtProcurementAmount.Text = Math.Round(total, 2).ToString();
            }
            txtProcurementQuantity.Text = (entity.Quantity / entity.Quantity).ToString();
            cboProcurementUnit.Value = entity.GCItemUnit;

            txtFixedAssetName.Text = entity.ItemName1;

            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;
            txtRemarks.Text = entity.RemarksDetail;
            txtBudgetPlanNo.Text = entity.OtherReferenceNo;
            cboBudgetCategory.Value = entity.GCBudgetCategory;
            hdnBusinessPartnerID.Value = entity.SupplierID.ToString();
            txtBusinessPartnerCode.Text = entity.SupplierCode;
            txtBusinessPartnerName.Text = entity.SupplierName;
        }

        private void EntityToControlDirect(vDirectPurchaseDt entity)
        {
            DirectPurchaseHd entityHd = BusinessLayer.GetDirectPurchaseHd(entity.DirectPurchaseID);

            hdnPurchaseReceiveID.Value = entity.DirectPurchaseID.ToString();
            txtPurchaseRequestNumber.Text = "";

            txtProcurementNumber.Text = entity.DirectPurchaseNo;
            txtReceivedDate.Text = entity.PurchaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementDate.Text = entity.PurchaseDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementReferenceNumber.Text = entity.ReferenceNo;
            txtProductLineFromPOR.Text = entity.ProductLineName;

            decimal unitPrice = entity.UnitPrice;
            decimal discountAmount1, discountAmount2;

            if (entity.IsDiscountInPercentage)
            {
                discountAmount1 = (unitPrice * entity.DiscountPercentage) / 100;
            }
            else
            {
                discountAmount1 = entity.DiscountAmount / entity.Quantity;
            }


            if (entity.IsDiscountInPercentage2)
            {
                discountAmount2 = ((unitPrice - discountAmount1) * entity.DiscountPercentage2) / 100;
            }
            else
            {
                discountAmount2 = entity.DiscountAmount2 / entity.Quantity;
            }

            if (hdnIsDiscountAppliedToFAItem.Value == "1")
            {
                unitPrice = unitPrice - (discountAmount1 + discountAmount2);
            }

            unitPrice = unitPrice * entity.Quantity;

            if (hdnIsPPNAppliedToFAItem.Value == "1")
            {
                decimal ppnAmountUnitPrice = unitPrice * (Convert.ToDecimal(entity.VATPercentage) / Convert.ToDecimal(100));
                unitPrice = unitPrice + ppnAmountUnitPrice;
            }

            Decimal total = unitPrice;

            Decimal totalPerolehan = total / entity.Quantity;

            txtProcurementAmount.Text = Math.Round(totalPerolehan, 2).ToString();
            txtProcurementQuantity.Text = (entity.Quantity / entity.Quantity).ToString();
            cboProcurementUnit.Value = entity.GCItemUnit;

            txtFixedAssetName.Text = entity.ItemName1;

            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;
            txtRemarks.Text = "";
            txtBudgetPlanNo.Text = "";
            cboBudgetCategory.Value = "";
            hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
            txtBusinessPartnerCode.Text = entity.BusinessPartnerCode;
            txtBusinessPartnerName.Text = entity.BusinessPartnerName;
        }

        private void EntityToControlFAWriteOff(vFAWriteOff entity)
        {
            hdnFAWriteOffID.Value = entity.FAWriteOffID.ToString();
            txtFAWriteOffNo.Text = entity.FAWriteOffNo;
            txtFAWriteOffDate.Text = entity.FAWriteOffDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtFAWriteOffTransactionStatus.Text = entity.TransactionStatus;
        }

        private void EntityToControl(vFAItem entity, vFAItemCOA entityCOA)
        {
            #region Data Aktiva Tetap
            txtFixedAssetCode.Text = entity.FixedAssetCode;
            txtFixedAssetName.Text = entity.FixedAssetName;
            hdnItemID.Value = entity.ItemID.ToString();
            txtItemCode.Text = entity.ItemCode;
            txtItemName.Text = entity.ItemName1;
            txtSerialNumber.Text = entity.SerialNumber;
            hdnOldFAGroupID.Value = entity.FAGroupID.ToString();
            hdnFAGroupID.Value = entity.FAGroupID.ToString();
            txtFAGroupCode.Text = entity.FAGroupCode;
            txtFAGroupName.Text = entity.FAGroupName;
            hdnOldLocationID.Value = entity.FALocationID.ToString();
            hdnFALocationID.Value = entity.FALocationID.ToString();
            txtFALocationCode.Text = entity.FALocationCode;
            txtFALocationName.Text = entity.FALocationName;
            hdnParentID.Value = entity.ParentID.ToString();
            txtParentFixedAssetCode.Text = entity.ParentFACode;
            txtParentFixedAssetName.Text = entity.ParentFAName;
            txtSequenceNo.Text = entity.Sequence.ToString();
            txtDisplayOrder.Text = entity.DisplayOrder.ToString();
            txtRemarks.Text = entity.Remarks;
            cboBudgetCategory.Value = entity.GCBudgetCategory.ToString();
            txtBudgetPlanNo.Text = entity.BudgetPlanNo;

            if (entity.BusinessPartnerID > 0)
            {
                chkIsBusinessPartnerFromMaster.Checked = true;
                hdnBusinessPartnerID.Value = entity.BusinessPartnerID.ToString();
                txtBusinessPartnerCode.Text = entity.BusinessPartnerCode;
                txtBusinessPartnerName.Text = entity.BusinessPartnerName;
                trBusinessPartnerNonMaster.Attributes.Add("style", "display:none");
            }
            else if (entity.BusinessPartnerName != "")
            {
                chkIsBusinessPartnerFromMaster.Checked = false;
                txtBusinessPartnerNameNonMaster.Text = entity.BusinessPartnerName;
                trBusinessPartner.Attributes.Add("style", "display:none");
            }
            else
            {
                chkIsBusinessPartnerFromMaster.Checked = true;
                trBusinessPartnerNonMaster.Attributes.Add("style", "display:none");
            }
            txtContractNumber.Text = entity.ContractNumber;
            #endregion

            #region Data Perolehan Aktiva Tetap
            hdnPurchaseReceiveID.Value = entity.PurchaseReceiveID.ToString();
            txtPurchaseRequestNumber.Text = entity.NoPPB;
            txtProcurementNumber.Text = entity.ProcurementNumber;
            txtReceivedDate.Text = entity.ProcurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementDate.Text = entity.ProcurementDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtProcurementAmount.Text = entity.ProcurementAmount.ToString();
            txtProcurementQuantity.Text = entity.ProcurementQuantity.ToString();
            cboProcurementUnit.Value = entity.GCProcurementUnit;
            txtProcurementReferenceNumber.Text = entity.ProcurementReferenceNumber;
            txtProductLineFromPOR.Text = entity.ProductLineName;
            txtGuaranteePeriod.Text = entity.GuaranteePeriod.ToString();
            txtGuaranteeStartDate.Text = entity.GuaranteeStartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtGuaranteeEndDate.Text = entity.GuaranteeEndDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            #endregion

            #region Data Perhitungan Penyusutan Aktiva Tetap

            if (entity.GCBudgetCategory != Constant.BudgetCategory.CAPEX)
            {
                chkIsUsedDepreciation.Attributes.Add("style", "display:none");
                chkIsUsedDepreciation.Checked = false;
            }
            else
            {
                chkIsUsedDepreciation.Attributes.Remove("style");
                chkIsUsedDepreciation.Checked = entity.IsUsedDepreciation;
            }

            hdnFADepreciationMethodID.Value = entity.MethodID.ToString();
            txtFADepreciationMethodCode.Text = entity.MethodCode;
            txtFADepreciationMethodName.Text = entity.MethodName;
            txtDepreciationStartDate.Text = entity.DepreciationStartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDepreciationStartLength.Text = entity.DepreciationLength.ToString();
            txtAssetFinalValue.Text = entity.AssetFinalValue.ToString();
            #endregion

            #region Lain - lain
            txtOldFixedAssetCode.Text = entity.OldFixedAssetCode;
            txtNoSuratKerusakanAset.Text = entity.NoSuratKerusakanAset;
            #endregion

            if (entityCOA != null)
            {
                #region Pengaturan Perkiraan untuk Aktiva Tetap
                #region GL Account 1
                hdnGLAccount1ID.Value = entityCOA.GLAccount1.ToString();
                txtGLAccount1Code.Text = entityCOA.GLAccount1No;
                txtGLAccount1Name.Text = entityCOA.GLAccount1Name;

                hdnSubLedgerID1.Value = entityCOA.SubLedgerID1.ToString();
                hdnSearchDialogTypeName1.Value = entityCOA.SearchDialogTypeName1;
                hdnIDFieldName1.Value = entityCOA.IDFieldName1;
                hdnCodeFieldName1.Value = entityCOA.CodeFieldName1;
                hdnDisplayFieldName1.Value = entityCOA.DisplayFieldName1;
                hdnMethodName1.Value = entityCOA.MethodName1;
                hdnFilterExpression1.Value = entityCOA.FilterExpression1;

                hdnSubLedgerDt1ID.Value = entityCOA.SubLedger1.ToString();
                txtSubLedgerDt1Code.Text = entityCOA.SubLedger1Code;
                txtSubLedgerDt1Name.Text = entityCOA.SubLedger1Name;
                #endregion

                #region GL Account 2
                hdnGLAccount2ID.Value = entityCOA.GLAccount2.ToString();
                txtGLAccount2Code.Text = entityCOA.GLAccount2No;
                txtGLAccount2Name.Text = entityCOA.GLAccount2Name;

                hdnSubLedgerID2.Value = entityCOA.SubLedgerID2.ToString();
                hdnSearchDialogTypeName2.Value = entityCOA.SearchDialogTypeName2;
                hdnIDFieldName2.Value = entityCOA.IDFieldName2;
                hdnCodeFieldName2.Value = entityCOA.CodeFieldName2;
                hdnDisplayFieldName2.Value = entityCOA.DisplayFieldName2;
                hdnMethodName2.Value = entityCOA.MethodName2;
                hdnFilterExpression2.Value = entityCOA.FilterExpression2;

                hdnSubLedgerDt2ID.Value = entityCOA.SubLedger2.ToString();
                txtSubLedgerDt2Code.Text = entityCOA.SubLedger2Code;
                txtSubLedgerDt2Name.Text = entityCOA.SubLedger2Name;
                #endregion

                #region GL Account 3
                hdnGLAccount3ID.Value = entityCOA.GLAccount3.ToString();
                txtGLAccount3Code.Text = entityCOA.GLAccount3No;
                txtGLAccount3Name.Text = entityCOA.GLAccount3Name;

                hdnSubLedgerID3.Value = entityCOA.SubLedgerID3.ToString();
                hdnSearchDialogTypeName3.Value = entityCOA.SearchDialogTypeName3;
                hdnIDFieldName3.Value = entityCOA.IDFieldName3;
                hdnCodeFieldName3.Value = entityCOA.CodeFieldName3;
                hdnDisplayFieldName3.Value = entityCOA.DisplayFieldName3;
                hdnMethodName3.Value = entityCOA.MethodName3;
                hdnFilterExpression3.Value = entityCOA.FilterExpression3;

                hdnSubLedgerDt3ID.Value = entityCOA.SubLedger3.ToString();
                txtSubLedgerDt3Code.Text = entityCOA.SubLedger3Code;
                txtSubLedgerDt3Name.Text = entityCOA.SubLedger3Name;
                #endregion

                #region GL Account 4
                hdnGLAccount4ID.Value = entityCOA.GLAccount4.ToString();
                txtGLAccount4Code.Text = entityCOA.GLAccount4No;
                txtGLAccount4Name.Text = entityCOA.GLAccount4Name;

                hdnSubLedgerID4.Value = entityCOA.SubLedgerID4.ToString();
                hdnSearchDialogTypeName4.Value = entityCOA.SearchDialogTypeName4;
                hdnIDFieldName4.Value = entityCOA.IDFieldName4;
                hdnCodeFieldName4.Value = entityCOA.CodeFieldName4;
                hdnDisplayFieldName4.Value = entityCOA.DisplayFieldName4;
                hdnMethodName4.Value = entityCOA.MethodName4;
                hdnFilterExpression4.Value = entityCOA.FilterExpression4;

                hdnSubLedgerDt4ID.Value = entityCOA.SubLedger4.ToString();
                txtSubLedgerDt4Code.Text = entityCOA.SubLedger4Code;
                txtSubLedgerDt4Name.Text = entityCOA.SubLedger4Name;
                #endregion

                #region GL Account 5
                hdnGLAccount5ID.Value = entityCOA.GLAccount5.ToString();
                txtGLAccount5Code.Text = entityCOA.GLAccount5No;
                txtGLAccount5Name.Text = entityCOA.GLAccount5Name;

                hdnSubLedgerID5.Value = entityCOA.SubLedgerID5.ToString();
                hdnSearchDialogTypeName5.Value = entityCOA.SearchDialogTypeName5;
                hdnIDFieldName5.Value = entityCOA.IDFieldName5;
                hdnCodeFieldName5.Value = entityCOA.CodeFieldName5;
                hdnDisplayFieldName5.Value = entityCOA.DisplayFieldName5;
                hdnMethodName5.Value = entityCOA.MethodName5;
                hdnFilterExpression5.Value = entityCOA.FilterExpression5;

                hdnSubLedgerDt5ID.Value = entityCOA.SubLedger5.ToString();
                txtSubLedgerDt5Code.Text = entityCOA.SubLedger5Code;
                txtSubLedgerDt5Name.Text = entityCOA.SubLedger5Name;
                #endregion

                #region GL Account 6
                hdnGLAccount6ID.Value = entityCOA.GLAccount6.ToString();
                txtGLAccount6Code.Text = entityCOA.GLAccount6No;
                txtGLAccount6Name.Text = entityCOA.GLAccount6Name;

                hdnSubLedgerID6.Value = entityCOA.SubLedgerID6.ToString();
                hdnSearchDialogTypeName6.Value = entityCOA.SearchDialogTypeName6;
                hdnIDFieldName6.Value = entityCOA.IDFieldName6;
                hdnCodeFieldName6.Value = entityCOA.CodeFieldName6;
                hdnDisplayFieldName6.Value = entityCOA.DisplayFieldName6;
                hdnMethodName6.Value = entityCOA.MethodName6;
                hdnFilterExpression6.Value = entityCOA.FilterExpression6;

                hdnSubLedgerDt6ID.Value = entityCOA.SubLedger6.ToString();
                txtSubLedgerDt6Code.Text = entityCOA.SubLedger6Code;
                txtSubLedgerDt6Name.Text = entityCOA.SubLedger6Name;
                #endregion
                #endregion
            }
        }

        private void ControlToEntity(FAItem entity, FAItemCOA entityCOA)
        {
            #region Data Aktiva Tetap
            entity.FixedAssetName = txtFixedAssetName.Text;

            if (hdnItemID.Value != null && hdnItemID.Value != "" && hdnItemID.Value != "0")
            {
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
            }
            else
            {
                entity.ItemID = null;
            }

            entity.SerialNumber = txtSerialNumber.Text;
            entity.FAGroupID = Convert.ToInt32(hdnFAGroupID.Value);
            entity.FALocationID = Convert.ToInt32(hdnFALocationID.Value);

            if (hdnParentID.Value != null && hdnParentID.Value != "" && hdnParentID.Value != "0")
            {
                entity.ParentID = Convert.ToInt32(hdnParentID.Value);
            }
            else
            {
                entity.ParentID = null;
            }

            if (txtSequenceNo.Text != "")
            {
                entity.Sequence = Convert.ToInt32(txtSequenceNo.Text);
            }

            if (txtDisplayOrder.Text != "")
            {
                entity.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
            }

            entity.Remarks = txtRemarks.Text;

            if (chkIsBusinessPartnerFromMaster.Checked)
            {
                if (hdnBusinessPartnerID.Value != null && hdnBusinessPartnerID.Value != "" && hdnBusinessPartnerID.Value != "0")
                {
                    entity.BusinessPartnerID = Convert.ToInt32(hdnBusinessPartnerID.Value);
                }
                else
                {
                    entity.BusinessPartnerID = null;
                }

                entity.BusinessPartnerName = "";
            }
            else
            {
                entity.BusinessPartnerID = null;
                entity.BusinessPartnerName = txtBusinessPartnerNameNonMaster.Text;
            }

            entity.ContractNumber = txtContractNumber.Text;

            if (cboBudgetCategory.Value != null)
            {
                entity.GCBudgetCategory = cboBudgetCategory.Value.ToString();
            }

            entity.BudgetPlanNo = txtBudgetPlanNo.Text;
            #endregion

            #region Data Perolehan Aktiva Tetap
            entity.NoPPB = txtPurchaseRequestNumber.Text;

            if (hdnTransactionCode.Value == Constant.TransactionCode.DIRECT_PURCHASE)
            {
                entity.TransactionCode = Constant.TransactionCode.DIRECT_PURCHASE;
            }
            else if (hdnTransactionCode.Value == Constant.TransactionCode.PURCHASE_RECEIVE)
            {
                entity.TransactionCode = Constant.TransactionCode.PURCHASE_RECEIVE;
            }
            else
            {
                entity.TransactionCode = null;
            }

            if (hdnPurchaseReceiveID.Value != null && hdnPurchaseReceiveID.Value != "0" && hdnPurchaseReceiveID.Value != "")
            {
                entity.PurchaseReceiveID = Convert.ToInt32(hdnPurchaseReceiveID.Value);
            }
            else
            {
                entity.PurchaseReceiveID = null;
            }

            entity.IsBonusItem = hdnIsBonusItemFromPOR.Value == "1" ? true : false;

            entity.ProcurementNumber = txtProcurementNumber.Text;
            entity.ReceivedDate = Helper.GetDatePickerValue(txtReceivedDate.Text);
            entity.ProcurementDate = Helper.GetDatePickerValue(txtProcurementDate.Text);

            if (txtGuaranteePeriod.Text != null && txtGuaranteePeriod.Text != "")
            {
                entity.GuaranteePeriod = Convert.ToInt16(txtGuaranteePeriod.Text);
            }

            entity.ProcurementAmount = Convert.ToDecimal(txtProcurementAmount.Text);
            entity.ProcurementQuantity = Convert.ToDecimal(txtProcurementQuantity.Text);

            if (cboProcurementUnit.Value != null && cboProcurementUnit.Value.ToString() != "")
            {
                entity.GCProcurementUnit = cboProcurementUnit.Value.ToString();
            }
            else
            {
                entity.GCProcurementUnit = null;
            }

            entity.GuaranteeStartDate = Helper.GetDatePickerValue(txtGuaranteeStartDate.Text);
            entity.GuaranteeEndDate = Helper.GetDatePickerValue(txtGuaranteeEndDate.Text);
            #endregion

            #region Data Perhitungan Penyusutan Aktiva Tetap

            if (entity.GCBudgetCategory != Constant.BudgetCategory.CAPEX)
            {
                entity.IsUsedDepreciation = false;
            }
            else
            {
                entity.IsUsedDepreciation = chkIsUsedDepreciation.Checked;
            }

            entity.MethodID = Convert.ToInt32(hdnFADepreciationMethodID.Value);
            entity.DepreciationStartDate = Helper.GetDatePickerValue(txtDepreciationStartDate.Text);
            entity.DepreciationLength = Convert.ToInt16(txtDepreciationStartLength.Text);
            entity.AssetFinalValue = Convert.ToDecimal(txtAssetFinalValue.Text);
            #endregion

            #region Lain - lain
            entity.NoSuratKerusakanAset = txtNoSuratKerusakanAset.Text;
            entity.OldFixedAssetCode = txtOldFixedAssetCode.Text;
            #endregion

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            #region GL Account 1
            if (hdnGLAccount1ID.Value != null && hdnGLAccount1ID.Value != "" && hdnGLAccount1ID.Value != "0")
                entityCOA.GLAccount1 = Convert.ToInt32(hdnGLAccount1ID.Value);
            else
                entityCOA.GLAccount1 = null;
            if (hdnSubLedgerDt1ID.Value != null && hdnSubLedgerDt1ID.Value != "" && hdnSubLedgerDt1ID.Value != "0")
                entityCOA.SubLedger1 = Convert.ToInt32(hdnSubLedgerDt1ID.Value);
            else
                entityCOA.SubLedger1 = null;
            #endregion

            #region GL Account 2
            if (hdnGLAccount2ID.Value != null && hdnGLAccount2ID.Value != "" && hdnGLAccount2ID.Value != "0")
                entityCOA.GLAccount2 = Convert.ToInt32(hdnGLAccount2ID.Value);
            else
                entityCOA.GLAccount2 = null;
            if (hdnSubLedgerDt2ID.Value != null && hdnSubLedgerDt2ID.Value != "" && hdnSubLedgerDt2ID.Value != "0")
                entityCOA.SubLedger2 = Convert.ToInt32(hdnSubLedgerDt2ID.Value);
            else
                entityCOA.SubLedger2 = null;
            #endregion

            #region GL Account 3
            if (hdnGLAccount3ID.Value != null && hdnGLAccount3ID.Value != "" && hdnGLAccount3ID.Value != "0")
                entityCOA.GLAccount3 = Convert.ToInt32(hdnGLAccount3ID.Value);
            else
                entityCOA.GLAccount3 = null;
            if (hdnSubLedgerDt3ID.Value != null && hdnSubLedgerDt3ID.Value != "" && hdnSubLedgerDt3ID.Value != "0")
                entityCOA.SubLedger3 = Convert.ToInt32(hdnSubLedgerDt3ID.Value);
            else
                entityCOA.SubLedger3 = null;
            #endregion

            #region GL Account 4
            if (hdnGLAccount4ID.Value != null && hdnGLAccount4ID.Value != "" && hdnGLAccount4ID.Value != "0")
                entityCOA.GLAccount4 = Convert.ToInt32(hdnGLAccount4ID.Value);
            else
                entityCOA.GLAccount4 = null;
            if (hdnSubLedgerDt4ID.Value != null && hdnSubLedgerDt4ID.Value != "" && hdnSubLedgerDt4ID.Value != "0")
                entityCOA.SubLedger4 = Convert.ToInt32(hdnSubLedgerDt4ID.Value);
            else
                entityCOA.SubLedger4 = null;
            #endregion

            #region GL Account 5
            if (hdnGLAccount5ID.Value != null && hdnGLAccount5ID.Value != "" && hdnGLAccount5ID.Value != "0")
                entityCOA.GLAccount5 = Convert.ToInt32(hdnGLAccount5ID.Value);
            else
                entityCOA.GLAccount5 = null;
            if (hdnSubLedgerDt5ID.Value != null && hdnSubLedgerDt5ID.Value != "" && hdnSubLedgerDt5ID.Value != "0")
                entityCOA.SubLedger5 = Convert.ToInt32(hdnSubLedgerDt5ID.Value);
            else
                entityCOA.SubLedger5 = null;
            #endregion

            #region GL Account 6
            if (hdnGLAccount6ID.Value != null && hdnGLAccount6ID.Value != "" && hdnGLAccount6ID.Value != "0")
                entityCOA.GLAccount6 = Convert.ToInt32(hdnGLAccount6ID.Value);
            else
                entityCOA.GLAccount6 = null;
            if (hdnSubLedgerDt6ID.Value != null && hdnSubLedgerDt6ID.Value != "" && hdnSubLedgerDt6ID.Value != "0")
                entityCOA.SubLedger6 = Convert.ToInt32(hdnSubLedgerDt6ID.Value);
            else
                entityCOA.SubLedger6 = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemDao entityDao = new FAItemDao(ctx);
            FAItemCOADao entityCOADao = new FAItemCOADao(ctx);

            try
            {
                FAItem entity = new FAItem();
                FAItemCOA entityCOA = new FAItemCOA();
                ControlToEntity(entity, entityCOA);
                entity.HealthcareID = entityCOA.HealthcareID = AppSession.UserLogin.HealthcareID;

                if (entity.ParentID != null && entity.ParentID != 0)
                {
                    entity.FixedAssetCode = GenerateChildFixedAssetCode(ctx, entity);
                }
                else
                {
                    entity.FixedAssetCode = GenerateFixedAssetCode(ctx, entity);
                }

                entity.GCItemStatus = Constant.ItemStatus.ACTIVE;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int oFixedAssetID = entityDao.InsertReturnPrimaryKeyID(entity);

                entityCOA.CreatedBy = AppSession.UserLogin.UserID;
                entityCOA.FixedAssetID = oFixedAssetID;
                entityCOADao.Insert(entityCOA);

                retval = entityCOA.FixedAssetID.ToString();

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

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            bool result = true;

            string filterAcceptance = string.Format("FixedAssetID = {0} AND IsDeleted = 0 AND GCTransactionStatus != '{1}'", hdnID.Value, Constant.TransactionStatus.VOID);
            List<vFAAcceptanceDt> lstAcceptance = BusinessLayer.GetvFAAcceptanceDtList(filterAcceptance);
            if (lstAcceptance.Count() > 0)
            {
                result = false;
                errMessage = string.Format("Master aset ini tidak dapat diubah karena sudah memiliki nomor Berita Acara Aset di nomor <b>{0}</b>", lstAcceptance.FirstOrDefault().FAAcceptanceNo);
            }

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            FAItemDao entityDao = new FAItemDao(ctx);
            FAItemCOADao entityCOADao = new FAItemCOADao(ctx);

            try
            {
                FAItem entity = entityDao.Get(Convert.ToInt32(hdnID.Value)); ;
                FAItemCOA entityCOA = entityCOADao.Get(AppSession.UserLogin.HealthcareID, entity.FixedAssetID);
                ControlToEntity(entity, entityCOA);

                if (hdnOldFAGroupID.Value != hdnFAGroupID.Value)
                {
                    if (entity.ParentID != null && entity.ParentID != 0)
                    {
                        entity.FixedAssetCode = GenerateChildFixedAssetCode(ctx, entity);
                    }
                    else
                    {
                        entity.FixedAssetCode = GenerateFixedAssetCode(ctx, entity);
                    }
                }
                else
                {
                    if (hdnOldLocationID.Value != hdnFALocationID.Value)
                    {
                        if (entity.ParentID != null && entity.ParentID != 0)
                        {
                            entity.FixedAssetCode = GenerateChildFixedAssetCode(ctx, entity);
                        }
                        else
                        {
                            entity.FixedAssetCode = GenerateFixedAssetCode(ctx, entity);
                        }
                    }
                }

                entity.LastUpdatedBy = entityCOA.LastUpdatedBy = AppSession.UserLogin.UserID;

                entityDao.Update(entity);
                entityCOADao.Update(entityCOA);

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

        private String GenerateFixedAssetCode(IDbContext ctx, FAItem entity)
        {
            HealthcareDao oHealthcareDao = new HealthcareDao(ctx);
            FALocationDao oFALocationDao = new FALocationDao(ctx);
            StandardCodeDao oStandardCodeDao = new StandardCodeDao(ctx);
            SettingParameterDtDao oSettingParameterDtDao = new SettingParameterDtDao(ctx);

            StringBuilder result = new StringBuilder();

            #region Get Data

            string delimiter = "/";

            string initial = oHealthcareDao.Get(entity.HealthcareID).Initial;

            string prefix = oSettingParameterDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.AC_PREFIX_ASSET_CODE).ParameterValue;

            string location = oFALocationDao.Get(entity.FALocationID).FALocationCode;

            string faGroup = txtFAGroupCode.Text;

            string budgetCategory = oStandardCodeDao.Get(entity.GCBudgetCategory).TagProperty;

            string procurementDate = Helper.GetDatePickerValue(Request.Form[txtProcurementDate.UniqueID]).ToString("MMyy");
            if (initial == "RSMD")
            {
                procurementDate = Helper.GetDatePickerValue(Request.Form[txtProcurementDate.UniqueID]).ToString("yyMM");
            }

            #endregion

            if (initial == "RSSBB")
            {
                delimiter = "";
                procurementDate = Helper.GetDatePickerValue(Request.Form[txtProcurementDate.UniqueID]).ToString("yy");
                result = result.Append(prefix).Append(delimiter).Append(location).Append(delimiter).Append(faGroup).Append(delimiter).Append(budgetCategory).Append(delimiter).Append(procurementDate).Append(delimiter);
            }
            else if (initial == "RSCK")
            {
                string txtLocation = string.Format("{0}/", location);
                result = result.Append(prefix).Append(delimiter).Append(txtLocation).Append(faGroup).Append(delimiter).Append(budgetCategory).Append(delimiter).Append(procurementDate).Append(delimiter);
            }
            else
            {
                result = result.Append(prefix).Append(delimiter).Append(faGroup).Append(delimiter).Append(procurementDate).Append(delimiter);
            }

            FAItem fai = BusinessLayer.GetFAItemList(string.Format("FixedAssetCode LIKE '{0}%' AND ParentID IS NULL", result.ToString()), 1, 1, "FixedAssetCode DESC", ctx).FirstOrDefault();
            int newNumber = 1;
            if (fai != null)
            {
                newNumber = Convert.ToInt32(fai.FixedAssetCode.Substring(result.ToString().Length)) + 1;
            }

            return result.Append(newNumber.ToString().PadLeft(6, '0')).ToString();
        }

        private String GenerateChildFixedAssetCode(IDbContext ctx, FAItem entity)
        {
            HealthcareDao oHealthcareDao = new HealthcareDao(ctx);
            FAItemDao oFAItemDao = new FAItemDao(ctx);
            FALocationDao oFALocationDao = new FALocationDao(ctx);
            StandardCodeDao oStandardCodeDao = new StandardCodeDao(ctx);

            StringBuilder result = new StringBuilder();

            FAItem pfai = BusinessLayer.GetFAItemList(string.Format("FixedAssetID = {0} AND ParentID IS NULL", entity.ParentID), 1, 1, "FixedAssetCode DESC", ctx).FirstOrDefault();

            string parentCode = pfai.FixedAssetCode;

            int newNumber = 1;

            List<FAItem> lstFAItem = BusinessLayer.GetFAItemList(string.Format("ParentID = {0}", entity.ParentID), 1, 1, "FixedAssetCode DESC", ctx);
            if (lstFAItem.Count > 0)
            {
                FAItem fai = oFAItemDao.Get(lstFAItem.FirstOrDefault().FixedAssetID);
                parentCode = fai.FixedAssetCode.Substring(0, (fai.FixedAssetCode.Length - 4));
                int numberParentCode = Convert.ToInt32(fai.FixedAssetCode.Substring((fai.FixedAssetCode.Length - 3), 3));
                newNumber = numberParentCode + 1;
            }

            return string.Format("{0}/{1}", parentCode, newNumber.ToString().PadLeft(3, '0'));
        }

    }
}