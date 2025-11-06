using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SuratKeteranganSehatRSMDCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                if (oHc.Initial == "RSMD")
                {
                    hdnRptCodeReport.Value = "PM-00702";
                }
                else
                {
                    hdnRptCodeReport.Value = "PM-00121";
                }
            }
            else
            {
                hdnRptCodeReport.Value = "PM-00121";
            }
            Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(true, true, true), "mpTrxPopup");
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}