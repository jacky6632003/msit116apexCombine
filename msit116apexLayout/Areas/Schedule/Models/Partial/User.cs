using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(UserMetadata))]
    public partial class User
    {
        public class UserMetadata
        {
            [DisplayName("員工代號")]
            public int UserID { get; set; }
            [DisplayName("員工姓名")]
            public string UserName { get; set; }
            [DisplayName("部門")]
            public int DepartmentID { get; set; }
            [DisplayName("可以請假")]
            public int LeaveDay { get; set; }
            [DisplayName("主管")]
            public int SupervisorID { get; set; }
        }
    }
}