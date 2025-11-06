using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Web.UI.HtmlControls;
using QIS.Data.Core.Dal;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ARInvoicePayerProcessEntryDtCtl : BaseEntryPopupCtl
    {
        protected int PageCount = 1;
        private string[] lstSelectedMember = null;
        private ARInvoicePayerProcessEntry2 DetailPage
        {
            get { return (ARInvoicePayerProcessEntry2)Page; }
        }

        public override void InitializeDataControl(string param)
        {
            hdnARInvoiceID.Value = param;

            txtPeriodFrom.Text = DateTime.Now.AddMonths(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<Department> lstDepartment = BusinessLayer.GetDepartmentList("IsActive = 1");
            lstDepartment.Insert(0, new Department { DepartmentID = "", DepartmentName = "" });
            Methods.SetComboBoxField<Department>(cboDepartment, lstDepartment, "DepartmentName", "DepartmentID");
            cboDepartment.SelectedIndex = 0;

            BindGridView(1, true, ref PageCount);
        }

        #region Bind Grid
        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("BusinessPartnerID IN (SELECT BusinessPartnerID FROM vCustomer WHERE BusinessPartnerID = {0} OR CustomerBillTo = {0}) AND GCTransactionStatus != '{1}' AND GCPaymentType = '{2}' AND RegistrationID NOT IN (SELECT RegistrationID FROM vARInvoiceDt WHERE GCTransactionStatus != '{3}') AND PaymentDate BETWEEN '{4}' AND '{5}'", AppSession.BusinessPartnerID, Constant.TransactionStatus.VOID, Constant.PaymentType.AR_PAYER, Constant.TransactionStatus.VOID, Helper.GetDatePickerValue(txtPeriodFrom).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo).ToString(Constant.FormatString.DATE_FORMAT_112));
            if (cboDepartment.Value != null && cboDepartment.Value.ToString() != "")
                filterExpression += string.Format(" AND DepartmentID = '{0}'", cboDepartment.Value);
            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientPaymentHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, 10);
            }

            lstSelectedMember = hdnSelectedMember.Value.Split(',');
            List<vPatientPaymentHd> lstPaymentHd = BusinessLayer.GetvPatientPaymentHdList(filterExpression, 10, pageIndex);
            grdView.DataSource = lstPaymentHd;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientPaymentHd entity = e.Row.DataItem as vPatientPaymentHd;
                CheckBox chkIsSelected = e.Row.FindControl("chkIsSelected") as CheckBox;
                if (lstSelectedMember.Contains(entity.PaymentID.ToString()))
                    chkIsSelected.Checked = true;
            }
        }

        protected void cbpProcessDetail_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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
        #endregion

        #region Save Entity
        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ARInvoiceDtDao entityDtDao = new ARInvoiceDtDao(ctx);
            int ARInvoiceID = Convert.ToInt32(hdnARInvoiceID.Value);
            try
            {
                DetailPage.SaveARInvoiceHd(ctx, ref ARInvoiceID);
                List<PatientPaymentHd> lstPaymentHd = BusinessLayer.GetPatientPaymentHdList(string.Format("PaymentID IN ({0})", hdnSelectedMember.Value.Substring(1)), ctx);
                foreach (PatientPaymentHd paymentHd in lstPaymentHd)
                {
                    ARInvoiceDt entityDt = new ARInvoiceDt();
                    entityDt.ARInvoiceID = ARInvoiceID;
                    entityDt.RegistrationID = paymentHd.RegistrationID;
                    entityDt.PaymentID = paymentHd.PaymentID;
                    entityDt.TransactionAmount = entityDt.ClaimedAmount = paymentHd.TotalPaymentAmount;
                    entityDt.CreatedBy = AppSession.UserLogin.UserID;
                    entityDtDao.Insert(entityDt);
                }
                retval = ARInvoiceID.ToString();
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}