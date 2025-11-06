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
    public partial class SuratKeteranganHamil1Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;
            Helper.SetControlEntrySetting(txtHamil, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDate, new ControlEntrySetting(false, false, true), "mpTrxPopup");

            txtValueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYY = txtValueDate.Text.Substring(6, 4);
            string MM = txtValueDate.Text.Substring(3, 2);
            string DD = txtValueDate.Text.Substring(0, 2);
            string dateALL = YYYY + '-' + MM + '-' + DD;
            hdnTanggal.Value = dateALL;
            hdnTanggal.Value = txtValueDate.Text;

        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}