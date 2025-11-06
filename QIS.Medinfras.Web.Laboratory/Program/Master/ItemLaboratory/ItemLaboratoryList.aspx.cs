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

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class ItemLaboratoryList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        private const string DEFAULT_GRDVIEW_FILTER = "GCItemType = '{0}' AND IsDeleted = 0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Laboratory.ITEM_SERVICE_LB;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetItemMasterRowIndex(filterExpression, keyValue, "ItemName1 ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            String menuID = Request.QueryString["id"];
            if (menuID == "FN")
            {
                hdnModuleID.Value = "fn";
            }
            else if (menuID == "IS")
            {
                hdnModuleID.Value = "is";
            }
            else if (menuID == "MD")
            {
                hdnModuleID.Value = "md";
            }
            else
            {
                hdnModuleID.Value = "lb";
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "ItemName1", "ItemCode", "OldItemCode", "ItemStatus (Active = 1, InActive = 0)" };
            fieldListValue = new string[] { "ItemName1", "ItemCode", "OldItemCode", "CustomItemStatus" };
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemType.LABORATORIUM);
            return filterExpression;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.LABORATORIUM);
            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetItemMasterRowCount(filterExpression);
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
            url = ResolveUrl("~/Program/Master/ItemLaboratory/ItemLaboratoryEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/ItemLaboratory/ItemLaboratoryEntry.aspx?id={0}", hdnID.Value));
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