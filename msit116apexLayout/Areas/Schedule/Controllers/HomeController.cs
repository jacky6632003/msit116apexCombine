using Microsoft.AspNet.Identity;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace msit116apexLayout.Areas.Schedule.Controllers
{
    public class HomeController : Controller
    {
        HubServerMethods NewsHub = new HubServerMethods();
        // GET: Home
        private MSIT116APEXEntities db = new MSIT116APEXEntities();

        public ActionResult Index()
        {
            var userid = User.Identity.GetUserId();
            var intname = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.UserID
                                 ).FirstOrDefault();
            var intDepartment = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.DepartmentID
                                 ).FirstOrDefault();

            var q = from a in db.User
                    where(a.UserID==intname)
                    select new
                    {
                      departmentName = a.Department.departmentName,
                      depThemeColor = a.Department.depThemeColor
                    };
            var q1 = from a in db.Department
                         //where(intDepartment==1?true:a.departmentID== intDepartment)
                     where ( a.departmentID == intDepartment)
                     select a;

            SelectList Department = new SelectList(q, "depThemeColor", "departmentName");
            ViewBag.ddThemeColor= new List<SelectListItem> { new SelectListItem { Value = "aqua", Text = "自己"  } }.Concat(Department);
            return View(q1.ToList());
        }

        public JsonResult GetEvents()
        {
            using (MSIT116APEXEntities dc = new MSIT116APEXEntities())
            {
                var userid = User.Identity.GetUserId();
                var intname = (from a in db.User
                               where a.nvarcharID == userid
                               select

                                  a.UserID
                                     ).FirstOrDefault();


                var q = (from a in db.User
                        where a.UserID == intname
                         select new
                        {
                            color = a.Department.depThemeColor
                        }).First();
                var q1 = from a in dc.Events
                         where (a.UserID== intname) ||
                         (a.ThemeColor== q.color)
                         select a;
                       

                //var events = dc.Events.Where(p=>p.UserID==1).ToList();
                return new JsonResult { Data = q1.ToList(), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        [HttpPost]
        public JsonResult SaveEvent(Events e)
        {
            var status = false;
            using (MSIT116APEXEntities dc = new MSIT116APEXEntities())
            {
                var userid = User.Identity.GetUserId();
                var intname = (from a in db.User
                               where a.nvarcharID == userid
                               select

                                  a.UserID
                                       ).FirstOrDefault();
                if (e.ThemeColor!= "aqua")
                {
                    
                  
                    var intDepartmentID = (from a in db.User
                                           where a.nvarcharID == userid
                                           select

                                              a.DepartmentID
                                        ).FirstOrDefault();
                    var q2 = (from a in db.Department
                              where a.departmentID == intDepartmentID
                              select a.departmentName).FirstOrDefault();

                    var q = from a in db.User
                            where a.DepartmentID == intDepartmentID
                            select new
                            {
                                a.AspNetUsers.UserName
                            };
                    List<string> uesrname = new List<string>();

                    foreach (var item in q)
                    {
                        uesrname.Add(item.UserName);
                    }

                    for (int i = 0; i <= uesrname.Count - 1; i++)
                    {
                        NewsHub.SendMessageToUser(User.Identity.GetUserName(), uesrname[i], q2 + "新增行程", e.Description, "");

                    }

                }




                if (e.EventID > 0)
                {
                    //Update the event
                    var v = dc.Events.Where(a => a.EventID == e.EventID).FirstOrDefault();
                    if (v != null)
                    {
                        v.UserID = intname;
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                    }
                }
                else
                {
                    e.UserID = intname;
                    dc.Events.Add(e);
                }

                dc.SaveChanges();
                status = true;

            }

          

           


            return new JsonResult { Data = new { status = status } };
        }

        [HttpPost]
        public JsonResult DeleteEvent(int eventID)
        {
            var status = false;
            using (MSIT116APEXEntities dc = new MSIT116APEXEntities())
            {
                var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
                if (v != null)
                {
                    dc.Events.Remove(v);
                    dc.SaveChanges();
                    status = true;
                }
            }
            return new JsonResult { Data = new { status = status } };
        }
    }
}