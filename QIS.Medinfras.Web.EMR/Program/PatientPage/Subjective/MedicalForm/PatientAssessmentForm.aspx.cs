using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientAssessmentForm : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                return Constant.MenuCode.EMR.MEDICAL_ASSESSMENT_FORM;
            }
            else
            {
                string formType = string.Format("X397^{0}", id);
                switch (formType)
                {
                    case Constant.AssessmentFormGroup.DOKTER_BEDAH_ANESTESI:
                        return Constant.MenuCode.EMR.FORM_PENGKAJIAN_KAMAR_OPERASI;
                    default:
                        return Constant.MenuCode.EMR.MEDICAL_ASSESSMENT_FORM;
                }
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            vConsultVisit4 entity = BusinessLayer.GetvConsultVisit4List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();

            if (entity != null)
            {
                hdnPageMedicalNo.Value = entity.MedicalNo;
                hdnPagePatientDOB.Value = entity.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT);
                hdnPagePatientName.Value = entity.PatientName;
                hdnPageRegistrationNo.Value = entity.RegistrationNo;
            }

            BindGridView(1, false, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            //Physician Assessment Form
            string filterExpression = "";

            string id = Page.Request.QueryString["id"];
            if (string.IsNullOrEmpty(id))
            {
                filterExpression = string.Format("ParentID = '{0}' AND TagProperty = '1' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_ASSESSMENT_FORM);
            }
            else
            {
                string formType = string.Format("X397^{0}", id);
                if (formType == Constant.AssessmentFormGroup.DOKTER_BEDAH_ANESTESI)
                {
                    filterExpression = string.Format("(ParentID = '{0}' OR (ParentID = '{1}' AND TagProperty = '1')) AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID",
                                                        Constant.StandardCode.FORM_PENGKAJIAN_KAMAR_OPERASI, Constant.StandardCode.PATIENT_ASSESSMENT_FORM);
                }
                else
                {
                    filterExpression = string.Format("ParentID = '{0}' AND TagProperty = '1' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_ASSESSMENT_FORM);
                }
            }

            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind();
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("RegistrationID IN ({0},{1}) AND GCAssessmentType = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.LinkedRegistrationID, hdnGCAssessmentType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAssessment> lstEntity = BusinessLayer.GetvPatientAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
        }


        protected void cbpFormList_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDt(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDt(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PatientAssessmentFormEntry.ascx");
            //queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", "1", hdnGCAssessmentType.Value, hdnID.Value, hdnAssessmentDate.Value, hdnAssessmentTime.Value, hdnParamedicID.Value, hdnFallRiskScore.Value, hdnGCFallRiskScoreType.Value, hdnIsFallRisk.Value, hdnAssessmentValues.Value, hdnIsInitialAssessment.Value);
            //queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", "1", hdnGCAssessmentType.Value, hdnID.Value, hdnAssessmentDate.Value, hdnAssessmentTime.Value, hdnParamedicID.Value, hdnAssessmentValues.Value, hdnIsInitialAssessment.Value, "X397^003");
            queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|0|0", "1", hdnGCAssessmentType.Value, "0", AppSession.RegisteredPatient.VisitID.ToString(), "X397^003", hdnPageMedicalNo.Value, hdnPagePatientName.Value, hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value);
            popupWidth = 800;
            popupHeight = 600;
            popupHeaderText = string.Format("Pengkajian Pasien : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            //if (hdnID.Value != "")
            PatientAssessment entity = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PatientAssessmentFormEntry.ascx");
                //queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}", "0", hdnGCAssessmentType.Value, hdnID.Value, hdnAssessmentDate.Value, hdnAssessmentTime.Value, hdnParamedicID.Value, hdnFallRiskScore.Value, hdnGCFallRiskScoreType.Value, hdnIsFallRisk.Value, hdnAssessmentValues.Value, hdnIsInitialAssessment.Value);
                //queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", "0", hdnGCAssessmentType.Value, hdnID.Value, hdnAssessmentDate.Value, hdnAssessmentTime.Value, hdnParamedicID.Value, hdnAssessmentValues.Value, hdnIsInitialAssessment.Value, "X397^003");
                queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|0|0", "0", hdnGCAssessmentType.Value, hdnID.Value, AppSession.RegisteredPatient.VisitID.ToString(), "X397^003", hdnPageMedicalNo.Value, hdnPagePatientName.Value, hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value);
                popupWidth = 800;
                popupHeight = 600;
                popupHeaderText = string.Format("Pengkajian Pasien : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            //if (hdnID.Value != "")
            PatientAssessment entity = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                //PatientAssessment entity = BusinessLayer.GetPatientAssessment(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientAssessment(entity);
                return true;
            }
            return false;
        }
    }
}