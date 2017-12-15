using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundTradeTypeMetadata))]
	public partial class FundTradeType
	{
		public class FundTradeTypeMetadata
		{

			public string TradeTypeCode { get; set; }
			[DisplayName("交易方式")]
			public string TradeTypeName { get; set; }

		}
	}
}