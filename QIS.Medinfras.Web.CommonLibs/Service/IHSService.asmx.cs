using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Xml.Linq;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using Newtonsoft.Json;
using QIS.Medinfras.Web.Common.API.Model;
using System.Globalization;

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for IHSService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class IHSService : System.Web.Services.WebService
    {
        //private string url = AppSession.SA0196;
        //private string url_MedinfrasAPI = AppSession.SA0132;
        private string url = "https://api-satusehat-dev.dto.kemkes.go.id";
        private string url_MedinfrasAPI = AppSession.SA0132;
        private string SA0194 = "I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE";
        private string SA0195 = "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4";
        private string SA0197 = "10083848";
        private const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
        private const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;

        private const string IDENTIFIER_URL = "";

        #region MEDINFRAS-API : IHS
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetPatientIHSNumberByNIK(string NIK)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(NIK))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r001/001/{1}", url_MedinfrasAPI, NIK));
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Informasi NIK tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0",string.Empty, ex.Message);
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetParamedicIHSNumberByNIK(string NIK)
        {
            string result = "";
            var json = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(NIK))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r002/001/{1}", url_MedinfrasAPI, NIK));
                    request.Method = "GET";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Informasi NIK tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GenerateServiceUnitIHSLocationID(string serviceUnitCode, string serviceUnitName, string departmentID)
        {
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(serviceUnitCode) && !string.IsNullOrEmpty(serviceUnitName))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r004/001", url_MedinfrasAPI));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload oData = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload();
                    QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_LocationPayload oDataLocation = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_LocationPayload();
                    oDataLocation.DepartmentID = departmentID;
                    oDataLocation.ServiceUnitCode = serviceUnitCode;
                    oDataLocation.ServiceUnitName = serviceUnitName;
                    oData.Location = oDataLocation;

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remark);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Informasi NIK tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GenerateOrganizationID(string departmentID, string departmentName)
        {
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(departmentID) && !string.IsNullOrEmpty(departmentName))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r003/001", url_MedinfrasAPI));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload oData = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload();
                    QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_OrganizationPayload oDataOrganization = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_OrganizationPayload();
                    oDataOrganization.DepartmentID = departmentID;
                    oDataOrganization.DepartmentName = departmentName;
                    oData.Organization = oDataOrganization;

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    APIResponse respInfo = JsonConvert.DeserializeObject<APIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remark);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Kode dan Nama Instalasi tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendObservationIHS(string ID, string registrationID)
        {
            string result = "";

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r007/001", url_MedinfrasAPI));
                request.Method = "POST";
                request.ContentType = "application/json";
                Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                Registration entity = BusinessLayer.GetRegistration(Convert.ToInt32(registrationID));
                QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload oData = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterbackPayload();
                QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_ObservationPayload oDataObservation = new QIS.Medinfras.Web.Common.IHSModel.MedinfrasCenterback_ObservationPayload();
                oDataObservation.RegistrationNo = entity.RegistrationNo;
                oData.Observation = oDataObservation;

                var json = JsonConvert.SerializeObject(oData);
                byte[] bytes = Encoding.UTF8.GetBytes(json);
                request.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();

                WebResponse response = (WebResponse)request.GetResponse();
                string responseMsg = string.Empty;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseMsg = sr.ReadToEnd();
                };

                MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                if (!string.IsNullOrEmpty(respInfo.Data))
                {
                    string[] respData = respInfo.Data.Split('#');
                    VitalSignType entityVST = BusinessLayer.GetVitalSignTypeList(string.Format("VitalSignLabel = '{0}'", respData[1])).FirstOrDefault();
                    VitalSignDt entityDt = BusinessLayer.GetVitalSignDtList(string.Format("ID = {0} AND VitalSignID = {1}", ID, entityVST.VitalSignID)).FirstOrDefault();
                    entityDt.IHSReferenceID = respData[0];
                    BusinessLayer.UpdateVitalSignDt(entityDt);
                    result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);

                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PostEncounter1(string registrationID, string registrationNo, string visitID)
        {
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(registrationNo))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/r005/001", url_MedinfrasAPI));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    IHSModel.MedinfrasCenterbackPayload oData = new IHSModel.MedinfrasCenterbackPayload();
                    IHSModel.MedinfrasCenterback_EncounterPayload oDataEncounter = new IHSModel.MedinfrasCenterback_EncounterPayload();
                    oDataEncounter.RegistrationID = registrationID;
                    oDataEncounter.RegistrationNo = registrationNo;
                    oDataEncounter.VisitID = visitID;
                    oData.Encounter = oDataEncounter;

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Parameter Nomor Registrasi tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string EncounterBundling1(string registrationID, string registrationNo, string visitID)
        {
            string result = "";

            try
            {
                if (!string.IsNullOrEmpty(registrationNo))
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/ihs/bundle/001", url_MedinfrasAPI));
                    request.Method = "POST";
                    request.ContentType = "application/json";
                    Methods.SetRequestHeaderWithHealthcareID(request, AppSession.UserLogin.HealthcareID, AppSession.SA0130, AppSession.SA0131);

                    IHSModel.MedinfrasCenterbackPayload oData = new IHSModel.MedinfrasCenterbackPayload();
                    IHSModel.MedinfrasCenterback_EncounterPayload oDataEncounter = new IHSModel.MedinfrasCenterback_EncounterPayload();
                    oDataEncounter.RegistrationID = registrationID;
                    oDataEncounter.RegistrationNo = registrationNo;
                    oDataEncounter.VisitID = visitID;
                    oData.Encounter = oDataEncounter;

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    MedinfrasAPIResponse respInfo = JsonConvert.DeserializeObject<MedinfrasAPIResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.Data))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.Data, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", string.Empty, respInfo.Remarks);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", string.Empty, "Parameter Nomor Registrasi tidak boleh kosong");
                }
                return result;
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", string.Empty, ex.Message);
                return result;
            }
        }
        #endregion

        #region Organization
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateIHSOrganizationID(string departmentID, string departmentName)
        {
            string result = "";
            try
            {
                #region Convert into DTO Objects
                QIS.Medinfras.Web.Common.IHSModel.OrganizationResourceBody oData = new QIS.Medinfras.Web.Common.IHSModel.OrganizationResourceBody();

                oData.resourceType = Constant.FHIRResourceType.ORGANIZATION;
                oData.active = true;
                List<QIS.Medinfras.Web.Common.IHSModel.Identifier> oDataIdentiferLst = new List<QIS.Medinfras.Web.Common.IHSModel.Identifier>();
                QIS.Medinfras.Web.Common.IHSModel.Identifier oDataIdentifer = new QIS.Medinfras.Web.Common.IHSModel.Identifier()
                {
                    use = "official",
                    system = string.Format("{0}{1}", Constant.IHSCodingSystemUrl.Location, SA0197),
                    value = departmentID
                };
                oDataIdentiferLst.Add(oDataIdentifer);
                oData.identifier = oDataIdentiferLst;

                List<QIS.Medinfras.Web.Common.IHSModel.Type> lstType = new List<QIS.Medinfras.Web.Common.IHSModel.Type>();
                QIS.Medinfras.Web.Common.IHSModel.Type oDataType = new QIS.Medinfras.Web.Common.IHSModel.Type();
                List<QIS.Medinfras.Web.Common.IHSModel.Coding> lstCoding = new List<QIS.Medinfras.Web.Common.IHSModel.Coding>();
                QIS.Medinfras.Web.Common.IHSModel.Coding oDataCoding = new QIS.Medinfras.Web.Common.IHSModel.Coding();
                oDataCoding.system = Constant.IHSCodingSystemUrl.OrganizationType;
                oDataCoding.code = "dept";
                oDataCoding.display = departmentName;
                lstCoding.Add(oDataCoding);
                oDataType.coding = lstCoding;
                lstType.Add(oDataType);
                oData.type = lstType;

                oData.name = departmentName;

                List<QIS.Medinfras.Web.Common.IHSModel.Telecom> lstTelecom = new List<QIS.Medinfras.Web.Common.IHSModel.Telecom>();
                QIS.Medinfras.Web.Common.IHSModel.Telecom oDataTelecom = new QIS.Medinfras.Web.Common.IHSModel.Telecom()
                {
                    system = "phone",
                    value = "123456",
                    use = "work"
                };
                lstTelecom.Add(oDataTelecom);
                oData.telecom = lstTelecom;

                List<QIS.Medinfras.Web.Common.IHSModel.Address> lstAddress = new List<QIS.Medinfras.Web.Common.IHSModel.Address>();
                QIS.Medinfras.Web.Common.IHSModel.Address oDataAddress = new QIS.Medinfras.Web.Common.IHSModel.Address();
                oDataAddress.use = "work";
                List<string> lstStr = new List<string>();
                lstStr.Add("Jalan Kawi");
                oDataAddress.line = lstStr;
                oDataAddress.city = "Semarang";
                oDataAddress.postalCode = "50614";
                oDataAddress.country = "ID";
                lstAddress.Add(oDataAddress);
                oData.address = lstAddress;

                QIS.Medinfras.Web.Common.IHSModel.PartOf oDataPartOf = new QIS.Medinfras.Web.Common.IHSModel.PartOf();
                oDataPartOf.reference = "Organization/" + SA0197;
                oData.partOf = oDataPartOf;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    //Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Encounter");
                    Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Organization", url));

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";

                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken(SA0194, SA0195));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    QIS.Medinfras.Web.Common.IHSModel.LocationResourceResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.LocationResourceResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }
        #endregion

        #region Location
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string CreateIHSLocationID(string serviceUnitCode, string serviceUnitName, string departmentID)
        {
            string result = "";
            try
            {
                #region Convert into DTO Objects
                QIS.Medinfras.Web.Common.IHSModel.LocationResourceBody oData = new QIS.Medinfras.Web.Common.IHSModel.LocationResourceBody();

                oData.resourceType = Constant.FHIRResourceType.LOCATION;
                List<QIS.Medinfras.Web.Common.IHSModel.Identifier> oDataIdentiferLst = new List<QIS.Medinfras.Web.Common.IHSModel.Identifier>();
                QIS.Medinfras.Web.Common.IHSModel.Identifier oDataIdentifer = new QIS.Medinfras.Web.Common.IHSModel.Identifier()
                {
                    system = string.Format("{0}{1}", Constant.IHSCodingSystemUrl.Location, SA0197),
                    value = serviceUnitCode
                };
                oDataIdentiferLst.Add(oDataIdentifer);
                oData.identifier = oDataIdentiferLst;

                oData.status = "active"; //need constant?
                oData.name = serviceUnitName;
                oData.description = serviceUnitName;
                switch (departmentID)
                {
                    case Constant.Facility.OUTPATIENT:
                        oData.description += ", INSTALASI RAWAT JALAN";
                        break;
                }
                oData.mode = "instance";

                List<QIS.Medinfras.Web.Common.IHSModel.Telecom> lstTelecom = new List<QIS.Medinfras.Web.Common.IHSModel.Telecom>();
                QIS.Medinfras.Web.Common.IHSModel.Telecom oDataTelecom = new QIS.Medinfras.Web.Common.IHSModel.Telecom()
                {
                    system = "phone",
                    value = "123456",
                    use = "work"
                };
                lstTelecom.Add(oDataTelecom);
                oData.telecom = lstTelecom;

                QIS.Medinfras.Web.Common.IHSModel.Address oDataAddress = new QIS.Medinfras.Web.Common.IHSModel.Address();
                oDataAddress.use = "work";
                List<string> lstStr = new List<string>();
                lstStr.Add("Jalan Kawi");
                oDataAddress.line = lstStr;
                oDataAddress.city = "Semarang";
                oDataAddress.postalCode = "50614";
                oDataAddress.country = "ID";
                oData.address = oDataAddress;

                QIS.Medinfras.Web.Common.IHSModel.PhysicalType oDataPhysicalType = new QIS.Medinfras.Web.Common.IHSModel.PhysicalType();
                List<QIS.Medinfras.Web.Common.IHSModel.Coding> lstCoding = new List<QIS.Medinfras.Web.Common.IHSModel.Coding>();
                QIS.Medinfras.Web.Common.IHSModel.Coding oDataCoding = new QIS.Medinfras.Web.Common.IHSModel.Coding();
                oDataCoding.system = Constant.IHSCodingSystemUrl.LocationPhysicialType;
                oDataCoding.code = "ro";
                oDataCoding.display = "Room";
                lstCoding.Add(oDataCoding);
                oDataPhysicalType.coding = lstCoding;

                QIS.Medinfras.Web.Common.IHSModel.Position oDataPosition = new QIS.Medinfras.Web.Common.IHSModel.Position();
                oDataPosition.longitude = -6.23115426275766;
                oDataPosition.latitude = 106.83239885393944;
                oDataPosition.altitude = 0;
                oData.position = oDataPosition;

                QIS.Medinfras.Web.Common.IHSModel.ManagingOrganization oDataOrganization = new QIS.Medinfras.Web.Common.IHSModel.ManagingOrganization();
                oDataOrganization.reference = "Organization/" + SA0197;
                oData.managingOrganization = oDataOrganization;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    //Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Encounter");
                    Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Location", url));

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";

                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken(SA0194, SA0195));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    QIS.Medinfras.Web.Common.IHSModel.LocationResourceResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.LocationResourceResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }
        #endregion

        #region Data Pasien

        #region Get IHSNumber By NIK
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetIHSNumberByNIK(String NIK)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SATUSEHAT,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                string base_url = url;
                string identifier = string.Format("identifier=https://fhir.kemkes.go.id/id/nik|{0}", NIK);
                Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Patient?{1}", base_url, identifier));
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(uri);
                //SetAuthenticationRequest(GETRequest, uri, "Bearer", GenerateToken("I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE", "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4"));
                SetAuthenticationRequest(GETRequest, uri, "Bearer", GenerateToken(AppSession.SA0194, AppSession.SA0195));
                GETRequest.Method = "GET";
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    QIS.Medinfras.Web.Common.IHSModel.PatientResouceResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PatientResouceResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (respInfo.total > 0)
                    {
                        string ihsNumber = respInfo.entry[0].resource.id;
                        if (respInfo.total > 1)
                            ihsNumber = respInfo.entry[1].resource.id;

                        result = string.Format("{0}|{1}|{2}", "1", ihsNumber, "");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", "Data Nomor SATUSEHAT Pasien tidak ditemukan");
                    }
                }
                int status = (int)response.StatusCode;
                return result;
            }
            catch (WebException ex)
            {
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", ex.Message, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        #endregion

        #endregion

        #region Data Tenaga Kesehatan / Paramedic

        #region Get Practicioner IHSNumber By NIK
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPractitionerIHSNumberByNIK(String NIK)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SATUSEHAT,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                string base_url = url;
                string identifier = string.Format("identifier=https://fhir.kemkes.go.id/id/nik|{0}", NIK);
                Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Practitioner?{1}", base_url, identifier));
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(uri);
                SetAuthenticationRequest(GETRequest, uri, "Bearer", GenerateToken(AppSession.SA0194, AppSession.SA0195));
                GETRequest.Method = "GET";
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    QIS.Medinfras.Web.Common.IHSModel.PatientResouceResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PatientResouceResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (respInfo.total > 0)
                    {
                        string ihsNumber = respInfo.entry[0].resource.id;
                        result = string.Format("{0}|{1}|{2}", "1", ihsNumber, "");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", "Data Nomor SATUSEHAT Tenaga Kesehatan  tidak ditemukan");
                    }
                }
                int status = (int)response.StatusCode;
                return result;
            }
            catch (WebException ex)
            {
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", ex.Message, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }
        #endregion

        #endregion

        #region Encounter
        /// <summary>
        /// Type : 00 = Data Kunjungan, 01 = Transaksi Pasien, 02 = Pembuatan Tagihan, 03 = Pembayaran
        /// TransactionReferenceID = Nomor Transaksi / Nomor Tagihan / Nomor Payment tergantung type
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PostEncounter(string healthcareID, vRegistration oRegistration, string type, int transactionReferenceID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0196;
                #region Convert into DTO Objects
                QIS.Medinfras.Web.Common.IHSModel.EncounterPayLoad oData = new QIS.Medinfras.Web.Common.IHSModel.EncounterPayLoad();

                oData.resourceType = Constant.FHIRResourceType.ENCOUNTER;
                oData.status = "arrived";

                IHSModel.Class oClass = new IHSModel.Class() { system = "http://terminology.hl7.org/CodeSystem/v3-ActCode", code = "AMB", display = "ambulatory" };
                oData.@class = oClass;


                IHSModel.Subject subject = new IHSModel.Subject() { reference = string.Format("Patient/{0}", oRegistration.IHSNumber), display = oRegistration.PatientName };
                oData.subject = subject;

                IHSModel.Participant oParticipant = new IHSModel.Participant();
                IHSModel.Coding coding = new IHSModel.Coding() { code = "ATND", display = "attender", system = "http://terminology.hl7.org/CodeSystem/v3-ParticipationType" };
                List<IHSModel.Coding> lstCoding = new List<IHSModel.Coding>();
                lstCoding.Add(coding);

                IHSModel.Type participantType = new IHSModel.Type();
                participantType.coding = lstCoding;

                List<IHSModel.Type> lstType = new List<IHSModel.Type>();
                lstType.Add(participantType);


                IHSModel.Individual individual = new IHSModel.Individual();
                individual.display = oRegistration.ParamedicName;
                //individual.reference = "Practitioner/N10000001";
                individual.reference = string.Format("Practitioner/{0}",oRegistration.ParamedicIHSReferenceID);

                oParticipant.type = lstType;
                oParticipant.individual = individual;

                List<IHSModel.Participant> lstParticipant = new List<IHSModel.Participant>();
                lstParticipant.Add(oParticipant);

                oData.participant = lstParticipant;

                IHSModel.Period oPeriod = new IHSModel.Period();
                DateTime arrivedTime = DateTime.ParseExact(string.Format("{0} {1}", oRegistration.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT), oRegistration.RegistrationTime), Common.Constant.FormatString.DATE_TIME_FORMAT_4, CultureInfo.InvariantCulture, DateTimeStyles.None);

                oPeriod.start = string.Format("{0}T{1}:00+07:00", oRegistration.RegistrationDate.ToString("yyyy-MM-dd"), oRegistration.RegistrationTime);
                oPeriod.end = string.Format("{0}T{1}:00+07:00", oRegistration.RegistrationDate.ToString("yyyy-MM-dd"), oRegistration.RegistrationTime);
                oData.period = oPeriod;

                IHSModel.SubLocation oSubLocation = new IHSModel.SubLocation();
                //oSubLocation.reference = string.Format("Location/{0}", "d649b9e5-5e55-42d1-b53c-a6be15dfcc85");
                oSubLocation.reference = string.Format("Location/{0}", oRegistration.ServiceUnitIHSLocationID);
                oSubLocation.display = string.Format("{0},{1}", oRegistration.ServiceUnitName, oRegistration.DepartmentID);

                IHSModel.Location oLocation = new IHSModel.Location();
                oLocation.location = oSubLocation;

                List<IHSModel.Location> lstLocation = new List<IHSModel.Location>();
                lstLocation.Add(oLocation);

                oData.location = lstLocation;

                IHSModel.StatusHistory oStatusHistory = new IHSModel.StatusHistory();
                oStatusHistory.status = "arrived";
                IHSModel.Period oArrivedPeriod = new IHSModel.Period();
                oArrivedPeriod.start = string.Format("{0}T{1}:00+07:00", oRegistration.RegistrationDate.ToString("yyyy-MM-dd"), oRegistration.RegistrationTime);
                oArrivedPeriod.end = string.Format("{0}T{1}:00+07:00", oRegistration.RegistrationDate.ToString("yyyy-MM-dd"), oRegistration.RegistrationTime);
                oStatusHistory.period = oArrivedPeriod;

                List<IHSModel.StatusHistory> lstStatusHistory = new List<IHSModel.StatusHistory>();
                lstStatusHistory.Add(oStatusHistory);

                oData.statusHistory = lstStatusHistory;

                IHSModel.ServiceProvider oServiceProvider = new IHSModel.ServiceProvider();
                //oServiceProvider.reference = string.Format("Organization/{0}", "10083848");
                oServiceProvider.reference = string.Format("Organization/{0}", AppSession.SA0197);

                IHSModel.EncounterIdentifier oIdentifier = new IHSModel.EncounterIdentifier();
                //oIdentifier.system = string.Format("http://sys-ids.kemkes.go.id/encounter/{0}", "10083848");
                oIdentifier.system = string.Format("http://sys-ids.kemkes.go.id/encounter/{0}", AppSession.SA0197);
                oIdentifier.value = oRegistration.RegistrationNo;
                List<IHSModel.EncounterIdentifier> lstIdentifier = new List<IHSModel.EncounterIdentifier>();
                lstIdentifier.Add(oIdentifier);

                oData.serviceProvider = oServiceProvider;
                oData.identifier = lstIdentifier;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    //Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Encounter");
                    Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Encounter", url));

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    //SetAuthenticationRequest(request, uri, "Bearer", GenerateToken("I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE", "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4"));

                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken(AppSession.SA0194, AppSession.SA0195));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        RegistrationInfo oRegistrationInfo = BusinessLayer.GetRegistrationInfo(oRegistration.RegistrationID);
                        if (oRegistrationInfo != null)
                        {
                            oRegistrationInfo.ExternalRegistrationNo = respInfo.id;
                            BusinessLayer.UpdateRegistrationInfo(oRegistrationInfo);
                        }
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    HttpWebResponse err = ((HttpWebResponse)ex.Response);
                    using (StreamReader sr = new StreamReader(err.GetResponseStream()))
                    {
                        string error = sr.ReadToEnd();

                        result = string.Format("{0}|{1}|{2}", "0", error, string.Empty);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                }
                return result;
            }
        }

        /// <summary>
        /// Type : 00 = Data Kunjungan, 01 = Transaksi Pasien, 02 = Pembuatan Tagihan, 03 = Pembayaran
        /// TransactionReferenceID = Nomor Transaksi / Nomor Tagihan / Nomor Payment tergantung type
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PutEncounter(string healthcareID, vRegistration oRegistration, string type, string encounterID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;
                #region Convert into DTO Objects
                QIS.Medinfras.Web.Common.IHSModel.PutEncounterPayLoad oData = new QIS.Medinfras.Web.Common.IHSModel.PutEncounterPayLoad();

                oData.resourceType = "Encounter";
                oData.id = encounterID;

                IHSModel.EncounterIdentifier oIdentifier = new IHSModel.EncounterIdentifier();
                oIdentifier.system = string.Format("http://sys-ids.kemkes.go.id/encounter/{0}", "10083848");
                oIdentifier.value = oRegistration.RegistrationNo;
                List<IHSModel.EncounterIdentifier> lstIdentifier = new List<IHSModel.EncounterIdentifier>();
                lstIdentifier.Add(oIdentifier);
                oData.identifier = lstIdentifier;

                oData.status = "finished";

                IHSModel.Class oClass = new IHSModel.Class() { system = "http://terminology.hl7.org/CodeSystem/v3-ActCode", code = "AMB", display = "ambulatory" };
                oData.@class = oClass;

                IHSModel.Subject subject = new IHSModel.Subject() { reference = "Patient/P01199742658", display = oRegistration.PatientName };
                oData.subject = subject;

                IHSModel.Participant oParticipant = new IHSModel.Participant();
                IHSModel.Coding coding = new IHSModel.Coding() { code = "ATND", display = "attender", system = "http://terminology.hl7.org/CodeSystem/v3-ParticipationType" };
                List<IHSModel.Coding> lstCoding = new List<IHSModel.Coding>();
                lstCoding.Add(coding);

                IHSModel.Type participantType = new IHSModel.Type();
                participantType.coding = lstCoding;

                List<IHSModel.Type> lstType = new List<IHSModel.Type>();
                lstType.Add(participantType);


                IHSModel.Individual individual = new IHSModel.Individual();
                individual.display = oRegistration.ParamedicName;
                individual.reference = "Practitioner/N10000001";

                oParticipant.type = lstType;
                oParticipant.individual = individual;

                List<IHSModel.Participant> lstParticipant = new List<IHSModel.Participant>();
                lstParticipant.Add(oParticipant);

                oData.participant = lstParticipant;

                IHSModel.Period oPeriod = new IHSModel.Period();
                string startDateTime = string.Format("{0}T{1}:00+07:00", oRegistration.RegistrationDate.ToString("yyyy-MM-dd"), oRegistration.RegistrationTime);
                string finishDateTime = string.Format("{0}T{1}:00+07:00", oRegistration.PhysicianDischargeOrderDate.ToString("yyyy-MM-dd"), oRegistration.PhysicianDischargeOrderTime);

                oPeriod.start = startDateTime;
                oPeriod.end = finishDateTime;
                oData.period = oPeriod;

                IHSModel.SubLocation oSubLocation = new IHSModel.SubLocation();
                oSubLocation.reference = string.Format("Location/{0}", "d649b9e5-5e55-42d1-b53c-a6be15dfcc85");
                oSubLocation.display = string.Format("{0},{1}", oRegistration.ServiceUnitName, oRegistration.DepartmentID);
                IHSModel.Location oLocation = new IHSModel.Location();
                oLocation.location = oSubLocation;
                List<IHSModel.Location> lstLocation = new List<IHSModel.Location>();
                lstLocation.Add(oLocation);
                oData.location = lstLocation;

                List<vPatientDiagnosis1> lstDiagnosis = BusinessLayer.GetvPatientDiagnosis1List(string.Format("VisitID = {0} ORDER BY GCDiagnoseType", oRegistration.VisitID));
                List<IHSModel.Diagnosis> lstIHSDiagnosis = new List<IHSModel.Diagnosis>();

                if (lstDiagnosis.Count>0)
                {
                    int i = 1;
                    foreach (vPatientDiagnosis1 obj  in lstDiagnosis)
	                {
		                IHSModel.Diagnosis oIHSDiagnosis = new IHSModel.Diagnosis();
                        IHSModel.Condition oIHSCondition = new IHSModel.Condition();
                        oIHSCondition.reference = string.Format("Condition/{0}",obj.Remarks.TrimEnd());
                        oIHSCondition.display = obj.DiagnoseName;

                        IHSModel.Use oUse = new IHSModel.Use();
                        IHSModel.Coding oCoding = new IHSModel.Coding();
                        oCoding.system = "http://terminology.hl7.org/CodeSystem/diagnosis-role";
                        oCoding.code = "DD";
                        oCoding.display = "Discharge diagnosis";
                        List<IHSModel.Coding> lstUseCoding = new List<IHSModel.Coding>();
                        lstUseCoding.Add(oCoding);

                        oUse.coding = lstUseCoding;

                        oIHSDiagnosis.condition = oIHSCondition;
                        oIHSDiagnosis.use = oUse;
                        oIHSDiagnosis.rank = i;

                        i++;

                        lstIHSDiagnosis.Add(oIHSDiagnosis);
	                }              
                }

                oData.diagnosis = lstIHSDiagnosis;

                IHSModel.StatusHistory oStatusHistory = new IHSModel.StatusHistory();
                List<IHSModel.StatusHistory> lstStatusHistory = new List<IHSModel.StatusHistory>();

                oStatusHistory.status = "arrived";
                IHSModel.Period oStatusHistoryPeriod = new IHSModel.Period();
                oStatusHistoryPeriod.start = startDateTime;
                oStatusHistoryPeriod.end = finishDateTime;
                oStatusHistory.period = oStatusHistoryPeriod;
                lstStatusHistory.Add(oStatusHistory);
                
                oStatusHistory = new IHSModel.StatusHistory();
                oStatusHistory.status = "in-progress";
                oStatusHistoryPeriod = new IHSModel.Period();
                oStatusHistoryPeriod.start = startDateTime;
                oStatusHistoryPeriod.end = finishDateTime;
                oStatusHistory.period = oStatusHistoryPeriod;
                lstStatusHistory.Add(oStatusHistory);

                oStatusHistory = new IHSModel.StatusHistory();
                oStatusHistory.status = "finished";
                oStatusHistoryPeriod = new IHSModel.Period();
                oStatusHistoryPeriod.start = startDateTime;
                oStatusHistoryPeriod.end = finishDateTime;
                oStatusHistory.period = oStatusHistoryPeriod;
                lstStatusHistory.Add(oStatusHistory);

                oData.statusHistory = lstStatusHistory;

                IHSModel.ServiceProvider oServiceProvider = new IHSModel.ServiceProvider();
                oServiceProvider.reference = string.Format("Organization/{0}", "10083848");

                oData.serviceProvider = oServiceProvider;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Encounter/"+encounterID);

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "PUT";
                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken("I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE", "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4"));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        RegistrationInfo oRegistrationInfo = BusinessLayer.GetRegistrationInfo(oRegistration.RegistrationID);
                        if (oRegistrationInfo != null)
                        {
                            oRegistrationInfo.SyncOutDate = DateTime.Now;
                            oRegistrationInfo.SyncTerminalID =  respInfo.id;
                            BusinessLayer.UpdateRegistrationInfo(oRegistrationInfo);
                        }
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }


        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse GetIHSNumberEncounterList(String ihsNumber)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.BPJS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string result = "";
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = Tls12;

                string base_url = "https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1";
                //string base_url = url;
                Uri uri = new Uri(string.Format("{0}/Encounter?subject={1}", base_url, ihsNumber));
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(uri);
                SetAuthenticationRequest(GETRequest, uri, "Bearer", GenerateToken("uKCKCZuFkkudD1225L8RHFtlH5y6RHQYGDaRjxJJBnE14sk8", "LtQQVc7Cpp9iN1Rsz1cWz9YG60QV0VsaAALxOJFjHHjKfurflqhWHvdyq4bvc7XS"));
                GETRequest.Method = "GET";
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
                Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = sr.ReadToEnd();
                    QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (respInfo.total > 0)
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.total, "");
                        return respInfo;
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", "Informasi kunjungan SATUSEHAT Pasien tidak ditemukan");
                        return new QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse();
                    }
                }
                int status = (int)response.StatusCode;
            }
            catch (WebException ex)
            {
                result = "null";
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        result = string.Format("{0}|{1}|{2}", "0", "null", Constant.BPJS_WS_EXCEPTION.TIMEOUT);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", "null", string.Format("{0} ({1})", ex.Message, ex.Status.ToString()));
                        break;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return new QIS.Medinfras.Web.Common.IHSModel.GetEncounterListResponse();
            }
        }
        #endregion

        #region Condition
        /// <summary>
        /// Post Condition to SATUSEHAT
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PostCondition(string healthcareID, vRegistration oRegistration, string type, int transactionReferenceID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;

                RegistrationInfo oRegInfo = BusinessLayer.GetRegistrationInfo(oRegistration.RegistrationID);

                List<vPatientDiagnosis1> lstDiagnosis = BusinessLayer.GetvPatientDiagnosis1List(string.Format("VisitID = {0}", oRegistration.VisitID));

                if (lstDiagnosis.Count > 0)
                {
                    foreach (vPatientDiagnosis1 oDiagnosis in lstDiagnosis)
                    {
                        #region Convert into DTO Objects
                        QIS.Medinfras.Web.Common.IHSModel.ConditionPayLoad oData = new QIS.Medinfras.Web.Common.IHSModel.ConditionPayLoad();

                        oData.resourceType = "Condition";

                        IHSModel.ClinicalStatus oClinicalStatus = new IHSModel.ClinicalStatus();
                        IHSModel.Coding oCoding = new IHSModel.Coding();
                        oCoding.system = "http://terminology.hl7.org/CodeSystem/condition-clinical";
                        oCoding.code = "active";
                        oCoding.display = "Active";

                        List<IHSModel.Coding> lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oClinicalStatus.coding = lstCoding;

                        oData.clinicalStatus = oClinicalStatus;

                        IHSModel.Category oCategory = new IHSModel.Category();
                        oCoding = new IHSModel.Coding();
                        oCoding.system = "http://terminology.hl7.org/CodeSystem/condition-category";
                        oCoding.code = "encounter-diagnosis";
                        oCoding.display = "Encounter Diagnosis";
                        lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oCategory.coding = lstCoding;
                        List<IHSModel.Category> lstCategory = new List<IHSModel.Category>();
                        lstCategory.Add(oCategory);

                        oData.category = lstCategory;

                        IHSModel.Code oCode = new IHSModel.Code();
                        oCoding = new IHSModel.Coding();
                        oCoding.code = oDiagnosis.DiagnoseID;
                        oCoding.display = oDiagnosis.DiagnoseName;
                        oCoding.system = "http://hl7.org/fhir/sid/icd-10";
                        lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oCode.coding = lstCoding;

                        oData.code = oCode;

                        IHSModel.Subject subject = new IHSModel.Subject() { reference = "Patient/P01199742658", display = oRegistration.PatientName };
                        oData.subject = subject;

                        IHSModel.Encounter encounter = new IHSModel.Encounter();
                        encounter.reference = string.Format("Encounter/{0}", oRegInfo.ExternalRegistrationNo);
                        encounter.display = string.Format("Kunjungan Pasien {0} dengan No. Registrasi Kunjungan {1} pada tanggal {1}", oRegistration.PatientName, oRegistration.RegistrationNo, oRegistration.RegistrationDateInDatePicker);

                        oData.encounter = encounter;
                        #endregion

                        if (oData != null)
                        {
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = Tls12;

                            Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Condition");

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                            request.Method = "POST";
                            SetAuthenticationRequest(request, uri, "Bearer", GenerateToken("I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE", "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4"));

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                            var json = JsonConvert.SerializeObject(oData);
                            byte[] bytes = Encoding.UTF8.GetBytes(json);
                            request.ContentLength = bytes.Length;
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            Stream putStream = request.GetRequestStream();
                            putStream.Write(bytes, 0, bytes.Length);
                            putStream.Close();

                            WebResponse response = (WebResponse)request.GetResponse();
                            string responseMsg = string.Empty;
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseMsg = sr.ReadToEnd();
                            };

                            QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse>(responseMsg);

                            if (!string.IsNullOrEmpty(respInfo.id))
                            {
                                PatientDiagnosis obj = BusinessLayer.GetPatientDiagnosis(oDiagnosis.ID);
                                if (obj != null)
                                {
                                    obj.Remarks = respInfo.id;
                                    BusinessLayer.UpdatePatientDiagnosis(obj);
                                }
                                result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                            }
                            else
                            {
                                result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                        }
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// Post Condition to SATUSEHAT
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="type"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PutCondition(string healthcareID, vRegistration oRegistration, string type, string encounterID)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0132;

                RegistrationInfo oRegInfo = BusinessLayer.GetRegistrationInfo(oRegistration.RegistrationID);

                List<vPatientDiagnosis1> lstDiagnosis = BusinessLayer.GetvPatientDiagnosis1List(string.Format("VisitID = {0}", oRegistration.VisitID));

                if (lstDiagnosis.Count > 0)
                {
                    foreach (vPatientDiagnosis1 oDiagnosis in lstDiagnosis)
                    {
                        #region Convert into DTO Objects
                        QIS.Medinfras.Web.Common.IHSModel.ConditionPayLoad oData = new QIS.Medinfras.Web.Common.IHSModel.ConditionPayLoad();

                        oData.resourceType = "Condition";
                        oData.id = encounterID;

                        IHSModel.ClinicalStatus oClinicalStatus = new IHSModel.ClinicalStatus();
                        IHSModel.Coding oCoding = new IHSModel.Coding();
                        oCoding.system = "http://terminology.hl7.org/CodeSystem/condition-clinical";
                        oCoding.code = "active";
                        oCoding.display = "Active";

                        List<IHSModel.Coding> lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oClinicalStatus.coding = lstCoding;

                        oData.clinicalStatus = oClinicalStatus;

                        IHSModel.Category oCategory = new IHSModel.Category();
                        oCoding = new IHSModel.Coding();
                        oCoding.system = "http://terminology.hl7.org/CodeSystem/condition-category";
                        oCoding.code = "encounter-diagnosis";
                        oCoding.display = "Encounter Diagnosis";
                        lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oCategory.coding = lstCoding;
                        List<IHSModel.Category> lstCategory = new List<IHSModel.Category>();
                        lstCategory.Add(oCategory);

                        oData.category = lstCategory;

                        IHSModel.Code oCode = new IHSModel.Code();
                        oCoding = new IHSModel.Coding();
                        oCoding.code = oDiagnosis.DiagnoseID;
                        oCoding.display = oDiagnosis.DiagnoseName;
                        oCoding.system = "http://hl7.org/fhir/sid/icd-10";
                        lstCoding = new List<IHSModel.Coding>();
                        lstCoding.Add(oCoding);
                        oCode.coding = lstCoding;

                        oData.code = oCode;

                        IHSModel.Subject subject = new IHSModel.Subject() { reference = "Patient/P01199742658", display = oRegistration.PatientName };
                        oData.subject = subject;

                        IHSModel.Encounter encounter = new IHSModel.Encounter();
                        encounter.reference = string.Format("Encounter/{0}", oRegInfo.ExternalRegistrationNo);
                        encounter.display = string.Format("Kunjungan Pasien {0} dengan No. Registrasi Kunjungan {1} pada tanggal {1}", oRegistration.PatientName, oRegistration.RegistrationNo, oRegistration.RegistrationDateInDatePicker);

                        oData.encounter = encounter;
                        #endregion

                        if (oData != null)
                        {
                            ServicePointManager.Expect100Continue = true;
                            ServicePointManager.SecurityProtocol = Tls12;

                            Uri uri = new Uri("https://api-satusehat-dev.dto.kemkes.go.id/fhir-r4/v1/Condition/" + encounterID);

                            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                            request.Method = "PUT";
                            SetAuthenticationRequest(request, uri, "Bearer", GenerateToken("I9lzGrGwgGuD20PosDuecUyHgEhzj7MnWZZYgyz78FuGloXE", "Ahd8AoVwwjR47XslAL8GvA0zVJYyhOdX8hbF29piIG6dNHWgApxy2pdcNnaEjDV4"));

                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                            var json = JsonConvert.SerializeObject(oData);
                            byte[] bytes = Encoding.UTF8.GetBytes(json);
                            request.ContentLength = bytes.Length;
                            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                            Stream putStream = request.GetRequestStream();
                            putStream.Write(bytes, 0, bytes.Length);
                            putStream.Close();

                            WebResponse response = (WebResponse)request.GetResponse();
                            string responseMsg = string.Empty;
                            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                            {
                                responseMsg = sr.ReadToEnd();
                            };

                            QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostEncounterResponse>(responseMsg);

                            if (!string.IsNullOrEmpty(respInfo.id))
                            {
                                PatientDiagnosis obj = BusinessLayer.GetPatientDiagnosis(oDiagnosis.ID);
                                if (obj != null)
                                {
                                    obj.Remarks = respInfo.id;
                                    BusinessLayer.UpdatePatientDiagnosis(obj);
                                }
                                result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                            }
                            else
                            {
                                result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                            }
                        }
                        else
                        {
                            result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                        }
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to 3rd Parties", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }
        #endregion

        #region Immunization
        /// <summary>
        /// pencatatan data imunisasi yang dilaksanakan bersamaan dengan dilakukannya tindakan imunisasi oleh tenaga kesehatan secara langsung pada saat kunjungan
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="oVaccineHistory"></param> 
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PostImmunization1(string healthcareID, vRegistration oRegistration, vVaccinationHistory oVaccineHistory)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0196;
                #region Convert into DTO Objects
                QIS.Medinfras.Web.Common.IHSModel.PostImmunizationPayLoad1 oData = new QIS.Medinfras.Web.Common.IHSModel.PostImmunizationPayLoad1();

                oData.resourceType = Constant.FHIRResourceType.IMMUNIZATION;
                oData.status = "completed";

                IHSModel.CodeReference oVaccineCoding = new IHSModel.CodeReference();
                List<IHSModel.CodeReference> lstCoding = new List<IHSModel.CodeReference>();
                //TODO : if Vaccine combination iterate to list
                //Vaccination KFA Code
                oVaccineCoding.system = Constant.IHSCodingSystemUrl.KFA;
                oVaccineCoding.code = oVaccineHistory.KFACode;
                oVaccineCoding.display = oVaccineHistory.KFAName;
                lstCoding.Add(oVaccineCoding);

                //Vaccination Group Code
                if (!string.IsNullOrEmpty(oVaccineHistory.cfCVXGroup))
                {
                    oVaccineCoding = new IHSModel.CodeReference();
                    oVaccineCoding.system = Constant.IHSCodingSystemUrl.KFA;
                    string[] cvxGroupCode = oVaccineHistory.cfCVXGroup.Split('-');
                    oVaccineCoding.code = cvxGroupCode[0];
                    oVaccineCoding.display = cvxGroupCode[1];
                    lstCoding.Add(oVaccineCoding); 
                }

                //Vaccination Group Code
                if (!string.IsNullOrEmpty(oVaccineHistory.cfCVXName))
                {
                    oVaccineCoding = new IHSModel.CodeReference();
                    oVaccineCoding.system = Constant.IHSCodingSystemUrl.KFA;
                    string[] cvxGroupName = oVaccineHistory.cfCVXName.Split('-');
                    oVaccineCoding.code = cvxGroupName[0];
                    oVaccineCoding.display = cvxGroupName[1];
                    lstCoding.Add(oVaccineCoding);
                }

                IHSModel.VaccineCode oVaccineCode = new IHSModel.VaccineCode();
                oVaccineCode.coding = lstCoding;
                oData.vaccineCode = oVaccineCode;

                IHSModel.Reference oPatientReference = new IHSModel.Reference();
                oPatientReference.reference = string.Format("Patient/{0}", oRegistration.IHSNumber);
                oPatientReference.display = oRegistration.PatientName;
                oData.patient = oPatientReference;

                IHSModel.ResourceReference1 oEncounter = new IHSModel.ResourceReference1();
                oEncounter.reference = string.Format("Encounter/{0}", oRegistration.ExternalRegistrationNo);
                oData.encounter = oEncounter;

                oData.occurrenceDateTime = oVaccineHistory.VaccinationDate.ToString(Constant.FormatString.DATE_FORMAT_3);
                oData.recorded = oVaccineHistory.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT_3);
                oData.primarySource = true;

                IHSModel.Reference oLocationReference = new IHSModel.Reference();
                oLocationReference.reference = string.Format("Location/{0}", oRegistration.ServiceUnitIHSLocationID);
                oLocationReference.display = oRegistration.ServiceUnitName;
                oData.location = oLocationReference;

                oData.lotNumber = oVaccineHistory.BatchNo;

                IHSModel.CodeReference oVaccineRoute = new IHSModel.CodeReference();
                oVaccineRoute.system = Constant.IHSCodingSystemUrl.VaccinationRoute;
                string[] routeInfo = oVaccineHistory.FHIRVaccinationRoute.Split('|');
                oVaccineRoute.code = routeInfo[0];
                oVaccineRoute.display = routeInfo[1];
                List<IHSModel.CodeReference> lstVaccineRoute = new List<IHSModel.CodeReference>();
                lstVaccineRoute.Add(oVaccineRoute);

                IHSModel.VaccineRoute oRoute = new IHSModel.VaccineRoute();
                oRoute.coding = lstVaccineRoute;
                oData.route = oRoute;

                IHSModel.VaccineDoseQuantity oDoseQuantity = new IHSModel.VaccineDoseQuantity();
                string[] doseUnitInfo = oVaccineHistory.FHIRDoseUnit.Split('|');
                oDoseQuantity.value = Convert.ToInt32(oVaccineHistory.Dose);
                oDoseQuantity.unit = doseUnitInfo[1];
                oDoseQuantity.code = doseUnitInfo[0];
                oData.doseQuantity = oDoseQuantity;

                IHSModel.CodeReference oVaccinePerformer = new IHSModel.CodeReference();
                oVaccinePerformer.system = Constant.IHSCodingSystemUrl.VaccinationPerformer;
                oVaccinePerformer.code = "AP";
                oVaccinePerformer.display = "Administering Provider";
                List<IHSModel.CodeReference> lstPerformerFunction = new List<IHSModel.CodeReference>();
                lstPerformerFunction.Add(oVaccinePerformer);
                IHSModel.VaccinePerformerFunction oPerformerFunction = new IHSModel.VaccinePerformerFunction();
                oPerformerFunction.coding = lstPerformerFunction;

                IHSModel.VaccinePerformer oPerformer = new IHSModel.VaccinePerformer();
                oPerformer.function = oPerformerFunction;
                oPerformer.actor = new IHSModel.ResourceReference1() { reference = string.Format("Practitioner/{0}", oRegistration.ParamedicIHSReferenceID) };
                List<IHSModel.VaccinePerformer> lstPerformer = new List<IHSModel.VaccinePerformer>();
                lstPerformer.Add(oPerformer);
                oData.performer = lstPerformer;


                #region this part should be review
                List<IHSModel.CodeReference> lstReasonCode = new List<IHSModel.CodeReference>();
                IHSModel.CodeReference oVaccineReason = new IHSModel.CodeReference();
                oVaccineReason.system = Constant.IHSCodingSystemUrl.VaccinationReason;
                oVaccineReason.code = "IM-Dasar"; //TODO : Should be defined based on vaccination type and sequence number
                oVaccineReason.display = "Imunisasi Program Rutin Dasar";
                lstReasonCode.Add(oVaccineReason);

                IHSModel.CodeReference oVaccineTiming = new IHSModel.CodeReference();
                oVaccineTiming.system = Constant.IHSCodingSystemUrl.VaccinationTiming;
                oVaccineTiming.code = "IM-Ideal"; //TODO : Should be defined based on vaccination type and sequence number
                oVaccineTiming.display = "Imunisasi Ideal";
                lstReasonCode.Add(oVaccineTiming); 
                #endregion

                IHSModel.VaccineReasonCoding oReasonCode = new IHSModel.VaccineReasonCoding();
                oReasonCode.coding = lstReasonCode;
                List<IHSModel.VaccineReasonCoding> lstReasonCoding = new List<IHSModel.VaccineReasonCoding>();
                lstReasonCoding.Add(oReasonCode);
                oData.reasonCode = lstReasonCoding;

                IHSModel.VaccineProtocolApplied oVaccineProtocolApplied = new IHSModel.VaccineProtocolApplied() { doseNumberPositiveInt = oVaccineHistory.SequenceNo };
                List<IHSModel.VaccineProtocolApplied> lstProtocol = new List<IHSModel.VaccineProtocolApplied>();
                lstProtocol.Add(oVaccineProtocolApplied);
                oData.protocolApplied = lstProtocol;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Immunization", url));

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken(AppSession.SA0194, AppSession.SA0195));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

                    var json = JsonConvert.SerializeObject(oData);
                    byte[] bytes = Encoding.UTF8.GetBytes(json);
                    request.ContentLength = bytes.Length;
                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                    Stream putStream = request.GetRequestStream();
                    putStream.Write(bytes, 0, bytes.Length);
                    putStream.Close();

                    WebResponse response = (WebResponse)request.GetResponse();
                    string responseMsg = string.Empty;
                    using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    {
                        responseMsg = sr.ReadToEnd();
                    };

                    QIS.Medinfras.Web.Common.IHSModel.PostImmunizationResponse1 respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostImmunizationResponse1>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        VaccinationHistory oVaccinationHistory = BusinessLayer.GetVaccinationHistory(oVaccineHistory.ID);
                        if (oVaccinationHistory != null)
                        {
                            oVaccinationHistory.IHSReferenceID = respInfo.id;
                            BusinessLayer.UpdateVaccinationHistory(oVaccinationHistory);
                        }
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to SATUSEHAT (IHS)", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }

        /// <summary>
        /// pencatatan data riwayat imunisasi yang dilaporkan pasien atau diketahui oleh tenaga medis pada saat kunjungan
        /// </summary>
        /// <param name="healthcareID"></param>
        /// <param name="oRegistration"></param>
        /// <param name="oVaccineHistory"></param> 
        /// <returns></returns>
        [WebMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string PostImmunization2(string healthcareID, vRegistration oRegistration, vVaccinationHistory oVaccineHistory)
        {
            string result = "";
            try
            {
                string url = AppSession.SA0196;
                #region Convert into DTO Objects
                IHSModel.PostImmunizationPayLoad2 oData = new IHSModel.PostImmunizationPayLoad2();

                oData.resourceType = Constant.FHIRResourceType.IMMUNIZATION;
                oData.status = "completed";

                IHSModel.CodeReference oVaccineCoding = new IHSModel.CodeReference();
                List<IHSModel.CodeReference> lstCoding = new List<IHSModel.CodeReference>();
                //TODO : if Vaccine combination iterate to list
                //Vaccination Group Code
                if (!string.IsNullOrEmpty(oVaccineHistory.cfCVXGroup))
                {
                    oVaccineCoding = new IHSModel.CodeReference();
                    oVaccineCoding.system = Constant.IHSCodingSystemUrl.KFA;
                    string[] cvxGroupCode = oVaccineHistory.cfCVXGroup.Split('-');
                    oVaccineCoding.code = cvxGroupCode[0];
                    oVaccineCoding.display = cvxGroupCode[1];
                    lstCoding.Add(oVaccineCoding);
                }

                //Vaccination Group Code
                if (!string.IsNullOrEmpty(oVaccineHistory.cfCVXName))
                {
                    oVaccineCoding = new IHSModel.CodeReference();
                    oVaccineCoding.system = Constant.IHSCodingSystemUrl.KFA;
                    string[] cvxGroupName = oVaccineHistory.cfCVXName.Split('-');
                    oVaccineCoding.code = cvxGroupName[0];
                    oVaccineCoding.display = cvxGroupName[1];
                    lstCoding.Add(oVaccineCoding);
                }

                IHSModel.VaccineCode oVaccineCode = new IHSModel.VaccineCode();
                oVaccineCode.coding = lstCoding;
                oData.vaccineCode = oVaccineCode;

                IHSModel.Reference oPatientReference = new IHSModel.Reference();
                oPatientReference.reference = string.Format("Patient/{0}", oRegistration.IHSNumber);
                oPatientReference.display = oRegistration.PatientName;
                oData.patient = oPatientReference;

                oData.occurrenceDateTime = oVaccineHistory.VaccinationDate.ToString(Constant.FormatString.DATE_FORMAT_3);
                oData.recorded = oVaccineHistory.CreatedDate.ToString(Constant.FormatString.DATE_FORMAT_3);
                oData.primarySource = true;

                IHSModel.CodeReference oReportOriginCoding = new IHSModel.CodeReference();
                List<IHSModel.CodeReference> lstReportOriginCoding = new List<IHSModel.CodeReference>();
                oReportOriginCoding.system = Constant.IHSCodingSystemUrl.VaccinationReportOrigin;
                oReportOriginCoding.code = "provider";
                oReportOriginCoding.display = "other provider";
                lstReportOriginCoding.Add(oReportOriginCoding);

                IHSModel.ReportOrigin reportOrigin = new IHSModel.ReportOrigin();
                reportOrigin.coding = lstReportOriginCoding;
                oData.reportOrigin = reportOrigin;

                IHSModel.CodeReference oVaccinePerformer = new IHSModel.CodeReference();
                oVaccinePerformer.system = Constant.IHSCodingSystemUrl.VaccinationPerformer;
                oVaccinePerformer.code = "EP";
                oVaccinePerformer.display = "Entering Provider (probably not the same as transcriptionist?)";
                List<IHSModel.CodeReference> lstPerformerFunction = new List<IHSModel.CodeReference>();
                lstPerformerFunction.Add(oVaccinePerformer);
                IHSModel.ExternalVaccinePerformerFunction oPerformerFunction = new IHSModel.ExternalVaccinePerformerFunction();
                oPerformerFunction.coding = lstPerformerFunction;
                oPerformerFunction.text = AppSession.UserLogin.UserFullName;

                IHSModel.ExternalVaccinePerformer oPerformer = new IHSModel.ExternalVaccinePerformer();
                oPerformer.function = oPerformerFunction;
                oPerformer.actor = new IHSModel.ResourceReference1() { reference = string.Format("Organization/{0}", AppSession.SA0197) };
                List<IHSModel.ExternalVaccinePerformer> lstPerformer = new List<IHSModel.ExternalVaccinePerformer>();
                lstPerformer.Add(oPerformer);
                oData.performer = lstPerformer;


                #region this part should be review
                List<IHSModel.CodeReference> lstReasonCode = new List<IHSModel.CodeReference>();
                IHSModel.CodeReference oVaccineReason = new IHSModel.CodeReference();
                oVaccineReason.system = Constant.IHSCodingSystemUrl.VaccinationReason;
                oVaccineReason.code = "IM-Dasar"; //TODO : Should be defined based on vaccination type and sequence number
                oVaccineReason.display = "Imunisasi Program Rutin Dasar";
                lstReasonCode.Add(oVaccineReason);

                IHSModel.CodeReference oVaccineTiming = new IHSModel.CodeReference();
                oVaccineTiming.system = Constant.IHSCodingSystemUrl.VaccinationTiming;
                oVaccineTiming.code = "IM-Ideal"; //TODO : Should be defined based on vaccination type and sequence number
                oVaccineTiming.display = "Imunisasi Ideal";
                lstReasonCode.Add(oVaccineTiming);
                #endregion

                IHSModel.VaccineReasonCoding oReasonCode = new IHSModel.VaccineReasonCoding();
                oReasonCode.coding = lstReasonCode;
                List<IHSModel.VaccineReasonCoding> lstReasonCoding = new List<IHSModel.VaccineReasonCoding>();
                lstReasonCoding.Add(oReasonCode);
                oData.reasonCode = lstReasonCoding;

                IHSModel.VaccineProtocolApplied oVaccineProtocolApplied = new IHSModel.VaccineProtocolApplied() { doseNumberPositiveInt = oVaccineHistory.SequenceNo };
                List<IHSModel.VaccineProtocolApplied> lstProtocol = new List<IHSModel.VaccineProtocolApplied>();
                lstProtocol.Add(oVaccineProtocolApplied);
                oData.protocolApplied = lstProtocol;

                #endregion

                if (oData != null)
                {
                    ServicePointManager.Expect100Continue = true;
                    ServicePointManager.SecurityProtocol = Tls12;

                    Uri uri = new Uri(string.Format("{0}/fhir-r4/v1/Immunization", url));

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    SetAuthenticationRequest(request, uri, "Bearer", GenerateToken(AppSession.SA0194, AppSession.SA0195));

                    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };

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

                    QIS.Medinfras.Web.Common.IHSModel.PostImmunizationResponse2 respInfo = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.PostImmunizationResponse2>(responseMsg);

                    if (!string.IsNullOrEmpty(respInfo.id))
                    {
                        VaccinationHistory oVaccinationHistory = BusinessLayer.GetVaccinationHistory(oVaccineHistory.ID);
                        if (oVaccinationHistory != null)
                        {
                            oVaccinationHistory.IHSReferenceID = respInfo.id;
                            BusinessLayer.UpdateVaccinationHistory(oVaccinationHistory);
                        }
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.id, json);
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "An error occured", json);
                    }
                }
                else
                {
                    result = string.Format("{0}|{1}|{2}", "0", "There is no information to be sent to SATUSEHAT (IHS)", string.Empty);
                }
                return result;
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        result = string.Format("{0}|{1}|{2}", "0", ex.Message, string.Empty);
                        break;
                    default:
                        result = string.Format("{0}|{1}|{2}", "0", string.Format("{0} ({1})", ex.Status.ToString(), string.Empty));
                        break;
                }
                return result;
            }
        }
        #endregion

        #region Utility Function
        private void SetRequestHeaderMedinfrasAPI(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = AppSession.SA0130;
            string pass = AppSession.SA0131;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}{1}", unixTimestamp, consID), pass));
        }

        private string GenerateSignature(string data, string secretKey)
        {
            // Initialize the keyed hash object using the secret key as the key
            HMACSHA256 hashObject = new HMACSHA256(Encoding.UTF8.GetBytes(secretKey));

            // Computes the signature by hashing the salt with the secret key as the key
            var signature = hashObject.ComputeHash(Encoding.UTF8.GetBytes(data));

            // Base 64 Encode
            var encodedSignature = Convert.ToBase64String(signature);

            // URLEncode
            // encodedSignature = System.Web.HttpUtility.UrlEncode(encodedSignature);
            return encodedSignature;
        }
        #endregion

        #region Helper
        public string GenerateToken(string clientID, string clientSecret)
        {
            string result = string.Empty;
            try
            {
                Uri uri = new Uri(string.Format("https://api-satusehat-dev.dto.kemkes.go.id/oauth2/v1/accesstoken?grant_type=client_credentials"));
                HttpWebRequest updateRequest = (HttpWebRequest)WebRequest.Create(uri);
                updateRequest.Method = "POST";
                updateRequest.ContentType = "application/x-www-form-urlencoded";

                String obj = string.Empty;
                obj += string.Format("client_id={0}&", clientID);
                obj += string.Format("client_secret={0}&", clientSecret);

                byte[] bytes = Encoding.UTF8.GetBytes(obj);
                updateRequest.ContentLength = bytes.Length;
                Stream putStream = updateRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();

                WebResponse response = (WebResponse)updateRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    string responseReq = sr.ReadToEnd();
                    QIS.Medinfras.Web.Common.IHSModel.TokenResponse respObj = JsonConvert.DeserializeObject<QIS.Medinfras.Web.Common.IHSModel.TokenResponse>(responseReq);
                    result = respObj.access_token;
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    HttpWebResponse err = ((HttpWebResponse)ex.Response);
                    using (StreamReader sr = new StreamReader(err.GetResponseStream()))
                    {
                        string error = sr.ReadToEnd();

                        result = string.Format("0|{0}", error);
                    }
                }
                else
                {
                    result = string.Format("0|{0}", ex.Message);
                }
            }

            return result;
        }

        public void SetAuthenticationRequest(HttpWebRequest Request, Uri uri, string authType, string authParameter)
        {
            switch (authType)
            {
                case "DIGEST":
                    string[] param = authParameter.Split('|');
                    string username = param[0];
                    string password = param[1];
                    NetworkCredential creds = new NetworkCredential(username, password);
                    CredentialCache credsCache = new CredentialCache();
                    credsCache.Add(uri, authType, creds);
                    Request.PreAuthenticate = true;
                    Request.Credentials = credsCache;
                    break;
                case "Bearer":
                    Request.PreAuthenticate = true;
                    //Request.Headers["Authorization"] = "Bearer " + authParameter;
                    Request.Headers.Add("Authorization", string.Format("Bearer {0}", authParameter));
                    Request.Accept = "application/json";
                    break;
            };
        }
        #endregion
    }
}
