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
using System.Text;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using QIS.Medinfras.Web.CommonLibs.MasterPage;


namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class InformationStatusKontrakRekanan : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnersID > 1 AND BusinessPartnersGCType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMER_CONTRACT_STATUS;
        }

        //public String GetContractDate()
        //{

        //    return Request.Form[hdnContractDate.UniqueID];
        //}

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("IsActive = 1 AND IsDeleted = 0 AND ParentID = '{0}'", Constant.StandardCode.CUSTOMER_TYPE));
            lst.Insert(0, new StandardCode { StandardCodeName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<StandardCode>(cboCustomerType, lst, "StandardCodeName", "StandardCodeID");
            cboCustomerType.SelectedIndex = 0;

            txtStartDate.Text = DateTime.Now.AddDays(-7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtEndDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetBusinessPartnersRowIndex(filterExpression, keyValue) + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);        
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Customer Name", "Customer Code", "Short Name" };
            fieldListValue = new string[] { "BusinessPartnersName", "BusinessPartnersCode", "BusinessPartnersShortName" };
        }

        private string GetFilterExpression()
        {
            DateTime StartDate = Helper.GetDatePickerValue(txtStartDate.Text);
            DateTime EndDate = Helper.GetDatePickerValue(txtEndDate.Text);

            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format(DEFAULT_GRDVIEW_FILTER, Constant.BusinessObjectType.CUSTOMER, AppSession.UserLogin.HealthcareID);

            if (hdnCustomerType.Value != null && hdnCustomerType.Value != "")
            {
                filterExpression += string.Format(" AND BusinessPartnersID IN (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", hdnCustomerType.Value);
            }

            if (chkIsExpired.Checked)
            {
                filterExpression += string.Format(" AND BusinessPartnersStartDate >= '{0}' AND BusinessPartnersEndDate < '{1}'", StartDate.ToString(Constant.FormatString.DATE_FORMAT), EndDate.ToString(Constant.FormatString.DATE_FORMAT));
            }
            else
            {
                filterExpression += string.Format(" AND BusinessPartnersEndDate >= '{0}'", EndDate.ToString(Constant.FormatString.DATE_FORMAT));
            }

            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvBusinessPartnersInformationContractRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }
            List<vBusinessPartnersInformationContract> lstEntity = BusinessLayer.GetvBusinessPartnersInformationContractList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, " BusinessPartnersCode ASC");
            lvwView.DataSource = lstEntity;
            lvwView.DataBind();
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