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
    /// Web-service Nalagenetics
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    [System.Web.Script.Services.ScriptService]

    public class NalageneticsService : System.Web.Services.WebService
    {
        private string url = AppSession.SA0121;

        #region Report

        #region Report Data
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public string GetReportData(string mrn, string transactionID, string sampleID)
        {
            APIMessageLog entityAPILog = new APIMessageLog()
            {
                MessageDateTime = DateTime.Now,
                Recipient = Constant.BridgingVendor.NALAGENETICS,
                Sender = Constant.BridgingVendor.HIS,
                IsSuccess = true
            };

            NalageneticsReportParameter1 postDATA = new NalageneticsReportParameter1()
            {
                sample_id = sampleID 
            };

            string data = JsonConvert.SerializeObject(postDATA);
            entityAPILog.MessageText = data;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format("{0}/external-report", url));
                request.Method = "POST";
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
                    NalageneticResponseInfo1 respInfo = JsonConvert.DeserializeObject<NalageneticResponseInfo1>(result);   
                    if (respInfo.status.ToUpper() == "TRUE")
                    {
                        IDbContext ctx = DbFactory.Configure(true);
                        PatientPharmacogenomicDao oPGxTestDao = new PatientPharmacogenomicDao(ctx);
                        LaboratoryResultHdDao resultHdDao = new LaboratoryResultHdDao(ctx);
                        PatientDao patientDao = new PatientDao(ctx);

                        try
                        {
                            LaboratoryResultHd oResultHd = BusinessLayer.GetLaboratoryResultHdList(string.Format("ChargeTransactionID = {0}", transactionID), ctx).FirstOrDefault();
                            if (oResultHd != null)
	                        {
                                List<NalageneticsMessage> lstMessage = respInfo.message;
                                foreach (NalageneticsMessage item in lstMessage)
                                {
                                    PatientPharmacogenomic obj = new PatientPharmacogenomic();
                                    obj.MRN = Convert.ToInt32(mrn);
                                    obj.TransactionID = Convert.ToInt32(transactionID);
                                    obj.SampleID = sampleID;
                                    obj.ReportID = item.report_id;
                                    obj.ATC_Code = item.atc_code;
                                    obj.DrugName = item.drug_name;
                                    obj.RecommendationText = item.current.rec_text;
                                    obj.RecommendationLevel = item.current.rec_level;
                                    obj.RecommendationCategory = item.current.rec_category;
                                    obj.ImplicationText = item.current.implication_text;
                                    obj.NalaScore = item.current.nala_score;
                                    obj.NalaScoreV2 = item.current.nala_score_v2;
                                    obj.CaveatText = item.current.caveat_text;
                                    obj.CaveatType = item.current.caveat_type;

                                    StringBuilder phenoTypeSummary = new StringBuilder();
                                    foreach (phenotype_summary phenotype in item.current.phenotype_summary)
                                    {
                                        phenoTypeSummary.AppendLine(string.Format("gene_symbol:{0}|geno_type:{1}|phenotype_text{2}", phenotype.gene_symbol, phenotype.geno_type, phenotype.phenotype_text));
                                    }
                                    obj.PhenotypeSummary = phenoTypeSummary.ToString();

                                    StringBuilder metabolism = new StringBuilder();
                                    //StringBuilder toxicity = new StringBuilder();
                                    StringBuilder pharmacokinetics = new StringBuilder();
                                    if (item.current.pmdids != null)
                                    {
                                        foreach (pmids pmid in item.current.pmdids)
                                        {
                                            foreach (metabolism mb in pmid.metabolism)
                                            {
                                                metabolism.AppendLine(string.Format("{0}", mb.pmidLink));
                                            }
                                            //foreach (toxicity tx in pmid.toxicity)
                                            //{
                                            //    metabolism.AppendLine(string.Format("{0}", mb.pmidLink));
                                            //}
                                            foreach (pharmacokinetics ph in pmid.pharmacokinetics)
                                            {
                                                pharmacokinetics.AppendLine(string.Format("{0}", ph.pmidLink));
                                            }
                                        } 
                                    }
                                    obj.Metabolism = metabolism.ToString();

                                    StringBuilder usageSummary = new StringBuilder();
                                    foreach (usage usage in item.current.usage)
                                    {
                                        usageSummary.AppendLine(string.Format("usage_text:{0}|usage_alternative:{1}", usage._usage_text, usage._usage_alternative));
                                    }
                                    obj.Usage = usageSummary.ToString();
                                    obj.VersionID = item.current.version_id;
                                    obj.CreatedBy = AppSession.UserLogin.UserID;

                                    oPGxTestDao.Insert(obj);
                                }

                                oResultHd.IsHasPharmacogenomicTest = true;
                                oResultHd.IsPharmacogenomicTestReportExists = true;
                                resultHdDao.Update(oResultHd);

                                #region Update Patient
                                Patient oPatient = patientDao.Get(Convert.ToInt32(mrn));
                                oPatient.IsHasPharmacogenomicProfile = true;
                                patientDao.Update(oPatient); 
                                #endregion

                                ctx.CommitTransaction();
                                result = string.Format("{0}|{1}|{2}", "1", JsonConvert.SerializeObject(respInfo.message), "null");
	                        }
                            else
                            {
                                ctx.RollBackTransaction();
                                result = string.Format("{0}|{1}|{2}", "0", "null", "Invalid Transaction ID.");
                            }
                        }
                        catch (Exception ex)
                        {
                            ctx.RollBackTransaction();
                            result = string.Format("{0}|{1}|{2}", "0", "null", ex.Message);                            
                        }
                        finally 
                        {
                            ctx.Close();
                        }
                    }
                    else
                    {
                        result = string.Format("{0}|{1}|{2}", "0", "null", "Terjadi kesalahan ketika proses pemanggilan ");
                    }
                }
                entityAPILog.Response = result;
                BusinessLayer.InsertAPIMessageLog(entityAPILog);
                return result;
            }
            catch (WebException ex)
            {
                NalageneticResponseInfo1 response = new NalageneticResponseInfo1();
                response.status = "False";
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
                response.message = null;
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
            string key = AppSession.SA0120;

            Request.Headers.Add("x-api-key", key);
        }
        #endregion
    }
}
