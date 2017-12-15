using msit116apexLayout.Areas.CRM.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Areas.CRM.Controllers
{

    public class MessagesController : Controller
    {
        MessageHub MH = new MessageHub();
        // GET: CRM/Messages
        public ActionResult Index()
        {

            return View("textmessageIndex");
        }
        //接收從前端來的資料
        public void send1v1(string toUser, string message)
        {
            var formuser = User.Identity.GetUserName();
            //叫用MessageHub(singalr hub)的class下的方法
            MH.SendMessageToUser(formuser, toUser, message);
        }
    }
}