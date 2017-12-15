using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.CRM.Models
{
    public class UsersDataVM
    {
        public IEnumerable<UserData> UserData { get; set; }
        //public IEnumerable<UserLevelData> UserLevelData { get; set; }
        public IEnumerable<InterviewRecordsDatas> InterviewRecordsDatas { get; set;}
    }

    public class InterviewRecordsDatas
    {
        public int InterviewID { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string Interviewer_Name { get; set; }
        public string Interviewee_Name { get; set; }
        public Nullable<int> C_Type_ID { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public string C_Location { get; set; }
        public string InterviewContent { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
        public string TypeName { get; set; }
    }

    public class UserData
    {
        public string UserName { get; set; }
        public string UserID { get; set; }
        public string Name { get; set; }
        public Nullable<System.DateTime> BirthDay { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string IdentityCardNumber { get; set; }
        public string MailingAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string ResidenceAddress { get; set; }
        public string Telephone { get; set; }
        public string Title { get; set; }
        public bool IsEmp { get; set; }
    }

    //public class UserLevelData
    //{
    //    public string AspUserId { get; set; }
    //    public string UserLevel1 { get; set; }
    //    public decimal Deposit { get; set; }
    //}

    public class subordinate
    {
        public int Id { get; set; }
        public string EmploteeID { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
    }

    public class MessageRecord
    {
        
    }

    public class Dlist
    {
        public string Id { get; set; }
    }

    public class UserDeteilDataModel
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public UserData GetUserByID(string UserName)
        {
            AspNetUsers aspuserData = db.AspNetUsers.Where(u => u.Id == UserName).First();
            UserData result = new UserData
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
                IsEmp = false
            };
           
            IsEmployee x = db.IsEmployee.Where(c => c.UserId == UserName).FirstOrDefault();
            if (x != null)
                result.IsEmp = true;
            return result;
        }

        //public UserLevelData GetUserLevelID(string UserName)
        //{
        //    UserLevel userLevel = db.UserLevel.Where(u => u.AspUserId == UserName).First();
        //    UserLevelData result1 = new UserLevelData
        //    {
        //        AspUserId = userLevel.AspUserId,
        //        UserLevel1 = userLevel.UserLevel1,
        //        Deposit = (decimal)userLevel.Deposit
        //    };
        //return result1;
        //}

    }
}