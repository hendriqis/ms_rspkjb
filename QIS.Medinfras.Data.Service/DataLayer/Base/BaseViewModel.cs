using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Data.Core.Dal;
using QIS.Medinfras.Common;
using QIS.Medinfras.Data.Service.Helper;

namespace QIS.Medinfras.Data.Service
{
    public class BaseViewModel
    {
        public partial class vwPrescriptionOrderHd
        {
            protected Int32 _PrescriptionOrderID;
            protected Int32 _RegistrationID;
            protected String _RegistrationNo;
            protected String _TransactionCode;
            protected String _PrescriptionOrderNo;
            protected Int32 _VisitID;
            protected String _DepartmentID;
            protected String _DepartmentName;
            protected Int32 _HealthcareServiceUnitID;
            protected String _ServiceUnitCode;
            protected String _ServiceUnitName;
            protected Int32 _ParamedicID;
            protected String _ParamedicCode;
            protected String _ParamedicName;
            protected Int32 _DispensaryServiceUnitID;
            protected String _DispensaryServiceUnitCode;
            protected String _DispensaryServiceUnitName;
            protected Int32 _ClassID;
            protected String _ClassName;
            protected Int32 _LocationID;
            protected String _LocationName;
            protected String _GCPrescriptionType;
            protected String _PrescriptionType;
            protected DateTime _PrescriptionDate;
            protected String _PrescriptionTime;
            protected String _ReferenceNo;
            protected String _GCRefillInstruction;
            protected String _RefillInstruction;
            protected String _GCTransactionStatus;
            protected String _GCOrderStatus;
            protected String _OrderStatus;
            protected String _Remarks;
            protected String _TransactionStatus;
            protected String _TransactionStatusWatermark;
            protected String _MedicalNo;
            protected String _PatientName;
            protected DateTime _DateOfBirth;
            protected String _Sex;
            protected String _BusinessPartnerName;
            protected Int32 _LinkedRegistrationID;
            protected DateTime _CreatedDate;
            protected String _CreatedByName;
            protected DateTime _LastUpdatedDate;
            protected String _LastUpdateByName;
            protected DateTime _StartDate;
            protected String _StartTime;
            protected String _StartByName;
            protected DateTime _CompleteDate;
            protected String _CompleteTime;
            protected String _CompleteByName;
            protected DateTime _ClosedDate;
            protected String _ClosedTime;
            protected String _ClosedByName;
            protected Boolean _IsOutstandingOrder;

