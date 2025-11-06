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
using System.Web.UI.HtmlControls;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.Dashboard.Program
{
    public partial class RumahSakit2 : BasePageTrx
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.Dashboard.RumahSakit2;
        }

        protected override void InitializeDataControl()
        {
            lblDateTime.InnerText = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
            lblHeader.InnerText = string.Format("Hello, {0}", AppSession.UserLogin.UserFullName);

            ((SurveyInformationCtl)ctlsurveyInformation).InitializeControl("");
            ((MasterRS2Ctl)ctlMasterRS2).InitializeControl("");
            ((BPJSInformationCtl)ctlBPJSInformation).InitializeControl("");
            ((PelayananFarmasiCtl)ctlPelayananFarmasi).InitializeControl("");
            ((DurasiTungguRJCtl)CtlDurasiTungguRJ).InitializeControl("");
            ((AntrianOnlineCtl)CtlAntrianOnline).InitializeControl("");
        }

        protected void cbpViewTime_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            if (e.Parameter == "refreshHour")
            {
                hdnTimeNow.Value = DateTime.Now.ToString(Constant.FormatString.DATE_TIME_FORMAT);
                result = string.Format("refreshHour");
            }
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}