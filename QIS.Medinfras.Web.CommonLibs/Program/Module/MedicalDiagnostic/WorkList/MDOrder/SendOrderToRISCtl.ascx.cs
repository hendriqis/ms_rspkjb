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

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class SendOrderToRISCtl : BaseEntryPopupCtl
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
            EntityToControl(entity);
        }

        private void EntityToControl(vPatientChargesHd entity)
        {
            if (entity != null)
            {
                txtTransactionNo.Text = entity.TransactionNo;
                txtTransactionDate.Text = entity.TransactionDateInString;
                txtMedicalNo.Text = entity.MedicalNo;
                txtPatientName.Text = entity.PatientName;
                hdnTestOrderID.Value = entity.TestOrderID.ToString();
            }
        }

        protected override bool OnSaveEditRecord(ref string errMessage, ref string retval)
        {
            bool result = true;

            IDbContext ctx = DbFactory.Configure(true);
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
                if (AppSession.MedicalDiagnostic.MedicalDiagnosticType == MedicalDiagnosticType.Imaging)
                {
                    if (isBridgingToRIS == "1")
                    {

                        string[] resultInfo;

                        switch (risBridgingProtocol)
                        {
                            case Constant.RIS_Bridging_Protocol.WEB_API:
                                var result1 = SendOrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                                resultInfo = ((string)result1).Split('|');
                                break;
                            case Constant.RIS_Bridging_Protocol.HL7:
                                var result2 = SendHL7OrderToRIS(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                                resultInfo = ((string)result2).Split('|');
                                break;
                            case Constant.RIS_Bridging_Protocol.LINK_DB:
                                var result3 = SendOrderToRISLinkDB(Convert.ToInt32(hdnTestOrderID.Value), Convert.ToInt32(hdnTransactionID.Value));
                                resultInfo = ((string)result3).Split('|');
                                break;
                            default:
                                resultInfo = "0|Unknown Protocol".Split('|');
                                break;
                        }
                        referenceNo = resultInfo[1];
                        isError = resultInfo[0] == "0";
                        if (isError)
                            errMessage = resultInfo[1];
                    }
                }


                result = !isError;
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = false;
                errMessage = ex.Message;
                Helper.InsertErrorLog(ex);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            return result;
        }

        #region RIS - Imaging Services
        public object SendOrderToRIS(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    TestOrderDTO oData = new TestOrderDTO();
                    if (oList.Count > 0)
                    {
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode;
                        string orderParamedicName = oVisit.ParamedicName;
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.placerOrderNumber = oHeader.TransactionNo;
                        oData.visitNumber = oVisit.RegistrationNo;
                        oData.pointOfCare = oHeader.ServiceUnitName;
                        oData.room = oVisit.RoomName;
                        oData.bed = oVisit.BedCode;
                        oData.orderDateTime = string.Format("{0} {1}:00", orderDate.ToString("yyyy-MM-dd"), orderTime);
                        oData.imagingOrderPriority = orderPriority;
                        oData.reportingPriority = orderPriority;

                        List<TestOrderDtDTO> lstDetail = new List<TestOrderDtDTO>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDtDTO oDetail = new TestOrderDtDTO();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);
                            procedure oProcedure = new procedure() { procedureCode = item.ItemCode, procedureName = item.ItemName1, modalityCode = modality, procedureFee = 0 };
                            readingPhysician oPhysician = new readingPhysician() { radStaffCode = item.ParamedicCode, radStaffName = item.ParamedicName };
                            List<readingPhysician> lst = new List<readingPhysician>();
                            lst.Add(oPhysician);

                            oDetail.procedure = oProcedure;
                            oDetail.readingPhysician = lst;
                            lstDetail.Add(oDetail);
                        }
                        oData.orderDetail = lstDetail;

                        patient oPatient = new patient();

                        oPatient.patientID = oVisit.MRN.ToString();
                        oPatient.mrn = oVisit.MedicalNo;
                        oPatient.patientName = oVisit.PatientName;
                        oPatient.sex = oVisit.GCGender.Substring(5);
                        oPatient.address = oVisit.HomeAddress;
                        oPatient.dateOfBirth = oVisit.DateOfBirth.ToString("yyyy-MM-dd");
                        oPatient.size = "0";
                        oPatient.weight = "0";
                        oPatient.maritalStatus = string.IsNullOrEmpty(oVisit.GCMaritalStatus) ? "U" : oVisit.GCMaritalStatus.Substring(5);

                        oData.patient = oPatient;

                        List<referringPhysician> lstReferringPhysician = new List<referringPhysician>();

                        if (testOrderID > 0)
                        {
                            lstReferringPhysician.Add(new referringPhysician() { refPhyCode = orderParamedicCode, refPhyName = orderParamedicName });
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(oVisit.ReferralPhysicianCode))
                                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ReferralPhysicianCode, refPhyName = oVisit.ReferralPhysicianName });
                            else
                                lstReferringPhysician.Add(new referringPhysician() { refPhyCode = oVisit.ParamedicCode, refPhyName = oVisit.ParamedicName });
                        }

                        oData.referringPhysician = lstReferringPhysician;

                        APIMessageLog entityAPILog = new APIMessageLog()
                        {
                            MessageDateTime = DateTime.Now,
                            Recipient = "RIS",
                            Sender = "MEDINFRAS",
                            MessageText = JsonConvert.SerializeObject(oData)
                        };

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inputOrder/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

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

                        APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                        if (!string.IsNullOrEmpty(respInfo.Data))
                        {
                            result = string.Format("{0}|{1}", "1", respInfo.Data);

                            entityAPILog.IsSuccess = true;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remark);

                            entityAPILog.IsSuccess = false;
                            entityAPILog.ErrorMessage = respInfo.Remark;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
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
            catch (WebException ex)
            {
                //switch (ex.Status)
                //{
                //    case WebExceptionStatus.ProtocolError:
                //        result = string.Format("{0}|{1}", "0", "Method not found");
                //        break;
                //    default:
                //        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                //        break;
                //}
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }

        public object SendHL7OrderToRIS(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();

                Healthcare healthcare = BusinessLayer.GetHealthcare(AppSession.UserLogin.HealthcareID);

                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();

                    if (healthcare.Initial == "RSMD")
                    {
                        //tambah IsTestItem
                        filterExpression += string.Format(" AND IsTestItem = 1");
                    }

                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    ImagingOrderDTO oData = new ImagingOrderDTO();
                    if (oList.Count > 0)
                    {
                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode;
                        string orderParamedicName = oVisit.ParamedicName;
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";

                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : oVisit.ParamedicCode;
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : oVisit.ParamedicName;

                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.HealthcareID = healthcareID;
                        oData.TestOrderID = transactionID;
                        oData.TestOrderNo = oHeader.TransactionNo;
                        oData.TestOrderDate = oHeader.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oData.TestOrderTime = oHeader.TransactionTime;
                        if (isfromOrder)
                        {
                            oData.PhysicianCode = orderParamedicCode;
                            oData.PhysicianName = orderParamedicName;
                        }
                        else
                        {
                            oData.PhysicianCode = AppSession.RegisteredPatient.ParamedicCode;
                            oData.PhysicianName = AppSession.RegisteredPatient.ParamedicName;
                        }

                        oData.PatientID = oVisit.MRN;

                        PatientInfo oPatientInfo = new PatientInfo();

                        oPatientInfo.PatientID = oVisit.MRN;
                        oPatientInfo.MedicalNo = oVisit.MedicalNo;
                        oPatientInfo.FirstName = oVisit.FirstName;
                        oPatientInfo.MiddleName = oVisit.MiddleName;
                        oPatientInfo.LastName = oVisit.LastName;
                        oPatientInfo.PrefferedName = oVisit.PreferredName;
                        oPatientInfo.Gender = oVisit.Gender;
                        oPatientInfo.Religion = oVisit.Religion;
                        oPatientInfo.MaritalStatus = oVisit.MaritalStatus;
                        oPatientInfo.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oPatientInfo.CityOfBirth = oVisit.CityOfBirth;
                        oPatientInfo.HomeAddress = oVisit.HomeAddress;
                        oPatientInfo.HomeZipCode = oVisit.ZipCode;
                        oPatientInfo.HomePhoneNo1 = oVisit.PhoneNo1;
                        oPatientInfo.HomePhoneNo2 = oVisit.PhoneNo2;
                        oPatientInfo.MobileNo1 = oVisit.MobilePhoneNo1;
                        oPatientInfo.MobileNo2 = oVisit.MobilePhoneNo2;

                        oData.PatientInfo = oPatientInfo;

                        oData.VisitID = oVisit.VisitID;
                        oData.RegistrationID = oVisit.RegistrationID;
                        oData.RegistrationNo = oVisit.RegistrationNo;

                        CompactVisitInfo oVisitInfo = new CompactVisitInfo();
                        oVisit.VisitDate = oVisit.VisitDate;
                        oVisit.VisitTime = oVisit.VisitTime;
                        oVisitInfo.DepartmentID = oVisit.DepartmentID;
                        oVisitInfo.RegistrationNo = oVisit.RegistrationNo;
                        oVisitInfo.ServiceUnitCode = oVisit.ServiceUnitCode;
                        oVisitInfo.ServiceUnitName = oVisit.ServiceUnitName;
                        oVisitInfo.RoomCode = oVisit.RoomCode;
                        oVisitInfo.BedCode = oVisit.BedCode;

                        oData.VisitInformation = oVisitInfo;

                        List<TestOrderDetailInfo> lstDetail = new List<TestOrderDetailInfo>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDetailInfo oDetail = new TestOrderDetailInfo();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);

                            ParamedicMaster oParamedic = BusinessLayer.GetParamedicMasterList(string.Format("ParamedicCode = '{0}'", item.ParamedicCode)).FirstOrDefault();
                            string requestedPhysicianName = string.Format("{0}^{1}^^^^", item.ParamedicCode, item.ParamedicName);
                            if (oParamedic != null)
                            {
                                requestedPhysicianName = string.Format("{0}^{1}^^^^", oParamedic.ParamedicCode, oParamedic.FullName);
                            }

                            oDetail.ItemCode = item.ItemCode;
                            oDetail.ItemName = item.ItemName1;
                            oDetail.RequestedPhysicianName = requestedPhysicianName;
                            oDetail.IsCITO = item.IsCITO;
                            oDetail.ModalityType = modality.Trim();
                            oDetail.ModalityCode = item.ModalityCode;
                            oDetail.ModalityAETitle = item.ModalityAETitle;
                            oDetail.Remarks = string.Empty;

                            lstDetail.Add(oDetail);
                        }

                        oData.OrderItemList = lstDetail;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/", url));
                        if (healthcare.Initial == "RSMD")
                        {
                            request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/imaging/order/sendOrderHL7/", url));
                        }

                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

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
                            result = string.Format("{0}|{1}", "1", respInfo.Data);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remarks);
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
            catch (WebException ex)
            {
                //switch (ex.Status)
                //{
                //    case WebExceptionStatus.ProtocolError:
                //        result = string.Format("{0}|{1}", "0", "Method not found");
                //        break;
                //    default:
                //        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                //        break;
                //}
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }

        public object SendOrderToRISLinkDB(int testOrderID, int transactionID)
        {
            string result = "";
            try
            {
                string url = AppSession.RIS_WEB_API_URL;
                #region Convert into DTO Objects
                bool isfromOrder = testOrderID > 0;

                string filterExpression = string.Format("TransactionID = {0}", transactionID);
                vPatientChargesHd oHeader = BusinessLayer.GetvPatientChargesHdList(filterExpression).FirstOrDefault();
                if (oHeader != null)
                {
                    vConsultVisit oVisit = BusinessLayer.GetvConsultVisitList(string.Format("VisitID = {0}", oHeader.VisitID)).FirstOrDefault();
                    List<vPatientChargesDt> oList = BusinessLayer.GetvPatientChargesDtList(filterExpression);
                    ImagingOrderDTO oData = new ImagingOrderDTO();
                    if (oList.Count > 0)
                    {
                        string healthcareID = AppSession.UserLogin.HealthcareID;
                        string orderNo = string.Empty;
                        string orderPriority = "NORMAL";
                        string orderParamedicCode = oVisit.ParamedicCode;
                        string orderParamedicName = oVisit.ParamedicName;
                        DateTime orderDate = DateTime.Now.Date;
                        string orderTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        if (testOrderID > 0)
                        {
                            vTestOrderHd oOrderHd = BusinessLayer.GetvTestOrderHdList(string.Format("TestOrderID = {0}", testOrderID)).FirstOrDefault();
                            orderNo = oOrderHd.TestOrderNo;
                            orderPriority = oOrderHd != null ? (oOrderHd.IsCITO ? "HIGH" : "NORMAL") : "NORMAL";
                            orderParamedicCode = oOrderHd != null ? oOrderHd.ParamedicCode : "";
                            orderParamedicName = oOrderHd != null ? oOrderHd.ParamedicName : "";
                            orderDate = oOrderHd.TestOrderDate;
                            orderTime = oOrderHd.TestOrderTime;
                        }

                        oData.HealthcareID = healthcareID;
                        oData.TestOrderID = transactionID;
                        oData.TestOrderNo = oHeader.TransactionNo;
                        oData.TestOrderDate = oHeader.TransactionDate.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oData.TestOrderTime = oHeader.TransactionTime;
                        if (isfromOrder)
                        {
                            oData.PhysicianCode = orderParamedicCode;
                            oData.PhysicianName = orderParamedicName;
                        }
                        else
                        {
                            oData.PhysicianCode = AppSession.RegisteredPatient.ParamedicCode;
                            oData.PhysicianName = AppSession.RegisteredPatient.ParamedicName;
                        }

                        oData.PatientID = oVisit.MRN;

                        PatientInfo oPatientInfo = new PatientInfo();

                        oPatientInfo.PatientID = oVisit.MRN;
                        oPatientInfo.MedicalNo = oVisit.MedicalNo;
                        oPatientInfo.FirstName = oVisit.FirstName;
                        oPatientInfo.MiddleName = oVisit.MiddleName;
                        oPatientInfo.LastName = oVisit.LastName;
                        oPatientInfo.PrefferedName = oVisit.PreferredName;
                        oPatientInfo.Gender = oVisit.Gender;
                        oPatientInfo.Religion = oVisit.Religion;
                        oPatientInfo.MaritalStatus = oVisit.MaritalStatus;
                        oPatientInfo.DateOfBirth = oVisit.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_112);
                        oPatientInfo.CityOfBirth = oVisit.CityOfBirth;
                        oPatientInfo.HomeAddress = oVisit.HomeAddress;
                        oPatientInfo.HomeZipCode = oVisit.ZipCode;
                        oPatientInfo.HomePhoneNo1 = oVisit.PhoneNo1;
                        oPatientInfo.HomePhoneNo2 = oVisit.PhoneNo2;
                        oPatientInfo.MobileNo1 = oVisit.MobilePhoneNo1;
                        oPatientInfo.MobileNo2 = oVisit.MobilePhoneNo2;

                        oData.PatientInfo = oPatientInfo;

                        oData.VisitID = oVisit.VisitID;
                        oData.RegistrationID = oVisit.RegistrationID;
                        oData.RegistrationNo = oVisit.RegistrationNo;

                        CompactVisitInfo oVisitInfo = new CompactVisitInfo();
                        oVisit.VisitDate = oVisit.VisitDate;
                        oVisit.VisitTime = oVisit.VisitTime;
                        oVisitInfo.DepartmentID = oVisit.DepartmentID;
                        oVisitInfo.RegistrationNo = oVisit.RegistrationNo;
                        oVisitInfo.ServiceUnitName = oVisit.ServiceUnitName;

                        oData.VisitInformation = oVisitInfo;

                        List<TestOrderDetailInfo> lstDetail = new List<TestOrderDetailInfo>();

                        foreach (vPatientChargesDt item in oList)
                        {
                            TestOrderDetailInfo oDetail = new TestOrderDetailInfo();
                            string modality = String.IsNullOrEmpty(item.GCModality) ? "CT" : item.GCModality.Substring(5);

                            oDetail.ItemCode = item.ItemCode;
                            oDetail.ItemName = item.ItemName1;
                            oDetail.RequestedPhysicianName = item.ParamedicName;
                            oDetail.IsCITO = item.IsCITO;
                            oDetail.ModalityType = modality.Trim();
                            oDetail.ModalityCode = item.ModalityCode;
                            oDetail.ModalityAETitle = item.ModalityAETitle;
                            oDetail.Remarks = string.Empty;

                            lstDetail.Add(oDetail);
                        }

                        oData.OrderItemList = lstDetail;

                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/imaging/order/sendOrderToRISLinkDB/", url));
                        request.Method = "POST";
                        request.ContentType = "application/json";
                        Methods.SetRequestHeader(request);

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
                            result = string.Format("{0}|{1}", "1", respInfo.Data);
                        }
                        else
                        {
                            result = string.Format("{0}|{1}", "0", respInfo.Remarks);
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
            catch (WebException ex)
            {
                //switch (ex.Status)
                //{
                //    case WebExceptionStatus.ProtocolError:
                //        result = string.Format("{0}|{1}", "0", "Method not found");
                //        break;
                //    default:
                //        result = string.Format("{0}|{1}", "0", string.Format("{0} ({1})", ex.Status.ToString()));
                //        break;
                //}
                result = string.Format("{0}|{1}", "0", ex.Status.ToString());
                return result;
            }
        }
        #endregion
    }
}