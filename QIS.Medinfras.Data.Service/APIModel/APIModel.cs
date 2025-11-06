using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Data.Service
{
    public class Person
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PrefferedName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string CityOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string HomeZipCode { get; set; }
        public string HomePhoneNo1 { get; set; }
        public string HomePhoneNo2 { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Nationality { get; set; }
        public string EmailAddress { get; set; }

    }

    public class PatientData : Person
    {
        public int PatientID { get; set; }
        public string MedicalNo { get; set; }
    }

    #region Paramedic
    public class Paramedic : Person
    {
        public string HealthcareID { get; set; }

        public Int32 ParamedicID { get; set; }
        public string ParamedicCode { get; set; }
        public string ParamedicName { get; set; }
        public string ParamedicType { get; set; }

        public int HomeAddressID { get; set; }

        public Metadata metadata { get; set; }
    }

    public class ParamedicScheduleDTO : Paramedic
    {
        public Int16 DayNumber { get; set; }
        public Int32 OperationalTimeID { get; set; }
        public String OperationalTimeCode { get; set; }
        public String OperationalTimeName { get; set; }
        public Int32 ParamedicID { get; set; }
        public String ParamedicName { get; set; }
        public Int32? RoomID { get; set; }
        public String RoomName { get; set; }
        public Int32 ServiceUnitID { get; set; }
        public String ServiceUnitName { get; set; }
        public String SpecialtyID { get; set; }
        public String SpecialtyName { get; set; }

        #region IsAppointment, MaxAppointment
        public Boolean IsAppointmentByTimeSlot1 { get; set; }
        public Boolean IsAppointmentByTimeSlot2 { get; set; }
        public Boolean IsAppointmentByTimeSlot3 { get; set; }
        public Boolean IsAppointmentByTimeSlot4 { get; set; }
        public Boolean IsAppointmentByTimeSlot5 { get; set; }

        public Int16 MaximumAppointment1 { get; set; }
        public Int16 MaximumAppointment2 { get; set; }
        public Int16 MaximumAppointment3 { get; set; }
        public Int16 MaximumAppointment4 { get; set; }
        public Int16 MaximumAppointment5 { get; set; }
        #endregion

        #region IsAllowWaitingList, MaxWaitingList
        public Boolean IsAllowWaitingList1 { get; set; }
        public Boolean IsAllowWaitingList2 { get; set; }
        public Boolean IsAllowWaitingList3 { get; set; }
        public Boolean IsAllowWaitingList4 { get; set; }
        public Boolean IsAllowWaitingList5 { get; set; }

        public Int16 MaximumWaitingList1 { get; set; }
        public Int16 MaximumWaitingList2 { get; set; }
        public Int16 MaximumWaitingList3 { get; set; }
        public Int16 MaximumWaitingList4 { get; set; }
        public Int16 MaximumWaitingList5 { get; set; }
        #endregion

        #region StartTime, EndTime
        public String StartTime1 { get; set; }
        public String StartTime2 { get; set; }
        public String StartTime3 { get; set; }
        public String StartTime4 { get; set; }
        public String StartTime5 { get; set; }

        public String EndTime1 { get; set; }
        public String EndTime2 { get; set; }
        public String EndTime3 { get; set; }
        public String EndTime4 { get; set; }
        public String EndTime5 { get; set; }
        #endregion
    }

    public class ParamedicScheduleWithDateDTO
    {
        public DateTime ScheduleDate { get; set; }
        public Int32 OperationalTimeID { get; set; }
        public String OperationalTimeCode { get; set; }
        public String OperationalTimeName { get; set; }
        public Int32 ParamedicID { get; set; }
        public String ParamedicName { get; set; }
        public Int32? RoomID { get; set; }
        public String RoomName { get; set; }
        public String SpecialtyID { get; set; }
        public String SpecialtyName { get; set; }
        public Int32 ServiceUnitID { get; set; }
        public String ServiceUnitName { get; set; }

        #region IsAppointment, MaxAppointment
        public Boolean IsAppointmentByTimeSlot1 { get; set; }
        public Boolean IsAppointmentByTimeSlot2 { get; set; }
        public Boolean IsAppointmentByTimeSlot3 { get; set; }
        public Boolean IsAppointmentByTimeSlot4 { get; set; }
        public Boolean IsAppointmentByTimeSlot5 { get; set; }

        public Int16 MaximumAppointment1 { get; set; }
        public Int16 MaximumAppointment2 { get; set; }
        public Int16 MaximumAppointment3 { get; set; }
        public Int16 MaximumAppointment4 { get; set; }
        public Int16 MaximumAppointment5 { get; set; }
        #endregion

        #region IsAllowWaitingList, MaxWaitingList
        public Boolean IsAllowWaitingList1 { get; set; }
        public Boolean IsAllowWaitingList2 { get; set; }
        public Boolean IsAllowWaitingList3 { get; set; }
        public Boolean IsAllowWaitingList4 { get; set; }
        public Boolean IsAllowWaitingList5 { get; set; }

        public Int16 MaximumWaitingList1 { get; set; }
        public Int16 MaximumWaitingList2 { get; set; }
        public Int16 MaximumWaitingList3 { get; set; }
        public Int16 MaximumWaitingList4 { get; set; }
        public Int16 MaximumWaitingList5 { get; set; }
        #endregion

        #region StartTime, EndTime
        public String StartTime1 { get; set; }
        public String StartTime2 { get; set; }
        public String StartTime3 { get; set; }
        public String StartTime4 { get; set; }
        public String StartTime5 { get; set; }

        public String EndTime1 { get; set; }
        public String EndTime2 { get; set; }
        public String EndTime3 { get; set; }
        public String EndTime4 { get; set; }
        public String EndTime5 { get; set; }
        #endregion
    }

    public class ParamedicLeaveScheduleDTO : Paramedic
    {
        public Int32 ID { get; set; }
        public String SpecialtyID { get; set; }
        public String SpecialtyName { get; set; }
        public DateTime StartDate { get; set; }
        public String StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public String EndTime { get; set; }
        public Boolean IsFullDay { get; set; }
        public String GCParamedicLeaveReason { get; set; }
        public String Remarks { get; set; }
    }

    public class ParamedicScheduleDeleteDTO
    {
        public Int32 ParamedicID { get; set; }
        public Int32 OperationalTimeID { get; set; }
        public Int16 DayNumber { get; set; }
    }
    #endregion

    #region Appointment
    public class AppointmentDTO
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 AppointmentID { get; set; }
        public String AppointmentNo { get; set; }
        public Int16 QueueNo { get; set; }
        public Boolean IsNewPatient { get; set; }
        public String DepartmentID { get; set; }
        public Int32 ServiceUnitID { get; set; }
        public String ServiceUnitCode { get; set; }
        public String ServiceUnitName { get; set; }
        public Int32 ParamedicID { get; set; }
        public String ParamedicCode { get; set; }
        public String ParamedicName { get; set; }
        public Int32 RoomID { get; set; }
        public String RoomCode { get; set; }
        public String RoomName { get; set; }
        public DateTime StartDate { get; set; }
        public String StartTime { get; set; }
        public DateTime EndDate { get; set; }
        public String EndTime { get; set; }
        public Int32 VisitTypeID { get; set; }
        public String VisitTypeCode { get; set; }
        public String VisitTypeName { get; set; }
        public Int16 VisitDuration { get; set; }
        public String GCAppointmentStatus { get; set; }
        public String AppointmentStatus { get; set; }
        public Boolean IsWaitingList { get; set; }
        public String MedicalNo { get; set; }
        public String PatientName { get; set; }
        public String Gender { get; set; }
        public String DOB { get; set; }
        public String StreetName { get; set; }
        public String PhoneNo { get; set; }
        public String MobilePhoneNo { get; set; }
        public String EmailAddress { get; set; }
        public String DeleteReason { get; set; }
    }

    public class Appointment2DTO
    {
        public string HealthcareID { get; set; }
        public int AppointmentID { get; set; }
        public int RegistrationID { get; set; }
        public int PatientID { get; set; }
    }

    public class Registration3DTO
    {
        public string HealthcareID { get; set; }
        public int RegistrationID { get; set; }
        public int PatientID { get; set; }
    }
    #endregion


    public class Metadata 
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class VisitInfo
    {
        public int VisitID { get; set; }

        public string VisitDate { get; set; }
        public string VisitTime { get; set; }

        #region Physician Information
        public int PhysicianID { get; set; }
        public string PhysicianCode { get; set; }
        public string PhysicianName { get; set; }
        public string SpecialtyName { get; set; }
        #endregion

        #region Service Unit
        public string DepartmentCode { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }

        public int? RoomID { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int BedID { get; set; }
        public string BedCode { get; set; }
        public string ExtensionNo { get; set; }
        #endregion

        #region Care and Charge Class
        public int ClassID { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public int ChargeClassID { get; set; }
        public string ChargeClassCode { get; set; }
        public string ChargeClassName { get; set; }
        #endregion

        #region Visit Type
        public int VisitTypeID { get; set; }
        public string VisitTypeCode { get; set; }
        public string VisitTypeName { get; set; }
        #endregion

        public string QueueNo { get; set; }

        public string DischargeDate { get; set; }
        public string DischargeTime { get; set; }
    }

    public class RegistrationDTO
    {
        public string HealthcareID { get; set; }
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public int AppointmentID { get; set; }

        public int PatientID { get; set; }
        public PatientData PatientInfo { get; set; }
        #region Visit Information
        public List<VisitInfo> VisitInformation { get; set; }
        #endregion

        #region Payer Information
        public string PayerType { get; set; }
        public string PayerCode { get; set; }
        public string PayerName { get; set; }
        #endregion
    }

    public class InventoryNotificationDTO
    {
        public string HealthcareID { get; set; }
        public string MessageType { get; set; }
        public string TransactionID { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionReferenceID { get; set; }
    }

    public class Registration2DTO
    {
        public string HealthcareID { get; set; }
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public int AppointmentID { get; set; }

        public int PatientID { get; set; }
        public PatientData PatientInfo { get; set; }
        #region Visit Information
        public List<VisitInfo> VisitInformation { get; set; }
        #endregion

        #region Payer Information
        public string PayerType { get; set; }
        public string PayerCode { get; set; }
        public string PayerName { get; set; }
        #endregion

        #region Transaction-specific information
        public string MessageType { get; set; }
        public int TransactionReferenceID { get; set; }
        #endregion
    }

    public class PrescriptionOrderDTO
    {
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public int PatientID { get; set; }
        public PatientData PatientInfo { get; set; }
        public int VisitID { get; set; }
        public VisitInfo VisitInformation { get; set; }
        public string PrescriptionOrderNo { get; set; }
        public string PrescriptionOrderDate { get; set; }
        public string PrescriptionOrderTime { get; set; }
        public bool IsCompound { get; set; }
    }

    public class OrderDTO
    {
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public int PatientID { get; set; }
        public PatientData PatientInfo { get; set; }
        public int VisitID { get; set; }
        public VisitInfo VisitInformation { get; set; }
        public string TransactionNo { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string OrderType { get; set; }
        public bool IsCompound { get; set; }
        public List<OrderDt> DetailList { get; set; }
    }

    public class OrderDt
    {
        public string itemCode { get; set; }
        public string itemName { get; set; }
    }

    public class PatientTranferInfo
    {
        public int fromRoomID { get; set; }
        public string fromRoomCode { get; set; }
        public string fromRoomName { get; set; }
        public int fromBedID { get; set; }
        public string fromBedCode { get; set; }
        public string fromExtensionNo { get; set; }
        public int toRoomID { get; set; }
        public string toRoomCode { get; set; }
        public string toRoomName { get; set; }
        public int toBedID { get; set; }
        public string toBedCode { get; set; }
        public string toExtensionNo { get; set; }
    }

    public class PatientTransferDTO
    {
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }

        public int PatientID { get; set; }
        #region Visit Information
        public VisitInfo VisitInformation { get; set; }
        #endregion

        public PatientTranferInfo TransferInfo { get; set; }
    }

    public class MFNParameter1DTO
    {
        public string HealthcareID { get; set; }
        public int ParamedicID { get; set; }
        public int HealthcareServiceUnitID { get; set; }
    }

    public class CenterbackBedTransferDTO
    {
        public string HealthcareID { get; set; }
        public string ProcessType { get; set; }
        public int RegistrationID { get; set; }
    }

    public enum CenterbackBedTranferType
    {
        checkin,
        move
    }

    public class PatientTransactionDto
    {
        public String TransactionNo { get; set; }
    }

}
