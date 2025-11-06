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
    public partial class MDNurseInitialAssessmentCtl1 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected int _visitID = 0;
        protected int _assessmentID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
                _visitID = Convert.ToInt32(paramInfo[0]);
            else
                _visitID = AppSession.RegisteredPatient.VisitID;

            if (!string.IsNullOrEmpty(paramInfo[1]))
            {
                _assessmentID = Convert.ToInt32(paramInfo[1]);
            }

            #region Patient Information
            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", _visitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.DateOfBirthInString, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDateInString, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
            #endregion

            string filterExpCC = string.Format("VisitID = {0} AND IsMedicalDiagnostic = 1 AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", _visitID);
            if (_assessmentID != 0)
            {
                filterExpCC = string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsMedicalDiagnostic = 1 AND IsDeleted = 0 ORDER BY ChiefComplaintID DESC", _visitID, _assessmentID);
            }
            vNurseChiefComplaint oChiefComplaint = BusinessLayer.GetvNurseChiefComplaintList(filterExpCC).FirstOrDefault();
            lblPhysicianName2.InnerHtml = registeredPatient.ParamedicName;

            if (oChiefComplaint != null)
            {
                lblAssessmentParamedicName.InnerHtml = oChiefComplaint.ParamedicName;
                lblPrimaryNurseName.InnerHtml = oChiefComplaint.PrimaryNurseName;
                hdnMRN.Value = oChiefComplaint.MRN.ToString();

                #region Chief Complaint and History Of Illness
                txtDate.Text = oChiefComplaint.ChiefComplaintDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = oChiefComplaint.ChiefComplaintTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtChiefComplaint.Text = oChiefComplaint.NurseChiefComplaintText;
                lblHealthcareServiceUnitName.InnerHtml = oChiefComplaint.AssessmentServiceUnitName;

                txtHPISummary.Text = oChiefComplaint.HPISummary;
                chkAutoAnamnesis.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = oChiefComplaint.IsAlloAnamnesis;
                txtMedicalHistory.Text = oChiefComplaint.MedicalHistory;
                txtMedicationHistory.Text = oChiefComplaint.MedicationHistory;
                txtFamilyHistory.Text = oChiefComplaint.FamilyHistory;
                #endregion

                #region HTML Form
                //hdnPhysicalExamLayout.Value = oChiefComplaint.PhysicalExamLayout;
                //hdnPhysicalExamValue.Value = oChiefComplaint.PhysicalExamValues;

                hdnSocialHistoryLayout.Value = oChiefComplaint.SocialHistoryLayout;
                hdnSocialHistoryValue.Value = oChiefComplaint.SocialHistoryValues;

                hdnEducationLayout.Value = oChiefComplaint.EducationLayout;
                hdnEducationValue.Value = oChiefComplaint.EducationValues;

                hdnDischargePlanningLayout.Value = oChiefComplaint.DischargePlanningLayout;
                hdnDischargePlanningValue.Value = oChiefComplaint.DischargePlanningValues;

                hdnAdditionalAssessmentLayout.Value = oChiefComplaint.AdditionalAssessmentLayout;
                hdnAdditionalAssessmentValue.Value = oChiefComplaint.AdditionalAssessmentValues;
                #endregion

                BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                BindGridViewROS(1, true, ref gridROSPageCount);

                //LoadBodyDiagram();
            }
        }

        #region Allergy
        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();

            chkIsPatientAllergyExists.Checked = !(lstEntity.Count > 0);
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

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

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();

            StringBuilder diagnosis = new StringBuilder();
            foreach (vPatientDiagnosis patientDiagnosis in lstEntity)
            {
                if (patientDiagnosis.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS)
                    diagnosis.AppendLine(string.Format("{0} ({1})", patientDiagnosis.DiagnosisText, patientDiagnosis.DiagnoseType));
                else
                    diagnosis.AppendLine(string.Format("{0}", patientDiagnosis.DiagnosisText));
            }

            lblDiagnosis.InnerHtml = diagnosis.ToString();
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

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadBodyDiagram(pageIndex);
                result = "index|" + pageIndex;
            }

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void BodyDiagramToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND IsInitialAssessment = 1 AND IsDeleted = 0 AND GCParamedicMasterType != '{1}' ORDER BY ID DESC", _visitID, Constant.ParamedicType.Physician);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND IsInitialAssessment = 1 AND GCParamedicMasterType != '{1}' ORDER BY DisplayOrder", _visitID, Constant.ParamedicType.Physician));
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
            string filterExpression = string.Format("VisitID = {0} AND IsInitialAssessment = 1 AND GCParamedicMasterType != '{1}' AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, Constant.ParamedicType.Physician);

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