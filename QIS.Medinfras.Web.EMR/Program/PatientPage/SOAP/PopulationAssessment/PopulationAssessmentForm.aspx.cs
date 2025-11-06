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
    public partial class PopulationAssessmentForm : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            switch (AppSession.RegisteredPatient.DepartmentID)
            {
                case Constant.Facility.OUTPATIENT:
                    return Constant.MenuCode.EMR.OUTPATIENT_SOAP_POPULATION_ASSESSMENT;
                case Constant.Facility.EMERGENCY:
                    return Constant.MenuCode.EMR.EMERGENCY_SOAP_POPULATION_ASSESSMENT;
                case Constant.Facility.INPATIENT:
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_POPULATION_ASSESSMENT;
                default:
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_POPULATION_ASSESSMENT;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

            hdnPageMedicalNo.Value = AppSession.RegisteredPatient.MedicalNo;
            hdnPagePatientDOB.Value = AppSession.RegisteredPatient.DateOfBirth.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnPagePatientName.Value = AppSession.RegisteredPatient.PatientName;
            hdnPageRegistrationNo.Value = AppSession.RegisteredPatient.RegistrationNo;

            BindGridView(1, false, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("ParentID = '{0}' ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_ADDITIONAL_ASSESSMENT_FORM);
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
                int rowCount = BusinessLayer.GetvPopulationAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPopulationAssessment> lstEntity = BusinessLayer.GetvPopulationAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
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
            url = ResolveUrl("~/Program/PatientPage/SOAP/PopulationAssessment/PopulationAssessmentFormEntry.ascx");
            queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}", "1", 
                                            hdnGCAssessmentType.Value, //0
                                            hdnID.Value, //1
                                            hdnAssessmentDate.Value, //2
                                            hdnAssessmentTime.Value, //3
                                            hdnParamedicID.Value, //4
                                            hdnAssessmentValues.Value, //5
                                            hdnIsInitialAssessment.Value, //6
                                            hdnAssessmentLayout.Value, //7
                                            AppSession.RegisteredPatient.VisitID, //8
                                            hdnPageMedicalNo.Value, //9
                                            hdnPagePatientName.Value, //10
                                            hdnPagePatientDOB.Value, //11
                                            hdnPageRegistrationNo.Value); //12
            popupWidth = 800;
            popupHeight = 600;
            popupHeaderText = string.Format("Pengkajian Populasi Khusus : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            PopulationAssessment entity = BusinessLayer.GetPopulationAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                url = ResolveUrl("~/Program/PatientPage/SOAP/PopulationAssessment/PopulationAssessmentFormEntry.ascx"); 
                queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}", "0",
                                 hdnGCAssessmentType.Value, //0
                                 hdnID.Value, //1
                                 hdnAssessmentDate.Value, //2
                                 hdnAssessmentTime.Value, //3
                                 hdnParamedicID.Value, //4
                                 hdnAssessmentValues.Value, //5
                                 hdnIsInitialAssessment.Value, //6
                                 hdnAssessmentLayout.Value, //7
                                 AppSession.RegisteredPatient.VisitID, //8
                                 hdnPageMedicalNo.Value, //9
                                 hdnPagePatientName.Value, //10
                                 hdnPagePatientDOB.Value, //11
                                 hdnPageRegistrationNo.Value); //12
                popupWidth = 800;
                popupHeight = 600;
                popupHeaderText = string.Format("Pengkajian Populasi Khusus : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            //if (hdnID.Value != "")
            PopulationAssessment entity = BusinessLayer.GetPopulationAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                //PopulationAssessment entity = BusinessLayer.GetPopulationAssessment(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePopulationAssessment(entity);
                return true;
            }
            return false;
        }
    }
}