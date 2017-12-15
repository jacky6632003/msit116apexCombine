using msit116apexLayout.Areas.Fund.ViewModels;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using System.Text;

namespace msit116apexLayout.Areas.Fund.Controllers
{
	[Authorize]
	public class CartController : Controller
    {
		private IRepository<ShoppingCart> cart = new Repository<ShoppingCart>();
		private IRepository<FundTradeType> tradeType = new Repository<FundTradeType>();
		private IRepository<FundTradeCurrency> ftc = new Repository<FundTradeCurrency>();
		private IRepository<FundUserBankAccount> fbc = new Repository<FundUserBankAccount>();
		private IRepository<FundChargeDate> fcd = new Repository<FundChargeDate>();
		private IRepository<FundOrder> fundOrder = new Repository<FundOrder>();
		private IRepository<FundOrderDetail> fundOrderDetail = new Repository<FundOrderDetail>();

		MSIT116APEXEntities db = new MSIT116APEXEntities();
		// GET: Fund/Cart

		[AllowAnonymous]
		public ActionResult Index()
        {
            ////暫時測試用
            //Session["CartID"] = "11-27-2017 12:26_未登錄";
            if (Session["CartID"] != null)
            {
                var shoppingcart = from cart in cart.GetAll()
                                   where cart.CartID == Session["CartID"].ToString() && cart.IsOrdered == false

                                   select new CartTable
                                   {
                                       RecordID = cart.RecordID,
                                       FundName = cart.FundGoods.FundMaster.FundName,
                                       FundCpyName = cart.FundGoods.FundCompany.FundCpyName,
                                       FundTypeName = cart.FundGoods.FundType.FundTypeName,
                                       StarRank = cart.FundGoods.FundMaster.StarRank,
                                       NetValue = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
                                       Date = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
                                       FundCurrencyCode = cart.FundGoods.FundCurrencyCode
                                   };

                FundViewModel fundVM = new FundViewModel();

                fundVM.CartTable = shoppingcart;
                return View(fundVM);
            }
            else
            {
                return Content("<script>alert('請先進行購物');window.location='" + Url.Action("Index", "FundOverview", new { area = "Fund" }) + "'</script>");
            }
        }

		[AllowAnonymous]
		public ActionResult DeleteCart(string id)
		{
			cart.Delete(cart.GetByID(id));
			return RedirectToAction("Index");
		}
		
		public ActionResult DeleteCartFromAddToOrder(string id)
		{
			cart.Delete(cart.GetByID(id));
			return RedirectToAction("AddToOrder");
		}

		
		public ActionResult UpdateCart()
		{		
			var updateCartId = from c in cart.GetAll()
							   where c.CartID == Session["CartID"].ToString() && c.IsOrdered == false
							   select c;

			//AspNetUsers userId = db.AspNetUsers.Find(User.Identity.GetUserId());
			
			//Session["CartID"] = userId.ToString();

			foreach (var cartId in updateCartId)
			{
				cartId.CartID = DateTime.Now.ToString("MM-dd-yyyy hh:mm") + "_" + User.Identity.GetUserId();
				//暫時寫死
				//cartId.CartID = DateTime.Now.ToString("MM-dd-yyyy hh:mm") + "_XXX";
				cart.Update(cartId);
			}
			//暫時寫死
			//Session["CartID"] = DateTime.Now.ToString("MM-dd-yyyy hh:mm") + "_XXX";
			Session["CartID"] = DateTime.Now.ToString("MM-dd-yyyy hh:mm") + "_" + User.Identity.GetUserId();
			return RedirectToAction("UpdateTradeCode");
		}

