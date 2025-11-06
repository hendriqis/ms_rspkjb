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
    public partial class IPMedicalResumeCtl1 : BaseViewPopupCtl
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
            txtHospitalIndication.Text = registeredPatient.HospitalizationIndication;

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
                txtComorbiditiesText.Text = obj.ComorbiditiesText;
                txtObjectiveResumeText.Text = obj.ObjectiveResumeText;
                txtMedicalResumeText.Text = obj.MedicalResumeText;
                txtMedicationResumeText.Text = obj.MedicationResumeText;
                txtPlanningResumeText.Text = obj.PlanningResumeText;

                txtDischargeMedicationResumeText.Text = obj.DischargeMedicationResumeText;
                txtDischargeMedicalSummary.Text = obj.DischargeMedicalResumeText;
                txtSurgeryResumeText.Text = obj.SurgeryResumeText;
                txtInstructionResumeText.Text = obj.InstructionResumeText;

                rblIsHasSickLetter.SelectedValue = obj.IsHasSickLetter ? "1" : "0";
                txtNoOfDays.Text = obj.NoOfAbsenceDays.ToString();

                lblRevisionDateTime.InnerHtml = obj.cfRevisionDateTime;
                #endregion

                txtPatientOutcome.Text = obj.DischargeCondition;
                txtDischargeRoutine.Text = obj.DischargeMethod;
                if (obj.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || obj.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
                {
                    trDeathInfo.Style.Add("Display", "table-row");
                    txtDateOfDeath.Text = obj.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtTimeOfDeath.Text = obj.TimeOfDeath;
                }
                if (!string.IsNullOrEmpty(obj.ReferrerGroup))
                {
                    trReferrerGroup.Style.Add("Display", "table-row");
                    txtReferrerGroup.Text = obj.ReferrerGroup;
                    txtReferrerCode.Text = obj.ReferrerCode;
                    txtReferrerName.Text = obj.ReferrerName;
                    txtDischargeReason.Text = obj.ReferralDischargeReason;
                    txtDischargeOtherReason.Text = obj.ReferralDischargeReasonOther;
                }

                if (entityVisit.PlanFollowUpVisitDate.Year != 1900)
                {
                    txtPlanFollowUpVisitDate.Text = entityVisit.PlanFollowUpVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }

                BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                BindGridViewROS(1, true, ref gridROSPageCount);
            }
        }

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = string.Empty;
        } 
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;

            filterExpression += string.Format("VisitID = {0} AND MedicalResumeID = {1} AND IsDeleted = 0", _visitID, _resumeID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} ORDER BY DisplayOrder", _visitID, _resumeID));
            grdVitalSignView.DataSource = lstEntity;
            grdVitalSignView.DataBind();
        }

        protected void grdVitalSignView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vVitalSignHd obj = (vVitalSignHd)e.Row.DataItem;
                Repeater rptVitalSignDt = (Repeater)e.Row.FindControl("rptVitalSignDt");
                rptVitalSignDt.DataSource = GetVitalSignDt(obj.ID);
                rptVitalSignDt.DataBind();
            }
        }

        protected List<vVitalSignDt> GetVitalSignDt(Int32 ID)
        {
            return lstVitalSignDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpVitalSignView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewVitalSign(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewVitalSign(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }
        }    

        #endregion

        #region Review of System
        protected void grdROSView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vReviewOfSystemHd obj = (vReviewOfSystemHd)e.Row.DataItem;
                Repeater rptReviewOfSystemDt = (Repeater)e.Row.FindControl("rptReviewOfSystemDt");
                rptReviewOfSystemDt.DataSource = GetReviewOfSystemDt(obj.ID);
                rptReviewOfSystemDt.DataBind();
            }
        }

        protected List<vReviewOfSystemDt> GetReviewOfSystemDt(Int32 ID)
        {
            return lstReviewOfSystemDt.Where(p => p.ID == ID).ToList();
        }

        protected void cbpROSView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewROS(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewROS(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewROS(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID IN ({0}) AND MedicalResumeID = {1} AND IsDeleted = 0", _visitID, _resumeID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(filterExpression);
            grdROSView.DataSource = lstEntity;
            grdROSView.DataBind();
        }
        #endregion
    }
}