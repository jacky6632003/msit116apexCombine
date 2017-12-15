using Microsoft.AspNet.Identity.Owin;
using msit116apexLayout.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity;
using GoogleRecaptcha;

namespace msit116apexLayout.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        Repository<AspNetUsers> dbUser = new Repository<AspNetUsers>();
        Repository<UserPreference> dbUserPreference = new Repository<UserPreference>();
        Repository<UserPreferenceAOrder> dbUserPreferenceAOrder = new Repository<UserPreferenceAOrder>();
        Repository<UserNews> dbUserNews = new Repository<UserNews>();
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        HubServerMethods NewsHub = new HubServerMethods();
        ConfirmUserPowerMethod cm = new ConfirmUserPowerMethod();
        // GET: Account
        public ActionResult Index()
        {
            return RedirectToAction("Login");
        }

        //HELP註解 會員登入
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }
                
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Login(LoginViewModel modal, string returnUrl,string EmailLoginCode, FormCollection form)
        {
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = "6LfqzzcUAAAAAHgafe6oixzCCTIZOzvbPbGta8fT"
            });

            RecaptchaV2Result recaptchaResult = recaptcha.Verify();
            string dontcheckgrc = form["dontcheckgrc"];
            if (!(dontcheckgrc =="on"|| recaptchaResult.Success) || !ModelState.IsValid)
            //if (!ModelState.IsValid)
            {
                return View(modal);
            }
            //Email Login Code
            //UserTwoFactor utf = db.UserTwoFactor.Where(c=>c.UserId== form.Email).FirstOrDefault();
            //if (utf != null&& utf.EmailLogin.HasValue&&utf.EmailLogin.Value)
            //{
            //    if (Session["EmailLoginCode"] == null)
            //        return View(form);
            //    if (Session["EmailLoginCode"].ToString() != EmailLoginCode)
            //        return View(form);
            //}

            //var result = await SignInManager.PasswordSignInAsync(form.Email, form.Password, form.RememberMe, shouldLockout: false);
            var result = await SignInManager.PasswordSignInAsync(modal.Email, modal.Password, modal.RememberMe, shouldLockout: true);

            //Email Confirm
            // If it was a successful login
            if (result == SignInStatus.Success || result == SignInStatus.RequiresVerification)
            {
                // check that their email address is confirmed:
                var user = await UserManager.FindByNameAsync(modal.Email);
                //Email認證
                if (!await UserManager.IsEmailConfirmedAsync(user.Id))
                {
                    // sign them out!
                    AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

                    TempData["UserId"] = user.Id;
                    return RedirectToAction("UnconfirmedEmail","Account",new { area=""});
                }

                // reset their login 
            }

            switch (result)
            {
                case SignInStatus.Success:
                    List<UserNewsUrls> testUrls = new List<UserNewsUrls>();
                    UserNewsUrls testUrl1 = new UserNewsUrls { UserNewsTitle = "管理", UserNewsUrl = Url.Action("Index", "Manage", new { area = "" }), UserNewsCSS = "btn btn-warning" };
                    UserNewsUrls testUrl2 = new UserNewsUrls { UserNewsTitle = "通知", UserNewsUrl = Url.Action("UserMessage", "Account", new { area = "" }), UserNewsCSS = "btn btn-success" };
                    UserNewsUrls testUrl3 = new UserNewsUrls { UserNewsTitle = "測試登入", UserNewsUrl = Url.Action("TestSuccessLogin", "Account", new { area = "" }), UserNewsCSS = "btn btn-info" };
                    testUrls.Add(testUrl1);
                    testUrls.Add(testUrl2);
                    testUrls.Add(testUrl3);
                    //NewsHub.SendMessageToAll("使用者登入", string.Format("{0}已登入", modal.Email),Url.Action("Index","Manage"),testUrls);
                    NewsHub.SendMessageToUser(true, "ycwaspserver@gmail.com", "使用者登入", string.Format("{0}已登入", modal.Email), Url.Action("Index", "Manage", new { area = "" }), testUrls);
                    //NewsHub.SendMessageToUser(modal.Email, "ycwaspserver@gmail.com", "使用者登入", string.Format("{0}已登入", modal.Email), "");
                    return RedirectToLocal(returnUrl);                    
                case SignInStatus.RequiresVerification:
                    return RedirectToAction("SendCode","Account", new { area="",ReturnUrl = returnUrl, RememberMe = modal.RememberMe });
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "登入嘗試失試。");
                    if (db.AspNetUsers.Where(n => n.UserName == modal.Email).ToList().Count() == 1)
                        LoginErrorEmail.SendEmail(modal.Email, Request.Browser);
                    return View(modal);
            }
        }

        //NEW
        [AllowAnonymous]
        public ActionResult UnconfirmedEmail()
        {
            ResendValidationEmailViewModel ViewModel = new ResendValidationEmailViewModel
            {
                UserId = (string)TempData["UserId"]
            };
            return View(ViewModel);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> ResendValidationEmail(ResendValidationEmailViewModel ViewModel)
        {
            string callbackUrl = await generateConfirmAccountEmail(ViewModel.UserId);

#if DEBUG
            ViewModel.CallbackUrl = callbackUrl;
#endif

            return View(ViewModel);
        }

        //NEW
        private async Task<string> generateConfirmAccountEmail(string userId)
        {
            string email = UserManager.GetEmail(userId);

            string code =
                await UserManager.GenerateEmailConfirmationTokenAsync(userId);

            var routeValues = new { userId = userId, code = code };

            var callbackUrl =
                Url.Action("ConfirmEmail", "Account", routeValues, protocol: Request.Url.Scheme);

            Emailer emailer = new Emailer();
            emailer.sendEmail(email,
                      "認證您的帳戶",
                      "請點擊以認證您的帳戶 <a href=\"" + callbackUrl + "\">認證連結</a>");

            return callbackUrl;
        }



        //NEW
        // GET: /Account/VerifyCode
        [AllowAnonymous]
        public async Task<ActionResult> VerifyCode(string provider, string returnUrl, bool rememberMe)
        {
            // Require that the user has already logged in via username/password or external login
            if (!await SignInManager.HasBeenVerifiedAsync())
            {
                return View("Login");
            }
            return View(new VerifyCodeViewModel { Provider = provider, ReturnUrl = returnUrl, RememberMe = rememberMe });
        }

        //
        // POST: /Account/VerifyCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // The following code protects for brute force attacks against the two factor codes. 
            // If a user enters incorrect codes for a specified amount of time then the user account 
            // will be locked out for a specified amount of time. 
            // You can configure the account lockout settings in IdentityConfig
            var result = await SignInManager.TwoFactorSignInAsync(
                model.Provider,
                model.Code,
                isPersistent: model.RememberMe,
                rememberBrowser: model.RememberBrowser);

            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(model.ReturnUrl);
                case SignInStatus.LockedOut:
                    return View("Lockout");
                case SignInStatus.Failure:
                default:
                    ModelState.AddModelError("", "Invalid code.");
                    return View(model);
            }
        }


        //外部登入
        [HttpPost]
        [AllowAnonymous]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // 要求重新導向至外部登入提供者
            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }
        // GET: /Account/ExternalLoginCallback
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("Login");
            }

            // 若使用者已經有登入資料，請使用此外部登入提供者登入使用者
            var result = await SignInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                case SignInStatus.Failure:
                default:
                    // 若使用者沒有帳戶，請提示使用者建立帳戶
                    ViewBag.ReturnUrl = returnUrl;
                    ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                    return View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email });
            }
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Manage");
            }

            if (ModelState.IsValid)
            {
                // 從外部登入提供者處取得使用者資訊
                var info = await AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                var user = new ApplicationUser { UserName = model.Email
                    , Email = model.Email
                    ,Name = model.Name
                    ,ResidenceAddress = model.ResidenceAddress
                    ,IdentityCardNumber = model.IdentityCardNumber                    
                    ,BirthDay = model.BirthDay
                };
                if (model.Email == "ycwaspclient@gmail.com")
                    user.Title = "軟體工程師";

                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user.Id, info.Login);
                    //if (result.Succeeded)
                    //{
                    //    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //    return RedirectToLocal(returnUrl);
                    //}
                    var callbackUrl = await generateConfirmAccountEmail(user.Id);

