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
    public partial class NursingNotes : BasePagePatientPageList
    {
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (menuType == "fo")
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    return Constant.MenuCode.EmergencyCare.FOLLOWUP_NURSING_NOTE;
                if (hdnDepartmentID.Value == Constant.Facility.INPATIENT) 
                    return Constant.MenuCode.Inpatient.FOLLOWUP_PATIENT_PAGE_NURSING_NOTE;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                    return Constant.MenuCode.MedicalDiagnostic.FOLLOWUP_PATIENT_NURSING_NOTES;
                else return Constant.MenuCode.Outpatient.FOLLOWUP_NURSING_NOTE;
            }
            else if (menuType == "dp")
            {
                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                    return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_NURSING_NOTES;
                else if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    return Constant.MenuCode.EmergencyCare.DATA_PATIENT_PATIENT_NURSING_NOTES;
                else return Constant.MenuCode.Outpatient.DATA_PATIENT_PATIENT_NURSING_NOTES;
            }
            else
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY)
                    if (menuType == "pe")
                        return Constant.MenuCode.EmergencyCare.ASSESMENT_NURSING_NOTE;
                    else
                        return Constant.MenuCode.EmergencyCare.NURSING_NOTE;
                else if (hdnDepartmentID.Value == Constant.Facility.INPATIENT) return Constant.MenuCode.Inpatient.PATIENT_PAGE_NURSING_NOTE;
                else if (hdnDepartmentID.Value == Constant.Facility.DIAGNOSTIC)
                {
                    if (menuType == "tr") return Constant.MenuCode.MedicalDiagnostic.NURSING_NOTE_TRANSACTION;
                    else if (menuType == "md") return Constant.MenuCode.MedicalDiagnostic.PATIENT_NURSING_NOTES;
                    else if (menuType == "nt") return Constant.MenuCode.Nutrition.NUTRITION_INTEGRATED_NOTES;
                    else return Constant.MenuCode.MedicalDiagnostic.PATIENT_PAGE_PARAMEDIC_NOTE;
                }
                else if (hdnDepartmentID.Value == Constant.Facility.LABORATORY) return Constant.MenuCode.Laboratory.PATIENT_NURSING_NOTES;
                else if (hdnDepartmentID.Value == Constant.Facility.IMAGING) return Constant.MenuCode.Imaging.PATIENT_NURSING_NOTES;
                else if (hdnDepartmentID.Value == Constant.Module.RADIOTHERAPHY) return Constant.MenuCode.Radiotheraphy.PATIENT_PAGE_RT_INTEGRATION_NOTES;
                else return Constant.MenuCode.Outpatient.NURSING_NOTE;
            }
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
                if (param.Length > 1)
                {
                    hdnDepartmentID.Value = param[0];
                    menuType = param[1];
                }
                else
                {
                    hdnDepartmentID.Value = Page.Request.QueryString["id"];
                }
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnBusinessPartnerID.Value = AppSession.RegisteredPatient.BusinessPartnerID.ToString();

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
                , new Variable() { Code = "Catatan Farmasi", Value = "5" }};
            Methods.SetComboBoxField(cboDisplay, lstDisplay, "Code", "Value");
            cboDisplay.Value = "0";
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            List<Registration> dataRegID = BusinessLayer.GetRegistrationList(string.Format("RegistrationID IN (SELECT RegistrationID FROM Registration WHERE LinkedToRegistrationID = {0}) OR RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
            string lstReg = "";
            if (dataRegID != null)
            {
                foreach (Registration reg in dataRegID)
                {
                    if (lstReg != "")
                    {
                        lstReg += ",";
                    }
                    lstReg += reg.RegistrationID;
                }
            }

            string filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType NOT IN ('{1}','{2}','{3}') AND (NoteDate BETWEEN '{4}' AND '{5}')", lstReg, Constant.PatientVisitNotes.REGISTRATION_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES, Constant.PatientVisitNotes.NURSE_NOTES, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            if (cboDisplay.Value.ToString() == "1")
            {
                filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND GCParamedicMasterType = '{3}'", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT, AppSession.UserLogin.GCParamedicMasterType);
            }
            else if (cboDisplay.Value.ToString() == "2")
            {
                filterExpression = string.Format("RegistrationID IN ({0}) AND GCPatientNoteType IN ('{1}','{2}') AND ParamedicID = {3}", lstReg, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT, AppSession.UserLogin.ParamedicID);
            }
            else if (cboDisplay.Value.ToString() == "3")
            {
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician);
            }
            else if (cboDisplay.Value.ToString() == "4")
            {
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Nutritionist);
            }
            else if (cboDisplay.Value.ToString() == "4")
            {
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Pharmacist);
            }

            if (filterExpression != "")
            {
                filterExpression += " AND ";
            }
            filterExpression += "IsDeleted = 0";

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
            url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/NursingNotesCtl.ascx");
            queryString = "";
            popupWidth = 900;
            popupHeight = 600;
            popupHeaderText = "Catatan Terintegrasi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/TransactionPage/NursingNotes/NursingNotesCtl.ascx");
                queryString = hdnID.Value;
                popupWidth = 900;
                popupHeight = 600;
                popupHeaderText = "Catatan Terintegrasi";
                return true;
            }
            return false;
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vPatientVisitNote entity = e.Row.DataItem as vPatientVisitNote;
                HtmlInputButton btnVerified = e.Row.FindControl("btnVerify") as HtmlInputButton;
                HtmlControl divVerifiedInfo = e.Row.FindControl("divVerifiedInformation") as HtmlControl;
                HtmlControl divPhysicianVerifiedInfo = e.Row.FindControl("divPhysicianVerifiedInformation") as HtmlControl;
                HtmlControl divNursingNotesInfo = (HtmlControl)e.Row.FindControl("divNursingNotesInfo");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divNotificationInfo = (HtmlControl)e.Row.FindControl("divNotificationInfo");
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;
                HtmlControl divViewDetail = e.Row.FindControl("divViewDetail") as HtmlControl;
                HtmlControl divViewDetail2 = e.Row.FindControl("divViewDetail2") as HtmlControl;

                if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                {
                    if (!entity.IsWrite)
                    {
                        divNursingNotesInfo.Style.Add("display", "none");
                    }

                    if (!entity.IsConfirmed)
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "block");
                            divNursingNotesInfo.Style.Add("display", "block");
                    }
                }
                else
                {
                    divNursingNotesInfo.Style.Add("display", "none");
                    divConfirmationInfo.Style.Add("display", "none");
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

                if (AppSession.UserLogin.IsPrimaryNurse)
                {
                    if (!entity.IsVerifiedByPrimaryNurse && entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                    {
                        btnVerified.Visible = true;
                        divVerifiedInfo.Visible = false;
                    }
                    else
                    {
                        btnVerified.Visible = false;
                        if (entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSING_NOTES)
                        {
                            divVerifiedInfo.Visible = true;
                        }
                        else
                        {
                            divVerifiedInfo.Visible = false;
                        }
                    }
                }
                else
                {
                    btnVerified.Visible = false;
                    divVerifiedInfo.Visible = false;
                }

                if (divSignature != null)
                {
                   divSignature.Visible = entity.ParamedicID == AppSession.UserLogin.ParamedicID;
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
                    divViewDetail.Visible = (entity.cfIsPostSurgeryInstruction);
                }

                if (divViewDetail2 != null)
                {
                    divViewDetail2.Visible = (entity.cfIsNutritionAssessment);
                }

                if (entity.IsNeedNotification)
                {
                    if (entity.cfIsNotificationConfirmed)
                    {
                        if (divNotificationInfo != null)
                            divNotificationInfo.Style.Add("display", "block");
                    }
                    else
                    {
                        if (divNotificationInfo != null)
                            divNotificationInfo.Style.Add("display", "none");
                    }
                }
                else
                {
                    if (divNotificationInfo != null)
                        divNotificationInfo.Style.Add("display", "none");
                }
            }
        }

        protected void cbpVerify_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = VerifyNursingNote(param);
                string[] retValInfo = retVal.Split('|');
                if (retValInfo[0] == "1")
                    result += string.Format("success|{0}", string.Empty);
                else
                    result += string.Format("fail|{0}", retValInfo[1]);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = param;
        }

        private string VerifyNursingNote(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNote(id);
                if (oVisitNote != null)
                {
                    if (!oVisitNote.IsVerifiedByPrimaryNurse)
                    {
                        if (AppSession.UserLogin.ParamedicID != null)
                        {
                            oVisitNote.IsVerifiedByPrimaryNurse = true;
                            oVisitNote.PrimaryNurseID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oVisitNote.PrimaryNurseVerifiedDateTime = DateTime.Now;
                        }
                        BusinessLayer.UpdatePatientVisitNote(oVisitNote);

                        result = string.Format("1|{0}", string.Empty);
                    }
                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
            }
            return result;
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
    }
}