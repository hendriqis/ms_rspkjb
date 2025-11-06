using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.MedicalCheckup.Program
{
    public partial class ItemServiceList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "GCItemType = '{0}' AND IsDeleted = 0";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalCheckup.ITEM_SERVICE_MC;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvItemServiceRowIndex(filterExpression, keyValue, "ItemName1 ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListValue = new string[] { "ItemName1", "ItemCode" };
            fieldListText = new string[] { "Item Name 1", "Item Code" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format(DEFAULT_GRDVIEW_FILTER, Constant.ItemGroupMaster.MEDICAL_CHECKUP); 
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

            List<vItemService> lstEntity = BusinessLayer.GetvItemServiceList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1 ASC");
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
            url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemService/ItemServiceEntry.aspx?menu={0}", MenuID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                String MenuID = Request.QueryString["id"];
                url = ResolveUrl(string.Format("~/Libs/Program/Master/ItemService/ItemServiceEntry.aspx?menu={0}&id={1}", MenuID, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateItemMaster(entity);
                return true;
            }
            return false;
        }

        //protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        //{
        //    List<vItemCost> lstHSU = BusinessLayer.GetvItemCostList(string.Format("ItemID = {0} AND IsDeleted = 0", hdnExpandID.Value));
        //    grdDetail.DataSource = lstHSU;
        //    grdDetail.DataBind();
        //}
    }
}