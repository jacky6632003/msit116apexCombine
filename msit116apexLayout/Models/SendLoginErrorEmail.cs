using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace msit116apexLayout.Models
{
    public class LoginErrorEmail
    {
        public static void SendEmail(string toUser, HttpBrowserCapabilitiesBase userBrowser)
        {
            MailMessage msg = new MailMessage();
            //收件人
            msg.To.Add(toUser);
            //寄件人 顯示名稱 顯示名稱編碼
            msg.From = new MailAddress("msit116apex@gmail.com", "MSIT116寶碩投資績效系統", System.Text.Encoding.UTF8);
            //郵件標題 
            msg.Subject = "[登入失敗]";
            //郵件標題編碼  
            msg.SubjectEncoding = System.Text.Encoding.UTF8;
            //郵件內容            
            string userDevice = "";
            if (userBrowser.IsMobileDevice)
                userDevice = userBrowser.MobileDeviceModel;
            else
                userDevice = userBrowser.Platform;

            string msgUser = "<br>使用者[" + toUser + "]於 " + DateTime.Now.ToString() + "登入失敗。";
            string msgBrowser = "<br>使用瀏覽器：" + userBrowser.Browser;
            string msgDevice = "<br>在裝置：" + userDevice ;
            msg.Body = "[登入失敗]：" + msgUser+ msgBrowser+ msgDevice;
            //使用html格式
            msg.IsBodyHtml = true;
            //郵件內容編碼 
            msg.BodyEncoding = System.Text.Encoding.UTF8;
            //郵件優先級 
            msg.Priority = MailPriority.Normal;

            //建立 SmtpClient 物件 並設定 Gmail的smtp主機及Port 
            SmtpClient MySmtp = new SmtpClient("smtp.gmail.com", 587);
            
            //設定帳號密碼
            MySmtp.Credentials = new System.Net.NetworkCredential("msit116apex@gmail.com", "P@ssw0rdmsit116apex");
            //Gmial 的 smtp 使用 SSL
            MySmtp.EnableSsl = true;
            MySmtp.Send(msg);
        }
    }
}