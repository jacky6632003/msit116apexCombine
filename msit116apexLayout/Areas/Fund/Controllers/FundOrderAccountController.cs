using msit116apexLayout.Areas.Fund.ViewModels;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Areas.Fund.Controllers
{
	[Authorize]
	public class FundOrderAccountController : Controller
    {
		private IRepository<FundOrderDetail> fOrderDetail = new Repository<FundOrderDetail>();
		private IRepository<FundGoods> fGoods = new Repository<FundGoods>();
		FundViewModel fvm = new FundViewModel();

		// GET: Fund/FundOrderAccount
		public ActionResult Index()
        {
			var accountA = from o in fOrderDetail.GetAll()
						   join g in fGoods.GetAll() on o.GoodID equals g.GoodID
						   where o.FundOrder.MemberID.Split('_')[1] == User.Identity.GetUserId() && o.TradeTypeCode == "A"
						   //先寫死
						   //where o.FundOrder.MemberID == "XXX" && o.TradeTypeCode == "A"
						   orderby o.FundOrder.OrderDate
						   group g by new { g.GoodID,g.FundMaster.FundName,o.FundTradeType.TradeTypeName,g.ChargeFee.ChargeFeePercentage,g.FundMaster.FundDetail} into fg
						  
			select new FundOrderAccount
			{
				GoodID = fg.Key.GoodID,
				FundName = fg.Key.FundName,
				TradeTypeName = fg.Key.TradeTypeName,
				TradePrice = fg.Sum(x=>x.ShoppingCart.First().TradePrice),
				ChargeFeePercentage = fg.Key.ChargeFeePercentage,
				NetValue = fg.Key.FundDetail.OrderByDescending(x=>x.Date).Select(x=>x.NetValue).FirstOrDefault(),	
				
				//boughtPrice =  fg.Key.FundDetail.OrderByDescending(x => x.Date).Where(x => DateTime.Compare(x.Date, fg.Key.Value) <= 0).FirstOrDefault().NetValue
				//boughtPrice = fg.Average(x=>x.FundMaster.FundDetail.OrderByDescending(y=>y.Date).Where(z => DateTime.Compare(z.Date, fg.Key.) <=0).FirstOrDefault().NetValue)

			};
			

			var accountB = from o in fOrderDetail.GetAll()
						   join g in fGoods.GetAll() on o.GoodID equals g.GoodID
						   where o.FundOrder.MemberID.Split('_')[1] == User.Identity.GetUserId() && o.TradeTypeCode == "B"
						   //先寫死
						   //where o.FundOrder.MemberID == "XXX" && o.TradeTypeCode == "B"						   
						   orderby o.FundOrder.OrderDate
						   select new FundOrderAccount
						   {
							   GoodID = g.GoodID,
							   FundName = g.FundMaster.FundName,
							   TradeTypeName = o.FundTradeType.TradeTypeName,
							   TradePrice = o.TradePrice,
							   ChargeFeePercentage = g.ChargeFee.ChargeFeePercentage,
							   OrderDetailID = o.OrderDetailID,
							   NetValue = g.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
							   ChargeDateName = o.FundChargeDate.ChargeDateName
						   };
			fvm.FundOrderAccountA = accountA;
			fvm.FundOrderAccountB = accountB;
			return View(fvm);
        }
    }
}