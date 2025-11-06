using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class RegistrationEditCtl : BaseEntryPopupCtl
    {
        protected string OnGetMotherRegistrationNoFilterExpression()
        {
            return string.Format("GCVisitStatus NOT IN ('{0}','{1}','{2}') AND DepartmentID = '{3}' AND IsParturition = 1", Constant.VisitStatus.OPEN, Constant.VisitStatus.CANCELLED, Constant.VisitStatus.CLOSED, hdnDepartmentID.Value);
        }

        public override void InitializeDataControl(string param)
        {
            string[] paramSplit = param.Split('|');
            hdnRegistrationDate.Value = paramSplit[1];
            hdnRegistrationHour.Value = paramSplit[2];
            vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", paramSplit[0])).FirstOrDefault();
            txtRegistrationID.Text = entity.RegistrationNo;
            hdnMRNCtl.Value = entity.MRN.ToString();
            txtMRN.Text = string.Format("{0} - {1}", entity.MedicalNo, entity.PatientName);
            txtServiceUnit.Text = entity.ServiceUnitName;
            hdnRoomIDCtl.Value = entity.RoomID.ToString();
            txtRoomCode.Text = entity.RoomCode;
            txtRoomName.Text = entity.RoomName;
            hdnPhysicianID.Value = hdnPhysicianIDORI.Value = entity.ParamedicID.ToString();
            txtPhysicianCode.Text = entity.ParamedicCode;
            txtPhysicianName.Text = entity.ParamedicName;
            hdnRegistrationID.Value = paramSplit[0];
            hdnVisitIDCtlPopUp.Value = entity.VisitID.ToString();
            hdnRegistrationNo.Value = entity.RegistrationNo;
            hdnHealthcareServiceUnitID.Value = entity.HealthcareServiceUnitID.ToString();
            hdnDepartmentID.Value = entity.DepartmentID;
            hdnVisitTypeCtlID.Value = entity.VisitTypeID.ToString();
            txtVisitTypeCtlCode.Text = entity.VisitTypeCode;
            txtVisitTypeCtlName.Text = entity.VisitTypeName;
            IsAdd = false;
            chkIsParturitionEdit.Checked = entity.IsParturition;
            chkIsPregnantCtl.Checked = entity.IsPregnant;
            chkIsVisitorRestriction.Checked = entity.IsVisitorRestriction;
            chkIsNeedPastoralCare.Checked = entity.IsNeedPastoralCare;
            chkIsFastTrack.Checked = entity.IsFastTrack;

            if (entity.DepartmentID == Constant.Facility.INPATIENT)
            {
                chkIsParturitionEdit.Visible = true;
                chkIsVisitorRestriction.Visible = true;
                chkIsNewBornCtl.Visible = true;
            }
            else if (entity.DepartmentID == Constant.Facility.EMERGENCY)
            {
                chkIsParturitionEdit.Visible = true;
                //chkIsVisitorRestriction.Visible = false;
                chkIsNewBornCtl.Visible = true;
            }

            
            List<Specialty> lstSpecialty = BusinessLayer.GetSpecialtyList("IsDeleted = 0");
            Methods.SetComboBoxField<Specialty>(cboRegistrationEditSpecialty, lstSpecialty, "SpecialtyName", "SpecialtyID");
            cboRegistrationEditSpecialty.Value = entity.SpecialtyID.ToString();
            List<StandardCode> lstRefferal = BusinessLayer.GetStandardCodeList(string.Format("ParentID = '{0}'", Constant.StandardCode.REFERRAL));
            Methods.SetComboBoxField<StandardCode>(cboReferral, lstRefferal, "StandardCodeName", "StandardCodeID");
            cboReferral.Value = entity.GCReferrerGroup;
            hdnReferrerID.Value = entity.ReferrerID.ToString();
            hdnReferrerParamedicID.Value = entity.ReferrerParamedicID.ToString();
            chkIsNewBornCtl.Checked = entity.IsNewBorn;
            hdnGender.Value = entity.GCGender;
            if (entity.IsNewBorn)
            {
                trMotherRegNoCtl.Attributes.Remove("style");
                vPatientBirthRecord entityBirthRecord = BusinessLayer.GetvPatientBirthRecordList(string.Format("MRN = '{0}' AND IsDeleted = 0", entity.MRN)).FirstOrDefault();
                if (entityBirthRecord != null)
                {
                    hdnMotherVisitIDCtl.Value = entityBirthRecord.MotherVisitID.ToString();
                    hdnMotherMRNCtl.Value = entityBirthRecord.MotherMRN.ToString();
                    hdnMotherNameCtl.Value = entityBirthRecord.MotherFirstName;
                    txtMotherRegNoCtl.Text = entityBirthRecord.MotherRegistrationNo;
                }
            }

            if (entity.ReferrerID != 0)
            {
                txtReferralDescriptionCode.Text = entity.ReferrerCommCode;
                txtReferralDescriptionName.Text = entity.ReferrerName;
            }
            else if (entity.ReferrerParamedicID != 0)
            {
                txtReferralDescriptionCode.Text = entity.ReferrerParamedicCode;
                txtReferralDescriptionName.Text = entity.ReferrerParamedicName;
            }

            txtDiagnoseCodeRegEditCtl.Text = entity.DiagnoseID;
            txtDiagnoseNameRegEditCtl.Text = entity.DiagnoseName;
            txtDiagnoseTextRegEditCtl.Text = entity.DiagnosisText;

            GetSettingParameter();

            if (hdnIsParamedicInRegistrationUseScheduleCtl.Value == "1")
            {
                if (hdnDepartmentID.Value == Constant.Facility.EMERGENCY || hdnDepartmentID.Value == Constant.Facility.INPATIENT || hdnDepartmentID.Value == Constant.Facility.PHARMACY)
                {
                    trParamedicHasScheduleCtl.Style.Add("display", "none");
                }
                else
                {
                    trParamedicHasScheduleCtl.Style.Remove("display");
                }
            }
            else
            {
                trParamedicHasScheduleCtl.Style.Add("display", "none");
            }

            if (hdnIsOutpatientUsingRoom.Value == "1" && hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                trRoomCtl.Style.Add("display", "table-row");
            }
            else if (hdnIsOutpatientUsingRoom.Value == "0" && hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                trRoomCtl.Style.Add("display", "none");
            }
            else if (hdnIsOutpatientUsingRoom.Value == "1" && hdnDepartmentID.Value != Constant.Facility.OUTPATIENT)
            {
                trRoomCtl.Style.Add("display", "none");
            }
            else if (hdnIsOutpatientUsingRoom.Value == "0" && hdnDepartmentID.Value != Constant.Facility.OUTPATIENT)
            {
                trRoomCtl.Style.Add("display", "none");
            }
        }

        private void GetSettingParameter()
        {
            string filterExp = string.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}')",
                Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, Constant.SettingParameter.OP_IS_CHANGE_QUEUE_AFTER_CHANGE_PHYSICIAN, Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG);
            List<SettingParameter> lstSettingParameter = BusinessLayer.GetSettingParameterList(filterExp);
            hdnIsBridgingToGateway.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).ParameterValue;
            hdnProviderGatewayService.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).ParameterValue;
            hdnIsQueueChangeAfterEdit.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_IS_CHANGE_QUEUE_AFTER_CHANGE_PHYSICIAN).ParameterValue;
            hdnIsOutpatientUsingRoom.Value = lstSettingParameter.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.OP_PENDAFTARAN_DENGAN_RUANG).ParameterValue;


            List<SettingParameterDt> lstSettingParameterDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE));
            hdnIsParamedicInRegistrationUseScheduleCtl.Value = lstSettingParameterDt.FirstOrDefault(p => p.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).ParameterValue;
        }

        protected string GetDayNumber()
        {
            DateTime selectedDate = Helper.GetDatePickerValue(hdnRegistrationDate.Value);

            //daynumber diubah jika 0 jadi 7 karena di database jika hari minggu disimpan daynumber 7, sedangkan di coding daynumber untuk hari minggu adalah 0
            int daynumber = (int)selectedDate.DayOfWeek;
            if (daynumber == 0)
            {
                daynumber = 7;
            }

            return daynumber.ToString();
        }

        protected string OnGetParamedicFilterExpression()
        {
            return string.Format("ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = {0}) AND IsDeleted = 0 AND (IsHasPhysicianRole = 1)", hdnHealthcareServiceUnitID.Value);
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtPhysicianCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtVisitTypeCtlCode, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(txtDiagnoseCodeRegEditCtl, new ControlEntrySetting(true, true, false, string.Empty));
            SetControlEntrySetting(txtDiagnoseNameRegEditCtl, new ControlEntrySetting(false, false, false, string.Empty));
            SetControlEntrySetting(txtDiagnoseTextRegEditCtl, new ControlEntrySetting(true, true, false, string.Empty));
            SetControlEntrySetting(cboRegistrationEditSpecialty, new ControlEntrySetting(true, true, true));

            if (hdnDepartmentID.Value != Constant.Facility.EMERGENCY)
            {
                SetControlEntrySetting(hdnRoomIDCtl, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, false, true));
                SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, true));
            }
            else
            {
                SetControlEntrySetting(hdnRoomIDCtl, new ControlEntrySetting(true, true));
                SetControlEntrySetting(txtRoomCode, new ControlEntrySetting(true, false, false));
                SetControlEntrySetting(txtRoomName, new ControlEntrySetting(false, false, false));
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool isValid = true;
            bool result = true;
            int newQueue = 0;

            if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
            {
                if (hdnIsBridgingToGateway.Value == "1")
                {
                    //Healthcare entityHSU = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);
                    if (Constant.HealthcareGatewayProvider.RSDOSOBA == hdnProviderGatewayService.Value)
                    {
                        vConsultVisit entity = BusinessLayer.GetvConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value)).FirstOrDefault();
                        string queue = BridgingToGatewayGetQueueNo(entity.MedicalNo, entity.VisitDate, entity.ParamedicCode, txtPhysicianCode.Text, DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), entity.QueueNo.ToString(), entity.BusinessPartnerCode, "DT", hdnHealthcareServiceUnitID.Value.ToString());
                        //string queue = string.Empty;
                        string[] queueSplit = queue.Split('|');
                        if (queueSplit[0] == "1")
                        {
                            newQueue = Convert.ToInt16(queueSplit[1]);
                            result = true;
                            isValid = true;
                        }
                        else
                        {
                            errMessage = queueSplit[1];
                            isValid = false;
                        }
                    }
                }
            }

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                ConsultVisitDao entityConsultVisitDao = new ConsultVisitDao(ctx);
                RegistrationDao entityRegistrationDao = new RegistrationDao(ctx);
                RegistrationBPJSDao entityRegistrationBPJSDao = new RegistrationBPJSDao(ctx);
                ParamedicTeamDao entityParamedicTeamDao = new ParamedicTeamDao(ctx);
                PatientBirthRecordDao entityPatientBirthRecordDao = new PatientBirthRecordDao(ctx);
                PatientDao entityPatientDao = new PatientDao(ctx);
                PatientFamilyDao entityPatientFamilyDao = new PatientFamilyDao(ctx);
                PatientDiagnosisDao patientDiagnosisDao = new PatientDiagnosisDao(ctx);
                AuditLogDao entityAuditLogDao = new AuditLogDao(ctx);
                ParamedicMasterDao paramedicDao = new ParamedicMasterDao(ctx);
                PatientChargesDtDao chargesDtDao = new PatientChargesDtDao(ctx);
                try
                {
                    Registration entityRegistration = BusinessLayer.GetRegistration(Convert.ToInt32(hdnRegistrationID.Value));
                    ConsultVisit entity = BusinessLayer.GetConsultVisitList(string.Format("RegistrationID = {0} AND IsMainVisit = 1", hdnRegistrationID.Value), ctx).FirstOrDefault();

                    List<PatientChargesDt> lstChargesDt = new List<PatientChargesDt>();
                    if (entity.ParamedicID != Convert.ToInt32(hdnPhysicianID.Value))
                    {
                        ParamedicMaster pm = paramedicDao.Get(Convert.ToInt32(entity.ParamedicID));
                        if (pm.IsDummy == true || pm.IsHasRevenueSharing == false)
                        {
                            string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                            if (lstCharegesHd.Count > 0)
                            {
                                foreach (PatientChargesHd hd in lstCharegesHd)
                                {
                                    string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                                    foreach (PatientChargesDt e in lstDt)
                                    {
                                        e.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                        e.LastUpdatedBy = AppSession.UserLogin.UserID;

                                        ctx.CommandType = CommandType.Text;
                                        ctx.Command.Parameters.Clear();
                                        chargesDtDao.Update(e);
                                    }
                                }
                            }
                        }
                        else
                        {
                            string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();
                            List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                            if (lstCharegesHd.Count > 0)
                            {
                                foreach (PatientChargesHd hd in lstCharegesHd)
                                {
                                    string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                                    if (lstDt.Count > 0)
                                    {
                                        lstChargesDt.AddRange(lstDt);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        string filterChargesHd = string.Format("VisitID = {0} AND GCTransactionStatus != '{1}' AND IsAutoTransaction = 1", entity.VisitID, Constant.TransactionStatus.VOID);
                        ctx.CommandType = CommandType.Text;
                        ctx.Command.Parameters.Clear();
                        List<PatientChargesHd> lstCharegesHd = BusinessLayer.GetPatientChargesHdList(filterChargesHd, ctx);
                        if (lstCharegesHd.Count > 0)
                        {
                            foreach (PatientChargesHd hd in lstCharegesHd)
                            {
                                string filterCHargesDt = string.Format("TransactionID = {0} AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != '{1}' AND ParamedicID = '{2}'", hd.TransactionID, Constant.TransactionStatus.VOID, entity.ParamedicID);
                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                List<PatientChargesDt> lstDt = BusinessLayer.GetPatientChargesDtList(filterCHargesDt, ctx);
                                if (lstDt.Count > 0)
                                {
                                    lstChargesDt.AddRange(lstDt);
                                }
                            }
                        }
                    }

                    if (lstChargesDt.Count <= 0)
                    {
                        if (entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CLOSED && entityRegistration.GCRegistrationStatus != Constant.VisitStatus.CANCELLED)
                        {
                            AuditLog entityAuditLog = new AuditLog();
                            entityAuditLog.OldValues = JsonConvert.SerializeObject(entity);
                            entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                            entity.SpecialtyID = cboRegistrationEditSpecialty.Value.ToString();

                            if (!string.IsNullOrEmpty(hdnRoomIDCtl.Value) && Convert.ToInt32(hdnRoomIDCtl.Value) > 0)
                            {
                                entity.RoomID = Convert.ToInt32(hdnRoomIDCtl.Value);
                            }
                            else
                            {
                                entity.RoomID = null;
                            }

                            if (!string.IsNullOrEmpty(hdnVisitTypeCtlID.Value))
                            {
                                entity.VisitTypeID = Convert.ToInt32(hdnVisitTypeCtlID.Value);
                            }
                            if (hdnIsQueueChangeAfterEdit.Value == "1")
                            {
                                if (hdnDepartmentID.Value == Constant.Facility.OUTPATIENT)
                                {
                                    if (newQueue != 0)
                                    {
                                        entity.QueueNo = Convert.ToInt16(newQueue);
                                    }
                                    else
                                    {
                                        bool isBPJS = false;
                                        if (entityRegistration.GCCustomerType == Constant.CustomerType.BPJS)
                                        {
                                            isBPJS = true;
                                        }
                                        //entity.QueueNo = Convert.ToInt16(BusinessLayer.GetMaxQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(hdnPhysicianID.Value), entity.VisitDate, Convert.ToInt32(entity.Session), 0));
                                        entity.QueueNo = Convert.ToInt16(BusinessLayer.GetQueueNo(entity.HealthcareServiceUnitID, Convert.ToInt32(entity.ParamedicID), entity.VisitDate, Convert.ToInt32(entity.Session), false, isBPJS, 0, ctx, 1));
                                    }
                                }
                            }

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            PatientBirthRecord entityPatientBirth = BusinessLayer.GetPatientBirthRecordList(string.Format("MRN = {0} AND IsDeleted = 0", hdnMRNCtl.Value), ctx).FirstOrDefault();
                            entityRegistration.IsNewBorn = chkIsNewBornCtl.Checked;
                            if (hdnMotherMRNCtl.Value != string.Empty)
                            {
                                bool flagNewRecord = false;
                                if (entityPatientBirth == null)
                                {
                                    entityPatientBirth = new PatientBirthRecord();
                                    flagNewRecord = true;
                                }

                                #region Patient Birth Data
                                #region Data Bayi
                                entityPatientBirth.MRN = Convert.ToInt32(hdnMRNCtl.Value);
                                entityPatientBirth.VisitID = entity.VisitID;
                                #endregion
                                #region Data Ibu
                                entityPatientBirth.MotherMRN = Convert.ToInt32(hdnMotherMRNCtl.Value);
                                entityPatientBirth.MotherVisitID = Convert.ToInt32(hdnMotherVisitIDCtl.Value);
                                #endregion
                                #endregion

                                if (flagNewRecord)
                                {
                                    entityPatientBirth.CreatedBy = AppSession.UserLogin.UserID;
                                    entityPatientBirthRecordDao.Insert(entityPatientBirth);
                                }
                                else
                                {
                                    entityPatientBirth.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    entityPatientBirthRecordDao.Update(entityPatientBirth);
                                }

                                #region PatientFamily
                                Patient entityMother = entityPatientDao.Get(Convert.ToInt32(hdnMotherMRNCtl.Value));
                                PatientFamily entityPatientFamily = BusinessLayer.GetPatientFamilyList(string.Format("MRN = '{0}' AND GCFamilyRelation = '{1}'", hdnMRNCtl.Value, Constant.FamilyRelation.MOTHER), ctx).FirstOrDefault();
                                bool flagAddPatientFamily = false;
                                if (entityPatientFamily == null)
                                {
                                    entityPatientFamily = new PatientFamily();
                                    flagAddPatientFamily = true;
                                }
                                entityPatientFamily.MRN = Convert.ToInt32(hdnMRNCtl.Value);
                                entityPatientFamily.GCFamilyRelation = Constant.FamilyRelation.MOTHER;
                                entityPatientFamily.FamilyMRN = Convert.ToInt32(hdnMotherMRNCtl.Value);
                                entityPatientFamily.FirstName = entityMother.FirstName;
                                entityPatientFamily.LastName = entityMother.LastName;
                                entityPatientFamily.FullName = entityMother.FullName;
                                entityPatientFamily.AddressID = entityMother.HomeAddressID;
                                if (flagAddPatientFamily) entityPatientFamilyDao.Insert(entityPatientFamily);
                                else entityPatientFamilyDao.Update(entityPatientFamily);
                                #endregion

                            }
                            else
                            {
                                if (entityPatientBirth != null)
                                {
                                    entityPatientBirth.IsDeleted = true;
                                    entityPatientBirth.LastUpdatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    entityPatientBirthRecordDao.Update(entityPatientBirth);
                                }
                            }

                            if (hdnReferrerID.Value != "" && hdnReferrerID.Value != "0")
                            {
                                entityRegistration.ReferrerID = Convert.ToInt32(hdnReferrerID.Value);
                                entityRegistration.ReferrerParamedicID = null;
                            }
                            else if (hdnReferrerParamedicID.Value != "" && hdnReferrerParamedicID.Value != "0")
                            {
                                entityRegistration.ReferrerID = null;
                                entityRegistration.ReferrerParamedicID = Convert.ToInt32(hdnReferrerParamedicID.Value);
                            }
                            else
                            {
                                entityRegistration.ReferrerID = null;
                                entityRegistration.ReferrerParamedicID = null;
                            }

                            if (cboReferral.Value != null)
                            {
                                entityRegistration.GCReferrerGroup = cboReferral.Value.ToString();
                            }
                            else
                            {
                                entityRegistration.GCReferrerGroup = null;
                            }

                            entityRegistration.IsParturition = chkIsParturitionEdit.Checked;
                            entityRegistration.IsPregnant = chkIsPregnantCtl.Checked;
                            entityRegistration.IsVisitorRestriction = chkIsVisitorRestriction.Checked;
                            entityRegistration.IsNeedPastoralCare = chkIsNeedPastoralCare.Checked;
                            entityRegistration.IsFastTrack = chkIsFastTrack.Checked;
                            entityRegistration.LastUpdatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityRegistrationDao.Update(entityRegistration);

                            entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityConsultVisitDao.Update(entity);

                            if (entityRegistration.IsPregnant)
                            {
                                if (entityRegistration.MRN != null && entityRegistration.MRN != 0)
                                {
                                    Patient patient = entityPatientDao.Get(Convert.ToInt32(entityRegistration.MRN));
                                    patient.IsPregnant = true;
                                    patient.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();
                                    entityPatientDao.Update(patient);
                                }
                            }

                            #region Patient Diagnose
                            PatientDiagnosis diffDx = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND GCDiagnoseType = '{1}' AND IsDeleted = 0", entity.VisitID, Constant.DiagnoseType.EARLY_DIAGNOSIS), ctx).FirstOrDefault();
                            if (diffDx == null)
                            {
                                diffDx = new PatientDiagnosis();

                                diffDx.ParamedicID = Convert.ToInt32(entity.ParamedicID);
                                diffDx.DiagnoseID = txtDiagnoseCodeRegEditCtl.Text;
                                diffDx.DiagnosisText = txtDiagnoseTextRegEditCtl.Text;
                                diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                                diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                                diffDx.DifferentialDate = entityRegistration.RegistrationDate;
                                diffDx.DifferentialTime = entityRegistration.RegistrationTime;

                                if ((diffDx.DiagnoseID != "" && diffDx.DiagnoseID != null) || (diffDx.DiagnosisText.Trim() != "" && diffDx.DiagnosisText.Trim() != null))
                                {
                                    diffDx.VisitID = entity.VisitID;
                                    diffDx.CreatedBy = AppSession.UserLogin.UserID;

                                    ctx.CommandType = CommandType.Text;
                                    ctx.Command.Parameters.Clear();

                                    patientDiagnosisDao.Insert(diffDx);
                                }
                            }
                            else
                            {
                                diffDx.ParamedicID = Convert.ToInt32(entity.ParamedicID);
                                diffDx.DiagnoseID = txtDiagnoseCodeRegEditCtl.Text;
                                diffDx.DiagnosisText = txtDiagnoseTextRegEditCtl.Text;
                                diffDx.GCDiagnoseType = Constant.DiagnoseType.EARLY_DIAGNOSIS;
                                diffDx.GCDifferentialStatus = Constant.DifferentialDiagnosisStatus.UNDER_INVESTIGATION;
                                diffDx.DifferentialDate = entityRegistration.RegistrationDate;
                                diffDx.DifferentialTime = entityRegistration.RegistrationTime;

                                diffDx.VisitID = entity.VisitID;
                                diffDx.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                patientDiagnosisDao.Update(diffDx);
                            }
                            #endregion

                            RegistrationBPJS regBPJS = entityRegistrationBPJSDao.Get(entity.RegistrationID);
                            if (regBPJS != null)
                            {
                                ParamedicMasterDao entityParamedicDao = new ParamedicMasterDao(ctx);
                                ParamedicMaster paramedicBPJS = entityParamedicDao.Get(Convert.ToInt32(hdnPhysicianID.Value));
                                if (paramedicBPJS != null)
                                {
                                    if (paramedicBPJS.BPJSReferenceInfo != null && paramedicBPJS.BPJSReferenceInfo != "")
                                    {
                                        string[] bpjsInfo = paramedicBPJS.BPJSReferenceInfo.Split(';');
                                        string[] hfisInfo = bpjsInfo[1].Split('|');
                                        regBPJS.KodeDPJP = hfisInfo[0];
                                    }

                                }

                                if (!string.IsNullOrEmpty(txtDiagnoseCodeRegEditCtl.Text))
                                {
                                    DiagnoseDao entityDiagnoseDao = new DiagnoseDao(ctx);
                                    Diagnose diagnoseBPJS = entityDiagnoseDao.Get(txtDiagnoseCodeRegEditCtl.Text);
                                    if (diagnoseBPJS != null)
                                    {
                                        regBPJS.KodeDiagnosa = diagnoseBPJS.BPJSReferenceInfo;
                                        regBPJS.NamaDiagnosa = diagnoseBPJS.DiagnoseName;
                                    }

                                }

                                regBPJS.KodeRujukan = txtReferralDescriptionCode.Text;
                                regBPJS.NamaRujukan = Request.Form[txtReferralDescriptionName.UniqueID];

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();
                                regBPJS.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityRegistrationBPJSDao.Update(regBPJS);
                            }

                            entityAuditLog.ObjectType = Constant.BusinessObjectType.VISIT;
                            entityAuditLog.NewValues = JsonConvert.SerializeObject(entity);
                            entityAuditLog.UserID = AppSession.UserLogin.UserID;
                            entityAuditLog.LogDate = DateTime.Now;
                            entityAuditLog.TransactionID = entity.VisitID;

                            ctx.CommandType = CommandType.Text;
                            ctx.Command.Parameters.Clear();

                            entityAuditLogDao.Insert(entityAuditLog);
                            retval = hdnRegistrationNo.Value;
                            if (hdnDepartmentID.Value.Equals(Constant.Facility.INPATIENT))
                            {
                                SettingParameterDt entitySetPar = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode = '{0}'", Constant.SettingParameter.IP_CODE_TYPE_DOCTOR_DPJP), ctx)[0];
                                ParamedicTeam entityParamedicTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND GCParamedicRole = '{1}'", hdnRegistrationID.Value, entitySetPar.ParameterValue.ToString()), ctx)[0];
                                entityParamedicTeam.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                                entityParamedicTeam.LastUpdatedBy = AppSession.UserLogin.UserID;

                                ctx.CommandType = CommandType.Text;
                                ctx.Command.Parameters.Clear();

                                entityParamedicTeamDao.Update(entityParamedicTeam);
                            }
                            ctx.CommitTransaction();
                        }
                        else
                        {
                            result = false;
                            errMessage = "Maaf, tidak dapat mengubah data registrasi karna status pendaftaran sudah ditutup (closed).";
                            Exception ex = new Exception(errMessage);
                            Helper.InsertErrorLog(ex);
                            ctx.RollBackTransaction();
                        }
                    }
                    else
                    {
                        result = false;
                        errMessage = "Maaf, tidak dapat mengubah data registrasi karna masih memiliki Transaksi Auto Bill dari dokter saat ini.";
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
            else
            {
                result = false;
            }
            return result;
        }

        protected string GetGenderFemale()
        {
            return Constant.Gender.FEMALE;
        }

        private string BridgingToGatewayGetQueueNo(string medicalNo, DateTime date, string oldParamedicCode, string newParamedicCode, string hour, string queueNo, string businessPartnerCode, string via, string healthcareServiceUnitID)
        {
            String queue = "";

            if (hdnIsBridgingToGateway.Value == "1")
            {
                GatewayService oService = new GatewayService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "QUEUE ENGINE";
                string apiResult = oService.OnRegistrationChangePhysician(medicalNo, date, oldParamedicCode, newParamedicCode, hour, queueNo, businessPartnerCode, via, healthcareServiceUnitID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    queue = apiResult;
                    entityAPILog.MessageDateTime = DateTime.Now;
                    entityAPILog.MessageText = apiResultInfo[2];
                    entityAPILog.Response = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }

            return queue;
        }
    }
}