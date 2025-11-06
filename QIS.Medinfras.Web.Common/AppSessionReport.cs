using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class AppSessionReport
    {
        public String HealthcareID { get; set; }
        public Int32 ParamedicID { get; set; }
        public Int32 UserID { get; set; }
        public String UserName { get; set; }
        public String UserFullName { get; set; }
        public String ReportFooterPrintedByInfo { get; set; }
    }
}
