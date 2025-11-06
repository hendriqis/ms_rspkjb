using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QIS.Medinfras.Web.Common.UI;
using QIS.Medinfras.Data.Service;
using QIS.Medinfras.Web.Common;
using DevExpress.Web.ASPxCallbackPanel;
using QIS.Data.Core.Dal;
using Newtonsoft.Json;
using QIS.Medinfras.Web.CommonLibs.Service;

namespace QIS.Medinfras.Web.CommonLibs.Program
{
    public partial class HistoryPatientBPJSCtl : BaseViewPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            txtNoPeserta.Text = param;
            txtDateFrom.Text = DateTime.Now.AddDays(-3).ToString(Constant.FormatString.DATE_PICKER_FORMAT);
            txtDateTo.Text = DateTime.Now.ToString(Constant.FormatString.DATE_PICKER_FORMAT);

            DateTime dateFrom = Helper.GetDatePickerValue(txtDateFrom.Text);
            DateTime dateTo = Helper.GetDatePickerValue(txtDateTo.Text);

            hdnNoPeserta.Value = param;
            hdnTglAwal.Value = dateFrom.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);
            hdnTglAkhir.Value = dateTo.ToString(Constant.FormatString.DATE_PICKER_FORMAT2);

            List<SettingParameterDt> lstSetvar = BusinessLayer.GetSettingParameterDtList(string.Format("HealthcareID = '001' AND ParameterCode IN ('{0}')", Constant.SettingParameter.SA0167));
            hdnBPJSVersion.Value = lstSetvar.Where(w => w.ParameterCode == Constant.SettingParameter.SA0167).FirstOrDefault().ParameterValue;

            //GetBPJSHistoryPelayananList(param, hdnTglAwal.Value, hdnTglAkhir.Value);
        }

        private string GetBPJSHistoryPelayananList(string noPeserta, string tglAwal, string tglAkhir)
        {
            string finalResult = string.Empty;
            string resultList = string.Empty;

            BPJSService bpjsAPI = new BPJSService();
            if (hdnBPJSVersion.Value == Constant.BPJS_Version_Release.v1_0)
            {
                resultList = bpjsAPI.GetHistoryPelayananPeserta(noPeserta, tglAwal, tglAkhir);
            }
            else
            {
                resultList = bpjsAPI.GetHistoryPelayananPesertaAPI(noPeserta, tglAwal, tglAkhir);
            }
            string[] resultInfoList = resultList.Split('|');

            if (resultInfoList[0] == "1")
            {
                DataHistoriPelayananApiResponse apiResponseList = JsonConvert.DeserializeObject<DataHistoriPelayananApiResponse>(resultInfoList[1]);
                finalResult = resultList;
                grdView.DataSource = ConvertHistoryKunjunganListDataModel(apiResponseList);
                grdView.DataBind();
            }

            return finalResult;
        }
        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";
            result = GetBPJSHistoryPelayananList(hdnNoPeserta.Value, hdnTglAwal.Value, hdnTglAkhir.Value);
            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        #region ConvertHistoryKunjunganListDataModel
        private List<HistoryKunjungan> ConvertHistoryKunjunganListDataModel(DataHistoriPelayananApiResponse apiResponse)
        {
            //List<DataKunjungan> lstRujukan = apiResponse.response.rujukan.ToList();
            List<histori> lstHistory = apiResponse.response.histori.ToList();

            List<HistoryKunjungan> lstEntity = new List<HistoryKunjungan>();
            if (lstHistory.Count > 0)
            {
                foreach (histori hs in lstHistory)
                {
                    HistoryKunjungan obj = new HistoryKunjungan()
                    {
                        diagnosa = hs.diagnosa,
                        jnsPelayanan = hs.jnsPelayanan,
                        kelasRawat = hs.kelasRawat,
                        namaPeserta = hs.namaPeserta,
                        noKartu = hs.noKartu,
                        noSep = hs.noSep,
                        noRujukan = hs.noRujukan,
                        poli = hs.poli,
                        ppkPelayanan = hs.ppkPelayanan,
                        tglPlgSep = hs.tglPlgSep,
                        tglSep = hs.tglSep
                    };
                    lstEntity.Add(obj);
                }
            }
            return lstEntity;
        }
        #endregion
    }
}