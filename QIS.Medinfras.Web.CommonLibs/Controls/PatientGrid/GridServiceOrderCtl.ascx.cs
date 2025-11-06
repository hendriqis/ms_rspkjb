using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class GridServiceOrderCtl : System.Web.UI.UserControl
    {
        protected int PageCount = 1;
        public void InitializeControl()
        {
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpViewOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            ((BasePagePatientOrder)Page).LoadAllWords();
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

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ((BasePagePatientOrder)Page).GetFilterExpressionTestOrder();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvServiceOrderHdVisitRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_PATIENT_LIST);
            }

            List<vServiceOrderHdVisit> lstEntity = BusinessLayer.GetvServiceOrderHdVisitList(filterExpression, Constant.GridViewPageSize.GRID_PATIENT_LIST, pageIndex, "ServiceOrderID ASC");
            lvwViewOrder.DataSource = lstEntity;
            lvwViewOrder.DataBind();
            
        }

        protected void lvwViewOrder_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType == ListViewItemType.DataItem)
            {
                //vServiceOrderHdVisit entity = (vServiceOrderHdVisit)e.Item.DataItem;
                //HtmlGenericControl spnProcessed = e.Item.FindControl("spnProcessed") as HtmlGenericControl;

                //if (entity.GCTransactionStatus != Constant.TransactionStatus.PROCESSED)
                //    spnProcessed.Style.Add("display", "none");
            }
        }

        protected string GetLabel(string code)
        {
            return ((BasePagePatientOrder)Page).GetLabel(code);
        }

        protected void btnOpenTransactionDtOrder_Click(object sender, EventArgs e)
        {
            if (hdnTransactionOrderNo.Value != "")
            {
                ((BasePagePatientOrder)Page).OnGrdRowClickTestOrder(hdnTransactionOrderNo.Value, hdnServiceOrderNo.Value);
            }
        }
    }
}