using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using System.Text;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class ProcessReferralCtl : BasePagePatientPageEntryCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            string recordID = paramInfo[0];
            string healthcareServiceUnitID = paramInfo[1];
            string serviceUnitCode = paramInfo[2];
            string serviceUnitName = paramInfo[3];
            string paramedicID = paramInfo[4];
            string paramedicCode = paramInfo[5];
            string paramedicName = paramInfo[6];
            string specialtyID = paramInfo[7];

            SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.EM_SALIN_CATATAN_KONSULTASI)).FirstOrDefault();

            if (oParam != null)
                hdnIsCopyFromSource.Value = oParam.ParameterValue;
            else
                hdnIsCopyFromSource.Value = "0";

            hdnHealthcareServiceUnitID.Value = healthcareServiceUnitID;

            List<vServiceUnitVisitType> visitTypeList = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID));
            Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            cboFollowupVisitType.SelectedIndex = 0;

            SetControlProperties();

            if (!string.IsNullOrEmpty(recordID))
            {
                IsAdd = false;
                vPatientReferral obj = BusinessLayer.GetvPatientReferralList(string.Format("ID = {0}", recordID)).FirstOrDefault();
                if (obj != null)
                {
                    hdnID.Value = obj.ID.ToString();
                    hdnFromRegistrationID.Value = obj.RegistrationID.ToString();
                    EntityToControl(obj);
                }
                else
                {
                    hdnID.Value = "0";
                }
            }
        }

        protected override void OnControlEntrySetting()
        {
        }

        protected override bool OnSaveAddRecord(ref string errMessage)
        {
            bool result = true;
            errMessage = string.Empty;
            return result;
        }

        protected override bool OnSaveEditRecord(ref string errMessage)
        {
            IDbContext ctx = DbFactory.Configure(true);

            try
            {
                PatientReferralDao oReferralDao = new PatientReferralDao(ctx);
                PatientReferral entity = oReferralDao.Get(Convert.ToInt32(hdnID.Value));
                ControlToEntity(entity);
                entity.IsProceed = true;
                entity.ProceedDateTime = DateTime.Now;
                entity.ProceedBy = AppSession.UserLogin.UserID;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;

                AppointmentRequestDao oAppointmentRequestDao = new AppointmentRequestDao(ctx);
               
                int appointmentID = Convert.ToInt32(hdnAppointmentRequestID.Value);
               
                if (rblReferralType.SelectedValue == Constant.ReferralType.APPOINTMENT)
                {
                    #region Appointment Information
                    AppointmentRequest oAppointmentRequest;
                    Registration oReg = BusinessLayer.GetRegistrationList(string.Format("RegistrationID='{0}'", hdnFromRegistrationID.Value)).FirstOrDefault();

                    AppointmentRequest oApmreg = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentDate='{0}'  AND HealthcareServiceUnitID={1} AND ParamedicID='{2}' AND MRN='{3}'", 
                        Helper.GetDatePickerValue(txtAppointmentDate), 
                        entity.ToHealthcareServiceUnitID, 
                        entity.ToPhysicianID, 
                        oReg.MRN
                        )).FirstOrDefault();

                    bool isApmReqIsAxist = false;
                    if (oApmreg != null)
                    {
                        isApmReqIsAxist = true;
                    }

                    if (isApmReqIsAxist == false)
                    {
                        if (!string.IsNullOrEmpty(hdnAppointmentRequestID.Value) && hdnAppointmentRequestID.Value != "0")
                        {

                            oAppointmentRequest = oAppointmentRequestDao.Get(Convert.ToInt32(hdnAppointmentRequestID.Value));
                            if (oAppointmentRequest != null)
                            {
                                oAppointmentRequest.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate);
                                oAppointmentRequest.AppointmentTime = txtAppointmentTime.Text;
                                oAppointmentRequest.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                                oAppointmentRequest.Remarks = txtAppointmentRemarks.Text;
                                oAppointmentRequest.LastUpdatedBy = AppSession.UserLogin.UserID;
                                oAppointmentRequest.LastUpdatedDate = DateTime.Now;

                                #region rekanan informasi
                                oAppointmentRequest.GCCustomerType = oReg.GCCustomerType;
                                if (oReg.GCCustomerType == Constant.CustomerType.PERSONAL)
                                {
                                    oAppointmentRequest.BusinessPartnerID = 1;
                                    oAppointmentRequest.ContractID = null;
                                    oAppointmentRequest.CoverageTypeID = null;
                                    oAppointmentRequest.CoverageLimitAmount = 0;
                                    oAppointmentRequest.IsCoverageLimitPerDay = false;
                                    oAppointmentRequest.GCTariffScheme = null;
                                    oAppointmentRequest.IsControlClassCare = false;
                                    oAppointmentRequest.EmployeeID = null;

                                }
                                else
                                {
                                    oAppointmentRequest.BusinessPartnerID = oReg.BusinessPartnerID;
                                    string filterexpressionContract = string.Format("BusinessPartnerID = {0} AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0", oAppointmentRequest.BusinessPartnerID);
                                    CustomerContract oCsContract = BusinessLayer.GetCustomerContractList(filterexpressionContract).FirstOrDefault();
                                    if (oCsContract != null)
                                    {

                                        string filtexexpres2 = string.Format(" {0} AND ContractNo = '{1}' ", filterexpressionContract, oCsContract.ContractNo);
                                        vCustomerContract oVcsContract = BusinessLayer.GetvCustomerContractList(filtexexpres2).FirstOrDefault();
                                        if (oVcsContract != null)
                                        {
                                            oAppointmentRequest.ContractID = oVcsContract.ContractID;
                                        }
                                    }
                                    else
                                    {
                                        oAppointmentRequest.ContractID = null;
                                    }

                                    oAppointmentRequest.IsControlClassCare = oReg.IsControlClassCare;
                                    oAppointmentRequest.IsCoverageLimitPerDay = oReg.IsCoverageLimitPerDay;
                                    oAppointmentRequest.GCTariffScheme = oReg.GCTariffScheme;
                                    oAppointmentRequest.CoverageLimitAmount = oReg.CoverageLimitAmount;
                                    oAppointmentRequest.CoverageTypeID = oReg.CoverageTypeID;
                                    oAppointmentRequest.EmployeeID = oReg.EmployeeID;
                                }
                                #endregion

                                oAppointmentRequestDao.Update(oAppointmentRequest);
                                entity.AppointmentRequestID = appointmentID;
                            }
                        }
                        else
                        {
                            if (cboFollowupVisitType.Value != null)
                            {
                                oAppointmentRequest = new AppointmentRequest();
                                oAppointmentRequest.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                                oAppointmentRequest.VisitID = AppSession.RegisteredPatient.VisitID;
                                oAppointmentRequest.MRN = AppSession.RegisteredPatient.MRN;
                                oAppointmentRequest.HealthcareServiceUnitID = Convert.ToInt32(entity.ToHealthcareServiceUnitID);
                                oAppointmentRequest.ParamedicID = Convert.ToInt32(entity.ToPhysicianID);
                                oAppointmentRequest.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                                oAppointmentRequest.AppointmentDate = Helper.GetDatePickerValue(txtAppointmentDate);
                                oAppointmentRequest.AppointmentTime = txtAppointmentTime.Text;
                                oAppointmentRequest.GCReferrerGroup = Constant.Referrer.DOKTER_RS;
                                oAppointmentRequest.ReferrerParamedicID = entity.FromPhysicianID;
                                oAppointmentRequest.IsReferralVisit = true;
                                oAppointmentRequest.ReferenceNo = entity.ReferenceNo;
                                oAppointmentRequest.Remarks = string.IsNullOrEmpty(txtAppointmentRemarks.Text) ? string.Format("Rujukan Pasien dari {0}", Request.Form[txtFromPhysicianName.UniqueID]) : txtAppointmentRemarks.Text;
                                oAppointmentRequest.CreatedBy = AppSession.UserLogin.UserID;
                                oAppointmentRequest.CreatedDate = DateTime.Now;


                                #region rekanan informasi
                                oAppointmentRequest.GCCustomerType = oReg.GCCustomerType;
                                if (oReg.GCCustomerType == Constant.CustomerType.PERSONAL)
                                {
                                    oAppointmentRequest.BusinessPartnerID = 1;
                                    oAppointmentRequest.ContractID = null;
                                    oAppointmentRequest.CoverageTypeID = null;
                                    oAppointmentRequest.CoverageLimitAmount = 0;
                                    oAppointmentRequest.IsCoverageLimitPerDay = false;
                                    oAppointmentRequest.GCTariffScheme = null;
                                    oAppointmentRequest.IsControlClassCare = false;
                                    oAppointmentRequest.EmployeeID = null;

                                }
                                else
                                {
                                    oAppointmentRequest.BusinessPartnerID = oReg.BusinessPartnerID;
                                    string filterexpressionContract = string.Format("BusinessPartnerID = {0} AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0", oAppointmentRequest.BusinessPartnerID);
                                    CustomerContract oCsContract = BusinessLayer.GetCustomerContractList(filterexpressionContract).FirstOrDefault();
                                    if (oCsContract != null)
                                    {

                                        string filtexexpres2 = string.Format(" {0} AND ContractNo = '{1}' ", filterexpressionContract, oCsContract.ContractNo);
                                        vCustomerContract oVcsContract = BusinessLayer.GetvCustomerContractList(filtexexpres2).FirstOrDefault();
                                        if (oVcsContract != null)
                                        {
                                            oAppointmentRequest.ContractID = oVcsContract.ContractID;
                                        }
                                    }
                                    else
                                    {
                                        oAppointmentRequest.ContractID = null;
                                    }

                                    oAppointmentRequest.IsControlClassCare = oReg.IsControlClassCare;
                                    oAppointmentRequest.IsCoverageLimitPerDay = oReg.IsCoverageLimitPerDay;
                                    oAppointmentRequest.GCTariffScheme = oReg.GCTariffScheme;
                                    oAppointmentRequest.CoverageLimitAmount = oReg.CoverageLimitAmount;
                                    oAppointmentRequest.CoverageTypeID = oReg.CoverageTypeID;
                                    oAppointmentRequest.EmployeeID = oReg.EmployeeID;
                                }
                                #endregion


                                appointmentID = oAppointmentRequestDao.InsertReturnPrimaryKeyID(oAppointmentRequest);

                                entity.AppointmentRequestID = appointmentID;
                            }
                        }

                        oReferralDao.Update(entity);
                  
                    }
                    else {
                        errMessage = string.Format("Maaf, untuk pasien dengan registrasi nomor {0} sudah memiliki permintaan perjanjian dengan dokter, poli dan tanggal yang sama", oReg.RegistrationNo);
                        ctx.RollBackTransaction();
                        //ctx.Close();
                        return false;
                    }

                    #endregion
                }
                else
                {
                    int visitID;
                    ConsultVisitDao entityVisitDao = new ConsultVisitDao(ctx);
                    SettingParameterDao entitySettingParameterDao = new SettingParameterDao(ctx);

                    int session = 0;
                    session = BusinessLayer.GetRegistrationSession(Convert.ToInt32(hdnHealthcareServiceUnitID.Value), Convert.ToInt32(hdnPhysicianID.Value), DateTime.Today.Date, DateTime.Now.ToString("HH:mm"));

                    if (session > 0)
                    {
                        #region Add Consult Visit to Current Episode
                        ConsultVisit entityVisit = new ConsultVisit();
                        entityVisit.HealthcareServiceUnitID = Convert.ToInt32(hdnHealthcareServiceUnitID.Value);
                        entityVisit.ParamedicID = Convert.ToInt32(hdnPhysicianID.Value);
                        entityVisit.SpecialtyID = hdnSpecialtyID.Value;
                        entityVisit.VisitTypeID = Convert.ToInt32(cboFollowupVisitType.Value);
                        entityVisit.GCVisitReason = Constant.VisitReason.OTHER;
                        entityVisit.ClassID = entityVisit.ChargeClassID = AppSession.RegisteredPatient.ChargeClassID;
                        entityVisit.RoomID = null;
                        entityVisit.BedID = null;
                        entityVisit.RegistrationID = AppSession.RegisteredPatient.RegistrationID;
                        entityVisit.VisitDate = entityVisit.ActualVisitDate = DateTime.Today;
                        entityVisit.VisitTime = entityVisit.ActualVisitTime = DateTime.Now.ToString("HH:mm");

                        string registrationStatus = "";
                        if (entitySettingParameterDao.Get(Constant.SettingParameter.IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN).ParameterValue == "1")
                            registrationStatus = Constant.VisitStatus.CHECKED_IN;
                        else
                            registrationStatus = Constant.VisitStatus.OPEN;

                        entityVisit.GCVisitStatus = registrationStatus;
                        entityVisit.CreatedBy = AppSession.UserLogin.UserID;
                        entityVisit.IsMainVisit = false;
                        entityVisit.MainVisitID = entity.VisitID;
                        entityVisit.IsReferralVisit = true;
                        entityVisit.Session = BusinessLayer.GetRegistrationSession(entityVisit.HealthcareServiceUnitID, Convert.ToInt32(entityVisit.ParamedicID), entityVisit.VisitDate, entityVisit.VisitTime);
                        entityVisit.QueueNo = BusinessLayer.GetConsultVisitMaxQueueNo(string.Format("HealthcareServiceUnitID = {0} AND ParamedicID = {1} AND VisitDate = '{2}'", entityVisit.HealthcareServiceUnitID, entityVisit.ParamedicID, entityVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)), ctx);
                        entityVisit.QueueNo++;
                        visitID = entityVisitDao.InsertReturnPrimaryKeyID(entityVisit);

                        if (registrationStatus == Constant.VisitStatus.CHECKED_IN)
                        {
                            entityVisit.VisitID = visitID;
                            Helper.InsertAutoBillItem(ctx, entityVisit, Constant.Facility.OUTPATIENT, Convert.ToInt32(entityVisit.ChargeClassID), AppSession.RegisteredPatient.GCCustomerType, false, 0);
                        }
                        #endregion

                        entity.ToVisitID = visitID;
                        oReferralDao.Update(entity);
                    }
                    else
                    {
                        errMessage = "Dokter Tujuan tidak memiliki sessi/jadwal untuk kunjungan saat ini.";
                        ctx.RollBackTransaction();
                        //ctx.Close();
                        return false;
                    }
                }

                ctx.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
                return false;
            }
            finally
            {
                ctx.Close();
            }
        }

        private void ControlToEntity(PatientReferral entity)
        {
            entity.GCRefferalType = rblReferralType.SelectedValue;
            entity.ToPhysicianID = Convert.ToInt32(hdnPhysicianID.Value);
        }

        private void SetControlProperties()
        {
            SetControlEntrySetting(txtAppointmentDate, new ControlEntrySetting(true, true, true, AppSession.RegisteredPatient.VisitDate.Date.AddDays(7).ToString(Constant.FormatString.DATE_PICKER_FORMAT)));
            SetControlEntrySetting(txtAppointmentTime, new ControlEntrySetting(true, true, true, "00:00"));

            PopulateVisitTypeList(hdnHealthcareServiceUnitID.Value);
        }

        private void EntityToControl(vPatientReferral obj)
        {
            txtReferralDate.Text = obj.ReferralDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtReferralTime.Text = obj.ReferralTime;
            hdnFromPhysicianID.Value = obj.FromPhysicianID.ToString();
            txtFromPhysicianCode.Text = obj.FromPhysicianCode;
            txtFromPhysicianName.Text = obj.FromPhysicianName;

            rblReferralType.SelectedValue = obj.GCRefferalType;
            hdnHealthcareServiceUnitID.Value = obj.ToHealthcareServiceUnitID.ToString();
            txtServiceUnitCode.Text = obj.ToServiceUnitCode;
            txtServiceUnitName.Text = obj.ToServiceUnitName;
            hdnPhysicianID.Value = obj.ToPhysicianID != null ? obj.ToPhysicianID.ToString() : "0";
            txtPhysicianCode.Text = obj.ToPhysicianID != null ? obj.ToPhysicianCode : "";
            txtPhysicianName.Text = obj.ToPhysicianID != null ? obj.ToPhysicianName : "";
            hdnSpecialtyID.Value = obj.ToSpecialtyID != null ? obj.ToSpecialtyID.ToString() : "0";

            if (obj.ToAppointmentID == 0)
            {
                if (obj.ScheduleDate.Year != 1900)
                {
                    txtAppointmentDate.Text = obj.ScheduleDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtAppointmentTime.Text = "00:00";
                }
                else
                {
                    txtAppointmentDate.Text = AppSession.RegisteredPatient.VisitDate.Date.AddDays(7).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                    txtAppointmentTime.Text = "00:00";
                }
            }
            else
            {
                //TODO Set Appointment-based information
            }
        }

        private void PopulateVisitTypeList(string healthcareServiceUnitID)
        {
            List<vServiceUnitVisitType> visitTypeList = BusinessLayer.GetvServiceUnitVisitTypeList(string.Format("HealthcareServiceUnitID = {0}", healthcareServiceUnitID));
            Methods.SetComboBoxField(cboFollowupVisitType, visitTypeList, "VisitTypeName", "VisitTypeID");
            if (visitTypeList.Count > 0)
                cboFollowupVisitType.SelectedIndex = 0;
        }
    }
}