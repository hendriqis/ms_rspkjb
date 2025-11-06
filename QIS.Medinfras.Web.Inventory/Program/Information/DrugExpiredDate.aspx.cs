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
    public partial class DrugExpiredDate : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.DRUGS_EXPIRED_DATE;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            //base.InitializeDataControl(filterExpression, keyValue);
            BindGridView(1, true, ref PageCount);
            List<Variable> lstVariable = new List<Variable>();
            lstVariable.Add(new Variable { Code = "1", Value = "All" });
            lstVariable.Add(new Variable { Code = "2", Value = "Almost Expired" });
            Methods.SetRadioButtonListField(rblShowStatus, lstVariable, "Value", "Code");
            rblShowStatus.SelectedIndex = 0;

            hdnRangeExpDate.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.RANGE_EXPIRED_DATE).ParameterValue;

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        public void rptListItem_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) 
            {
                vItemProductExpired entity = (vItemProductExpired)e.Item.DataItem;

                string filterExpression = "";
                if(rblShowStatus.SelectedValue != "2")
                    filterExpression = String.Format("ID IN (SELECT ID FROM PurchaseReceiveDt WHERE ItemID = {0} AND GCItemDetailStatus != '{1}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus IN ('{2}','{3}'))) AND ExpiredDate >= '{4}'", entity.ItemID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED, DateTime.Now);
                else
                    filterExpression = String.Format("ID IN (SELECT ID FROM PurchaseReceiveDt WHERE ItemID = {0} AND GCItemDetailStatus != '{1}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus IN ('{2}','{3}'))) AND (ExpiredDate >= '{4}' AND ExpiredDate <= '{5}')", entity.ItemID, Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED, DateTime.Now, DateTime.Now.AddMonths(Convert.ToInt32(hdnRangeExpDate.Value)));

                List<PurchaseReceiveDtExpired> lstPRDtExp = BusinessLayer.GetPurchaseReceiveDtExpiredList(filterExpression);
                List<PurchaseReceiveDtExpired> lst = (from p in lstPRDtExp
                                                          select new PurchaseReceiveDtExpired { ExpiredDate = p.ExpiredDate}).GroupBy(p => p.ExpiredDateInString).Select(p => p.First()).ToList();
                Repeater rpt = (Repeater)e.Item.FindControl("rptExpDate");
                rpt.DataSource = lst;
                rpt.DataBind();
            }
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = String.Format("GCItemType = '{0}' AND IsControlExpired = 1 AND IsDeleted = 0",Constant.ItemGroupMaster.DRUGS);
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            if (rblShowStatus.SelectedValue != "2")
            {
                filterExpression += String.Format(" AND ItemID IN (SELECT DISTINCT ItemID FROM PurchaseReceiveDt WHERE GCItemDetailStatus != '{0}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus IN ('{1}','{2}')) AND ID IN (SELECT ID FROM PurchaseReceiveDtExpired WHERE ExpiredDate >= '{3}'))", Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED, DateTime.Now);
            }
            else
            {
                filterExpression += String.Format(" AND ItemID IN (SELECT DISTINCT ItemID FROM PurchaseReceiveDt WHERE GCItemDetailStatus != '{0}' AND PurchaseReceiveID IN (SELECT PurchaseReceiveID FROM PurchaseReceiveHd WHERE GCTransactionStatus IN ('{1}','{2}')) AND ID IN (SELECT ID FROM PurchaseReceiveDtExpired WHERE ExpiredDate >= '{3}' AND ExpiredDate <= '{4}'))", Constant.TransactionStatus.VOID, Constant.TransactionStatus.APPROVED, Constant.TransactionStatus.CLOSED, DateTime.Now, DateTime.Now.AddMonths(Convert.ToInt32(hdnRangeExpDate.Value)));
            }

            if (isCountPageCount)
            {
                
                int rowCount = BusinessLayer.GetvItemProductExpiredRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vItemProductExpired> lstItemBalance = BusinessLayer.GetvItemProductExpiredList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ItemName1");
            rptListItem.DataSource = lstItemBalance;
            rptListItem.DataBind();
        }


    }
}