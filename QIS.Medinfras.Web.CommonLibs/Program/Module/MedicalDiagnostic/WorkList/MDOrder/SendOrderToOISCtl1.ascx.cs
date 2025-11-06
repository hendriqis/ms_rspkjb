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
    public partial class SendOrderToOISCtl1 : BaseProcessPopupCtl
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
                                                                        "HealthcareID = '{0}' AND ParameterCode IN ('{1}','{2}','{3}','{4}')",
                                                                        AppSession.UserLogin.HealthcareID, //0
                                                                        Constant.SettingParameter.RT0003, //1
                                                                        Constant.SettingParameter.RT0004, //2
                                                                        Constant.SettingParameter.RT0005, //3
                                                                        Constant.SettingParameter.RT0006  //4
                                                                    ));

                string isBridgingToOIS = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.RT0003).FirstOrDefault().ParameterValue;
                string oisBridgingProtocol = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.RT0004).FirstOrDefault().ParameterValue;
                string hl7MessageFormat = lstParameter.Where(w => w.ParameterCode == Constant.SettingParameter.RT0005).FirstOrDefault().ParameterValue;

                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                if (isBridgingToOIS == "1")
                {
                    string[] resultInfo = "0|Unknown Protocol".Split('|');
                    switch (oisBridgingProtocol)
                    {
                        case Constant.OIS_Bridging_Protocol.HL7:
                            switch (hl7MessageFormat)
                            {
                                case Constant.OIS_HL7MessageFormat.ARIA:
                                    var medavisResult = SendOrderToOIS_Aria(transactionID);
                                    resultInfo = ((string)medavisResult).Split('|');
                                    break;
                                default:
                                    resultInfo = "0|Unknown Protocol".Split('|');
                                    break;
                            }
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

        private object SendOrderToOIS_Aria(int transactionID)
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
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    if (oList.Count > 0)
                    {
                        string ipaddress, port = string.Empty;
                        SettingParameterDt oParam = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '{0}' AND ParameterCode ='{1}'", AppSession.UserLogin.HealthcareID, Constant.SettingParameter.RT0006)).FirstOrDefault();

                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string transactionNo = oHeader.TransactionNo;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode.Trim();
                        string orderParamedicName = oVisit.ParamedicName.Trim();
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        string remarks = "RADIOTERAPHY";
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