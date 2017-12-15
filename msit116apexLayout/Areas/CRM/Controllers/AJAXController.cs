using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

using msit116apexLayout.Areas.CRM.Models;
using Newtonsoft.Json;

namespace msit116apexLayout.Areas.CRM.Controllers
{
    public class AJAXController : Controller
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        UserDeteilDataModel ac = new UserDeteilDataModel();

        public JsonResult nameselect(string name, string Employeeid)
        {
            // var Ilist = db.InterviewRecords.Where(n => n.Interviewee_Name == name && n.EmployeeID == Employeeid);
            var Ilist = newInterview(name, Employeeid);
            return Json(Ilist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult IDselect(string id, string Employeeid)
        {
            var Ilist = newInterview(id, Employeeid);
            
            return Json(Ilist, JsonRequestBehavior.AllowGet);

        }

        //CRM Index
        public JsonResult Aspusers(string emid)
        {
            var a = db.CRM_subordinate.Where(n => n.EmploteeID == emid).Select(n => new { Name = n.Username, ID = n.UserID });
            return Json(a, JsonRequestBehavior.AllowGet);
        }
        //CRM Index
        public ActionResult changeUserInterview(string id)
        {
            var UserList = ac.GetUserByID(id);
            return Json(UserList, JsonRequestBehavior.AllowGet);
        }

        public IQueryable<InterviewRecordsDatas> newInterview(string id, string Employeeid)
        {
            var Ilist = db.InterviewRecords.Where(n => n.UserID == id && n.EmployeeID == Employeeid).Select(n => new InterviewRecordsDatas
            {
                InterviewID = n.InterviewID,
                UserID = n.UserID,
                EmployeeID = n.EmployeeID,
                Interviewer_Name = n.Interviewer_Name,
                Interviewee_Name = n.Interviewee_Name,
                C_Type_ID = n.C_Type_ID,
                StartTime = n.StartTime,
                C_Location = n.C_Location,
                InterviewContent = n.InterviewContent,
                CreateTime = n.CreateTime,
                LastModifyTime = n.LastModifyTime,
                TypeName = n.InterviewType.TypeName
            });
            return Ilist;
        }

        public ActionResult idDetail(int id)
        {
            var val = db.InterviewRecords.Find(id);
            InterviewRecordsDatas datas = new InterviewRecordsDatas
            {
                Interviewer_Name = val.Interviewer_Name,
                Interviewee_Name = val.Interviewee_Name,
                StartTime = val.StartTime,
                C_Location = val.C_Location,
                EmployeeID = val.EmployeeID,
                TypeName =val.InterviewType.TypeName,
                UserID = val.UserID,
                InterviewID = val.InterviewID,
                C_Type_ID = val.C_Type_ID,
                CreateTime = val.CreateTime,
                LastModifyTime = val.LastModifyTime,
                InterviewContent = val.InterviewContent
            };
            return Json(datas, JsonRequestBehavior.AllowGet);
        }
    }
}
