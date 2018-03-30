using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace AppMyRPG.Controllers
{
    public class EmailController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Email
        public ActionResult SendMail(int userid)
        {
            //查找用户是否验证
            var user = db.UserInfo.First(m => m.UserID == userid);
            if (user == null)
            {
                return HttpNotFound();
            }
            if (user.UserRegState == 0)
            {
                string uid = user.UserUID;
                string validataCode = Guid.NewGuid().ToString();

                //填写邮件地址和显示名称
                MailAddress from = new MailAddress("Your email","MyRPG站长");
                //填写收件人地址和名称
                MailAddress to = new MailAddress(user.UserEmail);

                MailMessage mail = new MailMessage();
                mail.From = from;
                mail.To.Add(to);
                mail.Subject = "MyRPG个人资源站";

                StringBuilder strBody = new StringBuilder();
                strBody.Append("点击链接激活账号，48小时有效，链接只能使用一次哦~成功激活，将给予进入里站的权限！<br/>");
                strBody.AppendFormat("<a href='http://game.tomonori.cc/Email/ActivePage?userid={0}&vaildataCode={1}'>Click This</a><br/>", user.UserID, validataCode);
                mail.Body = strBody.ToString();
                mail.IsBodyHtml = true;       //设置显示htmls
                //设置邮件服务地址
                try
                {
                    using (var smtp = new SmtpClient())
                    {
                        //填写服务器地址相关的用户名和密码
                        var credential = new NetworkCredential
                        {
                            UserName = "Your EMail",
                            Password = "Your Password"
                        };
                        smtp.Credentials = credential;
                        smtp.Host = "smtp-mail.outlook.com";
                        smtp.Port = 587;
                        smtp.EnableSsl = true;
                        smtp.Send(mail);

                        return RedirectToAction("SendSuccess");
                    }
                }
                catch
                {
                    throw;
                }
            }
            return RedirectToAction("Detail", "UserInfo", new { UserID = userid });
        }

        //发送忘记密码确认邮件
        public ActionResult SendForgotEmail(string useruid,string adress)
        {
            //查找用户是否验证设置邮箱
            string UserEmail = adress; 
            var user = db.UserInfo.First(m => m.UserUID == useruid);
            if (user == null)
            {
                return HttpNotFound();
            }

            string uid = user.UserUID;
            string validataCode = Guid.NewGuid().ToString();

            //填写邮件地址和显示名称
            MailAddress from = new MailAddress("gutrse3321@live.com", "MyRPG站长");
            //填写收件人地址和名称
            MailAddress to = new MailAddress(UserEmail);

            MailMessage mail = new MailMessage();
            mail.From = from;
            mail.To.Add(to);
            mail.Subject = "MyRPG个人资源站 - 找回密码";

            StringBuilder strBody = new StringBuilder();
            strBody.Append("找回你的密码，48小时有效，链接只能使用一次哦~<br/>");
            strBody.AppendFormat("<a href='http://game.tomonori.cc/Email/NewPassword?useruid={0}&vaildataCode={1}'>Click This</a><br/>", uid, validataCode);
            mail.Body = strBody.ToString();
            mail.IsBodyHtml = true;       //设置显示htmls
            //设置邮件服务地址
            try
            {
                using (var smtp = new SmtpClient())
                {
                    //填写服务器地址相关的用户名和密码
                    var credential = new NetworkCredential
                    {
                        UserName = "gutrse3321@live.com",
                        Password = "123357789zzs"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp-mail.outlook.com";
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Send(mail);

                    return RedirectToAction("SendSuccess");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //发送成功页面
        public ActionResult SendSuccess()
        {
            return View();
        }

        //接收激活邮件动作
        public ActionResult ActivePage(int userid)
        {
            //查询接收的userid
            var user = db.UserInfo.First(m => m.UserID == userid);
            if (user != null)
            {
                user.UserRegState = 1;
                user.GroupID = 3;
                db.SaveChanges();

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return HttpNotFound();
            }
        }

        //接收找回密码动作
        public ActionResult NewPassword(string useruid)
        {
            var user = db.UserInfo.First(m => m.UserUID == useruid);

            return View(user);
        }

        [HttpPost]
        public ActionResult NewPassword(UserInfo userinfo)
        {
            var user = db.UserInfo.First(m => m.UserUID == userinfo.UserUID);
            //密码加密
            string md5Pwd = MySec.GetMD5(userinfo.UserPWD);
            //更改密码
            user.UserPWD = md5Pwd;
            db.SaveChanges();

            return RedirectToAction("LoginHolder", "UserInfo");
        }
    }
}
