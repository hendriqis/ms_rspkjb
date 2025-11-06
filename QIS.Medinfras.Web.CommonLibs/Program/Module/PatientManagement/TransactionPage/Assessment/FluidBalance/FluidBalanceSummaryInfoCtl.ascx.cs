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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FluidBalanceSummaryInfoCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override void InitializeDataControl(string param)
        {
            //var orderDetailID|itemID|itemName|paramedicName;
            string[] paramInfo = param.Split('|');
            hdnFluidBalanceGroup.Value = paramInfo[0];
            hdnVisitID.Value = paramInfo[1];
            hdnLogDate.Value = paramInfo[2];
            hdnFluidTotal.Value = paramInfo[3];

            txtLogDate.Text = hdnLogDate.Value;

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND LogDate = '{1}' AND GCFluidGroup = '{2}'", hdnVisitID.Value, hdnLogDate.Value, hdnFluidBalanceGroup.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvFluidBalanceSummaryByFluidTypeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vFluidBalanceSummaryByFluidType> lstEntity = BusinessLayer.GetvFluidBalanceSummaryByFluidTypeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCFluidType");

            grdPopupView.DataSource = lstEntity;
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