using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    [MetadataType(typeof(MemberMetadata))]
    public partial class AspNetUsers
    {
        public class MemberMetadata
        {
            [DisplayName("國家")]
            public string Country { get; set; }
            [DisplayName("照片")]
            public byte[] Photo { get; set; }
            [DisplayName("職業")]
            public string Title { get; set; }
            [DisplayName("身分證字號")]
            public string IdentityCardNumber { get; set; }
            [DisplayName("戶籍地址")]
            public string ResidenceAddress { get; set; }
            [DisplayName("通訊地址")]
            public string MailingAddress { get; set; }
            [DisplayName("居住電話")]
            public string Telephone { get; set; }
            [DisplayName("生日")]
            public System.DateTime? BirthDay { get; set; }
            [DisplayName("姓名")]
            public string Name { get; set; }
        }
    }

}