using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using QIS.Medinfras.Web.Common.API.Model;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Text;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendJKNNotificationCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            ddlNotificationType.SelectedValue = paramInfo[0];
            hdnMessageType.Value = paramInfo[0];

            string filterExpression = "1=0";
            switch (hdnMessageType.Value)
            {
                case "00": //Jadwal Dokter
                    string[] refInfo = paramInfo[1].Split(';');
                    hdnParamedicID.Value = refInfo[0];
                    hdnHealthcareServiceUnitID.Value = refInfo[1];
                    SetMessageDetail00(hdnParamedicID.Value, hdnHealthcareServiceUnitID.Value);
                    break;
                case "01": //Tambah Antrian Appointment
                    hdnAppointmentID.Value = paramInfo[1];
                    SetMessageDetail01(hdnAppointmentID.Value);
                    break;
                case "02": //Billing
                    hdnAppointmentID.Value = paramInfo[1];
                    hdnRegistrationID.Value = paramInfo[2];
                    SetMessageDetail02(hdnAppointmentID.Value, hdnRegistrationID.Value);
                    break;
                case "03": //Catatan Terintegrasi
                    hdnRegistrationID.Value = paramInfo[1];
                    hdnTaskID.Value = paramInfo[2];
                    SetMessageDetail03(hdnRegistrationID.Value, hdnTaskID.Value);
                    break;
                default:
                    break;
            }

        }

        private void SetMessageDetail00(string paramedicID, string healthcareServiceUnitID)
        {
            string result = "";

            vParamedicMaster obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", hdnParamedicID.Value)).FirstOrDefault();
            vHealthcareServiceUnit obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", hdnHealthcareServiceUnitID.Value)).FirstOrDefault();

            if (obj1 != null)
            {
                StringBuilder messageDetail = new StringBuilder();

                string[] referenceInfo = obj1.BPJSReferenceInfo.Split(';');
                string[] paramedicInfo = referenceInfo[1].Split('|');

                messageDetail.AppendLine(string.Format("Kode Dokter RS   : {0}", obj1.ParamedicCode));
                messageDetail.AppendLine(string.Format("Kode Dokter HFIS : {0}", paramedicInfo[0]));
                messageDetail.AppendLine(string.Format("Nama Dokter HFIS : {0}", paramedicInfo[1]));

                if (obj2 != null)
                {
                    string[] bpjsInfo = obj2.BPJSPoli.Split('|');
                    messageDetail.AppendLine(string.Format("Kode Unit   : {0}", bpjsInfo[0]));
                    messageDetail.AppendLine(string.Format("Nama Unit   : {0}", bpjsInfo[1]));
                }

                txtNotificationDetail.Text = messageDetail.ToString();
            }

            
        }
        private void SetMessageDetail01(string appointmentID)
        {
            string result = "";

            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", appointmentID)).FirstOrDefault();
            vParamedicMaster obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();
            vHealthcareServiceUnit obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();

            if (obj1 != null)
            {
                StringBuilder messageDetail = new StringBuilder();

                string[] referenceInfo = obj1.BPJSReferenceInfo.Split(';');
                string[] paramedicInfo = referenceInfo[1].Split('|');


                messageDetail.AppendLine(string.Format("Nomor Perjanjian    : {0}", entity.AppointmentNo));
                messageDetail.AppendLine(string.Format("Kode Dokter RS      : {0}", obj1.ParamedicCode));
                messageDetail.AppendLine(string.Format("Kode Dokter HFIS    : {0}", paramedicInfo[0]));
                messageDetail.AppendLine(string.Format("Nama Dokter HFIS    : {0}", paramedicInfo[1]));

                if (obj2 != null)
                {
                    string[] bpjsInfo = obj2.BPJSPoli.Split('|');
                    messageDetail.AppendLine(string.Format("Kode Unit       : {0}", bpjsInfo[0]));
                    messageDetail.AppendLine(string.Format("Nama Unit       : {0}", bpjsInfo[1]));
                }

                txtNotificationDetail.Text = messageDetail.ToString();
            }


        }
        private void SetMessageDetail02(string appointmentID, string registrationID)
        {
            string result = "";
            StringBuilder messageDetail = new StringBuilder();
            StringBuilder errMessage = new StringBuilder();
            string appointmentNo = "";
            string registrationNo = "";
            string paramedicCode = "";
            string ParamedicHfisCode = "";
            string paramedicHfisName = "";
            string serviceUnitBpjsCode = "";
            string serviceUnitBpjsName = "";
            string referralNo = "";
            bool isBpjs = false;

            vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", appointmentID)).FirstOrDefault();
            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();
            vParamedicMaster obj1 = null;
            vHealthcareServiceUnit obj2 = null;
            if (entity != null)
            {
                obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();
                obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();
            }
            else
            {
                obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", entityReg.ParamedicID)).FirstOrDefault();
                obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entityReg.HealthcareServiceUnitID)).FirstOrDefault();
            }

            if (entity == null && entityReg == null)
            {
                errMessage.AppendLine("Tidak ada nomor perjanjian maupun nomor registrasi");
            }

            if (entityReg.GCCustomerType == Constant.CustomerType.BPJS)
            {
                isBpjs = true;
                RegistrationBPJS entityBpjs = BusinessLayer.GetRegistrationBPJS(entityReg.RegistrationID);
                //if (entityBpjs != null)
                //{
                //    if (!string.IsNullOrEmpty(entityBpjs.NoRujukan))
                //    {
                //        referralNo = entityBpjs.NoRujukan;
                //    }
                //    else
                //    {
                //        errMessage.AppendLine("Tidak ada nomor referensi untuk registrasi ini.");
                //    }
                //}
            }

            appointmentNo = entity != null ? entity.AppointmentNo : "";
            registrationNo = entityReg != null ? entityReg.RegistrationNo : "";
            paramedicCode = obj1 != null ? obj1.ParamedicCode : "";
            string paramedicHfis = obj1 != null ? obj1.BPJSReferenceInfo : "";
            string[] referenceInfo = new string[0];
            string[] paramedicInfo = new string[0];
            if (!string.IsNullOrEmpty(paramedicHfis))
            {
                ParamedicHfisCode = obj1.BPJSReferenceInfo.Split(';')[1].Split('|')[0];
                paramedicHfisName = obj1.BPJSReferenceInfo.Split(';')[1].Split('|')[1];
            }
            else
            {
                errMessage.AppendLine("Dokter belum termapping HFIS");
            }
            string serviceUnitBpjs = obj2 != null ? obj2.BPJSPoli : "";
            if (!string.IsNullOrEmpty(serviceUnitBpjs))
            {
                string[] bpjsInfo = obj2.BPJSPoli.Split('|');
                serviceUnitBpjsCode = bpjsInfo[0];
                serviceUnitBpjsName = bpjsInfo[1];
            }
            else
            {
                errMessage.AppendLine("Poli belum termapping kode BPJS");
            }

            messageDetail.AppendLine(string.Format("Nomor Perjanjian    : {0}", appointmentNo));
            messageDetail.AppendLine(string.Format("Nomor Registrasi    : {0}", registrationNo));
            messageDetail.AppendLine(string.Format("Nomor Referensi     : {0}", referralNo));
            messageDetail.AppendLine(string.Format("Kode Dokter RS      : {0}", paramedicCode));
            messageDetail.AppendLine(string.Format("Kode Dokter HFIS    : {0}", ParamedicHfisCode));
            messageDetail.AppendLine(string.Format("Nama Dokter HFIS    : {0}", paramedicHfisName));
            messageDetail.AppendLine(string.Format("Kode Unit           : {0}", serviceUnitBpjsCode));
            messageDetail.AppendLine(string.Format("Nama Unit           : {0}", serviceUnitBpjsName));

            txtNotificationDetail.Text = messageDetail.ToString();

            if (!string.IsNullOrEmpty(errMessage.ToString()))
            {
                hdnIsError.Value = "1";
                hdnErrMessage.Value = errMessage.ToString();
            }
        }
        private void SetMessageDetail03(string registrationID, string taskID)
        {
            string result = "";

            vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", registrationID)).FirstOrDefault();
            if (entityReg.AppointmentID > 0)
            {
                vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entityReg.AppointmentID)).FirstOrDefault();
                vParamedicMaster obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", entity.ParamedicID)).FirstOrDefault();
                vHealthcareServiceUnit obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", entity.HealthcareServiceUnitID)).FirstOrDefault();

                if (obj1 != null)
                {
                    StringBuilder messageDetail = new StringBuilder();

                    string[] referenceInfo = obj1.BPJSReferenceInfo.Split(';');
                    string[] paramedicInfo = referenceInfo[1].Split('|');


                    messageDetail.AppendLine(string.Format("Nomor Perjanjian    : {0}", entity.AppointmentNo));
                    messageDetail.AppendLine(string.Format("Nomor Registrasi    : {0}", entityReg.RegistrationNo));
                    messageDetail.AppendLine(string.Format("Kode Dokter RS      : {0}", obj1.ParamedicCode));
                    messageDetail.AppendLine(string.Format("Kode Dokter HFIS    : {0}", paramedicInfo[0]));
                    messageDetail.AppendLine(string.Format("Nama Dokter HFIS    : {0}", paramedicInfo[1]));

                    if (obj2 != null)
                    {
                        string[] bpjsInfo = obj2.BPJSPoli.Split('|');
                        messageDetail.AppendLine(string.Format("Kode Unit       : {0}", bpjsInfo[0]));
                        messageDetail.AppendLine(string.Format("Nama Unit       : {0}", bpjsInfo[1]));
                    }

                    txtNotificationDetail.Text = messageDetail.ToString();
                }
            }
        }

        private void EntityToControl(vRegistration entity)
        {
            if (entity != null)
            {
            }
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
            }
        }

        private void EntityToControl(vPatientBill entity)
        {
            if (entity != null)
            {
            }
        }

        private void EntityToControl(vPatientPaymentHd entity)
        {
            if (entity != null)
            {
            }
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                string messageType = hdnMessageType.Value;
                string referenceNo = string.Empty;
                bool isError = false;

                string parameter = string.Empty;
                if (messageType == "00")
                {
                    parameter = string.Format("{0}|{1}", hdnParamedicID.Value.ToString(), hdnHealthcareServiceUnitID.Value.ToString());
                }
                else if (messageType == "01")
                {
                    parameter = string.Format("{0}", hdnAppointmentID.Value);
                }
                else if (messageType == "02")
                {
                    parameter = string.Format("{0}|{1}", hdnAppointmentID.Value, hdnRegistrationID.Value);
                }
                else if (messageType == "03")
                {
                    parameter = string.Format("{0}|{1}", hdnRegistrationID.Value, hdnTaskID.Value);
                }

                var processResult = SendNotification(messageType, parameter);
                string[] resultInfo = processResult.Split('|');

                isError = resultInfo[0] == "0";
                if (isError)
                {
                    errMessage = resultInfo[1];
                    result = false;
                }
                else
                {
                    retval = resultInfo[1];
                }

                return result;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #region Notification Services
        public string SendNotification(string messageType, string parameter)
        {
            string result = "";
            if (hdnIsError.Value == "0")
            {
                try
                {
                    if (hdnMessageType.Value == "00")
                    {
                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                            Sender = Constant.BridgingVendor.HIS,
                            IsSuccess = true
                        };

                        BPJSService oService = new BPJSService();

                        string[] paramInfo = parameter.Split('|');
                        vParamedicMaster obj1 = BusinessLayer.GetvParamedicMasterList(string.Format("ParamedicID = {0}", paramInfo[0])).FirstOrDefault();
                        vHealthcareServiceUnit obj2 = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", paramInfo[1])).FirstOrDefault();
                        string param = string.Format("{0}|{1}", obj1.ParamedicCode, obj2.ServiceUnitCode);
                        result = oService.OnSendNotificationToJKN("UPDATE_JADWAL_DOKTER", param);

                        string[] resultInfo = result.Split('|');
                        if (resultInfo[0] == "1")
                        {
                            entityAPILog.MessageText = resultInfo[1];

                            result = string.Format("{0}|{1}", "1", txtNotificationDetail.Text);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = resultInfo[2];
                            entityAPILog.Response = resultInfo[2];
                            Exception ex = new Exception(resultInfo[2]);
                            Helper.InsertErrorLog(ex);

                            result = string.Format("{0}|{1}", "0", resultInfo[2]);
                        }

                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                    }
                    else if (hdnMessageType.Value == "01")
                    {
                        vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", parameter)).FirstOrDefault();
                        //result = oService.OnSendNotificationToJKN("TAMBAH_ANTRIAN", string.Format("{0}|", entity.AppointmentNo));
                        BusinessLayer.OnInsertBPJSTaskLog(Convert.ToInt32(hdnRegistrationID.Value), 3, AppSession.UserLogin.UserID, DateTime.Now);

                        result = string.Format("{0}|{1}", "1", txtNotificationDetail.Text);
                    }
                    else if (hdnMessageType.Value == "02")
                    {
                        string[] paramInfo = parameter.Split('|');
                        vAppointment entity = null;
                        if (!string.IsNullOrEmpty(paramInfo[0]))
                        {
                            entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", paramInfo[0])).FirstOrDefault();
                        }
                        vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", paramInfo[1])).FirstOrDefault();
                        //result = oService.OnSendNotificationToJKN("TAMBAH_ANTRIAN", string.Format("{0}|{1}", entity != null ? entity.AppointmentNo : string.Empty, entityReg.RegistrationNo));
                        BusinessLayer.OnInsertBPJSTaskLog(Convert.ToInt32(hdnRegistrationID.Value), 3, AppSession.UserLogin.UserID, DateTime.Now);

                        result = string.Format("{0}|{1}", "1", txtNotificationDetail.Text);
                    }
                    else if (hdnMessageType.Value == "03")
                    {
                        string[] paramInfo = parameter.Split('|');
                        vRegistration entityReg = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", paramInfo[0])).FirstOrDefault();
                        vAppointment entity = BusinessLayer.GetvAppointmentList(string.Format("AppointmentID = {0}", entityReg.AppointmentID)).FirstOrDefault();
                        //result = oService.OnSendNotificationToJKN("UPDATE_WAKTU", string.Format("{0}|{1}", entity.AppointmentNo, hdnTaskID.Value));
                        BusinessLayer.OnInsertBPJSTaskLog(Convert.ToInt32(hdnRegistrationID.Value), Convert.ToInt32(hdnTaskID.Value), AppSession.UserLogin.UserID, DateTime.Now);

                        result = string.Format("{0}|{1}", "1", txtNotificationDetail.Text);
                    }

                }
                catch (Exception ex)
                {
                    result = string.Format("{0}|{1}", "0", ex.Message);
                    Helper.InsertErrorLog(ex);
                }
            }
            else
            {
                result = string.Format("{0}|{1}", "0", hdnErrMessage.Value);
            }

            return result;
        }
        #endregion
    }
}