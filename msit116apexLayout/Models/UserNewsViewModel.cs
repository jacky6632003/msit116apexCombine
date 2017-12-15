using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class UserNewsViewModel
    {
        public UserNews UserNews { get; set; }
        public IEnumerable<UserNewsUrls> UserNewsUrlss { get; set; }
        public string FromUserName { get; set; }
    }
}