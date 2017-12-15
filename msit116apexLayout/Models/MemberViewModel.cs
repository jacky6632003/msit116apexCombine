using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class MemberViewModel
    {
            [Required]
            [DisplayName("帳號(電子郵件)")]
            [DataType(DataType.EmailAddress)]
            [EmailAddress]
            public string UserName { get; set; }
            [Required]
            [DisplayName("密碼")]
            public string Password { get; set; }
            [DisplayName("姓名")]
            public string Name { get; set; }
            [DisplayName("國籍")]
            public string Country { get; set; }
            [DisplayName("職業")]
            public string Title { get; set; }
            [DisplayName("身分證字號")]
            public string IdentityCardNumber { get; set; }
            [DisplayName("住籍地址")]
            public string ResidenceAddress { get; set; }
            [DisplayName("通訊地址")]
            public string MailingAddress { get; set; }
            [DisplayName("居住電話")]
            public string Telephone { get; set; }
            [DisplayName("生日")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
            public Nullable<System.DateTime> BirthDay { get; set; }
            [DisplayName("電子郵件")]
            public string Email { get; set; }
            [DisplayName("手機")]
            public string PhoneNumber { get; set; }
        
    }
}