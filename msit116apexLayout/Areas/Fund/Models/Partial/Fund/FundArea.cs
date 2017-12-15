using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundAreaMetadata))]
	public partial class FundArea
	{
		public class FundAreaMetadata
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			public FundAreaMetadata()
			{
				this.FundCompany = new HashSet<FundCompany>();
				this.FundGoods = new HashSet<FundGoods>();
			}

			[DisplayName("區域代碼")]
			public string FundAreaCode { get; set; }
			[DisplayName("區域")]
			public string FundAreaName { get; set; }

			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundCompany> FundCompany { get; set; }
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}