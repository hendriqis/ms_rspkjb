using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Script.Services;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using QIS.Data.Core.Dal;


namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for InhealthService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class InhealthService : System.Web.Services.WebService
    {
        private const string MSG_SUCCESS = "SUCCESS";
        String url = AppSession.SA0132;

        #region 1. CEK RESTRIKSI E-PRESCRIPTION
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CekRestriksiEPrescription(String token, String kodeprovider, String kodeobatrs,
            String user)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamCekRestriksiEPrescription postRequest = new ParamCekRestriksiEPrescription()
            {
                token = token,
                kodeprovider = kodeprovider,
                kodeobatrs = kodeobatrs,
                user = user
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/CekRestriksiEPrescriptions", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultCekRestriksiEPrescriptions resultCekRestriksiEPrescription = JsonConvert.DeserializeObject<ResultCekRestriksiEPrescriptions>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 2. CEK RESTRIKSI TRANSAKSI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CekRestriksiTransaksi(String token, String kodeprovider, String nosjp,
            String kodeobatrs, String jumlahobat, String user)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamCekRestriksiTransaksi postRequest = new ParamCekRestriksiTransaksi()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                kodeobatrs = kodeobatrs,
                jumlahobat = Convert.ToInt32(jumlahobat),
                user = user
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/CekRestriksiTransaksi", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultCekRestriksiTransaksi resultCekRestriksiTransaksi = JsonConvert.DeserializeObject<ResultCekRestriksiTransaksi>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 3. CEK SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CekSJP(String token, String kodeprovider, String nokainhealth,
            String tanggalsjp, String poli, String tkp)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamCekSJP postRequest = new ParamCekSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nokainhealth = nokainhealth,
                tanggalsjp = tanggalsjp,
                poli = poli,
                tkp = tkp
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/CekSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultCekSJP resultSTRITL = JsonConvert.DeserializeObject<ResultCekSJP>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 4. CETAK SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CetakSJP(String token, String kodeprovider, String nosjp,
            String tkp, String tipefile)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamCetakSJP postRequest = new ParamCetakSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                tkp = tkp,
                tipefile = tipefile
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/CetakSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultCetakSJP resultCetakSJP = JsonConvert.DeserializeObject<ResultCetakSJP>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 5. CONFIRM AKT FIRST PAYOR
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ConfirmAKTFirstPayor(String token, String kodeprovider, String nosjp, String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamConfirmAKTFirstPayor postRequest = new ParamConfirmAKTFirstPayor()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                userid = userid
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/ConfirmAKTFirstPayor", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultConfirmAKTFirstPayor resultConfirmAKTFirstPayor = JsonConvert.DeserializeObject<ResultConfirmAKTFirstPayor>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 6. ELIGIBILITAS PESERTA
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object EligibilitasPeserta(String token, String kodeprovider, String nokainhealth,
            String tglpelayanan, String jenispelayanan, String poli)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamEligibilitasPeserta postRequest = new ParamEligibilitasPeserta()
            {
                token = token,
                kodeprovider = kodeprovider,
                nokainhealth = nokainhealth,
                tglpelayanan = tglpelayanan,
                jenispelayanan = jenispelayanan,
                poli = poli
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/EligibilitasPeserta", url));  
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultEligibilitasPeserta resultEP = JsonConvert.DeserializeObject<ResultEligibilitasPeserta>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object EligibilitasPeserta_API(String nokainhealth,
            String tglpelayanan, String jenispelayanan, String poli)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "ELIGIBILITAS",
                Parameter = string.Format("{0}|{1}|{2}|{3}", nokainhealth, tglpelayanan, jenispelayanan, poli)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/peserta", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        ResultEligibilitasPeserta resultEP = JsonConvert.DeserializeObject<ResultEligibilitasPeserta>(respObj.Data);

                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
                
            }
            catch (WebException ex)
            {
                entityAPILog.Response = ex.Message;
                entityAPILog.IsSuccess = false;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 7. HAPUS DETAIL SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusDetailSJP(String token, String kodeprovider, String nosjp,
            String notes, String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamHapusDetailSJP postRequest = new ParamHapusDetailSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                notes = notes,
                userid = userid
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/HapusDetailSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultHapusDetailSJP resultHapusDetailSJP = JsonConvert.DeserializeObject<ResultHapusDetailSJP>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 8. HAPUS OBAT BY KODE OBAT INH
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusObatByKodeObatInh(String token, String kodeprovider, String nosjp,
            String noresep, String kodeobat, String alasan, String user)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamHapusObatByKodeObatInh postRequest = new ParamHapusObatByKodeObatInh()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                noresep = noresep,
                kodeobat = kodeobat,
                alasan = alasan,
                user = user
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/HapusObatByKodeObatInh", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultHapusObatByKodeObatInh resultHapusObatByKodeObatInh = JsonConvert.DeserializeObject<ResultHapusObatByKodeObatInh>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 9. HAPUS OBAT BY KODE OBAT RS
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusOBatByKodeObatRS(String token, String kodeprovider, String nosjp,
            String noresep, String kodeobatrs, String alasan, String user)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamHapusObatBykodeObatRS postRequest = new ParamHapusObatBykodeObatRS()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                noresep = noresep,
                kodeobatrs = kodeobatrs,
                alasan = alasan,
                user = user
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/HapusObatByKodeObatRS", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultHapusObatByKodeObatRS resultHapusObatByKodeObatRS = JsonConvert.DeserializeObject<ResultHapusObatByKodeObatRS>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 10.HAPUS SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusSJP(String token, String kodeprovider, String nosjp,
            String alasanhapus, String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamHapusSJP postRequest = new ParamHapusSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                alasanhapus = alasanhapus,
                userid = userid
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/HapusSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                ResultHapusSJP resultHapusSJP = new ResultHapusSJP();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    resultHapusSJP = JsonConvert.DeserializeObject<ResultHapusSJP>(result);
                }

                IDbContext ctx = DbFactory.Configure(true);
                try
                {
                    #region Update to RegistrationInhealth
                    RegistrationInhealth entityReg = BusinessLayer.GetRegistrationInhealthList(string.Format("NoSJP = '{0}'", resultHapusSJP.NOSJP)).FirstOrDefault();
                    RegistrationInhealthDao entityRegDao = new RegistrationInhealthDao(ctx);

                    if (entityReg != null)
                    {
                        entityReg.NoSJP = string.Empty;
                        entityReg.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityReg.LastUpdatedDate = DateTime.Now;
                        entityRegDao.Update(entityReg);
                    }
                    #endregion

                    #region Update to RegistrationInhealthInpatient
                    RegistrationInhealthInpatient entityRegInp = BusinessLayer.GetRegistrationInhealthInpatientList(string.Format("NoSJP = '{0}'", resultHapusSJP.NOSJP)).FirstOrDefault();
                    RegistrationInhealthInpatientDao entityRegInpDao = new RegistrationInhealthInpatientDao(ctx);

                    if (entityRegInp != null)
                    {
                        entityRegInp.IsDeleted = true;
                        entityRegInp.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityRegInp.LastUpdatedDate = DateTime.Now;
                        entityRegInpDao.Update(entityRegInp);
                    }
                    #endregion

                    #region Update to RegistrationInhealthOutpatient
                    RegistrationInhealthOutpatient entityRegOut = BusinessLayer.GetRegistrationInhealthOutpatientList(string.Format("NoSJP = '{0}'", resultHapusSJP.NOSJP)).FirstOrDefault();
                    RegistrationInhealthOutpatientDao entityRegOutDao = new RegistrationInhealthOutpatientDao(ctx);

                    if (entityRegOut != null)
                    {
                        entityRegOut.IsDeleted = true;
                        entityRegOut.LastUpdatedBy = AppSession.UserLogin.UserID;
                        entityRegOut.LastUpdatedDate = DateTime.Now;
                        entityRegOutDao.Update(entityRegOut);
                    }
                    #endregion

                    ctx.CommitTransaction();
                }
                catch (WebException ex)
                {
                    result = ex.Message;
                    ctx.RollBackTransaction();
                }
                finally
                {
                    ctx.Close();
                }

                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusSJP_API(String nosjp,
            String alasanhapus, String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "HAPUS",
                Parameter = string.Format("{0}|{1}|{2}", nosjp, alasanhapus, userid)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                ResultHapusSJP resultHapusSJP = new ResultHapusSJP();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //resultHapusSJP = JsonConvert.DeserializeObject<ResultHapusSJP>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        resultHapusSJP = JsonConvert.DeserializeObject<ResultHapusSJP>(respObj.Data);

                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.Response = ex.Message;
                entityAPILog.IsSuccess = false;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 11. HAPUS TINDAKAN
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object HapusTindakan(String token, String kodeprovider, String nosjp,
            String kodetindakan, String tgltindakan, String notes,
            String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamHapusTindakan postRequest = new ParamHapusTindakan()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                kodetindakan = kodetindakan,
                tgltindakan = tgltindakan,
                notes = notes,
                userid = userid
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/HapusTindakan", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultHapusTindakan resultHapusTindakan = JsonConvert.DeserializeObject<ResultHapusTindakan>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string HapusTindakan_API(string registrationNo, string date, string lstID, string itemID, string statusSend, string userID, string notes)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "HAPUS_TINDAKAN_BY_FILTER",
                Parameter = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", registrationNo, date, lstID, itemID, statusSend, userID, notes)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultHapusTindakan resultHapusTindakan = JsonConvert.DeserializeObject<ResultHapusTindakan>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 12. INFO BENEFIT
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string InfoBenefit(String token, String kodeprovider, String nokartu, String tanggal,
            String user)
        {
            string result = "";
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamInfoBenefit postRequest = new ParamInfoBenefit()
            {
                token = token,
                kodeprovider = kodeprovider,
                nokartu = nokartu,
                tanggal = tanggal,
                user = user
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/InfoBenefit", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string responseInhealth = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseInhealth = sr.ReadToEnd();
                    result = string.Format("1|{0}", responseInhealth);
                }
                entityAPILog.Response = responseInhealth;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }
            catch (WebException ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            return result;
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string InfoBenefit_API(String nokartu, String tanggal, String user)
        {
            string result = "";
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "INFO_BENEFIT",
                Parameter = string.Format("{0}|{1}|{2}", nokartu, tanggal, user)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/peserta", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string responseInhealth = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseInhealth = sr.ReadToEnd();
                    result = string.Format("1|{0}", responseInhealth);
                }
                entityAPILog.Response = responseInhealth;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);

            }
            catch (WebException ex)
            {
                result = string.Format("0|{0}", ex.Message);
            }
            return result;
        }
        #endregion

        #region 13. INFO SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InfoSJP(String token, String kodeprovider, String nosjp)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamInfoSJP postRequest = new ParamInfoSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/InfoSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                ResultInfoSJP resultInfoSJP = new ResultInfoSJP();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    resultInfoSJP = JsonConvert.DeserializeObject<ResultInfoSJP>(result);
                }

                #region Update to Table RegistrationInhealth
                RegistrationInhealth entityRegInhealth = BusinessLayer.GetRegistrationInhealthList(string.Format("NoSJP = '{0}'", resultInfoSJP.NOSJP)).FirstOrDefault();
                RegistrationInhealthInpatient entityRegInpInhealth = BusinessLayer.GetRegistrationInhealthInpatientList(string.Format("NoSJP = '{0}'", resultInfoSJP.NOSJP)).FirstOrDefault();
                RegistrationInhealthOutpatient entityRegOutInhealth = BusinessLayer.GetRegistrationInhealthOutpatientList(string.Format("NoSJP = '{0}'", resultInfoSJP.NOSJP)).FirstOrDefault();
                if (entityRegInhealth != null)
                {
                    IDbContext ctx = DbFactory.Configure(true);
                    RegistrationInhealthDao entityInhealthDao = new RegistrationInhealthDao(ctx);
                    RegistrationInhealthInpatientDao entityRegInpInhealthDao = new RegistrationInhealthInpatientDao(ctx);
                    RegistrationInhealthOutpatientDao entityRegOutInhealthDao = new RegistrationInhealthOutpatientDao(ctx);

                    try
                    {
                        RegistrationInhealthInpatient entityInhealthInp = new RegistrationInhealthInpatient();
                        RegistrationInhealthOutpatient entityInhealthOut = new RegistrationInhealthOutpatient();

                        #region Insert RegistrationInhealth
                        if (entityRegInhealth != null)
                        {
                            entityRegInhealth.IDAkomodasi = resultInfoSJP.IDAKOMODASI;
                            entityRegInhealth.BiayaAngsuranSJP = Convert.ToDecimal(resultInfoSJP.BYTAGSJP);
                            entityRegInhealth.BiayaVersiSJP = Convert.ToDecimal(resultInfoSJP.BYVERSJP);
                            entityRegInhealth.NoFPK = resultInfoSJP.NOFPK;
                            entityRegInhealth.TKP = resultInfoSJP.TKP;
                            entityInhealthDao.Update(entityRegInhealth);
                        }
                        else
                        {

                        }
                        #endregion

                        #region Insert RegistrationInhealthOutpatient
                        if (resultInfoSJP.LISTINFOTINDAKAN.Count > 0)
                        {
                            if (entityRegOutInhealth == null)
                            {
                                entityInhealthOut.RegistrationID = entityRegInhealth.RegistrationID;
                                entityInhealthOut.NoSJP = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().NOSJP;
                                entityInhealthOut.TanggalRuangRawat = Convert.ToDateTime(resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().TGLRUANGRAWAT);
                                entityInhealthOut.TanggalTindakan = Convert.ToDateTime(resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().TGLTINDAKAN);
                                entityInhealthOut.KodeTindakan = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().KDTINDAKAN;
                                entityInhealthOut.NamaTindakan = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().NMTINDAKAN;
                                entityInhealthOut.KodePoli = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().KDPOLI;
                                entityInhealthOut.NamaPoli = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().NMPOLI;
                                entityInhealthOut.KodeDokter = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().KDDOKTER;
                                entityInhealthOut.NamaDokter = resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().NMDOKTER;
                                entityInhealthOut.BiayaVerif = Convert.ToDecimal(resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().BIAYAVERIF);
                                entityInhealthOut.BiayaAju = Convert.ToDecimal(resultInfoSJP.LISTINFOTINDAKAN.FirstOrDefault().BIAYAAJU);
                                entityRegOutInhealthDao.Insert(entityInhealthOut);
                            }
                        }
                        #endregion

                        #region Insert RegistrationInhealthInpatient
                        if (resultInfoSJP.LISTINFORUANGRAWAT.Count > 0)
                        {
                            if (entityRegInpInhealth == null)
                            {
                                entityInhealthInp.RegistrationID = entityRegInhealth.RegistrationID;
                                entityInhealthInp.NoSJP = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().NOSJP;
                                entityInhealthInp.IDAkomodasi = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().IDAKOMODASI;
                                entityInhealthInp.KodeRuangRawat = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().KDRUANGRAWAT;
                                entityInhealthInp.NamaRuangRawat = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().NMRUANGRAWAT;
                                entityInhealthInp.KodeKelas = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().KDKELAS;
                                entityInhealthInp.NamaKelas = resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().NMKELAS;
                                entityInhealthInp.TanggalMasuk = Convert.ToDateTime(resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().TGLMASUK);
                                entityInhealthInp.TanggalKeluar = Convert.ToDateTime(resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().TGLKELUAR);
                                entityInhealthInp.JumlahHari = Convert.ToInt32(resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().JUMLAHHARI);
                                entityInhealthInp.BiayaRuangRawatPerHari = Convert.ToDecimal(resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().BIAYARUANGRAWATPERHARI);
                                entityInhealthInp.BiayaAju = Convert.ToDecimal(resultInfoSJP.LISTINFORUANGRAWAT.FirstOrDefault().BIAYAAJU);
                                entityRegInpInhealthDao.Insert(entityInhealthInp);
                            }
                        }
                        ctx.CommitTransaction();
                        #endregion
                    }
                    catch (Exception e)
                    {
                        ctx.RollBackTransaction();
                    }
                    finally
                    {
                        ctx.Close();
                    }
                }
                #endregion

                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object InfoSJP_API(String nosjp)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "INFO",
                Parameter = string.Format("{0}|{1}", nosjp, AppSession.UserLogin.UserID)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                ResultInfoSJP resultInfoSJP = new ResultInfoSJP();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        resultInfoSJP = JsonConvert.DeserializeObject<ResultInfoSJP>(respObj.Data);
                        if (resultInfoSJP != null)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 14. POLI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object Poli(String token, String kodeprovider, String keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamPoli postRequest = new ParamPoli()
            {
                token = token,
                kodeprovider = kodeprovider,
                keyword = keyword
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/Poli", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    List<ResultPoli> resultPoli = JsonConvert.DeserializeObject<List<ResultPoli>>(result);

                    if (resultPoli.Count > 0)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        InhealthReferenceDao entityDao = new InhealthReferenceDao(ctx);
                        InhealthReference entity = new InhealthReference();
                        string status = string.Empty;

                        try
                        {                           
                            for (int i = 0; i < resultPoli.Count(); i++)
                            {
                                List<vInhealthReferencePoli> lstPoliCheck = BusinessLayer.GetvInhealthReferencePoliList(string.Format("ObjectCode = '{0}' AND ObjectName = '{1}'", resultPoli[i].KDPOLI, resultPoli[i].NMPOLI));
                                if (lstPoliCheck.Count == 0)
                                {
                                    entity.GCInhealthObjectType = Constant.StandardCode.INHEALTH_REFERENCES + "^001";
                                    entity.ObjectCode = resultPoli[i].KDPOLI;
                                    entity.ObjectName = resultPoli[i].NMPOLI;
                                    entity.Others = resultPoli[i].KDPOLI + "|" + resultPoli[i].NMPOLI + "|" + resultPoli[i].STATUS;
                                    entity.Notes = "KDPROVIDER|NMPROVIDER|STATUS";
                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                    entity.CreatedDate = DateTime.Now;
                                    entityDao.Insert(entity);
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                    else
                    {
                        result = "Tidak ada data poli.";
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object Poli_API(String keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "POLI",
                Parameter = string.Format("{0}|{1}", keyword, AppSession.UserLogin.UserID)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/referensi", url));
                request.Method = "POST";
                SetRequestHeaderMedinfrasAPI(request);
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        List<ResultPoli> lstPoli = JsonConvert.DeserializeObject<List<ResultPoli>>(respObj.Data);
                        if (lstPoli.Count > 0)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 15. PROSES SJP TO FPK
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ProsesSJPToFPK(String token, String kodeprovider, String jeniscob, String jenispelayanan,
            String listnosjp, String namapicprovider, String noinvoiceprovider, String username)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSJPtoFPK postRequest = new ParamSJPtoFPK()
            {
                token = token,
                kodeprovider = kodeprovider,
                jeniscob = jeniscob,
                jenispelayanan = jenispelayanan,
                listnosjp = listnosjp,
                namapicprovider = namapicprovider,
                noinvoiceprovider = noinvoiceprovider,
                username = username
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/ProsesSJPtoFPK", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSJPtoFPK resultProsesSJPToFPK = JsonConvert.DeserializeObject<ResultSJPtoFPK>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 16. PROVIDER RUJUKAN
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ProviderRujukan(String token, String kodeprovider, String keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamProviderRujukan postRequest = new ParamProviderRujukan()
            {
                token = token,
                kodeprovider = kodeprovider,
                keyword = keyword
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/ProviderRujukan", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    List<ResultProviderRujukan> resultProviderRujukan = JsonConvert.DeserializeObject<List<ResultProviderRujukan>>(result);

                    if (resultProviderRujukan.Count > 0)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        InhealthReferenceDao entityDao = new InhealthReferenceDao(ctx);
                        InhealthReference entity = new InhealthReference();

                        try
                        {
                            for (int i = 0; i < resultProviderRujukan.Count(); i++)
                            {
                                List<vInhealthReferenceProviderRujukan> lstProviderCheck = BusinessLayer.GetvInhealthReferenceProviderRujukanList(string.Format("ObjectCode = '{0}' AND ObjectName = '{1}'", resultProviderRujukan[i].KDPROVIDER, resultProviderRujukan[i].NMPROVIDER));
                                if (lstProviderCheck.Count == 0)
                                {
                                    entity.GCInhealthObjectType = Constant.StandardCode.INHEALTH_REFERENCES + "^002";
                                    entity.ObjectCode = resultProviderRujukan[i].KDPROVIDER;
                                    entity.ObjectName = resultProviderRujukan[i].NMPROVIDER;
                                    entity.Others = resultProviderRujukan[i].LOKASI;
                                    entity.Notes = "LOKASI";
                                    entity.CreatedBy = AppSession.UserLogin.UserID;
                                    entity.CreatedDate = DateTime.Now;
                                    entityDao.Insert(entity);
                                }
                            }
                            ctx.CommitTransaction();
                        }
                        catch (Exception ex)
                        {
                            result = ex.Message;
                            ctx.RollBackTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object ProviderRujukan_API(String keyword)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "PROVIDER_RUJUKAN",
                Parameter = string.Format("{0}|{1}", keyword, AppSession.UserLogin.UserID)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/referensi", url));
                request.Method = "POST";
                SetRequestHeaderMedinfrasAPI(request);
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        List<ResultProviderRujukan> lstPoli = JsonConvert.DeserializeObject<List<ResultProviderRujukan>>(respObj.Data);
                        if (lstPoli.Count > 0)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 17. REKAP HASIL VERIFIKASI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object RekapHasilVerifikasi(String token, String kodeprovider, String nofpk)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamRekapHasilVerifikasi postRequest = new ParamRekapHasilVerifikasi()
            {
                token = token,
                kodeprovider = kodeprovider,
                nofpk = nofpk
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/RekapHasilVerifikasi", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultRekapHasilVerifikasi resultRekapHasilVerifikasi = JsonConvert.DeserializeObject<ResultRekapHasilVerifikasi>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 18. SIMPAN BIAYA INACBGS
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanBiayaINACBGS(String token, String kodeprovider, String nosjp,
            String kodeinacbg, String biayainacbg, String nosep,
            String notes, String userid)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanBiayaINACBGS postRequest = new ParamSimpanBiayaINACBGS()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                kodeinacbg = kodeinacbg,
                biayainacbg = Convert.ToInt32(biayainacbg),
                nosep = nosep,
                notes = notes,
                userid = userid
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanBiayaINACBGS", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanBiayaINACBGS resultSimpanBiayaINACBGS = JsonConvert.DeserializeObject<ResultSimpanBiayaINACBGS>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 19. SIMPAN OBAT
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanObat(String token, String kodeprovider, String nosjp,
            String noresep, String tanggalresep, String tanggalobat, String tipeobat,
            String jenisracikan, String kodeobatrs, String namaobat, String kodedokter,
            String jumlahobat, String signa1, String signa2, String jumlahhari,
            String hdasar, String confirmationcode, String username)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanObat postRequest = new ParamSimpanObat()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                noresep = noresep,
                tanggalresep = tanggalresep,
                tanggalobat = tanggalobat,
                tipeobat = tipeobat,
                jenisracikan = jenisracikan,
                kodeobatrs = kodeobatrs,
                namaobat = namaobat,
                kodedokter = kodedokter,
                jumlahobat = Convert.ToInt32(jumlahobat),
                signa1 = Convert.ToInt32(signa1),
                signa2 = Convert.ToInt32(signa2),
                jumlahhari = Convert.ToInt32(jumlahhari),
                hdasar = Convert.ToInt32(hdasar),
                confirmationcode = confirmationcode,
                username = username
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanObat", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanObat resultSimpanObat = JsonConvert.DeserializeObject<ResultSimpanObat>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 20. SIMPAN RUANG RAWAT
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanRuangRawat(String token, String kodeprovider, String nosjp,
            String tglmasuk, String kelasrawat, String kodejenispelayanan, String byharirawat)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanRuangRawat postRequest = new ParamSimpanRuangRawat()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                tglmasuk = tglmasuk,
                kelasrawat = kelasrawat,
                kodejenispelayanan = kodejenispelayanan,
                byharirawat = Convert.ToInt32(byharirawat)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanRuangRawat", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanRuangRawat resultSRR = JsonConvert.DeserializeObject<ResultSimpanRuangRawat>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 21. SIMPAN SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanSJP(String token, String kodeprovider, String tanggalpelayanan,
            String jenispelayanan, String nokainhealth, String nomormedicalreport,
            String nomorasalrujukan, String kodeproviderasalrujukan, String tanggalasalrujukan,
            String kodediagnosautama, String poli, String username,
            String informasitambahan, String kodediagnosatambahan, String kecelakaankerja,
            String kelasrawat, String kodejenpelruangrawat)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanSJP postRequest = new ParamSimpanSJP()
            {
                token = token, 
                kodeprovider = kodeprovider,
                tanggalpelayanan = tanggalpelayanan,
                jenispelayanan = jenispelayanan,
                nokainhealth = nokainhealth,
                nomormedicalreport = nomormedicalreport,
                nomorasalrujukan = nomorasalrujukan,
                kodeproviderasalrujukan = kodeproviderasalrujukan,
                tanggalasalrujukan = tanggalasalrujukan,
                kodediagnosautama = kodediagnosautama,
                poli = poli,
                username = username,
                informasitambahan = informasitambahan,
                kodediagnosatambahan = kodediagnosatambahan,
                kecelakaankerja = Convert.ToInt32(kecelakaankerja),
                kelasrawat = kelasrawat,
                kodejenpelruangrawat = kodejenpelruangrawat
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanSJP resultEP = JsonConvert.DeserializeObject<ResultSimpanSJP>(result);
                }

                #region insert to table RegistrationInhealth

                #endregion

                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanSJP_API(String tanggalpelayanan,
            String jenispelayanan, String nokainhealth, String nomormedicalreport,
            String nomorasalrujukan, String kodeproviderasalrujukan, String tanggalasalrujukan,
            String kodediagnosautama, String poli, String username,
            String informasitambahan, String kodediagnosatambahan, String kecelakaankerja,
            String kelasrawat, String kodejenpelruangrawat)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "SIMPAN",
                Parameter = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}", tanggalpelayanan, jenispelayanan, nokainhealth, nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan, kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan, kecelakaankerja, kelasrawat, kodejenpelruangrawat)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultSimpanSJP resultEP = JsonConvert.DeserializeObject<ResultSimpanSJP>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        ResultSimpanSJP resultEP = JsonConvert.DeserializeObject<ResultSimpanSJP>(respObj.Data);
                        if (resultEP != null)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanSJPV2_API(String tanggalpelayanan,
            String jenispelayanan, String nokainhealth, String nomormedicalreport,
            String nomorasalrujukan, String kodeproviderasalrujukan, String tanggalasalrujukan,
            String kodediagnosautama, String poli, String username,
            String informasitambahan, String kodediagnosatambahan, String kecelakaankerja,
            String kelasrawat, String kodejenpelruangrawat, String nohp, String email, String claimidprovider)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "SIMPAN_V2",
                Parameter = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}|{16}|{17}", tanggalpelayanan, jenispelayanan, nokainhealth, nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan, kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan, kecelakaankerja, kelasrawat, kodejenpelruangrawat, nohp, email, claimidprovider)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultSimpanSJP resultEP = JsonConvert.DeserializeObject<ResultSimpanSJP>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        ResultSimpanSJP resultEP = JsonConvert.DeserializeObject<ResultSimpanSJP>(respObj.Data);
                        if (resultEP != null)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 22. SIMPAN TINDAKAN
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanTindakan(String token, String kodeprovider, String jenispelayanan,
            String nosjp, String tglmasukrawat, String tanggalpelayanan, 
            String kodetindakan, String poli, String kodedokter, String biayaaju)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanTindakan postRequest = new ParamSimpanTindakan()
            {
                token = token,
                kodeprovider = kodeprovider,
                jenispelayanan = jenispelayanan,
                nosjp = nosjp,
                tglmasukrawat = tglmasukrawat,
                tanggalpelayanan = tanggalpelayanan,
                kodetindakan = kodetindakan,
                poli = poli,
                kodedokter = kodedokter,
                biayaaju = Convert.ToInt32(biayaaju)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanTindakan", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanTindakan resultST = JsonConvert.DeserializeObject<ResultSimpanTindakan>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanTindakan_API(String jenispelayanan,
            String nosjp, String tglmasukrawat, String tanggalpelayanan,
            String kodetindakan, String poli, String kodedokter, String biayaaju)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "SIMPAN_TINDAKAN",
                Parameter = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}", jenispelayanan, nosjp, tglmasukrawat, tanggalpelayanan, kodetindakan, poli, kodedokter, biayaaju)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultSimpanTindakan resultST = JsonConvert.DeserializeObject<ResultSimpanTindakan>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        ResultSimpanTindakan resultEP = JsonConvert.DeserializeObject<ResultSimpanTindakan>(respObj.Data);
                        if (resultEP != null)
                        {
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                        }
                        else
                        {
                            entityAPILog.IsSuccess = false;
                            BusinessLayer.InsertAPIMessageLog(entityAPILog);
                            return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                        }
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SimpanTindakanByFilter_API(string registrationNo, string date, string lstID, string itemID, string statusSend, string userID)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "SIMPAN_TINDAKAN_BY_FILTER",
                Parameter = string.Format("{0}|{1}|{2}|{3}|{4}|{5}", registrationNo, date, lstID, itemID, statusSend, userID)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultSimpanTindakan resultST = JsonConvert.DeserializeObject<ResultSimpanTindakan>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region 23. SIMPAN TINDAKAN RITL
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object SimpanTindakanRITL(String token, String kodeprovider, String jenispelayanan,
            String nosjp, String idakomodasi, String tglmasukrawat,
            String tanggalpelayanan, String kodetindakan, String poli,
            String kodedokter, String biayaaju)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamSimpanTindakanRITL postRequest = new ParamSimpanTindakanRITL()
            {
                token = token,
                kodeprovider = kodeprovider,
                jenispelayanan = jenispelayanan,
                nosjp = nosjp,
                idakomodasi = idakomodasi,
                tglmasukrawat = tglmasukrawat,
                tanggalpelayanan = tanggalpelayanan,
                kodetindakan = kodetindakan,
                poli = poli,
                kodedokter = kodedokter,
                biayaaju = Convert.ToInt32(biayaaju)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/SimpanTindakanRITL", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultSimpanTindakanRITL resultSTRITL = JsonConvert.DeserializeObject<ResultSimpanTindakanRITL>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 24. UPDATE SJP
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateSJP(String token, String kodeprovider, String nosjp, String nomormedicalreport,
            String nomorasalrujukan, String kodeproviderasalrujukan, String tanggalasalrujukan, String kodediagnosautama,
            String poli, String username, String informasitambahan, String kodediagnosatambahan, String kecelakaankerja)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamUpdateSJP postRequest = new ParamUpdateSJP()
            {
                token = token,
                kodeprovider = kodeprovider,
                nosjp = nosjp,
                nomormedicalreport = nomormedicalreport,
                nomorasalrujukan = nomorasalrujukan,
                kodeproviderasalrujukan = kodeproviderasalrujukan,
                tanggalasalrujukan = tanggalasalrujukan,
                poli = poli,
                username = username,
                informasitambahan = informasitambahan,
                kodediagnosatambahan = kodediagnosatambahan,
                kecelakaankerja = Convert.ToInt32(kecelakaankerja)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/UpdateSJP", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultUpdateSJP resultUpdateSJP = JsonConvert.DeserializeObject<ResultUpdateSJP>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region 25. UPDATE TANGGAL PULANG
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateTanggalPulang(String token, String kodeprovider, String id,
            String nosjp, String tglmasuk, String tglkeluar)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.INHEALTH,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            ParamUpdateTanggalPulang postRequest = new ParamUpdateTanggalPulang()
            {
                token = token,
                kodeprovider = kodeprovider,
                id = Convert.ToInt32(id),
                nosjp = nosjp,
                tglmasuk = tglmasuk,
                tglkeluar = tglkeluar
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/api/UpdateTanggalPulang", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    ResultUpdateTanggalPulang resultUpdateTanggalPulang = JsonConvert.DeserializeObject<ResultUpdateTanggalPulang>(result);
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string UpdateTanggalPulang_API(String id, String nosjp, String tglmasuk, String tglkeluar)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "UPDATE_TANGGAL_PULANG",
                Parameter = string.Format("{0}|{1}|{2}|{3}", id, nosjp, tglmasuk, tglkeluar)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/sjp", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultUpdateTanggalPulang resultUpdateTanggalPulang = JsonConvert.DeserializeObject<ResultUpdateTanggalPulang>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }
        #endregion

        #region PRA REGISTRASI
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string ListPraRegistrasi(String tanggal, String query)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "LIST",
                Parameter = string.Format("{0}|{1}", tanggal, query)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/pra-registrasi", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultUpdateTanggalPulang resultUpdateTanggalPulang = JsonConvert.DeserializeObject<ResultUpdateTanggalPulang>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string DetailPraRegistrasi(String nomorpraregistrasi)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.MEDINFRAS_API,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            MedinfrasAPI_BodyRequest postRequest = new MedinfrasAPI_BodyRequest()
            {
                Type = "DETAIL",
                Parameter = string.Format("{0}", nomorpraregistrasi)
            };

            string data = JsonConvert.SerializeObject(postRequest);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/inhealth/pra-registrasi", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeaderMedinfrasAPI(request);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                request.ContentLength = bytes.Length;
                Stream putStream = request.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)request.GetResponse();
                string result = "";
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    //result = sr.ReadToEnd();
                    //ResultUpdateTanggalPulang resultUpdateTanggalPulang = JsonConvert.DeserializeObject<ResultUpdateTanggalPulang>(result);

                    result = sr.ReadToEnd();
                    entityAPILog.Response = result;
                    MedinfrasAPI_Response respObj = JsonConvert.DeserializeObject<MedinfrasAPI_Response>(result);

                    if (respObj.Status == "SUCCESS")
                    {
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("1|{0}|{1}", respObj.Remarks, respObj.Data);
                    }
                    else
                    {
                        entityAPILog.IsSuccess = false;
                        BusinessLayer.InsertAPIMessageLog(entityAPILog);
                        return string.Format("0|{0}|{1}", respObj.Remarks, string.Empty);
                    }
                }
            }
            catch (WebException ex)
            {
                entityAPILog.IsSuccess = false;
                entityAPILog.Response = ex.Message;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("0|{0}|{1}", ex.Message, string.Empty);
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

    }
}