		//暫時
		//[AllowAnonymous]
		public ActionResult UpdateTradeCode()
		{	   
			if(Session["CartID"]==null)
			{
				return RedirectToAction("Index", "FundOverview");
			}
	     	FundViewModel fundVM = new FundViewModel();

			var shoppingcart = from cart in cart.GetAll()
							   where cart.CartID == Session["CartID"].ToString() && cart.IsOrdered == false
							   //暫時寫死
							   //where cart.CartID == "XXX"
							   select new CartTable
							   {
								   RecordID = cart.RecordID,
								   FundName = cart.FundGoods.FundMaster.FundName,
								   FundCpyName = cart.FundGoods.FundCompany.FundCpyName,
								   FundTypeName = cart.FundGoods.FundType.FundTypeName,
								   StarRank = cart.FundGoods.FundMaster.StarRank,
								   NetValue = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
								   Date = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
								   FundCurrencyCode = cart.FundGoods.FundCurrencyCode,							   
							   };

			var fundTradeType = from type in tradeType.GetAll()
								select type;
								
			fundVM.CartTable = shoppingcart;
			fundVM.FundTradeType = fundTradeType;
			return View(fundVM);
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult UpdateTradeCode(FormCollection form)
		{            
			foreach (var RadioName in form.AllKeys)
			{
				string value = form[RadioName];

				var shoppingCart = from c in cart.GetAll()
								   where c.RecordID == RadioName && c.IsOrdered == false
								   select c;
				
				foreach(var type in shoppingCart)
				{
					type.TradeTypeCode = value;
					cart.Update(type);
				}
			}
			return RedirectToAction("AddToOrder");
		}
		
		public ActionResult AddToOrder()
		{
			if (Session["CartID"] == null)
			{
				return RedirectToAction("Index", "FundOverview");
			}
			FundViewModel fundVM = new FundViewModel();			

			var typeA = from cart in cart.GetAll()												
						where cart.CartID == Session["CartID"].ToString() && cart.TradeTypeCode == "A" && cart.IsOrdered ==false
						select new OrderTypeA
						{
							RecordID = cart.RecordID,
							FundName = cart.FundGoods.FundMaster.FundName,
							FundCpyName = cart.FundGoods.FundCompany.FundCpyName,
							FundTypeName = cart.FundGoods.FundType.FundTypeName,
							NetValue = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
							Date = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
						    ChargeFeePercentage = cart.FundGoods.ChargeFee.ChargeFeePercentage,
							FundCurrencyCode = cart.FundGoods.FundCurrencyCode			
						};

			var typeB = from cart in cart.GetAll()													
						where cart.CartID == Session["CartID"].ToString() && cart.TradeTypeCode == "B" && cart.IsOrdered == false
						select new OrderTypeB
						{
							RecordID = cart.RecordID,
							FundName = cart.FundGoods.FundMaster.FundName,
							FundCpyName = cart.FundGoods.FundCompany.FundCpyName,
							FundTypeName = cart.FundGoods.FundType.FundTypeName,
							NetValue = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
							Date = cart.FundGoods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.Date).FirstOrDefault(),
							ChargeFeePercentage = cart.FundGoods.ChargeFee.ChargeFeePercentage,
							FundCurrencyCode = cart.FundGoods.FundCurrencyCode,						
						};			

			fundVM.OrderTypeA = typeA;
			fundVM.OrderTypeB = typeB;
			
			return View(fundVM);
		}

		[AllowAnonymous]
		public ActionResult GetCurrencyOption()
		{
			StringBuilder sb = new StringBuilder();
			var currency = from c in ftc.GetAll()
						   select new
						   {
							   TradeCurrencyCode = c.TradeCurrencyCode,
							   TradeCurrencyName = c.TradeCurrencyName
						   };

			foreach(var item in currency)
			{
				 sb.AppendFormat("<option value=\"{0}\">{1}</option>", item.TradeCurrencyCode, item.TradeCurrencyName);
			}			  
			return Content(sb.ToString());
		}

		[AllowAnonymous]
		public ActionResult GetChargeDateOption()
		{
			StringBuilder sb = new StringBuilder();
			var currency = from d in fcd.GetAll()
						   select new
						   {
							   ChargeDateCode = d.ChargeDateCode,
							   ChargeDateName = d.ChargeDateName
						   };

			foreach (var item in currency)
			{
				sb.AppendFormat("<option value=\"{0}\">每月 {1}號</option>", item.ChargeDateCode, item.ChargeDateName);
			}
			return Content(sb.ToString());
		}

