using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using Microsoft.AspNet.Identity;

namespace msit116apexLayout.Controllers
{
    [Authorize]
    public class PowerConfirmController : Controller
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        Repository<uRolePowerConfirm> dbuRolePowerConfirm = new Repository<uRolePowerConfirm>();
        // GET: PowerConfirm
        public ActionResult Index()
        {            
            return View();
        }

        public PartialViewResult ViewRoleConfirm(int roleID)
        {
            List<uPowerWithBoolPage> upwbpList = new List<uPowerWithBoolPage>();
            IEnumerable<uRoles> uRoles = db.uRoles.ToList();
            IEnumerable<uPages> uPages = db.uPages.ToList();
            IEnumerable<uPowers> uPowers = db.uPowers.ToList();
            IEnumerable<uRolePowers> uRolePowers = db.uRolePowers.ToList();
            IEnumerable<uRolePowerConfirm> uRolePowerConfirm = db.uRolePowerConfirm.ToList();
            IEnumerable<uRolePowerConfirmRole> uRolePowerConfirmRole = db.uRolePowerConfirmRole.ToList();
            IEnumerable<Department> department = db.Department.ToList();
            foreach (var upage in uPages)
            {
                List<uPowerWithBool> upwbList = new List<uPowerWithBool>();
                foreach (var upower in uPowers.Where(n => n.pageID == upage.pageID))
                {
                    bool haspower = (from c in uRolePowers
                                     where c.uRoleID == roleID && c.powerID == upower.powerID
                                     select c.powerID).Count() > 0;

                    IEnumerable<int> croleList = from urp in uRolePowers
                                                 join urpc in uRolePowerConfirm on urp.urpSn equals urpc.uRolePowerSn
                                                 join urpcr in uRolePowerConfirmRole on urpc.ucrSn equals urpcr.uRolePowerConfirmSn
                                                 where urp.uRoleID == roleID && urp.powerID == upower.powerID
                                                 select urpcr.uConfirmRoleID;
                    List<uRolesDpmt> uRolesDpmtCList = new List<uRolesDpmt>();
                    foreach (var crole in croleList)
                    {
                        uRolesDpmt urd = (from ur in uRoles
                                          join d in department on ur.departmentID equals d.departmentID
                                          where ur.uRoleID == crole
                                          select new uRolesDpmt
                                          {
                                              uRoles = ur,
                                              Department = d.departmentName
                                          }).FirstOrDefault();
                        uRolesDpmtCList.Add(urd);
                    }
                    uPowerWithBool uPowerWithBool = new uPowerWithBool {
                        uPowers = upower,
                        hasPower = haspower,
                        rolesConfirm = uRolesDpmtCList
                    };
                    upwbList.Add(uPowerWithBool);
                }
                uPowerWithBoolPage upwbp = new uPowerWithBoolPage
                {
                    uPages = upage,
                    uPowerWithBool = upwbList
                };
                upwbpList.Add(upwbp);
            }

            return PartialView(upwbpList);
        }

        [HttpGet]
        public PartialViewResult EditRoleConfirm(int upowerID)
        {
            uRolePowerConfirm urpcList = (from c in db.uRolePowerConfirm
                                            join x in db.uRolePowers on c.uRolePowerSn equals x.urpSn
                                            where x.powerID == upowerID
                                            select c).FirstOrDefault();

            return PartialView(urpcList);
        }

        [HttpPost]
        public ActionResult EditRoleConfirm(uRolePowerConfirm model, FormCollection form)
        {
            string result = "";
            if (model.uRolePowerSn != -1)
            {
                uRolePowerConfirm urpc = db.uRolePowerConfirm.Where(n => n.uRolePowerSn == model.uRolePowerSn).ToList().FirstOrDefault();
                urpc.uEachRoleMinNum = model.uEachRoleMinNum;
                urpc.uTotlaRoleMinNum = model.uTotlaRoleMinNum;                
            }
            else
            {
                int roleID = Convert.ToInt32(form["roleID"]);
                int powerID = Convert.ToInt32(form["powerID"]);
                int urpsn = db.uRolePowers.Where(n => n.uRoleID == roleID && n.powerID == powerID).Select(n => n.urpSn).ToList().FirstOrDefault();
                uRolePowerConfirm urpc = new uRolePowerConfirm {
                    uRolePowerSn=urpsn,
                    uEachRoleMinNum=model.uEachRoleMinNum,
                    uTotlaRoleMinNum=model.uTotlaRoleMinNum
                };
                db.uRolePowerConfirm.Add(urpc);
            }
            db.SaveChanges();
            result = "角色覆核人數設定更新成功";
            return Content(result);
        }
        
