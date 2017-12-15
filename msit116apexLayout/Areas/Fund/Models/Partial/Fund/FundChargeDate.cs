using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundChargeDateMetadata))]
	public partial class FundChargeDate
	{
		public class FundChargeDateMetadata
		{
			public string ChargeDateCode { get; set; }
			[DisplayName("扣款日期")]
			public string ChargeDateName { get; set; }
		}
	}
}