		[AllowAnonymous]
		[HttpPost]
		public ActionResult GetBankAccount(string currencyId)
		{
			StringBuilder sb = new StringBuilder();
			var account = from a in fbc.GetAll()
						  where a.TradeCurrencyCode == currencyId
						  select new
						   {
							   FundTradeBank = a.FundTradeBank.TradeBankName,
							   BankAccountName = a.BankAccountName
						   };
		
			foreach(var item in account)
				sb.AppendFormat("<div>{0}-{1}</div>", item.FundTradeBank, item.BankAccountName);
			
			return Content(sb.ToString());
		}
	
		[HttpPost]

		public ActionResult AddToOrder(FormCollection form)
		{
			if (Session["CartID"] == null)
			{
				return RedirectToAction("Index", "FundOverview");
			}
			List<ShoppingCart> cartList = new List<ShoppingCart>();
			List<string> checkRecordIDList = new List<string>();

			foreach (var RadioName in form.AllKeys)
			{
			   string recordId = RadioName.Split('_')[1];
							
				if(!checkRecordIDList.Contains(recordId))
					checkRecordIDList.Add(recordId);
				
			}
			foreach (var recordId in checkRecordIDList)
			{
				ShoppingCart shoppingcart = (from shop in db.ShoppingCart
											 where shop.RecordID == recordId && shop.IsOrdered == false
											 select shop).FirstOrDefault();

				shoppingcart.IsOrdered = true;
				cartList.Add(shoppingcart);

			}
			foreach (var RadioName in form.AllKeys)
			{
				string recordId = RadioName.Split('_')[1];
				foreach (ShoppingCart items in cartList)
				{

					if (items.RecordID == recordId)
					{

						string value = form[RadioName];
						string controlName = RadioName.Split('_')[0];


						switch (controlName)
						{
							case "currencyA":
								var bankAccount = from a in fbc.GetAll()
												  where a.TradeCurrencyCode == value
												  select a.BankAccountID;
								foreach (var account in bankAccount)
								{
									items.BankAccountID = account;
								}

								break;
							case "textA":
								int result = 0;
								if (int.TryParse(value, out result))
								{
									items.TradePrice = result;
									items.Amount = result + (result * items.FundGoods.ChargeFee.ChargeFeePercentage/100);
								}

								break;
							case "date":
								items.ChargeDateCode = value;

								break;
							case "currencyB":
								var bankAccount2 = from a in fbc.GetAll()
												   where a.TradeCurrencyCode == value
												   select a.BankAccountID;
								foreach (var account in bankAccount2)
								{
									items.BankAccountID = account;
								}

								break;
							case "textB":
								int result2 = 0;
								if (int.TryParse(value, out result2))
								{
									items.TradePrice = result2;
									items.TradePricePerTime = result2 + (result2 * items.FundGoods.ChargeFee.ChargeFeePercentage/100);
								}

								break;
						}
					}
				}

				db.SaveChanges();									
			}


			return RedirectToAction("CreatOrder");
		}
		
		public ActionResult CreatOrder()
		{			
			FundOrder fo = new FundOrder();
			fo.OrderID= GetOrderId(5, '0', "left");

			var order = from c in cart.GetAll()
						where c.CartID == Session["CartID"].ToString() && c.IsOrdered == true							
						select new FundOrder
						{						
							//MemberID= Session["CartID"].ToString().Split('_')[1],
							MemberID = Session["CartID"].ToString(),
							OrderDate = System.Convert.ToDateTime(DateTime.Now.ToString("s"))
						};

			foreach( var item in order)
			{
				fo.MemberID = item.MemberID;
				fo.OrderDate = item.OrderDate;
			}
			
			fundOrder.Create(fo);

			return RedirectToAction("CreatOrderDetail");
		}

