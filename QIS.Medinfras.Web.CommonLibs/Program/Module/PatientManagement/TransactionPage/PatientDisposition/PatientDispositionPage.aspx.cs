using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PatientDispositionPage : BasePagePatientPageList
    {
        protected int PageCount = 1;

        public override string OnGetMenuCode()
        {
            string id = Page.Request.QueryString["id"];
            //switch (hdnDepartmentID.Value)
            //{
            //    case Constant.Facility.EMERGENCY: return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            //    case Constant.Facility.INPATIENT: return Constant.MenuCode.Inpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            //    case Constant.Facility.OUTPATIENT: return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            //    default: return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            //} 
            switch (id)
            {
                case "er": return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                case "ip": return Constant.MenuCode.Inpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                case "op": return Constant.MenuCode.Outpatient.TRANSACTION_PAGE_PATIENT_DISPOSITION;
                case "erpd": return Constant.MenuCode.EmergencyCare.PATIENT_PAGE_TRANSACTION_DISPOTITION;
                default: return Constant.MenuCode.EmergencyCare.TRANSACTION_PAGE_PATIENT_DISPOSITION;
            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowEdit = IsAllowDelete = false;
        }

        protected string RegistrationDateTime = "";

        protected override void InitializeDataControl()
        {
            hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}','{4}') AND IsActive = 1 AND IsDeleted = 0",
                                                                    Constant.StandardCode.PATIENT_OUTCOME,
                                                                    Constant.StandardCode.DISCHARGE_ROUTINE,
                                                                    Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL,
                                                                    Constant.StandardCode.ROOM_TYPE,
                                                                    Constant.StandardCode.REFERRAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.StandardCodeID != Constant.PatientOutcome.DEAD_AFTER_48 && p.StandardCodeID != Constant.PatientOutcome.DEAD_BEFORE_48).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcomeDead, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && (p.StandardCodeID == Constant.PatientOutcome.DEAD_AFTER_48 || p.StandardCodeID == Constant.PatientOutcome.DEAD_BEFORE_48)).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRoomType, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.ROOM_TYPE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstStandardCode.Where(p => p.StandardCodeID == Constant.Referrer.FASKES || p.StandardCodeID == Constant.Referrer.RUMAH_SAKIT).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.SA0116,
                                                        Constant.SettingParameter.SA0138
                                                        )); 
            
            hdnIsValidateParamedicSchedule.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0116).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMJKN.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcomeDead, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            string filterVisit = string.Format("VisitID = {0}", AppSession.RegisteredPatient.VisitID);
            ConsultVisit entity = BusinessLayer.GetConsultVisitList(filterVisit).FirstOrDefault();
            hdnDepartmentID.Value = AppSession.RegisteredPatient.DepartmentID;
            hdnRegistrationID.Value = Convert.ToString(entity.RegistrationID);
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnVisitStatus.Value = entity.GCVisitStatus;

            PatientReferral entityPatientReferral = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityPatientReferral != null)
            {
                txtDiagnosisText.Text = entityPatientReferral.DiagnosisText;
                txtMedicalResumeText.Text = entityPatientReferral.MedicalResumeText;
                txtPlanningResumeText.Text = entityPatientReferral.PlanningResumeText;
            }


            txtReferralToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferralToTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            PatientReferralExternal entityPatientReferralExternal = BusinessLayer.GetPatientReferralExternalList(string.Format("VisitID = {0} ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityPatientReferralExternal != null)
            {
                if (!string.IsNullOrEmpty(entityPatientReferralExternal.ReferralToDiagnoseID))
                {
                    hdnDiagnoseID.Value = entityPatientReferralExternal.ReferralToDiagnoseID;
                    Diagnose entityDiagnose = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID = '{0}'", entityPatientReferralExternal.ReferralToDiagnoseID)).FirstOrDefault();
                    ledDiagnose.Text = string.Format("{0} ({1})", entityDiagnose.DiagnoseName, entityDiagnose.DiagnoseID);
                }
                txtReferralToDate.Text = entityPatientReferralExternal.ReferralToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReferralToTime.Text = entityPatientReferralExternal.ReferralToTime;
                txtDiagnosisText.Text = entityPatientReferralExternal.ReferralToDiagnoseText;
                txtMedicalResumeText.Text = entityPatientReferralExternal.ReferralToMedicalResumeText;
                txtPlanningResumeText.Text = entityPatientReferralExternal.ReferralToPlanningText;
            }

            EntityToControl(entity);
        }

        protected string GetPatientOutcomeDeadBefore48()
        {
            return Constant.PatientOutcome.DEAD_BEFORE_48;
        }

        protected string GetPatientOutcomeDeadAfter48()
        {
            return Constant.PatientOutcome.DEAD_AFTER_48;
        }

        private void EntityToControl(ConsultVisit entity)
        {
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48)
            {
                chkIsDead.Checked = true;
                cboPatientOutcomeDead.Value = entity.GCDischargeCondition;
            }
            else
            {
                cboPatientOutcome.Value = entity.GCDischargeCondition;
            }
            cboDischargeRoutine.Value = entity.GCDischargeMethod;
            if (!entity.DischargeDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            if (!string.IsNullOrEmpty(entity.DischargeTime))
            {
                txtDischargeTime.Text = entity.DischargeTime;
            }
            else
            {
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (!entity.ReferralDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT); ;
            }
            else
            {
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            hdnLOSInDay.Value = entity.LOSInDay.ToString();
            hdnLOSInHour.Value = entity.LOSInHour.ToString();
            hdnLOSInMinute.Value = entity.LOSInMinute.ToString();
            cboDischargeReason.Value = entity.GCReferralDischargeReason;
            txtDischargeOtherReason.Text = entity.ReferralDischargeReasonOther;
            txtDischargeRemarks.Text = entity.DischargeRemarks;

            hdnParamedicID2.Value = entity.ReferralPhysicianID.ToString();
            if (hdnParamedicID2.Value != "0" && hdnParamedicID2.Value != "")
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                } 
            }

            if (!string.IsNullOrEmpty(entity.GCRoomType))
                cboRoomType.Value = entity.GCRoomType;

            if (!string.IsNullOrEmpty(entity.HospitalizationIndication))
                txtHospitalizedIndication.Text = entity.HospitalizationIndication;

            chkIsPreventiveCare.Checked = entity.IsPreventiveCare;
            chkIsCurativeCare.Checked = entity.IsCurativeCare;
            chkIsRehabilitationCare.Checked = entity.IsRehabilitationCare;
            chkIsPalliativeCare.Checked = entity.IsPalliativeCare;

            if (entity.ReferralUnitID != null && entity.ReferralUnitID != 0)
            {
                cboClinic.Value = entity.ReferralUnitID.ToString();
                if (entity.ReferralPhysicianID != 0 && entity.ReferralPhysicianID != null)
                {
                    ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                    if (oParamedic != null)
                    {
                        hdnPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                        txtPhysicianCode.Text = oParamedic.ParamedicCode;
                        txtPhysicianName.Text = oParamedic.FullName;
                    }
                }
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            if (entity.ReferralTo != 0)
            {
                vReferrer oReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0}", Convert.ToInt32(entity.ReferralTo))).FirstOrDefault();
                if (oReferrer != null)
                {
                    hdnReferrerID.Value = oReferrer.BusinessPartnerID.ToString();
                    cboReferrerGroup.Value = oReferrer.GCReferrerGroup;
                    hdnGCReferrerGroup.Value = oReferrer.GCReferrerGroup;
                    txtReferrerCode.Text = oReferrer.BusinessPartnerCode;
                    txtReferrerName.Text = oReferrer.BusinessPartnerName;
                    cboDischargeReason.Value = entity.GCReferralDischargeReason;
                    txtDischargeOtherReason.Text = entity.ReferralDischargeReasonOther;
                }
            }

            if (entity.VisitID != null)
            {
                Registration oRegistration = BusinessLayer.GetRegistration((Int32)entity.RegistrationID);
                if (oRegistration != null)
                {
                    chkIsTransferredToInpatient.Checked = oRegistration.IsTransferredToInpatient;
                }
            }
        }

        private void ReferralControlToEntity(PatientReferral entity)
        {
            entity.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);            
            if (rblReferralType.SelectedValue == "1")
                entity.GCRefferalType = "X075^04";
            else
                entity.GCRefferalType = "X075^05";
            entity.ToHealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
            if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
            {
                entity.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
            }
            entity.DiagnosisText = txtDiagnosisText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
        }

        private void AppointmentRequestControlToEntity(AppointmentRequest entity)
        {
            entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
            if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
            {
                entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
            }
            entity.VisitTypeID = 1;
            entity.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate.Text);
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "save")
            {
                IDbContext ctx = DbFactory.Configure(true);
                RegistrationDao registrationDao = new RegistrationDao(ctx);
                ConsultVisitDao entityDao = new ConsultVisitDao(ctx);
                PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
                PatientReferralExternalDao entityReferralExternalDao = new PatientReferralExternalDao(ctx);
                AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);

                try
                {
                    Registration entityRegis = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                    //TODO : Validasi Tagihan Pasien
                    if (cboDischargeRoutine.Value.ToString() != Constant.DischargeMethod.DISCHARGED_TO_WARD)
                    {
                        bool isCheckOutstanding = true;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_VALIDASI_TAGIHAN_KETIKA_PULANG);
                        if (oParam != null)
                            isCheckOutstanding = oParam.ParameterValue == "1" ? true : false;

                        if (isCheckOutstanding)
                        {
                            List<vRegistrationOutstanding> lstOutstanding = BusinessLayer.GetvRegistrationOutstandingList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID));
                            if (lstOutstanding.Count > 0)
                            {
                                errMessage = "Pasien tidak dapat dipulangkan karena masih memiliki sisa tagihan";
                                return false;
                            }
                        }
                    }
                    List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                    ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                    if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() > 0)
                    {
                        entity.GCVisitStatus = Constant.VisitStatus.DISCHARGED;
                    }
                    if (chkIsDead.Checked)
                    {
                        entity.GCDischargeCondition = cboPatientOutcomeDead.Value.ToString();
                    }
                    else
                    {
                        entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                    }
                    entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                    entity.DischargeRemarks = txtDischargeRemarks.Text;
                    entity.AdminDischargedBy = AppSession.UserLogin.UserID;
                    entity.AdminDischargedDate = DateTime.Now;
                    entity.RoomDischargedBy = AppSession.UserLogin.UserID;
                    entity.RoomDischargeDateTime = DateTime.Now;
                    entity.ActualDischargeDateTime = DateTime.Now;
                    //if (AppSession.UserLogin.ParamedicID != null && AppSession.UserLogin.ParamedicID != 0)
                    //{
                    //    entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                    //    entity.PhysicianDischargedDate = DateTime.Now;
                    //}
                    entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                    entity.DischargeTime = txtDischargeTime.Text;
                    entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                    entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                    entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                    if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                    {
                        if (hdnReferrerID.Value != null && hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                        {
                            entity.ReferralTo = Convert.ToInt32(hdnReferrerID.Value);
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
                            entityReferralExternal.ReferralToDate = Helper.GetDatePickerValue(txtReferralToDate.Text);
                            entityReferralExternal.ReferralToTime = txtReferralToTime.Text;
                            entityReferralExternal.ReferralToDiagnoseText = txtDiagnosisText.Text;
                            entityReferralExternal.ReferralToMedicalResumeText = txtMedicalResumeText.Text;
                            entityReferralExternal.ReferralToPlanningText = txtPlanningResumeText.Text;
                            entityReferralExternal.ReferralToDiagnoseID = hdnDiagnoseID.Value;

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
                    }
                    else if (entity.GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                    {
                        if (hdnParamedicID2.Value != "0" && hdnParamedicID2.Value != "")
                        {
                            entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                        }
                        else
                        {
                            errMessage = "Mohon isi DPJP!";
                            result = false;
                        }
                        entity.IsRefferralProcessed = false;

                        if (cboRoomType.Value != null)
                            entity.GCRoomType = cboRoomType.Value.ToString();
                        if (!string.IsNullOrEmpty(txtHospitalizedIndication.Text))
                            entity.HospitalizationIndication = txtHospitalizedIndication.Text;

                        entity.IsRefferralProcessed = false;

                        entity.IsPreventiveCare = chkIsPreventiveCare.Checked;
                        entity.IsCurativeCare = chkIsCurativeCare.Checked;
                        entity.IsRehabilitationCare = chkIsRehabilitationCare.Checked;
                        entity.IsPalliativeCare = chkIsPalliativeCare.Checked;
                    }
                    else if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                    {
                        bool isValid = true;
                        if (hdnIsValidateParamedicSchedule.Value == "1")
                        {
                            if (rblReferralType.SelectedValue == "2")
                            {
                                if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
                                {
                                    vParamedicSchedule obj = null;
                                    vParamedicScheduleDate objSchDate = null;
                                    if (!Helper.ValidateParamedicSchedule(obj, objSchDate, Convert.ToInt32(hdnPhysicianID.Value), Convert.ToInt32(cboClinic.Value), Helper.GetDatePickerValue(txtAppointmentDate.Text), Constant.Facility.OUTPATIENT, ref errMessage))
                                    {
                                        isValid = false;
                                    }
                                }
                            }
                        }

                        if (isValid)
                        {
                            entity.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                            entity.ReferralPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                            entity.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                            entity.IsRefferralProcessed = false;

                            PatientReferral entityReff = new PatientReferral();
                            entityReff.VisitID = entity.VisitID;
                            entityReff.ReferralDate = entity.VisitDate;
                            entityReff.FromPhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
                            ReferralControlToEntity(entityReff);

                            if (rblReferralType.SelectedValue == "2")
                            {
                                Registration oReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value)).FirstOrDefault();
                                AppointmentRequest entityApmReq = new AppointmentRequest();
                                AppointmentRequestControlToEntity(entityApmReq);
                                entityApmReq.CreatedBy = AppSession.UserLogin.UserID;
                                entityApmReq.CreatedDate = DateTime.Now;
                                entityApmReq.GCCustomerType = oReg.GCCustomerType;
                                if (oReg.GCCustomerType == Constant.CustomerType.PERSONAL)
                                {
                                    entityApmReq.BusinessPartnerID = 1;
                                    entityApmReq.ContractID = null;
                                    entityApmReq.CoverageTypeID = null;
                                    entityApmReq.CoverageLimitAmount = 0;
                                    entityApmReq.IsCoverageLimitPerDay = false;
                                    entityApmReq.GCTariffScheme = null;
                                    entityApmReq.IsControlClassCare = false;
                                    entityApmReq.EmployeeID = null;
 
                                }
                                else 
                                {
                                    entityApmReq.BusinessPartnerID = oReg.BusinessPartnerID;
                                    string filterexpressionContract = string.Format("BusinessPartnerID = {0} AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0", entityApmReq.BusinessPartnerID); 
                                    CustomerContract oCsContract = BusinessLayer.GetCustomerContractList(filterexpressionContract).FirstOrDefault();
                                    if (oCsContract != null)
                                    {
                                       
                                        string filtexexpres2 = string.Format(" {0} AND ContractNo = '{1}' ", filterexpressionContract, oCsContract.ContractNo);
                                        vCustomerContract oVcsContract = BusinessLayer.GetvCustomerContractList(filtexexpres2).FirstOrDefault();
                                        if (oVcsContract != null) {
                                            entityApmReq.ContractID = oVcsContract.ContractID;
                                        }
                                    }
                                    else {
                                        entityApmReq.ContractID = null;
                                    }

                                    entityApmReq.IsControlClassCare = oReg.IsControlClassCare;
                                    entityApmReq.IsCoverageLimitPerDay = oReg.IsCoverageLimitPerDay;
                                    entityApmReq.GCTariffScheme = oReg.GCTariffScheme;
                                    entityApmReq.CoverageLimitAmount = oReg.CoverageLimitAmount;
                                    entityApmReq.CoverageTypeID = oReg.CoverageTypeID;
                                    entityApmReq.EmployeeID = oReg.EmployeeID;
                                }
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                int apmReqID = entityApmReqDao.InsertReturnPrimaryKeyID(entityApmReq);
                                entityReff.AppointmentRequestID = apmReqID;
                            }

                            entityReferralDao.Insert(entityReff);
                        }
                        else
                        {
                            ctx.RollBackTransaction();
                            return false;
                        }
                    }

                    if (cboDischargeReason.Value != null)
                    {
                        entity.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                        if (cboDischargeReason.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                        {
                            entity.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                        }
                    }

                    if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                    {
                        if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.CLOSED).Count() > 0)
                        {
                            entityRegis.IsTransferredToInpatient = chkIsTransferredToInpatient.Checked;
                            entityRegis.TransferredToInpatientBy = AppSession.UserLogin.UserID;
                            entityRegis.TransferredToInpatientDatetime = DateTime.Now;
                            entityRegis.GCRegistrationStatus = Constant.VisitStatus.DISCHARGED;
                            entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityRegis);
                        }
                    }
                    errMessage = entity.GCVisitStatus;

                    entityDao.Update(entity);
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    result = false;
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }
            }
            return result;
        }
    }
}