using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundTradeCurrencyMetadata))]
	public partial class FundTradeCurrency
	{
		public class FundTradeCurrencyMetadata
		{
			[DisplayName("交易幣別")]
			public string TradeCurrencyCode { get; set; }
			public string TradeCurrencyName { get; set; }
		}
	}
}