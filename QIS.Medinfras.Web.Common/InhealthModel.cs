using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    #region MedinfrasAPI
    public class MedinfrasAPI_BodyRequest
    {
        public string Type { get; set; }
        public string Parameter { get; set; }
    }

    public class MedinfrasAPI_Response
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Data { get; set; }
    }
    #endregion

    #region 1. CEK RESTRIKSI E-PRESCRIPTION

    public class ParamCekRestriksiEPrescription
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string kodeobatrs { get; set; }
        public string user { get; set; }
    }

    public class ResultCekRestriksiEPrescriptions
    {
        public string ERROCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string RESTRIKSI { get; set; }
    }
    #endregion

    #region 2. CEK RESTRIKSI TRANSAKSI

    public class ParamCekRestriksiTransaksi
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string kodeobatrs { get; set; }
        public Int32 jumlahobat { get; set; }
        public string user { get; set; }
    }

    public class ResultCekRestriksiTransaksi
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string RESTRIKSI { get; set; }
    }
    #endregion

    #region 3. CEK SJP

    public class ParamCekSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nokainhealth { get; set; }
        public string tanggalsjp { get; set; }
        public string poli { get; set; }
        public string tkp { get; set; }
    }

    public class ResultCekSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLSJP { get; set; }
        public string NOMORRUJUKAN { get; set; }
        public string TGLRUJUKAN { get; set; }
        public string KDPPKASALRUJUKAN { get; set; }
        public string NMPPKASALRUJUKAN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string KDDIAG { get; set; }
        public string NMDIAG { get; set; }
        public string NOKAPESERTA { get; set; }
        public string NAMAPESERTA { get; set; }
        public string PLAN { get; set; }
        public string PLANDESC { get; set; }
        public string KELAS { get; set; }
        public string KELASDESC { get; set; }
        public string NOMEDICALRECORD { get; set; }
        public string JENISKELAMIN { get; set; }
        public string TGLLAHIR { get; set; }
        public string KDBU { get; set; }
        public string NMBU { get; set; }
        public string PRODUKBADANUSAHA { get; set; }
        public string IDAKOMODASI { get; set; }
        public string TIPESJP { get; set; }
        public string TIPECOB { get; set; }
        public string NOBPJS { get; set; }
        public string KELASBPJS { get; set; }
    }
    #endregion

    #region 4. CETAK SJP

    public class ParamCetakSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string tkp { get; set; }
        public string tipefile { get; set; }
    }

    public class ResultCetakSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string FILENAME { get; set; }
        public string BYTEDATA { get; set; }
    }
    #endregion

    #region 5. CONFIRM ATK FIRST PAYOR

    public class ParamConfirmAKTFirstPayor
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string userid { get; set; }
    }

    public class ResultConfirmAKTFirstPayor
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string NOSEP { get; set; }
        public string KODEINACBGS { get; set; }
        public string DESKRIPSIINACBGS { get; set; }
        public string PROCEDURE { get; set; }
        public string DIAGNOSA { get; set; }
        public Int32 TOTALBIAYAINACBGS { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 6. ELIGIBILITAS PESERTA

    public class ParamEligibilitasPeserta
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nokainhealth { get; set; }
        public string tglpelayanan { get; set; }
        public string jenispelayanan { get; set; }
        public string poli { get; set; }
    }

    public class ResultEligibilitasPeserta
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOKAPST { get; set; }
        public string NMPST { get; set; }
        public string TGLLAHIR { get; set; }
        public string KODEPRODUK { get; set; }
        public string NAMAPRODUK { get; set; }
        public string KODEKELASRAWAT { get; set; }
        public string NAMAKELASRAWAT { get; set; }
        public string KODEBADANUSAHA { get; set; }
        public string NAMABADANUSAHA { get; set; }
        public string KODEPROVIDER { get; set; }
        public string NAMAPROVIDER { get; set; }
        public string NOKAPSTBPJS { get; set; }
        public string NMPSTBPJS { get; set; }
        public string KELASBPJS { get; set; }
        public string KODEPROVIDERBPJS { get; set; }
        public string NAMAPROVIDERBPJS { get; set; }
        public string FLAGPSTBPJS { get; set; }
        public string PRODUKCOB { get; set; }
        public string PRIORITAS { get; set; }
    }
    #endregion

    #region 7. HAPUS DETAIL SJP

    public class ParamHapusDetailSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string notes { get; set; }
        public string userid { get; set; }
    }

    public class ResultHapusDetailSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 8. HAPUS OBAT BY KODE OBAT INH

    public class ParamHapusObatByKodeObatInh
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string noresep { get; set; }
        public string kodeobat { get; set; }
        public string alasan { get; set; }
        public string user { get; set; }
    }

    public class ResultHapusObatByKodeObatInh
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string ALASANHAPUS { get; set; }
        public string USERID { get; set; }
    }
    #endregion

    #region 9. HAPUS OBAT BY KODE OBAT RS

    public class ParamHapusObatBykodeObatRS
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string noresep { get; set; }
        public string kodeobatrs { get; set; }
        public string alasan { get; set; }
        public string user { get; set; }
    }

    public class ResultHapusObatByKodeObatRS
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string ALASANHAPUS { get; set; }
        public string USERID { get; set; }
    }
    #endregion

    #region 10. HAPUS SJP

    public class ParamHapusSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string alasanhapus { get; set; }
        public string userid { get; set; }
    }

    public class ResultHapusSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string ALASANHAPUS { get; set; }
        public string USERID { get; set; }
    }
    #endregion

    #region 11. HAPUS TINDAKAN

    public class ParamHapusTindakan
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string kodetindakan { get; set; }
        public string tgltindakan { get; set; }
        public string notes { get; set; }
        public string userid { get; set; }
    }

    public class ResultHapusTindakan
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string KODETINDAKAN { get; set; }
        public string TGLTINDAKAN { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 12. INFO BENEFIT

    public class ParamInfoBenefit
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nokartu { get; set; }
        public string tanggal { get; set; }
        public string user { get; set; }
    }

    public class ResultInfoBenefit
    {
        public List<ModelInfoBenefit> ModelInfoBenefit { get; set; }
    }
    #endregion

    #region 13. INFO SJP

    public class ParamInfoSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
    }

    public class ResultInfoSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLSJP { get; set; }
        public string NOMORRUJUKAN { get; set; }
        public string TGLRUJUKAN { get; set; }
        public string KDPPKASALRUJUKAN { get; set; }
        public string NMPPKASALRUJUKAN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string KDDIAG { get; set; }
        public string NMDIAG { get; set; }
        public string NOKAPESERTA { get; set; }
        public string NAMAPESERTA { get; set; }
        public string PLAN { get; set; }
        public string PLANDESC { get; set; }
        public string KELAS { get; set; }
        public string KELASDESC { get; set; }
        public string NOMEDICALRECORD { get; set; }
        public string JENISKELAMIN { get; set; }
        public string TGLLAHIR { get; set; }
        public string KDBU { get; set; }
        public string NMBU { get; set; }
        public string IDAKOMODASI { get; set; }
        public string TIPESJP { get; set; }
        public string TIPECOB { get; set; }
        public string BYTAGSJP { get; set; }
        public string BYVERSJP { get; set; }
        public string NOFPK { get; set; }
        public string TKP { get; set; }
        public List<ListInfoRuangRawat> LISTINFORUANGRAWAT { get; set; }
        public List<ListInfoTindakan> LISTINFOTINDAKAN { get; set; }
    }

    public class ListInfoRuangRawat
    {
        public string NOSJP { get; set; }
        public string IDAKOMODASI { get; set; }
        public string KDRUANGRAWAT { get; set; }
        public string NMRUANGRAWAT { get; set; }
        public string KDKELAS { get; set; }
        public string NMKELAS { get; set; }
        public string TGLMASUK { get; set; }
        public string TGLKELUAR { get; set; }
        public string JUMLAHHARI { get; set; }
        public string BIAYARUANGRAWATPERHARI { get; set; }
        public string BIAYAAJU { get; set; }
        public string BIAYAVERIF { get; set; }
    }

    public class ListInfoTindakan
    {
        public string NOSJP { get; set; }
        public string TGLRUANGRAWAT { get; set; }
        public string TGLTINDAKAN { get; set; }
        public string KDTINDAKAN { get; set; }
        public string NMTINDAKAN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string KDDOKTER { get; set; }
        public string NMDOKTER { get; set; }
        public string BIAYAAJU { get; set; }
        public string BIAYAVERIF { get; set; }
    }

    #endregion

    #region 14. POLI
    public class ParamPoli
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string keyword { get; set; }
    }

    public class ResultPoli
    {
        //public List<ModelPoli> ModelPoli { get; set; }
        public string KDPROVIDER { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string STATUS { get; set; }
    }
    #endregion

    #region 15. PROSES SJP TO FPK

    public class ParamSJPtoFPK
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string jeniscob { get; set; }
        public string jenispelayanan { get; set; }
        public string listnosjp { get; set; }
        public string namapicprovider { get; set; }
        public string noinvoiceprovider { get; set; }
        public string username { get; set; }
    }

    public class ResultSJPtoFPK
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOFPK { get; set; }
        public Int32 BIAYATAGIHAN { get; set; }
        public string NAMAPICPROVIDER { get; set; }
        public string NOMORINVOICE { get; set; }
        public string USERID { get; set; }
    }
    #endregion

    #region 16. PROVIDER RUJUKAN

    public class ParamProviderRujukan
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string keyword { get; set; }
    }

    public class ResultProviderRujukan
    {
        //public List<ModelProviderRujukan> ModelProviderRujukan { get; set; }
        public string KDPROVIDER { get; set; }
        public string NMPROVIDER { get; set; }
        public string LOKASI { get; set; }
    }
    #endregion

    #region 17. REKAP HASIL VERIFIKASI

    public class ParamRekapHasilVerifikasi
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nofpk { get; set; }
    }

    public class ResultRekapHasilVerifikasi
    {
        public List<ModelRekapHasilVerifikasi> ModelRekapHasilVerifikasi { get; set; }
    }
    #endregion

    #region 18. SIMPAN BIAYA INACBGS

    public class ParamSimpanBiayaINACBGS
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string kodeinacbg { get; set; }
        public Int32 biayainacbg { get; set; }
        public string nosep { get; set; }
        public string notes { get; set; }
        public string userid { get; set; }
    }

    public class ResultSimpanBiayaINACBGS
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string KODEINACBGS { get; set; }
        public Int32 BIAYAINACBGS { get; set; }
        public string NOSEP { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 19. SIMPAN OBAT
    public class ParamSimpanObat
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string noresep { get; set; }
        public string tanggalresep { get; set; }
        public string tanggalobat { get; set; }
        public string tipeobat { get; set; }
        public string jenisracikan { get; set; }
        public string kodeobatrs { get; set; }
        public string namaobat { get; set; }
        public string kodedokter { get; set; }
        public Int32 jumlahobat { get; set; }
        public Int32 signa1 { get; set; }
        public Int32 signa2 { get; set; }
        public Int32 jumlahhari { get; set; }
        public Int32 hdasar { get; set; }
        public string confirmationcode { get; set; }
        public string username { get; set; }
    }

    public class ResultSimpanObat
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string KODEPROVIDER { get; set; }
        public string NOSJP { get; set; }
        public string NORESEP { get; set; }
        public string TGLOBAT { get; set; }
        public string TIPEOBAT { get; set; }
        public string TIPEOBATDESC { get; set; }
        public string JNSRACIKANOBT { get; set; }
        public string JNSRACIKANOBTDESC { get; set; }
        public string KODEOBATRS { get; set; }
        public string KODEOBATINHEALTH { get; set; }
        public string NAMAOBAT { get; set; }
        public string KDDOKTER { get; set; }
        public Int32 JUMLAHOBAT { get; set; }
        public Int32 SIGNA1 { get; set; }
        public Int32 SIGNA2 { get; set; }
        public Int32 JUMLAHHARI { get; set; }
        public Int32 HDASAR { get; set; }
        public Int32 FAKTOR { get; set; }
        public Int32 EMBALAGE { get; set; }
        public Int32 HARGATAGIHANOBAT { get; set; }
        public string CONFIRMATIONCODE { get; set; }
        public string USERNAME { get; set; }
    }
    #endregion

    #region 20. SIMPAN RUANG RAWAT

    public class ParamSimpanRuangRawat
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string tglmasuk { get; set; }
        public string kelasrawat { get; set; }
        public string kodejenispelayanan { get; set; }
        public Int32 byharirawat { get; set; }
    }

    public class ResultSimpanRuangRawat
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLMASUK { get; set; }
        public string KDPPK { get; set; }
        public string RRWT { get; set; }
        public string KELASRWT { get; set; }
        public string KDJENPEL { get; set; }
        public Int32 BYHARIRWT { get; set; }
        public Int32 ID { get; set; }
    }
    #endregion

    #region 21. SIMPAN SJP

    public class ParamSimpanSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string tanggalpelayanan { get; set; }
        public string jenispelayanan { get; set; }
        public string nokainhealth { get; set; }
        public string nomormedicalreport { get; set; }
        public string nomorasalrujukan { get; set; }
        public string kodeproviderasalrujukan { get; set; }
        public string tanggalasalrujukan { get; set; }
        public string kodediagnosautama { get; set; }
        public string poli { get; set; }
        public string username { get; set; }
        public string informasitambahan { get; set; }
        public string kodediagnosatambahan { get; set; }
        public Int32 kecelakaankerja { get; set; }
        public string kelasrawat { get; set; }
        public string kodejenpelruangrawat { get; set; }
    }

    public class ResultSimpanSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLSJP { get; set; }
        public string NOMORRUJUKAN { get; set; }
        public string TGLRUJUKAN { get; set; }
        public string KDPPKASALRUJUKAN { get; set; }
        public string NMPPKASALRUJUKAN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string KDDIAG { get; set; }
        public string NMDIAG { get; set; }
        public string NOKAPESERTA { get; set; }
        public string NAMAPESERTA { get; set; }
        public string PLAN { get; set; }
        public string PLANDESC { get; set; }
        public string KELAS { get; set; }
        public string KELASDESC { get; set; }
        public string NOMEDICALRECORD { get; set; }
        public string JENISKELAMIN { get; set; }
        public string TGLLAHIR { get; set; }
        public string KDBU { get; set; }
        public string NMBU { get; set; }
        public string PRODUKBADANUSAHA { get; set; }
        public string IDAKOMODASI { get; set; }
        public string TIPESJP { get; set; }
        public string TIPECOB { get; set; }
        public string NOBPJS { get; set; }
        public string KELASBPJS { get; set; }
    }
    #endregion

    #region 22. SIMPAN TINDAKAN

    public class ParamSimpanTindakan
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string jenispelayanan { get; set; }
        public string nosjp { get; set; }
        public string tglmasukrawat { get; set; }
        public string tanggalpelayanan { get; set; }
        public string kodetindakan { get; set; }
        public string poli { get; set; }
        public string kodedokter { get; set; }
        public Int32 biayaaju { get; set; }
    }

    public class ResultSimpanTindakan
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string TKP { get; set; }
        public string NOSJP { get; set; }
        public string TGLMSKRWT { get; set; }
        public string TGLPEL { get; set; }
        public string JENPEL { get; set; }
        public string NAMAJENPEL { get; set; }
        public string POLIUPF { get; set; }
        public string KDDOKTER { get; set; }
        public Int32 BYTAG { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 23. SIMPAN TINDAKAN RITL

    public class ParamSimpanTindakanRITL
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string jenispelayanan { get; set; }
        public string nosjp { get; set; }
        public string idakomodasi { get; set; }
        public string tglmasukrawat { get; set; }
        public string tanggalpelayanan { get; set; }
        public string kodetindakan { get; set; }
        public string poli { get; set; }
        public string kodedokter { get; set; }
        public Int32 biayaaju { get; set; }
    }

    public class ResultSimpanTindakanRITL
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TKP { get; set; }
        public string IDAKOMODASI { get; set; }
        public string TGLMSKRWT { get; set; }
        public string TGLPEL { get; set; }
        public string JENPEL { get; set; }
        public string NAMAJENPEL { get; set; }
        public string POLIUPF { get; set; }
        public string KDDOKTER { get; set; }
        public Int32 BYTAG { get; set; }
        public string NOTES { get; set; }
    }
    #endregion

    #region 24. UPDATE SJP

    public class ParamUpdateSJP
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nosjp { get; set; }
        public string nomormedicalreport { get; set; }
        public string nomorasalrujukan { get; set; }
        public string kodeproviderasalrujukan { get; set; }
        public string tanggalasalrujukan { get; set; }
        public string kodediagnosautama { get; set; }
        public string poli { get; set; }
        public string username { get; set; }
        public string informasitambahan { get; set; }
        public string kodediagnosatambahan { get; set; }
        public Int32 kecelakaankerja { get; set; }
    }

    public class ResultUpdateSJP
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLSJP { get; set; }
        public string NOMORRUJUKAN { get; set; }
        public string TGLRUJUKAN { get; set; }
        public string KDPPASALRUJUKAN { get; set; }
        public string NMPPASALRUJUKAN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string KDDIAG { get; set; }
        public string NMDIAG { get; set; }
        public string NOKAPESERTA { get; set; }
        public string NAMAPESERTA { get; set; }
        public string PLAN { get; set; }
        public string PLANDESC { get; set; }
        public string KELAS { get; set; }
        public string KELASDESC { get; set; }
        public string NOMEDICALRECORD { get; set; }
        public string JENISKELAMIN { get; set; }
        public string TGLLAHIR { get; set; }
        public string KDBU { get; set; }
        public string NMBU { get; set; }
        public string PRODUKBADANUSAHA { get; set; }
        public string IDAKOMODASI { get; set; }
        public string TIPESJP { get; set; }
        public string TIPECOB { get; set; }
        public string NOBPJS { get; set; }
        public string KELASBPJS { get; set; }
    }
    #endregion

    #region 25. UPDATE TANGGAL PULANG

    public class ParamUpdateTanggalPulang
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public Int32 id { get; set; }
        public string nosjp { get; set; }
        public string tglmasuk { get; set; }
        public string tglkeluar { get; set; }
    }

    public class ResultUpdateTanggalPulang
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLMASUK { get; set; }
        public string TGLKELUAR { get; set; }
        public string KDPPK { get; set; }
        public string RRWT { get; set; }
        public string KELASRWT { get; set; }
        public string KDJENPEL { get; set; }
        public Int32 BYHARIRWT { get; set; }
        public Int32 JMLHARI { get; set; }
        public Int32 BYRWT { get; set; }
    }
    #endregion

    #region PRA REGISTRASI

    public class PraRegistrasiListBodyRequest
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string tanggal { get; set; }
        public string query { get; set; }
    }

    public class PraRegistrasiListResponse
    {
        public string PRAREGISTRATIONNUMBER { get; set; }
        public string NOKAPESERTA { get; set; }
        public string NMPST { get; set; }
        public DateTime TANGGAL { get; set; }
        public DateTime TGLLAHIR { get; set; }
        public string NOHP { get; set; }
        public string KODEPOLI { get; set; }
        public string NAMAPOLI { get; set; }
        public string STATUS { get; set; }

        public string TujuanPoli
        {
            get
            {
                return string.Format("{0} - {1}", KODEPOLI, NAMAPOLI);
            }
        }

        public string cfTanggalKunjungan
        {
            get
            {
                return TANGGAL.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }

        public string cfTanggalLahir
        {
            get
            {
                return TGLLAHIR.ToString(Constant.FormatString.DATE_FORMAT);
            }
        }
    }



    public class PraRegistrasiDetailBodyRequest
    {
        public string token { get; set; }
        public string kodeprovider { get; set; }
        public string nomorpraregistrasi { get; set; }
    }


    public class PraRegistrasiDetailResponse
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string RESPONSECODE_ELIGIBILITAS { get; set; }
        public string RESPONSEDESC_ELIGIBILITAS { get; set; }
        public string NOPRAREGISTRASI { get; set; }
        public DateTime TGLCHECKIN { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string NOMORRUJUKAN { get; set; }
        public DateTime TGLRUJUKAN { get; set; }
        public string KDPPKASALRUJUKAN { get; set; }
        public string NMPPKASALRUJUKAN { get; set; }
        public string NOKAPST { get; set; }
        public string NMPST { get; set; }
        public string KODEBADANUSAHA { get; set; }
        public string NAMABADANUSAHA { get; set; }
        public DateTime TGLLAHIR { get; set; }
        public string KODEPRODUK { get; set; }
        public string NAMAPRODUK { get; set; }
        public string KODEKELASRAWAT { get; set; }
        public string NAMAKELASRAWAT { get; set; }
        public string KODEDOKKEL { get; set; }
        public string NAMADOKKEL { get; set; }
        public string NOKAPSTBPJS { get; set; }
        public string NMPSTBPJS { get; set; }
        public string KELASBPJS { get; set; }
        public string KODEPROVIDERBPJS { get; set; }
        public string NAMAPROVIDERBPJS { get; set; }
        public string FLAGPSTBPJS { get; set; }
        public string PRODUKCOB { get; set; }
        public string PRIORITAS { get; set; }
        public DateTime TGLMULAIPST { get; set; }
        public DateTime TGLAKHIRPST { get; set; }
        public string JENISKELAMIN { get; set; }
        public string NOHP { get; set; }
        public string EMAIL { get; set; }
        public string NOMEDICALRECORD { get; set; }
        public string KDDIAG { get; set; }
        public string NMDIAG { get; set; }
        public string CATATANKHUSUS { get; set; }
    }


    #endregion

    #region Inline Model

    public class ModelInfoBenefit
    {
        public string BENEFIT { get; set; }
        public string BENEFITDESC { get; set; }
    }

    public class ModelPoli
    {
        public string KDPROVIDER { get; set; }
        public string KDPOLI { get; set; }
        public string NMPOLI { get; set; }
        public string STATUS { get; set; }
    }

    public class ModelProviderRujukan
    {
        public string KDPROVIDER { get; set; }
        public string NMPROVIDER { get; set; }
        public string LOKASI { get; set; }
    }

    public class ModelRekapHasilVerifikasi
    {
        public string ERRORCODE { get; set; }
        public string ERRORDESC { get; set; }
        public string NOSJP { get; set; }
        public string TGLSJP { get; set; }
        public string TGLMASUK { get; set; }
        public string TGLPELAYANAN { get; set; }
        public string KDJENPEL { get; set; }
        public string NMJENPEL { get; set; }
        public Int32 BYTAG { get; set; }
        public Int32 BYVER { get; set; }
    }

    #endregion
}
