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
    public class PowerController : Controller
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        Repository<uPages> dbuPages = new Repository<uPages>();
        Repository<uPowers> dbuPowers = new Repository<uPowers>();
        Repository<uRoles> dbuRoles = new Repository<uRoles>();
        Repository<uRolePowers> dbuRolePowers = new Repository<uRolePowers>();
        Repository<uRoleUsers> dbuRoleUsers = new Repository<uRoleUsers>();
        // GET: Power
        public ActionResult Index()
        {
            List<PagePowersViewModel> ppvmList = new List<PagePowersViewModel>();
            foreach(uPages uPage in db.uPages.ToList())
            {
                PagePowersViewModel ppvmItem = new PagePowersViewModel
                {
                    uPages = uPage,
                    uPowers = db.uPowers.Where(n => n.pageID == uPage.pageID).ToList()
                };
                ppvmList.Add(ppvmItem);
            }

            List<RolesViewModel> rvmList = new List<RolesViewModel>();
            foreach(uRoles uRole in db.uRoles.ToList())
            {
                RolesViewModel rvmItem = new RolesViewModel {
                    uRoles=uRole,
                    uRolePowers=db.uRolePowers.Where(n=>n.uRoleID==uRole.uRoleID).ToList(),
                    uRoleUsers=db.uRoleUsers.Where(n=>n.uRoleID==uRole.uRoleID).ToList()
                };
                rvmList.Add(rvmItem);
            }


            IEnumerable<uRoles> urs = db.uRoles.ToList();
            List<SelectListItem> ursli = new List<SelectListItem>();
            foreach (uRoles ur in urs)
            {
                ursli.Add(new SelectListItem { Value = ur.uRoleID.ToString(), Text = ur.uRoleName });
            }

            PowerViewModel powerViewModel = new PowerViewModel
            {
                department=db.Department.ToList(),
                pagePowersViewModel= ppvmList,
                rolesViewModel= rvmList,
                roles= ursli
            };

            return View(powerViewModel);
        }

        public PartialViewResult NewRole()
        {
            IEnumerable<Department> dpts = db.Department.ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach(Department dpt in dpts)
            {
                sli.Add(new SelectListItem { Value = dpt.departmentID.ToString(), Text = dpt.departmentName });
            }

            RoleEdit roleEdit = new RoleEdit
            {
                departments= sli
            };
            return PartialView(roleEdit);
        }

        [HttpPost]
        public PartialViewResult NewRole(RoleEdit model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            uRoles uRoles = new uRoles
            {
                uRoleName = model.uRoles.uRoleName,
                uRoleDescription = model.uRoles.uRoleDescription,
                uRoleNums = model.uRoles.uRoleNums,
                departmentID = model.uRoles.departmentID,
                registedDate = DateTime.Now
            };
            dbuRoles.Create(uRoles);

            ViewBag.StatusMessage = "新增角色成功。";

            IEnumerable<Department> dpts = db.Department.ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach (Department dpt in dpts)
            {
                sli.Add(new SelectListItem { Value = dpt.departmentID.ToString(), Text = dpt.departmentName });
            }

            RoleEdit roleEdit = new RoleEdit
            {
                departments = sli,
                autoClearNewRole= model.autoClearNewRole
            };
            return PartialView(roleEdit);
        }

        public PartialViewResult EditRole(int roleID)
        {
            IEnumerable<Department> dpts = db.Department.ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach (Department dpt in dpts)
            {
                sli.Add(new SelectListItem { Value = dpt.departmentID.ToString(), Text = dpt.departmentName });
            }

            IEnumerable<uRoles> urss = db.uRoles.ToList();
            List<SelectListItem> ursli = new List<SelectListItem>();
            foreach (uRoles urs in urss)
            {
                ursli.Add(new SelectListItem { Value = urs.uRoleID.ToString(), Text = urs.uRoleName });
            }

            uRoles uRole = db.uRoles.Find(roleID);

            RoleEdit roleEdit = new RoleEdit
            {
                departments = sli,
                uRoles= uRole,
                roleID=roleID,
                supervisors= ursli
            };
            return PartialView(roleEdit);
        }

        [HttpPost]
        public PartialViewResult EditRole(RoleEdit model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            int hadUser = db.uRoleUsers.Where(n => n.uRoleID == model.roleID).Count();
            if (hadUser <= model.uRoles.uRoleNums)
            {
                bool checkDeathLoop = false;
                int? csID = model.uRoles.supervisorID;
                List<int> csIDList = new List<int>();
                csIDList.Add(model.roleID);
                while (!checkDeathLoop)
                {
                    if (!csID.HasValue)
                        break;
                    else
                    {
                        if (csIDList.Contains(csID.Value))
                            checkDeathLoop = true;
                        else
                        {
                            csIDList.Add(csID.Value);
                            csID = db.uRoles.Where(n => n.uRoleID == csID).Select(n => n.supervisorID).FirstOrDefault();
                        }
                    }
                }
                if (!checkDeathLoop)
                {
                    uRoles ur = db.uRoles.Where(n => n.uRoleID == model.roleID).FirstOrDefault();
                    if(ur!=null)
                    {
                        ur.uRoleName = model.uRoles.uRoleName;
                        ur.uRoleDescription = model.uRoles.uRoleDescription;
                        ur.uRoleNums = model.uRoles.uRoleNums;
                        ur.departmentID = model.uRoles.departmentID;
                        ur.supervisorID = model.uRoles.supervisorID;
                        dbuRoles.Update(ur);
                        ViewBag.StatusMessage = "編輯角色成功。";
                    }
                    else
                    {
                        ViewBag.StatusMessage = "編輯角色失敗。";
                    }
                    //uRoles uRoles = new uRoles
                    //{
                    //    uRoleID = model.roleID,
                    //    uRoleName = model.uRoles.uRoleName,
                    //    uRoleDescription = model.uRoles.uRoleDescription,
                    //    uRoleNums = model.uRoles.uRoleNums,
                    //    departmentID = model.uRoles.departmentID,
                    //    supervisorID = model.uRoles.supervisorID
                    //};
                    
                }
                else
                {
                    ViewBag.StatusMessage = "產生循環主管階層，請重新設定。";
                }
            }
            else
            {
                ViewBag.StatusMessage = "已有使用者人數超過角色人數，請先減少使用者人數。";
            }
            

            IEnumerable<Department> dpts = db.Department.ToList();
            List<SelectListItem> sli = new List<SelectListItem>();
            foreach (Department dpt in dpts)
            {
                sli.Add(new SelectListItem { Value = dpt.departmentID.ToString(), Text = dpt.departmentName });
            }

            IEnumerable<uRoles> urss = db.uRoles.ToList();
            List<SelectListItem> ursli = new List<SelectListItem>();
            foreach (uRoles urs in urss)
            {
                ursli.Add(new SelectListItem { Value = urs.uRoleID.ToString(), Text = urs.uRoleName });
            }

            RoleEdit roleEdit = new RoleEdit
            {
                departments = sli,
                autoClearNewRole = model.autoClearNewRole,
                supervisors= ursli
            };
            return PartialView(roleEdit);
        }

        public PartialViewResult EditRolePower(int roleID)
        {
            List<RoleEditPgPowers> reppsli = new List<RoleEditPgPowers>();
            foreach (uPages uPage in db.uPages.ToList())
            {
                IEnumerable<uPowers> uPowers = db.uPowers.Where(n => n.pageID == uPage.pageID).ToList();
                List<RoleEditPower> repli = new List<RoleEditPower>();
                foreach(uPowers ups in uPowers)
                {
                    bool hps = false;
                    if (db.uRolePowers.Where(n => n.uRoleID == roleID && n.powerID == ups.powerID).FirstOrDefault() != null)
                        hps = true;
                    RoleEditPower rep = new RoleEditPower
                    {
                        pageID = ups.pageID,
                        powerID = ups.powerID,
                        powerDescription = ups.powerDescription,
                        powerName = ups.powerName,
                        hasPower = hps
                    };
                    repli.Add(rep);
                }
                RoleEditPgPowers ppvmItem = new RoleEditPgPowers
                {
                    uPages = uPage,
                    roleEditPowers= repli
                };
                reppsli.Add(ppvmItem);
            }
            RoleEditPowersViewModel repvm = new RoleEditPowersViewModel {
                roleID=roleID,
                roleEditPgPowers=reppsli
            };
            return PartialView(repvm);
        }
        
        [HttpPost]
        public PartialViewResult EditRolePower(FormCollection form)
        {
            //if (!ModelState.IsValid)
            //{
            //    return PartialView(model);
            //}
            int roleID=Convert.ToInt32(form["roleID"]);
            List<int> powers = new List<int>();
            foreach (var key in form.AllKeys)
            {
                if (key.Contains("powerID"))
                {
                    powers.Add(Convert.ToInt32(form.Get(key)));
                }
            }
            IEnumerable<uRolePowers> roleurps = db.uRolePowers.Where(n => n.uRoleID == roleID).ToList();
            foreach(uRolePowers urp in roleurps)
            {
                if (powers.Contains(urp.powerID))
                {
                    powers.Remove(urp.powerID);
                }
                else
                {
                    db.uRolePowers.Remove(urp);
                }
            }
            foreach(int powerID in powers)
            {
                uRolePowers urps = new uRolePowers {
                    uRoleID= roleID,
                    powerID= powerID
                };
                db.uRolePowers.Add(urps);
            }
            db.SaveChanges();

            ViewBag.StatusMessage = "編輯權限成功。";

            List<RoleEditPgPowers> reppsli = new List<RoleEditPgPowers>();
            foreach (uPages uPage in db.uPages.ToList())
            {
                IEnumerable<uPowers> uPowers = db.uPowers.Where(n => n.pageID == uPage.pageID).ToList();
                List<RoleEditPower> repli = new List<RoleEditPower>();
                foreach (uPowers ups in uPowers)
                {
                    bool hps = false;
                    if (db.uRolePowers.Where(n => n.uRoleID == roleID && n.powerID == ups.powerID).FirstOrDefault() != null)
                        hps = true;
                    RoleEditPower rep = new RoleEditPower
                    {
                        pageID = ups.pageID,
                        powerID = ups.powerID,
                        powerDescription = ups.powerDescription,
                        powerName = ups.powerName,
                        hasPower = hps
                    };
                    repli.Add(rep);
                }
                RoleEditPgPowers ppvmItem = new RoleEditPgPowers
                {
                    uPages = uPage,
                    roleEditPowers = repli
                };
                reppsli.Add(ppvmItem);
            }
            RoleEditPowersViewModel repvm = new RoleEditPowersViewModel
            {
                roleID = roleID,
                roleEditPgPowers = reppsli
            };
            return PartialView(repvm);
            
        }

        public PartialViewResult EditRoleUser(int roleID)
        {
            
            IEnumerable<UserIdUserNameRoles> userIdName = from x in db.uRoleUsers.Where(n => n.uRoleID == roleID)
                                                     join y in db.AspNetUsers on x.uUserID equals y.UserName
                                                     select new UserIdUserNameRoles
                                                     {
                                                         UserID=x.uUserID,
                                                         UserName= y.Name
                                                     };
            int limitUser = db.uRoles.Where(n => n.uRoleID == roleID).FirstOrDefault().uRoleNums;

            List<SelectListItem> una = new List<SelectListItem>();
            IEnumerable<IsEmployee> isEmployees = db.IsEmployee.ToList();
            foreach (IsEmployee ies in isEmployees)
            {
                string username = db.AspNetUsers.Where(n => n.UserName == ies.UserId).Select(n => n.Name).FirstOrDefault();
                una.Add(new SelectListItem { Value = ies.UserId, Text = string.Format("{0}_{1}", username, ies.UserId) });
            }

            RoleEditUserViewModel roleEditUserViewModel = new RoleEditUserViewModel {
                roleID=roleID,
                UserIDNames= userIdName,
                userNameAll=una,
                limit= limitUser
            };
            
            return PartialView(roleEditUserViewModel);
        }

        [HttpPost]
        public PartialViewResult EditRoleUser(FormCollection form)
        {
            int roleID = Convert.ToInt32(form["roleID"]);
            IEnumerable<IsEmployee> iemp = db.IsEmployee.ToList();
            int limitUser = db.uRoles.Where(n => n.uRoleID == roleID).FirstOrDefault().uRoleNums;
            List<string> editUserNamesList = new List<string>();
            foreach (var key in form.AllKeys)
            {
                if (key.Contains("userName")&& iemp.Where(n=>n.UserId== form.Get(key)).Count()>0)
                {
                    editUserNamesList.Add(form.Get(key));
                }
            }
            if (editUserNamesList.Count <= limitUser)
            {
                IEnumerable<uRoleUsers> roleurus = db.uRoleUsers.Where(n => n.uRoleID == roleID).ToList();
                foreach (uRoleUsers uru in roleurus)
                {
                    if (editUserNamesList.Contains(uru.uUserID))
                    {
                        editUserNamesList.Remove(uru.uUserID);
                    }
                    else
                    {
                        db.uRoleUsers.Remove(uru);
                    }
                }
                foreach (string editUserName in editUserNamesList)
                {
                    uRoleUsers urus = new uRoleUsers
                    {
                        uRoleID = roleID,
                        uUserID = editUserName
                    };
                    db.uRoleUsers.Add(urus);
                }
                db.SaveChanges();

                ViewBag.StatusMessage = "編輯角色使用者成功。";
            }
            else
            {
                ViewBag.StatusMessage = "編輯角色超出人數。";
            }
            
            IEnumerable<UserIdUserNameRoles> userIdName = from x in db.uRoleUsers.Where(n => n.uRoleID == roleID)
                                                     join y in db.AspNetUsers on x.uUserID equals y.UserName
                                                     select new UserIdUserNameRoles
                                                     {
                                                         UserID = x.uUserID,
                                                         UserName = y.Name
                                                     };

            List<SelectListItem> una = new List<SelectListItem>();
            IEnumerable<IsEmployee> isEmployees = db.IsEmployee.ToList();
            foreach (IsEmployee ies in isEmployees)
            {
                string username = db.AspNetUsers.Where(n => n.UserName == ies.UserId).Select(n => n.Name).FirstOrDefault();
                una.Add(new SelectListItem { Value = ies.UserId, Text = string.Format("{0}_{1}", username, ies.UserId) });
            }

            RoleEditUserViewModel roleEditUserViewModel = new RoleEditUserViewModel
            {
                roleID = roleID,
                UserIDNames = userIdName,
                userNameAll = una,
                limit = limitUser
            };

            return PartialView(roleEditUserViewModel);
        }

        public ActionResult RemoveRoleUser(int roleID)
        {
            var uroleusre = db.uRoleUsers.Where(n => n.uRoleID == roleID).ToList();
            db.uRoleUsers.RemoveRange(uroleusre);
            var urolepsre = db.uRolePowers.Where(n => n.uRoleID == roleID).ToList();
            db.uRolePowers.RemoveRange(urolepsre);
            var urolesre = db.uRoles.Where(n => n.uRoleID == roleID).ToList();
            db.uRoles.RemoveRange(urolesre);

            db.SaveChanges();
            return Content("");
        }

        public JsonResult GetRolesList()
        {
            IEnumerable<uRoles> result = db.uRoles.ToList();
            IEnumerable<uRolesDpmt> result2 = from urli in result
                                              join d in db.Department on urli.departmentID equals d.departmentID
                                              orderby d.departmentName
                                              select new uRolesDpmt
                                              {
                                                  uRoles = urli,
                                                  Department = d.departmentName
                                              };
            return Json(result2, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesList()
        {
            IEnumerable<IsEmployee> result = db.IsEmployee.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetEmployeesList2()
        {
            IEnumerable<IsEmployee> result = db.IsEmployee.ToList();
            IEnumerable<UserIdUserNameRoles> result2 = from x in result
                                                  join c in db.AspNetUsers.ToList() on x.UserId equals c.UserName
                                                  select new UserIdUserNameRoles {
                                                      UserID = c.UserName,
                                                      UserName = c.Name
                                                  };
            return Json(result2, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult ViewRoleFromPower()
        {
            IEnumerable<uPages> uPages = db.uPages.ToList();            
            IEnumerable<uPowers> uPowers = db.uPowers.ToList();
            IEnumerable<uRoles> uRoles = db.uRoles.ToList();
            IEnumerable<uRolePowers> uRolePowers = db.uRolePowers.ToList();
            IEnumerable<uRoleUsers> uRoleUsers = db.uRoleUsers.ToList();
            List<RoleFromPowersViewModel> rfpvmList = new List<RoleFromPowersViewModel>();
            foreach (uPages ups in uPages)
            {
                List<RoleFromPower> rfpsList = new List<RoleFromPower>();
                foreach(uPowers uPower in uPowers.Where(n=>n.pageID==ups.pageID).ToList())
                {
                    IEnumerable<uRolePowers> urps = uRolePowers.Where(n => n.powerID == uPower.powerID).ToList();
                    IEnumerable<UserIdUserNameRoles> uids = (from x in uRoleUsers
                                              join y in urps on x.uRoleID equals y.uRoleID
                                               join z in db.AspNetUsers on x.uUserID equals z.UserName
                                               select new UserIdUserNameRoles {
                                                   UserID = x.uUserID,
                                                   UserName =z.Name,
                                                   uRolesDpmts=from urx in db.uRoles.ToList()
                                                               join urd in db.Department.ToList() on urx.departmentID equals urd.departmentID
                                                               join uruid in db.uRoleUsers.ToList() on urx.uRoleID equals uruid.uRoleID
                                                               where uruid.uUserID==x.uUserID
                                                               select new uRolesDpmt {
                                                                   uRoles =new uRoles {
                                                                       uRoleID =urx.uRoleID,
                                                                       uRoleName=urx.uRoleName,
                                                                       uRoleDescription=urx.uRoleDescription,
                                                                       uRoleNums=urx.uRoleNums,
                                                                       departmentID=urx.departmentID,
                                                                       supervisorID=urx.supervisorID
                                                                   },
                                                                   Department=urd.departmentName
                                                               }
                                               }).Distinct(new UserIdUserNameRolesComparer());
                    
                    IEnumerable<uRolesDpmtUserIdUserName> urds = from x in urps
                              join y in uRoles on x.uRoleID equals y.uRoleID
                              join z in db.Department.ToList() on y.departmentID equals z.departmentID
                              select new uRolesDpmtUserIdUserName
                              {
                                  Department =z.departmentName,
                                  uRoles=new uRoles {
                                      uRoleID=y.uRoleID,
                                      uRoleName=y.uRoleName,
                                      uRoleNums=y.uRoleNums,
                                      uRoleDescription=y.uRoleDescription,
                                      departmentID=y.departmentID
                                  },
                                  userIdUserName=from ax in uRoles
                                                 join ay in uRoleUsers on ax.uRoleID equals ay.uRoleID
                                                 join az in db.IsEmployee on ay.uUserID equals az.UserId
                                                 join azz in db.AspNetUsers on az.UserId equals azz.UserName
                                                 where ay.uRoleID==y.uRoleID
                                                 select new UserIdUserName {
                                                     UserID=az.UserId,
                                                     UserName=azz.Name
                                                 }
                              };
                    IEnumerable<string> dpmts = (from x in urds
                                                 select x.Department).Distinct();
                    RoleFromPower rfp = new RoleFromPower {
                        uPower=uPower,
                        UserIDNames= uids,
                        uRolesDpmtUserIdUserNames= urds,
                        Department= dpmts
                    };
                    rfpsList.Add(rfp);
                }

                RoleFromPowersViewModel rfpvm = new RoleFromPowersViewModel {
                    uPages=ups,
                    roleFromPower= rfpsList
                };
                rfpvmList.Add(rfpvm);
            }

            return PartialView(rfpvmList);
        }
        
        public PartialViewResult ViewRoleLevel()
        {
            List<RoleLevelViewModel> rlvms = new List<RoleLevelViewModel>();
            IEnumerable<uRoles> uRoles = db.uRoles.ToList();
            foreach(uRoles urole in uRoles)
            {
                List<RolesUserNames> runasli = new List<RolesUserNames>();
                IEnumerable<uRoleUsers> urus = db.uRoleUsers.Where(n => n.uRoleID == urole.uRoleID).ToList();
                foreach(var uru in urus)
                {
                    string una = db.AspNetUsers.Where(n => n.UserName == uru.uUserID).Select(n=>n.Name).FirstOrDefault();
                    RolesUserNames runa = new RolesUserNames {
                        uRoleUser=uru,
                        UserName= una
                    };
                    runasli.Add(runa);
                }

                RoleLevelViewModel rlvm = new RoleLevelViewModel {
                    uRoles=urole,
                    uRolePowers=db.uRolePowers.Where(n=>n.uRoleID==urole.uRoleID).ToList(),
                    rolesUserNames= runasli
                };
                rlvms.Add(rlvm);
            }

            List<PagePowersViewModel> ppvmList = new List<PagePowersViewModel>();
            foreach (uPages uPage in db.uPages.ToList())
            {
                PagePowersViewModel ppvmItem = new PagePowersViewModel
                {
                    uPages = uPage,
                    uPowers = db.uPowers.Where(n => n.pageID == uPage.pageID).ToList()
                };
                ppvmList.Add(ppvmItem);
            }

            RoleLevelWithPowerViewModel rlwpvm = new RoleLevelWithPowerViewModel {
                roleLevelViewModel=rlvms,
                pagePowersViewModel= ppvmList
            };
            return PartialView(rlwpvm);
        }
    }
}