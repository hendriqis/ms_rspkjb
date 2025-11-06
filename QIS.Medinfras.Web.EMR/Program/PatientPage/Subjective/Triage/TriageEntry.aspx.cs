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

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class TriageEntry : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.TRIAGE;
        }

        protected override void InitializeDataControl()
        {
            SetControlProperties();

            Helper.SetControlEntrySetting(cboTriage, new ControlEntrySetting(true, true, true), "mpPatientStatus");
            Helper.SetControlEntrySetting(cboVisitType, new ControlEntrySetting(true, true, true), "mpPatientStatus");

            vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}",AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            List<PatientVisitNote> lstPatientVisitNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = '{0}' AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE));

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
                       
            vRegistration entityRegistration = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}",entityVisit.RegistrationID)).FirstOrDefault();
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

            List<GetParamedicVisitTypeList> visitTypeList = BusinessLayer.GetParamedicVisitTypeList(AppSession.RegisteredPatient.HealthcareServiceUnitID,(int)AppSession.UserLogin.ParamedicID, "");
            Methods.SetComboBoxField(cboVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
        }

        private void EntityToControl(PatientVisitNote entitypvn)
        {
            txtEmergencyCase.Text = entitypvn.NoteText;
        }

        private void ControlToEntity(PatientVisitNote entitypvn)
        {
            entitypvn.NoteText = txtEmergencyCase.Text;
            entitypvn.NoteDate = Helper.GetDatePickerValue(txtServiceDate);
        }

        private void UpdateConsultVisitRegistration(IDbContext ctx)
        {
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
                entityConsultVisit.TimeElapsed0 = string.Format("{0}:{1}",hdnTimeElapsed0hour.Value.PadLeft(2,'0'),hdnTimeElapsed0minute.Value.PadLeft(2,'0'));
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

                    PatientVisitNote entityEmergencyCaseNote = BusinessLayer.GetPatientVisitNoteList(string.Format("VisitID = {0} AND GCPatientNoteType = '{1}'", AppSession.RegisteredPatient.VisitID, Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE), ctx).FirstOrDefault();
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
                        entityEmergencyCaseNote.GCPatientNoteType = Constant.PatientVisitNotes.EMERGENCY_CASE_NOTE;
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
    }
}