		public ActionResult CreatOrderDetail()
		{
			

			var orderdetail = from c in cart.GetAll()
							  join o in fundOrder.GetAll() on c.CartID equals o.MemberID
							  //where c.CartID == Session["CartID"].ToString() && c.CartID.Split('_')[1] == o.MemberID && c.IsOrdered == true
							  where c.CartID == Session["CartID"].ToString() &&  o.MemberID == Session["CartID"].ToString() && c.IsOrdered == true
							  ////暫時寫死
							  //where c.CartID == "XXX"
							  select new FundOrderDetail
							  {
								  OrderlID = o.OrderID,
								  GoodID = c.GoodID,
								  TradeTypeCode = c.TradeTypeCode,
								  TradePrice = c.TradePrice,
								  Amount = c.Amount,
								  ChargeDateCode = c.ChargeDateCode,
								  TradePricePerTime = c.TradePricePerTime,
								  BankAccountID = c.BankAccountID
							  };

			foreach( var item in orderdetail)
			{
				FundOrderDetail fod = new FundOrderDetail();

				fod.OrderDetailID = GetOrderDetailId(5, '0', "left");
				fod.OrderlID = item.OrderlID;
				fod.GoodID = item.GoodID;
				fod.TradeTypeCode = item.TradeTypeCode;
				fod.TradePrice = item.TradePrice;
				fod.Amount = item.Amount;
				fod.ChargeDateCode = item.ChargeDateCode;
				fod.TradePricePerTime = item.TradePricePerTime;
				fod.BankAccountID = item.BankAccountID;

				fundOrderDetail.Create(fod);
			}

			
			return RedirectToAction("DeleteShoppingCart");
		}

		public ActionResult DeleteShoppingCart()
		{
			var shoppingCart = from c in cart.GetAll()
							   join o in fundOrder.GetAll() on c.CartID equals o.MemberID
							   where c.CartID == Session["CartID"].ToString() && c.IsOrdered == false
							   select c;

			foreach(var item in shoppingCart)
			{
				cart.Delete(item);
			}

			return RedirectToAction("Index","FundOverview");
		}

		private string GetOrderId(int IdLength, char setWord, string padType)
		{
			string strId = "";
			string OrderId = "";

			if (db.FundOrder.Count() == 0)
			{
				OrderId = "1";
			}
			else
			{
				int result = 0;
				if (int.TryParse(db.FundOrder.Max(m => m.OrderID), out result))
				{
					OrderId = (result + 1).ToString();
				}
			}


			switch (padType)
			{
				case "left":
					strId = OrderId.ToString().PadLeft(IdLength, setWord);
					break;
				case "right":
					strId = OrderId.ToString().PadRight(IdLength, setWord);
					break;
				default:
					break;
			}

			return strId;
		}

		private string GetOrderDetailId(int IdLength, char setWord, string padType)
		{
			string strId = "";
			string OrderDetailId = "";

			if (db.FundOrderDetail.Count() == 0)
			{
				OrderDetailId = "1";
			}
			else
			{
				int result = 0;
				if (int.TryParse(db.FundOrderDetail.Max(m => m.OrderDetailID), out result))
				{
					OrderDetailId = (result + 1).ToString();
				}
			}

			switch (padType)
			{
				case "left":
					strId = OrderDetailId.ToString().PadLeft(IdLength, setWord);
					break;
				case "right":
					strId = OrderDetailId.ToString().PadRight(IdLength, setWord);
					break;
				default:
					break;
			}

			return strId;
		}

        [AllowAnonymous]
        public JsonResult GetCartNum()
        {
            string userID = "";
            if (Session["CartID"] != null)
                userID = Session["CartID"].ToString();
            IEnumerable<ShoppingCart> result = null;
            int result2 = 0;
            if (Session["CartID"] != null)
            {
                result = db.ShoppingCart.Where(n => n.CartID == userID&&n.IsOrdered==false);
                result2 = result.Count();
            }
            return Json(result2, JsonRequestBehavior.AllowGet);
        }
    }
}