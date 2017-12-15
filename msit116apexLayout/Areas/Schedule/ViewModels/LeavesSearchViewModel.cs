using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.Schedule.ViewModels
{
    public class LeavesSearchViewModel
    {

        public int ID { get; set; }
        public string Lav_ID { get; set; }
      
        public System.DateTime Lav_StartTime { get; set; }
     
        public System.DateTime Lac_EndTime { get; set; }
   
        public string Lac_LeavecategoryID { get; set; }

        public string Lac_Agent { get; set; }
        public string Lac_Tag { get; set; }

        public string Department { get; set; }
      
        public string Lav_ReviewID { get; set; }
    }
}