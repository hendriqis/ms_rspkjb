using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.Common
{
    #region Informasi Tempat Tidur DTO Model

    #region Response
    public class DinKesTTResponse1
    {
        public DinKesTTResponseInfo1 response { get; set; }
    }

    public class DinKesTTResponseInfo1
    {
        public string kode { get; set; }
        public string messages { get; set; }
    }
    #endregion

    #region Informasi Tempat Tidur
    public class DinKesTTPOSTParam
    {
        public string kapasitas_vip { get; set; }
        public string kapasitas_kelas_1 { get; set; }
        public string kapasitas_kelas_2 { get; set; }
        public string kapasitas_kelas_3 { get; set; }
        public string kapasitas_kelas_1_l { get; set; }
        public string kapasitas_kelas_2_l { get; set; }
        public string kapasitas_kelas_3_l { get; set; }
        public string kapasitas_kelas_1_p { get; set; }
        public string kapasitas_kelas_2_p { get; set; }
        public string kapasitas_kelas_3_p { get; set; }
        public string kapasitas_hcu { get; set; }
        public string kapasitas_iccu { get; set; }
        public string kapasitas_icu_negatif_ventilator { get; set; }
        public string kapasitas_icu_negatif_tanpa_ventilator { get; set; }
        public string kapasitas_icu_tanpa_negatif_ventilator { get; set; }
        public string kapasitas_icu_tanpa_negatif_tanpa_ventilator { get; set; }
        public string kapasitas_icu_covid_negatif_ventilator { get; set; }
        public string kapasitas_icu_covid_negatif_tanpa_ventilator { get; set; }
        public string kapasitas_icu_covid_tanpa_negatif_ventilator { get; set; }
        public string kapasitas_icu_covid_tanpa_negatif_tanpa_ventilator { get; set; }
        public string kapasitas_isolasi_negatif { get; set; }
        public string kapasitas_isolasi_tanpa_negatif { get; set; }
        public string kapasitas_nicu_covid { get; set; }
        public string kapasitas_perina_covid { get; set; }
        public string kapasitas_picu_covid { get; set; }
        public string kapasitas_ok_covid { get; set; }
        public string kapasitas_hd_covid { get; set; }
        public string kosong_vip { get; set; }
        public string kosong_kelas_1 { get; set; }
        public string kosong_kelas_2 { get; set; }
        public string kosong_kelas_3 { get; set; }
        public string kosong_kelas_1_l { get; set; }
        public string kosong_kelas_2_l { get; set; }
        public string kosong_kelas_3_l { get; set; }
        public string kosong_kelas_1_p { get; set; }
        public string kosong_kelas_2_p { get; set; }
        public string kosong_kelas_3_p { get; set; }
        public string kosong_hcu { get; set; }
        public string kosong_iccu { get; set; }
        public string kosong_icu_negatif_ventilator { get; set; }
        public string kosong_icu_negatif_tanpa_ventilator { get; set; }
        public string kosong_icu_tanpa_negatif_ventilator { get; set; }
        public string kosong_icu_tanpa_negatif_tanpa_ventilator { get; set; }
        public string kosong_icu_covid_negatif_ventilator { get; set; }
        public string kosong_icu_covid_negatif_tanpa_ventilator { get; set; }
        public string kosong_icu_covid_tanpa_negatif_ventilator { get; set; }
        public string kosong_icu_covid_tanpa_negatif_tanpa_ventilator { get; set; }
        public string kosong_isolasi_negatif { get; set; }
        public string kosong_isolasi_tanpa_negatif { get; set; }
        public string kosong_nicu_covid { get; set; }
        public string kosong_perina_covid { get; set; }
        public string kosong_picu_covid { get; set; }
        public string kosong_ok_covid { get; set; }
        public string kosong_hd_covid { get; set; } 
    }
    #endregion

    #endregion
}