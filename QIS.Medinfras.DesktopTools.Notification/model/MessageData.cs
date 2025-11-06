using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.DesktopTools.Notification
{
    public class MessageData
    {
        public string Sender { get; set; }
        public string MessageID   { get; set; }
        public string MessageDate { get; set; }
        public string MessageTime { get; set; }
        public string MessageType { get; set; }
        public string MessageOriginalText { get; set; }
        public string MessageText { get; set; }
    }
}
        