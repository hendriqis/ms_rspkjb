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
namespace QIS.Medinfras.Web.CommonLibs
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
            // Code that runs when an unhandled error occurs
            HttpServerUtility server = HttpContext.Current.Server;
            Exception exception = server.GetLastError();
            string userIP = HttpContext.Current.Request.UserHostAddress;
            string appPath = HttpContext.Current.Request.Url.AbsolutePath;
            string trace = exception.StackTrace;
            string moduleName = Helper.GetModuleName();
            string ModuleID = Helper.GetModuleID(moduleName);

            string message = string.Format("{0}|{1}|{2}|{3}|{4}{5}", DateTime.Now.ToString(Constant.FormatString.TIME_FORMAT), ModuleID, userIP, appPath, trace, Environment.NewLine);

            string myFile = VirtualPathUtility.ToAbsolute(string.Format("~/Libs/App_Data/log/{0}.txt", DateTime.Now.ToString("yyyyMMdd")));
            string physicalPath = HttpContext.Current.Request.MapPath(myFile);

            if (!File.Exists(physicalPath))
                File.WriteAllText(physicalPath, message);
            else
                File.AppendAllText(physicalPath, message);
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
