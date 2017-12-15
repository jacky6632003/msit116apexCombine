using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using System.Text;

namespace msit116apexLayout.Areas.Fund.Controllers
{
    public class BackStageController : Controller
    {
        private MSIT116APEXEntities db = new MSIT116APEXEntities();

        // GET: Fund/BackStage
        public ActionResult Index()
        {
            var fundGoods = db.FundGoods.Where(f=>f.FundMaster.FundStatus =="Y").Include(f => f.ChargeFee).Include(f => f.FundArea).Include(f => f.FundCompany).Include(f => f.FundCurrency).Include(f => f.FundMaster).Include(f => f.FundType);
            return View(fundGoods.ToList());
        }

        // GET: Fund/BackStage/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundGoods fundGoods = db.FundGoods.Find(id);
            if (fundGoods == null)
            {
                return HttpNotFound();
            }
            return View(fundGoods);
        }

        // GET: Fund/BackStage/Create
        public ActionResult Create()
        {

			ViewBag.getGoodsId = GetGoodsId(5, '0', "left");

			ViewBag.ChargeFeeCode = new SelectList(db.ChargeFee, "ChargeFeeCode", "ChargeFeePercentage");
            ViewBag.FundAreaCode = new SelectList(db.FundArea, "FundAreaCode", "FundAreaName");
			//ViewBag.FundCpyCode = new SelectList(db.FundCompany, "FundCpyCode", "FundCpyName");
			ViewBag.FundCpyCode = new SelectList(db.FundCompany
											     .Select(c => new { FundCpyCode = c.FundCpyCode, DisplayName = c.FundArea.FundAreaName + "-" + c.FundCpyName, OrderByAreaCode = c.FundArea.FundAreaCode, ThenByCpyCode = c.FundCpyCode })
								                 .Distinct()
											     .OrderBy(c => c.OrderByAreaCode)
											     .ThenBy(c => c.ThenByCpyCode)
												  , "FundCpyCode", "DisplayName");
			ViewBag.FundCurrencyCode = new SelectList(db.FundCurrency, "FundCurrencyCode", "FundCurrencyName");
            ViewBag.FundID = new SelectList(db.FundMaster.Where(x=>x.FundStatus=="N"), "FundID", "FundName");
            ViewBag.FundTypeCode = new SelectList(db.FundType, "FundTypeCode", "FundTypeName");
            return View();
        }

