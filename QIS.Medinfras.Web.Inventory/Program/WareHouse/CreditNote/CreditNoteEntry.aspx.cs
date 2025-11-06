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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class CreditNoteEntry : BasePageTrx
    {

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.CREDIT_NOTE;
        }

        protected override void InitializeDataControl()
        {
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

            hdnVATPercentage.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.VAT_PERCENTAGE).ParameterValue;
            SetControlProperties();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.SUPPLIER_CREDIT_NOTE_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboGCCreditNoteType, lstStandardCode, "StandardCodeName", "StandardCodeID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtCreditNoteNo, new ControlEntrySetting(false, false, true));
            SetControlEntrySetting(txtCreditNoteDate, new ControlEntrySetting(true, false, true, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(hdnSupplierID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSupplierCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtSupplierName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(hdnPurchaseReturnID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtPurchaseReturnNo, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtTaxInvoiceNo, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtTaxInvoiceDate, new ControlEntrySetting(false, false, false, Constant.DefaultValueEntry.DATE_NOW));
            SetControlEntrySetting(cboGCCreditNoteType, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtCNAmount, new ControlEntrySetting(false, false, true, 0));
            SetControlEntrySetting(chkPPN, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtVATPercentage, new ControlEntrySetting(false, false, false, 0));
            SetControlEntrySetting(chkIsReturnCostInPct, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtReturnCostPercentage, new ControlEntrySetting(false, false, false, 0));
            SetControlEntrySetting(txtReturnCostAmount, new ControlEntrySetting(false, false, false, 0));
            SetControlEntrySetting(txtRemarks, new ControlEntrySetting(true, true, false));

            SetControlEntrySetting(lblPurchaseReturn, new ControlEntrySetting(true, false));
            SetControlEntrySetting(lblSupplier, new ControlEntrySetting(true, false));

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

        #region Filter Expression Search Dialog
        protected string GetSupplierFilterExpression()
        {
            return string.Format("GCBusinessPartnerType = '{0}' AND IsDeleted = 0", Constant.BusinessObjectType.SUPPLIER);
        }

        protected string GetPurchaseReturnFilterExpression()
        {
            string filterExpression = string.Format("GCTransactionStatus = '{0}' AND GCPurchaseReturnType = '{1}'",
                    Constant.TransactionStatus.APPROVED, Constant.PurchaseReturnType.CREDIT_NOTE);
            filterExpression += string.Format(" AND PurchaseReturnID NOT IN (SELECT PurchaseReturnID FROM SupplierCreditNote WHERE GCTransactionStatus != '{0}')",
                    Constant.TransactionStatus.VOID);
            return filterExpression;
        }
        #endregion

        #region Load Entity
        protected string GetFilterExpression()
        {
            return "";
        }

        public override int OnGetRowCount()
        {
            string filterExpression = GetFilterExpression();
            return BusinessLayer.GetvSupplierCreditNoteRowCount(filterExpression);
        }

        protected override void OnLoadEntity(int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            vSupplierCreditNote entity = BusinessLayer.GetvSupplierCreditNote(filterExpression, PageIndex, "CreditNoteID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        protected override void OnLoadEntity(string keyValue, ref int PageIndex, ref bool isShowWatermark, ref string watermarkText)
        {
            string filterExpression = GetFilterExpression();
            PageIndex = BusinessLayer.GetvSupplierCreditNoteRowIndex(filterExpression, keyValue, "CreditNoteID DESC");
            vSupplierCreditNote entity = BusinessLayer.GetvSupplierCreditNote(filterExpression, PageIndex, "CreditNoteID DESC");
            EntityToControl(entity, ref isShowWatermark, ref watermarkText);
        }

        private void EntityToControl(vSupplierCreditNote entity, ref bool isShowWatermark, ref string watermarkText)
        {
            if (entity.GCTransactionStatus != Constant.TransactionStatus.OPEN)
            {
                isShowWatermark = true;
                watermarkText = entity.TransactionStatusWatermark;
            }

            if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
            {
                txtCNAmount.Enabled = true;
                txtVATPercentage.Enabled = true;
                if (entity.IsReturnCostInPercentage)
                {
                    txtReturnCostPercentage.Enabled = true;
                    txtReturnCostAmount.Enabled = false;
                }
                else
                {
                    txtReturnCostPercentage.Enabled = false;
                    txtReturnCostAmount.Enabled = true;
                }
                txtRemarks.Enabled = true;
            }
            else
            {
                txtCNAmount.Enabled = false;
                txtVATPercentage.Enabled = false;
                txtReturnCostPercentage.Enabled = false;
                txtReturnCostAmount.Enabled = false;
                txtRemarks.Enabled = false;
            }

            hdnCreditNoteID.Value = entity.CreditNoteID.ToString();
            txtCreditNoteNo.Text = entity.CreditNoteNo;
            txtCreditNoteDate.Text = entity.CreditNoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnSupplierID.Value = entity.BusinessPartnerID.ToString();
            txtSupplierCode.Text = entity.BusinessPartnerCode;
            txtSupplierName.Text = entity.BusinessPartnerName;
            hdnPurchaseReturnID.Value = entity.PurchaseReturnID.ToString();
            txtPurchaseReturnNo.Text = entity.PurchaseReturnNo;
            txtTaxInvoiceNo.Text = entity.TaxInvoiceNo;
            txtTaxInvoiceDate.Text = entity.TaxInvoiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            cboGCCreditNoteType.Value = entity.GCCreditNoteType;
            txtCNAmount.Text = entity.CNAmount.ToString(Constant.FormatString.NUMERIC_2);
            chkPPN.Checked = entity.IsIncludeVAT;
            txtVATPercentage.Text = entity.VATPercentage.ToString(Constant.FormatString.NUMERIC_2);
            chkIsReturnCostInPct.Checked = entity.IsReturnCostInPercentage;
            txtReturnCostPercentage.Text = entity.ReturnCostPercentage.ToString(Constant.FormatString.NUMERIC_2);
            txtReturnCostAmount.Text = entity.ReturnCostAmount.ToString(Constant.FormatString.NUMERIC_2);
            txtRemarks.Text = entity.Remarks;

            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnProductLineItemType.Value = entity.GCItemType;

            divCreatedBy.InnerHtml = entity.CreatedByName;
            divCreatedDate.InnerHtml = entity.CreatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            divLastUpdatedBy.InnerHtml = entity.LastUpdatedByName;
            if (entity.LastUpdatedDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) == Constant.ConstantDate.DEFAULT_NULL)
            {
                divLastUpdatedDate.InnerHtml = "";
            }
            else
            {
                divLastUpdatedDate.InnerHtml = entity.LastUpdatedDate.ToString(Constant.FormatString.DAY_DATE_TIME_FORMAT);
            }
        }
        #endregion

        #region Save
        private void ControlToEntity(SupplierCreditNote entity)
        {
            entity.CreditNoteDate = Helper.GetDatePickerValue(txtCreditNoteDate);
            entity.BusinessPartnerID = Convert.ToInt32(hdnSupplierID.Value);
            entity.PurchaseReturnID = Convert.ToInt32(hdnPurchaseReturnID.Value);
            entity.GCCreditNoteType = cboGCCreditNoteType.Value.ToString();
            entity.CNAmount = Convert.ToDecimal(txtCNAmount.Text);
            entity.IsIncludeVAT = chkPPN.Checked;
            if (entity.IsIncludeVAT)
            {
                entity.VATPercentage = Convert.ToDecimal(hdnVATPercentage.Value);
            }
            else
            {
                entity.VATPercentage = 0;
            }
            entity.IsReturnCostInPercentage = chkIsReturnCostInPct.Checked;
            entity.ReturnCostPercentage = Convert.ToDecimal(txtReturnCostPercentage.Text);
            entity.ReturnCostAmount = Convert.ToDecimal(txtReturnCostAmount.Text);
            entity.Remarks = txtRemarks.Text;

            if (hdnIsUsedProductLine.Value == "1")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierCreditNoteDao entityHdDao = new SupplierCreditNoteDao(ctx);
            try
            {
                SupplierCreditNote entity = new SupplierCreditNote();
                ControlToEntity(entity);
                entity.CreditNoteNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.SUPPLIER_CREDIT_NOTE, entity.CreditNoteDate);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                entity.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entityHdDao.Insert(entity);
                ctx.CommitTransaction();
                result = true;
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
            SupplierCreditNoteDao entityHdDao = new SupplierCreditNoteDao(ctx);
            try
            {
                SupplierCreditNote entity = entityHdDao.Get(Convert.ToInt32(hdnCreditNoteID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Nota Kredit " + entity.CreditNoteNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            return result;
        }

        protected override bool OnApproveRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierCreditNoteDao entityHdDao = new SupplierCreditNoteDao(ctx);
            try
            {
                SupplierCreditNote entity = entityHdDao.Get(Convert.ToInt32(hdnCreditNoteID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    ControlToEntity(entity);
                    entity.GCTransactionStatus = Constant.TransactionStatus.APPROVED;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Nota Kredit " + entity.CreditNoteNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            return result;
        }

        protected override bool OnVoidRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            SupplierCreditNoteDao entityHdDao = new SupplierCreditNoteDao(ctx);
            try
            {
                SupplierCreditNote entity = entityHdDao.Get(Convert.ToInt32(hdnCreditNoteID.Value));
                if (entity.GCTransactionStatus == Constant.TransactionStatus.OPEN)
                {
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityHdDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    result = false;
                    errMessage = "Nota Kredit " + entity.CreditNoteNo + " tidak dapat diubah. Harap refresh halaman ini.";
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
            return result;
        }
        #endregion
    }
}