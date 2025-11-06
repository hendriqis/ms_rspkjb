using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Accounting.Program
{
    public partial class GLProductLineEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Accounting.GL_PRODUCT_LINE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            String[] param = Request.QueryString["id"].Split('|');
            if (param[0] == "edit")
            {
                IsAdd = false;
                String ID = param[1];
                hdnID.Value = ID;
                ProductLine entity = BusinessLayer.GetProductLineList(String.Format("ProductLineID = {0}", hdnID.Value))[0];
                List<vProductLineDt> productLineDtList = BusinessLayer.GetvProductLineDtList(String.Format("ProductLineID = {0}", entity.ProductLineID));
                vProductLineDt entityDt = new vProductLineDt();

                if (productLineDtList.Count>0)
                    entityDt = productLineDtList[0]; 

                SetControlProperties();
                EntityToControl(entity, entityDt);
                hdnGCItemType.Value = entity.GCItemType;
            }
            else
            {
                SetControlProperties();
                IsAdd = true;
                hdnGCItemType.Value = param[1];
            }

            txtProductLineCode.Focus();
        }

        protected override void SetControlProperties()
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format(
                                                        "ParentID IN ('{0}') AND IsActive = 1 AND IsDeleted = 0",
                                                        Constant.StandardCode.PURCHASING_BUDGET_CATEGORY
                                                 ));

            Methods.SetComboBoxField<StandardCode>(cboBudgetCategory, lstStandardCode.Where(a => a.ParentID == Constant.StandardCode.PURCHASING_BUDGET_CATEGORY).ToList(), "StandardCodeName", "StandardCodeID");
            cboBudgetCategory.SelectedIndex = 0;

        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(cboBudgetCategory, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsInventoryItem, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsFixedAsset, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsConsigmentItem, new ControlEntrySetting(true, true, false));

            #region Pengaturan Perkiraan
            SetControlEntrySetting(hdnInventoryID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventorySearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventorySubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventoryGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInventoryGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblInventorySubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnInventorySubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventorySubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtInventorySubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnInventoryDiscountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventoryDiscountSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventoryDiscountSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventoryDiscountGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInventoryDiscountGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblInventoryDiscountSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnInventoryDiscountSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventoryDiscountSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtInventoryDiscountSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnInventoryVATID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventoryVATSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnInventoryVATSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventoryVATGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInventoryVATGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblInventoryVATSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnInventoryVATSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtInventoryVATSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtInventoryVATSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnCOGSID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnCOGSSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnCOGSSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCOGSGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtCOGSGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblCOGSSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnCOGSSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtCOGSSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtCOGSSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPurchaseID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchaseGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblPurchaseSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnPurchaseSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPurchaseReturnID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseReturnSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseReturnSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseReturnGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchaseReturnGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblPurchaseReturnSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnPurchaseReturnSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseReturnSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseReturnSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPurchaseDiscount, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseDiscountSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchaseDiscountSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseDiscountGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchaseDiscountGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblPurchaseDiscountSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnPurchaseDiscountSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseDiscountSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchaseDiscountSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAccountPayableInProcessID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableInProcessSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableInProcessSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableInProcessGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAccountPayableInProcessGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAccountPayableInProcessSubLedgerID, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAccountPayableInProcessSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableInProcessSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAccountPayableInProcessSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAccountPayableID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAccountPayableGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAccountPayableSubLedgerID, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAccountPayableSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAccountPayableSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAccountPayableConsignmentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableConsignmentSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableConsignmentSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableConsignmentGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAccountPayableConsignmentGLAccountName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAccountPayableDownPaymentID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableDownPaymentSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAccountPayableDownPaymentSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAccountPayableDownPaymentGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAccountPayableDownPaymentGLAccountName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnPurchasePriceVariant, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchasePriceVariantSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnPurchasePriceVariantSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchasePriceVariantGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPurchasePriceVariantGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblPurchasePriceVariantSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnPurchasePriceVariantSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchasePriceVariantSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtPurchasePriceVariantSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnSales, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSalesGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSalesSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSalesSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSalesSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnSalesReturn, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesReturnSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesReturnSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesReturnGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSalesReturnGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSalesReturnSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSalesReturnSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesReturnSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSalesReturnSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnSalesDiscount, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesDiscountSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSalesDiscountSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesDiscountGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtSalesDiscountGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSalesDiscountSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnSalesDiscountSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSalesDiscountSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSalesDiscountSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnMaterialRevenue, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnMaterialRevenueSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnMaterialRevenueSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMaterialRevenueGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtMaterialRevenueGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblMaterialRevenueSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnMaterialRevenueSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtMaterialRevenueSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtMaterialRevenueSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnConsumption, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnConsumptionSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnConsumptionSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtConsumptionGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtConsumptionGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblConsumptionSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnConsumptionSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtConsumptionSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtConsumptionSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAdjustmentIN, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAdjustmentINSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAdjustmentINSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentINGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAdjustmentINGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAdjustmentINSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAdjustmentINSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentINSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAdjustmentINSubLedgerName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnAdjustmentOUT, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAdjustmentOUTSearchDialogTypeName, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnAdjustmentOUTSubLedgerID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentOUTGLAccountNo, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtAdjustmentOUTGLAccountName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblAdjustmentOUTSubLedger, new ControlEntrySetting(false, false));
            SetControlEntrySetting(hdnAdjustmentOUTSubLedger, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtAdjustmentOUTSubLedgerCode, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtAdjustmentOUTSubLedgerName, new ControlEntrySetting(false, false, false));
            #endregion
        }

        private void EntityToControl(ProductLine entity, vProductLineDt entityDt)
        {
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            cboBudgetCategory.Value = entity.GCBudgetCategory;
            txtRemarks.Text = entity.Remarks;

            chkIsInventoryItem.Checked = entity.IsInventoryItem;
            chkIsFixedAsset.Checked = entity.IsFixedAsset;
            chkIsConsigmentItem.Checked = entity.IsConsigmentItem;

            if (entityDt != null)
            {
                #region Pengaturan Perkiraan
                #region Inventory
                hdnInventoryID.Value = entityDt.Inventory.ToString();
                txtInventoryGLAccountNo.Text = entityDt.InventoryGLAccountNo ?? string.Empty;
                txtInventoryGLAccountName.Text = entityDt.InventoryGLAccountName ?? string.Empty;
                hdnInventorySubLedgerID.Value = entityDt.InventorySubLedgerID.ToString() ?? string.Empty;
                hdnInventorySearchDialogTypeName.Value = entityDt.InventorySearchDialogTypeName ?? string.Empty;
                hdnInventoryIDFieldName.Value = entityDt.InventoryIDFieldName ?? string.Empty;
                hdnInventoryCodeFieldName.Value = entityDt.InventoryCodeFieldName ?? string.Empty;
                hdnInventoryDisplayFieldName.Value = entityDt.InventoryDisplayFieldName ?? string.Empty;
                hdnInventoryMethodName.Value = entityDt.InventoryMethodName ?? string.Empty;
                hdnInventoryFilterExpression.Value = entityDt.InventoryFilterExpression ?? string.Empty;

                hdnInventorySubLedger.Value = entityDt.InventorySubLedger.ToString() ?? string.Empty;
                txtInventorySubLedgerCode.Text = entityDt.InventorySubLedgerCode ?? string.Empty;
                txtInventorySubLedgerName.Text = entityDt.InventorySubLedgerName ?? string.Empty;
                #endregion

                #region InventoryDiscount
                hdnInventoryDiscountID.Value = entityDt.InventoryDiscount.ToString();
                txtInventoryDiscountGLAccountNo.Text = entityDt.InventoryDiscountGLAccountNo ?? string.Empty;
                txtInventoryDiscountGLAccountName.Text = entityDt.InventoryDiscountGLAccountName ?? string.Empty;
                hdnInventoryDiscountSubLedgerID.Value = entityDt.InventoryDiscountSubLedgerID.ToString() ?? string.Empty;
                hdnInventoryDiscountSearchDialogTypeName.Value = entityDt.InventoryDiscountSearchDialogTypeName ?? string.Empty;
                hdnInventoryDiscountIDFieldName.Value = entityDt.InventoryDiscountIDFieldName ?? string.Empty;
                hdnInventoryDiscountCodeFieldName.Value = entityDt.InventoryDiscountCodeFieldName ?? string.Empty;
                hdnInventoryDiscountDisplayFieldName.Value = entityDt.InventoryDiscountDisplayFieldName ?? string.Empty;
                hdnInventoryDiscountMethodName.Value = entityDt.InventoryDiscountMethodName ?? string.Empty;
                hdnInventoryDiscountFilterExpression.Value = entityDt.InventoryDiscountFilterExpression ?? string.Empty;

                hdnInventoryDiscountSubLedger.Value = entityDt.InventoryDiscountSubLedger.ToString() ?? string.Empty;
                txtInventoryDiscountSubLedgerCode.Text = entityDt.InventoryDiscountSubLedgerCode ?? string.Empty;
                txtInventoryDiscountSubLedgerName.Text = entityDt.InventoryDiscountSubLedgerName ?? string.Empty;
                #endregion

                #region InventoryVAT
                hdnInventoryVATID.Value = entityDt.InventoryVAT.ToString();
                txtInventoryVATGLAccountNo.Text = entityDt.InventoryVATGLAccountNo ?? string.Empty;
                txtInventoryVATGLAccountName.Text = entityDt.InventoryVATGLAccountName ?? string.Empty;
                hdnInventoryVATSubLedgerID.Value = entityDt.InventoryVATSubLedgerID.ToString();
                hdnInventoryVATSearchDialogTypeName.Value = entityDt.InventoryVATSearchDialogTypeName ?? string.Empty;
                hdnInventoryVATIDFieldName.Value = entityDt.InventoryVATIDFieldName ?? string.Empty;
                hdnInventoryVATCodeFieldName.Value = entityDt.InventoryVATCodeFieldName ?? string.Empty;
                hdnInventoryVATDisplayFieldName.Value = entityDt.InventoryVATDisplayFieldName ?? string.Empty;
                hdnInventoryVATMethodName.Value = entityDt.InventoryVATMethodName ?? string.Empty;
                hdnInventoryVATFilterExpression.Value = entityDt.InventoryVATFilterExpression ?? string.Empty;

                hdnInventoryVATSubLedger.Value = entityDt.InventoryVATSubLedger.ToString();
                txtInventoryVATSubLedgerCode.Text = entityDt.InventoryVATSubLedgerCode ?? string.Empty;
                txtInventoryVATSubLedgerName.Text = entityDt.InventoryVATSubLedgerName ?? string.Empty;
                #endregion

                #region COGS
                hdnCOGSID.Value = entityDt.COGS.ToString();
                txtCOGSGLAccountNo.Text = entityDt.COGSGLAccountNo ?? string.Empty;
                txtCOGSGLAccountName.Text = entityDt.COGSGLAccountName ?? string.Empty;
                hdnCOGSSubLedgerID.Value = entityDt.COGSSubLedgerID.ToString();
                hdnCOGSSearchDialogTypeName.Value = entityDt.COGSSearchDialogTypeName ?? string.Empty;
                hdnCOGSIDFieldName.Value = entityDt.COGSIDFieldName ?? string.Empty;
                hdnCOGSCodeFieldName.Value = entityDt.COGSCodeFieldName ?? string.Empty;
                hdnCOGSDisplayFieldName.Value = entityDt.COGSDisplayFieldName ?? string.Empty;
                hdnCOGSMethodName.Value = entityDt.COGSMethodName ?? string.Empty;
                hdnCOGSFilterExpression.Value = entityDt.COGSFilterExpression ?? string.Empty;

                hdnCOGSSubLedger.Value = entityDt.COGSSubLedger.ToString();
                txtCOGSSubLedgerCode.Text = entityDt.COGSSubLedgerCode ?? string.Empty;
                txtCOGSSubLedgerName.Text = entityDt.COGSSubLedgerName ?? string.Empty;
                #endregion

                #region Purchase
                hdnPurchaseID.Value = entityDt.Purchase.ToString();
                txtPurchaseGLAccountNo.Text = entityDt.PurchaseGLAccountNo ?? string.Empty;
                txtPurchaseGLAccountName.Text = entityDt.PurchaseGLAccountName ?? string.Empty;
                hdnPurchaseSubLedgerID.Value = entityDt.PurchaseSubLedgerID.ToString();
                hdnPurchaseSearchDialogTypeName.Value = entityDt.PurchaseSearchDialogTypeName ?? string.Empty;
                hdnPurchaseIDFieldName.Value = entityDt.PurchaseIDFieldName ?? string.Empty;
                hdnPurchaseCodeFieldName.Value = entityDt.PurchaseCodeFieldName ?? string.Empty;
                hdnPurchaseDisplayFieldName.Value = entityDt.PurchaseDisplayFieldName ?? string.Empty;
                hdnPurchaseMethodName.Value = entityDt.PurchaseMethodName ?? string.Empty;
                hdnPurchaseFilterExpression.Value = entityDt.PurchaseFilterExpression ?? string.Empty;

                hdnPurchaseSubLedger.Value = entityDt.PurchaseSubLedger.ToString();
                txtPurchaseSubLedgerCode.Text = entityDt.PurchaseSubLedgerCode ?? string.Empty;
                txtPurchaseSubLedgerName.Text = entityDt.PurchaseSubLedgerName ?? string.Empty;
                #endregion

                #region PurchaseReturn
                hdnPurchaseReturnID.Value = entityDt.PurchaseReturn.ToString();
                txtPurchaseReturnGLAccountNo.Text = entityDt.PurchaseReturnGLAccountNo ?? string.Empty;
                txtPurchaseReturnGLAccountName.Text = entityDt.PurchaseReturnGLAccountName ?? string.Empty;
                hdnPurchaseReturnSubLedgerID.Value = entityDt.PurchaseReturnSubLedgerID.ToString();
                hdnPurchaseReturnSearchDialogTypeName.Value = entityDt.PurchaseReturnSearchDialogTypeName ?? string.Empty;
                hdnPurchaseReturnIDFieldName.Value = entityDt.PurchaseReturnIDFieldName ?? string.Empty;
                hdnPurchaseReturnCodeFieldName.Value = entityDt.PurchaseReturnCodeFieldName ?? string.Empty;
                hdnPurchaseReturnDisplayFieldName.Value = entityDt.PurchaseReturnDisplayFieldName ?? string.Empty;
                hdnPurchaseReturnMethodName.Value = entityDt.PurchaseReturnMethodName ?? string.Empty;
                hdnPurchaseReturnFilterExpression.Value = entityDt.PurchaseReturnFilterExpression;

                hdnPurchaseReturnSubLedger.Value = entityDt.PurchaseReturnSubLedger.ToString();
                txtPurchaseReturnSubLedgerCode.Text = entityDt.PurchaseReturnSubLedgerCode ?? string.Empty;
                txtPurchaseReturnSubLedgerName.Text = entityDt.PurchaseReturnSubLedgerName ?? string.Empty;
                #endregion

                #region PurchaseDiscount
                hdnPurchaseDiscount.Value = entityDt.PurchaseDiscount.ToString();
                txtPurchaseDiscountGLAccountNo.Text = entityDt.PurchaseDiscountGLAccountNo ?? string.Empty;
                txtPurchaseDiscountGLAccountName.Text = entityDt.PurchaseDiscountGLAccountName ?? string.Empty;
                hdnPurchaseDiscountSubLedgerID.Value = entityDt.PurchaseDiscountSubLedgerID.ToString();
                hdnPurchaseDiscountSearchDialogTypeName.Value = entityDt.PurchaseDiscountSearchDialogTypeName ?? string.Empty;
                hdnPurchaseDiscountIDFieldName.Value = entityDt.PurchaseDiscountIDFieldName ?? string.Empty;
                hdnPurchaseDiscountCodeFieldName.Value = entityDt.PurchaseDiscountCodeFieldName ?? string.Empty;
                hdnPurchaseDiscountDisplayFieldName.Value = entityDt.PurchaseDiscountDisplayFieldName ?? string.Empty;
                hdnPurchaseDiscountMethodName.Value = entityDt.PurchaseDiscountMethodName ?? string.Empty;
                hdnPurchaseDiscountFilterExpression.Value = entityDt.PurchaseDiscountFilterExpression;

                hdnPurchaseDiscountSubLedger.Value = entityDt.PurchaseDiscountSubLedger.ToString();
                txtPurchaseDiscountSubLedgerCode.Text = entityDt.PurchaseDiscountSubLedgerCode ?? string.Empty;
                txtPurchaseDiscountSubLedgerName.Text = entityDt.PurchaseDiscountSubLedgerName ?? string.Empty;
                #endregion

                #region Account Payable In Process
                hdnAccountPayableInProcessID.Value = entityDt.APInProcess.ToString();
                txtAccountPayableInProcessGLAccountNo.Text = entityDt.APInProcessGLAccountNo ?? string.Empty;
                txtAccountPayableInProcessGLAccountName.Text = entityDt.APInProcessGLAccountName ?? string.Empty;
                hdnAccountPayableInProcessSubLedgerID.Value = entityDt.APInProcessSubLedgerID.ToString();
                hdnAccountPayableInProcessSearchDialogTypeName.Value = entityDt.APInProcessSearchDialogTypeName ?? string.Empty;
                hdnAccountPayableInProcessIDFieldName.Value = entityDt.APInProcessIDFieldName ?? string.Empty;
                hdnAccountPayableInProcessCodeFieldName.Value = entityDt.APInProcessCodeFieldName ?? string.Empty;
                hdnAccountPayableInProcessDisplayFieldName.Value = entityDt.APInProcessDisplayFieldName ?? string.Empty;
                hdnAccountPayableInProcessMethodName.Value = entityDt.APInProcessMethodName ?? string.Empty;
                hdnAccountPayableInProcessFilterExpression.Value = entityDt.APInProcessFilterExpression ?? string.Empty;

                //hdnAccountPayableInProcessSubLedger.Value = entityDt.APInProcessSubLedger.ToString();
                //txtAccountPayableInProcessSubLedgerCode.Text = entityDt.APInProcessSubLedgerCode ?? string.Empty;
                //txtAccountPayableInProcessSubLedgerName.Text = entityDt.APInProcessSubLedgerName ?? string.Empty;
                #endregion

                #region Account Payable
                hdnAccountPayableID.Value = entityDt.AP.ToString();
                txtAccountPayableGLAccountNo.Text = entityDt.APGLAccountNo ?? string.Empty;
                txtAccountPayableGLAccountName.Text = entityDt.APGLAccountName ?? string.Empty;
                hdnAccountPayableSubLedgerID.Value = entityDt.APSubLedgerID.ToString();
                hdnAccountPayableSearchDialogTypeName.Value = entityDt.APSearchDialogTypeName ?? string.Empty;
                hdnAccountPayableIDFieldName.Value = entityDt.APIDFieldName ?? string.Empty;
                hdnAccountPayableCodeFieldName.Value = entityDt.APCodeFieldName ?? string.Empty;
                hdnAccountPayableDisplayFieldName.Value = entityDt.APDisplayFieldName ?? string.Empty;
                hdnAccountPayableMethodName.Value = entityDt.APMethodName ?? string.Empty;
                hdnAccountPayableFilterExpression.Value = entityDt.APFilterExpression ?? string.Empty;

                hdnAccountPayableConsignmentID.Value = entityDt.APConsigment.ToString();
                txtAccountPayableConsignmentGLAccountNo.Text = entityDt.APConsigmentGLAccountNo ?? string.Empty;
                txtAccountPayableConsignmentGLAccountName.Text = entityDt.APConsigmentGLAccountName ?? string.Empty;
                hdnAccountPayableConsignmentSubLedgerID.Value = entityDt.APConsigmentSubLedgerID.ToString();
                hdnAccountPayableConsignmentSearchDialogTypeName.Value = entityDt.APConsigmentSearchDialogTypeName ?? string.Empty;
                hdnAccountPayableConsignmentIDFieldName.Value = entityDt.APConsigmentIDFieldName ?? string.Empty;
                hdnAccountPayableConsignmentCodeFieldName.Value = entityDt.APConsigmentCodeFieldName ?? string.Empty;
                hdnAccountPayableConsignmentDisplayFieldName.Value = entityDt.APConsigmentDisplayFieldName ?? string.Empty;
                hdnAccountPayableConsignmentMethodName.Value = entityDt.APConsigmentMethodName ?? string.Empty;
                hdnAccountPayableConsignmentFilterExpression.Value = entityDt.APConsigmentFilterExpression ?? string.Empty;

                hdnAccountPayableDownPaymentID.Value = entityDt.APDownPayment.ToString();
                txtAccountPayableDownPaymentGLAccountNo.Text = entityDt.APDownPaymentGLAccountNo ?? string.Empty;
                txtAccountPayableDownPaymentGLAccountName.Text = entityDt.APDownPaymentGLAccountName ?? string.Empty;
                hdnAccountPayableDownPaymentSubLedgerID.Value = entityDt.APDownPaymentSubLedgerID.ToString();
                hdnAccountPayableDownPaymentSearchDialogTypeName.Value = entityDt.APDownPaymentSearchDialogTypeName ?? string.Empty;
                hdnAccountPayableDownPaymentIDFieldName.Value = entityDt.APDownPaymentIDFieldName ?? string.Empty;
                hdnAccountPayableDownPaymentCodeFieldName.Value = entityDt.APDownPaymentCodeFieldName ?? string.Empty;
                hdnAccountPayableDownPaymentDisplayFieldName.Value = entityDt.APDownPaymentDisplayFieldName ?? string.Empty;
                hdnAccountPayableDownPaymentMethodName.Value = entityDt.APDownPaymentMethodName ?? string.Empty;
                hdnAccountPayableDownPaymentFilterExpression.Value = entityDt.APDownPaymentFilterExpression ?? string.Empty;

                //hdnAccountPayableSubLedger.Value = entityDt.APSubLedger.ToString();
                //txtAccountPayableSubLedgerCode.Text = entityDt.APSubLedgerCode ?? string.Empty;
                //txtAccountPayableSubLedgerName.Text = entityDt.APSubLedgerName ?? string.Empty;
                #endregion

                #region PurchasePriceVariant
                hdnPurchasePriceVariant.Value = entityDt.PurchasePriceVariant.ToString();
                txtPurchasePriceVariantGLAccountNo.Text = entityDt.PurchasePriceVariantGLAccountNo ?? string.Empty;
                txtPurchasePriceVariantGLAccountName.Text = entityDt.PurchasePriceVariantGLAccountName ?? string.Empty;
                hdnPurchasePriceVariantSubLedgerID.Value = entityDt.PurchasePriceVariantSubLedgerID.ToString();
                hdnPurchasePriceVariantSearchDialogTypeName.Value = entityDt.PurchasePriceVariantSearchDialogTypeName;
                hdnPurchasePriceVariantIDFieldName.Value = entityDt.PurchasePriceVariantIDFieldName;
                hdnPurchasePriceVariantCodeFieldName.Value = entityDt.PurchasePriceVariantCodeFieldName;
                hdnPurchasePriceVariantDisplayFieldName.Value = entityDt.PurchasePriceVariantDisplayFieldName;
                hdnPurchasePriceVariantMethodName.Value = entityDt.PurchasePriceVariantMethodName;
                hdnPurchasePriceVariantFilterExpression.Value = entityDt.PurchasePriceVariantFilterExpression;

                hdnPurchasePriceVariantSubLedger.Value = entityDt.PurchasePriceVariantSubLedger.ToString();
                txtPurchasePriceVariantSubLedgerCode.Text = entityDt.PurchasePriceVariantSubLedgerCode ?? string.Empty;
                txtPurchasePriceVariantSubLedgerName.Text = entityDt.PurchasePriceVariantSubLedgerName ?? string.Empty;
                #endregion

                #region Sales
                hdnSales.Value = entityDt.Sales.ToString();
                txtSalesGLAccountNo.Text = entityDt.SalesGLAccountNo ?? string.Empty;
                txtSalesGLAccountName.Text = entityDt.SalesGLAccountName ?? string.Empty;
                hdnSalesSubLedgerID.Value = entityDt.SalesSubLedgerID.ToString();
                hdnSalesSearchDialogTypeName.Value = entityDt.SalesSearchDialogTypeName ?? string.Empty;
                hdnSalesIDFieldName.Value = entityDt.SalesIDFieldName ?? string.Empty;
                hdnSalesCodeFieldName.Value = entityDt.SalesCodeFieldName ?? string.Empty;
                hdnSalesDisplayFieldName.Value = entityDt.SalesDisplayFieldName ?? string.Empty;
                hdnSalesMethodName.Value = entityDt.SalesMethodName ?? string.Empty;
                hdnSalesFilterExpression.Value = entityDt.SalesFilterExpression ?? string.Empty;

                hdnSalesSubLedger.Value = entityDt.SalesSubLedger.ToString();
                txtSalesSubLedgerCode.Text = entityDt.SalesSubLedgerCode ?? string.Empty;
                txtSalesSubLedgerName.Text = entityDt.SalesSubLedgerName ?? string.Empty;
                #endregion

                #region SalesReturn
                hdnSalesReturn.Value = entityDt.SalesReturn.ToString();
                txtSalesReturnGLAccountNo.Text = entityDt.SalesReturnGLAccountNo ?? string.Empty;
                txtSalesReturnGLAccountName.Text = entityDt.SalesReturnGLAccountName ?? string.Empty;
                hdnSalesReturnSubLedgerID.Value = entityDt.SalesReturnSubLedgerID.ToString();
                hdnSalesReturnSearchDialogTypeName.Value = entityDt.SalesReturnSearchDialogTypeName ?? string.Empty;
                hdnSalesReturnIDFieldName.Value = entityDt.SalesReturnIDFieldName ?? string.Empty;
                hdnSalesReturnCodeFieldName.Value = entityDt.SalesReturnCodeFieldName ?? string.Empty;
                hdnSalesReturnDisplayFieldName.Value = entityDt.SalesReturnDisplayFieldName ?? string.Empty;
                hdnSalesReturnMethodName.Value = entityDt.SalesReturnMethodName ?? string.Empty;
                hdnSalesReturnFilterExpression.Value = entityDt.SalesReturnFilterExpression;

                hdnSalesReturnSubLedger.Value = entityDt.SalesReturnSubLedger.ToString();
                txtSalesReturnSubLedgerCode.Text = entityDt.SalesReturnSubLedgerCode ?? string.Empty;
                txtSalesReturnSubLedgerName.Text = entityDt.SalesReturnSubLedgerName ?? string.Empty;
                #endregion

                #region SalesDiscount
                hdnSalesDiscount.Value = entityDt.SalesDiscount.ToString();
                txtSalesDiscountGLAccountNo.Text = entityDt.SalesDiscountGLAccountNo ?? string.Empty;
                txtSalesDiscountGLAccountName.Text = entityDt.SalesDiscountGLAccountName ?? string.Empty;
                hdnSalesDiscountSubLedgerID.Value = entityDt.SalesDiscountSubLedgerID.ToString();
                hdnSalesDiscountSearchDialogTypeName.Value = entityDt.SalesDiscountSearchDialogTypeName ?? string.Empty;
                hdnSalesDiscountIDFieldName.Value = entityDt.SalesDiscountIDFieldName ?? string.Empty;
                hdnSalesDiscountCodeFieldName.Value = entityDt.SalesDiscountCodeFieldName ?? string.Empty;
                hdnSalesDiscountDisplayFieldName.Value = entityDt.SalesDiscountDisplayFieldName ?? string.Empty;
                hdnSalesDiscountMethodName.Value = entityDt.SalesDiscountMethodName ?? string.Empty;
                hdnSalesDiscountFilterExpression.Value = entityDt.SalesDiscountFilterExpression;

                hdnSalesDiscountSubLedger.Value = entityDt.SalesDiscountSubLedger.ToString();
                txtSalesDiscountSubLedgerCode.Text = entityDt.SalesDiscountSubLedgerCode ?? string.Empty;
                txtSalesDiscountSubLedgerName.Text = entityDt.SalesDiscountSubLedgerName ?? string.Empty;
                #endregion

                #region MaterialRevenue
                hdnMaterialRevenue.Value = entityDt.MaterialRevenue.ToString();
                txtMaterialRevenueGLAccountNo.Text = entityDt.MaterialRevenueGLAccountNo ?? string.Empty;
                txtMaterialRevenueGLAccountName.Text = entityDt.MaterialRevenueGLAccountName ?? string.Empty;
                hdnMaterialRevenueSubLedgerID.Value = entityDt.MaterialRevenueSubLedgerID.ToString();
                hdnMaterialRevenueSearchDialogTypeName.Value = entityDt.MaterialRevenueSearchDialogTypeName;
                hdnMaterialRevenueIDFieldName.Value = entityDt.MaterialRevenueIDFieldName ?? string.Empty;
                hdnMaterialRevenueCodeFieldName.Value = entityDt.MaterialRevenueCodeFieldName ?? string.Empty;
                hdnMaterialRevenueDisplayFieldName.Value = entityDt.MaterialRevenueDisplayFieldName ?? string.Empty;
                hdnMaterialRevenueMethodName.Value = entityDt.MaterialRevenueMethodName ?? string.Empty;
                hdnMaterialRevenueFilterExpression.Value = entityDt.MaterialRevenueFilterExpression;

                hdnMaterialRevenueSubLedger.Value = entityDt.MaterialRevenueSubLedger.ToString();
                txtMaterialRevenueSubLedgerCode.Text = entityDt.MaterialRevenueSubLedgerCode ?? string.Empty;
                txtMaterialRevenueSubLedgerName.Text = entityDt.MaterialRevenueSubLedgerName ?? string.Empty;
                #endregion

                #region Consumption
                hdnConsumption.Value = entityDt.Consumption.ToString();
                txtConsumptionGLAccountNo.Text = entityDt.ConsumptionGLAccountNo;
                txtConsumptionGLAccountName.Text = entityDt.ConsumptionGLAccountName;
                hdnConsumptionSubLedgerID.Value = entityDt.ConsumptionSubLedgerID.ToString();
                hdnConsumptionSearchDialogTypeName.Value = entityDt.ConsumptionSearchDialogTypeName;
                hdnConsumptionIDFieldName.Value = entityDt.ConsumptionIDFieldName ?? string.Empty;
                hdnConsumptionCodeFieldName.Value = entityDt.ConsumptionCodeFieldName ?? string.Empty;
                hdnConsumptionDisplayFieldName.Value = entityDt.ConsumptionDisplayFieldName ?? string.Empty;
                hdnConsumptionMethodName.Value = entityDt.ConsumptionMethodName ?? string.Empty;
                hdnConsumptionFilterExpression.Value = entityDt.ConsumptionFilterExpression ?? string.Empty;

                hdnConsumptionSubLedger.Value = entityDt.ConsumptionSubLedger.ToString();
                txtConsumptionSubLedgerCode.Text = entityDt.ConsumptionSubLedgerCode ?? string.Empty;
                txtConsumptionSubLedgerName.Text = entityDt.ConsumptionSubLedgerName ?? string.Empty;
                #endregion

                #region AdjustmentIN
                hdnAdjustmentIN.Value = entityDt.AdjustmentIN.ToString();
                txtAdjustmentINGLAccountNo.Text = entityDt.AdjustmentINGLAccountNo ?? string.Empty;
                txtAdjustmentINGLAccountName.Text = entityDt.AdjustmentINGLAccountName ?? string.Empty;
                hdnAdjustmentINSubLedgerID.Value = entityDt.AdjustmentINSubLedgerID.ToString();
                hdnAdjustmentINSearchDialogTypeName.Value = entityDt.AdjustmentINSearchDialogTypeName;
                hdnAdjustmentINIDFieldName.Value = entityDt.AdjustmentINIDFieldName ?? string.Empty;
                hdnAdjustmentINCodeFieldName.Value = entityDt.AdjustmentINCodeFieldName ?? string.Empty;
                hdnAdjustmentINDisplayFieldName.Value = entityDt.AdjustmentINDisplayFieldName ?? string.Empty;
                hdnAdjustmentINMethodName.Value = entityDt.AdjustmentINMethodName ?? string.Empty;
                hdnAdjustmentINFilterExpression.Value = entityDt.AdjustmentINFilterExpression ?? string.Empty;

                hdnAdjustmentINSubLedger.Value = entityDt.AdjustmentINSubLedger.ToString();
                txtAdjustmentINSubLedgerCode.Text = entityDt.AdjustmentINSubLedgerCode ?? string.Empty;
                txtAdjustmentINSubLedgerName.Text = entityDt.AdjustmentINSubLedgerName ?? string.Empty;
                #endregion

                #region AdjustmentOUT
                hdnAdjustmentOUT.Value = entityDt.AdjustmentOUT.ToString();
                txtAdjustmentOUTGLAccountNo.Text = entityDt.AdjustmentOUTGLAccountNo ?? string.Empty;
                txtAdjustmentOUTGLAccountName.Text = entityDt.AdjustmentOUTGLAccountName ?? string.Empty;
                hdnAdjustmentOUTSubLedgerID.Value = entityDt.AdjustmentOUTSubLedgerID.ToString();
                hdnAdjustmentOUTSearchDialogTypeName.Value = entityDt.AdjustmentOUTSearchDialogTypeName;
                hdnAdjustmentOUTIDFieldName.Value = entityDt.AdjustmentOUTIDFieldName ?? string.Empty;
                hdnAdjustmentOUTCodeFieldName.Value = entityDt.AdjustmentOUTCodeFieldName ?? string.Empty;
                hdnAdjustmentOUTDisplayFieldName.Value = entityDt.AdjustmentOUTDisplayFieldName ?? string.Empty;
                hdnAdjustmentOUTMethodName.Value = entityDt.AdjustmentOUTMethodName ?? string.Empty;
                hdnAdjustmentOUTFilterExpression.Value = entityDt.AdjustmentOUTFilterExpression ?? string.Empty;

                hdnAdjustmentOUTSubLedger.Value = entityDt.AdjustmentOUTSubLedger.ToString();
                txtAdjustmentOUTSubLedgerCode.Text = entityDt.AdjustmentOUTSubLedgerCode ?? string.Empty;
                txtAdjustmentOUTSubLedgerName.Text = entityDt.AdjustmentOUTSubLedgerName ?? string.Empty;
                #endregion
                #endregion 
            }
        }

        private void ControlToEntity(ProductLine entity, ProductLineDt entityDt)
        {
            entity.ProductLineCode = txtProductLineCode.Text;
            entity.ProductLineName = txtProductLineName.Text;
            entity.GCBudgetCategory = cboBudgetCategory.Value.ToString();
            entity.Remarks = txtRemarks.Text;

            entity.IsInventoryItem = chkIsInventoryItem.Checked;
            entity.IsFixedAsset = chkIsFixedAsset.Checked;
            entity.IsConsigmentItem = chkIsConsigmentItem.Checked;

            #region Pengaturan Perkiraan
            #region Inventory
            if (hdnInventoryID.Value != "" && hdnInventoryID.Value != "0")
                entityDt.Inventory = Convert.ToInt32(hdnInventoryID.Value);
            else
                entityDt.Inventory = null;
            if (hdnInventorySubLedger.Value != "" && hdnInventorySubLedger.Value != "0")
                entityDt.InventorySubLedger= Convert.ToInt32(hdnInventorySubLedger.Value);
            else
                entityDt.InventorySubLedger = null;
            #endregion

            #region InventoryDiscount
            if (hdnInventoryDiscountID.Value != "" && hdnInventoryDiscountID.Value != "0")
                entityDt.InventoryDiscount = Convert.ToInt32(hdnInventoryDiscountID.Value);
            else
                entityDt.InventoryDiscount = null;
            if (hdnInventoryDiscountSubLedger.Value != "" && hdnInventoryDiscountSubLedger.Value != "0")
                entityDt.InventoryDiscountSubLedger = Convert.ToInt32(hdnInventoryDiscountSubLedger.Value);
            else
                entityDt.InventoryDiscountSubLedger = null;
            #endregion

            #region InventoryVAT
            if (hdnInventoryVATID.Value != "" && hdnInventoryVATID.Value != "0")
                entityDt.InventoryVAT = Convert.ToInt32(hdnInventoryVATID.Value);
            else
                entityDt.InventoryVAT = null;
            if (hdnInventoryVATSubLedger.Value != "" && hdnInventoryVATSubLedger.Value != "0")
                entityDt.InventoryVATSubLedger = Convert.ToInt32(hdnInventoryVATSubLedger.Value);
            else
                entityDt.InventoryVATSubLedger = null;
            #endregion

            #region COGS
            if (hdnCOGSID.Value != "" && hdnCOGSID.Value != "0")
                entityDt.COGS = Convert.ToInt32(hdnCOGSID.Value);
            else
                entityDt.COGS = null;
            if (hdnCOGSSubLedger.Value != "" && hdnCOGSSubLedger.Value != "0")
                entityDt.COGSSubLedger= Convert.ToInt32(hdnCOGSSubLedger.Value);
            else
                entityDt.COGSSubLedger = null;
            #endregion

            #region Purchase
            if (hdnPurchaseID.Value != "" && hdnPurchaseID.Value != "0")
                entityDt.Purchase = Convert.ToInt32(hdnPurchaseID.Value);
            else
                entityDt.Purchase = null;
            if (hdnPurchaseSubLedger.Value != "" && hdnPurchaseSubLedger.Value != "0")
                entityDt.PurchaseSubLedger= Convert.ToInt32(hdnPurchaseSubLedger.Value);
            else
                entityDt.PurchaseSubLedger = null;
            #endregion

            #region PurchaseReturn
            if (hdnPurchaseReturnID.Value != "" && hdnPurchaseReturnID.Value != "0")
                entityDt.PurchaseReturn = Convert.ToInt32(hdnPurchaseReturnID.Value);
            else
                entityDt.PurchaseReturn = null;
            if (hdnPurchaseReturnSubLedger.Value != "" && hdnPurchaseReturnSubLedger.Value != "0")
                entityDt.PurchaseReturnSubLedger= Convert.ToInt32(hdnPurchaseReturnSubLedger.Value);
            else
                entityDt.PurchaseReturnSubLedger = null;
            #endregion

            #region PurchaseDiscount
            if (hdnPurchaseDiscount.Value != "" && hdnPurchaseDiscount.Value != "0")
                entityDt.PurchaseDiscount = Convert.ToInt32(hdnPurchaseDiscount.Value);
            else
                entityDt.PurchaseDiscount = null;
            if (hdnPurchaseDiscountSubLedger.Value != "" && hdnPurchaseDiscountSubLedger.Value != "0")
                entityDt.PurchaseDiscountSubLedger= Convert.ToInt32(hdnPurchaseDiscountSubLedger.Value);
            else
                entityDt.PurchaseDiscountSubLedger = null;
            #endregion

            #region Account Payable In Process
            if (hdnAccountPayableInProcessID.Value != "" && hdnAccountPayableInProcessID.Value != "0")
                entityDt.APInProcess = Convert.ToInt32(hdnAccountPayableInProcessID.Value);
            else
                entityDt.APInProcess = null;
            //if (hdnAccountPayableInProcessSubLedger.Value != "" && hdnAccountPayableInProcessSubLedger.Value != "0")
            //    entityDt.APInProcessSubLedger = Convert.ToInt32(hdnAccountPayableInProcessSubLedger.Value);
            //else
            //    entityDt.APInProcessSubLedger = null;
            #endregion

            #region Account Payable
            if (hdnAccountPayableID.Value != "" && hdnAccountPayableID.Value != "0")
                entityDt.AP = Convert.ToInt32(hdnAccountPayableID.Value);
            else
                entityDt.AP = null;
            if (hdnAccountPayableConsignmentID.Value != "" && hdnAccountPayableConsignmentID.Value != "0")
                entityDt.APConsigment = Convert.ToInt32(hdnAccountPayableConsignmentID.Value);
            else
                entityDt.APConsigment = null;
            if (hdnAccountPayableDownPaymentID.Value != "" && hdnAccountPayableDownPaymentID.Value != "0")
                entityDt.APDownPayment = Convert.ToInt32(hdnAccountPayableDownPaymentID.Value);
            else
                entityDt.APDownPayment = null;
            //if (hdnAccountPayableSubLedger.Value != "" && hdnAccountPayableSubLedger.Value != "0")
            //    entityDt.APSubLedger = Convert.ToInt32(hdnAccountPayableSubLedger.Value);
            //else
            //    entityDt.APSubLedger = null;
            #endregion

            #region PurchasePriceVariant
            if (hdnPurchasePriceVariant.Value != "" && hdnPurchasePriceVariant.Value != "0")
                entityDt.PurchasePriceVariant = Convert.ToInt32(hdnPurchasePriceVariant.Value);
            else
                entityDt.PurchasePriceVariant = null;
            if (hdnPurchasePriceVariantSubLedger.Value != "" && hdnPurchasePriceVariantSubLedger.Value != "0")
                entityDt.PurchasePriceVariantSubLedger= Convert.ToInt32(hdnPurchasePriceVariantSubLedger.Value);
            else
                entityDt.PurchasePriceVariantSubLedger = null;
            #endregion

            #region Sales
            if (hdnSales.Value != "" && hdnSales.Value != "0")
                entityDt.Sales = Convert.ToInt32(hdnSales.Value);
            else
                entityDt.Sales = null;
            if (hdnSalesSubLedger.Value != "" && hdnSalesSubLedger.Value != "0")
                entityDt.SalesSubLedger= Convert.ToInt32(hdnSalesSubLedger.Value);
            else
                entityDt.SalesSubLedger = null;
            #endregion
            
            #region SalesReturn
            if (hdnSalesReturn.Value != "" && hdnSalesReturn.Value != "0")
                entityDt.SalesReturn = Convert.ToInt32(hdnSalesReturn.Value);
            else
                entityDt.SalesReturn = null;
            if (hdnSalesReturnSubLedger.Value != "" && hdnSalesReturnSubLedger.Value != "0")
                entityDt.SalesReturnSubLedger= Convert.ToInt32(hdnSalesReturnSubLedger.Value);
            else
                entityDt.SalesReturnSubLedger = null;
            #endregion
            
            #region SalesDiscount
            if (hdnSalesDiscount.Value != "" && hdnSalesDiscount.Value != "0")
                entityDt.SalesDiscount = Convert.ToInt32(hdnSalesDiscount.Value);
            else
                entityDt.SalesDiscount = null;
            if (hdnSalesDiscountSubLedger.Value != "" && hdnSalesDiscountSubLedger.Value != "0")
                entityDt.SalesDiscountSubLedger= Convert.ToInt32(hdnSalesDiscountSubLedger.Value);
            else
                entityDt.SalesDiscountSubLedger = null;
            #endregion

            #region MaterialRevenue
            if (hdnMaterialRevenue.Value != "" && hdnMaterialRevenue.Value != "0")
                entityDt.MaterialRevenue = Convert.ToInt32(hdnMaterialRevenue.Value);
            else
                entityDt.MaterialRevenue = null;
            if (hdnMaterialRevenueSubLedger.Value != "" && hdnMaterialRevenueSubLedger.Value != "0")
                entityDt.MaterialRevenueSubLedger= Convert.ToInt32(hdnMaterialRevenueSubLedger.Value);
            else
                entityDt.MaterialRevenueSubLedger = null;
            #endregion

            #region Consumption
            if (hdnConsumption.Value != "" && hdnConsumption.Value != "0")
                entityDt.Consumption = Convert.ToInt32(hdnConsumption.Value);
            else
                entityDt.Consumption = null;
            if (hdnConsumptionSubLedger.Value != "" && hdnConsumptionSubLedger.Value != "0")
                entityDt.ConsumptionSubLedger= Convert.ToInt32(hdnConsumptionSubLedger.Value);
            else
                entityDt.ConsumptionSubLedger = null;
            #endregion

            #region AdjustmentIN
            if (hdnAdjustmentIN.Value != "" && hdnAdjustmentIN.Value != "0")
                entityDt.AdjustmentIN = Convert.ToInt32(hdnAdjustmentIN.Value);
            else
                entityDt.AdjustmentIN = null;
            if (hdnAdjustmentINSubLedger.Value != "" && hdnAdjustmentINSubLedger.Value != "0")
                entityDt.AdjustmentINSubLedger= Convert.ToInt32(hdnAdjustmentINSubLedger.Value);
            else
                entityDt.AdjustmentINSubLedger = null;
            #endregion

            #region AdjustmentOUT
            if (hdnAdjustmentOUT.Value != "" && hdnAdjustmentOUT.Value != "0")
                entityDt.AdjustmentOUT = Convert.ToInt32(hdnAdjustmentOUT.Value);
            else
                entityDt.AdjustmentOUT = null;
            if (hdnAdjustmentOUTSubLedger.Value != "" && hdnAdjustmentOUTSubLedger.Value != "0")
                entityDt.AdjustmentOUTSubLedger= Convert.ToInt32(hdnAdjustmentOUTSubLedger.Value);
            else
                entityDt.AdjustmentOUTSubLedger = null;
            #endregion
            #endregion
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ProductLineDao productLineDao = new ProductLineDao(ctx);
            ProductLineDtDao productLineDtDao = new ProductLineDtDao(ctx);
            bool result = true;
            try
            {
                ProductLine entity = new ProductLine();
                ProductLineDt entityDt = new ProductLineDt();
                ControlToEntity(entity, entityDt);

                entityDt.HealthcareID = AppSession.UserLogin.HealthcareID;
                entity.GCItemType = hdnGCItemType.Value;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDt.ProductLineID = productLineDao.InsertReturnPrimaryKeyID(entity);

                entityDt.CreatedBy = AppSession.UserLogin.UserID;
                productLineDtDao.Insert(entityDt);
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
            IDbContext ctx = DbFactory.Configure(true);
            ProductLineDao productLineDao = new ProductLineDao(ctx);
            ProductLineDtDao productLineDtDao = new ProductLineDtDao(ctx);
            bool result = true;
            try
            {
                ProductLine entity = productLineDao.Get(Convert.ToInt32(hdnID.Value));
                ProductLineDt entityDt = productLineDtDao.Get(entity.ProductLineID, AppSession.UserLogin.HealthcareID);
                ControlToEntity(entity, entityDt);

                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                productLineDao.Update(entity);

                entityDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                productLineDtDao.Update(entityDt);
                
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
    }
}