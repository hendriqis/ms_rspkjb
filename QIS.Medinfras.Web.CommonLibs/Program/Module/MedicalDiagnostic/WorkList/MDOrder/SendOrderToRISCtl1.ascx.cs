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
    public partial class SendOrderToRISCtl1 : BaseProcessPopupCtl
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
                List<SettingParameterDt> lstParameter = BusinessLayer.GetSettingParameterDtList(string.Format(
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.IS_RIS_BRIDGING, //1
                                                                        Constant.SettingParameter.IS_RIS_BRIDGING_PROTOCOL, //2
                                                                        Constant.SettingParameter.IS_HL7_MESSAGE_FORMAT //3
                                                                    ));

                string isBridgingToRIS = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_BRIDGING).FirstOrDefault().ParameterValue;
                string risBridgingProtocol = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_BRIDGING_PROTOCOL).FirstOrDefault().ParameterValue;
                string hl7MessageFormat = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.IS_HL7_MESSAGE_FORMAT).FirstOrDefault().ParameterValue;

                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                if (isBridgingToRIS == "1")
                {
                    string[] resultInfo = "0|Unknown Protocol".Split('|');
                    switch (risBridgingProtocol)
                    {
                        case Constant.RIS_Bridging_Protocol.WEB_API:
                            //List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND ID IN ({1})", transactionID, hdnSelectedID.Value));
                            var result1 = SendToMedinfrasAPI(transactionID, hdnSelectedID.Value.ToString());

                            //var result1 = SendOrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                            resultInfo = ((string)result1).Split('|');
                            break;
                        case Constant.RIS_Bridging_Protocol.HL7:
                            switch (hl7MessageFormat)
                            {
                                case Constant.RIS_HL7MessageFormat.MEDAVIS:
                                    var medavisResult = SendMedavisHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)medavisResult).Split('|');
                                    break;
                                case Constant.RIS_HL7MessageFormat.INFINITT:
                                    var infinittResult = SendInfinittHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)infinittResult).Split('|');
                                    break;
                                case Constant.RIS_HL7MessageFormat.INFINITT_RSSES:
                                    var infinittResultRSSES = SendInfinittHL7OrderToRISRSSES(transactionID);
                                    resultInfo = ((string)infinittResultRSSES).Split('|');
                                    break;

                                case Constant.RIS_HL7MessageFormat.FUJI_FILM:
                                    var fujiResult = SendFujiFilmHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)fujiResult).Split('|');
                                    break;
                                case Constant.RIS_HL7MessageFormat.ZED:
                                    var zedResult = SendZedHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)zedResult).Split('|');
                                    break;
                                case Constant.RIS_HL7MessageFormat.MEDSYNAPTIC:
                                    var medsynapticResult = SendMedsynapticHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)medsynapticResult).Split('|');
                                    break;
                                case Constant.RIS_HL7MessageFormat.LIFETRACK:
                                    var lifeTrackResult = SendLifeTrackHL7OrderToRIS(transactionID);
                                    resultInfo = ((string)lifeTrackResult).Split('|');
                                    break;
                                //case Constant.RIS_HL7MessageFormat.JIVEX:
                                //    var medsynapticResult = SendMedsynapticHL7OrderToRIS(transactionID);
                                //    resultInfo = ((string)medsynapticResult).Split('|');
                                //    break;
                                default:
                                    resultInfo = "0|Unknown Protocol".Split('|');
                                    break;
                            }
                            break;
                        case Constant.RIS_Bridging_Protocol.LINK_DB:
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
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
            }
            return result;
        }

        #region Sending Order Process
        private object SendFujiFilmHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            remarks = oOrderHd.Remarks;
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

                        List<vPatientDiagnosis1> entityDiagnose = BusinessLayer.GetvPatientDiagnosis1List(string.Format("VisitID = {0} AND GCDiagnoseType IN ('{1}','{2}') ORDER BY ID DESC", oVisit.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS, Constant.DiagnoseType.EARLY_DIAGNOSIS));

                        string orderDateTime = string.Format("{0}{1}00", orderDate.ToString(Constant.FormatString.DATE_FORMAT_112), orderTime.Replace(":", ""));
                        string transactionDateTime = string.Format("{0}{1}00", oList.FirstOrDefault().TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112), oList.FirstOrDefault().TransactionTime.Replace(":", ""));

                        int sequenceNo = 1;
                        int errorNo = 0;
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}", messageControlID);
                            string orderStatus = "NW";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                    remarks = string.Format("{0}{1}{2}", oHeader.Remarks, Environment.NewLine, item.Remarks);
                                else
                                    remarks = item.Remarks;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = oHeader.Remarks;
                                }
                            }

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            string requestedParamedicCode = item.ParamedicCode;
                            string requestedParamedicName = item.ParamedicName;
                            if (oParamedic != null)
                            {
                                requestedParamedicCode = oParamedic.ParamedicCode;
                                requestedParamedicName = oParamedic.FullName;
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_FUJIFILM_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_FUJIFILM_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_MEDAVIS_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "ER");
                            msh.Field(16, "ER");
                            msh.Field(17, string.Empty);
                            msh.Field(18, "8859/1");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            string patientName = string.Format("{0}", oVisit.PatientName);
                            string dateofBirth = string.Format("{0}000000", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^^^^", orderParamedicCode);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, orderParamedicCode);
                            pv1.Field(8, orderParamedicName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Format("{0}^{1}", oVisit.BusinessPartnerCode, oVisit.BusinessPartnerName));
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, risOrderNo);
                            orc.Field(5, string.Empty);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, requestedParamedicCode);
                            orc.Field(13, requestedParamedicName);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, string.Empty);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            //string diagnose = !string.IsNullOrEmpty(entityDiagnose.cfDiagnosisText) ? entityDiagnose.cfDiagnosisText : string.Empty;
                            string diagnose = string.Empty;
                            if (entityDiagnose.Count > 0)
                            {
                                var listMainDiagnose = entityDiagnose.Where(d => d.GCDiagnoseType == Constant.DiagnoseType.MAIN_DIAGNOSIS).FirstOrDefault();
                                if (listMainDiagnose != null)
                                {
                                    diagnose = listMainDiagnose.DiagnoseName;
                                }
                                if (string.IsNullOrEmpty(diagnose))
                                {
                                    var listEarlyDiagnose = entityDiagnose.Where(d => d.GCDiagnoseType == Constant.DiagnoseType.EARLY_DIAGNOSIS).FirstOrDefault();
                                    if (listEarlyDiagnose != null)
                                    {
                                        diagnose = listEarlyDiagnose.DiagnoseName;
                                    }
                                }
                            }

                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, item.ItemCode);
                            obr.Field(5, item.ItemName1);
                            obr.Field(6, priority);
                            obr.Field(7, transactionDateTime);
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
                            obr.Field(18, remarks);
                            obr.Field(19, diagnose); //Kode Diagnosa Utama / Masuk
                            obr.Field(20, string.Empty);
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, modality);
                            obr.Field(24, string.Empty);
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
                            obr.Field(43, string.Empty);

                            hl7Message.Add(obr);
                            #endregion

                            #region NTE
                            HL7Segment nte = new HL7Segment();
                            nte.Field(0, "NTE");
                            nte.Field(1, string.Empty);
                            nte.Field(2, string.Format("1.840.{0}",accessionNo));
                            nte.Field(3, remarks);

                            hl7Message.Add(nte);
                            #endregion

                            #region Send To RIS Broker Service
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.Fujifilm, messageCode, msgText, isSuccess, resultInfo[2], item, messageControlID, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendMedavisHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            SettingParameterDt imagingHSU = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode = '{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_KODE_PENUNJANG_RADIOLOGI)).FirstOrDefault();
                            if (imagingHSU != null)
                            {
                                if (oVisit.HealthcareServiceUnitID == Convert.ToInt32(imagingHSU.ParameterValue))
                                {
                                    if (!string.IsNullOrEmpty(oVisit.ReferrerCode))
                                    {
                                        orderParamedicCode = oVisit.ReferrerCode;
                                        orderParamedicName = oVisit.ReferrerName;
                                    }
                                    else if (oVisit.ReferrerParamedicID > 0)
                                    {
                                        ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(oVisit.ReferrerParamedicID);
                                        if (entityParamedic != null)
                                        {
                                            orderParamedicCode = entityParamedic.ParamedicCode;
                                            orderParamedicName = entityParamedic.FullName;
                                        }
                                    }
                                }
                            }

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            remarks = oOrderHd.Remarks;
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
                                else if (oVisit.ReferrerParamedicID > 0)
                                {
                                    ParamedicMaster entityParamedic = BusinessLayer.GetParamedicMaster(oVisit.ReferrerParamedicID);
                                    if (entityParamedic != null)
                                    {
                                        orderParamedicCode = entityParamedic.ParamedicCode;
                                        orderParamedicName = entityParamedic.FullName;
                                    }
                                }
                            }
                        }

                        string orderDateTime = string.Format("{0}{1}00", orderDate.ToString(Constant.FormatString.DATE_FORMAT_112), orderTime.Replace(":", ""));

                        int sequenceNo = 1;
                        int errorNo = 0;
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                            string orderStatus = "NW";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                    remarks = string.Format("{0}{1}{2}", oHeader.Remarks, Environment.NewLine, item.Remarks);
                                else
                                    remarks = item.Remarks;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = oHeader.Remarks;
                                }
                            }

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_MEDAVIS_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_MEDAVIS_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "ER");
                            msh.Field(16, "ER");
                            msh.Field(17, string.Empty);
                            msh.Field(18, "8859/1");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string dateofBirth = string.Format("{0}000000", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^^^^", orderParamedicCode);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, risOrderNo);
                            orc.Field(5, string.Empty);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, requestedPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, string.Empty);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, item.AlternateItemName));
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
                            obr.Field(24, string.Empty);
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

                            #region NTE
                            HL7Segment nte = new HL7Segment();
                            nte.Field(0, "NTE");
                            nte.Field(1, string.Empty);
                            nte.Field(2, string.Empty);
                            nte.Field(3, remarks);

                            hl7Message.Add(nte);
                            #endregion

                            #region Send To RIS Broker Service
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.Medavis, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendInfinittHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);

                    PatientDiagnosis diagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", oVisit.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();
                     
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}') ", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER, Constant.SettingParameter.IS0026));

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "";
                        string catatanKlinis = "";
                        string PatientVisaNumber = oHeader.PatientVisaNumber;
                        string ParamPoliRuang = string.Format("{0}{1}", oVisit.ServiceUnitCode, oVisit.RoomCode);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            catatanKlinis = oOrderHd.Remarks;
                            //remarks = oOrderHd.Remarks;
                            if (oHeader.IsOrderCreatedBySystem)
                            {
                                vRegistration3 registration = BusinessLayer.GetvRegistration3List(string.Format("RegistrationID = {0}", oHeader.RegistrationID)).FirstOrDefault();
                                if (registration.ReferrerParamedicID > 0)
                                {
                                    string strFilterExpression = string.Format("ParamedicID = '{0}'", registration.ReferrerParamedicID);
                                    ParamedicMaster pm = BusinessLayer.GetParamedicMasterList(strFilterExpression).FirstOrDefault();
                                    if (pm != null)
                                    {
                                        orderParamedicCode = pm.ParamedicCode;
                                        orderParamedicName = pm.FullName;
                                    }
                                }
                                else if (registration.ReferrerID > 0)
                                {
                                    vReferrer oref = BusinessLayer.GetvReferrerList(string.Format("BusinessPartnerID='{0}'", registration.ReferrerID)).FirstOrDefault();
                                    if (oref != null)
                                    {
                                        orderParamedicCode = oref.BusinessPartnerCode;
                                        orderParamedicName = oref.BusinessPartnerName;
                                    }
                                }
                            }
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                            string orderStatus = "NW";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            //if (!string.IsNullOrEmpty(item.Remarks))
                            //{
                            //    if (!string.IsNullOrEmpty(oHeader.Remarks))
                            //        remarks = string.Format("{0}{1}{2}", oHeader.Remarks, ";", item.Remarks);
                            //    else
                            //        remarks += string.Format("{0}{1}", ";", item.Remarks);
                            //}
                            //else
                            //{
                            //    if (!string.IsNullOrEmpty(oHeader.Remarks))
                            //    {
                            //        remarks = oHeader.Remarks;
                            //    }
                            //}

                            SettingParameterDt isRemaksSend = lstParam.Where(p => p.ParameterCode == Constant.SettingParameter.IS0026).FirstOrDefault();
                            if (isRemaksSend.ParameterValue == "1")
                            {
                                if (diagnose != null)
                                {
                                    remarks = diagnose.DiagnosisText;
                                }
                            }
                            else {
                                remarks = catatanKlinis;
                            }
                            
                            if (remarks.Length >= 300)
                                remarks = remarks.Substring(0, 300);

                            remarks = Regex.Replace(remarks, @"\r\n?|\n", ";");
                            remarks = remarks.Replace(System.Environment.NewLine, ";");
                            remarks = remarks.Replace("|", ";");

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_INFINITT_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "AL");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string medNo = oVisit.MRN != null && oVisit.MRN != 0 ? oVisit.MedicalNo : oVisit.GuestNo;
                            string patientName = oVisit.PatientName;
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, medNo);
                            pid.Field(3, medNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}", orderParamedicCode, orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, oVisit.RoomCode);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, ParamPoliRuang);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, oHeader.PatientVisaNumber);
                            pv1.Field(19, oVisit.RegistrationNo);
                            pv1.Field(20, string.Empty);
                            pv1.Field(21, string.Empty);
                            pv1.Field(22, string.Empty);
                            pv1.Field(23, string.Empty);
                            pv1.Field(24, string.Empty);
                            pv1.Field(25, string.Empty);
                            pv1.Field(26, string.Empty);
                            pv1.Field(27, string.Empty);
                            pv1.Field(28, string.Empty);
                            pv1.Field(29, string.Empty);
                            pv1.Field(30, string.Empty);
                            pv1.Field(31, string.Empty);
                            pv1.Field(32, string.Empty);
                            pv1.Field(33, string.Empty);
                            pv1.Field(34, string.Empty);
                            pv1.Field(35, string.Empty);
                            pv1.Field(36, string.Empty);
                            pv1.Field(37, string.Empty);
                            pv1.Field(38, string.Empty);
                            pv1.Field(39, string.Empty);
                            pv1.Field(40, string.Empty);
                            pv1.Field(41, string.Empty);
                            pv1.Field(42, string.Empty);
                            pv1.Field(43, string.Empty);
                            pv1.Field(44, orderDateTime);
                            pv1.Field(45, string.Empty);
                            pv1.Field(46, string.Empty);
                            pv1.Field(47, string.Empty);
                            pv1.Field(48, string.Empty);
                            pv1.Field(49, string.Empty);
                            pv1.Field(50, string.Empty);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);


                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, string.Empty);
                            orc.Field(5, "a");
                            orc.Field(6, string.Empty);
                            orc.Field(7, orderDateTime);
                            orc.Field(8, string.Empty);
                            orc.Field(9, string.Empty);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, orderPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, oVisit.DepartmentID);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Format("{0}^{1}^^^^^", AppSession.UserLogin.UserName, AppSession.UserLogin.UserFullName));

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, item.ItemName1));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, string.Empty);
                            obr.Field(11, string.Empty);
                            obr.Field(12, string.Empty);
                            obr.Field(13, remarks);
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, orderPhysicianName);
                            obr.Field(17, string.Empty);
                            obr.Field(18, risOrderNo);
                            obr.Field(19, accessionNo);
                            obr.Field(20, string.Format("{0}", transactionNo));
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, string.Empty);
                            obr.Field(24, modality);
                            obr.Field(25, string.Empty);
                            obr.Field(26, string.Empty);
                            obr.Field(27, string.Format("^^^{0}", orderDateTime));
                            obr.Field(28, string.Empty);
                            obr.Field(29, string.Empty);
                            obr.Field(30, string.Empty);
                            obr.Field(31, string.Format("{0}", oVisit.RegistrationNo));
                            obr.Field(32, "^^^^^^");
                            obr.Field(33, "^^^^^^");
                            obr.Field(34, string.Empty);
                            obr.Field(35, "^^^^^^");
                            obr.Field(36, string.Empty);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}^^^{1}", item.AlternateItemName, item.ItemCode));

                            hl7Message.Add(obr);
                            #endregion

                            #region ZDS
                            HL7Segment zds = new HL7Segment();
                            zds.Field(0, "ZDS");
                            zds.Field(1, string.Empty);
                            zds.Field(2, string.Empty);
                            zds.Field(3, string.Empty);
                            zds.Field(4, string.Empty);

                            hl7Message.Add(zds);
                            #endregion

                            #region Send To RIS Broker Service
                            SettingParameterDt oParam = lstParam.Where(p => p.ParameterCode == Constant.SettingParameter.IS_RIS_HL7_BROKER).FirstOrDefault();
                            if (oParam != null)
                            {
                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                              
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";
                                
                                 result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.Infinitt, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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
        private object SendInfinittHL7OrderToRISRSSES(int transactionID)
        {
            string result = "";
            try
            {
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);

                    PatientDiagnosis diagnose = BusinessLayer.GetPatientDiagnosisList(string.Format("VisitID = {0} AND IsDeleted = 0 AND GCDiagnoseType = '{1}'", oVisit.VisitID, Constant.DiagnoseType.MAIN_DIAGNOSIS)).FirstOrDefault();

                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        List<SettingParameterDt> lstParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}', '{2}') ", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER, Constant.SettingParameter.IS0026));

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "";
                        string catatanKlinis = "";

                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            catatanKlinis = oOrderHd.Remarks;
                            //remarks = oOrderHd.Remarks;
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                            string orderStatus = "NW";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            //if (!string.IsNullOrEmpty(item.Remarks))
                            //{
                            //    if (!string.IsNullOrEmpty(oHeader.Remarks))
                            //        remarks = string.Format("{0}{1}{2}", oHeader.Remarks, ";", item.Remarks);
                            //    else
                            //        remarks += string.Format("{0}{1}", ";", item.Remarks);
                            //}
                            //else
                            //{
                            //    if (!string.IsNullOrEmpty(oHeader.Remarks))
                            //    {
                            //        remarks = oHeader.Remarks;
                            //    }
                            //}

                            SettingParameterDt isRemaksSend = lstParam.Where(p => p.ParameterCode == Constant.SettingParameter.IS0026).FirstOrDefault();
                            if (isRemaksSend.ParameterValue == "1")
                            {
                                if (diagnose != null)
                                {
                                    remarks = diagnose.DiagnosisText;
                                }
                            }
                            else
                            {
                                remarks = catatanKlinis;
                            }

                            if (remarks.Length >= 300)
                                remarks = remarks.Substring(0, 300);

                            remarks = Regex.Replace(remarks, @"\r\n?|\n", ";");
                            remarks = remarks.Replace(System.Environment.NewLine, ";");

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_INFINITT_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_INFINITT_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "AL");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string patientName = oVisit.PatientName;
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}", orderParamedicCode, orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, oVisit.RoomCode);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);
                            pv1.Field(20, string.Empty);
                            pv1.Field(21, string.Empty);
                            pv1.Field(22, string.Empty);
                            pv1.Field(23, string.Empty);
                            pv1.Field(24, string.Empty);
                            pv1.Field(25, string.Empty);
                            pv1.Field(26, string.Empty);
                            pv1.Field(27, string.Empty);
                            pv1.Field(28, string.Empty);
                            pv1.Field(29, string.Empty);
                            pv1.Field(30, string.Empty);
                            pv1.Field(31, string.Empty);
                            pv1.Field(32, string.Empty);
                            pv1.Field(33, string.Empty);
                            pv1.Field(34, string.Empty);
                            pv1.Field(35, string.Empty);
                            pv1.Field(36, string.Empty);
                            pv1.Field(37, string.Empty);
                            pv1.Field(38, string.Empty);
                            pv1.Field(39, string.Empty);
                            pv1.Field(40, string.Empty);
                            pv1.Field(41, string.Empty);
                            pv1.Field(42, string.Empty);
                            pv1.Field(43, string.Empty);
                            pv1.Field(44, orderDateTime);
                            pv1.Field(45, string.Empty);
                            pv1.Field(46, string.Empty);
                            pv1.Field(47, string.Empty);
                            pv1.Field(48, string.Empty);
                            pv1.Field(49, string.Empty);
                            pv1.Field(50, string.Empty);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);


                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, string.Empty);
                            orc.Field(5, "a");
                            orc.Field(6, string.Empty);
                            orc.Field(7, orderDateTime);
                            orc.Field(8, string.Empty);
                            orc.Field(9, string.Empty);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, orderPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, oVisit.DepartmentID);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Format("{0}^{1}^^^^^", AppSession.UserLogin.UserName, AppSession.UserLogin.UserFullName));

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, item.ItemName1));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, string.Empty);
                            obr.Field(11, string.Empty);
                            obr.Field(12, string.Empty);
                            obr.Field(13, remarks);
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, orderPhysicianName);
                            obr.Field(17, string.Empty);
                            obr.Field(18, risOrderNo);
                            obr.Field(19, accessionNo);
                            obr.Field(20, string.Format("{0}", transactionNo));
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, string.Empty);
                            obr.Field(24, modality);
                            obr.Field(25, string.Empty);
                            obr.Field(26, string.Empty);
                            obr.Field(27, string.Format("^^^{0}", orderDateTime));
                            obr.Field(28, string.Empty);
                            obr.Field(29, string.Empty);
                            obr.Field(30, string.Empty);
                            obr.Field(31, string.Format("{0}", oVisit.RegistrationNo));
                            obr.Field(32, "^^^^^^");
                            obr.Field(33, "^^^^^^");
                            obr.Field(34, string.Empty);
                            obr.Field(35, "^^^^^^");
                            obr.Field(36, string.Empty);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}^^^{1}", item.AlternateItemName, item.ItemCode));

                            hl7Message.Add(obr);
                            #endregion

                            #region ZDS
                            HL7Segment zds = new HL7Segment();
                            zds.Field(0, "ZDS");
                            zds.Field(1, string.Empty);
                            zds.Field(2, string.Empty);
                            zds.Field(3, string.Empty);
                            zds.Field(4, string.Empty);

                            hl7Message.Add(zds);
                            #endregion

                            #region Send To RIS Broker Service
                            SettingParameterDt oParam = lstParam.Where(p => p.ParameterCode == Constant.SettingParameter.IS_RIS_HL7_BROKER).FirstOrDefault();
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.Infinitt, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendZedHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oVisit.MRN)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        string remarksHd = string.Empty;
                        string remarksDt = string.Empty;
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;

                            //if (oOrderHd.Remarks.Contains("\n") || oOrderHd.Remarks.Contains("\r") || oOrderHd.Remarks.Contains("\t"))
                            //{
                            //    remarks = oOrderHd.Remarks.Replace("\n", " ");
                            //    remarks = remarks.Replace("\r", " ");
                            //    remarks = remarks.Replace("\t", " ");
                            //}
                            //else
                            //{
                            //    remarks = oOrderHd.Remarks;
                            //}
                            remarks = Helper.ReplaceLineBreak(oOrderHd.Remarks);
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                            string orderStatus = "IP";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            //else
                            //{
                            //    if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                            //    {
                            //        orderStatus = "XO";
                            //    }
                            //}

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = string.Format("{0} {1}", Helper.ReplaceLineBreak(oHeader.Remarks), Helper.ReplaceLineBreak(item.Remarks));
                                }
                                else
                                {
                                    remarks = Helper.ReplaceLineBreak(item.Remarks);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = Helper.ReplaceLineBreak(oHeader.Remarks);
                                }
                            }

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_ZED_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_ZED_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "ER");
                            msh.Field(16, "ER");
                            msh.Field(17, string.Empty);
                            msh.Field(18, "8859/1");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string patientName = string.Empty;
                            string newFirstName = string.Empty;
                            string newLastName = string.Empty;
                            if (string.IsNullOrEmpty(oVisit.FirstName))
                            {
                                string[] nameSplit = new string[0];

                                if (oVisit.LastName.Contains(" "))
                                {
                                    nameSplit = oVisit.LastName.Split(' ');
                                    if (nameSplit.Count() > 1)
                                    {
                                        newFirstName = nameSplit[0];
                                        for (int i = 1; i < nameSplit.Count(); i++)
                                        {
                                            if (string.IsNullOrEmpty(newLastName))
                                            {
                                                newLastName = nameSplit[i];
                                            }
                                            else
                                            {
                                                newLastName += string.Format(" {0}", nameSplit[i]);
                                            }
                                        }
                                        //patientName = string.Format("{0} {1} {2} {3}^^^^^^", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                        patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                    }
                                    else
                                    {
                                        patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                    }
                                }
                                else
                                {
                                    patientName = string.Format("{0} {3}^{1}^{2}", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                                }
                            }
                            else
                            {
                                patientName = string.Format("{0} {3}^{1}^{2}", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            }
                            if (patientName.Contains(','))
                            {
                                patientName = patientName.Replace(",", " ");
                            }
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string patientMobileNo = string.IsNullOrEmpty(oVisit.MobilePhoneNo1) ? string.Empty : oVisit.MobilePhoneNo1.Trim();
                            string patientEmailAddress = string.IsNullOrEmpty(entityPatient.EmailAddress) ? string.Empty : entityPatient.EmailAddress;
                            string field13 = string.Format("^^^{0}^^^{1}", patientEmailAddress, patientMobileNo);

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, "Y");
                            pid.Field(10, string.Empty);
                            pid.Field(11, Helper.ReplaceLineBreak(patientAddress));
                            pid.Field(12, string.Empty);
                            pid.Field(13, field13);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}", orderParamedicCode, orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, risOrderNo);
                            orc.Field(5, orderStatus);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, requestedPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, "NHS"); //Site code
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
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

                            #region NTE
                            HL7Segment nte = new HL7Segment();
                            nte.Field(0, "NTE");
                            nte.Field(1, string.Empty);
                            nte.Field(2, string.Empty);
                            nte.Field(3, remarks.Replace(System.Environment.NewLine, " "));

                            hl7Message.Add(nte);
                            #endregion

                            #region Send To RIS Broker Service
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.ZED, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendMedsynapticHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();

                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, vitalweight, port = string.Empty;
                        List<SettingParameterDt> oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER));
                        ipaddress = oParam.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_HL7_BROKER).FirstOrDefault().ParameterValue;
                        List<SettingParameter> oParamHd = BusinessLayer.GetSettingParameterList(string.Format("ParameterCode ='{0}'", Constant.SettingParameter.VITAL_SIGN_WEIGHT));
                        vitalweight = oParamHd.Where(w => w.ParameterCode == Constant.SettingParameter.VITAL_SIGN_WEIGHT).FirstOrDefault().ParameterValue;
                        vVitalSignDt oVitalSign = BusinessLayer.GetvVitalSignDtList(string.Format("VisitID = {0}", oVisit.VisitID)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            remarks = oOrderHd.Remarks;
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID.Substring(2));
                            string orderStatus = "NW";

                            string modalityAETitle = string.Empty;
                            if (!string.IsNullOrEmpty(item.GCModality))
                            {
                                Modality entityModality = BusinessLayer.GetModalityList(string.Format("ModalityID = '{0}' AND GCModality = '{1}'", item.ModalityID, item.GCModality)).FirstOrDefault();
                                if (entityModality != null)
                                {
                                    modalityAETitle = entityModality.AETitle;
                                }
                            }

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                    remarks = string.Format("{0}{1}{2}", oHeader.Remarks, Environment.NewLine, item.Remarks);
                                else
                                    remarks = item.Remarks;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = oHeader.Remarks;
                                }
                            }

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "STAT" : "ROUTINE";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            string requestedReferrerPhysicianName = string.Format("^{0}", item.ParamedicName);                            

                            //ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            //if (oParamedic != null)
                            //{
                            //    requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            //}

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_MEDSYNAPTIC_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_MEDSYNAPTIC_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_MEDSYNAPTIC_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, "ER");
                            msh.Field(16, "ER");
                            msh.Field(17, string.Empty);
                            msh.Field(18, "8859/1");

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            //string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^^", oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);
                            pid.Field(14, string.Empty);
                            pid.Field(15, string.Empty);
                            pid.Field(16, string.Empty);
                            pid.Field(17, string.Empty);
                            pid.Field(18, oVisit.RegistrationNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}^^^", orderParamedicCode, orderParamedicName);
                            
                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);
                            pv1.Field(20, vitalweight);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);
                            string businesspartner = string.Format("^{0}", oVisit.BusinessPartnerName);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, risOrderNo);
                            orc.Field(5, string.Empty);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12,string.Format("^{0}",item.ParamedicName)); 
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, businesspartner);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, accessionNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, item.AlternateItemName));
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
                            obr.Field(15, item.AlternateItemName);
                            obr.Field(16, string.Empty);
                            obr.Field(17, accessionNo);
                            obr.Field(18, modality);
                            obr.Field(19, item.ItemCode);
                            obr.Field(20, string.Empty);
                            obr.Field(21, modality);
                            obr.Field(22, string.Empty);
                            obr.Field(23, string.Empty);
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
                            obr.Field(34, item.ParamedicName);
                            obr.Field(35, string.Empty);
                            obr.Field(36, orderDateTime);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}^^^{1}", item.ItemCode, item.AlternateItemName));

                            hl7Message.Add(obr);
                            #endregion

                            #region ZDS
                            HL7Segment zds = new HL7Segment();
                            zds.Field(0, "ZDS");
                            zds.Field(1, string.Empty);
                            zds.Field(2, string.Empty);
                            zds.Field(3, string.Empty);
                            zds.Field(4, string.Empty);

                            hl7Message.Add(zds);
                            #endregion

                            #region Send To RIS Broker Service
                            if (ipaddress != null)
                            {
                                string[] paramInfo = ipaddress.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    UpdateErrorLogMessage(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, msgText, ex.Message, item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                UpdateErrorLogMessage(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, string.Empty, "Invalid Configuration for RIS HL7 Broker IP Address", item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                            {
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                            }
                        else
                        {
                            result = string.Format("{0}|{1}", "1", string.Empty);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendLifeTrackHL7OrderToRIS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();

                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        List<SettingParameterDt> oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}')", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER, Constant.SettingParameter.VITAL_SIGN_WEIGHT));
                        ipaddress = oParam.Where(w => w.ParameterCode == Constant.SettingParameter.IS_RIS_HL7_BROKER).FirstOrDefault().ParameterValue;

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                            remarks = oOrderHd.Remarks;
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112_2), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL_2).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID.Substring(2));
                            string orderStatus = "NW";

                            string modalityAETitle = string.Empty;
                            if (!string.IsNullOrEmpty(item.GCModality))
                            {
                                Modality entityModality = BusinessLayer.GetModalityList(string.Format("ModalityID = '{0}' AND GCModality = '{1}'", item.ModalityID, item.GCModality)).FirstOrDefault();
                                if (entityModality != null)
                                {
                                    modalityAETitle = entityModality.AETitle;
                                }
                            }

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            else
                            {
                                if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                                {
                                    orderStatus = "XO";
                                }
                            }

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks) && oHeader.Remarks.TrimEnd() != item.Remarks.TrimEnd())
                                    remarks = string.Format("{0}{1}{2}", oHeader.Remarks, Environment.NewLine, item.Remarks);
                                else
                                    remarks = item.Remarks;
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = oHeader.Remarks;
                                }
                            }

                            remarks = remarks.Replace("\r\n", ";").Replace("\n", ";").Replace("\r", ";");

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "STAT" : "ROUTINE";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            string requestedReferrerPhysicianName = string.Format("^{0}", item.ParamedicName);

                            string itemName = !string.IsNullOrEmpty(item.AlternateItemName) ? item.AlternateItemName : item.ItemName1;

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_LIFETRACK_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_LIFETRACK_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_LIFETRACK_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);
                            msh.Field(15, string.Empty);
                            msh.Field(16, string.Empty);
                            msh.Field(17, string.Empty);
                            msh.Field(18, string.Empty);

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName.Trim(), oVisit.FirstName.Trim(), oVisit.MiddleName.Trim(), oVisit.Salutation.Trim());
                            string patientName = string.Format("{0}^^^^^^", oVisit.PatientName);
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            //string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^^", oVisit.City.TrimEnd());
                            string phoneNo = oVisit.PhoneNo1 == null ? string.Empty : oVisit.PhoneNo1.Trim();

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, "1");
                            pid.Field(2, oVisit.MedicalNo);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, string.Empty);
                            pid.Field(10, string.Empty);
                            pid.Field(11, patientAddress);
                            pid.Field(12, string.Empty);
                            pid.Field(13, phoneNo);
                            pid.Field(14, string.Empty);
                            pid.Field(15, string.Empty);
                            pid.Field(16, string.Empty);
                            pid.Field(17, string.Empty);
                            pid.Field(18, oVisit.RegistrationNo);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}^^^", orderParamedicCode, orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty); //Pregnancy Status, set to B6 is the patient is pregnant
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);
                            string businesspartner = string.Format("^{0}", oVisit.BusinessPartnerName);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            orc.Field(2, risOrderNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, string.Empty);
                            orc.Field(5, string.Empty);
                            orc.Field(6, string.Empty);
                            orc.Field(7, string.Format("{0}^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, string.Format("{0}", orderParamedicName));
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, businesspartner);
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, "1");
                            obr.Field(2, item.TransactionNo);
                            obr.Field(3, accessionNo);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, itemName));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, string.Empty);
                            obr.Field(11, string.Empty);
                            obr.Field(12, string.Empty);
                            obr.Field(13, remarks); //Clinical Information
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, string.Empty);
                            obr.Field(17, string.Empty);
                            obr.Field(18, accessionNo);
                            obr.Field(19, item.ModalityID.ToString()); //numeric value for the modality
                            obr.Field(20, string.Empty);
                            obr.Field(21, string.Empty);
                            obr.Field(22, string.Empty);
                            obr.Field(23, string.Empty);
                            obr.Field(24, modality);
                            obr.Field(25, string.Empty);
                            obr.Field(26, string.Empty);
                            obr.Field(27, string.Empty);
                            obr.Field(28, string.Empty);
                            obr.Field(29, string.Empty);
                            obr.Field(30, string.Empty);
                            obr.Field(31, transactionNo);
                            obr.Field(32, string.Empty);
                            obr.Field(33, string.Empty);
                            obr.Field(34, item.ParamedicName);
                            obr.Field(35, string.Empty);
                            obr.Field(36, string.Empty);
                            obr.Field(37, string.Empty);
                            obr.Field(38, string.Empty);
                            obr.Field(39, string.Empty);
                            obr.Field(40, string.Empty);
                            obr.Field(41, string.Empty);
                            obr.Field(42, string.Empty);
                            obr.Field(43, string.Format("{0}^{1}", item.ItemCode, itemName));
                            obr.Field(44, string.Empty);

                            hl7Message.Add(obr);
                            #endregion

                            #region Send To RIS Broker Service
                            if (ipaddress != null)
                            {
                                string[] paramInfo = ipaddress.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    UpdateErrorLogMessage(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, msgText, ex.Message, item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                UpdateErrorLogMessage(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.MEDSYNAPTIC, messageCode, string.Empty, "Invalid Configuration for RIS HL7 Broker IP Address", item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                            {
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                            }
                        else
                        {
                            result = string.Format("{0}|{1}", "1", string.Empty);
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendJiveXHL7OrderToPACS(int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                string messageCode = "ORM^O01";

                #region Convert into DTO Objects
                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    int testOrderID = oHeader.TestOrderID;
                    bool isfromOrder = testOrderID > 0;

                    vConsultVisit10 oVisit = BusinessLayer.GetvConsultVisit10List(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oVisit.MRN)).FirstOrDefault();
                    filterExpression += string.Format(" AND ID IN ({0})", hdnSelectedID.Value);
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.IS_RIS_HL7_BROKER)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOLOGY";
                        string remarksHd = string.Empty;
                        string remarksDt = string.Empty;
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;

                            //if (oOrderHd.Remarks.Contains("\n") || oOrderHd.Remarks.Contains("\r") || oOrderHd.Remarks.Contains("\t"))
                            //{
                            //    remarks = oOrderHd.Remarks.Replace("\n", " ");
                            //    remarks = remarks.Replace("\r", " ");
                            //    remarks = remarks.Replace("\t", " ");
                            //}
                            //else
                            //{
                            //    remarks = oOrderHd.Remarks;
                            //}
                            remarks = Helper.ReplaceLineBreak(oOrderHd.Remarks);
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
                        foreach (vPatientChargesDt item in oList)
                        {
                            HL7MessageText hl7Message = new HL7MessageText();
                            string detailID = item.ID.ToString();
                            string messageControlID = string.Format("{0}{1}{2}", DateTime.Now.Date.ToString(Constant.FormatString.DATE_FORMAT_112), DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT_FULL).Replace(":", "").Replace(".", ""), sequenceNo);
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            string accessionNo = string.Format("{0}{1}", modality, messageControlID);
                            string orderStatus = "IP";

                            if (item.IsDeleted)
                            {
                                orderStatus = "CA";
                            }
                            //else
                            //{
                            //    if (item.GCRISBridgingStatus == Constant.RIS_Bridging_Status.SENT)
                            //    {
                            //        orderStatus = "XO";
                            //    }
                            //}

                            if (!string.IsNullOrEmpty(item.Remarks))
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = string.Format("{0} {1}", Helper.ReplaceLineBreak(oHeader.Remarks), Helper.ReplaceLineBreak(item.Remarks));
                                }
                                else
                                {
                                    remarks = Helper.ReplaceLineBreak(item.Remarks);
                                }
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(oHeader.Remarks))
                                {
                                    remarks = Helper.ReplaceLineBreak(oHeader.Remarks);
                                }
                            }

                            if (remarks.Length >= 500)
                                remarks = remarks.Substring(0, 500);

                            string priority = item.IsCITO ? "T" : "R";
                            string diagnoseName = item.DiagnoseTestOrder == null ? "-" : item.DiagnoseTestOrder.Trim();

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}", oParamedic.FullName);
                            }

                            #region MSH
                            HL7Segment msh = new HL7Segment();
                            msh.Field(0, "MSH");
                            msh.Field(1, ""); //will be ignored
                            msh.Field(2, @"^~\&");
                            msh.Field(3, "MEDINFRAS-API_RIS");
                            msh.Field(4, AppSession.UserLogin.HealthcareID);
                            msh.Field(5, CommonConstant.HL7_JIVEX_MSG.IDENTIFICATION_1);
                            msh.Field(6, CommonConstant.HL7_JIVEX_MSG.IDENTIFICATION_2);
                            msh.Field(7, orderDateTime);
                            msh.Field(8, string.Empty);
                            msh.Field(9, messageCode);
                            msh.Field(10, messageControlID);
                            msh.Field(11, "P");
                            msh.Field(12, CommonConstant.HL7_JIVEX_MSG.HL7_VERSION);
                            msh.Field(13, string.Empty);
                            msh.Field(14, string.Empty);

                            hl7Message.Add(msh);
                            #endregion

                            #region PID
                            //string patientName = string.Format("{2} {0} {3}^{1}^^^^^^", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            string patientName = string.Empty;
                            string newFirstName = string.Empty;
                            string newLastName = string.Empty;
                            if (string.IsNullOrEmpty(oVisit.FirstName))
                            {
                                string[] nameSplit = new string[0];

                                if (oVisit.LastName.Contains(" "))
                                {
                                    nameSplit = oVisit.LastName.Split(' ');
                                    if (nameSplit.Count() > 1)
                                    {
                                        newFirstName = nameSplit[0];
                                        for (int i = 1; i < nameSplit.Count(); i++)
                                        {
                                            if (string.IsNullOrEmpty(newLastName))
                                            {
                                                newLastName = nameSplit[i];
                                            }
                                            else
                                            {
                                                newLastName += string.Format(" {0}", nameSplit[i]);
                                            }
                                        }
                                        //patientName = string.Format("{0} {1} {2} {3}^^^^^^", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                        patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                    }
                                    else
                                    {
                                        patientName = string.Format("{0} {3}^{1}^{2}", newLastName, newFirstName, oVisit.MiddleName, oVisit.Salutation);
                                    }
                                }
                                else
                                {
                                    patientName = string.Format("{0} {3}^{1}^{2}", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                                }
                            }
                            else
                            {
                                patientName = string.Format("{0} {3}^{1}^{2}", oVisit.LastName, oVisit.FirstName, oVisit.MiddleName, oVisit.Salutation);
                            }
                            if (patientName.Contains(','))
                            {
                                patientName = patientName.Replace(",", " ");
                            }
                            string dateofBirth = string.Format("{0}", oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112).Trim());
                            string gender = oVisit.GenderCodeSuffix;
                            string patientAddress = oVisit.HomeAddress == null ? string.Empty : string.Format("{0}^^{1}^", oVisit.StreetName.Replace("\n", " ").Replace("\t", " ").Replace(Environment.NewLine, " ").TrimEnd(), oVisit.City.TrimEnd());
                            string patientMobileNo = string.IsNullOrEmpty(oVisit.MobilePhoneNo1) ? string.Empty : oVisit.MobilePhoneNo1.Trim();
                            string patientEmailAddress = string.IsNullOrEmpty(entityPatient.EmailAddress) ? string.Empty : entityPatient.EmailAddress;
                            string field13 = string.Format("^^^{0}^^^{1}", patientEmailAddress, patientMobileNo);

                            HL7Segment pid = new HL7Segment();
                            pid.Field(0, "PID");
                            pid.Field(1, string.Empty);
                            pid.Field(2, string.Empty);
                            pid.Field(3, oVisit.MedicalNo);
                            pid.Field(4, string.Empty);
                            pid.Field(5, patientName);
                            pid.Field(6, string.Empty);
                            pid.Field(7, dateofBirth);
                            pid.Field(8, gender);
                            pid.Field(9, "Y");
                            pid.Field(10, string.Empty);
                            pid.Field(11, Helper.ReplaceLineBreak(patientAddress));
                            pid.Field(12, string.Empty);
                            pid.Field(13, field13);

                            hl7Message.Add(pid);
                            #endregion

                            #region PV1
                            string pv1_Param1 = "I";
                            string serviceUnitName = oVisit.ServiceUnitName == null ? string.Empty : oVisit.ServiceUnitName.Trim();
                            string bedCode = oVisit.BedCode;
                            string patientLocation = string.Format("{0}{1}", serviceUnitName, !string.IsNullOrEmpty(bedCode) ? " - " + bedCode : string.Empty);

                            switch (oVisit.DepartmentID)
                            {
                                case Constant.Facility.INPATIENT:
                                    pv1_Param1 = "I";
                                    break;
                                case Constant.Facility.EMERGENCY:
                                    pv1_Param1 = "E";
                                    break;
                                default:
                                    pv1_Param1 = "O";
                                    break;
                            }

                            int noOfItem = oList.Count;
                            //string requestedPhysicianName = noOfItem > 1 ?
                            string orderPhysicianName = string.Format("{0}^{1}", orderParamedicCode, orderParamedicName);

                            HL7Segment pv1 = new HL7Segment();
                            pv1.Field(0, "PV1");
                            pv1.Field(1, string.Empty);
                            pv1.Field(2, pv1_Param1);
                            pv1.Field(3, patientLocation);
                            pv1.Field(4, string.Empty);
                            pv1.Field(5, string.Empty);
                            pv1.Field(6, string.Empty);
                            pv1.Field(7, string.Empty);
                            pv1.Field(8, orderPhysicianName);
                            pv1.Field(9, string.Empty);
                            pv1.Field(10, string.Empty);
                            pv1.Field(11, string.Empty);
                            pv1.Field(12, string.Empty);
                            pv1.Field(13, string.Empty);
                            pv1.Field(14, string.Empty);
                            pv1.Field(15, string.Empty);
                            pv1.Field(16, string.Empty);
                            pv1.Field(17, string.Empty);
                            pv1.Field(18, string.Empty);
                            pv1.Field(19, oVisit.RegistrationNo);

                            hl7Message.Add(pv1);
                            #endregion


                            //Alternate Order Number for RIS purpose
                            string risOrderNo = string.Format("{0}", detailID);

                            #region ORC
                            HL7Segment orc = new HL7Segment();
                            orc.Field(0, "ORC");
                            orc.Field(1, orderStatus);
                            //orc.Field(2, risOrderNo);
                            orc.Field(2, accessionNo);
                            orc.Field(3, string.Empty);
                            orc.Field(4, risOrderNo);
                            orc.Field(5, orderStatus);
                            orc.Field(6, string.Empty);
                            //orc.Field(7, string.Format("{0}^^5^^^{1}", item.ChargedQuantity.ToString(), priority));
                            orc.Field(7, orderDateTime);
                            orc.Field(8, string.Empty);
                            orc.Field(9, orderDateTime);
                            orc.Field(10, string.Empty);
                            orc.Field(11, string.Empty);
                            orc.Field(12, requestedPhysicianName);
                            orc.Field(13, string.Empty);
                            orc.Field(14, string.Empty);
                            orc.Field(15, string.Empty);
                            orc.Field(16, string.Empty);
                            orc.Field(17, string.Empty); //Site code
                            orc.Field(18, string.Empty);
                            orc.Field(19, string.Empty);
                            orc.Field(20, string.Empty);
                            orc.Field(21, string.Empty);

                            hl7Message.Add(orc);
                            #endregion

                            #region OBR
                            HL7Segment obr = new HL7Segment();
                            obr.Field(0, "OBR");
                            obr.Field(1, string.Empty);
                            obr.Field(2, risOrderNo);
                            obr.Field(3, string.Empty);
                            obr.Field(4, string.Format("{0}^{1}", item.ItemCode, !string.IsNullOrEmpty(item.AlternateItemName) ? item.AlternateItemName : item.ItemName1));
                            obr.Field(5, priority);
                            obr.Field(6, orderDateTime);
                            obr.Field(7, string.Empty);
                            obr.Field(8, string.Empty);
                            obr.Field(9, string.Empty);
                            obr.Field(10, "A");
                            obr.Field(11, modality);
                            obr.Field(12, string.Empty);
                            obr.Field(13, string.Empty);
                            obr.Field(14, string.Empty);
                            obr.Field(15, string.Empty);
                            obr.Field(16, string.Empty);
                            obr.Field(17, accessionNo);
                            obr.Field(18, accessionNo);
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

                            #region NTE
                            HL7Segment nte = new HL7Segment();
                            nte.Field(0, "NTE");
                            nte.Field(1, string.Empty);
                            nte.Field(2, string.Empty);
                            nte.Field(3, remarks.Replace(System.Environment.NewLine, " "));

                            hl7Message.Add(nte);
                            #endregion

                            #region Send To RIS Broker Service
                            if (oParam != null)
                            {
                                string[] paramInfo = oParam.ParameterValue.Split(':');
                                ipaddress = paramInfo[0];
                                port = !string.IsNullOrEmpty(paramInfo[1]) ? paramInfo[1] : "6000";

                                string msgText = (char)0x0B + hl7Message.Serialize() + (char)0x1C + (char)0x0D;
                                result = CommonMethods.SendMessageToListener(ipaddress, port, msgText);
                                string[] resultInfo = result.Split('|');
                                bool isSuccess = resultInfo[0] == "1";
                                if (!isSuccess)
                                    errorNo += 1;

                                #region Update Order Status and Log HL7 Message
                                try
                                {
                                    UpdateOrderStatus(Constant.HL7_Partner.MEDINFRAS, Constant.HL7_MessageType.ZED, messageCode, msgText, isSuccess, resultInfo[2], item, string.Empty, string.Format("{0}:{1}", ipaddress, port));
                                }
                                catch (Exception ex)
                                {
                                    result = string.Format("{0}|{1} ({2})", "0", "An error occured when update order status", ex.Message);
                                    break;
                                }
                                #endregion
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", "Invalid Configuration for RIS HL7 Broker IP Address");
                                break;
                            }
                            #endregion

                            sequenceNo += 1;
                        }
                        if (errorNo > 0)
                            if (errorNo < oList.Count)
                                result = string.Format("{0}|{1}", "1", "There are {0} item(s) is rejected by the RIS");
                            else
                                result = string.Format("{0}|{1}", "0", "The order is rejected by the RIS..Please check the log message");
                        else
                            result = string.Format("{0}|{1}", "1", string.Empty);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "There is no order to be sent to RIS");
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

        private object SendToMedinfrasAPI(int transactionID, string lstItemID)
        {
            string result = "1|";

            try
            {
                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };
                RISService oService = new RISService();
                //string apiResult = oService.ADT_A01(AppSession.UserLogin.HealthcareID, entity, visitInfo, patientInfo);
                string apiResult = oService.SendOrderToRIS_MedinfrasApi(transactionID, lstItemID);
                string[] apiResultInfo = apiResult.Split('|');
                if (apiResultInfo[0] == "0")
                {
                    entityAPILog.IsSuccess = false;
                    entityAPILog.MessageText = apiResultInfo[1];
                    entityAPILog.Response = apiResult;
                    entityAPILog.ErrorMessage = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    result = string.Format("{0}|{1}", "0", apiResultInfo[1]);

                    Exception ex = new Exception(apiResultInfo[1]);
                    Helper.InsertErrorLog(ex);
                }
                else
                {
                    entityAPILog.MessageText = apiResultInfo[1];
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);
                }
            }
            catch (Exception ex)
            {
                Helper.InsertErrorLog(ex);
                result = string.Format("{0}|{1}", "0", ex.Message);
            }

            return result;
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
    }
}