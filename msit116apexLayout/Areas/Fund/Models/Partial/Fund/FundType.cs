using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundTypeMetadata))]
	public partial class FundType
	{
		public class FundTypeMetadata
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			public FundTypeMetadata()
			{
				this.FundGoods = new HashSet<FundGoods>();
			}

			[DisplayName("類型編號")]
			public string FundTypeCode { get; set; }
			[DisplayName("基金類型")]
			public string FundTypeName { get; set; }

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}