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
    public partial class NursingDateCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            hdnHealthcareInitial.Value = healthcare.Initial;

            hdnRegistrationID.Value = param;

            Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDateTo, new ControlEntrySetting(true, true, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiagnosa, new ControlEntrySetting(true, true, false), "mpTrxPopup");

            if (hdnRegistrationID.Value != null && hdnRegistrationID.Value != "" && hdnRegistrationID.Value != "0")
            {
                Registration reg = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                txtValueDateFrom.Text = reg.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                hdnTanggalDariString.Value = txtValueDateFrom.Text;
            }
            hdnDiagnosa.Value = txtDiagnosa.Text;
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}