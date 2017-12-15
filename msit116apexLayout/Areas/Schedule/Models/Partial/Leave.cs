using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;


namespace msit116apexLayout.Models
{
    [MetadataType(typeof(LeaveResultMetadata))]
    public partial class Leave
    {
        public class LeaveResultMetadata
        {
       
        public int ID { get; set; }
            [DisplayName("員工姓名")]

            public int Lav_ID { get; set; }
            [DisplayName("起始時間")]
            [DataType(DataType.Text)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
            public System.DateTime Lav_StartTime { get; set; }
            [DisplayName("結束時間")]
            [DataType(DataType.Text)]
            [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}")]
            public System.DateTime Lac_EndTime { get; set; }
            [DisplayName("假別")]
            public int Lac_LeavecategoryID { get; set; }
            [DisplayName("代理人")]
            public int Lac_Agent { get; set; }
            [DisplayName("內容")]
            [Required]
            public string Lac_Tag { get; set; }

            [DisplayName("附件")]
            public string LeaveImage { get; set; }
            [DisplayName("附件")]
            public byte[] BytesImage { get; set; }

            [DisplayName("審查狀態")]
            public int Lav_ReviewID { get; set; }
            [DisplayName("主管留言")]
            public string LavSupervisor { get; set; }

            public virtual Leavecategory Leavecategory { get; set; }
            public virtual User User { get; set; }
          
        }
    }
}

