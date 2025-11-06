using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Data.Service.Helper
{
    public class Constant
    {
        public static class AccountType
        {
            public const string AKTIVA = "X180^001";
            public const string KEWAJIBAN = "X180^002";
            public const string MODAL_EKUITAS = "X180^003";
        }

        public static class ItemStatus
        {
            public const string ACTIVE = "X181^001";
            public const string IN_ACTIVE = "X181^002";
        }

        public static class DiagnosisStatus
        {
            public const string RULED_OUT = "X031^003";
        }

        public static class DiagnoseType
        {
            public const string EARLY_DIAGNOSIS = "X029^000";
            public const string MAIN_DIAGNOSIS = "X029^001";
            public const string COMPLICATION = "X029^002";
            public const string EXTERNAL_CAUSE = "X029^003";
        }

        public static class DischargeMethod
        {
            public const string REFFERRED_TO_OUTPATIENT = "X052^002";
            public const string REFFERRED_TO_EXTERNAL_PROVIDER = "X052^003";
            public const string FORCE_DISCHARGE = "X052^006";
            public const string DISCHARGED_TO_WARD = "X052^007";
        }

        public static class DosingFrequency
        {
            public const string HOUR = "X130^001";
            public const string DAY = "X130^002";
            public const string WEEK = "X130^999";
        }

        public static class Facility
        {
            public const string OUTPATIENT = "OUTPATIENT";
            public const string INPATIENT = "INPATIENT";
            public const string DIAGNOSTIC = "DIAGNOSTIC";
            public const string PHARMACY = "PHARMACY";
            public const string EMERGENCY = "EMERGENCY";
        }

        public static class FormatString
        {
            public const string DATE_FORMAT = "dd-MMM-yyyy";
            public const string DATE_FORMAT_2 = "yyyyMMdd";
            public const string DATE_FORMAT_3 = "dd/MM/yy";
            public const string DATE_FORMAT_4 = "yyyy-MM-dd";
            public const string DATE_FORMAT_5 = "dd-MMM";
            public const string DATE_FORMAT_6 = "dd/MM/yyyy";
            public const string DATE_PICKER_FORMAT = "dd-MM-yyyy";
            public const string DATE_TIME_FORMAT = "dd-MMM-yyyy HH:mm:ss";
            public const string DATE_TIME_FORMAT_2 = "dd-MMM-yyyy HH:mm";
            public const string DAY_DATE_FORMAT = "dddd, dd MMMM yyyy";
            public const string DAY_DATE_TIME_FORMAT = "dddd, dd MMMM yyyy HH:mm:ss";
            public const string TIME_FORMAT = "HH:mm";
            public const string TIME_FORMAT_2 = "HH:mm:ss";
            public const string NUMERIC_2 = "N2";
            public const string NUMERIC_3 = "N3";
            public const string NUMERIC_INT = "G16";
        }

        public static class ConstantDate
        {
            public const string DEFAULT_NULL = "01-01-1900";
            public const string DEFAULT_NULL_DATE_FORMAT = "01-Jan-1900";
        }
        public static class SpecialField
        {
            public const string MRN = "#MRN";
            public const string GUESTNO = "#GUESTNO";
            public const string BUSINESSPARTNERCODE = "#BusinessPartnerCode";
        }

        public static class SettingParameter
        {
            public const string LB_BRIDGING_LIS = "LB0003";
        }

        public static class StandardCode
        {
            public static class PaymentType
            {
                public const string DOWN_PAYMENT = "X034^001";
                public const string SETTLEMENT = "X034^002";
                public const string AR_PATIENT = "X034^003";
                public const string AR_PAYER = "X034^004";
                public const string CUSTOM = "X034^005";
            }
            public static class PaymentMethod
            {
                public const string CASH = "X035^001";
                public const string CREDIT_CARD = "X035^002";
                public const string DEBIT_CARD = "X035^003";
                public const string BANK_TRANSFER = "X035^004";
                public const string PIUTANG = "X035^005";
                public const string UANG_TITIPAN_KELUAR = "X035^006";
                public const string PENGEMBALIAN_PEMBAYARAN = "X035^007";
                public const string VOUCHER = "X035^008";
                public const string DEPOSIT_OUT = "X035^009";
                public const string TRANSFER_TRANSAKSI = "X035^010";
                public const string UANG_TITIPAN = "X035^011";
                public const string BILLING_MCU_RI = "X035^012";
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
            }
            public static class AR_PAYMENT_METHODS
            {
                public const string CASH = "X254^001";
                public const string BANK_TRANSFER = "X254^002";
                public const string CREDIT_CARD = "X254^003";
                public const string DEBIT_CARD = "X254^004";
                public const string WRITE_OFF = "X254^005";
            }
            public static class Gender
            {
                public const string MALE = "0003^M";
                public const string FEMALE = "0003^F";
            }
            public static class RegistrationStatus
            {
                public const string OPEN = "X020^001";
                public const string CHECKED_IN = "X020^002";
                public const string RECEIVING_TREATMENT = "X020^003";
                public const string PHYSICIAN_DISCHARGE = "X020^004";
                public const string PAID = "X020^005";
                public const string CANCELLED = "X020^006";
                public const string CLOSED = "X020^007";
            }

            public static class OrderStatus
            {
                public const string OPEN = "X126^001";
                public const string RECEIVED = "X126^002";
                public const string IN_PROGRESS = "X126^003";
                public const string CANCELLED = "X126^004";
                public const string COMPLETED = "X126^005";
                public const string CLOSED = "X126^006";
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

            public static class DistributionStatus
            {
                public const string OPEN = "X160^001";
            }

            public static class PatientTransferStatus
            {
                public const string OPEN = "X143^001";
                public const string TRANSFERRED = "X143^002";
                public const string CANCELLED = "X143^003";
            }

            public static class ToBePerformed
            {
                public const string CURRENT_EPISODE = "X125^001";
                public const string PRIOR_TO_NEXT_VISIT = "X125^002";
                public const string SCHEDULLED = "X125^003";
            }
            public static class DosingFrequency
            {
                public const string HOUR = "X130^001";
                public const string DAY = "X130^002";
                public const string WEEK = "X130^999";
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

            public static class PatientOutcome
            {
                public const string DIED_LESS_THAN_48_HOURS = "0241^004";
                public const string DIED_MORE_THAN_48_HOURS = "0241^005";
            }

            public static class NursingDomainClass
            {
                public const string DOMAIN = "X197^001";
                public const string CLASS = "X197^002";
            }

            public static class LaboratoryResultValue
            {
                public const string TEXT = "X002^001";
                public const string NUMERIC = "X002^002";
            }

            public static class CustomerType
            {
                public const string INSURANCE = "X004^100";
                public const string HEALTHCARE = "X004^400";
                public const string PERSONAL = "X004^999";
                public const string BPJS = "X004^500";
            }

            public static class ParamedicType
            {
                public const string Physician = "X019^001";
                public const string Nurse = "X019^002";
                public const string Midwife = "X019^003";
                public const string Physiotheraphist = "X019^004";
                public const string Laboratory_Analysis = "X019^006";
                public const string Pharmacist = "X019^008";
                public const string Ass_Pharmacist = "X019^009";
                public const string Nutritionist = "X019^011";
                public const string Okupasi_Terapis = "X019^012";
                public const string Terapis_Wicara = "X019^013";
            }

            public static class PatientStatus
            {                
                public const string ACTIVE = "X256^01";
                public const string RETENTION = "X256^02";
                public const string ARCHIEVED = "X256^03";
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
            }

            public static class AppointmentMethod
            {
                public const string MOBILE_APPS = "X359^002";
            }

            public static class ServiceStatus
            {
                public const string OPEN = "X321^001";
                public const string PROPOSED = "X321^002";
                public const string ON_DELIVERY = "X321^003";
                public const string PROCESSED = "X321^004";
                public const string ON_RETURN = "X321^005";
                public const string RECEIVED = "X321^006";
                public const string VOID = "X321^999";
            }

            public static class NursePatientTransferStatus
            {
                public const string OPEN = "X078^01";
                public const string PROPOSED = "X078^02";
                public const string CONFIRMED = "X078^03";
                public const string VOID = "X078^99";
            }

            public static class AssessmentStatus
            {
                public const string OPEN = "X395^01";
                public const string COMPLETED = "X395^02";
                public const string VERIFIKASI_PP = "X395^03";
                public const string VERIFIKASI_DPJP = "X395^04";
            }

            public static class RevenueSharingAdjustmentGroup
            {
                public const string PENAMBAHAN = "X166^001";
                public const string PENGURANGAN = "X166^002";
            }

            public static class ScheduleStatus
            {
                public const string OPEN = "X449^01";
                public const string STARTED = "X449^02";
                public const string COMPLETED = "X449^03";
            }

            public static class ReferralType
            {
                public const string TEAM_DOKTER = "X075^01";
                public const string APPOINTMENT = "X075^05";
            }

            public static class MedicalResumeStatus
            {
                public const string OPEN = "X544^001";
                public const string COMPLETED = "X544^002";
                public const string REVISED = "X544^003";
            }
        }
    }
}
