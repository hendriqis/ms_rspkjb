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

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CustomerContractList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CUSTOMER_CONTRACT;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            if (Request.Form["customerID"] != null)
            {
                hdnCustomerID.Value = Request.Form["customerID"].ToString();
                BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnCustomerID.Value));
                txtCustomerCode.Text = entity.BusinessPartnerCode;
                txtCustomerName.Text = entity.BusinessPartnerName;
            }

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetCustomerContractRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Contract No" };
            fieldListValue = new string[] { "ContractNo" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = "1 = 0";
            if (hdnCustomerID.Value != "")
            {
                filterExpression = hdnFilterExpression.Value;
                if (filterExpression != "")
                    filterExpression += " AND ";
                filterExpression += string.Format("BusinessPartnerID = {0} AND IsDeleted = 0", hdnCustomerID.Value);
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetCustomerContractRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }   

            List<CustomerContract> lstEntity = BusinessLayer.GetCustomerContractList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
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
            if (hdnCustomerID.Value != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/CustomerContract/CustomerContractEntry.aspx?id={0}", hdnCustomerID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/CustomerContract/CustomerContractEntry.aspx?id={0}|{1}", hdnCustomerID.Value, hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                CustomerContract entity = BusinessLayer.GetCustomerContract(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateCustomerContract(entity);
                return true;
            }
            return false;
        }

        protected void cbpViewDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            List<vContractCoverage> lstDetail = BusinessLayer.GetvContractCoverageList(string.Format("ContractID = {0}", hdnExpandID.Value));
            grdDetail.DataSource = lstDetail;
            grdDetail.DataBind();
        }
    }
}