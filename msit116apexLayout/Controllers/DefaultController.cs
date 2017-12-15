using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Controllers
{
    public class DefaultController : Controller
    {
        // GET: Default
        public ActionResult Index()
        {
            //ViewBag.uhn = Request.UserHostName;
            //ViewBag.uha = Request.UserHostAddress;
            //ViewBag.ua = Request.UserAgent;
            //ViewBag.bs = Request.Browser;
            

            return View();
        }
    }
}