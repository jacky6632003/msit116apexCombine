using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using msit116apexLayout.Models;
using Microsoft.AspNet.Identity;
using msit116apexLayout.Extensions;

namespace msit116apexLayout.Controllers
{
    public class ConfirmUserPowerController : Controller
    {
        MSIT116APEXEntities db = new MSIT116APEXEntities();
        public ActionResult UserConfirmUserPower(string needConfirmUserID,int? needConfirmPowerID,int? ConfirmResult,int? userNewsSn)
        {
            if (needConfirmUserID != "" && needConfirmPowerID.HasValue && ConfirmResult.HasValue && userNewsSn.HasValue)
            {
                ConfirmUserPowerMethod CUPM = new ConfirmUserPowerMethod();
                int? returnConfirmEndurpchSn = CUPM.UserConfirmUserPowerM(User.Identity.GetUserName(), needConfirmUserID, needConfirmPowerID.Value, ConfirmResult.Value);

                //將通知轉為核准/否決並轉到已讀
                UserNews un = db.UserNews.Where(n => n.sn == userNewsSn.Value).FirstOrDefault();
                IEnumerable<UserNewsUrls> unuie = db.UserNewsUrls.Where(n => n.UserNewsSn == userNewsSn.Value);
                un.read = true;
                string strCResult = ConfirmResult.Value == 1 ? "核准" : "否決";
                un.msgUrl = "";
                //un.msgContent += "<br/><button class='btn btn-info' disabled>" + strCResult + "</button>";
                foreach (var unu in unuie)
                {
                    db.UserNewsUrls.Remove(unu);
                }
                UserNewsUrls resultunus = new UserNewsUrls {
                    UserNewsSn= un.sn,
                    UserNewsCSS= "btn btn-info disabled",
                    UserNewsTitle= strCResult,
                    UserNewsUrl=""                    
                };
                db.UserNewsUrls.Add(resultunus);

                db.SaveChanges();

                if(returnConfirmEndurpchSn.HasValue)
                {
                    CUPM.ExecConfirmIsEnd(returnConfirmEndurpchSn.Value);
                }
            }
            //TODO
            return RedirectToAction("UserMessage", "Account", new { area = "" });
        }

    }
}