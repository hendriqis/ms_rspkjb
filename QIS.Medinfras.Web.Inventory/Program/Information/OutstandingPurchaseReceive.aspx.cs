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


namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class OutstandingPurchaseReceive : BasePageTrx
    {
        protected int PageCount = 1;
        protected string filterExpressionLocation = "";
        protected string filterExpressionLocationTo = "";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Inventory.PURCHASE_RECEIVE_OUTSTANDING;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            txtDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            filterExpressionLocation = string.Format("{0};{1};{2};", AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, Constant.TransactionCode.ITEM_DISTRIBUTION);
            filterExpressionLocationTo = string.Format("{0};0;{1};", AppSession.UserLogin.HealthcareID, Constant.TransactionCode.ITEM_REQUEST);

            List<StandardCode> lstEntitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN ('{0}','{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL));
            lstEntitySC.Insert(0, new StandardCode() { StandardCodeID = "", StandardCodeName = "SEMUA" });
            Methods.SetComboBoxField(cboPurchaseReceiveStatus, lstEntitySC, "StandardCodeName", "StandardCodeID");
            cboPurchaseReceiveStatus.SelectedIndex = 0;

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

        protected string GetFilterExpression()
        {
            String startDate = Helper.GetDatePickerValue(txtDateFrom.Text).ToString("yyyy-MM-dd");
            String endDate = Helper.GetDatePickerValue(txtDateTo.Text).ToString("yyyy-MM-dd");

            string filterExpression = string.Format("ReceivedDate BETWEEN '{0}' AND '{1}'", startDate, endDate);

            if (txtFromLocationCode.Text != "" && txtFromLocationCode.Text != "null" && txtFromLocationCode.Text != null)
            {
                filterExpression += string.Format(" AND LocationID = {0}", hdnFromLocationID.Value);
            }

            if (cboPurchaseReceiveStatus.Value != "null" && cboPurchaseReceiveStatus.Value != null)
            {
                filterExpression += string.Format(" AND GCTransactionStatus = '{0}'", cboPurchaseReceiveStatus.Value);
            }
            else
            {
                filterExpression += string.Format(" AND GCTransactionStatus IN ('{0}', '{1}')", Constant.TransactionStatus.OPEN, Constant.TransactionStatus.WAIT_FOR_APPROVAL);
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            string filterExpressionItem = string.Format("ItemID IN (SELECT ItemID FROM vOutstandingPurchaseReceive WHERE {0})", filterExpression);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvOutstandingPurchaseReceiveRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<ItemMaster> lstEntity = BusinessLayer.GetItemMasterList(filterExpressionItem, 10, pageIndex, "ItemName1");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}