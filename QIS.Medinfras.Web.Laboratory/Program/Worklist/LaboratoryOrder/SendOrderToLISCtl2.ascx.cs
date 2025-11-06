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
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Text;
using QIS.Medinfras.Common;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class SendOrderToLISCtl2 : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            string[] paramInfo = param.Split('|');
            hdnTransactionID.Value = paramInfo[0];
            vPatientChargesHd1 oHeader = BusinessLayer.GetvPatientChargesHd1List(string.Format("TransactionID = {0}", hdnTransactionID.Value)).FirstOrDefault();
            if (oHeader != null)
            {
                txtTransactionNo.Text = oHeader.TransactionNo;
                BindGridView();
            }
        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
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
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                        AppSession.UserLogin.HealthcareID, //0
                        Constant.SettingParameter.LB_BRIDGING_LIS, //1
                        Constant.SettingParameter.LB_LIS_BRIDGING_PROTOCOL, //2
                        Constant.SettingParameter.LB_HL7_MESSAGE_FORMAT //3
                    ));

                string isBridgingToLis = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.LB_BRIDGING_LIS).FirstOrDefault().ParameterValue;
                string bridgingProtocol = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_BRIDGING_PROTOCOL).FirstOrDefault().ParameterValue;
                string hl7MessageFormat = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.LB_HL7_MESSAGE_FORMAT).FirstOrDefault().ParameterValue;

                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                {
                    if (isBridgingToLis == "1")
                    {
                        string[] resultInfo = "0|Unknown Protocol".Split('|');
                        switch (bridgingProtocol)
                        {
                            case Constant.LIS_Bridging_Protocol.WEB_API:
                                //var result1 = SendOrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                                //resultInfo = ((string)result1).Split('|');
                                break;
                            case Constant.LIS_Bridging_Protocol.HL7:
                                switch (hl7MessageFormat)
                                {
                                    case Constant.LIS_HL7MessageFormat.ROCHE:
                                        var rocheResult = SendRocheHL7OrderToLIS(transactionID);
                                        resultInfo = ((string)rocheResult).Split('|');
                                        break;
                                    default:
                                        resultInfo = "0|Unknown Protocol".Split('|');
                                        break;
                                }
                                break;
                            case Constant.LIS_Bridging_Protocol.LINK_DB:
                                //var result3 = SendOrderToRISLinkDB(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                                //resultInfo = ((string)result3).Split('|');
                                break;
                            default:
                                resultInfo = "0|Unknown Protocol".Split('|');
                                break;
                        }
                        referenceNo = resultInfo[1];
                        isError = resultInfo[0] == "0";
                        if (isError)
                        {
                            errMessage = resultInfo[1];
                            result = false;
                        }
                    }
                }
                result = true;
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
        private object SendRocheHL7OrderToLIS(int transactionID)
        {
            string result = "";
            try
            {
                //string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "OML^O21";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    string gcTitle = string.Empty;
                    string emailAddress = string.Empty;
                    if (oVisit.MRN > 0)
                    {
                        Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oVisit.MRN)).FirstOrDefault();
                        if (!string.IsNullOrEmpty(entityPatient.GCTitle))
                        {
                            StandardCode entitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}' AND IsDeleted = 0", entityPatient.GCTitle)).FirstOrDefault();
                            if (entitySC != null)
                            {
                                gcTitle = entitySC.StandardCodeName;
                            }
                            if (!string.IsNullOrEmpty(entityPatient.EmailAddress))
                            {
                                emailAddress = entityPatient.EmailAddress;
                            }
                        }
                    }
                    else
                    {
                        Guest entityPatient = BusinessLayer.GetGuestList(string.Format("GuestID = {0}", oVisit.GuestID)).FirstOrDefault();
                        if (!string.IsNullOrEmpty(entityPatient.GCTitle))
                        {
                            StandardCode entitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}' AND IsDeleted = 0", entityPatient.GCTitle)).FirstOrDefault();
                            if (entitySC != null)
                            {
                                gcTitle = entitySC.StandardCodeName;
                            }
                        }

                        if (!string.IsNullOrEmpty(entityPatient.EmailAddress))
                        {
                            emailAddress = entityPatient.EmailAddress;
                        }
                    }
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_LIS_HL7_BROKER)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "LABORATORY";
                        string remarksHd = string.Empty;
                        string remarksDt = string.Empty;
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            if (oOrderHd.GCToBePerformed == Constant.ToBePerformed.SCHEDULLED)
                            {
                                orderDate = oOrderHd.ScheduledDate;
                                orderTime = oOrderHd.ScheduledTime;
                            }
                            else
                            {
                                orderDate = oOrderHd.TestOrderDate;
                                orderTime = oOrderHd.TestOrderTime;
                            }

                            remarks = Helper.ReplaceLineBreak((!string.IsNullOrEmpty(oOrderHd.Remarks) ? oOrderHd.Remarks : string.Empty));
                        }
                        else
                        {
                            if (oVisit.DepartmentID == Constant.Facility.DIAGNOSTIC || oVisit.DepartmentID == Constant.Facility.IMAGING)
                            {
                                if (oVisit.ReferrerParamedicID != 0)
                                {
                                    ParamedicMaster oParamedic = BusinessLayer.GetParamedicMaster(oVisit.ReferrerParamedicID);
                                    if (oParamedic != null)
                                    {
                                        orderParamedicCode = oParamedic.ParamedicCode;
                                        orderParamedicName = oParamedic.FullName;
                                    }
                                }
                                else if (!string.IsNullOrEmpty(oVisit.ReferrerCode))
                                {
                                    orderParamedicCode = oVisit.ReferrerCode;
                                    orderParamedicName = oVisit.ReferrerName;
                                }
                            }
                        }

                        string orderDateTime = string.Format("{0}{1}00", orderDate.ToString(Constant.FormatString.DATE_FORMAT_112), orderTime.Replace(":", ""));

                        int sequenceNo = 1;
                        int errorNo = 0;

                        #region Initialization
                        HL7MessageText hl7Message = new HL7MessageText();
                        string detailID = oList.FirstOrDefault().ID.ToString();
                        string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                        string modality = string.Empty;
                        string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                        string orderControl = "SC";
                        string orderStatus = "CM";
                        string priority = string.Empty;
                        string diagnoseName = string.Empty;

                        foreach (vPatientChargesDt dt in oList)
                        {
                            if (dt.GCLISBridgingStatus == Constant.LIS_Bridging_Status.SENT)
                            {
                                orderControl = "XO";
                            }

                            if (dt.IsDeleted)
                            {
                                orderStatus = "CA";
                            }

                            if (!string.IsNullOrEmpty(dt.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = string.Format("{0} {1}", Helper.ReplaceLineBreak(oHeader.Remarks), Helper.ReplaceLineBreak(dt.Remarks));
                                }
                                else
                                {
                                    remarks = Helper.ReplaceLineBreak(dt.Remarks);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = Helper.ReplaceLineBreak(oHeader.Remarks);
                                }
                            }

                            priority = dt.IsCITO ? "T" : "R";
                            diagnoseName = dt.DiagnoseTestOrder == null ? "-" : dt.DiagnoseTestOrder.Trim();
                        }

                        if (remarks.Length >= 500)
                            remarks = remarks.Substring(0, 500);

                        ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", oList.FirstOrDefault().ParamedicCode)).FirstOrDefault();
                        string requestedPhysicianName = string.Format("{0}^{1}", oList.FirstOrDefault().ParamedicCode, oList.FirstOrDefault().ParamedicName);
                        if (entityParamedic != null)
                        {
                            requestedPhysicianName = string.Format("{0}^{1}", entityParamedic.ParamedicCode, entityParamedic.FullName);
                        }
                        #endregion

                        #region MSH
                        HL7Segment msh = new HL7Segment();
                        msh.Field(0, "MSH");
                        msh.Field(1, ""); //will be ignored
                        msh.Field(2, @"^~\&");
                        msh.Field(3, "MEDINFRAS-API_LIS");
                        msh.Field(4, AppSession.UserLogin.HealthcareID);
                        msh.Field(5, CommonConstant.HL7_ROCHE_MSG.IDENTIFICATION_1);
                        msh.Field(6, CommonConstant.HL7_ROCHE_MSG.IDENTIFICATION_2);
                        msh.Field(7, orderDateTime);
                        msh.Field(8, string.Empty);
                        msh.Field(9, string.Format("{0}^OML_O21", messageCode));
                        msh.Field(10, messageControlID);
                        msh.Field(11, "P");
                        msh.Field(12, CommonConstant.HL7_ROCHE_MSG.HL7_VERSION);
                        msh.Field(13, string.Empty);
                        msh.Field(14, string.Empty);
                        msh.Field(15, "AL");
                        msh.Field(16, "ER");
                        msh.Field(17, string.Empty);
                        msh.Field(18, "8859/1");

                        hl7Message.Add(msh);
                        #endregion

                        #region PID
                        string patientName = string.Empty;
                        patientName = string.Format("{0}^{1}", oVisit.PatientName, gcTitle);
                        string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                        string gender = (oVisit.GCGender == Constant.Gender.UNSPECIFIED) ? "U" : (!string.IsNullOrEmpty(oVisit.GCGender)) ? oVisit.GCGender.Split('^')[1] : string.Empty;
                        string patientAddress = string.Format("{0}^^{1}^{2}^{3}^{4}", !string.IsNullOrEmpty(oVisit.StreetName) ? oVisit.StreetName.Replace("\n", " ").Replace(Environment.NewLine, " ").TrimEnd() : string.Empty, oVisit.City, oVisit.State, oVisit.ZipCode, oVisit.cfPatientNationality);

                        string patientMobileNo = string.IsNullOrEmpty(oVisit.MobilePhoneNo1) ? string.Empty : oVisit.MobilePhoneNo1.Trim();
                        string patientEmailAddress = emailAddress;
                        string field13 = string.Format("^^^^^^^^^^^{0}", !string.IsNullOrEmpty(patientMobileNo) ? patientMobileNo : string.Empty);

                        HL7Segment pid = new HL7Segment();
                        pid.Field(0, "PID");
                        pid.Field(1, "1");
                        pid.Field(2, string.Empty);
                        pid.Field(3, oVisit.MedicalNo);
                        pid.Field(4, string.Empty);
                        pid.Field(5, patientName);
                        pid.Field(6, !string.IsNullOrEmpty(oVisit.MotherName) ? oVisit.MotherName : string.Empty);
                        pid.Field(7, dateofBirth);
                        pid.Field(8, gender);
                        pid.Field(9, "Y");
                        pid.Field(10, oVisit.PatientAge); //0 Th 0 Bln 0 Hr
                        pid.Field(11, patientAddress);
                        pid.Field(12, patientEmailAddress);
                        pid.Field(13, field13);

                        hl7Message.Add(pid);
                        #endregion

                        #region PV1
                        string pv1_Param1 = "I";
                        string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                        string bedCode = oVisit.BedCode;
                        string patientLocation = string.Format("^^{0}", !string.IsNullOrEmpty(oVisit.RoomCode) ? oVisit.RoomCode : string.Empty);

                        switch (oVisit.DepartmentID)
                        {
                            case Constant.Facility.INPATIENT:
                                pv1_Param1 = "I";
                                break;
                            case Constant.Facility.OUTPATIENT:
                                pv1_Param1 = "O";
                                break;
                            default:
                                pv1_Param1 = "U";
                                break;
                        }

                        int noOfItem = oList.Count;
                        string orderPhysicianName = string.Format("{0}^{1}", orderParamedicCode, orderParamedicName);
                        vConsultVisit entityVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                        string field19 = string.Format("{0}^^^{1}^^^^^{2}", entityVisit.RegistrationNo, entityVisit.ParamedicName, entityVisit.ServiceUnitName);

                        HL7Segment pv1 = new HL7Segment();
                        pv1.Field(0, "PV1");
                        pv1.Field(1, "1");
                        pv1.Field(2, pv1_Param1);
                        pv1.Field(3, patientLocation);
                        pv1.Field(4, string.Empty);
                        pv1.Field(5, string.Empty);
                        pv1.Field(6, string.Empty);
                        pv1.Field(7, string.Empty);
                        pv1.Field(8, orderPhysicianName);
                        pv1.Field(9, string.Empty);
                        pv1.Field(10, oVisit.ServiceUnitCode);
                        pv1.Field(11, string.Empty);
                        pv1.Field(12, string.Empty);
                        pv1.Field(13, string.Empty);
                        pv1.Field(14, string.Empty);
                        pv1.Field(15, string.Empty);
                        pv1.Field(16, string.Empty);
                        pv1.Field(17, string.Empty);
                        pv1.Field(18, string.Empty);
                        pv1.Field(19, field19);

                        hl7Message.Add(pv1);
                        #endregion

                        //Alternate Order Number for LIS purpose
                        string risOrderNo = string.Format("{0}", detailID);

                        #region ORC
                        HL7Segment orc = new HL7Segment();
                        orc.Field(0, "ORC");
                        orc.Field(1, orderControl);
                        orc.Field(2, string.Format("{0}^", risOrderNo));
                        orc.Field(3, oHeader.TransactionNo);
                        orc.Field(4, string.Empty);
                        orc.Field(5, orderStatus);
                        orc.Field(6, oVisit.PatientAge);
                        orc.Field(7, string.Empty);
                        orc.Field(8, string.Empty);
                        orc.Field(9, orderDateTime);
                        orc.Field(10, string.Empty);
                        orc.Field(11, string.Empty);
                        orc.Field(12, requestedPhysicianName);
                        orc.Field(13, string.Format("^^^{0}^^^^^{1}", oVisit.ServiceUnitCode, oVisit.ServiceUnitName));
                        orc.Field(14, string.Empty);
                        orc.Field(15, string.Empty);
                        orc.Field(16, string.Empty);
                        orc.Field(17, AppSession.UserLogin.HealthcareName); //Site code
                        //orc.Field(17, string.Format("{0}^{1}", oVisit.ServiceUnitCode, oVisit.ServiceUnitName);
                        orc.Field(18, string.Empty);
                        orc.Field(19, string.Empty);
                        orc.Field(20, string.Empty);
                        orc.Field(21, string.Empty);

                        hl7Message.Add(orc);
                        #endregion

                        foreach (vPatientChargesDt item in oList)
                        {
                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, sequenceNo.ToString());
                            obr.Field(2, risOrderNo);
                            obr.Field(3, risOrderNo);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, !string.IsNullOrEmpty(item.AlternateItemName) ? item.AlternateItemName : item.ItemName1));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, "A");
                            obr.Field(11, string.Empty);
                            obr.Field(12, string.Empty);
                            obr.Field(13, string.Empty);
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, string.Empty);
                            obr.Field(17, accessionNo);
                            obr.Field(18, string.Empty);
                            obr.Field(19, string.Empty);
                            obr.Field(20, string.Empty);
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, modality);
                            obr.Field(24, modality);
                            obr.Field(25, string.Empty);
                            obr.Field(26, "^^5");
                            obr.Field(27, string.Empty);
                            obr.Field(28, string.Empty);
                            obr.Field(29, string.Empty);
                            obr.Field(30, string.Empty);
                            obr.Field(31, transactionNo);
                            obr.Field(32, string.Empty);
                            obr.Field(33, string.Empty);
                            obr.Field(34, string.Empty);
                            obr.Field(35, string.Empty);
                            obr.Field(36, string.Empty);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}^^^{1}", item.ItemCode, item.AlternateItemName));

                            hl7Message.Add(obr);
                            #endregion   

                            sequenceNo += 1;
                        }

                        #region Send To LIS Broker Service
                        if (oParam != null)
                        {
                            string[] paramInfo = oParam.ParameterValue.Split(':');
                            ipaddress = paramInfo[0];
                            port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                            string msgText = string.Empty;
                            string[] resultInfo = new string[0];
                            bool isSuccess = true;

                            try
                            {
                                msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = SendMessageToListener(ipaddress, port, msgText);
                                resultInfo = result.Split('|');
                                isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;
                            }
                            catch (Exception ex)
                            {
                                result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                errorNo += 1;
                            }

                            #region Update Order Status and Log HL7 Message
                            try
                            {
                                UpdateOrderStatusList(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.ROCHE, messageCode, msgText, isSuccess, isSuccess ? string.Empty : resultInfo[1], oList, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                            }
                            catch (Exception ex)
                            {
                                result = string.Format("{0}|{1} ({2}) (resultInfo : {3})", "0", "An error occured when update order status", ex.Message, resultInfo[1]);
                            }
                            #endregion
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", "Invalid Configuration for LIS HL7 Broker IP Address");
                        }
                        #endregion

                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the LIS");
                            else
                            {
                                if (result.Split('|')[0] != "0")
                                {
                                    result = string.Format("{0}|{1}", "0", "The order is rejected by LIS. Please check the log message");
                                }
                            }
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to LIS");
                    }
                #endregion
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Message.ToString());
                return result;
            }
        }

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

                        if (sender == Constant.HL7_Partner.INFINITT)
                        {
                            dtInfo.ReferenceNo = item.ID.ToString();
                        }
                    }
                    dtInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                    BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
                }
            }
        }

        private void UpdateOrderStatusList(string sender, string messageType, string messageCode, string messageText, bool isSuccess, string errorMessage, List<vPatientChargesDt> lstItem, string messageControlID = "", string deviceNo = "")
        {
            if (lstItem.Count > 0)
            {
                HL7Message log = new HL7Message();
                log.MessageDateTime = DateTime.Now;
                log.MessageControlID = Convert.ToString(lstItem.FirstOrDefault().ID);
                log.Sender = sender;
                log.DeviceNo = deviceNo;
                log.RegistrationID = lstItem.FirstOrDefault().RegistrationID;
                log.RegistrationNo = lstItem.FirstOrDefault().RegistrationNo;
                log.TransactionID = lstItem.FirstOrDefault().TransactionID;
                log.DetailTransactionID = lstItem.FirstOrDefault().ID;
                log.PatientName = lstItem.FirstOrDefault().PatientName;
                log.MessageType = messageType;
                log.MessageCode = messageCode;
                log.MessageText = messageText;
                log.MessageStatus = isSuccess ? "OK" : "ERR";
                log.ErrorMessage = errorMessage;
                BusinessLayer.InsertHL7Message(log);

                foreach (vPatientChargesDt item in lstItem)
                {
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

                                if (sender == Constant.HL7_Partner.INFINITT)
                                {
                                    dtInfo.ReferenceNo = item.ID.ToString();
                                }
                            }
                            dtInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                            BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
                        }
                    }
                }
            }
        }

        protected void grdView_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow || e.Row.RowType == DataControlRowType.EmptyDataRow)
            {
                vPatientChargesDt entity = e.Row.DataItem as vPatientChargesDt;
                CheckBox chkIsSelected = (CheckBox)e.Row.FindControl("chkIsProcessItem");

                if (entity.GCRISBridgingStatus == Constant.RIS_Bridging_Status.OPEN || entity.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT || string.IsNullOrEmpty(entity.GCRISBridgingStatus))
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

        public static string SendMessageToListener(string ipaddress, string port, string message)
        {
            string result = string.Format("{0}|{1}", "1", "OK");

            TcpClient client = new TcpClient();
            client.Connect(IPAddress.Parse(ipaddress), Convert.ToInt16(port));
            // Retrieve the network stream. 
            NetworkStream stream = client.GetStream();
            // Create a BinaryWriter for writing to the stream. 
            using (BinaryWriter w = new BinaryWriter(stream, Encoding.UTF8))
            {
                w.Write(string.Format("{0}", message).ToCharArray());

                #region Receive ACK Response From Server
                using (BinaryReader r = new BinaryReader(stream, Encoding.GetEncoding(1252)))
                {
                    // Reads NetworkStream into a byte buffer.
                    int length = (int)client.ReceiveBufferSize;
                    byte[] buffer = new byte[client.ReceiveBufferSize];

                    stream.Read(buffer, 0, length);

                    string data = Encoding.UTF8.GetString(buffer);

                    //Find start of MLLP frame, a VT character ...
                    int start = data.IndexOf((char)0x0B);
                    if (start >= 0)
                    {
                        //Look for the end of the frame, a FS Character
                        int end = data.IndexOf((char)0x1C);
                        if (end > start)
                        {
                            string temp = data.Substring(start + 1, end - start);
                            result = ResponseToACKMessage(temp);
                        }
                    }

                    //result = ResponseToACKMessage(data);
                }
                #endregion
            }

            return result;
        }

        private static string ResponseToACKMessage(string data)
        {
            string result = string.Empty;

            var msg = new HL7MessageText();
            msg.Parse(data);

            HL7Segment msa = msg.FindSegment("MSA");
            result = msa.Field(1) == "CA" ? string.Format("{0}|ACK: OK ({1})|", "1", msa.Field(2)) : string.Format("{0}|ACK: ERR ({1})|", "0", msa.Field(2), msa.Field(2));

            return result;
        }
    }
}