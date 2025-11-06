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

namespace QIS.Medinfras.Web.CommonLibs.Program.PatientPage
{
    public partial class ViewPhysicianReferralResponseCtl : BaseViewPopupCtl
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
            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}' AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {1})", Constant.ParamedicType.Physician, AppSession.RegisteredPatient.HealthcareServiceUnitID));
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
            if (string.IsNullOrEmpty(obj.ReplyDiagnosisText))
                txtDiagnosisText.Text = obj.DiagnosisText;
            else
                txtDiagnosisText.Text = obj.ReplyDiagnosisText;

            if (string.IsNullOrEmpty(obj.ReplyMedicalResumeText))
                txtMedicalResumeText.Text = obj.MedicalResumeText;
            else
                txtMedicalResumeText.Text = obj.ReplyMedicalResumeText;

            if (string.IsNullOrEmpty(obj.ReplyPlanningResumeText))           
                txtPlanningResumeText.Text = obj.PlanningResumeText; 
            else
                txtPlanningResumeText.Text = obj.ReplyPlanningResumeText;

            if (string.IsNullOrEmpty(obj.ReplyMedicalResumeText))
	        {
                txtReplyDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReplyTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
	        }
            else
            {
                txtReplyDate.Text = obj.ReplyDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReplyTime.Text = obj.ReplyTime;
            }
        }
    }
}