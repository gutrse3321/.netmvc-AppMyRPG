using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using AppMyRPG.Models;
using PagedList;
using System.IO;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1")]
    public class UserController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Admin/User
        public ActionResult Article(string order, string currentFilter,string selectString,int? page)
        {
            //保持排序顺序在分页时相同
            ViewBag.CurrentSort = order;
            //确保正确分页
            if (selectString != null)
            {
                page = 1;
            }
            else
            {
                selectString = currentFilter;
            }
            //设置搜索信息
            ViewBag.CurrentFilter = selectString;
            var list = db.UserInfo.OrderByDescending(m=>m.UserID);

            //搜索功能实现  可是根据昵称搜索或是用户名
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list
                    .Where(
                    m => m.UserNickname.Contains(selectString)
                    || m.UserUID.Contains(selectString)
                    || m.UserGroup.GroupName.Contains(selectString))
                    .OrderByDescending(m => m.UserID);
            }
            //排序功能实现
            switch (order)
            {
                case "time":
                    list = list.OrderBy(m => m.UserRegTime);break;
                case "timedesc":
                    list = list.OrderByDescending(m => m.UserRegTime);break;
                case "usertype":
                    list = list.OrderBy(m => m.GroupID);break;
                case "state":
                    list = list.OrderBy(m => m.UserRegState);break;
                default:list = list.OrderByDescending(m => m.UserID);break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            //绑定下拉框数据
            ViewBag.GroupID = new SelectList(db.UserGroup, "GroupID", "GroupName");
            return View();
        }

        [HttpPost]
        public ActionResult Add(UserInfo userinfo,HttpPostedFileBase UserFace,int RegState)
        {
            //查询是否存在相同用户名UserUID
            var uid = db.UserInfo.Where(m => m.UserUID == userinfo.UserUID);
            if (uid.Count() > 0)
            {
                ModelState.AddModelError("UserUID", "用户名已被使用");
                //重新绑定下拉框数据
                ViewBag.GroupID = new SelectList(db.UserGroup, "GroupID", "GroupName",userinfo.GroupID);
                return View(userinfo);
            }
            //密码md5加密
            string md5PWD = MySec.GetMD5(userinfo.UserPWD);
            //上传头像
            string facename = string.Empty;
            if (UserFace != null)
            {
                //默认强转.jpg格式
                facename = string.Format("user_{0}.jpg", Guid.NewGuid().ToString("N"));
                var facepath = Path.Combine(Server.MapPath("~/Images/face/User"), facename);
                UserFace.SaveAs(facepath);
            }
            //声明存入数据库头像字段
            string face = string.Format("/Images/face/User/{0}", facename);
            //前端写入数据库
            userinfo.UserPWD = md5PWD;
            userinfo.UserFace = face;
            userinfo.UserRegState = RegState;
            userinfo.UserRegTime = DateTime.Now;
            db.UserInfo.Add(userinfo);
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Edit(int? userid)
        {
            if (userid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var list = db.UserInfo.First(m => m.UserID == userid);
            if (list == null)
            {
                return HttpNotFound();
            }
            ViewBag.GroupID = new SelectList(db.UserGroup, "GroupID", "GroupName", list.GroupID);

            return View(list);
        }

        [HttpPost]
        public ActionResult Edit(UserInfo userinfo, HttpPostedFileBase UserFace, int RegState)
        {
            UserInfo u = db.UserInfo.First(m => m.UserID == userinfo.UserID);
            //上传头像
            string facename = string.Empty;
            if (UserFace != null)
            {
                //从数据库取出，去地址取名字
                facename = u.UserFace.Replace("/Images/face/User/", "");
                var facepath = Path.Combine(Server.MapPath("~/Images/face/User"), facename);
                UserFace.SaveAs(facepath);
            }
            //重新组合头像字段存入数据库
            string face = string.Format("/Images/face/Game/{0}", facename);
            u.UserNickname = userinfo.UserNickname;
            u.UserFace = string.IsNullOrEmpty(facename) ? u.UserFace : face;
            u.UserEmail = userinfo.UserEmail;
            u.UserRegState = RegState;
            u.GroupID = userinfo.GroupID;
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Delete(int userid)
        {
            //重复提交验证
            var userinfo = db.UserInfo.Find(userid);
            if (userinfo == null)
            {
                return HttpNotFound();
            }
            //查询是否有此用户的评论
            var usercmt = db.Comment.Where(m => m.UserID == userid);
            if (usercmt.Count() > 0) {
                db.Comment.RemoveRange(usercmt);
            }
            db.UserInfo.Remove(userinfo);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}