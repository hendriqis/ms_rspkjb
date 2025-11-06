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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BPJSReferralInformation : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            BindGridView();
        }

        private void BindGridView()
        {
            List<Faskes> lstEntityFaskes;
            if (hdnReferralList.Value != "")
            {
                string result = hdnReferralList.Value;
                BPJSFaskesListAPI respInfo = JsonConvert.DeserializeObject<BPJSFaskesListAPI>(result);
                lstEntityFaskes = respInfo.response.list;
            }
            else lstEntityFaskes = new List<Faskes>();
            grdView.DataSource = lstEntityFaskes;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

    }
}