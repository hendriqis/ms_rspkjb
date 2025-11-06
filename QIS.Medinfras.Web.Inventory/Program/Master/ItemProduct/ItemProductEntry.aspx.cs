using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemProductEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String MenuID = Request.QueryString["page"];
            hdnRequestID.Value = MenuID;
            switch (MenuID)
            {
                case Constant.ItemType.BARANG_MEDIS: return Constant.MenuCode.Inventory.MEDICAL_SUPPLIES;
                case Constant.ItemType.BARANG_UMUM: return Constant.MenuCode.Inventory.LOGISTICS;
                case Constant.ItemType.BAHAN_MAKANAN: return Constant.MenuCode.Inventory.FOOD_AND_BEVERAGES;
                default: return Constant.MenuCode.Inventory.DRUGS;
            }
        }

        protected string GetPageTitle()
        {
            String GCItemType = hdnGCItemType.Value;
            string title = "";
            switch (GCItemType)
            {
                case Constant.ItemType.OBAT_OBATAN: title = GetLabel("Obat-obatan"); break;
                case Constant.ItemType.BARANG_MEDIS: title = GetLabel("Persediaan Medis"); break;
                case Constant.ItemType.BARANG_UMUM: title = GetLabel("Barang Umum"); break;
                case Constant.ItemType.BAHAN_MAKANAN: title = GetLabel("Bahan Makanan"); break;
            }
            hdnPageTitle.Value = title;
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            String GCItemType = Request.QueryString["page"];
            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                AppSession.UserLogin.HealthcareID,
                Constant.SettingParameter.IM_IS_VISIBLE_ATTRIBUTE,
                Constant.SettingParameter.IS_USING_DRUG_ALERT,
                Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE,
                Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY));
            hdnIsVisibleAttribute.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IM_IS_VISIBLE_ATTRIBUTE).FirstOrDefault().ParameterValue;
            hdnIsUsingDrugAlert.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.IS_USING_DRUG_ALERT).FirstOrDefault().ParameterValue;
            hdnGCItemType.Value = GCItemType;
            hdnIsUsedProductLine.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.SA_IS_USED_PRODUCT_LINE).FirstOrDefault().ParameterValue;
            hdnIsEKlaimParameterMandatory.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY).FirstOrDefault().ParameterValue;

            if (hdnGCItemType.Value == Constant.ItemType.BARANG_UMUM || hdnGCItemType.Value == Constant.ItemType.BAHAN_MAKANAN)
            {
                tdIsImplant.Style.Add("display", "none");
                tdMedicalInstrument.Style.Add("display", "none");
                tdFormularium.Style.Add("display", "none");
                tdFormulariumBPJS.Style.Add("display", "none");
                tdFormulariumInHealth.Style.Add("display", "none");
                tdFORNAS.Style.Add("display", "none");
                tdEmployeeFormularium.Style.Add("display", "none");
                tdGovernmentDrugs.Style.Add("display", "none");

                trSalesPriceUsingKatalogSupplier.Style.Add("display", "none");
                trTariffRoundTo100.Style.Add("display", "none");
                trDiscount.Style.Add("display", "none");
                trPPN.Style.Add("display", "none");
            }
            else
            {
                tdIsImplant.Style.Remove("display");
                tdMedicalInstrument.Style.Remove("display");
                tdFormularium.Style.Remove("display");
                tdFormulariumBPJS.Style.Remove("display");
                tdFormulariumInHealth.Style.Remove("display");
                tdFORNAS.Style.Remove("display");
                tdEmployeeFormularium.Style.Remove("display");
                tdGovernmentDrugs.Style.Remove("display");

                trUsingStandardPrice.Style.Remove("display");
                trSalesPriceUsingKatalogSupplier.Style.Remove("display");
                trTariffRoundTo100.Style.Remove("display");
                trDiscount.Style.Remove("display");
                trPPN.Style.Remove("display");
            }

            if (hdnIsUsedProductLine.Value == "1")
            {
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));

                SetControlEntrySetting(chkIsInventoryItem, new ControlEntrySetting(false, false, false, true));
                SetControlEntrySetting(chkIsFixedAsset, new ControlEntrySetting(false, false, false));
                SetControlEntrySetting(chkIsConsigmentItem, new ControlEntrySetting(false, false, false));
            }
            else
            {
                SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));

                SetControlEntrySetting(chkIsInventoryItem, new ControlEntrySetting(true, true, false, true));
                SetControlEntrySetting(chkIsFixedAsset, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsConsigmentItem, new ControlEntrySetting(true, true, false));
            }

            if (hdnIsEKlaimParameterMandatory.Value == "1")
            {
                SetControlEntrySetting(hdnEKlaimParameterID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtEKlaimParameterCode, new ControlEntrySetting(true, true, true));
                SetControlEntrySetting(txtEKlaimParameterName, new ControlEntrySetting(false, false, true));
                lblEKlaimParameter.Attributes.Add("class", "lblLink lblMandatory");
            }
            else
            {
                SetControlEntrySetting(hdnEKlaimParameterID, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtEKlaimParameterCode, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(txtEKlaimParameterName, new ControlEntrySetting(false, false, false));
                lblEKlaimParameter.Attributes.Add("class", "lblLink");
            }

            if (hdnIsUsingDrugAlert.Value == "1")
            {
                if (hdnGCItemType.Value == Constant.ItemType.OBAT_OBATAN || hdnGCItemType.Value == Constant.ItemType.BARANG_MEDIS)
                {
                    trMIMSReference.Style.Remove("display");
                }
                else
                {
                    trMIMSReference.Style.Add("display", "none");
                }
            }
            else
            {
                trMIMSReference.Style.Add("display", "none");
            }

            if (Request.QueryString.Count > 1)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID.Trim();
                vItemProduct entity = BusinessLayer.GetvItemProductList(string.Format("ItemID = {0}", ID))[0];
                ItemTagField entityTagField = BusinessLayer.GetItemTagField(Convert.ToInt32(ID));

                vDrugInfo entityDrug = null;
                if (GCItemType == Constant.ItemType.OBAT_OBATAN || GCItemType == Constant.ItemType.BARANG_MEDIS)
                    entityDrug = BusinessLayer.GetvDrugInfoList(string.Format("ItemID = {0}", ID))[0];

                SetControlProperties();
                EntityToControl(entity, entityDrug, entityTagField);

                chkIsAllowUDD.Visible = GCItemType == Constant.ItemType.OBAT_OBATAN;
                chkIsCSSD.Visible = GCItemType != Constant.ItemType.BARANG_UMUM && GCItemType != Constant.ItemType.BAHAN_MAKANAN;
                chkIsMedicalInstrument.Visible = GCItemType == Constant.ItemType.BARANG_UMUM;

                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
            }

            txtItemName1.Focus();
        }

        protected override void SetControlProperties()
        {
            if (hdnGCItemType.Value != Constant.ItemType.OBAT_OBATAN)
            {
                pnlDrugInformation.Visible = false;
            }

            if (hdnGCItemType.Value != Constant.ItemType.BARANG_UMUM && hdnGCItemType.Value != Constant.ItemType.BAHAN_MAKANAN)
            {
                tdCashPatient.Style.Add("display", "none");
            }

            if (hdnGCItemType.Value != Constant.ItemType.BARANG_UMUM && hdnGCItemType.Value != Constant.ItemType.BARANG_MEDIS)
            {
                trFixed.Style.Add("display", "none");
            }

            chkIsAllowUDD.Visible = hdnGCItemType.Value == Constant.ItemType.OBAT_OBATAN;
            chkIsCSSD.Visible = hdnGCItemType.Value != Constant.ItemType.BARANG_UMUM && hdnGCItemType.Value != Constant.ItemType.BAHAN_MAKANAN;

            List<MarginMarkupHd> lstMarginMarkupHd = BusinessLayer.GetMarginMarkupHdList("IsDeleted = 0");
            lstMarginMarkupHd.Insert(0, new MarginMarkupHd { MarkupID = 0, MarkupName = "" });
            Methods.SetComboBoxField<MarginMarkupHd>(cboMarginMarkup, lstMarginMarkupHd, "MarkupName", "MarkupID");

            List<RestrictionHd> lstRestrictionHd = BusinessLayer.GetRestrictionHdList("IsDeleted = 0");
            lstRestrictionHd.Insert(0, new RestrictionHd { RestrictionID = 0, RestrictionName = "" });
            Methods.SetComboBoxField<RestrictionHd>(cboTransactionRestriction, lstRestrictionHd, "RestrictionName", "RestrictionID");

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}','{11}','{12}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ITEM_UNIT, Constant.StandardCode.ABC_CLASS,
                Constant.StandardCode.DRUG_FORM, Constant.StandardCode.DRUG_CLASSIFICATION, Constant.StandardCode.PREGNANCY_CATEGORY, Constant.StandardCode.QUANTITY_DEDUCTION_TYPE, Constant.StandardCode.TRANSACTION_TYPE,
                Constant.StandardCode.STATUS_VEN, Constant.StandardCode.MEDICATION_ROUTE, Constant.StandardCode.COENAM_RULE, Constant.StandardCode.ITEM_STATUS, Constant.StandardCode.MOVING_CLASSIFICATION, Constant.StandardCode.KATEGORI_ANTIBIOTIK);

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboDose, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboTransactionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.TRANSACTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugForm, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_FORM).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDrugClassification, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DRUG_CLASSIFICATION).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPregnancyCategory, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PREGNANCY_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboStatusVEN, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.STATUS_VEN).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMedicationRoute, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MEDICATION_ROUTE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDosingUnit, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_UNIT && (sc.TagProperty.Contains("PRE") || sc.TagProperty.Contains("1"))).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboCoenamRule, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.COENAM_RULE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboItemStatus, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ITEM_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboMovingClassification, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.MOVING_CLASSIFICATION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAntibioticCategory, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.KATEGORI_ANTIBIOTIK || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetRadioButtonListField<StandardCode>(rblABCClass, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.ABC_CLASS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetRadioButtonListField<StandardCode>(rblConsume, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.QUANTITY_DEDUCTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetRadioButtonListField<StandardCode>(rblSale, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.QUANTITY_DEDUCTION_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            rblABCClass.SelectedIndex = rblABCClass.Items.Count - 1;
            rblConsume.SelectedIndex = rblConsume.Items.Count - 1;
            rblSale.SelectedIndex = rblConsume.Items.Count - 1;
        }

        protected override void OnControlEntrySetting()
        {
            #region Item Information
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtItemName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAlternateItemName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnProductBrandID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtProductBrandCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProductBrandName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnManufacturerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtManufacturerCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtManufacturerName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsChargeToPatient, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(hdnBillingGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBillingGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingGroupName, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(hdnEKlaimParameterID, new ControlEntrySetting(true, true));
            //SetControlEntrySetting(txtEKlaimParameterCode, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtEKlaimParameterName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, true));
            #endregion

            #region Drug Information
            SetControlEntrySetting(txtGenericName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboDose, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDose, new ControlEntrySetting(true, true, false, 0));
            SetControlEntrySetting(cboDrugForm, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnMIMSClassID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMIMSClassCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMIMSClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGUIDMIMSReferece, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnATCClassID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtATCClassCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtATCClassName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnPharmacogeneticID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPharmacogeneticCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPharmacogeneticName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboDrugClassification, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboCoenamRule, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboMedicationRoute, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDosingUnit, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboStatusVEN, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboPregnancyCategory, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsGeneric, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsTempereture, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtTemp, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsFormularium, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBPJSFormularium, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsInhealthFormularium, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsExternalMedication, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkGovernmentDrugs, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(rblConsume, new ControlEntrySetting(true, true, true, "X207^002"));
            SetControlEntrySetting(rblSale, new ControlEntrySetting(true, true, true, "X207^002"));
            SetControlEntrySetting(hdnDefaultSignaID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSignaLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSignaName1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboMovingClassification, new ControlEntrySetting(true, true, false));
            #endregion

            #region Finance Information
            SetControlEntrySetting(txtSuggestedPrice, new ControlEntrySetting(true, true, true, 0));
            //SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            //SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(cboMarginMarkup, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMargin, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(chkIsUsingStandardPrice, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSalesPriceUsingKatalogSupplier, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsTariffRoundTo100, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsDiscountCalculateHNA, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPPNCalculateHNA, new ControlEntrySetting(true, true, false));
            #endregion

            #region Inventory Information
            SetControlEntrySetting(rblABCClass, new ControlEntrySetting(true, true, true, "X109^001"));
            SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboTransactionType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCountInterval, new ControlEntrySetting(true, true, true, 0));
            SetControlEntrySetting(cboTransactionRestriction, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBatchControl, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsProductionItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboItemStatus, new ControlEntrySetting(true, true, true));
            #endregion

            #region Other Information
            SetControlEntrySetting(txtNotes, new ControlEntrySetting(true, true, false));
            #endregion

            //SetControlEntrySetting(chkIsInventoryItem, new ControlEntrySetting(false, false, false, true));
            //SetControlEntrySetting(chkIsFixedAsset, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(chkIsConsigmentItem, new ControlEntrySetting(false, false, false));

            String ID = Request.QueryString["id"];
            if (string.IsNullOrEmpty(ID))
            {
                SetControlEntrySetting(chkIsUsingSerialNumber, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsUsingFixedAsset, new ControlEntrySetting(true, true, false));
            }
            else
            {
                if (hdnIsVisibleAttribute.Value == "0" || hdnIsVisibleAttribute.Value == "")
                {
                    SetControlEntrySetting(chkIsUsingSerialNumber, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(chkIsUsingFixedAsset, new ControlEntrySetting(true, true, false));
                }
                else
                {
                    SetControlEntrySetting(chkIsUsingSerialNumber, new ControlEntrySetting(true, true, false));
                    SetControlEntrySetting(chkIsUsingFixedAsset, new ControlEntrySetting(true, true, false));
                }
            }
        }

        protected override void OnReInitControl()
        {
            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                txt.Text = "";
            }
            #endregion
        }

        private void EntityToControl(vItemProduct entity, vDrugInfo entityDrug, ItemTagField entityTagField)
        {
            #region Item Information
            txtItemCode.Text = entity.ItemCode;
            txtOldItemCode.Text = entity.OldItemCode;
            txtItemName1.Text = entity.ItemName1;
            txtItemName2.Text = entity.ItemName2;
            txtAlternateItemName.Text = entity.AlternateItemName;
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            hdnProductBrandID.Value = entity.ProductBrandID.ToString();
            txtProductBrandCode.Text = entity.ProductBrandCode;
            txtProductBrandName.Text = entity.ProductBrandName;
            hdnManufacturerID.Value = entity.ManufacturerID.ToString();
            txtManufacturerCode.Text = entity.ManufacturerCode;
            txtManufacturerName.Text = entity.ManufacturerName;
            chkIsChargeToPatient.Checked = entity.IsChargeToPatient;
            if (!string.IsNullOrEmpty(entity.GCItemStatus))
            {
                cboItemStatus.Value = entity.GCItemStatus;
            }
            cboMovingClassification.Value = entity.GCMovingClassification;

            hdnBillingGroupID.Value = entity.BillingGroupID.ToString();
            txtBillingGroupCode.Text = entity.BillingGroupCode;
            txtBillingGroupName.Text = entity.BillingGroupName1;

            hdnEKlaimParameterID.Value = entity.EKlaimParameterID.ToString();
            txtEKlaimParameterCode.Text = entity.EKlaimParameterCode;
            txtEKlaimParameterName.Text = entity.EKlaimParameterName;
            #endregion

            #region Finance Information
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            txtSuggestedPrice.Text = entity.HETAmount.ToString();
            cboMarginMarkup.Value = entity.MarkupID.ToString();
            txtMargin.Text = entity.MarginPercentage.ToString();
            chkIsUsingStandardPrice.Checked = entity.IsUsingStandardPrice;
            chkIsSalesPriceUsingKatalogSupplier.Checked = entity.IsSalesPriceUsingKatalogSupplier;
            chkIsTariffRoundTo100.Checked = entity.IsTariffRoundTo100;
            chkIsDiscountCalculateHNA.Checked = entity.IsDiscountCalculateHNAFromItemGroupMaster;
            chkIsPPNCalculateHNA.Checked = entity.IsPPNCalculateHNAFromItemGroupMaster;
            hdnPLIsFixedAsset.Value = entity.IsFixedAsset ? "1" : "0";
            chkIsFixedAsset.Checked = entity.IsFixedAsset;
            cboTransactionType.Value = entity.GCItemRequestType;
            #endregion

            #region Inventory Information
            rblABCClass.SelectedValue = entity.GCABCClass;
            rblConsume.SelectedValue = entity.GCConsumptionDeductionType;
            rblSale.SelectedValue = entity.GCStockDeductionType;
            cboItemUnit.Value = entity.GCItemUnit;
            txtCountInterval.Text = entity.CycleCountInterval.ToString();
            cboTransactionRestriction.Value = entity.RestrictionID.ToString();
            hdnPLIsConsigmentItem.Value = entity.IsConsigmentItem ? "1" : "0";
            chkIsConsigmentItem.Checked = entity.IsConsigmentItem;
            chkIsBatchControl.Checked = entity.IsControlExpired;
            hdnPLIsInventoryItem.Value = entity.IsInventoryItem ? "1" : "0";
            chkIsInventoryItem.Checked = entity.IsInventoryItem;
            chkIsProductionItem.Checked = entity.IsProductionItem;
            chkIsControlExpired.Checked = entity.IsControlExpired;
            chkIsMedicalInstrument.Checked = entity.IsMedicalInstrument;
            chkIsHiddenItem.Checked = entity.IsHiddenItem;
            chkIsUsingSerialNumber.Checked = entity.IsUsingSerialNumber;
            chkIsUsingFixedAsset.Checked = entity.IsUsingFixedAsset;
            chkIsImplant.Checked = entity.IsImplant;
            #endregion

            #region Other Information
            txtNotes.Text = entity.Remarks;
            #endregion

            #region Item Photo
            txtFileName.Text = entity.PictureFileName;
            string ItemImagePath = string.Format("ItemMaster/{0}/", entity.ItemCode);
            imgPreview.Src = string.Format("{0}{1}Item{2}.png", AppConfigManager.QISVirtualDirectory, ItemImagePath, entity.ItemCode);
            #endregion

            #region Drug Information
            if (entityDrug != null)
            {
                txtGenericName.Text = entityDrug.GenericName;
                txtDose.Text = entityDrug.Dose.ToString();
                cboDose.Text = entityDrug.GCDoseUnit;
                cboDrugForm.Value = entityDrug.GCDrugForm;
                cboCoenamRule.Value = entityDrug.GCCoenamRule;
                cboMedicationRoute.Value = entityDrug.GCMedicationRoute;
                cboDosingUnit.Value = entityDrug.GCDosingUnit;
                cboStatusVEN.Value = entityDrug.GCStatusVEN;
                hdnMIMSClassID.Value = entityDrug.MIMSClassID.ToString();
                txtMIMSClassCode.Text = entityDrug.MIMSClassCode;
                txtMIMSClassName.Text = entityDrug.MIMSClassName;
                txtGUIDMIMSReferece.Text = entityDrug.MIMSReferenceID.ToString();
                hdnATCClassID.Value = entityDrug.ATCClassID.ToString();
                txtATCClassCode.Text = entityDrug.ATCClassCode;
                txtATCClassName.Text = entityDrug.ATCClassName;

                hdnKFAReferenceID.Value = entityDrug.KFAReferenceID.ToString();
                if (hdnKFAReferenceID.Value != "0")
                {
                    KFAReference oReference = BusinessLayer.GetKFAReference(Convert.ToInt32(hdnKFAReferenceID.Value));
                    if (oReference != null)
                    {
                        txtKFAReferenceCode.Text = oReference.KFACode;
                        txtKFAReferenceName.Text = oReference.KFAName;
                    }
                }
                //txtKFAReferenceCode.Text = entityDrug.

                hdnPharmacogeneticID.Value = entityDrug.PharmacogeneticID.ToString();
                txtPharmacogeneticCode.Text = entityDrug.PharmacogeneticCode;
                txtPharmacogeneticName.Text = entityDrug.PharmacogeneticName;
                cboDrugClassification.Value = entityDrug.GCDrugClass;
                cboPregnancyCategory.Value = entityDrug.GCPregnancyCategory;
                chkIsGeneric.Checked = entityDrug.IsGenericDrug;
                chkIsFormularium.Checked = entityDrug.IsFormularium;
                chkIsBPJSFormularium.Checked = entityDrug.IsBPJSFormularium;
                chkIsInhealthFormularium.Checked = entityDrug.IsInhealthFormularium;
                chkIsEmployeeFormularium.Checked = entityDrug.IsEmployeeFormularium;
                chkIsExternalMedication.Checked = entityDrug.IsExternalMedication;
                chkGovernmentDrugs.Checked = entityDrug.isGovernmentDrug;

                chkIsChronic.Checked = entityDrug.IsChronic;
                chkISHAM.Checked = entityDrug.IsHAM;
                chkIsLASA.Checked = entityDrug.IsLASA;
                chkIsOOT.Checked = entityDrug.IsOOT;
                chkIsPrecursor.Checked = entityDrug.IsPrecursor;
                chkIsCSSD.Checked = entityDrug.IsCSSD;
                chkIsFORNAS.Checked = entityDrug.IsFORNAS;
                chkIsInjection.Checked = entityDrug.IsInjection;
                chkIsAntibiotics.Checked = entityDrug.IsAntibiotics;
                chkIsPriority.Checked = entityDrug.IsDefault;
                chkIsRestrictiveAntibiotics.Checked = entityDrug.IsRestrictiveAntibiotics;
                chkIsSuplement.Checked = entityDrug.IsSupplement;
                chkIsTempereture.Checked = entityDrug.IsNeedTemperatureControl;
                txtTemp.Text = Convert.ToString(entityDrug.Temperature);
                chkIsAllowUDD.Checked = entityDrug.IsAllowUDD;
                chkIsHasResidualEffect.Checked = entityDrug.IsHasResidualEffect;

                cboAntibioticCategory.Value = entityDrug.GCAntibioticsCategory;

                txtPurposeOfMedication.Text = entityDrug.MedicationPurpose;
                txtMedicationAdministration.Text = entityDrug.MedicationAdministration;
                hdnDefaultSignaID.Value = entityDrug.SignaID.ToString();
                txtSignaLabel.Text = entityDrug.SignaLabel;
                txtSignaName1.Text = entityDrug.SignaName1;
                txtStorageRemarks.Text = entityDrug.StorageRemarks;
            }
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion
        }

        private void ControlToEntity(ItemMaster entity, ItemProduct entityProduct, DrugInfo entityDrug, ItemTagField entityTagField)
        {
            #region Item Information
            entity.ItemName1 = txtItemName1.Text.Replace('\'', '`');
            entity.OldItemCode = txtOldItemCode.Text;
            entity.ItemName2 = txtItemName2.Text.Replace('\'', '`');
            entity.AlternateItemName = txtAlternateItemName.Text.Replace('\'', '`');
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);

            if (hdnProductBrandID.Value == null || hdnProductBrandID.Value == "" || hdnProductBrandID.Value == "0")
            {
                entityProduct.ProductBrandID = null;
            }
            else
            {
                entityProduct.ProductBrandID = Convert.ToInt32(hdnProductBrandID.Value);
            }

            if (hdnManufacturerID.Value == null || hdnManufacturerID.Value == "" || hdnManufacturerID.Value == "0")
            {
                entityProduct.ManufacturerID = null;
            }
            else
            {
                entityProduct.ManufacturerID = Convert.ToInt32(hdnManufacturerID.Value);
            }

            if (hdnGCItemType.Value == Constant.ItemType.BARANG_UMUM || hdnGCItemType.Value == Constant.ItemType.BAHAN_MAKANAN)
            {
                entityProduct.IsChargeToPatient = chkIsChargeToPatient.Checked;
            }
            else
            {
                entityProduct.IsChargeToPatient = true;
            }

            if (cboMovingClassification.Value != null && cboMovingClassification.Value.ToString() != "")
            {
                entityProduct.GCMovingClassification = cboMovingClassification.Value.ToString();
            }
            else
            {
                entityProduct.GCMovingClassification = null;
            }

            if (cboItemStatus.Value != null)
            {
                entity.GCItemStatus = cboItemStatus.Value.ToString();
            }
            else
            {
                entity.GCItemStatus = Constant.ItemStatus.ACTIVE;
            }

            if (hdnBillingGroupID.Value == "" || hdnBillingGroupID.Value == "0")
            {
                entity.BillingGroupID = null;
            }
            else
            {
                entity.BillingGroupID = Convert.ToInt32(hdnBillingGroupID.Value);
            }

            if (hdnEKlaimParameterID.Value == "" || hdnEKlaimParameterID.Value == "0")
            {
                entity.EKlaimParameterID = null;
            }
            else
            {
                entity.EKlaimParameterID = Convert.ToInt32(hdnEKlaimParameterID.Value);
            }
            #endregion

            #region Finance Information
            if (string.IsNullOrEmpty(txtProductLineCode.Text))
                entity.ProductLineID = null;
            else
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);

            if (cboMarginMarkup.Value != null && cboMarginMarkup.Value.ToString() != "0")
                entityProduct.MarkupID = Convert.ToInt32(cboMarginMarkup.Value);
            else
                entityProduct.MarkupID = null;
            entityProduct.IsUsingStandardPrice = chkIsUsingStandardPrice.Checked;
            entityProduct.IsSalesPriceUsingKatalogSupplier = chkIsSalesPriceUsingKatalogSupplier.Checked;
            entityProduct.IsTariffRoundTo100 = chkIsTariffRoundTo100.Checked;
            entityProduct.IsDiscountCalculateHNAFromItemGroupMaster = chkIsDiscountCalculateHNA.Checked;
            entityProduct.IsPPNCalculateHNAFromItemGroupMaster = chkIsPPNCalculateHNA.Checked;
            entityProduct.MarginPercentage = Convert.ToDecimal(txtMargin.Text);

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityProduct.IsFixedAsset = hdnPLIsFixedAsset.Value == "1" ? true : false;
            }
            else
            {
                entityProduct.IsFixedAsset = chkIsFixedAsset.Checked;
            }

            entityProduct.GCItemRequestType = cboTransactionType.Value.ToString();
            #endregion

            #region Inventory Information
            entityProduct.HETAmount = Convert.ToDecimal(txtSuggestedPrice.Text);
            entityProduct.GCABCClass = rblABCClass.SelectedValue;
            entity.GCItemUnit = cboItemUnit.Value.ToString();
            entityProduct.CycleCountInterval = Convert.ToDecimal(txtCountInterval.Text);
            entityProduct.GCConsumptionDeductionType = rblConsume.SelectedValue;
            entityProduct.GCStockDeductionType = rblSale.SelectedValue;
            if (cboTransactionRestriction.Value != null && cboTransactionRestriction.Value.ToString() != "0")
                entityProduct.RestrictionID = Convert.ToInt32(cboTransactionRestriction.Value);
            else
                entityProduct.RestrictionID = null;

            if (hdnIsUsedProductLine.Value == "1")
            {
                entityProduct.IsConsigmentItem = hdnPLIsConsigmentItem.Value == "1" ? true : false;
                entityProduct.IsInventoryItem = hdnPLIsInventoryItem.Value == "1" ? true : false;
            }
            else
            {
                entityProduct.IsConsigmentItem = chkIsConsigmentItem.Checked;
                entityProduct.IsInventoryItem = chkIsInventoryItem.Checked;
            }

            entityProduct.IsControlExpired = chkIsBatchControl.Checked;
            entityProduct.IsProductionItem = chkIsProductionItem.Checked;
            entityProduct.IsControlExpired = chkIsControlExpired.Checked;
            entityProduct.IsMedicalInstrument = chkIsMedicalInstrument.Checked;
            entityProduct.IsHiddenItem = chkIsHiddenItem.Checked;
            entityProduct.IsUsingSerialNumber = chkIsUsingSerialNumber.Checked;
            entityProduct.IsUsingFixedAsset = chkIsUsingFixedAsset.Checked;
            entityProduct.IsImplant = chkIsImplant.Checked;
            #endregion

            #region Other Information
            entity.Remarks = txtNotes.Text;
            #endregion

            #region Drug Information
            if (entityDrug != null)
            {
                entityDrug.GenericName = txtGenericName.Text;
                entityDrug.Dose = Convert.ToDecimal(txtDose.Text);
                if (cboDose.Value != null && cboDose.Value.ToString() != "")
                    entityDrug.GCDoseUnit = cboDose.Value.ToString();
                else
                    entityDrug.GCDoseUnit = null;
                if (cboDrugForm.Value == null)
                    entityDrug.GCDrugForm = null;
                else
                    entityDrug.GCDrugForm = cboDrugForm.Value.ToString();
                if (string.IsNullOrEmpty(txtMIMSClassCode.Text))
                    entityDrug.MIMSClassID = null;
                else
                    entityDrug.MIMSClassID = Convert.ToInt32(hdnMIMSClassID.Value);

                if (string.IsNullOrEmpty(txtATCClassCode.Text))
                    entityDrug.ATCClassID = null;
                else
                    entityDrug.ATCClassID = Convert.ToInt32(hdnATCClassID.Value);

                if (string.IsNullOrEmpty(txtKFAReferenceCode.Text))
                    entityDrug.KFAReferenceID = null;
                else
                    entityDrug.KFAReferenceID = Convert.ToInt32(hdnKFAReferenceID.Value);

                if (string.IsNullOrEmpty(txtPharmacogeneticCode.Text))
                    entityDrug.PharmacogeneticID = null;
                else
                    entityDrug.PharmacogeneticID = Convert.ToInt32(hdnPharmacogeneticID.Value);

                if (cboDrugClassification.Value != null && cboDrugClassification.Value.ToString() != "")
                    entityDrug.GCDrugClass = cboDrugClassification.Value.ToString();
                else
                    entityDrug.GCDrugClass = null;

                if (cboCoenamRule.Value != null && cboCoenamRule.Value.ToString() != "")
                    entityDrug.GCCoenamRule = cboCoenamRule.Value.ToString();
                else
                    entityDrug.GCCoenamRule = null;

                if (cboMedicationRoute.Value != null && cboMedicationRoute.Value.ToString() != "")
                    entityDrug.GCMedicationRoute = cboMedicationRoute.Value.ToString();
                else
                    entityDrug.GCMedicationRoute = Constant.MedicationRoute.OTHER;

                if (cboDosingUnit.Value != null && cboDosingUnit.Value.ToString() != "")
                    entityDrug.GCDosingUnit = cboDosingUnit.Value.ToString();
                else
                    entityDrug.GCDosingUnit = null;

                if (cboStatusVEN.Value != null && cboStatusVEN.Value.ToString() != "")
                    entityDrug.GCStatusVEN = cboStatusVEN.Value.ToString();
                else
                    entityDrug.GCStatusVEN = null;

                if (cboPregnancyCategory.Value != null && cboPregnancyCategory.Value.ToString() != "")
                    entityDrug.GCPregnancyCategory = cboPregnancyCategory.Value.ToString();
                else
                    entityDrug.GCPregnancyCategory = null;

                entityDrug.IsGenericDrug = chkIsGeneric.Checked;
                entityDrug.IsFormularium = chkIsFormularium.Checked;
                entityDrug.IsBPJSFormularium = chkIsBPJSFormularium.Checked;
                entityDrug.IsEmployeeFormularium = chkIsEmployeeFormularium.Checked;
                entityDrug.IsInhealthFormularium = chkIsInhealthFormularium.Checked;
                entityDrug.IsAllowUDD = chkIsAllowUDD.Checked;

                if (hdnRequestID.Value == Constant.ItemGroupMaster.SUPPLIES)
                {
                    entityDrug.IsExternalMedication = true;
                }
                else
                {
                    entityDrug.IsExternalMedication = chkIsExternalMedication.Checked;
                }

                entityDrug.IsChronic = chkIsChronic.Checked;
                entityDrug.IsHAM = chkISHAM.Checked;
                entityDrug.IsLASA = chkIsLASA.Checked;
                entityDrug.IsOOT = chkIsOOT.Checked;
                entityDrug.IsPrecursor = chkIsPrecursor.Checked;
                entityDrug.IsCSSD = chkIsCSSD.Checked;
                entityDrug.IsFORNAS = chkIsFORNAS.Checked;
                entityDrug.IsInjection = chkIsInjection.Checked;
                entityDrug.IsGovernmentDrug = chkGovernmentDrugs.Checked;
                entityDrug.IsAntibiotics = chkIsAntibiotics.Checked;
                entityDrug.IsDefault = chkIsPriority.Checked;
                entityDrug.IsSupplement = chkIsSuplement.Checked;
                entityDrug.IsNeedTemperatureControl = chkIsTempereture.Checked;
                entityDrug.IsHasResidualEffect = chkIsHasResidualEffect.Checked;

                if (entityDrug.IsNeedTemperatureControl == true)
                {
                    if ((!String.IsNullOrEmpty(txtTemp.Text)))
                    {
                        //entityDrug.Temperature = Convert.ToDecimal(txtTemp.Text);
                        entityDrug.Temperature = txtTemp.Text;
                    }
                }
                entityDrug.MedicationPurpose = txtPurposeOfMedication.Text;
                entityDrug.MedicationAdministration = txtMedicationAdministration.Text;
                if (string.IsNullOrEmpty(txtSignaLabel.Text))
                    entityDrug.SignaID = null;
                else
                    entityDrug.SignaID = Convert.ToInt32(hdnDefaultSignaID.Value);
                entityDrug.StorageRemarks = txtStorageRemarks.Text;

                if (chkIsAntibiotics.Checked)
                {
                    if (cboAntibioticCategory.Value != null && cboAntibioticCategory.Value.ToString() != "")
                    {
                        entityDrug.GCAntibioticsCategory = cboAntibioticCategory.Value.ToString();
                        if (cboAntibioticCategory.Value.ToString() == Constant.AntibioticCategory.RESERVED)
                        {
                            entityDrug.IsRestrictiveAntibiotics = true;
                        }
                        else
                        {
                            entityDrug.IsRestrictiveAntibiotics = false;
                        }
                    }
                    else
                    {
                        entityDrug.GCAntibioticsCategory = null;
                        entityDrug.IsRestrictiveAntibiotics = false;
                    }
                }
                else
                {
                    entityDrug.GCAntibioticsCategory = null;
                    entityDrug.IsRestrictiveAntibiotics = false;
                }

                if (hdnIsUsingDrugAlert.Value == "1")
                {
                    if (!string.IsNullOrEmpty(hdnMIMSRefereceID.Value) && hdnMIMSRefereceID.Value != "0")
                    {
                        var tempGuid = Guid.Parse(Request.Form[txtGUIDMIMSReferece.UniqueID]);
                        entityDrug.MIMSReferenceID = tempGuid;
                    }
                    else
                    {
                        entityDrug.MIMSReferenceID = System.Guid.Empty;
                    }
                }
            }
            #endregion

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion
        }

        private void UploadItemPhoto(int ItemID, string ItemCode, string ItemName1, ref string fileName)
        {
            if (hdnUploadedFile1.Value != "")
            {
                string imageData = hdnUploadedFile1.Value;
                if (imageData != "")
                {
                    string[] parts = Regex.Split(imageData, ",").Skip(1).ToArray();
                    imageData = String.Join(",", parts);
                }

                string path = AppConfigManager.QISPhysicalDirectory;
                path += string.Format("{0}\\", AppConfigManager.QISItemImagePath.Replace('/', '\\'));

                path = path.Replace("#ItemCode", string.Format("{0}", ItemCode));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                fileName = String.Format("Item{0}.png", ItemCode);
                FileStream fs = new FileStream(string.Format("{0}{1}", path, fileName), FileMode.Create);
                BinaryWriter bw = new BinaryWriter(fs);

                byte[] data = Convert.FromBase64String(imageData);
                bw.Write(data);
                bw.Close();
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            string filename = "";
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemProductDao entityProductDao = new ItemProductDao(ctx);
            DrugInfoDao entityDrugDao = new DrugInfoDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            ItemCostDao entityCostDao = new ItemCostDao(ctx);
            ItemPlanningDao entityPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                ItemMaster entity = new ItemMaster();
                ItemProduct entityProduct = new ItemProduct();
                DrugInfo entityDrug = null;
                if (hdnGCItemType.Value == Constant.ItemGroupMaster.DRUGS || hdnGCItemType.Value == Constant.ItemGroupMaster.SUPPLIES)
                {
                    entityDrug = new DrugInfo();
                }
                ItemTagField entityTagField = new ItemTagField();
                ControlToEntity(entity, entityProduct, entityDrug, entityTagField);
                entity.ItemCode = Helper.GenerateItemCode(ctx, entity.ItemName1);
                entity.GCItemType = hdnGCItemType.Value;

                UploadItemPhoto(entity.ItemID, entity.ItemCode, entity.ItemName1, ref filename);
                entity.PictureFileName = filename;

                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityProduct.ItemID = entityDao.InsertReturnPrimaryKeyID(entity);

                entityProductDao.Insert(entityProduct);

                entityTagField.ItemID = entityProduct.ItemID;
                entityTagFieldDao.Insert(entityTagField);

                if (entityDrug != null)
                {
                    entityDrug.ItemID = entityProduct.ItemID;
                    entityDrugDao.Insert(entityDrug);
                }

                List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("", ctx);
                foreach (Healthcare healthcare in lstHealthcare)
                {
                    ItemCost ic = new ItemCost();
                    ic.ItemID = entityProduct.ItemID;
                    ic.HealthcareID = healthcare.HealthcareID;
                    ic.CreatedBy = AppSession.UserLogin.UserID;
                    entityCostDao.Insert(ic);

                    ItemPlanning ip = new ItemPlanning();
                    ip.BusinessPartnerID = null;
                    ip.ItemID = entityProduct.ItemID;
                    ip.IsUsingDynamicROP = false;

                    List<SettingParameterDt> ListSettingParameterDt = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}','{1}','{2}')", Constant.SettingParameter.DEFAULT_LEAD_TIME, Constant.SettingParameter.DEFAULT_BACKWARD_DAYS, Constant.SettingParameter.DEFAULT_FORWARD_DAYS));
                    ip.LeadTime = Convert.ToByte(ListSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_LEAD_TIME).FirstOrDefault().ParameterValue);
                    ip.BackwardDays = Convert.ToByte(ListSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_BACKWARD_DAYS).FirstOrDefault().ParameterValue);
                    ip.ForwardDays = Convert.ToByte(ListSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.DEFAULT_FORWARD_DAYS).FirstOrDefault().ParameterValue);

                    ip.HealthcareID = healthcare.HealthcareID;
                    ip.CreatedBy = AppSession.UserLogin.UserID;
                    ip.IsPriceLastUpdatedBySystem = false;
                    entityPlanningDao.Insert(ip);
                }
                retval = entityProduct.ItemID.ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            string filename = "";
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemProductDao entityProductDao = new ItemProductDao(ctx);
            DrugInfoDao entityDrugDao = new DrugInfoDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            ItemPlanningDao entityPlanningDao = new ItemPlanningDao(ctx);
            try
            {
                Int32 ItemID = Convert.ToInt32(hdnID.Value.Trim());
                ItemMaster entity = entityDao.Get(ItemID);

                string tempCboItemUnit = entity.GCItemUnit;
                int tempProductLineID = Convert.ToInt32(entity.ProductLineID);

                ItemProduct entityProduct = entityProductDao.Get(ItemID);

                DrugInfo entityDrug = null;
                if (hdnGCItemType.Value == Constant.ItemGroupMaster.DRUGS || hdnGCItemType.Value == Constant.ItemGroupMaster.SUPPLIES)
                {
                    entityDrug = entityDrugDao.Get(ItemID);
                }

                ItemTagField entityTagField = entityTagFieldDao.Get(ItemID);
                ControlToEntity(entity, entityProduct, entityDrug, entityTagField);

                ItemPlanning entityPlanning = BusinessLayer.GetItemPlanningList(string.Format("ItemID = {0}", ItemID), ctx).FirstOrDefault();

                decimal oOldAveragePrice = entityPlanning.AveragePrice;
                decimal oOldUnitPrice = entityPlanning.UnitPrice;
                decimal oOldPurchasePrice = entityPlanning.PurchaseUnitPrice;
                bool oOldIsPriceLastUpdatedBySystem = entityPlanning.IsPriceLastUpdatedBySystem;
                bool oOldIsDeleted = entityPlanning.IsDeleted;

                if (entityPlanning.GCPurchaseUnit == tempCboItemUnit)
                {
                    if (tempCboItemUnit != entity.GCItemUnit)
                    {
                        entityPlanning.GCPurchaseUnit = entity.GCItemUnit;
                        entityPlanning.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityPlanningDao.Update(entityPlanning);
                        result = true;
                    }
                }

                List<ItemBalance> lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID = {0}", ItemID), ctx);
                if (!entity.GCItemUnit.Equals(tempCboItemUnit) || entity.GCItemStatus == Constant.ItemStatus.IN_ACTIVE || (tempProductLineID != entity.ProductLineID && tempProductLineID != null && tempProductLineID != 0))
                {
                    if (lstItemBalance.Sum(T => T.QuantityEND) > 0)
                    {
                        result = false;
                    }
                }

                if (result)
                {
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityProduct.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;

                    UploadItemPhoto(entity.ItemID, entity.ItemCode, entity.ItemName1, ref filename);
                    entity.PictureFileName = filename;

                    entityDao.Update(entity);
                    entityTagFieldDao.Update(entityTagField);

                    ctx.CommandType = CommandType.Text;
                    ctx.Command.Parameters.Clear();
                    entityProductDao.Update(entityProduct);

                    if (entityDrug != null)
                    {
                        entityDrug.LastUpdatedBy = AppSession.UserLogin.UserID;
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        entityDrugDao.Update(entityDrug);
                    }

                    ctx.CommitTransaction();
                }
                else
                {
                    errMessage = "Item tidak bisa diubah karena masih ada stok di persediaan RS dan satuan kecil sudah digunakan dalam Perencanaan Persediaan (ItemPlanning).";
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

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if (pnlCustomAttribute.Visible)
            {
                List<Variable> ListCustomAttribute = initListCustomAttribute();
                if (ListCustomAttribute.Count == 0)
                    pnlCustomAttribute.Visible = false;
                else
                {
                    rptCustomAttribute.DataSource = ListCustomAttribute;
                    rptCustomAttribute.DataBind();
                }
            }
        }

        private List<Variable> initListCustomAttribute()
        {
            List<Variable> ListCustomAttribute = new List<Variable>();
            TagField tagField = BusinessLayer.GetTagField(Constant.BusinessObjectType.ITEM);
            if (tagField != null)
            {
                if (tagField.TagField1 != "") { ListCustomAttribute.Add(new Variable { Code = "1", Value = tagField.TagField1 }); }
                if (tagField.TagField2 != "") { ListCustomAttribute.Add(new Variable { Code = "2", Value = tagField.TagField2 }); }
                if (tagField.TagField3 != "") { ListCustomAttribute.Add(new Variable { Code = "3", Value = tagField.TagField3 }); }
                if (tagField.TagField4 != "") { ListCustomAttribute.Add(new Variable { Code = "4", Value = tagField.TagField4 }); }
                if (tagField.TagField5 != "") { ListCustomAttribute.Add(new Variable { Code = "5", Value = tagField.TagField5 }); }
                if (tagField.TagField6 != "") { ListCustomAttribute.Add(new Variable { Code = "6", Value = tagField.TagField6 }); }
                if (tagField.TagField7 != "") { ListCustomAttribute.Add(new Variable { Code = "7", Value = tagField.TagField7 }); }
                if (tagField.TagField8 != "") { ListCustomAttribute.Add(new Variable { Code = "8", Value = tagField.TagField8 }); }
                if (tagField.TagField9 != "") { ListCustomAttribute.Add(new Variable { Code = "9", Value = tagField.TagField9 }); }
                if (tagField.TagField10 != "") { ListCustomAttribute.Add(new Variable { Code = "10", Value = tagField.TagField10 }); }
                if (tagField.TagField11 != "") { ListCustomAttribute.Add(new Variable { Code = "11", Value = tagField.TagField11 }); }
                if (tagField.TagField12 != "") { ListCustomAttribute.Add(new Variable { Code = "12", Value = tagField.TagField12 }); }
                if (tagField.TagField13 != "") { ListCustomAttribute.Add(new Variable { Code = "13", Value = tagField.TagField13 }); }
                if (tagField.TagField14 != "") { ListCustomAttribute.Add(new Variable { Code = "14", Value = tagField.TagField14 }); }
                if (tagField.TagField15 != "") { ListCustomAttribute.Add(new Variable { Code = "15", Value = tagField.TagField15 }); }
                if (tagField.TagField16 != "") { ListCustomAttribute.Add(new Variable { Code = "16", Value = tagField.TagField16 }); }
                if (tagField.TagField17 != "") { ListCustomAttribute.Add(new Variable { Code = "17", Value = tagField.TagField17 }); }
                if (tagField.TagField18 != "") { ListCustomAttribute.Add(new Variable { Code = "18", Value = tagField.TagField18 }); }
                if (tagField.TagField19 != "") { ListCustomAttribute.Add(new Variable { Code = "19", Value = tagField.TagField19 }); }
                if (tagField.TagField20 != "") { ListCustomAttribute.Add(new Variable { Code = "20", Value = tagField.TagField20 }); }
            }
            return ListCustomAttribute;
        }
    }
}