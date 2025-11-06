using System;
using System.Collections.Generic;
using DevExpress.Web.ASPxCallbackPanel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.Inpatient.Program
{
    public partial class PatientDischargeEntryCtl : BaseEntryPopupCtl
    {
        protected string RegistrationDateTime = "";

        public override void InitializeDataControl(string param)
        {
            hdnIsBPJSBridging.Value = AppSession.IsBridgingToBPJS ? "1" : "0";
            if (param != "")
            {
                IsAdd = false;
                SetControlProperties();

                hdnRegistrationID.Value = param;

                vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value)).FirstOrDefault();
                hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
                hdnClassID.Value = entity.ClassID.ToString();
                hdnIsBridgingToAplicares.Value = AppSession.IsBridgingToAPLICARES ? "1" : "0";

                RegistrationBPJS entityBPJS = BusinessLayer.GetRegistrationBPJS(entity.RegistrationID);

                List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}','{7}')",
                    AppSession.UserLogin.HealthcareID,
                    Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID,
                    Constant.SettingParameter.IP_BED_STATUS_DEFAULT_WHEN_PATIENT_DISCHARGE,
                    Constant.SettingParameter.IP_TANGGAL_PULANG_DARI_RENCANA_PULANG,
                    Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE,
                    Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV));
                hdnHealthcareServiceUnitICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_ICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitNICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_NICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnHealthcareServiceUnitPICUID.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_PICU_SERVICE_UNIT_ID).FirstOrDefault().ParameterValue;
                hdnIsParamedicInRegistrationUseScheduleCtl.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).FirstOrDefault().ParameterValue;
                hdnIsDischargeDateFromPlanning.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_TANGGAL_PULANG_DARI_RENCANA_PULANG).FirstOrDefault().ParameterValue;
                hdnIsBridgingToIPTV.Value = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.SA_IS_BRIDGING_TO_IPTV).FirstOrDefault().ParameterValue;

                string setvarBedStatus = lstSettingParameterDt.Where(t => t.ParameterCode == Constant.SettingParameter.IP_BED_STATUS_DEFAULT_WHEN_PATIENT_DISCHARGE).FirstOrDefault().ParameterValue;

                if (entityBPJS != null)
                {
                    hdnNoSEP.Value = entityBPJS.NoSEP;
                }
                EntityToControl(entity);

                Bed bed = BusinessLayer.GetBed(Convert.ToInt32(entity.BedID));
                if (bed.IsTemporary)
                {
                    cboBedStatus.Value = Constant.BedStatus.CLOSED;
                }
                else
                {
                    if (setvarBedStatus != null && setvarBedStatus != "")
                    {
                        cboBedStatus.Value = setvarBedStatus;
                    }
                    else
                    {
                        cboBedStatus.Value = Constant.BedStatus.UNOCCUPIED;
                    }
                }


                string filterExpression = string.Format("RegistrationID = {0}", param);
                vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression).FirstOrDefault();
                bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);
                if (!outstanding) divOrderStatus.Style.Add("display", "none");

                DateTime selectedDate = Helper.GetDatePickerValue(txtAppointmentDate);

                //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
                int daynumber = (int)selectedDate.DayOfWeek;
                if (daynumber == 0)
                {
                    daynumber = 7;
                }

                hdnDayNumber.Value = daynumber.ToString();
            }
        }

        protected string GetPatientOutcomeDeadBefore48()
        {
            return Constant.PatientOutcome.DEAD_BEFORE_48;
        }

        protected string GetPatientOutcomeDeadAfter48()
        {
            return Constant.PatientOutcome.DEAD_AFTER_48;
        }

        protected string GetDischargeMethodToMortuary()
        {
            return Constant.DischargeMethod.DISCHARGED_TO_MORTUARY;
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        protected override void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID IN ('{0}','{1}','{2}','{3}') AND StandardCodeID NOT IN ('{4}','{5}','{6}') AND IsActive = 1 AND IsDeleted = 0", Constant.StandardCode.PATIENT_OUTCOME, Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL, Constant.StandardCode.DISCHARGE_ROUTINE, Constant.StandardCode.BED_STATUS, Constant.BedStatus.OCCUPIED, Constant.BedStatus.BOOKED, Constant.BedStatus.WAIT_TO_BE_TRANSFERRED);
            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(filterExpression);
            List<StandardCode> lstStandardCode2 = BusinessLayer.GetStandardCodeList(String.Format("StandardCodeID IN ('{0}','{1}')", Constant.Referrer.FASKES, Constant.Referrer.RUMAH_SAKIT));
            List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareID = '{0}' AND DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", AppSession.UserLogin.HealthcareID));

            Methods.SetComboBoxField<StandardCode>(cboPatientOutcome, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && p.StandardCodeID != Constant.PatientOutcome.DEAD_AFTER_48 && p.StandardCodeID != Constant.PatientOutcome.DEAD_BEFORE_48).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboPatientOutcomeDead, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.PATIENT_OUTCOME && (p.StandardCodeID == Constant.PatientOutcome.DEAD_AFTER_48 || p.StandardCodeID == Constant.PatientOutcome.DEAD_BEFORE_48)).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeRoutine, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_ROUTINE).OrderBy(x => x.TagProperty).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboBedStatus, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.BED_STATUS).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboDischargeReason, lstStandardCode.Where(p => p.ParentID == Constant.StandardCode.DISCHARGE_REASON_TO_OTHER_HOSPITAL).ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<StandardCode>(cboReferrerGroup, lstStandardCode2.ToList(), "StandardCodeName", "StandardCodeID");
            Methods.SetComboBoxField<vHealthcareServiceUnit>(cboClinic, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtDischargeDate, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDischargeTime1, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDischargeTime2, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDischargeRoutine, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboPatientOutcome, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboBedStatus, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboClinic, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboReferrerGroup, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtReferrerCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDischargeReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtParamedicCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtTimeOfDeath, new ControlEntrySetting(true, true, true, Constant.DefaultValueEntry.TIME_NOW));
        }

        protected void cbpViewPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string[] param = e.Parameter.Split('|');
            result = param[0] + "|";
            if (param[0] == "getdaynumber")
            {
                DateTime selectedDate = Helper.GetDatePickerValue(txtAppointmentDate);

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

        private void EntityToControl(vConsultVisit entity)
        {
            hdnRegistrationDate.Value = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            hdnRegistrationTime.Value = Convert.ToDateTime(entity.ActualVisitTime).ToString(Constant.FormatString.TIME_FORMAT_FULL);
            txtRegistrationNo.Text = entity.RegistrationNo;
            txtRegistrationDateTime.Text = entity.ActualVisitDate.ToString(Constant.FormatString.DATE_FORMAT) + " " + entity.ActualVisitTime;
            txtPatientInfo.Text = "(" + entity.MedicalNo + ") " + entity.PatientName;
            txtServiceUnitName.Text = entity.ServiceUnitName;
            txtRoomName.Text = entity.RoomName;
            txtBedCode.Text = entity.BedCode;

            RegistrationDateTime = entity.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            RegistrationDateTime += entity.VisitTime.Replace(":", "");

            hdnGCRegistrationStatus.Value = entity.GCVisitStatus;
            hdnBedID.Value = entity.BedID.ToString();
            hdnRoomID.Value = entity.RoomID.ToString();
            if (entity.GCVisitStatus == Constant.VisitStatus.DISCHARGED || entity.GCVisitStatus == Constant.VisitStatus.PHYSICIAN_DISCHARGE)
            {
                cboPatientOutcome.Value = entity.GCDischargeCondition;
                cboDischargeRoutine.Value = entity.GCDischargeMethod;
                txtDischargeDate.Text = entity.PhysicianDischargeOrderDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtDischargeTime1.Text = entity.PhysicianDischargeOrderTime.Substring(0, 2);
                txtDischargeTime2.Text = entity.PhysicianDischargeOrderTime.Substring(3, 2);
                hdnLOSInDay.Value = entity.LOSInDay.ToString();
                hdnLOSInHour.Value = entity.LOSInHour.ToString();
                hdnLOSInMinute.Value = entity.LOSInMinute.ToString();
            }
            else
            {
                if (entity.PlanDischargeDateInString != "" && entity.PlanDischargeDateInString != "01-Jan-1900")
                {
                    txtPlanningDate.Text = entity.PlanDischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtPlanningTime.Text = entity.PlanDischargeTime;

                    if (hdnIsDischargeDateFromPlanning.Value == "1")
                    {
                        txtDischargeDate.Text = entity.PlanDischargeDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtDischargeTime1.Text = entity.PlanDischargeTime.Substring(0, 2);
                        txtDischargeTime2.Text = entity.PlanDischargeTime.Substring(3, 2);
                    }
                    else
                    {
                        txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                        txtDischargeTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                        txtDischargeTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
                    }
                }
                else
                {
                    txtDischargeDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtDischargeTime1.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(0, 2);
                    txtDischargeTime2.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT).Substring(3, 2);
                }
            }
            txtDateOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtTimeOfDeath.Text = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDischargeRemarks.Text = entity.DischargeRemarks;
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
            RegistrationDao registrationDao = new RegistrationDao(ctx);
            BedDao entityBedDao = new BedDao(ctx);
            PatientDao patientDao = new PatientDao(ctx);
            PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
            AppointmentRequestDao entityApmRequestDao = new AppointmentRequestDao(ctx);
            SettingParameterDtDao setvarDtDao = new SettingParameterDtDao(ctx);

            try
            {
                DateTime registrationDate = Helper.YYYYMMDDHourToDate(hdnRegistrationDate.Value + " " + hdnRegistrationTime.Value);
                DateTime dischargeDateSelected = Helper.YYYYMMDDHourToDate(Helper.GetDatePickerValue(txtDischargeDate).ToString(Constant.FormatString.DATE_FORMAT_112) + " " + Convert.ToDateTime(string.Format("{0}:{1}", txtDischargeTime1.Text, txtDischargeTime2.Text)).ToString(Constant.FormatString.TIME_FORMAT_FULL));

                if (chkIsDead.Checked && (txtDateOfDeath.Text == "" || Helper.GetDatePickerValue(txtDateOfDeath).ToString(Constant.FormatString.DATE_FORMAT) == Constant.ConstantDate.DEFAULT_NULL_DATE_FORMAT || txtTimeOfDeath.Text == ""))
                {
                    result = false;
                    errMessage = "Maaf, jika pasien statusnya sudah meninggal, tolong lengkapi tanggal dan jam meninggalnya terlebih dahulu.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }

                if (dischargeDateSelected < registrationDate)
                {
                    result = false;
                    errMessage = "Maaf, Tanggal pulang tidak dapat lebih kecil dari tanggal masuk.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                }
                
                if (result)
                {
                    int count = BusinessLayer.GetPatientTransferRowCount(string.Format("RegistrationID = {0} AND GCPatientTransferStatus = '{1}'", hdnRegistrationID.Value, Constant.PatientTransferStatus.OPEN), ctx);
                    if (count > 0)
                    {
                        errMessage = "Please Transfer / Cancel All Opened Patient Transfer For This Registration First.";
                        result = false;
                    }
                    else
                    {
                        bool isCheckOutstanding = true;
                        SettingParameterDt oParam = setvarDtDao.Get(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.FN_VALIDASI_TAGIHAN_KETIKA_PULANG);
                        if (oParam != null)
                        {
                            isCheckOutstanding = oParam.ParameterValue == "1" ? true : false;
                        }

                        if (isCheckOutstanding)
                        {
                            List<vRegistrationOutstanding> lstOutstanding = BusinessLayer.GetvRegistrationOutstandingList(string.Format("RegistrationID = {0}", hdnRegistrationID.Value), ctx);
                            if (lstOutstanding.Count > 0)
                            {
                                errMessage = "Pasien tidak dapat dipulangkan karena masih memiliki sisa tagihan";
                                result = false;
                            }
                        }

                        if (result)
                        {
                            string filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                            vRegistrationOutstandingInfo lstInfo = BusinessLayer.GetvRegistrationOutstandingInfoList(filterExpression, ctx).FirstOrDefault();
                            bool outstanding = (lstInfo.ServiceOrder + lstInfo.PrescriptionOrder + lstInfo.PrescriptionReturnOrder + lstInfo.LaboratoriumOrder + lstInfo.RadiologiOrder + lstInfo.OtherOrder > 0);

                            if (!outstanding)
                            {
                                List<ConsultVisit> lstConsultVisit = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value), ctx);

                                ConsultVisit entityConsultVisit = lstConsultVisit.Where(t => t.RegistrationID == Convert.ToInt32(hdnRegistrationID.Value)).FirstOrDefault();
                                Registration entityRegis = registrationDao.Get(entityConsultVisit.RegistrationID);

                                entityConsultVisit.GCVisitStatus = Constant.VisitStatus.DISCHARGED;

                                if (chkIsDead.Checked)
                                {
                                    entityConsultVisit.GCDischargeCondition = cboPatientOutcomeDead.Value.ToString();
                                }
                                else
                                {
                                    entityConsultVisit.GCDischargeCondition = cboPatientOutcome.Value.ToString();
                                }

                                entityConsultVisit.GCDischargeMethod = cboDischargeRoutine.Value.ToString();
                                entityConsultVisit.DischargeDate = Helper.GetDatePickerValue(txtDischargeDate);
                                entityConsultVisit.DischargeTime = string.Format("{0}:{1}", txtDischargeTime1.Text, txtDischargeTime2.Text);
                                entityConsultVisit.RoomDischargedBy = AppSession.UserLogin.UserID;
                                entityConsultVisit.RoomDischargeDateTime = DateTime.Now;
                                entityConsultVisit.ActualDischargeDateTime = DateTime.Now;
                                entityConsultVisit.IsDischargeNeedFollowUp = chkIsAllowFollowUp.Checked;
                                entityConsultVisit.LOSInDay = Convert.ToDecimal(hdnLOSInDay.Value);
                                entityConsultVisit.LOSInHour = Convert.ToDecimal(hdnLOSInHour.Value);
                                entityConsultVisit.LOSInMinute = Convert.ToDecimal(hdnLOSInMinute.Value);
                                entityConsultVisit.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityConsultVisit.DischargeRemarks = txtDischargeRemarks.Text;

                                if (entityConsultVisit.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_EXTERNAL_PROVIDER)
                                {
                                    entityConsultVisit.ReferralTo = Convert.ToInt32(hdnReferrerID.Value);
                                }
                                else if (entityConsultVisit.GCDischargeMethod == Constant.DischargeMethod.DISCHARGED_TO_WARD)
                                {
                                    entityConsultVisit.ReferralPhysicianID = Convert.ToInt32(hdnParamedicID2.Value);
                                    entityConsultVisit.IsRefferralProcessed = false;
                                }
                                else if (entityConsultVisit.GCDischargeMethod == Constant.DischargeMethod.REFFERRED_TO_OUTPATIENT)
                                {
                                    entityConsultVisit.ReferralUnitID = Convert.ToInt32(cboClinic.Value.ToString());
                                    entityConsultVisit.ReferralPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                                    entityConsultVisit.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                                    entityConsultVisit.IsRefferralProcessed = false;

                                    string filterApptReq = string.Format("RegistrationID = {0} AND IsDeleted = 0", entityConsultVisit.RegistrationID);
                                    List<vAppointmentRequest> apptReqList = BusinessLayer.GetvAppointmentRequestList(filterApptReq, ctx);
                                    int appReqID = 0;
                                    if (apptReqList.Count() == 0)
                                    {
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

                                            Registration oReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID = '{0}'", hdnRegistrationID.Value), ctx).FirstOrDefault();
                                            AppointmentRequest entityApmRequest = new AppointmentRequest();
                                            entityApmRequest.RegistrationID = entityConsultVisit.RegistrationID;
                                            entityApmRequest.VisitID = entityConsultVisit.VisitID;
                                            entityApmRequest.MRN = oReg.MRN;
                                            entityApmRequest.HealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
                                            entityApmRequest.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                            entityApmRequest.VisitTypeID = visitTypeID;
                                            entityApmRequest.IsHasNoDate = false;
                                            entityApmRequest.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate.Text);
                                            entityApmRequest.IsDeleted = false;
                                            entityApmRequest.CreatedBy = AppSession.UserLogin.UserID;
                                            entityApmRequest.CreatedDate = DateTime.Now;
                                            entityApmRequest.GCCustomerType = oReg.GCCustomerType;

                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            entityApmRequest.AppointmentRequestNo = BusinessLayer.GenerateTransactionNo(Constant.TransactionCode.OP_APPOINTMENT_REQUEST, DateTime.Now);

                                            ctx.CommandType = CommandType.Text;
                                            ctx.Command.Parameters.Clear();

                                            if (oReg.GCCustomerType == Constant.CustomerType.PERSONAL)
                                            {
                                                entityApmRequest.BusinessPartnerID = 1;
                                                entityApmRequest.ContractID = null;
                                                entityApmRequest.CoverageTypeID = null;
                                                entityApmRequest.CoverageLimitAmount = 0;
                                                entityApmRequest.IsCoverageLimitPerDay = false;
                                                entityApmRequest.GCTariffScheme = null;
                                                entityApmRequest.IsControlClassCare = false;
                                                entityApmRequest.EmployeeID = null;

                                            }
                                            else
                                            {
                                                entityApmRequest.BusinessPartnerID = oReg.BusinessPartnerID;
                                                string filterexpressionContract = string.Format("BusinessPartnerID = {0} AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0", entityApmRequest.BusinessPartnerID);
                                                CustomerContract oCsContract = BusinessLayer.GetCustomerContractList(filterexpressionContract, ctx).FirstOrDefault();
                                                if (oCsContract != null)
                                                {

                                                    string filtexexpres2 = string.Format(" {0} AND ContractNo = '{1}' ", filterexpressionContract, oCsContract.ContractNo);
                                                    vCustomerContract oVcsContract = BusinessLayer.GetvCustomerContractList(filtexexpres2).FirstOrDefault();
                                                    if (oVcsContract != null)
                                                    {
                                                        entityApmRequest.ContractID = oVcsContract.ContractID;
                                                    }
                                                }
                                                else
                                                {
                                                    entityApmRequest.ContractID = null;
                                                }

                                                entityApmRequest.IsControlClassCare = oReg.IsControlClassCare;
                                                entityApmRequest.IsCoverageLimitPerDay = oReg.IsCoverageLimitPerDay;
                                                entityApmRequest.GCTariffScheme = oReg.GCTariffScheme;
                                                entityApmRequest.CoverageLimitAmount = oReg.CoverageLimitAmount;
                                                entityApmRequest.CoverageTypeID = oReg.CoverageTypeID;
                                                entityApmRequest.EmployeeID = oReg.EmployeeID;
                                            }

                                            appReqID = entityApmRequestDao.InsertReturnPrimaryKeyID(entityApmRequest);
                                        }
                                    }

                                    string filterPatientReferral = string.Format("VisitID = {0} AND IsDeleted = 0", entityConsultVisit.VisitID);
                                    List<PatientReferral> pReferralList = BusinessLayer.GetPatientReferralList(filterPatientReferral, ctx);
                                    if (pReferralList.Count() == 0)
                                    {
                                        PatientReferral entityReff = new PatientReferral();
                                        entityReff.VisitID = entityConsultVisit.VisitID;
                                        entityReff.ReferralDate = Helper.GetDatePickerValue(txtAppointmentDate);
                                        entityReff.FromPhysicianID = Convert.ToInt32(entityConsultVisit.ParamedicID);
                                        entityReff.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                                        if (rblReferralType.SelectedValue == "1")
                                            entityReff.GCRefferalType = "X075^04";
                                        else
                                            entityReff.GCRefferalType = "X075^05";
                                        entityReff.ToHealthcareServiceUnitID = Convert.ToInt32(cboClinic.Value);
                                        if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
                                        {
                                            entityReff.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                                        }
                                        entityReff.DiagnosisText = txtDiagnosisText.Text;
                                        entityReff.MedicalResumeText = txtMedicalResumeText.Text;
                                        entityReff.PlanningResumeText = txtPlanningResumeText.Text;
                                        entityReff.CreatedBy = AppSession.UserLogin.UserID;

                                        if (appReqID > 0)
                                        {
                                            entityReff.AppointmentRequestID = appReqID;
                                        }

                                        entityReferralDao.Insert(entityReff);
                                    }
                                }

                                if (cboDischargeReason.Value != null)
                                {
                                    entityConsultVisit.GCReferralDischargeReason = cboDischargeReason.Value.ToString();
                                    if (cboDischargeReason.Value.ToString() == Constant.ReferralDischargeReason.LAINNYA)
                                    {
                                        entityConsultVisit.ReferralDischargeReasonOther = txtDischargeOtherReason.Text;
                                    }
                                }


                                if (lstConsultVisit.Where(t => t.GCVisitStatus != Constant.VisitStatus.PHYSICIAN_DISCHARGE && t.GCVisitStatus != Constant.VisitStatus.DISCHARGED && t.GCVisitStatus != Constant.VisitStatus.CLOSED && t.GCVisitStatus != Constant.VisitStatus.CANCELLED).Count() == 0)
                                {
                                    entityRegis.GCRegistrationStatus = Constant.VisitStatus.DISCHARGED;
                                    entityRegis.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    registrationDao.Update(entityRegis);
                                }

                                if (entityConsultVisit.GCDischargeCondition.Equals(Constant.PatientOutcome.DEAD_AFTER_48) || entityConsultVisit.GCDischargeCondition.Equals(Constant.PatientOutcome.DEAD_BEFORE_48))
                                {
                                    Patient p = patientDao.Get(Convert.ToInt32(entityRegis.MRN));
                                    p.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                                    p.IsAlive = false;
                                    patientDao.Update(p);

                                    entityConsultVisit.DateOfDeath = Helper.GetDatePickerValue(txtDateOfDeath);
                                    entityConsultVisit.TimeOfDeath = txtTimeOfDeath.Text;
                                }
                                entityConsultVisitDao.Update(entityConsultVisit);

                                Bed bed = entityBedDao.Get(Convert.ToInt32(hdnBedID.Value));
                                bed.GCBedStatus = cboBedStatus.Value.ToString();
                                bed.RegistrationID = null;
                                bed.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityBedDao.Update(bed);

                                List<PatientAccompany> lstPatientAccompany = BusinessLayer.GetPatientAccompanyList(String.Format("RegistrationID = {0} AND IsDeleted = 0", entityRegis.RegistrationID), ctx);
                                if (lstPatientAccompany.Count > 0)
                                {
                                    foreach (PatientAccompany pa in lstPatientAccompany)
                                    {
                                        Bed bedPA = entityBedDao.Get(Convert.ToInt32(pa.BedID));
                                        if (pa.RegistrationID == bedPA.RegistrationID && bedPA.IsPatientAccompany == true)
                                        {
                                            bedPA.RegistrationID = null;
                                            bedPA.GCBedStatus = cboBedStatus.Value.ToString();
                                            bedPA.IsPatientAccompany = false;
                                            bedPA.LastUpdatedBy = AppSession.UserLogin.UserID;
                                            entityBedDao.Update(bedPA);
                                        }
                                    }
                                }

                                #region Bridging to Queue

                                if (AppSession.IsBridgingToQueue)
                                {
                                    try
                                    {
                                        VisitInfo visitInfo = new VisitInfo();
                                        visitInfo = ConvertVisitToDTO(entityConsultVisit.VisitID, entityConsultVisit.DischargeDate, entityConsultVisit.DischargeTime, ctx);

                                        APIMessageLog entityAPILog = new APIMessageLog()
                                        {
                                            MessageDateTime = DateTime.Now,
                                            Recipient = Constant.BridgingVendor.QUEUE,
                                            Sender = Constant.BridgingVendor.HIS,
                                            IsSuccess = true
                                        };

                                        QueueService oService = new QueueService();
                                        string apiResult = oService.SendDischargeInformation(entityConsultVisit.RegistrationID, entityRegis.RegistrationNo, (int)entityRegis.MRN, visitInfo);
                                        string[] apiResultInfo = apiResult.Split('|');
                                        if (apiResultInfo[0] == "0")
                                        {
                                            entityAPILog.IsSuccess = false;
                                            entityAPILog.MessageText = apiResultInfo[2];
                                            entityAPILog.Response = apiResultInfo[1];
                                            Exception ex = new Exception(apiResultInfo[1]);
                                            Helper.InsertErrorLog(ex);
                                        }
                                        else
                                            entityAPILog.MessageText = apiResultInfo[2];

                                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                    }
                                    catch (Exception ex)
                                    {
                                        Helper.InsertErrorLog(ex);
                                    }
                                }

                                #endregion
                            }
                            else
                            {
                                errMessage = "Pasien tidak dapat dipulangkan karena masih memiliki outstanding / pending order";
                                result = false;
                            }
                        }
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();

                    if (hdnIsBridgingToIPTV.Value == "1")
                    {
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                            Sender = Constant.BridgingVendor.HIS,
                            IsSuccess = true
                        };

                        List<CenterbackBedTransferDTO> lstCenterbackDTO = new List<CenterbackBedTransferDTO>();
                        CenterbackBedTransferDTO cbObj = new CenterbackBedTransferDTO()
                        {
                            HealthcareID = AppSession.UserLogin.HealthcareID,
                            ProcessType = "checkout",
                            RegistrationID = Convert.ToInt32(hdnRegistrationID.Value)
                        };
                        lstCenterbackDTO.Add(cbObj);

                        string apiResult = oService.IPTV_BedTransfer(lstCenterbackDTO);
                        string[] apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);
                        }
                        else
                            entityAPILog.MessageText = apiResultInfo[1];

                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
                else
                {
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
            return result;
        }

        #region Bridging to Queue - Methods
        private VisitInfo ConvertVisitToDTO(int visitID, DateTime dischargeDate, string dischargeTime, IDbContext ctx)
        {
            vConsultVisit oConsultVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", visitID), ctx).FirstOrDefault();
            VisitInfo visitInfo = new VisitInfo();
            visitInfo.VisitID = oConsultVisit.VisitID;
            visitInfo.VisitDate = oConsultVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.VisitTime = oConsultVisit.VisitTime;
            visitInfo.DepartmentCode = Constant.Facility.INPATIENT;
            visitInfo.ServiceUnitCode = oConsultVisit.ServiceUnitCode;
            visitInfo.ServiceUnitName = oConsultVisit.ServiceUnitName;
            visitInfo.RoomID = oConsultVisit.RoomID;
            visitInfo.RoomCode = oConsultVisit.RoomCode;
            visitInfo.RoomName = oConsultVisit.RoomName;
            visitInfo.PhysicianID = oConsultVisit.ParamedicID;
            visitInfo.PhysicianCode = oConsultVisit.ParamedicCode;
            visitInfo.PhysicianName = oConsultVisit.ParamedicName;
            visitInfo.SpecialtyName = oConsultVisit.SpecialtyName;
            visitInfo.BedCode = oConsultVisit.BedCode;
            visitInfo.ExtensionNo = oConsultVisit.ExtensionNo;
            visitInfo.DischargeDate = dischargeDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            visitInfo.DischargeTime = dischargeTime;
            return visitInfo;
        }
        #endregion
    }
}