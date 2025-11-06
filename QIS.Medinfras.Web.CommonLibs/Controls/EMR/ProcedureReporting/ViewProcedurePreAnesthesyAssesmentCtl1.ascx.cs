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
    public partial class ViewProcedurePreAnesthesyAssesmentCtl1 : BaseViewPopupCtl
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
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
            txtTransactionNo.Text = paramInfo[3];
            txtTransactionNo.Enabled = false;
            txtItemName.Text = paramInfo[4];


            string filterExp = string.Format("PreAnesthesyAssessmentID = {0}", assessmentID);
            vPreAnesthesyAssessment obj = BusinessLayer.GetvPreAnesthesyAssessmentList(filterExp).FirstOrDefault();
            vConsultVisit4 registeredPatient = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", visitID))[0];
            lblPhysicianName2.InnerHtml = obj.ParamedicName;
            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            hdnMRN.Value = obj.MRN.ToString();

            if (obj != null)
            {
                hdnAssessmentID.Value = obj.PreAnesthesyAssessmentID.ToString();
                txtServiceDate.Text = obj.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = obj.AssessmentTime;
                txtAnamnesisText.Text = obj.PreAnesthesyAssessmentText;
                txtPastSurgicalHistory.Text = obj.PastSurgicalHistory;
                txtMedicationHistory.Text = obj.PastMedicationHistory;
                chkIsPatientAllergyExists.Checked = !chkIsPatientAllergyExists.Checked;
                chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                txtFamilyRelation.Text = obj.FamilyRelation;
                divFormContent1.InnerHtml = obj.PhysicalExamLayout;
                hdnPhysicalExamLayout.Value = obj.PhysicalExamLayout;
                hdnPhysicalExamValue.Value = obj.PhysicalExamValue;
                hdnDiagnosticTestCheckListLayout.Value = obj.DiagnosticTestLayout;
                hdnDiagnosticTestCheckListValue.Value = obj.DiagnosticTestValue;
                divFormContent2.InnerHtml = obj.DiagnosticTestLayout;
                txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                divFormContent3.InnerHtml = obj.AnesthesyPlanLayout;
                hdnAnesthesyPlanningLayout.Value = obj.AnesthesyPlanLayout;
                hdnAnesthesyPlanningValue.Value = obj.AnesthesyPlanValue;

                if (!string.IsNullOrEmpty(obj.StartFastingTime))
                {
                    txtStartFastingDate.Text = obj.StartFastingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtStartFastingTime.Text = obj.StartFastingTime;
                }
                rblGCASAStatus.SelectedValue = obj.GCASAStatus;
                chkIsASAStatusE.Checked = obj.IsASAStatusE;
                txtAnesthesiaType.Text = obj.cfAnesthesiaType;
                txtRegionalAnesthesiaType.Text = obj.RegionalAnesthesiaType;
                txtPremedication.Text = obj.Premedication;
                chkIsAsthma.Checked = obj.IsHasAsthma;

                BindGridViewAllergy(1, true, ref gridAllergyPageCount);
                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
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

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND PreAnesthesyAssessmentID = {1} AND IsDeleted = 0", visitID, hdnAssessmentID.Value);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND PreAnesthesyAssessmentID = {1} AND IsDeleted = 0 ORDER BY DisplayOrder", visitID, hdnAssessmentID.Value));
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

    }
}