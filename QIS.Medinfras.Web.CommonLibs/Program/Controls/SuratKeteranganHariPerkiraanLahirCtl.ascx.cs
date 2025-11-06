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
    public partial class SuratKeteranganHariPerkiraanLahirCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            hdnHealthcareInitial.Value = healthcare.Initial;

            hdnRegistrationID.Value = param;

            Helper.SetControlEntrySetting(txtDate, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            txtDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYY = txtDate.Text.Substring(6, 4);
            string MM = txtDate.Text.Substring(3, 2);
            string DD = txtDate.Text.Substring(0, 2);
            string dateALL = YYYY + '-' + MM + '-' + DD;
            hdnTanggalDari.Value = dateALL;
            hdnTanggalDari.Value = txtDate.Text;

        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}