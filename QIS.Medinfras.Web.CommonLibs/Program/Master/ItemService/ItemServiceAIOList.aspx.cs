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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ItemServiceAIOList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER_IGM = "GCItemType = '{0}' AND IsDeleted = 0";
        private const string DEFAULT_GRDVIEW_FILTER = "GCItemType = '{0}' AND IsDeleted = 0 AND IsPackageAllInOne = 1";

        public override string OnGetMenuCode()
        {
            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "FN": return Constant.MenuCode.Finance.ITEM_SERVICE_AIO_FN;
                case "IS": return Constant.MenuCode.Imaging.ITEM_SERVICE_AIO_IS;
                case "LB": return Constant.MenuCode.Laboratory.ITEM_SERVICE_AIO_LB;
                case "MD": return Constant.MenuCode.MedicalDiagnostic.ITEM_SERVICE_AIO_MD;
                default: return Constant.MenuCode.Finance.ITEM_SERVICE_AIO_FN;

            }
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "FN": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER_IGM, Constant.ItemType.PELAYANAN); break;
                case "IS": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER_IGM, Constant.ItemType.RADIOLOGI); break;
                case "LB": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER_IGM, Constant.ItemType.LABORATORIUM); break;
                case "MD": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER_IGM, Constant.ItemType.PENUNJANG_MEDIS); break;
                default: filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER_IGM, Constant.ItemType.PELAYANAN); break;
            }
            return filterExpression;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnpPackageItemID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvItemServiceRowIndex(filterExpression, keyValue, "ItemCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);

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
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Item AIO", "Kode Item AIO" };
            fieldListValue = new string[] { "ItemName1", "ItemCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            String MenuID = Request.QueryString["id"];
            switch (MenuID)
            {
                case "FN": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
                case "IS": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.RADIOLOGI); break;
                case "LB": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.LABORATORIUM); break;
                case "MD": filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PENUNJANG_MEDIS); break;
                default: filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemType.PELAYANAN); break;
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
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vItemService> lstEntity = BusinessLayer.GetvItemServiceList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex, "ItemCode ASC");
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

        private void BindGridViewDt()
        {
            string filterExpression = string.Format("IsDeleted = 0 AND ItemID = {0}", hdnpPackageItemID.Value);

            List<vItemServiceDtAIO> lstEntity = BusinessLayer.GetvItemServiceDtAIOList(filterExpression, int.MaxValue, 1, "DepartmentID DESC, ServiceUnitCode ASC, ItemCode ASC");
            grdViewDt.DataSource = lstEntity;
            grdViewDt.DataBind();
        }

        protected void cbpViewDt_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            string errMessage = "";

            if (e.Parameter != null && e.Parameter != "")
            {
                if (param[0] == "delete")
                {
                    if (OnDeleteItemServiceDt(ref errMessage))
                    {
                        result += "success|";
                        BindGridViewDt();
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
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

        private bool OnDeleteItemServiceDt(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ItemServiceDtDao entityDtDao = new ItemServiceDtDao(ctx);
            ItemServiceDtTariffDao entityTariffDao = new ItemServiceDtTariffDao(ctx);
            try
            {
                ItemServiceDt entity = entityDtDao.Get(Convert.ToInt32(hdnItemServiceDtID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                entityDtDao.Update(entity);

                string filterTariff = string.Format("ItemServiceDtID = {0} AND IsDeleted = 0", entity.ID);
                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();
                List<ItemServiceDtTariff> lstTariff = BusinessLayer.GetItemServiceDtTariffList(filterTariff, ctx);
                foreach (ItemServiceDtTariff e in lstTariff)
                {
                    e.IsDeleted = true;
                    e.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityTariffDao.Update(e);
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