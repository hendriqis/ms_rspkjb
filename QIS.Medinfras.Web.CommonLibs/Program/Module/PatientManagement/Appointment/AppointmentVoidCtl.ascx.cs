using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentVoidCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnID.Value = queryString;
            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", queryString))[0];
            SetControlProperties();
            EntityToControl(entity);
        }

        protected void SetControlProperties()
        {
//            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON);
            string filterExpression = string.Format("StandardCodeID = '{0}' AND IsDeleted = 0", Constant.AppointmentDeleteReason.OTHER);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDeleteReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDeleteReason.SelectedIndex = 0;

            txtOtherDeleteReason.Text = "";
            GetSettingParameter();
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}','{5}','{6}')", 
                                                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                                                        Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, //1
                                                                                                        Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, //2
                                                                                                        Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE, //3
                                                                                                        Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE, //4
                                                                                                        Constant.SettingParameter.SA0138, //5
                                                                                                        Constant.SettingParameter.SA_BRIDGING_SISTEM_ANTRIAN //6
                                                                                                    ));
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
            hdnIsUsingMultiVisitSchedule.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.MD_IS_USING_MULTIVISIT_SCHEDULE).FirstOrDefault().ParameterValue;
            hdnIsBridgingToMobileJKN.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.SA0138).FirstOrDefault().ParameterValue;
            hdnIsBridgingToSistemAntrian.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.SA_BRIDGING_SISTEM_ANTRIAN).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vAppointment entity)
        {
            txtAppointmentNo.Text = entity.AppointmentNo;
            txtPatientName.Text = entity.cfPatientName;
            txtServiceUnit.Text = entity.ServiceUnitName;
            txtPhysician.Text = entity.ParamedicName;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            try
            {
                Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(hdnID.Value));                
                entity.GCDeleteReason = cboDeleteReason.Value.ToString();
                if (entity.GCDeleteReason == Constant.AppointmentDeleteReason.OTHER)
                    entity.DeleteReason = txtOtherDeleteReason.Text;
                entity.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                BusinessLayer.UpdateAppointment(entity);

                AppointmentRequest entityApmReq = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentID = {0}", entity.AppointmentID)).FirstOrDefault();
                if (entityApmReq != null)
                {
                    entityApmReq.IsDeleted = true;
                    entityApmReq.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityApmReq.LastUpdatedDate = DateTime.Now;
                    BusinessLayer.UpdateAppointmentRequest(entityApmReq);
                }

                if (hdnIsUsingMultiVisitSchedule.Value == "1")
                {
                    List<DiagnosticVisitSchedule> lstSchedule = BusinessLayer.GetDiagnosticVisitScheduleList(string.Format("AppointmentID = {0} AND IsDeleted = 0", hdnID.Value));
                    IDbContext ctx = DbFactory.Configure(true);
                    DiagnosticVisitScheduleDao scheduleDao = new DiagnosticVisitScheduleDao(ctx);

                    try
                    {
                        foreach (DiagnosticVisitSchedule sch in lstSchedule)
                        {
                            sch.AppointmentID = null;
                            sch.ScheduledDate = new DateTime(1900, 1, 1);
                            sch.GCDiagnosticScheduleStatus = Constant.DiagnosticVisitScheduleStatus.OPEN;
                            sch.LastUpdatedDate = DateTime.Now;
                            sch.LastUpdatedBy = AppSession.UserLogin.UserID;

                            scheduleDao.Update(sch);
                        }

                        ctx.CommitTransaction();
                    }
                    catch (Exception ex)
                    {
                        ctx.RollBackTransaction();
                        Helper.InsertErrorLog(ex);
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }

                BridgingToMedinfrasMobileApps(entity.AppointmentID);

                //#region Is Brigding To Gateway
                if (hdnIsBridgingToGateway.Value == "1")
                {
                    if (hdnProviderGatewayService.Value == Constant.HealthcareGatewayProvider.RSDOSOBA)
                    {
                        string queue = string.Empty;
                        GatewayService oService = new GatewayService();
                        APIMessageLog entityAPILog = new APIMessageLog();
                        entityAPILog.Sender = "MEDINFRAS";
                        entityAPILog.Recipient = "QUEUE ENGINE";
                        vAppointment entityApm = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID)).FirstOrDefault();
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
                    }
                    else
                    {
                        List<vAppointment> lstAppoFrom = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entity.AppointmentID));
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
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                    }
                }

                if (hdnIsBridgingToMobileJKN.Value == "1")
                {
                    APIMessageLog entityAPILog = new APIMessageLog()
                    {
                        MessageDateTime = DateTime.Now,
                        Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                        Sender = Constant.BridgingVendor.HIS,
                        IsSuccess = true
                    };
                    try
                    {
                        BPJSService oService = new BPJSService();

                        string param = string.Format("{0}|{1}", entity.AppointmentNo, !string.IsNullOrEmpty(txtOtherDeleteReason.Text) ? txtOtherDeleteReason.Text : cboDeleteReason.Text);
                        string service = oService.OnSendNotificationToJKN("BATAL_ANTRIAN", param);
                        string[] resultInfo = service.Split('|');
                        if (resultInfo[0] == "1")
                        {
                            entityAPILog.MessageText = resultInfo[1];
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.Response = resultInfo[2];
                        }

                    }
                    catch (Exception ex)
                    {
                        entityAPILog.IsSuccess = false;
                        entityAPILog.Response = ex.Message;
                    }

                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }

                if (hdnIsBridgingToSistemAntrian.Value == "1")
                {
                    //If Bridging to Queue / Center-back Notification - Send Information
                    try
                    {
                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                            Sender = Constant.BridgingVendor.HIS,
                            IsSuccess = true
                        };
                        QueueService oService = new QueueService();

                        string apiResult = oService.ADT_A05_Cancel(AppSession.UserLogin.HealthcareID, entity);
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
                    catch (Exception ex)
                    {
                        result = false;
                        Helper.InsertErrorLog(ex);
                    }
                }

                //#endregion
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
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