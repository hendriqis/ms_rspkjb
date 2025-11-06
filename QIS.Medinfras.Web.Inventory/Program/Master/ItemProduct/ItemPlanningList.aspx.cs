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
    public partial class ItemPlanningList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_PLANNING;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue.Trim();

            string filterSC = string.Format("ParentID IN ('{0}') AND IsDeleted = 0 AND IsActive = 1", Constant.StandardCode.ITEM_TYPE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterSC);
            lstStandardCode.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });

            Methods.SetComboBoxField<StandardCode>(cboItemType, lstStandardCode.Where(sc => sc.StandardCodeID == Constant.ItemType.OBAT_OBATAN || sc.StandardCodeID == Constant.ItemType.BARANG_MEDIS || sc.StandardCodeID == Constant.ItemType.BARANG_UMUM || sc.StandardCodeID == Constant.ItemType.BAHAN_MAKANAN || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            filterExpression = GetFilterExpression();
            if (keyValue.Trim() != "")
            {
                int row = BusinessLayer.GetvItemProductRowIndex(filterExpression, keyValue.Trim(), "GCItemType, ItemName1") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            GetSettingParameter();

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Item Name", "Item Code", "Old Item Code", "Status Item (Active = 1, InActive = 0)" };
            fieldListValue = new string[] { "ItemName1", "ItemCode", "OldItemCode", "CustomItemStatus" };
        }

        protected string OnGetItemGroupFilterExpression()
        {
            string filterExpression = string.Format("IsDeleted = 0 AND GCItemType IN ('{0}','{1}','{2}','{3}')", Constant.ItemType.OBAT_OBATAN, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.BARANG_UMUM, Constant.ItemType.BAHAN_MAKANAN);
            return filterExpression;
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("IsDeleted = 0");

            if (cboItemType.Value != null)
            {
                filterExpression += " AND ";
                filterExpression += string.Format("GCItemType = '{0}'", cboItemType.Value.ToString());
            }

            if (hdnItemGroupID.Value != "")
                filterExpression += string.Format(" AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%')", hdnItemGroupID.Value);

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemProductRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemProduct> lstEntity = BusinessLayer.GetvItemProductList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCItemType, ItemName1");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        private void GetSettingParameter()
        {
            string filterExpression = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN, Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX);
            List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(filterExpression);
            hdnFactorXMin.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MIN).FirstOrDefault().ParameterValue;
            hdnFactorXMax.Value = lstParameter.Where(lst => lst.ParameterCode == Constant.SettingParameter.IM_FAKTOR_X_ROP_MAX).FirstOrDefault().ParameterValue;
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

        protected void cbpViewDetail1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vItemPlanning> lstHSU = BusinessLayer.GetvItemPlanningList(string.Format("ItemID = {0} AND IsDeleted = 0", hdnExpandID.Value));
            lvwDetail1.DataSource = lstHSU;
            lvwDetail1.DataBind();
        }

    }
}