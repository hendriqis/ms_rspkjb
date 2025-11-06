using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxPivotGrid;
using QIS.Medinfras.Web.Common;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxEditors;


namespace QIS.Medinfras.Web.Imaging.Program
{
    public partial class RISTestStatusInfoList : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_BALANCE_INFORMATION_PER_LOCATION;
        }

        protected override void InitializeDataControl()
        {
            //base.InitializeDataControl(filterExpression, keyValue);
            List<StandardCode> lstEntitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN ('{0}','{1}','{2}') OR ParentID = '{3}'", Constant.ItemType.BARANG_UMUM, Constant.ItemType.BARANG_MEDIS, Constant.ItemType.OBAT_OBATAN, Constant.StandardCode.STOCK_ITEM_STATUS));
            List<StandardCode> lstItemType = lstEntitySC.Where(t => t.StandardCodeID == Constant.ItemType.BARANG_UMUM || t.StandardCodeID == Constant.ItemType.BARANG_MEDIS || t.StandardCodeID == Constant.ItemType.OBAT_OBATAN).ToList();
            List<StandardCode> lstStatusStock = lstEntitySC.Where(t => t.ParentID == Constant.StandardCode.STOCK_ITEM_STATUS).ToList();
            lstItemType.Insert(0, new StandardCode() { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboItemType, lstItemType, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboStockStatus, lstStatusStock, "StandardCodeName", "StandardCodeID");
            cboItemType.SelectedIndex = 0;
            cboStockStatus.SelectedIndex = 0;

            string filterExpression2 = string.Format("IsDeleted = 0");
            List<BinLocation> lstBinLocation = BusinessLayer.GetBinLocationList(filterExpression2);
            lstBinLocation.Insert(0, new BinLocation() { BinLocationID = 0, BinLocationName = "" });
            Methods.SetComboBoxField<BinLocation>(cboBinLocation, lstBinLocation, "BinLocationName", "BinLocationID");
            cboBinLocation.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }
        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtLocationCode, new ControlEntrySetting(true, false, true));
            SetControlEntrySetting(txtLocationName, new ControlEntrySetting(false, false, true));
        }

        protected string OnGetLocationFilterExpression()
        {
            return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        protected string OnGetItemGroupFilterExpression()
        {
            return string.Format("GCItemType IN ('{0}','{1}','{2}') AND IsDeleted = 0", Constant.ItemGroupMaster.DRUGS, Constant.ItemGroupMaster.SUPPLIES, Constant.ItemGroupMaster.LOGISTIC);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            //int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = "IsDeleted = 0 ";
            
            if (hdnLocationID.Value != null && hdnLocationID.Value.ToString() != ""){
                filterExpression += String.Format("AND LocationID = {0} ", hdnLocationID.Value);
                if (cboItemType.Value != null && cboItemType.Value.ToString() != "")
                    filterExpression += String.Format("AND GCItemType = '{0}' ", cboItemType.Value);
                if (hdnItemGroupID.Value != null && hdnItemGroupID.Value.ToString() != "")
                    filterExpression += String.Format("AND ItemGroupID IN (SELECT ItemGroupID FROM vItemGroupMaster WHERE DisplayPath like '%/{0}/%') ", hdnItemGroupID.Value);
                if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
                    filterExpression += String.Format("AND {0}", hdnFilterExpressionQuickSearch.Value);
                if (cboBinLocation.Value != null && cboBinLocation.Value.ToString() != "0")
                {
                    filterExpression += String.Format("AND BinLocationID = '{0}'", cboBinLocation.Value);
                }
                if (cboStockStatus.Value.ToString() == Constant.StockStatus.READY_STOCK)
                {
                    filterExpression += "AND QuantityEND > 0 ";
                }
                else if (cboStockStatus.Value.ToString() == Constant.StockStatus.NO_STOCK)
                {
                    filterExpression += "AND QuantityEND <= 0 ";
                }
            }
            else
                filterExpression += "AND 1 = 0";
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceAlternateUnit1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemBalanceAlternateUnit1> lstEntity = BusinessLayer.GetvItemBalanceAlternateUnit1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();

        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }
    }
}