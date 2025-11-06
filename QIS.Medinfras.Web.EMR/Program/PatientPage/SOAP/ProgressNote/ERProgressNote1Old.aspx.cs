﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Data;
using System.Text;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class ERProgressNote1 : BasePagePatientPageList
    {
        protected int gridAllergyPageCount = 1;
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected int gridROSPageCount = 1;
        protected int gridDiagnosisPageCount = 1;
        protected int gridLaboratoryPageCount = 1;
        protected int gridImagingPageCount = 1;
        protected int gridDiagnosticPageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;
        protected List<vReviewOfSystemDt> lstReviewOfSystemDt = null;

        public override string OnGetMenuCode()
        {
            if (AppSession.IsPatientPageByDepartment)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EMR.EMERGENCY_SOAP_PROGRESS_ASSESSMENT_1;
                    default:
                        return Constant.MenuCode.EMR.EMERGENCY_SOAP_PROGRESS_ASSESSMENT_1;
                }
            }
            else
            {
                return Constant.MenuCode.EMR.EMERGENCY_SOAP_PROGRESS_ASSESSMENT_1;
            }
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();

            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}' ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES));
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;

            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                EntityToControl(entitypvn);
            }
            else
            {
                ResetEntryControls();
            }

            vChiefComplaint obj = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                }
                else
                {
                    hdnChiefComplaintID.Value = obj.ID.ToString();
                    StringBuilder chiefComplaint = new StringBuilder();
                    chiefComplaint.AppendLine(obj.ChiefComplaintText.TrimEnd());
                    chiefComplaint.AppendLine("");
                    chiefComplaint.AppendLine("Riwayat Penyakit Sekarang:");
                    chiefComplaint.AppendLine("----------------------------");
                    chiefComplaint.AppendLine(obj.HPISummary);

                    #region Past Medical History
                    vPastMedical pastMedical = BusinessLayer.GetvPastMedicalList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID)).FirstOrDefault();
                    if (pastMedical != null)
                    {
                        chiefComplaint.AppendLine("");
                        chiefComplaint.AppendLine("Riwayat Penyakit Terdahulu:");
                        chiefComplaint.AppendLine("----------------------------");
                        chiefComplaint.AppendLine(pastMedical.MedicalSummary);
                        chiefComplaint.AppendLine("");
                        chiefComplaint.AppendLine("Riwayat Pengobatan");
                        chiefComplaint.AppendLine("------------------");
                        chiefComplaint.AppendLine(pastMedical.MedicationSummary);
                    }
                    #endregion

                    txtChiefComplaint.Text = chiefComplaint.ToString();
                }
            }

            BindGridViewAllergy(1, true, ref gridAllergyPageCount);
            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewROS(1, true, ref gridROSPageCount);
            BindGridViewDiagnosis(1, true, ref gridDiagnosisPageCount);
            BindGridViewLaboratory(1, true, ref gridLaboratoryPageCount);
            BindGridViewImaging(1, true, ref gridImagingPageCount);
            BindGridViewDiagnostic(1, true, ref gridDiagnosticPageCount);

            LoadBodyDiagram();

            hdnIsChanged.Value = "0";
        }

        private void ResetEntryControls()
        {
            txtNoteDate.Text = DateTime.Now.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtSubjective.Text = string.Empty;
            txtObjective.Text = string.Empty;
            txtAssessment.Text = string.Empty;
            txtPlanning.Text = string.Empty;
        }

        private void LoadBodyDiagram()
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            int pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
            hdnPageCount.Value = pageCount.ToString();

            if (pageCount > 0)
            {
                hdnPageIndex.Value = "0";
                OnLoadBodyDiagram(0);
                tblBodyDiagramNavigation.Style.Remove("display");
            }
            else
            {
                divBodyDiagram.Style.Add("display", "none");
                tblEmpty.Style.Remove("display");
            }
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.ALLERGEN_TYPE, Constant.StandardCode.DIAGNOSIS_TYPE, Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);

            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboAllergenType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ALLERGEN_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisType, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIAGNOSIS_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField(cboDiagnosisStatus, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.DIFFERENTIAL_DIAGNOSIS_STATUS).ToList(), "StandardCodeName", "StandardCodeID");

            List<SettingParameterDt> lstSettingParameter = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI));
            hdnImagingServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).ParameterValue;
            hdnLaboratoryServiceUnitID.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).ParameterValue;
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            txtNoteDate.Text = entitypvn.NoteDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtNoteTime.Text = entitypvn.NoteTime;
            txtSubjective.Text = entitypvn.SubjectiveText;
            txtObjective.Text = entitypvn.ObjectiveText;
            txtAssessment.Text = entitypvn.AssessmentText;
            txtPlanning.Text = entitypvn.PlanningText;        
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            //string soapNote = GenerateSOAPText();
            entitypvn.SubjectiveText = txtSubjective.Text;
            entitypvn.ObjectiveText = txtObjective.Text;
            entitypvn.AssessmentText = txtAssessment.Text;
            entitypvn.PlanningText = txtPlanning.Text;
            entitypvn.NoteText = string.Format(@"S:{0}{1}{2}O:{3}{4}{5}A:{6}{7}{8}P:{9}{10}",
                Environment.NewLine, txtSubjective.Text, Environment.NewLine,
                Environment.NewLine, txtObjective.Text, Environment.NewLine,
                Environment.NewLine, txtAssessment.Text, Environment.NewLine,
                Environment.NewLine, txtPlanning.Text);
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtNoteDate);
            entitypvn.NoteTime = txtNoteTime.Text;
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            if ((AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID))
            {
                vChiefComplaint oChiefComplaint = BusinessLayer.GetvChiefComplaintList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
                if (oChiefComplaint != null)
                {
                    sbNotes.AppendLine(string.Format("Chief Complaint  : "));
                    sbNotes.AppendLine(string.Format(" {0}   ", oChiefComplaint.ChiefComplaintText));
                    if ((!string.IsNullOrEmpty(oChiefComplaint.Location)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality)) ||
                       (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset)) ||
                       (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming)) || (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation)))
                    {
                        sbNotes.AppendLine(string.Format("HPI  : "));
                    }
                    if (!string.IsNullOrEmpty(oChiefComplaint.Location))
                        sbNotes.AppendLine(string.Format("- Location    (R) : {0}", oChiefComplaint.Location));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayQuality))
                        sbNotes.AppendLine(string.Format("- Quality     (Q) : {0}", oChiefComplaint.DisplayQuality));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplaySeverity))
                        sbNotes.AppendLine(string.Format("- Severity    (S) : {0}", oChiefComplaint.DisplaySeverity));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayOnset))
                        sbNotes.AppendLine(string.Format("- Onset       (O) : {0}", oChiefComplaint.DisplayOnset));
                    if (!string.IsNullOrEmpty(oChiefComplaint.CourseTiming))
                        sbNotes.AppendLine(string.Format("- Timing      (T) : {0}", oChiefComplaint.CourseTiming));
                    if (!string.IsNullOrEmpty(oChiefComplaint.DisplayProvocation))
                        sbNotes.AppendLine(string.Format("- Provocation (T) : {0}", oChiefComplaint.DisplayProvocation));
                }
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Objective :");
            sbNotes.AppendLine("-".PadRight(15, '-'));
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine("Vital Signs :");
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID));
            if (lstROS.Count > 0)
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Review of System :");
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }
            //sbNotes.AppendLine(" ");
            //sbNotes.AppendLine("Assessment");
            //sbNotes.AppendLine("-".PadRight(15, '-'));
            //if (!string.IsNullOrEmpty(txtDiagnoseText.Text))
            //{
            //    sbNotes.AppendLine(string.Format("{0} ({1})", txtDiagnoseText.Text, txtDiagnoseCode.Text));
            //}

            return sbNotes.ToString();
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "save")
            {
                bool result = true;
                IDbContext ctx = DbFactory.Configure(true);
                PatientVisitNoteDao patientVisitNoteDao = new PatientVisitNoteDao(ctx);
                try
                {

                    PatientVisitNote patientNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES), ctx).FirstOrDefault();
                    bool isPatientNoteNull = false;
                    if (patientNote == null)
                    {
                        isPatientNoteNull = true;
                        patientNote = new PatientVisitNote();
                    }
                    ControlToEntity(patientNote);

                    if (isPatientNoteNull)
                    {
                        patientNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        patientNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        patientNote.GCPatientNoteType = Constant.PatientVisitNotes.SOAP_SUMMARY_NOTES;
                        patientNote.CreatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Insert(patientNote);
                    }
                    else
                    {
                        patientNote.ParamedicID = AppSession.UserLogin.ParamedicID;
                        patientNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(patientNote);
                    }

                    ctx.CommitTransaction();

                    hdnIsSaved.Value = "1";
                    hdnIsChanged.Value = "0";
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    hdnIsSaved.Value = "0";
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }

                return result;
            }
            return true;
        }


        private void BindGridViewAllergy(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.MRN);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientAllergyRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientAllergy> lstEntity = BusinessLayer.GetvPatientAllergyList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex);
            grdAllergyView.DataSource = lstEntity;
            grdAllergyView.DataBind();
        }

        protected void cbpAllergyView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewAllergy(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {
                    BindGridViewAllergy(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

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

        protected void cbpAllergy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientAllergy oAllergy = new PatientAllergy();

                        oAllergy.MRN = AppSession.RegisteredPatient.MRN;
                        oAllergy.AllergyLogDate = DateTime.Now.Date;
                        oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                        oAllergy.Allergen = txtAllergenName.Text;
                        oAllergy.GCAllergySource = Constant.AllergenFindingSource.PATIENT;
                        oAllergy.GCAllergySeverity = Constant.AllergySeverity.UNKNOWN;
                        oAllergy.KnownDate = DateTime.Now.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oAllergy.Reaction = txtReaction.Text;
                        oAllergy.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientAllergy(oAllergy);
                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            oAllergy.GCAllergenType = cboAllergenType.Value.ToString();
                            oAllergy.Allergen = txtAllergenName.Text;
                            oAllergy.Reaction = txtReaction.Text;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Allergy Record Information");
                        }
                    }
                    else
                    {
                        int allergyID = Convert.ToInt32(hdnAllergyID.Value);
                        PatientAllergy oAllergy = BusinessLayer.GetPatientAllergy(allergyID);

                        if (oAllergy != null)
                        {
                            //TODO : Prompt user for delete reason
                            oAllergy.IsDeleted = true;
                            oAllergy.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientAllergy(oAllergy);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Allergy Record Information");
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

        #region Body Diagram
        protected void cbpBodyDiagramView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            int pageIndex = Convert.ToInt32(hdnPageIndex.Value);
            int pageCount = Convert.ToInt32(hdnPageCount.Value);
            if (e.Parameter == "refresh")
            {
                string filterExpression = "";
                filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

                pageCount = BusinessLayer.GetvPatientBodyDiagramHdRowCount(filterExpression);
                result = "count|" + pageCount;
                if (pageCount > 0)
                    OnLoadBodyDiagram(0);
            }
            else if (e.Parameter == "edit")
            {
                result = "edit";
                OnLoadBodyDiagram(pageIndex);
            }
            else
            {
                if (e.Parameter == "next")
                {
                    pageIndex++;
                    if (pageIndex == pageCount)
                        pageIndex = 0;
                }
                else if (e.Parameter == "prev")
                {
                    pageIndex--;
                    if (pageIndex < 0)
                        pageIndex = pageCount - 1;
                }
                OnLoadBodyDiagram(pageIndex);
                result = "index|" + pageIndex;
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void OnLoadBodyDiagram(int PageIndex)
        {
            string filterExpression = "";
            filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);
            vPatientBodyDiagramHd entity = BusinessLayer.GetvPatientBodyDiagramHd(filterExpression, PageIndex, "ID DESC");
            BodyDiagramToControl(entity);

            filterExpression = string.Format("ID = {0} AND IsDeleted = 0", entity.ID);
            rptRemarks.DataSource = BusinessLayer.GetvPatientBodyDiagramDtList(filterExpression);
            rptRemarks.DataBind();
        }

        private void BodyDiagramToControl(vPatientBodyDiagramHd entity)
        {
            spnParamedicName.InnerHtml = entity.ParamedicName;
            spnObservationDateTime.InnerHtml = entity.DisplayObservationDateTime;
            spnDiagramName.InnerHtml = entity.DiagramName;

            imgBodyDiagram.Src = entity.FileImageUrl;
            hdnBodyDiagramID.Value = entity.ID.ToString();

        }

        protected void cbpDeleteBodyDiagram_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|";

            if (hdnBodyDiagramID.Value != "")
            {
                PatientBodyDiagramHd entity = BusinessLayer.GetPatientBodyDiagramHd(Convert.ToInt32(hdnBodyDiagramID.Value));
                entity.IsDeleted = true;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdatePatientBodyDiagramHd(entity);
            }
            else
            {
                result = "0|There is no record to be deleted !";
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
        #endregion

        #region Diagnosis
        private void BindGridViewDiagnosis(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientDiagnosisRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vPatientDiagnosis> lstEntity = BusinessLayer.GetvPatientDiagnosisList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "GCDiagnoseType");
            List<vPatientDiagnosis> lstMainDiagnosis = lstEntity.Where(lst => lst.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).ToList();
            hdnIsMainDiagnosisExists.Value = lstMainDiagnosis.Count > 0 ? "1" : "0";

            grdDiagnosisView.DataSource = lstEntity;
            grdDiagnosisView.DataBind();
        }

        protected void cbpDiagnosisView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewDiagnosis(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewDiagnosis(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpRetval"] = hdnIsMainDiagnosisExists.Value;
        }
        protected void cbpDiagnosis_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "1|add|";

            try
            {
                if (e.Parameter != null && e.Parameter != "")
                {
                    string[] param = e.Parameter.Split('|');
                    if (param[0] == "add")
                    {
                        PatientDiagnosis entity = new PatientDiagnosis();

                        entity.VisitID = AppSession.RegisteredPatient.VisitID;
                        entity.DifferentialDate = Helper.GetDatePickerValue(txtNoteDate);
                        entity.DifferentialTime = txtNoteTime.Text;

                        entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                        entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                        hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                        entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                        if (hdnEntryDiagnoseID.Value != "")
                            entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                        else
                            entity.DiagnoseID = null;

                        entity.DiagnosisText = txtDiagnosisText.Text;
                        entity.MorphologyID = null;
                        entity.IsChronicDisease = false;
                        entity.IsFollowUpCase = false;
                        entity.Remarks = string.Empty;
                        entity.CreatedBy = AppSession.UserLogin.UserID;

                        BusinessLayer.InsertPatientDiagnosis(entity);

                        result = "1|add|";
                    }
                    else if (param[0] == "edit")
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            entity.DifferentialDate = Helper.GetDatePickerValue(txtNoteDate);
                            entity.DifferentialTime = txtNoteTime.Text;

                            entity.ParamedicID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            entity.GCDiagnoseType = cboDiagnosisType.Value.ToString();
                            hdnIsMainDiagnosisExists.Value = entity.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS ? "1" : "0";
                            entity.GCDifferentialStatus = cboDiagnosisStatus.Value.ToString();

                            if (hdnEntryDiagnoseID.Value != "")
                                entity.DiagnoseID = hdnEntryDiagnoseID.Value;
                            else
                                entity.DiagnoseID = null;

                            entity.DiagnosisText = txtDiagnosisText.Text;
                            entity.MorphologyID = null;
                            entity.IsChronicDisease = false;
                            entity.IsFollowUpCase = false;
                            entity.Remarks = string.Empty;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|edit|";
                        }
                        else
                        {
                            result = string.Format("0|delete|{0}", "Invalid Patient Diagnosis Record Information");
                        }
                    }
                    else
                    {
                        int recordID = Convert.ToInt32(hdnDiagnosisID.Value);
                        PatientDiagnosis entity = BusinessLayer.GetPatientDiagnosis(recordID);

                        if (entity != null)
                        {
                            //TODO : Prompt user for delete reason
                            entity.IsDeleted = true;
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                            BusinessLayer.UpdatePatientDiagnosis(entity);
                            result = "1|delete|";
                        }
                        else
                        {
                            result = string.Format("0|edit|{0}", "Invalid Patient Diagnosis Record Information");
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
            string filterExpression = string.Format("VisitID = {0} AND HealthcareServiceUnitID = {1} AND GCTransactionStatus != '{2}'", AppSession.RegisteredPatient.VisitID, hdnImagingServiceUnitID.Value ,Constant.TransactionStatus.VOID);

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

        #region Diagnostic
        private void BindGridViewDiagnostic(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}'", AppSession.RegisteredPatient.VisitID, Constant.TransactionStatus.VOID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvTestOrderHdRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_MASTER);
            }

            List<vTestOrderHd> lstEntity = BusinessLayer.GetvTestOrderHdList(filterExpression, Constant.GridViewPageSize.GRID_MASTER, pageIndex, "TestOrderID DESC");
            grdDiagnosticView.DataSource = lstEntity;
            grdDiagnosticView.DataBind();
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
                    result = "refresh|" + pageCount;
                }
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
                    default:
                        break;
                }

                if (testOrderID != "0")
                {
                    TestOrderHd entity = BusinessLayer.GetTestOrderHd(Convert.ToInt32(testOrderID));
                    entity.GCTransactionStatus = Constant.TransactionStatus.VOID;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateTestOrderHd(entity);
                    result = string.Format("1|{0}",e.Parameter);
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
                            //try
                            //{
                            //    HealthcareServiceUnit hsu = BusinessLayer.GetHealthcareServiceUnit(Convert.ToInt32(entity.HealthcareServiceUnitID));
                            //    string ipAddress = hsu.IPAddress == null ? string.Empty : hsu.IPAddress;

                            //    if (!String.IsNullOrEmpty(ipAddress))
                            //    {
                            //        SendNotification(entity,ipAddress);
                            //    }
                            //}
                            //catch (Exception ex)
                            //{
                            //}
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
    }
}
