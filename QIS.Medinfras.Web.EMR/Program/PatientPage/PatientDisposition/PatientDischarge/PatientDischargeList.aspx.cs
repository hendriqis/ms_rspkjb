using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using System.Text;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Data;

namespace QIS.Medinfras.Web.EMR.Program
{
    public partial class PatientDischargeList : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.EMR.PATIENT_DISCHARGE;
        }

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}')", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.ROOM_TYPE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL);
            hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')",
                                                        AppSession.UserLogin.HealthcareID,
                                                        Constant.SettingParameter.SA0116,
                                                        Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE,
                                                        Constant.SettingParameter.EM_Display_Next_Visit_Schedule,
                                                        Constant.SettingParameter.EM_IS_USING_WITHOUT_INITIAL_ASSESSMENT_FOR_PHYSICIAN_DISCHARGE,
                                                        Constant.SettingParameter.SA0138,
                                                        Constant.SettingParameter.EM0087
                                                    ));
            hdnIsValidateParamedicSchedule.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0116).FirstOrDefault().ParameterValue;
            hdnIsParamedicInRegistrationUseSchedule.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).ParameterValue;
            hdnIsAllowCreateAppointment.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_Display_Next_Visit_Schedule).ParameterValue;
            hdnIsUsingFlagWithoutInitialPhysicianAssessment.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM_IS_USING_WITHOUT_INITIAL_ASSESSMENT_FOR_PHYSICIAN_DISCHARGE).ParameterValue;
            hdnIsBridgingToMJKN.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.SA0138).ParameterValue;
            hdnIsDefaultDischargeConditionAndMethod.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.EM0087).ParameterValue;

            trIsWithoutInitialPhysicianAssessment.Attributes.Add("style", "display:none");
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                if (hdnIsUsingFlagWithoutInitialPhysicianAssessment.Value == "1")
                {
                    trIsWithoutInitialPhysicianAssessment.Attributes.Remove("style");
                }
            }

            if (hdnIsAllowCreateAppointment.Value == "0")
            {
                filterExpression += string.Format(" AND StandardCodeID NOT IN ('{0}','{1}','{2}')", Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT, Constant.DischargeMethod.TRANSFERED_TO_UPH, Constant.DischargeMethod.TRANSFERED_TO_ODS);
            }

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstStandardCode2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));

            List<StandardCode> lstDischargeOutcome = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.StandardCodeID != Constant.DischargeMethod.DISCHARGED_TO_WARD && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList(), "StandardCodeName", "StandardCodeID");
            }
            else
            {
                Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList(), "StandardCodeName", "StandardCodeID");
            }
            List<StandardCode> lstRoomType = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.ROOM_TYPE && p.IsActive == true).ToList();

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstDischargeOutcome, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboRoomType, lstRoomType, "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstStandardCode2.ToList(), "StandardCodeName", "StandardCodeID");

            List<vHealthcareServiceUnitCustom> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));
            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            if (hdnIsDefaultDischargeConditionAndMethod.Value == "0")
            {
                if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
                {
                    cboPatientOutcome.Value = Constant.PatientOutcome.BELUM_SEMBUH;
                    cboDischargeRoutine.Value = Constant.DischargeMethod.ATAS_PERSETUJUAN;
                }
            }

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            hdnRegistrationIDMainPage.Value = entity.RegistrationID.ToString();
            hdnVisitID.Value = entity.VisitID.ToString();
            hdnVisitStatus.Value = entity.GCVisitStatus;

            ChiefComplaint oChiefComplaint = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (oChiefComplaint != null)
            {
                hdnIsHasChiefComplaint.Value = "1";
                txtHospitalizedIndication.Text = oChiefComplaint.MedicalProblem;
            }
            else
            {
                hdnIsHasChiefComplaint.Value = "0";
                txtHospitalizedIndication.Text = "";
            }

            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.EMERGENCY || AppSession.RegisteredPatient.DepartmentID == Constant.Facility.OUTPATIENT)
            {
                if (!string.IsNullOrEmpty(entity.StartServiceTime))
                {
                    RegistrationDateTime = entity.StartServiceDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += entity.StartServiceTime.Replace(":", "");
                }
                else
                {
                    RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                    RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
                }
            }
            else
            {
                RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");
            }

            if (entity.GCDischargeCondition != "" && entity.GCDischargeMethod != "")
            {
                EntityToControl(entity);
            }
            else
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                txtReferralToDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReferralToTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            PatientReferral entityPatientReferral = BusinessLayer.GetPatientReferralList(string.Format("VisitID = {0} ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityPatientReferral != null)
            {
                txtDiagnosisText.Text = entityPatientReferral.DiagnosisText;
                txtMedicalResumeText.Text = entityPatientReferral.MedicalResumeText;
                txtPlanningResumeText.Text = entityPatientReferral.PlanningResumeText;
            }

            PatientReferralExternal entityPatientReferralExternal = BusinessLayer.GetPatientReferralExternalList(string.Format("VisitID = {0} ORDER BY ID DESC", AppSession.RegisteredPatient.VisitID)).FirstOrDefault();
            if (entityPatientReferralExternal != null)
            {
                if (!string.IsNullOrEmpty(entityPatientReferralExternal.ReferralToDiagnoseID))
                {
                    hdnDiagnosisID.Value = entityPatientReferralExternal.ReferralToDiagnoseID;
                    Diagnose entityDiagnose = BusinessLayer.GetDiagnoseList(string.Format("DiagnoseID = '{0}'", entityPatientReferralExternal.ReferralToDiagnoseID)).FirstOrDefault();
                    ledDiagnose.Text = string.Format("{0} ({1})", entityDiagnose.DiagnoseName, entityDiagnose.DiagnoseID);
                }
                txtReferralToDate.Text = entityPatientReferralExternal.ReferralToDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtReferralToTime.Text = entityPatientReferralExternal.ReferralToTime;
                txtDiagnosisText.Text = entityPatientReferralExternal.ReferralToDiagnoseText;
                txtMedicalResumeText.Text = entityPatientReferralExternal.ReferralToMedicalResumeText;
                txtPlanningResumeText.Text = entityPatientReferralExternal.ReferralToPlanningText;
            }

            DateTime selectedDate = DateTime.Now;

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            hdnDayNumber.Value = daynumber.ToString();

            if (entity.ReferralPhysicianID != null)
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnIPRegisteredPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                    hdnIPRegisteredPhysicianCode.Value = oParamedic.ParamedicCode;
                    hdnIPRegisteredPhysicianName.Value = oParamedic.FullName;
                }
                else
                {
                    hdnIPRegisteredPhysicianID.Value = "0";
                    txtParamedicCode.Text = string.Empty;
                    txtParamedicName.Text = string.Empty;
                    hdnIPRegisteredPhysicianCode.Value = string.Empty;
                    hdnIPRegisteredPhysicianName.Value = string.Empty;
                }

                if (!string.IsNullOrEmpty(entity.GCRoomType))
                    cboRoomType.Value = entity.GCRoomType;

                if (!string.IsNullOrEmpty(entity.HospitalizationIndication))
                    txtHospitalizedIndication.Text = entity.HospitalizationIndication;
            }

            List<vParamedicTeam> pTeamList = BusinessLayer.GetvParamedicTeamList(string.Format("RegistrationID = {0} AND IsDeleted = 0 AND IsSpecialist = 1", entity.RegistrationID));
            if (pTeamList.Count() == 1)
            {
                vParamedicTeam pTeam = pTeamList.FirstOrDefault();
                hdnIPRegisteredPhysicianID.Value = pTeam.ParamedicID.ToString();
                txtParamedicCode.Text = pTeam.ParamedicCode;
                txtParamedicName.Text = pTeam.ParamedicName;
                hdnIPRegisteredPhysicianCode.Value = pTeam.ParamedicCode;
                hdnIPRegisteredPhysicianName.Value = pTeam.ParamedicName;
            }
            else
            {
                if (entity.ReferralPhysicianID != null)
                {
                    ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                    if (oParamedic != null)
                    {
                        hdnIPRegisteredPhysicianID.Value = entity.ReferralPhysicianID.ToString();
                        txtParamedicCode.Text = oParamedic.ParamedicCode;
                        txtParamedicName.Text = oParamedic.FullName;
                        hdnIPRegisteredPhysicianCode.Value = oParamedic.ParamedicCode;
                        hdnIPRegisteredPhysicianName.Value = oParamedic.FullName;
                    }
                    else
                    {
                        hdnIPRegisteredPhysicianID.Value = "0";
                        txtParamedicCode.Text = string.Empty;
                        txtParamedicName.Text = string.Empty;
                        hdnIPRegisteredPhysicianCode.Value = string.Empty;
                        hdnIPRegisteredPhysicianName.Value = string.Empty;
                    }
                }
                else
                {
                    ParamedicMaster pMaster = BusinessLayer.GetParamedicMaster(Convert.ToInt32(entity.ParamedicID));
                    if (pMaster.IsSpecialist)
                    {
                        hdnIPRegisteredPhysicianID.Value = pMaster.ParamedicID.ToString();
                        txtParamedicCode.Text = pMaster.ParamedicCode;
                        txtParamedicName.Text = pMaster.FullName;
                        hdnIPRegisteredPhysicianCode.Value = pMaster.ParamedicCode;
                        hdnIPRegisteredPhysicianName.Value = pMaster.FullName;
                    }
                    else
                    {
                        hdnIPRegisteredPhysicianID.Value = "0";
                        txtParamedicCode.Text = string.Empty;
                        txtParamedicName.Text = string.Empty;
                        hdnIPRegisteredPhysicianCode.Value = string.Empty;
                        hdnIPRegisteredPhysicianName.Value = string.Empty;
                    }
                }
            }
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "getdaynumber")
            {
                DateTime selectedDate = DateTime.Now;
                if (rblReferralType.SelectedValue == "2")
                {
                    selectedDate = Helper.GetDatePickerValue(txtAppointmentDate);
                }

                //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
                int daynumber = (int)selectedDate.DayOfWeek;
                if (daynumber == 0)
                {
                    daynumber = 7;
                }

                hdnDayNumber.Value = daynumber.ToString();
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        private void EntityToControl(ConsultVisit entity)
        {
            cboPatientOutcome.Value = entity.GCDischargeCondition;
            string filterExpression = string.Format("ParentID IN ('{0}')", Constant.StandardCode.DISCHARGE_ROUTINE);
            if (hdnIsAllowCreateAppointment.Value == "0")
            {
                filterExpression += string.Format(" AND StandardCodeID NOT IN ('{0}','{1}','{2}')", Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT, Constant.DischargeMethod.TRANSFERED_TO_UPH, Constant.DischargeMethod.TRANSFERED_TO_ODS);
            }
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstDischargeMethod = lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && p.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            if (AppSession.RegisteredPatient.DepartmentID == Constant.Facility.INPATIENT)
            {
                lstDischargeMethod = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.StandardCodeID != Constant.DischargeMethod.DISCHARGED_TO_WARD && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            }
            else
            {
                lstDischargeMethod = lstStandardCode.Where(sc => sc.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE && sc.IsActive == true).OrderBy(lst => lst.TagProperty).ToList();
            }

            if (lstDischargeMethod.Where(t => t.StandardCodeID == entity.GCDischargeMethod).FirstOrDefault() != null)
            {
                cboDischargeRoutine.Value = entity.GCDischargeMethod;
            }

            txtDischargeRemarks.Text = entity.DischargeRemarks;

            if (entity.PhysicianDischargeOrderDate != null && entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) != Constant.ConstantDate.DEFAULT_NULL)
            {
                txtDischargeDate.Text = entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entity.PhysicianDischargeOrderTime;
            }
            else
            {
                txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (entity.ReferralUnitID != null)
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
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48)
            {
                txtDateOfDeath.Text = entity.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = entity.TimeOfDeath;
            }

            if (entity.ReferralTo != 0)
            {
                vReferrer oReferrer = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID = {0}", Convert.ToInt32(entity.ReferralTo))).FirstOrDefault();
                if (oReferrer != null)
                {
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
                    chkIsComplexVisit.Checked = oRegistration.IsComplexVisit;
                    chkIsTransferredToInpatient.Checked = oRegistration.IsTransferredToInpatient;
                }
            }

            chkIsPreventiveCare.Checked = entity.IsPreventiveCare;
            chkIsCurativeCare.Checked = entity.IsCurativeCare;
            chkIsRehabilitationCare.Checked = entity.IsRehabilitationCare;
            chkIsPalliativeCare.Checked = entity.IsPalliativeCare;
        }

        private void ReferralControlToEntity(PatientReferral entity)
        {
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ReferralDate = AppSession.RegisteredPatient.VisitDate;
            entity.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entity.FromPhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);
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

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            if (type == "process")
            {
                if (IsValidToDischarge())
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RegistrationDao registrationDao = new RegistrationDao(ctx);
                    ConsultVisitDao visitDao = new ConsultVisitDao(ctx);
                    PatientDao patientDao = new PatientDao(ctx);
                    PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
                    PatientReferralExternalDao entityReferralExternalDao = new PatientReferralExternalDao(ctx);
                    AppointmentRequestDao entityAppointmentRequestDao = new AppointmentRequestDao(ctx);

                    try
                    {
                        Registration entityRegis = registrationDao.Get(AppSession.RegisteredPatient.RegistrationID);
                        if (entityRegis.GCRegistrationStatus == Constant.VisitStatus.CHECKED_IN || entityRegis.GCRegistrationStatus == Constant.VisitStatus.RECEIVING_TREATMENT)
                        {
                            entityRegis.GCRegistrationStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                        }
                        entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;

                        entityRegis.IsComplexVisit = chkIsComplexVisit.Checked;
                        entityRegis.IsTransferredToInpatient = chkIsTransferredToInpatient.Checked;
                        entityRegis.TransferredToInpatientBy = AppSession.UserLogin.UserID;
                        entityRegis.TransferredToInpatientDatetime = DateTime.Now;
                        registrationDao.Update(entityRegis);

                        List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}' AND VisitID = {1}", entityRegis.RegistrationID, AppSession.RegisteredPatient.VisitID), ctx);
                        foreach (ConsultVisit entity in lstConsultVisit)
                        {
                            if (entity.GCVisitStatus == Constant.VisitStatus.CHECKED_IN || entity.GCVisitStatus == Constant.VisitStatus.RECEIVING_TREATMENT)
                            {
                                entity.GCVisitStatus = Constant.VisitStatus.PHYSICIAN_DISCHARGE;
                            }

                            entity.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                            entity.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                            entity.DischargeRemarks = txtDischargeRemarks.Text;
                            entity.PhysicianDischargeOrderDate = Helper.GetDatePickerValue(txtDischargeDate);
                            entity.PhysicianDischargeOrderTime = txtDischargeTime.Text;
                            entity.PhysicianDischargedBy = AppSession.UserLogin.UserID;
                            entity.PhysicianDischargedDate = DateTime.Now;
                            entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                            entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                            entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                            if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                            {
                                entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                                if (cboRoomType.Value != null)
                                    entity.GCRoomType = cboRoomType.Value.ToString();
                                if (!string.IsNullOrEmpty(txtHospitalizedIndication.Text))
                                    entity.HospitalizationIndication = txtHospitalizedIndication.Text;

                                entity.IsRefferralProcessed = false;

                                entity.IsPreventiveCare = chkIsPreventiveCare.Checked;
                                entity.IsCurativeCare = chkIsCurativeCare.Checked;
                                entity.IsRehabilitationCare = chkIsRehabilitationCare.Checked;
                                entity.IsPalliativeCare = chkIsPalliativeCare.Checked;
                                entity.IsHospitalizedByPatientRequest = chkIsHospitalizedByPatientRequest.Checked;
                            }
                            else if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
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

                                    #region Followup Visit or Referral
                                    int appReqID = 0;
                                    if (rblReferralType.SelectedValue == "2")
                                    {
                                        #region validate Visit Type
                                        int visitTypeID = 0;
                                        int hsuID = Convert.ToInt32(cboClinic.Value);
                                        int paramedicID = Convert.ToInt32(hdnPhysicianID.Value);

                                        List<ParamedicVisitType> lstVisitTypeParamedic = BusinessLayer.GetParamedicVisitTypeList(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1}", hsuID, paramedicID), ctx);
                                        vHealthcareServiceUnitCustom VisitTypeHealthcare = BusinessLayer.GetvHealthcareServiceUnitCustomList(string.Format("HealthcareServiceUnitID = {0}", hsuID), ctx).FirstOrDefault();
                                        if (lstVisitTypeParamedic.Count > 0)
                                        {
                                            visitTypeID = lstVisitTypeParamedic.FirstOrDefault().VisitTypeID;
                                        }
                                        else
                                        {
                                            if (VisitTypeHealthcare.IsHasVisitType)
                                            {
                                                List<vServiceUnitVisitType> lstServiceUnitVisitType = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", hsuID), ctx);
                                                if (lstServiceUnitVisitType.Count > 0)
                                                {
                                                    visitTypeID = lstServiceUnitVisitType.FirstOrDefault().VisitTypeID;
                                                }
                                                else
                                                {
                                                    List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"), ctx);
                                                    visitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                                }
                                            }
                                            else
                                            {
                                                List<VisitType> lstVisitType = BusinessLayer.GetVisitTypeList(string.Format("IsDeleted = 0"), ctx);
                                                visitTypeID = lstVisitType.FirstOrDefault().VisitTypeID;
                                            }
                                        }
                                        #endregion

                                        AppointmentRequest entityApmRequest = new AppointmentRequest();
                                        entityApmRequest.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                                        entityApmRequest.VisitID = entity.VisitID;
                                        entityApmRequest.MRN = AppSession.RegisteredPatient.MRN;
                                        entityApmRequest.HealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
                                        if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
                                        {
                                            entityApmRequest.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                        }
                                        entityApmRequest.VisitTypeID = visitTypeID;
                                        entityApmRequest.IsHasNoDate = false;
                                        entityApmRequest.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate.Text);
                                        entityApmRequest.IsDeleted = false;
                                        entityApmRequest.CreatedBy = AppSession.UserLogin.UserID;
                                        entityApmRequest.CreatedDate = DateTime.Now;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        entityApmRequest.AppointmentRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.OP_APPOINTMENT_REQUEST, DateTime.Now);

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();

                                        AppointmentRequest oApmreg = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentDate='{0}'  AND HealthcareServiceUnitID={1} AND ParamedicID='{2}' AND MRN='{3}'",
                                            entityApmRequest.AppointmentDate,
                                            entityApmRequest.HealthcareServiceUnitID,
                                            entityApmRequest.ParamedicID,
                                            entityApmRequest.MRN
                                            ), ctx).FirstOrDefault();

                                        if (oApmreg == null)
                                        {
                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            appReqID = entityAppointmentRequestDao.InsertReturnPrimaryKeyID(entityApmRequest);
                                        }
                                    }
                                    #endregion


                                    PatientReferral entityReff = new PatientReferral();
                                    ReferralControlToEntity(entityReff);
                                    if (appReqID > 0)
                                    {
                                        entityReff.AppointmentRequestID = appReqID;
                                    }
                                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityReferralDao.Insert(entityReff);
                                }
                                else
                                {
                                    ctx.RollBackTransaction();
                                    return false;
                                }
                            }
                            else if (cboDischargeRoutine.Value.ToString() == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
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

                            if (cboDischargeReason.Value != null)
                            {
                                entity.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                                if (cboDischargeReason.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                                {
                                    entity.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                                }
                            }

                            if (cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_BEFORE_48 || cboPatientOutcome.Value.ToString() == Constant.PatientOutcome.DEAD_AFTER_48)
                            {
                                entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                                entity.TimeOfDeath = txtTimeOfDeath.Text;

                                entity.ReferralUnitID = null;
                                entity.ReferralPhysicianID = null;
                                entity.ReferralDate = Helper.InitializeDateTimeNull();
                                entity.IsRefferralProcessed = false;

                                //Update Patient Death Status
                                Patient oPatient = patientDao.Get(AppSession.RegisteredPatient.MRN);
                                if (oPatient != null)
                                {
                                    oPatient.IsAlive = false;
                                    oPatient.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
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
                        }
                        result = true;

                        ctx.CommitTransaction();

                        if (AppSession.SA0137 == "1")
                        {
                            if (AppSession.SA0133 == Constant.CenterBackConsumerAPI.MEDINFRAS_EMR_V1)
                            {
                                BridgingToMedinfrasV1(entityRegis.RegistrationNo);
                            }
                        }

                        if (hdnIsBridgingToMJKN.Value == "1")
                        {
                            BusinessLayer.OnInsertBPJSTaskLog(entityRegis.RegistrationID, 5, AppSession.UserLogin.UserID, DateTime.Now);
                        }
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
                else
                {
                    errMessage = "You must be Registered Physician and must entry Patient Chief Complaint, Diagnosis before discharge this patient";
                    result = false;
                }
            }
            return result;
        }

        private bool IsValidToDischarge()
        {
            bool isDPJPPhysician = AppSession.RegisteredPatient.ParamedicID == AppSession.UserLogin.ParamedicID;

            bool isChiefComplaintExist = hdnIsHasChiefComplaint.Value == "1";

            PatientDiagnosis oDiagnosis = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.UserID)).FirstOrDefault();
            bool isDiagnosisExist = oDiagnosis != null;

            if (chkIsWithoutInitialPhysicianAssessment.Checked)
            {
                return true;
            }
            else
            {
                return isChiefComplaintExist && isDiagnosisExist && isDPJPPhysician;
            }
        }

        private void BridgingToMedinfrasV1(string registrationNo)
        {
            APIMessageLog apiLog = new APIMessageLog();
            apiLog.MessageDateTime = DateTime.Now;
            apiLog.Sender = Constant.BridgingVendor.HIS;
            apiLog.Recipient = Constant.BridgingVendor.MEDINFRAS_API;

            MedinfrasV1Service oService = new MedinfrasV1Service();
            string serviceResult = string.Empty;
            serviceResult = oService.OnSendPatientVisitDischarge(registrationNo);
            if (!string.IsNullOrEmpty(serviceResult))
            {
                string[] serviceResultInfo = serviceResult.Split('|');
                if (serviceResultInfo[0] == "1")
                {
                    apiLog.IsSuccess = true;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                }
                else
                {
                    apiLog.IsSuccess = false;
                    apiLog.MessageText = serviceResultInfo[1];
                    apiLog.Response = serviceResultInfo[2];
                    apiLog.ErrorMessage = serviceResultInfo[2];
                }
                BusinessLayer.InsertAPIMessageLog(apiLog);
            }
        }
    }
}