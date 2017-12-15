using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(DepartmentMetadata))]
    public partial class Department
    {

        public class DepartmentMetadata
        {
            [DisplayName("部門代號")]
            public int departmentID { get; set; }
            [DisplayName("部門")]
            public string departmentName { get; set; }
        }
       
    }
}