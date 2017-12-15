using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.ViewModels
{
    public class LeaveUserViewModel
    {
        public int ID { get; set; }
        public string Lav_ID { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public System.DateTime Lav_StartTime { get; set; }
        [DataType(DataType.Text)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm}")]
        public System.DateTime Lac_EndTime { get; set; }
        [DisplayName("假別")]
        public string Lac_LeavecategoryID { get; set; }
        
        public string Lac_Agent { get; set; }
        public string Lac_Tag { get; set; }
        
    }
}