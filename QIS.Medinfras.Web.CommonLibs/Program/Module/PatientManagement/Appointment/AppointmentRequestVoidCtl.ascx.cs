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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class AppointmentRequestVoidCtl : BaseEntryPopupCtl
    {
        public override void InitializeDataControl(string queryString)
        {
            IsAdd = true;
            hdnID.Value = queryString;
            vAppointmentRequest entity = BusinessLayer.GetvAppointmentRequestList(string.Format("AppointmentRequestID = {0}", queryString))[0];
            SetControlProperties();
            GetSettingParameter();
            EntityToControl(entity);
        }

        private void GetSettingParameter()
        {
            List<SettingParameterDt> setvar = BusinessLayer.GetSettingParameterDtList(String.Format("ParameterCode IN ('{0}')", Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS));
            hdnIsBridgingToMedinfrasMobileApps.Value = setvar.Where(t => t.ParameterCode == Constant.SettingParameter.IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS).FirstOrDefault().ParameterValue;
        }

        protected void SetControlProperties()
        {
            string filterExpression = string.Format("ParentID = '{0}' AND IsDeleted = 0", Constant.StandardCode.DELETE_REASON_APPOINTMENT);
            List<StandardCode> lstSc = BusinessLayer.GetStandardCodeList(filterExpression);
            Methods.SetComboBoxField<StandardCode>(cboDeleteReason, lstSc, "StandardCodeName", "StandardCodeID");
            cboDeleteReason.SelectedIndex = 0;

            txtOtherDeleteReason.Text = "";
        }

        protected override void OnControlEntrySetting()
        {
            SetControlEntrySetting(txtOtherDeleteReason, new ControlEntrySetting(true, true, true));
            SetControlEntrySetting(cboDeleteReason, new ControlEntrySetting(true, true, true));
        }

        private void EntityToControl(vAppointmentRequest entity)
        {
            txtPatientName.Text = entity.PatientName;
            txtServiceUnit.Text = entity.ServiceUnitName;
            txtPhysician.Text = entity.ParamedicName;
        }

        protected override bool OnSaveAddRecord(ref string errMessage, ref string retval)
        {
            try
            {
                AppointmentRequest entity = BusinessLayer.GetAppointmentRequest(Convert.ToInt32(hdnID.Value));
                if (entity.AppointmentID != null && entity.AppointmentID != 0)
                {
                    Appointment app = BusinessLayer.GetAppointment(Convert.ToInt32(entity.AppointmentID));
                    errMessage = string.Format("Maaf, permintaan perjanjian ini sudah di proses dengan no. {0}, Silahkan Refresh halaman ini.", app.AppointmentNo);
                    return false;
                }
                else
                {
                    entity.IsDeleted = true;
                    entity.GCDeleteReason = cboDeleteReason.Value.ToString();
                    if (entity.GCDeleteReason == Constant.AppointmentDeleteReason.OTHER)
                        entity.DeleteReason = txtOtherDeleteReason.Text;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    BusinessLayer.UpdateAppointmentRequest(entity);
                    if (entity.Remarks.Contains("^"))
                    {
                        BridgingToMedinfrasMobileApps(Convert.ToInt32(hdnID.Value));
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                errMessage = ex.Message;
                return false;
            }
        }

        private void BridgingToMedinfrasMobileApps(int appointmentRequestID)
        {
            if (hdnIsBridgingToMedinfrasMobileApps.Value == "1")
            {
                MedinfrasMobileAppsService oService = new MedinfrasMobileAppsService();
                APIMessageLog entityAPILog = new APIMessageLog();
                entityAPILog.IsSuccess = true;
                entityAPILog.MessageDateTime = DateTime.Now;
                entityAPILog.Sender = "MEDINFRAS";
                entityAPILog.Recipient = "MOBILE APPS";

                string apiResult = oService.OnRejectAppointmentRequest(appointmentRequestID);
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