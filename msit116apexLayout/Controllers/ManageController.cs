using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin;

using msit116apexLayout.Models;
using OtpSharp;
using Base32;
using System.Collections.Generic;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Text;

namespace msit116apexLayout.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        Repository<AspNetUsers> dbUser = new Repository<AspNetUsers>();
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        ConfirmUserPowerMethod cm = new ConfirmUserPowerMethod();

        public ManageController()
        {

        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var user = await UserManager.FindByIdAsync(userId);

            var model = new ManageIndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = user.PhoneNumber,
                TwoFactor = user.TwoFactorEnabled,
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId),
                IsGoogleAuthenticatorEnabled = user.IsGoogleAuthenticatorEnabled
            };

            return View(model);
        }





        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }



        //
        // GET: /Manage/ChangePassword
        public PartialViewResult ChangePassword()
        {
            return PartialView();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);

            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                ViewBag.StatusMessage = "修改密碼成功。";
                //return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                return PartialView(model);
            }
            AddErrors(result);
            return PartialView(model);
        }

        //
        // GET: /Manage/SetPassword
        public PartialViewResult SetPassword()
        {
            return PartialView();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    ViewBag.StatusMessage = "修改密碼成功。";
                    //return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                    return PartialView(model);
                }
                AddErrors(result);
            }

            return PartialView(model);
        }

        public ActionResult ManageAccounts()
        {
            IEnumerable<AspNetUsers> anuie = db.AspNetUsers.ToList();
            return View(anuie);
        }

        [HttpPost]
        public ActionResult ManageAccounts(System.Web.Mvc.FormCollection form)
        {
            string Id = form["Id"].ToString();
            string Email = form["Email"].ToString();
            string Password = form["Password"].ToString();
            string Name = form["Name"].ToString();
            string UserName = form["UserName"].ToString();
            string PhoneNumber = form["PhoneNumber"].ToString();
            string Telephone = form["Telephone"].ToString();
            string BirthDay = form["BirthDay"].ToString();
            string Country = form["Country"].ToString();
            string MailingAddress = form["MailingAddress"].ToString();
            string ResidenceAddress = form["ResidenceAddress"].ToString();
            string Title = form["Title"].ToString();
            AspNetUsers anu = db.AspNetUsers.Where(n => n.Id == Id).First();
            anu.Email = Email;
            anu.Name = Name;
            anu.UserName = UserName;
            anu.PhoneNumber = PhoneNumber;
            anu.Telephone = Telephone;
            anu.BirthDay = Convert.ToDateTime(BirthDay);
            anu.Country = Country;
            anu.MailingAddress = MailingAddress;
            anu.ResidenceAddress = ResidenceAddress;
            anu.Title = Title;
            db.SaveChanges();
            string strResult = "";
            try
            {
                
                UserManager.RemovePassword(Id);

                UserManager.AddPassword(Id, Password);

                //ApplicationDbContext context = new ApplicationDbContext();
                //UserStore<ApplicationUser> store = new UserStore<ApplicationUser>(context);
                //UserManager<ApplicationUser> UserManagerTmp = new UserManager<ApplicationUser>(store);
                //String userId = Id;//"<YourLogicAssignsRequestedUserId>";
                //String newPassword = Password; //"<PasswordAsTypedByUser>";
                //String hashedNewPassword = UserManagerTmp.PasswordHasher.HashPassword(newPassword);
                //ApplicationUser cUser = await store.FindByIdAsync(userId);
                //await store.SetPasswordHashAsync(cUser, hashedNewPassword);
                //await store.UpdateAsync(cUser);
                strResult = "完成";
            }
            catch (Exception ex)
            {
                strResult = "失敗"+ ex;
            }

            return Content(strResult);
        }

        public ActionResult GetImageFile(string userid)
        {
            AspNetUsers p = db.AspNetUsers.Where(n => n.Id == userid).FirstOrDefault();//dbUser.GetByID(userid);
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

        public ActionResult GetConfirmImageFile(int pid)
        {
            ConfirmPhoto p = db.ConfirmPhoto.Where(n => n.Id == pid).FirstOrDefault();
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

        [HttpPost]
        public ActionResult UpdateUserImageFile(System.Web.Mvc.FormCollection form, HttpPostedFileBase Photo)
        {
            string Id = form["Id"].ToString();
            AspNetUsers anu = db.AspNetUsers.Where(n=>n.Id== Id).First();
            if (Photo != null)
            {
                byte[] photobytes = new byte[Photo.InputStream.Length];
                Photo.InputStream.Read(photobytes, 0, photobytes.Length);
                anu.Photo = photobytes;
                db.SaveChanges();
                
                return Content("更新照片成功");
            }
            else
            {
                return Content("更新照片失敗");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

        public PartialViewResult ChangeUserPersonData()
        {
            //HELP註解 取得登入使用者ID
            ViewBag.LogUser = User.Identity.GetUserName();
            //HELP註解 取得登入使用者資料
            //AspNetUsers userdata= db.AspNetUsers.Find(User.Identity.GetUserId());            
            var userdata = db.ProcedureGetUserData(User.Identity.GetUserId()).First();
            //HELP註解 判斷登入使用者是否為員工
            if (db.IsEmployee.Find(User.Identity.GetUserName()) != null)
                ViewBag.AccountType = "員工";
            else
                ViewBag.AccountType = "會員";
            //HELP註解 會員資料表 db.AspNetUsers
            return PartialView(userdata);
        }

        [HttpPost]
        public PartialViewResult ChangeUserPersonData(ProcedureGetUserData_Result modal, HttpPostedFileBase Photo)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(modal);
            }

            AspNetUsers user = db.AspNetUsers.Find(User.Identity.GetUserId());

            user.Country = modal.Country;
            user.MailingAddress = modal.MailingAddress;
            user.PhoneNumber = modal.PhoneNumber;
            user.Telephone = modal.Telephone;
            string result = "";
            if (Photo != null)
            {
                //檢查員工
                if (cm.checkIsEmployee(User.Identity.GetUserName()))
                {
                    //檢查權限更新頭像的權限ID為1
                    if (cm.checkHasPower(User.Identity.GetUserName(), 1))
                    {
                        byte[] Tmpphotobytes = new byte[Photo.InputStream.Length];
                        Photo.InputStream.Read(Tmpphotobytes, 0, Tmpphotobytes.Length);
                        ConfirmPhoto cp = new ConfirmPhoto
                        {
                            Photo = Tmpphotobytes
                        };
                        db.ConfirmPhoto.Add(cp);
                        db.SaveChanges();

                        //檢查覆核                        
                        string userUserName = User.Identity.GetUserName();
                        string userName = User.Identity.Name;
                        string confirmDescription = "";
                        confirmDescription += "<div style='border:1px solid black;margin: 2px'>";
                        confirmDescription += "<p>欲更新頭像：</p>";
                        confirmDescription += "<div style='border:1px solid black;margin: 2px'>";
                        confirmDescription += "<img src='" + Url.Action("GetConfirmImageFile", "Manage", new { area = "" }) + "?pid=" + cp.Id + "' style='height:200px;width:200px;border-radius:50%'/>";

                        confirmDescription += "</div>";
                        confirmDescription += "</div>";
                        int? outurpchSn;
                        string ckConfirm = cm.checkNeedConfirm(out outurpchSn, User.Identity.GetUserName(), 1, Url.Action("UserConfirmUserPower", "ConfirmUserPower", new { area = "" }), confirmDescription);
                        //string ckConfirm = "";
                        if (ckConfirm == "")
                        {
                            //byte[] photobytes = new byte[Photo.InputStream.Length];
                            //Photo.InputStream.Read(photobytes, 0, photobytes.Length);
                            user.Photo = Tmpphotobytes;
                        }
                        else
                        {
                            result = ckConfirm;
                            //string base64 = Convert.ToBase64String(bytes);
                            //byte[] bytes = Convert.FromBase64String(base64);
                            if (outurpchSn.HasValue)
                            {
                                //byte[] photobytes = new byte[Photo.InputStream.Length];
                                //Photo.InputStream.Read(photobytes, 0, photobytes.Length);
                                //string PhotoByteStr = Convert.ToBase64String(photobytes);
                                StringBuilder PhotoByteStr = new StringBuilder();
                                foreach (byte pbtmp in Tmpphotobytes)
                                {
                                    StringBuilder ppptmp = new StringBuilder();
                                    ppptmp.Append(pbtmp);
                                    ppptmp.Append(",");
                                    PhotoByteStr.Append(ppptmp.ToString());
                                }

                                string tableName1 = "";
                                string primaryColumnName1 = "";
                                string primaryColumnValue1 = "";
                                SaveExecConfirmDataModelActionEnum execAction1;
                                string primaryColumnType1 = "";

                                List<SaveExecConfirmDataModel> cecdm2 = new List<SaveExecConfirmDataModel>();

                                tableName1 = "AspNetUsers";
                                primaryColumnName1 = "Id";
                                primaryColumnValue1 = User.Identity.GetUserId();
                                execAction1 = SaveExecConfirmDataModelActionEnum.Update;
                                primaryColumnType1 = "string";
                                List<string> cColumnName = new List<string>();
                                List<string> cColumnValue = new List<string>();
                                List<string> cColumnType = new List<string>();
                                cColumnName.Add("Photo");
                                cColumnValue.Add(PhotoByteStr.ToString());
                                cColumnType.Add("Byte[]");

                                SaveExecConfirmDataModel cecdm1 = new SaveExecConfirmDataModel
                                {
                                    urpchSn = outurpchSn.Value,
                                    tableName = tableName1,
                                    primaryColumnName = primaryColumnName1,
                                    primaryColumnValue = primaryColumnValue1,
                                    primaryColumnType = primaryColumnType1,
                                    execAction = execAction1,
                                    cColumnName = cColumnName,
                                    cColumnValue = cColumnValue,
                                    cColumnType = cColumnType

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
                    byte[] photobytes = new byte[Photo.InputStream.Length];
                    Photo.InputStream.Read(photobytes, 0, photobytes.Length);
                    user.Photo = photobytes;
                }

            }
            db.SaveChanges();
            //dbUser.UpdateWithoutNull(user);
            if (db.IsEmployee.Find(User.Identity.GetUserName()) != null)
                ViewBag.AccountType = "員工";
            else
                ViewBag.AccountType = "會員";
            var userdata = db.ProcedureGetUserData(User.Identity.GetUserId()).First();
            if (result != "")
                TempData["AlertConfirm"] = result;

            return PartialView(userdata);
        }

        public async Task<ActionResult> DisableGoogleAuthenticator()
        {
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                user.IsGoogleAuthenticatorEnabled = false;
                user.GoogleAuthenticatorSecretKey = null;
                user.TwoFactorEnabled = false;

                await UserManager.UpdateAsync(user);
            }
            return RedirectToAction("Index", "Manage");
        }

        [HttpGet]
        public ActionResult EnableGoogleAuthenticator()
        {
            byte[] secretKey = KeyGeneration.GenerateRandomKey(20);
            string userName = User.Identity.GetUserName();
            string issuer = "MSIT116寶碩投資績效系統";
            string issuerEncoded = HttpUtility.UrlEncode(issuer);
            string barcodeUrl = KeyUrl.GetTotpUrl(secretKey, userName) + "&issuer=" + issuerEncoded;

            var model = new GoogleAuthenticatorViewModel
            {
                SecretKey = Base32Encoder.Encode(secretKey),
                BarcodeUrl = barcodeUrl
            };

            return View(model);
        }


        [HttpPost]
        public async Task<ActionResult> EnableGoogleAuthenticator(GoogleAuthenticatorViewModel model)
        {
            if (ModelState.IsValid)
            {
                byte[] secretKey = Base32Encoder.Decode(model.SecretKey);

                long timeStepMatched = 0;
                var otp = new Totp(secretKey);
                if (model.Code != null && otp.VerifyTotp(model.Code.Trim(), out timeStepMatched, new VerificationWindow(2, 2)))
                {
                    var user = UserManager.FindById(User.Identity.GetUserId());
                    user.IsGoogleAuthenticatorEnabled = true;
                    user.GoogleAuthenticatorSecretKey = model.SecretKey;
                    user.TwoFactorEnabled = true;
                    await UserManager.UpdateAsync(user);

                    return RedirectToAction("Index", "Manage");
                }
                else
                {
                    ModelState.AddModelError("Code", "代碼錯誤");
                }
            }

            return View(model);

        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

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

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }


}