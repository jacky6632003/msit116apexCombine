using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundCurrencyMetadata))]
	public partial class FundCurrency
	{
		public class FundCurrencyMetadata
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			public FundCurrencyMetadata()
			{
				this.FundGoods = new HashSet<FundGoods>();
			}

			[DisplayName("貨幣編號")]
			public string FundCurrencyCode { get; set; }
			[DisplayName("貨幣名稱")]
			public string FundCurrencyName { get; set; }

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}