using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using static msit116apexLayout.Models.InterviewRecords;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(InterviewTypeMD))]
    public partial class InterviewType
    {
        public class InterviewTypeMD
        {
            public int InterviewType_ID { get; set; }
            public string TypeName { get; set; }

            [JsonIgnore]
            public virtual ICollection<InterviewRecords> InterviewRecords { get; set; }
        }
    }
}