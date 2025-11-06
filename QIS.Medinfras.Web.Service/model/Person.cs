using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MedinfrasAPI.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string PrefferedName { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string CityOfBirth { get; set; }
        public string MaritalStatus { get; set; }
        public string Religion { get; set; }
        public string HomeAddress { get; set; }
        public string HomeZipCode { get; set; }
        public string HomePhoneNo1 { get; set; }
        public string HomePhoneNo2 { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
    }
}