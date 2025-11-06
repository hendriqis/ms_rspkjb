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
    public partial class ItemGroupList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            String GCItemType = Request.QueryString["id"];

            if (GCItemType.Equals(Constant.ItemType.PENUNJANG_MEDIS)) return Constant.MenuCode.MedicalDiagnostic.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.PELAYANAN)) return Constant.MenuCode.Finance.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.RADIOLOGI)) return Constant.MenuCode.Imaging.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.LABORATORIUM)) return Constant.MenuCode.Laboratory.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.MEDICAL_CHECKUP)) return Constant.MenuCode.MedicalCheckup.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.BAHAN_MAKANAN)) return Constant.MenuCode.Nutrition.ITEM_GROUP;
            else if (GCItemType.Equals(Constant.ItemType.OBAT_OBATAN)) return Constant.MenuCode.Inventory.ITEM_GROUP_DRUGS;
            else if (GCItemType.Equals(Constant.ItemType.BARANG_MEDIS)) return Constant.MenuCode.Inventory.ITEM_GROUP_SUPPLIES;
            else return Constant.MenuCode.Inventory.ITEM_GROUP_LOGISTIC;

        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (hdnGCItemType.Value.Equals(Constant.ItemGroupMaster.DRUGS) ||
                hdnGCItemType.Value.Equals(Constant.ItemGroupMaster.SUPPLIES) ||
                hdnGCItemType.Value.Equals(Constant.ItemGroupMaster.LOGISTIC) ||
                hdnGCItemType.Value.Equals(Constant.ItemGroupMaster.NUTRITION))
            {
                grdView.Columns[5].Visible = false;
            }
            else grdView.Columns[5].Visible = true;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            String GCItemType = Request.QueryString["id"];
            hdnGCItemType.Value = GCItemType;
            hdnQueryItem.Value = String.Format("GCItemType = '{0}'", GCItemType);
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvItemGroupMasterRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Grup", "Kode Grup" };
            fieldListValue = new string[] { "ItemGroupName1", "ItemGroupCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("{0} AND IsHeader = 1 AND IsDeleted = 0", hdnQueryItem.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemGroupMasterRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemGroupMaster> lstEntity = BusinessLayer.GetvItemGroupMasterList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroup/ItemGroupEntry.aspx?id={0}", hdnGCItemType.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemGroup/ItemGroupEntry.aspx?id={0}|{1}", hdnGCItemType.Value, hdnID.Value));
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

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string filterExpression = string.Format("ItemGroupID = {0}", hdnExpandID.Value);
            List<ItemGroupMaster> lstDt = BusinessLayer.GetItemGroupMasterList(filterExpression);
            lvwViewDetail.DataSource = lstDt;
            lvwViewDetail.DataBind();
        }
    }
}