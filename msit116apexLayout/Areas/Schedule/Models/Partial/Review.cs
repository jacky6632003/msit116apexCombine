using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(ReviewMetadata))]
    public partial class Review
    {
        public class ReviewMetadata
        {
            [DisplayName("假單狀態代號")]
            public int ReviewID { get; set; }
            [DisplayName("假單狀態")]
            public string ReviewName { get; set; }

        }

    }
}