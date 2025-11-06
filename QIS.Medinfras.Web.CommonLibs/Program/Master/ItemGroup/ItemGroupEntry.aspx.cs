using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemGroupEntry : BasePageEntry
    {
        public override string OnGetMenuCode()
        {
            String GCItemType = hdnGCItemType.Value;
            String MenuType = hdnSubMenuType.Value;

            if (GCItemType.Equals(Constant.ItemType.OBAT_OBATAN))
            {
                return Constant.MenuCode.Inventory.ITEM_GROUP_DRUGS;
            }
            else if (GCItemType.Equals(Constant.ItemType.BARANG_MEDIS)) {
                return Constant.MenuCode.Inventory.ITEM_GROUP_SUPPLIES;
            }
            else if (GCItemType.Equals(Constant.ItemType.BARANG_UMUM)) {
                return Constant.MenuCode.Inventory.ITEM_GROUP_LOGISTIC;
            }
            else if (GCItemType.Equals(Constant.ItemType.LABORATORIUM)) {
                return Constant.MenuCode.Laboratory.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.RADIOLOGI)) {
                return Constant.MenuCode.Imaging.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.PENUNJANG_MEDIS)) {
                if (hdnSubMenuType.Value == "RT")
                {
                    return Constant.MenuCode.Radiotheraphy.ITEM_GROUP;
                }
                else
                {
                    return Constant.MenuCode.MedicalDiagnostic.ITEM_GROUP;
                }
            }
            else if (GCItemType.Equals(Constant.ItemType.MEDICAL_CHECKUP)) {
                return Constant.MenuCode.MedicalCheckup.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.BAHAN_MAKANAN))
            {
                if (MenuType == Constant.Module.INVENTORY)
                {
                    return Constant.MenuCode.Inventory.ITEM_GROUP_NUTRITION;
                }
                else
                {
                    return Constant.MenuCode.Nutrition.ITEM_GROUP;
                }
            }
            else {
                return Constant.MenuCode.Finance.ITEM_GROUP;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                hdnGCItemType.Value = param[0];
                hdnSubMenuType.Value = param[1];
                 if (param[1] == "RT")
                {
                    hdnGCSubItemType.Value = Constant.SubItemType.RADIOTERAPI;
                }
            }
            else
            {
                hdnGCItemType.Value = param[0];
            }
            chkIsHeader.Checked = true;

            if (hdnGCItemType.Value != Constant.ItemType.OBAT_OBATAN && hdnGCItemType.Value != Constant.ItemType.BARANG_MEDIS && hdnGCItemType.Value != Constant.ItemType.BARANG_UMUM && hdnGCItemType.Value != Constant.ItemType.BAHAN_MAKANAN)
            {
                trCITO.Style.Remove("display");
                trIsPrintWithDoctorName.Style.Remove("display");

                trProductLine.Style.Add("display", "none");
                trDiscount.Style.Add("display", "none");
                trPPN.Style.Add("display", "none");
            }

            if (hdnGCItemType.Value == Constant.ItemType.OBAT_OBATAN || hdnGCItemType.Value == Constant.ItemType.BARANG_MEDIS || hdnGCItemType.Value == Constant.ItemType.BARANG_UMUM || hdnGCItemType.Value == Constant.ItemType.BAHAN_MAKANAN)
            {
                trProductLine.Style.Remove("display");
            }


            if (param.Length > 2)
            {
                IsAdd = false;
                hdnID.Value = param[2];
                vItemGroupMaster entity = BusinessLayer.GetvItemGroupMasterList(string.Format("ItemGroupID = {0}", hdnID.Value)).FirstOrDefault();
                EntityToControl(entity);
            }
            else
            {
                IsAdd = true;
            }
            txtItemGroupName1.Focus();

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtItemGroupName2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtPrintOrder, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(hdnRevenueSharingID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtRevenueSharingCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtRevenueSharingName, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(chkIsDiscountCalculateHNA, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsPPNCalculateHNA, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(chkIsHeader, new ControlEntrySetting(false, false, false, true));
            SetControlEntrySetting(txtCITOAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsCITOInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtComplicationAmount, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(chkIsComplicationInPercentage, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(hdnBillingGroupID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtBillingGroupCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtBillingGroupName, new ControlEntrySetting(false, false, false));

            #region Pengaturan Perkiraan untuk Aktiva Tetap
            SetControlEntrySetting(hdnGLAccountID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountCode1, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountName1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt1, new ControlEntrySetting(false, true));
            SetControlEntrySetting(hdnSubLedgerDtID1, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDtCode1, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDtName1, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccountID2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountCode2, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountName2, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt2, new ControlEntrySetting(false, true));
            SetControlEntrySetting(hdnSubLedgerDtID2, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDtCode2, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDtName2, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccountID3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSearchDialogTypeName3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(hdnSubLedgerID3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountCode3, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountName3, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(lblSubLedgerDt3, new ControlEntrySetting(false, true));
            SetControlEntrySetting(hdnSubLedgerDtID3, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtSubLedgerDtCode3, new ControlEntrySetting(false, false, false));
            SetControlEntrySetting(txtSubLedgerDtName3, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnGLAccountDiscountID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtGLAccountDiscountCode, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtGLAccountDiscountName, new ControlEntrySetting(false, false, false));

            SetControlEntrySetting(hdnProductLineID, new ControlEntrySetting(true, true));
            SetControlEntrySetting(txtProductLineCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtProductLineName, new ControlEntrySetting(false, false, true));
            #endregion
        }

        private void EntityToControl(vItemGroupMaster entity)
        {
            txtItemGroupCode.Text = entity.ItemGroupCode;
            txtItemGroupName1.Text = entity.ItemGroupName1;
            txtItemGroupName2.Text = entity.ItemGroupName2;
            txtPrintOrder.Text = entity.PrintOrder.ToString();
            hdnRevenueSharingID.Value = entity.RevenueSharingID.ToString();
            txtRevenueSharingCode.Text = entity.RevenueSharingCode;
            txtRevenueSharingName.Text = entity.RevenueSharingName;

            chkIsDiscountCalculateHNA.Checked = entity.IsDiscountCalculateHNA;
            chkIsPPNCalculateHNA.Checked = entity.IsPPNCalculateHNA;
            chkIsHeader.Checked = entity.IsHeader;
            chkIsPrintWithDoctorName.Checked = entity.IsPrintWithDoctorName;
            txtCITOAmount.Text = entity.CitoAmount.ToString();
            chkIsCITOInPercentage.Checked = entity.IsCitoInPercentage;
            txtComplicationAmount.Text = entity.ComplicationAmount.ToString();
            chkIsComplicationInPercentage.Checked = entity.IsComplicationInPercentage;
            hdnProductLineID.Value = entity.ProductLineID.ToString();
            txtProductLineCode.Text = entity.ProductLineCode;
            txtProductLineName.Text = entity.ProductLineName;
            hdnBillingGroupID.Value = entity.BillingGroupID.ToString();
            txtBillingGroupCode.Text = entity.BillingGroupCode;
            txtBillingGroupName.Text = entity.BillingGroupName1;

            #region Pengaturan Perkiraan untuk Aktiva Tetap

            #region GL Account 1
            hdnGLAccountID1.Value = entity.GLAccount1.ToString();
            txtGLAccountCode1.Text = entity.GLAccountNo1;
            txtGLAccountName1.Text = entity.GLAccountName1;

            hdnSubLedgerID1.Value = entity.SubLedgerID1.ToString();
            hdnSearchDialogTypeName1.Value = entity.SearchDialogTypeName1;
            hdnIDFieldName1.Value = entity.IDFieldName1;
            hdnCodeFieldName1.Value = entity.CodeFieldName1;
            hdnDisplayFieldName1.Value = entity.DisplayFieldName1;
            hdnMethodName1.Value = entity.MethodName1;
            hdnFilterExpression1.Value = entity.FilterExpression1;

            hdnSubLedgerDtID1.Value = entity.SubLedger1.ToString();
            txtSubLedgerDtCode1.Text = entity.SubLedgerCode1;
            txtSubLedgerDtName1.Text = entity.SubLedgerName1;

            if (hdnSubLedgerDtID1.Value != "" && hdnSubLedgerDtID1.Value != "0")
            {
                lblSubLedgerDt1.Attributes.Add("class", "lblLink");
            }
            else
            {
                lblSubLedgerDt1.Attributes.Add("class", "lblDisabled");
            }

            #endregion

            #region GL Account 2
            hdnGLAccountID2.Value = entity.GLAccount2.ToString();
            txtGLAccountCode2.Text = entity.GLAccountNo2;
            txtGLAccountName2.Text = entity.GLAccountName2;

            hdnSubLedgerID2.Value = entity.SubLedgerID2.ToString();
            hdnSearchDialogTypeName2.Value = entity.SearchDialogTypeName2;
            hdnIDFieldName2.Value = entity.IDFieldName2;
            hdnCodeFieldName2.Value = entity.CodeFieldName2;
            hdnDisplayFieldName2.Value = entity.DisplayFieldName2;
            hdnMethodName2.Value = entity.MethodName2;
            hdnFilterExpression2.Value = entity.FilterExpression2;

            hdnSubLedgerDtID2.Value = entity.SubLedger2.ToString();
            txtSubLedgerDtCode2.Text = entity.SubLedgerCode2;
            txtSubLedgerDtName2.Text = entity.SubLedgerName2;

            if (hdnSubLedgerDtID2.Value != "" && hdnSubLedgerDtID2.Value != "0")
            {
                lblSubLedgerDt2.Attributes.Add("class", "lblLink");
            }
            else
            {
                lblSubLedgerDt2.Attributes.Add("class", "lblDisabled");
            }
            #endregion

            #region GL Account 3
            hdnGLAccountID3.Value = entity.GLAccount3.ToString();
            txtGLAccountCode3.Text = entity.GLAccountNo3;
            txtGLAccountName3.Text = entity.GLAccountName3;

            hdnSubLedgerID3.Value = entity.SubLedgerID3.ToString();
            hdnSearchDialogTypeName3.Value = entity.SearchDialogTypeName3;
            hdnIDFieldName3.Value = entity.IDFieldName3;
            hdnCodeFieldName3.Value = entity.CodeFieldName3;
            hdnDisplayFieldName3.Value = entity.DisplayFieldName3;
            hdnMethodName3.Value = entity.MethodName3;
            hdnFilterExpression3.Value = entity.FilterExpression3;

            hdnSubLedgerDtID3.Value = entity.SubLedger3.ToString();
            txtSubLedgerDtCode3.Text = entity.SubLedgerCode3;
            txtSubLedgerDtName3.Text = entity.SubLedgerName3;

            if (hdnSubLedgerDtID3.Value != "" && hdnSubLedgerDtID3.Value != "0")
            {
                lblSubLedgerDt3.Attributes.Add("class", "lblLink");
            }
            else
            {
                lblSubLedgerDt3.Attributes.Add("class", "lblDisabled");
            }
            #endregion

            hdnGLAccountDiscountID.Value = entity.GLAccountDiscount.ToString();
            txtGLAccountDiscountCode.Text = entity.GLAccountDiscountNo;
            txtGLAccountDiscountName.Text = entity.GLAccountDiscountName;

            #endregion
        }

        private void ControlToEntity(ItemGroupMaster entity)
        {
            entity.ItemGroupCode = txtItemGroupCode.Text;
            entity.ItemGroupName1 = txtItemGroupName1.Text;
            entity.ItemGroupName2 = txtItemGroupName2.Text;
            entity.GCItemType = hdnGCItemType.Value;
            if (!string.IsNullOrEmpty(hdnGCSubItemType.Value))
            {
                entity.GCSubItemType = hdnGCSubItemType.Value;
            }
            entity.PrintOrder = Convert.ToInt16(txtPrintOrder.Text);
            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0")
            {
                entity.RevenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }
            else
            {
                entity.RevenueSharingID = null;
            }
            entity.ParentID = null;
            entity.IsDiscountCalculateHNA = chkIsDiscountCalculateHNA.Checked;
            entity.IsPPNCalculateHNA = chkIsPPNCalculateHNA.Checked;
            entity.IsHeader = chkIsHeader.Checked;
            entity.CitoAmount = txtCITOAmount.Text.Equals(string.Empty) ? 0 : Convert.ToDecimal(txtCITOAmount.Text);
            entity.IsCitoInPercentage = chkIsCITOInPercentage.Checked;
            entity.ComplicationAmount = txtComplicationAmount.Text.Equals(string.Empty) ? 0 : Convert.ToDecimal(txtComplicationAmount.Text);
            entity.IsComplicationInPercentage = chkIsComplicationInPercentage.Checked;
            entity.IsPrintWithDoctorName = chkIsPrintWithDoctorName.Checked;
            entity.Remarks = txtNotes.Text;

            if (hdnProductLineID.Value != "" && hdnProductLineID.Value != "0")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineID.Value.ToString());
            }
            else 
            {
                entity.ProductLineID = null;
            }

            if (hdnBillingGroupID.Value != "" && hdnBillingGroupID.Value != "0")
            {
                entity.BillingGroupID = Convert.ToInt32(hdnBillingGroupID.Value);
            }
            else
            {
                entity.BillingGroupID = null;
            }


            //Account 1
            if (hdnGLAccountID1.Value != "" && hdnGLAccountID1.Value != "0")
            {
                entity.GLAccount1 = Convert.ToInt32(hdnGLAccountID1.Value);
            }
            else
            {
                entity.GLAccount1 = null;
            }

            if (hdnSubLedgerDtID1.Value != "" && hdnSubLedgerDtID1.Value != "0")
            {
                entity.SubLedger1 = Convert.ToInt32(hdnSubLedgerDtID1.Value);
            }
            else
            {
                entity.SubLedger1 = null;
            }

            //Account 2
            if (hdnGLAccountID2.Value != "" && hdnGLAccountID2.Value != "0")
            {
                entity.GLAccount2 = Convert.ToInt32(hdnGLAccountID2.Value);
            }
            else
            {
                entity.GLAccount2 = null;
            }

            if (hdnSubLedgerDtID2.Value != "" && hdnSubLedgerDtID2.Value != "0")
            {
                entity.SubLedger2 = Convert.ToInt32(hdnSubLedgerDtID2.Value);
            }
            else
            {
                entity.SubLedger2 = null;
            }

            //Account 3
            if (hdnGLAccountID3.Value != "" && hdnGLAccountID3.Value != "0")
            {
                entity.GLAccount3 = Convert.ToInt32(hdnGLAccountID3.Value);
            }
            else
            {
                entity.GLAccount3 = null;
            }

            if (hdnSubLedgerDtID3.Value != "" && hdnSubLedgerDtID3.Value != "0")
            {
                entity.SubLedger3 = Convert.ToInt32(hdnSubLedgerDtID3.Value);
            }
            else
            {
                entity.SubLedger3 = null;
            }

            //Account Disount
            if (hdnGLAccountDiscountID.Value != "" && hdnGLAccountDiscountID.Value != "0")
            {
                entity.GLAccountDiscount = Convert.ToInt32(hdnGLAccountDiscountID.Value);
            }
            else
            {
                entity.GLAccountDiscount = null;
            }
        }

        protected override bool OnBeforeSaveAddRecord(ref string errMessage)
        {
            errMessage = string.Empty;

            string FilterExpression = string.Format("ItemGroupCode = '{0}' AND IsDeleted = 0", txtItemGroupCode.Text);
            List<ItemGroupMaster> lst = BusinessLayer.GetItemGroupMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item Group with Code " + txtItemGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnBeforeSaveEditRecord(ref string errMessage)
        {
            errMessage = string.Empty;
            string FilterExpression = string.Format("ItemGroupCode = '{0}' AND ItemGroupID != {1} AND IsDeleted = 0", txtItemGroupCode.Text, hdnID.Value);
            List<ItemGroupMaster> lst = BusinessLayer.GetItemGroupMasterList(FilterExpression);

            if (lst.Count > 0)
                errMessage = " Item Group with Code " + txtItemGroupCode.Text + " is already exist!";

            return (errMessage == string.Empty);
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            ItemGroupMasterDao entityDao = new ItemGroupMasterDao(ctx);
            bool result = false;
            try
            {
                ItemGroupMaster entity = new ItemGroupMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                retval = entityDao.InsertReturnPrimaryKeyID(entity).ToString();
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

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemGroupMaster(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }
    }
}