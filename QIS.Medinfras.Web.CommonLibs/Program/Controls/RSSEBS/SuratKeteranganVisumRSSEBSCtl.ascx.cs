using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
    public partial class SuratKeteranganVisumRSSEBSCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnVisitID.Value = paramInfo[0];

            ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("VisitID = {0}", hdnVisitID.Value.ToString())).FirstOrDefault();
            if (entity != null)
            {
                hdnParamedicID.Value = entity.ParamedicID.ToString();
            }

            GenerateDefaultMedicalResumeData();

            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN));

            hdnReportCode.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN).FirstOrDefault().ParameterValue;
            Helper.SetControlEntrySetting(txtValueDate, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueTime1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtValueTime2, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtNumber1, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtNumber2, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtNumber3, new ControlEntrySetting(false, false, true), "mpTrxPopup");
            Helper.SetControlEntrySetting(txtRemarks, new ControlEntrySetting(false, false, false), "mpTrxPopup");

            txtValueDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            string YYYY = txtValueDate.Text.Substring(6, 4);
            string MM = txtValueDate.Text.Substring(3, 2);
            string DD = txtValueDate.Text.Substring(0, 2);
            string dateALL = YYYY + '-' + MM + '-' + DD;
            hdnTanggal.Value = dateALL;
            hdnTanggal.Value = txtValueDate.Text;

        }

        private void GenerateDefaultMedicalResumeData()
        {
            StringBuilder sbNotes;
            sbNotes = new StringBuilder();
            #region Objective Content

            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0}) AND IsDeleted = 0 AND GCParamedicMasterType = 'X019^001' ORDER BY GCRoSystem", hdnVisitID.Value));
            if (lstROS.Count > 0)
            {
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }

            txtObjectiveResumeText.Text = sbNotes.ToString();
            hdnObjectiveText.Value = sbNotes.ToString();
            #endregion
        }

        protected void cbpMedicalSickLeave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "success";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}