using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class Constant
    {

        #region Healthcare Parameter
        public static class HealthcareParameter
        {
            public const string ER_DEFAULT_PHYSICIAN = "ER2001";
            public const string IS_CONFIRM_PURCHASE_RECEIVE = "IM0002";
            public const string DEFAULT_CYCLE_COUNT_TYPE = "IM0003";
            public const string RANGE_EXPIRED_DATE = "IM0004";
            public const string IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE = "IM0005";
            public const string IS_DISCOUNT_APPLIED_TO_UNIT_PRICE = "IM0006";
            public const string IS_ITEM_DISTRIBUTION_AUTO_RECEIVED = "IM0007";

            public const string FISCAL_YEAR_PERIOD = "fiscalYearPeriod";
            public const string FISCAL_YEAR_START_MONTH = "fiscalYearStartMonth";

            public const string AC_CUTOFF_JOURNAL = "ac_cutoff_journal";
            public const string AC_DEFAULT_SEGMENT_REVENUE_COST_CENTER = "ac_defaultsegmentrcc";
            public const string AC_DEFAULT_SEGMENT_DEPARTMENT = "ac_defaultsegmentdept";
            public const string AC_DEFAULT_SEGMENT_SERVICE_UNIT = "ac_defaultsegmentserv";
            public const string AC_DEFAULT_SEGMENT_CUSTOMER_GROUP = "ac_defaultsegmentcstgr";
            public const string AC_DEFAULT_SEGMENT_BUSINESS_PARTNER = "ac_defaultsegmentbpart";
            public const string AC_DELIMETER_SEGMENT = "ac_delimetersegment";
            public const string AC_COA_AP_DOCTOR_FEE = "coaAPDoctorFee";
            public const string AC_COA_DIRECT_PURCHASE = "kdcoa_pembelian_cash";
            public const string AC_COA_PERMINTAAN_SPK = "kdcoa_permintaan_spk";
            public const string AC_COA_REALISASI_SPK = "kdcoa_realisasi_spk";
            public const string AC_COA_KAS_BON = "kdcoa_kas_bon";
            public const string AC_COA_KONTRA_PIUTANG = "coaKontraPiutang";
        }
        #endregion

        #region Item Master
        public static class ItemMaster
        {
            public const string KODE_BIAYA_PENDAFTARAN = "BI00009";
        }
        #endregion

        #region Setting Parameter
        public static class SettingParameter
        {
            public const string SA0219 = "SA0219";
            public const string SA0218 = "SA0218";
            public const string IS0026 = "IS0026";
            public const string EM_Display_Next_Visit_Schedule = "EM0041";

            public const string AC_IS_BRIDGING_USING_SUMMARY = "AC0002";
            public const string AC_PREFIX_ASSET_CODE = "AC0003";
            public const string KASI_PENGELOLAAN_INVENTARIS = "AC0004";
            public const string PENDING_HUTANG = "AC0007";
            public const string ARRECEIVING_FROM_TREASURY_CAN_DELETE_OR_VOID = "AC0010";
            public const string FADEPRECIATION_FROM_APPROVE_FAACCEPTANCE = "AC0011";
            public const string IS_APPROVED_FAACCEPTANCE_REPLACE_DEPRECIATIONSTARTDATE = "AC0013";
            public const string AC_FILE_NAME_DOCUMENT_DOWNLOAD_GLTRANSDT = "AC0014";
            public const string AC_FORMAT_CETAKAN_LABEL_ASSET = "AC0016";
            public const string AC_JENIS_PRINTER_LABEL_ASSET = "AC0018";
            public const string IS_DISCOUNT_APPLIED_TO_FA_ITEM = "AC0019";
            public const string IS_PPN_APPLIED_TO_FA_ITEM = "AC0020";
            public const string AC0021 = "AC0021";

            public const string FN_DEFAULT_MARKUP_MARGIN = "FN0001";
            public const string DEFAULT_CITO_PERCENTAGE = "FN0002";
            public const string DEFAULT_COMPLICATION_PERCENTAGE = "FN0003";
            public const string TARIFF_COMPONENT1_TEXT = "FN0004";
            public const string TARIFF_COMPONENT2_TEXT = "FN0005";
            public const string TARIFF_COMPONENT3_TEXT = "FN0006";
            public const string VAT_PERCENTAGE = "FN0007";
            public const string PPH_PERCENTAGE = "FN0008";
            public const string FN_KODE_PELAYANAN_KARTU = "FN0009";
            public const string FN_KONTROL_BIAYA_KARTU = "FN0010";
            public const string FN_SPECIAL_MARKUP_PERCENTAGE = "FN0011";
            public const string FN_KONTROL_PEMBUATAN_TAGIHAN = "FN0012";
            public const string FN_KONTROL_KODE_BIAYA_ADMINISTRASI_RJ = "FN0013";
            public const string FN_KODE_BIAYA_ADMINISTRASI_RJ_INSTANSI = "FN0014";
            public const string FN_BIAYA_ADM_RI_DALAM_PERSENTASE = "FN0016";
            public const string FN_NILAI_BIAYA_ADM_RI = "FN0017";
            public const string FN_NILAI_MIN_BIAYA_ADM_RI = "FN0018";
            public const string FN_NILAI_MAX_BIAYA_ADM_RI = "FN0019";
            public const string FN_BIAYA_SERVICE_RI_DALAM_PERSENTASE = "FN0020";
            public const string FN_NILAI_BIAYA_SERVICE_RI = "FN0021";
            public const string FN_NILAI_MIN_BIAYA_SERVICE_RI = "FN0022";
            public const string FN_NILAI_MAX_BIAYA_SERVICE_RI = "FN0023";
            public const string FN_VALIDASI_TAGIHAN_KETIKA_PULANG = "FN0024";
            public const string FN_SELISIH_PASIEN_BPJS_NAIK_KELAS = "FN0025";
            public const string FN_TRANSFER_TAGIHAN_KELAS_TERTINGGI = "FN0026";
            public const string FN_BIAYA_ADM_KELAS_TERTINGGI = "FN0027";
            public const string FN_KONTROL_BIAYA_ADM_PASIEN_RJ = "FN0028";
            public const string FN_PROPOSE_VALIDASI_TARIF_0 = "FN0029";
            public const string FN_KODE_BIAYA_ADMINISTRASI = "FN0030";
            public const string FN_PEMBUATAN_TAGIHAN_JIKA_TIDAK_ADA_OUTSTANDING_ORDER = "FN0031";
            public const string FN_NAMA_KOORDINATOR_KASIR = "FN0032";
            public const string FN_ADMIN_HANYA_RAWAT_INAP = "FN0033";
            public const string FN_REPORTCODE_CETAK_KWITANSI = "FN0034";
            public const string FN_WARNING_PASIEN_ADA_PIUTANG_PRIBADI = "FN0035";
            public const string FN_KEPALABAGIAN_KEUANGAN = "FN0036";
            public const string FN_ITEM_ID_FOR_MCU_PACKAGE_COST = "FN0038";
            public const string FN_TRANSAKSI_HANYA_DAPAT_DIUBAH_UNIT_ASAL = "FN0039";
            public const string FN_TIPE_CUSTOMER_BPJS = "FN0040";
            public const string FN_AR_LEAD_TIME = "FN0041";
            public const string FN_DEFAULT_PENGAMBILAN_BERKAS_PEMBAYARAN = "FN0042";
            public const string FN_MENGGUNAKAN_PENGATURAN_HARI_PEMBAYARAN_KE_SUPPLIER = "FN0043";
            public const string FN_HARI_PEMBAYARAN_KE_SUPPLIER = "FN0044";
            public const string FN_SELISIH_HARI_JATUH_TEMPO = "FN0045";
            public const string FN_PENJAMIN_BPJS_KESEHATAN = "FN0046";
            public const string FN_FAX_NO_AR = "FN0047";
            public const string FN_EMAIL_AR = "FN0048";
            public const string FN_ALLOW_CLOSE_REGISTRATION_WITHOUT_BILL = "FN0049";
            public const string FN_IS_USE_COUNTER_IN_PAYMENT_RECEIPT = "FN0050";
            public const string FN_LABEL_HONOR_DOKTER = "FN0051";
            public const string FN_DOWN_PAYMENT_IS_ALLOW_ALL = "FN0052";
            public const string FN_AR_IS_ALLOW_PAYMENT_RECEIPT = "FN0053";
            public const string FN_BAGIAN_PENAGIHAN = "FN0054";
            public const string PPH_PERCENTAGE_WITHOUT_NPWP = "FN0055";
            public const string FN_REPORTCODE_CETAK_KWITANSI_BAHASA_ASING = "FN0056";
            public const string FN_PEMBATASAN_CPOE_BPJS = "FN0058";
            public const string FN_IS_SETTLEMENT_ALLOW_WITH_ARPATIENT = "FN0059";
            public const string FN_TARIFF_DIBULATKAN_SAMPAI_SERATUS = "FN0060";
            public const string FN_BATAS_TANGGUNGAN_LEBIH_BESAR_DARI_TAGIHAN = "FN0061";
            public const string FN_HARGA_OBAT_ALKES_LOGISTIK_BERUBAH_KE_HARGA_TERAKHIR = "FN0066";
            public const string FN_IS_ALLOW_ROUNDING_AMOUNT = "FN0067";
            public const string FN_NILAI_PEMBULATAN_TAGIHAN = "FN0068";
            public const string FN_PEMBULATAN_TAGIHAN_KE_ATAS = "FN0069";
            public const string FN_PENJAMIN_BPJS_KETENAGAKERJAAN = "FN0070";
            public const string FN_PENJAMIN_INHEALTH = "FN0073";
            public const string FN_PEMBATASAN_CPOE_INHEALTH = "FN0074";
            public const string FN_DISKON_DOKTER_KOMPONEN_2 = "FN0076";
            public const string FN_IS_ALLOW_BACKDATED_PAYMENT = "FN0077";
            public const string FN_IS_CHECK_TEST_RESULT = "FN0079";
            public const string FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN = "FN0080";
            public const string FN_REPORTCODE_CETAK_KWITANSI_RAWATJALAN_BAHASA_ASING = "FN0081";
            public const string FN_IS_AP_CONSIGNMENT_FROM_ORDER = "FN0082";
            public const string FN_REGISTRATION_BABY_LINK_REGISTRATION_MOTHER = "FN0083";
            public const string FN_CARD_FEE_DITANGGUNG_PASIEN = "FN0084";
            public const string FN_PERSENTASE_PPH21_EKSTRA = "FN0085";
            public const string FN_KA_SIE_AKUN_PEMBELIAN = "FN0086";
            public const string FN_IS_USED_CALCULATE_COVERAGE_PER_BILLING_GROUP = "FN0087";
            public const string FN_BLOK_PEMBUATAN_TAGIHAN_SAAT_ADA_TRANSAKSI_MASIH_OPEN = "FN0088";
            public const string FN_IS_USED_CLAIM_FINAL = "FN0089";
            public const string FN_DISKONCOMP2_TIDAK_LEBIH_DARI_TARIFFCOMP2 = "FN0090";
            public const string FN_FILE_NAME_DOCUMENT_AR_CLAIM_BPJS = "FN0091";
            public const string FN_REPORT_CODE_AR_RECEIPT = "FN0093";
            public const string FN_KASIE_PENGELOLAAN_UTANG = "FN0095";
            public const string FN_IS_USED_REOPEN_BILLING = "FN0096";
            public const string FN_SUPERVISOR_PENAGIHAN_PIUTANG = "FN0100";
            public const string FN_IS_CLAIM_FINAL_AFTER_AR_INVOICE = "FN0102";
            public const string FN_TGL_AR_PATIENT_SESUAI_TGL_PILIH = "FN0103";
            public const string FN_UBAH_NILAI_PEMBUALATAN_PEMBAYARAN = "FN0104";
            public const string FN_REVISI_HUTANG_PENERIMAAN_MENGGUNAKAN_PEMBULATAN_TUKAR_FAKTUR = "FN0105";
            public const string FN_IS_GROUPER_AMOUNT_CLAIM_DEFAULT_ZERO = "FN0107";

            public const string FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP = "FN0109";
            public const string FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATINAP_BAHASA_ASING = "FN0110";
            public const string FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN = "FN0111";
            public const string FN_REPORTCODE_CETAK_LEGALISIR_KWITANSI_RAWATJALAN_BAHASA_ASING = "FN0112";
            public const string FN_AR_DUE_DATE_COUNT_FROM = "FN0113";
            public const string FN_IS_USED_BUTTON_PROCESS_REVENUE_SHARING_ADJUSTMENT = "FN0114";
            public const string FN_CAPTION_DOWN_PAYMENT_IN_MENU_PATIENT_PAYMENT = "FN0115";
            public const string FN_PU_NAME = "FN0119";
            public const string FN_IS_CLAIM_FINAL_BEFORE_AR_INVOICE_AND_SKIP_CLAIM = "FN0120";
            public const string FN_REPORTCODE_CETAK_KWITANSI_MCU = "FN0121";
            public const string FN_REPORTCODE_CETAK_KWITANSI_MCU_BAHASA_ASING = "FN0122";
            public const string FN_DEFAULT_BINDING_TERM_TERMID_ARINVOICE = "FN0123";
            public const string FN_IS_PATIENTBILLDISCOUNT_DISCOUNTCOMP2_VALIDATE_TARIFFCOMP2 = "FN0124";
            public const string FN_FILE_NAME_DOCUMENT_AR_FINAL_CLAIM_BPJS = "FN0125";
            public const string FN_FILE_NAME_DOCUMENT_REVENUE_SHARING_DOWNLOAD_UPLOAD = "FN0126";
            public const string FN_DEFAULT_SELISIH_HARI_UNTUK_FILTER_PERIODE_TRANSAKSI_PROSES_PIUTANG = "FN0127";
            public const string FN_PROSES_CETAK_KWITANSI_LANGSUNG_PREVIEW_DAN_HITUNG_JUMLAH_CETAK = "FN0128";
            public const string FN_FILE_NAME_DOCUMENT_UPLOAD_TARIFF = "FN0129";
            public const string FN_KEPALABAGIAN_PELAYANAN_ADMINISTRASI_PASIEN_JAMINAN = "FN0131";
            public const string FN_WAKIL_DIREKTUR_KEUANGAN = "FN0132";
            public const string FN_BAGIAN_KEUANGAN = "FN0133";
            public const string FN_IS_ARI_DATE_ALLOW_BACKDATE = "FN0134";
            public const string FN_STAMP_AMOUNT_IN_SUPPLIER_PAYMENT_HD = "FN0139";
            public const string FN_CUTOFFDATE_REVENUE_SHARING = "FN0140";
            public const string FN_INVOICE_DT_REMARKS_COPY_RECEIVE_REMARKS = "FN0141";
            public const string FN_MASTER_ITEM_SERVICE_USED_DEFAULT_TARIFF_COMP = "FN0142";
            public const string FN_PEMBUATAN_TAGIHAN_BPJS_MENGGUNAKAN_CARA_BPJS = "FN0143";
            public const string FN_SUPPLIERPAYMENTNO_COUNTER_BY_DATE = "FN0146";
            public const string FN_IS_USING_APPROVAL_VERIFICATION_SUPPLIER = "FN0147";
            public const string FN0148 = "FN0148";
            public const string FN_DEFAULT_CHARGE_CLASS_AIO = "FN0151";
            public const string FN_PERSENTASE_RETUR_RESEP = "FN0149";
            public const string FN_IS_PPN_ALLOW_CHANGED = "FN0150";
            public const string FN_IS_END_AMOUNT_ROUNDING_TO_100 = "FN0155";
            public const string FN_IS_ARINVOICEDATE_ALLOW_BACKDATE = "FN0156";
            public const string FN_IS_PURCHASEINVOICEDATE_ALLOW_BACKDATE = "FN0157";
            public const string FN_SMPT_CLIENT_CONF = "FN0158";
            public const string FN_INSERT_CHARGESHD_ALLOW_DISCHARGE_DATE = "FN0159";
            public const string FN_TRANSRS_ALLOW_FILTER_PAID_TYPE = "FN0160";
            public const string FN_TRANSRS_ADD_FILTER_BPJS_STATUS = "FN0161";
            public const string FN_REVENUE_SHARING_ADJUSTMENT_USING_BRUTO = "FN0162";
            public const string FN_FILE_NAME_DOCUMENT_REVENUE_SHARING_ADJUSTMENT_DOWNLOAD_UPLOAD = "FN0163";
            public const string FN_TEMPLATE_TEXT_EMAIL_SRS = "FN0166";
            public const string FN_DIREKTUR_KEUANGAN = "FN0167";
            public const string FN0168 = "FN0168";
            public const string FN_REPORT_CODE_FOR_EMAIL_SRS = "FN0169";
            public const string FN_LIST_CC_EMAIL_FOR_EMAIL_SRS = "FN0170";
            public const string FN0171 = "FN0171";
            public const string FN_IS_USING_PURCHASE_DISCOUNT_SHARED = "FN0172";
            public const string FN_IS_USING_HET_VALIDATION = "FN0173";
            public const string FN_IS_PAYER_ADMIN_FORMULA_VERSION = "FN0174";
            public const string FN_IS_PAYER_AMOUNT_FORMULA_VERSION = "FN0175";
            public const string FN_IS_END_AMOUNT_ROUNDING_TO_1 = "FN0177";
            public const string FN_IS_ALLOW_BACKDATED_PAYMENT_PERSONAL_AR = "FN0178";
            public const string FN_IS_EKLAIM_LIST_INCLUDE_REGWITHLINKEDTO = "FN0179";
            public const string FN_PEMBUATAN_TAGIHAN_MENGGUNAKAN_FILTER_COVERAGE = "FN0180";
            public const string FN_IS_DISPLAY_PRICE_IN_ORDER_AND_SERVICES_QUICKPICKS_NURSE = "FN0181";
            public const string FN_IS_DISPLAY_PRICE_IN_PELAYANAN_QUICKPICK = "FN0192";
            public const string FN_IS_DISPLAY_PRICE_IN_ALKES_N_BARANGUMUM_QUICKPICK = "FN0193";
            public const string FN0182 = "FN0182";
            public const string FN0183 = "FN0183";
            public const string FN_IS_ALLOW_PROCESS_BILL_WHEN_PENDING_RECALCULATED = "FN0184";
            public const string FN_IS_ALLOW_DISCOUNT_IN_DETAIL_WHEN_ALREADY_HAS_PRESCRIPTION_RETURN = "FN0185";
            public const string FN_IS_CHARGES_QTY_USING_VALIDATE_2DIGIT = "FN0186";
            public const string FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_REVENUESHARING = "FN0187";
            public const string FN_IS_PATIENT_TRANSFER_USED_DIFFERENT_CUSTOMER_BLOCK = "FN0188";
            public const string FN_IS_PATIENT_TRANSFER_USED_HAS_DOWN_PAYMENT_BLOCK = "FN0189";
            public const string FN_IS_VOID_PATIENTPAYMENT_USING_BLOCKER_VALIDATE_COPY_PAYMENT_RECONCILIATION_OR_USER_PATIENT_PAYMENT_BALANCE = "FN0191";
            public const string FN_IS_ARINVOICEDATE_ALLOW_FUTUREDATE = "FN0194";
            public const string FN_IS_PURCHASEINVOICEDATE_ALLOW_FUTUREDATE = "FN0195";
            public const string FN_IS_EKLAIM_NONINPATIENT_DISCHARGEDATE_FROM_SEPDATE = "FN0196";
            public const string FN_IS_EKLAIM_NONINPATIENT_REGISTRATIONDATE_FROM_SEPDATE = "FN0197";
            public const string FN_IS_EKLAIM_DOKTER_USING_DPJP_KONSULEN_VCLAIM = "FN0198";
            public const string FN_KAPAN_PERUBAHAN_NILAI_HARGA__PER_PENERIMAAN_ATAU_PER_BULANAN = "FN0199";
            public const string FN_NOTIFICATION_REGISTRATION_IS_LINKED_TO_INPATIENT_REGISTRATION = "FN0200";
            public const string FN_HEALTHCARE_NAME_RECAPITULATION_PPH21_REPORT = "FN0201";
            public const string FN_NPWP_RECAPITULATION_PPH21_REPORT = "FN0202";
            public const string FN_USER_NAME_RECAPITULATION_PPH21_REPORT = "FN0203";
            public const string FN_IS_EKLAIM_PARAMETER_MANDATORY = "FN0204";
            public const string FN_AUTO_APPROVE_REVENUE_SHARING_ADJ_FROM_UPLOAD = "FN0205";

            public const string IS_OUTPATIENT_ALLOW_BACK_DATE = "OP0001";
            public const string OUTPATIENT_CLASS = "OP0002";
            public const string VALIDATE_PATIENT_REGISTRATION_WITH_PHYSICIAN_SCHEDULE = "OP0003";
            public const string IS_OUTPATIENT_REGISTRATION_AUTOMATICALLY_CHECKED_IN = "OP0004";
            public const string ANAMNESE_SERVICE_UNIT_ID = "OP0005";
            public const string OP_KONTROL_BIAYA_ADMINISTRASI_PASIEN_HANYA_1X_DI_1HARI = "OP0006";
            public const string OP_PENDAFTARAN_DENGAN_RUANG = "OP0007";
            public const string OP_IS_QUEUE_NO_USING_APPOINTMENT = "OP0008";
            public const string OP_LONG_CONSULTATION_MINUTES = "OP0009";
            public const string OP_VALIDATE_DX_ON_TRANSACTION = "OP0010";
            public const string IS_OUTPATIENT_ALLOW_NEXT_DATE = "OP0011";
            public const string IS_OUTPATIENT_RECEIPT_USING_DOT_MATRIX = "OP0013";
            public const string OP_IS_USED_PDCLIENT = "OP0014";
            public const string OP_IS_TEST_ORDER_ALLOW_RESCHEDULE = "OP0015";
            public const string OP0016 = "OP0016";
            public const string OP0017 = "OP0017";
            public const string OP0018 = "OP0018";
            public const string OP0019 = "OP0019";
            public const string OP_IS_APPOINTMENT_ALLOW_BACK_DATE = "OP0020";
            public const string OP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "OP0021";
            public const string OP_IS_DEFAULT_SPECIALTY_CATARACT = "OP0022";
            public const string OP_IS_BLOCK_PATIENT_ALREADY_HAS_REGISTRATION_ON_DATE_CLINIC_AND_PARAMEDIC = "OP0023";
            public const string OP_IS_USING_CALL_PATIENT_FEATURE = "OP0024";
            public const string OP_IS_USING_CLINIC_SERVICE_FEATURE = "OP0025";
            public const string OP_CHECK_APPOINTMENT_BEFORE_CHANGE_PHYSICIAN_SCHEDULE = "OP0026";
            public const string OP0027 = "OP0027";
            public const string OP0028 = "OP0028";
            public const string OP0029 = "OP0029";
            public const string OP0030 = "OP0030";
            public const string OP0035 = "OP0035";
            public const string OP0038 = "OP0038";
            public const string OP0040 = "OP0040";
            public const string OP0042 = "OP0042";

            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string OP0043 = "OP0043";

            public const string OP_IS_CHANGE_QUEUE_AFTER_CHANGE_PHYSICIAN = "OP0031";
            public const string OP_ALLOW_RESCHEDULE_BACK_DATE = "OP0032";

            public const string VITAL_SIGN_HEIGHT = "EM0001";
            public const string VITAL_SIGN_WEIGHT = "EM0002";
            public const string VITAL_SIGN_HEAD_CIRCUMFERENCE = "EM0003";
            public const string VITAL_SIGN_BMI = "EM0004";
            public const string EMR_PATIENT_PAGE_BY_DEPARTMENT = "EM0008";

            public const string VITAL_SIGN_TEMPERATURE = "EM0005";
            public const string VITAL_SIGN_MAP = "EM0009";
            public const string VITAL_SIGN_EWS = "EM0010";
            public const string EM0010 = "EM0010"; //?
            public const string VITAL_SIGN_NBPs = "EM0011";
            public const string VITAL_SIGN_NBPd = "EM0012";
            public const string VITAL_SIGN_RR = "EM0013";
            public const string VITAL_SIGN_HR = "EM0014";
            public const string VITAL_SIGN_SPO2 = "EM0015";
            public const string VITAL_SIGN_AVPU = "EM0016";
            public const string EM0017 = "EM0017";
            public const string EM0018 = "EM0018"; //GCS-EMV
            public const string EM0019 = "EM0019"; //GCS-E1
            public const string EM0020 = "EM0020"; //GCS-V1
            public const string EM0021 = "EM0021"; //GCS-M1
            public const string IS_DIAGNOSTIC_ADD_RMO = "EM0022";
            public const string VITAL_SIGN_PEWS_BEHAVIOR = "EM0027";
            public const string VITAL_SIGN_PEWS_CARDIOVASKULAR = "EM0028";
            public const string VITAL_SIGN_PEWS_RESPIRATION = "EM0029";
            public const string VITAL_SIGN_PEWS = "EM0030";
            public const string DEFAULT_DAYS_PRMRJ = "EM0031";

            public const string EM_PEMBATASAN_CPOE_BPJS = "EM0023";
            public const string EM_PEMBATASAN_CPOE_INHEALTH = "EM0024";
            public const string EM_SALIN_CATATAN_KONSULTASI = "EM0026";
            public const string EM_PRINT_TRACER_FARMASI_KETIKA_SEND_ORDER = "EM0032";
            public const string EM_ORDER_RESEP_BISA_PILIH_DISPENSARY_FARMASI = "EM0033";
            public const string EM0034 = "EM0034";
            public const string EM_ORDER_RESEP_HANYA_BISA_PILIH_ITEM_STOK_RS = "EM0035";
            public const string EM0036 = "EM0036";
            public const string RESUME_MEDIS_TELEPHONE = "EM0037";
            public const string IGD_TELEPHONE_ON_RESUME_MEDIS = "EM0038";
            public const string EMR_ALLOW_CHANGE_CHARGES_QTY = "EM0039";
            public const string EMR_COMPOUND_ONLINE_PRES_EMBALACEQTY_AUTOMATIC_FILLED = "EM0040";

            public const string IS_SHOW_DASHBOARD_EMR = "EM0042";
            public const string CC_COPY_FROM_NCC = "EM0043";
            public const string EM_Display_Billing_Notification_NurseJournal = "EM0044";
            public const string EM_PHYSICIAN_PATIENT_CALL = "EM0045";
            public const string EM_IS_VALIDATION_INPUT_SURGERY_ASSESSMENT_FIRST = "EM0046";
            public const string EM_IS_DEFAULT_CHECKLIST_COPY_SOAP_DIAGNOSIS = "EM0047";
            public const string EM_IS_VALIDATION_EMPTY_STOCK_PRESCRIPTION_ORDER = "EM0048";
            public const string EM0049 = "EM0049";
            public const string EM0050 = "EM0050";
            public const string EM0051 = "EM0051";
            public const string EM_IS_USING_WITHOUT_INITIAL_ASSESSMENT_FOR_PHYSICIAN_DISCHARGE = "EM0054";
            public const string EM_VITALSIGN_AND_REVIEWOFSYSTEM_FROM_LINKED_REGISTRATION_IN_MEDICAL_RESUME = "EM0055";
            public const string EM0057 = "EM0057";
            public const string EM0058 = "EM0058";
            public const string EM_DURATION_FILTER_DATE_PREVIOUS_PATIENT = "EM0059";
            public const string EM_OUTPATIENT_USING_COMPLETE_SESSION = "EM0060";
            public const string EM0063 = "EM0063";
            public const string EM_CASEMIX_ALLOW_CHANGE_MEDICAL_RESUME = "EM0064";
            public const string EM_DEFAULT_DAY_FILTER_CPPT = "EM0066";
            public const string EM0067 = "EM0067";
            public const string EM0068 = "EM0068";
            public const string EM_IS_ASSESMENT_NON_INPATIENT_DEFAULT_USING_REGISTRATION_DATE = "EM0069";
            public const string EM_DEFAULT_DISPLAY_FILTER_ALL = "EM0070";
            public const string EM0071 = "EM0071";
            public const string EM0072 = "EM0072";
            public const string EM_IS_DOCTOR_FEE_ALLOW_PREVIEW_TARIFF = "EM0073";
            public const string EM_IS_QUICK_PICKS_HISTORY_READ_ORDER_DATE = "EM0074";
            public const string EM0075 = "EM0075";
            public const string EM_IS_ONLINE_PRESCRIPTION_ALLOW_PREVIEW_STOCK = "EM0078";
            public const string EM0079 = "EM0079";
            public const string EM_IS_MEDICAL_RESUME_CAN_INSERT_RESIDUAL_PRESCRIPTION = "EM0080";
            public const string EM_IS_MEDICAL_RESUME_CAN_INSERT_LABORATORIUM_RESULT = "EM0081";
            public const string EM0082 = "EM0082";
            public const string EM0083 = "EM0083";
            public const string EM0084 = "EM0084";
            public const string EM_IS_DOCTOR_ALLOW_CHANGE_ANOTHER_DOCTOR_PRESCRIPTION = "EM0085";
            public const string EM0086 = "EM0086";
            public const string EM0087 = "EM0087";
            /// <summary>
            /// Notifikasi Pengisian Form Program Pengendalian Resistensi Antimikroba (PPRA) ketika Pengiriman Resep Online Dokter ke Farmasi 
            /// </summary>
            public const string EM0088 = "EM0088";
            public const string EM0099 = "EM0099"; 
            /// <summary>
            /// Apakah pada menu Assessment EMR (Dokter + Perawat) diperbolehkan ubah tanggal dan jam kajian atau tidak ? 
            /// </summary>
            public const string EM0089 = "EM0089";
            /// <summary>
            /// Jenis Kunjungan Program Bayi Tabung 
            /// </summary>
            public const string EM0090 = "EM0090";
            /// <summary>
            /// Order Pemeriksaan Laboratorium Dokter untuk Pasien BPJS bersifat terbatas 
            /// </summary>
            public const string EM0091 = "EM0091";
            /// <summary>
            /// Order Pemeriksaan Radiologi Dokter untuk Pasien BPJS bersifat terbatas 
            /// </summary>
            public const string EM0092 = "EM0092";
            /// <summary>
            /// Menu Online Prescription (EMR Dokter), pada saat Quick Picks menggunakan fitur menampilkan Kalkulasi Harga Obat saat itu atau tidak? 
            /// </summary>
            public const string EM0093 = "EM0093";
            /// <summary>
            /// Order Pemeriksaan Penunjang Medis Dokter untuk Pasien BPJS bersifat terbatas 
            /// </summary>
            public const string EM0094 = "EM0094";
            /// <summary>
            /// Kode Tanda Vital untuk MST (Malnutrition Screening Tool)
            /// </summary>
            public const string EM0113 = "EM0113";

            public const string IS_EMERGENCY_ALLOW_BACK_DATE = "ER0001";
            public const string ER_DEFAULT_PHYSICIAN = "ER0002";
            public const string ER_NURSE_NOT_ALLOWED_ENTRY_PATIENT_STATUS = "ER0003";
            public const string IS_PENDAFTARAN_DENGAN_RUANG = "ER0004";
            public const string IS_IGNORE_EMERGENCY_PARAMEDIC = "ER0005";
            public const string ER_CHARGE_CLASS = "ER0006";
            public const string EMERGENCY_CLASS = "ER0006";
            public const string ER_DOKTER_UGD_SELALU_DEFAULT_SETVAR_DOKTER_UGD = "ER0007";
            public const string ER_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "ER0008";
            public const string ER_REGISTRATION_ORDER_BY = "ER0009";
            public const string ER0010 = "ER0010";
            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string ER0011 = "ER0011";

            public const string IS_CONFIRM_PURCHASE_RECEIVE = "IM0002";
            public const string DEFAULT_CYCLE_COUNT_TYPE = "IM0003";
            public const string RANGE_EXPIRED_DATE = "IM0004";
            public const string IS_DISCOUNT_APPLIED_TO_AVERAGE_PRICE = "IM0005";
            public const string IS_DISCOUNT_APPLIED_TO_UNIT_PRICE = "IM0006";
            public const string IM_KONFIRMASI_PENERIMAAN_DISTRIBUSI_BARANG = "IM0007";
            public const string IM_LOKASI_TRANSIT_DISTRIBUSI_BARANG = "IM0008";
            public const string KEPALA_LOGISTIK_UMUM = "IM0009";
            public const string IM_PERMINTAAN_BARANG_DENGAN_SATUAN_KECIL = "IM0010";
            public const string IM_KONTROL_CETAK_BUKTI_PERMINTAAN_PEMBELIAN = "IM0011";
            public const string IM_SALIN_PEMESANAN_BARANG_TANPA_FILTER = "IM0012";
            public const string IM_REVISI_PEMESANAN_KETIKA_KONFIRMASI_PENERIMAAN = "IM0013";
            public const string IM_DEFAULT_LOCATION_PURCHASEORDER_PHARMACY = "IM0014";
            public const string IM_DEFAULT_LOCATION_PURCHASEORDER_LOGISTIC = "IM0015";
            public const string IM_DEFAULT_ROLE_OFFICER_PHARMACY = "IM0016";
            public const string IM_DEFAULT_ROLE_OFFICER_LOGISTIC = "IM0017";
            public const string IM_APPROVER_PURCHASE_ORDER = "IM0018";
            public const string IM_MAX_TRANSACTION_AMOUNT_PURCHASE_ORDER = "IM0019";
            public const string IM_APPROVER_PURCHASE_RECEIVE_PHARMACY = "IM0020";
            public const string IM_APPROVER_PURCHASE_RECEIVE_LOGISTIC = "IM0021";
            public const string IS_PROCESS_INVOICE_CAN_CHANGE_AVERAGE_PRICE = "IM0022";
            public const string KEPALA_LOGISTIK_NO_LISENSI = "IM0023";
            public const string ALLOW_POR_QTY_BIGGER_THEN_PO = "IM0024";
            public const string IM_USERID_APPROVE_PO = "IM0025";
            public const string IM_USERID_PROPOSE_PO = "IM0026";
            public const string IM_USERID_CREATE_PO_PHARMACY = "IM0027";
            public const string IM_USERID_CREATE_PO_LOGISTIC = "IM0028";
            public const string IM_NAME_CREATE_PO_PHARMACY = "IM0029";
            public const string IM_NAME_CREATE_PO_LOGISTIC = "IM0030";
            public const string IM_APPROVE_CN_WHEN_APPROVE_POR = "IM0031";
            public const string KEPALA_LOGISTIK_OBAT = "IM0032";
            public const string IM_FAKTOR_X_ROP_MIN = "IM0033";
            public const string IM_FAKTOR_X_ROP_MAX = "IM0034";
            public const string DEFAULT_LEAD_TIME = "IM0035";
            public const string DEFAULT_BACKWARD_DAYS = "IM0036";
            public const string DEFAULT_FORWARD_DAYS = "IM0037";
            public const string IM_PERCENTAGE_MAXIMUM_STOCK_FORMULA = "IM0038";
            public const string ALLOW_PURCHASE_RECEIVE_AFTER_PRINT_PURCHASE_ORDER = "IM0039";
            public const string IM_PROSES_DUE_DATE_FROM_PORDATE_OR_REFERENCE_DATE = "IM0040";
            public const string IM_PURCHASE_RECEIVE_USE_BASE_UNIT = "IM0041";
            public const string IM_REORDER_PR_BY_QTY_END = "IM0042";
            public const string IM_AUTO_APPROVE_PR_FROM_REORDER = "IM0043";
            public const string IM_FORMAT_CETAKAN_LABEL_PRODUKSI = "IM0044";
            public const string KEPALA_UNIT_LOGISTIK_UMUM = "IM0046";
            public const string IM_POR_AUTO_UPDATE_SUPPLIER_ITEM = "IM0049";
            public const string IM_ITEM_REQUEST_GET_DESTINATION_LOCATION = "IM0050";
            public const string IM_OUTSTANDING_PO_PR_VISIBLE_IN_ROP_PR = "IM0051";
            public const string IM_USE_MAX_EXPIRED_DATE = "IM0052";
            public const string IM_MAX_EXPIRED_DATE = "IM0053";
            public const string IM_QTY_ROP_BOLEH_LEBIH_BESAR_DARI_QTY_MAX = "IM0054";
            public const string IM_ITEM_REQUEST_ALLOW_OUTSTANDING = "IM0055";
            public const string IM_DEFAULT_LOCATION_CSSD = "IM0056";
            public const string IM_IS_PURCHASE_REQUEST = "IM0057";
            public const string IM_TRANSAKSI_OBAT_HANYA_TIPE_DISTRIBUSI = "IM0058";
            public const string IM_IS_PURCHASE_REQUEST_SERVICE_UNIT_RI = "IM0059";
            public const string IM_IS_PURCHASE_REQUEST_SERVICE_UNIT_RJ = "IM0060";
            public const string IM_IS_PURCHASE_REQUEST_SERVICE_UNIT = "IM0061";
            public const string PJ_PEMBELIAN_ALKES = "IM0062";
            public const string PJ_PEMBELIAN_BARANG = "IM0063";
            public const string PJ_KA_PEMASARAN = "IM0118";
            public const string PJ_KA_BIRO_PENGEMBANGAN = "IM0119";
            public const string IM_PURCHASE_REQUEST_LIST_ORDER_BY = "IM0120";
            public const string PENJUAL = "IM0064";
            public const string IM_DEFAULT_KODE_LOKASI_MEDIK = "IM0065";
            public const string APOTEKER_PENANGGUNG_JAWAB = "IM0066";
            public const string KEPALA_LOGISTIK_FARMASI = "IM0067";
            public const string KEPALA_LOGISTIK_MEDIK = "IM0068";
            public const string IM_APPROVER_PURCHASE_RECEIVE_MEDIK = "IM0069";
            public const string IM_SEARCH_DIALOG_TYPE = "IM0070";
            public const string KEPALA_UNIT_LOGISTIK_BAHAN_MAKANAN = "IM0071";
            public const string IM_IS_POR_DATE_ALLOW_BACKDATE = "IM0072";
            public const string IM_IS_NO_FACTUR_DONT_ALLOW_DUPLICATE = "IM0073";
            public const string IM_IS_PURCHASE_REQUEST_ALLOW_CHANGE_OVER_RECOMMENDATION = "IM0074";
            public const string IM_IS_PO_QTY_CANNOT_OVER_PR_QTY = "IM0075";
            public const string IM_IS_QTY_DISTRIBUTION_CANNOT_OVER_REQUEST = "IM0076";
            public const string IM_IS_QTY_STOCK_OPNAME_OTOMATIS_TERISI = "IM0077";
            public const string KEPALA_SUB_BAGIAN_GUDANG_FARMASI = "IM0078";
            public const string KEPALA_SUB_BAGIAN_GUDANG_ALKES = "IM0079";
            public const string KEPALA_SUB_BAGIAN_GUDANG_BAHAN_MAKANAN = "IM0080";
            public const string KEPALA_INSTALASI_GIZI = "IM0081";
            public const string IM_ALLOW_PRINT_ORDER_RECEIPT_AFTER_PROPOSDED = "IM0082";
            public const string IS_PPN_APPLIED_TO_AVERAGE_PRICE = "IM0083";
            public const string IS_PPN_APPLIED_TO_UNIT_PRICE = "IM0084";
            public const string IM_POR_WITH_PRICE_INFORMATION = "IM0085";
            public const string IM_PURCHASE_RETURN_TAX_INFORMATION_MANDATORY = "IM0089";
            public const string IM_STOCK_OPNAME_INPUT_ALL = "IM0086";
            public const string IM_STOCK_OPNAME_INPUT_QTY_FISIK = "IM0087";
            public const string IM_STOCK_OPNAME_INPUT_QTY_SELISIH = "IM0088";
            public const string IM_UBAH_PPJDETAIL_POR = "IM0092";
            public const string IM_IS_VISIBLE_ATTRIBUTE = "IM0093";
            public const string IM_DEFAULT_LOCATION_PURCHASEORDER_NUTRITION = "IM0094";
            public const string IM_APPROVER_PURCHASE_RECEIVE_NUTRITION = "IM0095";
            public const string IM_CHANGE_QTY_POR = "IM0096";
            public const string REOPEN_CHARGESHD_REOPEN_ISAPPROVE_CHARGESDT = "IM0098";
            public const string IM_SHOW_TOTAL_PRICE_IN_PURCHASE_REQUEST = "IM0099";
            public const string IM_PENGADAAN_TAMPIL_DISCOUNT_FINAL = "IM0100";
            public const string IM_PENGADAAN_TAMPIL_ONGKOS_KIRIM = "IM0101";
            public const string IM_NAMA_JABATAN_DIREKTUR = "IM0102";
            public const string IM_MANAGER_LOGISTIK = "IM0103";
            public const string IM_MANAGER_FARMASI = "IM0104";
            public const string IM_NOMOR_IZIN_RUMAH_SAKIT_PO = "IM0105";
            public const string IM_DISTRIBUTION_ALLOWED_WITHOUT_REQUEST = "IM0106";
            public const string IM_PHONE_EMAIL_PURCHASEORDER_PHARMACY = "IM0107";
            public const string IM_PHONE_EMAIL_PURCHASEORDER_LOGISTIC = "IM0108";
            public const string IM_PHONE_EMAIL_PURCHASEORDER_NUTRITION = "IM0109";
            public const string IM_DEFAULT_LOCATION_PURCHASEORDER_SIMIT = "IM0110";
            public const string IM_APPROVER_PURCHASE_RECEIVE_SIMIT = "IM0111";
            public const string IM_BAGIAN_PURCHASING = "IM0112";
            public const string IM_DEFAULT_USER_PURCHASE_PHARMACY = "IM0113";
            public const string IM_IS_RETURN_MOVEMENT_RECALCULATE_HNA = "IM0116";
            public const string IM_SPV_RT_LOGISTIK = "IM0117";
            public const string IM0121 = "IM0121";
            public const string IM0122 = "IM0122";
            public const string IM0123 = "IM0123";
            public const string IM0125 = "IM0125";
            public const string IM0126 = "IM0126";
            public const string IM0127 = "IM0127";
            public const string IM0128 = "IM0128";
            public const string IM0131 = "IM0131";
            public const string IM0133 = "IM0133";
            public const string IM0134 = "IM0134";
            public const string IM0135 = "IM0135";
            public const string IM_IS_COPY_PO_ALLOW_CHANGE_PRICE_AND_DISCOUNT = "IM0136";
 
            public const string IS_INPATIENT_ALLOW_BACK_DATE = "IP0001";
            public const string IP_CODE_TYPE_DOCTOR_DPJP = "IP0002";
            public const string IP_CODE_TYPE_DOCTOR_RMO = "IP0003";
            public const string IP_BED_STATUS_DEFAULT_WHEN_PATIENT_DISCHARGE = "IP0004";
            public const string IP_CODE_TYPE_IS_MOVE_CLASS_TRANSFER = "IP0005";
            public const string IP_ICU_CLASS_CODE = "IP0006";
            public const string IP_ICU_SERVICE_UNIT_ID = "IP0007";
            public const string IP_PICU_SERVICE_UNIT_ID = "IP0008";
            public const string IP_NICU_SERVICE_UNIT_ID = "IP0009";
            public const string IP0012 = "IP0012"; //Pendaftaran Pasien Rawat Inap Tanpa Konfirmasi Penerimaan
            public const string IP0013 = "IP0013"; //Default Farmasi JKN/BPJS Pasien Rawat Inap
            public const string IP_KEPALA_BAGIAN_REGISTRASI_RAWAT_INAP = "IP0014";
            public const string IP_TANGGAL_PULANG_DARI_RENCANA_PULANG = "IP0015";
            public const string IP_REGISTRATION_BLOCK_OPEN_REGISTRATION = "IP0016";
            public const string IP_REGISTRATION_BLOCK_FROM_REGISTRATION_24JAM = "IP0017";
            public const string IP_ITEM_GROUP_ID_HONOR_VISITE = "IP0019";
            public const string IP_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "IP0020";
            public const string IP_IS_TRANSFER_MEAL_ORDER = "IP0022";
            public const string IP_IS_BLOCK_TRANSFER_BILL = "IP0023";

            public const string IP0024 = "IP0024"; //Format Layout Form Elektronik : Pemeriksaan Fisik
            public const string IP0025 = "IP0025"; //Format Layout Form Elektronik : Psikososial dan Spiritual
            public const string IP0026 = "IP0026"; //Format Layout Form Elektronik : Kebutuhan Edukasi
            public const string IP0027 = "IP0027"; //Format Layout Form Elektronik : Perencanaan Pasien Pulang
            public const string IP_BED_INFORMATION_TV_DISPLAY_ALL_BED = "IP0028";
            public const string IP_IS_PATIENT_NEWBORN_MANDATORY_MOTHER_REGISTRATIONNO = "IP0029";
            public const string IS_INPATIENT_RECEIPT_USING_DOT_MATRIX = "IP0032";
            public const string IP_BED_CHARGES_TIME_ROUNDING = "IP0033";
            public const string IP_BED_CHARGES_TYPE_DATE = "IP0034";
            public const string IP_BED_CHARGES_IN_DAY = "IP0035";
            public const string IP0036 = "IP0036";
            public const string IP_BED_CHARGES_HEALTHCARESERVICEUNIT = "IP0037";

            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string IP0038 = "IP0038";
            /// <summary>
            /// Apakah Kelas Tagihan pada Pendaftaran akan otomatis terisi setelah Pilih Tempat Tidur ?
            /// </summary>
            public const string IP0040 = "IP0040";

            public const string PERSON_NAME_FORMAT = "SA0001";
            public const string DEFAULT_PASSWORD = "SA0002";
            public const string MAX_BACK_DATE = "SA0003";
            public const string DEFAULT_SERVICE_UNIT_INTERVAL = "SA0004";
            public const string PATIENT_GRID_REFRESH_INTERVAL = "SA0005";
            public const string PHONE_AREA = "SA0007";

            public const string LABEL_PRINTER_NAME = "SA0008";
            public const string WRISTBAND_PRINTER_NAME = "SA0009";
            public const string MEDICAL_LABEL_PRINT_NO = "SA0010";
            public const string NT_REORDER_MEAL_BY_MEAL_TIME_PERIOD = "NT0001";
            public const string NT_FORMAT_CETAKAN_ETIKET_GIZI = "NT0002";
            public const string NT_JENIS_PRINTER_ETIKET_GIZI = "NT0003";
            public const string NT_PANEL_MAKAN_BY_DAY = "NT0004";
            public const string NT_ORDER_MEAL_NOT_ONLY_INPATIENT = "NT0005";
            public const string NT_DEFAULT_ORDER_DATE = "NT0006";

            public const string IS_BPJS_BRIDGING = "SA0011";
            public const string BPJS_CODE = "SA0012";
            public const string BPJS_CONSUMER_ID = "SA0013";
            public const string BPJS_CONSUMER_PASSWORD = "SA0014";
            public const string BPJS_SEP_WS_URL = "SA0015";

            public const string IS_USING_MULTI_WRISTBAND_PRINTER = "SA0013";
            public const string MALE_WRISTBAND_PRINTER_NAME = "SA0014";

            // harus cek ini mana yg bener
            public const string DIRECT_PRINT_STRING_PER_LINE = "SA0015";
            public const string FEMALE_WRISTBAND_PRINTER_NAME = "SA0015";

            public const string INFANT_WRISTBAND_PRINTER_NAME = "SA0016";
            public const string INTERNAL_MEDICATION_PRINTER_NAME = "SA0017";
            public const string EXTERNAL_MEDICATION_PRINTER_NAME = "SA0018";

            public const string DIREKTUR_YANMED = "SA0020";
            public const string MANAJER_YANMED = "SA0021";
            public const string OTOMATIS_CETAK_BUKTI_ORDER_PENUNJANG = "SA0022";
            public const string OTOMATIS_CETAK_BUKTI_ORDER_RESEP = "SA0023";
            public const string SA_KONTROL_PEMBATALAN_ORDER = "SA0024";
            public const string SA_CHECK_DATA_PASIEN = "SA0025";
            public const string SA_DURASI_TIMEOUT_WEBAPI = "SA0026";
            public const string SA_AUTHORIZE_TO_JUMP = "SA0027";
            public const string SA_BRIDGING_SISTEM_ANTRIAN = "SA0028";
            public const string SA_PROTOKOL_BRIDGING_SISTEM_ANTRIAN = "SA0029";
            public const string SA_CONSID_BRIDGING_SISTEM_ANTRIAN = "SA0030";
            public const string SA_CONSPWD_BRIDGING_SISTEM_ANTRIAN = "SA0031";
            public const string SA_ALAMAT_WEBAPI_SISTEM_ANTRIAN = "SA0032";
            public const string SA_LOKASI_PRINTER_IP_ADDR = "SA0033";
            public const string DIREKTUR_KEUANGAN = "SA0034";
            public const string SA_SISTEM_NOTIFIKASI_ORDER = "SA0035";
            public const string SA_KABAG_BILLING = "SA0036";
            public const string SA_TIPE_SEARCH_DIALOG_PASIEN = "SA0037";
            public const string FILTER_PREVIOUS_TRANSACTION_DATE_INTERVAL = "SA0038";
            public const string CAPITALIZE_PATIENT_NAME = "SA0039";
            public const string MAX_NEXT_DATE = "SA0040";
            public const string MANAGER_KEUANGAN = "SA0041";
            public const string IS_APLICARES_BRIDGING = "SA0042";
            public const string APLICARES_CONSUMER_ID = "SA0043";
            public const string APLICARES_CONSUMER_PASSWORD = "SA0044";
            public const string APLICARES_SEP_WS_URL = "SA0045";
            public const string WARNING_PATIENT_OUTSTANDING_REG_DIFF_DAY = "SA0046";
            public const string SA_REGISTRATION_ALLOW_GUEST = "SA0047";
            public const string NAMA_KODER_BPJS = "SA0048";
            public const string MENU_REALISASI_TAMPIL_HARGA_TIDAK = "SA0049";
            public const string SA_IS_USED_PRODUCT_LINE = "SA0050";
            public const string PRESIDEN_DIREKTUR = "SA0051";
            public const string IS_FIRST_NAME_MANDATORY = "SA0052";
            public const string IS_MIDDLE_NAME_ALLOW_NULL = "SA0053";
            public const string IS_ZIP_CODE_MANDATORY = "SA0054";
            public const string IS_PATIENT_DISCHARGE_DEAD = "SA0055";
            public const string CONSUMER_CONS_ID = "SA0058";
            public const string CONSUMER_PASS_ID = "SA0059";
            public const string URL_WEB_API = "SA0060";
            public const string IS_BRIDGING_TO_GATEWAY = "SA0061";
            public const string SA_TIPE_SEARCH_DIALOG_GUEST = "SA0062";
            public const string IS_USING_AUTHENTICATION = "SA0064";
            public const string INHEALTH_WEB_SERVICE_URL = "SA0065";
            public const string INHEALTH_ACCESS_TOKEN = "SA0066";
            public const string IS_OCCUPATION_PATIENT_MANDATORY = "SA0067";
            public const string IS_EDUCATION_PATIENT_MANDATORY = "SA0068";
            public const string CHANGE_DOCTOR_AUTOMATIC_CHANGE_UNIT = "SA0069";
            public const string CITO_PEMERIKSAAN_TAMPIL_PALING_ATAS = "SA0070";
            public const string IS_BRIDGING_TO_EKLAIM = "SA0071";
            public const string EKLAIM_WEB_SERVICE_URL = "SA0072";
            public const string EKLAIM_HOSPITAL_CODE = "SA0073";
            public const string EKLAIM_ENCRYPTION_KEY = "SA0074";
            public const string FN_NAMA_PETUGAS_PENAGIHAN_NONOPERASIONAL = "FN0207";
            public const string FN_ALAMAT_EMAIL_PENAGIHAN_NONOPERASIONAL = "FN0208";

            public const string QUMATIC_API_KEY = "SA0076";
            public const string INHEALTH_PROVIDER_CODE = "SA0077";
            public const string IS_BRIDGING_TO_INHEALTH = "SA0078";
            public const string IS_BRIDGING_TO_QUMATIC = "SA0079";
            public const string WEB_URL_QUMATIC = "SA0080";

            public const string PROVIDER_GATEWAY_SERVICE = "SA0081";
            public const string CAPITALIZE_PATIENT_ADDRESS = "SA0082";
            public const string PATIENT_LASTNAME_FROM_BPJS = "SA0083";

            public const string IS_USED_PATIENT_OWNER_STATUS = "SA0084";

            public const string WEB_URL_MEDINFRAS_MOBILE_APPS = "SA0085";
            public const string IS_BRIDGING_TO_MEDINFRAS_MOBILE_APPS = "SA0086";
            public const string HEALTHCARE_GROUP_MEDINFRAS_MOBILE_APPS = "SA0087";
            public const string SA_USE_VERIFY_ALL_BUTTON = "SA0088";
            public const string SA_IS_USED_REVENUE_COST_CENTER = "SA0089";
            public const string SA_VERIFICATION_TREASURY = "SA0093";
            public const string SA_APPROVAL_TREASURY = "SA0094";

            public const string SA_PARAMEDIC_SCHEDULE_VALIDATION_BEFORE_REGISTRATION = "SA0100";
            public const string SA_MAX_APPOINTMENT_VALIDATION = "SA0101";
            public const string SA_REGISTRATION_BUTTON_TRANSACTION_DIRECT_MENU = "SA0104";
            public const string DEFAULT_PENGATURAN_NOTIFIKASI_EMAIL = "SA0105";
            public const string API_KEY_SENDGRID = "SA0106";
            public const string FORMAT_TEMPLATE_ID_SENDGRID = "SA0107";
            public const string DIREKTUR_UMUM = "SA0108";
            public const string ASISTEN_MANAGER_KEUANGAN = "SA0110";
            public const string SA0111 = "SA0111";
            public const string SA0112 = "SA0112";
            public const string SA0113 = "SA0113";
            public const string IS_USED_PATIENT_OWNER_IN_INPATIENT_REGISTRATION = "SA0114";
            public const string SA0116 = "SA0116";
            public const string SA0117 = "SA0117";
            public const string SA_IS_VIEW_PHYSICIAN_LICENSE_NO_AT_PATIENT_BANNER = "SA0118";
            public const string SA0119 = "SA0119";
            public const string SA0120 = "SA0120";
            public const string SA0121 = "SA0121";
            public const string DEFAULT_PATIENT_OWNER_STATUS_IN_REGISTRATION = "SA0122";
            public const string SA0123 = "SA0123";
            public const string SA0124 = "SA0124";
            public const string SA0125 = "SA0125";
            public const string SA0126 = "SA0126";
            public const string SA0127 = "SA0127";
            public const string SA0128 = "SA0128";
            public const string SA0129 = "SA0129";
            public const string SA0130 = "SA0130";
            public const string SA0131 = "SA0131";
            public const string SA0132 = "SA0132";
            public const string SA0133 = "SA0133";
            public const string SA0134 = "SA0134";
            public const string SA0135 = "SA0135";
            public const string SA0136 = "SA0136";
            public const string SA0137 = "SA0137";
            public const string SA0138 = "SA0138";
            public const string SA0139 = "SA0139";
            public const string SA0140 = "SA0140";
            public const string SA0141 = "SA0141";
            public const string IS_USED_PATIENT_OWNER_IN_OUTPATIENT_REGISTRATION = "SA0142";
            public const string IS_USED_PATIENT_OWNER_IN_EMERGENCY_REGISTRATION = "SA0143";
            public const string IS_USED_PATIENT_OWNER_IN_MCU_REGISTRATION = "SA0144";
            public const string IS_USED_PATIENT_OWNER_IN_LABORATORY_REGISTRATION = "SA0145";
            public const string IS_USED_PATIENT_OWNER_IN_IMAGING_REGISTRATION = "SA0146";
            public const string IS_USED_PATIENT_OWNER_IN_DIAGNOSTIC_REGISTRATION = "SA0147";
            public const string IS_USED_PATIENT_OWNER_IN_PHARMACY_REGISTRATION = "SA0148";

            public const string SA0152 = "SA0152";
            public const string SA0169 = "SA0169";

            public const string SA0171 = "SA0171";
            public const string STAFF_PEMBELIAN = "SA0173";

            public const string SA_EDC_BRIDGING = "SA0174";

            public const string SA0175 = "SA0175";
            public const string SA0176 = "SA0176";
            public const string SA0177 = "SA0177";

            public const string IS_USED_INPUT_AIO_PACKAGE_IN_OUTPATIENT_REGISTRATION = "SA0153";
            public const string IS_USED_INPUT_AIO_PACKAGE_IN_LABORATORY_REGISTRATION = "SA0154";
            public const string IS_USED_INPUT_AIO_PACKAGE_IN_IMAGING_REGISTRATION = "SA0155";
            public const string IS_USED_INPUT_AIO_PACKAGE_IN_DIAGNOSTIC_REGISTRATION = "SA0156";
            public const string IS_USED_INPUT_AIO_PACKAGE_IN_INPATIENT_REGISTRATION = "SA0158";

            public const string SA_IS_BRIDGING_TO_PAYMENT_GATEWAY = "SA0162";

            public const string SA_AUTOBILL_ITEM_PRIORITY_FROM_PARAMEDIC = "SA0163";

            public const string SA_AUTOBILL_CHARGES_PARAMEDIC_FROM_DEFAULTPARAMEDIC = "SA0164";

            public const string SA0167 = "SA0167";
            public const string SA0168 = "SA0168";

            public const string SA_FORMAT_QRCODE = "SA0190";

            public const string SA_BRIDGING_WITH_MEDINLINK = "SA0193";

            public const string SA0193 = "SA0193";
            public const string SA0194 = "SA0194";
            public const string SA0195 = "SA0195";
            public const string SA0196 = "SA0196";
            public const string SA0197 = "SA0197";
            /// <summary>
            /// Menggunakan Paket Kunjungan atau Tidak? (1=YA|0=TIDAK)
            /// </summary>
            public const string SA0198 = "SA0198";
            /// <summary>
            /// Default kadaluarsa untuk item paket kunjungan
            /// </summary>
            public const string SA0199 = "SA0199";

            public const string SA0200 = "SA0200";
            public const string SA0201 = "SA0201";
           
            public const string SA_REPORT_FOOTER_PRINTEDBY_USERNAME_FULLNAME = "SA0204"; // 1 = Username || 2 = FullName
            /// <summary>
            /// Satuan kadaluarsa untuk item paket kunjungan (hari / minggu / bulan / tahun)
            /// </summary>
            public const string SA0205 = "SA0205";
            public const string SA0220 = "SA0220";

            /// <summary>
            /// Is Bridging To i-Care
            /// </summary>
            public const string SA0222 = "SA0222";
            public const string WARNING_PATIENT_PERSONAL_OUTSTANDING_REG_DIFF_DAY = "SA0231";

            public const string JABATAN_REPORT_PM90032 = "SA0240";

            #region Maspion
            public const string SA_IS_BRIDGING_MASPION = "SA0179";
            public const string SA_MASPION_URLAPI = "SA0180";
            public const string SA_MASPION_PASSWORD_CREDENTIAL = "SA0181";
            public const string SA_MASPION_TOKEN_SIGNATURE = "SA0182";

            public const string SA_IS_BRIDGING_TO_IPTV = "SA0188";
            #endregion

            #region Payment Gateway
            public static class PaymentGatewayConfig
            {
                public const string DOKU_NMID = "SA0170";
            }
            #endregion

            #region Drug Alert
            public const string DRUG_ALERT_VENDOR_NAME = "SA0209";
            public const string DRUG_ALERT_BASE_URL = "SA0210";
            public const string DRUG_ALERT_USERNAME = "SA0211";
            public const string DRUG_ALERT_PASSWORD = "SA0212";
            public const string IS_USING_DRUG_ALERT = "SA0213";
            #endregion

            public const string ADULT_WRISTBAND_TYPE = "";
            public const string INFANT_WRISTBAND_TYPE = "";

            public const string IS_MEDICAL_CHECKUP_ALLOW_BACK_DATE = "MC0001";
            public const string MEDICAL_CHECKUP_CLASS = "MC0002";
            public const string MC_KODE_DEFAULT_DOKTER = "MC0003";
            public const string MC_IS_USING_REGISTRATION_PARAMEDICID = "MC0005";
            public const string MC_FILE_TEMPLATE_EXCEL_DOWNLOAD = "MC0006";
            public const string MC_PENGISIAN_HASIL_LEBIH_DARISATU = "MC0008";
            public const string MC_GENERATE_ORDER_AUTO_PROPOSED = "MC0009";
            public const string MC_KELOMPOK_AUTO_CREATE_PATIENT = "MC0010";

            public const string MC_REGISTRATION_ORDER_BY = "MC0012";
            public const string MC_ITEMTAMBAHAN_PROPOSE = "MC0013";

            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string MC0014 = "MC0014";

            #region Imaging Parameter
            public const string IS_KODE_PENUNJANG_RADIOLOGI = "IS0001";
            public const string IS_PRINT_HASIL_SETELAH_VERIFIKASI = "IS0002";
            public const string IS_RIS_BRIDGING = "IS0003";
            public const string IS_RIS_BRIDGING_PROTOCOL = "IS0004";
            public const string IS_RIS_CONSUMER_ID = "IS0005";
            public const string IS_RIS_CONSUMER_PASSWORD = "IS0006";
            public const string IS_RIS_WEB_API_URL = "IS0007";
            public const string IS_RIS_WEB_VIEW_URL = "IS0010";
            public const string IS_KODE_DEFAULT_DOKTER = "IS0011";
            public const string IS_WAKTU_TUNGGU_RADIOLOGI = "IS0012";
            public const string IS_EMAIL_RADIOLOGI = "IS0013";
            public const string IS_RIS_HL7_BROKER = "IS0014";
            public const string IS_NAMA_TANDA_TANGAN_CETAKAN_HASIL_RADIOLOGI = "IS0015";
            public const string IS_BRIDGING_TOOLS = "IS0016";
            public const string IS_RESULT_SEND_TO_BRIDGING = "IS0017";
            public const string IS_FORMAT_CETAKAN_LABEL_COVER = "IS0018";
            public const string IS_AUTOMATICALLY_SEND_TO_BRIDGING = "IS0019";
            public const string IMAGING_CHARGE_CLASS = "IS0020";
            public const string IMAGING_CLASS = "IS0020";
            public const string IS_CLOSE_REGISTRATION_CHECK_OUTSTANDING_IMAGING_RESULT = "IS0021";
            public const string IS_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "IS0022";
            public const string IS_HL7_MESSAGE_FORMAT = "IS0023";
            public const string IS_RIS_USING_RESULT_IN_PDF = "IS0025";
            public const string IS_FORMAT_CETAKAN_LABEL_COVER_ENG = "IS0027";
            public const string IS_PEMERIKSAAN_RADIOLOGI_HANYA_BPJS = "IS0028";
            public const string IS_JENIS_PRINTER_CETAKAN_RADIOLOGI = "IS0029";
            public const string IS0030 = "IS0030";
            #endregion

            #region Laboratory Parameter
            public const string LB_KODE_PENUNJANG_LABORATORIUM = "LB0001";
            public const string LB_PRINT_HASIL_SETELAH_VERIFIKASI = "LB0002";
            public const string LB_BRIDGING_LIS = "LB0003";
            public const string LB_LIS_BRIDGING_PROTOCOL = "LB0004";
            public const string LB_LIS_CONSUMER_ID = "LB0005";
            public const string LB_LIS_CONSUMER_PASSWORD = "LB0006";
            public const string LB_LIS_WEB_API_URL = "LB0007";
            public const string LB_KODE_DEFAULT_DOKTER = "LB0009";
            public const string LB_FORMAT_CETAKAN_LABEL = "LB0010";
            public const string LB_ALAMAT_PRINTER_LABEL = "LB0011";
            public const string LB_INTERVAL_AUTO_REFRESH = "LB0012";
            public const string LB_WAKTU_TUNGGU_LABORATORIUM = "LB0013";
            public const string LB_PEMERIKSAAN_LABORATORIUM_HANYA_BPJS = "LB0016";
            public const string LB_CHARGE_CLASS = "LB0021";
            public const string LABORATORY_CLASS = "LB0021";
            public const string LB_PRINT_AFTER_PROPOSE_ORDER = "LB0024";
            public const string LB_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "LB0025";
            public const string LB_LIS_PROVIDER = "LB0026";
            public const string LB_IS_SEND_MEDICALNO_TO_LIS = "LB0027";
            public const string LB_LIS_HL7_BROKER = "LB0028";
            public const string LB_HL7_MESSAGE_FORMAT = "LB0029";
            public const string LB_IS_SEND_MEDICALNO_TO_LIS_OUTPATIENT_MUST_AFTER_CLOSE_TRANSACTION = "LB0030";
            public const string LB_IS_SEND_TO_LIS_AFTER_SAVE_PAYMENT = "LB0032";
            public const string LB_SMPT_CLIENT_CONF = "LB0033";
            public const string LB0034 = "LB0034";
            public const string LB0035 = "LB0035";
            public const string LB_IS_USING_RESULT_DELIVERY_PLAN = "LB0036";
            public const string LB_ITEM_ID_KREATININ_SERUM = "LB0038";
            public const string LB_FRACTION_ID_KREATININ_SERUM_DARAH = "LB0039";
            public const string LB_FRACTION_ID_eGFR = "LB0040";
            public const string LB_IS_PREVIEW_RESULT_AFTER_PROPOSED_RESULT = "LB0041";
            public const string LB_FORMAT_LABEL_AMPLOP_LABORATORIUM = "LB0042";
            #endregion

            public const string IS_MEDICAL_DIAGNOSTIC_ALLOW_BACK_DATE = "MD0001";
            public const string IMAGING_SERVICE_UNIT_ID = "MD0002";
            public const string LABORATORY_SERVICE_UNIT_ID = "MD0003";
            public const string IS_MEDICAL_DIAGNOSTIC_RESULT_NEED_VERIFICATION = "MD0004";
            public const string PRESCRIPTION_RETURN_ITEM = "MD0005";
            public const string MD_SERVICE_UNIT_OPERATING_THEATRE = "MD0006";
            public const string MD_SERVICE_UNIT_FISIOTHERAPY = "MD0007";
            public const string MD_CHARGE_CLASS = "MD0009";
            public const string DIAGNOSTIC_CLASS = "MD0009";
            public const string MD_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "MD0010";

            public const string MD0006 = "MD0006";
            public const string MD0012 = "MD0012";
            public const string MD0013 = "MD0013";
            public const string MD0014 = "MD0014";
            public const string MD0015 = "MD0015";
            public const string MD_IS_REGISTRATION_OPENED_SHOW_IN_REGISTRATION_LIST = "MD0015";
            public const string MD0016 = "MD0016";
            public const string MD0017 = "MD0017";
            public const string MD0018 = "MD0018";

            public const string TEST_ORDER_HANYA_UNTUK_ITEM_PEMERIKSAAN = "MD0020";
            public const string MD_JENIS_PRINTER_PENUNJANG_MEDIS = "MD0021";
            public const string MD_FORMAT_CETAKAN_PENUNJANG_MEDIS = "MD0022";
            public const string MD_REGISTRATION_ORDER_BY = "MD0023";
            public const string MD_IS_USING_MULTIVISIT_SCHEDULE = "MD0024";
            public const string MD_IS_BLOODBANK_ORDER_REMARKS_COPY_FROM_DIAGNOSE = "MD0025";

            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string MD0027 = "MD0027";
            public const string MD0028 = "MD0028";
            public const string MD_PEMERIKSAAN_PENUNJANG_MEDIS_HANYA_BPJS = "MD0029";

            public const string PRESCRIPTION_FEE_AMOUNT = "PH0001";
            public const string PHARMACY_PHYSICIAN = "PH0002";
            public const string NON_MASTER_ITEM = "PH0003";
            public const string PHARMACIST = "PH0004";
            public const string PHARMACIST_LICENSE_NO = "PH0005";
            public const string PH_PHONE_EXTENSION = "PH0067";
            public const string FM_KONTROL_ADVERSE_REACTION = "PH0006";
            public const string FM_MAKSIMUM_DURASI_NARKOTIKA = "PH0007";
            public const string FM_KONTROL_DUPLIKASI_TERAPI = "PH0008";
            public const string FM_ALAMAT_PRINTER_ETIKET_OBAT_DALAM = "PH0020";
            public const string FM_ALAMAT_PRINTER_ETIKET_OBAT_LUAR = "PH0021";
            public const string FM_FORMAT_CETAKAN_ETIKET = "PH0022";
            public const string FM_FORMAT_CETAKAN_ETIKET_BY_TYPE = "PH0034";
            public const string FM_FORMAT_CETAKAN_ETIKET_RJ = "PH0035";
            public const string FM_DEFAULT_ATURAN_PAKAI = "PH0023";
            public const string FM_PATIENT_GRID_REFRESH_INTERVAL = "PH0024";
            public const string FM_IS_ALLOW_NON_MASTER_ITEM = "PH0025";
            public const string FM_USING_UDD_FOR_INPATIENT = "PH0026";
            public const string FM_FORMAT_CETAKAN_LABEL_UDD = "PH0027";
            public const string FM_KONTROL_PEMBERIAN_OBAT_KRONIS_BPJS = "PH0028";
            public const string FM_JANGKA_WAKTU_PEMBERIAN_OBAT_KRONIS_BPJS = "PH0029";
            public const string FM_WAKTU_TUNGGU_PELAYANAN_RESEP_FARMASI = "PH0030";
            public const string MANAGER_FARMASI = "PH0031";
            public const string FM_UDD_ROUNDING_SYSTEM = "PH0032";
            public const string FM_KONTROL_ERROR_ALERGI = "PH0033";
            public const string FM_CHARGE_CLASS = "PH0036";
            public const string PHARMACY_CLASS = "PH0036";
            public const string PH0037 = "PH0037"; //Pembuatan Nomor Referensi per Lokasi setiap pelayanan resep
            public const string FM_IS_AS_REQUIRED = "PH0043";
            public const string FM_PRINT_ETIKET_DETAIL_OBAT_RACIKAN = "PH0053";
            public const string FM_FORMAT_CETAKAN_ETIKET_DETAIL_OBAT_RACIKAN = "PH0054";
            public const string FM_USE_UDD_DRUG_DISPENSE_LABEL = "PH0059";
            public const string FM_AKTIFASI_FILTER_JENIS_RESEP_RAWAT_INAP = "PH0060";
            public const string FM_FILTER_VALUE_JENIS_RESEP_RAWAT_INAP = "PH0061";
            public const string USE_LAST_PURCHASE_EXPIRED_DATE = "PH0063";
            public const string USE_FILTER_DATE_NON_UDD = "PH0064";
            public const string IS_DEFAULT_TRANSAKSI_BPJS = "PH0065";
            public const string MAX_DAY_DISPENSE_UDD = "PH0066";
            public const string PH0070 = "PH0070";
            public const string PH0071 = "PH0071";
            public const string PH0078 = "PH0078";
            public const string PH_IS_QPHISTORY_FOR_NEW_TRANSACTION = "PH0072";
            public const string PH_IS_RIGHTPANELPRINT_MUST_PROPOSED_CHARGES = "PH0073";
            public const string FM_WAKTU_TUNGGU_PELAYANAN_RESEP_FARMASI_RACIKAN = "PH0076";
            public const string MANAGER_PHARMACIST_LICENSE_NO = "PH0077";

            public const string PH_REGISTRATION_ORDER_BY = "PH0079";
            public const string IS_VISIBLE_FILTER_TAKEN_ITEM_IN_QUICK_PICKS_HISTORY = "PH0081";

            #region Nursing Parameter
            public const string IS_NURSING_RUN_IN_LINKED_MODE = "NT0001";
            public const string NR0001 = "NR0001";
            public const string LOAD_TRANSACTION_NO_WHEN_OPEN_NURSING_TRANSACTION_MENU = "NR0003";
            public const string IS_VISIBLE_PATIENT_HANDOVER_NOTES_IN_NURSING_JOURNAL = "NR0004";
            #endregion

            public const string RM_CETAK_TRACER_OTOMATIS = "RM0001";
            public const string RM_CETAK_TRACER_PASIEN_BARU = "RM0002";
            public const string RM_JENIS_PRINTER_TRACER = "RM0003";
            public const string RM_FORMAT_CETAKAN_TRACER = "RM0004";
            public const string RM_ALAMAT_PRINTER_TRACER = "RM0005";
            public const string RM_FORMAT_CETAKAN_LABEL = "RM0006";
            public const string RM_FORMAT_CETAKAN_LABEL_2 = "RM0040";
            public const string RM_REPORT_CODE_CUSTOM_LABEL = "RM0041";
            public const string RM_JUMLAH_CETAKAN_LABEL_RD = "RM0007";
            public const string RM_JUMLAH_CETAKAN_LABEL_RJ = "RM0008";
            public const string RM_JUMLAH_CETAKAN_LABEL_RI = "RM0009";
            public const string RM_JUMLAH_CETAKAN_LABEL_MD = "RM0010";
            public const string RM_ALAMAT_PRINTER_LABEL_RD = "RM0011";
            public const string RM_ALAMAT_PRINTER_LABEL_RJ = "RM0012";
            public const string RM_ALAMAT_PRINTER_LABEL_RI = "RM0013";
            public const string RM_ALAMAT_PRINTER_LABEL_MD = "RM0014";
            public const string RM_FORMAT_GELANG_PASIEN_DEWASA = "RM0015";
            public const string RM_FORMAT_GELANG_PASIEN_ANAK = "RM0016";
            public const string RM_FORMAT_GELANG_PASIEN_BAYI = "RM0017";
            public const string RM_FORMAT_CETAKAN_COVER = "RM0018";
            public const string RM_ALAMAT_PRINTER_COVER = "RM0019";
            public const string RM_FORMAT_BUKTI_PENDAFTARAN = "RM0020";
            public const string RM_ALAMAT_PRINTER_BUKTI_PENDAFTARAN = "RM0021";
            public const string RM_GELANG_PASIEN_ANAK_KHUSUS = "RM0022";
            public const string RM_ALAMAT_PRINTER_GELANG_PASIEN_LAKI = "RM0023";
            public const string RM_ALAMAT_PRINTER_GELANG_PASIEN_PEREMPUAN = "RM0024";
            public const string RM_ALAMAT_PRINTER_GELANG_PASIEN_ANAK = "RM0025";
            public const string RM_ALAMAT_PRINTER_GELANG_PASIEN_BAYI = "RM0026";
            public const string RM_MULTI_LOKASI_PENDAFTARAN = "RM0027";
            public const string RM_FORMAT_BUKTI_ORDER_PENUNJANG = "RM0028";
            public const string RM_NOMOR_RM_DIGUNAKAN_KEMBALI = "RM0029";
            public const string RM_JENIS_PRINTER_BUKTI_PENDAFTARAN = "RM0030";
            public const string RM_MAX_JUMLAH_LABEL = "RM0031";
            public const string RM_ALAMAT_PRINTER_DATACARD = "RM0032";
            public const string RM_DEFAULT_PATIENT_WALKIN = "RM0033";
            public const string RM_DEFAULT_PHARMACY_VISITTYPE = "RM0034";
            public const string RM_DEFAULT_MONTH_FOR_RETENTION = "RM0038";
            public const string RM_OUTPATIENT_USING_EMR = "RM0042";
            public const string RM_EMR_MEDICAL_FORM_TYPE = "RM0043";
            public const string RM_FORMAT_GELANG_PASIEN_RAWAT_JALAN = "RM0045";
            public const string NOTES_TEST_ORDER_COPY_DIAGNOSE = "RM0046";
            public const string RM_FORMAT_CETAKAN_TRACER_APPOINTMENT = "RM0047";
            public const string RM_CETAK_BUKTI_PENDAFTARAN_KETIKA_KONTROL_BERKAS_APPOINTMENT = "RM0049";
            public const string RM_REGISTRASI_SELAIN_RAJAL_MEMPERHATIKAN_CUTI_DOKTER = "RM0050";
            public const string RM_VOID_REGISTRASI_VALIDASI_DATA_CHIEF_COMPLAINT = "RM0051";
            public const string RM_ICON_IS_PATIENT_INFECTIOUS_IS_ALLOW_DISPLAY_IN_PATIENT_BANNER = "RM0052";
            public const string RM_IS_SSN_MANDATORY = "RM0054";
            public const string RM_IS_PARAMEDIC_IN_REGISTRATION_USE_SCHEDULE = "RM0056";
            public const string RM_IS_PREFIK_SURAT_KETERANGAN_BEBAS_NARKOBA = "RM0059";
            public const string RM_IS_NAMA_RS_SURAT_KETERANGAN_BEBAS_NARKOBA = "RM0060";
            public const string RM_KODE_REPORT_SURAT_KETERANGAN_KEMATIAN = "RM0062";
            public const string RM_BLOK_DOUBLE_PATIENT_DATA = "RM0063";
            public const string RM_IS_MOBILE_PHONE_NUMERIC = "RM0064";
            public const string RM_IS_BPJS_VISIT_CAN_CHANGE_CHARGE_CLASS = "RM0065";
            public const string RM_IS_VOID_REG_DELETE_LINKEDREG = "RM0066";
            public const string RM_EKLAIM_SEND_EKLAIM_MEDICALNO = "RM0067";
            public const string RM_CETAK_HASIL_LAB_DI_RINGKASAN_PERAWATAN = "RM0068";
            public const string RM_DEFAULT_LAST_PARAMEDIC = "RM0069";
            public const string RM_IS_SALUTATION_MANDATORY = "RM0070";
            public const string RM_SISTOLE_AND_DIASTOLE_FROM_LINKED_REGISTRATION_IN_EKLAIM_PROCESS = "RM0071";
            public const string RM0072 = "RM0072";
            public const string RM0073 = "RM0073";
            public const string RM_PATIENT_LIST_HAVE_DIAG_NOT_ONLY_MAIN_DIAGNOSIS = "RM0074";
            public const string RM_IS_RM_DIAGNOSE_TEXT_EDITABLE = "RM0075";
            public const string RM_IS_FOLLOWUP_DEFAULT_CHECKED = "RM0076";
            public const string RM_PATIENT_NAME_NEW_BORN_NOT_TAKE_FROM_MOTHER_NAME = "RM0077";
            public const string RM0078 = "RM0078";
            public const string RM_OTOMATIS_MENGISI_DIAGNOSA_REKAM_MEDIS_DARI_DIAGNOSA_KLAIM = "RM0079";
            public const string RM_PROSES_EKLAIM_MENAMPILKAN_DIAGNOSA_MASUK = "RM0080";

            //public const string PH_IS_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "PH0038";
            public const string PH_DEFAULT_EMBALACE_CODE_PRESCRIPTION = "PH0039";
            public const string PH_AUTO_INSERT_EMBALACE_PRESCRIPTION = "PH0040";
            public const string PH_USED_STRENGTH_UNIT_AS_DEFAULT = "PH0041";
            public const string PH_CREATE_BILL_AFTER_PROPOSED_TRANSACTION = "PH0042";
            public const string PH0068 = "PH0068";
            public const string PH_IS_REVIEW_PRESCRIPTION_MANDATORY_FOR_PROPOSED_TRANSACTION = "PH0069";
            public const string PH_CREATE_QUEUE_LABEL = "PH0044";
            public const string PH_ITEM_QTY_WITH_SPECIAL_QUEUE_PREFIX = "PH0045";
            public const string PH_IS_AUTO_RELOCATE_DISPENSARY = "PH0047";
            public const string PH_AUTO_RELOCATE_DISPENSARY_TIME = "PH0048";
            public const string PH_AUTO_RELOCATE_DISPENSARY_ID = "PH0049";
            public const string PH_USE_BPJS_ORDER_ENTRY = "PH0050";
            public const string PH_BPJS_TAKEN_QTY_FORMULA = "PH0051";
            public const string PH_AUTO_VOID_DISCHARGE_PRESCRIPTION_ORDER = "PH0052";
            public const string FM_ETIKET_UDD_PRINT_KOMPOSISI_RACIKAN = "PH0055";
            public const string PH_SEQUENCE_FOR_MEDICATION_LIST = "PH0056";
            public const string PH_UDD_MEDICATION_RECEIPT = "PH0057";
            public const string PH_CHECKBOX_IGNORE_DATE = "PH0058";

            public const string PH0075 = "PH0075";
            /// <summary>
            /// Order Pemeriksaan dan Resep untuk Pasien Inhealth bersifat terbatas
            /// </summary>
            public const string PH0082 = "PH0082";
            /// <summary>
            /// Order Resep (Menu Farmasi) untuk Pasien BPJS bersifat terbatas
            /// </summary>
            public const string PH0083 = "PH0083";


            public const string MC_MENGGUNAKAN_DEKSTOP_MCU = "MC0007";
            public const string EM0056 = "EM0056";

            public const string SA_BROKER_REPORT = "SA0206";
            public const string SA0215 = "SA0215";
            public const string SA_CREATE_APPOINTMENT_AFTER_CREATE_SURKON = "SA0217";
            public const string FN0206 = "FN0206";
            public const string FN0209 = "FN0209";
            #region Radioterapi
            public const string RT0001 = "RT0001";
            public const string RT0002 = "RT0002";
            /// <summary>
            /// Is Bridging to OIS
            /// </summary>
            public const string RT0003 = "RT0003";
            /// <summary>
            /// Protokol Bridging dengan OIS
            /// </summary>
            public const string RT0004 = "RT0004";
            /// <summary>
            /// Format HL-7 Message untuk OIS
            /// </summary>
            public const string RT0005 = "RT0005";
            /// <summary>
            /// OIS HL7 Broker Endpoint Address
            /// </summary>
            public const string RT0006 = "RT0006";
            #endregion
        }

        #endregion

        #region Menu Code Tab
        public static class MenuCodeTab
        {
            #region MCU
            public static class MC080300
            {
                public const string BELUM_DIPROSES = "MC080300T01";
                public const string SUDAH_PERJANJIAN = "MC080300T02";
                public const string SUDAH_REGISTRASI = "MC080300T03";
            }
            #endregion
        }
        #endregion

        #region RoomType
        public static class RoomType
        {
            public const string ICU = "X317^002";
            public const string NICUPICU = "X317^003";
            public const string ISOLASI = "X317^005";
            public const string VK = "X317^007";
            public const string Ruangan = "X317^999";
        }
        #endregion

        #region Menu Code
        public static class MenuCode
        {
            #region Accounting
            public static class Accounting
            {
                public const string CHART_OF_ACCOUNT = "AC010100";
                public const string SUB_LEDGER = "AC010200";
                public const string JOURNAL_TEMPLATE = "AC010300";
                public const string FA_DEPRECIATION_METHOD = "AC010401";
                public const string FA_GROUP = "AC010402";
                public const string FA_LOCATION = "AC010403";
                public const string FA_ITEM = "AC010404";
                public const string FA_ITEM_FROM_PURCHASE_RECEIVE = "AC010405";
                public const string FA_ITEM_FROM_PURCHASE_RECEIVE_PROCESS_LIST = "AC010406";
                public const string SUB_LEDGER_TYPE = "AC010500";
                public const string ACCOUNT_DISPLAY_SETTING = "AC010600";
                public const string CASH_FLOW_ACCOUNT = "AC010700";
                public const string REVENUE_COST_CENTER = "AC010800";
                public const string CASH_FLOW_TYPE = "AC010900";
                public const string BUDGETING_MASTER = "AC011010";
                public const string BUDGETING_REALIZATION_INFORMATION = "AC011020";
                public const string BUDGETING_TEMPLATE = "AC011030";
                public const string BUDGETING_MASTER_PER_MONTH = "AC011040";
                public const string BUDGETING_REALIZATION_INFORMATION_PER_MONTH = "AC011040";
                public const string TREASURY_TRANSACTION_MAPPING = "AC011100";

                public const string GL_SETTING_LEVEL1 = "AC020100";
                public const string GL_REVENUE_ACCOUNT = "AC020201";
                public const string GL_PRODUCT_LINE = "AC020202";
                public const string GL_CUSTOMER_LINE = "AC020203";
                public const string GL_SUPPLIER_LINE = "AC020204";
                public const string GL_PATIENT_PAYMENT_METHOD = "AC020205";
                public const string GL_MAPPING_REVENUE_AND_DISCOUNT = "AC020206";
                public const string GL_MAPPING_STANDARD_CODE = "AC020207";
                public const string GL_PATIENT_PAYMENT_TYPE = "AC020208";
                public const string GL_PATIENT_PAYMENT_METHOD_EDC_BANK = "AC020209";

                public const string GL_PAYMENT_METHOD = "AC020310";
                public const string GL_OP_REVENUE_ACCOUNT = "AC020321";
                public const string GL_IP_REVENUE_ACCOUNT = "AC020322";
                public const string GL_ER_REVENUE_ACCOUNT = "AC020323";
                public const string GL_MD_REVENUE_ACCOUNT = "AC020324";
                public const string GL_OTC_REVENUE_ACCOUNT = "AC020325";
                public const string GL_MAPPING_REVENUE_AND_DISCOUNT_DETAIL = "AC020326";
                public const string GL_FINAL_DISCOUNT_ACCOUNT = "AC020330";
                public const string GL_OP_REVENUE_SHARING_ACCOUNT = "AC020341";
                public const string GL_IP_REVENUE_SHARING_ACCOUNT = "AC020342";
                public const string GL_ER_REVENUE_SHARING_ACCOUNT = "AC020343";
                public const string GL_MD_REVENUE_SHARING_ACCOUNT = "AC020344";
                public const string GL_WAREHOUSE_PRODUCT_LINE_ACCOUNT = "AC020351";
                public const string GL_PRODUCT_LINE_ACCOUNT_SOURCEID = "AC020352";
                public const string GL_PRODUCT_LINE_HEALTHCARE_UNIT = "AC020353";
                public const string GL_AP_PAYMENT = "AC020361";
                public const string GL_ACCOUNT_PAYABLE = "AC020362";
                public const string GL_AP_REVENUE_SHARING = "AC020363";
                public const string GL_AR_PAYMENT = "AC020371";
                public const string GL_AR_PERAWATAN = "AC020372";
                public const string GL_AR_PROCESS = "AC020373";
                public const string GL_AR_INSTANSI = "AC020374";
                public const string GL_AR_ADJUSTMENT = "AC020375";
                public const string GL_FA_WRITE_OFF_ACCOUNT = "AC020380";
                public const string GL_ITEM_GROUP_HEALTHCARE_UNIT = "AC020391";

                public const string BANK_RECONCILIATION_ENTRY = "AC030100";

                public const string FA_ITEM_LIST = "AC040100";
                public const string FA_ITEM_MOVEMENT = "AC040101";
                public const string FA_WRITE_OFF = "AC040102";
                public const string FA_VOID_WRITE_OFF = "AC040200";
                public const string FA_COMBINED_ITEM_MOVEMENT = "AC040301";
                public const string FA_APPROVAL_COMBINED_ITEM_MOVEMENT = "AC040302";
                public const string FA_COMBINED_WRITE_OFF = "AC040401";
                public const string FA_APPROVAL_COMBINED_WRITE_OFF = "AC040402";
                public const string FA_ACCEPTANCE = "AC040500";

                public const string JOURNAL_ENTRY = "AC050100";
                public const string JOURNAL_ENTRY_CURRENT_DATE = "AC050101";
                public const string INTERFACE_JOURNAL_PROCESS = "AC050200";
                public const string JOURNAL_LIST = "AC050300";
                public const string INTERFACE_JOURNAL_PROCESS_BY_USER = "AC050400";
                public const string JOURNAL_AUDITED_ENTRY = "AC050500";

                public const string JOURNAL_POSTING = "AC060100";
                public const string BRIDGING_JOURNAL = "AC060200";
                public const string JOURNAL_CLOSING = "AC060300";
                public const string JOURNAL_UNPOSTING = "AC060400";
                public const string JOURNAL_RECALCULATE_BALANCE = "AC060500";
                public const string JOURNAL_POSTINGV2 = "AC060600";
                public const string JOURNAL_UNPOSTINGV2 = "AC060700";

                public const string UNBALANCE_JOURNAL = "AC070100";
                public const string BALANCE_INFORMATION = "AC070200";
                public const string BALANCE_INFORMATION_PER_ACCOUNT = "AC070300";
                public const string BALANCE_INFORMATION_SUB_ACCOUNT = "AC070400";
                public const string INFORMATION_JOURNAL_PROCESS = "AC070500";
                public const string INTERFACE_JOURNAL_STATUS_INFORMATION = "AC070600";
                public const string INFORMATION_JOURNAL_PROCESS_STATUS = "AC070700";
                public const string JOURNAL_INFORMATION_BY_REFERENCE_NUMBER = "AC070800";

                public const string GL_UTILITY_IMPORT_JOURNAL_ENTRY = "AC080100";

                public const string REPORT = "AC090000";
            }

            #endregion

            #region Emergency Care
            public static class EmergencyCare
            {
                public const string ER_PROCESS_BRIDING_MASPION = "ER070306";
                public const string REGISTRATION = "ER010100";
                public const string MEDICAL_FOLDER_REQUEST = "ER010200";
                public const string DIAGNOSE_ENTRY = "ER010300";

                public const string FOLLOWUP_PATIENT_DISCHARGE = "ER010400";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS = "ER010411";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "ER010412";
                public const string FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "ER010413";
                public const string FOLLOWUP_PATIENT_PAGE_EMERGENCY_INITIAL_ASSESSMENT = "ER010414";
                public const string FOLLOWUP_POPULATION_ASSESSMENT = "ER010415";
                public const string FOLLOWUP_MEDICAL_ASSESSMENT_FORM = "ER010416";
                public const string FOLLOWUP_NUTRITION_SCREENING = "ER010417";
                public const string FOLLOWUP_FALL_RISK_ASSESSMENT_FORM = "ER010418";
                public const string FOLLOWUP_PAIN_ASSESSMENT_FORM = "ER010419";
                public const string FOLLOWUP_EWS_ASSESSMENT_FORM = "ER010426";

                public const string FOLLOWUP_PATIENT_PAGE_ALLERGY = "ER010421";
                public const string FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY = "ER010422";
                public const string FOLLOWUP_PATIENT_PAGE_VACCINATION = "ER010423";
                public const string FOLLOWUP_MST_FORM = "ER010424";
                public const string FOLLOWUP_ANTENATAL_RECORD = "ER010425";

                public const string FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM = "ER010431";
                public const string FOLLOWUP_PATIENT_PAGE_VITAL_SIGN = "ER010432";

                public const string FOLLOWUP_NURSING_ASSESSMENT_PROBLEM = "ER010441";
                public const string FOLLOWUP_NURSING_ASSESSMENT_PROCESS = "ER010442";
                public const string FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION = "ER010443";

                public const string FOLLOWUP_NURSING_NOTE = "ER010451";
                public const string FOLLOWUP_PHYSICIAN_NOTE = "ER010452";
                public const string FOLLOWUP_INTEGRATED_NOTE = "ER010453";
                public const string FOLLOWUP_PATIENT_PAGE_NURSING_JOURNAL = "ER010454";
                public const string FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "ER010455";
                public const string FOLLOWUP_TRANSACTION_PAGE_PATIENT_DIAGNOSIS = "ER010456";
                public const string FOLLOWUP_NURSE_HANDS_OVER = "ER010457";
                public const string FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION = "ER010458";
                public const string FOLLOWUP_MEDICAL_RESUME = "ER010459";

                public const string FOLLOWUP_TEST_ORDER_RESULT_LIST = "ER010461";
                public const string FOLLOWUP_VENTILATOR_MONITORING = "ER010462";
                public const string FOLLOWUP_MONITORING_INTAKE_OUTPUT = "ER010463";
                public const string FOLLOWUP_PARTOGRAF = "ER010464";

                public const string FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT = "ER010471";
                public const string FOLLOWUP_PAGE_TESTORDER_INFORMATION = "ER010472";

                public const string FOLLOWUP_PATIENT_PAGE_E_DOCUMENT = "ER010481";
                public const string FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT = "ER010482";

                public const string MEDICAL_RECORD = "ER010500";
                public const string MEDICAL_RECORD_VIEW = "ER010501";
                public const string MEDICAL_RECORD_VISIT = "ER010502";
                public const string MEDICAL_RECORD_VISIT_LIST = "ER010503";


                public const string PATIENT_PAGE = "ER020100";
                public const string CHIEF_COMPLAINT = "ER020110";
                public const string DATA_PATIENT_EMERGENCY_PAGE_MEDICAL_ASSESSMENT_FORM = "ER020111";
                public const string DATA_PATIENT_FALL_RISK_ASSESSMENT_FORM = "ER020112";
                public const string DATA_PATIENT_PAIN_ASSESSMENT_FORM = "ER020113";
                public const string DATA_PATIENT_NUTRITION_SCREENING = "ER020114";
                public const string DATA_PATIENT_MST_FORM = "ER020115";
                public const string DATA_PATIENT_NUTRITION_SCREENING_VERIFY = "ER020116";
                public const string ER020121 = "ER020121";
                public const string DATA_PATIENT_VITAL_SIGN = "ER020131";
                public const string DATA_PATIENT_REVIEW_OF_SYSTEM = "ER020132";
                public const string DATA_PATIENT_NURSING_ASSESSMENT_PROBLEM = "ER020141";
                public const string DATA_PATIENT_NURSING_ASSESSMENT_PROCESS = "ER020142";
                public const string DATA_PATIENT_PATIENT_NURSING_NOTES = "ER020151";
                public const string DATA_PATIENT_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "ER020152";
                public const string DATA_PATIENT_PATIENT_PAGE_PARAMEDIC_NOTE = "ER020153";
                public const string DATA_PATIENT_PATIENT_PAGE_PHYSICIAN_NOTE = "ER020154";
                public const string DATA_PATIENT_PATIENT_PAGE_INTEGRATED_NOTE = "ER020155";
                public const string DATA_PATIENT_PATIENT_DIAGNOSE_DIAGNOSTIC = "ER020156";
                public const string DATA_PATIENT_PATIENT_VERIFY_NURSING_JOURNAL = "ER020157";
                public const string DATA_PATIENT_PATIENT_NURSING_JOURNAL = "ER020158";
                public const string DATA_PATIENT_MEDICAL_RESUME = "ER020159";

                public const string DATA_PATIENT_TEST_ORDER_LB_IS_RESULT_LIST = "ER020161";
                public const string DATA_PATIENT_MONITORING_INTAKE_OUTPUT = "ER020162";
                public const string DATA_PATIENT_VENTILATOR_MONITORING = "ER020163";
                public const string DATA_PATIENT_PARTOGRAF = "ER020164";

                public const string DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT = "ER020171";
                public const string PATIENT_OUTSTANDING_ORDER_LIST_ER020172 = "ER020172";

                public const string DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT = "ER020181";

                public const string PATIENT_TRANSACTION_MEDICATION_ORDER_LIST = "ER020191";
                public const string PATIENT_TRANSACTION_DIAGNOSTIC_ORDER = "ER020192";
                public const string PATIENT_DIAGNOSTIC_BLOOD_BANK_ORDER = "ER020193";
                public const string PATIENT_DIAGNOSTIC_TEST_ORDER_OK = "ER020194";
                public const string PATIENT_TRANSACTION_PAGE_PROCEDURES = "ER020195";

                public const string VITAL_SIGN = "ER020102";
                public const string PHYSICAL_EXAMINATION = "ER020103";
                public const string PHYSICAL_EXAMINATION_BODY_DIAGRAM = "ER020104";
                public const string PATIENT_DIAGNOSIS = "ER020107";
                public const string PATIENT_PROCEDURES = "ER020108";
                public const string ASSESMENT_NURSING_NOTE = "ER020109";
                public const string PATIENT_ALLERGY = "ER020199";
                public const string PATIENT_TRANSACTION_PAGE = "ER020200";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "ER020201";
                public const string PATIENT_TRANSACTION_TEST_ORDER = "ER020202";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "ER020203";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "ER020204";
                public const string PATIENT_TRANSACTION_OUTPATIENT_ORDER = "ER020205";
                public const string PATIENT_TRANSACTION_BLOOD_BANK_ORDER = "ER020206";
                public const string PATIENT_TRANSACTION_NUTRITION_ORDER = "ER020207";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "ER020208";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "ER020223";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES = "ER020224";

                public const string PATIENT_PAGE_NURSE_ANAMNESIS = "ER020241";
                public const string PATIENT_PAGE_E_DOCUMENT = "ER020244";

                public const string PATIENT_PAGE_PATIENT_EDUCATION = "ER020246";
                public const string FALL_RISK_ASSESSMENT_FORM = "ER020249";
                public const string MEDICAL_ASSESSMENT_FORM = "ER020253";
                public const string MST_FORM = "ER020254";
                public const string NUTRITION_SCREENING = "ER020256";
                public const string NUTRITION_SCREENING_VERIFY = "ER020258";

                #region Pengkajian Pasien
                public const string PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "ER020242";
                public const string PATIENT_PAGE_EMERGENCY_INITIAL_ASSESSMENT = "ER020247";
                public const string PATIENT_PAGE_EMERGENCY_PAIN_ASSESSMENT = "ER020248";
                public const string POPULATION_ASSESSMENT = "ER020252";
                public const string EWS_ASSESSMENT = "ER020257";
                #endregion

                #region Pengkajian Riwayat Kesehatan
                public const string PATIENT_PAGE_ALLERGY = "ER020245";
                public const string ANTENATAL_RECORD = "ER020251";
                public const string PATIENT_PAGE_VACCINATION = "ER020255";
                #endregion

                #region Pemeriksaan Fisik
                public const string PATIENT_PAGE_REVIEW_OF_SYSTEM = "ER020243";
                #endregion

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "ER022301";
                public const string NURSING_ASSESSMENT_PROCESS = "ER022302";
                #endregion

                #region Catatan Terintegrasi
                public const string PATIENT_PAGE_NURSING_JOURNAL = "ER020218";
                public const string PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "ER020219";
                #endregion

                #region Monitoring dan Evaluasi
                public const string MONITORING_INTAKE_OUTPUT = "ER020292";
                public const string MONITORING_VENTILATOR = "ER020293";
                public const string PARTOGRAF = "ER020294";
                #endregion

                #region Proses dan Utilitas
                public const string PATIENT_PAGE_CHANGE_ORDER_STATUS = "ER021901";
                public const string BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU = "ER021902";
                public const string PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT = "ER021903";
                public const string ER021904 = "ER021904";
                #endregion

                public const string NURSING_NOTE = "ER020209";
                public const string PHYSICIAN_NOTE = "ER020210";
                public const string INTEGRATED_NOTE = "ER020211";
                public const string VITAL_SIGN_TRANSACTION = "ER020212";
                public const string TEST_ORDER_RESULT_LIST = "ER020213";
                public const string PATIENT_USE = "ER020214";
                public const string TRANSACTION_PAGE_PATIENT_DIAGNOSIS = "ER020215";
                public const string TRANSACTION_PAGE_PATIENT_PROCEDURES = "ER020216";
                public const string TRANSACTION_PAGE_PATIENT_DISPOSITION = "ER020217";
                public const string PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "ER020220";

                public const string PATIENT_PAGE_OBSTETRIC_HISTORY = "ER020231";

                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "ER020221";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "ER020222";

                public const string MEDICAL_RESUME = "ER020225";

                public const string PATIENT_PAGE_TRANSACTION_DISPOTITION = "ER020291";

                public const string PATIENT_ORDER_LIST = "ER020298";
                public const string PATIENT_OUTSTANDING_ORDER_LIST = "ER020299";
                public const string SERVICE_ORDER_TRANS = "ER020300";
                public const string EPISODE_MR_SUMMARY = "ER020400";
                public const string SERVICE_ORDER_RESULT_TRANS = "ER020500";

                public const string INFORMATION_PATIENT_SURGERY_REPORT = "ER020601";

                public const string SERVICE_ORDER_TRANS_AIO = "ER020700";

                public const string PATIENT_TRANSFER = "ER030100";
                public const string PATIENT_EMERGENCY_DISCHARGE = "ER030200";
                public const string CHANGE_EMERGENCY_BED_STATUS = "ER030300";

                public const string BILL_SUMMARY = "ER060100";
                public const string BILL_SUMMARY_CHARGES = "ER060111";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "ER060112";
                public const string BILL_SUMMARY_IMAGING = "ER060113";
                public const string BILL_SUMMARY_LABORATORY = "ER060114";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "ER060115";
                public const string BILL_SUMMARY_PHARMACY = "ER060116";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "ER060117";
                public const string BILL_SUMMARY_GENERATE_BILL = "ER060121";
                public const string BILL_SUMMARY_DISCOUNT = "ER060122";
                public const string BILL_SUMMARY_PAYMENT = "ER060123";
                public const string BILL_SUMMARY_DETAIL = "ER060124";
                public const string BILL_SUMMARY_TARIFF_INACBGS = "ER060125";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "ER060126";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "ER060127";
                public const string BILL_SUMMARY_PAYMENT_AR = "ER060128";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "ER060129";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "ER0601210";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "ER060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "ER060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "ER060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "ER060134";
                public const string BILL_SUMMARY_VOID_BILL = "ER060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "ER060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "ER060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "ER060144";
                public const string TRANSACTION_PAGE_SUMMARY_UPDATE_REGISTRATION = "ER060145";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "ER060146";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "ER060147";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "ER060148";
                public const string BILL_SUMMARY_CLOSE_BILLING = "ER060149";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "ER060149";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "ER0601410";

                public const string BPJS_PROCESS_CLAIM_ORDER = "ER060155";
                public const string E_DOCUMENT = "ER060161";

                public const string BILL_INFORMATION = "ER060191";
                public const string PENDING_TRANSACTION = "ER060192";
                public const string PAYMENT_TRANSACTION = "ER060193";
                public const string PATIENT_DIAGNOSE_EMERGENCY = "ER060194";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_EMERGENCY = "ER060195";
                public const string INFORMATION_CUSTOMER_PAYER_EMERGENCY = "ER060196";
                public const string INFORMATION_VISIT_HISTORY_EMERGENCY = "ER060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "ER060198";
                public const string INFORMATION_DISCOUNT_DETAIL = "ER060199";

                public const string PATIENT_BILL_DETAIL_REPRINT = "ER070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "ER070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "ER070121";
                public const string PAYMENT_RECEIPT_REPRINT = "ER070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "ER070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "ER070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "ER070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "ER070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "ER070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "ER070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "ER070240";

                public const string CANCEL_PATIENT_DISCHARGE = "ER070301";
                public const string VOID_REGISTRATION_DAILY = "ER070302";
                public const string CLOSE_REGISTRATION = "ER070303";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "ER070304";
                public const string VOID_REGISTRATION = "ER070305";


                public const string REGISTERED_PATIENT_LIST = "ER080110";
                public const string HOSPITALIZED_PATIENT_LIST = "ER080120";
                public const string MEDICAL_RECORD_LIST = "ER080130";
                public const string PRINTING_PATIENT_CARD = "ER080140";
                public const string INFORMATION_PATIENT_TRANSFER = "ER080150";
                public const string BED_INFORMATION = "ER080160";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "ER080170";
                public const string TRANSACTION_PATIENT_INFORMATION = "ER080210";
                public const string CORRECTION_TRANSACTION_INFORMATION = "ER080220";
                public const string PAYMENT_INFORMATION = "ER080230";
                public const string TARIFF_ESTIMATION = "ER080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "ER080320";
                public const string TARIFF_SERVICE_INFORMATION = "ER080330";
                public const string DRUGS_LOGISTICS_PRICE = "ER080340";
                public const string ER_CARI_DATA_SEP = "ER080410";
                public const string ER_DATA_KUNJUNGAN_PASIEN_BPJS = "ER080411";
                public const string REPORT = "ER090000";


                public const string ER_BPJS_CLAIM_DIAGNOSA_PASIEN = "ER060151";
                public const string ER_BPJS_CLAIM_TINDAKAN_PASIEN = "ER060152";
                public const string ER_BPJS_CLAIM_DISPOSISI_PASIEN = "ER060153";
                public const string ER_BPJS_CLAIM_ENTRY = "ER060154";

                public const string ER010491 = "ER010491";
                public const string ER022410 = "ER022410";

                public const string PATIENT_PAGE_CHARM_BAR = "ER900000";
            }
            #endregion

            #region Dashboard
            public static class Dashboard
            {
                public const string RumahSakit1 = "DB010100";
                public const string RumahSakit2 = "DB010200";
                public const string Pasien1 = "DB020100";
                public const string RekamMedis1 = "DB030100";
                public const string Inventory1 = "DB040100";
                public const string Example1 = "DB090100";
                public const string Example2 = "DB090200";
                public const string Example3 = "DB090300";
                public const string Pivot = "DB050000";
                public const string Survey = "DB060000";
                public const string MasterSurvey = "DB070100";
                public const string PIVOT_MCU = "DB050100";
            }
            #endregion

            #region EMR
            public static class EMR
            {
                public const string PATIENT_PAGE_CHARM_BAR = "EM00000";

                public const string PATIENT_LIST = "EM11000";

                public const string PATIENT_IN_MY_AREA = "EM01100";
                public const string MY_TODAY_PATIENT_VISIT_LIST = "EM01200";
                public const string REGISTERED_PATIENT = "EM01300";
                public const string PIVOT_REPORTING = "EM02100";
                public const string INFORMATION_PATIENT_PER_DOCTOR = "EM03100";
                public const string TARIFF_ESTIMATION = "EM03200";
                public const string APPOINTMENT_INFORMATION_TRANSFER_PER_PARAMEDIC_PER_DAY = "EM03300";
                public const string PATIENT_INFORMATION_PHYSICIAN_VISIT = "EM03400";
                public const string INFORMATION_PHYSICIAN = "EM03500";
                public const string INFORMATION_PATIENT_TRANSFER = "EM03600";
                public const string INFORMATION_PHYSICIAN_BALANCE = "EM03700";

                public const string PATIENT_PAGE_EMERGENCY = "EM06000";
                public const string PATIENT_PAGE_EMERGENCY_TOP_BAR = "EM06001";

                public const string PATIENT_PAGE = "EM09000";
                public const string PATIENT_PAGE_LIST = "EM12000";


                public const string PATIENT_DATA = "EM05100";
                public const string PATIENT_MR_VIEW = "EM05101";

                public const string REPORT = "EM04000";

                public const string COMPOUND_TEMPLATE = "EM02100";

                public const string INFORMATION = "PT00000";

                public const string SOAP_TEMPLATE_TRIAGE = "EM09101";


                public const string SOAP_TEMPLATE_EMERGENCY_PROGRESS = "EM09102";
                public const string SOAP_TEMPLATE_EMERGENCY_DISCHARGE = "EM09103";
                public const string SOAP_TEMPLATE_EMERGENCY_PROGRESS_2 = "EM09104";

                public const string SOAP_TEMPLATE_INPATIENT_INITIAL = "EM09130";

                #region EMERGENCY SOAP
                public const string EMERGENCY_SOAP_INITIAL_ASSESSMENT_1 = "EM91100";
                public const string EMERGENCY_SOAP_PROGRESS_ASSESSMENT_1 = "EM91200";
                public const string EMERGENCY_SOAP_DISCHARGE_ASSESSMENT_1 = "EM91300";
                public const string EMERGENCY_SOAP_IP_INITIAL_ASSESSMENT_1 = "EM91400";
                public const string SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_1 = "EM91500";
                public const string EMERGENCY_SOAP_INITIAL_ASSESSMENT_2 = "EM91600"; // Instruksi dengan Free Text (Default Menu)
                public const string EMERGENCY_SOAP_POPULATION_ASSESSMENT = "EM91700";
                public const string SOAP_TEMPLATE_EMERGENCY_PROGRESS_NOTE_SUMMARY = "EM91800";
                #endregion

                #region OUTPATIENT SOAP
                public const string OUTPATIENT_SOAP_GENERAL_ASSESSMENT_1 = "EM92100";
                public const string OUTPATIENT_SOAP_GENERAL_ASSESSMENT_2 = "EM92200";
                public const string OUTPATIENT_SOAP_DENTIST_ASSESSMENT = "EM92101";
                public const string OUTPATIENT_SOAP_PROGRESS_NOTE_1 = "EM92300";
                public const string OUTPATIENT_SOAP_ONCOLOGIST_ASSESSMENT = "EM92400";
                public const string OUTPATIENT_SOAP_POPULATION_ASSESSMENT = "EM92500";
                public const string OUTPATIENT_SOAP_PROGRESS_NOTE_SUMMARY = "EM92600";
                public const string OUTPATIENT_SOAP_OBGYN_ASSESSMENT = "EM92700";
                #endregion

                #region INPATIENT SOAP
                public const string SOAP_TEMPLATE_INPATIENT_INITIAL_ASSESSMENT_1 = "EM93100";
                public const string SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_1 = "EM93200";
                public const string SOAP_TEMPLATE_INPATIENT_DISCHARGE_ASSESSMENT_1 = "EM93100";
                public const string SOAP_TEMPLATE_INPATIENT_INITIAL_ASSESSMENT_3 = "EM93400";
                public const string SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_2 = "EM93500";
                public const string SOAP_TEMPLATE_INPATIENT_INITIAL_ASSESSMENT_2 = "EM93600";
                public const string SOAP_TEMPLATE_INPATIENT_POPULATION_ASSESSMENT = "EM93700";
                public const string SOAP_TEMPLATE_INPATIENT_PROGRESS_NOTE_SUMMARY = "EM93800";
                #endregion

                #region DIAGNOSTIC SOAP
                public const string SOAP_TEMPLATE_DIAGNOSTIC_GENERAL_ASSESSMENT_1 = "EM96100";
                public const string SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE = "EM96200";
                public const string SOAP_TEMPLATE_DIAGNOSTIC_INTEGRATION_NOTE_SUMMARY = "EM96300";
                public const string SOAP_TEMPLATE_DIAGNOSTIC_GENERAL = "EM09618";
                public const string SOAP_TEMPLATE_DIAGNOSTIC_2 = "EM09619";
                #endregion

                #region MCU SOAP
                public const string MCU_SOAP_GENERAL_ASSESSMENT_1 = "EM95100";
                public const string MCU_SOAP_PROGRESS_NOTE_1 = "EM95200";
                public const string MCU_SOAP_PROGRESS_NOTE_SUMMARY = "EM95300";
                #endregion

                #region KAMAR OPERASI
                public const string ORDER_JADWAL_KAMAR_OPERASI = "EM94401";
                public const string FORM_PENGKAJIAN_KAMAR_OPERASI = "EM94402";
                public const string ASESMEN_PRA_BEDAH = "EM94403";
                public const string PENGKAJIAN_ANESTESI = "EM94404";
                #endregion

                #region INFORMATION
                public const string INFORMASI_LAPORAN_OPERASI = "EM94501";
                #endregion

                public const string SOAP_TEMPLATE_OUTPATIENT_GENERAL = "EM09201";
                public const string SOAP_TEMPLATE_OUTPATIENT_2 = "EM09202";

                public const string CPOE_PRESCRIPTION = "EM94300";

                public const string HEALTH_RECORD_ALLERGIES = "EM09101";
                public const string HEALTH_RECORD_CURRENT_MEDICATION = "EM09102";
                public const string HEALTH_RECORD_BODY_DIAGRAM = "EM09103";
                public const string MEDICAL_HISTORY = "EM09104";
                public const string MEDICAL_RECORD_SUMMARY = "EM09105";
                public const string HEALTH_RECORD_VACCINATION_SHOT = "EM09106";

                public const string SOAP_TEMPLATE_INPATIENT_SOAP = "EM09131";

                public const string CDC_GROWTH_CHART = "EM09201";
                public const string MEDICAL_CHART = "EM09202";

                public const string TRIAGE_ASSESSMENT = "EM06101";
                public const string TRIAGE = "EM09301";
                public const string CHIEF_COMPLAINT = "EM09302";
                public const string ANTENATAL_RECORD = "EM94201";
                public const string OBSTETRIC_HISTORY = "EM94203";
                public const string ALLERGIES = "EM09304";
                public const string CURRENT_MEDICATION = "EM09305";
                public const string GENERAL_ASSESMENT = "EM09306";
                public const string PAST_MEDICAL = "EM09307";
                public const string MEDICAL_ASSESSMENT_FORM = "EM09308";
                public const string PAIN_ASSESSMENT_FORM = "EM09309";

                public const string VITAL_SIGN_EMERGENCY = "EM06201";
                public const string VITAL_SIGN = "EM09401";

                public const string REVIEW_OF_SYSTEM_EMERGENCY = "EM06202";
                public const string REVIEW_OF_SYSTEM = "EM09402";

                public const string FETAL_MEASUREMENT = "EM94202";
                public const string BODY_DIAGRAM = "EM09404";
                public const string LABORATORY_TEST_RESULT = "EM09405";
                public const string IMAGING_TEST_RESULT = "EM09406";
                public const string PATIENT_DOCUMENT = "EM09901";
                public const string UNVERIFY_NOTES = "EM09902";
                public const string PSYCHIATRY_STATUS = "EM94101";
                public const string IMAGING_TEST_RESULT_1 = "EM09409";

                public const string DIFFERENTIAL_DIAGNOSIS = "EM09501";
                public const string DIAGNOSIS = "EM09501";
                public const string DENTAL_CHART = "EM09502";

                public const string TEST_ORDER = "EM09601";
                public const string MEDICATION_LIST_SUMMARY = "EM09602";
                public const string VACCINATION_SHOT = "EM09603";
                public const string PRESCRIPTION_ORDER = "EM09604";
                public const string PHYSICIAN_TEAM = "EM09605";
                public const string ONLINE_PRESCRIPTION = "EM09606";
                public const string PATIENT_REFERRAL = "EM09607";
                public const string PATIENT_REFERRAL_RESPONSE = "EM09608";
                public const string PHYSICIAN_TRANSFER = "EM09609";
                public const string TREATMENT_PROCEDURE = "EM09610";
                public const string PATIENT_INSTRUCTION = "EM09611";
                public const string PROGRESS_NOTES = "EM09612";
                public const string INTEGRATION_NOTES = "EM09613";
                public const string NEED_CONFIRMATION_NOTES = "EM09614";
                public const string PHYSICIAN_NOTES = "EM09615";
                public const string PHYSICIAN_NOTES_1 = "EM09616";
                public const string PATIENT_PAGE_PATIENT_EDUCATION = "EM09617";
                public const string PATIENT_PROCEDURES = "EM09620";
                public const string PATIENT_CONSENT_FORM = "EM09621";
                public const string PLANNING_NOTES = "EM09699";

                public const string PATIENT_CHARGES = "EM09701";
                public const string PATIENT_CHARGES_ENTRY = "EM09702";
                public const string PATIENT_DIAGNOSTIC_CHARGES_ENTRY = "EM09703";
                public const string EPISODE_SUMMARY = "EM09801";
                public const string MEDICATION_SUMMARY = "EM09802";
                public const string PATIENT_DISCHARGE = "EM09803";
                public const string FOLLOW_UP_VISIT = "EM09804";
                public const string PATIENT_TRANSFER = "EM09805";
                public const string MEDICAL_RESUME = "EM09806";

                public const string PRINT_TOOLS = "EM09910";

                public const string PATIENT_EMR_VIEW = "EM20000";
                public const string PATIENT_EMR_VISIT = "EM10100";
                public const string VISIT_INFORMATION = "EM10101";

                public const string MASTER_PHYSICIAN_SOAP_TEMPLATE = "EM30100";
                public const string MASTER_PHYSICIAN_PRESCRIPTION_TEMPLATE = "EM30200";
                public const string MASTER_PHYSICIAN_INSTRUCTION_TEMPLATE = "EM30300";

                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_CT_SIMULATION = "EM94601";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM = "EM94602";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM_BRACHYTHERAPY = "EM94603";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_REPORT = "EM94604";

                public const string EM09623 = "EM09623";
            }
            #endregion

            #region Finance
            public static class Finance
            {
                public const string ITEM_GROUP = "FN010100";

                public const string ITEM_SERVICE_FN = "FN010201";
                public const string ITEM_SERVICE_AIO_FN = "FN010202";

                public const string BILL_GROUP = "FN010300";

                public const string CUSTOMER = "FN010401";
                public const string CUSTOMER_CONTRACT = "FN010402";
                public const string SUPPLIER = "FN010403";
                public const string CUSTOMER_GROUP = "FN010404";
                public const string CUSTOMERS_DIAGNOSE = "FN010405";
                //public const string VIRTUAL_ACCOUNT_CUSTOMER = "FN010406";
                public const string SUPPLIER_FINANCE = "FN010406";
                public const string TEST_PARTNER = "FN010407";

                public const string REVENUE_SHARING_FORMULA = "FN010501";
                public const string PARAMEDIC_REVENUE_SHARING = "FN010502";
                public const string ITEM_MASTER_REVENUE_SHARING = "FN010503";
                public const string MAPPING_MASTER_REVENUE_SHARING = "FN010504";

                public const string COVERAGE_TYPE = "FN010600";
                public const string TEMPLATE_CHARGES = "FN010700";
                public const string TERM = "FN010800";
                public const string BANK = "FN010900";
                public const string EDC_MACHINE = "FN011000";
                public const string CREDIT_CARD_FEE = "FN011100";
                public const string MARKUP_MARGIN = "FN011200";

                public const string TAX_REVENUE_RANGE = "FN011301";
                public const string TAX_REVENUE_SETTING = "FN011302";

                public const string PROMOTION_SCHEME = "FN011400";
                public const string TEMPLATE_PANEL_SERVICES = "FN011600";
                public const string COVERAGE_TYPE_UPLOAD = "FN011700";
                public const string TRANSACTION_NON_OPERATIONAL_TYPE = "FN011800";

                public const string CREATE_TARIFF_BOOK = "FN020100";
                public const string PROPOSE_TARIFF_BOOK = "FN020200";
                public const string TARIFF_APPROVAL = "FN020300";
                public const string PROCESS_TARIFF_BOOK = "FN020400";
                public const string EDIT_TARIFF_BOOK = "FN020500";
                public const string DOWNLOAD_UPLOAD_TARIFF = "FN020600";

                public const string FN_TREASURY_TRANSACTION = "FN030100";
                public const string FN_TREASURY_TRANSACTION_CURRENT_DATE = "FN030101";
                public const string FN_REQUEST_REALIZATION_CASH_RECEIPT = "FN030200";
                public const string FN_REQUEST_REALIZATION_CASH_RECEIPT_CURRENT_DATE = "FN030201";
                public const string FN_COMBINE_PATIENT_BILLING = "FN030300";

                public const string AR_INVOICE_PAYER = "FN040110";
                public const string AR_INVOICE_PAYER_PROCESS = "FN040111";
                public const string AR_INVOICE_PAYER_EDIT = "FN040112";
                public const string AR_INVOICE_PAYER_RECEIVE = "FN040113";
                public const string AR_PAYER_RECEIVE_ALOCATION = "FN040114";
                public const string AR_INVOICE_PAYER_APPROVAL = "FN040115";
                public const string AR_INVOICE_PAYER_TRANSACTION_NON_OPERATIONAL = "FN040116";
                public const string AR_PAYER_RECEIVE_ALOCATION_PER_DETAIL = "FN040117";
                public const string AR_INVOICE_PAYER_ADJUSTMENT = "FN040118";

                public const string AR_INVOICE_PATIENT = "FN040120";
                public const string AR_INVOICE_PATIENT_PROCESS = "FN040121";
                public const string AR_INVOICE_PATIENT_EDIT = "FN040122";
                public const string AR_INVOICE_PATIENT_RECEIVE = "FN040123";
                public const string AR_PATIENT_RECEIVE_ALOCATION = "FN040124";
                public const string AR_INVOICE_PATIENT_APPROVAL = "FN040125";
                public const string AR_INVOICE_PATIENT_ADJUSTMENT = "FN040126";

                public const string AR_RECEIPT_PRINT = "FN040201";

                public const string CHANGE_REGISTRATION_PAYER = "FN040901";
                public const string AR_INFORMATION_PER_INVOICE = "FN040902";

                public const string PARAMEDIC_LIST = "FN050100";
                public const string REVENUE_SHARING_PROCESS = "FN050101";
                public const string REVENUE_SHARING_EDIT = "FN050102";
                public const string REVENUE_SHARING_ADJUSTMENT = "FN050103";
                public const string REVENUE_SHARING_VERIFICATION = "FN050104";
                public const string REVENUE_SHARING_SUMMARY_ENTRY = "FN050105";
                public const string REVENUE_SHARING_SUMMARY_ADJUSTMENT = "FN050106";
                public const string REVENUE_SHARING_SUMMARY_APPROVAL = "FN050107";
                public const string REVENUE_SHARING_CHARGES_EDIT_REVENUE = "FN050108";
                public const string REVENUE_SHARING_SUMMARY_REOPEN = "FN050109";
                public const string REVENUE_SHARING_ADJUSTMENT_TRANSACTION = "FN050110";

                public const string SUPPLIER_LIST = "FN050200";
                public const string AP_INVOICE_SUPPLIER_PROCESS = "FN050201";
                public const string AP_INVOICE_SUPPLIER_VERIFICATION = "FN050202";
                public const string AP_INVOICE_SUPPLIER_VERIFICATION_TO_PAYMENT = "FN050203";
                public const string AP_INVOICE_SUPPLIER_PAYMENT_VERIFICATION = "FN050204";

                public const string REVENUE_SHARING_EDIT_ENTRY = "FN050300"; // sudah dihilangkan dari master menu (RN patch 202206-05)

                public const string REVENUE_SHARING_DOWNLOAD_UPLOAD_PROCESS = "FN050301";
                public const string REVENUE_SHARING_ADJUSTMENT_DOWNLOAD_UPLOAD_PROCESS = "FN050302";

                public const string AP_SUPPLIER_VERIFICATION = "FN050401";
                public const string AP_SUPPLIER_PAYMENT = "FN050402";
                public const string AP_SUPPLIER_VERIFICATION_APPROVAL = "FN050403";
                public const string AP_SUPPLIER_VERIFICATION_APPROVAL_VOID = "FN050404";

                public const string REVENUE_SHARING_PAYMENT_VERIFICATION = "FN050501";
                public const string REVENUE_SHARING_PAYMENT = "FN050502";
                public const string REVENUE_SHARING_PAYMENT_RECAPITULATION = "FN050503";
                public const string REVENUE_SHARING_FEE_PROCESS = "FN050504";
                public const string TRANS_REVENUE_SHARING_SUMMARY_DOWNLOAD = "FN050505";

                public const string TEST_PARTNER_TRANSACTION = "FN050600";
                public const string AP_INFORMATION_PER_INVOICE_SUPPLIER = "FN050700";
                public const string REVENUE_SHARING_PER_REGISTRATION = "FN050800";

                public const string BPJS_PROCESS = "FN060000";
                public const string BPJS_MASTER = "FN060100";
                public const string BPJS_MASTER_EKLAIM_PARAMETER = "FN060101";
                public const string BPJS_MASTER_DIAGNOSE_EKLAIM = "FN060102";
                public const string BPJS_MASTER_PROCEDURE_EKLAIM = "FN060103";
                public const string BPJS_MASTER_INACBGs_GROUPER = "FN060104";
                public const string BPJS_MASTER_DIAGNOSE_INA_EKLAIM = "FN060105";
                public const string BPJS_MASTER_PROCEDURE_INA_EKLAIM = "FN060106";
                public const string BPJS_INACBGs_MASTER = "FN060107";

                public const string BPJS_EKLAIM = "FN060200";
                public const string BPJS_EKLAIM_PATIENT = "FN060201";
                public const string BPJS_EKLAIM_ENTRY = "FN060202";

                public const string BPJS_EKLAIM_SEND_ONLINE = "FN060300";
                public const string BPJS_EKLAIM_SEND_ONLINE_INDIVIDUAL = "FN060301";
                public const string BPJS_EKLAIM_SEND_ONLINE_COLLECTIVE = "FN060302";

                public const string PROCESS_CODING_CLAIM = "FN060400";
                public const string DIAGNOSE_PROCEDURE_CLAIM = "FN060401";

                public const string BPJS_TEMPORARY_CLAIM = "FN060500";
                public const string BPJS_TEMPORARY_CLAIM_OUTPATIENT = "FN060501";
                public const string BPJS_TEMPORARY_CLAIM_INPATIENT = "FN060502";

                public const string BPJS_AR_CLAIM = "FN060601";
                public const string BPJS_AR_FINAL_CLAIM_V1 = "FN060602";
                public const string BPJS_AR_FINAL_CLAIM_V2 = "FN060603";
                public const string BPJS_UPDATE_INA_AMOUNT = "FN060604";
                public const string BPJS_AR_FINAL_CLAIM_V2_NEW = "FN060605";
                public const string BPJS_AR_FINAL_CLAIM_V2_APPROVAL = "FN060606";

                public const string CUSTOMER_CONTRACT_STATUS = "FN070100";
                public const string TARIFF_SERVICE = "FN070200";
                public const string DRUGS_LOGISTICS_PRICE = "FN070300";
                public const string TARIFF_ESTIMATION = "FN070400";
                public const string BALANCE_ACCOUNT_DETAIL = "FN070500";
                public const string PURCHASE_INVOICE_INFORMATION = "FN070600";
                public const string HISTORY_PEMBATALAN_TRANSAKSI = "FN070700";
                public const string CUSTOMER_CONTRACT_INFORMATION = "FN070800";
                public const string HISTORY_PEMBATALAN_TRANSAKSI_DETAIL = "FN070701";
                public const string INFORMASI_UMUR_PIUTANG = "FN070901";
                public const string INFORMASI_PEMBAYARAN_HUTANG = "FN070902";
                public const string INFORMASI_PEMBAYARAN_HUTANG_PER_DETAIL_INVOICE = "FN070903";
                public const string INFORMASI_HISTORY_WALLET_PASIEN = "FN071100";
                public const string INFORMASI_TRANSAKSI_VS_PEMBAYARAN = "FN071200";
                public const string INFORMASI_PIUTANG = "FN071300";
                public const string INFORMASI_REGISTRASI_PROPOSIONAL = "FN071400";
                public const string INFORMASI_HISTORY_BILLING_PATIENT = "FN071500";
                public const string INFORMASI_DETAIL_PIUTANG = "FN071600";

                public const string FN_UTILITY_EXPORT_PATIENT_PAYMENT = "FN080100";
                public const string FN_UTILITY_EDIT_TARIFF = "FN080200";
                public const string FN_REOPEN_BILLING = "FN080300";
                public const string FN_REOPEN_BILLING_PROCESS = "FN080301";
                public const string FN_REOPEN_BILLING_INFORMATION = "FN080302";
                public const string FN_CHANGE_PAYMENT = "FN080401";
                public const string FN_CHANGE_PAYMENT_DATE = "FN080402";
                public const string FN_COLLECTIVE_PATIENT_RECEIVE = "FN080500";
                public const string FN_PRINT_ULANG_HASIL_LAB = "FN080601";

                public const string REPORT = "FN090000";

                public const string INHEALTH_SIMPAN_TINDAKAN = "FN100100";
                public const string INHEALTH_SIMPAN_TINDAKAN_RAWAT_JALAN = "FN100101";
                public const string INHEALTH_SIMPAN_TINDAKAN_RAWAT_INAP = "FN100102";

                public const string INHEALTH_HAPUS_TINDAKAN = "FN100200";


                public const string PATIENT_PAGE_CHARM_BAR = "FN990000";

            }
            #endregion

            #region Imaging
            public static class Imaging
            {
                public const string APPOINTMENT_INFORMATION = "IS080150";
                public const string IS_PROCESS_BRIDING_MASPION = "IS070304";
                public const string ITEM_GROUP = "IS010100";

                public const string ITEM_SERVICE_IS = "IS010201";
                public const string ITEM_SERVICE_AIO_IS = "IS010202";

                public const string TEMPLATE_GROUP = "IS010300";
                public const string MODALITY = "IS010400";
                public const string PANEL_RADIOLOGY = "IS010500";
                public const string TEMPLATE_PANEL_IMAGING = "IS010500";
                public const string TEMPLATE_CHARGES_RADIOLOGY = "IS010600";
                public const string TEST_PARTNER = "IS010700";

                public const string REGISTRATION = "IS020200";
                public const string MEDICAL_FOLDER_REQUEST = "IS020300";
                public const string GENERATE_ORDER_AIO = "IS020400";
                public const string CONTROL_ORDER_AIO = "IS020500";

                public const string WORK_LIST = "IS030100";
                public const string IMAGING_RESULT = "IS030200";
                public const string IMAGING_RESULT_VERIFICATION = "IS030300";
                public const string IMAGING_PATIENT_MR_LIST = "IS030101";
                public const string PATIENT_TRANSACTION_PAGE = "IS030400";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "IS030401";
                public const string PATIENT_TRANSACTION_TEST_ORDER = "IS030402";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "IS030403";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER = "IS030404";
                public const string PATIENT_TRANSACTION_OUTPATIENT_ORDER = "IS030405";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "IS030406";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "IS030407";
                public const string TEST_ORDER_RESULT_LIST = "IS030408";
                public const string PATIENT_OUTSTANDING_ORDER_LIST = "IS030409";
                public const string PATIENT_USE = "IS030410";
                public const string PATIENT_ORDER_LIST = "IS030411";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "IS030412";

                public const string WORK_LIST_AIO = "IS030600";

                public const string PATIENT_PAGE = "IS030101";
                public const string PATIENT_PAGE_INTEGRATED_NOTES = "IS033101";
                public const string PATIENT_PAGE_PATIENT_DIAGNOSIS = "IS033102";
                public const string PATIENT_PAGE_LABORATORY = "IS033103";
                public const string PATIENT_PAGE_E_DOCUMENT = "IS033104";
                public const string PATIENT_NURSING_NOTES = "IS033105";
                public const string PATIENT_PAGE_IMAGING_RESULT = "IS033106";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "IS033107";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "IS033108";

                public const string PARAMEDIC_SCHEDULE = "IS040100";
                public const string APPOINTMENT = "IS040200";
                public const string APPOINTMENT_2 = "IS040400";
                public const string GENERATE_APPOINTMENT = "IS040300";

                public const string BILL_SUMMARY = "IS060100";
                public const string BILL_SUMMARY_CHARGES = "IS060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "IS060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "IS060113";
                public const string BILL_SUMMARY_LABORATORY = "IS060114";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "IS060115";
                public const string BILL_SUMMARY_PHARMACY = "IS060116";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "IS060117";
                public const string BILL_SUMMARY_GENERATE_BILL = "IS060121";
                public const string BILL_SUMMARY_DISCOUNT = "IS060122";
                public const string BILL_SUMMARY_PAYMENT = "IS060123";
                public const string BILL_SUMMARY_DETAIL = "IS060124";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "IS060125";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "IS060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "IS060127";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "IS060128";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "IS060129";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "IS060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "IS060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "IS060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "IS060134";
                public const string BILL_SUMMARY_VOID_BILL = "IS060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "IS060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "IS060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "IS060144";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "IS060145";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "IS060146";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "IS060147";
                public const string BILL_SUMMARY_CLOSE_BILLING = "IS060148";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "IS060148";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "IS060149";

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "IS033210";
                public const string NURSING_ASSESSMENT_PROCESS = "IS033220";
                #endregion

                public const string E_DOCUMENT = "IS060161";

                public const string BPJS_PROCESS_CLAIM_ORDER = "IS060155";
                public const string AIO_ALOCATION_TRANSACTION_ENTRY = "IS060156";
                public const string AIO_ALOCATION_TRANSACTION_CONTROL = "IS060157";
                public const string AIO_BILL_SUMMARY_RECALCULATION = "IS060158";

                public const string BILL_INFORMATION = "IS060191";
                public const string PENDING_TRANSACTION = "IS060192";
                public const string PAYMENT_TRANSACTION = "IS060193";
                //public const string CHARGES_TRANSACTION = "IS060194";
                public const string INFORMATION_CUSTOMER_PAYER_IMAGING = "IS060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_IMAGING = "IS060196";
                public const string INFORMATION_VISIT_HISTORY_IMAGING = "IS060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "IS060198";
                public const string INFORMATION_DISCOUNT_DETAIL = "IS060199";

                public const string PATIENT_BILL_DETAIL_REPRINT = "IS070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "IS070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "IS070121";
                public const string PAYMENT_RECEIPT_REPRINT = "IS070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "IS070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "IS070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "IS070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "IS070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "IS070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "IS070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "IS070240";

                public const string CLOSE_REGISTRATION = "IS070301";
                public const string VOID_REGISTRATION_DAILY = "IS070302";
                public const string VOID_REGISTRATION = "IS070303";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "IS070306";

                public const string RESEND_ORDER = "IS070305";

                public const string IMAGING_RESULT_HISTORY = "IS070410";
                public const string IMAGING_RESULT_HISTORY_DETAIL = "IS070411";
                public const string BRIDGING_STATUS = "IS070420";
                public const string IMAGING_RESULT_TEMPORARY_PHYSICIAN = "IS070500";
                public const string TARIFF_TEST_ESTIMATION = "IS070600";

                public const string RESULT_DELIVERY_TO_PATIENT = "IS070701";
                public const string RESULT_DELIVERY_INTERNAL = "IS070702";

                public const string INFORMATION = "IS080110";
                public const string PRINTING_PATIENT_CARD = "IS080120";
                public const string HISTORY_INFORMATION = "IS080130";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "IS080140";
                public const string CORRECTION_TRANSACTION_INFORMATION = "IS080210";
                public const string PAYMENT_INFORMATION = "IS080220";
                public const string HISTORY_PATIENT_INFORMATION = "IS080211";
                public const string TARIFF_ESTIMATION = "IS080310";
                public const string PIVOT_PATIENT_CHARGES = "IS080320";
                public const string CUSTOMER_CONTRACT_INFORMATION = "IS080330";
                public const string TARIFF_SERVICE_INFORMATION = "IS080340";
                public const string IS_DATA_KUNJUNGAN_PASIEN_BPJS = "IS080411";
                public const string DRUGS_LOGISTICS_PRICE = "IS080350";

                public const string REPORT = "IS090000";

                public const string IS_BPJS_CLAIM_DIAGNOSA_PASIEN = "IS060151";
                public const string IS_BPJS_CLAIM_TINDAKAN_PASIEN = "IS060152";
                public const string IS_BPJS_CLAIM_DISPOSISI_PASIEN = "IS060153";
                public const string IS_BPJS_CLAIM_ENTRY = "IS060154";

                public const string PATIENT_PAGE_CHARM_BAR = "IS900000";
            }
            #endregion

            #region Inpatient
            public static class Inpatient
            {
                public const string IP_PROCESS_BRIDING_MASPION = "IP070309";
                public const string IP_ESTIMATED_CHARGES = "IP070310";
                public const string REGISTRATION = "IP010200";
                public const string MEDICAL_FOLDER_REQUEST = "IP010300";

                public const string FOLLOWUP_PATIENT_DISCHARGE = "IP010400";
                public const string FOLLOWUP_MEDICAL_ASSESSMENT_FORM = "IP010411";
                public const string FOLLOWUP_FALL_RISK_ASSESSMENT_FORM = "IP010412";
                public const string FOLLOWUP_NURSE_INITIAL_ASSESSMENT = "IP010413";
                public const string FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "IP010414";
                public const string FOLLOWUP_PAIN_ASSESSMENT_FORM = "IP010415";
                public const string FOLLOWUP_INITIAL_ASSESSMENT = "IP010416";
                public const string FOLLOWUP_POPULATION_ASSESSMENT = "IP010417";
                public const string FOLLOWUP_NUTRITION_SCREENING = "IP010418";
                public const string FOLLOWUP_MST_FORM = "IP010419";
                public const string FOLLOWUP_EWS_ASSESSMENT_FORM = "IP031120";

                public const string FOLLOWUP_ALLERGY_INPATIENT = "IP010421";
                public const string FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY = "IP010422";
                public const string FOLLOWUP_PATIENT_PAGE_VACCINATION = "IP010423";
                public const string FOLLOWUP_ANTENATAL_RECORD = "IP010424";
                public const string FOLLOWUP_ADULT_SCREENING_ASSESSMENT = "IP010425";

                public const string FOLLOWUP_PATIENT_PAGE_VITAL_SIGN = "IP010431";
                public const string FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM = "IP010432";

                public const string FOLLOWUP_NURSING_ASSESSMENT_PROBLEM = "IP010441";
                public const string FOLLOWUP_NURSING_ASSESSMENT_PROCESS = "IP010442";
                public const string FOLLOWUP_TRANSACTION_PAGE_PATIENT_DIAGNOSIS = "IP010443";
                public const string FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION = "IP010444";

                public const string FOLLOWUP_PATIENT_PAGE_NURSING_NOTE = "IP010451";
                public const string FOLLOWUP_INTEGRATED_NOTE = "IP010452";
                public const string FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "IP010453";
                public const string FOLLOWUP_NURSING_JOURNAL = "IP010454";
                public const string FOLLOWUP_MEDICAL_RESUME = "IP010455";
                public const string FOLLOWUP_NURSE_MEDICAL_RESUME = "IP010456";
                public const string FOLLOWUP_PHYSICIAN_NOTE = "IP010457";
                public const string FOLLOWUP_INTEGRATION_NOTES_INPATIENT_NUTRITIONIST = "IP010458";
                public const string FOLLOWUP_NUTRITION_NOTES = "IP010459";

                public const string FOLLOWUP_PATIENT_MEDICATION_SUMMARY_WITH_PROCESS = "IP010461";
                public const string FOLLOWUP_MEDICATION_LIST_NON_IV = "IP010461"; // obsolete menu
                public const string FOLLOWUP_VENTILATOR_MONITORING = "IP010462";
                public const string FOLLOWUP_TEST_ORDER_RESULT_LIST = "IP010463";
                public const string FOLLOWUP_MEDICATION_LIST_PRN = "IP010464";
                public const string FOLLOWUP_MONITORING_INTAKE_OUTPUT = "IP010465";

                public const string FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT = "IP010471";
                public const string FOLLOWUP_PHYSICIAN_VISIT_LIST = "IP010472";
                public const string FOLLOWUP_INFORMATION_PATIENT_USE1 = "IP010473";
                public const string FOLLOWUP_PATIENT_REFERRAL = "IP010474";
                public const string FOLLOWUP_NURSE_HANDS_OVER = "IP010475";
                public const string FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION = "IP010476";
                public const string FOLLOWUP_PAGE_TRANSFER_PHYISICIAN = "IP010477";
                public const string FOLLOWUP_PAGE_TESTORDER_INFORMATION = "IP010478";

                public const string FOLLOWUP_PATIENT_PAGE_E_DOCUMENT = "IP010481";
                public const string FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT = "IP010482";

                public const string FOLLOWUP_PATIENT_PAGE_FORM_PENGKAJIAN_HEMODIALISA = "IP010491";
                public const string FOLLOWUP_PATIENT_PAGE_PERESEPAN_HEMODIALISA = "IP010492";
                public const string FOLLOWUP_PATIENT_PAGE_PEMANTAUAN_INTRA_HD = "IP010493";
                public const string FOLLOWUP_PATIENT_PAGE_POST_HD = "IP010494";

                public const string MEDICAL_RECORD = "IP010500";
                public const string MEDICAL_RECORD_VIEW = "IP010501";
                public const string MEDICAL_RECORD_VISIT = "IP010502";
                public const string MEDICAL_RECORD_VISIT_LIST = "IP010503";

                public const string NURSE_NOTES = "IP010600";


                public const string PATIENT_REGISTRATION_CONFIRMATION = "IP020100";
                public const string PATIENT_TRANSFER = "IP020200";
                public const string PATIENT_DISCHARGE_PLAN = "IP020300";
                public const string PATIENT_DISCHARGE = "IP020400";
                public const string CHANGE_BED_STATUS = "IP020500";
                public const string BED_RESERVATION = "IP020600";
                public const string PROCESS_BED_RESERVATION_LIST = "IP020700";


                public const string PATIENT_TRANSACTION_PAGE = "IP031000";

                public const string NURSE_INITIAL_ASSESSMENT = "IP031101";
                public const string FOLLOWUP_PATIENT_PAGE_ALLERGY = "IP031103";
                public const string TRANSACTION_PAGE_PATIENT_DIAGNOSIS = "IP031104";
                public const string PATIENT_PAGE_PATIENT_EDUCATION = "IP031107";

                #region Pengkajian Pasien
                public const string MEDICAL_ASSESSMENT_FORM = "IP031109";
                public const string FALL_RISK_ASSESSMENT_FORM = "IP031110";
                public const string PAIN_ASSESSMENT_FORM = "IP031113";
                public const string MST_FORM = "IP031111";
                public const string POPULATION_ASSESSMENT = "IP031112";
                public const string INITIAL_ASSESSMENT = "IP031114";
                public const string NUTRITION_SCREENING = "IP031117";
                public const string EWS_ASSESSMENT = "IP031118";
                public const string NUTRITION_SCREENING_VERIFY = "IP031119";
                #endregion

                #region Pengkajian Riwayat Kesehatan Pasien
                public const string ALLERGY_INPATIENT = "IP030116";
                public const string ANTENATAL_RECORD = "IP031115";
                public const string PATIENT_PAGE_VACCINATION = "IP031116";
                #endregion

                #region Pemeriksaan Fisik
                public const string VITAL_SIGN_TRANSACTION = "IP031102";
                public const string PATIENT_PAGE_REVIEW_OF_SYSTEM = "IP031105";
                #endregion

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "IP031031";
                public const string NURSING_ASSESSMENT_PROCESS = "IP031032";
                public const string NURSING_ASSESSMENT_TRANSFER = "IP031033";
                #endregion

                #region Catatan Terintegrasi
                public const string PATIENT_PAGE_NURSING_NOTE = "IP031201";
                public const string PATIENT_PAGE_INTEGRATION_NOTES = "IP031202";
                public const string PATIENT_PAGE_INTEGRATION_NOTES_INPATIENT_NUTRITIONIST = "IP031203";
                public const string PATIENT_PAGE_INTEGRATED_NOTE_DISPLAY_BY_DATE = "IP031204";
                public const string PATIENT_PAGE_NUTRITION_NOTES = "IP031205";
                public const string PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "IP031206";
                public const string PATIENT_PAGE_NURSING_JOURNAL = "IP031207";
                public const string PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "IP031208";
                public const string PATIENT_PAGE_PATIENT_REFERRAL = "IP031209";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "IP031210";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "IP031211";
                public const string MEDICAL_RESUME = "IP031212";
                public const string NURSE_MEDICAL_RESUME = "IP031213";
                #endregion

                #region Proses dan Utilitas
                public const string PATIENT_PAGE_E_DOCUMENT = "IP031106";
                public const string PATIENT_PAGE_CHANGE_ORDER_STATUS = "IP031901";
                public const string BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU = "IP031902";
                public const string PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT = "IP031903";
                public const string IP031904 = "IP031904";
                #endregion

                #region Transaksi dan Order Pelayanan
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "IP031301";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES_USE = "IP031302";
                public const string PATIENT_TRANSACTION_PAGE_PATIENT_ACCOMPANY = "IP031303";
                public const string PATIENT_TRANSACTION_PAGE_PATIENT_ACCOMPANY_USE = "IP031304";
                public const string PATIENT_TRANSACTION_PAGE_VERIFICATION = "IP031305";
                public const string PATIENT_TRANSACTION_PAGE_INFORMATION = "IP031306";
                public const string PATIENT_TRANSACTION_PAGE_PROCEDURES = "IP031307";
                public const string PATIENT_PAGE_TRANSFER_PHYISICIAN = "IP031308";

                public const string PATIENT_TRANSACTION_TEST_ORDER = "IP031401";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "IP031402";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "IP031403";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "IP031404";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER = "IP031405";
                public const string PATIENT_TRANSACTION_OUTPATIENT_ORDER = "IP031406";
                public const string PATIENT_TRANSACTION_OUTSTANDING_ORDER_LIST = "IP031407";
                public const string PATIENT_TRANSACTION_ORDER_LIST = "IP031408";
                public const string PATIENT_TRANSACTION_NUTRITION_ORDER = "IP031409";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "IP031410";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES = "IP031411";
                public const string PATIENT_TRANSACTION_BLOOD_BANK_ORDER = "IP031412";
                #endregion

                #region Monitoring dan Evaluasi
                public const string MEDICATION_CHART_LIST = "IP031501";
                public const string MEDICATION_LIST_PRN = "IP031502";
                public const string MEDICATION_LIST = "IP031503";
                public const string MEDICATION_SUMMARY = "IP031504";
                public const string MEDICATION_SUMMARY_WITH_PROCESS = "IP031505";
                public const string VENTILATOR_MONITORING = "IP031506";
                public const string MONITORING_INTAKE_OUTPUT = "IP031507";
                public const string TEST_ORDER_RESULT_LIST = "IP031601";
                #endregion

                public const string TRANSACTION_PAGE_PATIENT_DISPOSITION = "IP031602";

                public const string PATIENT_TRANSACTION_PHYSICIAN_VISIT_LIST = "IP031701";
                public const string INFORMATION_PATIENT_USE1 = "IP031702";
                public const string INFORMATION_PATIENT_SURGERY_REPORT = "IP031703";


                public const string PATIENT_PAGE_OBSTETRIC_HISTORY = "IP031801";

                public const string TRANSACTION_PAGE_PAIN_ASSESSMENT = "IP030141";
                public const string TRANSACTION_PAGE_NURSING_NOTE_2 = "IP030151";

                public const string TRANSACTION_VERIFICATION = "IP030200";
                public const string PATIENT_BIRTH = "IP030300";
                public const string GENERATE_ORDER_AIO = "IP030400";
                public const string CONTROL_ORDER_AIO = "IP030500";

                public const string BILL_SUMMARY = "IP060100";
                public const string BILL_SUMMARY_CHARGES = "IP060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "IP060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "IP060113";
                public const string BILL_SUMMARY_IMAGING = "IP060114";
                public const string BILL_SUMMARY_LABORATORY = "IP060115";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "IP060116";
                public const string BILL_SUMMARY_PHARMACY = "IP060117";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "IP060118";
                public const string BILL_SUMMARY_GENERATE_BILL = "IP060121";
                public const string BILL_SUMMARY_DISCOUNT = "IP060122";
                public const string BILL_SUMMARY_PAYMENT = "IP060123";
                public const string BILL_SUMMARY_DETAIL = "IP060124";
                public const string BILL_SUMMARY_TARIFF_INACBGS = "IP060125";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "IP060126";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "IP060127";
                public const string BILL_SUMMARY_PAYMENT_AR = "IP060128";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "IP060129";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "IP0601210";

                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "IP060144";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "IP060145";
                public const string BILL_SUMMARY_RECEIPT_PRINT = "IP060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "IP060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "IP060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "IP060134";
                public const string BILL_SUMMARY_VOID_BILL = "IP060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "IP060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "IP060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "IP060146";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "IP060147";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "IP060148";
                public const string BILL_SUMMARY_PROCESS_CONTROL_CLASS = "IP060149";
                public const string PROCESS_TRANSFER_DOWN_PAYMENT = "IP060155";
                public const string PROCESS_VOID_TRANSFER_DOWN_PAYMENT = "IP060156";
                public const string BPJS_PROCESS_CLAIM_ORDER = "IP060157";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "IP060158";
                public const string MANUAL_BED_CHARGES = "IP060159";
                public const string BILL_SUMMARY_CLOSE_BILLING = "IP060160";
                public const string AIO_ALOCATION_TRANSACTION_ENTRY = "IP060160";
                public const string ESTIMATED_CHARGES_COPY = "IP060161";
                public const string AIO_ALOCATION_TRANSACTION_CONTROL = "IP060162";
                public const string AIO_BILL_SUMMARY_RECALCULATION = "IP060163";

                public const string E_DOCUMENT = "IP060171";

                public const string BILL_INFORMATION = "IP060191";
                public const string PENDING_TRANSACTION = "IP060192";
                public const string PAYMENT_TRANSACTION = "IP060193";
                public const string PATIENT_MUTATION = "IP060194";
                public const string PATIENT_DIAGNOSE_INPATIENT = "IP060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_INPATIENT = "IP060196";
                public const string INFORMATION_CUSTOMER_PAYER_INPATIENT = "IP060197";
                public const string INFORMATION_VISIT_HISTORY_INPATIENT = "IP060198";
                public const string INFORMATION_PATIENT_REFERRAL = "IP060199";
                public const string INFORMATION_PATIENT_USE = "IP060200";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "IP060201";
                public const string INFORMATION_DISCOUNT_DETAIL = "IP060202";

                public const string PATIENT_BILL_DETAIL_REPRINT = "IP070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "IP070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "IP070121";
                public const string PAYMENT_RECEIPT_REPRINT = "IP070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "IP070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "IP070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "IP070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "IP070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "IP070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "IP070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "IP070240";

                public const string CANCEL_PATIENT_DISCHARGE = "IP070301";
                public const string VOID_REGISTRATION_DAILY = "IP070302";
                public const string CLOSE_REGISTRATION = "IP070303";
                public const string TRANSFER_PATIENT_BILL = "IP070304";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "IP070305";
                public const string VOID_REGISTRATION = "IP070306";
                public const string NUTRITION_REORDER_MEAL_PLAN = "IP070307";
                public const string NUTRITION_ORDER_EDIT = "IP070308";

                public const string REGISTERED_PATIENT_LIST = "IP080110";
                public const string INPATIENT_LIST = "IP080120";
                public const string MEDICAL_RECORD_LIST = "IP080130";
                public const string PRINTING_PATIENT_CARD = "IP080140";
                public const string BED_INFORMATION = "IP080150";
                public const string BED_RESERVATION_INFORMATION = "IP080160";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "IP080170";
                public const string INFO_INPATIENT_TRANSFER_LIST = "IP080180";
                public const string PATIENT_INFORMATION_PHYSICIAN_VISIT = "IP080190";
                public const string TRANSACTION_PATIENT_INFORMATION = "IP080210";
                public const string CORRECTION_TRANSACTION_INFORMATION = "IP080220";
                public const string PAYMENT_INFORMATION = "IP080240";
                public const string TARIFF_ESTIMATION = "IP080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "IP080320";
                public const string TARIFF_SERVICE_INFORMATION = "IP080330";
                public const string DRUGS_LOGISTICS_PRICE = "IP080340";
                public const string HOSPITALIZED_PATIENT_LIST = "IP080400";
                public const string IP_DATA_KUNJUNGAN_PASIEN_BPJS = "IP080411";
                public const string REGISTERED_PATIENT_BPJS_LIST = "IP080420";
                public const string REPORT = "IP090000";

                public const string IP_BPJS_CLAIM_DIAGNOSA_PASIEN = "IP060151";
                public const string IP_BPJS_CLAIM_TINDAKAN_PASIEN = "IP060152";
                public const string IP_BPJS_CLAIM_DISPOSISI_PASIEN = "IP060153";
                public const string IP_BPJS_CLAIM_ENTRY = "IP060154";

                public const string PATIENT_PAGE_CHARM_BAR = "IP900000";
            }
            #endregion

            #region Billing Management
            public static class BillingManagement
            {
                public const string BILL_SUMMARY = "BM060100";
                public const string BILL_SUMMARY_CHARGES = "BM060111";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "BM070100";
            }
            #endregion

            #region Inventory
            public static class Inventory
            {
                public const string ITEM_GROUP = "IM010100";
                public const string ITEM_GROUP_DRUGS = "IM010101";
                public const string ITEM_GROUP_SUPPLIES = "IM010102";
                public const string ITEM_GROUP_LOGISTIC = "IM010103";
                public const string ITEM_GROUP_NUTRITION = "IM010104";
                public const string MANUFACTURER = "IM010201";
                public const string PRODUCT_BRAND = "IM010300";
                public const string DRUGS = "IM010400";
                public const string MEDICAL_SUPPLIES = "IM010500";
                public const string LOGISTICS = "IM010600";
                public const string LOCATION_STATUS = "IM010700";
                public const string LOCATION = "IM010800";
                public const string SUPPLIER = "IM010900";
                public const string ITEM_BOM = "IM011000";
                public const string ITEM_UNIT = "IM011100";
                public const string LOCATION_ITEM = "IM011200";
                public const string BIN_LOCATION = "IM011300";
                public const string FOOD_AND_BEVERAGES = "IM011400";
                public const string PRODUCT_LINE = "IM011500";
                public const string ITEM_PLANNING = "IM011600";

                public const string REORDER_PURCHASE_REQUEST = "IM020101";
                public const string REORDER_PURCHASE_REQUEST2 = "IM020104";
                public const string PURCHASE_REQUEST = "IM020102";
                public const string APPROVED_PURCHASE_REQUEST = "IM020103";
                public const string REORDER_PURCHASE_ORDER = "IM020201";
                public const string PURCHASE_ORDER = "IM020202";
                public const string APPROVED_PURCHASE_ORDER = "IM020203";
                public const string PRINT_PURCHASE_ORDER = "IM020204";
                public const string DIRECT_PURCHASE = "IM020301";
                public const string DIRECT_PURCHASE_RETURN = "IM020302";

                public const string REORDER_ITEM_REQUEST = "IM030101";
                public const string ITEM_REQUEST = "IM030102";
                public const string APPROVED_ITEM_REQUEST = "IM030103";
                public const string PRINT_APPROVED_ITEM_REQUEST = "IM030104";
                public const string REORDER_ITEM_DISTRIBUTION = "IM030201";
                public const string ITEM_DISTRIBUTION = "IM030202";
                public const string PURCHASE_RECEIVE = "IM030301";
                public const string PURCHASE_RETURN = "IM030302";
                public const string CREDIT_NOTE = "IM030303";
                public const string PURCHASE_REPLACEMENT = "IM030304";
                public const string PURCHASE_RECEIVE_V2 = "IM030305";
                public const string ITEM_DISTRIBUTION_CONFIRMED = "IM030203";
                public const string ITEM_ADJUSTMENT = "IM030502";
                public const string ITEM_CONSUMPTION = "IM030503";
                public const string ITEM_PRODUCTION = "IM030504";
                public const string STOCK_TAKING = "IM030505";
                public const string ITEM_ADJUSTMENT_RECEIPTS = "IM030506";
                public const string ITEM_ADJUSTMENT_ISSUES = "IM030507";
                public const string STOCK_TAKING_V2 = "IM030508";
                public const string STOCK_TAKING_V3 = "IM030509";

                public const string CONSIGNMENT_ORDER = "IM040101";
                public const string APPROVED_CONSIGNMENT_ORDER = "IM040102";
                public const string CONSIGNMENT_RECEIVE = "IM040200";
                public const string CONSIGNMENT_RETURN = "IM040300";

                public const string DONATION_RECEIVE = "IM030401";
                public const string DONATION_RETURN = "IM030402";

                public const string ITEM_REQUEST_APPROVAL = "IM050210"; //"IM050101";
                public const string ITEM_DISTRIBUTION_APPROVAL = "IM050220"; //"IM050102";
                public const string ITEM_ADJUSTMENT_APPROVAL = "IM050241"; //"IM050103";
                public const string ITEM_CONSUMPTION_APPROVAL = "IM050242"; //"IM050104";
                public const string STOCK_OPNAME_APPROVAL = "IM050243"; //"IM050104";
                public const string PURCHASE_RECEIVE_VOID_V2 = "IM050244";
                public const string PURCHASE_RECEIVE_CONFIRMED = "IM050231"; //"IM050105";
                public const string PURCHASE_RECEIVE_APPROVAL = "IM050232"; //"IM050106";
                public const string PURCHASE_RETURN_APPROVAL = "IM050234"; //"IM050107";
                public const string PURCHASE_RECEIVE_VOID = "IM050233"; //"IM050108";
                public const string PURCHASE_RETURN_VOID = "IM050235"; //"IM050109";
                public const string CREDIT_NOTE_VOID = "IM050236"; //"IM050109";
                public const string PHYSICAL_COUNT_CHECKED = "IM050237";
                public const string PHYSICAL_COUNT_UNCHECKED = "IM050238";
                public const string RECALCULATE_PURCHASE_RECEIVE = "IM050239";
                public const string PURCHASE_REQUEST_APPROVAL = "IM050110"; //"IM050201";
                public const string PURCHASE_ORDER_APPROVAL = "IM050120"; //"IM050202";
                public const string PURCHASE_ORDER_VOID = "IM050121"; //"IM050202";
                public const string PURCHASE_ORDER_REOPEN = "IM050122"; //"IM050202";
                public const string DIRECT_PURCHASE_APPROVAL = "IM050130";
                public const string DIRECT_PURCHASE_RETURN_APPROVAL = "IM050131";

                public const string DONATION_APPROVAL = "IM050251"; //"IM050202";
                public const string CONSIGNMENT_ORDER_APPROVAL = "IM050310"; //"IM050301";
                public const string CONSIGNMENT_RECEIVE_CONFIRMED = "IM050321"; //"IM050302";
                public const string CONSIGNMENT_RECEIVE_APPROVAL = "IM050322"; //"IM050303";
                public const string CONSIGNMENT_RETURN_APPROVAL = "IM050330"; //"IM050304";
                public const string CONSIGNMENT_ORDER_REOPEN = "IM050340";
                public const string RECALCULATE_MIN_MAX = "IM050400";
                public const string PIVOT_DRUGS_ANALYSIS = "IM060100";
                public const string ITEM_BALANCE_INFORMATION_PER_LOCATION = "IM060201";
                public const string ITEM_BALANCE_INFORMATION_PER_ITEM = "IM060202";
                public const string DRUGS_PRICE = "IM060200";
                public const string DRUGS_EXPIRED_DATE = "IM060300";
                public const string ITEM_DISTRIBUTION_INFO = "IM060401";
                public const string ITEM_DISTRIBUTION_INFO_PER_ITEM = "IM060402";
                public const string ITEM_DISTRIBUTION_OUTSTANDING = "IM060403";
                public const string ITEM_DISTRIBUTION_CONFIRMED_OUTSTANDING = "IM060404";
                public const string ITEM_PURCHASE_RECEIVE_PER_ITEM = "IM060501";
                public const string ITEM_PURCHASE_RECEIVE_PER_SUPPLIER = "IM060502";
                public const string PURCHASE_ORDER_OUTSTANDING = "IM060503";
                public const string PURCHASE_RECEIVE_OUTSTANDING = "IM060504";
                public const string TARIFF_ESTIMATION = "IM060600";
                public const string STOCK_DETAIL_INFO = "IM060700";
                public const string ITEM_NON_MOVEMENT = "IM060800";

                public const string CSSD_MASTER_PACKAGE = "IM070101";

                public const string CSSD_REQUEST = "IM070201";
                public const string CSSD_HANDOVER = "IM070202";

                public const string CSSD_HANDOVER_CONFIRMATION = "IM070301";
                public const string CSSD_PROCESS = "IM070302";
                public const string CSSD_STORAGING = "IM070303";

                public const string CSSD_DISTRIBUTION = "IM070401";
                public const string CSSD_DISTRIBUTION_CONFIRMATION = "IM070402";

                public const string REPORT = "IM090000";
            }
            #endregion

            #region Laboratory
            public static class Laboratory
            {
                public const string LB_PROCESS_BRIDING_MASPION = "LB070305";
                public const string ITEM_GROUP = "LB010100";
                public const string SPECIMEN_SOURCE_CODE = "LB010200";
                public const string FRACTION_GROUP = "LB010300";
                public const string FRACTION = "LB010400";

                public const string ITEM_SERVICE_LB = "LB010501";
                public const string ITEM_SERVICE_AIO_LB = "LB010502";

                public const string TEMPLATE_GROUP = "LB010600";
                public const string TEST_PARTNER = "LB010700";
                public const string PANEL_LAB = "LB010800";
                public const string TEMPLATE_PANEL_LABORATORY = "LB010800";


                public const string REGISTRATION = "LB020200";
                public const string MEDICAL_FOLDER_REQUEST = "LB020300";

                public const string WORK_LIST = "LB030100";
                public const string WORK_LIST_DETAIL = "LB030101";
                public const string LAB_RESULT = "LB030200";
                public const string LAB_RESULT_V2 = "LB030900";
                public const string LAB_RESULT_VERIFICATION = "LB030300";
                public const string PATIENT_TRANSACTION_PAGE = "LB030400";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "LB030401";
                public const string PATIENT_TRANSACTION_TEST_ORDER = "LB030402";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "LB030403";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER = "LB030404";
                public const string PATIENT_TRANSACTION_OUTPATIENT_ORDER = "LB030405";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "LB030406";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "LB030407";
                public const string TEST_ORDER_RESULT_LIST = "LB030408";
                public const string PATIENT_OUTSTANDING_ORDER_LIST = "LB030409";
                public const string PATIENT_USE = "LB030410";
                public const string PATIENT_ORDER_LIST = "LB030411";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "LB030412";
                public const string PATIENT_TRANSACTION_BLOOD_BANK_ORDER = "LB030413";

                public const string TRANSFER_ORDER = "LB030500";

                public const string PATIENT_PAGE = "LB030600";
                public const string GENERATE_ORDER_AIO = "LB030700";
                public const string CONTROL_ORDER_AIO = "LB030800";
                public const string PATIENT_PAGE_VITAL_SIGN = "LB030611";

                public const string TEST_ORDER_LB_IS_RESULT_LIST = "LB030641";

                public const string PATIENT_PAGE_E_DOCUMENT = "LB030651";

                public const string WORK_LIST_AIO = "LB031000";

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "LB030621";
                public const string NURSING_ASSESSMENT_PROCESS = "LB030622";
                #endregion

                #region Catatan Terintegrasi
                public const string PATIENT_NURSING_NOTES = "LB030631";
                public const string PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "LB030632";
                public const string PATIENT_PAGE_PARAMEDIC_NOTE = "LB030633";
                public const string PATIENT_PAGE_PHYSICIAN_NOTE = "LB030634";
                public const string PATIENT_PAGE_INTEGRATED_NOTE = "LB030635";
                public const string PATIENT_DIAGNOSE_DIAGNOSTIC = "LB030636";
                public const string PATIENT_VERIFY_NURSING_JOURNAL = "LB030637";
                public const string PATIENT_NURSING_JOURNAL = "LB030638";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "LB030660";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "LB030661";
                #endregion

                public const string PATIENT_PAGE_PATIENT_EDUCATION = "LB030623";
                public const string PARAMEDIC_SCHEDULE = "LB040100";
                public const string APPOINTMENT = "LB040200";
                public const string APPOINTMENT_2 = "LB040400";
                public const string GENERATE_APPOINTMENT = "LB040300";

                public const string BILL_SUMMARY = "LB060100";
                public const string BILL_SUMMARY_CHARGES = "LB060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "LB060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "LB060113";
                public const string BILL_SUMMARY_IMAGING = "LB060114";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "LB060115";
                public const string BILL_SUMMARY_PHARMACY = "LB060116";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "LB060117";
                public const string BILL_SUMMARY_GENERATE_BILL = "LB060121";
                public const string BILL_SUMMARY_DISCOUNT = "LB060122";
                public const string BILL_SUMMARY_PAYMENT = "LB060123";
                public const string BILL_SUMMARY_DETAIL = "LB060124";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "LB060125";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "LB060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "LB060127";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "LB060128";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "LB060129";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "LB060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "LB060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "LB060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "LB060134";
                public const string BILL_SUMMARY_VOID_BILL = "LB060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "LB060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "LB060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "LB060144";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "LB060145";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "LB060146";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "LB060147";
                public const string BILL_SUMMARY_CLOSE_BILLING = "LB060148";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "LB060148";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "LB060149";

                public const string BPJS_PROCESS_CLAIM_ORDER = "LB060155";
                public const string AIO_ALOCATION_TRANSACTION_ENTRY = "LB060156";
                public const string AIO_ALOCATION_TRANSACTION_CONTROL = "LB060157";
                public const string AIO_BILL_SUMMARY_RECALCULATION = "LB060158";

                public const string APPOINTMENT_INFORMATION = "LB080160";
                public const string E_DOCUMENT = "LB060161";

                public const string LB_LIS_HCLAB = "LB070601";

                public const string BILL_INFORMATION = "LB060191";
                public const string PENDING_TRANSACTION = "LB060192";
                public const string PAYMENT_TRANSACTION = "LB060193";
                //public const string CHARGES_TRANSACTION = "LB060194";
                public const string INFORMATION_CUSTOMER_PAYER_LABORATORY = "LB060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_LABORATORY = "LB060196";
                public const string INFORMATION_VISIT_HISTORY_LABORATORY = "LB060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "LB060198";
                public const string INFORMATION_DISCOUNT_DETAIL = "LB060199";

                public const string PATIENT_BILL_DETAIL_REPRINT = "LB070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "LB070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "LB070121";
                public const string PAYMENT_RECEIPT_REPRINT = "LB070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "LB070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "LB070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "LB070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "LB070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "LB070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "LB070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "LB070240";

                public const string VOID_REGISTRATION_DAILY = "LB070301";
                public const string CLOSE_REGISTRATION = "LB070302";
                public const string TARIFF_TEST_ESTIMATION = "LB070303";
                public const string VOID_REGISTRATION = "LB070304";

                public const string RESEND_ORDER = "LB070306";
                public const string SENT_EMAIL_LABORATORY_RESULT = "LB070307";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "LB070308";

                public const string BRIDGING_STATUS = "LB070420";

                public const string RESULT_DELIVERY_TO_PATIENT = "LB070501";
                public const string RESULT_DELIVERY_INTERNAL = "LB070502";

                public const string LAB_SCHEDULED_LIST = "LB080110";
                public const string INFORMATION = "LB080120";
                public const string PRINTING_PATIENT_CARD = "LB080130";
                public const string HISTORY_INFORMATION = "LB080140";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "LB080150";
                public const string CORRECTION_TRANSACTION_INFORMATION = "LB080210";
                public const string PAYMENT_INFORMATION = "LB080220";
                public const string TARIFF_ESTIMATION = "LB080310";
                public const string PIVOT_PATIENT_CHARGES = "LB080320";
                public const string CUSTOMER_CONTRACT_INFORMATION = "LB080330";
                public const string TARIFF_SERVICE_INFORMATION = "LB080340";
                public const string DRUGS_LOGISTICS_PRICE = "LB080350";
                public const string LB_DATA_KUNJUNGAN_PASIEN_BPJS = "LB080411";
                public const string INFORMATION_LABORATORIUM = "LB080510";
                public const string INFORMATION_RESULT_LABORATORIUM = "LB080520";

                public const string REPORT = "LB090000";

                public const string LB_BPJS_CLAIM_DIAGNOSA_PASIEN = "LB060151";
                public const string LB_BPJS_CLAIM_TINDAKAN_PASIEN = "LB060152";
                public const string LB_BPJS_CLAIM_DISPOSISI_PASIEN = "LB060153";
                public const string LB_BPJS_CLAIM_ENTRY = "LB060154";

                public const string PATIENT_PAGE_CHARM_BAR = "LB900000";

                #region Pemeriksaan Fisik
                public const string PATIENT_PAGE_REVIEW_OF_SYSTEM = "LB030612";
                #endregion
            }
            #endregion

            #region Medical Checkup
            public static class MedicalCheckup
            {
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "MC060149"; 
                public const string APPOINTMENT_INFORMATION = "MC050110";
                public const string MC_PROCESS_BRIDING_MASPION = "MC070400";
                public const string ITEM_GROUP = "MC010100";

                public const string ITEM_SERVICE_MC = "MC010201";

                public const string TEMPLATE_PANEL_MCU = "MC010300";
                public const string REGISTRATION = "MC020100";
                public const string REGISTRATION_GROUP = "MC020200";

                public const string GENERATE_ORDER = "MC030100";
                public const string CONTROL_ORDER = "MC030200";
                public const string GENERATE_ORDER_NEW = "MC030300";
                public const string CONTROL_ORDER_AND_BILL = "MC030400";
                public const string CHANGE_STATUS_MCU = "MC030500";
                public const string PATIENT_PAGE = "MC030600";

                public const string DATA_PATIENT_FALL_RISK_ASSESSMENT_FORM = "MC030611";
                public const string DATA_PATIENT_PAIN_ASSESSMENT_FORM = "MC030612";
                public const string DATA_PATIENT_PATIENT_PAGE_MEDICAL_ASSESSMENT_FORM = "MC030613";
                public const string DATA_PATIENT_PATIENT_PAGE_POPULATION_ASSESSMENT = "MC030614";

                public const string DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT = "MC030621";

                public const string MCU_GETRESULT_EXTERNAL = "MC070200";
                public const string MCU_PIVOT = "MC070300";
                public const string DOWNLOAD_PENGISIAN_HASIL_MCU = "MC070400";

                public const string MCU_RESULT = "MC040000";
                public const string MCU_RESULT_EXTRENAL = "MC040100";
                public const string MCU_RESULT_FORM = "MC040200";
                public const string BILL_SUMMARY = "MC060100";
                public const string BILL_SUMMARY_CHARGES = "MC060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "MC060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "MC060113";
                public const string BILL_SUMMARY_IMAGING = "MC060114";
                public const string BILL_SUMMARY_LABORATORY = "MC060115";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "MC060116";
                public const string BILL_SUMMARY_PHARMACY = "MC060117";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "MC060118";
                public const string BILL_SUMMARY_GENERATE_BILL = "MC060121";
                public const string BILL_SUMMARY_DISCOUNT = "MC060122";
                public const string BILL_SUMMARY_PAYMENT = "MC060123";
                public const string BILL_SUMMARY_DETAIL = "MC060124";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "MC060125";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "MC060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "MC060127";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "MC060128";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "MC060129";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "MC060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "MC060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "MC060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "MC060134";
                public const string BILL_SUMMARY_VOID_BILL = "MC060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "MC060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "MC060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "MC060144";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "MC060145";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "MC060146";
                public const string BILL_SUMMARY_CLOSE_BILLING = "MC060147";
                public const string BILL_SUMMARY_RECALCULATION_BILL_PACKAGE = "MC060148";

                public const string BPJS_PROCESS_CLAIM_ORDER = "MC060151";

                public const string E_DOCUMENT = "MC060161";

                public const string BILL_INFORMATION = "MC060191";
                public const string PENDING_TRANSACTION = "MC060192";
                public const string PAYMENT_TRANSACTION = "MC060193";
                //public const string CHARGES_TRANSACTION = "MC060194";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_MEDICALCHECKUP = "MC060195";
                public const string INFORMATION_CUSTOMER_PAYER_MEDICALCHECKUP = "MC060196";
                public const string INFORMATION_VISIT_HISTORY_MEDICALCHECKUP = "MC060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "MC060198";

                public const string MEDICAL_RECORD = "MC020500";
                public const string MEDICAL_RECORD_VIEW = "MC020501";
                public const string MEDICAL_RECORD_VISIT_LIST = "MC020503";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "MC070101";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "MC070102";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "MC070103";
                public const string CLOSE_REGISTRATION = "MC070601";

                public const string PATIENT_BILL_DETAIL_REPRINT = "MC070701";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "MC070702";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "MC070703";
                public const string PAYMENT_RECEIPT_REPRINT = "MC070704";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "MC070705";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "MC070706";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "MC070707";

                public const string MCU_PARAMEDIC_SCHEDULE = "MC080100";
                public const string MCU_APPOINTMENT = "MC080200";
                public const string MCU_APPOINTMENT_REQUEST = "MC080300";

                public const string REPORT = "MC090000";

                public const string PATIENT_PAGE_CHARM_BAR = "MC900000";
            }
            #endregion

            #region Medical Diagnostic
            public static class MedicalDiagnostic
            {
                public const string MD_PROCESS_BRIDING_MASPION = "MD070304";
                public const string ITEM_GROUP = "MD010100";

                public const string ITEM_SERVICE_MD = "MD010201";
                public const string ITEM_SERVICE_AIO_MD = "MD010202";

                public const string TEMPLATE_GROUP = "MD010300";
                public const string PANEL_MEDICAL_DIAGNOSTIC = "MD010400";
                public const string TEMPLATE_PANEL_DIAGNOSTIC = "MD010400";
                public const string PROCEDURE_GROUP_DIAGNOSTIC = "MD010500";
                public const string MD_PROCEDURE_PANEL = "MD010600";
                public const string TEMPLATE_CHARGES_DIAGNOSTIC = "MD010700";

                public const string REGISTRATION = "MD020200";
                public const string MEDICAL_FOLDER_REQUEST = "MD020300";

                public const string FOLLOWUP_PATIENT_DISCHARGE = "MD020400";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS = "MD020441";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "MD020442";
                public const string FOLLOWUP_PATIENT_PAGE_FISIOTERAPI = "MD020443";
                public const string MST_FORM = "MD020444";
                public const string FOLLOWUP_PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT = "MD020445";
                public const string FOLLOWUP_POPULATION_ASSESSMENT = "MD020446";
                public const string FOLLOWUP_MEDICAL_ASSESSMENT_FORM = "MD020447";
                public const string FOLLOWUP_FALL_RISK_ASSESSMENT_FORM = "MD020448";
                public const string FOLLOWUP_PAIN_ASSESSMENT_FORM = "MD020449";
                public const string FOLLOWUP_VACCINATION_SHOT = "MD020451";
                public const string FOLLOWUP_NUTRITION_SCREENING = "MD020452";
                public const string FOLLOWUP_MST_FORM = "MD020453";
                public const string FOLLOWUP_NURSE_HANDS_OVER = "MD020454";
                public const string FOLLOWUP_EDUCATION_FORM = "MD020455";
                public const string FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION = "MD020456";
                public const string PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "MD030572";
                public const string PATIENT_PAGE_NURSE_ANAMNESIS = "MD030571";
                public const string FOLLOWUP_PATIENT_PAGE_VITAL_SIGN = "MD020411";
                public const string FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM = "MD020412";
                public const string FOLLOWUP_PATIENT_PAGE_E_DOCUMENT = "MD020413";
                public const string FOLLOWUP_TEST_ORDER_LB_IS_RESULT_LIST = "MD020414";
                public const string FOLLOWUP_MONITORING_INTAKE_OUTPUT = "MD020416";
                public const string FOLLOWUP_PARTOGRAF = "MD020417";
                public const string FOLLOWUP_VENTILATOR_MONITORING = "MD020418";

                public const string FOLLOWUP_NURSING_ASSESSMENT_PROBLEM = "MD020421";
                public const string FOLLOWUP_NURSING_ASSESSMENT_PROCESS = "MD020422";
                public const string FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION = "MD020423";

                public const string FOLLOWUP_PATIENT_NURSING_NOTES = "MD020431";
                public const string FOLLOWUP_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "MD020432";
                public const string FOLLOWUP_PATIENT_PAGE_PARAMEDIC_NOTE = "MD020433";
                public const string FOLLOWUP_PATIENT_PAGE_PHYSICIAN_NOTE = "MD020434";
                public const string FOLLOWUP_PATIENT_PAGE_INTEGRATED_NOTE = "MD020435";
                public const string FOLLOWUP_PATIENT_DIAGNOSE_DIAGNOSTIC = "MD020436";
                public const string FOLLOWUP_PATIENT_VERIFY_NURSING_JOURNAL = "MD020437";
                public const string FOLLOWUP_PATIENT_NURSING_JOURNAL = "MD020438";
                public const string FOLLOWUP_PATIENT_MEDICAL_RESUME = "MD020439";

                public const string FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT = "MD020461";

                public const string WORK_LIST = "MD030100";
                public const string WORK_LIST_DETAIL = "MD030101";
                public const string MD_RESULT = "MD030200";
                public const string MD_RESULT_VERIFICATION = "MD030300";
                public const string PATIENT_TRANSACTION_PAGE = "MD030400";
                public const string PATIENT_TRANSACTION_PAGE_PARENT = "MD030450";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "MD030401";
                public const string PATIENT_TRANSACTION_TEST_ORDER = "MD030402";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "MD030403";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER = "MD030404";
                public const string PATIENT_TRANSACTION_OUTPATIENT_ORDER = "MD030405";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "MD030406";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "MD030407";
                public const string NURSING_NOTE_TRANSACTION = "MD030501";
                public const string INTEGRATED_NOTE_TRANSACTION = "MD030502";
                public const string TEST_ORDER_RESULT_LIST = "MD030410";
                public const string VITAL_SIGN_TRANSACTION = "MD030503";
                public const string PATIENT_OUTSTANDING_ORDER_LIST = "MD030412";
                public const string PATIENT_USE = "MD030413";
                public const string PATIENT_ORDER_LIST = "MD030414";
                public const string DIAGNOSTIC_NUTRITION_ORDER = "MD030415";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "MD030416";
                public const string PATIENT_TRANSACTION_BLOOD_BANK_ORDER = "MD030417";
                public const string VISIT_NOTE = "MD030500";

                public const string NURSE_INITIAL_ASSESSMENT = "MD030421";

                public const string APPOINTMENT_INFORMATION = "MD080170";
                public const string PATIENT_PAGE = "MD030500";
                public const string OPERATING_ROOM_PATIENT_LIST = "MD031000";

                public const string WORK_LIST_AIO = "MD030900";

                public const string PATIENT_PAGE_MD_NURSE_INITIAL_ASSESSMENT = "MD035101";

                #region Pengkajian Pasien
                public const string PATIENT_PAGE_POPULATION_ASSESSMENT = "MD035102";
                public const string PATIENT_PAGE_MEDICAL_ASSESSMENT_FORM = "MD035103";
                public const string FALL_RISK_ASSESSMENT_FORM = "MD035104";
                public const string PAIN_ASSESSMENT_FORM = "MD035105";
                public const string PATIENT_PAGE_OBSTETRIC_ASSESSMENT_FORM = "MD035107";
                public const string PATIENT_PAGE_NUTRITION_SCREENING = "MD035108";
                public const string PATIENT_PAGE_VERIFY_NUTRITION_SCREENING = "MD035109";
                #endregion

                #region Pengkajian Kamar Operasi
                public const string ASESMEN_KAMAR_OPERASI = "MD035111";
                public const string FORM_PENGKAJIAN_KAMAR_OPERASI = "MD035112";
                #endregion

                #region Pengkajian Hemodialisa
                public const string FORM_PENGKAJIAN_HEMODIALISA = "MD035121";
                public const string MD035122 = "MD035122";
                public const string MD035123 = "MD035123";
                public const string MD035124 = "MD035124";

                #region Follow-up Pasien Pulang
                public const string FOLLOWUP_PATIENT_PAGE_FORM_PENGKAJIAN_HEMODIALISA = "MD020471";
                public const string FOLLOWUP_PATIENT_PAGE_PERESEPAN_HEMODIALISA = "MD020472";
                public const string FOLLOWUP_PATIENT_PAGE_PEMANTAUAN_INTRA_HD = "MD020473";
                public const string FOLLOWUP_PATIENT_PAGE_POST_HD = "MD020474";

                public const string FOLLOWUP_ASESMEN_KAMAR_OPERASI = "MD020481";
                public const string FOLLOWUP_FORM_PENGKAJIAN_KAMAR_OPERASI = "MD020482";

                public const string FOLLOWUP_PATIENT_PAGE_OBSTETRIC_ASSESSMENT_FORM = "MD020491";

                public const string FOLLOWUP_PATIENT_PAGE_ALLERGY = "MD020501";
                public const string FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY = "MD020502";
                public const string FOLLOWUP_ANTENATAL_RECORD = "MD020503";
                public const string FOLLOWUP_PATIENT_PAGE_VACCINATION = "MD020504";

                #endregion

                #endregion

                #region Pengkajian Riwayat Kesehatan
                public const string PATIENT_PAGE_ALLERGY = "MD030521";
                public const string PATIENT_PAGE_OBSTETRIC_HISTORY = "MD030522";
                public const string ANTENATAL_RECORD = "MD030523";
                public const string PATIENT_PAGE_VACCINATION = "MD030524";

                #endregion

                #region Pemeriksaan Fisik
                public const string PATIENT_PAGE_REVIEW_OF_SYSTEM = "MD030481";
                #endregion

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "MD035031";
                public const string NURSING_ASSESSMENT_PROCESS = "MD035032";
                #endregion

                #region Transaksi dan Order Pelayanan
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER_LIST = "MD035501";
                public const string PATIENT_TRANSACTION_DIAGNOSTIC_ORDER = "MD035502";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER_2 = "MD035503";
                public const string PATIENT_DIAGNOSTIC_BLOOD_BANK_ORDER = "MD035504";
                public const string PATIENT_DIAGNOSTIC_TEST_ORDER_OK = "MD035505";
                public const string PATIENT_TRANSACTION_PAGE_PROCEDURES = "MD035506";
                #endregion

                #region Catatan Terintegrasi
                public const string PATIENT_NURSING_NOTES = "MD030546";
                public const string PATIENT_VERIFY_NURSING_JOURNAL = "MD030547";
                public const string PATIENT_NURSING_JOURNAL = "MD030548";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "MD030549";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "MD030550";
                public const string DATA_PATIENT_MEDICAL_RESUME = "MD030553";
                public const string PATIENT_PAGE_EDUCATION_FORM = "MD035610";
                #endregion

                #region Monitoring dan Evaluasi
                public const string TEST_ORDER_LB_IS_RESULT_LIST = "MD035701";
                public const string MONITORING_INTAKE_OUTPUT = "MD035702";
                public const string PARTOGRAF = "MD035703";
                public const string VENTILATOR_MONITORING = "MD035704";
                #endregion

                #region Informasi
                public const string DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT = "MD036101";
                #endregion

                #region Proses dan Utilitas
                public const string PATIENT_PAGE_E_DOCUMENT = "MD035801";
                public const string PATIENT_OUTSTANDING_ORDER_LIST_MD035802 = "MD035802";
                public const string PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT = "MD035903";
                #endregion

                public const string E_DOCUMENT = "MD060161";

                public const string VACCINATION_SHOT = "MD035106";

                public const string PATIENT_PAGE_TRANSACTION_ENTRY = "MD030510";
                public const string PATIENT_PAGE_ORDER_ENTRY = "MD030520";
                public const string PATIENT_PAGE_MEDICAL_RECORD = "MD030530";
                public const string PATIENT_PAGE_VITAL_SIGN = "MD030531";
                public const string PATIENT_PAGE_FISIOTERAPI = "MD030552";

                public const string PATIENT_PAGE_PARAMEDIC_NOTE = "MD030542";
                public const string PATIENT_PAGE_PHYSICIAN_NOTE = "MD030543";
                public const string PATIENT_PAGE_INTEGRATED_NOTE = "MD030544";
                public const string PATIENT_PAGE_SURGERY_REPORT = "MD030551";
                public const string PATIENT_PAGE_SURGERY_INTEGRATION_NOTES = "MD030522";
                public const string MEDICAL_HISTORY = "MD030591";
                public const string PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "MD030592";
                public const string ORDER_REALIZATION = "MD030561";
                public const string NURSING_NOTE = "MD030501";
                public const string INTEGRATED_NOTE = "MD030502";
                public const string VITAL_SIGN = "MD030503";
                public const string MEDICAL_RECORD = "MD030600";
                public const string MEDICAL_RECORD_VIEW = "MD030601";
                public const string MEDICAL_RECORD_VISIT_LIST = "MD030603";
                public const string GENERATE_ORDER_AIO = "MD030700";
                public const string CONTROL_ORDER_AIO = "MD030800";

                public const string PARAMEDIC_SCHEDULE = "MD040100";
                public const string APPOINTMENT = "MD040200";
                public const string APPOINTMENT_2 = "MD040500";
                public const string GENERATE_APPOINTMENT = "MD040300";
                public const string OPERATING_ROOM_SCHEDULE = "MD040400";
                public const string APPROVE_MULTI_VISIT_SCHEDULE_ORDER = "MD040601";
                public const string PROCESS_MULTI_VISIT_SCHEDULE_ORDER = "MD040602";

                public const string BILL_SUMMARY = "MD060100";
                public const string BILL_SUMMARY_CHARGES = "MD060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "MD060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "MD060113";
                public const string BILL_SUMMARY_IMAGING = "MD060114";
                public const string BILL_SUMMARY_LABORATORY = "MD060115";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "MD060116";
                public const string BILL_SUMMARY_PHARMACY = "MD060117";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "MD060118";
                public const string BILL_SUMMARY_GENERATE_BILL = "MD060121";
                public const string BILL_SUMMARY_DISCOUNT = "MD060122";
                public const string BILL_SUMMARY_PAYMENT = "MD060123";
                public const string BILL_SUMMARY_DETAIL = "MD060124";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "MD060125";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "MD060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "MD060127";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "MD060128";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "MD060129";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "MD060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "MD060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "MD060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "MD060134";
                public const string BILL_SUMMARY_VOID_BILL = "MD060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "MD060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "MD060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "MD060144";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "MD060145";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "MD060146";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "MD060147";
                public const string BILL_SUMMARY_CLOSE_BILLING = "MD060148";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "MD060148";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "MD060149";

                public const string BPJS_PROCESS_CLAIM_ORDER = "MD060155";
                public const string AIO_ALOCATION_TRANSACTION_ENTRY = "MD060156";
                public const string AIO_ALOCATION_TRANSACTION_CONTROL = "MD060157";
                public const string AIO_BILL_SUMMARY_RECALCULATION = "MD060158";

                public const string BILL_INFORMATION = "MD060191";
                public const string PENDING_TRANSACTION = "MD060192";
                public const string PAYMENT_TRANSACTION = "MD060193";
                //public const string CHARGES_TRANSACTION = "MD060194";
                public const string INFORMATION_CUSTOMER_PAYER_MEDICALDIAGNOSTIC = "MD060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_MEDICALDIAGNOSTIC = "MD060196";
                public const string INFORMATION_VISIT_HISTORY_MEDICALDIAGNOSTIC = "MD060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "MD060198";
                public const string INFORMATION_DISCOUNT_DETAIL = "MD060199";

                public const string PATIENT_BILL_DETAIL_REPRINT = "MD070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "MD070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "MD070121";
                public const string PAYMENT_RECEIPT_REPRINT = "MD070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "MD070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "MD070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "MD070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "MD070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "MD070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "MD070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "MD070240";

                public const string VOID_REGISTRATION_DAILY = "MD070301";
                public const string CLOSE_REGISTRATION = "MD070302";
                public const string VOID_REGISTRATION = "MD070303";

                public const string MD_RESULT_HISTORY = "MD070410";
                public const string MD_RESULT_HISTORY_DETAIL = "MD070411";

                public const string TARIFF_TEST_ESTIMATION = "MD070600";

                public const string MD_SCHEDULED_ORDER = "MD080110";
                public const string INFORMATION = "MD080120";
                public const string PRINTING_PATIENT_CARD = "MD080130";
                public const string HISTORY_INFORMATION = "MD080140";
                public const string INFORMATION_PATIENT_TRANSFER = "MD080150";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "MD080160";
                public const string CORRECTION_TRANSACTION_INFORMATION = "MD080210";
                public const string PAYMENT_INFORMATION = "MD080220";
                public const string INFORMATION_TRANSACTION_PATIENT_MD080230 = "MD080230";
                public const string INFORMATION_TRANSACTION_PATIENT_DETAIL_MD080231 = "MD080231";
                public const string TARIFF_ESTIMATION = "MD080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "MD080320";
                public const string TARIFF_SERVICE_INFORMATION = "MD080330";
                public const string MD_DATA_KUNJUNGAN_PASIEN_BPJS = "MD080411";
                public const string DRUGS_LOGISTICS_PRICE = "MD080340";

                public const string REPORT = "MD090000";

                public const string MD_BPJS_CLAIM_DIAGNOSA_PASIEN = "MD060151";
                public const string MD_BPJS_CLAIM_TINDAKAN_PASIEN = "MD060152";
                public const string MD_BPJS_CLAIM_DISPOSISI_PASIEN = "MD060153";
                public const string MD_BPJS_CLAIM_ENTRY = "MD060154";

                public const string PATIENT_PAGE_CHARM_BAR = "MD900000";
                public const string PATIENT_DIAGNOSE_DIAGNOSTIC = "MD030545";

                public const string PATIENT_PAGE_PATIENT_EDUCATION = "MD035033";

                public const string MD035141 = "MD035141";
                public const string MD020521 = "MD020521";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "MD070305";
                public const string MD035803 = "MD035803";

            }
            #endregion

            #region Medical Record
            public static class MedicalRecord
            {
                public const string DTD = "MR010100";
                public const string ICDBLOCK = "MR010200";
                public const string DIAGNOSE = "MR010300";
                public const string MORPHOLOGY = "MR010400";
                public const string PROCEDURES = "MR010500";
                public const string SPECIALTY = "MR010600";
                public const string VISIT_TYPE = "MR010700";
                public const string VACCINATION_TYPE = "MR010800";
                public const string VITAL_SIGN = "MR010900";
                public const string BODY_DIAGRAM = "MR011000";
                public const string PATIENT_INSTRUCTION = "MR011100";

                public const string REFERRER = "MR011500";

                public const string CLINICAL_PATHWAY = "MR011600";

                public const string BOR_MASTER = "MR011800";
                public const string BOR_MASTER_SERVICE_UNIT_BED_COUNT = "MR011801";
                public const string BOR_MASTER_CLASS_CARE_BED_COUNT = "MR011802";
                public const string BOR_MASTER_SERVICE_UNIT_CLASS_CARE_BED_COUNT = "MR011803";

                public const string PATIENT_DATA = "MR020100";
                public const string PATIENT_DETAIL = "MR020200";

                public const string RL_REPORT_CONFIGURATION = "MR030101";
                public const string PATIENT_FOLDER_STATUS = "MR030200";
                public const string PATIENT_DIAGNOSE_QUICK_ENTRY = "MR030400";

                public const string MEDICAL_RECORD_FORM = "MR040101";
                public const string MEDICAL_RECORD_FOLDER = "MR040102";
                public const string QUESTION_GROUP = "MR040201";
                public const string QUESTION = "MR040202";
                public const string ANSWER_TYPE = "MR040203";
                public const string ANSWER = "MR040204";
                public const string PATIENT_FOLDER_TRACKING = "MR040901";
                public const string PATIENT_SOAP = "MR040902";
                public const string MEDICAL_RECORD_KONSUL = "MR990300";

                public const string BPJS_CLAIM_PROCESS = "MR051000";
                public const string BPJS_CODING_CLAIM_PROCESS = "MR051001";
                public const string BPJS_CODING_CLAIM_DIAGNOSA = "MR051200";
                public const string BPJS_PROSES_DIAGNOSA_PASIEN = "MR051201";
                public const string BPJS_PROSES_TINDAKAN_PASIEN = "MR051202";
                public const string BPJS_PROSES_TINDAK_LANJUT = "MR051203";
                public const string BPJS_PROSES_RUJUKAN = "MR051204";
                public const string BPJS_PROSES_CLAIM_PROPOSE = "MR051301";
                public const string BPJS_SEP_APPROVAL = "MR052000";
                public const string BPJS_FINGER_PRINT = "MR053000";

                public const string BRIDGING_DINKES_TEMPAT_TIDUR = "MR060101";
                public const string BRIDGING_SIRANAP_REKAPPASIENMASUK = "MR060201";
                public const string BRIDGING_SIRANAP_REKAPPASIENDIRAWATDENGANKOMORBID = "MR060202";
                public const string BRIDGING_SIRANAP_REKAPPASIENDIRAWATTANPAKOMORBID = "MR060203";
                public const string BRIDGING_SIRANAP_REKAPPASIENKELUAR = "MR060204";

                public const string IHS_ENCOUNTER_LIST_1 = "MR060301";

                public const string MERGE_MEDICAL_RECORD = "MR070101";
                public const string RETENTION_MEDICAL_RECORD = "MR070102";
                public const string ARCHIVE_MEDICAL_RECORD = "MR070103";
                public const string GENERATE_MEDICAL_NUMBER_FROM_GUEST = "MR070104";
                public const string CENSUS_PER_DAY_PROCESS = "MR070200";
                public const string IMPORT_PATIENT_DIAGNOSIS_BPJS = "MR070910";
                public const string INACBGS = "MR070920";
                public const string INACBGS_PATIENT_DIAGNOSIS = "MR170921";
                public const string INACBGS_PATIENT_PROCEDURE_LIST = "MR170922";
                public const string INACBGS_PATIENT_DISCHARGE = "MR170923";
                public const string INACBGS_BRIDGING = "MR170924";
                public const string INACBGS_INTEGRATED_NOTES = "MR170925";

                public const string PIVOT_REGISTRATION_ANALYSIS = "MR080201";
                public const string PIVOT_DIAGNOSE_ANALSIS = "MR080202";
                public const string PIVOT_PATIENT_CHARGES = "MR080203";
                public const string PIVOT_VISIT_ANALYSIS = "MR080204";
                public const string PIVOT_RLCLASS = "MR080205";
                public const string CENCUS_PER_DAY = "MR080300";
                public const string APPOINTMENT_INFORMATION = "MR080400";
                public const string BRIDGING_TO_INACBGs = "MR080500";
                public const string MRN_MERGE_HISTORY = "MR080600";
                public const string PATIENT_IMPLANT_INFORMATION = "MR080700";

                public const string REPORT = "MR090000";
                public const string PATIENT_PAGE = "MR990000";
                public const string MEDICAL_HISTORY = "MR990101";
                public const string TRANSACTION_HISTORY = "MR990102";
                public const string APPOINTMENT_HISTORY = "MR990103";
                public const string PATIENT_FOLDER = "MR990201";

                public const string PATIENT_PAGE_CHARM_BAR = "MR900000";

                public const string SatuSEHAT_Dashboard = "MR101100";
            }
            #endregion

            #region Nursing
            public static class Nursing
            {
                public const string DOMAIN_CLASS = "NR010100";
                public const string NURSING_ITEM_GROUP = "NR010200";
                public const string NURSING_DIAGNOSE = "NR010300";
                public const string NURSING_OUTCOME = "NR010400";
                public const string NURSING_INTERVENTION = "NR010500";
                public const string NURSING_PROBLEM = "NR010600";
                public const string NURSING_OUTCOME_MASTER = "NR010700";
                public const string NURSING_INDICATOR_MASTER = "NR010800";
                public const string NURSING_TEMPLATE_TEXT_MASTER = "NR010900";

                public const string NURSING_PATIENT_LIST_EMERGENCY = "NR021000";
                public const string NURSING_PATIENT_LIST_INPATIENT = "NR022000";
                public const string NURSING_PATIENT = "NR020101";

                public const string NURSING_PATIENT_PAGE = "NR020200";
                public const string NURSING_PATIENT_ASSESSMENT = "NR020211";
                public const string NURSING_PATIENT_PAGE_VITAL_SIGN = "NR020221";
                public const string NURSING_PATIENT_PAGE_VITAL_SIGN_MENU = "NR020212";
                public const string NURSING_PATIENT_PAGE_REVIEW_OF_SYSTEM = "NR020222";
                public const string NURSING_PATIENT_PAGE_VACCINATION_COVID = "NR020223";
                public const string NURSING_PATIENT_PAGE_NANDA = "NR020231";
                public const string NURSING_PATIENT_PAGE_CHARGES = "NR020249";


                public const string NURSING_TRANSACTION = "NR020100";
                public const string NURSING_TRANSACTION_INPATIENT = "NR020100";
                public const string NURSING_TRANSACTION_EMERGENCY = "NR020300";
                public const string NURSING_TRANSACTION_OUTPATIENT = "NR020400";
                public const string NURSING_JOURNAL_INPATIENT = "NR020200";
                public const string NURSING_JOURNAL_EMERGENCY = "NR020500";
                public const string NURSING_JOURNAL_OUTPATIENT = "NR020600";
                public const string REPORT = "NR090000";

                public const string INFO_NURSING_TRANSACTION = "NR030100";
                public const string INFO_NURSING_JOURNAL = "NR030200";

                public const string NURSING_INPATIENT_INITIAL_ASSESSMENT = "NR020211";
            }
            #endregion

            #region Nutrition
            public static class Nutrition
            {
                public const string ITEM_GROUP = "NT010100";
                public const string NUTRIENT = "NT010200";
                public const string FOOD = "NT010300";
                public const string MEAL = "NT010400";
                public const string MEAL_PLAN = "NT010500";

                public const string NUTRITION_ORDER_INPATIENT = "NT020101";
                public const string NUTRITION_ORDER_EMERGENCY = "NT020102";
                public const string NUTRITION_ORDER_OUTPATIENT = "NT020103";
                public const string WORK_LIST = "NT020200";
                public const string NUTRITION_DISTRIBUTION = "NT020300";
                public const string NUTRITION_DISTRIBUTION_NEWORDER = "NT020500";
                public const string NUTRITION_EVALUATION = "NT020400";

                public const string NUTRITION_PATIENT_PAGE = "NT031000";
                public const string NUTRITION_GLOBAL_ASSESSMENT = "NT031101";
                public const string NUTRITION_INTEGRATED_NOTES = "NT031401";
                public const string NUTRITION_VITAL_SIGN = "NT031102";
                public const string NUTRITION_ALLERGY = "NT031103";
                public const string NUTRITION_REVIEW_OF_SYSTEM = "NT031104";
                public const string NUTRITION_PATIENT_DIAGNOSE = "NT031105";
                public const string NUTRITION_DIAGNOSTIC_RESULT = "NT031106";
                public const string NUTRITION_DIAGNOSTIC_FORM_PENGKAJIAN = "NT031107";
                public const string NUTRITION_DIAGNOSTIC_FORM_PENGKAJIAN_GIZI = "NT031108";
                public const string NUTRITION_DIAGNOSTIC_VERIFY_FORM_PENGKAJIAN_GIZI = "NT031109";

                public const string NUTRITION_NOTES = "NT031201";

                public const string NUTRITION_CARE_NOTES = "NT031402";

                public const string ADULT_SCREENING_ASSESSMENT = "NT031601";
                public const string GERIATRIC_SCREENING_ASSESSMENT = "NT031602";
                public const string OBSTETRIC_SCREENING_ASSESSMENT = "NT031603";
                public const string STRONG_KIDS_ASSESSMENT = "NT031604";
                public const string MST_FORM = "NT031605";
                public const string NUTRITION_ASSESMENT = "NT031606";
                public const string PATIENT_EDUCATION_NUTRITION = "NT031607";

                public const string NUTRITION_ASSESSMENT_WORKLIST = "NT032000";
                public const string NUTRITION_WORKLIST_INFORMATION = "NT033000";
                public const string NUTRITION_INGREDIENTS_INFORMATION = "NT034000";

                public const string NUTRITION_ORDER = "NT031301";
                public const string NUTRITION_INTERVENTION_NOTES = "NT031302";

                public const string CHANGE_MEAL_STATUS_ORDER = "NT031701";
                public const string DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT = "NT031702";

                public const string NUTRITION_ORDER_EDIT = "NT040100";
                public const string NUTRITION_REORDER_MEAL_PLAN = "NT040200";


                public const string HOSPITALIZED_PATIENT_LIST = "NT080100";

                public const string REPORT = "NT090000";

                public const string PATIENT_PAGE_CHARM_BAR = "NT900000";
            }
            #endregion

            #region Outpatient
            public static class Outpatient
            {
                public const string PARAMEDIC_SCHEDULE = "OP010100";
                public const string APPOINTMENT = "OP010200";
                public const string APPOINTMENT_2 = "OP010300";
                public const string GENERATE_APPOINTMENT = "OP010400";
                public const string REQUEST_CANCEL_APPOINTMENT = "OP010500";

                public const string REGISTRATION = "OP020100";
                public const string MEDICAL_FOLDER_REQUEST = "OP020200";
                public const string DIAGNOSE_ENTRY = "OP020300";
                public const string MEDICAL_RECORD = "OP020500";
                public const string MEDICAL_RECORD_VIEW = "OP020501";
                public const string MEDICAL_RECORD_VISIT = "OP020502";
                public const string MEDICAL_RECORD_VISIT_LIST = "OP020503";

                public const string PATIENT_REGISTRATION_CONFIRMATION = "OP030100";
                public const string PATIENT_TRANSACTION_PAGE = "OP030200";
                public const string PATIENT_TRANSACTION_TEST_ORDER = "OP030202";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER = "OP030203";
                public const string PATIENT_TRANSACTION_EMERGENCY_ORDER = "OP030204";
                public const string PATIENT_TRANSACTION_VISIT_ORDER = "OP030205";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_RETURN_ORDER = "OP030206";
                public const string PATIENT_TRANSACTION_PRESCRIPTION_ORDER = "OP030207";
                public const string NURSING_NOTE = "OP030208";
                public const string PHYSICIAN_NOTE = "OP030209";
                public const string INTEGRATED_NOTE = "OP030210";
                public const string TRANSACTION_PAGE_PATIENT_REFERRAL = "OP030211";
                public const string TEST_ORDER_RESULT_LIST = "OP030212";
                public const string PATIENT_TRANSACTION_NUTRITION_ORDER = "OP030213";
                public const string TRANSACTION_PAGE_PATIENT_DIAGNOSIS = "OP030215";
                public const string TRANSACTION_PAGE_PATIENT_PROCEDURES = "OP030216";
                public const string TRANSACTION_PAGE_PATIENT_DISPOSITION = "OP030217";
                public const string PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "OP030220";
                public const string PATIENT_TRANSACTION_TEST_ORDER_OK = "OP030221";
                public const string PATIENT_TRANSACTION_MEDICATION_ORDER_ALKES = "OP030222";
                public const string MEDICAL_RESUME = "OP030223";

                public const string PATIENT_PAGE_OBSTETRIC_HISTORY = "OP030231";

                public const string SERVICE_ORDER_TRANS = "OP030300";
                public const string INFORMATION_PATIENT_SURGERY_REPORT = "OP030311";
                public const string SERVICE_ORDER_RESULT_TRANS = "OP030500";

                public const string CLINIC_SERVICE_TIME = "OP030600";
                public const string GENERATE_ORDER_AIO = "OP030700";
                public const string CONTROL_ORDER_AIO = "OP030800";

                public const string PATIENT_PAGE_NURSE_ANAMNESIS = "OP030241";
                public const string PATIENT_VISIT_TYPE = "OP030247";
                public const string PATIENT_PAGE_CHARGES = "OP030251";
                public const string PATIENT_PAGE_CHARGES_2 = "OP030252";
                public const string PATIENT_PAGE_FOLLOW_UP_VISIT = "OP030281";
                public const string PATIENT_PAGE_FINAL_DIAGNOSIS = "OP030282";

                public const string PATIENT_PAGE = "OP030400";
                public const string DATA_PATIENT_PATIENT_PAGE_POPULATION_ASSESSMENT = "OP030411";
                public const string DATA_PATIENT_PATIENT_PAGE_MEDICAL_ASSESSMENT_FORM = "OP030412";
                public const string DATA_PATIENT_FALL_RISK_ASSESSMENT_FORM = "OP030413";
                public const string DATA_PATIENT_PAIN_ASSESSMENT_FORM = "OP030414";

                public const string DATA_PATIENT_VITAL_SIGN = "OP030421";
                public const string DATA_PATIENT_REVIEW_OF_SYSTEM = "OP030422";

                public const string DATA_PATIENT_NURSING_ASSESSMENT_PROBLEM = "OP030431";
                public const string DATA_PATIENT_NURSING_ASSESSMENT_PROCESS = "OP030432";

                public const string DATA_PATIENT_PATIENT_NURSING_NOTES = "OP030441";
                public const string DATA_PATIENT_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "OP030442";
                public const string DATA_PATIENT_PATIENT_PAGE_PARAMEDIC_NOTE = "OP030443";
                public const string DATA_PATIENT_PATIENT_PAGE_PHYSICIAN_NOTE = "OP030444";
                public const string DATA_PATIENT_PATIENT_PAGE_INTEGRATED_NOTE = "OP030445";
                public const string DATA_PATIENT_PATIENT_DIAGNOSE_DIAGNOSTIC = "OP030446";
                public const string DATA_PATIENT_PATIENT_VERIFY_NURSING_JOURNAL = "OP030447";
                public const string DATA_PATIENT_PATIENT_NURSING_JOURNAL = "OP030448";
                public const string DATA_PATIENT_MEDICAL_RESUME = "OP030449";

                public const string DATA_PATIENT_TEST_ORDER_LB_IS_RESULT_LIST = "OP030451";

                public const string DATA_PATIENT_PATIENT_PAGE_E_DOCUMENT = "OP030461";

                public const string DATA_PATIENT_INFORMATION_PATIENT_SURGERY_REPORT = "OP030471";

                public const string CHIEF_COMPLAINT = "OP030410";
                public const string VITAL_SIGN = "OP030402";
                public const string PHYSICAL_EXAMINATION = "OP030403";
                public const string PHYSICAL_EXAMINATION_BODY_DIAGRAM = "OP030404";
                public const string PATIENT_PAGE_PATIENT_EDUCATION = "OP030405";
                public const string PATIENT_DIAGNOSIS = "OP030407";
                public const string PATIENT_PROCEDURES = "OP030408";
                public const string PATIENT_DISPOSITION = "OP030499";

                public const string PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "OP030248";
                public const string OP030481 = "OP030481";
                public const string OP032410 = "OP032410";
                public const string OP020491 = "OP020491";
                public const string SERVICE_ORDER_TRANS_AIO = "OP030900";

                #region Pengkajian Pasien : Rawat Jalan
                public const string PATIENT_PAGE_NURSE_ASSESSMENT = "OP031201";
                public const string PAIN_ASSESSMENT_FORM = "OP031113";
                public const string MST_FORM = "OP031114";
                public const string FALL_RISK_ASSESSMENT_FORM = "OP031115";
                public const string POPULATION_ASSESSMENT = "OP031116";
                #endregion

                #region Pengkajian Riwayat Kesehatan
                public const string PATIENT_PAGE_ALLERGY = "OP030244";
                public const string PATIENT_PAGE_ANTENATAL = "OP030233";
                public const string PATIENT_PAGE_VACCINATION = "OP030232";
                public const string ANTENATAL_RECORD = "OP030245";
                public const string PATIENT_PAGE_VACCINATION_COVID = "OP030249";
                #endregion


                #region Pemeriksaan Fisik
                public const string PATIENT_PAGE_VITAL_SIGN = "OP030242";
                public const string PATIENT_PAGE_REVIEW_OF_SYSTEM = "OP020243";
                #endregion

                #region Transaksi dan Order
                public const string PATIENT_PAGE_OUTSTANDING_ORDER = "OP030253";
                public const string PATIENT_PAGE_ORDER_LIST = "OP030254";
                #endregion

                #region Asuhan Keperawatan
                public const string NURSING_ASSESSMENT_PROBLEM = "OP032301";
                public const string NURSING_ASSESSMENT_PROCESS = "OP032302";
                #endregion

                #region Catatan Terintegrasi
                public const string PATIENT_PAGE_NURSING_JOURNAL = "OP030218";
                public const string PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "OP030219";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER = "OP030224";
                public const string PATIENT_PAGE_NURSE_HANDS_OVER_CONFIRMATION = "OP030225";
                public const string PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "OP030226";
                public const string MONITORING_INTAKE_OUTPUT = "OP030227";
                public const string VENTILATOR_MONITORING = "OP030228";
                public const string PATIENT_PAGE_INTEGRATED_NOTE = "OP030229";
                #endregion

                #region Proses dan Utilitas
                public const string PATIENT_PAGE_E_DOCUMENT = "OP030243";
                public const string PATIENT_PAGE_CHANGE_ORDER_STATUS = "OP031801";
                public const string BPJS_PROCESS_CLAIM_ORDER_IN_NURSING_MENU = "OP031802";
                public const string PATIENT_PAGE_VERIFY_NURSE_INITIAL_ASSESSMENT = "OP031803";
                public const string OP031804 = "OP031804";
                #endregion


                public const string DRAFT_CHARGES_ENTRY_HEADER = "OP040000";
                public const string DRAFT_CHARGES_ENTRY = "OP040010";
                public const string DRAFT_CHARGES_IMAGING_ENTRY = "OP040020";

                public const string BILL_SUMMARY = "OP060100";
                public const string BILL_SUMMARY_CHARGES = "OP060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "OP060112";
                public const string BILL_SUMMARY_IMAGING = "OP060113";
                public const string BILL_SUMMARY_LABORATORY = "OP060114";
                public const string BILL_SUMMARY_MEDICAL_DIAGNOSTIC = "OP060115";
                public const string BILL_SUMMARY_PHARMACY = "OP060116";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "OP060117";
                public const string BILL_SUMMARY_GENERATE_BILL = "OP060121";
                public const string BILL_SUMMARY_DISCOUNT = "OP060122";
                public const string BILL_SUMMARY_PAYMENT = "OP060123";
                public const string BILL_SUMMARY_DETAIL = "OP060124";
                public const string BILL_SUMMARY_TARIFF_INACBGS = "OP060125";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "OP060126";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "OP060127";
                public const string BILL_SUMMARY_PAYMENT_AR = "OP060128";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "OP060129";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "OP0601210";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "OP060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "OP060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "OP060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "OP060134";
                public const string BILL_SUMMARY_VOID_BILL = "OP060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "OP060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "OP060143";
                public const string BILL_SUMMARY_ORDER_REALIZATION = "OP060144";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "OP060145";
                public const string BILL_SUMMARY_PHYSICIAN_VISIT_LIST = "OP060146";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "OP060147";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "OP060148";
                public const string BILL_SUMMARY_CLOSE_BILLING = "OP060149";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "OP060149";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "OP0601410";

                public const string BPJS_PROCESS_CLAIM_ORDER = "OP060155";
                public const string AIO_ALOCATION_TRANSACTION_ENTRY = "OP060156";
                public const string AIO_ALOCATION_TRANSACTION_CONTROL = "OP060157";
                public const string AIO_BILL_SUMMARY_RECALCULATION = "OP060158";

                public const string E_DOCUMENT = "OP060161";

                public const string BILL_INFORMATION = "OP060191";
                public const string PENDING_TRANSACTION = "OP060192";
                public const string PAYMENT_TRANSACTION = "OP060193";
                public const string PATIENT_DIAGNOSE_OUTPATIENT = "OP060194";
                public const string INFORMATION_CUSTOMER_PAYER_OUTPATIENT = "OP060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_OUTPATIENT = "OP060196";
                public const string INFORMATION_VISIT_HISTORY_OUTPATIENT = "OP060197";
                public const string INFORMATION_VISIT_PATIENT_REFERRAL = "OP060198";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "OP060199";
                public const string INFORMATION_DISCOUNT_DETAIL = "OP060200";

                public const string PATIENT_BILL_DETAIL_REPRINT = "OP070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "OP070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "OP070121";
                public const string PAYMENT_RECEIPT_REPRINT = "OP070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "OP070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "OP070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "OP070141";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "OP070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "OP070220";
                public const string CHANGE_PATIENT_ORDER_STATUS = "OP070230";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "OP070240";

                public const string CANCEL_PATIENT_DISCHARGE = "OP070301";
                public const string VOID_REGISTRATION_DAILY = "OP070302";
                public const string CLOSE_REGISTRATION = "OP070303";
                public const string CANCEL_PHYSICIAN_DISCHARGE = "OP070304";
                public const string VOID_REGISTRATION = "OP070305";
                public const string PROCESS_BRIDING_MASPION = "OP070306";
                
                public const string PATIENT_VISIT_INQUIRY = "OP080200";
                public const string APPOINTMENT_INFORMATION = "OP080120";
                public const string PATIENT_QUEUE_INFORMATION = "OP080130";
                public const string INFORMATION = "OP080140";
                public const string HOSPITALIZED_PATIENT_LIST = "OP080150";
                public const string MEDICAL_RECORD_LIST = "OP080160";
                public const string PRINTING_PATIENT_CARD = "OP080170";
                public const string INFORMATION_PATIENT_TRANSFER = "OP080180";
                public const string APPOINTMENT_INFORMATION_TRANSFER_PER_PARAMEDIC_PER_DAY = "OP080190";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "OP080191";
                public const string TRANSACTION_PATIENT_INFORMATION = "OP080210";
                public const string CORRECTION_TRANSACTION_INFORMATION = "OP080220";
                public const string PAYMENT_INFORMATION = "OP080230";
                public const string TARIFF_ESTIMATION = "OP080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "OP080320";
                public const string TARIFF_SERVICE_INFORMATION = "OP080330";
                public const string OP_DATA_KUNJUNGAN_PASIEN_BPJS = "OP080411";
                public const string DRUGS_LOGISTICS_PRICE = "OP080340";

                public const string REPORT = "OP090000";

                public const string OP_BPJS_CLAIM_DIAGNOSA_PASIEN = "OP060151";
                public const string OP_BPJS_CLAIM_TINDAKAN_PASIEN = "OP060152";
                public const string OP_BPJS_CLAIM_DISPOSISI_PASIEN = "OP060153";
                public const string OP_BPJS_CLAIM_ENTRY = "OP060154";

                public const string PATIENT_PAGE_CHARM_BAR = "OP900000";

                public const string FOLLOWUP_PATIENT_DISCHARGE = "OP020400";

                public const string FOLLOWUP_PATIENT_PAGE_NURSE_ANAMNESIS = "OP020411";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_INITIAL_ASSESSMENT = "OP020412";
                public const string FOLLOWUP_PATIENT_VISIT_TYPE = "OP020413";
                public const string FOLLOWUP_PATIENT_PAGE_PHYSICIAN_INSTRUCTION = "OP020414";
                public const string FOLLOWUP_PATIENT_PAGE_NURSE_ASSESSMENT = "OP020415";
                public const string FOLLOWUP_POPULATION_ASSESSMENT = "OP020416";
                public const string FOLLOWUP_MEDICAL_ASSESSMENT_FORM = "OP020417";
                public const string FOLLOWUP_FALL_RISK_ASSESSMENT_FORM = "OP020418";
                public const string FOLLOWUP_PAIN_ASSESSMENT_FORM = "OP020419";

                public const string FOLLOWUP_PATIENT_PAGE_ALLERGY = "OP020421";
                public const string FOLLOWUP_PATIENT_PAGE_OBSTETRIC_HISTORY = "OP020422";
                public const string FOLLOWUP_PATIENT_PAGE_VACCINATION_COVID = "OP020423";
                public const string FOLLOWUP_MST_FORM = "OP020424";
                public const string FOLLOWUP_NUTRITION_SCREENING = "OP020425";
                public const string FOLLOWUP_PATIENT_PAGE_VACCINATION = "OP020426";
                public const string FOLLOWUP_ANTENATAL_RECORD = "OP020427";

                public const string FOLLOWUP_PATIENT_PAGE_VITAL_SIGN = "OP020431";
                public const string FOLLOWUP_PATIENT_PAGE_REVIEW_OF_SYSTEM = "OP020432";

                public const string FOLLOWUP_NURSING_ASSESSMENT_PROBLEM = "OP020441";
                public const string FOLLOWUP_NURSING_ASSESSMENT_PROCESS = "OP020442";
                public const string FOLLOWUP_PATIENT_PAGE_PATIENT_EDUCATION = "OP020443";

                public const string FOLLOWUP_NURSING_NOTE = "OP020451";
                public const string FOLLOWUP_PHYSICIAN_NOTE = "OP020452";
                public const string FOLLOWUP_PATIENT_PAGE_NURSING_JOURNAL = "OP020453";
                public const string FOLLOWUP_PATIENT_PAGE_VERIFY_NURSING_JOURNAL = "OP020454";
                public const string FOLLOWUP_PATIENT_PAGE_MEDICAL_RESUME = "OP020455";
                public const string FOLLOWUP_PATIENT_PAGE_OUTPATIENT_INTEGRATION_NOTES = "OP020456";
                public const string FOLLOWUP_INTEGRATED_NOTE = "OP020457";
                public const string FOLLOWUP_NURSE_HANDS_OVER = "OP020458";
                public const string FOLLOWUP_NURSE_HANDS_OVER_CONFIRMATION = "OP020459";

                public const string FOLLOWUP_TEST_ORDER_RESULT_LIST = "OP020461";
                public const string FOLLOWUP_PATIENT_REFERRAL = "OP020462";
                public const string FOLLOWUP_MONITORING_INTAKE_OUTPUT = "OP020463";
                public const string FOLLOWUP_VENTILATOR_MONITORING = "OP020464";

                public const string FOLLOWUP_INFORMATION_PATIENT_SURGERY_REPORT = "OP020471";
                public const string FOLLOWUP_PAGE_TESTORDER_INFORMATION = "OP020472";

                public const string FOLLOWUP_PATIENT_PAGE_E_DOCUMENT = "OP020481";
                public const string FOLLOWUP_VERIFY_NURSE_INITIAL_ASSESSMENT = "OP020482";

                public const string MEDICAL_ASSESSMENT_FORM = "OP031109";
                public const string NUTRITION_SCREENING = "OP031117";
                public const string NUTRITION_SCREENING_VERIFY = "OP031118";
            }
            #endregion

            #region Pharmacy
            public static class Pharmacy
            {
                public const string PH_PROCESS_BRIDING_MASPION = "PH070304";
                public const string ATC_CLASSIFICATION = "PH010100";
                public const string MIMS_CLASS = "PH010200";
                public const string DRUGS = "PH010300";
                public const string EMBALACE = "PH010400";
                public const string PRESCRIPTION_SIGNA = "PH010500";
                public const string COMPOUND_TEMPLATE = "PH010600";
                public const string TEMPLATE_PHARMACY_CHARGES = "PH010700";
                public const string PHARMACOGENOMICS = "PH010800";
                public const string PH012000 = "PH012000";

                public const string PRESCRIPTION_ENTRY = "PH020100";
                public const string PRESCRIPTION_UDD_ORDER_ENTRY = "PH020110";
                public const string PRESCRIPTION_RETURN = "PH020200";
                public const string PRESCRIPTION_STATUS_EDIT = "PH020300";
                public const string PRESCRIPTION_STATUS_V2 = "PH020600";
                public const string UDD_PRESCRIPTION_ENTRY = "PH020400";
                public const string UDD_MEDICATION_ORDER = "PH020410";
                public const string UDD_MEDICATION_ORDER_ENTRY = "PH020411";
                public const string UDD_MEDICATION_CHARGES = "PH020412";
                public const string UDD_MEDICATION_SCHEDULE_VOID = "PH020413";
                public const string UDD_MEDICATION_ORDER_LIST = "PH020419";
                public const string UDD_MEDICATION_LIST = "PH020420";
                public const string UDD_MEDICATION_PROCESS = "PH020421";
                public const string UDD_MEDICATION_RECONCILIATION = "PH020422";
                public const string UDD_MEDICATION_LIST_PRN = "PH020424";
                public const string UDD_MEDICATION_SUMMARY = "PH020423";
                public const string PHARMACIST_NOTES = "PH020431";
                public const string PHARMACY_INTEGRATION_NOTES = "PH020432";
                public const string PHARMACY_NOTES_CONFIRMATION = "PH020433";
                public const string PHARMACY_NOTES_JOURNAL = "PH020434";
                public const string PHARMACY_PHYSICIAN_INSTRUCTION = "PH020435";
                public const string PHARMACY_MEDICAL_RESUME = "PH020436";
                public const string PURCHASE_REQUEST = "PH01200";
                public const string PURCHASE_REQUEST_APPROVAL = "PH01300";
                public const string PH_TRANSFER_PRESCRIPTION_ORDER = "PH025000";

                public const string REGISTRATION = "PH030100";
                public const string PATIENT_TRANSACTION_PAGE = "PH030200";
                public const string PHARMACIST_CLINICAL = "PH040100";
                public const string PHARMACIST_CLINICAL_ALLERGY = "PH040111";
                public const string PHARMACIST_CLINICAL_DIAGNOSE = "PH040112";
                public const string PHARMACIST_CLINICAL_TEST_ORDER_RESULTS = "PH040113";
                public const string PHARMACIST_CLINICAL_E_DOCUMENT = "PH040114";
                public const string PHARMACIST_CLINICAL_PHARMACIST_NOTES = "PH040191";
                public const string PHARMACIST_CLINICAL_INTEGRATION_NOTES = "PH040192";
                public const string PHARMACIST_CLINICAL_JOURNAL = "PH040193";
                public const string PATIENT_PAGE_PATIENT_EDUCATION = "PH040194";
                public const string PHARMACIST_CLINICAL_NOTES_CONFIRMATION = "PH040195";
                public const string PHARMACIST_CLINICAL_MEDICAL_RESUME = "PH040196";
                public const string MEDICATION_LIST_SUMMARY = "PH040121";
                public const string MEDICATION_RECONCILIATION = "PH040122";

                public const string PATIENT_PAGE_MEDICAL_ASSESSMENT_FORM = "PH040131";

                public const string BILL_SUMMARY = "PH060100";
                public const string BILL_SUMMARY_PHARMACY = "PH060111";
                public const string BILL_SUMMARY_PRESCRIPTION_RETURN = "PH060112";
                public const string BILL_SUMMARY_SERVICE = "PH060113";
                public const string BILL_SUMMARY_GENERATE_BILL = "PH060121";
                public const string BILL_SUMMARY_DISCOUNT = "PH060122";
                public const string BILL_SUMMARY_PAYMENT = "PH060123";
                public const string BILL_SUMMARY_DETAIL = "PH060124";
                public const string BILL_SUMMARY_GENERATE_BILL_AR = "PH060125";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "PH060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "PH060127";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "PH060128";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "PH060129";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "PH060131";
                public const string BILL_SUMMARY_RECEIPT_REPRINT = "PH060132";
                public const string BILL_SUMMARY_RECEIPT_VOID = "PH060133";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "PH060134";
                public const string BILL_SUMMARY_VOID_BILL = "PH060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "PH060142";
                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "PH060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "PH060144";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "PH060145";
                public const string BILL_SUMMARY_CLOSE_BILLING = "PH060146";
                public const string BILL_TRANSFER_FROM_OTHER_UNIT = "PH060146";
                public const string CANCEL_BILL_TRANSFER_FROM_OTHER_UNIT = "PH060147";

                public const string BILL_INFORMATION = "PH060161";
                public const string PENDING_TRANSACTION = "PH060162";
                public const string PAYMENT_TRANSACTION_DETAIL = "PH060163";
                public const string PATIENT_DIAGNOSE_EMERGENCY = "PH060164";
                public const string INFORMATION_CUSTOMER_PAYER_EMERGENCY = "PH060165";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT_INPATIENT = "PH060166";
                public const string INFORMATION_VISIT_HISTORY_EMERGENCY = "PH060167";
                //public const string PATIENT_DIAGNOSE_EMERGENCY = "PH060168";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "PH060169";
                public const string INFORMATION_DISCOUNT_DETAIL = "PH060170";
                public const string E_DOCUMENT = "PH060181";

                public const string BPJS_PROCESS_CLAIM_ORDER = "PH060155";

                //public const string BILL_INFORMATION = "PH060191";
                //public const string PENDING_TRANSACTION = "PH060192";
                public const string PAYMENT_TRANSACTION = "PH080241";

                public const string PATIENT_BILL_DETAIL_REPRINT = "PH070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "PH070120";
                public const string PATIENT_BILL_DETAIL_REPRINT_2_DETAIL = "PH070121";
                public const string PAYMENT_RECEIPT_REPRINT = "PH070130";
                public const string PAYMENT_RECEIPT_REPRINT_DETAIL = "PH070131";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "PH070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "PH070141";
                public const string PRESCRIPTION_REPRINT = "PH070150";
                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "PH070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "PH070220";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "PH070230";

                public const string VOID_REGISTRATION_DAILY = "PH070301";
                public const string CLOSE_REGISTRATION = "PH070302";
                public const string VOID_REGISTRATION = "PH070303";

                public const string INFORMATION = "PH080110";
                public const string PRINTING_PATIENT_CARD = "PH080120";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "PH080130";
                public const string INFORMATION_OUTSTANDING_UDD_LIST = "PH080140";
                public const string CORRECTION_TRANSACTION_INFORMATION = "PH080210";
                public const string RECIPE_HISTORY = "PH080220";
                public const string RECIPE_INFORMATION_PER_DRUG = "PH080230";
                public const string PAYMENT_INFORMATION = "PH080240";
                public const string DRUGS_LOGISTICS_PRICE = "PH080330";
                public const string TARIFF_ESTIMATION = "PH080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "PH080320";
                public const string PH_DATA_KUNJUNGAN_PASIEN_BPJS = "PH080411";
                public const string TARIFF_SERVICE_INFORMATION = "PH080200";

                public const string REPORT = "PH090000";

                public const string PH_BPJS_CLAIM_DIAGNOSA_PASIEN = "PH060151";
                public const string PH_BPJS_CLAIM_TINDAKAN_PASIEN = "PH060152";
                public const string PH_BPJS_CLAIM_DISPOSISI_PASIEN = "PH060153";
                public const string PH_BPJS_CLAIM_ENTRY = "PH060154";

                public const string PATIENT_PAGE_CHARM_BAR = "PH900000";
            }
            #endregion

            #region Reporting
            public static class Reporting
            {
                public const string RekamMedis = "RP010000";
                public const string RekamMedis_1 = "RP010100";
                public const string RekamMedis_1_1 = "RP010101";
            }
            #endregion

            #region System Setup
            public static class SystemSetup
            {
                public const string HEALTHCARE = "SA010100";
                public const string FACILITY = "SA010200";
                public const string CLINIC = "SA010300";
                public const string WARD = "SA010400";
                public const string DIAGNOSTIC_SUPPORT = "SA010500";
                public const string PHARMACY = "SA010600";
                public const string EMERGENCY = "SA010700";
                public const string MEDICAL_CHECKUP = "SA010800";
                public const string ROOM = "SA010900";
                public const string CLASS_CARE = "SA011000";
                public const string HEALTHCARE_PROFESSIONAL = "SA011100";
                public const string OPERATIONAL_TIME_HOUR = "SA011200";
                public const string EMPLOYEE = "SA011300";
                public const string BPJS_KABUPATEN = "SA011400";
                public const string BPJS_KECAMATAN = "SA011500";

                public const string MODULE = "SA050100";
                public const string MENU = "SA050200";
                public const string BUSINESS_OBJECT_CUSTOM_ATTRIBUTE = "SA050300";
                public const string TRANSACTION_TYPE = "SA050400";
                public const string STANDARD_CODE = "SA050500";
                public const string SYSTEM_PARAMETER = "SA050600";
                public const string HEALTHCARE_PARAMETER = "SA051400";
                public const string PER_HEALTHCARE_PARAMETER = "SA050602";
                public const string HOLIDAY = "SA050700";
                public const string ZIPCODES = "SA050800";
                public const string FILTER_PARAMETER = "SA050900";
                public const string REPORT_CONFIGURATION = "SA051000";
                public const string PRINTER_LOCATION = "SA051100";
                public const string PRINTER_LOCATION_PER_IP = "SA051200";
                public const string REPORT_CONFIGURATION_USER = "SA051300";
                public const string IP_DEFAULT_CONFIG = "SA051500";
                public const string IP_PHARMACY_UNIT = "SA051600";
                public const string IP_DEFAULT_CONFIG2 = "SA051600";

                public const string USER_ROLES = "SA060100";
                public const string USER_ACCOUNTS = "SA060200";

                public const string GENERATE_PASSWORD = "SA071200";

                public const string BPJS_REFERENCE_DIAGNOSA = "SA070201";
                public const string BPJS_REFERENCE_POLI = "SA070202";
                public const string BPJS_REFERENCE_FASILITAS_KESEHATAN = "SA070203";
                public const string BPJS_REFERENCE_DOKTER_DPJP = "SA070204";
                public const string BPJS_REFERENCE_PROPINSI = "SA070205";
                public const string BPJS_REFERENCE_KABUPATEN = "SA070206";
                public const string BPJS_REFERENCE_KECAMATAN = "SA070207";
                public const string BPJS_REFERENCE_PROCEDURE = "SA070208";
                public const string BPJS_REFERENCE_KELASRAWAT = "SA070209";
                public const string BPJS_REFERENCE_DOKTER = "SA070210";
                public const string BPJS_REFERENCE_SPESIALISTIK = "SA070211";
                public const string BPJS_REFERENCE_RUANGRAWAT = "SA070212";
                public const string BPJS_REFERENCE_CARAKELUAR = "SA070213";
                public const string BPJS_REFERENCE_PASCAPULANG = "SA070214";

                public const string SIRANAP_REFERENCE_RUANGAN_TEMPAT_TIDUR = "SA071501";
                public const string SIRANAP_REFERENCE_SDM = "SA071502";
                public const string SIRANAP_REFERENCE_ALKES_APD = "SA071503";

                public const string RESTORE_DATA_CONFIGURATION = "SA070101";
                public const string RESTORE_DATA = "SA070102";
                public const string DATA_MIGRATION_CONFIGURATION = "SA070301";
                public const string DATA_MIGRATION = "SA070302";
                public const string PERSON_NAME = "SA070402";
                public const string VIEW_ERROR_LOG = "SA070500";
                public const string VIEW_APIMESSAGE_LOG = "SA070600";
                public const string VIEW_RIS_APIMESSAGE_LOG = "SA071200";

                public const string VIEW_PATIENT_CHANGED_LOG = "SA071010";
                public const string VIEW_MRN_MERGE_LOG = "SA071020";

                public const string LIS_Wynacom = "SA071111";
                public const string LIS_HCLAB = "SA071112";

                public const string RIS_Infinit = "SA071131";

                public const string BPJS_AntrianOnline = "SA071122";
                public const string BPJS_AntrianOnline_Dashboard = "SA071123";

                public const string INHEALTH_REFERENCE_POLI = "SA071301";
                public const string INHEALTH_REFERENCE_PROVIDER_RUJUKAN = "SA071302";
                public const string INHEALTH_REFERENCE_TEMPAT_TIDUR = "SA071303";

                public const string CHANGE_LINKED_REGISTRATION = "SA072001";
                public const string CANCEL_PATIENT_DISCHARGE = "SA072002";
                public const string CHANGE_REGISTRATION_STATUS = "SA072003";
                public const string CHANGE_TEST_ORDER_SURGERY_REPORT = "SA072004";

                public const string INFORMATION_TRANSACTION_PATIENT = "SA080001";
                public const string INFORMATION_TRANSACTION_PATIENT_DETAIL = "SA080002";
                public const string INFORMATION_APLICARES = "SA080003";

                public const string REPORT = "SA090000";

                public const string LOCATION = "SA01111";

                public const string DTD = "SA01201";

                public const string DIAGNOSE = "SA01202";
                public const string ICD_BLOCK = "SA01203";
                public const string MORPHOLOGY = "SA01204";
                public const string PROCEDURE = "SA01205";
                public const string SPECIALTY = "SA01206";
                public const string FRACTION = "SA01207";
                public const string VISIT_TYPE = "SA01208";
                public const string VACCINATION_TYPE = "SA01209";
                public const string VITAL_SIGN = "SA01210";
                public const string SPECIMEN_SOURCE_CODE = "SA01211";
                public const string BODY_DIAGRAM = "SA01212";
                public const string SYMPTOM = "SA01213";
                public const string INSTRUCTION = "SA01214";
                public const string SIGNA = "SA01215";
                public const string NURSING_PROBLEM = "SA01222";
                public const string NURSING_DIAGNOSE = "SA01223";
                public const string NURSING_SUBJECTIVE = "SA01224";
                public const string NURSING_OBJECTIVE = "SA01225";

                public const string ATC_CLASSIFICATION = "SA01301";
                public const string MIMS_CLASS = "SA01302";
                public const string MANUFACTURER = "SA01303";
                public const string PRODUCT_BRAND = "SA01304";
                public const string Brodcast_Message = "SA071600";
                public const string ITEM_GROUP_MASTER = "SA01401";
                public const string PRODUCT_LINE = "SA01402";
                public const string ITEM_SERVICE = "SA01403";
                public const string DRUGS = "SA01404";
                public const string MEDICAL_SUPPLIES = "SA01405";
                public const string LOGISTICS = "SA01406";
                public const string LABORATORY = "SA01407";
                public const string IMAGING_TEST_ITEM = "SA01408";
                public const string OTHER_DIAGNOSTIC_ITEM = "SA01409";
                public const string ITEM_PACKAGE = "SA01410";
                public const string REVENUE_SHARING = "SA01411";
                public const string CUSTOMER = "SA01412";
                public const string CUSTOMER_CONTRACT = "SA01413";
                public const string SUPPLIER = "SA01414";
                public const string REFERRER = "SA01415";
                public const string TERM = "SA01416";
                public const string BANK = "SA01417";
                public const string EDC_MACHINE = "SA01418";
                public const string CREDIT_CARD_FEE = "SA01419";
                public const string MARKUP_MARGIN = "SA01420";
                public const string COVERAGE_TYPE = "SA01421";

                public const string CANNED_TEXT = "SA01503";
                public const string LOCATION_PERMISSION = "SA01504";

                public const string CREATE_TARIFF_BOOK = "SA03100";
                public const string PROPOSE_TARIFF_BOOK = "SA03200";
                public const string TARIFF_APPROVAL = "SA03300";
                public const string PROCESS_TARIFF_BOOK = "SA03400";

                public const string TRANSACTION_LOCK = "SA070900";
            }
            #endregion

            #region Radiotheraphy
            public static class Radiotheraphy
            {
                public const string ITEM_GROUP = "RT010100";
                public const string ITEM_SERVICE_RADIOTHERAPHY = "RT010201";
                public const string ITEM_SERVICE_MD_RADIOTHERAPHY = "RT010201";

                public const string PARAMEDIC_SCHEDULE = "RT040100";
                public const string APPOINTMENT = "RT040200";
                public const string GENERATE_APPOINTMENT = "RT040300";
                public const string REGISTRATION = "RT020200";

                public const string WORK_LIST = "RT030100";

                public const string PATIENT_PAGE = "RT030101";
                public const string PATIENT_TRANSACTION_PAGE_CHARGES = "RT030401";
                public const string TARIFF_TEST_ESTIMATION = "RT070600";


                public const string PATIENT_PAGE_CHARM_BAR = "RT900000";
                public const string PATIENT_PAGE_PENGKAJIAN_PASIEN = "RT035100";
                public const string PATIENT_PAGE_PEMERIKSAAN_FISIK = "RT035300";
                public const string PATIENT_PAGE_ASUHAN_KEPERAWATAN = "RT035400";
                public const string PATIENT_PAGE_CATATAN_TERINTEGRASI = "RT035600";
                public const string PATIENT_PAGE_MONITORING_EVALUASI = "RT035700";
                public const string PATIENT_PAGE_PROSES_DAN_UTILITAS = "RT035800";

                public const string PATIENT_PAGE_RT_NURSE_INITIAL_ASSESSMENT = "RT035101";
                public const string PATIENT_PAGE_RT_POPULATION_ASSESSMENT = "RT035102";
                public const string PATIENT_PAGE_RT_MEDICAL_ASSESSMENT_FORM = "RT035103";
                public const string PATIENT_PAGE_RT_FALL_RISK_ASSESSMENT_FORM = "RT035104";
                public const string PATIENT_PAGE_RT_PAIN_ASSESSMENT_FORM = "RT035105";
                public const string PATIENT_PAGE_RT_NUTRITION_SCREENING = "RT035106";
                public const string PATIENT_PAGE_RT_VERIFY_NUTRITION_SCREENING = "RT035107";
                public const string PATIENT_PAGE_RT_EWS_ASSESSMENT_FORM = "RT035108";

                public const string PATIENT_PAGE_RT_PATIENT_ALLERGY = "RT035201";
                public const string PATIENT_PAGE_RT_OBSGYN_HISTORY = "RT035202";
                public const string PATIENT_PAGE_RT_ANTENATAL_RECORD = "RT035203";
                public const string PATIENT_PAGE_RT_VACCINATION_HISTORY = "RT035204";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_CT_SIMULATION = "RT035206";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM = "RT035205";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_PROGRAM_BRACHYTHERAPY = "RT035207";
                public const string PATIENT_PAGE_RT_RADIOTHERAPHY_REPORT = "RT035208";

                public const string PATIENT_PAGE_RT_VITAL_SIGN = "RT035301";
                public const string PATIENT_PAGE_RT_REVIEW_OF_SYSTEM = "RT035302";

                public const string PATIENT_PAGE_RT_NURSING_ASSESSMENT_PROBLEM = "RT033210";
                public const string PATIENT_PAGE_RT_NURSING_ASSESSMENT_PROCESS = "RT033220";

                public const string PATIENT_PAGE_RT_DIAGNOSTIC_ORDER = "RT035501";

                public const string PATIENT_PAGE_RT_INTEGRATION_NOTES = "RT035601";
                public const string PATIENT_PAGE_RT_SUMMARY_INTEGRATION_NOTES = "RT035602";
                public const string PATIENT_PAGE_RT_PATIENT_DIAGNOSIS = "RT035603";
                public const string PATIENT_PAGE_RT_NURSING_NOTES = "RT035604";
                public const string PATIENT_PAGE_RT_PATIENT_HANDOVER = "RT035605";
                public const string PATIENT_PAGE_RT_PATIENT_HANDOVER_CONFIRMATION = "RT035606";
                public const string PATIENT_PAGE_RT_NURSING_NOTES_CONFIRMATION = "RT035607";

                public const string PATIENT_PAGE_RT_MONITORING_INTAKE_OUTPUT = "RT035701";
                public const string PATIENT_PAGE_RT_TEST_ORDER_RESULT_LIST = "RT035702";
                public const string PATIENT_PAGE_RT_PATIENT_MEDICAL_DEVICE = "RT035703";

                public const string PATIENT_PAGE_RT_CHANGE_ORDER_STATUS = "RT035901";
                public const string PATIENT_PAGE_RT_E_DOCUMENT = "RT035902";

                public const string RADIOTERAPHY_RESULT = "RT030200";
                public const string RADIOTERAPHY_RESULT_VERIFICATION = "RT030300";

                public const string BILL_SUMMARY = "RT060100";
                public const string BILL_SUMMARY_GENERATE_BILL = "RT060121";
                public const string BILL_SUMMARY_DETAIL = "RT060124";
                public const string BILL_SUMMARY_EDIT_COVERAGE_AMOUNT = "RT060128";
                public const string BILL_SUMMARY_VOID_BILL = "RT060141";
                public const string BILL_SUMMARY_RECALCULATION_BILL = "RT060142";
                public const string BILL_SUMMARY_DISCOUNT = "RT060122";
                public const string BILL_SUMMARY_DISCOUNT_DETAIL = "RT060129";

                public const string BILL_SUMMARY_CHARGES = "RT060111";
                public const string BILL_SUMMARY_CHARGES_EMERGENCY = "RT060112";
                public const string BILL_SUMMARY_CHARGES_OUTPATIENT = "RT060113";

                public const string BILL_SUMMARY_PAYMENT = "RT060123";
                public const string BILL_SUMMARY_PAYMENT_CASHIER = "RT060126";
                public const string BILL_SUMMARY_PAYMENT_AR = "RT060127";

                public const string BILL_SUMMARY_UPDATE_REGISTRATION = "RT060143";
                public const string BILL_SUMMARY_UPDATE_TRANSACTION_STATUS = "RT060144";
                public const string BILL_SUMMARY_OVER_LIMIT_CONFIRMATION = "RT060146";
                public const string BILL_SUMMARY_OUTSTANDING_TRANSACTION_ORDER_LIST = "RT060147";

                public const string BILL_SUMMARY_RECEIPT_PRINT = "RT060131";
                public const string BILL_SUMMARY_RECEIPT_PRINT_COMBINE = "RT060134";

                public const string BPJS_PROCESS_CLAIM_ORDER = "RT060155";

                public const string BILL_INFORMATION = "RT060191";
                public const string PENDING_TRANSACTION = "RT060192";
                public const string PAYMENT_TRANSACTION = "RT060193";
                public const string INFORMATION_CUSTOMER_PAYER = "RT060195";
                public const string INFORMATION_PATIENT_REGISTRATION_VISIT = "RT060196";
                public const string INFORMATION_VISIT_HISTORY = "RT060197";
                public const string INFORMATION_CHARGES_CLASS_CHANGE_HISTORY = "RT060198";
                public const string INFORMATION_DISCOUNT_DETAIL = "RT060199";

                public const string E_DOCUMENT = "RT060161";

                public const string INFORMATION_REGISTERED_PATIENT = "RT080110";
                public const string PRINTING_PATIENT_CARD = "RT080120";
                public const string APPOINTMENT_INFORMATION = "RT080150";
                public const string INFORMATION_VISIT_HISTORY_PATIENT_LIST = "RT080140";
                public const string TARIFF_ESTIMATION = "RT080310";
                public const string CUSTOMER_CONTRACT_INFORMATION = "RT080330";
                public const string CARI_DATA_SEP = "RT080410";
                public const string DATA_KUNJUNGAN_PASIEN_BPJS = "RT080411";

                public const string PATIENT_BILL_DETAIL_REPRINT = "RT070110";
                public const string PATIENT_BILL_DETAIL_REPRINT_2 = "RT070120";
                public const string PAYMENT_RECEIPT_REPRINT = "RT070130";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT = "RT070140";
                public const string PATIENT_PER_BILL_DETAIL_REPRINT_DETAIL = "RT070141";

                public const string OPEN_CLOSE_PATIENT_REGISTRATION = "RT070210";
                public const string CHANGE_PATIENT_TRANSACTION_STATUS = "RT070220";
                public const string CHANGE_PATIENT_BLACK_LIST_STATUS = "RT070240";
                public const string CLOSE_REGISTRATION = "RT070301";
                public const string VOID_REGISTRATION_DAILY = "RT070302";
                public const string VOID_REGISTRATION = "RT070303";

                public const string RT_RESULT_HISTORY = "RT070410";

                public const string REPORT = "RT090000";
            }

            #endregion
        }
        #endregion

        #region Standard Code
        public static class StandardCode
        {
            public const string MARITAL_STATUS = "0002";
            public const string GENDER = "0003";
            public const string ETHNIC = "0005";
            public const string RELIGION = "0006";
            public const string ADMISSION_TYPE = "0007";
            public const string ADMISSION_SOURCE = "0023";
            public const string ADMISSION_CONDITION = "0043";
            public const string FAMILY_RELATION = "0063";
            public const string EMPLOYMENT_STATUS = "0066";
            public const string BED_STATUS = "0116";
            public const string ALLERGEN_TYPE = "0127";
            public const string ALLERGY_SEVERITY = "0128";
            public const string MEDICATION_ROUTE_HL7 = "0162";
            public const string NATIONALITY = "0212";
            public const string PATIENT_OUTCOME = "0241";
            public const string DOCUMENT_TYPE = "0270";
            public const string PROVINCE = "0347";
            public const string ITEM_TYPE = "X001";
            public const string LABORATORY_RESULT_TYPE = "X002";
            public const string ITEM_UNIT = "X003";
            public const string CUSTOMER_TYPE = "X004";
            public const string TARIFF_SCHEME = "X005";
            public const string AGE_UNIT = "X008";
            public const string BLOOD_TYPE = "X009";
            public const string PATIENT_VISIT_NOTES = "X011";
            public const string OCCUPATION = "X012";
            public const string EDUCATION = "X013";
            public const string SALUTATION = "X014";
            public const string TITLE = "X015";
            public const string SUFFIX = "X016";
            public const string BUSINESS_OBJECT_TYPE = "X017";
            public const string HEALTHCARE_PROFESSIONAL_TYPE = "X019";
            public const string VISIT_STATUS = "X020";
            public const string BED_STATUS_REASON = "X328";
            public const string DIAGNOSIS_TYPE = "X029";
            public const string MEDICATION_ROUTE = "X030";
            public const string DIFFERENTIAL_DIAGNOSIS_STATUS = "X031";
            public const string HEALTHCARE_OPERATING_GROUP = "X033";
            public const string PAYMENT_TYPE = "X034";
            public const string PAYMENT_METHOD = "X035";
            public const string E_DOCUMENT_FILE_TYPE = "X036";
            public const string RESTRICTION_TYPE = "X038";
            public const string E_DOCUMENT_EKLAIM = "X527";

            public const string AIRWAY = "X042";
            public const string BREATHING = "X043";
            public const string CIRCULATION = "X049";
            public const string DISABILITY = "X050";
            public const string EXPOSURE = "X054";
            public const string ADMISSION_ROUTE = "X055";
            public const string PHYSICIAN_TEXT_TEMPLATE = "X058";

            public const string TOOTH = "X044";
            public const string TOOTH_PROBLEM = "X045";
            public const string TOOTH_STATUS = "X046";
            public const string TOOTH_SURFACES = "X047";
            public const string RL_CLASS = "X048";

            public const string DISCHARGE_ROUTINE = "X052";
            public const string DISCHARGE_REASON_TO_OTHER_HOSPITAL = "X219";
            public const string VACCINATION_ROUTE = "X059";
            public const string DIAGNOSIS_NUTRITION_SEVERITY = "X060";
            public const string NUTRITION_PHYSICAL_RATING = "X061";
            public const string DIAGNOSTIC_RESULT_INTERPRETATION = "X062";
            public const string ONSET = "X064";
            public const string QUALITY = "X065";
            public const string SEVERITY = "X066";
            public const string PATIENT_CATEGORY = "X067";
            public const string COURSE_TIMING = "X068";
            public const string EXACERBATED = "X069";
            public const string RELIEVED_BY = "X070";
            public const string LABORATORY_UNIT = "X072";
            public const string REFERRAL_TYPE = "X075";
            public const string NURSE_HANDS_OVER_TYPE = "X076";
            public const string TRIAGE = "X079";
            public const string VACCINATION_GROUP = "X080";
            public const string PARAMEDIC_ROLE = "X084";
            public const string FUNCTIONAL_TYPE = "X087";
            public const string PATIENT_EDUCATION_TYPE = "X088";
            public const string PSYCHOLOGY_STATUS = "X089";
            public const string RAPUH_RESISTENSI = "X090";
            public const string RAPUH_AKTIFITAS = "X092";
            public const string RAPUH_PENYAKIT = "X093";
            public const string RAPUH_USAHA_BERJALAN = "X094";
            public const string RAPUH_BERAT_BADAN = "X095";
            public const string RAPUH_SCORE = "X096";
            public const string IDENTITY_NUMBERY_TYPE = "X097";
            public const string REVIEW_OF_SYSTEM = "X098";
            public const string MST_WEIGHT_CHANGED = "X099";
            public const string MST_WEIGHT_CHANGED_GROUP = "X100";
            public const string MST_DIAGNOSIS = "X101";
            public const string CARD_TYPE = "X102";
            public const string VALUE_TYPE = "X103";
            public const string LABORATORY_TEST_CATEGORY = "X104";
            public const string REFERRAL = "X105";
            public const string REFERRER_GROUP = "X105";
            public const string REPORTING_PERIOD = "X106";
            public const string ADJUSTMENT_REASON = "X107";
            public const string FILTER_PARAMETER_TYPE = "X108";
            public const string ABC_CLASS = "X109";
            public const string MEDICAL_FILE_STATUS = "X111";
            public const string TEMPLATE_TEXT_GROUP = "X112";
            public const string OBJECTIVE_DATA_SOURCE = "X113";
            public const string BODY_DIAGRAM_GROUP = "X114";
            public const string BODY_DIAGRAM_SYMBOL = "X115";
            public const string ALLERGY_INFORMATION_SOURCE = "X116";
            public const string TRANSACTION_STATUS = "X121";
            public const string DRUG_FORM = "X122";
            public const string DRUG_CLASSIFICATION = "X123";
            public const string PREGNANCY_CATEGORY = "X124";
            public const string TO_BE_PERFORMED = "X125";
            public const string PATIENT_TRANSFER_TYPE = "X127";
            public const string DELETE_REASON = "X129";
            public const string DOSING_FREQUENCY = "X130";
            public const string BODY_PART_SYMPTOM_CHECKER = "X135";
            public const string DISCONTINUE_MEDICATION_REASON = "X136";
            public const string PRESCRIPTION_TYPE = "X137";
            public const string REFILL_INSTRUCTION = "X138";
            public const string PATIENT_INSTRUCTION_GROUP = "X139";
            public const string REPORT_TYPE = "X140";
            public const string DATA_SOURCE_TYPE = "X141";
            public const string CARD_PROVIDER = "X142";
            public const string MEDICAL_FOLDER_TYPE = "X144";
            public const string PURCHASE_ORDER_TYPE = "X145";
            public const string FRANCO_REGION = "X146";
            public const string CURRENCY_CODE = "X147";
            public const string BORN_CONDITION = "X148";
            public const string BIRTH_METHOD = "X149";
            public const string BIRTH_COMPLICATION_TYPE = "X150";
            public const string BIRTH_COD = "X151";
            public const string CAESAR_METHOD = "X152";
            public const string TWIN_SINGLE = "X153";
            public const string BORN_AT = "X154";
            public const string DISCOUNT_REASON = "X155";
            public const string VISIT_REASON = "X156";
            public const string CHARGES_TYPE = "X157";
            public const string REVENUE_SHARING_FORMULA_TYPE = "X158";
            public const string REVENUE_SHARING_COMPONENT = "X159";
            public const string PURCHASE_RETURN_TYPE = "X161";
            public const string PURCHASE_RETURN_REASON = "X162";
            public const string PATIENT_ATD_STATUS = "X163";
            public const string REVENUE_SHARING_ADJUSTMENT_GROUP = "X166";
            public const string REVENUE_SHARING_ADJUSTMENT_TYPE = "X167";
            public const string SHIFT = "X168";
            public const string CASHIER_GROUP = "X169";
            public const string RETURN_REASON = "X170";
            public const string COENAM_RULE = "X172";
            public const string ADJUSTMENT_TYPE = "X173";
            public const string CONSUMPTION_TYPE = "X174";
            public const string DIRECT_PURCHASE_TYPE = "X175";
            public const string SUPPLIER_CREDIT_NOTE_TYPE = "X176";
            public const string CHECK_COUNT_TYPE = "X177";
            public const string SUPPLIER_PAYMENT_METHOD = "X178";
            public const string PRESCRIPTION_RETURN_TYPE = "X179";
            public const string GLACCOUNT_TYPE = "X180";
            public const string ITEM_STATUS = "X181";
            public const string TIPE_PEMUSNAHAN = "X182";
            public const string ASSET_SALES_TYPE = "X183";
            public const string GL_TRANSACTION_GROUP = "X184";
            public const string GL_REVENUE_TRANSACTION = "X185";
            public const string SUPPLIER_TYPE = "X186";
            public const string NUTRIENT_UNIT = "X187";
            public const string JOURNAL_GROUP = "X188";
            public const string FOOD_GROUP = "X189";
            public const string MEAL_TIME = "X190";
            public const string GL_ACCOUNT_PAYABLE_TYPE = "X192";
            public const string EMPLOYEE_OCCUPATION = "X193";
            public const string DEPARTMENT = "X194";
            public const string EMPLOYEE_OCCUPATION_LEVEL = "X195";
            public const string MEAL_DATE = "X196";
            public const string NURSING_DOMAIN_CLASS_TYPE = "X197";
            public const string MEAL_STATUS = "X198";
            public const string HEALTHCARE_CLASS = "X199";
            public const string KDTARIF_INACBG = "X520";
            public const string MEAL_EVALUATION = "X063";
            public const string HOUSING_CONDITION = "X200";
            public const string PAIN_ASSESSMENT = "X201";
            public const string FALL_RISK_ASSESSMENT = "X202";
            public const string BPJS_PAYMENT_METHOD = "X205";
            public const string BPJS_SPECIAL_CMG_ADL = "X206";
            public const string QUANTITY_DEDUCTION_TYPE = "X207";
            public const string LOKASI_PENDAFTARAN = "X216";
            public const string TRANSACTION_TYPE = "X217";
            public const string STATUS_VEN = "X218";
            public const string ALASAN_PEMBATALAN_OBAT_RESEP = "X221";
            public const string JENIS_CATATAN_STATUS_RM = "X222";
            public const string BILLING_GROUP_DISPLAY_BY = "X223";
            public const string STOCK_ITEM_STATUS = "X224";
            public const string HEALTHCARE_UNIT = "X226";
            public const string LOCATION_GROUP = "X227";
            public const string TEST_PARTNER_TYPE = "X230";
            public const string PATIENT_TRANSFER_REASON = "X234";
            public const string MEDICAL_IMAGING_MODALITIES = "X235";
            public const string RECEIPT_REPRINT_REASON = "X236";
            public const string MEAL_FORM = "X238";
            public const string DIET_TYPE = "X239";
            public const string PRINTER_TYPE = "X240";

            public const string SPEECH_CONTACT = "X241";
            public const string CONSCIOUSNESS = "X242";
            public const string ORIENTATION_TIME = "X243";
            public const string ORIENTATION_PLACE = "X244";
            public const string ORIENTATION_PERSON = "X245";
            public const string MOODY_LEVEL = "X246";
            public const string THOUGHT_PROCESS_FORM = "X247";
            public const string THOUGHT_PROCESS_CONTENT = "X248";
            public const string THOUGHT_PROCESS_FLOW = "X249";
            public const string PERCEPTION = "X250";
            public const string VOLITION = "X251";
            public const string PSYCHMOTOR = "X252";
            public const string PATIENT_ATD_STATUS_GROUP = "X253";
            public const string AR_PAYMENT_METHOD = "X254";
            public const string PATIENT_STATUS = "X256";
            public const string CHARGES_CHANGE_STATUS_REASON = "X257";
            public const string UDD_MEDICATION_STATUS = "X258";
            public const string JOURNAL_PROCESS_STATUS = "X261";
            public const string PHYSICIAN_INSTRUCTION_SOURCE = "X262";
            public const string OPERATION_LOCATION_SIGN = "X274";
            public const string WOUND_CLASIFICATION = "X275";
            public const string ANESTHESIA_COMPLICATION = "X276";
            public const string ANESTHESIA_TYPE = "X277";
            public const string DELAY_OPERATION_REASON = "X278";
            public const string CLINIC_GROUP = "X280";
            public const string PARAMEDIC_LEAVE_REASON = "X281";
            public const string PPH_TYPE = "X283";

            public const string BPJS_SEP_Status = "X284";
            public const string CARA_KELUAR_BPJS = "X286";
            public const string KEADAAN_PULANG_BPJS = "X287";
            public const string TINDAK_LANJUT_BPJS = "X288";
            public const string VIP_PATIENT_GROUP = "X289";
            public const string COMINICATION = "X368";
            public const string TREASURY_GROUP = "X290";
            public const string TREASURY_TYPE = "X291";
            public const string NATION_GROUP = "X292";
            public const string TIPE_RUJUKAN_BPJS = "X293";
            public const string TIPE_MASTER_BPJS = "X294";
            public const string DELETE_REASON_APPOINTMENT = "X296";
            public const string PATIENT_BLACKLIST_REASON = "X297";
            public const string TAX_REVENUE_FORMULA_TYPE = "X309";
            public const string BANK_TYPE = "X310";
            public const string CONTRACT_DOCUMENT = "X311";
            public const string PATIENT_NOTE_TYPE = "X312";
            public const string MEDICAL_RECORD_CLASS_GROUP = "X314";
            public const string MEDICAL_RECORD_FLOOR_GROUP = "X315";
            public const string PATOLOGY_GROUP = "X316";
            public const string ROOM_TYPE = "X317";
            public const string SPECIALTY_GROUP = "X318";
            public const string CONTENT_CASH_FLOW_GROUP = "X319";
            public const string SUPPLIER_DOCUMENT_TYPE = "X230";
            public const string SERVICE_STATUS = "X321";
            public const string SERVICE_TYPE = "X322";
            public const string PACKAGING_TYPE = "X323";
            public const string STERILITATION_TYPE = "X324";
            public const string WASHING_METHOD = "X325";

            public const string PREWASHING_VOLUME = "X329";
            public const string PREWASHING_CONDITION = "X330";

            public const string EXTRENAL_MCU_RESULT_1 = "X331";
            public const string EXTRENAL_MCU_RESULT_2 = "X332";
            public const string EXTRENAL_MCU_RESULT_3 = "X333";
            public const string EXTRENAL_MCU_RESULT_4 = "X334";
            public const string EXTRENAL_MCU_RESULT_5 = "X335";
            public const string EXTRENAL_MCU_RESULT_6 = "X336";
            public const string EXTRENAL_MCU_RESULT_7 = "X337";
            public const string EXTRENAL_MCU_RESULT_8 = "X338";
            public const string EXTRENAL_MCU_RESULT_9 = "X339";
            public const string NUTRITION_DISRUPTION_STATUS = "X340";
            public const string DISEASE_SEVERITY = "X341";
            public const string MNA_SCREENING_A = "X342";
            public const string MNA_SCREENING_B = "X343";
            public const string MNA_SCREENING_C = "X344";
            public const string MNA_SCREENING_E = "X345";
            public const string MNA_SCREENING_F = "X346";
            public const string MNA_ASSESSMENT_J = "X347";
            public const string MNA_ASSESSMENT_M = "X348";
            public const string MNA_ASSESSMENT_N = "X349";
            public const string MNA_ASSESSMENT_O = "X350";
            public const string MNA_ASSESSMENT_P = "X351";
            public const string MNA_ASSESSMENT_Q = "X352";
            public const string EXTRENAL_MCU_RESULT_10 = "X353";
            public const string EXTRENAL_MCU_RESULT_11 = "X354";
            public const string EXTRENAL_MCU_RESULT_12 = "X355";
            public const string DIET_ROUTE = "X356";
            public const string SURGERY_CLASSIFICATION = "X357";

            public const string PLAN_DISCHARGE_NOTES_TYPE = "X085";
            public const string APPOINTMENT_METHOD = "X359";

            public const string INHEALTH_REFERENCES = "X361";

            public const string CLINIC_PAUSE_REASON = "X364";
            public const string CLINIC_STATUS = "X365";
            public const string TIPE_TRANSAKSI_BPJS = "X366";
            public const string LANGUAGE = "X367";
            public const string NURSING_EVALUATION = "X369";

            public const string PATIENT_OWNER_STATUS = "X379";

            public const string OPERATIONAL_TYPE = "X382";
            public const string FALL_RISK_INTERPRETATION = "X383";
            //public const string BUDGET_CATEGORY = "X384";
            public const string FA_ITEM_MOVEMENT_TYPE = "X385";
            public const string PATIENT_ADDITIONAL_ASSESSMENT_FORM = "X386";
            public const string PATIENT_FISIOTERAPI_FORM = "X388";
            public const string JOURNAL_TEMPLATE_TYPE = "X389";
            public const string REVENUE_REDUCTION = "X390";
            public const string REVENUE_PAYMENT_METHOD = "X391";
            public const string REVENUE_PERIODE_TYPE = "X392";
            public const string PURCHASING_BUDGET_CATEGORY = "X393";
            public const string PURCHASING_TYPE = "X394";
            public const string ASSESSMENT_STATUS = "X395";
            public const string EXTRENAL_MCU_RESULT_13 = "X396";
            public const string PAIN_SCALE_INTERPRETATION = "X398";
            public const string PAIN_REGIO = "X399";
            public const string PATIENT_ASSESSMENT_FORM = "X401";
            public const string FORM_PENGKAJIAN_KAMAR_OPERASI = "X457";
            public const string FORM_PENGKAJIAN_HEMODIALISA = "X458";
            public const string FORM_PENGKAJIAN_KAMAR_BERSALIN = "X491";
            public const string FORM_PENGKAJIAN_GIZI = "X492";

            public const string NURSING_DIAGNOSIS_TYPE = "X406";
            public const string PROMOTION_TYPE = "X415";
            public const string CLOSED_PO_REASON = "X416";

            public const string MEAL_PLAN_CATEGORY = "X417";
            public const string FINAL_CLAIM_FEEDBACK_STATUS = "X418";

            public const string NOC_Interval_Period = "X431";

            public const string EXTRENAL_MCU_RESULT_14 = "X432";
            public const string EXTRENAL_MCU_RESULT_15 = "X433";
            public const string EXTRENAL_MCU_RESULT_16 = "X434";
            public const string EXTRENAL_MCU_RESULT_17 = "X435";
            public const string EXTRENAL_MCU_RESULT_18 = "X436";
            public const string EXTRENAL_MCU_RESULT_19 = "X437";
            public const string EXTRENAL_MCU_RESULT_20 = "X438";
            public const string EXTRENAL_MCU_RESULT_21 = "X439";
            public const string EXTRENAL_MCU_RESULT_22 = "X440";
            public const string EXTRENAL_MCU_RESULT_23 = "X441";
            public const string EXTRENAL_MCU_RESULT_24 = "X442";

            public const string MCU_RESULT_TYPE = "X446";
            public const string BPJS_CLAIM_TYPE = "X447";
            public const string SURGERY_TEAM_ROLE = "X451";
            public const string JENIS_PEMBEDAHAN = "X452";

            public const string ITTER_STATUS = "X454";
            public const string JENIS_PREMEDIKASI = "X456";

            public const string JENIS_INTAKE = "X460";
            public const string JENIS_OUTPUT = "X461";
            public const string JENIS_INTAKE_TIDAK_DIUKUR = "X471";

            public const string JENIS_VAKSINASI_COVID_19 = "X476";

            public const string eHAC_TEST_TYPE = "X478";

            public const string JENIS_PERESEPAN_HD = "X479";
            public const string TEKNIK_HD = "X480";
            public const string JENIS_DIALISER = "X481";
            public const string JENIS_DIALISAT = "X482";
            public const string PAYMENT_GATEWAY_METHOD = "X483";

            public const string VIRTUAL_PAYMENT_CHANNEL = "X485";
            public const string SATUAN_PEMBILASAN_NACL = "X486";

            public const string MOVING_CLASSIFICATION = "X490";

            public const string NURSING_INTERVENTION_ITEM_TYPE = "X493";
            public const string BPJS_PAYER_SEP = "X496";
            public const string BPJS_KELAS_NAIK = "X497";
            public const string PHYSICAL_LIMITATION_TYPE = "X498";

            public const string REGIONAL_ANESTHESIA_TYPE = "X500";


            public const string VISIT_CASE_TYPE = "X502";

            public const string TUJUAN_KUNJUNGAN_BPJS = "X503";
            public const string PROSEDUR_BPJS = "X504";
            public const string PROSEDUR_PENUNJANG_BPJS = "X505";
            public const string ASESMEN_PELAYANAN_BPJS = "X506";

            public const string API_PARTY = "X509";
            public const string EDUCATION_UNDERSTANDING_LEVEL = "X511";
            public const string EDUCATION_METHOD = "X512";
            public const string EDUCATION_MATERIAL = "X513";
            public const string EDUCATION_EVALUATION = "X514";
            public const string EDUCATION_FORM = "X515";
            public const string ANESTHESIA_RISK = "X517";
            public const string DISCHARGE_TRANSPORTATION = "X521";
            public const string TIPE_PENYAKIT_INFEKSI = "X522";
            public const string TIPE_KOMORBID = "X523";
            public const string MODE_VENTILATOR = "X524";
            public const string MONITORING_DEVICE_TYPE = "X525";
            public const string BLOOD_BANK_TYPE = "X532";
            public const string BLOOD_BANK_SOURCE_TYPE = "X533";
            public const string BLOOD_BANK_USAGE_TYPE = "X534";
            public const string BLOOD_BANK_PAYMENT_TYPE = "X535";
            public const string JENIS_TABUNG_SAMPEL = "X539";
            public const string JENIS_CATATAN_PERAWAT = "X540";
            public const string KUALITAS_KANTONG_DARAH = "X541";
            public const string KUALITAS_DARAH = "X542";
            public const string MEDICATION_STORAGE = "X543";

            public const string CARA_KELUAR_EKLAIM = "X545";
            public const string RESULT_DELIVERY_PLAN = "X546";
            public const string KATEGORI_ANTIBIOTIK = "X547";
            public const string VACCINATION_CVX_GROUP = "X548";
            public const string VACCINATION_CVX_NAME = "X549";

            public const string DISCOUNT_REASON_CHARGESDT = "X550";
            public const string TIPE_PERTANYAAN_SURVEY = "X553";

            public const string CARA_BAYAR_EKLAIM = "X555";
            public const string COB_EKLAIM = "X556";
            public const string CARA_MASUK_EKLAIM = "X557";

            public const string EWS_ASSESSMENT = "X558";
            public const string LMP_PERIOD = "X560";

            public const string RESIKO_KEHAMILAN = "X564";
            public const string AIR_KETUBAN = "X565";
            public const string PENYUSUPAN = "X566";
            public const string DURASI_KONTRAKSI = "X567";
            public const string FREKUENSI_KONTRAKSI = "X568";

            public const string INCBGS_CLASS = "X570";
            public const string BEAM_TECHNIQUE = "X572";
            public const string BEAM_PESAWAT = "X573";
            public const string BEAM_RAD = "X574";
            public const string BEAM_ACCESS = "X575";
            public const string BEAM_SETUP = "X576";

            public const string PARAMEDIC_TEXT_TEMPLATE = "X577";

            public const string PATIENT_CONSENT_FORM_GROUP = "X578";
            public const string PATIENT_CONSENT_FORM_TYPE = "X579";

            public const string RADIOTHERAPY_TYPE = "X582";
            public const string RADIOTHERAPY_PURPOSE = "X583";
            public const string BRACHYTHERAPY_TYPE = "X584";
            public const string APPLICATOR_TYPE = "X585";
            public const string RADIOTHERAPY_PLAN = "X586";
            public const string RADIOTHERAPY_VERIFICATION = "X587";
            public const string SIMULATION_REQUEST_TYPE = "X588";
            public const string SIMULATION_SCAN_AREA = "X589";
            public const string SIMULATION_ORIENTASI_POSISI = "X590";
            public const string CANCER_STADIUM = "X591";
            public const string SIMULATION_POSISI_PASIEN = "X592";
            public const string SIMULATION_POSISI_TANGAN = "X593";
            public const string SIMULATION_ALAT_BANTU = "X594";
            public const string ENERGY = "X596";
            public const string INTRAUTERINE_LENGTH = "X598";
            public const string INTRAUTERINE_CORNER = "X599";
            public const string CYLINDER = "X600";
            public const string HEMORRHAGE = "X601";

            public const string RL_REPORT_GROUP_3_12 = "X659";
            public const string RL_REPORT_GROUP_3_19 = "X660";

            public const string BIRTH_FROM_HIV_MOTHER = "X663";
            public const string BIRTH_FROM_SYPHILIS_MOTHER = "X664";
            public const string BIRTH_FROM_HEPATITIS_MOTHER = "X665";
            public const string TODDLER_NUTRITION_PROBLEM = "X666";
            public const string PARTUM_DEATH_TYPE = "X667";
            public const string NEONATAL_PERINATAL_DEATH_TYPE = "X668";
            public const string REFERRAL_LETTER = "X669";

            public const string KOMPLIKASI_NON_OBSTETRI = "X685";

            #region QRCode Format
            public static class QRCodeFormat
            {
                public const string format_01 = "X528^001";
                public const string format_02 = "X528^002";
                public const string format_03 = "X528^003";
            }
            #endregion

            public static class MedicationStorage
            {
                public const string MANAGED_BY_PATIENT = "X543^001";
                public const string MANAGED_BY_PHARMACY = "X543^002";
            }

            public static class InfectiousDisease
            {
                public const string COVID19 = "X522^001";
                public const string HEPATITISB = "X522^002";
                public const string HIV = "X522^003";
                public const string TBC = "X522^004";
                public const string OTHERS = "X522^999";
            }

            public static class Comorbidities
            {
                public const string GANGGUAN_PERNAPASAN = "X523^001";
                public const string DIABETES = "X523^002";
                public const string HIPERTENSI = "X523^003";
                public const string PENYAKIT_KARDIOVASKULAR = "X523^004";
                public const string PENYAKIT_JANTUNG = "X523^005";
                public const string OBESITAS = "X523^006";
                public const string DEPRESI_KECEMASAN = "X523^007";
                public const string KANKER_KELAINAN_DARAH = "X523^008";
                public const string KEKEBALAN_TUBUH_LEMAH = "X523^009";
                public const string PENYAKIT_GINJAL = "X523^010";
                public const string OTHERS = "X523^999";
            }

            public static class VIRTUAL_PAYMENT
            {
                public const string VA_BCA = "X485^001";
                public const string VA_MANDIRI = "X485^002";
                public const string VA_BRI = "X485^003";
                public const string SHOPEEPAY = "X485^004";
                public const string OVO = "X485^005";
                public const string QRIS = "X485^007";
            }

            public static string EDC = "X494";
            public static class EDCVendor
            {
                public const string BCA = "X494^01";
                public const string MTI = "X494^02";

            }

            public static class Physical_Limitation
            {
                public const string PARAPLEGIA = "X498^001";
                public const string CEREBRAL_PALSY = "X498^002";
                public const string DWARFISM = "X498^003";
                public const string DISABILITAS_RUNGU = "X498^004";
                public const string DISABILITAS_NETRA = "X498^005";
            }
        }

        public static class Covid19Vaccination
        {
            public const string ASTRAZENECA = "X476^001";
            public const string MODERNA = "X476^002";
            public const string NOVAVAX = "X476^003";
            public const string PFIZER = "X476^004";
            public const string SINOPHARM = "X476^005";
            public const string SINOVAC = "X476^006";
        }

        public static class MovingClassification
        {
            public const string FAST = "X490^001";
            public const string MEDIUM = "X490^002";
            public const string SLOW = "X490^003";
        }

        public static class AppointmentRequestMethod
        {
            public const string RUJUKAN_KUNJUNGAN_LANGSUNG = "0279^001";
            public const string RUJUKAN_APPOINTMENT = "0279^002";
            public const string MOBILE_APPS = "0279^003";
            public const string APPOINTMENT_REQUEST_MCU = "0279^004";
        }

        public static class ItemUnit
        {
            public const string PCS = "X003^PCS";
            public const string X = "X003^X";
        }

        public static class BPJSTransactionType
        {
            public const string PAKET = "X366^01";
            public const string DITAGIHKAN = "X366^02";
            public const string DIBAYAR_PASIEN = "X366^03";
        }

        public static class BankType
        {
            public const string BANK_KASIR = "X310^001";
            public const string BANK_HUTANG = "X310^002";
            public const string BANK_PIUTANG = "X310^003";
        }

        public static class BPJS_Version_Release
        {
            public const string v1_0 = "X484^001";
            public const string v2_0 = "X484^002";
        }

        public static class GLAccountType
        {
            public const string AKTIVA = "X180^001";
            public const string KEWAJIBAN = "X180^002";
            public const string MODAL = "X180^003";
            public const string PENDAPATAN = "X180^004";
            public const string HPP = "X180^005";
            public const string BEBAN_USAHA = "X180^006";
            public const string BIAYA_LANGSUNG = "X180^007";
            public const string PENDAPATAN_BEBAN_LAIN = "X180^008";
        }

        public static class AppointmentDeleteReason
        {
            public const string PARAMEDIC = "X296^001";
            public const string BY_SYSTEM = "X296^998";
            public const string OTHER = "X296^999";
        }

        public static class POClosedReason
        {
            public const string KETERLAMBATAN = "X416^001";
            public const string OTHER = "X416^999";
        }

        public static class TreasuryType
        {
            public const string PENERIMAAN = "X291^001";
            public const string PENGELUARAN = "X291^002";
            public const string PINDAH_BUKU = "X291^003";
            public const string PERMINTAAN_KAS_BON = "X291^004";
            public const string REALISASI_KAS_BON = "X291^005";
        }

        public static class TreasuryGroup
        {
            public const string SUPPLIER_PAYMENT = "X290^001";
            public const string AR_RECEIVING = "X290^002";
            public const string MEMORIAL = "X290^003";
            public const string CASH_ADVANCE = "X290^004";
            public const string DIRECT_PURCHASE = "X290^005";
            public const string REVENUE_SHARING = "X290^006";
            public const string KAS_BON = "X290^007";
            public const string SURAT_PERINTAH_KERJA = "X290^008";
            public const string PERMINTAAN_PEMBELIAN_TUNAI = "X290^009";
            public const string REALISASI_PEMBELIAN_TUNAI = "X290^010";
            public const string SETORAN_KASIR = "X290^011";
            public const string SETORAN_KASIR_REKONSILIASI = "X290^012";
        }

        public static class PPHType
        {
            public const string PPH_1 = "X283^01";
            public const string PPH_2 = "X283^02";
            public const string PPH_3 = "X283^03";
            public const string PPH_4 = "X283^04";
        }

        public static class VIPPatientGroup
        {
            public const string VIP_1 = "X289^001";
            public const string VIP_2 = "X289^002";
            public const string VIP_3 = "X289^003";
            public const string VIP_OTHER = "X289^999";
        }

        public static class ClinicGroup
        {
            public const string CLINIC_GROUP_BPJS = "X280^001";
            public const string CLINIC_GROUP_NON_BPJS = "X280^002";
        }

        public static class PatientOutcome
        {
            public const string SEHAT_ATAU_NORMAL = "0241^001";
            public const string MEMBAIK = "0241^002";
            public const string TIDAK_SEMBUH = "0241^003";
            public const string DEAD_BEFORE_48 = "0241^004";
            public const string DEAD_AFTER_48 = "0241^005";
            public const string BELUM_SEMBUH = "0241^006";
        }

        public static class SupplierPaymentMethod
        {
            public const string TUNAI = "X178^001";
            public const string TRANSFER = "X178^002";
            public const string GIRO = "X178^003";
            public const string CHEQUE = "X178^004";
            public const string CREDIT_CARD = "X178^005";
            public const string DEBIT_CARD = "X178^006";
            public const string KOREKSI_FAKTUR = "X178^007";
            public const string ADMIN = "X178^008";
            public const string DISCOUNT = "X178^009";
        }

        public static class DifferentialDiagnosisStatus
        {
            public const string CONFIRMED = "X031^001";
            public const string UNDER_INVESTIGATION = "X031^002";
            public const string RULED_OUT = "X031^003";
        }

        public static class QuantityDeductionType
        {
            public const string DIBULATKAN = "X207^001";
            public const string SESUAI_TRANSAKSI = "X207^002";
        }

        public static class ReturnReason
        {
            public const string BERKAS_KURANG_LENGKAP = "X170^001";
            public const string LAIN_LAIN = "X170^999";
        }

        public static class RevenueSharingComponent
        {
            public const string PARAMEDIC = "X159^999";
        }

        public static class RefillInstruction
        {
            public const string DET_ORIG = "X454^001";
            public const string DET = "X454^002";
            public const string DETUR = "X454^003";
            public const string NEDET = "X454^004";
            public const string NO_REFILL = "X454^999";
        }

        public static class RevenueSharingAdjustmentGroup
        {
            public const string PENAMBAHAN = "X166^001";
            public const string PENGURANGAN = "X166^002";
        }

        public static class RevenueSharingAdjustmentType
        {
            public const string JUMLAH_PENAMBAHAN = "X167^001";
            public const string POTONGAN_PEMBELIAN_OBAT = "X167^101";
            public const string INTENSIF_RS = "X167^102";
            public const string POTONGAN_MAJALAH = "X167^103";
            public const string HNPK_HONOR_TETAP = "X167^301";
            public const string HNJM_SUBSIDI_JASMED_MINIMAL = "X167^302";
            public const string KEKURANGAN_PERHITUNGAN = "X167^401";
            public const string KELEBIHAN_PERHITUNGAN = "X167^501";
            public const string POTONGAN_PIUTANG = "X167^502";
            public const string FONDS = "X167^508";
            public const string PPH_21 = "X167^509";
            public const string PENYESUAIAN_PAJAK_PENAMBAHAN = "X167^701";
            public const string PENYESUAIAN_PAJAK_PENGURANGAN = "X167^702";
        }

        public static class ParamedicRole
        {
            public const string DPJP = "X084^001";
            public const string KONSULEN = "X084^002";

            public const string PELAKSANA = "X084^001";
            public const string PENGIRIM = "X084^002";
            public const string PEMERIKSA = "X084^003";
            public const string PENGAWAS = "X084^004";

            // RSSBB Version
            public const string DPJP_UTAMA = "X084^001";
            public const string DPJP_KONSUL = "X084^002";
            public const string DOKTER_JAGA = "X084^003";
            public const string LAIN_LAIN = "X084^004";
        }

        public static class TemplateTextType
        {
            public const string IMAGING = "IMAGING";
            public const string LABORATORY = "LABORATORY";
            public const string DIAGNOSTIC = "DIAGNOSTIC";
        }

        public static class DiagnoseType
        {
            public const string EARLY_DIAGNOSIS = "X029^000";
            public const string MAIN_DIAGNOSIS = "X029^001";
            public const string COMPLICATION = "X029^002";
            public const string EXTERNAL_CAUSE = "X029^003";
        }

        public static class MedicalFileStatus
        {
            public const string CHECK_OUT = "X111^001";
            public const string RETURN_TO_BIN = "X111^002";
            public const string PROCESSED = "X111^003";
            public const string RETURN_TO_PHYSICIAN = "X111^004";
        }

        public static class TemplateGroup
        {
            public const string CHIEF_COMPLAINT = "X112^001";
            public const string REVIEW_SYSTEM = "X112^002";
            public const string IMAGING = "X112^003";
            public const string LABORATORY = "X112^004";
            public const string DIAGNOSTIC = "X112^005";
        }

        public static class AdmissionType
        {
            public const string ACCIDENT = "0007^A";
            public const string ELECTIVE = "0007^C";
            public const string EMERGENCY = "0007^E";
            public const string LABOR_AND_DELIVERY = "0007^L";
            public const string NEWBORN = "0007^N";
            public const string ROUTINE = "0007^R";
            public const string URGENT = "0007^U";
        }

        public static class AdmissionSource
        {
            public const string ADMITTING = "0023^A";
            public const string FINAL = "0023^F";
            public const string WORKING = "0023^W";
            public const string EMERGENCY = "0023^X01";
            public const string OUTPATIENT = "0023^X02";
            public const string MEDICAL_DIAGNOSTIC = "0023^X03";
            public const string INPATIENT = "0023^X04";
        }

        public static class AdmissionCondition
        {
            public const string DEATH_ON_ARRIVAL = "0043^006";
        }

        public static class DischargeMethod
        {
            public const string ATAS_PERSETUJUAN = "X052^001";
            public const string REFFERRED_TO_OUTPATIENT = "X052^002";
            public const string REFFERRED_TO_EXTERNAL_PROVIDER = "X052^003";
            public const string GO_HOME_BY_OWN_REQUEST = "X052^004";
            public const string RUNAWAY = "X052^005";
            public const string DISCHARGED_TO_WARD = "X052^007";
            public const string DISCHARGED_TO_MORTUARY = "X052^008";
            public const string TRANSFERED_TO_UPH = "X052^009";
            public const string TRANSFERED_TO_ODS = "X052^010";
        }

        public static class DosingFrequency
        {
            public const string HOUR = "X130^001";
            public const string DAY = "X130^002";
            public const string WEEK = "X130^999";

        }

        public static class PatientInstructionGroup
        {
            public const string MEDICATION = "X139^001";
            public const string WOUND_CARE = "X139^002";
            public const string DIET_EXCERCISE = "X139^005";
            public const string MEDICATION_ORDER_EXTEND = "X139^006";
            public const string MEDICATION_ORDER_STOP = "X139^007";
            public const string MEDICATION_ORDER_CHANGE_SIGNA = "X139^008";
            public const string HYGIENE = "X139^003";
            public const string OTHER = "X139^999";

        }

        public static class AllergenFindingSource
        {
            public const string PATIENT = "X116^001";
            public const string CLINICAL_DOCUMENT = "X116^002";
            public const string ACCIDENT_AT_HOSPITAL = "X116^003";
        }

        public static class AllergySeverity
        {
            public const string MILD = "0128^MI";
            public const string MODERATE = "0128^MO";
            public const string SEVERE = "0128^SV";
            public const string UNKNOWN = "0128^U";
        }

        public static class AllergenType
        {
            public const string ANIMAL = "0127^AA";
            public const string DRUG = "0127^DA";
            public const string ENVIRONMENTAL = "0127^EA";
            public const string FOOD = "0127^FA";
            public const string POLLEN = "0127^LA";
            public const string MISCELLANEOUS = "0127^MA";
            public const string PLANT = "0127^Plant";
        }

        public static class CustomerType
        {
            public const string INSURANCE = "X004^100";
            public const string HEALTHCARE = "X004^400";
            public const string PERSONAL = "X004^999";
            public const string BPJS = "X004^500";
        }

        public static class TestOrderStatus
        {
            public const string OPEN = "X126^001";
            public const string RECEIVED = "X126^002";
            public const string IN_PROGRESS = "X126^003";
            public const string CANCELLED = "X126^004";
            public const string COMPLETED = "X126^005";
            public const string CLOSED = "X126^006";
        }

        public static class OrderStatus
        {
            public const string OPEN = "X126^001";
            public const string RECEIVED = "X126^002"; // RECEIVED | SCHEDULLED (Operating Room)
            public const string IN_PROGRESS = "X126^003";
            public const string CANCELLED = "X126^004";
            public const string COMPLETED = "X126^005";
            public const string CLOSED = "X126^006";
        }

        public static class JournalGroup
        {
            public const string PENDAPATAN_PENERIMAAN = "X188^001";
            public const string HUTANG_PIUTANG = "X188^002";
            public const string INVENTORY = "X188^003";
            public const string PHARMACY = "X188^004";
            public const string FIXED_ASSET = "X188^005";
            public const string MEMORIAL = "X188^006";
        }

        public static class VisitStatus
        {
            public const string OPEN = "X020^001";
            public const string CHECKED_IN = "X020^002";
            public const string RECEIVING_TREATMENT = "X020^003";
            public const string PHYSICIAN_DISCHARGE = "X020^004";
            public const string DISCHARGED = "X020^005";
            public const string CANCELLED = "X020^006";
            public const string CLOSED = "X020^007";
            public const string TRANSFERRED = "X020^008";
        }

        public static class ReferralDischargeReason
        {
            public const string FASILITAS = "X219^001";
            public const string APS = "X219^002";
            public const string PENUH = "X219^003";
            public const string LAINNYA = "X219^999";
        }

        public static class BusinessObjectType
        {
            public const string PATIENT = "X017^001";
            public const string CUSTOMER = "X017^002";
            public const string SUPPLIER = "X017^003";
            public const string ITEM = "X017^004";
            public const string USER = "X017^005";
            public const string RUJUKAN_DARI_PIHAK_KETIGA = "X017^006";
            public const string RUJUKAN_KE_PIHAK_KETIGA = "X017^007";
            public const string REGISTRATION_BPJS = "X017^008";
            public const string PURCHASE_ORDER = "X017^013";
            public const string REGISTRATION = "X017^014";
            public const string VISIT = "X017^015";
        }

        public static class ReviewOfSystem
        {
            public const string ABDOMEN = "X098^012";
            public const string KEPALA = "X098^025";
            public const string LEHER = "X098^026";
            public const string DADA_PUNGGUNG = "X098^027";
            public const string PERUT = "X098^028";
            public const string PELVIS = "X098^029";
            public const string EKSTREMITAS = "X098^030";
            public const string MULUT = "X098^031";
            public const string STATUS_DERMATOLOGIKUS = "X098^032";
            public const string NEUROLOGIS = "X098^033";
            public const string STATUS_VENEREOLOGIKUS = "X098^034";
        }

        public static class MSTWeightChanged
        {
            public const string TIDAK_ADA_PENURUNAN_BB = "X099^01";
            public const string TIDAK_YAKIN = "X099^02";
        }

        public static class MSTWeightChangedGroup
        {
            public const string SATU_SAMPAI_LIMA_KG = "X100^01";
            public const string ENAM_SAMPAI_SEPULUH_KG = "X100^02";
            public const string SEBELAS_SAMPAI_LIMABELAS_KG = "X100^03";
            public const string LEBIH_DARI_LIMABELAS_KG = "X100^04";
        }

        public static class MedicationRoute
        {
            public const string APPLY_EXTERNALLY = "0162^AP";
            public const string BUCCAL = "0162^B";
            public const string DENTAL = "0162^DT";
            public const string EPIDURAL = "0162^EP";
            public const string ENDOTRACHIAL_TUBE = "0162^ET";
            public const string ORAL = "X030^001";
            public const string OTHER = "X030^999";
        }

        public static class ItemGroupMaster
        {
            public const string SERVICE = "X001^001";
            public const string DRUGS = "X001^002";
            public const string SUPPLIES = "X001^003";
            public const string LABORATORY = "X001^004";
            public const string RADIOLOGY = "X001^005";
            public const string DIAGNOSTIC = "X001^006";
            public const string MEDICAL_CHECKUP = "X001^007";
            public const string LOGISTIC = "X001^008";
            public const string NUTRITION = "X001^009";
        }

        public static class AppointmentStatus
        {
            public const string CANCELLED = "0278^001";
            public const string COMPLETE = "0278^002";
            public const string DISCONTINUE = "0278^003";
            public const string DELETED = "0278^004";
            public const string NO_SHOW = "0278^005";
            public const string OVERBOOK = "0278^006";
            public const string PENDING = "0278^007";
            public const string STARTED = "0278^008";
            public const string WAITING_LIST = "0278^009";
            public const string CONFIRMED = "0278^010";

        }

        public static class PurchaseReturnType
        {
            public const string REPLACEMENT = "X161^001";
            public const string CREDIT_NOTE = "X161^002";
        }

        public static class RevenueSharingFormulaType
        {
            public const string BASE_TARIF = "X158^001";
            public const string TARIF = "X158^002";
        }

        public static class ParamedicType
        {
            public const string Physician = "X019^001";
            public const string Nurse = "X019^002";
            public const string Bidan = "X019^003";
            public const string Physiotherapist = "X019^004";
            public const string Anesthesia = "X019^005";
            public const string LaboratoryAnalyst = "X019^006";
            public const string Radiologist = "X019^007";
            public const string Pharmacist = "X019^008";
            public const string PharmacistAsst = "X019^009";
            public const string PetugasRekamMedis = "X019^010";
            public const string Nutritionist = "X019^011";
            public const string OkupasiTerapis = "X019^012";
            public const string TerapisWicara = "X019^013";
            public const string AsistenLuar = "X019^014";
            public const string PenataLuar = "X019^015";
            public const string Fisikawan = "X019^017";
        }

        public static class Specialty
        {
            public const string Nutritionist = "074";
        }

        public static class ToBePerformed
        {
            public const string CURRENT_EPISODE = "X125^001";
            public const string PRIOR_TO_NEXT_VISIT = "X125^002";
            public const string SCHEDULLED = "X125^003";
        }

        public static class VisitReason
        {
            public const string ACCIDENT = "X156^002";
            public const string OTHER = "X156^999";
        }

        public static class DiscontinueMedicationReason
        {
            public const string OTHER = "X136^999";
        }

        public static class PrescriptionType
        {
            public const string MEDICATION_ORDER = "X137^001";
            public const string DISCHARGE_PRESCRIPTION = "X137^002";
            public const string ODD = "X137^004"; // Standard Code ini hanya untuk RSMD
            public const string TERAPI_BARU = "X137^005";
            public const string PASIEN_BARU = "X137^010";
            public const string CITO = "X137^006";
        }

        public static class TransactionStatus
        {
            public const string OPEN = "X121^001";
            public const string WAIT_FOR_APPROVAL = "X121^002";
            public const string APPROVED = "X121^003";
            public const string PROCESSED = "X121^004";
            public const string CLOSED = "X121^005";
            public const string VOID = "X121^999";
        }

        public static class PrescriptionOrderChangesType
        {
            public const string DISPENSARY = "X006^01";
            public const string ITEM = "X006^02";
            public const string QUANTITY = "X006^003";
            public const string DURATION = "X006^04";
        }

        public static class MedicationStatus
        {
            public const string EMPTY = "X258^000";
            public const string OPEN = "X258^001";
            public const string DIPROSES_FARMASI = "X258^002";
            public const string TELAH_DIBERIKAN = "X258^003";
            public const string PASIEN_MENOLAK = "X258^004";
            public const string PASIEN_ABSEN = "X258^005";
            public const string PASIEN_PUASA = "X258^006";
            public const string DI_TUNDA = "X258^007";
            public const string DISCONTINUE = "X258^999";
        }

        public static class MedicationDiscontinueReason
        {
            public const string INSTRUKSI_DOKTER = "X259^001";
            public const string LAIN_LAIN = "X259^002";
        }

        public static class DistributionStatus
        {
            public const string OPEN = "X160^001";
            public const string WAIT_FOR_APPROVAL = "X160^002";
            public const string ON_DELIVERY = "X160^003";
            public const string RECEIVED = "X160^004";
            public const string VOID = "X160^999";
        }

        public static class GLTransactionGroup
        {
            public const string EMERGENCY = "X184^001";
            public const string OUTPATIENT = "X184^002";
            public const string INPATIENT = "X184^003";
            public const string MEDICAL_DIAGNOSTIC = "X184^004";
            public const string MEDICAL_CHECKUP = "X184^005";
            public const string PHARMACY = "X184^006";
        }

        public static class FilterParameterType
        {
            public const string COMBO_BOX = "X108^001";
            public const string CHECK_LIST = "X108^002";
            public const string DATE = "X108^003";
            public const string PAST_PERIOD = "X108^004";
            public const string UPCOMING_PERIOD = "X108^005";
            public const string FREE_TEXT = "X108^006";
            public const string SEARCH_DIALOG = "X108^007";
            public const string CUSTOM_COMBO_BOX = "X108^008";
            public const string YEAR_COMBO_BOX = "X108^009";
            public const string TEXT_BOX = "X108^010";
            public const string TIME_RANGE = "X108^011";
            public const string CONSTANT = "X108^012";
            public const string SINGLE_DATE = "X108^013";
            public const string ITEM_RANGE = "X108^014";
        }

        public static class DataSourceType
        {
            public const string VIEW = "X141^001";
            public const string STORED_PROCEDURE = "X141^002";
        }

        public static class DeleteReason
        {
            public const string WRONG_ENTRY = "X129^001";
            public const string INACTIVE_RECORD = "X129^002";
            public const string OTHER = "X129^999";
        }
        public static class MaspionProcessStatus
        {
            public const string OPEN = "X516^001";
            public const string INPROGRESS = "X516^002";
            public const string FINISH = "X516^003";
        }
        public static class PaymentMethod
        {
            public const string CASH = "X035^001";
            public const string CREDIT_CARD = "X035^002";
            public const string DEBIT_CARD = "X035^003";
            public const string BANK_TRANSFER = "X035^004";
            public const string CREDIT = "X035^005";
            public const string DOWN_PAYMENT = "X035^006";
            public const string PAYMENT_RETURN = "X035^007";
            public const string VOUCHER = "X035^008";
            public const string DEPOSIT_OUT = "X035^009";
            public const string TRANSFER_TRANSACTION = "X035^010";
            public const string UANG_TITIPAN = "X035^011";
            public const string VIRTUAL_PAYMENT = "X035^013";
            public const string QRIS = "X035^020";

            //#region RSMD Only
            public const string BRI_PEDULI = "X035^013";
            public const string OVO_PAYMENT = "X035^014";
            public const string ONLINE_PAYMENT = "X035^015";
            public const string QRIS_FELLO_PAYMENT = "X035^016";
            //#endregion
        }
        public static class PaymentType
        {
            public const string DOWN_PAYMENT = "X034^001";
            public const string SETTLEMENT = "X034^002";
            public const string AR_PATIENT = "X034^003";
            public const string AR_PAYER = "X034^004";
            public const string CUSTOM = "X034^005";
            //public const string PAYMENT_RETURN = "X034^006";
            public const string DEPOSIT_IN = "X034^006";
            public const string DEPOSIT_OUT = "X034^007";
        }
        public static class Gender
        {
            public const string MALE = "0003^M";
            public const string FEMALE = "0003^F";
            public const string UNSPECIFIED = "0003^U";
        }
        public static class PatientTransferStatus
        {
            public const string OPEN = "X143^001";
            public const string TRANSFERRED = "X143^002";
            public const string CANCELLED = "X143^003";
        }
        public static class PatientTransferType
        {
            public const string REGISTRATION = "X127^001";
            public const string INTER_WARD = "X127^002";
            public const string INTER_BED = "X127^003";
            public const string INTER_FACILITY = "X127^004";
        }
        public static class BedStatus
        {
            public const string BOOKED = "0116^B";
            public const string CLOSED = "0116^C";
            public const string HOUSEKEEPING = "0116^H";
            public const string ISOLATED = "0116^I";
            public const string CONTAMINATED = "0116^K";
            public const string OCCUPIED = "0116^O";
            public const string UNOCCUPIED = "0116^U";
            public const string WAIT_TO_BE_TRANSFERRED = "0116^W";
        }

        public static class PatientStatus
        {
            public const string ACTIVE = "X256^01";
            public const string RETENTION = "X256^02";
            public const string ARCHIVED = "X256^03";
        }

        public static class ChargesChangeStatusReason
        {
            public const string KESALAHAN_TRANSAKSI = "X257^01";
            public const string LAIN_LAIN = "X257^99";
        }

        public static class PatientVisitNotes
        {
            public const string REGISTRATION_NOTES = "X011^001";
            public const string EMERGENCY_INITIAL_ASSESSMENT = "X011^002";
            public const string INPATIENT_INITIAL_ASSESSMENT = "X011^004";
            public const string NURSE_INITIAL_ASSESSMENT = "X011^005";
            public const string NUTRITION_INITIAL_ASSESSMENT = "X011^006";
            public const string SUBJECTIVE_NOTES = "X011^007";
            public const string OBJECTIVE_NOTES = "X011^008";
            public const string ASSESSMENT_NOTES = "X011^009";
            public const string PLANNING_NOTES = "X011^010";
            public const string SOAP_SUMMARY_NOTES = "X011^011";
            public const string NURSING_NOTES = "X011^012";
            public const string MEDICAL_RECORD_NOTES = "X011^013";
            public const string FOLLOWUP_NOTES = "X011^014";
            public const string PHARMACY_NOTES = "X011^015";
            public const string DIAGNOSTIC_SUPPORT_NOTES = "X011^016";
            public const string REFERRAL_FROM_NOTES = "X011^017";
            public const string EMERGENCY_CASE_NOTE = "X011^020";
            public const string NURSE_NOTES = "X011^021";
        }

        public static class ItemStatus
        {
            public const string ACTIVE = "X181^001";
            public const string IN_ACTIVE = "X181^999";
        }

        public static class NursingDomainClassType
        {
            public const string DOMAIN = "X197^001";
            public const string CLASS = "X197^002";
        }

        public static class NursingEvaluation
        {
            public const string SUBJECTIVE = "X369^001";
            public const string OBJECTIVE = "X369^002";
            public const string ASSESSMENT = "X369^003";
            public const string PLANNING = "X369^004";
        }

        public static class MonitoringDeviceType
        {
            public const string Ventilator = "X525^001";
        }

        public static class DiagnosticVisitScheduleStatus
        {
            public const string OPEN = "X563^001";
            public const string STARTED = "X563^002";
            public const string COMPLETED = "X563^003";
        }
        #endregion

        public static class Module
        {
            public const string ACCOUNTING = "AC";
            public const string BILLING = "BM";
            public const string FINANCE = "FN";
            public const string LABORATORY = "LB";
            public const string IMAGING = "IS";
            public const string EMR = "EM";
            public const string MEDICAL_CHECKUP = "MC";
            public const string MEDICAL_DIAGNOSTIC = "MD";
            public const string OUTPATIENT = "OP";
            public const string INPATIENT = "IP";
            public const string EMERGENCY = "ER";
            public const string MEDICAL_RECORD = "RM";
            public const string INVENTORY = "IM";
            public const string SYSTEM_SETUP = "SA";
            public const string PHARMACY = "PH";
            public const string NURSING = "NR";
            public const string NUTRITION = "NT";
            public const string RADIOTHERAPHY = "RT";
        }

        public static class Facility
        {
            public const string OUTPATIENT = "OUTPATIENT";
            public const string INPATIENT = "INPATIENT";
            public const string DIAGNOSTIC = "DIAGNOSTIC";
            public const string PHARMACY = "PHARMACY";
            public const string EMERGENCY = "EMERGENCY";
            public const string IMAGING = "IMAGING";
            public const string LABORATORY = "LABORATORY";
            public const string MEDICAL_CHECKUP = "MCU";
        }

        public static class ErrorMessage
        {
            public const string MSG_OPENED_TRANSACTION_VALIDATION = "101";
            public const string MSG_REGISTRATION_BED_VALIDATION = "102";
            public const string MSG_PATIENT_OPENED_REGISTRATION_VALIDATION = "103";
            public const string MSG_CLOSED_REGISTRATION_VALIDATION = "104";
            public const string MSG_DISCHARGE_REGISTRATION_VALIDATION = "105";
            public const string MSG_FINISH_TRANSACTION_VALIDATION = "106";
            public const string MSG_VOID_REGISTRATION_OPENED_TRANSACTION_VALIDATION = "107";
            public const string MSG_COMPLETED_APPOINTMENT_VALIDATION = "108";
            public const string MSG_SELECT_APPOINTMENT_FIRST_VALIDATION = "109";
            public const string MSG_SELECT_REGISTRATION_FIRST_VALIDATION = "110";
            public const string MSG_OPENED_REGISTRATION_VALIDATION = "111";
            public const string MSG_SELECT_MEDICAL_NO_FIRST_VALIDATION = "112";
            public const string MSG_SELECT_TRANSACTION_FIRST_VALIDATION = "113";
            public const string MSG_CASH_BACK_AMOUNT_VALIDATION = "114";
            public const string MSG_CASH_BACK_AMOUNT_VALIDATION_CUSTOM = "115";
            public const string MSG_PATIENT_HAS_MEDICAL_NO_VALIDATION = "116";
            public const string MSG_APPOINTMENT_SLOT_VALIDATION = "117";
            public const string MSG_TRANFERRED_REGISTRATION = "118";
            public const string MSG_CHARGES_REVENUE_SHARING_PROCESSED = "119";
            public const string MSG_DOWN_PAYMENT_USED = "120";
            public const string MSG_AR_PATIENT_ALREADY_EXIST = "121";
            public const string MSG_AR_PAYER_ALREADY_EXIST = "122";
            public const string MSG_VOID_PAYMENT_PAYMENTRECEIPT = "123";
            public const string MSG_AR_ALREADY_PROCCESSED = "124";
            public const string MSG_REGISTRATION_CANNOT_VOID_BECAUSE_TRANSACTION = "125";
            public const string MSG_REGISTRATION_CANNOT_VOID_BECAUSE_CHIEF_COMPLAINT_ANAMNESA = "126";
            public const string MSG_REGISTRATION_CANNOT_VOID_BECAUSE_HAS_SOURCE_AMOUNT = "127";
            public const string MSG_CHARGES_TEST_PARTNER_TRANSACTION_PROCESSED = "128";
            public const string MSG_REGISTRATION_CANNOT_VOID_BECAUSE_HAS_LINKED_REGISTRATION = "129";
        }

        public static class GridViewPageSize
        {
            public const int GRID_COMPACT = 3;
            public const int GRID_FIVE = 5;
            public const int GRID_DEFAULT = 12;
            public const int GRID_CTL = 7;
            public const int GRID_MASTER = 25;
            public const int GRID_MATRIX = 10;
            public const int GRID_ITEM = 15;
            public const int GRID_SERATUS = 100;
            public const int GRID_LIMA_PULUH = 50;
            public const int GRID_PATIENT_LIST = 17;
            public const int GRID_PATIENT_LIST_2 = 7;
            public const int GRID_PATIENT_MR_LIST = 17;
            public const int GRID_REVENUE_SHARING_LIST = 25;
            public const int GRID_REORDER_ITEM = 25;
            public const int GRID_TEMP_MAX_FOR_ORDERING = 100;
            public const int GRID_TEMP_MAX_FOR_ORDERING_2 = 200;
            public const int GRID_TEMP_MAX = 1500;
            public const int GRID_TEMP_MAX_5000 = 5000;
        }

        public static class DefaultValueEntry
        {
            public const string DATE_NOW = "@DateNow";
            public const string TIME_NOW = "@TimeNow";
        }

        public static class FormatString
        {
            public const string DATE_FORMAT = "dd-MMM-yyyy";
            public const string DATE_FORMAT_1 = "dd/MM/yyyy";
            public const string DATE_FORMAT_2 = "dd/MM/yy";
            public const string DATE_FORMAT_3 = "yyyy-MM-dd";
            public const string DATE_PICKER_FORMAT = "dd-MM-yyyy";
            public const string DATE_PICKER_FORMAT2 = "yyyy-MM-dd";
            public const string DATE_FORMAT_112 = "yyyyMMdd";
            public const string DATE_FORMAT_112_2 = "yyMMdd";
            public const string DATE_TIME_FORMAT = "dd MMMM yyyy HH:mm:ss";
            public const string DATE_TIME_FORMAT_2 = "dd-MM-yyyy HH:mm:ss";
            public const string DATE_TIME_FORMAT_3 = "yyyyMMdd HH:mm";
            public const string DATE_TIME_FORMAT_4 = "dd-MM-yyyy HH:mm";
            public const string DATE_TIME_FORMAT_5 = "yyyy-MM-dd HH:mm:ss";
            public const string DATE_TIME_FORMAT_6 = "dd-MMM-yyyy HH:mm";
            public const string DAY_DATE_TIME_FORMAT = "dddd, dd MMMM yyyy HH:mm:ss";
            public const string DATE_FORMAT_WITH_PERIOD = "dd.MM.yyyy";
            public const string DATE_TIME_FORMAT_WITH_PERIOD = "dd.MM.yyyy HH:mm:ss";
            public const string YEAR_FORMAT = "yyyy";
            public const string MONTH_FORMAT = "MM";
            public const string MONTH_FORMAT_2 = "MMMM";
            public const string TIME_FORMAT = "HH:mm";
            public const string TIME_FORMAT_2 = "HHmmss";
            public const string TIME_FORMAT_FULL = "HH:mm:ss.fff";
            public const string TIME_FORMAT_FULL_2 = "HH:mm:ss.ff";
            public const string NUMERIC_2 = "N2";
            public const string YYYYMM = "yyyyMM";
        }

        public static class PrintCode
        {
            public const string LABEL_ASSET = "AC-00004";
            public const string LABEL_ASSET_PER_BA = "AC-00013";

            public const string LABEL_BARANG_PRODUKSI = "IM-00043";

            public const string NUTRITION_PATIENT_REGISTRATION = "IP-00115";

            public const string HASIL_PEMERIKSAAN_RAD_DENGAN_KOP_RSPW = "IS-00019";
            public const string HASIL_PEMERIKSAAN_RAD_TANPA_KOP_RSPW = "IS-00020";

            public const string BUKTI_PEMERIKSAAN_LAB_RSMD = "LB-00007";

            public const string LABEL_PENUNJANG_MEDIS = "MD-00009";

            public const string BUKTI_PENDAFTARAN = "PM-00101";
            public const string BUKTI_PENDAFTARAN_BROS = "PM-00167";
            public const string BUKTI_PENDAFTARAN_RSPBT = "PM-00169";
            public const string BUKTI_PENDAFTARAN_RSDOSOBA = "PM-00179";
            public const string BUKTI_PENDAFTARAN_RSCK = "PM-00185";
            public const string TRACER_REKAM_MEDIS = "PM-00103";
            public const string COVER_REKAM_MEDIS = "PM-00104";
            public const string LABEL_REKAM_MEDIS = "PM-00105";
            public const string KARTU_PASIEN = "PM-00106";
            public const string GELANG_PASIEN = "PM-00107";
            public const string GELANG_PASIEN_ANAK = "PM-00108";
            public const string LABEL_SAMPLE_LABORATORIUM = "PM-00125";
            public const string LABEL_SAMPLE_LABORATORIUM_DT = "PM-00655";
            public const string LABEL_REKAM_MEDIS_2 = "PM-00126";
            public const string GELANG_PASIEN_RAWAT_JALAN = "PM-00129";
            public const string LABEL_MR_RSMD = "PM-00131";
            public const string STICKER_GELANG_RSMD = "PM-00134";
            public const string STICKER_RAWAT_INAP_RSMD = "PM-00136";
            public const string STICKER_RAWAT_JALAN_RSMD = "PM-00137";
            public const string LABEL_COVER_HASIL_RADIOLOGI = "PM-00138";
            public const string TRACER_PERJANJIAN_REKAM_MEDIS = "PM-00150";
            public const string RINGKASAN_TRANSAKSI_RAWAT_JALAN = "PM-00218";
            public const string RINGKASAN_TRANSAKSI_RAWAT_JALAN_2 = "PM-00219";
            public const string KWITANSI_DOT_MATRIX_RSMD = "PM-00408";
            public const string BUKTI_PEMERIKSAAN_DOT_MATRIX_RSMD = "PM-00413";
            public const string DAFTAR_PASIEN_DIRAWAT_RSMD = "PM-00416";
            public const string LABEL_PASIEN_REGISTRASI_LABORATORIUM = "PM-00426";
            public const string BUKTI_PENDAFTARAN_PERJANJIAN = "PM-00144";
            public const string SLIP_GIZI = "PM-00145";
            public const string LABEL_REKAM_MEDIS_RSDOSOBA = "PM-00176";
            public const string LABEL_REKAM_MEDIS_RSRTH = "PM-00188";
            public const string COVER_LABEL = "PM-00428";
            public const string LABEL_PASIEN = "PM-00429";
            public const string GELANG_PASIEN_2 = "PM-00430";
            public const string TRACER_REKAM_MEDIS_PERJANJIAN = "PM-00505";
            public const string TRACER_REKAM_MEDIS_PERJANJIAN_PER_HARI = "PM-00506";
            public const string LABEL_RADIOLOGI = "PM-00509";
            public const string LABEL_PASIEN_REGISTRASI_RADIOLOGI = "PM-00427";
            public const string NUTRITION_WORKSLIST = "NT090103";
            public const string LABEL_PASIEN_REGISTRASI_CATH_LAB = "PM-00432";
            public const string NUTRITION_PATIENT_ORDER = "NT090104";
            public const string JOB_ORDER_LABORATORY = "PM-00515";
            public const string BUKTI_PENDAFTARAN_THERMAL = "PM-00613";
            public const string LABEL_REKAM_MEDIS_RSSMC = "PM-00154";
            public const string LABEL_PENDAFTARAN_RSDO = "PM-00156";
            public const string LABEL_RADIOLOGI_RSDO = "PM-00158";
            public const string GELANG_PASIEN_DEWASA_RSDO = "PM-00155";
            public const string GELANG_PASIEN_ANAK_RSDO = "PM-00157";
            public const string LABEL_OBAT = "PH-00023";
            public const string ORDER_FARMASI = "PM-00534";
            public const string LABEL_REKAM_MEDIS_3 = "PM-00440";
            public const string BUKTI_PERJANJIAN_RSDOSOBA = "PM-00172";
            public const string LABEL_MASTER_ITEM = "PM-00174";
            public const string LABEL_RADIOLOGI_2 = "PM-00177";
            public const string LABEL_RADIOLOGI_3 = "PM-00182";
            public const string LABEL_RADIOLOGI_RSRA = "PM-00189";
            public const string BUKTI_TRANSAKSI_FARMASI = "PM-00361";
            public const string BUKTI_ORDER_RSSES = "PM-00578";
            public const string BUKTI_ORDER_LAB_RSSES = "PM-00686";
            public const string LABEL_OBAT_RSSES = "PH-00061";
            public const string BUKTI_TRANSAKSI = "PM-00589";
            public const string COVER_REKAM_MEDIS_RSBL = "PM-00662";
            public const string SLIP_ANTRIAN_RAWAT_JALAN = "PM-00663";
            public const string LABEL_RADIOLOGI_IND = "PM-00725";
            public const string LABEL_RADIOLOGI_ENG = "PM-00726";
            public const string STICKER_RAWAT_JALAN_RSMD_ZEBRA = "PM-90039";
            public const string LABEL_AMPLOP_HASIL_LABORATORIUM = "PM-00734";
        }

        public static class PrinterType
        {
            public const string ZEBRA_PRINTER = "X208^1";
            public const string EPSON_RECEIPT = "X208^2";
            public const string EPSON_DOT_MATRIX = "X208^3";
            public const string DOT_MATRIX_FORMAT_1 = "X208^4";
            public const string THERMAL_RECEIPT_PRINTER = "X208^5";
            public const string THERMAL_RECEIPT_PRINTER_1 = "X208^6";
            public const string EPSON_DOT_MATRIX_RSRA = "X208^7";
            public const string EPSON_DOT_MATRIX_RSSB = "X208^8";
            public const string THERMAL_FORMAT_RSSES = "X208^9";
            public const string THERMAL_FORMAT_RSSM = "X208^10";
            public const string THERMAL_FORMAT_RSRT = "X208^11";
            public const string THERMAL_FORMAT_RSUKRIDA = "X208^12";
            public const string BROTHER_PRINTER = "X208^13";
        }

        public static class PrintFormat
        {
            public const string LABEL_COVER_HASIL_RADIOLOGI = "X039^02";
            public const string BROTHER_LABEL_COVER_HASIL_RADIOLOGI_RSSY = "X039^04";
            public const string LABEL_COVER_REGISTRASI_RADIOLOGI_RSCK = "X039^05";
            public const string LABEL_RADIOLOGI_KKDI_IND = "X039^06";
            public const string LABEL_RADIOLOGI_KKDI_ENG = "X039^07";

            public const string TRACER_RSSMP = "X209^05";
            public const string TRACER_APPOINTMENT_RSSMP = "X209^06";
            public const string TRACER_RSDOSKA = "X209^07";
            public const string TRACER_RSUKI = "X209^08";
            public const string TRACER_RSSES = "X209^09";
            public const string TRACER_RSASIH = "X209^10";
            public const string TRACER_RSPW = "X209^11";
            
            public const string BROTHER_LABELRM_RSSY = "X210^61";
            public const string ZEBRA_LABELRM_RSSY = "X210^62";
            public const string BROTHER_LABELRM_RSSK = "X210^64";
            public const string BIXOLON_LABELRM_RSPKSB = "X210^65";

            public const string BUKTI_PENDAFTARAN_RSSY = "X215^41";
            public const string BIXOLON_BUKTI_PENDAFTARAN_RSPKSB = "X215^63";

            public const string BROTHER_ETIKET_RSSY = "X225^36";
            public const string BIXOLON_ETIKET_RSPKSB = "X225^42";

            public const string BROTHER_ETIKET_UDD_RSSY = "X260^12";
            public const string BIXOLON_ETIKET_UDD_RSPKSB = "X260^16";

            public const string LABLE_ASSET_RSRTH = "X510^003";
            public const string LABLE_ASSET_RSMD = "X510^004";
            public const string BROTHER_LABEL_ASSET_RSSY = "X510^006";
            
            public const string LABEL_PENUNJANG_MEDIS_RSDOSOBA = "X559^002";
            public const string LABEL_RSP_RAJAL = "X210^57";
            public const string LABEL_RSP_RANAP = "X210^58";

            public const string BIXOLON_WRISTBAND_RSPKSB = "X211^38";
            public const string BIXOLON_WRISTBAND_CHILD_RSPKSB = "X212^29";

            public const string BIXOLON_NUTRITION_LABEL_RSPKSB = "X489^014";
        }

        public static class HL7_Partner
        {
            public const string SureSignsVS = "SureSignsVS";
            public const string BIO_CONNECT = "BIO_CONNECT";
            public const string NovaRAD = "IDN-MEDIS(NOVARIS)";
            public const string MEDAVIS = "MEDAVIS";
            public const string INFINITT = "INFINITT";
            public const string FUJIFILM = "FUJIFILM";
            public const string MEDINFRAS_API_LIS = "MEDINFRAS-API_LIS";
            public const string MEDINFRAS = "MEDINFRAS";
            public const string ZED = "ZED";
            public const string MEDSYNAPTIC = "MEDSYNAPTIC";
        }

        public static class HL7_MessageType
        {
            public const string VS3 = "VS3";
            public const string NOVUS = "NOVUS";
            public const string NovaRAD = "NovaRAD";
            public const string Medavis = "Medavis";
            public const string Infinitt = "Infinitt";
            public const string Fujifilm = "Fujifilm";
            public const string ZED = "ZED";
            public const string MEDSYNAPTIC = "MEDSYNAPTIC";
            public const string ROCHE = "ROCHE";
        }

        public static class SessionName
        {
            public const string COOKIES_NAME = "medinfrasv2.1";
            public const string COOKIES_ASPNET_SessionId = "ASP.NET_SessionId";
        }

        public static class Pivot
        {
            public const string REGISTRATION_ANALYSIS = "1";
        }

        public static class TextFormat
        {
            public const string PAYMENT_RECEIPT_DESCRIPTION = "PAYMENT_RECEIPT_DESCRIPTION";
            public const string REFUND_DOWN_PAYMENT_DESCRIPTION = "REFUND_DOWN_PAYMENT_DESCRIPTION";
            public const string AR_PATIENT_DESCRIPTION = "AR_PATIENT_DESCRIPTION";
            public const string AR_PAYER_DESCRIPTION = "AR_PAYER_DESCRIPTION";
            public const string DOWN_PAYMENT_RECEIPT_DESCRIPTION = "DOWN_PAYMENT_RECEIPT_DESCRIPTION";
        }

        public static class ControlType
        {
            public const string TEXT_BOX = "X103^001";
            public const string COMBO_BOX = "X103^002";
            public const string RADIO_BUTTON = "X103^003";
            public const string CHECK_BOX = "X103^004";
            public const string SEARCH_DIALOG = "X103^005";
            public const string CUSTOM_COMBO_BOX = "X103^006";
            public const string MULTI_SELECT_COMBO_BOX = "X103^007";
        }

        public static class CenterBackConsumerAPI
        {
            public const string DEMO_VERSION = "X470^000";
            public const string RAZAKI = "X470^001";
            public const string MEDAPP = "X470^002";
            public const string MEDINFRAS_EMR_V1 = "X470^003";
        }

        public static class DrugAlertVendor
        {
            public const string MIMS = "X554^001";
        }

        public static class FamilyRelation
        {
            public const string FATHER = "0063^FTH";
            public const string MOTHER = "0063^MTH";
            public const string SPOUSE = "0063^SPO";
            public const string CHILD = "0063^CHD";
        }

        public static class GCPatientCategory
        {
            public const string KARYAWAN_RS = "X067^001";
        }
        public static class PrescriptionTaskLogStatus
        {
            public const string Sent = "X552^001";
            public const string Received = "X552^002";
            public const string Started = "X552^003";
            public const string Completed = "X552^004";
            public const string Closed = "X552^005";
            public const string Reopen = "X552^998";
            public const string Void = "X552^999";
        }
        public static class TransactionCode
        {
            public const string ER_REGISTRATION = "1102";
            public const string ER_CHARGES = "1103";
            public const string ER_PATIENT_BILL = "1104";
            public const string ER_PATIENT_PAYMENT_DP = "1105";
            public const string ER_PATIENT_PAYMENT_SETTLEMENT = "1106";
            public const string ER_PATIENT_PAYMENT_AR_PATIENT = "1107";
            public const string ER_PATIENT_PAYMENT_AR_PAYER = "1108";
            public const string ER_MEDICATION_ORDER = "1109";
            public const string ER_OUTPATIENT_ORDER = "1111";
            public const string ER_PAYMENT_RECEIPT = "1112";
            public const string ER_PATIENT_PAYMENT_CUSTOM = "1113";
            //public const string ER_PATIENT_PAYMENT_RETURN = "1114";
            public const string ER_DEPOSIT_IN = "1114";
            public const string ER_DEPOSIT_OUT = "1115";

            public const string OP_APPOINTMENT = "1201";
            public const string OP_REGISTRATION = "1202";
            public const string OP_CHARGES = "1203";
            public const string OP_PATIENT_BILL = "1204";
            public const string OP_PATIENT_PAYMENT_DP = "1205";
            public const string OP_PATIENT_PAYMENT_SETTLEMENT = "1206";
            public const string OP_PATIENT_PAYMENT_AR_PATIENT = "1207";
            public const string OP_PATIENT_PAYMENT_AR_PAYER = "1208";
            public const string OP_MEDICATION_ORDER = "1209";
            public const string OP_EMERGENCY_ORDER = "1210";
            public const string OP_PAYMENT_RECEIPT = "1212";
            public const string OP_PATIENT_PAYMENT_CUSTOM = "1213";
            //public const string OP_PATIENT_PAYMENT_RETURN = "1214";
            public const string OP_DEPOSIT_IN = "1214";
            public const string OP_DEPOSIT_OUT = "1215";
            public const string OP_APPOINTMENT_REQUEST = "1216";

            public const string BED_RESERVATION = "1301";
            public const string IP_REGISTRATION = "1302";
            public const string IP_CHARGES = "1303";
            public const string IP_PATIENT_BILL = "1304";
            public const string IP_PATIENT_PAYMENT_DP = "1305";
            public const string IP_PATIENT_PAYMENT_SETTLEMENT = "1306";
            public const string IP_PATIENT_PAYMENT_AR_PATIENT = "1307";
            public const string IP_PATIENT_PAYMENT_AR_PAYER = "1308";
            public const string IP_MEDICATION_ORDER = "1309";
            public const string IP_EMERGENCY_ORDER = "1310";
            public const string IP_OUTPATIENT_ORDER = "1311";
            public const string IP_PAYMENT_RECEIPT = "1312";
            public const string IP_PATIENT_PAYMENT_CUSTOM = "1313";
            //public const string IP_PATIENT_PAYMENT_RETURN = "1314";
            public const string IP_DEPOSIT_IN = "1314";
            public const string IP_PATIENT_ACCOMPANY_CHARGES = "1315";
            public const string IP_DEPOSIT_OUT = "1316";

            public const string IP_NUTRITION_ORDER = "1321";
            public const string NURSING_TRANSACTION = "1331";

            public const string IMAGING_TEST_ORDER = "2101";
            public const string IMAGING_REGISTRATION = "2102";
            public const string IMAGING_CHARGES = "2103";
            public const string IMAGING_PATIENT_BILL = "2104";
            public const string IMAGING_PATIENT_PAYMENT_DP = "2105";
            public const string IMAGING_PATIENT_PAYMENT_SETTLEMENT = "2106";
            public const string IMAGING_PATIENT_PAYMENT_AR_PATIENT = "2107";
            public const string IMAGING_PATIENT_PAYMENT_AR_PAYER = "2108";
            public const string IMAGING_MEDICATION_ORDER = "2109";
            public const string IMAGING_PAYMENT_RECEIPT = "2112";
            public const string IMAGING_PATIENT_PAYMENT_CUSTOM = "2113";
            //public const string IMAGING_PATIENT_PAYMENT_RETURN = "2114";
            public const string IMAGING_DEPOSIT_IN = "2114";
            public const string IMAGING_DEPOSIT_OUT = "2115";

            public const string LABORATORY_TEST_ORDER = "2201";
            public const string LABORATORY_REGISTRATION = "2202";
            public const string LABORATORY_CHARGES = "2203";
            public const string LABORATORY_PATIENT_BILL = "2204";
            public const string LABORATORY_PATIENT_PAYMENT_DP = "2205";
            public const string LABORATORY_PATIENT_PAYMENT_SETTLEMENT = "2206";
            public const string LABORATORY_PATIENT_PAYMENT_AR_PATIENT = "2207";
            public const string LABORATORY_PATIENT_PAYMENT_AR_PAYER = "2208";
            public const string LABORATORY_MEDICATION_ORDER = "2209";
            public const string LABORATORY_PAYMENT_RECEIPT = "2212";
            public const string LABORATORY_PATIENT_PAYMENT_CUSTOM = "2213";
            //public const string LABORATORY_PATIENT_PAYMENT_RETURN = "2214";
            public const string LABORATORY_DEPOSIT_IN = "2214";
            public const string LABORATORY_DEPOSIT_OUT = "2215";
            public const string BLOOD_BANK_ORDER = "2216";

            public const string MCU_APPOINTMENT = "2301";
            public const string MCU_REGISTRATION = "2302";
            public const string MCU_CHARGES = "2303";
            public const string MCU_PATIENT_BILL = "2304";
            public const string MCU_PATIENT_PAYMENT_DP = "2305";
            public const string MCU_PATIENT_PAYMENT_SETTLEMENT = "2306";
            public const string MCU_PATIENT_PAYMENT_AR_PATIENT = "2307";
            public const string MCU_PATIENT_PAYMENT_AR_PAYER = "2308";
            public const string MCU_MEDICATION_ORDER = "2309";
            public const string MCU_OUTPATIENT_ORDER = "2310";
            public const string MCU_EMERGENCY_ORDER = "2311";
            public const string MCU_PAYMENT_RECEIPT = "2312";
            public const string MCU_PATIENT_PAYMENT_CUSTOM = "2313";
            //public const string MCU_PATIENT_PAYMENT_RETURN = "2314";
            public const string MCU_DEPOSIT_IN = "2314";
            public const string MCU_DEPOSIT_OUT = "2315";
            public const string MCU_GROUP_BATCH_NO = "2316";


            public const string RADIOTHERAPHY_TEST_ORDER = "2401";
            public const string RADIOTHERAPHY_REGISTRATION = "2402";
            public const string RADIOTHERAPHY_CHARGES = "2403";
            public const string RADIOTHERAPHY_PATIENT_BILL = "2404";
            public const string RADIOTHERAPHY_PATIENT_PAYMENT_DP = "2405";
            public const string RADIOTHERAPHY_PATIENT_PAYMENT_SETTLEMENT = "2406";
            public const string RADIOTHERAPHY_PATIENT_PAYMENT_AR_PATIENT = "2407";
            public const string RADIOTHERAPHY_PATIENT_PAYMENT_AR_PAYER = "2408";
            public const string RADIOTHERAPHY_MEDICATION_ORDER = "2409";
            public const string RADIOTHERAPHY_PAYMENT_RECEIPT = "2410";
            public const string RADIOTHERAPHY_PATIENT_PAYMENT_CUSTOM = "2411";
            public const string RADIOTHERAPHY_DEPOSIT_IN = "2412";
            public const string RADIOTHERAPHY_DEPOSIT_OUT = "2413";

            public const string OTHER_TEST_ORDER = "2901";
            public const string OTHER_MEDICAL_DIAGNOSTIC_REGISTRATION = "2902";
            public const string OTHER_DIAGNOSTIC_CHARGES = "2903";
            public const string OTHER_PATIENT_BILL = "2904";
            public const string OTHER_PATIENT_PAYMENT_DP = "2905";
            public const string OTHER_PATIENT_PAYMENT_SETTLEMENT = "2906";
            public const string OTHER_PATIENT_PAYMENT_AR_PATIENT = "2907";
            public const string OTHER_PATIENT_PAYMENT_AR_PAYER = "2908";
            public const string OTHER_MEDICATION_ORDER = "2909";
            public const string OTHER_PAYMENT_RECEIPT = "2912";
            public const string OTHER_PATIENT_PAYMENT_CUSTOM = "2913";
            //public const string OTHER_PATIENT_PAYMENT_RETURN = "2914";
            public const string OTHER_DEPOSIT_IN = "2914";
            public const string OTHER_DEPOSIT_OUT = "2915";
            public const string OTHER_DIAGNOSTIC_APPOINTMENT = "2916";
            public const string OTHER_DIAGNOSTIC_DRAFT_TEST_ORDER_APPOINTMENT = "2917";

            public const string PH_REGISTRATION = "3102";
            public const string PH_CHARGES = "3103";
            public const string PH_PATIENT_BILL = "3104";
            public const string PH_PATIENT_PAYMENT_DP = "3105";
            public const string PH_PATIENT_PAYMENT_SETTLEMENT = "3106";
            public const string PH_PATIENT_PAYMENT_AR_PATIENT = "3107";
            public const string PH_PATIENT_PAYMENT_AR_PAYER = "3108";
            public const string PH_MEDICATION_ORDER = "3109";
            public const string PH_PAYMENT_RECEIPT = "3112";
            public const string PH_PATIENT_PAYMENT_CUSTOM = "3113";
            //public const string PH_PATIENT_PAYMENT_RETURN = "3114";
            public const string PH_DEPOSIT_IN = "3114";
            public const string PH_DEPOSIT_OUT = "3115";

            public const string PRESCRIPTION_EMERGENCY = "3301";
            public const string PRESCRIPTION_OUTPATIENT = "3302";
            public const string PRESCRIPTION_INPATIENT = "3303";
            public const string PRESCRIPTION_IMAGING = "3304";
            public const string PRESCRIPTION_LABORATORY = "3305";
            public const string PRESCRIPTION_OTHER = "3306";
            public const string PRESCRIPTION_RETURN_ORDER = "3307";

            public const string ITEM_REQUEST = "4104";
            public const string ITEM_CONSUMPTION = "4105";
            public const string ITEM_ADJUSTMENT = "4106";
            public const string PURCHASE_REQUEST = "4201";
            public const string PURCHASE_ORDER = "4202";
            public const string PURCHASE_RECEIVE = "4203";
            public const string ITEM_DISTRIBUTION = "4204";
            public const string PURCHASE_RETURN = "4205";
            public const string DIRECT_PURCHASE = "4206";
            public const string DIRECT_PURCHASE_RETURN = "4207";
            public const string SUPPLIER_CREDIT_NOTE = "4208";
            public const string PRODUCTION_PROCESS = "4209";
            public const string STOCK_TAKING = "4210";
            public const string PURCHASE_RETURN_REPLACEMENT = "4211";
            public const string PRICE_CHANGED = "4212";
            public const string CONSIGNMENT_ORDER = "4213";
            public const string CONSIGNMENT_RECEIVE = "4214";
            public const string CONSIGNMENT_RETURN = "4215";
            public const string CONSIGNMENT_RECEIVE_WITHOUT_PO = "4216";
            public const string DONATION_RECEIVE = "4217";
            public const string DONATION_RETURN = "4218";

            public const string SERVICE_REQUEST = "4301";
            public const string SERVICE_DISTRIBUTION = "4302";
            public const string SERVICE_CONSUMPTION = "4303";
            public const string SERVICE_RETURN = "4304";

            public const string TRANS_REVENUE_SHARING_ENTRY = "5101";
            public const string AR_INVOICE_PATIENT = "5102";
            public const string AR_INVOICE_PAYER = "5103";
            public const string AR_RECEIVE_PATIENT = "5104";
            public const string AR_RECEIVE_PAYER = "5105";
            public const string AR_RECEIPT = "5106";
            public const string TRANS_REVENUE_SHARING_SUMMARY_ENTRY = "5107";
            public const string TRANS_REVENUE_SHARING_PAYMENT_ENTRY = "5108";
            public const string AR_INVOICE_NON_OPERATIONAL = "5109";
            public const string AR_INVOICE_PAYER_ADJUSTMENT = "5110";
            public const string AR_INVOICE_PATIENT_ADJUSTMENT = "5111";
            public const string TRANS_REVENUE_SHARING_ADJUSTMENT_ENTRY = "5112";

            public const string PURCHASE_INVOICE = "6101";
            public const string SUPPLIER_PAYMENT_VERIFICATION = "6102";
            public const string TEST_PARTNER_TRANSACTION = "6103";

            public const string FIXED_ASSET_ITEM_MOVEMENT = "7101";
            public const string FIXED_ASSET_WRITE_OFF = "7102";
            public const string FIXED_ASSET_ITEM_MOVEMENT_HD = "7103";
            public const string FIXED_ASSET_FA_ACCEPTANCE = "7104";
            public const string FIXED_ASSET_WRITE_OFF_HD = "7105";

            public const string JOURNAL_MEMORIAL = "7201";
            public const string JOURNAL = "72%";

            public const string JOURNAL_MEMORIAL_UMUM = "7281";
            public const string JOURNAL_MEMORIAL_KAS_KELUAR = "7282";
            public const string JOURNAL_MEMORIAL_KAS_MASUK = "7283";
            public const string JOURNAL_MEMORIAL_BANK_KELUAR = "7284";
            public const string JOURNAL_MEMORIAL_BANK_MASUK = "7285";
            public const string JOURNAL_MEMORIAL_PENYESUAIAN = "7286";
            public const string JOURNAL_MEMORIAL_PENYUSUTAN_AKTIVA = "7287";
            public const string JOURNAL_MEMORIAL_PEMUSNAHAN_AKTIVA = "7288";
            public const string JOURNAL_MEMORIAL_IKHTISAR = "7299";

            public const string BANK_RECONCILIATION = "7301";
            public const string ESTIMATED_CHARGES = "7302";

            public const string DRAFT_LABORATORY_TEST_ORDER = "8101";
            public const string DRAFT_IMAGING_TEST_ORDER = "8102";
            public const string DRAFT_OTHER_TEST_ORDER = "8103";

            public const string DRAFT_IP_OUTPATIENT_ORDER = "8201";
            public const string DRAFT_MCU_OUTPATIENT_ORDER = "8202";
            public const string DRAFT_ER_OUTPATIENT_ORDER = "8203";
            public const string DRAFT_IP_EMERGENCY_ORDER = "8204";
            public const string DRAFT_OP_EMERGENCY_ORDER = "8205";

            public const string DRAFT_IP_MEDICATION_ORDER = "8301";
            public const string DRAFT_ER_MEDICATION_ORDER = "8302";
            public const string DRAFT_OP_MEDICATION_ORDER = "8303";
            public const string DRAFT_MCU_MEDICATION_ORDER = "8304";
            public const string DRAFT_OTHER_MEDICATION_ORDER = "8305";
            public const string DRAFT_PH_MEDICATION_ORDER = "8306";

            public const string DRAFT_ER_CHARGES = "8401";
            public const string DRAFT_OP_CHARGES = "8402";
            public const string DRAFT_IP_CHARGES = "8403";
            public const string DRAFT_IP_PATIENT_ACCOMPANY_CHARGES = "8404";
            public const string DRAFT_IMAGING_CHARGES = "8405";
            public const string DRAFT_LABORATORY_CHARGES = "8406";
            public const string DRAFT_MCU_CHARGES = "8407";
            public const string DRAFT_OTHER_DIAGNOSTIC_CHARGES = "8408";
            public const string DRAFT_PH_CHARGES = "8409";
            public const string DRAFT_PRESCRIPTION_OUTPATIENT = "8410";
            public const string DRAFT_PRESCRIPTION_INPATIENT = "8411";
            public const string DRAFT_PRESCRIPTION_IMAGING = "8412";
            public const string DRAFT_PRESCRIPTION_LABORATORY = "8413";
            public const string DRAFT_PRESCRIPTION_OTHER = "8414";

            public const string REFERRAL_TO_NO = "9201";
        }

        public static class ItemGroup
        {
            public const string INVENTORY = "INVENTORY";
            public const string LABORATORY = "LABORATORY";
            public const string IMAGING = "IMAGING";
            public const string DIAGNOSTIC = "DIAGNOSTIC";
            public const string FINANCE = "FINANCE";
            public const string MCU = "MCU";
            public const string NUTRIENT = "NUTRIENT";
        }

        public static class ItemType
        {
            public const string PELAYANAN = "X001^001";
            public const string OBAT_OBATAN = "X001^002";
            public const string BARANG_MEDIS = "X001^003";
            public const string LABORATORIUM = "X001^004";
            public const string RADIOLOGI = "X001^005";
            public const string PENUNJANG_MEDIS = "X001^006";
            public const string MEDICAL_CHECKUP = "X001^007";
            public const string BARANG_UMUM = "X001^008";
            public const string BAHAN_MAKANAN = "X001^009";
        }

        public static class SubItemType
        {
            public const string RADIOTERAPI = "X569^RT";
        }

        public static class ReportCode
        {
            public static class SystemSetup
            {
                public const string DIAGNOSE = "MST-00001";
            }
            public static class MedicalRecord
            {
                public const string EPISODE_SUMMARY = "MR000001";
                public const string EPISODE_SUMMARY_NEW = "MR000002";
                public const string MEDICAL_RECORD = "MR000003";
                public const string EPISODE_SUMMARY_WITHOUT_INTEGRATED = "MR000004";
                public const string EPISODE_SUMMARY_WITH_INTEGRATED = "MR000005";
                public const string EPISODE_SUMMARY_WITH_INTEGRATED_TEMP = "MR000006";
            }
            public static class Finance
            {
                public const string LAPORAN_REKAP_PEMBAYARAN_JASA_MEDIS = "FN00100";
            }
        }

        public static class ConstantDate
        {
            public const string DEFAULT_NULL = "01-01-1900";
            public const string DEFAULT_NULL_DATE_FORMAT = "01-Jan-1900";
        }

        public static class BloodType
        {
            public const string BloodType_A = "X009^A";
            public const string BloodType_AB = "X009^AB";
            public const string BloodType_B = "X009^B";
            public const string BloodType_O = "X009^O";
            public const string BloodType_NA = "X009^X";
        }

        public static class AdjustmentType
        {
            public const string RECEIPTS = "X173^001";
            public const string ISSUES = "X173^002";
        }

        public static class DrugClass
        {
            public const string MORPHIN = "X123^M";
            public const string NARKOTIKA = "X123^O";
            public const string PSIKOTROPIKA = "X123^P";
        }

        public static class GCAccountReceivableType
        {
            public const string PIUTANG_DALAM_PERAWATAN = "X191^001";
            public const string PIUTANG_DALAM_PROSES = "X191^002";
            public const string PIUTANG_INSTANSI = "X191^003";
            public const string PENYESUAIAN_PIUTANG = "X191^004";
        }

        public static class Shift
        {
            public const string PAGI = "X168^001";
            public const string SIANG = "X168^002";
            public const string MALAM = "X168^003";
        }

        public static class PEWS_Behavior
        {
            public const string X370_01 = "X370^01";
            public const string X370_02 = "X370^02";
            public const string X370_03 = "X370^03";
            public const string X370_04 = "X370^04";
        }

        public static class PEWS_Cardiovascular
        {
            public const string X371_01 = "X371^01";
            public const string X371_02 = "X371^02";
            public const string X371_03 = "X371^03";
            public const string X371_04 = "X371^04";
        }

        public static class PEWS_Respiration
        {
            public const string X372_01 = "X372^01";
            public const string X372_02 = "X372^02";
            public const string X372_03 = "X372^03";
            public const string X372_04 = "X372^04";
        }

        public static class BudgetCategory
        {
            public const string Rutin = "X393^001";
            public const string CAPEX = "X393^002";
            public const string OPEX = "X393^003";
            public const string OCE = "X393^004";
        }

        public static class Nationality
        {
            public const string INDONESIA = "0212^001";
        }

        public static class PurchaseOrderType
        {
            public const string DRUGMS = "X145^001";
            public const string LOGISTIC = "X145^002";
            public const string SERVICES = "X145^003";
            public const string ASSET = "X145^004";
            public const string FOOD = "X145^005";
            public const string ALKES = "X145^006";
            public const string COVID = "X145^007";
            public const string MARKETING = "X145^008";
            public const string ALKES_RANAP = "X145^996";
            public const string ALKES_RAJAL = "X145^997";
            public const string OBAT_RANAP = "X145^998";
            public const string OBAT_RAJAL = "X145^999";
        }

        public static class ChargesType
        {
            public const string ONGKOS_KIRIM = "X157^001";
            public const string MATERAI = "X157^002";
            public const string LAIN_LAIN = "X157^999";
        }

        public static class BridgingVendor
        {
            public const string HIS = "MEDINFRAS";
            public const string MEDINFRAS_API = "MEDINFRAS API";
            public const string QUEUE = "QUEUE";
            public const string BPJS = "BPJS";
            public const string APLICARES = "APLICARES";
            public const string EKLAIM = "EKLAIM";
            public const string INHEALTH = "INHEALTH";
            public const string LIS = "LIS";
            public const string RIS = "RIS";
            public const string MEDINLINK = "MEDINLINK";
            public const string DINKES = "DINKES";
            public const string NALAGENETICS = "NALAGENETICS";
            public const string SIRANAP = "SIRANAP";
            public const string MOBILE_JKN = "MOBILE JKN";
            public const string MASPION = "MASPION";
            public const string SATUSEHAT = "SATUSEHAT";
        }

        public static class DocumentType
        {
            public const string AUTOPSY_REPORT = "0270^AR";
            public const string CARDIODIAGNOSTICS = "0270^CD";
            public const string CONSULTATION = "0270^CN";
            public const string DIAGNOSTIC_IMAGING = "0270^DI";
            public const string DISCHARGE_SUMMARY = "0270^DS";
            public const string EMERGENCY_DEPARTMENT_REPORT = "0270^ED";
            public const string HISTORY_PHYSICAL_EXAMINATION = "0270^HP";
            public const string LABORATORY_RESULT = "0270^LB";
            public const string MEDICAL_RECORD_SUMMARY = "0270^MR";
            public const string OPERATIVE_REPORT = "0270^OP";
            public const string PSYCHIATRIC_CONSULTATION = "0270^PC";
            public const string PSYCHIATRIC_HISTORY_PHYSICAL_EXAMINATION = "0270^PH";
            public const string PROCEDURE_NOTE = "0270^PN";
            public const string PROGRESS_NOTE = "0270^PR";
            public const string SURGICAL_PATHOLOGY = "0270^SP";
            public const string TRANSFER_SUMMARY = "0270^TS";
            public const string FINANCE_BILLING_DOCUMENT = "0270^X1";
            public const string OTHERS_DOCUMENT = "0270^X9";
        }

        public static class ItemRequestType
        {
            public const string DISTRIBUTION = "X217^01";
            public const string CONSUMPTION = "X217^02";
        }

        public static class MRStatusNote
        {
            public const string TULISAN_TIDAK_JELAS = "X222^01";
            public const string TIDAK_ADA_TANDA_TANGAN_DOKTER = "X222^02";
            public const string INFORMASI_TIDAK_LENGKAP = "X222^03";
            public const string IDENTIFIKASI = "X222^04";
            public const string OTENTIFIKASI = "X222^05";
            public const string PENCATATAN = "X222^06";
            public const string PELAPORAN = "X222^07";
            public const string OTHERS = "X222^99";
        }

        public static class StockStatus
        {
            public const string ALL = "X224^001";
            public const string READY_STOCK = "X224^002";
            public const string NO_STOCK = "X224^003";
        }

        public static class LocationGroup
        {
            public const string DRUG_AND_MEDICAL_SUPPLIES = "X227^1";
            public const string LOGISTIC = "X227^2";
            public const string NUTRITION = "X227^3";
        }

        public static class InventoryRoundingStatus
        {
            public const string ROUND_UP = "X207^001";
            public const string AS_IS = "X207^002";
        }

        public static class ServiceUnitLinkType
        {
            public const string INPATIENT = "01INPATIENT";
            public const string EMERGENCY_CARE = "02EMERGENCY";
            public const string OUTPATIENT = "03OUTPATIENT";
        }

        public static class Interface_Link_File
        {
            public const string Link_Department = "~/Libs/App_Data/interface/emr/department.lnk";
        }

        public static class BPJS_WS_EXCEPTION
        {
            public const string NOT_FOUND = "Data Peserta tidak ditemukan";
            public const string GENERIC_ERROR = "Proses bridging gagal";
            public const string CONNECTION_CLOSED = "Koneksi ke Server BPJS GAGAL!";
            public const string TIMEOUT = "Kesalahan pada jaringan, koneksi ke server BPJS Timeout";
            public const string PROTOCOL_ERROR = "Akses ke server BPJS ditolak / Protocol Error";
        }

        public static class API_WS_EXCEPTION
        {
            public const string GENERIC_ERROR = "Proses bridging gagal";
            public const string CONNECTION_CLOSED = "Koneksi ke Server API GAGAL!";
            public const string TIMEOUT = "Kesalahan pada jaringan, koneksi ke server API Timeout";
            public const string PROTOCOL_ERROR = "Akses ke server API ditolak / Protocol Error";
        }

        public static class LaboratoryResultValue
        {
            public const string TEXT = "X002^001";
            public const string NUMERIC = "X002^002";
        }

        public static class TestPartnerType
        {
            public const string LABORATORY = "X230^001";
            public const string IMAGING = "X230^002";
        }

        public static class TypeTarifINACBG
        {
            public const string ProsedurNonBedah = "EK001";
            public const string ProsedurBedah = "EK002";
            public const string Konsultasi = "EK003";
            public const string TenagaAhli = "EK004";
            public const string Keperawatan = "EK005";
            public const string Penunjang = "EK006";
            public const string Radiologi = "EK007";
            public const string Laboratorium = "EK008";
            public const string PelayananDarah = "EK009";
            public const string Rehabilitasi = "EK010";
            public const string KamarAkomodasi = "EK011";
            public const string RawatIntensiff = "EK012";
            public const string Obat = "EK013";
            public const string ObatKronis = "EK014";
            public const string ObatKemoterapi = "EK015";
            public const string Alkes = "EK016";
            public const string BMHP = "EK017";
            public const string SewaAlat = "EK018";
            public const string Lainlain = "EK019";
        }

        public static class TariffScheme
        {
            public const string Standard = "X005^001";
        }

        public static class PrescriptionFrequency
        {
            public const string QD = "X233^01";
            public const string BID = "X233^02";
            public const string TID = "X233^03";
            public const string QID = "X233^04";
            public const string QH = "X233^05";
            public const string DD = "X233^06";
            public const string OD = "X233^07";
        }

        public static class CoenamRule
        {
            public const string AC = "X172^AC";
            public const string DC = "X172^DC";
            public const string PC = "X172^PC";
        }

        public static class DirectPrintType
        {
            public const string BUKTI_PENDAFTARAN = "X240^01";
            public const string TRACER_RM = "X240^02";
            public const string LABEL_RM = "X240^03";
            public const string GELANG_DEWASA_L = "X240^04";
            public const string GELANG_DEWASA_P = "X240^05";
            public const string GELANG_ANAK_L = "X240^06";
            public const string GELANG_ANAK_P = "X240^07";
            public const string GELANG_BAYI_L = "X240^08";
            public const string GELANG_BAYI_P = "X240^09";
            public const string ETIKET_OBAT_DALAM = "X240^10";
            public const string ETIKET_OBAT_LUAR = "X240^11";
            public const string BUKTI_PEMBAYARAN = "X240^12";
            public const string LABEL_UDD = "X240^13";
            public const string LABEL_BARANG_PRODUKSI = "X240^14";
            public const string GELANG_PASIEN_RJ_L = "X240^15";
            public const string GELANG_PASIEN_RJ_P = "X240^16";
            public const string BUKTI_PENDAFTARAN_PERJANJIAN = "X240^17";
            public const string LABEL_REGISTRASI_RADIOLOGI = "X240^18";
            public const string GIZI_JOB_ORDER_MENU_DETAIL = "X240^19";
            public const string STICKER_GELANG_PASIEN = "X240^20";
            public const string GIZI_DISTRIBUTION_LABEL = "X240^20";
            public const string GIZI_ORDER_SLIP = "X240^21";
            public const string LABEL_REGISTRASI_LABORATORIUM = "X240^22";
            public const string LABEL_RADIOLOGI = "X240^24";
            public const string FORM_PERMINTAAN_MAKANAN = "X240^25";
            public const string BON_PERMINTAAN_MAKANAN = "X240^26";
            public const string JOB_ORDER_LABORATORIUM = "X240^27";
            public const string LABEL_DISTRIBUSI_GIZI = "X240^28";
            public const string LABEL_OBAT = "X240^29";
            public const string EDC_IP_CONFIGURATION = "X240^30";
            public const string ORDER_FARMASI = "X240^31";
            public const string LABEL_MASTER_ITEM = "X240^32";
            public const string LABEL_BARANG_PRODUKSI_LUAR = "X240^33";
            public const string LABEL_RADIOLOGI_2 = "X240^34";
            public const string PRINT_KWITANSI_RAJAL = "X240^35";
            public const string LABEL_ASSET = "X240^36";
            public const string LABEL_PERMINTAAN_MCU_KARYAWAN = "X240^37";
            public const string ORDER_FARMASI_RAWATINAP = "X240^39";
            public const string BUKTI_TRANSAKSI_FARMASI = "X240^40";
            public const string BUKTI_ORDER_PENUNJANG = "X240^42";
            public const string DESKTOP_TOOL = "X240^41";
            public const string BUKTI_TRANSAKSI = "X240^43";
            public const string LABEL_PENUNJANG_MEDIS = "X240^44";
            public const string LABEL_COVER_RADIOLOGI = "X240^45";
            public const string BUKTI_PERJANJIAN_MULTI_KUNJUNGAN = "X240^46";
            public const string LABEL_AMPLOP_HASIL_LABORATORIUM = "X240^47";

        }

        public static class BornAt
        {
            public const string INHOSPITAL = "X154^001";
            public const string OUTHOSPITAL = "X154^002";
        }

        public static class Referrer
        {
            public const string DOKTER_RS = "X105^000";
            public const string PRAKTEK_DOKTER_LUAR = "X105^001";
            public const string PUSKESMAS = "X105^002";
            public const string RUMAH_SAKIT = "X105^003";
            public const string TENAGA_PARAMEDIK = "X105^004";
            public const string KASUS_POLISI = "X105^005";
            public const string FASKES = "X105^006";
        }

        public static class DiscountReason
        {
            public const string DOKTER = "X155^001";
            public const string ALIM_ULAMA = "X155^002";
            public const string KARYAWAN = "X155^003";
            public const string INSTANSI_PEMERINTAH = "X155^004";
            public const string LAIN_LAIN = "X155^999";
        }

        public static class DiscountReasonChargesDt
        {
            public const string DOKTER = "X550^001";
            public const string ALIM_ULAMA = "X550^002";
            public const string KARYAWAN = "X550^003";
            public const string INSTANSI_PEMERINTAH = "X550^004";
            public const string KOLEGA = "X550^005";
            public const string ASURANSI = "X550^006";
            public const string PRIVILEDGE = "X550^007";
            public const string KHUSUS = "X550^008";
            public const string KAMAR = "X550^009";
            public const string ANAK_KE1 = "X550^010";
            public const string ANAK_KE2 = "X550^011";
            public const string ANAK_KE3 = "X550^012";
            public const string WARGA = "X550^013";
            public const string LAIN_LAIN = "X550^999";
        }

        public static class ATD_STATUS
        {
            public const string ADMISSION = "X163^001";
            public const string TRANSFER_OUT = "X163^002";
            public const string TRANSFER_IN = "X163^003";
            public const string STAY = "X163^004";
            public const string DISCHARGED_WITH_AGREEMENT = "X163^005";
            public const string DISCHARGED_TO_OTHER_FACILITY = "X163^006";
            public const string DISCHARGED_TO_ANOTHER_HOSPITAL = "X163^007";
            public const string DISCHARGED_FORCED_OUT = "X163^008";
            public const string DIED_BEFORE_48_HR = "X163^009";
            public const string DIED_AFTER_48_HR = "X163^010";
        }

        public static class ATD_STATUS_GROUP
        {
            public const string PATIENT_IN = "X253^001";
            public const string PATIENT_TRANSFER_IN = "X253^002";
            public const string PATIENT_TRANSFER_OUT = "X253^003";
            public const string PATIENT_OUT = "X253^004";
            public const string DIED_BEFORE_48_HR = "X253^005";
            public const string DIED_AFTER_48_HR = "X253^006";
        }

        public static class AR_PAYMENT_METHODS
        {
            public const string CASH = "X254^001";
            public const string BANK_TRANSFER = "X254^002";
            public const string CREDIT_CARD = "X254^003";
            public const string DEBIT_CARD = "X254^004";
            public const string WRITE_OFF = "X254^005";
            public const string BIAYA_ADMIN = "X254^006";
            public const string DISKON = "X254^007";
            public const string POTONGAN_HONOR_DOKTER = "X254^008";
            public const string PPH23 = "X254^103";
        }

        public static class Bed_Reservation_Status
        {
            public const string OPEN = "X263^001";
            public const string PROPOSED = "X263^002";
            public const string COMPLETE = "X263^003";
            public const string CANCELLED = "X263^999";
        }

        public static class SEP_Status
        {
            public const string PENGAJUAN = "X284^001";
            public const string DISETUJUI = "X284^002";
            public const string DICETAK = "X284^003";
        }

        public static class BPJS_Claim_Status
        {
            public const string PROSES_VERIFIKASI = "X285^001";
            public const string PENDING_VERIFIKASI = "X285^002";
            public const string KLAIM = "X285^003";
        }

        public static class BPJSObjectType
        {
            public const string EKLAIM_DIAGNOSE = "X358^001";
            public const string EKLAIM_PROCEDURE = "X358^002";
            public const string BPJS_REFERENCE_DIAGNOSA = "X358^003";
            public const string BPJS_REFERENCE_POLI = "X358^004";
            public const string BPJS_REFERENCE_FASILITAS_KESEHATAN = "X358^005";
            public const string BPJS_REFERENCE_DOKTER_DPJP = "X358^006";
            public const string BPJS_REFERENCE_PROPINSI = "X358^007";
            public const string BPJS_REFERENCE_KABUPATEN = "X358^008";
            public const string BPJS_REFERENCE_KECAMATAN = "X358^009";
            public const string BPJS_REFERENCE_PROCEDURE = "X358^010";
            public const string BPJS_REFERENCE_KELASRAWAT = "X358^011";
            public const string BPJS_REFERENCE_DOKTER = "X358^012";
            public const string BPJS_REFERENCE_SPESIALISTIK = "X358^013";
            public const string BPJS_REFERENCE_RUANGRAWAT = "X358^014";
            public const string BPJS_REFERENCE_CARAKELUAR = "X358^015";
            public const string BPJS_REFERENCE_PASCAPULANG = "X358^016";
            public const string EKLAIM_INA_DIAGNOSE = "X358^018";
            public const string EKLAIM_INA_PROCEDURE = "X358^019";
        }

        public static class SIRANAPObjectType
        {
            public const string SIRANAP_REFERENCE_RUANG_TEMPAT_TIDUR = "X469^001";
            public const string SIRANAP_REFERENCE_SDM = "X469^002";
            public const string SIRANAP_REFERENCE_APD = "X469^003";
        }

        public static class AppointmentMethod
        {
            public const string CALLCENTER = "X359^001";
            public const string MOBILE = "X359^002";
            public const string KIOSK = "X359^003";
            public const string GOSHOW = "X359^004";
            public const string TAMBAH_KUNJUNGAN = "X359^005";
        }

        public static class LIS_Bridging_Protocol
        {
            public const string WEB_API = "X232^001";
            public const string CSV = "X232^002";
            public const string HL7 = "X232^003";
            public const string LINK_DB = "X232^004";
        }

        public static class RIS_Bridging_Protocol
        {
            public const string WEB_API = "X232^001";
            public const string CSV = "X232^002";
            public const string HL7 = "X232^003";
            public const string LINK_DB = "X232^004";
        }

        public static class OIS_Bridging_Protocol
        {
            public const string WEB_API = "X232^001";
            public const string CSV = "X232^002";
            public const string HL7 = "X232^003";
            public const string LINK_DB = "X232^004";
        }

        public static class RIS_Bridging_Status
        {
            public const string OPEN = "X007^01";
            public const string SENT = "X007^02";
            public const string SCHEDULED = "X007^03";
            public const string IN_PROGRESS = "X007^04";
            public const string DISCONTINUE = "X007^05";
            public const string FINISH = "X007^06";
        }

        public static class LIS_Bridging_Status
        {
            public const string OPEN = "X007^01";
            public const string SENT = "X007^02";
            public const string SCHEDULED = "X007^03";
            public const string IN_PROGRESS = "X007^04";
            public const string DISCONTINUE = "X007^05";
            public const string FINISH = "X007^06";
        }

        public static class SatuSehat_Bridging_Status
        {
            public const string OPEN = "X562^001";
            public const string READY = "X562^002";
            public const string SENT = "X562^003";
            public const string PENDING = "X562^004";
        }

        public static class Patient_BlackList_Reason
        {
            public const string PASIEN_KABUR = "X297^001";
            public const string PASIEN_KREDIT_OVER_LIMIT = "X297^002";
            public const string OTHER = "X297^003";
        }

        public static class Patient_Communication
        {
            public const string TUNA_WICARA = "X368^001";
            public const string TUNA_RUNGU = "X368^002";
            public const string OTHER = "X368^003";
        }

        public static class FileType
        {
            public const string IMAGE = "X036^001";
            public const string PDF = "X036^002";
            public const string VIDEO = "X036^003";
        }

        public static class AVPU
        {
            public const string ALERT = "X040^A";
            public const string VERBAL = "X040^V";
            public const string PAIN = "X040^P";
            public const string UNRESPONSIVE = "X040^U";
        }

        public static class PatientNoteType
        {
            public const string INFORMASI_TEMPAT_TIDUR = "X312^001";
            public const string CATATAN_GENERAL = "X312^002";
            public const string CATATAN_BILLING = "X312^003";
            public const string INFORMASI_DIKUNJUNGI = "X312^004";
            public const string CATATAN_KLAIM = "X312^005";
        }

        public static class DocumentNoteType
        {
            public const string CUSTOMER_CONTRACT = "X313^001";
            public const string PURCHASE_ORDER = "X313^002";
        }

        public static class ServiceStatus
        {
            public const string OPEN = "X321^001";
            public const string PROPOSED = "X321^002";
            public const string ON_DELIVERY = "X321^003";
            public const string PROCESSED = "X321^004";
            public const string WASHING = "X321^005";
            public const string PACKAGING = "X321^006";
            public const string STERILITATION = "X321^007";
            public const string QUALITY_CONTROL = "X321^008";
            public const string ON_STORAGE = "X321^009";
            public const string ON_RETURN = "X321^010";
            public const string RECEIVED = "X321^011";
            public const string VOID = "X321^999";
        }

        public static class ERPatientTransferStatus
        {
            public const string SHIFT_CHANGED = "X073^001";
            public const string PATIENT_IN_MY_AREA = "X073^002";
            public const string OTHER = "X073^999";
        }

        public static class RIS_HL7MessageFormat
        {
            public const string MEDAVIS = "X081^01";
            public const string NovaRAD = "X081^02";
            public const string INFINITT = "X081^03";
            public const string FUJI_FILM = "X081^04";
            public const string ZED = "X081^05";
            public const string MEDSYNAPTIC = "X081^06";
            public const string JIVEX = "X081^07";
            public const string INFINITT_RSSES = "X081^08";
            public const string LIFETRACK = "X081^10";
        }

        public static class OIS_HL7MessageFormat
        {
            public const string ARIA = "X580^001";
        }

        public static class LIS_PROVIDER
        {
            public const string HCLAB = "X086^01";
            public const string WYNACOM = "X086^02";
            public const string SOFTMEDIX = "X086^03";
            public const string ELIMPSE = "X086^04";
            public const string GRACIA = "X086^05";
        }

        public static class LIS_HL7MessageFormat
        {
            public const string ROCHE = "X081^06";
        }

        public static class NurseHandOverType
        {
            public const string SHIFT = "X076^01";
            public const string PINDAH_BANGSAL = "X076^02";
            public const string LAYANAN_PENUNJANG = "X076^03";
        }

        public static class NursePatientTransferStatus
        {
            public const string OPEN = "X078^01";
            public const string PROPOSED = "X078^02";
            public const string CONFIRMED = "X078^03";
            public const string VOID = "X078^99";
        }

        public static class ElectronicSignatureType
        {
            public const string INTEGRATION_NOTES = "X077^001";
            public const string PATIENT_EDUCATION = "X077^002";
            public const string CONSENT_FORM = "X077^003";
        }

        public static class HealthcareGatewayProvider
        {
            public const string RSMD = "X363^001";
            public const string RSDOSKA = "X363^002";
            public const string RSDOSOBA = "X363^003";
        }

        public static class ClinicStatus
        {
            public const string OPEN = "X365^001";
            public const string STARTED = "X365^002";
            public const string PAUSED = "X365^003";
            public const string STOPPED = "X365^004";
        }

        public static class PausedReason
        {
            public const string VISIT = "X364^001";
            public const string OTHERS = "X364^002";
        }

        public static class ClaimStatus
        {
            public const string OPEN = "X380^001";
            public const string APPROVED = "X380^002";
            public const string VOID = "X380^999";
        }

        public static class FinalStatus
        {
            public const string OPEN = "X381^001";
            public const string APPROVED = "X381^002";
            public const string VOID = "X381^999";
        }

        public static class FinalClaimFeedbackStatus
        {
            public const string MENUNGGU_UMPAN_BALIK = "X418^001";
            public const string TAGIHAN_DISETUJUI = "X418^002";
            public const string TAGIHAN_PENDING = "X418^003";
            public const string TAGIHAN_TIDAK_DISETUJUI = "X418^004";
        }

        public static class ReferralType
        {
            public const string KONSULTASI = "X075^01";
            public const string RAWAT_BERSAMA = "X075^02";
            public const string ALIH_RAWAT = "X075^03";
            public const string KUNJUNGAN_LANGSUNG = "X075^04";
            public const string APPOINTMENT = "X075^05";
            public const string HANDOVER = "X075^06";
        }

        public static class JournalTemplateType
        {
            public const string ALOKASI = "X389^001";
            public const string TEMPLATE = "X389^002";
        }

        public static class RevenueReduction
        {
            public const string REDUCTION_0 = "X390^001";
            public const string REDUCTION_10 = "X390^002";
            public const string REDUCTION_15 = "X390^003";
        }

        public static class RevenuePaymentMethod
        {
            public const string TUNAI = "X391^001";
            public const string TRANSFER = "X391^002";
            public const string BULANAN = "X391^003";
        }

        public static class RevenuePeriodeType
        {
            public const string TANGGAL_PELUNASAN = "X392^001";
            public const string TANGGAL_TRANSAKSI = "X392^002";
        }

        public static class PurchasingType
        {
            public const string RUTIN = "X394^001";
            public const string NON_RUTIN = "X394^002";
        }

        public static class AuditLog
        {
            public const string Patient = "X017^001";
            public const string Customer = "X017^002";
            public const string Supplier = "X017^003";
            public const string Item = "X017^004";
            public const string User = "X017^005";
            public const string Referrer = "X017^006";
            public const string LaboratoriumRujukan = "X017^007";
            public const string RegistrationBPJS = "X017^008";
            public const string HomeAddress = "X017^009";
            public const string OfficeAddress = "X017^010";
            public const string OtherAddress = "X017^011";
            public const string PatientTagField = "X017^012";
            public const string PurchaseOrder = "X017^013";
            public const string Registration = "X017^014";
            public const string Visit = "X017^015";
            public const string SettingParameter = "X017^016";
            public const string SettingParameterDt = "X017^017";
            public const string Appointment = "X017^018";
            public const string Address = "X017^019";
            public const string PatientDB = "X017^020";
            public const string ARReceivingHdDB = "X017^021";
            public const string RegistrationBillingDB = "X017^022";
            public const string ItemSupplierDB = "X017^023";
            public const string ItemAlternateUnit = "X017^024";
        }

        public static class AssessmentStatus
        {
            public const string OPEN = "X395^01";
            public const string COMPLETED = "X395^02";
            public const string VERIFIKASI_PP = "X395^03";
            public const string VERIFIKASI_DPJP = "X395^04";
            public const string DIREVISI = "X395^05";
        }

        public static class MedicalResumeStatus
        {
            public const string OPEN = "X544^001";
            public const string COMPLETED = "X544^002";
            public const string REVISED = "X544^003";
        }

        public static class MailNotificationStatus
        {
            public const string OPENED = "X408^001";
            public const string SEND_PROCESSED = "X408^002";
        }

        public static class MailTypeOrder
        {
            public const string LAPORAN_PEMBAYARAN_HONDOK = "X409^001";
            public const string SLIP_HONOR_DOKTER = "X409^002";
            public const string HASIL_LABORATORIUM = "X409^003";
        }

        public static class ScheduleType
        {
            public const string OPERATING_ROOM = "X448^01";
        }

        public static class ScheduleStatus
        {
            public const string OPEN = "X449^01";
            public const string STARTED = "X449^02";
            public const string COMPLETED = "X449^03";
        }

        public static class AssessmentFormGroup
        {
            public const string KEPERAWATAN_UMUM = "X397^001";
            public const string KEPERAWATAN_KAMAR_OPERASI = "X397^002";
            public const string KEPERAWATAN_HEMODIALISA = "X397^003";
            public const string KEPERAWATAN_KAMAR_BERSALIN = "X397^004";
            public const string KEPERAWATAN_GIZI = "X397^005";
            public const string DOKTER_UMUM = "X397^101";
            public const string DOKTER_BEDAH_ANESTESI = "X397^102";
        }

        public static class FluidBalanceGroup
        {
            public const string Intake = "X459^01";
            public const string Output = "X459^02";
            public const string Output_Tidak_Diukur = "X459^03";
            public const string Intake_Tidak_Diukur = "X459^04";
        }

        public static class PatientOwnerStatus
        {
            public const string INTERNAL = "X379^01";
            public const string EXTERNAL = "X379^02";
        }

        public static class VitalSignAssessmentType
        {
            public const string PERIOPERATIVE_PRE = "01";
            public const string PERIOPERATIVE_POST = "02";
            public const string HSU_NURSING_ASSESSMENT = "03";
            public const string INTRA_HEMODIALYSIS = "04";
            public const string POST_HEMODIALYSIS = "05";
            public const string ANESTHESY_STATUS = "06";
            public const string VENTILATOR = "07";
            public const string PARTOGRAF_MOTHER = "08";
            public const string RADIOTHERAPY_BRACHYTHERAPY = "09";
        }

        public static class FluidBalanceAssessmentType
        {
            public const string INTRA_HEMODIALYSIS = "04";
            public const string INTRA_ANESTHESY = "05";
        }

        public static class SurgeryTeamRole
        {
            public const string OPERATOR = "X451^01";
            public const string ANESTESI = "X451^02";
            public const string ASISTEN_DOKTER = "X451^03";
            public const string ASISTEN_PERAWAT = "X451^04";
            public const string PERAWAT_INSTRUMEN = "X451^05";
            public const string PERAWAT_ON_LOOP = "X451^06";
            public const string PENATA_ANESTESI = "X451^07";
        }

        public static class ExtensionFile
        {
            public const string XLS = "xls";
            public const string XLSX = "xlsx";
        }

        public static class NursingEvaluationType
        {
            public const string Subjective = "X396^001";
            public const string Objective = "X396^002";
            public const string Assessment = "X396^003";
            public const string Planning = "X396^004";
        }

        public static class IdentityCardType
        {
            public const string KTP = "X097^001";
            public const string SIM = "X097^002";
            public const string KITAS = "X097^003";
            public const string PASSPORT = "X097^004";
        }

        public static class PromotionType
        {
            public const string DETAIL = "X415^001";
            public const string GLOBAL = "X415^002";
        }

        public static class Format_Label_Gizi
        {
            public const string RSSES = "X489^008";
        }

        public static class ResultDeliveryPlan
        {
            public const string EMAIL = "X546^001";
            public const string WHATSAPP = "X546^002";
            public const string OTHERS = "X546^999";
        }

        public static class FHIRResourceType
        {
            public const string ENCOUNTER = "Encounter";
            public const string IMMUNIZATION = "Immunization";
            public const string LOCATION = "Location";
            public const string ORGANIZATION = "Organization";
        }

        public static class IHSCodingSystemUrl
        {
            public const string Organization = "http://sys-ids.kemkes.go.id/organization/";
            public const string OrganizationType = "http://terminology.hl7.org/CodeSystem/organization-type";
            public const string Location = "http://sys-ids.kemkes.go.id/location/";
            public const string LocationPhysicialType = "http://terminology.hl7.org/CodeSystem/location-physical-type";
            public const string KFA = "http://sys-ids.kemkes.go.id/kfa";
            public const string VaccinationRoute = "http://www.whocc.no/atc";
            public const string VaccinationPerformer = "http://terminology.hl7.org/CodeSystem/v2-0443";
            public const string VaccinationReason = "https://terminology.kemkes.go.id/CodeSystem/immunization-reason";
            public const string VaccinationTiming = "https://terminology.kemkes.go.id/CodeSystem/immunization-routine-timing";
            public const string VaccinationReportOrigin = "http://terminology.hl7.org/CodeSystem/immunization-origin";
        }

        public static class AgeUnit
        {
            public const string HARI = "X008^001";
            public const string MINGGU = "X008^002";
            public const string BULAN = "X008^003";
            public const string TAHUN = "X008^004";
        }

        public static class EKlaimCaraKeluar
        {
            public const string ATAS_PERSETUJUAN = "X545^001";
            public const string DIRUJUK = "X545^002";
            public const string PERMINTAAN_SENDIRI = "X545^003";
            public const string MENINGGAL = "X545^004";
            public const string LAIN_LAIN = "X545^999";
        }

        public static class EKlaimJaminanCaraBayar
        {
            public const string JKN = "X555^001";
            public const string JAMINAN_COVID19 = "X555^002";
            public const string JAMINAN_KIPI = "X555^003";
            public const string JAMINAN_BAYI_BARU_LAHIR = "X555^004";
            public const string JAMINAN_PERPANJANGAN_MASA_RAWAT = "X555^005";
            public const string JAMINAN_CO_INSIDENSE = "X555^006";
            public const string JAMPERSAL = "X555^007";
        }

        public static class EKlaimCaraMasuk
        {
            public const string RUJUKAN_FKTP = "X557^001";
            public const string RUJUKAN_FKRTL = "X557^002";
            public const string RUJUKAN_DOKTER_SPESIALIS = "X557^003";
            public const string DARI_RAWAT_JALAN = "X557^004";
            public const string DARI_RAWAT_INAP = "X557^005";
            public const string DARI_RAWAT_DARURAT = "X557^006";
            public const string LAHIR_DI_RUMAH_SAKIT = "X557^007";
            public const string RUJUKAN_DARI_PANTI_JOMPO = "X557^008";
            public const string RUJUKAN_DARI_RUMAH_SAKIT_JIWA = "X557^009";
            public const string RUJUKAN_DARI_FASILITAS_REHABILITASI = "X557^010";
            public const string LAIN_LAIN = "X557^999";
        }

        public static class BloodBankSourceType
        {
            public const string PMI = "X533^001";
            public const string PERSEDIAAN_BDRS = "X533^002";
            public const string PENDONOR = "X533^003";
        }

        public static class ParamedicTemplateTextType
        {
            public const string NURSING_JOURNAL_TEMPLATE = "X577^001";
        }

        public static class AntibioticCategory
        {
            public const string ACCESS = "X547^001";
            public const string WATCH = "X547^002";
            public const string RESERVED = "X547^003";
            public const string NO_CLASSIFIED = "X547^004";
        }

        public static class RadiotherapyType
        {
            public const string EXTERNAL = "X582^001";
            public const string BRACHYTHERAPY = "X582^002";
        }

        public static class RadiotherapyPurpose
        {
            public const string KURATIF = "X583^001";
            public const string PALIATIF = "X583^002";
            public const string BOOSTER_EXTERNAL = "X583^003";
        }
    }
}