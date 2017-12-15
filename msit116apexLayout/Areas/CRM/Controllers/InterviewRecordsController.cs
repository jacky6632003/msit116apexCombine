using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using msit116apexLayout.Controllers;
using static msit116apexLayout.Models.InterviewRecords;
using Newtonsoft.Json;
using msit116apexLayout.Areas.CRM.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace msit116apexLayout.Areas.CRM.Controllers
{
    public class InterviewRecordsController : Controller
    {
        private MSIT116APEXEntities db = new MSIT116APEXEntities();
        private InterviewRepository<InterviewRecords> dbinterview = new InterviewRepository<InterviewRecords>();
        private InterviewRepository<CRM_subordinate> dbsub = new InterviewRepository<CRM_subordinate>();
        private InterviewRepository<AspNetUsers> dbasp = new InterviewRepository<AspNetUsers>();
        private InterviewRepository<IsEmployee> dbIsEm = new InterviewRepository<IsEmployee>();
        private int pagesize = 5;
        public ActionResult MasterIndex(int page = 1)
        {
            int IndexPage = page < 1 ? 1 : page;

            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var emid = User.Identity.GetUserId();
            if ((i == null) || (emid == null))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            var EmployeeData = db.AspNetUsers.Find(User.Identity.GetUserId());
            //var id = User.Identity.GetUserId();
            //var sub = dbsub.GetAll().Where(n => n.EmploteeID == id).OrderBy(n =>n.Username);
            //ViewBag.EmID = id;
            ViewBag.EmData = EmployeeData;
            ////var result = sub.ToPagedList(IndexPage, pagesize);
            //return View(sub);



            var abc = from n in dbasp.GetAll()
                      join m in dbsub.GetAll()
                      on n.Id equals m.UserID
                      where m.EmploteeID == emid
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

        public ActionResult parameterIndex(string userid)
        {
            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var id = User.Identity.GetUserId();

            if ((i == null) || (id == null))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }
            //var interviewRecords = db.InterviewRecords.Include(i => i.InterviewType);

            var l = dbinterview.GetAll();
            //傳回自己負責的會員名稱跟ID 放在dropdownlist
            var user = dbsub.GetAll().Where(n => n.EmploteeID == id).Select(n => new { UserID = n.UserID, UserName = n.Username });

            l = l.Where(n => n.EmployeeID == id).OrderBy(n => n.StartTime);
            if (l.Count() == 0)
            {
                ViewBag.message = "沒有內容";
                return View();
            }
            ViewBag.UserDropdown = user;
            ViewBag.abc = user.Where(n => n.UserID == userid).Select(n => n.UserID);
            ViewBag.EmployeeID = id;
            ViewBag.userid = userid;
            return View("Index", l);
        }

        public ActionResult Index()
        {
            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var id = User.Identity.GetUserId();
            ViewBag.EmployeeID = id;
            if ((i == null) || (id == null))
            {
                return RedirectToAction("Login", "Account", new { Area = "" });
            }

            var l = dbinterview.GetAll();
            //傳回自己負責的會員名稱跟ID 放在dropdownlist
            var user = dbsub.GetAll().Where(n => n.EmploteeID == id).Select(n => new { UserID = n.UserID, UserName = n.Username });

            ViewBag.UserDropdown = user;
            ViewBag.abc = user.First().UserID;
            ViewBag.Index = "Index";

            l = l.Where(n => n.EmployeeID == id).OrderBy(n => n.StartTime);
            if (l.Count() == 0)
            {
                ViewBag.message = "沒有內容";
                return View();
            }
            return View(l);
        }

        // GET: CRM/InterviewRecords/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewRecords interviewRecords = dbinterview.GetById(id);
           
            if (interviewRecords == null)
            {
                return HttpNotFound();
            }
            ViewBag.user =  interviewRecords.UserID;
            return PartialView(interviewRecords);
        }

        // GET: CRM/InterviewRecords/Create
        public ActionResult Create()
        {
            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var id = User.Identity.GetUserId();
            ViewBag.C_Type_ID = new SelectList(db.InterviewType, "InterviewType_ID", "TypeName");
            //todo 塞選不是員工的會員
            ViewBag.userlist = dbsub.GetAll().Where(n => n.EmploteeID == id).Select(n => new { ID = n.UserID, Name = n.Username });

            var a = db.AspNetUsers.Where(n => n.Id == id).Select(n => n.Name).FirstOrDefault();
            InterviewRecordMD im = new InterviewRecordMD
            {
                Interviewer_Name = a
            };

            return View(im);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InterviewID,UserID,Interviewer_Name,Interviewee_Name,C_Type_ID,StartTime,C_Location,InterviewContent,CreateTime,LastModifyTime")] InterviewRecords interviewRecords)
        {
            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                interviewRecords.CreateTime = DateTime.Now;
                interviewRecords.LastModifyTime = DateTime.Now;
                interviewRecords.EmployeeID = id;
                db.InterviewRecords.Add(interviewRecords);

                dbinterview.Create(interviewRecords);
                return RedirectToAction("Index");
            }


            ViewBag.C_Type_ID = new SelectList(db.InterviewType, "InterviewType_ID", "TypeName", interviewRecords.C_Type_ID);
            return View(interviewRecords);
        }


        public ActionResult Edit(int? id)
        {
            var Emid = User.Identity.GetUserId();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewRecords interviewRecords = dbinterview.GetById(id);
            if (interviewRecords == null)
            {
                return HttpNotFound();
            }
            ViewBag.selectd = db.InterviewRecords.Where(n => n.InterviewID == id).Select(n => n.Interviewee_Name).First();

            var userlist = from n in dbasp.GetAll()
                      join m in dbsub.GetAll()
                      on n.Id equals m.UserID
                      where m.EmploteeID == Emid
                      select new UserData
                      {
                          Name = n.Name,
                          UserID = n.Id
                      };

            ViewBag.userlist = userlist;
            ViewBag.C_Type_ID = new SelectList(db.InterviewType, "InterviewType_ID", "TypeName", interviewRecords.C_Type_ID);
            return View(interviewRecords);
        }

        // POST: CRM/InterviewRecords/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(InterviewRecords interviewRecords)
        {
            var i = db.AspNetUsers.Find(User.Identity.GetUserId());
            var id = User.Identity.GetUserId();
            if (ModelState.IsValid)
            {
                interviewRecords.LastModifyTime = DateTime.Now;
                interviewRecords.Interviewer_Name = i.Name;
                interviewRecords.EmployeeID = id;
                db.Entry(interviewRecords).State = EntityState.Modified;
                dbinterview.Update(interviewRecords);
                return RedirectToAction("Index");
            }
            ViewBag.C_Type_ID = new SelectList(db.InterviewType, "InterviewType_ID", "TypeName", interviewRecords.C_Type_ID);
            return View(interviewRecords);
        }

        // GET: CRM/InterviewRecords/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            InterviewRecords interviewRecords = dbinterview.GetById(id);
            if (interviewRecords == null)
            {
                return HttpNotFound();
            }
            return View(interviewRecords);
        }

        // POST: CRM/InterviewRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            InterviewRecords interviewRecords = dbinterview.GetById(id);
            //db.InterviewRecords.Remove(interviewRecords);
            dbinterview.Delete(interviewRecords);
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        public ActionResult changename(string Name, string id)
        {
            var a = db.AspNetUsers.Where(n => n.Id == Name).Select(n => n.Name).ToList();
            return Json(a, JsonRequestBehavior.AllowGet);
        }


        //取得特定用戶頭像
        public ActionResult GetUserImageFile(string userid)
        {
            var p = db.AspNetUsers.Where(n => n.Id == userid).ToList();
            if (p[0].Photo != null && p[0].Photo.Length > 2)
            {
                byte[] img = p[0].Photo;
                return File(img, "image/jpg");
            }
            else
            {
                return File("~/Images/defaultUser.png", "image/png");
            }
        }
    }
}