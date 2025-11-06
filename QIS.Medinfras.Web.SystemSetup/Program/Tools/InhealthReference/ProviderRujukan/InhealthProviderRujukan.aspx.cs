using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.SystemSetup.Tools
{
    public partial class InhealthProviderRujukan : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.INHEALTH_REFERENCE_PROVIDER_RUJUKAN;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnInhealthToken.Value = AppSession.Inheatlh_Access_Token;
            hdnInhealthKodeProvider.Value = AppSession.Inhealth_Provider_Code;
            hdnIsBridgingToInhealth.Value = AppSession.IsBridgingToInhealth ? "1" : "0";

            hdnID.Value = keyValue;

            filterExpression = GetFilterExpression();
            hdnFilterExpression.Value = filterExpression;

            if (keyValue != "")
            {
                int row = BusinessLayer.GetvInhealthReferenceProviderRujukanListRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_ITEM);
            }
            else
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Nama Provider", "Kode Provider", "Lokasi Provider" };
            fieldListValue = new string[] { "ObjectName", "ObjectCode", "Others" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvInhealthReferenceProviderRujukanListRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_ITEM);
            }

            List<vInhealthReferenceProviderRujukan> lstEntity = BusinessLayer.GetvInhealthReferenceProviderRujukanList(filterExpression, Constant.GridViewPageSize.GRID_ITEM, pageIndex);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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