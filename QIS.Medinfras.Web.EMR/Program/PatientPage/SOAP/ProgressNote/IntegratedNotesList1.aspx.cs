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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IntegratedNotesList1 : BasePagePatientPageList
    {
        string menuType = string.Empty;
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            if (string.IsNullOrEmpty(menuType))
                menuType = Page.Request.QueryString["id"];

            switch (menuType)
            {
                case "OP":
                    return Constant.MenuCode.EMR.OUTPATIENT_SOAP_PROGRESS_NOTE_1;
                case "MD":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE;
                case "IP":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_2;
                case "ER":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_1;
                case "MC":
                    return Constant.MenuCode.EMR.MCU_SOAP_PROGRESS_NOTE_1;
                default:
                    switch (AppSession.RegisteredPatient.DepartmentID)
                    {
                        case Constant.Facility.OUTPATIENT:
                            return Constant.MenuCode.EMR.OUTPATIENT_SOAP_PROGRESS_NOTE_1;
                        case Constant.Facility.INPATIENT:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_2;
                        case Constant.Facility.DIAGNOSTIC:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE;
                        default:
                            return Constant.MenuCode.EMR.SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_1;
                    }
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
            string filterSetVar = string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')",
                                                        AppSession.UserLogin.HealthcareID, //0
                                                        Constant.SettingParameter.EM_DEFAULT_DAY_FILTER_CPPT //1
                                                    );
            List<SettingParameterDt> lstSetVarDt = BusinessLayer.GetSettingParameterDtList(filterSetVar);

            int oDefaultDay = 1;
            if (lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_DEFAULT_DAY_FILTER_CPPT).FirstOrDefault().ParameterValue != "")
            {
                oDefaultDay = Convert.ToInt32(lstSetVarDt.Where(a => a.ParameterCode == Constant.SettingParameter.EM_DEFAULT_DAY_FILTER_CPPT).FirstOrDefault().ParameterValue);
            }

            oDefaultDay = oDefaultDay * -1;

            if (Page.Request.QueryString["id"] != null)
            {
                menuType = Page.Request.QueryString["id"];
            }
            hdnCurrentSessionID.Value = AppSession.UserLogin.UserID.ToString();
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            SetComboBox();

            txtFromDate.Text = DateTime.Today.AddDays(oDefaultDay).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            hdnIsShowVerifyButton.Value = AppSession.IsUseVerifyAllButton ? "1" : "0";
            hdnIsDPJPPhysician.Value = AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID ? "1" : "0";
            if (hdnIsShowVerifyButton.Value == "1")
            {
                btnVerifyAll.Visible = AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID;
            }
            else
            {
                btnVerifyAll.Visible = false;
            }

            //Jika validasi Kajian Awal dan merupakan Dokter DPJP
            if (AppSession.EM0034 == "1" && hdnIsDPJPPhysician.Value == "1")
            {
                string filterExp = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
                ChiefComplaint oChiefComplaint = BusinessLayer.GetChiefComplaintList(filterExp).FirstOrDefault();
                hdnIsAllowAdd.Value = oChiefComplaint != null ? "1" : "0";
            }
            else
            {
                hdnIsAllowAdd.Value = "1";
            }

            if (!string.IsNullOrEmpty(AppSession.EM0058))
                hdnEM0058.Value = AppSession.EM0058; 
            else
                hdnEM0058.Value = "0"; 

            BindGridView(1, true, ref PageCount);
        }

        private void SetComboBox()
        {
            List<Variable> lstDisplay = new List<Variable>() { new Variable() { Code = "Catatan Semua PPA", Value = "0" }
                , new Variable() { Code = "Catatan Dokter", Value = "1" }
                , new Variable() { Code = "Catatan Saya", Value = "2" }
                , new Variable() { Code = "Catatan Perawat dan Tenaga Medis Lainnya", Value = "3" }
                , new Variable() { Code = "Yang perlu konfirmasi", Value = "4" }};
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

            string filterExpression = string.Format("RegistrationID IN ({0}) AND (NoteDate BETWEEN '{1}' AND '{2}') AND GCPatientNoteType NOT IN ('{3}','{4}') AND GCNoteType IS NULL", lstReg, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112), Constant.PatientVisitNotes.NURSE_NOTES, Constant.PatientVisitNotes.REFERRAL_FROM_NOTES);

            if (cboDisplay.Value.ToString() == "1")
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician);
            else if (cboDisplay.Value.ToString() == "2")
                filterExpression += string.Format(" AND GCParamedicMasterType = '{0}' AND ParamedicID = {1}", Constant.ParamedicType.Physician, AppSession.UserLogin.ParamedicID);
            else if (cboDisplay.Value.ToString() == "3")
                filterExpression += string.Format(" AND GCParamedicMasterType != '{0}'", Constant.ParamedicType.Physician);
            else if (cboDisplay.Value.ToString() == "4")
                filterExpression += string.Format(" AND IsNeedConfirmation = 1 AND ConfirmationPhysicianID = {0}", AppSession.UserLogin.ParamedicID);

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

            List<vPatientVisitNote> lstNotes = lstEntity.Where(lst => lst.IsNeedConfirmation && !lst.IsConfirmed && lst.ConfirmationPhysicianID == AppSession.UserLogin.ParamedicID).ToList();
            if (lstNotes.Count > 0)
                hdnIsHasNeedConfirmation.Value = "1";
            else
                hdnIsHasNeedConfirmation.Value = "0";
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
                    hdnPageIndex.Value = param[1];
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
            panel.JSProperties["cpRetval"] = hdnIsHasNeedConfirmation.Value;
        }

        protected override bool OnAddRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            url = ResolveUrl("~/Program/PatientPage/SOAP/ProgressNote/IntegratedNotesEntryCtl1.ascx");
            queryString = "";
            popupWidth = 1000;
            popupHeight = 600;
            popupHeaderText = "Catatan Terintegrasi";
            return true;
        }

        protected override bool OnEditRecord(ref string url, ref string errMessage, ref string queryString, ref int popupWidth, ref int popupHeight, ref string popupHeaderText)
        {
            if (hdnID.Value != "")
            {
                url = ResolveUrl("~/Program/PatientPage/SOAP/ProgressNote/IntegratedNotesEntryCtl1.ascx");
                queryString = hdnID.Value;
                popupWidth = 1000;
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
                HtmlControl divReadback = (HtmlControl)e.Row.FindControl("divReadback");
                HtmlInputControl btnReadback = (HtmlInputControl)e.Row.FindControl("btnReadback");
                HtmlInputControl btnVerified = (HtmlInputControl)e.Row.FindControl("btnVerified");
                HtmlInputControl btnNote1 = (HtmlInputControl)e.Row.FindControl("btnNote1");
                HtmlInputControl btnNote2 = (HtmlInputControl)e.Row.FindControl("btnNote2");
                HtmlControl divNursingNotesInfo = (HtmlControl)e.Row.FindControl("divNursingNotesInfo");
                HtmlControl divNeedConfirmation = (HtmlControl)e.Row.FindControl("divNeedConfirmation");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divVerifiedInformation = (HtmlControl)e.Row.FindControl("divVerifiedInformation");
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;
                HtmlControl divViewDetail = e.Row.FindControl("divViewDetail") as HtmlControl;
                HtmlControl divViewDetail2 = e.Row.FindControl("divViewDetail2") as HtmlControl;

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

                if (entity.IsNeedConfirmation)
                {
                    if (!entity.IsConfirmed)
                    {
                        if (entity.ConfirmationPhysicianID == AppSession.UserLogin.ParamedicID)
                        {
                            if (btnReadback != null)
                                btnReadback.Style.Add("display", "block");
                            if (btnNote1 != null)
                                btnNote1.Style.Add("display", "block");
                            if (btnNote2 != null)
                                btnNote2.Style.Add("display", "none");
                            if (divNeedConfirmation != null)
                                divNeedConfirmation.Style.Add("display", "none");
                        }
                        else
                        {
                            if (btnReadback != null)
                                btnReadback.Style.Add("display", "none");
                            if (btnNote1 != null)
                                btnNote1.Style.Add("display", "none");
                            if (divNeedConfirmation != null)
                                divNeedConfirmation.Style.Add("display", "block");
                            btnVerified.Visible = false;
                        }
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (btnReadback != null)
                            btnReadback.Style.Add("display", "none");
                        if (btnNote1 != null)
                            btnNote1.Style.Add("display", "none");
                        if (divNeedConfirmation != null)
                            divNeedConfirmation.Style.Add("display", "none");
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "block");
                    }
                }
                else
                {
                    if (btnReadback != null)
                        btnReadback.Style.Add("display", "none");
                    if (btnNote1 != null)
                        btnNote1.Style.Add("display", "none");
                    if (divNeedConfirmation != null)
                        divNeedConfirmation.Style.Add("display", "none");

                    if (divConfirmationInfo != null)
                    {
                        divConfirmationInfo.Style.Add("display", "none");
                    }
                }

                if (!entity.IsVerified)
                {
                    if (AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID)
                    {
                        if (entity.IsNeedConfirmation && !entity.IsConfirmed && entity.ConfirmationPhysicianID == AppSession.UserLogin.ParamedicID)
                        {
                            btnReadback.Value = "Confirm and Verify";
                            btnReadback.Style.Add("width", "150px");
                            btnNote2.Visible = true;
                            btnVerified.Visible = false;
                        }
                        else
                        {
                            if (entity.IsNeedConfirmation && !entity.IsConfirmed)
                            {
                                btnNote2.Visible = false;
                                btnVerified.Visible = false;
                            }
                            else
                            {
                                btnReadback.Visible = false;
                                btnNote1.Visible = false;
                                btnNote2.Visible = true;
                                btnVerified.Visible = true;
                            }
                        }
                        if (divVerifiedInformation != null)
                            divVerifiedInformation.Visible = false;
                    }
                    else
                    {
                        if (btnVerified != null)
                            btnVerified.Visible = false;
                        if (btnNote2 != null)
                            btnNote2.Visible = false;
                        if (divVerifiedInformation != null)
                            divVerifiedInformation.Visible = false;
                    }
                }
                else
                {
                    if (btnNote2 != null)
                        btnNote2.Visible = false;
                    if (divVerifiedInformation != null)
                        divVerifiedInformation.Visible = true;
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
            }
        }

        protected void cbpReadback_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = ConfirmPlanningNote(param);
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

        protected void cbpVerify_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string param = e.Parameter;
            try
            {
                string retVal = VerifyPlanningNote(param);
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

        private string ConfirmPlanningNote(string recordID)
        {
            string result = string.Empty;

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                PatientVisitNote oVisitNote = BusinessLayer.GetPatientVisitNote(id);
                if (oVisitNote != null)
                {
                    if (oVisitNote.ConfirmationPhysicianID == AppSession.RegisteredPatient.ParamedicID)
                    {
                        //Jika DPJP sekalian verified
                        oVisitNote.IsVerified = true;
                        oVisitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        oVisitNote.VerifiedDateTime = DateTime.Now;
                    }
                    oVisitNote.IsConfirmed = true;
                    oVisitNote.ConfirmationDateTime = DateTime.Now;
                    BusinessLayer.UpdatePatientVisitNote(oVisitNote);
                }

                result = string.Format("1|{0}", string.Empty);
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

        private string VerifyPlanningNote(string recordID)
        {
            string result = "0|";

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao oVisitNoteDao = new PatientVisitNoteDao(ctx);

            try
            {
                //Confirm
                int id = Convert.ToInt32(recordID);
                bool isValid = true;
                bool isNurseInitialAssessment = false;
                int nurseChiefComplaintID = 0;
                PatientVisitNote oVisitNote = oVisitNoteDao.Get(id);
                if (oVisitNote != null)
                {
                    if (oVisitNote.IsNeedConfirmation && !oVisitNote.IsConfirmed && oVisitNote.ConfirmationPhysicianID != AppSession.UserLogin.ParamedicID)
                    {
                        result = string.Format("0|{0}", "Catatan Terintegrasi belum di-readback oleh Dokter yang dituju!");
                        isValid = false;
                    }
                    else
                    {
                        if (oVisitNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            NurseChiefComplaint oNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("PatientVisitNoteID = {0} ORDER BY ID DESC", oVisitNote.ID), ctx).FirstOrDefault();
                            if (oNurseChiefComplaint != null)
                            {
                                if (oNurseChiefComplaint.GCAssessmentStatus == Constant.AssessmentStatus.OPEN)
                                {
                                    result = string.Format("0|{0}", "Proses kajian awal perawat belum selesai dilakukan!");
                                    isValid = false;
                                }
                                else
                                {
                                    nurseChiefComplaintID = oNurseChiefComplaint.ID;
                                    isNurseInitialAssessment = true;
                                }
                            }
                            else
                            {
                                nurseChiefComplaintID = 0;
                                isNurseInitialAssessment = false;
                            }
                        }
                    }

                    if (isValid)
                    {
                        if (AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID)
                        {
                            //Jika DPJP sekalian verified
                            oVisitNote.IsVerified = true;
                            oVisitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            oVisitNote.VerifiedDateTime = DateTime.Now;
                            oVisitNoteDao.UpdateWithoutChangeLastUpdatedDate(oVisitNote);

                            if (isNurseInitialAssessment)
                            {
                                NurseChiefComplaintDao oNurseChiefComplaintDao = new NurseChiefComplaintDao(ctx);
                                NurseChiefComplaint oNurseChiefComplaint = oNurseChiefComplaintDao.Get(nurseChiefComplaintID);
                                if (oNurseChiefComplaint != null)
                                {
                                    oNurseChiefComplaint.IsVerifiedByRegisteredPhysician = true;
                                    oNurseChiefComplaint.RegisteredPhysicianID = AppSession.UserLogin.ParamedicID;
                                    oNurseChiefComplaint.RegisteredPhysicianVerifiedDateTime = DateTime.Now;
                                    oNurseChiefComplaintDao.Update(oNurseChiefComplaint);
                                }
                            }

                            ctx.CommitTransaction();
                            result = string.Format("1|{0}", string.Empty);
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                            result = string.Format("0|{0}", "Hanya DPJP Utama yang bisa melakukan proses verifikasi");
                        }
                    }
                    else
                    {
                        ctx.RollBackTransaction();
                    }
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = string.Format("0|{0}", ex.Message);
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        #region Verify All
        protected void cbpVerifyAll_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;

            result = VerifyAllPatientNote(param[0]);

            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = result;
        }

        private string VerifyAllPatientNote(string type)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao visitNoteDao = new PatientVisitNoteDao(ctx);
            try
            {
                if (type == "verify")
                {
                    //Confirm
                    string filterExpression = string.Empty;
                    vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                    int cvLinkedID = 0;
                    int totalError = 0;
                    bool isNurseInitialAssessment = false;
                    int nurseChiefComplaintID = 0;
                    if (entityLinkedRegistration != null)
                    {
                        cvLinkedID = entityLinkedRegistration.VisitID;
                    }

                    if (filterExpression != "")
                        filterExpression += " AND ";
                    filterExpression += string.Format("VisitID IN ('{0}','{1}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);
                    filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND (IsVerified = 0 OR IsVerified IS NULL)", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);

                    List<PatientVisitNote> lstVisitNote = BusinessLayer.GetPatientVisitNoteList(filterExpression, ctx);
                    foreach (PatientVisitNote visitNote in lstVisitNote)
                    {
                        bool isValid = true;
                        if (visitNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            NurseChiefComplaint oNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("PatientVisitNoteID = {0}", visitNote.ID), ctx).FirstOrDefault();
                            if (oNurseChiefComplaint != null)
                            {
                                if (oNurseChiefComplaint.GCAssessmentStatus == Constant.AssessmentStatus.OPEN)
                                {
                                    totalError += 1;
                                    isValid = false;
                                }
                                else
                                {
                                    nurseChiefComplaintID = oNurseChiefComplaint.ID;
                                    isNurseInitialAssessment = true;
                                }
                            }
                        }
                        else
                        {
                            if ((visitNote.IsNeedConfirmation && !visitNote.IsConfirmed))
                                isValid = false;
                        }

                        if (isValid)
                        {
                            visitNote.IsVerified = true;
                            visitNote.VerifiedBy = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            visitNote.VerifiedDateTime = DateTime.Now;
                            visitNoteDao.UpdateWithoutChangeLastUpdatedDate(visitNote);

                            if (isNurseInitialAssessment)
                            {
                                NurseChiefComplaintDao oNurseChiefComplaintDao = new NurseChiefComplaintDao(ctx);
                                NurseChiefComplaint oNurseChiefComplaint = oNurseChiefComplaintDao.Get(nurseChiefComplaintID);
                                if (oNurseChiefComplaint != null)
                                {
                                    oNurseChiefComplaint.IsVerifiedByRegisteredPhysician = true;
                                    oNurseChiefComplaint.RegisteredPhysicianID = AppSession.UserLogin.ParamedicID;
                                    oNurseChiefComplaint.RegisteredPhysicianVerifiedDateTime = DateTime.Now;
                                    oNurseChiefComplaintDao.Update(oNurseChiefComplaint);
                                }
                            }
                        }
                    }
                    if (totalError > 0)
                    {
                        result = string.Format("1|Ada beberapa catatan yang gagal diverifikasi!", string.Empty);
                    }
                    else
                    {
                        result = string.Format("1|{0}", string.Empty);
                    }

                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }
        #endregion
    }
}