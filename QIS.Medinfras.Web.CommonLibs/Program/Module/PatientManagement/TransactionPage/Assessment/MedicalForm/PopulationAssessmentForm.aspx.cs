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
    public partial class PopulationAssessmentForm : BasePagePatientPageList
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
                        return Constant.MenuCode.Inpatient.FOLLOWUP_POPULATION_ASSESSMENT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_POPULATION_ASSESSMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.FOLLOWUP_POPULATION_ASSESSMENT;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_POPULATION_ASSESSMENT;
                    default:
                        return Constant.MenuCode.Outpatient.FOLLOWUP_POPULATION_ASSESSMENT;
                }
                #endregion
            }
            else if (menuType == "dp")
            {
                #region Data Pemeriksaan Pasien
                switch (deptType)
                {
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_POPULATION_ASSESSMENT;
                    case Constant.Facility.MEDICAL_CHECKUP:
                        return Constant.MenuCode.MedicalCheckup.DATA_PATIENT_PATIENT_PAGE_POPULATION_ASSESSMENT;
                    default:
                        return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_POPULATION_ASSESSMENT;
                }
                #endregion
            }
            else
            {
                #region Pasien Dalam Perawatan
                switch (deptType)
                {
                    case Constant.Facility.INPATIENT:
                        return Constant.MenuCode.Inpatient.POPULATION_ASSESSMENT;
                    case Constant.Facility.OUTPATIENT:
                        return Constant.MenuCode.Outpatient.POPULATION_ASSESSMENT;
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EmergencyCare.POPULATION_ASSESSMENT;
                    case Constant.Facility.DIAGNOSTIC:
                        return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_POPULATION_ASSESSMENT;
                    case Constant.Module.RADIOTHERAPHY:
                        return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_POPULATION_ASSESSMENT;
                    default: return Constant.MenuCode.Inpatient.POPULATION_ASSESSMENT;
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

            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnMRN.Value = AppSession.RegisteredPatient.MRN.ToString();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnCurrentParamedicID.Value = AppSession.UserLogin.ParamedicID.ToString();

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
            string filterExpression = string.Format("ParentID = '{0}' AND IsActive = 1 AND IsDeleted = 0 ORDER BY StandardCodeID", Constant.StandardCode.PATIENT_ADDITIONAL_ASSESSMENT_FORM);
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

                List<vPopulationAssessment> lstPatient = BusinessLayer.GetvPopulationAssessmentList(string.Format("RegistrationID = '{0}' AND AssessmentID = '{1}' AND IsDeleted = 0 ORDER BY AssessmentID DESC", AppSession.RegisteredPatient.RegistrationID, hdnID.Value));
                if (lstPatient.Count > 0)
                {
                    string strhtml = "";
                    List<string> content = new List<string>();
                    string readString = string.Empty;
                    string path = HttpContext.Current.Server.MapPath("~/Libs/App_Data/medicalForm/Population");
                    string fileName = string.Format(@"{0}\PopulationPrint.html", path);

                    foreach (vPopulationAssessment row in lstPatient)
                    {
                        string chk1 = "";
                        string chk2 = "";
                        string FormLayout = row.AssessmentFormLayout.ToString();
                        string ParamValue = row.AssessmentFormValue.ToString();
                        string contentLayout = Helper.ParsingFormElektronik(FormLayout, ParamValue, string.Empty);
                        if (row.IsInitialAssessment == true)
                        {
                            chk1 = string.Format("checked");
                        }
                        IEnumerable<string> lstText = File.ReadAllLines(fileName);
                        StringBuilder innerHtml = new StringBuilder();
                        string readText = File.ReadAllText(fileName);
                        readString = Regex.Replace(readText, "#date", string.Format("{0} {1}<br/>", row.cfAssessmentDate, row.AssessmentTime));
                        readString = Regex.Replace(readString, "#medicalno", string.Format("{0}<br/>", hdnPageMedicalNo.Value));
                        readString = Regex.Replace(readString, "#patientname", string.Format("{0}<br/>", hdnPagePatientName.Value));
                        readString = Regex.Replace(readString, "#ppa", string.Format("{0}<br/>", row.ParamedicName));
                        readString = Regex.Replace(readString, "#assessment", string.Format("{0}<br/>", row.AssessmentType));
                        readString = Regex.Replace(readString, "#assessmenttext", string.Format("{0}<br/>", row.AssessmentFormText));
                        readString = Regex.Replace(readString, "#content", string.Format("{0}<br/>", contentLayout));
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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PopulationAssessmentFormEntry.ascx");
            queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", "1", hdnGCAssessmentType.Value, "0", AppSession.RegisteredPatient.VisitID, hdnPageMedicalNo.Value, hdnPagePatientName.Value, hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value); 
            popupWidth = 800;
            popupHeight = 600;
            popupHeaderText = string.Format("Pengkajian Populasi Khusus : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo,AppSession.RegisteredPatient.RegistrationNo);
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                PopulationAssessment entity = BusinessLayer.GetPopulationAssessment(Convert.ToInt32(hdnID.Value));
                if (entity.IsDeleted == true)
                {
                    errMessage = "Maaf, Pilih Data Pengkajian terlebih dahulu!";
                    return false;
                }
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/Assessment/MedicalForm/PopulationAssessmentFormEntry.ascx");
                queryString = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", "0", hdnGCAssessmentType.Value, hdnID.Value, AppSession.RegisteredPatient.VisitID, hdnPageMedicalNo.Value, hdnPagePatientName.Value, hdnPagePatientDOB.Value, hdnPageRegistrationNo.Value); 
                popupWidth = 800;
                popupHeight = 600;
                popupHeaderText = string.Format("Pengkajian Populasi Khusus : {0} (MRN = {1}, REG = {2})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo);
                return true;
            }
            errMessage = "Maaf, Pilih Data Pengkajian terlebih dahulu!";
            return false;
        }

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PopulationAssessment entity = BusinessLayer.GetPopulationAssessment(Convert.ToInt32(hdnID.Value));
                if (entity.IsDeleted == true)
                {
                    errMessage = "Maaf, Pilih Data Pengkajian terlebih dahulu!";
                    return false;
                }
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePopulationAssessment(entity);
                return true;
            }
            errMessage = "Maaf, Pilih Data Pengkajian terlebih dahulu!";
            return false;
        }
    }
}