		[HttpPost]
		public ActionResult GetCpyOption(string areaId)
		{
			StringBuilder sb = new StringBuilder();
			if (!string.IsNullOrWhiteSpace(areaId))
			{
				var goods = this.db.FundCompany
							.Where(x => x.FundAreaCode == areaId)
							.Select(a => new { FundCpyCode = a.FundCpyCode, FundAreaName = a.FundArea.FundAreaName, FundCpyName = a.FundCpyName, OrderByAreaCode = a.FundArea.FundAreaCode, ThenByCpyCode = a.FundCpyCode })
							.Distinct()
							.OrderBy(c => c.OrderByAreaCode)
							.ThenBy(c => c.ThenByCpyCode);

				foreach (var item in goods)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>",
							item.FundCpyCode.ToString(),
							string.Concat(item.FundAreaName, "-", item.FundCpyName));
				}
			}
			return Content(sb.ToString());
		}




		// POST: Fund/BackStage/Create
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GoodID,FundID,FundAreaCode,FundTypeCode,FundCurrencyCode,FundCpyCode,ChargeFeeCode,Description,ClickRate")] FundGoods fundGoods)
        {
            if (ModelState.IsValid)
            {

				/////////////////////////////////////////////////////////////////////////////////////////////////////////				
                db.FundGoods.Add(fundGoods);

				var a = (from m in db.FundMaster
						 where m.FundID == fundGoods.FundID
						 select m).FirstOrDefault();

				a.FundStatus = "Y";

				db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ChargeFeeCode = new SelectList(db.ChargeFee, "ChargeFeeCode", "ChargeFeeCode", fundGoods.ChargeFeeCode);
            ViewBag.FundAreaCode = new SelectList(db.FundArea, "FundAreaCode", "FundAreaName", fundGoods.FundAreaCode);
            ViewBag.FundCpyCode = new SelectList(db.FundCompany, "FundCpyCode", "FundCpyName", fundGoods.FundCpyCode);
            ViewBag.FundCurrencyCode = new SelectList(db.FundCurrency, "FundCurrencyCode", "FundCurrencyName", fundGoods.FundCurrencyCode);
            ViewBag.FundID = new SelectList(db.FundMaster, "FundID", "FundName", fundGoods.FundID);
            ViewBag.FundTypeCode = new SelectList(db.FundType, "FundTypeCode", "FundTypeName", fundGoods.FundTypeCode);
            return View(fundGoods);
        }

        // GET: Fund/BackStage/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundGoods fundGoods = db.FundGoods.Find(id);
            if (fundGoods == null)
            {
                return HttpNotFound();
            }
            ViewBag.ChargeFeeCode = new SelectList(db.ChargeFee, "ChargeFeeCode", "ChargeFeePercentage", fundGoods.ChargeFeeCode);
            ViewBag.FundAreaCode = new SelectList(db.FundArea, "FundAreaCode", "FundAreaName", fundGoods.FundAreaCode);
            ViewBag.FundCpyCode = new SelectList(db.FundCompany, "FundCpyCode", "FundCpyName", fundGoods.FundCpyCode);
            ViewBag.FundCurrencyCode = new SelectList(db.FundCurrency, "FundCurrencyCode", "FundCurrencyName", fundGoods.FundCurrencyCode);
            ViewBag.FundID = new SelectList(db.FundMaster, "FundID", "FundName", fundGoods.FundID);
            ViewBag.FundTypeCode = new SelectList(db.FundType, "FundTypeCode", "FundTypeName", fundGoods.FundTypeCode);
            return View(fundGoods);
        }

        // POST: Fund/BackStage/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoodID,FundID,FundAreaCode,FundTypeCode,FundCurrencyCode,FundCpyCode,ChargeFeeCode,Description,ClickRate")] FundGoods fundGoods)
        {
            if (ModelState.IsValid)
            {
                db.Entry(fundGoods).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ChargeFeeCode = new SelectList(db.ChargeFee, "ChargeFeeCode", "ChargeFeeCode", fundGoods.ChargeFeeCode);
            ViewBag.FundAreaCode = new SelectList(db.FundArea, "FundAreaCode", "FundAreaName", fundGoods.FundAreaCode);
            ViewBag.FundCpyCode = new SelectList(db.FundCompany, "FundCpyCode", "FundCpyName", fundGoods.FundCpyCode);
            ViewBag.FundCurrencyCode = new SelectList(db.FundCurrency, "FundCurrencyCode", "FundCurrencyName", fundGoods.FundCurrencyCode);
            ViewBag.FundID = new SelectList(db.FundMaster, "FundID", "FundName", fundGoods.FundID);
            ViewBag.FundTypeCode = new SelectList(db.FundType, "FundTypeCode", "FundTypeName", fundGoods.FundTypeCode);
            return View(fundGoods);
        }

        // GET: Fund/BackStage/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FundGoods fundGoods = db.FundGoods.Find(id);
            if (fundGoods == null)
            {
                return HttpNotFound();
            }
            return View(fundGoods);
        }

        // POST: Fund/BackStage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            FundGoods fundGoods = db.FundGoods.Find(id);
            db.FundGoods.Remove(fundGoods);

			var a = (from m in db.FundMaster
					 where m.FundID == fundGoods.FundID
					 select m).FirstOrDefault();

			a.FundStatus = "N";

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

		private string GetGoodsId(int IdLength, char setWord, string padType)
		{
			string strId = "";
			string GoodsId = "";

			if (db.FundGoods.Count() == 0)
			{
				GoodsId = "1";
			}
			else
			{
				int result = 0;
				if(int.TryParse(db.FundGoods.Max(m => m.GoodID), out result))
				{
					GoodsId = (result+1).ToString();
				}
			}
			

			switch (padType)
			{
				case "left":
					strId = GoodsId.ToString().PadLeft(IdLength, setWord);
					break;
				case "right":
					strId = GoodsId.ToString().PadRight(IdLength, setWord);
					break;
				default:
					break;
			}

			return strId;
		}
    }
}
