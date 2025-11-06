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

namespace QIS.Medinfras.Web.CommonLibs.Service
{
    /// <summary>
    /// Summary description for KemenKesService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class KemenKesService : System.Web.Services.WebService
    {
        private string url = AppSession.SA0128;

        #region Entry Data Pasien

        #region RekapPasienMasuk

        #region GetRekapPasienMasuk
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRekapPasienMasuk()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienMasuk", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    entityAPILog.Response = resp;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (resp != null)
                    {
                        result = "1|" + resp;
                    }
                    else {
                        result = "0|" + "tidak ada data";
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                resp = ex.Message;
                result = "0|" + resp; 
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region PostRekapPasienMasuk
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PostRekapPasienMasuk(string tanggal, string igd_suspect_l, string igd_suspect_p, string igd_confirm_l, string igd_confirm_p, string rj_suspect_l,
            string rj_suspect_p, string rj_confirm_l, string rj_confirm_p, string ri_suspect_l, string ri_suspect_p, string ri_confirm_l, string ri_confirm_p)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            RekapPasienMasukPost dataPost = new RekapPasienMasukPost()
            { 
                tanggal = tanggal,
                igd_suspect_l = igd_suspect_l,
                igd_suspect_p = igd_suspect_p,
                igd_confirm_l = igd_confirm_l,
                igd_confirm_p = igd_confirm_p,
                rj_suspect_l = rj_suspect_l,
                rj_suspect_p = rj_suspect_p,
                rj_confirm_l = rj_confirm_l,
                rj_confirm_p = rj_confirm_p,
                ri_suspect_l = ri_suspect_l,
                ri_suspect_p = ri_suspect_p,
                ri_confirm_l = ri_confirm_l,
                ri_confirm_p = ri_confirm_p
            };

            string data = JsonConvert.SerializeObject(dataPost);
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                //HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienMasuk", url));

                //GETRequest.Method = "GET";
                //SetRequestHeader(GETRequest);
                //WebResponse response = (WebResponse)GETRequest.GetResponse();
                //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //{
                //    result = sr.ReadToEnd();
                //    RekapPasienMasukRespon respInfo = JsonConvert.DeserializeObject<RekapPasienMasukRespon>(result);
                //    if (respInfo.RekapPasienMasuk.status == "200")
                //    {
                //        result = string.Format("{0}", respInfo.RekapPasienMasuk.message);
                //    }
                //    else
                //    {
                //        result = string.Format("{0}", respInfo.RekapPasienMasuk.message);
                //    }
                //}
                //entityAPILog.Response = result;

                HttpWebRequest PostRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienMasuk", url));
                PostRequest.Method = "POST";
                PostRequest.ContentType = "application/json";
                SetRequestHeader(PostRequest);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                PostRequest.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = PostRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)PostRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    result = string.Format("1|{0}", resp);
                }
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                resp = ex.Message;
                result = string.Format("0|{0}", resp);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region DeleteRekapPasienMasuk
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteRekapPasienMasuk(string tanggal)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            RekapPasienMasukPost dataPost = new RekapPasienMasukPost()
            {
                tanggal = tanggal,
                igd_suspect_l = "0",
                igd_suspect_p = "0",
                igd_confirm_l = "0",
                igd_confirm_p = "0",
                rj_suspect_l = "0",
                rj_suspect_p = "0",
                rj_confirm_l = "0",
                rj_confirm_p = "0",
                ri_suspect_l = "0",
                ri_suspect_p = "0",
                ri_confirm_l = "0",
                ri_confirm_p = "0"
            };

            string data = JsonConvert.SerializeObject(dataPost);
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                /*HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienMasuk", url));
                GETRequest.Method = "POST";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    RekapPasienMasukRespon respInfo = JsonConvert.DeserializeObject<RekapPasienMasukRespon>(result);
                    if (respInfo.RekapPasienMasuk.status == "200")
                    {
                        result = string.Format("{0}", respInfo.RekapPasienMasuk.message);
                    }
                    else
                    {
                        result = string.Format("{0}", respInfo.RekapPasienMasuk.message);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);*/