        public JsonResult EditRoleConfirmRole(int urpSn)
        {
            IEnumerable<uRolesWithDpmtSurName> result = urpSn == -1 ? null : (from cr in db.uRolePowerConfirmRole
                                                               join rc in db.uRolePowerConfirm on cr.uRolePowerConfirmSn equals rc.ucrSn
                                                               join r in db.uRoles on cr.uConfirmRoleID equals r.uRoleID
                                                               join d in db.Department on r.departmentID equals d.departmentID
                                                               where rc.uRolePowerSn == urpSn
                                                               select new uRolesWithDpmtSurName
                                                               {
                                                                   uRoles =r,
                                                                   Department =d.departmentName
                                                               });
            
            List<uRolesWithDpmtSurName> resultli = new List<uRolesWithDpmtSurName>();
            if (result!=null)
            {
                IEnumerable<uRoles> uRoleO = db.uRoles.ToList();
                foreach (var rff in result)
                {
                    if (rff.uRoles.supervisorID.HasValue)
                    {
                        rff.uRoleSurName = uRoleO.Where(n => n.uRoleID == rff.uRoles.supervisorID.Value).Select(n => n.uRoleName).FirstOrDefault();
                    }
                    else
                    {
                        rff.uRoleSurName = "無";
                    }
                    resultli.Add(rff);
                }
            }
            return Json(resultli, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult EditRoleConfirmRole(FormCollection form)
        {
            string result = "";
            int roleID = Convert.ToInt32(form["roleID"]);
            int powerID = Convert.ToInt32(form["powerID"]);
            List<int> editRoleConfirmRoleList = new List<int>();
            foreach (var key in form.AllKeys)
            {
                if (key.Contains("crID"))
                {
                    editRoleConfirmRoleList.Add(int.Parse(form.Get(key)));
                }
            }
            int urpsn = db.uRolePowers.Where(n => n.uRoleID == roleID && n.powerID == powerID).Select(n => n.urpSn).ToList().FirstOrDefault();
            int ucrsn = db.uRolePowerConfirm.Where(n => n.uRolePowerSn==urpsn).Select(n => n.ucrSn).ToList().FirstOrDefault();
            IEnumerable<uRolePowerConfirmRole> urpcrs = from rc in db.uRolePowerConfirm
                                                          join cr in db.uRolePowerConfirmRole on rc.ucrSn equals cr.uRolePowerConfirmSn
                                                          where rc.uRolePowerSn == urpsn
                                                          select cr;

            foreach (uRolePowerConfirmRole urpcr in urpcrs)
            {
                if (editRoleConfirmRoleList.Contains(urpcr.uConfirmRoleID))
                {
                    editRoleConfirmRoleList.Remove(urpcr.uConfirmRoleID);
                }
                else
                {
                    db.uRolePowerConfirmRole.Remove(urpcr);
                }
            }
            foreach (int croleid in editRoleConfirmRoleList)
            {
                uRolePowerConfirmRole urpcr = new uRolePowerConfirmRole
                {
                    uRolePowerConfirmSn= ucrsn,
                    uConfirmRoleID=croleid
                };
                db.uRolePowerConfirmRole.Add(urpcr);
            }
            db.SaveChanges();
            result = "角色覆核角色更新成功";
            return Content(result);
        }

        public JsonResult ECGetRolesList(int roleID=-1,int options=-1)
        {
            IEnumerable<uRoles> result = db.uRoles.ToList();
            IEnumerable<uRoles> uRoleO = db.uRoles.ToList();
            if (roleID!=-1&& options!=-1)
            {
                uRoles rfilter = db.uRoles.Where(n => n.uRoleID == roleID).FirstOrDefault();
                switch (options)
                {
                    case 1:                        
                        result = result.Where(n => n.uRoleID == rfilter.supervisorID);
                        break;
                    case 2:
                        List<uRoles> rList = new List<uRoles>();
                        if (rfilter != null)
                        {
                            if (rfilter.supervisorID.HasValue)
                            {
                                RoleMethod rolem = new RoleMethod();
                                rList = rolem.allSupervisorID(rfilter.supervisorID.Value, rList);
                            }
                        }
                        result = rList;
                        break;
                    default:
                        break;
                }
            }
            IEnumerable<uRolesWithDpmtSurName> result2 = from urli in result
                                                         join d in db.Department on urli.departmentID equals d.departmentID
                                              orderby d.departmentName
                                              select new uRolesWithDpmtSurName
                                              {
                                                  uRoles = urli,
                                                  Department = d.departmentName
                                              };
            List<uRolesWithDpmtSurName> result2li = new List<uRolesWithDpmtSurName>();
            foreach (var rff in result2)
            {
                if(rff.uRoles.supervisorID.HasValue)
                {
                    rff.uRoleSurName = uRoleO.Where(n => n.uRoleID == rff.uRoles.supervisorID.Value).Select(n => n.uRoleName).FirstOrDefault();
                }
                else
                {
                    rff.uRoleSurName = "無";
                }
                result2li.Add(rff);
            }
            return Json(result2li, JsonRequestBehavior.AllowGet);
        }
    }
}