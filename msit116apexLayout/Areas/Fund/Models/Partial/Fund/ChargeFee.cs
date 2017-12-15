using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(ChargeFeeMetadata))]
	public partial class ChargeFee
	{
		public class ChargeFeeMetadata
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			public ChargeFeeMetadata()
			{
				this.FundGoods = new HashSet<FundGoods>();
			}
			[DisplayName("手續費編號")]
			public string ChargeFeeCode { get; set; }
			[DisplayName("手續費")]
			public double ChargeFeePercentage { get; set; }

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}