using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Web.Common;
using QIS.Medinfras.Data.Service;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using System.Net;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class BridgingResultList : BasePagePatientPageList
    {
        protected string laboratoryTransactionCode = "";
        public override string OnGetMenuCode()
        {
            return Constant.MenuCode.MedicalDiagnostic.HISTORY_INFORMATION;
        }

        protected int PageCount = 1;
        protected override void InitializeDataControl()
        {
            HttpWebRequest GETRequest = (HttpWebRequest)WebRequest.Create("http://10.11.107.220/smartlab_atmajaya/his/api/gethispool/json");
            GETRequest.Method = "GET";
            string result = "";
            //SetRequestHeader(GETRequest);
            HttpWebResponse response = (HttpWebResponse)GETRequest.GetResponse();
            Encoding responseEncoding = Encoding.GetEncoding(response.CharacterSet);
            IDbContext ctx = DbFactory.Configure(true);
            LaboratoryResultHdDao entityLaboratoryResultHdDao = new LaboratoryResultHdDao(ctx);
            LaboratoryResultDtDao entityLaboratoryResultDtDao = new LaboratoryResultDtDao(ctx);
            try
            {
                using (StreamReader sr = new StreamReader(response.GetResponseStream(), responseEncoding))
                {
                    result = "[{\"HisPoolID\":\"98\",\"BillingNomor\":\"160441\",\"LabHeaderID\":\"54591\",\"LabNomor\":\"17082339\",\"LabID\":\"805\",\"KelompokPx\":\"KIMIA KLINIK\",\"LabKode\":\"02010401\",\"LabNama\":\"Jam 06:00\",\"LabMetode\":\"Manual\",\"LabHasil\":\"69\",\"LabSatuan\":\"mg/dL\",\"LabNilaiNormal\":\"60 - 100\",\"LabKeterangan\":\"\",\"LISDate\":\"2017-08-15 15:42:15\",\"HISDate\":\"0000-00-00 00:00:00\",\"LabStatus\":\"0\",\"Action\":\"A\",\"SpecialOrder\":\"\",\"ValUserName\":\"Ester Maria Mercedes.P. Amd.AK\",\"Status\":\"N\",\"IsMDT\":\"N\",\"LabNoRm\":\"293805\",\"T_RefTestCode\":\"02-Glukosa Kurva Harian j\"},{\"HisPoolID\":\"99\",\"BillingNomor\":\"160441\",\"LabHeaderID\":\"54591\",\"LabNomor\":\"17082339\",\"LabID\":\"806\",\"KelompokPx\":\"KIMIA KLINIK\",\"LabKode\":\"02010402\",\"LabNama\":\"Jam 11:00\",\"LabMetode\":\"Manual\",\"LabHasil\":\"141\",\"LabSatuan\":\"mg/dL\",\"LabNilaiNormal\":\"60 - 140\",\"LabKeterangan\":\"\",\"LISDate\":\"2017-08-15 15:42:15\",\"HISDate\":\"0000-00-00 00:00:00\",\"LabStatus\":\"0\",\"Action\":\"A\",\"SpecialOrder\":\"\",\"ValUserName\":\"Ester Maria Mercedes.P. Amd.AK\",\"Status\":\"N\",\"IsMDT\":\"N\",\"LabNoRm\":\"293805\",\"T_RefTestCode\":\"02-Glukosa Kurva Harian j\"},{\"HisPoolID\":\"100\",\"BillingNomor\":\"160441\",\"LabHeaderID\":\"54591\",\"LabNomor\":\"17082339\",\"LabID\":\"808\",\"KelompokPx\":\"KIMIA KLINIK\",\"LabKode\":\"02010403\",\"LabNama\":\"Jam 15:00\",\"LabMetode\":\"Manual\",\"LabHasil\":\"144\",\"LabSatuan\":\"mg/dL\",\"LabNilaiNormal\":\"60 - 140\",\"LabKeterangan\":\"\",\"LISDate\":\"2017-08-15 15:42:15\",\"HISDate\":\"0000-00-00 00:00:00\",\"LabStatus\":\"0\",\"Action\":\"A\",\"SpecialOrder\":\"\",\"ValUserName\":\"Ester Maria Mercedes.P. Amd.AK\",\"Status\":\"N\",\"IsMDT\":\"N\",\"LabNoRm\":\"293805\",\"T_RefTestCode\":\"02-Glukosa Kurva Harian j\"}]";//sr.ReadToEnd();
                    List<LISResultResponse> respInfo = JsonConvert.DeserializeObject<List<LISResultResponse>>(result);
                    string[] resp = respInfo.Select(o => o.BillingNomor).Distinct().ToArray();
                    for (int i = 0; i < resp.Count(); i++)
                    {
                        List<LISResultResponse> lstResponsePerTransactionID = respInfo.Where(t => t.BillingNomor == resp[i]).ToList();
                        int labResultHdID = 0;
                        PatientChargesHd entityTempHd = BusinessLayer.GetPatientChargesHd(Convert.ToInt32(resp[i]));
                        LaboratoryResultHd entityLabHd = new LaboratoryResultHd();
                        entityLabHd.VisitID = entityTempHd.VisitID;
                        entityLabHd.ReferenceNo = "";
                        entityLabHd.ResultDate = DateTime.Now;
                        entityLabHd.ResultTime = DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT);
                        entityLabHd.PrintedCount = 0;
                        entityLabHd.ChargeTransactionID = Convert.ToInt32(resp[i]);
                        entityLabHd.TestOrderID = entityTempHd.TestOrderID;
                        entityLabHd.IsInternal = true;
                        entityLabHd.ProviderName = "GRACIA";
                        entityLabHd.ParamedicName = "";
                        entityLabHd.Remarks = "";
                        entityLabHd.GCTransactionStatus = Constant.TransactionStatus.OPEN;
                        entityLaboratoryResultHdDao.Insert(entityLabHd);
                        labResultHdID = BusinessLayer.GetLaboratoryResultHdMaxID(ctx);
                        foreach (LISResultResponse entityPerTransactionID in lstResponsePerTransactionID)
                        {
                            vItemLaboratoryFraction entityFraction = BusinessLayer.GetvItemLaboratoryFractionList(string.Format("CommCode = '{0}'", entityPerTransactionID.LabKode)).FirstOrDefault();
                            if (entityFraction != null)
                            {
                                LaboratoryResultDt entityLabDt = new LaboratoryResultDt();
                                entityLabDt.ItemID = entityFraction.ItemID;
                                entityLabDt.FractionID = entityFraction.FractionID;
                                entityLabDt.ReferenceDtID = entityPerTransactionID.LabHeaderID;
                                decimal outresult = 0;
                                bool isDecimal = decimal.TryParse(entityPerTransactionID.LabHasil, out outresult);
                                if (isDecimal)
                                {
                                    entityLabDt.MetricResultValue = Convert.ToDecimal(entityPerTransactionID.LabHasil);
                                    entityLabDt.InternationalResultValue = Convert.ToDecimal(entityPerTransactionID.LabHasil);
                                }
                                entityLabDt.ConversionFactor = 0;
                                entityLabDt.MinMetricNormalValue = 0;
                                entityLabDt.MaxMetricNormalValue = 0;
                                entityLabDt.MinInternationalNormalValue = 0;
                                entityLabDt.MaxInternationalNormalValue = 0;
                                entityLabDt.PanicMetricUnitMin = -100;
                                entityLabDt.PanicMetricUnitMax = 9999;
                                entityLabDt.InternationalUnitMin = -100;
                                entityLabDt.InternationalUnitMax = 9999;
                                entityLabDt.TextValue = entityPerTransactionID.LabHasil;
                                entityLabDt.TextNormalValue = entityPerTransactionID.LabNilaiNormal;
                                entityLabDt.IsNormal = false;
                                entityLabDt.IsConfidential = false;
                                entityLabDt.IsVerified = true;
                                entityLabDt.ID = labResultHdID;
                                entityLaboratoryResultDtDao.Insert(entityLabDt);
                            }
                        }
                    }
                }
                ctx.CommitTransaction();
            }
            catch (Exception ex)
            {
                ctx.RollBackTransaction();
            }
            finally
            {
                ctx.Close();
            }

            BindGridView(1, true, ref PageCount);
        }

        private void BindGridView(int pageIndex, bool isCountPageCount, ref int pageCount)
        {
            
        }

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            int pageCount = 1;
            string result = "";
            if (e.Parameter != null && e.Parameter != "")
            {
                string[] param = e.Parameter.Split('|');
                if (param[0] == "changepage")
                {
                    BindGridView(Convert.ToInt32(param[1]), false, ref pageCount);
                    result = "changepage";
                }
                else // refresh
                {

                    BindGridView(1, true, ref pageCount);
                    result = "refresh|" + pageCount;
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }
    }
}