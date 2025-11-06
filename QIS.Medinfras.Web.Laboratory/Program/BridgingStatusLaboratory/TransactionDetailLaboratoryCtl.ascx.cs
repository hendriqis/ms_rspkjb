using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.CommonLibs.Service;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using System.Net;
using Newtonsoft.Json;
using QIS.Medinfras.Web.Common.API.Model;
using System.IO;

namespace QIS.Medinfras.Web.Laboratory.Program
{
    public partial class TransactionDetailLaboratoryCtl : BaseProcessPopupCtl
    {
        public override void SetToolbarVisibility(ref bool IsAllowAdd)
        {
            IsAllowAdd = false;
        }

        public override void InitializeDataControl(string param)
        {
            IsAdd = false;

            hdnTransactionID.Value = param;
            string filterExpression = string.Format("TransactionID = {0}", hdnTransactionID.Value);
            vPatientChargesHd entity = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
            if (entity != null)
            {
                EntityToControl(entity);
                BindGridView();
            }

        }

        private void BindGridView()
        {
            string filterExpression = string.Format("TransactionID = {0} AND GCItemType = '{1}' AND IsDeleted = 0", hdnTransactionID.Value, Constant.ItemType.LABORATORIUM);
            List<vPatientChargesDt> lstDetail = BusinessLayer.GetvPatientChargesDtList(filterExpression);
            grdView.DataSource = lstDetail;
            grdView.DataBind();
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtOrderPhysician.Text = string.Format("{0}", entity.TestOrderPhysician);
                hdnTestOrderID.Value = entity.TestOrderID.ToString();
                hdnBridgingStatus.Value = entity.GCLISBridgingStatus;
            }
        }

        private bool OnCheckSendToLIS(ref string errMessage)
        {
            bool isSent = false;
            if (hdnBridgingStatus.Value != Constant.LIS_Bridging_Status.OPEN && hdnBridgingStatus.Value != string.Empty)
            {
                errMessage = "Maaf, Transaksi ini sudah dikirim ke LIS";
                isSent = true;
            }
            return isSent;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            try
            {
                int transactionID = Convert.ToInt32(hdnTransactionID.Value);
                int testOrderID = Convert.ToInt32(hdnTestOrderID.Value);
                string referenceNo = string.Empty;
                bool isError = false;

                if (AppSession.IsBridgingToLIS)
                {
                    string[] resultInfo = "0|Unknown Protocol".Split('|');
                    switch (AppSession.LIS_BRIDGING_PROTOCOL)
                    {
                        case Constant.LIS_Bridging_Protocol.WEB_API:
                            switch (AppSession.LIS_PROVIDER)
                            {
                                case Constant.LIS_PROVIDER.HCLAB:
                                    BusinessLayer.SendToLISInterfaceDB(transactionID);
                                    resultInfo = "1|".Split('|');
                                    break;
                                case Constant.LIS_PROVIDER.WYNACOM:
                                    if (OnCheckSendToLIS(ref errMessage))
                                    {
                                        var result2 = SendLISOrderToWYNACOM(transactionID, testOrderID);
                                        resultInfo = ((string)result2).Split('|');
                                    }
                                    break;
                                case Constant.LIS_PROVIDER.ELIMPSE:
                                    if (OnCheckSendToLIS(ref errMessage))
                                    {
                                        var result3 = SendLISOrderToWYNACOM(transactionID, testOrderID);
                                        resultInfo = ((string)result3).Split('|');
                                    }
                                    break;
                                case Constant.LIS_PROVIDER.SOFTMEDIX:
                                    SendLISOrderToSoftmedix(transactionID, testOrderID);
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
                    isError = resultInfo[0] == "0";
                    if (isError)
                        errMessage = resultInfo[1];
                    else
                        retval = "Order berhasil dikirim ke LIS";
                }
                result = !isError;
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
            }
            return result;
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
                        oPatientData.Gender = string.Format("{0}^{1}",oVisit.GCGender, oVisit.Gender);
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
                            result = string.Format("{0}|{1}", "0", string.Format("Ada item pemeriksaan yang belum didefinisikan detail artikel pemeriksaan : {0}",invalidItemName.Substring(0, invalidItemName.Length-1)));
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