            [Column(Name = "PrescriptionOrderID", DataType = "Int32")]
            public Int32 PrescriptionOrderID
            {
                get { return _PrescriptionOrderID; }
                set { _PrescriptionOrderID = value; }
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
            [Column(Name = "TransactionCode", DataType = "String")]
            public String TransactionCode
            {
                get { return _TransactionCode; }
                set { _TransactionCode = value; }
            }
            [Column(Name = "PrescriptionOrderNo", DataType = "String")]
            public String PrescriptionOrderNo
            {
                get { return _PrescriptionOrderNo; }
                set { _PrescriptionOrderNo = value; }
            }
            [Column(Name = "VisitID", DataType = "Int32")]
            public Int32 VisitID
            {
                get { return _VisitID; }
                set { _VisitID = value; }
            }
            [Column(Name = "DepartmentID", DataType = "String")]
            public String DepartmentID
            {
                get { return _DepartmentID; }
                set { _DepartmentID = value; }
            }
            [Column(Name = "DepartmentName", DataType = "String")]
            public String DepartmentName
            {
                get { return _DepartmentName; }
                set { _DepartmentName = value; }
            }
            [Column(Name = "HealthcareServiceUnitID", DataType = "Int32")]
            public Int32 HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
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
            [Column(Name = "DispensaryServiceUnitID", DataType = "Int32")]
            public Int32 DispensaryServiceUnitID
            {
                get { return _DispensaryServiceUnitID; }
                set { _DispensaryServiceUnitID = value; }
            }
            [Column(Name = "DispensaryServiceUnitCode", DataType = "String")]
            public String DispensaryServiceUnitCode
            {
                get { return _DispensaryServiceUnitCode; }
                set { _DispensaryServiceUnitCode = value; }
            }
            [Column(Name = "DispensaryServiceUnitName", DataType = "String")]
            public String DispensaryServiceUnitName
            {
                get { return _DispensaryServiceUnitName; }
                set { _DispensaryServiceUnitName = value; }
            }
            [Column(Name = "ClassID", DataType = "Int32")]
            public Int32 ClassID
            {
                get { return _ClassID; }
                set { _ClassID = value; }
            }
            [Column(Name = "ClassName", DataType = "String")]
            public String ClassName
            {
                get { return _ClassName; }
                set { _ClassName = value; }
            }
            [Column(Name = "LocationID", DataType = "Int32")]
            public Int32 LocationID
            {
                get { return _LocationID; }
                set { _LocationID = value; }
            }
            [Column(Name = "LocationName", DataType = "String")]
            public String LocationName
            {
                get { return _LocationName; }
                set { _LocationName = value; }
            }
            [Column(Name = "GCPrescriptionType", DataType = "String")]
            public String GCPrescriptionType
            {
                get { return _GCPrescriptionType; }
                set { _GCPrescriptionType = value; }
            }
            [Column(Name = "PrescriptionType", DataType = "String")]
            public String PrescriptionType
            {
                get { return _PrescriptionType; }
                set { _PrescriptionType = value; }
            }
            [Column(Name = "PrescriptionDate", DataType = "DateTime")]
            public DateTime PrescriptionDate
            {
                get { return _PrescriptionDate; }
                set { _PrescriptionDate = value; }
            }
            [Column(Name = "PrescriptionTime", DataType = "String")]
            public String PrescriptionTime
            {
                get { return _PrescriptionTime; }
                set { _PrescriptionTime = value; }
            }
            [Column(Name = "ReferenceNo", DataType = "String")]
            public String ReferenceNo
            {
                get { return _ReferenceNo; }
                set { _ReferenceNo = value; }
            }
            [Column(Name = "GCRefillInstruction", DataType = "String")]
            public String GCRefillInstruction
            {
                get { return _GCRefillInstruction; }
                set { _GCRefillInstruction = value; }
            }
            [Column(Name = "RefillInstruction", DataType = "String")]
            public String RefillInstruction
            {
                get { return _RefillInstruction; }
                set { _RefillInstruction = value; }
            }
            [Column(Name = "GCTransactionStatus", DataType = "String")]
            public String GCTransactionStatus
            {
                get { return _GCTransactionStatus; }
                set { _GCTransactionStatus = value; }
            }
            [Column(Name = "GCOrderStatus", DataType = "String")]
            public String GCOrderStatus
            {
                get { return _GCOrderStatus; }
                set { _GCOrderStatus = value; }
            }
            [Column(Name = "OrderStatus", DataType = "String")]
            public String OrderStatus
            {
                get { return _OrderStatus; }
                set { _OrderStatus = value; }
            }
            [Column(Name = "Remarks", DataType = "String")]
            public String Remarks
            {
                get { return _Remarks; }
                set { _Remarks = value; }
            }
            [Column(Name = "TransactionStatus", DataType = "String")]
            public String TransactionStatus
            {
                get { return _TransactionStatus; }
                set { _TransactionStatus = value; }
            }
            [Column(Name = "TransactionStatusWatermark", DataType = "String")]
            public String TransactionStatusWatermark
            {
                get { return _TransactionStatusWatermark; }
                set { _TransactionStatusWatermark = value; }
            }
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
            [Column(Name = "BusinessPartnerName", DataType = "String")]
            public String BusinessPartnerName
            {
                get { return _BusinessPartnerName; }
                set { _BusinessPartnerName = value; }
            }
            [Column(Name = "LinkedRegistrationID", DataType = "Int32")]
            public Int32 LinkedRegistrationID
            {
                get { return _LinkedRegistrationID; }
                set { _LinkedRegistrationID = value; }
            }
            [Column(Name = "CreatedDate", DataType = "DateTime")]
            public DateTime CreatedDate
            {
                get { return _CreatedDate; }
                set { _CreatedDate = value; }
            }
            [Column(Name = "CreatedByName", DataType = "String")]
            public String CreatedByName
            {
                get { return _CreatedByName; }
                set { _CreatedByName = value; }
            }
            [Column(Name = "LastUpdatedDate", DataType = "DateTime")]
            public DateTime LastUpdatedDate
            {
                get { return _LastUpdatedDate; }
                set { _LastUpdatedDate = value; }
            }
            [Column(Name = "LastUpdateByName", DataType = "String")]
            public String LastUpdateByName
            {
                get { return _LastUpdateByName; }
                set { _LastUpdateByName = value; }
            }
            [Column(Name = "StartDate", DataType = "DateTime")]
            public DateTime StartDate
            {
                get { return _StartDate; }
                set { _StartDate = value; }
            }
            [Column(Name = "StartTime", DataType = "String")]
            public String StartTime
            {
                get { return _StartTime; }
                set { _StartTime = value; }
            }
            [Column(Name = "StartByName", DataType = "String")]
            public String StartByName
            {
                get { return _StartByName; }
                set { _StartByName = value; }
            }
            [Column(Name = "CompleteDate", DataType = "DateTime")]
            public DateTime CompleteDate
            {
                get { return _CompleteDate; }
                set { _CompleteDate = value; }
            }
            [Column(Name = "CompleteTime", DataType = "String")]
            public String CompleteTime
            {
                get { return _CompleteTime; }
                set { _CompleteTime = value; }
            }
            [Column(Name = "CompleteByName", DataType = "String")]
            public String CompleteByName
            {
                get { return _CompleteByName; }
                set { _CompleteByName = value; }
            }
            [Column(Name = "ClosedDate", DataType = "DateTime")]
            public DateTime ClosedDate
            {
                get { return _ClosedDate; }
                set { _ClosedDate = value; }
            }
            [Column(Name = "ClosedTime", DataType = "String")]
            public String ClosedTime
            {
                get { return _ClosedTime; }
                set { _ClosedTime = value; }
            }
            [Column(Name = "ClosedByName", DataType = "String")]
            public String ClosedByName
            {
                get { return _ClosedByName; }
                set { _ClosedByName = value; }
            }
            [Column(Name = "IsOutstandingOrder", DataType = "Boolean")]
            public Boolean IsOutstandingOrder
            {
                get { return _IsOutstandingOrder; }
                set { _IsOutstandingOrder = value; }
            }

            public String cfPrescriptionDate
            {
                get { return _PrescriptionDate.ToString(Constant.FormatString.DATE_FORMAT); }
            }
        }

