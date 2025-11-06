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
    public partial class LaporanKematianRSSEBCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN));

            hdnReportCode.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN).FirstOrDefault().ParameterValue;

            Helper.SetControlEntrySetting(txtValueDate, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDeathTime1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDeathTime2, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpTrxPopup");

            txtValueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYY = txtValueDate.Text.Substring(6, 4);
            string MM = txtValueDate.Text.Substring(3, 2);
            string DD = txtValueDate.Text.Substring(0, 2);
            string dateALL = YYYY + '-' + MM + '-' + DD;
            hdnTanggal.Value = dateALL;
            hdnTanggal.Value = txtValueDate.Text;

            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("RegistrationID = {0} AND GCDiagnoseType = '{1}'", hdnVisitID.Value, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
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