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
    public partial class SuratKeteranganIstirahat1Ctl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
            hdnHealthcareInitial.Value = healthcare.Initial;
            hdnVisitID.Value = param;

            Helper.SetControlEntrySetting(txtValueDateFrom, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueDateTo, new ControlEntrySetting(false, false, true, DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT)), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(false, false, false), "mpTrxPopup");

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

            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", hdnVisitID.Value, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            if (entityPD != null)
            {
                txtDiagnoseText.Text = entityPD.DiagnoseName + "|" + entityPD.DiagnosisText;
            }
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}