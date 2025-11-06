using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
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
    /// Summary description for AplicaresService
    /// </summary>
    [WebService(Namespace = "http://tempuri2.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]
    public class AplicaresService : System.Web.Services.WebService
    {
        private string url = AppSession.APLICARES_WS_URL;

        #region CREATE RUANGAN DENGAN KELAS BARU
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CreateRoom(string kodeKelas, string kodeRuang, string namaRuang, string jumlahKapasitas, string jumlahKosong, string jumlahKosongPria, string jumlahKosongWanita, string jumlahKosongPriaWanita)
        {
            try
            {
                string ppkPelayanan = AppSession.BPJS_Code;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/rest/bed/create/{1}", url, ppkPelayanan));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);
                RequestCreateRoom entityRequest = new RequestCreateRoom()
                {
                    kodekelas = kodeKelas,
                    koderuang = kodeRuang,
                    namaruang = namaRuang,
                    kapasitas = jumlahKapasitas,
                    tersedia = jumlahKosong,
                    tersediapria = jumlahKosongPria,
                    tersediawanita = jumlahKosongWanita,
                    tersediapriawanita = jumlahKosongPriaWanita
                };

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.APLICARES,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                string data = JsonConvert.SerializeObject(entityRequest);
                entityAPILog.MessageText = data;
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
                    CreateRoomResponse respInfo = JsonConvert.DeserializeObject<CreateRoomResponse>(result);
                    result = respInfo.metadata.message;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object CreateRoom_MedinfrasAPI(string kodeKelas, string kodeRuang, string namaRuang, string jumlahKapasitas, string jumlahKosong, string jumlahKosongPria, string jumlahKosongWanita, string jumlahKosongPriaWanita)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bpjs/aplicares/update", url));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);

                AplicaresMedinfrasRequest entityRequest = new AplicaresMedinfrasRequest()
                {
                    Type = "CREATE",
                    Parameter = kodeKelas + "|" + kodeRuang + "|" + namaRuang + "|" + jumlahKapasitas + "|" + jumlahKosong + "|" + jumlahKosongPria + "|" + jumlahKosongWanita + "|" + jumlahKosongPriaWanita
                };

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.APLICARES,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                string data = JsonConvert.SerializeObject(entityRequest);
                entityAPILog.MessageText = data;
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
                    CreateRoomResponse respInfo = JsonConvert.DeserializeObject<CreateRoomResponse>(result);
                    result = respInfo.metadata.message;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region UPDATE KETERSEDIAAN TEMPAT TIDUR
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateRoomStatus(string kodeKelas, string kodeRuang, string namaRuang, string jumlahKapasitas, string jumlahKosong, string jumlahKosongPria, string jumlahKosongWanita, string jumlahKosongPriaWanita)
        {
            try
            {
                string ppkPelayanan = AppSession.BPJS_Code;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/rest/bed/update/{1}", url, ppkPelayanan));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);
                RequestUpdateRoom entityRequest = new RequestUpdateRoom()
                {
                    kodekelas = kodeKelas,
                    koderuang = kodeRuang,
                    namaruang = namaRuang,
                    kapasitas = jumlahKapasitas,
                    tersedia = jumlahKosong,
                    tersediapria = jumlahKosongPria,
                    tersediawanita = jumlahKosongWanita,
                    tersediapriawanita = jumlahKosongPriaWanita
                };

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.APLICARES,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                string data = JsonConvert.SerializeObject(entityRequest);
                entityAPILog.MessageText = data;
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
                    UpdateRoomResponse respInfo = JsonConvert.DeserializeObject<UpdateRoomResponse>(result);
                    result = respInfo.metadata.message;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region DELETE RUANGAN DENGAN SUATU KELAS
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object DeleteClassRoom(string kodeKelas, string kodeRuang)
        {
            try
            {
                string ppkPelayanan = AppSession.BPJS_Code;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/rest/bed/delete/{1}", url, ppkPelayanan));
                request.Method = "POST";
                request.ContentType = "application/json";
                SetRequestHeader(request);
                RequestDeleteRoom entityRequest = new RequestDeleteRoom()
                {
                    kodekelas = kodeKelas,
                    koderuang = kodeRuang
                };

                APIMessageLog entityAPILog = new APIMessageLog()
                {
                    MessageDateTime = DateTime.Now,
                    Recipient = Constant.BridgingVendor.APLICARES,
                    Sender = Constant.BridgingVendor.HIS,
                    IsSuccess = true
                };

                string data = JsonConvert.SerializeObject(entityRequest);
                entityAPILog.MessageText = data;
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
                    DeleteRoomResponse respInfo = JsonConvert.DeserializeObject<DeleteRoomResponse>(result);
                    result = respInfo.metadata.message;
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Update Status Send to Aplicares

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateStatusSendToAplicares(string HealthcareServiceUnitID, string ClassID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitRoomDao entityDao = new ServiceUnitRoomDao(ctx);
            MetaData entityMetaData = new MetaData();

            try
            {
                string filterSUR1 = string.Format("HealthcareServiceUnitID = {0} AND ClassID = {1} AND IsAplicares = 1 AND IsDeleted = 0", HealthcareServiceUnitID, ClassID);
                List<ServiceUnitRoom> lstEntity1 = BusinessLayer.GetServiceUnitRoomList(filterSUR1, ctx);
                foreach (ServiceUnitRoom entity in lstEntity1)
                {
                    entity.IsSendToAplicares = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                string filterSUR2 = string.Format("HealthcareServiceUnitID = {0} AND ClassID = {1} AND IsAplicares = 0 AND IsDeleted = 0", HealthcareServiceUnitID, ClassID);
                List<ServiceUnitRoom> lstEntity2 = BusinessLayer.GetServiceUnitRoomList(filterSUR2, ctx);
                foreach (ServiceUnitRoom entity in lstEntity2)
                {
                    entity.IsSendToAplicares = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }

                entityMetaData.code = "200";
                entityMetaData.message = HealthcareServiceUnitID.ToString();

                result = string.Format("{0}|{1}", "1", "BERHASIL");

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", "null", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }


        #endregion

        #region Update Status Delete from Aplicares

        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public object UpdateStatusDeleteFromAplicares(string HealthcareServiceUnitID, string ClassID)
        {
            string result = string.Empty;
            IDbContext ctx = DbFactory.Configure(true);
            ServiceUnitRoomDao entityDao = new ServiceUnitRoomDao(ctx);
            MetaData entityMetaData = new MetaData();

            try
            {
                string filterSUR1 = string.Format("HealthcareServiceUnitID = {0} AND ClassID = {1} AND IsAplicares = 1 AND IsDeleted = 0", HealthcareServiceUnitID, ClassID);
                List<ServiceUnitRoom> lstEntity1 = BusinessLayer.GetServiceUnitRoomList(filterSUR1, ctx);
                foreach (ServiceUnitRoom entity in lstEntity1)
                {
                    entity.IsSendToAplicares = true;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }

                ctx.CommandType = CommandType.Text;
                ctx.Command.Parameters.Clear();

                string filterSUR2 = string.Format("HealthcareServiceUnitID = {0} AND ClassID = {1} AND IsAplicares = 0 AND IsDeleted = 0", HealthcareServiceUnitID, ClassID);
                List<ServiceUnitRoom> lstEntity2 = BusinessLayer.GetServiceUnitRoomList(filterSUR2, ctx);
                foreach (ServiceUnitRoom entity in lstEntity2)
                {
                    entity.IsSendToAplicares = false;
                    entity.LastUpdatedBy = AppSession.UserLogin.UserID;
                    entityDao.Update(entity);
                }

                entityMetaData.code = "200";
                entityMetaData.message = HealthcareServiceUnitID.ToString();

                result = string.Format("{0}|{1}", "1", "BERHASIL");

                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                result = string.Format("{0}|{1}|{2}", "0", "null", ex.Message);
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }
            return result;
        }


        #endregion

        #region Utility Function
        private void SetRequestHeader(HttpWebRequest Request)
        {
            TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

            string consID = AppSession.APLICARES_Consumer_ID;
            string pass = AppSession.APLICARES_Consumer_Pwd;

            Request.Headers.Add("X-Cons-ID", consID);
            Request.Headers.Add("X-Timestamp", unixTimestamp.ToString());
            Request.Headers.Add("X-Signature", GenerateSignature(string.Format("{0}&{1}", consID, unixTimestamp), pass));
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
