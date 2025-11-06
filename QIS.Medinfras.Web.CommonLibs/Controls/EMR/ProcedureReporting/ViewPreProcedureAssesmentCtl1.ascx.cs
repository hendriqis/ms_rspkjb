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
    public partial class ViewPreProcedureAssesmentCtl1 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected int visitID = 0;
        protected int chargesDtID = 0;
        protected int assessmentID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            visitID = Convert.ToInt32(paramInfo[0]);
            chargesDtID = Convert.ToInt32(paramInfo[1]);
            assessmentID = Convert.ToInt32(paramInfo[2]);

            hdnVisitID.Value = paramInfo[0];
            hdnPatientChargesDtID.Value = paramInfo[1];
            hdnAssessmentID.Value = paramInfo[2];

            lblTransactionNo.InnerHtml = paramInfo[3];
            lblItemName.InnerHtml = paramInfo[4];

            #region Patient Information
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.cfDateOfBirth, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));

            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.cfVisitDate, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl; 
            #endregion

            string filterExp = string.Format("PreSurgicalAssessmentID = {0}", assessmentID);
            vPreSurgeryAssessment obj = BusinessLayer.GetvPreSurgeryAssessmentList(filterExp).FirstOrDefault();
            lblPhysicianName2.InnerHtml = obj.ParamedicName;

            hdnMRN.Value = obj.MRN.ToString();

            if (obj != null)
            {
                #region Chief Complaint and History Of Illness
                txtDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = obj.AssessmentTime;
                txtPhysicianName.Text = obj.ParamedicName;
                txtAssessmentText.Text = obj.PreSurgeryAssessmentText;
                txtFamilyHistory.Text = obj.FamilyHistory;

                txtHPISummary.Text = obj.HPISummary;
                chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;

                txtMedicalHistory.Text = obj.PastMedicalHistory;
                txtMedicationHistory.Text = obj.PastMedicationHistory;
                txtPastSurgicalHistory.Text = obj.PastSurgicalHistory;
                #endregion

                #region HTML Form
                hdnDiagnosticTestLayout.Value = obj.DiagnosticTestChecklistLayout;
                hdnDiagnosticTestValue.Value = obj.DiagnosticTestChecklistValue;

                hdnDocumentChecklistLayout.Value = obj.DocumentChecklistLayout;
                hdnDocumentChecklistValue.Value = obj.DocumentChecklistValue;
                txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                #endregion

                if (!string.IsNullOrEmpty(obj.PreDiagnoseID))
                    txtPreDiagnosisID.Text = string.Format("{0} ({1})", obj.PreDiagnoseID, obj.PreDiagnoseText);
                else
                    txtPreDiagnosisID.Text = obj.PreDiagnoseText;

                txtProfilaxis.Text = obj.ProphylaxisSummary;
                txtPatientPositionSummary.Text = obj.PatientPositionSummary;
                txtEstimatedDuration.Text = obj.EstimatedDuration.ToString();
                txtSurgeryItemSummary.Text = obj.SurgeryItemSummary;
                txtReferralSummary.Text = obj.ReferralSummary;
                txtOtherSummary.Text = obj.OtherSummary;

                BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
                BindGridViewROS(1, true, ref gridROSPageCount);

                LoadBodyDiagram();
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

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", visitID, chargesDtID);

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
            string filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", visitID, chargesDtID);
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
            filterExpression = string.Format("VisitID = {0} AND PatientChargesDtID = {1} AND IsDeleted = 0", hdnVisitID.Value, hdnPatientChargesDtID.Value);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            if (entity != null)
            {
                BodyDiagramToControl(entity); 
            }

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
            filterExpression += string.Format("VisitID = {0} AND PreSurgeryAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND PreSurgeryAssessmentID = {1} AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value));
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
            string filterExpression = string.Format("VisitID IN ({0}) AND PreSurgeryAssessmentID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnAssessmentID.Value);

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