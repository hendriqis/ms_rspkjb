using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR.Hubs;
using QIS.Medinfras.Web.Common;
using Newtonsoft.Json;
using Microsoft.AspNet.SignalR;

namespace QIS.Medinfras.Web.CommonLibs.Service.signalR
{
    [HubName("BroadcastMessage")]
    public class BMessage : Hub
    {
        public void send(string message) {
           IHubContext context = GlobalHost.ConnectionManager.GetHubContext<BMessage>();
           ////Clients.All.addMessage(message);
           ////Clients.Client(ID).All.messageRecieved(message);
           context.Clients.All.messageRecieved(message);
        }
    }
}