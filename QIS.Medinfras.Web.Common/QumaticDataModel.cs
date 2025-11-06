using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class QumaticResponse
    {
        public string success { get; set; }
        public string payload { get; set; }
    }

    public class QumaticBodyRequest
    {
        public string apiKey { get; set; }
        public QumaticBodyRequestNewApm payload { get; set; }
    }

    public class QumaticBodyRequestConfirmation
    {
        public string apiKey { get; set; }
        public QumaticBodyRequestApmConfirmation payload { get; set; }
    }

    public class QumaticBodyRequestApmConfirmation
    {
        public int AppointmentID { get; set; }
        public int RegistrationID { get; set; }
    }

    public class QumaticBodyRequestNewApm
    {
        public int AppointmentID { get; set; }
        public int Token { get; set; }
        public string Type { get; set; }
        public string Payment { get; set; }
        public QumaticPatientInfo Patient { get; set; }
        public QumaticPhysicianInfo Physician { get; set; }
    }

    public class QumaticPatientInfo
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
    }

    public class QumaticPhysicianInfo
    {
        public string ID { get; set; }
        public string ServiceUnit { get; set; }
        public QumaticScheduleInfo Schedule { get; set; }
    }

    public class QumaticScheduleInfo
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Floor { get; set; }
        public string Room { get; set; }
    }
}
