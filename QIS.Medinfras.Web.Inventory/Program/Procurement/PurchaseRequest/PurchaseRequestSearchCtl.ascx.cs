using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequestSearchCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        public override void InitializeDataControl(string param)
        {
            string[] temp = param.Split('|');
            //hdnSelectedMember.Value = temp[0];
            hdnLocationParam.Value = temp[0];
            hdnProductLineIDCtl.Value = temp[1];
            hdnPurchaseOrderType.Value = temp[2];
            BindGridView(1, true, ref PageCount);

            //if (hdnSelectedMember.Value != "")
            //{
            //    List<vPurchaseRequestHd> lstSelected = BusinessLayer.GetvPurchaseRequestHdList(string.Format("GCTransactionStatus = '{0}' AND FromLocationID = {1}", Constant.TransactionStatus.APPROVED, hdnLocationParam.Value));
            //    grdView.DataSource = lstSelected;
            //    grdView.DataBind();
            //}
        }

        protected void cbpPopup_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private string GetFilterExpression()
        {
            string prDT = "";
            string filterDT = string.Format("GCItemDetailStatus = '{0}' AND OrderedQuantity < Quantity", Constant.TransactionStatus.APPROVED);
            List<PurchaseRequestDt> lstDt = BusinessLayer.GetPurchaseRequestDtList(filterDT);

            foreach (PurchaseRequestDt dt in lstDt)
            {
                prDT += dt.PurchaseRequestID + ",";
            }

            if (prDT != "")
            {
                prDT = prDT.Substring(0, prDT.Length - 1);
            }

            string filterExpression;
            if (hdnProductLineIDCtl.Value != "0")
            {
                filterExpression = string.Format(
                    "PurchaseRequestNo LIKE '%{0}%' AND FromLocationID LIKE '%{1}%' AND GCTransactionStatus = '{2}' AND ProductLineID = {3} AND PurchaseRequestID IN ({4})",
                    hdnFilterItemCode.Value, hdnLocationParam.Value, Constant.TransactionStatus.APPROVED, hdnProductLineIDCtl.Value, prDT);
            }
            else
            {
                filterExpression = string.Format(
                    "PurchaseRequestNo LIKE '%{0}%' AND FromLocationID LIKE '%{1}%' AND GCTransactionStatus = '{2}' AND ProductLineID IS NULL AND PurchaseRequestID IN ({3})",
                    hdnFilterItemCode.Value, hdnLocationParam.Value, Constant.TransactionStatus.APPROVED, prDT);
            }

            if (hdnPurchaseOrderType.Value != "null" && hdnPurchaseOrderType.Value != "")
            {
                filterExpression += string.Format(" AND GCPurchaseOrderType = '{0}'", hdnPurchaseOrderType.Value);
            }

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format("AND {0}", hdnFilterExpressionQuickSearch.Value);
            }
            return filterExpression;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPurchaseRequestHd entity = e.Row.DataItem as vPurchaseRequestHd;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.PurchaseRequestID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 5);
            }
            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPurchaseRequestHd> lstEntity = BusinessLayer.GetvPurchaseRequestHdList(filterExpression, 5, pageIndex, "PurchaseRequestNo ASC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}