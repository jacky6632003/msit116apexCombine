using msit116apexLayout.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(InterviewRecordMD))]
    public partial class InterviewRecords
    {
        public class InterviewRecordMD
        {
            
            [DisplayName("ID")]
            [Key]
            public int InterviewID { get; set; }
            [DisplayName("客戶ID")]
            public string UserID { get; set; }
            [DisplayName("員工姓名")]
            public string Interviewer_Name { get; set; }
            [DisplayName("客戶姓名")]
            public string Interviewee_Name { get; set; }
            [DisplayName("訪問類型")]
            public Nullable<int> C_Type_ID { get; set; }
            [DisplayName("訪問時間")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
            public DateTime StartTime { get; set; }
            [DisplayName("地點")]
            public string C_Location { get; set; }
            [DisplayName("內容")]
            public string InterviewContent { get; set; }

            [DisplayName("建表時間")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
            public DateTime CreateTime { get; set; }

            [DisplayName("最後修改日期")]
            [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
            public DateTime LastModifyTime { get; set; }


            public virtual InterviewType InterviewType { get; set; }
        }

    }
}