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
    public partial class TriageEntry1 : BasePagePatientPageList
    {
        protected int gridVitalSignPageCount = 1;
        protected int gridProcedurePageCount = 1;
        protected List<vVitalSignDt> lstVitalSignDt = null;

        public override string OnGetMenuCode()
        {
            if (AppSession.IsPatientPageByDepartment)
            {
                switch (AppSession.RegisteredPatient.DepartmentID)
                {
                    case Constant.Facility.EMERGENCY:
                        return Constant.MenuCode.EMR.TRIAGE_ASSESSMENT;
                    default:
                        return Constant.MenuCode.EMR.SOAP_TEMPLATE_TRIAGE;
                }
            }
            else
            {
                return Constant.MenuCode.EMR.SOAP_TEMPLATE_TRIAGE;
            }
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();

            Helper.SetControlEntrySetting(cboTriage, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT));

            cboVisitType.Value = entityVisit.VisitTypeID.ToString();

            if (entityVisit.StartServiceDate == null || entityVisit.StartServiceTime == "")
            {
                txtServiceDate.Text = entityVisit.ActualVisitDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entityVisit.ActualVisitTime;
            }
            else
            {
                txtServiceDate.Text = entityVisit.StartServiceDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtServiceTime.Text = entityVisit.StartServiceTime;
            }

            hdnDepartmentID.Value = entityVisit.DepartmentID;


            if (lstPatientVisitNote.Count > 0)
            {
                PatientVisitNote entitypvn = lstPatientVisitNote.First();
                EntityToControl(entitypvn);
            }

            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", entityVisit.RegistrationID)).FirstOrDefault();
            cboTriage.Value = entityRegistration.GCTriage;
            if (entityRegistration.GCReferrerGroup != null)
                cboReferral.Value = entityRegistration.GCReferrerGroup;
            else
                cboReferral.SelectedIndex = 0;
            hdnReferrerID.Value = entityRegistration.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = entityRegistration.ReferrerParamedicID.ToString();
            if (entityRegistration.ReferrerID != 0)
            {
                txtReferralDescriptionCode.Text = entityRegistration.ReferrerCode;
                txtReferralDescriptionName.Text = entityRegistration.ReferrerName;
            }
            else if (entityRegistration.ReferrerParamedicID != 0)
            {
                ParamedicMaster pm = BusinessLayer.GetParamedicMaster(entityRegistration.ReferrerParamedicID);
                txtReferralDescriptionCode.Text = pm.ParamedicCode;
                txtReferralDescriptionName.Text = pm.FullName;
            }

            cboVisitReason.Value = entityVisit.GCVisitReason;
            txtVisitNotes.Text = entityVisit.VisitReason;
            cboAdmissionCondition.Value = entityVisit.GCAdmissionCondition;

            vPatientEarlyStatus obj = BusinessLayer.GetvPatientEarlyStatusList(string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (obj != null)
            {
                if (obj.ChiefComplaintID == 0)
                {
                    hdnChiefComplaintID.Value = "0";
                }
                else
                { 
                    hdnChiefComplaintID.Value = obj.ChiefComplaintID.ToString();
                    txtChiefComplaint.Text = obj.ChiefComplaintText;
                }
            }


            txtDiagnoseCode.Text = entityRegistration.DiagnoseID;
            txtDiagnoseName.Text = entityRegistration.DiagnoseName;
            txtDiagnoseText.Text = entityRegistration.DiagnosisText;

            BindGridViewVitalSign(1, true, ref gridVitalSignPageCount);
            BindGridViewProcedure(1, true, ref gridProcedurePageCount);
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.TRIAGE, Constant.StandardCode.VISIT_REASON, Constant.StandardCode.ADMISSION_CONDITION, Constant.StandardCode.REFERRAL);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            lstSc.Insert(0, new StandardCode { StandardCodeID = "", StandardCodeName = "" });
            Methods.SetComboBoxField(cboTriage, lstSc.Where(p => p.ParentID == Constant.StandardCode.TRIAGE || p.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboAdmissionCondition, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.ADMISSION_CONDITION || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboVisitReason, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.VISIT_REASON || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstSc.Where(sc => sc.ParentID == Constant.StandardCode.REFERRAL || sc.StandardCodeID == "").ToList(), "StandardCodeName", "StandardCodeID");

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID, (int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            txtPlanningNotes.Text = entitypvn.NoteText;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            //string soapNote = GenerateSOAPText();
            string soapNote = txtPlanningNotes.Text;
            entitypvn.NoteText = soapNote;
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
        }

        private string GenerateSOAPText()
        {
            StringBuilder sbNotes = new StringBuilder();
            sbNotes.AppendLine("Subjective :");
            sbNotes.AppendLine("-".PadRight(15,'-'));
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
            sbNotes.AppendLine("-".PadRight(15,'-'));
            List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND ObservationDate = '{1}' AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstVitalSignDt.Count > 0)
            {
                sbNotes.AppendLine("Vital Signs :");
                foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                {
                    sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                }
            }
            List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0} AND  ObservationDate = '{1}') AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID, Helper.GetDatePickerValue(txtServiceDate).ToString(Constant.FormatString.DATE_FORMAT_112)));
            if (lstROS.Count > 0)
            {
                sbNotes.AppendLine(" ");
                sbNotes.AppendLine("Review of System :");
                foreach (vReviewOfSystemDt item in lstROS)
                {
                    sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                }
            }
            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Assessment");
            sbNotes.AppendLine("-".PadRight(15,'-'));
            if (!string.IsNullOrEmpty(txtDiagnoseText.Text))
            {
                sbNotes.AppendLine(string.Format("{0} ({1})", txtDiagnoseText.Text, txtDiagnoseCode.Text));
            }

            sbNotes.AppendLine(" ");
            sbNotes.AppendLine("Planning ");
            sbNotes.AppendLine("-".PadRight(15,'-'));
            sbNotes.AppendLine(txtPlanningNotes.Text);

            return sbNotes.ToString();
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
            ChiefComplaintDao chiefComplaintDao = new ChiefComplaintDao(ctx);
            ConsultVisitDao consultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);

            ConsultVisit entityConsultVisit = consultVisitDao.Get(AppSession.RegisteredPatient.VisitID);
            if (entityConsultVisit.GCVisitStatus == Constant.VisitStatus.CHECKED_IN)
            {
                entityConsultVisit.StartServiceDate = Helper.GetDatePickerValue(txtServiceDate);
                entityConsultVisit.StartServiceTime = txtServiceTime.Text;
            }

            if (!string.IsNullOrEmpty(cboVisitType.Value.ToString()))
            {
                entityConsultVisit.VisitTypeID = Convert.ToInt16(cboVisitType.Value);
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY)
            {
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}", hdnTimeElapsed0hour.Value.PadLeft(2, '0'), hdnTimeElapsed0minute.Value.PadLeft(2, '0'));
            }
            entityConsultVisit.TimeElapsed1 = string.Format("{0}:{1}", hdnTimeElapsed1hour.Value.PadLeft(2, '0'), hdnTimeElapsed1minute.Value.PadLeft(2, '0'));
            entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
            if (cboVisitReason.Value != null)
            {
                entityConsultVisit.GCVisitReason = cboVisitReason.Value.ToString();
                if (entityConsultVisit.GCVisitReason == Constant.VisitReason.OTHER)
                    entityConsultVisit.VisitReason = txtVisitNotes.Text;
            }
            else
                entityConsultVisit.VisitReason = txtVisitNotes.Text;
            if (cboAdmissionCondition.Value != null)
                entityConsultVisit.GCAdmissionCondition = cboAdmissionCondition.Value.ToString();
            else
                entityConsultVisit.GCAdmissionCondition = null;

            consultVisitDao.Update(entityConsultVisit);

            Registration entityRegistration = registrationDao.Get(entityConsultVisit.RegistrationID);
            if (cboTriage.Value == null)
                entityRegistration.GCTriage = "";
            else
                entityRegistration.GCTriage = cboTriage.Value.ToString();
            if (cboReferral.Value != null)
                entityRegistration.GCReferrerGroup = cboReferral.Value.ToString();
            else entityRegistration.GCReferrerGroup = null;
            if (hdnReferrerID.Value == "" || hdnReferrerID.Value == "0")
                entityRegistration.ReferrerID = null;
            else
                entityRegistration.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);

            if (hdnReferrerParamedicID.Value == "" || hdnReferrerParamedicID.Value == "0")
                entityRegistration.ReferrerParamedicID = null;
            else
                entityRegistration.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);

            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;
            registrationDao.Update(entityRegistration);

            ChiefComplaint entity = null;
            bool isNewChiefComplaint = true;

            if (hdnChiefComplaintID.Value != "" && hdnChiefComplaintID.Value != "0")
            {
                entity = chiefComplaintDao.Get(Convert.ToInt32(hdnChiefComplaintID.Value));
                isNewChiefComplaint = false;
            }
            else
            {
                entity = new ChiefComplaint();
                entity.VisitID = AppSession.RegisteredPatient.VisitID;
                entity.CreatedBy = AppSession.UserLogin.UserID;
            }
            entity.ObservationDate = Helper.GetDatePickerValue(txtServiceDate);
            entity.ObservationTime = txtServiceTime.Text;
            entity.ChiefComplaintText = txtChiefComplaint.Text;
            entity.IsAutoAnamnesis = Convert.ToBoolean(chkAutoAnamnesis.Checked);
            entity.IsAlloAnamnesis = Convert.ToBoolean(chkAlloAnamnesis.Checked);
            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

            if (isNewChiefComplaint)
                chiefComplaintDao.Insert(entity);
            else
                chiefComplaintDao.Update(entity);

            PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
            PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS)).FirstOrDefault();
            if (diffDx == null)
            {
                diffDx = new PatientDiagnosis();
                diffDx.ParamedicID = Convert.ToInt32(AppSession.RegisteredPatient.ParamedicID);
                diffDx.DiagnoseID = txtDiagnoseCode.Text;
                diffDx.DiagnosisText = txtDiagnoseText.Text;
                diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                diffDx.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                diffDx.DifferentialTime = txtServiceTime.Text;
                diffDx.VisitID = AppSession.RegisteredPatient.VisitID;
                diffDx.CreatedBy = AppSession.UserLogin.UserID;
                patientDiagnosisDao.Insert(diffDx);
            }
            else
            {
                diffDx.ParamedicID = Convert.ToInt32(AppSession.RegisteredPatient.ParamedicID);
                diffDx.DiagnoseID = txtDiagnoseCode.Text;
                diffDx.DiagnosisText = txtDiagnoseText.Text;
                diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                diffDx.DifferentialDate = Helper.GetDatePickerValue(txtServiceDate);
                diffDx.DifferentialTime = txtServiceTime.Text;
                diffDx.VisitID = AppSession.RegisteredPatient.VisitID;
                diffDx.LastUpdatedBy = AppSession.UserLogin.UserID;
                patientDiagnosisDao.Update(diffDx);
            }
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

                    PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT), ctx).FirstOrDefault();
                    bool isEntityEmergencyCaseNoteNull = false;
                    if (entityEmergencyCaseNote == null)
                    {
                        isEntityEmergencyCaseNoteNull = true;
                        entityEmergencyCaseNote = new PatientVisitNote();
                    }
                    ControlToEntity(entityEmergencyCaseNote);

                    if (isEntityEmergencyCaseNoteNull)
                    {
                        entityEmergencyCaseNote.VisitID = AppSession.RegisteredPatient.VisitID;
                        entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_INITIAL_ASSESSMENT;
                        entityEmergencyCaseNote.CreatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Insert(entityEmergencyCaseNote);
                    }
                    else
                    {
                        entityEmergencyCaseNote.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientVisitNoteDao.Update(entityEmergencyCaseNote);
                    }

                    UpdateConsultVisitRegistration(ctx);
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
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

        private void BindGridViewVitalSign(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Empty;
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

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

        protected void cbpProcedureView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridViewProcedure(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridViewProcedure(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void BindGridViewProcedure(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            if (isCountPageCount)
            {
                int rowCount = BusinessLayer.GetvPatientProcedureRowCount(filterExpression);
                pageCount = Helper.GetPageCount(rowCount, Constant.GridViewPageSize.GRID_COMPACT);
            }

            List<vPatientProcedure> lstEntity = BusinessLayer.GetvPatientProcedureList(filterExpression, Constant.GridViewPageSize.GRID_COMPACT, pageIndex, "ID DESC");
            grdProcedureView.DataSource = lstEntity;
            grdProcedureView.DataBind();
        }
    }
}
