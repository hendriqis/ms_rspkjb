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
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Inventory.Program;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseReceiveVoidInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            String[] lstParam = param.Split('|');
            hdnLocationID.Value = lstParam[0];
            txtLocation.Text = lstParam[1];
            String[] lstDate = lstParam[2].Split(';');
            hdnDateFrom.Value = lstDate[0];
            hdnDateTo.Value = lstDate[1];
            txtDateFrom.Text = lstDate[0];
            txtDateTo.Text = lstDate[1];

            hdnItemID.Value = lstParam[3];
            txtItemName.Text = string.Format("{0} ({1})", lstParam[4], hdnItemID.Value);

            hdnIsChargesIN.Value = lstParam[5];

            BindGridView(1, true, ref PageCount);
        }

        protected string GetFilterExpression()
        {
            string filterExpression = "";

            string startTime = "00:00:00";
            string endTime = "23:59:59";

            string startDateTime = string.Format("{0} {1}", Helper.GetDatePickerValue(hdnDateFrom.Value).ToString("yyyyMMdd"), startTime);
            string endDateTime = string.Format("{0} {1}", Helper.GetDatePickerValue(hdnDateTo.Value).ToString("yyyyMMdd"), endTime);

            if (hdnLocationID.Value != "" || hdnItemID.Value != "")
            {
                if (hdnIsChargesIN.Value != "1")
                    filterExpression += string.Format("LocationID = {0} AND QuantityOut > 0", hdnLocationID.Value);

                if (hdnDateFrom.Value != "" && hdnDateTo.Value != "")
                    filterExpression += String.Format(" AND CreatedDate BETWEEN CONVERT(DATETIME,'{0}',112) AND CONVERT(DATETIME,'{1}',112)", startDateTime, endDateTime);

                filterExpression += String.Format(" AND TransactionCode IN ({0},{1},{2},{3},{4},{5})",
                    Constant.TransactionCode.PURCHASE_RECEIVE, Constant.TransactionCode.DIRECT_PURCHASE, Constant.TransactionCode.PURCHASE_RETURN_REPLACEMENT, Constant.TransactionCode.CONSIGNMENT_RECEIVE, Constant.TransactionCode.CONSIGNMENT_RECEIVE_WITHOUT_PO, Constant.TransactionCode.DONATION_RECEIVE);
            }
            else
                filterExpression = "1 = 0";

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            filterExpression += string.Format(" AND ItemID = {0}", hdnItemID.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvItemMovementRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            List<vItemMovement> lstMovement = BusinessLayer.GetvItemMovementList(filterExpression, 10, pageIndex, "CreatedDate, MovementID");
            grdPopupView.DataSource = lstMovement;
            grdPopupView.DataBind();
        }

        protected void cbpPopupView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
    }
}