using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemServiceDtEntryCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] par = param.Split('|');
            hdnItemID.Value = par[0];
            hdnObatID.Value = par[0];
            hdnBarangID.Value = par[0];

            ItemMaster entityIM = BusinessLayer.GetItemMasterList(String.Format("ItemID = {0} AND IsDeleted = 0", Convert.ToInt32(hdnItemID.Value))).FirstOrDefault();
            txtItemServiceName.Text = string.Format("{0} - {1}", entityIM.ItemCode, entityIM.ItemName1);
            txtItemServiceName2.Text = string.Format("{0} - {1}", entityIM.ItemCode, entityIM.ItemName1);
            txtItemServiceName3.Text = string.Format("{0} - {1}", entityIM.ItemCode, entityIM.ItemName1);

            if (par[1] == "fn")
            {
                hdnGCItemTypeMain.Value = Constant.ItemType.PELAYANAN;
            }
            else if (par[1] == "is")
            {
                hdnGCItemTypeMain.Value = Constant.ItemType.RADIOLOGI;
            }
            else if (par[1] == "md")
            {
                hdnGCItemTypeMain.Value = Constant.ItemType.PELAYANAN + "','" + Constant.ItemType.PENUNJANG_MEDIS;
            }
            else
            {
                hdnGCItemTypeMain.Value = Constant.ItemType.LABORATORIUM;
            }

            hdnGCItemTypeObatMain.Value = Constant.ItemType.OBAT_OBATAN + "','" + Constant.ItemType.BARANG_MEDIS;
            hdnGCItemTypeBarangMain.Value = Constant.ItemType.BARANG_UMUM + "','" + Constant.ItemType.BAHAN_MAKANAN;

            BindGridView();

            Helper.SetControlEntrySetting(txtDetailItemCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailItemQuantity, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(txtDetailObatCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailObatQuantity, new ControlEntrySetting(true, true, true), "mpEntryPopup");

            Helper.SetControlEntrySetting(txtDetailBarangCode, new ControlEntrySetting(true, true, true), "mpEntryPopup");
            Helper.SetControlEntrySetting(txtDetailBarangQuantity, new ControlEntrySetting(true, true, true), "mpEntryPopup");
        }


        private void BindGridView()
        {
            grdView.DataSource = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 AND GCItemType NOT IN ('{1}','{2}','{3}','{4}') ORDER BY ItemID ASC", hdnItemID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN));
            grdView.DataBind();
            grdViewObat.DataSource = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 AND GCItemType IN ('{1}','{2}') ORDER BY ItemID ASC", hdnObatID.Value, Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS));
            grdViewObat.DataBind();
            grdViewBarang.DataSource = BusinessLayer.GetvItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0 AND GCItemType IN ('{1}','{2}') ORDER BY ItemID ASC", hdnBarangID.Value, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN));
            grdViewBarang.DataBind();
        }


        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewObat_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void grdViewBarang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                for (int i = 0; i < e.Row.Cells.Count; i++)
                    e.Row.Cells[i].Text = GetLabel(e.Row.Cells[i].Text);
            }

        }

        protected void cbpEntryPopupViewObat_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnOAID.Value.ToString() != "")
                {
                    if (OnSaveEditObatRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddObatRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteObatRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpEntryPopupViewBarang_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnBID.Value.ToString() != "")
                {
                    if (OnSaveEditBarangRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddBarangRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteBarangRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpEntryPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();

            string param = e.Parameter;

            string result = param + "|";
            string errMessage = "";

            if (param == "save")
            {
                if (hdnID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                        result += string.Format("fail|{0}", errMessage);
                }
            }
            else if (param == "delete")
            {
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void ControlToEntity(ItemServiceDt entity)
        {
            entity.DetailItemID = Convert.ToInt32(hdnDetailItemID.Value);
            entity.GCItemType = hdnGCItemType.Value;
            entity.Quantity = Convert.ToDecimal(txtDetailItemQuantity.Text);
            entity.IsAllowChanged = chkIsAllowChangedQty.Checked;
            entity.IsAutoPosted = chkIsAutoPosted.Checked;
        }

        private void ControlToEntityObat(ItemServiceDt entity)
        {
            entity.DetailItemID = Convert.ToInt32(hdnDetailObatID.Value);
            entity.GCItemType = hdngcObatType.Value;
            entity.Quantity = Convert.ToDecimal(txtDetailObatQuantity.Text);
            entity.IsAllowChanged = chkIsAllowChangedQtyObat.Checked;
            entity.IsAutoPosted = chkObatIsAutoPosted.Checked;
        }

        private void ControlToEntityBarang(ItemServiceDt entity)
        {
            entity.DetailItemID = Convert.ToInt32(hdnDetailBarangID.Value);
            entity.GCItemType = hdngcBarangType.Value;
            entity.Quantity = Convert.ToDecimal(txtDetailBarangQuantity.Text);
            entity.IsAllowChanged = chkIsAllowChangedQtyBarang.Checked;
            entity.IsAutoPosted = chkBarangIsAutoPosted.Checked;
        }

        private bool OnSaveAddObatRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = new ItemServiceDt();
                entity.ItemID = Convert.ToInt32(hdnObatID.Value);
                ControlToEntityObat(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditObatRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnOAID.Value));
                ControlToEntityObat(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteObatRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnOAID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }


        private bool OnSaveAddBarangRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = new ItemServiceDt();
                entity.ItemID = Convert.ToInt32(hdnBarangID.Value);
                ControlToEntityBarang(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditBarangRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnBID.Value));
                ControlToEntityBarang(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteBarangRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnBID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }


        private bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = new ItemServiceDt();
                entity.ItemID = Convert.ToInt32(hdnItemID.Value);
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertItemServiceDt(entity);
                return true;
            }

            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                return false;
            }
        }

        private bool OnDeleteRecord(ref string errMessage)
        {
            try
            {
                ItemServiceDt entity = BusinessLayer.GetItemServiceDt(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemServiceDt(entity);
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