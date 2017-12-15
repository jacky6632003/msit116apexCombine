using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundGoodsMetadata))]
	public partial class FundGoods
	{
		public class FundGoodsMetadata
		{
			[DisplayName("產品編號")]
			public int GoodID { get; set; }
			[DisplayName("基金代碼")]
			public int FundID { get; set; }
			public string FundAreaCode { get; set; }
			public string FundTypeCode { get; set; }
			[DisplayName("貨幣名稱")]
			public string FundCurrencyCode { get; set; }
			public string FundCpyCode { get; set; }
			public string ChargeFeeCode { get; set; }
			[DisplayName("基金描述")]
			[DataType(DataType.MultilineText)]
			public string Description { get; set; }
			[DisplayName("點擊率")]
			public Nullable<int> ClickRate { get; set; }

			//public virtual ChargeFee ChargeFee { get; set; }
			//public virtual FundArea FundArea { get; set; }
			//public virtual FundCompany FundCompany { get; set; }
			//public virtual FundCurrency FundCurrency { get; set; }
			//public virtual FundMaster FundMaster { get; set; }
			//public virtual FundType FundType { get; set; }
		}
	}
}