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
        public List<int> SendMessageToAll(string msgTitle, string msgContent,string msgUrl, List<UserNewsUrls> userNewsUrlss = null,string noDefaultAct="")
        {
            IEnumerable<AspNetUsers> aspNetUsers = dbAspNetUsers.GetAll().ToList();
            string fromUserID = "msit116apex@gmail.com";
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
            List<int> userNewsSnList = new List<int>();
            foreach (AspNetUsers usert in aspNetUsers)
            {
                UserNews userNews = new UserNews
                {
                    UserId = usert.UserName,
                    fromUser = fromUserID,
                    msgTitle = msgTitle,
                    msgContent = msgContent,
                    msgUrl = msgUrl,
                    time = DateTime.Now,
                    noDefaultAction = noDefaultAct != "" ? true : false
                };
                dbUserNews.Create(userNews);
                int sn = userNews.sn;
                userNewsSnList.Add(sn);
                if (userNewsUrlss!=null)
                {
                    foreach(UserNewsUrls unus in userNewsUrlss)
                    {
                        unus.UserNewsSn = sn;
                        if(noDefaultAct!="")
                        {
                            unus.UserNewsUrl = unus.UserNewsUrl + "&userNewsSn=" + sn;
                        }
                        dbUserNewsUrls.Create(unus);
                    }
                }

                string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
                string fromUserIdName = fromUserName + "<" + fromUserID + ">";
                hubContext.Clients.User(usert.UserName).addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss, noDefaultAct, sn.ToString());
            }
            //string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
            //string fromUserIdName = fromUserName + "<" + fromUserID + ">";
            //var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
            //hubContext.Clients.All.addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss, noDefaultAct);
            return userNewsSnList;
        }

        public int SendMessageToUser(string fromUserID, string toUserID, string msgTitle, string msgContent,string msgUrl,List<UserNewsUrls> userNewsUrlss=null, string noDefaultAct = "")
        {
            UserNews userNews = new UserNews
            {
                UserId = toUserID,
                fromUser = fromUserID,
                msgTitle = msgTitle,
                msgContent = msgContent,
                msgUrl = msgUrl,
                time = DateTime.Now,
                noDefaultAction = noDefaultAct != "" ? true : false
            };
            dbUserNews.Create(userNews);
            int sn = userNews.sn;
            if (userNewsUrlss != null)
            {
                foreach (UserNewsUrls unus in userNewsUrlss)
                {
                    unus.UserNewsSn = sn;
                    if (noDefaultAct != "")
                    {
                        unus.UserNewsUrl = unus.UserNewsUrl + "&userNewsSn=" + sn;
                    }
                    dbUserNewsUrls.Create(unus);
                }
            }
            
            string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
            string fromUserIdName = fromUserName + "<" + fromUserID + ">";
            var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
            hubContext.Clients.User(toUserID).addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss, noDefaultAct, sn.ToString());

            return sn;
        }

        public int? SendMessageToUser(bool isSystem, string toUserID, string msgTitle, string msgContent, string msgUrl, List<UserNewsUrls> userNewsUrlss = null, string noDefaultAct = "")
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
                    time = DateTime.Now,
                    noDefaultAction = noDefaultAct != "" ? true : false
                };
                dbUserNews.Create(userNews);
                int sn = userNews.sn;
                if (userNewsUrlss != null)
                {
                    foreach (UserNewsUrls unus in userNewsUrlss)
                    {
                        unus.UserNewsSn = sn;
                        if (noDefaultAct != "")
                        {
                            unus.UserNewsUrl = unus.UserNewsUrl + "&userNewsSn=" + sn;
                        }
                        dbUserNewsUrls.Create(unus);
                    }
                }
                string fromUserName = db.AspNetUsers.Where(n => n.UserName == fromUserID).Select(n => n.Name).FirstOrDefault();
                string fromUserIdName = fromUserName + "<" + fromUserID + ">";
                var hubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext<msit116apexLayout.newsHub>();
                hubContext.Clients.User(toUserID).addNewsToPage(fromUserIdName, msgTitle, msgContent, msgUrl, userNewsUrlss, noDefaultAct, sn.ToString());
                return sn;
            }
            return null;
        }
    }
}