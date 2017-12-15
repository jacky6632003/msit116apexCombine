using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using msit116apexLayout.ViewModels;
using PagedList;
using PagedList.Mvc;
using OfficeOpenXml;
using System.IO;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using static msit116apexLayout.Models.DisplayAttributeHelperMethod;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System.Web.Script.Serialization;
using LinqKit;
using msit116apexLayout.Models.Partial;
 using Microsoft.AspNet.Identity;
using System.Text;
using msit116apexLayout.Areas.Schedule.ViewModels;
using msit116apexLayout.Areas.Schedule.Models;

namespace msit116apexLayout.Areas.Schedule.Controllers
{
    public class LeavesController : Controller
    {
        private MSIT116APEXEntities db = new MSIT116APEXEntities();
        HubServerMethods NewsHub = new HubServerMethods();
        // GET: Schedule/Leaves

        public ActionResult Index(int? page, string sortBy, int year=0)
        {
            ViewBag.sortLeavecategory = string.IsNullOrEmpty(sortBy) ? "Leavecategory desc" : "Leavecategory";
            ViewBag.sortReview = sortBy == "ModelNumber" ? "ModelNumber desc" : "ModelNumber";
            ViewBag.sortStartTime = sortBy == "ModelStartTime" ? "ModelStartTime desc" : "ModelStartTime";

            int yes = DateTime.Now.Year;
            if (year == 0)
            {
                year = yes;
            }


            var userid=  User.Identity.GetUserId();
         
            string aa = "aa";
            //int a = int.Parse(aa);
            if (aa == null)
            {
                var leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Include(l => l.User.AspNetUsers);

                return View(leave.ToList().ToPagedList(page ?? 1,10));
            }
            else
            {

                var leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id== userid&&(year==0||(l.Lav_StartTime.Year==year))).OrderByDescending(l=>l.ID);
                
                switch (sortBy)
                {
                    case "Leavecategory desc":
                       leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderByDescending(p => p.Lac_LeavecategoryID);
                      break;
                    case "Leavecategory":
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderBy(p => p.Lac_LeavecategoryID);
                        break;
                    case "ModelNumber desc":
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderByDescending(p => p.Lav_ReviewID);
                        break;
                    case "ModelNumber":
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderBy(p => p.Lav_ReviewID);
                        break;
                    case "ModelStartTime desc":
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderByDescending(p => p.Lav_StartTime);
                        break;
                    case "ModelStartTime":
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderBy(p => p.Lav_StartTime);
                        break;
                    default:
                        leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).Where(l => l.User.AspNetUsers.Id == userid && (year == 0 || (l.Lav_StartTime.Year == year))).OrderByDescending(l => l.ID);
                        break;
                }

                return View(leave.ToList().ToPagedList(page ?? 1, 10));

            }


            //var q = from a in db.Leave
            //        join c in db.User on a.Lav_ID equals c.UserID
            //        join a1 in db.User on a.Lac_Agent equals a1.UserID
            //        select new
            //        {
            //            ID = a.ID,
            //            Lav_ID = a.User.UserName,
            //            Lav_StartTime = a.Lav_StartTime,
            //            Lac_EndTime = a.Lac_LeavecategoryID,
            //            Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
            //            Lac_Agent = a1.UserName,
            //            Lac_Tag = a.Lac_Tag

            //        };



