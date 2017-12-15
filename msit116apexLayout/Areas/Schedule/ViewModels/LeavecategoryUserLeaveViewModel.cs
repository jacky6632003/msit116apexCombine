using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace msit116apexLayout.ViewModels
{
    public class LeavecategoryUserLeaveViewModel
    {
        public IEnumerable<Leave> leave { get; set; }
        public IEnumerable<Leavecategory>  leavecategory { get; set; }
        public IEnumerable<Department> department { get; set; }
        public IEnumerable<Review> review { get; set; }

        public IEnumerable<User>  user { get; set; }

       
    }
}