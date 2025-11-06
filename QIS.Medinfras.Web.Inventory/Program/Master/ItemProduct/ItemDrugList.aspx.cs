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

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class ItemDrugList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DRUGS;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue.Trim();
            filterExpression = GetFilterExpression();
            if (keyValue.Trim() != "")
            {
                int row = BusinessLayer.GetvItemProductRowIndex(filterExpression, keyValue.Trim(), "ItemCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            GetSettingParameter();

            BindGridView(CurrPage, true, ref PageCount);
        }

        private void GetSettingParameter()
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",AppSession.UserLogin.HealthcareID,Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN, Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX);
            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(filterExpression);
            hdnFactorXMin.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN).FirstOrDefault().ParameterValue;
            hdnFactorXMax.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX).FirstOrDefault().ParameterValue;
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Item Name 1", "Item Code", "Old Item Code", "Status Item (Active = 1, InActive = 0)" };
            fieldListValue = new string[] { "ItemName1", "ItemCode", "OldItemCode", "CustomItemStatus" };
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = string.Format("GCItemType = '{0}' AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS);
            return filterExpression;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += string.Format(" AND GCItemType = '{0}' AND ", Constant.ItemGroupMaster.DRUGS);
            else
                filterExpression += string.Format(" GCItemType = '{0}' AND ", Constant.ItemGroupMaster.DRUGS);
            filterExpression += "IsDeleted = 0";
            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvDrugInfoRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vDrugInfo> lstEntity = BusinessLayer.GetvDrugInfoList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemCode ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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
            url = ResolveUrl(string.Format("~/Program/Master/ItemProduct/ItemProductEntry.aspx?page={0}", Constant.ItemGroupMaster.DRUGS));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/ItemProduct/ItemProductEntry.aspx?page={0}&id={1}", Constant.ItemGroupMaster.DRUGS, hdnID.Value.Trim()));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                bool isAllowDelete = true;
                List<ItemBalance> lstItemBalance = BusinessLayer.GetItemBalanceList(string.Format("ItemID = {0} AND IsDeleted = 0", hdnID.Value.Trim()));
                if (lstItemBalance.Count > 0)
                {
                    if (lstItemBalance.Sum(a => a.QuantityBEGIN) != 0)
                    {
                        isAllowDelete = false;
                    }
                    else if (lstItemBalance.Sum(a => a.QuantityEND) != 0)
                    {
                        isAllowDelete = false;
                    }
                }

                if (isAllowDelete)
                {
                    ItemMaster entity = BusinessLayer.GetItemMaster(Convert.ToInt32(hdnID.Value));
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateItemMaster(entity);
                    return true;
                }
                else
                {
                    errMessage = "Item tidak bisa dihapus karena memiliki nilai balance.";
                    return false;
                }
            }
            return false;
        }

        protected void cbpViewDetail1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vItemPlanning> lstHSU = BusinessLayer.GetvItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", hdnExpandID.Value));
            lvwDetail1.DataSource = lstHSU;
            lvwDetail1.DataBind();
        }

        protected void cbpViewDetail2_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vItemCost> lstHSU = BusinessLayer.GetvItemCostList(string.Format("ItemID = {0} AND IsDeleted = 0", hdnExpandID.Value));
            grdDetail2.DataSource = lstHSU;
            grdDetail2.DataBind();
        }
    }
}