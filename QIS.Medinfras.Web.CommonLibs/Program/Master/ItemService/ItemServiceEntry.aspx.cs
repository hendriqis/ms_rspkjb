using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.MasterPage;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemServiceEntry : BasePageEntry
    {

        public override string OnGetMenuCode()
        {
            String menuID = Request.QueryString["menu"];
            string[] param = menuID.Split('|');

            switch (param[0])
            {
                case "FN": return Constant.MenuCode.Finance.ITEM_SERVICE_FN;
                case "IS": return Constant.MenuCode.Imaging.ITEM_SERVICE_IS;
                case "MD":
                    hdnSubMenuType.Value = param[1];
                    if (param[1] == "RT")
                    {
                        return Constant.MenuCode.Radiotheraphy.ITEM_SERVICE_MD_RADIOTHERAPHY;
                    }
                    else
                    {
                        return Constant.MenuCode.MedicalDiagnostic.ITEM_SERVICE_MD;
                    }
                case "MC": return Constant.MenuCode.MedicalCheckup.ITEM_SERVICE_MC;
                default: return Constant.MenuCode.Finance.ITEM_SERVICE_FN;
            }
        }

        protected string GetPageTitle()
        {
            string menuCode = OnGetMenuCode();
            MenuMaster menu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", menuCode)).FirstOrDefault();
            if (menu != null)
                return GetLabel(menu.MenuCaption);
            return "";
        }

        protected override void InitializeDataControl()
        {
            String menuID = Request.QueryString["menu"];
            string[] param = menuID.Split('|');
            switch (param[0])
            {
                case "FN": hdnGCItemType.Value = Constant.ItemType.PELAYANAN;
                    trModality.Style.Add("display", "none");
                    trModalities.Style.Add("display", "none");
                    trTariffScheme.Style.Add("display", "none");
                    break;
                case "IS":
                    hdnGCItemType.Value = Constant.ItemType.RADIOLOGI;
                    trOperationType.Style.Add("display", "none");
                    trBloodComponent.Style.Add("display", "none");
                    tdBloodBankItem.Style.Add("display", "none");
                    trTariffScheme.Style.Add("display", "none");
                    break;
                case "MD":
                    hdnGCItemType.Value = Constant.ItemType.PENUNJANG_MEDIS;
                    trTariffScheme.Style.Add("display", "none");
                    if (param[1] == "RT")
                    {
                        hdnGCSubItemType.Value = Constant.SubItemType.RADIOTERAPI;
                        trModality.Style.Add("display", "none");
                        trModalities.Style.Add("display", "none");
                        trOperationType.Style.Add("display", "none");
                        trBloodComponent.Style.Add("display", "none");
                        tdBloodBankItem.Style.Add("display", "none");
                    }
                    break;
                case "MC": hdnGCItemType.Value = Constant.ItemType.MEDICAL_CHECKUP;
                    trIsTestItem.Style.Add("display", "none");
                    trModality.Style.Add("display", "none");
                    trModalities.Style.Add("display", "none");
                    trOperationType.Style.Add("display", "none");
                    trItemPackageAIO.Style.Add("display", "none");
                    tdBloodBankItem.Style.Add("display", "none");
                    trTariffScheme.Style.Remove("display");
                    break;
                default: hdnGCItemType.Value = Constant.ItemType.PELAYANAN;
                    trModality.Style.Add("display", "none");
                    trModalities.Style.Add("display", "none");
                    trOperationType.Style.Add("display", "none");
                    trBloodComponent.Style.Add("display", "none");
                    tdBloodBankItem.Style.Add("display", "none");
                    trTariffScheme.Style.Add("display", "none");
                    break;
            }

            if (Request.QueryString.Count > 1)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;

                SetControlProperties();
                vItemMaster entity = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", ID)).FirstOrDefault();
                vItemService entityService = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", ID)).FirstOrDefault();
                ItemTagField entityTagField = BusinessLayer.GetItemTagField(entity.ItemID);
                EntityToControl(entity, entityService, entityTagField);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;

                SetControlEntrySetting(chkIsPackageItem, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsPackageAllInOne, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsAccumulatedPrice, new ControlEntrySetting(false, false, false));


            }
            txtItemName1.Focus();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY));
            hdnIsEKlaimParameterMandatory.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY).FirstOrDefault().ParameterValue;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            if (AppSession.SA0198)
            {
                h4PaketKunjungan.Attributes.Remove("style");
                divPaketKunjungan.Attributes.Remove("style");
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
        }

        protected override void SetControlProperties()
        {
            String filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0 AND IsActive = 1",
                                                            Constant.StandardCode.ITEM_UNIT, //0
                                                            Constant.StandardCode.ITEM_STATUS, //1
                                                            Constant.StandardCode.MEDICAL_IMAGING_MODALITIES, //2
                                                            Constant.StandardCode.SURGERY_CLASSIFICATION, //3
                                                            Constant.StandardCode.BLOOD_BANK_TYPE, //4
                                                            Constant.StandardCode.TARIFF_SCHEME //5
                                                        );
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(filterExpression);

            Methods.SetComboBoxField<StandardCode>(cboTariffScheme, lstSC.Where(p => p.ParentID == Constant.StandardCode.TARIFF_SCHEME).ToList<StandardCode>(), "StandardCodeName", "StandardCodeID");
            cboTariffScheme.Value = Constant.TariffScheme.Standard;

            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstSC.Where(a => a.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboItemStatus, lstSC.Where(a => a.ParentID == Constant.StandardCode.ITEM_STATUS).ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboModalities, lstSC.Where(a => a.ParentID == Constant.StandardCode.MEDICAL_IMAGING_MODALITIES || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboSurgeryClassification, lstSC.Where(a => a.ParentID == Constant.StandardCode.SURGERY_CLASSIFICATION || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            Methods.SetComboBoxField<StandardCode>(cboBloodComponentType, lstSC.Where(a => a.ParentID == Constant.StandardCode.BLOOD_BANK_TYPE || a.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<Variable> lstVar = new List<Variable>(){ 
                new Variable{ Code = "1",Value = "1"},
                new Variable{ Code = "2",Value = "2"},
                new Variable{ Code = "3",Value = "3"}};
            Methods.SetComboBoxField<Variable>(cboDefaultTariffComp, lstVar, "Code", "Value");
            cboDefaultTariffComp.SelectedIndex = 0;

            SettingParameterDt setvarDt = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_MASTER_ITEM_SERVICE_USED_DEFAULT_TARIFF_COMP);
            if (setvarDt.ParameterValue == "1")
            {
                trDefaultKomponenTariff.Style.Remove("display");
            }
            else
            {
                trDefaultKomponenTariff.Style.Add("display", "none");
            }
        }

        protected override void OnControlEntrySetting()
        {
            if (hdnGCItemType.Value == Constant.ItemType.MEDICAL_CHECKUP)
            {
                SetControlEntrySetting(cboTariffScheme, new ControlEntrySetting(true, true, true));
            }
            else
            {
                SetControlEntrySetting(cboTariffScheme, new ControlEntrySetting(true, true, false));
            }

            if (hdnGCItemType.Value == Constant.ItemType.RADIOLOGI)
            {
                SetControlEntrySetting(chkIsTestItem, new ControlEntrySetting(true, true, false, true));
                chkIsTestItem.Checked = true;
            }
            else
            {
                SetControlEntrySetting(chkIsTestItem, new ControlEntrySetting(true, true, false));
            }

            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOldItemCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAlternateItemName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnModalityID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtModalityCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtModalityName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(cboModalities, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboDefaultTariffComp, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboItemStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboSurgeryClassification, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnDefaultParamedicID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtDefaultParamedicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDefaultParamedicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnBillingGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBillingGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingGroupName, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(hdnEKlaimParameterID, new ControlEntrySetting(true, true));
            //SetControlEntrySetting(txtEKlaimParameterCode, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtEKlaimParameterName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtGCRLReportGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGCRLReportGroupName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRegistrationReceiptLabel, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsRegistrationReceiptItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowCITO, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowComplication, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsAllowVariable, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp1, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp2, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp3, new ControlEntrySetting(true, true, false, false));

            SetControlEntrySetting(chkIsAllowDiscount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp1, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp2, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp3, new ControlEntrySetting(true, true, false, false));

            SetControlEntrySetting(chkIsAssetUtilization, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowRevenueSharing, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsQtyAllowChangeForDoctor, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsAllowChangeQty, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsRevenueSharingFee1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsRevenueSharingFee2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsRevenueSharingFee3, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIncludeInAdminCalculation, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsPrintWithDoctorName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSubContractItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsUnbilledItem, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsSpecialItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsIncludeRevenueSharingTax, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBloodBankItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBPJS, new ControlEntrySetting(true, true, false));
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

        private void EntityToControl(vItemMaster entity, vItemService entityService, ItemTagField entityTagField)
        {
            txtItemCode.Text = entity.ItemCode;
            txtOldItemCode.Text = entity.OldItemCode;
            txtItemName1.Text = entity.ItemName1;
            txtItemName2.Text = entity.ItemName2;
            txtAlternateItemName.Text = entity.AlternateItemName;
            hdnModalityID.Value = entityService.ModalityID.ToString();
            txtModalityCode.Text = entityService.ModalityCode;
            txtModalityName.Text = entityService.ModalityName;
            if (entityService.GCModality != null)
            {
                cboModalities.Value = entityService.GCModality;
            }
            cboItemUnit.Value = entity.GCItemUnit;
            cboItemStatus.Value = entity.GCItemStatus;
            if (entityService.GCSurgeryClassification != null)
            {
                cboSurgeryClassification.Value = entityService.GCSurgeryClassification;
            }
            if (entityService.GCBloodComponentType != null)
            {
                cboBloodComponentType.Value = entityService.GCBloodComponentType;
            }
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            if (hdnProductLineID.Value != null)
            {
                txtProductLineCode.Text = entity.ProductLineCode;
                txtProductLineName.Text = entity.ProductLineName;
            }
            hdnRevenueSharingID.Value = entityService.RevenueSharingID.ToString();
            txtRevenueSharingCode.Text = entityService.RevenueSharingCode;
            txtRevenueSharingName.Text = entityService.RevenueSharingName;
            txtRemarks.Text = entity.Remarks;
            hdnDefaultParamedicID.Value = entity.DefaultParamedicID.ToString();
            txtDefaultParamedicCode.Text = entity.DefaultParamedicCode;
            txtDefaultParamedicName.Text = entity.DefaultParamedicName;

            hdnBillingGroupID.Value = entity.BillingGroupID.ToString();
            txtBillingGroupCode.Text = entity.BillingGroupCode;
            txtBillingGroupName.Text = entity.BillingGroupName1;

            hdnEKlaimParameterID.Value = entity.EKlaimParameterID.ToString();
            txtEKlaimParameterCode.Text = entity.EKlaimParameterCode;
            txtEKlaimParameterName.Text = entity.EKlaimParameterName;

            txtKodeJenPelRajal.Text = entity.InhealthKodeJenPelRajal;
            txtKodeJenPelRanap.Text = entity.InhealthKodeJenPelRanap;
            chkIsInhealthRanapAkomodasi.Checked = entity.IsInhealthRanapAkomodasi;

            hdnGCRLReportGroup.Value = entityService.GCRLReportGroupCode;
            txtGCRLReportGroupCode.Text = entityService.GCRLReportGroupCode;
            txtGCRLReportGroupName.Text = entityService.GCRLReportGroupName;

            txtRegistrationReceiptLabel.Text = entityService.RegistrationReceiptLabel;
            chkIsRegistrationReceiptItem.Checked = entityService.IsRegistrationReceiptItem;
            chkIsAllowCITO.Checked = entityService.IsAllowCito;
            chkIsAllowComplication.Checked = entityService.IsAllowComplication;
            chkIsUsingSpecialMarkup.Checked = entityService.IsUsingSpecialMarkup;

            chkIsAllowVariable.Checked = entityService.IsAllowVariable;
            chkIsAllowVariableTariffComp1.Checked = entityService.IsAllowVariableTariffComp1;
            chkIsAllowVariableTariffComp2.Checked = entityService.IsAllowVariableTariffComp2;
            chkIsAllowVariableTariffComp3.Checked = entityService.IsAllowVariableTariffComp3;

            chkIsAllowDiscount.Checked = entityService.IsAllowDiscount;
            chkIsAllowDiscountTariffComp1.Checked = entityService.IsAllowDiscountTariffComp1;
            chkIsAllowDiscountTariffComp2.Checked = entityService.IsAllowDiscountTariffComp2;
            chkIsAllowDiscountTariffComp3.Checked = entityService.IsAllowDiscountTariffComp3;

            chkIsAssetUtilization.Checked = entityService.IsAssetUtilization;
            chkIsAllowRevenueSharing.Checked = entityService.IsAllowRevenueSharing;
            chkIsQtyAllowChangeForDoctor.Checked = entityService.IsQtyAllowChangeForDoctor;
            chkIsAllowChangeQty.Checked = entityService.IsAllowChangeQty;
            chkIsRevenueSharingFee1.Checked = entityService.IsRevenueSharingFee1;
            chkIsRevenueSharingFee2.Checked = entityService.IsRevenueSharingFee2;
            chkIsRevenueSharingFee3.Checked = entityService.IsRevenueSharingFee3;
            chkIsIncludeInAdminCalculation.Checked = entity.IsIncludeInAdminCalculation;
            chkIsPrintWithDoctorName.Checked = entityService.IsPrintWithDoctorName;
            chkIsSubContractItem.Checked = entityService.IsSubContractItem;
            chkIsUnbilledItem.Checked = entityService.IsUnbilledItem;
            chkIsPackageBalanceItem.Checked = entityService.IsPackageBalanceItem;
            txtDefaultPackageBalanceItemQty.Text = entityService.DefaultPackageBalanceQty.ToString();

            if (!chkIsPackageBalanceItem.Checked)
            {
                tdDefaultPackageBalanceItemQty.Style.Add("display", "none");
            }
            else
            {
                tdDefaultPackageBalanceItemQty.Style.Remove("display");
            }

            chkIsPackageItem.Checked = entityService.IsPackageItem;
            chkIsPackageAllInOne.Checked = entityService.IsPackageAllInOne;
            chkIsAccumulatedPrice.Checked = entityService.IsUsingAccumulatedPrice;

            if (entityService.IsPackageItem || entityService.IsPackageAllInOne)
            {
                if (entityService.IsPackageItem)
                {
                    SetControlEntrySetting(chkIsPackageAllInOne, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(chkIsAccumulatedPrice, new ControlEntrySetting(false, true, false));
                }

                if (entityService.IsPackageAllInOne)
                {
                    SetControlEntrySetting(chkIsPackageItem, new ControlEntrySetting(false, false, false));
                    SetControlEntrySetting(chkIsAccumulatedPrice, new ControlEntrySetting(false, false, false));
                }
            }
            else
            {
                SetControlEntrySetting(chkIsPackageItem, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsPackageAllInOne, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsAccumulatedPrice, new ControlEntrySetting(false, false, false));
            }

            chkIsSpecialItem.Checked = entityService.IsSpecialItem;
            chkIsBPJS.Checked = entityService.IsBPJS;
            chkIsUsingProcedureCoding.Checked = entityService.IsUsingProcedureCoding;
            chkIsIncludeRevenueSharingTax.Checked = entityService.IsIncludeRevenueSharingTax;
            chkIsBloodBankItem.Checked = entityService.IsBloodBankItem;

            //if (hdnGCItemType.Value != Constant.ItemType.PELAYANAN)
            //{
            chkIsTestItem.Checked = entityService.IsTestItem;
            //}

            cboTariffScheme.Value = entityService.GCTariffScheme;
            cboDefaultTariffComp.Value = entityService.DefaultTariffComp;

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                txt.Text = entityTagField.GetType().GetProperty("TagField" + hdn.Value).GetValue(entityTagField, null).ToString();
            }
            #endregion
        }

        private void ControlToEntity(ItemMaster entity, ItemService entityService, ItemTagField entityTagField)
        {
            entity.GCItemType = hdnGCItemType.Value;
            if (!string.IsNullOrEmpty(hdnGCSubItemType.Value))
            {
                entity.GCSubItemType = hdnGCSubItemType.Value;
            }
            entity.OldItemCode = txtOldItemCode.Text;
            entity.ItemName1 = txtItemName1.Text;
            entity.ItemName2 = txtItemName2.Text;
            entity.AlternateItemName = txtAlternateItemName.Text;
            if (hdnModalityID.Value == "" || hdnModalityID.Value == "0")
            {
                entityService.ModalityID = null;
            }
            else
            {
                entityService.ModalityID = Convert.ToInt32(hdnModalityID.Value);
            }
            if (cboModalities.Value == null)
            {
                entityService.GCModality = null;
            }
            else
            {
                entityService.GCModality = cboModalities.Value.ToString();
            }
            entity.GCItemUnit = cboItemUnit.Value.ToString();
            entity.GCItemStatus = cboItemStatus.Value.ToString();
            if (cboSurgeryClassification.Value == null)
            {
                entityService.GCSurgeryClassification = null;
            }
            else
            {
                entityService.GCSurgeryClassification = cboSurgeryClassification.Value.ToString();
            }

            if (cboBloodComponentType.Value != null)
            {
                if (!string.IsNullOrEmpty(cboBloodComponentType.Value.ToString()))
                    entityService.GCBloodComponentType = cboBloodComponentType.Value.ToString();
            }
            else
            {
                entityService.GCBloodComponentType = null;
            }

            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            if (hdnProductLineID.Value == "" || hdnProductLineID.Value == "0")
            {
                entity.ProductLineID = null;
            }
            else
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            if (hdnRevenueSharingID.Value == "" || hdnRevenueSharingID.Value == "0")
            {
                entityService.RevenueSharingID = null;
            }
            else
            {
                entityService.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            entity.Remarks = txtRemarks.Text;

            if (hdnDefaultParamedicID.Value == "" || hdnDefaultParamedicID.Value == "0")
            {
                entity.DefaultParamedicID = null;
            }
            else
            {
                entity.DefaultParamedicID = Convert.ToInt32(hdnDefaultParamedicID.Value);
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

            if (hdnGCRLReportGroup.Value == "" || hdnGCRLReportGroup.Value == "0")
            {
                entityService.GCRLReportGroup = null;
            }
            else
            {
                entityService.GCRLReportGroup = hdnGCRLReportGroup.Value;
            }

            entity.InhealthKodeJenPelRajal = txtKodeJenPelRajal.Text;
            entity.InhealthKodeJenPelRanap = txtKodeJenPelRanap.Text;
            entity.IsInhealthRanapAkomodasi = chkIsInhealthRanapAkomodasi.Checked;

            entityService.RegistrationReceiptLabel = txtRegistrationReceiptLabel.Text;
            entityService.IsRegistrationReceiptItem = chkIsRegistrationReceiptItem.Checked;
            entityService.IsAllowCito = chkIsAllowCITO.Checked;
            entityService.IsAllowComplication = chkIsAllowComplication.Checked;
            entityService.IsUsingSpecialMarkup = chkIsUsingSpecialMarkup.Checked;

            if (chkIsAllowVariableTariffComp1.Checked || chkIsAllowVariableTariffComp2.Checked || chkIsAllowVariableTariffComp3.Checked)
            {
                entityService.IsAllowVariable = true;
            }
            else
            {
                entityService.IsAllowVariable = false;
            }
            entityService.IsAllowVariableTariffComp1 = chkIsAllowVariableTariffComp1.Checked;
            entityService.IsAllowVariableTariffComp2 = chkIsAllowVariableTariffComp2.Checked;
            entityService.IsAllowVariableTariffComp3 = chkIsAllowVariableTariffComp3.Checked;

            if (chkIsAllowDiscountTariffComp1.Checked || chkIsAllowDiscountTariffComp2.Checked || chkIsAllowDiscountTariffComp3.Checked)
            {
                entityService.IsAllowDiscount = true;
            }
            else
            {
                entityService.IsAllowDiscount = false;
            }
            entityService.IsAllowDiscountTariffComp1 = chkIsAllowDiscountTariffComp1.Checked;
            entityService.IsAllowDiscountTariffComp2 = chkIsAllowDiscountTariffComp2.Checked;
            entityService.IsAllowDiscountTariffComp3 = chkIsAllowDiscountTariffComp3.Checked;

            entityService.IsAssetUtilization = chkIsAssetUtilization.Checked;
            entityService.IsAllowRevenueSharing = chkIsAllowRevenueSharing.Checked;
            entityService.IsQtyAllowChangeForDoctor = chkIsQtyAllowChangeForDoctor.Checked;
            entityService.IsAllowChangeQty = chkIsAllowChangeQty.Checked;
            entityService.IsRevenueSharingFee1 = chkIsRevenueSharingFee1.Checked;
            entityService.IsRevenueSharingFee2 = chkIsRevenueSharingFee2.Checked;
            entityService.IsRevenueSharingFee3 = chkIsRevenueSharingFee3.Checked;
            entity.IsIncludeInAdminCalculation = chkIsIncludeInAdminCalculation.Checked;
            entityService.IsPrintWithDoctorName = chkIsPrintWithDoctorName.Checked;
            entityService.IsSubContractItem = chkIsSubContractItem.Checked;
            entityService.IsUnbilledItem = chkIsUnbilledItem.Checked;
            entityService.IsTestItem = chkIsTestItem.Checked;
            entityService.IsPackageItem = chkIsPackageItem.Checked;
            entityService.IsPackageAllInOne = chkIsPackageAllInOne.Checked;
            entityService.IsUsingAccumulatedPrice = chkIsAccumulatedPrice.Checked;
            entityService.IsSpecialItem = chkIsSpecialItem.Checked;
            entityService.IsBPJS = chkIsBPJS.Checked;
            entityService.IsUsingProcedureCoding = chkIsUsingProcedureCoding.Checked;
            entityService.IsIncludeRevenueSharingTax = chkIsIncludeRevenueSharingTax.Checked;
            entityService.IsBloodBankItem = chkIsBloodBankItem.Checked;

            if (cboTariffScheme.Value != null)
            {
                entityService.GCTariffScheme = cboTariffScheme.Value.ToString();
            }

            entityService.DefaultTariffComp = Convert.ToInt16(cboDefaultTariffComp.Value);

            entityService.IsPackageBalanceItem = chkIsPackageBalanceItem.Checked;

            if (entityService.IsPackageBalanceItem)
            {
                entityService.DefaultPackageBalanceQty = Convert.ToInt32(txtDefaultPackageBalanceItemQty.Text);
            }
            else
            {
                entityService.DefaultPackageBalanceQty = 0;
            }

            #region Custom Attribute
            foreach (RepeaterItem item in rptCustomAttribute.Items)
            {
                TextBox txt = (TextBox)item.FindControl("txtTagField");
                HtmlInputHidden hdn = (HtmlInputHidden)item.FindControl("hdnTagFieldCode");
                entityTagField.GetType().GetProperty("TagField" + hdn.Value).SetValue(entityTagField, txt.Text, null);
            }
            #endregion
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            //errMessage = string.Empty;

            //string FilterExpression = string.Format("ItemCode = '{0}'", txtItemCode.Text);
            //List<ItemMaster> lst = BusinessLayer.GetItemMasterList(FilterExpression);

            //if (lst.Count > 0)
            //    errMessage = " Item with Code " + txtItemCode.Text + " is already exist!";

            //return (errMessage == string.Empty);
            return true;
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            //errMessage = string.Empty;
            //string FilterExpression = string.Format("ItemCode = '{0}' AND ItemID != {1}", txtItemCode.Text, hdnID.Value);
            //List<ItemMaster> lst = BusinessLayer.GetItemMasterList(FilterExpression);

            //if (lst.Count > 0)
            //    errMessage = " Item with Code " + txtItemCode.Text + " is already exist!";

            //return (errMessage == string.Empty);
            return true;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemServiceDao entityServiceDao = new ItemServiceDao(ctx);
            ItemCostDao entityCostDao = new ItemCostDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = new ItemMaster();
                ItemService entityService = new ItemService();
                ItemTagField entityTagField = new ItemTagField();

                ControlToEntity(entity, entityService, entityTagField);

                entity.ItemCode = Helper.GenerateItemCode(ctx, entity.ItemName1);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                int oItemID = entityDao.InsertReturnPrimaryKeyID(entity);

                if (hdnGCItemType.Value == Constant.ItemGroupMaster.SERVICE || hdnGCItemType.Value == Constant.ItemGroupMaster.MEDICAL_CHECKUP)
                    entityService.IsTestItem = false;

                entityService.ItemID = oItemID;
                entityServiceDao.Insert(entityService);

                entityTagField.ItemID = entityService.ItemID;
                entityTagFieldDao.Insert(entityTagField);

                List<Healthcare> lstHealthcare = BusinessLayer.GetHealthcareList("", ctx);
                foreach (Healthcare healthcare in lstHealthcare)
                {
                    ItemCost ic = new ItemCost();
                    ic.ItemID = entityService.ItemID;
                    ic.HealthcareID = healthcare.HealthcareID;
                    ic.CreatedBy = AppSession.UserLogin.UserID;
                    entityCostDao.Insert(ic);
                }

                retval = entityService.ItemID.ToString();
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
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemServiceDao entityServiceDao = new ItemServiceDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ItemService entityService = entityServiceDao.Get(Convert.ToInt32(hdnID.Value));
                ItemTagField entityTagField = entityTagFieldDao.Get(entity.ItemID);

                bool flagETFNull = true;
                if (entityTagField == null)
                {
                    entityTagField = new ItemTagField();
                }
                else flagETFNull = false;

                ControlToEntity(entity, entityService, entityTagField);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                entityService.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityServiceDao.Update(entityService);

                if (flagETFNull)
                {
                    entityTagField.ItemID = entity.ItemID;
                    entityTagFieldDao.Insert(entityTagField);
                }
                else
                {
                    entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTagFieldDao.Update(entityTagField);
                }

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