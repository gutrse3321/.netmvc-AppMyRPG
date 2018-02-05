using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AppMyRPG.Models;
using PagedList;
using System.Data.Entity;
using System.Net;

namespace AppMyRPG.Areas.Admin.Controllers
{
    [Authorize(Users = "1,6")]
    public class NoticeController : Controller
    {
        DB_MyRPGEntities db = new DB_MyRPGEntities();
        // GET: Admin/Notice
        public ActionResult Article(string order,string currentFilter, string selectString,int? page)
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
            var list = db.Notice.Include(m=>m.UserInfo).OrderByDescending(m => m.NoticeID);
            if (!string.IsNullOrEmpty(selectString))
            {
                list = list.Where(
                    m => m.NoticeTitle.Contains(selectString) 
                    || m.UserInfo.UserUID.Contains(selectString))
                    .OrderByDescending(m=>m.NoticeID);
            }
            //排序
            switch (order)
            {
                case "time":list = list.OrderBy(m => m.NoticeTime);break;
                case "timedesc":list = list.OrderByDescending(m => m.NoticeTime);break;
                case "useruid":list = list.OrderByDescending(m => m.UserInfo.UserUID);break;
                default: list = list.OrderByDescending(m => m.NoticeID);break;
            }
            //分页
            int pageSize = 10;
            //若page为null 则pageNumber=1
            int pageNumber = (page ?? 1);

            return View(list.ToPagedList(pageNumber,pageSize));
        }

        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(Notice notice)
        {
            //根据session查询用户ID
            string sessionUid = Session["UID"].ToString();
            var user = db.UserInfo.First(m => m.UserUID == sessionUid);
            //发布者暂时为admin
            notice.UserID = user.UserID;
            notice.NoticeTime = DateTime.Now;
            db.Notice.Add(notice);
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Edit(int? noticeid)
        {
            //验证获取的noticeid是否为空，空的返回错误页面
            if (noticeid == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //验证是否查找到noticeid
            var notice = db.Notice.First(m => m.NoticeID == noticeid);
            if (notice == null)
            {
                return HttpNotFound();
            }

            return View(notice);
        }

        [HttpPost]
        public ActionResult Edit(Notice notice)
        {
            //前台验证通过执行修改
            //Entry获取信息及操作
            var enotice = db.Notice.First(m => m.NoticeID==notice.NoticeID);
            enotice.NoticeTime = DateTime.Now;
            enotice.NoticeContent = notice.NoticeContent;
            enotice.NoticeTitle = notice.NoticeTitle;
            db.SaveChanges();

            return RedirectToAction("Article");
        }

        public ActionResult Delete(int noticeid)
        {
            //验证重复提交
            var notice = db.Notice.Find(noticeid);
            if (notice == null)
            {
                return HttpNotFound();
            }
            db.Notice.Remove(notice);
            db.SaveChanges();

            return RedirectToAction("Article");
        }
    }
}