            //return View(q.ToList());
        }

        public ActionResult IndexPartialView()
        {

            return PartialView();
        }


        // GET: Schedule/Leaves/Details/5
        public ActionResult Details(int? id )
        {
            var userid = User.Identity.GetUserId();
            var UserName = (from a in db.AspNetUsers
                            where a.Id == userid
                            select a.Name).FirstOrDefault();
          
              ViewBag.name = UserName;
            
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
           
            return View(leave);
        }

        // GET: Schedule/Leaves/Create
        public ActionResult Create()
        {
            var userid = User.Identity.GetUserId();
            var UserName = (from a in db.AspNetUsers
                     where a.Id == userid
                     select a.Name).FirstOrDefault();
            ViewBag.name = UserName;
            ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName");

            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();

            var intDepartmentName = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.Department.departmentName
                              ).FirstOrDefault();
            ViewBag.DepartmentName = intDepartmentName;
            var q = from a in db.User
                    where a.DepartmentID== intDepartmentID
                    select new
                    {
                        UserID = a.UserID,
                        UserName = a.AspNetUsers.Name

                    };
            var q1 =from a in db.User
                    where a.DepartmentID == intDepartmentID&&
                    a.nvarcharID== userid
                    select new
                    {
                        UserID = a.UserID,
                        UserName = a.AspNetUsers.Name

                    };


            var exceptResult = q.Except(q1);
            ViewBag.Lac_Agent = new SelectList(exceptResult, "UserID", "UserName");



            return View();
        }

        public ActionResult Createjson(int Lac_LeavecategoryID=0)
        {
            var userid = User.Identity.GetUserId();
            var intname = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.UserID
                              ).FirstOrDefault();


            var q = from a in db.Leavecategory
                    where (Lac_LeavecategoryID == 0 ? true : a.LeavecategoryID == Lac_LeavecategoryID)
                    select new
                    {
                       a.LeavecategoryTag,
                       day=(from b in db.Annualleav
                           where b.annualleaveUserID== intname &&
                           b.annualleaveYear==DateTime.Now.Year
                           select new
                           {
                               b.annualleaveUsingDay
                           }).FirstOrDefault()
                    };


            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }

        // POST: Schedule/Leaves/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Lav_StartTime,Lac_EndTime,Lac_LeavecategoryID,Lac_Agent,Lac_Tag,Lav_ReviewID,LeaveImage,BytesImage,Lav_Day")] Leave leave, HttpPostedFileBase LeaveImage)
        {
            if (ModelState.IsValid)
            {
               

                //將檔案轉成二進位
                if (LeaveImage != null)
                {
                    var imgSize = LeaveImage.ContentLength;
                    byte[] imgByte = new byte[imgSize];
                    LeaveImage.InputStream.Read(imgByte, 0, imgSize);

                    leave.LeaveImage = LeaveImage.FileName;
                    leave.BytesImage = imgByte;
                }
                var userid = User.Identity.GetUserId();
                var intname = (from a in db.User
                                  where a.nvarcharID== userid
                                  select 
                                  
                                     a.UserID
                                  ).FirstOrDefault();
                
                leave.Lav_ID = intname;
                leave.Lav_ReviewID = 3;
                db.Leave.Add(leave);
                db.SaveChanges();

                var intDepartmentID = (from a in db.User
                                       where a.nvarcharID == userid
                                       select

                                          a.DepartmentID
                                       ).FirstOrDefault();


                var q = from a in db.User
                        where (a.DepartmentID == intDepartmentID)&&
                        a.Depbool==true

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
                    NewsHub.SendMessageToUser(User.Identity.GetUserName(), uesrname[i], User.Identity.GetUserName() + "假單申請", leave.Lac_Tag, "");

                }

                return RedirectToAction("Index");

            }

            ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName", leave.Lac_LeavecategoryID);
           
            ViewBag.Lac_Agent = new SelectList(db.User, "UserID", "UserName", leave.Lac_Agent);
            return View(leave);
        }

        public ActionResult Create2()
        {
            var userid = User.Identity.GetUserId();
            var intname = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.UserID
                              ).FirstOrDefault();
            //DateTime aa = leave.Lav_StartTime;
            //string bb = aa.ToString();
            //string cc = bb.Split('/')[0];
            //int dd = int.Parse(cc);
            var q = (from a in db.Leave
                     select a).OrderByDescending(a => a.ID).FirstOrDefault();
            Annualleav annualleav = new Annualleav();

            var annualleavDay = (from a in db.Annualleav
                                 where (a.annualleaveYear == q.Lav_StartTime.Year) &&
                                 (a.annualleaveUserID == intname)
                                 select new
                                 {
                                     annualleaveID = a.annualleaveID,
                                     annualleaveDay = a.annualleaveDay

                                 }).First();
            annualleav.annualleaveUserID = intname;
            annualleav.annualleaveYear = q.Lav_StartTime.Year;
            annualleav.annualleaveID = annualleavDay.annualleaveID;
            annualleav.annualleaveDay = annualleavDay.annualleaveDay;
            annualleav.annualleaveUsingDay = q.Lav_Day;
            db.Entry(annualleav).State = EntityState.Modified;
            db.SaveChanges();

            return RedirectToAction("Index");
        }

        // GET: Schedule/Leaves/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            var userid = User.Identity.GetUserId();
            var UserName = (from a in db.AspNetUsers
                            where a.Id == userid
                            select a.Name).FirstOrDefault();
            ViewBag.name = UserName;
            ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName", leave.Lac_LeavecategoryID);
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();


            var q = from a in db.User
                    where a.DepartmentID == intDepartmentID
                    select new
                    {
                        UserID = a.UserID,
                        UserName = a.AspNetUsers.Name

                    };
            var q1 = from a in db.User
                     where a.DepartmentID == intDepartmentID &&
                     a.nvarcharID == userid
                     select new
                     {
                         UserID = a.UserID,
                         UserName = a.AspNetUsers.Name

                     };
            var exceptResult = q.Except(q1);


            ViewBag.Lac_Agent = new SelectList(exceptResult, "UserID", "UserName", leave.Lac_Agent);
            return View(leave);
        }

        // POST: Schedule/Leaves/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Lav_ID,Lav_StartTime,Lac_EndTime,Lac_LeavecategoryID,Lac_Agent,Lac_Tag,Lav_ReviewID,LeaveImage,BytesImage,Lav_Day")] Leave leave, HttpPostedFileBase LeaveImage)
        {
          

            if (ModelState.IsValid)
            {

                if (LeaveImage != null)
                {
                    //將檔案轉成二進位
                    var imgSize = LeaveImage.ContentLength;
                    byte[] imgByte = new byte[imgSize];
                    LeaveImage.InputStream.Read(imgByte, 0, imgSize);

                    leave.LeaveImage = LeaveImage.FileName;
                    leave.BytesImage = imgByte;
                }
                else if (LeaveImage == null)
                {
                    var q = (from a in db.Leave
                             where a.ID == leave.ID
                             select a.BytesImage).First();
                    leave.BytesImage = q;
                }


                if (leave.Lav_ReviewID == 0)
                {
                    var q = (from a in db.Leave
                             where a.ID == leave.ID
                             select a.Lav_ReviewID).First();
                    leave.Lav_ReviewID = q;

                }



                //Repository<Leave> leavedb = new Repository<Leave>();
                //leavedb.UpdateWithoutNull(leave);

                //if (leave.Lav_ReviewID == 0)
                //{
                //db.Entry(leave).Property("Lav_ReviewID").IsModified = false;


                //}

              


                db.Entry(leave).State = EntityState.Modified;

                db.SaveChanges();


                return RedirectToAction("Index");
            }
            ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName", leave.Lac_LeavecategoryID);
            ViewBag.Lac_Agent = new SelectList(db.User, "UserID", "UserName", leave.Lac_Agent);
            return View(leave);
        }

        // GET: Schedule/Leaves/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            return View(leave);
        }

        // POST: Schedule/Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leave leave = db.Leave.Find(id);
            db.Leave.Remove(leave);
            db.SaveChanges();
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

        public ActionResult print(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            ViewBag.Lac_Agent = new SelectList(db.User, "UserID", "UserName", leave.Lac_Agent);
            return View(leave);
        }

        public ActionResult search()
        {
            LeavecategoryUserLeaveViewModel cpcm = new LeavecategoryUserLeaveViewModel();
            cpcm.leave = db.Leave.ToList();
            cpcm.leavecategory = db.Leavecategory.ToList();
            cpcm.department = db.Department.ToList();
            cpcm.review = db.Review.ToList();
            cpcm.user = db.User.ToList();

            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.DepartmentID
                              ).FirstOrDefault();

            if (intDepartmentID == 1)
            {
                SelectList departmentID = new SelectList(db.Department, "departmentID ", "departmentName");
                ViewBag.sss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);

            }
            else
            {
                var qDepartment = from a in db.Department
                        where a.departmentID == intDepartmentID
                        select a;
                ViewBag.sss= new SelectList(qDepartment, "departmentID ", "departmentName");

            }


           

            SelectList leavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName");
            ViewBag.LacCheckBox = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有假別==" } }.Concat(leavecategoryID);
            return View(cpcm);
            //var leave = db.Leave.ToList();
            //ViewBag.leave = leave;
            //return View(db.Leavecategory.ToList());
        }
        public FileContentResult ExportToExcel(int year=0)
        {
            var userid = User.Identity.GetUserId();
            var q = from a in db.Leave
                    join c in db.User on a.Lav_ID equals c.UserID
                    where a.User.nvarcharID==userid&&
                    (year==0?true:a.Lav_StartTime.Year==year)
                    select new LeaveUserViewModel
                    {
                        ID = a.ID,
                        Lav_ID = a.User.AspNetUsers.Name,
                        Lav_StartTime = a.Lav_StartTime,
                        Lac_EndTime = a.Lac_EndTime,
                        Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                        Lac_Agent = a.User1.AspNetUsers.Name,
                        Lac_Tag = a.Lac_Tag

                    };
         
            List<Leave> lstLeave2 = db.Leave.ToList();
            List<LeaveUserViewModel> aa = q.ToList();
        

            //string[] columns = { "員工姓名", "起始時間", "結束時間", "代理人編號", "內容",  "假別" };
            string[] columns = { "ID", "Lav_ID", "Lav_StartTime", "Lac_EndTime", "Lac_LeavecategoryID" , "Lac_Agent", "Lac_Tag" };

            string DownloadName = "Leave"+DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";

            byte[] filecontent = ExcelExportHelper.ExportExcel(aa, "", false, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, DownloadName);
        }



        public FileContentResult ExportToExceSearch(string tags = "", int RadioButton = 0, string Lav_StartTime = "", string Lac_EndTime = "", int LacCheckBox = 0, int Department = 0)
        {
            if (Lav_StartTime == "")
            {
                Lav_StartTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (Lac_EndTime == "")
            {
                Lac_EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime sd = Convert.ToDateTime(Lav_StartTime);
            DateTime ed = Convert.ToDateTime(Lac_EndTime).AddDays(1);

            var tag = tags;
            var ss = Lav_StartTime;

            var q = from a in db.Leave
                    where (tag == "" ? 1 == 1 : a.User.UserName == tag) &&
                          (a.Lav_StartTime >= sd && a.Lav_StartTime <= ed && a.Lac_EndTime >= sd && a.Lac_EndTime <= ed) &&
                          (RadioButton == 0 ? true : a.Lav_ReviewID == RadioButton) &&
                          (LacCheckBox == 0 ? true : a.Lac_LeavecategoryID == LacCheckBox) &&
                          (Department == 0 ? true : a.User.DepartmentID == Department)

                    select new LeavesSearchViewModel
                    {
                        ID = a.ID,
                        Lav_ID = a.User.AspNetUsers.Name,
                        Lav_StartTime = a.Lav_StartTime,
                        Lac_EndTime = a.Lac_EndTime,
                        Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                        Lac_Agent = a.User1.AspNetUsers.Name,
                        Lac_Tag = a.Lac_Tag,
                        Department= a.User.Department.departmentName,
                        Lav_ReviewID = a.Review.ReviewName
                       

                    };

            List<LeavesSearchViewModel> lstLeave2 = q.ToList();
            //List<LeaveUserViewModel> aa = q.ToList();


            //string[] columns = { "員工姓名", "起始時間", "結束時間", "代理人編號", "內容",  "假別" };
            string[] columns = { "ID", "Lav_ID", "Lav_StartTime", "Lac_EndTime", "Lac_LeavecategoryID", "Lac_Agent", "Lac_Tag", "Department", "Lav_ReviewID" };

            string DownloadName = "LeaveSearch" + DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";

            byte[] filecontent = ExcelExportHelperSearch.ExportExcel(lstLeave2, "", false, columns);
            return File(filecontent, ExcelExportHelperSearch.ExcelContentType, DownloadName);
        }

        public ActionResult Export()
        {
            //取出要匯出Excel的資料
            List<Leave> rangerList = db.Leave.ToList();

            //建立Excel
            ExcelPackage ep = new ExcelPackage();

            //建立第一個Sheet，後方為定義Sheet的名稱
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("FirstSheet");

            Color colFromHex = System.Drawing.ColorTranslator.FromHtml("#B7DEE8");

           

            int col = 1;    //欄:直的，因為要從第1欄開始，所以初始為1

            //第1列是標題列 
            //標題列部分，是取得DataAnnotations中的DisplayName，這樣比較一致，
            //這也可避免後期有修改欄位名稱需求，但匯出excel標題忘了改的問題發生。
            //取得做法可參考最後的參考連結。
            sheet.Cells[1, col++].Value = "ID";
            sheet.Cells[1, col++].Value = "Lav_ID";
            sheet.Cells[1, col++].Value = "Lav_StartTime";
            sheet.Cells[1, col++].Value = "結束時間";
            sheet.Cells[1, col++].Value = "Lac_Agent";
            sheet.Cells[1, col++].Value = "Lac_LeavecategoryID";
            sheet.Cells[1, col++].Value = "Lac_Tag";

          
            //資料從第2列開始
            int row = 2;    //列:橫的
            foreach (Leave item in rangerList)
            {
                sheet.Column(4).Width = 20;
                sheet.Column(3).Width = 20;
                sheet.Column(3).Style.Numberformat.Format = "yyyy-MM-dd HH:mm";
                sheet.Column(4).Style.Numberformat.Format = "yyyy-MM-dd HH:mm";
                col = 1;//每換一列，欄位要從1開始
                        //指定Sheet的欄與列(欄名列號ex.A1,B20，在這邊都是用數字)，將資料寫入
                sheet.Cells[row, col++].Value = item.ID;
                sheet.Cells[row, col++].Value = item.User.UserName;
                sheet.Cells[row, col++].Value = item.Lav_StartTime;
                sheet.Cells[row, col++].Value = item.Lac_EndTime;
                sheet.Cells[row, col++].Value = item.Lac_Agent;
                sheet.Cells[row, col++].Value = item.Leavecategory.LeavecategoryName;
                sheet.Cells[row, col++].Value = item.Lac_Tag;
               
                row++;
            }

            string DownloadName = DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";
            //因為ep.Stream是加密過的串流，故要透過SaveAs將資料寫到MemoryStream，在將MemoryStream使用FileStreamResult回傳到前端
            MemoryStream fileStream = new MemoryStream();
            ep.SaveAs(fileStream);
            ep.Dispose();//如果這邊不下Dispose，建議此ep要用using包起來，但是要記得先將資料寫進MemoryStream在Dispose。
            fileStream.Position = 0;//不重新將位置設為0，excel開啟後會出現錯誤
            return File(fileStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DownloadName);
        }

    

        public ActionResult GetImage(int id = 1)
        {
            Leave leave = db.Leave.Find(id);
            byte[] img = leave.BytesImage;
            if (img == null)
            {
              return File("~/Areas/Schedule/images/NO.jpg", "image/png");
            }
            else
            return File(img, "image/jpeg");
        }

        public JsonResult GetUserName(string term)
        {
            if (term == null)
            {
                term = "a";
            }

            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();
            var q = from a in db.User
                     where a.AspNetUsers.Name.Contains(term)&&
                     (a.DepartmentID == intDepartmentID)
                    orderby a.UserName 
                    select a.AspNetUsers.Name;
            
            return Json(q.ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult test(string tags = "", int RadioButton = 0, string Lav_StartTime = "", string Lac_EndTime = "",int LacCheckBox=0, int Department = 0)
        {
            //IQueryable<Leave> leave = db.Leave;
         
            if (Lav_StartTime == "")
            {
                Lav_StartTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (Lac_EndTime == "")
            {
                Lac_EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime sd = Convert.ToDateTime(Lav_StartTime);
            DateTime ed = Convert.ToDateTime(Lac_EndTime).AddDays(1);
           
            var tag = tags;
            var ss = Lav_StartTime;

            var q = from a in db.Leave.AsEnumerable()
                    where (tag == "" ? 1 == 1 : a.User.AspNetUsers.Name == tag) &&
                          (a.Lav_StartTime >= sd && a.Lav_StartTime <= ed && a.Lac_EndTime >= sd && a.Lac_EndTime <= ed) &&
                          (RadioButton == 0 ? true : a.Lav_ReviewID == RadioButton) &&
                          (LacCheckBox == 0 ? true : a.Lac_LeavecategoryID == LacCheckBox) &&
                          (Department == 0 ? true : a.User.DepartmentID == Department)
                    select new
                    {
                        ID = "/Schedule/Leaves/GetImage/" + a.ID,
                        IDdetails = "/Schedule/Leaves/ReviewDetails/" + a.ID,
                        IDUSER = "/Schedule/Leaves/GetImageFileByuser?id=" + a.User.nvarcharID,
                        Lav_ID = a.User.AspNetUsers.Name,
                        Lav_StartTime = a.Lav_StartTime,
                        Lac_EndTime=a.Lac_EndTime,
                        Lac_LeavecategoryID=a.Leavecategory.LeavecategoryName,
                        Lac_Agent=a.User1.UserName,
                        Review= a.Review.ReviewName,
                        Day=(from b in db.Annualleav
                            where b.annualleaveUserID==a.User.UserID&&
                           b.annualleaveYear==a.Lav_StartTime.Year
                           select b.annualleaveDay).FirstOrDefault(),
                       UserDay= ((from b in db.Annualleav
                                 where b.annualleaveUserID == a.User.UserID &&
                                b.annualleaveYear == a.Lav_StartTime.Year
                                 select b.annualleaveUsingDay).FirstOrDefault()/9),
                       Department = a.User.Department.departmentName,
                       Lac_Tag=  a.Lac_Tag,
                       LACDAY=a.Lav_Day
                    };
            return new JsonResultPro(q.ToList(), "yyyy-MM-dd HH:mm");
          // return Json(q2.ToList(), JsonRequestBehavior.AllowGet);
         
        }


        public ActionResult GetImageFileByuser(string id="")
        {
            //AspNetUsers p = db.AspNetUsers.Where(n=>n.Id== user).FirstOrDefault();
            //if (p.Photo != null && p.Photo.Length > 2)
            //{
            //    byte[] img = p.Photo;
            //    return File(img, "image/jpg");
            //}
            //else
            //{

            //    return File("~/Images/defaultUser.png", "image/png");
            //}
            //var q = (from a in db.AspNetUsers
            //        where a.Id == id
            //        select a).First();

            AspNetUsers q = db.AspNetUsers.Find(id);
            byte[] img = q.Photo;
            if (img == null)
            {
                return File("~/Images/defaultUser.png", "image/png");
            }
            else
                return File(img, "image/jpeg");


        }


        public ActionResult testtest(string tags = "", int RadioButton = 0, string Lav_StartTime = "", string Lac_EndTime = "", List<string> LacCheckBox = null, int sss = 0)
        {
            IQueryable<Leave> leave = db.Leave;
            var predicateProductType = PredicateBuilder.False<Leave>();
            if (LacCheckBox != null)
            {
                foreach (var type in LacCheckBox)
                { predicateProductType = predicateProductType.Or(p => p.Leavecategory.LeavecategoryName == type); }
            }

            int[] aa = { 2, 3 };

            if (Lav_StartTime == "")
            {
                Lav_StartTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (Lac_EndTime == "")
            {
                Lac_EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime sd = Convert.ToDateTime(Lav_StartTime);
            DateTime ed = Convert.ToDateTime(Lac_EndTime).AddDays(1);

            var tag = tags;
            var ss = Lav_StartTime;
            var q1 = leave.Where(predicateProductType).ToList();
            var aaa = from a in q1.AsEnumerable()
                      where (tag == "" ? 1 == 1 : a.User.UserName == tag) &&
                          (a.Lav_StartTime >= sd && a.Lav_StartTime <= ed && a.Lac_EndTime >= sd && a.Lac_EndTime <= ed) &&
                          (RadioButton == 0 ? true : a.Lav_ReviewID == RadioButton) &&
                          (sss == 0 ? true : a.User.DepartmentID == sss)
                      select new
                      {
                          Lav_ID = a.User.UserName,
                          Lav_StartTime = a.Lav_StartTime,
                          Lac_EndTime = a.Lac_EndTime,
                          Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                          Lac_Agent = a.User1.UserName,
                          Review = a.Review.ReviewName,
                          Day = a.User.LeaveDay,
                          Department = a.User.Department.departmentName
                      };
            var q2 = (from a in db.Leave.AsEnumerable()
                      where a.Leavecategory.LeavecategoryName == LacCheckBox[0]
                      select new
                      {
                          Lav_ID = a.User.UserName,
                          Lav_StartTime = a.Lav_StartTime,
                          Lac_EndTime = a.Lac_EndTime,
                          Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                          Lac_Agent = a.User1.UserName,
                          Review = a.Review.ReviewName,
                          Day = a.User.LeaveDay,
                          Department = a.User.Department.departmentName
                      });
            for (int i = 1; i <= LacCheckBox.Count() - 1; i++)
            {
                string vaule = LacCheckBox[i];
                var q3 = (from a in db.Leave.AsEnumerable()
                          where a.Leavecategory.LeavecategoryName == vaule
                          select new
                          {
                              Lav_ID = a.User.UserName,
                              Lav_StartTime = a.Lav_StartTime,
                              Lac_EndTime = a.Lac_EndTime,
                              Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                              Lac_Agent = a.User1.UserName,
                              Review = a.Review.ReviewName,
                              Day = a.User.LeaveDay,
                              Department = a.User.Department.departmentName
                          });
                q2 = q3.Union(q2);

            }
            var q = from a in db.Leave.AsEnumerable()
                    where (tag == "" ? 1 == 1 : a.User.UserName == tag) &&
                          (a.Lav_StartTime >= sd && a.Lav_StartTime <= ed && a.Lac_EndTime >= sd && a.Lac_EndTime <= ed) &&
                          (RadioButton == 0 ? true : a.Lav_ReviewID == RadioButton)
                    select new
                    {
                        Lav_ID = a.User.UserName,
                        Lav_StartTime = a.Lav_StartTime,
                        Lac_EndTime = a.Lac_EndTime,
                        Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                        Lac_Agent = a.User1.UserName,
                        Review = a.Review.ReviewName,
                        Day = a.User.LeaveDay,
                        Department = a.User.Department.departmentName
                    };
            return new JsonResultPro(q2.ToList(), "yyyy-MM-dd HH:mm");
            // return Json(q2.ToList(), JsonRequestBehavior.AllowGet);

        }

        public ActionResult test1( )
        {
            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();

            if (intDepartmentID == 1)
            {
                SelectList departmentID = new SelectList(db.Department, "departmentID ", "departmentName");
                ViewBag.sss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);
                ViewBag.testss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);
            }
            else
            {
                var qDepartment = from a in db.Department
                                  where a.departmentID == intDepartmentID
                                  select a;
                ViewBag.sss = new SelectList(qDepartment, "departmentID ", "departmentName");
                ViewBag.testss = new SelectList(qDepartment, "departmentID ", "departmentName");
            }

            return PartialView(db.Department);
        }

        public ActionResult DeptLeave(int sss = 0, int year = 0, int i = 0)
        {
            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();


            List<LeaveUserViewModeltest> cpcm1 = new List<LeaveUserViewModeltest>();
            LeaveUserViewModel1 cpcm = new LeaveUserViewModel1();
            int toyear = DateTime.Now.Year;
            int tomon = DateTime.Now.Month;
            var q = from a in db.Leave
                    where (sss == 0 ? true : a.User.DepartmentID == sss) &&//a.User.DepartmentID== intDepartmentID
                     (year == 0 ? a.Lav_StartTime.Year == toyear : a.Lav_StartTime.Year == year) &&
                    (i == 0 ? a.Lav_StartTime.Month == tomon : a.Lav_StartTime.Month == i) &&
                    (a.Lav_ReviewID == 1)
                    orderby a.Lav_StartTime
                    select a;

            var q1 = from a in db.User
                     where (sss == 0 ? /*a.DepartmentID == intDepartmentID*/true : a.DepartmentID == sss)
                     select a;
            foreach (var item in q1)
            {
                IEnumerable<Leave> leave = q.Where(n=>n.Lav_ID==item.UserID);

                LeaveUserViewModeltest aaa = new LeaveUserViewModeltest
                {
                    user = item,
                    leave=leave


                };
                cpcm1.Add(aaa);
            }
            cpcm.leave = q.ToList();
            cpcm.user = q1.ToList();
              
            if (i == 1 || i == 3 || i == 5 || i == 7 || i == 8 || i == 10 || i == 12)
            {
                ViewBag.s2 = 31;
            }
            else
                ViewBag.s2 = 30;
            return View(cpcm1);

        }


        public ActionResult Review(int? page ,int ReviewID=0 ,int year=0)
        {
            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.DepartmentID
                              ).FirstOrDefault();

            int yes = DateTime.Now.Year;
            if (year == 0)
            {
                year = yes;
            }

            if (ReviewID == 0)
            {
                var leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).OrderByDescending(l => l.ID).Where(l=>l.User.DepartmentID== intDepartmentID&& l.User.DepartmentID == intDepartmentID&l.Lav_StartTime.Year==year);
                return View(leave.ToList().ToPagedList(page ?? 1, 10));
            }
            else
            {
                var leave = db.Leave.Include(l => l.Leavecategory).Include(l => l.Review).Include(l => l.User).Include(l => l.User1).OrderByDescending(l => l.ID).Where(l => l.Lav_ReviewID == ReviewID&&l.Lav_StartTime.Year==year&& l.User.DepartmentID == intDepartmentID);
                return View(leave.ToList().ToPagedList(page ?? 1, 10));
            }
        }
        public ActionResult ReviewDetails(int? id)
        {
            var userid = User.Identity.GetUserId();
            var UserName = (from a in db.AspNetUsers
                            where a.Id == userid
                            select a.Name).FirstOrDefault();
            ViewBag.name = UserName;
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }

            return View(leave);
        }

        public ActionResult ReviewPartialView()
        {
            

            return PartialView();
        }



        public ActionResult ReviewEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leave leave = db.Leave.Find(id);
            if (leave == null)
            {
                return HttpNotFound();
            }
            //ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName", leave.Lac_LeavecategoryID);
            //ViewBag.Lac_Agent = new SelectList(db.User, "UserID", "UserName", leave.Lac_Agent);
            ViewBag.Lav_ReviewID = new SelectList(db.Review, "ReviewID", "ReviewName", leave.Lav_ReviewID);
            return View(leave);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReviewEdit([Bind(Include = "ID,Lav_ID,Lav_StartTime,Lac_EndTime,Lac_LeavecategoryID,Lac_Agent,Lac_Tag,Lav_ReviewID,LeaveImage,BytesImage,Lav_Day")] Leave leave, HttpPostedFileBase LeaveImage)
        {
            if (ModelState.IsValid)
            {

                if (LeaveImage != null)
                {
                    //將檔案轉成二進位
                    var imgSize = LeaveImage.ContentLength;
                    byte[] imgByte = new byte[imgSize];
                    LeaveImage.InputStream.Read(imgByte, 0, imgSize);

                    leave.LeaveImage = LeaveImage.FileName;
                    leave.BytesImage = imgByte;
                }
                else if (LeaveImage == null)
                {
                    var qimg = (from a in db.Leave
                             where a.ID == leave.ID
                             select a.BytesImage).First();
                    leave.BytesImage = qimg;
                }




                if (leave.Lav_ReviewID == 1&&leave.Lac_LeavecategoryID==1)
                {
                    var annualleav = (from a in db.Annualleav
                                      where (a.annualleaveYear == leave.Lav_StartTime.Year) &&
                                            (a.annualleaveUserID == leave.Lav_ID)
                                      select a).First();
                   
                    var number = annualleav.annualleaveUsingDay;
                    if (number == null)
                    {
                        number = 0;
                    }
                    annualleav.annualleaveUsingDay = number + leave.Lav_Day;

                    db.Entry(annualleav).State = EntityState.Modified;
                    db.SaveChanges();
                }

                var q = (from a in db.Leave
                         where a.ID == leave.ID
                         select new
                         {
                             Lac_Agent = a.Lac_Agent,
                             Lac_LeavecategoryID = a.Lac_LeavecategoryID,

                         }).FirstOrDefault();
                leave.Lac_Agent = q.Lac_Agent;
                leave.Lac_LeavecategoryID = q.Lac_LeavecategoryID;


                if (leave.Lav_ReviewID != 3)
                {
                    var userid = User.Identity.GetUserId();
                    var userID = leave.Lav_ID;
                    var reuser = (from a in db.User
                                  where a.UserID == userID
                                  select a.AspNetUsers.UserName).FirstOrDefault();
                    NewsHub.SendMessageToUser(User.Identity.GetUserName(), reuser, User.Identity.GetUserName() + "假單已審批", leave.LavSupervisor == null ? "" : leave.LavSupervisor, "");
                }

               

                db.Entry(leave).State = EntityState.Modified;

                db.SaveChanges();
              
                return RedirectToAction("Review");



               

            }
            //ViewBag.Lac_LeavecategoryID = new SelectList(db.Leavecategory, "LeavecategoryID", "LeavecategoryName", leave.Lac_LeavecategoryID);
            //ViewBag.Lac_Agent = new SelectList(db.User, "UserID", "UserName", leave.Lac_Agent);
            ViewBag.Lav_ReviewID = new SelectList(db.Review, "ReviewID", "ReviewName", leave.Lav_ReviewID);
            return View(leave);
        }

      


        public ActionResult DeptLeavetest(int sss=0 , int year = 0, int i = 0)
        {

            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();


            //List<LeaveUserViewModeltest> cpcm = new List<LeaveUserViewModeltest>();
            LeaveUserViewModel1 cpcm = new LeaveUserViewModel1();
            int toyear=   DateTime.Now.Year;
             var q = from a in db.Leave
                    where (sss == 0 ?  true: a.User.DepartmentID == sss) &&//a.User.DepartmentID== intDepartmentID
                     (year==0 ?a.Lav_StartTime.Year== toyear : a.Lav_StartTime.Year==year) &&
                    (i==0? a.Lav_StartTime.Month == 1: a.Lav_StartTime.Month==i)&&
                    (a.Lav_ReviewID==1)
                     orderby a.Lav_StartTime 
                     select a;

            var q1 = from a in db.User
                     where (sss == 0 ? a.DepartmentID == intDepartmentID : a.DepartmentID == sss)
                     select a;
           //foreach(var item in q1)
           // {
           //     IEnumerable<Leave> leave = q.Where(n => n.Lav_ID == item.UserID);

           //     LeaveUserViewModeltest aaa = new LeaveUserViewModeltest
           //     {
           //         user=item


           //     };
           //     cpcm.Add(aaa);
           // }
            cpcm.user = q1.ToList(); ;
            cpcm.leave = q.ToList();
            if (i==1||i==3 || i == 5 || i == 7 || i == 8 || i == 10 || i == 12)
            {
                ViewBag.s2 = 31;
            }else
                ViewBag.s2 = 30;
            return View(cpcm);

        }

        public ActionResult test2()
        {
            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();
            
            if (intDepartmentID == 1)
            {
                SelectList departmentID = new SelectList(db.Department, "departmentID ", "departmentName");
                ViewBag.sss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);
                ViewBag.testss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);
            }
            else
            {
                var qDepartment = from a in db.Department
                                  where a.departmentID == intDepartmentID
                                  select a;
                ViewBag.sss = new SelectList(qDepartment, "departmentID ", "departmentName");
                ViewBag.testss = new SelectList(qDepartment, "departmentID ", "departmentName");
            }

            return PartialView(db.Department);
        }
        public JsonResult annualleave(int ans1 =0, int ans2 =0)
        {

            var q = from a in db.User
                    where (ans1 == 0 ? true : a.DepartmentID == ans1)
                 
                    select new
                    {
                        DepartmentName = a.Department.departmentName,
                        UserName= a.UserName,
                        LeaveDay= a.LeaveDay

                    };
           
            var q1 = from a in db.Annualleav
                     where (ans2 == 0 ? true : a.annualleaveYear == ans2) &&
                     (ans1 == 0 ? true : a.User.DepartmentID == ans1)
                     select new
                     {
                         DepartmentName = a.User.Department.departmentName,
                         UserName = a.User.AspNetUsers.Name,
                         LeaveDay = a.annualleaveDay,
                         AnnualleaveUsingDay= a.annualleaveUsingDay
                     };

            return Json(q1.ToList(), JsonRequestBehavior.AllowGet);
            
        }
        public ActionResult echarts()
        {



            var userid = User.Identity.GetUserId();
            var intDepartmentID = (from a in db.User
                                   where a.nvarcharID == userid
                                   select

                                      a.DepartmentID
                              ).FirstOrDefault();

            if (intDepartmentID == 1)
            {

                ViewBag.departmentID = new SelectList(db.Department, "departmentID ", "departmentName");
                ViewBag.departmentID2 = new SelectList(db.Department, "departmentID ", "departmentName");

                SelectList departmentIDt = new SelectList(db.Department, "departmentID ", "departmentName");
                ViewBag.departmentID3 = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentIDt);

                
                var q = from a in db.User
                        where a.DepartmentID == intDepartmentID
                        select a;
                ViewBag.UserID = new SelectList(q, "UserID", "UserName");
            }
            else
            {
                var qDepartment = from a in db.Department
                                  where a.departmentID == intDepartmentID
                                  select a;
                var q = from a in db.User
                        where a.DepartmentID == intDepartmentID
                        select a;
                ViewBag.UserID = new SelectList(q, "UserID", "UserName");

                ViewBag.departmentID = new SelectList(qDepartment, "departmentID ", "departmentName");
                ViewBag.departmentID2 = new SelectList(qDepartment, "departmentID ", "departmentName");
                ViewBag.departmentID3 = new SelectList(qDepartment, "departmentID ", "departmentName");
            }
            
           

            return View();
        }


        public ActionResult echartsDepartmentID(int Department=0)
        {
            StringBuilder sb = new StringBuilder();
            var q = from a in db.User
                    where (Department==0?true:a.DepartmentID== Department)
                    select a;
            foreach(var item in q)
            {
                sb.AppendFormat("<option value=\"{0}\">{1}</option>",item.UserID,item.UserName);

            }


            return Content(sb.ToString());
        }

        public ActionResult echartsDepartmentIDJson(int year =0 ,int ids=0 ,int mon=0)
        {
            List<int> count1 = new List<int>();
            var leave = db.Leave;
            for (int i = 1; i <= 10; i++)
            {
                count1.Add(leave.Where(years => years.Lav_StartTime.Year == year && years.Lav_StartTime.Month == mon && years.Lav_ID == ids && years.Lac_LeavecategoryID == i&&years.Lav_ReviewID==1).Count()) ;

            }

            return Json(count1, JsonRequestBehavior.AllowGet);
        }



        public JsonResult echartsleave(int year = 0,int dep=0)
        {
            var leave = db.Leave;
            List<int> count1=new List<int>();
         
            for(int i = 1; i <= 12; i++)
            {
               count1.Add(leave.Where(ch => ch.Lav_StartTime.Year == year && ch.Lav_StartTime.Month == i&&ch.Lav_ReviewID==1&&(dep==0||ch.User.DepartmentID==dep)).Count()) ;
            }

            return Json(count1, JsonRequestBehavior.AllowGet);
        }

        public JsonResult echartsleave1(int year = 0, int dep = 0)
        {
            var leave = db.Leave;
            List<int> count1 = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                count1.Add(leave.Where(ch => ch.Lav_StartTime.Year == year-1 && ch.Lav_StartTime.Month == i && ch.Lav_ReviewID == 1 && (dep == 0 || ch.User.DepartmentID == dep)).Count());
            }

            return Json(count1, JsonRequestBehavior.AllowGet);
        }


        public ActionResult searchPartialView()
        {
            LeavecategoryUserLeaveViewModel cpcm = new LeavecategoryUserLeaveViewModel();
            cpcm.leave = db.Leave.ToList();
            cpcm.leavecategory = db.Leavecategory.ToList();
            cpcm.department = db.Department.ToList();
            cpcm.review = db.Review.ToList();
            cpcm.user = db.User.ToList();



            SelectList departmentID = new SelectList(cpcm.department, "departmentID ", "departmentName");
            ViewBag.sss = new List<SelectListItem> { new SelectListItem { Value = "0", Text = "==所有部门==" } }.Concat(departmentID);
            

            return PartialView(cpcm);
        }
        public ActionResult searchtest(string tags = "", int RadioButton = 0, string Lav_StartTime = "", string Lac_EndTime = "", int sss = 0)
        {
           

            if (Lav_StartTime == "")
            {
                Lav_StartTime = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (Lac_EndTime == "")
            {
                Lac_EndTime = DateTime.Now.ToString("yyyy-MM-dd");
            }

            DateTime sd = Convert.ToDateTime(Lav_StartTime);
            DateTime ed = Convert.ToDateTime(Lac_EndTime).AddDays(1);

            var tag = tags;
            var ss = Lav_StartTime;
            
           
            //var q2 = (from a in db.Leave.AsEnumerable()
            //          where a.Leavecategory.LeavecategoryName == LacCheckBox[0]
            //          select a);
                     
            //for (int i = 1; i <= LacCheckBox.Count() - 1; i++)
            //{
            //    string vaule = LacCheckBox[i];
            //    var q3 = (from a in db.Leave.AsEnumerable()
            //              where a.Leavecategory.LeavecategoryName == vaule
            //              select a);
            //    q2 = q3.Union(q2);

            //}
            var q = from a in db.Leave.AsEnumerable()
                    where (tag == "" ? 1 == 1 : a.User.UserName == tag) &&
                          (a.Lav_StartTime >= sd && a.Lav_StartTime <= ed && a.Lac_EndTime >= sd && a.Lac_EndTime <= ed) &&
                          (RadioButton == 0 ? true : a.Lav_ReviewID == RadioButton)
                    select new
                    {
                        Lav_ID = a.User.UserName,
                        Lav_StartTime = a.Lav_StartTime,
                        Lac_EndTime = a.Lac_EndTime,
                        Lac_LeavecategoryID = a.Leavecategory.LeavecategoryName,
                        Lac_Agent = a.User1.UserName,
                        Review = a.Review.ReviewName,
                        Day = a.User.LeaveDay,
                        Department = a.User.Department.departmentName
                    };


            return View(q.ToList());
        }

        public ActionResult meteorological()
        {

            return View();
        }
        public ActionResult meteorologicalPartialView()
        {
            int year= DateTime.Now.Year;
            int mon = DateTime.Now.Month;
            var userid = User.Identity.GetUserId();
            var intname = (from a in db.User
                           where a.nvarcharID == userid
                           select

                              a.UserID
                              ).FirstOrDefault();


            var q1 = (from a in db.User
                     where a.UserID == intname
                     select new
                     {
                         color = a.Department.depThemeColor
                     }).First();
            var q = (from a in db.Events
                    where (a.UserID == intname||a.ThemeColor==q1.color)&&
                   ( a.Start> DateTime.Now )
                   orderby a.Start
                    //(a.Start.Year>=year)&&
                    //(a.Start.Month>=mon)

                    select a).Take(5);
                  

            return PartialView(q.ToList());
        }

        public ActionResult jasontest(int year = 0, int ids = 0, int mon = 0)
        {

            var q = from a in db.Leave
                    where (year == 0 ? true : a.Lav_StartTime.Year == year) &&
                    (mon == 0 ? true : a.Lav_StartTime.Month == mon) &&
                    (ids == 0 ? a.Lav_ID == 1 : a.Lav_ID == ids)&&
                    (a.Lav_ReviewID==1)
                    group a by a.Leavecategory.LeavecategoryName into g
                    select new
                    {
                        value = g.Count(),
                        name = g.Key,
                    };
            


            return Json(q.ToList(), JsonRequestBehavior.AllowGet);
        }
        public ActionResult jasonDep(int year = 0, int dqp = 0, int mon = 0)
        {

            //var q1 = (from a in db.User
            //          where (dqp == 0 ? true : a.DepartmentID == dqp)
            //          select a).Count();

            List<int> count1 = new List<int>();

            //var q = from a in db.Leave
            //        where (year == 0 ? true : a.Lav_StartTime.Year == year) &&
            //        (mon == 0 ? true : a.Lav_StartTime.Month == mon) &&
            //        (dqp == 0 ? true : a.User.DepartmentID == dqp) &&
            //        (a.Lav_ReviewID == 1)
            //        group a by a.User.UserName into g
            //        select new valuename
            //        {
                       
            //            name = g.Key,
            //            value = g.Count(),
            //        };
            //var q2 = from a in db.User
            //         where (dqp == 0 ? true : a.DepartmentID == dqp)
            //         select new valuename
            //         {
            //             name = a.UserName,
            //             value=0
            //         };
            //var x = q.Union(q2);

            var q3 = from a in db.User
                     where (dqp == 0 ? true : a.DepartmentID == dqp)
                     select new
                     {
                         name = a.UserName,
                         value = (from b in db.Leave
                                  where (year == 0 ? true : b.Lav_StartTime.Year == year) &&
                                         (mon == 0 ? true : b.Lav_StartTime.Month == mon) &&
                                         (b.Lav_ReviewID == 1) &&
                                         (a.UserID == b.Lav_ID)
                                  select new
                                  {
                                      b.Lav_StartTime
                                  }).Count()

                     };
            foreach(var item in q3)
            {
                count1.Add(item.value);
            }

            return Json(count1, JsonRequestBehavior.AllowGet);
        }
        public ActionResult jasonDep1( int dqp = 0)
        {
            List<string> count = new List<string>();
            var q1 = from a in db.User
                     where (dqp == 0 ? true : a.DepartmentID == dqp)
                    
                     select new
                     {
                         aa = a.UserName

                     };

          
           foreach(var item in q1)
            {
                count.Add(item.aa);

            }


            return Json(count, JsonRequestBehavior.AllowGet);
        }


        public FileContentResult ExportToExcelechart3(int year = 0, int dqp = 0, int mon = 0)
        {


            var q3 = from a in db.User
                     where (dqp == 0 ? true : a.DepartmentID == dqp)
                     select new ExportToExcelechart3ViewModel
                     {
                         name = a.UserName,
                         value = (from b in db.Leave
                                  where (year == 0 ? true : b.Lav_StartTime.Year == year) &&
                                         (mon == 0 ? true : b.Lav_StartTime.Month == mon) &&
                                         (b.Lav_ReviewID == 1) &&
                                         (a.UserID == b.Lav_ID)
                                  select new
                                  {
                                      b.Lav_StartTime
                                  }).Count()

                     };

            List<ExportToExcelechart3ViewModel> exportToExcelechart3 = q3.ToList();
            var dep = (from a in db.Department
                       where (dqp == 0 ? true : a.departmentID == dqp)
                       select a.departmentName).FirstOrDefault();


            //string[] columns = { "員工姓名", "起始時間", "結束時間", "代理人編號", "內容", "假別" };
            string[] columns = { "name", "value" };

            string DownloadName = dep+ "Leave" + DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";

            byte[] filecontent = ExportToExcelechart3Helper.ExportExcel(exportToExcelechart3, "", false, columns);
            return File(filecontent, ExportToExcelechart3Helper.ExcelContentType, DownloadName);
        }

        public FileContentResult ExportToExcelechart2(int year = 0, int ids = 0, int mon = 0)
        {


            var q = from a in db.Leavecategory
                    
                    select new ExportToExcelechart3ViewModel
                    {
                        name = a.LeavecategoryName,
                        value = ((from b in db.Leave
                                 where (year == 0 ? true : b.Lav_StartTime.Year == year) &&
                                       (mon == 0 ? true : b.Lav_StartTime.Month == mon) &&
                                       (ids == 0 ? true : b.Lav_ID == ids) &&
                                       (b.Lav_ReviewID == 1)&&
                                       (b.Lac_LeavecategoryID==a.LeavecategoryID)
                                       
                                 select b).Count())
                    };

            List < ExportToExcelechart3ViewModel > exportToExcelechart3 =q.ToList();

            //string[] columns = { "員工姓名", "起始時間", "結束時間", "代理人編號", "內容",  "假別" };
            string[] columns = { "name", "value" };
            var userName = User.Identity.Name;
            string DownloadName = userName+"Leave"  + DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";

            byte[] filecontent = ExportToExcelechart2Helper.ExportExcel(exportToExcelechart3, "", false, columns);
            return File(filecontent, ExportToExcelechart2Helper.ExcelContentType, DownloadName);
        }

        public FileContentResult ExportToExcelechart1(int year = 0)
        {
            var leave = db.Leave;
            List<int> count1 = new List<int>();

            for (int i = 1; i <= 12; i++)
            {
                count1.Add(leave.Where(ch => ch.Lav_StartTime.Year == year && ch.Lav_StartTime.Month == i && ch.Lav_ReviewID == 1).Count());
            }

            //string[] columns = { "員工姓名", "起始時間", "結束時間", "代理人編號", "內容",  "假別" };
            string[] columns = { "name"};

            string DownloadName = "LeaveExcelDep" + DateTime.Now.ToString(@"yyyy-MM-dd hhmmss") + ".xlsx";

            byte[] filecontent = ExportToExcelechart3Helper.ExportExcel(count1, "", false, columns);
            return File(filecontent, ExportToExcelechart3Helper.ExcelContentType, DownloadName);
        }

        public ActionResult databass()
        {
            Random tt = new Random();

            for (int i = 400; i <= 550; i++)
            {
                
               int a= tt.Next(1, 27);
                int b = tt.Next(1, 10);
                int c = tt.Next(1, 10);
                int d = tt.Next(1, 11);
                int e= tt.Next(1, 11);
                Leave leave = new Leave();
                leave.ID = i;
                leave.Lav_ID = c;
                leave.Lav_StartTime = new DateTime(2017,e,01+a ,09,00,00);
                leave.Lac_EndTime = new DateTime(2017, e, 01+a,17, 00, 00);
                leave.Lac_LeavecategoryID = d;
                leave.Lac_Agent = b;
                leave.Lac_Tag = "休息";
                leave.Lav_ReviewID = 1;
                leave.Lav_Day = 9;
                leave.LavSupervisor = "好好休息";
                db.Leave.Add(leave);
                db.SaveChanges();
            }
            return View();

        }
    }

}

