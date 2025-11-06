using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class ViewReferralResponseCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            string recordID = paramInfo[0];

            SetControlProperties();

            if (!string.IsNullOrEmpty(recordID))
            {
                PatientReferral obj = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} AND ID = {1}", AppSession.RegisteredPatient.VisitID, recordID)).FirstOrDefault();
                if (obj != null)
                {
                    hdnID.Value = obj.ID.ToString();
                    EntityToControl(obj);
                }
            }
        }

        private void SetControlProperties()
        {
            #region Physician Combobox
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician2, lstParamedic, "ParamedicName", "ParamedicID");

            cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = userLoginParamedic.ToString();
            }

            cboPhysician.Enabled = false;
            cboPhysician2.Enabled = false;
            #endregion
        }

        private void EntityToControl(PatientReferral obj)
        {
            cboPhysician.Value = obj.FromPhysicianID.ToString();
            cboPhysician2.Value = obj.ToPhysicianID.ToString();

            if (!string.IsNullOrEmpty(obj.ReplySubjectiveText))
                txtReplySubjectiveText.Text = obj.ReplySubjectiveText;

            if (!string.IsNullOrEmpty(obj.ReplyObjectiveText))
                txtReplyObjectiveText.Text = obj.ReplyObjectiveText;

            if (!string.IsNullOrEmpty(obj.ReplyMedicalResumeText))
                txtReplyMedicalResumeText.Text = obj.ReplyMedicalResumeText;

            if (!string.IsNullOrEmpty(obj.ReplyDiagnosisText))
                txtReplyDiagnosisText.Text = obj.ReplyDiagnosisText;

            if (!string.IsNullOrEmpty(obj.ReplyPlanningResumeText))
                txtReplyPlanningResumeText.Text = obj.ReplyPlanningResumeText;

            if (!string.IsNullOrEmpty(obj.ReplyInstructionResumeText))
                txtInstructionResumeText.Text = obj.ReplyInstructionResumeText;

            if (obj.IsReply)
	        {
                txtReplyDate.Text = obj.ReplyDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReplyTime.Text = obj.ReplyTime;
            }
        }
    }
}