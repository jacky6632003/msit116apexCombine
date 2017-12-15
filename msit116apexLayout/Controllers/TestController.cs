using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;

namespace msit116apexLayout.Controllers
{
    public class TestController : Controller
    {
        AccountMethod accountM = new AccountMethod();
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        // GET
        public ActionResult Index()
        {
            //取得使用者資料
            return View(accountM.GetUsersData());
            //return View(accountM.GetEmployeesData());
            //return View(accountM.GetMembersData());
        }

        // GET
        public ActionResult Details(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDataModel userData = accountM.GetUserByID(UserName);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }
        
        // GET
        public ActionResult Edit(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDataModel userData = accountM.GetUserByID(UserName);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        // POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserDataModel userData)
        {
            if (ModelState.IsValid)
            {
                accountM.UpdateUser(userData);
                return RedirectToAction("Index","Test");
            }
            return View(userData);
        }

        // GET
        public ActionResult Delete(string UserName)
        {
            if (UserName == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            UserDataModel userData = accountM.GetUserByID(UserName);
            if (userData == null)
            {
                return HttpNotFound();
            }
            return View(userData);
        }

        // POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string UserName)
        {
            accountM.DeleteUserByID(UserName);
            return RedirectToAction("Index","Test");
        }       

        public ActionResult setDemoAccountDefault()
        {
            string demoAccount = "ycwaspclient@gmail.com";
            AspNetUsers anu = db.AspNetUsers.Where(n => n.UserName == demoAccount).FirstOrDefault();
            if (anu != null)
            {
                var ura = db.UserResidenceAddress.Where(n => n.Id == anu.Id).FirstOrDefault();
                if (ura != null)
                {
                    db.UserResidenceAddress.Remove(ura);
                    db.SaveChanges();
                }
                var uma = db.UserMailAddress.Where(n => n.Id == anu.Id).FirstOrDefault();
                if (uma != null)
                {
                    db.UserMailAddress.Remove(uma);
                    db.SaveChanges();
                }
                var uns = db.UserNews.Where(n => n.fromUser == anu.UserName || n.UserId == anu.UserName).ToList();
                if (uns != null)
                {
                    foreach(var un in uns)
                    {
                        IEnumerable<UserNewsUrls> unu = db.UserNewsUrls.Where(n => n.UserNewsSn == un.sn);
                        if (unu.Count() != 0)
                            db.UserNewsUrls.RemoveRange(unu);
                        IEnumerable<UserNewsConfirmList> uncl = db.UserNewsConfirmList.Where(n => n.UserNewsSn == un.sn);
                        if (uncl.Count() != 0)
                            db.UserNewsConfirmList.RemoveRange(uncl);
                    }
                    db.UserNews.RemoveRange(uns);
                }
                var uru = db.uRoleUsers.Where(n => n.uUserID == anu.UserName);
                if (uru != null)
                {
                    db.uRoleUsers.RemoveRange(uru);
                }
                var urpchs = db.uRolePowerConfirmHistory.Where(n => n.UserID == anu.UserName).ToList();
                if(urpchs.Count()>0)
                {
                    foreach(var urpch in urpchs)
                    {
                        IEnumerable<uRolePowerConfirmHistoryConfirmData> urpchcds = db.uRolePowerConfirmHistoryConfirmData.Where(n => n.uRolePowerConfirmHistorySn == urpch.urpchSn).ToList();
                        if(urpchcds.Count()>0)
                        {
                            foreach (var urpchcd in urpchcds)
                            {
                                IEnumerable<uRolePowerConfirmHistoryConfirmDataDetail> urpchcdds = db.uRolePowerConfirmHistoryConfirmDataDetail.Where(n => n.uRolePowerConfirmHistoryConfirmDataSn == urpchcd.urpchcdSn);
                                if(urpchcdds.Count()>0)
                                {
                                    db.uRolePowerConfirmHistoryConfirmDataDetail.RemoveRange(urpchcdds);
                                }
                            }
                            db.uRolePowerConfirmHistoryConfirmData.RemoveRange(urpchcds);                            
                        }
                        IEnumerable<uRolePowerConfirmHistoryDetail> urpchds = db.uRolePowerConfirmHistoryDetail.Where(n => n.uRolePowerConfirmHistorySn == urpch.urpchSn);
                        if(urpchds.Count()>0)
                        {
                            db.uRolePowerConfirmHistoryDetail.RemoveRange(urpchds);
                        }
                    }
                    db.uRolePowerConfirmHistory.RemoveRange(urpchs);
                }
                IsEmployee isee = db.IsEmployee.Where(n => n.UserId == anu.UserName).FirstOrDefault();
                if(isee!=null)
                {
                    db.IsEmployee.Remove(isee);
                }
                var anul = db.AspNetUserLogins.Where(n => n.UserId == anu.Id).FirstOrDefault();
                if (anul != null)
                    db.AspNetUserLogins.Remove(anul);
                db.AspNetUsers.Remove(anu);
                db.SaveChanges();
                return Content("以成功移除");
            }
            IEnumerable<ShoppingCart> scs = db.ShoppingCart.ToList();
            if (scs.Count() > 0)
            {
                db.ShoppingCart.RemoveRange(scs);
                db.SaveChanges();
            }
            IEnumerable<FundOrderDetail> fod = db.FundOrderDetail.ToList();
            if (fod.Count() > 0)
            {
                db.FundOrderDetail.RemoveRange(fod);
                db.SaveChanges();
            }
            IEnumerable<FundOrder> fo = db.FundOrder.ToList();
            if (fo.Count() > 0)
            {
                db.FundOrder.RemoveRange(fo);
                db.SaveChanges();
            }
            Session["CartID"] = null;

            return Content("沒有Demo帳號資料");
        }
    }
}
