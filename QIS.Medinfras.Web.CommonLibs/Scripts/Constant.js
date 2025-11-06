var Constant = new (function () {
    this.StandardCode = new (function () {
        this.MARITAL_STATUS = "0002";
        this.ETHNIC = '0005';
        this.RELIGION = '0006';
        this.NATIONALITY = "0212";
        this.PROVINCE = "0347";
        this.OCCUPATION = "X012";
        this.EDUCATION = "X013";
        this.PATIENT_CATEGORY = "X067";
    })();
        
    this.SettingParameter = new (function () {
        this.RM_DEFAULT_PATIENT_WALKIN = "RM0033";
        this.RM_DEFAULT_PHARMACY_VISITTYPE = 'RM0034';
    })();

    this.MRStatusNote = new (function () {
        this.TULISAN_TIDAK_JELAS = "X222^01";
        this.TIDAK_ADA_TANDA_TANGAN_DOKTER = "X222^02";
        this.INFORMASI_TIDAK_LENGKAP = "X222^03";
        this.IDENTIFIKASI = "X222^04";
        this.OTENTIFIKASI = "X222^05";
        this.PENCATATAN = "X222^06";
        this.PELAPORAN = "X222^07";
        this.OTHERS = "X222^99";
    })();

    this.LocationGroup = new (function () {
        this.DRUGS_AND_MEDICALSUPPLIES = "X227^1";
        this.LOGISTICS = "X227^2";
    })();

    this.ItemGroupMaster = new (function () {
        this.SERVICE = "X001^001";
        this.DRUGS = "X001^002";
        this.SUPPLIES = "X001^003";
        this.LABORATORY = "X001^004";
        this.RADIOLOGY = "X001^005";
        this.DIAGNOSTIC = "X001^006";
        this.PACKAGE = "X001^007";
        this.LOGISTIC = "X001^008";
        this.FOOD = "X001^009";
    })();

    this.ItemType = new (function () {
        this.PELAYANAN = "X001^001";
        this.OBAT_OBATAN = "X001^002";
        this.BARANG_MEDIS = "X001^003";
        this.LABORATORIUM = "X001^004";
        this.RADIOLOGI = "X001^005";
        this.PENUNJANG_MEDIS = "X001^006";
        this.MEDICAL_CHECKUP = "X001^007";
        this.BARANG_UMUM = "X001^008";
        this.BAHAN_MAKANAN = "X001^009";
    })();

    this.CustomerType = new (function () {
        this.BPJS = "X004^500";
        this.INHEALTH = "X004^990";
        this.PERSONAL = "X004^999";
    })();

    this.DosingFrequency = new (function () {
        this.HOUR = "X130^001";
        this.DAY = 'X130^002';
        this.WEEK = 'X130^999';
    })();

    this.DiscontinueMedicationReason = new (function () {
        this.OTHER = 'X136^999';
    })();

    this.DeleteReason = new (function () {
        this.OTHER = 'X129^999';
    })();

    this.OrderStatus = new (function () {
        this.OPEN = 'X126^001';
        this.RECEIVED = 'X126^002';
        this.INPROGRESS = 'X126^003';
        this.CANCELLED = 'X126^004';
        this.COMPLETED = 'X126^005';
        this.CLOSED = 'X126^006';
    })();

    this.TransactionStatus = new (function () {
        this.OPEN = 'X121^001';
        this.WAIT_FOR_APPROVAL = 'X121^002';
        this.APPROVED = 'X121^003';
        this.PROCESSED = 'X121^004';
        this.CLOSED = 'X121^005';
        this.VOID = 'X121^999';
    })();

    this.RegistrationStatus = new (function () {
        this.OPEN = 'X020^001';
        this.CHECKED_IN = 'X020^002';
        this.RECEIVING_TREATMENT = 'X020^003';
        this.PHYSICIAN_DISCHARGE = 'X020^004';
        this.DISCHARGED = 'X020^005';
        this.CANCELLED = 'X020^006';
        this.CLOSED = 'X020^007';
    })();

    this.FilterParameterType = new (function () {
        this.COMBO_BOX = "X108^001";
        this.CHECK_LIST = "X108^002";
        this.DATE = "X108^003";
        this.PAST_PERIOD = "X108^004";
        this.UPCOMING_PERIOD = "X108^005";
        this.FREE_TEXT = "X108^006";
        this.SEARCH_DIALOG = "X108^007";
        this.CUSTOM_COMBO_BOX = "X108^008";
        this.YEAR_COMBO_BOX = "X108^009";
        this.TEXT_BOX = "X108^010";
        this.CONSTANT = "X108^012";
    })();

    this.PurchaseOrderType = new (function () {
        this.DRUGMS = "X145^001";
        this.LOGISTIC = "X145^002";
        this.SERVICES = "X145^003";
        this.ASSET = "X145^004";
        this.FOOD = "X145^005";
        this.ALKES = "X145^006";
        this.COVID = "X145^007";
        this.DRUG_INPATIENT = "X145^998";
        this.DRUG_OUTPATIENT = "X145^999";
    })();

    this.VisitReason = new (function () {
        this.ACCIDENT = "X156^002";
        this.OTHER = "X156^999";
    })();

    this.SupplierPaymentMethod = new (function () {
        this.TUNAI = "X178^001";
        this.TRANSFER = "X178^002";
        this.GIRO = "X178^003";
        this.CHEQUE = "X178^004";
        this.CREDIT_CARD = "X178^005";
        this.DEBIT_CARD = "X178^006";
        this.KOREKSI_FAKTUR = "X178^007";
        this.ADMIN = "X178^008";
        this.DISCOUNT = "X178^009";
    })();

    this.ToBePerformed = new (function () {
        this.CURRENT_EPISODE = "X125^001";
        this.PRIOR_TO_NEXT_VISIT = "X125^002";
        this.SCHEDULLED = "X125^003";
    })();

    this.BedReservation = new (function () {
        this.OPEN = 'X263^001';
        this.PROPOSED = 'X263^002';
        this.COMPLETE = 'X263^003';
        this.CANCELLED = 'X263^999';
    })();

    this.AppointmentStatus = new (function () {
        this.CANCELLED = "0278^001";
        this.COMPLETE = "0278^002";
        this.DISCONTINUE = "0278^003";
        this.DELETED = "0278^004";
        this.NOSHOW = "0278^005";
        this.OVERBOOK = "0278^006";
        this.PENDING = "0278^007";
        this.STARTED = "0278^008";
        this.WAITINGLIST = "0278^009";
        this.CONFIRMED = "0278^010";
    })();

    this.AppointmentDeleteReason = new (function () {
        this.PARAMEDIC = "X296^001";
        this.OTHER = "X296^999";
    })();

    this.AdmissionSource = new (function () {
        this.EMERGENCY = "0023^X01";
        this.OUTPATIENT = "0023^X02";
        this.DIAGNOSTIC = "0023^X03";
        this.INPATIENT = "0023^X04";
    })();

    this.PaymentType = new (function () {
        this.CASH = "X035^001";
        this.CREDIT_CARD = 'X035^002';
        this.DEBIT_CARD = 'X035^003';
        this.BANK_TRANSFER = 'X035^004';
        this.PIUTANG = 'X035^005';
        this.UANG_MUKA_KELUAR = 'X035^006';
        this.PENGEMBALIAN_PEMBAYARAN = 'X035^007';
        this.VOUCHER = 'X035^008';
        this.DEPOSIT_OUT = 'X035^009';
        this.TRANSFER_TRANSAKSI = 'X035^010';
    })();

    this.PaymentType = new (function () {
        this.DOWN_PAYMENT = "X034^001";
        this.SETTLEMENT = "X034^002";
        this.AR_PATIENT = "X034^003";
        this.AR_PAYER = "X034^004";
        this.CUSTOM = "X034^005";
        this.DEPOSIT_IN = "X034^006";
        this.DEPOSIT_OUT = "X034^007";
    })();

    this.DeleteReason = new (function () {
        this.OTHER = "X129^999";
    })();

    this.DischargeReasonToOtherHospital = new (function () {
        this.OTHER = "X219^999";
    })();

    this.Facility = new (function () {
        this.DIAGNOSTIC = "DIAGNOSTIC";
        this.EMERGENCY = "EMERGENCY";
        this.INPATIENT = "INPATIENT";
        this.MCU = "MCU";
        this.OUTPATIENT = "OUTPATIENT";
        this.PHARMACY = "PHARMACY";
        this.LABORATORY = "LABORATORY";
        this.IMAGING = "IMAGING";
    })();

    this.AllergenType = new (function () {
        this.DRUG = "0127^DA";
    })();

    this.DiagnosisType = new (function () {
        this.DIFFERENT_DIAGNOSIS = "X029^000";
        this.MAIN_DIAGNOSIS = "X029^001";
        this.COMPLICATION = "X029^002";
        this.EXTERNAL_CAUSE = "X029^003";
    })();

    this.DiagnosisStatus = new (function () {
        this.CONFIRMED = "X031^001";
        this.UNDER_INVESTIGATION = "X031^002";
        this.RULED_OUT = "X031^003";
    })();

    this.PatientOutcome = new (function () {
        this.DEAD_BEFORE_48 = "0241^004";
        this.DEAD_AFTER_48 = "0241^005";
    })();

    this.DischargeRoutine = new (function () {
        this.ATAS_PERSETUJUAN = "X052^001";
        this.REFER_TO_OUTPATIENT = "X052^002";
        this.OTHER_HOSPITAL = "X052^003";
        this.GO_HOME_BY_OWN_REQUEST = "X052^004";
        this.RUNAWAY = "X052^005";
        this.FORCE_DISCHARGE = "X052^006";
        this.REFER_TO_INPATIENT = "X052^007";
        this.SEND_TO_MORTUARY = "X052^008";
        this.TRANSFERED_TO_UPH = "X052^009";
        this.OTHER_ROUTINE = "X052^999";
    })();

    this.ParamedicType = new (function () {
        this.Physician = "X019^001";
        this.Nurse = "X019^002";
        this.Midwife = "X019^003";
        this.Pharmacist = "X019^008";
        this.Nutritionist = "X019^011";
    })();

    this.ReferrerGroup = new (function () {
        this.DOKTERRS = 'X105^000';
        this.RUMAH_SAKIT = "X105^003";
        this.FASKES = 'X105^006';

    })();

    this.MenuCode = new (function () {
        this.FOLLOWUP_PATIENT_ER = 'ER010400';
        this.FOLLOWUP_PATIENT_IP = 'IP010400';
        this.FOLLOWUP_PATIENT_MD = 'MD020400';
        this.FOLLOWUP_PATIENT_OP = 'OP020400';
        this.ER_PATIENT_PAGE = "ER020100";
        this.OP_PATIENT_PAGE = "OP030400";
        this.MD_PATIENT_PAGE = 'MD030500';
        this.MC_PATIENT_PAGE = "MC030600";
        this.LB_PATIENT_PAGE = 'LB030600';
        this.EMR_PATIENT_PAGE = "EM09000";
        this.IMAGING_PATIENT_TRANSACTION_PAGE = "IS030400";
        this.IMAGING_PATIENT_PAGE = "IS030101";
        this.UDD_PRESCRIPTION_PROCESS = "PH020400";
        this.UDD_MEDICATION_LIST = "PH020420";
        this.PHARMACIST_CLINICAL = "PH040100";
        this.NURSING_PATIENT_PAGE = "NR020200";
        this.NURSING_PATIENT_LIST_INPATIENT = "NR022000";
        this.NUTRITION_PATIENT_LIST = "NT031000";
        this.PHAR_PATIENT_LIST = "NT031000";
        this.RADIOTHERAPHY_PATIENT_PAGE = "RT030101";
    })();

    this.ItemTypeLocation = new (function () {
        this.DRUG_SUPPLIES = 'X227^1';
        this.LOGISTICS = 'X227^2';
    })();

    this.ClinicGroup = new (function () {
        this.BPJS = 'X280^001';
        this.NON_BPJS = 'X280^002';
    })();

    this.BPJS_CLAIM_STATUS = new (function () {
        this.OPEN = '';
        this.PROSES_VERIFIKASI = 'X285^001';
        this.PENDING_VERIFIKASI = 'X285^002';
        this.KLAIM = 'X285^003';
    })();

    this.MedicationStatus = new (function () {
        this.OPEN = 'X258^001';
        this.DISPENSED = 'X258^002';
        this.TAKEN = 'X258^003';
        this.REFUSED = 'X258^004';
        this.PENDING = 'X258^005';
        this.DISCONTINUE = 'X258^999';
    })();

    this.TreasuryGroup = new (function () {
        this.SUPPLIER_PAYMENT = 'X290^001';
        this.AR_RECEIVING = 'X290^002';
        this.MEMORIAL = 'X290^003';
        this.KAS_BON = 'X290^004';
        this.DIRECT_PURCHASE = 'X290^005';
        this.REVENUE_SHARING = 'X290^006';
        this.KAS_BON = 'X290^007';
        this.SURAT_PERINTAH_KERJA = 'X290^008';
        this.PERMINTAAN_PEMBELIAN_TUNAI = 'X290^009';
        this.REALISASI_PEMBELIAN_TUNAI = 'X290^010';
        this.SETORAN_KASIR = 'X290^011';
        this.SETORAN_KASIR_REKONSILIASI = 'X290^012';
    })();

    this.TreasuryType = new (function () {
        this.PENERIMAAN = "X291^001";
        this.PENGELUARAN = "X291^002";
        this.PINDAH_BUKU = "X291^003";
        this.PERMINTAAN_KAS_BON = "X291^004";
        this.REALISASI_KAS_BON = "X291^005";
    })();

    this.PatientBlackListReason = new (function () {
        this.PASIEN_KABUR = "X297^001";
        this.PASIEN_KREDIT_OVER_LIMIT = "X297^002";
        this.OTHER = "X297^003";
    })();

    this.PatientStatus = new (function () {
        this.ACTIVE = "X256^01";
        this.RETENTION = "X256^02";
        this.ARCHIEVED = "X256^03";
    })();

    this.BankType = new (function () {
        this.BANK_KASIR = "X310^001";
        this.BANK_HUTANG = "X310^002";
        this.BANK_PIUTANG = "X310^003";
    })();

    this.Gender = new (function () {
        this.MALE = "0003^M";
        this.FEMALE = "0003^F";
        this.UNSPECIFIED = "0003^U";
    })();

    this.ItemRequestType = new (function () {
        this.DISTRIBUTION = "X217^01";
        this.CONSUMPTION = "X217^02";
    })();

    this.DiscountReason = new (function () {
        this.Dokter = "X155^001";
    })();

    this.ServiceStatus = new (function () {
        this.OPEN = "X321^001";
        this.PROPOSED = "X321^002";
        this.ON_DELIVERY = "X321^003";
        this.PROCESSED = "X321^004";
        this.ON_RETURN = "X321^005";
        this.RECEIVED = "X321^006";
        this.VOID = "X321^999";
    })();

    this.SOAPNoteType = new (function () {
        this.EMERGENCY_INITIAL_ASSESSMENT = "X011^002";
        this.INPATIENT_INITIAL_ASSESSMENT = "X011^004";
        this.NURSE_ANAMNESIS = "X011^005";
        this.SOAP_SUMMARY_NOTES = "X011^011";
        this.NUTRITION_ASSESSMENT = "X011^006";
    })();

    this.ItemStatus = new (function () {
        this.ITEM_STATUS_ACTIVE = "X181^001";
        this.ITEM_STATUS_IN_ACTIVE = "X181^999";
    })();

    this.Sender = new (function () {
        this.MD000 = "MD000";
        this.MD101 = "MD101";
        this.MD201 = "MD201"; //BCA ECR
        this.MD201 = "MD202"; //MTI ECR
    })();

    this.JournalTemplateType = new (function () {
        this.ALOKASI = "X389^001";
        this.TEMPLATE = "X389^002";
    })();

    this.PurchasingType = new (function () {
        this.RUTIN = "X394^001";
        this.NON_RUTIN = "X394^002";
    })();

    this.PrescriptionType = new (function () {
        this.MEDICATION_ORDER = "X137^001";
        this.DISCHARGE_PRESCRIPTION = "X137^002";
        this.TERAPI_BARU = "X137^005";
        this.PASIEN_BARU = "X137^010";
    })();

    this.VitalSignAssessmentType = new (function () {
        this.PERIOPERATIVE_PRE = "01";
        this.PERIOPERATIVE_POST = "02";
        this.HSU_NURSING_ASSESSMENT = "03";
        this.POST_HEMODIALYSIS = "05";
    })();

    this.ScheduleStatus = new (function () {
        this.OPEN = "X449^01";
        this.STARTED = "X449^02";
        this.COMPLETED = "X449^03";
    })();

    this.VersionBridgingBPJSVClaim = new (function () {
        this.v1_0 = "X484^001";
        this.v1_1 = "X484^002";
    })();

    this.BPJSPayer = new (function () {
        this.X496 = "X496";
    })();

    this.BusinessPartnerType = new (function () {
        this.PATIENT = "X017^001";
        this.CUSTOMER = "X017^002";
        this.SUPPLIER = "X017^003";
        this.ITEM = "X017^004";
        this.USER = "X017^005";
        this.REFERRER = "X017^006";
        this.RUJUKAN_KE_PIHAK_KETIGA = "X017^007";
    })();

    this.RefferalType = new (function () {
        this.KONSULTASI = "X075^01";
        this.RAWAT_BERSAMA = 'X075^02';
        this.ALIH_RAWAT = 'X075^03';
        this.KUNJUNGAN_LANGSUNG = 'X075^04';
        this.APPOINTMENT = 'X075^05';
    })();

    this.InfectiousDisease = new (function () {
        this.COVID19 = "X522^001";
        this.HEPATITISB = "X522^002";
        this.HIV = "X522^003";
        this.TBC = "X522^004";
        this.OTHERS = "X522^999";
    })();

    this.Comorbidities = new (function () {
        this.GANGGUAN_PERNAPASAN = "X523^001";
        this.DIABETES = "X523^002";
        this.HIPERTENSI = "X523^003";
        this.PENYAKIT_KARDIOVASKULAR = "X523^004";
        this.PENYAKIT_JANTUNG = "X523^005";
        this.OBESITAS = "X523^006";
        this.DEPRESI_KECEMASAN = "X523^007";
        this.KANKER_KELAINAN_DARAH = "X523^008";
        this.KEKEBALAN_TUBUH_LEMAH = "X523^009";
        this.PENYAKIT_GINJAL = "X523^010";
        this.OTHERS = "X523^999";
    })();

    this.TestPartnerType = new (function () {
        this.LABORATORIUM = "X230^001";
        this.RADIOLOGI = "X230^002";
    })();

    this.BeamTechnique = new (function () {
        this.OTHERS = "X572^999";
    })();

    this.RadioTherapyType = new (function () {
        this.BRACHYTHERAPY = "X582^002";
    })();

    this.BrachytherapyType = new (function () {
        this.Intrakaviter = "X584^001";
        this.Interstitial = "X584^002";
        this.Intravaginal = "X584^003";
        this.Intrakaviter_Interstitial = "X584^004";
    })();
    this.BornCondition = new (function () {
        this.Normal = "X148^001";
        this.Prematur = "X148^002";
        this.KembarSiam = "X148^003";
        this.Komplikasi = "X148^004";
        this.Meninggal = "X148^005";
    })();
})();