using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    #region DTO Models
    public class procedure
    {
        public string procedureCode { get; set; }
        public string procedureName { get; set; }
        public string modalityCode { get; set; }
        public decimal procedureFee { get; set; }
    }

    public class referringPhysician
    {
        public string refPhyCode { get; set; }
        public string refPhyName { get; set; }
    }

    public class readingPhysician
    {
        public string radStaffCode { get; set; }
        public string radStaffName { get; set; }
    }

    public class patient
    {
        public string patientID { get; set; }
        public string mrn { get; set; }
        public string patientName { get; set; }
        public string dateOfBirth { get; set; }
        public string address { get; set; }
        public string sex { get; set; }
        public string size { get; set; }
        public string weight { get; set; }
        public string maritalStatus { get; set; }
    }

    public class TestOrderDtDTO
    {
        public procedure procedure { get; set; }
        public List<readingPhysician> readingPhysician { get; set; }
    }

    public class TestOrderDTO
    {
        public string placerOrderNumber { get; set; }
        public string visitNumber { get; set; }
        public string pointOfCare { get; set; }
        public string room { get; set; }
        public string bed { get; set; }
        public string orderDateTime { get; set; }
        public string imagingOrderPriority { get; set; }
        public string reportingPriority { get; set; }
        public List<TestOrderDtDTO> orderDetail { get; set; }
        public List<referringPhysician> referringPhysician { get; set; }
        public patient patient { get; set; }
    }

    public class APIResponse
    {
        public string Status { get; set; }
        public string Remark { get; set; }
        public string Data { get; set; }
    }

    public class MedinfrasAPIResponse
    {
        public string Status { get; set; }
        public string Remarks { get; set; }
        public string Data { get; set; }
    }

    public class GetTokenFujifilm
    {
        public string noro { get; set; }
        public string token { get; set; }
        public string url { get; set; }
    }
    #endregion
}
