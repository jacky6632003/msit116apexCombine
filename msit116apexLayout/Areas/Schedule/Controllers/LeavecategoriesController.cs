using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;

namespace msit116apexLayout.Areas.Schedule.Controllers
{
    public class LeavecategoriesController : Controller
    {
        private MSIT116APEXEntities db = new MSIT116APEXEntities();

        // GET: Schedule/Leavecategories
        public ActionResult Index()
        {
            return View(db.Leavecategory.ToList());
        }

        // GET: Schedule/Leavecategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leavecategory leavecategory = db.Leavecategory.Find(id);
            if (leavecategory == null)
            {
                return HttpNotFound();
            }
            return View(leavecategory);
        }

        // GET: Schedule/Leavecategories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Schedule/Leavecategories/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "LeavecategoryID,LeavecategoryName")] Leavecategory leavecategory)
        {
            if (ModelState.IsValid)
            {
                db.Leavecategory.Add(leavecategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(leavecategory);
        }

        // GET: Schedule/Leavecategories/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leavecategory leavecategory = db.Leavecategory.Find(id);
            if (leavecategory == null)
            {
                return HttpNotFound();
            }
            return View(leavecategory);
        }

        // POST: Schedule/Leavecategories/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "LeavecategoryID,LeavecategoryName")] Leavecategory leavecategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(leavecategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(leavecategory);
        }

        // GET: Schedule/Leavecategories/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Leavecategory leavecategory = db.Leavecategory.Find(id);
            if (leavecategory == null)
            {
                return HttpNotFound();
            }
            return View(leavecategory);
        }

        // POST: Schedule/Leavecategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Leavecategory leavecategory = db.Leavecategory.Find(id);
            db.Leavecategory.Remove(leavecategory);
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
    }
}
