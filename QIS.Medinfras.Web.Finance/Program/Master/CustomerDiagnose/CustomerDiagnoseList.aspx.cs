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

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class CustomerDiagnoseList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMERS_DIAGNOSE;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            BindGridView(1, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Customer Diagnose Code", "Customer Diagnose Name", "Instansi" };
            fieldListValue = new string[] { "CustomerDiagnoseCode", "CustomerDiagnoseName", "BusinessPartnerID" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = String.Format("IsDeleted = 0");
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {   
                int rowCount = BusinessLayer.GetvCustomerDiagnoseHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vCustomerDiagnoseHd> lstEntity = BusinessLayer.GetvCustomerDiagnoseHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, " CustomerDiagnoseID ASC");
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
            url = ResolveUrl("~/Program/Master/CustomerDiagnose/CustomerDiagnoseEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                url = ResolveUrl(string.Format("~/Program/Master/CustomerDiagnose/CustomerDiagnoseEntry.aspx?id={0}", hdnID.Value));
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            bool result = false;
            if (hdnID.Value.ToString() != "")
            {
                CustomerDiagnoseHd entity = BusinessLayer.GetCustomerDiagnoseHd(Convert.ToInt32(hdnID.Value));
                List<CustomerDiagnoseDt> lstEntityDt = BusinessLayer.GetCustomerDiagnoseDtList(String.Format("CustomerDiagnoseID = {0} AND IsDeleted = 0", Convert.ToInt32(hdnID.Value)));

                if (lstEntityDt.Count <= 0)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateCustomerDiagnoseHd(entity);
                    result = true;
                }
                else
                {
                    errMessage = "Tidak dapat dihapus karena memiliki Diagnosa";
                    result = false;
                }
            }
            return result;
        }
    }
}