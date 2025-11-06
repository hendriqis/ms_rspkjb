using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class StoreServiceUnitChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string ServiceUnitID { get; set; }
        public string ServiceUnitName { get; set; }
        public string ServiceUnitName2 { get; set; }
        public string ShortName { get; set; }
    }

    public class StoreOperationalTimeChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string OperationalTimeID { get; set; }
        public string StartTime1 { get; set; }
        public string EndTime1 { get; set; }
        public string StartTime2 { get; set; }
        public string EndTime2 { get; set; }
        public string StartTime3 { get; set; }
        public string EndTime3 { get; set; }
        public string StartTime4 { get; set; }
        public string EndTime4 { get; set; }
        public string StartTime5 { get; set; }
        public string EndTime5 { get; set; }
    }

    public class StoreSpecialtyChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string SpecialtyID { get; set; }
        public string SpecialtyName { get; set; }
        public string SpecialtyName2 { get; set; }
    }

    public class StoreParamedicChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string SpecialtyID { get; set; }
        public string ParamedicID { get; set; }
        public string FullName { get; set; }
    }

    public class StoreVisitTypeChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string VisitTypeID { get; set; }
        public string VisitTypeName { get; set; }
    }

    public class StorePatientChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string MedicalNo { get; set; }
        public string MobilePhoneNo1 { get; set; }
        public string MRN { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string DateOfBirth { get; set; }
    }

    public class StorePatientFamilyChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public int FamilyID { get; set; }
        public int MRN { get; set; }
        public string MedicalNo { get; set; }
        public string MobilePhone1 { get; set; }
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public int FamilyMRN { get; set; }
        public string DateOfBirth { get; set; }
        public string GCFamilyRelation { get; set; }
        public int IsDeleted { get; set; }
    }

    public class StoreParamedicScheduleChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public Int32 DayNumber { get; set; }
        public Int32 OperationalTimeID { get; set; }
        public bool IsBPJS1 { get; set; }
        public bool IsBPJS2 { get; set; }
        public bool IsBPJS3 { get; set; }
        public bool IsBPJS4 { get; set; }
        public bool IsBPJS5 { get; set; }
        public bool IsNonBPJS1 { get; set; }
        public bool IsNonBPJS2 { get; set; }
        public bool IsNonBPJS3 { get; set; }
        public bool IsNonBPJS4 { get; set; }
        public bool IsNonBPJS5 { get; set; }
    }

    public class StoreParamedicScheduleDateChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string ScheduleDate { get; set; }
        public Int32 OperationalTimeID { get; set; }
        public bool IsBPJS1 { get; set; }
        public bool IsBPJS2 { get; set; }
        public bool IsBPJS3 { get; set; }
        public bool IsBPJS4 { get; set; }
        public bool IsBPJS5 { get; set; }
        public bool IsNonBPJS1 { get; set; }
        public bool IsNonBPJS2 { get; set; }
        public bool IsNonBPJS3 { get; set; }
        public bool IsNonBPJS4 { get; set; }
        public bool IsNonBPJS5 { get; set; }
    }

    public class StoreParamedicLeaveScheduleChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public Int32 ID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string GCParamedicLeaveReason { get; set; }
        public string LeaveOtherReason { get; set; }
        public string Remarks { get; set; }
    }

    public class MobileAppointmentRequestBody
    {
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }

    public class MobileAppointmentRequestResponse
    {
        public string Code { get; set; }
        public string Status { get; set; }
        public List<MobileAppointmentRequestResponseData> Data { get; set; }
    }

    public class MobileAppointmentRequestResponseData
    {
        public int KeyID { get; set; }
        public int AppointmentID { get; set; }
        public int AppointmentQueueNo { get; set; }
        public int AppointmentSession { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string RegistrationNo { get; set; }
        public int RegistrationQueueNo { get; set; }
        public int RegistrationSession { get; set; }
        public string AppointmentNo { get; set; }
        public int ParamedicID { get; set; }
        public int HealthcareServiceUnitID { get; set; }
        public int MRN { get; set; }
        public int VisitTypeID { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndDate { get; set; }
        public string EndTime { get; set; }
        public bool IsRejected { get; set; }
        public string RejectedReason { get; set; }
        public int QueueNo { get; set; }
        public string GCCustomerType { get; set; }
        public string CreatedDate { get; set; }
        public int LastUpdatedBy { get; set; }
        public string LastUpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int Session { get; set; }
        public string RequestDeleteReason { get; set; }
    }

    public class MobileUpdateAppointmentRequest
    {
        public string EventType { get; set; }
        public string KeyID { get; set; }
        public string MRN { get; set; }
        public string GuestID { get; set; }
        public string StartDate { get; set; }
        public string ParamedicID { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string AppointmentID { get; set; }
        public string AppointmentQueueNo { get; set; }
        public string AppointmentSession { get; set; }
        public string AppointmentNo { get; set; }
        public string RejectedReason { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationQueueNo { get; set; }
        public string RegistrationSession { get; set; }
        public string IsRejected { get; set; }
        public string QueueNo { get; set; }
        public string RoomName { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    public class StoreHealthcareServiceUnitChanged
    {
        public string EventType { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string ServiceUnitID { get; set; }
    }

    public class MobileAppointmentInfo
    {
        public string EventType { get; set; }
        public string MRN { get; set; }
        public string GuestID { get; set; }
        public string MedicalNo { get; set; }
        public string PatientName { get; set; }
        public string MobilePhoneNo1 { get; set; }
        public string StartDate { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string ParamedicID { get; set; }
        public string ParamedicName { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string HealthcareGroup { get; set; }
        public string HealthcareID { get; set; }
        public string ServiceUnitName { get; set; }
        public string AppointmentID { get; set; }
        public string AppointmentQueueNo { get; set; }
        public string AppointmentSession { get; set; }
        public string AppointmentNo { get; set; }
        public string GCAppointmentMethod { get; set; }
        public string RejectedReason { get; set; }
        public string RegistrationNo { get; set; }
        public string RegistrationQueueNo { get; set; }
        public string RegistrationSession { get; set; }
        public string IsRejected { get; set; }
        public string QueueNo { get; set; }
        public string LastUpdatedBy { get; set; }
    }

    public class MobileRegistrationInfo
    {
        public string RegistrationNo { get; set; }
        public int AppointmentID { get; set; }
        public string AppointmentNo { get; set; }
        public int IsNewPatient { get; set; }
        public int MRN { get; set; }
        public string MedicalNo { get; set; }
        public string OldMedicalNo { get; set; }
        public int GuestID { get; set; }
        public string PatientName { get; set; }
        public string MobilePhoneNo1 { get; set; }
        public string EmailAddress { get; set; }
        public int HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public int ParamedicID { get; set; }
        public string ParamedicCode { get; set; }
        public string ParamedicName { get; set; }
        public string SpecialtyID { get; set; }
        public string SpecialtyName { get; set; }
        public int Session { get; set; }
        public int QueueNo { get; set; }
        public string RegistrationTicketNo { get; set; }
        public int RoomID { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public int VisitTypeID { get; set; }
        public string VisitTypeCode { get; set; }
        public string VisitTypeName { get; set; }
        public string GCCustomerType { get; set; }
        public string CustomerType { get; set; }
        public string BusinessPartnerName { get; set; }
        public string GCRegistrationStatus { get; set; }
        public string RegistrationStatus { get; set; }
        public string GCVoidReason { get; set; }
        public string VoidReason { get; set; }
    }
}
