using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class APInformationPerInvoiceSupplier : BasePageTrx
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.AP_INFORMATION_PER_INVOICE_SUPPLIER;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            MenuMaster oMenu = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault();
            hdnPageTitle.Value = oMenu.MenuCaption;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            txtPurchaseInvoiceNo.Focus();

            string filterSetvar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                    AppSession.UserLogin.HealthcareID,
                                                    Constant.SettingParameter.FN_AR_LEAD_TIME,
                                                    Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM);
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(filterSetvar);

            hdnSetvarLeadTime.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_LEAD_TIME).FirstOrDefault().ParameterValue;
            hdnSetvarHitungJatuhTempoDari.Value = lstSetvarDt.Where(a => a.ParameterCode == Constant.SettingParameter.FN_AR_DUE_DATE_COUNT_FROM).FirstOrDefault().ParameterValue;

        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpView1_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');

                BindGridView1();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridView()
        {
            String filterExpression = string.Format("PurchaseInvoiceNo = '{0}' AND IsDeleted = 0", txtPurchaseInvoiceNo.Text);

            List<vPurchaseInvoiceDt> lstInvoiceDt = BusinessLayer.GetvPurchaseInvoiceDtList(filterExpression);
            lvwView.DataSource = lstInvoiceDt;
            lvwView.DataBind();
        }

        private void BindGridView1()
        {
            String filterExpression = string.Format("PurchaseInvoiceID = {0}", hdnPurchaseInvoiceID.Value);

            List<vGLTransactionHdCustom> lstInvoiceDt = BusinessLayer.GetvGLTransactionHdCustomList(filterExpression);
            lvwView1.DataSource = lstInvoiceDt;
            lvwView1.DataBind();
        }
    }
}