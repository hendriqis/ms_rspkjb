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
    /// Web-service Dinas Kesehatan
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class DinKesService : System.Web.Services.WebService
    {
        private string url = AppSession.SA0111;

        #region Informasi Tempat Tidur

        #region Kirim Data Ketersediaan Bed
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string SendInformasiTT(string paramData)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.DINKES,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            string[] paramDataInfo = paramData.Split('|');
            DinKesTTPOSTParam postDATA = new DinKesTTPOSTParam()
            {
                kapasitas_vip = paramDataInfo[0],
                kapasitas_kelas_1 = paramDataInfo[1],
                kapasitas_kelas_2 = paramDataInfo[2],
                kapasitas_kelas_3 = paramDataInfo[3],
                kapasitas_kelas_1_l = paramDataInfo[4],
                kapasitas_kelas_2_l = paramDataInfo[5],
                kapasitas_kelas_3_l = paramDataInfo[6],
                kapasitas_kelas_1_p = paramDataInfo[7],
                kapasitas_kelas_2_p = paramDataInfo[8],
                kapasitas_kelas_3_p = paramDataInfo[9],
                kapasitas_hcu = paramDataInfo[10],
                kapasitas_iccu = paramDataInfo[11],
                kapasitas_icu_negatif_ventilator = paramDataInfo[12],
                kapasitas_icu_negatif_tanpa_ventilator = paramDataInfo[13],
                kapasitas_icu_tanpa_negatif_ventilator = paramDataInfo[14],
                kapasitas_icu_tanpa_negatif_tanpa_ventilator = paramDataInfo[15],
                kapasitas_icu_covid_negatif_ventilator = paramDataInfo[16],
                kapasitas_icu_covid_negatif_tanpa_ventilator = paramDataInfo[17],
                kapasitas_icu_covid_tanpa_negatif_ventilator = paramDataInfo[18],
                kapasitas_icu_covid_tanpa_negatif_tanpa_ventilator = paramDataInfo[19],
                kapasitas_isolasi_negatif = paramDataInfo[20],
                kapasitas_isolasi_tanpa_negatif = paramDataInfo[21],
                kapasitas_nicu_covid = paramDataInfo[22],
                kapasitas_perina_covid = paramDataInfo[23],
                kapasitas_picu_covid = paramDataInfo[24],
                kapasitas_ok_covid = paramDataInfo[25],
                kapasitas_hd_covid = paramDataInfo[26],
                kosong_vip = paramDataInfo[27],
                kosong_kelas_1 = paramDataInfo[28],
                kosong_kelas_2 = paramDataInfo[29],
                kosong_kelas_3 = paramDataInfo[30],
                kosong_kelas_1_l = paramDataInfo[31],
                kosong_kelas_2_l = paramDataInfo[32],
                kosong_kelas_3_l = paramDataInfo[33],
                kosong_kelas_1_p = paramDataInfo[34],
                kosong_kelas_2_p = paramDataInfo[35],
                kosong_kelas_3_p = paramDataInfo[36],
                kosong_hcu = paramDataInfo[37],
                kosong_iccu = paramDataInfo[38],
                kosong_icu_negatif_ventilator = paramDataInfo[39],
                kosong_icu_negatif_tanpa_ventilator = paramDataInfo[40],
                kosong_icu_tanpa_negatif_ventilator = paramDataInfo[41],
                kosong_icu_tanpa_negatif_tanpa_ventilator = paramDataInfo[42],
                kosong_icu_covid_negatif_ventilator = paramDataInfo[43],
                kosong_icu_covid_negatif_tanpa_ventilator = paramDataInfo[44],
                kosong_icu_covid_tanpa_negatif_ventilator = paramDataInfo[45],
                kosong_icu_covid_tanpa_negatif_tanpa_ventilator = paramDataInfo[46],
                kosong_isolasi_negatif = paramDataInfo[47],
                kosong_isolasi_tanpa_negatif = paramDataInfo[48],
                kosong_nicu_covid = paramDataInfo[49],
                kosong_perina_covid = paramDataInfo[50],
                kosong_picu_covid = paramDataInfo[51],
                kosong_ok_covid = paramDataInfo[52],
                kosong_hd_covid = paramDataInfo[53] 
            };

            string data = JsonConvert.SerializeObject(postDATA);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/bed", url));
                request.Method = "POST";
                //request.ContentType = "text/plain";
                request.ContentType = "application/json";
                SetRequestHeader(request);
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
                    DinKesTTResponseInfo1 respInfo = JsonConvert.DeserializeObject<DinKesTTResponseInfo1>(result);
                    if (respInfo.kode == "200")
                    {
                        result = string.Format("{0}|{1}|{2}", "1", respInfo.messages, "null");
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", respInfo.messages);
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                DinKesTTResponseInfo1 response = new DinKesTTResponseInfo1();
                response.kode = "999";
                string message = string.Empty;
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        message = string.Format("{0} (WebExceptionStatus.ProtocolError),", Constant.API_WS_EXCEPTION.PROTOCOL_ERROR);
                        break;
                    case WebExceptionStatus.ReceiveFailure:
                        message = string.Format("{0} (WebExceptionStatus.ReceiveFailure),", Constant.API_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    case WebExceptionStatus.Timeout:
                        message = string.Format("{0} (WebExceptionStatus.Timeout),", Constant.API_WS_EXCEPTION.TIMEOUT);
                        break;
                    case WebExceptionStatus.ConnectFailure:
                        message = string.Format("{0} (WebExceptionStatus.ConnectFailure),", Constant.API_WS_EXCEPTION.CONNECTION_CLOSED);
                        break;
                    default:
                        message = string.Format("{0} ({1}),", Constant.API_WS_EXCEPTION.GENERIC_ERROR, ex.Status.ToString());
                        break;
                }
                response.messages = message;
                string result = JsonConvert.SerializeObject(response);
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return string.Format("{0}|{1}|{2}", "0", "null", message);
            }
        }
        #endregion

        #endregion

        #region Utility Function
        private void SetRequestHeader(HttpWebRequest Request)
        {
            string consID = AppSession.SA0112;
            string pass = AppSession.SA0113;

            Request.Headers.Add("Api-Bed-User", consID);
            Request.Headers.Add("Api-Bed-Key", pass);
        }
        #endregion
    }
}
