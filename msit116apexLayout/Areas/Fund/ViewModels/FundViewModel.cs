using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace msit116apexLayout.Areas.Fund.ViewModels
{
	public class FundViewModel
	{
		public IEnumerable<SelectListItem> FundAreaList { get; set; }
		public IEnumerable<SelectListItem> FundTypeList { get; set; }
		public IEnumerable<SelectListItem> FundCompanyList { get; set; }
		public IEnumerable<SelectListItem> FundCurrencyList { get; set; }	
		public IEnumerable<SelectListItem> TradeCurrencyList { get; set; }
		public IEnumerable<SelectListItem> ChargeDateList { get; set; }
		public IEnumerable<FundGoods> FundGoods { get; set; }	
		public IEnumerable<FundArea> FundArea { get; set; }		
		public IEnumerable<FundType> FundType { get; set; }	
		public IEnumerable<FundCompany> FundCompany { get; set; }
		public IEnumerable<FundCurrency> FundCurrency { get; set; }
		public IEnumerable<FundDetail> FundDetail { get; set; }	
		public IEnumerable<FundMaster> FundMaster { get; set; }
		public IEnumerable<ShoppingCart> ShoppingCart { get; set; }
		public IEnumerable<FundTradeType> FundTradeType { get; set; }
		public IEnumerable<FundTradeCurrency> FundTradeCurrency { get; set; }
		public IEnumerable<FundOrder> FundOrder { get; set; }
		public IEnumerable<FundOrderDetail> FundOrderDetail { get; set; }
		public IEnumerable<ChargeFee> ChargeFee { get; set; }		
		public IEnumerable<FundTable> FundTable { get; set; }
		public IEnumerable<CartTable> CartTable { get; set; }
		public IEnumerable<OrderTypeA> OrderTypeA { get; set; }
		public IEnumerable<OrderTypeB> OrderTypeB { get; set; }
		public IEnumerable<FundChargeDate> FundChargeDate { get; set; }
		public IEnumerable<ShoppingCartAnalysis> CartAnalysis { get; set; }
		public IEnumerable<LikeTable> LikeTable { get; set; }
		public IEnumerable<FundInfo> FundInfo { get; set; }
		public IEnumerable<DaysValues> DaysValues { get; set; }
		public IEnumerable<FundOrderAccount> FundOrderAccountA { get; set; }
		public IEnumerable<FundOrderAccount> FundOrderAccountB { get; set; }
		//public bool A { get; set; }
		//public bool B { get; set; }


	}

	public class FundOrderAccount
	{
		public string GoodID { get; set; }
		[DisplayName("基金名稱")]
		public string FundName { get; set; }	
		public string TradeTypeName { get; set; }
		[DisplayName("申購價金")]
		public Nullable<int> TradePrice { get; set; }
		[DisplayName("手續費")]
		public decimal ChargeFeePercentage { get; set; }
		public string OrderDetailID { get; set; }
		[DisplayName("基金淨值")]
		public decimal NetValue { get; set; }
		public decimal boughtPrice { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }
		[DisplayName("扣款日期")]
		public string ChargeDateName { get; set; }
	}
	

	public class DaysValues
	{
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public System.DateTime tenDays { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public System.DateTime tenToTween { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public System.DateTime tweenToThreen { get; set; }
		public decimal tenDaysValues { get; set; }
		public decimal tenToTweenDays { get; set; }
		public decimal tweenToThreenDays { get; set; }
	}

	public class FundInfo
	{	
		public string FundName { get; set; }
		public string GoodID { get; set; }
		public string FundScale { get; set; }
		public decimal ValueChange { get; set; }
		public decimal ChangeRate { get; set; }
		public int StarRank { get; set; }
		public Nullable<int> ClickRate { get; set; }
		public int FundID { get; set; }
		[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
		public Nullable<System.DateTime> SetUpDate { get; set; }
		public string Location { get; set; }
		public string Custodian { get; set; }
		public string FundCpyName { get; set; }
		public string FundTypeName { get; set; }
		public string Description { get; set; }				
	}

	public class LikeTable
	{
		public string FundName { get; set; }
		public decimal NetValue { get; set; }
		public int FundID { get; set; }
		public string FundCurrencyCode { get; set; }
	}

	public class ShoppingCartAnalysis
	{		
		public string FundAreaCode { get; set; }
		public string FundTypeCode { get; set; }
		public int StarRank { get; set; }
		public string OrderID { get; set; }
		public string MemberID { get; set; }
		public Nullable<System.DateTime> OrderDate { get; set; }

	}


	public class OrderTypeA
	{
		public string RecordID { get; set; }
		public string FundName { get; set; }
		public string FundCpyName { get; set; }
		public string FundTypeName { get; set; }
		public decimal NetValue { get; set; }
		public string FundCurrencyCode { get; set; }
		[DisplayFormat(DataFormatString = "{0:MM/dd}"/*, ApplyFormatInEditMode = true*/)]
		public System.DateTime Date { get; set; }
		[DisplayName("交易幣別")]
		public string TradeCurrencyCode { get; set; }
		public string TradeBankCode { get; set; }
		public Nullable<int> TradePrice { get; set; }
		public Nullable<int> Amount { get; set; }
		public decimal ChargeFeePercentage { get; set; }
		public string TradeBankName { get; set; }
		public string BankAccountName { get; set; }

	}

	public class OrderTypeB
	{
		public string RecordID { get; set; }
		public string FundName { get; set; }
		public string FundCpyName { get; set; }
		public string FundTypeName { get; set; }
		public decimal NetValue { get; set; }
		public string FundCurrencyCode { get; set; }
		[DisplayFormat(DataFormatString = "{0:MM/dd}"/*, ApplyFormatInEditMode = true*/)]
		public System.DateTime Date { get; set; }
		public string TradeCurrencyCode { get; set; }
		public string TradeBankCode { get; set; }
		public Nullable<int> TradePrice { get; set; }
		//public Nullable<int> Amount { get; set; }
		public decimal ChargeFeePercentage { get; set; }
		public string TradeBankName { get; set; }
		public string BankAccountName { get; set; }
	}

	public class CartTable
	{
		public string RecordID { get; set; }
		public string FundName { get; set; }
		public string FundCpyName { get; set; }
		public string FundTypeName { get; set; }
		[DisplayName("風險報酬等級")]
		public int StarRank { get; set; }
		public decimal NetValue { get; set; }
		public System.DateTime Date { get; set; }
		public string FundCurrencyCode { get; set; }
		
	}

	public class FundTable
	{
		[DisplayName("基金名稱")]
		public string FundName { get; set; }
		[DisplayName("基金淨值")]
		public decimal NetValue { get; set; }
		//[DataType(DataType.Date)]
		[DisplayName("淨值日")]
		[DisplayFormat(DataFormatString = "{0:MM/dd}"/*, ApplyFormatInEditMode = true*/)]
		public System.DateTime Date { get; set; }
		public decimal ChangeValue { get; set; }
		[DisplayName("幣別")]
		public string FundCurrencyCode { get; set; }
		[DisplayName("基金規模(百萬)")]
		public string FundScale { get; set; }
		[DisplayName("點擊率")]
		public Nullable<int> ClickRate { get; set; }
		[DisplayName("風險報酬等級")]
		public int StarRank { get; set; }
		public string GoodID { get; set; }
		public decimal threeMonths { get; set; }
		public decimal sixMonths { get; set; }
		public decimal oneYear { get; set; }
		public decimal fiveYear { get; set; }
		public int FundID { get; set; }
		public decimal ValueChange { get; set; }
		public decimal ChangeRate { get; set; }
		//public string FundAreaCode { get; set; }
		//public string FundTypeCode { get; set; }

		//public string FundCpyCode { get; set; }
		//public string ChargeFeeCode { get; set; }	


		//public virtual ChargeFee ChargeFee { get; set; }
		//public virtual FundArea FundArea { get; set; }
		//public virtual FundCompany FundCompany { get; set; }
		//public virtual FundCurrency FundCurrency { get; set; }
		//public virtual FundMaster FundMaster { get; set; }
		//public virtual FundType FundType { get; set; }
		//public string FundCurrencyName { get; set; }		
	}
	
}