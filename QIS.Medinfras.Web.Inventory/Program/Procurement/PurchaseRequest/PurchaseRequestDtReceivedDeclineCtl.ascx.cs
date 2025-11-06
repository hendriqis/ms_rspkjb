using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;

namespace QIS.Medinfras.Web.Inventory.Program
{
    public partial class PurchaseRequestDtReceivedDeclineCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                hdnPRID.Value = param;
                BindGridView(1, true, ref PageCount);
            }
        }

        #region Purchase Request Dt
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("PurchaseRequestID = {0} AND IsDeleted = 0 AND GCItemDetailStatus = '{1}'", hdnPRID.Value, Constant.TransactionStatus.VOID);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPurchaseRequestDtRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MATRIX);
            }

            PurchaseRequestHd prhd = BusinessLayer.GetPurchaseRequestHd(Convert.ToInt32(hdnPRID.Value));
            txtPurchaseRequestNo.Text = prhd.PurchaseRequestNo;

            List<vPurchaseRequestDt> lstEntity = BusinessLayer.GetvPurchaseRequestDtList(filterExpression, Constant.GridViewPageSize.GRID_MATRIX, pageIndex, "ItemName1 ASC");
            grdViewPRDT.DataSource = lstEntity;
            grdViewPRDT.DataBind();
        }

        protected void cbpViewPODT_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion
    }
}