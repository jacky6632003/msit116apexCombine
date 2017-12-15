using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
	[MetadataType(typeof(FundMasterMetadata))]
	public partial class FundMaster
	{
		public class FundMasterMetadata
		{
			//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
			//public FundMasterMetadata()
			//{
			//	this.FundDetail = new HashSet<FundDetail>();
			//	this.FundDeploy = new HashSet<FundDeploy>();
			//	this.FundGoods = new HashSet<FundGoods>();
			//}

			
			[DisplayName("基金代碼")]
			public int FundID { get; set; }
		
			[DisplayName("基金名稱")]
			public string FundName { get; set; }
			
			[DisplayName("成立日期")]
			[DisplayFormat(DataFormatString = "{0:YY-MM-dd}", ApplyFormatInEditMode = true)]
			public Nullable<System.DateTime> SetUpDate { get; set; }
			[DisplayName("基金註冊地")]
			public string Location { get; set; }
			[DisplayName("總代理")]
			public string Custodian { get; set; }
			
			[DisplayName("基金規模(百萬)")]
			public Nullable<decimal> FundScale { get; set; }

			[DisplayName("風險報酬等級")]
			public int StarRank { get; set; }
			
			[DisplayName("狀態/是否上架")]
			public string FundStatus { get; set; }

			
			//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			
			//public virtual ICollection<FundDetail> FundDetail { get; set; }
			
			//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			
			//public virtual ICollection<FundDeploy> FundDeploy { get; set; }
			
			//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
			
			//public virtual ICollection<FundGoods> FundGoods { get; set; }
		}
	}
}