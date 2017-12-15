using msit116apexLayout.Areas.Fund.ViewModels;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Areas.Fund.Controllers
{
	public class FundOverviewController : Controller
	{
		private MSIT116APEXEntities db = new MSIT116APEXEntities();
		private IRepository<FundArea> area = new Repository<FundArea>();
		private IRepository<FundType> type = new Repository<FundType>();
		private IRepository<FundGoods> goods = new Repository<FundGoods>();
		private IRepository<FundDetail> detail = new Repository<FundDetail>();
		private IRepository<FundMaster> master = new Repository<FundMaster>();
		private IRepository<ShoppingCart> cart = new Repository<ShoppingCart>();
		private IRepository<FundDeploy> deploy = new Repository<FundDeploy>();
		FundViewModel fundView = new FundViewModel();
		// GET: Fund/FundOverview
        [AllowAnonymous]
		public ActionResult Index()
		{
			ViewBag.idd = User.Identity.GetUserId();
			var fundArea = area.GetAll();
			var fundType = type.GetAll();			
			var fundGoods = goods.GetAll();

			SelectList areaSelectList = new SelectList(fundArea, "FundAreaCode", "FundAreaName");
			SelectList typeSelectList = new SelectList(fundGoods									   
							                           .Select(t => new { FundTypeCode = t.FundTypeCode, FundTypeName = t.FundType.FundTypeName, OrderByTypeCode = t.FundType.FundTypeCode })
							                           .Distinct()
							                           .OrderBy(t => t.OrderByTypeCode)
				                                       , "FundTypeCode", "FundTypeName");
			SelectList companySelectList = new SelectList(fundGoods
														  .Select(c => new { FundCpyCode = c.FundCpyCode, DisplayName = c.FundArea.FundAreaName + "-" + c.FundCompany.FundCpyName, OrderByAreaCode = c.FundArea.FundAreaCode, ThenByCpyCode = c.FundCompany.FundCpyCode })
														  .Distinct()
														  .OrderBy(c => c.OrderByAreaCode)
														  .ThenBy(c => c.ThenByCpyCode)
														  , "FundCpyCode", "DisplayName");
			SelectList currencySelectList = new SelectList(fundGoods
														   .Select(c => new { FundCurrencyCode = c.FundCurrencyCode, FundCurrencyName = c.FundCurrency.FundCurrencyName, OrderByCryCode = c.FundCurrency.FundCurrencyCode })
														   .Distinct()
														   .OrderBy(c => c.OrderByCryCode)
														   , "FundCurrencyCode", "FundCurrencyName");
			
				fundView.FundAreaList = areaSelectList;
				fundView.FundTypeList = typeSelectList;
		     	fundView.FundCompanyList = companySelectList;
		    	fundView.FundCurrencyList = currencySelectList;
				fundView.FundGoods = fundGoods;
			
			return View(fundView);
		}

		public ActionResult GetAreaOptioin()
		{
			StringBuilder sb = new StringBuilder();
			var areas = this.area.GetAll()
						.Select(a => new { FundAreaCode = a.FundAreaCode, FundAreaName = a.FundAreaName });
			foreach (var item in areas)
			{
				sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundAreaCode, item.FundAreaName);
			}
			return Content(sb.ToString());
		}



		//取得Type Options
		[HttpPost]
		public ActionResult GetTypeOption(string areaId)
		{
			StringBuilder sb = new StringBuilder();

			if(!string.IsNullOrWhiteSpace(areaId) && areaId != "0")
			{
				var goods = this.goods.GetAll()
							.Where(a => (areaId == "0") || a.FundAreaCode == areaId)
							.Select(t => new { FundTypeCode = t.FundTypeCode, FundTypeName = t.FundType.FundTypeName, OrderByTypeCode = t.FundType.FundTypeCode })
							.Distinct()
							.OrderBy(t => t.OrderByTypeCode);
				foreach (var item in goods)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundTypeCode, item.FundTypeName);
				}
			}
			else
			{
				var goods = this.goods.GetAll()							
							.Select(t => new { FundTypeCode = t.FundTypeCode, FundTypeName = t.FundType.FundTypeName, OrderByTypeCode = t.FundType.FundTypeCode })
							.Distinct()
							.OrderBy(t => t.OrderByTypeCode);
				foreach (var item in goods)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundTypeCode, item.FundTypeName);
				}
			}
			return Content(sb.ToString());
		}


		//取得Company Options
		[HttpPost]
		public ActionResult GetCpyOption(string areaId, string typeId)
		{
			StringBuilder sb = new StringBuilder();

			if ((!string.IsNullOrWhiteSpace(areaId) && areaId != "0") || (!string.IsNullOrWhiteSpace(typeId) && typeId != "0"))
			{
				var goods = this.goods.GetAll()
							.Where(x => (areaId == "0" || x.FundAreaCode == areaId) && (typeId == "0" || x.FundTypeCode == typeId))
							.Select(a => new { FundCpyCode = a.FundCpyCode, FundAreaName = a.FundArea.FundAreaName, FundCpyName = a.FundCompany.FundCpyName, OrderByAreaCode = a.FundArea.FundAreaCode, ThenByCpyCode = a.FundCompany.FundCpyCode })
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
			else
			{
				var goods = this.goods.GetAll()
						   .Select(c => new { FundCpyCode = c.FundCpyCode, DisplayName = c.FundArea.FundAreaName + "-" + c.FundCompany.FundCpyName, OrderByAreaCode = c.FundArea.FundAreaCode, ThenByCpyCode = c.FundCompany.FundCpyCode })
						   .Distinct()
						   .OrderBy(c => c.OrderByAreaCode)
						   .ThenBy(c => c.ThenByCpyCode);

				foreach (var item in goods)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundCpyCode, item.DisplayName);
				}
			}
			return Content(sb.ToString());
		}

		//取得Currency Options
		[HttpPost]
		public ActionResult GetCryOption(string areaId, string typeId, string cpyId)
		{
			StringBuilder sb = new StringBuilder();

			if ((!string.IsNullOrWhiteSpace(areaId) && areaId != "0") || (!string.IsNullOrWhiteSpace(typeId) && typeId != "0") || (!string.IsNullOrWhiteSpace(cpyId) && cpyId != "0"))
			{
				var goods = this.goods.GetAll()
						   .Where(x => (areaId == "0" || x.FundAreaCode == areaId) && (typeId == "0" || x.FundTypeCode == typeId) && (cpyId == "0" || x.FundCpyCode == cpyId))
						   .Select(c => new { FundCurrencyCode = c.FundCurrencyCode, FundCurrencyName = c.FundCurrency.FundCurrencyName, OrderByCryCode = c.FundCurrency.FundCurrencyCode })
						   .Distinct()
						   .OrderBy(c => c.OrderByCryCode);
				foreach (var item in goods)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundCurrencyCode, item.FundCurrencyName);
				}
			}
			else
			{
				var currency = this.goods.GetAll()
							  .Select(c => new { FundCurrencyCode = c.FundCurrencyCode, FundCurrencyName = c.FundCurrency.FundCurrencyName, OrderByCryCode = c.FundCurrency.FundCurrencyCode })
							  .Distinct()
							  .OrderBy(c => c.OrderByCryCode);
				foreach (var item in currency)
				{
					sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.FundCurrencyCode.ToString(), item.FundCurrencyName);
				};
			}
			return Content(sb.ToString());
		}

		
		public ActionResult FundTable(int? page, string sortBy)
		{
			//var fundGoods = goods.GetAll()
			//	.Select(x => x.FundMaster.FundDetail.OrderByDescending(d => d.Date).Select(n => n.NetValue).First());
			//fundView.FundGoods = fundGoods;

			var fundtable = from g in this.goods.GetAll()
							join d in this.detail.GetAll() on g.FundID equals d.FundID into t
							select new FundTable()
							{
								FundName = g.FundMaster.FundName,
								NetValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
								Date = t.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
								ValueChange = t.OrderByDescending(x => x.Date).Select(x => x.ValueChange).FirstOrDefault(),
								//ChangeValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault() - t.OrderByDescending(x => x.Date).Skip(1).Select(x => x.NetValue).FirstOrDefault(),
								FundCurrencyCode = g.FundCurrencyCode,
								FundScale = g.FundMaster.FundScale,
								ClickRate = g.ClickRate,
								StarRank = g.FundMaster.StarRank,
								GoodID = g.GoodID,
								threeMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.ValueChange),
								sixMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-6)) >= 0).Sum(x => x.ValueChange),
								oneYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-1)) >= 0).Sum(x => x.ValueChange),
							    fiveYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-5)) >= 0).Sum(x => x.ValueChange),
								FundID = g.FundID
								//threeMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3).AddDays(1)) >= 0).Sum(x => x.NetValue)
								//            - t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.NetValue)
								//			+ t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),							
							};
			fundView.FundTable = fundtable.ToList().ToPagedList(page ?? 1, 20);

			return PartialView(fundtable.ToList().ToPagedList(page ?? 1, 20));
		}

		[HttpPost]
		public ActionResult FundTable(string areaId, string typeId, string cpyId, string cryId, int? page, string sortBy, string searchFund)
		{
			
			if ((!string.IsNullOrWhiteSpace(areaId) && areaId != "0") || (!string.IsNullOrWhiteSpace(typeId) && typeId != "0") || (!string.IsNullOrWhiteSpace(cpyId) && cpyId != "0") || (!string.IsNullOrWhiteSpace(cryId) && cryId != "0") ||(!string.IsNullOrWhiteSpace(searchFund) && searchFund !="0"))
			{
				
				//var fundGoods = goods.GetAll()
				//			   .Where(x => (areaId == "0" || x.FundAreaCode == areaId) && (typeId == "0" || x.FundTypeCode == typeId) && (cpyId == "0" || x.FundCpyCode == cpyId) && (cryId == "0" || x.FundCurrencyCode == cryId));
				var fundtable = from g in this.goods.GetAll()
								join d in this.detail.GetAll() on g.FundID equals d.FundID into t
								where (areaId == "0" || g.FundAreaCode == areaId) && (typeId == "0" || g.FundTypeCode == typeId) && (cpyId == "0" || g.FundCpyCode == cpyId) && (cryId == "0" || g.FundCurrencyCode == cryId) && (searchFund=="" || g.FundMaster.FundName.Contains(searchFund))  
								select new FundTable
								{
									FundName = g.FundMaster.FundName,
									NetValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
									Date = t.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
									ValueChange = t.OrderByDescending(x => x.Date).Select(x => x.ValueChange).FirstOrDefault(),
									//ChangeValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault() - t.OrderByDescending(x => x.Date).Skip(1).Select(x => x.NetValue).FirstOrDefault(),
									FundCurrencyCode = g.FundCurrencyCode,
									FundScale = g.FundMaster.FundScale,
									ClickRate = g.ClickRate,
									StarRank = g.FundMaster.StarRank,
									GoodID = g.GoodID,
									threeMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.ValueChange),
									sixMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-6)) >= 0).Sum(x => x.ValueChange),
									oneYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-1)) >= 0).Sum(x => x.ValueChange),
									fiveYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-5)) >= 0).Sum(x => x.ValueChange),
									FundID = g.FundID
									//threeMonths =t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3).AddDays(1)) >= 0).Sum(x => x.NetValue)
									//		   - t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.NetValue)
									//		   + t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),

								};
				//fundView.FundTable = fundtable;
				var aaa = 1;
				return PartialView(fundtable.ToList().ToPagedList(page ?? 1, 20));
			}
			else
			{
				//var fundGoods = goods.GetAll();
				//fundView.FundGoods = fundGoods;
				var fundtable = from g in this.goods.GetAll()
								join d in this.detail.GetAll() on g.FundID equals d.FundID into t
								select new FundTable
								{
									FundName = g.FundMaster.FundName,
									NetValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
									Date = t.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
									ValueChange = t.OrderByDescending(x => x.Date).Select(x => x.ValueChange).FirstOrDefault(),
									//ChangeValue = t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault() - t.OrderByDescending(x => x.Date).Skip(1).Select(x => x.NetValue).FirstOrDefault(),
									FundCurrencyCode = g.FundCurrencyCode,
									FundScale = g.FundMaster.FundScale,
									ClickRate = g.ClickRate,
									StarRank = g.FundMaster.StarRank,
									GoodID = g.GoodID,
									threeMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.ValueChange),
									sixMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-6)) >= 0).Sum(x => x.ValueChange),
									oneYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-1)) >= 0).Sum(x => x.ValueChange),
									fiveYear = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddYears(-5)) >= 0).Sum(x => x.ValueChange),
									FundID = g.FundID
									//threeMonths = t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3).AddDays(1)) >= 0).Sum(x => x.NetValue)
									//		- t.Where(x => DateTime.Compare(x.Date, DateTime.Today.AddMonths(-3)) >= 0).Sum(x => x.NetValue)
									//		+ t.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),

								};
				//fundView.FundTable = fundtable;
				return PartialView(fundtable.ToList().ToPagedList(page ?? 1, 20));
			}		
			//return PartialView(fundtable.ToList().ToPagedList(page ?? 1, 20));
		}
	
		public ActionResult fundInfo(int id)
		{
			var Info = from g in goods.GetAll()
					   where g.FundID == id
					   select new FundInfo
					   {
						   FundName = g.FundMaster.FundName,
						   GoodID = g.GoodID,
						   FundScale = g.FundMaster.FundScale,
						   ValueChange = g.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.ValueChange).FirstOrDefault(),
						   ChangeRate = g.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.ChangeRate).FirstOrDefault(),
						   StarRank = g.FundMaster.StarRank,
						   ClickRate = g.ClickRate,
						   FundID = g.FundID,
						   Custodian = g.FundMaster.Custodian,
						   SetUpDate = g.FundMaster.SetUpDate,
						   Location = g.FundMaster.Location,
						   FundCpyName = g.FundCompany.FundCpyName,
						   FundTypeName = g.FundType.FundTypeName,
						   Description = g.Description,					   
					   };

			var dateValue = from d in detail.GetAll()
							where d.FundID == id 
						    orderby d.Date descending							
							select new DaysValues
							{
								tenDays = d.Date,
								tenDaysValues = d.NetValue
							};

			

			fundView.FundInfo = Info;
			fundView.DaysValues = dateValue;
			return View(fundView);
		}

		public ActionResult CreateFund()
		{
			return View();
		}

	   public ActionResult ShowFundDetails(int id)
		{
			return View();
		}

		
		public ActionResult AddToCart(string id)
		{
								
			if (Session["CartID"] == null)
			{
				Session["CartID"] = DateTime.Now.ToString("MM-dd-yyyy hh:mm")+"_未登錄";
			}

			var countGoodsId = (from c in cart.GetAll()
					            where c.CartID == Session["CartID"].ToString() && c.GoodID == id
					            select c.GoodID).Count();

			if (countGoodsId == 0)
			{
				ShoppingCart addToCart = new ShoppingCart();
				addToCart.RecordID = GetRecordId(3, '0', "left");
				addToCart.CartID = Session["CartID"].ToString();
				addToCart.GoodID = id;
				addToCart.Quantity = 1;
				addToCart.DateOrdered = System.Convert.ToDateTime(DateTime.Now.ToString("s"));
				addToCart.IsOrdered = false;
				cart.Create(addToCart);
			}


			return RedirectToAction("Index");


		}

		private string GetRecordId(int IdLength, char setWord, string padType)
		{
			string strId = "";
			string RecordId = "";

			if (db.ShoppingCart.Count() == 0)
			{
				RecordId = "1";
			}
			else
			{
				int result = 0;
				if (int.TryParse(db.ShoppingCart.Max(m => m.RecordID), out result))
				{
					RecordId = (result + 1).ToString();
				}
			}


			switch (padType)
			{
				case "left":
					strId = RecordId.ToString().PadLeft(IdLength, setWord);
					break;
				case "right":
					strId = RecordId.ToString().PadRight(IdLength, setWord);
					break;
				default:
					break;
			}

			return strId;
		}

		
		public JsonResult ValueChart(int id=0)
		{
			if(id==0)
			{
				id = 1;
			}
			var data = from d in detail.GetAll()
					   where d.FundID == id
					   select new
					   {
						   Year = d.Date.Year,
						   month = d.Date.Month,
						   date = d.Date.Day,
						   NetValue = d.NetValue
					   };
			return Json(data, JsonRequestBehavior.AllowGet);
		}


		public JsonResult fundDeploy(int id=0)
		{
			var data = from d in deploy.GetAll()
					   where d.FundID == id
					   select new
					   {
						   StockPT = d.StockPT,
						   BondPT = d.BondPT,
						   CashPT = d.CashPT,
						   OtherPT = d.OtherPT
					   };
			return Json(data, JsonRequestBehavior.AllowGet);
		}











		public JsonResult test3()
		{
			var data = from d in deploy.GetAll()
					   where d.FundID == 1
					   select new
					   {
						   StockPT = d.StockPT,
						   BondPT = d.BondPT,
						   CashPT = d.CashPT,
						   OtherPT= d.OtherPT
					   };		
			return Json(data, JsonRequestBehavior.AllowGet);		
		}







		public ActionResult text()
		{		
			return View();
		}

		public JsonResult test1()
		{
			

			var data = from d in detail.GetAll()
						where d.FundID == 1
						select new
						{
							Year = d.Date.Year,
							month = d.Date.Month,
							date = d.Date.Day,
							NetValue = d.NetValue
						};

			return Json(data, JsonRequestBehavior.AllowGet);
		}
	}
}
