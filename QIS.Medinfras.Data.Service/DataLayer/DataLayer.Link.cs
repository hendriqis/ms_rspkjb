using System;
using QIS.Data.Core.Dal;
using System.Text;
using System.ComponentModel;
using QIS.Medinfras.Common;
using System.IO;
using System.Data;

namespace QIS.Medinfras.Data.Service
{
    #region Medinfrasv1.1 View
    #region vOrderResep
    [Serializable]
    [Table(Name = "vOrderResep")]
    public class vOrderResep
    {
        private String _NoJO;
        private String _NoReg;
        private String _NoRM;
        private String _NamaPasien;
        private String _App;
        private String _KdDokter;
        private String _NmDokter;
        private String _KdUnit;
        private String _NmUnit;
        private String _NoTransaksi;
        private String _KdLayan;
        private String _Resep;
        private DateTime _TglUpdate;
        private Boolean _IsOrderTextSentToPrinter;
        private Int32 _PrintCount;
        private String _UsrUpdate;

        [Column(Name = "NoJO", DataType = "String")]
        public String NoJO
        {
            get { return _NoJO; }
            set { _NoJO = value; }
        }
        [Column(Name = "NoReg", DataType = "String")]
        public String NoReg
        {
            get { return _NoReg; }
            set { _NoReg = value; }
        }
        [Column(Name = "NoRM", DataType = "String")]
        public String NoRM
        {
            get { return _NoRM; }
            set { _NoRM = value; }
        }
        [Column(Name = "NamaPasien", DataType = "String")]
        public String NamaPasien
        {
            get { return _NamaPasien; }
            set { _NamaPasien = value; }
        }
        [Column(Name = "App", DataType = "String")]
        public String App
        {
            get { return _App; }
            set { _App = value; }
        }
        [Column(Name = "KdDokter", DataType = "String")]
        public String KdDokter
        {
            get { return _KdDokter; }
            set { _KdDokter = value; }
        }
        [Column(Name = "NmDokter", DataType = "String")]
        public String NmDokter
        {
            get { return _NmDokter; }
            set { _NmDokter = value; }
        }
        [Column(Name = "KdUnit", DataType = "String")]
        public String KdUnit
        {
            get { return _KdUnit; }
            set { _KdUnit = value; }
        }
        [Column(Name = "NmUnit", DataType = "String")]
        public String NmUnit
        {
            get { return _NmUnit; }
            set { _NmUnit = value; }
        }
        [Column(Name = "NoTransaksi", DataType = "String")]
        public String NoTransaksi
        {
            get { return _NoTransaksi; }
            set { _NoTransaksi = value; }
        }
        [Column(Name = "KdLayan", DataType = "String")]
        public String KdLayan
        {
            get { return _KdLayan; }
            set { _KdLayan = value; }
        }
        [Column(Name = "Resep", DataType = "String")]
        public String Resep
        {
            get { return _Resep; }
            set { _Resep = value; }
        }
        [Column(Name = "TglUpdate", DataType = "DateTime")]
        public DateTime TglUpdate
        {
            get { return _TglUpdate; }
            set { _TglUpdate = value; }
        }
        [Column(Name = "IsOrderTextSentToPrinter", DataType = "Boolean")]
        public Boolean IsOrderTextSentToPrinter
        {
            get { return _IsOrderTextSentToPrinter; }
            set { _IsOrderTextSentToPrinter = value; }
        }
        [Column(Name = "PrintCount", DataType = "Int32")]
        public Int32 PrintCount
        {
            get { return _PrintCount; }
            set { _PrintCount = value; }
        }
        [Column(Name = "UsrUpdate", DataType = "String")]
        public String UsrUpdate
        {
            get { return _UsrUpdate; }
            set { _UsrUpdate = value; }
        }
    }
    #endregion

    #region vEmergencyPatientListLink
    [Serializable]
    [Table(Name = "vEmergencyPatientListLink")]
    public partial class vEmergencyPatientListLink
    {
        private Int32 _VisitID;
        private Int32 _RegistrationID;
        private String _RegistrationNo;
        private Int32 _HealthcareServiceUnitID;
        private String _DepartmentID;
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _LocationID;
        private Int32 _LogisticLocationID;
        private Int32 _RoomID;
        private String _RoomCode;
        private String _RoomName;
        private Int32 _ClassID;
        private String _ClassCode;
        private String _ClassName;
        private Int32 _ChargeClassID;
        private String _ChargeClassCode;
        private String _ChargeClassName;
        private Int32 _BedID;
        private String _BedCode;
        private Int32 _ParamedicID;
        private String _ParamedicCode;
        private String _ParamedicName;
        private String _SpecialtyID;
        private String _SpecialtyName;
        private Int32 _VisitTypeID;
        private String _VisitTypeCode;
        private String _VisitTypeName;
        private DateTime _VisitDate;
        private String _VisitTime;
        private String _ActualVisitDate;
        private String _ActualVisitTime;
        private String _GCVisitReason;
        private Int32 _VisitReason;
        private Int32 _QueueNo;
        private DateTime _DischargeDate;
        private String _DischargeTime;
        private String _GCVisitStatus;
        private String _GCRegistrationStatus;
        private Boolean _IsMainVisit;
        private Int32 _GCAdmissionCondition;
        private String _AdmissionCondition;
        private String _IsNewBorn;
        private String _IsParturition;
        private Int32 _ReferrerName;
        private Int32 _PlanDischargeDate;
        private String _GCDischargeCondition;
        private String _DischargeCondition;
        private String _GCDischargeMethod;
        private String _DischargeMethod;
        private String _LOSInDay;
        private String _LOSInHour;
        private String _LOSInMinute;
        private String _GCCustomerType;
        private String _CustomerType;
        private Int32 _BusinessPartnerID;
        private String _BusinessPartnerCode;
        private String _BusinessPartnerName;
        private Int32 _EmployeeID;
        private Int32 _EmployeeCode;
        private Int32 _EmployeeName;
        private Int32 _MRN;
        private Int32 _GuestID;
        private Int32 _GCPatientCategory;
        private String _PatientCategory;
        private Int32 _GCTriage;
        private String _TriageColor;
        private String _MedicalNo;
        private Int32 _OldMedicalNo;
        private Int32 _GCSalutation;
        private Int32 _Salutation;
        private String _PatientName;
        private Int32 _PreferredName;
        private String _CityOfBirth;
        private DateTime _DateOfBirth;
        private String _GCSex;
        private String _Sex;
        private String _GCReligion;
        private String _Religion;
        private String _PictureFileName;
        private String _GCGender;
        private String _Gender;
        private String _PhoneNo1;
        private String _PhoneNo2;
        private String _MobilePhoneNo1;
        private String _MobilePhoneNo2;
        private String _StreetName;
        private String _County;
        private String _District;
        private String _City;
        private Int32 _GCState;
        private String _State;
        private Int32 _ZipCodeID;
        private String _ZipCode;
        private Int32 _GCMedicalFileStatus;
        private String _PatientAllergy;
        private Boolean _IsChargesTransfered;
        private Int32 _LinkedRegistrationID;
        private Boolean _IsHasAllergy;
        private Boolean _IsFallRisk;
        private Boolean _IsDNR;

