using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class PiutangInformation : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.INFORMASI_PIUTANG;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowNextPrev = IsAllowSave = IsAllowVoid = false;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

            List<Variable> lstPeriodeType = new List<Variable>();
            lstPeriodeType.Add(new Variable { Code = "0", Value = "Tanggal Pengakuan Piutang" });
            lstPeriodeType.Add(new Variable { Code = "1", Value = "Tanggal Invoice" });
            Methods.SetComboBoxField<Variable>(cboPeriodeType, lstPeriodeType, "Value", "Code");
            cboPeriodeType.SelectedIndex = 0;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BusinessPartners bp = BusinessLayer.GetBusinessPartners(1);
            hdnPayerID.Value = bp.BusinessPartnerID.ToString();
            txtPayerCode.Text = bp.BusinessPartnerCode;
            txtPayerName.Text = bp.BusinessPartnerName;

            List<Variable> lstStatusInvoice = new List<Variable>();
            lstStatusInvoice.Add(new Variable { Code = "0", Value = "" });
            lstStatusInvoice.Add(new Variable { Code = "1", Value = "Ada Invoice" });
            lstStatusInvoice.Add(new Variable { Code = "2", Value = "Tidak ada Invoice" });
            Methods.SetComboBoxField<Variable>(cboStatusInvoice, lstStatusInvoice, "Value", "Code");
            cboStatusInvoice.Value = "0";
            
            BindGridView(1, true, ref PageCount);
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref PageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridView(1, true, ref PageCount);
                    result = "refresh|" + PageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {

            DateTime FromDate = Helper.GetDatePickerValue(txtPeriodFrom.Text);
            DateTime ToDate = Helper.GetDatePickerValue(txtPeriodTo.Text);
          
            
            string filterExpression = string.Format("GCPaymentType IN ('{0}','{1}')", Constant.PaymentType.AR_PATIENT, Constant.PaymentType.AR_PAYER);

            if (cboPeriodeType.Value.ToString() == "0")
            {
                filterExpression += string.Format(" AND PaymentDate BETWEEN '{0}' AND '{1}'", FromDate.ToString(Constant.FormatString.DATE_FORMAT_112), ToDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }
            else
            {
                filterExpression += string.Format(" AND ARInvoiceDate BETWEEN '{0}' AND '{1}'", FromDate.ToString(Constant.FormatString.DATE_FORMAT_112), ToDate.ToString(Constant.FormatString.DATE_FORMAT_112));
            }

            if (hdnPayerID.Value != null && hdnPayerID.Value != "" && hdnPayerID.Value != "0")
                filterExpression += string.Format(" AND BusinessPartnerID = {0}", hdnPayerID.Value);

            if (hdnFilterExpressionQuickSearch.Value != "")
                filterExpression += string.Format(" AND {0}", hdnFilterExpressionQuickSearch.Value);

            if (cboStatusInvoice.Value.ToString() == "1")
            {
                filterExpression += " AND ARInvoiceID IS NOT NULL";
            }
            else if (cboStatusInvoice.Value.ToString() == "2")
            {
                filterExpression += " AND ARInvoiceID IS NULL";
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvARControlPerPeriodeRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vARControlPerPeriode> lstEntity = BusinessLayer.GetvARControlPerPeriodeList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PaymentID, ARInvoiceID");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }
    }
}