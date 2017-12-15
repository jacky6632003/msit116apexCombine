using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace msit116apexLayout.Areas.CRM.Models
{
    public class InterviewRecordVM
    {
        public int InterviewID { get; set; }
        public string UserID { get; set; }
        public string EmployeeID { get; set; }
        public string Interviewer_Name { get; set; }
        public string Interviewee_Name { get; set; }
        public string InterviewReason { get; set; }
        public Nullable<int> C_Type_ID { get; set; }
        public Nullable<System.DateTime> StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public string C_Location { get; set; }
        public string InterviewContent { get; set; }
        public Nullable<System.DateTime> CreateTime { get; set; }
        public Nullable<System.DateTime> LastModifyTime { get; set; }
    }
    public class ITypeViewModel
    {
        public int InterviewType_ID { get; set; }
        public string TypeName { get; set; }
    }

    public class InterviewTypeVM
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public InterviewRecordVM GetInterview_ID(string Id)
        {
            //InterviewRecords IT = db.InterviewRecords.Where(n => n.UserID == Id).FirstOrDefault();
            ////List<InterviewRecordVM> LIV = new List<InterviewRecordVM>();  
            //    InterviewRecordVM result = new InterviewRecordVM
            //    {
            //        InterviewID = IT.InterviewID,
            //        UserID = IT.UserID,
            //        EmployeeID = IT.EmployeeID,
            //        Interviewer_Name = IT.Interviewer_Name,
            //        Interviewee_Name = IT.Interviewee_Name,
            //        InterviewReason = IT.InterviewReason,
            //        C_Type_ID = IT.C_Type_ID,
            //        StartTime = IT.StartTime,
            //        EndTime = IT.EndTime,
            //        C_Location = IT.C_Location,
            //        InterviewContent = IT.InterviewContent,
            //        CreateTime = IT.CreateTime,
            //        LastModifyTime = IT.LastModifyTime,
            //    };
            //     //LIV.Add(result);

            //return result;
            return null;
        }
    }
}