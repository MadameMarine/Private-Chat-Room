using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace SignalRChat.Hubs
{
    public class BroadcastHub : Hub
    {
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}