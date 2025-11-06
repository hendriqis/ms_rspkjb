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
    public partial class CustomerList : BasePageList
    {
        protected int PageCount = 1;
        protected int CurrPage = 1;
        private const string DEFAULT_GRDVIEW_FILTER = "BusinessPartnerID >= 1 AND GCBusinessPartnerType = '{0}' AND (HealthcareID = '{1}' OR HealthcareID IS NULL) AND IsDeleted = 0";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.CUSTOMER;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            List<StandardCode> lst = BusinessLayer.GetStandardCodeList(string.Format("IsActive = 1 AND IsDeleted = 0 AND ParentID = '{0}'", Constant.StandardCode.CUSTOMER_TYPE));
            lst.Insert(0, new StandardCode { StandardCodeName = string.Format("{0}", GetLabel(" ")) });
            Methods.SetComboBoxField<StandardCode>(cboCustomerType, lst, "StandardCodeName", "StandardCodeID");
            cboCustomerType.SelectedIndex = 0;

            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;
            filterExpression = GetFilterExpression();
            if (keyValue != "")
            {
                int row = BusinessLayer.GetBusinessPartnersRowIndex(filterExpression, keyValue, "BusinessPartnerCode ASC") + 1;
                CurrPage = Helper.GetPageCount(row, Constant.GridViewPageSize.GRID_MASTER);
            }
            else
                CurrPage = 1;

            BindGridView(CurrPage, true, ref PageCount);
        }

        public override void SetFilterParameter(ref string[] fieldListText, ref string[] fieldListValue)
        {
            fieldListText = new string[] { "Customer Name", "Short Name", "Customer Code" };
            fieldListValue = new string[] { "BusinessPartnerName", "ShortName", "BusinessPartnerCode" };
        }

        private string GetFilterExpression()
        {
            string filterExpression = hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += String.Format(DEFAULT_GRDVIEW_FILTER, Constant.BusinessObjectType.CUSTOMER, AppSession.UserLogin.HealthcareID);

            if (hdnCustomerType.Value != null && hdnCustomerType.Value != "")
            {
                filterExpression += string.Format(" AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE GCCustomerType = '{0}')", hdnCustomerType.Value);
            }
            return filterExpression;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = GetFilterExpression();

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetBusinessPartnersRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<BusinessPartners> lstEntity = BusinessLayer.GetBusinessPartnersList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "BusinessPartnerCode ASC");
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
            url = ResolveUrl("~/Program/Master/Customer/CustomerEntry.aspx");
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                if (hdnID.Value == "1")
                {
                    errMessage = "Maaf, Tidak dapat edit penjamin bayar ini";
                    return false;
                }
                else
                {
                    url = ResolveUrl(string.Format("~/Program/Master/Customer/CustomerEntry.aspx?id={0}", hdnID.Value));
                    return true;
                }
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value.ToString() != "")
            {
                if (hdnID.Value == "1")
                {
                    errMessage = "Maaf, Tidak dapat hapus penjamin bayar ini";
                    return false;
                }
                else
                {
                    List<Registration> reg = BusinessLayer.GetRegistrationList(string.Format("BusinessPartnerID = {0} AND GCRegistrationStatus != '{1}'",
                                        hdnID.Value, Constant.VisitStatus.CANCELLED));
                    List<ARInvoiceHd> arinvoice = BusinessLayer.GetARInvoiceHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                        hdnID.Value, Constant.TransactionStatus.VOID));
                    List<ARReceivingHd> arreceive = BusinessLayer.GetARReceivingHdList(string.Format("BusinessPartnerID = {0} AND GCTransactionStatus != '{1}'",
                                        hdnID.Value, Constant.TransactionStatus.VOID));

                    if (reg.Count == 0 && arinvoice.Count == 0 && arreceive.Count == 0)
                    {
                        BusinessPartners entity = BusinessLayer.GetBusinessPartners(Convert.ToInt32(hdnID.Value));
                        entity.IsDeleted = true;
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        BusinessLayer.UpdateBusinessPartners(entity);
                        return true;
                    }
                    else
                    {
                        errMessage = "Tidak dapat hapus penjamin bayar ini karena sudah digunakan dalam pendaftaran / proses piutang instansi";
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }
        }
    }
}