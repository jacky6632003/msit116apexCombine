using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.Stock.Models.Partial
{
    [MetadataType(typeof(PerformanceModel))]
    public partial class PerformanceModel
    {
        [DisplayName("部門編號")]
        public int departmentID { get; set; }

        [DisplayName("部門名稱")]
        public string departmentName { get; set; }

        [DisplayName("科別名稱")]
        public string uRoleName { get; set; } //

        [DisplayName("使用者姓名")]
        public string UserName { get; set; } //


        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DisplayName("未實現損益")]
        public decimal? unrealizedgainsorlosses { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DisplayName("已實現損益")]
        public decimal realizedgainsorlosses { get; set; }

        [DisplayName("員工帳號")]
        public IEnumerable<EMPPRModel> EMPPR { get; set; }
       
    }

    [MetadataType(typeof(EMPPRModel))]
    public partial class EMPPRModel
    {
        [DisplayName("員工帳號")]
        public string _UserName { get; set; }


        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DisplayName("未實現損益(員工)")]
        public decimal? _unrealizedgainsorlosses { get; set; }

        [DisplayFormat(DataFormatString = "{0:C0}", ApplyFormatInEditMode = true)]
        [DisplayName("已實現損益(員工)")]
        public decimal _realizedgainsorlosses { get; set; }
        
    }


}