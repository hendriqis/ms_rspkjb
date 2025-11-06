using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Data;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentVoidByParamedicCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            IsAdd = true;
            string[] paramLst = param.Split('|');
            hdnParamedicID.Value = paramLst[0];
            hdnSelectedDate.Value = paramLst[1];
            hdnDepartmentID.Value = paramLst[2];
            ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicID = {0}", hdnParamedicID.Value)).FirstOrDefault();
            txtPhysician.Text = pm.FullName;
            txtDate.Text = Helper.GetDatePickerValue(hdnSelectedDate.Value).ToString(Constant.FormatString.DATE_FORMAT);
            SetControlProperties();
            //EntityToControl(entity);
        }

        protected void SetControlProperties()
        {
            GetSettingParameter();

            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON_APPOINTMENT);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDeleteReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDeleteReason.SelectedIndex = 0;

            string filterHSU = string.Format("HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM ServiceUnitParamedic WITH(NOLOCK) WHERE ParamedicID = {0}) AND IsDeleted = 0", hdnParamedicID.Value);

            switch (hdnDepartmentID.Value)
            {
                case "OP": filterHSU += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
                case "IS": filterHSU += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = {1}", Constant.Facility.DIAGNOSTIC, hdnDefaultImagingHSUID.Value); break;
                case "LB": filterHSU += string.Format(" AND DepartmentID = '{0}' AND HealthcareServiceUnitID = {1}", Constant.Facility.DIAGNOSTIC, hdnDefaultLaboratoryHSUID.Value); break;
                case "MD": filterHSU += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.DIAGNOSTIC); break;
                case "MC": filterHSU += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.MEDICAL_CHECKUP); break;
                default: filterHSU += string.Format(" AND DepartmentID = '{0}'", Constant.Facility.OUTPATIENT); break;
            }

            List<vHealthcareServiceUnitCustom> lstHSU = BusinessLayer.GetvHealthcareServiceUnitCustomList(filterHSU);

            lstHSU.Insert(0, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = -1, ServiceUnitName = "--SEMUA--" });

            switch (hdnDepartmentID.Value)
            {
                case "OP": lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA RAWAT JALAN--" }); break;
                case "IS": lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA PENUNJANG RADIOLOGI--" }); break;
                case "LB": lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA PENUNJANG LABORATORIUM--" }); break;
                case "MD": lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA PENUNJANG MEDIS--" }); break;
                case "MC": lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA MEDICAL CHECKUP--" }); break;
                default: lstHSU.Insert(1, new vHealthcareServiceUnitCustom { HealthcareServiceUnitID = 0, ServiceUnitName = "--SEMUA RAWAT JALAN--" }); break;
            }

            Methods.SetComboBoxField<vHealthcareServiceUnitCustom>(cboServiceUnitCtl, lstHSU, "ServiceUnitName", "HealthcareServiceUnitID");
            cboServiceUnitCtl.SelectedIndex = 0;

            txtOtherDeleteReason.Text = "";
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI, Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE));
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnDefaultImagingHSUID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI).FirstOrDefault().ParameterValue;
            hdnDefaultLaboratoryHSUID.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM).FirstOrDefault().ParameterValue;
            hdnIsUsingMultiVisitDiagnostic.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            bool isAll = cboServiceUnitCtl.Value.ToString() == "-1" ? true : false;
            bool isAllDepartment = cboServiceUnitCtl.Value.ToString() == "0" ? true : false;
            Int32 ParamedicID = Convert.ToInt32(hdnParamedicID.Value);
            DateTime selectedDate = Helper.GetDatePickerValue(hdnSelectedDate.Value);
            string filterExpression = string.Empty;

            filterExpression = String.Format("GCAppointmentStatus NOT IN ('{0}','{1}') AND ParamedicID = {2} AND StartDate = '{3}'", Constant.AppointmentStatus.DELETED, Constant.AppointmentStatus.CANCELLED, ParamedicID, selectedDate.ToString("yyyy-MM-dd"));
            if (!isAll)
            {
                if (isAllDepartment)
                {
                    switch (hdnDepartmentID.Value)
                    {
                        case "OP": filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}')", Constant.Facility.OUTPATIENT); break;
                        case "IS": filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID = {0})", Constant.Facility.DIAGNOSTIC, hdnDefaultImagingHSUID.Value); break;
                        case "LB": filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID = {0})", Constant.Facility.DIAGNOSTIC, hdnDefaultLaboratoryHSUID.Value); break;
                        case "MD": filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}' AND HealthcareServiceUnitID NOT IN ({1}, {2}))", Constant.Facility.OUTPATIENT, hdnDefaultImagingHSUID.Value, hdnDefaultLaboratoryHSUID.Value); break;
                        case "MC": filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}')", Constant.Facility.MEDICAL_CHECKUP); break;
                        default: filterExpression += string.Format(" AND HealthcareServiceUnitID IN (SELECT HealthcareServiceUnitID FROM vHealthcareServiceUnitCustom WITH(NOLOCK) WHERE DepartmentID = '{0}')", Constant.Facility.OUTPATIENT); break;
                    }
                }
                else
                {
                    filterExpression += string.Format(" AND HealthcareServiceUnitID = {0}", cboServiceUnitCtl.Value.ToString());
                }
            }
            
            List<Appointment> lstAppo = BusinessLayer.GetAppointmentList(filterExpression);

            if (lstAppo.Where(p => p.GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE).Count() > 0)
            {
                errMessage = "Dokter ini sudah memiliki Perjanjian Pasien yang sudah Registrasi.";
                result = false;
            }
            else
            {
                List<DiagnosticVisitSchedule> lstSch = null;
                if (hdnIsUsingMultiVisitDiagnostic.Value == "1") 
                {
                    string lstApmID = string.Empty;
                    foreach (Appointment e in lstAppo)
                    {
                        lstApmID += e.AppointmentID.ToString() + ",";
                    }

                    lstApmID = lstApmID.Remove(lstApmID.Length - 1, 1);

                    lstSch = new List<DiagnosticVisitSchedule>();
                    lstSch = BusinessLayer.GetDiagnosticVisitScheduleList(string.Format("AppointmentID IN ({0})", lstApmID));
                }

                IDbContext ctx = DbFactory.Configure(true);
                AppointmentDao entityDao = new AppointmentDao(ctx);
                APIMessageLogDao entityLogDao = new APIMessageLogDao(ctx);
                DiagnosticVisitScheduleDao diagVisitScheduleDao = new DiagnosticVisitScheduleDao(ctx);

                try
                {
                    foreach (Appointment e in lstAppo)
                    {
                        e.GCDeleteReason = cboDeleteReason.Value.ToString();
                        if (e.GCDeleteReason == Constant.AppointmentDeleteReason.OTHER)
                            e.DeleteReason = txtOtherDeleteReason.Text;
                        e.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                        e.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityDao.Update(e);

                        if (hdnIsUsingMultiVisitDiagnostic.Value == "1")
                        {
                            if (lstSch != null)
                            {
                                if (lstSch.Count > 0)
                                {
                                    foreach (DiagnosticVisitSchedule d in lstSch)
                                    {
                                        d.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                                        d.AppointmentID = null;
                                        d.ScheduledDate = new DateTime(1900, 1, 1);
                                        d.LastUpdatedBy = AppSession.UserLogin.UserID;
                                        d.LastUpdatedDate = DateTime.Now;

                                        diagVisitScheduleDao.Update(d);
                                    }
                                }
                            }
                        }
                    }

                    ctx.CommitTransaction();
                }
                catch (Exception ex)
                {
                    ctx.RollBackTransaction();
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                }
                finally
                {
                    ctx.Close();
                }

                #region Bridging Notification
                try
                {
                    foreach (Appointment e in lstAppo)
                    {
                        BridgingToMedinfrasMobileApps(e.AppointmentID);

                        if (hdnIsBridgingToGateway.Value == "1")
                        {
                            if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                            {
                                string queue = string.Empty;
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                entityAPILog.Sender = "MEDINFRAS";
                                entityAPILog.Recipient = "QUEUE ENGINE";
                                vAppointment entityApm = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID), ctx).FirstOrDefault();
                                if (entityApm != null)
                                {
                                    string apiResult = oService.GetQueueNoByVoidRegistration(entityApm.MedicalNo, entityApm.StartDate, entityApm.ParamedicCode, entityApm.ServiceUnitCode, entityApm.StartTime, Convert.ToString(entityApm.QueueNo), entityApm.HealthcareServiceUnitID.ToString());
                                    string[] apiResultInfo = apiResult.Split('|');
                                    if (apiResultInfo[0] == "0")
                                    {
                                        queue = string.Format("{0}|{1}", apiResultInfo[0], apiResultInfo[1]);
                                        entityAPILog.MessageDateTime = DateTime.Now;
                                        entityAPILog.IsSuccess = false;
                                        entityAPILog.MessageText = apiResultInfo[2];
                                        entityAPILog.Response = apiResultInfo[1];
                                        entityLogDao.Insert(entityAPILog);
                                        Exception ex = new Exception(apiResultInfo[1]);
                                        Helper.InsertErrorLog(ex);
                                    }
                                    else
                                    {
                                        queue = apiResult;
                                        entityAPILog.MessageDateTime = DateTime.Now;
                                        entityAPILog.MessageText = apiResultInfo[2];
                                        entityAPILog.Response = apiResultInfo[1];
                                        entityLogDao.Insert(entityAPILog);
                                    }
                                }
                            }
                            else
                            {
                                List<vAppointment> lstAppoFrom = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", e.AppointmentID));
                                GatewayService oService = new GatewayService();
                                APIMessageLog entityAPILog = new APIMessageLog();
                                string apiResult = oService.OnChangedAppointmentInformation("VOID", lstAppoFrom);
                                string[] apiResultInfo = apiResult.Split('|');
                                if (apiResultInfo[0] == "0")
                                {
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.MessageText = apiResultInfo[1];
                                    entityAPILog.Response = apiResultInfo[1];
                                    Exception ex = new Exception(apiResultInfo[1]);
                                    Helper.InsertErrorLog(ex);
                                }
                                else
                                {
                                    entityAPILog.MessageText = apiResultInfo[0];
                                    entityLogDao.Insert(entityAPILog);
                                }
                            }
                        }

                        if (AppSession.IsBridgingToQueue)
                        {
                            APIMessageLog entityAPILog = new APIMessageLog()
                            {
                                MessageDateTime = DateTime.Now,
                                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                                Sender = Constant.BridgingVendor.HIS,
                                IsSuccess = true
                            };
                            QueueService oService = new QueueService();

                            string apiResult = oService.ADT_A05_Cancel(AppSession.UserLogin.HealthcareID, e);
                            string[] apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[2];
                                entityAPILog.Response = apiResult;
                                entityAPILog.ErrorMessage = apiResultInfo[1];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);
                            }
                            else
                            {
                                entityAPILog.MessageText = apiResultInfo[2];
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    errMessage = ex.Message;
                    Helper.InsertErrorLog(ex);
                }
                #endregion
            }

            return result;
        }

        private void BridgingToMedinfrasMobileApps(int appointmentID)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.IsSuccess = true;
                entityAPILog.MessageDateTime = DateTime.Now;
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "MOBILE APPS";

                string apiResult = oService.OnVoidAppointment(appointmentID);
                if (!string.IsNullOrEmpty(apiResult))
                {
                    string[] apiResultInfo = apiResult.Split('|');
                    if (apiResultInfo[0] == "0")
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.ErrorMessage = apiResultInfo[2];
                        //entityAPILog.Response = apiResultInfo[1];
                        Exception ex = new Exception(apiResultInfo[2]);
                        Helper.InsertErrorLog(ex);
                    }
                    else
                    {
                        entityAPILog.MessageText = apiResultInfo[1];
                        entityAPILog.Response = apiResultInfo[2];
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                }
            }
        }
    }
}