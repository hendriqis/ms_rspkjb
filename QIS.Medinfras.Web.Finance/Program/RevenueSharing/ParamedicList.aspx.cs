using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.Finance.Program
{
    public partial class ParamedicList : BasePageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Finance.PARAMEDIC_LIST;
        }

        protected override void InitializeDataControl(string filterExpression, string keyValue)
        {
            hdnFilterExpression.Value = filterExpression;
            hdnID.Value = keyValue;

            txtPeriodFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtPeriodTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView();
        }

        private void BindGridView()
        {
            int paramedicID = 0;
            if (hdnParamedicID.Value != "" && hdnParamedicID.Value != null)
            {
                paramedicID = Convert.ToInt32(hdnParamedicID.Value);
            }

            string paramDate = string.Format("{0}|{1}", Helper.GetDatePickerValue(txtPeriodFrom.Text).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtPeriodTo.Text).ToString(Constant.FormatString.DATE_FORMAT_112));

            int oIsExcludeParamDate = chkIsExludeParamDate.Checked ? 1 : 0;

            int revenueSharingID = 0;
            if (hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != null)
            {
                revenueSharingID = Convert.ToInt32(hdnRevenueSharingID.Value);
            }

            int oIsExcludeChargesFilter = chkIsExcludeChargesFilter.Checked ? 1 : 0;

            List<GetParamedicMasterRevenueSharing> lstEntity = BusinessLayer.GetParamedicMasterRevenueSharing(paramedicID, paramDate, oIsExcludeParamDate, revenueSharingID, oIsExcludeChargesFilter);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                BindGridView();
                result = "refresh|";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void btnOpenTransactionDt_Click(object sender, EventArgs e)
        {
            if (hdnID.Value.ToString() != "")
            {
                ParamedicMasterRevenueSharingProcess pmrs = new ParamedicMasterRevenueSharingProcess();
                pmrs.ParamedicID = Convert.ToInt32(hdnID.Value);
                pmrs.PeriodeStart = Helper.GetDatePickerValue(txtPeriodFrom.Text);
                pmrs.PeriodeEnd = Helper.GetDatePickerValue(txtPeriodTo.Text);
                pmrs.RevenueSharingID = hdnRevenueSharingID.Value != "" && hdnRevenueSharingID.Value != "0" ? Convert.ToInt32(hdnRevenueSharingID.Value) : 0;
                AppSession.ParamedicMasterRevenueSharingProcess = pmrs;

                AppSession.ParamedicID = Convert.ToInt32(hdnID.Value);

                List<GetUserMenuAccess> lstMenu = BusinessLayer.GetUserMenuAccess(Constant.Module.FINANCE, AppSession.UserLogin.HealthcareID, AppSession.UserLogin.UserID, string.Format("ParentCode = '{0}'", OnGetMenuCode())).OrderBy(p => p.MenuIndex).ToList();
                GetUserMenuAccess menu = lstMenu.OrderBy(p => p.MenuIndex).FirstOrDefault();
                Response.Redirect(Page.ResolveUrl(menu.MenuUrl));
            }
        }
    }
}