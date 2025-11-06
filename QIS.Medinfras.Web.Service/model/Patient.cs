using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedinfrasAPI.Models
{
    public class PatientData : Person
    {
        public int MRN { get; set; }
        public string MedicalNo { get; set; }
    }
}