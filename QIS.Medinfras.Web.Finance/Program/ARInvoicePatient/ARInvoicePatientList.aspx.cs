using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePatientList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AR_INVOICE_PATIENT;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetvPatient4RowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "MedicalNo", "PatientName", "StreetName" };
            fieldListValue = new string[] { "MedicalNo", "PatientName", "StreetName" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            
            if (chkIsEmployeeOnly.Checked)
            {
                if (txtEmployeeName.Text != "")
                {
                    filterExpression += string.Format("MRN IN (SELECT MRN FROM Registration WHERE EmployeeID IN (SELECT EmployeeID FROM Employee WHERE FullName like '%{0}%'))", txtEmployeeName.Text);
                }
                else
                {
                    filterExpression += "EmployeeID IS NOT NULL";
                }
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatient4RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatient4> lstEntity = BusinessLayer.GetvPatient4List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientName ASC");
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

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value.ToString() != "")
            {
                Patient entity = BusinessLayer.GetPatient(Convert.ToInt32(hdnID.Value));
                PatientDetail pt = new PatientDetail();
                pt.MRN = entity.MRN;
                pt.MedicalNo = entity.MedicalNo;
                pt.PatientName = entity.FullName;
                AppSession.PatientDetail = pt;
                
                string filterCust = string.Format("GCCustomerType = '{0}' AND IsDeleted = 0", Constant.CustomerType.PERSONAL);
                vCustomer entityCust = BusinessLayer.GetvCustomerList(filterCust).FirstOrDefault();
                AppSession.BusinessPartnerID = entityCust.BusinessPartnerID;

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", OnGetMenuCode())).OrderBy(p => p.MenuIndex).ToList();
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
            }
        }
    }
}