using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.Common
{
    #region Nalagenetics DTO Model

    #region Nalagenetics Report Data Information
    public class expert_guideline_symbol
    {
        public string guide_symbol { get; set; }
        public string guideline_link { get; set; }
    }
    public class drug_list
    {
        public object value { get; set; }
    }
    public class geno_type
    {
    }
    public class phenotype_summary
    {
        public string _id { get; set; }
        public string gene_symbol { get; set; }
        public string geno_type { get; set; }
        public string phenotype_text { get; set; }
        public string createdAt { get; set; }
        public string updatedAt { get; set; }
    }

    public class metabolism
    {
        public string _id { get; set; }
        public string pmid { get; set; }
        public string pmidLink { get; set; }
    }

    public class toxicity
    {
        public object value { get; set; }
    }

    public class pharmacokinetics
    {
        public string _id { get; set; }
        public string pmid { get; set; }
        public string pmidLink { get; set; }
    }

    public class safety
    {
        public object value { get; set; }
    }

    public class pmids
    {
        public string _id { get; set; }
        public List<metabolism> metabolism { get; set; }
        public List<toxicity> toxicity { get; set; }
        public List<pharmacokinetics> pharmacokinetics { get; set; }
        public List<safety> safety { get; set; }
    }

    public class usage
    {
        public string _id { get; set; }
        public string _usage_text { get; set; }
        public string _usage_alternative { get; set; }
    }

    public class NalageneticsReportData
    {
        public List<expert_guideline_symbol> expert_guideline_symbol { get; set; }
        public List<drug_list> drug_list { get; set; }
        public List<geno_type> geno_type { get; set; }
        public string rec_text { get; set; }
        public string rec_level { get; set; }
        public string rec_category { get; set; }
        public string implication_text { get; set; }
        public string regulatory_body_warning { get; set; }
        public string expert_guideline_text { get; set; }
        public string nala_score { get; set; }
        public string nala_score_v2 { get; set; }
        public string caveat_text { get; set; }
        public string caveat_type { get; set; }
        public List<phenotype_summary> phenotype_summary { get; set; }
        public List<pmids> pmdids { get; set; }
        public string version_id { get; set; }
        public List<usage> usage { get; set; }
        public string report_url { get; set; }
    }
    #endregion

    #region Response
    public class NalageneticsMessage 
    {
        public NalageneticsReportData current { get; set; }
        public string report_id { get; set; }
        public string sample_id { get; set; }
        public string drug_name { get; set; }
        public string atc_code { get; set; }
        public List<NalageneticsReportData> previous { get; set; }
        public string provider_name { get; set; }
    }

    public class NalageneticResponseInfo1
    {
        public string status { get; set; }
        public List<NalageneticsMessage> message { get; set; }
    }

    public class NalageneticResponse1
    {
        public NalageneticResponseInfo1 response { get; set; }
    }

    public class NalageneticOrderResponseInfo1
    {
        public bool success { get; set; }
        public List<string> errorMessages { get; set; }
    }

    public class NalageneticOrderResponse1
    {
        public NalageneticOrderResponseInfo1 data { get; set; }
    }
    #endregion

    #region POST Parameter
    public class NalageneticsReportParameter1
    {
        public string sample_id { get; set; }
    }

    #region pgxContext
    public class ReasonForOrderingPgxTest 
    {
        public string type { get; set; }
        public List<string> values { get; set; }
    }

    public class PgxContext
    {
        public List<ReasonForOrderingPgxTest> reasonForOrderingPgxTest { get; set; }
        public List<string> relevantClinicalInformation { get; set; }
        public string needGeneticsCounseling { get; set; }
    }
    #endregion

    public class PGxTestOrder
    {
        public string skuCode { get; set; }
        public string specimenType { get; set; }
        public string physicianEmail { get; set; }
        public string remarks { get; set; }
        public string sampleId { get; set; }
        public string consentForSampleSharing { get; set; }
        public PgxContext pgxContext { get; set; }
    }

    public class PGxTestPatient
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string gender { get; set; }
        public string birthDate { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string city { get; set; }
        public string race { get; set; }
        public string phone { get; set; }
        public string email { get; set; }
        public string postalCode { get; set; }
        public string acceptTermsConditionPrivacyPolicy { get; set; }
        public string nationality { get; set; }
        public string icNumber { get; set; }
        public string externalPatientId { get; set; }
    }

    public class NalageneticsOrderParameter1
    {
        public PGxTestOrder order { get; set; }
        public PGxTestPatient patient { get; set; }
    }

    #endregion
    #endregion
}