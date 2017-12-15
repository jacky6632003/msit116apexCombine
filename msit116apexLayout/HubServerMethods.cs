using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout
{
    public class HubServerMethods
    {
        Repository<UserNews> dbUserNews = new Repository<UserNews>();
        Repository<UserNewsUrls> dbUserNewsUrls = new Repository<UserNewsUrls>();
        Repository<AspNetUsers> dbAspNetUsers = new Repository<AspNetUsers>();
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public void SendMessageToAll(string msgTitle, string msgContent,string msgUrl, List<UserNewsUrls> userNewsUrlss = null)
        {
            IEnumerable<AspNetUsers> aspNetUsers = dbAspNetUsers.GetAll().ToList();
            string fromUserID = "msit116apex@gmail.com";
            foreach (AspNetUsers usert in aspNetUsers)
            {
                UserNews userNews = new UserNews
                {
                    UserId = usert.UserName,
                    fromUser = fromUserID,
                    msgTitle = msgTitle,
                    msgContent = msgContent,
                    msgUrl = msgUrl,
                    time = DateTime.Now
                };
                dbUserNews.Create(userNews);
                int sn = userNews.sn;
                if(userNewsUrlss!=null)
                {
                    foreach(UserNewsUrls unus in userNewsUrlss)
                    {
                        unus.UserNewsSn = sn;
                        dbUserNewsUrls.Create(unus);
                    }
                }
            }
            string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
            string fromUserIdName = fromUserName + "<" + fromUserID + ">";
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
            hubContext.Clients.All.addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss);
        }

        public void SendMessageToUser(string fromUserID, string toUserID, string msgTitle, string msgContent,string msgUrl,List<UserNewsUrls> userNewsUrlss=null)
        {
            UserNews userNews = new UserNews
            {
                UserId = toUserID,
                fromUser = fromUserID,
                msgTitle = msgTitle,
                msgContent = msgContent,
                msgUrl = msgUrl,
                time = DateTime.Now
            };
            dbUserNews.Create(userNews);
            int sn = userNews.sn;
            if (userNewsUrlss != null)
            {
                foreach (UserNewsUrls unus in userNewsUrlss)
                {
                    unus.UserNewsSn = sn;
                    dbUserNewsUrls.Create(unus);
                }
            }
            string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
            string fromUserIdName = fromUserName + "<" + fromUserID + ">";
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
            hubContext.Clients.User(toUserID).addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss);
        }

        public void SendMessageToUser(bool isSystem, string toUserID, string msgTitle, string msgContent, string msgUrl, List<UserNewsUrls> userNewsUrlss = null)
        {
            if(isSystem)
            {
                string fromUserID = "msit116apex@gmail.com";
                UserNews userNews = new UserNews
                {
                    UserId = toUserID,
                    fromUser = fromUserID,
                    msgTitle = msgTitle,
                    msgContent = msgContent,
                    msgUrl = msgUrl,
                    time = DateTime.Now
                };
                dbUserNews.Create(userNews);
                int sn = userNews.sn;
                if (userNewsUrlss != null)
                {
                    foreach (UserNewsUrls unus in userNewsUrlss)
                    {
                        unus.UserNewsSn = sn;
                        dbUserNewsUrls.Create(unus);
                    }
                }
                string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
                string fromUserIdName = fromUserName + "<" + fromUserID + ">";
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
                hubContext.Clients.User(toUserID).addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss);
            }            
        }
    }
}