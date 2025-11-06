using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class IHSModel
    {
        #region Token
        public class TokenResponse
        {
            public string refresh_token_expires_in { get; set; }
            public string api_product_list { get; set; }
            public string[] api_product_list_json { get; set; }
            public string organization_name { get; set; }
            public string developeremail { get; set; }
            public string token_type { get; set; }
            public string issued_at { get; set; }
            public string client_id { get; set; }
            public string access_token { get; set; }
            public string application_name { get; set; }
            public string scope { get; set; }
            public string expires_in { get; set; }
            public string refresh_count { get; set; }
            public string status { get; set; }
        }
        #endregion

        #region Parameter Base Model
        public class Reference
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        public class CodeReference
        {
            public string system { get; set; }
            public string code { get; set; }
            public string display { get; set; }
        }

        public class ResourceReference1
        {
            public string reference { get; set; }
        }

        public class VersionUpdatedInfo
        {
            public string lastUpdated { get; set; }
            public string versionId { get; set; }
        }

        #endregion

        public class Coding
        {
            public string code { get; set; }
            public string display { get; set; }
            public string system { get; set; }
        }

        public class Identifier
        {
            public string system { get; set; }
            public string use { get; set; }
            public string value { get; set; }
        }

        public class Subject
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        #region Organization
        public class OrganizationResourceBody
        {
            public string resourceType { get; set; }
            public bool active { get; set; }
            public List<Identifier> identifier { get; set; }
            public List<Type> type { get; set; }
            public string name { get; set; }
            public List<Telecom> telecom { get; set; }
            public List<Address> address { get; set; }
            public PartOf partOf { get; set; }
        }

        public class PartOf
        {
            public string reference { get; set; }
        }

        public class OrganizationResourceResponse
        {
            public string id { get; set; }
        }
        #endregion

        #region Location

        public class LocationResourceBody
        {
            public string resourceType { get; set; }
            public List<Identifier> identifier { get; set; }
            public string status { get; set; }
            public string name { get; set; }
            public string description { get; set; }
            public string mode { get; set; }
            public List<Telecom> telecom { get; set; }
            public Address address { get; set; }
            public PhysicalType physicalType { get; set; }
            public Position position { get; set; }
            public ManagingOrganization managingOrganization { get; set; }
        }

        public class Address
        {
            public string use { get; set; }
            public List<string> line { get; set; }
            public string city { get; set; }
            public string postalCode { get; set; }
            public string country { get; set; }
            public Extension[] extension { get; set; }
        }

        public class Extension
        {
            public string url { get; set; }
            public Extension1[] extension { get; set; }
        }

        public class Extension1
        {
            public string url { get; set; }
            public string valueCode { get; set; }
        }

        public class PhysicalType
        {
            public List<Coding> coding { get; set; }
        }

        public class Position
        {
            public double longitude { get; set; }
            public double latitude { get; set; }
            public int altitude { get; set; }
        }

        public class ManagingOrganization
        {
            public string reference { get; set; }
        }

        public class Telecom
        {
            public string system { get; set; }
            public string value { get; set; }
            public string use { get; set; }
        }

        public class LocationResourceResponse
        {
            public string id { get; set; }
        }

        #endregion

        #region Patient

        public class Communication
        {
            public Language language { get; set; }
            public bool preferred { get; set; }
        }

        public class Entry
        {
            public string fullUrl { get; set; }
            public Resource resource { get; set; }
            public Search search { get; set; }
        }

        public class Language
        {
            public List<Coding> coding { get; set; }
            public string text { get; set; }
        }

        public class Meta
        {
            public DateTime lastUpdated { get; set; }
            public List<string> profile { get; set; }
            public string versionId { get; set; }
        }

        public class Name
        {
            public string text { get; set; }
            public string use { get; set; }
        }

        public class Resource
        {
            public bool active { get; set; }
            public List<Communication> communication { get; set; }
            public bool deceasedBoolean { get; set; }
            public string gender { get; set; }
            public string id { get; set; }
            public List<Identifier> identifier { get; set; }
            public Meta meta { get; set; }
            public bool multipleBirthBoolean { get; set; }
            public List<Name> name { get; set; }
            public string resourceType { get; set; }
        }

        public class Search
        {
            public string mode { get; set; }
        }

        public class PatientResouceResponse
        {
            public List<Entry> entry { get; set; }
            public string resourceType { get; set; }
            public int total { get; set; }
            public string type { get; set; }
        }
        #endregion

        #region Encounter
        public class Class
        {
            public string system { get; set; }
            public string code { get; set; }
            public string display { get; set; }
        }

        public class EncounterIdentifier
        {
            public string system { get; set; }
            public string value { get; set; }
        }

        public class Individual
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        public class Location
        {
            public SubLocation location { get; set; }
        }

        public class SubLocation
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        public class Participant
        {
            public List<Type> type { get; set; }
            public Individual individual { get; set; }
        }

        public class Period
        {
            public string start { get; set; }
            public string end { get; set; }
        }

        public class EncounterPayLoad
        {
            public string resourceType { get; set; }
            //public string id { get; set; }
            public List<EncounterIdentifier> identifier { get; set; }
            public string status { get; set; }
            public Class @class { get; set; }
            public Subject subject { get; set; }
            public List<Participant> participant { get; set; }
            public Period period { get; set; }
            public List<Location> location { get; set; }
            public List<Diagnosis> diagnosis { get; set; }
            public List<StatusHistory> statusHistory { get; set; }
            public ServiceProvider serviceProvider { get; set; }

        }

        public class PutEncounterPayLoad
        {
            public string resourceType { get; set; }
            public string id { get; set; }
            public List<EncounterIdentifier> identifier { get; set; }
            public string status { get; set; }
            public Class @class { get; set; }
            public Subject subject { get; set; }
            public List<Participant> participant { get; set; }
            public Period period { get; set; }
            public List<Location> location { get; set; }
            public List<Diagnosis> diagnosis { get; set; }
            public List<StatusHistory> statusHistory { get; set; }
            public ServiceProvider serviceProvider { get; set; }
        }

        public class ServiceProvider
        {
            public string reference { get; set; }
        }

        public class StatusHistory
        {
            public string status { get; set; }
            public Period period { get; set; }
        }

        public class Type
        {
            public List<Coding> coding { get; set; }
        }

        public class Condition
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        public class Use
        {
            public List<Coding> coding { get; set; }
        }

        public class Diagnosis
        {
            public Condition condition { get; set; }
            public Use use { get; set; }
            public int rank { get; set; }
        }

        public class PostEncounterResponse
        {
            public Class @class { get; set; }
            public string id { get; set; }
            public List<Identifier> identifier { get; set; }
            public List<Location> location { get; set; }
            public Meta meta { get; set; }
            public List<Participant> participant { get; set; }
            public Period period { get; set; }
            public string resourceType { get; set; }
            public ServiceProvider serviceProvider { get; set; }
            public string status { get; set; }
            public List<StatusHistory> statusHistory { get; set; }
            public Subject subject { get; set; }
        }

        public class PutEncounterResponse
        {
            public Class @class { get; set; }
            public List<Diagnosis> diagnosis { get; set; }
            public string id { get; set; }
            public List<Identifier> identifier { get; set; }
            public List<Location> location { get; set; }
            public Meta meta { get; set; }
            public List<Participant> participant { get; set; }
            public Period period { get; set; }
            public string resourceType { get; set; }
            public ServiceProvider serviceProvider { get; set; }
            public string status { get; set; }
            public List<StatusHistory> statusHistory { get; set; }
            public Subject subject { get; set; }
        }

        #region Encounter History List
        public class EncounterResource
        {
            public Class @class { get; set; }
            public List<Diagnosis> diagnosis { get; set; }
            public string id { get; set; }
            public List<EncounterIdentifier> identifier { get; set; }
            public List<Location> location { get; set; }
            public Meta meta { get; set; }
            public List<Participant> participant { get; set; }
            public Period period { get; set; }
            public string resourceType { get; set; }
            public ServiceProvider serviceProvider { get; set; }
            public string status { get; set; }
            public List<StatusHistory> statusHistory { get; set; }
            public Subject subject { get; set; }
        }

        public class GetEncounterListResponse
        {
            public List<EncounterHistory> entry { get; set; }
            public string resourceType { get; set; }
            public int total { get; set; }
            public string type { get; set; }
        }

        public class EncounterHistory
        {
            public string fullUrl { get; set; }
            public EncounterResource resource { get; set; }
            public Search search { get; set; }
        }

        #endregion
        #endregion

        #region Condition
        public class ClinicalStatus
        {
            public List<Coding> coding { get; set; }
        }

        public class Category
        {
            public List<Coding> coding { get; set; }
        }

        public class Code
        {
            public List<Coding> coding { get; set; }
        }

        public class Encounter
        {
            public string reference { get; set; }
            public string display { get; set; }
        }

        public class ConditionPayLoad
        {
            public string resourceType { get; set; }
            public string id { get; set; }  
            public ClinicalStatus clinicalStatus { get; set; }
            public List<Category> category { get; set; }
            public Code code { get; set; }
            public Subject subject { get; set; }
            public Encounter encounter { get; set; }
        }
        #endregion

        #region Immunization
        public class VaccineCode
        {
            public List<CodeReference> coding { get; set; }
        }
        public class VaccineRoute
        {
            public List<CodeReference> coding { get; set; }
        }
        public class VaccineDoseQuantity
        {
            public int value { get; set; }
            public string unit { get; set; }
            public string system { get; set; }
            public string code { get; set; }
        }
        public class VaccinePerformerFunction
        {
            public List<CodeReference> coding { get; set; }
        }
        public class ExternalVaccinePerformerFunction
        {
            public List<CodeReference> coding { get; set; }
            public string text { get; set; }
        }
        public class VaccinePerformer
        {
            public VaccinePerformerFunction function { get; set; }
            public ResourceReference1 actor { get; set; }
        }
        public class ExternalVaccinePerformer
        {
            public ExternalVaccinePerformerFunction function { get; set; }
            public ResourceReference1 actor { get; set; }
        }
        public class VaccineReasonCoding
        {
            public List<CodeReference> coding { get; set; }
        }
        public class VaccineProtocolApplied
        {
            public Int32 doseNumberPositiveInt { get; set; }
        }
        public class ReportOrigin
        {
            public List<CodeReference> coding { get; set; }
        }
        /// <summary>
        /// Imunisasi dilakukan secara internal (pada saat kunjungan) oleh Tenaga Kesehatan (Paramedic)
        /// </summary>
        public class PostImmunizationPayLoad1
        {
            public string resourceType { get; set; }
            public string status { get; set; }
            public VaccineCode vaccineCode { get; set; }
            public Reference patient { get; set; }
            public ResourceReference1 encounter { get; set; }
            public string occurrenceDateTime { get; set; }
            public string recorded { get; set; }
            public Boolean primarySource { get; set; }
            public Reference location { get; set; }
            public string lotNumber { get; set; }
            public VaccineRoute route { get; set; }
            public VaccineDoseQuantity doseQuantity { get; set; }
            public List<VaccinePerformer> performer { get; set; }
            public List<VaccineReasonCoding> reasonCode { get; set; }
            public List<VaccineProtocolApplied> protocolApplied { get; set; }
        }

        /// <summary>
        /// Imunisasi dilakukan secara external / dilaporkan oleh tenaga medis/kader
        /// </summary>
        public class PostImmunizationPayLoad2
        {
            public string resourceType { get; set; }
            public string status { get; set; }
            public VaccineCode vaccineCode { get; set; }
            public Reference patient { get; set; }
            public string occurrenceDateTime { get; set; }
            public string recorded { get; set; }
            public Boolean primarySource { get; set; }
            public ReportOrigin reportOrigin { get; set; }
            public List<ExternalVaccinePerformer> performer { get; set; }
            public List<VaccineReasonCoding> reasonCode { get; set; }
            public List<VaccineProtocolApplied> protocolApplied { get; set; }
        }

        public class PostImmunizationResponse1
        {
            public string resourceType { get; set; }
            public string status { get; set; }
            public string recorded { get; set; }
            public VaccineCode vaccineCode { get; set; }
            public VaccineRoute route { get; set; }
            public VaccineDoseQuantity doseQuantity { get; set; }
            public ResourceReference1 encounter { get; set; }
            public string id { get; set; }
            public bool primarySource { get; set; }
            public Reference location { get; set; }
            public string lotNumber { get; set; }
            public VersionUpdatedInfo meta { get; set; }
            public string occurenceDateTime { get; set; }
            public Reference patient { get; set; }
            public List<VaccinePerformer> performer { get; set; }
            public List<VaccineProtocolApplied> protocolApplied { get; set; }
            public List<VaccineReasonCoding> reasonCode { get; set; }
        }

        public class PostImmunizationResponse2
        {
            public string resourceType { get; set; }
            public string status { get; set; }
            public string recorded { get; set; }
            public VaccineCode vaccineCode { get; set; }
            public string id { get; set; }
            public string primarySource { get; set; }
            public ReportOrigin reportOrigin { get; set; }
            public VersionUpdatedInfo meta { get; set; }
            public string occurenceDateTime { get; set; }
            public Reference patient { get; set; }
            public List<ExternalVaccinePerformer> performer { get; set; }
            public List<VaccineProtocolApplied> protocolApplied { get; set; }
            public List<VaccineReasonCoding> reasonCode { get; set; }
        }
        #endregion

        #region MEDINFRAS CENTERBACK

        #region Medinfras API Centerback
        public class MedinfrasCenterbackPayload
        {
            public MedinfrasCenterback_OrganizationPayload Organization { get; set; }
            public MedinfrasCenterback_LocationPayload Location { get; set; }
            public MedinfrasCenterback_EncounterPayload Encounter { get; set; }
            public MedinfrasCenterback_ObservationPayload Observation { get; set; }
        }

        public class MedinfrasCenterback_OrganizationPayload
        {
            public string DepartmentID { get; set; }
            public string DepartmentName { get; set; }
        }

        public class MedinfrasCenterback_LocationPayload
        {
            public string ServiceUnitCode { get; set; }
            public string ServiceUnitName { get; set; }
            public string DepartmentID { get; set; }
        }

        public class MedinfrasCenterback_EncounterPayload
        {
            /// <summary>
            /// MessageType : CREATE, UPDATE
            /// </summary>
            public string MessageType { get; set; }
            public string RegistrationID { get; set; }
            public string RegistrationNo { get; set; }
            public string VisitID { get; set; }
        }

        public class MedinfrasCenterback_ObservationPayload
        {
            public string RegistrationNo { get; set; }
        }
        #endregion

        #endregion
    }
}
