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
using System.Web.UI.HtmlControls;


namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceiveInfoPerItem : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.ITEM_PURCHASE_RECEIVE_PER_ITEM;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected string OnGetLocationFilterExpression()
        {
            return string.Format("{0};{1};;", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID);
        }

        protected override void InitializeDataControl()
        {
            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.PURCHASE_RECEIVE);
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
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

        public string GetFilterExpression()
        {
            string filterExpression = string.Format("LocationID = {0} AND ReceivedDate BETWEEN '{1}' AND '{2}' AND GCItemDetailStatus != '{3}'", hdnLocationID.Value, Helper.GetDatePickerValue(txtDateFrom.Text), Helper.GetDatePickerValue(txtDateTo.Text), Constant.TransactionStatus.VOID);
            
            if (hdnFilterExpressionQuickSearch.Value == "Search")
                hdnFilterExpressionQuickSearch.Value = " ";
            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            return filterExpression;
        }

        public string GetPurchaseReceiveDtFilterExpression() 
        {
            return Request.Form[hdnFilterExpression.UniqueID];
        }

        List<vPurchaseReceiveDt> lstPurchaseReceiveDt = null;
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string purchaseReceiveDtFilterExpression = "";
            string filterExpression = "";
            if (hdnLocationID.Value != "")
            {
                purchaseReceiveDtFilterExpression = GetFilterExpression();
                hdnFilterExpression.Value = purchaseReceiveDtFilterExpression;
                filterExpression = String.Format("LocationID = {0} AND ItemID IN (SELECT ItemID FROM vPurchaseReceiveDt WHERE {1})", hdnLocationID.Value, purchaseReceiveDtFilterExpression);
            }
            else
                filterExpression = "1 = 0";
            
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemBalanceRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vItemBalance> lstItemBalance = BusinessLayer.GetvItemBalanceList(filterExpression, 10, pageIndex, "ItemName1");
            if (lstItemBalance.Count > 0)
            {
                string lstItemID = "";
                foreach (vItemBalance itemBalance in lstItemBalance)
                {
                    if (lstItemID != "")
                        lstItemID += ",";
                    lstItemID += itemBalance.ItemID.ToString();
                }
                lstPurchaseReceiveDt = BusinessLayer.GetvPurchaseReceiveDtList(string.Format("{0} AND ItemID IN ({1})", purchaseReceiveDtFilterExpression, lstItemID));
            }
            grdView.DataSource = lstItemBalance;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vItemBalance entity = e.Row.DataItem as vItemBalance;
                if (lstPurchaseReceiveDt != null)
                {
                    decimal usedQty = lstPurchaseReceiveDt.Where(p => p.ItemID == entity.ItemID).Sum(p => p.cfQtyConversion);
                    HtmlGenericControl divUsedQuantity = e.Row.FindControl("divUsedQuantity") as HtmlGenericControl;
                    divUsedQuantity.InnerHtml = string.Format("{0} {1}", usedQty.ToString("N2"), entity.ItemUnit);
                }
            }
        }
    }
}