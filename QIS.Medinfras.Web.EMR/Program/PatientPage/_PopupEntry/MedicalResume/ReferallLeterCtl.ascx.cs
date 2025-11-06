using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using System.IO;

namespace QIS.Medinfras.Web.EMR.Program.PatientPage
{
    public partial class ReferallLeterCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(String.Format("(StandardCodeID IN ('{0}','{1}') OR ParentID IN ('{2}','{3}')) AND IsActive = 1", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT, Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL));

            Methods.SetComboBoxField<StandardCode>(cboReferrerGroupCtl, lstStandardCode.Where(t => t.StandardCodeID == Constant.Referrer.FASKES || t.StandardCodeID == Constant.Referrer.RUMAH_SAKIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcomeCtl, lstStandardCode.Where(t => t.ParentID == Constant.StandardCode.PATIENT_OUTCOME).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReasonCtl, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");

            txtReferralToDateCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferralToTimeCtl.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDateOfDeathCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeOfDeathCtl.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtDischargeDateCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeTimeCtl.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            hdnVisitIDCtl.Value = param;
            ConsultVisit visit = BusinessLayer.GetConsultVisit(Convert.ToInt32(param));
            hdnRegistrationIDCtl.Value = visit.RegistrationID.ToString();
            EntityToControl(visit);
        }

        protected override void OnControlEntrySetting()
        {
            //SetControlEntrySetting(txtMedicalResumeDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            //SetControlEntrySetting(txtMedicalResumeTime, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitTime));
            SetControlEntrySetting(cboPatientOutcomeCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboReferrerGroupCtl, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDischargeReasonCtl, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
            PatientReferralExternalDao entityReferralExternalDao = new PatientReferralExternalDao(ctx);
            AppointmentRequestDao entityAppointmentRequestDao = new AppointmentRequestDao(ctx);
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    ConsultVisit entity = visitDao.Get(Convert.ToInt32(hdnVisitIDCtl.Value));

                    Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                    entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityRegis.IsComplexVisit = chkIsComplexVisitCtl.Checked;
                    registrationDao.Update(entityRegis);

                    entity.GCDischargeCondition = cboPatientOutcomeCtl.Value.ToString();
                    entity.GCDischargeMethod = Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER;
                    entity.PhysicianDischargeOrderDate = Helper.GetDatePickerValue(txtDischargeDateCtl);
                    entity.PhysicianDischargeOrderTime = txtDischargeTimeCtl.Text;
                    entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                    entity.PhysicianDischargedDate = DateTime.Now;
                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDayCtl.Value);
                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHourCtl.Value);
                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinuteCtl.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (hdnReferrerIDCtl.Value != null && hdnReferrerIDCtl.Value != "" && hdnReferrerIDCtl.Value != "0")
                    {
                        entity.ReferralTo = Convert.ToInt32(hdnReferrerIDCtl.Value);
                        bool isExist = true;
                        PatientReferralExternal entityReferralExternal = BusinessLayer.GetPatientReferralExternalList(String.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                        if (entityReferralExternal == null)
                        {
                            isExist = false;
                            entityReferralExternal = new PatientReferralExternal();
                            entityReferralExternal.VisitID = entity.VisitID;
                            entityReferralExternal.ReferralToNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.REFERRAL_TO_NO, DateTime.Now, ctx);
                            entityReferralExternal.IsDeleted = false;
                            entityReferralExternal.CreatedBy = AppSession.UserLogin.UserID;
                            entityReferralExternal.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            entityReferralExternal.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityReferralExternal.LastUpdatedDate = DateTime.Now;
                        }
                        entityReferralExternal.ReferralToDate = Helper.GetDatePickerValue(txtReferralToDateCtl.Text);
                        entityReferralExternal.ReferralToTime = txtReferralToTimeCtl.Text;
                        entityReferralExternal.ReferralToDiagnoseText = txtDiagnosisTextCtl.Text;
                        entityReferralExternal.ReferralToMedicalResumeText = txtMedicalResumeTextCtl.Text;
                        entityReferralExternal.ReferralToPlanningText = txtPlanningResumeTextCtl.Text;
                        entityReferralExternal.ReferralToDiagnoseID = hdnDiagnoseIDCtl.Value;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        if (!isExist)
                        {
                            entityReferralExternalDao.InsertReturnPrimaryKeyID(entityReferralExternal);
                        }
                        else
                        {
                            entityReferralExternalDao.Update(entityReferralExternal);
                        }
                    }

                    if (cboDischargeReasonCtl.Value != null)
                    {
                        entity.GCReferralDischargeReason = cboDischargeReasonCtl.Value.ToString();
                        if (cboDischargeReasonCtl.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                        {
                            entity.ReferralDischargeReasonOther = txtDischargeOtherReasonCtl.Text;
                        }
                    }

                    if (cboPatientOutcomeCtl.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcomeCtl.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                    {
                        entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeathCtl);
                        entity.TimeOfDeath = txtTimeOfDeathCtl.Text;

                        entity.ReferralUnitID = null;
                        entity.ReferralPhysicianID = null;
                        entity.ReferralDate = Helper.InitializeDateTimeNull();
                        entity.IsRefferralProcessed = false;

                        //Update Patient Death Status
                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (oPatient != null)
                        {
                            oPatient.IsAlive = false;
                            oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeathCtl);
                        }
                        else
                        {
                            oPatient.IsAlive = true;
                            oPatient.DateOfDeath = DateTime.MinValue;
                        }
                        oPatient.LastVisitDate = AppSession.RegisteredPatient.VisitDate;
                        oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDao.Update(oPatient);
                    }

                    visitDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            IDbContext ctx = DbFactory.Configure(true);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
            PatientReferralExternalDao entityReferralExternalDao = new PatientReferralExternalDao(ctx);
            AppointmentRequestDao entityAppointmentRequestDao = new AppointmentRequestDao(ctx);
            bool result = true;
            try
            {
                if (IsValid(ref errMessage))
                {
                    ConsultVisit entity = visitDao.Get(Convert.ToInt32(hdnVisitIDCtl.Value));

                    Registration entityRegis = registrationDao.Get(entity.RegistrationID);
                    entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;

                    entityRegis.IsComplexVisit = chkIsComplexVisitCtl.Checked;
                    registrationDao.Update(entityRegis);

                    entity.GCDischargeCondition = cboPatientOutcomeCtl.Value.ToString();
                    entity.GCDischargeMethod = Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER;
                    entity.PhysicianDischargeOrderDate = Helper.GetDatePickerValue(txtDischargeDateCtl);
                    entity.PhysicianDischargeOrderTime = txtDischargeTimeCtl.Text;
                    entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                    entity.PhysicianDischargedDate = DateTime.Now;
                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDayCtl.Value);
                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHourCtl.Value);
                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinuteCtl.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (hdnReferrerIDCtl.Value != null && hdnReferrerIDCtl.Value != "" && hdnReferrerIDCtl.Value != "0")
                    {
                        entity.ReferralTo = Convert.ToInt32(hdnReferrerIDCtl.Value);
                        bool isExist = true;
                        PatientReferralExternal entityReferralExternal = BusinessLayer.GetPatientReferralExternalList(String.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID), ctx).FirstOrDefault();
                        if (entityReferralExternal == null)
                        {
                            isExist = false;
                            entityReferralExternal = new PatientReferralExternal();
                            entityReferralExternal.VisitID = entity.VisitID;
                            entityReferralExternal.ReferralToNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.REFERRAL_TO_NO, DateTime.Now, ctx);
                            entityReferralExternal.IsDeleted = false;
                            entityReferralExternal.CreatedBy = AppSession.UserLogin.UserID;
                            entityReferralExternal.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            entityReferralExternal.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityReferralExternal.LastUpdatedDate = DateTime.Now;
                        }
                        entityReferralExternal.ReferralToDate = Helper.GetDatePickerValue(txtReferralToDateCtl.Text);
                        entityReferralExternal.ReferralToTime = txtReferralToTimeCtl.Text;
                        entityReferralExternal.ReferralToDiagnoseText = txtDiagnosisTextCtl.Text;
                        entityReferralExternal.ReferralToMedicalResumeText = txtMedicalResumeTextCtl.Text;
                        entityReferralExternal.ReferralToPlanningText = txtPlanningResumeTextCtl.Text;
                        entityReferralExternal.ReferralToDiagnoseID = hdnDiagnoseIDCtl.Value;

                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();

