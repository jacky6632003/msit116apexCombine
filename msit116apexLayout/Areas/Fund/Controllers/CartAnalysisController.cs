using Microsoft.AspNet.Identity;
using msit116apexLayout.Areas.Fund.ViewModels;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace msit116apexLayout.Areas.Fund.Controllers
{
	[Authorize]
	public class CartAnalysisController : Controller
    {
		MSIT116APEXEntities db = new MSIT116APEXEntities();

		private IRepository<ShoppingCart> cart = new Repository<ShoppingCart>();
		private IRepository<AspNetUsers> users = new Repository<AspNetUsers>();
		private IRepository<FundGoods> goods = new Repository<FundGoods>();
		// GET: Fund/CartAnalysis

		//暫時不用登入
        //[AllowAnonymous]
		public ActionResult Index()
        {
			//if (Session["CartID"] == null)
			//{
			//	//Session["CartID"] = DateTime.Now.ToString("MM-dd-yyyy hh:mm") + "_" + db.AspNetUsers.Find(User.Identity.GetUserId());
			//	Session["CartID"] = "11-27-2017 05:30_XXX";
			//}
			FundViewModel fvm = new FundViewModel();
			AspNetUsers userId = db.AspNetUsers.Find(User.Identity.GetUserId());

			
			var cartitems = from u in users.GetAll()
							join c in cart.GetAll() on u.Id equals c.CartID.Split('_')[1]
							where c.CartID.Split('_')[1] == User.Identity.GetUserId()
                            //from c in cart.GetAll()
                            //where c.CartID == "11-28-2017 10:09_XXX"
                            select new ShoppingCartAnalysis
					   {
						   FundAreaCode = c.FundGoods.FundAreaCode,
						   FundTypeCode = c.FundGoods.FundTypeCode,
						   StarRank = c.FundGoods.FundMaster.StarRank					   						   
					   };
			
			int countA = 0;
			int countB = 0;
			int count_03 = 0;
			int count_01 = 0;
			int count_02 = 0;
			int count_04 = 0;
			int countR1 = 0;
			int countR2 = 0;
			int countR3 = 0;
			int countR4 = 0;
			int countR5 = 0;

			string maxAreaA = null;
			string maxAreaB = null;
			string maxType01 = null;
			string maxType02 = null;
			string maxType03 = null;
			string maxType04 = null;
			int maxRate1 = 0;
			int maxRate2 = 0;
			int maxRate3 = 0;
			int maxRate4 = 0;
			int maxRate5 = 0;

			foreach (var item in cartitems)
			{
				switch (item.FundAreaCode)
				{
					case "A":
						countA++;
						break;
					case "B":
						countB++;
						break;
				}
				switch(item.FundTypeCode)
				{
					case "01":
						count_01++;
						break;
					case "02":
						count_02++;
						break;
					case "03":
						count_03++;
						break;
					case "04":
						count_04++;
						break;
				}
				switch(item.StarRank)
				{
					case 1:
						countR1++;
						break;
					case 2:
						countR2++;
						break;
					case 3:
						countR3++;
						break;
					case 4:
						countR4++;
						break;
					case 5:
						countR5++;
						break;
				}				
			}

			if (countA > countB)
			{
				maxAreaA = "A";
			}
			else if(countB>countA)
			{
				maxAreaB = "B";
			}
			else
			{
				maxAreaA = "A";
				maxAreaB = "B";
			}

			List<int> typeCount = new List<int>();
			typeCount.Add(count_01);
			typeCount.Add(count_02);
			typeCount.Add(count_03);
			typeCount.Add(count_04);

			if (count_01 == typeCount.Max())
				maxType01 = "01";
			if (count_02 == typeCount.Max())
				maxType02 = "02";
			if (count_03 == typeCount.Max())
				maxType03 = "03";
			if (count_04 == typeCount.Max())
				maxType04 = "04";

			List<int> rateCount = new List<int>();
			rateCount.Add(countR1);
			rateCount.Add(countR2);
			rateCount.Add(countR3);
			rateCount.Add(countR4);
			rateCount.Add(countR5);

			if (countR1 == rateCount.Max())
				maxRate1 = 1;
			if (countR2 == rateCount.Max())
				maxRate2 = 2;
			if (countR3 == rateCount.Max())
				maxRate3 = 3;
			if (countR4 == rateCount.Max())
				maxRate4 = 4;
			if (countR5 == rateCount.Max())
				maxRate5 = 5;


			var like = from goods in goods.GetAll()
					   where (goods.FundAreaCode == maxAreaA || goods.FundAreaCode==maxAreaB)
					   && (goods.FundTypeCode == maxType01 || goods.FundTypeCode == maxType02 || goods.FundTypeCode == maxType03 || goods.FundTypeCode == maxType04)
					   && (goods.FundMaster.StarRank == maxRate1 || goods.FundMaster.StarRank == maxRate2 || goods.FundMaster.StarRank == maxRate3 || goods.FundMaster.StarRank == maxRate4 || goods.FundMaster.StarRank == maxRate5)
					   select new LikeTable
					   {
						   FundName = goods.FundMaster.FundName,
						   NetValue = goods.FundMaster.FundDetail.OrderByDescending(x => x.Date).Select(x => x.NetValue).FirstOrDefault(),
						   FundCurrencyCode = goods.FundCurrencyCode,
						   FundID = goods.FundID
					   };

			int count = like.Count();
			int index = new Random().Next(count);
            //var exLike = from c in cart.GetAll()
            //             where c.CartID.Split('_')[1] == User.Identity.GetUserId()
            //             select c;
            var randomLike = like.Skip(index).Take(5);

			fvm.LikeTable = randomLike;
				return PartialView(fvm);
			

			

		}
    }
}