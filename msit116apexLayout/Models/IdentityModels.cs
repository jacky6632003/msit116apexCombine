using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel;

namespace msit116apexLayout.Models
{
    // 您可將更多屬性新增至 ApplicationUser 類別，藉此為使用者新增設定檔資料，如需深入了解，請瀏覽 https://go.microsoft.com/fwlink/?LinkID=317594。
    public class ApplicationUser : IdentityUser
    {
        [DisplayName("國家")]
        public string Country { get; set; }
        public byte[] Photo { get; set; }
        public int State { get; set; }
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
        //NEW
        public bool IsGoogleAuthenticatorEnabled { get; set; }
        public string GoogleAuthenticatorSecretKey { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // 注意 authenticationType 必須符合 CookieAuthenticationOptions.AuthenticationType 中定義的項目
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // 在這裡新增自訂使用者宣告
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("ConnectionStringMSIT116APEX", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}