using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Data.Service;

namespace QIS.Medinfras.Web.Common
{
    public static class EventCode
    {
        #region Master Dokter
        public const string SCH_001 = "SCH-001";
        public const string SCH_002 = "SCH-002";
        public const string SCH_003 = "SCH-003";
        public const string SCH_004 = "SCH-004";
        public const string SCH_005 = "SCH-005";
        public const string SCH_006 = "SCH-006";
        #endregion

        #region Appointment
        public const string SCH_007 = "SCH-007";
        public const string SCH_008 = "SCH-008";
        public const string SCH_009 = "SCH-009";
        public const string SCH_010 = "SCH-010";
        public const string SCH_011 = "SCH-011";
        public const string SCH_012 = "SCH-012";
        #endregion

        #region Jadwal Dokter Per Tanggal
        public const string SCH_013 = "SCH-013";
        public const string SCH_014 = "SCH-014";
        public const string SCH_015 = "SCH-015";
        #endregion
    }

    public class EventMessage
    {
        public string EventCode { get; set; }
        public string Remarks { get; set; }
        public string Data { get; set; }
    }

    public class EventResponse
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Data { get; set; }
    }

    #region RS DR. OEN KANDANG SAPI SURAKARTA
    public class PhysicianScheduleUpdate
    {
        public string jenisJadwal { get; set; } //1 = jadwal rutin, 2 = jadwal per tanggal, 3 = jadwal cuti
        public string serviceUnitID { get; set; }
        public string physicianID { get; set; }
        public string user { get; set; }
    }

    public class ClinicStatusNotificationChanged
    {
        public string jenisNotif { get; set; } //1= klinik buka, 2 = klinik tutup sementara, 3 = klinik buka kembali, 4 = pasien segera diperiksa
        public string noAntrian { get; set; }
        public string serviceUnitID { get; set; }
        public string physicianID { get; set; }
        public string keterangan { get; set; }
        public string user { get; set; }
    }

    public class SendPdfStream
    {
        public string stream { get; set; }
        public string filename { get; set; }
    }

    public class SendNotificationRegistration
    {
        public int appointmentID { get; set; }
        public string appointmentNo { get; set; }
        public string registrationNo { get; set; }
        public string user { get; set; }
    }
    #endregion

    #region RS DR. OEN SOLO BARU

    public class GetQueueBodyRequest
    {
        public string rm { get; set; }
        public string nama { get; set; }
        public string tgllahir { get; set; }
        public string jnskelamin { get; set; }
        public string noktp { get; set; }
        public string nobpjs { get; set; }
        public string str { get; set; }
        public string tipe { get; set; }
        public string tanggal { get; set; }
        public string kodejam { get; set; }
        public int sesi { get; set; }
        public string via { get; set; }
    }

    public class GetQueueResponse
    {
        public string status { get; set; }
        public string type { get; set; }
        public string antrian { get; set; }
        public string mr { get; set; }
        public string dokter { get; set; }
        public string spesialis { get; set; }
        public string date { get; set; }
        public string time { get; set; }
        public string message { get; set; }
        public string notifikasi { get; set; }
    }

    public class ResetQueueBodyRequest
    {
        public string rm { get; set; }
        public string tanggal { get; set; }
        public string str { get; set; }
        public string serviceunit { get; set; }
        public string kodejam { get; set; }
        public string noantrian { get; set; }
    }

    public class ResetQueueResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ChangePhysicianBodyRequest
    {
        public string rm { get; set; }
        public string tanggallama { get; set; }
        public string tanggalbaru { get; set; }
        public string kddokterlama { get; set; }
        public string kddokterbaru { get; set; }
        public string kodejamlama { get; set; }
        public string kodejambaru { get; set; }
        public string antrianlama { get; set; }
        public string penjaminbaru { get; set; }
        public string via { get; set; }
    }

    public class ChangePhysicianResponse
    {
        public string status { get; set; }
        public string type { get; set; }
        public string antrian { get; set; }
        public string kddokter { get; set; }
        public string mr { get; set; }
        public string date { get; set; }
        public string message { get; set; }
    }

    public class ChangeMasterPhysicianScheduleRoutineBodyRequest
    {
        public string eventcode { get; set; }
        public string kddokter { get; set; }
        public string hari { get; set; }
        public string jamstart { get; set; }
        public string jamend { get; set; }
        public string tipepoli { get; set; }
        public string qmax { get; set; }
        public string serviceunit { get; set; }
        public string specialty { get; set; }
        public string kodejam { get; set; }
        public string qapp { get; set; }
        public string petugas { get; set; }
    }

    public class ChangeMasterPhysicianScheduleRoutineResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ChangeMasterPhysicianScheduleByDateBodyRequest
    {
        public string eventcode { get; set; }
        public string kddokter { get; set; }
        public string serviceunit { get; set; }
        public string specialty { get; set; }
        public string kodejam { get; set; }
        public string tanggal { get; set; }
        public string jamstart { get; set; }
        public string jamend { get; set; }
        public string tipepoli { get; set; }
        public string qmax { get; set; }
        public string qapp { get; set; }
    }

    public class ChangeMasterPhysicianScheduleByDateResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    public class ChangeMasterPhysicianLeaveBodyRequest
    {
        public string eventcode { get; set; }
        public string kddokter { get; set; }
        public string qmaxreguler { get; set; }
        public string qmaxnonreguler { get; set; }
        public string tanggalmulai { get; set; }
        public string tanggalakhir { get; set; }
        public string alasancuti { get; set; }
    }

    public class ChangeMasterPhysicianLeaveResponse
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    #endregion

    #region Bridging EMR v1
    public class SendPatientClinicalInformation
    {
        public int ProcessType { get; set; }
        public string RegistrationNo { get; set; }
        public PatientDiagnosis Diagnosis { get; set; }
        public PatientProcedure Procedures { get; set; }
    }

    public class SendOrderInfo
    {
        public int ProcessType { get; set; }
        public TestOrderHd TestOrderHd { get; set; }
        public PrescriptionOrderHd PrescriptionOrderHd { get; set; }
        public List<vPrescriptionOrderDt1> ListPrescriptionOrderDt { get; set; }
    }

    public class PV1_BodyRequest
    {
        public string RegistrationNo { get; set; }
    }
    #endregion
}
