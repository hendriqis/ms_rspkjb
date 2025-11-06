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
    public partial class NTNutritionistAssessmentCtl1 : BaseViewPopupCtl
    {
        protected int gridVitalSignPageCount = 1;
        private List<vVitalSignDt> lstVitalSignDt = null;
        protected int VisitID = 0;
        protected int _assessmentID = 0;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            if (paramInfo[0] != "")
                VisitID = Convert.ToInt32(paramInfo[0]);
            else
                VisitID = AppSession.RegisteredPatient.VisitID;

            #region Patient Information
            vConsultVisit9 registeredPatient = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", VisitID))[0];
            lblPatientName.InnerHtml = registeredPatient.cfPatientNameSalutation;
            lblGender.InnerHtml = registeredPatient.Gender;
            lblDateOfBirth.InnerHtml = string.Format("{0} ({1})", registeredPatient.DateOfBirthInString, Helper.GetPatientAge(words, registeredPatient.DateOfBirth));
            hdnVisitIDCtl.Value = registeredPatient.VisitID.ToString();
            lblRegistrationDateTime.InnerHtml = string.Format("{0} / {1}", registeredPatient.VisitDateInString, registeredPatient.VisitTime);
            lblRegistrationNo.InnerHtml = registeredPatient.RegistrationNo;
            lblPhysician.InnerHtml = registeredPatient.ParamedicName;

            lblMedicalNo.InnerHtml = registeredPatient.MedicalNo;

            hdnMRN.Value = registeredPatient.MRN.ToString();

            lblPayerInformation.InnerHtml = registeredPatient.BusinessPartnerName;
            lblPatientLocation.InnerHtml = registeredPatient.cfPatientLocation;
            imgPatientImage.Src = registeredPatient.PatientImageUrl;
            #endregion

            string filterExpCC = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY ID DESC", VisitID);
            if (_assessmentID != 0)
            {
                filterExpCC = string.Format("VisitID = {0} AND NutritionAssessmentID = {1} AND IsDeleted = 0 ORDER BY ID DESC", VisitID, _assessmentID);
            }

            vNutritionAssessment oChiefComplaint = BusinessLayer.GetvNutritionAssessmentList(filterExpCC).FirstOrDefault();

            if (oChiefComplaint != null)
            {
                lblAssessmentParamedicName.InnerHtml = oChiefComplaint.ParamedicName;
                if (paramInfo[1] != "")
                {
                    hdnNutritionAssessmentID.Value = paramInfo[1];
                }
                else
                {
                    hdnNutritionAssessmentID.Value = oChiefComplaint.ID.ToString();
                }

                txtDate.Text = oChiefComplaint.AssessmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTime.Text = oChiefComplaint.AssessmentTime;
                txtPhysicianName.Text = oChiefComplaint.ParamedicName;
                txtNutritionHistory.Text = oChiefComplaint.NutritionHistory;
                chkAutoAnamnesis.Checked = oChiefComplaint.IsAutoAnamnesis;
                chkAlloAnamnesis.Checked = oChiefComplaint.IsAlloAnamnesis;
                txtMedicalHistory.Text = oChiefComplaint.MedicalHistory;
                txtAntropometricNotes.Text = oChiefComplaint.AntropometricNotes;
                txtBiochemistryNotes.Text = oChiefComplaint.BiochemistryNotes;
                txtPhysicalNotes.Text = oChiefComplaint.PhysicalNotes;
                txtProblem.Text = oChiefComplaint.Problem;
                txtEtiology.Text = oChiefComplaint.Etiology;
                txtSymptom.Text = oChiefComplaint.Symptoms;
                txtInterventionPurpose.Text = oChiefComplaint.InterventionGoal;
                txtInterventionDiet.Text = oChiefComplaint.NutritionDelivery;
                txtInterventionEducation.Text = oChiefComplaint.NutritionEducation;
                txtInterventionCollaboration.Text = oChiefComplaint.NutritionCounseling;
                txtMonitoring.Text = oChiefComplaint.Monitoring;
                txtEvaluation.Text = oChiefComplaint.Evaluation;

                BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            }
        }

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ({0}) AND NutritionAssessmentID = {1} AND IsDeleted = 0 AND GCParamedicMasterType = '{2}' ORDER BY ID DESC", VisitID, hdnNutritionAssessmentID.Value, Constant.ParamedicType.Nutritionist);

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression);
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID IN ({0}) AND GCParamedicMasterType = '{1}' AND NutritionAssessmentID = {2} ORDER BY DisplayOrder", VisitID, Constant.ParamedicType.Nutritionist, hdnNutritionAssessmentID.Value));
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