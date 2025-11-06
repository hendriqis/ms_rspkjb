using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common
{
    public class MetaData
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class GeneralData
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #region SEP Model
    public class rujukan_sep
    {
        public string asalRujukan { get; set; }
        public string tglRujukan { get; set; }
        public string noRujukan { get; set; }
        public string ppkRujukan { get; set; }
    }

    public class poli_sep
    {
        public string tujuan { get; set; }
        public string eksekutif { get; set; }
    }

    public class poli_sep_2
    {
        public string eksekutif { get; set; }
    }

    public class cob_sep
    {
        public string cob { get; set; }
    }

    public class katarak_sep
    {
        public string katarak { get; set; }
    }

    #region ver 1.0
    public class jaminan_sep
    {
        public string lakaLantas { get; set; }
        public string penjamin { get; set; }
        public string lokasiLaka { get; set; }
    }
    #endregion

    public class lokasiLaka
    {
        public string kdPropinsi { get; set; }
        public string kdKabupaten { get; set; }
        public string kdKecamatan { get; set; }
    }

    public class suplesi_sep
    {
        public string suplesi { get; set; }
        public string noSepSuplesi { get; set; }
        public lokasiLaka lokasiLaka { get; set; }
    }

    public class penjamin_sep
    {
        public string penjamin { get; set; }
        public string tglKejadian { get; set; }
        public string keterangan { get; set; }
        public suplesi_sep suplesi { get; set; }
    }

    public class jaminan
    {
        public string lakaLantas { get; set; }
        public penjamin_sep penjamin { get; set; }
    }

    public class skdp
    {
        public string noSurat { get; set; }
        public string kodeDPJP { get; set; }
    }

    public class t_sep
    {
        public string noKartu { get; set; }
        public string tglSep { get; set; }
        public string ppkPelayanan { get; set; }
        public string jnsPelayanan { get; set; }
        public string klsRawat { get; set; }
        public string noMR { get; set; }
        public rujukan_sep rujukan { get; set; }
        public string catatan { get; set; }
        public string diagAwal { get; set; }
        public poli_sep poli { get; set; }
        public cob_sep cob { get; set; }
        public katarak_sep katarak { get; set; }
        public jaminan jaminan { get; set; }
        public skdp skdp { get; set; }
        public string noTelp { get; set; }
        public string user { get; set; }
    }

    public class t_sep_1
    {
        public string noKartu { get; set; }
        public string tglSep { get; set; }
        public string jnsPelayanan { get; set; }
        public string keterangan { get; set; }
        public string user { get; set; }
    }

    public class sep_peserta
    {
        public string asuransi { get; set; }
        public string hakKelas { get; set; }
        public string jnsPeserta { get; set; }
        public string kelamin { get; set; }
        public string nama { get; set; }
        public string noKartu { get; set; }
        public string noMr { get; set; }
        public string tglLahir { get; set; }     
    }

    public class sep
    {
        public string catatan { get; set; }
        public string diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string noSep { get; set; }
        public string noRujukan { get; set; }
        public string penjamin { get; set; }
        public sep_peserta peserta { get; set; }
        public string poli { get; set; }
        public string poliEksekutif { get; set; }
        public string tglSep { get; set; }
    }
    #endregion

    #region Update SEP Model
    public class updateSEP
    {
        public string noSep { get; set; }
        public string klsRawat { get; set; }
        public string noMR { get; set; }
        public rujukan_sep rujukan { get; set; }
        public string catatan { get; set; }
        public string diagAwal { get; set; }
        public poli_sep_2 poli { get; set; }
        public cob_sep cob { get; set; }
        public katarak_sep katarak { get; set; }
        public skdp skdp { get; set; }
        public jaminan jaminan { get; set; }
        public string noTelp { get; set; }
        public string user { get; set; }
    }
    #endregion

    #region Delete SEP Model
    public class deleteSEP
    {
        public string noSep { get; set; }
        public string user { get; set; }
    }
    #endregion

    #region Delete SEP Model
    public class updateTglPlg
    {
        public string noSep { get; set; }
        public string tglPulang { get; set; }
        public string user { get; set; }
    }
    #endregion

    #region Mapping BPJS Transaction Model
    public class mappingTransaction
    {
        public string noSep { get; set; }
        public string noTrans { get; set; }
        public string ppkPelayanan { get; set; }
    }
    #endregion

    #region Rujukan
    public class provPerujuk
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    public class rujukan
    {
        public Diagnosa diagnosa { get; set; }
        public string keluhan { get; set; }
        public string noKunjungan { get; set; }
        public JenisPelayanan pelayanan { get; set; }
        public peserta peserta { get; set; }
        public PoliTujuan poliRujukan { get; set; }
        public provPerujuk provPerujuk { get; set; }
        public string tglKunjungan { get; set; }
    }

    public class RujukanResponse
    {
        public rujukan rujukan { get; set; }
    }

    public class RujukanResponse2
    {
        public List<rujukan> rujukan { get; set; }
    }

    public class RujukanAPIResponse
    {
        public MetaData metadata { get; set; }
        public RujukanResponse response { get; set; }
    }

    public class RujukanListAPIResponse
    {
        public MetaData metadata { get; set; }
        public RujukanResponse2 response { get; set; }
    }
    #endregion

    public class JenisPelayanan
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #region Peserta Model
    public class cob
    {
        public string nmAsuransi { get; set; }
        public string noAsuransi { get; set; }
        public string tglTAT { get; set; }
        public string tglTMT { get; set; }
    }
    public class hakKelas
    {
        public string keterangan { get; set; }
        public string kode { get; set; }
    }
    public class informasi
    {
        public string dinsos { get; set; }
        public string noSKTM { get; set; }
        public string prolanisPRB { get; set; }
    }
    public class jenisPeserta
    {
        public string keterangan { get; set; }
        public string kode { get; set; }
    }
    public class mr
    {
        public string noMR { get; set; }
        public string noTelepon { get; set; }
    }
    public class provUmum
    {
        public string kdProvider { get; set; }
        public string nmProvider { get; set; }
    }
    public class statusPeserta
    {
        public string keterangan { get; set; }
        public string kode { get; set; }
    }
    public class umur
    {
        public string umurSaatPelayanan { get; set; }
        public string umurSekarang { get; set; }
    }
    public class RiwayatPeserta
    {
        public string biayaTagihan { get; set; }
        public Diagnosa diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string noSEP { get; set; }
        public PoliTujuan poliTujuan { get; set; }
        public string tglPulang { get; set; }
        public string tglSEP { get; set; }
    }
    public class Faskes
    {
        public string kdCabang { get; set; }
        public string kdProvider { get; set; }
        public string nmCabang { get; set; }
        public string nmProvider { get; set; }
    }

    /// <summary>
    /// Default Peserta Model
    /// </summary>
    public class peserta
    {
        public cob cob { get; set; }
        public hakKelas hakKelas { get; set; }
        public informasi informasi { get; set; }
        public jenisPeserta jenisPeserta { get; set; }
        public mr mr { get; set; }
        public string nama { get; set; }
        public string nik { get; set; }
        public string noKartu { get; set; }
        public string pisa { get; set; }
        public provUmum provUmum { get; set; }
        public string sex { get; set; }
        public statusPeserta statusPeserta { get; set; }
        public string tglCetakKartu { get; set; }
        public string tglLahir { get; set; }
        public string tglTAT { get; set; }
        public string tglTMT { get; set; }
        public umur umur { get; set; }
    }

    /// <summary>
    /// Model Peserta : Integrasi dengan INACBG
    /// </summary>
    public class pesertasep
    {
        public string noMr { get; set; }
        public string noKartuBpjs { get; set; }
        public string noRujukan { get; set; }
        public string nama { get; set; }
        public string tglLahir { get; set; }
        public string kelamin { get; set; }
        public string klsRawat { get; set; }
        public string tglPelayanan { get; set; }
        public string tktPelayanan { get; set; }
        public string poli { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string poliEksekutif { get; set; }
        public string catatan { get; set; }
    }

    /// <summary>
    /// Generic MEDINFRAS Peserta SEP Model
    /// </summary>
    public class PesertaSEPObj
    {
        public string noSep { get; set; }
        public string tglSep { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string diagnosa { get; set; }
        public string noRujukan { get; set; }
        public string poli { get; set; }
        public string poliEksekutif { get; set; }
        public string catatan { get; set; }
        public string penjamin { get; set; }
        public string kdStatusKecelakaan { get; set; }
        public string nmstatusKecelakaan { get; set; }
        public Lokasikejadian lokasiKejadian { get; set; }
        public Dpjp dpjp { get; set; }
        public sep_peserta peserta { get; set; }
        public Klsrawat klsRawat { get; set; }
        public Kontrol kontrol { get; set; }
        public string cob { get; set; }
        public string katarak { get; set; }
    }

    public class Lokasikejadian
    {
        public string kdKab { get; set; }
        public string kdKec { get; set; }
        public string kdProp { get; set; }
        public string ketKejadian { get; set; }
        public string lokasi { get; set; }
        public string tglKejadian { get; set; }
    }

    public class Dpjp
    {
        public string kdDPJP { get; set; }
        public string nmDPJP { get; set; }
    }
    public class Klsrawat
    {
        public string klsRawatHak { get; set; }
        public string klsRawatNaik { get; set; }
        public string pembiayaan { get; set; }
        public string penanggungJawab { get; set; }
    }

    public class Kontrol
    {
        public string kdDokter { get; set; }
        public string nmDokter { get; set; }
        public string noSurat { get; set; }
    }

    public class Peserta
    {
        public string noKartu { get; set; }
        public string nama { get; set; }
        public string tglLahir { get; set; }
        public string kelamin { get; set; }
        public string hakKelas { get; set; }
    }

    /// <summary>
    /// Model Peserta : Peserta di LPK
    /// </summary>
    public class pesertaLPK
    {
        public string asuransi { get; set; }
        public string hakKelas { get; set; }
        public string jnsPeserta { get; set; }
        public string kelamin { get; set; }
        public string nama { get; set; }
        public string noKartu { get; set; }
        public string noMr { get; set; }
        public string tglLahir { get; set; }
    }

    /// <summary>
    /// Model Peserta : Model Umum Peserta di Response API
    /// </summary>
    public class pesertaGeneral
    {
        public string asuransi { get; set; }
        public string hakKelas { get; set; }
        public string jnsPeserta { get; set; }
        public string kelamin { get; set; }
        public string nama { get; set; }
        public string noKartu { get; set; }
        public string noMr { get; set; }
        public string tglLahir { get; set; }
    }
    #endregion

    public class Diagnosa
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    public class PoliTujuan
    {
        public string kode { get; set; }
        public string nama { get; set; }
    }

    #region Response Info
    public class PesertaResponse
    {
        public peserta peserta { get; set; }
    }
    public class RiwayatPesertaResponse
    {
        public int count { get; set; }
        public int limit { get; set; }
        public List<RiwayatPeserta> list { get; set; }
    }
    public class FaskesListResponse
    {
        public int count { get; set; }
        public int limit { get; set; }
        public List<Faskes> list { get; set; }
    }

    public class InsertSEPResponse
    {
        public sep sep { get; set; }
    }

    public class BPJSPesertaAPI
    {
        public MetaData metadata { get; set; }
        public PesertaResponse response { get; set; }
    }
    public class BPJSRiwayatPesertaAPI
    {
        public MetaData metadata { get; set; }
        public RiwayatPesertaResponse response { get; set; }
    }
    public class BPJSFaskesListAPI
    {
        public MetaData metadata { get; set; }
        public FaskesListResponse response { get; set; }
    }
    public class BPJSSepAPI
    {
        public MetaData metadata { get; set; }
        public InsertSEPResponse response { get; set; }
    }
    public class BPJSUpdateSepAPI
    {
        public MetaData metadata { get; set; }
        public string response { get; set; }
    }
    public class BPJSDefaultResponse
    {
        public MetaData metadata { get; set; }
        public string response { get; set; }
    }
    #endregion

    #region Request Data
    public class BPJSGenerateSEP
    {
        public t_sep t_sep { get; set; }
    }

    public class BPJSUpdateSEP
    {
        public updateSEP t_sep { get; set; }
    }

    public class BPJSDeleteSEP
    {
        public deleteSEP t_sep { get; set; }
    }

    public class BPJSUpdateTglPlg
    {
        public updateTglPlg t_sep { get; set; }
    }

    public class MappingBPJSTransaction
    {
        public mappingTransaction t_map_sep { get; set; }
    }

    public class BPJSGenerateSEPAPI
    {
        public BPJSGenerateSEP request { get; set; }
    }

    public class BPJSUpdateSEPAPI
    {
        public BPJSUpdateSEP request { get; set; }
    }

    public class BPJSUpdateTglPlgAPI
    {
        public BPJSUpdateTglPlg request { get; set; }
    }

    public class BPJSDeleteSEPAPI
    {
        public BPJSDeleteSEP request { get; set; }
    }

    public class MappingBPJSTransactionAPI
    {
        public MappingBPJSTransaction request { get; set; }
    }

    //public class BPJSRujukanAPI
    //{
    //    public  request { get; set; }
    //}
    #endregion


    #region SEP Object - Model
    /// <summary>
    /// Data Model SEP : Monitoring Data Kunjungan
    /// </summary>
    public class sep1
    {
        public string noKartu { get; set; }
        public string noSep { get; set; }
        public string nama { get; set; }
        public string diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string poli { get; set; }
        public string tglPlgSep { get; set; }
        public string tglSep { get; set; }
    }

    public class LPK_SEP
    {
        public string catatan { get; set; }
        public string diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string noSep { get; set; }
        public string penjamin { get; set; }
        public pesertaLPK peserta { get; set; }
        public string poli { get; set; }
        public string poliEksekutif { get; set; }
        public string tglSep { get; set; }
    }
    #endregion

    #region Pengajuan SEP
    public class SEP_Propose_Param
    {
        public t_sep_1 t_sep { get; set; }
    }
    public class SEP_Propose_Request
    {
        public SEP_Propose_Param request { get; set; }
    }
    #endregion

    #region Pencarian Data SEP
    public class BPJSFindSepAPIResponse1
    {
        public MetaData metadata { get; set; }
        public sep response { get; set; }
    }

    public class BPJSFindSepAPIResponse2
    {
        public MetaData metadata { get; set; }
        public pesertasep response { get; set; }
    }

    /// <summary>
    /// Standard Medinfras - BPJS API : Find SEP Model
    /// </summary>
    public class FindSepApiResponse
    {
        public MetaData metadata { get; set; }
        public PesertaSEPObj response { get; set; }
    }
    #endregion

    #region Monitoring Kunjungan
    /// <summary>
    /// Standard Medinfras - BPJS API : Monitoring Data Kunjungan
    /// </summary>
    public class DataKunjungan
    {
        public string noSEP { get; set; }
        public string noPeserta { get; set; }
        public string nama { get; set; }
        public string poli { get; set; }
        public string tglPulangSEP { get; set; }
        public string tglSEP { get; set; }
        public string kodeJenisPelayanan { get; set; }
        public string namaJenisPelayanan { get; set; }
        public string jenisPelayanan { get; set; }
        public string kodeKelasRawat { get; set; }
        public string namaKelasRawat { get; set; }
        public string kelasRawat { get; set; }
        public vConsultVisit1 consultVisit { get; set; }

    }

    public class ListKunjunganResponse1
    {
        public List<sep1> sep { get; set; }
    }

    /// <summary>
    /// Standard Medinfras - BPJS API : Monitoring Data Kunjungan
    /// </summary>
    public class MonitoringKunjunganApiResponse
    {
        public MetaData metadata { get; set; }
        public ListKunjunganResponse1 response { get; set; }
    }

    /// <summary>
    /// Standard Medinfras - BPJS API : Monitoring Data Kunjungan
    /// </summary>
    public class DataKunjunganApiResponse
    {
        public MetaData metadata { get; set; }
        public List<DataKunjungan> response { get; set; }

    }



    /// <summary>
    /// Standard Medinfras - BPJS API : Monitoring Data Histori Pelayanan Peserta
    /// </summary>
    public class DataHistoriPelayananApiResponse
    {
        public MetaData metaData { get; set; }
        public DataHistoriPelayananPeserta response { get; set; }
    }

    public class DataHistoriPelayananPeserta
    {
        public List<histori> histori { get; set; }
    }

    public class histori
    {
        public string diagnosa { get; set; }
        public string jnsPelayanan { get; set; }
        public string kelasRawat { get; set; }
        public string namaPeserta { get; set; }
        public string noKartu { get; set; }
        public string noSep { get; set; }
        public string noRujukan { get; set; }
        public string poli { get; set; }
        public string ppkPelayanan { get; set; }
        public string tglPlgSep { get; set; }
        public string tglSep { get; set; }
    }

    public class GetSuratKontrolByNoPeserta
    {
        public Metadata metaData { get; set; }
        public GetSuratKontrolByNoPeserta_Response response { get; set; }
    }

    public class GetSuratKontrolByNoPeserta_Response
    {
        public ListSuratKontrol[] list { get; set; }
    }

    public class ListSuratKontrol
    {
        public string noSuratKontrol { get; set; }
        public string jnsPelayanan { get; set; }
        public string jnsKontrol { get; set; }
        public string namaJnsKontrol { get; set; }
        public string tglRencanaKontrol { get; set; }
        public string tglTerbitKontrol { get; set; }
        public object noSepAsalKontrol { get; set; }
        public string poliAsal { get; set; }
        public string namaPoliAsal { get; set; }
        public string poliTujuan { get; set; }
        public string namaPoliTujuan { get; set; }
        public object tglSEP { get; set; }
        public string kodeDokter { get; set; }
        public string namaDokter { get; set; }
        public string noKartu { get; set; }
        public string nama { get; set; }
        public string terbitSEP { get; set; }
    }

    #endregion

    #region Pengajuan Klaim
    public class t_lpk_poli
    {
        public string poli { get; set; }
    }
    public class t_lpk_perawatan
    {
        public string ruangRawat { get; set; }
        public string kelasRawat { get; set; }
        public string spesialistik { get; set; }
        public string caraKeluar { get; set; }
        public string kondisiPulang { get; set; }
    }

    public class t_lpk_diagnosa
    {
        public string kode { get; set; }
        public string level { get; set; }
    }

    public class t_lpk_procedure
    {
        public string kode { get; set; }
    }

    public class t_lpk_diagnosa_list
    {
        public List<t_lpk_diagnosa> diagnosa { get; set; }
    }

    public class t_lpk_procedure_list
    {
        public List<t_lpk_procedure> procedure { get; set; }
    }

    public class t_lpk_rujukan
    {
        public string kodePPK { get; set; }
    }

    public class t_lpk_kontrol
    {
        public string tglKontrol { get; set; }
        public string poli { get; set; }
    }

    public class t_lpk_rencanaTL
    {
        public string tindakLanjut { get; set; }
        public t_lpk_rujukan dirujukKe { get; set; }
        public t_lpk_kontrol kontrolKembali { get; set; }
    }

    public class t_lpk
    {
        public string noSep { get; set; }
        public string tglMasuk { get; set; }
        public string tglKeluar { get; set; }
        public string jaminan { get; set; }
        public t_lpk_poli poli { get; set; }
        public t_lpk_perawatan perawatan { get; set; }
        public List<t_lpk_diagnosa> diagnosa { get; set; }
        public List<t_lpk_procedure> procedure { get; set; }
        public t_lpk_rencanaTL rencanaTL { get; set; }
        public string DPJP { get; set; }
        public string user { get; set; }
    }

    public class t_lpk_delete
    {
        public string noSep { get; set; }
    }

    public class LPK_SEP_Request
    {
        public t_lpk t_lpk { get; set; }
    }

    public class LPK_SEP_Delete_Request
    {
        public t_lpk_delete t_lpk { get; set; }
    }

    public class LPK_SEP_Request_Param
    {
        public LPK_SEP_Request request { get; set; }
    }

    public class LPK_SEP_Delete_Request_Param
    {
        public LPK_SEP_Delete_Request request { get; set; }
    }
    public class LPKResponse
    {
        public MetaData metadata { get; set; }
        public string response { get; set; }
    }
    #endregion

    #region Rujukan
    public class t_rujukan
    {
        public string noSep { get; set; }
        public string tglRujukan { get; set; }
        public string ppkDirujuk { get; set; }
        public string jnsPelayanan { get; set; }
        public string catatan { get; set; }
        public string diagRujukan { get; set; }
        public string tipeRujukan { get; set; }
        public string poliRujukan { get; set; }
        public string user { get; set; }
    }

    public class Rujukan1
    {
        public GeneralData AsalRujukan { get; set; }
        public GeneralData diagnosa { get; set; }
        public string noRujukan { get; set; }
        public pesertaGeneral peserta { get; set; }
        public PoliTujuan poliTujuan { get; set; }
        public string tglRujukan { get; set; }
        public GeneralData tujuanRujukan { get; set; }
    }

    public class Rujukan_Default_Param
    {
        public t_rujukan t_rujukan { get; set; }
    }

    public class Rujukan_API_Request
    {
        public Rujukan_Default_Param request { get; set; }
    }

    public class Rujukan_API_Response
    {
        public MetaData metadata { get; set; }
        public Rujukan1 response { get; set; }
    }
    #endregion
}