using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common
{
    public class EKlaimMetadata
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    #region 1. New Claim [Membuat klaim baru (dan registrasi pasien jika belum ada)]

    public class NewClaimMethod
    {
        public NewClaimMetadata metadata { get; set; }
        public NewClaimData data { get; set; }
    }

    public class NewClaimMetadata
    {
        public string method { get; set; }
    }

    public class NewClaimData
    {
        public string nomor_kartu { get; set; }
        public string nomor_sep { get; set; }
        public string nomor_rm { get; set; }
        public string nama_pasien { get; set; }
        public string tgl_lahir { get; set; } // YYYY-MM-DD hh:mm:ss
        public string gender { get; set; } // 1 = Laki-laki, 2 = Perempuan
    }

    public class NewClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public NewClaimResponseData response { get; set; }
    }

    public class NewClaimResponseData
    {
        public string patient_id { get; set; }
        public string admission_id { get; set; }
        public string hospital_admission_id { get; set; }
    }

    public class NewClaimResponseDuplicate
    {
        public EKlaimMetadata metadata { get; set; }
        public List<NewClaimResponseDataDuplicate> duplicate { get; set; }
    }

    public class NewClaimResponseDataDuplicate
    {
        public string nama_pasien { get; set; }
        public string nomor_rm { get; set; }
        public string tgl_masuk { get; set; }
    }

    #endregion

    #region 2. Update Patient [Update data pasien]

    public class UpdatePatientMethod
    {
        public UpdatePatientMetadata metadata { get; set; }
        public UpdatePatientData data { get; set; }
    }

    public class UpdatePatientMetadata
    {
        public string method { get; set; }
        public string nomor_rm { get; set; }
    }

    public class UpdatePatientData
    {
        public string nomor_kartu { get; set; }
        public string nomor_rm { get; set; }
        public string nama_pasien { get; set; }
        public string tgl_lahir { get; set; } // YYYY-MM-DD hh:mm:ss
        public string gender { get; set; } // 1 = Laki-laki, 2 = Perempuan
    }

    public class UpdatePatientResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 3. Delete Patient [Hapus data pasien]

    public class DeletePatientMethod
    {
        public DeletePatientMetadata metadata { get; set; }
        public DeletePatientData data { get; set; }
    }

    public class DeletePatientMetadata
    {
        public string method { get; set; }
    }

    public class DeletePatientData
    {
        public string nomor_rm { get; set; }
        public string coder_nik { get; set; }
    }

    public class DeletePatientResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 4. Set Claim [Untuk mengisi/update data klaim]

    public class SetClaimMethod
    {
        public SetClaimMetadata metadata { get; set; }
        public SetClaimData data { get; set; }
    }

    public class SetClaimMetadata
    {
        public string method { get; set; }
        public string nomor_sep { get; set; }
    }

    public class SetClaimData
    {
        public string nomor_sep { get; set; }
        public string nomor_kartu { get; set; }
        public string tgl_masuk { get; set; }
        public string tgl_pulang { get; set; }
        public string cara_masuk { get; set; }
        public string jenis_rawat { get; set; } // 1 = rawat inap, 2 = rawat jalan
        public string kelas_rawat { get; set; } // 3 = Kelas 3, 2 = Kelas 2, 1 = Kelas 1
        public string adl_sub_acute { get; set; }
        public string adl_chronic { get; set; }
        public string icu_indikator { get; set; }
        public string icu_los { get; set; }
        public string ventilator_hour { get; set; }
        public ventilator ventilator { get; set; }
        public string upgrade_class_ind { get; set; }
        public string upgrade_class_class { get; set; }
        public string upgrade_class_los { get; set; }
        public string upgrade_class_payor { get; set; }
        public string add_payment_pct { get; set; }
        public string birth_weight { get; set; }
        public int sistole { get; set; }
        public int diastole { get; set; }
        public string discharge_status { get; set; }
        public string diagnosa { get; set; }
        public string procedure { get; set; }
        public string diagnosa_inagrouper { get; set; }
        public string procedure_inagrouper { get; set; }
        public tarif_rs tarif_rs { get; set; }
        public string pemulasaraan_jenazah { get; set; }
        public string kantong_jenazah { get; set; }
        public string peti_jenazah { get; set; }
        public string plastik_erat { get; set; }
        public string desinfektan_jenazah { get; set; }
        public string mobil_jenazah { get; set; }
        public string desinfektan_mobil_jenazah { get; set; }
        public string covid19_status_cd { get; set; }
        public string nomor_kartu_t { get; set; }
        public string episodes { get; set; }
        public string covid19_cc_ind { get; set; }
        public string covid19_rs_darurat_ind { get; set; }
        public string covid19_co_insidense_ind { get; set; }
        public covid19_penunjang_pengurang covid19_penunjang_pengurang { get; set; }
        public string terapi_konvalesen { get; set; }
        public string akses_naat { get; set; }
        public string isoman_ind { get; set; }
        public string bayi_lahir_status_cd { get; set; }
        public string dializer_single_use { get; set; }
        public int kantong_darah { get; set; }
        public apgar apgar { get; set; }
        public persalinan persalinan { get; set; }
        public string tarif_poli_eks { get; set; }
        public string nama_dokter { get; set; }
        public string kode_tarif { get; set; }
        public string payor_id { get; set; }
        public string payor_cd { get; set; }
        public string cob_cd { get; set; }
        public string coder_nik { get; set; }
    }
    public class ventilator
    {
        public string use_ind { get; set; }
        public string start_dttm { get; set; }
        public string stop_dttm { get; set; }
    }
    public class tarif_rs
    {
        public string prosedur_non_bedah { get; set; }
        public string prosedur_bedah { get; set; }
        public string konsultasi { get; set; }
        public string tenaga_ahli { get; set; }
        public string keperawatan { get; set; }
        public string penunjang { get; set; }
        public string radiologi { get; set; }
        public string laboratorium { get; set; }
        public string pelayanan_darah { get; set; }
        public string rehabilitasi { get; set; }
        public string kamar { get; set; }
        public string rawat_intensif { get; set; }
        public string obat { get; set; }
        public string obat_kronis { get; set; }
        public string obat_kemoterapi { get; set; }
        public string alkes { get; set; }
        public string bmhp { get; set; }
        public string sewa_alat { get; set; }
    }
    public class covid19_penunjang_pengurang
    {
        public string lab_asam_laktat { get; set; }
        public string lab_procalcitonin { get; set; }
        public string lab_crp { get; set; }
        public string lab_kultur { get; set; }
        public string lab_d_dimer { get; set; }
        public string lab_pt { get; set; }
        public string lab_aptt { get; set; }
        public string lab_waktu_pendarahan { get; set; }
        public string lab_anti_hiv { get; set; }
        public string lab_albumin { get; set; }
        public string lab_analisa_gas { get; set; }
        public string rad_thorax_ap_pa { get; set; }
    }
    public class apgar
    {
        public menit_1 menit_1 { get; set; }
        public menit_5 menit_5 { get; set; }
    }
    public class menit_1
    {
        public int appearance { get; set; }
        public int pulse { get; set; }
        public int grimace { get; set; }
        public int activity { get; set; }
        public int respiration { get; set; }
    }
    public class menit_5
    {
        public int appearance { get; set; }
        public int pulse { get; set; }
        public int grimace { get; set; }
        public int activity { get; set; }
        public int respiration { get; set; }
    }
    public class persalinan
    {
        public int usia_kehamilan { get; set; }
        public int gravida { get; set; }
        public int partus { get; set; }
        public int abortus { get; set; }
        public string onset_kontraksi { get; set; }
        public List<delivery> delivery { get; set; }
    }
    public class delivery
    {
        public string delivery_sequence { get; set; }
        public string delivery_method { get; set; }
        public string delivery_dttm { get; set; }
        public string letak_janin { get; set; }
        public string kondisi { get; set; }
        public string use_manual { get; set; }
        public string use_forcep { get; set; }
        public string use_vacuum { get; set; }
        public string shk_spesimen_ambil { get; set; }
        public string shk_alasan { get; set; }
        public string shk_lokasi { get; set; }
        public string shk_spesimen_dttm { get; set; }
    }

    public class SetClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 5. Grouping Stage 1 [Grouping Stage 1]

    public class GroupingStage1Method
    {
        public GroupingStage1Metadata metadata { get; set; }
        public GroupingStage1Data data { get; set; }
    }

    public class GroupingStage1Metadata
    {
        public string method { get; set; }
        public string stage { get; set; }
    }

    public class GroupingStage1Data
    {
        public string nomor_sep { get; set; }
    }

    public class GroupingStage1Response
    {
        public EKlaimMetadata metadata { get; set; }
        public GroupingStage1ResponseData response { get; set; }
        public List<GroupingStage1SpecialCMGOption> special_cmg_option { get; set; }
        public List<GroupingStage1TarifAlt> tarif_alt { get; set; }
    }

    public class GroupingStage1ResponseData
    {
        public cbg cbg { get; set; }
        public sub_acute sub_acute { get; set; }
        public chronic chronic { get; set; }
        public string kelas { get; set; }
        public string add_payment_amt { get; set; }
        public string inacbg_version { get; set; }
    }

    public class cbg
    {
        public string code { get; set; }
        public string description { get; set; }
        public string tariff { get; set; }
    }

    public class sub_acute
    {
        public string code { get; set; }
        public string description { get; set; }
        public string tariff { get; set; }
    }

    public class chronic
    {
        public string code { get; set; }
        public string description { get; set; }
        public string tariff { get; set; }
    }

    public class GroupingStage1SpecialCMGOption
    {
        public string code { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

    public class GroupingStage1TarifAlt
    {
        public string kelas { get; set; }
        public string tarif_inacbg { get; set; }
    }


    #endregion

    #region 6. Grouping Stage 2 [Grouping Stage 2]

    public class GroupingStage2Method
    {
        public GroupingStage2Metadata metadata { get; set; }
        public GroupingStage2Data data { get; set; }
    }

    public class GroupingStage2Metadata
    {
        public string method { get; set; }
        public string stage { get; set; }
    }

    public class GroupingStage2Data
    {
        public string nomor_sep { get; set; }
        public string special_cmg { get; set; }
    }

    public class GroupingStage2Response
    {
        public EKlaimMetadata metadata { get; set; }
        public GroupingStage2ResponseData response { get; set; }
        public List<GroupingStage2SpecialCMGOption> special_cmg_option { get; set; }
        public List<GroupingStage2TarifAlt> tarif_alt { get; set; }
    }

    public class GroupingStage2ResponseData
    {
        public cbg cbg { get; set; }
        public List<special_cmg> special_cmg { get; set; }
        public string kelas { get; set; }
        public string add_payment_amt { get; set; }
        public string inacbg_version { get; set; }

    }

    public class special_cmg
    {
        public string code { get; set; }
        public string description { get; set; }
        public decimal tariff { get; set; }
        public string type { get; set; }
    }

    public class GroupingStage2SpecialCMGOption
    {
        public string code { get; set; }
        public string description { get; set; }
        public string type { get; set; }
    }

    public class GroupingStage2TarifAlt
    {
        public string kelas { get; set; }
        public string tarif_inacbg { get; set; }
        public string tarif_sp { get; set; }
        public string tarif_sr { get; set; }
    }

    #endregion

    #region 7. Claim Final [Untuk finalisasi klaim]

    public class ClaimFinalMethod
    {
        public ClaimFinalMetadata metadata { get; set; }
        public ClaimFinalData data { get; set; }
    }

    public class ClaimFinalMetadata
    {
        public string method { get; set; }
    }

    public class ClaimFinalData
    {
        public string nomor_sep { get; set; }
        public string coder_nik { get; set; }
    }

    public class ClaimFinalResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 8. Reedit Claim [Untuk mengedit ulang klaim]

    public class ReeditClaimMethod
    {
        public ReeditClaimMetadata metadata { get; set; }
        public ReeditClaimData data { get; set; }
    }

    public class ReeditClaimMetadata
    {
        public string method { get; set; }
    }

    public class ReeditClaimData
    {
        public string nomor_sep { get; set; }
    }

    public class ReeditClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 9. Send Claim [Untuk mengirim klaim ke data center (kolektif per hari)]

    public class SendClaimMethod
    {
        public SendClaimMetadata metadata { get; set; }
        public SendClaimData data { get; set; }
    }

    public class SendClaimMetadata
    {
        public string method { get; set; }
    }

    public class SendClaimData
    {
        public string start_dt { get; set; }
        public string stop_dt { get; set; }
        public string jenis_rawat { get; set; }
        public string date_type { get; set; }
    }

    public class SendClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SendClaimResponseData response { get; set; }
    }

    public class SendClaimResponseData
    {
        public List<SendClaimResponseDataDetail> data { get; set; }
    }

    public class SendClaimResponseDataDetail
    {
        public string SEP { get; set; }
        public string tgl_pulang { get; set; }
        public string kemkes_dc_Status { get; set; }
        public string bpjs_dc_Status { get; set; }
    }

    #endregion

    #region 10. Send Claim Individual [Untuk mengirim klaim individual ke data center]

    public class SendClaimIndividualMethod
    {
        public SendClaimIndividualMetadata metadata { get; set; }
        public SendClaimIndividualData data { get; set; }
    }

    public class SendClaimIndividualMetadata
    {
        public string method { get; set; }
    }

    public class SendClaimIndividualData
    {
        public string nomor_sep { get; set; }
    }

    public class SendClaimIndividualResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SendClaimIndividualResponseData response { get; set; }
    }

    public class SendClaimIndividualResponseData
    {
        public List<SendClaimIndividualResponseDataDetail> data { get; set; }
    }

    public class SendClaimIndividualResponseDataDetail
    {
        public string no_sep { get; set; }
        public string tgl_pulang { get; set; }
        public string kemkes_dc_status { get; set; }
        public string bpjs_dc_status { get; set; }
        public string cob_dc_status { get; set; }
    }

    #endregion

    #region 11. Pull Claim [Untuk menarik data klaim dari E-Klaim (method sudah ditutup)]

    public class PullClaimMethod
    {
        public PullClaimMetadata metadata { get; set; }
        public PullClaimData data { get; set; }
    }

    public class PullClaimMetadata
    {
        public string method { get; set; }
    }

    public class PullClaimData
    {
        public string start_dt { get; set; }
        public string stop_dt { get; set; }
        public string jenis_rawat { get; set; }
    }

    public class PullClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public PullClaimResponseData response { get; set; }
    }

    public class PullClaimResponseData
    {
        public string data { get; set; }
    }

    #endregion

    #region 12. Get Claim [Untuk mengambil data detail per klaim]

    public class GetClaimMethod
    {
        public GetClaimMetadata metadata { get; set; }
        public GetClaimData data { get; set; }
    }

    public class GetClaimMetadata
    {
        public string method { get; set; }
    }

    public class GetClaimData
    {
        public string nomor_sep { get; set; }
    }

    public class GetClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public GetClaimDataHDResponse response { get; set; }
    }
    public class GetClaimDataHDResponse
    {
        public GetClaimResponseData data { get; set; }
    }
    public class GetClaimResponseData
    {
        public string kode_rs { get; set; }
        public string kelas_rs { get; set; }
        public string kelas_rawat { get; set; }
        public string kode_tarif { get; set; }
        public string jenis_rawat { get; set; }
        public string tgl_masuk { get; set; }
        public string tgl_pulang { get; set; }
        public string tgl_lahir { get; set; }
        public string berat_lahir { get; set; }
        public string discharge_status { get; set; }
        public string diagnosa { get; set; }
        public string procedure { get; set; }
        public string adl_sub_acute { get; set; }
        public string adl_chronic { get; set; }
        public tarif_rs tarif_rs { get; set; }
        public string los { get; set; }
        public string icu_indikator { get; set; }
        public string icu_los { get; set; }
        public string ventilator_hour { get; set; }
        public string upgrade_class_ind { get; set; }
        public string upgrade_class_class { get; set; }
        public string upgrade_class_los { get; set; }
        public string add_payment_pct { get; set; }
        public string add_payment_amt { get; set; }
        public string nama_pasien { get; set; }
        public string nomor_rm { get; set; }
        public string umur_tahun { get; set; }
        public string umur_hari { get; set; }
        public string tarif_poli_eks { get; set; }
        public string nama_dokter { get; set; }
        public string nomor_sep { get; set; }
        public string nomor_kartu { get; set; }
        public string payor_id { get; set; }
        public string payor_nm { get; set; }
        public string coder_nm { get; set; }
        public string coder_nik { get; set; }
        public string patient_id { get; set; }
        public string admission_id { get; set; }
        public string hospital_admission_id { get; set; }
        public string grouping_count { get; set; }
        public grouper grouper { get; set; }
        public string kemenkes_dc_status_cd { get; set; }
        public string kemenkes_dc_sent_dttm { get; set; }
        public string bpjs_dc_status_cd { get; set; }
        public string bpjs_dc_sent_dttm { get; set; }
        public string klaim_status_cd { get; set; }
        public string bpjs_klaim_status_cd { get; set; }
        public string bpjs_klaim_status_nm { get; set; }
    }

    public class grouper
    {
        public response response { get; set; }
        public List<tarif_alt> tarif_alt { get; set; }
    }

    public class response
    {
        public cbg cbg { get; set; }
        public List<special_cmg> special_cmg { get; set; }
        public string inacbg_version { get; set; }
    }

    public class tarif_alt
    {
        public string kelas { get; set; }
        public string tarif_inacbg { get; set; }
        public string tarif_sp { get; set; }
        public string tarif_sr { get; set; }
    }

    #endregion

    #region 13. Get Claim Status [Untuk mengambil status per klaim]

    public class GetClaimStatusMethod
    {
        public GetClaimStatusMetadata metadata { get; set; }
        public GetClaimStatusData data { get; set; }
    }

    public class GetClaimStatusMetadata
    {
        public string method { get; set; }
    }

    public class GetClaimStatusData
    {
        public string nomor_sep { get; set; }
    }

    public class GetClaimStatusResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public GetClaimStatusResponseData response { get; set; }
    }

    public class GetClaimStatusResponseData
    {
        public string kdStatusSep { get; set; }
        public string nmStatusSep { get; set; }
    }

    #endregion

    #region 14. Delete Claim [Untuk menghapus klaim]

    public class DeleteClaimMethod
    {
        public DeleteClaimMetadata metadata { get; set; }
        public DeleteClaimData data { get; set; }
    }

    public class DeleteClaimMetadata
    {
        public string method { get; set; }
    }

    public class DeleteClaimData
    {
        public string nomor_sep { get; set; }
        public string coder_nik { get; set; }
    }

    public class DeleteClaimResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public DeleteClaimResponseData response { get; set; }
    }

    public class DeleteClaimResponseData
    {
        public string nomor_sep { get; set; }
        public string coder_nik { get; set; }
    }

    #endregion

    #region 15. Claim Print [Cetak klaim]

    public class ClaimPrintMethod
    {
        public ClaimPrintMetadata metadata { get; set; }
        public ClaimPrintData data { get; set; }
    }

    public class ClaimPrintMetadata
    {
        public string method { get; set; }
    }

    public class ClaimPrintData
    {
        public string nomor_sep { get; set; }
    }

    public class ClaimPrintResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public string data { get; set; }
    }

    #endregion

    #region 16. Search Diagnosis [Pencarian diagnosa]

    public class SearchDiagnosisMethod
    {
        public SearchDiagnosisMetadata metadata { get; set; }
        public SearchDiagnosisData data { get; set; }
    }

    public class SearchDiagnosisMetadata
    {
        public string method { get; set; }
    }

    public class SearchDiagnosisData
    {
        public string keyword { get; set; } //diisi dengan kode, sebagian dari kode, atau sebagian dari nama diagnosa
    }

    public class SearchDiagnosisResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SearchDiagnosisResponseData response { get; set; }
    }

    public class SearchDiagnosisResponseData
    {
        public string count { get; set; }
        public List<List<string>> data { get; set; }
    }

    #endregion

    #region 17. Search Procedures [Pencarian prosedur]

    public class SearchProceduresMethod
    {
        public SearchProceduresMetadata metadata { get; set; }
        public SearchProceduresData data { get; set; }
    }

    public class SearchProceduresMetadata
    {
        public string method { get; set; }
    }

    public class SearchProceduresData
    {
        public string keyword { get; set; } //diisi dengan kode, sebagian dari kode, atau sebagian dari nama prosedur
    }

    public class SearchProceduresResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SearchProceduresResponseData response { get; set; }
    }

    public class SearchProceduresResponseData
    {
        public string count { get; set; }
        public List<List<string>> data { get; set; }
    }

    #endregion

    #region 19. Upload File

    public class UploadFileMethod
    {
        public UploadFileMetadata metadata { get; set; }
        public string data { get; set; }
    }

    public class UploadFileMetadata
    {
        public string method { get; set; }
        public string nomor_sep { get; set; }
        public string file_class { get; set; }
        public string file_name { get; set; }
    }

    public class UploadFileResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public UploadFileResponseData response { get; set; }
    }

    public class UploadFileResponseData
    {
        public string file_id { get; set; }
        public string file_name { get; set; }
        public string file_type { get; set; }
        public string file_size { get; set; }
        public string file_class { get; set; }
    }

    #endregion

    #region 20. Hapus File

    public class DeleteFileMethod
    {
        public DeleteFileMetadata metadata { get; set; }
        public DeleteFileData data { get; set; }
    }

    public class DeleteFileMetadata
    {
        public string method { get; set; }
    }

    public class DeleteFileData
    {
        public string nomor_sep { get; set; }
        public string file_id { get; set; }
    }

    public class DeleteFileResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region 20. Daftar File >>> response belum definisiin
    
    public class DaftarFileMethod
    {
        public DaftarFileMetadata metadata { get; set; }
        public DaftarFileData data { get; set; }
    }

    public class DaftarFileMetadata
    {
        public string method { get; set; }
    }

    public class DaftarFileData
    {
        public string nomor_sep { get; set; }
    }

    #endregion

    #region 20. Check Status Klaim (Retrieve Claim Status)

    public class RetrieveClaimStatusMethod
    {
        public RetrieveClaimStatusMetadata metadata { get; set; }
        public RetrieveClaimStatusData data { get; set; }
    }

    public class RetrieveClaimStatusMetadata
    {
        public string method { get; set; }
    }

    public class RetrieveClaimStatusData
    {
        public string nomor_sep { get; set; }
        public string nomor_pengajuan { get; set; }
    }

    public class RetrieveClaimStatusResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public RetrieveClaimStatusDetail response { get; set; }
    }

    public class RetrieveClaimStatusDetail
    {
        public string nomor_pengajuan { get; set; }
        public string nomor_sep { get; set; }
        public string bpjs_statusKlaim { get; set; }
        public string bpjs_keterangan { get; set; }
        public string yankes_tahap_cd { get; set; }
        public string yankes_tahap_msg { get; set; }
        public string yankes_status_cd { get; set; }
        public string yankes_status_msg { get; set; }
        public string yankes_revisi_cd { get; set; }
        public string yankes_revisi_msg { get; set; }
        public string yankes_bayar_cd { get; set; }
        public string yankes_bayar_msg { get; set; }
    }

    #endregion

    #region 21. Search Diagnosis INA Grouper [Pencarian diagnosa]

    public class SearchDiagnosisINAGrouperMethod
    {
        public SearchDiagnosisINAGrouperMetadata metadata { get; set; }
        public SearchDiagnosisINAGrouperData data { get; set; }
    }

    public class SearchDiagnosisINAGrouperMetadata
    {
        public string method { get; set; }
    }

    public class SearchDiagnosisINAGrouperData
    {
        public string keyword { get; set; } //diisi dengan kode, sebagian dari kode, atau sebagian dari nama diagnosa
    }

    public class SearchDiagnosisINAGrouperResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SearchDiagnosisINAGrouperResponseData response { get; set; }
    }

    public class SearchDiagnosisINAGrouperResponseData
    {
        public string count { get; set; }
        public List<DiagnosisINAGrouperData> data { get; set; }
    }

    public class DiagnosisINAGrouperData
    {
        public string description { get; set; }
        public string code { get; set; }
        public string validcode { get; set; }
        public string accpdx { get; set; }
        public string code_asterisk { get; set; }
        public string asterisk { get; set; }
        public string im { get; set; }
    }

    #endregion

    #region 22. Search Procedures INA Grouper [Pencarian prosedur]

    public class SearchProceduresINAGrouperMethod
    {
        public SearchProceduresINAGrouperMetadata metadata { get; set; }
        public SearchProceduresINAGrouperData data { get; set; }
    }

    public class SearchProceduresINAGrouperMetadata
    {
        public string method { get; set; }
    }

    public class SearchProceduresINAGrouperData
    {
        public string keyword { get; set; } //diisi dengan kode, sebagian dari kode, atau sebagian dari nama prosedur
    }

    public class SearchProceduresINAGrouperResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SearchProceduresINAGrouperResponseData response { get; set; }
    }

    public class SearchProceduresINAGrouperResponseData
    {
        public string count { get; set; }
        public List<ProceduresINAGrouperData> data { get; set; }
    }

    public class ProceduresINAGrouperData
    {
        public string description { get; set; }
        public string code { get; set; }
        public string validcode { get; set; }
        public string im { get; set; }
    }

    #endregion

    #region 23. Validasi Nomor Register SITB

    public class SITBValidateMethod
    {
        public SITBValidateMetadata metadata { get; set; }
        public SITBValidateData data { get; set; }
    }

    public class SITBValidateMetadata
    {
        public string method { get; set; }
    }

    public class SITBValidateData
    {
        public string nomor_sep { get; set; }
        public string nomor_register_sitb { get; set; }

    }

    public class SITBValidateResponse
    {
        public EKlaimMetadata metadata { get; set; }
        public SITBValidateResponseData response { get; set; }
    }

    public class SITBValidateResponseData
    {
        public string status { set; get; }
        public string detail { get; set; }
        public SITBValidateValidationData validation { get; set; }
    }

    public class SITBValidateValidationData
    {
        public List<SITBValidateValidationDataDetail> data { get; set; }
        public bool success { get; set; }
    }

    public class SITBValidateValidationDataDetail
    {
        public string id { set; get; }
        public string nama { get; set; }
        public string nik { set; get; }
        public string jenis_kelamin_id { get; set; }
    }

    #endregion

    #region 24. Membatalkan Validasi Nomor Register SITB

    public class SITBInvalidateMethod
    {
        public SITBInvalidateMetadata metadata { get; set; }
        public SITBInvalidateData data { get; set; }
    }

    public class SITBInvalidateMetadata
    {
        public string method { get; set; }
    }

    public class SITBInvalidateData
    {
        public string nomor_sep { get; set; }
    }

    public class SITBInvalidateResponse
    {
        public EKlaimMetadata metadata { get; set; }
    }

    #endregion

    #region Diagnosa Data list
    public class EklaimDiagnosaDataModel
    {
        public string description { get; set; }
        public string code { get; set; }
        public string validcode { get; set; }
        public string accpdx { get; set; }
        public string code_asterisk { get; set; }
        public string asterisk { get; set; }
        public string im { get; set; }
    }
    #endregion

}