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
    public partial class SuratPerintahRawatInapCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN));

            hdnReportCode.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN).FirstOrDefault().ParameterValue;

            Helper.SetControlEntrySetting(txtDiagnoseText, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtPhysicianName, new ControlEntrySetting(true, true, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitCode, new ControlEntrySetting(false, false, false), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtServiceUnitName, new ControlEntrySetting(true, true, true), "mpTrxPopup");


            vPatientDiagnosis entityPD = BusinessLayer.GetvPatientDiagnosisList(string.Format("RegistrationID = {0} AND GCDiagnoseType = '{1}'", hdnVisitID.Value, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
            if (entityPD != null)
            {
                txtDiagnoseText.Text = entityPD.DiagnoseName;
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