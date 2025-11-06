using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemGroupListV2 : BasePageList
    {
        public override string OnGetMenuCode()
        {
            String GCItemType = hdnGCItemType.Value;
            String MenuType = hdnSubMenuType.Value;

            if (GCItemType.Equals(Constant.ItemType.PENUNJANG_MEDIS))
            {
                if (hdnSubMenuType.Value == "RT")
                {
                    return Constant.MenuCode.Radiotheraphy.ITEM_GROUP;
                }
                else
                {
                    return Constant.MenuCode.MedicalDiagnostic.ITEM_GROUP;
                }
            }
            else if (GCItemType.Equals(Constant.ItemType.PELAYANAN))
            {
                return Constant.MenuCode.Finance.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.RADIOLOGI))
            {
                return Constant.MenuCode.Imaging.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.LABORATORIUM))
            {
                return Constant.MenuCode.Laboratory.ITEM_GROUP;
            }
            else if (GCItemType.Equals(Constant.ItemType.MEDICAL_CHECKUP))
            {
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
            else if (GCItemType.Equals(Constant.ItemType.OBAT_OBATAN))
            {
                return Constant.MenuCode.Inventory.ITEM_GROUP_DRUGS;
            }
            else if (GCItemType.Equals(Constant.ItemType.BARANG_MEDIS))
            {
                return Constant.MenuCode.Inventory.ITEM_GROUP_SUPPLIES;
            }
            else
            {
                return Constant.MenuCode.Inventory.ITEM_GROUP_LOGISTIC;
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
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

            if (!string.IsNullOrEmpty(hdnGCSubItemType.Value))
            {
                hdnQueryItem.Value = String.Format("GCItemType = '{0}' AND GCSubItemType = '{1}'", hdnGCItemType.Value, hdnGCSubItemType.Value);
            }
            else
            {
                hdnQueryItem.Value = String.Format("GCItemType = '{0}' AND GCSubItemType IS NULL", hdnGCItemType.Value);
            }

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();

            if (hdnGCItemType.Value.Equals(Constant.ItemType.PELAYANAN) || hdnGCItemType.Value.Equals(Constant.ItemType.PENUNJANG_MEDIS) || hdnGCItemType.Value.Equals(Constant.ItemType.LABORATORIUM) || hdnGCItemType.Value.Equals(Constant.ItemType.RADIOLOGI))
            {
                tdItemService.Style.Remove("visibility");
                Helper.SetControlEntrySetting(txtCITOAmountCtl, new ControlEntrySetting(true, false, true, string.Empty), "mpEntryPopup");
            }

            Helper.SetControlEntrySetting(txtItemGroupCode, new ControlEntrySetting(true, false, true, string.Empty), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtItemGroupName, new ControlEntrySetting(true, true, true, string.Empty), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtPrintOrder, new ControlEntrySetting(true, false, true, string.Empty), "mpEntryPopup");

            if (hdnGCItemType.Value != Constant.ItemType.OBAT_OBATAN && hdnGCItemType.Value != Constant.ItemType.BARANG_MEDIS && hdnGCItemType.Value != Constant.ItemType.BARANG_UMUM && hdnGCItemType.Value != Constant.ItemType.BAHAN_MAKANAN)
            {
                trProductLine.Style.Add("display", "none");
            }

            if (hdnGCItemType.Value == Constant.ItemType.OBAT_OBATAN || hdnGCItemType.Value == Constant.ItemType.BARANG_MEDIS || hdnGCItemType.Value == Constant.ItemType.BARANG_UMUM || hdnGCItemType.Value == Constant.ItemType.BAHAN_MAKANAN)
            {
                trProductLine.Style.Remove("display");

                Helper.SetControlEntrySetting(txtProductLineCodeCtl, new ControlEntrySetting(true, true, true, string.Empty), "mpEntryPopup");
                Helper.SetControlEntrySetting(txtProductLineNameCtl, new ControlEntrySetting(false, false, true, string.Empty), "mpEntryPopup");
            }


            BindGridView();
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Kelompok / Nama Sub-Kelompok", "Kode Kelompok / Kode Sub-Kelompok" };
            fieldListValue = new string[] { "ItemGroupName1", "ItemGroupCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }

            filterExpression += string.Format("{0} AND IsHeader = 1 AND IsDeleted = 0", hdnQueryItem.Value);

            return filterExpression;
        }

        private void BindGridView()
        {
            string filterExpression = GetFilterExpression();

            List<vItemGroupMaster> lstEntity = BusinessLayer.GetvItemGroupMasterList(filterExpression, int.MaxValue, 1, "ItemGroupCode");
            grdViewIGHD.DataSource = lstEntity;
            grdViewIGHD.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroup/ItemGroupEntry.aspx?id={0}|{1}", hdnGCItemType.Value, hdnSubMenuType.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroup/ItemGroupEntry.aspx?id={0}|{1}|{2}", hdnGCItemType.Value, hdnSubMenuType.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnID.Value));
                List<ItemGroupMaster> lstChild = BusinessLayer.GetItemGroupMasterList(string.Format("ParentID = {0} AND IsDeleted = 0", entity.ItemGroupID));
                if (lstChild.Count() == 0)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemGroupMaster(entity);
                    return true;
                }
                else
                {
                    errMessage = "Maaf, kelompok ini tidak bisa dihapus karena masih memiliki sub-kelompok yang belum dihapus.";
                    return false;
                }
            }
            return false;
        }

        #region Sub Item Group Master
        private void BindGridViewDt()
        {
            string filterExpression = string.Format("ParentID = {0} AND IsHeader = 0 AND IsDeleted = 0", hdnID.Value);
            if (hdnFilterExpression.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpression.Value);
            }

            List<vItemGroupMaster> lstEntity = BusinessLayer.GetvItemGroupMasterList(filterExpression, int.MaxValue, 1, "ItemGroupCode");
            grdViewIGDT.DataSource = lstEntity;
            grdViewIGDT.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "save")
                {
                    if (hdnItemGroupID.Value.ToString() != "")
                    {
                        if (OnSaveEditRecordDt(ref errMessage))
                        {
                            result += "success";
                            BindGridViewDt();
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                    else
                    {
                        if (OnSaveAddRecordDt(ref errMessage))
                        {
                            result += "success";
                            BindGridViewDt();
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else if (param[0] == "delete")
                {
                    if (OnDeleteRecordDt(ref errMessage))
                    {
                        result += "success";
                        BindGridViewDt();
                    }
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else // refresh
                {

                    BindGridViewDt();
                    result = "refresh|";
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        private void ControlToEntity(ItemGroupMaster entity)
        {
            entity.ItemGroupCode = txtItemGroupCode.Text;
            entity.ItemGroupName1 = txtItemGroupName.Text;
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
            entity.Remarks = txtNotes.Text;

            entity.ParentID = Convert.ToInt32(hdnID.Value);
            entity.IsHeader = false;

            if (hdnProductLineIDCtl.Value != "" && hdnProductLineIDCtl.Value != "0")
            {
                entity.ProductLineID = Convert.ToInt32(hdnProductLineIDCtl.Value.ToString());
            }
            else
            {
                entity.ProductLineID = null;
            }

            entity.IsCitoInPercentage = chkIsCITOInPercentageCtl.Checked;
            if (!string.IsNullOrEmpty(txtCITOAmountCtl.Text))
                entity.CitoAmount = Convert.ToDecimal(txtCITOAmountCtl.Text);
            else
                entity.CitoAmount = 0;

            entity.IsComplicationInPercentage = chkIsComplicationInPercentageCtl.Checked;
            if (!string.IsNullOrEmpty(txtComplicationAmountCtl.Text))
                entity.ComplicationAmount = Convert.ToDecimal(txtComplicationAmountCtl.Text);
            else
                entity.ComplicationAmount = 0;

            entity.IsPrintWithDoctorName = chkIsPrintWithDoctorNameCtl.Checked;

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

        private bool OnSaveAddRecordDt(ref string errMessage)
        {
            try
            {
                ItemGroupMaster entity = new ItemGroupMaster();
                ControlToEntity(entity);
                entity.CreatedBy = AppSession.UserLogin.UserID;
                entity.IsHeader = false;
                BusinessLayer.InsertItemGroupMaster(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecordDt(ref string errMessage)
        {
            try
            {
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnItemGroupID.Value));
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

        private bool OnDeleteRecordDt(ref string errMessage)
        {
            try
            {
                ItemGroupMaster entity = BusinessLayer.GetItemGroupMaster(Convert.ToInt32(hdnItemGroupID.Value));
                entity.IsDeleted = true;
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