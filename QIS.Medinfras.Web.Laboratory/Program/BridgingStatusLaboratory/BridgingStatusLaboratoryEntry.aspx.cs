using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.CommonLibs.Controls;
using QIS.Medinfras.Web.CommonLibs.Program;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Medinfras.Web.Common.API.Model;
using Newtonsoft.Json;
using System.Net;
using System.IO;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class BridgingStatusLaboratoryEntry : BasePageTrx
    {
        protected string transactionCode = "0";

        protected string GetErrorMsgSelectTransactionFirst()
        {
            return Helper.GetErrorMessageText(this, Constant.ErrorMessage.MSG_SELECT_TRANSACTION_FIRST_VALIDATION);
        }

        public override string OnGetMenuCode()
        {
            switch (hdnDepartmentID.Value)
            {
                case Constant.Facility.DIAGNOSTIC:
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        return Constant.MenuCode.Laboratory.BRIDGING_STATUS;
                    }
                    return Constant.MenuCode.Laboratory.BRIDGING_STATUS;
                default: return Constant.MenuCode.Laboratory.BRIDGING_STATUS;

            }
        }

        protected string GetPageTitle()
        {
            return hdnPageTitle.Value;
        }

        protected override void InitializeDataControl()
        {
            if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
            {
                transactionCode = String.Format("{0},{1}", Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES, Constant.TransactionCode.LABORATORY_CHARGES);
                hdnTrxCode.Value = String.Format("{0},{1}", Constant.TransactionCode.OTHER_DIAGNOSTIC_CHARGES, Constant.TransactionCode.LABORATORY_CHARGES);
            }

            if (Page.Request.QueryString.Count > 0)
            {
                string[] param = Request.QueryString["id"].Split('|');
                hdnVisitID.Value = param[1];
                hdnPageTitle.Value = BusinessLayer.GetMenuMasterList(string.Format("MenuCode = '{0}'", OnGetMenuCode())).FirstOrDefault().MenuCaption;

                SettingParameterDt setvar = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_KODE_PENUNJANG_LABORATORIUM);

                vHealthcareServiceUnit hsu = BusinessLayer.GetvHealthcareServiceUnitList(string.Format("HealthcareServiceUnitID = {0}", setvar.ParameterValue))[0];
                hdnDepartmentID.Value = hsu.DepartmentID;
                hdnHealthcareServiceUnitID.Value = hsu.HealthcareServiceUnitID.ToString();

                vConsultVisit2 entity = BusinessLayer.GetvConsultVisit2List(string.Format("VisitID = {0}", hdnVisitID.Value))[0];
                hdnLinkedRegistrationID.Value = entity.LinkedRegistrationID.ToString();
                hdnRegistrationID.Value = entity.RegistrationID.ToString();
                EntityToControl(entity);

                BindGridDetail();
            }
        }

        private void BindGridDetail()
        {
            string filterExpression = "";
            if (hdnLinkedRegistrationID.Value != "" && hdnLinkedRegistrationID.Value != "0")
                filterExpression = string.Format("(VisitID = {0} OR (RegistrationID = {1} AND IsChargesTransfered = 1))", hdnVisitID.Value, hdnLinkedRegistrationID.Value);
            else
                filterExpression = string.Format("RegistrationID = {0}", hdnRegistrationID.Value);
            filterExpression += string.Format(" AND GCTransactionStatus = '{0}' AND HealthcareServiceUnitID = {1} AND TransactionCode IN ({2})",
                Constant.TransactionStatus.WAIT_FOR_APPROVAL, hdnHealthcareServiceUnitID.Value, hdnTrxCode.Value);

            filterExpression += " ORDER BY TransactionDate DESC";
            List<vPatientChargesHd> lst = BusinessLayer.GetvPatientChargesHdList(filterExpression);
            lvwView.DataSource = lst;
            lvwView.DataBind();
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            BindGridDetail();
        }

        private void EntityToControl(vConsultVisit2 entity)
        {
            ((PatientBannerCtl)ctlPatientBanner).InitializePatientBanner(entity);
        }

        public override void SetToolbarVisibility(ref bool IsAllowAdd, ref bool IsAllowSave, ref bool IsAllowVoid, ref bool IsAllowNextPrev)
        {
            IsAllowAdd = IsAllowSave = IsAllowVoid = IsAllowNextPrev = false;
        }

        private bool OnCheckSendToLIS(string bridgingStatus, ref string errMessage)
        {
            bool isSent = false;
            if (bridgingStatus != Constant.LIS_Bridging_Status.OPEN && bridgingStatus != string.Empty)
            {
                errMessage = "Maaf, Transaksi ini sudah dikirim ke LIS";
                isSent = true;
            }
            return isSent;
        }

        protected override bool OnCustomButtonClick(string type, ref string errMessage)
        {
            if (type == "resend")
            {
                string[] listParam = hdnParam.Value.Split('|');

                foreach (string param in listParam)
                {
                    //PatientChargesHd chargeHD = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(param));
                    vPatientChargesHd chargeHD = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", param)).FirstOrDefault();

                    bool isProcessCharges = true;
                    string referenceNo = string.Empty;
                    if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Laboratory)
                    {
                        if (AppSession.IsBridgingToLIS)
                        {
                            string[] resultInfo = "0|Unknown Protocol".Split('|');
                            switch (AppSession.LIS_BRIDGING_PROTOCOL)
                            {
                                case Constant.LIS_Bridging_Protocol.WEB_API:
                                    switch (AppSession.LIS_PROVIDER)
                                    {
                                        case Constant.LIS_PROVIDER.HCLAB:
                                            BusinessLayer.SendToLISInterfaceDB(chargeHD.TransactionID);
                                            resultInfo = "1|".Split('|');
                                            break;
                                        case Constant.LIS_PROVIDER.WYNACOM:
                                            if (OnCheckSendToLIS(chargeHD.GCLISBridgingStatus, ref errMessage))
                                            {
                                                var result2 = SendLISOrderToWYNACOM(chargeHD.TransactionID, Convert.ToInt32(chargeHD.TestOrderID));
                                                resultInfo = ((string)result2).Split('|');
                                            }
                                            break;
                                        case Constant.LIS_PROVIDER.ELIMPSE:
                                            if (OnCheckSendToLIS(chargeHD.GCLISBridgingStatus, ref errMessage))
                                            {
                                                var result3 = SendLISOrderToWYNACOM(chargeHD.TransactionID, Convert.ToInt32(chargeHD.TestOrderID));
                                                resultInfo = ((string)result3).Split('|');
                                            }
                                            break;
                                        case Constant.LIS_PROVIDER.SOFTMEDIX:
                                            SendLISOrderToSoftmedix(chargeHD.TransactionID, Convert.ToInt32(chargeHD.TestOrderID));
                                            break;
                                        default:
                                            break;
                                    }
                                    break;
                                default:
                                    resultInfo = "0|Unknown Protocol".Split('|');
                                    break;
                            }
                            referenceNo = resultInfo[1];
                            bool isError = resultInfo[0] == "0";
                            if (isError)
                                errMessage = resultInfo[1];
                        }
                    }

                    //if (isProcessCharges)
                    //{
                    //    Int32 TransactionID = Convert.ToInt32(param);
                    //    PatientChargesHd entity = BusinessLayer.GetPatientChargesHd(TransactionID);
                    //    if (!string.IsNullOrEmpty(referenceNo))
                    //        entity.ReferenceNo = referenceNo;
                    //    //entity.GCTransactionStatus = Constant.TransactionStatus.WAIT_FOR_APPROVAL;
                    //    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    //    BusinessLayer.UpdatePatientChargesHd(entity);
                    //    return true;
                    //}
                    //else
                    //{
                    //    return false;
                    //}

                }
            }
            return true;
        }

        private OrderDTO ConvertOrderToDTO(PatientChargesHd chargesHd, vConsultVisit oVisit)
        {
            OrderDTO oData = new OrderDTO();

            oData.RegistrationID = Convert.ToInt32(hdnRegistrationID.Value);
            oData.RegistrationNo = oVisit.RegistrationNo;
            oData.PatientID = AppSession.RegisteredPatient.MRN;
            oData.PatientInfo = new PatientData() { PatientID = oData.PatientID, MedicalNo = AppSession.RegisteredPatient.MedicalNo, FullName = AppSession.RegisteredPatient.PatientName };
            oData.VisitID = AppSession.RegisteredPatient.VisitID;
            if (oVisit != null)
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = oVisit.VisitID,
                    VisitDate = oVisit.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112),
                    VisitTime = oVisit.VisitTime,
                    PhysicianID = oVisit.ParamedicID,
                    PhysicianCode = oVisit.ParamedicCode,
                    PhysicianName = oVisit.ParamedicName
                };
            }
            else
            {
                oData.VisitInformation = new VisitInfo()
                {
                    VisitID = AppSession.RegisteredPatient.VisitID,
                    VisitDate = AppSession.RegisteredPatient.VisitDate.ToString(Constant.FormatString.DATE_FORMAT_112)
                };
            }
            oData.TransactionNo = chargesHd.TransactionNo;
            oData.TransactionDate = chargesHd.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112);
            oData.TransactionTime = chargesHd.TransactionTime;
            oData.OrderType = "LABORATORY";
            oData.IsCompound = false;

            List<OrderDt> lstOrderDt = new List<OrderDt>();
            List<vPatientChargesDt> lstChargesDt = BusinessLayer.GetvPatientChargesDtList(string.Format("TransactionID = {0} AND IsDeleted = 0", chargesHd.TransactionID));
            foreach (vPatientChargesDt item in lstChargesDt)
            {
                OrderDt orderDt = new OrderDt() { itemCode = item.ItemCode, itemName = item.ItemName1 };
                lstOrderDt.Add(orderDt);
            }
            oData.DetailList = lstOrderDt;
            return oData;
        }

        private object SendLISOrderToWYNACOM(int transactionID, int testOrderID)
        {
            string result = "";
            try
            {
                string url = AppSession.LIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    filterExpression += " AND IsDeleted = 0 ORDER BY ID";
                    List<vPatientChargesDt1> oList = BusinessLayer.GetvPatientChargesDt1List(filterExpression);
                    WynacomOrderDTO oData = new WynacomOrderDTO();
                    if (oList.Count > 0)
                    {
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode;
                        string orderParamedicName = oVisit.ParamedicName;
                        DateTime orderDate = oHeader.TransactionDate;
                        string orderTime = oHeader.TransactionTime;
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : orderParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : orderParamedicName;
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }
                        else
                        {
                            if (oVisit.DepartmentID == Constant.Facility.DIAGNOSTIC)
                            {
                                orderParamedicCode = oVisit.ReferralPhysicianID != null ? oVisit.ReferralPhysicianCode : orderParamedicCode;
                                orderParamedicName = oVisit.ReferralPhysicianID != null ? oVisit.ReferralPhysicianName : orderParamedicName;
                            }
                        }

                        PatientInfo oPatientData = new PatientInfo();
                        oPatientData.PatientID = oVisit.MRN;
                        oPatientData.GuestID = oVisit.GuestID;

                        string isUsedMedicalNo = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.LB_IS_SEND_MEDICALNO_TO_LIS).ParameterValue;
                        if (isUsedMedicalNo == "1")
                        {
                            oPatientData.MedicalNo = oVisit.MedicalNo;
                        }
                        else
                        {
                            oPatientData.MedicalNo = oVisit.OldMedicalNo != "" ? oVisit.OldMedicalNo : oVisit.MedicalNo;
                        }

                        oPatientData.GuestNo = oVisit.GuestNo;
                        oPatientData.FullName = oVisit.PatientName;
                        oPatientData.Salutation = !string.IsNullOrEmpty(oVisit.Salutation) ? oVisit.Salutation : string.Empty;
                        oPatientData.Gender = string.Format("{0}^{1}", oVisit.GCGender, oVisit.Gender);
                        oPatientData.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oPatientData.HomeAddress = oVisit.HomeAddress;
                        oPatientData.HomeZipCode = oVisit.ZipCode;
                        oPatientData.HomePhoneNo1 = oVisit.PhoneNo1;
                        oPatientData.HomePhoneNo2 = oVisit.PhoneNo2;
                        oPatientData.MobileNo1 = oVisit.MobilePhoneNo1;
                        oPatientData.MobileNo2 = oVisit.MobilePhoneNo2;
                        oPatientData.EmailAddress = oVisit.EmailAddress;
                        if (!string.IsNullOrEmpty(oVisit.SSN))
                        {
                            Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", oVisit.MRN)).FirstOrDefault();
                            if (entityPatient != null)
                            {
                                if (entityPatient.GCIdentityNoType == Constant.IdentityCardType.KTP)
                                {
                                    oPatientData.SSN = oVisit.SSN;
                                }
                            }
                        }

                        VisitInfo1 oVisitData = new VisitInfo1();
                        oVisitData.VisitID = oVisit.VisitID;
                        oVisitData.VisitDate = oVisit.VisitDate;
                        oVisitData.VisitTime = oVisit.VisitTime;
                        oVisitData.RegistrationNo = oVisit.RegistrationNo;
                        oVisitData.DepartmentID = oVisit.DepartmentID;
                        oVisitData.DepartmentName = oVisit.DepartmentID;
                        oVisitData.ServiceUnitCode = oVisit.ServiceUnitCode;
                        oVisitData.ServiceUnitName = oVisit.ServiceUnitName;
                        oVisitData.RoomCode = oVisit.RoomCode;
                        oVisitData.RoomName = oVisit.RoomName;
                        oVisitData.BedCode = oVisit.BedCode;
                        oVisitData.BedName = oVisit.BedCode;
                        oVisitData.ClassCode = oVisit.ClassCode;
                        oVisitData.ClassName = oVisit.ClassName;
                        oVisitData.ChargeClassCode = oVisit.ChargeClassCode;
                        oVisitData.ChargeClassName = oVisit.ChargeClassName;
                        oVisitData.PhysicianCode = oVisit.ParamedicCode;
                        oVisitData.PhysicianName = oVisit.ParamedicName;
                        oVisitData.PayerCode = !string.IsNullOrEmpty(oVisit.BusinessPartnerCommCode) ? oVisit.BusinessPartnerCommCode : oVisit.BusinessPartnerCode;
                        oVisitData.PayerName = oVisit.BusinessPartnerName;

                        WynacomOrderHd oHeaderData = new WynacomOrderHd();
                        oHeaderData.TransactionID = oHeader.TransactionID;
                        oHeaderData.TransactionNo = oHeader.TransactionNo;
                        oHeaderData.TransactionDateTime = oHeader.TransactionDate;
                        oHeaderData.OrderPhysicianCode = orderParamedicCode;
                        oHeaderData.OrderPhysicianName = orderParamedicName;

                        List<WynacomOrderDt> lstDetail = new List<WynacomOrderDt>();

                        int numberOfCito = 0;
                        int invalidItem = 0; //Item with no-fraction defined
                        string invalidItemName = string.Empty;
                        foreach (vPatientChargesDt1 item in oList)
                        {
                            if (item.IsCITO)
                                numberOfCito += 1;

                            string filterExp = string.Format("ItemID = {0} ORDER BY DisplayOrder", item.ItemID);
                            List<vItemLaboratoryFraction> itemFractionList = BusinessLayer.GetvItemLaboratoryFractionList(filterExp);
                            if (itemFractionList.Count == 0)
                            {
                                invalidItem += 1;
                                invalidItemName += item.ItemName1 + ";";
                            }

                            foreach (vItemLaboratoryFraction fractionItem in itemFractionList)
                            {
                                WynacomOrderDt oDetailData = new WynacomOrderDt();
                                oDetailData.TransactionID = oHeader.TransactionID;
                                oDetailData.ItemCode = string.IsNullOrEmpty(item.OldItemCode) ? item.ItemCode : item.OldItemCode;
                                oDetailData.ItemName = item.ItemName1;
                                oDetailData.fractionCommCode = fractionItem.CommCode;
                                oDetailData.fractionName = fractionItem.FractionName1;
                                oDetailData.ClinicianRemarks = item.Remarks;

                                lstDetail.Add(oDetailData);
                            }
                        }
                        oHeaderData.IsCITO = numberOfCito > 0;

                        oData.PatientData = oPatientData;
                        oData.VisitData = oVisitData;
                        oData.OrderHeaderData = oHeaderData;
                        oData.OrderDetailList = lstDetail;
                        oData.CreatedByUserID = oHeader.CreatedByUserName;
                        oData.CreatedByUserName = oHeader.CreatedByName;
                        oData.CreatedDate = DateTime.Now;

                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = Constant.BridgingVendor.LIS,
                            Sender = Constant.BridgingVendor.HIS,
                            MessageText = JsonConvert.SerializeObject(oData)
                        };

                        if (invalidItem == 0)
                        {
                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/laboratory/order/insertOrderWynacom/", url));
                            request.Method = "POST";
                            request.ContentType = "application/json";
                            Methods.SetRequestHeader(request, AppSession.LIS_Consumer_ID, AppSession.LIS_Consumer_Pwd);

                            var json = JsonConvert.SerializeObject(oData);
                            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                            {
                                streamWriter.Write(json);
                            }

                            WebResponse response = (WebResponse)request.GetResponse();
                            string responseMsg = string.Empty;
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseMsg = sr.ReadToEnd();
                            };

                            MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                            if (!string.IsNullOrEmpty(respInfo.Data))
                            {
                                result = string.Format("{0}|{1}", "1", respInfo.Remarks);

                                entityAPILog.IsSuccess = true;
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);

                                //UPDATE ORDER STATUS
                                foreach (vPatientChargesDt1 item in oList)
                                {
                                    PatientChargesDtInfo dtInfo = BusinessLayer.GetPatientChargesDtInfo(item.ID);
                                    if (dtInfo != null)
                                    {
                                        dtInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                                        BusinessLayer.UpdatePatientChargesDtInfo(dtInfo);
                                    }
                                }
                                PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(oHeader.TransactionID);
                                if (hdInfo != null)
                                {
                                    hdInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                                    BusinessLayer.UpdatePatientChargesHdInfo(hdInfo);
                                }
                            }
                            else
                            {
                                result = string.Format("{0}|{1}", "0", respInfo.Remarks);

                                entityAPILog.IsSuccess = false;
                                entityAPILog.ErrorMessage = respInfo.Remarks;
                                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", string.Format("Ada item pemeriksaan yang belum didefinisikan detail artikel pemeriksaan : {0}", invalidItemName.Substring(0, invalidItemName.Length - 1)));
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}", "0", "Tidak ada informasi order yang dapat dikirim ke LIS");
                    }
                #endregion
                }
                return result;
            }
            catch (WebException ex)
            {
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }

        private string SendLISOrderToSoftmedix(int transactionID, int testOrderID)
        {
            string result = string.Empty;
            LaboratoryService oService = new LaboratoryService();

            APIMessageLog log = new APIMessageLog();
            log.Sender = Constant.BridgingVendor.HIS;
            log.Recipient = Constant.BridgingVendor.LIS;
            log.MessageDateTime = DateTime.Now;

            string apiResult = oService.OnSendOrderToLISMethod("N", transactionID, testOrderID, log);
            string[] apiResultInfo = apiResult.Split('|');
            //if (apiResultInfo[0] == "0")
            //{
            //    result = apiResultInfo[1];
            //}
            //else
            //{
            //    result = apiResultInfo[1];
            //}
            result = apiResultInfo[1];

            return result;
        }
    }
}