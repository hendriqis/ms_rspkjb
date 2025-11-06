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
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class FollowupBPJSReferralCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            txtNoPeserta.Text = paramInfo[0];
            hdnAsalRujukan.Value = paramInfo[1];
            txtNoRujukan.Text = paramInfo[2];
            GetBPJSReferralFollowupList(paramInfo[0], paramInfo[2]);
        }

        private string GetBPJSReferralFollowupList(string noPeserta, string noRujukan)
        {
            string result = string.Empty;

            string filterExpression = string.Format("NoPeserta = '{0}' AND NoRujukan = '{1}' ORDER BY AppointmentID DESC", noPeserta, noRujukan);
            List<vRegistrationBPJSInfo> list = BusinessLayer.GetvRegistrationBPJSInfoList(filterExpression);
            if (list.Count>0)
            {
                grdView.DataSource = list;
                grdView.DataBind();
            }
            return result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            result = GetBPJSReferralFollowupList(txtNoPeserta.Text, hdnAsalRujukan.Value);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }
    }
}