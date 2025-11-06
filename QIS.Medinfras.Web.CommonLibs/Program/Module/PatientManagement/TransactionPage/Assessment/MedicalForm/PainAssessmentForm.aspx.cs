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
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PainAssessmentForm : BasePagePatientPageList
    {
        string deptType = string.Empty;
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                #region Follow-up Pasien Pulang
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.FOLLOWUP_PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PAIN_ASSESSMENT_FORM;
                    default: return Constant.MenuCode.Inpatient.FOLLOWUP_PAIN_ASSESSMENT_FORM;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.DATA_PATIENT_PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.MEDICAL_CHECKUP: return Constant.MenuCode.MedicalCheckup.DATA_PATIENT_PAIN_ASSESSMENT_FORM;
                    default: return Constant.MenuCode.Outpatient.DATA_PATIENT_PAIN_ASSESSMENT_FORM;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT: 
                        return Constant.MenuCode.Inpatient.PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.PAIN_ASSESSMENT_FORM;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_EMERGENCY_PAIN_ASSESSMENT;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PAIN_ASSESSMENT_FORM;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_PAIN_ASSESSMENT_FORM;
                    case "EMR": 
                        return Constant.MenuCode.EMR.MEDICAL_ASSESSMENT_FORM;
                    default: 
                        return Constant.MenuCode.Inpatient.PAIN_ASSESSMENT_FORM;
                }
                #endregion
            }

        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            string[] param = Page.Request.QueryString["id"].Split('|');
            if (param.Count() > 1)
            {
                deptType = param[0];
                menuType = param[1];
            }
            else
            {
                deptType = param[0];
            }

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
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PAIN_ASSESSMENT);
            List<StandardCode> lstEntity = BusinessLayer.GetStandardCodeList(filterExpression);
            grdFormList.DataSource = lstEntity;
            grdFormList.DataBind(); 
        }

        private void BindGridViewDt(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = ""; //hdnFilterExpression.Value;
            if (filterExpression != "")
                filterExpression += " AND ";

            filterExpression += string.Format("RegistrationID IN ({0},{1}) AND GCPainAssessmentType = '{2}' AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, AppSession.RegisteredPatient.LinkedRegistrationID, hdnGCAssessmentType.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPainAssessmentRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPainAssessment> lstEntity = BusinessLayer.GetvPainAssessmentList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "AssessmentID DESC");
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
                else if (param[0] == "printout")
                {
                    BindPrintout();
                    result = "printout";
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

        private void BindPrintout()
        {
            if (!string.IsNullOrEmpty(hdnID.Value) && hdnID.Value != "0")
            {

                List<vPainAssessment> lstPain = BusinessLayer.GetvPainAssessmentList(string.Format("RegistrationID = '{0}' AND IsDeleted = 0 ORDER BY AssessmentID DESC", AppSession.RegisteredPatient.RegistrationID));
                if (lstPain.Count > 0)
                {
                    string strhtml = "";
                    List<string> content = new List<string>();
                    string readString = string.Empty;
                    string path = HttpContext.Current.Server.MapPath("~/Libs/App_Data/medicalForm/Pain");
                    string fileName = string.Format(@"{0}\PainPrint.html", path);

                    foreach (vPainAssessment row in lstPain)
                    {
                        string chk1 = "";
                        string chk2 = "";
                        string FormLayout = row.AssessmentFormLayout.ToString();
                        string ParamValue = row.AssessmentFormValue.ToString();
                        string contentLayout = Helper.ParsingFormElektronik(FormLayout, ParamValue, string.Empty);
                        if (row.IsPain == true)
                        {
                            chk1 = string.Format("checked");
                        }
                        if (row.IsInitialAssessment == true)
                        {
                            chk2 = string.Format("checked");
                        }
                        IEnumerable<string> lstText = File.ReadAllLines(fileName);
                        StringBuilder innerHtml = new StringBuilder();
                        string readText = File.ReadAllText(fileName);
                        readString = Regex.Replace(readText, "#date", string.Format("{0} {1}<br/>", row.cfAssessmentDate, row.AssessmentTime));
                        readString = Regex.Replace(readString, "#ppa", string.Format("{0}<br/>", row.ParamedicName));
                        readString = Regex.Replace(readString, "#provocation", string.Format("{0}", row.ProvokingType));
                        readString = Regex.Replace(readString, "#provocationtext", string.Format("{0}<br/>", row.Provoking));
                        readString = Regex.Replace(readString, "#quality", string.Format("{0}", row.QualityType));
                        readString = Regex.Replace(readString, "#qualitytext", string.Format("{0}<br/>", row.Quality));
                        readString = Regex.Replace(readString, "#regio", string.Format("{0}", row.RegioType));
                        readString = Regex.Replace(readString, "#regiotext", string.Format("{0}<br/>", row.Regio));
                        readString = Regex.Replace(readString, "#time", string.Format("{0}", row.TimeType));
                        readString = Regex.Replace(readString, "#timetext", string.Format("{0}<br/>", row.Time));
                        readString = Regex.Replace(readString, "#content", string.Format("{0}<br/>", contentLayout));
                        readString = Regex.Replace(readString, "#skorvalue", string.Format("{0}", row.PainScore));
                        readString = Regex.Replace(readString, "#skorname", string.Format("{0}<br/>", row.PainScoreType));
                        readString = Regex.Replace(readString, "#kesimpulan", string.Format("{0}<br/>", row.PainScoreType));
                        readString = Regex.Replace(readString, "#checked1", string.Format("{0}", chk1));
                        readString = Regex.Replace(readString, "#checked2", string.Format("{0}", chk2));
                        strhtml = string.Format("{0}<br/>", readString);
                        content.Add(strhtml);
                    }
                    hdnFileString.Value = string.Join("", content);

                    #region createpdf
                    //StringReader sr = new StringReader(ovMCUResultForm.FormLayout.ToString());
                    //Document pdfDoc = new Document(PageSize.LETTER_LANDSCAPE);
                    //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                    //using (MemoryStream memoryStream = new MemoryStream())
                    //{
                    //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                    //    pdfDoc.Open();
                    //    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    //    //htmlparser.Parse(sr);
                    //    pdfDoc.Close();

                    //    byte[] bytes = memoryStream.ToArray();
                    //    hdnFileString.Value = Convert.ToBase64String(bytes);
                    //    memoryStream.Close();
                    //}
                    #endregion
                }
            }
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PainAssessmentFormEntry.ascx");
            queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}","1",hdnGCAssessmentType.Value,"0",AppSession.RegisteredPatient.VisitID, hdnPageMedicalNo.Value, hdnPagePatientName.Value,hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value);
            popupWidth = 800;
            popupHeight = 600;
            popupHeaderText = string.Format("Skala Nyeri : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo,AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            PainAssessment entity = BusinessLayer.GetPainAssessment(Convert.ToInt32(hdnID.Value));
            if (hdnID.Value != "" && entity.IsDeleted != true)
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PainAssessmentFormEntry.ascx");
                queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", "0", hdnGCAssessmentType.Value, hdnID.Value, AppSession.RegisteredPatient.VisitID, hdnPageMedicalNo.Value, hdnPagePatientName.Value, hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value);
                popupWidth = 800;
                popupHeight = 600;
                popupHeaderText = string.Format("Skala Nyeri : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
                return true;
            }
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PainAssessment entity = BusinessLayer.GetPainAssessment(Convert.ToInt32(hdnID.Value));
                if (!entity.IsDeleted)
                {
                    entity.IsDeleted = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdatePainAssessment(entity); 
                }
                return true;
            }
            return false;
        }
    }
}