#if DEBUG
                    TempData["ViewBagLink"] = callbackUrl;
#endif

                    ViewBag.Message = "請完成帳號的電子郵件認證，方可登入。";

                    return View("Info");
                }
                AddErrors(result);
                return View(model);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View(model);
        }
        
        //HELP註解 會員註冊
        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> Register(RegisterViewModel modal, FormCollection form)
        {
            
            int CityId = 0;
            int DistrictId = 0;
            int RoadId = 0;
            string LnStr = form["AddressRLn"];
            string AlyStr = form["AddressRAly"];
            string NoStr = form["AddressRNo"];
            string FStr = form["AddressRF"];
            string RmStr = form["AddressRRm"];
            string dontcheckgrc = form["dontcheckgrc"];
            string dontcheckemail = form["dontcheckemail"];
            string registerEmployee = form["registerEmployee"];
            bool checkAddress = false;
            if(int.TryParse(form["selectRCity"], out CityId))
            {
                if (int.TryParse(form["selectRDistrict"], out DistrictId))
                {
                    if (int.TryParse(form["selectRRoad"], out RoadId))
                    {
                        checkAddress = true;
                    }
                }
            }

            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = "6LfqzzcUAAAAAHgafe6oixzCCTIZOzvbPbGta8fT"
            });

            RecaptchaV2Result recaptchaResult = recaptcha.Verify();
            
            if ((dontcheckgrc =="on"|| recaptchaResult.Success)&&ModelState.IsValid&& checkAddress)
            //if (ModelState.IsValid)
            {
                string CityName = db.AddressCity.Where(n => n.Id == CityId).Select(n => n.Name).FirstOrDefault();
                string DistrictName = db.AddressDistrict.Where(n => n.Id == DistrictId).Select(n => n.Name).FirstOrDefault();
                string RoadName = db.AddressRoad.Where(n => n.Id == RoadId).Select(n => n.Name).FirstOrDefault();
                string ResidenceAddress = CityName + " " + DistrictName + " " + RoadName;
                if (LnStr != string.Empty)
                    ResidenceAddress +=" "+ LnStr+"巷";
                if (AlyStr != string.Empty)
                    ResidenceAddress += " " + AlyStr + "弄";
                if (NoStr != string.Empty)
                    ResidenceAddress += " " + NoStr + "號";
                if (FStr != string.Empty)
                    ResidenceAddress += " " + FStr + "樓";
                if (RmStr != string.Empty)
                    ResidenceAddress += " " + RmStr + "室";

                var user = new ApplicationUser { UserName = modal.Email
                    , Email = modal.Email
                    ,Name=modal.Name
                    ,ResidenceAddress= ResidenceAddress
                    ,IdentityCardNumber=modal.IdentityCardNumber
                    ,BirthDay=modal.BirthDay };
                var result = await UserManager.CreateAsync(user, modal.Password);
                if (result.Succeeded)
                {
                    AspNetUsers anuFixData = db.AspNetUsers.Where(n => n.UserName == user.UserName).First();
                    UserResidenceAddress ura = new UserResidenceAddress {
                        Id= anuFixData.Id,
                        AddressCityId=CityId,
                        AddressDistrictId=DistrictId,
                        AddressRoadId=RoadId,
                        AddressLn=LnStr,
                        AddressAly=AlyStr,
                        AddressNo=NoStr,
                        AddressF=FStr,
                        AddressRm=RmStr
                    };
                    db.UserResidenceAddress.Add(ura);
                    if(registerEmployee=="on")
                    {
                        IsEmployee nie = new IsEmployee {
                            UserId= modal.Email,
                            RegisterDate=DateTime.Now
                        };
                        db.IsEmployee.Add(nie);
                    }
                    db.SaveChanges();
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    //return RedirectToAction("TestSuccessLogin", "Account");
                    if (dontcheckemail == "on")
                    {
                        anuFixData.EmailConfirmed = true;
                        db.SaveChanges();
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                        return RedirectToAction("Index","Default",new { area=""});
                    }
                    var callbackUrl = await generateConfirmAccountEmail(user.Id);

#if DEBUG
                    TempData["ViewBagLink"] = callbackUrl;
#endif

                    ViewBag.Message = "請完成帳號的電子郵件認證，方可登入。";

                    return View("Info");
                }
            }
            return View(modal);
        }

        [AllowAnonymous]
        public JsonResult GetCityList()
        {
            IEnumerable<AddressCity> result = db.AddressCity.ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetDistrictList(int CityId)
        {
            IEnumerable<AddressDistrict> result = db.AddressDistrict.Where(n=>n.AddressCityId== CityId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetRoadList(int DistrictId)
        {
            IEnumerable<AddressRoad> result = db.AddressRoad.Where(n=>n.AddressDistrictId== DistrictId).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Account/ConfirmEmail
        [AllowAnonymous]
        public async Task<ActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return View("Error");
            }
            var result = await UserManager.ConfirmEmailAsync(userId, code);
            return View(result.Succeeded ? "ConfirmEmail" : "Error");
        }

        //NEW
        // GET: /Account/SendCode
        [AllowAnonymous]
        public async Task<ActionResult> SendCode(string returnUrl, bool rememberMe)
        {
            var userId = await SignInManager.GetVerifiedUserIdAsync();
            if (userId == null)
            {
                return View("Error");
            }
            var userFactors = await UserManager.GetValidTwoFactorProvidersAsync(userId);

            // If there's only one, don't make the user select it
            if (userFactors.Count == 1)
            {
                return RedirectToAction("VerifyCode", new
                {
                    Provider = userFactors[0],
                    ReturnUrl = returnUrl,
                    RememberMe = rememberMe
                });
            }

            var factorOptions = userFactors.Select(
                purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();

            return View(new SendCodeViewModel
            {
                Providers = factorOptions,
                ReturnUrl = returnUrl,
                RememberMe = rememberMe
            });
        }

        //
        // POST: /Account/SendCode
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SendCode(SendCodeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            // Generate the token and send it
            if (!await SignInManager.SendTwoFactorCodeAsync(model.SelectedProvider))
            {
                return View("Error");
            }
            return RedirectToAction("VerifyCode", new { Provider = model.SelectedProvider, ReturnUrl = model.ReturnUrl, RememberMe = model.RememberMe });
        }

        //HELP註解 登出
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Login", "Account");
        }

        public ActionResult MembersManager()
        {
            //HELP註解 取得所有會員資料
            return View(db.ProcedureGetMembers().ToList());
        }

        public ActionResult EmlpoyeesManager()
        {
            IEnumerable<ProcedureGetEmployees_Result> result = db.ProcedureGetEmployees().Where(c => c.UserName == "x@x.com");
            //string x = result.First().UserName;
            //HELP註解 取得所有員工資料
            return View(db.ProcedureGetEmployees().ToList());
            //return View(result.ToList());
        }

        //HELP註解 表單式新增會員
        public ActionResult NewMember()
        {
            MemberViewModel mvm = new MemberViewModel();
            return PartialView(mvm);
        }
        [HttpPost]
        public async Task<ActionResult> NewMember(MemberViewModel modal)
        {
            if (modal != null && modal.UserName != null && modal.Password != null && modal.Name != null)
            {
                var user = new ApplicationUser() { UserName = modal.UserName, Email = modal.UserName, Name = modal.Name, PhoneNumber = modal.PhoneNumber, Telephone = modal.Telephone, IdentityCardNumber = modal.IdentityCardNumber, ResidenceAddress = modal.ResidenceAddress, MailingAddress = modal.MailingAddress, Title = modal.Title, BirthDay = modal.BirthDay, Country = modal.Country };
                var result = await UserManager.CreateAsync(user, modal.Password);
                if (result.Succeeded)
                {
                }
            }
            return RedirectToAction("MembersManager");
        }

        //HELP註解 表單式新增員工
        public ActionResult NewEmlpoyee()
        {
            EmployeeViewModel evm = new EmployeeViewModel();
            return PartialView(evm);
        }
        [HttpPost]
        public async Task<ActionResult> NewEmlpoyee(EmployeeViewModel modal)
        {
            if (modal != null && modal.UserName != null && modal.Password != null && modal.Name != null)
            {
                //var user = new ApplicationUser { UserName = form.Email, Email = form.Email };
                var user = new ApplicationUser() { UserName = modal.UserName, Email = modal.UserName, Name = modal.Name, PhoneNumber = modal.PhoneNumber, Telephone = modal.Telephone, IdentityCardNumber = modal.IdentityCardNumber, ResidenceAddress = modal.ResidenceAddress, MailingAddress = modal.MailingAddress, Title = modal.Title, BirthDay = modal.BirthDay, Country = modal.Country };
                var result = await UserManager.CreateAsync(user, modal.Password);
                if (result.Succeeded)
                {
                    Repository<IsEmployee> isempDB = new Repository<IsEmployee>();
                    IsEmployee isemp = new IsEmployee
                    {
                        UserId = modal.UserName,
                        RegisterDate = DateTime.Now
                    };
                    isempDB.Create(isemp);
                }
            }
            EmployeeViewModel evm = new EmployeeViewModel();
            //return PartialView(evm);
            return RedirectToAction("EmlpoyeesManager");
        }
       
        [HttpGet]
        public ActionResult TestSuccessLogin()
        {
            //HELP註解 取得登入使用者ID
            ViewBag.LogUser = User.Identity.GetUserName();
            //HELP註解 取得登入使用者資料
            //AspNetUsers userdata= db.AspNetUsers.Find(User.Identity.GetUserId());            
            var userdata= db.ProcedureGetUserData(User.Identity.GetUserId()).First();            
            //HELP註解 判斷登入使用者是否為員工
            if (db.IsEmployee.Find(User.Identity.GetUserName()) != null)
                ViewBag.IsEmployee = "是員工";
            else
                ViewBag.IsEmployee = "不是員工";
            //HELP註解 會員資料表 db.AspNetUsers
            return View(userdata);
        }

        [HttpPost]
        public ActionResult TestSuccessLogin(ProcedureGetUserData_Result modal, HttpPostedFileBase Photo)
        {
            AspNetUsers user = new AspNetUsers
            {
                Id = User.Identity.GetUserId(),
                UserName = User.Identity.GetUserName(),
                Name = modal.Name,
                BirthDay = modal.BirthDay,
                Country = modal.Country,
                Email = modal.Email,
                IdentityCardNumber = modal.IdentityCardNumber,
                MailingAddress = modal.MailingAddress,
                PhoneNumber = modal.PhoneNumber,
                ResidenceAddress = modal.ResidenceAddress,
                Telephone = modal.Telephone,
                Title = modal.Title
            };
            if (Photo != null)
            {
                byte[] photobytes = new byte[Photo.InputStream.Length];
                Photo.InputStream.Read(photobytes, 0, photobytes.Length);
                user.Photo = photobytes;
            }
            dbUser.UpdateWithoutNull(user);

            return RedirectToAction("TestSuccessLogin","Account");
        }

        // GET: /Manage/ManageLogins
        public ActionResult _PartialManageSocialLogin()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user == null)
            {
                //return View("Error");
            }
            var userLogins = UserManager.GetLogins(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return PartialView(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        // POST: /Manage/LinkLogin
        [HttpPost]
        public ActionResult LinkLogin(string provider)
        {
            // 要求重新導向至外部登入提供者，以連結目前使用者的登入
            return new ChallengeResult(provider, Url.Action("LinkLoginCallback", "Account"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync("XsrfId", User.Identity.GetUserId());
            if (loginInfo == null)
            {
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return RedirectToAction("Index", "Manage");
        }

        // POST: /Manage/RemoveLogin
        [HttpPost]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }
            return RedirectToAction("Index","Manage");
        }

        public ActionResult UserMessage()
        {
            List<UserNews> userNewss = dbUserNews.GetAll().Where(n=>n.UserId==User.Identity.GetUserName()).OrderByDescending(n=>n.time).ToList();
            List<UserNewsViewModel> unvms = new List<UserNewsViewModel>();
            foreach(UserNews uns in userNewss)
            {
                UserNewsViewModel unvm = new UserNewsViewModel
                {
                    UserNews = uns,
                    UserNewsUrlss = db.UserNewsUrls.Where(n => n.UserNewsSn == uns.sn).ToList(),
                    FromUserName= db.AspNetUsers.Where(n => n.UserName == uns.fromUser).Select(n => n.Name).FirstOrDefault()
            };
                unvms.Add(unvm);
            }

            //return View(userNewss);
            return View(unvms);
        }

        [HttpPost]
        public void UserMessage(UserNews model)
        {
            UserNews userNews = db.UserNews.Find(model.sn);
            if(userNews.UserId==User.Identity.GetUserName())
            {
                userNews.read = model.read;
                db.SaveChanges();
            }            
        }

        [HttpPost]
        public ActionResult RemoveUserMessage(UserNews model)
        {
            string result = "";
            //檢查員工
            if (cm.checkIsEmployee(User.Identity.GetUserName()))
            {
                //檢查權限刪除通知的權限ID為5
                if (cm.checkHasPower(User.Identity.GetUserName(), 5))
                {
                    //檢查覆核
                    UserNews userNews = db.UserNews.Find(model.sn);
                    List<UserNewsUrls> userNewsUrls = db.UserNewsUrls.Where(n => n.UserNewsSn == model.sn).ToList();
                    string userName = db.AspNetUsers.Where(n => n.UserName == userNews.fromUser).Select(n => n.Name).First();
                    string confirmDescription = "";
                    confirmDescription += "<div style='border:1px solid black;margin: 2px'>";
                    confirmDescription += "<p>欲移除通知：</p>";
                    confirmDescription += "<div style='border:1px solid black;margin: 2px'>";
                    confirmDescription += "<p>寄送者：" + userName+"&lt"+ userNews.fromUser+"&gt" + "</p>";
                    confirmDescription += "<p>標題：" + userNews.msgTitle + "</p>";
                    confirmDescription += "<p>內容：" + userNews.msgContent + "</p>";
                    confirmDescription += "<p>發送時間：" + userNews.time + "</p>連結：";
                    confirmDescription += "<a href='" + userNews.msgUrl + "' class='btn btn-success'>連結</a>";
                    foreach (var unus in userNewsUrls)
                    {
                        confirmDescription += "<a href='" + unus.UserNewsUrl + "' class='" + unus.UserNewsCSS + "'>" + unus.UserNewsTitle + "</a>";
                    }

                    confirmDescription += "</div>";
                    confirmDescription += "</div>";
                    int? outurpchSn;
                    string ckConfirm = cm.checkNeedConfirm(out outurpchSn, User.Identity.GetUserName(), 5,Url.Action("UserConfirmUserPower", "ConfirmUserPower", new { area=""}), confirmDescription);
                    if (ckConfirm == "")
                    {
                        //UserNews userNews = db.UserNews.Find(model.sn);
                        if (userNews.UserId == User.Identity.GetUserName())
                        {
                            IEnumerable<UserNewsUrls> unus = db.UserNewsUrls.Where(n => n.UserNewsSn == userNews.sn);
                            foreach (var unu in unus)
                            {
                                db.UserNewsUrls.Remove(unu);
                            }
                            db.UserNews.Remove(userNews);
                            db.SaveChanges();
                        }
                    }
                    else
                    {
                        result = ckConfirm;
                        if (outurpchSn.HasValue)
                        {
                            string tableName1 = "";
                            string primaryColumnName1 = "";
                            string primaryColumnValue1 = "";
                            SaveExecConfirmDataModelActionEnum execAction1;
                            string primaryColumnType1 = "";

                            List<SaveExecConfirmDataModel> cecdm2 = new List<SaveExecConfirmDataModel>();


                            //UserNews userNews = db.UserNews.Find(model.sn);
                            tableName1 = "UserNews";
                            primaryColumnName1 = "sn";
                            primaryColumnValue1 = userNews.sn.ToString();
                            execAction1 = SaveExecConfirmDataModelActionEnum.Remove;
                            primaryColumnType1 = "int";
                            List<string> cColumnEmtry = new List<string>();
                            if (userNews.UserId == User.Identity.GetUserName())
                            {
                                IEnumerable<UserNewsUrls> unus = db.UserNewsUrls.Where(n => n.UserNewsSn == userNews.sn);

                                foreach (var unu in unus)
                                {
                                    string tableName2 = "UserNewsUrls";
                                    string primaryColumnName2 = "UserNewsUrlsID";
                                    string primaryColumnValue2 = unu.UserNewsUrlsID.ToString();
                                    SaveExecConfirmDataModelActionEnum execAction2 = SaveExecConfirmDataModelActionEnum.Remove;
                                    string primaryColumnType2 = "int";
                                    SaveExecConfirmDataModel cecdm2t = new SaveExecConfirmDataModel
                                    {
                                        urpchSn = outurpchSn.Value,
                                        tableName = tableName2,
                                        primaryColumnName = primaryColumnName2,
                                        primaryColumnValue = primaryColumnValue2,
                                        primaryColumnType = primaryColumnType2,
                                        execAction = execAction2,
                                        cColumnName = cColumnEmtry,
                                        cColumnValue = cColumnEmtry,
                                        cColumnType = cColumnEmtry
                                    };
                                    cecdm2.Add(cecdm2t);
                                }
                            }
                            cm.SaveExecConfirmData(cecdm2);

                            SaveExecConfirmDataModel cecdm1 = new SaveExecConfirmDataModel
                            {
                                urpchSn = outurpchSn.Value,
                                tableName = tableName1,
                                primaryColumnName = primaryColumnName1,
                                primaryColumnValue = primaryColumnValue1,
                                primaryColumnType = primaryColumnType1,
                                execAction = execAction1,
                                cColumnName = cColumnEmtry,
                                cColumnValue = cColumnEmtry,
                                cColumnType = cColumnEmtry

                            };
                            cm.SaveExecConfirmData(cecdm1);
                        }
                    }
                }
                else
                    result = "沒有權限";
            }
            else
            {
                UserNews userNews = db.UserNews.Find(model.sn);
                if (userNews.UserId == User.Identity.GetUserName())
                {
                    bool checkUrls = true;
                    while (checkUrls)
                    {
                        UserNewsUrls unus = db.UserNewsUrls.Where(n => n.UserNewsSn == userNews.sn).FirstOrDefault();
                        if (unus != null)
                            db.UserNewsUrls.Remove(unus);
                        else
                            checkUrls = false;
                    }
                    db.UserNews.Remove(userNews);
                    db.SaveChanges();
                }
            }
            return Content(result);
        }

        public ActionResult GetImageFile()
        {
            string struid = User.Identity.GetUserId();
            AspNetUsers p = db.AspNetUsers.Where(n => n.Id == struid).FirstOrDefault();//dbUser.GetByID(User.Identity.GetUserId());
            if (p.Photo != null && p.Photo.Length > 2)
            {
                byte[] img = p.Photo;
                return File(img, "image/jpg");
            }
            else
            {                
                return File("~/Images/defaultUser.png", "image/png");
            }                
        }

        public JsonResult GetUserPreference()
        {
            UserPreference result = dbUserPreference.GetByID(User.Identity.GetUserName());
            if (result == null)
                result = new UserPreference() { UserId = User.Identity.GetUserName() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SetUserPreference(UserPreference modal)
        {
            modal.UserId = User.Identity.GetUserName();
            if (modal != null){
                if (db.UserPreference.Find(User.Identity.GetUserName()) != null)
                    dbUserPreference.UpdateWithoutNull(modal);
                else
                    dbUserPreference.Create(modal);
            }
        }

        public JsonResult GetUserPreferenceAOrder()
        {
            UserPreferenceAOrder result = dbUserPreferenceAOrder.GetByID(User.Identity.GetUserName());
            if (result == null)
                result = new UserPreferenceAOrder() { UserId = User.Identity.GetUserName() };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public void SetUserPreferenceAOrder(UserPreferenceAOrder modal)
        {
            modal.UserId = User.Identity.GetUserName();
            if (modal != null)
            {
                if (db.UserPreferenceAOrder.Find(User.Identity.GetUserName()) != null)
                    dbUserPreferenceAOrder.UpdateWithoutNull(modal);
                else
                    dbUserPreferenceAOrder.Create(modal);
            }
        }

        public JsonResult GetUserHavntReadNews()
        {
            //IEnumerable<UserNews> userNews = dbUserNews.GetAll().Where(n => n.read == false&&n.UserId==User.Identity.GetUserName()).ToList();
            List<UserNews> userNewss = dbUserNews.GetAll().Where(n => n.read == false && n.UserId == User.Identity.GetUserName()).ToList();
            List<UserNewsViewModel> unvms = new List<UserNewsViewModel>();
            foreach (UserNews uns in userNewss)
            {
                UserNewsViewModel unvm = new UserNewsViewModel
                {
                    UserNews = uns,
                    UserNewsUrlss = db.UserNewsUrls.Where(n => n.UserNewsSn == uns.sn).ToList(),
                    FromUserName = db.AspNetUsers.Where(n => n.UserName == uns.fromUser).Select(n => n.Name).FirstOrDefault()
                };
                unvms.Add(unvm);
            }
            
            return Json(unvms, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult UserHavntReadNewsTopList()
        {
            //IEnumerable<UserNews> userNews = dbUserNews.GetAll().Where(n => n.read == false&&n.UserId==User.Identity.GetUserName()).ToList();
            List<UserNews> userNewss = dbUserNews.GetAll().Where(n => n.read == false && n.UserId == User.Identity.GetUserName()).OrderByDescending(n=>n.time).Take(5).ToList();
            List<UserNewsViewModel> unvms = new List<UserNewsViewModel>();
            foreach (UserNews uns in userNewss)
            {
                UserNewsViewModel unvm = new UserNewsViewModel
                {
                    UserNews = uns,
                    UserNewsUrlss = db.UserNewsUrls.Where(n => n.UserNewsSn == uns.sn).ToList(),
                    FromUserName = db.AspNetUsers.Where(n => n.UserName == uns.fromUser).Select(n => n.Name).FirstOrDefault()
                };
                unvms.Add(unvm);
            }

            return PartialView(unvms);
        }

        public ActionResult DemoNews(string userName)
        {
            string result = "";
            try
            {
                List<UserNewsUrls> demounls = new List<UserNewsUrls>();
                UserNewsUrls unus = new UserNewsUrls
                {
                    UserNewsTitle = "寶碩投資績效系統",
                    UserNewsUrl = Url.Action("Index", "Default", new { area = "" }),
                    UserNewsCSS = "btn btn-warning"
                };
                demounls.Add(unus);
                NewsHub.SendMessageToUser(true, userName, "Demo通知", "向您介紹寶碩投資績效系統網站，詳情如連結：", Url.Action("Index", "Default", new { area = "" }), demounls);
                result = "發送Demo成功";
            }
            catch
            {
                result = "發送Demo失敗";
            }
            return Content(result);
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Default",new { area=""});
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();
                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();
                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }
    }
    #region
    internal class ChallengeResult : HttpUnauthorizedResult
    {
        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }
        public string RedirectUri { get; set; }
        public string UserId { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
            {
                properties.Dictionary["XsrfId"] = UserId;
            }
            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }
    }
    #endregion
}