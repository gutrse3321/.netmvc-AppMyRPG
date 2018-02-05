using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using System.Web.Security;
using System.IO;
using System.Net;

namespace AppMyRPG.Controllers
{
    public class UserInfoController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: UserInfo
        public ActionResult Info()
        {
            return View();
        }

        //登陆
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult LoginHolder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginHolder(UserInfo u)
        {
            if (ModelState.IsValid)
            {
                var pwd = MySec.GetMD5(u.UserPWD);
                int c = db.UserInfo.Count(m => m.UserUID.Equals(u.UserUID) && m.UserPWD.Equals(pwd));
                if (c > 0)
                {
                    //查询用户的GroupID
                    var user = db.UserInfo.First(m => m.UserUID == u.UserUID);

                    Session["UID"] = u.UserUID;
                    Session["GID"] = user.GroupID.ToString();
                    FormsAuthentication.SetAuthCookie(user.GroupID.ToString(), false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("UserPWD", "用户名或密码错误~");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }

        //注册
        public ActionResult RegisterHolder()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegisterHolder(UserInfo userinfo, HttpPostedFileBase UserFace)
        {
            //查询是否有同名用户名UID
            var uid = db.UserInfo.Where(m => m.UserUID == userinfo.UserUID);
            if (uid.Count() > 0)
            {
                ModelState.AddModelError("UserUID", "用户名已被使用");

                return View(userinfo);
            }
            //查询是否有同名邮箱
            var email = db.UserInfo.Where(m => m.UserEmail == userinfo.UserEmail);
            if (email.Count() > 0)
            {
                ModelState.AddModelError("UserEmail", "邮箱已被使用");

                return View(userinfo);
            }
            //密码md5加密
            string md5Pwd = MySec.GetMD5(userinfo.UserPWD);
            //上传头像
            string facename = string.Empty;
            if (UserFace != null)
            {
                //默认强转.jpg格式
                facename = string.Format("user_{0}.jpg", Guid.NewGuid().ToString("N"));
                var facepath = Path.Combine(Server.MapPath("~/Images/face/User"), facename);
                UserFace.SaveAs(facepath);
            }
            //声明存入数据库头像的字段
            string face = string.Format("/Images/face/User/{0}", facename);
            //验证通过写入数据库
            if (ModelState.IsValid)
            {
                userinfo.UserNickname = userinfo.UserUID;
                userinfo.UserPWD = md5Pwd;
                userinfo.UserFace = face;
                userinfo.UserRegState = 0;
                userinfo.UserRegTime = DateTime.Now;
                userinfo.GroupID = 2;
                db.UserInfo.Add(userinfo);
                db.SaveChanges();

                return RedirectToAction("LoginHolder", "UserInfo");
            }

            return View(userinfo);
        }

        //已登录
        public ActionResult LoggedIn()
        {
            string sessionUid = Session["UID"].ToString();
            var user = db.UserInfo.First(m => m.UserUID == sessionUid);

            return View(user);
        }

        //个人中心
        public ActionResult Detail(int UserID)
        {
            var user = db.UserInfo.First(m => m.UserID == UserID);

            return View(user);
        }

        //修改用户信息
        public ActionResult Edit(int? userid)
        {
            if (userid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.UserInfo.First(m => m.UserID == userid);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(UserInfo userinfo, HttpPostedFileBase UserFace)
        {
            var user = db.UserInfo.First(m => m.UserID == userinfo.UserID);
            //上传头像
            string facename = string.Empty;
            if (UserFace != null)
            {
                //从数据库取出，去掉地址取得名字
                facename = user.UserFace.Replace("/Images/face/User/", "");
                var facepath = Path.Combine(Server.MapPath("~/Images/face/User"), facename);
                UserFace.SaveAs(facepath);
            }
            //重新组合头像字段
            string face = string.Format("/Images/face/User/{0}", facename);
            user.UserNickname = userinfo.UserNickname;
            user.UserFace = string.IsNullOrEmpty(facename) ? user.UserFace : face;
            user.UserEmail = userinfo.UserEmail;
            db.SaveChanges();

            return RedirectToAction("Detail", "UserInfo", new { UserID = userinfo.UserID });
        }

        //用户修改密码
        public ActionResult PwdEdit(int? userid)
        {
            
            if (userid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var user = db.UserInfo.First(m => m.UserID == userid);
            if (user.UserUID == Session["UID"].ToString())
            {
                if (user == null)
                {
                    return HttpNotFound();
                }

                return View(user);
            }

            return RedirectToAction("Detail", "UserInfo", user.UserUID);
        }

        [HttpPost]
        public ActionResult PwdEdit(UserInfo userinfo)
        {
            //查询用户
            var user = db.UserInfo.First(m => m.UserID == userinfo.UserID);
            //密码md5加密
            string md5 = MySec.GetMD5(userinfo.UserPWD);

            user.UserPWD = md5;
            db.SaveChanges();

            return RedirectToAction("LoginHolder", "UserInfo");
        }

        //忘记密码？
        public ActionResult ForgotPwd()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPwd(ForgotPwdViewModel fpv)
        {
            if (ModelState.IsValid)
            {
                //查找是否存在用户
                var user = db.UserInfo.Where(m => m.UserUID == fpv.UserUID);
                if (user.Count() > 0)
                {
                    var ch = db.UserInfo.First(m => m.UserUID == fpv.UserUID);
                    //验证邮箱是否正确
                    if (ch.UserEmail == fpv.UserEmail)
                    {
                        return RedirectToAction("SendForgotEmail", "Email", new { useruid = fpv.UserUID, adress = fpv.UserEmail });
                    }
                    else
                    {
                        ModelState.AddModelError("UserEmail", "邮箱与注册时不相同");
                    }
                }
                else
                {
                    ModelState.AddModelError("UserUID", "用户不存在哦");
                    return View(fpv);
                }
            }

            return View(fpv);
        }

        //登出
        public ActionResult SignOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}