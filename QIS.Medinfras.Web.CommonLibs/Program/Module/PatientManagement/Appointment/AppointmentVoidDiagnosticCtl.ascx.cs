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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentVoidDiagnosticCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnID.Value = queryString; //AppointmentRequestID
            //vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", queryString))[0];
            List<vAppointmentRequest> entity = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentID IN ({0})", queryString));
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
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}', '{1}', '{2}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS, Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY, Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE));
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
            hdnIsBridgingToGateway.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_GATEWAY).FirstOrDefault().ParameterValue;
            hdnProviderGatewayService.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.PROVIDER_GATEWAY_SERVICE).FirstOrDefault().ParameterValue;
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(List<vAppointmentRequest> entity)
        {
            //int count = entity.Count;
            //txtAppointmentNo.Text = count.ToString();
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
            AppointmentRequestDao entityReqDao = new AppointmentRequestDao(ctx);
            AppointmentDao entityDao = new AppointmentDao(ctx);

            try
            {
                List<AppointmentRequest> lstApmReq = BusinessLayer.GetAppointmentRequestList(string.Format("AppointmentRequestID IN ({0})", hdnID.Value));

                foreach (AppointmentRequest a in lstApmReq)
                {
                    if (!result)
                    {
                        break;
                    }

                    if (!chkIsVoidAppointmentRequest.Checked)
                    {
                        a.IsDeleted = true;
                        a.GCDeleteReason = cboDeleteReason.Value.ToString();
                        a.DeleteReason = txtOtherDeleteReason.Text;
                        //BusinessLayer.UpdateAppointmentRequest(a);
                        entityReqDao.Update(a);

                        DeleteAppointment(ctx, entityDao, a, ref errMessage, ref result);
                    }
                    else
                    {
                        DeleteAppointment(ctx, entityDao, a, ref errMessage, ref result);

                        a.AppointmentID = null;
                        a.AppointmentTime = null;
                        //BusinessLayer.UpdateAppointmentRequest(a);
                        entityReqDao.Update(a);
                    }
                }

                if (result)
                {
                    ctx.CommitTransaction();
                }
                else
                {
                    ctx.RollBackTransaction();
                }
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
                errMessage = ex.Message;
                result = false;
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        private void DeleteAppointment(IDbContext ctx, AppointmentDao entityDao, AppointmentRequest a, ref string errMessage, ref bool result)
        {
            if (a.AppointmentID > 0)
            {
                Appointment entity = BusinessLayer.GetAppointment(Convert.ToInt32(a.AppointmentID));
                entity.GCDeleteReason = cboDeleteReason.Value.ToString();
                if (entity.GCDeleteReason == Constant.AppointmentDeleteReason.OTHER)
                    entity.DeleteReason = txtOtherDeleteReason.Text;
                entity.GCAppointmentStatus = Constant.AppointmentStatus.DELETED;
                entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                //BusinessLayer.UpdateAppointment(entity);
                entityDao.Update(entity);
            }
            else
            {
                if (chkIsVoidAppointmentRequest.Checked)
                {
                    errMessage = "Ada data yang tidak memiliki nomor perjanjian";
                    result = false;
                }
            }
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