using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Models
{
    public class AccountMethod
    {
        Repository<AspNetUsers> dbUser = null;
        MSIT116APEXEntities db = null;
        public AccountMethod()
        {
            dbUser = new Repository<AspNetUsers>();
            db = new MSIT116APEXEntities();
        }
        public UserDataModel GetUserByID(string UserName)
        {
            AspNetUsers aspuserData = db.AspNetUsers.Where(u => u.UserName == UserName).First();
            UserDataModel result = new UserDataModel
            {
                UserName = aspuserData.UserName,
                Name = aspuserData.Name,
                BirthDay = aspuserData.BirthDay,
                Country = aspuserData.Country,
                Email = aspuserData.Email,
                IdentityCardNumber = aspuserData.IdentityCardNumber,
                MailingAddress = aspuserData.MailingAddress,
                PhoneNumber = aspuserData.PhoneNumber,
                ResidenceAddress = aspuserData.ResidenceAddress,
                Telephone = aspuserData.Telephone,
                Title = aspuserData.Title,
                IsEmp=false
            };
            IsEmployee x = db.IsEmployee.Where(c => c.UserId == UserName).FirstOrDefault();
            if (x != null)
                result.IsEmp = true;
            return result;
        }
        public IEnumerable<UserDataModel> GetUsersData()
        {
            IEnumerable<AspNetUsers> aspuserData = db.AspNetUsers;
            IEnumerable<UserDataModel> result = db.AspNetUsers.Select(c => new UserDataModel
            {
                UserName = c.UserName,
                Name = c.Name,
                BirthDay = c.BirthDay,
                Country = c.Country,
                Email = c.Email,
                IdentityCardNumber = c.IdentityCardNumber,
                MailingAddress = c.MailingAddress,
                PhoneNumber = c.PhoneNumber,
                ResidenceAddress = c.ResidenceAddress,
                Telephone = c.Telephone,
                Title = c.Title,
                IsEmp = false
            }).ToList();
            foreach(UserDataModel u in result)
            {
                IsEmployee x = db.IsEmployee.Where(c => c.UserId == u.UserName).FirstOrDefault();
                if (x!=null)
                    u.IsEmp = true;
            }
            return result;
        }

        public IEnumerable<UserDataModel> GetEmployeesData()
        {
            IEnumerable<AspNetUsers> aspuserData = db.AspNetUsers;
            IEnumerable<UserDataModel> result = db.AspNetUsers.Select(c => new UserDataModel
            {
                UserName = c.UserName,
                Name = c.Name,
                BirthDay = c.BirthDay,
                Country = c.Country,
                Email = c.Email,
                IdentityCardNumber = c.IdentityCardNumber,
                MailingAddress = c.MailingAddress,
                PhoneNumber = c.PhoneNumber,
                ResidenceAddress = c.ResidenceAddress,
                Telephone = c.Telephone,
                Title = c.Title,
                IsEmp = false
            }).ToList();
            foreach (UserDataModel u in result)
            {
                IsEmployee x = db.IsEmployee.Where(c => c.UserId == u.UserName).FirstOrDefault();
                if (x != null)
                    u.IsEmp = true;
            }
            result = result.Where(c => c.IsEmp == true);
            return result;
        }

        public IEnumerable<UserDataModel> GetMembersData()
        {
            IEnumerable<AspNetUsers> aspuserData = db.AspNetUsers;
            IEnumerable<UserDataModel> result = db.AspNetUsers.Select(c => new UserDataModel
            {
                UserName = c.UserName,
                Name = c.Name,
                BirthDay = c.BirthDay,
                Country = c.Country,
                Email = c.Email,
                IdentityCardNumber = c.IdentityCardNumber,
                MailingAddress = c.MailingAddress,
                PhoneNumber = c.PhoneNumber,
                ResidenceAddress = c.ResidenceAddress,
                Telephone = c.Telephone,
                Title = c.Title,
                IsEmp = false
            }).ToList();
            foreach (UserDataModel u in result)
            {
                IsEmployee x = db.IsEmployee.Where(c => c.UserId == u.UserName).FirstOrDefault();
                if (x != null)
                    u.IsEmp = true;
            }
            result = result.Where(c => c.IsEmp == false);
            return result;
        }

        public void UpdateUser(UserDataModel _entity)
        {
            AspNetUsers aspuserData = db.AspNetUsers.Where(u => u.UserName == _entity.UserName).First();
            aspuserData.Name = _entity.Name;
            aspuserData.BirthDay = _entity.BirthDay;
            aspuserData.Country = _entity.Country;
            aspuserData.Email = _entity.Email;
            aspuserData.IdentityCardNumber = _entity.IdentityCardNumber;
            aspuserData.MailingAddress = _entity.MailingAddress;
            aspuserData.PhoneNumber = _entity.PhoneNumber;
            aspuserData.ResidenceAddress = _entity.ResidenceAddress;
            aspuserData.Telephone = _entity.Telephone;
            aspuserData.Title = _entity.Title;
            
            dbUser.Update(aspuserData);
        }
        public void UpdateUserWithoutNull(UserDataModel _entity)
        {
            AspNetUsers aspuserData = db.AspNetUsers.Where(u => u.UserName == _entity.UserName).First();
            aspuserData.Name = _entity.Name;
            aspuserData.BirthDay = _entity.BirthDay;
            aspuserData.Country = _entity.Country;
            aspuserData.Email = _entity.Email;
            aspuserData.IdentityCardNumber = _entity.IdentityCardNumber;
            aspuserData.MailingAddress = _entity.MailingAddress;
            aspuserData.PhoneNumber = _entity.PhoneNumber;
            aspuserData.ResidenceAddress = _entity.ResidenceAddress;
            aspuserData.Telephone = _entity.Telephone;
            aspuserData.Title = _entity.Title;

            dbUser.UpdateWithoutNull(aspuserData);
        }
        public void DeleteUserByID(string UserName)
        {
            db.AspNetUsers.Remove(db.AspNetUsers.Where(u=>u.UserName==UserName).First());
            db.IsEmployee.Remove(db.IsEmployee.Find(UserName));
            db.SaveChanges();
        }
        
    }

    public class UserDataModel
    {
        [DisplayName("帳號ID")]
        public string UserName { get; set; }
        [DisplayName("姓名")]
        public string Name { get; set; }
        [DisplayName("生日")]
        public Nullable<System.DateTime> BirthDay { get; set; }
        [DisplayName("國家")]
        public string Country { get; set; }
        [DisplayName("電子郵件")]
        public string Email { get; set; }
        [DisplayName("身分證字號")]
        public string IdentityCardNumber { get; set; }
        [DisplayName("通訊地址")]
        public string MailingAddress { get; set; }
        [DisplayName("手機")]
        public string PhoneNumber { get; set; }
        [DisplayName("戶籍地址")]
        public string ResidenceAddress { get; set; }
        [DisplayName("居住電話")]
        public string Telephone { get; set; }
        [DisplayName("職業")]
        public string Title { get; set; }
        [DisplayName("是不是員工")]
        public bool IsEmp { get; set; }
    }
}