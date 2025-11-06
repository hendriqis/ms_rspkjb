using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Common;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Service;
using DevExpress.Web.ASPxCallbackPanel;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class PostIHSVaccinationHistoryCtl1 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');

            hdnRegistrationID.Value = paramInfo[0];
            hdnVisitID.Value = paramInfo[1];
            hdnMRN.Value = paramInfo[2];
            txtRegistrationNo.Text = paramInfo[3];

            if (hdnVisitID.Value != "0")
            {
                RegistrationInfo oRegistrationInfo = BusinessLayer.GetRegistrationInfo(Convert.ToInt32(hdnRegistrationID.Value));
                if (oRegistrationInfo != null)
                {
                    txtIHSEncounterID.Text = oRegistrationInfo.ExternalRegistrationNo;
                    BindGridView();
                }
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("MRN = {0} AND IsDeleted = 0", hdnMRN.Value);
            if (rblDisplayType.SelectedValue == "0")
            {
                filterExpression += string.Format(" AND VisitID = {0} AND IsCurrentVisit = 1", hdnVisitID.Value);
            }
            else
            {
                filterExpression += string.Format(" AND IsExternalProvider = 1", hdnVisitID.Value);
            }
            List<vVaccinationHistory> lstDetail = BusinessLayer.GetvVaccinationHistoryList(filterExpression);
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
                string[] resultInfo = "0|Unknown Protocol".Split('|');
                string referenceNo = string.Empty;
                bool isError = false;

                var processResult = SendInformationToIHS(hdnRegistrationID.Value);
                resultInfo = ((string)processResult).Split('|');

                referenceNo = resultInfo[1];
                isError = resultInfo[0] == "0";
                if (isError)
                {
                    errMessage = resultInfo[1];
                    result = false;
                }
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
        }

        private object SendInformationToIHS(string registrationID)
        {
            string result = "";
            try
            {
                string filterExpression = string.Format(" ID IN ({0})", hdnSelectedID.Value);
                List<vVaccinationHistory> oList = BusinessLayer.GetvVaccinationHistoryList(filterExpression);
                vRegistration entity = BusinessLayer.GetvRegistrationList(string.Format("RegistrationID = {0}", Convert.ToInt32(hdnRegistrationID.Value))).FirstOrDefault();

                if (oList.Count > 0)
                {
                    int errorNo = 0;
                    foreach (var item in oList)
                    {
                        IHSService oService = new IHSService();
                        string apiResult = string.Empty;
                        string[] apiResultInfo = apiResult.Split('|');

                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = Constant.BridgingVendor.SATUSEHAT,
                            Sender = Constant.BridgingVendor.HIS,
                            IsSuccess = true
                        };

                        if (rblDisplayType.SelectedValue == "0")
                        {
                            apiResult = oService.PostImmunization1(AppSession.UserLogin.HealthcareID, entity, item);
                        }
                        else
                        {
                            apiResult = oService.PostImmunization2(AppSession.UserLogin.HealthcareID, entity, item);
                        }
                        apiResultInfo = apiResult.Split('|');
                        if (apiResultInfo[0] == "0")
                        {
                            entityAPILog.IsSuccess = false;
                            entityAPILog.MessageText = apiResultInfo[1];
                            entityAPILog.Response = apiResultInfo[1];
                            Exception ex = new Exception(apiResultInfo[1]);

                            BusinessLayer.InsertAPIMessageLog(entityAPILog);

                            errorNo += 1;
                        }
                        else
                        {
                            entityAPILog.MessageText = apiResultInfo[1];
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }

                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", string.Format("There are {0} item(s) is rejected by the Endpoint",errorNo));
                            else
                                result = string.Format("{0}|{1}", "0", "The information is rejected by the endpoint..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Message.ToString());
                return result;
            }
        }

        protected string OnGetRegistrationNoFilterExpression()
        {
            string filterExpression = string.Format("(MRN = '{0}')", hdnMRN.Value);
            return filterExpression;
        }

        #region Sending Order Process
        private void UpdateOrderStatus(string sender, string messageType, string messageCode, string messageText, bool isSuccess, string errorMessage, vPatientChargesDt item, string messageControlID = "", string deviceNo = "")
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
            log.MessageStatus = isSuccess ? "OK" : "ERR";
            log.ErrorMessage = errorMessage;
            BusinessLayer.InsertHL7Message(log);

            if (isSuccess)
            {
                //If Success, update order status to SENT
                PatientChargesDtInfo dtInfo = BusinessLayer.GetPatientChargesDtInfo(item.ID);
                if (dtInfo != null)
                {
                    if (sender != Constant.HL7_Partner.MEDAVIS && sender != Constant.HL7_Partner.FUJIFILM && sender != Constant.HL7_Partner.ZED)
                    {
                        //Accession number is generated by HIS
                        dtInfo.ReferenceNo = messageControlID;

                        if (sender == Constant.HL7_Partner.INFINITT || sender == Constant.HL7_Partner.MEDSYNAPTIC)
                        {
                            dtInfo.ReferenceNo = item.ID.ToString();
                        }
                    }
                    dtInfo.GCRISBridgingStatus = Constant.RIS_Bridging_Status.SENT;
                    BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
                }
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
            
        }
        #endregion
    }
}