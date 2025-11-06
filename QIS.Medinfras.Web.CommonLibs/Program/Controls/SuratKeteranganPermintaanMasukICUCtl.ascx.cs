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
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SuratKeteranganPermintaanMasukICUCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnRegistrationID.Value = paramInfo[0];

            hdnReportCode.Value = "PM-00752";

            Helper.SetControlEntrySetting(txtMorningTime1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtAfternoonTime1, new ControlEntrySetting(false, false, true), "mpTrxPopup");

        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}