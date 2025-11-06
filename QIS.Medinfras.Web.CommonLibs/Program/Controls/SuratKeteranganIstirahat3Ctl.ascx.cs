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
    public partial class SuratKeteranganIstirahat3Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnRegistrationID.Value = param;

            Healthcare oHc = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            if (oHc != null)
            {
                if (oHc.Initial == "RSSA")
                {
                    hdnRptCodeReport.Value = "PM-00600";
                }
                else
                {
                    hdnRptCodeReport.Value = "PM-00593";
                }
            }
            else
            {
                hdnRptCodeReport.Value = "PM-00593";
            }
            Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDateTo, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpTrxPopup");

            txtValueDateFrom.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYYDari = txtValueDateFrom.Text.Substring(6, 4);
            string MMDari = txtValueDateFrom.Text.Substring(3, 2);
            string DDDari = txtValueDateFrom.Text.Substring(0, 2);
            string dateALLDari = YYYYDari + '-' + MMDari + '-' + DDDari;
            hdnTanggalDari.Value = dateALLDari;
            hdnTanggalDariString.Value = txtValueDateFrom.Text;

            txtValueDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYYSampai = txtValueDateTo.Text.Substring(6, 4);
            string MMSampai = txtValueDateTo.Text.Substring(3, 2);
            string DDSampai = txtValueDateTo.Text.Substring(0, 2);
            string dateALLSampai = YYYYSampai + '-' + MMSampai + '-' + DDSampai;
            hdnTanggalSampai.Value = dateALLSampai;
            hdnTanggalSampaiString.Value = txtValueDateTo.Text;

        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}