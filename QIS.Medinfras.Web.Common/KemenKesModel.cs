using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.Common
{
   
    #region Entry Data Pasien

    #region RekapPasienMasuk
    public class RekapPasienMasukPost
    {
        public string tanggal { get; set; }
        public string igd_suspect_l { get; set; }
        public string igd_suspect_p { get; set; }
        public string igd_confirm_l { get; set; }
        public string igd_confirm_p { get; set; }
        public string rj_suspect_l { get; set; }
        public string rj_suspect_p { get; set; }
        public string rj_confirm_l { get; set; }
        public string rj_confirm_p { get; set; }
        public string ri_suspect_l { get; set; }
        public string ri_suspect_p { get; set; }
        public string ri_confirm_l { get; set; }
        public string ri_confirm_p { get; set; }
    }

    public class RekapPasienMasukDelete
    {
        public RekapPasienMasukPost tanggal {get; set;}
    }

    #region RekapPasienMasukRespon
    public class RekapPasienMasukData
    {
        public string id { get; set; }
        public string koders { get; set; }
        public string tanggal { get; set; }
        public string igd_suspect_l { get; set; }
        public string igd_suspect_p { get; set; }
        public string igd_confirm_l { get; set; }
        public string igd_confirm_p { get; set; }
        public string rj_suspect_l { get; set; }
        public string rj_suspect_p { get; set; }
        public string rj_confirm_l { get; set; }
        public string rj_confirm_p { get; set; }
        public string ri_suspect_l { get; set; }
        public string ri_suspect_p { get; set; }
        public string ri_confirm_l { get; set; }
        public string ri_confirm_p { get; set; }
        public string tgl_lapor { get; set; }
    }

    public class RekapPasienMasukResponNew {
        public List<RekapPasienMasukData> RekapPasienMasuk { get; set; }
    }

    public class RekapPasienMasukRespon
    {
        public RekapPasienMasukResponInfo RekapPasienMasuk { get; set; }
    }

    public class RekapPasienMasukResponInfo
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    #endregion
    #endregion

    #region PasienDirawatdenganKomorbid
    public class PasienDirawatdenganKomorbidPost
    {
       public string tanggal { get; set; }
       public string icu_dengan_ventilator_suspect_l{ get; set; }
       public string icu_dengan_ventilator_suspect_p{ get; set; }
       public string icu_dengan_ventilator_confirm_l { get; set; }
       public string icu_dengan_ventilator_confirm_p { get; set; }
       public string icu_tanpa_ventilator_suspect_l { get; set; }
       public string icu_tanpa_ventilator_suspect_p { get; set; }
       public string icu_tanpa_ventilator_confirm_l { get; set; }
       public string icu_tanpa_ventilator_confirm_p { get; set; }
       public string icu_tekanan_negatif_dengan_ventilator_suspect_l { get; set; }
       public string icu_tekanan_negatif_dengan_ventilator_suspect_p { get; set; }
       public string icu_tekanan_negatif_dengan_ventilator_confirm_l { get; set; }
       public string icu_tekanan_negatif_dengan_ventilator_confirm_p { get; set; }
       public string icu_tekanan_negatif_tanpa_ventilator_suspect_l { get; set; }
       public string icu_tekanan_negatif_tanpa_ventilator_suspect_p { get; set; }
       public string icu_tekanan_negatif_tanpa_ventilator_confirm_l { get; set; }
       public string icu_tekanan_negatif_tanpa_ventilator_confirm_p { get; set; }
       public string isolasi_tekanan_negatif_suspect_l { get; set; }
       public string isolasi_tekanan_negatif_suspect_p { get; set; }
       public string isolasi_tekanan_negatif_confirm_l { get; set; }
       public string isolasi_tekanan_negatif_confirm_p { get; set; }
       public string isolasi_tanpa_tekanan_negatif_suspect_l { get; set; }
       public string isolasi_tanpa_tekanan_negatif_suspect_p { get; set; }
       public string isolasi_tanpa_tekanan_negatif_confirm_l { get; set; }
       public string isolasi_tanpa_tekanan_negatif_confirm_p { get; set; }
       public string nicu_khusus_covid_suspect_l { get; set; }
       public string nicu_khusus_covid_suspect_p { get; set; }
       public string nicu_khusus_covid_confirm_l{ get; set; }
       public string nicu_khusus_covid_confirm_p { get; set; }
       public string picu_khusus_covid_suspect_l { get; set; }
       public string picu_khusus_covid_suspect_p { get; set; }
       public string picu_khusus_covid_confirm_l { get; set; }
       public string picu_khusus_covid_confirm_p { get; set; }
    }

    public class PasienDirawatdenganKomorbidDelete
    {
        public string tanggal;
    }

    #region PasienDirawatdenganKomorbidRespon
    public class PasienDirawatdenganKomorbidRespon
    {
        public PasienDirawatdenganKomorbidResponInfo PasienDirawatdenganKomorbid { get; set; }
    }

    public class PasienDirawatdenganKomorbidResponInfo
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    #endregion
    #endregion

    #region PasienDirawattanpaKomorbid
    public class PasienDirawattanpaKomorbidPost
    {
        public string tanggal { get; set; }
        public string icu_dengan_ventilator_suspect_l { get; set; }
        public string icu_dengan_ventilator_suspect_p { get; set; }
        public string icu_dengan_ventilator_confirm_l { get; set; }
        public string icu_dengan_ventilator_confirm_p { get; set; }
        public string icu_tanpa_ventilator_suspect_l { get; set; }
        public string icu_tanpa_ventilator_suspect_p { get; set; }
        public string icu_tanpa_ventilator_confirm_l { get; set; }
        public string icu_tanpa_ventilator_confirm_p { get; set; }
        public string icu_tekanan_negatif_dengan_ventilator_suspect_l { get; set; }
        public string icu_tekanan_negatif_dengan_ventilator_suspect_p { get; set; }
        public string icu_tekanan_negatif_dengan_ventilator_confirm_l { get; set; }
        public string icu_tekanan_negatif_dengan_ventilator_confirm_p { get; set; }
        public string icu_tekanan_negatif_tanpa_ventilator_suspect_l { get; set; }
        public string icu_tekanan_negatif_tanpa_ventilator_suspect_p { get; set; }
        public string icu_tekanan_negatif_tanpa_ventilator_confirm_l { get; set; }
        public string icu_tekanan_negatif_tanpa_ventilator_confirm_p { get; set; }
        public string isolasi_tekanan_negatif_suspect_l { get; set; }
        public string isolasi_tekanan_negatif_suspect_p { get; set; }
        public string isolasi_tekanan_negatif_confirm_l { get; set; }
        public string isolasi_tekanan_negatif_confirm_p { get; set; }
        public string isolasi_tanpa_tekanan_negatif_suspect_l { get; set; }
        public string isolasi_tanpa_tekanan_negatif_suspect_p { get; set; }
        public string isolasi_tanpa_tekanan_negatif_confirm_l { get; set; }
        public string isolasi_tanpa_tekanan_negatif_confirm_p { get; set; }
        public string nicu_khusus_covid_suspect_l { get; set; }
        public string nicu_khusus_covid_suspect_p { get; set; }
        public string nicu_khusus_covid_confirm_l { get; set; }
        public string nicu_khusus_covid_confirm_p { get; set; }
        public string picu_khusus_covid_suspect_l { get; set; }
        public string picu_khusus_covid_suspect_p { get; set; }
        public string picu_khusus_covid_confirm_l { get; set; }
        public string picu_khusus_covid_confirm_p { get; set; }
    }

    public class PasienDirawattanpaKomorbidDelete
    {
        public string tanggal;
    }

    #region PasienDirawattanpaKomorbidRespon
    public class PasienDirawattanpaKomorbidRespon
    {
        public PasienDirawattanpaKomorbidResponInfo PasienDirawattanpaKomorbid { get; set; }
    }

    public class PasienDirawattanpaKomorbidResponInfo
    {
        public string status { get; set; }
        public string message { get; set; }
    }
    #endregion
    #endregion

    #region Pasien Keluar
    public class PostPasienkeluar { 
        public string tanggal {get; set;}
        public string sembuh  {get; set;}
        public string discarded  {get; set;}
        public string meninggal_komorbid  {get; set;}
        public string meninggal_tanpa_komorbid {get; set;}
        public string meninggal_prob_pre_komorbid {get; set;}
        public string meninggal_prob_bayi_komorbid {get; set;}
        public string meninggal_prob_balita_komorbid {get; set;}
        public string meninggal_prob_anak_tanpa_komorbid { get; set; }
        public string meninggal_prob_anak_komorbid {get; set;}
        public string meninggal_prob_remaja_komorbid {get; set;}
        public string meninggal_prob_dws_komorbid  {get; set;}
        public string meninggal_prob_lansia_komorbid  {get; set;}
        public string meninggal_prob_pre_tanpa_komorbid  {get; set;}
        public string meninggal_prob_neo_komorbid { get; set; }
        public string meninggal_prob_neo_tanpa_komorbid  {get; set;}
        public string meninggal_prob_bayi_tanpa_komorbid  {get; set;}
        public string meninggal_prob_balita_tanpa_komorbid {get; set;} 
        public string meninggal_prob_remaja_tanpa_komorbid  {get; set;}
        public string meninggal_prob_dws_tanpa_komorbid  {get; set;}
        public string meninggal_prob_lansia_tanpa_komorbid  {get; set;}
        public string meninggal_discarded_komorbid { get; set; }
        public string meninggal_discarded_tanpa_komorbid { get; set; }
        public string dirujuk  {get; set;} 
        public string isman  {get; set;}
        public string aps { get; set; }
    }
    #endregion

    #endregion

    #region TempatTidur
    public class GetTempatTidur
    {
        public GetTempatTidurInfo[] tempat_tidur { get; set; }
    }

    public class GetTempatTidurInfo
    {
        public string kode_tt { get; set; }
        public string nama_tt { get; set; }
    }
    #endregion

    #region SDM
    public class GetSDM
    {
        public GetSDMInfo[] kebutuhan_sdm { get; set; }
    }

    public class GetSDMInfo
    {
        public string id_kebutuhan { get; set; }
        public string kebutuhan { get; set; }
    }
    #endregion

    #region APD
    public class GetAPD
    {
        public GetAPDInfo[] kebutuhan_apd { get; set; }
    }

    public class GetAPDInfo
    {
        public string id_kebutuhan { get; set; }
        public string kebutuhan { get; set; }
    }
    #endregion
}