using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;

namespace msit116apexLayout.Areas.Fund.Controllers
{
    public class CreateNewFundController : Controller
    {
        private MSIT116APEXEntities db = new MSIT116APEXEntities();

        // GET: Fund/CreateNewFund
        public ActionResult Index()
        {
            return View(db.FundMaster.ToList());
        }

        // GET: Fund/CreateNewFund/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundMaster fundMaster = db.FundMaster.Find(id);
            if (fundMaster == null)
            {
                return HttpNotFound();
            }
            return View(fundMaster);
        }

        // GET: Fund/CreateNewFund/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Fund/CreateNewFund/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FundID,FundName,SetUpDate,Location,Custodian,FundScale,StarRank,FundStatus")] FundMaster fundMaster)
        {
            if (ModelState.IsValid)
            {
                db.FundMaster.Add(fundMaster);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(fundMaster);
        }

        // GET: Fund/CreateNewFund/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundMaster fundMaster = db.FundMaster.Find(id);
            if (fundMaster == null)
            {
                return HttpNotFound();
            }
            return View(fundMaster);
        }

        // POST: Fund/CreateNewFund/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FundID,FundName,SetUpDate,Location,Custodian,FundScale,StarRank,FundStatus")] FundMaster fundMaster)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fundMaster).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(fundMaster);
        }

        // GET: Fund/CreateNewFund/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundMaster fundMaster = db.FundMaster.Find(id);
            if (fundMaster == null)
            {
                return HttpNotFound();
            }
            return View(fundMaster);
        }

        // POST: Fund/CreateNewFund/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FundMaster fundMaster = db.FundMaster.Find(id);
            db.FundMaster.Remove(fundMaster);
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
