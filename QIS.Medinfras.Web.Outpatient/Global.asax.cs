using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using DevExpress.Web.ASPxClasses;
using QIS.Medinfras.Web.Common;
using System.IO;
using System.Web.Routing;
using Microsoft.AspNet.SignalR;

namespace QIS.Medinfras.Web.Outpatient
{
    public class Global : System.Web.HttpApplication
    {

        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            ASPxWebControl.CallbackError += new EventHandler(Application_Error);
            RouteTable.Routes.MapHubs();
                
        }

        void Application_End(object sender, EventArgs e)
        {
            //  Code that runs on application shutdown

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Helper.InsertErrorLog();
        }

        void Session_Start(object sender, EventArgs e)
        {
            // Code that runs when a new session is started

        }

        void Session_End(object sender, EventArgs e)
        {
            // Code that runs when a session ends. 
            // Note: The Session_End event is raised only when the sessionstate mode
            // is set to InProc in the Web.config file. If session mode is set to StateServer 
            // or SQLServer, the event is not raised.

        }

    }
}