        [Column(Name = "VisitID", DataType = "Int32")]
        public Int32 VisitID
        {
            get { return _VisitID; }
            set { _VisitID = value; }
        }
        [Column(Name = "RegistrationID", DataType = "Int32")]
        public Int32 RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String")]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "HealthcareServiceUnitID", DataType = "Int32")]
        public Int32 HealthcareServiceUnitID
        {
            get { return _HealthcareServiceUnitID; }
            set { _HealthcareServiceUnitID = value; }
        }
        [Column(Name = "DepartmentID", DataType = "String")]
        public String DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }
        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "LocationID", DataType = "Int32")]
        public Int32 LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        [Column(Name = "LogisticLocationID", DataType = "Int32")]
        public Int32 LogisticLocationID
        {
            get { return _LogisticLocationID; }
            set { _LogisticLocationID = value; }
        }
        [Column(Name = "RoomID", DataType = "Int32")]
        public Int32 RoomID
        {
            get { return _RoomID; }
            set { _RoomID = value; }
        }
        [Column(Name = "RoomCode", DataType = "String")]
        public String RoomCode
        {
            get { return _RoomCode; }
            set { _RoomCode = value; }
        }
        [Column(Name = "RoomName", DataType = "String")]
        public String RoomName
        {
            get { return _RoomName; }
            set { _RoomName = value; }
        }
        [Column(Name = "ClassID", DataType = "Int32")]
        public Int32 ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }
        [Column(Name = "ClassCode", DataType = "String")]
        public String ClassCode
        {
            get { return _ClassCode; }
            set { _ClassCode = value; }
        }
        [Column(Name = "ClassName", DataType = "String")]
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        [Column(Name = "ChargeClassID", DataType = "Int32")]
        public Int32 ChargeClassID
        {
            get { return _ChargeClassID; }
            set { _ChargeClassID = value; }
        }
        [Column(Name = "ChargeClassCode", DataType = "String")]
        public String ChargeClassCode
        {
            get { return _ChargeClassCode; }
            set { _ChargeClassCode = value; }
        }
        [Column(Name = "ChargeClassName", DataType = "String")]
        public String ChargeClassName
        {
            get { return _ChargeClassName; }
            set { _ChargeClassName = value; }
        }
        [Column(Name = "BedID", DataType = "Int32")]
        public Int32 BedID
        {
            get { return _BedID; }
            set { _BedID = value; }
        }
        [Column(Name = "BedCode", DataType = "String")]
        public String BedCode
        {
            get { return _BedCode; }
            set { _BedCode = value; }
        }
        [Column(Name = "ParamedicID", DataType = "Int32")]
        public Int32 ParamedicID
        {
            get { return _ParamedicID; }
            set { _ParamedicID = value; }
        }
        [Column(Name = "ParamedicCode", DataType = "String")]
        public String ParamedicCode
        {
            get { return _ParamedicCode; }
            set { _ParamedicCode = value; }
        }
        [Column(Name = "ParamedicName", DataType = "String")]
        public String ParamedicName
        {
            get { return _ParamedicName; }
            set { _ParamedicName = value; }
        }
        [Column(Name = "SpecialtyID", DataType = "String")]
        public String SpecialtyID
        {
            get { return _SpecialtyID; }
            set { _SpecialtyID = value; }
        }
        [Column(Name = "SpecialtyName", DataType = "String")]
        public String SpecialtyName
        {
            get { return _SpecialtyName; }
            set { _SpecialtyName = value; }
        }
        [Column(Name = "VisitTypeID", DataType = "Int32")]
        public Int32 VisitTypeID
        {
            get { return _VisitTypeID; }
            set { _VisitTypeID = value; }
        }
        [Column(Name = "VisitTypeCode", DataType = "String")]
        public String VisitTypeCode
        {
            get { return _VisitTypeCode; }
            set { _VisitTypeCode = value; }
        }
        [Column(Name = "VisitTypeName", DataType = "String")]
        public String VisitTypeName
        {
            get { return _VisitTypeName; }
            set { _VisitTypeName = value; }
        }
        [Column(Name = "VisitDate", DataType = "DateTime")]
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set { _VisitDate = value; }
        }
        [Column(Name = "VisitTime", DataType = "String")]
        public String VisitTime
        {
            get { return _VisitTime; }
            set { _VisitTime = value; }
        }
        [Column(Name = "ActualVisitDate", DataType = "String")]
        public String ActualVisitDate
        {
            get { return _ActualVisitDate; }
            set { _ActualVisitDate = value; }
        }
        [Column(Name = "ActualVisitTime", DataType = "String")]
        public String ActualVisitTime
        {
            get { return _ActualVisitTime; }
            set { _ActualVisitTime = value; }
        }
        [Column(Name = "GCVisitReason", DataType = "String")]
        public String GCVisitReason
        {
            get { return _GCVisitReason; }
            set { _GCVisitReason = value; }
        }
        [Column(Name = "VisitReason", DataType = "Int32")]
        public Int32 VisitReason
        {
            get { return _VisitReason; }
            set { _VisitReason = value; }
        }
        [Column(Name = "QueueNo", DataType = "Int32")]
        public Int32 QueueNo
        {
            get { return _QueueNo; }
            set { _QueueNo = value; }
        }
        [Column(Name = "DischargeDate", DataType = "DateTime")]
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set { _DischargeDate = value; }
        }
        [Column(Name = "DischargeTime", DataType = "String")]
        public String DischargeTime
        {
            get { return _DischargeTime; }
            set { _DischargeTime = value; }
        }
        [Column(Name = "GCVisitStatus", DataType = "String")]
        public String GCVisitStatus
        {
            get { return _GCVisitStatus; }
            set { _GCVisitStatus = value; }
        }
        [Column(Name = "GCRegistrationStatus", DataType = "String")]
        public String GCRegistrationStatus
        {
            get { return _GCRegistrationStatus; }
            set { _GCRegistrationStatus = value; }
        }
        [Column(Name = "IsMainVisit", DataType = "Boolean")]
        public Boolean IsMainVisit
        {
            get { return _IsMainVisit; }
            set { _IsMainVisit = value; }
        }
        [Column(Name = "GCAdmissionCondition", DataType = "Int32")]
        public Int32 GCAdmissionCondition
        {
            get { return _GCAdmissionCondition; }
            set { _GCAdmissionCondition = value; }
        }
        [Column(Name = "AdmissionCondition", DataType = "String")]
        public String AdmissionCondition
        {
            get { return _AdmissionCondition; }
            set { _AdmissionCondition = value; }
        }
        [Column(Name = "IsNewBorn", DataType = "String")]
        public String IsNewBorn
        {
            get { return _IsNewBorn; }
            set { _IsNewBorn = value; }
        }
        [Column(Name = "IsParturition", DataType = "String")]
        public String IsParturition
        {
            get { return _IsParturition; }
            set { _IsParturition = value; }
        }
        [Column(Name = "ReferrerName", DataType = "Int32")]
        public Int32 ReferrerName
        {
            get { return _ReferrerName; }
            set { _ReferrerName = value; }
        }
        [Column(Name = "PlanDischargeDate", DataType = "Int32")]
        public Int32 PlanDischargeDate
        {
            get { return _PlanDischargeDate; }
            set { _PlanDischargeDate = value; }
        }
        [Column(Name = "GCDischargeCondition", DataType = "String")]
        public String GCDischargeCondition
        {
            get { return _GCDischargeCondition; }
            set { _GCDischargeCondition = value; }
        }
        [Column(Name = "DischargeCondition", DataType = "String")]
        public String DischargeCondition
        {
            get { return _DischargeCondition; }
            set { _DischargeCondition = value; }
        }
        [Column(Name = "GCDischargeMethod", DataType = "String")]
        public String GCDischargeMethod
        {
            get { return _GCDischargeMethod; }
            set { _GCDischargeMethod = value; }
        }
        [Column(Name = "DischargeMethod", DataType = "String")]
        public String DischargeMethod
        {
            get { return _DischargeMethod; }
            set { _DischargeMethod = value; }
        }
        [Column(Name = "LOSInDay", DataType = "String")]
        public String LOSInDay
        {
            get { return _LOSInDay; }
            set { _LOSInDay = value; }
        }
        [Column(Name = "LOSInHour", DataType = "String")]
        public String LOSInHour
        {
            get { return _LOSInHour; }
            set { _LOSInHour = value; }
        }
        [Column(Name = "LOSInMinute", DataType = "String")]
        public String LOSInMinute
        {
            get { return _LOSInMinute; }
            set { _LOSInMinute = value; }
        }
        [Column(Name = "GCCustomerType", DataType = "String")]
        public String GCCustomerType
        {
            get { return _GCCustomerType; }
            set { _GCCustomerType = value; }
        }
        [Column(Name = "CustomerType", DataType = "String")]
        public String CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        [Column(Name = "BusinessPartnerID", DataType = "Int32")]
        public Int32 BusinessPartnerID
        {
            get { return _BusinessPartnerID; }
            set { _BusinessPartnerID = value; }
        }
        [Column(Name = "BusinessPartnerCode", DataType = "String")]
        public String BusinessPartnerCode
        {
            get { return _BusinessPartnerCode; }
            set { _BusinessPartnerCode = value; }
        }
        [Column(Name = "BusinessPartnerName", DataType = "String")]
        public String BusinessPartnerName
        {
            get { return _BusinessPartnerName; }
            set { _BusinessPartnerName = value; }
        }
        [Column(Name = "EmployeeID", DataType = "Int32")]
        public Int32 EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }
        [Column(Name = "EmployeeCode", DataType = "Int32")]
        public Int32 EmployeeCode
        {
            get { return _EmployeeCode; }
            set { _EmployeeCode = value; }
        }
        [Column(Name = "EmployeeName", DataType = "Int32")]
        public Int32 EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; }
        }
        [Column(Name = "MRN", DataType = "Int32")]
        public Int32 MRN
        {
            get { return _MRN; }
            set { _MRN = value; }
        }
        [Column(Name = "GuestID", DataType = "Int32")]
        public Int32 GuestID
        {
            get { return _GuestID; }
            set { _GuestID = value; }
        }
        [Column(Name = "GCPatientCategory", DataType = "Int32")]
        public Int32 GCPatientCategory
        {
            get { return _GCPatientCategory; }
            set { _GCPatientCategory = value; }
        }
        [Column(Name = "PatientCategory", DataType = "String")]
        public String PatientCategory
        {
            get { return _PatientCategory; }
            set { _PatientCategory = value; }
        }
        [Column(Name = "GCTriage", DataType = "Int32")]
        public Int32 GCTriage
        {
            get { return _GCTriage; }
            set { _GCTriage = value; }
        }
        [Column(Name = "TriageColor", DataType = "String")]
        public String TriageColor
        {
            get { return _TriageColor; }
            set { _TriageColor = value; }
        }
        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "OldMedicalNo", DataType = "Int32")]
        public Int32 OldMedicalNo
        {
            get { return _OldMedicalNo; }
            set { _OldMedicalNo = value; }
        }
        [Column(Name = "GCSalutation", DataType = "Int32")]
        public Int32 GCSalutation
        {
            get { return _GCSalutation; }
            set { _GCSalutation = value; }
        }
        [Column(Name = "Salutation", DataType = "Int32")]
        public Int32 Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "PreferredName", DataType = "Int32")]
        public Int32 PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
        [Column(Name = "CityOfBirth", DataType = "String")]
        public String CityOfBirth
        {
            get { return _CityOfBirth; }
            set { _CityOfBirth = value; }
        }
        [Column(Name = "DateOfBirth", DataType = "DateTime")]
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        [Column(Name = "GCSex", DataType = "String")]
        public String GCSex
        {
            get { return _GCSex; }
            set { _GCSex = value; }
        }
        [Column(Name = "Sex", DataType = "String")]
        public String Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [Column(Name = "GCReligion", DataType = "String")]
        public String GCReligion
        {
            get { return _GCReligion; }
            set { _GCReligion = value; }
        }
        [Column(Name = "Religion", DataType = "String")]
        public String Religion
        {
            get { return _Religion; }
            set { _Religion = value; }
        }
        [Column(Name = "PictureFileName", DataType = "String")]
        public String PictureFileName
        {
            get { return _PictureFileName; }
            set { _PictureFileName = value; }
        }
        [Column(Name = "GCGender", DataType = "String")]
        public String GCGender
        {
            get { return _GCGender; }
            set { _GCGender = value; }
        }
        [Column(Name = "Gender", DataType = "String")]
        public String Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [Column(Name = "PhoneNo1", DataType = "String")]
        public String PhoneNo1
        {
            get { return _PhoneNo1; }
            set { _PhoneNo1 = value; }
        }
        [Column(Name = "PhoneNo2", DataType = "String")]
        public String PhoneNo2
        {
            get { return _PhoneNo2; }
            set { _PhoneNo2 = value; }
        }
        [Column(Name = "MobilePhoneNo1", DataType = "String")]
        public String MobilePhoneNo1
        {
            get { return _MobilePhoneNo1; }
            set { _MobilePhoneNo1 = value; }
        }
        [Column(Name = "MobilePhoneNo2", DataType = "String")]
        public String MobilePhoneNo2
        {
            get { return _MobilePhoneNo2; }
            set { _MobilePhoneNo2 = value; }
        }
        [Column(Name = "StreetName", DataType = "String")]
        public String StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
        [Column(Name = "County", DataType = "String")]
        public String County
        {
            get { return _County; }
            set { _County = value; }
        }
        [Column(Name = "District", DataType = "String")]
        public String District
        {
            get { return _District; }
            set { _District = value; }
        }
        [Column(Name = "City", DataType = "String")]
        public String City
        {
            get { return _City; }
            set { _City = value; }
        }
        [Column(Name = "GCState", DataType = "Int32")]
        public Int32 GCState
        {
            get { return _GCState; }
            set { _GCState = value; }
        }
        [Column(Name = "State", DataType = "String")]
        public String State
        {
            get { return _State; }
            set { _State = value; }
        }
        [Column(Name = "ZipCodeID", DataType = "Int32")]
        public Int32 ZipCodeID
        {
            get { return _ZipCodeID; }
            set { _ZipCodeID = value; }
        }
        [Column(Name = "ZipCode", DataType = "String")]
        public String ZipCode
        {
            get { return _ZipCode; }
            set { _ZipCode = value; }
        }
        [Column(Name = "GCMedicalFileStatus", DataType = "Int32")]
        public Int32 GCMedicalFileStatus
        {
            get { return _GCMedicalFileStatus; }
            set { _GCMedicalFileStatus = value; }
        }
        [Column(Name = "PatientAllergy", DataType = "String")]
        public String PatientAllergy
        {
            get { return _PatientAllergy; }
            set { _PatientAllergy = value; }
        }
        [Column(Name = "IsChargesTransfered", DataType = "Boolean")]
        public Boolean IsChargesTransfered
        {
            get { return _IsChargesTransfered; }
            set { _IsChargesTransfered = value; }
        }
        [Column(Name = "LinkedRegistrationID", DataType = "Int32")]
        public Int32 LinkedRegistrationID
        {
            get { return _LinkedRegistrationID; }
            set { _LinkedRegistrationID = value; }
        }
        [Column(Name = "IsHasAllergy", DataType = "Boolean")]
        public Boolean IsHasAllergy
        {
            get { return _IsHasAllergy; }
            set { _IsHasAllergy = value; }
        }
        [Column(Name = "IsFallRisk", DataType = "Boolean")]
        public Boolean IsFallRisk
        {
            get { return _IsFallRisk; }
            set { _IsFallRisk = value; }
        }
        [Column(Name = "IsDNR", DataType = "Boolean")]
        public Boolean IsDNR
        {
            get { return _IsDNR; }
            set { _IsDNR = value; }
        }
    }
    #endregion

    #region vInpatientAllPatientLink
    [Serializable]
    [Table(Name = "vInpatientAllPatientLink")]
    public partial class vInpatientAllPatientLink
    {
        private String _MedicalNo;
        private String _PatientName;
        private Int32 _PreferredName;
        private String _CityOfBirth;
        private DateTime _DateOfBirth;
        private String _Sex;
        private String _Religion;
        private String _Gender;
        private String _PhoneNo1;
        private String _PhoneNo2;
        private String _MobilePhoneNo1;
        private String _MobilePhoneNo2;
        private String _StreetName;
        private String _County;
        private String _District;
        private String _City;

        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "PreferredName", DataType = "Int32")]
        public Int32 PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
        [Column(Name = "CityOfBirth", DataType = "String")]
        public String CityOfBirth
        {
            get { return _CityOfBirth; }
            set { _CityOfBirth = value; }
        }
        [Column(Name = "DateOfBirth", DataType = "DateTime")]
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        [Column(Name = "Sex", DataType = "String")]
        public String Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [Column(Name = "Religion", DataType = "String")]
        public String Religion
        {
            get { return _Religion; }
            set { _Religion = value; }
        }
        [Column(Name = "Gender", DataType = "String")]
        public String Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [Column(Name = "PhoneNo1", DataType = "String")]
        public String PhoneNo1
        {
            get { return _PhoneNo1; }
            set { _PhoneNo1 = value; }
        }
        [Column(Name = "PhoneNo2", DataType = "String")]
        public String PhoneNo2
        {
            get { return _PhoneNo2; }
            set { _PhoneNo2 = value; }
        }
        [Column(Name = "MobilePhoneNo1", DataType = "String")]
        public String MobilePhoneNo1
        {
            get { return _MobilePhoneNo1; }
            set { _MobilePhoneNo1 = value; }
        }
        [Column(Name = "MobilePhoneNo2", DataType = "String")]
        public String MobilePhoneNo2
        {
            get { return _MobilePhoneNo2; }
            set { _MobilePhoneNo2 = value; }
        }
        [Column(Name = "StreetName", DataType = "String")]
        public String StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
        [Column(Name = "County", DataType = "String")]
        public String County
        {
            get { return _County; }
            set { _County = value; }
        }
        [Column(Name = "District", DataType = "String")]
        public String District
        {
            get { return _District; }
            set { _District = value; }
        }
        [Column(Name = "City", DataType = "String")]
        public String City
        {
            get { return _City; }
            set { _City = value; }
        }
    }
    #endregion

    #region vInpatientClassLink
    [Serializable]
    [Table(Name = "vInpatientClassLink")]
    public class vInpatientClassLink
    {
        private String _ClassCode;
        private String _ClassName;
        private Boolean _IsDeleted;

        [Column(Name = "ClassCode", DataType = "String")]
        public String ClassCode
        {
            get { return _ClassCode; }
            set { _ClassCode = value; }
        }
        [Column(Name = "ClassName", DataType = "String")]
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Boolean")]
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
    }
    #endregion

    #region vInpatientPatientListLink
    [Serializable]
    [Table(Name = "vInpatientPatientListLink")]
    public partial class vInpatientPatientListLink
    {
        private Int32 _VisitID;
        private Int32 _RegistrationID;
        private String _RegistrationNo;
        private Int32 _HealthcareServiceUnitID;
        private String _DepartmentID;
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _LocationID;
        private Int32 _LogisticLocationID;
        private Int32 _RoomID;
        private String _RoomCode;
        private String _RoomName;
        private Int32 _ClassID;
        private String _ClassCode;
        private String _ClassName;
        private Int32 _ChargeClassID;
        private String _ChargeClassCode;
        private String _ChargeClassName;
        private Int32 _BedID;
        private String _BedCode;
        private Int32 _ParamedicID;
        private String _ParamedicCode;
        private String _ParamedicName;
        private String _SpecialtyID;
        private String _SpecialtyName;
        private Int32 _VisitTypeID;
        private String _VisitTypeCode;
        private String _VisitTypeName;
        private DateTime _VisitDate;
        private String _VisitTime;
        private DateTime _ActualVisitDate;
        private String _ActualVisitTime;
        private String _GCVisitReason;
        private String _VisitReason;
        private Int32 _QueueNo;
        private DateTime _DischargeDate;
        private String _DischargeTime;
        private String _GCVisitStatus;
        private String _GCRegistrationStatus;
        private Boolean _IsMainVisit;
        private String _GCAdmissionCondition;
        private String _AdmissionCondition;
        private Boolean _IsNewBorn;
        private Boolean _IsParturition;
        private String _ReferrerName;
        private DateTime _PlanDischargeDate;
        private String _GCDischargeCondition;
        private String _DischargeCondition;
        private String _GCDischargeMethod;
        private String _DischargeMethod;
        private Decimal _LOSInDay;
        private Decimal _LOSInHour;
        private Decimal _LOSInMinute;
        private String _GCCustomerType;
        private String _CustomerType;
        private Int32 _BusinessPartnerID;
        private String _BusinessPartnerCode;
        private String _BusinessPartnerName;
        private Int32 _EmployeeID;
        private String _EmployeeCode;
        private String _EmployeeName;
        private Int32 _MRN;
        private Int32 _GuestID;
        private String _GCPatientCategory;
        private String _PatientCategory;
        private String _GCTriage;
        private String _TriageColor;
        private String _MedicalNo;
        private String _OldMedicalNo;
        private String _GCSalutation;
        private String _Salutation;
        private String _PatientName;
        private String _PreferredName;
        private String _CityOfBirth;
        private DateTime _DateOfBirth;
        private String _GCSex;
        private String _Sex;
        private String _GCReligion;
        private String _Religion;
        private String _PictureFileName;
        private String _GCGender;
        private String _Gender;
        private String _PhoneNo1;
        private String _PhoneNo2;
        private String _MobilePhoneNo1;
        private String _MobilePhoneNo2;
        private String _StreetName;
        private String _County;
        private String _District;
        private String _City;
        private String _GCState;
        private String _State;
        private Int32 _ZipCodeID;
        private String _ZipCode;
        private String _GCMedicalFileStatus;
        private String _PatientAllergy;
        private Boolean _IsChargesTransfered;
        private Int32 _LinkedRegistrationID;
        private Boolean _IsHasAllergy;
        private Boolean _IsFallRisk;
        private Boolean _IsDNR;

        [Column(Name = "VisitID", DataType = "Int32")]
        public Int32 VisitID
        {
            get { return _VisitID; }
            set { _VisitID = value; }
        }
        [Column(Name = "RegistrationID", DataType = "Int32")]
        public Int32 RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String")]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "HealthcareServiceUnitID", DataType = "Int32")]
        public Int32 HealthcareServiceUnitID
        {
            get { return _HealthcareServiceUnitID; }
            set { _HealthcareServiceUnitID = value; }
        }
        [Column(Name = "DepartmentID", DataType = "String")]
        public String DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }
        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "LocationID", DataType = "Int32")]
        public Int32 LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        [Column(Name = "LogisticLocationID", DataType = "Int32")]
        public Int32 LogisticLocationID
        {
            get { return _LogisticLocationID; }
            set { _LogisticLocationID = value; }
        }
        [Column(Name = "RoomID", DataType = "Int32")]
        public Int32 RoomID
        {
            get { return _RoomID; }
            set { _RoomID = value; }
        }
        [Column(Name = "RoomCode", DataType = "String")]
        public String RoomCode
        {
            get { return _RoomCode; }
            set { _RoomCode = value; }
        }
        [Column(Name = "RoomName", DataType = "String")]
        public String RoomName
        {
            get { return _RoomName; }
            set { _RoomName = value; }
        }
        [Column(Name = "ClassID", DataType = "Int32")]
        public Int32 ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }
        [Column(Name = "ClassCode", DataType = "String")]
        public String ClassCode
        {
            get { return _ClassCode; }
            set { _ClassCode = value; }
        }
        [Column(Name = "ClassName", DataType = "String")]
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        [Column(Name = "ChargeClassID", DataType = "Int32")]
        public Int32 ChargeClassID
        {
            get { return _ChargeClassID; }
            set { _ChargeClassID = value; }
        }
        [Column(Name = "ChargeClassCode", DataType = "String")]
        public String ChargeClassCode
        {
            get { return _ChargeClassCode; }
            set { _ChargeClassCode = value; }
        }
        [Column(Name = "ChargeClassName", DataType = "String")]
        public String ChargeClassName
        {
            get { return _ChargeClassName; }
            set { _ChargeClassName = value; }
        }
        [Column(Name = "BedID", DataType = "Int32")]
        public Int32 BedID
        {
            get { return _BedID; }
            set { _BedID = value; }
        }
        [Column(Name = "BedCode", DataType = "String")]
        public String BedCode
        {
            get { return _BedCode; }
            set { _BedCode = value; }
        }
        [Column(Name = "ParamedicID", DataType = "Int32")]
        public Int32 ParamedicID
        {
            get { return _ParamedicID; }
            set { _ParamedicID = value; }
        }
        [Column(Name = "ParamedicCode", DataType = "String")]
        public String ParamedicCode
        {
            get { return _ParamedicCode; }
            set { _ParamedicCode = value; }
        }
        [Column(Name = "ParamedicName", DataType = "String")]
        public String ParamedicName
        {
            get { return _ParamedicName; }
            set { _ParamedicName = value; }
        }
        [Column(Name = "SpecialtyID", DataType = "String")]
        public String SpecialtyID
        {
            get { return _SpecialtyID; }
            set { _SpecialtyID = value; }
        }
        [Column(Name = "SpecialtyName", DataType = "String")]
        public String SpecialtyName
        {
            get { return _SpecialtyName; }
            set { _SpecialtyName = value; }
        }
        [Column(Name = "VisitTypeID", DataType = "Int32")]
        public Int32 VisitTypeID
        {
            get { return _VisitTypeID; }
            set { _VisitTypeID = value; }
        }
        [Column(Name = "VisitTypeCode", DataType = "String")]
        public String VisitTypeCode
        {
            get { return _VisitTypeCode; }
            set { _VisitTypeCode = value; }
        }
        [Column(Name = "VisitTypeName", DataType = "String")]
        public String VisitTypeName
        {
            get { return _VisitTypeName; }
            set { _VisitTypeName = value; }
        }
        [Column(Name = "VisitDate", DataType = "DateTime")]
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set { _VisitDate = value; }
        }
        [Column(Name = "VisitTime", DataType = "String")]
        public String VisitTime
        {
            get { return _VisitTime; }
            set { _VisitTime = value; }
        }
        [Column(Name = "ActualVisitDate", DataType = "DateTime")]
        public DateTime ActualVisitDate
        {
            get { return _ActualVisitDate; }
            set { _ActualVisitDate = value; }
        }
        [Column(Name = "ActualVisitTime", DataType = "String")]
        public String ActualVisitTime
        {
            get { return _ActualVisitTime; }
            set { _ActualVisitTime = value; }
        }
        [Column(Name = "GCVisitReason", DataType = "String")]
        public String GCVisitReason
        {
            get { return _GCVisitReason; }
            set { _GCVisitReason = value; }
        }
        [Column(Name = "VisitReason", DataType = "String")]
        public String VisitReason
        {
            get { return _VisitReason; }
            set { _VisitReason = value; }
        }
        [Column(Name = "QueueNo", DataType = "Int16")]
        public Int32 QueueNo
        {
            get { return _QueueNo; }
            set { _QueueNo = value; }
        }
        [Column(Name = "DischargeDate", DataType = "DateTime")]
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set { _DischargeDate = value; }
        }
        [Column(Name = "DischargeTime", DataType = "String")]
        public String DischargeTime
        {
            get { return _DischargeTime; }
            set { _DischargeTime = value; }
        }
        [Column(Name = "GCVisitStatus", DataType = "String")]
        public String GCVisitStatus
        {
            get { return _GCVisitStatus; }
            set { _GCVisitStatus = value; }
        }
        [Column(Name = "GCRegistrationStatus", DataType = "String")]
        public String GCRegistrationStatus
        {
            get { return _GCRegistrationStatus; }
            set { _GCRegistrationStatus = value; }
        }
        [Column(Name = "IsMainVisit", DataType = "Boolean")]
        public Boolean IsMainVisit
        {
            get { return _IsMainVisit; }
            set { _IsMainVisit = value; }
        }
        [Column(Name = "GCAdmissionCondition", DataType = "String")]
        public String GCAdmissionCondition
        {
            get { return _GCAdmissionCondition; }
            set { _GCAdmissionCondition = value; }
        }
        [Column(Name = "AdmissionCondition", DataType = "String")]
        public String AdmissionCondition
        {
            get { return _AdmissionCondition; }
            set { _AdmissionCondition = value; }
        }
        [Column(Name = "IsNewBorn", DataType = "Boolean")]
        public Boolean IsNewBorn
        {
            get { return _IsNewBorn; }
            set { _IsNewBorn = value; }
        }
        [Column(Name = "IsParturition", DataType = "Boolean")]
        public Boolean IsParturition
        {
            get { return _IsParturition; }
            set { _IsParturition = value; }
        }
        [Column(Name = "ReferrerName", DataType = "String")]
        public String ReferrerName
        {
            get { return _ReferrerName; }
            set { _ReferrerName = value; }
        }
        [Column(Name = "PlanDischargeDate", DataType = "DateTime")]
        public DateTime PlanDischargeDate
        {
            get { return _PlanDischargeDate; }
            set { _PlanDischargeDate = value; }
        }
        [Column(Name = "GCDischargeCondition", DataType = "String")]
        public String GCDischargeCondition
        {
            get { return _GCDischargeCondition; }
            set { _GCDischargeCondition = value; }
        }
        [Column(Name = "DischargeCondition", DataType = "String")]
        public String DischargeCondition
        {
            get { return _DischargeCondition; }
            set { _DischargeCondition = value; }
        }
        [Column(Name = "GCDischargeMethod", DataType = "String")]
        public String GCDischargeMethod
        {
            get { return _GCDischargeMethod; }
            set { _GCDischargeMethod = value; }
        }
        [Column(Name = "DischargeMethod", DataType = "String")]
        public String DischargeMethod
        {
            get { return _DischargeMethod; }
            set { _DischargeMethod = value; }
        }
        [Column(Name = "LOSInDay", DataType = "Decimal")]
        public Decimal LOSInDay
        {
            get { return _LOSInDay; }
            set { _LOSInDay = value; }
        }
        [Column(Name = "LOSInHour", DataType = "Decimal")]
        public Decimal LOSInHour
        {
            get { return _LOSInHour; }
            set { _LOSInHour = value; }
        }
        [Column(Name = "LOSInMinute", DataType = "Decimal")]
        public Decimal LOSInMinute
        {
            get { return _LOSInMinute; }
            set { _LOSInMinute = value; }
        }
        [Column(Name = "GCCustomerType", DataType = "String")]
        public String GCCustomerType
        {
            get { return _GCCustomerType; }
            set { _GCCustomerType = value; }
        }
        [Column(Name = "CustomerType", DataType = "String")]
        public String CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        [Column(Name = "BusinessPartnerID", DataType = "Int32")]
        public Int32 BusinessPartnerID
        {
            get { return _BusinessPartnerID; }
            set { _BusinessPartnerID = value; }
        }
        [Column(Name = "BusinessPartnerCode", DataType = "String")]
        public String BusinessPartnerCode
        {
            get { return _BusinessPartnerCode; }
            set { _BusinessPartnerCode = value; }
        }
        [Column(Name = "BusinessPartnerName", DataType = "String")]
        public String BusinessPartnerName
        {
            get { return _BusinessPartnerName; }
            set { _BusinessPartnerName = value; }
        }
        [Column(Name = "EmployeeID", DataType = "Int32")]
        public Int32 EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }
        [Column(Name = "EmployeeCode", DataType = "String")]
        public String EmployeeCode
        {
            get { return _EmployeeCode; }
            set { _EmployeeCode = value; }
        }
        [Column(Name = "EmployeeName", DataType = "String")]
        public String EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; }
        }
        [Column(Name = "MRN", DataType = "Int32")]
        public Int32 MRN
        {
            get { return _MRN; }
            set { _MRN = value; }
        }
        [Column(Name = "GuestID", DataType = "Int32")]
        public Int32 GuestID
        {
            get { return _GuestID; }
            set { _GuestID = value; }
        }
        [Column(Name = "GCPatientCategory", DataType = "String")]
        public String GCPatientCategory
        {
            get { return _GCPatientCategory; }
            set { _GCPatientCategory = value; }
        }
        [Column(Name = "PatientCategory", DataType = "String")]
        public String PatientCategory
        {
            get { return _PatientCategory; }
            set { _PatientCategory = value; }
        }
        [Column(Name = "GCTriage", DataType = "String")]
        public String GCTriage
        {
            get { return _GCTriage; }
            set { _GCTriage = value; }
        }
        [Column(Name = "TriageColor", DataType = "String")]
        public String TriageColor
        {
            get { return _TriageColor; }
            set { _TriageColor = value; }
        }
        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "OldMedicalNo", DataType = "String")]
        public String OldMedicalNo
        {
            get { return _OldMedicalNo; }
            set { _OldMedicalNo = value; }
        }
        [Column(Name = "GCSalutation", DataType = "String")]
        public String GCSalutation
        {
            get { return _GCSalutation; }
            set { _GCSalutation = value; }
        }
        [Column(Name = "Salutation", DataType = "String")]
        public String Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "PreferredName", DataType = "String")]
        public String PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
        [Column(Name = "CityOfBirth", DataType = "String")]
        public String CityOfBirth
        {
            get { return _CityOfBirth; }
            set { _CityOfBirth = value; }
        }
        [Column(Name = "DateOfBirth", DataType = "DateTime")]
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        [Column(Name = "GCSex", DataType = "String")]
        public String GCSex
        {
            get { return _GCSex; }
            set { _GCSex = value; }
        }
        [Column(Name = "Sex", DataType = "String")]
        public String Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [Column(Name = "GCReligion", DataType = "String")]
        public String GCReligion
        {
            get { return _GCReligion; }
            set { _GCReligion = value; }
        }
        [Column(Name = "Religion", DataType = "String")]
        public String Religion
        {
            get { return _Religion; }
            set { _Religion = value; }
        }
        [Column(Name = "PictureFileName", DataType = "String")]
        public String PictureFileName
        {
            get { return _PictureFileName; }
            set { _PictureFileName = value; }
        }
        [Column(Name = "GCGender", DataType = "String")]
        public String GCGender
        {
            get { return _GCGender; }
            set { _GCGender = value; }
        }
        [Column(Name = "Gender", DataType = "String")]
        public String Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [Column(Name = "PhoneNo1", DataType = "String")]
        public String PhoneNo1
        {
            get { return _PhoneNo1; }
            set { _PhoneNo1 = value; }
        }
        [Column(Name = "PhoneNo2", DataType = "String")]
        public String PhoneNo2
        {
            get { return _PhoneNo2; }
            set { _PhoneNo2 = value; }
        }
        [Column(Name = "MobilePhoneNo1", DataType = "String")]
        public String MobilePhoneNo1
        {
            get { return _MobilePhoneNo1; }
            set { _MobilePhoneNo1 = value; }
        }
        [Column(Name = "MobilePhoneNo2", DataType = "String")]
        public String MobilePhoneNo2
        {
            get { return _MobilePhoneNo2; }
            set { _MobilePhoneNo2 = value; }
        }
        [Column(Name = "StreetName", DataType = "String")]
        public String StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
        [Column(Name = "County", DataType = "String")]
        public String County
        {
            get { return _County; }
            set { _County = value; }
        }
        [Column(Name = "District", DataType = "String")]
        public String District
        {
            get { return _District; }
            set { _District = value; }
        }
        [Column(Name = "City", DataType = "String")]
        public String City
        {
            get { return _City; }
            set { _City = value; }
        }
        [Column(Name = "GCState", DataType = "String")]
        public String GCState
        {
            get { return _GCState; }
            set { _GCState = value; }
        }
        [Column(Name = "State", DataType = "String")]
        public String State
        {
            get { return _State; }
            set { _State = value; }
        }
        [Column(Name = "ZipCodeID", DataType = "Int32")]
        public Int32 ZipCodeID
        {
            get { return _ZipCodeID; }
            set { _ZipCodeID = value; }
        }
        [Column(Name = "ZipCode", DataType = "String")]
        public String ZipCode
        {
            get { return _ZipCode; }
            set { _ZipCode = value; }
        }
        [Column(Name = "GCMedicalFileStatus", DataType = "String")]
        public String GCMedicalFileStatus
        {
            get { return _GCMedicalFileStatus; }
            set { _GCMedicalFileStatus = value; }
        }
        [Column(Name = "PatientAllergy", DataType = "String")]
        public String PatientAllergy
        {
            get { return _PatientAllergy; }
            set { _PatientAllergy = value; }
        }
        [Column(Name = "IsChargesTransfered", DataType = "Boolean")]
        public Boolean IsChargesTransfered
        {
            get { return _IsChargesTransfered; }
            set { _IsChargesTransfered = value; }
        }
        [Column(Name = "LinkedRegistrationID", DataType = "Int32")]
        public Int32 LinkedRegistrationID
        {
            get { return _LinkedRegistrationID; }
            set { _LinkedRegistrationID = value; }
        }
        [Column(Name = "IsHasAllergy", DataType = "Boolean")]
        public Boolean IsHasAllergy
        {
            get { return _IsHasAllergy; }
            set { _IsHasAllergy = value; }
        }
        [Column(Name = "IsFallRisk", DataType = "Boolean")]
        public Boolean IsFallRisk
        {
            get { return _IsFallRisk; }
            set { _IsFallRisk = value; }
        }
        [Column(Name = "IsDNR", DataType = "Boolean")]
        public Boolean IsDNR
        {
            get { return _IsDNR; }
            set { _IsDNR = value; }
        }
    }
    #endregion

    #region vInpatientServiceUnitLink
    [Serializable]
    [Table(Name = "vInpatientServiceUnitLink")]
    public class vInpatientServiceUnitLink
    {
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Boolean _IsDeleted;

        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Boolean")]
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
    }
    #endregion

    #region vInpatientServiceUnitLinkPerUser
    [Serializable]
    [Table(Name = "vInpatientServiceUnitLinkPerUser")]
    public class vInpatientServiceUnitLinkPerUser
    {
        private String _UserID;
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Boolean _IsDeleted;

        [Column(Name = "UserID", DataType = "String")]
        public String UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Boolean")]
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
    }
    #endregion

    #region vOutPatientPatientListLink
    [Serializable]
    [Table(Name = "vOutPatientPatientListLink")]
    public partial class vOutPatientPatientListLink
    {
        private Int32 _VisitID;
        private Int32 _RegistrationID;
        private String _RegistrationNo;
        private Int32 _HealthcareServiceUnitID;
        private String _DepartmentID;
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _LocationID;
        private Int32 _LogisticLocationID;
        private Int32 _RoomID;
        private String _RoomCode;
        private String _RoomName;
        private Int32 _ClassID;
        private String _ClassCode;
        private String _ClassName;
        private Int32 _ChargeClassID;
        private String _ChargeClassCode;
        private String _ChargeClassName;
        private Int32 _BedID;
        private String _BedCode;
        private Int32 _ParamedicID;
        private String _ParamedicCode;
        private String _ParamedicName;
        private String _SpecialtyID;
        private String _SpecialtyName;
        private Int32 _VisitTypeID;
        private String _VisitTypeCode;
        private String _VisitTypeName;
        private DateTime _VisitDate;
        private String _VisitTime;
        private String _ActualVisitDate;
        private String _ActualVisitTime;
        private String _GCVisitReason;
        private Int32 _VisitReason;
        private Int32 _QueueNo;
        private DateTime _DischargeDate;
        private String _DischargeTime;
        private String _GCVisitStatus;
        private String _GCRegistrationStatus;
        private Boolean _IsMainVisit;
        private Int32 _GCAdmissionCondition;
        private String _AdmissionCondition;
        private String _IsNewBorn;
        private String _IsParturition;
        private Int32 _ReferrerName;
        private Int32 _PlanDischargeDate;
        private String _GCDischargeCondition;
        private String _DischargeCondition;
        private String _GCDischargeMethod;
        private String _DischargeMethod;
        private String _LOSInDay;
        private String _LOSInHour;
        private String _LOSInMinute;
        private String _GCCustomerType;
        private String _CustomerType;
        private Int32 _BusinessPartnerID;
        private String _BusinessPartnerCode;
        private String _BusinessPartnerName;
        private Int32 _EmployeeID;
        private Int32 _EmployeeCode;
        private Int32 _EmployeeName;
        private Int32 _MRN;
        private Int32 _GuestID;
        private Int32 _GCPatientCategory;
        private String _PatientCategory;
        private Int32 _GCTriage;
        private String _TriageColor;
        private String _MedicalNo;
        private Int32 _OldMedicalNo;
        private Int32 _GCSalutation;
        private Int32 _Salutation;
        private String _PatientName;
        private Int32 _PreferredName;
        private String _CityOfBirth;
        private DateTime _DateOfBirth;
        private String _GCSex;
        private String _Sex;
        private String _GCReligion;
        private String _Religion;
        private String _PictureFileName;
        private String _GCGender;
        private String _Gender;
        private String _PhoneNo1;
        private String _PhoneNo2;
        private String _MobilePhoneNo1;
        private String _MobilePhoneNo2;
        private String _StreetName;
        private String _County;
        private String _District;
        private String _City;
        private Int32 _GCState;
        private String _State;
        private Int32 _ZipCodeID;
        private String _ZipCode;
        private Int32 _GCMedicalFileStatus;
        private String _PatientAllergy;
        private Boolean _IsChargesTransfered;
        private Int32 _LinkedRegistrationID;
        private Boolean _IsHasAllergy;
        private Boolean _IsFallRisk;
        private Boolean _IsDNR;

        [Column(Name = "VisitID", DataType = "Int32")]
        public Int32 VisitID
        {
            get { return _VisitID; }
            set { _VisitID = value; }
        }
        [Column(Name = "RegistrationID", DataType = "Int32")]
        public Int32 RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String")]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "HealthcareServiceUnitID", DataType = "Int32")]
        public Int32 HealthcareServiceUnitID
        {
            get { return _HealthcareServiceUnitID; }
            set { _HealthcareServiceUnitID = value; }
        }
        [Column(Name = "DepartmentID", DataType = "String")]
        public String DepartmentID
        {
            get { return _DepartmentID; }
            set { _DepartmentID = value; }
        }
        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "LocationID", DataType = "Int32")]
        public Int32 LocationID
        {
            get { return _LocationID; }
            set { _LocationID = value; }
        }
        [Column(Name = "LogisticLocationID", DataType = "Int32")]
        public Int32 LogisticLocationID
        {
            get { return _LogisticLocationID; }
            set { _LogisticLocationID = value; }
        }
        [Column(Name = "RoomID", DataType = "Int32")]
        public Int32 RoomID
        {
            get { return _RoomID; }
            set { _RoomID = value; }
        }
        [Column(Name = "RoomCode", DataType = "String")]
        public String RoomCode
        {
            get { return _RoomCode; }
            set { _RoomCode = value; }
        }
        [Column(Name = "RoomName", DataType = "String")]
        public String RoomName
        {
            get { return _RoomName; }
            set { _RoomName = value; }
        }
        [Column(Name = "ClassID", DataType = "Int32")]
        public Int32 ClassID
        {
            get { return _ClassID; }
            set { _ClassID = value; }
        }
        [Column(Name = "ClassCode", DataType = "String")]
        public String ClassCode
        {
            get { return _ClassCode; }
            set { _ClassCode = value; }
        }
        [Column(Name = "ClassName", DataType = "String")]
        public String ClassName
        {
            get { return _ClassName; }
            set { _ClassName = value; }
        }
        [Column(Name = "ChargeClassID", DataType = "Int32")]
        public Int32 ChargeClassID
        {
            get { return _ChargeClassID; }
            set { _ChargeClassID = value; }
        }
        [Column(Name = "ChargeClassCode", DataType = "String")]
        public String ChargeClassCode
        {
            get { return _ChargeClassCode; }
            set { _ChargeClassCode = value; }
        }
        [Column(Name = "ChargeClassName", DataType = "String")]
        public String ChargeClassName
        {
            get { return _ChargeClassName; }
            set { _ChargeClassName = value; }
        }
        [Column(Name = "BedID", DataType = "Int32")]
        public Int32 BedID
        {
            get { return _BedID; }
            set { _BedID = value; }
        }
        [Column(Name = "BedCode", DataType = "String")]
        public String BedCode
        {
            get { return _BedCode; }
            set { _BedCode = value; }
        }
        [Column(Name = "ParamedicID", DataType = "Int32")]
        public Int32 ParamedicID
        {
            get { return _ParamedicID; }
            set { _ParamedicID = value; }
        }
        [Column(Name = "ParamedicCode", DataType = "String")]
        public String ParamedicCode
        {
            get { return _ParamedicCode; }
            set { _ParamedicCode = value; }
        }
        [Column(Name = "ParamedicName", DataType = "String")]
        public String ParamedicName
        {
            get { return _ParamedicName; }
            set { _ParamedicName = value; }
        }
        [Column(Name = "SpecialtyID", DataType = "String")]
        public String SpecialtyID
        {
            get { return _SpecialtyID; }
            set { _SpecialtyID = value; }
        }
        [Column(Name = "SpecialtyName", DataType = "String")]
        public String SpecialtyName
        {
            get { return _SpecialtyName; }
            set { _SpecialtyName = value; }
        }
        [Column(Name = "VisitTypeID", DataType = "Int32")]
        public Int32 VisitTypeID
        {
            get { return _VisitTypeID; }
            set { _VisitTypeID = value; }
        }
        [Column(Name = "VisitTypeCode", DataType = "String")]
        public String VisitTypeCode
        {
            get { return _VisitTypeCode; }
            set { _VisitTypeCode = value; }
        }
        [Column(Name = "VisitTypeName", DataType = "String")]
        public String VisitTypeName
        {
            get { return _VisitTypeName; }
            set { _VisitTypeName = value; }
        }
        [Column(Name = "VisitDate", DataType = "DateTime")]
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set { _VisitDate = value; }
        }
        [Column(Name = "VisitTime", DataType = "String")]
        public String VisitTime
        {
            get { return _VisitTime; }
            set { _VisitTime = value; }
        }
        [Column(Name = "ActualVisitDate", DataType = "String")]
        public String ActualVisitDate
        {
            get { return _ActualVisitDate; }
            set { _ActualVisitDate = value; }
        }
        [Column(Name = "ActualVisitTime", DataType = "String")]
        public String ActualVisitTime
        {
            get { return _ActualVisitTime; }
            set { _ActualVisitTime = value; }
        }
        [Column(Name = "GCVisitReason", DataType = "String")]
        public String GCVisitReason
        {
            get { return _GCVisitReason; }
            set { _GCVisitReason = value; }
        }
        [Column(Name = "VisitReason", DataType = "Int32")]
        public Int32 VisitReason
        {
            get { return _VisitReason; }
            set { _VisitReason = value; }
        }
        [Column(Name = "QueueNo", DataType = "Int32")]
        public Int32 QueueNo
        {
            get { return _QueueNo; }
            set { _QueueNo = value; }
        }
        [Column(Name = "DischargeDate", DataType = "DateTime")]
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set { _DischargeDate = value; }
        }
        [Column(Name = "DischargeTime", DataType = "String")]
        public String DischargeTime
        {
            get { return _DischargeTime; }
            set { _DischargeTime = value; }
        }
        [Column(Name = "GCVisitStatus", DataType = "String")]
        public String GCVisitStatus
        {
            get { return _GCVisitStatus; }
            set { _GCVisitStatus = value; }
        }
        [Column(Name = "GCRegistrationStatus", DataType = "String")]
        public String GCRegistrationStatus
        {
            get { return _GCRegistrationStatus; }
            set { _GCRegistrationStatus = value; }
        }
        [Column(Name = "IsMainVisit", DataType = "Boolean")]
        public Boolean IsMainVisit
        {
            get { return _IsMainVisit; }
            set { _IsMainVisit = value; }
        }
        [Column(Name = "GCAdmissionCondition", DataType = "Int32")]
        public Int32 GCAdmissionCondition
        {
            get { return _GCAdmissionCondition; }
            set { _GCAdmissionCondition = value; }
        }
        [Column(Name = "AdmissionCondition", DataType = "String")]
        public String AdmissionCondition
        {
            get { return _AdmissionCondition; }
            set { _AdmissionCondition = value; }
        }
        [Column(Name = "IsNewBorn", DataType = "String")]
        public String IsNewBorn
        {
            get { return _IsNewBorn; }
            set { _IsNewBorn = value; }
        }
        [Column(Name = "IsParturition", DataType = "String")]
        public String IsParturition
        {
            get { return _IsParturition; }
            set { _IsParturition = value; }
        }
        [Column(Name = "ReferrerName", DataType = "Int32")]
        public Int32 ReferrerName
        {
            get { return _ReferrerName; }
            set { _ReferrerName = value; }
        }
        [Column(Name = "PlanDischargeDate", DataType = "Int32")]
        public Int32 PlanDischargeDate
        {
            get { return _PlanDischargeDate; }
            set { _PlanDischargeDate = value; }
        }
        [Column(Name = "GCDischargeCondition", DataType = "String")]
        public String GCDischargeCondition
        {
            get { return _GCDischargeCondition; }
            set { _GCDischargeCondition = value; }
        }
        [Column(Name = "DischargeCondition", DataType = "String")]
        public String DischargeCondition
        {
            get { return _DischargeCondition; }
            set { _DischargeCondition = value; }
        }
        [Column(Name = "GCDischargeMethod", DataType = "String")]
        public String GCDischargeMethod
        {
            get { return _GCDischargeMethod; }
            set { _GCDischargeMethod = value; }
        }
        [Column(Name = "DischargeMethod", DataType = "String")]
        public String DischargeMethod
        {
            get { return _DischargeMethod; }
            set { _DischargeMethod = value; }
        }
        [Column(Name = "LOSInDay", DataType = "String")]
        public String LOSInDay
        {
            get { return _LOSInDay; }
            set { _LOSInDay = value; }
        }
        [Column(Name = "LOSInHour", DataType = "String")]
        public String LOSInHour
        {
            get { return _LOSInHour; }
            set { _LOSInHour = value; }
        }
        [Column(Name = "LOSInMinute", DataType = "String")]
        public String LOSInMinute
        {
            get { return _LOSInMinute; }
            set { _LOSInMinute = value; }
        }
        [Column(Name = "GCCustomerType", DataType = "String")]
        public String GCCustomerType
        {
            get { return _GCCustomerType; }
            set { _GCCustomerType = value; }
        }
        [Column(Name = "CustomerType", DataType = "String")]
        public String CustomerType
        {
            get { return _CustomerType; }
            set { _CustomerType = value; }
        }
        [Column(Name = "BusinessPartnerID", DataType = "Int32")]
        public Int32 BusinessPartnerID
        {
            get { return _BusinessPartnerID; }
            set { _BusinessPartnerID = value; }
        }
        [Column(Name = "BusinessPartnerCode", DataType = "String")]
        public String BusinessPartnerCode
        {
            get { return _BusinessPartnerCode; }
            set { _BusinessPartnerCode = value; }
        }
        [Column(Name = "BusinessPartnerName", DataType = "String")]
        public String BusinessPartnerName
        {
            get { return _BusinessPartnerName; }
            set { _BusinessPartnerName = value; }
        }
        [Column(Name = "EmployeeID", DataType = "Int32")]
        public Int32 EmployeeID
        {
            get { return _EmployeeID; }
            set { _EmployeeID = value; }
        }
        [Column(Name = "EmployeeCode", DataType = "Int32")]
        public Int32 EmployeeCode
        {
            get { return _EmployeeCode; }
            set { _EmployeeCode = value; }
        }
        [Column(Name = "EmployeeName", DataType = "Int32")]
        public Int32 EmployeeName
        {
            get { return _EmployeeName; }
            set { _EmployeeName = value; }
        }
        [Column(Name = "MRN", DataType = "Int32")]
        public Int32 MRN
        {
            get { return _MRN; }
            set { _MRN = value; }
        }
        [Column(Name = "GuestID", DataType = "Int32")]
        public Int32 GuestID
        {
            get { return _GuestID; }
            set { _GuestID = value; }
        }
        [Column(Name = "GCPatientCategory", DataType = "Int32")]
        public Int32 GCPatientCategory
        {
            get { return _GCPatientCategory; }
            set { _GCPatientCategory = value; }
        }
        [Column(Name = "PatientCategory", DataType = "String")]
        public String PatientCategory
        {
            get { return _PatientCategory; }
            set { _PatientCategory = value; }
        }
        [Column(Name = "GCTriage", DataType = "Int32")]
        public Int32 GCTriage
        {
            get { return _GCTriage; }
            set { _GCTriage = value; }
        }
        [Column(Name = "TriageColor", DataType = "String")]
        public String TriageColor
        {
            get { return _TriageColor; }
            set { _TriageColor = value; }
        }
        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "OldMedicalNo", DataType = "Int32")]
        public Int32 OldMedicalNo
        {
            get { return _OldMedicalNo; }
            set { _OldMedicalNo = value; }
        }
        [Column(Name = "GCSalutation", DataType = "Int32")]
        public Int32 GCSalutation
        {
            get { return _GCSalutation; }
            set { _GCSalutation = value; }
        }
        [Column(Name = "Salutation", DataType = "Int32")]
        public Int32 Salutation
        {
            get { return _Salutation; }
            set { _Salutation = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "PreferredName", DataType = "Int32")]
        public Int32 PreferredName
        {
            get { return _PreferredName; }
            set { _PreferredName = value; }
        }
        [Column(Name = "CityOfBirth", DataType = "String")]
        public String CityOfBirth
        {
            get { return _CityOfBirth; }
            set { _CityOfBirth = value; }
        }
        [Column(Name = "DateOfBirth", DataType = "DateTime")]
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        [Column(Name = "GCSex", DataType = "String")]
        public String GCSex
        {
            get { return _GCSex; }
            set { _GCSex = value; }
        }
        [Column(Name = "Sex", DataType = "String")]
        public String Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [Column(Name = "GCReligion", DataType = "String")]
        public String GCReligion
        {
            get { return _GCReligion; }
            set { _GCReligion = value; }
        }
        [Column(Name = "Religion", DataType = "String")]
        public String Religion
        {
            get { return _Religion; }
            set { _Religion = value; }
        }
        [Column(Name = "PictureFileName", DataType = "String")]
        public String PictureFileName
        {
            get { return _PictureFileName; }
            set { _PictureFileName = value; }
        }
        [Column(Name = "GCGender", DataType = "String")]
        public String GCGender
        {
            get { return _GCGender; }
            set { _GCGender = value; }
        }
        [Column(Name = "Gender", DataType = "String")]
        public String Gender
        {
            get { return _Gender; }
            set { _Gender = value; }
        }
        [Column(Name = "PhoneNo1", DataType = "String")]
        public String PhoneNo1
        {
            get { return _PhoneNo1; }
            set { _PhoneNo1 = value; }
        }
        [Column(Name = "PhoneNo2", DataType = "String")]
        public String PhoneNo2
        {
            get { return _PhoneNo2; }
            set { _PhoneNo2 = value; }
        }
        [Column(Name = "MobilePhoneNo1", DataType = "String")]
        public String MobilePhoneNo1
        {
            get { return _MobilePhoneNo1; }
            set { _MobilePhoneNo1 = value; }
        }
        [Column(Name = "MobilePhoneNo2", DataType = "String")]
        public String MobilePhoneNo2
        {
            get { return _MobilePhoneNo2; }
            set { _MobilePhoneNo2 = value; }
        }
        [Column(Name = "StreetName", DataType = "String")]
        public String StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
        [Column(Name = "County", DataType = "String")]
        public String County
        {
            get { return _County; }
            set { _County = value; }
        }
        [Column(Name = "District", DataType = "String")]
        public String District
        {
            get { return _District; }
            set { _District = value; }
        }
        [Column(Name = "City", DataType = "String")]
        public String City
        {
            get { return _City; }
            set { _City = value; }
        }
        [Column(Name = "GCState", DataType = "Int32")]
        public Int32 GCState
        {
            get { return _GCState; }
            set { _GCState = value; }
        }
        [Column(Name = "State", DataType = "String")]
        public String State
        {
            get { return _State; }
            set { _State = value; }
        }
        [Column(Name = "ZipCodeID", DataType = "Int32")]
        public Int32 ZipCodeID
        {
            get { return _ZipCodeID; }
            set { _ZipCodeID = value; }
        }
        [Column(Name = "ZipCode", DataType = "String")]
        public String ZipCode
        {
            get { return _ZipCode; }
            set { _ZipCode = value; }
        }
        [Column(Name = "GCMedicalFileStatus", DataType = "Int32")]
        public Int32 GCMedicalFileStatus
        {
            get { return _GCMedicalFileStatus; }
            set { _GCMedicalFileStatus = value; }
        }
        [Column(Name = "PatientAllergy", DataType = "String")]
        public String PatientAllergy
        {
            get { return _PatientAllergy; }
            set { _PatientAllergy = value; }
        }
        [Column(Name = "IsChargesTransfered", DataType = "Boolean")]
        public Boolean IsChargesTransfered
        {
            get { return _IsChargesTransfered; }
            set { _IsChargesTransfered = value; }
        }
        [Column(Name = "LinkedRegistrationID", DataType = "Int32")]
        public Int32 LinkedRegistrationID
        {
            get { return _LinkedRegistrationID; }
            set { _LinkedRegistrationID = value; }
        }
        [Column(Name = "IsHasAllergy", DataType = "Boolean")]
        public Boolean IsHasAllergy
        {
            get { return _IsHasAllergy; }
            set { _IsHasAllergy = value; }
        }
        [Column(Name = "IsFallRisk", DataType = "Boolean")]
        public Boolean IsFallRisk
        {
            get { return _IsFallRisk; }
            set { _IsFallRisk = value; }
        }
        [Column(Name = "IsDNR", DataType = "Boolean")]
        public Boolean IsDNR
        {
            get { return _IsDNR; }
            set { _IsDNR = value; }
        }
    }
    #endregion

    #region vOutPatientServiceUnitLinkPerUser
    [Serializable]
    [Table(Name = "vOutPatientServiceUnitLinkPerUser")]
    public class vOutPatientServiceUnitLinkPerUser
    {
        private String _UserID;
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Boolean _IsDeleted;

        [Column(Name = "UserID", DataType = "String")]
        public String UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Boolean")]
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
    }
    #endregion

    #region vParamedicMasterLink
    [Serializable]
    [Table(Name = "vParamedicMasterLink")]
    public class vParamedicMasterLink
    {
        private String _ParamedicCode;
        private String _ParamedicName;
        private Boolean _aktif;

        [Column(Name = "ParamedicCode", DataType = "String")]
        public String ParamedicCode
        {
            get { return _ParamedicCode; }
            set { _ParamedicCode = value; }
        }
        [Column(Name = "ParamedicName", DataType = "String")]
        public String ParamedicName
        {
            get { return _ParamedicName; }
            set { _ParamedicName = value; }
        }
        [Column(Name = "aktif", DataType = "Boolean")]
        public Boolean aktif
        {
            get { return _aktif; }
            set { _aktif = value; }
        }
    }
    #endregion

    #region vPatientListLink
    [Serializable]
    [Table(Name = "vPatientListLink")]
    public partial class vPatientListLink
    {
        private String _MedicalNo;
        private String _PatientName;
        private DateTime _DateOfBirth;
        private String _Sex;
        private String _StreetName;

        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "DateOfBirth", DataType = "DateTime")]
        public DateTime DateOfBirth
        {
            get { return _DateOfBirth; }
            set { _DateOfBirth = value; }
        }
        [Column(Name = "Sex", DataType = "String")]
        public String Sex
        {
            get { return _Sex; }
            set { _Sex = value; }
        }
        [Column(Name = "StreetName", DataType = "String")]
        public String StreetName
        {
            get { return _StreetName; }
            set { _StreetName = value; }
        }
    }
    #endregion

    #region vRegistrationListLink
    [Serializable]
    [Table(Name = "vRegistrationListLink")]
    public partial class vRegistrationListLink
    {
        private String _App;
        private String _MedicalNo;
        private String _RegistrationNo;
        private String _PatientName;
        private DateTime _VisitDate;
        private DateTime _DischargeDate;

        [Column(Name = "App", DataType = "String")]
        public String App
        {
            get { return _App; }
            set { _App = value; }
        }

        [Column(Name = "MedicalNo", DataType = "String")]
        public String MedicalNo
        {
            get { return _MedicalNo; }
            set { _MedicalNo = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String")]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "PatientName", DataType = "String")]
        public String PatientName
        {
            get { return _PatientName; }
            set { _PatientName = value; }
        }
        [Column(Name = "VisitDate", DataType = "DateTime")]
        public DateTime VisitDate
        {
            get { return _VisitDate; }
            set { _VisitDate = value; }
        }
        [Column(Name = "DischargeDate", DataType = "DateTime")]
        public DateTime DischargeDate
        {
            get { return _DischargeDate; }
            set { _DischargeDate = value; }
        }
    }
    #endregion

    #region vServiceUnitLink
    [Serializable]
    [Table(Name = "vServiceUnitLink")]
    public class vServiceUnitLink
    {
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _IsDeleted;
        private String _Tipe;

        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Int32")]
        public Int32 IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        [Column(Name = "Tipe", DataType = "String")]
        public String Tipe
        {
            get { return _Tipe; }
            set { _Tipe = value; }
        }
    }
    #endregion

    #region vServiceUnitLinkPerUser
    [Serializable]
    [Table(Name = "vServiceUnitLinkPerUser")]
    public class vServiceUnitLinkPerUser
    {
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _IsDeleted;
        private String _Tipe;
        private String _UserID;

        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Int32")]
        public Int32 IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        [Column(Name = "Tipe", DataType = "String")]
        public String Tipe
        {
            get { return _Tipe; }
            set { _Tipe = value; }
        }
        [Column(Name = "UserID", DataType = "String")]
        public String UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }
    #endregion

    #region vlnk_ServiceUnitPerUser
    [Serializable]
    [Table(Name = "vlnk_ServiceUnitPerUser")]
    public class vlnk_ServiceUnitPerUser
    {
        private String _ServiceUnitCode;
        private String _ServiceUnitName;
        private Int32 _IsDeleted;
        private String _Department;
        private String _UserID;

        [Column(Name = "ServiceUnitCode", DataType = "String")]
        public String ServiceUnitCode
        {
            get { return _ServiceUnitCode; }
            set { _ServiceUnitCode = value; }
        }
        [Column(Name = "ServiceUnitName", DataType = "String")]
        public String ServiceUnitName
        {
            get { return _ServiceUnitName; }
            set { _ServiceUnitName = value; }
        }
        [Column(Name = "IsDeleted", DataType = "Int32")]
        public Int32 IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        [Column(Name = "Department", DataType = "String")]
        public String Department
        {
            get { return _Department; }
            set { _Department = value; }
        }
        [Column(Name = "UserID", DataType = "String")]
        public String UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }
    }
    #endregion
    #endregion 
     
    #region Medinfras LINK
    #region ARInvoiceReportDt
    [Serializable]
    [Table(Name = "ARInvoiceReportDt")]
    public class ARInvoiceReportDt : DbDataModel
    {
        private Int32 _ID;
        private Int32? _ARInvoiceReportID;
        private Int32? _RegistrationID;
        private Int32? _PaymentID;
        private Int32? _PaymentDetailID;
        private Int32 _JobStatusDt;
        private Int32 _LinkedToRegistrationID;
        private String _LinkedToRegistrationNo;
        private String _RegistrationNo; 

        [Column(Name = "ID", DataType = "Int32", IsPrimaryKey = true, IsIdentity = true)]
        public Int32 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [Column(Name = "ARInvoiceReportID", DataType = "Int32", IsNullable = true)]
        public Int32? ARInvoiceReportID
        {
            get { return _ARInvoiceReportID; }
            set { _ARInvoiceReportID = value; }
        }
        [Column(Name = "RegistrationID", DataType = "Int32", IsNullable = true)]
        public Int32? RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }
        [Column(Name = "PaymentID", DataType = "Int32", IsNullable = true)]
        public Int32? PaymentID
        {
            get { return _PaymentID; }
            set { _PaymentID = value; }
        }
        [Column(Name = "PaymentDetailID", DataType = "Int32", IsNullable = true)]
        public Int32? PaymentDetailID
        {
            get { return _PaymentDetailID; }
            set { _PaymentDetailID = value; }
        }
        [Column(Name = "JobStatusDt", DataType = "Int32")]
        public Int32 JobStatusDt
        {
            get { return _JobStatusDt; }
            set { _JobStatusDt = value; }
        }
        [Column(Name = "LinkedToRegistrationID", DataType = "Int32")]
        public Int32 LinkedToRegistrationID
        {
            get { return _LinkedToRegistrationID; }
            set { _LinkedToRegistrationID = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String", IsNullable = true)]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "LinkedToRegistrationNo", DataType = "String", IsNullable = true)]
        public String LinkedToRegistrationNo
        {
            get { return _LinkedToRegistrationNo; }
            set { _LinkedToRegistrationNo = value; }
        }
    }

    public class ARInvoiceReportDtDao
    {
        private readonly IDbContext _ctx = DbFactory.Configure();
        private readonly DbHelper _helper = new DbHelper(typeof(ARInvoiceReportDt));
        private bool _isAuditLog = false;
        private const string p_ID = "@p_ID";
        public ARInvoiceReportDtDao() { }
        public ARInvoiceReportDtDao(IDbContext ctx)
        {
            _ctx = ctx;
        }
        public ARInvoiceReportDt Get(Int32 ID)
        {
            _ctx.CommandText = _helper.GetRecord();
            _ctx.Add(p_ID, ID);
            DataRow row = DaoBase.GetDataRow(_ctx);
            return (row == null) ? null : (ARInvoiceReportDt)_helper.DataRowToObject(row, new ARInvoiceReportDt());
        }
        public int Insert(ARInvoiceReportDt record)
        {
            _helper.Insert(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
        public int InsertReturnPrimaryKeyID(ARInvoiceReportDt record)
        {
            _helper.InsertReturnPrimaryKeyID(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteScalar(_ctx);
        }
        public int Update(ARInvoiceReportDt record)
        {
            _helper.Update(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx, true);
        }
        public int Delete(Int32 ID)
        {
            ARInvoiceReportDt record;
            if (_ctx.Transaction == null)
                record = new ARInvoiceReportDtDao().Get(ID);
            else
                record = Get(ID);
            _helper.Delete(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
    }
    #endregion
    #region ARInvoiceReportHD
    [Serializable]
    [Table(Name = "ARInvoiceReportHD")]
    public class ARInvoiceReportHD : DbDataModel
    {
        private Int32 _ID;
        private Int32? _ARInvoiceID;
        private String _ARInvoiceNo;
        private DateTime? _ARInvoiceDate;
        private Int32 _JobStatus;
        private Int32? _JobRequestBy;
        private DateTime? _JobRequestDate;
        private String _ReportCode;

        [Column(Name = "ID", DataType = "Int32", IsPrimaryKey = true, IsIdentity = true)]
        public Int32 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [Column(Name = "ARInvoiceID", DataType = "Int32", IsNullable = true)]
        public Int32? ARInvoiceID
        {
            get { return _ARInvoiceID; }
            set { _ARInvoiceID = value; }
        }
        [Column(Name = "ARInvoiceNo", DataType = "String", IsNullable = true)]
        public String ARInvoiceNo
        {
            get { return _ARInvoiceNo; }
            set { _ARInvoiceNo = value; }
        }
        [Column(Name = "ARInvoiceDate", DataType = "DateTime", IsNullable = true)]
        public DateTime? ARInvoiceDate
        {
            get { return _ARInvoiceDate; }
            set { _ARInvoiceDate = value; }
        }
        [Column(Name = "JobStatus", DataType = "Int32")]
        public Int32 JobStatus
        {
            get { return _JobStatus; }
            set { _JobStatus = value; }
        }
        [Column(Name = "JobRequestBy", DataType = "Int32", IsNullable = true)]
        public Int32? JobRequestBy
        {
            get { return _JobRequestBy; }
            set { _JobRequestBy = value; }
        }
        [Column(Name = "JobRequestDate", DataType = "DateTime", IsNullable = true)]
        public DateTime? JobRequestDate
        {
            get { return _JobRequestDate; }
            set { _JobRequestDate = value; }
        }
        [Column(Name = "ReportCode", DataType = "String", IsNullable = true)]
        public String ReportCode
        {
            get { return _ReportCode; }
            set { _ReportCode = value; }
        }
    }

    public class ARInvoiceReportHDDao
    {
        private readonly IDbContext _ctx = DbFactory.Configure();
        private readonly DbHelper _helper = new DbHelper(typeof(ARInvoiceReportHD));
        private bool _isAuditLog = false;
        private const string p_ID = "@p_ID";
        public ARInvoiceReportHDDao() { }
        public ARInvoiceReportHDDao(IDbContext ctx)
        {
            _ctx = ctx;
        }
        public ARInvoiceReportHD Get(Int32 ID)
        {
            _ctx.CommandText = _helper.GetRecord();
            _ctx.Add(p_ID, ID);
            DataRow row = DaoBase.GetDataRow(_ctx);
            return (row == null) ? null : (ARInvoiceReportHD)_helper.DataRowToObject(row, new ARInvoiceReportHD());
        }
        public int Insert(ARInvoiceReportHD record)
        {
            _helper.Insert(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
        public int InsertReturnPrimaryKeyID(ARInvoiceReportHD record)
        {
            _helper.InsertReturnPrimaryKeyID(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteScalar(_ctx);
        }
        public int Update(ARInvoiceReportHD record)
        {
            _helper.Update(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx, true);
        }
        public int Delete(Int32 ID)
        {
            ARInvoiceReportHD record;
            if (_ctx.Transaction == null)
                record = new ARInvoiceReportHDDao().Get(ID);
            else
                record = Get(ID);
            _helper.Delete(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
    }
    #endregion
    
    #region RegistrationBpjsReport
    #region RegistrationBpjsReport
    [Serializable]
    [Table(Name = "RegistrationBpjsReport")]
    public partial class RegistrationBpjsReport : DbDataModel
    {
        private Int32 _ID;
        private Int32? _RegistrationID;
        private String _RegistrationNo;
        private String _NoSEP;
        private String _NoPeserta;
        private Int32? _JobStatus;
        private Int32? _JobRequestBy;
        private DateTime? _JobRequestDate;
        private String _ReportCode;

        [Column(Name = "ID", DataType = "Int32", IsPrimaryKey = true, IsIdentity = true)]
        public Int32 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        [Column(Name = "RegistrationID", DataType = "Int32", IsNullable = true)]
        public Int32? RegistrationID
        {
            get { return _RegistrationID; }
            set { _RegistrationID = value; }
        }
        [Column(Name = "RegistrationNo", DataType = "String", IsNullable = true)]
        public String RegistrationNo
        {
            get { return _RegistrationNo; }
            set { _RegistrationNo = value; }
        }
        [Column(Name = "NoSEP", DataType = "String", IsNullable = true)]
        public String NoSEP
        {
            get { return _NoSEP; }
            set { _NoSEP = value; }
        }
        [Column(Name = "NoPeserta", DataType = "String", IsNullable = true)]
        public String NoPeserta
        {
            get { return _NoPeserta; }
            set { _NoPeserta = value; }
        }
        [Column(Name = "JobStatus", DataType = "Int32", IsNullable = true)]
        public Int32? JobStatus
        {
            get { return _JobStatus; }
            set { _JobStatus = value; }
        }
        [Column(Name = "JobRequestBy", DataType = "Int32", IsNullable = true)]
        public Int32? JobRequestBy
        {
            get { return _JobRequestBy; }
            set { _JobRequestBy = value; }
        }
        [Column(Name = "JobRequestDate", DataType = "DateTime", IsNullable = true)]
        public DateTime? JobRequestDate
        {
            get { return _JobRequestDate; }
            set { _JobRequestDate = value; }
        }
        [Column(Name = "ReportCode", DataType = "String", IsNullable = true)]
        public String ReportCode
        {
            get { return _ReportCode; }
            set { _ReportCode = value; }
        }
    }

    public class RegistrationBpjsReportDao
    {
        private readonly IDbContext _ctx = DbFactory.Configure();
        private readonly DbHelper _helper = new DbHelper(typeof(RegistrationBpjsReport));
        private bool _isAuditLog = false;
        private const string p_ID = "@p_ID";
        public RegistrationBpjsReportDao() { }
        public RegistrationBpjsReportDao(IDbContext ctx)
        {
            _ctx = ctx;
        }
        public RegistrationBpjsReport Get(Int32 ID)
        {
            _ctx.CommandText = _helper.GetRecord();
            _ctx.Add(p_ID, ID);
            DataRow row = DaoBase.GetDataRow(_ctx);
            return (row == null) ? null : (RegistrationBpjsReport)_helper.DataRowToObject(row, new RegistrationBpjsReport());
        }
        public int Insert(RegistrationBpjsReport record)
        {
            _helper.Insert(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
        public int InsertReturnPrimaryKeyID(RegistrationBpjsReport record)
        {
            _helper.InsertReturnPrimaryKeyID(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteScalar(_ctx);
        }
        public int Update(RegistrationBpjsReport record)
        {
            _helper.Update(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx, true);
        }
        public int Delete(Int32 ID)
        {
            RegistrationBpjsReport record;
            if (_ctx.Transaction == null)
                record = new RegistrationBpjsReportDao().Get(ID);
            else
                record = Get(ID);
            _helper.Delete(_ctx, record, _isAuditLog);
            return DaoBase.ExecuteNonQuery(_ctx);
        }
    }
    #endregion
    #endregion
    #endregion

}