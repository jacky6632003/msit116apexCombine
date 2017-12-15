using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(LeavecategoryMetadata))]
    public partial class Leavecategory
    {
        public class LeavecategoryMetadata
        {
            [DisplayName("假別代號")]
            public int LeavecategoryID { get; set; }
            [DisplayName("假別")]
            public string LeavecategoryName { get; set; }
        }
    }
}