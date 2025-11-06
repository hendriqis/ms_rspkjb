using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web.Script.Serialization;


namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for GatewayService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class LaboratoryService : System.Web.Services.WebService
    {

        private const string MSG_SUCCESS = "SUCCESS";
        protected string LIS_PRODUCT = string.Empty;
        protected string USER_ID = string.Empty;
        protected string KEY = string.Empty;
        protected string LIS_VERSION = string.Empty;
        protected string URL = string.Empty;
        protected string BRIDGING_SUCCESS = "10";
        //private const string BRIDGING_FAILED = "30";

        #region POST METHOD

        #region SOFTMEDIX LIS

        #region Send Order To LIS
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnSendOrderToLISMethod(string statusOrder, int transactionID, int testOrderID, APIMessageLog log)
        {
            string result = "";
            try
            {
                vPatientChargesHd entityChargesHd = BusinessLayer.GetvPatientChargesHdList(string.Format("TransactionID = {0}", transactionID)).FirstOrDefault();
                List<vPatientChargesDt1> lstCharges = BusinessLayer.GetvPatientChargesDt1List(string.Format("TransactionID = {0}", transactionID));
                //List<TestOrderDt> lstOrder = BusinessLayer.GetTestOrderDtList(string.Format("TestOrderID = {0} AND IsDeleted = 0 AND GCTestOrderStatus != '{1}'", testOrderID, Constant.OrderStatus.CANCELLED));
                vConsultVisit9 entityCV = BusinessLayer.GetvConsultVisit9List(string.Format("VisitID = {0}", entityChargesHd.VisitID)).FirstOrDefault();
                Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", entityCV.MRN)).FirstOrDefault();
                BridgingStatus entityBridgingStatus = BusinessLayer.GetBridgingStatusList(string.Format("TransactionID = {0}", transactionID)).FirstOrDefault();

                if (entityChargesHd != null)
                {
                    if (entityPatient != null)
                    {
                        if (entityBridgingStatus != null)
                        {
                            statusOrder = "U";
                        }

                        GetSettingParameter();

                        #region Initialize Request
                        HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", URL));
                        updateRequest.Method = "POST";
                        updateRequest.ContentType = "application/json";

                        #region message
                        SoftmedixModel message = new SoftmedixModel();

                        #region order
                        SoftmedixOrder order = new SoftmedixOrder();

                        #region msh
                        SoftmedixOrderMsh msh = new SoftmedixOrderMsh();
                        msh.product = LIS_PRODUCT;
                        msh.version = LIS_VERSION;
                        msh.user_id = USER_ID;
                        msh.key = KEY;
                        #endregion

                        #region pid
                        SoftmedixOrderPid pid = new SoftmedixOrderPid();
                        pid.pmrn = entityPatient.MedicalNo;
                        pid.pname = entityPatient.FullName;
                        if (entityPatient.GCSex == Constant.Gender.MALE)
                        {
                            pid.sex = "L";
                        }
                        else
                        {
                            pid.sex = "P";
                        }
                        pid.birth_dt = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_WITH_PERIOD);
                        if (entityPatient.HomeAddressID != 0)
                        {
                            vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entityPatient.HomeAddressID)).FirstOrDefault();
                            if (entityAddress != null)
                            {
                                if (entityAddress.cfFullHomeAddress.Length > 100)
                                {
                                    pid.address = entityAddress.cfFullHomeAddress.Substring(0, 100);
                                }
                                else
                                {
                                    pid.address = entityAddress.cfFullHomeAddress;
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(entityPatient.MobilePhoneNo1))
                        {
                            pid.no_tlp = entityPatient.MobilePhoneNo1;
                        }
                        #endregion

                        #region obr
                        SoftmedixOrderObr obr = new SoftmedixOrderObr();
                        obr.order_control = statusOrder;
                        if (entityCV.DepartmentID == Constant.Facility.INPATIENT)
                        {
                            obr.ptype = "IP";
                        }
                        else if (entityCV.DepartmentID == Constant.Facility.MEDICAL_CHECKUP)
                        {
                            obr.ptype = "MC";
                        }
                        else
                        {
                            obr.ptype = "OP";
                        }
                        obr.reg_no = entityCV.RegistrationNo;
                        obr.order_lab = entityChargesHd.TransactionNo;
                        obr.provider_id = entityCV.BusinessPartnerCode;
                        if (entityCV.BusinessPartnerName.Length > 50)
                        {
                            obr.provider_name = entityCV.BusinessPartnerName.Substring(0, 50);
                        }
                        else
                        {
                            obr.provider_name = entityCV.BusinessPartnerName;
                        }
                        obr.order_date = entityChargesHd.TransactionDate.ToString(Constant.FormatString.DATE_TIME_FORMAT_WITH_PERIOD);
                        obr.bangsal_id = entityCV.ServiceUnitCode;
                        obr.bangsal_name = entityCV.ServiceUnitName;
                        if (obr.ptype == "IP")
                        {
                            obr.bed_id = obr.bed_name = entityCV.BedCode;
                        }
                        else
                        {
                            obr.bed_id = obr.bed_name = "0000";
                        }
                        obr.class_id = entityCV.ChargeClassCode;
                        obr.class_name = entityCV.ChargeClassName;
                        obr.med_legal = "N";
                        obr.user_id = AppSession.UserLogin.UserName;

                        List<SoftmedixOrderObrDt> lstObrDt = new List<SoftmedixOrderObrDt>();
                        int n = 0;
                        int invalidItem = 0;
                        string invalidItemName = string.Empty;
                        if (lstCharges.Count > 0)
                        {
                            foreach (vPatientChargesDt1 dt in lstCharges)
                            {
                                obr.cito = dt.IsCITO == true ? "Y" : "N";
                                obr.clinician_id = dt.ParamedicCode;
                                obr.clinician_name = dt.ParamedicName;

                                string filterExp = string.Format("ItemID = {0} ORDER BY DisplayOrder", dt.ItemID);
                                List<vItemLaboratoryFraction> itemFractionList = BusinessLayer.GetvItemLaboratoryFractionList(filterExp);
                                if (itemFractionList.Count == 0)
                                {
                                    invalidItem += 1;
                                    invalidItemName += dt.ItemName1 + ";";
                                }

                                if (invalidItem == 0)
                                {
                                    foreach (vItemLaboratoryFraction fractionItem in itemFractionList)
                                    {
                                        lstObrDt.Add(new SoftmedixOrderObrDt(n.ToString(), fractionItem.ItemCode));
                                        n += 1;
                                    }
                                }
                                else
                                {
                                    result = string.Format("0|{0}|{1}", "0", string.Format("Ada item pemeriksaan yang belum didefinisikan detail artikel pemeriksaan : {0}", invalidItemName.Substring(0, invalidItemName.Length - 1)), string.Empty);
                                }
                            }
                        }
                        obr.order_test = lstObrDt;
                        #endregion

                        order.msh = msh;
                        order.pid = pid;
                        order.obr = obr;
                        #endregion

                        message.order = order;
                        #endregion

                        if (string.IsNullOrEmpty(result))
                        {
                            string data = JsonConvert.SerializeObject(message);

                            byte[] bytes = Encoding.UTF8.GetBytes(data);
                            updateRequest.ContentLength = bytes.Length;
                            Stream putStream = updateRequest.GetRequestStream();
                            putStream.Write(bytes, 0, bytes.Length);
                            putStream.Close();
                            WebResponse response = (WebResponse)updateRequest.GetResponse();
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                string resultLis = sr.ReadToEnd();
                                SoftmedixResponse responseLis = JsonConvert.DeserializeObject<SoftmedixResponse>(resultLis);
                                if (responseLis.response.code == BRIDGING_SUCCESS)
                                {
                                    foreach (vPatientChargesDt1 dt in lstCharges)
                                    {
                                        PatientChargesDtInfo entityDtInfo = BusinessLayer.GetPatientChargesDtInfo(dt.ID);
                                        if (entityDtInfo != null)
                                        {
                                            entityDtInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                                            BusinessLayer.UpdatePatientChargesDtInfo(entityDtInfo);
                                        }
                                    }
                                    PatientChargesHdInfo hdInfo = BusinessLayer.GetPatientChargesHdInfo(transactionID);
                                    if (hdInfo != null)
                                    {
                                        hdInfo.GCLISBridgingStatus = Constant.LIS_Bridging_Status.SENT;
                                        BusinessLayer.UpdatePatientChargesHdInfo(hdInfo);
                                    }

                                    BridgingStatus bridgingStatus = new BridgingStatus();
                                    bridgingStatus.TransactionID = entityChargesHd.TransactionID;
                                    bridgingStatus.TransactionNo = entityChargesHd.TransactionNo;
                                    bridgingStatus.SendDateTime = DateTime.Now;
                                    BusinessLayer.InsertBridgingStatus(bridgingStatus);

                                    log.MessageText = data;
                                    log.Response = resultLis;
                                    log.IsSuccess = true;
                                    BusinessLayer.InsertAPIMessageLog(log);

                                    result = string.Format("0|{0}|{1}", resultLis, data);
                                }
                                else
                                {
                                    log.MessageText = data;
                                    log.Response = resultLis;
                                    log.IsSuccess = false;
                                    BusinessLayer.InsertAPIMessageLog(log);

                                    result = string.Format("0|{0}|{1}", resultLis, data);
                                }
                            }
                        }
                        else
                        {
                            log.MessageText = string.Empty;
                            log.Response = result;
                            log.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(log);
                        }
                        #endregion
                    }
                    else
                    {
                        result = string.Format("0|Pasien tidak ditemukan|{0}", string.Empty);

                        log.MessageText = string.Empty;
                        log.Response = result;
                        log.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(log);
                    }
                }
                else
                {
                    result = string.Format("0|Nomor transaksi tidak ditemukan|{0}", string.Empty);

                    log.MessageText = string.Empty;
                    log.Response = result;
                    log.IsSuccess = false;
                    BusinessLayer.InsertAPIMessageLog(log);
                }
            }
            catch (Exception ex)
            {
                result = string.Format("0|{0}", ex.Message);

                log.MessageText = string.Empty;
                log.Response = result;
                log.IsSuccess = false;
                BusinessLayer.InsertAPIMessageLog(log);
            }

            return result;
        }
        #endregion

        #region Update Patient Data Master
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string OnPatientDataChanged(string mrn, APIMessageLog log)
        {
            try
            {
                string result = "";
                Patient entityPatient = BusinessLayer.GetPatientList(string.Format("MRN = {0}", mrn)).FirstOrDefault();

                if (entityPatient != null)
                {
                    GetSettingParameter();

                    #region Initialize Request
                    HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}", URL));
                    updateRequest.Method = "POST";
                    updateRequest.ContentType = "application/json";

                    #region message
                    SoftmedixModel message = new SoftmedixModel();

                    #region order
                    SoftmedixOrder order = new SoftmedixOrder();

                    #region msh
                    SoftmedixOrderMsh msh = new SoftmedixOrderMsh();
                    msh.product = LIS_PRODUCT;
                    msh.version = LIS_VERSION;
                    msh.user_id = USER_ID;
                    msh.key = KEY;
                    #endregion

                    #region pid
                    SoftmedixOrderPid pid = new SoftmedixOrderPid();
                    pid.pmrn = entityPatient.MedicalNo;
                    pid.pname = entityPatient.FullName;
                    if (entityPatient.GCSex == Constant.Gender.MALE)
                    {
                        pid.sex = "L";
                    }
                    else
                    {
                        pid.sex = "P";
                    }
                    pid.birth_dt = entityPatient.DateOfBirth.ToString(Constant.FormatString.DATE_FORMAT_WITH_PERIOD);
                    if (entityPatient.HomeAddressID != 0)
                    {
                        vAddress entityAddress = BusinessLayer.GetvAddressList(string.Format("AddressID = {0}", entityPatient.HomeAddressID)).FirstOrDefault();
                        if (entityAddress != null)
                        {
                            if (entityAddress.cfFullHomeAddress.Length > 100)
                            {
                                pid.address = entityAddress.cfFullHomeAddress.Substring(0, 100);
                            }
                            else
                            {
                                pid.address = entityAddress.cfFullHomeAddress;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(entityPatient.MobilePhoneNo1))
                    {
                        pid.no_tlp = entityPatient.MobilePhoneNo1;
                    }
                    #endregion

                    #region obr
                    SoftmedixOrderObr obr = new SoftmedixOrderObr();
                    obr.order_control = "U";
                    obr.user_id = AppSession.UserLogin.UserName;
                    #endregion

                    order.msh = msh;
                    order.pid = pid;
                    order.obr = obr;
                    #endregion

                    message.order = order;
                    #endregion

                    string data = JsonConvert.SerializeObject(message);

                    byte[] bytes = Encoding.UTF8.GetBytes(data);
                    updateRequest.ContentLength = bytes.Length;
                    Stream putStream = updateRequest.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();
                    WebResponse response = (WebResponse)updateRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        string resultLis = sr.ReadToEnd();
                        SoftmedixResponse responseLis = JsonConvert.DeserializeObject<SoftmedixResponse>(resultLis);
                        if (responseLis.response.code == BRIDGING_SUCCESS)
                        {
                            log.MessageText = data;
                            log.Response = resultLis;
                            log.IsSuccess = true;
                            BusinessLayer.InsertAPIMessageLog(log);

                            result = string.Format("0|{0}|{1}", resultLis, data);
                        }
                        else
                        {
                            log.MessageText = data;
                            log.Response = resultLis;
                            log.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(log);

                            result = string.Format("0|{0}|{1}", resultLis, data);
                        }
                    }
                    #endregion

                    return result;
                }
                else
                {
                    result = string.Format("0|Pasien tidak ditemukan|{0}", string.Empty);
                }
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #endregion

        #endregion

        #region Utility Function
        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            SettingParameterDt entityConsID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_CONS_ID);
            SettingParameterDt entityPassID = BusinessLayer.GetSettingParameterDt(AppSession.UserLogin.HealthcareID, Constant.SettingParameter.CONSUMER_PASS_ID);

            string consID = entityConsID.ParameterValue;
            string pass = entityPassID.ParameterValue;
            string data = unixTimestamp.ToString() + consID;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}", data), pass));
        }

        private string GenerateSignature(string data, string secretKey)
        {
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            var encodedSignature = Convert.ToBase64String(signature);

            return encodedSignature;
        }
        #endregion

        private void GetSettingParameter()
        {
            List<SettingParameterDt> lstSetvarDt = BusinessLayer.GetSettingParameterDtList(string.Format("ParameterCode IN ('{0}','{1}','{2}','{3}')", Constant.SettingParameter.LB_LIS_CONSUMER_ID, Constant.SettingParameter.LB_LIS_CONSUMER_PASSWORD, Constant.SettingParameter.LB_LIS_WEB_API_URL, Constant.SettingParameter.LB_LIS_PROVIDER));
            URL = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_WEB_API_URL).FirstOrDefault().ParameterValue;
            KEY = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_CONSUMER_PASSWORD).FirstOrDefault().ParameterValue;
            USER_ID = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_CONSUMER_ID).FirstOrDefault().ParameterValue;
            LIS_PRODUCT = lstSetvarDt.Where(w => w.ParameterCode == Constant.SettingParameter.LB_LIS_PROVIDER).FirstOrDefault().ParameterValue;

            List<StandardCode> lstStandardCode = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID IN ('{0}')", LIS_PRODUCT));
            string providerLis = lstStandardCode.Where(s => s.StandardCodeID == Constant.LIS_PROVIDER.SOFTMEDIX).FirstOrDefault().TagProperty;
            if (!string.IsNullOrEmpty(providerLis))
            {
                string[] version = new string[] {};
                if (providerLis.Contains('|'))
                {
                    if (providerLis.Contains(':'))
                    {
                        version = providerLis.Split(':');
                        LIS_VERSION = version[1];
                    }
                }
                else
                {
                    if (providerLis.Contains(':'))
                    {
                        version = providerLis.Split(':');
                        LIS_VERSION = version[1];
                    }
                }
            }
        }

    }
}
