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

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class InfoOutstandingPiutangPribadiCtl : BaseViewPopupCtl
    {
        protected int PageCount = 1;

        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID > 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";

        public override void InitializeDataControl(string param)  
        {
            if (param != "")
            {
                hdnParam.Value = param;
                BindGridView(1, true, ref PageCount);
            }            
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            String filterExpression = "";
            filterExpression += string.Format("MRN = {0}", hdnParam.Value);
            if (hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);
            }


            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInfoOutstandingPiutangPribadiRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vInfoOutstandingPiutangPribadi> lstEntity = BusinessLayer.GetvInfoOutstandingPiutangPribadiList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, " RegistrationNo ASC, PaymentNo ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
        }

        //private string GetFilterExpression()
        //{
        //    String filterExpression = hdnFilterExpressionQuickSearch.Value;
        //    filterExpression += string.Format("MRN = {0}", hdnParam.Value);
        //    if (hdnFilterExpressionQuickSearch.Value != "")
        //        filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

        //    return filterExpression;
        //}

        protected void cbpInfoRegistrationView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
                    result = "refresh";
                }
            }
            result += "|" + pageCount;
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}