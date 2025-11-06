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
    public partial class PostIHSEncounterCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            ddlNotificationType.SelectedValue = paramInfo[0];
            hdnMessageType.Value = paramInfo[0];
            hdnVisitID.Value = paramInfo[1];
            hdnTransactionID.Value = paramInfo[2];

            string filterExpression = "1=0";
            switch (hdnMessageType.Value)
            {
                case "01": //Kunjungan : Post Encounter
                    filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
                    vRegistration entity1 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
                    EntityToControl(entity1);
                    break;
                case "02": //Selesai Kunjungan (Put Encounter with Condition)
                    filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
                    vRegistration entity2 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
                    EntityToControl(entity2);
                    break;
                case "03": //Payment
                    //filterExpression = string.Format("PaymentID = {0}", hdnTransactionID.Value);
                    //vPatientPaymentHd entity4 = BusinessLayer.GetvPatientPaymentHdList(filterExpression).FirstOrDefault();
                    //EntityToControl(entity4);

                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                    vRegistration entity4 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();            
                    EntityToControl(entity4);
                    break;
                case "04": //Encounter Bundling : Condition
                    filterExpression = string.Format("VisitID = {0}", hdnVisitID.Value);
                    vRegistration entity5 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
                    EntityToControl(entity5);
                    break;
                case "05": //Payment
                    //filterExpression = string.Format("PaymentID = {0}", hdnTransactionID.Value);
                    //vPatientPaymentHd entity4 = BusinessLayer.GetvPatientPaymentHdList(filterExpression).FirstOrDefault();
                    //EntityToControl(entity4);

                    filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
                    vRegistration entity6 = BusinessLayer.GetvRegistrationList(filterExpression).FirstOrDefault();
                    EntityToControl(entity6);
                    break;
                default:
                    break;
            }

        }

        private void EntityToControl(vRegistration entity)
        {
            if (entity != null)
            {
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtTransactionNo.Text = entity.RegistrationNo;
                txtTransactionDate.Text = entity.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientIHSNumber.Text = entity.IHSNumber;
                txtPatientName.Text = entity.PatientName;

                if (!string.IsNullOrEmpty(entity.PelepasanInformasiSatuSEHAT))
                {
                    rblSATUSEHAT.SelectedValue = entity.PelepasanInformasiSatuSEHAT;
                    txtSatuSEHATConsentDate.Text = entity.cfLastSatuSEHATConsentDate;
                }

                RegistrationInfo oRegInfo = BusinessLayer.GetRegistrationInfo(entity.RegistrationID);
                hdnEncounterID.Value = oRegInfo.ExternalRegistrationNo;
                #region Parameter Integrasi
                StringBuilder detailInfo = new StringBuilder();
                detailInfo.AppendLine(string.Format("Encounter ID : {0}", oRegInfo.ExternalRegistrationNo));
                detailInfo.AppendLine(string.Format("Practicioner : {0} (IHS : {1})", entity.ParamedicName, entity.ParamedicIHSReferenceID));
                detailInfo.AppendLine(string.Format("Location     : {0} (IHS : {1})", entity.ServiceUnitName, entity.ServiceUnitIHSLocationID));
                txtNotificationDetail.Text = detailInfo.ToString(); 
                #endregion
            }
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtRegistrationNo.Text = entity.RegistrationNo;
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnTestOrderID.Value = entity.TestOrderID.ToString();
            }
        }

        private void EntityToControl(vPatientBill entity)
        {
            if (entity != null)
            {
                if (AppSession.RegisteredPatient != null)
                {
                    txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo; 
                }
                txtTransactionNo.Text = entity.PatientBillingNo;
                txtTransactionDate.Text = entity.BillingDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT) ;
                txtMedicalNo.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
            }
        }

        private void EntityToControl(vPatientPaymentHd entity)
        {
            if (entity != null)
            {
                if (AppSession.RegisteredPatient != null)
                {
                    txtRegistrationNo.Text = AppSession.RegisteredPatient.RegistrationNo;
                }
                txtTransactionNo.Text = entity.PaymentNo;
                txtTransactionDate.Text = entity.PaymentDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                txtMedicalNo.Text = AppSession.RegisteredPatient.MedicalNo;
                txtPatientName.Text = AppSession.RegisteredPatient.PatientName;
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
                int registrationID = Convert.ToInt32(hdnRegistrationID.Value);
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendInformationToSatuSEHAT(messageType,registrationID, transactionID);
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
        public string SendNotification(string messageType, int registrationID, int transactionID)
        {
            string result = "";
            try
            {
                //VisitInfo visitInfo = new VisitInfo();
                //visitInfo = ConvertVisitToDTO(entityVisit);
                //PatientData patientInfo = ConvertPatientToDTO(entityPatient);
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.SATUSEHAT,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}",Convert.ToInt32(hdnRegistrationID.Value))).FirstOrDefault();
                IHSService oService = new IHSService();
                string apiResult = string.Empty;
                string[] apiResultInfo = apiResult.Split('|');

                //Temporary not using Center-back API
                switch (messageType)
                {
                    case "01":
                        if (string.IsNullOrEmpty(hdnEncounterID.Value))
                        {
                            apiResult = oService.PostEncounter(AppSession.UserLogin.HealthcareID, entity, hdnMessageType.Value, Convert.ToInt32(hdnTransactionID.Value));
                            apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[1];
                                entityAPILog.Response = apiResultInfo[1];
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);

                                result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                            }
                            else
                            {
                                entityAPILog.MessageText = apiResultInfo[1];

                                result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", "Data Kunjungan sudah dikirim, lanjutkan dengan proses pengiriman berikutnya : Selesai Kunjungan (Put Encounter)");
                        }
                        break;
                    case "04":
                        if (string.IsNullOrEmpty(hdnEncounterID.Value))
                        {
                            apiResult = oService.PostEncounter(AppSession.UserLogin.HealthcareID, entity, hdnMessageType.Value, Convert.ToInt32(hdnTransactionID.Value));
                        }
                        else
                        {
                            apiResult = oService.PutEncounter(AppSession.UserLogin.HealthcareID, entity, hdnMessageType.Value, hdnEncounterID.Value);
                        }
                        apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);

                            result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[1];

                            result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                        }
                        break;
                    case "05":
                        apiResult = oService.PutEncounter(AppSession.UserLogin.HealthcareID, entity, hdnMessageType.Value, hdnEncounterID.Value);
                        apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);

                            result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[1];
                            result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                        }
                        break;
                    default:
                        break;
                }

                BusinessLayer.InsertAPIMessageLog(entityAPILog);
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Message);
                Helper.InsertErrorLog(ex);
            }

            return result;
        }
        #endregion

        #region IHS Services
        public string SendInformationToSatuSEHAT(string messageType, int registrationID, int transactionID)
        {
            string result = "";
            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.SATUSEHAT,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                IHSService oService = new IHSService();
                string apiResult = string.Empty;
                string[] apiResultInfo = apiResult.Split('|');

                //Temporary not using Center-back API
                switch (messageType)
                {
                    case "01":
                        if (string.IsNullOrEmpty(hdnEncounterID.Value))
                        {
                            apiResult = oService.PostEncounter1(hdnRegistrationID.Value, Request.Form[txtRegistrationNo.UniqueID], hdnVisitID.Value);
                            apiResultInfo = apiResult.Split('|');
                            if (apiResultInfo[0] == "0")
                            {
                                entityAPILog.IsSuccess = false;
                                entityAPILog.MessageText = apiResultInfo[1];
                                entityAPILog.Response = apiResultInfo[1];
                                Exception ex = new Exception(apiResultInfo[1]);
                                Helper.InsertErrorLog(ex);

                                result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                            }
                            else
                            {
                                entityAPILog.MessageText = apiResultInfo[1];

                                result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", "Data Kunjungan sudah dikirim, lanjutkan dengan proses pengiriman berikutnya : Selesai Kunjungan (Put Encounter)");
                        }
                        break;
                    case "04":
                        if (string.IsNullOrEmpty(hdnEncounterID.Value))
                        {
                            apiResult = oService.EncounterBundling1(hdnRegistrationID.Value, Request.Form[txtRegistrationNo.UniqueID], hdnVisitID.Value);
                        }
                        else
                        {
                            //Temporary not implemented PUT Encounter
                            apiResult = oService.EncounterBundling1(hdnRegistrationID.Value, Request.Form[txtRegistrationNo.UniqueID], hdnVisitID.Value);
                        }
                        apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[2]);
                            Helper.InsertErrorLog(ex);

                            result = string.Format("{0}|{1}", "0", apiResultInfo[2]);
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[1];

                            result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                        }
                        break;
                    case "05":
                        vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", Convert.ToInt32(hdnRegistrationID.Value))).FirstOrDefault();
                        apiResult = oService.PutEncounter(AppSession.UserLogin.HealthcareID, entity, hdnMessageType.Value, hdnEncounterID.Value);
                        apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);
                            Helper.InsertErrorLog(ex);

                            result = string.Format("{0}|{1}", "0", apiResultInfo[1]);
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[1];
                            result = string.Format("{0}|{1}", "1", apiResultInfo[1]);
                        }
                        break;
                    default:
                        break;
                }

                BusinessLayer.InsertAPIMessageLog(entityAPILog);
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Message);
                Helper.InsertErrorLog(ex);
            }

            return result;
        }
        #endregion
    }
}