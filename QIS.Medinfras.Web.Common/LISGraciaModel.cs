using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    #region Response Data
    public class LISResultResponse
    {
        public int HisPoolID { get; set; }
        public string BillingNomor { get; set; }
        public int LabHeaderID { get; set; }
        public string LabNomor { get; set; }
        public int LabID { get; set; }
        public string KelompokPx { get; set; }
        public string LabKode { get; set; }
        public string LabNama { get; set; }
        public string LabMetode { get; set; }
        public string LabHasil { get; set; }
        public string LabSatuan { get; set; }
        public string LabNilaiNormal { get; set; }
        public string LabKeterangan { get; set; }
        public string LISDate { get; set; }
        public string HISDate { get; set; }
        public int LabStatus { get; set; }
        public string Action { get; set; }
        public string SpecialOrder { get; set; }
        public string ValUserName { get; set; }
        public string Status { get; set; }
        public string IsMDT { get; set; }
        public string LabNoRm { get; set; }
        public string T_RefTestCode { get; set; }
    }
    #endregion
}