        #region vwConsultVisit
        [Serializable]
        public class vwConsultVisit
        {
            protected Int32 _VisitID;
            protected Int32 _RegistrationID;
            protected Int32 _HealthcareServiceUnitID;
            protected DateTime _DischargeDate;
            protected String _DischargeTime;
            protected DateTime _VisitDate;
            protected String _VisitTime;
            protected String _GCAdmissionCondition;
            protected String _GCVisitStatus;
            protected Int32 _VisitTypeID;
            protected Int32 _ClassID;
            protected Int32 _ChargeClassID;
            protected Int32 _ParamedicID;
            protected String _SpecialtyID;
            protected Int32 _RoomID;
            protected Int32 _MRN;
            protected String _RegistrationNo;
            protected String _GCCustomerType;
            protected Boolean _IsFallRisk;
            protected Boolean _IsDNR;
            protected Int32 _BusinessPartnerID;
            protected Int32 _LinkedRegistrationID;
            protected String _LastWeight;
            protected String _LastHeight;
            protected String _LastTemperature;
            protected String _LastBloodPressureS;
            protected String _LastBloodPressureD;
            protected String _LastGlucoseLevel;
            protected String _GCPatientCategory;
            protected String _GCTriage;

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
            [Column(Name = "HealthcareServiceUnitID", DataType = "Int32")]
            public Int32 HealthcareServiceUnitID
            {
                get { return _HealthcareServiceUnitID; }
                set { _HealthcareServiceUnitID = value; }
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
            [Column(Name = "GCAdmissionCondition", DataType = "String")]
            public String GCAdmissionCondition
            {
                get { return _GCAdmissionCondition; }
                set { _GCAdmissionCondition = value; }
            }
            [Column(Name = "GCVisitStatus", DataType = "String")]
            public String GCVisitStatus
            {
                get { return _GCVisitStatus; }
                set { _GCVisitStatus = value; }
            }
            [Column(Name = "VisitTypeID", DataType = "Int32")]
            public Int32 VisitTypeID
            {
                get { return _VisitTypeID; }
                set { _VisitTypeID = value; }
            }
            [Column(Name = "ClassID", DataType = "Int32")]
            public Int32 ClassID
            {
                get { return _ClassID; }
                set { _ClassID = value; }
            }
            [Column(Name = "ChargeClassID", DataType = "Int32")]
            public Int32 ChargeClassID
            {
                get { return _ChargeClassID; }
                set { _ChargeClassID = value; }
            }
            [Column(Name = "ParamedicID", DataType = "Int32")]
            public Int32 ParamedicID
            {
                get { return _ParamedicID; }
                set { _ParamedicID = value; }
            }
            [Column(Name = "SpecialtyID", DataType = "String")]
            public String SpecialtyID
            {
                get { return _SpecialtyID; }
                set { _SpecialtyID = value; }
            }
            [Column(Name = "RoomID", DataType = "Int32")]
            public Int32 RoomID
            {
                get { return _RoomID; }
                set { _RoomID = value; }
            }
            [Column(Name = "MRN", DataType = "Int32")]
            public Int32 MRN
            {
                get { return _MRN; }
                set { _MRN = value; }
            }
            [Column(Name = "RegistrationNo", DataType = "String")]
            public String RegistrationNo
            {
                get { return _RegistrationNo; }
                set { _RegistrationNo = value; }
            }
            [Column(Name = "GCCustomerType", DataType = "String")]
            public String GCCustomerType
            {
                get { return _GCCustomerType; }
                set { _GCCustomerType = value; }
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
            [Column(Name = "BusinessPartnerID", DataType = "Int32")]
            public Int32 BusinessPartnerID
            {
                get { return _BusinessPartnerID; }
                set { _BusinessPartnerID = value; }
            }
            [Column(Name = "LinkedRegistrationID", DataType = "Int32")]
            public Int32 LinkedRegistrationID
            {
                get { return _LinkedRegistrationID; }
                set { _LinkedRegistrationID = value; }
            }
            [Column(Name = "LastWeight", DataType = "String")]
            public String LastWeight
            {
                get { return _LastWeight; }
                set { _LastWeight = value; }
            }
            [Column(Name = "LastHeight", DataType = "String")]
            public String LastHeight
            {
                get { return _LastHeight; }
                set { _LastHeight = value; }
            }
            [Column(Name = "LastTemperature", DataType = "String")]
            public String LastTemperature
            {
                get { return _LastTemperature; }
                set { _LastTemperature = value; }
            }
            [Column(Name = "LastBloodPressureS", DataType = "String")]
            public String LastBloodPressureS
            {
                get { return _LastBloodPressureS; }
                set { _LastBloodPressureS = value; }
            }
            [Column(Name = "LastBloodPressureD", DataType = "String")]
            public String LastBloodPressureD
            {
                get { return _LastBloodPressureD; }
                set { _LastBloodPressureD = value; }
            }
            [Column(Name = "LastGlucoseLevel", DataType = "String")]
            public String LastGlucoseLevel
            {
                get { return _LastGlucoseLevel; }
                set { _LastGlucoseLevel = value; }
            }
            [Column(Name = "GCPatientCategory", DataType = "String")]
            public String GCPatientCategory
            {
                get { return _GCPatientCategory; }
                set { _GCPatientCategory = value; }
            }
            [Column(Name = "GCTriage", DataType = "String")]
            public String GCTriage
            {
                get { return _GCTriage; }
                set { _GCTriage = value; }
            }
        }
        #endregion

        #region vwPrescriptionOrderDt
        public partial class vwPrescriptionOrderDt
        {
            protected Int32 _PrescriptionOrderDetailID;
            protected Int32 _PrescriptionOrderID;
            protected Int32 _VisitID;
            protected Int32 _ParamedicID;
            protected String _ParamedicCode;
            protected String _ParamedicName;
            protected Int32 _ParentID;
            protected Int32 _SignaID;
            protected Decimal _ConversionFactor;
            protected String _SignaLabel;
            protected String _SignaName1;
            protected String _SignaName2;
            protected Boolean _IsRFlag;
            protected Boolean _IsCompound;
            protected Int32 _ItemID;
            protected String _ItemCode;
            protected String _GCItemUnit;
            protected String _GenericName;
            protected String _DrugName;
            protected String _CompoundDrugname;
            protected String _GCDrugForm;
            protected String _DrugForm;
            protected Decimal _Dose;
            protected String _GCDoseUnit;
            protected String _DoseUnit;
            protected String _GCDosingFrequency;
            protected String _DosingFrequency;
            protected Int16 _Frequency;
            protected Decimal _NumberOfDosage;
            protected String _NumberOfDosageInString;
            protected Decimal _CompoundQty;
            protected String _CompoundQtyInString;
            protected String _GCCompoundUnit;
            protected String _CompoundUnit;
            protected String _GCDosingUnit;
            protected String _DosingUnit;
            protected DateTime _StartDate;
            protected String _StartTime;
            protected Decimal _DosingDuration;
            protected String _GCRoute;
            protected String _GCCoenamRule;
            protected String _CoenamRuleLabel;
            protected String _Route;
            protected String _MedicationAdministration;
            protected String _MedicationPurpose;
            protected Decimal _DispenseQty;
            protected Decimal _TakenQty;
            protected Decimal _ResultQty;
            protected Decimal _ChargeQty;
            protected Boolean _IsUseSweetener;
            protected Boolean _OrderIsDeleted;
            protected String _CreatedByUserName;
            protected Int32 _LastUpdatedBy;
            protected String _LastUpdatedByUserName;
            protected DateTime _LastUpdatedDate;
            protected Int32 _EmbalaceID;
            protected String _EmbalaceCode;
            protected String _EmbalaceName;
            protected Decimal _EmbalaceQty;
            protected String _GCPrescriptionOrderStatus;
            protected String _PrescriptionOrderStatus;
            protected String _GCVoidReason;
            protected String _VoidReasonType;
            protected String _VoidReasonText;
            protected Boolean _IsAsRequired;
            protected Boolean _IsMorning;
            protected Boolean _IsNoon;
            protected Boolean _IsEvening;
            protected Boolean _IsNight;
            protected DateTime? _ExpiredDate;
            protected Boolean _IsUsingUDD;

            [Column(Name = "PrescriptionOrderDetailID", DataType = "Int32")]
            public Int32 PrescriptionOrderDetailID
            {
                get { return _PrescriptionOrderDetailID; }
                set { _PrescriptionOrderDetailID = value; }
            }
            [Column(Name = "PrescriptionOrderID", DataType = "Int32")]
            public Int32 PrescriptionOrderID
            {
                get { return _PrescriptionOrderID; }
                set { _PrescriptionOrderID = value; }
            }
            [Column(Name = "VisitID", DataType = "Int32")]
            public Int32 VisitID
            {
                get { return _VisitID; }
                set { _VisitID = value; }
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
            [Column(Name = "ParentID", DataType = "Int32")]
            public Int32 ParentID
            {
                get { return _ParentID; }
                set { _ParentID = value; }
            }
            [Column(Name = "SignaID", DataType = "Int32")]
            public Int32 SignaID
            {
                get { return _SignaID; }
                set { _SignaID = value; }
            }
            [Column(Name = "ConversionFactor", DataType = "Decimal")]
            public Decimal ConversionFactor
            {
                get { return _ConversionFactor; }
                set { _ConversionFactor = value; }
            }
            [Column(Name = "SignaLabel", DataType = "String")]
            public String SignaLabel
            {
                get { return _SignaLabel; }
                set { _SignaLabel = value; }
            }
            [Column(Name = "SignaName1", DataType = "String")]
            public String SignaName1
            {
                get { return _SignaName1; }
                set { _SignaName1 = value; }
            }
            [Column(Name = "SignaName2", DataType = "String")]
            public String SignaName2
            {
                get { return _SignaName2; }
                set { _SignaName2 = value; }
            }
            [Column(Name = "IsRFlag", DataType = "Boolean")]
            public Boolean IsRFlag
            {
                get { return _IsRFlag; }
                set { _IsRFlag = value; }
            }
            [Column(Name = "IsCompound", DataType = "Boolean")]
            public Boolean IsCompound
            {
                get { return _IsCompound; }
                set { _IsCompound = value; }
            }
            [Column(Name = "ItemID", DataType = "Int32")]
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            [Column(Name = "ItemCode", DataType = "String")]
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            [Column(Name = "GCItemUnit", DataType = "String")]
            public String GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
            [Column(Name = "GenericName", DataType = "String")]
            public String GenericName
            {
                get { return _GenericName; }
                set { _GenericName = value; }
            }
            [Column(Name = "DrugName", DataType = "String")]
            public String DrugName
            {
                get { return _DrugName; }
                set { _DrugName = value; }
            }
            [Column(Name = "CompoundDrugname", DataType = "String")]
            public String CompoundDrugname
            {
                get { return _CompoundDrugname; }
                set { _CompoundDrugname = value; }
            }
            [Column(Name = "GCDrugForm", DataType = "String")]
            public String GCDrugForm
            {
                get { return _GCDrugForm; }
                set { _GCDrugForm = value; }
            }
            [Column(Name = "DrugForm", DataType = "String")]
            public String DrugForm
            {
                get { return _DrugForm; }
                set { _DrugForm = value; }
            }
            [Column(Name = "Dose", DataType = "Decimal")]
            public Decimal Dose
            {
                get { return _Dose; }
                set { _Dose = value; }
            }
            [Column(Name = "GCDoseUnit", DataType = "String")]
            public String GCDoseUnit
            {
                get { return _GCDoseUnit; }
                set { _GCDoseUnit = value; }
            }
            [Column(Name = "DoseUnit", DataType = "String")]
            public String DoseUnit
            {
                get { return _DoseUnit; }
                set { _DoseUnit = value; }
            }
            [Column(Name = "GCDosingFrequency", DataType = "String")]
            public String GCDosingFrequency
            {
                get { return _GCDosingFrequency; }
                set { _GCDosingFrequency = value; }
            }
            [Column(Name = "DosingFrequency", DataType = "String")]
            public String DosingFrequency
            {
                get { return _DosingFrequency; }
                set { _DosingFrequency = value; }
            }
            [Column(Name = "Frequency", DataType = "Int16")]
            public Int16 Frequency
            {
                get { return _Frequency; }
                set { _Frequency = value; }
            }
            [Column(Name = "NumberOfDosage", DataType = "Decimal")]
            public Decimal NumberOfDosage
            {
                get { return _NumberOfDosage; }
                set { _NumberOfDosage = value; }
            }
            [Column(Name = "NumberOfDosageInString", DataType = "String")]
            public String NumberOfDosageInString
            {
                get { return _NumberOfDosageInString; }
                set { _NumberOfDosageInString = value; }
            }
            [Column(Name = "CompoundQty", DataType = "Decimal")]
            public Decimal CompoundQty
            {
                get { return _CompoundQty; }
                set { _CompoundQty = value; }
            }
            [Column(Name = "CompoundQtyInString", DataType = "String")]
            public String CompoundQtyInString
            {
                get { return _CompoundQtyInString; }
                set { _CompoundQtyInString = value; }
            }
            [Column(Name = "GCCompoundUnit", DataType = "String")]
            public String GCCompoundUnit
            {
                get { return _GCCompoundUnit; }
                set { _GCCompoundUnit = value; }
            }
            [Column(Name = "CompoundUnit", DataType = "String")]
            public String CompoundUnit
            {
                get { return _CompoundUnit; }
                set { _CompoundUnit = value; }
            }
            [Column(Name = "GCDosingUnit", DataType = "String")]
            public String GCDosingUnit
            {
                get { return _GCDosingUnit; }
                set { _GCDosingUnit = value; }
            }
            [Column(Name = "DosingUnit", DataType = "String")]
            public String DosingUnit
            {
                get { return _DosingUnit; }
                set { _DosingUnit = value; }
            }
            [Column(Name = "StartDate", DataType = "DateTime")]
            public DateTime StartDate
            {
                get { return _StartDate; }
                set { _StartDate = value; }
            }
            [Column(Name = "StartTime", DataType = "String")]
            public String StartTime
            {
                get { return _StartTime; }
                set { _StartTime = value; }
            }
            [Column(Name = "DosingDuration", DataType = "Decimal")]
            public Decimal DosingDuration
            {
                get { return _DosingDuration; }
                set { _DosingDuration = value; }
            }
            [Column(Name = "GCRoute", DataType = "String")]
            public String GCRoute
            {
                get { return _GCRoute; }
                set { _GCRoute = value; }
            }
            [Column(Name = "GCCoenamRule", DataType = "String")]
            public String GCCoenamRule
            {
                get { return _GCCoenamRule; }
                set { _GCCoenamRule = value; }
            }
            [Column(Name = "CoenamRuleLabel", DataType = "String")]
            public String CoenamRuleLabel
            {
                get { return _CoenamRuleLabel; }
                set { _CoenamRuleLabel = value; }
            }
            [Column(Name = "Route", DataType = "String")]
            public String Route
            {
                get { return _Route; }
                set { _Route = value; }
            }
            [Column(Name = "MedicationAdministration", DataType = "String")]
            public String MedicationAdministration
            {
                get { return _MedicationAdministration; }
                set { _MedicationAdministration = value; }
            }
            [Column(Name = "MedicationPurpose", DataType = "String")]
            public String MedicationPurpose
            {
                get { return _MedicationPurpose; }
                set { _MedicationPurpose = value; }
            }
            [Column(Name = "DispenseQty", DataType = "Decimal")]
            public Decimal DispenseQty
            {
                get { return _DispenseQty; }
                set { _DispenseQty = value; }
            }
            [Column(Name = "TakenQty", DataType = "Decimal")]
            public Decimal TakenQty
            {
                get { return _TakenQty; }
                set { _TakenQty = value; }
            }
            [Column(Name = "ResultQty", DataType = "Decimal")]
            public Decimal ResultQty
            {
                get { return _ResultQty; }
                set { _ResultQty = value; }
            }
            [Column(Name = "ChargeQty", DataType = "Decimal")]
            public Decimal ChargeQty
            {
                get { return _ChargeQty; }
                set { _ChargeQty = value; }
            }
            [Column(Name = "IsUseSweetener", DataType = "Boolean")]
            public Boolean IsUseSweetener
            {
                get { return _IsUseSweetener; }
                set { _IsUseSweetener = value; }
            }
            [Column(Name = "OrderIsDeleted", DataType = "Boolean")]
            public Boolean OrderIsDeleted
            {
                get { return _OrderIsDeleted; }
                set { _OrderIsDeleted = value; }
            }
            [Column(Name = "CreatedByUserName", DataType = "String")]
            public String CreatedByUserName
            {
                get { return _CreatedByUserName; }
                set { _CreatedByUserName = value; }
            }
            [Column(Name = "LastUpdatedBy", DataType = "Int32")]
            public Int32 LastUpdatedBy
            {
                get { return _LastUpdatedBy; }
                set { _LastUpdatedBy = value; }
            }
            [Column(Name = "LastUpdatedByUserName", DataType = "String")]
            public String LastUpdatedByUserName
            {
                get { return _LastUpdatedByUserName; }
                set { _LastUpdatedByUserName = value; }
            }
            [Column(Name = "LastUpdatedDate", DataType = "DateTime")]
            public DateTime LastUpdatedDate
            {
                get { return _LastUpdatedDate; }
                set { _LastUpdatedDate = value; }
            }
            [Column(Name = "EmbalaceID", DataType = "Int32")]
            public Int32 EmbalaceID
            {
                get { return _EmbalaceID; }
                set { _EmbalaceID = value; }
            }
            [Column(Name = "EmbalaceCode", DataType = "String")]
            public String EmbalaceCode
            {
                get { return _EmbalaceCode; }
                set { _EmbalaceCode = value; }
            }
            [Column(Name = "EmbalaceName", DataType = "String")]
            public String EmbalaceName
            {
                get { return _EmbalaceName; }
                set { _EmbalaceName = value; }
            }
            [Column(Name = "EmbalaceQty", DataType = "Decimal")]
            public Decimal EmbalaceQty
            {
                get { return _EmbalaceQty; }
                set { _EmbalaceQty = value; }
            }
            [Column(Name = "GCPrescriptionOrderStatus", DataType = "String")]
            public String GCPrescriptionOrderStatus
            {
                get { return _GCPrescriptionOrderStatus; }
                set { _GCPrescriptionOrderStatus = value; }
            }
            [Column(Name = "PrescriptionOrderStatus", DataType = "String")]
            public String PrescriptionOrderStatus
            {
                get { return _PrescriptionOrderStatus; }
                set { _PrescriptionOrderStatus = value; }
            }
            [Column(Name = "GCVoidReason", DataType = "String")]
            public String GCVoidReason
            {
                get { return _GCVoidReason; }
                set { _GCVoidReason = value; }
            }
            [Column(Name = "VoidReasonType", DataType = "String")]
            public String VoidReasonType
            {
                get { return _VoidReasonType; }
                set { _VoidReasonType = value; }
            }
            [Column(Name = "VoidReasonText", DataType = "String")]
            public String VoidReasonText
            {
                get { return _VoidReasonText; }
                set { _VoidReasonText = value; }
            }
            [Column(Name = "IsAsRequired", DataType = "Boolean")]
            public Boolean IsAsRequired
            {
                get { return _IsAsRequired; }
                set { _IsAsRequired = value; }
            }
            [Column(Name = "IsMorning", DataType = "Boolean")]
            public Boolean IsMorning
            {
                get { return _IsMorning; }
                set { _IsMorning = value; }
            }
            [Column(Name = "IsNoon", DataType = "Boolean")]
            public Boolean IsNoon
            {
                get { return _IsNoon; }
                set { _IsNoon = value; }
            }
            [Column(Name = "IsEvening", DataType = "Boolean")]
            public Boolean IsEvening
            {
                get { return _IsEvening; }
                set { _IsEvening = value; }
            }
            [Column(Name = "IsNight", DataType = "Boolean")]
            public Boolean IsNight
            {
                get { return _IsNight; }
                set { _IsNight = value; }
            }
            [Column(Name = "ExpiredDate", DataType = "DateTime", IsNullable = true)]
            public DateTime? ExpiredDate
            {
                get { return _ExpiredDate; }
                set { _ExpiredDate = value; }
            }
            [Column(Name = "IsUsingUDD", DataType = "Boolean")]
            public Boolean IsUsingUDD
            {
                get { return _IsUsingUDD; }
                set { _IsUsingUDD = value; }
            }

            public string cfItemName
            {
                get
                {
                    string itemName = _DrugName;
                    if (_IsCompound)
                    {
                        itemName = _CompoundDrugname;
                    }
                    return itemName;
                }
            }

            public String cfMedicationLine
            {
                get
                {
                    StringBuilder result = new StringBuilder();
                    if (!_IsCompound)
                    {
                        if (_DrugName != "")
                            result.Append(_DrugName).Append(" ");
                        if (_GenericName != "")
                            result.Append(string.Format("( {0} )", _GenericName));
                        result.Append(String.Format(" - {0} № {1}", _DrugForm, Function.ToRoman(Convert.ToInt32(_DispenseQty))));
                    }
                    else
                    {
                        if (_CompoundDrugname != "")
                            result.Append(_CompoundDrugname).Append(" - ");
                        result.Append(_DosingUnit);
                    }
                    return result.ToString();
                }
            }

            public String cfMedicationLine2
            {
                get
                {
                    StringBuilder result = new StringBuilder();
                    if (!_IsCompound)
                    {
                        if (_DrugName != "")
                            result.Append(_DrugName).Append(" ");
                        result.Append(String.Format(" - {0}", _DrugForm));
                    }
                    else
                    {
                        if (_CompoundDrugname != "")
                            result.Append(_CompoundDrugname).Append(" - ");
                        result.Append(_DosingUnit);
                    }
                    return result.ToString();
                }
            }

            public String cfIsRFlag
            {
                get
                {
                    if (_IsRFlag)
                        return "R/";
                    else
                        return "";
                }
            }
            public String cfSignaRule
            {
                get
                {
                    string result = string.Empty;
                    if (!String.IsNullOrEmpty(_SignaName1))
                    {
                        result = _SignaName1;
                    }
                    else
                    {
                        if (!_IsAsRequired)
                        {
                            string consumeMethod = "";
                            switch (_GCDosingFrequency)
                            {
                                case Helper.Constant.StandardCode.DosingFrequency.DAY:
                                    consumeMethod = string.Format("{0} x sehari {1} {2}", _Frequency.ToString("G29"), _NumberOfDosage.ToString("G29"), _DosingUnit);
                                    break;
                                case Helper.Constant.StandardCode.DosingFrequency.WEEK:
                                    consumeMethod = string.Format("{0} x seminggu {1} {2}", _Frequency.ToString("G29"), _NumberOfDosage.ToString("G29"), _DosingUnit);
                                    break;
                                case Helper.Constant.StandardCode.DosingFrequency.HOUR:
                                    consumeMethod = string.Format("setiap {0} jam {1} {2}", _Frequency.ToString("G29"), _NumberOfDosage.ToString("G29"), _DosingUnit);
                                    break;
                                default:
                                    break;
                            }
                            result = string.Format("{0}", consumeMethod);
                        }
                        else
                        {
                            result = "Jika diperlukan";
                        }
                    }
                    return result;
                }
            }

            public String cfNumberOfDosage
            {
                get
                {
                    return _NumberOfDosage.ToString("G29");
                }
            }

            public String cfDoseFrequency
            {
                get
                {
                    StringBuilder result = new StringBuilder();
                    if (!_IsAsRequired)
                    {
                        if (_GCDosingFrequency == Constant.StandardCode.DosingFrequency.HOUR)
                        {
                            result.Append("Every ").Append(_Frequency).Append(" hour(s)");
                        }
                        else
                        {
                            string sFrequency = "";
                            if (_Frequency == 1)
                                sFrequency = "Once";
                            else if (_Frequency == 2)
                                sFrequency = "Twice";
                            else
                                sFrequency = String.Format("{0} times", _Frequency);

                            result.Append(sFrequency);
                            if (_GCDosingFrequency == Constant.StandardCode.DosingFrequency.DAY)
                                result.Append(" per day");
                            else
                                result.Append(" per week");
                        }
                    }
                    else
                    {
                        result.Append(" as required");
                    }
                    return result.ToString();
                }
            }

            public String cfStartDate
            {
                get
                {
                    return _StartDate.ToString(Constant.FormatString.DATE_PICKER_FORMAT);
                }
            }

            public Boolean cfIsEditable
            {
                get
                {
                    if (_GCPrescriptionOrderStatus == Constant.StandardCode.OrderStatus.OPEN || _GCPrescriptionOrderStatus == Constant.StandardCode.OrderStatus.RECEIVED)
                        return true;
                    else
                        return false;
                }
            }
        }
        #endregion

        #region ItemProductQuickPick
        [Serializable]
        public class ItemProductQuickPick
        {
            private Int32 _ID;
            private Int32 _LocationID;
            private Int32 _ItemID;
            private String _ItemCode;
            private String _ItemName1;
            private String _GCItemType;
            private String _GCItemUnit;
            private String _ItemUnit;
            private Int32 _ItemGroupID;
            private Decimal _QuantityMIN;
            private Decimal _QuantityMAX;
            private Decimal _QuantityBEGIN;
            private Decimal _QuantityIN;
            private Decimal _QuantityOUT;
            private Decimal _QuantityEND;
            private Decimal _TotalQtyOnHand;
            private Boolean _IsDeleted;

            [Column(Name = "ID", DataType = "Int32")]
            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            [Column(Name = "LocationID", DataType = "Int32")]
            public Int32 LocationID
            {
                get { return _LocationID; }
                set { _LocationID = value; }
            }
            [Column(Name = "ItemID", DataType = "Int32")]
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            [Column(Name = "ItemCode", DataType = "String")]
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            [Column(Name = "ItemName1", DataType = "String")]
            public String ItemName1
            {
                get { return _ItemName1; }
                set { _ItemName1 = value; }
            }
            [Column(Name = "GCItemType", DataType = "String")]
            public String GCItemType
            {
                get { return _GCItemType; }
                set { _GCItemType = value; }
            }
            [Column(Name = "GCItemUnit", DataType = "String")]
            public String GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
            [Column(Name = "ItemUnit", DataType = "String")]
            public String ItemUnit
            {
                get { return _ItemUnit; }
                set { _ItemUnit = value; }
            }
            [Column(Name = "ItemGroupID", DataType = "Int32")]
            public Int32 ItemGroupID
            {
                get { return _ItemGroupID; }
                set { _ItemGroupID = value; }
            }
            [Column(Name = "QuantityMIN", DataType = "Decimal")]
            public Decimal QuantityMIN
            {
                get { return _QuantityMIN; }
                set { _QuantityMIN = value; }
            }
            [Column(Name = "QuantityMAX", DataType = "Decimal")]
            public Decimal QuantityMAX
            {
                get { return _QuantityMAX; }
                set { _QuantityMAX = value; }
            }
            [Column(Name = "QuantityBEGIN", DataType = "Decimal")]
            public Decimal QuantityBEGIN
            {
                get { return _QuantityBEGIN; }
                set { _QuantityBEGIN = value; }
            }
            [Column(Name = "QuantityIN", DataType = "Decimal")]
            public Decimal QuantityIN
            {
                get { return _QuantityIN; }
                set { _QuantityIN = value; }
            }
            [Column(Name = "QuantityOUT", DataType = "Decimal")]
            public Decimal QuantityOUT
            {
                get { return _QuantityOUT; }
                set { _QuantityOUT = value; }
            }
            [Column(Name = "QuantityEND", DataType = "Decimal")]
            public Decimal QuantityEND
            {
                get { return _QuantityEND; }
                set { _QuantityEND = value; }
            }
            [Column(Name = "TotalQtyOnHand", DataType = "Decimal")]
            public Decimal TotalQtyOnHand
            {
                get { return _TotalQtyOnHand; }
                set { _TotalQtyOnHand = value; }
            }
            [Column(Name = "IsDeleted", DataType = "Boolean")]
            public Boolean IsDeleted
            {
                get { return _IsDeleted; }
                set { _IsDeleted = value; }
            }
        }
        #endregion

        #region vwItemBalanceQuickPick
        [Serializable]
        public partial class vwItemBalanceQuickPick
        {
            private Int32 _ID;
            private Int32 _LocationID;
            private Int32 _ItemID;
            private String _ItemCode;
            private String _ItemName1;
            private String _GenericName;
            private String _GCItemType;
            private String _GCItemUnit;
            private String _ItemUnit;
            private Int32 _ItemGroupID;
            private Decimal _QuantityMIN;
            private Decimal _QuantityMAX;
            private Decimal _QuantityBEGIN;
            private Decimal _QuantityIN;
            private Decimal _QuantityOUT;
            private Decimal _QuantityEND;
            private Decimal _TotalQtyOnHand;
            private Boolean _IsDeleted;
            private String _GCMedicationRoute;

            [Column(Name = "ID", DataType = "Int32")]
            public Int32 ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            [Column(Name = "LocationID", DataType = "Int32")]
            public Int32 LocationID
            {
                get { return _LocationID; }
                set { _LocationID = value; }
            }
            [Column(Name = "ItemID", DataType = "Int32")]
            public Int32 ItemID
            {
                get { return _ItemID; }
                set { _ItemID = value; }
            }
            [Column(Name = "ItemCode", DataType = "String")]
            public String ItemCode
            {
                get { return _ItemCode; }
                set { _ItemCode = value; }
            }
            [Column(Name = "ItemName1", DataType = "String")]
            public String ItemName1
            {
                get { return _ItemName1; }
                set { _ItemName1 = value; }
            }
            [Column(Name = "GenericName", DataType = "String")]
            public String GenericName
            {
                get { return _GenericName; }
                set { _GenericName = value; }
            }
            [Column(Name = "GCItemType", DataType = "String")]
            public String GCItemType
            {
                get { return _GCItemType; }
                set { _GCItemType = value; }
            }
            [Column(Name = "GCItemUnit", DataType = "String")]
            public String GCItemUnit
            {
                get { return _GCItemUnit; }
                set { _GCItemUnit = value; }
            }
            [Column(Name = "ItemUnit", DataType = "String")]
            public String ItemUnit
            {
                get { return _ItemUnit; }
                set { _ItemUnit = value; }
            }
            [Column(Name = "ItemGroupID", DataType = "Int32")]
            public Int32 ItemGroupID
            {
                get { return _ItemGroupID; }
                set { _ItemGroupID = value; }
            }
            [Column(Name = "QuantityMIN", DataType = "Decimal")]
            public Decimal QuantityMIN
            {
                get { return _QuantityMIN; }
                set { _QuantityMIN = value; }
            }
            [Column(Name = "QuantityMAX", DataType = "Decimal")]
            public Decimal QuantityMAX
            {
                get { return _QuantityMAX; }
                set { _QuantityMAX = value; }
            }
            [Column(Name = "QuantityBEGIN", DataType = "Decimal")]
            public Decimal QuantityBEGIN
            {
                get { return _QuantityBEGIN; }
                set { _QuantityBEGIN = value; }
            }
            [Column(Name = "QuantityIN", DataType = "Decimal")]
            public Decimal QuantityIN
            {
                get { return _QuantityIN; }
                set { _QuantityIN = value; }
            }
            [Column(Name = "QuantityOUT", DataType = "Decimal")]
            public Decimal QuantityOUT
            {
                get { return _QuantityOUT; }
                set { _QuantityOUT = value; }
            }
            [Column(Name = "QuantityEND", DataType = "Decimal")]
            public Decimal QuantityEND
            {
                get { return _QuantityEND; }
                set { _QuantityEND = value; }
            }
            [Column(Name = "TotalQtyOnHand", DataType = "Decimal")]
            public Decimal TotalQtyOnHand
            {
                get { return _TotalQtyOnHand; }
                set { _TotalQtyOnHand = value; }
            }
            [Column(Name = "IsDeleted", DataType = "Boolean")]
            public Boolean IsDeleted
            {
                get { return _IsDeleted; }
                set { _IsDeleted = value; }
            }
            [Column(Name = "GCMedicationRoute", DataType = "String")]
            public String GCMedicationRoute
            {
                get { return _GCMedicationRoute; }
                set { _GCMedicationRoute = value; }
            }
        }
        #endregion
    }
}
