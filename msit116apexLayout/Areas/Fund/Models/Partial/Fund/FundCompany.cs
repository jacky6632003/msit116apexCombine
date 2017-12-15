using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundCompanyMetadata))]
	public partial class FundCompany
	{
		public class FundCompanyMetadata
		{
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			public FundCompanyMetadata()
			{
				this.FundGoods = new HashSet<FundGoods>();
			}
			[DisplayName("系列代碼")]
			public string FundCpyCode { get; set; }
			[DisplayName("基金系列")]
			public string FundCpyName { get; set; }
			public string FundAreaCode { get; set; }

			public virtual FundArea FundArea { get; set; }
			[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}