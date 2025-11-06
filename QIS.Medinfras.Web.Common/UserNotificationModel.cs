using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.Web.Common
{
    public class UserNotification
    {
        public int UserID { get; set; }
        public int TotalUnReadMessage { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}
