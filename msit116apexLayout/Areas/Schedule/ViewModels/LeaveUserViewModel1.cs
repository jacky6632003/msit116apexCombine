using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.ViewModels
{
    public class LeaveUserViewModel1
    {
        public IEnumerable<Leave> leave { get; set; }
        public IEnumerable<User> user { get; set; }
      

    }
}