                HttpWebRequest PostRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienMasuk", url));
                PostRequest.Method = "DELETE";
                PostRequest.ContentType = "application/json";
                SetRequestHeader(PostRequest);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                PostRequest.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = PostRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)PostRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    result = string.Format("1|{0}", resp);
                }
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
               
            }
            catch (WebException ex)
            {
                resp = string.Format("0|{0}", ex.Message);
                result = resp;
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #endregion

        #region PasienDirawatdenganKomorbid
        #region GetPasienDirawatdenganKomorbid
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPasienDirawatdenganKomorbid()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienDirawatKomorbid", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    entityAPILog.Response = resp;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        result = string.Format("1|{0}", resp);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                resp = ex.Message;
                result = string.Format("0|{0}", resp);
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region PostPasienDirawatdenganKomorbid
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PostPasienDirawatdenganKomorbid(string tanggal, string icu_dengan_ventilator_suspect_l, string icu_dengan_ventilator_suspect_p,
                string icu_dengan_ventilator_confirm_l, string icu_dengan_ventilator_confirm_p, string icu_tanpa_ventilator_suspect_l,
                string icu_tanpa_ventilator_suspect_p, string icu_tanpa_ventilator_confirm_l, string icu_tanpa_ventilator_confirm_p,
                string icu_tekanan_negatif_dengan_ventilator_suspect_l, string icu_tekanan_negatif_dengan_ventilator_suspect_p,
                string icu_tekanan_negatif_dengan_ventilator_confim_l, string icu_tekanan_negatif_dengan_ventilator_confim_p,
                string icu_tekanan_negatif_tanpa_ventilator_suspect_l, string icu_tekanan_negatif_tanpa_ventilator_suspect_p,
                string icu_tekanan_negatif_tanpa_ventilator_confirm_l, string icu_tekanan_negatif_tanpa_ventilator_confirm_p,
                string isolasi_tekanan_negatif_suspect_l, string isolasi_tekanan_negatif_suspect_p, string isolasi_tekanan_negatif_confirm_l, string isolasi_tekanan_negatif_confirm_p,
                string isolasi_tanpa_tekanan_negatif_suspect_l, string isolasi_tanpa_tekanan_negatif_suspect_p, string isolasi_tanpa_tekanan_negatif_confirm_l,
                string isolasi_tanpa_tekanan_negatif_confirm_p, string nicu_khusus_covid_suspect_l, string nicu_khusus_covid_suspect_p, string nicu_khusus_covid_confirm_l,
                string nicu_khusus_covid_confirm_p, string picu_khusus_covid_suspect_l, string picu_khusus_covid_suspect_p, string picu_khusus_covid_confirm_l, 
                string picu_khusus_covid_confirm_p)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            PasienDirawatdenganKomorbidPost test = new PasienDirawatdenganKomorbidPost()
            {
               tanggal = tanggal,
               icu_dengan_ventilator_suspect_l = icu_dengan_ventilator_suspect_l,
               icu_dengan_ventilator_suspect_p = icu_dengan_ventilator_suspect_p,
               icu_dengan_ventilator_confirm_l =icu_dengan_ventilator_confirm_l,
               icu_dengan_ventilator_confirm_p = icu_dengan_ventilator_confirm_p,
               icu_tanpa_ventilator_suspect_l = icu_tanpa_ventilator_suspect_l,
               icu_tanpa_ventilator_suspect_p = icu_tanpa_ventilator_suspect_p,
               icu_tanpa_ventilator_confirm_l = icu_tanpa_ventilator_confirm_l,
               icu_tanpa_ventilator_confirm_p = icu_tanpa_ventilator_confirm_p,
               icu_tekanan_negatif_dengan_ventilator_suspect_l = icu_tekanan_negatif_dengan_ventilator_suspect_l,
               icu_tekanan_negatif_dengan_ventilator_suspect_p = icu_tekanan_negatif_dengan_ventilator_suspect_p,
               icu_tekanan_negatif_dengan_ventilator_confirm_l = icu_tekanan_negatif_dengan_ventilator_confim_l,
               icu_tekanan_negatif_dengan_ventilator_confirm_p = icu_tekanan_negatif_dengan_ventilator_confim_p,
               icu_tekanan_negatif_tanpa_ventilator_suspect_l = icu_tekanan_negatif_tanpa_ventilator_suspect_l,
               icu_tekanan_negatif_tanpa_ventilator_suspect_p = icu_tekanan_negatif_tanpa_ventilator_suspect_p,
               icu_tekanan_negatif_tanpa_ventilator_confirm_l = icu_tekanan_negatif_tanpa_ventilator_confirm_l,
               icu_tekanan_negatif_tanpa_ventilator_confirm_p = icu_tekanan_negatif_tanpa_ventilator_confirm_p,
               isolasi_tekanan_negatif_suspect_l = isolasi_tekanan_negatif_suspect_l,
               isolasi_tekanan_negatif_suspect_p = isolasi_tekanan_negatif_suspect_p,
               isolasi_tekanan_negatif_confirm_l = isolasi_tekanan_negatif_confirm_l,
               isolasi_tekanan_negatif_confirm_p = isolasi_tekanan_negatif_confirm_p,
               isolasi_tanpa_tekanan_negatif_suspect_l = isolasi_tanpa_tekanan_negatif_suspect_l,
               isolasi_tanpa_tekanan_negatif_suspect_p = isolasi_tanpa_tekanan_negatif_suspect_p,
               isolasi_tanpa_tekanan_negatif_confirm_l = isolasi_tanpa_tekanan_negatif_confirm_l,
               isolasi_tanpa_tekanan_negatif_confirm_p = isolasi_tanpa_tekanan_negatif_confirm_p,
               nicu_khusus_covid_suspect_l = nicu_khusus_covid_suspect_l,
               nicu_khusus_covid_suspect_p = nicu_khusus_covid_suspect_p,
               nicu_khusus_covid_confirm_l = nicu_khusus_covid_confirm_l,
               nicu_khusus_covid_confirm_p = nicu_khusus_covid_confirm_p,
               picu_khusus_covid_suspect_l = picu_khusus_covid_suspect_l,
               picu_khusus_covid_suspect_p = picu_khusus_covid_suspect_p,
               picu_khusus_covid_confirm_l = picu_khusus_covid_confirm_l,
               picu_khusus_covid_confirm_p = picu_khusus_covid_confirm_p
            };

            string data = JsonConvert.SerializeObject(test);
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest PostRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienDirawatKomorbid", url));
                PostRequest.Method = "POST";
                PostRequest.ContentType = "application/json";
                SetRequestHeader(PostRequest);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                PostRequest.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = PostRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)PostRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    result = string.Format("1|{0}", resp);
                }
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                resp = ex.Message;
                result = string.Format("0|{0}", resp);

                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #endregion

        #region PasienDirawattanpaKomorbid

        #region GetPasienDirawattanpaKomorbid
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPasienDirawattanpaKomorbid()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienDirawatTanpaKomorbid", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    entityAPILog.Response = resp;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        result = string.Format("1|{0}", resp);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
               
                result = string.Format("0|{0}", ex.Message);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region PostPasienDirawattanpaKomorbid
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PostPasienDirawattanpaKomorbid(string tanggal, string icu_dengan_ventilator_suspect_l, string icu_dengan_ventilator_suspect_p,
                string icu_dengan_ventilator_confirm_l, string icu_dengan_ventilator_confirm_p, string icu_tanpa_ventilator_suspect_l,
                string icu_tanpa_ventilator_suspect_p, string icu_tanpa_ventilator_confirm_l, string icu_tanpa_ventilator_confirm_p,
                string icu_tekanan_negatif_dengan_ventilator_suspect_l, string icu_tekanan_negatif_dengan_ventilator_suspect_p,
                string icu_tekanan_negatif_dengan_ventilator_confim_l, string icu_tekanan_negatif_dengan_ventilator_confim_p,
                string icu_tekanan_negatif_tanpa_ventilator_suspect_l, string icu_tekanan_negatif_tanpa_ventilator_suspect_p,
                string icu_tekanan_negatif_tanpa_ventilator_confirm_l, string icu_tekanan_negatif_tanpa_ventilator_confirm_p,
                string isolasi_tekanan_negatif_suspect_l, string isolasi_tekanan_negatif_suspect_p, string isolasi_tekanan_negatif_confirm_l, string isolasi_tekanan_negatif_confirm_p, 
                string isolasi_tanpa_tekanan_negatif_suspect_l, string isolasi_tanpa_tekanan_negatif_suspect_p, string isolasi_tanpa_tekanan_negatif_confirm_l,
                string isolasi_tanpa_tekanan_negatif_confirm_p, string nicu_khusus_covid_suspect_l, string nicu_khusus_covid_suspect_p, string nicu_khusus_covid_confirm_l,
                string nicu_khusus_covid_confirm_p, string picu_khusus_covid_suspect_l, string picu_khusus_covid_suspect_p, string picu_khusus_covid_confirm_l,
                string picu_khusus_covid_confirm_p)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            PasienDirawattanpaKomorbidPost test = new PasienDirawattanpaKomorbidPost()
            {
                tanggal = tanggal,
                icu_dengan_ventilator_suspect_l = icu_dengan_ventilator_suspect_l,
                icu_dengan_ventilator_suspect_p = icu_dengan_ventilator_suspect_p,
                icu_dengan_ventilator_confirm_l = icu_dengan_ventilator_confirm_l,
                icu_dengan_ventilator_confirm_p = icu_dengan_ventilator_confirm_p,
                icu_tanpa_ventilator_suspect_l = icu_tanpa_ventilator_suspect_l,
                icu_tanpa_ventilator_suspect_p = icu_tanpa_ventilator_suspect_p,
                icu_tanpa_ventilator_confirm_l = icu_tanpa_ventilator_confirm_l,
                icu_tanpa_ventilator_confirm_p = icu_tanpa_ventilator_confirm_p,
                icu_tekanan_negatif_dengan_ventilator_suspect_l = icu_tekanan_negatif_dengan_ventilator_suspect_l,
                icu_tekanan_negatif_dengan_ventilator_suspect_p = icu_tekanan_negatif_dengan_ventilator_suspect_p,
                icu_tekanan_negatif_dengan_ventilator_confirm_l = icu_tekanan_negatif_dengan_ventilator_confim_l,
                icu_tekanan_negatif_dengan_ventilator_confirm_p = icu_tekanan_negatif_dengan_ventilator_confim_p,
                icu_tekanan_negatif_tanpa_ventilator_suspect_l = icu_tekanan_negatif_tanpa_ventilator_suspect_l,
                icu_tekanan_negatif_tanpa_ventilator_suspect_p = icu_tekanan_negatif_tanpa_ventilator_suspect_p,
                icu_tekanan_negatif_tanpa_ventilator_confirm_l = icu_tekanan_negatif_tanpa_ventilator_confirm_l,
                icu_tekanan_negatif_tanpa_ventilator_confirm_p = icu_tekanan_negatif_tanpa_ventilator_confirm_p,
                isolasi_tekanan_negatif_suspect_l = isolasi_tekanan_negatif_suspect_l,
                isolasi_tekanan_negatif_suspect_p = isolasi_tekanan_negatif_suspect_p,
                isolasi_tekanan_negatif_confirm_l = isolasi_tekanan_negatif_confirm_l,
                isolasi_tekanan_negatif_confirm_p = isolasi_tekanan_negatif_confirm_p,
                isolasi_tanpa_tekanan_negatif_suspect_l = isolasi_tanpa_tekanan_negatif_suspect_l,
                isolasi_tanpa_tekanan_negatif_suspect_p = isolasi_tanpa_tekanan_negatif_suspect_p,
                isolasi_tanpa_tekanan_negatif_confirm_l = isolasi_tanpa_tekanan_negatif_confirm_l,
                isolasi_tanpa_tekanan_negatif_confirm_p = isolasi_tanpa_tekanan_negatif_confirm_p,
                nicu_khusus_covid_suspect_l = nicu_khusus_covid_suspect_l,
                nicu_khusus_covid_suspect_p = nicu_khusus_covid_suspect_p,
                nicu_khusus_covid_confirm_l = nicu_khusus_covid_confirm_l,
                nicu_khusus_covid_confirm_p = nicu_khusus_covid_confirm_p,
                picu_khusus_covid_suspect_l = picu_khusus_covid_suspect_l,
                picu_khusus_covid_suspect_p = picu_khusus_covid_suspect_p,
                picu_khusus_covid_confirm_l = picu_khusus_covid_confirm_l,
                picu_khusus_covid_confirm_p = picu_khusus_covid_confirm_p
            };

            string data = JsonConvert.SerializeObject(test);
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest PostRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienDirawatTanpaKomorbid", url));
                PostRequest.Method = "POST";
                PostRequest.ContentType = "application/json";
                SetRequestHeader(PostRequest);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                PostRequest.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = PostRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)PostRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    result = string.Format("1|{0}", resp);
                }
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
                //HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienDirawatTanpaKomorbid", url));

                //GETRequest.Method = "GET";
                //SetRequestHeader(GETRequest);
                //WebResponse response = (WebResponse)GETRequest.GetResponse();
                //using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                //{
                //    result = sr.ReadToEnd();
                //    PasienDirawattanpaKomorbidRespon respInfo = JsonConvert.DeserializeObject<PasienDirawattanpaKomorbidRespon>(result);
                //    if (respInfo.PasienDirawattanpaKomorbid.status == "200")
                //    {
                //        result = string.Format("{0}", respInfo.PasienDirawattanpaKomorbid.message);
                //    }
                //    else
                //    {
                //        result = string.Format("{0}", respInfo.PasienDirawattanpaKomorbid.message);
                //    }
                //}
                //entityAPILog.Response = result;
                //BusinessLayer.InsertAPIMessageLog(entityAPILog);
                //return result;
            }
            catch (WebException ex)
            {
                result = "Gagal";
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #endregion
        #endregion
        #region Pasien Keluar

        #region Get Pasien Keluar
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetPasienKeluar()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienKeluar", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    entityAPILog.Response = resp;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        result = string.Format("1|{0}", resp);
                    }
                }
                return result;
            }
            catch (WebException ex)
            {

                result = string.Format("0|{0}", ex.Message);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region Post Pasien Keluar
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object PostPasienKeluar(
         string tanggal, 
         string sembuh,
         string discarded,
         string meninggal_komorbid,
         string meninggal_tanpa_komorbid,
         string meninggal_prob_pre_komorbid,
         string meninggal_prob_neo_komorbid,
         string meninggal_prob_bayi_komorbid,
         string meninggal_prob_balita_komorbid,
         string meninggal_prob_anak_komorbid,
         string meninggal_prob_remaja_komorbid,
         string meninggal_prob_dws_komorbid,
         string meninggal_prob_lansia_komorbid,
         string meninggal_prob_pre_tanpa_komorbid,
         string meninggal_prob_neo_tanpa_komorbid,
         string meninggal_prob_bayi_tanpa_komorbid,
         string meninggal_prob_balita_tanpa_komorbid,
         string meninggal_prob_anak_tanpa_komorbid,
         string meninggal_prob_remaja_tanpa_komorbid,
         string meninggal_prob_dws_tanpa_komorbid,
         string meninggal_prob_lansia_tanpa_komorbid,
         string meninggal_disarded_komorbid,
         string meninggal_discarded_tanpa_komorbid,
         string dirujuk,
         string isman,
         string aps)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            PostPasienkeluar test = new PostPasienkeluar()
            {
                 tanggal  = tanggal,
                 sembuh = sembuh,
                 discarded  = discarded,
                 meninggal_komorbid  = meninggal_komorbid,
                 meninggal_tanpa_komorbid = meninggal_tanpa_komorbid,
                 meninggal_prob_pre_komorbid = meninggal_prob_pre_komorbid,
                 meninggal_prob_bayi_komorbid = meninggal_prob_bayi_komorbid,
                 meninggal_prob_balita_komorbid = meninggal_prob_balita_komorbid,
                 meninggal_prob_anak_komorbid = meninggal_prob_anak_komorbid,
                 meninggal_prob_remaja_komorbid = meninggal_prob_remaja_komorbid,
                 meninggal_prob_dws_komorbid  = meninggal_prob_dws_komorbid,
                 meninggal_prob_lansia_komorbid  = meninggal_prob_lansia_komorbid,
                 meninggal_prob_pre_tanpa_komorbid  = meninggal_prob_pre_tanpa_komorbid,
                 meninggal_prob_neo_komorbid = meninggal_prob_neo_komorbid, 
                 meninggal_prob_neo_tanpa_komorbid  = meninggal_prob_neo_tanpa_komorbid,
                 meninggal_prob_bayi_tanpa_komorbid  = meninggal_prob_bayi_tanpa_komorbid,
                 meninggal_prob_balita_tanpa_komorbid = meninggal_prob_balita_tanpa_komorbid, 
                 meninggal_prob_anak_tanpa_komorbid  = meninggal_prob_anak_tanpa_komorbid,
                 meninggal_prob_remaja_tanpa_komorbid  = meninggal_prob_remaja_tanpa_komorbid,
                 meninggal_prob_dws_tanpa_komorbid  = meninggal_prob_dws_tanpa_komorbid,
                 meninggal_prob_lansia_tanpa_komorbid  = meninggal_prob_lansia_tanpa_komorbid,
                 meninggal_discarded_komorbid = meninggal_disarded_komorbid,
                 meninggal_discarded_tanpa_komorbid = meninggal_discarded_tanpa_komorbid,
                 dirujuk  = dirujuk, 
                 isman  = isman,
                 aps = aps,
            };

            string data = JsonConvert.SerializeObject(test);
            entityAPILog.MessageText = data;

            string result = "";
            string resp = "";
            try
            {
                HttpWebRequest PostRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/LapV2/PasienKeluar", url));
                PostRequest.Method = "POST";
                PostRequest.ContentType = "application/json";
                SetRequestHeader(PostRequest);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                PostRequest.ContentLength = bytes.Length;
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };
                Stream putStream = PostRequest.GetRequestStream();
                putStream.Write(bytes, 0, bytes.Length);
                putStream.Close();
                WebResponse response = (WebResponse)PostRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    resp = sr.ReadToEnd();
                    result = string.Format("1|{0}", resp);
                }
                entityAPILog.Response = resp;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
                 
            }
            catch (WebException ex)
            {
                resp = ex.Message;
                result = string.Format("0|{0}", resp);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #endregion

        #region Referensi SIRANAP
        #region Ruangan dan Tempat Tidur
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetRuangDanTempatTidurList()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Referensi/tempat_tidur", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    GetTempatTidur respInfo = JsonConvert.DeserializeObject<GetTempatTidur>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        KemenkesReferenceDao entityDao = new KemenkesReferenceDao(ctx);
                        try
                        {
                            List<KemenkesReference> lstReference = BusinessLayer.GetKemenkesReferenceList(string.Format("GCSiRanapObjectType = '{0}'", Constant.SIRANAPObjectType.SIRANAP_REFERENCE_RUANG_TEMPAT_TIDUR), ctx);
                            if (lstReference != null)
                            {
                                foreach (KemenkesReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.SIRANAPObjectType.SIRANAP_REFERENCE_RUANG_TEMPAT_TIDUR, entity.SiRanapCode);
                                }
                            }

                            KemenkesReference entityInsert = new KemenkesReference();
                            GetTempatTidurInfo[] objArr = respInfo.tempat_tidur;
                            int n = objArr.Length;

                            for (int i = 0; i < objArr.Length; i++)
                            {
                                GetTempatTidurInfo obj = objArr[i];
                                entityInsert.GCSiRanapObjectType = Constant.SIRANAPObjectType.SIRANAP_REFERENCE_RUANG_TEMPAT_TIDUR;
                                entityInsert.SiRanapCode = obj.kode_tt;
                                entityInsert.SiRanapName = obj.nama_tt;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }
                            ctx.CommitTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                        result = "Berhasil";
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                result = "Gagal";
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region SDM
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetSDMList()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Referensi/kebutuhan_sdm", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    GetSDM respInfo = JsonConvert.DeserializeObject<GetSDM>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        KemenkesReferenceDao entityDao = new KemenkesReferenceDao(ctx);
                        try
                        {
                            List<KemenkesReference> lstReference = BusinessLayer.GetKemenkesReferenceList(string.Format("GCSiRanapObjectType = '{0}'", Constant.SIRANAPObjectType.SIRANAP_REFERENCE_SDM), ctx);
                            if (lstReference != null)
                            {
                                foreach (KemenkesReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.SIRANAPObjectType.SIRANAP_REFERENCE_SDM, entity.SiRanapCode);
                                }
                            }

                            KemenkesReference entityInsert = new KemenkesReference();
                            GetSDMInfo[] objArr = respInfo.kebutuhan_sdm;
                            int n = objArr.Length;

                            for (int i = 0; i < objArr.Length; i++)
                            {
                                GetSDMInfo obj = objArr[i];
                                entityInsert.GCSiRanapObjectType = Constant.SIRANAPObjectType.SIRANAP_REFERENCE_SDM;
                                entityInsert.SiRanapCode = obj.id_kebutuhan;
                                entityInsert.SiRanapName = obj.kebutuhan;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }
                            ctx.CommitTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                        result = "Berhasil";
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                result = "Gagal";
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion

        #region APD
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object GetAPDList()
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.SIRANAP,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string data = "";
            entityAPILog.MessageText = data;

            string result = "";
            try
            {
                HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/Referensi/kebutuhan_apd", url));

                GETRequest.Method = "GET";
                SetRequestHeader(GETRequest);
                WebResponse response = (WebResponse)GETRequest.GetResponse();
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    GetAPD respInfo = JsonConvert.DeserializeObject<GetAPD>(result);
                    entityAPILog.Response = result;
                    BusinessLayer.InsertAPIMessageLog(entityAPILog);

                    if (result != null)
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        KemenkesReferenceDao entityDao = new KemenkesReferenceDao(ctx);
                        try
                        {
                            List<KemenkesReference> lstReference = BusinessLayer.GetKemenkesReferenceList(string.Format("GCSiRanapObjectType = '{0}'", Constant.SIRANAPObjectType.SIRANAP_REFERENCE_APD), ctx);
                            if (lstReference != null)
                            {
                                foreach (KemenkesReference entity in lstReference)
                                {
                                    entityDao.Delete(Constant.SIRANAPObjectType.SIRANAP_REFERENCE_APD, entity.SiRanapCode);
                                }
                            }

                            KemenkesReference entityInsert = new KemenkesReference();
                            GetAPDInfo[] objArr = respInfo.kebutuhan_apd;
                            int n = objArr.Length;

                            for (int i = 0; i < objArr.Length; i++)
                            {
                                GetAPDInfo obj = objArr[i];
                                entityInsert.GCSiRanapObjectType = Constant.SIRANAPObjectType.SIRANAP_REFERENCE_APD;
                                entityInsert.SiRanapCode = obj.id_kebutuhan;
                                entityInsert.SiRanapName = obj.kebutuhan;
                                entityInsert.CreatedBy = AppSession.UserLogin.UserID;
                                entityDao.Insert(entityInsert);
                            }
                            ctx.CommitTransaction();
                        }
                        finally
                        {
                            ctx.Close();
                        }
                        result = "Berhasil";
                    }
                }
                return result;
            }
            catch (WebException ex)
            {
                result = "Gagal";
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
        }

        #endregion
        #endregion

        #region Utility Function
        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = AppSession.SA0126;
            string pass = AppSession.SA0127;

            Request.Headers.Add("X-rs-id", consID);
            Request.Headers.Add("X-timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-pass", pass);
        }
        #endregion
    }
}
