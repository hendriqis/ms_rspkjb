using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;

namespace QIS.Medinfras.Web.MedicalRecord.Program
{
    public partial class MRPatientDischarge : BasePagePatientPageList
    {
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalRecord.PATIENT_SOAP;
        }

        public override void SetCRUDMode(ref bool IsAllowAdd, ref bool IsAllowEdit, ref bool IsAllowDelete)
        {
            IsAllowAdd = IsAllowDelete = IsAllowEdit = false;
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

        protected string RegistrationDateTime = "";
        protected override void InitializeDataControl()
        {
            ctlToolbar.SetSelectedMenu(5);

            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA0116, Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE));
            hdnIsValidateParamedicSchedule.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0116).FirstOrDefault().ParameterValue;
            hdnIsParamedicInRegistrationUseSchedule.Value = lstSetvarDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).ParameterValue;
            hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            if (hdnIsParamedicInRegistrationUseSchedule.Value == "1")
            {
                trParamedicHasSchedule.Style.Remove("display");
            }
            else
            {
                trParamedicHasSchedule.Style.Add("display", "none");
            }

            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstStandardCode2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.StandardCodeID != Constant.PatientOutcome.DEAD_AFTER_48 && p.StandardCodeID != Constant.PatientOutcome.DEAD_BEFORE_48).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcomeDead, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && (p.StandardCodeID == Constant.PatientOutcome.DEAD_AFTER_48 || p.StandardCodeID == Constant.PatientOutcome.DEAD_BEFORE_48)).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstStandardCode2.ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");

            RegistrationDateTime = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += AppSession.RegisteredPatient.VisitTime.Replace(":", "");

            Helper.SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(txtDischargeTime, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true), "mpPatientDischarge");
            Helper.SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true), "mpPatientDischarge");

            ConsultVisit entity = BusinessLayer.GetConsultVisit(AppSession.RegisteredPatient.VisitID);
            EntityToControl(entity);

            if (entity.IsNeedCodification)
            {
                chkIsNeedCodification.Checked = true;
            }
            else
            {
                chkIsNeedCodification.Checked = false;
            }

            DateTime selectedDate = DateTime.Now;

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            hdnDayNumber.Value = daynumber.ToString();
        }

        private void EntityToControl(ConsultVisit entity)
        {
            hdnRegistrationID.Value = entity.RegistrationID.ToString();
            hdnRegistrationDate.Value = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnRegistrationTime.Value = Convert.ToDateTime(entity.ActualVisitTime).ToString(Constant.FormatString.TIME_FORMAT_FULL);
            if (entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_AFTER_48 || entity.GCDischargeCondition == Constant.PatientOutcome.DEAD_BEFORE_48)
            {
                chkIsDead.Checked = true;
                cboPatientOutcomeDead.Value = entity.GCDischargeCondition;

                trDeathInfo.Attributes.Remove("style");
            }
            else
            {
                cboPatientOutcome.Value = entity.GCDischargeCondition;

                trDeathInfo.Attributes.Add("style", "display:none");
            }
            cboDischargeRoutine.Value = entity.GCDischargeMethod;

            chkIsNeedCodification.Checked = entity.IsNeedCodification;

            DateTime dischargeDate = DateTime.Now;
            string dischargeTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);

            if (!entity.DischargeDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtDischargeDate.Text = entity.DischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entity.DischargeTime;
            }
            else if (!entity.PhysicianDischargeOrderDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtDischargeDate.Text = entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime.Text = entity.PhysicianDischargeOrderTime;
            }
            else
            {
                if (AppSession.RegisteredPatient.DepartmentID != Constant.Facility.INPATIENT)
                {
                    List<ChiefComplaint> oChiefComplaintList = BusinessLayer.GetChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                    if (oChiefComplaintList.Count() > 0)
                    {
                        ChiefComplaint oChiefComplaint = oChiefComplaintList.FirstOrDefault();
                        dischargeDate = oChiefComplaint.ObservationDate;
                        dischargeTime = oChiefComplaint.ObservationTime;
                    }
                    else
                    {
                        List<NurseChiefComplaint> oNurseChiefComplaintList = BusinessLayer.GetNurseChiefComplaintList(string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID));
                        if (oNurseChiefComplaintList.Count() > 0)
                        {
                            NurseChiefComplaint oNurseChiefComplaint = oNurseChiefComplaintList.FirstOrDefault();
                            dischargeDate = oNurseChiefComplaint.ChiefComplaintDate;
                            dischargeTime = oNurseChiefComplaint.ChiefComplaintTime;
                        }
                        else
                        {
                            dischargeDate = DateTime.Now;
                            dischargeTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        }
                    }
                    txtDischargeDate.Text = dischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDischargeTime.Text = dischargeTime;
                }
                else
                {
                    txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDischargeTime.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                }
            }

            if (entity.DateOfDeath != null)
            {
                txtDateOfDeath.Text = entity.DateOfDeath.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = entity.TimeOfDeath;
            }
            else
            {
                txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            }

            if (!entity.ReferralDate.Equals(new DateTime(1900, 1, 1)))
            {
                txtAppointmentDate.Text = entity.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }
            else
            {
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            }

            if (entity.ReferralPhysicianID != null)
            {
                ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster((Int32)entity.ReferralPhysicianID);
                if (oParamedic != null)
                {
                    hdnParamedicID2.Value = entity.ReferralPhysicianID.ToString();
                    txtParamedicCode.Text = oParamedic.ParamedicCode;
                    txtParamedicName.Text = oParamedic.FullName;
                }
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

            vConsultVisit13 oConsultVisit13 = BusinessLayer.GetvConsultVisit13List(string.Format("VisitID = {0}", entity.VisitID)).FirstOrDefault();
            divRegistrationStatus.InnerHtml = oConsultVisit13.RegistrationStatus;
            divPhysicianDischargedBy.InnerHtml = oConsultVisit13.PhysicianDischargedByUserName;
            divPhysicianDischargedDate.InnerHtml = oConsultVisit13.PhysicianDischargedDateInString;
            divRoomDischargedBy.InnerHtml = oConsultVisit13.RoomDischargedByUserName;
            divRoomDischargedDate.InnerHtml = oConsultVisit13.RoomDischargedDateInString;
            divAdminDischargedBy.InnerHtml = oConsultVisit13.AdminDischargedByUserName;
            divAdminDischargedDate.InnerHtml = oConsultVisit13.AdminDischargedDateInString;

            hdnLOSInDay.Value = entity.LOSInDay.ToString();
            hdnLOSInHour.Value = entity.LOSInHour.ToString();
            hdnLOSInMinute.Value = entity.LOSInMinute.ToString();
            cboDischargeReason.Value = entity.GCReferralDischargeReason;
            txtDischargeOtherReason.Text = entity.ReferralDischargeReasonOther;
            txtDischargeRemarks.Text = entity.DischargeRemarks;

            string filterPatientReferral = string.Format("VisitID = {0} AND IsDeleted = 0", entity.VisitID);
            List<PatientReferral> pReferralList = BusinessLayer.GetPatientReferralList(filterPatientReferral);
            if (pReferralList.Count() > 0)
            {
                hdnIsHasPatientReferral.Value = "1";
            }

            //string filterApptReq = string.Format("RegistrationID = {0} AND IsDeleted = 0", entity.RegistrationID);
            //List<vAppointmentRequest> apptReqList = BusinessLayer.GetvAppointmentRequestList(filterApptReq);
            //if (apptReqList.Count() > 0)
            //{
            //    vAppointmentRequest apptReq = apptReqList.FirstOrDefault();
            //    cboClinic.Value = apptReq.HealthcareServiceUnitID.ToString();
            //    hdnPhysicianID.Value = apptReq.ParamedicID.ToString();
            //    txtPhysicianCode.Text = apptReq.ParamedicCode;
            //    txtPhysicianName.Text = apptReq.ParamedicName;
            //    txtAppointmentDate.Text = apptReq.AppointmentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            //    rblReferralType.SelectedValue = "2";

            //    hdnIsHasAppointmentRequest.Value = "1";
            //}
            //else
            //{
            //    if (entity.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
            //    {
            //        rblReferralType.SelectedValue = "1";
            //    }
            //}
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            SettingParameterDtDao setvarDtDao = new SettingParameterDtDao(ctx);
            PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
            PatientReferralExternalDao entityReferralExternalDao = new PatientReferralExternalDao(ctx);
            AppointmentRequestDao entityApmRequestDao = new AppointmentRequestDao(ctx);

            if (type == "process")
            {
                try
                {
                    DateTime registrationDate = Helper.YYYYMMDDHourToDate(hdnRegistrationDate.Value + " " + hdnRegistrationTime.Value);
                    DateTime dischargeDateSelected = Helper.YYYYMMDDHourToDate(Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112) + " " + Convert.ToDateTime(string.Format("{0}", txtDischargeTime.Text)).ToString(Constant.FormatString.TIME_FORMAT_FULL));

                    string errMessageFrom = "";
                    //TODO : Validasi Tagihan Pasien
                    if (cboDischargeRoutine.Value.ToString() != Constant.DischargeMethod.DISCHARGED_TO_WARD)
                    {
                        bool isCheckOutstanding = true;
                        SettingParameterDt oParam = setvarDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_VALIDASI_TAGIHAN_KETIKA_PULANG);
                        if (oParam != null)
                        {
                            isCheckOutstanding = oParam.ParameterValue == "1" ? true : false;
                        }

                        if (isCheckOutstanding)
                        {
                            List<vRegistrationOutstanding> lstOutstanding = BusinessLayer.GetvRegistrationOutstandingList(string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID), ctx);
                            if (lstOutstanding.Count > 0)
                            {
                                result = false;
                                errMessageFrom = " masih memiliki sisa tagihan";
                            }
                        }
                    }

                    string filterExpression = string.Format("RegistrationID = {0}", AppSession.RegisteredPatient.RegistrationID);
                    vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression, ctx).FirstOrDefault();
                    bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);

                    if (outstanding)
                    {
                        result = false;
                        errMessageFrom = " masih memiliki outstanding / pending order";
                    }

                    int lstPatientTrasfer = BusinessLayer.GetPatientTransferRowCount(string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", AppSession.RegisteredPatient.RegistrationID, Constant.PatientTransferStatus.OPEN), ctx);
                    if (lstPatientTrasfer > 0)
                    {
                        result = false;
                        errMessageFrom = " masih memiliki proses pasien pindah yang masih outstanding konfirmasi";
                    }

                    List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", AppSession.RegisteredPatient.RegistrationID), ctx);
                    ConsultVisit entity = lstConsultVisit.Where(t => t.VisitID == AppSession.RegisteredPatient.VisitID).FirstOrDefault();
                    Registration entityRegis = registrationDao.Get(entity.RegistrationID);

                    if (chkIsDead.Checked && (txtDateOfDeath.Text == "" || Helper.GetDatePickerValue(txtDateOfDeath).ToString(Constant.FormatString.DATE_FORMAT) == Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT || txtTimeOfDeath.Text == ""))
                    {
                        result = false;
                        errMessageFrom = " jika pasien statusnya sudah meninggal, tolong lengkapi tanggal dan jam meninggalnya terlebih dahulu";
                    }

                    if (dischargeDateSelected < registrationDate)
                    {
                        result = false;
                        errMessageFrom = " tanggal pulang tidak dapat lebih kecil dari tanggal masuk.";
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                    }

                    if (result)
                    {
                        if (entity.GCVisitStatus == Constant.VisitStatus.CHECKED_IN || entity.GCVisitStatus == Constant.VisitStatus.RECEIVING_TREATMENT || entity.GCVisitStatus == Constant.VisitStatus.PHYSICIAN_DISCHARGE)
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
                        entity.IsNeedCodification = chkIsNeedCodification.Checked;

                        entity.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                        entity.DischargeTime = txtDischargeTime.Text;
                        entity.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                        entity.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                        entity.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entity.AdminDischargedBy = AppSession.UserLogin.UserID;
                        entity.AdminDischargedDate = DateTime.Now;

                        if (entity.ActualDischargeDateTime == null || entity.ActualDischargeDateTime.ToString(Constant.FormatString.DATE_FORMAT) == Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT)
                        {
                            entity.ActualDischargeDateTime = DateTime.Now;
                        }

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
                            entity.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                            entity.IsRefferralProcessed = false;
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

                                if (hdnIsHasPatientReferral.Value == "0")
                                {
                                    PatientReferral entityReff = new PatientReferral();
                                    entityReff.VisitID = entity.VisitID;
                                    entityReff.ReferralDate = entity.VisitDate;
                                    entityReff.FromPhysicianID = Convert.ToInt32(entity.ParamedicID);
                                    entityReff.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                    entityReff.ToHealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);

                                    if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
                                    {
                                        entityReff.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                                    }

                                    if (rblReferralType.SelectedValue == "1")
                                    {
                                        entityReff.GCRefferalType = "X075^04";
                                    }
                                    else
                                    {
                                        entityReff.GCRefferalType = "X075^05";
                                    }
                                    entityReff.CreatedBy = AppSession.UserLogin.UserID;
                                    entityReferralDao.Insert(entityReff);
                                }

                                if (rblReferralType.SelectedValue == "2")
                                {
                                    if (hdnIsHasAppointmentRequest.Value == "0")
                                    {
                                        AppointmentRequest entityApmRequest = new AppointmentRequest();
                                        entityApmRequest.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                                        entityApmRequest.VisitID = entity.VisitID;
                                        entityApmRequest.MRN = AppSession.RegisteredPatient.MRN;
                                        entityApmRequest.HealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
                                        entityApmRequest.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);

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

                                        entityApmRequestDao.Insert(entityApmRequest);
                                    }
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

                        if (entity.GCDischargeCondition.Equals(Constant.PatientOutcome.DEAD_AFTER_48) || entity.GCDischargeCondition.Equals(Constant.PatientOutcome.DEAD_BEFORE_48))
                        {
                            Patient p = patientDao.Get(Convert.ToInt32(entityRegis.MRN));
                            p.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            p.IsAlive = false;
                            patientDao.Update(p);

                            entity.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                            entity.TimeOfDeath = txtTimeOfDeath.Text;
                        }

                        if (entity.GCVisitStatus == Constant.VisitStatus.DISCHARGED)
                        {
                            entityRegis.GCRegistrationStatus = Constant.VisitStatus.DISCHARGED;
                            entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                            registrationDao.Update(entityRegis);
                        }

                        entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entity);


                        SettingParameterDt oParamBedStatus = setvarDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IP_BED_STATUS_DEFAULT_WHEN_PATIENT_DISCHARGE);

                        if (entity.BedID != null && entity.BedID != 0)
                        {
                            Bed bed = entityBedDao.Get(Convert.ToInt32(entity.BedID));
                            if (bed.RegistrationID != null)
                            {
                                if (bed.GCBedStatus == Constant.BedStatus.OCCUPIED && bed.RegistrationID == entity.RegistrationID)
                                {
                                    bed.GCBedStatus = oParamBedStatus.ParameterValue;
                                    bed.RegistrationID = null;
                                    bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityBedDao.Update(bed);
                                }
                            }
                        }

                        List<PatientAccompany> lstPatientAccompany = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0} AND IsDeleted = 0", entityRegis.RegistrationID), ctx);
                        if (lstPatientAccompany.Count > 0)
                        {
                            foreach (PatientAccompany pa in lstPatientAccompany)
                            {
                                Bed bedPA = entityBedDao.Get(Convert.ToInt32(pa.BedID));
                                if (pa.RegistrationID == bedPA.RegistrationID && bedPA.IsPatientAccompany == true && bedPA.GCBedStatus == Constant.BedStatus.OCCUPIED)
                                {
                                    bedPA.RegistrationID = null;
                                    bedPA.GCBedStatus = oParamBedStatus.ParameterValue;
                                    bedPA.IsPatientAccompany = false;
                                    bedPA.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityBedDao.Update(bedPA);
                                }
                            }
                        }
                        ctx.CommitTransaction();
                    }
                    else
                    {
                        result = false;
                        errMessage = "Registrasi " + entityRegis.RegistrationNo + " tidak dapat dipulangkan karena" + errMessageFrom;
                        Exception ex = new Exception(errMessage);
                        Helper.InsertErrorLog(ex);
                        ctx.RollBackTransaction();
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
            return result;
        }
    }
}