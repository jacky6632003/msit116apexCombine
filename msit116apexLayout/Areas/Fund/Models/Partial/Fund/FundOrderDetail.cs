using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundOrderDetailMetadata))]
	public partial class FundOrderDetail
	{
		public class FundOrderDetailMetadata
		{
			public string OrderDetailID { get; set; }
			public string OrderlID { get; set; }
			public string GoodID { get; set; }
		
			public string TradeTypeCode { get; set; }
			[DisplayName("申購價金")]
			public Nullable<int> TradePrice { get; set; }
			[DisplayName("總價金")]
			public Nullable<decimal> Amount { get; set; }
			public string ChargeDateCode { get; set; }
			[DisplayName("每次扣款金額")]
			public Nullable<decimal> TradePricePerTime { get; set; }
			public string BankAccountID { get; set; }

			
		}
	}

	
}