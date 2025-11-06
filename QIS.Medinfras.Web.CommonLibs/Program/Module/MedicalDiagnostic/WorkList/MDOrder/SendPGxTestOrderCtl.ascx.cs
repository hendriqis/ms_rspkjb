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
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Security.Authentication;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendPGxTestOrderCtl : BaseProcessPopupCtl
    {
        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTestOrderID.Value = paramInfo[0];
            hdnTransactionID.Value = paramInfo[1];
            if (!string.IsNullOrEmpty(hdnTransactionID.Value) || hdnTransactionID.Value != "0")
            {
                txtTransactionNo.Text = paramInfo[2];
                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND IsPGxTest = 1", hdnTransactionID.Value);
            List<vPatientChargesDtLabForSendOrder> lstDetail = BusinessLayer.GetvPatientChargesDtLabForSendOrderList(filterExpression);

            vPatientChargesDtLabForSendOrder obj = lstDetail.FirstOrDefault();
            if (obj != null)
            {
                string[] detailRemarksInfo = obj.DetailRemarks.Split('|');
                txtClinicalNotes.Text = detailRemarksInfo[0];

                if (detailRemarksInfo.Length >= 2)
                {
                    List<string> lstDiseaseRisk = detailRemarksInfo[1].Split(';').ToList();
                    foreach (string risk in lstDiseaseRisk)
                    {
                        chkListRisk.Items.FindByValue(risk).Selected = true;
                    }

                    chkIsHasFamilyMemberWithPGxTest.Checked = detailRemarksInfo[2] == "True";
                    chkIsInterestedInADR.Checked = detailRemarksInfo[3] == "True";
                }
            }

            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            BindGridView();
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
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
                int testOrderID = hdnTestOrderID.Value != "" && hdnTestOrderID.Value !=  null ? Convert.ToInt32(hdnTestOrderID.Value) : 0;
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendOrder(testOrderID, transactionID);
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

        #region Sending Order Process
        private string SendOrderViaMedinfrasAPI(int testOrderID, int transactionID)
        {
            string result = "";
            string messageText = string.Empty;
            try
            {
                string url = AppSession.SA0121;
                string defaultEmailAddress = AppSession.SA0129;
                string[] defaulEmailAddressInfo = defaultEmailAddress.Split('@');

                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression = string.Format("TransactionID = {0} AND ID IN ({1})", transactionID, hdnSelectedID.Value);
                    List<vPatientChargesDtLabForSendOrder> oList = BusinessLayer.GetvPatientChargesDtLabForSendOrderList(filterExpression);
                    vPatientWithNalageneticsReference oPatient = BusinessLayer.GetvPatientWithNalageneticsReferenceList(string.Format("MRN = {0}", AppSession.RegisteredPatient.MRN)).FirstOrDefault();
                    NalageneticsOrderParameter1 oData = new NalageneticsOrderParameter1();
                    if (oList.Count > 0)
                    {
                        foreach (vPatientChargesDtLabForSendOrder item in oList)
                        {
                            PGxTestOrder order = new PGxTestOrder();
                            order.skuCode = item.OldItemCode;
                            order.specimenType = "Buccal Swab";
                            order.physicianEmail = AppSession.SA0129;
                            order.remarks = string.Empty;
                            order.sampleId = string.Format("{0}_{1}", Request.Form[txtTransactionNo.UniqueID].ToString(), item.OldItemCode);
                            order.consentForSampleSharing = "1";

                            PgxContext context = new PgxContext();

                            ReasonForOrderingPgxTest obj = new ReasonForOrderingPgxTest();
                            List<ReasonForOrderingPgxTest> lstObj = new List<ReasonForOrderingPgxTest>();

                            obj.type = "DISEASE_RISK";
                            List<string> lstDiseaseRisk = new List<string>();
                            foreach (ListItem risk in chkListRisk.Items)
                            {
                                if (risk.Selected)
                                {
                                    lstDiseaseRisk.Add(risk.Value);
                                }
                            }
                            obj.values = lstDiseaseRisk;
                            lstObj.Add(obj);

                            string riskList = String.Join(";", lstDiseaseRisk.ToArray());

                            context.reasonForOrderingPgxTest = lstObj;
                            context.relevantClinicalInformation = new List<string> { txtClinicalNotes.Text };
                            context.needGeneticsCounseling = "no";

                            order.pgxContext = context;

                            PGxTestPatient patient = new PGxTestPatient();
                            patient.firstName = oVisit.FirstName;
                            patient.lastName = oVisit.LastName;
                            patient.gender = oVisit.GCGender == Constant.Gender.MALE ? "male" : "female";
                            patient.birthDate = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                            patient.address1 = oVisit.HomeAddress;
                            patient.address2 = string.Empty;
                            patient.country = "5ad09b4e3e767e328c80c14d"; // Nalagenetics Country Code for : Indonesia
                            patient.state = oPatient.State;
                            patient.city = oPatient.City;
                            patient.race = oPatient.Ethnic;
                            patient.phone = oVisit.MobilePhoneNo1;
                            patient.email = string.IsNullOrEmpty(oVisit.EmailAddress) ? string.Format("{0}@{1}", oPatient.MedicalNo.Replace('-', '_'), defaulEmailAddressInfo[1]) : oVisit.EmailAddress;
                            patient.postalCode = oVisit.ZipCode;
                            patient.acceptTermsConditionPrivacyPolicy = "true";
                            patient.nationality = oPatient.Nationality;
                            patient.icNumber = oVisit.SSN;
                            patient.externalPatientId = oVisit.MedicalNo;

                            oData.order = order;
                            oData.patient = patient;


                            string remarks = string.Format("{0}|{1}|{2}|{3}", txtClinicalNotes.Text, riskList, chkIsHasFamilyMemberWithPGxTest.Checked.ToString(), chkIsInterestedInADR.Checked.ToString());

                            APIMessageLog entityAPILog = new APIMessageLog()
                            {
                                MessageDateTime = DateTime.Now,
                                Recipient = "Pharmacogenomics",
                                Sender = "MEDINFRAS",
                                MessageText = JsonConvert.SerializeObject(oData)
                            };

                            messageText = entityAPILog.MessageText;

                            string[] apiKeyInfo = AppSession.SA0120.Split('|');

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/external-create-order", url));
                            request.Method = "POST";
                            request.ContentType = "application/json";
                            request.UserAgent = apiKeyInfo[0];
                            request.Headers.Add("x-api-key", apiKeyInfo[1]);

                            var json = JsonConvert.SerializeObject(oData);

                            byte[] bytes = Encoding.UTF8.GetBytes(json);
                            request.ContentLength = bytes.Length;
                            Stream putStream = request.GetRequestStream();
                            putStream.Write(bytes, 0, bytes.Length);
                            putStream.Close();
                            WebResponse response = (WebResponse)request.GetResponse();
                            string responseMsg = string.Empty;
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseMsg = sr.ReadToEnd();
                            };

                            NalageneticOrderResponse1 respInfo = JsonConvert.DeserializeObject<NalageneticOrderResponse1>(responseMsg);

                            if (respInfo.data.success)
                            {
                                result = string.Format("{0}|{1}", "1", txtTransactionNo.Text);

                                #region Update Order Status and Log API Message
                                try
                                {
                                    UpdateOrderStatus(item.ID, remarks);
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion

                                entityAPILog.IsSuccess = true;
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                            else
                            {
                                string errMessage = string.Join("<br/>", respInfo.data.errorMessages);
                                result = string.Format("{0}|{1}", "0", errMessage);
                                entityAPILog.IsSuccess = false;
                                entityAPILog.ErrorMessage = errMessage;
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to Pharmacogenomics Test Provider");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException webex)
            {
                WebResponse errResponse = webex.Response;
                using (Stream respStream = errResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    result = reader.ReadToEnd();
                }
                NalageneticOrderResponse1 respInfo = JsonConvert.DeserializeObject<NalageneticOrderResponse1>(result);

                string exMessage = string.Join("<br/>", respInfo.data.errorMessages);

                string errMessage = exMessage + "<br/><br/>" + string.Format("{0}/external-create-order", AppSession.SA0121);

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = "Pharmacogenomics",
                    Sender = "MEDINFRAS",
                    MessageText = messageText
                };
                entityAPILog.IsSuccess = false;
                entityAPILog.ErrorMessage = errMessage;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

                result = string.Format("{0}|{1}", "0", errMessage);
                return result;
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message + "<br/>" + string.Format("{0}/external-create-order", AppSession.SA0121);
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = "Pharmacogenomics",
                    Sender = "MEDINFRAS",
                    MessageText = messageText
                };
                entityAPILog.IsSuccess = false;
                entityAPILog.ErrorMessage = errMessage;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

                result = string.Format("{0}|{1}", "0", errMessage);
                return result;
            }
        }
        private string SendOrder(int testOrderID, int transactionID)
        {
            string result = "";
            string messageText = string.Empty;
            try
            {
                string url = AppSession.SA0121;
                string defaultEmailAddress = AppSession.SA0129;
                string[] defaulEmailAddressInfo = defaultEmailAddress.Split('@');

                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression = string.Format("TransactionID = {0} AND ID IN ({1})", transactionID, hdnSelectedID.Value);
                    List<vPatientChargesDtLabForSendOrder> oList = BusinessLayer.GetvPatientChargesDtLabForSendOrderList(filterExpression);
                    vPatientWithNalageneticsReference oPatient = BusinessLayer.GetvPatientWithNalageneticsReferenceList(string.Format("MRN = {0}", oVisit.MRN)).FirstOrDefault();
                    NalageneticsOrderParameter1 oData = new NalageneticsOrderParameter1();
                    if (oPatient != null)
                    {
                        if (oList.Count > 0)
                        {
                            foreach (vPatientChargesDtLabForSendOrder item in oList)
                            {
                                PGxTestOrder order = new PGxTestOrder();
                                order.skuCode = item.OldItemCode;
                                order.specimenType = "Buccal Swab";
                                order.physicianEmail = AppSession.SA0129;
                                order.remarks = string.Empty;
                                order.sampleId = string.Format("{0}_{1}", Request.Form[txtTransactionNo.UniqueID].ToString().Replace("/", "_"), item.OldItemCode);
                                order.consentForSampleSharing = "1";

                                PgxContext context = new PgxContext();

                                ReasonForOrderingPgxTest obj = new ReasonForOrderingPgxTest();
                                List<ReasonForOrderingPgxTest> lstObj = new List<ReasonForOrderingPgxTest>();

                                obj.type = "DISEASE_RISK";
                                List<string> lstDiseaseRisk = new List<string>();
                                foreach (ListItem risk in chkListRisk.Items)
                                {
                                    if (risk.Selected)
                                    {
                                        lstDiseaseRisk.Add(risk.Value);
                                    }
                                }
                                obj.values = lstDiseaseRisk;
                                lstObj.Add(obj);

                                string riskList = String.Join(";", lstDiseaseRisk.ToArray());

                                context.reasonForOrderingPgxTest = lstObj;
                                context.relevantClinicalInformation = new List<string> { txtClinicalNotes.Text };
                                context.needGeneticsCounseling = "no";

                                order.pgxContext = context;

                                PGxTestPatient patient = new PGxTestPatient();
                                patient.firstName = string.IsNullOrEmpty(oVisit.FirstName) ? " " : oVisit.FirstName;
                                patient.lastName = oVisit.LastName;
                                patient.gender = oVisit.GCGender == Constant.Gender.MALE ? "male" : "female";
                                patient.birthDate = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_1);
                                patient.address1 = oVisit.HomeAddress;
                                patient.address2 = string.Empty;
                                patient.country = "5ad09b4e3e767e328c80c14d"; // Nalagenetics Country Code for : Indonesia
                                patient.state = oPatient.State;
                                patient.city = oPatient.City;
                                patient.race = oPatient.Ethnic;
                                patient.phone = oVisit.MobilePhoneNo1;
                                patient.email = string.IsNullOrEmpty(oVisit.EmailAddress) ? string.Format("{0}@{1}", oPatient.MedicalNo.Replace('-', '_'), defaulEmailAddressInfo[1]) : oVisit.EmailAddress;
                                patient.postalCode = oVisit.ZipCode;
                                patient.acceptTermsConditionPrivacyPolicy = "true";
                                patient.nationality = oPatient.Nationality;
                                patient.icNumber = oVisit.SSN;
                                patient.externalPatientId = oVisit.MedicalNo;

                                oData.order = order;
                                oData.patient = patient;


                                string remarks = string.Format("{0}|{1}|{2}|{3}", txtClinicalNotes.Text, riskList, chkIsHasFamilyMemberWithPGxTest.Checked.ToString(), chkIsInterestedInADR.Checked.ToString());

                                APIMessageLog entityAPILog = new APIMessageLog()
                                {
                                    MessageDateTime = DateTime.Now,
                                    Recipient = "Pharmacogenomics",
                                    Sender = "MEDINFRAS",
                                    MessageText = JsonConvert.SerializeObject(oData)
                                };

                                messageText = entityAPILog.MessageText;

                                string[] apiKeyInfo = AppSession.SA0120.Split('|');

                                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/external-create-order", url));
                                request.Method = "POST";
                                request.ContentType = "application/json";
                                request.UserAgent = apiKeyInfo[0];
                                request.Headers.Add("x-api-key", apiKeyInfo[1]);

                                var json = JsonConvert.SerializeObject(oData);

                                byte[] bytes = Encoding.UTF8.GetBytes(json);
                                request.ContentLength = bytes.Length;
                                Stream putStream = request.GetRequestStream();
                                putStream.Write(bytes, 0, bytes.Length);
                                putStream.Close();
                                WebResponse response = (WebResponse)request.GetResponse();
                                string responseMsg = string.Empty;
                                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                                {
                                    responseMsg = sr.ReadToEnd();
                                };

                                NalageneticOrderResponse1 respInfo = JsonConvert.DeserializeObject<NalageneticOrderResponse1>(responseMsg);

                                if (respInfo.data.success)
                                {
                                    result = string.Format("{0}|{1}", "1", txtTransactionNo.Text);

                                    #region Update Order Status and Log API Message
                                    try
                                    {
                                        UpdateOrderStatus(item.ID, remarks);
                                    }
                                    catch (Exception ex)
                                    {
                                        result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                        break;
                                    }
                                    #endregion

                                    entityAPILog.IsSuccess = true;
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                                else
                                {
                                    string errMessage = string.Join("<br/>", respInfo.data.errorMessages);
                                    result = string.Format("{0}|{1}", "0", errMessage);
                                    entityAPILog.IsSuccess = false;
                                    entityAPILog.ErrorMessage = errMessage;
                                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                                }
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", "There is no order to be sent to Pharmacogenomics Test Provider");
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "Invalid Patient with Nalagenetic Reference");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException webex)
            {
                WebResponse errResponse = webex.Response;
                using (Stream respStream = errResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    result = reader.ReadToEnd();
                }
                NalageneticOrderResponse1 respInfo = JsonConvert.DeserializeObject<NalageneticOrderResponse1>(result);

                string exMessage = string.Join("<br/>", webex.Message);
                if (respInfo.data != null)
                {
                    exMessage = string.Join("<br/>", respInfo.data.errorMessages);   
                }

                string errMessage = exMessage + "<br/><br/>" + string.Format("{0}/external-create-order", AppSession.SA0121);

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = "Pharmacogenomics",
                    Sender = "MEDINFRAS",
                    MessageText = messageText
                };
                entityAPILog.IsSuccess = false;
                entityAPILog.ErrorMessage = errMessage;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

                result = string.Format("{0}|{1}", "0", errMessage);
                return result;                
            }
            catch (Exception ex)
            {
                string errMessage = ex.Message + "<br/>" + string.Format("{0}/external-create-order", AppSession.SA0121);
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = "Pharmacogenomics",
                    Sender = "MEDINFRAS",
                    MessageText = messageText
                };
                entityAPILog.IsSuccess = false;
                entityAPILog.ErrorMessage = errMessage;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

                result = string.Format("{0}|{1}", "0", errMessage);
                return result;
            }
        }

        private void UpdateOrderStatus(int id,string remarks)
        {
            //If Success, update order status to SENT
            PatientChargesDtInfo dtInfo = BusinessLayer.GetPatientChargesDtInfo(id);
            if (dtInfo != null)
            {
                dtInfo.Remarks = remarks;
                dtInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
            }
        }

        private void UpdateErrorLogMessage(string sender, string messageType, string messageCode, string messageText, string errorMessage, vPatientChargesDt item, string messageControlID = "", string deviceNo = "")
        {
            HL7Message log = new HL7Message();
            log.MessageDateTime = DateTime.Now;
            log.MessageControlID = Convert.ToString(item.ID);
            log.Sender = sender;
            log.DeviceNo = deviceNo;
            log.RegistrationID = item.RegistrationID;
            log.RegistrationNo = item.RegistrationNo;
            log.TransactionID = item.TransactionID;
            log.DetailTransactionID = item.ID;
            log.PatientName = item.PatientName;
            log.MessageType = messageType;
            log.MessageCode = messageCode;
            log.MessageText = messageText;
            log.MessageStatus = "ERR";
            log.ErrorMessage = errorMessage;
            BusinessLayer.InsertHL7Message(log);
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                vPatientChargesDtLabForSendOrder entity = e.Row.DataItem as vPatientChargesDtLabForSendOrder;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsProcessItem");

                if (entity.GCLISBridgingStatus == Constant.LIS_Bridging_Status.OPEN || string.IsNullOrEmpty(entity.GCLISBridgingStatus))
                {
                    chkIsSelected.Visible = true;
                }
                else
                {
                    chkIsSelected.Visible = false;
                }
            }
        }
        #endregion
    }
}