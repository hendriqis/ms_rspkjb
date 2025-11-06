using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.CommonLibs.MasterPage;
using DevExpress.Utils;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.SystemSetup.Program
{
    public partial class CancelPatientRegistrationList : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.SystemSetup.CHANGE_REGISTRATION_STATUS;
        }

        protected string GetPageTitle()
        {
            return BusinessLayer.GetMenuMasterList(string.Format("MenuCode='{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
        }

        protected override void InitializeDataControl()
        {
            txtRegistrationDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            Helper.SetControlEntrySetting(txtRegistrationDate, new ControlEntrySetting(true, true, true), "mpBedStatus");

            BindGridView();

        }

        public string OnGetFilterExpression()
        {
            return Request.Form[hdnFilterExpression.UniqueID];
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("GCRegistrationStatus IN ('{0}','{1}','{2}') AND RegistrationDate = '{3}'",
                                       Constant.VisitStatus.OPEN, Constant.VisitStatus.CHECKED_IN, Constant.VisitStatus.RECEIVING_TREATMENT,
                                       Helper.GetDatePickerValue(txtRegistrationDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            hdnFilterExpression.Value = filterExpression;

            if (hdnFilterExpressionQuickSearch.Value != null && hdnFilterExpressionQuickSearch.Value != "")
            {
                filterExpression += String.Format("AND {0}", hdnFilterExpressionQuickSearch.Value);
            }

            List<vCancelRegistration> lstEntity = BusinessLayer.GetvCancelRegistrationList(filterExpression);
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void cbpCancelPatientDischarge_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {

        }
    }
}