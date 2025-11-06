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
    public partial class SearchBPJSSurkonCtl : BaseProcessPopupCtl
    {
        public override void InitializeDataControl(string param)
        {
            hdnIsBridgingBPJSVClaimVersion.Value = AppSession.SA0167;
            string[] paramInfo = param.Split('|');
            txtNoPeserta.Text = paramInfo[0];
            hdnNoPeserta.Value = paramInfo[0];
            hdnAsalRujukan.Value = paramInfo[1];
            //GetBPJSPatientReferralList(paramInfo[0], paramInfo[1]);

            List<Variable> lstBulan = new List<Variable>();
            lstBulan.Add(new Variable { Code = "01", Value = "Januari" });
            lstBulan.Add(new Variable { Code = "02", Value = "Februari" });
            lstBulan.Add(new Variable { Code = "03", Value = "Maret" });
            lstBulan.Add(new Variable { Code = "04", Value = "April" });
            lstBulan.Add(new Variable { Code = "05", Value = "Mei" });
            lstBulan.Add(new Variable { Code = "06", Value = "Juni" });
            lstBulan.Add(new Variable { Code = "07", Value = "Juli" });
            lstBulan.Add(new Variable { Code = "08", Value = "Agustus" });
            lstBulan.Add(new Variable { Code = "09", Value = "September" });
            lstBulan.Add(new Variable { Code = "10", Value = "Oktober" });
            lstBulan.Add(new Variable { Code = "11", Value = "November" });
            lstBulan.Add(new Variable { Code = "12", Value = "Desember" });
            Methods.SetComboBoxField<Variable>(cboBulanSuratKontrol, lstBulan, "Value", "Code");
            cboBulanSuratKontrol.SelectedIndex = 0;

            List<Variable> lstTahun = new List<Variable>();
            int todayYear = DateTime.Now.Year;
            int lastYear = DateTime.Now.AddYears(-1).Year;
            lstTahun.Add(new Variable { Code = todayYear.ToString(), Value = todayYear.ToString() });
            lstTahun.Add(new Variable { Code = lastYear.ToString(), Value = lastYear.ToString() });
            Methods.SetComboBoxField<Variable>(cboTahunSuratKontrol, lstTahun, "Value", "Code");
            cboTahunSuratKontrol.SelectedIndex = 0;
        }

        private string GetBPJSPatientReferralList(string noPeserta, string asalRujukan)
        {
            StandardCode entitySC = BusinessLayer.GetStandardCodeList(string.Format("StandardCodeID = '{0}'", asalRujukan)).FirstOrDefault();
            string resultList = string.Empty;
            string resultListRS = string.Empty;
            string result = string.Empty;
            string resultRS = string.Empty;
            string finalResult = string.Empty;

           

            return finalResult;

        }

        #region ConvertToRujukanDataModel
        private List<RujukanDTO> ConvertToRujukanDataModel(RujukanAPIResponse apiResponse)
        {
            string filterReg = string.Format("NoPeserta = '{0}' AND DepartmentID = '{1}'", hdnNoPeserta.Value, Constant.Facility.INPATIENT);
            vRegistrationBPJS1 oReg = BusinessLayer.GetvRegistrationBPJS1List(filterReg, 1, 1, "RegistrationID DESC").FirstOrDefault();
            List<RujukanDTO> lstEntity = new List<RujukanDTO>();
            if (apiResponse != null)
            {
                rujukan oRujukan = apiResponse.response.rujukan;

                if (oRujukan != null)
                {
                    RujukanDTO obj = new RujukanDTO()
                    {
                        tglKunjungan = oRujukan.tglKunjungan,
                        noKunjungan = oRujukan.noKunjungan,
                        noKartu = oRujukan.peserta.noKartu,
                        namaPeserta = oRujukan.peserta.nama,
                        kodePPK = oRujukan.peserta.provUmum.kdProvider,
                        namaPPK = oRujukan.peserta.provUmum.nmProvider,
                        kodeKelas = oRujukan.peserta.hakKelas.kode,
                        namaKelas = oRujukan.peserta.hakKelas.keterangan,
                        kodeJenisPeserta = oRujukan.peserta.jenisPeserta.kode,
                        namaJenisPeserta = oRujukan.peserta.jenisPeserta.keterangan,
                        nik = oRujukan.peserta.nik,
                        tglLahir = oRujukan.peserta.tglLahir,
                        kodeSex = oRujukan.peserta.sex,
                        kodeStatusPeserta = oRujukan.peserta.statusPeserta.kode,
                        namaStatusPeserta = oRujukan.peserta.statusPeserta.keterangan,
                        kodeDiagnosa = oRujukan.diagnosa.kode,
                        namaDiagnosa = oRujukan.diagnosa.nama,
                        kodePerujuk = oRujukan.provPerujuk.kode,
                        namaPerujuk = oRujukan.provPerujuk.nama,
                        kodePoli = oRujukan.poliRujukan.kode,
                        namaPoli = oRujukan.poliRujukan.nama,
                        kodePelayanan = oRujukan.pelayanan.kode,
                        namaPelayanan = oRujukan.pelayanan.nama,
                        keluhan = oRujukan.keluhan
                    };

                    lstEntity.Add(obj);
                }
            }

            if (oReg != null)
            {
                RujukanDTO obj = new RujukanDTO()
                {
                    tglKunjungan = oReg.RegistrationDate.ToString(),
                    noKunjungan = oReg.NoSEP,
                    noKartu = oReg.NoPeserta,
                    namaPeserta = oReg.PatientName,
                    kodePPK = "",
                    namaPPK = "",
                    kodeKelas = "",
                    namaKelas = "",
                    kodeJenisPeserta = "",
                    namaJenisPeserta = "",
                    nik = "",
                    tglLahir = "",
                    kodeSex = "",
                    kodeStatusPeserta = "",
                    namaStatusPeserta = "",
                    kodeDiagnosa = "",
                    namaDiagnosa = "",
                    kodePerujuk = "",
                    namaPerujuk = "",
                    kodePoli = "",
                    namaPoli = oReg.ServiceUnitName,
                    kodePelayanan = "",
                    namaPelayanan = "",
                    keluhan = "Kunjungan Rawat Inap"
                };

                lstEntity.Add(obj);
            }
            return lstEntity;

        }
        #endregion

        #region ConvertToRujukanListDataModel
        private List<RujukanDTO> ConvertToRujukanListDataModel(RujukanListAPIResponse apiResponse)
        {
            string filterReg = string.Format("NoPeserta = '{0}' AND DepartmentID = '{1}'", hdnNoPeserta.Value, Constant.Facility.INPATIENT);

            List<rujukan> lstRujukan = apiResponse.response.rujukan.ToList();
            vRegistrationBPJS1 oReg = BusinessLayer.GetvRegistrationBPJS1List(filterReg, 1, 1, "RegistrationID ASC").FirstOrDefault();

            List<RujukanDTO> lstEntity = new List<RujukanDTO>();
            if (lstRujukan.Count > 0)
            {
                foreach (rujukan item in lstRujukan)
                {
                    RujukanDTO obj = new RujukanDTO()
                    {
                        tglKunjungan = item.tglKunjungan,
                        noKunjungan = item.noKunjungan,
                        noKartu = item.peserta.noKartu,
                        namaPeserta = item.peserta.nama,
                        kodePPK = item.peserta.provUmum.kdProvider,
                        namaPPK = item.peserta.provUmum.nmProvider,
                        kodeKelas = item.peserta.hakKelas.kode,
                        namaKelas = item.peserta.hakKelas.keterangan,
                        kodeJenisPeserta = item.peserta.jenisPeserta.kode,
                        namaJenisPeserta = item.peserta.jenisPeserta.keterangan,
                        nik = item.peserta.nik,
                        tglLahir = item.peserta.tglLahir,
                        kodeSex = item.peserta.sex,
                        kodeStatusPeserta = item.peserta.statusPeserta.kode,
                        namaStatusPeserta = item.peserta.statusPeserta.keterangan,
                        kodeDiagnosa = item.diagnosa.kode,
                        namaDiagnosa = item.diagnosa.nama,
                        kodePerujuk = item.provPerujuk.kode,
                        namaPerujuk = item.provPerujuk.nama,
                        kodePoli = item.poliRujukan.kode,
                        namaPoli = item.poliRujukan.nama,
                        kodePelayanan = item.pelayanan.kode,
                        namaPelayanan = item.pelayanan.nama,
                        keluhan = item.keluhan
                    };
                    lstEntity.Add(obj);
                }
            }

            if (oReg != null)
            {
                RujukanDTO obj = new RujukanDTO()
                {
                    tglKunjungan = oReg.RegistrationDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT2),
                    noKunjungan = oReg.NoSEP,
                    noKartu = oReg.NoPeserta,
                    namaPeserta = oReg.PatientName,
                    kodePPK = "",
                    namaPPK = "",
                    kodeKelas = "",
                    namaKelas = "",
                    kodeJenisPeserta = "",
                    namaJenisPeserta = "",
                    nik = "",
                    tglLahir = "",
                    kodeSex = "",
                    kodeStatusPeserta = "",
                    namaStatusPeserta = "",
                    kodeDiagnosa = "",
                    namaDiagnosa = "",
                    kodePerujuk = "",
                    namaPerujuk = "",
                    kodePoli = "",
                    namaPoli = oReg.ServiceUnitName,
                    kodePelayanan = "",
                    namaPelayanan = "",
                    keluhan = "Kunjungan Rawat Inap"
                };

                lstEntity.Add(obj);
            }
            return lstEntity;
        }
        #endregion

        protected void cbpView_Callback(object sender, DevExpress.Web.ASPxClasses.CallbackEventArgsBase e)
        {
            LoadWords();
            string[] param = e.Parameter.Split('|');
            string result = param[0] + "|";

            if (param[0] == "search")
            {
                BPJSService oService = new BPJSService();
                //result = GetBPJSPatientReferralList(txtNoPeserta.Text, hdnAsalRujukan.Value);
                result = oService.GetRencanaKontrolByNoPeserta_MEDINFRASAPI(param[1], param[2], hdnNoPeserta.Value, param[3]);
                string[] resultInfo = result.Split('|');
                if (resultInfo[0] == "1")
                {
                    GetSuratKontrolByNoPeserta data = JsonConvert.DeserializeObject<GetSuratKontrolByNoPeserta>(resultInfo[1]);
                    if (data.metaData.code == "200")
                    {
                        grdView.DataSource = data.response.list;
                        grdView.DataBind();
                    }
                }
            }

            ASPxCallbackPanel panel = sender as ASPxCallbackPanel;
            panel.JSProperties["cpResult"] = result;
        }

        public override void SetProcessButtonVisibility(ref bool IsUsingProcessButton)
        {
            IsUsingProcessButton = true;
        }

        protected override bool OnProcessRecord(ref string errMessage, ref string retval)
        {
            bool result = true;
            return result;
        }
    }
}