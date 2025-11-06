using System;
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
    public partial class ReferralVisitCtl1 : BaseViewPopupCtl
    {
        protected bool IsAllowEditPatientVisit = true;
        public override void InitializeDataControl(string param)
        {
            if (param != "")
            {
                List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.SA0116, Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE));
                hdnIsValidateParamedicSchedule.Value = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.SA0116).FirstOrDefault().ParameterValue;
                hdnIsParamedicInRegistrationUseScheduleCtl.Value = lstSetvarDt.Where(t => t.ParameterCode == Constant.SettingParameter.RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE).FirstOrDefault().ParameterValue;
                hdnItemCardFee.Value = BusinessLayer.GetSettingParameter(Constant.SettingParameter.FN_KODE_PELAYANAN_KARTU).ParameterValue;
                if (string.IsNullOrEmpty(hdnItemCardFee.Value)) hdnItemCardFee.Value = "0";
                txtMRN.ReadOnly = true;
                txtPatientName.ReadOnly = true;
                txtRegistrationNo.ReadOnly = true;

                hdnRegistrationID.Value = param;

                if (hdnIsParamedicInRegistrationUseScheduleCtl.Value == "1")
                {
                    trParamedicHasScheduleCtl.Style.Remove("display");
                }
                else
                {
                    trParamedicHasScheduleCtl.Style.Add("display", "none");
                }

                hdnRegistrationID.Value = AppSession.RegisteredPatient.RegistrationID.ToString();
                txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo;
                txtMRN.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
                hdnClassID.Value = AppSession.RegisteredPatient.ChargeClassID.ToString();
                hdnBusinessPartnerID.Value = AppSession.RegisteredPatient.BusinessPartnerID.ToString();
                txtAppointmentDate.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

                //IsAllowEditPatientVisit = (AppSession.RegisteredPatient.VisitDate == DateTime.Today && AppSession.RegisteredPatient.GCRegistrationStatus != Constant.VisitStatus.CANCELLED && AppSession.RegisteredPatient.GCRegistrationStatus != Constant.VisitStatus.CLOSED);

                //if (!IsAllowEditPatientVisit)
                //    divContainerAddData.Style.Add("display", "none");

                BindGridView();

                txtDiagnosisText.Attributes.Add("validationgroup", "mpPatientVisit");
                txtMedicalResumeText.Attributes.Add("validationgroup", "mpPatientVisit");
                txtPlanningResumeText.Attributes.Add("validationgroup", "mpPatientVisit");
                txtAppointmentDate.Attributes.Add("validationgroup", "mpPatientVisit");


                List<vHealthcareServiceUnit> lstServiceUnit = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("DepartmentID = '{0}' AND IsDeleted = 0 AND IsUsingRegistration = 1", Constant.Facility.OUTPATIENT));
                Methods.SetComboBoxField<vHealthcareServiceUnit>(cboServiceUnit, lstServiceUnit, "ServiceUnitName", "HealthcareServiceUnitID");
                cboServiceUnit.SelectedIndex = 0;
                hdnDateToday.Value = Helper.GetCurrentDate().ToString(Constant.FormatString.DATE_PICKER_FORMAT);
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

        private void BindGridView()
        {
            string filterExpression = string.Empty;
            if (filterExpression != "")
                filterExpression += " AND ";
            filterExpression += string.Format("VisitID = {0} AND IsDeleted = 0", AppSession.RegisteredPatient.VisitID);

            List<vPatientReferral> lstView = BusinessLayer.GetvPatientReferralList(filterExpression);
            grdView.DataSource = lstView;
            grdView.DataBind();
        }

        protected void cbpPatientVisitTransHd_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "save")
            {
                result = "save|";
                if (hdnPatientReferralID.Value.ToString() != "")
                {
                    if (OnSaveEditRecord(ref errMessage))
                        result += "success";
                    else
                    {
                        if (errMessage.Contains('|'))
                        {
                            result += string.Format("fail|{0}|{1}", errMessage.Split('|')[0], errMessage.Split('|')[1]);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
                else
                {
                    if (OnSaveAddRecord(ref errMessage))
                        result += "success";
                    else
                    {
                        if (errMessage.Contains('|'))
                        {
                            result += string.Format("fail|{0}|{1}", errMessage.Split('|')[0], errMessage.Split('|')[1]);
                        }
                        else
                        {
                            result += string.Format("fail|{0}", errMessage);
                        }
                    }
                }
            }
            else if (e.Parameter == "refresh")
            {
                result = "refresh|";
            }
            else
            {
                result = "delete|";
                if (OnDeleteRecord(ref errMessage))
                    result += "success";
                else
                    result += string.Format("fail|{0}", errMessage);
            }
            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpConfirmSave_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string result = "";
            string errMessage = "";
            if (e.Parameter == "confirm")
            {
                result = "confirm|";
                if (hdnPatientReferralID.Value != "")
                {
                    if (OnConfirmEditRecord(ref errMessage))
                    {
                        result += "success";
                    }
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
                else
                {
                    if (OnConfirmAddRecord(ref errMessage))
                        result += "success";
                    else
                    {
                        result += string.Format("fail|{0}", errMessage);
                    }
                }
            }

            BindGridView();

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        protected void cbpViewPopUpCtl_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
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

        private bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            bool isValid = true;
            if (hdnIsValidateParamedicSchedule.Value == "1")
            {
                if (rblReferralType.SelectedValue == "2")
                {
                    if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
                    {
                        errMessage += "confirmation|";
                        vParamedicSchedule obj = null;
                        vParamedicScheduleDate objSchDate = null;
                        if (!Helper.ValidateParamedicSchedule(obj, objSchDate, Convert.ToInt32(hdnPhysicianID.Value), Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(txtAppointmentDate.Text), Constant.Facility.OUTPATIENT, ref errMessage))
                        {
                            isValid = false;
                        }
                    }
                }
            }

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
                ParamedicTeamDao teamDao = new ParamedicTeamDao(ctx);
                try
                {
                    #region Patient Referral Information
                    PatientReferral entity = new PatientReferral();
                    ReferralControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    if (rblReferralType.SelectedValue == "0")
                    {
                        List<ParamedicTeam> entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, Convert.ToInt32(hdnPhysicianID.Value.ToString())), ctx);
                        if (entityTeam.Count < 1)
                        {
                            ParamedicTeam obj = new ParamedicTeam();
                            obj.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                            obj.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value.ToString());
                            obj.GCParamedicRole = Constant.ParamedicRole.KONSULEN;
                            obj.CreatedBy = AppSession.UserLogin.UserID;
                            teamDao.Insert(obj);
                        }
                    }
                    entityReferralDao.Insert(entity);
                    #endregion
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
            else
            {
                result = false;
            }

            return result;
        }

        private bool OnConfirmAddRecord(ref string errMessage)
        {
            bool result = true;
            bool isValid = true;
            //if (rblReferralType.SelectedValue == "2")
            //{
            //    if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
            //    {
            //        vParamedicSchedule obj = null;
            //        vParamedicScheduleDate objSchDate = null;
            //        if (!Helper.ValidateParamedicSchedule(obj, objSchDate, Convert.ToInt32(hdnPhysicianID.Value), Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(txtAppointmentDate.Text), Constant.Facility.OUTPATIENT, ref errMessage))
            //        {
            //            isValid = false;
            //        }
            //    }
            //}

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientReferralDao entityReferralDao = new PatientReferralDao(ctx);
                AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);
                try
                {
                    #region Patient Referral Information
                    PatientReferral entity = new PatientReferral();
                    ReferralControlToEntity(entity);
                    entity.CreatedBy = AppSession.UserLogin.UserID;
                    if (rblReferralType.SelectedValue == "2")
                    {
                        AppointmentRequest entityApmReq = new AppointmentRequest();
                        AppointmentRequestControlToEntity(entityApmReq);
                        entityApmReq.CreatedBy = AppSession.UserLogin.UserID;
                        entityApmReq.CreatedDate = DateTime.Now;
                        int apmReqID = entityApmReqDao.InsertReturnPrimaryKeyID(entityApmReq);
                        entity.AppointmentRequestID = apmReqID;
                    }
                    entityReferralDao.Insert(entity);
                    #endregion

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
            else
            {
                result = false;
            }

            return result;
        }

        private bool OnConfirmEditRecord(ref string errMessage)
        {
            bool result = true;
            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao entityDao = new PatientReferralDao(ctx);
            AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);
            try
            {
                PatientReferral entityReferral = BusinessLayer.GetPatientReferral(Convert.ToInt32(hdnPatientReferralID.Value));
                if (entityReferral != null)
                {
                    entityReferral.DiagnosisText = txtDiagnosisText.Text;
                    entityReferral.MedicalResumeText = txtMedicalResumeText.Text;
                    entityReferral.PlanningResumeText = txtPlanningResumeText.Text;
                    entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entityReferral);

                    if (entityReferral.GCRefferalType == Constant.ReferralType.APPOINTMENT)
                    {
                        AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(entityReferral.AppointmentRequestID));
                        if (entityApmReq != null)
                        {
                            AppointmentRequestControlToEntity(entityApmReq);
                            entityApmReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                            entityApmReq.LastUpdatedDate = DateTime.Now;
                            entityApmReqDao.Update(entityApmReq);
                        }
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                ctx.RollBackTransaction();
                Helper.InsertErrorLog(ex);
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void AppointmentRequestControlToEntity(AppointmentRequest entity)
        {
            entity.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.MRN = AppSession.RegisteredPatient.MRN;
            entity.HealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            entity.Remarks = string.Format("Rujukan Pasien Internal dari No. Registrasi {0}", AppSession.RegisteredPatient.RegistrationNo);
            if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
            {
                if (hdnPhysicianID.Value != "0")
                {
                    entity.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                }
            }
            entity.VisitTypeID = 1;
            entity.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate.Text);
        }

        private void ReferralControlToEntity(PatientReferral entity)
        {
            string referenceNoPrefix = AppSession.RegisteredPatient.RegistrationNo;
            int rowCount = grdView.Rows.Count + 1;

            entity.VisitID = AppSession.RegisteredPatient.VisitID;
            entity.ReferralDate = AppSession.RegisteredPatient.VisitDate;
            entity.ReferralTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
            entity.ReferenceNo = string.Format("{0}-{1}", AppSession.RegisteredPatient.RegistrationNo.Replace(@"/","."),rowCount.ToString().PadLeft(2,'0'));
            entity.FromPhysicianID = Convert.ToInt32(AppSession.UserLogin.ParamedicID);

            if (rblReferralType.SelectedValue == "1")
            {
                entity.GCRefferalType = "X075^04";
            }
            else if (rblReferralType.SelectedValue == "0")
            {
                entity.GCRefferalType = Constant.ReferralType.KONSULTASI;
            }
            else
            {
                entity.GCRefferalType = Constant.ReferralType.APPOINTMENT;
                entity.ScheduleDate = Helper.GetDatePickerValue(txtAppointmentDate);
            }

            if (rblReferralType.SelectedValue == "0")
            {
                entity.ToVisitID = AppSession.RegisteredPatient.VisitID;
                entity.IsProceed = true;
                entity.ProceedBy = AppSession.UserLogin.UserID;
                entity.ProceedDateTime = DateTime.Now;
            }

            entity.ToHealthcareServiceUnitID = Convert.ToInt32(cboServiceUnit.Value);
            if (!string.IsNullOrEmpty(hdnPhysicianID.Value) && hdnPhysicianID.Value != "0")
            {
                entity.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
            }
            entity.DiagnosisText = txtDiagnosisText.Text;
            entity.MedicalResumeText = txtMedicalResumeText.Text;
            entity.PlanningResumeText = txtPlanningResumeText.Text;
        }

        private bool OnSaveEditRecord(ref string errMessage)
        {
            bool result = true;
            bool isValid = true;
            if (hdnIsValidateParamedicSchedule.Value == "1")
            {
                if (rblReferralType.SelectedValue == "2")
                {
                    if (!string.IsNullOrEmpty(hdnPhysicianID.Value))
                    {
                        errMessage += "confirmation|";
                        vParamedicSchedule obj = null;
                        vParamedicScheduleDate objSchDate = null;
                        if (!Helper.ValidateParamedicSchedule(obj, objSchDate, Convert.ToInt32(hdnPhysicianID.Value), Convert.ToInt32(cboServiceUnit.Value), Helper.GetDatePickerValue(txtAppointmentDate.Text), Constant.Facility.OUTPATIENT, ref errMessage))
                        {
                            isValid = false;
                        }
                    }
                }
            }

            if (isValid)
            {
                IDbContext ctx = DbFactory.Configure(true);
                PatientReferralDao entityDao = new PatientReferralDao(ctx);
                ParamedicTeamDao teamDao = new ParamedicTeamDao(ctx);
                AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);
                try
                {
                    PatientReferral entityReferral = BusinessLayer.GetPatientReferral(Convert.ToInt32(hdnPatientReferralID.Value));
                    int oldPhysicianID = 0;
                    if (entityReferral != null)
                    {
                        oldPhysicianID = Convert.ToInt32(entityReferral.ToPhysicianID);

                        if (rblReferralType.SelectedValue == "0")
                            entityReferral.ToVisitID = AppSession.RegisteredPatient.VisitID;
                        else if (rblReferralType.SelectedValue == "2")
                            entityReferral.ScheduleDate = Helper.GetDatePickerValue(txtAppointmentDate);

                        entityReferral.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
                        entityReferral.DiagnosisText = txtDiagnosisText.Text;
                        entityReferral.MedicalResumeText = txtMedicalResumeText.Text;
                        entityReferral.PlanningResumeText = txtPlanningResumeText.Text;
                        entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(entityReferral);

                        if (rblReferralType.SelectedValue == "0")
                        {
                            if (hdnPhysicianID.Value != oldPhysicianID.ToString())
                            {
                                //Update Old Paramedic Team : Set IsDeleted = 1
                                ParamedicTeam entityTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, oldPhysicianID), ctx).FirstOrDefault();
                                if (entityTeam != null)
                                {
                                    entityTeam.IsDeleted = true;
                                    entityTeam.LastUpdatedBy = AppSession.UserLogin.UserID;
                                    teamDao.Update(entityTeam);
                                }

                                List<ParamedicTeam> lstTeam = BusinessLayer.GetParamedicTeamList(string.Format("RegistrationID = {0} AND ParamedicID = {1} AND IsDeleted = 0", AppSession.RegisteredPatient.RegistrationID, Convert.ToInt32(hdnPhysicianID.Value.ToString())), ctx);
                                if (lstTeam.Count < 1)
                                {
                                    ParamedicTeam obj = new ParamedicTeam();
                                    obj.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                                    obj.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value.ToString());
                                    obj.GCParamedicRole = Constant.ParamedicRole.KONSULEN;
                                    obj.CreatedBy = AppSession.UserLogin.UserID;
                                    teamDao.Insert(obj);
                                }
                            }
                        }
                    }
                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    ctx.RollBackTransaction();
                    Helper.InsertErrorLog(ex);
                    result = false;
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

        private bool OnDeleteRecord(ref string errMessage)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            PatientReferralDao patientReferralDao = new PatientReferralDao(ctx);
            AppointmentRequestDao entityApmReqDao = new AppointmentRequestDao(ctx);
            try
            {
                PatientReferral entityReferral = patientReferralDao.Get(Convert.ToInt32(hdnPatientReferralID.Value));
                if (entityReferral.ToVisitID != null || entityReferral.ToAppointmentID != null)
                {
                    result = false;
                    errMessage = "Rujukan tidak dapat dihapus karena sudah diproses menjadi kunjungan atau appointment.";
                    Exception ex = new Exception(errMessage);
                    Helper.InsertErrorLog(ex);
                    ctx.RollBackTransaction();
                }
                else
                {
                    if (entityReferral != null)
                    {
                        entityReferral.IsDeleted = true;
                        entityReferral.LastUpdatedBy = AppSession.UserLogin.UserID;

                        if (entityReferral.GCRefferalType == Constant.ReferralType.APPOINTMENT)
                        {
                            AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(entityReferral.AppointmentRequestID));
                            if (entityApmReq != null)
                            {
                                entityApmReq.GCDeleteReason = Constant.StandardCode.DELETE_REASON_APPOINTMENT + "^001";
                                entityApmReq.IsDeleted = true;
                                entityApmReq.DeleteReason = string.Empty;
                                entityApmReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                                entityApmReq.LastUpdatedDate = DateTime.Now;
                                entityApmReqDao.Update(entityApmReq);
                            }
                        }
                        patientReferralDao.Update(entityReferral);
                    }
                    ctx.CommitTransaction();
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

        protected void cbpSOAPCopy_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            string result = "";
            string errMessage = "";
            string diagnosisSummary = string.Empty;
            string planningSummary = string.Empty;
            string medicationSummary = string.Empty;

            try
            {
                #region Assessment Content
                StringBuilder sbNotes = new StringBuilder();
                string filterExpression = string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY GCDiagnoseType", AppSession.RegisteredPatient.VisitID);
                List<vPatientDiagnosis> lstDiagnosis = BusinessLayer.GetvPatientDiagnosisList(filterExpression);
                if (lstDiagnosis.Count > 0)
                {
                    foreach (vPatientDiagnosis item in lstDiagnosis)
                    {
                        if (string.IsNullOrEmpty(item.DiagnoseID))
                            sbNotes.AppendLine(string.Format("- {0}", item.cfDiagnosisText));
                        else
                            sbNotes.AppendLine(string.Format("- {0} ({1})", item.cfDiagnosisText, item.DiagnoseID));
                    }
                }
                diagnosisSummary = sbNotes.ToString();
                #endregion

                #region Objective Content
                string vitalSummary = string.Empty;
                sbNotes = new StringBuilder();
                List<vVitalSignDt> lstVitalSignDt = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0} AND IsDeleted = 0 ORDER BY DisplayOrder", AppSession.RegisteredPatient.VisitID));
                if (lstVitalSignDt.Count > 0)
                {
                    foreach (vVitalSignDt vitalSign in lstVitalSignDt)
                    {
                        sbNotes.AppendLine(string.Format(" {0} {1} {2}", vitalSign.VitalSignLabel, vitalSign.VitalSignValue, vitalSign.ValueUnit));
                    }
                }

                vitalSummary = sbNotes.ToString();

                string rosSummary = string.Empty;
                sbNotes = new StringBuilder();
                List<vReviewOfSystemDt> lstROS = BusinessLayer.GetvReviewOfSystemDtList(string.Format("ID IN (SELECT ID FROM ReviewOfSystemHd WHERE VisitID = {0}) AND IsDeleted = 0 ORDER BY GCRoSystem", AppSession.RegisteredPatient.VisitID));
                if (lstROS.Count > 0)
                {
                    foreach (vReviewOfSystemDt item in lstROS)
                    {
                        sbNotes.AppendLine(string.Format(" {0}: {1}", item.ROSystem, item.cfRemarks));
                    }
                }
                rosSummary = sbNotes.ToString();
                planningSummary = planningSummary + string.Format("Pemeriksaan Fisik: {0}{1}{2}{3}{4}", Environment.NewLine, vitalSummary, Environment.NewLine, rosSummary, Environment.NewLine);
                #endregion

                #region Planning Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1}  AND IsDeleted=0", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
                List<vTestOrderDt> lstOrder = BusinessLayer.GetvTestOrderDtList(filterExpression);
                if (lstOrder.Count > 0)
                {
                    foreach (vTestOrderDt item in lstOrder)
                    {
                        sbNotes.AppendLine(string.Format("- {0}", item.ItemName1));
                    }
                }

                planningSummary = planningSummary + sbNotes.ToString();
                #endregion

                #region Medication Content
                sbNotes = new StringBuilder();
                filterExpression = string.Format("VisitID = {0} AND ParamedicID = {1} AND OrderIsDeleted=0 AND IsRFlag = 1", AppSession.RegisteredPatient.VisitID, AppSession.UserLogin.ParamedicID);
                List<vPrescriptionOrderDt1> lstPrescription = BusinessLayer.GetvPrescriptionOrderDt1List(filterExpression);
                if (lstPrescription.Count > 0)
                {
                    foreach (vPrescriptionOrderDt1 item in lstPrescription)
                    {
                        if (item.IsRFlag)
                            sbNotes.AppendLine(string.Format("- {0} {1}x{2}", item.DrugName, item.Frequency, item.NumberOfDosage.ToString("G29")));
                        else
                            sbNotes.AppendLine(string.Format("  {0}", item.DrugName));
                    }
                }
                medicationSummary = sbNotes.ToString();
                #endregion

                result += string.Format("success|{0}~{1}~{2}", diagnosisSummary, planningSummary, medicationSummary);
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                result += string.Format("fail|{0}", errMessage);
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
            panel.JSProperties["cpTransactionID"] = 0;
        }
    }
}