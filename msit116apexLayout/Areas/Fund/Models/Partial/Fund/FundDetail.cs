using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundDetailMetadata))]
	public partial class FundDetail
	{
		public class FundDetailMetadata
		{
			[DisplayName("基金代碼")]
			public int FundID { get; set; }
			[DisplayName("淨值日")]
			[DataType(DataType.Date)]
			[DisplayFormat(DataFormatString ="{0:MM/dd}", ApplyFormatInEditMode = true)]
			public System.DateTime Date { get; set; }
			[DisplayName("基金淨值")]
			public decimal NetValue { get; set; }
			[DisplayName("漲跌")]
			public decimal ValueChange { get; set; }
			[DisplayName("漲跌幅 %")]
			public decimal ChangeRate { get; set; }

			public virtual FundMaster FundMaster { get; set; }
		}
	}
}