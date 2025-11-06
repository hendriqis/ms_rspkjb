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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class ItemLaboratoryEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.ITEM_SERVICE_LB;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnGCItemType.Value = Constant.ItemType.LABORATORIUM;
            if (Request.QueryString.Count > 0)
            {
                IsAdd = false;
                String ID = Request.QueryString["id"];
                hdnID.Value = ID;
                SetControlProperties();
                vItemMaster entity = BusinessLayer.GetvItemMasterList(string.Format("ItemID = {0}", ID)).FirstOrDefault();
                vItemService entityService = BusinessLayer.GetvItemServiceList(string.Format("ItemID = {0}", entity.ItemID)).FirstOrDefault();
                ItemTagField entityTagField = BusinessLayer.GetItemTagField(entity.ItemID);
                ItemLaboratory entityLab = BusinessLayer.GetItemLaboratory(entity.ItemID);
                EntityToControl(entity, entityService, entityLab, entityTagField);
            }
            else
            {
                SetControlProperties();
                IsAdd = true;

                SetControlEntrySetting(chkIsPackageItem, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsPackageAllInOne, new ControlEntrySetting(true, true, false));
                SetControlEntrySetting(chkIsAccumulatedPrice, new ControlEntrySetting(false, false, false));
            }
            txtItemCode.Focus();

            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID='{0}' AND ParameterCode IN ('{1}')",
                AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY));
            hdnIsEKlaimParameterMandatory.Value = lstSettingParameterDt.Where(lstDt => lstDt.ParameterCode == Constant.SettingParameter.FN_IS_EKLAIM_PARAMETER_MANDATORY).FirstOrDefault().ParameterValue;

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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstSC = BusinessLayer.GetStandardCodeList(string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND IsDeleted = 0",
                                                                                            Constant.StandardCode.ITEM_UNIT,
                                                                                            Constant.StandardCode.ITEM_STATUS,
                                                                                            Constant.StandardCode.LABORATORY_TEST_CATEGORY,
                                                                                            Constant.StandardCode.PATOLOGY_GROUP,
                                                                                            Constant.StandardCode.eHAC_TEST_TYPE,
                                                                                            Constant.StandardCode.BLOOD_BANK_TYPE));
            Methods.SetComboBoxField<StandardCode>(cboItemUnit, lstSC.Where(p => p.ParentID == Constant.StandardCode.ITEM_UNIT).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboItemStatus, lstSC.Where(p => p.ParentID == Constant.StandardCode.ITEM_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboLabTestCategory, lstSC.Where(p => p.ParentID == Constant.StandardCode.LABORATORY_TEST_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");//field ke 3 untuk yang ditampilin ke layar, field ke 4 itu yg dikirim untuk diproses
            Methods.SetComboBoxField<StandardCode>(cboeHACTestType, lstSC.Where(p => p.ParentID == Constant.StandardCode.eHAC_TEST_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboGCBloodBankType, lstSC.Where(p => p.ParentID == Constant.StandardCode.BLOOD_BANK_TYPE).ToList(), "StandardCodeName", "StandardCodeID");

            lstSC.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField<StandardCode>(cboPatologyGroup, lstSC.Where(p => p.ParentID == Constant.StandardCode.PATOLOGY_GROUP || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            cboPatologyGroup.SelectedIndex = 0;
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

        protected override void OnControlEntrySetting()
        {
            #region ItemInformation
            SetControlEntrySetting(txtItemCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtOldItemCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtItemName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAlternateItemName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtDuration, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboItemUnit, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboItemStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboLabTestCategory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboeHACTestType, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnItemGroupID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnGCRLReportGroup, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGCRLReportGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGCRLReportGroupName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnDefaultParamedicID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtDefaultParamedicCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtDefaultParamedicName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnBillingGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBillingGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingGroupName, new ControlEntrySetting(false, false, false));
            //SetControlEntrySetting(hdnEKlaimParameterID, new ControlEntrySetting(true, true));
            //SetControlEntrySetting(txtEKlaimParameterCode, new ControlEntrySetting(true, true, false));
            //SetControlEntrySetting(txtEKlaimParameterName, new ControlEntrySetting(false, false, false));
            #endregion

            #region Precondition Notes
            SetControlEntrySetting(txtPreconditionNotes, new ControlEntrySetting(true, true, false));
            #endregion

            #region Item Status
            SetControlEntrySetting(chkIsAllowCITO, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsAllowDiscount, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowDiscountTariffComp3, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsAllowVariable, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowVariableTariffComp3, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(chkIsIncludeInAdminCalculation, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsPrintWithDoctorName, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsAllowRevenueSharing, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsQtyAllowChangeForDoctor, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsRevenueSharingFee1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsRevenueSharingFee2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsRevenueSharingFee3, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsSubContractItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsTestItem, new ControlEntrySetting(true, true, false, true));
            SetControlEntrySetting(chkIsBridgingItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsBPJS, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsUnbilledItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(ChkIsPPRA, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsPGxTest, new ControlEntrySetting(true, true, false, false));
            SetControlEntrySetting(chkIsBridgingToeHAC, new ControlEntrySetting(true, true, false, false));
            #endregion
        }

        private void EntityToControl(vItemMaster entity, vItemService entityService, ItemLaboratory entityLab, ItemTagField entityTagField)
        {
            #region Item Information
            txtItemCode.Text = entity.ItemCode;
            txtOldItemCode.Text = entity.OldItemCode;
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName.Text = entity.ItemGroupName1;
            txtItemName1.Text = entity.ItemName1;
            txtItemName2.Text = entity.ItemName2;
            txtAlternateItemName.Text = entity.AlternateItemName;
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            if (hdnProductLineID.Value != null)
            {
                txtProductLineCode.Text = entity.ProductLineCode;
                txtProductLineName.Text = entity.ProductLineName;
            }
            txtRemarks.Text = entity.Remarks;
            txtRevenueSharingCode.Text = entityService.RevenueSharingCode;
            txtRevenueSharingName.Text = entityService.RevenueSharingName;
            txtDuration.Text = entityLab.Duration.ToString();
            cboItemUnit.Value = entityService.GCItemUnit;
            cboItemStatus.Value = entity.GCItemStatus;
            cboPatologyGroup.Value = entityLab.GCPatologyGroup;
            cboLabTestCategory.Value = entityLab.GCLabTestCategory;
            cboeHACTestType.Value = entityLab.GCeHACTestType;
            hdnItemGroupID.Value = entity.ItemGroupID.ToString();
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            hdnRevenueSharingID.Value = entityService.RevenueSharingID.ToString();

            hdnGCRLReportGroup.Value = entityService.GCRLReportGroupCode;
            txtGCRLReportGroupCode.Text = entityService.GCRLReportGroupCode;
            txtGCRLReportGroupName.Text = entityService.GCRLReportGroupName;

            hdnDefaultParamedicID.Value = entity.DefaultParamedicID.ToString();
            txtDefaultParamedicCode.Text = entity.DefaultParamedicCode;
            txtDefaultParamedicName.Text = entity.DefaultParamedicName;
            hdnBillingGroupID.Value = entity.BillingGroupID.ToString();
            if (hdnBillingGroupID.Value != "0")
            {
                BillingGroup entityBilling = BusinessLayer.GetBillingGroupList(string.Format("BillingGroupID = {0}", entity.BillingGroupID.ToString())).FirstOrDefault();
                txtBillingGroupCode.Text = entityBilling.BillingGroupCode;
                txtBillingGroupName.Text = entityBilling.BillingGroupName1;
            }
            txtKodeJenPelRajal.Text = entity.InhealthKodeJenPelRajal;
            txtKodeJenPelRanap.Text = entity.InhealthKodeJenPelRanap;
            chkIsInhealthRanapAkomodasi.Checked = entity.IsInhealthRanapAkomodasi;

            hdnEKlaimParameterID.Value = entity.EKlaimParameterID.ToString();
            txtEKlaimParameterCode.Text = entity.EKlaimParameterCode;
            txtEKlaimParameterName.Text = entity.EKlaimParameterName;

            chkIsBloodBankOrder.Checked = entityLab.IsBloodBankOrder;
            cboGCBloodBankType.Value = entityLab.GCBloodBankType;
            #endregion

            #region Precondition Notes
            //txtPrintOrder.Text = entityLab
            txtPreconditionNotes.Text = entityLab.PreconditionNotes;
            txtPrintOrder.Text = entityLab.PrintOrder.ToString();
            #endregion

            #region Item Status
            chkIsAllowCITO.Checked = entityService.IsAllowCito;
            chkIsAllowVariable.Checked = entityService.IsAllowVariable;
            chkIsAllowVariableTariffComp1.Checked = entityService.IsAllowVariableTariffComp1;
            chkIsAllowVariableTariffComp2.Checked = entityService.IsAllowVariableTariffComp2;
            chkIsAllowVariableTariffComp3.Checked = entityService.IsAllowVariableTariffComp3;
            chkIsAllowDiscount.Checked = entityService.IsAllowDiscount;
            chkIsAllowDiscountTariffComp1.Checked = entityService.IsAllowDiscountTariffComp1;
            chkIsAllowDiscountTariffComp2.Checked = entityService.IsAllowDiscountTariffComp2;
            chkIsAllowDiscountTariffComp3.Checked = entityService.IsAllowDiscountTariffComp3;
            chkIsIncludeInAdminCalculation.Checked = entity.IsIncludeInAdminCalculation;
            chkIsPrintWithDoctorName.Checked = entityService.IsPrintWithDoctorName;
            chkIsAllowRevenueSharing.Checked = entityService.IsAllowRevenueSharing;
            chkIsQtyAllowChangeForDoctor.Checked = entityService.IsQtyAllowChangeForDoctor;
            chkIsRevenueSharingFee1.Checked = entityService.IsRevenueSharingFee1;
            chkIsRevenueSharingFee2.Checked = entityService.IsRevenueSharingFee2;
            chkIsRevenueSharingFee3.Checked = entityService.IsRevenueSharingFee3;
            chkIsSubContractItem.Checked = entityService.IsSubContractItem;

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

            chkIsTestItem.Checked = entityService.IsTestItem;
            chkIsBridgingItem.Checked = entityService.IsBridgingItem;
            chkIsBPJS.Checked = entityService.IsBPJS;
            chkIsUnbilledItem.Checked = entityService.IsUnbilledItem;
            ChkIsPPRA.Checked = entityLab.IsPPRAFlag;
            chkIsPGxTest.Checked = entityLab.IsPGxTest;
            chkIsBridgingToeHAC.Checked = entityLab.IsLinkedToeHAC;
            chkIsSpotCheckItem.Checked = entityLab.IsSpotCheckItem; 
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

        private void ControlToEntity(ItemMaster entity, ItemService entityService, ItemLaboratory entityLab, ItemTagField entityTagField)
        {
            #region Item Information
            entity.OldItemCode = txtOldItemCode.Text;
            entity.ItemName1 = txtItemName1.Text;
            entity.ItemName2 = txtItemName2.Text;
            entity.AlternateItemName = txtAlternateItemName.Text;
            entity.ItemGroupID = Convert.ToInt32(hdnItemGroupID.Value);
            if (hdnProductLineID.Value == "" || hdnProductLineID.Value == "0")
            {
                entity.ProductLineID = null;
            }
            else
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
            entity.Remarks = txtRemarks.Text;
            if (hdnRevenueSharingID.Value == "0" || hdnRevenueSharingID.Value == "")
                entityService.RevenueSharingID = null;
            else
                entityService.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            entityLab.Duration = Convert.ToByte(txtDuration.Text);
            entity.GCItemUnit = cboItemUnit.Value.ToString();

            if (cboPatologyGroup.Value == null)
            {
                entityLab.GCPatologyGroup = null;
            }
            else
            {
                entityLab.GCPatologyGroup = cboPatologyGroup.Value.ToString();
            }

            entity.GCItemStatus = cboItemStatus.Value.ToString();
            entityLab.GCLabTestCategory = cboLabTestCategory.Value.ToString();
            if (cboeHACTestType.Value != null)
            {
                entityLab.GCeHACTestType = cboeHACTestType.Value.ToString();
            }

            entityLab.IsBloodBankOrder = chkIsBloodBankOrder.Checked;
            if (cboGCBloodBankType.Value != null)
            {
                entityLab.GCBloodBankType = cboGCBloodBankType.Value.ToString();
            }

            if (hdnGCRLReportGroup.Value == "" || hdnGCRLReportGroup.Value == "0")
            {
                entityService.GCRLReportGroup = null;
            }
            else
            {
                entityService.GCRLReportGroup = hdnGCRLReportGroup.Value;
            }

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
            entity.InhealthKodeJenPelRajal = txtKodeJenPelRajal.Text;
            entity.InhealthKodeJenPelRanap = txtKodeJenPelRanap.Text;
            entity.IsInhealthRanapAkomodasi = chkIsInhealthRanapAkomodasi.Checked;
            //if (!string.IsNullOrEmpty(hdnEKlaimParameterID.Value) && hdnEKlaimParameterID.Value != "0") {
            //    entity.EKlaimParameterID = Convert.ToInt32(hdnEKlaimParameterID.Value);
            //}

            if (hdnEKlaimParameterID.Value == "" || hdnEKlaimParameterID.Value == "0")
            {
                entity.EKlaimParameterID = null;
            }
            else
            {
                entity.EKlaimParameterID = Convert.ToInt32(hdnEKlaimParameterID.Value);
            }
           
            #endregion

            #region Precondition Notes
            entityLab.PreconditionNotes = Helper.GetHTMLEditorText(txtPreconditionNotes);
            decimal printOrder = 0;
            if (!string.IsNullOrEmpty(txtPrintOrder.Text)) printOrder = Convert.ToDecimal(txtPrintOrder.Text);
            entityLab.PrintOrder = printOrder;
            #endregion

            #region Item Status
            entityService.IsAllowCito = chkIsAllowCITO.Checked;

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

            entity.IsIncludeInAdminCalculation = chkIsIncludeInAdminCalculation.Checked;
            entityService.IsPrintWithDoctorName = chkIsPrintWithDoctorName.Checked;
            entityService.IsAllowRevenueSharing = chkIsAllowRevenueSharing.Checked;
            entityService.IsQtyAllowChangeForDoctor = chkIsQtyAllowChangeForDoctor.Checked;
            entityService.IsRevenueSharingFee1 = chkIsRevenueSharingFee1.Checked;
            entityService.IsRevenueSharingFee2 = chkIsRevenueSharingFee2.Checked;
            entityService.IsRevenueSharingFee3 = chkIsRevenueSharingFee3.Checked;
            entityService.IsSubContractItem = chkIsSubContractItem.Checked;
            entityService.IsTestItem = chkIsTestItem.Checked;
            entityService.IsPackageItem = chkIsPackageItem.Checked;
            entityService.IsPackageAllInOne = chkIsPackageAllInOne.Checked;
            entityService.IsUsingAccumulatedPrice = chkIsAccumulatedPrice.Checked;
            entityService.IsBridgingItem = chkIsBridgingItem.Checked;
            entityService.IsBPJS = chkIsBPJS.Checked;
            entityService.IsUnbilledItem = chkIsUnbilledItem.Checked;
            entityLab.IsPPRAFlag = ChkIsPPRA.Checked;
            entityLab.IsPGxTest = chkIsPGxTest.Checked;
            entityLab.IsLinkedToeHAC = chkIsBridgingToeHAC.Checked;
            entityLab.IsSpotCheckItem = chkIsSpotCheckItem.Checked; 
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

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            //errMessage = string.Empty;
            //string FilterExpression = string.Format("ItemCode = '{0}'", txtItemCode.Text);
            //List<ItemMaster> lst = BusinessLayer.GetItemMasterList(FilterExpression);

            //if (lst.Count > 0)
            //    errMessage = "Item Code " + txtItemCode.Text + " is already exist";

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
            ItemLaboratoryDao entityLabDao = new ItemLaboratoryDao(ctx);
            ItemCostDao entityCostDao = new ItemCostDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = new ItemMaster();
                ItemService entityService = new ItemService();
                ItemLaboratory entityLab = new ItemLaboratory();
                ItemTagField entityTagField = new ItemTagField();
                ControlToEntity(entity, entityService, entityLab, entityTagField);
                entity.ItemCode = Helper.GenerateItemCode(ctx, entity.ItemName1);
                entity.GCItemType = hdnGCItemType.Value;

                entity.CreatedBy = AppSession.UserLogin.UserID;
                int oItemID = entityDao.InsertReturnPrimaryKeyID(entity);

                entityService.ItemID = oItemID;
                entityServiceDao.Insert(entityService);

                entityLab.ItemID = entityService.ItemID;
                entityLabDao.Insert(entityLab);

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
            ItemLaboratoryDao entityLaboratoryDao = new ItemLaboratoryDao(ctx);
            ItemTagFieldDao entityTagFieldDao = new ItemTagFieldDao(ctx);
            try
            {
                ItemMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                ItemService entityService = entityServiceDao.Get(entity.ItemID);
                ItemLaboratory entityLab = entityLaboratoryDao.Get(entity.ItemID);
                ItemTagField entityTagField = entityTagFieldDao.Get(entity.ItemID);

                ControlToEntity(entity, entityService, entityLab, entityTagField);
                
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                entityService.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityServiceDao.Update(entityService);

                entityLab.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityLaboratoryDao.Update(entityLab);

                entityTagField.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityTagFieldDao.Update(entityTagField);

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