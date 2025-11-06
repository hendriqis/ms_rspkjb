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
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Web.UI.HtmlControls;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ERProgressNote1 : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected int gridInstructionPageCount = 1;

        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.EMERGENCY_SOAP_PROGRESS_ASSESSMENT_1;
        }

        #region List
        protected override void InitializeDataControl()
        {
            vConsultVisit1 entityVisit = BusinessLayer.GetvConsultVisit1List(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3}, DOB = {4})", entityVisit.cfPatientNameInLabel, entityVisit.MedicalNo, entityVisit.RegistrationNo, entityVisit.ServiceUnitName, entityVisit.DateOfBirthInString);

            hdnDatePickerToday.Value = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            hdnTimeToday.Value = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
            if (AppSession.UserLogin.IsSpecialist)
                tblNotesInstruction.Style.Add("display", "none");           
            else
                tblNotesInstruction.Style.Add("display", "table");

            SetControlProperties();

            BindGridView(1, true, ref PageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);
            BindGridViewInstruction(1, true, ref gridInstructionPageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = hdnFilterExpression.Value;

            string code = ddlViewType.SelectedValue;

            vConsultVisit4 entityLinkedRegistration = BusinessLayer.GetvConsultVisit4List(string.Format("RegistrationID = (SELECT LinkedRegistrationID FROM Registration WHERE RegistrationID = {0})", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
            int cvLinkedID = 0;
            if (entityLinkedRegistration != null)
            {
                cvLinkedID = entityLinkedRegistration.VisitID;
            }

            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID IN ('{0}','{1}') AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, cvLinkedID);

            if (code == "0")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);
            else if (code == "1")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}')",Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT);
            else if (code == "2")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);
            else
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}') AND ParamedicID = {3}", Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, AppSession.UserLogin.ParamedicID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientVisitNoteRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientVisitNote> lstEntity = BusinessLayer.GetvPatientVisitNoteList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "NoteDate DESC,NoteTime DESC");
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

        protected override bool OnDeleteRecord(ref string errMessage)
        {
            if (hdnID.Value != "")
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.Now;
                BusinessLayer.UpdatePatientVisitNote(entity);
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
                HtmlControl divNeedConfirmation = (HtmlControl)e.Row.FindControl("divNeedConfirmation");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");

                if (entity.IsNeedConfirmation)
                {
                    if (!entity.IsConfirmed)
                    {
                        if (entity.ConfirmationPhysicianID == AppSession.UserLogin.ParamedicID)
                        {
                            if (btnReadback != null)
                                btnReadback.Style.Add("display", "block");
                            if (divNeedConfirmation != null)
                                divNeedConfirmation.Style.Add("display", "none");
                        }
                        else
                        {
                            if (btnReadback != null)
                                btnReadback.Style.Add("display", "none");
                            if (divNeedConfirmation != null)
                                divNeedConfirmation.Style.Add("display", "block");
                        }
                        if (divConfirmationInfo != null)
                            divConfirmationInfo.Style.Add("display", "none");
                    }
                    else
                    {
                        if (btnReadback != null)
                            btnReadback.Style.Add("display", "none");
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
                    if (divNeedConfirmation != null)
                        divNeedConfirmation.Style.Add("display", "none");

                    if (divConfirmationInfo != null)
                    {
                        divConfirmationInfo.Style.Add("display", "none");
                    }
                }
            }
        }
        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            string filterExpression = string.Format("ParentID IN ('{0}','{1}')", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE, Constant.StandardCode.PATIENT_INSTRUCTION_GROUP);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE).ToList(), "StandardCodeName", "StandardCodeID");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboSpecialistPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField(cboInstructionType, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP).ToList(), "StandardCodeName", "StandardCodeID");

            string defaultInstruction = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.PATIENT_INSTRUCTION_GROUP && sc.IsDefault == true).FirstOrDefault().StandardCodeID;
            if (!string.IsNullOrEmpty(defaultInstruction))
            {
                cboInstructionType.Value = defaultInstruction;
            }

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtNoteDate, new ControlEntrySetting(true, true, true, DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtNoteTime, new ControlEntrySetting(true, true, true, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT)));
            SetControlEntrySetting(cboPhysician, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtSubjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtObjectiveText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtAssessmentText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPlanningText, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtInstructionSummary, new ControlEntrySetting(true, true, false));
            SetControlEntrySetting(txtInstructionText, new ControlEntrySetting(true, true, true));
        }

        private void ControlToEntity(PatientVisitNote entity)
        {
            entity.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
            entity.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entity.NoteTime = txtNoteTime.Text;
            entity.ParamedicID = Convert.ToInt32(cboPhysician.Value);
            entity.HealthcareServiceUnitID = AppSession.RegisteredPatient.HealthcareServiceUnitID;
            entity.SubjectiveText = txtSubjectiveText.Text;
            entity.ObjectiveText = txtObjectiveText.Text;
            entity.AssessmentText = txtAssessmentText.Text;
            entity.PlanningText = txtPlanningText.Text;
            //entity.InstructionText = hdnInstructionText.Value;
            entity.InstructionText = txtInstructionSummary.Text;
            if (!string.IsNullOrEmpty(txtInstructionSummary.Text))
            {
                entity.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}I:{12}{13}",
            Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
            Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
            Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
            Environment.NewLine, txtPlanningText.Text, Environment.NewLine,
            Environment.NewLine, txtInstructionSummary.Text);
            }
            else
            {
                entity.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}",
           Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
           Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
           Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
           Environment.NewLine, txtPlanningText.Text, Environment.NewLine);
            }

            if (hdnPlanningNoteID.Value != "" && hdnPlanningNoteID.Value != "0")
                entity.LinkedNoteID = Convert.ToInt32(hdnPlanningNoteID.Value);
            if (cboPhysicianInstructionSource.Value != null)
                entity.GCPhysicianInstructionSource = cboPhysicianInstructionSource.Value.ToString();
            if (chkIsNeedConfirmation.Checked)
            {
                entity.IsNeedConfirmation = chkIsNeedConfirmation.Checked;
                if (hdnParamedicID.Value != "" && hdnParamedicID.Value != "0")
                    entity.ConfirmationPhysicianID = Convert.ToInt32(hdnParamedicID.Value);
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            try
            {
                PatientVisitNote entity = new PatientVisitNote();
                ControlToEntity(entity);
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.InsertPatientVisitNote(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            try
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientVisitNote(entity);
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }
        #endregion

        #region Vital Sign
        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            filterExpression += string.Format("VisitID = {0} AND IsDischargeVitalSign = 0 AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvVitalSignHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vVitalSignHd> lstEntity = BusinessLayer.GetvVitalSignHdList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
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

        protected void cbpDeleteVitalSign_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnVitalSignRecordID.Value != "")
            {
                VitalSignHd entity = BusinessLayer.GetVitalSignHd(Convert.ToInt32(hdnVitalSignRecordID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateVitalSignHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

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
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvReviewOfSystemHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vReviewOfSystemHd> lstEntity = BusinessLayer.GetvReviewOfSystemHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "ID DESC");
            lstReviewOfSystemDt = BusinessLayer.GetvReviewOfSystemDtList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID));
            grdROSView.DataSource = lstEntity;
            grdROSView.DataBind();
        }

        protected void cbpDeleteROS_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnReviewOfSystemID.Value != "")
            {
                ReviewOfSystemHd entity = BusinessLayer.GetReviewOfSystemHd(Convert.ToInt32(hdnReviewOfSystemID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateReviewOfSystemHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Laboratory
        private void BindGridViewLaboratory(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.LABORATORY_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnLaboratoryOrderSummaryText.Value = sbText.ToString();

            grdLaboratoryView.DataSource = lstEntity;
            grdLaboratoryView.DataBind();
        }

        protected void cbpLaboratoryView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewLaboratory(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewLaboratory(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnLaboratoryOrderSummaryText.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnLaboratoryOrderSummaryText.Value;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = obj.cfTestOrderDetailList;
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0 ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND TransactionCode = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionCode.IMAGING_TEST_ORDER, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnImagingSummary.Value = sbText.ToString();

            grdImagingView.DataSource = lstEntity;
            grdImagingView.DataBind();
        }

        protected void cbpImagingView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewImaging(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewImaging(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnImagingSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnImagingSummary.Value;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                List<vTestOrderDt> lstOrderDt = (List<vTestOrderDt>)GetTestOrderDt(obj.TestOrderID);
                rptImagingDt.DataSource = lstOrderDt;
                rptImagingDt.DataBind();


                StringBuilder sbOrderInfo = new StringBuilder();
                foreach (vTestOrderDt item in lstOrderDt)
                {
                    string remarks = String.IsNullOrEmpty(item.Remarks) ? string.Empty : "(" + item.Remarks + ")";
                    sbOrderInfo.AppendLine(string.Format("{0} {1}", item.ItemName1, remarks));
                }
                hdnImagingSummaryText.Value = sbOrderInfo.ToString();
            }
        }
        #endregion

        #region Diagnostic
        private void BindGridViewDiagnostic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID NOT IN ({1},{2}) AND GCTransactionStatus != '{3}'", AppSession.RegisteredPatient.VisitID, hdnLaboratoryServiceUnitID.Value, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHd1RowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd1> lstEntity = BusinessLayer.GetvTestOrderHd1List(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            StringBuilder sbText = new StringBuilder();
            foreach (vTestOrderHd1 item in lstEntity)
            {
                foreach (CompactTestOrderDtInfo detail in item.cfTestOrderDetailList)
                {
                    sbText.AppendLine(string.Format("- {0}", detail.ItemName1));
                }
            }
            hdnDiagnosticSummary.Value = sbText.ToString();

            grdDiagnosticView.DataSource = lstEntity;
            grdDiagnosticView.DataBind();
        }

        protected void grdDiagnosticView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd1 obj = (vTestOrderHd1)e.Row.DataItem;
                Repeater rptDiagnosticDt = (Repeater)e.Row.FindControl("rptDiagnosticDt");
                rptDiagnosticDt.DataSource = obj.cfTestOrderDetailList;
                rptDiagnosticDt.DataBind();
            }
        }

        protected void cbpDiagnosticView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnostic(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnostic(1, true, ref pageCount);
                    result = "refresh|" + pageCount + '|' + hdnDiagnosticSummary.Value;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpSummary"] = hdnDiagnosticSummary.Value;
        }
        #endregion

        #region Instruction
        private void BindGridViewInstruction(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, hdnPatientVisitNoteID.Value);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientInstructionRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientInstruction> lstEntity = BusinessLayer.GetvPatientInstructionList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "PatientInstructionID");

            if (!string.IsNullOrEmpty(hdnPatientVisitNoteID.Value))
            {
                StringBuilder instructionText = new StringBuilder();
                foreach (vPatientInstruction obj in lstEntity)
                {
                    if (obj.PatientVisitNoteID == Convert.ToInt32(hdnPatientVisitNoteID.Value) || (hdnPatientVisitNoteID.Value == "0" && obj.PhysicianID == AppSession.UserLogin.ParamedicID && obj.InstructionDate == Helper.GetDatePickerValue(txtNoteDate)))
                    {
                        instructionText.AppendLine(string.Format("{0} {1} {2}", obj.cfInstructionDate, obj.InstructionTime, obj.Description));
                    }
                }

                hdnInstructionText.Value += string.Format("{0}{1}{2}", hdnInstructionText.Value, Environment.NewLine, instructionText.ToString());
                txtInstructionSummary.Text = hdnInstructionText.Value.TrimEnd();
            }

            grdInstructionView.DataSource = lstEntity;
            grdInstructionView.DataBind();
        }

        protected void cbpInstructionView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewInstruction(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewInstruction(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = "";
        }
        protected void cbpInstruction_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientInstruction entity = new PatientInstruction();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.InstructionDate = DateTime.Now.Date;
                        entity.InstructionTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

                        entity.PhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                        entity.Description = txtInstructionText.Text;

                        if (!string.IsNullOrEmpty(hdnPatientVisitNoteID.Value))
                        {
                            entity.PatientVisitNoteID = Convert.ToInt32(hdnPatientVisitNoteID.Value);
                        }

                        BusinessLayer.InsertPatientInstruction(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            entity.GCInstructionGroup = cboInstructionType.Value.ToString();
                            entity.Description = txtInstructionText.Text;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Instruction Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnInstructionID.Value);
                        PatientInstruction entity = BusinessLayer.GetPatientInstruction(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientInstruction(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Instruction Record Information");
                        }
                        result = "1|delete|";
                    }

                }

            }
            catch (Exception ex)
            {
                result = string.Format("0|process|{0}", ex.Message);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        protected void cbpDeleteTestOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (e.Parameter != null && e.Parameter != "")
            {
                string testOrderID = "0";

                switch (e.Parameter)
                {
                    case "LB":
                        testOrderID = hdnLaboratoryTestOrderID.Value;
                        break;
                    case "IS":
                        testOrderID = hdnImagingTestOrderID.Value;
                        break;
                    case "MD":
                        testOrderID = hdnDiagnosticTestOrderID.Value;
                        break;
                    default:
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}", e.Parameter);
                }
                else
                {
                    result = string.Format("0|{0}|There is no record to be deleted !");
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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

        protected void cbpSendOrder_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string[] param = e.Parameter.Split('|');
            int transactionID = Convert.ToInt32(param[2]);
            result = param[0] + "|" + param[1] + "|";
            try
            {
                if (param[0] == "sendOrder")
                {
                    if (param[1] != "PH")
                    {
                        TestOrderHd entity = BusinessLayer.GetTestOrderHdList(String.Format("TestOrderID = {0}", transactionID))[0];
                        if (entity.GCTransactionStatus != Constant.TransactionStatus.CLOSED)
                        {
                            entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                            BusinessLayer.UpdateTestOrderHd(entity);
                            try
                            {
                                HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                                string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                                if (!String.IsNullOrEmpty(ipAddress))
                                {
                                    SendNotification(entity, ipAddress);
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                        }
                    }
                }
                result += string.Format("success|{0}", errMessage);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = transactionID;
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

        private void SendNotification(TestOrderHd order, string ipAddress)
        {
            StringBuilder sbMessage = new StringBuilder();
            sbMessage.AppendLine(string.Format("No  : {0}", order.TestOrderNo));
            sbMessage.AppendLine(string.Format("Fr  : {0}", string.Format("{0} ({1})", AppSession.RegisteredPatient.ServiceUnitName, AppSession.UserLogin.UserFullName)));
            sbMessage.AppendLine(string.Format("Px  : {0}", AppSession.RegisteredPatient.PatientName));
            sbMessage.AppendLine(string.Format("PDx :    "));
            sbMessage.AppendLine(string.Format("{0}", order.Remarks));
            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipAddress), 6000);
            NetworkStream stream = client.GetStream();
            using (BinaryWriter w = new BinaryWriter(stream))
            {
                using (BinaryReader r = new BinaryReader(stream))
                {
                    w.Write(string.Format(@"{0}", sbMessage.ToString()).ToCharArray());
                }
            }
        }
    }
}