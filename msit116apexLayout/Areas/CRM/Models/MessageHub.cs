using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.CRM.Models
{
    public class MessageHub : Hub
    {
        //寄送給所有人
        public void Send(string name, string message)
        {
            //All表示所有 broadcastMessage表示轉給有監聽接收此方法的人(使用這方法都會收到訊息)
            Clients.All.broadcastMessage(name, message);
        }

        //1v1聊天
        public void SendMessageToUser(string fromUserID, string toUserID, string msgContent)
        {
            //todo新增資料表(群組).內{群組名,用戶名,主鍵} 
            //todo在此寫入Db


            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.ChatHub>();
            //User() 寄送是用帳戶名稱來區別 ,controller會叫用此自訂方法(smessage1v1)
            hubContext.Clients.User(toUserID).smessage1v1(fromUserID, msgContent);
            
        }

        //1v多 聊天
        public void SendMessageToGroup(string fromUserID,string groupName, List<string> toUsersID, string msgContent)
        {
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.ChatHub>();
            hubContext.Clients.Users(toUsersID).SendMesaage1vS(fromUserID, groupName, msgContent);
        }

        public void saveMessage(string formUserId,string toUserID,string message,DateTime now)
        {

        }
    }
}