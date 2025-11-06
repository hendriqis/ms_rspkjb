using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common.API.Model
{
    public class Person
    {
        public string Salutation { get; set; }
        public string Title { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PrefferedName { get; set; }

        public string Suffix { get; set; }

        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string DateOfBirthFormat1 { get; set; }
        public string CityOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string StreetName { get; set; }
        public string HomeCity { get; set; }
        public string HomeZipCode { get; set; }
        public string HomePhoneNo1 { get; set; }
        public string HomePhoneNo2 { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string Nationality { get; set; }
        public string EmailAddress { get; set; }
        public string SSN { get; set; }

    }

    public class PatientInfo : Person
    {
        public int PatientID { get; set; }
        public int GuestID { get; set; }
        public string MedicalNo { get; set; }
        public string GuestNo { get; set; }
        public string SpouseName { get; set; }

        public string BloodType { get; set; }
        public string BloodRhesus { get; set; }

        public string Weight { get; set; }
        public string Height { get; set; }

        public string HealthcareID { get; set; }

        public int HomeAddressID { get; set; }
        public int OfficeAddressID { get; set; }
        public int OtherAddressID { get; set; }

        public string TagField1 { get; set; }
        public string TagField2 { get; set; }
        public string TagField3 { get; set; }
        public string TagField4 { get; set; }
        public string TagField5 { get; set; }
        public string TagField6 { get; set; }
        public string TagField7 { get; set; }
        public string TagField8 { get; set; }
        public string TagField9 { get; set; }
        public string TagField10 { get; set; }
        public string TagField11 { get; set; }
        public string TagField12 { get; set; }
        public string TagField13 { get; set; }
        public string TagField14 { get; set; }
        public string TagField15 { get; set; }
        public string TagField16 { get; set; }
        public string TagField17 { get; set; }
        public string TagField18 { get; set; }
        public string TagField19 { get; set; }
        public string TagField20 { get; set; }

        public int UserID { get; set; }
    }

    public class CompactVisitInfo
    {
        public int VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string RegistrationNo { get; set; }
        public string DepartmentID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string RoomCode { get; set; }
        public string BedCode { get; set; }
    }

    public class VisitInfo1
    {
        public int VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public string VisitTime { get; set; }
        public string RegistrationNo { get; set; }
        public string DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string RoomCode { get; set; }
        public string RoomName { get; set; }
        public string BedCode { get; set; }
        public string BedName { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public string ChargeClassCode { get; set; }
        public string ChargeClassName { get; set; }
        public string PhysicianCode { get; set; }
        public string PhysicianName { get; set; }
        public string PayerCode { get; set; }
        public string PayerName { get; set; }
    }

    public class TestOrderInfoDTO
    {
        #region Header Information
        public string HealthcareID { get; set; }
        public int TestOrderID { get; set; }
        public string TestOrderNo { get; set; }
        public string TestOrderDate { get; set; }
        public string TestOrderTime { get; set; }
        public string PhysicianCode { get; set; }
        public string PhysicianName { get; set; }
        #endregion

        #region Patient Information
        public int PatientID { get; set; }
        public PatientInfo PatientInfo { get; set; }
        #endregion

        #region Visit Information
        public int RegistrationID { get; set; }
        public string RegistrationNo { get; set; }
        public int VisitID { get; set; }
        public CompactVisitInfo VisitInformation { get; set; }
        #endregion

        #region Order Detail Information
        public List<TestOrderDetailInfo> OrderItemList { get; set; }
        #endregion
    }

    public class TestOrderDetailInfo
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string RequestedPhysicianName { get; set; }
        public bool IsCITO { get; set; }
        public string DiagnoseName { get; set; }
        public string ModalityType { get; set; }
        public string ModalityCode { get; set; }
        public string ModalityAETitle { get; set; }
        public string Remarks { get; set; }
    }

    public class ImagingOrderDTO : TestOrderInfoDTO
    {
    }

    public class MedinfrasAPIResponse2
    {
        public string Status { get; set; }
        public object Remarks { get; set; }
        public string Data { get; set; }
    }
    public class ReportDocumentParam
    {
        public string ReportCode { get; set; }
        public int VisitID { get; set; }
        public string ParamValue { get; set; }
    }
}
