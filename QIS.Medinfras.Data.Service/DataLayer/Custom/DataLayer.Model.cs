using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Data.Service
{
    #region LastContentPrescriptionEntry
    public class LastContentPrescriptionEntry
    {
        public string ContentID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string FromDepartmentID { get; set; }
        public Int32 FromHealthcareServiceUnitID { get; set; }
        public string CustomerType { get; set; }
        public string Date { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string VariableDisplay { get; set; }
        public string PrescriptionType { get; set; }
    }
    #endregion

    #region LastContentFarmasiKlinis
    public class LastContentFarmasiKlinis
    {
        public string ContentID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string FromDepartmentID { get; set; }
        public Int32 FromHealthcareServiceUnitID { get; set; }
        public string CustomerType { get; set; }
        public string Date { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string VariableDisplay { get; set; }
        public string PrescriptionType { get; set; }
    }
    #endregion

    #region LastContentVisitList
    public class LastContentVisitListPH
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string RegistrationDate { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string MedicalNo { get; set; }
    }

    public class LastContentVisitListER
    {
        public string RegistrationDate { get; set; }
        public Int32 ParamedicID { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string MedicalNo { get; set; }
    }

    public class LastContentVisitListOP
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ClinicHealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public Int32 ParamedicIDPatientCall { get; set; }
        public string RegistrationDate { get; set; }
        public string RegistrationDate2 { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string QuickTextPatientCall { get; set; }
        public string QuickFilterExpressionPatientCall { get; set; }
        public string MedicalNo { get; set; }
    }

    public class LastContentVisitListIP
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string Status { get; set; }
        public string MedicalNo { get; set; }
    }

    public class LastContentVisitListMD
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ClinicHealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string RegistrationDate { get; set; }
        public string RegistrationDate2 { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string MedicalNo { get; set; }
    }

    public class LastContentFollowupPatientVisitList
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public Int32 ParamedicID { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string Status { get; set; }
        public string MedicalNo { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string DateFilter { get; set; }
    }

    public class LastContentVisitListMR
    {
        public string ContentID { get; set; }
        public string DepartmentID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string RegistrationStatus { get; set; }
        public string ProcessStatus { get; set; }
        public Boolean IsAllEmpty { get; set; }
        public Boolean IsChiefComplaintNull { get; set; }
        public Boolean IsPatientDiagnosisNull { get; set; }
        public Boolean IsPhysicianDischargeNull { get; set; }
        public Boolean IsDischargeDateNull { get; set; }
        public string DateFilter { get; set; }
        public string PatientCondition { get; set; }
    }

    public class LastContentVisitListMR2
    {
        public string DepartmentID { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
        public string RegistrationStatus { get; set; }
        public string ProcessStatus { get; set; }
        public Boolean IsAllEmpty { get; set; }
        public Boolean IsChiefComplaintNull { get; set; }
        public Boolean IsPatientDiagnosisNull { get; set; }
        public Boolean IsPhysicianDischargeNull { get; set; }
        public Boolean IsDischargeDateNull { get; set; }
        public string DateFilter { get; set; }
        public string PatientCondition { get; set; }
    }

    public class LastContentLaboratoryRealization
    {
        public string LabContentID { get; set; }
        public string LabPatientFrom { get; set; }
        public string LabDate { get; set; }
        public string LabQuickFilter { get; set; }
        public string LabQuickFilterExpression { get; set; }
        public string LabDisplayResult { get; set; }
    }

    public class LastContentImagingRealization
    {
        public string ImagingDate { get; set; }
    }

    public class LastContentImagingRealizationResult
    {
        public string ImagingDateFrom { get; set; }
        public string ImagingDateTo { get; set; }
        public string ImagingFromDepartmentID { get; set; }
        public string ImagingGCModality { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
    }

    public class LastContentEMRPatientListTrx
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string FromDepartmentID { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
    }

    public class LastContentMyPatientList
    {
        public string DepartmentID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string RegistrationDate { get; set; }
        public string QuickFilterExpression { get; set; }
    }

    public class LastContentPreviousPatientList
    {
        public string DepartmentID { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string RegistrationDate { get; set; }
        public string QuickFilterExpression { get; set; }
    }


    public class LastContentPreviousPatientList2
    {
        public string BarcodeNo { get; set; }
        public string DepartmentID { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string ServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string RealisationDate { get; set; }
        public string RealisationDateFrom { get; set; }
        public string RealisationDateTo { get; set; }
        public string QuickFilterExpression { get; set; }
        public string QuickText { get; set; }
        public Boolean IsIgnoreDate { get; set; }
        public Boolean IsIgnoreDateInpatient { get; set; }
        public Boolean IsIncludeOpenPatient { get; set; }
        public int LastPageIndex { get; set; }
    }

    public class LastPagingMR1
    {
        public Int32 PageID { get; set; }
    }

    public class LastPagingMR2
    {
        public Int32 PageID { get; set; }
    }

    public class LastPagingMR3
    {
        public Int32 PageID { get; set; }
    }
    public class LastPatientVisitMCUForm
    {
        public Int32 VisitID { get; set; }
    }
    #endregion
    #region LastContentListFinance

    public class LastContentEKlaimList
    {
        public string FilterNoSEP { get; set; }
        public string JenisRawat { get; set; }
        public bool AbaikanTanggalSEP { get; set; }
        public string TanggalSEP_dari { get; set; }
        public string TanggalSEP_sampai { get; set; }
        public bool AbaikanTanggalPulang { get; set; }
        public string TanggalPulang_dari { get; set; }
        public string TanggalPulang_sampai { get; set; }
        public string KelasSEP { get; set; }
        public string QuickText { get; set; }
        public string QuickFilterExpression { get; set; }
    }

    public class LastContentClaimDiagnoseProcedure
    {
        public string ContentID { get; set; }
    }

    public class LastContentClaimDiagnoseProcedureList
    {
        public string FromRegistrationDate { get; set; }
        public string ToRegistrationDate { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string DepartmentID { get; set; }
        public string FilterExpressionSearchView { get; set; }
        public string FilterExpressionQuickSearch { get; set; }
        public string RegistrationStatus { get; set; }
    }

    public class LastContentClaimDiagnoseProcedureListHaveDiag
    {
        public string FromRegistrationDate { get; set; }
        public string ToRegistrationDate { get; set; }
        public string HealthcareServiceUnitID { get; set; }
        public string ServiceUnitCode { get; set; }
        public string ServiceUnitName { get; set; }
        public string DepartmentID { get; set; }
        public string FilterExpressionSearchView { get; set; }
        public string FilterExpressionQuickSearch { get; set; }
        public string RegistrationStatus { get; set; }
    }

    #endregion
    #region Matrix
    public class ProceedEntity
    {
        private String _ID;
        private ProceedEntityStatus _Status;

        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public ProceedEntityStatus Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        public enum ProceedEntityStatus
        {
            Remove = 0,
            Add = 1
        }
    }

    public class CEntity
    {
        private string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }

    public class CMatrix
    {
        public bool IsChecked { get; set; }
        public object ID { get; set; }
        public string Name { get; set; }
        public string Name2 { get; set; }
        public string ToolTip { get; set; }
    }
    #endregion
    #region MedicalDiagnosticUserLogin
    public class MedicalDiagnosticUserLogin
    {
        public Int32 HealthcareServiceUnitID { get; set; }
        public MedicalDiagnosticType MedicalDiagnosticType { get; set; }
        public Int32 ImagingHealthcareServiceUnitID { get; set; }
        public Int32 LaboratoryHealthcareServiceUnitID { get; set; }
    }
    #endregion
    #region ParamedicMasterRevenueSharingProcess
    public class ParamedicMasterRevenueSharingProcess
    {
        public Int32 ParamedicID { get; set; }
        public DateTime PeriodeStart { get; set; }
        public DateTime PeriodeEnd { get; set; }
        public Int32 RevenueSharingID { get; set; }
    }
    #endregion
    #region PatientDetail
    public class PatientDetail
    {
        public Int32 MRN { get; set; }
        public String MedicalNo { get; set; }
        public String PatientName { get; set; }
    }
    #endregion
    #region RegisteredPatient
    public class RegisteredPatient
    {
        public Int32 MRN { get; set; }
        public String MedicalNo { get; set; }
        public String PatientName { get; set; }
        public String GCGender { get; set; }
        public String GCSex { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Int32 RegistrationID { get; set; }
        public String RegistrationNo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public String RegistrationTime { get; set; }
        public Int32 VisitID { get; set; }
        public DateTime VisitDate { get; set; }
        public String VisitTime { get; set; }
        public DateTime StartServiceDate { get; set; }
        public String StartServiceTime { get; set; }
        public DateTime DischargeDate { get; set; }
        public String DischargeTime { get; set; }
        public Int32 HealthcareServiceUnitID { get; set; }
        public String DepartmentID { get; set; }
        public String ServiceUnitName { get; set; }
        public String RoomCode { get; set; }
        public String BedCode { get; set; }
        public Int32 ParamedicID { get; set; }
        public String ParamedicCode { get; set; }
        public String ParamedicName { get; set; }
        public String SpecialtyID { get; set; }
        public Int32 ClassID { get; set; }
        public Int32 ChargeClassID { get; set; }
        public String GCRegistrationStatus { get; set; }
        public Boolean IsLockDown { get; set; }
        public Int32 LinkedRegistrationID { get; set; }
        public Int32 LinkedToRegistrationID { get; set; }
        public String GCCustomerType { get; set; }
        public Int32 BusinessPartnerID { get; set; }
        public Boolean IsBillingReopen { get; set; }
        public Boolean IsPlanDischarge { get; set; }
        public Boolean IsUsingImplant { get; set; }
        public DateTime LastAcuteInitialAssessmentDate { get; set; }
        public DateTime LastChronicInitialAssessmentDate { get; set; }
        public Boolean IsNeedRenewalAcuteInitialAssessment { get; set; }
        public Boolean IsNeedRenewalChronicInitialAssessment { get; set; }
        public String OpenFromModuleID { get; set; }
    }
    #endregion
    #region TimeOfDayInterval
    public class TimeOfDayInterval
    {
        public TimeOfDayInterval(TimeSpan Start, TimeSpan End)
        {
            this.Start = Start;
            this.End = End;
        }
        public TimeSpan Start { get; set; }
        public TimeSpan End { get; set; }
    }
    #endregion
    #region UserLogin
    public class UserLogin
    {
        public Int32 UserID { get; set; }
        public String UserName { get; set; }
        public String UserFullName { get; set; }
        public Int32? ParamedicID { get; set; }
        public String ParamedicName { get; set; }
        public Boolean IsSysAdmin { get; set; }
        public String SpecialtyID { get; set; }
        public String DepartmentID { get; set; }
        public String GCParamedicMasterType { get; set; }
        public String HealthcareID { get; set; }
        public String HealthcareName { get; set; }
        public String ModuleID { get; set; }
        public Boolean IsSpecialist { get; set; }
        public Boolean IsRMO { get; set; }
        public Boolean IsPrimaryNurse { get; set; }
        public String SSN { get; set; }
    }
    #endregion
    #region Variable
    public class Variable
    {
        private string _Code;
        private string _Value;

        public string Code
        {
            get { return _Code; }
            set { _Code = value; }
        }

        public string Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
    #endregion
    #region Words
    public class Words
    {
        public string Code { get; set; }
        public string Text { get; set; }
    }
    #endregion

    public class VitalSignIndicator
    {
        private Int32 _VitalSignID;
        private String _VitalSignCode;
        private String _VitalSignName;
        private String _VitalSignLabel;
        private String _GCValueType;
        private String _GCValueCode;
        private String _ValueUnit;
        private Decimal _MinNormalValue;
        private Decimal _MaxNormalValue;
        private Boolean _IsDeleted;
        private Int16 _DisplayOrder;
        private Boolean _IsNumericValue;
        private Boolean _IsAutoGenerated;
        private String _VitalSignType;

        public Int32 VitalSignID
        {
            get { return _VitalSignID; }
            set { _VitalSignID = value; }
        }
        public String VitalSignCode
        {
            get { return _VitalSignCode; }
            set { _VitalSignCode = value; }
        }
        public String VitalSignName
        {
            get { return _VitalSignName; }
            set { _VitalSignName = value; }
        }
        public String VitalSignLabel
        {
            get { return _VitalSignLabel; }
            set { _VitalSignLabel = value; }
        }
        public String GCValueType
        {
            get { return _GCValueType; }
            set { _GCValueType = value; }
        }
        public String GCValueCode
        {
            get { return _GCValueCode; }
            set { _GCValueCode = value; }
        }
        public String ValueUnit
        {
            get { return _ValueUnit; }
            set { _ValueUnit = value; }
        }
        public Decimal MinNormalValue
        {
            get { return _MinNormalValue; }
            set { _MinNormalValue = value; }
        }
        public Decimal MaxNormalValue
        {
            get { return _MaxNormalValue; }
            set { _MaxNormalValue = value; }
        }
        public Boolean IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }
        public Int16 DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        public Boolean IsNumericValue
        {
            get { return _IsNumericValue; }
            set { _IsNumericValue = value; }
        }
        public Boolean IsAutoGenerated
        {
            get { return _IsAutoGenerated; }
            set { _IsAutoGenerated = value; }
        }
        public String VitalSignType
        {
            get { return _VitalSignType; }
            set { _VitalSignType = value; }
        }
    }

    public enum MedicalDiagnosticType
    {
        Laboratory = 0,
        Imaging = 1,
        OtherMedicalDiagnostic = 2,
        Nutrition = 3,
        None = 4,
        Radiotheraphy = 5
    }

    public class CompactTestOrderDtInfo
    {
        public string ItemName1 { get; set; }
    }
}
