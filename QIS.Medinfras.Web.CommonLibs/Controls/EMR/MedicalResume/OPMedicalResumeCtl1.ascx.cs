using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using System.Text;
using System.IO;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class OPMedicalResumeCtl1 : BaseViewPopupCtl
    {
        protected int gridDiagnosisPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected int _visitID = 0;
        protected int _resumeID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
                _visitID = Convert.ToInt32(paramInfo[0]);
            else
                _visitID = 0;

            if (paramInfo[1] != "")
                _resumeID = Convert.ToInt32(paramInfo[1]);
            else
                _resumeID = 0;

            #region Patient Information
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", _visitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.cfDateOfBirth2, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.cfVisitDate, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            List<vPatientDiagnosis> lstPatientDiagnosis = BusinessLayer.GetvPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", registeredPatient.VisitID));
            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstPatientDiagnosis)
            {
                if (patientDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    diagnosis.AppendLine(string.Format("{0} ({1})", patientDiagnosis.DiagnosisText, patientDiagnosis.DiagnoseType));
                else
                    diagnosis.AppendLine(string.Format("{0}", patientDiagnosis.DiagnosisText));
            }

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            lblDiagnosis.InnerHtml = diagnosis.ToString();
            imgPatientImage.Src = registeredPatient.PatientImageUrl;

            #endregion

            string filterExpCC = string.Format("ID = {0} AND IsDeleted = 0", _resumeID);

            vMedicalResume obj = BusinessLayer.GetvMedicalResumeList(filterExpCC).FirstOrDefault();
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", obj.VisitID)).FirstOrDefault();
            if (obj != null)
            {

                lblMedicalResumeDateTime.InnerText = obj.cfMedicalResumeDateTime;
                lblResumeParamedicName.InnerHtml = obj.ParamedicName;
                hdnMRN.Value = registeredPatient.MRN.ToString();

                #region Resume Medis
                txtResumeDate.Text = obj.MedicalResumeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtResumeTime.Text = obj.MedicalResumeTime;
                txtSubjectiveResumeText.Text = obj.SubjectiveResumeText;
                txtObjectiveResumeText.Text = obj.ObjectiveResumeText;
                txtAssessmentResumeText.Text = obj.AssessmentResumeText;
                txtMedicalResumeText.Text = obj.MedicalResumeText;
                txtMedicationResumeText.Text = obj.MedicationResumeText;
                txtPlanningResumeText.Text = obj.PlanningResumeText;
                txtInstructionResumeText.Text = obj.InstructionResumeText;

                //rblIsHasSickLetter.SelectedValue = obj.IsHasSickLetter ? "1" : "0";
                //txtNoOfDays.Text = obj.NoOfAbsenceDays.ToString();

                lblRevisionDateTime.InnerHtml = obj.cfRevisionDateTime;
                #endregion

            }
        }
    }
}