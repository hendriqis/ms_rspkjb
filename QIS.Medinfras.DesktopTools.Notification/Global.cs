using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QIS.Medinfras.DesktopTools.Notification
{
    public static class Global
    {
        private static MessageData lastMessage;

        public static MessageData LastMessage
        {
            get { return lastMessage; }
            set { lastMessage = value; }
        }

        #region Message Log
        private static List<MessageData> lstMessage = new List<MessageData>();

        public static List<MessageData> MessageList
        {
            get { return lstMessage; }
            set { lstMessage = value; }
        }

        #endregion
    }
}
