using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemServiceList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "GCItemType = '{0}' AND IsDeleted = 0";
        public override string OnGetMenuCode()
        {
            string[] param = Request.QueryString["id"].Split('|');
            switch (param[0])
            {
                case "FN": return Constant.MenuCode.Finance.ITEM_SERVICE_FN;
                case "IS": return Constant.MenuCode.Imaging.ITEM_SERVICE_IS;
                case "MD":
                    if (hdnSubMenuType.Value == "RT")
                        return Constant.MenuCode.Radiotheraphy.ITEM_SERVICE_RADIOTHERAPHY;
                    else
                        return Constant.MenuCode.MedicalDiagnostic.ITEM_SERVICE_MD;
                default: return Constant.MenuCode.Finance.ITEM_SERVICE_FN;

            }
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            string[] param = Request.QueryString["id"].Split('|');
            switch (param[0])
            {
                case "FN": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
                case "IS": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.RADIOLOGI); break;
                case "MD": 
                    filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PENUNJANG_MEDIS);
                    if (!string.IsNullOrEmpty(hdnGCSubItemType.Value))
                    {
                        filterExpression += string.Format(" AND GCSubItemType = '{0}'", hdnGCSubItemType.Value);
                    }
                    else
                    {
                        filterExpression += " AND GCSubItemType IS NULL";
                    }
                    break;
                default: filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
            }
            return filterExpression;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            String menuID = Request.QueryString["id"];
            string[] param = menuID.Split('|');

            if (param[0] == "FN")
            {
                hdnModuleID.Value = "fn";
            }
            else if (param[0] == "IS")
            {
                hdnModuleID.Value = "is";
            }
            else if (param[0] == "MD")
            {
                hdnModuleID.Value = "md";

                if (param.Count() > 1)
                {
                    hdnSubMenuType.Value = param[1];
                    if (param[1] == "RT")
                    {
                        hdnGCSubItemType.Value = Constant.SubItemType.RADIOTERAPI;
                    }
                }
            }

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvItemServiceRowIndex(filterExpression, keyValue, "ItemCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "ItemName1", "ItemCode", "OldItemCode", "ItemStatus (Active = 1, InActive = 0)" };
            fieldListValue = new string[] { "ItemName1", "ItemCode", "OldItemCode", "CustomItemStatus" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            string[] param = Request.QueryString["id"].Split('|');
            switch (param[0])
            {
                case "FN": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
                case "IS": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.RADIOLOGI); break;
                case "MD": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PENUNJANG_MEDIS); break;
                default: filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
            }
            if (!string.IsNullOrEmpty(hdnGCSubItemType.Value))
            {
                filterExpression += string.Format(" AND GCSubItemType = '{0}'", hdnGCSubItemType.Value);
            }
            else
            {
                filterExpression += " AND GCSubItemType IS NULL";
            }

            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath LIKE '%/{0}/%')", hdnItemGroupID.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemServiceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemService> lstEntity = BusinessLayer.GetvItemServiceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemCode ASC");
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
            String MenuID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemService/ItemServiceEntry.aspx?menu={0}|{1}", MenuID, hdnSubMenuType.Value));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                String MenuID = Request.QueryString["id"];
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemService/ItemServiceEntry.aspx?menu={0}|{1}&id={2}", MenuID, hdnSubMenuType.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemMasterDao entityDao = new ItemMasterDao(ctx);
            ItemServiceDtDao entityServiceDtDao = new ItemServiceDtDao(ctx);
            try
            {
                ItemMaster entity = entityDao.Get(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDao.Update(entity);

                List<ItemServiceDt> lstItemServiceDt = BusinessLayer.GetItemServiceDtList(string.Format("ItemID = {0} AND IsDeleted = 0", entity.ItemID), ctx);
                foreach (ItemServiceDt isDt in lstItemServiceDt)
                {
                    isDt.IsDeleted = true;
                    isDt.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityServiceDtDao.Update(isDt);
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

    }
}