                        if (!isExist)
                        {
                            entityReferralExternalDao.InsertReturnPrimaryKeyID(entityReferralExternal);
                        }
                        else
                        {
                            entityReferralExternalDao.Update(entityReferralExternal);
                        }
                    }

                    if (cboDischargeReasonCtl.Value != null)
                    {
                        entity.GCReferralDischargeReason = cboDischargeReasonCtl.Value.ToString();
                        if (cboDischargeReasonCtl.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                        {
                            entity.ReferralDischargeReasonOther = txtDischargeOtherReasonCtl.Text;
                        }
                    }

                    if (cboPatientOutcomeCtl.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcomeCtl.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                    {
                        entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeathCtl);
                        entity.TimeOfDeath = txtTimeOfDeathCtl.Text;

                        entity.ReferralUnitID = null;
                        entity.ReferralPhysicianID = null;
                        entity.ReferralDate = Helper.InitializeDateTimeNull();
                        entity.IsRefferralProcessed = false;

                        //Update Patient Death Status
                        Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                        if (oPatient != null)
                        {
                            oPatient.IsAlive = false;
                            oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeathCtl);
                        }
                        else
                        {
                            oPatient.IsAlive = true;
                            oPatient.DateOfDeath = DateTime.MinValue;
                        }
                        oPatient.LastVisitDate = AppSession.RegisteredPatient.VisitDate;
                        oPatient.LastUpdatedBy = AppSession.UserLogin.UserID;
                        patientDao.Update(oPatient);
                    }

                    visitDao.Update(entity);
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    result = false;
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                result = false;
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }

        private void EntityToControl(ConsultVisit entity)
        {
            PatientReferral entityPatientReferral = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} ORDER BY ID DESC", entity.VisitID)).FirstOrDefault();
            if (entityPatientReferral != null)
            {
                txtDiagnosisTextCtl.Text = entityPatientReferral.DiagnosisText;
                txtMedicalResumeTextCtl.Text = entityPatientReferral.MedicalResumeText;
                txtPlanningResumeTextCtl.Text = entityPatientReferral.PlanningResumeText;
            }
            
            hdnReferrerIDCtl.Value = entity.ReferralTo.ToString();

            PatientReferralExternal entityPatientReferralExternal = BusinessLayer.GetPatientReferralExternalList(string.Format("VisitID = {0} ORDER BY ID DESC", entity.VisitID)).FirstOrDefault();
            if (entityPatientReferralExternal != null)
            {
                if (!string.IsNullOrEmpty(entityPatientReferralExternal.ReferralToDiagnoseID))
                {
                    hdnDiagnoseIDCtl.Value = entityPatientReferralExternal.ReferralToDiagnoseID;
                    Diagnose entityDiagnose = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID = '{0}'", entityPatientReferralExternal.ReferralToDiagnoseID)).FirstOrDefault();
                    ledDiagnoseCtl.Text = string.Format("{0} ({1})", entityDiagnose.DiagnoseName, entityDiagnose.DiagnoseID);
                }
                txtReferralToDateCtl.Text = entityPatientReferralExternal.ReferralToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReferralToTimeCtl.Text = entityPatientReferralExternal.ReferralToTime;
                txtDiagnosisTextCtl.Text = entityPatientReferralExternal.ReferralToDiagnoseText;
                txtMedicalResumeTextCtl.Text = entityPatientReferralExternal.ReferralToMedicalResumeText;
                txtPlanningResumeTextCtl.Text = entityPatientReferralExternal.ReferralToPlanningText;
            }

            cboPatientOutcomeCtl.Value = entity.GCDischargeCondition;

            if (entity.PhysicianDischargeOrderDate != null && entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtDischargeDateCtl.Text = entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTimeCtl.Text = entity.PhysicianDischargeOrderTime;
            }
            else
            {
                txtDischargeDateCtl.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTimeCtl.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            {
                txtDateOfDeathCtl.Text = entity.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeathCtl.Text = entity.TimeOfDeath;
            }

            if (entity.ReferralTo != 0)
            {
                vReferrer oReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0}", Convert.ToInt32(entity.ReferralTo))).FirstOrDefault();
                if (oReferrer != null)
                {
                    cboReferrerGroupCtl.Value = oReferrer.GCReferrerGroup;
                    hdnGCReferrerGroupCtl.Value = oReferrer.GCReferrerGroup;
                    txtReferrerCodeCtl.Text = oReferrer.BusinessPartnerCode;
                    txtReferrerNameCtl.Text = oReferrer.BusinessPartnerName;
                    cboDischargeReasonCtl.Value = entity.GCReferralDischargeReason;
                    txtDischargeOtherReasonCtl.Text = entity.ReferralDischargeReasonOther;
                }
            }

            if (entity.VisitID != null)
            {
                Registration oRegistration = BusinessLayer.GetRegistration((Int32)entity.RegistrationID);
                if (oRegistration != null)
                {
                    chkIsComplexVisitCtl.Checked = oRegistration.IsComplexVisit;
                }
            }
        }

        private bool IsValid(ref string errMessage)
        {
            bool isValid = false;
            string filterParamedic = string.Format("RegistrationID = '{0}' AND IsDeleted = 0 AND GCParamedicRole != '{1}'", hdnRegistrationIDCtl.Value, Constant.ParamedicRole.DPJP);
            List<ParamedicTeam> lstParamedic = BusinessLayer.GetParamedicTeamList(filterParamedic);

            foreach (ParamedicTeam e in lstParamedic)
            {
                if (e.ParamedicID == AppSession.UserLogin.ParamedicID)
                {
                    isValid = true;
                }
            }

            if (!isValid)
            {
                errMessage = "Proses ini hanya bisa dilakukan oleh dokter RMO saja.";
            }

            return isValid;
        }
    }
}