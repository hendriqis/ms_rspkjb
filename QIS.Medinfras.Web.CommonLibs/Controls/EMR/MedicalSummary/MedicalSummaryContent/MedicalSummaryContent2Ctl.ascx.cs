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
using DevExpress.Web.ASPxCallbackPanel;

namespace QIS.Medinfras.Web.CommonLibs.Controls
{
    public partial class MedicalSummaryContent2Ctl : BaseViewPopupCtl
    {
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        protected int gridROSPageCount = 1;
        protected int _visitID = 0;
        protected int _visitNoteID = 0;

        public override void InitializeDataControl(string queryString)
        {
            if (queryString != "")
                _visitID = Convert.ToInt32(queryString);
            else
                _visitID = AppSession.RegisteredPatient.VisitID;

            InitControlProperties();

            LoadContentInformation(Convert.ToInt32(queryString));
        }

        private void InitControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0",
Constant.StandardCode.MST_WEIGHT_CHANGED, Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP, Constant.StandardCode.MST_DIAGNOSIS);

            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            //lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            //Methods.SetComboBoxField(cboGCWeightChangedStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField(cboGCWeightChangedGroup, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_WEIGHT_CHANGED_GROUP || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            //Methods.SetComboBoxField(cboGCMSTDiagnosis, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.MST_DIAGNOSIS || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
        }

        private void LoadContentInformation(int visitID)
        {
            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0} AND IsInpatientInitialAssessment = 1", _visitID)).FirstOrDefault();
            if (obj != null)
            {
                txtServiceDate.Text = obj.ObservationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = obj.ObservationTime;
                txtChiefComplaint.Text = obj.ChiefComplaintText;
                txtHPISummary.Text = obj.HPISummary;
                txtMedicalHistory.Text = obj.PastMedicalHistory;
                txtMedicationHistory.Text = obj.PastMedicationHistory;
                txtDiagnosticResultSummary.Text = obj.DiagnosticResultSummary;
                txtFamilyHistory.Text = obj.FamilyHistory;
                txtNursingObjectives.Text = obj.NursingObjectives;
                rblIsNeedDischargePlan.SelectedValue = obj.IsNeedDischargePlan ? "1" : "0";
                txtEstimatedLOS.Text = !string.IsNullOrEmpty(obj.EstimatedLOS.ToString("N0")) ? obj.EstimatedLOS.ToString("N0") : "0";
                rblEstimatedLOSUnit.SelectedValue = obj.IsEstimatedLOSInDays ? "1" : "0";
                chkAlloAnamnesis.Checked = obj.IsAlloAnamnesis;
                chkAutoAnamnesis.Checked = obj.IsAutoAnamnesis;
                txtVisitTypeName.Text = obj.VisitTypeName;

                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", _visitID);
                vMSTAssessment mst = BusinessLayer.GetvMSTAssessmentList(filterExpression).FirstOrDefault();
                if (mst != null)
                {
                    txtGCWeightChangedStatus.Text = mst.WeightChangedStatus;
                    txtGCWeightChangedGroup.Text = mst.WeightChangedGroup;
                    txtGCMSTDiagnosis.Text = mst.MSTDiagnosis;

                    if (mst.GCFoodIntakeChanged != null)
                        rblIsFoodIntakeChanged.SelectedValue = mst.GCFoodIntakeChanged == "X450^01" ? "1" : "0";
                    else
                        rblIsFoodIntakeChanged.SelectedValue = "0";
                    txtFoodIntakeScore.Text = rblIsFoodIntakeChanged.SelectedValue;

                    txtOtherMSTDiagnosis.Text = mst.OtherMSTDiagnosis;
                    txtTotalMST.Text = mst.MSTScore.ToString();
                }
                else
                {
                    txtGCWeightChangedStatus.Text = "";
                    txtGCWeightChangedGroup.Text = "";
                    txtGCMSTDiagnosis.Text = "";
                    rblIsFoodIntakeChanged.SelectedValue = null;
                    txtOtherMSTDiagnosis.Text = string.Empty;
                    txtFoodIntakeScore.Text = "0";
                    txtTotalMST.Text = "0";
                }

                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}' AND IsDeleted = 0", _visitID, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT)).FirstOrDefault();
                if (oVisitNote != null)
                {
                    _visitNoteID = oVisitNote.ID;
                    txtInstructionText.Text = oVisitNote.InstructionText;
                }
               
                BindGridViewROS(1, true, ref gridROSPageCount);
            }
        }

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
            string filterExpression = string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsDeleted = 0", _visitID, _visitNoteID);

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