using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Globalization;
using QIS.Data.Core.Dal;
using System.Web.UI.HtmlControls;
using DevExpress.Web.ASPxEditors;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ParamedicNotes : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string menuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSING_NOTE;
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.PHARMACY:
                    if (hdnSubMenuType.Value == Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_PHARMACIST_NOTES)
                        menuCode = Constant.MenuCode.Pharmacy.PHARMACIST_CLINICAL_PHARMACIST_NOTES;
                    else
                        menuCode = Constant.MenuCode.Pharmacy.PHARMACIST_NOTES;

                    break;
                case Constant.Facility.INPATIENT:
                    switch (hdnSubMenuType.Value)
                    {
                        case "fo":
                            menuCode = Constant.MenuCode.Inpatient.FOLLOWUP_INTEGRATION_NOTES_INPATIENT_NUTRITIONIST;
                            break;
                        default:
                            menuCode = Constant.MenuCode.Inpatient.PATIENT_PAGE_INTEGRATION_NOTES_INPATIENT_NUTRITIONIST;
                            break;
                    }
                    break;
                case Constant.Facility.OUTPATIENT:
                    menuCode = Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_PAGE_PARAMEDIC_NOTE;
                    break;
                case Constant.Facility.EMERGENCY:
                    menuCode = Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_PAGE_PARAMEDIC_NOTE;
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    switch (hdnSubMenuType.Value)
                    {
                        case "01":
                            menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_SURGERY_INTEGRATION_NOTES;
                            break;
                        case "NT":
                            menuCode = Constant.MenuCode.Nutrition.NUTRITION_INTEGRATED_NOTES;
                            break;
                        case "fo":
                            menuCode = Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_PAGE_PARAMEDIC_NOTE;
                            break;
                        default:
                            menuCode = Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_PARAMEDIC_NOTE;
                            break;
                    }
                    break;
                case Constant.Facility.LABORATORY:
                    menuCode = Constant.MenuCode.Laboratory.PATIENT_PAGE_PARAMEDIC_NOTE;
                    break;
                default:
                    break;
            }
            return menuCode;
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowDelete = false;
        }

        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                string[] param = Page.Request.QueryString["id"].Split('|');
                hdnDepartmentID.Value = param[0];
                hdnSubMenuType.Value = param[1];
                if (param.Length > 1)
                {
                    hdnSubMenuType.Value = param[1];
                }
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            SetComboBox();

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            BindGridView(1, true, ref PageCount);
        }

        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Catatan Semua PPA", Value = "0" }
                , new Variable() { Code = "Catatan Satu Profesi", Value = "1" }
                , new Variable() { Code = "Catatan Saya", Value = "2" }
                , new Variable() { Code = "Catatan Dokter", Value = "3" }
                , new Variable() { Code = "Catatan Ahli Gizi", Value = "4" }
            };
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            string notesType = Constant.PatientVisitNotes.NURSING_NOTES;

            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.PHARMACY:
                    notesType = Constant.PatientVisitNotes.PHARMACY_NOTES;
                    break;
                case Constant.Facility.DIAGNOSTIC:
                    notesType = Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES;
                    break;
                default:
                    break;
            }

            string filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType NOT IN ('{2}','{3}') AND (NoteDate BETWEEN '{4}' AND '{5}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID, Constant.PatientVisitNotes.REGISTRATION_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (cboDisplay.Value.ToString() == "1")
            {
                filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType = '{2}' AND GCParamedicMasterType = '{3}'", AppSession.RegisteredPatient.VisitID, cvLinkedID, notesType, AppSession.UserLogin.GCParamedicMasterType);
            }
            else if (cboDisplay.Value.ToString() == "2")
            {
                filterExpression = string.Format("VisitID IN ({0},{1}) AND GCPatientNoteType = '{2}' AND ParamedicID = {3}", AppSession.RegisteredPatient.VisitID, cvLinkedID, notesType, AppSession.UserLogin.ParamedicID);
            }
            else if (cboDisplay.Value.ToString() == "3")
            {
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician);
            }
            else if (cboDisplay.Value.ToString() == "4")
            {
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nutritionist);
            }

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC, NoteTime DESC");
            grdView.DataSource = lstEntity;
            grdView.DataBind();
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

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/IntegratedNotes/ParamedicNotesCtl.ascx");
            queryString = string.Format("{0}|{1}|{2}", hdnDepartmentID.Value, hdnDepartmentID.Value, "");
            popupWidth = 900;
            popupHeight = 500;
            popupHeaderText = hdnPageTitle.Value;
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/IntegratedNotes/ParamedicNotesCtl.ascx");
                queryString = string.Format("{0}|{1}|{2}", hdnDepartmentID.Value, hdnDepartmentID.Value, hdnID.Value);
                popupWidth = 900;
                popupHeight = 500;
                popupHeaderText = hdnPageTitle.Value;
                return true;
            }
            return false;
        }

        //protected override bool OnDeleteRecord(ref string errMessage)
        //{
        //    if (hdnID.Value != "")
        //    {
        //        NursingJournal entity = BusinessLayer.GetNursingJournal(Convert.ToInt32(hdnID.Value));
        //        entity.IsDeleted = true;
        //        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
        //        BusinessLayer.UpdateNursingJournal(entity);
        //        return true;
        //    }
        //    return false;
        //}

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientVisitNote entity = e.Row.DataItem as vPatientVisitNote;
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divPhysicianVerifiedInfo = e.Row.FindControl("divPhysicianVerifiedInformation") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;
                HtmlControl divViewDetail = e.Row.FindControl("divViewDetail") as HtmlControl;

                if (divSignature != null)
                    divSignature.Visible = entity.ParamedicID == AppSession.UserLogin.ParamedicID;

                if (entity.IsNeedConfirmation)
                {
                    if (!entity.IsConfirmed)
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "block");
                    }
                }
                else
                {
                    if (divConfirmationInfo != null)
                    {
                        divConfirmationInfo.Style.Add("display", "none");
                    }
                }

                if (!entity.IsVerified)
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = false;
                }
                else
                {
                    if (divPhysicianVerifiedInfo != null)
                        divPhysicianVerifiedInfo.Visible = true;
                }

                if (divView != null)
                {
                    divView.Visible = (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT ||
                        entity.GCPatientNoteType == Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT ||
                        entity.GCPatientNoteType == Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT ||
                        (entity.DepartmentID == Constant.Facility.OUTPATIENT && entity.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES) ||
                        ((entity.DepartmentID == Constant.Facility.DIAGNOSTIC || string.IsNullOrEmpty(entity.DepartmentID)) && entity.GCPatientNoteType == Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES));
                }

                if (divViewDetail != null)
                {
                    divViewDetail.Visible = (entity.cfIsPostSurgeryInstruction || entity.cfIsNutritionAssessment);
                }
            }
        }
    }
}