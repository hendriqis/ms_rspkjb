using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QIS.Medinfras.Web.Common.API.Model;

namespace QIS.Medinfras.Web.Common
{
    #region Wynacom DTO Model
    public class WynacomOrderHd
    {
        public int TransactionID { get; set; }
        public string TransactionNo { get; set; }
        public DateTime TransactionDateTime { get; set; }
        public bool  IsCITO { get; set; }
        public string OrderPhysicianCode { get; set; }
        public string OrderPhysicianName { get; set; }
        public string LIS_REG_NO { get; set; }
        public DateTime RETRIEVED_DT { get; set; }
        public string RETRIEVED_FLAG { get; set; }
    }

    public class WynacomOrderDt
    {
        public int TransactionID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string fractionCommCode { get; set; }
        public string fractionName { get; set; }
        public string ClinicianRemarks { get; set; }
    }

    public class WynacomOrderDTO
    {
        public PatientInfo PatientData { get; set; }
        public VisitInfo1 VisitData { get; set; }
        public string CreatedByUserID { get; set; }
        public string CreatedByUserName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdatedDate { get; set; }
        public WynacomOrderHd OrderHeaderData { get; set; }
        public List<WynacomOrderDt> OrderDetailList { get; set; }
    }
    #endregion

    #region Softmedix DTO Model

    #region Response
    public class SoftmedixResponse
    {
        public SoftmedixResponseDt response { get; set; }
    }

    public class SoftmedixResponseDt
    {
        public string code { get; set; }
        public string message { get; set; }
    }
    #endregion

    #region Order
    public class SoftmedixModel
    {
        public SoftmedixOrder order { get; set; }
    }

    public class SoftmedixOrder
    {
        public SoftmedixOrderMsh msh { get; set; }
        public SoftmedixOrderPid pid { get; set; }
        public SoftmedixOrderObr obr { get; set; }
    }

    public class SoftmedixOrderMsh
    {
        public string product { get; set; }
        public string version { get; set; }
        public string user_id { get; set; }
        public string key { get; set; }
    }

    public class SoftmedixOrderPid
    {
        public string pmrn { get; set; }
        public string pname { get; set; }
        public string sex { get; set; }
        public string birth_dt { get; set; }
        public string address { get; set; }
        public string no_tlp { get; set; }
    }

    public class SoftmedixOrderObr
    {
        public string order_control { get; set; }
        public string ptype { get; set; }
        public string reg_no { get; set; }
        public string order_lab { get; set; }
        public string provider_id { get; set; }
        public string provider_name { get; set; }
        public string order_date { get; set; }
        public string clinician_id { get; set; }
        public string clinician_name { get; set; }
        public string bangsal_id { get; set; }
        public string bangsal_name { get; set; }
        public string bed_id { get; set; }
        public string bed_name { get; set; }
        public string class_id { get; set; }
        public string class_name { get; set; }
        public string cito { get; set; }
        public string med_legal { get; set; }
        public string user_id { get; set; }
        public List<SoftmedixOrderObrDt> order_test { get; set; }
    }

    public class SoftmedixOrderObrDt
    {
        private string number;
        private string testCode;

        public SoftmedixOrderObrDt(string number, string testCode)
        {
            this.number = number;
            this.testCode = testCode;
        }

    }
    #endregion

    #region Update Patient Data Master
    public class SoftmedixPatientUpdate
    {
        public SoftmedixPatient pasien { get; set; }
    }

    public class SoftmedixPatient
    {
        public SoftmedixOrderMsh msh { get; set; }
        public SoftmedixOrderPid pid { get; set; }
        public SoftmedixOrderObr obt { get; set; }
    }
    #endregion

    #endregion
}