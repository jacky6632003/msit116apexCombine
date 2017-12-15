using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using msit116apexLayout.Areas.CRM.Models;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Areas.CRM.Controllers
{
    public class PartialController : Controller
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        private InterviewRepository<CRM_subordinate> dbsub = new InterviewRepository<CRM_subordinate>();
        private InterviewRepository<AspNetUsers> dbasp = new InterviewRepository<AspNetUsers>();
        private InterviewRepository<IsEmployee> dbIsEm = new InterviewRepository<IsEmployee>();

        public ActionResult Viewcreate_partial()
        {
            var id = User.Identity.GetUserId();
            var aspall = dbasp.GetAll();
            var IsEmall = dbIsEm.GetAll().Select(n => new { EmMail = n.UserId });//mail
            var Usersub = dbsub.GetAll().Select(n => new { ID = n.UserID });//id

            //篩掉自己
            var result = dbasp.GetAll().Where(n => n.Id != id);
            foreach (var Emmail in IsEmall)//篩掉員工
            {
                result = result.Where(n => n.UserName != Emmail.EmMail);
            }
            foreach (var Userid in Usersub)//篩掉已有關聯的會員
            {
                result = result.Where(n => n.Id != Userid.ID);
            }
            //取得員工姓名跟ID
            ViewBag.EmIDandName = aspall.Where(n => n.Id == id).Select(n => new { Username = n.Name, UserID = n.Id });
            return View(result);
        }



        public ActionResult create_partial(string Userid)
        {
            var id = User.Identity.GetUserId();
            var subordinate = dbasp.GetAll().Where(n => n.Id == Userid).First();
            CRM_subordinate crm_subordinate = new CRM_subordinate
            {
                EmploteeID = id,
                UserID = subordinate.Id,
                Username = subordinate.Name
            };
            //var Username = dbsub.GetAll().Where(n => n.UserID == crm_subordinate.UserID).Select(n =>new { UserID = n.Username }).First();
            //crm_subordinate.Username = Username.UserID;                 
            dbsub.Create(crm_subordinate);
            return RedirectToAction("Index", "InterviewRecords", new { area = "CRM" });
        }

        public ActionResult viewDelectSub(string id)// id是員工( 目前登入人)id
        {
            var abc = from n in dbasp.GetAll()
                      join m in dbsub.GetAll()
                      on n.Id equals m.UserID
                      where m.EmploteeID == id
                      select new UserData
                      {
                          UserName = n.UserName,
                          Name = n.Name,
                          BirthDay = n.BirthDay,
                          Email = n.Email,
                          MailingAddress = n.MailingAddress,
                          PhoneNumber = n.PhoneNumber,
                          Telephone = n.Telephone,
                          Title = n.Title,
                          UserID = n.Id
                      };

            return View(abc);
        }

        public ActionResult DelectSub(string Userid)
        {
            var v = db.CRM_subordinate.Where(n => n.UserID == Userid).Select(n => new { Id = n.Id }).ToList();
            var v2 = dbsub.GetById(v[0].Id);
            dbsub.Delete(v2);
            return RedirectToAction("Index", "InterviewRecords");
        }

        public ActionResult abd()
        {
            Random random = new Random();

            for (int i = 0; i < 150; i++)
            {
                if (i % 7 == 1 || i%7 == 3)
                {
                    var v = random.Next(1, 15);
                    InterviewRecords records = new InterviewRecords();
                    records.UserID = "640335af-e64a-482c-a10a-61173a9321df";
                    records.EmployeeID = "886710ef-aaf3-408c-8a94-cc3d7e7de151";
                    records.Interviewer_Name = "陳韋光";
                    records.Interviewee_Name = "鄭依婷";
                    records.C_Type_ID = 5;
                    records.C_Location = "無";
                    records.StartTime = DateTime.Now.AddDays(-150+i);
                    records.InterviewContent = "";
                    records.CreateTime = DateTime.Now.AddDays(-150 + i +1);
                    records.LastModifyTime = DateTime.Now.AddDays(-v);

                    db.InterviewRecords.Add(records);
                }
            }
           
            db.SaveChanges();

            var a = (from n in db.InterviewRecords
                     select n).Count();
   
            return Json(a, JsonRequestBehavior.AllowGet);
        }
    }
}