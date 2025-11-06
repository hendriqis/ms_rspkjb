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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class TestPartnerList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            String requestID = Request.QueryString["id"];
            switch (requestID)
            {
                case "LB": return Constant.MenuCode.Laboratory.TEST_PARTNER;
                case "IS": return Constant.MenuCode.Imaging.TEST_PARTNER;
                case "FN": return Constant.MenuCode.Finance.TEST_PARTNER;
                default: return Constant.MenuCode.Finance.TEST_PARTNER;
            }
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnRequestID.Value = Request.QueryString["id"];

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;

            filterExpression = GetFilterExpression();

            if (keyValue != "")
            {
                int row = BusinessLayer.GetvTestPartnerRowIndex(filterExpression, keyValue, "BusinessPartnerCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
            {
                CurrPage = 1;
            }

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Business Partner Name", "Business Partner Code" };
            fieldListValue = new string[] { "BusinessPartnerName", "BusinessPartnerCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("BusinessPartnerID > 1 AND HealthcareID = '{0}' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID);

            if (hdnRequestID.Value == "LB")
            {
                filterExpression += string.Format(" AND GCTestPartnerType = '{0}'", Constant.TestPartnerType.LABORATORY);
            }
            else if (hdnRequestID.Value == "IS")
            {
                filterExpression += string.Format(" AND GCTestPartnerType = '{0}'", Constant.TestPartnerType.IMAGING);
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestPartnerRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestPartner> lstEntity = BusinessLayer.GetvTestPartnerList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BusinessPartnerCode ASC");
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

        protected override bool OnAddRecord(ref string url, ref string errMessage)
        {
            String requestID = Request.QueryString["id"];
            url = ResolveUrl(string.Format("~/Libs/Program/Master/TestPartner/TestPartnerEntry.aspx?id={0}", requestID));
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                String requestID = Request.QueryString["id"];
                url = ResolveUrl(string.Format("~/Libs/Program/Master/TestPartner/TestPartnerEntry.aspx?id={0}|{1}", requestID, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateBusinessPartners(entity);
                return true;
            }
            return false;
        }
    }
}