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
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class IntegrationNote1 : BasePagePatientPageListEntry
    {
        protected int PageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;

        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;
        public override string OnGetMenuCode()
        {
            switch (hdnMenuType.Value)
            {
                case "MD":
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE;
                default:
                    return Constant.MenuCode.EMR.SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE;
            }
        }

        #region List
        protected override void InitializeDataControl()
        {
            if (Page.Request.QueryString["id"] != null)
            {
                hdnMenuType.Value = Page.Request.QueryString["id"];
            }

            hdnVisitID.Value = AppSession.RegisteredPatient.VisitID.ToString();
            hdnIsDPJPPhysician.Value = AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID ? "1" : "0";
            hdnPatientInformation.Value = string.Format("{0} (MRN = {1}, REG = {2}, LOC = {3})", AppSession.RegisteredPatient.PatientName, AppSession.RegisteredPatient.MedicalNo, AppSession.RegisteredPatient.RegistrationNo, AppSession.RegisteredPatient.ServiceUnitName);
            hdnIsShowVerifyButton.Value = AppSession.IsUseVerifyAllButton ? "1" : "0";

            txtFromDate.Text = DateTime.Today.AddDays(-1).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtToDate.Text = DateTime.Today.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (AppSession.UserLogin.IsSpecialist)
                tblNotesInstruction.Style.Add("display", "none");
            else
                tblNotesInstruction.Style.Add("display", "table");

            SetControlProperties();

            BindGridView(1, true, ref PageCount);
            //BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            //BindGridViewROS(1, true, ref gridROSPageCount);
            //BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            //BindGridViewImaging(1, true, ref gridImagingPageCount);

            btnVerifyAll.Visible = AppSession.UserLogin.ParamedicID == AppSession.RegisteredPatient.ParamedicID;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowDelete = false;
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
            filterExpression += string.Format("VisitID IN ('{0}','{1}') AND IsDeleted = 0 AND (NoteDate BETWEEN '{2}' AND '{3}') ", AppSession.RegisteredPatient.VisitID, cvLinkedID, Helper.GetDatePickerValue(txtFromDate).ToString(Constant.FormatString.DATE_FORMAT_112), Helper.GetDatePickerValue(txtToDate).ToString(Constant.FormatString.DATE_FORMAT_112));

            filterExpression += string.Format(" AND GCPatientNoteType NOT IN ('{0}')", Constant.PatientVisitNotes.REGISTRATION_NOTES);

            if (code == "0")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}','{6}')", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);
            else if (code == "1")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}', '{2}')", Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES);
            else if (code == "2")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}')", Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);
            else if (code == "3")
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}') AND ParamedicID = {3}", Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, AppSession.UserLogin.ParamedicID);
            else
                filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}','{7}') AND IsNeedConfirmation = 1 AND ConfirmationPhysicianID = {6}", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.INPATIENT_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES, AppSession.RegisteredPatient.ParamedicID, Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);

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
        #endregion
        #region Entry
        protected override void SetControlProperties()
        {
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            string filterExpression = string.Format("ParentID = '{0}'", Constant.StandardCode.PHYSICIAN_INSTRUCTION_SOURCE);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField(cboPhysicianInstructionSource, lstStandardCode, "StandardCodeName", "StandardCodeID");

            List<vParamedicMaster> lstParamedic = BusinessLayer.GetvParamedicMasterList(string.Format("GCParamedicMasterType = '{0}'", Constant.ParamedicType.Physician));
            Methods.SetComboBoxField<vParamedicMaster>(cboPhysician, lstParamedic, "ParamedicName", "ParamedicID");
            Methods.SetComboBoxField<vParamedicMaster>(cboSpecialistPhysician, lstParamedic, "ParamedicName", "ParamedicID");

            hdnDefaultParamedicID.Value = AppSession.RegisteredPatient.ParamedicID.ToString();

            if (AppSession.UserLogin.GCParamedicMasterType == Constant.ParamedicType.Physician)
            {
                int? userLoginParamedic = AppSession.UserLogin.ParamedicID;
                cboPhysician.ClientEnabled = false;
                cboPhysician.Value = AppSession.UserLogin.ParamedicID.ToString();
                hdnDefaultParamedicID.Value = userLoginParamedic.ToString();
            }

            hdnImagingServiceUnitID.Value = AppSession.ImagingServiceUnitID;
            hdnLaboratoryServiceUnitID.Value = AppSession.LaboratoryServiceUnitID;
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
            entity.InstructionText = txtInstructionSummary.Text;
            entity.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}{11}I:{12}{13}",
                Environment.NewLine, txtSubjectiveText.Text, Environment.NewLine,
                Environment.NewLine, txtObjectiveText.Text, Environment.NewLine,
                Environment.NewLine, txtAssessmentText.Text, Environment.NewLine,
                Environment.NewLine, txtPlanningText.Text, Environment.NewLine,
                Environment.NewLine, txtInstructionSummary.Text);

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

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            txtNoteDate.Text = entitypvn.NoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entitypvn.NoteTime;
            txtSubjectiveText.Text = entitypvn.SubjectiveText;
            txtObjectiveText.Text = entitypvn.ObjectiveText;
            txtAssessmentText.Text = entitypvn.AssessmentText;
            txtPlanningText.Text = entitypvn.PlanningText;
            chkIsNeedConfirmation.Checked = entitypvn.IsNeedConfirmation;
            if (entitypvn.IsNeedConfirmation)
            {
                if (entitypvn.ConfirmationPhysicianID != null)
                {
                    cboSpecialistPhysician.Value = entitypvn.ConfirmationPhysicianID;
                }
            }
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao oVisitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao oInstructionDao = new PatientInstructionDao(ctx);

            try
            {
                PatientVisitNote entity = new PatientVisitNote();
                int visitNoteID = 0;
                ControlToEntity(entity);
                if (entity.IsNeedConfirmation && entity.ConfirmationPhysicianID == null)
                {
                    errMessage = "Dokter untuk konfirmasi harus ditentukan";
                    ctx.RollBackTransaction();
                    result = false;
                }
                else
                {
                    entity.VisitID = AppSession.RegisteredPatient.VisitID;
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    visitNoteID = oVisitNoteDao.InsertReturnPrimaryKeyID(entity);

                    if (!string.IsNullOrEmpty(txtInstructionSummary.Text))
                    {
                        PatientInstruction oInstruction = new PatientInstruction();
                        oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
                        oInstruction.PatientVisitNoteID = visitNoteID;
                        oInstruction.PhysicianID = Convert.ToInt32(cboPhysician.Value);
                        oInstruction.GCInstructionGroup = "X139^003";
                        oInstruction.Description = txtInstructionSummary.Text;
                        oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
                        oInstruction.InstructionTime = txtNoteTime.Text;
                        oInstruction.CreatedBy = AppSession.UserLogin.UserID;
                        oInstructionDao.Insert(oInstruction);
                    }
                    ctx.CommitTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientVisitNoteDao oVisitNoteDao = new PatientVisitNoteDao(ctx);
            PatientInstructionDao oInstructionDao = new PatientInstructionDao(ctx);

            try
            {
                PatientVisitNote entity = BusinessLayer.GetPatientVisitNote(Convert.ToInt32(hdnEntryID.Value));
                ControlToEntity(entity);
                if (entity.IsNeedConfirmation && entity.ConfirmationPhysicianID == null)
                {
                    errMessage = "Dokter untuk konfirmasi harus ditentukan";
                    ctx.RollBackTransaction();
                    result = false;
                }
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                oVisitNoteDao.Update(entity);

                if (!string.IsNullOrEmpty(txtInstructionSummary.Text))
                {
                    PatientInstruction oInstruction = BusinessLayer.GetPatientInstructionList(string.Format("VisitID = {0} AND PatientVisitNoteID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, entity.ID), ctx).FirstOrDefault();
                    if (oInstruction == null)
                    {
                        oInstruction = new PatientInstruction();
                        SetInstructionEntity(entity.ID, oInstruction);
                        oInstructionDao.Insert(oInstruction);
                    }
                    else
                    {
                        SetInstructionEntity(entity.ID, oInstruction);
                        oInstructionDao.Update(oInstruction);
                    }
                }

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                result = false;
                errMessage = string.Format("{0}<br/><br/><i>{1}</i>", ex.Message, ex.Source);
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void SetInstructionEntity(int visitNoteID, PatientInstruction oInstruction)
        {
            oInstruction.VisitID = AppSession.RegisteredPatient.VisitID;
            oInstruction.PatientVisitNoteID = visitNoteID;
            oInstruction.PhysicianID = Convert.ToInt32(cboPhysician.Value);
            oInstruction.GCInstructionGroup = "X139^003";
            oInstruction.Description = txtInstructionSummary.Text;
            oInstruction.InstructionDate = Helper.GetDatePickerValue(txtNoteDate);
            oInstruction.InstructionTime = txtNoteTime.Text;
            oInstruction.CreatedBy = AppSession.UserLogin.UserID;
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
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnLaboratoryServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdLaboratoryView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Row.DataItem;
                Repeater rptLaboratoryDt = (Repeater)e.Row.FindControl("rptLaboratoryDt");
                rptLaboratoryDt.DataSource = GetTestOrderDt(obj.TestOrderID);
                rptLaboratoryDt.DataBind();
            }
        }

        private object GetTestOrderDt(int testOrderID)
        {
            List<vTestOrderDt> lstOrderDt = BusinessLayer.GetvTestOrderDtList(string.Format("TestOrderID = {0} ORDER BY ItemName1", testOrderID));
            return lstOrderDt;
        }
        #endregion

        #region Imaging
        private void BindGridViewImaging(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnImagingServiceUnitID.Value, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
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
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void grdImagingView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                vTestOrderHd obj = (vTestOrderHd)e.Row.DataItem;
                Repeater rptImagingDt = (Repeater)e.Row.FindControl("rptImagingDt");
                rptImagingDt.DataSource = GetTestOrderDt(obj.TestOrderID);
                rptImagingDt.DataBind();
            }
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
                HtmlControl divNeedConfirmation = (HtmlControl)e.Row.FindControl("divNeedConfirmation");
                HtmlControl divConfirmationInfo = (HtmlControl)e.Row.FindControl("divConfirmationInfo");
                HtmlControl divVerifiedInformation = (HtmlControl)e.Row.FindControl("divVerifiedInformation");
                HtmlControl divSignature = e.Row.FindControl("divParamedicSignature") as HtmlControl;
                HtmlControl divView = e.Row.FindControl("divView") as HtmlControl;

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
                    divView.Visible = entity.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT;
                }
            }
        }

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
                            NurseChiefComplaint oNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("PatientVisitNoteID = {0}",oVisitNote.ID),ctx).FirstOrDefault();
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
                            oVisitNoteDao.Update(oVisitNote);

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
                    string filterExpression = hdnFilterExpression.Value;
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
                    filterExpression += string.Format(" AND GCPatientNoteType IN ('{0}','{1}','{2}','{3}','{4}','{5}') AND (IsVerified = 0 OR IsVerified IS NULL)", Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT, Constant.PatientVisitNotes.NURSING_NOTES, Constant.PatientVisitNotes.PHARMACY_NOTES, Constant.PatientVisitNotes.DIAGNOSTIC_SUPPORT_NOTES,Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT);

                    List<PatientVisitNote> lstVisitNote = BusinessLayer.GetPatientVisitNoteList(filterExpression, ctx);
                    foreach (PatientVisitNote visitNote in lstVisitNote)
                    {
                        bool isValid = true;
                        if (visitNote.GCPatientNoteType == Constant.PatientVisitNotes.NURSE_INITIAL_ASSESSMENT)
                        {
                            NurseChiefComplaint oNurseChiefComplaint = BusinessLayer.GetNurseChiefComplaintList(string.Format("PatientVisitNoteID = {0}", visitNote.ID),ctx).FirstOrDefault();
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
                            visitNote.VerifiedDateTime = DateTime.Now;
                            visitNoteDao.Update(visitNote);

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