using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace msit116apexLayout
{
    public class ChatHub : Hub
    {
        public void Send(string name, string message)
        {
            // Call the broadcastMessage method to update clients.
            //All表示所有 broadcastMessage表示轉給有監聽接收此方法的人(使用這方法都會收到訊息)
            Clients.All.broadcastMessage(name, message);
        }
    }
}