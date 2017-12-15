using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.AspNet.Identity;
using System.Threading.Tasks;
using OtpSharp;
using Base32;
using System.Net.Mail;

namespace msit116apexLayout
{
    public class Emailer
    {
        const string replyToEmail = "msit116apex@gmail.com";
        public void sendEmail(string email, string subject, string body)
        {
            //MailMessage mail = new MailMessage("donotreply@yourcompany.com", email,subject, body);
            //mail.IsBodyHtml = true;

            //SmtpClient client = new SmtpClient();

            //client.Port = 25;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.Host = "localhost";

            //client.Send(mail);
            MailMessage mail = new MailMessage(
                replyToEmail,
                email,
                subject,
                body);
            mail.From = new MailAddress(replyToEmail, "MSIT116寶碩投資績效系統", System.Text.Encoding.UTF8);
            
            mail.IsBodyHtml = true;

            SmtpClient client = new SmtpClient();

            client.Port = 587;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Host = "smtp.gmail.com";
            //設定帳號密碼
            client.Credentials = new System.Net.NetworkCredential("msit116apex@gmail.com", "P@ssw0rdmsit116apex");
            //Gmial 的 smtp 使用 SSL
            client.EnableSsl = true;

            client.Send(mail);
        }
    }
}